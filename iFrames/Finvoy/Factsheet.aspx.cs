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
using System.Web.UI.HtmlControls;

namespace iFrames.Finvoy
{
    public partial class Factsheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadAMC();
                LoadNature();
                LoadSubNature();
                LoadStructure();
                LoadScheme();
                checkDefaultSelect();
                //getFundHouse();
                if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                {
                    dvFactSheet.Visible = true;
                    hdSchemeId.Value = Request.QueryString["param"];
                    //tblResult.Visible = true;
                    setInvestmentInfo(Convert.ToInt32(hdSchemeId.Value));
                    TopCompanys(Convert.ToInt32(hdSchemeId.Value));
                    PeerSchemes(Convert.ToInt32(hdSchemeId.Value));
                    TopSector(Convert.ToInt32(hdSchemeId.Value));
                    PreCalculatedRatio(Convert.ToInt32(hdSchemeId.Value));
                    if (Convert.ToInt32(hdSchemeId.Value) > 0)
                    {
                        var rows = AllMethods.GetHighLowNav(Convert.ToInt32(hdSchemeId.Value));
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
                    //tblResult.Visible = false;
                }
            }
        }



        [System.Web.Services.WebMethod]
        public static BansalCapitalMcapAvgMaturity McapAndAvgMat(int SchId)
        {

            var vv = AllMethods.getMCapAvgMaturityWoutRebase(SchId);
            return vv;
        }

        

        private void PreCalculatedRatio(int SchemeId)
        {
            var _data = AllMethods.getPreCalculatedRatio(SchemeId);
            if (_data != null)
            {
                double trydata;
                lblSharpe.InnerText = _data.Sharp != null ? Double.TryParse(_data.Sharp.ToString(), out trydata) ? Convert.ToString(Math.Round(Convert.ToDouble(_data.Sharp), 2)) : "--" : "--";
                lblSortino.InnerText = _data.Sortino != null ? Double.TryParse(_data.Sortino.ToString(), out trydata) ? Convert.ToString(Math.Round(Convert.ToDouble(_data.Sortino), 2)) : "--" : "--";
                lblSdv.InnerText = _data.STDV != null ? Double.TryParse(_data.STDV.ToString(), out trydata) ? Convert.ToString(Math.Round(Convert.ToDouble(_data.STDV), 2)) : "--" : "--";
                lblBeta.InnerText = _data.Beta != null ? Double.TryParse(_data.Beta.ToString(), out trydata) ? Convert.ToString(Math.Round(Convert.ToDouble(_data.Beta), 2)) : "--" : "--";
                lblRSqure.InnerText = _data.RSQR != null ? Double.TryParse(_data.RSQR.ToString(), out trydata) ? Convert.ToString(Math.Round(Convert.ToDouble(_data.RSQR), 2)) : "--" : "--";
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
            #region Top Fund Details Section 
            var _investmentInfo = AllMethods.getInvestInfoNew(schemeId);
            lblFundType.Text = _investmentInfo.StructureName;
            lblFundNameBold.Text = _investmentInfo.FundName;
            lblEntry.Text = _investmentInfo.EntryLoad;
            lblExit.Text = _investmentInfo.ExitLoad;
            lblAsset.Text = Double.TryParse(_investmentInfo.FundSize, out double tryAsset) ? (String.Format("{0:#,0.00}", Convert.ToDouble(tryAsset))) : "";
            lblInvestMent.Text = _investmentInfo.InvestmentPlan;
            lblMinIvest.Text = _investmentInfo.MinInvestment.ToString();
            lblIncepDate.Text = _investmentInfo.LunchDate != null ? Convert.ToDateTime(_investmentInfo.LunchDate).ToString("dd/MM/yyyy") : "";
            lblFundMan.Text = _investmentInfo.FundMan;
            lblLastDiv.Text = Double.TryParse(_investmentInfo.LatestDiv, out double tryDiv) ? (String.Format("{0:#,0.00}", tryDiv)) : _investmentInfo.LatestDiv;
            lblBench.Text = _investmentInfo.BenchMark;
            lblWebSite.Text = _investmentInfo.Website;
            lblBonus.Text = _investmentInfo.Bonous;
            lblCurrNav.Text = _investmentInfo.CurrentNav;
            #endregion
            double num;
            if (double.TryParse(_investmentInfo.CurrentNav.ToString().Trim(), out num) && double.TryParse(_investmentInfo.PrevNav.ToString().Trim(), out num))
            {
                if (double.TryParse(_investmentInfo.PrevNav, out num))
                {
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
                }
            }
            else
            {
                imgArrow.Visible = false;
                lblIncrNav.Text = "";
            }

           
        }


        [System.Web.Services.WebMethod]
        public static List<AssetAlocation> assetAllocaton(int schemeIds)
        {
            return AllMethods.getAssetAllocationUsingFundId(schemeIds);
        }


        public void TopCompanys(int schemeId)
        {
            DataTable _TopComp = AllMethods.getTopCompany(schemeId, 10);

            _TopComp.Columns.Add("BarValue", typeof(string));
            var totalAsset = _TopComp.AsEnumerable().Where(x => Double.TryParse(x["Net_Asset"].ToString(), out double tryData)).Sum(x => Convert.ToDouble(x["Net_Asset"].ToString()));
            _TopComp.AsEnumerable().Where(x => Double.TryParse(x["Net_Asset"].ToString(), out double tryData)).ToList().ForEach(x => x["BarValue"] = Math.Round((((Convert.ToDouble(x["Net_Asset"].ToString())) / totalAsset) * 100), 0));
            double tryDatadbl = 0;
            var dtTopComp = (from v in _TopComp.AsEnumerable()
                             orderby v["Net_Asset"] descending
                             select (new
                             {
                                 CompName = v["CompName"],
                                 Sector_Name = v["Sector_Name"],
                                 Bar_Value = v["BarValue"],
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

            var rowCnt = 0;
            foreach (RepeaterItem item in TopCompDetails.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    if (Int32.TryParse(dtCompTop.Rows[rowCnt]["Bar_Value"].ToString(), out int tryData))
                    {
                        HtmlContainerControl mydiv = (HtmlContainerControl)item.FindControl("dvBarWidth");
                        mydiv.Attributes.Add("style", "width:" + tryData + "%;");
                        //do something
                    }
                    rowCnt++;
                }
            }

            rowCnt = 0;

            if (dtCompBottom5 != null && dtCompBottom5.Rows.Count > 0)
            {
                foreach (RepeaterItem item in TopCompDetails1.Items)
                {
                    if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                    {
                        if (Int32.TryParse(dtCompBottom5.Rows[rowCnt]["Bar_Value"].ToString(), out int tryData))
                        {
                            HtmlContainerControl mydiv = (HtmlContainerControl)item.FindControl("dvBarWidth1");
                            mydiv.Attributes.Add("style", "width:" + tryData + "%;");
                            //do something
                        }
                        rowCnt++;
                    }
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

            //int peerSchCnt = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ValueInvestPeerSchemeCount"]);
            DataSet ds = AllMethods.getSebiPeerPerformance(schemeId);
            DataTable _PeerSchemes = ds.Tables[0];
            //RankingDate = Convert.ToString(ds.Tables[1].Rows[0][1]);
            //var Data = _PeerSchemes.AsEnumerable().Where(x => Convert.ToInt32(x["SchemeId"]) == schemeId).Select(x => x).FirstOrDefault();
            //spn1mth.InnerText = Data["Per30Days"].ToString();
            //spn3mth.InnerText = Data["Per91Days"].ToString();
            //spn6mth.InnerText = Data["Per182Days"].ToString();
            //spn1yr.InnerText = Data["Per1Year"].ToString();
            //spn3yr.InnerText = Data["Per3Year"].ToString();
            //spnSinceInception.InnerText = Data["SI"].ToString();

            DataRow drSchemeData = _PeerSchemes.AsEnumerable().Where(x => Convert.ToInt32(x["SchemeId"].ToString()) == Convert.ToInt32(hdSchemeId.Value.ToString())).Select(x => x).FirstOrDefault();
            DataRow drSchemeDataToInsert = _PeerSchemes.NewRow();
            drSchemeDataToInsert.ItemArray = drSchemeData.ItemArray;
            _PeerSchemes.Rows.Remove(drSchemeData);
            _PeerSchemes.Rows.InsertAt(drSchemeDataToInsert, 0);
            _PeerSchemes.AcceptChanges();
            PeerPerformance.DataSource = _PeerSchemes;
            PeerPerformance.DataBind();

            HtmlContainerControl mydiv1 = (HtmlContainerControl)((RepeaterItem)PeerPerformance.Items[0]).FindControl("dvTrID");
            mydiv1.Attributes.Add("style", "background-color: #ebebeb;");

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

            dtOutput.AsEnumerable().ToList().ForEach(x => x["Bar_Value"] = Math.Round(((Convert.ToDouble(x["Scheme"].ToString()) / Sum) * 100), 0));


            var rowCnt = 0;
            foreach (RepeaterItem item in RepTopSector.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    if (Int32.TryParse(dtOutput.Rows[rowCnt]["Bar_Value"].ToString(), out int tryData))
                    {
                        HtmlContainerControl mydiv = (HtmlContainerControl)item.FindControl("dvBarWidthSect");
                        mydiv.Attributes.Add("style", "width:" + tryData + "%;");
                        //do something
                    }
                    rowCnt++;
                }
            }

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

        protected void LoadAMC()
        {
            DataTable dtFund;
            dtFund = AllMethods.getFundHouse();
            DataRow drFund = dtFund.NewRow();
            drFund["MutualFund_Name"] = "-Select MutualFund-";
            drFund["MutualFund_ID"] = -1;
            dtFund.Rows.InsertAt(drFund, 0);
            ddlFundHouse.DataSource = dtFund;
            ddlFundHouse.DataTextField = "MutualFund_Name";
            ddlFundHouse.DataValueField = "MutualFund_ID";
            ddlFundHouse.DataBind();

        }

        protected void ddlFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            int AMCID = Convert.ToInt32(ddlFundHouse.SelectedValue);

            LoadScheme(Convert.ToInt32(ddlFundHouse.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlType.SelectedValue), Convert.ToInt32(ddlSubNature.SelectedValue));
        }

        protected void LoadScheme(int AMCID = -1, int sebiNature = -1, int Type = -1 ,int sebiSubNature = -1)
        {
            var SchemeList = new DataTable();
            try
            {
                SchemeList = AllMethods.getSebiSchemeBluechip(Convert.ToInt32(ddlCategory.SelectedValue.ToString()), Convert.ToInt32(ddlType.SelectedValue.ToString()), Convert.ToInt32(ddlSubNature.SelectedValue.ToString()), 2, AMCID, false);
                var drFund = SchemeList.NewRow();
                drFund["Sch_Short_Name"] = "-Select Scheme-";
                drFund["Scheme_Id"] = -1;
                SchemeList.Rows.InsertAt(drFund, 0);
                ddlScheme.DataSource = SchemeList;
                ddlScheme.DataTextField = "Sch_Short_Name";
                ddlScheme.DataValueField = "Scheme_Id";
                ddlScheme.DataBind();

                if (!IsPostBack)
                {
                    if (Cache["dtSchemeMaster"] == null)
                    {
                        var dtResult = SchemeList.Copy();

                        Cache.Add("dtSchemeMaster", dtResult, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
                    }
                }
            }
            catch
            {
                SchemeList = new DataTable();
                ddlScheme.DataSource = SchemeList;
                ddlScheme.DataBind();

                Response.Write("<script>alert('No Scheme Found');</script>");
            }
        }

        protected void ddlScheme_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Convert.ToInt32(ddlScheme.SelectedValue) != -1)
            {
                Response.Redirect("/Finvoy/Factsheet.aspx?param=" + Convert.ToInt32(ddlScheme.SelectedValue));
            }
        }

        protected void LoadNature()
        {

            DataTable _dt = AllMethods.getSebiNature();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("Select All ", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlCategory.Items[0].Selected = true;


        }

        protected void LoadSubNature(int natureId = 0)
        {

            if (natureId == 0 || natureId == -1)
            {
                DataTable _dt = AllMethods.getAllSebiSubNature();
                ddlSubNature.Items.Clear();
                ddlSubNature.Items.Add(new ListItem("Select All ", "-1"));
                foreach (DataRow drow in _dt.Rows)
                {
                    ddlSubNature.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }

                ddlSubNature.Items[0].Selected = true;

            }
            else
            {
                DataTable _dt = AllMethods.getSebiSubNature(natureId);
                ddlSubNature.Items.Clear();
                ddlSubNature.Items.Add(new ListItem("Select All ", "-1"));
                foreach (DataRow drow in _dt.Rows)
                {
                    ddlSubNature.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }

                ddlSubNature.Items[0].Selected = true;
            }
        }

        protected void LoadStructure()
        {
            ddlFundHouse.ClearSelection();
            DataTable _dt = AllMethods.getStructure();
            ddlType.Items.Clear();
            ddlType.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlType.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlType.Items[0].Selected = true;
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            LoadSubNature(nature_id);
            LoadScheme(Convert.ToInt32(ddlFundHouse.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlType.SelectedValue), Convert.ToInt32(ddlSubNature.SelectedValue));
        }

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadScheme(Convert.ToInt32(ddlFundHouse.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlType.SelectedValue), Convert.ToInt32(ddlSubNature.SelectedValue));
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadScheme(Convert.ToInt32(ddlFundHouse.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlType.SelectedValue), Convert.ToInt32(ddlSubNature.SelectedValue));
        }


        protected void checkDefaultSelect()
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                {
                    var getMFID = AllMethods.getMFID(Convert.ToInt32(Request.QueryString["param"]));
                    ddlFundHouse.ClearSelection();
                    ddlFundHouse.Items.FindByValue(getMFID.ToString()).Selected = true;

                    var dtResult = new DataTable();
                    if (Cache["dtSchemeMaster"] != null)
                    {
                        dtResult = (DataTable)Cache["dtSchemeMaster"];
                    }
                    else
                    {
                        dtResult = AllMethods.getSebiSchemeBluechip(-1, -1, -1, 2, -1, false);
                    }
                    
                    if(dtResult != null && dtResult.Rows.Count>0)
                    {
                       
                        var sebiNature = dtResult.AsEnumerable().Where(x => x["Scheme_Id"].ToString() == Convert.ToString(Request.QueryString["param"])).Select(x => x["NATURE_ID"].ToString()).FirstOrDefault();
                        if (sebiNature != null)
                        {
                            ddlCategory.ClearSelection();
                            ddlCategory.Items.FindByValue(Convert.ToString(sebiNature)).Selected = true;

                            LoadSubNature(Convert.ToInt32(sebiNature));
                        }
                        else
                        {
                            sebiNature = "-1";
                        }
                        var sebiSubNature = dtResult.AsEnumerable().Where(x => x["Scheme_Id"].ToString() == Convert.ToString(Request.QueryString["param"])).Select(x => x["SubcategoryId"].ToString()).FirstOrDefault();
                        if (sebiSubNature != null)
                        {
                            ddlSubNature.ClearSelection();
                            ddlSubNature.Items.FindByValue(Convert.ToString(sebiSubNature)).Selected = true;
                        }
                        else
                        {
                            sebiSubNature = "-1";
                        }
                        var valType = dtResult.AsEnumerable().Where(x => x["Scheme_Id"].ToString() == Convert.ToString(Request.QueryString["param"])).Select(x => x["Structure_ID"].ToString()).FirstOrDefault();
                        if (valType != null)
                        {
                            ddlType.ClearSelection();
                            ddlType.Items.FindByValue(Convert.ToString(valType)).Selected = true;
                        }
                        else
                        {
                            valType = "-1";
                        }

                        LoadScheme(getMFID, Convert.ToInt32(sebiNature), Convert.ToInt32(valType), Convert.ToInt32(sebiSubNature));
                    }

                    ddlScheme.ClearSelection();
                    ddlScheme.Items.FindByValue(Convert.ToString(Request.QueryString["param"])).Selected = true;
                }
            }
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

                if (Ind > 0)
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