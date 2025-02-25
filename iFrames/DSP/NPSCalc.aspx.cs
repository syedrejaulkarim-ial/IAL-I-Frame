using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.DSP.Retirement;
using System.IO;
using WkHtmlToXSharp;
using NUnit.Framework;
using System.Drawing;

namespace iFrames.DSP
{
	public partial class NPSCalc : System.Web.UI.Page
	{
		public static string SimplePageFile = null;
		public static int count = 0;
		private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		protected void Page_Load(object sender, EventArgs e)
		{
			MainSipMonthly objSip = new MainSipMonthly() { SipAmount = 5000, TotalYear = 27 };
		}

		private string FillHtmlGridViewTable(System.Web.UI.HtmlControls.HtmlGenericControl objSipGridView)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			try
			{
				HttpContext.Current.Response.Clear();
				using (System.IO.StringWriter stringWrite = new System.IO.StringWriter())
				{
					using (System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite))
					{
						if (objSipGridView.InnerHtml.Length > 0)
						{
							objSipGridView.RenderControl(htmlWrite);
							sb.Append(stringWrite.ToString());
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

		public void CreateHTMLDSPSIP()
		{
			System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
			string gvFirstTablestr = string.Empty;
			string GridViewSIPResultstr = string.Empty;
			string sipGridViewstr = string.Empty;
			string path = string.Empty;

			System.Globalization.CultureInfo CInfo = new System.Globalization.CultureInfo("hi-IN");
			var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\PDF", "DSP_*");
			foreach (var f in allFiles)
				if (File.GetCreationTime(f) < DateTime.Now.AddMinutes(-1))
					File.Delete(f);
			try
			{

				strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/DSP/DSP_NPS_PDF.html")));

				#region format PDF

				strHTML.Replace("<!txtAge!>", hdCurrentAge.Value.ToString());
				strHTML.Replace("<!txtExpRetirement!>", hdExpRetirementAge.Value.ToString());//sipAmt
				strHTML.Replace("<!txtMonthlyContribution!>", sipAmt.Value.ToString());
				strHTML.Replace("<!txtExpReturn!>", hdExpReturn.Value.ToString());
				strHTML.Replace("<!txtWealthOnRetirement!>", Convert.ToDouble(hdWealthOnRetirement.Value).ToString("n0", CInfo));
				strHTML.Replace("<!txtCurpus!>", hdCurpus.Value.ToString());
				strHTML.Replace("<!txtCurpusWithdralAmt!>", Convert.ToDouble(hdCurpusWithdralAmt.Value).ToString("n0", CInfo));
				strHTML.Replace("<!txtCorpusAnnuitiMonthPension!>", Convert.ToDouble(hdCorpusAnnuitiMonthPension.Value).ToString("n0", CInfo));
				strHTML.Replace("<!txtExpectedRateOfAnnuiti!>", Convert.ToDouble(hdExpectedRateOfAnnuiti.Value).ToString("n0", CInfo));
				strHTML.Replace("<!txtPensionEarnedPerMonth!>", Convert.ToDouble(hdPensionEarnedPerMonth.Value).ToString("n0", CInfo));
				strHTML.Replace("<!txtTaxSaved!>", Convert.ToDouble(hdTaxSaved.Value).ToString("n0", CInfo));
				strHTML.Replace("<!txtTax!>", hdTax.Value.ToString());

				#region for chart image

				string ChartimagePath = Convert.ToString(HttpContext.Current.Session["DSP_NPS_Chart_IMG_PATH"]);
				if (!string.IsNullOrEmpty(ChartimagePath))
				{
					var strIngChartName = ChartimagePath.Replace("\\", "/").Split('/');
					var ingName = "../IMG/dsp_ins_pdf_img/" + strIngChartName[strIngChartName.Length - 1];
					if (File.Exists(ChartimagePath))
					{
						strHTML = strHTML.Replace("<!image!>", "<img width='50%' src='" + ingName + "'  style=\"border:none\"/>");
						//strHTML = strHTML.Replace("<!image!>", "<img width='80%' height='80%' src='" + ingName + "' />");
					}
				}
				#endregion for chart image

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
					Response.AppendHeader("Content-Disposition", "attachment; filename=DSP_Nps.pdf");
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
		protected void LinkButtonGenerateReport_Click(object sender, EventArgs e)
		{
			CreateHTMLDSPSIP();
		}

		public string addcommas(string nstr)
		{
			string str = nstr.ToString();
			string rtnStr = "";
			while (Convert.ToDecimal(str) % 1000 > 0)
			{
				rtnStr = "," + Convert.ToDecimal(str) % 1000 + rtnStr;
				str = str.Substring(0, str.Length - 3);
			}
			return rtnStr;
		}

		//public string addcommas(string nstr)
		//{
		//	nstr += "";
		//	var x = nstr.Split('.');
		//	var x1 = x[0];
		//	var x2 = x.Length > 1 ? "." + x[1] : "";
		//	string rgx = @"/(\d+)(\d{3})/";
		//	//var rgx = "/(\\d+)(\\d{3})/";

		//	System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(nstr, x1, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		//	 //while (rgx.test(x1))
		//	while (match.Success)
		//	{
		//		x1 = x1.Replace(rgx, "$1" + ',' + "$2");
		//	}
		//	return x1 + x2;
		//}
	}
}