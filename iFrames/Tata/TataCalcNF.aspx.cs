using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using iFrames.DAL;
using iFrames.BLL;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Text;

namespace iFrames.Tata
{
    public partial class TataCalcNF : System.Web.UI.Page
    {
        #region: Global Variable

        DateTime _fromDate;
        DateTime _toDate;
        DateTime _calDate;
        DataView _viewCal;
        DateTime _asOnDate;
        public DateTime allotmentdate;
        public DateTime navdate;
        Boolean alotdatval = false;
        readonly DataTable _dtReturn = new DataTable();
        static string sqlcon = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString.ToString();
        SqlConnection cn = new SqlConnection(sqlcon);
        SqlCommand cmd = null;
        string strSchid, strIndexid;
        public string startdate = string.Empty;
        public string strNature = string.Empty;
        string SubNatureId = string.Empty;
        bool ddlfundmanegerindexcheged = false;
        bool InceptionChanged = false;


        #endregion

        #region: Page Events

        protected void Page_Load(object sender, EventArgs e)
        {

            // ddlFunManager.Attributes["onchange"] = "selectAll()";
            // ddlFunManager.Attributes.Add("onChange", "return selectAll()");
            ddlFunManager.Attributes.Add("onChange", "CheckSelectAll()");
            lblMessage.Text = "";
            if (!IsPostBack)
            {
                FillNature();
                FillScheme();
                FillFundManager();
                Fillyear();
            }
        }

        #endregion

        #region: Dropdown Events

