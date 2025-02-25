using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Data.Linq;
using iFrames.DAL;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Collections;
using WkHtmlToXSharp;
using NUnit.Framework;

namespace iFrames.Pages
{
    public partial class BigDaySIP : System.Web.UI.Page
    {

        #region : Global data

        double AnnualSensxAmnt = 0;// use for showing negative color for sensex annual decrement
        double AnnualSensxAmnt1 = 0;
        bool _lessAyear;
        string returnAmountSx = string.Empty;
        string returnAmountFd = string.Empty;
        ArrayList arr = new ArrayList();

        public string DependOn = string.Empty;
        public string logo = string.Empty;

        #region: PDF Global Variable BY Abhishek Das

        private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string SimplePageFile = null;
        public static int count = 0;

        #endregion

        #endregion

        #region : Page Event

        protected void Page_Load(object sender, EventArgs e)
        {

            showResultDiv.Visible = false;
            highlightPanel.Visible = false;
            // WeddingDiv.Visible = false;
            //BirthDateDiv.Visible = false;
            //ColorTablediv.Visible = false;
            //OutputTablediv.Visible = false;
            OutputDiv.Visible = false;
            disclamerDiv.Visible = false;
            //Textvalsensex_15.Text = Textvalsensex_18.Text = Textvalsensex_21.Text = Textvalsensex_25.Text = null;
            //Textvalfd_15.Text = Textvalfd_18.Text = Textvalfd_21.Text = Textvalfd_25.Text = null;
            // returnsensex15Cagr.Text = returnsensex18Cagr.Text = returnsensex21Cagr.Text = returnsensex25Cagr.Text = null;
            //returnfd15Cagr.Text = returnfd18Cagr.Text = returnfd21Cagr.Text = returnfd25Cagr.Text = null;
            _lessAyear = false;

            if (!Page.IsPostBack)
            {
                advisorTxt.Text = "Advisor Name";
                txtAdvisorname.Text = "Advisor Company Name";

                //********************** Delete Previous Files from Cache Folder****************************
                //string pdfpath = string.Empty;
                //if (ViewState["selectedval"] != null)
                //if (Session["GUID"] != null)
                //{
                //    pdfpath = HttpContext.Current.Server.MapPath("~") + "\\Pages\\cache_pdf\\BigDaySIP_" + Convert.ToString(Session["GUID"]) + ".pdf";
                //    if (File.Exists(pdfpath))
                //    {
                //        File.Delete(pdfpath);
                //    }
                //}
                //*******************************Enbd************************************************************

            }

            //btnLinkBack.Visible = false;
            //foreach (ListItem RadioButton in RadioButtonList1.Items)
            //{ RadioButton.Attributes.Add("onclick", "return date_validate();"); }

            //RadioButtonList1.Attributes.Add("onclick", "return date_validate();");


        }
        #endregion

        #region: Data retrieved by LINQ

