using iFrames.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace iFrames.BlueChip
{
    public partial class Portfolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            {
                if (!IsPostBack)
                {
                    LoadAMC();
                    LoadNature();
                    LoadSubNature();
                    LoadStructure();
                    LoadScheme();
                    checkDefaultSelect();

                    if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                    {
                        dvPortFolio.Visible = true;
                        hdSchemeId.Value = Request.QueryString["param"];
                        var latPortDate = AllMethods.getLatestPortfiloDateDT(hdSchemeId.Value);
                        if (latPortDate != null && latPortDate.Rows.Count > 0)
                        {
                            lblPortDate.Text = Convert.ToDateTime(latPortDate.Rows[0]["PORTDATE"].ToString()).ToString("dd/MM/yyyy");
                        }
                        //tblResult.Visible = true;
                        setInvestmentInfo(Convert.ToInt32(hdSchemeId.Value));
                        TopCompanys(Convert.ToInt32(hdSchemeId.Value));
                        //PeerSchemes(Convert.ToInt32(Request.QueryString["param"]));
                        TopSector(Convert.ToInt32(hdSchemeId.Value));
                        GetCreditRatingInstrumentBreakUp(Convert.ToInt32(hdSchemeId.Value));
                        GetOtherDataBlueChip(Convert.ToInt32(hdSchemeId.Value));
                    }
                    else
                    {

                    }
                }
            }

        }

        private void setInvestmentInfo(int schemeId)
        {
            #region Top Fund Details Section 
            var _investmentInfo = AllMethods.getInvestInfoNew(schemeId);
            lblFundNameBold.Text = _investmentInfo.FundName;
            lblCurrNav.Text = _investmentInfo.CurrentNav;
            lblCurrNavDate.Text = DateTime.TryParse(_investmentInfo.CurrentNavDate, out DateTime tryDate) ? tryDate.ToString("dd MMM yyyy") : "";
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

        public void TopCompanys(int schemeId)
        {
            if (!IsPostBack)
            {
                DataTable _TopComp = AllMethods.getTopCompany(schemeId, 0);

                _TopComp.Columns.Add("BarValue", typeof(string));
                var totalAsset = _TopComp.AsEnumerable().Where(x => Double.TryParse(x["Net_Asset"].ToString(), out double tryData)).Sum(x => Convert.ToDouble(x["Net_Asset"].ToString()));
                _TopComp.AsEnumerable().Where(x => Double.TryParse(x["Net_Asset"].ToString(), out double tryData)).ToList().ForEach(x => x["BarValue"] = Math.Round((((Convert.ToDouble(x["Net_Asset"].ToString())) / totalAsset) * 100), 0));
                double tryDatadbl = 0;
                var dtTopComp = (from v in _TopComp.AsEnumerable()
                                 orderby v["Net_Asset"] descending
                                 select (new
                                 {
                                     CompName = v["CompName"],
                                     Sector_Name = v["NatureName"],
                                     Bar_Value = v["BarValue"],
                                     Net_Asset = Double.TryParse(v["Net_Asset"].ToString(), out tryDatadbl) ? (tryDatadbl != 0) ? (String.Format("{0:#,0.00}", tryDatadbl)) + "%" : "-" : "-"
                                 }));

                var dtTopComp10 = dtTopComp.ToDataTable();
                TopCompDetails.DataSource = dtTopComp10;
                //dtCompTop = dtTopComp5.ToDataTable().Copy();
                TopCompDetails.DataBind();

                if (dtTopComp10 == null)
                {
                    btnShowMoreButton.Style.Add("display", "none");
                }
                else if (dtTopComp10 != null && dtTopComp10.Rows.Count <= 10)
                {
                    btnShowMoreButton.Style.Add("display", "none");
                    foreach (RepeaterItem item in TopCompDetails.Items)
                    {
                        if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                        {
                            HtmlContainerControl mydiv = (HtmlContainerControl)item.FindControl("cmpIdData");
                            //mydiv.Visible = true;
                            mydiv.Attributes["class"] = "";
                            //do something
                        }
                    }
                }
                else
                {
                    btnShowMoreButton.Visible = true;
                    int skip = 0;
                    foreach (RepeaterItem item in TopCompDetails.Items)
                    {
                        if (skip == 10)
                            break;
                        if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                        {
                            HtmlContainerControl mydiv = (HtmlContainerControl)item.FindControl("cmpIdData");
                            //mydiv.Visible = true;
                            mydiv.Attributes["class"] = "";
                            //do something
                            skip++;
                        }
                    }
                }
            }
        }


        public void TopSector(int schemeId)
        {
            var _dtOutPut = AllMethods.getTopSector(schemeId);
            double trydata = 0;
            var Sum = _dtOutPut.Where(x => Double.TryParse(x.Scheme.ToString(), out  trydata)).Sum(x => Convert.ToDouble(x.Scheme.ToString()));
            var dtOutput = _dtOutPut.Where(x => Double.TryParse(x.Scheme.ToString(), out  trydata)).OrderByDescending(x => x.Scheme).ToList().ToDataTable();


            var dtTopComp = (from v in dtOutput.AsEnumerable()
                             select (new
                             {
                                 Sector_Name = v["Sector_Name"],
                                 Scheme = v["Scheme"],
                                 Scheme_Per = Double.TryParse(v["Scheme"].ToString(), out trydata) ? (trydata != 0) ? (String.Format("{0:#,0.00}", trydata)) + "%" : "-" : "-"
                             }));

            dtOutput = dtTopComp.ToDataTable();

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

        public void ShowMoreButton_Click(object sender, EventArgs e)
        {
            int skip = 0;
            if (btnShowMoreButton.Text == "Detailed Portfolio")
            {
                foreach (RepeaterItem item in TopCompDetails.Items)
                {
                    if (skip >= 11)
                    {
                        if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                        {
                            HtmlContainerControl mydiv = (HtmlContainerControl)item.FindControl("cmpIdData");
                            mydiv.Visible = true;
                            //do something
                        }
                    }
                    skip++;
                }
                btnShowMoreButton.Text = "Show Less";
            }
            else if (btnShowMoreButton.Text == "Show Less")
            {
                foreach (RepeaterItem item in TopCompDetails.Items)
                {
                    if (skip >= 11)
                    {
                        if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                        {
                            HtmlContainerControl mydiv = (HtmlContainerControl)item.FindControl("cmpIdData");
                            mydiv.Visible = false;
                            //do something
                        }
                    }
                    skip++;
                }
                btnShowMoreButton.Text = "Detailed Portfolio";
            }
        }

        public void GetOtherDataBlueChip(int schemeId)
        {
            var Data = AllMethods.getPortfolioOthers(schemeId);
            var dtOtherData = new DataTable();
            if (Data != null)
            {
                Double tryData = 0;
                dtOtherData.Columns.Add("ColName", typeof(string));
                dtOtherData.Columns.Add("ColDate", typeof(string));
                dtOtherData.Columns.Add("ColData", typeof(string));
                if (Data.Ptr != null)
                {
                    var drOthData = dtOtherData.Rows.Add();
                    drOthData["ColName"] = "PTR";
                    drOthData["ColDate"] = Convert.ToDateTime(Data.Ptr.IMPORT_DATE).ToString("MMM yyyy");
                    drOthData["ColData"] = Double.TryParse(Data.Ptr.RATIO.ToString(), out tryData) ? (String.Format("{0:#,0.00}", tryData)) + "%" : "";
                }
                if (Data.Ytm != null)
                {
                    var drOthData = dtOtherData.Rows.Add();
                    drOthData["ColName"] = "YTM";
                    drOthData["ColDate"] = Convert.ToDateTime(Data.Ytm.Port_Date).ToString("MMM yyyy");
                    drOthData["ColData"] = Data.Ytm != null ? Double.TryParse(Data.Ytm.YTM_Value.ToString(), out tryData) ? tryData != 0 ? (String.Format("{0:#,0.00}", tryData)) + "%" : "-" : "-" : "-";
                }
                if (Data.AttributionAnalysis != null)
                {
                    var drOthData = dtOtherData.Rows.Add();
                    drOthData["ColName"] = "PE";
                    drOthData["ColDate"] = Convert.ToDateTime(Data.AttributionAnalysis.PORT_DATE).ToString("MMM yyyy");
                    drOthData["ColData"] = Data.AttributionAnalysis != null ? Double.TryParse(Data.AttributionAnalysis.PRICE_EARNING.ToString(), out tryData) ? Math.Round(tryData, 2) + " Times" : "-" : "-";

                    drOthData = dtOtherData.Rows.Add();
                    drOthData["ColName"] = "PB";
                    drOthData["ColDate"] = Convert.ToDateTime(Data.AttributionAnalysis.PORT_DATE).ToString("MMM yyyy");
                    drOthData["ColData"] = Data.AttributionAnalysis != null ? Double.TryParse(Data.AttributionAnalysis.PRICE_TO_BOOKVAL.ToString(), out tryData) ? Math.Round(tryData, 2) + " Times" : "-" : "-";
                }
            }
            ExtraData.DataSource = dtOtherData;
            ExtraData.DataBind();
        }

        public void GetCreditRatingInstrumentBreakUp(int schemeId)
        {
            var RatingInst = AllMethods.getCreditRatingInsBreakupBluechip(Convert.ToInt32(schemeId.ToString()));

            if (RatingInst != null)
            {
                if (RatingInst.LstCreditrating.Count() > 0)
                {
                    RepeaterRate.DataSource = RatingInst.LstCreditrating.ToDataTable();
                    RepeaterRate.DataBind();
                }

                if (RatingInst.LstInsBreakup.Count() > 0)
                {
                    RepeaterInst.DataSource = RatingInst.LstInsBreakup.ToDataTable();
                    RepeaterInst.DataBind();
                }
            }

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

        protected void LoadScheme(int AMCID = -1, int sebiNature = -1, int Type = -1, int sebiSubNature = -1)
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
                    LoadScheme(getMFID);
                    
                    var dtResult = new DataTable();
                    if (Cache["dtSchemeMaster"] != null)
                    {
                        dtResult = (DataTable)Cache["dtSchemeMaster"];
                    }
                    else
                    {
                        dtResult = AllMethods.getSebiSchemeBluechip(-1, -1, -1, 2, -1, false);
                    }

                    if (dtResult != null && dtResult.Rows.Count > 0)
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

        protected void ddlScheme_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Convert.ToInt32(ddlScheme.SelectedValue) != -1)
            {
                Response.Redirect("/BlueChip/Portfolio.aspx?param=" + Convert.ToInt32(ddlScheme.SelectedValue));
            }
        }

        #region WebMethods

        [System.Web.Services.WebMethod]
        public static List<AssetAlocation> assetAllocaton(int schemeIds)
        {
            return AllMethods.getAssetAllocationUsingFundId(schemeIds);
        }
        [System.Web.Services.WebMethod]
        public static List<PortfolioMktValue> portfolioMKT_Val(int schemeIds)
        {
            return AllMethods.getPortfolioAsset(schemeIds);
        }

        [System.Web.Services.WebMethod]
        public static BansalCapitalMcapAvgMaturity McapAndAvgMat(int SchId)
        {

            var vv = AllMethods.getMCapAvgMaturityWoutRebase(SchId);
            return vv;
        }

        #endregion
        
    }
}