using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;
using System.Web.UI.DataVisualization.Charting;
using System.Text;
using System.Globalization;
using System.IO;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Data.SqlClient;


namespace iFrames.Edelweiss
{
    public partial class SipCalc : System.Web.UI.Page
    {
        #region Global Variables


        protected string Values;
        protected string SelectedValues;
        public EdelweissSchemes[] EdelweissProvidedSchemes { get; set; }
        public bool IsGSM { get; set; }
        string strError = string.Empty;
        string imgPath = string.Empty;
        DemoEdelweissMethods objEdel;// = new EdelweissMethods();
        IList<Repository_Scheme_Details> RepositorySchemes = new List<Repository_Scheme_Details>();
        static Dictionary<string, EdelweissProcess> ProcessList = new Dictionary<string, EdelweissProcess>();
        EdelweissProcess CurrentProcess { get; set; }

        Dictionary<string, string> InvestmentMode = new Dictionary<string, string>();

        /// <summary>
        /// 12,49 is hard coded due to client has specified 
        /// Nifty 50 and Nifty Free Float 100
        /// </summary>
        /// 

        private static EdelSchemeIndex[] _edelIndex = null;
        public static EdelSchemeIndex[] EdelIndexes
        {
            get
            {
                if (_edelIndex == null)
                    _edelIndex = DemoEdelweissMethods.GetIndex(new int[] { 12, 49 });
                return _edelIndex;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                reportPanel.Visible = false;
                string i_mod = Request.QueryString.AllKeys.Contains("i_mod") ? Request.QueryString["i_mod"].ToString() : null;
                string s_scheme = Request.QueryString.AllKeys.Contains("s_scheme") ? Request.QueryString["s_scheme"].ToString() : null; //amfi code
                string t_scheme = Request.QueryString.AllKeys.Contains("t_scheme") ? Request.QueryString["t_scheme"].ToString() : null;//amfi code
                objEdel = new DemoEdelweissMethods();
                var data = DemoEdelweissMethods.GetIndex(new int[] { 1, 2 });
                LoadSchemes();
                if (!IsPostBack)
                {
                    reportPanel.Visible = false;
                    LoadToScheme(IsGSM);
                    LoadTriggarAmount(IsGSM);
                    LoadFromScheme(IsGSM);
                    LoadIndex(Convert.ToInt32(ddlToScheme.SelectedValue));
                    if (ProcessList.Count == 0)
                    {
                        ProcessList.Add("Power STP", EdelweissProcess.PrepaidSTP);
                        ProcessList.Add("Gain Switch Mechanism", EdelweissProcess.GainSwitchSIP);
                        ProcessList.Add("Power SIP", EdelweissProcess.PrepaidSIP);
                    }


                    if (InvestmentMode.Count == 0)
                    {
                        InvestmentMode.Add("pstp", "Power STP");
                        InvestmentMode.Add("psip", "Power SIP");
                        InvestmentMode.Add("gsm", "Gain Switch Mechanism");
                    }

                    if (!String.IsNullOrEmpty(i_mod))
                    {
                        ddlSIPType.SelectedValue = InvestmentMode[i_mod];
                        //  CurrentProcess = ProcessList[ddlSIPType.Text];

                        ddlSIPType_SelectedIndexChanged(null, null);
                        // OnChangeSipType(CurrentProcess);
                        if (!String.IsNullOrEmpty(s_scheme))
                        {
                            string FromSchemeId = GetSchemeidByAmfiCode(s_scheme);
                            if (!String.IsNullOrEmpty(FromSchemeId))
                            {
                                ddlFromScheme.SelectedValue = FromSchemeId;
                                ddlFromScheme_SelectedIndexChanged(null, null);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error Message: ", "alert('Oops!!! Wrong source scheme vaule.')", true);
                            }
                        }
                        if (!String.IsNullOrEmpty(t_scheme))
                        {
                            string TargetSchemeId = GetSchemeidByAmfiCode(t_scheme);
                            if (!String.IsNullOrEmpty(TargetSchemeId))
                            {
                                ddlToScheme.SelectedValue = TargetSchemeId;
                                ddlToScheme_PreRender(null, null);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error Message: ", "alert('Oops!!! Wrong target scheme vaule.')", true);
                            }
                        }
                    }

                }

                CurrentProcess = ProcessList[ddlSIPType.Text];
                ChangeDisplayNoOfInvestment();
                LoadIndex(Convert.ToInt32(ddlToScheme.SelectedValue));
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.Write(ex.Message);
            }

        }

        /// <summary>
        /// Load Index As Per Scheme is Selected
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <returns></returns>
        /// 
        private EdelSchemeIndex[] _edelIndexFromScheme = null;// add new 
        private SchemeNature[] _SchemeNature = null;// add new 
        private EdelSchemeIndex[] _GetAddbenchMark = null;// add new 
        private void LoadIndex(int Scheme_ID)
        {
            EdelSchemeIndex curIndex = null;
            // Check Only For Edelweiss Emerging Fund
            if (Scheme_ID == 17276 || Scheme_ID == 5271)
                curIndex = EdelIndexes.Where(p => p.IndexID == 49).FirstOrDefault();
            else
                curIndex = EdelIndexes.Where(p => p.IndexID == 12).FirstOrDefault();
            if (curIndex != null)
            {
                //txtIndex.Text = curIndex.IndexName;                
                HdnIndexNameWithOutTRI.Value = curIndex.IndexName;
                hdnIndexID.Value = curIndex.IndexID.ToString();
            }
            //Add new For TRI index..         
            EdelSchemeIndex curIndexTRI = null;
            decimal IndexId = objEdel.GetIndexID(Convert.ToDecimal(Scheme_ID));
            _edelIndexFromScheme = DemoEdelweissMethods.GetIndex(new int[] { Convert.ToInt32(IndexId) });
            curIndexTRI = _edelIndexFromScheme.FirstOrDefault();
            //decimal? GetindexTri = objEdel.GetIndexIdTRI(curIndexTRI.IndexName + " TRI");
            EdelSchemeIndex GetindexTri = objEdel.GetIndexIdTRI(curIndexTRI.IndexName + " TRI");
            if (GetindexTri == null)
            {
                // HdnIndexNameWithOutTRI.Value = curIndex.IndexName; //Change now
                // HdnIndexName.Value = curIndex.IndexName;
                //txtIndex.Text = curIndex.IndexName;

                HdnIndexName.Value = curIndexTRI.IndexName;//Change now
            }
            else
            {
                HdnIndexName.Value = GetindexTri.IndexName;
                // txtIndex.Text = curIndexTRI.IndexName + " TRI";
            }
            SchemeNature _natureSubNature = null;
            _SchemeNature = objEdel.GetNatureSubnature(Convert.ToDecimal(Scheme_ID));
            _natureSubNature = _SchemeNature.FirstOrDefault();
            if (_natureSubNature.Nature == "Equity" || _natureSubNature.Nature == "Balanced" || _natureSubNature.Nature == "Dynamic/Asset Allocation")
            {
                if (GetindexTri != null)
                {
                    if (GetindexTri.IndexID == 12)
                    {
                        HdnAddBenchId.Value = Convert.ToString(13);
                    }
                    else
                        HdnAddBenchId.Value = Convert.ToString(12);
                }
                else
                {
                    // if(curIndex.IndexID==12)
                    //     HdnAddBenchId.Value = Convert.ToString(13);
                    // else
                    //     HdnAddBenchId.Value = Convert.ToString(12);

                    //Change now
                    if (curIndexTRI.IndexID == 12)
                        HdnAddBenchId.Value = Convert.ToString(13);
                    else
                        HdnAddBenchId.Value = Convert.ToString(12);
                }
            }
            else if (_natureSubNature.Nature == "Liquid" || _natureSubNature.SubNature == "Ultra Short Term" || _natureSubNature.SubNature == "Floating Rate Fund" || _natureSubNature.SubNature == "FMP" || _natureSubNature.SubNature == "Short Term")
            {
                HdnAddBenchId.Value = Convert.ToString(135);
            }
            else
            {
                HdnAddBenchId.Value = Convert.ToString(134);
            }
            EdelSchemeIndex AddBenchmark = null;
            _GetAddbenchMark = DemoEdelweissMethods.GetIndex(new int[] { Convert.ToInt32(HdnAddBenchId.Value) });
            AddBenchmark = _GetAddbenchMark.FirstOrDefault();
            //Change now 
            EdelSchemeIndex GetAddBenchmark4Tri = objEdel.GetIndexIdTRI(AddBenchmark.IndexName + " TRI");
            if (GetAddBenchmark4Tri == null)
            {
                HdnAddBenchName.Value = AddBenchmark.IndexName;
            }
            else
            {
                HdnAddBenchName.Value = GetAddBenchmark4Tri.IndexName;
            }
            //end
            //HdnAddBenchName.Value = AddBenchmark.IndexName;
            //End
        }

        private void ChangeDisplayNoOfInvestment()
        {
            if (CurrentProcess == EdelweissProcess.PrepaidSIP)
            {
                NoOfInvestmentId.Attributes.Add("style", "visibility:visible");
                NoOfInvestmenttxt.Attributes.Add("style", "visibility:visible");
                TrTriggerGain.Attributes.Add("style", "visibility:hidden");
                TrNoInvestmentSIP.Attributes.Add("style", "visibility:visible");
            }
            else if (CurrentProcess == EdelweissProcess.PrepaidSTP)
            {
                benchmarkChangeText.Style.Add("visibility", "hidden");
                triggarAmt.Style.Add("visibility", "hidden");
                switchamountid.Visible = false;
                NoOfInvestmentId.Attributes.Add("style", "visibility:hidden");
                NoOfInvestmenttxt.Attributes.Add("style", "visibility:hidden");
                TrNoInvestmentSIP.Attributes.Add("style", "visibility:hidden");
                TrTriggerGain.Attributes.Add("style", "visibility:hidden");
            }
            else
            {
                NoOfInvestmentId.Attributes.Add("style", "visibility:hidden");
                NoOfInvestmenttxt.Attributes.Add("style", "visibility:hidden");
                TrTriggerGain.Attributes.Add("style", "visibility:visible");
                TrNoInvestmentSIP.Attributes.Add("style", "visibility:hidden");

            }
        }

