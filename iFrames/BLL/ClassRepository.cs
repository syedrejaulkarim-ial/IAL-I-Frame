using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Reflection;
using iFrames.BLL;
using System.ComponentModel; 

namespace iFrames.BLL
{
    public class SchemeIndexList
    {
        public List<decimal> ListScheme { get; set; }
        public List<decimal> ListIndex { get; set; }

        public SchemeIndexList()
        {
            ListScheme = new List<decimal>();
            ListIndex = new List<decimal>();
        }

    }
    
  
    public class FundManager
    {
        public int MFId { get; set; }
        public string MFName { get; set; }

    }
    public class FundManagerDataStructure
    {
        public List<DataStructure> LstDataStructure { get; set; }
        public string FundManagerName { get; set; }
    }

    public class DataStructure
    {
        public List<List<string>> PageData { get; set; }
        public List<string> PageDataStructure { get; set; }
        public List<string> PageDataReturnHeader { get; set; }
    }

    public class TataSebiParam
    {
        public System.Data.DataTable DtData { get; set; }
        public List<string> ReturnColumn { get; set; }
    }

    public class ClassRepository
    {

        public string Year { get; set; }
        public int? Scheme_Index_Id { get; set; }
        public string Scheme_Index_name { get; set; }
        public double? Total_Amount_Invest { get; set; }
        public double? Scheme_Return { get; set; }
        public double? Scheme_Market_Value { get; set; }
        public DateTime? Inception_Date { get; set; }
    }

    public class PreCalculatedRatioResults
    {
        public decimal Scheme_Id { get; set; }
        public double? Sharp { get; set; }
        public double? Sortino { get; set; }
        public double? STDV { get; set; }
        public double? Beta { get; set; }
        public double? RSQR { get; set; }
    }

    public class TopFundRank
    {
        public string Sch_Name { get; set; }
        public DateTime Calculation_Date { get; set; }
        public double? Div_Yield { get; set; }
        public double? Per_7_Days { get; set; }
        public double? Per_30_Days { get; set; }
        public double? Per_91_Days { get; set; }
        public double? Per_182_Days { get; set; }
        public double? Per_1_Year { get; set; }
        public double? Per_3_Year { get; set; }
        public double? Per_5_Year { get; set; }
        public double? Since_Inception { get; set; }
        public decimal Nature_ID { get; set; }
        public decimal Structure_ID { get; set; }
        public string Nature { get; set; }
        public string SubNature { get; set; }
        public double? Nav { get; set; }
        public decimal? SubcategoryId { get; set; }
        public decimal? OptionId { get; set; }
        public decimal? RiskColorId { get; set; }
        public double? MinInvest { get; set; }
        public decimal? MF_Rank { get; set; }
        public string MF_Rank_Html { get; set; }
        public decimal SchemeId { get; set; }
        public double? Fund_Size { get; set; }
        public string AmfiCode { get; set; }

    }

    public class topHolding
    {
        public string Sch_Short_Name { get; set; }
        public decimal Scheme_Id { get; set; }
       // public string Sector_Name { get; set; }
        public decimal PORT_ID { get; set; }
        public double? SumCorpus { get; set; }
    }

    public class ExitLoadOutput
    {
        public decimal SCHEME_ID { get; set; }
        public string SCH_SHORT_NAME { get; set; }
        public string EE_Type { get; set; }
        public string Dur_Type1 { get; set; }
        public string Dur_Type2 { get; set; }
        public string Cond_Type { get; set; }
        public string Remark { get; set; }  

        public double? EE_Load { get; set; }
        public double? Greater_Cond { get; set; }
        public double? Less_Cond { get; set; }
        public decimal? Cond_Id { get; set; }
        public double? CDSC { get; set; }
      
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
    }

    #region Class Definition
    public class SipCalcReturnDataSummary
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

    public class SipCalcReturnDataDetail
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


    public class AttributeAnalysisOutput
    {
       
        public decimal SCHEME_ID { get; set; }
        public string SCHEME_NAME { get; set; }
        public DateTime PORT_DATE { get; set; }
        public double? PRICE_EARNING { get; set; }
        public double? PRICE_TO_BOOKVAL { get; set; }
        public double? DIV_YIELD { get; set; }
        public double? MARKETCAP { get; set; }       
    }

    #region : Class Definiton
    public class CalcReturnData
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

    public class CalcReturnData2
    {
        public decimal ID { get; set; }
        public decimal SCHEME_ID { get; set; }
        public string SCHEME_NAME { get; set; }
        public string NAV_DATE { get; set; }
        public double NAV { get; set; }
        public double? INDEX_VALUE { get; set; }
        public double? SCHEME_UNITS { get; set; }
        public double? INDEX_UNIT { get; set; }
        public double? SCHEME_CASHFLOW { get; set; }
        public double? INDEX_CASHFLOW { get; set; }
        public double? CUMULATIVE_AMOUNT { get; set; }
        public double? AMOUNT_ONETIME { get; set; }
        public double? SCHEME_UNIT_ONETIME { get; set; }
        public double? INDEX_UNIT_ONETIME { get; set; }
        public string DIVIDEND_BONUS { get; set; }
    }

