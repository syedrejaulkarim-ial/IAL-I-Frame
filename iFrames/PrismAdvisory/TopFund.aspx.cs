using iFrames.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.PrismAdvisory
{
    public partial class TopFund : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["user"]))
            {
                Userid.Value = Request.QueryString["user"];
            }
            else
            {
                Userid.Value = "WrongUser";
            }

            if (!IsPostBack)
            {
                LoadNature();
                LoadAllSubNature();
               LoadStructure();
               loadOption();
               //PopulateFundRiskButtons();
               PageLoadDataBind();
               HiddenFundRiskStrColor.Value = "all";
               HiddenFundRisk.Value = "-1";
            } 
        }
        protected void LoadNature()
        {

            DataTable _dt = AllMethods.getNature();
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
            DataTable _dt = AllMethods.getAllSubNature();
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
            if (id == -1)
            {
                ddlSubCategory.ClearSelection();
                ddlSubCategory.Items[0].Selected = true;
                return;
            }
            DataTable _dt = AllMethods.getSubNature(id);
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
        protected void LoadStructure()
        {
            DataTable _dt = AllMethods.getStructure();
            ddlType.Items.Clear();
            ddlType.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlType.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlType.Items[0].Selected = true;
        }
        protected void loadOption()
        {
            var OptionDataTable = AllMethods.getOption();
            rdbOption.DataSource = OptionDataTable;
            rdbOption.DataTextField = "Name";
            rdbOption.DataValueField = "Id";
            rdbOption.DataBind();
            rdbOption.SelectedIndex = 0;
        }

        #region Fetch Data

        private void PageLoadDataBind()
        {
            

            DataTable dtResult = new DataTable();
            
            if (Cache["dtResult"] == null)
            {
                dtResult = AllMethods.getTopFundRank(-1, -1, -1, "Per_1_Year", -1, 2, -1, 500, Convert.ToInt32(HiddenMinimumSIReturn.Value));
                if (dtResult != null)
                {
                    DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));

                    dtResult.Columns.Add(rankCol);

                    int i = 1;
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        dr["Rank"] = i;
                        i++;
                    }
                }
                Cache.Add("dtResult", dtResult, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtResult = (DataTable)Cache["dtResult"];
            }


            //lstResult.DataSource = dtResult;
            if (dtResult == null)
            {
                lstResult.DataSource = new DataTable();
            }
            else
            {
                lstResult.DataSource = dtResult;
            }
            lstResult.DataBind();

            if (dtResult != null)
            {
                lbtopText.Text = ddlRank.SelectedItem.Text + " the schemes on the basis of : " + ddlPeriod.SelectedItem.Text;
            }
            else
            {
                lbtopText.Text = "";
            }
        }

        private void ResultDataBind()
        {
            hdIsLoad.Value = "1";
           

            DataTable dtResult = new DataTable();              //getTopFundRank4BansalCapital

           

            dtResult = AllMethods.getTopFundRank(Convert.ToInt32(ddlRank.SelectedItem.Value), Convert.ToInt32(ddlType.SelectedItem.Value),
                Convert.ToInt32(ddlCategory.SelectedItem.Value), ddlPeriod.SelectedItem.Value, Convert.ToInt32(ddlSubCategory.SelectedValue),
                Convert.ToInt32(rdbOption.SelectedValue), Convert.ToInt32(HiddenFundRisk.Value), Convert.ToInt32(HiddenMinimumInvesment.Value), Convert.ToInt32(HiddenMinimumSIReturn.Value));
            if (dtResult != null)
            {
                DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));

                dtResult.Columns.Add(rankCol);

                int i = 1;
                foreach (DataRow dr in dtResult.Rows)
                {
                    dr["Rank"] = i;
                    i++;
                }
                lstResult.DataSource = dtResult;
            }
            else
            {
                lstResult.DataSource = new DataTable();
            }

            lstResult.DataBind();

            if (dtResult != null)
            {
                lbtopText.Text = ddlRank.SelectedItem.Text + " schemes on the basis of : " + ddlPeriod.SelectedItem.Text;
            }
            else
            {
                lbtopText.Text = "";
            }
        }

        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ResultDataBind();

            var dp = lstResult.FindControl("dpTopFund") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(0, dp.MaximumRows, true);
            }
            lstResult.Visible = true;
            Result.Visible = true;

        }

        protected void btnResetClick(object sender, EventArgs e)
        {
            ddlCategory.SelectedIndex = 0;
            ddlSubCategory.SelectedIndex = 0;

            ddlPeriod.SelectedIndex = 0;
            ddlRank.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            lstResult.Visible = false;
            Result.Visible = false;
            lbtopText.Text = "";
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            LoadSubNature(nature_id);
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dpTopFund") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                //DataBind();
                if (hdIsLoad.Value == "1")
                    ResultDataBind();
                else
                    PageLoadDataBind();
            }
        }

    }
}