        public DataTable GetDataThruLinq(DateTime strdate, DateTime enddate, Double amnt, ref Double sensexVal, ref Double fdVal)
        {
            decimal? fdValMax = null, sensexValMax = null, investedAmount = null;
            DateTime Senddate;
            int year_diff, month_diff;
            double valsensex_15, valsensex_18, valsensex_21, valsensex_25;
            double valfd_15, valfd_18, valfd_21, valfd_25;
            Double fdCagr15 = 0, SensexCagr15 = 0, fdCagr18 = 0, SensexCagr18 = 0, fdCagr21 = 0, SensexCagr21 = 0, fdCagr25 = 0, SensexCagr25 = 0;

            XmlDocument xmldoc = new XmlDocument();
            string xmlpath = Server.MapPath("~");
            xmlpath += "Images\\Excel\\Fdreturn.xml";
            xmldoc.Load(xmlpath);
            SqlXml xmlparameter = new SqlXml(new XmlTextReader(xmldoc.InnerXml, XmlNodeType.Document, null));
            //string s = xmlparameter.Value;
            XElement xmlelement = XElement.Parse(xmlparameter.Value);
            DataTable resultDataTable = new DataTable();

            DataTable SipDtable = new DataTable();
            SipDtable.Columns.Add("HeaderColumn", typeof(String));
            SipDtable.Columns.Add("SipValue", typeof(String));
            SipDtable.Columns.Add("SipCagrValue", typeof(String));
            SipDtable.Columns.Add("FdValue", typeof(String));
            SipDtable.Columns.Add("FdCagrValue", typeof(String));


            using (var pramrica = new PramericaDataContext())
            {
                try
                {
                    IMultipleResults dtb1 = pramrica.MFI_SIP_CALC_PRAMERICA(strdate, enddate, Convert.ToDecimal(amnt), xmlelement, ref investedAmount, ref  sensexValMax, ref fdValMax);
                    sensexVal = Convert.ToDouble(sensexValMax);
                    fdVal = Convert.ToDouble(fdValMax);
                    resultDataTable = dtb1.GetResult<SpPramericaReturnData>().ToDataTable();//get the data


                    if (resultDataTable.Rows.Count > 0)
                    {
                        //PRIndexListView.DataSource = resultDataTable;
                        // PRIndexListView.DataBind();
                        PRIndexListViewGrid.DataSource = resultDataTable;
                        PRIndexListViewGrid.DataBind();

                    }
                    else
                    {
                        resultDataTable.Rows.Clear();
                        PRIndexListViewGrid.DataSource = null;
                        PRIndexListViewGrid.DataBind();
                    }


                    if (resultDataTable.Rows.Count >= 15)
                    {
                        //abv15.Style.Add("Visible","true");// == false) abv15.Visible = true;
                        //abv15.Visible = true;
                        valsensex_15 = Convert.ToDouble(resultDataTable.Rows[14][2]);
                        valfd_15 = Convert.ToDouble(resultDataTable.Rows[14][3]);

                        //Textvalsensex_15.Text = valsensex_15.ToString("n0");// "<img src='../Images/rupee.gif'/> "
                        //Textvalfd_15.Text = valfd_15.ToString("n0");

                        Senddate = strdate.AddMonths(12 * 15);

                        GetCagrValue(strdate, Senddate, amnt, valsensex_15, ref SensexCagr15);
                        GetCagrValue(strdate, Senddate, amnt, valfd_15, ref fdCagr15);
                        //returnsensex15Cagr.Text = SensexCagr15.ToString();
                        //returnfd15Cagr.Text = fdCagr15.ToString();

                        SipDtable.Rows.Add("<b>On 15th Anniversary</b>", indiarupeeformat(valsensex_15.ToString()), SensexCagr15.ToString("n2"), indiarupeeformat(valfd_15.ToString()), fdCagr15.ToString("n2"));

                    }
                    else
                    {
                        //if (abv15.Visible == true) abv15.Visible = false;
                    }



                    if (resultDataTable.Rows.Count >= 18)
                    {
                        //if (abv18.Visible == false) abv18.Visible = true;
                        valsensex_18 = Convert.ToDouble(resultDataTable.Rows[17][2]);
                        valfd_18 = Convert.ToDouble(resultDataTable.Rows[17][3]);

                        Senddate = strdate.AddMonths(12 * 18);
                        //Textvalsensex_18.Text = valsensex_18.ToString("n0");// "<img src='../Images/rupee.gif'/> "
                        // Textvalfd_18.Text = valfd_18.ToString("n0");


                        GetCagrValue(strdate, Senddate, amnt, valsensex_18, ref SensexCagr18);
                        GetCagrValue(strdate, Senddate, amnt, valfd_18, ref fdCagr18);
                        //returnsensex18Cagr.Text = SensexCagr18.ToString();
                        //returnfd18Cagr.Text = fdCagr18.ToString();

                        SipDtable.Rows.Add("<b>On 18th Birthday</b>", indiarupeeformat(valsensex_18.ToString()), SensexCagr18.ToString("n2"), indiarupeeformat(valfd_18.ToString()), fdCagr18.ToString("n2"));

                    }
                    else
                    {
                        // if (abv18.Visible == true) abv18.Visible = false;
                    }


                    if (resultDataTable.Rows.Count >= 21)
                    {
                        //if (abv21.Visible == false) abv21.Visible = true;
                        valsensex_21 = Convert.ToDouble(resultDataTable.Rows[20][2]);
                        valfd_21 = Convert.ToDouble(resultDataTable.Rows[20][3]);

                        Senddate = strdate.AddMonths(12 * 21);
                        if (valsensex_21 != null && valfd_21 != null)
                        {
                            //Textvalsensex_21.Text = valsensex_21.ToString("n0");
                            // Textvalfd_21.Text = valfd_21.ToString("n0");



                            GetCagrValue(strdate, Senddate, amnt, valsensex_21, ref SensexCagr21);
                            GetCagrValue(strdate, Senddate, amnt, valfd_21, ref fdCagr21);
                            //returnsensex21Cagr.Text = SensexCagr21.ToString();
                            //returnfd21Cagr.Text = fdCagr21.ToString();
                            SipDtable.Rows.Add("<b>On 21st Birthday</b>", indiarupeeformat(valsensex_21.ToString()), SensexCagr21.ToString("n2"), indiarupeeformat(valfd_21.ToString()), fdCagr21.ToString("n2"));

                        }

                    }
                    else
                    {
                        //if (abv21.Visible == true) abv21.Visible = false;
                    }


                    if (resultDataTable.Rows.Count >= 25)
                    {
                        //if (abv25.Visible == false) abv25.Visible = true;
                        valsensex_25 = Convert.ToDouble(resultDataTable.Rows[24][2]);
                        valfd_25 = Convert.ToDouble(resultDataTable.Rows[24][3]);

                        if (valsensex_25 != null && valfd_25 != null)
                        {
                            //Textvalsensex_25.Text = valsensex_25.ToString("n0");
                            // Textvalfd_25.Text = valfd_25.ToString("n0");

                            Senddate = strdate.AddMonths(12 * 25);
                            GetCagrValue(strdate, Senddate, amnt, valsensex_25, ref SensexCagr25);
                            GetCagrValue(strdate, Senddate, amnt, valfd_25, ref fdCagr25);
                            //returnsensex25Cagr.Text = SensexCagr25.ToString();
                            // returnfd25Cagr.Text = fdCagr25.ToString();

                            SipDtable.Rows.Add("<b>On 25th Anniversary</b>", indiarupeeformat(valsensex_25.ToString()), SensexCagr25.ToString("n2"), indiarupeeformat(valfd_25.ToString()), fdCagr25.ToString("n2"));

                        }

                    }
                    else
                    {
                        //if (abv21.Visible == true) abv21.Visible = false;
                    }

                    //SipGridView.DataSource = SipDtable;
                    //SipGridView.DataBind();

                }
                catch (Exception exc)
                {
                }


            }

            double Total_month_diff = (double)investedAmount / amnt;
            Total_month_diff = Total_month_diff - 1;
            //year_diff = Convert.ToInt16(Math.Truncate((((double)investedAmount / amnt) / 12)));
            //month_diff = Convert.ToInt16(((double)investedAmount / amnt) % 12);

            year_diff = (Int16)Math.Truncate(Total_month_diff / 12);
            month_diff = (Int16)Total_month_diff % 12;

            investmentPeriod.Text = year_diff.ToString() + " Years " + month_diff.ToString() + " Months";



            //investmentPeriod.Text = TimePeriod(strdate);
            if (year_diff < 1)
            {
                showResultDiv.Visible = false;
                _lessAyear = true;

            }

            //investedAmountTotal.Text = investedAmount.ToString();
            // investedAmountTotal.Text = Convert.ToDouble(investedAmount).ToString("n0");
            //investedAmountTotal.Text = String.Format("{0:##,##,##,##0;(##,##,##,##0);Zero}", Convert.ToDouble(investedAmount));
            //investedAmountTotal.Text = indiarupeeformat(investedAmount.ToString());
            investedAmountTotal.Text = Math.Round((Convert.ToDouble(investedAmount.ToString()) / 100000.00), 2).ToString() + "  Lakhs";
            ViewState["investedAmountTotal"] = investedAmountTotal.Text;

            //investedAmountTotal.Text = string.Format("{###,##}", investedAmount);
            //investedAmountTotal.Text = investedAmountTotal.Text.ToString("n0");
            returnAmountSx = sensexValMax.ToString();
            //todaySensexReturn.Text = returnAmountSx.Text = sensexValMax.ToString();
            returnAmountFd = fdValMax.ToString();
            //todayFdReturn.Text =  returnAmountFd.Text = fdValMax.ToString();
            return SipDtable;


        }


