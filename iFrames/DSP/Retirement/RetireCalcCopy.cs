using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using WkHtmlToXSharp;
using System.IO;
using NUnit.Framework;
using System.Globalization;
using iFrames.BLL;

namespace iFrames.DSP.Retirement
{
    public partial class RetireCalcCopy : System.Web.UI.Page
    {
        #region: PDF Global Variable

        private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string SimplePageFile = null;
        public static int count = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                SetHiddenFromLabel();

            }
            Session["isDistributor"] = false;
        }

        private void SetHiddenFromLabel()
        {
            CultureInfo CInfo = new CultureInfo("hi-IN");
            lbtxtExpectedMonthlyExpRetire.Text = Convert.ToDouble(HidlbtxtExpectedMonthlyExpRetire.Value).ToString("n0", CInfo);
            lbtxtRetireCorpusLast.Text = HidlbtxtRetireCorpusLast.Value;
            lbtxtPlannedRetireCorpus.Text = HidlbtxtPlannedRetireCorpus.Value;
            lbTotalTimeReqRetire.Text = HidlbTotalTimeReqRetire.Value;
            lbExpTimeReqRetire.Text = HidlbExpTimeReqRetire.Value;

        }

        protected void PDFGenerate_Click(object sender, EventArgs e)
        {
            // CreateHTMLRetirement();
        }

        protected void LBGenerateReport_Click(object sender, EventArgs e)
        {
            //  CreateHTMLTax();
            DSPDistributor objDist;


            // if (RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim() == "DISTRIBUTOR")

            if (HidDist.Value.ToUpper().Trim() == "DISTRIBUTOR")
            {

                DspUtility objUtil = new DspUtility();
                objDist = new DSPDistributor()
                {
                    Preparedby = txtPreparedby.Value,
                    PreparedFor = txtPreparedFor.Value,
                    Mobile = txtMobile.Value,
                    ArnNo = "ARN-" + txtArn1.Value,
                    Email = txtEmail.Value,
                    Investment_mode = "Retirement calc"
                };

                //objUtil.SetDistributorCredential(objDist);
                //objUtil.InsertDistributorCredential(objDist);
            }
            else
            {
                objDist = null;
            }

            //CreateHTMLTax(objDist);
            CreateHTMLRetirement(objDist);

        }

        protected void RadioButtonListCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistributerDiv.Visible = RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim() == "DISTRIBUTOR";
            Showpdfdiv.Attributes.Add("style", "display:");
        }

        #region PDF


        /// <summary>
        /// This Function will Generate a PDF
        /// </summary>
        private void CreateHTMLRetirement(DSPDistributor objDist)
        {
            System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
            // string sipGridViewstr = string.Empty;
            string path = string.Empty;
            CultureInfo CInfo = new CultureInfo("hi-IN");

            var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\Retirement\\PDF", "DSP_*");


            foreach (var f in allFiles)
                if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
                    File.Delete(f);

            try
            {
                bool isDistributor = false;

                //  Control ctrl = divDist.FindControl("radioDist");


                //if (radioDist.Checked)
                //    isDistributor = true;
                //Session["isDistributor"] = false;
                //var val = Session["isDistributor"];

                //string strDist = HidDist.Value;

                //if (RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim() == "DISTRIBUTOR")
                //    strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/DSP/Retirement/Retirement_PDF.html")));//Read File
                //else
                //    strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/DSP/Retirement/Retirement_PDF_wd.html")));//Read File

                if (HidDist.Value.ToUpper().Trim() == "DISTRIBUTOR")
                    strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/DSP/Retirement/Retirement_PDF.html")));//Read File
                else
                    strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/DSP/Retirement/Retirement_PDF_wd.html")));//Read File



                #region Replace Value

                //input Section
                strHTML = strHTML.Replace("<!txtUserName!>", txtUserName.Value);
                strHTML = strHTML.Replace("<!txtAge!>", txtAge.Value);
                strHTML = strHTML.Replace("<!txtUserEmail!>", txtUserEmail.Value);
                strHTML = strHTML.Replace("<!txtWishRetireAge!>", txtWishRetireAge.Value);
                strHTML = strHTML.Replace("<!txtRatePreRetire!>", txtRatePreRetire.Value);
                strHTML = strHTML.Replace("<!txtRatePostRetire!>", txtRatePostRetire.Value);
                strHTML = strHTML.Replace("<!txtRateEstInflation!>", txtRateEstInflation.Value);


                //content 1
                if (txtcurrentMonthlyExp.Value.Length > 0) strHTML = strHTML.Replace("<!txtcurrentMonthlyExp!>", Convert.ToDouble(txtcurrentMonthlyExp.Value).ToString("n0", CInfo));
                if (lbtxtExpectedMonthlyExpRetire.Text.Length > 0) strHTML = strHTML.Replace("<!lbtxtExpectedMonthlyExpRetire!>", Convert.ToDouble(lbtxtExpectedMonthlyExpRetire.Text).ToString("n0", CInfo));

                //content 2
                if (txtEstimatedRetirementCorpus.Value.Length > 0) strHTML = strHTML.Replace("<!txtEstimatedRetirementCorpus!>", Convert.ToDouble(txtEstimatedRetirementCorpus.Value).ToString("n0", CInfo));
                if (txtMonthlyIncomePostRetire.Value.Length > 0) strHTML = strHTML.Replace("<!txtMonthlyIncomePostRetire!>", Convert.ToDouble(txtMonthlyIncomePostRetire.Value).ToString("n0", CInfo));

                if (txtMonthlyIncrementPostRetire.Value.Trim() != "Would you want an increase in monthly income every year post retirement? If yes, how much?")
                {
                    if (txtMonthlyIncrementPostRetire.Value.Length > 0) strHTML = strHTML.Replace("<!txtMonthlyIncrementPostRetire!>", Convert.ToDouble(txtMonthlyIncrementPostRetire.Value).ToString("n0", CInfo));
                }

                if (lbtxtRetireCorpusLast.Text.Length > 0) strHTML = strHTML.Replace("<!lbtxtRetireCorpusLast!>", Convert.ToDouble(lbtxtRetireCorpusLast.Text).ToString("n0", CInfo));

                //content 3
                if (txtExpectedMonthlyIncomePostRetire.Value.Length > 0) strHTML = strHTML.Replace("<!txtExpectedMonthlyIncomePostRetire!>", Convert.ToDouble(txtExpectedMonthlyIncomePostRetire.Value).ToString("n0", CInfo));
                if (txtExpectdRetireCorpusLast.Value.Length > 0) strHTML = strHTML.Replace("<!txtExpectdRetireCorpusLast!>", Convert.ToDouble(txtExpectdRetireCorpusLast.Value).ToString("n0", CInfo));
                if (lbtxtPlannedRetireCorpus.Text.Length > 0) strHTML = strHTML.Replace("<!lbtxtPlannedRetireCorpus!>", Convert.ToDouble(lbtxtPlannedRetireCorpus.Text).ToString("n0", CInfo));
				//Add by syed
				if (txtMonthlyIncrementPostRetire4Stage3.Value.Length > 0) strHTML = strHTML.Replace("<!txtMonthlyIncrementPostRetire4stage3!>", Convert.ToDouble(txtMonthlyIncrementPostRetire4Stage3.Value).ToString("n0", CInfo));
				//end syed

                //content 4
                if (txtEstRetCorp.Value.Length > 0) strHTML = strHTML.Replace("<!txtEstRetCorp!>", Convert.ToDouble(txtEstRetCorp.Value).ToString("n0", CInfo));
                if (txtRetDate.Value.Length > 0) strHTML = strHTML.Replace("<!txtRetDate!>", ConvertDate(txtRetDate.Value).ToString("MMM yyyy"));

                if (txtMonthlySipAmount.Value.Length > 0) strHTML = strHTML.Replace("<!txtMonthlySipAmount!>", Convert.ToDouble(txtMonthlySipAmount.Value).ToString("n0", CInfo));

                if (txtSIPYearly.Value.Trim() != "How Many Yrs")
                {
                    if (txtSIPYearly.Value.Length > 0) strHTML = strHTML.Replace("<!txtSIPYearly!>", Convert.ToDouble(txtSIPYearly.Value).ToString("n0", CInfo));
                }

                if (txtLsInvestAmount.Value.Length > 0) strHTML = strHTML.Replace("<!txtLsInvestAmount!>", Convert.ToDouble(txtLsInvestAmount.Value).ToString("n0", CInfo));


                if (lbTotalTimeReqRetire.Text.Length > 0) strHTML = strHTML.Replace("<!lbTotalTimeReqRetire!>", lbTotalTimeReqRetire.Text.ToString());
                // if (lbExpTimeReqRetire.Text.Length > 0) strHTML = strHTML.Replace("<!lbExpTimeReqRetire!>", ConvertDate(lbExpTimeReqRetire.Text).ToString("MMM yyyy"));
                if (lbExpTimeReqRetire.Text.Length > 0) strHTML = strHTML.Replace("<!lbExpTimeReqRetire!>", lbExpTimeReqRetire.Text);

                if (cbMainSipS.Checked)
                {
                    //string tempstr = if( txtSIPYearly.Value == "" || txtSIPYearly.Value == "How Many Yrs")  return "Not Specified"; else return (txtSIPYearly.Value.ToString() + " Years").ToString();
                    string tempstr = (txtSIPYearly.Value == "How Many Yrs") ? "Not Specified" : (txtSIPYearly.Value.ToString() + " Years").ToString();
                    string temps = "<tr><td class='column1'>" + ConvertDate(txtRetDate.Value).ToString("dd MMM yyyy") + "&nbsp;&nbsp;<img src='../images/green.jpg' style='vertical-align: top; padding-top: 5px;'/></td><td class='column1'><img src='../images/grimg.jpg' style='vertical-align: top; padding-top: 2px;' />" + txtMonthlySipAmount.Value.ToString() + "</td><td class='column2'>" + tempstr + "</td></tr>";
                    if (!string.IsNullOrEmpty(temps)) strHTML = strHTML.Replace("<!AddTrMainSip!>", temps);
                }

                if (cbMainLsS.Checked)
                {
                    string temps = "<tr><td class='column3'>" + ConvertDate(txtRetDate.Value).ToString("dd MMM yyyy") + "&nbsp;&nbsp;<img src='../images/green.jpg' style='vertical-align: top; padding-top: 5px;'/></td><td class='column4'><img src='../images/grimg.jpg' style='vertical-align: top; padding-top: 2px;' />" + txtLsInvestAmount.Value.ToString() + "</td></tr>";
                    if (!string.IsNullOrEmpty(temps)) strHTML = strHTML.Replace("<!AddTrMainLs!>", temps);
                }

                if (cbTopUpS.Checked)
                {
                    string addTrSip = string.Empty;
                    string addTrLs = string.Empty;
                    if (cbAddSipS.Checked)
                    {
                        if (!string.IsNullOrEmpty(HidAddSipRec.Value) && HidAddSipRec.Value.Contains('#'))
                        {
                            var listCounter = HidAddSipRec.Value.TrimEnd('#').Split('#');

                            foreach (string str in listCounter)
                            {
                                //System.Web.UI.HtmlControls.HtmlInputText objHtmlInputTextSipMonthly = ((System.Web.UI.HtmlControls.HtmlInputText)(divMainAddedSip.FindControl("ntxtSip" + s)));
                                //System.Web.UI.HtmlControls.HtmlInputText objHtmlInputTextSipAfterYear = ((System.Web.UI.HtmlControls.HtmlInputText)(divMainAddedSip.FindControl("ntxtAfterYear" + s)));
                                //System.Web.UI.HtmlControls.HtmlInputText objHtmlInputTextSipYearlast = ((System.Web.UI.HtmlControls.HtmlInputText)(divMainAddedSip.FindControl("ntxtYearlast" + s)));

                                var objStrArray = str.Split('|');
                                if (objStrArray.Count() == 4)
                                {
                                    string tempstr = objStrArray[3].Trim().ToString() == "1000" ? "Not Specified" : (objStrArray[3].Trim().ToString() + " Years");
                                    string temps = "<tr><td class='column1'>" + ConvertDate(txtRetDate.Value).AddYears(Convert.ToInt32(objStrArray[1])).ToString("MMM yyyy") + "</td><td class='column1'><img src='../images/grimg.jpg' style='vertical-align: top; padding-top: 2px;' />" + objStrArray[2].ToString() + "</td><td class='column2'>" + tempstr + "</td></tr>";
                                    addTrSip += temps;
                                }
                                // addTrSip += @"<tr><td class='column1'>" + Convert.ToDateTime(txtRetDate.Value).AddYears(Convert.ToInt32(objHtmlInputTextSipAfterYear.Value)).ToShortDateString() + @"</td><td class='column1'><img src='../images/grimg.jpg' style='vertical-align: top; padding-top: 2px;' />" + objHtmlInputTextSipMonthly.Value + @"</td><td class='column2'>" + objHtmlInputTextSipYearlast.Value + "  Years</td></tr>";
                            }

                        }
                    }
                    else
                    {
                        if (!cbMainSipS.Checked)
                        {
                            addTrSip += @"<tr><td class='column1'>Not Specified</td><td class='column1'>Not Specified</td><td class='column2'>Not Specified</td></tr>";
                        }
                    }
                    if (!string.IsNullOrEmpty(addTrSip)) strHTML = strHTML.Replace("<!AddTrSip!>", addTrSip);

                    if (cbAddLsS.Checked)
                    {

                        if (!string.IsNullOrEmpty(HidAddLsRec.Value) && HidAddLsRec.Value.Contains('#'))
                        {
                            var listCounter = HidAddLsRec.Value.TrimEnd('#').Split('#');

                            foreach (string str in listCounter)
                            {
                                //System.Web.UI.HtmlControls.HtmlInputText objHtmlInputTextLsInvest = ((System.Web.UI.HtmlControls.HtmlInputText)(divMainAddedSip.FindControl("ntxtLsInvstmnt" + s)));
                                //System.Web.UI.HtmlControls.HtmlInputText objHtmlInputTextLsAfterYear = ((System.Web.UI.HtmlControls.HtmlInputText)(divMainAddedSip.FindControl("ntxtAfterYearLs" + s)));
                                var objStrArray = str.Split('|');
                                if (objStrArray.Count() == 3)
                                {
                                    string temps = "<tr><td class='column3'>" + ConvertDate(txtRetDate.Value).AddYears(Convert.ToInt32(objStrArray[1])).ToString("MMM yyyy") + "</td><td class='column4'><img src='../images/grimg.jpg' style='vertical-align: top; padding-top: 2px;' />" + objStrArray[2].ToString() + @"</td></tr>";
                                    addTrLs += temps;
                                }

                                //addTrLs += @"<tr><td class='column3'>" + Convert.ToDateTime(txtRetDate.Value).AddYears(Convert.ToInt32(objHtmlInputTextLsAfterYear.Value)).ToShortDateString() + @"</td><td class='column4'><img src='../images/grimg.jpg' style='vertical-align: top; padding-top: 2px;' />" + objHtmlInputTextLsInvest.Value + @"</td></tr>";
                            }


                        }
                    }
                    else
                    {
                        if (!cbMainLsS.Checked)
                        {
                            addTrLs += @"<tr><td class='column3'>Not Specified</td><td class='column4'>Not Specified</td></tr>";
                        }
                    }
                    if (!string.IsNullOrEmpty(addTrLs)) strHTML = strHTML.Replace("<!AddTrLs!>", addTrLs);
                }



                if (objDist != null)
                {
                    strHTML = strHTML.Replace("<!PreparedBy!>", objDist.Preparedby);
                    strHTML = strHTML.Replace("<!ArnNo!>", objDist.ArnNo);
                    strHTML = strHTML.Replace("<!Mobile!>", objDist.Mobile);
                    strHTML = strHTML.Replace("<!Email!>", objDist.Email);
                    strHTML = strHTML.Replace("<!PreparedFor!>", objDist.PreparedFor);


                }


                #endregion


                Session["GUID"] = Guid.NewGuid().ToString();
                path = HttpContext.Current.Server.MapPath("~/DSP/Retirement/PDF/" + "DSP" + "_" + Convert.ToString(Session["GUID"]) + ".htm");

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (File.Create(path))
                {

                }
                File.WriteAllText(path, strHTML.ToString());
                strHTML = null;
                _SimpleConversion();

                string pdfName = string.Empty;

                if (Session["GUID"] != null)
                {
                    pdfName = Convert.ToString(Session["GUID"]) + ".pdf";
                    Response.ContentType = "Application/pdf";
                    pdfName = Convert.ToString(Session["GUID"]) + ".pdf";
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=DSP_Sample.pdf");
                    Response.AppendHeader("Content-Disposition", "attachment; filename="+txtUserName.Value.Trim().Replace(' ','_') +".pdf");
                    Response.TransmitFile(Server.MapPath("~/DSP/Retirement/PDF/DSP_" + pdfName));
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                strHTML = null;
                throw ex;
            }
        }

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

                    htmlfile = HttpContext.Current.Server.MapPath("~/DSP/Retirement/PDF/" + "DSP" + "_" + Convert.ToString(Session["GUID"]) + ".htm");
                    wk.ObjectSettings.Page = htmlfile;

                    wk.ObjectSettings.Load.Proxy = "none";

                    var tmp = wk.Convert();

                    Assert.IsNotEmpty(tmp);
                    var number = 0;
                    lock (this) number = count++;

                    string savepdfpath = string.Empty;
                    string savehtmlpath = string.Empty;
                    savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\DSP\\Retirement\\PDF\\DSP_" + Convert.ToString(Session["GUID"]) + ".pdf";

                    savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\DSP\\Retirement\\PDF\\DSP_" + Convert.ToString(Session["GUID"]) + ".htm";
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



        protected DateTime ConvertDate(string startDate)
        {
            DateTime sstartDate = new DateTime(Convert.ToInt16(startDate.Split('/')[2]),
                                     Convert.ToInt16(startDate.Split('/')[1]),
                                     Convert.ToInt16(startDate.Split('/')[0]));
            return sstartDate;
        }
    }

}