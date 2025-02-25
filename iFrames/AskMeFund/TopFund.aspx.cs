using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.DAL;
using iFrames.BLL;
using iFrames.Pages;
using System.Linq.Dynamic;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;
using iFrames.Classes;

namespace iFrames.AskMeFund
{

    public partial class TopFund : System.Web.UI.Page
    {
        #region GlobalVar
        readonly string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        readonly SqlConnection conn = new SqlConnection();

        DataTable dtResultTopFund = new DataTable();
        static ICacheRepository CacheRepo = null;
        #endregion

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

            HostAuthority.Value = HttpContext.Current.Request.Url.Authority;
            //PageValue.Value = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("page");
            PageValue.Value = Request.QueryString["page"];


            if (!IsPostBack)
            {
                CacheRepo = CachingRepo.GetCacheRepository().MemoryCacheRepo;

                //For Top Fund Page
                LoadNavDetails();
                

                LoadNature();
                LoadAllSubNature();
                LoadStructure();
                loadOption();
                PopulateFundRiskButtons();
                //PageLoadDataBind();
                //LoadSubNatureTopFundListTab();
                HiddenFundRiskStrColor.Value = "all";
                HiddenFundRisk.Value = "-1";

                //For Comapre Fund Page
                getFundHouseCompare();
                getIndicesNameCompare();
                LoadNatureCompare();
                LoadAllSubNatureCompare();
                LoadStructureCompare();
                loadOptionCompare();
                //FirstAddScheme.Visible = false; 
                //SecondAddScheme.Visible = false;
                //ThirdAddScheme.Visible = false;
                //FourthAddScheme.Visible = false;

                //For Nav Track Fund Page
                getFundHouseNav();
                getIndicesNameNav();
                LoadNatureNav();
                LoadAllSubNatureNav();
                //LoadStructureNav();
                loadOptionNav();
            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            // DataBind();
            base.OnPreRender(e);
        }

        #endregion


