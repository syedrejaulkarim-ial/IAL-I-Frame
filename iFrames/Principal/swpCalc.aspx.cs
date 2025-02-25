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

namespace iFrames.Principal
{
    public partial class swpCalc : System.Web.UI.Page
    {
        #region Global declaration

        string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        DataTable finalResultdt = new DataTable();

        #endregion

        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            this.swpbtnshow.Attributes.Add("onclick", "javascript:return validate_SWP();");
            if (!IsPostBack)
            {
                FillDropdownScheme();
            }
            //  ddlscname.Attributes.Add("onmouseover", "this.text=this.options[this.selectedIndex].text");

            //ddlscname.Attributes.Add("onfocusout", String.Format("focusOut('{0}',{1})", ddlscname.ClientID, ddlscname.Width.Value));
            //ddlscname.Attributes.Add("onmouseenter", String.Format("mouseEnter('{0}',{1})", ddlscname.ClientID, ddlscname.Width.Value));


            SetInceptionOnDropDown();
        }

        #endregion

        #region Button Event
        protected void swpbtnshow_Click(object sender, ImageClickEventArgs e)
        {
            CalculateReturn();

        }

        protected void swpbtnreset_Click(object sender, ImageClickEventArgs e)
        {
            resultDiv.Visible = false;
            Reset();
        }


        #endregion

        #region: DropDown Method

        protected void ddlscname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (resultDiv.Visible) resultDiv.Visible = false;
            if (TableNote.Visible) TableNote.Visible = false;
            SWPSchDt.Text = "";

            FillDropdownIndex();

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

