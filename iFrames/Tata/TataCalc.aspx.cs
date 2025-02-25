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
    public partial class TataCalc : System.Web.UI.Page
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
            SetDateDropdown();
            if (!IsPostBack)
            {
                FillNature();
                // FillScheme();
                FillSchemeByOption();
                // FillFundManager();
                Fillyear();
            }


            // Added to dynamic the ddlSchemeList width 
            ddlSchemeList.Attributes.Add("onfocusout", String.Format("focusOut('{0}',{1})", ddlSchemeList.ClientID, ddlSchemeList.Width.Value));
            ddlSchemeList.Attributes.Add("onmouseenter", String.Format("mouseEnter('{0}',{1})", ddlSchemeList.ClientID, ddlSchemeList.Width.Value));

            ddBenchMark.Attributes.Add("onfocusout", String.Format("focusOut('{0}',{1})", ddBenchMark.ClientID, ddBenchMark.Width.Value));
            ddBenchMark.Attributes.Add("onmouseenter", String.Format("mouseEnter('{0}',{1})", ddBenchMark.ClientID, ddBenchMark.Width.Value));

            //ddlSchemeList.Attributes.Add("onchange", String.Format("focusOut('{0}',{1})", ddlSchemeList.ClientID, ddlSchemeList.Width.Value));
            //ddlSchemeList.Attributes.Add("onmouseout", String.Format("focusOut('{0}',{1})", ddlSchemeList.ClientID, ddlSchemeList.Width.Value));

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
            LiteralFinalReturns.Text = "";
            LiteralDisclaimer.Text = "";
            // settin
            //if (ddlSchemeList.SelectedItem.Value == "8746" || ddlSchemeList.SelectedItem.Value == "8698" || ddlSchemeList.SelectedItem.Value == "8778")
            //trset.Visible = true;
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

            FetchreturnInFormat();


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
                                                     where s.Nav_Check == 3 && s.Option_Id.ToString() == selected_option //s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&                                                    
                                                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                                     on s.Scheme_Id equals tsi.SCHEME_ID
                                                     orderby s.Sch_Short_Name
                                                     select new
                                                     {
                                                         s.Sch_Short_Name,
                                                         s.Scheme_Id
                                                     }).Distinct();
                                if (scheme_name_1.Count() > 0)
                                    //  dt2 = scheme_name_1.ToDataTable();
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
                                    //dt2 = scheme_name_1.ToDataTable();
                                    dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
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
                                                     where s.Nav_Check == 3 && s.Option_Id.ToString() == selected_option //s.T_FUND_MASTER_tata.SUB_NATURE_ID != 2 &&                                                    
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


                                decimal[] schemArray = new decimal[42]{
                                    //4070,4072,8693,8699,8701,8706,8716,8717,8733,8742,8744,8751,8753,8756,8761,8762,8767,8768,8769,8770,8771,8775,8779,8780,8781,8782,8788,8793,8806,8833,8834,8841,8871,8875,9162,9164,11281,11355,13155,13366,15140,15397,15442
                                    4070,8693,8699,8701,8706,8716,8733,8751,8771,8793,8806,8833,8834,8841,8871,9162,9164,11281,11355,13155,13366,15140,15442,8724,8726,9080,8812,9261,8739,8858,8783,8773,8694,13668,13667,13666,8713,13530,8847,8702,9034,8700
                                };

                                List<decimal> lstschem = new List<decimal>();
                            lstschem.AddRange(schemArray);

                       
                           
                                //var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                //                     join T in fundtable
                                //                     on s.Fund_Id equals T.FUND_ID
                                //                     where s.Nav_Check != 2
                                //                     join tsi in scheme.T_SCHEMES_INDEX_tatas
                                //                     on s.Scheme_Id equals tsi.SCHEME_ID
                                //                     orderby s.Sch_Short_Name
                                //                     select new
                                //                     {
                                //                         s.Sch_Short_Name,
                                //                         s.Scheme_Id
                                //                     }).Distinct();


                                     var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_tatas
                                                     join T in fundtable
                                                     on s.Fund_Id equals T.FUND_ID
                                                          where (lstschem.Contains(s.Scheme_Id) && s.Nav_Check == 3) || (s.Nav_Check == 3 && s.Option_Id.ToString() == "2" && s.T_FUND_MASTER_tata.SUB_NATURE_ID == 2)                                                        
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

                            // this section was added to include TMIF Monthly Dividend Individual AND cORPORATE while removing monthly dividend
                            if (dt2.Rows.Count > 0)
                            {
                                ddlSchemeList.Visible = true;
                                DataColumn[] pk = new DataColumn[] { dt2.Columns["Scheme_Id"] };
                                dt2.PrimaryKey = pk;
                      
                                //dt2.Rows.Remove(dt2.Rows.Find("8698"));
                                //DataColumn[] pkk = new DataColumn[] { dt2.Columns["Sch_Short_Name"] };
                                //dt2.PrimaryKey = pkk;
                            
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
            string selected_Scheme = ddlSchemeList.SelectedValue.ToString();

            if (selected_Scheme == "9999999991")
                selected_Scheme = "8698";
            else if (selected_Scheme == "9999999992")
                selected_Scheme = "8698";

            try
            {
                using (var FundmanagerData = new TataCalculatorDataContext())
                {

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
                    //                  });
                    //                  //}).Take(1);



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
                    //}).Take(1);


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
            //ddlQtrEnd.Items.Add(new ListItem("31st January", "01-31-"));
            //if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
            //    ddlQtrEnd.Items.Add(new ListItem("29th February", "02-29-"));
            //else
            //    ddlQtrEnd.Items.Add(new ListItem("28th February", "02-28-"));
            ddlQtrEnd.Items.Add(new ListItem("31st March", "03-31-"));
            //ddlQtrEnd.Items.Add(new ListItem("30th April", "04-30-"));
            //ddlQtrEnd.Items.Add(new ListItem("31st May", "05-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th June", "06-30-"));
            //ddlQtrEnd.Items.Add(new ListItem("31st July", "07-31-"));
            //ddlQtrEnd.Items.Add(new ListItem("31st August", "08-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th September", "09-30-"));
            //ddlQtrEnd.Items.Add(new ListItem("31st October", "10-31-"));
            //ddlQtrEnd.Items.Add(new ListItem("30th November", "11-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st December", "12-31-"));


            if (month != 1)
            {
                //foreach (ListItem li in ddlQtrEnd.Items)
                //{
                    
                //    if (Convert.ToInt32(li.Value.Split('-')[0]) == month - 1)
                //        li.Selected = true;
                //}
                if(month>9)
                    ddlQtrEnd.Items[2].Selected = true;
                if (month > 6 && month<10)
                    ddlQtrEnd.Items[1].Selected = true;
                if (month > 3 && month < 7)
                    ddlQtrEnd.Items[0].Selected = true;
                if (month > 1 && month < 4)
                    ddlQtrEnd.Items[3].Selected = true;
            }

            if (month == 1)
            {
                ddlYearEnd.Items[ddlYearEnd.SelectedIndex].Selected = false;
                //ddlYearEnd.Items[ddlYearEnd.SelectedIndex - 1].Selected = true;
                ddlQtrEnd.Items[3].Selected = true;//              
                ddlYearEnd.Items.FindByValue((year - 1).ToString()).Selected = true;
            }


        }

        #endregion

        #region: Fetch Methods

        public void FetchreturnInFormat()
        {
            System.Text.StringBuilder sbFinal = new System.Text.StringBuilder();
            Decimal dcmlIndexid;
            bool isEquity = true;
            string IndexBenchmarlIds = null;

            try
            {
                if (ddlSchemeList.Items.Count > 0)
                {
                    DataTable dtFinal = new DataTable();
                    DataTable dtScheme = new DataTable();
                    DataTable dtTempOld = new DataTable();
                    DataTable dtTempNew = new DataTable();
                    DataTable dtIndex = new DataTable();
                    DataTable dtSchemeNewFrmt = new DataTable();
                    DataTable dtIndexNewFrmt = new DataTable();
                    DataSet dsOldFormatFinal = new DataSet();

                    bool isCompositeIndex = false;
                    bool modifiedInceptionDate = false;
                    bool showSpecificDisclaimer = false;

                    // DateTime _todate1 = Convert.ToDateTime(txtStartDate.Text);
                    //DateTime _todate = Convert.ToDateTime(ddlQtrEnd.SelectedValue + ddlYearEnd.SelectedValue);
                    DateTime _todate = new DateTime(Convert.ToInt16(ddlYearEnd.SelectedValue.Split('-')[0]), Convert.ToInt16(ddlQtrEnd.SelectedValue.Split('-')[0]), Convert.ToInt16(ddlQtrEnd.SelectedValue.Split('-')[1]));


                    //DateTime _todate = new DateTime(Convert.ToInt16(txtStartDate.Text.Split('/')[2]),
                    //                 Convert.ToInt16(txtStartDate.Text.Split('/')[1]),
                    //                 Convert.ToInt16(txtStartDate.Text.Split('/')[0]));

                    _asOnDate = _todate;
                    string val_SchemeIDs = string.Empty;
                    List<decimal> lstschmId = new List<decimal>();
                    lstschmId.Clear();
                    //for (int i = 0; i < ddlSchemeList.Items.Count; i++)
                    //{
                    //    CheckBox cb = ddlSchemeList.Items[i].FindControl("chkItem") as CheckBox;
                    //    if (cb.Checked == true)
                    //    {
                    //        val_SchemeIDs += ((Label)ddlSchemeList.Items[i].Cells[0].FindControl("lblSchemeID")).Text + ",";
                    //        lstschmId.Add(Convert.ToDecimal(((Label)ddlSchemeList.Items[i].Cells[0].FindControl("lblSchemeID")).Text));
                    //    }
                    //}


                    if (ddlSchemeList.SelectedValue != "--")
                    {
                        val_SchemeIDs = ddlSchemeList.SelectedValue;
                        lstschmId.Add(Convert.ToDecimal(ddlSchemeList.SelectedValue));
                    }
                    else
                    {
                        return;
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



                    string val_SchemeID = ddlSchemeList.SelectedValue;

                    string strSettingset = string.Empty;
                    int settingSet = 2;
                    int settingSetAbs = 8;

                    //strSettingset = radioList.SelectedItem.Text;
                    //if (strSettingset.ToUpper() == "CORPORATE")
                    //{
                    //    settingSet = 26;
                    //}
                    //else
                    //{
                    //    settingSet = 2;
                    //}



                    //if (ddlSchemeList.SelectedItem.Text.ToUpper() == "TATA MONTHLY INCOME FUND (TMIF) - OTHER THAN INDIVIDUAL & HUF - MONTHLY INCOME OPTION")
                    //{
                    //    settingSet = 26;
                    //}
                    //else if (ddlSchemeList.SelectedItem.Text.ToUpper() == "TATA MONTHLY INCOME FUND (TMIF) - INDIVIDUAL & HUF - MONTHLY INCOME OPTION")
                    //{
                    //    settingSet = 2;
                    //}

                    if (ddlSchemeList.SelectedValue == "9999999991")
                    {
                        settingSet = 2;
                        val_SchemeID = "8698";
                    }
                    else if (ddlSchemeList.SelectedValue == "9999999992")
                    {
                        settingSet = 26;
                        settingSetAbs = 27;
                        val_SchemeID = "8698";
                    }


                    string strRollingPeriodin = string.Empty;
                    strRollingPeriodin = "1 m,3 m,6 m,1 YYYY,3 YYYY,5 YYYY,0 Si";
                    string strRollingPeriodinNewFormat = string.Empty;
                    //strRollingPeriodinNewFormat = "1 YYYY,2 YYYY,3 YYYY,0 Si";
                    strRollingPeriodinNewFormat = "0 Si";

                    DateTime LastQuarterDate;
                    //LastQuarterDate = GetLastQuarterDates(_todate);//_todate is already qtrend
                    LastQuarterDate = _todate;

                    DateTime allotDate;

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
                            lblMessage.Text = "Launch date is not available";
                            lblMessage.Visible = false;
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



                        //// Specal case of Tata Monthly Income Fund - Monthly Dividend  SI need to show -30 April 2009
                        //if (val_SchemeID == "8746")
                        //    allotDate = new DateTime(2000, 4, 27);


                        string _indexName = string.Empty;
                        string _indexId = string.Empty;
                        
                     

                        _indexName = ddBenchMark.SelectedItem.Text;
                        _indexId = ddBenchMark.SelectedItem.Value;




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


                        bool isAdditionalBenchMark = false;
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
                                    if (val_SchemeID != "9079" && val_SchemeID != "9080" && val_SchemeID != "17802" && val_SchemeID != "17803" && val_SchemeID != "9161" && val_SchemeID != "9162" && val_SchemeID != "9163" && val_SchemeID != "9164" && val_SchemeID != "17812" && val_SchemeID != "17813" && val_SchemeID != "17814" && val_SchemeID != "17815")
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

                        //if (val_SchemeID == "13530" || val_SchemeID == "13531")
                        //{
                        //    isAdditionalBenchMark = true;
                        //    _indexId = "28,13";
                        //    IndexBenchmarlIds = _indexId;
                        //}

                        // code on 02.07.2013 
                        if (val_SchemeID == "15140" || val_SchemeID == "15442")
                        {
                            isAdditionalBenchMark = true;
                            _indexId = "25,134";
                            IndexBenchmarlIds = _indexId;
                        }

                        /// changes for Isec - composite
                        /// 
                        string strIndexBenchmarlIds = string.Empty;
                        strIndexBenchmarlIds = IndexBenchmarlIds;


                        if (_indexId.Trim() == "32")
                        {
                            //strIndexBenchmarlIds = strIndexBenchmarlIds.Replace("32", "1T");
                            strIndexBenchmarlIds = IndexBenchmarlIds.Replace("32", "1T");
                        }
                        #region Scheme Return New Format

                        DataTable _tempdataTable1 = new DataTable();
                        DataTable _tempdataTable2 = new DataTable();


                        //// Specal case of Tata Treasury Manager Fund - SHIP - Growth SI need to show -30 April 2009
                        // if (val_SchemeID == "9042" || val_SchemeID == "8746")
                        //if (val_SchemeID == "9042")
                        if (modifiedInceptionDate)
                        {
                            string daydiff = string.Empty;
                            TimeSpan noofday = LastQuarterDate.Subtract(allotDate);
                            daydiff = noofday.Days.ToString() + " d";
                            strRollingPeriodinNewFormat = strRollingPeriodinNewFormat.Replace("0 Si", daydiff);

                        }
                        string strdaydiff = string.Empty;
                        string strdaydiffColumn = string.Empty;
                        TimeSpan _noofday = LastQuarterDate.Subtract(allotDate);
                        strdaydiff = _noofday.Days.ToString() + " D";
                        strdaydiffColumn = strdaydiff + "ay";

                        SqlCommand oldcmdd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_TATA", cn);
                        oldcmdd.CommandType = CommandType.StoredProcedure;
                        oldcmdd.CommandTimeout = 2000;
                        oldcmdd.Parameters.Add(new SqlParameter("@SchemeIDs", val_SchemeID));
                        oldcmdd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                        oldcmdd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                        oldcmdd.Parameters.Add(new SqlParameter("@DateTo", LastQuarterDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                        oldcmdd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                        oldcmdd.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodinNewFormat));
                        oldcmdd.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
                        oldcmdd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                        oldcmdd.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
                        oldcmdd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                        oldcmdd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


                        SqlCommand cmdd = new SqlCommand("MFIE_SP_FIXPERIODICROLLINGRETURNS_TATA", cn);
                        cmdd.CommandType = CommandType.StoredProcedure;
                        cmdd.CommandTimeout = 2000;
                        cmdd.Parameters.Add(new SqlParameter("@SchemeIDs", val_SchemeID));
                        //cmdd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet)); 
                        cmdd.Parameters.Add(new SqlParameter("@SettingSetID", settingSetAbs)); 
                        cmdd.Parameters.Add(new SqlParameter("@DateFrom", LastQuarterDate.AddYears(-2).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                        cmdd.Parameters.Add(new SqlParameter("@DateTo", LastQuarterDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                        cmdd.Parameters.Add(new SqlParameter("@RollingPeriod", ""));
                        cmdd.Parameters.Add(new SqlParameter("@FixPeriodic", "YoY"));
                        cmdd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                        cmdd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



                        SqlCommand oldindexCmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_TATA", cn);
                        // SqlCommand indexCmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", cn);

                        oldindexCmd.CommandType = CommandType.StoredProcedure;
                        oldindexCmd.CommandTimeout = 2000;
                        oldindexCmd.Parameters.Add(new SqlParameter("@IndexIDs", strIndexBenchmarlIds));
                        oldindexCmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                        oldindexCmd.Parameters.Add(new SqlParameter("@DateFrom", allotDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                        oldindexCmd.Parameters.Add(new SqlParameter("@DateTo", LastQuarterDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                        oldindexCmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                        //oldindexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodinNewFormat));
                        oldindexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strdaydiff));
                        oldindexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
                        oldindexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                        oldindexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
                        oldindexCmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                        oldindexCmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

                        SqlDataAdapter sqlda = new SqlDataAdapter();


                        //Modified for composite index
                        if (strIndexBenchmarlIds.Contains("T"))
                        {
                            DataTable dttempindex = new DataTable();
                            for (int i = 0; i < 3; i++)
                            {
                                dttempindex.Reset();
                                SqlCommand indexCmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_TATA", cn);
                                indexCmd.CommandType = CommandType.StoredProcedure;
                                indexCmd.CommandTimeout = 2000;
                                indexCmd.Parameters.Add(new SqlParameter("@IndexIDs", strIndexBenchmarlIds));
//                                indexCmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                                indexCmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSetAbs));
                                indexCmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                                indexCmd.Parameters.Add(new SqlParameter("@DateTo", LastQuarterDate.AddYears(-i).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                                indexCmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", "1 YYYY"));
                                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
                                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
                                indexCmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));                               
                                indexCmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));
                                indexCmd.Parameters.Add(new SqlParameter("@SCHEME_ID", val_SchemeID));

                                sqlda.SelectCommand = indexCmd;
                                sqlda.Fill(dttempindex);
                                if (i == 0)
                                    dtIndexNewFrmt = dttempindex.Clone();
                                foreach (DataRow dr in dttempindex.Rows)
                                {
                                    dtIndexNewFrmt.ImportRow(dr);
                                }
                            }

                            //dtIndexNewFrmt.Columns.Add("id");
                            //dtIndexNewFrmt.Columns[dtIndexNewFrmt.Columns.Count - 1].SetOrdinal(0);
                        }
                        else
                        {
                            SqlCommand indexCmd = new SqlCommand("MFIE_SP_FIXEDPERIODICINDEXROLLINGRETURN_TATA", cn);
                            indexCmd.CommandType = CommandType.StoredProcedure;
                            indexCmd.CommandTimeout = 2000;
                            indexCmd.Parameters.Add(new SqlParameter("@IndexIDs", strIndexBenchmarlIds));
                            //indexCmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSet));
                            indexCmd.Parameters.Add(new SqlParameter("@SettingSetID", settingSetAbs));
                            indexCmd.Parameters.Add(new SqlParameter("@DateFrom", LastQuarterDate.AddYears(-2).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                            indexCmd.Parameters.Add(new SqlParameter("@DateTo", LastQuarterDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                            indexCmd.Parameters.Add(new SqlParameter("@RollingPeriod", ""));
                            indexCmd.Parameters.Add(new SqlParameter("@FixPeriodic", "YoY"));
                            indexCmd.Parameters.Add(new SqlParameter("@RoundTill", 2));                            
                            indexCmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));
                            indexCmd.Parameters.Add(new SqlParameter("@SCHEME_ID", val_SchemeID));

                            sqlda.SelectCommand = indexCmd;
                            dtIndexNewFrmt.Reset();
                            sqlda.Fill(dtIndexNewFrmt);
                        }

                        /// for inception and schem index Name
                        _tempdataTable1.Reset();
                        _tempdataTable2.Reset();
                        sqlda.SelectCommand = oldcmdd;
                        sqlda.Fill(_tempdataTable1);
                        sqlda.SelectCommand = oldindexCmd;
                        sqlda.Fill(_tempdataTable2);
                        sqlda.SelectCommand = cmdd;

                        dtSchemeNewFrmt.Reset();
                        sqlda.Fill(dtSchemeNewFrmt);
                        //dsOldFormatFinal.Tables.Add(dtSchemeNewFrmt);                        
                        IndexBenchmarlIds = strIndexBenchmarlIds.Replace("T", "");


                        //dsOldFormatFinal.Tables.Add(dtSchemeNewFrmt);
                        if (dtIndexNewFrmt.Columns.Contains("INDEX_TYPE"))
                            dtIndexNewFrmt.Columns.Remove("INDEX_TYPE");

                        if (_tempdataTable2.Columns.Contains("INDEX_TYPE"))
                            _tempdataTable2.Columns.Remove("INDEX_TYPE");



                        IEnumerable<DataRow> _testdtIndex = from ib in IndexBenchmarlIds.Split(',').AsEnumerable()
                                                            join tst in dtIndexNewFrmt.AsEnumerable()
                                                            on ib equals tst.Field<decimal>("Index_id").ToString()
                                                            select tst;
                        DataTable _dttt = new DataTable("tblIndex");
                        _dttt = _testdtIndex.CopyToDataTable<DataRow>();
                        dtIndexNewFrmt.Clear();
                        dtIndexNewFrmt = _dttt;




                        IEnumerable<DataRow> _testdtIndex2 = from ib in IndexBenchmarlIds.Split(',').AsEnumerable()
                                                             join tst in _tempdataTable2.AsEnumerable()
                                                            on ib equals tst.Field<decimal>("Index_id").ToString()
                                                             select tst;
                        DataTable _dttt2 = new DataTable();
                        _dttt2 = _testdtIndex2.CopyToDataTable<DataRow>();
                        _tempdataTable2.Clear();
                        _tempdataTable2 = _dttt2;




                        DataRow[] datarowsNewfrmt;

                        if (isAdditionalBenchMark)
                        {

                            if (IndexBenchmarlIds.Split(',').Count() == 3 || isCompositeIndex == true)//isCompositeIndex==  
                            {
                                datarowsNewfrmt = new DataRow[3] { _tempdataTable1.NewRow(), _tempdataTable1.NewRow(), _tempdataTable1.NewRow() };
                                datarowsNewfrmt[0] = _tempdataTable2.Rows[0]; datarowsNewfrmt[1] = _tempdataTable2.Rows[1]; datarowsNewfrmt[2] = _tempdataTable2.Rows[2];
                                _tempdataTable1.Rows.Add(datarowsNewfrmt[0].ItemArray);
                                _tempdataTable1.Rows.Add(datarowsNewfrmt[1].ItemArray);
                                _tempdataTable1.Rows.Add(datarowsNewfrmt[2].ItemArray);
                            }
                            else
                            {
                                datarowsNewfrmt = new DataRow[2] { _tempdataTable1.NewRow(), _tempdataTable1.NewRow() };
                                datarowsNewfrmt[0] = _tempdataTable2.Rows[0];
                                datarowsNewfrmt[1] = _tempdataTable2.Rows[1];
                                _tempdataTable1.Rows.Add(datarowsNewfrmt[0].ItemArray);
                                _tempdataTable1.Rows.Add(datarowsNewfrmt[1].ItemArray);
                            }
                        }
                        else
                        {
                            if (IndexBenchmarlIds.Split(',').Count() == 3 || isCompositeIndex == true)
                            {
                                datarowsNewfrmt = new DataRow[2] { _tempdataTable1.NewRow(), _tempdataTable1.NewRow() };
                                datarowsNewfrmt[0] = _tempdataTable2.Rows[0];
                                datarowsNewfrmt[1] = _tempdataTable2.Rows[1];
                                _tempdataTable1.Rows.Add(datarowsNewfrmt[0].ItemArray);
                                _tempdataTable1.Rows.Add(datarowsNewfrmt[1].ItemArray);
                            }
                            else
                            {
                                datarowsNewfrmt = new DataRow[1] { _tempdataTable1.NewRow() };
                                datarowsNewfrmt[0] = _tempdataTable2.Rows[0];
                                _tempdataTable1.Rows.Add(datarowsNewfrmt[0].ItemArray);
                            }
                        }

                        DataTable newFrmtDataTable = new DataTable();
                        newFrmtDataTable.Columns.Add("SchemeIndex", typeof(string));
                        newFrmtDataTable.Columns.Add("FirstyearRs", typeof(string));
                        newFrmtDataTable.Columns.Add("FirstyearReturn", typeof(string));
                        newFrmtDataTable.Columns.Add("SecondyearRs", typeof(string));
                        newFrmtDataTable.Columns.Add("SecondyearReturn", typeof(string));
                        newFrmtDataTable.Columns.Add("ThirdyearRs", typeof(string));
                        newFrmtDataTable.Columns.Add("ThirdyearReturn", typeof(string));
                        newFrmtDataTable.Columns.Add("CagrInceptionRs", typeof(string));
                        newFrmtDataTable.Columns.Add("CagrInceptionReturn", typeof(string));
                        newFrmtDataTable.Columns.Add("InceptionDate", typeof(string));

                        double? sinceInceptionSch = null, sinceInceptionIndex = null, sinceInceptionAddlIndex = null;
                        Double? CompundReturnSI = null, CompundReturnSIndex = null, CompundReturnSIAddIndex = null;

                        string inceptionColumnName = string.Empty;

                        if (strRollingPeriodinNewFormat.ToLower().Contains("d"))
                            inceptionColumnName = strdaydiffColumn;
                        else if (strRollingPeriodinNewFormat.ToLower().Contains("si"))
                            inceptionColumnName = "Since Inception";


                        if (_tempdataTable1.Rows.Count > 1)
                        {
                            // _tempdataTable1.Columns.RemoveAt(0);//scheme or index id
                            // Calculate Inceptio index

                            if (_tempdataTable1.Rows[0][inceptionColumnName].ToString().Trim() != "N/A")
                                sinceInceptionSch = Convert.ToDouble(_tempdataTable1.Rows[0][inceptionColumnName].ToString());

                            if (IndexBenchmarlIds.Split(',').Count() == 3 || isCompositeIndex == true)
                            {
                                if (_tempdataTable1.Rows[1][inceptionColumnName].ToString().Trim() != "N/A" && _tempdataTable1.Rows[2][inceptionColumnName].ToString().Trim() != "N/A")
                                    sinceInceptionIndex = Convert.ToDouble(CompositeReturn(Convert.ToInt32(_tempdataTable1.Rows[0][0]), Convert.ToDouble(_tempdataTable1.Rows[1][inceptionColumnName]), Convert.ToDouble(_tempdataTable1.Rows[2][inceptionColumnName])));
                                if (isAdditionalBenchMark)
                                {
                                    if (_tempdataTable1.Rows[3][inceptionColumnName].ToString().Trim() != "N/A")
                                        sinceInceptionAddlIndex = Convert.ToDouble(_tempdataTable1.Rows[3][inceptionColumnName].ToString());
                                }
                            }
                            else
                            {
                                if (_tempdataTable1.Rows[1][inceptionColumnName].ToString().Trim() != "N/A")
                                    sinceInceptionIndex = Convert.ToDouble(_tempdataTable1.Rows[1][inceptionColumnName].ToString());
                                if (isAdditionalBenchMark)
                                {
                                    if (_tempdataTable1.Rows[2][inceptionColumnName].ToString().Trim() != "N/A")
                                        sinceInceptionAddlIndex = Convert.ToDouble(_tempdataTable1.Rows[2][inceptionColumnName].ToString());
                                }
                            }
                        }


                        TimeSpan days = LastQuarterDate.Subtract(allotDate);
                        int day = days.Days;

                        int amount;
                        if (Int32.TryParse(txtInvestment.Text, out amount))
                        {
                            if (sinceInceptionSch != null)
                                CompundReturnSI = amount * Math.Pow(1 + (double)sinceInceptionSch / 100, Math.Round((double)day / 365, 4));
                            if (sinceInceptionIndex != null)
                                CompundReturnSIndex = amount * Math.Pow(1 + (double)sinceInceptionIndex / 100, Math.Round((double)day / 365, 4));
                            if (isAdditionalBenchMark)
                            {
                                if (sinceInceptionAddlIndex != null)
                                    CompundReturnSIAddIndex = amount * Math.Pow(1 + (double)sinceInceptionAddlIndex / 100, Math.Round((double)day / 365, 4));
                            }
                        }
                        else
                        {
                            if (sinceInceptionSch != null)
                                CompundReturnSI = 10000 * Math.Pow(1 + (double)sinceInceptionSch / 100, Math.Round((double)day / 365, 4));
                            if (sinceInceptionIndex != null)
                                CompundReturnSIndex = 10000 * Math.Pow(1 + (double)sinceInceptionIndex / 100, Math.Round((double)day / 365, 4));
                            if (isAdditionalBenchMark)
                            {
                                if (sinceInceptionAddlIndex != null)
                                    CompundReturnSIAddIndex = 10000 * Math.Pow(1 + (double)sinceInceptionAddlIndex / 100, Math.Round((double)day / 365, 4));
                            }
                        }
                        // amount= Convert.ToInt32(txtInvestment.Text);



                        List<object> indexnavs = new List<object>();
                        indexnavs.Clear();

                        DataTable dtperiod = new DataTable();
                        dtperiod.Columns.Add(new DataColumn("Period", typeof(String)));
                        dtperiod.Columns.Add(new DataColumn("Val", typeof(String)));
                        for (Int32 i = 0; i <= 2; i++)
                        {
                            DataRow row = dtperiod.NewRow();
                            row["Period"] = LastQuarterDate.AddYears(-i).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            row["Val"] = "N/A";
                            dtperiod.Rows.Add(row);
                        }

                        //var _indexnavs = (from k in dtIndexNewFrmt.AsEnumerable()
                        //                  where k.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                        //                  select new
                        //                      {
                        //                          NAV = (k != null) ? k.Field<object>("index_value") : "N/A"
                        //                      }).Take(3).ToArray();

                        //var _indexnavs = (from k in dtIndexNewFrmt.AsEnumerable()
                        //                where k.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                        //               select k.Field<object>("index_value") != null ? k.Field<object>("index_value") : "N/A").Take(3).ToArray();

                        string fetchColumn = string.Empty;
                        if (IndexBenchmarlIds.Split(',').Count() == 3 || isCompositeIndex == true || strIndexBenchmarlIds.Contains('T'))
                            fetchColumn = "1 year";
                        else
                            fetchColumn = "index_value";

                        if (IndexBenchmarlIds.Split(',').Count() == 3 || isCompositeIndex == true)
                        {
                            var _indexnavs1 = (from k in dtIndexNewFrmt.AsEnumerable()
                                               where k.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                                               select k.Field<object>(fetchColumn) != null ? k.Field<object>(fetchColumn) : "N/A").Take(3).ToArray();

                            var _indexnavs2 = (from k in dtIndexNewFrmt.AsEnumerable()
                                               where k.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[1])
                                               select k.Field<object>(fetchColumn) != null ? k.Field<object>(fetchColumn) : "N/A").Take(3).ToArray();

                            if (_indexnavs1.Count() == 3 && _indexnavs2.Count() == 3)
                            {
                                for (int k = 0; k <= 2; k++)
                                {
                                    if (_indexnavs1[k].ToString().Trim() != "N/A" && _indexnavs2[k].ToString().Trim() != "N/A")
                                        indexnavs.Add(CompositeReturn(Convert.ToInt32(_tempdataTable1.Rows[0][0]), Convert.ToDouble(_indexnavs1[k]), Convert.ToDouble(_indexnavs2[k])));
                                    else
                                    {
                                        indexnavs.Add("N/A");
                                    }
                                }
                                if (indexnavs.Count == 0)
                                {
                                    indexnavs.Add("N/A"); indexnavs.Add("N/A"); indexnavs.Add("N/A");
                                }
                            }
                            else
                            {
                                indexnavs.Add("N/A"); indexnavs.Add("N/A"); indexnavs.Add("N/A");
                            }


                        }
                        else
                        {

                            var _indexnavs = (from k in dtIndexNewFrmt.AsEnumerable()
                                              where k.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0])
                                              select k.Field<object>(fetchColumn) != null ? k.Field<object>(fetchColumn) : "N/A").Take(3).ToArray();

                            if (_indexnavs.Count() != 3)
                            {
                                _indexnavs = (from sc in dtperiod.AsEnumerable()
                                              join k in dtIndexNewFrmt.AsEnumerable() on sc.Field<string>("Period").Split('/')[2] equals k.Field<string>("Record_Date").Split('/')[2]
                                                 into temp
                                                 from tmp in temp.DefaultIfEmpty()
                                              where ((tmp != null) ? tmp.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[0]) : 1 == 1)
                                                 select ((tmp != null) ? tmp.Field<object>("Index_value") : sc.Field<object>("Val"))
                                                     ).Take(3).ToArray();
                            }

                            if (_indexnavs.Count() == 3)
                                indexnavs.AddRange(_indexnavs);
                            else
                            {
                                indexnavs.Add("N/A"); indexnavs.Add("N/A"); indexnavs.Add("N/A");
                            }

                        }




                        List<object> addindexnavs = new List<object>();

                        if (isAdditionalBenchMark)
                        {
                            //var _addindexnavs = (from k in dtIndexNewFrmt.AsEnumerable()
                            //                 where k.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[1])
                            //                 select new
                            //                 {
                            //                     NAV = (k != null) ? k.Field<object>("index_value") : "N/A"
                            //                 }).Take(3).ToArray();

                            int intcode;

                            if (IndexBenchmarlIds.Split(',').Count() == 3 || isCompositeIndex == true)
                            {
                                intcode = 2;
                            }
                            else
                                intcode = 1;

                           

                            var _addindexnavs = (from k in dtIndexNewFrmt.AsEnumerable()
                                                 where k.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[intcode])
                                                 select k.Field<object>(fetchColumn) != null ? k.Field<object>(fetchColumn) : "N/A").Take(3).ToArray();


                            //addindexnavs.AddRange(_addindexnavs);
                            if (_addindexnavs.Count() != 3)
                            {
                                _addindexnavs = (from sc in dtperiod.AsEnumerable()
                                                 join k in dtIndexNewFrmt.AsEnumerable() on sc.Field<string>("Period").Split('/')[2] equals k.Field<string>("Record_Date").Split('/')[2]
                                                     into temp
                                                     from tmp in temp.DefaultIfEmpty()
                                                     where ((tmp != null) ? tmp.Field<System.Decimal>("index_id") == Convert.ToDecimal(IndexBenchmarlIds.Split(',')[intcode]) : 1==1)
                                                     select ((tmp != null) ? tmp.Field<object>("Index_value") : sc.Field<object>("Val"))
                                                     ).Take(3).ToArray(); 
                            }
                            if (_addindexnavs.Count() == 3)
                                addindexnavs.AddRange(_addindexnavs);
                            else
                            {
                                addindexnavs.Add("N/A"); addindexnavs.Add("N/A"); addindexnavs.Add("N/A");
                            }

                        }



                        var listschemeval = (from sc in dtperiod.AsEnumerable()
                                             join sch in dtSchemeNewFrmt.AsEnumerable() on sc.Field<string>("Period").Split('/')[2] equals sch.Field<string>("nav_date").Split('/')[2]  
                                             into temp
                                             from tmp in temp.DefaultIfEmpty()
                                             select ((tmp != null) ? tmp.Field<object>("Nav") : sc.Field<object>("Val"))
                                             ).Take(3).ToList();



                        //var newrow=new schemeval{
                        //  //  "Nav"=
                        //}
                        // modified on 23 august 2012
                        if (listschemeval.Count() != 3)
                        {

                            if (allotDate.Subtract(LastQuarterDate.AddYears(-1)).Days >= 0)
                                listschemeval.Insert(0, "N/A");
                            if (allotDate.Subtract(LastQuarterDate.AddYears(-2)).Days >= 0)
                                listschemeval.Insert(1, "N/A");
                            if (allotDate.Subtract(LastQuarterDate.AddYears(-3)).Days >= 0)
                                listschemeval.Insert(2, "N/A");



                            if (listschemeval.Count() == 0)
                            {
                                listschemeval.Insert(0, "N/A"); listschemeval.Insert(1, "N/A"); listschemeval.Insert(2, "N/A");
                            }
                            else if (listschemeval.Count() == 1)
                            {
                                listschemeval.Insert(1, "N/A"); listschemeval.Insert(2, "N/A");
                            }
                            else if (listschemeval.Count() == 2)
                            {
                                listschemeval.Insert(2, "N/A");
                            }

                        }
                        newFrmtDataTable.Clear();
                        string SchemeName = string.Empty;
                        SchemeName = _tempdataTable1.Rows[0][1].ToString();
                        if (settingSet == 26 && val_SchemeID == "8698")
                        {
                            SchemeName = "Tata Monthly Income Fund (TMIF) - Other than Individual & HUF - Monthly Income Option";
                        }
                        else if (settingSet == 2 && val_SchemeID == "8698")
                        {
                            SchemeName = "Tata Monthly Income Fund (TMIF) - Individual & HUF - Monthly Income Option";
                        }

                        if (dtSchemeNewFrmt != null)
                        {
                            if (dtSchemeNewFrmt.Columns.Count > 2)
                            {


                                if (Int32.TryParse(txtInvestment.Text, out amount))
                                {
                                    newFrmtDataTable.Rows.Add(SchemeName, listschemeval[0].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listschemeval[0]) * amount / 100).ToString("n2"), listschemeval[0].ToString(), listschemeval[1].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listschemeval[1]) * amount / 100).ToString("n2"), listschemeval[1].ToString(), listschemeval[2].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(listschemeval[2]) * amount / 100).ToString("n2"), listschemeval[2].ToString(), CompundReturnSI == null ? "N/A" : Convert.ToDecimal(CompundReturnSI).ToString("n2"), _tempdataTable1.Rows[0][inceptionColumnName].ToString(), allotDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));

                                    if (IndexBenchmarlIds.Split(',').Count() == 3 || isCompositeIndex == true)
                                    {
                                        newFrmtDataTable.Rows.Add(_tempdataTable1.Rows[1][1].ToString() + " & " + _tempdataTable1.Rows[2][1].ToString(), indexnavs[0].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(indexnavs[0]) * amount / 100).ToString("n2"), indexnavs[0].ToString(), indexnavs[1].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(indexnavs[1]) * amount / 100).ToString("n2"), indexnavs[1].ToString(), indexnavs[2].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(indexnavs[2]) * amount / 100).ToString("n2"), indexnavs[2].ToString(), CompundReturnSIndex == null ? "N/A" : Convert.ToDecimal(CompundReturnSIndex).ToString("n2"), sinceInceptionIndex == null ? "N/A" : Convert.ToDecimal(sinceInceptionIndex).ToString("n2"), "");
                                        if (isAdditionalBenchMark)
                                            newFrmtDataTable.Rows.Add(_tempdataTable1.Rows[3][1].ToString(), addindexnavs[0].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(addindexnavs[0]) * amount / 100).ToString("n2"), addindexnavs[0].ToString(), addindexnavs[1].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(addindexnavs[1]) * amount / 100).ToString("n2"), addindexnavs[1].ToString(), addindexnavs[2].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(addindexnavs[2]) * amount / 100).ToString("n2"), addindexnavs[2].ToString(), CompundReturnSIAddIndex == null ? "N/A" : Convert.ToDecimal(CompundReturnSIAddIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "N/A" : Convert.ToDecimal(sinceInceptionAddlIndex).ToString("n2"), "");

                                    }
                                    else
                                    {
                                        newFrmtDataTable.Rows.Add(_tempdataTable1.Rows[1][1].ToString(), indexnavs[0].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(indexnavs[0]) * amount / 100).ToString("n2"), indexnavs[0].ToString(), indexnavs[1].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(indexnavs[1]) * amount / 100).ToString("n2"), indexnavs[1].ToString(), indexnavs[2].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(indexnavs[2]) * amount / 100).ToString("n2"), indexnavs[2].ToString(), CompundReturnSIndex == null ? "N/A" : Convert.ToDecimal(CompundReturnSIndex).ToString("n2"), sinceInceptionIndex == null ? "N/A" : Convert.ToDecimal(sinceInceptionIndex).ToString("n2"), "");
                                        if (isAdditionalBenchMark)
                                            newFrmtDataTable.Rows.Add(_tempdataTable1.Rows[2][1].ToString(), addindexnavs[0].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(addindexnavs[0]) * amount / 100).ToString("n2"), addindexnavs[0].ToString(), addindexnavs[1].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(addindexnavs[1]) * amount / 100).ToString("n2"), addindexnavs[1].ToString(), addindexnavs[2].ToString() == "N/A" ? "N/A" : (amount + Convert.ToDecimal(addindexnavs[2]) * amount / 100).ToString("n2"), addindexnavs[2].ToString(), CompundReturnSIAddIndex == null ? "N/A" : Convert.ToDecimal(CompundReturnSIAddIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "N/A" : Convert.ToDecimal(sinceInceptionAddlIndex).ToString(), "");
                                    }
                                }
                                else
                                {
                                    newFrmtDataTable.Rows.Add(_tempdataTable1.Rows[0][1].ToString(), listschemeval[0].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(listschemeval[0]) * 100).ToString("n2"), listschemeval[0].ToString(), listschemeval[1].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(listschemeval[1]) * 100).ToString("n2"), listschemeval[1].ToString(), listschemeval[2].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(listschemeval[2]) * 100).ToString("n2"), listschemeval[2].ToString(), CompundReturnSI == null ? "N/A" : Convert.ToDecimal(CompundReturnSI).ToString("n2"), _tempdataTable1.Rows[0][inceptionColumnName].ToString(), allotDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                                    newFrmtDataTable.Rows.Add(_tempdataTable1.Rows[1][1].ToString(), indexnavs[0].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(indexnavs[0]) * 100).ToString("n2"), indexnavs[0].ToString(), indexnavs[1].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(indexnavs[1]) * 100).ToString("n2"), indexnavs[1].ToString(), indexnavs[2].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(indexnavs[2]) * 100).ToString("n2"), indexnavs[2].ToString(), CompundReturnSIndex == null ? "N/A" : Convert.ToDecimal(CompundReturnSIndex).ToString("n2"), _tempdataTable1.Rows[1][inceptionColumnName].ToString(), "");
                                    if (isAdditionalBenchMark)
                                        newFrmtDataTable.Rows.Add(_tempdataTable1.Rows[2][1].ToString(), addindexnavs[0].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(addindexnavs[0]) * 100).ToString("n2"), addindexnavs[0].ToString(), addindexnavs[1].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(addindexnavs[1]) * 100).ToString("n2"), addindexnavs[1].ToString(), addindexnavs[2].ToString() == "N/A" ? "N/A" : (10000 + Convert.ToDecimal(addindexnavs[2]) * 100).ToString("n2"), addindexnavs[2].ToString(), CompundReturnSIAddIndex == null ? "N/A" : Convert.ToDecimal(CompundReturnSIAddIndex).ToString("n2"), _tempdataTable1.Rows[2][inceptionColumnName].ToString(), "");
                                }



                            }
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


                        dsOldFormatFinal.Tables.Add(newFrmtDataTable);
                        #endregion
                        //dsOldFormatFinal.Tables[0].
                        // dsOldFormatFinal.Tables.Add(dtIndex);
                    }

                    #endregion
                    //objFinalstr.Append(GenerateTable(dsOldFormatFinal, isEquity, LastQuarterDate));
                    //sbFinal.Append(objFinalstr.ToString());
                    //ViewState["FinalExcelString"] = objFinalstr.ToString();
                    sbFinal.Append(GenerateTable(dsOldFormatFinal, isEquity, LastQuarterDate));

                    sbFinal.Append("<div style='clear:both;'></div>");






                    LiteralFinalReturns.Text = sbFinal.ToString();
                    lnkbtnDownload.Visible = true;

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
                                LiteralDisclaimer.Text += "\u2022 Nifty 500 to the extent of 65% and MSCI World Index to the extent of 35% of the net assets of the Scheme.<br/>";
                                break;
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

                string error = ex.InnerException.ToString();
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
            ddlSchemeList.SelectedIndex = 0;
            FundmanegerText.Text = "";
            txtInvestment.Text = "10000";
            ddlQtrEnd.SelectedIndex = 3;
            ddlYearEnd.SelectedIndex = ddlYearEnd.Items.Count-4;
            lnkbtnDownload.Visible = false;
            FillSchemeByOption();
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


        private void SetDateDropdown()
        {
            DateTime todayDate = DateTime.Today;
            // ddlQtrEnd.Items.Add();
        }
        #endregion

        #region: Generate  Table Methods

        public string GenerateTable(DataSet dsFormatFinal, bool isEquity, DateTime lastQtrDate)
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
                    sbResultTable.AppendLine("<table width='100%'><tr><td colspan='6'></td></tr><tr><td align='left' colspan='6' background='../Images/heading.jpg' height='22' width='500'><span class='style12'>&nbsp;&nbsp;<span class='style4'><img src='../Images/arrow.jpg' /> " + dsFormatFinal.Tables[0].Rows[0]["SchemeIndex"].ToString() + "</span></span></td></tr></table>");

                    //<td background="../Images/heading.jpg" height="26" width="960"><span class="style12">&nbsp;&nbsp;<span class="style4">TATA Mutual Fund </span></span></td>
                }
                int colorcount = 1;
                //sbResultTable.AppendLine("<br/>");


                sbResultTable.AppendLine("<div style='padding-top:5px;'><table class='resultgrid' cellspacing='0' cellpadding='4' rules='all' id='gvNewFormatEquity' style='border-width:1px;border-style:solid;border-collapse:collapse; border-color:#265599'>");

                sbResultTable.AppendLine("<tr>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='1'></th>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + lastQtrDate.AddYears(-1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + lastQtrDate.ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + "</th>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + lastQtrDate.AddYears(-2).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + lastQtrDate.AddYears(-1).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + "</th>");
                sbResultTable.AppendLine("<th class='headerCss' colspan='2'>" + lastQtrDate.AddYears(-3).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + lastQtrDate.AddYears(-2).ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture) + "</th>");
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
                        sbResultTable.AppendLine(Convert.ToString(dr["FirstyearReturn"]).Trim() == "N/A" ? "N/A" : Convert.ToDecimal(dr["FirstyearReturn"]).ToString("n2"));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["SecondyearRs"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                       // sbResultTable.AppendLine(Convert.ToString(dr["SecondyearReturn"]));
                        sbResultTable.AppendLine(Convert.ToString(dr["SecondyearReturn"]).Trim() == "N/A" ? "N/A" : Convert.ToDecimal(dr["SecondyearReturn"]).ToString("n2"));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["ThirdyearRs"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                       // sbResultTable.AppendLine(Convert.ToString(dr["ThirdyearReturn"]));
                        sbResultTable.AppendLine(Convert.ToString(dr["ThirdyearReturn"]).Trim() == "N/A" ? "N/A" : Convert.ToDecimal(dr["ThirdyearReturn"]).ToString("n2"));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        sbResultTable.AppendLine(Convert.ToString(dr["CagrInceptionRs"]));
                        sbResultTable.AppendLine("</td>");

                        sbResultTable.AppendLine("<td align='center'>");
                        //sbResultTable.AppendLine(Convert.ToString(dr["CagrInceptionReturn"]));
                        sbResultTable.AppendLine(Convert.ToString(dr["CagrInceptionReturn"]).Trim() == "N/A" ? "N/A" : Convert.ToDecimal(dr["CagrInceptionReturn"]).ToString("n2"));
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
            objFinalstr.Append("<br/>");
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

            Response.Write(objFinalstr.ToString());



            Response.Flush();
            Response.End();


        }
        #endregion

        #region : Rest Method
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

        #endregion

        
    }
}