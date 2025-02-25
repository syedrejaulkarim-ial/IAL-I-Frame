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

namespace iFrames.BansalCapitalv2
{

    public partial class TopFund : System.Web.UI.Page
    {
        #region GlobalVar
        readonly string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        readonly SqlConnection conn = new SqlConnection();

        DataTable dtResultTopFund = new DataTable();

        private int CurrentPage
        {
            get
            {
                object objPage = ViewState["_CurrentPage"];
                int _CurrentPage = 0;
                if (objPage == null)
                {
                    _CurrentPage = 0;
                }
                else
                {
                    _CurrentPage = (int)objPage;
                }
                return _CurrentPage;
            }
            set { ViewState["_CurrentPage"] = value; }
        }
        private int fistIndex
        {
            get
            {
                int _FirstIndex = 0;
                if (ViewState["_FirstIndex"] == null)
                {
                    _FirstIndex = 0;
                }
                else
                {
                    _FirstIndex = Convert.ToInt32(ViewState["_FirstIndex"]);
                }
                return _FirstIndex;
            }
            set { ViewState["_FirstIndex"] = value; }
        }

        private int lastIndex
        {
            get
            {
                int _LastIndex = 0;
                if (ViewState["_LastIndex"] == null)
                {
                    _LastIndex = 0;
                }
                else
                {
                    _LastIndex = Convert.ToInt32(ViewState["_LastIndex"]);
                }
                return _LastIndex;
            }
            set { ViewState["_LastIndex"] = value; }
        }

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

            if (!IsPostBack)
            {
                //For Top Fund Page
                LoadNature();
                LoadAllSubNature();
                LoadStructure();
                loadOption();
                PopulateFundRiskButtons();
                PageLoadDataBind();
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
                LoadStructureNav();
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            ResultDataBind();

            //var dp = lstResult.FindControl("dpTopFund") as DataPager;
            //if (dp != null)
            //{
            //    dp.SetPageProperties(0, dp.MaximumRows, true);
            //}
            lstResult.Visible = true;
            Result.Visible = true;
        }

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
        protected void LoadSubNature(int id)
        {
            ddlSubCategory.ClearSelection();
            if (id == -1)
            {
                ddlSubCategory.Items[0].Selected = true;
                return;
            }
            DataTable _dt = AllMethods.getSebiSubNature(id);
            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Add(new ListItem("All", "-1"));
            //if (id == 3)
            //{
            //    ddlSubCategory.Items.Add(new ListItem("Large Cap", "9000"));
            //    ddlSubCategory.Items.Add(new ListItem("Mid & Small Cap", "9001"));
            //}
            if (_dt != null)
                foreach (DataRow drow in _dt.Rows)
                {
                    ddlSubCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }

            ddlSubCategory.Items[0].Selected = true;
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


        #region Fetch Data

        private void PageLoadDataBind()
        {
            DataTable dtResult = new DataTable();
            if (Cache["dtResult"] == null)
            {
                dtResult = AllMethods.getSebiTopFundRank(-1, -1, -1, "Per_1_Year", -1, 2, -1, 500, Convert.ToInt32(HiddenMinimumSIReturn.Value));
                if (dtResult != null)
                {
                    DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));

                    dtResult.Columns.Add(rankCol);

                    int i = 1;
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        dr["Rank"] = i;
                        i++;
                    }

                    dtResult.Columns.Add("CategoryAverage", typeof(decimal));

                    decimal x = 3.07m;
                    foreach (DataRow row in dtResult.Rows)
                    {
                        row["CategoryAverage"] = x;
                        x++;
                    }

                    dtResult.Columns.Add("CurrentNav");
                    dtResult.Columns.Add("LblIncrNav");
                    dtResult.Columns.Add("PrevNav");
                    dtResult.Columns.Add("LblHigh");
                    dtResult.Columns.Add("LblLow");
                    dtResult.Columns.Add("SpnMaxDate");
                    dtResult.Columns.Add("spnMinDate");
                    foreach (DataRow row in dtResult.Rows)
                    {
                        var SchId = Convert.ToInt32(row["SchemeId"]);
                        var _investmentInfo = AllMethods.getInvestInfo(SchId);
                        row["CurrentNav"] = _investmentInfo.CurrentNav;
                        row["PrevNav"] = _investmentInfo.PrevNav;
                        double num;
                        if (double.TryParse(_investmentInfo.CurrentNav.ToString().Trim(), out num) && double.TryParse(_investmentInfo.PrevNav.ToString().Trim(), out num))
                        {
                            double curNav = Convert.ToDouble(_investmentInfo.CurrentNav);
                            double PrevNav = Convert.ToDouble(_investmentInfo.PrevNav);
                            row["LblIncrNav"] = (Math.Round(curNav - PrevNav, 4)).ToString() + " (" +
                                            (Math.Round(100 * (curNav - PrevNav) / PrevNav, 2)).ToString() + "%)";
                        }

                        var rows = AllMethods.GetHighLowNav(SchId);
                        if (rows != null && rows.Any())
                        {
                            row["LblHigh"] = rows["max_nav"];
                            row["LblLow"] = rows["min_nav"];
                            row["spnMinDate"] = rows["min_nav_date"];
                            row["SpnMaxDate"] = rows["max_nav_date"];
                        }

                    }
                }
                Cache.Add("dtResult", dtResult, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtResult = (DataTable)Cache["dtResult"];
            }

