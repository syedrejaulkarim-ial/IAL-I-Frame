using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using iFrames.DAL;
using System.Data.Linq;
using iFrames.Pages;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
//using Microsoft.Office.Interop.Excel;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Linq.Mapping;
using System.Reflection;
using iFrames.Kotak;
using System.Web.SessionState;

namespace iFrames.Kotak
{
    public partial class KotakSIPCalc : System.Web.UI.Page
    {
        #region Global declaration

        string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        System.Data.DataTable finalResultdt = new System.Data.DataTable();
        System.Data.DataTable finalResultdtwobenchmark = new System.Data.DataTable();
        string imgPath = string.Empty;
        string tmpChartName = string.Empty;
        //bool btnclick = false;

        #endregion

        #region Page Event

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.sipbtnshow.Attributes.Add("onclick", "javascript:return validate_SIP();");
            if (!IsPostBack)
            {
                FillDropdownScheme();
            }
        }


        #endregion

        #region Click Event
        protected void btnSip_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtinstall.Text))
            {
                if (Convert.ToDouble(txtinstall.Text) >= 1000)
                {
                    ViewState["btnclick"] = false;
                    CalculateReturn();
                }
                else
                {
                    return;
                }
            }
            else
            {
                ViewState["btnclick"] = false;
                CalculateReturn();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            resultDiv.Visible = false;
            Reset();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        protected void imgbtnExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportToExcel();
        }


        #endregion

        #region: DropDown Method
        protected void ddlscheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SetIndexDropdown();
            //ListItem li = (ListItem)ViewState["ListItem"];
            FillDropdownIndex();
            if (resultDiv.Visible)
                resultDiv.Visible = false;
            //if (ddlsipbnmark.Visible)
            //    ddlsipbnmark.Visible = false;
        }

        public void FillDropdown(Control ddl)
        {
            try
            {
                DataTable dt = FetchScheme();
                if (dt.Rows.Count > 0)
                {
                    DropDownList drpdwn = (DropDownList)ddl;
                    //drpdwn.DataSource = dt;


                    Dictionary<string, string> SchemeInception = new Dictionary<string, string>();
                    drpdwn.Items.Clear();
                    drpdwn.DataTextField = "Sch_Short_Name";
                    drpdwn.DataValueField = "Scheme_Id";
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

                Response.Write(@"<script>alert('" + ex.Message + "')</script>");
            }
        }

        public void FillDropdownIndex(Control ddl)
        {
            try
            {
                DataTable dt = FetchBenchMark(Convert.ToDecimal(ddlscheme.SelectedItem.Value));
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
                    drpdwn.Items.Insert(0, new ListItem("-Select Index-", "0"));
                    drpdwn.Items[1].Selected = true;
                }
            }
            catch (Exception ex)
            {

                Response.Write(@"<script>alert('" + ex.Message + "')</script>");
            }
        }

        public void FillDropdownScheme()
        {
            FillDropdown(ddlscheme);
        }

        public void FillDropdownIndex()
        {
            FillDropdownIndex(ddlsipbnmark);
        }

        #endregion

        #region: Fetch Methods

        public System.Data.DataTable FetchScheme()
        {
            //conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            try
            {

                using (var scheme = new SIP_ClientDataContext())
                {
                    var fundtable = (from t_fund_masters in scheme.T_FUND_MASTER_clients
                                     where
                                       t_fund_masters.MUTUALFUND_ID == 37
                                     select new
                                     {
                                         t_fund_masters.FUND_ID
                                     });

                    DataTable dtt = null;
                    if (fundtable.Count() > 0)
                        dtt = fundtable.ToDataTable();

                    List<decimal> indexIdwithoutData = new List<decimal> { 1,29,32,65 };
                    List<decimal> schemeIdwithoutData = new List<decimal> { 12087,12088,12132,12133,12134 }; // on client request 29.08.2012
                    var scheme_name_1 = (from s in scheme.T_SCHEMES_MASTER_Clients
                                         join T in fundtable
                                         on s.Fund_Id equals T.FUND_ID
                                         where s.Nav_Check == 3//s.T_FUND_MASTER_client.SUB_NATURE_ID != 2 &&
                                         join tsi in scheme.T_SCHEMES_INDEX_clients
                                         on s.Scheme_Id equals tsi.SCHEME_ID
                                         where !indexIdwithoutData.Contains(tsi.INDEX_ID)
                                         && !schemeIdwithoutData.Contains(s.Scheme_Id)
                                         orderby s.Sch_Short_Name
                                         select new
                                         {
                                             s.Sch_Short_Name,
                                             s.Scheme_Id,
                                             s.Launch_Date
                                         }).Distinct();
                    DataTable dt2 = null;
                    if (scheme_name_1.Count() > 0)
                        dt2 = scheme_name_1.ToDataTable();

                    dt = dt2.Copy();

                }
            }
            catch (Exception ex)
            {
                Response.Write(@"<script>alert('" + ex.Message + "')</script>");
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

                using (var kotak = new SIP_ClientDataContext())
                {
                    var index_name = (from t_index_masters in kotak.T_INDEX_MASTER_Clients
                                      where
                                        t_index_masters.INDEX_ID ==
                                          ((from t_schemes_indexes in kotak.T_SCHEMES_INDEX_clients
                                            where
                                              t_schemes_indexes.SCHEME_ID ==
                                                ((from t_schemes_masters in kotak.T_SCHEMES_MASTER_Clients
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
                Response.Write(@"<script>alert('" + ex.Message + "')</script>");
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        #endregion

        # region Calculation
        public void CalculateReturn()
        {
            #region Initialize

            DateTime sipStartDate, sipEndDate, sipAsonDate, schmStartDate, tempDate, allotDate, investmentDate;
            string strsql = String.Empty, schmeId = string.Empty, indexId = string.Empty, Colstr = string.Empty, daydiffCol = string.Empty;
            string strFrequency, strInvestorType;
            TimeSpan tmspan;
            DataTable dt = new DataTable();
            DataSet dset = new DataSet();
            DataTable SipDtable1, SipDtable2;
            int PrdVal, daydiff, SIP_date = 1, tempInt;
            double dblSIPamt;
            SqlCommand cmdsip = null, cmd = null, cmdScheme = null, cmdIndex = null, cmdIndx = null;
            int val = 0, tstval = 1;

            conn.ConnectionString = connstr;


            #endregion


            try
            {
            //    sipStartDate = Convert.ToDateTime(txtfromDate.Text);
            //    sipEndDate = Convert.ToDateTime(txtToDate.Text);
            //    sipAsonDate = Convert.ToDateTime(txtvalason.Text);


                sipStartDate = new DateTime(Convert.ToInt16(txtfromDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtfromDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtfromDate.Text.Split('/')[0]));

                sipEndDate = new DateTime(Convert.ToInt16(txtToDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[0]));

                sipAsonDate = new DateTime(Convert.ToInt16(txtvalason.Text.Split('/')[2]),
                                     Convert.ToInt16(txtvalason.Text.Split('/')[1]),
                                     Convert.ToInt16(txtvalason.Text.Split('/')[0]));
              


                schmeId = ddlscheme.SelectedItem.Value;
                dblSIPamt = Convert.ToDouble(txtinstall.Text);

                if (ViewState["INDEX_ID"] != null)
                    indexId = ViewState["INDEX_ID"].ToString();
                else
                    indexId = ddlsipbnmark.SelectedItem.Value;

                #region Inception Date


                using (var kotak = new SIP_ClientDataContext())
                {
                    var alotdate = from ind in kotak.T_SCHEMES_MASTER_Clients
                                   where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                   select ind.Launch_Date;

                    schmStartDate = Convert.ToDateTime(alotdate.Single().ToString());

                    var lastAvailbleNavDate = (from ind in kotak.T_NAV_DIV_clients
                                               where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                               select ind.Nav_Date).Max();


                    if (lastAvailbleNavDate != null)
                    {
                        DateTime LastNavDate;
                        LastNavDate = Convert.ToDateTime(lastAvailbleNavDate);

                        tmspan = LastNavDate.Subtract(sipAsonDate);
                        if (tmspan.Days < 0)
                        {
                            //Response.Write(@"<script>alert(""Value as on date is not available for the scheme on " + sipAsonDate.ToShortDateString() + @".."")</script>");
                            sipAsonDate = LastNavDate;
                            //return;
                        }

                        tmspan = sipAsonDate.Subtract(sipEndDate);

                        if (tmspan.Days < 0)
                        {
                            sipEndDate = sipAsonDate;
                        }

                    }
                }

                tmspan = schmStartDate.Subtract(sipStartDate);
                if (tmspan.Days > 0)
                {
                    Response.Write(@"<script>alert(""From Date cannot be Greater than Inception Date of the scheme which is  " + schmStartDate.ToShortDateString() + @".."")</script>");
                    return;
                }
                #endregion

                #region Assigned value and Validation

                if (ddPeriod_SIP.SelectedItem.Text == "Monthly")
                {
                    PrdVal = 1;
                }
                else if (ddPeriod_SIP.SelectedItem.Text == "Quarterly")
                {
                    PrdVal = 3;
                }
                else
                {
                    return;
                }


                if (PrdVal == 1)
                {
                    tempDate = sipEndDate.AddMonths(-6);
                    tmspan = tempDate.Subtract(sipStartDate);
                    daydiff = tmspan.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"SIP is allowed for minimum 6 withdrawal\")</script>");
                        return;
                    }
                }
                else if (PrdVal == 3)
                {
                    tempDate = sipEndDate.AddMonths(-12);
                    tmspan = tempDate.Subtract(sipStartDate);
                    daydiff = tmspan.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"SIP is allowed for minimum 4 withdrawal\")</script>");
                        return;
                    }
                }


                switch (ddSIPdate.SelectedItem.Text)
                {
                    case "1st":
                        SIP_date = 1;
                        break;
                    case "7th":
                        SIP_date = 7;
                        break;
                    case "14th":
                        SIP_date = 14;
                        break;
                    case "21st":
                        SIP_date = 21;
                        break;
                    case "28th":
                        SIP_date = 28;
                        break;

                }

                dblSIPamt = Convert.ToDouble(txtinstall.Text);// sip amount

                strFrequency = ddPeriod_SIP.SelectedItem.Text;
                strInvestorType = "Individual/Huf";

                tempInt = sipStartDate.Day;
                if (SIP_date < tempInt)
                {
                    if (sipStartDate.Month != 12)
                        investmentDate = new DateTime(sipStartDate.Year, sipStartDate.Month + 1, SIP_date);
                    else
                        investmentDate = new DateTime(sipStartDate.Year + 1, 1, SIP_date);
                }
                else if (SIP_date == tempInt)
                {
                    investmentDate = sipStartDate;
                }
                else
                {
                    investmentDate = new DateTime(sipStartDate.Year, sipStartDate.Month, SIP_date);
                }

                #endregion


                using (var kotak = new SIP_ClientDataContext() { CommandTimeout = 600000 })
                {
                    IMultipleResults datatble = kotak.MFIE_SP_SIP_CALCULATER_CLIENT(schmeId, investmentDate, sipEndDate, sipAsonDate, dblSIPamt, strFrequency, strInvestorType, 0, 0, null, 1, "Y");
                    var firstTable = datatble.GetResult<CalcReturnDataClient2>();
                    var secondTable = datatble.GetResult<CalcReturnDataClient>();
                    SipDtable1 = firstTable.ToDataTable();
                    SipDtable2 = secondTable.ToDataTable();
                    if (SipDtable2.Columns.Count > 0)
                        SipDtable2.Columns.RemoveAt(0);
                }


                #region Launch Date
                using (var kotak = new SIP_ClientDataContext())
                {
                    var allotdate = from ind in kotak.T_SCHEMES_MASTER_Clients
                                    where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                    select new
                                    {
                                        LaunchDate = ind.Launch_Date
                                    };
                    //  SIPSchDt.Text = "";
                    if (allotdate != null && allotdate.Count() > 0)
                    {
                        allotDate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                        // SIPSchDt.Text = allotDate.ToShortDateString();
                    }
                }
                #endregion



                if (SipDtable1.Rows.Count > 0)
                {

                    #region manual calculation

                    //if (SipDtable1.Rows.Count > 2)
                    //{
                    //    SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);
                    //    DataColumn dc = new DataColumn("Amount", System.Type.GetType("System.Double"));
                    //    SipDtable1.Columns.Add(dc);
                    //    SipDtable1.Columns.Add(new DataColumn("Index_Value_amount", System.Type.GetType("System.Double")));
                    //    SipDtable1.Columns.Add(new DataColumn("Index_unit_cumulative", System.Type.GetType("System.Double")));
                    //    double result;
                    //    for (int i = 0; i < SipDtable1.Rows.Count - 1; i++)
                    //    {

                    //        if (i == 0)
                    //        {
                    //            SipDtable1.Rows[i]["Amount"] = (-1) * Convert.ToDouble(SipDtable1.Rows[i]["Scheme_cashflow"]);

                    //            if (Double.TryParse(SipDtable1.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(SipDtable1.Rows[i]["Index_Unit"].ToString(), out result))
                    //            {
                    //                SipDtable1.Rows[i]["Index_unit_cumulative"] = SipDtable1.Rows[i]["Index_Unit"];
                    //                SipDtable1.Rows[i]["Index_Value_amount"] = Convert.ToDouble(SipDtable1.Rows[i]["Index_Value"]) * Convert.ToDouble(SipDtable1.Rows[i]["Index_unit_cumulative"]);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            SipDtable1.Rows[i]["Amount"] = Convert.ToDouble(SipDtable1.Rows[i - 1]["Amount"]) + (-1) * Convert.ToDouble(SipDtable1.Rows[i]["Scheme_cashflow"]);

                    //            if (Double.TryParse(SipDtable1.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(SipDtable1.Rows[i]["Index_Unit"].ToString(), out result))
                    //            {
                    //                SipDtable1.Rows[i]["Index_unit_cumulative"] = Convert.ToDouble(SipDtable1.Rows[i]["Index_Unit"]) + Convert.ToDouble(SipDtable1.Rows[i - 1]["Index_unit_cumulative"]);
                    //                SipDtable1.Rows[i]["Index_Value_amount"] = Convert.ToDouble(SipDtable1.Rows[i]["Index_Value"]) * Convert.ToDouble(SipDtable1.Rows[i]["Index_unit_cumulative"]);
                    //            }
                    //        }
                    //    }
                    //}

                    #endregion

                    SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);

                    sipGridView.DataSource = SipDtable1;
                    sipGridView.DataBind();


                    #region Remove Bonus Row For Graph Calculation

                    if (SipDtable1.Rows.Count > 0)
                    {
                        if (SipDtable1.Rows.Count > 2)
                        {
                            for (int i = SipDtable1.Rows.Count - 1; i >= 0; i--)
                            {
                                if (SipDtable1.Rows[i]["DIVIDEND_BONUS"].ToString().Trim() != string.Empty && SipDtable1.Rows[i]["DIVIDEND_BONUS"].ToString().Trim() != "0")
                                {
                                    SipDtable1.Rows.RemoveAt(i);
                                }
                            }

                        }
                    }


                    #endregion


                    #region add extra column
                    if (SipDtable1.Rows.Count > 0)
                    {
                        if (SipDtable1.Rows.Count > 2)
                        {
                            SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);
                            DataColumn dc = new DataColumn("Amount", System.Type.GetType("System.Double"));
                            SipDtable1.Columns.Add(dc);
                            SipDtable1.Columns.Add(new DataColumn("Index_Value_amount", System.Type.GetType("System.Double")));
                            // SipDtable1.Columns.Add(new DataColumn("Index_unit_cumulative", System.Type.GetType("System.Double")));
                            double result;
                            for (int i = 0; i < SipDtable1.Rows.Count - 1; i++)
                            {

                                if (i == 0)
                                {
                                    SipDtable1.Rows[i]["Amount"] = (-1) * Convert.ToDouble(SipDtable1.Rows[i]["Scheme_cashflow"]);

                                    if (Double.TryParse(SipDtable1.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"].ToString(), out result))
                                    {
                                        // SipDtable1.Rows[i]["Index_unit_cumulative"] = SipDtable1.Rows[i]["Index_Unit"];
                                        SipDtable1.Rows[i]["Index_Value_amount"] = Math.Round(Convert.ToDouble(SipDtable1.Rows[i]["Index_Value"]) * Convert.ToDouble(SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"]), 2);
                                    }
                                }
                                else
                                {
                                    SipDtable1.Rows[i]["Amount"] = Convert.ToDouble(SipDtable1.Rows[i - 1]["Amount"]) + (-1) * Convert.ToDouble(SipDtable1.Rows[i]["Scheme_cashflow"]);

                                    if (Double.TryParse(SipDtable1.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"].ToString(), out result))
                                    {
                                        // SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"] = Convert.ToDouble(SipDtable1.Rows[i]["Index_Unit"]) + Convert.ToDouble(SipDtable1.Rows[i - 1]["INDEX_UNIT_CUMULATIVE"]);
                                        SipDtable1.Rows[i]["Index_Value_amount"] = Math.Round(Convert.ToDouble(SipDtable1.Rows[i]["Index_Value"]) * Convert.ToDouble(SipDtable1.Rows[i]["INDEX_UNIT_CUMULATIVE"]), 2);
                                    }
                                    else
                                    {
                                        SipDtable1.Rows[i]["Index_Value_amount"] = SipDtable1.Rows[i-1]["Index_Value_amount"];
                                    }
                                }
                            }
                        }
                    }


                    #endregion


                    #region chart
                    DataTable dtChart = new DataTable("dtChart");
                    dtChart = SipDtable1.Copy();
                    dtChart.Rows.RemoveAt(dtChart.Rows.Count - 1);

                    for (int col = dtChart.Columns.Count - 1; col >= 0; col--)
                    {
                        DataColumn dc = dtChart.Columns[col];
                        if (dc.ColumnName.ToUpper() != "NAV_DATE" && dc.ColumnName.ToUpper() != "CUMULATIVE_AMOUNT" && dc.ColumnName.ToUpper() != "INDEX_VALUE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT")//&& dc.ColumnName.ToUpper() != "INVESTMENT_AMOUNT"
                        {
                            //dtChart.Columns.Remove(dc);
                            dtChart.Columns.RemoveAt(col);
                        }
                    }

                    //for (int row = dtChart.Rows.Count - 1; row >= 0; row--)
                    //{
                    //    if (dtChart.Rows[row]["INVESTMENT_AMOUNT"] != DBNull.Value)
                    //    {
                    //        if (Convert.ToDouble(dtChart.Rows[row]["INVESTMENT_AMOUNT"]) < 0)
                    //            dtChart.Rows[row]["INVESTMENT_AMOUNT"] = (-1) * Convert.ToDouble(dtChart.Rows[row]["INVESTMENT_AMOUNT"]);

                    //    }

                    //}
                    ViewState["chartDataTable"] = dtChart;
                    ViewState["btnclick"] = true;

                    //if (CheckBoxChart.Checked)
                    //{
                        BindDataTableToChart(dtChart, "NAV_DATE", chrtResult);
                    //}
                    #endregion

                }

                if (SipDtable2.Rows.Count > 0)
                {
                    GridViewSip2.DataSource = SipDtable2;
                    GridViewSip2.DataBind();

                    if (infodiv.Visible == false)
                        infodiv.Visible = true;

                    Double totalInvestAmount = Convert.ToDouble(SipDtable2.Rows[0]["TOTAL_AMOUNT"]);
                    lblTotalInvst.Text = "Total Investment Amount : Rs " + totalInvestAmount.ToString();

                    if (SipDtable2.Rows[0]["PRESENT_VALUE"].ToString() != "N/A" && Convert.ToDouble(SipDtable2.Rows[0]["PRESENT_VALUE"]) != 0.0)
                    {
                        Double presntInvestValue = Convert.ToDouble(SipDtable2.Rows[0]["PRESENT_VALUE"]);
                        lblInvstvalue.Text = "On " + txtvalason.Text + ", the Scheme value of your total investment Rs " + totalInvestAmount.ToString() + " would be Rs " + presntInvestValue.ToString();
                    }
                    else
                        lblInvstvalue.Text = "On " + txtvalason.Text + ", the Scheme value of your total investment Rs " + totalInvestAmount.ToString() + " would be Rs N/A";

                }

               // divshowChart.Visible = CheckBoxChart.Checked;

                if (CheckBoxChart.Checked)
                    divshowChart.Style.Add("display", "inline");
                else
                    divshowChart.Style.Add("display", "none");

                if (resultDiv.Visible == false)
                    resultDiv.Visible = true;

                //ExportToExcel();

            }
            catch (Exception ex)
            {
               // Response.Write(ex.Message);
                Response.Write(@"<script>alert('" + ex.Message + "')</script>");
               // Response.Write(@"<script>alert('hi');</script>");

            }
            finally
            {
            }

        }


        public void Reset()
        {
            ddlscheme.SelectedIndex = 0;
            ddlsipbnmark.SelectedIndex = 0;
            ddPeriod_SIP.SelectedIndex = 0;
            ddSIPdate.SelectedIndex = 0;
            txtinstall.Text = "1000";
            txtfromDate.Text = "";
            txtToDate.Text = "";
            txtvalason.Text = "";
            CheckBoxChart.Checked = false;
        }
        # endregion

        #region Excel

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the
            // specified ASP.NET server control at run time.
            // No code required here.
            return;
        }

        public void ExportToExcel()
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=KotakSIP.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Text.StringBuilder objFinalstr = new System.Text.StringBuilder();
            resultDiv2.RenderControl(hw);
            //infodiv.RenderControl(hw);
            objFinalstr.Append("<table width='720px'><tr><td width='720px'>");
            objFinalstr.Append("<img src='../KotakImg/kotakheaderlogo.JPG' width='100%' /><br/><br/><br/><br/><br/></td></tr><tr><td>");
            string ImagePath = string.Empty;
            //ImagePath = HttpContext.Current.Server.MapPath("~") + "KotakImg\\";
            //ImagePath = AppDomain.CurrentDomain.BaseDirectory + "Kotak\\KotakImg\\";
            ImagePath = "http://mfiframes.mutualfundsindia.com" + @"/Kotak/KotakImg/";
            objFinalstr.Replace("../KotakImg/kotakheaderlogo.JPG", ImagePath + "kotakheaderlogo.JPG");
            objFinalstr.Append("<br/><b> Scheme :" + ddlscheme.SelectedItem.Text);
            objFinalstr.Append("<br/> Benchmark: " + ddlsipbnmark.SelectedItem.Text + "</b><br/><br/>");



            tmpChartName = "ChartImagetest.jpg";
            //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + tmpChartName;
            //chrtResult.SaveImage(imgPath); 
            //Session["imgPath"] = imgPath;
            // string imgPath2 = Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/" + tmpChartName); 
            if (Session["imgPath"] != null)
            {
                imgPath = Convert.ToString(Session["imgPath"]);
                imgPath = imgPath.Substring(imgPath.IndexOf("KotakImg", 0));
                imgPath = "http://mfiframes.mutualfundsindia.com" + @"/Kotak/" + imgPath.Replace("\\","/");


            }
            //Response.Clear();
            string headerTable = @"</td></tr><tr><td height'500'><br/><Table width='720px'><tr><td width='720px' height='400px'><img src='" + imgPath + @"' width='720px' \></td></tr></Table><br/>";

            objFinalstr.Append(headerTable);
            objFinalstr.Append("<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/></td></tr><tr><td>");
            //Response.Write(headerTable);

            objFinalstr.Append(sw.ToString());

            objFinalstr.Replace(@"class=""grdHead""", "style ='border-bottom: solid;text-align: center;line-height: 21px;border-top-color: #d0d6db; border-collapse: collapse;font-family: Arial;background:#013974;  color: #ffffff;    border-right-color: #d0d6db;    font-size: 11px;    border-left-color: #d0d6db;    font-weight: bold;'");
            objFinalstr.Replace(@"class=""grdRow""", "style=' border: #d0d6db solid 1px;    text-align: center;    line-height: 22px;    background-color: #ffffff;    font-family: Arial;    color: #000;    font-size: 11px;    font-weight: normal;'");
            objFinalstr.Replace(@"class=""grdAltRow""", "style='line-height: 22px;    background-color: #f4f4f4;    font-family: Arial;    color: #000;    font-size: 11px;    border-top: #d0d6db;font-weight: normal;    border: #d0d6db solid 1px;     text-align: center;'");
            objFinalstr.Append("</td></tr></table>");
            Response.Write(objFinalstr.ToString());
            Response.Flush();
            Response.End();


        }


        # endregion

        #region Chart
        public void BindDataTableToChart(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt)
        {
            string columnName = null;
            bool showIndex = false;

            try
            {

                chrt.Series.Clear();
                if (_dt.AsEnumerable().Max(x => x.Field<double?>("INDEX_VALUE_AMOUNT")) != null)
                    showIndex = true;

                foreach (DataColumn dc in _dt.Columns)
                {
                    if (dc.ColumnName == xField)
                        continue;
                    chrt.Series.Add(dc.ColumnName);
                    chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Spline;
                    columnName = dc.ColumnName;

                    //if (dc.ColumnName == "INVESTMENT_AMOUNT")
                    //{
                    //    //chrt.Series[dc.ColumnName].YAxisType = AxisType.Primary;
                    //    chrt.Series[dc.ColumnName].LegendText = "INVESTMENT AMOUNT";
                    //    chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
                    //    //chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Green;
                    //}
                    //else
                    if (dc.ColumnName.ToUpper() == "AMOUNT")
                    {
                        //chrt.Series[dc.ColumnName].YAxisType = AxisType.Primary;
                        chrt.Series[dc.ColumnName].LegendText = "INVESTMENT AMOUNT";
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
                        //chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Yellow;
                    }
                    else if (dc.ColumnName.ToUpper() == "INDEX_VALUE_AMOUNT")
                    {
                        //chrt.Series[dc.ColumnName].YAxisType = AxisType.Primary;

                        chrt.Series[dc.ColumnName].LegendText = "INDEX RETURN AMOUNT";
                        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                        chrt.Series[dc.ColumnName].IsVisibleInLegend = showIndex;

                        //chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Green;
                    }
                    else
                    {
                        //  chrt.Series[dc.ColumnName].YAxisType = AxisType.Secondary;
                        //chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                        chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Blue;
                        chrt.Series[dc.ColumnName].LegendText = "CUMULATIVE AMOUNT";
                    }


                    //chrt.Series.Add(columnName);
                    // if (showIndex)
                    chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, columnName);
                    chrt.Series[columnName].IsValueShownAsLabel = false;
                    //chrt.Series[columnName].ChartType = SeriesChartType.Line;
                    chrt.Series[columnName].BorderWidth = 1;

                    //.Column()
                    //chrt.Legends(0).Enabled = True
                }

                chrt.Series[0].XValueType = ChartValueType.DateTime;



                // chrt.Series["INDEX_VALUE_AMOUNT"].IsVisibleInLegend = showIndex;

                //chrt.Visible = true;


                //foreach (Series ser in chrt.Series)
                //{
                //    ser.ShadowOffset = 1;
                //    // ser.BorderWidth = 3
                //    ser.ChartType = SeriesChartType.Spline;
                //    ser.IsValueShownAsLabel = false;
                //    ser["LabelStyle"] = "Top";
                //}

                //chrt.Titles.Add("Scheme Graph")


                double? maxval = 1;
                double? minval = 10000;






                //for (int k = 0; k <= _dt.Rows.Count - 1; k++)
                //{
                //    foreach (DataColumn dc in _dt.Columns)
                //    {
                //        if (dc.ColumnName.ToUpper() != "NAV_DATE" && !string.IsNullOrEmpty(_dt.Rows[k][dc].ToString()))
                //        {
                //            if (Convert.ToDouble(_dt.Rows[k][dc]) > maxval)
                //            {
                //                maxval = Convert.ToDouble(_dt.Rows[k][dc]);
                //            }

                //            if (Convert.ToDouble(_dt.Rows[k][dc]) < minval)
                //            {
                //                minval = Convert.ToDouble(_dt.Rows[k][dc]);
                //            }
                //        }

                //    }
                //}



                maxval = _dt.AsEnumerable().Max(x => x.Field<double?>("AMOUNT")) >= _dt.AsEnumerable().Max(x => x.Field<double?>("CUMULATIVE_AMOUNT")) ? _dt.AsEnumerable().Max(x => x.Field<double?>("AMOUNT")) : _dt.AsEnumerable().Max(x => x.Field<double?>("CUMULATIVE_AMOUNT"));

                if (_dt.AsEnumerable().Max(x => x.Field<double?>("INDEX_VALUE_AMOUNT")) != null)
                    maxval = maxval >= _dt.AsEnumerable().Max(x => x.Field<double?>("INDEX_VALUE_AMOUNT")) ? maxval : _dt.AsEnumerable().Max(x => x.Field<double?>("INDEX_VALUE_AMOUNT"));

                minval = _dt.AsEnumerable().Min(x => x.Field<double?>("AMOUNT")) <= _dt.AsEnumerable().Min(x => x.Field<double?>("CUMULATIVE_AMOUNT")) ? _dt.AsEnumerable().Min(x => x.Field<double?>("AMOUNT")) : _dt.AsEnumerable().Min(x => x.Field<double?>("CUMULATIVE_AMOUNT"));

                if (_dt.AsEnumerable().Max(x => x.Field<double?>("INDEX_VALUE_AMOUNT")) != null)
                    minval = minval <= _dt.AsEnumerable().Min(x => x.Field<double?>("INDEX_VALUE_AMOUNT")) ? minval : _dt.AsEnumerable().Min(x => x.Field<double?>("INDEX_VALUE_AMOUNT"));



                double scale = Math.Pow(10, (int)Math.Log10((double)maxval));
                int mxval = (int)(Math.Ceiling((double)maxval / scale) * scale);
                maxval = Convert.ToDouble(mxval);

                scale = Math.Pow(10, (int)Math.Log10((double)minval));
                mxval = (int)(Math.Ceiling((double)minval / scale) * scale);
                minval = Convert.ToDouble(mxval);


                // maxval = Math.Ceiling( (int)maxval/1000

                //maxval = _dt.AsEnumerable().Max(x => x.Field<double>("INVESTMENT_AMOUNT")).CompareTo(_dt.AsEnumerable().Max(x => x.Field<double>("INVESTMENT_AMOUNT")));
                //|| y => y.Field<double>("CUMULATIVE_AMOUNT") );
                // maxval = _dt.AsEnumerable().Select(x => x.Field<double>("INVESTMENT_AMOUNT"), x => x.Field<double>("CUMULATIVE_AMOUNT") )

                chrt.ChartAreas[0].AxisY.Maximum = Convert.ToInt32(maxval) + 500;
                if (minval < 1000)
                    chrt.ChartAreas[0].AxisY.Minimum = 0;
                else
                    chrt.ChartAreas[0].AxisY.Minimum = Convert.ToInt32(minval) - 500;

                //chrt.ChartAreas[0].AxisY.Maximum = 30000;
                //chrt.ChartAreas[0].AxisY.Minimum = 1000;
                //chrt.ChartAreas[0].Position.X = 

                //chrt.ChartAreas[0].AxisX.Minimum = _dt.Rows.Cast<DataRow>().Select(x => Convert.ToDateTime(x["NAV_DATE"])).Min().ToOADate();
                //chrt.ChartAreas[0].AxisX.Maximum = _dt.Rows.Cast<DataRow>().Select(x => Convert.ToDateTime(x["NAV_DATE"])).Max().AddMonths(4).ToOADate();


                chrt.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chrt.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
                //chrt.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;

                //chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;

                chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
                chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;

                //chrt.ChartAreas(0).AxisX.Title = "Dates"
                chrt.ChartAreas[0].AxisY.Title = "Figure in Rs";
                chrt.ChartAreas[0].AxisX.Title = "SIP Period";

                var chrtArea = chrt.ChartAreas[0];
                chrtArea.AxisX.MajorGrid.LineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.NotSet;
                chrtArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
                //chrtArea.Visible = true;


                //chrt.Legends.Add("Legend")
                // chrt.Legends(0).Alignment = StringAlignment.Far
                //chrt.Legends(0).Position

                //'chrt.ChartAreas(0).AxisY.in
                //chrt.Legends.Add(new Legend("Default") {Docking = Docking.Bottom   }) ;
                Legend legend = chrt.Legends[0];


                //legend.CustomItems[3].Legend.
                //LegendItem item = new LegendItem();
                //item.SeparatorColor = System.Drawing.Color.White;
                //item.Name = "TestLegend";
                //item.BorderWidth = 4;
                //item.ShadowOffset = 1;
                //chrt.Legends[0].CustomItems.Adsd(item);

                // Customize the legend appearance.
                //legend.BackColor = Color.Beige;

                legend.Font = new Font("Arial", 9, FontStyle.Bold);

                #region Chart Image MyRegion
                // public static string TheSessionId() {
                HttpSessionState ss = HttpContext.Current.Session;
                //HttpContext.Current.Session["test"] = "test";
                // HttpContext.Current.Response.Write(ss.SessionID);
                // return "ok";
                //}

                //String sFileImage = System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["UploadPath"], Session["UserId"].ToString() + ".gif");                
                tmpChartName = "ChartImagetest.jpg";
                string todaydate = System.DateTime.Today.ToString("dd-MM-yyyy").Replace('-', '_').Replace('/', '_').Trim();// DateTime.Today.ToShortDateString().ToString("dd/MM/yyyy");


                string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                string localImgPath = @"Kotak\KotakImg";
                localImgPath = System.IO.Path.Combine(appPath, localImgPath);

                string testpath = localImgPath;




                #region : save Image


                localImgPath = System.IO.Path.Combine(localImgPath, todaydate);
                if (!Directory.Exists(localImgPath))
                {
                    //using (Directory.CreateDirectory(localImgPath))
                    //{                    }
                    Directory.CreateDirectory(localImgPath);
                }


                imgPath = System.IO.Path.Combine(localImgPath, ss.SessionID + "_" + tmpChartName);

                Session["imgPath"] = imgPath;
                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                }
                chrtResult.SaveImage(imgPath);
                chrtResult.Visible = false;
                #endregion
                ImgchrtResult.Visible = true;
                ImgchrtResult.ImageUrl = @"KotakImg\" + todaydate + @"\" + Path.GetFileName(imgPath);
                #region Delete MyRegion
                //first method
                if (Directory.Exists(testpath))
                {

                    string[] objfilearray = Directory.GetDirectories(testpath); // File.GetAttributes(testpath)

                    foreach (string filename in objfilearray)
                    {
                        if (filename != localImgPath)
                            Directory.Delete(filename, true);
                    }
                    //Directory.Delete(testpath, true);
                }

                ////aonther method
                //for (int i = 1; i <= 7; i++)
                //{
                //    testpath = localImgPath;
                //    string testdate = System.DateTime.Today.AddDays(-i).ToString("dd-MM-yyyy").Replace('-', '_').Replace('/', '_').Trim();// 
                //    testpath = System.IO.Path.Combine(testpath, testdate);
                //    if (Directory.Exists(testpath))
                //    {
                //        Directory.Delete(testpath, true);
                //    }
                //}

                #endregion

                #endregion
                //chrt.Series.Clear();
                //foreach (DataColumn dc in _dt.Columns)
                //{
                //    if (dc.ColumnName == "")
                //        continue;
                //    chrt.Series.Add(dc.ColumnName);
                //    chrt.Series[dc.ColumnName].ChartType = SeriesChartType.Line;

                //    if (dc.ColumnName == "EPS")
                //    {
                //        chrt.Series[dc.ColumnName].YAxisType = AxisType.Secondary;
                //        chrt.Series[dc.ColumnName].LegendText = "EPS";
                //        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
                //        //chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Green;
                //    }
                //    else
                //    {
                //        chrt.Series[dc.ColumnName].YAxisType = AxisType.Primary;
                //        chrt.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                //        //chrt.Series[dc.ColumnName].Color = System.Drawing.Color.Red;
                //        chrt.Series[dc.ColumnName].LegendText = "Share Price";
                //    }
                //    chrt.Series[dc.ColumnName].Points.DataBindXY(_dt.DefaultView, "", _dt.DefaultView, dc.ColumnName);
                //    chrt.Series[dc.ColumnName].BorderWidth = 1;
                //}


                //chrt.Legends[0].LegendStyle
                //chrt.Legends[0].LegendItemOrder = LegendItemOrder. Orientation.Vertical;                    
                //  chrt.Legends.Add(new Legend("Legend2"));

                // Set Docking of the Legend chart to the Default Chart Area.
                //chrt.Legends["Legend2"].DockedToChartArea = "Default";

                // Assign the legend to Series1.
                //chrt.Series["Series1"].Legend = "Legend2";
                //chrt.Series["Series1"].IsVisibleInLegend = true;

                //chrt.Legends(0).Alignment = Docking.Bottom
                // chrt.Legends.Add(new Legend( GetLegend("Legend", 7f);



            }
            catch (Exception ex)
            {
                Response.Write(@"<script>alert('" + ex.Message + "')</script>");
            }

        }

        protected void chkBoxShowGraph_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkBoxShowGraph.Checked)
            //{
            //    if (ViewState["chartDataTable"] != null && Convert.ToBoolean(ViewState["btnclick"]) == true)
            //    {
            //        DataTable dtblchart = (DataTable)ViewState["chartDataTable"];
            //        if (dtblchart.Rows.Count > 0)
            //        {
            //            BindDataTableToChart(dtblchart, "NAV_DATE", chrtResult);
            //            //chrtResult.Visible = true;
            //        }
            //    }
            //}
            //else
            //{
            //    //chrtResult.Visible = false;
            //    return;
            //}
        }


        //void formatChart(DataTable _dt)
        //{
        //    LegendItem item = new LegendItem();
        //    item.SeparatorColor = System.Drawing.Color.White;
        //    item.Name = "lgnchtArea";
        //    item.BorderWidth = 4;
        //    item.ShadowOffset = 1;
        //}
        # endregion

    }


    #region Class Definition
    public class CalcReturnDataClient
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

    public class CalcReturnDataClient2
    {
        public decimal ID { get; set; }
        public decimal SCHEME_ID { get; set; }
        public string SCHEME_NAME { get; set; }
        public string NAV_DATE { get; set; }
        public double NAV { get; set; }
        public double? INDEX_VALUE { get; set; }
        public double? SCHEME_UNITS { get; set; }
        public double? SCHEME_UNITS_CUMULATIVE { get; set; }
        public double? INDEX_UNIT { get; set; }
        public double? INDEX_UNIT_CUMULATIVE { get; set; }
        public double? SCHEME_CASHFLOW { get; set; }
        public double? INDEX_CASHFLOW { get; set; }
        public double? CUMULATIVE_AMOUNT { get; set; }
        public double? AMOUNT_ONETIME { get; set; }
        public double? SCHEME_UNIT_ONETIME { get; set; }
        public double? INDEX_UNIT_ONETIME { get; set; }
        public string DIVIDEND_BONUS { get; set; }
        public double? INVESTMENT_AMOUNT { get; set; }
    }

    /// <summary>
    /// Added For Reinvest Option
    /// </summary>
    public class CalcReturnDataClient3
    {
        public object SCHEME_ID { get; set; }
        public string SCHEME_NAME { get; set; }
        public DateTime NAV_DATE { get; set; }
        public double? NAV { get; set; }
        public string DIVIDEND { get; set; }
        public string BONUS { get; set; }
        public double? PAYOUT_AMOUNT { get; set; }
        //public object SCHEME_ID { get; set; }
        //public object SCHEME_NAME { get; set; }
        //public object NAV_DATE { get; set; }
        //public object NAV { get; set; }
        //public object DIVIDEND { get; set; }
        //public object BONUS { get; set; }
        //public object PAYOUT_AMOUNT { get; set; }
    }

    #endregion
}

