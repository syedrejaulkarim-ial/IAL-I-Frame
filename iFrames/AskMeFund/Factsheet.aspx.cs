using iFrames.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace iFrames.AskMeFund
{
    public partial class Factsheet : System.Web.UI.Page
    {
        string RankingDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                {
                    hdSchemeId.Value = Request.QueryString["param"];
                    ResultFactsheet.Visible = true;
                    setInvestmentInfo(Convert.ToInt32(Request.QueryString["param"]));
                    TopCompanys(Convert.ToInt32(Request.QueryString["param"]));
                    PeerSchemes(Convert.ToInt32(Request.QueryString["param"]));
                    TopSector(Convert.ToInt32(Request.QueryString["param"]));
                    PreCalculatedRatio(Convert.ToInt32(Request.QueryString["param"]));
                    if (Convert.ToInt32(Request.QueryString["param"]) > 0)
                    {
                        var rows = AllMethods.GetHighLowNavAskMeFund(Convert.ToInt32(Request.QueryString["param"]));
                        if (rows != null && rows.Any())
                        {
                            lblHigh.InnerText = rows["max_nav"];
                            lblLow.InnerText = rows["min_nav"];
                            spnMinDate.InnerText = rows["min_nav_date"];
                            spnMaxDate.InnerText = rows["max_nav_date"];
                        }
                    }
                }
                else
                {
                    ResultFactsheet.Visible = false;
                }
            }
        }

        private void setInvestmentInfo(int schemeId)
        {
            var _investmentInfo = AllMethods.getInvestInfo(schemeId);
            lblFundName.InnerText = _investmentInfo.FundName;
            lblPresentNav.InnerText = _investmentInfo.CurrentNav.ToString();
            double num;
            if (double.TryParse(_investmentInfo.CurrentNav.ToString().Trim(), out num) && double.TryParse(_investmentInfo.PrevNav.ToString().Trim(), out num))
            {
                //if (double.TryParse(_investmentInfo.PrevNav, out num))
                //{
                double curNav = Convert.ToDouble(_investmentInfo.CurrentNav);
                double PrevNav = Convert.ToDouble(_investmentInfo.PrevNav);
                lblIncrNav.InnerText = (Math.Round(curNav - PrevNav, 4)).ToString() + " (" +
                                (Math.Round(100 * (curNav - PrevNav) / PrevNav, 2)).ToString() + "%)";
                ImgArrow.Visible = true;
                if (curNav - PrevNav >= 0)
                {
                    ImgArrow.Attributes.Add("class", "mdi-arrow-up-bold mdi text-success");
                }
                else
                {
                    ImgArrow.Attributes.Add("class", "mdi-arrow-down-bold mdi text-success");
                }
            }
            else
            {
                ImgArrow.Visible = false;
                lblIncrNav.InnerText = "";
            }

            
            tdFundName.InnerText = _investmentInfo.FundName; 
            //tdAssetSize.InnerText = Convert.ToString(_investmentInfo.FundSize);
            tdAssetSize.InnerText = Convert.ToString(Convert.ToDouble(_investmentInfo.FundSize).ToString("#,#.##", CultureInfo.CreateSpecificCulture("hi-IN")));
            tdStructure.InnerText = _investmentInfo.StructureName;
            tdMinInvest.InnerText = _investmentInfo.MinInvestment;
            tdLunchDate.InnerText = _investmentInfo.LunchDate.HasValue ? _investmentInfo.LunchDate.Value.ToString("dd MMM yyyy") : "";
            lblCurrNavDate.InnerText = _investmentInfo.LunchDate.HasValue ? _investmentInfo.CurrentNavDate.Value.ToString("dd MMM yyyy") : "";
            tdFundMan.InnerText = _investmentInfo.FundMan;
            //tdLastDiv.InnerText = _investmentInfo.LatestDiv;
            tdBenchMark.InnerText = _investmentInfo.BenchMark;
            //tdEmail.InnerText = string.IsNullOrEmpty(_investmentInfo.Website) ? _investmentInfo.Email : _investmentInfo.Website + "/" + _investmentInfo.Email;
            //tdBonus.InnerText = _investmentInfo.Bonous;
            //tdAmcName.InnerText = _investmentInfo.AmcName;
            tdEntryLoad.InnerText = _investmentInfo.EntryLoad;
            tdExitLoad.InnerText = _investmentInfo.ExitLoad;
        }

        public void PeerSchemes(int schemeId)
        {

            int peerSchCnt = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ValueInvestPeerSchemeCount"]);
            DataSet ds = AllMethods.getSebiPeerPerformance(schemeId, peerSchCnt);
            DataTable _PeerSchemes = ds.Tables[0];
            RankingDate = Convert.ToString(ds.Tables[1].Rows[0][1]);
            var Data = _PeerSchemes.AsEnumerable().Where(x => Convert.ToInt32(x["SchemeId"]) == schemeId).Select(x => x).FirstOrDefault();

            spn1mth.InnerText = string.IsNullOrEmpty(Data["Per30Days"].ToString()) ? "--" : Data["Per30Days"].ToString();
            //Label lblprgbar1 = this.FindControl("spn1mthPrg") as Label;
            //lblprgbar1.Attributes.Add("style", "width:" + Data["Per30Days"] + "%");

            spn3mth.InnerText = string.IsNullOrEmpty(Data["Per91Days"].ToString()) ? "--" : Data["Per91Days"].ToString();
            //Label lblprgbar2 = this.FindControl("spn3mthPrg") as Label;
            //lblprgbar2.Attributes.Add("style", "width:" + Data["Per91Days"] + "%");

            spn6mth.InnerText = string.IsNullOrEmpty(Data["Per182Days"].ToString()) ? "--" : Data["Per182Days"].ToString();
            //Label lblprgbar3 = this.FindControl("spn6mthPrg") as Label;
            //lblprgbar3.Attributes.Add("style", "width:" + Data["Per182Days"] + "%");

            spn1yr.InnerText = string.IsNullOrEmpty(Data["Per1Year"].ToString()) ? "--" : Data["Per1Year"].ToString();
            //Label lblprgbar4 = this.FindControl("spn1yrPrg") as Label;
            //lblprgbar4.Attributes.Add("style", "width:" + Data["Per1Year"] + "%");

            spn3yr.InnerText = string.IsNullOrEmpty(Data["Per3Year"].ToString()) ? "--" : Data["Per3Year"].ToString();
            //Label lblprgbar5 = this.FindControl("spn3yrPrg") as Label;
            //lblprgbar5.Attributes.Add("style", "width:" + Data["Per3Year"] + "%");

            spnSinceInception.InnerText = string.IsNullOrEmpty(Data["SI"].ToString()) ? "--" : Data["SI"].ToString();
            //Label lblprgbar6 = this.FindControl("spnSinceInceptionPrg") as Label;
            //lblprgbar6.Attributes.Add("style", "width:" + Data["SI"] + "%");
        }

        private void PreCalculatedRatio(int SchemeId)
        {
            var _data = AllMethods.getPreCalculatedRatio(SchemeId);
            if (_data != null)
            {
                lblSharpe.InnerText = Convert.ToString(Math.Round(Convert.ToDouble(_data.Sharp), 2));
                lblSortino.InnerText = Convert.ToString(Math.Round(Convert.ToDouble(_data.Sortino), 2));
                lblSdv.InnerText = Convert.ToString(Math.Round(Convert.ToDouble(_data.STDV), 2));
                lblBeta.InnerText = Convert.ToString(Math.Round(Convert.ToDouble(_data.Beta), 2));
                lblRSqure.InnerText = Convert.ToString(Math.Round(Convert.ToDouble(_data.RSQR), 2));
            }
            else
            {
                lblSharpe.InnerText = "--";
                lblSortino.InnerText = "--";
                lblSdv.InnerText = "--";
                lblBeta.InnerText = "--";
                lblRSqure.InnerText = "--";
            }
        }

        [WebMethod]
        public static ChartNavReturnModel getChartData(string schIndexid, DateTime startDate, DateTime endDate)
        {
            ChartNavReturnModel objChartNavReturnModel = null;
            List<string> schList = new List<string>();
            List<string> indList = new List<string>();
            List<decimal> sortList = new List<decimal>();

            if (!string.IsNullOrEmpty(schIndexid))
            {
                var Ind = AllMethods.GetIndexId(Convert.ToInt32(schIndexid));
                schList.Add(schIndexid);
                sortList.Add(Convert.ToInt32(schIndexid));

                if (Ind > 0)
                {
                    indList.Add(Ind.ToString());
                    sortList.Add(Ind);
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
            var returnData = names.Select(name => new SimpleNavReturnModel
            {
                Name = name,
                ValueAndDate =
                    data.Where(x => x.Field<string>("Sch_Short_Name") == name)
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

        [WebMethod]
        public static List<PortfolioMktValue> portfolioMKT_Val(int schemeIds)
        {
            return AllMethods.getPortfolioAsset(schemeIds);
        }

        [WebMethod]
        public static List<AssetAlocation> assetAllocaton(int schemeIds)
        {
            return AllMethods.getAssetAllocationUsingFundId(schemeIds);
        }

        public void TopCompanys(int schemeId)
        {
            DataTable _TopComp = AllMethods.getTopCompany(schemeId, 10);

            _TopComp.Columns.Add("BarValue", typeof(string));
            var totalAsset = _TopComp.AsEnumerable().Where(x => Double.TryParse(x["Net_Asset"].ToString(), out double tryData)).Sum(x => Convert.ToDouble(x["Net_Asset"].ToString()));
            _TopComp.AsEnumerable().Where(x => Double.TryParse(x["Net_Asset"].ToString(), out double tryData)).ToList().ForEach(x => x["BarValue"] = Math.Round(((((Convert.ToDouble(x["Net_Asset"].ToString())) * 1.8) / totalAsset) * 100), 0));
            double tryDatadbl = 0;
            var dtTopComp = (from v in _TopComp.AsEnumerable()
                             orderby v["Net_Asset"] descending
                             select (new
                             {
                                 CompName = v["CompName"],
                                 Sector_Name = v["Sector_Name"],
                                 Bar_Value =  v["BarValue"],
                                 Net_Asset = Double.TryParse(v["Net_Asset"].ToString(), out tryDatadbl) ? (tryDatadbl != 0) ? (String.Format("{0:#,0.00}", tryDatadbl)) + "%" : "-" : "-"
                             }));
            var dtCompTop = new DataTable();
            var dtCompBottom5 = new DataTable();

            if (_TopComp.Rows.Count >= 10)
            {
                var dtTopComp10 = dtTopComp.Take(10);

                var dtTopComp5 = dtTopComp10.Take(5);
                TopCompDetails.DataSource = dtTopComp5.ToDataTable();
                dtCompTop = dtTopComp5.ToDataTable().Copy();
                TopCompDetails.DataBind();

                TopCompDetails1.DataSource = dtTopComp10.Except(dtTopComp5.AsEnumerable()).ToDataTable();
                dtCompBottom5 = dtTopComp10.Except(dtTopComp5.AsEnumerable()).ToDataTable().Copy();
                TopCompDetails1.DataBind();
            }
            else
            {
                var dtTopComp10 = dtTopComp;
                if (dtTopComp10.Count() <= 5)
                {
                    var dtTopComp5 = dtTopComp10.ToDataTable();
                    TopCompDetails.DataSource = dtTopComp5;
                    TopCompDetails.DataBind();
                    dtCompTop = dtTopComp10.ToDataTable().Copy();
                    TopCompDetails1.Visible = false;
                }
                else
                {
                    var dtTopComp5 = dtTopComp10.Take(5);
                    TopCompDetails.DataSource = dtTopComp5.ToDataTable();
                    dtCompTop = dtTopComp5.ToDataTable().Copy();
                    TopCompDetails.DataBind();

                    TopCompDetails1.DataSource = dtTopComp10.Except(dtTopComp5.AsEnumerable()).ToDataTable();
                    dtCompBottom5 = dtTopComp10.Except(dtTopComp5.AsEnumerable()).ToDataTable().Copy();
                    TopCompDetails1.DataBind();
                }
            }


        }

        protected void TopCompDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var prgBarValue = (e.Item.FindControl("hdfProgressBarWidth") as HiddenField).Value;
                //HtmlGenericControl lblprgbar = e.Item.FindControl("lblprgbar") as HtmlGenericControl;


                Label lblprgbar1 = e.Item.FindControl("lblprgbar") as Label;
                lblprgbar1.Attributes.Add("style", "width:" + prgBarValue + "%");
            }
        }

        protected void TopCompDetails1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var prgBarValue = (e.Item.FindControl("hdfProgressBarWidth1") as HiddenField).Value;
                //HtmlGenericControl lblprgbar = e.Item.FindControl("lblprgbar") as HtmlGenericControl;

                Label lblprgbar1 = e.Item.FindControl("lblprgbar1") as Label;
                lblprgbar1.Attributes.Add("style", "width:" + prgBarValue + "%");
            }
        }

        public void TopSector(int schemeId)
        {
            var _dtOutPut = AllMethods.getTopSector(schemeId);
            var dtOutput = _dtOutPut.ToDataTable();
            double trydata = 0;
            var Sum = _dtOutPut.Where(x => Double.TryParse(x.Scheme.ToString(), out trydata)).Sum(x => Convert.ToDouble(x.Scheme.ToString()));

            dtOutput.Columns.Add("Scheme_Per", typeof(string));

            dtOutput.AsEnumerable().Where(x => Double.TryParse(x["Scheme"].ToString(), out trydata) && trydata != 0).ToList().ForEach(x => x["Scheme_Per"] = (String.Format("{0:#,0.00}", Convert.ToDouble(x["Scheme"]))) + " % ");
            dtOutput.AcceptChanges();
            RepTopSector.DataSource = dtOutput;
            RepTopSector.DataBind();


            dtOutput.Columns.Add("Bar_Value", typeof(string));

            dtOutput.AsEnumerable().ToList().ForEach(x => x["Bar_Value"] = Math.Round((((Convert.ToDouble(x["Scheme"].ToString()) * 1.8) / Sum) * 100), 0));


            var rowCnt = 0;
            foreach (RepeaterItem item in RepTopSector.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    if (Int32.TryParse(dtOutput.Rows[rowCnt]["Bar_Value"].ToString(), out int tryData))
                    {
                        Label mydiv = item.FindControl("lblprgbarTop5") as Label;
                        mydiv.Attributes.Add("style", "width:" + tryData + "%;");
                        //do something
                    }
                    rowCnt++;
                }
            }

        }

        [WebMethod]
        public static BansalCapitalMcapAvgMaturity McapAndAvgMat(string SchId)
        {
            int _SchId = Convert.ToInt32(SchId);
            var vv = AllMethods.getMCapAvgMaturityWoutRebase(_SchId);
            return vv;
        }
    }
}