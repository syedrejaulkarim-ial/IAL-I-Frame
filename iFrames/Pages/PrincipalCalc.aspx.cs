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
using iFrames.Pages;
using System.Globalization;

namespace iFrames.Pages
{
    public partial class PrincipleCalc : System.Web.UI.Page
    {


        #region : Global Varible
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

        #region : Page Event

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblerrmsg.Text = "";
            lbmsg.Text = "";
            if (!Page.IsPostBack)
                load_nature();
            trLS.Visible = false;
            trSIP.Visible = false;

            if (alotdatval)
            {
                Hidden1.Value = allotmentdate.ToString();
                Hidden2.Value = navdate.ToString();
            }

        }

        #endregion

        #region : Method Event
        protected void load_nature()
        {

            /*  WITHOUT USING LINQ   */

            //string SelectCommand = "SELECT [Nature] FROM [T_SCHEMES_NATURE] WHERE ([Nature] <> 'N.A') ORDER BY [Nature]";
            //// string val = "N.A";
            //SqlCommand cmdd = new SqlCommand(SelectCommand, cn);
            //SqlDataReader drr = null;
            //// cmd.Parameters..AddWithValue("@Nature",val);
            //try
            //{
            //    cn.Open();
            //    drr = cmdd.ExecuteReader();
            //    ddlNature.Items.Clear();
            //    //ddlNature.Items.Add("--");
            //    while (drr.Read())
            //    {
            //        ddlNature.Items.Add(drr.GetString(0));
            //    }
            //    cn.Close();
            //}
            //catch (Exception exp)
            //{

            //}
            //finally
            //{
            //    if (drr != null)
            //        drr.Close();
            //    if (cn != null)
            //        cn.Close();

            //}


            /* USING LINQ   */

            try
            {
                using (var natureData = new PrincipalCalcDataContext())
                {
                    var nature = from nat in natureData.T_SCHEMES_NATUREs
                                 where nat.Nature != "N.A"
                                 orderby nat.Nature
                                 select new
                                 {
                                     nat.Nature
                                 };
                    DataTable dataNature = null;
                    if (nature.Count() > 0)
                    {
                        dataNature = nature.ToDataTable();
                        foreach (DataRow drow in dataNature.Rows)
                        {
                            ddlNature.Items.Add(drow[0].ToString());
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

        #region: DropDown Event
        protected void ShowAllScheme(object sender, EventArgs e)
        {
            string selected_nature = ddlNature.SelectedValue;
            strNature = ddlNature.SelectedValue;
            string selected_option = ddlOption.SelectedValue;
            DateTime yearBacktodaysdate1 = DateTime.Today.AddYears(-1);
            try
            {


                /* By usins linq */
                using (var scheme = new PrincipalCalcDataContext())
                {
                    var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                     where
                                       t_fund_masters.MUTUALFUND_ID == 15 &&
                                       t_fund_masters.NATURE_ID ==
                                         ((from t_schemes_natures in scheme.T_SCHEMES_NATUREs
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




                    var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                         join T in fundtable
                                         on s.Fund_Id equals T.FUND_ID
                                         where s.T_FUND_MASTER.SUB_NATURE_ID != 2 && s.Nav_Check == 3 && s.Launch_Date <= yearBacktodaysdate1
                                         join tsi in scheme.T_SCHEMES_INDEXes
                                         on s.Scheme_Id equals tsi.SCHEME_ID
                                         where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
                                         orderby s.Sch_Short_Name
                                         select new
                                         {
                                             s.Sch_Short_Name
                                         }).Distinct();



                    DataTable dt2 = null;
                    if (scheme_name_1.Count() > 0)
                        dt2 = scheme_name_1.ToDataTable();

                    ddlSchemeList.Items.Clear();
                    ddlSchemeList.Items.Add("--");

                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow drw in dt2.Rows)
                            {
                                ddlSchemeList.Items.Add(drw[0].ToString());
                            }
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
                ddlMode.SelectedIndex = 0;
                FundmanegerText.Text = "";
                ddBenchMark.Items.Clear();
                if (trSIP.Visible == true)
                    trSIP.Visible = false;
                if (LsResultDiv.Visible == true)
                    LsResultDiv.Visible = false;
                if (showResult.Visible == true)
                    showResult.Visible = false;
                if (SipResultDiv.Visible == true)
                    SipResultDiv.Visible = false;
                if (trLS.Visible == true)
                    trLS.Visible = false;
            }


        }



        protected void SchemeListforNature(object sender, EventArgs e)
        {

            string selected_nature = ddlNature.SelectedValue;
            string selected_option = ddlOption.SelectedValue;

            /*Without LINQ */

            //            SqlDataReader dr = null;
            //            try
            //            {
            //                
            //                string sqlqry = @"SELECT  DISTINCT(S.SCHEME_ID),S.SCH_SHORT_NAME, S.SCHEME_NAME  ,T.fund_id FROM T_SCHEMES_MASTER S INNER JOIN 
            //(SELECT T_FUND_MASTER.FUND_ID,MUTUALFUND_ID,NATURE_ID FROM T_FUND_MASTER  WHERE  MUTUALFUND_ID =15  AND NATURE_ID=(select NATURE_ID from T_SCHEMES_NATURE WHERE Nature =@selectedNature ))T  ON S.FUND_ID=T.FUND_ID  WHERE NAV_CHECK<>2 AND S.OPTION_ID = @OPTION_ID ORDER BY SCH_SHORT_NAME ASC";

            //                cmd = new SqlCommand();
            //                cmd.Connection = cn;
            //                cmd.CommandText = sqlqry;
            //                cmd.Parameters.AddWithValue("@selectedNature", selected_nature);
            //                cmd.Parameters.AddWithValue("@OPTION_ID", selected_option);
            //                cn.Open();
            //                dr = cmd.ExecuteReader();
            //                ddlSchemeList.Items.Clear();
            //                ddlSchemeList.Items.Add("--");
            //                while (dr.Read())
            //                {
            //                    ddlSchemeList.Items.Add(dr.GetString(1));
            //                }
            //                cn.Close();
            //            }
            //            catch (Exception ex)
            //            {
            //            }
            //            finally
            //            {
            //                if (dr != null)
            //                    dr.Close();
            //            }



            try
            {

                /* By usins linq */
                using (var scheme = new PrincipalCalcDataContext())
                {
                    var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                     where
                                       t_fund_masters.MUTUALFUND_ID == 15 &&
                                       t_fund_masters.NATURE_ID ==
                                         ((from t_schemes_natures in scheme.T_SCHEMES_NATUREs
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

                    DataTable dt2 = null;
                    // var scheme_name_1 = null;
                    DateTime yearBacktodaysdate = DateTime.Today.AddYears(-1);

                    if (ddlOption.SelectedValue != "--")
                    {
                        var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                             join T in fundtable
                                             on s.Fund_Id equals T.FUND_ID
                                             where s.T_FUND_MASTER.SUB_NATURE_ID != 2 && s.Nav_Check == 3 && s.Option_Id.ToString() == selected_option
                                             && (s.Launch_Date <= yearBacktodaysdate)
                                             join tsi in scheme.T_SCHEMES_INDEXes
                                             on s.Scheme_Id equals tsi.SCHEME_ID
                                             where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
                                             orderby s.Sch_Short_Name
                                             select new
                                             {
                                                 s.Sch_Short_Name
                                             }).Distinct();
                        if (scheme_name_1.Count() > 0)
                            dt2 = scheme_name_1.ToDataTable();

                    }
                    else
                    {
                        var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                             join T in fundtable
                                             on s.Fund_Id equals T.FUND_ID
                                             where s.Nav_Check == 3 && s.Launch_Date <= yearBacktodaysdate
                                             join tsi in scheme.T_SCHEMES_INDEXes
                                             on s.Scheme_Id equals tsi.SCHEME_ID
                                             where tsi.INDEX_ID != 1 && tsi.INDEX_ID != 32//insert for not including I sec and Msci
                                             orderby s.Sch_Short_Name
                                             select new
                                             {
                                                 s.Sch_Short_Name
                                             }).Distinct();
                        if (scheme_name_1.Count() > 0)
                            dt2 = scheme_name_1.ToDataTable();
                    }








                    ddlSchemeList.Items.Clear();
                    ddlSchemeList.Items.Add("--");


                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow drw in dt2.Rows)
                        {
                            ddlSchemeList.Items.Add(drw[0].ToString());
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



            //var scheme_name = (from s in scheme.T_SCHEMES_MASTERs
            //                   join T in
            //                       (
            //                           (from t_fund_masters in scheme.T_FUND_MASTERs
            //                            where
            //                              t_fund_masters.MUTUALFUND_ID == 15 &&
            //                              t_fund_masters.NATURE_ID ==
            //                                ((from t_schemes_natures in scheme.T_SCHEMES_NATUREs
            //                                  where
            //                                    t_schemes_natures.Nature == selected_nature
            //                                  select new
            //                                  {
            //                                      t_schemes_natures.Nature_ID
            //                                  }).First().Nature_ID)
            //                            select new
            //                            {
            //                                t_fund_masters.FUND_ID,
            //                                t_fund_masters.MUTUALFUND_ID,
            //                                t_fund_masters.NATURE_ID
            //                            })) on new { Fund_Id = s.Fund_Id } equals new { Fund_Id = T.FUND_ID }
            //                   where
            //                     s.Nav_Check != 2 &&
            //                     s.Option_Id == selected_option
            //                   orderby
            //                     s.Sch_Short_Name
            //                   select new
            //                   {
            //                       s.Sch_Short_Name
            //                   }).Distinct();

            foreach (ListItem itemlist in ddlSchemeList.Items)
            {
                itemlist.Attributes.Add("title", itemlist.Text);
            }

            ddlMode.SelectedIndex = 0;
            ddBenchMark.Items.Clear();
            FundmanegerText.Text = "";

        }

        protected void SetBenchmarkSelScheme(object sender, EventArgs e)
        {
            string selected_Scheme = ddlSchemeList.SelectedValue.ToString();
            if (selected_Scheme.Trim('-').Length == 0)
            {
                ddBenchMark.Items.Clear();
                ddlMode.SelectedIndex = 0;
                return;
            }
            //using (var scheme = new PrincipalCalcDataContext())
            //{
            //var scode = from a in scheme.T_SCHEMES_MASTERs
            //            where a.Scheme_Name == selected_Scheme
            //            select a.Scheme_Id;

            //ddBenchMark.SelectedValue = Schemes.GetInd(scode.ToString()).Rows[0][0].ToString();
            using (var principl = new PrincipalCalcDataContext())
            {
                var index_name = (from t_index_masters in principl.T_INDEX_MASTERs
                                  where
                                    t_index_masters.INDEX_ID ==
                                      ((from t_schemes_indexes in principl.T_SCHEMES_INDEXes
                                        where
                                          t_schemes_indexes.SCHEME_ID ==
                                            ((from t_schemes_masters in principl.T_SCHEMES_MASTERs
                                              where
                                                t_schemes_masters.Sch_Short_Name == selected_Scheme
                                              select new
                                              {
                                                  t_schemes_masters.Scheme_Id
                                              }).First().Scheme_Id)
                                        select new
                                        {
                                            t_schemes_indexes.INDEX_ID
                                        }).Take(1).First().INDEX_ID)
                                  select new
                                  {
                                      t_index_masters.INDEX_NAME
                                  }).ToDataTable();

                ddBenchMark.Items.Clear();
                ddBenchMark.Items.Add(index_name.Rows[0][0].ToString());
                // ddBenchMark.SelectedValue.Replace(ddBenchMark.SelectedValue, index_name.Rows[0][0].ToString());// = index_name.Rows[0][0].ToString();

                var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                where ind.Sch_Short_Name == Convert.ToString(selected_Scheme)
                                select ind.Launch_Date;




                var fundmanager = (from fd in principl.T_FUND_MANAGERs
                                   join
                                cfm in principl.T_CURRENT_FUND_MANAGERs on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                   join
                                   fms in principl.T_FUND_MASTERs on cfm.FUND_ID equals fms.FUND_ID
                                   join
                                   sm in principl.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
                                   where
                                   sm.Sch_Short_Name == selected_Scheme && cfm.LATEST_FUNDMAN == true
                                   select new
                                     {
                                         fd.FUND_MANAGER_NAME
                                     }).Distinct().ToArray();//fd.FUND_MANAGER_NAME;

                //    t_schemes_masters in principl.T_SCHEMES_MASTERs on fd.FUND_ID equals t_schemes_masters.Fund_Id
                //    where t_schemes_masters.Sch_Short_Name == selected_Scheme 
                //   select fd.FUND_MAN ;
                ////}.ToDataTable(); 
                //fundmanager = fundmanager.Select(x => x.ToString()).ToArray(); 
                FundmanegerText.Text = "";
                if (fundmanager.Count() > 0)
                {
                    foreach (var fn in fundmanager.AsEnumerable())
                    {
                        FundmanegerText.Text += fn.FUND_MANAGER_NAME.ToString() + " , ";
                    }

                }
                FundmanegerText.Text = FundmanegerText.Text.TrimEnd(' ', ',');

                //FundmanegerText.Text = fundmanager.ToString();


                //string[] trmstr = allotdate.Single().ToString().Split(' ')[0].Split('/');
                //int month = Convert.ToInt32(trmstr[0]);
                //if (month < 10)
                //{
                //    trmstr[0] = "0" + trmstr[0]; 
                //}
                //int day = Convert.ToInt32(trmstr[1]);
                //if (day < 10)
                //{
                //    trmstr[1] = "0" + trmstr[1];
                //}


                //allotmentdate = Convert.ToDateTime(allotdate.Single().ToString());
                if (allotdate != null)
                    ViewState["alotedate"] = Convert.ToDateTime(allotdate.Single().ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //ViewState["alotedate"] = trmstr[1] + '\\' +  trmstr[0] + '\\' + trmstr[2];
                var scheme_id = from prin in principl.T_SCHEMES_MASTERs
                                where prin.Sch_Short_Name == ddlSchemeList.SelectedValue
                                select prin.Scheme_Id;

                var nav_date = (from k in principl.T_NAV_DIVs
                                where k.Scheme_Id == scheme_id.Single()
                                select k.Nav_Date).Max();

                //string[] trmstr2 = nav_date.ToString().Split(' ')[0].Split('/');
                //month = Convert.ToInt32(trmstr2[0]);
                //if (month < 10)
                //{
                //    trmstr2[0] = "0" + trmstr2[0];
                //}
                //day = Convert.ToInt32(trmstr2[1]);
                //if (day < 10)
                //{
                //    trmstr2[1] = "0" + trmstr2[1];
                //}
                //int month2 = Convert.ToInt32(trmstr[0]) - 1;
                //navdate = Convert.ToDateTime(nav_date.ToString());
                //ViewState["navDate"] = trmstr2[1] + '\\' + trmstr2[0] + '\\' + trmstr2[2];

                if (nav_date != null)
                    ViewState["navDate"] = Convert.ToDateTime(nav_date.ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //alotdatval = true;

                //Hidden1.Value = allotmentdate.ToString();
                //Hidden2.Value = navdate.ToString();
                //H1.Text = allotmentdate.ToString();
                //H2.Text = navdate.ToString();



            }
            // ddBenchMark.SelectedValue = Schemes.GetIndPrnpl(selected_Scheme).Rows[0][0].ToString();

            //}
            //string sch_code = from 
            foreach (ListItem itemlist in ddBenchMark.Items)
            {
                itemlist.Attributes.Add("title", itemlist.Text);
            }
            if (ddlMode.SelectedIndex != 0)
                ddlMode.SelectedIndex = 0;


            if (trSIP.Visible == true)
                trSIP.Visible = false;
            if (LsResultDiv.Visible == true)
                LsResultDiv.Visible = false;
            if (showResult.Visible == true)
                showResult.Visible = false;
            if (SipResultDiv.Visible == true)
                SipResultDiv.Visible = false;
            if (trLS.Visible == true)
                trLS.Visible = false;


        }

        protected void ShowRelativeDiv(object sender, EventArgs e)
        {

            if (ddlMode.SelectedValue != "--" && ddlSchemeList.SelectedIndex >= 1)
            {
                SipResultDiv.Visible = false;
                LsResultDiv.Visible = false;
                if (ddlMode.SelectedValue == "LumpSum")
                {
                    if (showResult.Visible == true)
                        showResult.Visible = false;
                    if (trLS.Visible == false)
                        trLS.Visible = true;
                    if (trSIP.Visible == true)
                        trSIP.Visible = false;


                }
                else if (ddlMode.SelectedValue == "SIP")
                {
                    if (showResult.Visible == true)
                        showResult.Visible = false;
                    if (trSIP.Visible == false)
                        trSIP.Visible = true;
                    if (trLS.Visible == true)
                        trLS.Visible = false;

                }
            }
            else if (ddlSchemeList.SelectedIndex < 1)
            {
                lblMessage.Text = "Please select scheme...";
                ddlSchemeList.Focus();
                SipResultDiv.Visible = false;
                LsResultDiv.Visible = false;
                trSIP.Visible = false;
                trLS.Visible = false;
                showResult.Visible = false;
                ddlMode.SelectedIndex = 0;
            }
            else
            {
                SipResultDiv.Visible = false;
                LsResultDiv.Visible = false;
                trSIP.Visible = false;
                trLS.Visible = false;
                showResult.Visible = false;
            }
        }

        #endregion

        #region : Button Event

        protected void SipCalcBtn_Click(object sender, EventArgs e)
        {

            DataTable SipDtable1 = null;
            DataTable SipDtable2 = null;
            DateTime allotDate;
            _toDate = default(System.DateTime);
            _fromDate = default(System.DateTime);
            _asOnDate = default(System.DateTime);
            //System.DateTime dtInitialDate = default(System.DateTime);

            try
            {

                _toDate = new DateTime(Convert.ToInt16(txtToDate.Value.Split('/')[2]),
                                         Convert.ToInt16(txtToDate.Value.Split('/')[1]),
                                         Convert.ToInt16(txtToDate.Value.Split('/')[0]));
                _fromDate = new DateTime(Convert.ToInt16(txtfrmDate.Value.Split('/')[2]),
                                         Convert.ToInt16(txtfrmDate.Value.Split('/')[1]),
                                         Convert.ToInt16(txtfrmDate.Value.Split('/')[0]));
                _asOnDate = new DateTime(Convert.ToInt16(txtAsOn.Value.Split('/')[2]),
                                         Convert.ToInt16(txtAsOn.Value.Split('/')[1]),
                                         Convert.ToInt16(txtAsOn.Value.Split('/')[0]));

                using (var principl = new PrincipalCalcDataContext())
                {
                    var scheme_id = from prin in principl.T_SCHEMES_MASTERs
                                    where prin.Sch_Short_Name == ddlSchemeList.SelectedValue
                                    select prin.Scheme_Id;
                    strSchid = scheme_id.Single().ToString(); //scheme id
                    Session["scheme_id"] = strSchid;

                    var indx_id = from ppl in principl.T_INDEX_MASTERs
                                  where ppl.INDEX_NAME == ddBenchMark.SelectedValue
                                  select ppl.INDEX_ID;

                    strIndexid = indx_id.Single().ToString();


                    var alotdate = from ind in principl.T_SCHEMES_MASTERs
                                   where ind.Sch_Short_Name == Convert.ToString(ddlSchemeList.SelectedValue)
                                   select ind.Launch_Date;

                    allotDate = Convert.ToDateTime(alotdate.Single().ToString());
                }

                double dblSIPamt = Convert.ToDouble(sipAmount.Text);// sip amount


                int strInitialDate = Convert.ToInt32(ddSipIntialDate.SelectedValue);  // intial date
                string strFrequency = ddPeriod.SelectedValue;
                string strInvestorType = "Individual/Huf";

                /** change start date***/
                int _sdate, _smonth, _syear;

                if (Convert.ToInt32(txtfrmDate.Value.Split('/')[0]) <= strInitialDate)
                {
                    _sdate = strInitialDate;
                    _smonth = Convert.ToInt32(txtfrmDate.Value.Split('/')[1]);
                    _syear = Convert.ToInt32(txtfrmDate.Value.Split('/')[2]);

                }
                else
                {

                    _sdate = strInitialDate;
                    _smonth = Convert.ToInt32(txtfrmDate.Value.Split('/')[1]);
                    _smonth = _smonth + 1;
                    if (Convert.ToInt32(txtfrmDate.Value.Split('/')[0]) == 31 && Convert.ToInt32(txtfrmDate.Value.Split('/')[1]) == 1)
                        _sdate = 5;
                    if (_smonth > 12)
                    {
                        _syear = Convert.ToInt32(txtfrmDate.Value.Split('/')[2]) + 1;
                        _smonth = 1;

                    }
                    else
                        _syear = Convert.ToInt32(txtfrmDate.Value.Split('/')[2]);
                }

                // special case 

                DateTime _fromDatemodified = new DateTime(_syear, _smonth, _sdate);


                ViewState["fromDatemodified"] = _fromDatemodified;



                TimeSpan dydiff = _fromDatemodified.Subtract(_toDate);
                if (dydiff.Days >= 1)
                {
                    lbmsg.Text = "Your SIP Start Date is " + _fromDatemodified.ToString("dd/MM/yyyy") + " which must be greater than or equal to the End Date";
                    if (!trSIP.Visible) trSIP.Visible = true;
                    return;
                }




                /***** using SQL TO LINQ *****/
                using (var principl = new PrincipalCalcDataContext())
                {

                    IMultipleResults datatble = principl.MFIE_SP_SIP_CALCULATER(strSchid, _fromDatemodified, _toDate, _asOnDate, dblSIPamt, strFrequency, strInvestorType, 0, 0, null, 1);
                    var firstTable = datatble.GetResult<CalcReturnData2>();
                    ////firstTable.TableName = "sdfsaf";
                    var secondTable = datatble.GetResult<CalcReturnData>();
                    SipDtable1 = firstTable.ToDataTable();
                    SipDtable2 = secondTable.ToDataTable();
                    SipDtable2.Columns.RemoveAt(0);
                    for (int cnt = 1; cnt <= 6; cnt++)
                    {
                        SipDtable1.Columns.RemoveAt(SipDtable1.Columns.Count - 1);
                        if (cnt == 1 || cnt == 2)
                            SipDtable1.Columns.RemoveAt(1);
                    }
                    SipDtable1.Columns.RemoveAt(3);
                    SipDtable1.Columns.RemoveAt(4);
                    SipDtable1.Columns[4].SetOrdinal(3);
                    if (SipDtable1.Rows.Count > 2)
                    {
                        SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);
                        SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);
                    }



                    //foreach (DataColumn dcol in SipDtable1.Columns)
                    for (int dcol = 0; dcol < SipDtable1.Columns.Count; dcol++)
                    {
                        SipDtable1.Columns[dcol].ColumnName = Microsoft.VisualBasic.Strings.StrConv(SipDtable1.Columns[dcol].ColumnName.Replace("_", " "), Microsoft.VisualBasic.VbStrConv.ProperCase, 0);
                        //SipDtable1.Columns[dcol].ColumnName = SentenceCase(SipDtable1.Columns[dcol].ColumnName.Replace("_", " "));

                    }

                    for (int dcol = 0; dcol < SipDtable2.Columns.Count; dcol++)
                    {
                        SipDtable2.Columns[dcol].ColumnName = Microsoft.VisualBasic.Strings.StrConv(SipDtable2.Columns[dcol].ColumnName.Replace("_", " "), Microsoft.VisualBasic.VbStrConv.ProperCase, 0);

                        //SipDtable2.Columns[dcol].ColumnName = SentenceCase(SipDtable2.Columns[dcol].ColumnName.Replace("_", " "));
                    }



                    DataTable dummySipDtable1 = new DataTable();

                    foreach (DataColumn dc in SipDtable1.Columns)
                    {
                        dummySipDtable1.Columns.Add(dc.ColumnName, typeof(String));
                    }



                    //for (int i = 0; i < SipDtable1.Rows.Count; i++)
                    //{
                    //    DataRow dr = dummySipDtable1.NewRow();
                    //    for (int j = 0; j < SipDtable1.Columns.Count; j++)
                    //    {
                    //        dr[i] += Convert.ToString(SipDtable1.Rows[i][j]);
                    //        //dummySipDtable1.Rows[i][j] = Convert.ToString(SipDtable1.Rows[i][j]);
                    //    }
                    //    dummySipDtable1.Rows.Add(dr);

                    //}


                    foreach (DataRow dr in SipDtable1.Rows)
                    {
                        DataRow dr1 = dummySipDtable1.NewRow();
                        foreach (DataColumn dcc in SipDtable1.Columns)
                            dr1[dcc.ColumnName] = Convert.ToString(dr[dcc.ColumnName]);
                        dummySipDtable1.Rows.Add(dr1);
                    }

                    foreach (DataRow drw in dummySipDtable1.Rows)
                    {
                        drw["Scheme Units"] = Convert.ToDouble(drw["Scheme Units"]).ToString("n2");
                    }

                    //for (int dcol = 0; dcol < dummySipDtable1.Columns.Count; dcol++)
                    //{
                    //    if (dummySipDtable1.Rows[dummySipDtable1.Rows.Count - 1][dcol].ToString() == "")
                    //        dummySipDtable1.Rows[dummySipDtable1.Rows.Count - 1][dcol] = "--";
                    //}

                    foreach (DataRow ddrr in dummySipDtable1.Rows)
                    {
                        foreach (DataColumn dcc in dummySipDtable1.Columns)
                        {
                            if (ddrr[dcc].ToString() == "")
                                ddrr[dcc] = "--";
                            if (ddrr[dcc].ToString().StartsWith("-"))
                            {
                                ddrr[dcc] = ddrr[dcc].ToString().Replace('-', ' ');
                            }
                            //dummySipDtable1.Add(ddrr[dcc]);
                        }
                    }


                    //foreach (DataColumn dcc in dummySipDtable1.Columns)
                    //{
                    //    if (dummySipDtable1.Rows[dummySipDtable1.Rows.Count - 1][dcc].ToString() == "0")
                    //        dummySipDtable1.Rows[dummySipDtable1.Rows.Count - 1][dcc] = "--";
                    //}

                    DataTable Show_Sip_tb = new DataTable();
                    Show_Sip_tb.Columns.Add("TotalOutflow", typeof(String));
                    Show_Sip_tb.Columns.Add("SumUnit", typeof(String));
                    Show_Sip_tb.Columns.Add("ValueasofDate", typeof(String));

                    Show_Sip_tb.Rows.Add(SipDtable2.Rows[0][3].ToString(), Convert.ToDouble(SipDtable2.Rows[0][2]).ToString("n2"), Convert.ToDouble(SipDtable2.Rows[0][4]).ToString("n2"));

                    SPGridViewtbl1.Columns[2].HeaderText = "Value as of Date " + "( " + txtAsOn.Value + " )";
                    SPGridViewtbl1.DataSource = Show_Sip_tb;
                    SPGridViewtbl1.DataBind();



                    if (SipDtable1 != null)
                        SipResultGrid.DataSource = dummySipDtable1;
                    else
                        SipResultGrid.DataSource = null;

                    //if (SipDtable2 != null)
                    //    SipResultGridTable2.DataSource = SipDtable2;
                    //else
                    //    SipResultGridTable2.DataSource = null;



                    SipResultGrid.DataBind();
                    //SipResultGridTable2.DataBind();

                    allotmentdate = allotDate;

                    PerformanceReturn(strSchid, strIndexid, allotDate);
                }
            }
            catch (Exception ex)
            {
                lblerrmsg.Text = ex.Message;
            }
            finally
            {

            }

            if (trSIP.Visible == false)
                trSIP.Visible = true;
            if (trLS.Visible == true)
                trLS.Visible = false;
            if (LsResultDiv.Visible == true)
                LsResultDiv.Visible = false;
            if (SipResultDiv.Visible == false)
                SipResultDiv.Visible = true;
            if (showResult.Visible == false)
                showResult.Visible = true;

            //ResetFormControlValues(this);
        }

        protected void LumpSumCalcBtn_Click(object sender, EventArgs e)
        {

            /* WITHOUT LINQ*/


            double? daysDiffVal = null;


            DateTime _toDateLs, _fromDateLs;

            //DateTime navDate = Convert.ToDateTime(ViewState["navDate"]);
            DateTime allotDate;//= Convert.ToDateTime(ViewState["alotedate"]);
            _fromDateLs = new DateTime(Convert.ToInt16(LumpStartDate.Value.Split('/')[2]),
                                     Convert.ToInt16(LumpStartDate.Value.Split('/')[1]),
                                     Convert.ToInt16(LumpStartDate.Value.Split('/')[0]));
            _toDateLs = new DateTime(Convert.ToInt16(LumpEndDate.Value.Split('/')[2]),
                                     Convert.ToInt16(LumpEndDate.Value.Split('/')[1]),
                                     Convert.ToInt16(LumpEndDate.Value.Split('/')[0]));
            double amountLs = Convert.ToDouble(LumpAmount.Text);
            string srollinprd = "D,0 Si";
            TimeSpan dateDiff2 = _toDateLs.Subtract(_fromDateLs);
            int day2 = dateDiff2.Days;
            srollinprd = day2.ToString() + " " + srollinprd;
            string daydiffCol = day2.ToString() + " " + "Day";

            try
            {
                using (var principl = new PrincipalCalcDataContext())
                {

                    var scheme_id = from prin in principl.T_SCHEMES_MASTERs
                                    where prin.Sch_Short_Name == ddlSchemeList.SelectedValue
                                    select prin.Scheme_Id;
                    strSchid = scheme_id.Single().ToString(); //scheme id
                    Session["scheme_id"] = strSchid;

                    var indx_id = from ppl in principl.T_INDEX_MASTERs
                                  where ppl.INDEX_NAME == ddBenchMark.SelectedValue
                                  select ppl.INDEX_ID;

                    strIndexid = indx_id.Single().ToString();


                    var alotdate = from ind in principl.T_SCHEMES_MASTERs
                                   where ind.Sch_Short_Name == Convert.ToString(ddlSchemeList.SelectedValue)
                                   select ind.Launch_Date;

                    allotDate = Convert.ToDateTime(alotdate.Single().ToString());

                }


                try
                {
                    DataSet ds = new DataSet();
                    DataTable datatble = null;
                    //DataTable indexDataTable = null;
                    int val = 0;

                    cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SchemeIDs", strSchid));
                    cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                    cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                    cmd.Parameters.Add(new SqlParameter("@DateTo", _toDateLs.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                    cmd.Parameters.Add(new SqlParameter("@RoundTill", 4));
                    cmd.Parameters.Add(new SqlParameter("@RollingPeriodin", srollinprd));
                    cmd.Parameters.Add(new SqlParameter("@RollingPeriod", val));
                    cmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                    cmd.Parameters.Add(new SqlParameter("@RollingFrequency", val));
                    cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                    cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    datatble = ds.Tables[0];

                    if (Convert.ToString(datatble.Rows[0][daydiffCol]) != "N/A")
                        daysDiffVal = Convert.ToDouble(datatble.Rows[0][daydiffCol]);


                    DataTable Show_Ls_tb = new DataTable();
                    Show_Ls_tb.Columns.Add("AmountInvested", typeof(String));
                    Show_Ls_tb.Columns.Add("ValueasofDate", typeof(String));



                    Double CompundReturnDayVal = 0;

                    if (daysDiffVal != null)
                    {
                        if (day2 > 365)
                            CompundReturnDayVal = amountLs * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day2 / 365, 4));
                        else
                            CompundReturnDayVal = amountLs + amountLs * (double)daysDiffVal / 100;
                    }

                    Show_Ls_tb.Rows.Add(amountLs.ToString(), CompundReturnDayVal.ToString("n2"));


                    LsGridViewtbl1.Columns[0].HeaderText = "Amount Invested " + "( " + LumpStartDate.Value + " )";
                    LsGridViewtbl1.Columns[1].HeaderText = "Value as of Date " + "( " + LumpEndDate.Value + " )";
                    LsGridViewtbl1.DataSource = Show_Ls_tb;
                    LsGridViewtbl1.DataBind();


                    allotmentdate = allotDate;
                    PerformanceReturn(strSchid, strIndexid, allotDate);


                }
                catch (Exception ex)
                {
                    //string s = ex.ToString();
                    lblerrmsg.Text = ex.Message;
                }
                finally
                {

                }
            }

            catch (Exception exp)
            {
                lbmsg.Text = "Error!!! Can't produce report.";
            }
            finally
            {

            }
            if (trSIP.Visible == true)
                trSIP.Visible = false;
            if (LsResultDiv.Visible == false)
                LsResultDiv.Visible = true;
            if (SipResultDiv.Visible == true)
                SipResultDiv.Visible = false;
            if (trLS.Visible == false)
                trLS.Visible = true;
            if (showResult.Visible == false)
                showResult.Visible = true;

            //ResetFormControlValues(this);
        }


        private void PerformanceReturn(string schmeId, string indexId, DateTime allotDate)
        {

            DateTime LastQtrEndDate = GetLastQuarterDates();
            //string AdditionalBechmarkid = "12";// for the Nifty and for bse 100 it is 33 and bse sensex it is 13
            double? sinceInceptionIndex = null, sinceInceptionAddlIndex = null;
            double? sinceInception = null;
            DateTime LastQtrEndDateInRecord = LastQtrEndDate;//= null;
            SqlCommand sqlcmd, sqlcmd1;
            //string sqlqry = "select top(1) Nav_Date from T_nav_div where scheme_id='" + schmeId + "'and nav_date<= '" + LastQtrEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "' order by nav_date desc";
            //sqlcomnd = new SqlCommand(sqlqry, cn);
            //LastQtrEndDateInRecord = (DateTime)sqlcomnd.ExecuteScalar();

            string datediff = string.Empty;
            // LastQtrEndDateInRecord = GetLatestNavdate(LastQtrEndDate, schmeId);
            //if (LastQtrEndDateInRecord == LastQtrEndDate)
            //    datediff = "365 d";
            //else
            //    datediff = "364 d";

            using (var principl = new PrincipalCalcDataContext())
            {
                var sbNtrid = (from tfm in principl.T_FUND_MASTERs
                               where tfm.FUND_ID ==
                               (from tsm in principl.T_SCHEMES_MASTERs
                                where tsm.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    tsm.Fund_Id
                                }
                               ).First().Fund_Id
                               select new { tfm.SUB_NATURE_ID }).First().SUB_NATURE_ID;
                SubNatureId = sbNtrid.ToString();
            }

            //monthDate
            strNature = ddlNature.SelectedValue;
            try
            {

                //sqlcmd = new SqlCommand("MFIE_SP_FIXPERIODICROLLINGRETURNS", cn);
                //sqlcmd.CommandType = CommandType.StoredProcedure;
                //sqlcmd.CommandTimeout = 2000;
                //sqlcmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
                //sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                //sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                //sqlcmd.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDateInRecord.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                //sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriod", ""));
                //sqlcmd.Parameters.Add(new SqlParameter("@FixPeriodic", "YoY"));
                //sqlcmd.Parameters.Add(new SqlParameter("@RoundTill", 4));
                //sqlcmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));
                // string datediff = "365 d";
                int oval = 0;
                string IndexBenchmarlIds = null;
                Decimal dcmlIndexid = 0;
                DataTable nwdataTble = null, nwIndexDataTble = null, odatatble = null, oindexDataTable = null;
                DataSet dset1 = new DataSet();
                DataSet dset2 = new DataSet();
                DataSet dst1 = new DataSet();
                DataSet dst2 = new DataSet();

                if (strNature == "Equity" || strNature == "Balanced" || strNature == "Dynamic/Asset Allocation")
                {
                    dcmlIndexid = (strIndexid != "12" ? 12 : 13);

                    if (indexId.ToString() == "12")
                        IndexBenchmarlIds = "12,13";
                    else if (indexId.ToString() == "13")
                        IndexBenchmarlIds = "13,12";
                    else
                        IndexBenchmarlIds = indexId + ",12";
                }

                else
                {

                    if (SubNatureId == "13" || strNature == "Liquid")// || SubNatureId == "41")
                    {
                        IndexBenchmarlIds = indexId + ",135";
                        dcmlIndexid = 135;
                    }
                    else //if (SubNatureId == "4")
                    {
                        IndexBenchmarlIds = indexId + ",134";
                        dcmlIndexid = 134;
                    }

                }

                List<double> schemval = new List<double>();
                List<double> benchval = new List<double>();
                List<double> adbenchval = new List<double>();

                DateTime tempdate = LastQtrEndDateInRecord;
                DateTime tempdate2 = LastQtrEndDateInRecord;
                //int rcount = 0;
                DateTime[] arrdate = new DateTime[6];


                for (int i = 0; i < 3; i++)
                {

                    //TimeSpan dateDiff2 = _toDateLs.Subtract(_fromDateLs);
                    //int day2 = dateDiff2.Days;


                    tempdate = GetLatestNavdate(LastQtrEndDate.AddYears(-(i + 1)), schmeId);

                    tempdate2 = GetLatestNavdate(LastQtrEndDate.AddYears(-i), schmeId);

                    arrdate[2 * i + 0] = tempdate;
                    arrdate[2 * i + 1] = tempdate2;


                    TimeSpan dayyDiff2 = tempdate2.Subtract(tempdate);
                    int dday2 = dayyDiff2.Days;

                    datediff = dday2 + " d";

                    //if (LastQtrEndDateInRecord == LastQtrEndDate)
                    //    datediff = "365 d";
                    //else
                    //    datediff = "364 d";

                    sqlcmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", cn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 2000;
                    sqlcmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
                    sqlcmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                    sqlcmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                    sqlcmd.Parameters.Add(new SqlParameter("@DateTo", tempdate2.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                    sqlcmd.Parameters.Add(new SqlParameter("@RoundTill", 6));
                    sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriodin", datediff));
                    sqlcmd.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
                    sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                    sqlcmd.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
                    sqlcmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                    sqlcmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));


                    //sqlcmd1 = new SqlCommand("MFIE_SP_FIXEDPERIODICINDEXROLLINGRETURN", cn);
                    //sqlcmd1.CommandType = CommandType.StoredProcedure;
                    //sqlcmd1.CommandTimeout = 2000;
                    //sqlcmd1.Parameters.Add(new SqlParameter("@IndexIDs", IndexBenchmarlIds));
                    //sqlcmd1.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                    //sqlcmd1.Parameters.Add(new SqlParameter("@DateFrom", ""));
                    //sqlcmd1.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDateInRecord.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                    //sqlcmd1.Parameters.Add(new SqlParameter("@RollingPeriod", ""));
                    //sqlcmd1.Parameters.Add(new SqlParameter("@FixPeriodic", "YoY"));
                    //sqlcmd1.Parameters.Add(new SqlParameter("@RoundTill", 4));
                    //sqlcmd1.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));




                    sqlcmd1 = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", cn);
                    sqlcmd1.CommandType = CommandType.StoredProcedure;
                    sqlcmd1.CommandTimeout = 2000;
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndexIDs", IndexBenchmarlIds));
                    sqlcmd1.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                    sqlcmd1.Parameters.Add(new SqlParameter("@DateFrom", ""));
                    sqlcmd1.Parameters.Add(new SqlParameter("@DateTo", tempdate2.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                    sqlcmd1.Parameters.Add(new SqlParameter("@RoundTill", 6));
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", datediff));
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                    sqlcmd1.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
                    sqlcmd1.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                    sqlcmd1.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



                    dset1.Clear();
                    dset2.Clear();
                    dset1.Reset();
                    dset2.Reset();
                    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlcmd);
                    sqlDa.Fill(dset1);
                    sqlDa.SelectCommand = sqlcmd1;
                    sqlDa.Fill(dset2);

                    nwdataTble = null;
                    nwIndexDataTble = null;
                    nwdataTble = dset1.Tables[0];
                    nwIndexDataTble = dset2.Tables[0];


                    if (nwdataTble.Rows[0][dday2 + " Day"].ToString() != "N/A")
                        schemval.Add(Convert.ToDouble(nwdataTble.Rows[0][dday2 + " Day"]));

                    for (int k = 0; k < 2; k++)
                    {
                        if (nwIndexDataTble.Rows[k][0].ToString() == strIndexid && nwIndexDataTble.Rows[k][dday2 + " Day"].ToString() != "")
                        {
                            benchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[k][dday2 + " Day"].ToString()));
                        }
                        if (nwIndexDataTble.Rows[k][0].ToString() == dcmlIndexid.ToString() && nwIndexDataTble.Rows[k][dday2 + " Day"].ToString() != "")
                        {
                            adbenchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[k][dday2 + " Day"].ToString()));
                        }
                    }


                    //if (nwdataTble.Rows[i][dday2 + " Day"].ToString() !="N/A")
                    //schemval.Add(Convert.ToDouble(nwdataTble.Rows[i][dday2 + " Day"]));

                    //for( int k = 0;k<2;k++)
                    //{
                    //    if (nwIndexDataTble.Rows[2 * i + k][0].ToString() == strIndexid && nwIndexDataTble.Rows[2 * i + k][dday2 + " Day"].ToString() != "")
                    //    {
                    //        benchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[2 * i + k][dday2 + " Day"].ToString()));
                    //    }
                    //    if (nwIndexDataTble.Rows[2 * i + k][0].ToString() == dcmlIndexid.ToString() && nwIndexDataTble.Rows[2 * i + k][dday2 + " Day"].ToString() != "")
                    //    {
                    //        adbenchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[2 * i + k][dday2 + " Day"].ToString()));
                    //    }
                    //    //rcount++;
                    //}





                    //foreach (DataRow dr in nwIndexDataTble.Rows)
                    //{
                    //    if (dr[0].ToString() == strIndexid && dr[dday2 + " Day"].ToString() !="")
                    //    {
                    //        benchval.Add(Convert.ToDouble(dr[dday2 + " Day"].ToString()));
                    //    }
                    //    if (dr[0].ToString() == dcmlIndexid.ToString() && dr[dday2 + " Day"].ToString() != "")
                    //    {
                    //        adbenchval.Add(Convert.ToDouble(dr[dday2 + " Day"].ToString()));
                    //    }
                    //}


                    //var vindexnavVal = (from dt in nwIndexDataTble.AsEnumerable()
                    //                    where dt.Field<Decimal>("index_id") == Convert.ToDecimal(strIndexid)
                    //                    select new
                    //{
                    //    value = dt.Field<double>(dday2 + " Day")
                    //}).Single();

                    ////vindexnavVal.v

                    //var vaddlindexnavVal = (from dt in nwIndexDataTble.AsEnumerable()
                    //                        where dt.Field<Decimal>("index_id") == dcmlIndexid
                    //                        select new
                    //                            {
                    //                                value = dt.Field<double>(dday2 + " Day")
                    //                            }).Single();


                    //benchval.Add(Convert.ToDouble(vindexnavVal.value));
                    //adbenchval.Add(Convert.ToDouble(vaddlindexnavVal.value));

                }

                ////addlIndexDataTble = dset2.Tables[0];
                //double[] navVal = (from dt in nwdataTble.AsEnumerable()
                //                   select dt.Field<double>("nav")).Take(3).ToArray();

                //double[] indexnavVal = (from dt in nwIndexDataTble.AsEnumerable()
                //                        where dt.Field<Decimal>("index_id") == Convert.ToDecimal(strIndexid)
                //                        select dt.Field<double>("index_value")).Take(3).ToArray();



                //double[] addlindexnavVal = (from dt in nwIndexDataTble.AsEnumerable()
                //                            where dt.Field<Decimal>("index_id") == dcmlIndexid
                //                            select dt.Field<double>("index_value")).Take(3).ToArray();


                double[] navVal = schemval.ToArray();
                double[] indexnavVal = benchval.ToArray();
                double[] addlindexnavVal = adbenchval.ToArray();

                //string[] strindexnavVal = ;


                //string[] dateQtr = (from mwt in nwdataTble.AsEnumerable()
                //                    select mwt.Field<string>("Nav_date")).Take(4).ToArray();



                SqlCommand cmdd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", cn);
                cmdd.CommandType = CommandType.StoredProcedure;
                cmdd.CommandTimeout = 2000;
                cmdd.Parameters.Add(new SqlParameter("@SchemeIDs", strSchid));
                cmdd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                cmdd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                cmdd.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDateInRecord.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdd.Parameters.Add(new SqlParameter("@RoundTill", 6));
                cmdd.Parameters.Add(new SqlParameter("@RollingPeriodin", "0 Si"));
                cmdd.Parameters.Add(new SqlParameter("@RollingPeriod", oval));
                cmdd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                cmdd.Parameters.Add(new SqlParameter("@RollingFrequency", oval));
                cmdd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                cmdd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));




                SqlCommand indexCmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", cn);
                indexCmd.CommandType = CommandType.StoredProcedure;
                indexCmd.CommandTimeout = 2000;
                indexCmd.Parameters.Add(new SqlParameter("@IndexIDs", IndexBenchmarlIds));
                indexCmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                indexCmd.Parameters.Add(new SqlParameter("@DateFrom", allotDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                indexCmd.Parameters.Add(new SqlParameter("@DateTo", LastQtrEndDateInRecord.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                indexCmd.Parameters.Add(new SqlParameter("@RoundTill", 6));
                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", "0 Si"));
                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", oval));
                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                indexCmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", oval));
                indexCmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                indexCmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



                SqlDataAdapter sqlda = new SqlDataAdapter(cmdd);


                sqlda.Fill(dst1);
                sqlda.SelectCommand = indexCmd;
                sqlda.Fill(dst2);
                odatatble = dst1.Tables[0];
                oindexDataTable = dst2.Tables[0];



                if (odatatble.Rows[0]["Since Inception"].ToString() != "N/A")
                    sinceInception = Convert.ToDouble(odatatble.Rows[0]["Since Inception"]);

                if (oindexDataTable.Rows[0]["Index_id"].ToString() == strIndexid)
                {
                    if (oindexDataTable.Rows[0]["0 Si"].ToString() != "N/A")
                        sinceInceptionIndex = Convert.ToDouble(oindexDataTable.Rows[0]["0 Si"]);

                    if (oindexDataTable.Rows[1]["0 Si"].ToString() != "N/A")
                        sinceInceptionAddlIndex = Convert.ToDouble(oindexDataTable.Rows[1]["0 Si"]);
                }
                else
                {
                    if (oindexDataTable.Rows[0]["0 Si"].ToString() != "N/A")
                        sinceInceptionAddlIndex = Convert.ToDouble(oindexDataTable.Rows[0]["0 Si"]);

                    if (oindexDataTable.Rows[1]["0 Si"].ToString() != "N/A")
                        sinceInceptionIndex = Convert.ToDouble(oindexDataTable.Rows[1]["0 Si"]);
                }



                //TimeSpan dateDiff = GetLastQuarterDates().Subtract(allotDate);

                TimeSpan dateDiff = GetLatestNavdate(GetLastQuarterDates(), schmeId).Subtract(allotDate);
                int day = dateDiff.Days;

                Double? CompundReturnSI = null;
                Double? CompundReturnSIndex = null;
                Double? CompundReturnSIAddIndex = null;
                if (sinceInception != null)
                    CompundReturnSI = 10000 * Math.Pow(1 + (double)sinceInception / 100, Math.Round((double)day / 365, 4));
                if (sinceInceptionIndex != null)
                    CompundReturnSIndex = 10000 * Math.Pow(1 + (double)sinceInceptionIndex / 100, Math.Round((double)day / 365, 4));
                if (sinceInceptionAddlIndex != null)
                    CompundReturnSIAddIndex = 10000 * Math.Pow(1 + (double)sinceInceptionAddlIndex / 100, Math.Round((double)day / 365, 4));

                DataTable perfDataTable = new DataTable();
                perfDataTable.Columns.Add("qtryear", typeof(string));
                perfDataTable.Columns.Add("navReturn", typeof(string));
                perfDataTable.Columns.Add("navp2pReturn", typeof(string));
                perfDataTable.Columns.Add("indexReturn", typeof(string));
                perfDataTable.Columns.Add("indexp2pReturn", typeof(string));
                perfDataTable.Columns.Add("addlindexReturn", typeof(string));
                perfDataTable.Columns.Add("addlindexp2pReturn", typeof(string));

                //double?[] dindexnavVal = new double?[3];
                //double?[] daddlindexnavVal = new double?[3];

                //Code added for dealing non index value

                List<double?> ddindexnavVal = new List<double?>();
                List<double?> ddaddlindexnavVal = new List<double?>();

                if (indexnavVal.Count() == 0)
                {
                    //indexnavVal = new double[3] { 0, 0, 0 };
                    ddindexnavVal.Add(null); ddindexnavVal.Add(null); ddindexnavVal.Add(null);
                }
                else
                {
                    foreach (double d in indexnavVal)
                        ddindexnavVal.Add(d);
                }

                if (addlindexnavVal.Count() == 0)
                {
                    ddaddlindexnavVal.Add(null); ddaddlindexnavVal.Add(null); ddaddlindexnavVal.Add(null);
                    // addlindexnavVal = new double[3] { 0, 0, 0 };
                }
                else
                {
                    foreach (double d in addlindexnavVal)
                        ddaddlindexnavVal.Add(d);
                }





                //if (navVal.Count() > 0 || ddindexnavVal.Count > 0)
                //{

                //    //addlindexnavVal[0] = 0; addlindexnavVal[1] = 0; addlindexnavVal[2] = 0;

                //    if (navVal.Count() == 0)
                //        perfDataTable.Rows.Add(LastQtrEndDateInRecord.AddYears(-1).ToString("dd-MMM-yy") + " - " + LastQtrEndDateInRecord.ToString("dd-MMM-yy"), "N.A.", "N.A.", ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
                //    else
                //        perfDataTable.Rows.Add(LastQtrEndDateInRecord.AddYears(-1).ToString("dd-MMM-yy") + " - " + LastQtrEndDateInRecord.ToString("dd-MMM-yy"), navVal[0].ToString("n2"), (10000 + navVal[0] * 100).ToString("n2"), ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
                //    if (navVal.Count() > 1)
                //        perfDataTable.Rows.Add(LastQtrEndDateInRecord.AddYears(-2).ToString("dd-MMM-yy") + " - " + LastQtrEndDateInRecord.AddYears(-1).ToString("dd-MMM-yy"), navVal[1].ToString("n2"), (10000 + navVal[1] * 100).ToString("n2"), ddindexnavVal[1] == null ? "" : Convert.ToDouble(ddindexnavVal[1]).ToString("n2"), ddindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[1]) * 100).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[1]).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[1]) * 100).ToString("n2"));
                //    if (navVal.Count() > 2)
                //        perfDataTable.Rows.Add(LastQtrEndDateInRecord.AddYears(-3).ToString("dd-MMM-yy") + " - " + LastQtrEndDateInRecord.AddYears(-2).ToString("dd-MMM-yy"), navVal[2].ToString("n2"), (10000 + navVal[2] * 100).ToString("n2"), ddindexnavVal[2] == null ? "" : Convert.ToDouble(ddindexnavVal[2]).ToString("n2"), ddindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[2]) * 100).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[2]).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[2]) * 100).ToString("n2"));

                //    //perfDataTable.Rows.Add("Since Inception to " + GetLastQuarterDates().ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
                //    perfDataTable.Rows.Add("Since Inception as on <br/> " + allotDate.ToString("dd-MMM-yy") + " to " + LastQtrEndDateInRecord.ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
                //}

                int amount = 10000;
                if (ddlMode.SelectedItem.Text.ToUpper() != "SIP")
                    amount = Convert.ToInt32(LumpAmount.Text);
                else
                    amount = 10000;

                if (navVal.Count() > 0 || ddindexnavVal.Count > 0)
                {

                    //addlindexnavVal[0] = 0; addlindexnavVal[1] = 0; addlindexnavVal[2] = 0;

                    if (navVal.Count() == 0)
                        perfDataTable.Rows.Add(arrdate[0].ToString("dd-MMM-yy") + " - " + arrdate[1].ToString("dd-MMM-yy"), "N.A.", "N.A.", ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (amount + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (amount + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
                    else
                        perfDataTable.Rows.Add(arrdate[0].ToString("dd-MMM-yy") + " - " + arrdate[1].ToString("dd-MMM-yy"), navVal[0].ToString("n2"), (amount + navVal[0] * 100).ToString("n2"), ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (amount + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (amount + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
                    if (navVal.Count() > 1)
                        perfDataTable.Rows.Add(arrdate[2].ToString("dd-MMM-yy") + " - " + arrdate[3].ToString("dd-MMM-yy"), navVal[1].ToString("n2"), (amount + navVal[1] * 100).ToString("n2"), ddindexnavVal[1] == null ? "" : Convert.ToDouble(ddindexnavVal[1]).ToString("n2"), ddindexnavVal[1] == null ? "" : (amount + Convert.ToDouble(ddindexnavVal[1]) * 100).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[1]).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : (amount + Convert.ToDouble(ddaddlindexnavVal[1]) * 100).ToString("n2"));
                    if (navVal.Count() > 2)
                        perfDataTable.Rows.Add(arrdate[4].ToString("dd-MMM-yy") + " - " + arrdate[5].ToString("dd-MMM-yy"), navVal[2].ToString("n2"), (amount + navVal[2] * 100).ToString("n2"), ddindexnavVal[2] == null ? "" : Convert.ToDouble(ddindexnavVal[2]).ToString("n2"), ddindexnavVal[2] == null ? "" : (amount + Convert.ToDouble(ddindexnavVal[2]) * 100).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[2]).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : (amount + Convert.ToDouble(ddaddlindexnavVal[2]) * 100).ToString("n2"));

                    //perfDataTable.Rows.Add("Since Inception to " + GetLastQuarterDates().ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
                    perfDataTable.Rows.Add("Since Inception as on <br/> " + allotDate.ToString("dd-MMM-yy") + " to " + arrdate[1].ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
                }



                //if (navVal.Count() > 0)
                //{

                //    //addlindexnavVal[0] = 0; addlindexnavVal[1] = 0; addlindexnavVal[2] = 0;

                //    perfDataTable.Rows.Add(dateQtr[1] + " - " + dateQtr[0], navVal[0].ToString("n2"), (10000 + navVal[0] * 100).ToString("n2"), ddindexnavVal[0] == null ? "" : Convert.ToDouble(ddindexnavVal[0]).ToString("n2"), ddindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[0]) * 100).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[0]).ToString("n2"), ddaddlindexnavVal[0] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[0]) * 100).ToString("n2"));
                //    if (navVal.Count() > 1)
                //        perfDataTable.Rows.Add(dateQtr[2] + " - " + dateQtr[1], navVal[1].ToString("n2"), (10000 + navVal[1] * 100).ToString("n2"), ddindexnavVal[1] == null ? "" : Convert.ToDouble(ddindexnavVal[1]).ToString("n2"), ddindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[1]) * 100).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[1]).ToString("n2"), ddaddlindexnavVal[1] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[1]) * 100).ToString("n2"));
                //    if (navVal.Count() > 2)
                //        perfDataTable.Rows.Add(dateQtr[3] + " - " + dateQtr[2], navVal[2].ToString("n2"), (10000 + navVal[2] * 100).ToString("n2"), ddindexnavVal[2] == null ? "" : Convert.ToDouble(ddindexnavVal[2]).ToString("n2"), ddindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddindexnavVal[2]) * 100).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : Convert.ToDouble(ddaddlindexnavVal[2]).ToString("n2"), ddaddlindexnavVal[2] == null ? "" : (10000 + Convert.ToDouble(ddaddlindexnavVal[2]) * 100).ToString("n2"));

                //    perfDataTable.Rows.Add("Since Inception to " + GetLastQuarterDates().ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
                //    //perfDataTable.Rows.Add("Since Inception as on " + allotDate.ToString("dd-MMM-yy") + " to " + GetLastQuarterDates().ToString("dd-MMM-yy"), sinceInception == null ? "" : Convert.ToDouble(sinceInception).ToString("n2"), CompundReturnSI == null ? "" : Convert.ToDouble(CompundReturnSI).ToString("n2"), sinceInceptionIndex == null ? "" : Convert.ToDouble(sinceInceptionIndex).ToString("n2"), CompundReturnSIndex == null ? "" : Convert.ToDouble(CompundReturnSIndex).ToString("n2"), sinceInceptionAddlIndex == null ? "" : Convert.ToDouble(sinceInceptionAddlIndex).ToString("n2"), CompundReturnSIAddIndex == null ? "" : Convert.ToDouble(CompundReturnSIAddIndex).ToString("n2"));
                //}









                //LsListViewtbl.DataSource = perfDataTable;
                //LsListViewtbl.DataBind(); 
                foreach (DataRow drr in perfDataTable.Rows)
                {
                    foreach (DataColumn dcc in perfDataTable.Columns)
                    {
                        if (drr[dcc].ToString() == "")
                            drr[dcc] = "--";
                    }
                }

                if (perfDataTable.Rows.Count > 0)
                {
                    CommonResultGridView.DataSource = perfDataTable;
                    CommonResultGridView.DataBind();
                }
                //TableNote.Text = "";

            }
            catch (Exception exc)
            {
                lblerrmsg.Text = exc.Message;
            }
            if (TableNote.Visible == false) TableNote.Visible = true;

            //return perfDataTable;
        }




        private DateTime GetLastQuarterDates()
        {
            DateTime currentDate = DateTime.Today;
            int month = currentDate.Month;
            DateTime qtrenddate = currentDate;

            if (month >= 1 && month <= 3)
            {
                qtrenddate = new DateTime(currentDate.Year - 1, 12, 31);

            }
            if (month >= 4 && month <= 6)
            {
                qtrenddate = new DateTime(currentDate.Year, 3, 31);

            }
            if (month >= 7 && month <= 9)
            {
                qtrenddate = new DateTime(currentDate.Year, 6, 30);

            }
            if (month >= 10 && month <= 12)
            {
                qtrenddate = new DateTime(currentDate.Year, 9, 30);
            }

            return qtrenddate;
        }


        private DateTime GetLatestNavdate(DateTime LastQtrEndDate, string schmeId)
        {
            DateTime LastQtrEndDateInRecord = LastQtrEndDate;
            using (var principl = new PrincipalCalcDataContext())
            {
                var dateEnd = (from tnd in principl.T_NAV_DIVs
                               where ((tnd.Scheme_Id == Convert.ToDecimal(schmeId)) && (tnd.Nav_Date <= LastQtrEndDateInRecord))
                               orderby tnd.Nav_Date descending
                               select tnd.Nav_Date).Take(1);

                LastQtrEndDateInRecord = dateEnd.Count() > 0 ? Convert.ToDateTime(dateEnd.Single()) : LastQtrEndDate;

            }
            return LastQtrEndDateInRecord;
        }



        private DateTime GetLatestNavdate2(DateTime LastQtrEndDate, string schmeId)
        {
            DateTime LastQtrEndDateInRecord = LastQtrEndDate;
            using (var principl = new PrincipalCalcDataContext())
            {
                var dateEnd = (from tnd in principl.T_NAV_DIVs
                               where ((tnd.Scheme_Id == Convert.ToDecimal(schmeId)) && (tnd.Nav_Date >= LastQtrEndDateInRecord))
                               orderby tnd.Nav_Date ascending
                               select tnd.Nav_Date).Take(1);

                LastQtrEndDateInRecord = dateEnd.Count() > 0 ? Convert.ToDateTime(dateEnd.Single()) : LastQtrEndDate;

            }
            return LastQtrEndDateInRecord;
        }



        #endregion

        #region : Reset Event

        private void ResetFormControlValues(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            {
                                if (c.ID == "ddlSchemeList" || c.ID == "ddBenchMark")
                                    ((DropDownList)c).Items.Clear();
                                else
                                    ((DropDownList)c).ClearSelection();
                                break;
                            }

                    }
                }
            }
        }

        protected void ResetForm(object sender, EventArgs e)
        {
            ResetFormControlValues(this);
            ddlOption.SelectedIndex = 0;
            ddlMode.SelectedIndex = 0;
            showResult.Visible = false;
        }

        #endregion

        #region:  Bound Event

        private bool allotLess3Year()
        {
            bool boolCheck = true;
            DateTime year3Backtoday = DateTime.Today.AddYears(-3);

            if (allotmentdate <= year3Backtoday)
                boolCheck = false;

            return boolCheck;
        }

        public void GV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = sender as GridView;

            if (grid != null)
            {


                //Check row state of gridview whether it is data row or not
                if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
                {
                    //if (ddlNature.SelectedValue != "Equity" && ddlNature.SelectedValue != "Debt" && ddlNature.SelectedValue != "Liquid")//&& ddlNature.SelectedValue != "Debt"
                    //{
                    //    //Now set the visibility of cell we want to hide to false 
                    //    e.Row.Cells[grid.Columns.Count - 1].Visible = false;
                    //    e.Row.Cells[grid.Columns.Count - 2].Visible = false;
                    //}
                    if (allotLess3Year())
                    {
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[4].Visible = false;
                        if (grid.Columns.Count > 4)
                            e.Row.Cells[6].Visible = false;
                    }
                }
            }
        }

        protected void gv_DataBound(object sender, EventArgs e)
        {
            GridView grid = sender as GridView;
            if (grid != null)
            {
                GridViewRow row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                row.CssClass = "headerCss";
                TableCell left = new TableHeaderCell();
                left.ColumnSpan = 1; row.Cells.Add(left);
                TableCell pscheme = new TableHeaderCell();
                if (allotLess3Year())
                    pscheme.ColumnSpan = 1;
                else
                    pscheme.ColumnSpan = 2;
                pscheme.Text = "Performance of " + ddlSchemeList.SelectedValue; row.Cells.Add(pscheme);
                TableCell pbench = new TableHeaderCell();
                if (allotLess3Year())
                    pbench.ColumnSpan = 1;
                else
                    pbench.ColumnSpan = 2;
                pbench.Text = "Performance of " + ddBenchMark.SelectedValue; row.Cells.Add(pbench);
                //if (ddlNature.SelectedValue == "Equity" || ddlNature.SelectedValue == "Debt" || ddlNature.SelectedValue == "Liquid")
                //{
                TableCell pbenchaddl = new TableHeaderCell();
                if (allotLess3Year())
                    pbenchaddl.ColumnSpan = 1;
                else
                    pbenchaddl.ColumnSpan = 2;
                pbenchaddl.Text = "Performance of ";

                if (ddlNature.SelectedValue == "Equity" || ddlNature.SelectedValue == "Balanced" || ddlNature.SelectedValue == "Dynamic/Asset Allocation")
                {
                    pbenchaddl.Text += ddBenchMark.SelectedValue == "Nifty 50" ? "S&P BSE Sensex" : "Nifty 50"; row.Cells.Add(pbenchaddl);
                }
                else
                {
                    //if (ddlNature.SelectedValue == "Debt")
                    //{
                    if (SubNatureId == "13" || ddlNature.SelectedValue.ToLower() == "liquid")
                        pbenchaddl.Text += "CRISIL 1 Year T-Bill Index";
                    else
                        pbenchaddl.Text += "CRISIL 10 Year Gilt Index";
                    row.Cells.Add(pbenchaddl);
                }

                //}
                //TableCell totals = new TableHeaderCell();
                //totals.ColumnSpan = grid.Columns.Count - 2;
                //totals.Text = "right"; row.Cells.Add(totals);
                Table t = grid.Controls[0] as Table;
                if (t != null)
                {
                    t.Rows.AddAt(0, row);
                }
            }
        }
        #endregion

        //public string SentenceCase(string inputstr)
        //{
        //    string[] strarray = inputstr.Split(' ');
        //    string finalstr = "";
        //    foreach (string str in strarray)
        //    {
        //        finalstr += str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
        //        finalstr += " ";
        //    }
        //    return finalstr.TrimEnd(' ');
        //}


    }

    #region : Class Definiton
    public class CalcReturnData
    {
        public decimal ID { get; set; }
        public string SCHEME { get; set; }
        public double? CURRENT_NAV { get; set; }
        public double? TOTAL_UNIT { get; set; }
        public double? TOTAL_AMOUNT { get; set; }
        public double? PRESENT_VALUE { get; set; }
        public double? YIELD { get; set; }
        public double? PROFIT_SIP { get; set; }
        public double? PROFIT_ONETIME { get; set; }
    }

    public class CalcReturnData2
    {
        public decimal ID { get; set; }
        public decimal SCHEME_ID { get; set; }
        public string SCHEME_NAME { get; set; }
        public string NAV_DATE { get; set; }
        public double NAV { get; set; }
        public double? INDEX_VALUE { get; set; }
        public double? SCHEME_UNITS { get; set; }
        public double? INDEX_UNIT { get; set; }
        public double? SCHEME_CASHFLOW { get; set; }
        public double? INDEX_CASHFLOW { get; set; }
        public double? CUMULATIVE_AMOUNT { get; set; }
        public double? AMOUNT_ONETIME { get; set; }
        public double? SCHEME_UNIT_ONETIME { get; set; }
        public double? INDEX_UNIT_ONETIME { get; set; }
        public string DIVIDEND_BONUS { get; set; }
    }

    public class SchemeSpReturn
    {
        public decimal SCHEME_ID { get; set; }
        public string SCHEME_NAME { get; set; }
        public char Scince_Inception { get; set; }
    }

    public class IndexSpReturn
    {
        public decimal INDEX_ID { get; set; }
        public string INDEX_NAME { get; set; }
        public double? Index_type { get; set; }
    }

    #endregion



}

//namespace iFrames.DAL
//{
//    public partial class PrincipalCalcDataContext : System.Data.Linq.DataContext
//    {
//        [Function(Name = "dbo.MFIE_SP_SIP_CALCULATER")]
//        [ResultType(typeof(CalcReturnData2))]
//        [ResultType(typeof(CalcReturnData))]
//        //public IMultipleResults MFIE_SP_SIP_CALCULATER([Parameter(Name = "Scheme_Ids", DbType = "VarChar(MAX)")] string scheme_Ids, [Parameter(Name = "Plan_Start_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_Start_Date, [Parameter(Name = "Plan_End_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_End_Date, [Parameter(Name = "Report_As_On", DbType = "DateTime")] System.Nullable<System.DateTime> report_As_On, [Parameter(Name = "SIP_Amt", DbType = "Float")] System.Nullable<double> sIP_Amt, [Parameter(Name = "Frequency", DbType = "VarChar(50)")] string frequency, [Parameter(Name = "Dividend_Type", DbType = "VarChar(50)")] string dividend_Type, [Parameter(Name = "Initial_Flage", DbType = "Int")] System.Nullable<int> initial_Flage, [Parameter(Name = "Initial_Amount", DbType = "Float")] System.Nullable<double> initial_Amount, [Parameter(Name = "Initial_Date", DbType = "DateTime")] System.Nullable<System.DateTime> initial_Date, [Parameter(Name = "Index_Flage", DbType = "Int")] System.Nullable<int> index_Flage)
//        //{
//        //    IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), scheme_Ids, plan_Start_Date, plan_End_Date, report_As_On, sIP_Amt, frequency, dividend_Type, initial_Flage, initial_Amount, initial_Date, index_Flage);
//        //    return ((IMultipleResults)(result.ReturnValue));
//        //}

//        //[Function(Name = "dbo.MFIE_SP_INDEX_P2P_ROLLING_RETURN")]
//        //[ResultType(typeof(IndexSpReturn))]
//        //public IMultipleResults MFIE_SP_INDEX_P2P_ROLLING_RETURN([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "INDEXIDS", DbType = "VarChar(MAX)")] string iNDEXIDS, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "SETTINGSETID", DbType = "Decimal(18,0)")] System.Nullable<decimal> sETTINGSETID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "DATEFROM", DbType = "DateTime")] System.Nullable<System.DateTime> dATEFROM, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "DATETO", DbType = "DateTime")] System.Nullable<System.DateTime> dATETO, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ROUNDTILL", DbType = "Int")] System.Nullable<int> rOUNDTILL, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "INDXROLLINGPERIODIN", DbType = "VarChar(500)")] string iNDXROLLINGPERIODIN, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "INDXROLLINGPERIOD", DbType = "Int")] ref System.Nullable<int> iNDXROLLINGPERIOD, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "INDXROLLINGFREQUENCYIN", DbType = "VarChar(10)")] string iNDXROLLINGFREQUENCYIN, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "INDXROLLINGFREQUENCY", DbType = "Int")] ref System.Nullable<int> iNDXROLLINGFREQUENCY, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ROLLING_P2P", DbType = "VarChar(50)")] string rOLLING_P2P, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "OTHERCALCULATION", DbType = "VarChar(3)")] string oTHERCALCULATION)
//        //{
//        //    IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), iNDEXIDS, sETTINGSETID, dATEFROM, dATETO, rOUNDTILL, iNDXROLLINGPERIODIN, iNDXROLLINGPERIOD, iNDXROLLINGFREQUENCYIN, iNDXROLLINGFREQUENCY, rOLLING_P2P, oTHERCALCULATION);
//        //    iNDXROLLINGPERIOD = ((System.Nullable<int>)(result.GetParameterValue(6)));
//        //    iNDXROLLINGFREQUENCY = ((System.Nullable<int>)(result.GetParameterValue(8)));

//        //    return ((IMultipleResults)(result.ReturnValue));
//        //}



//        //[Function(Name = "dbo.MFIE_SP_SCHEME_P2P_ROLLING_RETURN")]
//        //[ResultType(typeof(SchemeSpReturn))]
//        //public IMultipleResults MFIE_SP_SCHEME_P2P_ROLLING_RETURN([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "SCHEMEIDS", DbType = "VarChar(MAX)")] string sCHEMEIDS, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "SETTINGSETID", DbType = "Decimal(18,0)")] System.Nullable<decimal> sETTINGSETID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "DATEFROM", DbType = "DateTime")] System.Nullable<System.DateTime> dATEFROM, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "DATETO", DbType = "DateTime")] System.Nullable<System.DateTime> dATETO, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ROUNDTILL", DbType = "Int")] System.Nullable<int> rOUNDTILL, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ROLLINGPERIODIN", DbType = "VarChar(500)")] string rOLLINGPERIODIN, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ROLLINGPERIOD", DbType = "Int")] ref System.Nullable<int> rOLLINGPERIOD, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ROLLINGFREQUENCYIN", DbType = "VarChar(10)")] string rOLLINGFREQUENCYIN, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ROLLINGFREQUENCY", DbType = "Int")] ref System.Nullable<int> rOLLINGFREQUENCY, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "ROLLING_P2P", DbType = "VarChar(50)")] string rOLLING_P2P, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "OTHERCALCULATION", DbType = "VarChar(3)")] string oTHERCALCULATION)
//        //{
//        //    IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), sCHEMEIDS, sETTINGSETID, dATEFROM, dATETO, rOUNDTILL, rOLLINGPERIODIN, rOLLINGPERIOD, rOLLINGFREQUENCYIN, rOLLINGFREQUENCY, rOLLING_P2P, oTHERCALCULATION);
//        //    rOLLINGPERIOD = ((System.Nullable<int>)(result.GetParameterValue(6)));
//        //    rOLLINGFREQUENCY = ((System.Nullable<int>)(result.GetParameterValue(8)));


//        //    return ((IMultipleResults)(result.ReturnValue));
//        //}

//    }
//}