        protected void ddlNature_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillScheme();
        }
        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {

            FillScheme();
            #region :Comment
            //string selected_nature = ddlNature.SelectedValue;
            //string selected_option = ddlOption.SelectedValue;
            //try
            //{
            //    if (ddlNature.SelectedIndex != 0)
            //    {
            //        using (var scheme = new TataCalculatorDataContext())
            //        {
            //            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTER_tatas
            //                             where
            //                                  t_fund_masters.MUTUALFUND_ID == 31 &&
            //                             t_fund_masters.NATURE_ID ==
            //                             ((from t_schemes_natures in scheme.T_SCHEMES_NATURE_tatas
            //                               where
            //                               t_schemes_natures.Nature == selected_nature
            //                               select new
            //                               {
            //                                   t_schemes_natures.Nature_ID
            //                               }).First().Nature_ID)
            //                             select new
            //                             {
            //                                 t_fund_masters.FUND_ID
            //                             });


            //            DataTable dtt = null;
            //            if (fundtable.Count() > 0)
            //                dtt = fundtable.ToDataTable();

            //            DataTable dt2 = new DataTable();
            //            // DateTime yearBacktodaysdate = DateTime.Today.AddYears(-1);

            //            if (ddlOption.SelectedValue != "--")
            //            {
            //                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
            //                                     join T in fundtable
            //                                     on s.Fund_Id equals T.FUND_ID
            //                                     where s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 && s.Nav_Check != 2 && s.Option_Id.ToString() == selected_option
            //                                     //&& (s.Launch_Date <= yearBacktodaysdate)
            //                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
            //                                     on s.Scheme_Id equals tsi.SCHEME_ID
            //                                     //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
            //                                     orderby s.Sch_Short_Name
            //                                     select new
            //                                     {
            //                                         s.Sch_Short_Name,
            //                                         s.Scheme_Id
            //                                     }).Distinct();
            //                if (scheme_name_1.Count() > 0)
            //                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
            //                    //dt2 = scheme_name_1.ToDataTable();

            //            }
            //            else
            //            {
            //                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
            //                                     join T in fundtable
            //                                     on s.Fund_Id equals T.FUND_ID
            //                                     where s.Nav_Check != 2
            //                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
            //                                     on s.Scheme_Id equals tsi.SCHEME_ID
            //                                     // where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
            //                                     orderby s.Sch_Short_Name
            //                                     select new
            //                                     {
            //                                         s.Sch_Short_Name,
            //                                         s.Scheme_Id
            //                                     }).Distinct();
            //                if (scheme_name_1.Count() > 0)
            //                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
            //                    //dt2 = scheme_name_1.ToDataTable();
            //            }
            //            if (dt2.Rows.Count > 0)
            //            {
            //                dgSchemeList.Visible = true;
            //                dgSchemeList.DataSource = dt2;
            //                dgSchemeList.DataBind();
            //            }
            //            else
            //            {
            //                dgSchemeList.Visible = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        using (var scheme = new TataCalculatorDataContext())
            //        {
            //            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTER_tatas
            //                             where
            //                                  t_fund_masters.MUTUALFUND_ID == 31
            //                             //t_fund_masters.NATURE_ID ==
            //                             //((from t_schemes_natures in scheme.T_SCHEMES_NATURE_tatas
            //                             //  where
            //                             //  t_schemes_natures.Nature == selected_nature
            //                             //  select new
            //                             //  {
            //                             //      t_schemes_natures.Nature_ID
            //                             //  }).First().Nature_ID)
            //                             select new
            //                             {
            //                                 t_fund_masters.FUND_ID
            //                             });


            //            DataTable dtt = null;
            //            if (fundtable.Count() > 0)
            //                dtt = fundtable.ToDataTable();

            //            DataTable dt2 = new DataTable();
            //            //DateTime yearBacktodaysdate = DateTime.Today.AddYears(-1);

            //            if (ddlOption.SelectedValue != "--")
            //            {
            //                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
            //                                     join T in fundtable
            //                                     on s.Fund_Id equals T.FUND_ID
            //                                     where s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 && s.Nav_Check != 2 && s.Option_Id.ToString() == selected_option
            //                                     //&& (s.Launch_Date <= yearBacktodaysdate)
            //                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
            //                                     on s.Scheme_Id equals tsi.SCHEME_ID
            //                                     //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
            //                                     orderby s.Sch_Short_Name
            //                                     select new
            //                                     {
            //                                         s.Sch_Short_Name,
            //                                         s.Scheme_Id
            //                                     }).Distinct();
            //                if (scheme_name_1.Count() > 0)
            //                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
            //                    //dt2 = scheme_name_1.ToDataTable();

            //            }
            //            else
            //            {
            //                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
            //                                     join T in fundtable
            //                                     on s.Fund_Id equals T.FUND_ID
            //                                     where s.Nav_Check != 2
            //                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
            //                                     on s.Scheme_Id equals tsi.SCHEME_ID
            //                                     // where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
            //                                     orderby s.Sch_Short_Name
            //                                     select new
            //                                     {
            //                                         s.Sch_Short_Name,
            //                                         s.Scheme_Id
            //                                     }).Distinct();
            //                if (scheme_name_1.Count() > 0)
            //                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
            //                    //dt2 = scheme_name_1.ToDataTable();
            //            }
            //            if (dt2.Rows.Count > 0)
            //            {
            //                dgSchemeList.Visible = true;
            //                dgSchemeList.DataSource = dt2;
            //                dgSchemeList.DataBind();
            //            }
            //            else
            //            {
            //                dgSchemeList.Visible = false;
            //            }
            //        }
            //    }


            //    LiteralFinalReturns.Text = "";
            //    lnkbtnDownload.Visible = false;
            //}
            //catch (Exception exp)
            //{
            //}
            //finally
            //{
            //}

            ////ddlMode.SelectedIndex = 0;
            ////  ddBenchMark.Items.Clear();
            ////FundmanegerText.Text = "";
            #endregion
        }
        protected void ddlSchemeList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlFunManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlfundmanegerindexcheged = true;
            try
            {
                if (ddlFunManager.SelectedIndex != 0)
                {
                    using (var schemeFundMngr = new TataCalculatorDataContext())
                    {
                        if (ddlFunManager.SelectedValue != "--")
                        {
                            //var schenames = (from sm in schemeFundMngr.T_SCHEMES_MASTER_tatas
                            //                 join fm in schemeFundMngr.T_FUND_MASTER_tatas
                            //                 on sm.Fund_Id equals fm.FUND_ID
                            //                 where fm.FUND_MAN == ddlFunManager.SelectedValue
                            //                 && fm.MUTUALFUND_ID == 31
                            //                 select new
                            //                 {
                            //                     sm.Sch_Short_Name,
                            //                     sm.Scheme_Id
                            //                 }).Distinct();


                            var schenames = (from sm in schemeFundMngr.T_SCHEMES_MASTER_tatas
                                             join fm in schemeFundMngr.T_FUND_MASTER_tatas on sm.Fund_Id equals fm.FUND_ID
                                             join tcfm in schemeFundMngr.T_CURRENT_FUND_MANAGER_tatas on fm.FUND_ID equals tcfm.FUND_ID
                                             where fm.MUTUALFUND_ID == 31 && tcfm.LATEST_FUNDMAN == true && sm.Nav_Check == 3 && tcfm.FUNDMAN_ID == Convert.ToDecimal(ddlFunManager.SelectedValue)
                                             select new
                                             {
                                                 sm.Sch_Short_Name,
                                                 sm.Scheme_Id
                                             }).Distinct();

                            DataTable dt2 = new DataTable();
                            if (schenames.Count() > 0)
                            {
                                //dt2 = schenames.ToDataTable();
                                dt2 = schenames.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                                if (dt2.Rows.Count > 0)
                                {
                                    dgSchemeList.Visible = true;
                                    LiteralFinalReturns.Text = "";
                                    lnkbtnDownload.Visible = false;
                                    dgSchemeList.DataSource = dt2;
                                    dgSchemeList.DataBind();


                                    //foreach (DataGridItem row in dgSchemeList.Items)
                                    //{
                                    //    CheckBox cbSelectAll = (CheckBox)dgSchemeList.FindControl("idSelectAllCheckBox");
                                    //    if (cbSelectAll != null)
                                    //        cbSelectAll.Checked = true;
                                    //}


                                    //idSelectAllCheckBox.checked
                                    for (int i = 0; i < dgSchemeList.Items.Count; i++)
                                    {
                                        CheckBox cb = dgSchemeList.Items[i].FindControl("chkItem") as CheckBox;
                                        if (cb.Checked == false)
                                        {
                                            cb.Checked = true;
                                        }
                                        //   CheckBox cbSelectAll = dgSchemeList.FindControl("idSelectAllCheckBox") as CheckBox;

                                    }

                                }

                            }
                            else
                            {
                                dgSchemeList.Visible = false;
                            }

                        }
                        else
                        {
                            FillScheme();


                        }

                    }
                }
                else
                {
                    //dgSchemeList.Visible = false;
                    FillScheme();
                }
            }
            catch (Exception exc)
            {
            }
        }


        protected void dgItemdatabound(object sender, DataGridItemEventArgs e)
        {
            DataGrid tblDataGrid = (DataGrid)sender;
            if (tblDataGrid != null)
            {

                if (e.Item.ItemType == ListItemType.Header)
                {

                    CheckBox chkSelect = e.Item.FindControl("idSelectAllCheckBox") as CheckBox;
                    if (chkSelect != null && ddlfundmanegerindexcheged && ddlFunManager.SelectedIndex != 0)
                    {
                        chkSelect.Checked = true;
                    }

                }
            }


        }
        #endregion

        #region: Button Events

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        protected void btnLumpCal_Click(object sender, EventArgs e)
        {
            FetchreturnInNewFormat();
        }

        #endregion

        #region: Methods

        #region: Fill Methods Dropdown

        protected void FillNature()
        {
            try
            {
                using (var natureData = new TataCalculatorDataContext())
                {
                    var nature = from nat in natureData.T_SCHEMES_NATURE_tatas
                                 where nat.Nature != "N.A"
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
                        ddlNature.Items.Insert(0, new ListItem("--", "0"));
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
            }


        }
        protected void FillScheme()
        {
            try
            {
                string selected_nature = ddlNature.SelectedValue;
                strNature = ddlNature.SelectedValue;
                string selected_option = ddlOption.SelectedValue;
                //DateTime yearBacktodaysdate1 = DateTime.Today.AddYears(-1);
                try
                {

                    if (ddlNature.SelectedIndex != 0)
                    {
                        using (var scheme = new TataCalculatorDataContext())
                        {
                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTER_tatas
                                             where
                                                 t_fund_masters.MUTUALFUND_ID == 31 &&
                                             t_fund_masters.NATURE_ID ==
                                                 ((from t_schemes_natures in scheme.T_SCHEMES_NATURE_tatas
                                                   where
                                                       t_schemes_natures.Nature == selected_nature
                                                   select new
                                                   {
                                                       t_schemes_natures.Nature_ID
                                                   }).First().Nature_ID)
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID
                                             });

                            DataTable dtt = new DataTable();
                            if (fundtable.Count() > 0)
                                dtt = fundtable.ToDataTable();
                            DataTable dt2 = new DataTable();

                            if (ddlOption.SelectedIndex != 0)
                            {

                                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                     where s.Nav_Check == 3 && s.Option_Id.ToString() == selected_option //s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
                                                     //&& s.Launch_Date <= yearBacktodaysdate1
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
                                                     orderby s.Sch_Short_Name
                                                     select new
                                                     {
                                                         s.Sch_Short_Name,
                                                         s.Scheme_Id
                                                     }).Distinct();

                                if (scheme_name_1.Count() > 0)
                                {
                                    //dt2 = scheme_name_1.ToDataTable();
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                                }


                            }
                            else
                            {
                                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                     where s.Nav_Check == 3 //s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
                                                     //&& s.Launch_Date <= yearBacktodaysdate1
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     //where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
                                                     orderby s.Sch_Short_Name
                                                     select new
                                                     {
                                                         s.Sch_Short_Name,
                                                         s.Scheme_Id
                                                     }).Distinct();

                                if (scheme_name_1.Count() > 0)
                                {
                                    //dt2 = scheme_name_1.ToDataTable();
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                                }
                            }




                            if (dt2 != null)
                            {
                                if (dt2.Rows.Count > 0)
                                {
                                    dgSchemeList.Visible = true;
                                    dgSchemeList.DataSource = dt2;
                                    dgSchemeList.DataBind();
                                }
                            }
                            else
                            {
                                dgSchemeList.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        using (var scheme = new TataCalculatorDataContext())
                        {
                            var fundtable = (from t_fund_masters in scheme.T_FUND_MASTER_tatas
                                             where
                                             t_fund_masters.MUTUALFUND_ID == 31
                                             //t_fund_masters.NATURE_ID ==
                                             //    ((from t_schemes_natures in scheme.T_SCHEMES_NATURE_tatas
                                             //      //where
                                             //      //    t_schemes_natures.Nature == selected_nature
                                             //      select new
                                             //      {
                                             //          t_schemes_natures.Nature_ID
                                             //      }).First().Nature_ID)
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID
                                             });

                            DataTable dtt = new DataTable();
                            if (fundtable.Count() > 0)
                                dtt = fundtable.ToDataTable();

                            DataTable dt2 = new DataTable();
                            if (ddlOption.SelectedIndex != 0)
                            {
                                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                     where s.Nav_Check == 3 && s.Option_Id.ToString() == selected_option //s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
                                                     //&& s.Launch_Date <= yearBacktodaysdate1
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     //orderby s.Scheme_Name
                                                     //orderby s.Sch_Short_Name ascending//// where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
                                                     select new
                                                     {
                                                         s.Scheme_Id,
                                                         s.Sch_Short_Name
                                                     }).Distinct();
                                if (scheme_name_1.Count() > 0)
                                {
                                    //dt2 = scheme_name_1.ToDataTable();
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                                }

                            }
                            else
                            {

                                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                     where s.Nav_Check == 3 //s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
                                                     //&& s.Launch_Date <= yearBacktodaysdate1
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     //orderby s.Scheme_Name
                                                     //orderby s.Sch_Short_Name ascending//// where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
                                                     select new
                                                     {
                                                         s.Sch_Short_Name,
                                                         s.Scheme_Id
                                                     }).Distinct();

                                if (scheme_name_1.Count() > 0)
                                {
                                    //dt2 = scheme_name_1.ToDataTable();
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                                }
                            }

                            if (dt2 != null)
                            {
                                if (dt2.Rows.Count > 0)
                                {
                                    DataColumn[] pk = new DataColumn[] { dt2.Columns["Scheme_Id"] };
                                    dt2.PrimaryKey = pk;
                                    dt2.Rows.Remove(dt2.Rows.Find("8698"));
                                    DataColumn[] pkk = new DataColumn[] { dt2.Columns["Sch_Short_Name"] };
                                    dt2.PrimaryKey = pkk;
                                    //dt2.Rows.Add("Tata Monthly Income Fund - Monthly Dividend Individual", "9999999991");
                                    //dt2.Rows.Add("Tata Monthly Income Fund - Monthly Dividend Corporate", "9999999992");

                                    dt2.Rows.Add("Tata Monthly Income Fund (TMIF) - Individual & HUF - Monthly Income Option", "9999999991");
                                    dt2.Rows.Add("Tata Monthly Income Fund (TMIF) - Other than Individual & HUF - Monthly Income Option", "9999999992");

                                    DataView dv = dt2.AsDataView();
                                    dv.Sort = "Sch_Short_Name asc";
                                    dt2 = dv.ToTable();

                                    dgSchemeList.Visible = true;
                                    dgSchemeList.DataSource = dt2;
                                    dgSchemeList.DataBind();
                                }
                            }
                            else
                            {
                                dgSchemeList.Visible = false;
                            }
                        }
                    }




                }
                catch (Exception exp)
                {
                }
                finally
                {
                    //ddlOption.SelectedIndex = 0;
                    lnkbtnDownload.Visible = false;
                    LiteralFinalReturns.Text = "";
                    //ddlMode.SelectedIndex = 0;
                    //FundmanegerText.Text = "";
                    // ddBenchMark.Items.Clear();

                    //if (LsResultDiv.Visible == true)
                    //LsResultDiv.Visible = false;
                    //if (showResult.Visible == true)
                    //showResult.Visible = false;
                }
            }
            catch (Exception ex)
            {


            }
        }
        protected void FillFundManager()
        {
            try
            {
                using (var FundmanagerData = new TataCalculatorDataContext())
                {
                    //var FundManager = from nat in FundmanagerData.T_FUND_MANAGER_tatas
                    //                  orderby nat.FUND_MANAGER_NAME
                    //                  select new
                    //                  {
                    //                      nat.FUND_CODE,
                    //                      nat.FUND_MANAGER_NAME
                    //                  };


                    //var FundManager = (from fm in FundmanagerData.T_FUND_MASTER_tatas
                    //                   where
                    //                   (
                    //                   from cfm in FundmanagerData.T_CURRENT_FUND_MANAGER_tatas
                    //                   join
                    //                       sm in FundmanagerData.T_SCHEMES_MASTER_tatas
                    //                     on cfm.FUND_ID equals sm.Fund_Id
                    //                   where cfm.LATEST_FUNDMAN == true && sm.Nav_Check != 2
                    //                   select cfm.FUND_ID
                    //                     ).Contains(fm.FUND_ID) && fm.FUND_MAN != string.Empty
                    //                       && fm.MUTUALFUND_ID == 31
                    //                   orderby fm.FUND_MAN
                    //                   select new
                    //                   {
                    //                       fm.FUND_MAN
                    //                   }).Distinct();

                    var FundManager = (from tfm in FundmanagerData.T_FUND_MANAGER_tatas
                                       join cfm in FundmanagerData.T_CURRENT_FUND_MANAGER_tatas on tfm.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                       join fm in FundmanagerData.T_FUND_MASTER_tatas on cfm.FUND_ID equals fm.FUND_ID
                                       join sm in FundmanagerData.T_SCHEMES_MASTER_tatas on fm.FUND_ID equals sm.Fund_Id
                                       where
                                         fm.MUTUALFUND_ID == 31 && sm.Nav_Check == 3 && cfm.LATEST_FUNDMAN == true
                                       orderby tfm.FUND_MANAGER_NAME
                                       select new
                                       {
                                           tfm.FUND_MANAGER_NAME,
                                           tfm.FUNDMAN_ID
                                       }).Distinct();


                    if (FundManager.Count() > 0)
                    {
                        DataTable dtFundManager = null;
                        if (FundManager.Count() > 0)
                        {
                            dtFundManager = FundManager.ToDataTable();
                            dtFundManager = dtFundManager.OrderBy("FUND_MANAGER_NAME");
                            ddlFunManager.DataSource = dtFundManager;
                            ddlFunManager.DataTextField = "FUND_MANAGER_NAME";
                            //ddlFunManager.DataTextField = "FUND_MAN";
                            // ddlFunManager.DataValueField = "FUND_CODE";
                            //ddlFunManager.DataValueField = "FUND_MAN";
                            ddlFunManager.DataValueField = "FUNDMAN_ID";
                            ddlFunManager.DataBind();
                            ddlFunManager.Items.Insert(0, new ListItem("--", "--"));
                        }
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
            }


        }

        protected void Fillyear()
        {

            ///year
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            ddlYearEnd.Items.Clear();
            for (int i = year - 25, k = 0; i < year + 3; i++, k++)
            {
                ddlYearEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == year)
                    ddlYearEnd.Items[k].Selected = true;
            }


            ///month
            ///
            ddlQtrEnd.Items.Clear();
            ddlQtrEnd.Items.Add(new ListItem("31st January", "01-31-"));
            if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
                ddlQtrEnd.Items.Add(new ListItem("29th February", "02-29-"));
            else
                ddlQtrEnd.Items.Add(new ListItem("28th February", "02-28-"));
            ddlQtrEnd.Items.Add(new ListItem("31st March", "03-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th April", "04-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st May", "05-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th June", "06-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st July", "07-31-"));
            ddlQtrEnd.Items.Add(new ListItem("31st August", "08-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th September", "09-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st October", "10-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th November", "11-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st December", "12-31-"));


            if (month != 1)
            {
                foreach (ListItem li in ddlQtrEnd.Items)
                {
                    if (Convert.ToInt32(li.Value.Split('-')[0]) == month - 1)
                        li.Selected = true;
                }
            }

            if (month == 1)
            {
                ddlYearEnd.Items[ddlYearEnd.SelectedIndex].Selected = false;
                //ddlYearEnd.Items[ddlYearEnd.SelectedIndex - 1].Selected = true;
                ddlQtrEnd.Items[11].Selected = true;//              
                ddlYearEnd.Items.FindByValue((year - 1).ToString()).Selected = true;
            }


        }

        #endregion

        #region: Fetch Methods


        public void FetchreturnInNewFormat()
        {
            StringBuilder sbFinal = new StringBuilder();
            double amountLs;
            if (double.TryParse(txtInvestment.Text, out amountLs))
                amountLs = Convert.ToDouble(txtInvestment.Text);
            else
                amountLs = 10000;
            //DateTime _todate = Convert.ToDateTime(ddlQtrEnd.SelectedValue + ddlYearEnd.SelectedValue);

            DateTime _todate = new DateTime(Convert.ToInt16(ddlYearEnd.SelectedValue.Split('-')[0]), Convert.ToInt16(ddlQtrEnd.SelectedValue.Split('-')[0]), Convert.ToInt16(ddlQtrEnd.SelectedValue.Split('-')[1]));
            _toDate = _todate;
            string val_SchemeIDs = string.Empty;
            List<decimal> lstschmId = new List<decimal>();
            List<decimal> lstIndexId = new List<decimal>();
            DataSet dsNewFormatFinal = new DataSet();
            lstschmId.Clear();
            lstIndexId.Clear();



            DataTable _finalDataTable = new DataTable();

            try
            {
                for (int i = 0; i < dgSchemeList.Items.Count; i++)
                {
                    CheckBox cb = dgSchemeList.Items[i].FindControl("chkItem") as CheckBox;
                    if (cb.Checked == true)
                    {
                        val_SchemeIDs += ((Label)dgSchemeList.Items[i].Cells[0].FindControl("lblSchemeID")).Text + ",";
                        lstschmId.Add(Convert.ToDecimal(((Label)dgSchemeList.Items[i].Cells[0].FindControl("lblSchemeID")).Text));
                    }
                }



                foreach (decimal deciml in lstschmId)
                {
                    //_finalDataTable.Copy( PerformanceReturn(deciml));
                    _finalDataTable.Merge(PerformanceReturn(deciml));
                    _finalDataTable.Rows.Add(_finalDataTable.NewRow());
                    //_finalDataTable.Rows.Add(_finalDataTable.NewRow());
                }

                _finalDataTable.Rows[_finalDataTable.Rows.Count - 1].Delete();
                //DataView dv = new DataView();
                //dv = _finalDataTable.DefaultView;                
                //dv.Sort = "SchemeIndex";
                //_finalDataTable = dv.ToTable();


                dsNewFormatFinal.Reset();
                dsNewFormatFinal.Tables.Add(_finalDataTable);


                sbFinal.Append(GenerateTable(dsNewFormatFinal, true, _todate));

                sbFinal.Append("<div style='clear:both;'></div>");
                lnkbtnDownload.Visible = true;
                LiteralFinalReturns.Text = sbFinal.ToString();

                #region :Disclaimer
                if (InceptionChanged)
                {
                    // LiteralDisclaimer.Text = "Disclaimer :<br/>";
                    //LiteralDisclaimer.Text = "<table width='100%'><tr><td colspan='6'></td></tr><tr><td align='left' colspan='6' background='../Images/heading.jpg' height='22' width='500'><span class='style12'>&nbsp;&nbsp;<span class='style4'><img src='../Images/arrow.jpg' /> Disclaimer</span></span><br/></td></tr><tr><td align='centre' style='font:verdena; font-size:11px; padding-left:20px;'>";
                    LiteralDisclaimer.Text = "";
                    foreach (decimal decimlschme in lstschmId)
                    {


                        switch (Convert.ToInt32(decimlschme))
                        {

                            case 9114:
                                LiteralDisclaimer.Text += "\u2022 On 16th April, 2008, the units had become zero under TFIPA3-RIP (Growth) plan and new units were alloted on 20th May 2008 at face value. Hence returns are computed from 20th May 2008.<br/>";
                                break;
                            case 9121:
                                LiteralDisclaimer.Text += "\u2022 On 31st December 2008, the units had become zero under TFIPB3-IP (Quarterly Dividend) plan and new units were alloted on 23rd March, 2010 at face value. Hence returns are computed from 23rd March, 2010.<br/>";
                                break;
                            case 9133:
                                LiteralDisclaimer.Text += "\u2022 On 24th October, 2008 units had become zero under TFIPC2 IP (Half Yearly Dividend) plan and new units were alloted on 21st January 2011 at face value. Hence returns are computed from 21st January 2011.<br/>";
                                break;
                            case 9105:
                                LiteralDisclaimer.Text += "\u2022 On 4th March, 2009 the units had become zero under TFIPA2-IP (Monthly Dividend) plan and new units were allotted on 4th June, 2010 at face value. Hence returns are computed from 4th June, 2010.<br/>";
                                break;
                            case 9116:
                                LiteralDisclaimer.Text += "\u2022 On 23 October 2008, the units had become zero under TFIPA3-IP (Growth) plan and new units were allotted on 09th June, 2010 at face value. Hence returns are computed from 09th June, 2010.<br/>";
                                break;
                            case 9100:
                                LiteralDisclaimer.Text += "\u2022 On 17th November, 2009, the units had become zero under TFIPB2-IP (Monthly Dividend) plan and new units were allotted on 18th June, 2010 at face value. Hence returns are computed from 18th June, 2010.<br/>";
                                break;
                            case 9140:
                                LiteralDisclaimer.Text += "\u2022 On 25th May, 2010 units had become zero under TFIPC3-IP (Growth) plan and new units were allotted on 26th May 2011 at face value. Hence returns are computed from 26th May 2011.<br/>";
                                break;
                            case 9122:
                                LiteralDisclaimer.Text += "\u2022 On 23rd December 2010, the units had become zero under TFIPB3-IP (Growth) plan and new units were allotted on 31st December, 2010 at face value. Hence returns are computed from 31st December, 2010.<br/>";
                                break;
                            case 9102:
                                LiteralDisclaimer.Text += "\u2022 On 15th December 2010, the units had become zero under TFIPB2-IP (Growth) plan and new units were allotted on 1st March, 2011 at face value. Hence returns are computed from 1st March, 2011.<br/>";
                                break;
                            case 9161:
                            case 9162:
                            case 17812:
                            case 17813:
                                LiteralDisclaimer.Text += "\u2022 MSCI Emerging Market Index to the extent of 70% of the net assets and S&P BSE Sensex to the extent of 30% of the net assets of the Plan.<br/>";
                                break;
                            case 9163:
                            case 9164:
                            case 17814:
                            case 17815:
                                LiteralDisclaimer.Text += "\u2022 S&P BSE Sensex to the extent of 65% of the net assets and MSCI Emerging Market Index to the extent of 35% of the net assets of the Plan.<br/>";
                                break;
                            case 9042:
                                LiteralDisclaimer.Text += "\u2022 No units were outstanding under TTMF SHIP Growth plan on 03 March 2009 and new units were allotted on 30 April 2009 at face value. Hence returns are computed from 30 April 2009.<br/>";
                                break;
                            case 9079:
                            case 9080:
                            case 17802:
                            case 17803:
                                LiteralDisclaimer.Text += "\u2022 Nifty 500 Index to the extent of 65% and MSCI World Index to the extent of 35% of the net assets of the Scheme.<br/>";
                                break;

                        }
                    }
                    LiteralDisclaimer.Text += "<br/>";

                }
                else
                    LiteralDisclaimer.Text = "";
                #endregion
            }
            catch (Exception ex)
            {
            }
        }

        private DataTable PerformanceReturn(decimal schmeId)
        {

            #region intialize variables
            // Take the End of Quarter Date
            DateTime LastQtrEndDate = _toDate;
            //string AdditionalBechmarkid = "12";// for the Nifty and for bse 100 it is 33 and bse sensex it is 13
            double? sinceInception = null; double? sinceInceptionIndex = null, sinceInceptionAddlIndex = null;
            string _indexId = string.Empty, _indexName = string.Empty, _additionalindexName = string.Empty, _indexNamestr = string.Empty;
            string datediff = string.Empty;
            datediff = "1 YYYY,0 Si";
            DateTime allotDate;
            bool isCompositeIndex = false;
            SqlCommand sqlcmd, sqlcmd1;
            SqlDataAdapter sqlDa = null;
            bool modifiedInceptionDate = false;
            bool showSpecificDisclaimer = false;
            int settingSet = 2;
            int settingSetAbs = 8;
            //string strSettingset = string.Empty;
            //strSettingset = radioList.SelectedItem.Text;
            //if (strSettingset.ToUpper() == "CORPORATE")
            //{
            //    settingSet = 26;
            //}
            //else
            //{
            //    settingSet = 2;
            //}


            //if (ddlSchemeList.SelectedItem.Text.ToUpper() == "TATA MONTHLY INCOME FUND - MONTHLY DIVIDEND CORPORATE")
            //{
            //    settingSet = 26;
            //}
            //else if (ddlSchemeList.SelectedItem.Text.ToUpper() == "TATA MONTHLY INCOME FUND - MONTHLY DIVIDEND INDIVIDUAL")
            //{
            //    settingSet = 2;
            //}

            if (schmeId == 9999999991)
            {
                settingSet = 2;
                schmeId = 8698;
            }
            else if (schmeId == 9999999992)
            {
                settingSet = 26;
                settingSetAbs = 27;
                schmeId = 8698;
            }


            //Creating New format datatable 
            DataTable newFrmtDataTable = new DataTable();
            newFrmtDataTable.Columns.Add("SchemeIndex", typeof(string));
            newFrmtDataTable.Columns.Add("FirstyearRs", typeof(string)); newFrmtDataTable.Columns.Add("FirstyearReturn", typeof(string));
            newFrmtDataTable.Columns.Add("SecondyearRs", typeof(string)); newFrmtDataTable.Columns.Add("SecondyearReturn", typeof(string));
            newFrmtDataTable.Columns.Add("ThirdyearRs", typeof(string)); newFrmtDataTable.Columns.Add("ThirdyearReturn", typeof(string));
            newFrmtDataTable.Columns.Add("CagrInceptionRs", typeof(string)); newFrmtDataTable.Columns.Add("CagrInceptionReturn", typeof(string));
            newFrmtDataTable.Columns.Add("InceptionDate", typeof(string));
            #endregion

            #region fetch Nature,Sub Nature,Index Name,Index Id,Launch Date
            using (var tata = new TataCalculatorDataContext())
            {
                var sbNtrid = (from tfm in tata.T_FUND_MASTER_tatas
                               where tfm.FUND_ID ==
                               (from tsm in tata.T_SCHEMES_MASTER_tatas
                                where tsm.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    tsm.Fund_Id
                                }
                               ).First().Fund_Id
                               select new { tfm.SUB_NATURE_ID }).First().SUB_NATURE_ID;
                SubNatureId = sbNtrid.ToString();// Corresponding Sub NatureId is fetched

                //var indexid = (from tsm in tata.T_SCHEMES_INDEX_tatas
                //               where tsm.SCHEME_ID == Convert.ToDecimal(schmeId)
                //               select tsm.INDEX_ID).First();

                //_indexId = indexid.ToString();//  Corresponding Index Id is fetched


                //var IndexName = (from tim in tata.T_INDEX_MASTER_tatas
                //                 where tim.INDEX_ID == indexid
                //                 select tim.INDEX_NAME).First();
                //_indexName = IndexName.ToString(); //  Corresponding Index Name is fetched


                var index_name = (from t_index_masters in tata.T_INDEX_MASTER_tatas
                                  where
                                      ((from t_schemes_indexes in tata.T_SCHEMES_INDEX_tatas
                                        where
                                          t_schemes_indexes.SCHEME_ID == Convert.ToDecimal(schmeId)
                                        select t_schemes_indexes.INDEX_ID
                                        ).Contains(t_index_masters.INDEX_ID))
                                  select new
                                  {

                                      t_index_masters.INDEX_NAME,
                                      t_index_masters.INDEX_ID
                                  });

                if (index_name.Count() == 1)
                {
                    _indexName = index_name.Single().INDEX_NAME.ToString();
                    _indexId = index_name.Single().INDEX_ID.ToString();
                }
                else
                {
                    foreach (var r in index_name)
                    {
                        _indexName = r.INDEX_NAME;
                        _indexId = r.INDEX_ID.ToString();
                    }
                }




                var _Launchdate = (from tsm in tata.T_SCHEMES_MASTER_tatas
                                   where tsm.Scheme_Id == Convert.ToDecimal(schmeId)
                                   select tsm.Launch_Date).First();

                //if(_Launchdate != null)
                allotDate = Convert.ToDateTime(_Launchdate); //  Corresponding Allotment Date is fetched


                //if (schmeId == 9042)// Specal case of Tata Treasury Manager Fund - SHIP - Growth SI need to show -30 April 2009
                //    allotDate = new DateTime(2009, 4, 30);
                //else if (schmeId == 9116)
                //    allotDate = new DateTime(2008, 5, 20);

                switch (Convert.ToInt32(schmeId))
                {
                    case 9042:
                        allotDate = new DateTime(2009, 4, 30);
                        modifiedInceptionDate = true;
                        break;
                    case 9114:
                        allotDate = new DateTime(2008, 5, 20);
                        modifiedInceptionDate = true;
                        break;
                    case 9116:
                        allotDate = new DateTime(2010, 6, 09);
                        modifiedInceptionDate = true;
                        break;
                    case 9121:
                        allotDate = new DateTime(2010, 3, 23);
                        modifiedInceptionDate = true;
                        break;
                    case 9133:
                        allotDate = new DateTime(2011, 1, 21);
                        modifiedInceptionDate = true;
                        break;
                    case 9105:
                        allotDate = new DateTime(2010, 6, 04);
                        modifiedInceptionDate = true;
                        break;
                    case 9100:
                        allotDate = new DateTime(2010, 6, 18);
                        modifiedInceptionDate = true;
                        break;
                    case 9140:
                        allotDate = new DateTime(2011, 5, 26);
                        modifiedInceptionDate = true;
                        break;
                    case 9122:
                        allotDate = new DateTime(2010, 12, 31);
                        modifiedInceptionDate = true;
                        break;
                    case 9102:
                        allotDate = new DateTime(2011, 3, 01);
                        modifiedInceptionDate = true;
                        break;
                    case 9101:
                        allotDate = new DateTime(2010, 6, 18);
                        modifiedInceptionDate = true;
                        break;
                    case 9139:
                        allotDate = new DateTime(2011, 5, 25);
                        modifiedInceptionDate = true;
                        break;
                    case 9163:
                    case 9164:
                    case 17814:
                    case 17815:
                        showSpecificDisclaimer = true;
                        break;
                    case 9161:
                    case 9162:
                    case 17812:
                    case 17813:
                        showSpecificDisclaimer = true;
                        break;
                    case 9079:
                    case 9080:
                    case 17802:
                    case 17803:
                        showSpecificDisclaimer = true;
                        break;
                }

                if (modifiedInceptionDate || showSpecificDisclaimer)
                    InceptionChanged = true;
                //

                var strNaturedata = (from natr in tata.T_SCHEMES_NATURE_tatas
                                     where natr.Nature_ID == (
                                     from tfm in tata.T_FUND_MASTER_tatas
                                     where tfm.FUND_ID ==
                                     (from tsm in tata.T_SCHEMES_MASTER_tatas
                                      where tsm.Scheme_Id == Convert.ToDecimal(schmeId)
                                      select new
                                      {
                                          tsm.Fund_Id
                                      }
                                     ).First().Fund_Id
                                     select new { tfm.NATURE_ID }).First().NATURE_ID
                                     select new
                                     {
                                         natr.Nature
                                     }).First().Nature;

                strNature = strNaturedata.ToString();////  Corresponding Index Name is fetched

            }
            #endregion


            #region Composite Index
            // ####### case for composite Index  ###########
            if (schmeId == 9161 || schmeId == 9162 || schmeId == 9163 || schmeId == 9164 || schmeId == 17812 || schmeId == 17813 || schmeId == 17814 || schmeId == 17815)
            {
                _indexId = "13,3T"; isCompositeIndex = true;
            }
            if (schmeId == 9079 || schmeId == 9080 || schmeId == 17802 || schmeId == 17803)
            {
                _indexId = "15,2T"; isCompositeIndex = true;
            }

            // Special Case for Tata SIP Fund 3
            if (schmeId == 13530 || schmeId == 13531)
            {
                //isCompositeIndex = true;
                _indexId = "4T";

            }

            

            #endregion

            try
            {

                int oval = 0;
                string IndexBenchmarlIds = null;
                bool isAdditionalBenchMark = false;
                string SchemeName = string.Empty;
                DataTable nwdataTble = new DataTable(), nwIndexDataTble = new DataTable(), oIndexDataTble = new DataTable();
                Double? schemeInception = null, indexInception = null, addlindexInception = null;
                List<object> listschemeval = new List<object>(); List<object> listbenchval = new List<object>(); List<object> listadbenchval = new List<object>();
                string strdaydiff = string.Empty; string strdaydiffColumn = string.Empty;
                string strIndexBenchmarlIds = string.Empty;
                Double? CompundReturnSI = null, CompundReturnSIndex = null, CompundReturnSIndexaddl = null;
                int amount; int day;


                if (Int32.TryParse(txtInvestment.Text.Split('.')[0], out amount))
                    amount = Convert.ToInt32(txtInvestment.Text.Split('.')[0]);
                else
                    amount = 10000;


                #region Adding Additional Benchmark part

                if (strNature == "Equity" || strNature == "Balanced" || strNature == "Dynamic/Asset Allocation")
                {

                    if (_indexName.ToUpper().Contains("BSE"))
                    {
                        if (_indexName.ToUpper() != "S&P BSE SENSEX")
                        {
                            isAdditionalBenchMark = true;
                            IndexBenchmarlIds = _indexId + ",13";
                        }
                        else
                        {
                            IndexBenchmarlIds = _indexId;
                        }

                    }
                    else
                    {
                        if (_indexName.ToUpper() != "NIFTY 50" || _indexId != "12")
                        {
                            //special case for Tata Indo Global Infrastructure Fund do not require additional benchmark
                            if (schmeId != 9079 && schmeId != 9080 && schmeId != 9161 && schmeId != 9162 && schmeId != 9163 && schmeId != 9164 && schmeId != 13530 && schmeId != 13531 && schmeId != 17812 && schmeId != 17813 && schmeId != 17814 && schmeId != 17815 && schmeId != 17802 && schmeId != 17803)
                            {
                                isAdditionalBenchMark = true;
                                IndexBenchmarlIds = _indexId + ",12";
                            }
                            else
                            {
                                IndexBenchmarlIds = _indexId;
                            }
                        }
                        else
                        {
                            IndexBenchmarlIds = _indexId;
                        }
                    }

                }

                else
                {
                    isAdditionalBenchMark = true;
                    // logic for long term debt scheme ----crisil 10 yr gilt
                    // for short term debt scheme ---------crisil 1 year t-bill
                    //SubNatureId == "41" for short term 
                    //SubNatureId == "15") for floating rate


                    if (SubNatureId == "13" || strNature == "Liquid" || SubNatureId == "41" || SubNatureId == "15" || SubNatureId == "2")
                    {
                        IndexBenchmarlIds = _indexId + ",135";

                    }
                    else //if (SubNatureId == "4")
                    {
                        IndexBenchmarlIds = _indexId + ",134";

                    }

                }

                // Special case if Crisil Balanced Fund Index is there No additional benchmark are required
                if (_indexId.Trim() == "27")
                {
                    isAdditionalBenchMark = false;
                    IndexBenchmarlIds = _indexId;
                }
                //if (schmeId == 13530 || schmeId == 13531)
                //{
                //    isAdditionalBenchMark = true;
                //    _indexId = "28,13";
                //    IndexBenchmarlIds = _indexId;
                //}

                // code on 02.07.2013 
                if (schmeId == 15140 || schmeId == 15442)
                {
                    isAdditionalBenchMark = true;
                    _indexId = "25,134";
                    IndexBenchmarlIds = _indexId;
                }


                /// changes for Isec - composite
                /// 

                strIndexBenchmarlIds = IndexBenchmarlIds;

                if (_indexId.Trim() == "32")
                {
                    //strIndexBenchmarlIds = strIndexBenchmarlIds.Replace("32", "1T");
                    strIndexBenchmarlIds = IndexBenchmarlIds.Replace("32", "1T");
                }



                #endregion


                TimeSpan _noofday = LastQtrEndDate.Subtract(allotDate);
                day = _noofday.Days;
                strdaydiff = _noofday.Days.ToString() + " D";
                strdaydiffColumn = strdaydiff + "ay";
                listschemeval.Clear(); listbenchval.Clear(); listadbenchval.Clear();

                #region Loop Start For last 3 years Calculation, Will set listschemeval and listbenchval ,listadbenchval

                for (int i = 0; i < 3; i++)
                {
                    //########## MFIE_SP_SCHEME_P2P_ROLLING_RETURN  #################
                    sqlcmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_TATA", cn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 2000;
                    sqlcmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
                    //sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                    sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSetAbs));
                    sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                    sqlcmd.Parameters.Add(new SqlParameter("@DateTo", (DateTime.IsLeapYear(LastQtrEndDate.AddYears(-i).Year) == true && LastQtrEndDate.AddYears(-i).Month == 2 && LastQtrEndDate.AddYears(-i).Day==28) ? LastQtrEndDate.AddYears(-i).AddDays(1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : LastQtrEndDate.AddYears(-i).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                    sqlcmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                    sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriodin", datediff));
                    sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
                    sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                    sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
                    sqlcmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                    sqlcmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


                    //########## MFIE_SP_INDEX_P2P_ROLLING_RETURN  #################
                    sqlcmd1 = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_TATA", cn);
                    sqlcmd1.CommandType = CommandType.StoredProcedure;
                    sqlcmd1.CommandTimeout = 2000;
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndexIDs", strIndexBenchmarlIds));
                    //sqlcmd1.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                    sqlcmd1.Parameters.Add(new SqlParameter("@SettingSetID", settingSetAbs));
                    sqlcmd1.Parameters.Add(new SqlParameter("@DateFrom", ""));
                    sqlcmd1.Parameters.Add(new SqlParameter("@DateTo", (DateTime.IsLeapYear(LastQtrEndDate.AddYears(-i).Year) == true && LastQtrEndDate.AddYears(-i).Month == 2 && LastQtrEndDate.AddYears(-i).Day == 28) ? LastQtrEndDate.AddYears(-i).AddDays(1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : LastQtrEndDate.AddYears(-i).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                    sqlcmd1.Parameters.Add(new SqlParameter("@RoundTill", 2));
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", datediff));
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
                    sqlcmd1.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                    sqlcmd1.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


                    nwdataTble.Reset();
                    sqlDa = new SqlDataAdapter(sqlcmd);
                    sqlDa.Fill(nwdataTble);// scheme

                    nwIndexDataTble.Reset();
                    sqlDa.SelectCommand = sqlcmd1;
                    sqlDa.Fill(nwIndexDataTble);// index                    

                    IndexBenchmarlIds = strIndexBenchmarlIds.Replace("T", "");


                    if (nwdataTble.Rows[0]["1 Year"].ToString() != "N/A")
                        listschemeval.Add(Convert.ToDouble(nwdataTble.Rows[0]["1 Year"]));
                    else
                        listschemeval.Add("N/A");

                    IEnumerable<DataRow> _testdtIndex = from ib in IndexBenchmarlIds.Split(',').AsEnumerable()
                                                        join tst in nwIndexDataTble.AsEnumerable()
                                                        on ib equals tst.Field<decimal>("Index_id").ToString()
                                                        select tst;
                    DataTable _dttt = new DataTable("tblIndex");
                    _dttt = _testdtIndex.CopyToDataTable<DataRow>();
                    nwIndexDataTble.Clear();
                    nwIndexDataTble = _dttt;

                    if (isCompositeIndex)
                        _indexName = nwIndexDataTble.Rows[0]["INDEX_NAME"].ToString() + " & " + nwIndexDataTble.Rows[1]["INDEX_NAME"].ToString();
                    if (schmeId == 13530 || schmeId == 13531)
                        _indexName = nwIndexDataTble.Rows[0]["INDEX_NAME"].ToString();

                    if (!isAdditionalBenchMark)
                    {

                        if (isCompositeIndex)
                        {
                            if (nwIndexDataTble.Rows[0]["1 Year"].ToString() != "N/A" && nwIndexDataTble.Rows[1]["1 Year"].ToString() != "N/A")
                                listbenchval.Add(CompositeReturn(Convert.ToInt32(schmeId), Convert.ToDouble(nwIndexDataTble.Rows[0]["1 Year"]), Convert.ToDouble(nwIndexDataTble.Rows[1]["1 Year"])));
                            else
                                listbenchval.Add("N/A");
                        }
                        else
                        {
                            if (nwIndexDataTble.Rows[0]["1 Year"].ToString() != "N/A")
                                listbenchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[0]["1 Year"]));
                            else
                                listbenchval.Add("N/A");
                        }
                    }
                    else
                    {
                        if (isCompositeIndex)
                        {
                            var indxfirst = (from t in nwIndexDataTble.AsEnumerable()
                                             where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                                             select new
                                             {
                                                 NAV = (t != null) ? t.Field<object>("1 Year") : "N/A"
                                             }).First();

                            var indxSecond = (from t in nwIndexDataTble.AsEnumerable()
                                              where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[1])
                                              select new
                                              {
                                                  NAV = (t != null) ? t.Field<object>("1 Year") : "N/A"
                                              }).First();

                            if (indxfirst.NAV.ToString() != "N/A" && indxSecond.NAV.ToString() != "N/A")
                                listbenchval.Add(CompositeReturn(Convert.ToInt32(schmeId), Convert.ToDouble(indxfirst.NAV), Convert.ToDouble(indxSecond.NAV)));
                            else
                                listbenchval.Add("N/A");

                            //for additional benchmark

                            var indx2 = (from t in nwIndexDataTble.AsEnumerable()
                                         where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[2])
                                         select new
                                         {
                                             NAV = (t != null) ? t.Field<object>("1 Year") : "N/A"
                                         }).First();
                            if (indx2.NAV.ToString() != "N/A")
                                listadbenchval.Add(Convert.ToDouble(indx2.NAV));
                            else
                                listadbenchval.Add("N/A");

                        }
                        else
                        {


                            var indx = (from t in nwIndexDataTble.AsEnumerable()
                                        where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0].ToString().Replace("t", ""))
                                        select new
                                        {
                                            NAV = (t != null) ? t.Field<object>("1 Year") : "N/A"
                                        }).First();
                            if (indx.NAV.ToString() != "N/A")
                                listbenchval.Add(Convert.ToDouble(indx.NAV));
                            else
                                listbenchval.Add("N/A");

                            var indx2 = (from t in nwIndexDataTble.AsEnumerable()
                                         where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[1])
                                         select new
                                         {
                                             NAV = (t != null) ? t.Field<object>("1 Year") : "N/A"
                                         }).First();
                            if (indx2.NAV.ToString() != "N/A")
                                listadbenchval.Add(Convert.ToDouble(indx2.NAV));
                            else
                                listadbenchval.Add("N/A");

                        }
                    }




                    if (i == 0)
                    {
                        //if (nwdataTble.Rows[0]["Since Inception"].ToString() != "N/A")
                        //    schemeInception = Convert.ToDouble(nwdataTble.Rows[0]["Since Inception"]);
                        // 
                        /* SI need to be changed for tata treasury*/

                        //if (schmeId == 9042 || schmeId == 9116)
                        
                            DataTable dtbletemp = new DataTable("temp");
                            string daydiff = string.Empty;

                            TimeSpan noofday = LastQtrEndDate.Subtract(allotDate);


                            if (modifiedInceptionDate)
                            {
                                daydiff = noofday.Days.ToString() + " d";
                            }
                            else
                            {
                                daydiff = "0 SI";
                            }


                            sqlcmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_TATA", cn);
                            sqlcmd.CommandType = CommandType.StoredProcedure;
                            sqlcmd.CommandTimeout = 2000;
                            sqlcmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
                            sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                          //  sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSetAbs));
                            sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                            sqlcmd.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                            sqlcmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                            sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriodin", daydiff));
                            sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
                            sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                            sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
                            sqlcmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                            sqlcmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

                            sqlDa.SelectCommand = sqlcmd;
                            sqlDa.Fill(dtbletemp);
                            if (dtbletemp.Rows[0][2].ToString().ToUpper().Trim() != "N/A")
                                schemeInception = Convert.ToDouble(dtbletemp.Rows[0][2]);
                            else
                                schemeInception = null;

                        //}
                        /* */


                        SchemeName = nwdataTble.Rows[0]["Scheme_Name"].ToString();
                        if (schmeId == 8698 && settingSet == 26)
                            SchemeName = "Tata Monthly Income Fund (TMIF) - Other than Individual & HUF - Monthly Income Option";
                        if (schmeId == 8698 && settingSet == 2)
                            SchemeName = "Tata Monthly Income Fund (TMIF) - Individual & HUF - Monthly Income Option";

                    }
                }

                #endregion


                #region addititional Inception,indexInception, addlindexInception

                SqlCommand indexCmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_TATA", cn);
                indexCmd.CommandType = CommandType.StoredProcedure;
                indexCmd.CommandTimeout = 2000;
                indexCmd.Parameters.Add(new SqlParameter("@IndexIDs", strIndexBenchmarlIds));
                indexCmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                indexCmd.Parameters.Add(new SqlParameter("@DateFrom", allotDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                indexCmd.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                indexCmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                //indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", "0 Si"));
                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strdaydiff));
                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
                indexCmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                indexCmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


                oIndexDataTble.Reset();
                sqlDa.SelectCommand = indexCmd;
                sqlDa.Fill(oIndexDataTble);

                if (!isAdditionalBenchMark)
                {

                    if (isCompositeIndex)
                    {
                        //if (oIndexDataTble.Rows[0]["0 Si"].ToString() != "N/A" || oIndexDataTble.Rows[1]["0 Si"].ToString() != "N/A")
                        //    //indexInception = Convert.ToDouble(oIndexDataTble.Rows[0]["0 Si"]);
                        //    indexInception = CompositeReturn(Convert.ToInt32(schmeId), Convert.ToDouble(oIndexDataTble.Rows[0]["0 Si"]), Convert.ToDouble(oIndexDataTble.Rows[1]["0 Si"]));
                        if (oIndexDataTble.Rows[0][strdaydiffColumn].ToString() != "N/A" && oIndexDataTble.Rows[1][strdaydiffColumn].ToString() != "N/A")
                            indexInception = CompositeReturn(Convert.ToInt32(schmeId), Convert.ToDouble(oIndexDataTble.Rows[0][strdaydiffColumn]), Convert.ToDouble(oIndexDataTble.Rows[1][strdaydiffColumn]));

                    }
                    else
                    {
                        if (oIndexDataTble.Rows[0][strdaydiffColumn].ToString() != "N/A")
                            indexInception = Convert.ToDouble(oIndexDataTble.Rows[0][strdaydiffColumn]);
                    }
                }
                else//for additional benchmark
                {
                    if (isCompositeIndex)
                    {
                        var indxfrst = (from t in oIndexDataTble.AsEnumerable()
                                        where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                                        select new
                                        {
                                            NAV = (t != null) ? t.Field<object>(strdaydiffColumn) : "N/A"
                                        }).First();

                        var indxScnd = (from t in oIndexDataTble.AsEnumerable()
                                        where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[1])
                                        select new
                                        {
                                            NAV = (t != null) ? t.Field<object>(strdaydiffColumn) : "N/A"
                                        }).First();


                        if (indxfrst.NAV.ToString() != "N/A" && indxScnd.NAV.ToString() != "N/A")
                            indexInception = CompositeReturn(Convert.ToInt32(schmeId), Convert.ToDouble(indxfrst.NAV), Convert.ToDouble(indxScnd.NAV));


                        var inx2 = (from t in oIndexDataTble.AsEnumerable()
                                    where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[2])
                                    select new
                                    {
                                        NAV = (t != null) ? t.Field<object>(strdaydiffColumn) : "N/A"
                                    }).First();

                        var addnlbenchName = (from t in oIndexDataTble.AsEnumerable()
                                              where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[2])
                                              select new
                                              {
                                                  indexNam = (t != null) ? t.Field<object>("Index_Name") : "N/A"
                                              }).First();
                        if (inx2.NAV.ToString() != "N/A")
                            addlindexInception = Convert.ToDouble(inx2.NAV);

                        if (isAdditionalBenchMark)
                            _additionalindexName = addnlbenchName.indexNam.ToString();
                    }
                    else
                    {


                        var inx = (from t in oIndexDataTble.AsEnumerable()
                                   where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                                   select new
                                   {
                                       NAV = (t != null) ? t.Field<object>(strdaydiffColumn) : "N/A"
                                   }).First();
                        if (inx.NAV.ToString() != "N/A")
                            indexInception = Convert.ToDouble(inx.NAV);

                        var inx2 = (from t in oIndexDataTble.AsEnumerable()
                                    where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[1])
                                    select new
                                    {
                                        NAV = (t != null) ? t.Field<object>(strdaydiffColumn) : "N/A"
                                    }).First();

                        var addnlbenchName = (from t in oIndexDataTble.AsEnumerable()
                                              where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[1])
                                              select new
                                              {
                                                  indexNam = (t != null) ? t.Field<object>("Index_Name") : "N/A"
                                              }).First();
                        if (inx2.NAV.ToString() != "N/A")
                            addlindexInception = Convert.ToDouble(inx2.NAV);

                        if (isAdditionalBenchMark)
                            _additionalindexName = addnlbenchName.indexNam.ToString();


                        //var benchNamestr = (from t in oIndexDataTble.AsEnumerable()
                        //                      where t.Field<System.Decimal>("Index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                        //                      select new
                        //                      {
                        //                          indexNam = (t != null) ? t.Field<object>("Index_Name") : "N/A"
                        //                      }).First();

                        //_indexNamestr = benchNamestr.indexNam.ToString();
                    }

                }


                #endregion


                #region Initiliaze Inception CompundReturnSI ,CompundReturnSIndex ,CompundReturnSIndexaddl

                if (schemeInception != null)
                    CompundReturnSI = amount * Math.Pow(1 + (double)schemeInception / 100, Math.Round((double)day / 365,4));

                if (indexInception != null)
                    CompundReturnSIndex = amount * Math.Pow(1 + (double)indexInception / 100, Math.Round((double)day / 365, 4));
                if (isAdditionalBenchMark)
                {
                    if (addlindexInception != null)
                        CompundReturnSIndexaddl = amount * Math.Pow(1 + (double)addlindexInception / 100, Math.Round((double)day / 365, 4));
                }

                #endregion


                #region Set newFrmtDataTable Table from listschemeval and listbenchval ,listadbenchval
                if (listschemeval.Count == 3 && listbenchval.Count == 3)
                {
                    newFrmtDataTable.Rows.Add(SchemeName, listschemeval[0].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listschemeval[0]) * amount / 100).ToString("n2"), listschemeval[0].ToString(), listschemeval[1].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listschemeval[1]) * amount / 100).ToString("n2"), listschemeval[1].ToString(), listschemeval[2].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listschemeval[2]) * amount / 100).ToString("n2"), listschemeval[2].ToString(), CompundReturnSI == null ? "N/A" : Convert.ToDecimal(CompundReturnSI).ToString("n2"), schemeInception == null ? "N/A" : schemeInception.ToString(), allotDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                    newFrmtDataTable.Rows.Add("Scheme Benchmark(" + _indexName + ")", listbenchval[0].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listbenchval[0]) * amount / 100).ToString("n2"), listbenchval[0].ToString(), listbenchval[1].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listbenchval[1]) * amount / 100).ToString("n2"), listbenchval[1].ToString(), listbenchval[2].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listbenchval[2]) * amount / 100).ToString("n2"), listbenchval[2].ToString(), CompundReturnSIndex == null ? "N/A" : Convert.ToDecimal(CompundReturnSIndex).ToString("n2"), indexInception == null ? "N/A" : indexInception.ToString(), "");
                    if (isAdditionalBenchMark)
                        newFrmtDataTable.Rows.Add("Additional Benchmark(" + _additionalindexName + ")", listadbenchval[0].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listadbenchval[0]) * amount / 100).ToString("n2"), listadbenchval[0].ToString(), listadbenchval[1].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listadbenchval[1]) * amount / 100).ToString("n2"), listadbenchval[1].ToString(), listadbenchval[2].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listadbenchval[2]) * amount / 100).ToString("n2"), listadbenchval[2].ToString(), CompundReturnSIndexaddl == null ? "N/A" : Convert.ToDecimal(CompundReturnSIndexaddl).ToString("n2"), addlindexInception == null ? "N/A" : addlindexInception.ToString(), "");
                }
                #endregion


            }
            catch (Exception exc)
            {
                lblMessage.Text = exc.Message;
                lblMessage.Visible = false;
            }


            // special case 
            if (newFrmtDataTable.Rows.Count >= 2)
            {
                foreach (DataColumn dc in newFrmtDataTable.Columns)
                {
                    if (newFrmtDataTable.Rows[0][dc].ToString().ToUpper() == "N/A")
                    {
                        newFrmtDataTable.Rows[1][dc] = "N/A";
                        if (newFrmtDataTable.Rows.Count == 3)
                            newFrmtDataTable.Rows[2][dc] = "N/A";
                    }
                }
            }

            return newFrmtDataTable;
            //return perfDataTable;
        }
        #endregion

        #region: Reset Method

        public void Reset()
        {
            ddlNature.SelectedIndex = 0;
            ddlOption.SelectedIndex = 0;
            ddlFunManager.SelectedIndex = 0;
            txtInvestment.Text = "10000";
            lnkbtnDownload.Visible = false;
            ddlQtrEnd.SelectedIndex = 3;
            ddlYearEnd.SelectedIndex = ddlYearEnd.Items.Count - 4;
            FillScheme();
            LiteralDisclaimer.Text = "";
        }

        #endregion

        #region: Date Methods

        private DateTime GetLastQuarterDates(DateTime todate)
        {
            DateTime currentDate;//= DateTime.Today;
            currentDate = todate;
            int month = currentDate.Month;
            DateTime qtrenddate = currentDate;

            if (month >= 1 && month <= 3)
            {
                if (qtrenddate.Day == 31 && qtrenddate.Month == 3)
                    return qtrenddate;
                else
                    qtrenddate = new DateTime(currentDate.Year - 1, 12, 31);

            }
            if (month >= 4 && month <= 6)
            {
                if (qtrenddate.Day == 30 && qtrenddate.Month == 6)
                    return qtrenddate;
                else
                    qtrenddate = new DateTime(currentDate.Year, 3, 31);

            }
            if (month >= 7 && month <= 9)
            {
                if (qtrenddate.Day == 30 && qtrenddate.Month == 9)
                    return qtrenddate;
                else
                    qtrenddate = new DateTime(currentDate.Year, 6, 30);

            }
            if (month >= 10 && month <= 12)
            {
                if (qtrenddate.Day == 31 && qtrenddate.Month == 12)
                    return qtrenddate;
                else
                    qtrenddate = new DateTime(currentDate.Year, 9, 30);
            }

            return qtrenddate;
        }



        public DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }



        #endregion

        #region: Generate  Table Methods

        public string GenerateTable(DataSet dsFormatFinal, bool isEquity, DateTime lastQtrDate)
        {
            System.Text.StringBuilder sbResultTable = new System.Text.StringBuilder();

            try
            {

                sbResultTable.AppendLine("<br/>");
                if (dsFormatFinal.Tables[0].Rows.Count > 0)
                {//text-align: center;
                    // sbResultTable.AppendLine("<table width='100%'><tr><td align='center'><centre><bold>" + dsFormatFinal.Tables[0].Rows[0]["SCHEME_NAME"].ToString() + "</bold></centre></td></tr></table>");
                    //                    sbResultTable.AppendLine("<table width='100%'><tr><td background='../Images/heading.jpg' height='22' width='500'><span class='style12'>&nbsp;&nbsp;<span class='style4'><img src='../Images/arrow.jpg' /> " + dsFormatFinal.Tables[0].Rows[0][0].ToString() + "</span></span></td></tr></table>");
                    sbResultTable.AppendLine("<table width='100%'><tr><td colspan='6'></td></tr><tr><td align='left' colspan='6' background='../Images/heading.jpg' height='22' width='500'><span class='style12'>&nbsp;&nbsp;<span class='style4'><img src='../Images/arrow.jpg' /> " + "Performance at Glance" + "</span></span></td></tr></table>");

                    //<td background="../Images/heading.jpg" height="26" width="960"><span class="style12">&nbsp;&nbsp;<span class="style4">TATA Mutual Fund </span></span></td>
                } 
                int colorcount;
                sbResultTable.AppendLine("<div style='padding-top:5px;'><table class='resultgrid' cellspacing='0' cellpadding='4' rules='all' id='gvNewFormatEquity' style='border-width:1px;border-style:solid;border-collapse:collapse; border-color:#1068B2'>");
                sbResultTable.AppendLine("<tr>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='1'></th>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + ((DateTime.IsLeapYear(lastQtrDate.AddYears(-1).Year) == true && lastQtrDate.AddYears(-1).Month == 2 && lastQtrDate.AddYears(-1).Day == 28) ? lastQtrDate.AddYears(-1).AddDays(1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) : lastQtrDate.AddYears(-1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture)) + " to " + lastQtrDate.ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + "</th>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + ((DateTime.IsLeapYear(lastQtrDate.AddYears(-2).Year) == true && lastQtrDate.AddYears(-2).Month == 2 && lastQtrDate.AddYears(-2).Day == 28) ? lastQtrDate.AddYears(-2).AddDays(1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) : lastQtrDate.AddYears(-2).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture)) + " to " + ((DateTime.IsLeapYear(lastQtrDate.AddYears(-1).Year) == true && lastQtrDate.AddYears(-1).Month == 2 && lastQtrDate.AddYears(-1).Day == 28) ? lastQtrDate.AddYears(-1).AddDays(1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) : lastQtrDate.AddYears(-1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture)) + "</th>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + ((DateTime.IsLeapYear(lastQtrDate.AddYears(-3).Year) == true && lastQtrDate.AddYears(-3).Month == 2 && lastQtrDate.AddYears(-3).Day == 28) ? lastQtrDate.AddYears(-3).AddDays(1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) : lastQtrDate.AddYears(-3).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture)) + " to " + ((DateTime.IsLeapYear(lastQtrDate.AddYears(-2).Year) == true && lastQtrDate.AddYears(-2).Month == 2 && lastQtrDate.AddYears(-2).Day == 28) ? lastQtrDate.AddYears(-2).AddDays(1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) : lastQtrDate.AddYears(-2).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture)) + "</th>");
                //sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + lastQtrDate.AddYears(-1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + lastQtrDate.ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + "</th>");
                //sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + lastQtrDate.AddYears(-2).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + lastQtrDate.AddYears(-1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + "</th>");
                //sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + lastQtrDate.AddYears(-3).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + lastQtrDate.AddYears(-2).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + "</th>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='2'>Since Inception</th>");
                sbResultTable.AppendLine("<th class='headerCss' align='center' scope='col'>Inception Date</th>");

                sbResultTable.AppendLine("</tr>");

                sbResultTable.AppendLine("<tr>");
                sbResultTable.AppendLine("<th width='30%' class='headerCss' align='left' scope='col'>Fund / Benchmark</th>");
                sbResultTable.AppendLine("<th width='9%' class='headerCss' align='center' scope='col'>Absolute returns (<img src='../Images/rupee.jpg' style='vertical-align:bottom;' />)</th>");
                sbResultTable.AppendLine("<th width='6%' class='headerCss' align='center' scope='col'>Returns(%)</th>");
                sbResultTable.AppendLine("<th width='9%' class='headerCss' align='center' scope='col'>Absolute returns (<img src='../Images/rupee.jpg' style='vertical-align:bottom;' />)</th>");
                sbResultTable.AppendLine("<th width='6%' class='headerCss' align='center' scope='col'>Returns(%)</th>");
                sbResultTable.AppendLine("<th width='9%' class='headerCss' align='center' scope='col'>Absolute returns (<img src='../Images/rupee.jpg' style='vertical-align:bottom;' />)</th>");
                sbResultTable.AppendLine("<th width='6%' class='headerCss' align='center' scope='col'>Returns(%)</th>");
                sbResultTable.AppendLine("<th width='9%' class='headerCss' align='center' scope='col'>CAGR returns (<img src='../Images/rupee.jpg' style='vertical-align:bottom;' />)</th>");
                sbResultTable.AppendLine("<th width='6%' class='headerCss' align='center' scope='col'>Returns(%)</th>");
                sbResultTable.AppendLine("<th width='10%' class='headerCss' align='center' scope='col'> </th>");

                sbResultTable.AppendLine("</tr>");
                colorcount = 1;
                #region New Format
                if (dsFormatFinal.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsFormatFinal.Tables[0].Rows)
                    {
                        if (colorcount % 2 == 1)
                            //sbResultTable.AppendLine("<tr style='background-color:#f5f3f3;color:Black;'>");
                            sbResultTable.AppendLine("<tr class='row1'>");
                        else
                            sbResultTable.AppendLine("<tr class='row2'>");
                        //sbResultTable.AppendLine("<tr style='background-color:#ffffff ;color:Black;'>");

                        sbResultTable.AppendLine("<td align='left'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["SchemeIndex"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["FirstyearRs"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["FirstyearReturn"]).Trim() == "" ? "" : Convert.ToString(dr["FirstyearReturn"]).Trim()==
                            "N/A" ? "N/A" : Convert.ToDecimal(dr["FirstyearReturn"]).ToString("n2"));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["SecondyearRs"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["SecondyearReturn"]).Trim() == "" ? "" : Convert.ToString(dr["SecondyearReturn"]).Trim() ==
                            "N/A" ? "N/A" : Convert.ToDecimal(dr["SecondyearReturn"]).ToString("n2"));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["ThirdyearRs"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["ThirdyearReturn"]).Trim() == "" ? "" : Convert.ToString(dr["ThirdyearReturn"]).Trim() ==
                            "N/A" ? "N/A" : Convert.ToDecimal(dr["ThirdyearReturn"]).ToString("n2"));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["CagrInceptionRs"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["CagrInceptionReturn"]).Trim() == "" ? "" : Convert.ToString(dr["CagrInceptionReturn"]).Trim() ==
                            "N/A" ? "N/A" : Convert.ToDecimal(dr["CagrInceptionReturn"]).ToString("n2"));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["InceptionDate"]));
                        sbResultTable.AppendLine("</td>");
                        sbResultTable.AppendLine("</tr>");
                        colorcount++;
                    }
                }
                #endregion
                sbResultTable.AppendLine("</table></div>");

                sbResultTable.AppendLine("<br/>");

                return sbResultTable.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            finally
            {
                //sbResultTable = null;
            }
        }


        #endregion

        #endregion

        #region: Export to Excel
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the
            // specified ASP.NET server control at run time.
            // No code required here.
            return;
        }

        protected void ExportExcelClick(object sender, EventArgs e)
        {
            ExportToExcel();
        }


        public void ExportToExcel()
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=TataSchemeExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Text.StringBuilder objFinalstr = new System.Text.StringBuilder();

            //TestGridView.RenderControl(hw);

            Response.Write("<br/><br/>");

            LiteralFinalReturns.RenderControl(hw);


            objFinalstr.Append("<table width='100%'><td width='100%' colspan='6' style='padding-left:15px; padding-bottom:5px; padding-top:5px;'><img src='../Images/tatamf.jpg' /></td></tr></table>");
            // objFinalstr.Append("<br/>");
            objFinalstr.Append(sw.ToString());
            objFinalstr.Replace("class='headerCss'", "style ='background: #265599; color: #FFFFFF; font-weight: bold; border: #012258 solid 1px;'");
            objFinalstr.Replace("class='row1'", "style='background: #f5f3f3; color: #353131; border-bottom: #999999 1px dotted;'");
            objFinalstr.Replace("class='row2'", "style='background: #fff; color: #353131; border-bottom: #999999 1px dotted;'");
            objFinalstr.Replace("class='style12'", "style='color: #265599; font-weight: bold;'");
            objFinalstr.Replace("class='style4'", "style='font-family: Tahoma; font-size: 12px;'");
            objFinalstr.Replace("class='resultgrid'", "width='100%' style='width: 99%; font-family: Verdana; font-size: 11px; height: 53px; margin-left: .5%; margin-right: .5%; top: -50px;'");


            string ImagePath = string.Empty;
            ImagePath = HttpContext.Current.Server.MapPath("~") + "Images\\";
            ImagePath = AppDomain.CurrentDomain.BaseDirectory + "Images\\";
            ImagePath = "http://mfiframes.mutualfundsindia.com" + @"/Images/";
            //objFinalstr.Replace("../Images/heading.jpg", ImagePath+ "heading.jpg");
            objFinalstr.Replace("background='../Images/heading.jpg' height='22' width='500'", "width='800px' align='left' style ='background:#fff;color:#265599; font-weight: bold;'");
            objFinalstr.Replace("../Images/arrow.jpg", ImagePath + "arrow.jpg");
            //objFinalstr.Replace("../Images/tatamf.jpg'", ImagePath + "tatamf.jpg'");
            objFinalstr.Replace("../Images/tatamf.jpg'", ImagePath + "tatamf.jpg'");
            // objFinalstr.Replace("../Images/tata.gif", ImagePath + "tata.gif");
            objFinalstr.Replace("../Images/tata.gif", "None");
            // objFinalstr.Replace("../Images/rupee.jpg", ImagePath +"rupee.jpg");
            objFinalstr.Replace("<img src='../Images/rupee.jpg' style='vertical-align:bottom;' />", "Rs.");

            objFinalstr.Append("<br/><b>Disclaimer:</b><br/>");
            if (LiteralDisclaimer.Text.Length > 2)
            {
                if (LiteralDisclaimer.Text.Split('.').Count() == 2)
                    objFinalstr.Append(LiteralDisclaimer.Text.Substring(1));
                else
                {
                    foreach (string strr in LiteralDisclaimer.Text.Split( new string[] {"<br/>"},StringSplitOptions.None))
                    {
                        if(strr.Length>2)                        
                        objFinalstr.Append(strr.Substring(1) +"<br/>");
                    }
                }
            }
            objFinalstr.Append(@"  <b>Past performance may or may not be sustained in future.</b>. Absolute returns is
                                        computed on investment is of Rs 10,000.  For computation of since inception returns
                                        the allotment NAV has been taken as Rs. 10.00 (Except for Tata Liquid Fund, Tata
                                        Floater Fund, Tata Treasury Manager Fund & Tata Liquidity Management Fund where
                                        NAV is taken as Rs. 1,000).  All payouts during the period have been reinvested
                                        in the units of the scheme at the then prevailing NAV.  Load is not considered
                                        for computation of returns. While calculating returns dividend distribution tax
                                        is excluded.  In case, the start/end date of the concerned period is non-business
                                        date, the benchmark value of the previous date is considered for computation of
                                        returns.  'N/A' - Not Available. Schemes in existence for > 1 year performance
                                        provided for as many 12 months period as possible.  Please also refer to performance
                                        details of other schemes of Tata Mutual Fund managed by the Fund Manager(s)
                                        of this Scheme.  The Calculators provided on the website is for information purposes
                                        only and is not an offer to sell or a solicitation to buy any mutual fund units/securities.
                                        The recipient of this information should rely on their investigations and take professional
                                        advice before making any investment. <br/> The Calculators alone are not sufficient
                                        and shouldn't be used for the development or implementation of an investment strategy.
                                        It should not be construed as investment advice to any party.  Neither Tata Asset
                                        Management Ltd, nor any person connected with it, accepts any liability arising
                                        from the use of this information.  In view of individual nature of tax consequences,
                                        each investor is advised to consult his/ her own professional tax advisor.  <b>Mutual
                                        Fund Investments are subject to market risks, read all Scheme related documents
                                        carefully.</b>");



            Response.Write(objFinalstr.ToString());



            Response.Flush();
            Response.End();


        }
        #endregion

        private double CompositeReturn(int schemeid, double firstIndexreturn, double secondIndexreturn)
        {
            double returncomp = 0;
            switch (schemeid)
            {
                case 17802:
                case 17803:
                case 9080:
                case 9079://indo global
                case 17814:
                case 17815:
                case 9163:
                case 9164://Growing Economies Infrastructure Fund - Plan B - Growth
                    returncomp = firstIndexreturn * 0.65 + secondIndexreturn * 0.35;
                    break;
                case 17812:
                case 17813:
                case 9161:
                case 9162:// Growing Economies Infrastructure Fund - Plan A - Growth
                    returncomp = firstIndexreturn * 0.3 + secondIndexreturn * 0.7;
                    break;
                default:
                    returncomp = firstIndexreturn * 0.5 + secondIndexreturn * 0.5;
                    break;
            }
            return Math.Round(returncomp, 2);
        }

        protected void btnResetForm(object sender, EventArgs e)
        {
            Reset();
        }


        private void LeapyearMonthlyCalculation()
        {
            Int32 year;
            year = Convert.ToInt32(ddlYearEnd.SelectedValue);
            ddlQtrEnd.Items.RemoveAt(1);
            if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
            {
                ddlQtrEnd.Items.Insert(1, new ListItem("29th February", "02-29-"));
            }
            else
                ddlQtrEnd.Items.Insert(1, new ListItem("28th February", "02-28-"));
        }

        protected void ddlYearEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            LeapyearMonthlyCalculation();
        }
    }

}