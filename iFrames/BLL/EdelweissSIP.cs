using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace iFrames.BLL
{
    public class TriggerredIndexDetails
    {
        public int Index_ID { get; set; }
        public DateTime Record_Date { get; set; }
        public decimal Index_Value { get; set; }
        public EdelweissTriggerType IsTriggerred { get; set; }
    }

    public enum EdelweissTriggerType
    {
        NoTrigger,
        PositiveTrigger,
        NegativeTrigger
    }

    public class EdelweissSIPInput
    {
        public int MaxSwitch { get; set; }
        public string CalculatorType { get; set; }

        public int SourceSchemeId { get; set; }

        public int DestinitionSchemeId { get; set; }

        private int _initialInvestment;
        public int InitialInvestment
        {
            get
            {
                //if (_initialInvestment < 25000)
                //{
                //    throw new Exception("Please make minimum investment of 25,000.");
                //}
                return _initialInvestment;
            }
            set
            {
                _initialInvestment = value;
            }
        }
        public string TRIBenchmark { get; set; }//Add new 4 TRI
        public string Benchmark { get; set; }
        public string AdditionalBenchmark { get; set; }

        decimal ouResult = 0M;

        public decimal TriggerCutOff { get; set; }
        /// <summary>
        /// Switching Percent That Calculates The Switching Slab
        /// </summary>
        public decimal SwitchAmount { get; set; }
        public decimal SwitchAmountForMonthlyStp { get; set; }

        private DateTime _fromDate { get; set; }
        public DateTime FromDate
        {

            get { return this._fromDate; }
            set
            {
                if ((this._fromDate.Equals(value) != true))
                {
                    this._fromDate = value;
                    if (this._toDate >= this._fromDate)
                    {
                        throw new Exception("To Date Should Always Be Greater Than From Date.");
                    }
                }
            }
        }

        private DateTime _toDate { get; set; }
        public DateTime ToDate
        {
            get { return this._toDate; }
            set
            {
                if ((this._toDate.Equals(value) != true))
                {
                    this._toDate = value;
                    if (this.ToDate <= this.FromDate)
                    {
                        throw new Exception("To Date Should Always Be Greater Than From Date.");
                    }
                }
            }
        }

        public DateTime _curDate { get; set; }
        public DateTime CurrentDate
        {
            get { return this._curDate; }
            set
            {
                if ((this._curDate.Equals(value) != true))
                {
                    this._curDate = value;
                    if (this.CurrentDate <= this._toDate)
                    {
                        throw new Exception("Current Date Should Always Be Greater Than To Date.");
                    }
                }
            }
        }
        public bool IsGSM { get; set; }
        public EdelweissProcess SIPProcess { get; set; }

        /// <summary>
        /// Amount To Be Switched From Bank Account To Destination Scheme
        /// </summary>
        public decimal SwitchAmtSIPPlus { get; set; }
        public int NoOfSwitchPerMonth { get; set; }

        public Dictionary<decimal, decimal> SwitchingRange { get; set; }

    }

    public class OutputTriggeredSIP
    {
        public DateTime CalculateDate { get; set; }
        [DefaultValue(0)]
        public decimal Liquid { get; set; }
        public decimal Liquid_Unit { get; set; }
        [DefaultValue(0)]
        public decimal Edge { get; set; }
        public decimal Edge_Unit { get; set; }
        public decimal SIP_Nav { get; set; }
        public decimal Swithch_Amt { get; set; }
        /// <summary>
        /// Absolute Return
        /// </summary>
        public decimal SIP_Return { get; set; }

        // Test Purpose
        public decimal SourceNav { get; set; }
        public decimal DestinationNav { get; set; }
        public bool IsTriggard { get; set; }
        public bool IsGsm { get; set; }
        public decimal Sip_Return_New { get; set; } //Add new 15-03-2018
        public bool IsMonthlyDeduct { get; set; }//Add new 15-03-2018
    }

    public class SchemesNav
    {
        public int SchemeId { get; set; }
        public DateTime NavDate { get; set; }
        public decimal NavValue { get; set; }
    }

    public class OutputMonthlySIP : OutputTriggeredSIP
    {

    }

    public class BasicSIP
    {
        public DateTime SIP_Date { get; set; }
        public decimal Scheme_Nav { get; set; }
        public decimal SIP_Nav { get; set; }
        public decimal SIP_Unit { get; set; }
        public decimal SIP_Return { get; set; }
    }

    public class EdelweissSchemes
    {
        public string Fund_Name { get; set; }
        public decimal Fund_Id { get; set; }
        public EdelweissFundType Fund_Type { get; set; }
        public string Scheme_Name { get; set; }
        public decimal Scheme_ID { get; set; }
        public DateTime SchemeInception { get; set; }
        public string Scheme_Code { get; set; }
        public string AMFI_Code { get; set; }
    }

    public enum EdelweissFundType
    {
        FromTSIP,
        ToTSP,
        FromGSMSIP,
        ToGSMSIP
    }

    public class IndexData
    {
        public int Index_ID { get; set; }
        public DateTime Index_Date { get; set; }
        public double Index_Val { get; set; }
    }

    public class EdelweissGraph
    {
        public DateTime Date { get; set; }
        public decimal Nifty { get; set; }
        public decimal TriggerSIP { get; set; }
        public decimal MonthlySIP { get; set; }
    }

    public class EdelweissReturnParams
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FromScheme { get; set; }
        public string ToScheme { get; set; }
        public string SelectedIndex { get; set; }
    }

    public class EdelweissIndexReturn
    {
        public int INDEX_ID { get; set; }
        public DateTime RECORD_DATE { get; set; }
        public decimal INDEX_VALUE { get; set; }
    }

    public class EdelweissSchemeReturn
    {
        public int Scheme_id { get; set; }
        public DateTime RECORD_DATE { get; set; }
        public decimal Nav_Value { get; set; }
    }

    public enum EdelweissProcess
    {
        PrepaidSTP,
        GainSwitchSIP,
        PrepaidSIP
    }

    public class OutputSIPPlus
    {
        public decimal InvestedAmt { get; set; }
        public decimal DestinationSchemeNav { get; set; }
        public decimal SIPUnit { get; set; }
        public decimal TransferredAmt { get; set; }
        public decimal AllowedTransactionLimit { get; set; }
        public DateTime CalculatingDate { get; set; }
        public bool IsTriggard { get; set; }
        public decimal? CurrentValuation { get; set; }
        public decimal SIPReturn { get; set; }
        public decimal SIPReturn_New { get; set; }

    }
    /// <summary>
    /// Holds The Of Transactions Occurred in a Month
    /// </summary>
    public class SIPPlusMonthlyDeductionCount
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class EdelSchemeIndex
    {
        public int IndexID { get; set; }
        public string IndexName { get; set; }
    }
    public class SchemeNature
    {
        public decimal NatureID { get; set; }
        public string Nature { get; set; }
        public string SubNature { get; set; }
        public decimal SubNatureId { get; set; }
    }

}