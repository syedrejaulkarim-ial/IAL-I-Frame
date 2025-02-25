using iFrames.BLL;
using iFrames.DAL;
using iFrames.Kotak;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.HDFC_SIP
{
    public partial class HDFCSipCalc : System.Web.UI.Page
    {
        string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        string SubNatureId = string.Empty;
        DateTime allotmentdate = new DateTime();
        protected void Page_Load(object sender, EventArgs e)
        {
            //edited as per hdfc requirement 09-01-2019
            var SchByAmfyLst = new List<SchemeByAmfi>();
            SchByAmfyLst.Add(new SchemeByAmfi { SchemeId = "37593", AmfiCode = "100119" });
            SchByAmfyLst.Add(new SchemeByAmfi { SchemeId = "37595", AmfiCode = "118968" });
            SchByAmfyLst.Add(new SchemeByAmfi { SchemeId = "37594", AmfiCode = "102948" });
            SchByAmfyLst.Add(new SchemeByAmfi { SchemeId = "37596", AmfiCode = "119062" });

            List<decimal> notForSip = new List<decimal>();
            notForSip.AddRange(new decimal[]
            {
                        11388, //HDFC Gold ETF
                        32122, //HDFC NIFTY 50 ETF
                        32121 //HDFC Sensex ETF
            });

            if (!IsPostBack)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["AmfiCode"]))
                    {
                        FillNature();
                        string AmfiCode = Request.QueryString["AmfiCode"].Trim();
                        var row = SchByAmfyLst.Where(x => x.AmfiCode == AmfiCode).FirstOrDefault();
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

                        if (notForSip.Contains(Convert.ToDecimal(SchemeId)))
                        {
                            Response.Redirect("/HDFC_SIP/HDFCLumpSumCalc.aspx?AmfiCode=" + AmfiCode, false);
                            Response.End();
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
                        txtiniAmount.Text = "500";
                        ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
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
                            allotmentdate = Convert.ToDateTime(allotdate.Single().LaunchDate);

                            //Code Changed By B.K.Rao Reason When Data passed by querystring the scheme date was selected scheme inception date and populate in from date textbox//
                            string schemeInceptionDate = allotmentdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            FromDate.Text = schemeInceptionDate.ToString();

                        }
                        decimal IndexId = AllMethods.GetIndexId(Convert.ToDecimal(SchemeId));
                        HdnbenchMarkId.Value = IndexId.ToString();
                        #endregion
                        if (!ChkTempFund(SchemeId))
                        PerformanceSipReturn(SchemeId);
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["NatureId"]) && !string.IsNullOrEmpty(Request.QueryString["SchemeId"]))
                    {
                        string SchemeId = Request.QueryString["SchemeId"].Trim();
                        if (Convert.ToInt32(SchemeId) != 0)
                        {
                            if (notForSip.Contains(Convert.ToDecimal(SchemeId)))
                            {
                                Response.Redirect("/HDFC_SIP/HDFCLumpSumCalc.aspx?SchemeId=" + SchemeId, false);
                                Response.End();
                            }

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
                            txtiniAmount.Text = "500";
                            ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
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
                                allotmentdate = Convert.ToDateTime(allotdate.Single().LaunchDate);

                            }
                            decimal IndexId = AllMethods.GetIndexId(Convert.ToDecimal(SchemeId));
                            HdnbenchMarkId.Value = IndexId.ToString();
                            //Code Changed By B.K.Rao Reason When Data passed by querystring the scheme date was selected scheme inception date and populate in from date textbox//
                            string schemeInceptionDate = allotmentdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            FromDate.Text = schemeInceptionDate.ToString();

                            //Added By B K Rao
                            if (ddlscheme.SelectedItem.ToString().ToUpper().Contains("DIR"))
                                txtPlan.InnerText = "Direct";
                            else
                                txtPlan.InnerText = "Regular";
                            #endregion
                            if (!ChkTempFund(SchemeId))
                            PerformanceSipReturn(SchemeId);
                        }
                        else
                        {
                            FillNature();
                            FillDropdownScheme();
                            txtiniAmount.Text = "500";
                            ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                            FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        FillNature();
                        FillDropdownScheme();
                        txtiniAmount.Text = "500";
                        ToDate.Text = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        FromDate.Text = DateTime.Today.AddDays(-1).AddYears(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    FillNature();
                    FillDropdownScheme();
                    txtiniAmount.Text = "500";
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
            if (dt.Rows.Count > 0)
            {
                ddlscheme.DataSource = dt;
                ddlscheme.DataTextField = "Sch_Short_Name";
                ddlscheme.DataValueField = "Scheme_Id";
                ddlscheme.DataBind();
                ddlscheme.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
            }
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
            // FillDropdownIndex();
            if (ddlscheme.SelectedIndex == 0 || ddlscheme.SelectedItem.Value == "--")
            {
                return;
            }
            else
            {
                //FillDropdown(ddlschtrto, ddlscheme.SelectedItem.Value);
            }
            string schmeId = string.Empty;
            SIPSchDt.Value = "";

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
                allotmentdate = Convert.ToDateTime(allotdate.Single().LaunchDate);

            }
            decimal IndexId = AllMethods.GetIndexId(Convert.ToDecimal(schmeId));
            HdnbenchMarkId.Value = IndexId.ToString();
            string AllocDate = allotmentdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            FromDate.Text = AllocDate.ToString();
            #endregion

            // PerformanceReturn(schmeId, HdnbenchMarkId.Value, Convert.ToDateTime(SIPSchDt.Value), 10000);    
            if (!ChkTempFund(schmeId))
                PerformanceSipReturn(schmeId);

            if (ddlscheme.SelectedItem.ToString().ToUpper().Contains("DIR"))
                txtPlan.InnerText = "Direct";
            else
                txtPlan.InnerText = "Regular";

        }
        public System.Data.DataTable FetchScheme()
        {
            //conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            string selected_mode = "SIP"; //ddlMode.SelectedItem.Text;
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
              11388,
              32122,
              32121
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
                                             t_fund_masters.SUB_NATURE_ID
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

        private void PerformanceSipReturn(string schemeId)
        {
            var BenchmarkNAList = new List<decimal> { 3617, 4610 };
            var intSch = Convert.ToInt32(schemeId);
            using (var principl = new PrincipalCalcDataContext())
            {
                var FundId = principl.T_SCHEMES_MASTERs.Where(n => n.Scheme_Id == Convert.ToDecimal(schemeId)).FirstOrDefault().Fund_Id;
                if (BenchmarkNAList.Contains(FundId.Value))
                {
                    hdnNABenchmark.Value = "1";
                }
                else
                {
                    hdnNABenchmark.Value = "0";
                }
            }

            using (var principl = new PrincipalCalcDataContext())
            {
                var DbRet = (from tfm in principl.RETURN_SIP_PRINCIPALs
                             where tfm.Scheme_ID == intSch
                             orderby tfm.Total_Amount_Invest
                             select tfm);
                if (DbRet.Any())
                {

                    var data = DbRet.ToDataTable();
                    RptCommonSipResult.DataSource = data;
                    RptCommonSipResult.DataBind();
                    RptCommonSipResult.Visible = true;
                    // CommonResultGridView.Visible = false;
                    ((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnSch")).Text = Convert.ToString(data.Rows[0]["Scheme_name"]);
                    ((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnIndex")).Text = Convert.ToString(data.Rows[0]["Index_Name"]);
                    ((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnAddIndex")).Text = Convert.ToString(data.Rows[0]["Additional_Index_Name"]);
                    //var dayinMonth = DateTime.DaysInMonth(DateTime.Today.AddMonths(-1).Year, DateTime.Today.AddMonths(-1).Month);
                    var lastdate = new DateTime(DateTime.Today.AddMonths(-1).Year, DateTime.Today.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Today.AddMonths(-1).Year, DateTime.Today.AddMonths(-1).Month));
                    lblReturnValueDate.Text = lastdate.ToString("dd MMM yyyy");
                }
            }

            //if (TableNote.Visible == false) TableNote.Visible = true;
        }

        [WebMethod]
        public static ChartNavReturnModel getChartData(string schIndexid, DateTime startDate, DateTime endDate, string Amount, string Frequency)// string startdate, string enddate, List<string> schList,List<string> indList
        {
            ChartNavReturnModel objChartNavReturnModel = null;

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
                    var FundId = (from tsm in principl.T_SCHEMES_MASTERs
                                  where tsm.Scheme_Id == Convert.ToDecimal(schIndexid.Trim())
                                  select new
                                  {
                                      tsm.Fund_Id
                                  }
                                   ).First().Fund_Id;

                    var sbNtrid = (from tfm in principl.T_FUND_MASTERs
                                   where tfm.FUND_ID == FundId
                                   select new { tfm.T_SCHEMES_SUB_NATURE.Sub_Nature, tfm.T_SCHEMES_NATURE.Nature }).FirstOrDefault();//.First().SUB_NATURE_ID;

                    var ChkAddIndex = principl.T_Client_Additional_Benchmarks.Where(x => x.Fund_ID == FundId).Select(v => v.Add_Bench_ID);

                    if (ChkAddIndex.Any())
                    {
                        AddbenchMarkId = (decimal)ChkAddIndex.FirstOrDefault();
                    }
                    else
                    {
                        if (nature == "Equity" || nature == "Balanced" || nature == "Dynamic/Asset Allocation")
                        {
                            if (Convert.ToInt32(IndexId) == 12)
                                AddbenchMarkId = 13;
                            else
                                AddbenchMarkId = 12;
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
                    nature = sbNtrid.Nature;
                    subnature = sbNtrid.Sub_Nature;
                }

                //Commonted by syed on 03 aug 2018 
                //if (nature == "Equity" || nature == "Balanced" || nature == "Dynamic/Asset Allocation")
                //{
                //    if (Convert.ToInt32(IndexId) == 12)
                //        AddbenchMarkId = 13;
                //    else
                //        AddbenchMarkId = 12;
                //}
                //else if (nature == "Liquid" || subnature == "Ultra Short Term" || subnature == "Floating Rate Fund" || subnature == "FMP" || subnature == "Short Term")
                //{
                //    AddbenchMarkId = 135;
                //}
                //else
                //{
                //    AddbenchMarkId = 134;
                //}


                Final.Columns.Add("Scheme_Id", typeof(int));
                Final.Columns.Add("Sch_Short_Name", typeof(string));
                Final.Columns.Add("Nav_Date", typeof(DateTime));
                Final.Columns.Add("Nav", typeof(decimal));
                Final.Columns.Add("InvestValue", typeof(decimal));
                Final.Columns.Add("InvestAmount", typeof(decimal));
                Final = AllMethods.HDFC_Lumpsum_Sip(Convert.ToDecimal(schIndexid), IndexId, AddbenchMarkId, startDate, endDate, Convert.ToDecimal(Amount.Trim()), Frequency);
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
                        // data.Rows[i][6] = 100;
                        // data.Rows[i - 1][6] = 100;
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
        public static DataTable RebaseDT(DataTable dt, double SipAmount, DateTime _fromDt, DateTime _toDt, string Frequency)
        {

            DataTable _dtrebase = dt.Copy();
            var preScheme = "";
            var firstNav = 0d;
            DataColumn dCol = new DataColumn("ReInvestNav", typeof(double));
            DataColumn dColAmt = new DataColumn("InvestValue", typeof(double));
            _dtrebase.Columns.Add(dCol);
            _dtrebase.Columns.Add(dColAmt);
            DataTable data = _dtrebase.Copy();
            var RebasedUnit = 0d;
            var MnthSipIncr = 0d;
            var SipAmountvar = 0d;
            DateTime PreNavdate;
            DateTime NextNavdate;
            DateTime St = new DateTime();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                data.Rows[i][4] = 0;

                if (data.Rows[i][1].ToString() != preScheme)
                {
                    St = _fromDt;
                    SipAmountvar = SipAmount;
                    RebasedUnit = 0d;
                    MnthSipIncr = 0d;
                    firstNav = Convert.ToDouble(data.Rows[i][3]);
                    data.Rows[i][4] = 100;
                    RebasedUnit = Math.Round((SipAmountvar / Convert.ToDouble(data.Rows[i][4])), 6);
                    data.Rows[i][5] = RebasedUnit * Convert.ToDouble(data.Rows[i][4]);//Add new

                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                else
                {
                    if (Convert.ToDouble(data.Rows[i - 1][3]) != 0)
                    {
                        PreNavdate = Convert.ToDateTime(data.Rows[i - 1][2]);
                        NextNavdate = Convert.ToDateTime(data.Rows[i][2]);
                        data.Rows[i][4] = (100 * Convert.ToDouble(data.Rows[i][3])) / firstNav;
                        //  if (PreNavdate.Month != NextNavdate.Month)
                        // {
                        DateTime Rt = new DateTime();
                        if (Frequency == "Daily")
                            Rt = St.AddDays(1);
                        else if (Frequency == "Weekly")
                            Rt = St.AddDays(7);
                        else if (Frequency == "Quarterly")
                            Rt = St.AddMonths(3);
                        else
                            Rt = St.AddMonths(1);
                        if (NextNavdate >= Rt)
                        {
                            St = Rt;
                            if (Rt <= _toDt)
                            {
                                SipAmountvar = SipAmount;
                                MnthSipIncr = MnthSipIncr + SipAmountvar;
                                //SipAmountvar = MnthSipIncr;
                                RebasedUnit = Math.Round((MnthSipIncr / Convert.ToDouble(data.Rows[i][4])), 6);
                            }
                        }
                        // }                       
                        data.Rows[i][5] = RebasedUnit * Convert.ToDouble(data.Rows[i][4]);//Add new
                        MnthSipIncr = Convert.ToDouble(data.Rows[i][5]);
                    }
                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                preScheme = data.Rows[i][1].ToString();
            }

            return data;
        }
        [WebMethod]
        public static string getCalculatedate(string schIndexid, DateTime startDate, DateTime endDate, string Amount, string Frequency)
        {
            DataTable SipDtable2;

            DateTime sipEndDate, sipAsonDate, investmentDate, schmInitialDate, LstMaxnavdate;
            string strsql = String.Empty, schmeId = string.Empty, indexId = string.Empty, Colstr = string.Empty, daydiffCol = string.Empty;
            string strFrequency, strInvestorType;



            double dblSIPamt;

            int initialFlag = 0;
            double dblSIPINtialAmnt = 0;
            schmInitialDate = new DateTime(1990, 01, 01);
            //conn.ConnectionString = connstr;
            investmentDate = startDate;
            sipEndDate = endDate;
            sipAsonDate = sipEndDate;
            dblSIPamt = Convert.ToDouble(Amount);
            strFrequency = Frequency; //"Monthly";
            strInvestorType = "Individual/Huf";
            schmeId = schIndexid;
            LstMaxnavdate = AllMethods.getMaxNavDate(Convert.ToDecimal(schmeId));
            if (LstMaxnavdate < sipEndDate)
            {
                sipEndDate = LstMaxnavdate;
                sipAsonDate = LstMaxnavdate;
            }
            using (var principl = new SIP_ClientDataContext() { CommandTimeout = 600000 })
            {

                IMultipleResults datatble = principl.MFIE_SP_SIP_CALCULATER_CLIENT(schmeId, investmentDate, sipEndDate, sipAsonDate, dblSIPamt, strFrequency, strInvestorType, initialFlag, dblSIPINtialAmnt, schmInitialDate, 1, "Y");
                var firstTable = datatble.GetResult<CalcReturnDataClient2>();
                var secondTable = datatble.GetResult<CalcReturnDataClient>();
                // SipDtable1 = firstTable.ToDataTable();
                SipDtable2 = secondTable.ToDataTable();
            }
            // SipDtable2 = AllMethods.HDFC_Lumpsum_Sip(Convert.ToInt32(schIndexid), Convert.ToInt32(IndexId), Convert.ToInt32(AddbenchMarkId), startDate, endDate, Convert.ToDecimal(Amount.Trim()), Frequency);
            if (SipDtable2.Rows.Count > 0)
            {
                var ValueInvs = SipDtable2.Rows[0]["TOTAL_AMOUNT"];
                var ValueProf = (Math.Round(Convert.ToDecimal(SipDtable2.Rows[0]["PRESENT_VALUE"]), 2)).ToString();
                return ValueInvs + "#" + ValueProf;
            }
            else
            {
                return " # ";
            }
        }


        private bool ChkTempFund(string fundId)
        {
            bool flag = false;
            HighContainer.Visible = true;
            tab_default_2.Visible = true;
            // temporaryly change when data will come this code need to remove edited 05 Apr 2019
            string[] temparr = { "17519", "6392" };
            if (!temparr.Contains(fundId))
                flag = false;
            else
            {
                flag = true;
                HighContainer.Visible = false;
                tab_default_2.Visible = false;
            }
            return flag;
        }

    }


    public class SchemeIndexList
    {
        public List<decimal> ListScheme { get; set; }
        public List<decimal> ListIndex { get; set; }
        public List<decimal> ListAddBenchMark { get; set; }

        public SchemeIndexList()
        {
            ListScheme = new List<decimal>();
            ListIndex = new List<decimal>();
            ListAddBenchMark = new List<decimal>();
        }

    }
    public class ValueAndDate
    {
        public string Date { get; set; }
        public double Value { get; set; }
        public double OrginalValue { get; set; }
        public double InvestAmount { get; set; }
    }
    public class SimpleNavReturnModel
    {
        public string Name { get; set; }
        public decimal Id { get; set; }
        public bool IsIndex { get; set; }
        public IList<ValueAndDate> ValueAndDate { get; set; }
        public string MaxDate { get; set; }
        public string MinDate { get; set; }
    }
    public class ChartNavReturnModel
    {
        public IList<SimpleNavReturnModel> SimpleNavReturnModel { get; set; }
        public string MaxDate { get; set; }
        public string MinDate { get; set; }
        public decimal MaxVal { get; set; }
        public decimal MinVal { get; set; }
        public decimal InvestedAmt { get; set; }
        public decimal InvestedValue { get; set; }
    }

}