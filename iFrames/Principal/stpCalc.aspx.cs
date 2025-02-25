using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.DAL;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace iFrames.Principal
{
    public partial class stpCalc : System.Web.UI.Page
    {
        #region: Global Variables

        readonly SqlConnection _conn = new SqlConnection();
        private readonly string _connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;

        #endregion

        #region: Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (stpbtnshow != null) stpbtnshow.Attributes.Add("onclick", "javascript:return validate_STP();");
            if (!IsPostBack)
            {
                FillFromScheme();
                FillToScheme();
            }
         //   SetInceptionOnDropDown();
        }

        #endregion

        #region: DropDown Events

        protected void ddlschtrf_SelectedIndexChanged(object sender, EventArgs e)
        {
            resultDiv.Visible = false;
            FillDropdownIndex();

            #region Launch Date

            //string schmeId = ddlschtrf.SelectedItem.Value;
            //if (schmeId == null || schmeId == string.Empty) throw new ArgumentNullException("schmeId");

            //using (var principl = new PrincipalCalcDataContext())
            //{

            //    var allotdate = from ind in principl.T_SCHEMES_MASTERs
            //                    where ind.Scheme_Id == Convert.ToDecimal(schmeId)
            //                    select new
            //                    {
            //                        LaunchDate = ind.Launch_Date
            //                    };


            //    //SIPSchDt.Text = "";
            //    //if (allotdate != null && allotdate.Count() > 0)
            //    //{

            //    //    //SWPSchDt.Text = "<b>" + Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "<b/>";
            //    //}
            //}
            #endregion


            if (ddlschtrf.SelectedIndex == 0 || ddlschtrf.SelectedItem.Value == "--")
            {
                return;
            }
            else
            {
                FillDropdown(ddlschtrto, ddlschtrf.SelectedItem.Value);
            }
            SetInceptionOnDropDown();

        }
        protected void ddlschtrto_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetInceptionOnDropDown();
        }
        protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowRelativeDiv();
        }
        #endregion

        #region: Button Events

        protected void stpbtnshow_Click(object sender, ImageClickEventArgs e)
        {
            CalculateReturn();
        }
        protected void stpbtnreset_Click(object sender, ImageClickEventArgs e)
        {
            resultDiv.Visible = false;
            Reset();
        }

        #endregion

        #region: Methods

        #region: Fill Methods

        public void FillFromScheme()
        {
            FillDropdown(ddlschtrf);
        }
        public void FillToScheme()
        {
            FillDropdown(ddlschtrto);
            ListItem itemToRemove = ddlschtrto.Items.FindByText("Principal Tax Savings Fund");
            if (itemToRemove != null)
            {
                ddlschtrto.Items.Remove(itemToRemove);
            }
        }
        public void FillDropdownIndex()
        {
            FillDropdownIndex(ddlbnmark);
        }
        public void FillDropdown(Control ddl)
        {
            try
            {
                using (DataTable dt = FetchScheme())
                {
                    if (dt == null) throw new ArgumentNullException("dt");

                    if (dt.Rows.Count > 0)
                    {
                        Dictionary<string, string> SchemeInception = new Dictionary<string, string>();
                        DropDownList drpdwn = (DropDownList)ddl;
                        if (drpdwn != null)
                        {
                            drpdwn.DataSource = dt;
                            drpdwn.Items.Clear();
                            drpdwn.DataTextField = "Sch_Short_Name";
                            drpdwn.DataValueField = "Scheme_Id";
                        }
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
               
            }
            catch (Exception ex)
            {


            }
        }

        public void FillDropdown(Control ddl, string schid)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = FetchSchemeWithout(schid);
                if (dt.Rows.Count > 0)
                {
                    DropDownList drpdwn = (DropDownList)ddl;
                   // Dictionary<string, string> SchemeInception = new Dictionary<string, string>();
                    drpdwn.Items.Clear();
                    drpdwn.DataSource = dt;
                    drpdwn.DataTextField = "Sch_Short_Name";
                    drpdwn.DataValueField = "Scheme_Id";
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem li = new ListItem(dr["Sch_Short_Name"].ToString(), dr["Scheme_Id"].ToString());
                        li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        //SchemeInception.Add(dr["Scheme_Id"].ToString(), dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                        drpdwn.Items.Add(li);
                    }
                    // drpdwn.DataBind();
                   // ViewState["SchemeInception"] = SchemeInception;
                    drpdwn.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
                    drpdwn.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void FillDropdownIndex(Control ddl)
        {
            try
            {
                DataTable dt = FetchBenchMark(Convert.ToDecimal(ddlschtrf.SelectedItem.Value));
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
        public DataTable FetchScheme()
        {
            DataTable dt = new DataTable();
            try
            {

                using (var scheme = new PrincipalCalcDataContext())
                {
                    var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                     where
                                       t_fund_masters.MUTUALFUND_ID == 15 && t_fund_masters.SUB_NATURE_ID != 2
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
                                         where  //s.T_FUND_MASTER.SUB_NATURE_ID != 2 && 
                                         s.Nav_Check == 3 && // for growth option 
                                         s.Option_Id == 2
                                         join tsi in scheme.T_SCHEMES_INDEXes
                                         on s.Scheme_Id equals tsi.SCHEME_ID
                                         orderby s.Sch_Short_Name
                                         select new
                                         {
                                             s.Sch_Short_Name,
                                             s.Scheme_Id,
                                             s.Launch_Date
                                         }).Distinct();
                    DataTable dt2 = null;
                    if (scheme_name_1.Count() > 0)
                        dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();

                    dt = dt2.Copy();

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                _conn.Close();
            }
            return dt;
        }

        public DataTable FetchSchemeWithout(string schid)
        {
            //conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            try
            {

                using (var scheme = new PrincipalCalcDataContext())
                {
                    var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                     where
                                       t_fund_masters.MUTUALFUND_ID == 15 && t_fund_masters.SUB_NATURE_ID != 2
                                     select new
                                     {
                                         t_fund_masters.FUND_ID
                                     });

                    DataTable dtt = null;
                    if (fundtable.Count() > 0)
                        dtt = fundtable.ToDataTable();


                    var scheme_name_1 = (
                        from s in scheme.T_SCHEMES_MASTERs
                        join T in fundtable
                        on s.Fund_Id equals T.FUND_ID
                        join tsi in scheme.T_SCHEMES_INDEXes
                        on s.Scheme_Id equals tsi.SCHEME_ID
                        where //s.T_FUND_MASTER.SUB_NATURE_ID != 2 && 
                        s.Nav_Check == 3 && s.Scheme_Id != Convert.ToDecimal(schid)
                        && s.Option_Id == 2// || s.Scheme_Id == 2554 // for growth option & DSP BlackRock Equity Fund added additionaly
                        orderby s.Sch_Short_Name
                        select new
                        {
                            s.Sch_Short_Name,
                            s.Scheme_Id,
                            s.Launch_Date
                        }).Distinct();
                    DataTable dt2 = null;
                    if (scheme_name_1.Count() > 0)
                        dt2 = scheme_name_1.OrderBy(x => x.Sch_Short_Name).ToDataTable();

                    dt = dt2.Copy();

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                _conn.Close();
            }
            return dt;
        }
        public System.Data.DataTable FetchBenchMark(decimal schid)
        {
            _conn.ConnectionString = _connstr;
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
                _conn.Close();
            }
            return dt;
        }
        public void SetInceptionOnDropDown()
        {
            if ((ViewState["SchemeInception"] != null) && (ddlschtrf.Items.Count > 0))
            {
                Dictionary<string, string> SchemeInception = (Dictionary<string, string>)(ViewState["SchemeInception"]);
                for (int i = 0; i < ddlschtrf.Items.Count; i++)
                {
                    string s = "";
                    if (SchemeInception.TryGetValue(ddlschtrf.SelectedItem.Value, out s) && ddlschtrf.Items[i].Selected == true)
                    {
                        ddlschtrf.Items[i].Attributes.Add("title", s);
                    }
                   
                }

                for (int i = 0; i < ddlschtrto.Items.Count; i++)
                {
                    string s = "";
                    if (SchemeInception.TryGetValue(ddlschtrto.SelectedItem.Value, out s) && ddlschtrto.Items[i].Selected == true)
                    {
                        ddlschtrto.Items[i].Attributes.Add("title", s);
                    }

                }
            }
        }

        #endregion

        #region: Anonymous Methods

        public string TwoDecimal(string data)
        {
            string result = string.Empty;
            result = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(data))).ToString();
            return result;
        }
        public void Reset()
        {
            ddlschtrf.SelectedIndex = 0;
            ddlschtrto.SelectedIndex = 0;
            if (ddlbnmark.Items.Count > 0)
                ddlbnmark.Items.Clear();
            txtddperiod.Text = "";
            txtddSTPDate.Text = "";
            txtiniamt.Text = "";
            txttranamt.Text = "";
            txtfrdt.Text = "";
            txttodt.Text = "";
            txtvalue.Text = "";
            CheckBoxChart.Checked = false;
            TableNote.Visible = false;
        }

        #endregion

        #region: Calculation Method

        public void CalculateReturn()
        {
            #region Initialize
            DateTime stpStartDate, stpEndDate, stpAsonDate, tempDate, investmentDate, allotDate, initialDate;
            int daydiff, stpDate, PrdVal, tempInt;
            TimeSpan datedifference;
            SqlCommand cmdstp = null;
            double dblStpTransferAmnt, dblstpIntialAmnt;
            string strFrequency = string.Empty, schemeIdtrf = string.Empty, schemeIdtrto = string.Empty, strInvestorType = string.Empty;
            DataSet dSet = new DataSet("stpDataSet");
            DataTable dtble = new DataTable("stpDataTable"), datatTble = new DataTable("stpDT");
            double? amountLeft = null, investmntValue = null, cumulativAmountTo = null, returnXIRRSchemeFrom = null, returnXIRRSchemeTo = null;

            #endregion
            try
            {
                stpStartDate = new DateTime(Convert.ToInt16(txtfrdt.Text.Split('/')[2]),
                                       Convert.ToInt16(txtfrdt.Text.Split('/')[1]),
                                       Convert.ToInt16(txtfrdt.Text.Split('/')[0]));

                stpEndDate = new DateTime(Convert.ToInt16(txttodt.Text.Split('/')[2]),
                                         Convert.ToInt16(txttodt.Text.Split('/')[1]),
                                         Convert.ToInt16(txttodt.Text.Split('/')[0]));

                stpAsonDate = new DateTime(Convert.ToInt16(txtvalue.Text.Split('/')[2]),
                                     Convert.ToInt16(txtvalue.Text.Split('/')[1]),
                                     Convert.ToInt16(txtvalue.Text.Split('/')[0]));


                stpDate = Convert.ToInt32(txtddSTPDate.Text);
                dblStpTransferAmnt = Convert.ToDouble(txttranamt.Text);
                dblstpIntialAmnt = Convert.ToDouble(txtiniamt.Text);
                schemeIdtrf = ddlschtrf.SelectedItem.Value;
                schemeIdtrto = ddlschtrto.SelectedItem.Value;
                strFrequency = txtddperiod.Text;
                strInvestorType = "Individual/Huf";
                initialDate = stpStartDate;
                stpStartDate = stpStartDate.AddDays(7);// add 7 days for intermediate transaction time lag

                _conn.ConnectionString = _connstr;
                #region condition check
                //if (txtddperiod.Text == "Monthly")
                if (txtddperiod.SelectedItem.Text == "Monthly")
                {
                    PrdVal = 1;
                }
                //else if (txtddperiod.Text == "Quarterly")
                else if (txtddperiod.SelectedItem.Text == "Quarterly")
                {
                    PrdVal = 3;
                }
                else
                {
                    return;
                }

                //--v01-Apr-08---validation for at least 6  transfers by jscript



                if (PrdVal == 1)
                {
                    tempDate = stpEndDate.AddMonths(-6);
                    datedifference = tempDate.Subtract(stpStartDate);
                    daydiff = datedifference.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"STP is allowed for minimum 6 withdrawal\")</script>");
                        return;
                    }
                }
                else if (PrdVal == 3)
                {
                    tempDate = stpEndDate.AddMonths(-18);
                    datedifference = tempDate.Subtract(stpStartDate);
                    daydiff = datedifference.Days;
                    if (daydiff < 0)
                    {
                        Response.Write("<script>alert(\"STP is allowed for minimum 6 withdrawal\")</script>");
                        return;
                    }
                }

                tempInt = stpStartDate.Day;
                if (stpDate < tempInt)
                {
                    if (stpStartDate.Month != 12)
                        investmentDate = new DateTime(stpStartDate.Year, stpStartDate.Month + 1, stpDate);
                    else
                        investmentDate = new DateTime(stpStartDate.Year + 1, 1, stpDate);
                }
                else if (stpDate == tempInt)
                {
                    investmentDate = stpStartDate;
                }
                else
                {
                    investmentDate = new DateTime(stpStartDate.Year, stpStartDate.Month, stpDate);
                }
                #endregion

                # region calling SP


                cmdstp = new SqlCommand("MFIE_SP_STP_CALCULATER_DSP", _conn);
                cmdstp.CommandType = CommandType.StoredProcedure;
                cmdstp.CommandTimeout = 2000;
                cmdstp.Parameters.Add(new SqlParameter("@From_Scheme_Id", schemeIdtrf));
                cmdstp.Parameters.Add(new SqlParameter("@To_Scheme_Id", schemeIdtrto));
                cmdstp.Parameters.Add(new SqlParameter("@Plan_Start_Date", investmentDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));//swpStartDate
                cmdstp.Parameters.Add(new SqlParameter("@Plan_End_Date", stpEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdstp.Parameters.Add(new SqlParameter("@Report_As_On", stpAsonDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdstp.Parameters.Add(new SqlParameter("@STP_Amt", dblStpTransferAmnt));
                cmdstp.Parameters.Add(new SqlParameter("@Frequency", strFrequency));
                cmdstp.Parameters.Add(new SqlParameter("@Dividend_Type", strInvestorType));
                cmdstp.Parameters.Add(new SqlParameter("@Investment_Amount", dblstpIntialAmnt));
                cmdstp.Parameters.Add(new SqlParameter("@Investment_Date", initialDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

                SqlDataAdapter da = new SqlDataAdapter(cmdstp);
                da.Fill(dSet);
                #endregion

                if (dSet.Tables.Count > 0)
                {
                    dtble = dSet.Tables[0];
                    ViewState["stpDataTable"] = dtble;
                    GetSummary(dtble);
                    GetDetails(dtble);
                }

                //#region Launch Date
                //using (var principl = new PrincipalCalcDataContext())
                //{
                //    var allotdate = from ind in principl.T_SCHEMES_MASTERs
                //                    where ind.Scheme_Id == Convert.ToDecimal(schemeIdtrf)
                //                    select new
                //                    {
                //                        LaunchDate = ind.Launch_Date
                //                    };
                //    //STPSchDt.Text = "";
                //    if (allotdate != null && allotdate.Count() > 0)
                //    {
                //        allotDate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                //        //STPSchDt.Text = "<b>" + allotDate.ToShortDateString() + "</b>";
                //    }
                //}
                //#endregion

                //if (dtble != null && dtble.Rows.Count > 2)
                //{
                //    if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 3]["CUMILATIVE_AMOUNT_FROM"]) != "N/A")
                //        amountLeft = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 3]["CUMILATIVE_AMOUNT_FROM"]), 2);
                //    ViewState["amountLeft"] = amountLeft;

                //    if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 2]["INVST_AMOUNT"]) != "N/A")
                //        returnXIRRSchemeFrom = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 2]["INVST_AMOUNT"]), 2);

                //    if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 1]["INVST_AMOUNT"]) != "N/A")
                //        returnXIRRSchemeTo = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 1]["INVST_AMOUNT"]), 2);

                //}

                //var investval = dtble.AsEnumerable().Where(x => x.Field<double?>("INVST_AMOUNT") == dblStpTransferAmnt).Select(x => x.Field<double?>("INVST_AMOUNT")).Sum();

                //investmntValue = Math.Round(Convert.ToDouble(investval), 2);
                //ViewState["investmntValue"] = investmntValue;

                //if (Convert.ToString(dtble.Rows[dtble.Rows.Count - 3]["FINAL_INVST_AMOUNT_TO"]) != "N/A")
                //{
                //    if (Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 3]["FINAL_INVST_AMOUNT_TO"]) != 0)
                //        cumulativAmountTo = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 3]["FINAL_INVST_AMOUNT_TO"]), 2);
                //    else
                //        cumulativAmountTo = Math.Round(Convert.ToDouble(dtble.Rows[dtble.Rows.Count - 4]["FINAL_INVST_AMOUNT_TO"]), 2);
                //}

                //ViewState["cumulativAmountTo"] = cumulativAmountTo;
                //double? invstAmountLeft = dblstpIntialAmnt - investmntValue;
                //if (invstAmountLeft < 0)
                //    invstAmountLeft = 0;


                //ViewState["invstAmountLeft"] = invstAmountLeft;


                resultDiv.Visible = true;
                if (TableNote.Visible == false) TableNote.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }
        }
        public void GetSummary(DataTable dtMain)
        {
            DataTable dt = new DataTable();
            string SchemeName = string.Empty;
            string TotalAmtInvst = string.Empty;
            string TotalAmtWidrn = string.Empty;
            string PresentValue = string.Empty;
            string Yield = string.Empty;

            try
            {
                dt.Columns.Add("FromSchemeName", typeof(String));
                dt.Columns.Add("TotalAmtInvst", typeof(String));
                dt.Columns.Add("TotalAmtWidrn", typeof(String));
                dt.Columns.Add("PresentValue", typeof(String));
                dt.Columns.Add("Yield", typeof(String));


                SchemeName = dtMain.AsEnumerable().Select(x => x.Field<String>("FROM_SCHEME_NAME")).First();
                TotalAmtInvst = dtMain.AsEnumerable().Select(x => x.Field<double?>("INVST_AMOUNT")).First().ToString();
                //var withdrawnval = dtMain.AsEnumerable().Where(x => x.Field<double?>("INVST_AMOUNT") == Convert.ToDouble(txttranamt.Text)).Select(x => x.Field<double?>("INVST_AMOUNT")).Sum();
                var withdrawnval = dtMain.AsEnumerable().Where(x => x.Field<double?>("INVST_AMOUNT") > 0 && x.Field<String>("FROM_SCHEME_NAME") != "YIELD FROM:" && x.Field<String>("FROM_SCHEME_NAME") != "YIELD TO:").Select(x => x.Field<double?>("INVST_AMOUNT")).Sum();
                TotalAmtWidrn = Math.Round(Convert.ToDouble(withdrawnval), 2).ToString();

                if (Convert.ToString(dtMain.Rows[dtMain.Rows.Count - 3]["CUMILATIVE_AMOUNT_FROM"]) != "N/A")
                    PresentValue = Math.Round(Convert.ToDouble(dtMain.Rows[dtMain.Rows.Count - 3]["CUMILATIVE_AMOUNT_FROM"]), 2).ToString();
                // var prsntval = dtMain.AsEnumerable().Select(x => x.Field<double?>("CUMILATIVE_AMOUNT_FROM")).Last();
                //PresentValue = Math.Round(Convert.ToDouble(prsntval), 2).ToString();

                if (Convert.ToString(dtMain.Rows[dtMain.Rows.Count - 2]["INVST_AMOUNT"]) != "N/A")
                    Yield = Math.Round(Convert.ToDouble(dtMain.Rows[dtMain.Rows.Count - 2]["INVST_AMOUNT"]), 2).ToString();


                DataRow dr = dt.NewRow();
                dr["FromSchemeName"] = SchemeName;
                dr["TotalAmtInvst"] = TotalAmtInvst;
                dr["TotalAmtWidrn"] = TotalAmtWidrn;
                dr["PresentValue"] = PresentValue;
                dr["Yield"] = Yield;
                dt.Rows.Add(dr);

                if (dt.Rows.Count > 0)
                {
                    gvFromSchemeSummary.DataSource = dt;
                    gvFromSchemeSummary.DataBind();
                    GetSummary(dtMain, TotalAmtWidrn);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetSummary(DataTable dtMain, string _TotalAmtInvst)
        {
            DataTable dt = new DataTable();
            string SchemeName = string.Empty;
            string TotalAmtInvst = _TotalAmtInvst;
            string PresentValue = string.Empty;
            string Yield = string.Empty;

            try
            {
                dt.Columns.Add("ToSchemeName", typeof(String));
                dt.Columns.Add("TotalAmtInvst", typeof(String));
                dt.Columns.Add("PresentValue", typeof(String));
                dt.Columns.Add("Yield", typeof(String));


                SchemeName = dtMain.AsEnumerable().Select(x => x.Field<String>("TO_SCHEME_NAME")).First();
                TotalAmtInvst = _TotalAmtInvst;

                if (Convert.ToString(dtMain.Rows[dtMain.Rows.Count - 3]["CUMILATIVE_AMOUNT_TO"]) != "N/A")
                    PresentValue = Math.Round(Convert.ToDouble(dtMain.Rows[dtMain.Rows.Count - 3]["CUMILATIVE_AMOUNT_TO"]), 2).ToString();

                //var prsntval = dtMain.AsEnumerable().Select(x => x.Field<double?>("CUMILATIVE_AMOUNT_TO")).Last();
                //PresentValue = Math.Round(Convert.ToDouble(prsntval), 2).ToString();

                if (Convert.ToString(dtMain.Rows[dtMain.Rows.Count - 1]["INVST_AMOUNT"]) != "N/A")
                    Yield = Math.Round(Convert.ToDouble(dtMain.Rows[dtMain.Rows.Count - 1]["INVST_AMOUNT"]), 2).ToString();


                DataRow dr = dt.NewRow();
                dr["ToSchemeName"] = SchemeName;
                dr["TotalAmtInvst"] = TotalAmtInvst;
                dr["PresentValue"] = PresentValue;
                dr["Yield"] = Yield;
                dt.Rows.Add(dr);

                if (dt.Rows.Count > 0)
                {
                    gvToSchemeSummary.DataSource = dt;
                    gvToSchemeSummary.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetDetails(DataTable dtMain)
        {
            try
            {
                DataTable dtbleSchemeFrom = new DataTable("infoSchemeFrom");
                DataTable dtbleSchemeTo = new DataTable("infoSchemeTo");

                dtbleSchemeFrom = dtMain.Copy();
                dtbleSchemeTo = dtMain.Copy();

                for (int i = dtMain.Columns.Count - 1; i > 0; i--)
                {
                    if (i < 10)
                        dtbleSchemeTo.Columns.RemoveAt(i);
                    else
                        dtbleSchemeFrom.Columns.RemoveAt(i);
                }

                if (dtbleSchemeFrom.Rows.Count > 0)
                {

                    dtbleSchemeFrom.Rows.RemoveAt(dtbleSchemeFrom.Rows.Count - 1);
                    dtbleSchemeFrom.Rows.RemoveAt(dtbleSchemeFrom.Rows.Count - 1);
                    gvSTPDetails1.DataSource = dtbleSchemeFrom;
                    gvSTPDetails1.DataBind();
                }
                if (dtbleSchemeTo.Rows.Count > 0)
                {
                    dtbleSchemeTo.Rows.RemoveAt(dtbleSchemeTo.Rows.Count - 1);
                    dtbleSchemeTo.Rows.RemoveAt(dtbleSchemeTo.Rows.Count - 1);
                    gvSTPDetails2.DataSource = dtbleSchemeTo;
                    gvSTPDetails2.DataBind();
                }

                GetChartDT(dtbleSchemeFrom, dtbleSchemeTo);

                if (CheckBoxChart.Checked)
                {
                    divFromSchemeshowChart.Style.Add("display", "inline");
                    divToSchemeshowChart.Style.Add("display", "inline");
                }
                else
                {
                    divFromSchemeshowChart.Style.Add("display", "none");
                    divToSchemeshowChart.Style.Add("display", "none");
                }

                divFromSchemeshowChart.Visible = true;
                divToSchemeshowChart.Visible = true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region: Chart Methods

        public void GetChartDT(DataTable dtFrom, DataTable dtTo)
        {
            try
            {
                DataTable dtChartFrom = new DataTable("dtChart");
                DataTable dtChartTo = new DataTable("dtChart1");
                dtChartFrom = dtFrom.Copy();
                dtChartTo = dtTo.Copy();

                dtChartFrom.Rows.RemoveAt(dtChartFrom.Rows.Count - 1);
                dtChartFrom.Rows.RemoveAt(dtChartFrom.Rows.Count - 1);

                dtChartTo.Rows.RemoveAt(0);
                dtChartTo.Rows.RemoveAt(dtChartTo.Rows.Count - 1);
                dtChartTo.Rows.RemoveAt(dtChartTo.Rows.Count - 1);

                #region add extra column
                if (dtChartFrom.Rows.Count > 0)
                {
                    if (dtChartFrom.Rows.Count > 2)
                    {
                        DataColumn dc = new DataColumn("Amount_D", System.Type.GetType("System.Double"));
                        dtChartFrom.Columns.Add(dc);

                        for (int i = 0; i < dtChartFrom.Rows.Count - 1; i++)
                        {

                            if (i == 0)
                            {
                                if (dtChartFrom.Rows[i]["INVST_AMOUNT"] != DBNull.Value)
                                {
                                    dtChartFrom.Rows[i]["Amount_D"] = (-1) * Convert.ToDouble(dtChartFrom.Rows[i]["INVST_AMOUNT"]);
                                }
                                else
                                {
                                    dtChartFrom.Rows[i]["Amount_D"] = dtChartFrom.Rows[i]["INVST_AMOUNT"];
                                }

                            }
                            else
                            {

                                if (dtChartFrom.Rows[i]["INVST_AMOUNT"] != DBNull.Value && dtChartFrom.Rows[i - 1]["Amount_D"] != DBNull.Value)
                                {
                                    dtChartFrom.Rows[i]["Amount_D"] = Convert.ToDouble(dtChartFrom.Rows[i - 1]["Amount_D"]) + (-1) * Convert.ToDouble(dtChartFrom.Rows[i]["INVST_AMOUNT"]);
                                }
                                else
                                {
                                    dtChartFrom.Rows[i]["Amount_D"] = dtChartFrom.Rows[i]["INVST_AMOUNT"];
                                }
                            }
                        }
                    }
                }


                #endregion

                for (int col = dtChartFrom.Columns.Count - 1; col >= 0; col--)
                {
                    DataColumn dc = dtChartFrom.Columns[col];
                    if (dc.ColumnName.ToUpper() != "FROM_DATE" && dc.ColumnName.ToUpper() != "CUMILATIVE_AMOUNT_FROM" && dc.ColumnName.ToUpper() != "AMOUNT_D")//&& dc.ColumnName.ToUpper() != "INDEX_VALUE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT"
                    {

                        dtChartFrom.Columns.RemoveAt(col);
                    }
                }


                #region add extra column

                if (dtChartTo.Rows.Count > 0)
                {
                    if (dtChartTo.Rows.Count > 2)
                    {
                        // datatTble.Rows.RemoveAt(datatTble.Rows.Count - 1);
                        DataColumn dc = new DataColumn("Amount_D", System.Type.GetType("System.Double"));
                        dtChartTo.Columns.Add(dc);

                        for (int i = 0; i < dtChartTo.Rows.Count - 1; i++)
                        {

                            if (i == 0)
                            {
                                if (dtChartTo.Rows[i]["AMOUNT"] != DBNull.Value)
                                {
                                    dtChartTo.Rows[i]["Amount_D"] = (-1) * Convert.ToDouble(dtChartTo.Rows[i]["AMOUNT"]);
                                }
                                else
                                {
                                    dtChartTo.Rows[i]["Amount_D"] = dtChartTo.Rows[i]["AMOUNT"];
                                }
                            }
                            else
                            {
                                if (dtChartTo.Rows[i]["AMOUNT"] != DBNull.Value && dtChartTo.Rows[i - 1]["Amount_D"] != DBNull.Value)
                                {
                                    dtChartTo.Rows[i]["Amount_D"] = Convert.ToDouble(dtChartTo.Rows[i - 1]["Amount_D"]) + (-1) * Convert.ToDouble(dtChartTo.Rows[i]["AMOUNT"]);
                                }
                                else
                                {
                                    dtChartTo.Rows[i]["Amount_D"] = dtChartTo.Rows[i - 1]["Amount_D"];
                                }


                            }
                        }
                    }
                }


                #endregion


                for (int col = dtChartTo.Columns.Count - 1; col >= 0; col--)
                {
                    DataColumn dc = dtChartTo.Columns[col];
                    if (dc.ColumnName.ToUpper() != "TO_DATE" && dc.ColumnName.ToUpper() != "CUMILATIVE_AMOUNT_TO" && dc.ColumnName.ToUpper() != "AMOUNT_D")//&& dc.ColumnName.ToUpper() != "INDEX_VALUE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT"
                    {

                        dtChartTo.Columns.RemoveAt(col);
                    }
                }
                BindDataTableToChart(dtChartFrom, "FROM_DATE", chrtFromSchemeResult, "From");
                BindDataTableToChart(dtChartTo, "TO_DATE", chrtToSchemeResult, "To");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void BindDataTableToChart(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart chrt, string checkpoint)
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

                    if (dc.ColumnName.ToUpper() == "AMOUNT_D")
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

                //chrt.ChartAreas[0].AxisY.Maximum = Math.Round(Convert.ToDouble(maxval), 0) + 500;
                //if (minval < 1000)
                //    chrt.ChartAreas[0].AxisY.Minimum = 0;
                //else
                //    chrt.ChartAreas[0].AxisY.Minimum = Math.Round(Convert.ToDouble(minval), 0) - 500;

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


                chrt.ChartAreas[0].AxisY.Title = "Figure in Rs";
                chrt.ChartAreas[0].AxisX.Title = "STP Period";

                DateTime dtMax = (DateTime)_dt.AsEnumerable().Select(x => x[xField]).Max();
                DateTime dtMin = (DateTime)_dt.AsEnumerable().Select(x => x[xField]).Min();


                if (dtMax.Subtract(dtMin).Days > 365 && dtMax.Subtract(dtMin).Days < 1000)
                {
                    chrt.ChartAreas[0].AxisX.Interval = 6;
                    chrt.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chrt.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yy";
                }
                else if (dtMax.Subtract(dtMin).Days > 365)
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

                Title title = new Title();
                //title.Font = new Font("Trebuchet MS", 12, FontStyle.Bold);
                title.Font = new Font("Arial", 12, FontStyle.Bold);
                //title.ShadowOffset = 3;
                if (checkpoint == "From")
                {
                    title.Text = "Switch Out (" + ddlschtrf.SelectedItem.Text + ")";
                    chrtFromSchemeResult.Titles.Add(title);
                }
                else
                {
                    title.Text = "Switch In (" + ddlschtrto.SelectedItem.Text + ")";
                    chrtToSchemeResult.Titles.Add(title);
                }


                chrtArea.Visible = true;

                System.Web.UI.DataVisualization.Charting.Legend legend = chrt.Legends[0];
                legend.Font = new Font("Arial", 9, FontStyle.Bold);



            }
            catch (Exception ex)
            {
                Response.Write(@"'<script>alert('" + ex.Message + "'')</script>");
            }

        }
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
            //    Response.Redirect("http://mfiframes.mutualfundsindia.com/Principal/swpCalc.aspx");
            //}
            //if (ddlMode.SelectedValue == "STP")
            //{
            //}

            if (ddlMode.SelectedValue == "LumpSum")
            {
                Response.Redirect("returnCalc.aspx?return=lumpsum");
            }
            else if (ddlMode.SelectedValue == "SIP")
            {
                Response.Redirect("returnCalc.aspx?return=sip");
            }
            else if (ddlMode.SelectedValue == "SWP")
            {
                Response.Redirect("swpCalc.aspx");
            }
            else
            {
            }
        }

        #endregion



        #endregion
    }
}
