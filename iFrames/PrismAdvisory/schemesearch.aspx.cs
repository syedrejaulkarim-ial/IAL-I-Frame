using iFrames.BansalCapital;
using iFrames.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.PrismAdvisory
{
    public partial class schemesearch : System.Web.UI.Page
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
                Result.Visible = false;
                if (Request.QueryString["param"] != null)
                {
                    OnloadDataBind(Convert.ToString(Request.QueryString["param"]));
                }
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
        private void OnloadDataBind(string strSch)
        {
           

            DataTable dtResult = new DataTable();              //getTopFundRank4BansalCapital
          
            List<WatchScheme> _listScheme = new List<WatchScheme>();

            _listScheme.Add(new WatchScheme() { SchemeId = Convert.ToInt32(strSch) });

            dtResult = AllMethods.getSchemeDetails(_listScheme, "Per_7_Days");

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
            if (dtResult == null)
            {
                lstResult.DataSource = new DataTable();
                Result.Visible = false;
            }
            else
            {
                lstResult.DataSource = dtResult;
                Result.Visible = true;
            }
            lstResult.DataBind();

            if (dtResult != null)
            {
                lbtopText.Text = "All the schemes on the basis of 1 year performance";
            }
            else
            {
                lbtopText.Text = "";
            }
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
        private void DataBind()
        {
            

            DataTable dtResult = new DataTable();              //getTopFundRank4BansalCapital          

            dtResult = AllMethods.getTopFundRank(-1, Convert.ToInt32(ddlType.SelectedItem.Value),
                Convert.ToInt32(ddlCategory.SelectedItem.Value), "Per_1_Year", Convert.ToInt32(ddlSubCategory.SelectedValue),
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
            }
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
                lbtopText.Text = "All the schemes on the basis of 1 year performance";
            }
            else
            {
                lbtopText.Text = "";
            }
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            LoadSubNature(nature_id);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataBind();

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
            ddlType.SelectedIndex = 0;
            lstResult.Visible = false;
            Result.Visible = false;
            lbtopText.Text = "";
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dpTopFund") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                DataBind();
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string[] GetCustomers(string prefix)
        { 
            var data = AllMethods.getAutoCompleteScheme(prefix);
            if (data != null)
                return data.ToArray();
            else
                return new string[] { };
        }
    }
}