                    SWPSchDt.Text = "<b>" + Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "<b/>";
                }
            }
            #endregion
        }

        protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowRelativeDiv();
        }

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
            try
            {

                using (var scheme = new PrincipalCalcDataContext())
                {
                    var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                     where
                                       t_fund_masters.MUTUALFUND_ID == 15 && t_fund_masters.SUB_NATURE_ID!=2
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
                                         s.Nav_Check == 3  // for growth option & DSP BlackRock Equity Fund added additionaly
                                         && 
                                         s.Option_Id==2
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

        public void CalculateReturn()
        {
            #region Initialize

            DateTime swpStartDate, swpEndDate, swpAsonDate, tempDate, investmentDate, allotDate, initialStartDate;

            int swpDate, tempInt;
            double dblSwpTransferAmnt, dblswpIntialAmnt;
            string strFrequency = string.Empty, schemeId = string.Empty, strInvestorType = string.Empty;
            SqlCommand cmdswp = null;
            DataSet dSet = new DataSet("swpDataSet");
            DataTable dtble = new DataTable("swpDataTable"), datatTble = new DataTable("swpDT");


            conn.ConnectionString = connstr;

            #endregion


            try
            {
                #region set Value


                swpStartDate = new DateTime(Convert.ToInt16(txtwfrdt.Text.Split('/')[2]),
                                      Convert.ToInt16(txtwfrdt.Text.Split('/')[1]),
                                      Convert.ToInt16(txtwfrdt.Text.Split('/')[0]));

                swpEndDate = new DateTime(Convert.ToInt16(txtwtdt.Text.Split('/')[2]),
                                         Convert.ToInt16(txtwtdt.Text.Split('/')[1]),
                                         Convert.ToInt16(txtwtdt.Text.Split('/')[0]));

                swpAsonDate = new DateTime(Convert.ToInt16(txtwvaldate.Text.Split('/')[2]),
                                     Convert.ToInt16(txtwvaldate.Text.Split('/')[1]),
                                     Convert.ToInt16(txtwvaldate.Text.Split('/')[0]));



                dblSwpTransferAmnt = Convert.ToDouble(txtwtramt.Text);
                dblswpIntialAmnt = Convert.ToDouble(txtwinamt.Text);
                swpDate = Convert.ToInt32(ddSWPdate.SelectedItem.Text);
                // strFrequency = txtddwperiod.Text;
                strFrequency = ddwperiod.SelectedItem.Text;
                schemeId = ddlscname.SelectedItem.Value;
                strInvestorType = "Individual/Huf";

                #endregion

                initialStartDate = swpStartDate;


                #region condition check

                tempInt = swpStartDate.Day;
                if (swpDate < tempInt)
                {
                    if (swpStartDate.Month != 12)
                        investmentDate = new DateTime(swpStartDate.Year, swpStartDate.Month + 1, swpDate);
                    else
                        investmentDate = new DateTime(swpStartDate.Year + 1, 1, swpDate);
                }
                else if (swpDate == tempInt)
                {
                    investmentDate = swpStartDate;
                }
                else
                {
                    investmentDate = new DateTime(swpStartDate.Year, swpStartDate.Month, swpDate);
                }
                #endregion

                # region calling SP


                cmdswp = new SqlCommand("MFIE_SP_SWP_Calculater", conn);
                cmdswp.CommandType = CommandType.StoredProcedure;
                cmdswp.Parameters.Add(new SqlParameter("@Scheme_Ids", schemeId));
                cmdswp.Parameters.Add(new SqlParameter("@Plan_Start_Date", investmentDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));//swpStartDate
                cmdswp.Parameters.Add(new SqlParameter("@Plan_End_Date", swpEndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdswp.Parameters.Add(new SqlParameter("@Report_As_On", swpAsonDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdswp.Parameters.Add(new SqlParameter("@SWP_Amt", dblSwpTransferAmnt));
                cmdswp.Parameters.Add(new SqlParameter("@Frequency", strFrequency));
                cmdswp.Parameters.Add(new SqlParameter("@Dividend_Type", strInvestorType));
                cmdswp.Parameters.Add(new SqlParameter("@Investment_Amount", dblswpIntialAmnt));
                cmdswp.Parameters.Add(new SqlParameter("@Investment_Date", initialStartDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

                SqlDataAdapter da = new SqlDataAdapter(cmdswp);
                da.Fill(dSet);

                if (dSet.Tables.Count > 0)
                {
                    datatTble = dSet.Tables[0];
                    ViewState["swpDataTable"] = datatTble;
                    if (dSet.Tables.Count > 1)
                        dtble = dSet.Tables[1];


                    if (datatTble.Rows.Count > 2)
                    {
                        datatTble.Rows.RemoveAt(datatTble.Rows.Count - 1);
                    }

                    GridViewSWPResult.DataSource = datatTble;
                    GridViewSWPResult.DataBind();
                    GridViewSWPResult.Visible = true;

                    GridViewSWPSummary.DataSource = dtble;
                    GridViewSWPSummary.DataBind();
                    GridViewSWPSummary.Visible = true;
                }

                #endregion

                #region Launch Date
                using (var principl = new PrincipalCalcDataContext())
                {
                    var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                    where ind.Scheme_Id == Convert.ToDecimal(schemeId)
                                    select new
                                    {
                                        LaunchDate = ind.Launch_Date
                                    };
                    // SWPSchDt.Text = "";
                    if (allotdate != null && allotdate.Count() > 0)
                    {
                        allotDate = Convert.ToDateTime(allotdate.Single().LaunchDate);
                        // SWPSchDt.Text = "<b>" + allotDate.ToShortDateString() + "<b/>";
                    }
                }
                #endregion

                #region chart


                #region add extra column
                if (datatTble.Rows.Count > 0)
                {
                    if (datatTble.Rows.Count > 2)
                    {
                        // datatTble.Rows.RemoveAt(datatTble.Rows.Count - 1);
                        DataColumn dc = new DataColumn("Amount", System.Type.GetType("System.Double"));
                        datatTble.Columns.Add(dc);
                        // datatTble.Columns.Add(new DataColumn("Index_Value_amount", System.Type.GetType("System.Double")));
                        // datatTble.Columns.Add(new DataColumn("Index_unit_cumulative", System.Type.GetType("System.Double")));
                        double result;
                        for (int i = 0; i < datatTble.Rows.Count - 1; i++)
                        {

                            if (i == 0)
                            {
                                datatTble.Rows[i]["Amount"] = Convert.ToDouble(datatTble.Rows[i]["INVST_AMOUNT"]);

                                //if (Double.TryParse(datatTble.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(datatTble.Rows[i]["INDEX_UNIT_CUMULATIVE"].ToString(), out result))
                                //{
                                //    // datatTble.Rows[i]["Index_unit_cumulative"] = datatTble.Rows[i]["Index_Unit"];
                                //    datatTble.Rows[i]["Index_Value_amount"] = Math.Round(Convert.ToDouble(datatTble.Rows[i]["Index_Value"]) * Convert.ToDouble(datatTble.Rows[i]["INDEX_UNIT_CUMULATIVE"]), 2);
                                //}
                            }
                            else
                            {
                                datatTble.Rows[i]["Amount"] = Convert.ToDouble(datatTble.Rows[i - 1]["Amount"]) + (-1) * Convert.ToDouble(datatTble.Rows[i]["INVST_AMOUNT"]);

                                //if (Double.TryParse(datatTble.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(datatTble.Rows[i]["INDEX_UNIT_CUMULATIVE"].ToString(), out result))
                                //{

                                //    datatTble.Rows[i]["Index_Value_amount"] = Math.Round(Convert.ToDouble(datatTble.Rows[i]["Index_Value"]) * Convert.ToDouble(datatTble.Rows[i]["INDEX_UNIT_CUMULATIVE"]), 2);
                                //}
                            }
                        }
                    }
                }


                #endregion



                #region Remove Bonus Row For Graph Calculation

                if (datatTble.Rows.Count > 2)
                {
                    for (int i = datatTble.Rows.Count - 2; i >= 0; i--)
                    {
                        if (datatTble.Columns.Contains("INVST_AMOUNT")) 
                        {
                            if (datatTble.Rows[i]["INVST_AMOUNT"].ToString().Trim() != string.Empty && datatTble.Rows[i]["INVST_AMOUNT"].ToString().Trim() == "0")
                        {
                            datatTble.Rows.RemoveAt(i);
                        }
                        }
                    }

                }



                #endregion



                DataTable dtChart = new DataTable("dtChart");
                dtChart = datatTble.Copy();
                dtChart.Rows.RemoveAt(dtChart.Rows.Count - 1);

                for (int col = dtChart.Columns.Count - 1; col >= 0; col--)
                {
                    DataColumn dc = dtChart.Columns[col];
                    if (dc.ColumnName.ToUpper() != "DATE" && dc.ColumnName.ToUpper() != "CUMILATIVE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT")
                    {

                        dtChart.Columns.RemoveAt(col);
                    }
                }


                BindDataTableToChart(dtChart, "DATE", chrtResult);

                if (CheckBoxChart.Checked)
                    divshowChart.Style.Add("display", "inline");
                else
                    divshowChart.Style.Add("display", "none");

                divshowChart.Visible = true;
                resultDiv.Visible = true;
                #endregion

                if (TableNote.Visible == false) TableNote.Visible = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }

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
            if (ddlMode.SelectedValue == "LumpSum")
            {
                Response.Redirect("returnCalc.aspx?return=lumpsum");
            }
            else if (ddlMode.SelectedValue == "SIP")
            {
                Response.Redirect("returnCalc.aspx?return=sip");
            }
            else if (ddlMode.SelectedValue == "STP")
            {
                Response.Redirect("stpCalc.aspx");
            }
            else
            {
            }
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
            ddwperiod.SelectedIndex = 0;
            ddSWPdate.SelectedIndex = 0;
            txtwinamt.Text = "";
            txtwtramt.Text = "";
            txtwfrdt.Text = "";
            txtwtdt.Text = "";
            txtwvaldate.Text = "";
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

                chrt.ChartAreas[0].AxisY.Maximum = Math.Round((Math.Round(Convert.ToDouble(maxval), 0) + 500)/10,0)*10;
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
        

        #endregion

        
    }
}