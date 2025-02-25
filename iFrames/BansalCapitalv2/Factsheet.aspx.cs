using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;
using System.Text;
using System.Web.Services;

namespace iFrames.BansalCapital
{
    public partial class Factsheet : System.Web.UI.Page
    {
        string RankingDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //getFundHouse();
                if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                {
                    hdSchemeId.Value=Request.QueryString["param"];
                    tblResult.Visible = true;
                    setInvestmentInfo(Convert.ToInt32(Request.QueryString["param"]));
                    TopCompanys(Convert.ToInt32(Request.QueryString["param"]));
                    PeerSchemes(Convert.ToInt32(Request.QueryString["param"]));
                    TopSector(Convert.ToInt32(Request.QueryString["param"]));
                    PreCalculatedRatio(Convert.ToInt32(Request.QueryString["param"]));
                    if (Convert.ToInt32(Request.QueryString["param"]) > 0)
                    {
                        var rows = AllMethods.GetHighLowNav(Convert.ToInt32(Request.QueryString["param"]));
                        if (rows != null && rows.Any())
                        {
                            lblHigh.Text = rows["max_nav"];
                            lblLow.Text = rows["min_nav"];
                            spnMinDate.InnerText = rows["min_nav_date"];
                            spnMaxDate.InnerText = rows["max_nav_date"];
                        }
                    }
                }
                else
                {
                    tblResult.Visible = false;
                }
            }
        }



        [System.Web.Services.WebMethod]
        public static BansalCapitalMcapAvgMaturity McapAndAvgMat(int SchId)
        {

            var vv = AllMethods.getMCapAvgMaturity(SchId);
            return vv;
        }

        //protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlFundHouse.SelectedIndex != 0)
        //        getSchemesList(ddlFundHouse.SelectedItem.Value);
        //    else
        //    {
        //        listboxSchemeName.Items.Clear();
        //    }
        //}

        //protected void listboxSchemeName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //if (listboxSchemeName.SelectedIndex != -1)
        //    //{
        //    //	TopCompanys(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
        //    //	PeerSchemes(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
        //    //}
        //}

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    tblResult.Visible = true;
        //    if (listboxSchemeName.SelectedIndex > 0)
        //    {
        //        setInvestmentInfo(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
        //        TopCompanys(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
        //        PeerSchemes(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
        //    }
        //    else
        //    {
        //        tblResult.Visible = false;
        //    }
        //}

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

        private void setInvestmentInfo(int schemeId)
        {
            var _investmentInfo = AllMethods.getInvestInfo(schemeId);
            lblFundName.Text = _investmentInfo.FundName;
            lblPresentNav.Text = _investmentInfo.CurrentNav.ToString();
            double num;
            if (double.TryParse(_investmentInfo.CurrentNav.ToString().Trim(), out num) && double.TryParse(_investmentInfo.PrevNav.ToString().Trim(), out num))
            {
                //if (double.TryParse(_investmentInfo.PrevNav, out num))
                //{
                double curNav = Convert.ToDouble(_investmentInfo.CurrentNav);
                double PrevNav = Convert.ToDouble(_investmentInfo.PrevNav);
                lblIncrNav.Text = (Math.Round(curNav - PrevNav, 4)).ToString() + " (" +
                                (Math.Round(100 * (curNav - PrevNav) / PrevNav, 2)).ToString() + "%)";
                imgArrow.Visible = true;
                if (curNav - PrevNav >= 0)
                {
                    imgArrow.Src = "img/arwup.png";
                }
                else
                {
                    imgArrow.Src = "img/arwdwn.png";
                }
                //}
            }
            else
            {
                imgArrow.Visible = false;
                lblIncrNav.Text = "";
            }

            tdFundName.InnerText = _investmentInfo.FundName;
            tdAssetSize.InnerText = Convert.ToString(_investmentInfo.FundSize);
            tdStructure.InnerText = _investmentInfo.StructureName;
            tdMinInvest.InnerText = _investmentInfo.MinInvestment;
            tdLunchDate.InnerText = _investmentInfo.LunchDate.HasValue ? _investmentInfo.LunchDate.Value.ToString("dd/MM/yyyy") : "";
            tdFundMan.InnerText = _investmentInfo.FundMan;
            tdLastDiv.InnerText = _investmentInfo.LatestDiv;
            tdBenchMark.InnerText = _investmentInfo.BenchMark;
            tdEmail.InnerText = string.IsNullOrEmpty(_investmentInfo.Website) ? _investmentInfo.Email : _investmentInfo.Website + "/" + _investmentInfo.Email;
            tdBonus.InnerText = _investmentInfo.Bonous;
            tdAmcName.InnerText = _investmentInfo.AmcName;
            tdEntryLoad.InnerText = _investmentInfo.EntryLoad;
            tdExitLoad.InnerText = _investmentInfo.ExitLoad;
        }
        //private void getFundHouse()
        //{
        //    DataTable dtFund = AllMethods.getFundHouse();
        //    DataRow drFund = dtFund.NewRow();
        //    drFund["MutualFund_Name"] = "Select";
        //    drFund["MutualFund_ID"] = 0;
        //    dtFund.Rows.InsertAt(drFund, 0);
        //    ddlFundHouse.DataSource = dtFund;
        //    ddlFundHouse.DataTextField = "MutualFund_Name";
        //    ddlFundHouse.DataValueField = "MutualFund_ID";
        //    ddlFundHouse.DataBind();
        //}

        //private void getSchemesList(string fundId)
        //{
        //    DataTable dtScheme = AllMethods.getScheme(false, false, fundId);
        //    DataRow drScheme = dtScheme.NewRow();
        //    //drScheme["Sch_Short_Name"] = "Select";
        //    //drScheme["Scheme_Id"] = 0;
        //    dtScheme.Rows.InsertAt(drScheme, 0);
        //    listboxSchemeName.DataSource = dtScheme;
        //    listboxSchemeName.DataTextField = "Sch_Short_Name";
        //    listboxSchemeName.DataValueField = "Scheme_Id";
        //    listboxSchemeName.DataBind();
        //    listboxSchemeName.SelectedIndex = 0;
        //}

        [System.Web.Services.WebMethod]
        public static List<AssetAlocation> assetAllocaton(int schemeIds)
        {
            return AllMethods.getAssetAllocationUsingFundId(schemeIds);
        }

       
        public void TopCompanys(int schemeId)
        {
            DataTable _TopComp = AllMethods.getTopCompany(schemeId, 10);

            var dtTopComp = (from v in _TopComp.AsEnumerable()
                             orderby v["Net_Asset"] descending
                             select (new
                             {
                                 CompName = v["CompName"],
                                 Sector_Name = v["Sector_Name"],
                                 Net_Asset = Math.Round(Convert.ToDecimal(v["Net_Asset"]), 2)
                             }));

            if (_TopComp.Rows.Count >= 10)
            {
                var dtTopComp10 = dtTopComp.Take(10);

                var dtTopComp5 = dtTopComp10.Take(5);
                TopCompDetails.DataSource = dtTopComp5.ToDataTable();
                TopCompDetails.DataBind();

                TopCompDetails1.DataSource = dtTopComp10.Except(dtTopComp5.AsEnumerable()).ToDataTable();
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
                }
                else
                {
                    var dtTopComp5 = dtTopComp10.Take(5);
                    TopCompDetails.DataSource = dtTopComp5.ToDataTable();
                    TopCompDetails.DataBind();

                    TopCompDetails1.DataSource = dtTopComp10.Except(dtTopComp5.AsEnumerable()).ToDataTable();
                    TopCompDetails1.DataBind();
                }
            }
        }

        //public void TopCompanys(int schemeId)
        //{
        //    DataTable _TopComp = AllMethods.getTopCompany(schemeId, 10);
        //    if (_TopComp.Rows.Count >= 10)
        //    {
        //        var dtTopComp10 = (from v in _TopComp.AsEnumerable()
        //                           orderby v["Net_Asset"] descending
        //                           select (new
        //                           {
        //                               CompName = v["CompName"],
        //                               Sector_Name = v["Sector_Name"],
        //                               Net_Asset = Math.Round(Convert.ToDecimal(v["Net_Asset"]), 2)
        //                           })).Take(10);


        //        var dtTopComp5 = dtTopComp10.Take(5).ToDataTable();
        //        TopCompDetails.DataSource = dtTopComp5;
        //        TopCompDetails.DataBind();

        //        TopCompDetails1.DataSource = dtTopComp10.Except(dtTopComp10.AsEnumerable().Take(5)).ToDataTable();
        //        TopCompDetails1.DataBind();
        //    }
        //}

        public void PeerSchemes(int schemeId)
        {

            int peerSchCnt = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ValueInvestPeerSchemeCount"]);
            DataSet ds = AllMethods.getSebiPeerPerformance(schemeId, peerSchCnt);
            DataTable _PeerSchemes = ds.Tables[0];
            RankingDate = Convert.ToString(ds.Tables[1].Rows[0][1]);
            var Data = _PeerSchemes.AsEnumerable().Where(x => Convert.ToInt32(x["SchemeId"]) == schemeId).Select(x => x).FirstOrDefault();
            spn1mth.InnerText = Data["Per30Days"].ToString();
            spn3mth.InnerText = Data["Per91Days"].ToString();
            spn6mth.InnerText = Data["Per182Days"].ToString();
            spn1yr.InnerText = Data["Per1Year"].ToString();
            spn3yr.InnerText = Data["Per3Year"].ToString();
            spnSinceInception.InnerText = Data["SI"].ToString();

            //PeerPerformance.DataSource = _PeerSchemes;
            //PeerPerformance.DataBind();
        }
        public void TopSector(int schemeId)
        {
            RepTopSector.DataSource = AllMethods.getTopSector(schemeId);
            RepTopSector.DataBind();
        }
        [System.Web.Services.WebMethod]
        public static List<PortfolioMktValue> portfolioMKT_Val(int schemeIds)
        {
            return AllMethods.getPortfolioAsset(schemeIds);
        }

        protected void PeerPerformance_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                //Label headerRepeater = e.Item.FindControl("lbl_Rank_Head") as Label;
                //headerRepeater.Text = "ICRON Ranking (" + RankingDate + ")";
            }
        }

        //old
        [System.Web.Services.WebMethod]
        public static iFrames.BLL.ChartNavReturnModel NavChartData(string schemeIds)
        {
            var _NavChart = AllMethods.GetMFINav(Convert.ToDecimal(schemeIds), DateTime.Today.AddDays(-365), DateTime.Today);
            return _NavChart;
        }

        #region: Datagrid Event
        [WebMethod]
        public static ChartNavReturnModel getChartData(string schIndexid, DateTime startDate, DateTime endDate)// string startdate, string enddate, List<string> schList,List<string> indList
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

                if (Ind >0)
                {
                    indList.Add(Ind.ToString());
                    sortList.Add(Ind);
                }
                //var strArray = schIndexid.Split('#').ToList();

                //foreach (string str in strArray)
                //{
                //    if (str.Trim().ToUpper().StartsWith("S"))
                //    {
                //        schList.Add(str.Trim().ToUpper().TrimStart('S'));
                //        sortList.Add(Convert.ToDecimal(str.Trim().ToUpper().TrimStart('S')));
                //    }

                //    if (str.Trim().ToUpper().StartsWith("I"))
                //    {
                //        indList.Add(str.Trim().ToUpper().TrimStart('I'));
                //        sortList.Add(Convert.ToDecimal(str.Trim().ToUpper().TrimStart('I')));
                //    }

                //}
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

                if (objSchInd.ListIndex.Count() > 0)
                {
                    _dtInd = AllMethods.getHistIndexRecordDetails(AllMethods.getFundSchemeIdStr(objSchInd.ListIndex), mindate, System.DateTime.Now);
                    if (_dtInd != null && _dtInd.Rows.Count > 0)
                    {

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

                            OrginalValue = Math.Round(x.Field<double>("Nav"), 2)
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

        //This  function will rebse to 100
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
                data.Rows[i][4] = 0;

                if (data.Rows[i][1].ToString() != preScheme)
                {
                    firstNav = Convert.ToDouble(data.Rows[i][3]);
                    data.Rows[i][4] = 100;
                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                else
                {
                    if (Convert.ToDouble(data.Rows[i - 1][3]) != 0)
                        data.Rows[i][4] = (100 * Convert.ToDouble(data.Rows[i][3])) / firstNav;
                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                preScheme = data.Rows[i][1].ToString();
            }

            return data;
        }
        #endregion

    }
}