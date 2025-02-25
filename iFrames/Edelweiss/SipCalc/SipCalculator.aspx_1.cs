using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using iFrames.DAL;
using System.Data.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Web.SessionState;
using System.Drawing;
using iFrames.Kotak;
using System.Net;
using System.Collections;
using WkHtmlToXSharp;
using NUnit.Framework;
using iFrames.BLL;

namespace iFrames.Edelweiss
{
    //public abstract class BasePage : System.Web.UI.Page
    //{
    //    public object SafeEval(object container, string expression)
    //    {
    //        try
    //        {
    //            return DataBinder.Eval(container, expression);
    //        }
    //        catch (HttpException e)
    //        {
    //            // Write error details to minimize the harm caused by suppressed exception 
    //            //Trace.Write("DataBinding", "Failed to process the Eval expression", e);
    //        }

    //        return "Put here whatever default value you want";
    //    }
    //}

    public partial class SipCalculator : System.Web.UI.Page
    {
        #region Global declaration

        readonly string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        readonly SqlConnection conn = new SqlConnection();
        System.Data.DataTable finalResultdt = new System.Data.DataTable();
        System.Data.DataTable finalResultdtwobenchmark = new System.Data.DataTable();
        System.Data.DataTable sipDataTable = new System.Data.DataTable();

        string imgPath = string.Empty;
        string tmpChartName = string.Empty;

        // Added On 07.02.2017 For Dividend Schemes Check Only
        bool IsSchemeDividend = false;
        #endregion

        #region: PDF Global Variable

        private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string SimplePageFile = null;
        public static int count = 0;

        #endregion

        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            this.sipbtnshow.Attributes.Add("onclick", "javascript:return validateDSP();");// Add ClientSide Script for Data Validation function defined in check.js
            string cat = Request.QueryString.AllKeys.Contains("cat") ? HttpUtility.UrlDecode(Request.QueryString["cat"].Trim()).ToString() : null;
            string opt = Request.QueryString.AllKeys.Contains("opt") ? Request.QueryString["opt"].Trim().ToString() : null;
            string sch = Request.QueryString.AllKeys.Contains("sch") ? Request.QueryString["sch"].ToString() : null;//amfi code
            string tosch = Request.QueryString.AllKeys.Contains("tosch") ? Request.QueryString["tosch"].ToString() : null;//amfi code
            btnExcelCalculation.Visible = false;
            if (!IsPostBack)
            {
                try
                {
                    FillNature();
                    FillOption();
                    FillDropdownScheme();

                    if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
                    {
                        if (Request.QueryString["Type"].Trim().ToUpper() == "SWP")
                        {
                            ddlMode.ClearSelection();
                            ddlMode.Items.FindByValue("SWP").Selected = true;
                            txtfromDate.Text = "";
                            setCategory();
                        }
                        else if (Request.QueryString["Type"].Trim().ToUpper() == "STP")
                        {
                            ddlMode.ClearSelection();
                            ddlMode.Items.FindByValue("STP").Selected = true;
                            txtfromDate.Text = "";
                            setCategory();
                        }
                        else if (Request.QueryString["Type"].Trim().ToUpper() == "LUMP SUM")
                        {
                            ddlMode.ClearSelection();
                            ddlMode.Items.FindByValue("Lump Sum").Selected = true;
                            txtfromDate.Text = "";
                            setCategory();
                        }
                        else
                        {
                            ddlMode.ClearSelection();
                            ddlMode.Items.FindByValue("SIP").Selected = true;
                            txtfromDate.Text = "";
                            setCategory();
                        }

                        if (cat != null)
                        {
                            cat = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(cat);
                            ddlNature.SelectedValue = cat;
                            ddlNature_SelectedIndexChanged(null, null);
                        }
                        if (opt != null)
                        {
                            opt = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(opt);
                            ddlOption.SelectedValue = GetOptionIdByName(opt);
                            ddlOption_SelectedIndexChanged(null, null);
                        }
                        callCalc();
                        if (sch != null)
                        {
                            string sch_id = GetSchemeidByAmfiCode(sch);
                            if (sch_id != null && Convert.ToInt32(sch_id) > 0)
                            {
                                ddlscheme.SelectedValue = sch_id;
                                ddlscheme_SelectedIndexChanged(null, null);
                            }
                        }
                        if (Request.QueryString["Type"].Trim().ToUpper() == "STP" && tosch != null)
                        {
                            string sch_id = GetSchemeidByAmfiCode(tosch);
                            if (sch_id != null && Convert.ToInt32(sch_id) > 0)
                            {
                                ddlschtrto.SelectedValue = sch_id;
                                ddlschtrto_SelectedIndexChanged(null, null);
                            }
                        }
                    }
                    else
                    {
                        ddlMode.ClearSelection();
                        ddlMode.Items.FindByValue("SIP").Selected = true;
                        txtfromDate.Text = "";
                        setCategory();
                    }

                    SetDefaultView();

                    PopulateFrequency();

                    //callCalc();
                    //FillNature();
                    //FillOption();
                    //// FillDropdownScheme();
                    //SetDefaultView();
                    //callCalc();
                    //PopulateFrequency();                    
                    //ddlMode.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    ddlMode.ClearSelection();
                    ddlMode.Items.FindByValue("SIP").Selected = true;
                    txtfromDate.Text = "";
                    setCategory();
                    FillNature();
                    FillOption();
                    SetDefaultView();
                    callCalc();
                    ddlNature.SelectedValue = "0";
                    ddlOption.SelectedValue = "0";
                    PopulateFrequency();
                }
            }
            SetInceptionOnDropDown();
            //txtddlsipbnmark.Attributes.Add("onmouseenter", String.Format("mouseEnter2('{0}',{1})", txtddlsipbnmark.ClientID, txtddlsipbnmark.Width.Value));
            //txtddlsipbnmark.Attributes.Add("onfocusout", String.Format("focusOut('{0}',{1})", txtddlsipbnmark.ClientID, txtddlsipbnmark.Width.Value));


            //if (ddlMode.SelectedItem.Value.ToUpper() == "LUMP SUM" || ddlMode.SelectedItem.Value.ToUpper() == "SWP" || ddlMode.SelectedItem.Value.ToUpper() == "STP")
            //    LSDisc.Visible = true;
            //else
            //
            //if (ddlMode.SelectedItem.Value.ToUpper() == "LUMP SUM")// && resultDivLS.Visible == true)
            //    LSDisc1.Visible = true;
            //else
            //LSDisc1.Visible = false;


            if (gvFirstTable.Visible || gvSWPSummaryTable.Visible)
                LSDisc.Visible = true;
            else
                LSDisc.Visible = false;

            if (GridViewLumpSum.Visible)
                LSDisc1.Visible = true;
            else
                LSDisc1.Visible = false;
        }

        private string GetSchemeidByAmfiCode(string AmfiCode)
        {
            using (var db = new PrincipalCalcDataContext())
            {
                var Schemes = (from tbl in db.T_SCHEMES_MASTERs
                               where tbl.Amfi_Code == AmfiCode && tbl.Amfi_Code != null
                               select tbl).FirstOrDefault();
                if (Schemes != null)
                    return Convert.ToString(Schemes.Scheme_Id);
                else
                    return null;
            }
        }

        private string GetOptionIdByName(string optName)
        {
            using (var db = new PrincipalCalcDataContext())
            {
                var opt = (from tbl in db.T_SCHEMES_OPTIONs
                           where tbl.Option_Name == optName
                           select tbl).FirstOrDefault();
                if (opt != null)
                    return Convert.ToString(opt.Option_ID);
                else
                    return null;
            }
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
        protected void sipbtnshow_Click(object sender, EventArgs e)
        {
            resultDiv.Visible = true;
            ShowResult();
        }

        protected void sipbtnreset_Click(object sender, EventArgs e)
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

        //protected void ExcelCalculation_Click(object sender, ImageClickEventArgs e)
        //{
        //    //Calculate_Excel_modified();
        //    ShowExcel("Edelweiss");
        //}

        #region Tab Control
        private void SetDefaultView()
        {
            MultiView1.ActiveViewIndex = -1;
        }

        private void RemoveCssFromButton()
        {
            lnkTab1.CssClass = lnkTab1.CssClass.Replace("tablinks-active", "");
            lnkTab2.CssClass = lnkTab2.CssClass.Replace("tablinks-active", "");
            lnkTab3.CssClass = lnkTab3.CssClass.Replace("tablinks-active", "");
            lnkTab5.CssClass = lnkTab5.CssClass.Replace("tablinks-active", "");
            lnkTab4.CssClass = lnkTab4.CssClass.Replace("tablinks-active", "");
        }

        protected void lnkTab1_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
            RemoveCssFromButton();
            lnkTab1.CssClass = "tab-button tablinks tab-br-tl tablinks-active";
            btnExcelCalculation.Visible = true;
        }