        public EdelweissSchemes[] SchemeReader(string myXmLfile)
        {
            DataSet _ds = new DataSet();
            var fsReadXml = new System.IO.FileStream
                (myXmLfile, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            try
            {
                //AddSchemes();
                _ds.ReadXml(fsReadXml);

                DataTable _dtScheme = _ds.Tables[0];
                return (from tbl in _dtScheme.AsEnumerable()
                        select new EdelweissSchemes()
                        {
                            Fund_Name = tbl.Field<string>("Name"),
                            Fund_Id = Convert.ToDecimal(tbl.Field<string>("Id")),
                            Fund_Type = (EdelweissFundType)Enum.Parse(typeof(EdelweissFundType), tbl.Field<string>("Scheme_Type")),
                        }).ToArray();

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //fsReadXml.Close();
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            EdelweissSIPInput objInput = new EdelweissSIPInput();

            try
            {
                if (CurrentProcess == EdelweissProcess.GainSwitchSIP)
                    IsGSM = true;
                else
                    IsGSM = false;

                objInput.SIPProcess = CurrentProcess;

                objInput.FromDate = new DateTime(Convert.ToInt16(txtfrdt.Text.Split('/')[2]),
                                           Convert.ToInt16(txtfrdt.Text.Split('/')[1]),
                                           Convert.ToInt16(txtfrdt.Text.Split('/')[0]));

                objInput.ToDate = new DateTime(Convert.ToInt16(txttodt.Text.Split('/')[2]),
                                         Convert.ToInt16(txttodt.Text.Split('/')[1]),
                                         Convert.ToInt16(txttodt.Text.Split('/')[0]));

                objInput.CurrentDate = new DateTime(Convert.ToInt16(txtcurdt.Text.Split('/')[2]),
                                     Convert.ToInt16(txtcurdt.Text.Split('/')[1]),
                                     Convert.ToInt16(txtcurdt.Text.Split('/')[0]));

                objInput.CalculatorType = ddlSIPType.Text;

                if (!string.IsNullOrEmpty(txtInitialInvestment.Text))
                    objInput.InitialInvestment = Convert.ToInt32(txtInitialInvestment.Text);

                objInput.SourceSchemeId = Convert.ToInt32(ddlFromScheme.SelectedItem.Value);
                objInput.DestinitionSchemeId = Convert.ToInt32(ddlToScheme.SelectedItem.Value);
                objInput.IsGSM = IsGSM;
                //objInput.TRIBenchmark=
                objInput.TRIBenchmark = HdnIndexName.Value; //HdnIndexName.Value;// txtIndex.Text; //ddlBenchmark.SelectedValue;//Change here 
                objInput.AdditionalBenchmark = HdnAddBenchName.Value;//ddlAdditionalBenchmark.SelectedValue;
                objInput.Benchmark = HdnIndexNameWithOutTRI.Value;
                objInput.TriggerCutOff = Convert.ToDecimal(ddlTriggerAmt.SelectedValue);

                if (CurrentProcess == EdelweissProcess.PrepaidSIP || CurrentProcess == EdelweissProcess.PrepaidSTP)//add
                {
                    objInput.NoOfSwitchPerMonth = Convert.ToInt32(ddlNoOfInvestment.SelectedValue);
                    Dictionary<decimal, decimal> triggarRanges = new Dictionary<decimal, decimal>();

                    #region Fetch Switch Range For SIP Plus 24.11.2016

                    string[] triggerValues = Request.Form.GetValues("dynTriggerAmt");
                    string[] switchingValues = Request.Form.GetValues("txtDynSwitchAmt");



                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    this.Values = serializer.Serialize(switchingValues);
                    this.SelectedValues = serializer.Serialize(triggerValues);


                    #region Checks On Selection

                    if (switchingValues.Where(p => p.Trim() == string.Empty).Count() > 0 || switchingValues.Where(p => p.Trim() == "0").Count() > 0)
                    {
                        if (CurrentProcess == EdelweissProcess.PrepaidSIP)
                        {
                            Show("SIP amount shoulb be more than 0. Please check."); return;
                        }
                        if (CurrentProcess == EdelweissProcess.PrepaidSTP)
                        {
                            Show("Switch amount shoulb be more than 0. Please check."); return;
                        }
                    }

                    var CheckSwitchingAmounts = switchingValues.Where(p => Convert.ToDecimal(p) % 1000 != 0).FirstOrDefault();
                    if (CheckSwitchingAmounts != null)
                    {
                        if (CurrentProcess == EdelweissProcess.PrepaidSIP)
                        {
                            Show("SIP amount should be in multiple of 1000. Please check."); return;
                        }
                        if (CurrentProcess == EdelweissProcess.PrepaidSTP)
                        {
                            Show("Switch amount should be in multiple of 1000. Please check."); return;
                        }
                    }


                    var CountItems = from item in triggerValues
                                     group item by item into grp
                                     select new { key = grp.Key, cnt = grp.Count() };

                    var RepeatedItem = CountItems.Where(p => p.cnt > 1).FirstOrDefault();
                    if (RepeatedItem != null)
                    {
                        Show("You have selected " + RepeatedItem.key + " as trigger amount for more than once. Please check."); return;
                    }

                    #endregion

                    for (int i = 0; i < triggerValues.Length; i++)
                    {
                        triggarRanges.Add(Convert.ToDecimal(triggerValues[i]), Convert.ToDecimal(switchingValues[i]));
                    }

                    objInput.SwitchingRange = triggarRanges;
                }


                #endregion


                if (CurrentProcess == EdelweissProcess.GainSwitchSIP)
                    objInput.SwitchAmount = Convert.ToDecimal(ddlTriggerAmt.SelectedValue);
                if (CurrentProcess == EdelweissProcess.PrepaidSTP)
                    objInput.SwitchAmount = Convert.ToDecimal(ddlTriggerAmt.SelectedValue);
                //if (CurrentProcess == EdelweissProcess.SIPPlus)
                //    objInput.SwitchAmtSIPPlus = Convert.ToInt32(txtSwitchAmt.Text);

                if (CurrentProcess == EdelweissProcess.GainSwitchSIP) //|| CurrentProcess == EdelweissProcess.PrepaidSTP)
                {
                    var SwitchAmount = objInput.InitialInvestment * (objInput.SwitchAmount / 100);
                    objInput.MaxSwitch = Convert.ToInt32(Math.Round((objInput.InitialInvestment / SwitchAmount), 0));
                }

                objEdel.Input = objInput;
                //var SIPAmount = objInput.InitialInvestment * (objInput.SwitchAmount / 100);

                var Headertext = IsGSM ? "Capital Appreciation (%)" : "Index Change (%)";

                decimal SIPPlusInvestedAmt = default(decimal); // Since The Invested Amount Is Unknown
                decimal SIPPlusInvestedAmtBenchmark = default(decimal); // Since The Invested Amount Is Unknown
                decimal SIPPlusInvestedAmtAddBenchmark = default(decimal); // Since The Invested Amount Is Unknown
                OutputTriggeredSIP[] TriggardSIPBenchmark = null;
                OutputTriggeredSIP[] TriggardSIPAdditionalBenchmark = null;

                IList<OutputMonthlySIP> MonthlySIP = new List<OutputMonthlySIP>();
                IList<BasicSIP> ARF_SIP = new List<BasicSIP>();
                IList<BasicSIP> Edge_SIP = new List<BasicSIP>();
                IList<BasicSIP> BenchmarkSIP = new List<BasicSIP>();
                IList<BasicSIP> AdditionalBenchmarkSIP = new List<BasicSIP>();

                #region Calculate Basic Data

                var TriggardSIP = CurrentProcess == EdelweissProcess.GainSwitchSIP ? objEdel.CalculateTriggeredGSMSIP(objInput, out strError) : CurrentProcess == EdelweissProcess.PrepaidSTP ? objEdel.CalculateTriggeredSIPChanged(objInput, out strError) : objEdel.CalculateSIPPlus(objInput, out SIPPlusInvestedAmt, out strError);


                #endregion



                #region Main Data Calculation



                DataTable dtSummary = new DataTable();
                dtSummary.Columns.Add("Return", typeof(string));
                dtSummary.Columns.Add("ARF", typeof(string));
                dtSummary.Columns.Add("Edge", typeof(string));
                dtSummary.Columns.Add("PrepaidSIP", typeof(string));
                dtSummary.Columns.Add("MonthlySIP", typeof(string));
                dtSummary.Columns.Add("Benchmark", typeof(string));
                dtSummary.Columns.Add("AdditionalBenchmark", typeof(string));


                List<DateTime> _DtSimpleSip = new List<DateTime>();
                List<double> _value = new List<double>();
                if (CurrentProcess == EdelweissProcess.GainSwitchSIP || CurrentProcess == EdelweissProcess.PrepaidSTP)
                {
                    if (CurrentProcess == EdelweissProcess.PrepaidSTP)
                    {
                        int month = (TriggardSIP.Last().CalculateDate.Month - TriggardSIP.First().CalculateDate.Month);
                        var Count = 0;
                        if (TriggardSIP.First().CalculateDate.Day == 1)
                            month = month + 1;
                        for (DateTime d1 = TriggardSIP.First().CalculateDate; d1 <= TriggardSIP.Last().CalculateDate; d1 = d1.AddMonths(1))
                        {
                            if (TriggardSIP.ToArray().Where(c => c.CalculateDate == d1).Any())
                            {
                                Count++;
                                // _DtSimpleSip.Add(d1);
                            }
                            else
                            {
                                for (DateTime d2 = d1; d2 <= TriggardSIP.Last().CalculateDate; d2 = d2.AddDays(1))
                                {
                                    if (TriggardSIP.ToArray().Where(c => c.CalculateDate == d2).Any())
                                    {
                                        Count++;
                                        // _DtSimpleSip.Add(d2);
                                        break;
                                    }

                                }
                            }
                        }

                        var InvestedAmt = TriggardSIP.Where(x => x.IsTriggard == true).Sum(x => x.Swithch_Amt);
                        objEdel.Input.SwitchAmountForMonthlyStp = InvestedAmt / (Count - 1);
                    }
                    MonthlySIP = CurrentProcess == EdelweissProcess.PrepaidSIP ? objEdel.CalculateMonthlySIPPlus(out strError) : objEdel.CalculateMonthlySIP(out strError);
                    ARF_SIP = CurrentProcess == EdelweissProcess.PrepaidSIP ? null : objEdel.CalculateSIPReturn(objEdel.Input.SourceSchemeId);
                    Edge_SIP = objEdel.CalculateSIPReturn(objEdel.Input.DestinitionSchemeId);
                    BenchmarkSIP = objEdel.CalculateBenchmarkSIP(objEdel.IndexValWithTRI); //change  IndexVal to IndexValWithTRI
                    AdditionalBenchmarkSIP = objEdel.CalculateBenchmarkSIP(objEdel.AdditionalIndexVal);

                    #region Summary Part Calc
                    // This two variables are used only for SIP PLus calculation
                    decimal TotalInvestedForTriggared = default(decimal);
                    decimal TotalInvestedForMonthly = default(decimal);

                    int period = TriggardSIP.Last().CalculateDate.Subtract(objInput.FromDate.Date).Days;
                    // ARF Nav

                    decimal[] Nav_Returns = null; double ARF_Volatility = default(double); decimal[] RebaseARFNav = null; decimal ARF_Drawdown = default(decimal);
                    decimal ARF_Investment = default(decimal); decimal ARF_Profit = default(decimal); double ARF_Return = default(double);


                    Nav_Returns = NavReturns(true);
                    ARF_Volatility = objEdel.CalculateVolatility(Nav_Returns);
                    RebaseARFNav = RebaseValue(Nav_Returns);
                    ARF_Drawdown = objEdel.CalculateDrawdown(RebaseARFNav);
                    //var ARF_Investment = Math.Round(((ARF_SIP.Last().SIP_Nav - AllMethods.Input.InitialInvestment) / AllMethods.Input.InitialInvestment) * 100, 4);
                    ARF_Investment = Math.Round(ARF_SIP.Last().SIP_Nav, 2);
                    ARF_Profit = Math.Round(ARF_SIP.Last().SIP_Nav - objEdel.Input.InitialInvestment, 2);
                    //var ARF_Return = CalculateReturn((float)RebaseARFNav.First(), (float)RebaseARFNav.Last(), period);
                    ARF_Return = SchemeReturn(true);
                    // Edge nav

                    decimal[] Edge_Returns = null; double Edge_Volatility = default(double); decimal[] RebaseEdgeNav = null; decimal Edge_Drawdown = default(decimal);
                    decimal Edge_Investment = default(decimal); decimal Edge_Profit = default(decimal); double Edge_Return = default(double);


                    Edge_Returns = NavReturns(false);
                    Edge_Volatility = objEdel.CalculateVolatility(Edge_Returns);
                    RebaseEdgeNav = RebaseValue(Edge_Returns);
                    Edge_Drawdown = objEdel.CalculateDrawdown(RebaseEdgeNav);
                    //var Edge_Investment = Math.Round(((Edge_SIP.Last().SIP_Nav - AllMethods.Input.InitialInvestment) / AllMethods.Input.InitialInvestment) * 100, 4);
                    Edge_Investment = Math.Round(Edge_SIP.Last().SIP_Nav, 2);
                    Edge_Profit = Math.Round(Edge_SIP.Last().SIP_Nav - objEdel.Input.InitialInvestment, 2);
                    Edge_Return = SchemeReturn(false);


                    // Triggard SIP
                    var SIP_Returns = TriggardSIP.Select(p => p.SIP_Return).ToArray();
                    var SIP_Volatility = objEdel.CalculateVolatility(SIP_Returns);
                    var RebaseSIP = RebaseValue(SIP_Returns);
                    var SIP_Drawdown = objEdel.CalculateDrawdown(RebaseSIP);
                    //var SIP_Investment = Math.Round(((TriggardSIP.Last().SIP_Nav - AllMethods.Input.InitialInvestment) / AllMethods.Input.InitialInvestment) * 100, 4);
                    var SIP_Investment = Math.Round(TriggardSIP.Last().SIP_Nav, 2);

                    decimal SIP_Profit = default(decimal);
                    SIP_Profit = Math.Round(TriggardSIP.Last().SIP_Nav - objEdel.Input.InitialInvestment, 2);

                    double SIP_Return = default(double);

                    SIP_Return = CalculateReturn((float)(TriggardSIP.First().SIP_Nav), (float)(TriggardSIP.Last().SIP_Nav), period);

                    // Monthly SIP
                    var MSIP_Returns = MonthlySIP.Select(p => p.SIP_Return).ToArray();
                    var MSIP_Volatility = objEdel.CalculateVolatility(MSIP_Returns);
                    var RebaseMSIP = RebaseValue(MSIP_Returns);
                    var MSIP_Drawdown = objEdel.CalculateDrawdown(RebaseMSIP);
                    //var MSIP_Investment = Math.Round(((MonthlySIP.Last().SIP_Nav - AllMethods.Input.InitialInvestment) / AllMethods.Input.InitialInvestment) * 100, 4);
                    var MSIP_Investment = Math.Round(MonthlySIP.Last().SIP_Nav, 2);

                    decimal MSIP_Profit = default(decimal);

                    MSIP_Profit = Math.Round(MonthlySIP.Last().SIP_Nav - objEdel.Input.InitialInvestment, 2);



                    double MSIP_Return = default(double);

                    MSIP_Return = CalculateReturn((float)(MonthlySIP.First().SIP_Nav), (float)(MonthlySIP.Last().SIP_Nav), period);

                    // Index
                    //var Index_Returns = objEdel.IndexData.Select(p => p.Index_Value).ToArray();
                    var Index_Returns = IndexReturns();

                    //** Commented as it is not used
                    //var Index_Volatility = objEdel.CalculateVolatility(Index_Returns);
                    //var ReabseIndex = RebaseValue(Index_Returns);
                    //var Index_Drawdown = objEdel.CalculateDrawdown(ReabseIndex);
                    //var Index_Return = BenchMarkReturn(objEdel.IndexValWithTRI); //change  IndexVal to IndexValWithTRI

                    //var Index_Investment = Math.Round(BenchmarkSIP.Last().SIP_Nav, 2);
                    //var Index_Profit = Math.Round(Index_Investment - objEdel.Input.InitialInvestment, 2);

                    // Additional Index

                    var Additional_Index_Returns = AdditionalIndexReturns();
                    var Additional_Index_Volatility = objEdel.CalculateVolatility(Additional_Index_Returns);
                    var Rebase_Additional_Index = RebaseValue(Additional_Index_Returns);
                    var Additional_Index_Drawdown = objEdel.CalculateDrawdown(Rebase_Additional_Index);
                    var Additonal_Index_Return = BenchMarkReturn(objEdel.AdditionalIndexVal);
                    var Additional_Index_Investment = Math.Round(AdditionalBenchmarkSIP.Last().SIP_Nav, 2);
                    var Additional_Index_Profit = Math.Round(Additional_Index_Investment - objEdel.Input.InitialInvestment, 2);

                    //Amount Invested
                    var SipInvestment = Math.Round(Convert.ToDecimal(objEdel.Input.InitialInvestment), 2);
                    dtSummary.Rows.Add(new object[] { "Amount Invested (₹)", "", "", SipInvestment.ToString("N", new CultureInfo("en-US")), SipInvestment.ToString("N", new CultureInfo("en-US")), BenchmarkSIP.Any() ? SipInvestment.ToString("N", new CultureInfo("en-US")) : "-", SipInvestment.ToString("N", new CultureInfo("en-US")) });
                    //End 
                    dtSummary.Rows.Add(new object[] { "Investment Value as on Date (₹)", ARF_Investment.ToString("N", new CultureInfo("en-US")), Edge_Investment.ToString("N", new CultureInfo("en-US")), SIP_Investment.ToString("N", new CultureInfo("en-US")), MSIP_Investment.ToString("N", new CultureInfo("en-US")), BenchmarkSIP.Any() ? Math.Round(BenchmarkSIP.Last().SIP_Nav, 2).ToString("N", new CultureInfo("en-US")) : "-", Additional_Index_Investment.ToString("N", new CultureInfo("en-US")) });
                    dtSummary.Rows.Add(new object[] { "Total Profit (₹)", ARF_Profit.ToString("N", new CultureInfo("en-US")), Edge_Profit.ToString("N", new CultureInfo("en-US")), SIP_Profit.ToString("N", new CultureInfo("en-US")), MSIP_Profit.ToString("N", new CultureInfo("en-US")), BenchmarkSIP.Any() ? Math.Round(Math.Round(BenchmarkSIP.Last().SIP_Nav, 2) - objEdel.Input.InitialInvestment, 2).ToString("N", new CultureInfo("en-US")) : "-", Additional_Index_Profit.ToString("N", new CultureInfo("en-US")) });
                    dtSummary.Rows.Add(new object[] { "Return (%) *", ARF_Return, Edge_Return, SIP_Return, MSIP_Return, objEdel.IndexValWithTRI.Any() ? Convert.ToString(BenchMarkReturn(objEdel.IndexValWithTRI)) : "-", Additonal_Index_Return });
                    // dtSummary.Rows.Add(new object[] { "Volatility (%)", ARF_Volatility, Edge_Volatility, SIP_Volatility, MSIP_Volatility, Index_Volatility, Additional_Index_Volatility });
                    // dtSummary.Rows.Add(new object[] { "Drawdown (%)", ARF_Drawdown, Edge_Drawdown, SIP_Drawdown, MSIP_Drawdown, Index_Drawdown, Additional_Index_Drawdown });

                    #endregion
                }
                else
                {
                    // commented as it is not used
                    //int month = (TriggardSIP.Last().CalculateDate.Month - TriggardSIP.First().CalculateDate.Month);                    
                    //if (TriggardSIP.First().CalculateDate.Day == 1)
                    //    month = month + 1;
                    var Count = 0;
                    var LastCalDate = TriggardSIP.Where(x => x.CalculateDate <= objInput.ToDate).Last().CalculateDate;
                    for (DateTime d1 = TriggardSIP.First().CalculateDate; d1 <= LastCalDate; d1 = d1.AddMonths(1))
                    {
                        if (TriggardSIP.ToArray().Where(c => c.CalculateDate == d1).Any())
                        {
                            Count++;
                            _DtSimpleSip.Add(d1);
                        }
                        else
                        {
                            for (DateTime d2 = d1; d2 <= LastCalDate; d2 = d2.AddDays(1))
                            {
                                if (TriggardSIP.ToArray().Where(c => c.CalculateDate == d2).Any())
                                {
                                    Count++;
                                    _DtSimpleSip.Add(d2);
                                    break;
                                }
                            }
                        }
                    }

                    objEdel.Input.InitialInvestment = 1000;
                    var MonthlySIPDemo = objEdel.CalculateMonthlySimpleSIP(out strError);
                    if (MonthlySIPDemo.Any())
                        Count = MonthlySIPDemo.Where(c => c.IsMonthlyDeduct).Count();

                    objEdel.Input.InitialInvestment = Convert.ToInt32(TriggardSIP.Last().Liquid) / Count;
                    MonthlySIP = objEdel.CalculateMonthlySimpleSIP(out strError);
                    int period = TriggardSIP.Last().CalculateDate.Subtract(objInput.FromDate.Date).Days;



                    #region Triggard SIP
                    var SIP_Returns = TriggardSIP.Select(p => p.SIP_Return).ToArray();

                    var SIP_Investment = Math.Round(TriggardSIP.Last().SIP_Nav, 2);

                    decimal SIP_Profit = default(decimal);

                    //TotalInvestedForTriggared = TriggardSIP.Last().Liquid;
                    SIP_Profit = Math.Round(TriggardSIP.Last().SIP_Nav - SIPPlusInvestedAmt, 2);


                    double SIP_Return = default(double);

                    int RetPeriod = TriggardSIP.Last().CalculateDate.Subtract(TriggardSIP.Where(p => p.SIP_Nav != 0).First().CalculateDate).Days;
                    SIP_Return = CalculateReturn((float)(SIPPlusInvestedAmt), (float)(TriggardSIP.Last().SIP_Nav), RetPeriod);

                    #endregion


                    #region Volatility & Drawdown Calculation

                    var Edge_Returns = NavReturns(false);
                    var Sip_Return_volatility = TriggardSIP.Where(x => x.Sip_Return_New != 0).Select(x => x.Sip_Return_New).ToArray(); //Add hete 15.03.2018
                    var SIP_Volatility = objEdel.CalculateVolatility(Sip_Return_volatility);
                    var RebaseEdge = RebaseValue(Edge_Returns);
                    var SIP_Drawdown = objEdel.CalculateDrawdown(RebaseEdge);


                    #endregion

                    #region Triggared SIP Invested In Benchmark


                    //TriggardSIPBenchmark = objEdel.CalculateSIPPlusIndex(objInput, out SIPPlusInvestedAmtBenchmark, out strError);
                    TriggardSIPBenchmark = objEdel.CalculateSIPPlusIndex4TRI(objInput, out SIPPlusInvestedAmtBenchmark, out strError);

                    //edited 22 05 2019
                    if (TriggardSIPBenchmark != null)
                        if (TriggardSIPBenchmark.Where(x => x.SIP_Nav != 0).Count() == 0)
                        {
                            reportPanel.Visible = false;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Oops!!", "alert('No triggered occured for the specific selection.')", true);
                            return;
                        }

                    //***commented as it is not used
                    //  var SIP_Returns_Benchmark = TriggardSIPBenchmark.Select(p => p.SIP_Return).ToArray();

                    // var Index_Returns = IndexReturns();
                    // var SIP_Volatility_Benchmark = objEdel.CalculateVolatility(Index_Returns);

                    // var Rebase_Benchmark_Return = RebaseValue(Index_Returns);
                    //var RebaseSIP_Benchmark = RebaseValue(Rebase_Benchmark_Return);
                    
                    // var SIP_Drawdown_Benchmark = objEdel.CalculateDrawdown(Rebase_Benchmark_Return);
                   
                    //***
                    // var SIP_InvestmentBenchmark = Math.Round(TriggardSIPBenchmark.Last().SIP_Nav, 2);

                    // decimal SIP_Profit_Benchmark = default(decimal);

                    //TotalInvestedForTriggared = TriggardSIP.Last().Liquid;

                    //***
                    //SIP_Profit_Benchmark = Math.Round(TriggardSIPBenchmark.Last().SIP_Nav - SIPPlusInvestedAmtBenchmark, 2);

                    //*** commented no longer used

                    //double SIP_Return_Benchmark = default(double);                    
                    //int RetPeriodBenchmark = 0;
                    //if (TriggardSIPBenchmark.Where(p => p.SIP_Nav != 0).Any())
                    //{
                    //   RetPeriodBenchmark = TriggardSIPBenchmark.Last().CalculateDate.Subtract(TriggardSIPBenchmark.Where(p => p.SIP_Nav != 0).First().CalculateDate).Days;
                    //    SIP_Return_Benchmark = CalculateReturn((float)(SIPPlusInvestedAmtBenchmark), (float)(TriggardSIPBenchmark.Last().SIP_Nav), RetPeriod);
                    //}
                    #endregion

                    #region Triggared SIP Invested In Additional Benchmark



                    TriggardSIPAdditionalBenchmark = objEdel.CalculateSIPPlusAdditionalIndex(objInput, out SIPPlusInvestedAmtAddBenchmark, out strError);

                    var SIP_Returns_Additional_Benchmark = TriggardSIPAdditionalBenchmark == null ? null : TriggardSIPAdditionalBenchmark.Select(p => p.SIP_Return).ToArray();

                    var Additional_Index_Returns = AdditionalIndexReturns();
                    var SIP_Volatility_Additional_Benchmark = Additional_Index_Returns.Any() ? Convert.ToString(objEdel.CalculateVolatility(Additional_Index_Returns)) : "-";


                    //var RebaseSIP_Additional_Benchmark = SIP_Returns_Additional_Benchmark == null ? null : RebaseValue(SIP_Returns_Additional_Benchmark);
                    var Rebase_Additional_Benchmark = RebaseValue(AdditionalIndexReturns());
                    var SIP_Drawdown_Additional_Benchmark = objEdel.CalculateDrawdown(Rebase_Additional_Benchmark);
                    decimal? SIP_Investment_Additional_Benchmark = null;
                    if (TriggardSIPAdditionalBenchmark != null)
                        SIP_Investment_Additional_Benchmark = Math.Round(TriggardSIPAdditionalBenchmark.Last().SIP_Nav, 2);

                    decimal? SIP_Profit_Additional_Benchmark = null;

                    //TotalInvestedForTriggared = TriggardSIP.Last().Liquid;
                    if(TriggardSIPAdditionalBenchmark!=null)
                    SIP_Profit_Additional_Benchmark = Math.Round(TriggardSIPAdditionalBenchmark.Last().SIP_Nav - SIPPlusInvestedAmtAddBenchmark, 2);


                    //*** not required
                    //double? SIP_Return_additonal_Benchmark = default(double);

                    //int RetPeriodAdditionalBenchmark = TriggardSIPAdditionalBenchmark.Last().CalculateDate.Subtract(TriggardSIPAdditionalBenchmark.Where(p => p.SIP_Nav != 0).First().CalculateDate).Days;
                    //SIP_Return_additonal_Benchmark = CalculateReturn((float)(SIPPlusInvestedAmtAddBenchmark), (float)(TriggardSIPAdditionalBenchmark.Last().SIP_Nav), RetPeriod);


                    #endregion
                    var MSIP_Returns = MonthlySIP.Select(p => p.SIP_Return).ToArray();
                    var MSIP_Returns_volatility = MonthlySIP.Where(x => x.Sip_Return_New != 0).Select(p => p.Sip_Return_New).ToArray(); //Add hete 15.03.2018
                    var MSIP_Volatility = objEdel.CalculateVolatility(MSIP_Returns_volatility);
                    var RebaseMSIP = RebaseValue(MSIP_Returns);
                    var MSIP_Drawdown = objEdel.CalculateDrawdown(RebaseMSIP);
                    //var MSIP_Investment = Math.Round(((MonthlySIP.Last().SIP_Nav - AllMethods.Input.InitialInvestment) / AllMethods.Input.InitialInvestment) * 100, 4);
                    var MSIP_Investment = Math.Round(MonthlySIP.Last().SIP_Nav, 2);

                    decimal MSIP_Profit = default(decimal);

                    MSIP_Profit = Math.Round(MonthlySIP.Last().SIP_Nav - TriggardSIP.Last().Liquid, 2);

                    double MSIP_Return = default(double);
                    RetPeriod = TriggardSIP.Last().CalculateDate.Subtract(TriggardSIP.Where(p => p.SIP_Nav != 0).First().CalculateDate).Days;
                    MSIP_Return = CalculateReturn((float)(SIPPlusInvestedAmt), (float)(MonthlySIP.Last().SIP_Nav), RetPeriod);



                    //MSIP_Return = CalculateReturn((float)(MonthlySIP.First().SIP_Nav), (float)(MonthlySIP.Last().SIP_Nav), _DtSimpleSip.Count);

                    //add for Xirr
                    double XIIRPrepaid = 0.0;
                    double XIIMonth = 0.0;
                    double XIIRBenchMark = 0.0;
                    double? XIIRAddBenchMark = null;
                    _DtSimpleSip.Insert(Count, objInput.CurrentDate);
                    for (int i = 1; i <= Count; i++)
                    {
                        _value.Add(-objEdel.Input.InitialInvestment);
                    }
                    _value.Insert(Count, Convert.ToDouble(MSIP_Investment));
                    //   _DtSimpleSip.Reverse();
                    //   _value.Reverse();
                    //---
                    //   XIIMonth = Math.Round(Utilities.XIRR(_value.ToArray(), _DtSimpleSip.ToArray()) * 100, 4);
                    //    _value[0] = Convert.ToDouble(SIP_Investment);
                    //    XIIRPrepaid = Math.Round(Utilities.XIRR(_value.ToArray(), _DtSimpleSip.ToArray()) * 100, 4);
                    //    _value[0] = Convert.ToDouble(SIP_InvestmentBenchmark);
                    //    XIIRBenchMark = Math.Round(Utilities.XIRR(_value.ToArray(), _DtSimpleSip.ToArray()) * 100, 4);
                    //    _value[0] = Convert.ToDouble(SIP_Investment_Additional_Benchmark);
                    //    XIIRAddBenchMark = Math.Round(Utilities.XIRR(_value.ToArray(), _DtSimpleSip.ToArray()) * 100, 4);
                    //end
                    //Claculate XIRR Using Sp  

                    string Date = string.Empty;
                    string MonthlySipAmt = string.Empty;

                    var MonthlySIPDates = MonthlySIP.Where(x => x.IsMonthlyDeduct).Select(c => c.CalculateDate).ToList();
                    MonthlySIPDates.Add(objInput.CurrentDate);

                    foreach (var item in MonthlySIPDates)
                        Date = Date + item.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + ",";
                    Date = Date.Remove(Date.Length - 1);
                    //monthlysip
                    foreach (var item in _value)
                        MonthlySipAmt = MonthlySipAmt + item + ",";

                    MonthlySipAmt = MonthlySipAmt.Remove(MonthlySipAmt.Length - 1);
                  
                    XIIMonth = AllMethods.getXIRR(Date, MonthlySipAmt);

                    //prepaid
                    //   string PrepaidSipAmt = string.Empty;
                    //** Commented as it is wrong
                    //_value[_value.Count - 1] = Convert.ToDouble(SIP_Investment);
                    //foreach (var item in _value)
                    //    PrepaidSipAmt = PrepaidSipAmt + item + ",";
                    //PrepaidSipAmt = PrepaidSipAmt.Remove(PrepaidSipAmt.Length - 1);

                    //**
                    Date = string.Empty;
                    Date = string.Join(",", TriggardSIP.Where(x => x.IsTriggard).Select(x => x.CalculateDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                    Date = Date + "," + objInput.CurrentDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                   var _LstTriggared = TriggardSIP.Where(x => x.IsTriggard);

                    string PrepaidSipAmt = string.Join(",-", _LstTriggared.Select(x => Convert.ToString(x.Swithch_Amt)));
                    PrepaidSipAmt ="-"+ PrepaidSipAmt + "," + Convert.ToString(SIP_Investment);
                    XIIRPrepaid = AllMethods.getXIRR(Date, PrepaidSipAmt);

                    //bench mark
                    if (TriggardSIPBenchmark != null)
                    {
                        string benchSipAmt = string.Join(",-", _LstTriggared.Select(x => Convert.ToString(x.Swithch_Amt)));
                        benchSipAmt = "-" + benchSipAmt + "," + Convert.ToString(Math.Round(TriggardSIPBenchmark.Last().SIP_Nav, 2));

                        //PrepaidSipAmt[_value.Count - 1] = Convert.ToDouble(Math.Round(TriggardSIPBenchmark.Last().SIP_Nav, 2));
                        //foreach (var item in PrepaidSipAmt)
                        //    benchSipAmt = benchSipAmt + item + ",";

                        benchSipAmt = benchSipAmt.Remove(benchSipAmt.Length - 1);
                        XIIRBenchMark = AllMethods.getXIRR(Date, benchSipAmt);
                    }
                    //Add bench
                    //string AddbenchSipAmt = string.Empty;
                    //_value[_value.Count - 1] = Convert.ToDouble(SIP_Investment_Additional_Benchmark);
                    //foreach (var item in _value)
                    //    AddbenchSipAmt = AddbenchSipAmt + item + ",";
                    //AddbenchSipAmt = AddbenchSipAmt.Remove(AddbenchSipAmt.Length - 1);

                    if (SIP_Investment_Additional_Benchmark.HasValue)
                    {
                        string AddbenchSipAmt = string.Join(",-", _LstTriggared.Select(x => Convert.ToString(x.Swithch_Amt)));
                        AddbenchSipAmt = "-" + AddbenchSipAmt + "," + Convert.ToString(SIP_Investment_Additional_Benchmark);
                        XIIRAddBenchMark = AllMethods.getXIRR(Date, AddbenchSipAmt);
                    }

                    //End
                    //Amount Invested
                    var SipInvestment = Math.Round(SIPPlusInvestedAmt, 2);
                    var MSipInvest = Math.Round(TriggardSIP.Last().Liquid, 2);
                    //***
                    //  var SipInvestMentBenchMark = Math.Round(SIPPlusInvestedAmtBenchmark, 2);
                    //**
                    // var SipInvestmentAddBenchMark = Math.Round(SIPPlusInvestedAmtAddBenchmark, 2);
                    //End
                    dtSummary.Rows.Add(new object[] { "Amount Invested (Rs.)", "", "", SipInvestment.ToString("N", new CultureInfo("en-US")), MSipInvest.ToString("N", new CultureInfo("en-US")), TriggardSIPBenchmark != null ? Math.Round(SIPPlusInvestedAmtBenchmark, 2).ToString("N", new CultureInfo("en-US")) : "-", SIP_Profit_Additional_Benchmark.HasValue? Math.Round(SIPPlusInvestedAmtAddBenchmark, 2).ToString("N", new CultureInfo("en-US")):"-"});
                    dtSummary.Rows.Add(new object[] { "Investment Value as on Date (Rs.)", "", "", SIP_Investment.ToString("N", new CultureInfo("en-US")), MSIP_Investment.ToString("N", new CultureInfo("en-US")),
                        TriggardSIPBenchmark!=null?
                        Math.Round(TriggardSIPBenchmark.Last().SIP_Nav, 2).ToString("N", new CultureInfo("en-US")):"-",SIP_Investment_Additional_Benchmark.HasValue? SIP_Investment_Additional_Benchmark.Value.ToString("N", new CultureInfo("en-US")):"-" });
                    dtSummary.Rows.Add(new object[] { "Total Profit (Rs.)", "", "", SIP_Profit.ToString("N", new CultureInfo("en-US")), MSIP_Profit.ToString("N", new CultureInfo("en-US")), TriggardSIPBenchmark != null ? Math.Round(TriggardSIPBenchmark.Last().SIP_Nav - SIPPlusInvestedAmtBenchmark, 2).ToString("N", new CultureInfo("en-US")) : "-", SIP_Profit_Additional_Benchmark.HasValue ? SIP_Profit_Additional_Benchmark.Value.ToString("N", new CultureInfo("en-US")) : "-" });
                    //dtSummary.Rows.Add(new object[] { "Return (%) *", "", "", SIP_Return, MSIP_Return, SIP_Return_Benchmark, SIP_Return_additonal_Benchmark });
                    dtSummary.Rows.Add(new object[] { "Return (%) *", "", "", XIIRPrepaid, XIIMonth, TriggardSIPBenchmark != null ? Convert.ToString(XIIRBenchMark) : "-", XIIRAddBenchMark.HasValue ? Convert.ToString(XIIRAddBenchMark.Value) : "-" });
                    //  dtSummary.Rows.Add(new object[] { "Volatility (%)", "", "", SIP_Volatility, MSIP_Volatility, SIP_Volatility_Benchmark, SIP_Volatility_Additional_Benchmark });
                    // dtSummary.Rows.Add(new object[] { "Drawdown (%)", "", "", SIP_Drawdown, MSIP_Drawdown, SIP_Drawdown_Benchmark, SIP_Drawdown_Additional_Benchmark });
                }
                if (dtSummary.Rows.Count > 0)
                {
                    reportPanel.Visible = true;
                }
                else
                {
                    reportPanel.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Oops!!", "alert('No results found.')", true);
                    return;
                }

                gridSummary.DataSource = dtSummary;
                gridSummary.Columns[1].HeaderText = objEdel.MakeSchemeShortForm(ddlFromScheme.SelectedItem.Text) + " ^";

                for (int i = 0; i < gridSummary.Columns.Count; i++)
                {
                    gridSummary.Columns[i].Visible = true;
                }

                // Since ARF is missing in SIP Plus Option
                //if (CurrentProcess == EdelweissProcess.PrepaidSIP)
                // {
                gridSummary.Columns[1].Visible = false; gridSummary.Columns[2].Visible = false; gridSummary.Columns[4].Visible = true; tblFromScheme.Visible = false;
                // }               
                // else
                // {
                // tblFromScheme.Visible = true;
                // }

                gridSummary.Columns[2].HeaderText = objEdel.MakeSchemeShortForm(ddlToScheme.SelectedItem.Text) + " #";

                if (CurrentProcess == EdelweissProcess.PrepaidSIP)
                {
                    //fromschemefull.InnerText = "^ " + objEdel.MakeSchemeShortForm(ddlFromScheme.SelectedItem.Text) + "= " + ddlFromScheme.SelectedItem.Text.Replace(ddlFromScheme.SelectedItem.Text.Split(' ')[ddlFromScheme.SelectedItem.Text.Split(' ').Length - 1], "");
                    fromschemefull.Visible = false;
                    //toschemefull.InnerText = "# " + objEdel.MakeSchemeShortForm(ddlToScheme.SelectedItem.Text) + "= " + ddlToScheme.SelectedItem.Text.Replace(ddlToScheme.SelectedItem.Text.Split(' ')[ddlToScheme.SelectedItem.Text.Split(' ').Length - 1], "");
                    toschemefull.Visible = false;
                    benchmarkfull.InnerText = "@ Benchmark= " + HdnIndexName.Value; //txtIndex.Text;//.SelectedItem.Text;
                    addbenchmarkfull.InnerText = "$ Additional Benchmark= " + HdnAddBenchName.Value; //ddlAdditionalBenchmark.SelectedItem.Text; //change here
                    benchmarkfull.Visible = true;
                    MonthlySIPid.Visible = true;
                    MonthlySIPid.InnerText = "> SIP is a systematic investment to the target scheme on the first of every month";
                    addbenchmarkfull.Visible = true;
                    div_text.InnerText = "* Return calculated using XIRR calculation";
                }
                else
                {
                    tblFromScheme.Visible = true;
                    fromschemefull.InnerText = "^ " + objEdel.MakeSchemeShortForm(ddlFromScheme.SelectedItem.Text) + "= " + ddlFromScheme.SelectedItem.Text.Replace(ddlFromScheme.SelectedItem.Text.Split(' ')[ddlFromScheme.SelectedItem.Text.Split(' ').Length - 1], "");
                    fromschemefull.Visible = false;
                    toschemefull.InnerText = "# " + objEdel.MakeSchemeShortForm(ddlToScheme.SelectedItem.Text) + "= " + ddlToScheme.SelectedItem.Text.Replace(ddlToScheme.SelectedItem.Text.Split(' ')[ddlToScheme.SelectedItem.Text.Split(' ').Length - 1], "");
                    toschemefull.Visible = false;
                    benchmarkfull.InnerText = "@ Benchmark= " + HdnIndexName.Value;//txtIndex.Text; // ddlBenchmark.SelectedItem.Text;
                    benchmarkfull.Visible = true;
                    addbenchmarkfull.InnerText = "$ Additional Benchmark= " + HdnAddBenchName.Value; //ddlAdditionalBenchmark.SelectedItem.Text; //change here 
                    addbenchmarkfull.Visible = true;
                    MonthlySIPid.Visible = true;
                    MonthlySIPid.InnerText = "> STP is a systematic investment from the source scheme to the target scheme on the first of every month";
                    div_text.InnerText = "* Returns are ABSOLUTE for <1 yr and COMPOUND ANNUALIZED for >=1 yr";
                }

                if (CurrentProcess == EdelweissProcess.GainSwitchSIP)
                {
                    gridSummary.Columns[3].HeaderText = "GSM STP";
                    charid.InnerText = "GSM STP Performance Chart";
                    gridSummary.Columns[4].HeaderText = "Monthly STP >";
                }
                if (CurrentProcess == EdelweissProcess.PrepaidSTP)
                {
                    gridSummary.Columns[3].HeaderText = "Power STP";
                    charid.InnerText = "Power STP Performance Chart";
                    gridSummary.Columns[4].HeaderText = "Monthly STP >";
                }
                if (CurrentProcess == EdelweissProcess.PrepaidSIP)
                {
                    gridSummary.Columns[3].HeaderText = "Power SIP";
                    charid.InnerText = "Power SIP Performance Chart";
                    gridSummary.Columns[4].HeaderText = "Monthly SIP >";

                }

                gridSummary.DataBind();

                #endregion

                //var 

                var triggardIndex = objEdel.GetTriggeredIndexData();
                var SchemeLastDayData = TriggardSIP.Last();
                var SchemeInitialData = TriggardSIP.First();
                var IndexVal = triggardIndex.Where(p => p.Record_Date.Date == SchemeInitialData.CalculateDate.Date).FirstOrDefault();

                #region From Scheme Details

                if (CurrentProcess == EdelweissProcess.GainSwitchSIP || CurrentProcess == EdelweissProcess.PrepaidSTP)
                {
                    // Final Triggared SIP Value Details
                    #region Code Obsolate
                    //var currentDateSIP = TriggardSIP.Last();
                    //var triggaredIndexData = AllMethods.GetTriggeredIndexData();
                    //var currentDateIndexData = triggaredIndexData.Where(p => p.Record_Date.Date == currentDateSIP.CalculateDate.Date).FirstOrDefault();


                    //var uniqueDate = (from tbl in TriggardSIP
                    //                  where tbl.IsTriggard == true
                    //                  select new { Date = tbl.CalculateDate }.Date).ToArray();

                    //var TriggarType = (objInput.IsGSM) ? EdelweissTriggerType.PositiveTrigger : EdelweissTriggerType.NegativeTrigger;

                    //var uniqueDate1 = (from tbl in triggaredIndexData
                    //                   join date in AllMethods.DateList on tbl.Record_Date.Date equals date.Date
                    //                   where tbl.IsTriggerred == TriggarType
                    //                   && tbl.Record_Date <= objInput.ToDate
                    //                   select new { Date = tbl.Record_Date }.Date).Take(objInput.MaxSwitch).ToArray();
                    //var finalDateList = uniqueDate.Union(uniqueDate1).ToArray();

                    //var triggaredData = (from tbl in TriggardSIP
                    //                     join dt in finalDateList on tbl.CalculateDate equals dt
                    //                     select tbl).OrderBy(p => p.CalculateDate).ToArray();


                    //var DataFromScheme = (from tbl in triggaredData
                    //                      join indxData in AllMethods.IndexData on tbl.CalculateDate equals indxData.Record_Date
                    //                      select new
                    //                      {
                    //                          Date = tbl.CalculateDate.ToString("dd-MMM-yyyy"),
                    //                          Nav = tbl.SourceNav,
                    //                          Index_Change = Math.Round(indxData.Index_Value, 2),
                    //                          SIP_Amount = -SIPAmount,
                    //                          Scheme_Units = Math.Round(tbl.Liquid_Unit, 4),
                    //                          //SIP_Nav = tbl.SourceNav,
                    //                          Cumulative_Fund_Value = Math.Round(tbl.Liquid, 2)
                    //                      }).ToList();


                    //DataFromScheme.Insert(0, new
                    //{
                    //    Date = objInput.FromDate.ToString("dd-MMM-yyyy"),
                    //    Nav = TriggardSIP.First().SourceNav,
                    //    Index_Change = 0M,
                    //    SIP_Amount = (decimal)objInput.InitialInvestment,
                    //    Scheme_Units = Math.Round(TriggardSIP.First().Liquid_Unit, 4),
                    //    //SIP_Nav = Convert.ToDecimal(objInput.InitialInvestment),
                    //    Cumulative_Fund_Value = Math.Round(TriggardSIP.First().Liquid, 2)
                    //});

                    //DataFromScheme.Add(new
                    //{
                    //    Date = currentDateSIP.CalculateDate.ToString("dd-MMM-yyyy"),
                    //    Nav = currentDateSIP.SourceNav,
                    //    Index_Change = currentDateIndexData == null ? 0M : Math.Round(currentDateIndexData.Index_Value, 2),
                    //    SIP_Amount = 0M,
                    //    Scheme_Units = Math.Round(currentDateSIP.Liquid_Unit, 4),
                    //    Cumulative_Fund_Value = Math.Round(currentDateSIP.Liquid, 2)
                    //});

                    #endregion


                    var DataFromScheme = (from tsip in TriggardSIP
                                          join tIndex in triggardIndex on tsip.CalculateDate.Date equals tIndex.Record_Date.Date
                                          where tsip.IsTriggard == true
                                          select new
                                          {
                                              Date = tsip.CalculateDate.ToString("dd-MMM-yyyy"),
                                              Nav = tsip.SourceNav,
                                              Index_Change = tIndex == null ? 0M : Math.Round(tIndex.Index_Value, 2),
                                              SIP_Amount = "-" + tsip.Swithch_Amt.ToString("N", new CultureInfo("en-US")),
                                              Scheme_Units = Math.Round(tsip.Liquid_Unit, 4),
                                              Cumulative_Fund_Value = Math.Round(tsip.Liquid, 2).ToString("N", new CultureInfo("en-US")),
                                              Headertext = IsGSM ? "Capital Appreciation (%)" : "Index Change (%)"
                                          }).ToList();


                    DataFromScheme.Insert(0, new
                    {
                        Date = SchemeInitialData.CalculateDate.ToString("dd-MMM-yyyy"),
                        Nav = SchemeInitialData.SourceNav,
                        Index_Change = IndexVal == null ? 0M : Math.Round(IndexVal.Index_Value, 2),
                        SIP_Amount = "0",
                        Scheme_Units = Math.Round(SchemeInitialData.Liquid_Unit, 4),
                        Cumulative_Fund_Value = Math.Round(SchemeInitialData.Liquid, 2).ToString("N", new CultureInfo("en-US")),
                        Headertext = IsGSM ? "Capital Appreciation (%)" : "Index Change (%)"
                    });


                    IndexVal = triggardIndex.Where(p => p.Record_Date.Date == SchemeLastDayData.CalculateDate.Date).FirstOrDefault();

                    DataFromScheme.Add(new
                    {
                        Date = SchemeLastDayData.CalculateDate.ToString("dd-MMM-yyyy"),
                        Nav = SchemeLastDayData.SourceNav,
                        Index_Change = IndexVal == null ? 0M : Math.Round(IndexVal.Index_Value, 2),
                        SIP_Amount = "0",
                        Scheme_Units = Math.Round(SchemeLastDayData.Liquid_Unit, 4),
                        Cumulative_Fund_Value = Math.Round(SchemeLastDayData.Liquid, 2).ToString("N", new CultureInfo("en-US")),
                        Headertext = IsGSM ? "Capital Appreciation (%)" : "Index Change (%)"
                    });

                    var dtResultFromScheme = DataFromScheme.ToDataTable();


                    gridDetailsFrom.Columns[2].HeaderText = Headertext;
                    gridDetailsFrom.DataSource = DataFromScheme.ToDataTable();
                    gridDetailsFrom.DataBind();

                    //gridDetailsFrom.Columns[0].HeaderText = IsGSM ? "Capital Appreciation (%)" : "Index Change (%)";



                    lblFromScheme.InnerText = ddlFromScheme.SelectedItem.Text;
                    Session["FromScheme"] = ddlFromScheme.SelectedItem.Text;
                }
                #endregion

                #region To Scheme Details

                var DataToScheme = (from tbl in TriggardSIP
                                    join indxData in triggardIndex on tbl.CalculateDate.Date equals indxData.Record_Date.Date
                                    where tbl.IsTriggard == true
                                    select new
                                    {
                                        Date = tbl.CalculateDate.ToString("dd-MMM-yyyy"),
                                        Nav = tbl.DestinationNav,
                                        Index_Change = Math.Round(indxData.Index_Value, 2),
                                        SIP_Amount = tbl.Swithch_Amt.ToString("N", new CultureInfo("en-US")),
                                        Scheme_Units = Math.Round(tbl.Edge_Unit, 4),
                                        //SIP_Nav = tbl.DestinationNav,
                                        Cumulative_Fund_Value = Math.Round(tbl.Edge, 2).ToString("N", new CultureInfo("en-US")),
                                        Headertext = IsGSM ? "Capital Appreciation (%)" : "Index Change (%)"
                                    }).ToList();

                DataToScheme.Add(new
                {
                    Date = SchemeLastDayData.CalculateDate.ToString("dd-MMM-yyyy"),
                    Nav = SchemeLastDayData.DestinationNav,
                    Index_Change = IndexVal == null ? 0M : Math.Round(IndexVal.Index_Value, 2),
                    SIP_Amount = "0",
                    Scheme_Units = Math.Round(SchemeLastDayData.Edge_Unit, 4),
                    Cumulative_Fund_Value = Math.Round(SchemeLastDayData.Edge, 2).ToString("N", new CultureInfo("en-US")),
                    Headertext = IsGSM ? "Capital Appreciation (%)" : "Index Change (%)"
                });

                gridDetailsTo.Columns[2].HeaderText = Headertext;
                gridDetailsTo.DataSource = DataToScheme.ToDataTable();
                gridDetailsTo.DataBind();
                lblToScheme.InnerText = ddlToScheme.SelectedItem.Text;
                Session["ToScheme"] = ddlToScheme.SelectedItem.Text;


                #endregion

                #region Rebase for graph

                EdelweissGraph[] DataToBeRebased = null;
                IList<EdelweissGraph> GraphData = new List<EdelweissGraph>();

                if (CurrentProcess == EdelweissProcess.PrepaidSIP)
                {
                    if (TriggardSIPBenchmark != null)
                    {
                        DataToBeRebased = (from indexdt in TriggardSIPBenchmark.Where(p => p.SIP_Nav != 0)
                                           join trgSIP in TriggardSIP.Where(p => p.SIP_Nav != 0) on indexdt.CalculateDate equals trgSIP.CalculateDate
                                           join mnthSIP in TriggardSIPAdditionalBenchmark.Where(p => p.SIP_Nav != 0) on indexdt.CalculateDate equals mnthSIP.CalculateDate
                                           where indexdt.CalculateDate <= objInput.ToDate
                                           select new EdelweissGraph
                                           {
                                               Date = indexdt.CalculateDate,
                                               Nifty = Convert.ToDecimal(indexdt.SIP_Nav),
                                               TriggerSIP = trgSIP.SIP_Nav,
                                               MonthlySIP = mnthSIP.SIP_Nav
                                           }).ToArray();                
                       

                        for (int i = 0; i < DataToBeRebased.Length; i++)
                        {
                            if (GraphData.Count == 0)
                            {
                                GraphData.Add(new EdelweissGraph() { Date = DataToBeRebased[i].Date, Nifty = 100, TriggerSIP = 100, MonthlySIP = 100 });
                                continue;
                            }

                            // Rebase logic is different for SIP Plus Than Triggered and Gain Switch Mechanism

                            var objEdelweiss = new EdelweissGraph()
                            {
                                Date = DataToBeRebased[i].Date,
                                Nifty = Math.Round((100 / SIPPlusInvestedAmtBenchmark) * DataToBeRebased[i].Nifty, 8),
                                TriggerSIP = Math.Round((100 / SIPPlusInvestedAmt) * DataToBeRebased[i].TriggerSIP, 8),
                                MonthlySIP = Math.Round((100 / SIPPlusInvestedAmtAddBenchmark) * DataToBeRebased[i].MonthlySIP, 8)
                            };
                            GraphData.Add(objEdelweiss);
                        }
                        PlotSIPGraph(GraphData);
                    }
                    else if((TriggardSIPBenchmark == null)&& (TriggardSIPAdditionalBenchmark != null))
                    {
                            DataToBeRebased = (from trgSIP in TriggardSIP.Where(p => p.SIP_Nav != 0)
                                               join mnthSIP in TriggardSIPAdditionalBenchmark.Where(p => p.SIP_Nav != 0) on trgSIP.CalculateDate equals mnthSIP.CalculateDate
                                               where trgSIP.CalculateDate <= objInput.ToDate
                                               select new EdelweissGraph
                                               {
                                                   Date = trgSIP.CalculateDate,
                                                   //Nifty = Convert.ToDecimal(indexdt.SIP_Nav),
                                                   TriggerSIP = trgSIP.SIP_Nav,
                                                   MonthlySIP = mnthSIP.SIP_Nav
                                               }).ToArray();
                        for (int i = 0; i < DataToBeRebased.Length; i++)
                        {
                            if (GraphData.Count == 0)
                            {
                                GraphData.Add(new EdelweissGraph() { Date = DataToBeRebased[i].Date, TriggerSIP = 100, MonthlySIP = 100 });
                                continue;
                            }
                            // Rebase logic is different for SIP Plus Than Triggered and Gain Switch Mechanism
                            var objEdelweiss = new EdelweissGraph()
                            {
                                Date = DataToBeRebased[i].Date,
                                //Nifty = Math.Round((100 / SIPPlusInvestedAmtBenchmark) * DataToBeRebased[i].Nifty, 8),
                                TriggerSIP = Math.Round((100 / SIPPlusInvestedAmt) * DataToBeRebased[i].TriggerSIP, 8),
                                MonthlySIP = Math.Round((100 / SIPPlusInvestedAmtAddBenchmark) * DataToBeRebased[i].MonthlySIP, 8)
                            };
                            GraphData.Add(objEdelweiss);
                        }

                        PlotSIPGraph(GraphData, false);
                    }
                    else if ((TriggardSIPBenchmark == null) && (TriggardSIPAdditionalBenchmark == null))
                    {
                        DataToBeRebased = (from trgSIP in TriggardSIP.Where(p => p.SIP_Nav != 0)
                                          // join mnthSIP in TriggardSIPAdditionalBenchmark.Where(p => p.SIP_Nav != 0) on trgSIP.CalculateDate equals mnthSIP.CalculateDate
                                           where trgSIP.CalculateDate <= objInput.ToDate
                                           select new EdelweissGraph
                                           {
                                               Date = trgSIP.CalculateDate,
                                               //Nifty = Convert.ToDecimal(indexdt.SIP_Nav),
                                               TriggerSIP = trgSIP.SIP_Nav,
                                               //MonthlySIP = mnthSIP.SIP_Nav
                                           }).ToArray();
                        for (int i = 0; i < DataToBeRebased.Length; i++)
                        {
                            if (GraphData.Count == 0)
                            {
                                GraphData.Add(new EdelweissGraph() { Date = DataToBeRebased[i].Date, TriggerSIP = 100 });
                                continue;
                            }
                            // Rebase logic is different for SIP Plus Than Triggered and Gain Switch Mechanism
                            var objEdelweiss = new EdelweissGraph()
                            {
                                Date = DataToBeRebased[i].Date,
                                //Nifty = Math.Round((100 / SIPPlusInvestedAmtBenchmark) * DataToBeRebased[i].Nifty, 8),
                                TriggerSIP = Math.Round((100 / SIPPlusInvestedAmt) * DataToBeRebased[i].TriggerSIP, 8),
                                // MonthlySIP = Math.Round((100 / SIPPlusInvestedAmtAddBenchmark) * DataToBeRebased[i].MonthlySIP, 8)
                            };
                            GraphData.Add(objEdelweiss);
                        }

                        PlotSIPGraph(GraphData, false);
                    }
                }
                else
                {
                    if (BenchmarkSIP.Any())
                    {
                        DataToBeRebased = (from indexdt in BenchmarkSIP
                                           join trgSIP in TriggardSIP on indexdt.SIP_Date equals trgSIP.CalculateDate
                                           join mnthSIP in MonthlySIP on indexdt.SIP_Date equals mnthSIP.CalculateDate
                                           where indexdt.SIP_Date <= objInput.ToDate
                                           select new EdelweissGraph
                                           {
                                               Date = indexdt.SIP_Date,
                                               Nifty = Convert.ToDecimal(indexdt.SIP_Nav),
                                               TriggerSIP = trgSIP.SIP_Nav,
                                               MonthlySIP = mnthSIP.SIP_Nav
                                           }).ToArray();


                        for (int i = 0; i < DataToBeRebased.Length; i++)
                        {
                            if (GraphData.Count == 0)
                            {
                                GraphData.Add(new EdelweissGraph() { Date = DataToBeRebased[i].Date, Nifty = 0, TriggerSIP = 0, MonthlySIP = 0 });
                                continue;
                            }

                            // Rebase logic is different for SIP Plus Than Triggered and Gain Switch Mechanism
                            var objEdelweiss = new EdelweissGraph()
                            {
                                Date = DataToBeRebased[i].Date,
                                Nifty = (Math.Round((100 / DataToBeRebased[0].Nifty) * DataToBeRebased[i].Nifty, 8) - 100),
                                TriggerSIP = (Math.Round((100 / DataToBeRebased[0].TriggerSIP) * DataToBeRebased[i].TriggerSIP, 8) - 100),
                                MonthlySIP = (Math.Round((100 / DataToBeRebased[0].MonthlySIP) * DataToBeRebased[i].MonthlySIP, 8) - 100)
                            };
                            GraphData.Add(objEdelweiss);
                        }

                        PlotSIPGraph(GraphData);
                    }
                    else
                    {
                        DataToBeRebased = (from  trgSIP in TriggardSIP
                                           join mnthSIP in MonthlySIP on trgSIP.CalculateDate equals mnthSIP.CalculateDate
                                           where trgSIP.CalculateDate <= objInput.ToDate
                                           select new EdelweissGraph
                                           {
                                               Date = trgSIP.CalculateDate,
                                               //Nifty = Convert.ToDecimal(indexdt.SIP_Nav),
                                               TriggerSIP = trgSIP.SIP_Nav,
                                               MonthlySIP = mnthSIP.SIP_Nav
                                           }).ToArray();


                        for (int i = 0; i < DataToBeRebased.Length; i++)
                        {
                            if (GraphData.Count == 0)
                            {
                                GraphData.Add(new EdelweissGraph() { Date = DataToBeRebased[i].Date, Nifty = 0, TriggerSIP = 0, MonthlySIP = 0 });
                                continue;
                            }

                            // Rebase logic is different for SIP Plus Than Triggered and Gain Switch Mechanism
                            var objEdelweiss = new EdelweissGraph()
                            {
                                Date = DataToBeRebased[i].Date,
                                //Nifty = (Math.Round((100 / DataToBeRebased[0].Nifty) * DataToBeRebased[i].Nifty, 8) - 100),
                                TriggerSIP = (Math.Round((100 / DataToBeRebased[0].TriggerSIP) * DataToBeRebased[i].TriggerSIP, 8) - 100),
                                MonthlySIP = (Math.Round((100 / DataToBeRebased[0].MonthlySIP) * DataToBeRebased[i].MonthlySIP, 8) - 100)
                            };
                            GraphData.Add(objEdelweiss);
                        }

                        PlotSIPGraph(GraphData,false);
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {
                Show(ex.Message.ToString());
                return;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            CreateHTMLEdelweissSIP();
        }

        private void PlotSIPGraph(IList<EdelweissGraph> GraphData, bool IsBenchmark=true)
        {
            try
            {
                #region Set Graph Data
                chartSIP.Series.Clear();
                var dtGraph = GraphData.ToDataTable();

                if (!IsBenchmark)
                    dtGraph.Columns.Remove("Nifty");


                var dtOrdered = GraphData.OrderByDescending(p => p.Date).ToArray();

                IList<decimal> maxList = new List<decimal>();
                maxList.Add(dtOrdered.Select(p => p.Nifty).Max());
                maxList.Add(dtOrdered.Select(p => p.TriggerSIP).Max());
                maxList.Add(dtOrdered.Select(p => p.MonthlySIP).Max());

                IList<decimal> minList = new List<decimal>();
                minList.Add(dtOrdered.Select(p => p.Nifty).Min());
                minList.Add(dtOrdered.Select(p => p.TriggerSIP).Min());
                minList.Add(dtOrdered.Select(p => p.MonthlySIP).Min());

                foreach (DataColumn datacol in dtGraph.Columns)
                {
                    if (datacol.ColumnName == "Date")
                        continue;

                    Series series = new Series();

                    foreach (DataRow dr in dtGraph.Rows)
                    {

                        decimal y = (decimal)dr[datacol.ColumnName];
                        series.XValueType = ChartValueType.DateTime;
                        series.Points.AddXY(Convert.ToDateTime(dr["Date"].ToString()).Date.ToOADate(), y);

                        if (CurrentProcess == EdelweissProcess.PrepaidSIP)
                        {
                            if (datacol.ColumnName == "TriggerSIP")
                                series.LegendText = "Power SIP";
                            if (datacol.ColumnName == "Nifty")
                                series.LegendText = "Trigger Benchmark";
                            if (datacol.ColumnName == "MonthlySIP")
                                series.LegendText = "Trigger Additional Benchmark";
                        }
                        else { series.LegendText = IsGSM ? (datacol.ColumnName == "TriggerSIP" ? "GSM STP" : datacol.ColumnName.Replace("MonthlySIP", "Monthly STP")) : ((datacol.ColumnName == "TriggerSIP" ? "Power STP" : datacol.ColumnName.Replace("MonthlySIP", "Monthly STP"))); }

                    }



                    series.XValueType = ChartValueType.Date;
                    series.YValueType = ChartValueType.Double;
                    series.ChartType = SeriesChartType.Line;
                    series.BorderWidth = 3;



                    if (datacol.ColumnName == "Nifty")
                        series.Color = System.Drawing.Color.FromArgb(83, 142, 213);
                    if (datacol.ColumnName == "TriggerSIP")
                        series.Color = System.Drawing.Color.FromArgb(204, 51, 0);
                    if (datacol.ColumnName == "MonthlySIP")
                        series.Color = System.Drawing.Color.FromArgb(146, 208, 80);

                    series.BorderWidth = 2;

                    chartSIP.Series.Add(series);
                }

                var dtMax = dtOrdered.First().Date;
                var dtMin = dtOrdered.Last().Date;
                //chartSIP.ChartAreas[0].AxisX.Maximum = dtOrdered.First().Date.ToOADate();
                //chartSIP.ChartAreas[0].AxisX.Minimum = dtOrdered.Last().Date.ToOADate();
                //chartSIP.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                //chartSIP.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;


                if (dtMax.Subtract(dtMin).Days > 365 && dtMax.Subtract(dtMin).Days < 1000)
                {
                    //chartSIP.ChartAreas[0].AxisX.Interval = 6;
                    chartSIP.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chartSIP.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM-yy";
                }
                else if (dtMax.Subtract(dtMin).Days > 365)
                {
                    //chartSIP.ChartAreas[0].AxisX.Interval = 5;
                    chartSIP.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
                    chartSIP.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM-yyyy";
                }
                else
                {
                    //chartSIP.ChartAreas[0].AxisX.Interval = 10;
                    chartSIP.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    chartSIP.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM-yy";
                }

                chartSIP.ChartAreas[0].AxisX.LineWidth = 1;
                chartSIP.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Gray;
                chartSIP.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Gray;

                chartSIP.ChartAreas[0].AxisY.Maximum = Math.Round(Convert.ToDouble(maxList.Max()), 0);
                chartSIP.ChartAreas[0].AxisY.Minimum = Math.Round(Convert.ToDouble(minList.Min()), 0);
                //chartSIP.ChartAreas[0].AxisY.Interval = (Math.Round(Convert.ToDouble(maxList.Max()), 0) - Math.Round(Convert.ToDouble(maxList.Min()), 0)) / 10;
                chartSIP.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;

                chartSIP.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;
                //chartSIP.ChartAreas[0].AxisY.Interval = 5;
                //chartSIP.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM-yy";
                chartSIP.ChartAreas[0].AxisY.LabelStyle.Format = "#";
                chartSIP.ChartAreas[0].AxisX.LabelStyle.Angle = -40;
                chartSIP.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("verdana", 9.0F, System.Drawing.FontStyle.Regular);
                chartSIP.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("verdana", 9.0F, System.Drawing.FontStyle.Regular);

                chartSIP.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.FromArgb(179, 179, 179);
                chartSIP.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.FromArgb(179, 179, 179);

                chartSIP.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
                chartSIP.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);

                chartSIP.Width = 600;

                var legend = new Legend("Legend2");
                legend.Docking = Docking.Right;
                //legend.IsDockedInsideChartArea = true;
                legend.Alignment = System.Drawing.StringAlignment.Far;
                chartSIP.Legends.Add(legend);


                Div_tab.Visible = true;
                div_text.Visible = true;

                #endregion

                #region Create Graph Image

                var allImageFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "Edelweiss\\images", "SIP_Temp_*");
                foreach (var f in allImageFiles)
                    if (File.GetCreationTime(f) < DateTime.Now.AddHours(-2))
                        File.Delete(f);

                string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                string localImgPath = @"Edelweiss\images";
                localImgPath = System.IO.Path.Combine(appPath, localImgPath);

                if (!Directory.Exists(localImgPath))
                {
                    Directory.CreateDirectory(localImgPath);
                }

                var ImageName = "SIP_Temp_" + Guid.NewGuid().ToString("N") + "_" + "_SIP_Chart.jpg";
                imgPath = System.IO.Path.Combine(localImgPath, ImageName);
                Session["SIPImagePath"] = "http://" + Request.Url.Authority + "/Edelweiss/images/" + ImageName;

                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                }
                chartSIP.SaveImage(imgPath);
                ImgChrt.ImageUrl = "Images\\" + ImageName;
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
            #endregion
        }

        private double CalculateReturn(float FirstValue, float LastValue, int Period)
        {
            if (Period < 365)
            {
                return Math.Round((((LastValue - FirstValue) / FirstValue) * 100) * (Period / Period), 2);
            }
            else
            {
                return Math.Round((((Math.Pow(((LastValue - FirstValue) / FirstValue) + 1, (float)Math.Round((float)365 / (float)Period, 8)) - 1) * 100) * (Period / Period)), 2);
            }
        }

        public static void Show(string message)
        {
            string cleanMessage = message.Replace("'", "\'");
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("alert('{0}');", cleanMessage);
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
            }
        }

        private void LoadSchemes()
        {
            var myXmLfile = Server.MapPath("../App_Data/EdelweissSIP.xml");
            var Schemes = SchemeReader(myXmLfile);
            EdelweissProvidedSchemes = objEdel.GetEdelweissSchemes(Schemes);
        }

        private void LoadFromScheme(bool isGsm)
        {
            var Schemes = (from tbl in EdelweissProvidedSchemes
                           where tbl.Fund_Type == (isGsm ? EdelweissFundType.FromGSMSIP : EdelweissFundType.FromTSIP)
                           select tbl).ToArray();


            System.Data.DataTable dt = Schemes.Select(p => new { Sch_Short_Name = p.Scheme_Name, Scheme_Id = p.Scheme_ID, Launch_Date = p.SchemeInception }).ToDataTable();

            ddlFromScheme.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                Dictionary<string, string> SchemeInception = new Dictionary<string, string>();

                ddlFromScheme.DataTextField = "Scheme_Name";
                ddlFromScheme.DataValueField = "Scheme_ID";
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem li = new ListItem(dr["Sch_Short_Name"].ToString(), dr["Scheme_Id"].ToString());
                    li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    SchemeInception.Add(dr["Scheme_Id"].ToString(), dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    ddlFromScheme.Items.Add(li);
                }
                ViewState["SchemeInception"] = SchemeInception;

            }
        }

        private void LoadToScheme(bool isGsm)
        {
            var Schemes = (from tbl in EdelweissProvidedSchemes
                           where tbl.Fund_Type == (isGsm ? EdelweissFundType.ToGSMSIP : EdelweissFundType.ToTSP)
                            && tbl.Scheme_ID != 2902 && tbl.Scheme_ID != 2904 && tbl.Scheme_ID != 10386 && tbl.Scheme_ID != 2892
                           select tbl).ToArray();

            System.Data.DataTable dt = Schemes.Select(p => new { Sch_Short_Name = p.Scheme_Name, Scheme_Id = p.Scheme_ID, Launch_Date = p.SchemeInception }).ToDataTable();

            ddlToScheme.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                Dictionary<string, string> SchemeInception = new Dictionary<string, string>();

                ddlToScheme.DataTextField = "Scheme_Name";
                ddlToScheme.DataValueField = "Scheme_ID";
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem li = new ListItem(dr["Sch_Short_Name"].ToString(), dr["Scheme_Id"].ToString());
                    li.Attributes.Add("title", dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    SchemeInception.Add(dr["Scheme_Id"].ToString(), dr["Launch_Date"] == DBNull.Value ? "" : Convert.ToDateTime(dr["Launch_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    ddlToScheme.Items.Add(li);
                }
                ViewState["ToSchemeInception"] = SchemeInception;

            }
        }

        private void LoadToSchemeRef(bool isGsm, string exceptschemeid, string schemetype)
        {
            DataTable Schemes = new DataTable();
            if (schemetype == "Dir")
            {
                Schemes = (from tbl in EdelweissProvidedSchemes
                           where tbl.Fund_Type == (isGsm ? EdelweissFundType.ToGSMSIP : EdelweissFundType.ToTSP)
                            && tbl.Scheme_ID != 2902 && tbl.Scheme_ID != 2904 && tbl.Scheme_ID != 10386 && tbl.Scheme_ID != 2892 && tbl.Scheme_ID != Convert.ToDecimal(exceptschemeid) && tbl.Scheme_Name.Contains("Dir")
                           select tbl).ToArray().ToDataTable();
            }
            else
            {
                Schemes = (from tbl in EdelweissProvidedSchemes
                           where tbl.Fund_Type == (isGsm ? EdelweissFundType.ToGSMSIP : EdelweissFundType.ToTSP)
                           && tbl.Scheme_ID != 2902 && tbl.Scheme_ID != 2904 && tbl.Scheme_ID != 10386 && tbl.Scheme_ID != 2892 && tbl.Scheme_ID != Convert.ToDecimal(exceptschemeid) && tbl.Scheme_Name.Contains("Dir") == false
                           select tbl).ToArray().ToDataTable();
            }
            if (Schemes != null && Schemes.Rows.Count > 0)
            {
                ddlToScheme.DataSource = Schemes;
                ddlToScheme.DataTextField = "Scheme_Name";
                ddlToScheme.DataValueField = "Scheme_ID";
                ddlToScheme.DataBind();
            }
        }

        private string GetSchemeidByAmfiCode(string AmfiCode)
        {
            var Schemes = (from tbl in EdelweissProvidedSchemes
                           where tbl.AMFI_Code == AmfiCode
                           select tbl).FirstOrDefault();
            if (Schemes != null)
                return Convert.ToString(Schemes.Scheme_ID);
            else
                return null;
        }

        private void EdelWeissChart()
        {

        }

        protected void ddlSIPType_SelectedIndexChanged(object sender, EventArgs e)
        {
            reportPanel.Visible = false;
            CurrentProcess = ProcessList[ddlSIPType.Text];
            OnChangeSipType(CurrentProcess);
        }

        private void OnChangeSipType(EdelweissProcess currentProcess)
        {
            #region UI Related Changes For SIP Plus

            IntInvtRow.Visible = CurrentProcess == EdelweissProcess.PrepaidSIP ? false : true;
            sourceSchemeselection.Visible = CurrentProcess == EdelweissProcess.PrepaidSIP ? false : true;
            rvInitialInvestment.Enabled = CurrentProcess == EdelweissProcess.PrepaidSIP ? false : true;
            switchAmtSIPPlus.Visible = CurrentProcess == EdelweissProcess.PrepaidSIP ? true : false;
            if (CurrentProcess == EdelweissProcess.PrepaidSIP || CurrentProcess == EdelweissProcess.PrepaidSTP)
                switchAmtSIPPlus.Visible = true;
            else
                switchAmtSIPPlus.Visible = false;
            ChangeDisplayNoOfInvestment();

            #endregion

            if (CurrentProcess == EdelweissProcess.GainSwitchSIP)
            {
                changetype.InnerText = "(capital appreciation %)";
                switchamountid.Visible = false;
                rvInitialInvestment.MaximumValue = "1000000000";
                rvInitialInvestment.ErrorMessage = "&nbsp*Amount Invested should be 25,000 or above";
                IsGSM = true;
                LoadFromScheme(IsGSM);
                LoadToScheme(IsGSM);

                // Design Change

                benchmarkChangeText.Style.Add("visibility", "visible");
                triggarAmt.Style.Add("visibility", "visible");
            }
            if (CurrentProcess == EdelweissProcess.PrepaidSTP)
            {

                ddlFromScheme.Enabled = true;
                changetype.InnerText = "(benchmark change %)";
                switchamountid.Visible = false;
                rvInitialInvestment.MaximumValue = "1800000";
                rvInitialInvestment.ErrorMessage = "&nbsp*Amount Invested Range 25,000 to 18 Lacs";
                IsGSM = false;
                LoadFromScheme(IsGSM);
                LoadToScheme(IsGSM);

                // Design Change

                benchmarkChangeText.Style.Add("visibility", "hidden");
                triggarAmt.Style.Add("visibility", "hidden");

            }
            if (CurrentProcess == EdelweissProcess.PrepaidSIP)
            {
                switchamountid.Visible = false;
                rvInitialInvestment.MaximumValue = "1800000";
                rvInitialInvestment.ErrorMessage = "&nbsp*Amount Invested Range 25,000 to 18 Lacs";
                IsGSM = false;
                LoadToScheme(IsGSM);

                // Design Change

                benchmarkChangeText.Style.Add("visibility", "hidden");
                triggarAmt.Style.Add("visibility", "hidden");
            }
            LoadTriggarAmount(IsGSM);
        }

        protected void ddlFromScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            //reportPanel.Visible = false;
            OnChangeFromScheme();
        }

        private void OnChangeFromScheme()
        {
            string schemetype = ddlFromScheme.SelectedItem.ToString().ToUpper().Trim().Contains("DIR") ? "Dir" : "";
            LoadToSchemeRef(IsGSM, ddlFromScheme.SelectedValue, schemetype);
            //LoadIndex(Convert.ToInt32(ddlFromScheme.SelectedValue));
        }

        private void LoadTriggarAmount(bool isGsm)
        {
            ddlTriggerAmt.Items.Clear();
            if (isGsm)
            {
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "5 %", Value = "5" });
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "10 %", Value = "10" });
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "25 %", Value = "25" });
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "50 %", Value = "50" });
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "75 %", Value = "75" });
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "100 %", Value = "100" });
            }
            else
            {
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "-0.5 %", Value = "0.5" });
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "-1 %", Value = "1" });
                ddlTriggerAmt.Items.Add(new ListItem() { Text = "-2 %", Value = "2" });
            }
        }

        private double SchemeReturn(bool IsARF)
        {
            var NavData = objEdel.navData.Where(p => p.SchemeId == (IsARF ? objEdel.Input.SourceSchemeId : objEdel.Input.DestinitionSchemeId)).OrderBy(p => p.NavDate).ToArray();

            var DayDiff = NavData.Last().NavDate.Subtract(NavData.First().NavDate).Days;
            return CalculateReturn((float)NavData.First().NavValue, (float)NavData.Last().NavValue, DayDiff);
        }

        private double BenchMarkReturn(IndexData[] IndexVal)
        {
            var BenchaMarkDate = IndexVal.OrderBy(p => p.Index_Date).ToArray();

            var DayDiff = BenchaMarkDate.Last().Index_Date.Subtract(BenchaMarkDate.First().Index_Date).Days;
            return CalculateReturn((float)BenchaMarkDate.First().Index_Val, (float)BenchaMarkDate.Last().Index_Val, DayDiff);
        }

        private decimal[] NavReturns(bool IsARF)
        {
            var _NavData = objEdel.navData.Where(p => p.SchemeId == (IsARF ? objEdel.Input.SourceSchemeId : objEdel.Input.DestinitionSchemeId)).ToArray();
            var NavData = (from data in _NavData
                           join dtCommon in objEdel.DateList
                           on data.NavDate equals dtCommon
                           select new { Rec = data.NavValue }.Rec).ToArray();

            IList<decimal> navReturn = new List<decimal>();
            for (int i = 0; i < NavData.Length; i++)
            {
                if (i == 0)
                    navReturn.Add(0);
                else
                {
                    navReturn.Add((((NavData[i] / NavData[i - 1]) - 1) * 100));
                }
            }
            return navReturn.ToArray();
        }

        private decimal[] IndexReturns()
        {
            var IndexData = (from indexes in objEdel.IndexValWithTRI //change to IndexVal to IndexValWithTRI
                             join commonDt in objEdel.DateList
                             on indexes.Index_Date equals commonDt.Date
                             select new { p = Convert.ToDecimal(indexes.Index_Val) }.p).ToArray();

            IList<decimal> indReturn = new List<decimal>();
            for (int i = 0; i < IndexData.Length; i++)
            {
                if (i == 0)
                    indReturn.Add(0);
                else
                {
                    indReturn.Add(((IndexData[i] / IndexData[i - 1]) - 1) * 100);
                }
            }
            return indReturn.ToArray();
        }

        private decimal[] AdditionalIndexReturns()
        {
            var IndexData = (from indexes in objEdel.AdditionalIndexVal
                             join commonDt in objEdel.DateListAdditionalIndex
                             on indexes.Index_Date equals commonDt.Date
                             select new { p = Convert.ToDecimal(indexes.Index_Val) }.p).ToArray();

            IList<decimal> indReturn = new List<decimal>();
            for (int i = 0; i < IndexData.Length; i++)
            {
                if (i == 0)
                    indReturn.Add(0);
                else
                {
                    indReturn.Add(((IndexData[i] / IndexData[i - 1]) - 1) * 100);
                }
            }
            return indReturn.ToArray();
        }
        // List of values rebase to 100

        private decimal[] RebaseValue(decimal[] val)
        {
            IList<decimal> outPut = new List<decimal>();
            if (val.Length > 0)
            {
                outPut.Add(100);
                foreach (var item in val)
                {
                    outPut.Add(Math.Round(outPut.Last() * ((item / 100) + 1), 4));
                }
            }
            return outPut.ToArray();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Thsi function will return Html of Gridview        
        /// </summary>
        /// <param name="objSipGridView"></param>
        /// <returns></returns>
        private string FillHtmlGridViewTable(GridView objSipGridView)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                HttpContext.Current.Response.Clear();
                using (System.IO.StringWriter stringWrite = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite))
                    {
                        if (objSipGridView.Rows.Count > 0)
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

        public void CreateHTMLEdelweissSIP()
        {
            try
            {
                //Response.Write("CreateHTMLEdelweissSIP 0");
                System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
                string gvFirstTablestr = string.Empty;
                string GridViewSIPResultstr = string.Empty;
                string sipGridViewstr = string.Empty;
                string path = string.Empty;

                //Response.Write("CreateHTMLEdelweissSIP 00");

                //var allFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~") + "Edelweiss\\ExcelReport", "EDEL_*");
                //Response.Write("CreateHTMLEdelweissSIP 0001");

                //foreach (var f in allFiles)
                //    if (File.GetCreationTime(f) < DateTime.Now.AddHours(-1))
                //        File.Delete(f);

                //Response.Write("CreateHTMLEdelweissSIP 10101");


                var htmlOfSummary = FillHtmlGridViewTable(gridSummary);
                var htmlFromSchemeDetails = FillHtmlGridViewTable(gridDetailsFrom);
                var htmlToSchemeDetails = FillHtmlGridViewTable(gridDetailsTo);
                //Response.Write("CreateHTMLEdelweissSIP 1");

                StringBuilder sbImage = new StringBuilder("");
                //Response.Write("Session[SIPImagePath]");

                if (!string.IsNullOrEmpty(Convert.ToString(Session["SIPImagePath"])))
                {
                    //Response.Write("Session[SIPImagePath]4");
                    var ImagePath = Session["SIPImagePath"].ToString();
                    //Response.Write("Session[SIPImagePath]5");
                    var imagehtml = sbImage.Append(@"<img src=""").Append(ImagePath).Append(@""" height='300' width='685' >").Append("<br/>");
                    strHTML.Append("<th>SIP Summary</th>").Append("<br/>");
                    strHTML.Append(htmlOfSummary).Append("<br/>");
                    //Response.Write("Session[SIPImagePath]6");

                    strHTML.Append("<th>SIP Graph</th>").Append("<br/>");
                    strHTML.Append(imagehtml);
                }
                //Response.Write("CreateHTMLEdelweissSIP 1 1");

                strHTML.Append("<th>From Scheme : " + Session["FromScheme"] + "</th>").Append("<br/>");
                strHTML.Append(htmlFromSchemeDetails).Append("<br/>");
                //Response.Write("CreateHTMLEdelweissSIP 1 2");

                strHTML.Append("<th>From Scheme : " + Session["ToScheme"] + "</th>").Append("<br/>");
                strHTML.Append(htmlToSchemeDetails).Append("<br/>");
                strHTML.Append("<th>Disclaimer</th>").Append("<br/>");
                var disclaimer = @" The return calculator has been developed and is maintained by ICRA Analytics Limited.
                                    Edelweiss Investment Managers Pvt. Ltd./ ICRA Analytics Ltd do not endorse the authenticity
                                    or accuracy of the figures based on which the returns are calculated; nor shall
                                    they be held responsible or liable for any error or inaccuracy or for any losses
                                    suffered by any investor as a direct or indirect consequence of relying upon the
                                    data displayed by the calculator.";
                strHTML.Append(disclaimer);
                //Response.Write("CreateHTMLEdelweissSIP 2");

                Session["GUID"] = Guid.NewGuid().ToString();

                //Response.Write("CreateHTMLEdelweissSIP 3");

                //path = HttpContext.Current.Server.MapPath("~/Edelweiss/ExcelReport/" + "EDEL" + "_" + Convert.ToString(Session["GUID"]) + ".htm");

               


                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //}
                //using (File.Create(path))
                //{

                //}
                //File.WriteAllText(path, strHTML.ToString());
                var content = strHTML;
             
                Response.AppendHeader("content-disposition", "attachment;filename=SIP_Report_" + Session["GUID"] + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                this.EnableViewState = false;
                Response.Output.Write(content);
                Response.End();
            }
            catch(Exception ex)
            {
                //Response.Write(ex.Message);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        public void SetInceptionOnDropDown()
        {

            if ((ViewState["SchemeInception"] != null) && (ddlFromScheme.Items.Count > 0) && ddlFromScheme.SelectedIndex > 0)
            {

                Dictionary<string, string> SchemeInception = (Dictionary<string, string>)(ViewState["SchemeInception"]);


                for (int i = 0; i < ddlFromScheme.Items.Count; i++)
                {
                    string s = string.Empty;
                    if (SchemeInception.TryGetValue(ddlFromScheme.SelectedItem.Value, out s) && ddlFromScheme.Items[i].Selected == true)
                    {
                        ddlFromScheme.Items[i].Attributes.Add("title", s);
                    }
                }
            }
        }

        private void FillDropdown(Control ddl)
        {
            try
            {
                System.Data.DataTable dt = EdelweissProvidedSchemes.Select(p => new { Sch_Short_Name = p.Scheme_Name, Launch_Date = p.SchemeInception }).ToDataTable();
                DropDownList drpdwn = (DropDownList)ddl;
                drpdwn.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    Dictionary<string, string> SchemeInception = new Dictionary<string, string>();

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

                }

                drpdwn.Items.Insert(0, new ListItem("-Select Scheme-", "0"));
                drpdwn.SelectedIndex = 0;
            }
            catch (Exception ex)
            {


            }
        }

        private DataTable FetchScheme()
        {
            IList<decimal> SchemeIds = new List<decimal>();
            foreach (var item in ddlFromScheme.Items)
            {
                SchemeIds.Add(Convert.ToDecimal(((ListItem)item).Value));
            }
            return null;
        }

        private class Repository_Scheme_Details
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public EdelweissFundType Scheme_Type { get; set; }

        }

        [Obsolete]
        private void AddSchemes()
        {
            if (RepositorySchemes.Count == 0)
            {
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Liquid Fund", Id = 1312, Scheme_Type = EdelweissFundType.FromTSIP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Absolute Return Fund", Id = 3275, Scheme_Type = EdelweissFundType.FromTSIP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Arbitrage Fund", Id = 7310, Scheme_Type = EdelweissFundType.FromTSIP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Diversified Growth Equity Top 100 Fund", Id = 3633, Scheme_Type = EdelweissFundType.ToTSP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Select Midcap Fund", Id = 4481, Scheme_Type = EdelweissFundType.ToTSP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss ELSS Fund", Id = 1316, Scheme_Type = EdelweissFundType.ToTSP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Value Opportunities Fund", Id = 3244, Scheme_Type = EdelweissFundType.ToTSP, });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Absolute Return Fund", Id = 3275, Scheme_Type = EdelweissFundType.ToTSP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Diversified Growth Equity Top 100 Fund", Id = 3633, Scheme_Type = EdelweissFundType.ToGSMSIP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Select Midcap Fund", Id = 4481, Scheme_Type = EdelweissFundType.ToGSMSIP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Value Opportunities Fund", Id = 3244, Scheme_Type = EdelweissFundType.FromGSMSIP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Absolute Return Fund", Id = 3275, Scheme_Type = EdelweissFundType.ToGSMSIP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Arbitrage Fund", Id = 7310, Scheme_Type = EdelweissFundType.ToGSMSIP });
                RepositorySchemes.Add(new Repository_Scheme_Details() { Name = "Edelweiss Short Term Income Fund", Id = 1314, Scheme_Type = EdelweissFundType.ToGSMSIP });
            }
        }

        protected void ddlFromScheme_PreRender(object sender, EventArgs e)
        {
            var isGsm = CurrentProcess == EdelweissProcess.GainSwitchSIP ? true : false;
            var Schemes = (from tbl in EdelweissProvidedSchemes
                           where tbl.Fund_Type == (isGsm ? EdelweissFundType.FromGSMSIP : EdelweissFundType.FromTSIP)
                           select tbl).ToArray();


            //System.Data.DataTable dt = Schemes.Select(p => new { Sch_Short_Name = p.Scheme_Name, Scheme_Id = p.Scheme_ID, Launch_Date = p.SchemeInception }).ToDataTable();

            foreach (ListItem _listItem in ddlFromScheme.Items)
            {
                var inceptionDt = Schemes.Where(p => p.Scheme_ID == Convert.ToDecimal(_listItem.Value)).FirstOrDefault();
                if (inceptionDt != null)
                    _listItem.Attributes.Add("title", inceptionDt.SchemeInception.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            //LoadIndex(Convert.ToInt32(ddlFromScheme.SelectedValue));
        }

        protected void ddlToScheme_PreRender(object sender, EventArgs e)
        {
            //reportPanel.Visible = false;
             OnPreRenderToScheme();
        }
        private void OnPreRenderToScheme()
        {
            LoadIndex(Convert.ToInt32(ddlToScheme.SelectedValue));

            if (CurrentProcess == EdelweissProcess.PrepaidSIP)
            {
                var isGsm = CurrentProcess == EdelweissProcess.GainSwitchSIP ? true : false;
                //var Schemes = (from tbl in EdelweissProvidedSchemes
                //               where tbl.Fund_Type == (isGsm ? EdelweissFundType.FromGSMSIP : EdelweissFundType.FromTSIP)
                //               select tbl).ToArray();

                foreach (ListItem _listItem in ddlToScheme.Items)
                {
                    var inceptionDt = EdelweissProvidedSchemes.Where(p => p.Scheme_ID == Convert.ToDecimal(_listItem.Value)).FirstOrDefault();
                    if (inceptionDt != null)
                        _listItem.Attributes.Add("title", inceptionDt.SchemeInception.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
            }
        }
        protected void ddlToScheme_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}