        #endregion

        #region: CAGR Calculation
        public void GetCagrValue(DateTime strdate, DateTime enddate, Double amnt, Double schemeLastValue, ref Double scheme)
        {

            try
            {
                List<DateTime> datearray = new List<DateTime>();
                List<Double> doublearray = new List<Double>();

                while (strdate < enddate)
                {
                    datearray.Add(strdate);
                    doublearray.Add(amnt);
                    //dataTable.Rows.Add(strdate, amnt);
                    strdate = strdate.AddMonths(1);
                }
                schemeLastValue = schemeLastValue * (-1);
                datearray.Add(enddate);
                doublearray.Add(schemeLastValue);
                datearray.Reverse();   //as reversed in utilities.cs
                doublearray.Reverse();
                scheme = Math.Round(Utilities.XIRR(doublearray.ToArray(), datearray.ToArray()) * 100, 2);

            }
            catch (Exception exc)
            {
            }
        }
        #endregion

        #region : Click Event

        protected void btnSipclick(object sender, EventArgs e)
        {
            //********************** Delete Previous Files from Cache Folder****************************
            string pdfpath = string.Empty;
            if (ViewState["selectedval"] != null)
            {
                pdfpath = Convert.ToString(ViewState["selectedval"]);
                if (File.Exists(pdfpath))
                {
                    File.Delete(pdfpath);
                }
            }
            //*******************************Enbd************************************************************

            SelectedRadioIndexChanged();
        }

