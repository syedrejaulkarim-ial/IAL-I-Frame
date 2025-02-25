using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.DAL;
using iFrames.BLL;
using iFrames.Pages;
using System.Linq.Dynamic;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.Script.Services;

namespace iFrames.BansalCapital
{
    public partial class SchemeSearch : System.Web.UI.Page
    {

        #region Page
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

                LoadSebiNature();
                LoadAllSebiSubNature();
                LoadStructure();
                loadOption();
                PopulateFundRiskButtons();
                Result.Visible = false;
                if (Request.QueryString["param"] != null)
                {
                    OnloadDataBind(Convert.ToString(Request.QueryString["param"]));
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            // DataBind();
            base.OnPreRender(e);
        }

        #endregion


        #region Event

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

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dpTopFund") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                DataBind();
            }
        }

        #endregion


        #region Load Data

        protected void loadOption()
        {
            var OptionDataTable = AllMethods.getOption();
            rdbOption.DataSource = OptionDataTable;
            rdbOption.DataTextField = "Name";
            rdbOption.DataValueField = "Id";
            rdbOption.DataBind();
            rdbOption.SelectedIndex = 0;
        }

        protected void LoadSebiNature()
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


        //getAllSubNature

        protected void LoadAllSebiSubNature()
        {
            DataTable _dt = AllMethods.getAllSebiSubNature();
            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Add(new ListItem("All", "-1"));
            //ddlSubCategory.Items.Add(new ListItem("Large Cap", "9000"));
            //ddlSubCategory.Items.Add(new ListItem("Mid & Small Cap", "9001"));
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


        #endregion


        #region Fetch Data


        private void OnloadDataBind(string strSch)
        {
            //string RankYearChk = System.Configuration.ConfigurationManager.AppSettings["RankYearChk"];

            DataTable dtResult = new DataTable();              //getTopFundRank4BansalCapital

            //dtResult = AllMethods.getTopFundRank(-1, Convert.ToInt32(ddlType.SelectedItem.Value),
            //    Convert.ToInt32(ddlCategory.SelectedItem.Value), "Per_1_Year", Convert.ToInt32(ddlSubCategory.SelectedValue),
            //    Convert.ToInt32(rdbOption.SelectedValue), Convert.ToInt32(HiddenFundRisk.Value), Convert.ToInt32(HiddenMinimumInvesment.Value),
            //    Convert.ToInt32(HiddenFieldRating.Value), RankYearChk);
            List<WatchScheme> _listScheme = new List<WatchScheme>();

            _listScheme.Add(new WatchScheme() { SchemeId = Convert.ToInt32(strSch) });

            dtResult = AllMethods.getSebiSchemeDetails(_listScheme, "Per_7_Days");

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

        private void DataBind()
        {
            //string RankYearChk = System.Configuration.ConfigurationManager.AppSettings["RankYearChk"];

            DataTable dtResult = new DataTable();              //getTopFundRank4BansalCapital
            //var _LstRating = new List<int>();

            //if (HiddenFieldRating1.Value != "0")
            //    _LstRating.Add(Convert.ToInt16(HiddenFieldRating1.Value));
            
            //if (HiddenFieldRating2.Value != "0")
            //    _LstRating.Add(Convert.ToInt16(HiddenFieldRating2.Value));
            
            //if (HiddenFieldRating3.Value != "0")
            //    _LstRating.Add(Convert.ToInt16(HiddenFieldRating3.Value));
            
            //if (HiddenFieldRating4.Value != "0")
            //    _LstRating.Add(Convert.ToInt16(HiddenFieldRating4.Value));
            
            //if (HiddenFieldRating5.Value != "0")
            //    _LstRating.Add(Convert.ToInt16(HiddenFieldRating5.Value));

            dtResult = AllMethods.getSebiTopFundRank(-1, Convert.ToInt32(ddlType.SelectedItem.Value),
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

        #endregion

        protected void btnResetClick(object sender, EventArgs e)
        {
            ddlCategory.SelectedIndex = 0;
            ddlSubCategory.SelectedIndex = 0;
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

        protected void rbuttonOption_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void PopulateFundRiskButtons()
        {
            int _Low = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LowRiskometer"]);
            int _ModLow = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ModLowRiskometer"]);
            int _Mod = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ModRiskometer"]);
            int _ModHigh = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ModHighRiskometer"]);
            int _High = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["HighRiskometer"]);

            low.Attributes.Add("for", _Low.ToString());
            mod_low.Attributes.Add("for", _ModLow.ToString());
            mod.Attributes.Add("for", _Mod.ToString());
            mod_high.Attributes.Add("for", _ModHigh.ToString());
            high.Attributes.Add("for", _High.ToString());
            all.Attributes.Add("for", "-1");
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string[] GetCustomers(string prefix)
        {
            //List<string> customers = new List<string>();
            //customers.Add("test-1");
            //customers.Add("teseeet-3");

            //customers.Add("tessdfrt-7");

            //return customers.ToArray();

            var data = AllMethods.getAutoCompleteScheme(prefix);
            if (data != null)
                return data.ToArray();
            else
                return new string[] { };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string[] TopSearchScheme(string prefix)
        {
            //List<string> customers = new List<string>();
            //customers.Add("test-1");
            //customers.Add("teseeet-3");

            //customers.Add("tessdfrt-7");

            //return customers.ToArray();

            var data = AllMethods.getTopSearch(prefix);
            if (data != null)
                return data.ToArray();
            else
                return new string[] { };
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            LoadSebiNature();
            LoadAllSebiSubNature();
            LoadStructure();
            loadOption();
            PopulateFundRiskButtons();
            Result.Visible = false;
        }

        //[WebMethod]
        //public static List<dataJson> GetCustomersAll(string prefix)
        //{
        //    //                  List<dataJson> _dd= new List<dataJson>();
        //    //_dd.Add(new dataJson(){id="12",label="fgfgjasvj"});     
        //    //_dd.Add(new dataJson(){id="234",label="eeeeeeeeee"});
        //    //_dd.Add(new dataJson(){id="345",label="ccccccccc"});

        //    //return _dd;
        //    var data = AllMethods.getAutoCompleteScheme(prefix);
        //    return data;
        //}
    }


}