        #region Event

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{

        //    ResultDataBind();

        //    //var dp = lstResult.FindControl("dpTopFund") as DataPager;
        //    //if (dp != null)
        //    //{
        //    //    dp.SetPageProperties(0, dp.MaximumRows, true);
        //    //}
        //    //lstResult.Visible = true;
        //    //Result.Visible = true;
        //}

        //protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    var dp = (sender as ListView).FindControl("dpTopFund") as DataPager;
        //    if (dp != null)
        //    {
        //        dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        //        //DataBind();
        //        if (hdIsLoad.Value == "1")
        //            ResultDataBind();
        //        else
        //            PageLoadDataBind();
        //    }
        //}

        #endregion


        #region Load Data
        [WebMethod]
        public static string LoadSubNatureTopFundListTab(string SebiNatureId)
        {
            string json="";
            DataTable dtEqity = new DataTable();
            dtEqity.Columns.Add("_Sebi_Sub_Nature");
            dtEqity.Columns.Add("Sebi_Sub_Nature");

            dtEqity.Rows.Add("Multi Cap Fund", "Multi Cap");
            dtEqity.Rows.Add("Flexi Cap Fund", "Flexicap");
            dtEqity.Rows.Add("Large Cap Fund", "Large Cap");
            dtEqity.Rows.Add("Large & Mid Cap Fund", "Large & MidCap");
            dtEqity.Rows.Add("Mid Cap Fund", "Mid Cap");
            dtEqity.Rows.Add("Small cap Fund", "Small Cap");
            dtEqity.Rows.Add("ELSS", "ELSS");
            dtEqity.Rows.Add("Dividend Yield Fund", "Div. Yield");
            dtEqity.Rows.Add("Focused Fund", "Focused");
            dtEqity.Rows.Add("Sectoral", "Sectoral");
            dtEqity.Rows.Add("Contra Fund", "Contra");
            dtEqity.Rows.Add("Thematic", "Thematic ");
            dtEqity.Rows.Add("Value Fund", "Value");
            dtEqity.Rows.Add("Others", "Others");

            DataTable DtDebt = new DataTable();
            DtDebt.Columns.Add("_Sebi_Sub_Nature");
            DtDebt.Columns.Add("Sebi_Sub_Nature");

            DtDebt.Rows.Add("Long Duration Fund", "Long Duration");
            DtDebt.Rows.Add("Medium to Long Duration Fund", "Medium to Long Dur.");
            DtDebt.Rows.Add("Medium Duration Fund", "Medium Dur.");
            DtDebt.Rows.Add("Low Duration Fund", "Low Duration");
            DtDebt.Rows.Add("Short Duration Fund", "Short Duration");
            DtDebt.Rows.Add("Ultra Short Duration Fund", "Ultra Short Duration");
            DtDebt.Rows.Add("Money Market Fund", "Money Market Fund");
            DtDebt.Rows.Add("Liquid Fund", "Liquid Fund");
            DtDebt.Rows.Add("Overnight Fund", "Overnight Fund");
            DtDebt.Rows.Add("Banking and PSU Fund", "Banking and PSU");
            DtDebt.Rows.Add("Corporate Bond Fund", "Corporate Bond ");
            DtDebt.Rows.Add("Credit Risk Fund", "Credit Risk");
            DtDebt.Rows.Add("Dynamic Bond", "Dynamic Bond");
            DtDebt.Rows.Add("Floater Fund", "Floater");
            DtDebt.Rows.Add("Gilt Fund", "Gilt");
            DtDebt.Rows.Add("Gilt Fund with 10 year constant duration", "Gilt (10 Yrs Constant Duration)");
            DtDebt.Rows.Add("FMP", "FMP");
            DtDebt.Rows.Add("Others", "Others");

            DataTable DtHybrid = new DataTable();
            DtHybrid.Columns.Add("_Sebi_Sub_Nature");
            DtHybrid.Columns.Add("Sebi_Sub_Nature");

            DtHybrid.Rows.Add("Aggressive Hybrid Fund", "Aggressive Hybrid");
            DtHybrid.Rows.Add("Conservative Hybrid Fund", "Conservative Hybrid");
            DtHybrid.Rows.Add("Dynamic Asset Allocation or Balanced Advantage", "Dynamic Asset Allocation");
            DtHybrid.Rows.Add("Multi Asset Allocation", "Multi Asset Alocation");
            DtHybrid.Rows.Add("Equity Savings", "Equity Savings");
            DtHybrid.Rows.Add("Arbitrage Fund", "Arbitrage Fund");
            DtHybrid.Rows.Add("Capital Protection funds", "Capital Protection Fund");
            DtHybrid.Rows.Add("Others", "Other");


            if (SebiNatureId == "4")
            {
                DataTable _dt = AllMethods.getSebiSubNature(Convert.ToInt32(SebiNatureId));

                var joinREs = (from t in dtEqity.AsEnumerable()
                               join p in _dt.AsEnumerable()
                               on t.Field<string>("_Sebi_Sub_Nature") equals p.Field<string>("Sebi_Sub_Nature")
                               select new
                               {
                                   Sebi_Sub_Nature = t.Field<string>("Sebi_Sub_Nature"),
                                   Sebi_Sub_Nature_ID = p.Field<decimal>("Sebi_Sub_Nature_ID"),
                                   //Sebi_Sub_Nature = p.Field<string>("Sebi_Sub_Nature")

                               }).ToDataTable();

                json = JsonConvert.SerializeObject(joinREs, Formatting.Indented);

                
            }
            else if (SebiNatureId == "3")
            {

                DataTable _dt = AllMethods.getSebiSubNature(Convert.ToInt32(SebiNatureId));

                var joinREs = (from t in DtDebt.AsEnumerable()
                               join p in _dt.AsEnumerable()
                               on t.Field<string>("_Sebi_Sub_Nature") equals p.Field<string>("Sebi_Sub_Nature")
                               select new
                               {
                                   Sebi_Sub_Nature = t.Field<string>("Sebi_Sub_Nature"),
                                   Sebi_Sub_Nature_ID = p.Field<decimal>("Sebi_Sub_Nature_ID"),
                                   //Sebi_Sub_Nature = p.Field<string>("Sebi_Sub_Nature")

                               }).ToDataTable();

                json = JsonConvert.SerializeObject(joinREs, Formatting.Indented);
            }
            else if (SebiNatureId == "1")
            {
                DataTable _dt = AllMethods.getSebiSubNature(Convert.ToInt32(SebiNatureId));

                var joinREs = (from t in DtHybrid.AsEnumerable()
                               join p in _dt.AsEnumerable()
                               on t.Field<string>("_Sebi_Sub_Nature") equals p.Field<string>("Sebi_Sub_Nature")
                               select new
                               {
                                   Sebi_Sub_Nature = t.Field<string>("Sebi_Sub_Nature"),
                                   Sebi_Sub_Nature_ID = p.Field<decimal>("Sebi_Sub_Nature_ID"),
                                   //Sebi_Sub_Nature = p.Field<string>("Sebi_Sub_Nature")

                               }).ToDataTable();

                json = JsonConvert.SerializeObject(joinREs, Formatting.Indented);
            }
            else if (SebiNatureId == "6")
            {
                DataTable _dt = AllMethods.getSebiSubNature(Convert.ToInt32(SebiNatureId));
                json = JsonConvert.SerializeObject(_dt, Formatting.Indented);
            }
            else if (SebiNatureId == "5")
            {
                DataTable _dt = AllMethods.getSebiSubNature(Convert.ToInt32(SebiNatureId));
                DataTable _dt1 = AllMethods.getSebiSubNature(Convert.ToInt32(6));

                foreach (DataRow dr in _dt1.Rows)
                {
                    _dt.Rows.Add(dr.ItemArray);
                }

                json = JsonConvert.SerializeObject(_dt, Formatting.Indented);
            }



            return json;
        }

        [WebMethod]
        public static string getTopFundData(int SebiSubNatureId, int SebiNatureId)
        {
            DataTable dtMain = new DataTable();
            DataTable dtResult = AllMethods.getSebiTopFundRank(15, -1, SebiNatureId, "Per_1_Year", SebiSubNatureId, 2, -1, 500, -1, -1);
            dtMain = dtResult.Clone();
            foreach (DataRow drRes in dtResult.Rows)
            {
                dtMain.Rows.Add(drRes.ItemArray);
            }

            DataTable tblFinal = new DataTable();

            tblFinal = GetData(dtMain);

            string JSONresult;
            JSONresult = Newtonsoft.Json.JsonConvert.SerializeObject(tblFinal);

            return JSONresult;
        }


        public static DataTable GetData(DataTable dt)
        {
            DataTable _Cdt = new DataTable();
            //DataTable _Cdt = (DataTable)HttpContext.Current.Cache["InvestNavInfo"];

            if (CacheRepo.Exists("InvestNavInfo"))
                _Cdt = CacheRepo.Get("InvestNavInfo").GetData<DataTable>();

            DataTable dtFinal = new DataTable();

            dtFinal = (from p in dt.AsEnumerable()
                         join t in _Cdt.AsEnumerable()
                         on Convert.ToInt32(p["SchemeId"]) equals Convert.ToInt32(t["SCHEME_ID"])
                         where !string.IsNullOrEmpty(Convert.ToString(p["SchemeId"])) && !string.IsNullOrEmpty(Convert.ToString(t["SCHEME_ID"]))
                         select new
                         {
                             Scheme_Id = Convert.ToDecimal(p["SchemeId"]),
                             Sch_Name = Convert.ToString(p["Sch_Name"]),
                             Per_182_Days = string.IsNullOrEmpty(Convert.ToString(p["Per_182_Days"])) ? "--" : Convert.ToDecimal(p["Per_182_Days"]).ToString("N"),
                             Per_1_Year = string.IsNullOrEmpty(Convert.ToString(p["Per_1_Year"])) ? "--" : Convert.ToDecimal(p["Per_1_Year"]).ToString("N"),
                             Per_3_Year = string.IsNullOrEmpty(Convert.ToString(p["Per_3_Year"])) ? "--" : Convert.ToDecimal(p["Per_3_Year"]).ToString("N"),
                             Since_Inception = Convert.ToDecimal(p["Since_Inception"]).ToString("N"),
                             Fund_Size = string.IsNullOrEmpty(Convert.ToString(p["Fund_Size"])) ? 0 : Math.Round( Convert.ToDecimal(p["Fund_Size"]), 2),
                             Curr_Nav = string.IsNullOrEmpty(Convert.ToString(t["LATEST_NAV"])) ? "--" : Convert.ToDecimal(t["LATEST_NAV"]).ToString("N"),
                             Prev_Nav = string.IsNullOrEmpty(Convert.ToString(t["PREVIOUS_NAV"])) ? "--" : Convert.ToDecimal(t["PREVIOUS_NAV"]).ToString("N"),
                             Incr_Nav = (Math.Round(Convert.ToDouble(t["LATEST_NAV"]) - Convert.ToDouble(t["PREVIOUS_NAV"]), 2)).ToString() + " (" +
                                                 (Math.Round(100 * (Convert.ToDouble(t["LATEST_NAV"]) - Convert.ToDouble(t["PREVIOUS_NAV"])) / Convert.ToDouble(t["PREVIOUS_NAV"]), 2)).ToString() + "%)",
                             LblHigh = string.IsNullOrEmpty(Convert.ToString(t["MAX_NAV"])) ? "--" : Convert.ToDecimal(t["MAX_NAV"]).ToString("N"),
                             LblLow = string.IsNullOrEmpty(Convert.ToString(t["MIN_NAV"])) ? "--" : Convert.ToDecimal(t["MIN_NAV"]).ToString("N"),
                             SpnMaxDate = string.IsNullOrEmpty(Convert.ToString(t["MAX_NAV_DATE"])) ? "--" : Convert.ToDateTime(t["MAX_NAV_DATE"]).ToString("dd/MM/yyyy"),
                             spnMinDate = string.IsNullOrEmpty(Convert.ToString(t["MIN_NAV_DATE"])) ? "--" : Convert.ToDateTime(t["MIN_NAV_DATE"]).ToString("dd/MM/yyyy")

                         }).ToDataTable();

                       

            return dtFinal;

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

        protected void LoadNavDetails()
        {
            DataTable dt;
            if (!CacheRepo.Exists("InvestNavInfo"))
            {
                #region memorycache
                dt = AllMethods.getCurrentNavInfo();
                CacheRepo.Add(new Cacheable(dt, "InvestNavInfo"));
                //var dtfd = CacheRepo.Get("InvestNavInfo").GetData<DataTable>();
                //if (CacheRepo.Exists("InvestNavInfo"))
                //{
                //    // CacheRepo.Remove()
                //}
                #endregion


                //dt = AllMethods.getCurrentNavInfo();
                //Cache.Add("InvestNavInfo", dt, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }

        }



        //getAllSubNature

        protected void LoadAllSubNature()
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

        [WebMethod]
        public static string LoadSubNature(string ddlCategory)
        {
            int id = Convert.ToInt32(ddlCategory);
            DataTable _dt = new DataTable();


            if (id == -1)
            {
               _dt = AllMethods.getAllSebiSubNature();
            }
            else
            {
                _dt = AllMethods.getSebiSubNature(id);
            }
            

            //if (_dt != null)
            //    foreach (DataRow drow in _dt.Rows)
            //    {
            //        ddlSubCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            //    }

            //ddlSubCategory.Items[0].Selected = true;
            string json = JsonConvert.SerializeObject(_dt, Formatting.Indented);

            return json;
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

        //[WebMethod]
        //public static string GetTopFund(string Fund)
        //{
        //    DataTable dt = new DataTable();
        //    dt = TopFundData();

        //    string json = JsonConvert.SerializeObject(dt, Formatting.Indented);

        //    return json;
        //}


        //public static DataTable TopFundData()
        //{
        //    DataTable dtResult = new DataTable();

        //    dtResult = AllMethods.getSebiTopFundRank(-1, -1, -1, "Per_1_Year", -1, 2, -1, 500, 5);
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

        //        dtResult.Columns.Add("CategoryAverage", typeof(decimal));

        //        decimal x = 3.07m;
        //        foreach (DataRow row in dtResult.Rows)
        //        {
        //            row["CategoryAverage"] = x;
        //            x++;
        //        }

        //        dtResult.Columns.Add("CurrentNav");
        //        dtResult.Columns.Add("LblIncrNav");
        //        dtResult.Columns.Add("PrevNav");
        //        dtResult.Columns.Add("LblHigh");
        //        dtResult.Columns.Add("LblLow");
        //        dtResult.Columns.Add("SpnMaxDate");
        //        dtResult.Columns.Add("spnMinDate");
        //        foreach (DataRow row in dtResult.Rows)
        //        {
        //            var SchId = Convert.ToInt32(row["SchemeId"]);
        //            var _investmentInfo = AllMethods.getInvestInfo(SchId);
        //            row["CurrentNav"] = _investmentInfo.CurrentNav;
        //            row["PrevNav"] = _investmentInfo.PrevNav;
        //            double num;
        //            if (double.TryParse(_investmentInfo.CurrentNav.ToString().Trim(), out num) && double.TryParse(_investmentInfo.PrevNav.ToString().Trim(), out num))
        //            {
        //                double curNav = Convert.ToDouble(_investmentInfo.CurrentNav);
        //                double PrevNav = Convert.ToDouble(_investmentInfo.PrevNav);
        //                row["LblIncrNav"] = (Math.Round(curNav - PrevNav, 4)).ToString() + " (" +
        //                                (Math.Round(100 * (curNav - PrevNav) / PrevNav, 2)).ToString() + "%)";
        //            }

        //            var rows = AllMethods.GetHighLowNav(SchId);
        //            if (rows != null && rows.Any())
        //            {
        //                row["LblHigh"] = rows["max_nav"];
        //                row["LblLow"] = rows["min_nav"];
        //                row["spnMinDate"] = rows["min_nav_date"];
        //                row["SpnMaxDate"] = rows["max_nav_date"];
        //            }

        //        }
        //    };
        //    return dtResult;
        //}
            
           

        [WebMethod]
        public static ChartNavReturnModel getChartData(string schIndexid, DateTime startDate, DateTime endDate)// string startdate, string enddate, List<string> schList,List<string> indList
        {
            ChartNavReturnModel objChartNavReturnModel = null;
            List<string> schList = new List<string>();
            List<string> indList = new List<string>();
            List<decimal> sortList = new List<decimal>();

            if (!string.IsNullOrEmpty(schIndexid))
            {
                var strArray = schIndexid.Split('#').ToList();

                foreach (string str in strArray)
                {
                    if (str.Trim().ToUpper().StartsWith("S"))
                    {
                        schList.Add(str.Trim().ToUpper().TrimStart('S'));
                        sortList.Add(Convert.ToDecimal(str.Trim().ToUpper().TrimStart('S')));
                    }

                    if (str.Trim().ToUpper().StartsWith("I"))
                    {
                        indList.Add(str.Trim().ToUpper().TrimStart('I'));
                        sortList.Add(Convert.ToDecimal(str.Trim().ToUpper().TrimStart('I')));
                    }

                }
            }



            DataTable _dtFinal = new DataTable();
            _dtFinal.Columns.Add("Scheme_Id", typeof(decimal));
            _dtFinal.Columns.Add("Sch_Short_Name", typeof(string));
            _dtFinal.Columns.Add("Nav_Date", typeof(DateTime));
            _dtFinal.Columns.Add("Nav", typeof(double));


            try
            {

                #region GetSchmeIndex

                SchemeIndexList objSchInd = new SchemeIndexList();
                objSchInd.ListScheme = schList.Select(x => Convert.ToDecimal(x)).ToList();
                objSchInd.ListIndex = indList.Select(x => Convert.ToDecimal(x)).ToList();

                DataTable _dtSch = new DataTable();
                DataTable _dtInd = new DataTable();

                DateTime mindate = AllMethods.getSchmindate(AllMethods.getFundSchemeIdStr(objSchInd.ListScheme));


                if (objSchInd.ListScheme.Count() > 0)
                {
                    _dtSch = AllMethods.getHistNavDetails(AllMethods.getFundSchemeIdStr(objSchInd.ListScheme), mindate, System.DateTime.Now);
                    if (_dtSch != null && _dtSch.Rows.Count > 0)
                    {
                        _dtFinal = _dtSch.Copy();
                    }
                }

                _dtFinal.Columns.Add("isIndex", typeof(string));     // added by suman

                if (objSchInd.ListIndex.Count() > 0)
                {
                    _dtInd = AllMethods.getHistIndexRecordDetails(AllMethods.getFundSchemeIdStr(objSchInd.ListIndex), mindate, System.DateTime.Now);
                    if (_dtInd != null && _dtInd.Rows.Count > 0)
                    {
                        _dtInd.Columns.Add("isIndex", typeof(string));
                        foreach (DataRow row in _dtInd.Rows)
                            row["isIndex"] = -1;

                        if (_dtFinal != null && _dtFinal.Rows.Count == 0)
                        {
                            // var dtble = _dtInd.AsEnumerable().Select(x => x).ToDataTable();
                            foreach (DataRow dr in _dtInd.Rows)
                                _dtFinal.Rows.Add(dr.ItemArray);
                        }
                        else
                            _dtInd.AsEnumerable().CopyToDataTable(_dtFinal, LoadOption.OverwriteChanges);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }




            IEnumerable<DataRow> _testdtIndex = from sl in sortList.AsEnumerable()
                                                join tst in _dtFinal.AsEnumerable()
                                                on sl.ToString() equals tst.Field<decimal>("Scheme_Id").ToString()
                                                select tst;
            DataTable _dttt = new DataTable("tblSort");
            _dttt = _testdtIndex.CopyToDataTable<DataRow>();
            _dtFinal = _dttt.Copy();

            _dtFinal = RebaseDT(_dtFinal);


            var data = _dtFinal.AsEnumerable().Select(x => x);

            var names = data.Select(x => x.Field<string>("Sch_Short_Name")).Distinct();

            //#region ReBase Algo
            //var data2 = _dtFinal.AsEnumerable().Cast<NavReturnModelSP>().ToList();//.OrderBy(x => x.Scheme_Name).ThenBy(x => x.Date).ToList();



            //#endregion
            //ObjWebIner.EventDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")

            var returnData = names.Select(name => new SimpleNavReturnModel
            {
                Name = name,
                ValueAndDate =
                    data.Where(x => x.Field<string>("Sch_Short_Name") == name)
                        //.Select(x => new ValueAndDate { Date = x.Field<DateTime>("Nav_Date"), Value = x.Field<double>("ReInvestNav") })  //Nav
                        .Select(x => new ValueAndDate
                        {
                            Date = x.Field<DateTime>("Nav_Date").ToString("yyyy-MM-dd"),
                            Value = x.Field<double>("ReInvestNav"),

                            OrginalValue = Math.Round(x.Field<double>("Nav"), 2),
                            IsIndex = x.Field<string>("IsIndex")
                        })  //Nav

                        .ToList()
            }).ToList();



            objChartNavReturnModel = new ChartNavReturnModel
            {
                MaxDate = data.Select(x => x.Field<DateTime>("Nav_Date")).Max().ToString("MM/dd/yyyy"),
                MinDate = data.Select(x => x.Field<DateTime>("Nav_Date")).Min().ToString("MM/dd/yyyy"),
                MaxVal = data.Select(x => x.Field<double>("ReInvestNav")).Max(),
                MinVal = data.Select(x => x.Field<double>("ReInvestNav")).Min(),
                SimpleNavReturnModel = returnData
            };
            //return _dtFinal;
            //return "sucess";
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
                data.Rows[i][5] = 0;

                if (data.Rows[i][1].ToString() != preScheme)
                {
                    firstNav = Convert.ToDouble(data.Rows[i][3]);
                    data.Rows[i][5] = 100;
                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                else
                {
                    if (Convert.ToDouble(data.Rows[i - 1][3]) != 0)
                        data.Rows[i][5] = (100 * Convert.ToDouble(data.Rows[i][3])) / firstNav;
                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                preScheme = data.Rows[i][1].ToString();
            }

            return data;
        }


        #region Fetch Data


        [WebMethod]
        public static string GetSearchResult(string ddlCategory, string ddlType, string ddlSubCategory, string rdbOption, string ddlPeriod, string HiddenFundRisk, string HiddenMinimumInvesment, string HiddenMinimumSIReturn)
        {
            //hdIsLoad.Value = "1";
            DataTable dtResult = new DataTable();             
            List<int> RiskList = new List<int>();
            DataTable _Cdt = new DataTable();

            RiskList = HiddenFundRisk?.Split(',')?.Select(Int32.Parse)?.ToList();


            dtResult = AllMethods.getSebiTopFundRank1(-1, Convert.ToInt32(ddlType),
                Convert.ToInt32(ddlCategory), ddlPeriod, Convert.ToInt32(ddlSubCategory),
                Convert.ToInt32(rdbOption), RiskList, Convert.ToInt32(HiddenMinimumInvesment), Convert.ToInt32(HiddenMinimumSIReturn));


            //DataTable _Cdt = (DataTable)HttpContext.Current.Cache["InvestNavInfo"];
            if (CacheRepo.Exists("InvestNavInfo"))
                _Cdt = CacheRepo.Get("InvestNavInfo").GetData<DataTable>();

            DataTable dtFinal = new DataTable();

            dtFinal = (from p in dtResult.AsEnumerable()
                       join t in _Cdt.AsEnumerable()
                       on Convert.ToInt32(p["SchemeId"]) equals Convert.ToInt32(t["SCHEME_ID"])
                       where !string.IsNullOrEmpty(Convert.ToString(p["SchemeId"])) && !string.IsNullOrEmpty(Convert.ToString(t["SCHEME_ID"]))
                       select new
                       {
                           Scheme_Id = Convert.ToDecimal(p["SchemeId"]),
                           Sch_Name = Convert.ToString(p["Sch_Name"]),
                           Per_7_Days = string.IsNullOrEmpty(Convert.ToString(p["Per_7_Days"])) ? "--" : Convert.ToDecimal(p["Per_7_Days"]).ToString("N"),
                           Per_30_Days = string.IsNullOrEmpty(Convert.ToString(p["Per_30_Days"])) ? "--" : Convert.ToDecimal(p["Per_30_Days"]).ToString("N"),
                           Per_91_Days = string.IsNullOrEmpty(Convert.ToString(p["Per_91_Days"])) ? "--" : Convert.ToDecimal(p["Per_91_Days"]).ToString("N"),
                           Per_182_Days = string.IsNullOrEmpty(Convert.ToString(p["Per_182_Days"])) ? "--" : Convert.ToDecimal(p["Per_182_Days"]).ToString("N"),
                           Per_1_Year = string.IsNullOrEmpty(Convert.ToString(p["Per_1_Year"])) ? "--" : Convert.ToDecimal(p["Per_1_Year"]).ToString("N"),
                           Per_3_Year = string.IsNullOrEmpty(Convert.ToString(p["Per_3_Year"])) ? "--" : Convert.ToDecimal(p["Per_3_Year"]).ToString("N"),
                           Since_Inception = Convert.ToDecimal(p["Since_Inception"]).ToString("N"),
                           Fund_Size = string.IsNullOrEmpty(Convert.ToString(p["Fund_Size"])) ? 0 : Math.Round( Convert.ToDecimal(p["Fund_Size"]), 2),
                           Curr_Nav = string.IsNullOrEmpty(Convert.ToString(t["LATEST_NAV"])) ? "--" : Convert.ToDecimal(t["LATEST_NAV"]).ToString("N"),
                           Prev_Nav = string.IsNullOrEmpty(Convert.ToString(t["PREVIOUS_NAV"])) ? "--" : Convert.ToDecimal(t["PREVIOUS_NAV"]).ToString("N"),
                           Incr_Nav = (Math.Round(Convert.ToDouble(t["LATEST_NAV"]) - Convert.ToDouble(t["PREVIOUS_NAV"]), 2)).ToString() + " (" +
                                               (Math.Round(100 * (Convert.ToDouble(t["LATEST_NAV"]) - Convert.ToDouble(t["PREVIOUS_NAV"])) / Convert.ToDouble(t["PREVIOUS_NAV"]), 2)).ToString() + "%)",
                           LblHigh = string.IsNullOrEmpty(Convert.ToString(t["MAX_NAV"])) ? "--" : Convert.ToDecimal(t["MAX_NAV"]).ToString("N"),
                           LblLow = string.IsNullOrEmpty(Convert.ToString(t["MIN_NAV"])) ? "--" :  Convert.ToDecimal( t["MIN_NAV"]).ToString("N"),
                           SpnMaxDate = string.IsNullOrEmpty(Convert.ToString(t["MAX_NAV_DATE"])) ? "--" : Convert.ToDateTime(t["MAX_NAV_DATE"]).ToString("dd/MM/yyyy"),
                           spnMinDate = string.IsNullOrEmpty(Convert.ToString(t["MIN_NAV_DATE"])) ? "--" : Convert.ToDateTime(t["MIN_NAV_DATE"]).ToString("dd/MM/yyyy")
                       }).ToDataTable();

                       

            string json = JsonConvert.SerializeObject(dtFinal, Formatting.Indented);

            return json;
        }
        #endregion

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

        

        //------------------------------------------ Compare Fund Block ----------------------------------------------------
        #region Compare Fund Block
        private void getFundHouseCompare()
        {
            DataTable dtFund;

            dtFund = AllMethods.getFundHouse();
            DataRow drFund = dtFund.NewRow();
            drFund["MutualFund_Name"] = "- Select Mutual Fund -";
            drFund["MutualFund_ID"] = 0;
            dtFund.Rows.InsertAt(drFund, 0);
            //Cache.Add("dtFund", dtFund, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);

            ddlFundHouseCompare.DataSource = dtFund;
            ddlFundHouseCompare.DataTextField = "MutualFund_Name";
            ddlFundHouseCompare.DataValueField = "MutualFund_ID";
            ddlFundHouseCompare.DataBind();
        }
        protected void LoadNatureCompare()
        {

            DataTable _dt = AllMethods.getSebiNature();
            ddlCategoryCompare.Items.Clear();
            ddlCategoryCompare.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlCategoryCompare.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlCategoryCompare.Items[0].Selected = true;
        }

        protected void LoadAllSubNatureCompare()
        {
            DataTable _dt = AllMethods.getAllSebiSubNature();
            ddlSubCategoryCompare.Items.Clear();
            ddlSubCategoryCompare.Items.Add(new ListItem("All", "-1"));
            ddlSubCategoryCompare.Items.Add(new ListItem("Large Cap", "9000"));
            ddlSubCategoryCompare.Items.Add(new ListItem("Mid & Small Cap", "9001"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlSubCategoryCompare.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }
            ddlSubCategoryCompare.Items[0].Selected = true;
        }

        [WebMethod]
        public static string LoadSubNatureCompare(string ddlCategoryCompare)
        {
            var id = Convert.ToInt32(ddlCategoryCompare);
            //ddlSubCategoryCompare.ClearSelection();
            if (id == -1)
            {
                //ddlSubCategoryCompare.Items[0].Selected = true;
                return " ";
            }
            DataTable _dt = AllMethods.getSebiSubNature(id);

            string json = JsonConvert.SerializeObject(_dt, Formatting.Indented);

            return json;
        }

        public string SetHyperlinkFundDetail(string schemeId, string SchemeName)
        {
            string resultStr = string.Empty;
            //if (schemeId != "-1")
            //    resultStr = " <a href='FundDetails.aspx?id=" + schemeId + "'><asp:Label ID='lblSchName' runat='server' Text='" + SchemeName + "'/></a>";
            //else
            //    resultStr = "<asp:Label ID='lblSchName' runat='server' Text='" + SchemeName + "'/>";

            if (schemeId != "-1")
                resultStr = " <a href='http://www.askmefund.com/factsheet.aspx?param=" + schemeId + "' id='urlID' target='_blank'>" + SchemeName + "</a>";
            else
                resultStr = SchemeName;

            return resultStr;
        }


        protected void LoadStructureCompare()
        {
            DataTable _dt = AllMethods.getStructure();
            ddlTypeCompare.Items.Clear();
            ddlTypeCompare.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlTypeCompare.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlTypeCompare.Items[0].Selected = true;
        }

        protected void loadOptionCompare()
        {
            var OptionDataTable = AllMethods.getOption();

            //DataRow drIndex = dtIndex.NewRow();
            //drIndex["INDEX_NAME"] = "Select";
            //drIndex["INDEX_ID"] = 0;
            //dtIndex.Rows.InsertAt(drIndex, 0);

            ddlOptionCompare.DataSource = OptionDataTable;
            ddlOptionCompare.DataTextField = "Name";
            ddlOptionCompare.DataValueField = "Id";
            ddlOptionCompare.DataBind();
            ddlOptionCompare.SelectedIndex = 0;
        }

        private void getIndicesNameCompare()
        {
            DataTable dtIndex = AllMethods.getIndices();
            DataRow drIndex = dtIndex.NewRow();
            drIndex["INDEX_NAME"] = "Select";
            drIndex["INDEX_ID"] = 0;
            dtIndex.Rows.InsertAt(drIndex, 0);
            ddlIndicesCompare.DataSource = dtIndex;
            ddlIndicesCompare.DataTextField = "INDEX_NAME";
            ddlIndicesCompare.DataValueField = "INDEX_ID";
            ddlIndicesCompare.DataBind();
        }

        protected void ddlCategory_SelectedIndexChangedCompare(object sender, EventArgs e)
        {
            //if (ddlCategoryCompare.SelectedIndex == 0) return;
            //if (ddlFundHouseCompare.SelectedIndex != 0)
                //getSchemesListCompare();

            //int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            //LoadSubNature(nature_id);
        }
        protected void ddlType_SelectedIndexChangedCompare(object sender, EventArgs e)
        {
            //if (ddlFundHouseCompare.SelectedIndex != 0)
                //getSchemesListCompare();
        }

 
        protected void ddlSubCategory_SelectedIndexChangedCompare(object sender, EventArgs e)
        {
            //if (ddlFundHouseCompare.SelectedIndex != 0)
                //getSchemesListCompare();
        }
        protected void ddlMutualFund_SelectedIndexChangedCompare(object sender, EventArgs e)
        {
            //if (ddlFundHouseCompare.SelectedIndex != 0)
                //getSchemesListCompare();
        }

        [WebMethod]
        public static string getSchemesListCompare(string ddlCategoryCompare, string ddlSubCategoryCompare, string rdbOptionCompare, string ddlFundHouseCompare)
        {
            
            DataTable dtScheme = new DataTable();

            dtScheme = AllMethods.getSebiScheme(Convert.ToInt32(ddlCategoryCompare),
               -1, Convert.ToInt32(ddlSubCategoryCompare),
              Convert.ToInt32(rdbOptionCompare), Convert.ToInt32(ddlFundHouseCompare), false);

            string json = JsonConvert.SerializeObject(dtScheme, Formatting.Indented);

            return json;

        }

        protected void GrdCompFund_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdCompFund_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
               
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ImgArrow = e.Row.FindControl("ImgArrowCompare") as Label;
                var CurrNav = (e.Row.FindControl("CurrNavCompare") as HiddenField).Value;
                var PrevNav = (e.Row.FindControl("PrevNavCompare") as HiddenField).Value;

                double num;
                if (double.TryParse(CurrNav.Trim(), out num) && double.TryParse(PrevNav.Trim(), out num))
                {
                    double curNav = Convert.ToDouble(CurrNav);
                    double PrevNav1 = Convert.ToDouble(PrevNav);

                    if (curNav - PrevNav1 >= 0)
                    {

                        ImgArrow.CssClass = "mdi-arrow-up-bold mdi text-success";
                    }
                    else
                    {
                        ImgArrow.CssClass = "mdi-arrow-down-bold mdi text-success";
                    }
                }

            }
            
            

        }

        [WebMethod]
        public static string PopulateSchemeIndexCompareFund(string CurrSchemeId1, string CurrSchemeId2, string CurrSchemeId3, string CurrSchemeId4, string CurrIndexId1, string CurrIndexId2, string CurrIndexId3, string CurrIndexId4)
        {
            DataTable dt = new DataTable();
            string jsonResut;
            try
            {
                #region GetSchmeIndex
                //DivShowPerformance.Visible = true;
                SchemeIndexList ojSchemeIndexList = new SchemeIndexList();

                if (!string.IsNullOrEmpty(CurrSchemeId1))
                {
                    ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(CurrSchemeId1));
                    ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(CurrIndexId1));
                }

                if (!string.IsNullOrEmpty(CurrSchemeId2))
                {
                    ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(CurrSchemeId2));
                    ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(CurrIndexId2));
                }

                if (!string.IsNullOrEmpty(CurrSchemeId3))
                {
                    ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(CurrSchemeId3));
                    ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(CurrIndexId3));
                }


                if (!string.IsNullOrEmpty(CurrSchemeId4))
                {
                    ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(CurrSchemeId4));
                    ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(CurrIndexId4));
                }