            dtResultTopFund = dtResult;
            if (dtResultTopFund == null)
            {
                BindItemsList(new DataTable());
            }
            else
            {

                BindItemsList(dtResultTopFund);
            }

            if (dtResultTopFund != null)
            {
                lbtopText.Text = ddlRank.SelectedItem.Text + " the schemes on the basis of : " + ddlPeriod.SelectedItem.Text;
            }
            else
            {
                lbtopText.Text = "";
            }
        }

        private void ResultDataBind()
        {
            hdIsLoad.Value = "1";
            DataTable dtResult = new DataTable();             
            List<int> RiskList = new List<int>();

            RiskList = HiddenFundRisk.Value?.Split(',')?.Select(Int32.Parse)?.ToList();


            dtResult = AllMethods.getSebiTopFundRank1(Convert.ToInt32(ddlRank.SelectedItem.Value), Convert.ToInt32(ddlType.SelectedItem.Value),
                Convert.ToInt32(ddlCategory.SelectedItem.Value), ddlPeriod.SelectedItem.Value, Convert.ToInt32(ddlSubCategory.SelectedValue),
                Convert.ToInt32(rdbOption.SelectedValue), RiskList, Convert.ToInt32(HiddenMinimumInvesment.Value), Convert.ToInt32(HiddenMinimumSIReturn.Value));
            

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                dtResult.Columns.Add("CategoryAverage", typeof(decimal));

                decimal x = 3.07m;
                foreach (DataRow row in dtResult.Rows)
                {
                    row["CategoryAverage"] = x;
                    x++;
                }

                dtResult.Columns.Add("CurrentNav");
                dtResult.Columns.Add("LblIncrNav");
                dtResult.Columns.Add("PrevNav");
                dtResult.Columns.Add("LblHigh");
                dtResult.Columns.Add("LblLow");
                dtResult.Columns.Add("SpnMaxDate");
                dtResult.Columns.Add("spnMinDate");
                foreach (DataRow row in dtResult.Rows)
                {
                    var SchId = Convert.ToInt32(row["SchemeId"]);
                    var _investmentInfo = AllMethods.getInvestInfo(SchId);
                    row["CurrentNav"] = _investmentInfo.CurrentNav;
                    row["PrevNav"] = _investmentInfo.PrevNav;
                    double num;
                    if (double.TryParse(_investmentInfo.CurrentNav.ToString().Trim(), out num) && double.TryParse(_investmentInfo.PrevNav.ToString().Trim(), out num))
                    {
                        double curNav = Convert.ToDouble(_investmentInfo.CurrentNav);
                        double PrevNav = Convert.ToDouble(_investmentInfo.PrevNav);
                        row["LblIncrNav"] = (Math.Round(curNav - PrevNav, 4)).ToString() + " (" +
                                        (Math.Round(100 * (curNav - PrevNav) / PrevNav, 2)).ToString() + "%)";
                    }

                    var rows = AllMethods.GetHighLowNav(SchId);
                    if (rows != null && rows.Any())
                    {
                        row["LblHigh"] = rows["max_nav"];
                        row["LblLow"] = rows["min_nav"];
                        row["spnMinDate"] = rows["min_nav_date"];
                        row["SpnMaxDate"] = rows["max_nav_date"];
                    }

                }
                DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));

                dtResult.Columns.Add(rankCol);

                int i = 1;
                foreach (DataRow dr in dtResult.Rows)
                {
                    dr["Rank"] = i;
                    i++;
                }
                dtResultTopFund = dtResult;
                Cache["dtResult"] = dtResultTopFund;
                BindItemsList(dtResultTopFund);
                //PaginationForReapetar(dtResultTopFund);
                //lstResult.DataSource = dtResult;
            }
            else
            {
                Cache["dtResult"] = new DataTable();
                dtResultTopFund = new DataTable();
                BindItemsList(dtResultTopFund);
                //PaginationForReapetar(new DataTable());
                //lstResult.DataSource = new DataTable();
            }

            //lstResult.DataBind();

            if (dtResultTopFund != null)
            {
                lbtopText.Text = ddlRank.SelectedItem.Text + " schemes on the basis of : " + ddlPeriod.SelectedItem.Text;
            }
            else
            {
                lbtopText.Text = "";
            }
        }


        private void BindItemsList(DataTable dt)
        {
            PagedDataSource _PageDataSource = new PagedDataSource();
            DataTable dataTable = dt;
            _PageDataSource.DataSource = dataTable.DefaultView;
            _PageDataSource.AllowPaging = true;
            _PageDataSource.PageSize = 10;
            _PageDataSource.CurrentPageIndex = CurrentPage;
            ViewState["TotalPages"] = _PageDataSource.PageCount;
            lblPageInfo.Text = "Page " + (CurrentPage + 1) + " of " + _PageDataSource.PageCount;
            _lbtnPrevious.Enabled = !_PageDataSource.IsFirstPage;
            _lbtnNext.Enabled = !_PageDataSource.IsLastPage;
            _lbtnFirst.Enabled = !_PageDataSource.IsFirstPage;
            _lbtnLast.Enabled = !_PageDataSource.IsLastPage;
            lstResult.DataSource = _PageDataSource;
            lstResult.DataBind();
            doPaging();
        }

        private void doPaging()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            fistIndex = CurrentPage - 5;
            if (CurrentPage > 5)
            {
                lastIndex = CurrentPage + 5;
            }
            else
            {
                lastIndex = 10;
            }
            if (lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                fistIndex = lastIndex - 10;
            }
            if (fistIndex < 0)
            {
                fistIndex = 0;
            }
            for (int i = fistIndex; i < lastIndex; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }
            this.PgnData.DataSource = dt;
            this.PgnData.DataBind();
        }

        protected void PgnData_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("Paging"))
            {
                CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
                this.BindItemsList(dtResultTopFund);
            }
            //PaginationForReapetar(dtResultTopFund);
        }

        protected void PgnData_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
            if (lnkbtnPage.CommandArgument.ToString() == CurrentPage.ToString())
            {
                lnkbtnPage.Enabled = false;
                lnkbtnPage.Style.Add("fone-size", "14px");
                lnkbtnPage.Font.Bold = true;

            }
        }

        protected void _lbtnFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            dtResultTopFund = (DataTable)Cache["dtResult"];
            this.BindItemsList(dtResultTopFund);
        }

        protected void _lbtnPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            dtResultTopFund = (DataTable)Cache["dtResult"];
            this.BindItemsList(dtResultTopFund);
        }

        protected void _lbtnNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            dtResultTopFund = (DataTable)Cache["dtResult"];
            this.BindItemsList(dtResultTopFund);
        }

        protected void _lbtnLast_Click(object sender, EventArgs e)
        {
            CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
            dtResultTopFund = (DataTable)Cache["dtResult"];
            this.BindItemsList(dtResultTopFund);
        }


        #endregion

        protected void btnResetClick(object sender, EventArgs e)
        {
            ddlCategory.SelectedIndex = 0;
            ddlSubCategory.SelectedIndex = 0;

            ddlPeriod.SelectedIndex = 0;
            ddlRank.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            lstResult.Visible = false;
            Result.Visible = false;
            lbtopText.Text = "";
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string c = BtnClickedNo.Value;
            int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            LoadSubNature(nature_id);
        }

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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("/BansalCapital/TopFund.aspx");
            //LoadNature();
            //LoadAllSubNature();
            //LoadStructure();
            //loadOption();
            //PopulateFundRiskButtons();
            //PageLoadDataBind();
            //HiddenFundRiskStrColor.Value = "all";
            //HiddenFundRisk.Value = "-1";
        }
        
        protected void lstResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                
                Label lblIncrNav = e.Item.FindControl("lblIncrNav") as Label;
                Label ImgArrow = e.Item.FindControl("ImgArrow") as Label;
                var CurrNav = (e.Item.FindControl("CurrNav") as HiddenField).Value;
                var PrevNav = (e.Item.FindControl("PrevNav") as HiddenField).Value;
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
                else
                {
                    lblIncrNav.Text = "";
                }


                var prgBarValue = (e.Item.FindControl("hdfProgressBarWidth") as HiddenField).Value; 
                HtmlGenericControl lblprgbar = e.Item.FindControl("lblprgbar") as HtmlGenericControl;
                
                Label lblprgbar1 = e.Item.FindControl("lblprgbar") as Label;
                lblprgbar1.Attributes.Add("style", "width:"+prgBarValue+"%");
                
            }
            
        }

        //------------------------------------------ Compare Fund Block ----------------------------------------------------
        #region Compare Fund Block
        private void getFundHouseCompare()
        {
            DataTable dtFund;
            if (Cache["dtFund"] == null)
            {
                dtFund = AllMethods.getFundHouse();
                DataRow drFund = dtFund.NewRow();
                drFund["MutualFund_Name"] = "- Select Mutual Fund -";
                drFund["MutualFund_ID"] = 0;
                dtFund.Rows.InsertAt(drFund, 0);
                Cache.Add("dtFund", dtFund, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtFund = (DataTable)Cache["dtFund"];
            }
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
            rdbOptionCompare.DataSource = OptionDataTable;
            rdbOptionCompare.DataTextField = "Name";
            rdbOptionCompare.DataValueField = "Id";
            rdbOptionCompare.DataBind();
            rdbOptionCompare.SelectedIndex = 0;
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

        protected void rdbOption_SelectedIndexChangedCompare(object sender, EventArgs e)
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
        public static string getSchemesListCompare(string ddlCategoryCompare, string ddlTypeCompare, string ddlSubCategoryCompare, string rdbOptionCompare, string ddlFundHouseCompare)
        {
            
            DataTable dtScheme = new DataTable();

            dtScheme = AllMethods.getSebiScheme(Convert.ToInt32(ddlCategoryCompare),
               Convert.ToInt32(ddlTypeCompare), Convert.ToInt32(ddlSubCategoryCompare),
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

                ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(CurrSchemeId1));
                ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(CurrSchemeId2));
                ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(CurrSchemeId3));
                ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(CurrSchemeId4));

                ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(CurrIndexId1));
                ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(CurrIndexId2));
                ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(CurrIndexId3));
                ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(CurrIndexId4));


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
            _dtFinal.Columns.Add("status");
            _dtFinal.Columns.Add("CurrentNav");
            _dtFinal.Columns.Add("LblIncrNav");
            _dtFinal.Columns.Add("PrevNav");



            if (strSch != string.Empty)
                _dtSch = AllMethods.getSebiFundComparisonWithSI(strSch);

            if (_dtFinal != null)
            {
                for (int i = 0; i < _dtSch.Rows.Count; i++)
                {
                    _dtFinal.Rows.Add(_dtSch.Rows[i].ItemArray);
                }
            }

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(strSch))
            {
                lstSchemeId = AllMethods.getFundSchemeId(strSch);
            }

            int x = 0;
            foreach (DataRow row in _dtFinal.Rows)
            {
                var SchId = Convert.ToInt32(lstSchemeId[x]);
                var _investmentInfo = AllMethods.getInvestInfo(SchId);
                row["CurrentNav"] = _investmentInfo.CurrentNav;
                row["PrevNav"] = _investmentInfo.PrevNav;
                double num;
                if (double.TryParse(_investmentInfo.CurrentNav.ToString().Trim(), out num) && double.TryParse(_investmentInfo.PrevNav.ToString().Trim(), out num))
                {
                    double curNav = Convert.ToDouble(_investmentInfo.CurrentNav);
                    double PrevNav = Convert.ToDouble(_investmentInfo.PrevNav);
                    row["LblIncrNav"] = (Math.Round(curNav - PrevNav, 4)).ToString() + " (" +
                                    (Math.Round(100 * (curNav - PrevNav) / PrevNav, 2)).ToString() + "%)";
                }

                x++;

            }

            _dtInd = CalculateIndexHistPerf(DateTime.Today.AddDays(-1), strIndex);


            if (_dtInd != null && _dtInd.Rows.Count > 0)
            {
                _dtInd.Columns.Add("Nav_Rs");
                _dtInd.Columns.Add("Nature");
                _dtInd.Columns.Add("Sub_Nature");
                _dtInd.Columns.Add("Option_Id");
                _dtInd.Columns.Add("Structure_Name");
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
            DataTable _dt = AllMethods.getStructure();
            ddlTypeNav.Items.Clear();
            ddlTypeNav.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlTypeNav.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlTypeNav.Items[0].Selected = true;
        }

        protected void loadOptionNav()
        {
            var OptionDataTable = AllMethods.getOption();
            rdbOptionNav.DataSource = OptionDataTable;
            rdbOptionNav.DataTextField = "Name";
            rdbOptionNav.DataValueField = "Id";
            rdbOptionNav.DataBind();
            rdbOptionNav.SelectedIndex = 0;
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

                    DataRow dr1 = dt.NewRow();
                    dr1["SCHEME_ID"] = string.Empty;
                    dr1["Sch_Short_Name"] = string.Empty;
                    dr1["INDEX_ID"] = IndId;
                    dr1["INDEX_NAME"] = IndName;
                    dt.Rows.Add(dr1);    
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