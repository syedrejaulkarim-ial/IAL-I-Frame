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
    public partial class TataCalcOMF : System.Web.UI.Page
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

            lblMessage.Text = "";
            if (!IsPostBack)
            {
                FillNature();
                FillScheme();
                FillFundManager();
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
            FillScheme();//added
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
            //                                     where s.Nav_Check != 2 && s.Option_Id.ToString() == selected_option //s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
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
            //                    dt2 = scheme_name_1.ToDataTable();

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
            //                    dt2 = scheme_name_1.ToDataTable();
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
            //                                     where s.Nav_Check != 2 && s.Option_Id.ToString() == selected_option //s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
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
            //                    dt2 = scheme_name_1.ToDataTable();

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
            //                    dt2 = scheme_name_1.ToDataTable();
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

            //ddlMode.SelectedIndex = 0;
            //  ddBenchMark.Items.Clear();
            //FundmanegerText.Text = "";
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
                                dt2 = schenames.ToDataTable();
                                if (dt2.Rows.Count > 0)
                                {
                                    dgSchemeList.Visible = true;
                                    LiteralFinalReturns.Text = "";
                                    lnkbtnDownload.Visible = false;
                                    dgSchemeList.DataSource = dt2;
                                    dgSchemeList.DataBind();


                                    for (int i = 0; i < dgSchemeList.Items.Count; i++)
                                    {
                                        CheckBox cb = dgSchemeList.Items[i].FindControl("chkItem") as CheckBox;
                                        if (cb.Checked == false)
                                        {
                                            cb.Checked = true;
                                        }
                                    }

                                }

                            }
                            else
                            {
                                dgSchemeList.Visible = false;
                            }

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
            FetchreturnInOldFormat();
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
        protected void FillSchemeold()
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

                            DataTable dt2 = new DataTable();
                            if (scheme_name_1.Count() > 0)
                            {
                                //dt2 = scheme_name_1.ToDataTable();
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
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

                            var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                 join T in fundtable
                                                 on s.Fund_Id equals T.FUND_ID
                                                 where s.Nav_Check == 3//s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
                                                 //&& s.Launch_Date <= yearBacktodaysdate1
                                                 join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 // where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id
                                                 }).Distinct();

                            DataTable dt2 = new DataTable();
                            if (scheme_name_1.Count() > 0)
                            {
                                //dt2 = scheme_name_1.ToDataTable();
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
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




                }
                catch (Exception exp)
                {
                }
                finally
                {
                    ddlOption.SelectedIndex = 0;
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

                    //modified
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
                            // ddlFunManager.DataTextField = "FUND_MAN";
                            // ddlFunManager.DataValueField = "FUND_CODE";
                            // ddlFunManager.DataValueField = "FUND_MAN";
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


        #endregion

        #region: Fetch Methods


        public void FetchreturnInOldFormat()
        {
            System.Text.StringBuilder sbFinal = new System.Text.StringBuilder();
            Decimal dcmlIndexid;
            // bool isEquity = true;
            string IndexBenchmarlIds = null;
            bool showSpecificDisclaimer = false;


            try
            {
                if (dgSchemeList.Items.Count > 0)
                {
                    DataTable dtFinal = new DataTable();
                    DataTable dtScheme = new DataTable();
                    DataTable dtTempOld = new DataTable();
                    DataTable dtTempNew = new DataTable();
                    DataTable dtIndex = new DataTable();
                    DataTable dtSchemeNewFrmt = new DataTable();
                    DataTable dtIndexNewFrmt = new DataTable();
                    DataSet dsOldFormatFinal = new DataSet();

                    int settingSet = 2;


                    DateTime _todate = Convert.ToDateTime(txtStartDate.Text);
                    //DateTime _todate = new DateTime(Convert.ToInt16(txtStartDate.Text.Split('/')[2]),
                    //                 Convert.ToInt16(txtStartDate.Text.Split('/')[1]),
                    //                 Convert.ToInt16(txtStartDate.Text.Split('/')[0]));

                    _asOnDate = _todate;
                    string val_SchemeIDs = string.Empty;
                    List<decimal> lstschmId = new List<decimal>();
                    lstschmId.Clear();
                    for (int i = 0; i < dgSchemeList.Items.Count; i++)
                    {
                        CheckBox cb = dgSchemeList.Items[i].FindControl("chkItem") as CheckBox;
                        if (cb.Checked == true)
                        {
                            val_SchemeIDs += ((Label)dgSchemeList.Items[i].Cells[0].FindControl("lblSchemeID")).Text + ",";
                            lstschmId.Add(Convert.ToDecimal(((Label)dgSchemeList.Items[i].Cells[0].FindControl("lblSchemeID")).Text));
                        }
                    }
                    if (val_SchemeIDs != string.Empty)
                    {
                        val_SchemeIDs = val_SchemeIDs.TrimEnd(',');

                        using (var IndexDC = new TataCalculatorDataContext())
                        {
                            var lastInceptionDate = (from tsm in IndexDC.T_SCHEMES_MASTER_tatas
                                                     where lstschmId.Contains(tsm.Scheme_Id)
                                                     select tsm.Launch_Date).Min();

                            TimeSpan Daydiff = _todate.Subtract(Convert.ToDateTime(lastInceptionDate));

                            if (Daydiff.Days <= 0)
                            {
                                // lblMessage.Text = "Please Select Date Greater Than The Minimum Inception Date Of Selected Schemes";
                                lblMessage.Text = "Please select date greater than " + Convert.ToDateTime(lastInceptionDate).ToString("dd-MMMM-yyyy", CultureInfo.InvariantCulture);
                                return;
                            }
                        }
                    }


                    for (int i = 0; i < dgSchemeList.Items.Count; i++)
                    {
                        CheckBox cb = dgSchemeList.Items[i].FindControl("chkItem") as CheckBox;
                        string val_SchemeID = ((Label)dgSchemeList.Items[i].Cells[0].FindControl("lblSchemeID")).Text;


                        if (cb.Checked == true)
                        {
                            string strRollingPeriodin = string.Empty;
                            strRollingPeriodin = "1 m,3 m,6 m,1 YYYY,3 YYYY,5 YYYY,0 Si";
                            DateTime LastQuarterDate = GetLastQuarterDates(_todate);
                            DateTime allotDate;
                            bool modifiedInceptionDate = false;

                            if (val_SchemeID == "9999999991")
                            {
                                settingSet = 2;
                                val_SchemeID = "8698";
                            }
                            else if (val_SchemeID == "9999999992")
                            {
                                settingSet = 26;
                                val_SchemeID = "8698";
                            }

                            Int32 oval = 0;

                            #region DataContext
                            using (var IndexData = new TataCalculatorDataContext())
                            {

                                var strNature = (from natr in IndexData.T_SCHEMES_NATURE_tatas
                                                 where natr.Nature_ID == (
                                                 from tfm in IndexData.T_FUND_MASTER_tatas
                                                 where tfm.FUND_ID ==
                                                 (from tsm in IndexData.T_SCHEMES_MASTER_tatas
                                                  where tsm.Scheme_Id == Convert.ToDecimal(val_SchemeID)
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


                                var alotdate = (from ind in IndexData.T_SCHEMES_MASTER_tatas
                                                where ind.Scheme_Id.ToString() == val_SchemeID
                                                select ind.Launch_Date).Single();

                                if (alotdate == null)
                                {
                                    //  lblMessage.Text = "Launch date is not available";
                                    //return;
                                    continue;
                                }
                                else
                                    allotDate = Convert.ToDateTime(alotdate.ToString());

                                //if (val_SchemeID == "9042")// Specal case of Tata Treasury Manager Fund - SHIP - Growth SI need to show -30 April 2009
                                //    allotDate = new DateTime(2009, 4, 30);

                                switch (Convert.ToInt32(val_SchemeID))
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

                                var IndexID = (from indexid in IndexData.T_SCHEMES_INDEX_tatas
                                               where Convert.ToString(indexid.SCHEME_ID) == val_SchemeID
                                               select new
                                               {
                                                   indexid.INDEX_ID
                                               }).First().INDEX_ID;


                                var sbNtrid = (from tfm in IndexData.T_FUND_MASTER_tatas
                                               where tfm.FUND_ID ==
                                               (from tsm in IndexData.T_SCHEMES_MASTER_tatas
                                                where tsm.Scheme_Id == Convert.ToDecimal(val_SchemeID)
                                                select new
                                                {
                                                    tsm.Fund_Id
                                                }
                                               ).First().Fund_Id
                                               select new { tfm.SUB_NATURE_ID }).First().SUB_NATURE_ID;
                                SubNatureId = sbNtrid.ToString();

                                var IndexName = (from tim in IndexData.T_INDEX_MASTER_tatas
                                                 where tim.INDEX_ID == IndexID
                                                 select tim.INDEX_NAME).First();

                                string _indexName = string.Empty;
                                _indexName = IndexName.ToString();
                                string _indexId = string.Empty;
                                _indexId = IndexID.ToString();
                                bool isCompositeIndex = false;


                                // case for composit Index
                                if (val_SchemeID == "9161" || val_SchemeID == "9162" || val_SchemeID == "9163" || val_SchemeID == "9164" || val_SchemeID == "17812" || val_SchemeID == "17813" || val_SchemeID == "17814" || val_SchemeID == "17815")
                                {
                                    _indexId = "13,3T";
                                    isCompositeIndex = true;
                                }
                                if (val_SchemeID == "9079" || val_SchemeID == "9080" || val_SchemeID == "17802" || val_SchemeID == "17803")
                                {
                                    _indexId = "15,2T";
                                    isCompositeIndex = true;
                                }

                                // Special Case for Tata SIP Fund 3
                                if (val_SchemeID == "13530" || val_SchemeID == "13531")
                                {
                                    //isCompositeIndex = true;
                                    _indexId = "4T";

                                }


                                bool isAdditionalBenchMark = false;

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
                                            if (val_SchemeID != "9079" && val_SchemeID != "9080" && val_SchemeID != "9161" && val_SchemeID != "9162" && val_SchemeID != "9163" && val_SchemeID != "9164" && val_SchemeID != "13530" && val_SchemeID != "13531" && val_SchemeID != "17802" && val_SchemeID != "17803" && val_SchemeID != "17812" && val_SchemeID != "17813" && val_SchemeID != "17814" && val_SchemeID != "17815")
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
                                    if (SubNatureId == "13" || strNature == "Liquid" || SubNatureId == "41" || SubNatureId == "15" || SubNatureId == "2")
                                    {
                                        IndexBenchmarlIds = _indexId + ",135";
                                    }
                                    else //if (SubNatureId == "4")
                                    {
                                        IndexBenchmarlIds = _indexId + ",134";
                                    }

                                }
                                //Special case if Crisil Balanced Fund Index is there No additional benchmark are required
                                if (_indexId.Trim() == "27")
                                {
                                    isAdditionalBenchMark = false;
                                    IndexBenchmarlIds = _indexId;
                                }

                                string strIndexBenchmarlIds = string.Empty;
                                strIndexBenchmarlIds = IndexBenchmarlIds;


                                if (_indexId.Trim() == "32")
                                {
                                    //strIndexBenchmarlIds = strIndexBenchmarlIds.Replace("32", "1T");
                                    strIndexBenchmarlIds = IndexBenchmarlIds.Replace("32", "1T");
                                }

                                // code on 02.07.2013 
                                if (val_SchemeID == "15140" || val_SchemeID == "15442")
                                {
                                    isAdditionalBenchMark = true;
                                    _indexId = "25,134";
                                    IndexBenchmarlIds = _indexId;
                                }

                                #region old format

                                //if (val_SchemeID == "9042")// Specal case of Tata Treasury Manager Fund - SHIP - Growth SI need to show -30 April 2009 
                                if (modifiedInceptionDate)
                                {

                                    string daydiff = string.Empty;

                                    TimeSpan noofday;
                                    // noofday  = LastQuarterDate.Subtract(allotDate);
                                    noofday = _todate.Subtract(allotDate);
                                    daydiff = noofday.Days.ToString() + " d";
                                    strRollingPeriodin = strRollingPeriodin.Replace("0 Si", daydiff);

                                }

                                string strdaydiff = string.Empty;
                                string strdaydiffColumn = string.Empty;
                                TimeSpan _noofday;
                                //_noofday = LastQuarterDate.Subtract(allotDate);
                                _noofday = _todate.Subtract(allotDate);
                                strdaydiff = _noofday.Days.ToString() + " D";
                                strdaydiffColumn = strdaydiff + "ay";


                                SqlCommand cmdScheme = new SqlCommand();
                                cmdScheme = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_TATA", cn);
                                // cmdScheme = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", cn);
                                cmdScheme.CommandType = CommandType.StoredProcedure;
                                cmdScheme.CommandTimeout = 2000;
                                cmdScheme.Parameters.Add(new SqlParameter("@SchemeIDs", val_SchemeID));
                                cmdScheme.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                                cmdScheme.Parameters.Add(new SqlParameter("@DateFrom", ""));
                                //cmdScheme.Parameters.Add(new SqlParameter("@DateTo", LastQuarterDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                                cmdScheme.Parameters.Add(new SqlParameter("@DateTo", _todate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                                cmdScheme.Parameters.Add(new SqlParameter("@RoundTill", 2));
                                cmdScheme.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
                                cmdScheme.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
                                cmdScheme.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                                cmdScheme.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
                                cmdScheme.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                                cmdScheme.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));
                                SqlDataAdapter sqldaScheme = new SqlDataAdapter(cmdScheme);

                                dtScheme.Reset();
                                sqldaScheme.Fill(dtScheme);
                                //dsOldFormatFinal.Tables.Add(dtScheme);

                                strRollingPeriodin = strRollingPeriodin.Replace("0 Si", strdaydiff);

                                SqlCommand cmdIndex = new SqlCommand();
                                cmdIndex = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_TATA", cn);
                                // cmdIndex = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", cn);
                                cmdIndex.CommandType = CommandType.StoredProcedure;
                                cmdIndex.CommandTimeout = 2000;
                                cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", strIndexBenchmarlIds));
                                cmdIndex.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                                cmdIndex.Parameters.Add(new SqlParameter("@DateFrom", ""));
                                //cmdIndex.Parameters.Add(new SqlParameter("@DateTo", LastQuarterDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                                cmdIndex.Parameters.Add(new SqlParameter("@DateTo", _todate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                                cmdIndex.Parameters.Add(new SqlParameter("@RoundTill", 2));
                                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
                                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
                                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                                cmdIndex.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
                                cmdIndex.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                                cmdIndex.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

                                SqlDataAdapter sqldaIndex = new SqlDataAdapter(cmdIndex);
                                dtIndex.Reset();
                                sqldaIndex.Fill(dtIndex);
                                if (dtIndex.Columns.Contains("INDEX_TYPE"))
                                    dtIndex.Columns.Remove("INDEX_TYPE");

                                IndexBenchmarlIds = strIndexBenchmarlIds.Replace("T", "");


                                IEnumerable<DataRow> _testdtIndex = from ib in IndexBenchmarlIds.Split(',').AsEnumerable()
                                                                    join tst in dtIndex.AsEnumerable()
                                                                    on ib equals tst.Field<decimal>("Index_id").ToString()
                                                                    select tst;
                                DataTable _dttt = new DataTable("tblIndex");
                                _dttt = _testdtIndex.CopyToDataTable<DataRow>();
                                dtIndex.Clear();
                                dtIndex = _dttt;

                                // Composite Index Part
                                if (isCompositeIndex)
                                {
                                    // DataTable _tempdt = dtIndex.Copy();
                                    if (dtIndex.Rows.Count >= 2)
                                    {
                                        foreach (DataColumn dc in dtIndex.Columns)
                                        {
                                            if (dc.ColumnName.ToUpper() == "INDEX_NAME")
                                            {
                                                dtIndex.Rows[0][dc] = dtIndex.Rows[0][dc].ToString() + " & " + dtIndex.Rows[1][dc].ToString();
                                            }
                                            else
                                            {
                                                if (dc.ColumnName.ToUpper() != "INDEX_ID" && dc.ColumnName.ToUpper() != "INDEX_TYPE")
                                                {
                                                    if (dtIndex.Rows[0][dc].ToString().ToUpper() != "N/A" && dtIndex.Rows[1][dc].ToString().ToUpper() != "N/A")
                                                        dtIndex.Rows[0][dc] = CompositeReturn(Convert.ToInt32(val_SchemeID), Convert.ToDouble(dtIndex.Rows[0][dc]), Convert.ToDouble(dtIndex.Rows[1][dc]));
                                                    else
                                                        dtIndex.Rows[0][dc] = "N/A";

                                                }
                                            }
                                        }
                                    }
                                    dtIndex.Rows[1].Delete();
                                    dtIndex.AcceptChanges();
                                }


                                DataRow[] datarows;

                                if (isAdditionalBenchMark)
                                {
                                    datarows = new DataRow[2] { dtScheme.NewRow(), dtScheme.NewRow() };
                                    datarows[0] = dtIndex.Rows[0];
                                    datarows[1] = dtIndex.Rows[1];
                                }
                                else
                                {
                                    datarows = new DataRow[1] { dtScheme.NewRow() };
                                    datarows[0] = dtIndex.Rows[0];
                                }

                                if (!isCompositeIndex)
                                {
                                    var benchMark = (from indx in dtIndex.AsEnumerable()
                                                     where indx.Field<System.Decimal>("INDEX_ID") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                                                     select indx).FirstOrDefault();
                                    datarows[0] = (DataRow)benchMark;
                                }

                                dtScheme.Rows.Add(datarows[0].ItemArray);


                                if (isAdditionalBenchMark)
                                {
                                    if (!isCompositeIndex)
                                    {
                                        var addbenchMark = (from indx in dtIndex.AsEnumerable()
                                                            where indx.Field<System.Decimal>("INDEX_ID") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[1])
                                                            select indx).FirstOrDefault();
                                        datarows[1] = (DataRow)addbenchMark;
                                    }

                                    dtScheme.Rows.Add(datarows[1].ItemArray);
                                }

                                dtScheme.AcceptChanges();



                                string SchemeName = dtScheme.Rows[0]["Scheme_Name"].ToString();
                                if (val_SchemeID == "8698" && settingSet == 26)
                                    SchemeName = "Tata Monthly Income Fund (TMIF) - Other than Individual & HUF - Monthly Income Option";
                                if (val_SchemeID == "8698" && settingSet == 2)
                                    SchemeName = "Tata Monthly Income Fund (TMIF) - Individual & HUF - Monthly Income Option";

                                dtScheme.Rows[0]["Scheme_Name"] = SchemeName;


                                // special case 
                                if (dtScheme.Rows.Count >= 2)
                                {
                                    foreach (DataColumn dc in dtScheme.Columns)
                                    {
                                        if (dtScheme.Rows[0][dc].ToString().ToUpper() == "N/A")
                                        {
                                            dtScheme.Rows[1][dc] = "N/A";
                                            if (dtScheme.Rows.Count == 3)
                                                dtScheme.Rows[2][dc] = "N/A";
                                        }
                                    }
                                }



                                DataTable _dttemp = dtScheme.Copy();

                                dsOldFormatFinal.Reset();
                                dsOldFormatFinal.Tables.Add(_dttemp);


                                #endregion


                            }

                            #endregion
                            //objFinalstr.Append(GenerateTable(dsOldFormatFinal, isEquity, LastQuarterDate));
                            //sbFinal.Append(objFinalstr.ToString());
                            //ViewState["FinalExcelString"] = objFinalstr.ToString();
                            sbFinal.Append(GenerateTable(dsOldFormatFinal));

                            sbFinal.Append("<div style='clear:both;'></div>");
                        }


                    }


                    LiteralFinalReturns.Text = sbFinal.ToString();
                    lnkbtnDownload.Visible = true;

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
                                    LiteralDisclaimer.Text += "\u2022 Nifty 500 to the extent of 65% and MSCI World Index to the extent of 35% of the net assets of the Scheme.<br/>";
                                    break;

                            }
                        }
                        LiteralDisclaimer.Text += "<br/>";

                    }
                    else
                        LiteralDisclaimer.Text = "";
                    #endregion

                    //sbFinal

                }
            }
            catch (Exception ex)
            {


            }
        }



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
                //case 9163:
                //case 9164:
                //    returncomp = firstIndexreturn * 0.7 + secondIndexreturn * 0.3;
                //    break;
                default:
                    returncomp = firstIndexreturn * 0.5 + secondIndexreturn * 0.5;
                    break;


            }


            return Math.Round(returncomp, 2);
        }

        #endregion

        #region: Reset Method

        public void Reset()
        {
            ddlNature.SelectedIndex = 0;
            ddlOption.SelectedIndex = 0;
            ddlFunManager.SelectedIndex = 0;
            txtStartDate.Text = "";
            txtInvestment.Text = "10000";
            lnkbtnDownload.Visible = false;
            FillScheme();
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

        #endregion

        #region: Generate  Table Methods

        public string GenerateTable(DataSet dsFormatFinal)
        {
            System.Text.StringBuilder sbResultTable = new System.Text.StringBuilder();

            try
            {
                //if (isEquity)
                //    sbResultTable.AppendLine("Old Format (Equity Fund)");// Equity or Debt
                //else
                //    sbResultTable.AppendLine("Old Format (Debt Fund)");// Equity or Debt
                sbResultTable.AppendLine("<br/>");
                if (dsFormatFinal.Tables[0].Rows.Count > 0)
                {//text-align: center;
                    // sbResultTable.AppendLine("<table width='100%'><tr><td align='center'><centre><bold>" + dsFormatFinal.Tables[0].Rows[0]["SCHEME_NAME"].ToString() + "</bold></centre></td></tr></table>");
                    sbResultTable.AppendLine("<table width='100%'><tr><td colspan='6'></td></tr><tr><td align='left' colspan='6' background='../Images/heading.jpg' height='22' width='500'><span class='style12'>&nbsp;&nbsp;<span class='style4'><img src='../Images/arrow.jpg' /> " + dsFormatFinal.Tables[0].Rows[0]["SCHEME_NAME"].ToString() + "</span></span></td></tr></table>");

                    //<td background="../Images/heading.jpg" height="26" width="960"><span class="style12">&nbsp;&nbsp;<span class="style4">TATA Mutual Fund </span></span></td>
                }
                //sbResultTable.AppendLine("<br/>");

                sbResultTable.AppendLine("<div style='padding-top:5px;'><table class='resultgrid' cellspacing='0' cellpadding='4' rules='all' id='gvOldFormatDebt' style='border-width:1px;border-style:solid;border-collapse:collapse; border-color:#265599'>");

                sbResultTable.AppendLine("<tr>");
                sbResultTable.AppendLine("<th width='30%' class='headerCss' align='left' scope='col'>Scheme Name</th>");
                sbResultTable.AppendLine("<th width='10%' class='headerCss' align='center' scope='col'>Last 1 month</th>");
                sbResultTable.AppendLine("<th width='10%' class='headerCss' align='center' scope='col'>Last 3 month</th>");
                sbResultTable.AppendLine("<th width='10%' class='headerCss' align='center' scope='col'>Last 6 month</th>");
                sbResultTable.AppendLine("<th width='10%' class='headerCss' align='center' scope='col'>Last 1 year</th>");
                sbResultTable.AppendLine("<th width='10%' class='headerCss' align='center' scope='col'>Last 3 year</th>");
                sbResultTable.AppendLine("<th width='10%' class='headerCss' align='center' scope='col'>Last 5 year</th>");
                sbResultTable.AppendLine("<th width='10%' class='headerCss' align='center' scope='col'>Since Inception</th>");

                sbResultTable.AppendLine("</tr>");
                int colorcount = 1;
                #region Old Format
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
                        sbResultTable.AppendLine(Convert.ToString(dr["SCHEME_NAME"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["1 m"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["3 m"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["6 m"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["1 Year"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["3 Year"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["5 Year"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        //sbResultTable.AppendLine(Convert.ToString(dr["Since Inception"]));//modified for tata treasury
                        sbResultTable.AppendLine(Convert.ToString(dr[8]));//last column
                        sbResultTable.AppendLine("</td>");
                        sbResultTable.AppendLine("</tr>");
                        colorcount++;
                    }
                }
                #endregion

                sbResultTable.AppendLine("</table></div>");
                //sbResultTable.AppendLine("<br/>");               

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
            //TestGridView2.RenderControl(hw);            
            // sw.WriteLine(objFinalstr.ToString());
            //Response.Output.Write(sw.ToString() + "<br/>");
            //Response.Output.Write(objFinalstr.ToString() + "<br/>");
            //objFinalstr.Append(Convert.ToString(ViewState["FinalExcelString"]));
            LiteralFinalReturns.RenderControl(hw);

            //objFinalstr.Append(Convert.ToString(ViewState["FinalExcelString"]));

            //objFinalstr.Append("<table width='960px' border='1' cellspacing='0' cellpadding='0' align='center' style='border: #265599 solid 1px;' ><tr><td><table width='100%' border='0' cellspacing='0' cellpadding='0' align='center'><tr>");
            objFinalstr.Append("<table width='100%'><td width='100%' colspan='6' style='padding-left:15px; padding-bottom:5px; padding-top:5px;'><img src='../Images/tatamf.jpg' /></td></tr></table>");
            // objFinalstr.Append("<br/>");
            objFinalstr.Append(sw.ToString());
            objFinalstr.Replace("class='headerCss'", "style ='background: #265599; color: #FFFFFF; font-weight: bold; border: #012258 solid 1px;'");
            objFinalstr.Replace("class='row1'", "style='background: #f5f3f3; color: #353131; border-bottom: #999999 1px dotted;'");
            objFinalstr.Replace("class='row2'", "style='background: #fff; color: #353131; border-bottom: #999999 1px dotted;'");
            objFinalstr.Replace("class='style12'", "style='color: #265599; font-weight: bold;'");
            objFinalstr.Replace("class='style4'", "style='font-family: Tahoma; font-size: 12px;'");
            objFinalstr.Replace("class='resultgrid'", "width='100%' style='width: 99%; font-family: Verdana; font-size: 11px; height: 53px; margin-left: .5%; margin-right: .5%; top: -50px;'");
            //objFinalstr.Append("</td></tr></table></td></tr></table>");


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
                    foreach (string strr in LiteralDisclaimer.Text.Split(new string[] { "<br/>" }, StringSplitOptions.None))
                    {
                        if (strr.Length > 2)
                            objFinalstr.Append(strr.Substring(1) + "<br/>");
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



            //
            //if (ViewState["FinalExcelString"] != null)
            Response.Write(objFinalstr.ToString());
            // Response.Write(Convert.ToString(ViewState["FinalExcelString"]));            
            //Response.Output.Write(sw.ToString() + "<br/>");


            Response.Flush();
            Response.End();


        }
        #endregion

        protected void btnResetForm(object sender, EventArgs e)
        {
            Reset();
        }


    }
}