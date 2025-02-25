using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace iFrames.DSP.Retirement
{
	public partial class WebMethod : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		[WebMethod]
		public static string getExpectdMonthRetCorp(double ExpectedCorpus, double IntrestRate, string startDate, MainSipMonthly objMainSipMonthly, MainLsInvestment objMainLsInvestment, object objAddedSip, object objAddedLs)
		{
			double MonthlyAmount = objMainSipMonthly.SipAmount;
			int MainSipYearMonth = objMainSipMonthly.TotalYear * 12;


			DateTime sstartDate = new DateTime(Convert.ToInt16(startDate.Split('/')[2]),
									   Convert.ToInt16(startDate.Split('/')[1]),
									   Convert.ToInt16(startDate.Split('/')[0]));

			double SipCumulativeAmount = 0;// MonthlyAmount;
			double LsinvestmentMain = objMainLsInvestment.InvestmentAmount;
			double LsCumulativeAmount = LsinvestmentMain;
			int MonthCounter = 0;
			double TotalCumulativeAmount = 0;
			bool isAddedSip = false, isAddedLs = false;

			List<AddedSipMonthly> listAddedSipMonthly = new List<AddedSipMonthly>();
			listAddedSipMonthly = new JavaScriptSerializer().Deserialize<List<AddedSipMonthly>>(objAddedSip.ToString());



			List<AddedLsInvestment> listAddedLsInvestment = new List<AddedLsInvestment>();
			listAddedLsInvestment = new JavaScriptSerializer().Deserialize<List<AddedLsInvestment>>(objAddedLs.ToString());

			if (listAddedSipMonthly.Count > 0)
			{
				isAddedSip = true;
			}

			if (listAddedLsInvestment.Count > 0)
			{
				isAddedLs = true;
			}


			List<int> StartMonthSIP = new List<int>();
			List<int> TotalMonthSIP = new List<int>();
			List<double> AddedMonthlySIPAmount = new List<double>();
			List<double> TotalAddedMonthlySIPAmount = new List<double>();

			if (isAddedSip)
			{
				foreach (AddedSipMonthly objSip in listAddedSipMonthly)
				{
					StartMonthSIP.Add(objSip.AfterYear * 12);
					TotalMonthSIP.Add((objSip.AfterYear * 12) + (objSip.TotalYear * 12));
					AddedMonthlySIPAmount.Add(objSip.SipAmount);
					TotalAddedMonthlySIPAmount.Add(0);
				}
			}


			List<int> StartMonthLS = new List<int>();
			List<double> AddedLSInvstAmount = new List<double>();
			List<double> TotalLsInvAmount = new List<double>();

			if (isAddedLs)
			{
				foreach (AddedLsInvestment objLs in listAddedLsInvestment)
				{
					StartMonthLS.Add(objLs.AfterYear * 12);
					AddedLSInvstAmount.Add(objLs.InvestmentAmount);
					TotalLsInvAmount.Add(objLs.InvestmentAmount);
				}
			}



			while (TotalCumulativeAmount <= ExpectedCorpus)
			{
				if (MonthCounter <= MainSipYearMonth)
					SipCumulativeAmount = getReturnMonth(SipCumulativeAmount + MonthlyAmount, IntrestRate);
				else
					SipCumulativeAmount = getReturnMonth(SipCumulativeAmount, IntrestRate);

				LsCumulativeAmount = getReturnMonth(LsCumulativeAmount, IntrestRate);
				TotalCumulativeAmount = SipCumulativeAmount + LsCumulativeAmount;


				if (isAddedSip)
				{
					for (int i = 0; i < listAddedSipMonthly.Count; i++)
					{
						if (MonthCounter >= StartMonthSIP[i])
						{
							if (MonthCounter < TotalMonthSIP[i])
							{
								TotalAddedMonthlySIPAmount[i] = getReturnMonth(TotalAddedMonthlySIPAmount[i] + AddedMonthlySIPAmount[i], IntrestRate);
							}
							else
							{
								TotalAddedMonthlySIPAmount[i] = getReturnMonth(TotalAddedMonthlySIPAmount[i], IntrestRate);
							}

							TotalCumulativeAmount += TotalAddedMonthlySIPAmount[i];
						}
					}
				}


				if (isAddedLs)
				{
					for (int i = 0; i < listAddedLsInvestment.Count; i++)
					{
						if (MonthCounter >= StartMonthLS[i])
						{
							TotalLsInvAmount[i] = getReturnMonth(TotalLsInvAmount[i], IntrestRate);
							TotalCumulativeAmount += TotalLsInvAmount[i];
						}
					}
				}



				MonthCounter++;
			};


			decimal Totalyear = Math.Floor(Convert.ToDecimal(MonthCounter / 12));
			int month = Convert.ToInt32(MonthCounter % 12);

			//DateTime ExpectedMaturityDate = sstartDate.AddMonths(MonthCounter);
			//return Totalyear.ToString() + " Years and " + month.ToString() + " Months#" + ExpectedMaturityDate.ToString("dd/MM/yyyy");

			sstartDate = sstartDate.AddMonths(MonthCounter);
			return Totalyear.ToString() + " Years and " + month.ToString() + " Months#" + sstartDate.ToString("dd/MM/yyyy");
			// return Totalyear.ToString() + " Years and " + month.ToString() + " Months#" + sstartDate.ToString("MMMM yyyy");


		}

		[WebMethod]
		public static double getReturnMonth(double Amount, double IntrestRate)
		{
			return Math.Round((Amount + Convert.ToDouble((Amount * IntrestRate) / (100 * 12))), 8);
		}

		[WebMethod]
		public static double getReturnMonth(double Amount, double IntrestRate, Int16 Roundupto)
		{
			return Math.Round((Amount + Convert.ToDouble((Amount * IntrestRate) / (100 * 12))), Roundupto);
		}


		[WebMethod]
		public static double CalculateCompundInterest(double PrinciPal, double Rate, int Year, int NumTimes)
		{
			double ret;
			ret = Math.Pow(Convert.ToDouble((1 + (Rate / (100 * NumTimes)))), Convert.ToDouble(NumTimes * Year));
			ret = PrinciPal * ret;
			return Math.Round(ret, 8);
		}

		[WebMethod]
		public static double CalculateSimpleInterest(double PrinciPal, double Rate, int Time)
		{
			double ret;
			ret = (PrinciPal * Rate * Time) / 100;
			ret = PrinciPal + ret;
			return Math.Round(ret, 8);
		}

		[WebMethod]
		public static int GetRequiredRetirementCorpus(double Corpus, double MonthlyExpense, double InfRate, double InterestRate, double ExpenseRate)
		{

			// var valArray = [[]];
			double deduc;
			deduc = MonthlyExpense * 12;
			InfRate = InfRate + ExpenseRate;
			InfRate = 1 + (InfRate / 100);
			InterestRate = 1 + (InterestRate / 100);

			List<Corpus_Deduc> ListCorpus_Deduc = new List<Corpus_Deduc>();

			while (Corpus > 0 && Corpus > deduc)
			{
				//  valArray.push(new Array(Math.round(Corpus), Math.round(deduc)));
				ListCorpus_Deduc.Add(new Corpus_Deduc { Corpus = Corpus, Deduction = deduc });
				Corpus -= deduc;
				Corpus = Corpus * InterestRate;
				deduc = deduc * InfRate;

			}

			return ListCorpus_Deduc.Count;


		}


		[WebMethod]
		public static double RetirementCorpusAmount(int Year, double MonthlyExpense, double InfRate, double InterestRate, double ExpenseRate = 0)
		{
			//var MonthlyExpenseArray = [];

			List<double> listMonthlyExpenseArray = new List<double>();
			var temppYear = Year;
			double deduc;
			deduc = MonthlyExpense * 12;
			InfRate = InfRate + ExpenseRate;
			InfRate = 1 + (InfRate / 100);
			//InterestRate = 1 + (InterestRate / 100);

			while (temppYear > 0)
			{
				//document.write(deduc + "<br />");
				//MonthlyExpenseArray.push(Math.round(deduc));
				listMonthlyExpenseArray.Add(deduc);
				deduc = deduc * InfRate;
				temppYear--;
			}

			var FinalBalance = listMonthlyExpenseArray[listMonthlyExpenseArray.Count - 1];
			//var YearlyBalanceArray = [];
			List<double> listYearlyBalance = new List<double>();
			listYearlyBalance.Add(FinalBalance);
			for (var key = listMonthlyExpenseArray.Count - 1; key > 0; key--)
			{
				//document.write(FinalBalance + "<br />");+
				FinalBalance = (FinalBalance * 100) / (100 + InterestRate);
				FinalBalance = Math.Round(FinalBalance, 8) + Math.Round(listMonthlyExpenseArray[key - 1], 8);
				listYearlyBalance.Add(FinalBalance);
			}

			double finalAmnt = listYearlyBalance[listYearlyBalance.Count - 1];
			return Math.Round(finalAmnt);
			//  alert(finalAmnt);
		}


		#region NPS
		[WebMethod]
		public static double FutureValue(double Rate, double nper, double pmt, double pv)
		{
			return System.Numeric.Financial.Fv(Rate, nper, pmt, pv, System.Numeric.PaymentDue.EndOfPeriod);
		}

		[WebMethod]
		public static double GetExpectedReturn(double IntrestRate, double SipAmount, int TotalYear, short RoundOff)
		{
			double MonthlyAmount = SipAmount;
			int MainSipYearMonth = TotalYear * 12;
			double SipCumulativeAmount = 0;
			int MonthCounter = 0;
			double TotalCumulativeAmount = 0;

			while (MonthCounter < MainSipYearMonth)
			{
				if (MonthCounter <= MainSipYearMonth)
					SipCumulativeAmount = getReturnMonth(SipCumulativeAmount + MonthlyAmount, IntrestRate, RoundOff);
				else
					SipCumulativeAmount = getReturnMonth(SipCumulativeAmount, IntrestRate, RoundOff);
				TotalCumulativeAmount = SipCumulativeAmount;
				MonthCounter++;
			};
			//new generate asp.net chart
			//var totInvest = SipAmount * TotalYear * 12;
			//NpsGenereteChartImg(totInvest, TotalCumulativeAmount);
			//new 
			return TotalCumulativeAmount;
		}
		[WebMethod]
		public static void setNPSChartSession(string chartStr)
		{
			HttpContext.Current.Session["chartStr"] = chartStr;
		}

		[WebMethod]
		public static void setNPSChartimg(string baseimg)
		{
			var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\IMG\\dsp_ins_pdf_img", "*NPS_Chart*");
			foreach (var f in allFiles)
				if (File.GetCreationTime(f) < DateTime.Now.AddMinutes(-1))
					File.Delete(f);

			byte[] data = System.Convert.FromBase64String(baseimg.Replace("data:image/png;base64,", ""));
			//var file = new FileStream(@"C:\Sanjay D\abcd.png", FileMode.OpenOrCreate);
			string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
			string localImgPath = @"DSP\IMG\dsp_ins_pdf_img\";
			localImgPath = System.IO.Path.Combine(appPath, localImgPath);

			//comment new
			//string imgPath = System.IO.Path.Combine(localImgPath + Guid.NewGuid().ToString("N").Replace("-", "") + "NPS_Chart.png");
			//var file = new FileStream(imgPath, FileMode.OpenOrCreate);
			//file.Write(data, 0, data.Length - 1);
			//file.Close();
			//---

			//add new 
			string imgPath = System.IO.Path.Combine(localImgPath + Guid.NewGuid().ToString("N").Replace("-", "") + "NPS_Chart.jpg");
			using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(data)))
			{
				image.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			//--
			HttpContext.Current.Session["DSP_NPS_Chart_IMG_PATH"] = imgPath;
		}
		[WebMethod]
		public static double NpsGenereteChartImg(double totInvest, double interestEarned)
		{
			var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\IMG\\dsp_ins_pdf_img", "*NPS_Chart*");
			foreach (var f in allFiles)
				if (File.GetCreationTime(f) < DateTime.Now.AddMinutes(-1))
					File.Delete(f);

			string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
			string localImgPath = @"DSP\IMG\dsp_ins_pdf_img\";
			localImgPath = System.IO.Path.Combine(appPath, localImgPath);
			string imgPath = System.IO.Path.Combine(localImgPath + Guid.NewGuid().ToString("N").Replace("-", "") + "NPS_Chart.jpg");
			
			System.Web.UI.DataVisualization.Charting.Chart achart = new System.Web.UI.DataVisualization.Charting.Chart();
			achart.Width = 150;
			achart.Height = 150;
			achart.BackColor = Color.White;
			achart.Series.Add("PieSeries");
			achart.ChartAreas.Add("ChartArea1");
			achart.ChartAreas[0].Area3DStyle.Enable3D = false;
			Dictionary<string, double> testData = new Dictionary<string, double>();
			testData.Add("", totInvest);
			testData.Add(" ", interestEarned);
			achart.Series["PieSeries"].Points.DataBind(testData, "Key", "Value", string.Empty);
			achart.Series["PieSeries"].ChartTypeName = "Pie";
			achart.Series["PieSeries"].Points[0].Color = System.Drawing.ColorTranslator.FromHtml("#2895ce");
			achart.Series["PieSeries"].Points[1].Color = System.Drawing.ColorTranslator.FromHtml("#d9192b");
			achart.Series["PieSeries"].IsValueShownAsLabel = false;
			achart.ChartAreas[0].Area3DStyle.Enable3D = false;
			achart.ChartAreas[0].Area3DStyle.Rotation = 10;
			achart.ChartAreas[0].Area3DStyle.Perspective = 10;
			achart.ChartAreas[0].Area3DStyle.Inclination = 15;
			achart.ChartAreas[0].Area3DStyle.IsRightAngleAxes = false;
			achart.ChartAreas[0].Area3DStyle.WallWidth = 0;
			achart.ChartAreas[0].Area3DStyle.IsClustered = false;
			achart.SaveImage(imgPath);
			HttpContext.Current.Session["DSP_NPS_Chart_IMG_PATH"] = imgPath;
			return 100;
		}

		[WebMethod]
		public static void setValueChartimg(string baseimg)
		{
			var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "DSP\\IMG\\dsp_ins_pdf_img", "*NPS_Chart*");
			foreach (var f in allFiles)
				if (File.GetCreationTime(f) < DateTime.Now.AddMinutes(-1))
					File.Delete(f);

			byte[] data = System.Convert.FromBase64String(baseimg.Replace("data:image/png;base64,", ""));
			//var file = new FileStream(@"C:\Sanjay D\abcd.png", FileMode.OpenOrCreate);
			string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
			string localImgPath = @"DSP\IMG\dsp_ins_pdf_img\";
			localImgPath = System.IO.Path.Combine(appPath, localImgPath);

			//comment new
			//string imgPath = System.IO.Path.Combine(localImgPath + Guid.NewGuid().ToString("N").Replace("-", "") + "NPS_Chart.png");
			//var file = new FileStream(imgPath, FileMode.OpenOrCreate);
			//file.Write(data, 0, data.Length - 1);
			//file.Close();
			//---

			//add new 
			string imgPath = System.IO.Path.Combine(localImgPath + Guid.NewGuid().ToString("N").Replace("-", "") + "NPS_Chart.jpg");
			using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(data)))
			{
				image.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			//--
			HttpContext.Current.Session["DSP_NPS_Chart_IMG_PATH"] = imgPath;
		}
		#endregion
	}
}