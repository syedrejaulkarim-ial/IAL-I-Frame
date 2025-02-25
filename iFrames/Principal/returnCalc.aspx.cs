using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using iFrames.DAL;
using System.Globalization;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Data.Linq;
using iFrames.Kotak;
using System.Data.Linq.Mapping;
using System.Reflection;
using iFrames.Pages;
namespace iFrames.Principal
{
    public partial class returnCalc : System.Web.UI.Page
    {
        #region Global declaration

        string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        DataTable finalResultdt = new DataTable();
        DateTime allotmentdate = new DateTime();
        string SubNatureId = string.Empty;

        #endregion

        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            this.sipbtnshow.Attributes.Add("onclick", "javascript:return validatePrincipal();");
            if (!IsPostBack)
            {
                callRelativeCalc();
                load_nature();
                FillDropdownScheme();
            }

            divshowChart.Visible = false;
            SetInceptionOnDropDown();
        }

        #endregion

        #region Button Event

        protected void sipbtnshow_Click(object sender, ImageClickEventArgs e)
        {
            RptCommonSipResult.Visible = false;
            CalculateReturn();
        }

        protected void sipbtnreset_Click(object sender, ImageClickEventArgs e)
        {
            resultDiv.Visible = false;
            Reset();
        }

        #endregion

        #region: DropDown Method

        protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            resultDiv.Visible = false;
            ShowRelativeDiv();
           
        }

        protected void ddlscname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (resultDiv.Visible) resultDiv.Visible = false;
            if (TableNote.Visible) TableNote.Visible = false;
            SWPSchDt.Text = "";

            FillDropdownIndex();
            FillFundManager();

            #region Launch Date
            using (var principl = new PrincipalCalcDataContext())
            {
                string schmeId = ddlscname.SelectedItem.Value;
                var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                select new
                                {
                                    LaunchDate = ind.Launch_Date
                                };
                //SIPSchDt.Text = "";
                if (allotdate != null && allotdate.Count() > 0)
                {
                    allotmentdate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                    SWPSchDt.Text = "<b>" + Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "<b/>";
                }
            }
            #endregion
        }

        protected void ddlNature_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropdownScheme();
            ddlbnmark.Items.Clear();
            FundmanegerText.Text = "";

            if (resultDiv.Visible) resultDiv.Visible = false;
            if (TableNote.Visible) TableNote.Visible = false;
        }

        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropdownScheme();
            ddlbnmark.Items.Clear();
            FundmanegerText.Text = "";

            if (resultDiv.Visible) resultDiv.Visible = false;
            if (TableNote.Visible) TableNote.Visible = false;
        }


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
                row.CssClass = "gridheader";
                // row.BorderWidth = Unit.Parse("1");
                TableCell left = new TableHeaderCell();
                left.CssClass = "gridtd";

                left.ColumnSpan = 1; row.Cells.Add(left);
                TableCell pscheme = new TableHeaderCell();
                if (allotLess3Year())
                    pscheme.ColumnSpan = 1;
                else
                    pscheme.ColumnSpan = 2;
                pscheme.CssClass = "gridtd";
                pscheme.Text = "Performance of " + ddlscname.SelectedItem.Text; row.Cells.Add(pscheme);
                TableCell pbench = new TableHeaderCell();
                if (allotLess3Year())
                    pbench.ColumnSpan = 1;
                else
                    pbench.ColumnSpan = 2;
                pbench.CssClass = "gridtd";
                pbench.Text = "Performance of " + ddlbnmark.SelectedItem.Text; row.Cells.Add(pbench);

                TableCell pbenchaddl = new TableHeaderCell();
                if (allotLess3Year())
                    pbenchaddl.ColumnSpan = 1;
                else
                    pbenchaddl.ColumnSpan = 2;
                pbenchaddl.Text = "Performance of ";
                pbenchaddl.CssClass = "gridtd";

                if (ddlNature.SelectedValue == "Equity" || ddlNature.SelectedValue == "Balanced" || ddlNature.SelectedValue == "Dynamic/Asset Allocation")
                {
                    pbenchaddl.Text += ddlbnmark.SelectedItem.Text == "Nifty 50" ? "S&P BSE Sensex" : "Nifty 50"; row.Cells.Add(pbenchaddl);
                }
                else
                {

                    if (SubNatureId == "13" || ddlNature.SelectedValue.ToLower() == "liquid")
                        pbenchaddl.Text += "CRISIL 1 Year T-Bill Index";
                    else
                        pbenchaddl.Text += "CRISIL 10 Year Gilt Index";
                    row.Cells.Add(pbenchaddl);
                }


                Table t = grid.Controls[0] as Table;
                if (t != null)
                {
                    t.Rows.AddAt(0, row);
                }
            }
        }
        #endregion

        #endregion

        #region : Fill method

        public void FillDropdown(Control ddl)
        {
            try
            {
                DataTable dt = FetchScheme();
                if (dt.Rows.Count > 0)
                {
                    DropDownList drpdwn = (DropDownList)ddl;
                    drpdwn.DataSource = dt;
                    Dictionary<string, string> SchemeInception = new Dictionary<string, string>();
                    drpdwn.Items.Clear();
                    drpdwn.DataTextField = "Sch_Short_Name";
                    drpdwn.DataValueField = "Scheme_Id";
                    //int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem li = new ListItem(dr["Sch_Short_Name"].ToString(), dr["Scheme_Id"].ToString());
                        li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        SchemeInception.Add(dr["Scheme_Id"].ToString(), dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        drpdwn.Items.Add(li);
                    }
                    // drpdwn.DataBind();
                    ViewState["SchemeInception"] = SchemeInception;
                    drpdwn.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
                    drpdwn.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void FillDropdownIndex()
        {
            FillDropdownIndex(ddlbnmark);
        }
        public void FillDropdownIndex(Control ddl)
        {
            try
            {
                DataTable dt = FetchBenchMark(Convert.ToDecimal(ddlscname.SelectedItem.Value));
                if (dt.Rows.Count > 0)
                {
                    DropDownList drpdwn = (DropDownList)ddl;
                    //drpdwn.DataSource = dt;
                    drpdwn.Items.Clear();
                    drpdwn.DataTextField = "INDEX_NAME";
                    drpdwn.DataValueField = "INDEX_ID";
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem li = new ListItem(dr["INDEX_NAME"].ToString(), dr["INDEX_ID"].ToString());
                        ViewState["INDEX_ID"] = dr["INDEX_ID"].ToString();
                        ViewState["INDEX_NAME"] = dr["INDEX_NAME"].ToString();
                        // li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        drpdwn.Items.Add(li);
                    }



                    //drpdwn.DataBind();
                    //drpdwn.Items.Insert(0, new ListItem("-Select Index-", "0"));
                    // drpdwn.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void FillDropdownScheme()
        {
            FillDropdown(ddlscname);
        }

        #endregion

        #region Method


        #region: Fetch Methods

        public DataTable FetchScheme()
        {
            //conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            DateTime yearBacktodaysdate = DateTime.Today.AddYears(-1);
            try
            {

                using (var scheme = new PrincipalCalcDataContext())
                {
                    DataTable dtt = null; DataTable dt2 = null;

                    if (ddlNature.SelectedIndex == 0)
                    {
                        var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                         where
                                           t_fund_masters.MUTUALFUND_ID == 15 && t_fund_masters.SUB_NATURE_ID != 2
                                         select new
                                         {
                                             t_fund_masters.FUND_ID
                                         });

                        if (fundtable.Count() > 0)
                            dtt = fundtable.ToDataTable();


                        if (ddlOption.SelectedIndex == 0)
                        {
                            var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                                 join T in fundtable
                                                 on s.Fund_Id equals T.FUND_ID
                                                 where  //s.T_FUND_MASTER.SUB_NATURE_ID != 2 && 
                                                 s.Nav_Check == 3  // for growth option & DSP BlackRock Equity Fund added additionaly
                                                     //&& s.Option_Id == 2
                                                 && s.Launch_Date <= yearBacktodaysdate
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date
                                                 }).Distinct();

                            if (scheme_name_1.Count() > 0)
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                        }
                        else
                        {
                            var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                                 join T in fundtable
                                                 on s.Fund_Id equals T.FUND_ID
                                                 where s.T_FUND_MASTER.SUB_NATURE_ID != 2 && s.Nav_Check == 3 && s.Option_Id.ToString() == ddlOption.SelectedItem.Value
                                                     // && (s.Launch_Date <= yearBacktodaysdate)
                                                 && s.Launch_Date <= yearBacktodaysdate
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date
                                                 }).Distinct();
                            if (scheme_name_1.Count() > 0)
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                        }
                    }
                    else
                    {

                        var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                         where
                                             t_fund_masters.MUTUALFUND_ID == 15 &&
                                         t_fund_masters.NATURE_ID ==
                                             ((from t_schemes_natures in scheme.T_SCHEMES_NATUREs
                                               where
                                                   t_schemes_natures.Nature == ddlNature.SelectedItem.Value
                                               select new
                                               {
                                                   t_schemes_natures.Nature_ID
                                               }).First().Nature_ID)
                                         select new
                                         {
                                             t_fund_masters.FUND_ID
                                         });

                        if (fundtable.Count() > 0)
                            dtt = fundtable.ToDataTable();


                        if (ddlOption.SelectedValue != "--")
                        {
                            var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                                 join T in fundtable
                                                 on s.Fund_Id equals T.FUND_ID
                                                 where s.T_FUND_MASTER.SUB_NATURE_ID != 2 && s.Nav_Check == 3 && s.Option_Id.ToString() == ddlOption.SelectedItem.Value
                                                 && (s.Launch_Date <= yearBacktodaysdate)
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date
                                                 }).Distinct();
                            if (scheme_name_1.Count() > 0)
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();

                        }
                        else
                        {
                            var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTERs
                                                 join T in fundtable
                                                 on s.Fund_Id equals T.FUND_ID
                                                 where s.Nav_Check == 3
                                                 && s.Launch_Date <= yearBacktodaysdate
                                                 join tsi in scheme.T_SCHEMES_INDEXes
                                                 on s.Scheme_Id equals tsi.SCHEME_ID
                                                 orderby s.Sch_Short_Name
                                                 select new
                                                 {
                                                     s.Sch_Short_Name,
                                                     s.Scheme_Id,
                                                     s.Launch_Date
                                                 }).Distinct();
                            if (scheme_name_1.Count() > 0)
                                dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                        }





                    }

                    dt = dt2.Copy();

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public System.Data.DataTable FetchBenchMark(decimal schid)
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            try
            {

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
                                                    t_schemes_masters.Scheme_Id == schid
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
                                          t_index_masters.INDEX_NAME,
                                          t_index_masters.INDEX_ID
                                      }).ToDataTable();

                    dt = index_name.Copy();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        #endregion

        protected void load_nature()
        {

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
                        ddlNature.Items.Clear();
                        ddlNature.Items.Add(new ListItem("--", "--"));
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

        protected void FillFundManager()
        {
            string selected_Scheme = ddlscname.SelectedItem.Value;

            try
            {
                using (var FundmanagerData = new PrincipalCalcDataContext())
                {

                    if (selected_Scheme != "--")
                    {

                        var FundManager = (from fd in FundmanagerData.T_FUND_MANAGERs
                                           join
                                        cfm in FundmanagerData.T_CURRENT_FUND_MANAGERs on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                           join
                                           fms in FundmanagerData.T_FUND_MASTERs on cfm.FUND_ID equals fms.FUND_ID
                                           join
                                           sm in FundmanagerData.T_SCHEMES_MASTERs on fms.FUND_ID equals sm.Fund_Id
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
                    else
                    {
                        FundmanegerText.Text = "";
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

        public void CalculateReturn()
        {

            if (ddlMode.SelectedItem.Text.ToUpper() == "SIP")
            {

                SipCalc();
            }
            else if (ddlMode.SelectedItem.Text.ToUpper() == "LUMPSUM")
            {
                LumpCalc();
            }
            resultDiv.Visible = true;
        }

        protected void SipCalc()
        {

            DataTable SipDtable1 = null;
            DataTable SipDtable2 = null;
            DateTime allotDate;
            DateTime _toDate, _fromDate, _asOnDate;
            string strSchid = string.Empty, strIndexid = string.Empty;
            DataTable datatTbleChart = new DataTable();
            //System.DateTime dtInitialDate = default(System.DateTime);

            try
            {

                _fromDate = new DateTime(Convert.ToInt16(txtstartDate.Text.Split('/')[2]),
                                           Convert.ToInt16(txtstartDate.Text.Split('/')[1]),
                                           Convert.ToInt16(txtstartDate.Text.Split('/')[0]));
                _toDate = new DateTime(Convert.ToInt16(txtendDate.Text.Split('/')[2]),
                                           Convert.ToInt16(txtendDate.Text.Split('/')[1]),
                                           Convert.ToInt16(txtendDate.Text.Split('/')[0]));
                _asOnDate = new DateTime(Convert.ToInt16(txtvalDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtvalDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtvalDate.Text.Split('/')[0]));

                using (var principl = new PrincipalCalcDataContext())
                {
                    var scheme_id = from prin in principl.T_SCHEMES_MASTERs
                                    where prin.Sch_Short_Name == ddlscname.SelectedItem.Text
                                    select prin.Scheme_Id;
                    strSchid = scheme_id.Single().ToString(); //scheme id
                    Session["scheme_id"] = strSchid;

                    var indx_id = from ppl in principl.T_INDEX_MASTERs
                                  where ppl.INDEX_NAME == ddlbnmark.SelectedItem.Text
                                  select ppl.INDEX_ID;

                    strIndexid = indx_id.Single().ToString();


                    var alotdate = from ind in principl.T_SCHEMES_MASTERs
                                   where ind.Sch_Short_Name == Convert.ToString(ddlscname.SelectedItem.Text)
                                   select ind.Launch_Date;

                    allotDate = Convert.ToDateTime(alotdate.Single().ToString());
                }

                double dblSIPamt = Convert.ToDouble(txtsipAmount.Text);// sip amount


                int strInitialDate = Convert.ToInt32(ddSIPdate.SelectedItem.Value);  // intial date
                string strFrequency = ddperiod.SelectedItem.Text;
                string strInvestorType = "Individual/Huf";

                /** change start date***/
                int _sdate, _smonth, _syear;

                if (Convert.ToInt32(txtstartDate.Text.Split('/')[0]) <= strInitialDate)
                {
                    _sdate = strInitialDate;
                    _smonth = Convert.ToInt32(txtstartDate.Text.Split('/')[1]);
                    _syear = Convert.ToInt32(txtstartDate.Text.Split('/')[2]);

                }
                else
                {

                    _sdate = strInitialDate;
                    _smonth = Convert.ToInt32(txtstartDate.Text.Split('/')[1]);
                    _smonth = _smonth + 1;
                    if (Convert.ToInt32(txtstartDate.Text.Split('/')[0]) == 31 && Convert.ToInt32(txtstartDate.Text.Split('/')[1]) == 1)
                        _sdate = 5;
                    if (_smonth > 12)
                    {
                        _syear = Convert.ToInt32(txtstartDate.Text.Split('/')[2]) + 1;
                        _smonth = 1;

                    }
                    else
                        _syear = Convert.ToInt32(txtstartDate.Text.Split('/')[2]);
                }

                // special case 

                DateTime _fromDatemodified = new DateTime(_syear, _smonth, _sdate);


                ViewState["fromDatemodified"] = _fromDatemodified;



                TimeSpan dydiff = _fromDatemodified.Subtract(_toDate);
                //if (dydiff.Days >= 1)
                //{
                //   // lbmsg.Text = "Your SIP Start Date is " + _fromDatemodified.ToString("dd/MM/yyyy") + " which must be greater than or equal to the End Date";
                //  //  if (!trSIP.Visible) trSIP.Visible = true;
                //    return;
                //}




                /***** using SQL TO LINQ *****/
                using (var principl = new PrincipalCalcDataContext())
                {

                    IMultipleResults datatble = principl.MFIE_SP_SIP_CALCULATER(strSchid, _fromDatemodified, _toDate, _asOnDate, dblSIPamt, strFrequency, strInvestorType, 0, 0, null, 1);
                    var firstTable = datatble.GetResult<CalcReturnData2>();
                    ////firstTable.TableName = "sdfsaf";
                    var secondTable = datatble.GetResult<CalcReturnData>();
                    SipDtable1 = firstTable.ToDataTable();
                    SipDtable2 = secondTable.ToDataTable();

                    datatTbleChart = SipDtable1.Copy();



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

                    //  Show_Sip_tb.Rows.Add(SipDtable2.Rows[0][3].ToString(), Convert.ToDouble(SipDtable2.Rows[0][2]).ToString("n2"), Convert.ToDouble(SipDtable2.Rows[0][4]).ToString("n2"));
                    Show_Sip_tb.Rows.Add(Convert.ToDouble(SipDtable2.Rows[0]["TOTAL AMOUNT"]).ToString("n2"), Convert.ToDouble(SipDtable2.Rows[0]["TOTAL UNIT"]).ToString("n2"), Convert.ToDouble(SipDtable2.Rows[0]["PRESENT VALUE"]).ToString("n2"));

                    SPGridViewtbl1.Columns[2].HeaderText = "Value as of Date " + "( " + txtvalDate.Text + " )";
                    SPGridViewtbl1.DataSource = Show_Sip_tb;
                    SPGridViewtbl1.DataBind();
                    SPGridViewtbl1.Visible = true;

                    if (SipDtable1 != null)
                        SipResultGrid.DataSource = dummySipDtable1;
                    else
                        SipResultGrid.DataSource = null;

                    SipResultGrid.DataBind();
                    SipResultGrid.Visible = true;
                    divSipResultGrid.Visible = true;


                    #region: Chart calculation



                    // datatTble = 
                    #region add extra column

                    if (datatTbleChart.Rows.Count > 2)
                    {
                        datatTbleChart.Rows.RemoveAt(datatTbleChart.Rows.Count - 1);
                        datatTbleChart.Rows.RemoveAt(datatTbleChart.Rows.Count - 1);
                    }


                    if (datatTbleChart.Rows.Count > 0)
                    {
                        if (datatTbleChart.Rows.Count > 2)
                        {
                            DataColumn dc = new DataColumn("Amount", System.Type.GetType("System.Double"));
                            datatTbleChart.Columns.Add(dc);
                            for (int i = 0; i < datatTbleChart.Rows.Count - 1; i++)
                            {

                                if (i == 0)
                                {
                                    datatTbleChart.Rows[i]["Amount"] = (-1) * Convert.ToDouble(datatTbleChart.Rows[i]["Scheme_cashflow"]);
                                }
                                else
                                {
                                    datatTbleChart.Rows[i]["Amount"] = Convert.ToDouble(datatTbleChart.Rows[i - 1]["Amount"]) + (-1) * Convert.ToDouble(datatTbleChart.Rows[i]["Scheme_cashflow"]);
                                }
                            }
                        }
                    }


                    #endregion

                    #region : Remove Columns
                    for (int col = datatTbleChart.Columns.Count - 1; col >= 0; col--)
                    {
                        DataColumn dc = datatTbleChart.Columns[col];
                        if (dc.ColumnName.ToUpper() != "NAV_DATE" && dc.ColumnName.ToUpper() != "CUMULATIVE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT")
                        {
                            datatTbleChart.Columns.RemoveAt(col);
                        }
                    }

                    #endregion
                    BindDataTableToChart(datatTbleChart, "NAV_DATE", chrtResult);
                   
                    if (CheckBoxChart.Checked)
                        divshowChart.Style.Add("display", "inline");
                    else
                        divshowChart.Style.Add("display", "none");

                    divshowChart.Visible = true;

                    #endregion
                    string StrNature = "";
                    string Option = "";
                    decimal? SubNatureId;
                    using (var DBPrin = new PrincipalCalcDataContext())
                    {
                        var ObjSch = DBPrin.T_SCHEMES_MASTERs.Where(x => x.Scheme_Id == Convert.ToInt32(ddlscname.SelectedValue))
                             .Select(x => x).FirstOrDefault();

                        StrNature = ObjSch.T_FUND_MASTER.T_SCHEMES_NATURE.Nature;
                        Option = ObjSch.T_SCHEMES_OPTION.Option_Name;
                        SubNatureId = ObjSch.Sub_Nature2_Id;

                    }
                    if (StrNature == "Balanced" || StrNature == "Equity" || StrNature == "Dynamic/Asset Allocation")
                    {
                        DateTime d = DateTime.Now;
                        DateTime lastDayOfLastQuarter = d.AddMonths(-((d.Month - 1) % 3)).AddDays(-d.Day);

                        if (allotDate.AddYears(1) <= lastDayOfLastQuarter)
                        {
                            if ((SubNatureId == 40) && (Option.ToUpper() == "GROWTH"))
                            {
                                PerformanceSipReturn(strSchid);
                            }
                            else
                            {
                                PerformanceReturn(strSchid, strIndexid, allotDate, 10000);
                            }
                        }
                        else
                        {
                            PerformanceReturn(strSchid, strIndexid, allotDate, 10000);
                        }
                    }
                    else
                    {
                        PerformanceReturn(strSchid, strIndexid, allotDate, 10000);
                    }
                }
            }
            catch (Exception ex)
            {
                string temp= ex.Message;
            }
            finally
            {

            }



            //ResetFormControlValues(this);
        }

        protected void LumpCalc()
        {

            double? daysDiffVal = null;
            DateTime _toDateLs, _fromDateLs;

            string strSchid = string.Empty;
            string strIndexid = string.Empty;
            conn.ConnectionString = connstr;

            //DateTime navDate = Convert.ToDateTime(ViewState["navDate"]);
            DateTime allotDate;//= Convert.ToDateTime(ViewState["alotedate"]);
            _fromDateLs = new DateTime(Convert.ToInt16(LumpStartDate.Text.Split('/')[2]),
                                     Convert.ToInt16(LumpStartDate.Text.Split('/')[1]),
                                     Convert.ToInt16(LumpStartDate.Text.Split('/')[0]));
            _toDateLs = new DateTime(Convert.ToInt16(LumpEndDate.Text.Split('/')[2]),
                                     Convert.ToInt16(LumpEndDate.Text.Split('/')[1]),
                                     Convert.ToInt16(LumpEndDate.Text.Split('/')[0]));
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
                                    where prin.Sch_Short_Name == ddlscname.SelectedItem.Text
                                    select prin.Scheme_Id;
                    strSchid = scheme_id.Single().ToString(); //scheme id
                    Session["scheme_id"] = strSchid;

                    var indx_id = from ppl in principl.T_INDEX_MASTERs
                                  where ppl.INDEX_NAME == ddlbnmark.SelectedItem.Text
                                  select ppl.INDEX_ID;

                    strIndexid = indx_id.Single().ToString();


                    var alotdate = from ind in principl.T_SCHEMES_MASTERs
                                   where ind.Sch_Short_Name == Convert.ToString(ddlscname.SelectedItem.Text)
                                   select ind.Launch_Date;

                    allotDate = Convert.ToDateTime(alotdate.Single().ToString());

                }


                try
                {
                    DataSet ds = new DataSet();
                    DataTable datatble = null;
                    //DataTable indexDataTable = null;
                    int val = 0;

                    SqlCommand cmd;

                    cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", conn);
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


                    LsGridViewtbl1.Columns[0].HeaderText = "Amount Invested " + "( " + LumpStartDate.Text + " )";
                    LsGridViewtbl1.Columns[1].HeaderText = "Value as of Date " + "( " + LumpEndDate.Text + " )";
                    LsGridViewtbl1.DataSource = Show_Ls_tb;
                    LsGridViewtbl1.DataBind();
                    LsGridViewtbl1.Visible = true;


                    allotmentdate = allotDate;
                    PerformanceReturn(strSchid, strIndexid, allotDate, Convert.ToInt32(amountLs));


                }
                catch (Exception ex)
                {
                    //string s = ex.ToString();
                    //lblerrmsg.Text = ex.Message;
                }
                finally
                {

                }
            }

            catch (Exception exp)
            {
                // lbmsg.Text = "Error!!! Can't produce report.";
            }
            finally
            {

            }
            if (trSIP.Visible == true)
                trSIP.Visible = false;

            //ResetFormControlValues(this);

        }

        private void PerformanceReturn(string schmeId, string indexId, DateTime allotDate, int amount)
        {

            DateTime LastQtrEndDate = GetLastQuarterDates();
            double? sinceInceptionIndex = null, sinceInceptionAddlIndex = null;
            double? sinceInception = null;
            DateTime LastQtrEndDateInRecord = LastQtrEndDate;//= null;
            SqlCommand sqlcmd, sqlcmd1;
            conn.ConnectionString = connstr;

            string datediff = string.Empty;
            string strNature = string.Empty;
            string strIndexid = string.Empty;


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

                var indx_id = from ppl in principl.T_INDEX_MASTERs
                              where ppl.INDEX_NAME == ddlbnmark.SelectedItem.Text
                              select ppl.INDEX_ID;

                strIndexid = indx_id.Single().ToString();

            }

            //monthDate
            strNature = ddlNature.SelectedValue;
            try
            {

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

                    sqlcmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", conn);
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





                    sqlcmd1 = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", conn);
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
                        if (nwIndexDataTble.Rows[k][0].ToString() == strIndexid && nwIndexDataTble.Rows[k][dday2 + " Day"].ToString() != "N/A")
                        {
                            benchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[k][dday2 + " Day"].ToString()));
                        }
                        if (nwIndexDataTble.Rows[k][0].ToString() == dcmlIndexid.ToString() && nwIndexDataTble.Rows[k][dday2 + " Day"].ToString() != "N/A")
                        {
                            adbenchval.Add(Convert.ToDouble(nwIndexDataTble.Rows[k][dday2 + " Day"].ToString()));
                        }
                    }



                }




                double[] navVal = schemval.ToArray();
                double[] indexnavVal = benchval.ToArray();
                double[] addlindexnavVal = adbenchval.ToArray();


                SqlCommand cmdd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN_PRINCIPAL", conn);
                cmdd.CommandType = CommandType.StoredProcedure;
                cmdd.CommandTimeout = 2000;
                cmdd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
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




                SqlCommand indexCmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN_PRINCIPAL", conn);
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



                //}                               

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
                    CommonResultGridView.Visible = true;
                }
                //TableNote.Text = "";

            }
            catch (Exception exc)
            {
                //lblerrmsg.Text = exc.Message;
            }
            if (TableNote.Visible == false) TableNote.Visible = true;

            //return perfDataTable;
        }

        private void PerformanceSipReturn(string schemeId)
        {
            var intSch = Convert.ToInt32(schemeId);


            using (var principl = new PrincipalCalcDataContext())
            {
                var DbRet = (from tfm in principl.RETURN_SIP_PRINCIPALs
                             where tfm.Scheme_ID == intSch

                             select tfm);
                if (DbRet.Any())
                {

                    var data = DbRet.ToDataTable();
                    RptCommonSipResult.DataSource = data;
                    RptCommonSipResult.DataBind();
                    RptCommonSipResult.Visible = true;
                    CommonResultGridView.Visible = false;
                    ((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnSch")).Text = Convert.ToString(data.Rows[0]["Scheme_name"]);
                    ((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnIndex")).Text = Convert.ToString(data.Rows[0]["Index_Name"]);
                    ((Label)RptCommonSipResult.Controls[0].Controls[0].FindControl("lblReturnAddIndex")).Text = Convert.ToString(data.Rows[0]["Additional_Index_Name"]);
                }
            }
            if (TableNote.Visible == false) TableNote.Visible = true;
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

        public void SetInceptionOnDropDown()
        {

            if ((ViewState["SchemeInception"] != null) && (ddlscname.Items.Count > 0))
            {

                Dictionary<string, string> SchemeInception = (Dictionary<string, string>)(ViewState["SchemeInception"]);
                for (int i = 0; i < ddlscname.Items.Count; i++)
                {
                    string s = "";
                    if (SchemeInception.TryGetValue(ddlscname.SelectedItem.Value, out s) && ddlscname.Items[i].Selected == true)
                    {
                        ddlscname.Items[i].Attributes.Add("title", s);
                    }
                }
            }
        }

        #region Rest Method

        protected void ShowRelativeDiv()
        {
            //if (ddlMode.SelectedValue == "LumpSum")
            //{
            //    Response.Redirect("http://mfiframes.mutualfundsindia.com/Pages/PrincipalCalc_Test.aspx?return=LUMPSUM");
            //}
            //if (ddlMode.SelectedValue == "SIP")
            //{
            //    Response.Redirect("http://mfiframes.mutualfundsindia.com/Pages/PrincipalCalc_Test.aspx?return=SIP");
            //}
            //if (ddlMode.SelectedValue == "SWP")
            //{
            //}
            //if (ddlMode.SelectedValue == "STP")
            //{
            //    Response.Redirect("http://mfiframes.mutualfundsindia.com/Principal/stpCalc.aspx");
            //}



            if (ddlMode.SelectedValue == "SIP")
            {
                trSIP.Visible = true; trlumpsum.Visible = false;
                LsGridViewtbl1.Visible = false;
                logocalc.Src = "img/sip.jpg";
            }
            else if (ddlMode.SelectedItem.Text.ToUpper() == "LUMPSUM")
            {
                trSIP.Visible = false; trlumpsum.Visible = true;
                SipResultGrid.Visible = false; divSipResultGrid.Visible = false; SPGridViewtbl1.Visible = false;
                logocalc.Src = "img/Lump-Sum.jpg";
            }
            else if (ddlMode.SelectedItem.Text.ToUpper() == "SWP")
            {
                Response.Redirect("swpCalc.aspx");
            }
            else if (ddlMode.SelectedItem.Text.ToUpper() == "STP")
            {
                Response.Redirect("stpCalc.aspx");
            }
            else
            {
            }

        }


        private void callRelativeCalc()
        {
            string calc = Request.QueryString["return"];
            if (calc != null)
            {
                if (!string.IsNullOrEmpty(calc))
                {
                    //ddlMode.SelectedItem.Selected = false;
                    //if(calc.ToUpper() =="SWP")
                    switch (calc.ToUpper())
                    {
                        case "SIP":
                            //ddlMode.Items[2].Selected = true;
                            ddlMode.SelectedIndex = 0;
                            break;
                        case "LUMPSUM":
                            //ddlMode.Items[1].Selected = true;
                            ddlMode.SelectedIndex = 1;
                            break;
                        default:
                            //ddlMode.Items[0].Selected = true;
                            ddlMode.SelectedIndex = 0;
                            break;
                    }

                }
            }
            ShowRelativeDiv();
        }

        public string TwoDecimal(string data)
        {
            string result = string.Empty;
            result = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(data))).ToString();
            return result;
        }

        public void Reset()
        {
            ddlscname.SelectedIndex = 0;
            ddperiod.SelectedIndex = 0;
            ddSIPdate.SelectedIndex = 0;
            txtsipAmount.Text = "";
            //txtwtramt.Text = "";
            txtstartDate.Text = "";
            txtendDate.Text = "";
            txtvalDate.Text = "";
            SWPSchDt.Text = "";
            TableNote.Visible = false;
        }


        #endregion


        #endregion

        #region : Chart

        public void BindDataTableToChart(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt)
        {
            string columnName = null;


            try
            {

                chrt.Series.Clear();

                List<string> columnList = new List<string>();
                columnList.Clear();

                #region :Add Date Column
                _dt.Columns.Add("DateStr", typeof(DateTime));
                foreach (DataRow dr in _dt.Rows)
                {
                    //dr["DateStr"] = Convert.ToDateTime(string.Format("{0:MM/dd/yyyy}", dr[xField]));
                    var arr = dr[xField].ToString().Split('/');
                    dr["DateStr"] = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[1]), Convert.ToInt32(arr[0]));
                }
                _dt.Columns.Remove(xField);
                xField = "DateStr";
                #endregion

                foreach (DataColumn dc in _dt.Columns)
                {
                    if (dc.ColumnName == xField)
                        continue;
                    chrt.Series.Add(dc.ColumnName);
                    columnList.Add(dc.ColumnName);
                    chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Spline;
                    columnName = dc.ColumnName;

                    if (dc.ColumnName.ToUpper() == "AMOUNT")
                    {
                        //chrt.Series[dc.ColumnName].YAxisType = AxisType.Primary;
                        chrt.Series[dc.ColumnName].LegendText = "INVESTMENT AMOUNT";
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
                        // chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#001F5C");                
                        // chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Solid;
                    }
                    else
                    {
                        //  chrt.Series[dc.ColumnName].YAxisType = AxisType.Secondary;
                        //chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");               
                        chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Blue;
                        chrt.Series[dc.ColumnName].LegendText = "CUMULATIVE AMOUNT";
                        // chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Dash;
                        // chrt.Series[dc.ColumnName].BorderWidth = 15;
                        //chrt.Series[dc.ColumnName].ShadowOffset = 8;                        
                    }

                    chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, columnName);
                    chrt.Series[columnName].IsValueShownAsLabel = false;
                    chrt.Series[columnName].BorderWidth = 1;


                }

                chrt.Series[0].XValueType = ChartValueType.DateTime;

                chrt.Visible = true;

                double? maxval = 1;
                double? minval = 10000;

                if (columnList.Count >= 2)
                {
                    maxval = _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[0])) >= _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[1])) ? _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[0])) : _dt.AsEnumerable().Max(x => x.Field<double?>(columnList[1]));

                    minval = _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[0])) <= _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[1])) ? _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[0])) : _dt.AsEnumerable().Min(x => x.Field<double?>(columnList[1]));


                    //maxval = _dt.AsEnumerable().Max(x => x.Field<double?>("AMOUNT")) >= _dt.AsEnumerable().Max(x => x.Field<double?>("CUMULATIVE_AMOUNT")) ? _dt.AsEnumerable().Max(x => x.Field<double?>("AMOUNT")) : _dt.AsEnumerable().Max(x => x.Field<double?>("CUMULATIVE_AMOUNT"));
                    //minval = _dt.AsEnumerable().Min(x => x.Field<double?>("AMOUNT")) <= _dt.AsEnumerable().Min(x => x.Field<double?>("CUMULATIVE_AMOUNT")) ? _dt.AsEnumerable().Min(x => x.Field<double?>("AMOUNT")) : _dt.AsEnumerable().Min(x => x.Field<double?>("CUMULATIVE_AMOUNT"));

                }

                chrt.ChartAreas[0].AxisY.Maximum = Math.Round((Math.Round(Convert.ToDouble(maxval), 0) + 500) / 10, 0) * 10;
                if (minval < 1000)
                    chrt.ChartAreas[0].AxisY.Minimum = 0;
                else
                    chrt.ChartAreas[0].AxisY.Minimum = Math.Round((Math.Round(Convert.ToDouble(minval), 0) - 500) / 10, 0) * 10;



                chrt.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chrt.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;

                chrt.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chrt.ChartAreas[0].AxisY.MajorGrid.Enabled = false;


                chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
                chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;
                chrt.ChartAreas[0].AxisY.LabelStyle.Format = "0000";


                chrt.ChartAreas[0].AxisY.Title = "Figure in Rs";
                chrt.ChartAreas[0].AxisX.Title = "Time Period";



                DateTime dtMax = (DateTime)_dt.AsEnumerable().Select(x => x[xField]).Max();
                DateTime dtMin = (DateTime)_dt.AsEnumerable().Select(x => x[xField]).Min();

                if (dtMax.Subtract(dtMin).Days > 365)
                {
                    chrt.ChartAreas[0].AxisX.Interval = 1;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy";
                }
                else
                {
                    chrt.ChartAreas[0].AxisX.Interval = 3;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yy";
                }


                chrt.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Horizontal;
                chrt.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.Chocolate;

                var chrtArea = chrt.ChartAreas[0];
                chrtArea.Visible = true;




                System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];

                legend.Font = new Font("Arial", 9, FontStyle.Bold);



            }
            catch (Exception ex)
            {
                Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");
            }

        }

        public void BindDataTableToChart(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt, string strCompareColumn)
        {
            string columnName = null;            
            string CompareToColumn = string.Empty;

            try
            {

                chrt.Series.Clear();

                #region :Add Date Column
                _dt.Columns.Add("DateStr", typeof(DateTime));
                foreach (DataRow dr in _dt.Rows)
                {
                    //dr["DateStr"] = Convert.ToDateTime(string.Format("{0:MM/dd/yyyy}", dr[xField]));
                    var arr = dr[xField].ToString().Split('/');
                    dr["DateStr"] = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[1]), Convert.ToInt32(arr[0]));
                }
                _dt.Columns.Remove(xField);
                xField = "DateStr";
                #endregion

                foreach (DataColumn dc in _dt.Columns)
                {
                    if (dc.ColumnName == xField)
                        continue;
                    chrt.Series.Add(dc.ColumnName);

                    chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Spline;
                    columnName = dc.ColumnName;

                    if (dc.ColumnName.ToUpper() == strCompareColumn)
                    {
                        chrt.Series[dc.ColumnName].Color = Color.Black;
                        chrt.Series[dc.ColumnName].LegendText = "Investment Amount";
                        chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Dash;
                        chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Spline;
                    }
                    else
                    {
                        CompareToColumn = dc.ColumnName.ToUpper();
                        chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Blue;
                        chrt.Series[dc.ColumnName].LegendText = "Investment Value";
                        chrt.Series[dc.ColumnName].BorderDashStyle = ChartDashStyle.Solid;
                        chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Spline;
                    }

                    chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, columnName);
                    chrt.Series[columnName].IsValueShownAsLabel = false;
                    chrt.Series[columnName].BorderWidth = 1;

                }

                if (ddlMode.SelectedItem.Value.ToUpper() == "SIP")
                {
                    chrt.Series["Amount"].LegendText = "Investment Amount";
                    chrt.Series[CompareToColumn].LegendText = "Investment Value";
                    if (ViewState["FundName"] != null)
                        chrt.Titles[0].Text = "SIP Performance Chart:" + Convert.ToString(ViewState["FundName"]);
                }


                chrt.Visible = true;
                chrtResult.Visible = true;

                double? maxval = 1;
                double? minval = 10000;

                //CompareToColumn
                maxval = _dt.AsEnumerable().Max(x => x.Field<double?>(CompareToColumn));
                //>= _dt.AsEnumerable().Max(x => x.Field<double?>("CUMULATIVE_AMOUNT")) ? _dt.AsEnumerable().Max(x => x.Field<double?>("AMOUNT")) : _dt.AsEnumerable().Max(x => x.Field<double?>("CUMULATIVE_AMOUNT"));



                if (_dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn)) != null)
                    maxval = maxval >= _dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn)) ? maxval : _dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn));

                minval = _dt.AsEnumerable().Min(x => x.Field<double?>(CompareToColumn));
                //<= _dt.AsEnumerable().Min(x => x.Field<double?>("CUMULATIVE_AMOUNT")) ? _dt.AsEnumerable().Min(x => x.Field<double?>("AMOUNT")) : _dt.AsEnumerable().Min(x => x.Field<double?>("CUMULATIVE_AMOUNT"));

                if (_dt.AsEnumerable().Max(x => x.Field<double?>(strCompareColumn)) != null)
                    minval = minval <= _dt.AsEnumerable().Min(x => x.Field<double?>(strCompareColumn)) ? minval : _dt.AsEnumerable().Min(x => x.Field<double?>(strCompareColumn));



                maxval = Math.Round(Convert.ToDouble(maxval), 0);
                minval = Math.Round(Convert.ToDouble(minval), 0);

                //maxval = RoundToNearest((double)maxval, 1000);
                //minval = RoundToNearest((double)minval, 1000);
                minval = minval - 1000;



                chrt.ChartAreas[0].AxisY.Maximum = Math.Round(Convert.ToDouble(maxval), 0) + 1000;
                if (minval < 1000)
                    chrt.ChartAreas[0].AxisY.Minimum = 0;
                else
                    chrt.ChartAreas[0].AxisY.Minimum = Math.Round(Convert.ToDouble(minval), 0) - 1000;



                chrt.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chrt.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;


                chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
                chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;

                chrt.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 12);
                chrt.ChartAreas[0].AxisY.Title = "Value(Rs)";
                chrt.ChartAreas[0].AxisY.LabelStyle.Format = "#,###";
                chrt.ChartAreas[0].AxisY.IsLabelAutoFit = true;
                chrt.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Horizontal;
                chrt.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 12);
                chrt.ChartAreas[0].AxisX.Title = "Time Period";
                // chrt.ChartAreas[0].AxisX.LabelStyle.Format = "yy";
                var chrtArea = chrt.ChartAreas[0];
                chrtArea.AxisX.MajorGrid.LineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.NotSet;
                chrtArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
                chrtArea.Visible = true;

                DateTime dtMax = (DateTime)_dt.AsEnumerable().Select(x => x[xField]).Max();
                DateTime dtMin = (DateTime)_dt.AsEnumerable().Select(x => x[xField]).Min();

                if (dtMax.Subtract(dtMin).Days > 365 * 3)
                {
                    chrt.ChartAreas[0].AxisX.Interval = 1;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy";
                }
                else if (dtMax.Subtract(dtMin).Days > 365)
                {
                    chrt.ChartAreas[0].AxisX.Interval = 6;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yy";
                }
                else
                {
                    chrt.ChartAreas[0].AxisX.Interval = 3;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yy";
                }

                System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];
                legend.Font = new Font("Arial", 9, FontStyle.Bold);



                #region Chart Image MyRegion

                string tmpChartName = "ChartImagetest.jpg";

                string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                string localImgPath = @"DSP\Img";
                localImgPath = System.IO.Path.Combine(appPath, localImgPath);

                string testpath = localImgPath;


                #region Delete MyRegion
                var allImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\IMG", "DSP_SIP_Temp*");
                foreach (var f in allImageFiles)
                    if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
                        File.Delete(f);


                #endregion

                #region : save Image


                //localImgPath = System.IO.Path.Combine(localImgPath, "SIP"+ Guid.NewGuid().ToString("N"));
                if (!Directory.Exists(localImgPath))
                {
                    //using (Directory.CreateDirectory(localImgPath))
                    //{                    }
                    Directory.CreateDirectory(localImgPath);
                }

                string imgPath = System.IO.Path.Combine(localImgPath, "DSP_SIP_Temp" + Guid.NewGuid().ToString("N") + "_" + tmpChartName);

                Session["imgPath"] = imgPath;
                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                }
                chrtResult.SaveImage(imgPath);
                #endregion



                #endregion




            }
            catch (Exception ex)
            {
                Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");
            }

        }


        #endregion




    }

}
