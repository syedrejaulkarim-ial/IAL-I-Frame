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

namespace iFrames.Tata
{
    public partial class TataCalcOF : System.Web.UI.Page
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
                // FillScheme();
                FillSchemeByOption();
                // FillFundManager();
            }

            // Added to dynamic the ddlSchemeList width
            ddlSchemeList.Attributes.Add("onfocusout", String.Format("focusOut('{0}',{1})", ddlSchemeList.ClientID, ddlSchemeList.Width.Value));
            ddlSchemeList.Attributes.Add("onmouseenter", String.Format("mouseEnter('{0}',{1})", ddlSchemeList.ClientID, ddlSchemeList.Width.Value));

        }

        #endregion

        #region: Dropdown Events

        protected void ddlNature_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillScheme();
            ddlOption.SelectedIndex = 0;
            FillSchemeByOption();

        }
        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSchemeByOption();
            // FillScheme();            
        }
        protected void ddlSchemeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillBenchMarkFM();
        }



        //protected void ddlFunManager_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ddlFunManager.SelectedIndex != 0)
        //        {
        //            using (var schemeFundMngr = new TataCalculatorDataContext())
        //            {
        //                if (ddlFunManager.SelectedValue != "--")
        //                {
        //                    var schenames = (from sm in schemeFundMngr.T_SCHEMES_MASTER_tatas
        //                                     join fm in schemeFundMngr.T_FUND_MASTER_tatas
        //                                     on sm.Fund_Id equals fm.FUND_ID
        //                                     where fm.FUND_MAN == ddlFunManager.SelectedValue
        //                                     && fm.MUTUALFUND_ID == 31
        //                                     select new
        //                                     {
        //                                         sm.Sch_Short_Name,
        //                                         sm.Scheme_Id
        //                                     }).Distinct();

        //                    DataTable dt2 = new DataTable();
        //                    if (schenames.Count() > 0)
        //                    {
        //                        dt2 = schenames.ToDataTable();
        //                        if (dt2.Rows.Count > 0)
        //                        {
        //                            ddlSchemeList.Visible = true;
        //                            LiteralFinalReturns.Text = "";
        //                            lnkbtnDownload.Visible = false;
        //                            ddlSchemeList.DataSource = dt2;
        //                            ddlSchemeList.DataBind();



        //                        }

        //                    }
        //                    else
        //                    {
        //                        ddlSchemeList.Visible = false;
        //                    }

        //                }

        //            }
        //        }
        //        else
        //        {
        //            ddlSchemeList.Visible = false;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //    }
        //}
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
        protected void ResetForm(object sender, EventArgs e)
        {
            Reset();
        }
        protected void ExportExcelClick(object sender, EventArgs e)
        {
            ExportToExcel();
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

                    if (ddlNature.SelectedIndex != 0)// Nature is not Selected
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
                                                 where s.Nav_Check == 3//s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
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
                                dt2 = scheme_name_1.ToDataTable();
                                if (dt2.Rows.Count > 0)
                                {
                                    ddlSchemeList.Visible = true;
                                    ddlSchemeList.DataSource = dt2;
                                    ddlSchemeList.DataBind();
                                }

                            }
                            else
                            {
                                ddlSchemeList.Visible = false;
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
                                dt2 = scheme_name_1.ToDataTable();
                                if (dt2.Rows.Count > 0)
                                {
                                    ddlSchemeList.Visible = true;


                                    ddlSchemeList.Items.Clear();
                                    ddlSchemeList.Items.Add(new ListItem("--", "0"));


                                    if (dt2.Rows.Count > 0)
                                    {
                                        foreach (DataRow drw in dt2.Rows)
                                        {
                                            ddlSchemeList.Items.Add(new ListItem(drw[0].ToString(), drw[1].ToString()));
                                        }
                                    }

                                    //ddlSchemeList.DataSource = dt2;
                                    //ddlSchemeList.DataBind();
                                }

                            }
                            else
                            {
                                ddlSchemeList.Visible = false;
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
                    FillBenchMarkFM();
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

        protected void FillSchemeByOption()
        {

            {
                string selected_nature = ddlNature.SelectedValue;
                string selected_option = ddlOption.SelectedValue;
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


                            DataTable dtt = null;
                            if (fundtable.Count() > 0)
                                dtt = fundtable.ToDataTable();

                            DataTable dt2 = new DataTable();


                            if (ddlOption.SelectedValue != "--")
                            {
                                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                     where s.Nav_Check == 3 && s.Option_Id.ToString() == selected_option//s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     orderby s.Sch_Short_Name
                                                     select new
                                                     {
                                                         s.Sch_Short_Name,
                                                         s.Scheme_Id
                                                     }).Distinct();
                                if (scheme_name_1.Count() > 0)
                                    //dt2 = scheme_name_1.ToDataTable();
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();

                            }
                            else
                            {
                                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                     where s.Nav_Check == 3
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     orderby s.Sch_Short_Name
                                                     select new
                                                     {
                                                         s.Sch_Short_Name,
                                                         s.Scheme_Id
                                                     }).Distinct();
                                if (scheme_name_1.Count() > 0)
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                                //dt2 = scheme_name_1.ToDataTable();
                            }
                            if (dt2.Rows.Count > 0)
                            {
                                ddlSchemeList.Visible = true;
                                ddBenchMark.Visible = true;
                                ddlSchemeList.DataSource = dt2;
                                ddlSchemeList.DataBind();
                             
                               



                            }
                            else
                            {
                                //ddlSchemeList.Visible = false;
                                //ddBenchMark.Visible = false;

                                ddlSchemeList.Items.Clear();
                                ddlSchemeList.Items.Add(new ListItem("--", "--"));

                                ddBenchMark.Items.Clear();
                                ddBenchMark.Items.Add(new ListItem("--", "--"));
                                FundmanegerText.Text = "";
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
                                             select new
                                             {
                                                 t_fund_masters.FUND_ID
                                             });


                            DataTable dtt = null;
                            if (fundtable.Count() > 0)
                                dtt = fundtable.ToDataTable();

                            DataTable dt2 = new DataTable();

                            if (ddlOption.SelectedValue != "--")
                            {
                                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                     where s.Nav_Check == 3 && s.Option_Id.ToString() == selected_option//s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     orderby s.Sch_Short_Name
                                                     select new
                                                     {
                                                         s.Sch_Short_Name,
                                                         s.Scheme_Id
                                                     }).Distinct();
                                if (scheme_name_1.Count() > 0)
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                                //dt2 = scheme_name_1.ToDataTable();

                            }
                            else
                            {
                                var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                     where s.Nav_Check == 3
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     orderby s.Sch_Short_Name
                                                     select new
                                                     {
                                                         s.Sch_Short_Name,
                                                         s.Scheme_Id
                                                     }).Distinct();
                                if (scheme_name_1.Count() > 0)
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                                //dt2 = scheme_name_1.ToDataTable();
                            }
                            if (dt2.Rows.Count > 0)
                            {
                                ddlSchemeList.Visible = true;
                                DataColumn[] pk = new DataColumn[] { dt2.Columns["Scheme_Id"] };
                                dt2.PrimaryKey = pk;

                                dt2.Rows.Remove(dt2.Rows.Find("8698"));
                                DataColumn[] pkk = new DataColumn[] { dt2.Columns["Sch_Short_Name"] };
                                dt2.PrimaryKey = pkk;

                                dt2.Rows.Add("Tata Monthly Income Fund (TMIF) - Individual & HUF - Monthly Income Option", "9999999991");
                                dt2.Rows.Add("Tata Monthly Income Fund (TMIF) - Other than Individual & HUF - Monthly Income Option", "9999999992");


                                DataView dv = dt2.AsDataView();
                                dv.Sort = "Sch_Short_Name asc";
                                dt2 = dv.ToTable();

                                ddlSchemeList.DataSource = dt2;
                                ddlSchemeList.DataBind();
                                //change 24/08/12
                            }
                            else
                            {
                                // ddlSchemeList.Visible = false;
                                ddlSchemeList.Items.Clear();
                                ddlSchemeList.Items.Add(new ListItem("--", "--"));
                            }
                        }
                    }

                    FillBenchMarkFM();
                    LiteralFinalReturns.Text = "";
                    lnkbtnDownload.Visible = false;
                }
                catch (Exception exp)
                {
                }
                finally
                {
                }

                //ddlMode.SelectedIndex = 0;
                //  ddBenchMark.Items.Clear();
                //FundmanegerText.Text = "";
            }
        }
        protected void FillFundManager()
        {
            try
            {
                using (var FundmanagerData = new TataCalculatorDataContext())
                {

                    string selected_Scheme = ddlSchemeList.SelectedValue.ToString();

                    if (selected_Scheme == "9999999991")
                        selected_Scheme = "8698";
                    else if (selected_Scheme == "9999999992")
                        selected_Scheme = "8698";

                    if (ddlSchemeList.SelectedValue != "--")
                    {
                        //var FundManager = (from fm in FundmanagerData.T_FUND_MASTER_tatas
                        //                   where
                        //                   (
                        //                   from cfm in FundmanagerData.T_CURRENT_FUND_MANAGER_tatas
                        //                   join
                        //                       sm in FundmanagerData.T_SCHEMES_MASTER_tatas
                        //                     on cfm.FUND_ID equals sm.Fund_Id
                        //                   where cfm.LATEST_FUNDMAN == true && sm.Nav_Check != 2 && sm.Scheme_Id == Convert.ToDecimal(ddlSchemeList.SelectedValue)
                        //                   select cfm.FUND_ID
                        //                     ).Contains(fm.FUND_ID) && fm.FUND_MAN != string.Empty
                        //                       && fm.MUTUALFUND_ID == 31
                        //                   orderby fm.FUND_MAN
                        //                   select new
                        //                   {
                        //                       fm.FUND_MAN
                        //                   }).Distinct().ToArray();

                        var FundManager = (from fd in FundmanagerData.T_FUND_MANAGER_tatas
                                           join
                                        cfm in FundmanagerData.T_CURRENT_FUND_MANAGER_tatas on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                           join
                                           fms in FundmanagerData.T_FUND_MASTER_tatas on cfm.FUND_ID equals fms.FUND_ID
                                           join
                                           sm in FundmanagerData.T_SCHEMES_MASTER_tatas on fms.FUND_ID equals sm.Fund_Id
                                           where
                                           sm.Scheme_Id == Convert.ToDecimal(selected_Scheme) && cfm.LATEST_FUNDMAN == true
                                           select new
                                           {
                                               fd.FUND_MANAGER_NAME
                                           }).Distinct().ToArray();//fd.FUND_MANAGER_NAME;


                        FundmanegerText.Text = "";
                        if (FundManager.Count() > 0)
                        {
                            foreach (var fn in FundManager.AsEnumerable())
                            {
                                FundmanegerText.Text += fn.FUND_MANAGER_NAME.ToString() + " , ";
                            }
                            FundmanegerText.Text = FundmanegerText.Text.TrimEnd(' ', ',');
                        }
                        else
                        {
                            FundmanegerText.Text = "";
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

        protected void FillBenchMarkFM()
        {


            string selected_Scheme = ddlSchemeList.SelectedValue.ToString();

            if (selected_Scheme == "9999999991")
                selected_Scheme = "8698";
            else if (selected_Scheme == "9999999992")
                selected_Scheme = "8698";


                if (selected_Scheme.Trim('-').Length == 0)
                {
                    ddBenchMark.Items.Clear();
                    return;
                }
                FillFundManager();
                using (var tata = new TataCalculatorDataContext())
                {
                    //var index_name = (from t_index_masters in tata.T_INDEX_MASTER_tatas
                    //                  where
                    //                    t_index_masters.INDEX_ID ==
                    //                      ((from t_schemes_indexes in tata.T_SCHEMES_INDEX_tatas
                    //                        where
                    //                          t_schemes_indexes.SCHEME_ID == Convert.ToDecimal(ddlSchemeList.SelectedValue)
                    //                        select new
                    //                        {
                    //                            t_schemes_indexes.INDEX_ID
                    //                        }).Take(1).First().INDEX_ID)
                    //                  select new
                    //                  {

                    //                      t_index_masters.INDEX_NAME,
                    //                      t_index_masters.INDEX_ID
                    //                  }).Take(1);

                    var index_name = (from t_index_masters in tata.T_INDEX_MASTER_tatas
                                      where
                                          ((from t_schemes_indexes in tata.T_SCHEMES_INDEX_tatas
                                            where
                                              t_schemes_indexes.SCHEME_ID == Convert.ToDecimal(selected_Scheme)
                                            select t_schemes_indexes.INDEX_ID
                                            ).Contains(t_index_masters.INDEX_ID))
                                      select new
                                      {

                                          t_index_masters.INDEX_NAME,
                                          t_index_masters.INDEX_ID
                                      });

                    //ddBenchMark.Items.Clear();
                    //ddBenchMark.Items.Add(new ListItem(index_name.Single().INDEX_NAME.ToString(), index_name.Single().INDEX_ID.ToString()));

                    ddBenchMark.Items.Clear();
                    if (index_name.Count() == 1)
                    {
                        ddBenchMark.Items.Add(new ListItem(index_name.Single().INDEX_NAME.ToString(), index_name.Single().INDEX_ID.ToString()));
                    }
                    else if (index_name.Count() > 1)
                    {
                        foreach (var v in index_name)
                        {
                            ddBenchMark.Items.Add(new ListItem(v.INDEX_NAME.ToString(), v.INDEX_ID.ToString()));
                        }
                    }




                    if (selected_Scheme == "9079" || selected_Scheme == "9080" || selected_Scheme == "17802" || selected_Scheme == "17803")
                    {
                        ddBenchMark.Items.Clear();
                        ddBenchMark.Items.Add(new ListItem("Combination of Nifty 500 & MSCI World Index", "15"));
                    }

                    else if (selected_Scheme == "9161" || selected_Scheme == "9162" || selected_Scheme == "17812" || selected_Scheme == "17813")
                    {
                        ddBenchMark.Items.Clear();
                        ddBenchMark.Items.Add(new ListItem("Combination of MSCI Emerging Market Index & S&P BSE Sensex", "13"));
                    }

                    else if (selected_Scheme == "9163" || selected_Scheme == "9164" || selected_Scheme == "17814" || selected_Scheme == "17815")
                    {
                        ddBenchMark.Items.Clear();
                        ddBenchMark.Items.Add(new ListItem("Combination of S&P BSE Sensex & MSCI Emerging Market Index", "13"));
                    }

                    else if (selected_Scheme == "13530" || selected_Scheme == "13531")
                    {
                        ddBenchMark.Items.Clear();
                        ddBenchMark.Items.Add(new ListItem("Combination of CRISIL Liquid Fund Index & S&P BSE Sensex", "13"));
                    }
                }






            
        }



        #endregion

        #region: Fetch Methods

        public void FetchreturnInOldFormat()
        {
            System.Text.StringBuilder sbFinal = new System.Text.StringBuilder();

            bool showSpecificDisclaimer = false;
            try
            {
                string val_SchemeID = ddlSchemeList.SelectedItem.Value;


               

                List<decimal> lstschmId = new List<decimal>();
                lstschmId.Clear();
                //DateTime _todate = Convert.ToDateTime(ddlQtrEnd.SelectedValue + ddlYearEnd.SelectedValue);
                DateTime _todate = Convert.ToDateTime(txtStartDate.Text);

                if (val_SchemeID != string.Empty)
                {
                    val_SchemeID = val_SchemeID.TrimEnd(',');

                    using (var IndexDC = new TataCalculatorDataContext())
                    {
                        var lastInceptionDate = (from tsm in IndexDC.T_SCHEMES_MASTER_tatas
                                                 where lstschmId.Contains(tsm.Scheme_Id)
                                                 select tsm.Launch_Date).Min();

                        TimeSpan Daydiff = _todate.Subtract(Convert.ToDateTime(lastInceptionDate));

                        if (Daydiff.Days <= 0)
                        {
                            lblMessage.Text = "Please select date greater than " + Convert.ToDateTime(lastInceptionDate).ToString("dd-MMMM-yyyy", CultureInfo.InvariantCulture);
                            return;
                        }
                    }
                }

                DataTable dtFinal = new DataTable();
                DataTable dtScheme = new DataTable("tblScheme");
                DataTable dtIndex = new DataTable("tblIndex");
                DataSet dsOldFormatFinal = new DataSet();
                bool modifiedInceptionDate = false;
                int settingSet = 2;
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

                string strRollingPeriodin = string.Empty;
                strRollingPeriodin = "1 m,3 m,6 m,1 YYYY,3 YYYY,5 YYYY,0 Si";
                DateTime LastQuarterDate = GetLastQuarterDates(_todate);

                if (ddlSchemeList.SelectedValue == "9999999991")
                {
                    settingSet = 2;
                    val_SchemeID = "8698";
                }
                else if (ddlSchemeList.SelectedValue == "9999999992")
                {
                    settingSet = 26;
                    val_SchemeID = "8698";
                }


                using (var IndexData = new TataCalculatorDataContext())
                {

                    var IndexID = (from indexid in IndexData.T_SCHEMES_INDEX_tatas
                                   where Convert.ToString(indexid.SCHEME_ID) == val_SchemeID
                                   select new
                                   {
                                       indexid.INDEX_ID
                                   }).First().INDEX_ID;

                    var IndexName = (from tim in IndexData.T_INDEX_MASTER_tatas
                                     where tim.INDEX_ID == IndexID
                                     select tim.INDEX_NAME).First();

                    string _indexName = string.Empty;
                    _indexName = IndexName.ToString();


                    var strNaturedata = (from natr in IndexData.T_SCHEMES_NATURE_tatas
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

                    strNature = strNaturedata.ToString();

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

                    string IndexBenchmarlIds = null;
                    string _indexId = string.Empty;
                    _indexId = IndexID.ToString();
                    bool isAdditionalBenchMark = false;
                    bool isCompositeIndex = false;





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






                    if (strNature == "Equity" || strNature == "Balanced" || strNature == "Dynamic/Asset Allocation")
                    {

                        if (_indexName.ToUpper().Contains("BSE"))
                        {
                            if (_indexName.ToUpper() != "S&P BSE SENSEX" && !_indexName.ToUpper().Contains("MSCI") && (!(val_SchemeID == "13530" || val_SchemeID == "13531")))
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
                                if (val_SchemeID != "9079" && val_SchemeID != "9080" && val_SchemeID != "9161" && val_SchemeID != "9162" && val_SchemeID != "9163" && val_SchemeID != "9164" && val_SchemeID != "17802" && val_SchemeID != "17803" && val_SchemeID != "17812" && val_SchemeID != "17813" && val_SchemeID != "17814" && val_SchemeID != "17815")
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
                    if (_indexId.Trim() == "32")
                    {
                        IndexBenchmarlIds = IndexBenchmarlIds.Replace("32", "1T");
                    }


                    // code on 02.07.2013 
                    if (val_SchemeID == "15140" || val_SchemeID == "15442")
                    {
                        isAdditionalBenchMark = true;
                        _indexId = "25,134";
                        IndexBenchmarlIds = _indexId;
                    }


                    DateTime allotDate;

                    var alotdate = (from ind in IndexData.T_SCHEMES_MASTER_tatas
                                    where ind.Scheme_Id.ToString() == val_SchemeID
                                    select ind.Launch_Date).Single();

                    if (alotdate == null)
                    {
                        lblMessage.Text = "Launch date is not available";
                        return;
                    }
                    else
                        allotDate = Convert.ToDateTime(alotdate.ToString());


                    //// Specal case of Tata Treasury Manager Fund - SHIP - Growth SI need to show -30 April 2009
                    //if (val_SchemeID == "9042")
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



                    //if (val_SchemeID == "9042")
                    if(modifiedInceptionDate)
                    {
                        string daydiff = string.Empty;
                        TimeSpan noofday;
                        //noofday = LastQuarterDate.Subtract(allotDate);
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


                    Int32 oval = 0;
                    SqlCommand cmdScheme = new SqlCommand();
                    cmdScheme = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_TATA", cn);
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
                    sqldaScheme.Fill(dtScheme);


                    string SchemeName = string.Empty;
                    SchemeName = dtScheme.Rows[0]["SCHEME_NAME"].ToString();
                    if (settingSet == 26 && val_SchemeID == "8698")
                    {
                        SchemeName = "Tata Monthly Income Fund (TMIF) - Other than Individual & HUF - Monthly Income Option";
                    }
                    else if (settingSet == 2 && val_SchemeID == "8698")
                    {
                        SchemeName = "Tata Monthly Income Fund (TMIF) - Individual & HUF - Monthly Income Option";
                    }


                    dtScheme.Rows[0]["SCHEME_NAME"] = SchemeName;

                    dsOldFormatFinal.Tables.Add(dtScheme);


                    strRollingPeriodin = strRollingPeriodin.Replace("0 Si", strdaydiff);

                    SqlCommand cmdIndex = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_TATA", cn);
                    cmdIndex.CommandType = CommandType.StoredProcedure;
                    cmdIndex.CommandTimeout = 2000;
                    cmdIndex.Parameters.Add(new SqlParameter("@IndexIDs", IndexBenchmarlIds));
                    cmdIndex.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                    cmdIndex.Parameters.Add(new SqlParameter("@DateFrom", allotDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
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
                    sqldaIndex.Fill(dtIndex);
                    IndexBenchmarlIds = IndexBenchmarlIds.Replace("T", "");


                    IEnumerable<DataRow> _testdtIndex = from ib in IndexBenchmarlIds.Split(',').AsEnumerable()
                                                        join tst in dtIndex.AsEnumerable()
                                                        on ib equals tst.Field<decimal>("Index_id").ToString()
                                                        select tst;
                    DataTable _dttt = new DataTable("tblIndex");
                    _dttt = _testdtIndex.CopyToDataTable<DataRow>();
                    dtIndex.Clear();
                    dtIndex = _dttt;
                    dtIndex.TableName = "tblIndex";

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

                    // special case 
                    if (dtIndex.Rows.Count >= 1)
                    {
                        foreach (DataColumn dc in dtIndex.Columns)
                        {
                            if (dc.Ordinal != 0 && dc.Ordinal != 1 && dc.Ordinal != 9)
                            {
                                if (dsOldFormatFinal.Tables[0].Rows[0][dc.Ordinal].ToString().ToUpper() == "N/A")
                                {
                                    dtIndex.Rows[0][dc] = "N/A";
                                    if (dtIndex.Rows.Count == 2)
                                        dtIndex.Rows[1][dc] = "N/A";
                                }
                            }
                        }
                    }
                    dtIndex.AcceptChanges();

                    dsOldFormatFinal.Tables.Add(dtIndex);
                    //dsOldFormatFinal.Tables.Add();
                }

                sbFinal.Append(GenerateTable(dsOldFormatFinal));
                //sbFinal.Append("<div style='clear:both;'></div>");
                lnkbtnDownload.Visible = true;
                LiteralFinalReturns.Text = sbFinal.ToString();
                sbFinal = null;
                #region :Disclaimer
                if (modifiedInceptionDate || showSpecificDisclaimer)
                {
                    // LiteralDisclaimer.Text = "Disclaimer :<br/>";
                    //LiteralDisclaimer.Text = "<table width='100%'><tr><td colspan='6'></td></tr><tr><td align='left' colspan='6' background='../Images/heading.jpg' height='22' width='500'><span class='style12'>&nbsp;&nbsp;<span class='style4'><img src='../Images/arrow.jpg' /> Disclaimer</span></span><br/></td></tr><tr><td align='centre' style='font:verdena; font-size:11px; padding-left:20px;'>";

                    LiteralDisclaimer.Text = "";
                    switch (Convert.ToInt32(val_SchemeID))
                    {

                        case 9114:
                            LiteralDisclaimer.Text += "\u2022 On 16th April, 2008, the units had become zero under TFIPA3-RIP (Growth) plan and new units were alloted on 20th May 2008 at face value. Hence returns are computed from 20th May 2008.";
                            break;
                        case 9121:
                            LiteralDisclaimer.Text += "\u2022 On 31st December 2008, the units had become zero under TFIPB3-IP (Quarterly Dividend) plan and new units were alloted on 23rd March, 2010 at face value. Hence returns are computed from 23rd March, 2010.";
                            break;
                        case 9133:
                            LiteralDisclaimer.Text += "\u2022 On 24th October, 2008 units had become zero under TFIPC2 IP (Half Yearly Dividend) plan and new units were alloted on 21st January 2011 at face value. Hence returns are computed from 21st January 2011.";
                            break;
                        case 9105:
                            LiteralDisclaimer.Text += "\u2022 On 4th March, 2009 the units had become zero under TFIPA2-IP (Monthly Dividend) plan and new units were allotted on 4th June, 2010 at face value. Hence returns are computed from 4th June, 2010.";
                            break;
                        case 9116:
                            LiteralDisclaimer.Text += "\u2022 On 23 October 2008, the units had become zero under TFIPA3-IP (Growth) plan and new units were allotted on 09th June, 2010 at face value. Hence returns are computed from 09th June, 2010.";
                            break;
                        case 9100:
                            LiteralDisclaimer.Text += "\u2022 On 17th November, 2009, the units had become zero under TFIPB2-IP (Monthly Dividend) plan and new units were allotted on 18th June, 2010 at face value. Hence returns are computed from 18th June, 2010.";
                            break;
                        case 9140:
                            LiteralDisclaimer.Text += "\u2022 On 25th May, 2010 units had become zero under TFIPC3-IP (Growth) plan and new units were allotted on 26th May 2011 at face value. Hence returns are computed from 26th May 2011.";
                            break;
                        case 9122:
                            LiteralDisclaimer.Text += "\u2022 On 23rd December 2010, the units had become zero under TFIPB3-IP (Growth) plan and new units were allotted on 31st December, 2010 at face value. Hence returns are computed from 31st December, 2010.";
                            break;
                        case 9102:
                            LiteralDisclaimer.Text += "\u2022 On 15th December 2010, the units had become zero under TFIPB2-IP (Growth) plan and new units were allotted on 1st March, 2011 at face value. Hence returns are computed from 1st March, 2011.";
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
        //private void PerformanceReturn(string schmeId, string indexId, DateTime allotDate)
        //{

        //    DateTime LastQtrEndDate = GetLastQuarterDates(_toDate);
        //    //string AdditionalBechmarkid = "12";// for the Nifty and for bse 100 it is 33 and bse sensex it is 13
        //    double? sinceInceptionIndex = null, sinceInceptionAddlIndex = null;
        //    double? sinceInception = null;
        //    DateTime LastQtrEndDateInRecord = LastQtrEndDate;//= null;
        //    SqlCommand sqlcmd, sqlcmd1;
        //    //string sqlqry = "select top(1) Nav_Date from T_nav_div where scheme_id='" + schmeId + "'and nav_date<= '" + LastQtrEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "' order by nav_date desc";
        //    //sqlcomnd = new SqlCommand(sqlqry, cn);
        //    //LastQtrEndDateInRecord = (DateTime)sqlcomnd.ExecuteScalar();

        //    string datediff = string.Empty;
        //    // LastQtrEndDateInRecord = GetLatestNavdate(LastQtrEndDate, schmeId);
        //    //if (LastQtrEndDateInRecord == LastQtrEndDate)
        //    //    datediff = "365 d";
        //    //else
        //    //    datediff = "364 d";

        //    using (var principl = new PrincipalCalcDataContext())
        //    {
        //        var sbNtrid = (from tfm in principl.T_FUND_MASTERs
        //                       where tfm.FUND_ID ==
        //                       (from tsm in principl.T_SCHEMES_MASTERs
        //                        where tsm.Scheme_Id == Convert.ToDecimal(schmeId)
        //                        select new
        //                        {
        //                            tsm.Fund_Id
        //                        }
        //                       ).First().Fund_Id
        //                       select new { tfm.SUB_NATURE_ID }).First().SUB_NATURE_ID;
        //        SubNatureId = sbNtrid.ToString();
        //    }

        //    //monthDate
        //    strNature = ddlNature.SelectedValue;
        //    try
        //    {

        //        //sqlcmd = new SqlCommand("MFIE_SP_FIXPERIODICROLLINGRETURNS", cn);
        //        //sqlcmd.CommandType = CommandType.StoredProcedure;
        //        //sqlcmd.CommandTimeout = 2000;
        //        //sqlcmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
        //        //sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
        //        //sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
        //        //sqlcmd.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDateInRecord.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        //        //sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriod", ""));
        //        //sqlcmd.Parameters.Add(new SqlParameter("@FixPeriodic", "YoY"));
        //        //sqlcmd.Parameters.Add(new SqlParameter("@RoundTill", 4));
        //        //sqlcmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));
        //        // string datediff = "365 d";
        //        int oval = 0;
        //        string IndexBenchmarlIds = null;
        //        Decimal dcmlIndexid = 0;
        //        DataTable nwdataTble = null, nwIndexDataTble = null, odatatble = null, oindexDataTable = null;
        //        DataSet dset1 = new DataSet();
        //        DataSet dset2 = new DataSet();
        //        DataSet dst1 = new DataSet();
        //        DataSet dst2 = new DataSet();

        //        if (strNature == "Equity" || strNature == "Balanced" || strNature == "Dynamic/Asset Allocation")
        //        {
        //            dcmlIndexid = (strIndexid != "12" ? 12 : 13);

        //            if (indexId.ToString() == "12")
        //                IndexBenchmarlIds = "12,13";
        //            else if (indexId.ToString() == "13")
        //                IndexBenchmarlIds = "13,12";
        //            else
        //                IndexBenchmarlIds = indexId + ",12";
        //        }

        //        else
        //        {

        //            if (SubNatureId == "13" || strNature == "Liquid")// || SubNatureId == "41" || SubNatureId == "15")
        //            {
        //                IndexBenchmarlIds = indexId + ",135";
        //                dcmlIndexid = 135;
        //            }
        //            else //if (SubNatureId == "4")
        //            {
        //                IndexBenchmarlIds = indexId + ",134";
        //                dcmlIndexid = 134;
        //            }

        //        }

        //        List<double> schemval = new List<double>();
        //        List<double> benchval = new List<double>();
        //        List<double> adbenchval = new List<double>();

        //        DateTime tempdate = LastQtrEndDateInRecord;
        //        DateTime tempdate2 = LastQtrEndDateInRecord;
        //        //int rcount = 0;
        //        DateTime[] arrdate = new DateTime[6];


        //        for (int i = 0; i < 3; i++)
        //        {

        //            //TimeSpan dateDiff2 = _toDateLs.Subtract(_fromDateLs);
        //            //int day2 = dateDiff2.Days;


        //            arrdate[2 * i + 0] = tempdate;
        //            arrdate[2 * i + 1] = tempdate2;


        //            TimeSpan dayyDiff2 = tempdate2.Subtract(tempdate);
        //            int dday2 = dayyDiff2.Days;

        //            datediff = dday2 + " d";

        //            //if (LastQtrEndDateInRecord == LastQtrEndDate)
        //            //    datediff = "365 d";
        //            //else
        //            //    datediff = "364 d";

        //            sqlcmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", cn);
        //            sqlcmd.CommandType = CommandType.StoredProcedure;
        //            sqlcmd.CommandTimeout = 2000;
        //            sqlcmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
        //            sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
        //            sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
        //            sqlcmd.Parameters.Add(new SqlParameter("@DateTo", tempdate2.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        //            sqlcmd.Parameters.Add(new SqlParameter("@RoundTill", 6));
        //            sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriodin", datediff));
        //            sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
        //            sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
        //            sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
        //            sqlcmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
        //            sqlcmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


        //            //sqlcmd1 = new SqlCommand("MFIE_SP_FIXEDPERIODICINDEXROLLINGRETURN", cn);
        //            //sqlcmd1.CommandType = CommandType.StoredProcedure;
        //            //sqlcmd1.CommandTimeout = 2000;
        //            //sqlcmd1.Parameters.Add(new SqlParameter("@IndexIDs", IndexBenchmarlIds));
        //            //sqlcmd1.Parameters.Add(new SqlParameter("@SettingSetID", 2));
        //            //sqlcmd1.Parameters.Add(new SqlParameter("@DateFrom", ""));
        //            //sqlcmd1.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDateInRecord.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        //            //sqlcmd1.Parameters.Add(new SqlParameter("@RollingPeriod", ""));
        //            //sqlcmd1.Parameters.Add(new SqlParameter("@FixPeriodic", "YoY"));
        //            //sqlcmd1.Parameters.Add(new SqlParameter("@RoundTill", 4));
        //            //sqlcmd1.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));




        //            sqlcmd1 = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", cn);
        //            sqlcmd1.CommandType = CommandType.StoredProcedure;
        //            sqlcmd1.CommandTimeout = 2000;
        //            sqlcmd1.Parameters.Add(new SqlParameter("@IndexIDs", IndexBenchmarlIds));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@SettingSetID", 2));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@DateFrom", ""));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@DateTo", tempdate2.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@RoundTill", 6));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", datediff));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
        //            sqlcmd1.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



        //            dset1.Clear();
        //            dset2.Clear();
        //            dset1.Reset();
        //            dset2.Reset();
        //            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlcmd);
        //            sqlDa.Fill(dset1);
        //            sqlDa.SelectCommand = sqlcmd1;
        //            sqlDa.Fill(dset2);

        //            nwdataTble = null;
        //            nwIndexDataTble = null;
        //            nwdataTble = dset1.Tables[0];
        //            nwIndexDataTble = dset2.Tables[0];


        //            if (nwdataTble.Rows[0][dday2 + " Day"].ToString() != "N/A")
        //                schemval.Add(Convert.ToDouble(nwdataTble.Rows[0][dday2 + " Day"]));

        //            for (int k = 0; k < 2; k++)
        //            {
        //                if (nwIndexDataTble.Rows[k][0].ToString() == strIndexid && nwIndexDataTble.Rows[k][dday2 + " Day"].ToString() != "")
        //                {
        //                    benchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[k][dday2 + " Day"].ToString()));
        //                }
        //                if (nwIndexDataTble.Rows[k][0].ToString() == dcmlIndexid.ToString() && nwIndexDataTble.Rows[k][dday2 + " Day"].ToString() != "")
        //                {
        //                    adbenchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[k][dday2 + " Day"].ToString()));
        //                }
        //            }


        //            //if (nwdataTble.Rows[i][dday2 + " Day"].ToString() !="N/A")
        //            //schemval.Add(Convert.ToDouble(nwdataTble.Rows[i][dday2 + " Day"]));

        //            //for( int k = 0;k<2;k++)
        //            //{
        //            //    if (nwIndexDataTble.Rows[2 * i + k][0].ToString() == strIndexid && nwIndexDataTble.Rows[2 * i + k][dday2 + " Day"].ToString() != "")
        //            //    {
        //            //        benchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[2 * i + k][dday2 + " Day"].ToString()));
        //            //    }
        //            //    if (nwIndexDataTble.Rows[2 * i + k][0].ToString() == dcmlIndexid.ToString() && nwIndexDataTble.Rows[2 * i + k][dday2 + " Day"].ToString() != "")
        //            //    {
        //            //        adbenchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[2 * i + k][dday2 + " Day"].ToString()));
        //            //    }
        //            //    //rcount++;
        //            //}





        //            //foreach (DataRow dr in nwIndexDataTble.Rows)
        //            //{
        //            //    if (dr[0].ToString() == strIndexid && dr[dday2 + " Day"].ToString() !="")
        //            //    {
        //            //        benchval.Add(Convert.ToDouble(dr[dday2 + " Day"].ToString()));
        //            //    }
        //            //    if (dr[0].ToString() == dcmlIndexid.ToString() && dr[dday2 + " Day"].ToString() != "")
        //            //    {
        //            //        adbenchval.Add(Convert.ToDouble(dr[dday2 + " Day"].ToString()));
        //            //    }
        //            //}


        //            //var vindexnavVal = (from dt in nwIndexDataTble.AsEnumerable()
        //            //                    where dt.Field<Decimal>("index_id") == Convert.ToDecimal(strIndexid)
        //            //                    select new
        //            //{
        //            //    value = dt.Field<double>(dday2 + " Day")
        //            //}).Single();

        //            ////vindexnavVal.v

        //            //var vaddlindexnavVal = (from dt in nwIndexDataTble.AsEnumerable()
        //            //                        where dt.Field<Decimal>("index_id") == dcmlIndexid
        //            //                        select new
        //            //                            {
        //            //                                value = dt.Field<double>(dday2 + " Day")
        //            //                            }).Single();


        //            //benchval.Add(Convert.ToDouble(vindexnavVal.value));
        //            //adbenchval.Add(Convert.ToDouble(vaddlindexnavVal.value));

        //        }

        //        ////addlIndexDataTble = dset2.Tables[0];
        //        //double[] navVal = (from dt in nwdataTble.AsEnumerable()
        //        //                   select dt.Field<double>("nav")).Take(3).ToArray();

        //        //double[] indexnavVal = (from dt in nwIndexDataTble.AsEnumerable()
        //        //                        where dt.Field<Decimal>("index_id") == Convert.ToDecimal(strIndexid)
        //        //                        select dt.Field<double>("index_value")).Take(3).ToArray();



        //        //double[] addlindexnavVal = (from dt in nwIndexDataTble.AsEnumerable()
        //        //                            where dt.Field<Decimal>("index_id") == dcmlIndexid
        //        //                            select dt.Field<double>("index_value")).Take(3).ToArray();


        //        double[] navVal = schemval.ToArray();
        //        double[] indexnavVal = benchval.ToArray();
        //        double[] addlindexnavVal = adbenchval.ToArray();

        //        //string[] strindexnavVal = ;


        //        //string[] dateQtr = (from mwt in nwdataTble.AsEnumerable()
        //        //                    select mwt.Field<string>("Nav_date")).Take(4).ToArray();



        //        SqlCommand cmdd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", cn);
        //        cmdd.CommandType = CommandType.StoredProcedure;
        //        cmdd.CommandTimeout = 2000;
        //        cmdd.Parameters.Add(new SqlParameter("@SchemeIDs", strSchid));
        //        cmdd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
        //        cmdd.Parameters.Add(new SqlParameter("@DateFrom", ""));
        //        cmdd.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDateInRecord.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        //        cmdd.Parameters.Add(new SqlParameter("@RoundTill", 6));
        //        cmdd.Parameters.Add(new SqlParameter("@RollingPeriodin", "0 Si"));
        //        cmdd.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
        //        cmdd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
        //        cmdd.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
        //        cmdd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
        //        cmdd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));




        //        SqlCommand indexCmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", cn);
        //        indexCmd.CommandType = CommandType.StoredProcedure;
        //        indexCmd.CommandTimeout = 2000;
        //        indexCmd.Parameters.Add(new SqlParameter("@IndexIDs", IndexBenchmarlIds));
        //        indexCmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
        //        indexCmd.Parameters.Add(new SqlParameter("@DateFrom", allotDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        //        indexCmd.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDateInRecord.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        //        indexCmd.Parameters.Add(new SqlParameter("@RoundTill", 6));
        //        indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", "0 Si"));
        //        indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
        //        indexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
        //        indexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
        //        indexCmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
        //        indexCmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



        //        SqlDataAdapter sqlda = new SqlDataAdapter(cmdd);


        //        sqlda.Fill(dst1);
        //        sqlda.SelectCommand = indexCmd;
        //        sqlda.Fill(dst2);
        //        odatatble = dst1.Tables[0];
        //        oindexDataTable = dst2.Tables[0];



        //        if (odatatble.Rows[0]["Since Inception"].ToString() != "N/A")
        //            sinceInception = Convert.ToDouble(odatatble.Rows[0]["Since Inception"]);

        //        if (oindexDataTable.Rows[0]["Index_id"].ToString() == strIndexid)
        //        {
        //            if (oindexDataTable.Rows[0]["0 Si"].ToString() != "N/A")
        //                sinceInceptionIndex = Convert.ToDouble(oindexDataTable.Rows[0]["0 Si"]);

        //            if (oindexDataTable.Rows[1]["0 Si"].ToString() != "N/A")
        //                sinceInceptionAddlIndex = Convert.ToDouble(oindexDataTable.Rows[1]["0 Si"]);
        //        }
        //        else
        //        {
        //            if (oindexDataTable.Rows[0]["0 Si"].ToString() != "N/A")
        //                sinceInceptionAddlIndex = Convert.ToDouble(oindexDataTable.Rows[0]["0 Si"]);

        //            if (oindexDataTable.Rows[1]["0 Si"].ToString() != "N/A")
        //                sinceInceptionIndex = Convert.ToDouble(oindexDataTable.Rows[1]["0 Si"]);
        //        }



        //        //TimeSpan dateDiff = GetLastQuarterDates().Subtract(allotDate);


        //        Double? CompundReturnSI = null;
        //        Double? CompundReturnSIndex = null;
        //        Double? CompundReturnSIAddIndex = null;


        //        DataTable perfDataTable = new DataTable();
        //        perfDataTable.Columns.Add("qtryear", typeof(string));
        //        perfDataTable.Columns.Add("navReturn", typeof(string));
        //        perfDataTable.Columns.Add("navp2pReturn", typeof(string));
        //        perfDataTable.Columns.Add("indexReturn", typeof(string));
        //        perfDataTable.Columns.Add("indexp2pReturn", typeof(string));
        //        perfDataTable.Columns.Add("addlindexReturn", typeof(string));
        //        perfDataTable.Columns.Add("addlindexp2pReturn", typeof(string));

        //        //double?[] dindexnavVal = new double?[3];
        //        //double?[] daddlindexnavVal = new double?[3];

        //        //Code added for dealing non index value

        //        List<double?> ddindexnavVal = new List<double?>();
        //        List<double?> ddaddlindexnavVal = new List<double?>();

        //        if (indexnavVal.Count() == 0)
        //        {
        //            //indexnavVal = new double[3] { 0, 0, 0 };
        //            ddindexnavVal.Add(null); ddindexnavVal.Add(null); ddindexnavVal.Add(null);
        //        }
        //        else
        //        {
        //            foreach (double d in indexnavVal)
        //                ddindexnavVal.Add(d);
        //        }

        //        if (addlindexnavVal.Count() == 0)
        //        {
        //            ddaddlindexnavVal.Add(null); ddaddlindexnavVal.Add(null); ddaddlindexnavVal.Add(null);
        //            // addlindexnavVal = new double[3] { 0, 0, 0 };
        //        }
        //        else
        //        {
        //            foreach (double d in addlindexnavVal)
        //                ddaddlindexnavVal.Add(d);
        //        }





        //        //if (navVal.Count() > 0 || ddindexnavVal.Count > 0)
        //        //{

        //        //    //addlindexnavVal[0] = 0; addlindexnavVal[1] = 0; addlindexnavVal[2] = 0;

        //        //    if (navVal.Count() == 0)
        //        //        perfDataTable.Rows.Add(LastQtrEndDateInRecord.AddYears(-1).ToString("dd-MMM-yy") + " - " + LastQtrEndDateInRecord.ToString("dd-MMM-yy"), "N.A.", "N.A.", ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
        //        //    else
        //        //        perfDataTable.Rows.Add(LastQtrEndDateInRecord.AddYears(-1).ToString("dd-MMM-yy") + " - " + LastQtrEndDateInRecord.ToString("dd-MMM-yy"), navVal[0].ToString("n2"), (10000 + navVal[0] * 100).ToString("n2"), ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
        //        //    if (navVal.Count() > 1)
        //        //        perfDataTable.Rows.Add(LastQtrEndDateInRecord.AddYears(-2).ToString("dd-MMM-yy") + " - " + LastQtrEndDateInRecord.AddYears(-1).ToString("dd-MMM-yy"), navVal[1].ToString("n2"), (10000 + navVal[1] * 100).ToString("n2"), ddindexnavVal[1] == null ? "" : Convert.ToDouble(ddindexnavVal[1]).ToString("n2"), ddindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[1]) * 100).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[1]).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[1]) * 100).ToString("n2"));
        //        //    if (navVal.Count() > 2)
        //        //        perfDataTable.Rows.Add(LastQtrEndDateInRecord.AddYears(-3).ToString("dd-MMM-yy") + " - " + LastQtrEndDateInRecord.AddYears(-2).ToString("dd-MMM-yy"), navVal[2].ToString("n2"), (10000 + navVal[2] * 100).ToString("n2"), ddindexnavVal[2] == null ? "" : Convert.ToDouble(ddindexnavVal[2]).ToString("n2"), ddindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[2]) * 100).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[2]).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[2]) * 100).ToString("n2"));

        //        //    //perfDataTable.Rows.Add("Since Inception to " + GetLastQuarterDates().ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
        //        //    perfDataTable.Rows.Add("Since Inception as on <br/> " + allotDate.ToString("dd-MMM-yy") + " to " + LastQtrEndDateInRecord.ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
        //        //}



        //        if (navVal.Count() > 0 || ddindexnavVal.Count > 0)
        //        {

        //            //addlindexnavVal[0] = 0; addlindexnavVal[1] = 0; addlindexnavVal[2] = 0;

        //            if (navVal.Count() == 0)
        //                perfDataTable.Rows.Add(arrdate[0].ToString("dd-MMM-yy") + " - " + arrdate[1].ToString("dd-MMM-yy"), "N.A.", "N.A.", ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
        //            else
        //                perfDataTable.Rows.Add(arrdate[0].ToString("dd-MMM-yy") + " - " + arrdate[1].ToString("dd-MMM-yy"), navVal[0].ToString("n2"), (10000 + navVal[0] * 100).ToString("n2"), ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
        //            if (navVal.Count() > 1)
        //                perfDataTable.Rows.Add(arrdate[2].ToString("dd-MMM-yy") + " - " + arrdate[3].ToString("dd-MMM-yy"), navVal[1].ToString("n2"), (10000 + navVal[1] * 100).ToString("n2"), ddindexnavVal[1] == null ? "" : Convert.ToDouble(ddindexnavVal[1]).ToString("n2"), ddindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[1]) * 100).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[1]).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[1]) * 100).ToString("n2"));
        //            if (navVal.Count() > 2)
        //                perfDataTable.Rows.Add(arrdate[4].ToString("dd-MMM-yy") + " - " + arrdate[5].ToString("dd-MMM-yy"), navVal[2].ToString("n2"), (10000 + navVal[2] * 100).ToString("n2"), ddindexnavVal[2] == null ? "" : Convert.ToDouble(ddindexnavVal[2]).ToString("n2"), ddindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[2]) * 100).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[2]).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[2]) * 100).ToString("n2"));

        //            //perfDataTable.Rows.Add("Since Inception to " + GetLastQuarterDates().ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
        //            perfDataTable.Rows.Add("Since Inception as on <br/> " + allotDate.ToString("dd-MMM-yy") + " to " + arrdate[1].ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
        //        }



        //        //if (navVal.Count() > 0)
        //        //{

        //        //    //addlindexnavVal[0] = 0; addlindexnavVal[1] = 0; addlindexnavVal[2] = 0;

        //        //    perfDataTable.Rows.Add(dateQtr[1] + " - " + dateQtr[0], navVal[0].ToString("n2"), (10000 + navVal[0] * 100).ToString("n2"), ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
        //        //    if (navVal.Count() > 1)
        //        //        perfDataTable.Rows.Add(dateQtr[2] + " - " + dateQtr[1], navVal[1].ToString("n2"), (10000 + navVal[1] * 100).ToString("n2"), ddindexnavVal[1] == null ? "" : Convert.ToDouble(ddindexnavVal[1]).ToString("n2"), ddindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[1]) * 100).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[1]).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[1]) * 100).ToString("n2"));
        //        //    if (navVal.Count() > 2)
        //        //        perfDataTable.Rows.Add(dateQtr[3] + " - " + dateQtr[2], navVal[2].ToString("n2"), (10000 + navVal[2] * 100).ToString("n2"), ddindexnavVal[2] == null ? "" : Convert.ToDouble(ddindexnavVal[2]).ToString("n2"), ddindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[2]) * 100).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[2]).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[2]) * 100).ToString("n2"));

        //        //    perfDataTable.Rows.Add("Since Inception to " + GetLastQuarterDates().ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
        //        //    //perfDataTable.Rows.Add("Since Inception as on " + allotDate.ToString("dd-MMM-yy") + " to " + GetLastQuarterDates().ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
        //        //}









        //        //LsListViewtbl.DataSource = perfDataTable;
        //        //LsListViewtbl.DataBind(); 
        //        foreach (DataRow drr in perfDataTable.Rows)
        //        {
        //            foreach (DataColumn dcc in perfDataTable.Columns)
        //            {
        //                if (drr[dcc].ToString() == "")
        //                    drr[dcc] = "--";
        //            }
        //        }

        //        if (perfDataTable.Rows.Count > 0)
        //        {

        //        }
        //        //TableNote.Text = "";

        //    }
        //    catch (Exception exc)
        //    {

        //    }


        //    //return perfDataTable;
        //}

        #endregion

        #region: Reset Method

        public void Reset()
        {
            ddlNature.SelectedIndex = 0;
            ddlOption.SelectedIndex = 0;
            FundmanegerText.Text = "";
            txtInvestment.Text = "10000";
            txtStartDate.Text = "";
            lnkbtnDownload.Visible = false;
            FillSchemeByOption();
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

        public string GenerateTable(DataSet dsOldFormatFinal)
        {
            System.Text.StringBuilder sbResultTable = new System.Text.StringBuilder();

            try
            {

                if (dsOldFormatFinal.Tables["tblScheme"].Rows.Count > 0 && dsOldFormatFinal.Tables["tblIndex"].Rows.Count > 0)
                {
                    sbResultTable.AppendLine("<br/>");
                    sbResultTable.AppendLine("<table width='100%'><tr><td colspan='6'></td></tr><tr><td align='left' colspan='6' background='../Images/heading.jpg' height='22' width='500'><span class='style12'>&nbsp;&nbsp;<span class='style4'><img src='../Images/arrow.jpg' /> " + ddlSchemeList.SelectedItem.Text.Trim() + "</span></span></td></tr></table>");
                    //sbResultTable.AppendLine("<br/>");

                    sbResultTable.AppendLine("<center>");
                    // sbResultTable.AppendLine("<table class='style1' cellspacing='0' cellpadding='4' rules='all' border='2' id='gvOldFormatDebt' style='border-width:2px;border-style:solid;border-collapse:collapse;'>");
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

                    Int32 colorcount = 1;

                    #region: Scheme Part

                    if (dsOldFormatFinal.Tables["tblScheme"].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsOldFormatFinal.Tables["tblScheme"].Rows)
                        {
                            if (colorcount % 2 == 1)
                            {
                                sbResultTable.AppendLine("<tr class='row1'>");
                            }
                            else
                            {
                                sbResultTable.AppendLine("<tr class='row2'>");
                            }

                            //sbResultTable.AppendLine("<tr style='background-color:#A0C0DF;'>");
                            sbResultTable.AppendLine("<td class='itemCss' align='left'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["SCHEME_NAME"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["1 m"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["3 m"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["6 m"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["1 Year"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["3 Year"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["5 Year"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td  class='itemCss' align='center'>");
                            //sbResultTable.AppendLine(Convert.ToString(dr["Since Inception"]));// modified for tata treasury
                            sbResultTable.AppendLine(Convert.ToString(dr[8]));
                            sbResultTable.AppendLine("</td>");
                            sbResultTable.AppendLine("</tr>");
                            colorcount++;
                        }
                    }


                    #endregion

                    #region: Index Part

                    if (dsOldFormatFinal.Tables["tblIndex"].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsOldFormatFinal.Tables["tblIndex"].Rows)
                        {

                            if (colorcount % 2 == 1)
                            {
                                sbResultTable.AppendLine("<tr class='row1'>");
                            }
                            else
                            {
                                sbResultTable.AppendLine("<tr class='row2'>");
                            }

                            //sbResultTable.AppendLine("<tr style='background-color:#A0C0DF;'>");
                            sbResultTable.AppendLine("<td class='itemCss' align='left'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["INDEX_NAME"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["1 m"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["3 m"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["6 m"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["1 Year"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["3 Year"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            sbResultTable.AppendLine(Convert.ToString(dr["5 Year"]));
                            sbResultTable.AppendLine("</td>");

                            sbResultTable.AppendLine("<td class='itemCss' align='center'>");
                            //sbResultTable.AppendLine(Convert.ToString(dr["0 Si"])); modified 
                            sbResultTable.AppendLine(Convert.ToString(dr[8]));//last column for inception
                            sbResultTable.AppendLine("</td>");
                            sbResultTable.AppendLine("</tr>");
                            colorcount++;
                        }
                    }


                    #endregion

                    sbResultTable.AppendLine("</table>");
                    sbResultTable.AppendLine("<br/>");
                    sbResultTable.AppendLine("</center>");


                }

                return sbResultTable.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            finally
            {
                sbResultTable = null;
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

        #region: Export to Excel

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
                objFinalstr.Append(LiteralDisclaimer.Text.Substring(1));
            objFinalstr.Append(@"<span style='text-align:justify ;width:500px'><b>Past performance may or may not be sustained in future.</b> Absolute returns is
                                        computed on investment is of Rs 10,000. For computation of since inception returns
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
                                        advice before making any investment. ");
            objFinalstr.Append(@"<br/>The Calculators alone are not sufficient
                                        and shouldn't be used for the development or implementation of an investment strategy.
                                        It should not be construed as investment advice to any party.  Neither Tata Asset
                                        Management Ltd, nor any person connected with it, accepts any liability arising
                                        from the use of this information.  In view of individual nature of tax consequences,
                                        each investor is advised to consult his/ her own professional tax advisor.  <b>Mutual
                                        Fund Investments are subject to market risks, read all Scheme related documents
                                        carefully.</b><br/></span> ");
            //
            //if (ViewState["FinalExcelString"] != null)
            Response.Write(objFinalstr.ToString());
            // Response.Write(Convert.ToString(ViewState["FinalExcelString"]));            
            //Response.Output.Write(sw.ToString() + "<br/>");


            Response.Flush();
            Response.End();


        }


        #endregion

        #endregion





        #region: VerifyRenderingInServerForm
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the
            // specified ASP.NET server control at run time.
            // No code required here.
            return;
        }
        #endregion

    }
}