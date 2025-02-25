using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using WkHtmlToXSharp;
using NUnit.Framework;

namespace iFrames.DSP
{
    public partial class DSPtaxCalc : System.Web.UI.Page
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
                spanRgessTaxExempt.Text = HidspanRgessTaxExempt.Value;
                spanTaxfreeLimit.Text = HidspanTaxfreeLimit.Value;
                spanTaxfreeLimit1.Text = HidspanTaxfreeLimit1.Value;
                spanTaxfreeLimit2.Text = HidspanTaxfreeLimit2.Value;
                spanTaxfreeLimit3.Text = HidspanTaxfreeLimit3.Value;
                spanTaxfreeLimit4.Text = HidspanTaxfreeLimit4.Value;
                spanTaxfreeLimit5.Text = HidspanTaxfreeLimit5.Value;
                spanTaxLimit1Rate.Text = HidspanTaxLimit1Rate.Value;
                spanTaxLimit2Rate.Text = HidspanTaxLimit2Rate.Value;
                spanTaxLimit3Rate.Text = HidspanTaxLimit3Rate.Value;
                spanEducationCessRate.Text = HidspanEducationCessRate.Value;
                spanMedicalClaim.Text = HidspanMedicalClaim.Value;
                spanTaxExempt80c.Text = HidspanTaxExempt80c.Value;

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the
            // specified ASP.NET server control at run time.
            // No code required here.
            return;
        }

        protected void LinkButtonGenerateReport_Click(object sender, EventArgs e)
        {
            //  CreateHTMLTax();
            DSPDistributor objDist;

            if (RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim() == "DISTRIBUTOR")
            {

                DspUtility objUtil = new DspUtility();
                objDist = new DSPDistributor()
                {
                    Preparedby = txtPreparedby.Value,
                    PreparedFor = txtPreparedFor.Value,
                    Mobile = txtMobile.Value,
                    ArnNo = txtArn1.Value,
                    Email = txtEmail.Value,
                    Investment_mode = "Rgess"//;//RadioButtonListCustomerType.SelectedItem.Value
                };

                //objUtil.SetDistributorCredential(objDist);
                objUtil.InsertDistributorCredential(objDist);
            }
            else
            {
                objDist = null;
            }

            CreateHTMLTax(objDist);

        }

        protected void ExcelCalculation_Click(object sender, ImageClickEventArgs e)
        {
            //ShowExcel();
            ShowExcelFromHtml();
        }


        #region PDF


        /// <summary>
        /// This Function will Generate a PDF
        /// </summary>
        private void CreateHTMLTax(DSPDistributor  objDist)
        {
            System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
            // string sipGridViewstr = string.Empty;
            string path = string.Empty;


            var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\PDF", "DSP_*");


            foreach (var f in allFiles)
                if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
                    File.Delete(f);

            try
            {
                // objDist.d
                if (RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim() == "DISTRIBUTOR")
                    strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/DSP/taxPdf_dist.html")));//Read File
                else
                    strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/DSP/taxPdf.html")));//Read File

                // strHTML = strHTML.Replace("<!SIP!>", ddlMode.SelectedItem.Value);

                #region Replace Value


                strHTML = strHTML.Replace("<!txtuser!>", txtuser.Value);

                if (salaryInput.Value.Length > 0) strHTML = strHTML.Replace("<!salaryInput!>", Convert.ToDouble(salaryInput.Value).ToString("n0"));
                if (licInput.Value.Length > 0) strHTML = strHTML.Replace("<!licInput!>", Convert.ToDouble(licInput.Value).ToString("n0"));
                if (medInput.Value.Length > 0) strHTML = strHTML.Replace("<!medInput!>", Convert.ToDouble(medInput.Value).ToString("n0"));
                if (txtRgess.Value.Length > 0) strHTML = strHTML.Replace("<!txtRgess!>", Convert.ToDouble(txtRgess.Value).ToString("n0"));
                if (txtTaxIncome.Value.Length > 0) strHTML = strHTML.Replace("<!txtTaxIncome!>", Convert.ToDouble(txtTaxIncome.Value).ToString("n0"));
                if (txtbenefitRgess.Value.Length > 0) strHTML = strHTML.Replace("<!txtbenefitRgess!>", Convert.ToDouble(txtbenefitRgess.Value).ToString("n0"));
                if (txtTaxIncomeR.Value.Length > 0) strHTML = strHTML.Replace("<!txtTaxIncomeR!>", Convert.ToDouble(txtTaxIncomeR.Value).ToString("n0"));
                if (txtTaxIncomeRWo.Value.Length > 0) strHTML = strHTML.Replace("<!txtTaxIncomeRWo!>", Convert.ToDouble(txtTaxIncomeRWo.Value).ToString("n0"));
                if (taxLimit1.Value.Length > 0) strHTML = strHTML.Replace("<!taxLimit1!>", Convert.ToDouble(taxLimit1.Value).ToString("n0"));
                if (taxLimit1Wo.Value.Length > 0) strHTML = strHTML.Replace("<!taxLimit1Wo!>", Convert.ToDouble(taxLimit1Wo.Value).ToString("n0"));
                if (taxLimit2.Value.Length > 0) strHTML = strHTML.Replace("<!taxLimit2!>", Convert.ToDouble(taxLimit2.Value).ToString("n0"));
                if (taxLimit2Wo.Value.Length > 0) strHTML = strHTML.Replace("<!taxLimit2Wo!>", Convert.ToDouble(taxLimit2Wo.Value).ToString("n0"));
                if (taxLimit3.Value.Length > 0) strHTML = strHTML.Replace("<!taxLimit3!>", Convert.ToDouble(taxLimit3.Value).ToString("n0"));
                if (taxLimit3Wo.Value.Length > 0) strHTML = strHTML.Replace("<!taxLimit3Wo!>", Convert.ToDouble(taxLimit3Wo.Value).ToString("n0"));

                if (taxLimit4.Value.Length > 0) strHTML = strHTML.Replace("<!taxLimit4!>", Convert.ToDouble(taxLimit4.Value).ToString("n0"));
                if (taxLimit4Wo.Value.Length > 0) strHTML = strHTML.Replace("<!taxLimit4Wo!>", Convert.ToDouble(taxLimit4Wo.Value).ToString("n0"));


                if (txtTotalTax.Value.Length > 0) strHTML = strHTML.Replace("<!txtTotalTax!>", Convert.ToDouble(txtTotalTax.Value).ToString("n0"));
                if (txtTotalTaxWo.Value.Length > 0) strHTML = strHTML.Replace("<!txtTotalTaxWo!>", Convert.ToDouble(txtTotalTaxWo.Value).ToString("n0"));
                if (txtEduCess.Value.Length > 0) strHTML = strHTML.Replace("<!txtEduCess!>", Convert.ToDouble(txtEduCess.Value).ToString("n0"));
                if (txtEduCessWo.Value.Length > 0) strHTML = strHTML.Replace("<!txtEduCessWo!>", Convert.ToDouble(txtEduCessWo.Value).ToString("n0"));
                if (txtTaxSaved.Value.Length > 0) strHTML = strHTML.Replace("<!txtTaxSaved!>", Convert.ToDouble(txtTaxSaved.Value).ToString("n0"));

                if (textAge.Value.Length > 0) strHTML = strHTML.Replace("<!textAge!>", Convert.ToDouble(textAge.Value).ToString("n0"));
                if (txtHomeLoanInt.Value.Length > 0) strHTML = strHTML.Replace("<!txtHomeLoanInt!>", Convert.ToDouble(txtHomeLoanInt.Value).ToString("n0"));
                if (spanRgessTaxExempt.Text.Length > 0) strHTML = strHTML.Replace("<!spanRgessTaxExempt!>", Convert.ToDouble(spanRgessTaxExempt.Text).ToString("n0"));

                if (spanTaxfreeLimit.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit!>", Convert.ToDouble(spanTaxfreeLimit.Text).ToString("n0"));
                if (spanTaxfreeLimit1.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit1!>", Convert.ToDouble(spanTaxfreeLimit1.Text).ToString("n0"));
                if (spanTaxfreeLimit2.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit2!>", Convert.ToDouble(spanTaxfreeLimit2.Text).ToString("n0"));
                if (spanTaxfreeLimit3.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit3!>", Convert.ToDouble(spanTaxfreeLimit3.Text).ToString("n0"));
                if (spanTaxfreeLimit4.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit4!>", Convert.ToDouble(spanTaxfreeLimit4.Text).ToString("n0"));
                if (spanTaxfreeLimit5.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit5!>", Convert.ToDouble(spanTaxfreeLimit5.Text).ToString("n0"));

                if (spanTaxLimit1Rate.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxLimit1Rate!>", Convert.ToDouble(spanTaxLimit1Rate.Text).ToString("n0"));
                if (spanTaxLimit2Rate.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxLimit2Rate!>", Convert.ToDouble(spanTaxLimit2Rate.Text).ToString("n0"));
                if (spanTaxLimit3Rate.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxLimit3Rate!>", Convert.ToDouble(spanTaxLimit3Rate.Text).ToString("n0"));

                if (spanEducationCessRate.Text.Length > 0) strHTML = strHTML.Replace("<!spanEducationCessRate!>", Convert.ToDouble(spanEducationCessRate.Text).ToString("n0"));

                if (txtECess.Value.Length > 0) strHTML = strHTML.Replace("<!txtECess!>", Convert.ToDouble(txtECess.Value).ToString("n0"));
                if (txtECessWo.Value.Length > 0) strHTML = strHTML.Replace("<!txtECessWo!>", Convert.ToDouble(txtECessWo.Value).ToString("n0"));

                if (textAge.Value.Length > 0 && Convert.ToInt32(textAge.Value) >= 80)
                    strHTML = strHTML.Replace(@"<tr id=""rowtaxFreelimit"" style=""display:"">", @"<tr id=""rowtaxFreelimit"" style=""display:none"">");


                if (spanTaxExempt80c.Text.Length > 0)
                    strHTML = strHTML.Replace("<!spanTaxExempt80c!>", Convert.ToDouble(spanTaxExempt80c.Text).ToString("n0"));

                if (spanMedicalClaim.Text.Length > 0)
                    strHTML = strHTML.Replace("<!spanMedicalClaim!>", Convert.ToDouble(spanMedicalClaim.Text).ToString("n0"));


                if (objDist != null)
                {
                    strHTML = strHTML.Replace("<!PreparedBy!>", objDist.Preparedby);
                    strHTML = strHTML.Replace("<!Mobile!>", objDist.Mobile);
                    strHTML = strHTML.Replace("<!Email!>", objDist.Email);
                    strHTML = strHTML.Replace("<!PreparedFor!>", objDist.PreparedFor);
                }


                #endregion


                Session["GUID"] = Guid.NewGuid().ToString();
                path = HttpContext.Current.Server.MapPath("~/DSP/PDF/" + "DSP" + "_" + Convert.ToString(Session["GUID"]) + ".htm");

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
                    Response.AppendHeader("Content-Disposition", "attachment; filename=DSP_Sample.pdf");
                    Response.TransmitFile(Server.MapPath("~/DSP/PDF/DSP_" + pdfName));
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

                    htmlfile = HttpContext.Current.Server.MapPath("~/DSP/PDF/" + "DSP" + "_" + Convert.ToString(Session["GUID"]) + ".htm");
                    wk.ObjectSettings.Page = htmlfile;

                    wk.ObjectSettings.Load.Proxy = "none";

                    var tmp = wk.Convert();

                    Assert.IsNotEmpty(tmp);
                    var number = 0;
                    lock (this) number = count++;

                    string savepdfpath = string.Empty;
                    string savehtmlpath = string.Empty;
                    savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\DSP\\PDF\\DSP_" + Convert.ToString(Session["GUID"]) + ".pdf";

                    savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\DSP\\PDF\\DSP_" + Convert.ToString(Session["GUID"]) + ".htm";
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


        #region Excel

        private void ShowExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DSPTax.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            string tempStr = string.Empty;

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Text.StringBuilder objFinalstr = new System.Text.StringBuilder();

            string ImagePath = string.Empty;
            ImagePath = HttpContext.Current.Server.MapPath("~") + "DSP\\img\\";

            divTax.RenderControl(hw);


            tempStr = sw.ToString();

            objFinalstr.Append("<br/>");
            objFinalstr.Append("<table width='520px' border ='1' style='text-align:justify'><tr><td width='100%'>");
            objFinalstr.Append(tempStr);
            objFinalstr.Replace("img/rupee_black.png", ImagePath + "rupee_black.png");
            objFinalstr.Replace("img/rupee_blue.png", ImagePath + "rupee_blue.png");

            //objFinalstr.Replace(@"type=""text"" ", "");
            objFinalstr.Replace(@"<input ", "<label ");
            // objFinalstr.Replace(@"<input ", @"<input style='border:none; padding:0; background-color:Red'  readonly='readonly' ");
            //objFinalstr.Replace(@"<input ", @"<input style='background-color:Red' ");
            //objFinalstr.Replace(@"name=""textfield"" ", " ");
            //objFinalstr.Replace(@"autocomplete=""off"" ", " ");
            //objFinalstr.Replace(@"value="" ", " ");


            //  objFinalstr.Append("<br/><b>Mutual Fund investments are subject to market risks, read all scheme related documents carefully.</b>");
            objFinalstr.Replace("<table", "<p height='90px'/><table");
            objFinalstr.Replace("width:220px", "width:350px");
            objFinalstr.Append("</td></tr></table>");
            objFinalstr.Append("<br/>");


            objFinalstr.Replace(@"class=""grdHead""", "style ='text-align: center;	font-family: Arial;	color: #ffffff;	font-size: 11px;background:#569fd3;	font-weight:normal;'");

            Response.Write(objFinalstr.ToString());
            Response.Flush();
            Response.End();
        }
        #endregion


        private string strHtmlDesign()
        {
            System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
            strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/DSP/taxExcel.html")));//Read File

            // strHTML = strHTML.Replace("<!SIP!>", ddlMode.SelectedItem.Value);

            #region Replace Value


            strHTML = strHTML.Replace("<!txtuser!>", txtuser.Value);
            if (salaryInput.Value.Length > 0)
                strHTML = strHTML.Replace("<!salaryInput!>", "Rs. " + Convert.ToDouble(salaryInput.Value).ToString("n0"));
            if (licInput.Value.Length > 0)
                strHTML = strHTML.Replace("<!licInput!>", "Rs. " + Convert.ToDouble(licInput.Value).ToString("n0"));
            if (medInput.Value.Length > 0)
                strHTML = strHTML.Replace("<!medInput!>", "Rs. " + Convert.ToDouble(medInput.Value).ToString("n0"));

            if (txtRgess.Value.Length > 0)
            {
                strHTML = strHTML.Replace("<!txtRgess!>", "Rs. " + Convert.ToDouble(txtRgess.Value).ToString("n0"));
                strHTML = strHTML.Replace("<!txtbenefitRgess!>", "Rs. " + Convert.ToDouble(txtbenefitRgess.Value).ToString("n0"));
            }


            if (txtTaxIncome.Value.Length > 0)
                strHTML = strHTML.Replace("<!txtTaxIncome!>", "Rs. " + Convert.ToDouble(txtTaxIncome.Value).ToString("n0"));


            if (txtTaxIncomeR.Value.Length > 0)
            {
                strHTML = strHTML.Replace("<!txtTaxIncomeR!>", "Rs. " + Convert.ToDouble(txtTaxIncomeR.Value).ToString("n0"));
                strHTML = strHTML.Replace("<!txtTaxIncomeRWo!>", "Rs. " + Convert.ToDouble(txtTaxIncomeRWo.Value).ToString("n0"));
            }

            if (taxLimit1.Value.Length > 0)
                strHTML = strHTML.Replace("<!taxLimit1!>", "Rs. " + Convert.ToDouble(taxLimit1.Value).ToString("n0"));
            if (taxLimit1Wo.Value.Length > 0)
                strHTML = strHTML.Replace("<!taxLimit1Wo!>", "Rs. " + Convert.ToDouble(taxLimit1Wo.Value).ToString("n0"));


            if (taxLimit2.Value.Length > 0)
                strHTML = strHTML.Replace("<!taxLimit2!>", "Rs. " + Convert.ToDouble(taxLimit2.Value).ToString("n0"));
            if (taxLimit2Wo.Value.Length > 0)
                strHTML = strHTML.Replace("<!taxLimit2Wo!>", "Rs. " + Convert.ToDouble(taxLimit2Wo.Value).ToString("n0"));


            if (taxLimit3.Value.Length > 0)
                strHTML = strHTML.Replace("<!taxLimit3!>", "Rs. " + Convert.ToDouble(taxLimit3.Value).ToString("n0"));
            if (taxLimit3Wo.Value.Length > 0)
                strHTML = strHTML.Replace("<!taxLimit3Wo!>", "Rs. " + Convert.ToDouble(taxLimit3Wo.Value).ToString("n0"));



            if (taxLimit4.Value.Length > 0)
                strHTML = strHTML.Replace("<!taxLimit4!>", "Rs. " + Convert.ToDouble(taxLimit4.Value).ToString("n0"));
            if (taxLimit4Wo.Value.Length > 0)
                strHTML = strHTML.Replace("<!taxLimit4Wo!>", "Rs. " + Convert.ToDouble(taxLimit4Wo.Value).ToString("n0"));



            if (txtTotalTax.Value.Length > 0)
            {
                strHTML = strHTML.Replace("<!txtTotalTax!>", "Rs. " + Convert.ToDouble(txtTotalTax.Value).ToString("n0"));
                strHTML = strHTML.Replace("<!txtTotalTaxWo!>", "Rs. " + Convert.ToDouble(txtTotalTaxWo.Value).ToString("n0"));
            }
            if (txtEduCess.Value.Length > 0)
            {
                strHTML = strHTML.Replace("<!txtEduCess!>", "Rs. " + Convert.ToDouble(txtEduCess.Value).ToString("n0"));
                strHTML = strHTML.Replace("<!txtEduCessWo!>", "Rs. " + Convert.ToDouble(txtEduCessWo.Value).ToString("n0"));
            }

            if (txtTaxSaved.Value.Length > 0)
                strHTML = strHTML.Replace("<!txtTaxSaved!>", "Rs. " + Convert.ToDouble(txtTaxSaved.Value).ToString("n0"));


            if (textAge.Value.Length > 0) strHTML = strHTML.Replace("<!textAge!>", Convert.ToDouble(textAge.Value).ToString("n0"));
            if (txtHomeLoanInt.Value.Length > 0) strHTML = strHTML.Replace("<!txtHomeLoanInt!>", "Rs. " + Convert.ToDouble(txtHomeLoanInt.Value).ToString("n0"));
            if (spanRgessTaxExempt.Text.Length > 0) strHTML = strHTML.Replace("<!spanRgessTaxExempt!>", Convert.ToDouble(spanRgessTaxExempt.Text).ToString("n0"));

            if (spanTaxfreeLimit.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit!>", Convert.ToDouble(spanTaxfreeLimit.Text).ToString("n0"));
            if (spanTaxfreeLimit1.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit1!>", Convert.ToDouble(spanTaxfreeLimit1.Text).ToString("n0"));
            if (spanTaxfreeLimit2.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit2!>", Convert.ToDouble(spanTaxfreeLimit2.Text).ToString("n0"));
            if (spanTaxfreeLimit3.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit3!>", Convert.ToDouble(spanTaxfreeLimit3.Text).ToString("n0"));
            if (spanTaxfreeLimit4.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit4!>", Convert.ToDouble(spanTaxfreeLimit4.Text).ToString("n0"));
            if (spanTaxfreeLimit5.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxfreeLimit5!>", Convert.ToDouble(spanTaxfreeLimit5.Text).ToString("n0"));

            if (spanTaxLimit1Rate.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxLimit1Rate!>", Convert.ToDouble(spanTaxLimit1Rate.Text).ToString("n0"));
            if (spanTaxLimit2Rate.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxLimit2Rate!>", Convert.ToDouble(spanTaxLimit2Rate.Text).ToString("n0"));
            if (spanTaxLimit3Rate.Text.Length > 0) strHTML = strHTML.Replace("<!spanTaxLimit3Rate!>", Convert.ToDouble(spanTaxLimit3Rate.Text).ToString("n0"));

            if (spanEducationCessRate.Text.Length > 0) strHTML = strHTML.Replace("<!spanEducationCessRate!>", Convert.ToDouble(spanEducationCessRate.Text).ToString("n0"));

            if (txtECess.Value.Length > 0)
                strHTML = strHTML.Replace("<!txtECess!>", "Rs. " + Convert.ToDouble(txtECess.Value).ToString("n0"));
            if (txtECessWo.Value.Length > 0)
                strHTML = strHTML.Replace("<!txtECessWo!>", "Rs. " + Convert.ToDouble(txtECessWo.Value).ToString("n0"));

            if (textAge.Value.Length > 0 && Convert.ToInt32(textAge.Value) >= 80)
                strHTML = strHTML.Replace(@"<tr id=""rowtaxFreelimit"" style=""display:inline"">", @"<tr id=""rowtaxFreelimit"" style=""display:none"">");


            if (spanTaxExempt80c.Text.Length > 0)
                strHTML = strHTML.Replace("<!spanTaxExempt80c!>", Convert.ToDouble(spanTaxExempt80c.Text).ToString("n0"));

            if (spanMedicalClaim.Text.Length > 0)
                strHTML = strHTML.Replace("<!spanMedicalClaim!>", Convert.ToDouble(spanMedicalClaim.Text).ToString("n0"));


            #endregion

            strHTML = strHTML.Replace(@"class=""row1""", @"style = 'background:#e3e8ec;	height:26px;	width:230px;	color:#000;	font-family:Arial, Helvetica, sans-serif;	font-size:12px;	vertical-align:middle;	text-align:left;	border-bottom:#fff solid 1px;'");
            strHTML = strHTML.Replace(@"class=""row2""", @"style = 'background:#e3e8ec;	height:26px;	width:215px;	color:#000;	font-family:Arial, Helvetica, sans-serif;	font-size:12px;	vertical-align:middle;	text-align:left;	margin-bottom: 2px;	border-bottom: #fff solid 1px;	padding-left: 3px;'");
            strHTML = strHTML.Replace(@"class=""row3""", @"style = 'background:#fff;height:26px;width:215px;color:#0f8ccf;font-family:Arial, Helvetica, sans-serif;	font-size:12px;	border:#7c868e 1px solid;	vertical-align:middle;	text-align:center;	margin-left: 3px;	margin-right: 2px;	margin-bottom: 2px;'");
            strHTML = strHTML.Replace(@"class=""row31""", @"style = 'background:#e3e8ec;height:26px;width:215px;color:#000;font-family:Arial, Helvetica, sans-serif;	font-size:12px;	border:#7c868e 1px solid;	vertical-align:middle;	text-align:center;	margin-left: 3px;	margin-right: 2px;	margin-bottom: 2px;'");
            strHTML = strHTML.Replace(@"class=""row4""", @"style = 'background:#e3e8ec;	height:26px;	width:215px;	color:#000;	font-family:Arial, Helvetica, sans-serif;	font-size:12px;	vertical-align:middle;	text-align:center;	border: #7c868e solid 1px;	margin-bottom: 2px;'");
            strHTML = strHTML.Replace(@"class=""row5""", @"style = 'background:#e3e8ec;	height:37px;	width:230px;	color:#000;	font-family:Arial, Helvetica, sans-serif;	font-size:12px;	vertical-align:middle;	text-align:left;	padding-left: 3px;'");
            strHTML = strHTML.Replace(@"class=""row6""", @"style = 'height:15px;'");
            strHTML = strHTML.Replace(@"class=""row7""", @"style = 'background:#fff;	height:26px;	width:215px;color:#000;font-family:Arial, Helvetica, sans-serif;	font-size:12px;	border:#7c868e 1px solid;vertical-align:middle;	text-align:center;	margin-left: 3px;	margin-right: 2px;'");
            strHTML = strHTML.Replace(@"class=""row8""", @"style = 'background:#e3e8ec;	height:26px;	width:190px;	color:#000;	font-family:Arial, Helvetica, sans-serif;	font-size:12px;	vertical-align:middle;	text-align:center;	border-bottom:#7c868e  solid 1px;'");
            strHTML = strHTML.Replace(@"class=""row9""", @"style = 'background:#e3e8ec;	height:26px;	width:190px;	color:#000;	font-family:Arial, Helvetica, sans-serif;	font-size:12px;	vertical-align:middle;	text-align:center;'");
            strHTML = strHTML.Replace(@"class=""row10""", @"style = 'height:37px;	width:215px;	color:#fff;	font-family:Arial, Helvetica, sans-serif;	font-size:12px;	vertical-align:middle;	text-align:left;	padding-left: 3px;	border:#7c868e 1px solid;	margin-left: 3px;	margin-right: 2px;'");
            strHTML = strHTML.Replace(@"class=""row11""", @"style = 'background:#e3e8ec;	height:37px;	width:215px;	color:#000;	font-family:Arial, Helvetica, sans-serif;	font-size:12px;	vertical-align:middle;	text-align:center;	border: #7c868e solid 1px;	margin-bottom: 2px;'");
            strHTML = strHTML.Replace(@"class=""heading_text""", @"style = 'color:#2296d3; font-weight:bold; font-size:12px; vertical-align:middle; text-align:center; height:25px;'");
            strHTML = strHTML.Replace(@"class=""blueBox1""", @"style = 'height: 100%;	border-bottom: 2px solid #0f8ccf;	border-top: 6px solid #0f8ccf;	background:#e3e8ec;'");
            strHTML = strHTML.Replace(@"class=""bottom_row1""", @"style = 'border-bottom:#7c868e  solid 1px;	height:26px;	color:#000000;	font-size:12px;	padding-left:3px;'");
            strHTML = strHTML.Replace(@"class=""bottom_altrow1""", @"style = 'height:26px; color:#000000; font-size:12px; padding-left:3px; '");
            strHTML = strHTML.Replace(@"class=""rupee_blue""", @"style = 'height:10px;	width:20px;	vertical-align:middle;	margin:10px 0 0 10px;	text-align: right;'");

            return strHTML.ToString();

        }

        private void ShowExcelFromHtml()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DSPTax.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            string tempStr = string.Empty;

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Text.StringBuilder objFinalstr = new System.Text.StringBuilder();

            string ImagePath = string.Empty;
            ImagePath = HttpContext.Current.Server.MapPath("~") + "DSP\\img\\";

            // divTax.RenderControl(hw);


            tempStr = strHtmlDesign();

            objFinalstr.Append("<br/>");
            objFinalstr.Append("<table width='680px' border ='1' style='text-align:justify'><tr><td width='100%'>");
            objFinalstr.Append(tempStr);
            objFinalstr.Append("<br/><b>Mutual Fund investments are subject to market risks, read all scheme related documents carefully.</b>");
            //objFinalstr.Replace("img/rupee_black.png", ImagePath + "rupee_black.png"); 

            objFinalstr.Append("</td></tr></table>");
            objFinalstr.Append("<br/>");


            Response.Write(objFinalstr.ToString());
            Response.Flush();
            Response.End();
        }

        protected void RadioButtonListCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistributerDiv.Visible = RadioButtonListCustomerType.SelectedItem.Text.ToUpper().Trim() == "DISTRIBUTOR";
            Showpdfdiv.Attributes.Add("style", "display:");
        }
    }
}