                dt = GetGridData(ojSchemeIndexList);
                jsonResut = JsonConvert.SerializeObject(dt, Formatting.Indented);

                
                //GrdCompFund.DataBind();
                //lbRetrnMsg.Visible = true;
                //lblSortPeriod.Visible = true;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonResut;
        }

        public static DataTable GetGridData(SchemeIndexList ojSchemeIndexList)
        {
            List<string> SelectList = getSelectedItem(ojSchemeIndexList);

            //HdSchemes.Value = string.Join("#", SelectList[0].Split(',').Select(x => "s" + x.ToString())) + "#" +
            //                  string.Join("#", SelectList[1].Split(',').Select(x => "i" + x.ToString()));

            //HdSchemes.Value = HdSchemes.Value.TrimEnd('i');
            //HdSchemes.Value = HdSchemes.Value.TrimEnd('s');
            //HdSchemes.Value = HdSchemes.Value.TrimEnd('#');


            //HdToData.Value = DateTime.Today.ToString("dd MMM yyyy");
            //HdFromData.Value = DateTime.Today.AddYears(-3).ToString("dd MMM yyyy");

            DataTable _dtFinal = GetData(SelectList[0], SelectList[1]);
            return _dtFinal;
        }

        public static DataTable GetData(string strSch, string strIndex)
        {
            DataTable _dtSch = new DataTable();
            DataTable _dtInd = new DataTable();
            DataTable _dtFinal = new DataTable();

            _dtFinal.Columns.Add("Sch_id");
            _dtFinal.Columns.Add("Sch_Short_Name");
            _dtFinal.Columns.Add("Per_30_Days");
            _dtFinal.Columns.Add("Per_91_Days");
            _dtFinal.Columns.Add("Per_182_Days");
            _dtFinal.Columns.Add("Per_1_Year");
            _dtFinal.Columns.Add("Per_3_Year");
            _dtFinal.Columns.Add("Per_Since_Inception");
            _dtFinal.Columns.Add("Nav_Rs");
            _dtFinal.Columns.Add("Nature");
            _dtFinal.Columns.Add("Sub_Nature");
            _dtFinal.Columns.Add("Option_Id");
            _dtFinal.Columns.Add("Structure_Name");
            _dtFinal.Columns.Add("Fund_Size");
            _dtFinal.Columns.Add("status");
            _dtFinal.Columns.Add("CurrentNav");
            _dtFinal.Columns.Add("LblIncrNav");
            _dtFinal.Columns.Add("PrevNav");
            



            if (strSch != string.Empty)
                _dtSch = AllMethods.getSebiFundComparisonWithSI1(strSch);

            if (_dtFinal != null)
            {
                for (int i = 0; i < _dtSch.Rows.Count; i++)
                {
                    _dtFinal.Rows.Add(_dtSch.Rows[i].ItemArray);
                }
            }

            //List<decimal> lstSchemeId = new List<decimal>();
            //if (!string.IsNullOrEmpty(strSch))
            //{
            //    lstSchemeId = AllMethods.getFundSchemeId(strSch);
            //}

            //int x = 0;
            //foreach (DataRow row in _dtFinal.Rows)
            //{
            //    var SchId = Convert.ToInt32(lstSchemeId[x]);
            //    var _investmentInfo = AllMethods.getInvestInfo(SchId);
            //    row["CurrentNav"] = _investmentInfo.CurrentNav;
            //    row["PrevNav"] = _investmentInfo.PrevNav;
            //    double num;
            //    if (double.TryParse(_investmentInfo.CurrentNav.ToString().Trim(), out num) && double.TryParse(_investmentInfo.PrevNav.ToString().Trim(), out num))
            //    {
            //        double curNav = Convert.ToDouble(_investmentInfo.CurrentNav);
            //        double PrevNav = Convert.ToDouble(_investmentInfo.PrevNav);
            //        row["LblIncrNav"] = (Math.Round(curNav - PrevNav, 4)).ToString() + " (" +
            //                        (Math.Round(100 * (curNav - PrevNav) / PrevNav, 2)).ToString() + "%)";
            //    }

            //    x++;

            //}

            _dtInd = CalculateIndexHistPerf(DateTime.Today.AddDays(-1), strIndex);


            if (_dtInd != null && _dtInd.Rows.Count > 0)
            {
                _dtInd.Columns.Add("Nav_Rs");
                _dtInd.Columns.Add("Nature");
                _dtInd.Columns.Add("Sub_Nature");
                _dtInd.Columns.Add("Option_Id");
                _dtInd.Columns.Add("Structure_Name");
                _dtInd.Columns.Add("Fund_Size");
                _dtInd.Columns.Add("status");

                if (_dtFinal != null)
                {
                    for (int i = 0; i < _dtInd.Rows.Count; i++)
                    {
                        _dtInd.Rows[i]["status"] = "3";
                        _dtInd.Rows[i][0] = "-1";// for disablinnf link
                        _dtFinal.Rows.Add(_dtInd.Rows[i].ItemArray);
                    }
                }
            }

            return _dtFinal;
        }