    public class SchemeSpReturn
    {
        public decimal SCHEME_ID { get; set; }
        public string SCHEME_NAME { get; set; }
        public char Scince_Inception { get; set; }
    }

    public class IndexSpReturn
    {
        public decimal INDEX_ID { get; set; }
        public string INDEX_NAME { get; set; }
        public double? Index_type { get; set; }
    }

    #endregion
    #endregion
}


namespace iFrames.DAL
{
    public partial class SIP_ClientDataContext : System.Data.Linq.DataContext
    {
        //[Function(Name = "dbo.MFIE_SP_SIP_CALCULATER")]
        //[ResultType(typeof(iFrames.Pages.CalcReturnData2))]
        //[ResultType(typeof(iFrames.Pages.CalcReturnData))]
        //public IMultipleResults MFIE_SP_SIP_CALCULATER([Parameter(Name = "Scheme_Ids", DbType = "VarChar(MAX)")] string scheme_Ids, [Parameter(Name = "Plan_Start_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_Start_Date, [Parameter(Name = "Plan_End_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_End_Date, [Parameter(Name = "Report_As_On", DbType = "DateTime")] System.Nullable<System.DateTime> report_As_On, [Parameter(Name = "SIP_Amt", DbType = "Float")] System.Nullable<double> sIP_Amt, [Parameter(Name = "Frequency", DbType = "VarChar(50)")] string frequency, [Parameter(Name = "Dividend_Type", DbType = "VarChar(50)")] string dividend_Type, [Parameter(Name = "Initial_Flage", DbType = "Int")] System.Nullable<int> initial_Flage, [Parameter(Name = "Initial_Amount", DbType = "Float")] System.Nullable<double> initial_Amount, [Parameter(Name = "Initial_Date", DbType = "DateTime")] System.Nullable<System.DateTime> initial_Date, [Parameter(Name = "Index_Flage", DbType = "Int")] System.Nullable<int> index_Flage)
        //{
        //    IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo )(MethodInfo.GetCurrentMethod())), scheme_Ids, plan_Start_Date, plan_End_Date, report_As_On, sIP_Amt, frequency, dividend_Type, initial_Flage, initial_Amount, initial_Date, index_Flage);
        //    return ((IMultipleResults)(result.ReturnValue));
        //}

        [Function(Name = "dbo.MFIE_SP_SIP_CALCULATER_CLIENT_Scheme")]
        [ResultType(typeof(SipCalcReturnDataSummary))]
        //[ResultType(typeof(SipCalcReturnDataDetail))]

        public IMultipleResults MFIE_SP_SIP_CALCULATER_CLIENT_Scheme([Parameter(Name = "Scheme_Ids", DbType = "VarChar(MAX)")] string scheme_Ids, [Parameter(Name = "Plan_Start_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_Start_Date, [Parameter(Name = "Plan_End_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_End_Date, [Parameter(Name = "Report_As_On", DbType = "DateTime")] System.Nullable<System.DateTime> report_As_On, [Parameter(Name = "SIP_Amt", DbType = "Float")] System.Nullable<double> sIP_Amt, [Parameter(Name = "Frequency", DbType = "VarChar(50)")] string frequency, [Parameter(Name = "Dividend_Type", DbType = "VarChar(50)")] string dividend_Type, [Parameter(Name = "Initial_Flage", DbType = "Int")] System.Nullable<int> initial_Flage, [Parameter(Name = "Initial_Amount", DbType = "Float")] System.Nullable<double> initial_Amount, [Parameter(Name = "Initial_Date", DbType = "DateTime")] System.Nullable<System.DateTime> initial_Date, [Parameter(Name = "Index_Flage", DbType = "Int")] System.Nullable<int> index_Flage, [Parameter(Name = "OPTIONAL_RET_FLAG", DbType = "VarChar(1)")] string oPTIONAL_RET_FLAG)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), scheme_Ids, plan_Start_Date, plan_End_Date, report_As_On, sIP_Amt, frequency, dividend_Type, initial_Flage, initial_Amount, initial_Date, index_Flage, oPTIONAL_RET_FLAG);
            return ((IMultipleResults)(result.ReturnValue));
        }