namespace iFrames.DAL
{
    public partial class SIP_ClientDataContext : System.Data.Linq.DataContext
    {
        [Function(Name = "dbo.MFIE_SP_SIP_Calculater_client")]        
        [ResultType(typeof(CalcReturnDataClient2))]
        [ResultType(typeof(CalcReturnDataClient))]
        [ResultType(typeof(CalcReturnDataClient3))]
        public IMultipleResults MFIE_SP_SIP_CALCULATER_CLIENT([Parameter(Name = "Scheme_Ids", DbType = "VarChar(MAX)")] string scheme_Ids, [Parameter(Name = "Plan_Start_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_Start_Date, [Parameter(Name = "Plan_End_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_End_Date, [Parameter(Name = "Report_As_On", DbType = "DateTime")] System.Nullable<System.DateTime> report_As_On, [Parameter(Name = "SIP_Amt", DbType = "Float")] System.Nullable<double> sIP_Amt, [Parameter(Name = "Frequency", DbType = "VarChar(50)")] string frequency, [Parameter(Name = "Dividend_Type", DbType = "VarChar(50)")] string dividend_Type, [Parameter(Name = "Initial_Flage", DbType = "Int")] System.Nullable<int> initial_Flage, [Parameter(Name = "Initial_Amount", DbType = "Float")] System.Nullable<double> initial_Amount, [Parameter(Name = "Initial_Date", DbType = "DateTime")] System.Nullable<System.DateTime> initial_Date, [Parameter(Name = "Index_Flage", DbType = "Int")] System.Nullable<int> index_Flage, [Parameter(Name = "OPTIONAL_RET_FLAG", DbType = "VarChar(1)")] string oPTIONAL_RET_FLAG)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), scheme_Ids, plan_Start_Date, plan_End_Date, report_As_On, sIP_Amt, frequency, dividend_Type, initial_Flage, initial_Amount, initial_Date, index_Flage, oPTIONAL_RET_FLAG);
            return ((IMultipleResults)(result.ReturnValue));
        }

    }
}