        public static DataTable CalculateIndexHistPerf(DateTime EndDate, string indexId)
        {
            #region :Historical Performance

             string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
             SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connstr;
            string strRollingPeriodin = "30 D,91 D,182 D,1 YYYY,3 YYYY,0 Si";
            int val = 0;

            # region calling sp


            DataTable dtIndexAbsolute = new DataTable();

            SqlCommand cmd;
            SqlDataAdapter da = new SqlDataAdapter();

            cmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 2000;
            cmd.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
            cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
            cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
            cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
            cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

            da.SelectCommand = cmd;
            da.Fill(dtIndexAbsolute);

            #endregion



            if (dtIndexAbsolute != null && dtIndexAbsolute.Rows.Count > 0)
            {
                //if (dtIndexAbsolute.Columns.Contains("INDEX_ID"))
                //    dtIndexAbsolute.Columns.Remove("INDEX_ID");
                if (dtIndexAbsolute.Columns.Contains("INDEX_TYPE"))
                    dtIndexAbsolute.Columns.Remove("INDEX_TYPE");
            }

            return dtIndexAbsolute;

            #endregion
        }

        public static List<string> getSelectedItem(SchemeIndexList ojSchemeIndexList)
        {
            List<string> itemList = new List<string>();
            SchemeIndexList objSchInd = ojSchemeIndexList;
            StringBuilder _objSbSch = new StringBuilder();
            StringBuilder _objSbInd = new StringBuilder();

            if (objSchInd.ListScheme.Count() > 0)
            {
                _objSbSch.Append(string.Join(",", objSchInd.ListScheme.Select(k => k.ToString(CultureInfo.InstalledUICulture)).ToArray()));
            }

            if (objSchInd.ListIndex.Count() > 0)
            {
                _objSbInd.Append(string.Join(",", objSchInd.ListIndex.Select(k => k.ToString(CultureInfo.InvariantCulture)).ToList()));
            }

            if (_objSbSch.ToString() != string.Empty)
                itemList.Add(_objSbSch.ToString());
            else
                itemList.Add(string.Empty);

            if (_objSbInd.ToString() != string.Empty)
                itemList.Add(_objSbInd.ToString());
            else
                itemList.Add(string.Empty);


            return itemList;
        }

        