        protected void SelectedRadioIndexChanged()
        {

            try
            {

                //DateTime start_Date = new DateTime(Convert.ToInt16(StartDate.Text.Split('-')[2]),
                //                         Convert.ToInt16(StartDate.Text.Split('-')[1]),
                //                         Convert.ToInt16(StartDate.Text.Split('-')[0]));


                DateTime start_Date = Convert.ToDateTime(StartDate.Text.Trim());

                if (start_Date.Year == 1980 && start_Date.Month == 1 && start_Date.Day == 1)
                {
                    start_Date = start_Date.AddDays(1);
                }

                DateTime end_Date = DateTime.Today; //Todays Date
                Double amount = Convert.ToDouble(Amount.Text.Split(' ')[0]);
                DataTable dtable;

                TimeSpan daydiff = end_Date.Subtract(start_Date);
                //TimeSpan daydiff = end_Date.Month - start_Date.Month;

                int daybetween = daydiff.Days;

                Double fdCagr = 0, SensexCagr = 0, sensexVal = 0, fdVal = 0;
                dtable = GetDataThruLinq(start_Date, end_Date, amount, ref sensexVal, ref fdVal);


                GetCagrValue(start_Date, end_Date, amount, sensexVal, ref SensexCagr);
                GetCagrValue(start_Date, end_Date, amount, fdVal, ref fdCagr);


                // returnSensexCagrWD.Text = returnSensexCagrBD.Text = SensexCagr.ToString();
                // returnFdCagrWD.Text = returnFdCagrBD.Text = fdCagr.ToString();
                //returnSensexCagr1.Text = returnSensexCagr.Text;
                // returnFdCagr1.Text = returnFdCagr.Text;

                //advisorTxt.Text = txtAdvisorname.Text;
                var dr = dtable.NewRow();
                //dtable.Rows.Add("Today", sensexVal.ToString("n0"), SensexCagr.ToString(), fdVal.ToString("n0"), fdCagr.ToString());
                //dr["HeaderColumn"] = "<b>Today</b>";
                //dr["SipValue"] = sensexVal.ToString("n0");
                //dr["SipCagrValue"] = SensexCagr.ToString("n2");
                //dr["FdValue"] = fdVal.ToString("n0");
                //dr["FdCagrValue"] = fdCagr.ToString("n2");

                string todate = string.Empty;
                todate = System.DateTime.Now.ToString("dd-MMMM-yyyy");

                dr["HeaderColumn"] = "<b>Today<span style='font-size:12px;font-family:Verdana;'>(" + todate + ")</span></b>";
                dr["SipValue"] = indiarupeeformat(sensexVal.ToString());
                dr["SipCagrValue"] = SensexCagr.ToString("n2");
                dr["FdValue"] = indiarupeeformat(fdVal.ToString());
                dr["FdCagrValue"] = fdCagr.ToString("n2");

                //dtable.Rows.Add("Today", sensexVal.ToString("n0"), SensexCagr.ToString(), fdVal.ToString("n0"), fdCagr.ToString());
                //dtable.Rows.InsertAt(dr, 0);
                // dtable.Rows.RemoveAt(5);
                // dtable.Rows.Add(dr);
                dtable.Rows.InsertAt(dr, 0);

                DataTable birthDaTble;//= new DataTable();
                DataTable wedDaTble;//= new DataTable();
                birthDaTble = (from b in dtable.AsEnumerable()
                               where b.Field<string>("HeaderColumn").Contains("Birthday") || b.Field<string>("HeaderColumn").Contains("Today")
                               select b).CopyToDataTable();
                wedDaTble = (from b in dtable.AsEnumerable()
                             where b.Field<string>("HeaderColumn").Contains("Anniversary") || b.Field<string>("HeaderColumn").Contains("Today")
                             select b).CopyToDataTable();




                if (showResultDiv.Visible == false)
                    showResultDiv.Visible = true;

                if (_lessAyear)
                    showResultDiv.Visible = false;

                if (disclamerDiv.Visible == false)
                    disclamerDiv.Visible = true;

                //if (btnLinkBack.Visible == false)
                //    btnLinkBack.Visible = true;


                // ColorTablediv.Visible = true;
                if (RadioButtonList1.SelectedValue == "Birth Date")
                {
                    DependOn = "Birth Date";
                    //BirthDateDiv.Visible = true;
                    //todaySensexReturnBD.Text = returnAmountSx;
                    //todayFdReturnBD.Text = returnAmountFd;
                    SipGridView.DataSource = birthDaTble;
                    SipGridView.DataBind();


                    //todaySensexReturnBD.Text.ToString("n0");

                }
                else
                {
                    DependOn = "Wedding Date";
                    //WeddingDiv.Visible = true;
                    //todaySensexReturnWD.Text = returnAmountSx;
                    //todayFdReturnWD.Text = returnAmountFd;
                    SipGridView.DataSource = wedDaTble;
                    SipGridView.DataBind();

                }


                //txtAdvisorname1.Text = txtAdvisorname.Text;
                //advisorTxt11.Text = advisorTxt.Text;



                if (txtAdvisorname.Text == "Advisor Company Name" && txtARNNumber.Text == "ARN Number" && advisorTxt.Text == "Advisor Name")
                {
                    lblAdvisorHeading.Visible = false;
                }
                else
                {
                    lblAdvisorHeading.Visible = true;
                }


                if (txtAdvisorname.Text != "Advisor Company Name")
                {
                    lblAdvisorCompanyName.Text = txtAdvisorname.Text;
                }
                else
                {
                    lblAdvisorCompanyName.Text = string.Empty;
                }

                if (txtARNNumber.Text != "ARN Number")
                {
                    lblAdvisorARNNumber.Text = txtARNNumber.Text;
                }
                else
                {
                    lblAdvisorARNNumber.Text = string.Empty;
                }

                if (advisorTxt.Text != "Advisor Name")
                {
                    lblAdvisorName.Text = advisorTxt.Text;
                }
                else
                {
                    lblAdvisorName.Text = string.Empty;
                }

                Amount1.Text = indiarupeeformat(Amount.Text.Split(' ')[0]);
                Amount1.Text += "  per month";



                DateTime yearEndDate = new DateTime(DateTime.Now.Year, 3, 31, 0, 0, 0);
                DateTime todayDateComp = DateTime.Now;
                int result = DateTime.Compare(yearEndDate, todayDateComp);

                //if (DateTime.Now <= )
                if (result < 0)
                    yearLabel.Text = DateTime.Now.Year.ToString();
                else
                    yearLabel.Text = DateTime.Now.AddYears(-1).Year.ToString();

                yearLabel1.Text = yearLabel.Text;

                //Convert.ToDouble(Amount.Text.Split(' ')[0]).ToString("##,##,###") +"  per month";
                //Amount1.Text =  string.Format("{0:##,##,##,###}", Amount.Text.Split(' ')[0]);
                StartDate1.Text = StartDate.Text;



                InvestorName1.Text = InvestorName.Text;

                //RadioButtonList2.SelectedItem = RadioButtonList1.SelectedItem;
                RadioButtonList2.Items[RadioButtonList1.SelectedIndex].Selected = true;
                if (RadioButtonList1.SelectedIndex == 1)
                    RadioButtonList2.Items[0].Enabled = false;
                else
                    RadioButtonList2.Items[1].Enabled = false;



                if (inputDiv.Visible == true)
                    inputDiv.Visible = false;

                if (OutputDiv.Visible == false)
                    OutputDiv.Visible = true;

                //ViewState["advisorTxtvw"] = advisorTxt.Text;
                //ViewState["txtAdvisorname"] = txtAdvisorname.Text;
                headerId.Text = "Big Day SIP for " + InvestorName.Text;


                ////////for backspace when the work get completed//
                //txtAdvisorname.Text = "";
                //txtAdvisorname.Text = "";
                //advisorTxt.Text = "";
                //Amount.Text = "";
                //StartDate.Text = "";
                //InvestorName.Text = "";

                //*********Code By Abhishek 06.03.2012********************
                arr.Add(InvestorName1.Text);
                arr.Add(StartDate1.Text);
                arr.Add(Amount1.Text);
                arr.Add(investedAmountTotal.Text);
                arr.Add(investmentPeriod.Text);
                if (txtAdvisorname.Text != "Advisor Company Name")
                {
                    arr.Add(txtAdvisorname.Text);
                }
                else
                {
                    arr.Add(string.Empty);
                }

                if (advisorTxt.Text != "Advisor Name")
                {
                    arr.Add(advisorTxt.Text);
                }
                else
                {
                    arr.Add(string.Empty);
                }

                if (txtARNNumber.Text != "ARN Number")
                {
                    arr.Add(txtARNNumber.Text);
                }
                else
                {
                    arr.Add(string.Empty);
                }

                string AdvisorLogoPath = string.Empty;
                System.Text.StringBuilder sbimgname = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(txtAdvisorname.Text.Trim()))
                {
                    if (File.Exists(Server.MapPath("~/Images/AdvisorLogo/" + txtAdvisorname.Text.Trim().Replace(" ", "_") + ".jpeg")))
                    {
                        //sbimgname.Append("~/Images/AdvisorLogo/" + advisorTxt.Text.Trim().Replace(" ", "_") + ".jpeg");
                        sbimgname.Append("<img  src='../Images/AdvisorLogo/" + txtAdvisorname.Text.Trim().Replace(" ", "_") + ".jpeg' alt='' width='150' height='100'/>");
                        //imgAdvisorLogo.ImageUrl = sbimgname.ToString();
                        logo = sbimgname.ToString();
                    }
                    //else
                    //{
                    //sbimgname.Append("~/Images/AdvisorLogo/NoLogo.jpeg");

                    //imgAdvisorLogo.ImageUrl = sbimgname.ToString();
                    //}
                    sbimgname = null;
                }

                if (RadioButtonList1.SelectedValue == "Birth Date")
                {
                    CreateHTMLBigDaySIP(arr, RadioButtonList1.SelectedValue);
                    _SimpleConversion();
                }
                else
                {
                    CreateHTMLBigDaySIP(arr, RadioButtonList1.SelectedValue);
                    _SimpleConversion();
                }
                //****************End**************************************

            }
            catch (Exception ex)
            { }


        }

