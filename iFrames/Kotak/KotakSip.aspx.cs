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
namespace iFrames.Kotak
{
    public partial class KotakSip : System.Web.UI.Page
    {
        #region Global declaration

        string connstr = ConfigurationManager.ConnectionStrings["connectionStringDSP"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        System.Data.DataTable finalResultdt = new System.Data.DataTable();
        System.Data.DataTable finalResultdtwobenchmark = new System.Data.DataTable();

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            //this.sipbtnshow.Attributes.Add("onclick", "javascript:return validate_SIP();");
            if (!IsPostBack)
            {
                FillDropdownScheme();                
            }
        }

        protected void btnSip_Click(object sender, EventArgs e)
        {
            CalculateReturn();
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            resultDiv.Visible = false;
            Reset();
        }

        #region: DropDown Method
        protected void ddlscheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SetIndexDropdown();
            //ListItem li = (ListItem)ViewState["ListItem"];
            FillDropdownIndex();
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


            }
        }

        public void SetIndexDropdown()
        {
            try
            {
                DataTable dt = FetchBenchMark(Convert.ToDecimal(ddlscheme.SelectedItem.Value));
                if (dt.Rows.Count > 0)
                {
                    //ddlsipbnmark.DataSource = dt;                    
                    ddlsipbnmark.DataTextField = "INDEX_NAME";
                    ddlsipbnmark.DataValueField = "INDEX_ID";
                    ddlsipbnmark.DataBind();
                }
            }
            catch (Exception ex)
            {
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


            }
        }
        #endregion


        public void FillDropdownScheme()
        {
            FillDropdown(ddlscheme);
        }

        public void FillDropdownIndex()
        {
            FillDropdownIndex(ddlsipbnmark);
        }


        #region: Fetch Methods

        public System.Data.DataTable FetchScheme()
        {
            //conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            try
            {

                using (var scheme = new PrincipalCalcDataContext())
                {
                    var fundtable = (from t_fund_masters in scheme.T_FUND_MASTERs
                                     where
                                       t_fund_masters.MUTUALFUND_ID == 37
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
                                         where s.T_FUND_MASTER.SUB_NATURE_ID != 2 && s.Nav_Check == 3
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
                        dt2 = scheme_name_1.ToDataTable();

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
                sipStartDate = Convert.ToDateTime(txtfromDate.Text);
                sipEndDate = Convert.ToDateTime(txtToDate.Text);
                sipAsonDate = Convert.ToDateTime(txtvalason.Text);
                schmeId = ddlscheme.SelectedItem.Value;
                dblSIPamt = Convert.ToDouble(txtinstall.Text);

                if (ViewState["INDEX_ID"] != null)
                    indexId = ViewState["INDEX_ID"].ToString();
                else
                    indexId = ddlsipbnmark.SelectedItem.Value;

                #region Inception Date


                using (var principl = new PrincipalCalcDataContext())
                {
                    var alotdate = from ind in principl.T_SCHEMES_MASTERs
                                   where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                   select ind.Launch_Date;

                    schmStartDate = Convert.ToDateTime(alotdate.Single().ToString());
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

                using (var principl = new PrincipalCalcDataContext() { CommandTimeout = 6000 })
                {
                    IMultipleResults datatble = principl.MFIE_SP_SIP_CALCULATER(schmeId, investmentDate, sipEndDate, sipAsonDate, dblSIPamt, strFrequency, strInvestorType, 0, 0, null, 1);
                    var firstTable = datatble.GetResult<CalcReturnData2>();
                    ////firstTable.TableName = "sdfsaf";
                    var secondTable = datatble.GetResult<CalcReturnData>();
                    SipDtable1 = firstTable.ToDataTable();
                    SipDtable2 = secondTable.ToDataTable();
                    SipDtable2.Columns.RemoveAt(0);
                }
                
              
                #region Launch Date
                using (var principl = new PrincipalCalcDataContext())
                {
                    var allotdate = from ind in principl.T_SCHEMES_MASTERs
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
                    if (SipDtable1.Rows.Count > 2)
                    {
                        SipDtable1.Rows.RemoveAt(SipDtable1.Rows.Count - 1);
                        DataColumn dc = new DataColumn("Amount", System.Type.GetType("System.Double"));
                        SipDtable1.Columns.Add(dc);
                        SipDtable1.Columns.Add( new DataColumn("Index_Value_amount", System.Type.GetType("System.Double")));
                        SipDtable1.Columns.Add( new DataColumn("Index_unit_cumulative", System.Type.GetType("System.Double")));
                        double result;
                        for (int i = 0; i < SipDtable1.Rows.Count - 1; i++)
                        {
                            
                            if (i == 0)
                            {
                                SipDtable1.Rows[i]["Amount"] = (-1) * Convert.ToDouble(SipDtable1.Rows[i]["Scheme_cashflow"]);

                                if (Double.TryParse(SipDtable1.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(SipDtable1.Rows[i]["Index_Unit"].ToString(), out result))
                                {
                                    SipDtable1.Rows[i]["Index_unit_cumulative"] = SipDtable1.Rows[i]["Index_Unit"];
                                    SipDtable1.Rows[i]["Index_Value_amount"] = Convert.ToDouble(SipDtable1.Rows[i]["Index_Value"]) * Convert.ToDouble(SipDtable1.Rows[i]["Index_unit_cumulative"]);
                                }
                            }
                            else
                            {
                                SipDtable1.Rows[i]["Amount"] = Convert.ToDouble(SipDtable1.Rows[i - 1]["Amount"]) + (-1) * Convert.ToDouble(SipDtable1.Rows[i]["Scheme_cashflow"]);

                                if (Double.TryParse(SipDtable1.Rows[i]["Index_Value"].ToString(), out result) && Double.TryParse(SipDtable1.Rows[i]["Index_Unit"].ToString(), out result))
                                {
                                    SipDtable1.Rows[i]["Index_unit_cumulative"] = Convert.ToDouble(SipDtable1.Rows[i]["Index_Unit"]) + Convert.ToDouble(SipDtable1.Rows[i - 1]["Index_unit_cumulative"]);
                                    SipDtable1.Rows[i]["Index_Value_amount"] = Convert.ToDouble(SipDtable1.Rows[i]["Index_Value"]) * Convert.ToDouble(SipDtable1.Rows[i]["Index_unit_cumulative"]);
                                }
                            }
                        }
                    }
                    sipGridView.DataSource = SipDtable1;
                    sipGridView.DataBind();

                    #region chart
                    DataTable dtChart = new DataTable("dtChart");
                    dtChart = SipDtable1.Copy();
                    dtChart.Rows.RemoveAt(dtChart.Rows.Count -1);

                
                    for (int col = dtChart.Columns.Count - 1; col >= 0; col-- )
                    {
                        DataColumn dc = dtChart.Columns[col];
                        if (dc.ColumnName.ToUpper() != "NAV_DATE" && dc.ColumnName.ToUpper() != "CUMULATIVE_AMOUNT" && dc.ColumnName.ToUpper() != "AMOUNT")// && dc.ColumnName.ToUpper() != "INDEX_VALUE_AMOUNT") 
                        {
                            //dtChart.Columns.Remove(dc);
                            dtChart.Columns.RemoveAt(col);
                        }
                    }

                    

                    BindDataTableToChart(dtChart, chrtResult);
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

                if (resultDiv.Visible == false)
                    resultDiv.Visible = true;
                

            }
            catch (Exception ex)
            {
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
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }


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
            resultDiv.RenderControl(hw); 
            objFinalstr.Append(sw.ToString());            
            Response.Write(objFinalstr.ToString());
            Response.Flush();
            Response.End();


        }



        public void  BindDataTableToChart(DataTable _dt, System.Web.UI.DataVisualization.Charting.Chart chrt)
        {
            string columnName = null;
            foreach (DataColumn dc in _dt.Columns)
            {
                if (dc.ColumnName == "NAV_DATE")
                {
                    continue;
                }
                columnName = dc.ColumnName;
                chrt.Series.Add(columnName);
                chrt.Series[columnName].Points.DataBindXY(_dt.DefaultView, "Nav_Date", _dt.DefaultView, columnName) ;
                chrt.Series[columnName].IsValueShownAsLabel = true;
                chrt.Series[columnName].ChartType = SeriesChartType.Line;
                chrt.Series[columnName].BorderWidth = 1;


                //.Column()
                //chrt.Legends(0).Enabled = True
            }

            chrt.Visible = true;

            foreach (Series ser in chrt.Series)
            {
                ser.ShadowOffset = 1;
                // ser.BorderWidth = 3
                ser.ChartType = SeriesChartType.Spline;
                ser.IsValueShownAsLabel = false;
                ser["LabelStyle"] = "Top";
            }

            //chrt.Titles.Add("Scheme Graph")


            double maxval = 1;
            double minval = 10000;


            for (int k = 0; k <= _dt.Rows.Count - 1; k++)
            {
                foreach (DataColumn dc in _dt.Columns)
                {
                    if (dc.ColumnName.ToUpper() != "NAV_DATE" && !string.IsNullOrEmpty(_dt.Rows[k][dc].ToString()))
                    {
                        if (Convert.ToDouble(_dt.Rows[k][dc]) > maxval)
                        {
                            maxval = Convert.ToDouble(_dt.Rows[k][dc]);
                        }

                        if (Convert.ToDouble(_dt.Rows[k][dc]) < minval)
                        {
                            minval = Convert.ToDouble(_dt.Rows[k][dc]);
                        }
                    }

                }
            }





            //chrt.ChartAreas(0).AxisX.Title = "Dates"
            chrt.ChartAreas[0].AxisY.Title = "Figure in Rs";
            chrt.ChartAreas[0].AxisX.Title = "SIP Period";

            var chrtArea = chrt.ChartAreas[0];
            chrtArea.AxisX.MajorGrid.LineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.NotSet;
            chrtArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
            chrtArea.Visible = true;

            chrt.ChartAreas[0].AxisY.IntervalOffsetType = DateTimeIntervalType.Number;
            chrt.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;

            //chrt.Legends.Add("Legend")
            // chrt.Legends(0).Alignment = StringAlignment.Far
            //chrt.Legends(0).Position

            //'chrt.ChartAreas(0).AxisY.in
             //chrt.Legends.Add(new Legend("Default") {Docking = Docking.Bottom   }) ;
             Legend legend = chrt.Legends[0];
             //LegendItem item = new LegendItem();
             //item.SeparatorColor = System.Drawing.Color.White;
             //item.Name = "TestLegend";
             //item.BorderWidth = 4;
             //item.ShadowOffset = 1;
             //chrt.Legends[0].CustomItems.Add(item);

             // Customize the legend appearance.
             legend.BackColor = Color.Beige;
            
             legend.Font = new Font("Arial", 9, FontStyle.Bold);
            




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

            chrt.ChartAreas[0].AxisY.Maximum = Convert.ToInt32(maxval) + 500;
            chrt.ChartAreas[0].AxisY.Minimum = Convert.ToInt32(minval) - 500;



        }


    }
    
}