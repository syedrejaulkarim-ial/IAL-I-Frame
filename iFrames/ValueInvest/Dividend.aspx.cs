using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;

namespace iFrames.ValueInvest
{
	public partial class Dividend : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				getFundHouse();
				txtfromDate.Text = DateTime.Today.AddDays(-8).ToString("dd MMM yyyy");
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			getDividend();
			dpDividend.SetPageProperties(0, dpDividend.MaximumRows, true);

		}

		protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlFundHouse.SelectedIndex != 0)
				getSchemesList(ddlFundHouse.SelectedItem.Value);
			else
			{
				listboxSchemeName.Items.Clear();
			}
		}


		protected void listboxSchemeName_SelectedIndexChanged(object sender, EventArgs e)
		{

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
			listboxSchemeName.DataSource = dtScheme;
			listboxSchemeName.DataTextField = "Sch_Short_Name";
			listboxSchemeName.DataValueField = "Scheme_Id";
			listboxSchemeName.DataBind();
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
				}
				listvwDividendDetail.DataSource = _dt;
				listvwDividendDetail.DataBind();

				if (_dt == null)
					resetDp();
				else
					dpDividend.Visible = true;
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
	}
}