        protected void lnkTab2_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
            // Have to cahrt
            CallChartFromView2();
            RemoveCssFromButton();
            lnkTab2.CssClass = "tab-button tablinks tab-br-md tablinks-active";
            btnExcelCalculation.Visible = false;
        }

        protected void lnkTab3_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
            RemoveCssFromButton();
            lnkTab3.CssClass = "tab-button tablinks tab-br-md tablinks-active";
            btnExcelCalculation.Visible = false;
        }
        protected void lnkTab4_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;
            CallChartFromView2();
            RemoveCssFromButton();
            lnkTab4.CssClass = "tab-button tablinks tab-br-rb tablinks-active";
            btnExcelCalculation.Visible = false;
        }
        //protected void lnkTab5_Click(object sender, EventArgs e)
        //{
        //    MultiView2.ActiveViewIndex = 0;
        //    // LSDisc.Visible = ddlMode.SelectedItem.Value.ToUpper() == "LUMP SUM";
        //}

        #endregion
        #endregion

        #region: DropDown Method

        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropdownScheme();
        }

        protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PopulateFrequency();
            //ShowRelativeDiv();
            //// txtinstall.Text = "";
            //txtfromDate.Text = "";
            //setCategory();
        }

        public void setCategory()
        {
            if (ddlMode.SelectedValue == "SIP")
            {
                if (ddlNature.Items.FindByText("CPOF") != null)
                {
                    ddlNature.Items.Remove("CPOF");
                }
                if (ddlNature.Items.FindByText("FMP") != null)
                {
                    ddlNature.Items.Remove("FMP");
                }
            }
            else
            {
                if (ddlNature.Items.FindByText("FMP") == null)
                {
                    ddlNature.Items.Add(new ListItem("FMP"));
                }
                if (ddlNature.Items.FindByText("CPOF") == null)
                {
                    ddlNature.Items.Add(new ListItem("CPOF"));
                }
            }
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
                FillDropdownIndex();
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
                    if (chkInception.Checked)
                        txtLumpfromDate.Text = SIPSchDt.Text;
                }
            }
            #endregion
            SetInceptionOnDropDown();

            //txtinstall.Text = "";
            txtfromDate.Text = "";

            IsSchemeDividend = this.IsDividendScheme();
            ViewState["IsSchemeDividend"] = IsSchemeDividend;
            //var visibility = (ddlMode.SelectedValue == "SIP" || ddlMode.SelectedValue == "SIP with Initial Investment" || ddlMode.SelectedValue == "Lump Sum") && IsSchemeDividend;
            tr_Cal_Type.Visible = (ddlMode.SelectedValue == "SIP" || ddlMode.SelectedValue == "SIP with Initial Investment") && IsSchemeDividend; ;


            trCalTypeLmpsm.Visible = (ddlMode.SelectedValue == "Lump Sum") && IsSchemeDividend;

            lnkTab5.Visible = tr_Cal_Type.Visible || trCalTypeLmpsm.Visible;
        }


        private bool IsDividendScheme()
        {
            try
            {
                Dictionary<string, string> SchemeOption = (Dictionary<string, string>)(ViewState["SchemeOption"]);
                string schemeOption = string.Empty;
                if (SchemeOption.TryGetValue(ddlscheme.SelectedValue.ToString(), out schemeOption))
                    return schemeOption == "3"; // Scheme Of Dividend Option
                return false;
            }
            catch { return false; }
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
            SIPSchDt.Text = "";
            SIPSchDt2.Text = "";
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
        /// fill the Option list 
        /// </summary>
        protected void FillOption()
        {
            try
            {
                using (var optionData = new PrincipalCalcDataContext())
                {
                    var option = from opt in optionData.T_SCHEMES_OPTIONs
                                 orderby opt.Option_Name
                                 where opt.Option_Name.ToUpper() != "N.A"
                                 select new
                                 {
                                     Id = opt.Option_ID,
                                     //Option = opt.Option_Name,// commented by syed
                                     Option = opt.Option_Name == "Income/Dividend" ? "Dividend" : opt.Option_Name // add by syed
                                 };
                    if (option.Count() > 0)
                    {
                        var dtOption = option.ToDataTable();
                        ddlOption.DataSource = dtOption;
                        ddlOption.DataTextField = "Option";
                        ddlOption.DataValueField = "Id";
                        ddlOption.DataBind();
                        ddlOption.Items.Insert(0, new ListItem("--", "0"));
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

        /// <summary>
        /// FillNature() define for Populating Category
        /// Added Custom Category in dropdownlist for Sundarm 06.08.2013 and then 12.08.2013
        /// </summary>
        /// 
        protected void FillNature()
        {
            try
            {
                //using (var natureData = new PrincipalCalcDataContext())
                //{

                //    string[] natureExcluded = { "N.A", "Speciality", "ETF", "Dynamic/Asset Allocation", "Balanced", "Liquid", "Debt" };
                //    string[] natureIncluded = { "Equity" };
                //    List<string> natureExcludedList = natureExcluded.ToList<string>();
                //    List<string> natureIncludedList = natureIncluded.ToList<string>();

                //    //var nature = from nat in natureData.T_SCHEMES_NATUREs
                //    //             where !natureExcludedList.Contains(nat.Nature) //nat.Nature != "N.A" &&
                //    //             orderby nat.Nature
                //    //             select new
                //    //             {
                //    //                 nat.Nature
                //    //             };

                //    var nature = from nat in natureData.T_SCHEMES_NATUREs
                //                 where natureIncludedList.Contains(nat.Nature)
                //                 orderby nat.Nature
                //                 select new
                //                 {
                //                     nat.Nature
                //                 };


                //    if (nature.Count() > 0)
                //    {
                //        DataTable dtNature = null;
                //        dtNature = nature.ToDataTable();
                //        ddlNature.DataSource = dtNature;
                //        ddlNature.DataTextField = "Nature";
                //        ddlNature.DataValueField = "Nature";
                //        ddlNature.DataBind();
                //        ddlNature.Items.Insert(0, new ListItem("--", "0"));
                //        ddlNature.Items.Add(new ListItem("Fixed Income"));
                //        ddlNature.Items.Add(new ListItem("Fund of Funds"));
                //        //ddlNature.Items.Add(new ListItem("FMP"));
                //        //ddlNature.Items.Add(new ListItem("CPOF"));

                //        //  ddlNature.Items.Add(new ListItem("Hybrid"));
                //    }
                //}


                DataTable _dt = AllMethods.getSebiNature();
                ddlNature.Items.Clear();
                ddlNature.Items.Add(new ListItem("--", "-1"));
                foreach (DataRow drow in _dt.Rows)
                {
                    if(drow["sebi_nature"].ToString() != "Solution Oriented"|| drow["sebi_nature"].ToString() != "Other")
                    ddlNature.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }
                ddlNature.Items.Add(new ListItem("International Fund", "777"));
                ddlNature.Items.Add(new ListItem("Passive Debt Funds", "888"));
                ddlNature.Items.Add(new ListItem("Passive Equity Funds", "999"));

                ddlNature.Items[0].Selected = true;
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
                System.Data.DataTable dt = FetchSchemeNew();


                DropDownList drpdwn = (DropDownList)ddl;
                drpdwn.Items.Clear();
                if (dt.Rows.Count > 0)
                {

                    Dictionary<string, string> SchemeInception = new Dictionary<string, string>();
                    Dictionary<string, string> SchemeOptions = new Dictionary<string, string>();


                    drpdwn.DataTextField = "Sch_Short_Name";
                    drpdwn.DataValueField = "Scheme_Id";
                    foreach (DataRow dr in dt.Rows)
                    {
                        //if (dr["Scheme_Id"].ToString() != "17713" && dr["Scheme_Id"].ToString() != "17712" && dr["Scheme_Id"].ToString() != "8400" && dr["Scheme_Id"].ToString() != "8399" && dr["Scheme_Id"].ToString() != "13251")
                        //{
                        ListItem li = new ListItem(dr["Sch_Short_Name"].ToString(), dr["Scheme_Id"].ToString());
                        li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        SchemeInception.Add(dr["Scheme_Id"].ToString(), dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        drpdwn.Items.Add(li);
                        //}
                        SchemeOptions.Add(dr["Scheme_Id"].ToString(), dr["Option_Id"].ToString());
                    }
                    // drpdwn.DataBind();
                    ViewState["SchemeInception"] = SchemeInception;
                    ViewState["SchemeOption"] = SchemeOptions;
                }

                drpdwn.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
                drpdwn.SelectedIndex = 0;
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
                        //if (dr["Scheme_Id"].ToString() != "17713" && dr["Scheme_Id"].ToString() != "17712" && dr["Scheme_Id"].ToString() != "8400" && dr["Scheme_Id"].ToString() != "8399" && dr["Scheme_Id"].ToString() != "13251")
                        //{
                        ListItem li = new ListItem(dr["Sch_Short_Name"].ToString(), dr["Scheme_Id"].ToString());
                        li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        SchemeInception.Add(dr["Scheme_Id"].ToString(), dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        drpdwn.Items.Add(li);
                        //}
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
                    int count = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        // ListItem li = new ListItem(dr["INDEX_NAME"].ToString(), dr["INDEX_ID"].ToString());
                        // ViewState["INDEX_ID"] = dr["INDEX_ID"].ToString();
                        // ViewState["INDEX_NAME"] = dr["INDEX_NAME"].ToString();

                        // labelbenchmark.Text += dr["INDEX_NAME"].ToString() + " , ";
                        //Add new for TRI
                        //string TRIIndex = dr["INDEX_NAME"].ToString() + " TRI";
                        var FetchTRIIndex = AllMethods.FetchTRIIndexName(Convert.ToDecimal(dr["INDEX_ID"].ToString()));
                        if (FetchTRIIndex != null)
                        {
                            if (count == 0)
                                labelbenchmark.Text = "";

                            ViewState["INDEX_ID"] = FetchTRIIndex.IndexID.ToString();
                            ViewState["INDEX_NAME"] = FetchTRIIndex.IndexName.ToString();
                            labelbenchmark.Text += FetchTRIIndex.IndexName + " , ";
                            count++;
                        }
                        //


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
        /// Function Changes on 6 aug 2013 as requested by client

        protected void PopulateFrequency()
        {
            ddPeriod_SIP.Items.Clear();
            if (ddlMode.SelectedItem.Text.ToUpper().StartsWith("SIP"))
            {
                ddPeriod_SIP.Items.Add(new ListItem("Daily", "Daily"));
                ddPeriod_SIP.Items.Add(new ListItem("Weekly", "Weekly"));
                // ddPeriod_SIP.Items.Add(new ListItem("Fortnightly", "Fortnightly"));
                ddPeriod_SIP.Items.Add(new ListItem("Monthly", "Monthly"));
                ddPeriod_SIP.Items.Add(new ListItem("Quarterly", "Quarterly"));
            }
            else
            {
                ddPeriod_SIP.Items.Add(new ListItem("Daily", "Daily"));
                ddPeriod_SIP.Items.Add(new ListItem("Weekly", "Weekly"));
                //ddPeriod_SIP.Items.Add(new ListItem("Fortnightly", "Fortnightly"));
                ddPeriod_SIP.Items.Add(new ListItem("Monthly", "Monthly"));
                ddPeriod_SIP.Items.Add(new ListItem("Quarterly", "Quarterly"));
            }

        }

        public System.Data.DataTable FetchSchemeNew()
        {
            DataTable dt = new DataTable();
            string selected_mode = string.Empty;
            selected_mode = ddlMode.SelectedItem.Text;
            List<decimal?> excludeSubnatureList = new List<decimal?>();
            // excludeSubnatureList.AddRange(new decimal?[] { 2, 21 });// FMP FTP ,Marginal Equity
            excludeSubnatureList.AddRange(new decimal?[] { 2 });// FMP FTP 
            List<decimal?> excludeNatureIdList = new List<decimal?>();
            excludeNatureIdList.AddRange(new decimal?[] { 6 });// Liqidity
            try
            {
                using (var scheme = new PrincipalCalcDataContext())
                {

                    var SqlScheme = (
                        from Sch in scheme.T_SCHEMES_MASTERs
                        from t_fund_masters in scheme.T_FUND_MASTERs
                        from sebi in scheme.T_SEBI_SCHEMES_NATUREs
                        from SchOption in scheme.T_SCHEMES_OPTIONs
                        where t_fund_masters.MUTUALFUND_ID == 55 && Sch.Fund_Id == t_fund_masters.FUND_ID
                        && Sch.Option_Id == SchOption.Option_ID && t_fund_masters.SEBI_NATURE_ID == sebi.Sebi_Nature_ID
                        && Sch.Nav_Check == 3  && Sch.Launch_Date != null
                        select new
                        {
                            t_fund_masters.FUND_ID,
                            t_fund_masters.NATURE_ID,
                            //t_fund_masters.SUB_NATURE_ID,
                            //t_fund_masters.STRUCTURE_ID,
                            sebi.Sebi_Nature_ID,
                            SchOption.Option_ID,
                            Sch.Sch_Short_Name,
                            Sch.Scheme_Id,
                            Sch.Launch_Date,
                        });

                    if (ddlOption.SelectedIndex != 0)
                    {
                        var optionId = Convert.ToDecimal(ddlOption.SelectedValue);
                       SqlScheme = SqlScheme.Where(x => x.Option_ID == optionId);
                    }
                    if (ddlNature.SelectedIndex != 0)
                    {
                        var NatureId = Convert.ToDecimal(ddlNature.SelectedIndex);
                        if (NatureId == 2 || NatureId == 3)
                        {
                            SqlScheme = SqlScheme.Where(x => x.Sebi_Nature_ID == NatureId);
                        }
                        else
                        {
                            var EddSch = (from v in SqlScheme.ToArray()
                                          from edd in scheme.T_IFrame_Edelweiss_Natures
                                          where v.FUND_ID == edd.Fund_Id && edd.Principal_Nature_Id == NatureId
                                          select new
                                          {
                                              v.Sch_Short_Name,
                                              v.Scheme_Id,
                                              v.Launch_Date,
                                              Option_Id = v.Option_ID
                                          }).OrderBy(x => x.Sch_Short_Name).Distinct();

                            DataTable dt3 = null;
                            if (EddSch.Count() > 0)
                                dt3 = EddSch.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                            DataTable Newdt = new DataTable();
                            Newdt = dt3.Copy();
                            return Newdt;
                        }
                    }

                    var scheme_name_1 = SqlScheme.Select(c => new
                    {
                        c.Sch_Short_Name,
                        c.Scheme_Id,
                        c.Launch_Date,
                        c.Option_ID
                    }).OrderBy(x => x.Sch_Short_Name).Distinct();

                    DataTable dt2 = null;
                    if (scheme_name_1.Count() > 0)
                        dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    dt = dt2.Copy();


                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public System.Data.DataTable FetchScheme()
        {
            DataTable dt = new DataTable();
            string selected_mode = string.Empty;
            selected_mode = ddlMode.SelectedItem.Text;
            List<decimal?> excludeSubnatureList = new List<decimal?>();
            // excludeSubnatureList.AddRange(new decimal?[] { 2, 21 });// FMP FTP ,Marginal Equity
            excludeSubnatureList.AddRange(new decimal?[] { 2 });// FMP FTP 
            List<decimal?> excludeNatureIdList = new List<decimal?>();
            excludeNatureIdList.AddRange(new decimal?[] { 6 });// Liqidity
            try
            {
                using (var scheme = new PrincipalCalcDataContext())
                {
                    if (ddlOption.SelectedIndex == 0)
                    {
                        if (ddlNature.SelectedIndex == 0)// Nature is not Selected
                        {
                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                               t_fund_masters.MUTUALFUND_ID == 55
                                             //  && t_fund_masters.STRUCTURE_ID ==2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            if (selected_mode.Trim().ToUpper().StartsWith("SIP"))
                            {
                                fundtable = fundtable.Where(x => x.STRUCTURE_ID == 2 && x.FUND_ID != 3094);
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
                                                 s.Nav_Check == 3 //&& s.Option_Id == 2 
                                                 && s.Launch_Date != null
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date,
                                                     s.Option_Id
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
                            if (selected_nature == "Fixed Income")
                            {
                                lstnatureSelected.Add("Debt"); lstnatureSelected.Add("Liquid"); lstnatureSelected.Add("Gilt"); lstnatureSelected.Add("Dynamic/Asset Allocation");
                            }
                            else if (selected_nature == "FMP")
                            {
                                lstnatureSelected.Add("Debt");
                            }
                            else if (selected_nature == "CPOF")
                            {
                                //lstnatureSelected.Add("Dynamic/Asset Allocation");
                            }
                            else if(selected_nature == "Fund of Funds")
                            {
                                lstnatureSelected.Add("Fund of Funds");
                            }
                            else
                            {
                                lstnatureSelected.Add(selected_nature);
                                lstnatureSelected.Add("Balanced");
                            }
                            var natureList = (from t_schemes_natures in scheme.T_SCHEMES_NATUREs
                                              where lstnatureSelected.Contains(t_schemes_natures.Nature)
                                              select t_schemes_natures.Nature_ID).ToList();

                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            if (selected_nature == "FMP")
                            {
                                fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                                && t_fund_masters.SUB_NATURE_ID.Value == 2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            }

                            if (selected_nature == "Fixed Income")
                            {
                                fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                                && t_fund_masters.SUB_NATURE_ID.Value != 2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            }
                            // Need to show Oped ended scheme for sip scheme
                            if (selected_mode.Trim().ToUpper().StartsWith("SIP"))
                            {
                                fundtable = fundtable.Where(x => x.STRUCTURE_ID == 2 && x.FUND_ID != 3094);
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
                                                 && s.Launch_Date != null
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci                                              
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date,
                                                     s.Option_Id
                                                 }).Distinct();
                            DataTable dt2 = new DataTable();
                            if (scheme_name_1.Count() > 0)
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                            dt = dt2.Copy();
                        }
                    }
                    else
                    {
                        var optionId = Convert.ToDecimal(ddlOption.SelectedValue);
                        if (ddlNature.SelectedIndex == 0)// Nature is not Selected
                        {
                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                               t_fund_masters.MUTUALFUND_ID == 55
                                             //  && t_fund_masters.STRUCTURE_ID ==2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            if (selected_mode.Trim().ToUpper().StartsWith("SIP"))
                            {
                                fundtable = fundtable.Where(x => x.STRUCTURE_ID == 2 && x.FUND_ID != 3094);
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
                                                 s.Nav_Check == 3 //&& s.Option_Id == 2 
                                                 && s.Launch_Date != null
                                                 && s.Option_Id == optionId // add by syed
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date,
                                                     s.Option_Id
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
                            if (selected_nature == "Fixed Income")
                            {
                                lstnatureSelected.Add("Debt"); lstnatureSelected.Add("Liquid"); lstnatureSelected.Add("Gilt"); lstnatureSelected.Add("Dynamic/Asset Allocation");
                            }
                            else if (selected_nature == "FMP")
                            {
                                lstnatureSelected.Add("Debt");
                            }
                            else if (selected_nature == "CPOF")
                            {
                                //lstnatureSelected.Add("Dynamic/Asset Allocation");
                            }
                            else if (selected_nature == "Fund of Funds")
                            {
                                lstnatureSelected.Add("Fund of Funds");
                            }
                            else
                            {
                                lstnatureSelected.Add(selected_nature);
                                lstnatureSelected.Add("Balanced");
                            }
                            var natureList = (from t_schemes_natures in scheme.T_SCHEMES_NATUREs
                                              where lstnatureSelected.Contains(t_schemes_natures.Nature)
                                              select t_schemes_natures.Nature_ID).ToList();

                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            if (selected_nature == "FMP")
                            {
                                fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                                && t_fund_masters.SUB_NATURE_ID.Value == 2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            }

                            if (selected_nature == "Fixed Income")
                            {
                                fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                                && t_fund_masters.SUB_NATURE_ID.Value != 2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            }
                            // Need to show Oped ended scheme for sip scheme
                            if (selected_mode.Trim().ToUpper().StartsWith("SIP"))
                            {
                                fundtable = fundtable.Where(x => x.STRUCTURE_ID == 2 && x.FUND_ID != 3094);
                            }
                            DataTable dtt = new DataTable();
                            if (fundtable.Count() > 0)
                                dtt = fundtable.ToDataTable();
                            var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                                 join T in fundtable
                                                 on s.Fund_Id equals T.FUND_ID
                                                 where s.Nav_Check == 3//s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 && s.Nav_Check != 2
                                                  && s.Option_Id == optionId // add by syed
                                                                             //&& T.SUB_NATURE_ID !=2
                                                                             // && !excludeSubnatureList.Contains(T.SUB_NATURE_ID)
                                                                             //&& s.Launch_Date <= yearBacktodaysdate1
                                                 && s.Launch_Date != null
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci                                              
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date,
                                                     s.Option_Id
                                                 }).Distinct();
                            DataTable dt2 = new DataTable();
                            if (scheme_name_1.Count() > 0)
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                            dt = dt2.Copy();
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public System.Data.DataTable FetchSchemeNew()
        {
            DataTable dt = new DataTable();
            string selected_mode = string.Empty;
            selected_mode = ddlMode.SelectedItem.Text;
            List<decimal?> excludeSubnatureList = new List<decimal?>();
            // excludeSubnatureList.AddRange(new decimal?[] { 2, 21 });// FMP FTP ,Marginal Equity
            excludeSubnatureList.AddRange(new decimal?[] { 2 });// FMP FTP 
            List<decimal?> excludeNatureIdList = new List<decimal?>();
            excludeNatureIdList.AddRange(new decimal?[] { 6 });// Liqidity
            try
            {
                using (var scheme = new PrincipalCalcDataContext())
                {
                    #region New
                    //fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                    //             where
                    //                 t_fund_masters.MUTUALFUND_ID == 55 &&
                    //                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                    //                && t_fund_masters.SUB_NATURE_ID.Value == 2
                    //             select new
                    //             {
                    //                 t_fund_masters.FUND_ID,
                    //                 t_fund_masters.NATURE_ID,
                    //                 t_fund_masters.SUB_NATURE_ID,
                    //                 t_fund_masters.STRUCTURE_ID
                    //             });
                    var ChkSch = (
                        from  sch in scheme.T_SCHEMES_MASTERs
                        from t_fund_masters in scheme.T_FUND_MASTERs
                                  from fund in scheme.
                                  where
                                      t_fund_masters.MUTUALFUND_ID == 55 && sch.Fund_Id==t_fund_masters.FUND_ID 
                                  select new
                                  {
                                      t_fund_masters.FUND_ID,
                                      t_fund_masters.NATURE_ID,
                                      t_fund_masters.SUB_NATURE_ID,
                                      t_fund_masters.STRUCTURE_ID,
                                     
                                  });

                    var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                         join T in fundtable
                                         on s.Fund_Id equals T.FUND_ID
                                         where //s.T_FUND_MASTER.SUB_NATURE_ID != 2 &&  s.Nav_Check != 2
                                               // !excludeSubnatureList.Contains(T.SUB_NATURE_ID) &&
                                               // T.SUB_NATURE_ID!= 2 &&                                             
                                         s.Nav_Check == 3 //&& s.Option_Id == 2 
                                         && s.Launch_Date != null
                                         && s.Option_Id == optionId // add by syed
                                         join tsi in scheme.T_SCHEMES_INDEXes
                                         on s.Scheme_Id equals tsi.SCHEME_ID
                                         orderby s.Sch_Short_Name
                                         select new
                                         {
                                             s.Sch_Short_Name,
                                             s.Scheme_Id,
                                             s.Launch_Date,
                                             s.Option_Id
                                         }).Distinct();
                    #endregion new

                    if (ddlOption.SelectedIndex == 0)
                    {
                        if (ddlNature.SelectedIndex == 0)// Nature is not Selected
                        {
                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                               t_fund_masters.MUTUALFUND_ID == 55
                                             //  && t_fund_masters.STRUCTURE_ID ==2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            if (selected_mode.Trim().ToUpper().StartsWith("SIP"))
                            {
                                fundtable = fundtable.Where(x => x.STRUCTURE_ID == 2 && x.FUND_ID != 3094);
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
                                                 s.Nav_Check == 3 //&& s.Option_Id == 2 
                                                 && s.Launch_Date != null
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date,
                                                     s.Option_Id
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
                            if (selected_nature == "Fixed Income")
                            {
                                lstnatureSelected.Add("Debt"); lstnatureSelected.Add("Liquid"); lstnatureSelected.Add("Gilt"); lstnatureSelected.Add("Dynamic/Asset Allocation");
                            }
                            else if (selected_nature == "FMP")
                            {
                                lstnatureSelected.Add("Debt");
                            }
                            else if (selected_nature == "CPOF")
                            {
                                //lstnatureSelected.Add("Dynamic/Asset Allocation");
                            }
                            else if (selected_nature == "Fund of Funds")
                            {
                                lstnatureSelected.Add("Fund of Funds");
                            }
                            else
                            {
                                lstnatureSelected.Add(selected_nature);
                                lstnatureSelected.Add("Balanced");
                            }
                            var natureList = (from t_schemes_natures in scheme.T_SCHEMES_NATUREs
                                              where lstnatureSelected.Contains(t_schemes_natures.Nature)
                                              select t_schemes_natures.Nature_ID).ToList();

                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            if (selected_nature == "FMP")
                            {
                                fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                                && t_fund_masters.SUB_NATURE_ID.Value == 2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            }

                            if (selected_nature == "Fixed Income")
                            {
                                fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                                && t_fund_masters.SUB_NATURE_ID.Value != 2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            }
                            // Need to show Oped ended scheme for sip scheme
                            if (selected_mode.Trim().ToUpper().StartsWith("SIP"))
                            {
                                fundtable = fundtable.Where(x => x.STRUCTURE_ID == 2 && x.FUND_ID != 3094);
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
                                                 && s.Launch_Date != null
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci                                              
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date,
                                                     s.Option_Id
                                                 }).Distinct();
                            DataTable dt2 = new DataTable();
                            if (scheme_name_1.Count() > 0)
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                            dt = dt2.Copy();
                        }
                    }
                    else
                    {
                        var optionId = Convert.ToDecimal(ddlOption.SelectedValue);
                        if (ddlNature.SelectedIndex == 0)// Nature is not Selected
                        {
                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                               t_fund_masters.MUTUALFUND_ID == 55
                                             //  && t_fund_masters.STRUCTURE_ID ==2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            if (selected_mode.Trim().ToUpper().StartsWith("SIP"))
                            {
                                fundtable = fundtable.Where(x => x.STRUCTURE_ID == 2 && x.FUND_ID != 3094);
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
                                                 s.Nav_Check == 3 //&& s.Option_Id == 2 
                                                 && s.Launch_Date != null
                                                 && s.Option_Id == optionId // add by syed
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date,
                                                     s.Option_Id
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
                            if (selected_nature == "Fixed Income")
                            {
                                lstnatureSelected.Add("Debt"); lstnatureSelected.Add("Liquid"); lstnatureSelected.Add("Gilt"); lstnatureSelected.Add("Dynamic/Asset Allocation");
                            }
                            else if (selected_nature == "FMP")
                            {
                                lstnatureSelected.Add("Debt");
                            }
                            else if (selected_nature == "CPOF")
                            {
                                //lstnatureSelected.Add("Dynamic/Asset Allocation");
                            }
                            else if (selected_nature == "Fund of Funds")
                            {
                                lstnatureSelected.Add("Fund of Funds");
                            }
                            else
                            {
                                lstnatureSelected.Add(selected_nature);
                                lstnatureSelected.Add("Balanced");
                            }
                            var natureList = (from t_schemes_natures in scheme.T_SCHEMES_NATUREs
                                              where lstnatureSelected.Contains(t_schemes_natures.Nature)
                                              select t_schemes_natures.Nature_ID).ToList();

                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            if (selected_nature == "FMP")
                            {
                                fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                                && t_fund_masters.SUB_NATURE_ID.Value == 2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            }

                            if (selected_nature == "Fixed Income")
                            {
                                fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 55 &&
                                                natureList.Contains(t_fund_masters.NATURE_ID.Value)
                                                && t_fund_masters.SUB_NATURE_ID.Value != 2
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID,
                                                 t_fund_masters.NATURE_ID,
                                                 t_fund_masters.SUB_NATURE_ID,
                                                 t_fund_masters.STRUCTURE_ID
                                             });
                            }
                            // Need to show Oped ended scheme for sip scheme
                            if (selected_mode.Trim().ToUpper().StartsWith("SIP"))
                            {
                                fundtable = fundtable.Where(x => x.STRUCTURE_ID == 2 && x.FUND_ID != 3094);
                            }
                            DataTable dtt = new DataTable();
                            if (fundtable.Count() > 0)
                                dtt = fundtable.ToDataTable();
                            var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                                 join T in fundtable
                                                 on s.Fund_Id equals T.FUND_ID
                                                 where s.Nav_Check == 3//s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 && s.Nav_Check != 2
                                                  && s.Option_Id == optionId // add by syed
                                                                             //&& T.SUB_NATURE_ID !=2
                                                                             // && !excludeSubnatureList.Contains(T.SUB_NATURE_ID)
                                                                             //&& s.Launch_Date <= yearBacktodaysdate1
                                                 && s.Launch_Date != null
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci                                              
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date,
                                                     s.Option_Id
                                                 }).Distinct();
                            DataTable dt2 = new DataTable();
                            if (scheme_name_1.Count() > 0)
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                            dt = dt2.Copy();
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public System.Data.DataTable FetchEdelweissSchemes()
        {
            DataTable dt = new DataTable();
            //int selected_natureID = ddlNature.SelectedValue == "777" || ddlNature.SelectedValue == "888" || ddlNature.SelectedValue == "999" ? 5 : Convert.ToInt32(ddlNature.SelectedValue);

            if (ddlNature.SelectedValue == "777" || ddlNature.SelectedValue == "888" || ddlNature.SelectedValue == "999")
            {

            }
            else
            {
                var sebi
            }





            return null;
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
                                       t_fund_masters.MUTUALFUND_ID == 55
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
                            s.Launch_Date,
                            s.Option_Id
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
            DataTable SipDtable1, SipDtable2, sipDTable3;
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

                if (rdbPayout.Checked)
                    strInvestorType = "Payout";

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
                objFundDescList.AddRange(GetSundFundDesc(schmeId));

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
                else if (ddPeriod_SIP.SelectedItem.Value == "Weekly")
                {
                    PrdVal = 2;
                }
                else if (ddPeriod_SIP.SelectedItem.Text == "Daily")
                {
                    PrdVal = 4;
                }
                else
                {
                    return;
                }



                if (PrdVal == 1)
                {

                    tempDate = sipEndDate.AddMonths(-1);//12
                    tmspan = tempDate.Subtract(sipStartDate);
                    daydiff = tmspan.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"SIP is allowed for minimum 1 Installment\")</script>");
                        return;
                    }

                }
                else if (PrdVal == 3)
                {

                    tempDate = sipEndDate.AddMonths(-2);
                    tmspan = tempDate.Subtract(sipStartDate);
                    daydiff = tmspan.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"SIP is allowed for minimum 1 Installment\")</script>");
                        return;
                    }

                }
                else if (PrdVal == 2)
                {

                    tempDate = sipEndDate.AddDays(-7);
                    tmspan = tempDate.Subtract(sipStartDate);
                    daydiff = tmspan.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"SIP is allowed for minimum 1 Installment\")</script>");
                        return;
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
                    var thirdTable = datatble.GetResult<CalcReturnDataClient3>();
                    SipDtable1 = firstTable.ToDataTable();
                    SipDtable2 = secondTable.ToDataTable();


                    // Specially for third table
                    var divHistoryTbl = thirdTable.ToList();
                    sipDTable3 = divHistoryTbl.ToDataTable();

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



                    #region remove Index Row 30/10/2012
                    //if (SipDtable2.Rows.Count == 2)
                    //{
                    //    SipDtable2.Rows.RemoveAt(1);
                    //}
                    #endregion


                    double divAmountTotal = 0;
                    if (divHistoryTbl.Any() && rdbPayout.Checked)
                    {
                        divAmountTotal = Math.Round(divHistoryTbl.Select(p => new { DivAmt = Math.Round(p.PAYOUT_AMOUNT.Value, 2) }.DivAmt).Sum(), 2);
                    }

                    #region Add Absolute Return //14.01.2013
                    SipDtable2.Columns.Add("ABSOLUTERETURN");

                    if (SipDtable2.Rows[0]["TOTAL_AMOUNT"] != DBNull.Value && SipDtable2.Rows[0]["PROFIT_SIP"] != DBNull.Value)
                    {
                        // First Add Dividend Amount To The Calculated Profit

                        SipDtable2.Rows[0]["PROFIT_SIP"] = (Convert.ToDouble(SipDtable2.Rows[0]["Present_Value"]) + divAmountTotal) - Convert.ToDouble(SipDtable2.Rows[0]["TOTAL_AMOUNT"]);
                        SipDtable2.AcceptChanges();

                        // Changed To Add Dividend Amount Somabrata
                        SipDtable2.Rows[0]["ABSOLUTERETURN"] = Math.Round(Convert.ToDouble((Convert.ToDouble(SipDtable2.Rows[0]["PROFIT_SIP"])) / Convert.ToDouble(SipDtable2.Rows[0]["TOTAL_AMOUNT"]) * 100), 2);
                    }


                    if (SipDtable2.Rows.Count > 1)
                    {

                        if (SipDtable2.Rows[1]["TOTAL_AMOUNT"] != DBNull.Value && SipDtable2.Rows[1]["PROFIT_SIP"] != DBNull.Value && Convert.ToDouble(SipDtable2.Rows[1]["PROFIT_SIP"]) != 0)
                        {
                            SipDtable2.Rows[1]["ABSOLUTERETURN"] = Math.Round(Convert.ToDouble(Convert.ToDouble(SipDtable2.Rows[1]["PROFIT_SIP"]) / Convert.ToDouble(SipDtable2.Rows[1]["TOTAL_AMOUNT"]) * 100), 2);
                        }
                    }

                    #endregion



                    // Add Dividend Amount At The End Of The Table

                    SipDtable2.Columns.Add("Dividend_Amount");
                    SipDtable2.Rows[0]["Dividend_Amount"] = divAmountTotal;

                    // If dividend reinvest then dividend amount column hides (client requirement)
                    ((DataControlField)gvFirstTable.Columns
                        .Cast<DataControlField>()
                        .Where(fld => fld.HeaderText.ToLower().Contains("dividend"))
                        .SingleOrDefault()).Visible = rdbPayout.Checked && IsSchemeDividend;

                    gvFirstTable.DataSource = SipDtable2;
                    gvFirstTable.DataBind();

                    sipGridView.DataSource = SipDtable1;
                    sipGridView.DataBind();

                    gridDivHistory.DataSource = sipDTable3;
                    gridDivHistory.DataBind();

                    // Somabrata
                    //sipDisclaimer.InnerText = string.Empty;
                    //sipDisclaimer.Visible = tr_Cal_Type.Visible;

                    //if (divHistoryTbl.Any() && rdbPayout.Checked)
                    //{
                    //    var divAmountTotal = Math.Round(divHistoryTbl.Select(p => new { DivAmt = Math.Round(p.PAYOUT_AMOUNT.Value, 2) }.DivAmt).Sum(), 2);
                    //    sipDisclaimer.InnerText = "Total amount paid towards dividend distribution : Rs. " + divAmountTotal.ToString();
                    //}

                    if (swpGridView.Visible) swpGridView.Visible = false;
                    if (gvSWPSummaryTable.Visible) gvSWPSummaryTable.Visible = false;

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
                        if (dc.ColumnName.ToUpper() != "NAV_DATE" && dc.ColumnName.ToUpper() != "CUMULATIVE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT" && dc.ColumnName.ToUpper() != "INDEX_VALUE_AMOUNT")// &&  dc.ColumnName.ToUpper() != "INDEX_VALUE_AMOUNT"
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

                lblInvestment.Text = "Investment amount per month : <b>₹ " + txtinstall.Text + "</b>";
                if (SipDtable2.Rows.Count > 0)
                {
                    totalInvestAmount = Math.Round(Convert.ToDouble(SipDtable2.Rows[0]["TOTAL_AMOUNT"]), 2);
                    lblTotalInvst.Text = "Total Investment Amount : <b>₹ " + totalInvestAmount.ToString() + "</b>";
                    ViewState["totalInvestedAmount"] = totalInvestAmount;//set viewstate


                    if (SipDtable2.Rows[0]["PRESENT_VALUE"].ToString() != "N/A" && Convert.ToDouble(SipDtable2.Rows[0]["PRESENT_VALUE"]) != 0.0)
                    {
                        presntInvestValue = Math.Round(Convert.ToDouble(SipDtable2.Rows[0]["PRESENT_VALUE"]), 2);
                        daysDiffValAbs = Math.Round(((double)(presntInvestValue - totalInvestAmount) / totalInvestAmount) * 100, 2);


                        lblInvstvalue.Text = "On " + txtvalason.Text + ", the Scheme value of your total investment <b>₹ " + totalInvestAmount.ToString() + "</b> would be <b>₹  " + presntInvestValue.ToString() + "</b>";

                        ViewState["presentInvestValue"] = presntInvestValue;// set viewstate

                    }
                    else
                        lblInvstvalue.Text = "On " + txtvalason.Text + ", the Scheme value of your total investment <b>₹ " + totalInvestAmount.ToString() + "</b> would be ₹ N/A";


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
                        lblIfInvested.Text = "Had you invested <b>₹ " + totalInvestAmount.ToString() + "</b> at " + txtfromDate.Text + ", the total value of this investment at " + txtvalason.Text + " in Scheme would have become <b>₹ " + schemeReturnOneTime.ToString() + "</b>";
                    }
                    else
                        lblIfInvested.Text = "Had you invested <b>₹ " + totalInvestAmount.ToString() + "</b> at " + txtfromDate.Text + ", the total value of this investment at " + txtvalason.Text + " in Scheme would have become N/A";

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

        //private List<string> GetFundDesc(string schmeId)
        //{
        //    List<string> ListFundDesc = new List<string>();
        //    using (var iframeClient = new SIP_ClientDataContext())
        //    {

        //        var FundDesc = (from fd in iframeClient.T_Iframe_DSP_Fund_Descs
        //                        join fm in iframeClient.T_FUND_MASTER_clients
        //                        on fd.Fund_Id equals fm.FUND_ID
        //                        join sm in iframeClient.T_SCHEMES_MASTER_Clients
        //                        on fm.FUND_ID equals sm.Fund_Id
        //                        where sm.Scheme_Id == Convert.ToDecimal(schmeId)
        //                        select new
        //                        {
        //                            fd.Desc1,
        //                            fd.Desc2,
        //                            fd.Desc3
        //                        }).FirstOrDefault();

        //        if (FundDesc != null)
        //        {
        //            ListFundDesc.Add(FundDesc.Desc1);
        //            ListFundDesc.Add(FundDesc.Desc2);
        //            ListFundDesc.Add(FundDesc.Desc3);
        //        }

        //    }
        //    return ListFundDesc;
        //}


        private List<string> GetSundFundDesc(string schmeId)
        {
            List<string> ListFundDesc = new List<string>();
            using (var iframeClient = new SIP_ClientDataContext())
            {

                //var FundDesc = (from fm in iframeClient.T_FUND_MASTER_clients
                //                join sm in iframeClient.T_SCHEMES_MASTER_Clients
                //                on fm.FUND_ID equals sm.Fund_Id
                //                join fcm in iframeClient.T_SCHEME_INFO_FUND_COLOR_MASTs
                //               on fm.FUND_COLOR_MAST_ID equals fcm.ID
                //                where sm.Scheme_Id == Convert.ToDecimal(schmeId)
                //                select new
                //                {
                //                    fm.FUND_OBJECT,
                //                    fm.FUND_ID,
                //                    fcm.DENOMINATE
                //                }).FirstOrDefault();

                var FundDesc = (from fm in iframeClient.T_FUND_MASTER_clients
                                join sm in iframeClient.T_SCHEMES_MASTER_Clients
                                on fm.FUND_ID equals sm.Fund_Id
                                join inv in iframeClient.T_Iframe_DSP_Fund_Descs on
                                fm.FUND_ID equals inv.Fund_Id
                                join fcm in iframeClient.T_SCHEME_INFO_FUND_COLOR_MASTs
                               on fm.FUND_COLOR_MAST_ID equals fcm.ID
                                where sm.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    inv.Desc1,
                                    inv.Desc2,
                                    inv.Desc3,
                                    inv.Desc4,
                                    inv.Desc5,
                                    inv.HeaderText,
                                    fm.FUND_OBJECT,
                                    fm.FUND_ID,
                                    fcm.DENOMINATE
                                }).FirstOrDefault();

                if (FundDesc != null)
                {
                    string fndObjective = "";
                    if (!string.IsNullOrEmpty(FundDesc.HeaderText) && !string.IsNullOrWhiteSpace(FundDesc.HeaderText))
                    {
                        fndObjective = FundDesc.HeaderText;

                        if (!string.IsNullOrEmpty(FundDesc.Desc1) || !string.IsNullOrEmpty(FundDesc.Desc2) || !string.IsNullOrEmpty(FundDesc.Desc3) || !string.IsNullOrEmpty(FundDesc.Desc4) || !string.IsNullOrEmpty(FundDesc.Desc5))
                        {
                            fndObjective += "<br><br>How do you gain<ul>";
                            if (!string.IsNullOrEmpty(FundDesc.Desc1))
                            {
                                fndObjective += "<li>" + FundDesc.Desc1 + "</li>";
                            }
                            if (!string.IsNullOrEmpty(FundDesc.Desc2))
                            {
                                fndObjective += "<li>" + FundDesc.Desc2 + "</li>";
                            }
                            if (!string.IsNullOrEmpty(FundDesc.Desc3))
                            {
                                fndObjective += "<li>" + FundDesc.Desc3 + "</li>";
                            }
                            if (!string.IsNullOrEmpty(FundDesc.Desc4))
                            {
                                fndObjective += "<li>" + FundDesc.Desc4 + "</li>";
                            }
                            if (!string.IsNullOrEmpty(FundDesc.Desc5))
                            {
                                fndObjective += "<li>" + FundDesc.Desc5 + "</li>";
                            }

                            fndObjective += "</ul>";
                        }
                    }
                    else
                    {
                        fndObjective = FundDesc.FUND_OBJECT;
                    }

                    ListFundDesc.Add(fndObjective);
                    ListFundDesc.Add(FundDesc.FUND_ID.ToString());
                    ListFundDesc.Add(FundDesc.DENOMINATE);
                    // ListFundDesc.Add(Convert.ToString(FundDesc.COLOR_CODE));
                }

            }
            return ListFundDesc;
        }

        /// <summary>
        /// This Function will Show Result if SWP is Selected
        /// </summary>

        public void CalculateReturnSWP()
        {
            #region Initialize
            // bool flgIsCompositeIndex = false;
            DateTime swpStartDate, swpEndDate, swpAsonDate, tempDate, investmentDate, allotDate, initialStartDate;
            TimeSpan datedifference;
            int daydiff, swpDate, PrdVal, tempInt;
            double dblSwpTransferAmnt, dblswpIntialAmnt;
            string strFrequency = string.Empty, schemeId = string.Empty, strInvestorType = string.Empty;
            SqlCommand cmdswp = null;
            DataSet dSet = new DataSet("swpDataSet");
            DataTable dtble = new DataTable("swpDataTable"), datatTble = new DataTable("swpDT"), datatTbleHistorical = new DataTable("datatTbleHistorical");
            //double? totalWithdrawAmnt = null, prsntInvstVal = null, yieldReturn = null;
            DataTable dwsFirstDataTable = new DataTable();
            conn.ConnectionString = connstr;
            string strRollingPeriodin = "1 YYYY,3 YYYY,5 YYYY,0 Si";
            int val = 0;
            #endregion


            try
            {
                #region set Value

                swpStartDate = new DateTime(Convert.ToInt16(txtfromDate.Text.Split('/')[2]),
                                      Convert.ToInt16(txtfromDate.Text.Split('/')[1]),
                                      Convert.ToInt16(txtfromDate.Text.Split('/')[0]));

                swpEndDate = new DateTime(Convert.ToInt16(txtToDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[0]));

                swpAsonDate = new DateTime(Convert.ToInt16(txtvalason.Text.Split('/')[2]),
                                     Convert.ToInt16(txtvalason.Text.Split('/')[1]),
                                     Convert.ToInt16(txtvalason.Text.Split('/')[0]));



                dblSwpTransferAmnt = Convert.ToDouble(txtTransferWithdrawal2.Text);
                dblswpIntialAmnt = Convert.ToDouble(txtiniAmount.Text);

                swpDate = Convert.ToInt32(ddSIPdate.SelectedItem.Value);
                strFrequency = ddPeriod_SIP.SelectedItem.Text;
                schemeId = ddlscheme.SelectedItem.Value;
                strInvestorType = "Individual/Huf";
                PrdVal = 1;
                string indexId = string.Empty;
                if (ViewState["INDEX_ID"] != null)
                    indexId = ViewState["INDEX_ID"].ToString();
                //else
                //    indexId = ddlsipbnmark.SelectedItem.Value;


                #endregion

                initialStartDate = new DateTime(Convert.ToInt16(txtIniToDate.Text.Split('/')[2]),
                                     Convert.ToInt16(txtIniToDate.Text.Split('/')[1]),
                                     Convert.ToInt16(txtIniToDate.Text.Split('/')[0]));
                //   swpStartDate = swpStartDate.AddDays(7);/// add 7 days for transaction time

                #region condition check
                if (ddPeriod_SIP.SelectedItem.Text == "Monthly")
                {
                    PrdVal = 1;
                }
                else if (ddPeriod_SIP.SelectedItem.Text == "Quarterly")
                {
                    PrdVal = 3;
                }
                else if (ddPeriod_SIP.SelectedItem.Text == "Weekly")
                {
                    PrdVal = 2;
                }
                else if (ddPeriod_SIP.SelectedItem.Text == "Daily")
                {
                    PrdVal = 4;
                }
                else
                {
                    return;
                }

                //--v01-Apr-08---validation for at least 6  transfers by jscript



                if (PrdVal == 1)
                {
                    tempDate = swpEndDate.AddMonths(-1);
                    datedifference = tempDate.Subtract(swpStartDate);
                    daydiff = datedifference.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"SWP is allowed for minimum 1 withdrawal\")</script>");
                        return;
                    }
                }
                else if (PrdVal == 3)
                {
                    tempDate = swpEndDate.AddMonths(-2);
                    datedifference = tempDate.Subtract(swpStartDate);
                    daydiff = datedifference.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"SWP is allowed for minimum 1 withdrawal\")</script>");
                        return;
                    }
                }
                else if (PrdVal == 2)
                {
                    tempDate = swpEndDate.AddDays(-7);
                    datedifference = tempDate.Subtract(swpStartDate);
                    daydiff = datedifference.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"SWP is allowed for minimum 1 withdrawal\")</script>");
                        return;
                    }
                }


                tempInt = swpStartDate.Day;
                if (swpDate < tempInt)
                {
                    if (swpStartDate.Month != 12)
                        investmentDate = new DateTime(swpStartDate.Year, swpStartDate.Month + 1, swpDate);
                    else
                        investmentDate = new DateTime(swpStartDate.Year + 1, 1, swpDate);
                }
                else if (swpDate == tempInt)
                {
                    investmentDate = swpStartDate;
                }
                else
                {
                    investmentDate = new DateTime(swpStartDate.Year, swpStartDate.Month, swpDate);
                }
                #endregion

                # region calling SP


                //  cmdswp = new SqlCommand("MFIE_SP_SWP_Calculater", conn);
                cmdswp = new SqlCommand("MFIE_SP_SWP_Calculater_client", conn);
                cmdswp.CommandType = CommandType.StoredProcedure;
                cmdswp.Parameters.Add(new SqlParameter("@Scheme_Ids", schemeId));
                cmdswp.Parameters.Add(new SqlParameter("@Plan_Start_Date", investmentDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));//swpStartDate
                cmdswp.Parameters.Add(new SqlParameter("@Plan_End_Date", swpEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdswp.Parameters.Add(new SqlParameter("@Report_As_On", swpAsonDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdswp.Parameters.Add(new SqlParameter("@SWP_Amt", dblSwpTransferAmnt));
                cmdswp.Parameters.Add(new SqlParameter("@Frequency", strFrequency));
                cmdswp.Parameters.Add(new SqlParameter("@Dividend_Type", strInvestorType));
                cmdswp.Parameters.Add(new SqlParameter("@Investment_Amount", dblswpIntialAmnt));
                cmdswp.Parameters.Add(new SqlParameter("@Investment_Date", initialStartDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

                SqlDataAdapter da = new SqlDataAdapter(cmdswp);
                da.Fill(dSet);

                if (dSet.Tables.Count > 0)
                {
                    datatTble = dSet.Tables[0];


                    //datatTble.Rows.RemoveAt(datatTble.Rows.Count - 1);



                    ViewState["swpDataTable"] = datatTble;

                    //Summary TAble
                    if (dSet.Tables.Count > 1)
                    {
                        dwsFirstDataTable = dSet.Tables[1];
                        ViewState["dwsFirstDataTableSummary"] = dwsFirstDataTable;
                    }



                    #region Add Absolute Return //14.01.2013
                    dwsFirstDataTable.Columns.Add("ABSOLUTERETURN");

                    if (dwsFirstDataTable.Rows[0]["TOTAL_AMOUNT_INVESTED"] != DBNull.Value && dwsFirstDataTable.Rows[0]["TOTAL_AMOUNT_WITHDRAWN"] != DBNull.Value && dwsFirstDataTable.Rows[0]["PRESENT_VALUE"] != DBNull.Value)
                    {
                        double dblTotalAmount = Convert.ToDouble(dwsFirstDataTable.Rows[0]["PRESENT_VALUE"]) + Convert.ToDouble(dwsFirstDataTable.Rows[0]["TOTAL_AMOUNT_WITHDRAWN"]);
                        double profit = dblTotalAmount - Convert.ToDouble(dwsFirstDataTable.Rows[0]["TOTAL_AMOUNT_INVESTED"]);
                        dwsFirstDataTable.Rows[0]["ABSOLUTERETURN"] = Math.Round((profit / Convert.ToDouble(dwsFirstDataTable.Rows[0]["TOTAL_AMOUNT_INVESTED"])) * 100, 2);
                    }

                    #endregion

                    #region : Set GridView

                    gvSWPSummaryTable.DataSource = dwsFirstDataTable;
                    gvSWPSummaryTable.DataBind();

                    //
                    datatTble.Rows[datatTble.Rows.Count - 1]["CUMILATIVE_UNITS"] = DBNull.Value;
                    datatTble.Rows[datatTble.Rows.Count - 1]["CUMILATIVE_AMOUNT"] = DBNull.Value;

                    swpGridView.DataSource = datatTble;
                    swpGridView.DataBind();


                    #region Remove Bonus Row For Graph Calculation

                    if (datatTble.Rows.Count > 0)
                    {
                        if (datatTble.Rows.Count > 2)
                        {
                            for (int i = datatTble.Rows.Count - 2; i >= 0; i--)
                            {
                                if (datatTble.Rows[i]["FINAL_INVST_AMOUNT"].ToString().Trim() == "0")
                                {
                                    datatTble.Rows.RemoveAt(i);
                                }
                            }

                        }
                    }


                    #endregion


                    if (!swpGridView.Visible) swpGridView.Visible = true;
                    if (!gvSWPSummaryTable.Visible) gvSWPSummaryTable.Visible = true;

                    if (gvFirstTable.Visible) gvFirstTable.Visible = false;
                    if (sipGridView.Visible) sipGridView.Visible = false;


                    if (dwsFirstDataTable.Rows.Count > 0)
                    {
                        double totalWithDrawAmount = Math.Round(Convert.ToDouble(dwsFirstDataTable.Rows[0]["TOTAL_AMOUNT_WITHDRAWN"]), 2);
                        ViewState["TotalWithdrawalAmount"] = totalWithDrawAmount;//set viewstate

                        if (dwsFirstDataTable.Rows[0]["PRESENT_VALUE"].ToString() != "N/A")// && Convert.ToDouble(dwsFirstDataTable.Rows[0]["PRESENT_VALUE"]) != 0.0)
                        {
                            double presntInvestValue = Math.Round(Convert.ToDouble(dwsFirstDataTable.Rows[0]["PRESENT_VALUE"]), 2);
                            ViewState["presentInvestValue"] = presntInvestValue;// set viewstate
                        }

                    }

                    #endregion


                }




                #endregion


                #region Corpus
                fetchCorpus(schemeId);
                #endregion

                #region Launch Date
                using (var principl = new PrincipalCalcDataContext())
                {
                    var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                    where ind.Scheme_Id == Convert.ToDecimal(schemeId)
                                    select new
                                    {
                                        LaunchDate = ind.Launch_Date
                                    };



                    // SWPSchDt.Text = "";
                    if (allotdate != null && allotdate.Count() > 0)
                    {
                        allotDate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                        // SWPSchDt.Text = "<b>" + allotDate.ToShortDateString() + "<b/>";
                    }
                    DateTime schmStartDate = Convert.ToDateTime(allotdate.Single().LaunchDate.ToString());
                    ViewState["schmStartDate"] = schmStartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    var FundManager = (from fd in principl.T_FUND_MANAGERs
                                       join
                                    cfm in principl.T_CURRENT_FUND_MANAGERs on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                       join
                                       fms in principl.T_FUND_MASTERs on cfm.FUND_ID equals fms.FUND_ID
                                       join
                                       sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                       where
                                       sm.Scheme_Id == Convert.ToDecimal(schemeId) && cfm.LATEST_FUNDMAN == true
                                       select new
                                       {
                                           fd.FUND_MANAGER_NAME
                                       }).Distinct().ToArray();//fd.FUND_MANAGER_NAME;

                    string FundmanegerText = string.Empty;

                    if (FundManager.Count() > 0)
                    {
                        foreach (var fn in FundManager.AsEnumerable())
                        {
                            FundmanegerText += fn.FUND_MANAGER_NAME.ToString() + " , ";
                        }
                        FundmanegerText = FundmanegerText.TrimEnd(' ', ',');
                        ViewState["FundmanegerText"] = FundmanegerText;
                    }

                    var FundName = from fms in principl.T_FUND_MASTERs
                                   join
                                   sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                   where
                                   sm.Scheme_Id == Convert.ToDecimal(schemeId)
                                   select new
                                   {
                                       fms.FUND_NAME
                                   };

                    if (FundName != null && FundName.Count() == 1)
                    {
                        ViewState["FundName"] = FundName.Single().FUND_NAME.ToString();
                    }
                }


                List<string> objFundDescList = new List<string>();
                objFundDescList.AddRange(GetSundFundDesc(schemeId));

                if (objFundDescList.Count > 0)
                {
                    ViewState["FundDesc1"] = objFundDescList[0].ToString();
                    ViewState["FundDesc2"] = objFundDescList[1].ToString();
                    ViewState["FundDesc3"] = objFundDescList[2].ToString();
                }

                #endregion

                #region show result
                resultDiv.Visible = true;
                //lblInvestment.Text = "Total Investment amount as on " + txtfromDate.Text + " is<b> Rs " + txtinstall.Text + "</b>";
                ////lblWithdrwAmnt.Text = "Withdrawal amount per " + txtddwperiod.Text + " is<b> Rs " + txtTransferWithdrawal.Text + "</b>";
                ////lblTotalWithdrw.Text = "Total Withdrawals from " + txtfromDate.Text + " to " + txtToDate.Text + " id <b>Rs ";//
                ////lblswpInvsVal.Text = "On " + txtvalason.Text + ", the value of Invstments in your SWP account would be<b> Rs ";

                //if (dtble.Rows.Count > 0)
                //{
                //    if (Convert.ToString(dtble.Rows[0]["TOTAL_AMOUNT_WITHDRAWN"]) != "N/A")
                //        totalWithdrawAmnt = Math.Round(Convert.ToDouble(dtble.Rows[0]["TOTAL_AMOUNT_WITHDRAWN"]), 2);


                //    ViewState["totalWithdrawAmnt"] = totalWithdrawAmnt;
                //    //lblTotalWithdrw.Text += totalWithdrawAmnt == null ? "N/A" : totalWithdrawAmnt.ToString() + "</b>";

                //    if (Convert.ToString(dtble.Rows[0]["PRESENT_VALUE"]) != "N/A")
                //        prsntInvstVal = Math.Round(Convert.ToDouble(dtble.Rows[0]["PRESENT_VALUE"]), 2);
                //    ViewState["prsntInvstVal"] = prsntInvstVal;
                //    //lblswpInvsVal.Text += prsntInvstVal == null ? "N/A" : prsntInvstVal.ToString() + "</b>";

                //    lblAbsoluteReturn.Text = "Absolute return of Investment on " + ddlscheme.SelectedItem.Text + " from " + txtfromDate.Text + " to " + txtToDate.Text + " is<b> ";
                //    double? retrn = ((totalWithdrawAmnt + prsntInvstVal) - Convert.ToDouble(txtinstall.Text)) / Convert.ToDouble(txtinstall.Text) * 100;
                //    lblAbsoluteReturn.Text += retrn == null ? "N/A" : Math.Round(Convert.ToDouble(retrn), 2).ToString() + " % </b>";



                //    if (Convert.ToString(dtble.Rows[0]["YIELD"]) != "N/A")
                //        yieldReturn = Math.Round(Convert.ToDouble(dtble.Rows[0]["YIELD"]), 2);

                //    lblCagrReturn.Text = "XIRR return of Investment on " + ddlscheme.SelectedItem.Text + " from " + txtfromDate.Text + " to " + txtToDate.Text + " is <b>";// +;
                //    lblCagrReturn.Text += yieldReturn == null ? "N/A" : yieldReturn.ToString() + " %</b>";
                //    lblCagrReturn.Visible = true;

                //    //lblRemaingPeriod.Text = "If you continue to withdraw this amount in the future, your money could last you for <b>";
                //    double month = 0;
                //    if (prsntInvstVal > 0 && prsntInvstVal >= Convert.ToDouble(txtTransferWithdrawal.Text))
                //    {
                //        month = Math.Ceiling((double)prsntInvstVal / Convert.ToDouble(txtTransferWithdrawal.Text));
                //        if (month >= 12)
                //        {
                //            int noOfyear = (int)month / 12;
                //            int noOfmonth = (int)month % 12;
                //            //if (noOfmonth == 0)
                //            //    lblRemaingPeriod.Text += noOfyear.ToString() + " Years </b>";
                //            //else
                //            //    lblRemaingPeriod.Text += noOfyear.ToString() + " Years " + noOfmonth.ToString() + " Months </b>";
                //        }
                //        else
                //        {
                //            //lblRemaingPeriod.Text += month.ToString() + " Months </b>";
                //        }

                //    }
                //    //else if (prsntInvstVal > 0 && prsntInvstVal < Convert.ToDouble(txtTransferWithdrawal.Text))
                //    //{
                //    //    lblRemaingPeriod.Text += " 1 Month </b>";
                //    //}
                //    //else if (prsntInvstVal == 0)
                //    //{
                //    //    lblRemaingPeriod.Text += " 0 Day </b>";
                //    //}



                //    // btnBenchMarkReturn.Visible = true;
                //    //GridViewSWPResult.DataSource = datatTble;
                //    //GridViewSWPResult.DataBind();
                //    //GridViewSWPResult.Visible = false;
                //    //btnExcelCalculation.Visible = true;
                //}

                #endregion

                #region chart


                #region add extra column
                if (datatTble.Rows.Count > 0)
                {
                    if (datatTble.Rows.Count > 2)
                    {
                        datatTble.Rows.RemoveAt(datatTble.Rows.Count - 1);
                        DataColumn dc = new DataColumn("Amount", System.Type.GetType("System.Double"));
                        datatTble.Columns.Add(dc);


                        for (int i = 0; i < datatTble.Rows.Count - 1; i++)
                        {

                            if (i == 0)
                            {
                                datatTble.Rows[i]["Amount"] = Convert.ToDouble(datatTble.Rows[i]["INVST_AMOUNT"]);
                            }
                            else
                            {
                                datatTble.Rows[i]["Amount"] = Convert.ToDouble(datatTble.Rows[i - 1]["Amount"]) + (-1) * Convert.ToDouble(datatTble.Rows[i]["INVST_AMOUNT"]);

                            }
                        }
                    }
                }


                #endregion


                DataTable dtChart = new DataTable("dtChart");
                dtChart = datatTble.Copy();
                // dtChart.Rows.RemoveAt(dtChart.Rows.Count - 1);




                #region Remove Column
                for (int col = dtChart.Columns.Count - 1; col >= 0; col--)
                {
                    DataColumn dc = dtChart.Columns[col];
                    if (dc.ColumnName.ToUpper() != "DATE" && dc.ColumnName.ToUpper() != "CUMILATIVE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT")
                    {

                        dtChart.Columns.RemoveAt(col);
                    }
                }

                #endregion

                ViewState["dtchartView"] = dtChart;

                // BindDataTableToChartGeneral(dtChart, "DATE", chrtResult);


                //chrtResult.Visible = true;
                //divshowChart.Visible = true;
                resultDiv.Visible = true;
                #endregion


                #region :Historical Performance
                datatTbleHistorical = CalculateHistPerf(swpEndDate);
                if (datatTbleHistorical.Rows.Count > 1)
                {

                    GridViewSIPResult.DataSource = datatTbleHistorical;
                    GridViewSIPResult.DataBind();
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
            if (lnkTab3.Visible == false) lnkTab3.Visible = true;

        }

        /// <summary>
        /// This Function will Show Result if STP is Selected
        /// </summary>

        public void CalculateReturnSTP()
        {
            #region Initialize
            DateTime stpStartDate, stpEndDate, stpAsonDate, tempDate, investmentDate, allotDate, initialDate;
            int daydiff, stpDate, PrdVal, tempInt;
            TimeSpan datedifference;
            SqlCommand cmdstp = null;
            double dblStpTransferAmnt, dblstpIntialAmnt;
            string strFrequency = string.Empty, schemeIdtrf = string.Empty, schemeIdtrto = string.Empty, strInvestorType = string.Empty;
            DataSet dSet = new DataSet("stpDataSet");
            DataTable dtble = new DataTable("stpDataTable"), datatTble = new DataTable("stpDT");
            double? amountLeft = null, investmntValue = null, cumulativAmountTo = null, returnXIRRSchemeFrom = null, returnXIRRSchemeTo = null, amountLeftTo = null;
            DataTable STPFromTable = new DataTable(); DataTable STPToTable = new DataTable();
            DataTable STPToDetailTable = new DataTable(); DataTable STPFromDetailTable = new DataTable();
            DataTable datatTbleHistorical = new DataTable("datatTbleHistorical"); DataTable datatTbleHistoricalTO = new DataTable("datatTbleHistoricalTO");
            #endregion
            try
            {
                //stpStartDate = Convert.ToDateTime(txtfrdt.Text);
                //stpEndDate = Convert.ToDateTime(txttodt.Text);
                //stpAsonDate = Convert.ToDateTime(txtvalue.Text);


                stpStartDate = new DateTime(Convert.ToInt16(txtfromDate.Text.Split('/')[2]),
                                      Convert.ToInt16(txtfromDate.Text.Split('/')[1]),
                                      Convert.ToInt16(txtfromDate.Text.Split('/')[0]));

                stpEndDate = new DateTime(Convert.ToInt16(txtToDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[0]));

                stpAsonDate = new DateTime(Convert.ToInt16(txtvalason.Text.Split('/')[2]),
                                     Convert.ToInt16(txtvalason.Text.Split('/')[1]),
                                     Convert.ToInt16(txtvalason.Text.Split('/')[0]));


                stpDate = Convert.ToInt32(ddSIPdate.SelectedItem.Value);
                dblStpTransferAmnt = Convert.ToDouble(txtTransferWithdrawal2.Text);
                dblstpIntialAmnt = Convert.ToDouble(txtiniAmount.Text);
                schemeIdtrf = ddlscheme.SelectedItem.Value;
                schemeIdtrto = ddlschtrto.SelectedItem.Value;
                strFrequency = ddPeriod_SIP.SelectedItem.Text;
                strInvestorType = "Individual/Huf";
                initialDate = new DateTime(Convert.ToInt16(txtIniToDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtIniToDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtIniToDate.Text.Split('/')[0]));
                //stpStartDate = stpStartDate.AddDays(7);// add 7 days for intermediate transaction time lag

                conn.ConnectionString = connstr;

                STPFromTable.Columns.Add("Scheme_Name"); STPFromTable.Columns.Add("Total_Amount_Invested");
                STPFromTable.Columns.Add("Total_Amount_Withdrawn"); STPFromTable.Columns.Add("Present_Value");
                STPFromTable.Columns.Add("Yield"); STPFromTable.Columns.Add("ABSOLUTERETURN");
                STPFromTable.Rows.Add();

                //STPToTable.Columns.Add("Scheme_Name"); STPToTable.Columns.Add("Total_Amount_Invested");
                //STPToTable.Columns.Add("Present_Value"); STPToTable.Columns.Add("Yield");
                //STPToTable.Rows.Add();

                #region condition check
                if (ddPeriod_SIP.SelectedItem.Text == "Monthly")
                {
                    PrdVal = 1;
                }
                else if (ddPeriod_SIP.SelectedItem.Text == "Quarterly")
                {
                    PrdVal = 3;
                }
                else if (ddPeriod_SIP.SelectedItem.Text == "Daily")
                {
                    PrdVal = 4;
                }
                else
                {
                    return;
                }

                //--v01-Apr-08---validation for at least 6  transfers by jscript



                if (PrdVal == 1)
                {
                    tempDate = stpEndDate.AddMonths(-1);
                    datedifference = tempDate.Subtract(stpStartDate);
                    daydiff = datedifference.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"STP is allowed for minimum 1 withdrawal\")</script>");
                        return;
                    }
                }
                else if (PrdVal == 3)
                {
                    tempDate = stpEndDate.AddMonths(-3);
                    datedifference = tempDate.Subtract(stpStartDate);
                    daydiff = datedifference.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"STP is allowed for minimum 1 withdrawal\")</script>");
                        return;
                    }
                }

                tempInt = stpStartDate.Day;
                if (stpDate < tempInt)
                {
                    if (stpStartDate.Month != 12)
                        investmentDate = new DateTime(stpStartDate.Year, stpStartDate.Month + 1, stpDate);
                    else
                        investmentDate = new DateTime(stpStartDate.Year + 1, 1, stpDate);
                }
                else if (stpDate == tempInt)
                {
                    investmentDate = stpStartDate;
                }
                else
                {
                    investmentDate = new DateTime(stpStartDate.Year, stpStartDate.Month, stpDate);
                }
                #endregion

                # region calling SP
                cmdstp = new SqlCommand("MFIE_SP_STP_CALCULATER_DSPTEST", conn);
                cmdstp.CommandType = CommandType.StoredProcedure;
                cmdstp.CommandTimeout = 2000;
                cmdstp.Parameters.Add(new SqlParameter("@From_Scheme_Id", schemeIdtrf));
                cmdstp.Parameters.Add(new SqlParameter("@To_Scheme_Id", schemeIdtrto));
                cmdstp.Parameters.Add(new SqlParameter("@Plan_Start_Date", investmentDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));//swpStartDate
                cmdstp.Parameters.Add(new SqlParameter("@Plan_End_Date", stpEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdstp.Parameters.Add(new SqlParameter("@Report_As_On", stpAsonDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdstp.Parameters.Add(new SqlParameter("@STP_Amt", dblStpTransferAmnt));
                cmdstp.Parameters.Add(new SqlParameter("@Frequency", strFrequency));
                cmdstp.Parameters.Add(new SqlParameter("@Dividend_Type", strInvestorType));
                cmdstp.Parameters.Add(new SqlParameter("@Investment_Amount", dblstpIntialAmnt));
                cmdstp.Parameters.Add(new SqlParameter("@Investment_Date", initialDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

                SqlDataAdapter da = new SqlDataAdapter(cmdstp);
                da.Fill(dSet);
                #endregion

                if (dSet.Tables.Count > 0)
                {
                    dtble = dSet.Tables[0];

                    ViewState["stpDataTable"] = dtble.Copy();

                    #region Remove Dividend row

                    if (dtble.Rows.Count > 0)
                    {
                        if (dtble.Rows.Count > 2)
                        {
                            for (int i = dtble.Rows.Count - 4; i >= 0; i--)
                            {
                                if (dtble.Rows[i]["INVST_AMOUNT"] == DBNull.Value)
                                {
                                    dtble.Rows.RemoveAt(i);
                                }
                            }

                        }
                    }
                    #endregion



                }

                #region Launch Date and FundManager Data
                //From Scheme
                using (var principl = new PrincipalCalcDataContext())
                {
                    var allotdate = (from ind in principl.T_SCHEMES_MASTERs
                                     where ind.Scheme_Id == Convert.ToDecimal(schemeIdtrf)
                                     select new
                                     {
                                         LaunchDate = Convert.ToDateTime(ind.Launch_Date)
                                     }).Single().LaunchDate;
                    //DateTime dDate = Convert.ToDateTime(allotdate);
                    ViewState["schmStartDate"] = Convert.ToDateTime(allotdate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var FundManager = (from fd in principl.T_FUND_MANAGERs
                                       join
                                       cfm in principl.T_CURRENT_FUND_MANAGERs on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                       join
                                       fms in principl.T_FUND_MASTERs on cfm.FUND_ID equals fms.FUND_ID
                                       join
                                       sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                       where
                                       sm.Scheme_Id == Convert.ToDecimal(schemeIdtrf) && cfm.LATEST_FUNDMAN == true
                                       select new
                                       {
                                           fd.FUND_MANAGER_NAME
                                       }).Distinct().ToArray();

                    string FundmanegerText = string.Empty;

                    if (FundManager.Count() > 0)
                    {
                        foreach (var fn in FundManager.AsEnumerable())
                        {
                            FundmanegerText += fn.FUND_MANAGER_NAME.ToString() + " , ";
                        }
                        FundmanegerText = FundmanegerText.TrimEnd(' ', ',');
                        ViewState["FundmanegerText"] = FundmanegerText;
                    }

                    var FundName = from fms in principl.T_FUND_MASTERs
                                   join
                                   sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                   where
                                   sm.Scheme_Id == Convert.ToDecimal(schemeIdtrf)
                                   select new
                                   {
                                       fms.FUND_NAME
                                   };

                    if (FundName != null && FundName.Count() == 1)
                    {
                        ViewState["FundName"] = FundName.Single().FUND_NAME.ToString();
                    }
                }

                //TO SCHEME
                using (var principl = new PrincipalCalcDataContext())
                {
                    var allotdateSTPTO = (from ind in principl.T_SCHEMES_MASTERs
                                          where ind.Scheme_Id == Convert.ToDecimal(schemeIdtrto)
                                          select new
                                          {
                                              LaunchDate = Convert.ToDateTime(ind.Launch_Date)
                                          }).Single().LaunchDate;

                    //DateTime dDate = Convert.ToDateTime(allotdateSTPTO);
                    ViewState["ToSchStartDate"] = Convert.ToDateTime(allotdateSTPTO).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    var FundManager = (from fd in principl.T_FUND_MANAGERs
                                       join
                                       cfm in principl.T_CURRENT_FUND_MANAGERs on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                       join
                                       fms in principl.T_FUND_MASTERs on cfm.FUND_ID equals fms.FUND_ID
                                       join
                                       sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                       where
                                       sm.Scheme_Id == Convert.ToDecimal(schemeIdtrto) && cfm.LATEST_FUNDMAN == true
                                       select new
                                       {
                                           fd.FUND_MANAGER_NAME
                                       }).Distinct().ToArray();//fd.FUND_MANAGER_NAME;

                    string FundmanegerText = string.Empty;

                    if (FundManager.Count() > 0)
                    {
                        foreach (var fn in FundManager.AsEnumerable())
                        {
                            FundmanegerText += fn.FUND_MANAGER_NAME.ToString() + " , ";
                        }
                        FundmanegerText = FundmanegerText.TrimEnd(' ', ',');
                        ViewState["ToSchFundmanager"] = FundmanegerText;
                    }

                    var FundName = from fms in principl.T_FUND_MASTERs
                                   join
                                   sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                   where
                                   sm.Scheme_Id == Convert.ToDecimal(schemeIdtrto)
                                   select new
                                   {
                                       fms.FUND_NAME
                                   };

                    if (FundName != null && FundName.Count() == 1)
                    {
                        ViewState["ToSchFundName"] = FundName.Single().FUND_NAME.ToString();
                    }


                    var _schemeToIndex = from tim in principl.T_INDEX_MASTERs
                                         join k in principl.T_SCHEMES_INDEXes
                                         on tim.INDEX_ID equals k.INDEX_ID
                                         where k.SCHEME_ID == Convert.ToDecimal(ddlschtrto.SelectedItem.Value)
                                         select tim.INDEX_ID;

                    if (_schemeToIndex != null && _schemeToIndex.Count() == 1)
                    {
                        ViewState["ToSchIndex"] = _schemeToIndex.Single().ToString();
                    }


                }

                List<string> objFundDescList = new List<string>();
                List<string> objFundDescList1 = new List<string>();



                objFundDescList.AddRange(GetSundFundDesc(schemeIdtrf));
                objFundDescList1.AddRange(GetSundFundDesc(schemeIdtrto));


                if (objFundDescList.Count > 0)
                {
                    ViewState["FundDesc1"] = objFundDescList[0].ToString();
                    ViewState["FundDesc2"] = objFundDescList[1].ToString();
                    ViewState["FundDesc3"] = objFundDescList[2].ToString();
                }




                if (objFundDescList1.Count > 0)
                {
                    ViewState["FundDesc11"] = objFundDescList1[0].ToString();
                    ViewState["FundDesc21"] = objFundDescList1[1].ToString();
                    ViewState["FundDesc31"] = objFundDescList1[2].ToString();
                }

                #endregion

                if (dtble != null && dtble.Rows.Count > 2)
                {
                    if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 3]["CUMILATIVE_AMOUNT_FROM"]) != "N/A")
                        amountLeft = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 3]["CUMILATIVE_AMOUNT_FROM"]), 2);
                    ViewState["amountLeft"] = amountLeft;

                    if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 3]["CUMILATIVE_AMOUNT_TO"]) != "N/A")
                        amountLeftTo = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 3]["CUMILATIVE_AMOUNT_TO"]), 2);
                    ViewState["amountLeftTo"] = amountLeftTo;

                    if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 2]["INVST_AMOUNT"]) != "N/A")
                        returnXIRRSchemeFrom = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 2]["INVST_AMOUNT"]), 2);

                    if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 1]["INVST_AMOUNT"]) != "N/A")
                        returnXIRRSchemeTo = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 1]["INVST_AMOUNT"]), 2);

                }

                // Commented By Somabrata 20.04.2017
                //var investval = dtble.AsEnumerable().Where(x => x.Field<double?>("INVST_AMOUNT") == dblStpTransferAmnt).Select(x => x.Field<double?>("INVST_AMOUNT")).Sum();

                var investval = dtble.AsEnumerable().Where(x => x.Field<double?>("INVST_AMOUNT") > 0 && x.Field<object>("From_Scheme_ID").ToString() != "0").Select(x => x.Field<double?>("INVST_AMOUNT")).Sum();

                investmntValue = Math.Round(Convert.ToDouble(investval), 2);
                ViewState["investmntValue"] = investmntValue;

                if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 3]["FINAL_INVST_AMOUNT_TO"]) != "N/A")
                {
                    if (Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 3]["FINAL_INVST_AMOUNT_TO"]) != 0)
                        cumulativAmountTo = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 3]["FINAL_INVST_AMOUNT_TO"]), 2);
                    else
                        cumulativAmountTo = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 4]["FINAL_INVST_AMOUNT_TO"]), 2);
                }

                ViewState["cumulativAmountTo"] = cumulativAmountTo;
                double? invstAmountLeft = dblstpIntialAmnt - investmntValue;
                if (invstAmountLeft < 0)
                    invstAmountLeft = 0;


                ViewState["invstAmountLeft"] = invstAmountLeft;

                var YieldFrom = dtble.AsEnumerable().Where(x => x.Field<string>("FROM_SCHEME_NAME") == "YIELD FROM:").Select(x => x.Field<double?>("INVST_AMOUNT")).Single();
                var YieldFromTo = dtble.AsEnumerable().Where(x => x.Field<string>("FROM_SCHEME_NAME") == "YIELD TO:").Select(x => x.Field<double?>("INVST_AMOUNT")).Single();

                ViewState["YieldFrom"] = YieldFrom;
                ViewState["YieldFromTo"] = YieldFromTo;
                //Data Part
                STPFromTable.Rows[0]["Scheme_Name"] = ddlscheme.SelectedItem.Text.ToString();
                STPFromTable.Rows[0]["Total_Amount_Invested"] = Convert.ToDouble(dblstpIntialAmnt);
                STPFromTable.Rows[0]["Total_Amount_Withdrawn"] = Convert.ToDouble(investmntValue);
                STPFromTable.Rows[0]["Present_Value"] = Convert.ToDouble(amountLeft);
                STPFromTable.Rows[0]["Yield"] = Math.Round(Convert.ToDouble(YieldFrom), 2);
                STPFromTable.Rows[0]["ABSOLUTERETURN"] = Math.Round(Convert.ToDouble((amountLeft + investmntValue - dblstpIntialAmnt) / dblstpIntialAmnt) * 100, 2);

                STPFromTable.Rows.Add();
                STPFromTable.Rows[1]["Scheme_Name"] = ddlschtrto.SelectedItem.Text.ToString();
                STPFromTable.Rows[1]["Total_Amount_Invested"] = Convert.ToDouble(investmntValue);
                //STPFromTable.Rows[1]["Total_Amount_Withdrawn"] ="N.A.";
                STPFromTable.Rows[1]["Present_Value"] = Convert.ToDouble(amountLeftTo);
                STPFromTable.Rows[1]["Yield"] = Math.Round(Convert.ToDouble(YieldFromTo), 2);
                STPFromTable.Rows[1]["ABSOLUTERETURN"] = Math.Round(Convert.ToDouble((amountLeftTo - investmntValue) / investmntValue) * 100, 2);


                gvSWPSummaryTable.DataSource = STPFromTable;
                gvSWPSummaryTable.DataBind();





                DataTable tempstpDataTable = (System.Data.DataTable)ViewState["stpDataTable"];
                if (tempstpDataTable.Rows.Count > 2)
                {
                    tempstpDataTable.Rows.RemoveAt(tempstpDataTable.Rows.Count - 1);
                    tempstpDataTable.Rows.RemoveAt(tempstpDataTable.Rows.Count - 1);
                }

                DataTable stpfromDataTable, stptoDataTable;
                stpfromDataTable = tempstpDataTable.Copy(); stptoDataTable = tempstpDataTable.Copy();


                #region REMOVE COLUMN
                //10/01/2013
                int LastColumnofFromSchemeSTP = 11;

                for (int i = stpfromDataTable.Columns.Count - 1; i >= LastColumnofFromSchemeSTP; i--)
                {
                    DataColumn dc = stpfromDataTable.Columns[i];
                    stpfromDataTable.Columns.Remove(dc);
                }


                #region Remove Blank row

                if (stpfromDataTable.Rows.Count > 0)
                {
                    if (stpfromDataTable.Rows.Count > 2)
                    {
                        for (int i = stpfromDataTable.Rows.Count - 2; i >= 0; i--)
                        {
                            if (stpfromDataTable.Rows[i]["INVST_AMOUNT"] == DBNull.Value && stpfromDataTable.Rows[i]["DIVIDEND_BONUS_FROM"] == DBNull.Value)
                            {
                                stpfromDataTable.Rows.RemoveAt(i);
                            }
                        }

                    }
                }
                #endregion

                if (stpfromDataTable.Columns.Contains("ID")) stpfromDataTable.Columns.Remove("ID");
                if (stpfromDataTable.Columns.Contains("FROM_SCHEME_ID")) stpfromDataTable.Columns.Remove("FROM_SCHEME_ID");
                if (stpfromDataTable.Columns.Contains("INVST_AMOUNT")) stpfromDataTable.Columns.Remove("INVST_AMOUNT");


                for (int i = 0; i < LastColumnofFromSchemeSTP; i++)
                {
                    stptoDataTable.Columns.RemoveAt(0);
                }



                #region Remove Blank row

                if (stptoDataTable.Rows.Count > 0)
                {
                    if (stptoDataTable.Rows.Count > 2)
                    {
                        for (int i = stptoDataTable.Rows.Count - 2; i >= 0; i--)
                        {
                            if ((stptoDataTable.Rows[i]["AMOUNT"] == DBNull.Value || stptoDataTable.Rows[i]["AMOUNT"].ToString() == "0") && stptoDataTable.Rows[i]["DIVIDEND_BONUS_TO"] == DBNull.Value)
                            {
                                stptoDataTable.Rows.RemoveAt(i);
                            }
                        }

                    }
                }
                #endregion

                if (stptoDataTable.Columns.Contains("TO_SCHEME_ID")) stptoDataTable.Columns.Remove("TO_SCHEME_ID");
                if (stptoDataTable.Columns.Contains("AMOUNT")) stptoDataTable.Columns.Remove("AMOUNT");

                //if (stptoDataTable.Rows.Count > 1)
                //    stptoDataTable.Rows.RemoveAt(0);

                #endregion


                stpFromGridview.DataSource = stpfromDataTable;
                stpFromGridview.DataBind();

                stpToGridview.DataSource = stptoDataTable;
                stpToGridview.DataBind();
                //Data Part

                resultDiv.Visible = true;


                #region Corpus
                fetchCorpus(schemeIdtrf);

                DataTable dtTemp = FindCorpusVal(Convert.ToInt32(schemeIdtrto));
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["Fund_Size"] != DBNull.Value)
                    {
                        ViewState["CorpusFund2"] = Convert.ToDouble(dtTemp.Rows[0]["Fund_Size"]);
                    }
                }

                //fetchCorpus(schemeIdtrf);
                #endregion

                #region chart


                #region add extra column
                DataTable dtSTPFinal = (System.Data.DataTable)ViewState["stpDataTable"];
                DataTable tempstpChartFrom = dtSTPFinal.Copy();
                DataTable tempstpChartTO = dtSTPFinal.Copy();
                if (tempstpChartFrom.Rows.Count > 0)
                {
                    tempstpChartFrom.Rows.RemoveAt(tempstpChartFrom.Rows.Count - 1);
                    tempstpChartFrom.Rows.RemoveAt(tempstpChartFrom.Rows.Count - 1);
                    // tempstpChartFrom.Rows.RemoveAt(tempstpChartFrom.Rows.Count - 1);
                    for (int i = tempstpChartFrom.Columns.Count - 1; i >= LastColumnofFromSchemeSTP; i--)
                    {
                        DataColumn dcol = tempstpChartFrom.Columns[i];
                        tempstpChartFrom.Columns.Remove(dcol);
                    }


                    if (tempstpChartFrom.Rows.Count > 2)
                    {
                        for (int i = tempstpChartFrom.Rows.Count - 1; i >= 0; i--)
                        {
                            if (tempstpChartFrom.Rows[i]["INVST_AMOUNT"] == DBNull.Value)
                            {
                                tempstpChartFrom.Rows.RemoveAt(i);
                            }
                        }

                    }


                    DataColumn dc = new DataColumn("Amount", System.Type.GetType("System.Double"));
                    tempstpChartFrom.Columns.Add(dc);

                    for (int i = 0; i < tempstpChartFrom.Rows.Count; i++)
                    {

                        if (i == 0)
                        {
                            tempstpChartFrom.Rows[i]["Amount"] = (-1) * Convert.ToDouble(tempstpChartFrom.Rows[i]["INVST_AMOUNT"]);
                        }
                        else
                        {
                            tempstpChartFrom.Rows[i]["Amount"] = Convert.ToDouble(tempstpChartFrom.Rows[i - 1]["Amount"]) - Convert.ToDouble(tempstpChartFrom.Rows[i]["INVST_AMOUNT"]);

                        }
                    }
                    tempstpChartFrom.Rows.RemoveAt(0);
                }

                if (tempstpChartTO.Rows.Count > 0)
                {
                    for (int i = 0; i < LastColumnofFromSchemeSTP; i++)
                    {
                        tempstpChartTO.Columns.RemoveAt(0);
                    }
                    tempstpChartTO.Rows.RemoveAt(0);


                    if (tempstpChartTO.Rows.Count > 2)
                    {
                        for (int i = tempstpChartTO.Rows.Count - 2; i >= 0; i--)
                        {
                            if (tempstpChartTO.Rows[i]["AMOUNT"] == DBNull.Value)
                            {
                                tempstpChartTO.Rows.RemoveAt(i);
                            }
                        }

                    }


                    DataColumn dc = new DataColumn("Amount_TO", System.Type.GetType("System.Double"));
                    tempstpChartTO.Columns.Add(dc);

                    for (int i = 0; i < tempstpChartTO.Rows.Count - 1; i++)
                    {

                        if (i == 0)
                        {
                            tempstpChartTO.Rows[i]["Amount_TO"] = (-1) * Convert.ToDouble(tempstpChartTO.Rows[i]["AMOUNT"]);
                        }
                        else
                        {
                            tempstpChartTO.Rows[i]["Amount_TO"] = Convert.ToDouble(tempstpChartTO.Rows[i - 1]["Amount_TO"]) + ((-1) * Convert.ToDouble(tempstpChartTO.Rows[i]["AMOUNT"]));

                        }
                    }
                }
                #endregion


                DataTable dtChart = new DataTable("dtChart");
                dtChart = tempstpChartFrom.Copy();




                for (int col = dtChart.Columns.Count - 1; col >= 0; col--)
                {
                    DataColumn dc = dtChart.Columns[col];
                    if (dc.ColumnName.ToUpper() != "FROM_DATE" && dc.ColumnName.ToUpper() != "CUMILATIVE_AMOUNT_FROM" && dc.ColumnName.ToUpper() != "AMOUNT")
                    {

                        dtChart.Columns.RemoveAt(col);
                    }
                }


                ViewState["dtchartView"] = dtChart;

                // BindDataTableToChartGeneral(dtChart, "FROM_DATE", chrtResult);

                //Chart Scheme TO DATA
                DataTable dtChartTO = new DataTable("dtChartTO");
                dtChartTO = tempstpChartTO.Copy();



                for (int col = dtChartTO.Columns.Count - 1; col >= 0; col--)
                {
                    DataColumn dc = dtChartTO.Columns[col];
                    if (dc.ColumnName.ToUpper() != "TO_DATE" && dc.ColumnName.ToUpper() != "CUMILATIVE_AMOUNT_TO" && dc.ColumnName.ToUpper() != "AMOUNT_TO")
                    {

                        dtChartTO.Columns.RemoveAt(col);
                    }
                }

                ViewState["dtchartViewSTPto"] = dtChartTO;



                /////////////// 09-01-2013
                #region STACK Bar Chart
                DataTable tempstpChart = dtSTPFinal.Copy();



                for (int col = tempstpChart.Columns.Count - 1; col >= 0; col--)
                {
                    DataColumn dc = tempstpChart.Columns[col];
                    if (dc.ColumnName.ToUpper() != "FROM_DATE" && dc.ColumnName.ToUpper() != "CUMILATIVE_AMOUNT_TO" && dc.ColumnName.ToUpper() != "CUMILATIVE_AMOUNT_FROM")
                    {
                        tempstpChart.Columns.RemoveAt(col);
                    }
                }

                ViewState["dtchartViewSTP"] = tempstpChart.Copy();
                #endregion

                resultDiv.Visible = true;
                #endregion

                #region :Historical Performance
                datatTbleHistorical = CalculateHistPerf(stpEndDate);
                string schmeToIndexId = ViewState["ToSchIndex"].ToString();
                datatTbleHistoricalTO = CalculateHistPerf(stpEndDate, ddlschtrto.SelectedItem.Value, schmeToIndexId);

                if (datatTbleHistorical.Rows.Count == 2)
                    datatTbleHistorical.Rows[1][0] = "Benchmark : " + datatTbleHistorical.Rows[1][0].ToString();

                if (datatTbleHistoricalTO.Rows.Count == 2)
                    datatTbleHistoricalTO.Rows[1][0] = "Benchmark : " + datatTbleHistoricalTO.Rows[1][0].ToString();

                //datatTbleHistorical.Rows.Add(datatTbleHistoricalTO.Rows);
                foreach (DataRow temp in datatTbleHistoricalTO.Rows)
                {
                    datatTbleHistorical.ImportRow(temp);
                }

                if (datatTbleHistorical.Rows.Count > 1)
                {
                    GridViewSIPResult.DataSource = datatTbleHistorical;
                    GridViewSIPResult.DataBind();
                }

                //if (datatTbleHistoricalTO.Rows.Count > 0)
                //{

                //    GridViewSTPTOResult.DataSource = datatTbleHistoricalTO;
                //    GridViewSTPTOResult.DataBind();
                //}
                #endregion


                if (!LSDisc.Visible)
                    LSDisc.Visible = true;
            }
            catch (Exception ex)
            {
                string a = ex.Message.ToString();
                //throw ex;
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// This Fuction will populate Chart
        /// </summary>
        /// 
        public void CallChartFromView2()
        {

            if (ddlMode.SelectedItem.Text.ToUpper() == "SWP")
            {
                return;
            }


            if (ddlMode.SelectedItem.Text.ToUpper() == "STP")
            {

                if (ViewState["dtchartViewSTP"] != null)
                {
                    DataTable dtChrt = ViewState["dtchartViewSTP"] as DataTable;
                    chrtResultSTPTO.Visible = false;
                    if (dtChrt.Rows.Count > 1)
                        BindDataTableToBarChart(dtChrt, "FROM_DATE", chrtResult, "CUMILATIVE_AMOUNT_FROM");
                }


            }
            else
            {
                chrtResultSTPTO.Visible = false;

                if (ViewState["dtchartView"] != null)
                {
                    DataTable dtChrt = ViewState["dtchartView"] as DataTable;
                    if (dtChrt.Rows.Count > 1)
                        CallChart(dtChrt, chrtResult);
                }
            }
        }


        public void CallChartFromView()
        {

            if (ddlMode.SelectedItem.Text.ToUpper() == "SWP")
            {
                return;
            }

            if (ViewState["dtchartView"] != null)
            {
                DataTable dtChrt = ViewState["dtchartView"] as DataTable;
                if (dtChrt.Rows.Count > 1)
                    CallChart(dtChrt, chrtResult);
            }
            if (ddlMode.SelectedItem.Text.ToUpper() == "STP")
            {
                if (ViewState["dtchartViewSTPto"] != null)
                {
                    DataTable dtChrt = ViewState["dtchartViewSTPto"] as DataTable;
                    chrtResultSTPTO.Visible = false;
                    if (dtChrt.Rows.Count > 1)
                        CallChart(dtChrt, chrtResultSTPTO);
                }
            }
            else
            {
                chrtResultSTPTO.Visible = false;
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

            };
            // };


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



        /// <summary>
        /// This Function will Show Result for Lumpsum .
        /// </summary>

        private void PerformanceReturn()
        {
            #region initialize variable
            double? daysDiffValAbs = null, daysDiffVal = null, daysDiffValAbsIndex = null, daysDiffValINdex = null, daysDiffValINdex_Additional = null;
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
            string strSchemeNature = "";



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



            #region Somabrata

            if (IsSchemeDividend && lmprdbPayout.Checked)
                settingSet = 32; // Setting Set Sundaram Raw INvest

            #endregion



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
                strSchemeNature = strNature.ToUpper();
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
                bool IsEquityPlusSchemes; // add by syed
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

                    IsEquityPlusSchemes = Convert.ToString(ViewState["FundName"]) == "Sundaram Equity Plus" ? true : false; //add by syed


                }

                List<string> objFundDescList = new List<string>();
                objFundDescList.AddRange(GetSundFundDesc(schmeId));


                if (objFundDescList.Count > 0)
                {
                    ViewState["FundDesc1"] = objFundDescList[0].ToString();
                    ViewState["FundDesc2"] = objFundDescList[1].ToString();
                    ViewState["FundDesc3"] = objFundDescList[2].ToString();
                }

                #endregion


                # region calling sp

                ////commented by syed as it is no longer used
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
                ////end commented by syed

                //SqlDataAdapter da = new SqlDataAdapter(cmd); // add by syed

                cmdIndx = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
                cmdIndx.CommandType = CommandType.StoredProcedure;
                cmdIndx.CommandTimeout = 2000;
                //commented by syed
                //cmdIndx.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
                // end commented by syed

                if (IsEquityPlusSchemes == false)
                {
                    if (strSchemeNature.ToUpper() == "EQUITY" && indexId == "35")
                        cmdIndx.Parameters.Add(new SqlParameter("@IndexIDs", indexId + ",142"));
                    else if (strSchemeNature.ToUpper() == "EQUITY" && indexId != "35")
                        cmdIndx.Parameters.Add(new SqlParameter("@IndexIDs", indexId + ",35"));
                    else
                        cmdIndx.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
                }
                else
                    cmdIndx.Parameters.Add(new SqlParameter("@IndexIDs", "90,35"));

                //Commented by Arindam Jash on 01/28/2014
                //add by syed
                //if (!IsEquityPlusSchemes)
                //    cmdIndx.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
                //else
                //    cmdIndx.Parameters.Add(new SqlParameter("@IndexIDs", "90,12"));
                //end add by syed
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
                cmdScheme.Parameters.Add(new SqlParameter("@SettingSetID", settingSet)); // change somabrata
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
                //commenetd by syed
                //cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
                //end commented by syed


                //Add by syed
                if (IsEquityPlusSchemes == false)
                {
                    if (strSchemeNature.ToUpper() == "EQUITY" && indexId == "35")
                        cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", indexId + ",142"));
                    else if (strSchemeNature.ToUpper() == "EQUITY" && indexId != "35")
                        cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", indexId + ",35"));
                    else
                        cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
                }
                else
                    cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", "90,35"));
                //end by syed
                cmdIndex.Parameters.Add(new SqlParameter("@SettingSetID", settingSet)); // change somabrata
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

                //if (Convert.ToString(dtScheme.Rows[0]["Since Inception"]) != "N/A")
                //       daysDiffVal = Convert.ToDouble(dtScheme.Rows[0]["Since Inception"]);




                //  lblReturn.Text = returnText + "Return of " + ddlscheme.SelectedItem.Text + " from  " + txtLumpfromDate.Text + " to " + txtLumpToDate.Text + " is <b>";
                //   lblReturn.Text += daysDiffVal == null ? "N/A" : daysDiffVal.ToString() + " % </b>";
                // lblReturn.Text = "<Strong>" + lblReturn.Text + "</Strong>";


                if (Convert.ToString(dttblIndxReturn.Rows[0][daydiffCol]) != "N/A")

                    //commened by syed
                    //daysDiffValINdex = Convert.ToDouble(dttblIndxReturn.Rows[0][daydiffCol]);
                    //end commented by syed
                    // add by syed
                    if (!IsEquityPlusSchemes)
                    {
                        int Key = -1;
                        //Added by Arindam Jash on 01/28/2014
                        for (int rownum = 0; rownum < dttblIndxReturn.Rows.Count; rownum++)
                        {
                            if (strSchemeNature.ToUpper() == "EQUITY" && indexId == "35")
                            {
                                if (dttblIndxReturn.Rows[rownum]["Index_Name"].ToString().Trim().ToUpper() == "S&P BSE SENSEX TRI")
                                {
                                    dttblIndxReturn.Rows[rownum]["Index_Name"] = "S&P BSE Sensex TRI (Additional Benchmark)";
                                    Key = rownum;
                                }
                                else
                                {
                                    if (Convert.ToString(dttblIndxReturn.Rows[rownum][daydiffCol]) != "N/A")
                                        daysDiffValINdex = Convert.ToDouble(dttblIndxReturn.Rows[rownum][daydiffCol]);
                                }
                            }
                            else if (strSchemeNature.ToUpper() == "EQUITY" && indexId != "35")
                            {
                                if (dttblIndxReturn.Rows[rownum]["Index_Name"].ToString().Trim().ToUpper() == "NIFTY 50 TRI")
                                {
                                    dttblIndxReturn.Rows[rownum]["Index_Name"] = "Nifty 50 TRI (Additional Benchmark)";
                                    Key = rownum;
                                }
                                else
                                {
                                    if (Convert.ToString(dttblIndxReturn.Rows[rownum][daydiffCol]) != "N/A")
                                        daysDiffValINdex = Convert.ToDouble(dttblIndxReturn.Rows[rownum][daydiffCol]);
                                }
                            }
                            else
                            {
                                if (Convert.ToString(dttblIndxReturn.Rows[rownum][daydiffCol]) != "N/A")
                                    daysDiffValINdex = Convert.ToDouble(dttblIndxReturn.Rows[rownum][daydiffCol]);
                            }
                        }
                        if (Key != -1)
                        {
                            daysDiffValINdex_Additional = Convert.ToDouble(dttblIndxReturn.Rows[Key][daydiffCol]);
                        }

                    }
                    else
                        daysDiffValINdex = Math.Round((Convert.ToDouble(dttblIndxReturn.Rows[0][daydiffCol]) * 65 / 100) +
                        (Convert.ToDouble(dttblIndxReturn.Rows[1][daydiffCol]) * 35 / 100), 2);
                //end add by syed
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




                //commented by syed
                //tempTable.Rows.Add(dtIndex.Rows[0].ItemArray);
                //end commented by syed



                // add  by syed
                if (!IsEquityPlusSchemes)
                {
                    int Key = -1;
                    //Added by Arindam Jash on 01/28/2014
                    for (int rownum = 0; rownum < dtIndex.Rows.Count; rownum++)
                    {
                        if (strSchemeNature.ToUpper() == "EQUITY" && indexId == "35")
                        {
                            if (dtIndex.Rows[rownum][0].ToString().Trim().ToUpper() == "S&P BSE SENSEX TRI")
                            {
                                dtIndex.Rows[rownum][0] = "S&P BSE Sensex TRI(Additional Benchmark)";
                                Key = rownum;
                            }
                            else
                            {
                                tempTable.Rows.Add(dtIndex.Rows[rownum].ItemArray);
                            }
                        }
                        else if (strSchemeNature.ToUpper() == "EQUITY" && indexId != "35")
                        {
                            if (dtIndex.Rows[rownum][0].ToString().Trim().ToUpper() == "NIFTY 50 TRI")
                            {
                                dtIndex.Rows[rownum][0] = "Nifty 50 TRI(Additional Benchmark)";
                                Key = rownum;
                            }
                            else
                            {
                                tempTable.Rows.Add(dtIndex.Rows[rownum].ItemArray);
                            }
                        }
                        else
                            tempTable.Rows.Add(dtIndex.Rows[rownum].ItemArray);
                    }
                    if (Key != -1)
                    {
                        tempTable.Rows.Add(dtIndex.Rows[Key].ItemArray);
                    }
                }
                else
                {
                    DataRow dr4EqPluss = tempTable.NewRow();
                    dr4EqPluss[0] = "65% of Nifty & 35% of Price of Gold";

                    dr4EqPluss[1] = Math.Round((Convert.ToDouble(dtIndex.Rows[0]["1 Year"]) * 65 / 100) +
                        (Convert.ToDouble(dtIndex.Rows[1]["1 Year"]) * 35 / 100), 2);

                    dr4EqPluss[2] = Math.Round((Convert.ToDouble(dtIndex.Rows[0]["3 Year"]) * 65 / 100) +
                        (Convert.ToDouble(dtIndex.Rows[1]["3 Year"]) * 35 / 100), 2);

                    dr4EqPluss[3] = Math.Round((Convert.ToDouble(dtIndex.Rows[0]["5 Year"]) * 65 / 100) +
                        (Convert.ToDouble(dtIndex.Rows[1]["5 Year"]) * 35 / 100), 2);
                    //daydiffCol
                    dr4EqPluss[4] = Math.Round((Convert.ToDouble(dtIndex.Rows[0][4]) * 65 / 100) +
                        (Convert.ToDouble(dtIndex.Rows[1][4]) * 35 / 100), 2);
                    tempTable.Rows.Add(dr4EqPluss);
                }
                //end add by syed
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

                //Commented by Arindam Jash on 01/28/2014
                //if (tempTable != null && tempTable.Rows.Count == 2)
                //{
                //    GridViewResultLS.DataSource = tempTable;
                //    GridViewResultLS.DataBind();
                //    //lblNote.Visible = true;
                //}

                //Added by Arindam Jash on 01/28/2014
                if (tempTable != null && tempTable.Rows.Count >= 2)
                {
                    GridViewResultLS.DataSource = tempTable;
                    GridViewResultLS.DataBind();
                    //lblNote.Visible = true;
                }


                Double CompundReturnDayVal = 0, CompundReturnDayValIndex = 0, CompundReturnDayValIndex_Additional = 0;

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

                if (daysDiffValINdex_Additional != null)
                {
                    if (daydiff > 365)
                        CompundReturnDayValIndex_Additional = amountLs * Math.Pow(1 + (double)daysDiffValINdex_Additional / 100, Math.Round((double)daydiff / 365, 2));
                    else
                        CompundReturnDayValIndex_Additional = amountLs + amountLs * (double)daysDiffValINdex_Additional / 100;
                    CompundReturnDayValIndex_Additional = Math.Round(CompundReturnDayValIndex_Additional, 2);
                }






                //if (CompundReturnDayVal != 0)
                //{
                //    lblreturnAmount.Text = "By " + txtLumpToDate.Text + " the value of your investment would be <b> Rs " + CompundReturnDayVal.ToString() + "</b>";
                //    lblreturnAmount.Visible = true;
                //}
                //if (CompundReturnDayValIndex != 0)
                //{
                //    lblreturnAmountIndex.Text = "On " + txtfromDate.Text + " you had invested Rs " + txtinstall.Text + "  in " + ddlsipbnmark.SelectedItem.Text + ". By " + txtToDate.Text + " the value of this investment would be  <b> Rs " + CompundReturnDayValIndex.ToString() + "</b>";
                //    //trBenchmarkReturn.Visible = false;
                //    //lblreturnAmountIndex.Visible = false;
                //}

                #endregion


                #region LumpsumTable
                DataTable dtLsTable = new DataTable();
                dtLsTable.Columns.Add("Scheme_Name");
                dtLsTable.Columns.Add("InvestedAmount");
                dtLsTable.Columns.Add("InvestedValue");
                dtLsTable.Columns.Add("Profit");
                dtLsTable.Columns.Add("Return");
                dtLsTable.Columns.Add("DividendAmount");

                dtLsTable.Rows.Clear();




                #region Dividend Payout Calculation

                if (IsSchemeDividend && lmprdbPayout.Checked)
                {
                    var DivAmount = string.Empty;
                    var initialInvestment = Convert.ToDouble(txtinstallLs.Text);
                    double CompundReturnIncludedDividend = default(double);
                    double? DividendValue = default(double?);
                    var result = iFrames.BLL.AllMethods.DividendPayoutCalcSundaram(Convert.ToDecimal(schmeId), initialInvestment, _fromDate, _toDate, true, out DividendValue);
                    if (DividendValue.HasValue)
                    {
                        DivAmount = string.Format("{0:#,0.######}", DividendValue);
                        // If Payout,the div amount will be added to CompundReturnDayVal SUM
                        CompundReturnIncludedDividend = CompundReturnDayVal + DividendValue.Value;
                    }
                    else { DivAmount = "--"; CompundReturnIncludedDividend = CompundReturnDayVal; }


                    //var CalcReturn = CalculateReturn(Convert.ToDouble(txtinstallLs.Text), CompundReturnIncludedDividend, daydiff);

                    List<XIRRData> ItemList = new List<XIRRData>();
                    // Initial Investment
                    ItemList.Add(new XIRRData() { Amount = -(Convert.ToDouble(txtinstallLs.Text)), Date = Convert.ToDateTime(_fromDate) });
                    // Dividend Payout
                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (DataRow item in result.Rows)
                        {
                            ItemList.Add(new XIRRData() { Amount = Convert.ToDouble(item["Payout_Amount"]), Date = Convert.ToDateTime(item["Nav_Date"]) });
                        }
                    }
                    // Final Result
                    ItemList.Add(new XIRRData() { Amount = CompundReturnDayVal, Date = Convert.ToDateTime(_toDate) });

                    var CalcReturn = Math.Round(XIRRCalculation.CalcXirr(ItemList, XIRRCalculation.NewthonsMethod) * 100, 2);


                    double profit = CompundReturnIncludedDividend - Convert.ToDouble(txtinstallLs.Text);
                    dtLsTable.Rows.Add(ddlscheme.SelectedItem.Text, txtinstallLs.Text.ToString(), CompundReturnDayVal, profit, CalcReturn, DivAmount);

                    ViewState["CompundReturnDayVal"] = CompundReturnDayVal;
                    ViewState["profit"] = profit;
                    ViewState["daysDiffVal"] = daysDiffVal;

                    GridViewLumpSum.Columns[GridViewLumpSum.Columns.Count - 1].Visible = true;

                    gridDivHistory.DataSource = result;
                    gridDivHistory.DataBind();

                }
                else if (IsSchemeDividend && lmprdbReinvest.Checked)
                {
                    var DivAmount = string.Empty;
                    var initialInvestment = Convert.ToDouble(txtinstallLs.Text);
                    double? DividendValue = default(double?);
                    var result = iFrames.BLL.AllMethods.DividendPayoutCalcSundaram(Convert.ToDecimal(schmeId), initialInvestment, _fromDate, _toDate, false, out DividendValue);

                    double profit = CompundReturnDayVal - Convert.ToDouble(txtinstallLs.Text);
                    dtLsTable.Rows.Add(ddlscheme.SelectedItem.Text, txtinstallLs.Text.ToString(), CompundReturnDayVal, profit, daysDiffVal, "--");

                    ViewState["CompundReturnDayVal"] = CompundReturnDayVal;
                    ViewState["profit"] = profit;
                    ViewState["daysDiffVal"] = daysDiffVal;

                    gridDivHistory.DataSource = result;
                    gridDivHistory.DataBind();

                }
                else
                {
                    double profit = CompundReturnDayVal - Convert.ToDouble(txtinstallLs.Text);
                    dtLsTable.Rows.Add(ddlscheme.SelectedItem.Text, txtinstallLs.Text.ToString(), CompundReturnDayVal, profit, daysDiffVal, "--");

                    ViewState["CompundReturnDayVal"] = CompundReturnDayVal;
                    ViewState["profit"] = profit;
                    ViewState["daysDiffVal"] = daysDiffVal;


                }
                #endregion





                if (CompundReturnDayValIndex != 0)
                {
                    double profitIndex = CompundReturnDayValIndex - Convert.ToDouble(txtinstallLs.Text);
                    //  dtLsTable.Rows.Add(ddlsipbnmark.SelectedItem.Text, txtinstallLs.Text.ToString(), CompundReturnDayValIndex, profitIndex, daysDiffValINdex);
                    if (!IsEquityPlusSchemes)
                        dtLsTable.Rows.Add(ViewState["INDEX_NAME"], txtinstallLs.Text.ToString(), CompundReturnDayValIndex, profitIndex, daysDiffValINdex, "--");
                    else
                        dtLsTable.Rows.Add("65% of Nifty & 35% of Price of Gold", txtinstallLs.Text.ToString(), CompundReturnDayValIndex, profitIndex, daysDiffValINdex, "--");
                }
                if (CompundReturnDayValIndex_Additional != 0)
                {
                    double profitIndex_Additional = CompundReturnDayValIndex_Additional - Convert.ToDouble(txtinstallLs.Text);
                    //  dtLsTable.Rows.Add(ddlsipbnmark.SelectedItem.Text, txtinstallLs.Text.ToString(), CompundReturnDayValIndex, profitIndex, daysDiffValINdex);
                    if (indexId == "35")
                        dtLsTable.Rows.Add("S&P BSE Sensex TRI (Additional Benchmark)", txtinstallLs.Text.ToString(), CompundReturnDayValIndex_Additional, profitIndex_Additional, daysDiffValINdex_Additional, "--");
                    else
                        dtLsTable.Rows.Add("Nifty 50 TRI (Additional Benchmark)", txtinstallLs.Text.ToString(), CompundReturnDayValIndex_Additional, profitIndex_Additional, daysDiffValINdex_Additional, "--");
                }

                // If dividend reinvest then column hides (client requirement)
                ((DataControlField)GridViewLumpSum.Columns
                    .Cast<DataControlField>()
                    .Where(fld => fld.HeaderText.ToLower().Contains("dividend amount"))
                    .SingleOrDefault()).Visible = lmprdbPayout.Checked && IsSchemeDividend;



                GridViewLumpSum.DataSource = dtLsTable;
                GridViewLumpSum.DataBind();



                #endregion




                //LSDisc.Text = "<b><br/>  * For Time Periods > 1 yr,CAGR Returns have been shown. For Time Periods < 1 yr,Absolute Returns have been shown.<br/></b>";

                //if (!resultDivLS.Visible) resultDivLS.Visible = true;
                if (GridViewResultLS.Visible == false) GridViewResultLS.Visible = true;
                if (GridViewLumpSum.Visible == false) GridViewLumpSum.Visible = true;

                if (!LSDisc1.Visible)
                    LSDisc1.Visible = true;
                //Button benhMarkbtn = new Button();
                //benhMarkbtn.Attributes.Add("onclick","ShowBenchMarkResult");
                //resultDiv.Controls.Add(benhMarkbtn);
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

            IsSchemeDividend = (bool)ViewState["IsSchemeDividend"];


            lnkTab3.Visible = false; //added 06 august 2013
            if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
            {
                if (!String.IsNullOrEmpty(txtinstallLs.Text))
                {
                    if (Convert.ToDouble(txtinstallLs.Text) >= 5000)
                    {
                        CalculateReturnLumpSum();
                    }
                    else
                    { return; }
                }
            }
            else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP WITH INITIAL INVESTMENT")
            {
                if (!String.IsNullOrEmpty(txtinstall.Text))
                {
                    if (Convert.ToDouble(txtinstall.Text) >= 500)
                    {
                        if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP WITH INITIAL INVESTMENT")
                        {
                            if (!String.IsNullOrEmpty(txtiniAmount.Text))
                            {
                                if (Convert.ToDouble(txtiniAmount.Text) < 500)
                                {
                                    return;
                                }
                            }
                        }
                        // SetDefaultView();
                        if (!divTab.Visible) divTab.Visible = true;
                        if (!gvFirstTable.Visible) gvFirstTable.Visible = true;
                        //if (resultDivLS.Visible) resultDivLS.Visible = false;
                        if (GridViewResultLS.Visible) GridViewResultLS.Visible = false;
                        resultDiv.Visible = false;
                        CalculateReturn();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP")
            {
                if (!String.IsNullOrEmpty(txtTransferWithdrawal2.Text) && !String.IsNullOrEmpty(txtiniAmount.Text))
                {
                    if (Convert.ToDouble(txtTransferWithdrawal2.Text) >= 500 || Convert.ToDouble(txtiniAmount.Text) >= 3000)//&& rePLACE BY koustav 5-apr-2018
                    {
                        //SetDefaultView();
                        if (!divTab.Visible) divTab.Visible = true;
                        if (!gvSWPSummaryTable.Visible) gvSWPSummaryTable.Visible = true;
                        //if (resultDivLS.Visible) resultDivLS.Visible = false;
                        if (GridViewResultLS.Visible) GridViewResultLS.Visible = false;
                        resultDiv.Visible = false;
                        CalculateReturnSWP();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
            {
                if (!String.IsNullOrEmpty(txtTransferWithdrawal2.Text) && !String.IsNullOrEmpty(txtiniAmount.Text))
                {
                    if (Convert.ToDouble(txtTransferWithdrawal2.Text) >= 500 && Convert.ToDouble(txtiniAmount.Text) >= 3000)
                    {
                        //SetDefaultView();
                        if (!divTab.Visible) divTab.Visible = true;
                        if (!gvSWPSummaryTable.Visible) gvSWPSummaryTable.Visible = true;
                        if (!gvSTPToSummaryTable.Visible) gvSTPToSummaryTable.Visible = true;
                        //if (resultDivLS.Visible) resultDivLS.Visible = false;
                        if (GridViewResultLS.Visible) GridViewResultLS.Visible = false;

                        resultDiv.Visible = false;
                        CalculateReturnSTP();
                    }
                    else
                    {
                        return;
                    }
                }
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
                            ddlMode.SelectedIndex = 2;
                            break;
                        case "LUMP SUM":
                            //ddlMode.Items[1].Selected = true;
                            ddlMode.SelectedIndex = 1;
                            break;
                        case "STP":
                            //ddlMode.Items[3].Selected = true;
                            ddlMode.SelectedIndex = 3;
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

            InvestMode = ddlMode.SelectedItem.Value;

            #region Save Info

            //try
            //{
            //    conn.ConnectionString = connstr;
            //    if (conn.State == ConnectionState.Closed)
            //    {
            //        conn.Open();
            //    }

            //    sql = "INSERT INTO [T_Iframe_DSP_Distributor] ([Distributor_Name],[Mobile_No],[Email_Id],[Prapared_For],[Investment_mode],[Arn_No]) VALUES (@Distributor_Name,@Mobile_No,@Email_Id,@Prapared_For,@Investment_mode,@Arn_no)";

            //    preparedBy_Param.ParameterName = "@Distributor_Name";
            //    preparedBy_Param.SqlDbType = SqlDbType.NVarChar;
            //    preparedBy_Param.Direction = ParameterDirection.Input;
            //    preparedBy_Param.Value = Preparedby;

            //    mobile_ph_Param.ParameterName = "@Mobile_No";
            //    mobile_ph_Param.SqlDbType = SqlDbType.NVarChar;
            //    mobile_ph_Param.Direction = ParameterDirection.Input;
            //    mobile_ph_Param.Value = Mobile;


            //    email_id_Param.ParameterName = "@Email_Id";
            //    email_id_Param.SqlDbType = SqlDbType.NVarChar;
            //    email_id_Param.Direction = ParameterDirection.Input;
            //    email_id_Param.Value = Email;

            //    prepared_for_Param.ParameterName = "@Prapared_For";
            //    prepared_for_Param.SqlDbType = SqlDbType.NVarChar;
            //    prepared_for_Param.Direction = ParameterDirection.Input;
            //    prepared_for_Param.Value = PreparedFor;

            //    invest_mode_Param.ParameterName = "@Investment_mode";
            //    invest_mode_Param.SqlDbType = SqlDbType.NVarChar;
            //    invest_mode_Param.Direction = ParameterDirection.Input;
            //    invest_mode_Param.Value = InvestMode;

            //    arn_no_Param.ParameterName = "@Arn_No";
            //    arn_no_Param.SqlDbType = SqlDbType.NVarChar;
            //    arn_no_Param.Direction = ParameterDirection.Input;
            //    arn_no_Param.Value = ArnNo;

            //    //  conn.Open();
            //    SqlCommand cmd = new SqlCommand(sql, conn);
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Parameters.Add(preparedBy_Param);
            //    cmd.Parameters.Add(mobile_ph_Param);
            //    cmd.Parameters.Add(email_id_Param);
            //    cmd.Parameters.Add(prepared_for_Param);
            //    cmd.Parameters.Add(invest_mode_Param);
            //    cmd.Parameters.Add(arn_no_Param);


            //    int i = cmd.ExecuteNonQuery();
            //    if (i > 0)
            //    {
            //        success = true;
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    conn.Close();
            //}

            #endregion
        }


        /// <summary>
        /// This function will show Corresponding  controll for selected Calculator
        /// </summary>

        protected void ShowRelativeDiv()
        {
            chkInception4sip.Checked = false;// add by syed
            trInception.Visible = false;
            sipDisclaimer.Visible = false;
            SIPSchDt.Text = "";
            txtddlsipbnmark.Text = "";
            if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP")
            {
                tr_SIP_Sinc.Visible = true;
                lnkTab1.Visible = true; lnkTab2.Visible = true;
                GridViewResultLS.Visible = false; GridViewLumpSum.Visible = false;
                trSipInvst.Visible = true; trLumpInvst.Visible = false; trInitialInvst.Visible = false;
                trCategory.Visible = true; lblSchemeName.Text = "Scheme Name"; trTransferTo.Visible = false;
                trBenchmark.Visible = true; lblInstallmentAmt.Text = "Installment Amount (₹)";
                lblInstallmentAmt.Visible = true;
                SIP_withdrawl.Visible = true;
                SWP_STP_withdrawl.Visible = false;
                txtinstall.Visible = true;
                lblDiffDate.Text = "SIP Date";
                //trTransferWithdrawal.Visible = false;
                GridViewSIPResult.Visible = true;
                lblTransferWithdrawal.Visible = false;
                divTransferWithdrawal.Visible = false;
                txtTransferWithdrawal.Visible = false;
                gvSWPSummaryTable.Visible = false;
                gvSTPToSummaryTable.Visible = false; swpGridView.Visible = false; stpFromGridview.Visible = false;
                stpToGridview.Visible = false; GridViewSTPTOResult.Visible = false; chrtResult.Visible = true;
                divSTP.Visible = false;
                lblDisclaimer.Text = @" While comparing the performance of investments made through SIPs with the respective
                                benchmark of the scheme, the value of the Index on the days when investment is made
                                is assumed to be the price of one unit. The “since inception” returns signify returns realized since the launch of the scheme. 
                                The returns calculated do not take into account Entry Load/ Exit Load. Hence actual “Returns”
                                may be lower.";
            }
            else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
            {
                tr_SIP_Sinc.Visible = false;
                lnkTab1.Visible = false; lnkTab2.Visible = false;
                GridViewResultLS.Visible = true; GridViewLumpSum.Visible = true;
                trLumpInvst.Visible = true; trInitialInvst.Visible = false; trSipInvst.Visible = false;
                trCategory.Visible = true; lblSchemeName.Text = "Scheme Name"; trTransferTo.Visible = false;
                trBenchmark.Visible = true; lblInstallmentAmt.Text = "Installment Amount (₹)";
                lblDiffDate.Text = "SIP Date";
                // trTransferWithdrawal.Visible = false;
                lblTransferWithdrawal.Visible = false;
                divTransferWithdrawal.Visible = false;
                txtTransferWithdrawal.Visible = false;
                GridViewSIPResult.Visible = false;
                gvSWPSummaryTable.Visible = false;
                gvSTPToSummaryTable.Visible = false; swpGridView.Visible = false; stpFromGridview.Visible = false;
                stpToGridview.Visible = false; GridViewSTPTOResult.Visible = false;
                divSTP.Visible = false;
                lblDisclaimer.Text = @"While comparing the performance of investments with the respective benchmark of the scheme,
                                    the value of the index on the days when investment is made is assumed to be the price of one unit.
                                    The “since inception” returns signify returns realized since the launch of the fund.
                                    The returns calculated do not take into account Entry Load/ Exit Load.
                                    Hence, actual “Returns” may be lower.";
            }
            else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP WITH INITIAL INVESTMENT")
            {
                tr_SIP_Sinc.Visible = true;
                lnkTab1.Visible = true; lnkTab2.Visible = true;
                GridViewResultLS.Visible = false; GridViewLumpSum.Visible = false;
                trInitialInvst.Visible = true; trSipInvst.Visible = true; trLumpInvst.Visible = false;
                trCategory.Visible = true; lblSchemeName.Text = "Scheme Name"; trTransferTo.Visible = false;
                trBenchmark.Visible = true; lblInstallmentAmt.Text = "Installment Amount (₹)";
                lblDiffDate.Text = "SIP Date";
                //trTransferWithdrawal.Visible = false; 
                GridViewSIPResult.Visible = true;
                lblTransferWithdrawal.Visible = false;
                divTransferWithdrawal.Visible = false;
                txtTransferWithdrawal.Visible = false;
                gvSWPSummaryTable.Visible = false;
                swpGridView.Visible = false; stpFromGridview.Visible = false; stpToGridview.Visible = false; GridViewSTPTOResult.Visible = false;
                chrtResult.Visible = true;
                divSTP.Visible = false;
                lblDisclaimer.Text = @" While comparing the performance of investments made through SIPs with the respective
                                benchmark of the scheme, the value of the Index on the days when investment is made
                                is assumed to be the price of one unit. The “since inception” returns signify returns realized since the launch of the scheme. The returns
                                calculated do not take into account Entry Load/ Exit Load. Hence actual “Returns”
                                may be lower.";
                //The “since inception” returns are calculated assuming the start date of SIP investments as the “From” date selected.
                //The “since inception” returns signify returns realized since the launch of the Scheme.

            }
            else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP")
            {
                tr_SIP_Sinc.Visible = false;
                lnkTab1.Visible = true; lnkTab2.Visible = true;
                GridViewResultLS.Visible = false; GridViewLumpSum.Visible = false;
                trSipInvst.Visible = true; trLumpInvst.Visible = false; trInitialInvst.Visible = true;
                trCategory.Visible = true; lblSchemeName.Text = "Transfer From"; trTransferTo.Visible = false;
                trBenchmark.Visible = false; lblInstallmentAmt.Text = "Initial Amount (₹)";
                lblInstallmentAmt.Visible = false;
                SIP_withdrawl.Visible = false;
                SWP_STP_withdrawl.Visible = true;
                txtinstall.Visible = false;
                lblDiffDate.Text = "SWP Date";
                //trTransferWithdrawal.Visible = true;
                GridViewSIPResult.Visible = true;
                divTransferWithdrawal.Visible = false;
                lblTransferWithdrawal2.Visible = true; txtTransferWithdrawal2.Visible = true; divTransferWithdrawal2.Visible = true;
                lblTransferWithdrawal2.Text = "Withdrawal Amount";
                Swp_Persentage_payout.Visible = true; Swp_Persentage_payout_message.Visible = true;

                gvSWPSummaryTable.Visible = true; gvFirstTable.Visible = false; gvSTPToSummaryTable.Visible = false; sipGridView.Visible = false;
                swpGridView.Visible = true; stpFromGridview.Visible = false; stpToGridview.Visible = false; GridViewSTPTOResult.Visible = false;
                chrtResult.Visible = true;
                divSTP.Visible = false;
                lblDisclaimer.Text = @"While comparing the performance of an investment made through SWP with the respective benchmark of the scheme,
                                    the value of the index on the days when investment is made is assumed to be the price of one unit of a scheme.
                                    The “since inception” returns signify returns realized since the launch of the Scheme. The returns calculated do not take into account Entry Load/ Exit Load.
                                    Hence, actual “Returns” may be lower.";
            }
            else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
            {
                tr_SIP_Sinc.Visible = false;
                lnkTab1.Visible = true; lnkTab2.Visible = true;
                GridViewResultLS.Visible = false; GridViewLumpSum.Visible = false;
                trSipInvst.Visible = true; trLumpInvst.Visible = false; trInitialInvst.Visible = true;
                trCategory.Visible = false; lblSchemeName.Text = "Transfer From"; trTransferTo.Visible = true;
                trBenchmark.Visible = false; lblInstallmentAmt.Text = "Investment Amount (₹)";
                lblInstallmentAmt.Visible = false;
                SIP_withdrawl.Visible = false;
                SWP_STP_withdrawl.Visible = true;
                txtinstall.Visible = false;
                lblDiffDate.Text = "STP Date";
                //trTransferWithdrawal.Visible = true;
                GridViewSIPResult.Visible = true;
                trInception.Visible = true;
                lblTransferWithdrawal2.Visible = true; txtTransferWithdrawal2.Visible = true; divTransferWithdrawal2.Visible = true;
                lblTransferWithdrawal2.Text = "Transfer Amount";
                Swp_Persentage_payout.Visible = false; Swp_Persentage_payout_message.Visible = false;
                gvSWPSummaryTable.Visible = true; gvFirstTable.Visible = false; gvSTPToSummaryTable.Visible = true; sipGridView.Visible = false;
                swpGridView.Visible = false; stpFromGridview.Visible = true; stpToGridview.Visible = true; GridViewSTPTOResult.Visible = true;
                chrtResult.Visible = true; chrtResultSTPTO.Visible = true;
                divSTP.Visible = true;
                ddlNature.ClearSelection();
                // FillDropdownScheme();
                //  SIPSchDt.Text = "";
                // lblDisclaimer.Text = @"While comparing the performance of an investment made through STP with the respective benchmark of the scheme, the value of the index on the days when investment is made is assumed to be the price of one unit of a scheme. The “since inception” returns signify returns realized since the launch of the fund. The returns calculated do not take into account Entry Load/ Exit Load. Hence, actual “Returns” may be lower.";
                lblDisclaimer.Text = @"While comparing the performance of investments made through STP with the respective benchmarks of the schemes,
                                    the value of the indices on the days when the investment is made is assumed to be the price of one unit of a scheme. 
                                    The “since inception” returns signify returns realized since the launch of the schemes. The returns calculated do not take into account Entry Load/ Exit Load. 
                                    Hence, actual “Returns” may be lower.";

            }


            # region Hide Graph in SWP 24-12-2012
            if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP")
                lnkTab2.Visible = false;
            else
                lnkTab2.Visible = true;


            if (ddlMode.SelectedItem.Value.Trim().ToUpper() != "STP")
            {
                if (ddlschtrto.SelectedIndex > -1)
                    ddlschtrto.SelectedIndex = 0;
                SIPSchDt2.Text = "";
            }

            #endregion

            //if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
            //    LabelInception.Text = "From Scheme Inception Date";
            //else
            //    LabelInception.Text = "Scheme Inception Date";

            FillDropdownScheme();

            resultDiv.Visible = false;
            LSDisc.Visible = false;
            LSDisc1.Visible = false;

            // Dividend Calculation Toggle

            tr_Cal_Type.Visible = (ddlMode.SelectedValue == "SIP" || ddlMode.SelectedValue == "SIP with Initial Investment") && IsSchemeDividend;
            lnkTab5.Visible = tr_Cal_Type.Visible || trCalTypeLmpsm.Visible;

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

        //private void Calculate_Excel_modified()
        //{
        //    try
        //    {
        //        var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\EXCEL", "DSP_SIP_Temp*");
        //        foreach (var f in allFiles)
        //            if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
        //                File.Delete(f);


        //        string _ExcelPath = string.Empty;
        //        string saveAsPath = string.Empty;
        //        double totalInvestedAmount, presentInvestValue;
        //        //_ExcelPath = AppDomain.CurrentDomain.BaseDirectory;
        //        _ExcelPath = HttpContext.Current.Server.MapPath("~");
        //        if (ddlMode.SelectedItem.Value.ToUpper() == "SIP")
        //            _ExcelPath += @"DSP\Excel\DSP_SIP2.xls";
        //        else
        //            _ExcelPath += @"DSP\Excel\DSP_SIP_initial.xls";

        //        string currentSheet = "Summary";
        //        currentSheet = "Detail Data";


        //        Excel.Application excellapp = new Excel.Application();
        //        ExcelWrite xlReport = new ExcelWrite(_ExcelPath);

        //        //var saveAsPath = _ExcelPath.Replace("DSP_SIP", "DSP_SIP_Temp_" + Guid.NewGuid().ToString("N"));
        //        //xlReport.SaveExcelAs(saveAsPath);
        //        //xlReport.CloseExcel(false);
        //        //xlReport = new ExcelWrite(saveAsPath);

        //        try
        //        {

        //            #region Datatable

        //            DataTable tempsipDataTable, tempsipDataTableRet, tempsipDataTableHis;
        //            tempsipDataTable = (System.Data.DataTable)ViewState["sipDataTable"];


        //            string[] columnarray = { "SCHEME_NAME", "NAV_DATE", "NAV", "SCHEME_UNITS", "DIVIDEND_BONUS", "SCHEME_CASHFLOW", "CUMULATIVE_AMOUNT" };

        //            string[] columnarrayhis = { "Name", "Total Units", "Amount Invested", "Current Value", "Profit from Investment", "SIP Returns%" };

        //            string[] columnarrayret = { "Name", "1 Year", "3 Year", "5 Year", "Since Inception" };

        //            for (int i = tempsipDataTable.Columns.Count - 1; i >= 0; i--)
        //            {
        //                DataColumn dc = tempsipDataTable.Columns[i];
        //                if (!columnarray.Contains(dc.ColumnName.ToUpper()))
        //                    tempsipDataTable.Columns.Remove(dc);
        //            }

        //            tempsipDataTable.Columns["DIVIDEND_BONUS"].SetOrdinal(4);
        //            // tempsipDataTable.Rows.RemoveAt(tempsipDataTable.Rows.Count - 1);


        //            tempsipDataTableHis = (System.Data.DataTable)ViewState["GridViewSIPResult"];
        //            tempsipDataTableHis.Columns.RemoveAt(0);

        //            tempsipDataTableRet = (System.Data.DataTable)ViewState["gvFirstTableDT"];

        //            tempsipDataTableRet.Columns.Remove("CURRENT_NAV"); tempsipDataTableRet.Columns.Remove("PROFIT_ONETIME");
        //            tempsipDataTableRet.Columns["Profit_Sip"].SetOrdinal(4);





        //            #endregion

        //            // sipDataTable = (System.Data.DataTable)ViewState["sipDataTable"];
        //            //xlReport = new ExcelWrite(strSavePath);

        //            #region : Excel Calculation
        //            String fromdate, todate, asondate, inceptionDate;
        //            fromdate = Convert.ToDateTime(txtfromDate.Text.ToString().Trim().Split('/')[1] + "/" + txtfromDate.Text.ToString().Trim().Split('/')[0] + "/" + txtfromDate.Text.ToString().Trim().Split('/')[2]).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        //            todate = Convert.ToDateTime(txtToDate.Text.ToString().Trim().Split('/')[1] + "/" + txtToDate.Text.ToString().Trim().Split('/')[0] + "/" + txtToDate.Text.ToString().Trim().Split('/')[2]).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        //            asondate = Convert.ToDateTime(txtvalason.Text.ToString().Trim().Split('/')[1] + "/" + txtvalason.Text.ToString().Trim().Split('/')[0] + "/" + txtvalason.Text.ToString().Trim().Split('/')[2]).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);


        //            string idate = Convert.ToString(ViewState["schmStartDate"]);
        //            //may neeed to change
        //            inceptionDate = Convert.ToDateTime(idate.Split('/')[0] + "/" + idate.Split('/')[1] + "/" + idate.Split('/')[2]).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

        //            xlReport.IsReadOnly = false;
        //            xlReport.IsVisible = false;
        //            xlReport.OpenExcel();
        //            xlReport.SetDataFromDatatable(currentSheet, 4, 1, tempsipDataTable);
        //            xlReport.BorderCells(4, 1, (4 + tempsipDataTable.Rows.Count - 1), tempsipDataTable.Columns.Count, currentSheet);
        //            Excel.Range rang = xlReport.getRange("B3", "G" + (tempsipDataTable.Rows.Count + 3).ToString(), currentSheet);
        //            rang.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //            currentSheet = "Summary";

        //            if (ddlMode.SelectedItem.Value.ToUpper() == "SIP")
        //            {
        //                xlReport.SetCellData("B2", currentSheet, ddlscheme.SelectedItem.Text);
        //                xlReport.SetCellData("B3", currentSheet, txtinstall.Text);
        //                xlReport.SetCellData("B4", currentSheet, ddPeriod_SIP.SelectedItem.Text);
        //                xlReport.SetCellData("B5", currentSheet, ddSIPdate.SelectedItem.Value);
        //                xlReport.SetCellData("B6", currentSheet, " " + fromdate);
        //                xlReport.SetCellData("B7", currentSheet, " " + todate);
        //                xlReport.SetCellData("B8", currentSheet, " " + asondate);

        //                xlReport.SetCellData("A13", currentSheet, "On " + asondate + ", the value of your total investment would be :");
        //                totalInvestedAmount = (double)ViewState["totalInvestedAmount"];
        //                xlReport.SetCellData("B12", currentSheet, totalInvestedAmount.ToString());
        //                presentInvestValue = (double)ViewState["presentInvestValue"];
        //                xlReport.SetCellData("B13", currentSheet, presentInvestValue.ToString());

        //                xlReport.SetCellData("B10", currentSheet, inceptionDate);
        //                xlReport.SetCellData("B11", currentSheet, Convert.ToString(ViewState["FundmanegerText"]));
        //            }
        //            else
        //            {
        //                string initialdate = Convert.ToDateTime(txtIniToDate.Text.ToString().Trim().Split('/')[1] + "/" + txtIniToDate.Text.ToString().Trim().Split('/')[0] + "/" + txtIniToDate.Text.ToString().Trim().Split('/')[2]).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

        //                xlReport.SetCellData("B2", currentSheet, ddlscheme.SelectedItem.Text);
        //                xlReport.SetCellData("B3", currentSheet, txtiniAmount.Text);
        //                xlReport.SetCellData("B4", currentSheet, " " + initialdate);
        //                xlReport.SetCellData("B5", currentSheet, txtinstall.Text);
        //                xlReport.SetCellData("B6", currentSheet, ddPeriod_SIP.SelectedItem.Text);
        //                xlReport.SetCellData("B7", currentSheet, ddSIPdate.SelectedItem.Value);
        //                xlReport.SetCellData("B8", currentSheet, " " + fromdate);
        //                xlReport.SetCellData("B9", currentSheet, " " + todate);
        //                xlReport.SetCellData("B10", currentSheet, " " + asondate);

        //                xlReport.SetCellData("B12", currentSheet, inceptionDate);
        //                xlReport.SetCellData("B13", currentSheet, Convert.ToString(ViewState["FundmanegerText"]));
        //                xlReport.SetCellData("A15", currentSheet, "On " + asondate + ", the value of your total investment would be :");
        //                totalInvestedAmount = (double)ViewState["totalInvestedAmount"];
        //                xlReport.SetCellData("B14", currentSheet, totalInvestedAmount.ToString());
        //                presentInvestValue = (double)ViewState["presentInvestValue"];
        //                xlReport.SetCellData("B15", currentSheet, presentInvestValue.ToString());


        //            }





        //            //if (ddlMode.SelectedItem.Value.Trim().Length >= 12)
        //            //{

        //            //    xlReport.BorderCells(14, 1, 15, 7, currentSheet);
        //            //    Excel.Range rng = xlReport.getRange("A14", "b15", currentSheet);
        //            //    rng.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
        //            //    rng.Font.Bold = true;
        //            //    rng.Font.Size = 11;
        //            //    rng.Font.Name = "Calibri";
        //            //    rng = xlReport.getRange("B14", "G14", currentSheet);
        //            //    rng.MergeCells = true;
        //            //    rng = xlReport.getRange("B15", "G15", currentSheet);
        //            //    rng.MergeCells = true;
        //            //    xlReport.BorderCells(14, 1, 15, 2, currentSheet);
        //            //    //xlReport.FormatCells(14, 1, 15, 2, currentSheet,0, 11, "Calibri",,);

        //            //}



        //            xlReport.SetRowData(currentSheet, xlReport.GetUsedRowCount(currentSheet) + 3, 1, columnarrayhis);
        //            //Microsoft.Office.Interop.Excel.Range objRange = xlReport.getRange(xlReport.GetUsedRowCount(currentSheet), 1, xlReport.GetUsedRowCount(currentSheet), tempsipDataTableRet.Columns.Count, currentSheet);

        //            Excel.Range objRange = xlReport.getRange("A" + (xlReport.GetUsedRowCount(currentSheet)).ToString(), "F" + xlReport.GetUsedRowCount(currentSheet).ToString(), currentSheet);

        //            objRange.Font.Bold = true;
        //            objRange.Font.Size = 11;
        //            objRange.Font.Name = "Calibri";
        //            //objRange.Font.ColorIndex = 2;
        //            xlReport.SetBackgroundColorFromRGB(xlReport.GetUsedRowCount(currentSheet), 1, xlReport.GetUsedRowCount(currentSheet), tempsipDataTableRet.Columns.Count, currentSheet, 33, 88, 103);

        //            xlReport.SetDataFromDatatable(currentSheet, xlReport.GetUsedRowCount(currentSheet) + 1, 1, tempsipDataTableRet);


        //            xlReport.BorderCells(xlReport.GetUsedRowCount(currentSheet) - tempsipDataTableRet.Rows.Count, 1, xlReport.GetUsedRowCount(currentSheet), tempsipDataTableRet.Columns.Count, currentSheet);

        //            Excel.Range rngobjRange = xlReport.getRange("B" + (xlReport.GetUsedRowCount(currentSheet) - 2).ToString(), "F" + xlReport.GetUsedRowCount(currentSheet).ToString(), currentSheet);
        //            rngobjRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //            float top = (float)((double)xlReport.GetUsedRowCount(currentSheet) * 17);


        //            string imagePath = Convert.ToString(Session["imgPath"]);
        //            //imagePath = imagePath.Substring(imagePath.IndexOf("Img", 0));
        //            // imagePath = "http://mfiframes.mutualfundsindia.com" + @"/DSP/" + imagePath.Replace("\\", "/");
        //            xlReport.AddImage(currentSheet, imagePath, 20f, top, 670f, 330f);

        //            Excel.Range oRange = xlReport.getRange("A" + (xlReport.GetUsedRowCount(currentSheet) + 4).ToString(), "E" + (xlReport.GetUsedRowCount(currentSheet) + 28).ToString(), currentSheet);

        //            oRange.MergeCells = true;

        //            xlReport.SetRowData(currentSheet, xlReport.GetUsedRowCount(currentSheet) + 3, 1, columnarrayret);

        //            objRange = xlReport.getRange("A" + (xlReport.GetUsedRowCount(currentSheet)).ToString(), "E" + xlReport.GetUsedRowCount(currentSheet).ToString(), currentSheet);

        //            objRange.Font.Bold = true;
        //            objRange.Font.Size = 11;
        //            objRange.Font.Name = "Calibri";
        //            //objRange.Font.ColorIndex = 2;
        //            //xlReport.SetColorIndexFromRGB(xlReport.GetUsedRowCount(currentSheet), 1, xlReport.GetUsedRowCount(currentSheet), tempsipDataTableHis.Columns.Count, currentSheet, 33, 88, 103);


        //            xlReport.SetDataFromDatatable(currentSheet, xlReport.GetUsedRowCount(currentSheet) + 1, 1, tempsipDataTableHis);
        //            xlReport.BorderCells(xlReport.GetUsedRowCount(currentSheet) - tempsipDataTableHis.Rows.Count, 1, xlReport.GetUsedRowCount(currentSheet), tempsipDataTableHis.Columns.Count, currentSheet);
        //            rngobjRange = xlReport.getRange("B" + (xlReport.GetUsedRowCount(currentSheet) - 2).ToString(), "E" + xlReport.GetUsedRowCount(currentSheet).ToString(), currentSheet);
        //            rngobjRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        //            xlReport.AutoFitColumn(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, currentSheet);

        //            //xlReport.Save();



        //            if (ddlMode.SelectedItem.Value.ToUpper() == "SIP")
        //            {
        //                saveAsPath = _ExcelPath.Replace("DSP_SIP2", "DSP_SIP_Temp_" + Guid.NewGuid().ToString("N"));
        //            }
        //            else
        //            {
        //                saveAsPath = _ExcelPath.Replace("DSP_SIP_initial", "DSP_SIP_Temp_" + Guid.NewGuid().ToString("N"));
        //            }


        //            xlReport.SaveExcelAs(saveAsPath);


        //            #endregion
        //            xlReport.CloseExcel(false);
        //            excellapp.Workbooks.Close();
        //            excellapp.Quit();
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        finally
        //        {

        //            if (excellapp != null)
        //                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excellapp);//                       System.Runtime.InteropServices.Marshal.ReleaseComObject(excellapp);
        //            xlReport = null; excellapp = null;
        //            GC.Collect();
        //            GC.WaitForPendingFinalizers();
        //        }

        //        showExcel(saveAsPath);

        //        //excellapp.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write(@"<script>alert("" error is " + ex.Message + @".."")</script>");
        //        Response.Write(@"<script>alert("" Error in Excel Calculation ."")</script>");
        //    }

        //}


        /// <summary>
        /// This function will Generate Excel
        /// </summary>
        private void ShowExcel(string FundName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FundName + "_Detail_Report.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";


            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Text.StringBuilder objFinalstr = new System.Text.StringBuilder();


            switch (ddlMode.SelectedItem.Value.ToUpper())
            {
                case "SWP":
                    gvSWPSummaryTable.RenderControl(hw);
                    swpGridView.RenderControl(hw);
                    break;
                case "SIP":
                case "SIP WITH INITIAL INVESTMENT":
                    gvFirstTable.RenderControl(hw);
                    sipGridView.RenderControl(hw);
                    break;
                case "STP":
                    divSummary.RenderControl(hw);
                    divSTP.RenderControl(hw);
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
            ImagePath = HttpContext.Current.Server.MapPath("~") + FundName + "\\img\\";
            //  objFinalstr.Replace("img/rimg.png", ImagePath + "rimg.png");
            objFinalstr.Replace("<img src='img/rimg.png' style='vertical-align:middle;'>", "");


            //objFinalstr.Replace("<table", "<br/><table"); 

            //objFinalstr.Replace(@"class=""grdHead""", "style ='text-align: center;	font-family: Roboto;	color: #ffffff;	font-size: 11px;	font-weight:normal;	line-height: 20px;	background:#034EA2;	height:25px; width:15px '");
            objFinalstr.Replace(@"class=""grdHead""", "style ='text-align: center;	font-family: Roboto; color: #034ea2; font-size: 11px;background:#c3dded; font-weight:normal; border-bottom:#034ea2 solid 2px'");
            //objFinalstr.Replace(@"class=""grdRow""", "style=' border: #d0d6db solid 1px;    text-align: center; background-color: #ffffff;    font-family: Roboto;    color: #000;    font-size: 11px;    font-weight: normal;'");
            //objFinalstr.Replace(@"class=""grdAltRow""", "style='background-color: #f4f4f4;    font-family: Roboto;    color: #000;    font-size: 11px; font-weight: normal;    border: #d0d6db solid 1px;     text-align: center;'");


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

                foreach (DataColumn dc in _dt.Columns)
                {
                    if (_dt.Rows[_dt.Rows.Count - 1][dc] == DBNull.Value || _dt.Rows[_dt.Rows.Count - 1][dc].ToString() == string.Empty)
                        _dt.Rows[_dt.Rows.Count - 1][dc] = _dt.Rows[_dt.Rows.Count - 2][dc];

                }
                if (_dt.Rows.Count > 1)
                    _dt.Rows.RemoveAt(_dt.Rows.Count - 1);

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
                    else if (dc.ColumnName.ToUpper() == "INDEX_VALUE_AMOUNT")
                    {
                        CompareToColumn = dc.ColumnName.ToUpper();
                        chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Green;//#034EA2 //System.Drawing.ColorTranslator.FromHtml("#02278b");//
                        chrt.Series[dc.ColumnName].LegendText = "Index Value";
                        chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Solid;
                        // chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Line;

                    }
                    else
                    {
                        CompareToColumn = dc.ColumnName.ToUpper();
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#034EA2");// System.Drawing.Color.Blue;//#034EA2
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
                    // chrt.Series[CompareToColumn].LegendText = "Investment Value";
                    if (ViewState["FundName"] != null)
                        chrt.Titles[0].Text = "SIP Performance Chart:" + Convert.ToString(ViewState["FundName"]);
                }
                else if (ddlMode.SelectedItem.Value.ToUpper() == "SWP")
                {
                    // chrt.Series[strCompareColumn].LegendText = "Investment Amount";
                    //chrt.Series[CompareToColumn].LegendText = "Investment Value";
                    if (ViewState["FundName"] != null)
                        chrt.Titles[0].Text = "SWP Performance Chart:" + Convert.ToString(ViewState["FundName"]);
                }
                else if (ddlMode.SelectedItem.Value.ToUpper() == "STP")
                {
                    if (chrt == chrtResultSTPTO)
                    {
                        if (ViewState["ToSchFundName"] != null)
                        {
                            chrt.Titles[0].Text = "Investment Performance Chart:" + Convert.ToString(ViewState["ToSchFundName"]);
                        }
                    }
                    else
                    {
                        if (ViewState["FundName"] != null)
                            chrt.Titles[0].Text = "Withdrawal Performance Chart:" + Convert.ToString(ViewState["FundName"]);
                    }


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

                var diffAmount = Convert.ToDouble((maxval - minval) / 5);



                chrt.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chrt.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;


                //chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
                //chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;

                //chrt.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Roboto",15f);//, FontStyle.Bold);
                chrt.ChartAreas[0].AxisY.Title = "Value(₹)";

                chrt.ChartAreas[0].AxisY.LabelStyle.Format = "#,###";
                chrt.ChartAreas[0].AxisY.IsLabelAutoFit = true;
                chrt.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Horizontal;
                // chrt.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Roboto", 11);
                chrt.ChartAreas[0].AxisX.Title = "Time Period";
                chrt.ChartAreas[0].AxisX.TitleFont = new Font("Roboto", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisY.TitleFont = new Font("Roboto", 12, FontStyle.Bold);
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
                else if (dtMax.Subtract(dtMin).Days > 90)
                {
                    chrt.ChartAreas[0].AxisX.Interval = 3;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yy";
                }
                else
                {
                    chrt.ChartAreas[0].AxisX.Interval = 10;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM-yy";
                    chrt.ChartAreas[0].AxisY.Interval = diffAmount;

                }

                System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];
                legend.Font = new Font("Roboto", 12, FontStyle.Bold);
                legend.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                chrt.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Roboto", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Roboto", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                chrt.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                //chrt.ChartAreas[0].AxisX.LabelStyle.Font #4f4e50

                #region Chart Image MyRegion


                tmpChartName = "ChartImagetest.jpg";



                string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                string localImgPath = @"Edelweiss\Img";
                localImgPath = System.IO.Path.Combine(appPath, localImgPath);

                string testpath = localImgPath;


                #region Delete MyRegion
                var allImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "Edelweiss\\IMG", "Edelweiss_SIP_Temp*");
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


                imgPath = System.IO.Path.Combine(localImgPath, "Edelweiss_SIP_Temp" + Guid.NewGuid().ToString("N") + "_" + tmpChartName);


                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                }

                if (ddlMode.SelectedItem.Text.ToUpper() == "STP" && chrt == chrtResultSTPTO)
                {
                    Session["imgPathChart2"] = imgPath;
                    chrtResultSTPTO.SaveImage(imgPath);
                }
                else
                {
                    Session["imgPath"] = imgPath;
                    chrtResult.SaveImage(imgPath);
                }

                #endregion

                #endregion


            }
            catch (Exception ex)
            {
                Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");
            }

        }



        public void BindDataTableToBarChart(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt, string strCompareColumn)
        {
            string columnName = null;
            bool showIndex = false;
            string CompareToColumn = string.Empty;

            try
            {
                chrt.Series.Clear();

                #region :Add Date Column

                if (!_dt.Columns.Contains("DateStr"))
                {
                    _dt.Columns.Add("DateStr", typeof(DateTime));
                    foreach (DataRow dr in _dt.Rows)
                    {
                        //dr["DateStr"] = Convert.ToDateTime(string.Format("{0:MM/dd/yyyy}", dr[xField]));
                        var arr = dr[xField].ToString().Split('/');
                        dr["DateStr"] = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[1]), Convert.ToInt32(arr[0]));
                    }
                    if (_dt.Columns.Contains(xField))
                        _dt.Columns.Remove(xField);
                }
                xField = "DateStr";
                #endregion

                #region Special Case

                if (_dt.Rows.Count > 2)
                {
                    for (int i = _dt.Rows.Count - 2; i >= 0; i--)
                    {
                        if (_dt.Rows[i]["CUMILATIVE_AMOUNT_FROM"] == DBNull.Value || _dt.Rows[i]["CUMILATIVE_AMOUNT_TO"] == DBNull.Value)
                        {
                            _dt.Rows.RemoveAt(i);
                        }
                    }
                }

                int _tmp; int maxColumn;

                maxColumn = 6;
                _tmp = _dt.Rows.Count / maxColumn;
                DataTable _tempdt = _dt.Clone();
                for (int i = 0; i <= _dt.Rows.Count - 1; i += _tmp)
                {
                    _tempdt.Rows.Add(_dt.Rows[i].ItemArray);
                }

                if (_dt.Rows.Count % maxColumn == 0)
                    _tempdt.Rows.Add(_dt.Rows[_dt.Rows.Count - 1].ItemArray);

                _dt = _tempdt.Copy();

                #endregion

                foreach (DataColumn dc in _dt.Columns)
                {
                    if (dc.ColumnName == xField)
                        continue;
                    chrt.Series.Add(dc.ColumnName);

                    chrt.Series[dc.ColumnName].ChartType = SeriesChartType.StackedColumn;
                    columnName = dc.ColumnName;

                    if (dc.ColumnName.ToUpper() == strCompareColumn)
                    {
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#4F4E50");// Color.Black;                      
                        chrt.Series[dc.ColumnName].LegendText = "Transfer Fund ( " + Convert.ToString(ViewState["FundName"]) + " )";
                    }
                    else
                    {
                        CompareToColumn = dc.ColumnName.ToUpper();
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#034EA2");// System.Drawing.Color.Blue;//#034EA2
                        chrt.Series[dc.ColumnName].LegendText = "Transferee Fund ( " + Convert.ToString(ViewState["ToSchFundName"]) + " )";
                    }
                    chrt.Series[dc.ColumnName].IsXValueIndexed = true;
                    chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, columnName);
                    chrt.Series[columnName].IsValueShownAsLabel = false;
                    chrt.Series[columnName].BorderWidth = 2;
                    chrt.Series[columnName]["PointWidth"] = (0.6).ToString();
                    chrt.Series[columnName]["PixelPointWidth"] = (30).ToString();
                }



                chrt.Series[0].XValueType = ChartValueType.Date;
                chrt.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yy";
                chrt.ChartAreas[0].AxisX.LabelStyle.Angle = -90;

                double? maxval = 1;

                maxval = _dt.AsEnumerable().Max(x => x.Field<double?>(CompareToColumn));
                if (_dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn)) != null)
                    maxval = maxval >= _dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn)) ? maxval : _dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn));


                maxval = Math.Round(Convert.ToDouble(maxval), 0);
                maxval = RoundToNearest((double)maxval, 1000);

                chrt.ChartAreas[0].AxisY.Maximum = Math.Round(Convert.ToDouble(maxval), 0) * 1.10;

                //chrt.ChartAreas[0].AxisX.Minimum = chrt.ChartAreas[0].AxisX.Minimum * 0.998;
                //chrt.ChartAreas[0].AxisX.Maximum = chrt.ChartAreas[0].AxisX.Maximum * 1.002;

                chrt.ChartAreas[0].AxisY.Title = "Value(₹)";
                chrt.ChartAreas[0].AxisY.LabelStyle.Format = "#,###";
                chrt.ChartAreas[0].AxisY.IsLabelAutoFit = true;
                chrt.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Horizontal;
                chrt.ChartAreas[0].AxisX.Title = "Time Period";
                chrt.ChartAreas[0].AxisX.TitleFont = new Font("Roboto", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisY.TitleFont = new Font("Roboto", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                chrt.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");

                chrt.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Roboto", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Roboto", 12, FontStyle.Bold);
                chrt.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                chrt.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");


                var chrtArea = chrt.ChartAreas[0];
                chrtArea.AxisX.MajorGrid.LineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.NotSet;
                chrtArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
                chrtArea.Visible = true;

                chrt.Titles[0].Text = "STP Performance Chart";

                System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];
                legend.Font = new Font("Roboto", 12, FontStyle.Bold);
                legend.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F4E50");
                legend.Alignment = StringAlignment.Center;
                legend.IsTextAutoFit = true;
                legend.LegendItemOrder = LegendItemOrder.ReversedSeriesOrder;
                legend.TitleAlignment = StringAlignment.Center;
                legend.IsDockedInsideChartArea = true;




                #region Chart Image MyRegion

                tmpChartName = "ChartImagetest.jpg";



                string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                //string localImgPath = @"Sundaram\Img"; Edelweiss
                string localImgPath = @"Edelweiss\Img";
                localImgPath = System.IO.Path.Combine(appPath, localImgPath);

                string testpath = localImgPath;


                #region Delete MyRegion
                var allImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "Edelweiss\\IMG", "Edelweiss_SIP_Temp*");
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


                imgPath = System.IO.Path.Combine(localImgPath, "Edelweiss_SIP_Temp" + Guid.NewGuid().ToString("N") + "_" + tmpChartName);


                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                }

                if (ddlMode.SelectedItem.Text.ToUpper() == "STP" && chrt == chrtResultSTPTO)
                {
                    Session["imgPathChart2"] = imgPath;
                    chrtResultSTPTO.SaveImage(imgPath);
                }
                else
                {
                    Session["imgPath"] = imgPath;
                    chrtResult.SaveImage(imgPath);
                }





                #endregion



                #endregion




            }
            catch (Exception ex)
            {
                Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");
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

        #region
        //public void BindDataTableToChartGeneral(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt)
        //{
        //    string columnName = null;


        //    try
        //    {

        //        chrt.Series.Clear();

        //        List<string> columnList = new List<string>();
        //        columnList.Clear();

        //        foreach (DataColumn dc in _dt.Columns)
        //        {
        //            if (dc.ColumnName == xField)
        //                continue;
        //            chrt.Series.Add(dc.ColumnName);
        //            columnList.Add(dc.ColumnName);
        //            chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Spline;
        //            columnName = dc.ColumnName;

        //            if (dc.ColumnName.ToUpper() == "AMOUNT")
        //            {
        //                //chrt.Series[dc.ColumnName].YAxisType = AxisType.Primary;
        //                chrt.Series[dc.ColumnName].LegendText = "INVESTMENT AMOUNT";
        //                chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
        //                // chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#001F5C");                
        //                // chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Solid;
        //            }
        //            else
        //            {
        //                //  chrt.Series[dc.ColumnName].YAxisType = AxisType.Secondary;
        //                //chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");               
        //                chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Blue;
        //                chrt.Series[dc.ColumnName].LegendText = "CUMULATIVE AMOUNT";
        //                // chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Dash;
        //                // chrt.Series[dc.ColumnName].BorderWidth = 15;
        //                //chrt.Series[dc.ColumnName].ShadowOffset = 8;                        
        //            }

        //            chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, columnName);
        //            chrt.Series[columnName].IsValueShownAsLabel = false;
        //            chrt.Series[columnName].BorderWidth = 1;


        //        }

        //        chrt.Series[0].XValueType = ChartValueType.DateTime;
        //        chrt.Series[0].XValueType = ChartValueType.Date;

        //        chrt.Visible = true;
        //        chrtResult.Visible = true;

        //        double? maxval = 1;
        //        double? minval = 10000;

        //        if (columnList.Count >= 2)
        //        {
        //            maxval = _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[0])) >= _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[1])) ? _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[0])) : _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[1]));

        //            minval = _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[0])) <= _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[1])) ? _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[0])) : _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[1]));


        //        }

        //        chrt.ChartAreas[0].AxisY.Maximum = Math.Round(Convert.ToDouble(maxval), 0) + 1000;

        //        if (minval < 1000)
        //            chrt.ChartAreas[0].AxisY.Minimum = 0;
        //        else
        //            chrt.ChartAreas[0].AxisY.Minimum = Math.Round(Convert.ToDouble(minval), 0) - 500;



        //        chrt.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
        //        chrt.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;

        //        chrt.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
        //        chrt.ChartAreas[0].AxisY.MajorGrid.Enabled = false;


        //        chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
        //        chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;


        //        chrt.ChartAreas[0].AxisY.Title = "Figure in Rs";

        //        if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
        //        {
        //            chrt.ChartAreas[0].AxisX.Title = ddlMode.SelectedItem.Value + " Period";
        //            //chrt.Titles[0].Text = "DSP " + ddlMode.SelectedItem.Value + " Chart";

        //            if (ViewState["FundName"] != null)
        //                chrt.Titles[0].Text = Convert.ToString(ViewState["FundName"]) + " " + ddlMode.SelectedItem.Value + " Graph";
        //        }

        //        chrt.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;
        //        chrt.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.Chocolate;

        //        var chrtArea = chrt.ChartAreas[0];
        //        chrtArea.Visible = true;


        //        System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];

        //        legend.Font = new Font("Roboto", 9, FontStyle.Bold);

        //        #region Chart Image MyRegion


        //        tmpChartName = "ChartImagetest.jpg";



        //        string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
        //        string localImgPath = @"DSP\Img";
        //        localImgPath = System.IO.Path.Combine(appPath, localImgPath);

        //        string testpath = localImgPath;


        //        #region Delete MyRegion
        //        var allImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\IMG", "DSP_SIP_Temp*");
        //        foreach (var f in allImageFiles)
        //            if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
        //                File.Delete(f);


        //        #endregion

        //        #region : save Image


        //        //localImgPath = System.IO.Path.Combine(localImgPath, "SIP"+ Guid.NewGuid().ToString("N"));
        //        if (!Directory.Exists(localImgPath))
        //        {
        //            //using (Directory.CreateDirectory(localImgPath))
        //            //{                    }
        //            Directory.CreateDirectory(localImgPath);
        //        }


        //        imgPath = System.IO.Path.Combine(localImgPath, "DSP_SIP_Temp" + Guid.NewGuid().ToString("N") + "_" + tmpChartName);

        //        Session["imgPath"] = imgPath;
        //        if (File.Exists(imgPath))
        //        {
        //            File.Delete(imgPath);
        //        }
        //        chrtResult.SaveImage(imgPath);
        //        #endregion



        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");
        //    }

        //}

        //public void BindDataTableToChartGeneralSTPTO(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt)
        //{
        //    string columnName = null;


        //    try
        //    {

        //        chrt.Series.Clear();

        //        List<string> columnList = new List<string>();
        //        columnList.Clear();

        //        foreach (DataColumn dc in _dt.Columns)
        //        {
        //            if (dc.ColumnName == xField)
        //                continue;
        //            chrt.Series.Add(dc.ColumnName);
        //            columnList.Add(dc.ColumnName);
        //            chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Spline;
        //            columnName = dc.ColumnName;

        //            if (dc.ColumnName.ToUpper() == "AMOUNT_TO")
        //            {
        //                //chrt.Series[dc.ColumnName].YAxisType = AxisType.Primary;
        //                chrt.Series[dc.ColumnName].LegendText = "INVESTMENT AMOUNT";
        //                chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
        //                // chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#001F5C");                
        //                // chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Solid;
        //            }
        //            else
        //            {
        //                //  chrt.Series[dc.ColumnName].YAxisType = AxisType.Secondary;
        //                //chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");               
        //                chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Blue;
        //                chrt.Series[dc.ColumnName].LegendText = "CUMULATIVE AMOUNT";
        //                // chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Dash;
        //                // chrt.Series[dc.ColumnName].BorderWidth = 15;
        //                //chrt.Series[dc.ColumnName].ShadowOffset = 8;                        
        //            }

        //            chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, columnName);
        //            chrt.Series[columnName].IsValueShownAsLabel = false;
        //            chrt.Series[columnName].BorderWidth = 1;


        //        }

        //        chrt.Series[0].XValueType = ChartValueType.DateTime;

        //        chrt.Visible = true;
        //        chrtResultSTPTO.Visible = true;

        //        double? maxval = 1;
        //        double? minval = 10000;

        //        if (columnList.Count >= 2)
        //        {
        //            maxval = _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[0])) >= _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[1])) ? _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[0])) : _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[1]));

        //            minval = _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[0])) <= _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[1])) ? _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[0])) : _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[1]));


        //        }

        //        chrt.ChartAreas[0].AxisY.Maximum = Math.Round(Convert.ToDouble(maxval), 0) + 500;
        //        if (minval < 1000)
        //            chrt.ChartAreas[0].AxisY.Minimum = 0;
        //        else
        //            chrt.ChartAreas[0].AxisY.Minimum = Math.Round(Convert.ToDouble(minval), 0) - 500;



        //        chrt.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
        //        chrt.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;

        //        chrt.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
        //        chrt.ChartAreas[0].AxisY.MajorGrid.Enabled = false;


        //        chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
        //        chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;


        //        chrt.ChartAreas[0].AxisY.Title = "Figure in Rs";

        //        if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
        //        {
        //            chrt.ChartAreas[0].AxisX.Title = ddlMode.SelectedItem.Value + " Period";
        //            //chrt.Titles[0].Text = "DSP " + ddlMode.SelectedItem.Value + " Chart";

        //            if (ViewState["ToSchFundName"] != null)
        //                chrt.Titles[0].Text = Convert.ToString(ViewState["FundName"]) + " " + ddlMode.SelectedItem.Value + " Graph";
        //        }

        //        chrt.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;
        //        chrt.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.Chocolate;

        //        var chrtArea = chrt.ChartAreas[0];
        //        chrtArea.Visible = true;


        //        System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];

        //        legend.Font = new Font("Roboto", 9, FontStyle.Bold);

        //        #region Chart Image MyRegion


        //        tmpChartName = "ChartImagetest.jpg";



        //        string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
        //        string localImgPath = @"DSP\Img";
        //        localImgPath = System.IO.Path.Combine(appPath, localImgPath);

        //        string testpath = localImgPath;


        //        #region Delete MyRegion
        //        var allImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\IMG", "STPTO_Temp*");
        //        foreach (var f in allImageFiles)
        //            if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
        //                File.Delete(f);


        //        #endregion

        //        #region : save Image


        //        //localImgPath = System.IO.Path.Combine(localImgPath, "SIP"+ Guid.NewGuid().ToString("N"));
        //        if (!Directory.Exists(localImgPath))
        //        {
        //            //using (Directory.CreateDirectory(localImgPath))
        //            //{                    }
        //            Directory.CreateDirectory(localImgPath);
        //        }


        //        imgPath = System.IO.Path.Combine(localImgPath, "STPTO_Temp" + Guid.NewGuid().ToString("N") + "_" + tmpChartName);

        //        Session["imgPathSTPTO"] = imgPath;
        //        if (File.Exists(imgPath))
        //        {
        //            File.Delete(imgPath);
        //        }
        //        chrtResultSTPTO.SaveImage(imgPath);
        //        #endregion



        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");
        //    }

        //}

        #endregion
        #endregion

        #region export to PDF Section

        /// <summary>
        /// This Function will Generate a PDF
        /// </summary>
        /// 

        public string GETHtmlFile()
        {
            System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
            #region Select Html Pages
            switch (RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim())
            {
                case "DISTRIBUTOR":
                    if (ddlMode.SelectedItem.Value.ToUpper() == "SIP")
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissSIPPDFTemplate.html")));// for din regular EdelweissPDFtemplate3 normal
                    }
                    else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP")
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissswpPDFTemplate_uat.html")));
                    }
                    else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissstpPDFTemplate_uat.html")));
                    }
                    else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP WITH INITIAL INVESTMENT")
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissPDFTemplateInitial_uat.html")));
                    }
                    else
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissLsPDFTemplate_uat.html")));
                    }
                    break;
                default:
                    if (ddlMode.SelectedItem.Value.ToUpper() == "SIP")
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissSIPPDFTemplateWOD.html")));//
                    }
                    else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP")
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissswpPDFTemplateWOD_uat.html")));
                    }
                    else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissstpPDFTemplateWOD_uat.html")));
                    }
                    else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP WITH INITIAL INVESTMENT")
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissPDFTemplateInitialWOD_uat.html")));
                    }
                    else
                    {
                        strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Edelweiss/SipCalc/Html/EdelweissLsPDFTemplateWOD_uat.html")));
                    }
                    break;
            }

            #endregion

            return strHTML.ToString();

        }


        public void CreateHTMLSIP()
        {

            System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
            string gvFirstTablestr = string.Empty;
            string GridViewSIPResultstr = string.Empty;
            string sipGridViewstr = string.Empty;
            string path = string.Empty;

            double trydouble = 0;
            var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "Edelweiss\\PDF", "Edelweiss_*");
            foreach (var f in allFiles)
                if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
                    File.Delete(f);

            try
            {
                strHTML.Append(GETHtmlFile());

                if (ddlMode.SelectedItem.Value.ToUpper().StartsWith("SIP"))
                    gvFirstTablestr = FillHtmlGridViewTable(gvFirstTable);
                else if (ddlMode.SelectedItem.Value.ToUpper() == "SWP")
                    gvFirstTablestr = FillHtmlGridViewTable(gvSWPSummaryTable);
                else if (ddlMode.SelectedItem.Value.ToUpper() == "STP")
                {
                    gvFirstTablestr = FillHtmlGridViewTable(gvSWPSummaryTable);
                }
                else
                    gvFirstTablestr = FillHtmlGridViewTable(GridViewLumpSum);



                gvFirstTablestr = "<table border='0' align='center' width='100%' cellpadding='0' cellspacing='0'>" + gvFirstTablestr.Substring(gvFirstTablestr.IndexOf("<th"));



                #region changes on and after 30-10-2012

                if (ddlMode.SelectedItem.Value.ToUpper().StartsWith("SIP"))
                {
                    gvFirstTablestr = gvFirstTablestr.Replace(@"scope=""col""", "style ='text-align:center;font-family: Roboto;color: #034ea2;font-size: 13px;font-weight: normal;background: #c3dded;height: 25px; border-bottom:#034ea2 solid 2px'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""grdRow""", "style = 'font-size:13px;vertical-align:middle;	text-align:left;height: 25px;'");//font-family:Trebuchet MS;
                }
                else
                {
                    gvFirstTablestr = gvFirstTablestr.Replace(@"scope=""col""", "style ='text-align: center;font-family: Roboto;color: #034ea2;font-size: 13px;font-weight: normal;background: #c3dded;height: 25px;border-bottom:#034ea2 solid 2px'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""grdRow""", "style = 'font-size:13px;vertical-align:middle;	text-align:center;height: 25px;'");//font-family:Trebuchet MS;
                }

                gvFirstTablestr = gvFirstTablestr.Replace("<th", "<td");
                gvFirstTablestr = gvFirstTablestr.Replace("</th", "</td");
                gvFirstTablestr = gvFirstTablestr.Replace("style='vertical-align:middle;'", "style='vertical-align:top; padding-top:2px;'");

                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
                {
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""borderlefft""", "style = 'text-align:center; font-size:13px; border-bottom:black solid 1px'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""borderbottom""", "style = 'text-align:center; font-size:13px; border-bottom:black solid 1px'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""leftal""", "style = 'width:300px; font-size:13px; border-bottom:black solid 1px;height: 25px;'");
                }
                else
                {
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""borderlefft""", "style = 'border-bottom:black solid 1px;text-align:center; font-size:13px;'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""borderbottom""", "style = 'border-bottom:black solid 1px;text-align:center; font-size:13px;'");
                    gvFirstTablestr = gvFirstTablestr.Replace(@"class=""leftal""", "style = 'width:250px;border-bottom:black solid 1px;font-size: 13px;height: 25px;'");
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

                        strHTML = strHTML.Replace("<!chartImage!>", "<img alt=''   src='" + imgname + "' />"); //height='90%'
                    }

                }





                if (ddlMode.SelectedItem.Value.ToUpper() == "LUMP SUM")
                    GridViewSIPResultstr = FillHtmlGridViewTable(GridViewResultLS);
                else
                {
                    GridViewSIPResult.Visible = true;
                    GridViewSIPResultstr = FillHtmlGridViewTable(GridViewSIPResult);
                }

                GridViewSIPResultstr = "<table border='0' align='left' width='100%' cellpadding='0' cellspacing='0'>" + GridViewSIPResultstr.Substring(GridViewSIPResultstr.IndexOf("<th"));
                //added 30 oct 2012
                GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"scope=""col""", "style ='text-align: center;font-family: Roboto;color: #034ea2;font-size: 13px;font-weight: normal;background: #c3dded;height: 25px; border-bottom:#034ea2 solid 2px;'");

                GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""grdRow""", "style = 'font-size:13px;	vertical-align:middle;	text-align:left;	border-bottom:#c6c8ca solid 1px;height: 25px;'");//font-family:Trebuchet MS;

                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
                {
                    GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""borderlefft""", "style = 'font-size: 13px;text-align:center;border-bottom:black solid 1px'");
                    GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""borderbottom""", "style = 'font-size: 13px;text-align:center;border-bottom:black solid 1px'");
                    GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""leftal""", "style = 'width:400px;font-size: 13px;text-align:left;border-bottom:black solid 1px'");
                }
                else
                {
                    GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""borderlefft""", "style = 'border-bottom:black solid 1px;text-align:center;font-size: 13px'");
                    GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""borderbottom""", "style = 'border-bottom:black solid 1px;text-align:center;font-size: 13px'");
                    GridViewSIPResultstr = GridViewSIPResultstr.Replace(@"class=""leftal""", "style = 'width:400px;border-bottom:black solid 1px;text-align:left;font-size: 13px'");
                }
                strHTML = strHTML.Replace("<!GridViewSIPResult!>", GridViewSIPResultstr);

                strHTML = strHTML.Replace(@"<table cellspacing='0' rules='all' border='1' id='gvFirstTable' style='width:100%;border-collapse:collapse;", @"<table border='0' align='center' cellpadding='0' cellspacing='0'");




                strHTML = strHTML.Replace("<!disclaimerDiv!>", FillHtmlDisclaimerDiv());
                strHTML = strHTML.Replace(@"class=""grdheader""", "style ='font-family:'Roboto'; font-size: 13px;vertical-align:middle;text-align:center;border-bottom:#c6c8ca solid 1px;height: 25px;font-weight: bold;'");

                strHTML = strHTML.Replace(@"class=""grdRow""", "style = 'font-family:Roboto;	font-size: 13px;vertical-align:middle;	text-align:center;	border-bottom:#c6c8ca solid 1px;height: 25px;'");


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

                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP" || ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
                {
                    if (txtiniAmount.Text.Length > 0)
                        strHTML = strHTML.Replace("<!InitialAmount!>", Convert.ToDouble(txtiniAmount.Text).ToString("n0"));
                    if (txtIniToDate.Text.Length > 0)
                        strHTML = strHTML.Replace("<!inidate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtIniToDate.Text.ToString().Split('/')[2]), Convert.ToInt32(txtIniToDate.Text.ToString().Split('/')[1]), Convert.ToInt32(txtIniToDate.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                    if (txtTransferWithdrawal2.Text.Length > 0)
                        strHTML = strHTML.Replace("<!WithdrawalAmount!>", Convert.ToDouble(txtTransferWithdrawal2.Text).ToString("n0"));


                    strHTML = strHTML.Replace("<!Benchmark!>", txtddlsipbnmark.Text);

                }


                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
                {
                    if (txtinstallLs.Text.Length > 0)
                        strHTML = strHTML.Replace("<!InstallmentAmount!>", Convert.ToDouble(txtinstallLs.Text).ToString("n0"));
                }
                else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SIP")
                {
                    if (txtinstall.Text.Length > 0)
                        strHTML = strHTML.Replace("<!InstallmentAmount!>", Convert.ToDouble(txtinstall.Text).ToString("n0"));
                }
                else
                {
                    if (txtTransferWithdrawal2.Text.Length > 0)
                        strHTML = strHTML.Replace("<!InstallmentAmount!>", Convert.ToDouble(txtTransferWithdrawal2.Text).ToString("n0"));
                }

                strHTML = strHTML.Replace("<!Frequency!>", ddPeriod_SIP.SelectedItem.Text);
                strHTML = strHTML.Replace("<!SIPDate!>", ddSIPdate.SelectedItem.Text);


                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
                {
                    strHTML = strHTML.Replace("<!LsStartDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtLumpfromDate.Text.ToString().Split('/')[2]), Convert.ToInt32(txtLumpfromDate.Text.ToString().Split('/')[1]), Convert.ToInt32(txtLumpfromDate.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                    strHTML = strHTML.Replace("<!LsEndDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtLumpToDate.Text.ToString().Split('/')[2]), Convert.ToInt32(txtLumpToDate.Text.ToString().Split('/')[1]), Convert.ToInt32(txtLumpToDate.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                }
                else
                {
                    strHTML = strHTML.Replace("<!FromDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtfromDate.Text.ToString().Split('/')[2]), Convert.ToInt32(txtfromDate.Text.ToString().Split('/')[1]), Convert.ToInt32(txtfromDate.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                    strHTML = strHTML.Replace("<!ToDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtToDate.Text.ToString().Split('/')[2]), Convert.ToInt32(txtToDate.Text.ToString().Split('/')[1]), Convert.ToInt32(txtToDate.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                    strHTML = strHTML.Replace("<!ValueDate!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(txtvalason.Text.ToString().Split('/')[2]), Convert.ToInt32(txtvalason.Text.ToString().Split('/')[1]), Convert.ToInt32(txtvalason.Text.ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                }

                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
                {
                    strHTML = strHTML.Replace("<!SchemeNameTo!>", ddlschtrto.SelectedItem.Text);//
                }



                if (ddlMode.SelectedItem.Value.Trim().ToUpper().StartsWith("SIP"))
                {
                    if (ViewState["gvFirstTableDT"] != null)
                    {
                        DataTable dtTemp = ViewState["gvFirstTableDT"] as DataTable;
                        if (dtTemp.Rows.Count > 0)
                        {
                            double profitsip = dtTemp.Rows[0]["Profit_Sip"] != DBNull.Value ? Double.TryParse(dtTemp.Rows[0]["Profit_Sip"].ToString(), out trydouble) ? Convert.ToDouble(dtTemp.Rows[0]["Profit_Sip"]) : 0 : 0;
                            if (dtTemp.Rows[0]["yield"] != DBNull.Value && !string.IsNullOrEmpty(dtTemp.Rows[0]["yield"].ToString()))
                            {
                                double sipreturn = Convert.ToDouble(dtTemp.Rows[0]["yield"]);
                                strHTML = strHTML.Replace("<!SIPReturns!>", Convert.ToString(Math.Round(sipreturn, 2)));
                            }
                            strHTML = strHTML.Replace("<!TotalProfit!>", Math.Round(profitsip, 2).ToString("n0"));
                        }
                    }
                }
                else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP")
                {
                    if (ViewState["dwsFirstDataTableSummary"] != null)
                    {
                        DataTable dtTemp = ViewState["dwsFirstDataTableSummary"] as DataTable;
                        if (dtTemp.Rows.Count > 0)
                        {

                            double swpreturn = Convert.ToDouble(dtTemp.Rows[0]["yield"]);

                            strHTML = strHTML.Replace("<!SWPReturns!>", Convert.ToString(Math.Round(swpreturn, 2)));
                        }
                    }
                }
                else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "LUMP SUM")
                {
                    double CurValLs = Convert.ToDouble(ViewState["CompundReturnDayVal"]);
                    double profitLs = Convert.ToDouble(ViewState["profit"]);
                    double returnLs = Convert.ToDouble(ViewState["daysDiffVal"]);

                    strHTML = strHTML.Replace("<!returnInvestment!>", Math.Round(CurValLs, 2).ToString("n0"));
                    strHTML = strHTML.Replace("<!TotalProfit!>", Math.Round(profitLs, 2).ToString("n0"));
                    strHTML = strHTML.Replace("<!LSReturns!>", Convert.ToString(Math.Round(returnLs, 2)));
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

                //if (ViewState["FundDesc2"] != null)
                //    strHTML = strHTML.Replace("<!FundDesc2!>", Convert.ToString(ViewState["FundDesc2"]));

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




                if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
                {

                    if (ViewState["ToSchFundName"] != null)
                        strHTML = strHTML.Replace("<!ToSchFundName!>", Convert.ToString(ViewState["ToSchFundName"]));

                    if (ViewState["CorpusFund2"] != null)
                        strHTML = strHTML.Replace("<!CorpusFund2!>", Convert.ToDouble(ViewState["CorpusFund2"]).ToString("n2"));
                    if (ViewState["ToSchStartDate"] != null)
                        strHTML = strHTML.Replace("<!InceptionDate2!>", Convert.ToDateTime(new DateTime(Convert.ToInt32(ViewState["ToSchStartDate"].ToString().Split('/')[2]), Convert.ToInt32(ViewState["ToSchStartDate"].ToString().Split('/')[1]), Convert.ToInt32(ViewState["ToSchStartDate"].ToString().Split('/')[0])).ToString()).ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString());
                    if (ViewState["ToSchFundmanager"] != null)
                        strHTML = strHTML.Replace("<!FundManagerName2!>", Convert.ToString(ViewState["ToSchFundmanager"]));

                    if (ViewState["ToSchFundName"] != null)
                        strHTML = strHTML.Replace("<!FundName1!>", Convert.ToString(ViewState["ToSchFundName"]));

                    if (ViewState["FundDesc11"] != null)
                        strHTML = strHTML.Replace("<!FundDesc11!>", Convert.ToString(ViewState["FundDesc11"]));

                    if (ViewState["FundDesc21"] != null)
                        strHTML = strHTML.Replace("<!FundDesc21!>", Convert.ToString(ViewState["FundDesc21"]));

                    //if (ViewState["FundDesc31"] != null)
                    //    strHTML = strHTML.Replace("<!FundDesc31!>", Convert.ToString(ViewState["FundDesc31"]));

                    string riskImage1 = string.Empty;
                    string riskImageColor1 = string.Empty;
                    string riskImageColorStmnt1 = string.Empty;
                    if (ViewState["FundDesc31"] != null)
                    {
                        riskImage1 = Convert.ToString(ViewState["FundDesc31"]).Trim().ToUpper();
                        Riskometer(riskImage1, ref riskImageColor1, ref riskImageColorStmnt1);
                        strHTML = strHTML.Replace("<!RiskImage31!>", riskImageColor1);

                        //riskImage1 = Convert.ToString(ViewState["FundDesc31"]).Trim().ToUpper();
                        //RiskColor(riskImage1, ref riskImageColor1, ref riskImageColorStmnt1);
                        //strHTML = strHTML.Replace("<!RiskImage1!>", riskImageColor1);
                        //strHTML = strHTML.Replace("<!riskImageColorStmnt1!>", riskImageColorStmnt1);
                    }

                }



                if (ddlMode.SelectedItem.Value.Trim().ToUpper().StartsWith("SIP"))
                {
                    if (ViewState["totalInvestedAmount"] != null)
                        strHTML = strHTML.Replace("<!TotalInvestmentAmount!>", Convert.ToDouble(ViewState["totalInvestedAmount"]).ToString("n0"));
                    if (ViewState["presentInvestValue"] != null)
                        strHTML = strHTML.Replace("<!totalInvestment!>", Convert.ToDouble(ViewState["presentInvestValue"]).ToString("n0"));
                }
                else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "SWP")
                {
                    if (ViewState["TotalWithdrawalAmount"] != null)
                        strHTML = strHTML.Replace("<!TotalWithdrawalAmount!>", Convert.ToDouble(ViewState["TotalWithdrawalAmount"]).ToString("n0"));
                    if (ViewState["presentInvestValue"] != null)
                        strHTML = strHTML.Replace("<!presentInvestValue!>", Convert.ToDouble(ViewState["presentInvestValue"]).ToString("n0"));

                    if (ViewState["TotalWithdrawalAmount"] != null && ViewState["presentInvestValue"] != null)
                    {
                        double totalSWP = Convert.ToDouble(ViewState["TotalWithdrawalAmount"]) + Convert.ToDouble(ViewState["presentInvestValue"]);
                        double profitSWP = totalSWP - Convert.ToDouble(txtiniAmount.Text);
                        strHTML = strHTML.Replace("<!TotalProfit!>", Math.Round(profitSWP, 2).ToString("n0"));
                    }
                }
                else if (ddlMode.SelectedItem.Value.Trim().ToUpper() == "STP")
                {
                    if (ViewState["investmntValue"] != null)
                        strHTML = strHTML.Replace("<!TotalWithdrawalAmount!>", Convert.ToDouble(ViewState["investmntValue"]).ToString("n0"));
                    if (ViewState["amountLeft"] != null)
                        strHTML = strHTML.Replace("<!totalInvestment!>", Convert.ToDouble(ViewState["amountLeft"]).ToString("n0"));
                    if (ViewState["YieldFrom"] != null)
                        strHTML = strHTML.Replace("<!SWPReturns!>", Convert.ToDouble(ViewState["YieldFrom"]).ToString("n2"));


                    if (ViewState["investmntValue"] != null)
                        strHTML = strHTML.Replace("<!TotalInvestedAmount!>", Convert.ToDouble(ViewState["investmntValue"]).ToString("n0"));
                    if (ViewState["amountLeftTo"] != null)
                        strHTML = strHTML.Replace("<!totalInvestment2!>", Convert.ToDouble(ViewState["amountLeftTo"]).ToString("n0"));
                    if (ViewState["YieldFromTo"] != null)
                        strHTML = strHTML.Replace("<!SIPReturns!>", Convert.ToDouble(ViewState["YieldFromTo"]).ToString("n2"));
                }
                else//lump part
                {
                }



                Session["GUID"] = Guid.NewGuid().ToString();


                path = HttpContext.Current.Server.MapPath("~/Edelweiss/PDF/" + "Edelweiss" + "_" + Convert.ToString(Session["GUID"]) + ".htm");

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (File.Create(path))
                {
                }

                File.WriteAllText(path, strHTML.ToString());
                strHTML = null;
                _SimpleConversion("Edelweiss");

                string pdfName = string.Empty;
                if (Session["GUID"] != null)
                {

                    pdfName = Convert.ToString(Session["GUID"]) + ".pdf";
                    Response.ContentType = "Application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Edelweiss.pdf");
                    Response.TransmitFile(Server.MapPath("~/Edelweiss/PDF/Edelweiss_" + pdfName));
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
                    riskImageColor = "<img src='../IMG/Low_Riskometer.png' />";
                    riskImageColorStmnt = riskImageColor + " (Green)";
                    break;
                case "MODERATELY LOW RISKOMETER":
                    riskImageColor = "<img src='../IMG/Low-to-Moderate_Riskometer.png' />";
                    riskImageColorStmnt = riskImageColor + " (Light Green)";
                    break;
                case "LOW TO MODERATE RISKOMETER":
                    riskImageColor = "<img src='../IMG/Low-to-Moderate_Riskometer.png' />";
                    riskImageColorStmnt = riskImageColor + " (Light Green)";
                    break;
                case "MODERATE RISKOMETER":
                    riskImageColor = "<img src='../IMG/Moderate_Riskometer.png' />";
                    riskImageColorStmnt = riskImageColor + " (Yellow)";
                    break;
                case "MODERATELY HIGH RISKOMETER":
                    riskImageColor = "<img src='../IMG/Moderately-High_Riskometer.png' />";
                    riskImageColorStmnt = riskImageColor + " (Brown)";
                    break;
                case "HIGH RISKOMETER":
                    riskImageColor = "<img src='../IMG/High_Riskometer.png' />";
                    riskImageColorStmnt = riskImageColor + " (Orange)";
                    break;
                case "VERY HIGH RISKOMETER":
                    riskImageColor = "<img src='../IMG/Very-High_Riskometer.png' />";
                    riskImageColorStmnt = riskImageColor + " (Red)";
                    break;
            }
        }
        private static void RiskColorIcra(string riskImage, ref string riskImageColor, ref string riskImageColorStmnt)
        {
            //string color = ColorTranslator.FromHtml("#FFE7EFF2");


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
                //objSipGridView.Visible = false;
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

        public void _SimpleConversion(string CompanyName)
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

                    htmlfile = HttpContext.Current.Server.MapPath("~/" + CompanyName + "/PDF/" + CompanyName + "_" + Convert.ToString(Session["GUID"]) + ".htm");
                    wk.ObjectSettings.Page = htmlfile;

                    wk.ObjectSettings.Load.Proxy = "none";

                    var tmp = wk.Convert();

                    Assert.IsNotEmpty(tmp);
                    var number = 0;
                    lock (this) number = count++;

                    string savepdfpath = string.Empty;
                    string savehtmlpath = string.Empty;
                    savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\" + CompanyName + "\\PDF\\" + CompanyName + "_" + Convert.ToString(Session["GUID"]) + ".pdf";

                    savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\" + CompanyName + "\\PDF\\" + CompanyName + "_" + Convert.ToString(Session["GUID"]) + ".htm";
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

        protected void lnkTab5_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
            RemoveCssFromButton();
            lnkTab5.CssClass = "tab-button tablinks tab-br-md tablinks-active";
        }

        private double CalculateReturn(double FirstValue, double LastValue, int Period)
        {
            if (Period < 365)
            {
                return Math.Round((((LastValue - FirstValue) / FirstValue) * 100) * (Period / Period), 2);
            }
            else
            {
                return Math.Round((((Math.Pow(((LastValue - FirstValue) / FirstValue) + 1, (float)Math.Round((float)365 / (float)Period, 8)) - 1) * 100) * (Period / Period)), 2);
            }
        }

        #endregion

        protected void ExcelCalculation_Click(object sender, EventArgs e)
        {
            ShowExcel("Edelweiss");
        }
    }
}