using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFrames.BLL
{
    public class WatchScheme
    {
        public int SchemeId { get; set; }
    }
    public class AssetAlocation
	{
		public string NatureName { get; set; }
		public double Value { get; set; }
		public string Date { get; set; }
		public decimal NatureId { get; set; }
	}
	public class ChartNavReturnModel
	{
		public IList<SimpleNavReturnModel> SimpleNavReturnModel { get; set; }
		public string MaxDate { get; set; }
		public string MinDate { get; set; }
		public double MaxVal { get; set; }
		public double MinVal { get; set; }
	}

	public class NavReturnModelSP
	{
		public decimal Id { get; set; }
		public string Scheme_Name { get; set; }
		public DateTime Date { get; set; }
        public string StrDate { get; set; }
        public double ReInvest_Nav { get; set; }
		public double Nav { get; set; }
	}
	public class SimpleNavReturnModel
	{
		public string Name { get; set; }
		public decimal Id { get; set; }
		public bool IsIndex { get; set; }
		public IList<ValueAndDate> ValueAndDate { get; set; }
		public string MaxDate { get; set; }
		public string MinDate { get; set; }
	}
	public class ValueAndDate
	{
		public string Date { get; set; }
		public double Value { get; set; }
        public double OrginalValue { get; set; }
        public string IsIndex { get; set; }

    }


	public class PortfolioMktValue
	{
		public double MatketValue { get; set; }
		public int PortId { get; set; }
		public string PortDate { get; set; }
	}

	public class FundInvestmentInfo
	{
		public string FundName { get; set; }
		public string CurrentNav { get; set; }
		public string FundSize { get; set; }
		public string IncrNav { get; set; }
		public bool IsNavIncr { get; set; }
		public string StructureName { get; set; }
		public string EntryLoad { get; set; }
		public string InvestmentPlan { get; set; }
		public string ExitLoad { get; set; }
		public string MinInvestment { get; set; }
		public DateTime? LunchDate { get; set; }
		public string FundMan { get; set; }
		public string LatestDiv { get; set; }
		public string BenchMark { get; set; }
		public string Email { get; set; }
		public string Bonous { get; set; }
		public string AmcName { get; set; }
		public string PrevNav { get; set; }
		public string Website { get; set; }
        public string SchemeObject { get; set; }
        public DateTime? CurrentNavDate { get; set; }
	}


    public class FundInvestmentInfoNew
    {
        public string FundName { get; set; }
        public string CurrentNav { get; set; }
        public string CurrentNavDate { get; set; }
        public string FundSize { get; set; }
        public string IncrNav { get; set; }
        public bool IsNavIncr { get; set; }
        public string StructureName { get; set; }
        public string EntryLoad { get; set; }
        public string InvestmentPlan { get; set; }
        public string ExitLoad { get; set; }
        public string MinInvestment { get; set; }
        public DateTime? LunchDate { get; set; }
        public string FundMan { get; set; }
        public string LatestDiv { get; set; }
        public string BenchMark { get; set; }
        public string Email { get; set; }
        public string Bonous { get; set; }
        public string AmcName { get; set; }
        public string PrevNav { get; set; }
        public string Website { get; set; }
        public string SchemeObject { get; set; }
    }
}