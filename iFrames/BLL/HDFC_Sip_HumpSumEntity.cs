using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFrames.BLL
{
    public class HDFCSchemesNav
    {
        public string SchemeName { get; set; }
        public int SchemeId { get; set; }
        public DateTime NavDate { get; set; }
        public decimal NavValue { get; set; }
    }
    public class HDFCSIndexData
    {
        public int Index_ID { get; set; }
        public DateTime Index_Date { get; set; }
        public double Index_Val { get; set; }
    }
    public class OutputSIP
    {       
        public DateTime CalculateDate { get; set; }
        public decimal SchemeId { get; set; }
        public decimal IndexId { get; set; }
        public decimal ABenchMarkId { get; set; }

        public string SchemeName { get; set; }
        public string IndexName { get; set; }
        public string ABName { get; set; }

        public decimal InvestmentAmt { get; set; }
        public decimal Scheme_Nav { get; set; }
        public decimal Index_Nav { get; set; }
        public decimal AB_Nav { get; set; }
        public decimal Scheme_Unit { get; set; }
        public decimal Index_Unit { get; set; }
        public decimal AB_Unit { get; set; }
        public decimal SIP_Nav { get; set; }      
        public decimal Scheme_SIP_Return { get; set; }
        public decimal Index_SIP_Return { get; set; }
        public decimal AB_SIP_Return { get; set; }
        public decimal SchemeNav { get; set; }
        public decimal IndexNav { get; set; }
        public decimal Scheme_Div_Value { get; set; }
        public decimal Scheme_Div_Amt { get; set; }
        public decimal Scheme_Div_Unit { get; set; }
        public decimal Scheme_Cumulative_Unit { get; set; }
        public decimal Index_Cumulative_Unit { get; set; }
        public decimal AB_Cumulative_Unit { get; set; }
        public decimal Scheme_Investment_Value { get; set; }
        public decimal Index_Investment_Value { get; set; }
        public decimal AB_Investment_Value { get; set; }
    }
}