        [Function(Name = "dbo.MFIE_SP_SCHEME_GENERAL_INFORMATION")]
        [ResultType(typeof(ExitLoadOutput))]
        public IMultipleResults MFIE_SP_SCHEME_GENERAL_INFORMATION([Parameter(Name = "INFORMATIONTYPE", DbType = "VarChar(50)")] string iNFORMATIONTYPE, [Parameter(Name = "SCHEMEIDS", DbType = "VarChar(8000)")] string sCHEMEIDS, [Parameter(Name = "OPTIONALPARAMETER", DbType = "VarChar(8000)")] string oPTIONALPARAMETER)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), iNFORMATIONTYPE, sCHEMEIDS, oPTIONALPARAMETER);
            return ((IMultipleResults)(result.ReturnValue));
        }


        [Function(Name = "dbo.MFIE_SP_ATTRIBUTION_ANALYSIS")]
        [ResultType(typeof(AttributeAnalysisOutput))] 
        public IMultipleResults MFIE_SP_ATTRIBUTION_ANALYSIS([Parameter(Name = "PSCHEMEIDS", DbType = "VarChar(MAX)")] string pSCHEMEIDS, [Parameter(Name = "PDATE", DbType = "VarChar(2002)")] string pDATE, [Parameter(Name = "CRIATARIA", DbType = "VarChar(2002)")] string cRIATARIA)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), pSCHEMEIDS, pDATE, cRIATARIA);
            return ((IMultipleResults)(result.ReturnValue));
        }
  
        //[Function(Name = "dbo.MFIE_SP_SCHEME_P2P_ROLLING_RETURN")]
        //[ResultType(typeof(ExitLoadOutput))]
        //public IMultipleResults MFIE_SP_SCHEME_P2P_ROLLING_RETURN([Parameter(Name = "SCHEMEIDS", DbType = "VarChar(MAX)")] string sCHEMEIDS, [Parameter(Name = "SETTINGSETID", DbType = "Decimal(18,0)")] System.Nullable<decimal> sETTINGSETID, [Parameter(Name = "DATEFROM", DbType = "DateTime")] System.Nullable<System.DateTime> dATEFROM, [Parameter(Name = "DATETO", DbType = "DateTime")] System.Nullable<System.DateTime> dATETO, [Parameter(Name = "ROUNDTILL", DbType = "Int")] System.Nullable<int> rOUNDTILL, [Parameter(Name = "ROLLINGPERIODIN", DbType = "VarChar(500)")] string rOLLINGPERIODIN, [Parameter(Name = "ROLLINGPERIOD", DbType = "Int")] ref System.Nullable<int> rOLLINGPERIOD, [Parameter(Name = "ROLLINGFREQUENCYIN", DbType = "VarChar(10)")] string rOLLINGFREQUENCYIN, [Parameter(Name = "ROLLINGFREQUENCY", DbType = "Int")] ref System.Nullable<int> rOLLINGFREQUENCY, [Parameter(Name = "ROLLING_P2P", DbType = "VarChar(50)")] string rOLLING_P2P, [Parameter(Name = "OTHERCALCULATION", DbType = "VarChar(3)")] string oTHERCALCULATION)
        //{
        //    IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), sCHEMEIDS, sETTINGSETID, dATEFROM, dATETO, rOUNDTILL, rOLLINGPERIODIN, rOLLINGPERIOD, rOLLINGFREQUENCYIN, rOLLINGFREQUENCY, rOLLING_P2P, oTHERCALCULATION);
        //    rOLLINGPERIOD = ((System.Nullable<int>)(result.GetParameterValue(6)));
        //    rOLLINGFREQUENCY = ((System.Nullable<int>)(result.GetParameterValue(8)));
        //    return ((IMultipleResults)(result.ReturnValue));
        //}

    }


    public partial class PrincipalCalcDataContext : System.Data.Linq.DataContext
    {
        [Function(Name = "dbo.MFIE_SP_SIP_CALCULATER")]
        [ResultType(typeof(CalcReturnData2))]
        [ResultType(typeof(CalcReturnData))]
        public IMultipleResults MFIE_SP_SIP_CALCULATER([Parameter(Name = "Scheme_Ids", DbType = "VarChar(MAX)")] string scheme_Ids, [Parameter(Name = "Plan_Start_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_Start_Date, [Parameter(Name = "Plan_End_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_End_Date, [Parameter(Name = "Report_As_On", DbType = "DateTime")] System.Nullable<System.DateTime> report_As_On, [Parameter(Name = "SIP_Amt", DbType = "Float")] System.Nullable<double> sIP_Amt, [Parameter(Name = "Frequency", DbType = "VarChar(50)")] string frequency, [Parameter(Name = "Dividend_Type", DbType = "VarChar(50)")] string dividend_Type, [Parameter(Name = "Initial_Flage", DbType = "Int")] System.Nullable<int> initial_Flage, [Parameter(Name = "Initial_Amount", DbType = "Float")] System.Nullable<double> initial_Amount, [Parameter(Name = "Initial_Date", DbType = "DateTime")] System.Nullable<System.DateTime> initial_Date, [Parameter(Name = "Index_Flage", DbType = "Int")] System.Nullable<int> index_Flage)
        {
            this.CommandTimeout = 60;
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), scheme_Ids, plan_Start_Date, plan_End_Date, report_As_On, sIP_Amt, frequency, dividend_Type, initial_Flage, initial_Amount, initial_Date, index_Flage);
            return ((IMultipleResults)(result.ReturnValue));
        }
    }
}