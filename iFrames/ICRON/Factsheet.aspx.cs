using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;

namespace iFrames.ICRON
{

    public partial class Factsheet : System.Web.UI.Page
    {
        string RankingDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                getFundHouse();
                if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                {
                    int SchemeId = AllMethods.getSchemeId(Request.QueryString["param"].ToString());

                    hdSchemeId.Value = SchemeId.ToString();
                    tblResult_div.Visible = true;
                    setInvestmentInfo(SchemeId);
                    TopCompanys(SchemeId);
                    SipReturns(SchemeId);
                    TopSector(SchemeId);
                }
                else
                {
                    tblResult_div.Visible = false;
                }
            }
        }

        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundHouse.SelectedIndex != 0)
            {
                hdSchemeId.Value = "0";
                getSchemesList(ddlFundHouse.SelectedItem.Value);
            }
            else
            {
                listboxSchemeName.Items.Clear();
            }
        }

        protected void listboxSchemeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listboxSchemeName.SelectedIndex != -1)
            //{
            //	TopCompanys(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
            //	PeerSchemes(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
            //}
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            tblResult_div.Visible = true;
            if (listboxSchemeName.SelectedIndex > 0)
            {
                setInvestmentInfo(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
                TopCompanys(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
                // PeerSchemes(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
                SipReturns(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
                TopSector(Convert.ToInt32(listboxSchemeName.SelectedItem.Value));
            }
            else
            {
                tblResult_div.Visible = false;
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
        private void getFundHouse()
        {
            DataTable dtFund;
            if (Cache["dtFund"] == null)
            {
                dtFund = AllMethods.getFundHouse();
                DataRow drFund = dtFund.NewRow(); 
                drFund["MutualFund_Name"] = "Select";
                drFund["MutualFund_ID"] = 0;
                dtFund.Rows.InsertAt(drFund, 0);
                Cache.Add("dtFund", dtFund, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtFund = (DataTable)Cache["dtFund"];
            }
                       
            ddlFundHouse.DataSource = dtFund;
            ddlFundHouse.DataTextField = "MutualFund_Name";
            ddlFundHouse.DataValueField = "MutualFund_ID";
            ddlFundHouse.DataBind();
        }

        private void getSchemesList(string fundId)
        {
            DataTable dtScheme = AllMethods.getScheme(true, false, fundId);
            DataRow drScheme = dtScheme.NewRow();
            //drScheme["Sch_Short_Name"] = "Select";
            //drScheme["Scheme_Id"] = 0;
            dtScheme.Rows.InsertAt(drScheme, 0);
            listboxSchemeName.DataSource = dtScheme;
            listboxSchemeName.DataTextField = "Sch_Short_Name";
            listboxSchemeName.DataValueField = "Scheme_Id";
            listboxSchemeName.DataBind();
            listboxSchemeName.SelectedIndex = 0;
        }

        [System.Web.Services.WebMethod]
        public static List<AssetAlocation> assetAllocaton(int schemeIds)
        {
            return AllMethods.getAssetAllocationUsingFundId(schemeIds);
        }

        [System.Web.Services.WebMethod]
        public static iFrames.BLL.ChartNavReturnModel NavChartData(string schemeIds)
        {
            var _NavChart = AllMethods.GetMFINav(Convert.ToDecimal(schemeIds), DateTime.Today.AddDays(-365), DateTime.Today);
            return _NavChart;
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

        public void PeerSchemes(int schemeId)
        {
            int peerSchCnt = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ValueInvestPeerSchemeCount"]);
            DataSet ds = AllMethods.getPeerPerformance(schemeId, peerSchCnt);
            DataTable _PeerSchemes = ds.Tables[0];
            RankingDate = Convert.ToString(ds.Tables[1].Rows[0][1]);
            //PeerPerformance.DataSource = _PeerSchemes;
            //PeerPerformance.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static List<PortfolioMktValue> portfolioMKT_Val(int schemeIds)
        {
            return AllMethods.getPortfolioAsset(schemeIds);
        }

        [System.Web.Services.WebMethod]
        public static BansalCapitalMcapAvgMaturity McapAndAvgMat(int SchId)
        {

            var vv = AllMethods.getMCapAvgMaturity(SchId);
            return vv;
        }
        public void TopSector(int schemeId)
        {
            RepTopSector.DataSource = AllMethods.getTopSector(schemeId);
            RepTopSector.DataBind();
        }
        protected void PeerPerformance_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                //Label headerRepeater = e.Item.FindControl("lbl_Rank_Head") as Label;
                //headerRepeater.Text = "ICRON Ranking (" + RankingDate + ")";
            }
        }

        public void SipReturns(int schemeId)
        {
            DataTable data = AllMethods.getSip4MutualfundWala(schemeId);
            if (data.Rows.Count > 0)
            {
                //var data = DbRet;
                RptCommonSipResult.DataSource = data;
                RptCommonSipResult.DataBind();
                RptCommonSipResult.Visible = true;
                //CommonResultGridView.Visible = false;
                //((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnSch")).Text = Convert.ToString(data.Rows[0]["Scheme_name"]);
                ((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnIndex")).Text = Convert.ToString(data.Rows[0]["Index_Name"]);
                ((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnAddIndex")).Text = Convert.ToString(data.Rows[0]["Additional_Index_Name"]);
            }
        }
    }
}