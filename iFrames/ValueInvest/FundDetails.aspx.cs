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
using iFrames.Classes;

namespace iFrames.ValueInvest
{
    public partial class FundDetails : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getSchemeId();
            }

        }


        private void getSchemeId()
        {
            string SchemeId = Request.QueryString["id"];
            if (SchemeId != null)
            {
                if (!string.IsNullOrEmpty(SchemeId))
                {
                    ShowFundDeails(SchemeId);
                }
            }
        }



        private void ShowFundDeails(string SchemeId)
        {
            #region Variable  Declaration and initilisation
            StringBuilder sb = new StringBuilder();
            DataSet dsCompFund = new DataSet("dataSetFundDetails");
            DataTableUtility dtu = new DataTableUtility();
            DataTable dtScheme = new DataTable("Scheme Table");
            string strPeriods = "91 D,182 D,1 YYYY,3 YYYY,5 YYYY";
            DateTime curentDate = DateTime.Today.AddDays(-1);// YesterDay Date
            DataTable SipdataTable = new DataTable();
            #endregion



            #region Collect Data Points

            #region Get Selected Scheme from the List
            List<string> SelectList = new List<string>();
            SelectList.Add(SchemeId);
            #endregion

            #region Get Latest Portfolio Date
            DateTime latestPortfolioDate = AllMethods.getLatestPortfiloDate(SelectList[0]);

            #endregion

            #region    Get scheme id and name details
            dtScheme = AllMethods.getSchName(SelectList[0]);
            dsCompFund.Tables.Add(dtScheme);

            #endregion

            #region portfolio date datatable

            //portfolio dtae
            dsCompFund.Tables.Add(AllMethods.getLatestPortfiloDateDT(SelectList[0]));

            #endregion

            #region Schemes Nature

            // Get Schemes Nature
            dsCompFund.Tables.Add(AllMethods.getCategoryOfScheme(SelectList[0]));

            #endregion

            #region Get Schemes FundManger Name

            // // Get Schemes FundManger Name
            dsCompFund.Tables.Add(AllMethods.getFundManager(SelectList[0]));

            #endregion

            #region Scheme ICRON Rank

            // Get Scheme iCRON Rank
            dsCompFund.Tables.Add(AllMethods.getSchemeICRONRank(SelectList[0], "1 year"));

            #endregion

            #region Get Scheme Asset Allocation
            // Get Scheme Asset Allocation
            dsCompFund.Tables.Add(AllMethods.getAssetAllocation(SelectList[0]));

            #endregion

            #region Get Market Cap
            //Get Market Cap

            dsCompFund.Tables.Add(AllMethods.getMarketCapClassification(SelectList[0]));

            #endregion

            #region Top Holding

            // Get Scheme Top Holding
            dsCompFund.Tables.Add(AllMethods.getTopHolding(SelectList[0], 5));

            #endregion

            #region Top Company Holding

            // Get Scheme Top Holding
            dsCompFund.Tables.Add(AllMethods.getTopCompanyHolding(SelectList[0], 5));

            #endregion

            #region Top 3 Sector

            // Get top 3 Sector 
            dsCompFund.Tables.Add(AllMethods.getTopSector(SelectList[0], 3));

            #endregion

            #region Get Portfolio PE
            // dsCompFund.Tables.Add(AllMethods.getPortfolioPE(SelectList[0]));
            dsCompFund.Tables.Add(AllMethods.getPortfolioPE(SelectList[0], latestPortfolioDate));

            #endregion

            #region Get Avg_Mat YTM
            // get  Avg_Mat YTM

            dsCompFund.Tables.Add(AllMethods.getAvg_Mat_YTM(SelectList[0]));

            #endregion

            #region Get Expense ratio

            //get expense ratio
            dsCompFund.Tables.Add(AllMethods.getExpenseRatio(SelectList[0]));

            #endregion

            #region Get exit load


            //get exit load

            //dsCompFund.Tables.Add(AllMethods.getEntryExitLoad(SelectList[0]));

            //dtScheme.Columns.Add("SchemeId");

            //foreach (string strSch in SelectList[0].Split(',').ToList())
            //{
            //    dtScheme.Rows.Add(new object[] { strSch });
            //}

            dsCompFund.Tables.Add(dtu.GetEntryExitLoad(dtScheme, "Scheme_Id", "Exit", ""));

            #endregion

            #region performance
            //get performance          

            //get data from sp
            //dsCompFund.Tables.Add(AllMethods.getPerformance2(SelectList[0],strPeriods,latestPortfolioDate,2,2));          
            // get data from top Fund           

            dsCompFund.Tables.Add(AllMethods.getPerformance_TF(SelectList[0]));

            #endregion

            #region Sip analysis
            //Get sip analysis

            // get latest nav date of each scheme respective
            //DataTable navdateTable = new DataTable();
            //navdateTable = AllMethods.getLatestNavDetail(SelectList[0]);
            //DateTime leastNavDate = navdateTable.AsEnumerable().Select(x => x.Field<DateTime>("Nav_Date")).Min();
            //dsCompFund.Tables.Add(AllMethods.getSIPAnalysis(SelectList[0], 5000, leastNavDate.AddYears(-3), leastNavDate, leastNavDate, "Monthly", "Individual/HUF"));


            SipdataTable = AllMethods.getSIPAnalysis(SelectList[0], 5000, curentDate.AddYears(-3).AddMonths(1), curentDate, curentDate, "Monthly", "Individual/HUF");
            //  dsCompFund.Tables.Add(AllMethods.getSIPAnalysis(SelectList[0], 5000, curentDate.AddYears(-3).AddMonths(1), curentDate, curentDate, "Monthly", "Individual/HUF"));


            #endregion

            #region Risk Measure
            // Get Risk Measure

            dsCompFund.Tables.Add(AllMethods.getRiskMeasure(SelectList[0]));
            #endregion

            #endregion

            # region Set Data On Page



            # region Name,Portfoliodate,Category,Manager

            LabelSchemeName.Text = dsCompFund.Tables[0].Rows[0]["Sch_Short_Name"].ToString();
            LabelPortfolioDate.Text = dsCompFund.Tables[1].Rows[0]["PortDate"].ToString().Split(' ')[0];
            LabelCategory.Text = dsCompFund.Tables[2].Rows[0]["Nature"].ToString();

            var fmgdata = dsCompFund.Tables[3].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                      .Select(p => p.Field<string>("FUND_MANAGER_NAME"));
            var fundManName = string.Empty;
            if (fmgdata != null && fmgdata.Count() > 0)
            {
                fundManName = string.Join(",", fmgdata.ToList());
            }

            LabelFundManager.Text = fundManName.ToString();

            #endregion

            # region Rank
            dynamic RankIcron = "N.A.";
            var fmdata = dsCompFund.Tables[4].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == SchemeId)
                  .Select(p => p.Field<string>("RANK")).SingleOrDefault();
            if (fmdata != null)
            {
                var rank = fmdata.ToString();
                if (!string.IsNullOrEmpty(rank))
                {
                    for (int n = 1; n <= Convert.ToInt16(rank.TrimStart().Split(' ')[0]); n++)
                    {
                        sb.Append(@"<img src='img/star.png' width='12' height='12' />");
                    }
                }
                else
                {
                    sb.Append("N.A.");
                }

                RankIcron = sb.ToString();
            }
            divSpanrank.InnerHtml = RankIcron;

            #endregion

            # region Asset Allocation

            var Eqdata = dsCompFund.Tables[5].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                    .Where(x => x.Field<string>("NATURE_NAME") == "EQ")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("Corpus_per")), 2)).SingleOrDefault();

            var Debtdata = dsCompFund.Tables[5].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                   .Where(x => x.Field<string>("NATURE_NAME") == "Debt")
                   .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("Corpus_per")), 2)).SingleOrDefault();

            var Othersdata = dsCompFund.Tables[5].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                   .Where(x => x.Field<string>("NATURE_NAME") == "Others")
                   .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("Corpus_per")), 2)).SingleOrDefault();


            LabelEquity.Text = Eqdata != null ? Eqdata.ToString() + " %" : "N.A.";
            LabelDebt.Text = Debtdata != null ? Debtdata.ToString() + " %" : "N.A.";
            LabelOthers.Text = Othersdata != null ? Othersdata.ToString() + " %" : "N.A.";

            #endregion

            #region Market cap

            var LargeCapdata = dsCompFund.Tables[6].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                    .Where(x => x.Field<string>("MARKET_SLAB") == "Large Cap")
                  .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("MCAPALLOCATION")), 2)).SingleOrDefault();

            var MidCapdata = dsCompFund.Tables[6].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                      .Where(x => x.Field<string>("MARKET_SLAB") == "Mid Cap")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("MCAPALLOCATION")), 2)).SingleOrDefault();


            var SmallCapdata = dsCompFund.Tables[6].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                      .Where(x => x.Field<string>("MARKET_SLAB") == "Small Cap")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("MCAPALLOCATION")), 2)).SingleOrDefault();


            LabelLCap.Text = LargeCapdata != null ? LargeCapdata.ToString() + " %" : "N.A.";
            LabelMCap.Text = MidCapdata != null ? MidCapdata.ToString() + " %" : "N.A.";
            LabelSCap.Text = SmallCapdata != null ? SmallCapdata.ToString() + " %" : "N.A.";

            #endregion


            #region Top 5 Holding

            var Top5HoldTotaldata = dsCompFund.Tables[7].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("SumCorpus")), 2)).SingleOrDefault();

            LabelTop5HoldingsTotal.Text = Top5HoldTotaldata != null ? Top5HoldTotaldata.ToString() + " %" : "N.A.";


            var TopCompanayHolding = dsCompFund.Tables[8].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                    .Select(t => new { Company_Name = t.Field<string>("Company_Name"), SumCorpus = t.Field<double>("SumCorpus") }).ToList();

            if (TopCompanayHolding != null)
            {
                LabelTop1Hold.Text = TopCompanayHolding[0].Company_Name;
                LabelTop1HoldPer.Text = Convert.ToString(Math.Round(TopCompanayHolding[0].SumCorpus, 2)) + " %";

                LabelTop2Hold.Text = TopCompanayHolding[1].Company_Name;
                LabelTop2HoldPer.Text = Convert.ToString(Math.Round(TopCompanayHolding[1].SumCorpus, 2)) + " %";

                LabelTop3Hold.Text = TopCompanayHolding[2].Company_Name;
                LabelTop3HoldPer.Text = Convert.ToString(Math.Round(TopCompanayHolding[2].SumCorpus, 2)) + " %";

                LabelTop4Hold.Text = TopCompanayHolding[3].Company_Name;
                LabelTop4HoldPer.Text = Convert.ToString(Math.Round(TopCompanayHolding[3].SumCorpus, 2)) + " %";

                LabelTop5Hold.Text = TopCompanayHolding[4].Company_Name;
                LabelTop5HoldPer.Text = Convert.ToString(Math.Round(TopCompanayHolding[4].SumCorpus, 2)) + " %";
            }


            #endregion

            # region Top 3 Sector

            var SectordataTotal = dsCompFund.Tables[9].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                       .Select(p => p.Field<double>("SumCorpus")).Sum();
            if (SectordataTotal != null)
            {
                LabelTop3SecPer.Text = Convert.ToString(Math.Round(SectordataTotal, 2));
            }

            var Sectordata = dsCompFund.Tables[9].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
               .Select(p => new { Sector_Name = p.Field<string>("Sector_Name"), SumCorpus = p.Field<double>("SumCorpus") }).Take(3).ToList();


            if (Sectordata != null)
            {
                LabelTopSec1.Text = Sectordata[0].Sector_Name;
                LabelTopSec1Per.Text = Convert.ToString(Math.Round(Sectordata[0].SumCorpus, 2)) + " %";

                LabelTopSec2.Text = Sectordata[1].Sector_Name;
                LabelTopSec2Per.Text = Convert.ToString(Math.Round(Sectordata[1].SumCorpus, 2)) + " %";

                LabelTopSec3.Text = Sectordata[2].Sector_Name;
                LabelTopSec3Per.Text = Convert.ToString(Math.Round(Sectordata[2].SumCorpus, 2)) + " %";

            }

            #endregion

            # region Portfolio PE,AverageMat,YTM,ExitLoad,Exp_Ratio

            var PEdata = dsCompFund.Tables[10].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                      .Select(p => new { PRICE_EARNING = p.Field<double>("PRICE_EARNING") }).SingleOrDefault();
            LabelPortfolioPE.Text = PEdata != null ? Convert.ToString(Math.Round(PEdata.PRICE_EARNING, 2)) + " %" : "N.A.";

            var AvgMatdata = dsCompFund.Tables[11].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                      .Select(p => p.Field<double?>("AVG_MAT")).Take(1).SingleOrDefault();
            LabelAverageMat.Text = AvgMatdata != null ? Convert.ToString(Math.Round(AvgMatdata.Value / 365, 2)) + " Years" : "N.A.";

            var YTMdata = dsCompFund.Tables[11].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                      .Select(p => p.Field<double?>("YTM")).Take(1).SingleOrDefault();
            LabelYTM.Text = YTMdata != null ? YTMdata.ToString() + " %" : "N.A.";

            var Exp_Ratiodata = dsCompFund.Tables[12].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == SchemeId)
                    .OrderBy(p => p.Field<double?>("Exp_Ratio"))
                       .Select(p => p.Field<double?>("Exp_Ratio")).Take(1).SingleOrDefault();
            LabelExpenseRatio.Text = Exp_Ratiodata != null ? Exp_Ratiodata.ToString() + " %" : "N.A.";

            var ExitLoaddata = dsCompFund.Tables[13].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == SchemeId)
                       .Select(p => p.Field<string>("Statement")).Take(1).SingleOrDefault();

            LabelExitload.Text = ExitLoaddata != null ? ExitLoaddata.ToString() : "N.A.";

            #endregion

            # region Performance

            var return3month = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == SchemeId)
                       .Select(p => p.Field<double?>("Per_91_Days")).SingleOrDefault();
            LabelPer3Mon.Text = return3month != null ? Convert.ToString(Math.Round(return3month.Value, 2)) + " %" : "N.A.";

            var return6month = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == SchemeId)
                      .Select(p => p.Field<double?>("Per_182_Days")).SingleOrDefault();
            LabelPer6Mon.Text = return6month != null ? Convert.ToString(Math.Round(return6month.Value, 2)) + " %" : "N.A.";

            var return1yr = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == SchemeId)
                      .Select(p => p.Field<double?>("Per_1_Year")).SingleOrDefault();
            LabelPer1Year.Text = return1yr != null ? Convert.ToString(Math.Round(return1yr.Value, 2)) + " %" : "N.A.";

            var return3yr = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == SchemeId)
                      .Select(p => p.Field<double?>("Per_3_Year")).SingleOrDefault();
            LabelPer3Year.Text = return3yr != null ? Convert.ToString(Math.Round(return3yr.Value, 2)) + " %" : "N.A.";

            var return5yr = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == SchemeId)
                      .Select(p => p.Field<double?>("Per_5_Year")).SingleOrDefault();
            LabelPer5Year.Text = return5yr != null ? Convert.ToString(Math.Round(return5yr.Value, 2)) + " %" : "N.A.";

            #endregion

            # region SIP Analysis

            var SIPdata = SipdataTable.AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme")) == dsCompFund.Tables[0].Rows[0]["Sch_Short_Name"].ToString())
                     .Select(p => new { TOTAL_AMOUNT = p.Field<double?>("TOTAL_AMOUNT"), PRESENT_VALUE = p.Field<double?>("PRESENT_VALUE"), YIELD = p.Field<double?>("YIELD") }).ToList();
            if (SIPdata != null && SIPdata.Count > 0)
            {
                LabelInvestedAmt.Text = Convert.ToString(Math.Round(SIPdata[0].TOTAL_AMOUNT.Value, 2));
                LabelSIPvalue.Text = Convert.ToString(Math.Round(SIPdata[0].PRESENT_VALUE.Value, 2));
                LabelReturnCAGR.Text = Convert.ToString(Math.Round(SIPdata[0].YIELD.Value, 2)) + " %";
            }
            else
            {
                LabelInvestedAmt.Text = LabelSIPvalue.Text = LabelReturnCAGR.Text = "N.A.";
            }

            #endregion

            # region Risk Measure

            var Riskdata = dsCompFund.Tables[15].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == SchemeId)
                      .Select(p => new { Sharp = p.Field<double?>("Sharp"), Sortino = p.Field<double?>("Sortino"), STDV = p.Field<double?>("STDV") }).Take(1).ToList();

            if (Riskdata != null && Riskdata.Count > 0)
            {
                if (Riskdata[0].Sharp != null) LabelSharpe.Text = Convert.ToString(Math.Round(Riskdata[0].Sharp.Value, 2)) + " %"; else LabelSharpe.Text = "N.A.";
                if (Riskdata[0].Sortino != null) LabelSortino.Text = Convert.ToString(Math.Round(Riskdata[0].Sortino.Value, 2)) + " %"; else LabelSortino.Text = "N.A.";
                if (Riskdata[0].STDV != null) LabelStandardDev.Text = Convert.ToString(Math.Round(Riskdata[0].STDV.Value, 2)) + " %"; else LabelStandardDev.Text = "N.A.";
            }
            else
            {
                LabelSharpe.Text = LabelSortino.Text = LabelStandardDev.Text = "N.A.";
            }
            #endregion


            #endregion

        }
    }
}