        protected void btnLinkBack_Click(object sender, EventArgs e)
        {
            if (inputDiv.Visible == false)
                inputDiv.Visible = true;

            if (OutputDiv.Visible == true)
                OutputDiv.Visible = false;

            headerId.Text = "Big Day SIP";
            //txtAdvisorname.Text = "Advisor Company Name";
            //advisorTxt.Text = "Advisor Name";
        }

        #endregion

        #region : GridEvent


        protected void SipGridViewRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image img1 = new Image();
                Image img2 = new Image();
                if (e.Row.RowState == DataControlRowState.Alternate)
                {


                    //img1.ImageUrl = img2.ImageUrl = "~/Images/rsymbol.jpg";
                    //e.Row.Cells[3].Controls.AddAt(0, img1);
                    //e.Row.Cells[1].Controls.AddAt(0, img2);


                }
                else
                {

                    //img1.ImageUrl = img2.ImageUrl = "~/Images/rsym.jpg";
                    //e.Row.Cells[1].Controls.AddAt(0, img1);
                    //e.Row.Cells[3].Controls.AddAt(0, img2);
                }
            }

        }

        protected void changeCellColor(object sender, GridViewRowEventArgs e)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Object ob = drv["SIP_CMPD_AMNT"];
                if (!Convert.IsDBNull(ob))
                {
                    double dVal = 0f;
                    if (Double.TryParse(ob.ToString(), out dVal))
                    {
                        if (dVal < AnnualSensxAmnt1)
                        {
                            TableCell cell = e.Row.Cells[2];
                            //cell.CssClass = "heavyrow";     
                            cell.BackColor = System.Drawing.Color.Red;
                            highlightPanel.Visible = true;
                        }
                        AnnualSensxAmnt1 = dVal;
                    }
                }
            }





        }

        #endregion

        #region: LinkButton Events

        protected void lnkbtnDownload_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            if (Session["GUID"] != null)
            {
                //path = Server.MapPath("~") + "\\Pages\\cache_pdf\\BigDaySIP_" + Convert.ToString(Session["GUID"]) + ".pdf";
                //Response.Redirect("BigDaySipDownload.aspx?path=" + path + "&check=BigDaySip");
                path = Server.MapPath("~") + "\\Pages\\cache_pdf\\BigDaySIP_" + Convert.ToString(Session["GUID"]) + ".pdf";
                Response.Redirect("BigDaySipDownload.aspx?path=" + path + "");
            }

        }

        #endregion


        #region: Methods By Abhishek


        #region: VerifyRenderingInServerForm(Require to pass gridview control)

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the
            // specified ASP.NET server control at run time.
            // No code required here.
            return;
        }

        #endregion

        #region: Big Day SIP Create HTML By Abhishek Das

        public void CreateHTMLBigDaySIP(ArrayList arr, string checker)
        {

            //string strHTML = string.Empty;
            System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
            try
            {
                strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Pages/BigDaySipTemplate/BigDaySIPTemplate.htm")));

                strHTML = strHTML.Replace("<!InvestorName1Val!>", arr[0].ToString());
                strHTML = strHTML.Replace("<!StartDate1Val!>", arr[1].ToString());
                strHTML = strHTML.Replace("<!Amount1Val!>", arr[2].ToString());
                strHTML = strHTML.Replace("<!investedAmountTotalVal!>", Convert.ToString(ViewState["investedAmountTotal"]));
                strHTML = strHTML.Replace("<!investmentPeriodVal!>", arr[4].ToString());
                strHTML = strHTML.Replace("<!txtAdvisorname1Val!>", arr[5].ToString());
                strHTML = strHTML.Replace("<!advisorTxt11Val!>", arr[6].ToString());
                strHTML = strHTML.Replace("<!advisornameheader!>", arr[0].ToString());
                strHTML = strHTML.Replace("<!AdvisorName!>", arr[6].ToString());
                strHTML = strHTML.Replace("<!AdvisorCompanyName!>", arr[5].ToString());
                strHTML = strHTML.Replace("<!AdvisorARNNumber!>", arr[7].ToString());

                if (txtAdvisorname.Text == "Advisor Company Name" && txtARNNumber.Text == "ARN Number" && advisorTxt.Text == "Advisor Name")
                {
                    strHTML = strHTML.Replace("<!AdvisorHeading!>", string.Empty);
                }
                else
                {
                    strHTML = strHTML.Replace("<!AdvisorHeading!>", "Advisor");
                }

                string AdvisorLogoPath = string.Empty;
                string imgname = string.Empty;
                if (!string.IsNullOrEmpty(arr[5].ToString()))
                {
                    imgname = arr[5].ToString().Replace(" ", "_") + ".jpeg";
                    AdvisorLogoPath = HttpContext.Current.Server.MapPath("~") + "\\Images\\AdvisorLogo\\" + imgname;
                    if (File.Exists(AdvisorLogoPath))
                    {
                        imgname = "../../Images/AdvisorLogo/" + imgname;
                        strHTML = strHTML.Replace("<!AdvisorLogo!>", "<img alt='' src='" + imgname + "' width='150' height='100' />");
                    }

                }

                string table = FillHtmlSipGridViewTable();
                strHTML = strHTML.Replace("<!ListSipGridView!>", table);

                strHTML = strHTML.Replace("<!ListPRIndexListViewGrid!>", FillHtmlstyleResultTble());


                if (checker == "Birth Date")
                {
                    //strHTML = strHTML.Replace("<!BirthDate!>", "<input id='RadioButtonList2_0' type='radio' name='RadioButtonList2' value='Birth Date'  checked=checked /><label for='RadioButtonList2_0'>Birth Date</label>");

                    //strHTML = strHTML.Replace("<!WeddingDate!>", "<input id='RadioButtonList2_1' type='radio' name='RadioButtonList2' value='Wedding Date' disabled='disabled' /><label for='RadioButtonList2_1'>Wedding Date</label>");
                    strHTML = strHTML.Replace("<!DependOn!>", "Birth Date");
                }
                else
                {
                    //strHTML = strHTML.Replace("<!BirthDate!>", "<input id='RadioButtonList2_0' type='radio' name='RadioButtonList2' value='Birth Date' disabled='disabled'/><label for='RadioButtonList2_0'>Birth Date</label>");

                    //strHTML = strHTML.Replace("<!WeddingDate!>", "<input id='RadioButtonList2_1' type='radio' name='RadioButtonList2' value='Wedding Date'  checked=checked/><label for='RadioButtonList2_1'>Wedding Date</label>");
                    strHTML = strHTML.Replace("<!DependOn!>", "Wedding Date");

                }

                if (PRIndexListViewGrid.Rows.Count > 0)
                {
                    Int32 space = (PRIndexListViewGrid.Rows.Count * 30);
                    space = 1250 - space;
                    strHTML.Replace("[!space!]", "style = 'height:" + space + "px;float:right;'");
                    Int32 topspace = 1090 - (PRIndexListViewGrid.Rows.Count * 30);
                    strHTML.Replace("[!secondpage!]", "<div style='padding-top:" + topspace.ToString() + "px;float:right;'> <span style='color:DarkGray;'>Page 2 of 3</span></div>");
                    // strHTML.Replace(" [!Thirdpage!]", "<div style='float:right;padding-top:670px;'><span style='color:DarkGray;'>Page 3 of 3</span></div>");
                    strHTML.Replace(" [!Thirdpage!]", "<div style='float:right;padding-top:900px;'><span style='color:DarkGray;'>Page 3 of 3</span></div>");
                    strHTML.Replace("<!ListHeader!>", "JOURNEY OF YOUR INVESTMENT");
                    strHTML.Replace("<!ListNote!>", "Red highlight shows when the value of investments fell over the previous year.");
                    strHTML.Replace("[!RedBox!]", "style='background-color: Red; height: 4px; width: 4px;'");
                    strHTML.Replace("[!MiddleHeader!]", GetHeader().Replace("<!advisornameheader!>", arr[0].ToString()));
                    strHTML.Replace("[!BottomHeader!]", GetHeader().Replace("<!advisornameheader!>", arr[0].ToString()));

                    //***********************************************************************************

                    if (txtAdvisorname.Text == "Advisor Company Name" && txtARNNumber.Text == "ARN Number" && advisorTxt.Text == "Advisor Name")
                    {
                        strHTML.Replace("[!Height!]", "style='height: 150px;'");
                    }
                    else
                    {
                        if (txtAdvisorname.Text != "Advisor Company Name" && txtARNNumber.Text != "ARN Number" && advisorTxt.Text != "Advisor Name")
                        {
                            strHTML.Replace("[!Height!]", "style='height: 35px;'");
                        }
                        else if (txtAdvisorname.Text != "Advisor Company Name" && txtARNNumber.Text != "ARN Number")
                        {
                            strHTML.Replace("[!Height!]", "style='height: 65px;'");
                        }
                        else if (txtARNNumber.Text != "ARN Number" && advisorTxt.Text != "Advisor Name")
                        {
                            strHTML.Replace("[!Height!]", "style='height: 65px;'");
                        }
                        else if (txtAdvisorname.Text != "Advisor Company Name" && advisorTxt.Text != "Advisor Name")
                        {
                            strHTML.Replace("[!Height!]", "style='height: 65px;'");
                        }
                        else if (txtAdvisorname.Text != "Advisor Company Name")
                        {
                            strHTML.Replace("[!Height!]", "style='height: 85px;'");
                        }
                        else if (txtARNNumber.Text != "ARN Number")
                        {
                            strHTML.Replace("[!Height!]", "style='height: 85px;'");
                        }
                        else if (advisorTxt.Text != "Advisor Name")
                        {
                            strHTML.Replace("[!Height!]", "style='height: 85px;'");
                        }
                    }
                    //***************************************************************************
                }
                else
                {
                    strHTML.Replace("[!secondpage!]", string.Empty);
                    strHTML.Replace("[!MiddleHeader!]", string.Empty);
                    strHTML.Replace("[!BottomHeader!]", GetHeader().Replace("<!advisornameheader!>", arr[0].ToString()));
                    //strHTML.Replace(" [!Thirdpage!]", "<div style='float:right;padding-top:370px;'><span style='color:DarkGray;'>Page 2 of 3</span></div>");
                    strHTML.Replace(" [!Thirdpage!]", "<div style='float:right;padding-top:800px;'><span style='color:DarkGray;'>Page 2 of 3</span></div>");
                    strHTML.Replace("[!space!]", "style = 'height:45px;float:right;'");
                }


                string path = string.Empty;
                //path = HttpContext.Current.Server.MapPath("~/Pages/cache_pdf/BigDaySIP.htm");
                Session["GUID"] = Guid.NewGuid().ToString();
                path = HttpContext.Current.Server.MapPath("~/Pages/cache_pdf/" + "BigDaySIP" + "_" + Convert.ToString(Session["GUID"]) + ".htm");

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (File.Create(path))
                {

                }
                File.WriteAllText(path, strHTML.ToString());
                strHTML = null;
            }
            catch (Exception ex)
            {
                strHTML = null;
                throw ex;
            }

        }

        private string FillHtmlSipGridViewTable()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                HttpContext.Current.Response.Clear();
                using (System.IO.StringWriter stringWrite = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite))
                    {
                        if (SipGridView.Rows.Count > 0)
                        {
                            SipGridView.RenderControl(htmlWrite);
                            sb.Append(stringWrite.ToString());
                            //sb.Replace("../Images/rsym.jpg", "../../Images/rsym.jpg");
                            sb.Replace("../Images/rsymbol.JPG", "../../Images/rsymbol.JPG");
                        }
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string FillHtmlstyleResultTble()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                HttpContext.Current.Response.Clear();
                using (System.IO.StringWriter stringWrite = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite))
                    {

                        if (PRIndexListViewGrid.Rows.Count > 0)
                        {
                            PRIndexListViewGrid.RenderControl(htmlWrite);
                            sb.Append(stringWrite.ToString());
                            sb.Replace("../Images/rsymbol.JPG", "../../Images/rsymbol.JPG");
                        }

                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string GetHeader()
        {
            System.Text.StringBuilder sbResult = new System.Text.StringBuilder();
            try
            {
                sbResult.Append(@"<div><table width='975' border='0' cellspacing='0' cellpadding='0'><tr>
                <td width='9%' height='89' bgcolor='#006bc4'>
                </td><td width='9%'>
                &nbsp;
                </td>
                <td width='57%' class='header_top'>
                <span id='Span2' style='color:#006bc4; font-weight:bold;'>Big Day SIP for <!advisornameheader!></span>
                </td>
                <td width='25%' align='right'>
                <img src='../../Images/logo.jpg' width='234' height='86' alt=' style='border:0px;' />
                </td>
                </tr>
                </table>
                </div>");
            }
            catch (Exception ex)
            {


            }
            return sbResult.ToString();
        }


        #endregion

        #region: Generate PDf BY Abhishek Das

        public void _SimpleConversion()
        {
            try
            {
                using (var wk = _GetConverter())
                {

                    _Log.DebugFormat("Performing conversion..");

                    wk.GlobalSettings.Margin.Top = "0cm";
                    wk.GlobalSettings.Margin.Bottom = "0cm";
                    wk.GlobalSettings.Margin.Left = "0cm";
                    wk.GlobalSettings.Margin.Right = "0cm";
                    //wk.GlobalSettings.Out = @"c:\temp\tmp.pdf";



                    wk.ObjectSettings.Web.EnablePlugins = false;
                    wk.ObjectSettings.Web.EnableJavascript = true;
                    wk.ObjectSettings.Web.LoadImages = true;
                    wk.ObjectSettings.Page = SimplePageFile;

                    string htmlfile = string.Empty;

                    htmlfile = HttpContext.Current.Server.MapPath("~/Pages/cache_pdf/BigDaySIP_" + Convert.ToString(Session["GUID"]) + ".htm");

                    wk.ObjectSettings.Page = htmlfile;

                    wk.ObjectSettings.Load.Proxy = "none";

                    var tmp = wk.Convert();

                    Assert.IsNotEmpty(tmp);
                    var number = 0;
                    lock (this) number = count++;

                    string savepdfpath = string.Empty;
                    string savehtmlpath = string.Empty;
                    savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\Pages\\cache_pdf\\BigDaySIP_" + Convert.ToString(Session["GUID"]) + ".pdf";
                    savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\Pages\\cache_pdf\\BigDaySIP_" + Convert.ToString(Session["GUID"]) + ".htm";
                    ViewState["selectedval"] = savepdfpath;
                    if (File.Exists(savepdfpath))
                    {
                        File.Delete(savepdfpath);
                    }
                    File.WriteAllBytes(savepdfpath, tmp);
                    if (File.Exists(savehtmlpath))
                    {
                        File.Delete(savehtmlpath);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private MultiplexingConverter _GetConverter()
        {
            var obj = new MultiplexingConverter();
            obj.Begin += (s, e) => _Log.DebugFormat("Conversion begin, phase count: {0}", e.Value);
            obj.Error += (s, e) => _Log.Error(e.Value);
            obj.Warning += (s, e) => _Log.Warn(e.Value);
            obj.PhaseChanged += (s, e) => _Log.InfoFormat("PhaseChanged: {0} - {1}", e.Value, e.Value2);
            obj.ProgressChanged += (s, e) => _Log.InfoFormat("ProgressChanged: {0} - {1}", e.Value, e.Value2);
            obj.Finished += (s, e) => _Log.InfoFormat("Finished: {0}", e.Value ? "success" : "failed!");
            return obj;
        }

        #endregion

        #region: Two Decimal Place Method
        public string TwoDecimal(string data)
        {
            string result = string.Empty;
            result = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(data))).ToString();
            return result;
        }
        #endregion

        #endregion

        public string indiarupeeformat(string amount)
        {
            CultureInfo CInfo = new CultureInfo("hi-IN");
            string rstr = string.Empty;
            //Convert.ToDecimal(Prc).ToString("C", CInfo));
            rstr = Convert.ToDecimal(amount).ToString("N", CInfo);
            return rstr.Split('.')[0];
        }

        //protected void Amount_TextChanged(object sender, EventArgs e)
        //{
        //    Amount.Text.Replace("per month", "");
        //    Amount.Text += "per month";
        //}



    }
}