        #endregion

        /////////////////////////////////////  Nav Fund Block   //////////////////////////////////////////////////////////////////
        #region Nav Fund Block

        private void getFundHouseNav()
        {
            DataTable dtFund;
            //if (Cache["dtFund"] == null)
            //{
            dtFund = AllMethods.getFundHouse();
            DataRow drFund = dtFund.NewRow();
            drFund["MutualFund_Name"] = "Select";
            drFund["MutualFund_ID"] = 0;
            dtFund.Rows.InsertAt(drFund, 0);
                //Cache.Add("dtFund", dtFund, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            //}
            //else
            //{
            //    dtFund = (DataTable)Cache["dtFund"];
            //}
            ddlFundHouseNav.DataSource = dtFund;
            ddlFundHouseNav.DataTextField = "MutualFund_Name";
            ddlFundHouseNav.DataValueField = "MutualFund_ID";
            ddlFundHouseNav.DataBind();
        }

        private void getIndicesNameNav()
        {
            DataTable dtIndex = AllMethods.getIndices();
            DataRow drIndex = dtIndex.NewRow();
            drIndex["INDEX_NAME"] = "Select";
            drIndex["INDEX_ID"] = 0;
            dtIndex.Rows.InsertAt(drIndex, 0);
            ddlIndicesNav.DataSource = dtIndex;
            ddlIndicesNav.DataTextField = "INDEX_NAME";
            ddlIndicesNav.DataValueField = "INDEX_ID";
            ddlIndicesNav.DataBind();
        }
        
