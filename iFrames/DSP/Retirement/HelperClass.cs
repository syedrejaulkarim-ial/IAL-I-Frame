using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFrames.DSP.Retirement
{
    public class MainSipMonthly
    {
        public double SipAmount;
        public int TotalYear;
    }

    public class AddedSipMonthly : MainSipMonthly
    {
        public int AfterYear;
    }

    public class MainLsInvestment
    {
        public double InvestmentAmount;

    }

    public class AddedLsInvestment : MainLsInvestment
    {
        public int AfterYear;
    }


    public class Corpus_Deduc
    {
        public double Corpus;
        public double Deduction;
    }

       
}