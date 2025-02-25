using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;

namespace iFrames.MutualFundIndia
{
	public partial class Dividend : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				getFundHouse();
                LoadNature();
                LoadAllSubNature();
                getSchemesList();
                txtfromDate.Text = DateTime.Today.AddDays(-8).ToString("dd MMM yyyy");
                txtToDate.Text= DateTime.Today.AddDays(-2).ToString("dd MMM yyyy");
            }
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			getDividend();
			dpDividend.SetPageProperties(0, dpDividend.MaximumRows, true);

		}
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("/MutualFundIndia/Dividend.aspx");
            
        }

        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (ddlFundHouse.SelectedIndex != 0)
                getSchemesList();

            else
            {
                listboxSchemeName.Items.Clear();
                listboxSchemeName.Items.Add(new ListItem("Select", "-1"));
            }
            
            //DataTable dtScheme = new DataTable();

            //dtScheme = AllMethods.getSebiScheme(Convert.ToInt32(ddlCategory.SelectedItem.Value),
            //           -1, Convert.ToInt32(ddlSubCategory.SelectedValue),
            //           -1, Convert.ToInt32(ddlFundHouse.SelectedValue), false);

            //listboxSchemeName.Items.Clear();
            //if (dtScheme != null)
            //{
            //    DataRow drSch = dtScheme.NewRow();
            //    drSch["Sch_Short_Name"] = "Select";
            //    drSch["Scheme_Id"] = 0;
            //    dtScheme.Rows.InsertAt(drSch, 0);

            //    listboxSchemeName.DataSource = dtScheme;
            //    listboxSchemeName.DataTextField = "Sch_Short_Name";
            //    listboxSchemeName.DataValueField = "Scheme_Id";
            //    listboxSchemeName.DataBind();
            //}
            //else
            //{
            //    listboxSchemeName.Items.Add(new ListItem("Select", "-1"));
            //}
        }
        protected void listboxSchemeName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
            //if (ddlCategory.SelectedIndex == 0) return;
            int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            LoadSubNature(nature_id);

            if (ddlFundHouse.SelectedIndex != 0)
                getSchemesList();//ddlFundHouse.SelectedItem.Value
           
            //int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            //LoadSubNature(nature_id);
        }

		protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
		{
			dpDividend.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
			getDividend();
		}

		private void getFundHouse()
		{
            DataTable dtFund;
            if (Cache["dtFund"] == null)
            {
                dtFund = AllMethods.getFundHouse();
                DataRow drFund = dtFund.NewRow();
                drFund["MutualFund_Name"] = "Select";
                drFund["MutualFund_ID"] = 0;
                dtFund.Rows.InsertAt(drFund, 0);
                Cache.Add("dtFund", dtFund, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtFund = (DataTable)Cache["dtFund"];
            }
			ddlFundHouse.DataSource = dtFund;
			ddlFundHouse.DataTextField = "MutualFund_Name";
			ddlFundHouse.DataValueField = "MutualFund_ID";
			ddlFundHouse.DataBind();
		}

		private void getSchemesList(string fundId)
		{
			DataTable dtScheme = AllMethods.getSchemeOption(false,"3", fundId);
            listboxSchemeName.Items.Clear();
            if (dtScheme != null && dtScheme.Rows.Count>0)
            {
                listboxSchemeName.DataSource = dtScheme;
                listboxSchemeName.DataTextField = "Sch_Short_Name";
                listboxSchemeName.DataValueField = "Scheme_Id";
                listboxSchemeName.DataBind();
            }
            else
            {
                listboxSchemeName.Items.Add(new ListItem("Select", "-1"));
            }
		}

		private void getDividend()
		{
			DataTable _dt = null;
			string SchmeId = GetSelectedScheme();
			DateTime frmDate, toDate;
			if (txtfromDate.Text != string.Empty && txtToDate.Text != string.Empty)
			{
				frmDate = Convert.ToDateTime(txtfromDate.Text);
				toDate = Convert.ToDateTime(txtToDate.Text);
			}
			else
			{
				frmDate = DateTime.Today.AddDays(-8);
				toDate = DateTime.Today.AddDays(-2);
			}

			try
			{
				_dt = AllMethods.getDividendDetails(SchmeId, frmDate, toDate);


				if (_dt != null && _dt.Rows.Count > 0)
				{
					_dt = _dt.OrderBy("Record_Date desc");
                    //listvwDividendDetail.DataSource = _dt;
                    //listvwDividendDetail.DataBind();
                }
                listvwDividendDetail.DataSource = _dt;
                listvwDividendDetail.DataBind();

                if (_dt == null)
                    resetDp();
                else
                {
                    dpDividend.Visible = true;
                    listvwDividendDetail.Visible = true;
                    lblErrMsg.Text = "";
                    lblErrMsg.Visible = false;
                }
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void resetDp()
		{

			//var dpCurrentPageLabel = dpDividend.Controls[0].FindControl("CurrentPageLabel") as Label;
			//if (dpCurrentPageLabel != null) dpCurrentPageLabel.Text = "0";

			//var dpTotalPagesLabel = dpDividend.Controls[0].FindControl("TotalPagesLabel") as Label;
			//if (dpTotalPagesLabel != null) dpTotalPagesLabel.Text = "0";

			//var dpTotalItemsLabel = dpDividend.Controls[0].FindControl("TotalItemsLabel") as Label;
			//if (dpTotalItemsLabel != null) dpTotalItemsLabel.Text = "0";

			dpDividend.Visible = false;
            listvwDividendDetail.Visible = false;
            lblErrMsg.Visible = true;
            lblErrMsg.Text = "Data not Found";

            //var dpNextPreviousPagerField = dpDividend.Controls[1];
            //as NextPreviousPagerField;
            //if (dpNextPreviousPagerField != null) dpNextPreviousPagerField.Text = "";

        }

		private string GetSelectedScheme()
		{
			string strScmId = string.Empty;
			foreach (ListItem li in listboxSchemeName.Items)
			{
				if (li.Selected == true)
				{
					strScmId += li.Value + ",";
				}

			}
			strScmId = strScmId.Trim().TrimEnd(',');
			return strScmId;
		}

        protected void LoadNature()
        {

            DataTable _dt = AllMethods.getSebiNature();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlCategory.Items[0].Selected = true;
        }

        protected void LoadAllSubNature()
        {
            DataTable _dt = AllMethods.getAllSebiSubNature();
            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Add(new ListItem("All", "-1"));
            ddlSubCategory.Items.Add(new ListItem("Large Cap", "9000"));
            ddlSubCategory.Items.Add(new ListItem("Mid & Small Cap", "9001"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlSubCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }
            ddlSubCategory.Items[0].Selected = true;
        }

        protected void LoadSubNature(int id)
        {
            ddlSubCategory.ClearSelection();
            if (id == -1)
            {
                ddlSubCategory.Items[0].Selected = true;
                return;
            }
            DataTable _dt = AllMethods.getSebiSubNature(id);
            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Add(new ListItem("All", "-1"));
            //if (id == 3)
            //{
            //    ddlSubCategory.Items.Add(new ListItem("Large Cap", "9000"));
            //    ddlSubCategory.Items.Add(new ListItem("Mid & Small Cap", "9001"));
            //}
            if (_dt != null)
                foreach (DataRow drow in _dt.Rows)
                {
                    ddlSubCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }

            ddlSubCategory.Items[0].Selected = true;
        }

        

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundHouse.SelectedIndex != 0)
            {
                //int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
                getSchemesList();
            }
           
        }
        private void getSchemesList()
        {
            DataTable dtScheme = new DataTable();
            
            dtScheme = AllMethods.getSebiSchemeBluechip(Convert.ToInt32(ddlCategory.SelectedItem.Value),
                           -1, Convert.ToInt32(ddlSubCategory.SelectedValue),
                           3, Convert.ToInt32(ddlFundHouse.SelectedValue), false);


            listboxSchemeName.Items.Clear();
            if (dtScheme != null)
            {
                DataRow drSch = dtScheme.NewRow();
                drSch["Sch_Short_Name"] = "Select";
                drSch["Scheme_Id"] = -1;
                dtScheme.Rows.InsertAt(drSch, 0);

                listboxSchemeName.DataSource = dtScheme;
                listboxSchemeName.DataTextField = "Sch_Short_Name";
                listboxSchemeName.DataValueField = "Scheme_Id";
                listboxSchemeName.DataBind();
            }
            else
            {
                listboxSchemeName.Items.Add(new ListItem("Select", "-1"));
            }
        }
    }
}