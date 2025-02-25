using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using iFrames.DAL;
using iFrames.BLL;
using System.Data.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.UI.DataVisualization.Charting;
using System.Web.SessionState;
using System.Drawing;
using iFrames.Kotak;
using System.Net;
using System.Collections;
using WkHtmlToXSharp;
using NUnit.Framework;

namespace iFrames.Top3choice
{
    public partial class SIPCalc : System.Web.UI.Page
    {
        #region Global declaration

        readonly string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        readonly SqlConnection conn = new SqlConnection();
        System.Data.DataTable finalResultdt = new System.Data.DataTable();
        System.Data.DataTable finalResultdtwobenchmark = new System.Data.DataTable();
        System.Data.DataTable sipDataTable = new System.Data.DataTable();

        string imgPath = string.Empty;
        string tmpChartName = string.Empty;
        int ParamSchemeId = 0;
        #endregion

        #region: PDF Global Variable

        private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string SimplePageFile = null;
        public static int count = 0;

        #endregion

        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            this.sipbtnshow.Attributes.Add("onclick", "javascript:return validateSIP();");// Add ClientSide Script for Data Validation function defined in check.js
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                {
                    int Scheme_id = AllMethods.getSchemeId(Request.QueryString["param"].ToString());
                    if (Scheme_id != 0)
                    {
                        ParamSchemeId = Scheme_id;
                    }
                }
                FillNature();
                // FillDropdownScheme();
                SetDefaultView();
                callCalc();
                if (ParamSchemeId != 0)
                    ddlscheme_SelectedIndexChanged(sender, e);
            }
            trmode.Visible = false;
            SetInceptionOnDropDown();
            txtddlsipbnmark.Attributes.Add("onmouseenter", String.Format("mouseEnter2('{0}',{1})", txtddlsipbnmark.ClientID, txtddlsipbnmark.Width.Value));
            //txtddlsipbnmark.Attributes.Add("onfocusout", String.Format("focusOut('{0}',{1})", txtddlsipbnmark.ClientID, txtddlsipbnmark.Width.Value));


            //if (ddlMode.SelectedItem.Value.ToUpper() == "LUMP SUM" || ddlMode.SelectedItem.Value.ToUpper() == "SWP" || ddlMode.SelectedItem.Value.ToUpper() == "STP")
            //    LSDisc.Visible = true;
            //else
            //
            //if (ddlMode.SelectedItem.Value.ToUpper() == "LUMP SUM")// && resultDivLS.Visible == true)
            //    LSDisc1.Visible = true;
            //else
            //LSDisc1.Visible = false;


            if (gvFirstTable.Visible)
                LSDisc.Visible = true;
            else
                LSDisc.Visible = false;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void OnPreRender(object sender, EventArgs e)
        {
            //FillDropdownScheme();
        }

        #endregion

        #region  Button Event
        protected void sipbtnshow_Click(object sender, ImageClickEventArgs e)
        {
            resultDiv.Visible = true;
            ShowResult();
        }

        protected void sipbtnreset_Click(object sender, ImageClickEventArgs e)
        {
            resultDiv.Visible = false;
            Reset();

        }

        protected void LinkButtonGenerateReport_Click(object sender, EventArgs e)
        {
            if (RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim() == "DISTRIBUTOR")
                SetDistributorCredential();

            CreateHTMLSIP();
            //Calculate_Excel_modified();
        }