        protected void LoadNatureNav()
        {
            DataTable _dt = AllMethods.getSebiNature();
            ddlCategoryNav.Items.Clear();
            ddlCategoryNav.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlCategoryNav.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlCategoryNav.Items[0].Selected = true;
        }

        protected void LoadAllSubNatureNav()
        {
            DataTable _dt = AllMethods.getAllSebiSubNature();
            ddlSubCategoryNav.Items.Clear();
            ddlSubCategoryNav.Items.Add(new ListItem("All", "-1"));
            ddlSubCategoryNav.Items.Add(new ListItem("Large Cap", "9000"));
            ddlSubCategoryNav.Items.Add(new ListItem("Mid & Small Cap", "9001"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlSubCategoryNav.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlSubCategoryNav.Items[0].Selected = true;
        }

        protected void LoadStructureNav()
        {
            //DataTable _dt = AllMethods.getStructure();
            //ddlTypeNav.Items.Clear();
            //ddlTypeNav.Items.Add(new ListItem("All", "-1"));
            //foreach (DataRow drow in _dt.Rows)
            //{
            //    ddlTypeNav.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            //}

            //ddlTypeNav.Items[0].Selected = true;
        }

        protected void loadOptionNav()
        {
            var OptionDataTable = AllMethods.getOption();
            ddlOptionNav.DataSource = OptionDataTable;
            ddlOptionNav.DataTextField = "Name";
            ddlOptionNav.DataValueField = "Id";
            ddlOptionNav.DataBind();
            ddlOptionNav.SelectedIndex = 0;
        }

        protected void ddlMutualFund_SelectedIndexChangedNav(object sender, EventArgs e)
        {
            //getSchemesListNav();
        }

        protected void ddlCategory_SelectedIndexChangedNav(object sender, EventArgs e)
        {
            //int nature_id = Convert.ToInt32(ddlCategoryNav.SelectedValue);
            //LoadSubNatureNav(nature_id);
            //getSchemesListNav();
        }

        protected void ddlSubCategory_SelectedIndexChangedNav(object sender, EventArgs e)
        {
            //getSchemesListNav();
        }

        protected void ddlType_SelectedIndexChangedNav(object sender, EventArgs e)
        {
            //getSchemesListNav();
        }

        protected void rdbOption_SelectedIndexChangedNav(object sender, EventArgs e)
        {
            //getSchemesListNav();
        }


        //private void getSchemesListNav()
        //{
        //    ddlSchemesNav.ClearSelection();
        //    DataTable dtScheme = AllMethods.getSebiScheme(Convert.ToInt32(ddlCategoryNav.SelectedItem.Value),
        //        Convert.ToInt32(ddlTypeNav.SelectedItem.Value), Convert.ToInt32(ddlSubCategoryNav.SelectedValue),
        //        Convert.ToInt32(rdbOptionNav.SelectedValue), Convert.ToInt32(ddlFundHouseNav.SelectedValue), false);

        //    ddlSchemesNav.DataSource = dtScheme;
        //    ddlSchemesNav.DataTextField = "Sch_Short_Name";
        //    ddlSchemesNav.DataValueField = "Scheme_Id";
        //    ddlSchemesNav.DataBind();
        //}

        protected void LoadSubNatureNav(int id)
        {
            ddlSubCategoryNav.ClearSelection();
            if (id == -1)
            {
                ddlSubCategoryNav.Items[0].Selected = true;
                return;
            }
            DataTable _dt = AllMethods.getSebiSubNature(id);
            ddlSubCategoryNav.Items.Clear();
            ddlSubCategoryNav.Items.Add(new ListItem("All", "-1"));
            if (_dt != null)
                foreach (DataRow drow in _dt.Rows)
                {
                    ddlSubCategoryNav.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }

            ddlSubCategoryNav.Items[0].Selected = true;
        }

        protected void _btnAddSchemeNav_Click(object sender, EventArgs e)
        {
            //DivGridContain.Visible = true;
            //DivPlotChart.Visible = true;

            //AddScheme();
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        [WebMethod]
        public static string btnAddSchemeNav(string SchName, string SchId, string IndName, string IndId)
        {
            
            DataTable dt = new DataTable();
            try
            {
                if (SchName != "")
                {
                    //DataTable dt = objNavGraph.FetchIndicesAgainstScheme(ddlSchemes.SelectedItem.Value.Trim());
                    
                    dt.Columns.Add("SCHEME_ID", typeof(System.String));
                    dt.Columns.Add("Sch_Short_Name", typeof(System.String));
                    dt.Columns.Add("INDEX_ID", typeof(System.String));
                    dt.Columns.Add("INDEX_NAME", typeof(System.String));

                    DataView dv = AllMethods.getIndicesAgainstScheme(SchId).DefaultView;
                    DataTable dtScheme = dv.ToTable(true, "SCHEME_ID", "Sch_Short_Name");
                    DataTable dtIndex = dv.ToTable(true, "INDEX_ID", "INDEX_NAME");

                    string schcode = string.Empty;
                    string schname = string.Empty;
                    schcode = dtScheme.Rows[0]["SCHEME_ID"].ToString();
                    schname = dtScheme.Rows[0]["Sch_Short_Name"].ToString();
                    foreach (DataRow drInex in dtIndex.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["SCHEME_ID"] = schcode;
                        dr["Sch_Short_Name"] = schname;
                        dr["INDEX_ID"] = drInex["INDEX_ID"];
                        dr["INDEX_NAME"] = drInex["INDEX_NAME"];
                        dt.Rows.Add(dr);
                        schcode = string.Empty;
                        schname = string.Empty;
                    }

                    if(IndId != "0")
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1["SCHEME_ID"] = string.Empty;
                        dr1["Sch_Short_Name"] = string.Empty;
                        dr1["INDEX_ID"] = IndId;
                        dr1["INDEX_NAME"] = IndName;
                        dt.Rows.Add(dr1);
                    }
                       
                }

            }
            catch (Exception ex)
            {

            }
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }







        #endregion


    }
}