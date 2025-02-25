using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFrames.BLL
{
    public class Top5Sector
    {
        public int Scheme_Id { get; set; }
        public DateTime Port_Date { get; set; }
        public string Scheme_Name { get; set; }
        public string Sector_Name { get; set; }
        public double Scheme { get; set; }
    }

    public class MarketCap
    {
        public DateTime Port_Date { get; set; }
        public string Scheme_Short_Name { get; set; }
        public int Scheme_Id { get; set; }
        public string Market_Slap { get; set; }
        public double M_Cap { get; set; }
    }

    public class AverageMaturity
    {
        public int Scheme_Id { get; set; }
        public DateTime Port_Date { get; set; }
        public string Scheme_Name { get; set; }
        public double Average_Maturity { get; set; }
        public string MonthYear { get; set; }
    }

    public class BansalCapitalMcapAvgMaturity
    {
        public BansalCapitalMcapAvgMaturity()
        {
            LstMarketCap = new List<MarketCap>();
            LstAverageMaturity = new List<AverageMaturity>();
            //LstTop5Sector = new List<Top5Sector>();
        }
        public List<MarketCap> LstMarketCap { get; set; }
        public List<AverageMaturity> LstAverageMaturity { get; set; }
        public bool IsMCap { get; set; }
        public string StyleBoxImgName { get; set; }
        public bool isSchemeEquity { get; set; }
        public bool isStyleBoxExist { get; set; }
    }

    public class ExpenseRatio
    {
        public DateTime? Date { get; set; }
        public double value { get; set; }
        public string StrValue { get; set; }
        public string StrDate
        {
            get
            {
                return Date.HasValue ? Date.Value.ToString("MMM-yy") : "";
            }
        }
    }

    public class CreditRatingInsBreakup
    {
        public IEnumerable<BasicData> LstCreditrating { get; set; }
        public IEnumerable<BasicData> LstInsBreakup { get; set; }
    }
    public class BasicData
    {
        public string DataHead { get; set; }
        public double Data { get { return Math.Round(DataActual, 2); } }
        public double DataActual { get; set; }
    }

    public class PortfolioOthers
    {
        public PTR Ptr { get; set; }
        public AttributionAnalysis AttributionAnalysis { get; set; }
        public YTM Ytm { get; set; }
    }

    public class PTR
    {
        public decimal PT_RATIO_SCHEME_LIST { get; set; }
        public string SchemeName { get; set; }
        public double RATIO { get; set; }
        public string IMPORT_DATE { get; set; }
    }
    public class AttributionAnalysis
    {
        public decimal SCHEME_ID { get; set; }
        public string SCHEME_NAME { get; set; }
        public double PRICE_EARNING { get; set; }
        public DateTime PORT_DATE { get; set; }
        public double PRICE_TO_BOOKVAL { get; set; }
        public double DIV_YIELD { get; set; }
        public double MARKETCAP { get; set; }
    }
    public class YTM
    {
        public int Scheme_Id { get; set; }
        public DateTime Port_Date { get; set; }
        public string Scheme_Name { get; set; }
        public double YTM_Value { get; set; }
    }
}