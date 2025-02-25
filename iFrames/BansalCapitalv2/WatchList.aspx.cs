using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;
using System.Text;
using System.Collections;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace iFrames.BansalCapital
{
    public partial class WatchList : System.Web.UI.Page
    {

        #region Page
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string v = Request.QueryString["param"];
                if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                {
                    DivWatchList.Visible = true;
                    HdSelectedScheme.Value = Request.QueryString["param"];
                    OnLoadDataBind();
                }
                else
                {
                    DivWatchList.Visible = false;
                    HdSelectedScheme.Value = "";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["user"]))
                {
                    Userid.Value = Request.QueryString["user"];
                }
                else
                {
                    Userid.Value = "WrongUser";
                }

                //getFundHouse();
                //LoadNature();
                //LoadAllSubNature();
                //LoadStructure();
                //loadOption();
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
        //private void getFundHouse()
        //{
        //    DataTable dtFund = AllMethods.getFundHouse();
        //    DataRow drFund = dtFund.NewRow();
        //    drFund["MutualFund_Name"] = "Select";
        //    drFund["MutualFund_ID"] = -1;
        //    dtFund.Rows.InsertAt(drFund, 0);
        //    ddlFundHouse.DataSource = dtFund;
        //    ddlFundHouse.DataTextField = "MutualFund_Name";
        //    ddlFundHouse.DataValueField = "MutualFund_ID";
        //    ddlFundHouse.DataBind();
        //}
        //protected void loadOption()
        //{
        //    var OptionDataTable = AllMethods.getOption();
        //    rdbOption.DataSource = OptionDataTable;
        //    rdbOption.DataTextField = "Name";
        //    rdbOption.DataValueField = "Id";
        //    rdbOption.DataBind();
        //    rdbOption.SelectedIndex = 0;
        //}

        //protected void LoadNature()
        //{

        //    DataTable _dt = AllMethods.getNature();
        //    ddlCategory.Items.Clear();
        //    ddlCategory.Items.Add(new ListItem("All", "-1"));
        //    foreach (DataRow drow in _dt.Rows)
        //    {
        //        ddlCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
        //    }

        //    ddlCategory.Items[0].Selected = true;
        //}


        ////getAllSubNature

        //protected void LoadAllSubNature()
        //{
        //    DataTable _dt = AllMethods.getAllSubNature();
        //    ddlSubCategory.Items.Clear();
        //    ddlSubCategory.Items.Add(new ListItem("All", "-1"));
        //    foreach (DataRow drow in _dt.Rows)
        //    {
        //        ddlSubCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
        //    }

        //    ddlSubCategory.Items[0].Selected = true;
        //}
        //protected void LoadSubNature(int id)
        //{
        //    if (id == -1)
        //    {
        //        ddlSubCategory.Items[0].Selected = true;
        //        return;
        //    }
        //    DataTable _dt = AllMethods.getSubNature(id);
        //    ddlSubCategory.Items.Clear();
        //    ddlSubCategory.Items.Add(new ListItem("All", "-1"));
        //    if (_dt != null)
        //        foreach (DataRow drow in _dt.Rows)
        //        {
        //            ddlSubCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
        //        }

        //    ddlSubCategory.Items[0].Selected = true;
        //}


        //protected void LoadStructure()
        //{
        //    DataTable _dt = AllMethods.getStructure();
        //    ddlType.Items.Clear();
        //    ddlType.Items.Add(new ListItem("All", "-1"));
        //    foreach (DataRow drow in _dt.Rows)
        //    {
        //        ddlType.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
        //    }

        //    ddlType.Items[0].Selected = true;
        //}


        #endregion


        #region Fetch Data

        //private void DataBind()
        //{
        //    string RankYearChk = System.Configuration.ConfigurationManager.AppSettings["RankYearChk"];

        //    DataTable dtResult = new DataTable();              //getTopFundRank4BansalCapital

        //    dtResult = AllMethods.getTopFundRank(Convert.ToInt32(ddlType.SelectedItem.Value),Convert.ToInt32(ddlCategory.SelectedItem.Value), "Per_7_Days",
        //        Convert.ToInt32(ddlSubCategory.SelectedValue),Convert.ToInt32(rdbOption.SelectedValue), RankYearChk);

        //    if (dtResult != null)
        //    {
        //        DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));

        //        dtResult.Columns.Add(rankCol);

        //        int i = 1;
        //        foreach (DataRow dr in dtResult.Rows)
        //        {
        //            dr["Rank"] = i;
        //            i++;
        //        }
        //    }
        //    if (dtResult == null)
        //    {
        //        lstResult.DataSource = new DataTable();
        //    }
        //    else
        //    {
        //        lstResult.DataSource = dtResult;
        //    }
        //    lstResult.DataBind();

        //    //if (dtResult != null)
        //    //{
        //    //    lbtopText.Text = "";
        //    //}
        //    //else
        //    //{
        //    //    lbtopText.Text = "";
        //    //}
        //}



        private void OnLoadDataBind()
        {
            //string RankYearChk = System.Configuration.ConfigurationManager.AppSettings["RankYearChk"];

            DataTable dtResult = new DataTable();              //getTopFundRank4BansalCapital

            List<WatchScheme> _listScheme = new List<WatchScheme>();
            foreach (string s in HdSelectedScheme.Value.Split(','))
            {
                _listScheme.Add(new WatchScheme() { SchemeId = Convert.ToInt32(s) });
            }
            //_listScheme.Add(new WatchScheme() { SchemeId = 9566 });
            //_listScheme.Add(new WatchScheme() { SchemeId = 10890 });
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
            }
            else
            {
                lstResult.DataSource = dtResult;
            }
            lstResult.DataBind();

            //if (dtResult != null)
            //{
            //    lbtopText.Text = "All the schemes on the basis of last 1 Week performance";
            //}
            //else
            //{
            //    lbtopText.Text = "";
            //}
        }
        #endregion

        //private void getSchemesList()
        //{
        //    DataTable dtScheme = new DataTable();
        //    //if (NatureId == "-1")
        //    //    dtScheme = AllMethods.getSchemeCategory(false, false);
        //    //else
        //    //    dtScheme = AllMethods.getSchemeCategory(false, false, NatureId);
        //    dtScheme = AllMethods.getScheme(Convert.ToInt32(ddlCategory.SelectedItem.Value), Convert.ToInt32(ddlType.SelectedItem.Value), 
        //        Convert.ToInt32(ddlSubCategory.SelectedValue),Convert.ToInt32(rdbOption.SelectedValue), Convert.ToInt32(ddlFundHouse.SelectedValue));
        //    ddlSchemes.Items.Clear();
        //    if (dtScheme != null)
        //    {
        //        DataRow drSch = dtScheme.NewRow();
        //        drSch["Sch_Short_Name"] = "Select";
        //        drSch["Scheme_Id"] = 0;
        //        dtScheme.Rows.InsertAt(drSch, 0);

        //        ddlSchemes.DataSource = dtScheme;
        //        ddlSchemes.DataTextField = "Sch_Short_Name";
        //        ddlSchemes.DataValueField = "Scheme_Id";
        //        ddlSchemes.DataBind();
        //    }
        //    else
        //    {
        //        ddlSchemes.Items.Add(new ListItem("Select", "-1"));
        //    }
            
        //}

        //protected void btnResetClick(object sender, EventArgs e)
        //{
        //    ddlCategory.SelectedIndex = 0;
        //    ddlSubCategory.SelectedIndex = 0;
        //    ddlType.SelectedIndex = 0;
        //    lstResult.Visible = false;
        //    Result.Visible = false;
        //    lbtopText.Text = "";
        //}

        //protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
        //    LoadSubNature(nature_id);
        //}

        //protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlFundHouse.SelectedIndex != 0)
        //        getSchemesList();
        //}

        //protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlFundHouse.SelectedIndex != 0)
        //        getSchemesList();
        //}

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void rdbOption_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlFundHouse.SelectedIndex != 0)
        //        getSchemesList();
        //}

        //protected void ddlMutualFund_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlFundHouse.SelectedIndex != 0)
        //        getSchemesList();
        //}

        //protected void BtnAdd2Watch_Click(object sender, EventArgs e)
        //{
        //    if (ddlSchemes.SelectedIndex != 0)
        //    {
        //        DivWatchList.Visible = true;
        //        int SchemeId = Convert.ToInt32(ddlSchemes.SelectedValue);
        //        HdSelectedScheme.Value = HdSelectedScheme.Value == "" ? SchemeId.ToString() : HdSelectedScheme.Value + "," + SchemeId;
        //        OnLoadDataBind();
        //    }
        //}
    }
}