        protected void RadioButtonListCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistributerDiv.Visible = RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim() == "DISTRIBUTOR";
        }

        protected void ExcelCalculation_Click(object sender, ImageClickEventArgs e)
        {
            //Calculate_Excel_modified();
            ShowExcel();
        }

        #region Tab Control
        private void SetDefaultView()
        {
            MultiView1.ActiveViewIndex = -1;
        }

        protected void lnkTab1_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;

        }

        protected void lnkTab2_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
            // Have to cahrt
            CallChartFromView2();

        }

        protected void lnkTab3_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;

        }
        protected void lnkTab4_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
            CallChartFromView2();
        }
        //protected void lnkTab5_Click(object sender, EventArgs e)
        //{
        //    MultiView2.ActiveViewIndex = 0;
        //    // LSDisc.Visible = ddlMode.SelectedItem.Value.ToUpper() == "LUMP SUM";
        //}

        #endregion
        #endregion

        #region: DropDown Method


        protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowRelativeDiv();
            // txtinstall.Text = "";
            txtfromDate.Text = "";
        }

        protected void ddlscheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SetIndexDropdown();
            //ListItem li = (ListItem)ViewState["ListItem"];
            FillDropdownIndex();

            //if (ddlsipbnmark.Visible)
            //    ddlsipbnmark.Visible = false;
            if (ddlscheme.SelectedIndex == 0 || ddlscheme.SelectedItem.Value == "--")
            {
                return;
            }
            else
            {
                FillDropdown(ddlschtrto, ddlscheme.SelectedItem.Value);
            }
            resultDiv.Visible = false;

            SIPSchDt.Text = "";
            SIPSchDt2.Text = "";
            #region Launch Date
            using (var principl = new PrincipalCalcDataContext())
            {
                string schmeId = ddlscheme.SelectedItem.Value;
                var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    LaunchDate = ind.Launch_Date
                                };
                //SIPSchDt.Text = "";
                if (allotdate != null && allotdate.Count() > 0)
                {

                    //SIPSchDt.Text = "<b>" + Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "<b/>";

                    SIPSchDt.Text = Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
            #endregion
            SetInceptionOnDropDown();

            //txtinstall.Text = "";
            txtfromDate.Text = "";
        }

        protected void ddlschtrto_SelectedIndexChanged(object sender, EventArgs e)
        {
            // STPSchDt.Text = "";
            resultDiv.Visible = false;

            #region Launch Date
            using (var principl = new PrincipalCalcDataContext())
            {
                string schmeId = ddlschtrto.SelectedItem.Value;
                var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    LaunchDate = ind.Launch_Date
                                };
                //SIPSchDt.Text = "";
                if (allotdate != null && allotdate.Count() > 0)
                {

                    //SIPSchDt.Text = "<b>" + Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "<b/>";

                    SIPSchDt2.Text = Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
            #endregion
            SetInceptionOnDropDown();
            // txtinstall.Text = "";
            txtfromDate.Text = "";
        }

        protected void ddlNature_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropdownScheme();
            resultDiv.Visible = false;
            //ddlsipbnmark.Items.Clear();
            txtddlsipbnmark.Text = "";
            //lblddlsipbnmark.Text = "";
            SIPSchDt.Text = ""; SIPSchDt2.Text = "";
            //Reset();
        }

        protected void GridViewSIPResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            int rw = e.Row.RowIndex;
            if (e.Row.RowType == DataControlRowType.DataRow && rw == 2)
            {

                // e.Row.Attributes["style"] = "border-top: #c6c8ca solid 5px;";
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-top:#4f4e50 solid 3px";
                }
            }


            // foreach(DataColumn 
        }

        #endregion

        #region Method

        #region : Fill Methods


        /// <summary>
        /// FillNature() define for Populating Category
        /// Added Custom Category in dropdownlist
        /// </summary>
        /// 

        protected void FillNature()
        {
            try
            {
                using (var natureData = new PrincipalCalcDataContext())
                {

                    string[] natureExcluded = { "N.A", "Speciality", "ETF", "Dynamic/Asset Allocation", "Balanced", "Liquid", "Debt" };
                    List<string> natureExcludedList = natureExcluded.ToList<string>();

                    var nature = from nat in natureData.T_SCHEMES_NATUREs
                                 where !natureExcludedList.Contains(nat.Nature) //nat.Nature != "N.A" &&
                                 orderby nat.Nature
                                 select new
                                 {
                                     nat.Nature
                                 };

                    if (nature.Count() > 0)
                    {
                        DataTable dtNature = null;
                        dtNature = nature.ToDataTable();
                        ddlNature.DataSource = dtNature;
                        ddlNature.DataTextField = "Nature";
                        ddlNature.DataValueField = "Nature";
                        ddlNature.DataBind();
                        ddlNature.Items.Insert(0, new ListItem("--", "0"));
                        ddlNature.Items.Add(new ListItem("Hybrid"));
                        ddlNature.Items.Add(new ListItem("Fixed Income"));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }


        }

        public void FillDropdownScheme()
        {
            FillDropdown(ddlscheme);
        }

        /// <summary>
        /// FillDropdown Method is Used To populate Schemes in corresponding dropdownlist
        /// </summary>
        /// <param name="ddl"></param>

        private void FillDropdown(Control ddl)
        {
            try
            {
                System.Data.DataTable dt = FetchScheme();
                DropDownList drpdwn = (DropDownList)ddl;
                drpdwn.Items.Clear();
                int selectedindex = 0;
                if (dt.Rows.Count > 0)
                {

                    Dictionary<string, string> SchemeInception = new Dictionary<string, string>();

                    drpdwn.DataTextField = "Sch_Short_Name";
                    drpdwn.DataValueField = "Scheme_Id";
                    
                    int counter=0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        counter++;
                        if (ParamSchemeId != 0)
                        {
                            if (dr["Scheme_Id"].ToString() == ParamSchemeId.ToString())
                                selectedindex = counter;
                        }
                        ListItem li = new ListItem(dr["Sch_Short_Name"].ToString(), dr["Scheme_Id"].ToString());
                        li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        SchemeInception.Add(dr["Scheme_Id"].ToString(), dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        drpdwn.Items.Add(li);
                    }
                    // drpdwn.DataBind();
                    ViewState["SchemeInception"] = SchemeInception;

                }

                drpdwn.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
                drpdwn.SelectedIndex = selectedindex;               
            }
            catch (Exception ex)
            {


            }
        }

        public void FillDropdown(Control ddl, string schid)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = FetchSchemeWithout(schid);

                DropDownList drpdwn = (DropDownList)ddl;
                drpdwn.Items.Clear();
                if (dt.Rows.Count > 0)
                {

                    Dictionary<string, string> SchemeInception = new Dictionary<string, string>();

                    drpdwn.DataTextField = "Sch_Short_Name";
                    drpdwn.DataValueField = "Scheme_Id";
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem li = new ListItem(dr["Sch_Short_Name"].ToString(), dr["Scheme_Id"].ToString());
                        li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        SchemeInception.Add(dr["Scheme_Id"].ToString(), dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        drpdwn.Items.Add(li);
                    }
                    // drpdwn.DataBind();
                    ViewState["SchemeInception2"] = SchemeInception;
                }

                drpdwn.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
                drpdwn.SelectedIndex = 0;
                //if (dt.Rows.Count > 0)
                //{
                //    DropDownList drpdwn = (DropDownList)ddl;
                //    drpdwn.DataSource = dt;
                //    drpdwn.DataTextField = "Sch_Short_Name";
                //    drpdwn.DataValueField = "Scheme_Id";
                //    drpdwn.DataBind();
                //    drpdwn.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
                //    drpdwn.SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void FillDropdownIndex()
        {
            FillDropdownIndex(txtddlsipbnmark);
            //FillDropdownIndex(lblddlsipbnmark);
        }

        /// <summary>
        /// this Function will Populate Benchmark of Selected Scheme
        /// </summary>
        /// <param name="ddl"></param>

        public void FillDropdownIndex(Control ddl)
        {
            try
            {
                DataTable dt = FetchBenchMark(Convert.ToDecimal(ddlscheme.SelectedItem.Value));
                if (dt.Rows.Count > 0)
                {

                    // Control is DropDown

                    //DropDownList drpdwn = (DropDownList)ddl;                    
                    //drpdwn.Items.Clear();
                    //drpdwn.DataTextField = "INDEX_NAME";
                    //drpdwn.DataValueField = "INDEX_ID";
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ListItem li = new ListItem(dr["INDEX_NAME"].ToString(), dr["INDEX_ID"].ToString());
                    //    ViewState["INDEX_ID"] = dr["INDEX_ID"].ToString();
                    //    ViewState["INDEX_NAME"] = dr["INDEX_NAME"].ToString();                        
                    //    drpdwn.Items.Add(li);
                    //}


                    TextBox labelbenchmark = (TextBox)ddl;
                    labelbenchmark.Text = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem li = new ListItem(dr["INDEX_NAME"].ToString(), dr["INDEX_ID"].ToString());
                        ViewState["INDEX_ID"] = dr["INDEX_ID"].ToString();
                        ViewState["INDEX_NAME"] = dr["INDEX_NAME"].ToString();

                        labelbenchmark.Text += dr["INDEX_NAME"].ToString() + " , ";

                    }
                    labelbenchmark.Text = labelbenchmark.Text.Trim(); labelbenchmark.Text = labelbenchmark.Text.TrimEnd(','); labelbenchmark.Text = labelbenchmark.Text.Trim();

                }
            }
            catch (Exception ex)
            {


            }
        }



        #endregion

        #region: Fetch Methods

        /// <summary>
        /// This Function will Fetch all Scheme
        /// </summary>
        /// <returns>DataTable with SchemeName,SchemeId,Launch Date </returns>


        public System.Data.DataTable FetchScheme()
        {
            //conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            string selected_mode = ddlMode.SelectedItem.Text;
            List<decimal?> excludeSubnatureList = new List<decimal?>();
            excludeSubnatureList.AddRange(new decimal?[] { 2, 21 });// FMP FTP ,Marginal Equity

            List<decimal?> excludeNatureIdList = new List<decimal?>();
            excludeNatureIdList.AddRange(new decimal?[] { 6 });// Liqidity

            try
            {


                using (var scheme = new PrincipalCalcDataContext())
                {

                    if (ddlNature.SelectedIndex == 0)// Nature is not Selected
                    {

                        var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs                                         
                                         select new
                                         {
                                             t_fund_masters.FUND_ID,
                                             t_fund_masters.NATURE_ID,
                                             t_fund_masters.SUB_NATURE_ID
                                         });

                        if (selected_mode.ToUpper().StartsWith("SIP"))
                        {
                            //fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                            //             where
                            //                 t_fund_masters.MUTUALFUND_ID == 9 &&
                            //                 && !excludeNatureIdList.Contains( t_fund_masters.NATURE_ID )
                            //                //t_fund_masters.NATURE_ID != 6
                            //             select new
                            //             {
                            //                 t_fund_masters.FUND_ID,
                            //                 t_fund_masters.NATURE_ID,
                            //                 t_fund_masters.SUB_NATURE_ID
                            //             });

                            fundtable = from d in fundtable
                                        where !excludeNatureIdList.Contains(d.NATURE_ID)
                                        && !excludeSubnatureList.Contains(d.SUB_NATURE_ID)
                                        select d;
                        }


                        DataTable dtt = null;
                        if (fundtable.Count() > 0)
                            dtt = fundtable.ToDataTable();


                        var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                             join T in fundtable
                                             on s.Fund_Id equals T.FUND_ID
                                             where //s.T_FUND_MASTER.SUB_NATURE_ID != 2 &&  s.Nav_Check != 2
                                                 // !excludeSubnatureList.Contains(T.SUB_NATURE_ID) &&
                                                 // T.SUB_NATURE_ID!= 2 &&                                             
                                             s.Nav_Check == 3 //&& s.Option_Id == 2 || s.Scheme_Id == 2554 // for growth option & DSP BlackRock Equity Fund added additionaly
                                             // && s.Launch_Date != null
                                             join tsi in scheme.T_SCHEMES_INDEXes
                                             on s.Scheme_Id equals tsi.SCHEME_ID
                                             orderby s.Sch_Short_Name
                                             select new
                                             {
                                                 s.Sch_Short_Name,
                                                 s.Scheme_Id,
                                                 s.Launch_Date
                                             }).Distinct();
                        DataTable dt2 = null;
                        if (scheme_name_1.Count() > 0)
                            dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();

                        dt = dt2.Copy();
                    }
                    else// WHEN THE NATURE IS SELECTED
                    {
                        string selected_nature = ddlNature.SelectedValue;
                        List<string> lstnatureSelected = new List<string>();
                        lstnatureSelected.Clear();

                        if (selected_nature == "Hybrid")
                        {
                            lstnatureSelected.Add("Debt"); lstnatureSelected.Add("Balanced");
                        }
                        else if (selected_nature == "Fixed Income")
                        {
                            lstnatureSelected.Add("Debt"); lstnatureSelected.Add("Liquid");
                        }
                        else
                        {
                            lstnatureSelected.Add(selected_nature);
                        }


                        var natur = from t_schemes_natures in scheme.T_SCHEMES_NATUREs
                                    where lstnatureSelected.Contains(t_schemes_natures.Nature)
                                    select t_schemes_natures.Nature_ID;


                        List<decimal?> natureList = new List<decimal?>();
                        natureList.Clear();
                        if (natur.Count() > 0)
                        {
                            foreach (var k in natur.ToList())
                            {
                                natureList.Add(k);
                            }
                        }



                        var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                         where                                             
                                            natureList.Contains(t_fund_masters.NATURE_ID)
                                         select new
                                         {
                                             t_fund_masters.FUND_ID,
                                             t_fund_masters.NATURE_ID,
                                             t_fund_masters.SUB_NATURE_ID
                                         });

                        if (selected_nature == "Hybrid")
                        {
                            fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                         where
                                           ((t_fund_masters.NATURE_ID == 22 && t_fund_masters.SUB_NATURE_ID == 8) || t_fund_masters.NATURE_ID == 7)
                                         select new
                                         {
                                             t_fund_masters.FUND_ID,
                                             t_fund_masters.NATURE_ID,
                                             t_fund_masters.SUB_NATURE_ID
                                         });


                        }
                        else if (selected_nature == "Fixed Income")
                        {
                            fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                         where
                                            ((t_fund_masters.NATURE_ID == 22 && t_fund_masters.SUB_NATURE_ID != 8) || t_fund_masters.NATURE_ID == 6)
                                         select new
                                         {
                                             t_fund_masters.FUND_ID,
                                             t_fund_masters.NATURE_ID,
                                             t_fund_masters.SUB_NATURE_ID
                                         });

                            if (selected_mode.ToUpper() != "LUMP SUM")
                            {
                                var d = from ft in fundtable
                                        where !excludeSubnatureList.Contains(ft.SUB_NATURE_ID)
                                        select ft;
                                fundtable = d;
                            }
                        }

                        // sip does not contain liquidity
                        if (selected_mode.ToUpper().StartsWith("SIP"))
                        {
                            var d = from ft in fundtable
                                    where !excludeNatureIdList.Contains(ft.NATURE_ID)
                                    select ft;
                            fundtable = d;
                        }

                        DataTable dtt = new DataTable();
                        if (fundtable.Count() > 0)
                            dtt = fundtable.ToDataTable();

                        var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                             join T in fundtable
                                             on s.Fund_Id equals T.FUND_ID
                                             where s.Nav_Check == 3//s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 && s.Nav_Check != 2
                                             //&& T.SUB_NATURE_ID !=2
                                             // && !excludeSubnatureList.Contains(T.SUB_NATURE_ID)
                                             //&& s.Launch_Date <= yearBacktodaysdate1
                                             //&& s.Launch_Date != null
                                             join tsi in scheme.T_SCHEMES_INDEXes
                                             on s.Scheme_Id equals tsi.SCHEME_ID
                                             //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci                                              
                                             orderby s.Sch_Short_Name
                                             select new
                                             {
                                                 s.Sch_Short_Name,
                                                 s.Scheme_Id,
                                                 s.Launch_Date
                                             }).Distinct();

                        DataTable dt2 = new DataTable();
                        if (scheme_name_1.Count() > 0)
                            dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();

                        dt = dt2.Copy();


                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// This Function will fetch all scheme except schid
        /// </summary>
        /// <param name="schid"></param>
        /// <returns>DataTable containing Scheme Name and SchemeId</returns>

        public DataTable FetchSchemeWithout(string schid)
        {
            //conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            List<decimal?> excludeSubnatureList = new List<decimal?>();
            excludeSubnatureList.AddRange(new decimal?[] { 2, 21 });// FMP FTP ,Marginal Equity

            try
            {

                using (var scheme = new PrincipalCalcDataContext())
                {
                    var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                     where
                                       t_fund_masters.MUTUALFUND_ID == 9
                                       && !excludeSubnatureList.Contains(t_fund_masters.SUB_NATURE_ID)
                                     select new
                                     {
                                         t_fund_masters.FUND_ID
                                     });

                    DataTable dtt = null;
                    if (fundtable.Count() > 0)
                        dtt = fundtable.ToDataTable();


                    var scheme_name_1 = (
                        from s in scheme.T_SCHEMES_MASTERs
                        join T in fundtable
                        on s.Fund_Id equals T.FUND_ID
                        join tsi in scheme.T_SCHEMES_INDEXes
                        on s.Scheme_Id equals tsi.SCHEME_ID
                        where //s.T_FUND_MASTER.SUB_NATURE_ID != 2 && 
                        s.Nav_Check == 3 && s.Scheme_Id != Convert.ToDecimal(schid)
                        //&& s.Option_Id == 2   s.Nav_Check != 2
                        // || s.Scheme_Id == 2554 // for growth option & DSP BlackRock Equity Fund added additionaly
                        // && s.Launch_Date != null
                        orderby s.Sch_Short_Name
                        select new
                        {
                            s.Sch_Short_Name,
                            s.Scheme_Id,
                            s.Launch_Date
                        }).Distinct();
                    DataTable dt2 = null;
                    if (scheme_name_1.Count() > 0)
                        dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();

                    dt = dt2.Copy();

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// This Function will Fetch Benchmark of the Scheme
        /// </summary>
        /// <param name="schid"></param>
        /// <returns>DataTable will contain Benchmark name and its id  </returns>

        public System.Data.DataTable FetchBenchMark(decimal schid)
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            try
            {

                using (var principl = new PrincipalCalcDataContext())
                {
                    //var index_name = (from t_index_masters in principl.T_INDEX_MASTERs
                    //                  where
                    //                    t_index_masters.INDEX_ID ==
                    //                      ((from t_schemes_indexes in principl.T_SCHEMES_INDEXes
                    //                        where
                    //                          t_schemes_indexes.SCHEME_ID ==
                    //                            ((from t_schemes_masters in principl.T_SCHEMES_MASTERs
                    //                              where
                    //                                t_schemes_masters.Scheme_Id == schid
                    //                              select new
                    //                              {
                    //                                  t_schemes_masters.Scheme_Id
                    //                              }).First().Scheme_Id)
                    //                        select new
                    //                        {
                    //                            t_schemes_indexes.INDEX_ID
                    //                        }).Take(1).First().INDEX_ID)// need to remove take 1
                    //                  select new
                    //                  {
                    //                      t_index_masters.INDEX_NAME,
                    //                      t_index_masters.INDEX_ID
                    //                  }).ToDataTable();


                    var index_name = (from t_index_masters in principl.T_INDEX_MASTERs
                                      where

                                          ((from t_schemes_indexes in principl.T_SCHEMES_INDEXes
                                            where
                                              t_schemes_indexes.SCHEME_ID ==
                                                ((from t_schemes_masters in principl.T_SCHEMES_MASTERs
                                                  where
                                                    t_schemes_masters.Scheme_Id == schid
                                                  select new
                                                  {
                                                      t_schemes_masters.Scheme_Id
                                                  }).First().Scheme_Id)
                                            select t_schemes_indexes.INDEX_ID
                                            )).Contains(t_index_masters.INDEX_ID)// need to remove take 1
                                      select new
                                      {
                                          t_index_masters.INDEX_NAME,
                                          t_index_masters.INDEX_ID
                                      }).ToDataTable();

                    dt = index_name.Copy();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }



        #endregion

        #region : Calculatimon Method

        /// <summary>
        /// This Function will Show Result if SIP is Selected
        /// </summary>

        public void CalculateReturn()
        {
            #region Initialize
            DateTime sipStartDate, sipEndDate, sipAsonDate, schmStartDate, tempDate, allotDate, investmentDate, schmInitialDate;
            string strsql = String.Empty, schmeId = string.Empty, indexId = string.Empty, Colstr = string.Empty, daydiffCol = string.Empty;
            string strFrequency, strInvestorType;
            TimeSpan tmspan;
            DataTable dt = new DataTable();
            DataSet dset = new DataSet();
            DataTable SipDtable1, SipDtable2;
            int daydiff;
            double dblSIPamt;
            SqlCommand cmdScheme = null, cmdIndex = null;
            int val = 0, SIP_date;
            double totalInvestAmount, presntInvestValue, schemeReturnOneTime;
            DataTable datatbleAbsolute = new DataTable("resdtble");
            DataTable datatbleCagr = new DataTable();
            DataTable dtSchemeAbsolute = new DataTable();
            DataTable dtSchemeCagr = new DataTable();
            DataTable dttblIndxAbsolte = new DataTable();
            DataTable dttblIndxCagr = new DataTable();
            DataTable dtIndexAbsolute = new DataTable();
            DataTable dtIndexCagr = new DataTable();
            string srollinprd = "D,0 Si";
            string strRollingPeriodin = "1 YYYY,3 YYYY,5 YYYY,0 Si";
            string strRollingPeriod = "1 YYYY,3 YYYY,5 YYYY,7 YYYY,10 YYYY,";
            double? daysDiffValAbs = null, daysDiffValCagr = null;
            int initialFlag = 0, fundId;
            double dblSIPINtialAmnt = 0;
            schmInitialDate = new DateTime(1990, 01, 01);




            // Colstr = "Date#NAV#Units#CashFlow(scheme)#CashFlow(Index)#Amount#SIP Value#Index#Index Value";
            conn.ConnectionString = connstr;


            #endregion


            try
            {
                #region Set Region
                //sipStartDate = Convert.ToDateTime(txtfromDate.Text);
                //sipEndDate = Convert.ToDateTime(txtToDate.Text);
                //sipAsonDate = Convert.ToDateTime(txtvalason.Text);

                sipStartDate = new DateTime(Convert.ToInt16(txtfromDate.Text.Split('/')[2]),
                                      Convert.ToInt16(txtfromDate.Text.Split('/')[1]),
                                      Convert.ToInt16(txtfromDate.Text.Split('/')[0]));

                sipEndDate = new DateTime(Convert.ToInt16(txtToDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[0]));

                sipAsonDate = new DateTime(Convert.ToInt16(txtvalason.Text.Split('/')[2]),
                                     Convert.ToInt16(txtvalason.Text.Split('/')[1]),
                                     Convert.ToInt16(txtvalason.Text.Split('/')[0]));




                schmeId = ddlscheme.SelectedItem.Value;
                dblSIPamt = Convert.ToDouble(txtinstall.Text);
                //strFrequency = txtddPeriod_SIP.Text;
                strFrequency = ddPeriod_SIP.SelectedItem.Text;
                strInvestorType = "Individual/Huf";

                if (ViewState["INDEX_ID"] != null)
                    indexId = ViewState["INDEX_ID"].ToString();
                else
                {
                    // indexId = ddlsipbnmark.SelectedItem.Value;
                    // indexId = ddlsipbnmark.SelectedItem.Value;
                }

                #endregion

                #region Inception Date


                using (var principl = new PrincipalCalcDataContext())
                {
                    var alotdate = from ind in principl.T_SCHEMES_MASTERs
                                   where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                   select ind.Launch_Date;

                    schmStartDate = Convert.ToDateTime(alotdate.Single().ToString());
                    ViewState["schmStartDate"] = schmStartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var FundManager = (from fd in principl.T_FUND_MANAGERs
                                       join
                                    cfm in principl.T_CURRENT_FUND_MANAGERs on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                       join
                                       fms in principl.T_FUND_MASTERs on cfm.FUND_ID equals fms.FUND_ID
                                       join
                                       sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                       where
                                       sm.Scheme_Id == Convert.ToDecimal(schmeId) && cfm.LATEST_FUNDMAN == true
                                       select new
                                       {
                                           fd.FUND_MANAGER_NAME
                                       }).Distinct().ToArray();//fd.FUND_MANAGER_NAME;

                    string FundmanegerText = string.Empty;


                    var FundName = from fms in principl.T_FUND_MASTERs
                                   join
                                   sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                   where
                                   sm.Scheme_Id == Convert.ToDecimal(schmeId)
                                   select new
                                   {
                                       fms.FUND_NAME
                                   };

                    if (FundName != null && FundName.Count() == 1)
                    {
                        ViewState["FundName"] = FundName.Single().FUND_NAME.ToString();
                    }

                    if (FundManager.Count() > 0)
                    {
                        foreach (var fn in FundManager.AsEnumerable())
                        {
                            FundmanegerText += fn.FUND_MANAGER_NAME.ToString() + " , ";
                        }
                        FundmanegerText = FundmanegerText.TrimEnd(' ', ',');
                        ViewState["FundmanegerText"] = FundmanegerText;
                    }

                    var lastAvailbleNavDate = (from ind in principl.T_NAV_DIVs
                                               where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                               select ind.Nav_Date).Max();

                    var NeardateToSipEnddate = (from nd in principl.T_NAV_DIVs
                                                where nd.Scheme_Id == Convert.ToDecimal(schmeId)
                                                && nd.Nav_Date.Value <= sipEndDate
                                                select nd.Nav_Date).Max();

                    if (NeardateToSipEnddate.HasValue)
                    {
                        sipEndDate = Convert.ToDateTime(NeardateToSipEnddate);
                    }


                    if (lastAvailbleNavDate != null)
                    {
                        DateTime LastNavDate;
                        LastNavDate = Convert.ToDateTime(lastAvailbleNavDate);

                        tmspan = LastNavDate.Subtract(sipAsonDate);
                        if (tmspan.Days < 0)
                        {
                            //Response.Write(@"<script>alert(""Value as on date is not available for the scheme on " + sipAsonDate.ToShortDateString() + @".."")</script>");
                            sipAsonDate = LastNavDate;
                            //return;
                        }

                        tmspan = sipAsonDate.Subtract(sipEndDate);

                        if (tmspan.Days < 0)
                        {
                            // sipEndDate = sipAsonDate;
                            sipAsonDate = sipEndDate;
                        }

                    }

                }


                List<string> objFundDescList = new List<string>();
                objFundDescList.AddRange(GetFundDesc(schmeId));

                if (objFundDescList.Count > 0)
                {
                    ViewState["FundDesc1"] = objFundDescList[0].ToString();
                    ViewState["FundDesc2"] = objFundDescList[1].ToString();
                    ViewState["FundDesc3"] = objFundDescList[2].ToString();
                }



                tmspan = schmStartDate.Subtract(sipStartDate);
                if (tmspan.Days > 0)
                {
                    Response.Write(@"<script>alert(""From Date cannot be Greater than Inception Date of the scheme which is  " + schmStartDate.ToShortDateString() + @".."")</script>");
                    return;
                }

                tmspan = schmStartDate.Subtract(sipAsonDate);


                #endregion

                #region Assigned value and Validation
                //if (ddlsipbnmark.SelectedItem.Text == "35% BSE Oil & Gas, 30% BSE Metals, 35% MSCI World Energy (net and expressed in INR)" || ddlsipbnmark.SelectedItem.Text == "70% MSCI World Energy (Net), 30% MSCI World (Net)")
                //{
                //    flgIsCompositeIndex = true;
                //}
                int PrdVal = 1;
                if (ddPeriod_SIP.SelectedItem.Value == "Monthly")
                {
                    PrdVal = 1;
                }
                else if (ddPeriod_SIP.SelectedItem.Value == "Quarterly")
                {
                    PrdVal = 3;
                }
                else
                {
                    return;
                }


                if (PrdVal == 1)
                {

                    if (schmeId.ToString().Trim() == "2631" || schmeId.ToString().Trim() == "2632" || schmeId.ToString().Trim() == "16818" || schmeId.ToString().Trim() == "16819")
                    {
                        tempDate = sipEndDate.AddMonths(-5);//6
                        tmspan = tempDate.Subtract(sipStartDate);
                        daydiff = tmspan.Days;
                        if (daydiff < 0)
                        {
                            Response.Write("<script>alert(\"SIP is allowed for minimum 6 Installment\")</script>");
                            return;
                        }
                    }
                    else
                    {

                        tempDate = sipEndDate.AddMonths(-11);//12
                        tmspan = tempDate.Subtract(sipStartDate);
                        daydiff = tmspan.Days;
                        if (daydiff < 0)
                        {
                            Response.Write("<script>alert(\"SIP is allowed for minimum 12 Installment\")</script>");
                            return;
                        }
                    }

                }
                else if (PrdVal == 3)
                {
                    if (schmeId.ToString().Trim() == "2631" || schmeId.ToString().Trim() == "2632" || schmeId.ToString().Trim() == "16818" || schmeId.ToString().Trim() == "16819")
                    {
                        tempDate = sipEndDate.AddMonths(-17);
                        tmspan = tempDate.Subtract(sipStartDate);
                        daydiff = tmspan.Days;
                        if (daydiff < 0)
                        {
                            Response.Write("<script>alert(\"SIP is allowed for minimum 6 Installment\")</script>");
                            return;
                        }
                    }
                    else
                    {
                        tempDate = sipEndDate.AddMonths(-35);
                        tmspan = tempDate.Subtract(sipStartDate);
                        daydiff = tmspan.Days;
                        if (daydiff < 0)
                        {
                            Response.Write("<script>alert(\"SIP is allowed for minimum 12 Installment\")</script>");
                            return;
                        }
                    }
                }


                //switch (txtddSIPdate.Text)
                //{

                //    case "7":
                //        SIP_date = 7;
                //        break;
                //    case "14":
                //        SIP_date = 14;
                //        break;
                //    case "21":
                //        SIP_date = 21;
                //        break;
                //    case "28":
                //        SIP_date = 28;
                //        break;
                //}

                SIP_date = Convert.ToInt32(ddSIPdate.SelectedItem.Value);

                int tempInt = sipStartDate.Day;
                if (SIP_date < tempInt)
                {
                    if (sipStartDate.Month != 12)
                        investmentDate = new DateTime(sipStartDate.Year, sipStartDate.Month + 1, SIP_date);
                    else
                        investmentDate = new DateTime(sipStartDate.Year + 1, 1, SIP_date);
                }
                else if (SIP_date == tempInt)
                {
                    investmentDate = sipStartDate;
                }
                else
                {
                    investmentDate = new DateTime(sipStartDate.Year, sipStartDate.Month, SIP_date);
                }

                //dblSIPamt = Convert.ToDouble(txtinstall.Text);// sip amount



                #endregion


                #region SIP_CALCULATER sp

                if (ddlMode.SelectedItem.Value.ToUpper() == "SIP WITH INITIAL INVESTMENT")
                {
                    initialFlag = 1;
                    schmInitialDate = new DateTime(Convert.ToInt16(txtIniToDate.Text.Split('/')[2]),
                                 Convert.ToInt16(txtIniToDate.Text.Split('/')[1]),
                                 Convert.ToInt16(txtIniToDate.Text.Split('/')[0]));
                    ////
                    //modify initial date

                    schmInitialDate = GetLastDate(schmInitialDate, Convert.ToInt32(schmeId));

                    dblSIPINtialAmnt = Convert.ToDouble(txtiniAmount.Text);

                }


                using (var principl = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {


                    IMultipleResults datatble = principl.MFIE_SP_SIP_CALCULATER_CLIENT(schmeId, investmentDate, sipEndDate, sipAsonDate, dblSIPamt, strFrequency, strInvestorType, initialFlag, dblSIPINtialAmnt, schmInitialDate, 1, "Y");
                    var firstTable = datatble.GetResult<CalcReturnDataClient2>();
                    var secondTable = datatble.GetResult<CalcReturnDataClient>();
                    SipDtable1 = firstTable.ToDataTable();
                    SipDtable2 = secondTable.ToDataTable();
                    if (SipDtable2.Columns.Count > 1)
                        SipDtable2.Columns.RemoveAt(0);




                    //var allotdate = from ind in principl.T_SCHEMES_MASTER_Clients
                    //                where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                    //                select new
                    //                {
                    //                    LaunchDate = ind.Launch_Date
                    //                };
                    ////SIPSchDt.Text = "";
                    //// if (allotdate != null && allotdate.Count() > 0)                    
                    //allotDate = Convert.ToDateTime(allotdate.Single().LaunchDate);



                    var fundIds = from sm in principl.T_SCHEMES_MASTER_Clients
                                  where
                                      sm.Scheme_Id == Convert.ToDecimal(schmeId)
                                  select sm.Fund_Id;

                    fundId = Convert.ToInt32(fundIds.Single());

                    SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);
                    // SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);


                    #region Set Grid View



                    //#region remove Index Row 30/10/2012 again added 29/08/2013
                    //if (SipDtable2.Rows.Count == 2)
                    //{
                    //    SipDtable2.Rows.RemoveAt(1);
                    //}
                    //#endregion
                    // logic added 29/08/2013
                    if (SipDtable2.Rows.Count == 2)
                    {
                        if (SipDtable2.Columns.Contains("Total_unit"))
                            SipDtable2.Rows[1]["Total_unit"] = DBNull.Value;
                    }


                    #region Add Absolute Return //14.01.2013
                    SipDtable2.Columns.Add("ABSOLUTERETURN");

                    if (SipDtable2.Rows[0]["TOTAL_AMOUNT"] != DBNull.Value && SipDtable2.Rows[0]["PROFIT_SIP"] != DBNull.Value)
                    {
                        SipDtable2.Rows[0]["ABSOLUTERETURN"] = Math.Round(Convert.ToDouble(Convert.ToDouble(SipDtable2.Rows[0]["PROFIT_SIP"]) / Convert.ToDouble(SipDtable2.Rows[0]["TOTAL_AMOUNT"]) * 100), 2);
                    }

                    #endregion



                    gvFirstTable.DataSource = SipDtable2;
                    gvFirstTable.DataBind();

                    sipGridView.DataSource = SipDtable1;
                    sipGridView.DataBind();
                    

                    if (!gvFirstTable.Visible) gvFirstTable.Visible = true;
                    if (!sipGridView.Visible) sipGridView.Visible = true;

                    ViewState["sipDataTable"] = SipDtable1;
                    ViewState["gvFirstTableDT"] = SipDtable2;

                    #endregion

                    #region chart


                    #region Remove Bonus Row For Graph Calculation

                    if (SipDtable1.Rows.Count > 0)
                    {
                        if (SipDtable1.Rows.Count > 2)
                        {
                            for (int i = SipDtable1.Rows.Count - 1; i >= 0; i--)
                            {
                                if (SipDtable1.Rows[i]["DIVIDEND_BONUS"].ToString().Trim() != string.Empty && SipDtable1.Rows[i]["DIVIDEND_BONUS"].ToString().Trim() != "0")
                                {
                                    SipDtable1.Rows.RemoveAt(i);
                                }
                            }

                        }
                    }


                    #endregion

                    #region add extra column
                    if (SipDtable1.Rows.Count > 0)
                    {
                        if (SipDtable1.Rows.Count > 2)
                        {
                            // SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);
                            DataColumn dc = new DataColumn("Amount", System.Type.GetType("System.Double"));
                            SipDtable1.Columns.Add(dc);
                            SipDtable1.Columns.Add(new DataColumn("Index_Value_amount", System.Type.GetType("System.Double")));
                            // SipDtable1.Columns.Add(new DataColumn("Index_unit_cumulative", System.Type.GetType("System.Double")));
                            double result;
                            for (int i = 0; i < SipDtable1.Rows.Count - 1; i++)
                            {

                                if (i == 0)
                                {

                                    SipDtable1.Rows[i]["Amount"] = (-1) * Convert.ToDouble(SipDtable1.Rows[i]["Scheme_cashflow"]);

                                    if (Double.TryParse(SipDtable1.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"].ToString(), out result))
                                    {
                                        // SipDtable1.Rows[i]["Index_unit_cumulative"] = SipDtable1.Rows[i]["Index_Unit"];
                                        SipDtable1.Rows[i]["Index_Value_amount"] = Math.Round(Convert.ToDouble(SipDtable1.Rows[i]["Index_Value"]) * Convert.ToDouble(SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"]), 2);
                                    }
                                }
                                else
                                {
                                    SipDtable1.Rows[i]["Amount"] = Convert.ToDouble(SipDtable1.Rows[i - 1]["Amount"]) + (-1) * Convert.ToDouble(SipDtable1.Rows[i]["Scheme_cashflow"]);

                                    if (Double.TryParse(SipDtable1.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"].ToString(), out result))
                                    {
                                        // SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"] = Convert.ToDouble(SipDtable1.Rows[i]["Index_Unit"]) + Convert.ToDouble(SipDtable1.Rows[i - 1]["INDEX_UNIT_CUMULATIVE"]);
                                        SipDtable1.Rows[i]["Index_Value_amount"] = Math.Round(Convert.ToDouble(SipDtable1.Rows[i]["Index_Value"]) * Convert.ToDouble(SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"]), 2);
                                    }
                                    else
                                    {
                                        SipDtable1.Rows[i]["Index_Value_amount"] = SipDtable1.Rows[i - 1]["Index_Value_amount"];
                                    }
                                }
                            }
                        }
                    }


                    #endregion


                    #region delete last row (value as on date Row)

                    if (SipDtable1.Columns.Contains("SCHEME_CASHFLOW") && SipDtable1.Columns.Contains("CUMULATIVE_AMOUNT"))
                    {
                        SipDtable1.Rows[SipDtable1.Rows.Count - 1]["CUMULATIVE_AMOUNT"] = SipDtable1.Rows[SipDtable1.Rows.Count - 1]["SCHEME_CASHFLOW"];
                        SipDtable1.Rows[SipDtable1.Rows.Count - 1]["AMOUNT"] = SipDtable1.Rows[SipDtable1.Rows.Count - 2]["AMOUNT"];
                        //SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);
                    }
                    #endregion


                    #region : Remove Columns
                    for (int col = SipDtable1.Columns.Count - 1; col >= 0; col--)
                    {
                        DataColumn dc = SipDtable1.Columns[col];
                        if (dc.ColumnName.ToUpper() != "NAV_DATE" && dc.ColumnName.ToUpper() != "CUMULATIVE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT")// &&  dc.ColumnName.ToUpper() != "INDEX_VALUE_AMOUNT"
                        {
                            SipDtable1.Columns.RemoveAt(col);
                        }
                    }

                    #endregion

                    ViewState["dtchartView"] = SipDtable1;
                    //CallChart(SipDtable1);
                    //chrtResult.Visible = true;
                    //divshowChart.Visible = true;
                    #endregion

                }

                #endregion

                #region  Corpus QAUUM

                fetchCorpus(schmeId);

                #endregion









                # region calling sp

                tmspan = sipEndDate.Subtract(investmentDate);
                daydiff = tmspan.Days;
                srollinprd = daydiff.ToString() + " " + srollinprd;
                strRollingPeriod = strRollingPeriod + srollinprd;
                daydiffCol = daydiff.ToString() + " " + "Day";

                int InceptionDateDiff; string strRollingPeriodinMod = "1 YYYY,3 YYYY,5 YYYY,";
                tmspan = sipEndDate.Subtract(schmStartDate);
                InceptionDateDiff = tmspan.Days;
                strRollingPeriodinMod += InceptionDateDiff.ToString() + " D";





                //cmdsip = new SqlCommand("MFIE_SP_SIP_CALCULATER", conn);
                //cmdsip.CommandType = CommandType.StoredProcedure;
                //cmdsip.Parameters.Add(new SqlParameter("@Scheme_Ids", schmeId));
                //cmdsip.Parameters.Add(new SqlParameter("@Plan_Start_Date", sipStartDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                //cmdsip.Parameters.Add(new SqlParameter("@Plan_End_Date", sipEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                //cmdsip.Parameters.Add(new SqlParameter("@Report_As_On", sipAsonDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                //cmdsip.Parameters.Add(new SqlParameter("@SIP_Amt", dblSIPamt));
                //cmdsip.Parameters.Add(new SqlParameter("@Frequency", strFrequency));
                //cmdsip.Parameters.Add(new SqlParameter("@Dividend_Type", strInvestorType));
                //cmdsip.Parameters.Add(new SqlParameter("@Initial_Flage", val));
                //cmdsip.Parameters.Add(new SqlParameter("@Initial_Amount", val));
                //cmdsip.Parameters.Add(new SqlParameter("@Initial_Date", ""));
                //cmdsip.Parameters.Add(new SqlParameter("@Index_Flage", tstval));
                ////cmdsip.Parameters.Add(new SqlParameter("@OPTIONAL_RET_FLAG","y"));


                //SqlDataAdapter da = new SqlDataAdapter(cmdsip);
                //da.Fill(dset);

                //calling the sp to get Scheme Absolute return
                cmdScheme = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN", conn);
                cmdScheme.CommandType = CommandType.StoredProcedure;
                cmdScheme.CommandTimeout = 2000;
                cmdScheme.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
                cmdScheme.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                cmdScheme.Parameters.Add(new SqlParameter("@DateFrom", ""));
                cmdScheme.Parameters.Add(new SqlParameter("@DateTo", sipEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdScheme.Parameters.Add(new SqlParameter("@RoundTill", 1));
                cmdScheme.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
                cmdScheme.Parameters.Add(new SqlParameter("@RollingPeriod", val));
                cmdScheme.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                cmdScheme.Parameters.Add(new SqlParameter("@RollingFrequency", val));
                cmdScheme.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                cmdScheme.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmdScheme;
                da.Fill(dtSchemeAbsolute);




                cmdIndex = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
                cmdIndex.CommandType = CommandType.StoredProcedure;
                cmdIndex.CommandTimeout = 2000;
                cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
                cmdIndex.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                cmdIndex.Parameters.Add(new SqlParameter("@DateFrom", ""));
                cmdIndex.Parameters.Add(new SqlParameter("@DateTo", sipEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdIndex.Parameters.Add(new SqlParameter("@RoundTill", 1));
                // cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodinMod));
                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
                cmdIndex.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                cmdIndex.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



                da.SelectCommand = cmdIndex;
                da.Fill(dtIndexAbsolute);




                #endregion

                resultDiv.Visible = true;
                //btnExcelCalculation.Visible = true;



                #region Data Calculation


                if (dtSchemeAbsolute != null && dtSchemeAbsolute.Rows.Count > 0)
                {
                    if (dtSchemeAbsolute.Columns.Contains("SCHEME_ID"))
                        dtSchemeAbsolute.Columns.Remove("SCHEME_ID");
                    if (dtSchemeAbsolute.Columns.Contains(daydiffCol))
                        dtSchemeAbsolute.Columns.Remove(daydiffCol);
                }

                //if (dtSchemeCagr != null && dtSchemeCagr.Rows.Count > 0)
                //{
                //    if (dtSchemeCagr.Columns.Contains("SCHEME_ID"))
                //        dtSchemeCagr.Columns.Remove("SCHEME_ID");
                //    if (dtSchemeCagr.Columns.Contains(daydiffCol))
                //        dtSchemeCagr.Columns.Remove(daydiffCol);
                //}


                // dtSchemeAbsolute.Rows.Add(dtSchemeCagr.Rows[0].ItemArray);

                DataTable tempTable = new DataTable();
                tempTable = dtSchemeAbsolute.Copy();



                if (dtIndexAbsolute != null && dtIndexAbsolute.Rows.Count > 0)
                {
                    if (dtIndexAbsolute.Columns.Contains("INDEX_ID"))
                        dtIndexAbsolute.Columns.Remove("INDEX_ID");
                    if (dtIndexAbsolute.Columns.Contains("INDEX_TYPE"))
                        dtIndexAbsolute.Columns.Remove("INDEX_TYPE");
                    if (dtIndexAbsolute.Columns.Contains(daydiffCol))
                        dtIndexAbsolute.Columns.Remove(daydiffCol);

                    //dtSchemeAbsolute.Rows.Add(dtIndexAbsolute.Rows[0].ItemArray);
                }


                //if (dtIndexCagr != null && dtIndexCagr.Rows.Count > 0)
                //{
                //    if (dtIndexCagr.Columns.Contains("INDEX_ID"))
                //        dtIndexCagr.Columns.Remove("INDEX_ID");
                //    if (dtIndexCagr.Columns.Contains("INDEX_TYPE"))
                //        dtIndexCagr.Columns.Remove("INDEX_TYPE");
                //    if (dtIndexCagr.Columns.Contains(daydiffCol))
                //        dtIndexCagr.Columns.Remove(daydiffCol);


                //    //dtSchemeCagr.Rows.Add(dtIndexCagr.Rows[0].ItemArray);
                //}

                //dtIndexAbsolute.Rows.Add(dtIndexCagr.Rows[0].ItemArray);

                tempTable.Rows.Add(dtIndexAbsolute.Rows[0].ItemArray);
                //tempTable.Rows.Add(dtIndexCagr.Rows[0].ItemArray);
                tempTable.Columns.Add("Type");
                tempTable.Columns["Type"].SetOrdinal(0);



                //if (tempTable.Rows.Count == 4)
                //{
                //    tempTable.Rows[0][0] = tempTable.Rows[2][0] = "Absolute";
                //    tempTable.Rows[1][0] = tempTable.Rows[3][0] = "CAGR";
                //}

                finalResultdt = tempTable.Copy();// copy in global dt
                ViewState["finalResultdtble"] = finalResultdt;
                //tempTable.Rows.RemoveAt(2); tempTable.Rows.RemoveAt(2);
                finalResultdtwobenchmark = tempTable.Copy();
                ViewState["finalResultdtwobenchmark"] = finalResultdtwobenchmark;

                if (tempTable != null && tempTable.Rows.Count == 2)
                {
                    GridViewSIPResult.DataSource = tempTable;
                    GridViewSIPResult.DataBind();


                    ViewState["GridViewSIPResult"] = tempTable;
                    //lblNote.Visible = true;
                }


                Double CompundReturnDayVal = 0;

                if (daysDiffValAbs != null)
                {
                    if (daydiff > 365)
                        CompundReturnDayVal = dblSIPamt * Math.Pow(1 + (double)daysDiffValAbs / 100, Math.Round((double)daydiff / 365, 2));
                    else
                        CompundReturnDayVal = dblSIPamt + dblSIPamt * (double)daysDiffValAbs / 100;
                    CompundReturnDayVal = Math.Round(CompundReturnDayVal, 2);
                }

                //if (CompundReturnDayVal != 0)
                //{
                //    lblreturnAmount.Text = "* On " + txtfromDate.Text + " you had invested Rs " + txtinvested.Text + ". By " + txtToDate.Text + " the value of this investment would be Rs" + CompundReturnDayVal.ToString();
                //}

                #endregion

                #region DataBind

                lblInvestment.Text = "Investment amount per month : <b>Rs " + txtinstall.Text + "</b>";
                if (SipDtable2.Rows.Count > 0)
                {
                    totalInvestAmount = Math.Round(Convert.ToDouble(SipDtable2.Rows[0]["TOTAL_AMOUNT"]), 2);
                    lblTotalInvst.Text = "Total Investment Amount : <b>Rs " + totalInvestAmount.ToString() + "</b>";
                    ViewState["totalInvestedAmount"] = totalInvestAmount;//set viewstate


                    if (SipDtable2.Rows[0]["PRESENT_VALUE"].ToString() != "N/A" && Convert.ToDouble(SipDtable2.Rows[0]["PRESENT_VALUE"]) != 0.0)
                    {
                        presntInvestValue = Math.Round(Convert.ToDouble(SipDtable2.Rows[0]["PRESENT_VALUE"]), 2);
                        daysDiffValAbs = Math.Round(((double)(presntInvestValue - totalInvestAmount) / totalInvestAmount) * 100, 2);


                        lblInvstvalue.Text = "On " + txtvalason.Text + ", the Scheme value of your total investment <b>Rs " + totalInvestAmount.ToString() + "</b> would be <b>Rs  " + presntInvestValue.ToString() + "</b>";

                        ViewState["presentInvestValue"] = presntInvestValue;// set viewstate

                    }
                    else
                        lblInvstvalue.Text = "On " + txtvalason.Text + ", the Scheme value of your total investment <b>Rs " + totalInvestAmount.ToString() + "</b> would be Rs N/A";


                    if (SipDtable2.Rows[0]["YIELD"].ToString() != "N/A" && Convert.ToDouble(SipDtable2.Rows[0]["YIELD"]) != 0.0)
                    {
                        daysDiffValCagr = Math.Round(Convert.ToDouble(SipDtable2.Rows[0]["YIELD"]), 2);
                    }




                    lblAbsoluteReturn.Text = "Absolute return of " + ddlscheme.SelectedItem.Text + "  from " + txtfromDate.Text + " to " + txtToDate.Text + " is <b>";
                    lblAbsoluteReturn.Text += daysDiffValAbs == null ? "N/A" : daysDiffValAbs.ToString() + " % </b>";

                    lblCagrReturn.Text = "XIRR return from " + ddlscheme.SelectedItem.Text + "  " + txtfromDate.Text + " to " + txtToDate.Text + " is <b>";
                    lblCagrReturn.Text += daysDiffValCagr == null ? "N/A" : daysDiffValCagr.ToString() + " % </b>";


                    if (SipDtable2.Rows[0]["PROFIT_ONETIME"].ToString() != "N/A")
                    {
                        schemeReturnOneTime = Math.Round(Convert.ToDouble(SipDtable2.Rows[0]["PROFIT_ONETIME"]), 2);
                        schemeReturnOneTime += totalInvestAmount;
                        lblIfInvested.Text = "Had you invested <b>Rs " + totalInvestAmount.ToString() + "</b> at " + txtfromDate.Text + ", the total value of this investment at " + txtvalason.Text + " in Scheme would have become <b>Rs " + schemeReturnOneTime.ToString() + "</b>";
                    }
                    else
                        lblIfInvested.Text = "Had you invested <b>Rs " + totalInvestAmount.ToString() + "</b> at " + txtfromDate.Text + ", the total value of this investment at " + txtvalason.Text + " in Scheme would have become N/A";

                    //lblIfInvested.Visible = false;

                }

                #endregion

                if (!LSDisc.Visible)
                    LSDisc.Visible = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }

        }

        private List<string> GetFundDesc(string schmeId)
        {
            List<string> ListFundDesc = new List<string>();
            using (var iframeClient = new SIP_ClientDataContext())
            {

                var FundDesc = (from fm in iframeClient.T_FUND_MASTER_clients
                                join sm in iframeClient.T_SCHEMES_MASTER_Clients
                                on fm.FUND_ID equals sm.Fund_Id
                                join fcm in iframeClient.T_SCHEME_INFO_FUND_COLOR_MASTs
                               on fm.FUND_COLOR_MAST_ID equals fcm.ID
                                where sm.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    fm.FUND_OBJECT,
                                    fm.FUND_ID,
                                    fcm.DENOMINATE
                                }).FirstOrDefault();

                if (FundDesc != null)
                {
                    ListFundDesc.Add(FundDesc.FUND_OBJECT);
                    ListFundDesc.Add(FundDesc.FUND_ID.ToString());
                    ListFundDesc.Add(FundDesc.DENOMINATE);
                    // ListFundDesc.Add(Convert.ToString(FundDesc.COLOR_CODE));
                }

            }
            return ListFundDesc;
        }
        /// <summary>
        /// This Function will Show Result if Lumpsum is Selected
        /// </summary>

        public void CalculateReturnLumpSum()
        {
            //if (divTab.Visible)
            divTab.Visible = true;
            if (gvFirstTable.Visible) gvFirstTable.Visible = false;
            //resultDivLS.Visible = false;
            PerformanceReturn();
            lnkTab1.Visible = false; lnkTab2.Visible = false;
        }

        /// <summary>
        /// This Fuction will populate Chart
        /// </summary>
        /// 
        public void CallChartFromView2()
        {
            if (ViewState["dtchartView"] != null)
            {
                DataTable dtChrt = ViewState["dtchartView"] as DataTable;
                if (dtChrt.Rows.Count > 1)
                    CallChart(dtChrt, chrtResult);
            }
        }


        public void CallChartFromView()
        {

            if (ViewState["dtchartView"] != null)
            {
                DataTable dtChrt = ViewState["dtchartView"] as DataTable;
                if (dtChrt.Rows.Count > 1)
                    CallChart(dtChrt, chrtResult);
            }
        }
        public void CallChart(DataTable SipDtable, System.Web.UI.DataVisualization.Charting.Chart chrtResult)
        {

            DataTable dtChart = new DataTable("dtChart");
            dtChart = SipDtable.Copy();
            // dtChart.Rows.RemoveAt(dtChart.Rows.Count - 1);

            string dateColumn = dtChart.Columns[0].ColumnName.ToUpper();
            string compareColumn = "AMOUNT";
            if (dtChart.Columns.Count >= 2)
            {
                compareColumn = dtChart.Columns[2].ColumnName.ToUpper();
            }

            BindDataTableToChart(dtChart, dateColumn, chrtResult, compareColumn);
            //BindDataTableToBarChart(dtChart, dateColumn, chrtResult, compareColumn);
        }

        /// <summary>
        /// This Function will Fetch Corpus of the Fund
        /// </summary>
        /// <param name="schmeId"></param>
        /// 

        public void fetchCorpus(string schmeId)
        {
            if (Convert.ToInt32(schmeId) > 0)
            {
                DataTable dtTemp = FindCorpusVal(Convert.ToInt32(schmeId));
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {

                    if (dtTemp.Rows[0]["Fund_Size"] != DBNull.Value)
                    {
                        ViewState["CorpusFund"] = Convert.ToDouble(dtTemp.Rows[0]["Fund_Size"]);
                    }
                }
            }
        }

        public DataTable CalculateHistPerf(DateTime EndDate)
        {
            #region :Historical Performance

            conn.ConnectionString = connstr;
            string strRollingPeriodin = "1 YYYY,3 YYYY,5 YYYY,0 Si";
            int val = 0; DateTime SchInception;
            string schmeId = ddlscheme.SelectedItem.Value;
            string indexId = string.Empty;
            if (ViewState["INDEX_ID"] != null)
                indexId = ViewState["INDEX_ID"].ToString();
            //else
            //    indexId = ddlsipbnmark.SelectedItem.Value;


            # region calling sp


            DataTable dtSchemeAbsolute = new DataTable();
            DataTable dtIndexAbsolute = new DataTable();

            SqlCommand cmd;


            //calling the sp to get Scheme Absolute return
            cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 2000;
            cmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
            cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
            cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
            cmd.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
            cmd.Parameters.Add(new SqlParameter("@RollingPeriod", val));
            cmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
            cmd.Parameters.Add(new SqlParameter("@RollingFrequency", val));
            cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
            cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtSchemeAbsolute);

            //if (ViewState["schmStartDate"] != null)
            //    SchInception = Convert.ToDateTime(ViewState["schmStartDate"].ToString());
            //else
            //{
                using (var principl = new PrincipalCalcDataContext())
                {
                    var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                    where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                    select new
                                    {
                                        LaunchDate = ind.Launch_Date
                                    };

                    SchInception = Convert.ToDateTime(allotdate.Single().LaunchDate.ToString());

                }
            //};


            int InceptionDateDiff; string strRollingPeriodinMod = "1 YYYY,3 YYYY,5 YYYY,";
            TimeSpan tmspan = EndDate.Subtract(SchInception);
            InceptionDateDiff = tmspan.Days;
            strRollingPeriodinMod += InceptionDateDiff.ToString() + " D";

            cmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 2000;
            cmd.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
            cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
            cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
            //cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodinMod));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
            cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
            cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



            da.SelectCommand = cmd;
            da.Fill(dtIndexAbsolute);



            #endregion


            if (dtSchemeAbsolute != null && dtSchemeAbsolute.Rows.Count > 0)
            {
                if (dtSchemeAbsolute.Columns.Contains("SCHEME_ID"))
                    dtSchemeAbsolute.Columns.Remove("SCHEME_ID");

            }



            if (dtIndexAbsolute != null && dtIndexAbsolute.Rows.Count > 0)
            {
                if (dtIndexAbsolute.Columns.Contains("INDEX_ID"))
                    dtIndexAbsolute.Columns.Remove("INDEX_ID");
                if (dtIndexAbsolute.Columns.Contains("INDEX_TYPE"))
                    dtIndexAbsolute.Columns.Remove("INDEX_TYPE");

                dtSchemeAbsolute.Rows.Add(dtIndexAbsolute.Rows[0].ItemArray);
            }



            return dtSchemeAbsolute;



            #endregion
        }


        public DataTable CalculateHistPerf(DateTime EndDate, string schmeId, string indexId)
        {
            #region :Historical Performance

            conn.ConnectionString = connstr;
            string strRollingPeriodin = "1 YYYY,3 YYYY,5 YYYY,0 Si";
            int val = 0;
            DateTime SchInception;
            # region calling sp


            DataTable dtSchemeAbsolute = new DataTable();
            DataTable dtIndexAbsolute = new DataTable();

            SqlCommand cmd;


            //calling the sp to get Scheme Absolute return
            cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 2000;
            cmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
            cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
            cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
            cmd.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
            cmd.Parameters.Add(new SqlParameter("@RollingPeriod", val));
            cmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
            cmd.Parameters.Add(new SqlParameter("@RollingFrequency", val));
            cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
            cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtSchemeAbsolute);


            using (var principl = new PrincipalCalcDataContext())
            {
                var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    LaunchDate = ind.Launch_Date
                                };

                SchInception = Convert.ToDateTime(allotdate.Single().LaunchDate.ToString());

            }



            int InceptionDateDiff; string strRollingPeriodinMod = "1 YYYY,3 YYYY,5 YYYY,";
            TimeSpan tmspan = EndDate.Subtract(SchInception);
            InceptionDateDiff = tmspan.Days;
            strRollingPeriodinMod += InceptionDateDiff.ToString() + " D";


            cmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 2000;
            cmd.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
            cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
            cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
            //  cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodinMod));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
            cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
            cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



            da.SelectCommand = cmd;
            da.Fill(dtIndexAbsolute);



            #endregion


            if (dtSchemeAbsolute != null && dtSchemeAbsolute.Rows.Count > 0)
            {
                if (dtSchemeAbsolute.Columns.Contains("SCHEME_ID"))
                    dtSchemeAbsolute.Columns.Remove("SCHEME_ID");

            }



            if (dtIndexAbsolute != null && dtIndexAbsolute.Rows.Count > 0)
            {
                if (dtIndexAbsolute.Columns.Contains("INDEX_ID"))
                    dtIndexAbsolute.Columns.Remove("INDEX_ID");
                if (dtIndexAbsolute.Columns.Contains("INDEX_TYPE"))
                    dtIndexAbsolute.Columns.Remove("INDEX_TYPE");

                dtSchemeAbsolute.Rows.Add(dtIndexAbsolute.Rows[0].ItemArray);
            }



            return dtSchemeAbsolute;



            #endregion
        }

        //public DataTable CalculateHistPerfSTPTO(DateTime EndDate)
        //{
        //    #region :Historical Performance

        //    conn.ConnectionString = connstr;
        //    string strRollingPeriodin = "1 YYYY,3 YYYY,5 YYYY,0 Si";
        //    int val = 0;
        //    string schmeId = ddlschtrto.SelectedItem.Value;

        //    # region calling sp
        //    DataTable dtSchemeAbsolute = new DataTable();
        //    SqlCommand cmd;

        //    //calling the sp to get Scheme Absolute return
        //    cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandTimeout = 2000;
        //    cmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
        //    cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
        //    cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
        //    cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        //    cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
        //    cmd.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
        //    cmd.Parameters.Add(new SqlParameter("@RollingPeriod", val));
        //    cmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
        //    cmd.Parameters.Add(new SqlParameter("@RollingFrequency", val));
        //    cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
        //    cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    da.Fill(dtSchemeAbsolute);
        //    #endregion


        //    if (dtSchemeAbsolute != null && dtSchemeAbsolute.Rows.Count > 0)
        //    {
        //        if (dtSchemeAbsolute.Columns.Contains("SCHEME_ID"))
        //            dtSchemeAbsolute.Columns.Remove("SCHEME_ID");
        //    }
        //    return dtSchemeAbsolute;

        //    #endregion
        //}

        /// <summary>
        /// This Function will Show Result for Lumpsum .
        /// </summary>

        private void PerformanceReturn()
        {
            #region initialize variable
            double? daysDiffValAbs = null, daysDiffVal = null, daysDiffValAbsIndex = null, daysDiffValINdex = null;
            DateTime _toDate, _fromDate, allotDate;
            string srollinprd = "D,0 Si";
            TimeSpan dateDiff2;
            double amountLs; int daydiff; string daydiffCol = string.Empty;
            SqlCommand cmd = null, cmdScheme = null, cmdIndex = null, cmdIndx = null;
            string strRollingPeriodin = string.Empty;
            strRollingPeriodin = "1 YYYY,3 YYYY,5 YYYY,0 Si";
            int settingSet = 2;//( <1 Abs >1 comp)
            string schmeId = ddlscheme.SelectedItem.Value;
            //string indexId = ddlsipbnmark.SelectedItem.Value;
            string indexId = ViewState["INDEX_ID"].ToString();



            _fromDate = new DateTime(Convert.ToInt16(txtLumpfromDate.Text.Split('/')[2]),
                                   Convert.ToInt16(txtLumpfromDate.Text.Split('/')[1]),
                                   Convert.ToInt16(txtLumpfromDate.Text.Split('/')[0]));

            _toDate = new DateTime(Convert.ToInt16(txtLumpToDate.Text.Split('/')[2]),
                                     Convert.ToInt16(txtLumpToDate.Text.Split('/')[1]),
                                     Convert.ToInt16(txtLumpToDate.Text.Split('/')[0]));





            amountLs = Convert.ToDouble(txtinstallLs.Text);

            dateDiff2 = _toDate.Subtract(_fromDate);
            daydiff = dateDiff2.Days;
            srollinprd = daydiff.ToString() + " " + srollinprd;
            daydiffCol = daydiff.ToString() + " " + "Day";



            using (var dspData = new SIP_ClientDataContext())
            {

                var strNature = (from natr in dspData.T_SCHEMES_NATURE_Clients
                                 where natr.Nature_ID == (
                                 from tfm in dspData.T_FUND_MASTER_clients
                                 where tfm.FUND_ID ==
                                 (from tsm in dspData.T_SCHEMES_MASTER_Clients
                                  where tsm.Scheme_Id == Convert.ToDecimal(schmeId)
                                  select new
                                  {
                                      tsm.Fund_Id
                                  }
                                 ).First().Fund_Id
                                 select new { tfm.NATURE_ID }).First().NATURE_ID
                                 select new
                                 {
                                     natr.Nature
                                 }).First().Nature;

                if (strNature.ToUpper() == "DEBT" || strNature.ToUpper() == "GILT" || strNature.ToUpper() == "LIQUID")
                {
                    //settingSet = 23;//remove comment if needed
                }
            }

            //if(daydiff>365)
            //    settingSet

            #endregion

            conn.ConnectionString = connstr;



            try
            {
                #region Datatable

                DataTable datatbleReturn = new DataTable("resdtble");
                DataTable dtScheme = new DataTable();
                DataTable dttblIndxReturn = new DataTable();
                DataTable dtIndex = new DataTable();

                int val = 0;
                #endregion


                #region Launch Date
                using (var principl = new PrincipalCalcDataContext())
                {
                    var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                    where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                    select new
                                    {
                                        LaunchDate = ind.Launch_Date
                                    };

                    allotDate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                    //returnSchDt.Text = "<b>" + allotDate.ToShortDateString() + "<b/>";

                    TimeSpan tmspan = allotDate.Subtract(_fromDate);
                    if (tmspan.Days > 0)
                    {
                        Response.Write(@"<script>alert(""From Date cannot be Greater than Inception Date of the scheme which is  " + allotDate.ToShortDateString() + @".."")</script>");
                        return;
                    }


                    ViewState["schmStartDate"] = allotDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var FundManager = (from fd in principl.T_FUND_MANAGERs
                                       join
                                    cfm in principl.T_CURRENT_FUND_MANAGERs on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                       join
                                       fms in principl.T_FUND_MASTERs on cfm.FUND_ID equals fms.FUND_ID
                                       join
                                       sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                       where
                                       sm.Scheme_Id == Convert.ToDecimal(schmeId) && cfm.LATEST_FUNDMAN == true
                                       select new
                                       {
                                           fd.FUND_MANAGER_NAME
                                       }).Distinct().ToArray();//fd.FUND_MANAGER_NAME;

                    string FundmanegerText = string.Empty;


                    var FundName = from fms in principl.T_FUND_MASTERs
                                   join
                                   sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                   where
                                   sm.Scheme_Id == Convert.ToDecimal(schmeId)
                                   select new
                                   {
                                       fms.FUND_NAME
                                   };

                    if (FundName != null && FundName.Count() == 1)
                    {
                        ViewState["FundName"] = FundName.Single().FUND_NAME.ToString();
                    }

                    if (FundManager.Count() > 0)
                    {
                        foreach (var fn in FundManager.AsEnumerable())
                        {
                            FundmanegerText += fn.FUND_MANAGER_NAME.ToString() + " , ";
                        }
                        FundmanegerText = FundmanegerText.TrimEnd(' ', ',');
                        ViewState["FundmanegerText"] = FundmanegerText;
                    }

                }

                List<string> objFundDescList = new List<string>();
                objFundDescList.AddRange(GetFundDesc(schmeId));

                if (objFundDescList.Count > 0)
                {
                    ViewState["FundDesc1"] = objFundDescList[0].ToString();
                    ViewState["FundDesc2"] = objFundDescList[1].ToString();
                    ViewState["FundDesc3"] = objFundDescList[2].ToString();
                }

                #endregion


                # region calling sp


                cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 2000;
                cmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
                cmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                cmd.Parameters.Add(new SqlParameter("@DateTo", _toDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                cmd.Parameters.Add(new SqlParameter("@RollingPeriodin", srollinprd));
                cmd.Parameters.Add(new SqlParameter("@RollingPeriod", val));
                cmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                cmd.Parameters.Add(new SqlParameter("@RollingFrequency", val));
                cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(datatbleReturn);

                cmdIndx = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
                cmdIndx.CommandType = CommandType.StoredProcedure;
                cmdIndx.CommandTimeout = 2000;
                cmdIndx.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
                cmdIndx.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                cmdIndx.Parameters.Add(new SqlParameter("@DateFrom", allotDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdIndx.Parameters.Add(new SqlParameter("@DateTo", _toDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdIndx.Parameters.Add(new SqlParameter("@RoundTill", 2));
                cmdIndx.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", srollinprd));
                cmdIndx.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
                cmdIndx.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                cmdIndx.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
                cmdIndx.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                cmdIndx.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

                da.SelectCommand = cmdIndx;
                da.Fill(dttblIndxReturn);




                cmdScheme = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN", conn);
                cmdScheme.CommandType = CommandType.StoredProcedure;
                cmdScheme.CommandTimeout = 2000;
                cmdScheme.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
                cmdScheme.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                cmdScheme.Parameters.Add(new SqlParameter("@DateFrom", ""));
                cmdScheme.Parameters.Add(new SqlParameter("@DateTo", _toDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdScheme.Parameters.Add(new SqlParameter("@RoundTill", 2));
                cmdScheme.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
                cmdScheme.Parameters.Add(new SqlParameter("@RollingPeriod", val));
                cmdScheme.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                cmdScheme.Parameters.Add(new SqlParameter("@RollingFrequency", val));
                cmdScheme.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                cmdScheme.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


                da.SelectCommand = cmdScheme;
                da.Fill(dtScheme);


                int InceptionDateDiff; string strRollingPeriodinMod = "1 YYYY,3 YYYY,5 YYYY,";
                dateDiff2 = _toDate.Subtract(allotDate);
                InceptionDateDiff = dateDiff2.Days;
                strRollingPeriodinMod += InceptionDateDiff.ToString() + " D";


                cmdIndex = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
                cmdIndex.CommandType = CommandType.StoredProcedure;
                cmdIndex.CommandTimeout = 2000;
                cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
                cmdIndex.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                cmdIndex.Parameters.Add(new SqlParameter("@DateFrom", ""));
                cmdIndex.Parameters.Add(new SqlParameter("@DateTo", _toDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdIndex.Parameters.Add(new SqlParameter("@RoundTill", 2));
                // cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodinMod));
                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
                cmdIndex.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                cmdIndex.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



                da.SelectCommand = cmdIndex;
                da.Fill(dtIndex);

                #endregion

                #region Data Calculation

                resultDiv.Visible = true;


                string returnText = "";

                //if (daydiff < 365)
                //{
                //    if (settingSet == 2)
                //        returnText = "Absolute";
                //    else if (settingSet == 23)
                //        returnText = "Simple Annualized";
                //}



                if (Convert.ToString(datatbleReturn.Rows[0][daydiffCol]) != "N/A")
                    daysDiffVal = Convert.ToDouble(datatbleReturn.Rows[0][daydiffCol]);

                //  lblReturn.Text = returnText + "Return of " + ddlscheme.SelectedItem.Text + " from  " + txtLumpfromDate.Text + " to " + txtLumpToDate.Text + " is <b>";
                //   lblReturn.Text += daysDiffVal == null ? "N/A" : daysDiffVal.ToString() + " % </b>";
                // lblReturn.Text = "<Strong>" + lblReturn.Text + "</Strong>";


                if (Convert.ToString(dttblIndxReturn.Rows[0][daydiffCol]) != "N/A")
                    daysDiffValINdex = Convert.ToDouble(dttblIndxReturn.Rows[0][daydiffCol]);

                // lblReturnIndex.Text = returnText + "Return of " + ddlsipbnmark.SelectedItem.Text + " from " + txtLumpfromDate.Text + " to " + txtLumpToDate.Text + " is <b>";
                //  lblReturnIndex.Text += daysDiffValINdex == null ? "N/A" : daysDiffValINdex.ToString() + " % </b>";
                // lblReturnIndex.Text = "<Strong>" + lblReturnIndex.Text + "</Strong>";



                if (dtScheme != null && dtScheme.Rows.Count > 0)
                {
                    if (dtScheme.Columns.Contains("SCHEME_ID"))
                        dtScheme.Columns.Remove("SCHEME_ID");
                }



                if (dtIndex != null && dtIndex.Rows.Count > 0)
                {
                    if (dtIndex.Columns.Contains("INDEX_ID"))
                        dtIndex.Columns.Remove("INDEX_ID");
                    if (dtIndex.Columns.Contains("INDEX_TYPE"))
                        dtIndex.Columns.Remove("INDEX_TYPE");

                }




                DataTable tempTable = new DataTable();
                tempTable = dtScheme.Copy();





                tempTable.Rows.Add(dtIndex.Rows[0].ItemArray);

                tempTable.Columns.Add("Type");
                tempTable.Columns["Type"].SetOrdinal(0);



                if (tempTable.Rows.Count == 4)
                {
                    tempTable.Rows[0][0] = tempTable.Rows[2][0] = "Absolute";
                    tempTable.Rows[1][0] = tempTable.Rows[3][0] = "CAGR";
                }

                finalResultdt = tempTable.Copy();// copy in global dt
                ViewState["finalResultdtble"] = finalResultdt;
                //   tempTable.Rows.RemoveAt(2); tempTable.Rows.RemoveAt(2);
                ViewState["finalResultdtbleWithoutBnch"] = tempTable;


                Double CompundReturnDayVal = 0, CompundReturnDayValIndex = 0;

                if (daysDiffVal != null)
                {
                    if (daydiff > 365)
                        CompundReturnDayVal = amountLs * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)daydiff / 365, 2));
                    else
                        CompundReturnDayVal = amountLs + amountLs * (double)daysDiffVal / 100;
                    CompundReturnDayVal = Math.Round(CompundReturnDayVal, 2);
                }


                if (daysDiffValINdex != null)
                {
                    if (daydiff > 365)
                        CompundReturnDayValIndex = amountLs * Math.Pow(1 + (double)daysDiffValINdex / 100, Math.Round((double)daydiff / 365, 2));
                    else
                        CompundReturnDayValIndex = amountLs + amountLs * (double)daysDiffValINdex / 100;
                    CompundReturnDayValIndex = Math.Round(CompundReturnDayValIndex, 2);
                }

                #endregion


               

               
                #region fetch corpus
                fetchCorpus(schmeId);
                #endregion

            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Others Method

        /// <summary>
        /// This Functon will call respective selected function
        /// </summary>
        private void ShowResult()
        {
            SetDefaultView();
            if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP WITH INITIAL INVESTMENT")
            {
                // SetDefaultView();
                if (!divTab.Visible) divTab.Visible = true;
                if (!gvFirstTable.Visible) gvFirstTable.Visible = true;
                resultDiv.Visible = false;
                CalculateReturn();
            }            
        }

        /// <summary>
        /// Select proper return Calculator on Screen
        /// </summary>

        private void callCalc()
        {
            string calc = Request.QueryString["return"];
            if (calc != null)
            {
                if (!string.IsNullOrEmpty(calc))
                {
                    //ddlMode.SelectedItem.Selected = false;
                    //if(calc.ToUpper() =="SWP")
                    switch (calc.ToUpper())
                    {
                        case "SWP":
                            //ddlMode.Items[2].Selected = true;
                            ddlMode.SelectedIndex = 3;
                            break;
                        case "LUMP SUM":
                            //ddlMode.Items[1].Selected = true;
                            ddlMode.SelectedIndex = 1;
                            break;
                        case "STP":
                            //ddlMode.Items[3].Selected = true;
                            ddlMode.SelectedIndex = 4;
                            break;
                        default:
                            //ddlMode.Items[0].Selected = true;
                            ddlMode.SelectedIndex = 0;
                            break;
                    }

                }
            }
            ShowRelativeDiv();
        }

        /// <summary>
        /// Set Credential of Distributer
        /// </summary>

        private void SetDistributorCredential()
        {
            bool success = false;
            string sql = string.Empty;

            string Preparedby = string.Empty;
            string PreparedFor = string.Empty;
            string Mobile = string.Empty;
            string Email = string.Empty;
            string InvestMode = string.Empty;
            string ArnNo = string.Empty;

            SqlParameter preparedBy_Param = new SqlParameter();
            SqlParameter mobile_ph_Param = new SqlParameter();
            SqlParameter email_id_Param = new SqlParameter();
            SqlParameter prepared_for_Param = new SqlParameter();
            SqlParameter invest_mode_Param = new SqlParameter();
            SqlParameter arn_no_Param = new SqlParameter();

            if (txtPreparedby.Text.Length > 0)
            {
                ViewState["Preparedby"] = txtPreparedby.Text;
                Preparedby = txtPreparedby.Text.Trim();
            }

            if (txtPreparedFor.Text.Length > 0)
            {
                ViewState["PreparedFor"] = txtPreparedFor.Text;
                PreparedFor = txtPreparedFor.Text.Trim();

            }

            if (txtMobile.Text.Length > 0)
            {
                ViewState["Mobile"] = txtMobile.Text;
                Mobile = txtMobile.Text.Trim();
            }

            if (txtEmail.Text.Length > 0)
            {
                ViewState["Email"] = txtEmail.Text;
                Email = txtEmail.Text.Trim();
            }

            if (txtArn.Text.Length > 0)
            {
                ViewState["ArnNo"] = txtArn.Text;
                ArnNo = txtArn.Text.Trim();
            }            
            
        }


        /// <summary>
        /// This function will show Corresponding  controll for selected Calculator
        /// </summary>

        protected void ShowRelativeDiv()
        {

            trInception.Visible = false;

            SIPSchDt.Text = "";
            txtddlsipbnmark.Text = "";
            
                lnkTab1.Visible = true; lnkTab2.Visible = true;                
                trSipInvst.Visible = true; trLumpInvst.Visible = false; trInitialInvst.Visible = false;
                trCategory.Visible = true; lblSchemeName.Text = "Scheme Name"; trTransferTo.Visible = false;
                trBenchmark.Visible = true; lblInstallmentAmt.Text = "Installment Amount (Rs.)";
                lblDiffDate.Text = "SIP Date";
                //trTransferWithdrawal.Visible = false;
                GridViewSIPResult.Visible = true;
                lblTransferWithdrawal.Visible = false; txtTransferWithdrawal.Visible = false;
                chrtResult.Visible = true;
                lblDisclaimer.Text = @" While comparing the performance of investments made through SIPs with the respective
                                benchmark of the scheme, the value of the Index on the days when investment is made
                                is assumed to be the price of one unit. The “since inception” returns signify returns realized since the launch of the scheme. 
                                The returns calculated do not take into account Entry Load/ Exit Load. Hence actual “Returns”
                                may be lower.";

            FillDropdownScheme();

            resultDiv.Visible = false;
            LSDisc.Visible = false;

            //if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
            //    LSDisc1.Visible = true;
            //else
            //    LSDisc1.Visible = false;
        }

        /// <summary>
        /// Reset all Controlls
        /// </summary>

        public void Reset()
        {
            ddlscheme.SelectedIndex = 0;
            if (ddlschtrto.SelectedIndex > -1)
                ddlschtrto.SelectedIndex = 0;
            ddPeriod_SIP.SelectedIndex = 0;
            ddSIPdate.SelectedIndex = 0;
            //ddlMode.SelectedIndex = 0;
            ddlNature.SelectedIndex = 0;
            txtinstall.Text = "";
            txtTransferWithdrawal.Text = "";
            txtfromDate.Text = "";
            txtToDate.Text = "";
            txtvalason.Text = "";
            SIPSchDt.Text = ""; SIPSchDt2.Text = "";
            txtiniAmount.Text = "";
            txtIniToDate.Text = "";
            txtinstallLs.Text = "";
            txtLumpfromDate.Text = "";
            txtLumpToDate.Text = "";
            //ddlsipbnmark.Items.Clear();
            txtddlsipbnmark.Text = "";
            FillDropdownScheme();
        }

        public string TwoDecimal(string data)
        {
            string result = string.Empty;
            result = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(data))).ToString();
            return result;
        }

        public string totalProfit(string totalinivested, string withdraw, string present)
        {

            return (Convert.ToDouble(withdraw == "" ? "0" : withdraw) + Convert.ToDouble(present) - Convert.ToDouble(totalinivested)).ToString("n0");
        }


        public int SetInterval(double YaxisMax, double YaxisMin)
        {
            int interval = 500;
            int d = (int)(YaxisMax - YaxisMin) / 6;
            if (d % 10 != 0)
            {
                int y = 10;
                for (int i = 1; i <= (Convert.ToString(d % 10)).Count(); i++)
                {
                    y = y * 10;
                }
                d = d + y - (d % y);
                interval = d;
            }
            return interval;
        }

        /// <summary>
        /// This Function will Add Inception Date of corresponding Selected Scheme in dropdown  
        /// </summary>


        public void SetInceptionOnDropDown()
        {

            if ((ViewState["SchemeInception"] != null) && (ddlscheme.Items.Count > 0) && ddlscheme.SelectedIndex > 0)
            {

                Dictionary<string, string> SchemeInception = (Dictionary<string, string>)(ViewState["SchemeInception"]);


                for (int i = 0; i < ddlscheme.Items.Count; i++)
                {
                    string s = "";
                    if (SchemeInception.TryGetValue(ddlscheme.SelectedItem.Value, out s) && ddlscheme.Items[i].Selected == true)
                    {
                        ddlscheme.Items[i].Attributes.Add("title", s);
                    }
                }
            }

            if ((ViewState["SchemeInception2"] != null) && (ddlschtrto.Items.Count > 0) && ddlschtrto.SelectedIndex > 0)
            {
                Dictionary<string, string> SchemeInception2 = (Dictionary<string, string>)(ViewState["SchemeInception2"]);

                for (int i = 0; i < ddlschtrto.Items.Count; i++)
                {
                    string s = "";
                    if (SchemeInception2.TryGetValue(ddlschtrto.SelectedItem.Value, out s) && ddlschtrto.Items[i].Selected == true)
                    {
                        ddlschtrto.Items[i].Attributes.Add("title", s);
                    }
                }
            }

        }

        public DataTable FindCorpusVal(int _SchemeId)
        {
            DataTable dt = new DataTable();
            int _month;
            int _year;
            string sql = string.Empty;
            DateTime lastQtrDate;//= GetLastQuarterDates(DateTime.Today);
            conn.ConnectionString = connstr;

            sql = @"select top(1) record_date from T_AMFI_QUARTER_FUNDSIZE order by record_date desc";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                lastQtrDate = (DateTime)cmd.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;

            }
            _month = lastQtrDate.Month;
            _year = lastQtrDate.Year;

            //            sql = @"SELECT sum(corpus_per) FROM T_COM_POT A,T_COM_POT_DETAILS B WHERE 
            //                        A.PORT_ID=B.PORT_ID AND FUND_ID=@FUND_ID AND month(PORT_DATE)=@MONTH and year(PORT_DATE)=@YEAR 
            //                        group by FUND_ID";


            sql = @"SELECT CONVERT(VARCHAR,C.SCHEME_ID) AS SCHEME_ID,ROUND(SUM(A.FUND_SIZE),2) FUND_SIZE FROM T_AMFI_QUARTER_FUNDSIZE A,
                T_SCHEMES_MASTER B,T_SCHEMES_MASTER C 
                WHERE A.SCHEME_ID=B.SCHEME_ID AND B.FUND_ID=C.FUND_ID
                AND C.SCHEME_ID IN (@SCHEME_ID) AND MONTH(A.RECORD_DATE)=@MONTH AND YEAR(A.RECORD_DATE)=@YEAR  
                GROUP BY C.SCHEME_ID";

            SqlParameter schemeidParam = new SqlParameter();
            SqlParameter yearParam = new SqlParameter();
            SqlParameter monthParam = new SqlParameter();

            schemeidParam.ParameterName = "@SCHEME_ID";
            schemeidParam.SqlDbType = SqlDbType.Int;
            schemeidParam.Direction = ParameterDirection.Input;
            schemeidParam.Value = Convert.ToInt32(_SchemeId);

            monthParam.ParameterName = "@MONTH";
            monthParam.SqlDbType = SqlDbType.Int;
            monthParam.Direction = ParameterDirection.Input;
            monthParam.Value = Convert.ToInt32(_month);

            yearParam.ParameterName = "@YEAR";
            yearParam.SqlDbType = SqlDbType.Int;
            yearParam.Direction = ParameterDirection.Input;
            yearParam.Value = Convert.ToInt32(_year);


            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(schemeidParam);
                cmd.Parameters.Add(monthParam);
                cmd.Parameters.Add(yearParam);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
        }



        private DateTime GetLastQuarterDates(DateTime todate)
        {
            DateTime currentDate;//= DateTime.Today;
            currentDate = todate;
            int month = currentDate.Month;
            DateTime qtrenddate = currentDate;

            if (month >= 1 && month <= 3)
            {
                if (qtrenddate.Day == 31 && qtrenddate.Month == 3)
                    return qtrenddate;
                else
                    qtrenddate = new DateTime(currentDate.Year - 1, 12, 31);

            }
            if (month >= 4 && month <= 6)
            {
                if (qtrenddate.Day == 30 && qtrenddate.Month == 6)
                    return qtrenddate;
                else
                    qtrenddate = new DateTime(currentDate.Year, 3, 31);

            }
            if (month >= 7 && month <= 9)
            {
                if (qtrenddate.Day == 30 && qtrenddate.Month == 9)
                    return qtrenddate;
                else
                    qtrenddate = new DateTime(currentDate.Year, 6, 30);

            }
            if (month >= 10 && month <= 12)
            {
                if (qtrenddate.Day == 31 && qtrenddate.Month == 12)
                    return qtrenddate;
                else
                    qtrenddate = new DateTime(currentDate.Year, 9, 30);
            }

            return qtrenddate;
        }


        private DateTime GetLastDate(DateTime initialDate, int Scheme_id)
        {

            string sql = string.Empty;
            SqlParameter date_param = new SqlParameter();
            SqlParameter Scheme_id_param = new SqlParameter();
            DateTime returnDate;
            try
            {
                conn.ConnectionString = connstr;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                date_param.ParameterName = "@initial_date";
                date_param.SqlDbType = SqlDbType.DateTime;
                date_param.Direction = ParameterDirection.Input;
                date_param.Value = initialDate;

                Scheme_id_param.ParameterName = "@Scheme_id";
                Scheme_id_param.SqlDbType = SqlDbType.Int;
                Scheme_id_param.Direction = ParameterDirection.Input;
                Scheme_id_param.Value = Scheme_id;

                sql = @"select min(nav_date) from t_nav_div where scheme_id =@Scheme_id and nav_date >=@initial_date";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(date_param);
                cmd.Parameters.Add(Scheme_id_param);

                object objreturnDate = cmd.ExecuteScalar();
                if (objreturnDate == DBNull.Value || objreturnDate == null)
                    returnDate = initialDate;
                else
                    returnDate = (DateTime)objreturnDate;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return returnDate;
        }


        #endregion

        #region Excel Work

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the
            // specified ASP.NET server control at run time.
            // No code required here.
            return;
        }

       


        /// <summary>
        /// This function will Generate Excel
        /// </summary>
        private void ShowExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Top3choice_SIP_Report.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";


            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Text.StringBuilder objFinalstr = new System.Text.StringBuilder();


            switch (ddlMode.SelectedItem.Value.ToUpper())
            {
                case "SIP":
                case "SIP WITH INITIAL INVESTMENT":
                    gvFirstTable.RenderControl(hw);
                    sipGridView.RenderControl(hw);
                    break;                
            }
            LSDisc.RenderControl(hw);
            objFinalstr.Append("<br/>");
            objFinalstr.Append("<table width='520px' style='text-align:justify'><tr><td width='100%'>");
            objFinalstr.Append(sw.ToString());
            objFinalstr.Append("<br/><b>Mutual Fund investments are subject to market risks, read all scheme related documents carefully.</b>");
            objFinalstr.Replace("<table", "<p height='90px'/><table");
            objFinalstr.Replace("width:220px", "width:350px");
            objFinalstr.Append("</td></tr></table>");
            objFinalstr.Append("<br/>");



            string ImagePath = string.Empty;
            ImagePath = HttpContext.Current.Server.MapPath("~") + "Top3choice\\img\\";
            //  objFinalstr.Replace("img/rimg.png", ImagePath + "rimg.png");
            objFinalstr.Replace("<img src='img/rimg.png' style='vertical-align:middle;'>", "");




            //objFinalstr.Replace("<table", "<br/><table"); 

            //objFinalstr.Replace(@"class=""grdHead""", "style ='text-align: center;	font-family: Arial;	color: #ffffff;	font-size: 12px;	font-weight:normal;	line-height: 20px;	background:#569fd3;	height:25px; width:15px '");
            objFinalstr.Replace(@"class=""grdHead""", "style ='text-align: center;	font-family: Arial;	color: #ffffff;	font-size: 11px;background:#004fa3;	font-weight:normal;'");
            //objFinalstr.Replace(@"class=""grdRow""", "style=' border: #d0d6db solid 1px;    text-align: center; background-color: #ffffff;    font-family: Arial;    color: #000;    font-size: 11px;    font-weight: normal;'");
            //objFinalstr.Replace(@"class=""grdAltRow""", "style='background-color: #f4f4f4;    font-family: Arial;    color: #000;    font-size: 11px; font-weight: normal;    border: #d0d6db solid 1px;     text-align: center;'");


            // Response.WriteFile(pathtoSave);
            Response.Write(objFinalstr.ToString());
            Response.Flush();
            Response.End();
        }

        #endregion

        #region : Chart

        /// <summary>
        /// This Function will Bind the DataTablw with correponding Chart
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="xField"></param>
        /// <param name="chrt"></param>
        /// <param name="strCompareColumn"></param>

        public void BindDataTableToChart(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt, string strCompareColumn)
        {
            string columnName = null;
            bool showIndex = false;
            string CompareToColumn = string.Empty;

            try
            {

                chrt.Series.Clear();

                #region :Add Date Column
                _dt.Columns.Add("DateStr", typeof(DateTime));
                foreach (DataRow dr in _dt.Rows)
                {
                    //dr["DateStr"] = Convert.ToDateTime(string.Format("{0:MM/dd/yyyy}", dr[xField]));
                    var arr = dr[xField].ToString().Split('/');
                    dr["DateStr"] = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[1]), Convert.ToInt32(arr[0]));
                }
                if (_dt.Columns.Contains(xField))
                    _dt.Columns.Remove(xField);
                xField = "DateStr";
                #endregion



                foreach (DataColumn dc in _dt.Columns)
                {
                    if (dc.ColumnName == xField)
                        continue;
                    chrt.Series.Add(dc.ColumnName);

                    chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Line;
                    columnName = dc.ColumnName;

                    if (dc.ColumnName.ToUpper() == strCompareColumn)
                    {
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#4F4E50");// Color.Black;
                        chrt.Series[dc.ColumnName].LegendText = "Investment Amount";
                        chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Dash;
                        // chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Line;

                    }
                    else
                    {
                        CompareToColumn = dc.ColumnName.ToUpper();
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#004fa3");// System.Drawing.Color.Blue;//#569fd3
                        chrt.Series[dc.ColumnName].LegendText = "Investment Value";
                        chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Solid;
                        // chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Line;

                    }

                    chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, columnName);
                    chrt.Series[columnName].IsValueShownAsLabel = false;
                    chrt.Series[columnName].BorderWidth = 2;

                }

                if (ddlMode.SelectedItem.Value.ToUpper().StartsWith("SIP"))
                {

                    // chrt.Series[strCompareColumn].LegendText = "Investment Amount";
                    chrt.Series["Amount"].LegendText = "Investment Amount";
                    chrt.Series[CompareToColumn].LegendText = "Investment Value";
                    if (ViewState["FundName"] != null)
                        chrt.Titles[0].Text = "SIP Performance Chart:" + Convert.ToString(ViewState["FundName"]);
                }                
                //chrt.Series[0].XValueType = ChartValueType.DateTime;

                chrt.Visible = true;
                chrtResult.Visible = true;

                double? maxval = 1;
                double? minval = 10000;

                //CompareToColumn
                maxval = _dt.AsEnumerable().Max(x => x.Field<double?>(CompareToColumn));
                //>= _dt.AsEnumerable().Max(x => x.Field<double?>("CUMULATIVE_AMOUNT")) ? _dt.AsEnumerable().Max(x => x.Field<double?>("AMOUNT")) : _dt.AsEnumerable().Max(x => x.Field<double?>("CUMULATIVE_AMOUNT"));



                if (_dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn)) != null)
                    maxval = maxval >= _dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn)) ? maxval : _dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn));

                minval = _dt.AsEnumerable().Min(x => x.Field<double?>(CompareToColumn));
                //<= _dt.AsEnumerable().Min(x => x.Field<double?>("CUMULATIVE_AMOUNT")) ? _dt.AsEnumerable().Min(x => x.Field<double?>("AMOUNT")) : _dt.AsEnumerable().Min(x => x.Field<double?>("CUMULATIVE_AMOUNT"));

                if (_dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn)) != null)
                    minval = minval <= _dt.AsEnumerable().Min(x => x.Field<double?>(strCompareColumn)) ? minval : _dt.AsEnumerable().Min(x => x.Field<double?>(strCompareColumn));



                maxval = Math.Round(Convert.ToDouble(maxval), 0);
                minval = Math.Round(Convert.ToDouble(minval), 0);

                maxval = RoundToNearest((double)maxval, 1000);
                minval = RoundToNearest((double)minval, 1000);
                minval = minval - 1000;



                chrt.ChartAreas[0].AxisY.Maximum = Math.Round(Convert.ToDouble(maxval), 0) * 1.03;
                if (minval < 1000)
                    chrt.ChartAreas[0].AxisY.Minimum = 0;
                else
                    chrt.ChartAreas[0].AxisY.Minimum = Math.Round(Convert.ToDouble(minval), 0) * 0.97;





                chrt.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chrt.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;


                //chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
                //chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;

                //chrt.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial",15f);//, FontStyle.Bold);
                chrt.ChartAreas[0].AxisY.Title = "Value(Rs)";

                chrt.ChartAreas[0].AxisY.LabelStyle.Format = "#,###";
                chrt.ChartAreas[0].AxisY.IsLabelAutoFit = true;
                chrt.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Horizontal;
                // chrt.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 11);
                chrt.ChartAreas[0].AxisX.Title = "Time Period";
                chrt.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                chrt.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");

                // chrt.ChartAreas[0].AxisX.LabelStyle.Format = "yy";
                var chrtArea = chrt.ChartAreas[0];
                chrtArea.AxisX.MajorGrid.LineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.NotSet;
                chrtArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
                chrtArea.Visible = true;



                DateTime dtMax = (DateTime)_dt.AsEnumerable().Select(x => x[xField]).Max();
                DateTime dtMin = (DateTime)_dt.AsEnumerable().Select(x => x[xField]).Min();




                chrt.ChartAreas[0].AxisX.Minimum = dtMin.ToOADate();
                chrt.ChartAreas[0].AxisX.Maximum = dtMax.ToOADate();

                if (dtMax.Subtract(dtMin).Days > 365 * 3)
                {
                    chrt.ChartAreas[0].AxisX.Interval = 1;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy";
                }
                else if (dtMax.Subtract(dtMin).Days > 365)
                {
                    chrt.ChartAreas[0].AxisX.Interval = 6;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yy";
                }
                else
                {
                    chrt.ChartAreas[0].AxisX.Interval = 3;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yy";
                }

                System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];
                legend.Font = new Font("Arial", 12, FontStyle.Bold);
                legend.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                chrt.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                chrt.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                //chrt.ChartAreas[0].AxisX.LabelStyle.Font #4f4e50

                #region Chart Image MyRegion


                tmpChartName = "ChartImagetest.jpg";



                string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                string localImgPath = @"Top3choice\Img";
                localImgPath = System.IO.Path.Combine(appPath, localImgPath);

                string testpath = localImgPath;


                #region Delete MyRegion
                var allImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "Top3choice\\IMG", "Top3choice_SIP_Temp*");
                foreach (var f in allImageFiles)
                    if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
                        File.Delete(f);


                #endregion

                #region : save Image


                //localImgPath = System.IO.Path.Combine(localImgPath, "SIP"+ Guid.NewGuid().ToString("N"));
                if (!Directory.Exists(localImgPath))
                {
                    //using (Directory.CreateDirectory(localImgPath))
                    //{                    }
                    Directory.CreateDirectory(localImgPath);
                }


                imgPath = System.IO.Path.Combine(localImgPath, "Top3choice_SIP_Temp" + Guid.NewGuid().ToString("N") + "_" + tmpChartName);


                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                }
               
                Session["imgPath"] = imgPath;
                chrtResult.SaveImage(imgPath);
                

                #endregion



                #endregion




            }
            catch (Exception ex)
            {
                lblError.Text= ex.Message;
               // Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");

            }

        }
       

        public double RoundToNearest(double Amount, double RoundTo)
        {

            double ExcessAmount = Amount % RoundTo;

            Amount += (RoundTo - ExcessAmount);
            //if (ExcessAmount < (RoundTo / 2))
            //{
            //    Amount -= ExcessAmount;
            //}
            //else
            //{
            //    Amount += (RoundTo - ExcessAmount);
            //}

            return Amount;
        }


        private Color LightenColor(Color color)
        {
            throw new NotImplementedException();
        }

        public void BindDataTableToChartGeneral(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt)
        {
            string columnName = null;


            try
            {

                chrt.Series.Clear();

                List<string> columnList = new List<string>();
                columnList.Clear();

                foreach (DataColumn dc in _dt.Columns)
                {
                    if (dc.ColumnName == xField)
                        continue;
                    chrt.Series.Add(dc.ColumnName);
                    columnList.Add(dc.ColumnName);
                    chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Spline;
                    columnName = dc.ColumnName;

                    if (dc.ColumnName.ToUpper() == "AMOUNT")
                    {
                        //chrt.Series[dc.ColumnName].YAxisType = AxisType.Primary;
                        chrt.Series[dc.ColumnName].LegendText = "INVESTMENT AMOUNT";
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
                        // chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#001F5C");                
                        // chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Solid;
                    }
                    else
                    {
                        //  chrt.Series[dc.ColumnName].YAxisType = AxisType.Secondary;
                        //chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");               
                        chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Blue;
                        chrt.Series[dc.ColumnName].LegendText = "CUMULATIVE AMOUNT";
                        // chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Dash;
                        // chrt.Series[dc.ColumnName].BorderWidth = 15;
                        //chrt.Series[dc.ColumnName].ShadowOffset = 8;                        
                    }

                    chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, columnName);
                    chrt.Series[columnName].IsValueShownAsLabel = false;
                    chrt.Series[columnName].BorderWidth = 1;


                }

                chrt.Series[0].XValueType = ChartValueType.DateTime;
                chrt.Series[0].XValueType = ChartValueType.Date;

                chrt.Visible = true;
                chrtResult.Visible = true;

                double? maxval = 1;
                double? minval = 10000;

                if (columnList.Count >= 2)
                {
                    maxval = _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[0])) >= _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[1])) ? _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[0])) : _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[1]));

                    minval = _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[0])) <= _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[1])) ? _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[0])) : _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[1]));


                }

                chrt.ChartAreas[0].AxisY.Maximum = Math.Round(Convert.ToDouble(maxval), 0) + 1000;

                if (minval < 1000)
                    chrt.ChartAreas[0].AxisY.Minimum = 0;
                else
                    chrt.ChartAreas[0].AxisY.Minimum = Math.Round(Convert.ToDouble(minval), 0) - 500;



                chrt.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chrt.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;

                chrt.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chrt.ChartAreas[0].AxisY.MajorGrid.Enabled = false;


                chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
                chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;


                chrt.ChartAreas[0].AxisY.Title = "Figure in Rs";
               

                chrt.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;
                chrt.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.Chocolate;

                var chrtArea = chrt.ChartAreas[0];
                chrtArea.Visible = true;


                System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];

                legend.Font = new Font("Arial", 9, FontStyle.Bold);

                #region Chart Image MyRegion


                tmpChartName = "ChartImagetest.jpg";



                string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                string localImgPath = @"Top3choice\Img";
                localImgPath = System.IO.Path.Combine(appPath, localImgPath);

                string testpath = localImgPath;


                #region Delete MyRegion
                var allImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "Top3choice\\IMG", "Top3choice_SIP_Temp*");
                foreach (var f in allImageFiles)
                    if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
                        File.Delete(f);


                #endregion

                #region : save Image


                //localImgPath = System.IO.Path.Combine(localImgPath, "SIP"+ Guid.NewGuid().ToString("N"));
                if (!Directory.Exists(localImgPath))
                {
                    //using (Directory.CreateDirectory(localImgPath))
                    //{                    }
                    Directory.CreateDirectory(localImgPath);
                }


                imgPath = System.IO.Path.Combine(localImgPath, "Top3choice_SIP_Temp" + Guid.NewGuid().ToString("N") + "_" + tmpChartName);

                Session["imgPath"] = imgPath;
                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                }
                chrtResult.SaveImage(imgPath);
                #endregion



                #endregion

            }
            catch (Exception ex)
            {
                Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");
            }

        }
        

        #endregion

        #region export to PDF Section

        /// <summary>
        /// This Function will Generate a PDF
        /// </summary>
        public void CreateHTMLSIP()
        {

            System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
            string gvFirstTablestr = string.Empty;
            string GridViewSIPResultstr = string.Empty;
            string sipGridViewstr = string.Empty;
            string path = string.Empty;


            var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "Top3choice\\PDF", "Top3choice_*");
            foreach (var f in allFiles)
                if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
                    File.Delete(f);

            try
            {


                #region Select Html Pages
                switch (RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim())
                {
                    case "DISTRIBUTOR":
                        if (ddlMode.SelectedItem.Value.ToUpper() == "SIP")
                        {
                            strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Top3choice/dspPDFtemplate3.html")));//dspPDFtemplate2 for din regular dspPDFtemplate3 normal
                        }
                        else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP WITH INITIAL INVESTMENT")
                        {
                            strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Top3choice/dspPDFTemplateInitial.html")));
                        }
                        break;
                    default:
                        if (ddlMode.SelectedItem.Value.ToUpper() == "SIP")
                        {
                            strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Top3choice/dspPDFtemplateWOD.html")));//
                        }
                        else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP WITH INITIAL INVESTMENT")
                        {
                            strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Top3choice/dspPDFTemplateInitialWOD.html")));
                        }
                        break;
                }

                #endregion





                if (ddlMode.SelectedItem.Value.ToUpper().StartsWith("SIP"))
                    gvFirstTablestr = FillHtmlGridViewTable(gvFirstTable); 
                gvFirstTablestr = "<div><table border='0' align='center' width='100%' cellpadding='0' cellspacing='0'>" + gvFirstTablestr.Substring(gvFirstTablestr.IndexOf("<th"));



                #region changes on and after 30-10-2012

                if (ddlMode.SelectedItem.Value.ToUpper().StartsWith("SIP"))
                {
                    //  gvFirstTablestr = gvFirstTablestr.Replace(@"scope=""col""", "style ='text-align: left;font-family: Arial;color: #ffffff;font-size: 15px;font-weight: normal;background: #569fd3;height: 25px;'"); for header left


                    // for header center
                    gvFirstTablestr = gvFirstTablestr.Replace(@"scope=""col""", "style ='text-align:center;font-family: Arial;color: #ffffff;font-size: 15px;font-weight: normal;background: #569fd3;height: 25px;'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""grdRow""", "style = 'font-size:15px;	vertical-align:middle;	text-align:left;height: 25px;'");//font-family:Trebuchet MS;
                }
                else// if (ddlMode.SelectedItem.Value.ToUpper() == "SWP")
                {
                    gvFirstTablestr = gvFirstTablestr.Replace(@"scope=""col""", "style ='text-align: center;font-family: Arial;color: #ffffff;font-size: 15px;font-weight: normal;background: #569fd3;height: 25px;'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""grdRow""", "style = 'font-size:15px;	vertical-align:middle;	text-align:center;height: 25px;'");//font-family:Trebuchet MS;
                }

                gvFirstTablestr = gvFirstTablestr.Replace("<th", "<td");
                gvFirstTablestr = gvFirstTablestr.Replace("</th", "</td");
                gvFirstTablestr = gvFirstTablestr.Replace("style='vertical-align:middle;'", "style='vertical-align:top; padding-top:2px;'");

                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
                {
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""borderlefft""", "style = 'border-top:black solid 1px;text-align:center;'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""borderbottom""", "style = 'border-top:black solid 1px;text-align:center;'");
                    //  gvFirstTablestr = gvFirstTablestr.Replace(@"class=""leftal""", "style = 'width:250px;border-bottom:black solid 1px;'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""leftal""", "style = 'width:300px;border-top:black solid 1px;'");
                }
                else
                {
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""borderlefft""", "style = 'border-bottom:black solid 1px;text-align:center;'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""borderbottom""", "style = 'border-bottom:black solid 1px;text-align:center;'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""leftal""", "style = 'width:250px;border-bottom:black solid 1px;'");
                }


                #endregion

                gvFirstTablestr = gvFirstTablestr.Replace("img/rimg.png", "../img/rimg.png");
                gvFirstTablestr = gvFirstTablestr.Replace(@"border=""1""", "border='0'");


                strHTML = strHTML.Replace("<!gvFirstTable!>", gvFirstTablestr);

                strHTML = strHTML.Replace("<!SIP!>", ddlMode.SelectedItem.Value);


                string imagePath = Convert.ToString(Session["imgPath"]);


                if (!string.IsNullOrEmpty(imagePath))
                {

                    if (File.Exists(imagePath))
                    {
                        string imgname = imagePath.Split('\\')[imagePath.Split('\\').Count() - 1];

                        imgname = "../IMG/" + imgname;

						strHTML = strHTML.Replace("<!chartImage!>", "<img alt=''   src='" + imgname + "' height='100%' width='100%' />"); //height='90%'
                    }

                }




                GridViewSIPResultstr = FillHtmlGridViewTable(GridViewSIPResult);



                GridViewSIPResultstr = "<div><table border='0' align='left' width='100%' cellpadding='0' cellspacing='0'>" + GridViewSIPResultstr.Substring(GridViewSIPResultstr.IndexOf("<th"));
                //added 30 oct 2012
                GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"scope=""col""", "style ='text-align: center;font-family: Arial;color: #ffffff;font-size: 15px;font-weight: normal;background: #569fd3;height: 25px;'");

                GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""grdRow""", "style = 'font-size:15px;	vertical-align:middle;	text-align:left;	border-bottom:#c6c8ca solid 1px;height: 25px;'");//font-family:Trebuchet MS;

                
                GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""borderlefft""", "style = 'border-bottom:black solid 1px;text-align:center;'");
                GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""borderbottom""", "style = 'border-bottom:black solid 1px;text-align:center;'");
                GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""leftal""", "style = 'width:400px;border-bottom:black solid 1px;text-align:left;'");
                strHTML = strHTML.Replace("<!GridViewSIPResult!>", GridViewSIPResultstr);

                strHTML = strHTML.Replace(@"<table cellspacing='0' rules='all' border='1' id='gvFirstTable' style='width:100%;border-collapse:collapse;", @"<table border='0' align='center' cellpadding='0' cellspacing='0'");




                strHTML = strHTML.Replace("<!disclaimerDiv!>", FillHtmlDisclaimerDiv());
                strHTML = strHTML.Replace(@"class=""grdheader""", "style ='font-family:Arial Narrow; font-size:14px;vertical-align:middle;text-align:center;border-bottom:#c6c8ca solid 1px;height: 25px;font-weight: bold;'");

                strHTML = strHTML.Replace(@"class=""grdRow""", "style = 'font-family:Arial Narrow;	font-size:14px;	vertical-align:middle;	text-align:center;	border-bottom:#c6c8ca solid 1px;height: 25px;'");


                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
                {
                    strHTML = strHTML.Replace("<!SchemeName!>", ddlscheme.SelectedItem.Text + " (A)");//
                    strHTML = strHTML.Replace("<!SchemeNameTo!>", ddlschtrto.SelectedItem.Text + " (B)");//
                    strHTML = strHTML.Replace("<!SchemeName1!>", ddlscheme.SelectedItem.Text);//
                    strHTML = strHTML.Replace("<!SchemeNameTo1!>", ddlschtrto.SelectedItem.Text);//
                }
                else
                {
                    strHTML = strHTML.Replace("<!SchemeName!>", ddlscheme.SelectedItem.Text);//
                }

                if (ddlMode.SelectedItem.Value.Length > 15 && ddlMode.SelectedItem.Value.ToUpper().StartsWith("SIP"))// with initial
                {
                    if (txtiniAmount.Text.Length > 0)
                        strHTML = strHTML.Replace("<!InitialAmount!>", Convert.ToDouble(txtiniAmount.Text).ToString("n0"));
                    if (txtIniToDate.Text.Length > 0)
                        strHTML = strHTML.Replace("<!InitialDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtIniToDate.Text.ToString().Split('/')[2]), Convert.ToInt32(txtIniToDate.Text.ToString().Split('/')[1]), Convert.ToInt32(txtIniToDate.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                }               

                
                strHTML = strHTML.Replace("<!InstallmentAmount!>", Convert.ToDouble(txtinstall.Text).ToString("n0"));

                strHTML = strHTML.Replace("<!Frequency!>", ddPeriod_SIP.SelectedItem.Text);
                strHTML = strHTML.Replace("<!SIPDate!>", ddSIPdate.SelectedItem.Text);


                strHTML = strHTML.Replace("<!FromDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtfromDate.Text.ToString().Split('/')[2]), Convert.ToInt32(txtfromDate.Text.ToString().Split('/')[1]), Convert.ToInt32(txtfromDate.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                strHTML = strHTML.Replace("<!ToDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtToDate.Text.ToString().Split('/')[2]), Convert.ToInt32(txtToDate.Text.ToString().Split('/')[1]), Convert.ToInt32(txtToDate.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                strHTML = strHTML.Replace("<!ValueDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtvalason.Text.ToString().Split('/')[2]), Convert.ToInt32(txtvalason.Text.ToString().Split('/')[1]), Convert.ToInt32(txtvalason.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                
                


                if (ddlMode.SelectedItem.Value.Trim().ToUpper().StartsWith("SIP"))
                {
                    if (ViewState["gvFirstTableDT"] != null)
                    {
                        DataTable dtTemp = ViewState["gvFirstTableDT"] as DataTable;
                        if (dtTemp.Rows.Count > 0)
                        {
                            double profitsip = Convert.ToDouble(dtTemp.Rows[0]["Profit_Sip"]);
                            double sipreturn = Convert.ToDouble(dtTemp.Rows[0]["yield"]);
                            strHTML = strHTML.Replace("<!TotalProfit!>", Math.Round(profitsip, 2).ToString("n0"));
                            strHTML = strHTML.Replace("<!SIPReturns!>", Convert.ToString(Math.Round(sipreturn, 2)));
                        }
                    }
                }

                if (ViewState["Mobile"] != null)
                    strHTML = strHTML.Replace("<!Mobile!>", Convert.ToString(ViewState["Mobile"]));

                if (ViewState["ArnNo"] != null)
                    strHTML = strHTML.Replace("<!ArnNo!>", Convert.ToString(ViewState["ArnNo"]));

                if (ViewState["Preparedby"] != null)
                    strHTML = strHTML.Replace("<!PreparedBy!>", Convert.ToString(ViewState["Preparedby"]));

                if (ViewState["PreparedFor"] != null)
                    strHTML = strHTML.Replace("<!PreparedFor!>", Convert.ToString(ViewState["PreparedFor"]));

                if (ViewState["Email"] != null)
                    strHTML = strHTML.Replace("<!Email!>", Convert.ToString(ViewState["Email"]));


                if (ViewState["FundName"] != null)
                    strHTML = strHTML.Replace("<!FundName!>", Convert.ToString(ViewState["FundName"]));


                if (ViewState["CorpusFund"] != null)
                    strHTML = strHTML.Replace("<!CorpusFund!>", Convert.ToDouble(ViewState["CorpusFund"]).ToString("n2"));

                if (ViewState["schmStartDate"] != null)
                    strHTML = strHTML.Replace("<!InceptionDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(ViewState["schmStartDate"].ToString().Split('/')[2]), Convert.ToInt32(ViewState["schmStartDate"].ToString().Split('/')[1]), Convert.ToInt32(ViewState["schmStartDate"].ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());

                if (ViewState["FundmanegerText"] != null)
                    strHTML = strHTML.Replace("<!FundManagerName!>", Convert.ToString(ViewState["FundmanegerText"]));

                if (ViewState["FundDesc1"] != null)
                    strHTML = strHTML.Replace("<!FundDesc1!>", Convert.ToString(ViewState["FundDesc1"]));

                if (ViewState["FundDesc2"] != null)
                    strHTML = strHTML.Replace("<!FundDesc2!>", Convert.ToString(ViewState["FundDesc2"]));
                string riskImage = string.Empty;
                string riskImageColor = string.Empty;
                string riskImageColorStmnt = string.Empty;

                if (ViewState["FundDesc3"] != null)
                {
                    //strHTML = strHTML.Replace("<!FundDesc3!>", Convert.ToString(ViewState["FundDesc3"]));
                    //riskImage = Convert.ToString(ViewState["FundDesc3"]).Trim().ToUpper();
                    //RiskColor(riskImage, ref riskImageColor, ref riskImageColorStmnt);
                    //strHTML = strHTML.Replace("<!RiskImage!>", riskImageColor);
                    //strHTML = strHTML.Replace("<!riskImageColorStmnt!>", riskImageColorStmnt);
                    riskImage = Convert.ToString(ViewState["FundDesc3"]).Trim().ToUpper();
                    Riskometer(riskImage, ref riskImageColor, ref riskImageColorStmnt);
                    strHTML = strHTML.Replace("<!RiskImage!>", riskImageColor);
                }



                if (ddlMode.SelectedItem.Value.Trim().ToUpper().StartsWith("SIP"))
                {
                    if (ViewState["totalInvestedAmount"] != null)
                        strHTML = strHTML.Replace("<!TotalInvestmentAmount!>", Convert.ToDouble(ViewState["totalInvestedAmount"]).ToString("n0"));
                    if (ViewState["presentInvestValue"] != null)
                        strHTML = strHTML.Replace("<!totalInvestment!>", Convert.ToDouble(ViewState["presentInvestValue"]).ToString("n0"));
                }                
                else//lump part
                {
                }



                Session["GUID"] = Guid.NewGuid().ToString();
                path = HttpContext.Current.Server.MapPath("~/Top3choice/PDF/" + "Top3choice" + "_" + Convert.ToString(Session["GUID"]) + ".htm");

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (File.Create(path))
                {

                }
                File.WriteAllText(path, strHTML.ToString());
                strHTML = null;
                _SimpleConversion();

                string pdfName = string.Empty;
                if (Session["GUID"] != null)
                {
                    pdfName = Convert.ToString(Session["GUID"]) + ".pdf";
                    Response.ContentType = "Application/pdf";
                    pdfName = Convert.ToString(Session["GUID"]) + ".pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Top3choice_SIP.pdf");
                    Response.TransmitFile(Server.MapPath("~/Top3choice/PDF/Top3choice_" + pdfName));
                    Response.End();
                }

            }
            catch (Exception ex)
            {
                strHTML = null;
                throw ex;
            }

        }

        private static void RiskColor(string riskImage, ref string riskImageColor, ref string riskImageColorStmnt)
        {
            switch (riskImage)
            {
                case "LOW RISK":
                    riskImageColor = "<img src='../IMG/img_blue.jpg' />";
                    riskImageColorStmnt = riskImageColor + " (Blue)";
                    break;
                case "MEDIUM RISK":
                    riskImageColor = "<img src='../IMG/img_yellow.jpg' />";
                    riskImageColorStmnt = riskImageColor + " (Yellow)";
                    break;
                case "HIGH RISK":
                    riskImageColor = "<img src='../IMG/img_brown.jpg' />";
                    riskImageColorStmnt = riskImageColor + " (Brown)";
                    break;
            }
        }
        private static void Riskometer(string riskImage, ref string riskImageColor, ref string riskImageColorStmnt)
        {
            switch (riskImage)
            {
                case "LOW RISKOMETER":
                    riskImageColor = "<img src='../IMG/Low.jpg'  height='150px' widtht='50px' />";
                    riskImageColorStmnt = riskImageColor + " (Blue)";
                    break;
                case "MODERATELY LOW RISKOMETER":
                    riskImageColor = "<img src='../IMG/Moderately_Low.jpg'  height='150px' widtht='50px' />";
                    riskImageColorStmnt = riskImageColor + " (Yellow)";
                    break;
                case "MODERATE RISKOMETER":
                    riskImageColor = "<img src='../IMG/Moderate.jpg'  height='150px' widtht='50px' />";
                    riskImageColorStmnt = riskImageColor + " (Brown)";
                    break;
                case "MODERATELY HIGH RISKOMETER":
                    riskImageColor = "<img src='../IMG/Moderately_High.jpg'  height='150px' widtht='50px' />";
                    riskImageColorStmnt = riskImageColor + " (Brown)";
                    break;
                case "HIGH RISKOMETER":
                    riskImageColor = "<img src='../IMG/High.jpg'  height='150px' widtht='50px' />";
                    riskImageColorStmnt = riskImageColor + " (Brown)";
                    break;
            }
        }

        /// <summary>
        /// Thsi function will return Html of Gridview        
        /// </summary>
        /// <param name="objSipGridView"></param>
        /// <returns></returns>
        private string FillHtmlGridViewTable(GridView objSipGridView)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                HttpContext.Current.Response.Clear();
                using (System.IO.StringWriter stringWrite = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite))
                    {
                        if (objSipGridView.Rows.Count > 0)
                        {
                            objSipGridView.RenderControl(htmlWrite);
                            sb.Append(stringWrite.ToString());
                        }
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private string FillHtmlDisclaimerDiv()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                HttpContext.Current.Response.Clear();
                using (System.IO.StringWriter stringWrite = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite))
                    {
                        disclaimerDiv.RenderControl(htmlWrite);
                        sb.Append(stringWrite.ToString());
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void _SimpleConversion()
        {
            try
            {
                using (var wk = _GetConverter())
                {

                    _Log.DebugFormat("Performing conversion..");

                    wk.GlobalSettings.Margin.Top = "0cm";
                    wk.GlobalSettings.Margin.Bottom = "0cm";
                    wk.GlobalSettings.Margin.Left = "0cm";
                    wk.GlobalSettings.Margin.Right = "0cm";
                    //wk.GlobalSettings.Out = @"c:\temp\tmp.pdf";




                    wk.ObjectSettings.Web.EnablePlugins = false;
                    wk.ObjectSettings.Web.EnableJavascript = true;
                    wk.ObjectSettings.Web.LoadImages = true;
                    wk.ObjectSettings.Page = SimplePageFile;


                    string htmlfile = string.Empty;

                    htmlfile = HttpContext.Current.Server.MapPath("~/Top3choice/PDF/" + "Top3choice" + "_" + Convert.ToString(Session["GUID"]) + ".htm");
                    wk.ObjectSettings.Page = htmlfile;

                    wk.ObjectSettings.Load.Proxy = "none";

                    var tmp = wk.Convert();

                    Assert.IsNotEmpty(tmp);
                    var number = 0;
                    lock (this) number = count++;

                    string savepdfpath = string.Empty;
                    string savehtmlpath = string.Empty;
                    savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\Top3choice\\PDF\\Top3choice_" + Convert.ToString(Session["GUID"]) + ".pdf";

                    savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\Top3choice\\PDF\\Top3choice_" + Convert.ToString(Session["GUID"]) + ".htm";
                    ViewState["selectedval"] = savepdfpath;
                    if (File.Exists(savepdfpath))
                    {
                        File.Delete(savepdfpath);
                    }
                    File.WriteAllBytes(savepdfpath, tmp);
                    if (File.Exists(savehtmlpath))
                    {
                        File.Delete(savehtmlpath);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private MultiplexingConverter _GetConverter()
        {
            var obj = new MultiplexingConverter();
            obj.Begin += (s, e) => _Log.DebugFormat("Conversion begin, phase count: {0}", e.Value);
            obj.Error += (s, e) => _Log.Error(e.Value);
            obj.Warning += (s, e) => _Log.Warn(e.Value);
            obj.PhaseChanged += (s, e) => _Log.InfoFormat("PhaseChanged: {0} - {1}", e.Value, e.Value2);
            obj.ProgressChanged += (s, e) => _Log.InfoFormat("ProgressChanged: {0} - {1}", e.Value, e.Value2);
            obj.Finished += (s, e) => _Log.InfoFormat("Finished: {0}", e.Value ? "success" : "failed!");
            return obj;
        }

        #endregion



        #endregion
    }
}