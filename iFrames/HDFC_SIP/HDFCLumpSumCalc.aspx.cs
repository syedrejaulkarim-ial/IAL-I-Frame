using iFrames.BLL;
using iFrames.DAL;
using iFrames.Kotak;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.HDFC_SIP
{
    public partial class HDFCLumpSumCalc : System.Web.UI.Page
    {
        string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        string SubNatureId = string.Empty;
        //static string schmeId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //edited as per hdfc requirement 09-01-2019
                    var SchByAmfyLst = new List<SchemeByAmfi>();
                    SchByAmfyLst.Add(new SchemeByAmfi { SchemeId = "37593", AmfiCode = "100119" });
                    SchByAmfyLst.Add(new SchemeByAmfi { SchemeId = "37595", AmfiCode = "118968" });
                    SchByAmfyLst.Add(new SchemeByAmfi { SchemeId = "37594", AmfiCode = "102948" });
                    SchByAmfyLst.Add(new SchemeByAmfi { SchemeId = "37596", AmfiCode = "119062" });

                    rdSwitchPanel.Visible = true;

                    List<decimal> notForSip = new List<decimal>();
                    notForSip.AddRange(new decimal[]
                    {
                        11388, //HDFC Gold ETF
                        32122, //HDFC NIFTY 50 ETF
                        32121 //HDFC Sensex ETF
                    });

                    if (!string.IsNullOrEmpty(Request.QueryString["AmfiCode"]))
                    {
                        string AmfiCode = Request.QueryString["AmfiCode"].Trim();
                        var row = SchByAmfyLst.Where(x => x.AmfiCode == AmfiCode).FirstOrDefault();

                        if (Convert.ToInt32(AmfiCode) != 0)
                        {
                            FillNature();
                            int SchemeIdfrmAmfiCode = 0;
                            string SchemeId = null;
                            //edited as per hdfc requirement 09-01-2019
                            if (row != null && !String.IsNullOrEmpty(row.SchemeId))
                            {
                                SchemeId = row.SchemeId;
                            }
                            else
                            {
                                SchemeIdfrmAmfiCode = AllMethods.getSchemeId(AmfiCode);
                                SchemeId = Convert.ToString(SchemeIdfrmAmfiCode);
                            }
                            DataTable dt = AllMethods.getCategoryOfScheme(SchemeId);
                            if (dt.Rows.Count > 0)
                            {
                                string nature = dt.Rows[0][3].ToString();
                                ddlNature.ClearSelection();
                                ddlNature.Items.FindByValue(nature).Selected = true;
                            }
                            FillDropdownScheme();
                            ddlscheme.ClearSelection();
                            ddlscheme.Items.FindByValue(SchemeId).Selected = true;
                            // ddlscheme.SelectedValue = SchemeId;
                            txtiniAmount.Text = "10000";
                            ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            decimal IndexId = AllMethods.GetIndexId(Convert.ToDecimal(SchemeId));
                            DateTime AllocateDate;
                            #region Launch Date
                            using (var principl = new PrincipalCalcDataContext())
                            {
                                var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                                where ind.Scheme_Id == Convert.ToDecimal(SchemeId)
                                                select new
                                                {
                                                    LaunchDate = ind.Launch_Date
                                                };

                                if (allotdate != null && allotdate.Count() > 0)
                                {
                                    SIPSchDt.Value = Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                AllocateDate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                                HdnbenchMarkId.Value = IndexId.ToString();
                                //Code Changed By B.K.Rao Reason When Data passed by querystring the scheme date was selected scheme inception date and populate in from date textbox//
                                string schemeInceptionDate = AllocateDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                FromDate.Text = schemeInceptionDate.ToString();
                            }
                            #endregion
                            if (!ChkTempFund(SchemeId))
                                PerformanceReturn(SchemeId, HdnbenchMarkId.Value, AllocateDate, Convert.ToDouble(txtiniAmount.Text));
                            if (notForSip.Contains(Convert.ToDecimal(SchemeId)))
                            {
                                rdSwitchPanel.Visible = false;
                            }
                        }
                        else
                        {
                            FillNature();
                            FillDropdownScheme();
                            txtiniAmount.Text = "10000";
                            ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["NatureId"]) && !string.IsNullOrEmpty(Request.QueryString["SchemeId"]))
                    {
                        string SchemeId = Request.QueryString["SchemeId"].Trim();
                        if (Convert.ToInt32(SchemeId) != 0)
                        {
                            FillNature();
                            DataTable dt = AllMethods.getCategoryOfScheme(SchemeId);
                            if (dt.Rows.Count > 0)
                            {
                                string nature = dt.Rows[0][3].ToString();
                                ddlNature.ClearSelection();
                                ddlNature.Items.FindByValue(nature).Selected = true;
                            }
                            FillDropdownScheme();
                            ddlscheme.ClearSelection();
                            ddlscheme.Items.FindByValue(SchemeId).Selected = true;
                            // ddlscheme.SelectedValue = SchemeId;
                            txtiniAmount.Text = "10000";
                            ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            decimal IndexId = AllMethods.GetIndexId(Convert.ToDecimal(SchemeId));
                            DateTime AllocateDate;
                            #region Launch Date
                            using (var principl = new PrincipalCalcDataContext())
                            {
                                var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                                where ind.Scheme_Id == Convert.ToDecimal(SchemeId)
                                                select new
                                                {
                                                    LaunchDate = ind.Launch_Date
                                                };

                                if (allotdate != null && allotdate.Count() > 0)
                                {
                                    SIPSchDt.Value = Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                AllocateDate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                                HdnbenchMarkId.Value = IndexId.ToString();
                                //Code Changed By B.K.Rao Reason When Data passed by querystring the scheme date was selected scheme inception date and populate in from date textbox//
                                string schemeInceptionDate = AllocateDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                FromDate.Text = schemeInceptionDate.ToString();
                            }
                            #endregion
                            if (!ChkTempFund(SchemeId))
                                PerformanceReturn(SchemeId, HdnbenchMarkId.Value, AllocateDate, Convert.ToDouble(txtiniAmount.Text));
                            if (notForSip.Contains(Convert.ToDecimal(SchemeId)))
                            {
                                rdSwitchPanel.Visible = false;
                            }
                        }
                        else
                        {
                            FillNature();
                            FillDropdownScheme();
                            txtiniAmount.Text = "10000";
                            ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        FillNature();
                        FillDropdownScheme();
                        txtiniAmount.Text = "10000";
                        ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }

                }
                catch (Exception ex)
                {
                    FillNature();
                    FillDropdownScheme();
                    txtiniAmount.Text = "10000";
                    ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
            }
        }
        protected void FillNature()
        {
            try
            {
                using (var natureData = new PrincipalCalcDataContext())
                {

                    string[] natureExcluded = { "N.A" };
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
                        ddlNature.Items.Insert(0, new ListItem("Select Category", "0"));
                        // ddlNature.Items.Add(new ListItem("Hybrid"));
                        // ddlNature.Items.Add(new ListItem("Fixed Income"));
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

        protected void ddlNature_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropdownScheme();
        }
        public void FillDropdownScheme()
        {
            // FillDropdown(ddlscheme);
            System.Data.DataTable dt = FetchScheme();
            //if (dt.Rows.Count > 0)
            //{
            ddlscheme.DataSource = dt;
            ddlscheme.DataTextField = "Sch_Short_Name";
            ddlscheme.DataValueField = "Scheme_Id";
            ddlscheme.DataBind();
            ddlscheme.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
            //}
        }
        private void FillDropdown(Control ddl)
        {
            try
            {
                System.Data.DataTable dt = FetchScheme();
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
                    ViewState["SchemeInception"] = SchemeInception;

                }

                drpdwn.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
                drpdwn.SelectedIndex = 0;
            }
            catch (Exception ex)
            {


            }
        }
        protected void ddlscheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            //edited 05 Apr 2019 
            rdSwitchPanel.Visible = true;

            List<decimal> notForSip = new List<decimal>();
            notForSip.AddRange(new decimal[]
            {
              11388, //HDFC Gold ETF
              32122, //HDFC NIFTY 50 ETF
              32121 //HDFC Sensex ETF
            });
            // FillDropdownIndex();            
            if (ddlscheme.SelectedIndex == 0 || ddlscheme.SelectedItem.Value == "--")
            {
                return;
            }
            else
            {
                if (notForSip.Contains(Convert.ToDecimal(ddlscheme.SelectedItem.Value)))
                {
                    rdSwitchPanel.Visible = false;
                }
                //FillDropdown(ddlschtrto, ddlscheme.SelectedItem.Value);
            }

            SIPSchDt.Value = "";
            string schmeId = string.Empty;
            DateTime AllocateDate;
            #region Launch Date
            using (var principl = new PrincipalCalcDataContext())
            {
                schmeId = ddlscheme.SelectedItem.Value;

                var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    LaunchDate = ind.Launch_Date
                                };

                if (allotdate != null && allotdate.Count() > 0)
                {
                    SIPSchDt.Value = Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                decimal IndexId = AllMethods.GetIndexId(Convert.ToDecimal(schmeId));
                AllocateDate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                HdnbenchMarkId.Value = IndexId.ToString();
                // HiddenFieldName.Value = index_name.Single().INDEX_NAME.ToString();               
                //  HdSchemes.Value = "s" + schmeId + "#" + "i" + HdnbenchMarkId.Value;
                //  HdToData.Value = DateTime.Today.ToString("dd MMM yyyy");
                //  HdFromData.Value = DateTime.Today.AddYears(-3).ToString("dd MMM yyyy");
                string AllocDate = AllocateDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                FromDate.Text = AllocDate.ToString();
            }
            #endregion


            // temporaryly change when data will come this code need to remove edited 05 Apr 2019            
            if (!ChkTempFund(ddlscheme.SelectedItem.Value))
                PerformanceReturn(schmeId, HdnbenchMarkId.Value, AllocateDate, Convert.ToDouble(txtiniAmount.Text));
        }

        private void PerformanceReturn(string schmeId, string indexId, DateTime allotDate, double amount)
        {
            var BenchmarkNAList = new List<decimal> { 3617, 4610 };
            SqlCommand sqlcmd, sqlcmd1;
            conn.ConnectionString = connstr;
            string strRollingPeriodin = string.Empty;
            string strRollingPeriodin4Index = string.Empty;
            string strRollingPeriod4Liquid = string.Empty;
            string[] overNightSch = { "16709", "10299", "16074", "3081" };
            //Changed By B K Rao
            //string[] liquidFundSch = { "16074", "3081" };
            //hdnLiquidFund.Value = "0";
            //if (liquidFundSch.Contains(schmeId))
            //    hdnLiquidFund.Value = "1";
            //strRollingPeriod4Liquid = "7 D,15 D,1 MM,1 YYYY,3 YYYY,5 YYYY,0 Si";

            //End
            //aurbitary fund, which take setting set of debt (AMFICode)
            string[] aurbiteryList = { "118931", "106796", "129052", "106793" };
            hdnIsoverNight.Value = "0";
            if (overNightSch.Contains(schmeId))
                hdnIsoverNight.Value = "1";
            //added by B K Rao 1month for Liquid and Overnight fund
            strRollingPeriodin = "7 D,15 D,1 MM,3 MM,6 MM,1 YYYY,3 YYYY,5 YYYY,10 YYYY,15 YYYY,0 Si";

            string datediff = string.Empty;
            string strNature = string.Empty;
            string strIndexid = string.Empty;
            string strAddbenchMarkId = string.Empty;
            Decimal dcmlIndexid = 0;

            // DateTime _FromDate = Convert.ToDateTime(FromDate.Text);
            // DateTime _ToDate = Convert.ToDateTime(ToDate.Text);

            DateTime _FromDate = new DateTime(Convert.ToInt16(FromDate.Text.Split('/')[2]),
                                  Convert.ToInt16(FromDate.Text.Split('/')[0]),
                                  Convert.ToInt16(FromDate.Text.Split('/')[1]));

            DateTime _ToDate = new DateTime(Convert.ToInt16(ToDate.Text.Split('/')[2]),
                                     Convert.ToInt16(ToDate.Text.Split('/')[0]),
                                     Convert.ToInt16(ToDate.Text.Split('/')[1]));

            using (var principl = new PrincipalCalcDataContext())
            {
                var sbNtrid = (from tfm in principl.T_FUND_MASTERs
                               where tfm.FUND_ID == principl.T_SCHEMES_MASTERs.Where(s => s.Scheme_Id == Convert.ToDecimal(schmeId)).Select(x => x.Fund_Id).FirstOrDefault()
                               select new { tfm.T_SCHEMES_SUB_NATURE.Sub_Nature, tfm.T_SCHEMES_NATURE.Nature }).FirstOrDefault();//.First().SUB_NATURE_ID;
                                                                                                                                 //SubNatureId = sbNtrid.

                // DataTable dtnature = AllMethods.getCategoryOfScheme(schmeId);
                string nature = sbNtrid.Nature; //dtnature.Rows[0][3].ToString();

                //DataTable dtSubnature = AllMethods.getSubNature(Convert.ToInt32(dtnature.Rows[0][2]));
                string subnature = sbNtrid.Sub_Nature; //dtSubnature.Rows[0][1].ToString();

                //edited done 26-12-2018
                var FundId = principl.T_SCHEMES_MASTERs.Where(n => n.Scheme_Id == Convert.ToDecimal(schmeId)).FirstOrDefault().Fund_Id;
                var SchMasterRow = principl.T_Client_Additional_Benchmarks.Where(n => n.Fund_ID == FundId).FirstOrDefault();
                if (SchMasterRow != null && SchMasterRow.Fund_ID.Value > 0)
                {
                    if (!BenchmarkNAList.Contains(FundId.Value))
                    {
                        dcmlIndexid = SchMasterRow.Add_Bench_ID.Value;
                    }
                }
                else
                {
                    if (nature == "Equity" || nature == "Balanced" || nature == "Dynamic/Asset Allocation")
                    {
                        if (Convert.ToInt32(indexId) == 12 || Convert.ToInt32(indexId) == 35)
                            dcmlIndexid = 142;
                        else
                            dcmlIndexid = 35;
                    }
                    else if (nature == "Liquid" || subnature == "Ultra Short Term" || subnature == "Floating Rate Fund" || subnature == "FMP" || subnature == "Short Term")
                    {
                        dcmlIndexid = 135;
                    }
                    else
                    {
                        dcmlIndexid = 134;
                    }
                }
                strIndexid = HdnbenchMarkId.Value.ToString();
                //var DbRet = (from tfm in principl.RETURN_SIP_PRINCIPALs
                //             where tfm.Scheme_ID == Convert.ToInt32(schmeId)
                //             select tfm.Additional_Index_ID);
                //dcmlIndexid = DbRet.FirstOrDefault().Value;
            }

            //monthDate
            strNature = ddlNature.SelectedValue;
            string schemeAmficode = String.Empty;
            DateTime? launchDate = null;
            using (var db = new PrincipalCalcDataContext())
            {
                var NatureData = (from sm in db.T_SCHEMES_MASTERs
                                  join fm in db.T_FUND_MASTERs on sm.Fund_Id equals fm.FUND_ID
                                  join tsn in db.T_SCHEMES_NATUREs on fm.NATURE_ID equals tsn.Nature_ID
                                  where sm.Scheme_Id == Convert.ToDecimal(schmeId)
                                  select new { tsn.Nature, tsn.Nature_ID, sm.Amfi_Code, sm.Scheme_Name, sm.Launch_Date }).FirstOrDefault();

                if ((strNature == null || strNature == "0") && !String.IsNullOrEmpty(schmeId) && Convert.ToDecimal(schmeId) > 0)
                {
                    strNature = NatureData.Nature;
                }
                schemeAmficode = NatureData.Amfi_Code;
                launchDate = NatureData.Launch_Date.Value;

                //for disclaimer edited 02 May 2019
                divClientDisclaimer.InnerHtml = string.Empty;
                if (schmeId == "37594" || schmeId == "37596")
                {
                    divClientDisclaimer.InnerHtml = @"<div style='width:100%; float:left;'>
                                                        Returns greater than 1 year period are compounded annualized (CAGR). Scheme performance may not strictly be comparable with that of its
                                                        Additional Benchmark in view of balanced nature of the scheme where a portion of scheme’s investments are made in debt instruments.
                                                        For performance of other schemes managed by the Fund Manager, please click
                                                        <a style='text-decoration: underline;' href='https://www.hdfcfund.com/information/fund-manager-wise-scheme-performance/5034' target='_blank'>Fund Manager-Wise Scheme Performance</a>.
                                                        <br />
                                                        Different plans viz. Regular Plan and Direct Plan have a different expense structure. The expenses of the Direct Plan under the 
                                                        Scheme will be lower to the extent of the distribution expenses / commission charged in the Regular Plan. For Performance of Direct Plan, 
                                                        please select the direct plan from the drop-down above.
                                                        <br /> <br />     
                                                        Past performance may or may not be sustained in the future.Load is not taken into consideration for computation of performance.
                                                    </div>";
                }
                if (schmeId == "37595" || schmeId == "37593")
                {
                    divClientDisclaimer.InnerHtml = @"<div style='width:100%; float:left;'>
                                                        Returns greater than 1 year period are compounded annualized (CAGR).  Scheme performance may not strictly be
                                                        comparable with that of its Additional Benchmark in view of balanced nature of the scheme where a portion of scheme’s investments are made in debt instruments. All dividends declared prior to the splitting of the Scheme into Dividend & Growth Options are assumed to be reinvested in the units of the Scheme(Regular Plan-Direct Option) at the then prevailing NAV (ex-dividend NAV). For performance of other schemes managed by the Fund Manager, please click
                                                        <a style='text-decoration: underline;' href='https://www.hdfcfund.com/information/fund-manager-wise-scheme-performance/5034' target='_blank'>Fund Manager-Wise Scheme Performance</a>.
                                                        <br />
                                                        Different plans viz. Regular Plan and Direct Plan have a different expense structure. The expenses of the Direct Plan under the Scheme will be lower to the extent of the distribution expenses / commission charged in the Regular Plan. For Performance of Direct Plan, please refer direct plan from the drop down above. For FPI Portfolio, kindly refer factsheet.
                                                    </div>";
                }
                if (!String.IsNullOrEmpty(schemeAmficode))
                {
                    var disclaimerRow = (from d in db.T_IFRAME_CLIENT_DISCLAIMERs
                                         where d.IsActive && d.Amfi_Code == Convert.ToInt32(schemeAmficode) && DateTime.Now >= d.Effective_From
                                         select new { d }).FirstOrDefault();
                    if (disclaimerRow != null && disclaimerRow.d.Disclaimer_ID > 0)
                    {
                        if (disclaimerRow.d.Effective_To.HasValue)
                        {
                            if (DateTime.Now <= disclaimerRow.d.Effective_To)
                                divClientDisclaimer.InnerHtml = disclaimerRow.d.Disclaimer;
                        }
                        else
                            divClientDisclaimer.InnerHtml = disclaimerRow.d.Disclaimer;
                    }
                }
            }

            try
            {

                // string datediff = "365 d";
                int oval = 0;
                string IndexBenchmarlIds = null;
                int SettingId = 2;
                DataTable nwdataTble = null, nwIndexDataTble = null, odatatble = null, oindexDataTable = null;
                DataSet dset1 = new DataSet();
                DataSet dset2 = new DataSet();
                DataSet dst1 = new DataSet();
                DataSet dst2 = new DataSet();

                if (strNature == "Debt" || strNature == "Gilt" || strNature == "Liquid")
                {
                    SettingId = 34;//simple
                }
                else if (aurbiteryList.Contains(schemeAmficode))
                {
                    SettingId = 34;
                }
                else
                {
                    SettingId = 33;  //absolute
                }
                var perfTitle = "";
                using (var db = new PrincipalCalcDataContext())
                {
                    decimal setId = (decimal)SettingId;
                    var principl = (from i in db.T_MFIE_RETURNSETTINGSETs
                                    where i.SETTINGSET_ID == setId
                                    select i).FirstOrDefault();
                    if (principl != null && principl.SETTINGSET_ID > 0)
                    {
                        perfTitle = "<span>Note: Returns less than 1 year are " + principl.RETURNTYPELESSTHAN + " and returns more than or equal to 1 year are " + principl.RETURNTYPEGRATERTHAN + ".&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Value of Rs 10,000 invested.</span>";
                    }
                }
                txtPerformanceTitle.InnerHtml = perfTitle;
                //---------------------------TRI---------------------------
                //using (var principl = new PrincipalCalcDataContext())
                //{
                //    var ISTRI = (principl.T_INDEX_MASTERs.Where(x => x.INDEX_ID == Convert.ToInt32(indexId)).Where(c => c.IsTRI.HasValue));
                //    var ISTRI_dcmlIndexid = (principl.T_INDEX_MASTERs.Where(x => x.INDEX_ID == Convert.ToInt32(dcmlIndexid)).Where(c => c.IsTRI.HasValue));
                //    if (ISTRI.Any())
                //       indexId = Convert.ToString(ISTRI.FirstOrDefault().TRI_PRI_Index.Value);
                //    if (ISTRI_dcmlIndexid.Any())
                //        dcmlIndexid = ISTRI_dcmlIndexid.FirstOrDefault().TRI_PRI_Index.Value;
                //}
                //----------------------EndEventHandler TRI---------------
                IndexBenchmarlIds = dcmlIndexid + "," + indexId;
                DateTime LastDatyOfPrevMth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                //TimeSpan dateDiff4Index = _ToDate.Subtract(allotDate);
                TimeSpan dateDiff4Index = LastDatyOfPrevMth.Subtract(allotDate);
                int dayDiffIndex = dateDiff4Index.Days;
                strRollingPeriodin4Index = "7 D,15 D,1 MM,3 MM,6 MM,1 YYYY,3 YYYY,5 YYYY,10 YYYY,15 YYYY," + dayDiffIndex + " d";
                List<double> schemval = new List<double>();
                List<double> benchval = new List<double>();
                List<double> adbenchval = new List<double>();

                sqlcmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", conn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandTimeout = 2000;
                sqlcmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
                sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", SettingId));
                sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", allotDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                //sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                //sqlcmd.Parameters.Add(new SqlParameter("@DateTo", _ToDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));


                sqlcmd.Parameters.Add(new SqlParameter("@DateTo", LastDatyOfPrevMth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

                sqlcmd.Parameters.Add(new SqlParameter("@RoundTill", 6));
                sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
                sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
                sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
                sqlcmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                sqlcmd.Parameters.Add(new SqlParameter("@OtherCalculation", @"N~L"));





                sqlcmd1 = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", conn);
                sqlcmd1.CommandType = CommandType.StoredProcedure;
                sqlcmd1.CommandTimeout = 2000;
                sqlcmd1.Parameters.Add(new SqlParameter("@IndexIDs", IndexBenchmarlIds));
                sqlcmd1.Parameters.Add(new SqlParameter("@SettingSetID", SettingId));
                sqlcmd1.Parameters.Add(new SqlParameter("@DateFrom", allotDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                // sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                //sqlcmd1.Parameters.Add(new SqlParameter("@DateTo", _ToDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                sqlcmd1.Parameters.Add(new SqlParameter("@DateTo", LastDatyOfPrevMth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

                sqlcmd1.Parameters.Add(new SqlParameter("@RoundTill", 6));
                sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin4Index));
                sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
                sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
                sqlcmd1.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                sqlcmd1.Parameters.Add(new SqlParameter("@OtherCalculation", @"N~L"));

                string day = LastDatyOfPrevMth.Day.ToString();
                lblDetailsDate.InnerHtml = string.Format(LastDatyOfPrevMth.ToString("dd{0} MMMM yyyy"), "<sup>" + GetSuffix(day) + "</sup>");

                dset1.Clear();
                dset2.Clear();
                dset1.Reset();
                dset2.Reset();
                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlcmd);
                sqlDa.Fill(dset1);
                //SqlDataAdapter SQLdA1 = new SqlDataAdapter(sqlcmd1);
                sqlDa.SelectCommand = sqlcmd1;
                sqlDa.Fill(dset2);

                //----------------
                //edited 26-12-2018 as per clients need if sceeme has no value the benchmark and add-bench mark is na 
                foreach (DataColumn v in dset1.Tables[0].Columns)
                {
                    if (Convert.ToString(dset1.Tables[0].Rows[0][dset1.Tables[0].Columns.IndexOf(v)]) == "N/A")
                    {
                        for (int i = 0; i < dset2.Tables[0].Rows.Count; i++)
                            dset2.Tables[0].Rows[i][dset1.Tables[0].Columns.IndexOf(v)] = "N/A";
                    }

                }

                //-----------------
                nwdataTble = null;
                nwIndexDataTble = null;
                nwdataTble = dset1.Tables[0];
                nwIndexDataTble = dset2.Tables[0];
                // Code Commented by B.K.Rao due to when HDFC Equity Fund Growth select datatable not contains index_name 'S&P BSE Sensex TRI' it goes exception.
                // var s = nwIndexDataTble.Select("INDEX_NAME='S&P BSE Sensex TRI'").ToList()[0];                            
                //AddlBenchmarkVal.Value = s["INDEX_ID"].ToString();
                //End
                nwIndexDataTble.Columns.Remove("INDEX_TYPE");
                // if (day2 > 365)
                //     CompundReturnDayVal = amountLs * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day2 / 365, 4));
                // else
                //     CompundReturnDayVal = amountLs + amountLs * (double)daysDiffVal / 100;

                List<double> SchemeNavVal = new List<double>();
                List<double> SchemeAmountVal = new List<double>();


                List<double> IndexNavVal = new List<double>();
                List<double> IndexAmountVal = new List<double>();

                List<double> AddBenchNavVal = new List<double>();
                List<double> AddBenchAmountVal = new List<double>();


                using (var principl = new PrincipalCalcDataContext())
                {
                    //edited done 24-12-2018
                    //decimal FundId = 0;

                    var FundId = principl.T_SCHEMES_MASTERs.Where(n => n.Scheme_Id == Convert.ToDecimal(schmeId)).FirstOrDefault().Fund_Id;
                    //if (BenchmarkNAList.Contains(FundId.Value))
                    //{
                    //    nwIndexDataTble.Rows.RemoveAt(1);
                    //}


                    for (int i = 2; i < nwdataTble.Columns.Count; i++)
                    {
                        int day2;
                        if (nwdataTble.Rows[0][i].ToString() != "N/A")
                        {
                            var daysDiffVal1 = Convert.ToDouble(nwdataTble.Rows[0][i]);
                            SchemeNavVal.Add(Convert.ToDouble(daysDiffVal1));
                            if (i == 8)
                            {
                                day2 = 365 * 3;
                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                            }
                            else if (i == 9)
                            {
                                day2 = 365 * 5;
                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                            }
                            else if (i == 10)
                            {
                                day2 = 365 * 10;
                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                            }
                            else if (i == 11)
                            {
                                day2 = 365 * 15;
                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                            }
                            else if (i == 12)
                            {

                                DateTime dateDiffSiw = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                                TimeSpan dateDiffSi = dateDiffSiw.Subtract(allotDate);
                                day2 = dateDiffSi.Days;
                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                            }
                            else
                                SchemeAmountVal.Add(amount + amount * (double)daysDiffVal1 / 100);
                        }
                        else
                        {
                            SchemeNavVal.Add(0);
                            SchemeAmountVal.Add(0);
                        }
                    }
                    for (int i = 2; i < nwIndexDataTble.Columns.Count; i++)
                    {
                        int day3;
                        int indxId = Convert.ToInt32(nwIndexDataTble.Rows[0][0]);
                        DateTime lnchDt = new DateTime(1995, 11, 3);
                        if (nwIndexDataTble.Rows[0][i].ToString() != "N/A")
                        {
                                var daysDiffVal = Convert.ToDouble(nwIndexDataTble.Rows[0][i]);
                                IndexNavVal.Add(Convert.ToDouble(daysDiffVal));
                                if (i == 8)
                                {
                                    day3 = 365 * 3;
                                    IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                }
                                else if (i == 9)
                                {
                                    day3 = 365 * 5;
                                    IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                }
                                else if (i == 10)
                                {
                                    day3 = 365 * 10;
                                    IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                }
                                else if (i == 11)
                                {
                                    day3 = 365 * 15;
                                    IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                }
                                else if (i == 12)
                                {
                                    //check is nifty 50 or nifty 50 tri with launch date
                                    if ((indxId == 35 || indxId == 12) && launchDate.Value < lnchDt)
                                    {
                                        IndexAmountVal.Add(0);
                                        IndexNavVal[9] = 0;
                                    }
                                    else
                                    {
                                        TimeSpan dateDiffSi = _ToDate.Subtract(allotDate);
                                        day3 = dateDiffSi.Days;
                                        IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                    }
                                }
                                else
                                    IndexAmountVal.Add(amount + amount * (double)daysDiffVal / 100);
                        }
                        else
                        {
                            IndexNavVal.Add(0);
                            IndexAmountVal.Add(0);
                        }
                        int day4;
                        if ((schmeId != "16762" && schmeId != "3143") && !BenchmarkNAList.Contains(FundId.Value))
                        {
                            indxId = Convert.ToInt32(nwIndexDataTble.Rows[1][0]);
                            if (nwIndexDataTble.Rows[1][i].ToString() != "N/A")
                            {
                                    var daysDiffVal = Convert.ToDouble(nwIndexDataTble.Rows[1][i]);
                                    AddBenchNavVal.Add(Convert.ToDouble(daysDiffVal));
                                    if (i == 8)
                                    {
                                        day4 = 365 * 3;
                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));
                                    }
                                    else if (i == 9)
                                    {
                                        day4 = 365 * 5;
                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));
                                    }
                                    else if (i == 10)
                                    {
                                        day4 = 365 * 10;
                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));
                                    }
                                    else if (i == 11)
                                    {
                                        day4 = 365 * 15;
                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));
                                    }
                                    else if (i == 12)
                                    {
                                        TimeSpan dateDiffSi = _ToDate.Subtract(allotDate);
                                        day4 = dateDiffSi.Days;
                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));

                                    }
                                    else
                                        AddBenchAmountVal.Add(amount + amount * (double)daysDiffVal / 100);
                            }
                            else
                            {
                                AddBenchNavVal.Add(0);
                                AddBenchAmountVal.Add(0);
                            }
                        }
                    }

                    DataTable perfDataTable = new DataTable();
                    perfDataTable.Columns.Add("Scheme_Index", typeof(string));
                    perfDataTable.Columns.Add("7DReturn", typeof(string));
                    perfDataTable.Columns.Add("7DAmount", typeof(string));
                    perfDataTable.Columns.Add("15DReturn", typeof(string));
                    perfDataTable.Columns.Add("15DAmount", typeof(string));
                    perfDataTable.Columns.Add("1MReturn", typeof(string));
                    perfDataTable.Columns.Add("1MAmount", typeof(string));
                    perfDataTable.Columns.Add("3MReturn", typeof(string));
                    perfDataTable.Columns.Add("3MAmount", typeof(string));
                    perfDataTable.Columns.Add("6MReturn", typeof(string));
                    perfDataTable.Columns.Add("6MAmount", typeof(string));
                    perfDataTable.Columns.Add("1YReturn", typeof(string));
                    perfDataTable.Columns.Add("1YAmount", typeof(string));
                    perfDataTable.Columns.Add("3YReturn", typeof(string));
                    perfDataTable.Columns.Add("3YAmount", typeof(string));
                    perfDataTable.Columns.Add("5YReturn", typeof(string));
                    perfDataTable.Columns.Add("5YAmount", typeof(string));
                    perfDataTable.Columns.Add("10YReturn", typeof(string));
                    perfDataTable.Columns.Add("10YAmount", typeof(string));
                    perfDataTable.Columns.Add("15YReturn", typeof(string));
                    perfDataTable.Columns.Add("15YAmount", typeof(string));
                    perfDataTable.Columns.Add("SIReturn", typeof(string));
                    perfDataTable.Columns.Add("SIAmount", typeof(string));
                    perfDataTable.Rows.Add(nwdataTble.Rows[0]["Scheme_Name"].ToString(),
                        SchemeNavVal[0] == 0 ? "--" : SchemeNavVal[0].ToString(),
                        SchemeAmountVal[0] == 0 ? "--" : SchemeAmountVal[0].ToString(),
                        SchemeNavVal[1] == 0 ? "--" : SchemeNavVal[1].ToString(),
                        SchemeAmountVal[1] == 0 ? "--" : SchemeAmountVal[1].ToString(),
                        SchemeNavVal[2] == 0 ? "--" : SchemeNavVal[2].ToString(),
                        SchemeAmountVal[2] == 0 ? "--" : SchemeAmountVal[2].ToString(),
                        SchemeNavVal[3] == 0 ? "--" : SchemeNavVal[3].ToString(),
                        SchemeAmountVal[3] == 0 ? "--" : SchemeAmountVal[3].ToString(),
                        SchemeNavVal[4] == 0 ? "--" : SchemeNavVal[4].ToString(),
                        SchemeAmountVal[4] == 0 ? "--" : SchemeAmountVal[4].ToString(),
                        SchemeNavVal[5] == 0 ? "--" : SchemeNavVal[5].ToString(),
                        SchemeAmountVal[5] == 0 ? "--" : SchemeAmountVal[5].ToString(),
                        SchemeNavVal[6] == 0 ? "--" : SchemeNavVal[6].ToString(),
                        SchemeAmountVal[6] == 0 ? "--" : SchemeAmountVal[6].ToString(),
                        SchemeNavVal[7] == 0 ? "--" : SchemeNavVal[7].ToString(),
                        SchemeAmountVal[7] == 0 ? "--" : SchemeAmountVal[7].ToString(),
                        SchemeNavVal[8] == 0 ? "--" : SchemeNavVal[8].ToString(),
                        SchemeAmountVal[8] == 0 ? "--" : SchemeAmountVal[8].ToString(),
                        SchemeNavVal[9] == 0 ? "--" : SchemeNavVal[9].ToString(),
                        SchemeAmountVal[9] == 0 ? "--" : SchemeAmountVal[9].ToString(),
                        SchemeNavVal[10] == 0 ? "--" : SchemeNavVal[10].ToString(),
                        SchemeAmountVal[10] == 0 ? "--" : SchemeAmountVal[10].ToString());
                    //For order by index
                    if (nwIndexDataTble.Rows[0]["Index_Id"].ToString() == indexId)
                    {
                        perfDataTable.Rows.Add(nwIndexDataTble.Rows[0]["Index_Name"].ToString().Substring(nwIndexDataTble.Rows[0]["Index_Name"].ToString().Length - 1, 1) != "*" ? nwIndexDataTble.Rows[0]["Index_Name"].ToString() : nwIndexDataTble.Rows[0]["Index_Name"].ToString().Remove(nwIndexDataTble.Rows[0]["Index_Name"].ToString().Length - 1),
                            IndexNavVal[0] == 0 ? "--" : IndexNavVal[0].ToString(),
                            IndexAmountVal[0] == 0 ? "--" : IndexAmountVal[0].ToString(),
                            IndexNavVal[1] == 0 ? "--" : IndexNavVal[1].ToString(),
                            IndexAmountVal[1] == 0 ? "--" : IndexAmountVal[1].ToString(),
                            IndexNavVal[2] == 0 ? "--" : IndexNavVal[2].ToString(),
                            IndexAmountVal[2] == 0 ? "--" : IndexAmountVal[2].ToString(),
                            IndexNavVal[3] == 0 ? "--" : IndexNavVal[3].ToString(),
                            IndexAmountVal[3] == 0 ? "--" : IndexAmountVal[3].ToString(),
                            IndexNavVal[4] == 0 ? "--" : IndexNavVal[4].ToString(),
                            IndexAmountVal[4] == 0 ? "--" : IndexAmountVal[4].ToString(),
                            IndexNavVal[5] == 0 ? "--" : IndexNavVal[5].ToString(),
                            IndexAmountVal[5] == 0 ? "--" : IndexAmountVal[5].ToString(),
                            IndexNavVal[6] == 0 ? "--" : IndexNavVal[6].ToString(),
                            IndexAmountVal[6] == 0 ? "--" : IndexAmountVal[6].ToString(),
                            IndexNavVal[7] == 0 ? "--" : IndexNavVal[7].ToString(),
                            IndexAmountVal[7] == 0 ? "--" : IndexAmountVal[7].ToString(),
                             IndexNavVal[8] == 0 ? "--" : IndexNavVal[8].ToString(),
                            IndexAmountVal[8] == 0 ? "--" : IndexAmountVal[8].ToString(),
                             IndexNavVal[9] == 0 ? "--" : IndexNavVal[9].ToString(),
                            IndexAmountVal[9] == 0 ? "--" : IndexAmountVal[9].ToString(),     
                            IndexNavVal[10] == 0 ? "--" : IndexNavVal[10].ToString(),
                            IndexAmountVal[10] == 0 ? "--" : IndexAmountVal[10].ToString());//----This row represent index 
                        if ((schmeId != "16762" && schmeId != "3143") && !BenchmarkNAList.Contains(FundId.Value))
                            perfDataTable.Rows.Add(nwIndexDataTble.Rows[1]["Index_Name"].ToString(),
                                AddBenchNavVal[0] == 0 ? "--" : AddBenchNavVal[0].ToString(),
                                AddBenchAmountVal[0] == 0 ? "--" : AddBenchAmountVal[0].ToString(),
                                AddBenchNavVal[1] == 0 ? "--" : AddBenchNavVal[1].ToString(),
                                AddBenchAmountVal[1] == 0 ? "--" : AddBenchAmountVal[1].ToString(),
                                AddBenchNavVal[2] == 0 ? "--" : AddBenchNavVal[2].ToString(),
                                AddBenchAmountVal[2] == 0 ? "--" : AddBenchAmountVal[2].ToString(),
                                AddBenchNavVal[3] == 0 ? "--" : AddBenchNavVal[3].ToString(),
                                AddBenchAmountVal[3] == 0 ? "--" : AddBenchAmountVal[3].ToString(),
                                AddBenchNavVal[4] == 0 ? "--" : AddBenchNavVal[4].ToString(),
                                AddBenchAmountVal[4] == 0 ? "--" : AddBenchAmountVal[4].ToString(),
                                AddBenchNavVal[5] == 0 ? "--" : AddBenchNavVal[5].ToString(),
                                AddBenchAmountVal[5] == 0 ? "--" : AddBenchAmountVal[5].ToString(),
                                AddBenchNavVal[6] == 0 ? "--" : AddBenchNavVal[6].ToString(),
                                AddBenchAmountVal[6] == 0 ? "--" : AddBenchAmountVal[6].ToString(),
                                AddBenchNavVal[7] == 0 ? "--" : AddBenchNavVal[7].ToString(),
                                AddBenchAmountVal[7] == 0 ? "--" : AddBenchAmountVal[7].ToString(),
                                 AddBenchNavVal[8] == 0 ? "--" : AddBenchNavVal[8].ToString(),
                                AddBenchAmountVal[8] == 0 ? "--" : AddBenchAmountVal[8].ToString(),
                                 AddBenchNavVal[9] == 0 ? "--" : AddBenchNavVal[9].ToString(),
                                AddBenchAmountVal[9] == 0 ? "--" : AddBenchAmountVal[9].ToString(),
                                AddBenchNavVal[10] == 0 ? "--" : AddBenchNavVal[10].ToString(),
                                AddBenchAmountVal[10] == 0 ? "--" : AddBenchAmountVal[10].ToString());//this row represent Additional bench mark 
                    }
                    else
                    {
                        if ((schmeId != "16762" && schmeId != "3143") && !BenchmarkNAList.Contains(FundId.Value))
                            perfDataTable.Rows.Add(nwIndexDataTble.Rows[1]["Index_Name"].ToString(),
                                AddBenchNavVal[0] == 0 ? "--" : AddBenchNavVal[0].ToString(),
                                AddBenchAmountVal[0] == 0 ? "--" : AddBenchAmountVal[0].ToString(),
                                AddBenchNavVal[1] == 0 ? "--" : AddBenchNavVal[1].ToString(),
                                AddBenchAmountVal[1] == 0 ? "--" : AddBenchAmountVal[1].ToString(),
                                AddBenchNavVal[2] == 0 ? "--" : AddBenchNavVal[2].ToString(),
                                AddBenchAmountVal[2] == 0 ? "--" : AddBenchAmountVal[2].ToString(),
                                AddBenchNavVal[3] == 0 ? "--" : AddBenchNavVal[3].ToString(),
                                AddBenchAmountVal[3] == 0 ? "--" : AddBenchAmountVal[3].ToString(),
                                AddBenchNavVal[4] == 0 ? "--" : AddBenchNavVal[4].ToString(),
                                AddBenchAmountVal[4] == 0 ? "--" : AddBenchAmountVal[4].ToString(),
                                AddBenchNavVal[5] == 0 ? "--" : AddBenchNavVal[5].ToString(),
                                AddBenchAmountVal[5] == 0 ? "--" : AddBenchAmountVal[5].ToString(),
                                AddBenchNavVal[6] == 0 ? "--" : AddBenchNavVal[6].ToString(),
                                AddBenchAmountVal[6] == 0 ? "--" : AddBenchAmountVal[6].ToString(),
                                AddBenchNavVal[7] == 0 ? "--" : AddBenchNavVal[7].ToString(),
                                AddBenchAmountVal[7] == 0 ? "--" : AddBenchAmountVal[7].ToString(),
                                 AddBenchNavVal[8] == 0 ? "--" : AddBenchNavVal[8].ToString(),
                                AddBenchAmountVal[8] == 0 ? "--" : AddBenchAmountVal[8].ToString(),
                                 AddBenchNavVal[9] == 0 ? "--" : AddBenchNavVal[9].ToString(),
                                AddBenchAmountVal[9] == 0 ? "--" : AddBenchAmountVal[9].ToString(),
                                AddBenchNavVal[10] == 0 ? "--" : AddBenchNavVal[10].ToString(),
                                AddBenchAmountVal[10] == 0 ? "--" : AddBenchAmountVal[10].ToString());//----This row represent index
                        perfDataTable.Rows.Add(nwIndexDataTble.Rows[0]["Index_Name"].ToString(),
                            IndexNavVal[0] == 0 ? "--" : IndexNavVal[0].ToString(),
                            IndexAmountVal[0] == 0 ? "--" : IndexAmountVal[0].ToString(),
                            IndexNavVal[1] == 0 ? "--" : IndexNavVal[1].ToString(),
                            IndexAmountVal[1] == 0 ? "--" : IndexAmountVal[1].ToString(),
                            IndexNavVal[2] == 0 ? "--" : IndexNavVal[2].ToString(),
                            IndexAmountVal[2] == 0 ? "--" : IndexAmountVal[2].ToString(),
                            IndexNavVal[3] == 0 ? "--" : IndexNavVal[3].ToString(),
                            IndexAmountVal[3] == 0 ? "--" : IndexAmountVal[3].ToString(),
                            IndexNavVal[4] == 0 ? "--" : IndexNavVal[4].ToString(),
                            IndexAmountVal[4] == 0 ? "--" : IndexAmountVal[4].ToString(),
                            IndexNavVal[5] == 0 ? "--" : IndexNavVal[5].ToString(),
                            IndexAmountVal[5] == 0 ? "--" : IndexAmountVal[5].ToString(),
                            IndexNavVal[6] == 0 ? "--" : IndexNavVal[6].ToString(),
                            IndexAmountVal[6] == 0 ? "--" : IndexAmountVal[6].ToString(),
                            IndexNavVal[7] == 0 ? "--" : IndexNavVal[7].ToString(),
                            IndexAmountVal[7] == 0 ? "--" : IndexAmountVal[7].ToString(),
                            IndexNavVal[8] == 0 ? "--" : IndexNavVal[8].ToString(),
                            IndexAmountVal[8] == 0 ? "--" : IndexAmountVal[8].ToString(),
                            IndexNavVal[9] == 0 ? "--" : IndexNavVal[9].ToString(),
                            IndexAmountVal[9] == 0 ? "--" : IndexAmountVal[9].ToString(),
                            IndexNavVal[10] == 0 ? "--" : IndexNavVal[10].ToString(),
                            IndexAmountVal[10] == 0 ? "--" : IndexAmountVal[10].ToString());//this row represent Additional bench mark             
                    }
                    RptCommonSipResult.Visible = true;
                    RptCommonSipResult.DataSource = perfDataTable;
                    RptCommonSipResult.DataBind();
                    /* on the above the last row of data table come from sp is index and the 1st row is additionale benchmark that's why naming conversion change */
                }
            }
            catch (Exception exc)
            {
                //lblerrmsg.Text = exc.Message;
            }


            //return perfDataTable;
        }
        private DateTime GetLatestNavdate(DateTime LastQtrEndDate, string schmeId)
        {
            DateTime LastQtrEndDateInRecord = LastQtrEndDate;
            using (var principl = new PrincipalCalcDataContext())
            {
                var dateEnd = (from tnd in principl.T_NAV_DIVs
                               where ((tnd.Scheme_Id == Convert.ToDecimal(schmeId)) && (tnd.Nav_Date <= LastQtrEndDateInRecord))
                               orderby tnd.Nav_Date descending
                               select tnd.Nav_Date).Take(1);

                LastQtrEndDateInRecord = dateEnd.Count() > 0 ? Convert.ToDateTime(dateEnd.Single()) : LastQtrEndDate;

            }
            return LastQtrEndDateInRecord;
        }
        public System.Data.DataTable FetchScheme()
        {
            //conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            string selected_mode = "LUMP SUM"; //ddlMode.SelectedItem.Text;
            List<decimal?> excludeSubnatureList = new List<decimal?>();
            excludeSubnatureList.AddRange(new decimal?[] { 2, 21 });// FMP FTP ,Marginal Equity

            List<decimal?> excludeNatureIdList = new List<decimal?>();
            excludeNatureIdList.AddRange(new decimal?[] { 6 });// Liqidity
            //string[] adjustedNature = { "Dynamic/Asset Allocation", "Balanced" };
            List<decimal> ignorescheme = new List<decimal>();
            ignorescheme.AddRange(new decimal[]
            { 3077,
              16736,
              3145,
              16766,
              //11388, //HDFC Gold ETF
              //32122, //HDFC NIFTY 50 ETF
              //32121 //HDFC Sensex ETF
            });

            try
            {
                using (var scheme = new PrincipalCalcDataContext())
                {

                    if (ddlNature.SelectedIndex == 0)// Nature is not Selected
                    {

                        var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                         where
                                           t_fund_masters.MUTUALFUND_ID == 41
                                         select new
                                         {
                                             t_fund_masters.FUND_ID,
                                             t_fund_masters.NATURE_ID,
                                             t_fund_masters.SUB_NATURE_ID
                                         });

                        if (selected_mode.ToUpper().StartsWith("SIP"))
                        {

                            fundtable = from d in fundtable
                                        where !excludeSubnatureList.Contains(d.SUB_NATURE_ID)
                                        //&& !excludeNatureIdList.Contains(d.NATURE_ID)
                                        select d;
                        }
                        DataTable dtt = null;
                        if (fundtable.Count() > 0)
                            dtt = fundtable.ToDataTable();

                        var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                             join T in fundtable
                                             on s.Fund_Id equals T.FUND_ID
                                             where
                                             s.Nav_Check == 3 && s.Option_Id == 2 && !ignorescheme.Contains(s.Scheme_Id)
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
                        lstnatureSelected.Add(selected_nature);


                        var natur = from t_schemes_natures in scheme.T_SCHEMES_NATUREs
                                    where lstnatureSelected.Contains(t_schemes_natures.Nature)
                                    select t_schemes_natures.Nature_ID;

                        //foreach (string str in adjustedNature)
                        //{
                        //    if (selected_nature.Trim().ToUpper().Contains(str.Trim().ToUpper()))
                        //    {
                        //        flag = true;
                        //        break;
                        //    }
                        //}

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
                                             t_fund_masters.MUTUALFUND_ID == 41 &&
                                            natureList.Contains(t_fund_masters.NATURE_ID)
                                         select new
                                         {
                                             t_fund_masters.FUND_ID,
                                             t_fund_masters.NATURE_ID,
                                             t_fund_masters.SUB_NATURE_ID,
                                         });

                        if (selected_mode.ToUpper().StartsWith("SIP"))
                        {
                            var d = from ft in fundtable
                                        //where !excludeNatureIdList.Contains(ft.NATURE_ID)
                                    select ft;
                            fundtable = d;
                        }


                        DataTable dtt = new DataTable();
                        if (fundtable.Count() > 0)
                            dtt = fundtable.ToDataTable();

                        var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                             join T in fundtable
                                             on s.Fund_Id equals T.FUND_ID
                                             where s.Nav_Check == 3 && s.Option_Id == 2 && !ignorescheme.Contains(s.Scheme_Id)
                                             join tsi in scheme.T_SCHEMES_INDEXes
                                            on s.Scheme_Id equals tsi.SCHEME_ID
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
            return dt;
        }
        [WebMethod]
        public static string getCalculatedate(string schIndexid, DateTime startDate, DateTime endDate, string Amount, string NatureId)
        {
            double? daysDiffVal = null;
            DateTime _toDateLs, _fromDateLs;
            Double CompundReturnDayVal = 0;
            string strSchid = string.Empty;
            string strIndexid = string.Empty;
            SqlConnection conn1 = new SqlConnection();
            conn1.ConnectionString = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
            _fromDateLs = startDate;
            _toDateLs = endDate;
            double amountLs = Convert.ToDouble(Amount);
            string srollinprd = "D,0 Si";
            TimeSpan dateDiff2 = _toDateLs.Subtract(_fromDateLs);
            int day2 = dateDiff2.Days;
            srollinprd = day2.ToString() + " " + srollinprd;
            string daydiffCol = day2.ToString() + " " + "Day";
            int SettingId = 2;
            if (NatureId == "Debt" || NatureId == "Gilt" || NatureId == "Liquid")
            {
                SettingId = 2;//simple T_MFIE_RETURNSETTINGSET
            }
            else
            {
                SettingId = 2;  //absolute
            }
            try
            {
                using (var principl = new PrincipalCalcDataContext())
                {

                    //var scheme_id = from prin in principl.T_SCHEMES_MASTERs
                    //                where prin.Sch_Short_Name == ddlscname.SelectedItem.Text
                    //                select prin.Scheme_Id;
                    strSchid = schIndexid; //scheme_id.Single().ToString(); //scheme id                    
                                           //
                                           //var indx_id = from ppl in principl.T_INDEX_MASTERs
                                           //              where ppl.INDEX_NAME == ddlbnmark.SelectedItem.Text
                                           //              select ppl.INDEX_ID;
                                           //
                                           //strIndexid = indx_id.Single().ToString();
                                           //
                                           //
                                           //var alotdate = from ind in principl.T_SCHEMES_MASTERs
                                           //               where ind.Sch_Short_Name == Convert.ToString(ddlscname.SelectedItem.Text)
                                           //               select ind.Launch_Date;
                                           //
                                           //allotDate = Convert.ToDateTime(alotdate.Single().ToString());

                }


                try
                {
                    DataSet ds = new DataSet();
                    DataTable datatble = null;
                    //DataTable indexDataTable = null;
                    int val = 0;

                    SqlCommand cmd;

                    cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", conn1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SchemeIDs", strSchid));
                    cmd.Parameters.Add(new SqlParameter("@SettingSetID", SettingId));
                    cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                    cmd.Parameters.Add(new SqlParameter("@DateTo", _toDateLs.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                    cmd.Parameters.Add(new SqlParameter("@RoundTill", 4));
                    cmd.Parameters.Add(new SqlParameter("@RollingPeriodin", srollinprd));
                    cmd.Parameters.Add(new SqlParameter("@RollingPeriod", val));
                    cmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                    cmd.Parameters.Add(new SqlParameter("@RollingFrequency", val));
                    cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                    cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    datatble = ds.Tables[0];
                    if (Convert.ToString(datatble.Rows[0][daydiffCol]) != "N/A")
                        daysDiffVal = Convert.ToDouble(datatble.Rows[0][daydiffCol]);

                    if (daysDiffVal != null)
                    {
                        if (day2 > 365)
                            CompundReturnDayVal = amountLs * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day2 / 365, 4));
                        else
                            CompundReturnDayVal = amountLs + amountLs * (double)daysDiffVal / 100;
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }

            }
            catch (Exception exp)
            {
                return "";
            }

            return (Math.Round(CompundReturnDayVal, 2)).ToString();
        }


        [WebMethod]
        public static ChartNavReturnModel getChartData(string schIndexid, DateTime startDate, DateTime endDate, string Amount)
        {
            ChartNavReturnModel objChartNavReturnModel = null;
            var BenchmarkNAList = new List<decimal> { 3617, 4610 };
            Decimal dcmlIndexid = 0;

            //string schmeId = string.Empty;
            // schmeId = ddlscheme.SelectedItem.Value;
            decimal IndexId = AllMethods.GetIndexId(Convert.ToDecimal(schIndexid));

            DataTable _dtFinal = new DataTable();
            _dtFinal.Columns.Add("Scheme_Id", typeof(decimal));
            _dtFinal.Columns.Add("Sch_Short_Name", typeof(string));
            _dtFinal.Columns.Add("Nav_Date", typeof(DateTime));
            _dtFinal.Columns.Add("Nav", typeof(double));

            DataTable Final = new DataTable();

            try
            {

                #region GetSchmeIndex
                decimal AddbenchMarkId = 0;
                string nature = ""; string subnature = "";
                using (var principl = new PrincipalCalcDataContext())
                {
                    var sbNtrid = (from tfm in principl.T_FUND_MASTERs
                                   where tfm.FUND_ID ==
                                   (from tsm in principl.T_SCHEMES_MASTERs
                                    where tsm.Scheme_Id == Convert.ToDecimal(schIndexid.Trim())
                                    select new
                                    {
                                        tsm.Fund_Id
                                    }
                                   ).First().Fund_Id
                                   select new { tfm.T_SCHEMES_SUB_NATURE.Sub_Nature, tfm.T_SCHEMES_NATURE.Nature }).FirstOrDefault();//.First().SUB_NATURE_ID;

                    nature = sbNtrid.Nature;
                    subnature = sbNtrid.Sub_Nature;
                }


                //-----------Additional benchmark
                using (var principl = new PrincipalCalcDataContext())
                {
                    var FundId = principl.T_SCHEMES_MASTERs.Where(n => n.Scheme_Id == Convert.ToDecimal(schIndexid)).FirstOrDefault().Fund_Id;
                    var SchMasterRow = principl.T_Client_Additional_Benchmarks.Where(n => n.Fund_ID == FundId).FirstOrDefault();
                    if (SchMasterRow != null && SchMasterRow.Fund_ID.Value > 0)
                    {
                        if (!BenchmarkNAList.Contains(FundId.Value))
                        {
                            dcmlIndexid = SchMasterRow.Add_Bench_ID.Value;
                            AddbenchMarkId = dcmlIndexid;
                        }
                        else
                        {
                            if (nature == "Equity" || nature == "Balanced" || nature == "Dynamic/Asset Allocation")
                            {
                                if (Convert.ToInt32(IndexId) == 12 || Convert.ToInt32(IndexId) == 35)
                                    AddbenchMarkId = 142;
                                else
                                    AddbenchMarkId = 35;
                            }
                            else if (nature == "Liquid" || subnature == "Ultra Short Term" || subnature == "Floating Rate Fund" || subnature == "FMP" || subnature == "Short Term")
                            {
                                AddbenchMarkId = 135;
                            }
                            else
                            {
                                AddbenchMarkId = 134;
                            }
                        }
                    }
                    else
                    {
                        if (nature == "Equity" || nature == "Balanced" || nature == "Dynamic/Asset Allocation")
                        {
                            if (Convert.ToInt32(IndexId) == 12 || Convert.ToInt32(IndexId) == 35)
                                AddbenchMarkId = 142;
                            else
                                AddbenchMarkId = 35;
                        }
                        else if (nature == "Liquid" || subnature == "Ultra Short Term" || subnature == "Floating Rate Fund" || subnature == "FMP" || subnature == "Short Term")
                        {
                            AddbenchMarkId = 135;
                        }
                        else
                        {
                            AddbenchMarkId = 134;
                        }
                    }
                }
                //---------------------------------------------------------------------

                //if (nature == "Equity" || nature == "Balanced" || nature == "Dynamic/Asset Allocation")
                //{
                //    if (Convert.ToInt32(IndexId) == 12 || Convert.ToInt32(IndexId) == 35)
                //        AddbenchMarkId = 142;
                //    else
                //    {
                //        //Page page = (Page)HttpContext.Current.Handler;
                //        //HiddenField TextBox1 = (HiddenField)page.FindControl("AddlBenchmarkVal");
                //        //decimal ff = Convert.ToDecimal(TextBox1.Value);
                //        //AddbenchMarkId = ff;
                //        AddbenchMarkId = 132;
                //    }



                //}
                //else if (nature == "Liquid" || subnature == "Ultra Short Term" || subnature == "Floating Rate Fund" || subnature == "FMP" || subnature == "Short Term")
                //{
                //    AddbenchMarkId = 135;
                //}
                //else
                //{
                //    AddbenchMarkId = 134;
                //}
                //------------ end add bench mark

                Final.Columns.Add("Scheme_Id", typeof(int));
                Final.Columns.Add("Sch_Short_Name", typeof(string));
                Final.Columns.Add("Nav_Date", typeof(DateTime));
                Final.Columns.Add("Nav", typeof(decimal));
                Final.Columns.Add("InvestValue", typeof(decimal));
                Final.Columns.Add("InvestAmount", typeof(decimal));
                Final = AllMethods.HDFC_Lumpsum_Sip(Convert.ToDecimal(schIndexid), IndexId, AddbenchMarkId, startDate, endDate, Convert.ToDecimal(Amount.Trim()), "LumpSum");
                //-----------

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

            _dtFinal = RebaseDT(Final);



            var data = _dtFinal.AsEnumerable().Select(x => x);

            var names = data.Select(x => x.Field<string>("Sch_Short_Name")).Distinct();
            var investAmt = data.Where(x => x.Field<decimal>("Scheme_Id") == Convert.ToDecimal(schIndexid)).Select(x => x.Field<decimal>("InvestAmount")).Sum();
            var investvalue = data.Where(x => x.Field<decimal>("Scheme_Id") == Convert.ToDecimal(schIndexid)).Select(x => x.Field<decimal>("InvestValue")).Last();

            var returnData = names.Select(name => new SimpleNavReturnModel
            {
                Name = name,
                ValueAndDate =
                    data.Where(x => x.Field<string>("Sch_Short_Name") == name)
                        //.Select(x => new ValueAndDate { Date = x.Field<DateTime>("Nav_Date"), Value = x.Field<double>("ReInvestNav") })  //Nav
                        .Select(x => new ValueAndDate
                        {
                            Date = x.Field<DateTime>("Nav_Date").ToString("yyyy-MM-dd"),
                            Value = (double)x.Field<decimal>("InvestValue"),
                            InvestAmount = (double)Math.Round(x.Field<decimal>("InvestValue"), 2),
                            OrginalValue = (double)Math.Round(x.Field<decimal>("Nav"), 2)
                        })  //Nav

                        .ToList()
            }).ToList();


            objChartNavReturnModel = new ChartNavReturnModel
            {
                MaxDate = data.Select(x => x.Field<DateTime>("Nav_Date")).Max().ToString("MM/dd/yyyy"),
                MinDate = data.Select(x => x.Field<DateTime>("Nav_Date")).Min().ToString("MM/dd/yyyy"),
                MaxVal = data.Select(x => x.Field<decimal>("InvestValue")).Max(),
                MinVal = data.Select(x => x.Field<decimal>("InvestValue")).Min(),
                InvestedAmt = Math.Round(investAmt, 2),
                InvestedValue = Math.Round(investvalue, 2),
                SimpleNavReturnModel = returnData
            };
            return objChartNavReturnModel;
        }
        //public static DataTable RebaseDT(DataTable dt,double RebasedUnit)
        //{

        //    DataTable _dtrebase = dt.Copy();
        //    var preScheme = "";
        //    var firstNav = 0d;
        //    DataColumn dCol = new DataColumn("ReInvestNav", typeof(double));
        //    DataColumn dColAmt = new DataColumn("InvestAmount", typeof(double));
        //    _dtrebase.Columns.Add(dCol);
        //    _dtrebase.Columns.Add(dColAmt);
        //    DataTable data = _dtrebase.Copy();

        //    for (int i = 0; i < data.Rows.Count; i++)
        //    {
        //        data.Rows[i][4] = 0;

        //        if (data.Rows[i][1].ToString() != preScheme)
        //        {
        //            firstNav = Convert.ToDouble(data.Rows[i][3]);
        //            data.Rows[i][4] = 100;
        //            data.Rows[i][5] = RebasedUnit * Convert.ToDouble(data.Rows[i][4]);//Add new
        //            if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
        //                data.Rows[i][3] = "-1";
        //        }
        //        else
        //        {
        //            if (Convert.ToDouble(data.Rows[i - 1][3]) != 0)
        //            {
        //                data.Rows[i][4] = (100 * Convert.ToDouble(data.Rows[i][3])) / firstNav;
        //                data.Rows[i][5] = RebasedUnit * Convert.ToDouble(data.Rows[i][4]);//Add new
        //            }
        //            if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
        //                data.Rows[i][3] = "-1";
        //        }
        //        preScheme = data.Rows[i][1].ToString();
        //    }

        //    return data;
        //}
        public static DataTable RebaseDT(DataTable dt)
        {

            DataTable _dtrebase = dt.Copy();
            var preScheme = "";
            var firstNav = 0d;
            DataColumn dCol = new DataColumn("ReInvestNav", typeof(double));
            _dtrebase.Columns.Add(dCol);

            DataTable data = _dtrebase.Copy();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                data.Rows[i][6] = 0;
                if (data.Rows[i][1].ToString() != preScheme)
                {
                    firstNav = Convert.ToDouble(data.Rows[i][3]);
                    data.Rows[i][6] = 100;
                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                else
                {
                    if (Convert.ToDouble(data.Rows[i - 1][3]) != 0)
                        data.Rows[i][6] = (100 * Convert.ToDouble(data.Rows[i][3])) / firstNav;

                    if (Convert.ToDouble(data.Rows[i - 1][3]) == 0)
                    {
                        data.Rows[i - 1][3] = "-1";
                        // data.Rows[i-1][6] = 100;
                        // data.Rows[i][6] = 100;
                    }

                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";

                    if (Convert.ToDouble(data.Rows[i][6]) == 0)
                    {
                        data.Rows[i][6] = 100;
                    }

                }
                preScheme = data.Rows[i][1].ToString();
            }

            return data;
        }

        private bool ChkTempFund(string fundId)
        {
            bool flag = false;
            RptCommonSipResult.Visible = true;
            HighContainer.Visible = true;
            tab_default_2.Visible = true;
            // temporaryly change when data will come this code need to remove edited 05 Apr 2019
            string[] temparr = { "17519", "6392" };
            if (!temparr.Contains(fundId))
                flag = false;
            else
            {
                flag = true;
                tab_default_2.Visible = false;
                RptCommonSipResult.Visible = false;
                HighContainer.Visible = false;
            }
            return flag;
        }

        //SuperScript Function
        private string GetSuffix(string day)
        {
            string suffix = "th";

            if (int.Parse(day) < 11 || int.Parse(day) > 20)
            {
                day = day.ToCharArray()[day.ToCharArray().Length - 1].ToString();
                switch (day)
                {
                    case "1":
                        suffix = "st";
                        break;
                    case "2":
                        suffix = "nd";
                        break;
                    case "3":
                        suffix = "rd";
                        break;
                }
            }

            return suffix;
        }
    }
    // edited as per hdfc requirement
    public class SchemeByAmfi
    {
        public string AmfiCode { get; set; }
        public string SchemeId { get; set; }
    }

}