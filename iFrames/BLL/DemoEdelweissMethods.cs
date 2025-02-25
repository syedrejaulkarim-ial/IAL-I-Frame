
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using iFrames.DAL;

namespace iFrames.BLL
{
    public class DemoEdelweissMethods
    {
        #region Edelweiss SIP


        private DateTime[] _IndexDateList;
        public DateTime[] IndexDateList
        {
            get
            {
                if (_IndexDateList == null)
                {
                    _IndexDateList = (from data in GetTriggeredIndexData()
                                      select new { IndexDatetime = data.Record_Date }.IndexDatetime).ToArray();
                }
                return _IndexDateList;
            }
            set
            {
                _IndexDateList = value;
            }
        }

        private SchemesNav[] _navData;
        public SchemesNav[] navData
        {
            get
            {
                if (_navData == null)
                    _navData = GetSchemesNav();
                return _navData;
            }
            set { _navData = value; }
        }

        private TriggerredIndexDetails[] _indexData;
        public TriggerredIndexDetails[] IndexData
        {
            get
            {
                if (_indexData == null)
                    _indexData = GetTriggeredIndexData().OrderBy(p => p.Record_Date).ToArray();
                return _indexData;
            }
            set { _indexData = value; }
        }

        private EdelweissSIPInput _Input;
        public EdelweissSIPInput Input
        {
            get
            {
                if (_Input == null)
                    throw new Exception("Please provide the input");
                return _Input;
            }
            set { _Input = value; }
        }

        private DateTime[] _dateList;
        public DateTime[] DateList
        {
            get
            {
                if (Input != null)
                    if (_dateList == null)
                        _dateList = GetCommonDateTime();
                return _dateList;
            }
            set { _dateList = value; }
        }

        private DateTime[] _dateListAdditionalIndex;
        public DateTime[] DateListAdditionalIndex
        {
            get
            {
                if (Input != null)
                    if (_dateListAdditionalIndex == null)
                        _dateListAdditionalIndex = GetCommonDateTimeForAdditionalIndex();
                return _dateListAdditionalIndex;
            }
            set { _dateListAdditionalIndex = value; }
        }

        //Add for TRI
        private IndexData[] _indexValWithTRI;
        public IndexData[] IndexValWithTRI
        {
            get
            {
                if (Input != null)
                    if (_indexValWithTRI == null)
                        _indexValWithTRI = GetIndexData(Input.TRIBenchmark);
                return _indexValWithTRI;
            }
            set { _indexValWithTRI = value; }
        }
        //End
        private IndexData[] _indexVal;

        private IndexData _indexValPrev;
        public IndexData[] IndexVal
        {
            get
            {
                if (Input != null)
                    if (_indexVal == null)
                        _indexVal = GetIndexData(Input.Benchmark);
                return _indexVal;
            }
            set { _indexVal = value; }
        }
        //GetPreviousIndexData

        public IndexData IndexValPrevious
        {
            get
            {
                if (Input != null)
                    if (_indexValPrev == null)
                        _indexValPrev= GetPreviousIndexData(Input.Benchmark);
                return _indexValPrev;
            }
            set { _indexValPrev = value; }
        }

        private IndexData GetPreviousIndexData(string index)
        {
            using (var db = new PrincipalCalcDataContext())
            {
                var IndexId = db.T_INDEX_MASTERs.Where(p => p.INDEX_NAME == index).FirstOrDefault().INDEX_ID;
                return (from tbl in db.T_INDEX_RECORDs
                        where tbl.RECORD_DATE < Input.FromDate && tbl.INDEX_ID == IndexId
                        select new IndexData()
                        {
                            Index_ID = (int)tbl.INDEX_ID,
                            Index_Date = tbl.RECORD_DATE,
                            Index_Val = tbl.INDEX_VALUE
                        }).OrderByDescending(p => p.Index_Date).FirstOrDefault();
            }
        }
        private IndexData[] GetIndexData(string index) //change blank parameter to index
        {
            using (var db = new PrincipalCalcDataContext())
            {
                var IndexId = db.T_INDEX_MASTERs.Where(p => p.INDEX_NAME == index).FirstOrDefault().INDEX_ID; //change Input.Benchmark to index
                //var Data = db.GetTable<T_SCHEMES_MASTER>().AsEnumerable().First();
                return (from tbl in db.T_INDEX_RECORDs
                        //join tblMast in db.GetTable<T_INDEX_MASTER>().AsEnumerable() on tbl.INDEX_ID equals tblMast.INDEX_ID
                        where tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                            //&& tblMast.INDEX_NAME == Input.Benchmark
                        && tbl.INDEX_ID == IndexId
                        select new IndexData()
                        {
                            Index_ID = (int)tbl.INDEX_ID,
                            Index_Date = tbl.RECORD_DATE,
                            Index_Val = tbl.INDEX_VALUE
                        }).OrderBy(p => p.Index_Date).ToArray();

                //return (from tbl in db.GetTable<T_INDEX_RECORD>().AsEnumerable()
                //        join tblMast in db.GetTable<T_INDEX_MASTER>().AsEnumerable() on tbl.INDEX_ID equals tblMast.INDEX_ID
                //        where tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                //        && tblMast.INDEX_NAME == Input.Benchmark
                //        select new IndexData()
                //        {
                //            Index_Date = tbl.RECORD_DATE,
                //            Index_Val = tbl.INDEX_VALUE
                //        }).ToArray();
            }
        }

        private DateTime[] GetCommonDateTime()
        {
            try
            {
                if (Input.SIPProcess == EdelweissProcess.PrepaidSIP)
                {   
                    var DestSchemeData = navData.Where(p => p.SchemeId == Input.DestinitionSchemeId).OrderBy(p => p.NavDate).ToArray();

                    var SelectedIndexData = this.IndexVal;
                    return (from indexDt in SelectedIndexData

                            join dScheme in DestSchemeData on indexDt.Index_Date equals dScheme.NavDate
                            select new { UniqueDt = indexDt.Index_Date }.UniqueDt).ToArray();
                }
                else
                {
                    var SourceSchemeData = navData.Where(p => p.SchemeId == Input.SourceSchemeId).OrderBy(p => p.NavDate).ToArray();
                    var DestSchemeData = navData.Where(p => p.SchemeId == Input.DestinitionSchemeId).OrderBy(p => p.NavDate).ToArray();

                    var SelectedIndexData = this.IndexVal;
                    return (from indexDt in SelectedIndexData
                            join sScheme in SourceSchemeData on indexDt.Index_Date equals sScheme.NavDate
                            join dScheme in DestSchemeData on sScheme.NavDate equals dScheme.NavDate
                            select new { UniqueDt = indexDt.Index_Date }.UniqueDt).ToArray();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private DateTime[] GetCommonDateTimeForAdditionalIndex()
        {
            try
            {
                if (Input.SIPProcess == EdelweissProcess.PrepaidSIP)
                {
                   
                    var DestSchemeData = navData.Where(p => p.SchemeId == Input.DestinitionSchemeId).OrderBy(p => p.NavDate).ToArray();

                    var SelectedIndexData = this.AdditionalIndexVal;
                    return (from indexDt in SelectedIndexData
                            join dScheme in DestSchemeData on indexDt.Index_Date equals dScheme.NavDate
                            select new { UniqueDt = indexDt.Index_Date }.UniqueDt).ToArray();
                }
                else
                {

                    var SourceSchemeData = navData.Where(p => p.SchemeId == Input.SourceSchemeId).OrderBy(p => p.NavDate).ToArray();
                    var DestSchemeData = navData.Where(p => p.SchemeId == Input.DestinitionSchemeId).OrderBy(p => p.NavDate).ToArray();

                    var SelectedIndexData = this.AdditionalIndexVal;
                    return (from indexDt in SelectedIndexData
                            join sScheme in SourceSchemeData on indexDt.Index_Date equals sScheme.NavDate
                            join dScheme in DestSchemeData on sScheme.NavDate equals dScheme.NavDate
                            select new { UniqueDt = indexDt.Index_Date }.UniqueDt).ToArray();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TriggerredIndexDetails[] GetTriggeredIndexData4TRI()
        {
            try
            {
                using (var db = new EdelweissSIPDataContext())
                {

                    if (Input.SIPProcess == EdelweissProcess.GainSwitchSIP)
                    {
                        var SchemeReturn = SchemeReturnData();
                        return (from tbl in SchemeReturn
                                where tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                                select new TriggerredIndexDetails
                                {
                                    Index_ID = tbl.Scheme_id,
                                    Record_Date = tbl.RECORD_DATE,
                                    Index_Value = Convert.ToDecimal(tbl.Nav_Value),
                                    IsTriggerred = tbl.Nav_Value > Input.TriggerCutOff ? EdelweissTriggerType.PositiveTrigger : EdelweissTriggerType.NoTrigger
                                }).OrderBy(p => p.Record_Date).ToArray();
                    }
                    if (Input.SIPProcess == EdelweissProcess.PrepaidSTP)
                    {
                        var IndexReturn = IndexReturnData();
                        var MinTriggarCutoff = Input.SwitchingRange.Select(p => p.Key).Max();//add new 
                        return (from tbl in IndexReturn
                                join indexM in db.GetTable<T_INDEX_MASTER>().AsEnumerable() on tbl.INDEX_ID equals indexM.INDEX_ID
                                where indexM.INDEX_NAME == Input.Benchmark && tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                                select new TriggerredIndexDetails
                                {
                                    Index_ID = tbl.INDEX_ID,
                                    Record_Date = tbl.RECORD_DATE,
                                    Index_Value = tbl.INDEX_VALUE,
                                    //IsTriggerred = (tbl.INDEX_VALUE < -Input.TriggerCutOff) ? EdelweissTriggerType.NegativeTrigger : EdelweissTriggerType.NoTrigger
                                    IsTriggerred = (tbl.INDEX_VALUE < MinTriggarCutoff) ? EdelweissTriggerType.NegativeTrigger : EdelweissTriggerType.NoTrigger
                                }).OrderBy(p => p.Record_Date).ToArray();
                    }
                    if (Input.SIPProcess == EdelweissProcess.PrepaidSIP)
                    {
                        // var IndexReturn = IndexReturnData4TRI();
                        var IndexReturn = IndexReturnData();
                        var MinTriggarCutoff = Input.SwitchingRange.Select(p => p.Key).Max();

                        var x = -Input.TriggerCutOff;
                        var nn1 = IndexReturn.Where(c => c.INDEX_VALUE < Convert.ToDecimal(0.00001)).ToArray();

                        var nn = IndexReturn.Where(c => c.INDEX_VALUE < MinTriggarCutoff).ToArray();
                        return (from tbl in IndexReturn
                                join indexM in db.GetTable<T_INDEX_MASTER>().AsEnumerable() on tbl.INDEX_ID equals indexM.INDEX_ID
                                where
                                //commented on 22 May 2019
                                //indexM.INDEX_NAME == Input.TRIBenchmark 
                                indexM.INDEX_NAME == Input.Benchmark
                                && tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                                select new TriggerredIndexDetails
                                {
                                    Index_ID = tbl.INDEX_ID,
                                    Record_Date = tbl.RECORD_DATE,
                                    Index_Value = tbl.INDEX_VALUE,
                                    IsTriggerred = (tbl.INDEX_VALUE < MinTriggarCutoff) ? EdelweissTriggerType.NegativeTrigger : EdelweissTriggerType.NoTrigger
                                }).OrderBy(p => p.Record_Date).ToArray();
                    }

                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TriggerredIndexDetails[] GetTriggeredIndexData()
        {
            try
            {
                using (var db = new EdelweissSIPDataContext())
                {

                    if (Input.SIPProcess == EdelweissProcess.GainSwitchSIP)
                    {
                        var SchemeReturn = SchemeReturnData();
                        return (from tbl in SchemeReturn
                                where tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                                select new TriggerredIndexDetails
                                {
                                    Index_ID = tbl.Scheme_id,
                                    Record_Date = tbl.RECORD_DATE,
                                    Index_Value = Convert.ToDecimal(tbl.Nav_Value),
                                    IsTriggerred = tbl.Nav_Value > Input.TriggerCutOff ? EdelweissTriggerType.PositiveTrigger : EdelweissTriggerType.NoTrigger
                                }).OrderBy(p => p.Record_Date).ToArray();
                    }
                    if (Input.SIPProcess == EdelweissProcess.PrepaidSTP)
                    {
                        var IndexReturn = IndexReturnData();
                        var MinTriggarCutoff = Input.SwitchingRange.Select(p => p.Key).Max();//add new 
                        return (from tbl in IndexReturn
                                join indexM in db.GetTable<T_INDEX_MASTER>().AsEnumerable() on tbl.INDEX_ID equals indexM.INDEX_ID
                                where indexM.INDEX_NAME == Input.Benchmark && tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                                select new TriggerredIndexDetails
                                {
                                    Index_ID = tbl.INDEX_ID,
                                    Record_Date = tbl.RECORD_DATE,
                                    Index_Value = tbl.INDEX_VALUE,
                                    //IsTriggerred = (tbl.INDEX_VALUE < -Input.TriggerCutOff) ? EdelweissTriggerType.NegativeTrigger : EdelweissTriggerType.NoTrigger
                                    IsTriggerred = (tbl.INDEX_VALUE < MinTriggarCutoff) ? EdelweissTriggerType.NegativeTrigger : EdelweissTriggerType.NoTrigger
                                }).OrderBy(p => p.Record_Date).ToArray();
                    }
                    if (Input.SIPProcess == EdelweissProcess.PrepaidSIP)
                    {
                        var IndexReturn = IndexReturnData();

                        var MinTriggarCutoff = Input.SwitchingRange.Select(p => p.Key).Max();

                        var x = -Input.TriggerCutOff;

                        return (from tbl in IndexReturn
                                join indexM in db.GetTable<T_INDEX_MASTER>().AsEnumerable() on tbl.INDEX_ID equals indexM.INDEX_ID
                                where indexM.INDEX_NAME == Input.Benchmark && tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                                select new TriggerredIndexDetails
                                {
                                    Index_ID = tbl.INDEX_ID,
                                    Record_Date = tbl.RECORD_DATE,
                                    Index_Value = tbl.INDEX_VALUE,
                                    IsTriggerred = (tbl.INDEX_VALUE < MinTriggarCutoff) ? EdelweissTriggerType.NegativeTrigger : EdelweissTriggerType.NoTrigger
                                }).OrderBy(p => p.Record_Date).ToArray();
                    }

                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Nav is fetched for the two schemes for the range of selected from date to selected current date
        /// </summary>
        /// <returns></returns>
        private SchemesNav[] GetSchemesNav()
        {
            int[] Schemes = new int[] { Input.SourceSchemeId, Input.DestinitionSchemeId };

            using (var db = new EdelweissSIPDataContext())
            {

                return (from tbl in db.GetTable<T_NAV_DIV>()
                        join schm in db.GetTable<T_SCHEMES_MASTER>() on tbl.Scheme_Id equals schm.Scheme_Id
                        where tbl.Nav_Date >= Input.FromDate && tbl.Nav_Date <= Input.CurrentDate
                        && Schemes.Contains(Convert.ToInt32(tbl.Scheme_Id))
                        select new SchemesNav
                        {
                            SchemeId = Convert.ToInt32(tbl.Scheme_Id),
                            NavDate = tbl.Nav_Date.Value,
                            NavValue = Convert.ToDecimal(tbl.Nav.Value)
                        }).OrderBy(p => p.NavDate).ToArray();
            }
        }

        public IList<OutputTriggeredSIP> CalculateTriggeredSIP(EdelweissSIPInput Input, out string Msg)
        {
            Msg = string.Empty;
            var uniqeDt = DateList.ToList();
            var TrigIndexData = GetTriggeredIndexData();
            if (uniqeDt.First().Date > Input.FromDate) uniqeDt.Insert(0, Input.FromDate);
            var IndexData = (from indexData in TrigIndexData
                             join uniqueDate in uniqeDt on indexData.Record_Date equals uniqueDate
                             select indexData).ToArray();


            IList<OutputTriggeredSIP> outputList = new List<OutputTriggeredSIP>();
            var SwitchAmount = Input.InitialInvestment * (Input.SwitchAmount / 100);
            //var SwitchAmount = Input.SwitchAmount;
            var MaxSwitch = Math.Round((Input.InitialInvestment / SwitchAmount), 0);

            var CurrentSwitch = 0;

            try
            {
                #region Calculate 1

                TriggerredIndexDetails lastIndexDetails = null;
                foreach (var item in IndexData)
                {
                    var sourceSchemeNav = GetLatestSchemeNav(item.Record_Date, Input.SourceSchemeId).Nav_Value;
                    var destinationSchemeNav = GetLatestSchemeNav(item.Record_Date, Input.DestinitionSchemeId).Nav_Value;

                    OutputTriggeredSIP objLastSIP = null;

                    if (outputList.Count > 0)
                        objLastSIP = outputList.Last();

                    if (item.Record_Date <= Input.ToDate)
                    {
                        if (outputList.Count == 0)
                        {
                            OutputTriggeredSIP objOutput = new OutputTriggeredSIP()
                            {
                                SourceNav = sourceSchemeNav,
                                DestinationNav = destinationSchemeNav,
                                Liquid = Input.InitialInvestment,
                                Liquid_Unit = Math.Round(Convert.ToDecimal((Input.InitialInvestment / sourceSchemeNav)), 6),
                                Edge = 0,
                                Edge_Unit = 0,
                                CalculateDate = item.Record_Date
                            };
                            objOutput.SIP_Nav = (objOutput.Liquid + objOutput.Edge);
                            outputList.Add(objOutput);
                            continue;
                        }

                        if (outputList.Count > 0)
                        {
                            OutputTriggeredSIP objOutput = new OutputTriggeredSIP() { };

                            //if (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && CurrentSwitch < MaxSwitch)
                            //{
                            //if (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6) > SwitchAmount)
                            //{
                            if (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6) > 0)
                            {
                                // Switch amount is deducted from liquid to edge on the day after the triggered is fired
                                // divided by that day's latest nav
                                objOutput.Liquid_Unit = objLastSIP.Liquid_Unit;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = objLastSIP.Edge_Unit;
                                objOutput.Edge = Math.Round(objOutput.Edge_Unit * destinationSchemeNav, 6);

                                if (Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6) <= SwitchAmount)
                                    SwitchAmount = Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6);

                                //if (CurrentSwitch + 1 == MaxSwitch)
                                //    SwitchAmount = Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Liquid_Unit = Math.Round((objOutput.Liquid - SwitchAmount) / sourceSchemeNav, 6);

                                // Transfer the switch amount from liquid to edge
                                objOutput.Edge = objOutput.Edge + SwitchAmount;
                                objOutput.Swithch_Amt = SwitchAmount;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = Math.Round((objOutput.Edge / destinationSchemeNav), 6);
                                // Sum Of Liquid & Edge Value
                                objOutput.SIP_Nav = objOutput.Liquid + objOutput.Edge;
                                objOutput.SIP_Return = Math.Round(((objOutput.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                objOutput.SourceNav = sourceSchemeNav;
                                objOutput.DestinationNav = destinationSchemeNav;
                                objOutput.IsTriggard = true;
                                CurrentSwitch++;
                            }
                            else
                            {
                                objOutput.Liquid_Unit = objLastSIP.Liquid_Unit;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = objLastSIP.Edge_Unit;
                                objOutput.Edge = Math.Round(objOutput.Edge_Unit * destinationSchemeNav, 6);
                                objOutput.SIP_Nav = objOutput.Liquid + objOutput.Edge;
                                objOutput.SIP_Return = Math.Round(((objOutput.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                objOutput.SourceNav = sourceSchemeNav;
                                objOutput.DestinationNav = destinationSchemeNav;
                                objOutput.IsTriggard = false;
                            }
                            objOutput.CalculateDate = item.Record_Date;
                            outputList.Add(objOutput);

                        }
                        lastIndexDetails = item;
                    }
                    else
                    {
                        //if (lastCalculatedSIP == null) lastCalculatedSIP = outputList.Last();

                        var calcDate = Input.ToDate;
                        var currentLastSIP = outputList.Last();
                        OutputTriggeredSIP objTrgOutput = new OutputTriggeredSIP()
                        {
                            //CalculateDate = calcDate,
                            CalculateDate = item.Record_Date,
                            SourceNav = sourceSchemeNav,
                            DestinationNav = destinationSchemeNav,
                            Edge_Unit = objLastSIP.Edge_Unit,
                            Liquid_Unit = objLastSIP.Liquid_Unit,
                            Edge = (destinationSchemeNav * objLastSIP.Edge_Unit),
                            Liquid = (sourceSchemeNav * objLastSIP.Liquid_Unit),
                        };
                        objTrgOutput.SIP_Nav = (objTrgOutput.Liquid + objTrgOutput.Edge);
                        objTrgOutput.SIP_Return = Math.Round((objTrgOutput.SIP_Nav / currentLastSIP.SIP_Nav) - 1, 4) * 100;
                        outputList.Add(objTrgOutput);
                    }
                }
                #endregion



                return outputList;
            }
            catch (Exception ex)
            {
                Msg = ex.Message.ToString();
                return null;
            }
        }

        public IList<OutputTriggeredSIP> CalculateTriggeredGSMSIP(EdelweissSIPInput Input, out string Msg)
        {
            Msg = string.Empty;
            var uniqeDt = DateList.ToList();
            var TrigIndexData = GetTriggeredIndexData();
            //var commonDateTime = GetCommonDateTime().ToList();
            //if (commonDateTime.First().Date > Input.FromDate) commonDateTime.Insert(0, Input.FromDate);
            //var uniqeDt = DateList.ToList();
            if (uniqeDt.First().Date > Input.FromDate) uniqeDt.Insert(0, Input.FromDate);
            var IndexData = (from indexData in TrigIndexData
                             join uniqueDate in uniqeDt on indexData.Record_Date equals uniqueDate
                             select indexData).ToArray();

            IList<OutputTriggeredSIP> outputList = new List<OutputTriggeredSIP>();

            var SwitchAmount = Input.InitialInvestment * (Input.SwitchAmount / 100);
            var MaxSwitch = Math.Round((Input.InitialInvestment / SwitchAmount), 0);

            var CurrentSwitch = 0;

            try
            {
                #region Calculate 1

                //TriggerredIndexDetails lastIndexDetails = null;
                foreach (var item in IndexData)
                {
                    var sourceSchemeNav = GetLatestSchemeNav(item.Record_Date, Input.SourceSchemeId).Nav_Value;
                    var destinationSchemeNav = GetLatestSchemeNav(item.Record_Date, Input.DestinitionSchemeId).Nav_Value;

                    OutputTriggeredSIP objLastSIP = null;
                    //OutputTriggeredSIP lastCalculatedSI=P = null;
                    if (outputList.Count > 0)
                        objLastSIP = outputList.Last();

                    if (item.Record_Date <= Input.ToDate)
                    {
                        if (outputList.Count == 0)
                        {
                            OutputTriggeredSIP objOutput = new OutputTriggeredSIP()
                            {
                                Liquid = Input.InitialInvestment,
                                Liquid_Unit = Math.Round(Convert.ToDecimal((Input.InitialInvestment / sourceSchemeNav)), 6),
                                Edge = 0,
                                Edge_Unit = 0,
                                CalculateDate = item.Record_Date
                            };
                            objOutput.SIP_Nav = (objOutput.Liquid + objOutput.Edge);
                            outputList.Add(objOutput);
                            continue;
                        }

                        if (outputList.Count > 0)
                        {
                            OutputTriggeredSIP objOutput = new OutputTriggeredSIP() { };

                            if (item.IsTriggerred == EdelweissTriggerType.PositiveTrigger && Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6) > 0)
                            {
                                // Switch amount is deducted from liquid to edge on the day after the triggered is fired
                                // divided by that day's latest nav
                                objOutput.Liquid_Unit = objLastSIP.Liquid_Unit;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = objLastSIP.Edge_Unit;
                                objOutput.Edge = Math.Round(objOutput.Edge_Unit * destinationSchemeNav, 6);

                                if (Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6) <= SwitchAmount)
                                    SwitchAmount = Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6);

                                //if (CurrentSwitch + 1 == MaxSwitch)
                                //    SwitchAmount = Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Liquid_Unit = Math.Round((objOutput.Liquid - SwitchAmount) / sourceSchemeNav, 6);

                                // Transfer the switch amount from liquid to edge
                                objOutput.Edge = objOutput.Edge + SwitchAmount;
                                objOutput.Swithch_Amt = SwitchAmount;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = Math.Round((objOutput.Edge / destinationSchemeNav), 6);
                                // Sum Of Liquid & Edge Value
                                objOutput.SIP_Nav = objOutput.Liquid + objOutput.Edge;
                                objOutput.SIP_Return = Math.Round(((objOutput.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                objOutput.SourceNav = sourceSchemeNav;
                                objOutput.DestinationNav = destinationSchemeNav;
                                objOutput.IsTriggard = true;
                                CurrentSwitch++;
                                if (objOutput.Liquid_Unit != 0)
                                    SwitchAmount = objOutput.Liquid * (Input.SwitchAmount / 100);
                            }
                            else
                            {
                                objOutput.Liquid_Unit = objLastSIP.Liquid_Unit;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = objLastSIP.Edge_Unit;
                                objOutput.Edge = Math.Round(objOutput.Edge_Unit * destinationSchemeNav, 6);
                                objOutput.SIP_Nav = objOutput.Liquid + objOutput.Edge;
                                objOutput.SIP_Return = Math.Round(((objOutput.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                objOutput.SourceNav = sourceSchemeNav;
                                objOutput.DestinationNav = destinationSchemeNav;
                                objOutput.IsTriggard = false;
                            }
                            objOutput.CalculateDate = item.Record_Date;
                            outputList.Add(objOutput);

                        }
                    }
                    else
                    {
                        //if (lastCalculatedSIP == null) lastCalculatedSIP = outputList.Last();
                        var calcDate = Input.ToDate;
                        var currentLastSIP = outputList.Last();
                        OutputTriggeredSIP objTrgOutput = new OutputTriggeredSIP()
                        {
                            //CalculateDate = calcDate,
                            CalculateDate = item.Record_Date,
                            SourceNav = sourceSchemeNav,
                            DestinationNav = destinationSchemeNav,
                            Edge_Unit = objLastSIP.Edge_Unit,
                            Liquid_Unit = objLastSIP.Liquid_Unit,
                            Edge = (destinationSchemeNav * objLastSIP.Edge_Unit),
                            Liquid = (sourceSchemeNav * objLastSIP.Liquid_Unit),
                        };
                        objTrgOutput.SIP_Nav = (objTrgOutput.Liquid + objTrgOutput.Edge);
                        objTrgOutput.SIP_Return = Math.Round((objTrgOutput.SIP_Nav / currentLastSIP.SIP_Nav) - 1, 6) * 100;
                        outputList.Add(objTrgOutput);
                    }
                }
                #endregion

                return outputList;
            }
            catch (Exception ex)
            {
                Msg = ex.Message.ToString();
                return null;
            }
        }

        public IList<OutputMonthlySIP> CalculateMonthlySIP(out string Msg)
        {
            Msg = string.Empty;

           // var SwitchAmount = Input.InitialInvestment * (Input.SwitchAmount / 100);
            var SwitchAmount = default(decimal);
            var SwitchAmount1 = default(decimal);  //Add hete 15.03.2018
            if (Input.SIPProcess == EdelweissProcess.PrepaidSTP)
            {
                var Amoount = Input.SwitchingRange.ToList().FirstOrDefault().Value;
                //SwitchAmount = Input.InitialInvestment * (Amoount / 100);
               // SwitchAmount = Amoount; //Add hete 15.03.2018
                SwitchAmount=Input.SwitchAmountForMonthlyStp;
            }
            if (Input.SIPProcess == EdelweissProcess.GainSwitchSIP)
            { 
                SwitchAmount= Input.InitialInvestment * (Input.SwitchAmount / 100);
                //SwitchAmount1 = Input.SwitchAmount; //Add hete 15.03.2018
            }
            IList<OutputMonthlySIP> OutResult = new List<OutputMonthlySIP>();
            OutputMonthlySIP objLastSIP = null;

            try
            {
                #region Calculation Between From And To Date

                foreach (var currentDate in DateList)
                {

                    OutputMonthlySIP lastCalculatedSIP = null; // This object holds the last triggared SIP Item
                    OutputMonthlySIP objSIPMonthly = null;
                    var latestSourceSchemeNav = GetLatestSchemeNav(currentDate, Input.SourceSchemeId).Nav_Value;
                    var latestDestinationSchemeNav = GetLatestSchemeNav(currentDate, Input.DestinitionSchemeId).Nav_Value;

                    if (currentDate <= Input.ToDate)
                    {
                        if (OutResult.Count > 0)
                            objLastSIP = OutResult.Last();

                        // First item of the collection
                        if (OutResult.Count == 0)
                        {
                            objSIPMonthly = new OutputMonthlySIP()
                            {
                                CalculateDate = currentDate,
                                Liquid = Input.InitialInvestment,
                                Edge = 0,
                                Edge_Unit = 0
                            };
                            objSIPMonthly.Liquid_Unit = Math.Round((objSIPMonthly.Liquid / latestSourceSchemeNav), 6);
                            objSIPMonthly.SIP_Nav = (objSIPMonthly.Liquid + objSIPMonthly.Edge);
                            objSIPMonthly.SIP_Return = 0M;
                        }
                        if (objLastSIP != null)
                        {
                            if (currentDate.Month != objLastSIP.CalculateDate.Month && objLastSIP.Liquid > 0)
                            {
                                if (objLastSIP.Liquid < SwitchAmount)
                                    SwitchAmount = objLastSIP.Liquid;
                                objSIPMonthly = new OutputMonthlySIP()
                                {
                                    CalculateDate = currentDate,
                                   // Liquid = (objLastSIP.Liquid - SwitchAmount), //replace SwitchAmount by SwitchAmount1  //Add hete 15.03.2018
                                   // Edge = (objLastSIP.Edge + SwitchAmount) //replace SwitchAmount by SwitchAmount1  //Add hete 15.03.2018
                                   //Change new ...23.03.2018
                                    Liquid = (Math.Round((objLastSIP.Liquid_Unit * latestSourceSchemeNav), 6) - SwitchAmount),
                                    Edge = (Math.Round((objLastSIP.Edge_Unit * latestDestinationSchemeNav), 6) + SwitchAmount)
                                };
                                objSIPMonthly.Liquid_Unit = Math.Round((objSIPMonthly.Liquid / latestSourceSchemeNav), 6);
                                objSIPMonthly.Edge_Unit = Math.Round((objSIPMonthly.Edge / latestDestinationSchemeNav), 6);
                                objSIPMonthly.SIP_Nav = (objSIPMonthly.Liquid + objSIPMonthly.Edge);
                                objSIPMonthly.SIP_Return = Math.Round(((objSIPMonthly.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                            }
                            else
                            {
                                objSIPMonthly = new OutputMonthlySIP()
                                {
                                    CalculateDate = currentDate,
                                    Liquid_Unit = objLastSIP.Liquid_Unit,
                                    Edge_Unit = objLastSIP.Edge_Unit
                                };
                                objSIPMonthly.Liquid = Math.Round((objSIPMonthly.Liquid_Unit * latestSourceSchemeNav), 6);
                                objSIPMonthly.Edge = Math.Round((objSIPMonthly.Edge_Unit * latestDestinationSchemeNav), 6);
                                objSIPMonthly.SIP_Nav = (objSIPMonthly.Liquid + objSIPMonthly.Edge);
                                objSIPMonthly.SIP_Return = Math.Round(((objSIPMonthly.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                            }
                        }
                    }
                    else
                    {
                        if (lastCalculatedSIP == null) lastCalculatedSIP = OutResult.Last();
                        var calcDate = Input.ToDate;

                        var currentLastSIP = OutResult.Last();

                        objSIPMonthly = new OutputMonthlySIP()
                        {
                            CalculateDate = currentDate,
                            SourceNav = latestSourceSchemeNav,
                            DestinationNav = latestDestinationSchemeNav,
                            Edge_Unit = lastCalculatedSIP.Edge_Unit,
                            Liquid_Unit = lastCalculatedSIP.Liquid_Unit,
                            Edge = (latestDestinationSchemeNav * lastCalculatedSIP.Edge_Unit),
                            Liquid = (latestSourceSchemeNav * lastCalculatedSIP.Liquid_Unit),
                        };
                        objSIPMonthly.SIP_Nav = (objSIPMonthly.Liquid + objSIPMonthly.Edge);
                        objSIPMonthly.SIP_Return = Math.Round((objSIPMonthly.SIP_Nav / currentLastSIP.SIP_Nav) - 1, 6) * 100;
                    }
                    OutResult.Add(objSIPMonthly);
                }

                #endregion

                return OutResult;
            }
            catch (Exception ex)
            {
                Msg = ex.Message.ToString();
                return null;
            }
        }
        public IList<OutputMonthlySIP> CalculateMonthlySimpleSIP(out string Msg)
        {
           // _DtSimpleSip = _DtSimpleSip.Where(w => w != _DtSimpleSip[0]).ToArray(); 
            Msg = string.Empty;
            var SwitchAmount = Input.InitialInvestment * (Input.SwitchAmount / 100);
            IList<OutputMonthlySIP> OutResult = new List<OutputMonthlySIP>();
            OutputMonthlySIP objLastSIP = null;           
            try
            {
                #region Calculation Between From And To Date
                var _FirstDate = DateList.FirstOrDefault();
                var FirstCalDate = _FirstDate.Day == 1 ? _FirstDate : _FirstDate.AddMonths(1).AddDays(-_FirstDate.Day).AddDays(1);
                var _DT4Sip = navData.Where(x => x.SchemeId == Input.DestinitionSchemeId).ToArray();
                //foreach (var currentDate in DateList)
                foreach (var _DT in _DT4Sip)
                {
                    var currentDate = _DT.NavDate;
                    if (currentDate < FirstCalDate)
                        continue;
                    OutputMonthlySIP lastCalculatedSIP = null; // This object holds the last triggared SIP Item
                    OutputMonthlySIP objSIPMonthly = null;
                    // var latestSourceSchemeNav = GetLatestSchemeNav(currentDate, Input.SourceSchemeId).Nav_Value;
                    //var latestDestinationSchemeNav = GetLatestSchemeNav(currentDate, Input.DestinitionSchemeId).Nav_Value;
                    var latestDestinationSchemeNav = _DT.NavValue;


                    if (currentDate <= Input.ToDate)
                    {
                        if (OutResult.Count > 0)
                            objLastSIP = OutResult.Last();
                        //if (_DtSimpleSip.Any(x => x.Date == currentDate.Date))
                        //    objLastSIP.Liquid = objLastSIP.Liquid + Input.InitialInvestment;
                        // First item of the collection
                        if (OutResult.Count == 0)
                        {
                            objSIPMonthly = new OutputMonthlySIP()
                            {
                                CalculateDate = currentDate,
                                Liquid = Input.InitialInvestment,
                                Edge = 0,
                                Edge_Unit = 0
                            };
                            objSIPMonthly.Liquid_Unit = Math.Round((objSIPMonthly.Liquid / latestDestinationSchemeNav), 6);
                            objSIPMonthly.SIP_Nav = (objSIPMonthly.Liquid + objSIPMonthly.Edge);
                            objSIPMonthly.SIP_Return = 0M;
                            objSIPMonthly.IsMonthlyDeduct = true;
                        }
                        if (objLastSIP != null)
                        {
                            if (currentDate.Month != objLastSIP.CalculateDate.Month && objLastSIP.Liquid > 0)
                            {
                                if (objLastSIP.Liquid < SwitchAmount)
                                    SwitchAmount = objLastSIP.Liquid;

                                objSIPMonthly = new OutputMonthlySIP()
                                {
                                    CalculateDate = currentDate,
                                    //Add new here instead below 15.03.2018
                                    Liquid = (Math.Round((objLastSIP.Liquid_Unit * latestDestinationSchemeNav), 6) + Input.InitialInvestment + SwitchAmount),
                                    //End
                                    //Liquid = (objLastSIP.Liquid+Input.InitialInvestment + SwitchAmount),
                                    Edge = 0
                                };
                                objSIPMonthly.Liquid_Unit = Math.Round((objSIPMonthly.Liquid / latestDestinationSchemeNav), 6);
                                objSIPMonthly.Edge_Unit = 0;
                                objSIPMonthly.SIP_Nav = (objSIPMonthly.Liquid);
                                objSIPMonthly.SIP_Return = Math.Round((((objSIPMonthly.SIP_Nav / objSIPMonthly.Liquid_Unit) / (objLastSIP.SIP_Nav / objLastSIP.Liquid_Unit)) - 1) * 100, 6);
                                //Add new here 15.03.2018
                                objSIPMonthly.IsMonthlyDeduct = true;
                               // var MonthlyTransaction = OutResult.Where(x => x.IsMonthlyDeduct);
                               // if (MonthlyTransaction.Any())
                               // {
                               //     var lastMonthTransaction = MonthlyTransaction.Last().SIP_Nav;
                               //     if (lastMonthTransaction>0)
                               //         objSIPMonthly.Sip_Return_New = Math.Round((((objSIPMonthly.SIP_Nav - Input.InitialInvestment) / lastMonthTransaction) - 1) * 100, 6);
                               // }
                                if (objLastSIP.SIP_Nav > 0)
                                    objSIPMonthly.Sip_Return_New = Math.Round((((objSIPMonthly.SIP_Nav - Input.InitialInvestment) / objLastSIP.SIP_Nav) - 1) * 100, 6);
                            }
                            else
                            {
                                objSIPMonthly = new OutputMonthlySIP()
                                {
                                    CalculateDate = currentDate,
                                    Liquid_Unit = objLastSIP.Liquid_Unit,
                                    Edge_Unit = 0
                                };
                                objSIPMonthly.Liquid = Math.Round((objSIPMonthly.Liquid_Unit * latestDestinationSchemeNav), 6);
                                objSIPMonthly.Edge = 0;
                                objSIPMonthly.SIP_Nav = (objSIPMonthly.Liquid);
                                objSIPMonthly.SIP_Return = Math.Round(((objSIPMonthly.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                //objSIPMonthly.Sip_Return_New = Math.Round((((objSIPMonthly.SIP_Nav - Input.InitialInvestment) / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                objSIPMonthly.Sip_Return_New = objSIPMonthly.SIP_Return; //Add new here 15.03.2018
                            }
                        }
                    }
                    else
                    {
                        if (lastCalculatedSIP == null) lastCalculatedSIP = OutResult.Last();
                        var calcDate = Input.ToDate;

                        var currentLastSIP = OutResult.Last();

                        objSIPMonthly = new OutputMonthlySIP()
                        {
                            CalculateDate = currentDate,
                            SourceNav = latestDestinationSchemeNav,
                            DestinationNav = latestDestinationSchemeNav,
                            Edge_Unit = 0,
                            Liquid_Unit = lastCalculatedSIP.Liquid_Unit,
                            Edge = 0,
                            Liquid = (latestDestinationSchemeNav * lastCalculatedSIP.Liquid_Unit),
                        };
                        objSIPMonthly.SIP_Nav = (objSIPMonthly.Liquid);
                        objSIPMonthly.SIP_Return = Math.Round((objSIPMonthly.SIP_Nav / currentLastSIP.SIP_Nav) - 1, 6) * 100;
                        objSIPMonthly.Sip_Return_New = objSIPMonthly.SIP_Return; //Add new here 15.03.2018
                    }
                    OutResult.Add(objSIPMonthly);
                }

                #endregion

                return OutResult;
            }
            catch (Exception ex)
            {
                Msg = ex.Message.ToString();
                return null;
            }
        }

        private class LatestSchemeNav
        {
            public DateTime Nav_Date { get; set; }
            public decimal Nav_Value { get; set; }
        }

        private class LatestIndexRecords : LatestSchemeNav { }

        private class LatestAdditionalIndexRecords : LatestSchemeNav { }

        private LatestSchemeNav GetLatestSchemeNav(DateTime RecordDate, int SchemeId)
        {
            SchemesNav latestNav = null;

            latestNav = navData.Where(p => p.SchemeId == SchemeId &&
             p.NavDate.Date >= RecordDate.Date).OrderBy(p => p.NavDate).FirstOrDefault();

            if (latestNav != null)
                return new LatestSchemeNav() { Nav_Date = latestNav.NavDate, Nav_Value = latestNav.NavValue };
            else
            {
                latestNav = navData.Where(p => p.SchemeId == SchemeId && p.NavDate.Date <= RecordDate.Date).OrderByDescending(p => p.NavDate).FirstOrDefault();
                return new LatestSchemeNav() { Nav_Date = latestNav.NavDate, Nav_Value = latestNav.NavValue };
            }
        }

        public IList<BasicSIP> CalculateSIPReturn(int Scheme_Id)
        {
            var Current_Date = Input.FromDate;
            var Current_Nav = GetLatestSchemeNav(Current_Date, Scheme_Id);
            var LiquidUnit = Math.Round(Input.InitialInvestment / Current_Nav.Nav_Value, 6);
            IList<BasicSIP> OutPut = new List<BasicSIP>();
            BasicSIP objBasicSIP = null;
            BasicSIP lastSIP = null;

            try
            {
                while (Current_Date <= Input.CurrentDate)
                {
                    if (OutPut.Count > 0)
                        lastSIP = OutPut.Last();

                    if (Current_Date == Input.FromDate)
                    {
                        objBasicSIP = new BasicSIP()
                        {
                            SIP_Date = Current_Date,
                            SIP_Nav = Input.InitialInvestment,
                            SIP_Return = 0M,
                            SIP_Unit = LiquidUnit,
                            Scheme_Nav = Current_Nav.Nav_Value
                        };
                    }
                    else
                    {
                        if (lastSIP != null)
                        {
                            Current_Nav = GetLatestSchemeNav(Current_Date, Scheme_Id);
                            objBasicSIP = new BasicSIP()
                            {
                                SIP_Date = Current_Nav.Nav_Date,
                                SIP_Nav = Math.Round((Current_Nav.Nav_Value * LiquidUnit), 6),
                                SIP_Unit = LiquidUnit,
                                Scheme_Nav = Current_Nav.Nav_Value
                            };
                            objBasicSIP.SIP_Return = Math.Round(((objBasicSIP.SIP_Nav / lastSIP.SIP_Nav) - 1) * 100, 6);
                        }
                    }
                    OutPut.Add(objBasicSIP);
                    Current_Date = Current_Date.AddDays(1);
                }
                return OutPut;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<BasicSIP> CalculateBenchmarkSIP(IndexData[] IndexVal)
        {
            var IndexRecords = IndexVal.Where(p => p.Index_Date >= Input.FromDate).OrderBy(p => p.Index_Date);
            var currentIndexVal = IndexRecords.FirstOrDefault();
            IList<BasicSIP> OutPut = new List<BasicSIP>();
            BasicSIP objBasicSIP = null;
            BasicSIP lastSIP = null;
            try
            {
                if (currentIndexVal != null)
                {
                    var currentIndex = currentIndexVal.Index_Val;
                    var currentUnit = Convert.ToDecimal(Input.InitialInvestment / currentIndex);

                    foreach (var item in IndexRecords)
                    {
                        if (OutPut.Count == 0)
                        {
                            objBasicSIP = new BasicSIP()
                            {
                                Scheme_Nav = Convert.ToDecimal(item.Index_Val),
                                SIP_Date = item.Index_Date,
                                SIP_Return = 0M,
                                SIP_Unit = currentUnit,
                                SIP_Nav = Input.InitialInvestment
                            };
                        }
                        else
                        {
                            objBasicSIP = new BasicSIP()
                            {
                                Scheme_Nav = Convert.ToDecimal(item.Index_Val),
                                SIP_Date = item.Index_Date,
                                SIP_Unit = currentUnit,
                                SIP_Nav = Math.Round(Convert.ToDecimal(item.Index_Val) * currentUnit, 6)
                            };
                            objBasicSIP.SIP_Return = Math.Round(((objBasicSIP.SIP_Nav / lastSIP.SIP_Nav) - 1) * 100, 6);
                        }

                        OutPut.Add(objBasicSIP);

                        if (OutPut.Count > 0)
                            lastSIP = OutPut.Last();
                    }
                }
                return OutPut;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public double CalculateVolatility(decimal[] Values)
        {
            var average = Values.Average();
            var sumOfSquaresofDifference = Values.Select(p => (p - average) * (p - average)).Sum();
            //var sd = Math.Sqrt(Convert.ToDouble(sumOfSquaresofDifference / Values.Length));
            var sd1 = StandardDev(Values);
            return Math.Round((sd1 * Math.Sqrt(252)), 2);
        }

        private double Variance(IEnumerable<decimal> source)
        {
            int n = 0;
            double mean = 0;
            double M2 = 0;

            foreach (double x in source)
            {
                n = n + 1;
                double delta = x - mean;
                mean = mean + delta / n;
                M2 += delta * (x - mean);
            }
            return M2 / (n - 1);
        }

        private double StandardDev(IEnumerable<decimal> source)
        {
            return Math.Sqrt(Variance(source));
        }

        public double StandardDeviation(decimal[] valueList)
        {
            double M = 0.0;
            double S = 0.0;
            int k = 1;
            foreach (double value in valueList)
            {
                double tmpM = M;
                M += (value - tmpM) / k;
                S += (value - tmpM) * (value - M);
                k++;
            }
            return Math.Sqrt(S / (k - 2));
        }

        public decimal CalculateDrawdown(decimal[] Values)
        {
            if (Values.Length > 0)
            {
                var firstVal = (decimal)Values.GetValue(0);
                IList<decimal> drawdownList = new List<decimal>();
                for (int i = 1; i < Values.Count(); i++)
                {
                    var calcMax = Values.Take(i).Max();// Math.Max(firstVal, (decimal)Values.GetValue(i));
                    drawdownList.Add(Math.Round((((decimal)Values.GetValue(i - 1) - calcMax) / calcMax) * 100, 2));
                }
                return drawdownList.Min();
            }
            return 0M;
        }

        public decimal CalculateProfit(decimal BaseValue, decimal GainValue)
        {
            return Math.Round(((GainValue - BaseValue) / BaseValue) * 100, 4);
        }

        public EdelweissSchemes[] GetEdelweissSchemes(EdelweissSchemes[] InputSchemes)
        {
            try
            {
                using (var db = new EdelweissSIPDataContext())
                {
                    db.CommandTimeout = 20000;
                    var Fund_Id = InputSchemes.Select(p => new { Fund_Id = p.Fund_Id }).ToArray();
                    var allSchemes = (from sm in db.GetTable<T_SCHEMES_MASTER>()
                                      join fm in db.GetTable<T_FUND_MASTER>() on sm.Fund_Id equals fm.FUND_ID
                                      //join ischeme in InputSchemes on fm.FUND_NAME equals ischeme.Fund_Name
                                      where sm.Nav_Check.Value == 3 && fm.MUTUALFUND_ID == 55 // MutFund ID Of Edelweiss
                                      && sm.Option_Id == 2 && sm.Launch_Date != null
                                      select new
                                      {
                                          Fund_Name = fm.FUND_NAME,
                                          Scheme_ID = sm.Scheme_Id,
                                          Fund_Id = fm.FUND_ID,
                                          Scheme_Name = sm.Sch_Short_Name,
                                          InceptionDt = sm.Launch_Date.Value,
                                          AMFI_Code = sm.Amfi_Code,
                                          Scheme_Code = sm.Scheme_Code
                                      }).ToArray();


                    return (from scheme in allSchemes
                            join input in InputSchemes on scheme.Fund_Id equals input.Fund_Id
                            select new EdelweissSchemes()
                            {
                                Fund_Name = scheme.Fund_Name,
                                Fund_Type = input.Fund_Type,
                                Scheme_ID = scheme.Scheme_ID,
                                Scheme_Name = scheme.Scheme_Name + " (" + MakeSchemeShortForm(scheme.Scheme_Name) + ")",
                                SchemeInception = scheme.InceptionDt.Date,
                                Scheme_Code = scheme.Scheme_Code,
                                AMFI_Code = scheme.AMFI_Code,
                            }).OrderBy(p => p.Scheme_Name).ToArray();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        

        public string MakeSchemeShortForm(string SchemeName)
        {
            var NameArray = SchemeName.Replace("Growth", "").Replace("-", "").Split(' ');
            StringBuilder sb = new StringBuilder("");
            foreach (var element in NameArray)
            {
                if (element.Length > 0)
                    sb.Append(element.Substring(0, 1));
            }
            return sb.ToString().Replace("(", "");
        }
        private IList<EdelweissIndexReturn> IndexReturnData4TRI()
        {
            var CalculatingDate = DateList;
            IList<EdelweissIndexReturn> Indexreturns = new List<EdelweissIndexReturn>();
            decimal calcReturn = 0M;
            try
            {
                using (var db = new EdelweissSIPDataContext())
                {

                    var IndexRecords = (from data in IndexValWithTRI
                                        join calDate in CalculatingDate on data.Index_Date.Date equals calDate.Date
                                        select data).OrderBy(p => p.Index_Date).ToArray();
                    EdelweissIndexReturn objReturn = null;

                    for (int i = 0; i < IndexRecords.Length; i++)
                    {
                        if (i == 0)
                        {
                        }
                        if (i > 0)
                        {
                            calcReturn = Math.Round((((decimal)IndexRecords[i].Index_Val / (decimal)IndexRecords[i - 1].Index_Val) - 1) * 100, 6);

                        }
                        objReturn = new EdelweissIndexReturn()
                        {
                            INDEX_ID = IndexRecords[i].Index_ID,
                            INDEX_VALUE = calcReturn,
                            RECORD_DATE = IndexRecords[i].Index_Date
                        };
                        if (objReturn != null)
                            Indexreturns.Add(objReturn);
                    }

                }
                return Indexreturns;
            }
            catch (Exception ex)
            {
                return Indexreturns;
            }
        }
        private IList<EdelweissIndexReturn> IndexReturnData()
        {
            var CalculatingDate = DateList;
            IList<EdelweissIndexReturn> Indexreturns = new List<EdelweissIndexReturn>();
            decimal calcReturn = 0M;
            try
            {
                using (var db = new EdelweissSIPDataContext())
                {
                 

                    var IndexRecords = (from data in IndexVal
                                        join calDate in CalculatingDate on data.Index_Date.Date equals calDate.Date
                                        select data).OrderBy(p => p.Index_Date).ToArray();
                    EdelweissIndexReturn objReturn = null;

                    for (int i = 0; i < IndexRecords.Length; i++)
                    {
                        if (i == 0)
                        {
                            calcReturn = Math.Round((((decimal)IndexRecords[i].Index_Val / (decimal)IndexValPrevious.Index_Val) - 1) * 100, 6);
                        }
                        if (i > 0)
                        {
                            calcReturn = Math.Round((((decimal)IndexRecords[i].Index_Val / (decimal)IndexRecords[i - 1].Index_Val) - 1) * 100, 6);

                        }
                        objReturn = new EdelweissIndexReturn()
                        {
                            INDEX_ID = IndexRecords[i].Index_ID,
                            INDEX_VALUE = calcReturn,
                            RECORD_DATE = IndexRecords[i].Index_Date
                        };
                        if (objReturn != null)
                            Indexreturns.Add(objReturn);
                    }

                }
                return Indexreturns;
            }
            catch (Exception ex)
            {
                return Indexreturns;
            }
        }
        private IList<EdelweissSchemeReturn> SchemeReturnData()
        {
            var CalculatingDate = DateList;
            IList<EdelweissSchemeReturn> SchemeReturn = new List<EdelweissSchemeReturn>();
            decimal calcReturn = 0M;
            try
            {
                using (var db = new EdelweissSIPDataContext())
                {

                    var Sourceschemedata = navData.Where(p => p.SchemeId == Input.SourceSchemeId);
                    var SchemeRecords = (from data in Sourceschemedata
                                         join calDate in CalculatingDate on data.NavDate.Date equals calDate.Date
                                         select data).OrderBy(p => p.NavDate).ToArray();
                    EdelweissSchemeReturn objReturn = null;
                    int j = 0;
                    for (int i = 0; i < SchemeRecords.Length; i++)
                    {

                        if (i > 0)
                        {
                            calcReturn = Math.Round((((decimal)SchemeRecords[i].NavValue / (decimal)SchemeRecords[j].NavValue) - 1) * 100, 6);
                            if (calcReturn > Input.TriggerCutOff)
                                j = i;
                        }
                        objReturn = new EdelweissSchemeReturn()
                        {
                            Scheme_id = SchemeRecords[i].SchemeId,
                            Nav_Value = calcReturn,
                            RECORD_DATE = SchemeRecords[i].NavDate
                        };
                        if (objReturn != null)
                            SchemeReturn.Add(objReturn);
                    }

                }
                return SchemeReturn;
            }
            catch (Exception ex)
            {
                return SchemeReturn;
            }
        }

        private IndexData[] _additionalIndexVal;
        public IndexData[] AdditionalIndexVal
        {
            get
            {
                if (Input != null)
                    if (_additionalIndexVal == null)
                        _additionalIndexVal = GetAdditionalIndexData();
                return _additionalIndexVal;
            }
            set { _additionalIndexVal = value; }
        }

        private IndexData[] GetAdditionalIndexData()
        {
            using (var db = new PrincipalCalcDataContext())
            {
                var IndexId = db.T_INDEX_MASTERs.Where(p => p.INDEX_NAME == Input.AdditionalBenchmark).FirstOrDefault().INDEX_ID;
                return (from tbl in db.T_INDEX_RECORDs
                        where tbl.RECORD_DATE >= Input.FromDate && tbl.RECORD_DATE <= Input.CurrentDate
                        && tbl.INDEX_ID == IndexId
                        select new IndexData()
                        {
                            Index_ID = (int)tbl.INDEX_ID,
                            Index_Date = tbl.RECORD_DATE,
                            Index_Val = tbl.INDEX_VALUE
                        }).OrderBy(p => p.Index_Date).ToArray();

            }
        }


        #endregion

        #region Edelweiss SIP Plus

        private decimal? GetIndexID(string IndexName)
        {
            using (var db = new PrincipalCalcDataContext())
            {
                var Index = db.T_INDEX_MASTERs.Where(p => p.INDEX_NAME.ToUpper() == IndexName.ToUpper()).FirstOrDefault();
                return Index == null ? default(decimal?) : Index.INDEX_ID;
            }
        }

        private List<SIPPlusMonthlyDeductionCount> TransCount = null;

        [Obsolete]
        public OutputTriggeredSIP[] CalculateSIPPlusOld(EdelweissSIPInput Input, out decimal TotalInvestedAmount, out string strError)
        {
            strError = string.Empty;
            TotalInvestedAmount = default(decimal);
            TransCount = new List<SIPPlusMonthlyDeductionCount>();
            var uniqeDt = DateList.ToList();
            var TrigIndexData = GetTriggeredIndexData();
            if (uniqeDt.First().Date > Input.FromDate) uniqeDt.Insert(0, Input.FromDate);

            var IndexData = (from indexData in TrigIndexData
                             join uniqueDate in uniqeDt on indexData.Record_Date equals uniqueDate
                             select indexData).ToArray();


            IList<OutputSIPPlus> outputList = new List<OutputSIPPlus>();
            var SwitchAmount = Input.SwitchAmtSIPPlus;
            decimal TotalSwitchedAmt = 0;

            try
            {
                #region Calculate 1

                TriggerredIndexDetails lastIndexDetails = null;
                foreach (var item in IndexData)
                {
                    var destinationSchemeNav = GetLatestSchemeNav(item.Record_Date, Input.DestinitionSchemeId).Nav_Value;

                    OutputSIPPlus objLastSIP = null;

                    if (outputList.Count > 0)
                        objLastSIP = outputList.Last();

                    if (item.Record_Date <= Input.ToDate)
                    {
                        if (outputList.Count == 0)
                        {
                            OutputSIPPlus objOutput = new OutputSIPPlus()
                            {
                                DestinationSchemeNav = destinationSchemeNav,
                                SIPUnit = 0,
                                TransferredAmt = 0,
                                CalculatingDate = item.Record_Date,
                                CurrentValuation = 0
                            };
                            outputList.Add(objOutput);
                            continue;
                        }

                        if (outputList.Count > 0)
                        {
                            OutputSIPPlus objOutput = new OutputSIPPlus() { };

                            var curTransactionCount = TransCount.Where(p => p.Month == item.Record_Date.Month && p.Year == item.Record_Date.Year).ToArray().Count();

                            if (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && curTransactionCount < Input.NoOfSwitchPerMonth)
                            {
                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.SIPUnit = objOutput.SIPUnit + Math.Round((SwitchAmount / destinationSchemeNav), 6);
                                objOutput.CurrentValuation = Math.Round(objOutput.SIPUnit * destinationSchemeNav, 6);
                                TotalSwitchedAmt = TotalSwitchedAmt + SwitchAmount;

                                objOutput.IsTriggard = true;
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);
                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = TotalSwitchedAmt;
                                TransCount.Add(new SIPPlusMonthlyDeductionCount() { Month = item.Record_Date.Month, Year = item.Record_Date.Year });
                            }
                            else
                            {
                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.CurrentValuation = Math.Round((objOutput.SIPUnit * destinationSchemeNav), 6);
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);

                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = objLastSIP.InvestedAmt;
                                objOutput.IsTriggard = false;
                                objOutput.IsTriggard = false;

                            }
                            objOutput.CalculatingDate = item.Record_Date;
                            outputList.Add(objOutput);

                        }
                        lastIndexDetails = item;
                    }
                    else
                    {
                        var calcDate = Input.ToDate;
                        var currentLastSIP = outputList.Last();
                        OutputSIPPlus objTrgOutput = new OutputSIPPlus()
                        {
                            CalculatingDate = item.Record_Date,
                            DestinationSchemeNav = destinationSchemeNav,
                            SIPUnit = objLastSIP.SIPUnit,
                            CurrentValuation = objLastSIP.SIPUnit * destinationSchemeNav,
                            InvestedAmt = TotalSwitchedAmt,
                            IsTriggard = false
                        };
                        outputList.Add(objTrgOutput);
                    }
                }
                #endregion

                // Return The Amount Invested Logic For SIP Plus Only

                TotalInvestedAmount = TransCount.Count * SwitchAmount;

                return outputList.Select(p => new OutputTriggeredSIP
                {
                    CalculateDate = p.CalculatingDate,
                    Edge_Unit = p.SIPUnit,
                    IsTriggard = p.IsTriggard,
                    Edge = p.CurrentValuation.Value,
                    SIP_Return = p.SIPReturn,
                    SIP_Nav = p.CurrentValuation.Value,
                    Swithch_Amt = p.TransferredAmt,
                    DestinationNav = p.DestinationSchemeNav,
                    Liquid = p.InvestedAmt
                }).ToArray();
            }
            catch (Exception ex)
            {
                strError = ex.Message.ToString();
                return null;
            }
        }

        public OutputTriggeredSIP[] CalculateSIPPlus(EdelweissSIPInput Input, out decimal TotalInvestedAmount, out string strError)
        {
            strError = string.Empty;
            TotalInvestedAmount = default(decimal);
            TransCount = new List<SIPPlusMonthlyDeductionCount>();
            var uniqeDt = DateList.ToList();
            var TrigIndexData = GetTriggeredIndexData();

            var chk = TrigIndexData.Where(c => c.IsTriggerred != EdelweissTriggerType.NoTrigger).ToArray();

            if (uniqeDt.First().Date > Input.FromDate) uniqeDt.Insert(0, Input.FromDate);

            var IndexData = (from indexData in TrigIndexData
                             join uniqueDate in uniqeDt on indexData.Record_Date equals uniqueDate
                             select indexData).ToArray();


            IList<OutputSIPPlus> outputList = new List<OutputSIPPlus>();
            var SwitchAmount = Input.SwitchAmtSIPPlus;
            decimal TotalSwitchedAmt = 0;

            try
            {
                #region Calculate 1

                TriggerredIndexDetails lastIndexDetails = null;
                foreach (var item in IndexData)
                {
                    var destinationSchemeNav = GetLatestSchemeNav(item.Record_Date, Input.DestinitionSchemeId).Nav_Value;

                    OutputSIPPlus objLastSIP = null;

                    if (outputList.Count > 0)
                        objLastSIP = outputList.Last();

                    if (item.Record_Date <= Input.ToDate)
                    {
                        if (outputList.Count == 0)
                        {




                            var curTransactionCount = TransCount.Where(p => p.Month == item.Record_Date.Month && p.Year == item.Record_Date.Year).ToArray().Count();

                            if ((item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && (Input.NoOfSwitchPerMonth != 0 && curTransactionCount < Input.NoOfSwitchPerMonth)) ||
                            (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Input.NoOfSwitchPerMonth == 0))
                            {
                                OutputSIPPlus objOutput = new OutputSIPPlus() { };
                                var calSwitch = Input.SwitchingRange.Where(p => item.Index_Value <= p.Key).ToList().OrderBy(p => p.Key).FirstOrDefault().Value;
                                objOutput.TransferredAmt = calSwitch;
                                //objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.SIPUnit = objOutput.SIPUnit + (calSwitch / destinationSchemeNav);
                                objOutput.CurrentValuation = Math.Round(objOutput.SIPUnit * destinationSchemeNav, 6);
                                TotalSwitchedAmt = TotalSwitchedAmt + calSwitch;
                                TotalInvestedAmount = TotalInvestedAmount + calSwitch;
                                objOutput.IsTriggard = true;
                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = TotalSwitchedAmt;
                                TransCount.Add(new SIPPlusMonthlyDeductionCount() { Month = item.Record_Date.Month, Year = item.Record_Date.Year });
                                objOutput.CalculatingDate = item.Record_Date;
                                outputList.Add(objOutput);
                                continue;
                            }
                            else
                            {


                                OutputSIPPlus objOutput = new OutputSIPPlus()
                                {
                                    DestinationSchemeNav = destinationSchemeNav,
                                    SIPUnit = 0,
                                    TransferredAmt = 0,
                                    CalculatingDate = item.Record_Date,
                                    CurrentValuation = 0
                                };
                                outputList.Add(objOutput);
                                continue;
                            }
                        }

                        if (outputList.Count > 0)
                        {
                            OutputSIPPlus objOutput = new OutputSIPPlus() { };

                            var curTransactionCount = TransCount.Where(p => p.Month == item.Record_Date.Month && p.Year == item.Record_Date.Year).ToArray().Count();

                            if ((item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && (Input.NoOfSwitchPerMonth != 0 && curTransactionCount < Input.NoOfSwitchPerMonth)) ||
                            (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Input.NoOfSwitchPerMonth == 0))
                            {

                                // Calculate Switching Amount Based On The Range Selected

                                var calSwitch = Input.SwitchingRange.Where(p => item.Index_Value <= p.Key).ToList().OrderBy(p => p.Key).FirstOrDefault().Value;

                                objOutput.TransferredAmt = calSwitch;
                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.SIPUnit = objOutput.SIPUnit + (calSwitch / destinationSchemeNav);
                                objOutput.CurrentValuation = Math.Round(objOutput.SIPUnit * destinationSchemeNav, 6);
                                TotalSwitchedAmt = TotalSwitchedAmt + calSwitch;

                                TotalInvestedAmount = TotalInvestedAmount + calSwitch;

                                objOutput.IsTriggard = true;
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);
                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = TotalSwitchedAmt;
                                TransCount.Add(new SIPPlusMonthlyDeductionCount() { Month = item.Record_Date.Month, Year = item.Record_Date.Year });
                                //Add new 15.03.2018
                              // var LastTriggerAmt = outputList.Where(x => x.IsTriggard);
                              // if (LastTriggerAmt.Any())
                              // {
                              //     var LastCurrentvalueAmtwithTrigger = LastTriggerAmt.Last().CurrentValuation;
                              //     if (LastCurrentvalueAmtwithTrigger > 0)
                              //         objOutput.SIPReturn_New = Math.Round((((objOutput.CurrentValuation.Value - calSwitch) / LastCurrentvalueAmtwithTrigger.Value) - 1) * 100, 6);
                              // }
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn_New = Math.Round((((objOutput.CurrentValuation.Value - calSwitch) / objLastSIP.CurrentValuation.Value) - 1) * 100, 9);
                                //end
                            }
                            else
                            {
                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.CurrentValuation = Math.Round((objOutput.SIPUnit * destinationSchemeNav), 6);
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 9);

                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = objLastSIP.InvestedAmt;
                                objOutput.IsTriggard = false;
                                objOutput.IsTriggard = false;
                                objOutput.SIPReturn_New = objOutput.SIPReturn; //Add new 15.03.2018
                            }
                            objOutput.CalculatingDate = item.Record_Date;
                            outputList.Add(objOutput);

                        }
                        lastIndexDetails = item;
                    }
                    else
                    {
                        var calcDate = Input.ToDate;
                        var currentLastSIP = outputList.Last();
                        OutputSIPPlus objTrgOutput = new OutputSIPPlus()
                        {
                            CalculatingDate = item.Record_Date,
                            DestinationSchemeNav = destinationSchemeNav,
                            SIPUnit = objLastSIP.SIPUnit,
                            CurrentValuation = objLastSIP.SIPUnit * destinationSchemeNav,
                            InvestedAmt = TotalSwitchedAmt,
                            IsTriggard = false,
                            SIPReturn_New = Math.Round((((objLastSIP.SIPUnit * destinationSchemeNav) / objLastSIP.CurrentValuation.Value) - 1) * 100, 9)//Add new 15.03.2018
                        };                       
                        outputList.Add(objTrgOutput);
                    }
                }
                #endregion

                // Return The Amount Invested Logic For SIP Plus Only

                //TotalInvestedAmount = TransCount.Count * SwitchAmount;

                return outputList.Select(p => new OutputTriggeredSIP
                {
                    CalculateDate = p.CalculatingDate,
                    Edge_Unit = p.SIPUnit,
                    IsTriggard = p.IsTriggard,
                    Edge = p.CurrentValuation.Value,
                    SIP_Return = p.SIPReturn,
                    SIP_Nav = p.CurrentValuation.Value,
                    Swithch_Amt = p.TransferredAmt,
                    DestinationNav = p.DestinationSchemeNav,
                    Liquid = p.InvestedAmt,
                    Sip_Return_New=p.SIPReturn_New
                }).ToArray();
            }
            catch (Exception ex)
            {
                strError = ex.Message.ToString();
                return null;
            }
        }

        public IList<OutputMonthlySIP> CalculateMonthlySIPPlus(out string Msg)
        {
            Msg = string.Empty;
            var SwitchAmount = Input.SwitchAmtSIPPlus;
            IList<OutputMonthlySIP> OutResult = new List<OutputMonthlySIP>();
            OutputMonthlySIP objLastSIP = null;

            try
            {
                #region Calculation Between From And To Date

                foreach (var currentDate in DateList)
                {

                    OutputMonthlySIP lastCalculatedSIP = null; // This object holds the last triggared SIP Item
                    OutputMonthlySIP objSIPMonthly = null;
                    var latestDestinationSchemeNav = GetLatestSchemeNav(currentDate, Input.DestinitionSchemeId).Nav_Value;

                    if (currentDate <= Input.ToDate)
                    {
                        if (OutResult.Count > 0)
                            objLastSIP = OutResult.Last();

                        // First item of the collection
                        if (OutResult.Count == 0)
                        {
                            objSIPMonthly = new OutputMonthlySIP()
                            {
                                CalculateDate = currentDate,
                                Liquid = Input.InitialInvestment,
                                Edge = 0,
                                Edge_Unit = 0
                            };
                            objSIPMonthly.Liquid_Unit = 0;
                            objSIPMonthly.SIP_Nav = 0;
                            objSIPMonthly.SIP_Return = 0M;
                        }
                        if (objLastSIP != null)
                        {
                            if (currentDate.Month != objLastSIP.CalculateDate.Month && objLastSIP.Liquid > 0)
                            {
                                if (objLastSIP.Liquid < SwitchAmount)
                                    SwitchAmount = objLastSIP.Liquid;

                                objSIPMonthly = new OutputMonthlySIP()
                                {
                                    CalculateDate = currentDate,
                                    Liquid = (objLastSIP.Liquid - SwitchAmount)
                                };
                                //objSIPMonthly.Liquid_Unit = Math.Round((objSIPMonthly.Liquid / latestSourceSchemeNav), 6);
                                objSIPMonthly.Edge_Unit = objLastSIP.Edge_Unit + Math.Round((SwitchAmount / latestDestinationSchemeNav), 6);
                                objSIPMonthly.Edge = Math.Round(objSIPMonthly.Edge_Unit * latestDestinationSchemeNav, 6);
                                objSIPMonthly.SIP_Nav = objSIPMonthly.Edge;
                                //objSIPMonthly.SIP_Return = Math.Round(((objSIPMonthly.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                objSIPMonthly.DestinationNav = latestDestinationSchemeNav;


                                if (objSIPMonthly.SIP_Nav != 0 && objLastSIP.SIP_Nav != 0)
                                    objSIPMonthly.SIP_Return = Math.Round(((objSIPMonthly.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);

                            }
                            else
                            {
                                objSIPMonthly = new OutputMonthlySIP()
                                {
                                    CalculateDate = currentDate,
                                    //Liquid_Unit = objLastSIP.Liquid_Unit,
                                    Edge_Unit = objLastSIP.Edge_Unit
                                };
                                objSIPMonthly.Liquid = objLastSIP.Liquid;
                                objSIPMonthly.Edge = Math.Round((objSIPMonthly.Edge_Unit * latestDestinationSchemeNav), 6);
                                objSIPMonthly.SIP_Nav = objSIPMonthly.Edge;
                                objSIPMonthly.DestinationNav = latestDestinationSchemeNav;

                                if (objSIPMonthly.SIP_Nav != 0 && objLastSIP.SIP_Nav != 0)
                                    objSIPMonthly.SIP_Return = Math.Round(((objSIPMonthly.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                //objSIPMonthly.SIP_Return = Math.Round(((objSIPMonthly.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                            }
                        }
                    }
                    else
                    {
                        if (lastCalculatedSIP == null) lastCalculatedSIP = OutResult.Last();
                        var calcDate = Input.ToDate;

                        var currentLastSIP = OutResult.Last();

                        objSIPMonthly = new OutputMonthlySIP()
                        {
                            CalculateDate = currentDate,
                            //SourceNav = latestSourceSchemeNav,
                            DestinationNav = latestDestinationSchemeNav,
                            Edge_Unit = lastCalculatedSIP.Edge_Unit,
                            //Liquid_Unit = lastCalculatedSIP.Liquid_Unit,
                            Edge = (latestDestinationSchemeNav * lastCalculatedSIP.Edge_Unit),
                            Liquid = objLastSIP.Liquid
                        };
                        objSIPMonthly.SIP_Nav = objSIPMonthly.Edge;

                        if (objSIPMonthly.SIP_Nav != 0 && objLastSIP.SIP_Nav != 0)
                            objSIPMonthly.SIP_Return = Math.Round(((objSIPMonthly.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                        //objSIPMonthly.SIP_Return = Math.Round((objSIPMonthly.SIP_Nav / currentLastSIP.SIP_Nav) - 1, 4) * 100;
                    }
                    OutResult.Add(objSIPMonthly);
                }

                #endregion

                return OutResult;
            }
            catch (Exception ex)
            {
                Msg = ex.Message.ToString();
                return null;
            }
        }
        public OutputTriggeredSIP[] CalculateSIPPlusIndex4TRI(EdelweissSIPInput Input, out decimal TotalInvestedAmount, out string strError)
        {
            strError = string.Empty;
            TotalInvestedAmount = default(decimal);
            TransCount = new List<SIPPlusMonthlyDeductionCount>();
            var uniqeDt = DateList.ToList();
            var TrigIndexData = GetTriggeredIndexData4TRI();

            var chk = TrigIndexData.Where(c => c.IsTriggerred != EdelweissTriggerType.NoTrigger).ToArray();
            //var hh = chk.Where(b => b.Index_Value > Convert.ToDecimal(0.50)).ToArray();
            if (uniqeDt.First().Date > Input.FromDate) uniqeDt.Insert(0, Input.FromDate);

            var IndexData = (from indexData in TrigIndexData
                             join uniqueDate in uniqeDt on indexData.Record_Date equals uniqueDate
                             select indexData).ToArray();


            IList<OutputSIPPlus> outputList = new List<OutputSIPPlus>();
            var SwitchAmount = Input.SwitchAmtSIPPlus;
            decimal TotalSwitchedAmt = 0;

            var IndexID = GetIndexID(Input.TRIBenchmark);
            if (IndexID == null) { strError = "Index Name Error."; return null; }


            try
            {
                #region Calculate 1

                TriggerredIndexDetails lastIndexDetails = null;
                foreach (var item in IndexData)
                {
                    var destinationSchemeNav = GetLatestIndexRec4TRI(item.Record_Date, Convert.ToInt32(IndexID.Value)).Nav_Value;

                    OutputSIPPlus objLastSIP = null;

                    if (outputList.Count > 0)
                        objLastSIP = outputList.Last();

                    if (item.Record_Date <= Input.ToDate)
                    {
                        if (outputList.Count == 0)
                        {


                            var curTransactionCount = TransCount.Where(p => p.Month == item.Record_Date.Month && p.Year == item.Record_Date.Year).ToArray().Count();

                            if ((item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && (Input.NoOfSwitchPerMonth != 0 && curTransactionCount < Input.NoOfSwitchPerMonth)) ||
                            (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Input.NoOfSwitchPerMonth == 0))
                            {
                                OutputSIPPlus objOutput = new OutputSIPPlus() { };

                                
                                var calSwitch = Input.SwitchingRange.Where(p => item.Index_Value <= p.Key).ToList().OrderBy(p => p.Key).FirstOrDefault().Value;

                                //objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.SIPUnit = objOutput.SIPUnit + (calSwitch / destinationSchemeNav);
                                objOutput.CurrentValuation = Math.Round(objOutput.SIPUnit * destinationSchemeNav, 6);
                                TotalSwitchedAmt = TotalSwitchedAmt + calSwitch;
                                TotalInvestedAmount = TotalInvestedAmount + calSwitch;
                                objOutput.IsTriggard = true;
                                //if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                //    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                //        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);
                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = TotalSwitchedAmt;
                                TransCount.Add(new SIPPlusMonthlyDeductionCount() { Month = item.Record_Date.Month, Year = item.Record_Date.Year });

                                objOutput.CalculatingDate = item.Record_Date;
                                outputList.Add(objOutput);
                                continue;
                            }

                            else
                            {


                                OutputSIPPlus objOutput = new OutputSIPPlus()
                                {
                                    DestinationSchemeNav = destinationSchemeNav,
                                    SIPUnit = 0,
                                    TransferredAmt = 0,
                                    CalculatingDate = item.Record_Date,
                                    CurrentValuation = 0
                                };
                                outputList.Add(objOutput);
                                continue;
                            }
                        }

                        if (outputList.Count > 0)
                        {
                            OutputSIPPlus objOutput = new OutputSIPPlus() { };

                            var curTransactionCount = TransCount.Where(p => p.Month == item.Record_Date.Month && p.Year == item.Record_Date.Year).ToArray().Count();


                            if ((item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && (Input.NoOfSwitchPerMonth != 0 && curTransactionCount < Input.NoOfSwitchPerMonth)) ||
                            (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Input.NoOfSwitchPerMonth == 0))
                            {

                                // Calculate Switching Amount Based On The Range Selected

                                var calSwitch = Input.SwitchingRange.Where(p => item.Index_Value <= p.Key).ToList().OrderBy(p => p.Key).FirstOrDefault().Value;

                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.SIPUnit = objOutput.SIPUnit + (calSwitch / destinationSchemeNav);
                                objOutput.CurrentValuation = Math.Round(objOutput.SIPUnit * destinationSchemeNav, 6);
                                TotalSwitchedAmt = TotalSwitchedAmt + calSwitch;
                                TotalInvestedAmount = TotalInvestedAmount + calSwitch;
                                objOutput.IsTriggard = true;
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);
                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = TotalSwitchedAmt;
                                TransCount.Add(new SIPPlusMonthlyDeductionCount() { Month = item.Record_Date.Month, Year = item.Record_Date.Year });
                            }
                            else
                            {
                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.CurrentValuation = Math.Round((objOutput.SIPUnit * destinationSchemeNav), 6);
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);

                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = objLastSIP.InvestedAmt;
                                objOutput.IsTriggard = false;
                                objOutput.IsTriggard = false;

                            }
                            objOutput.CalculatingDate = item.Record_Date;
                            outputList.Add(objOutput);

                        }
                        lastIndexDetails = item;
                    }
                    else
                    {
                        var calcDate = Input.ToDate;
                        var currentLastSIP = outputList.Last();
                        OutputSIPPlus objTrgOutput = new OutputSIPPlus()
                        {
                            CalculatingDate = item.Record_Date,
                            DestinationSchemeNav = destinationSchemeNav,
                            SIPUnit = objLastSIP.SIPUnit,
                            CurrentValuation = objLastSIP.SIPUnit * destinationSchemeNav,
                            InvestedAmt = TotalSwitchedAmt,
                            IsTriggard = false
                        };
                        outputList.Add(objTrgOutput);
                    }
                }
                #endregion

                // Return The Amount Invested Logic For SIP Plus Only

                //TotalInvestedAmount = TransCount.Count * SwitchAmount;

                return outputList.Select(p => new OutputTriggeredSIP
                {
                    CalculateDate = p.CalculatingDate,
                    Edge_Unit = p.SIPUnit,
                    IsTriggard = p.IsTriggard,
                    Edge = p.CurrentValuation.Value,
                    SIP_Return = p.SIPReturn,
                    SIP_Nav = p.CurrentValuation.Value,
                    Swithch_Amt = SwitchAmount,
                    DestinationNav = p.DestinationSchemeNav,
                    Liquid = p.InvestedAmt
                }).ToArray();
            }
            catch (Exception ex)
            {
                strError = ex.Message.ToString();
                return null;
            }
        }

        public OutputTriggeredSIP[] CalculateSIPPlusIndex(EdelweissSIPInput Input, out decimal TotalInvestedAmount, out string strError)
        {
            strError = string.Empty;
            TotalInvestedAmount = default(decimal);
            TransCount = new List<SIPPlusMonthlyDeductionCount>();
            var uniqeDt = DateList.ToList();
            var TrigIndexData = GetTriggeredIndexData();
            if (uniqeDt.First().Date > Input.FromDate) uniqeDt.Insert(0, Input.FromDate);

            var IndexData = (from indexData in TrigIndexData
                             join uniqueDate in uniqeDt on indexData.Record_Date equals uniqueDate
                             select indexData).ToArray();


            IList<OutputSIPPlus> outputList = new List<OutputSIPPlus>();
            var SwitchAmount = Input.SwitchAmtSIPPlus;
            decimal TotalSwitchedAmt = 0;

            var IndexID = GetIndexID(Input.Benchmark);
            if (IndexID == null) { strError = "Index Name Error."; return null; }


            try
            {
                #region Calculate 1

                TriggerredIndexDetails lastIndexDetails = null;
                foreach (var item in IndexData)
                {
                    var destinationSchemeNav = GetLatestIndexRec(item.Record_Date, Convert.ToInt32(IndexID.Value)).Nav_Value;

                    OutputSIPPlus objLastSIP = null;

                    if (outputList.Count > 0)
                        objLastSIP = outputList.Last();

                    if (item.Record_Date <= Input.ToDate)
                    {
                        if (outputList.Count == 0)
                        {
                            OutputSIPPlus objOutput = new OutputSIPPlus()
                            {
                                DestinationSchemeNav = destinationSchemeNav,
                                SIPUnit = 0,
                                TransferredAmt = 0,
                                CalculatingDate = item.Record_Date,
                                CurrentValuation = 0
                            };
                            outputList.Add(objOutput);
                            continue;
                        }

                        if (outputList.Count > 0)
                        {
                            OutputSIPPlus objOutput = new OutputSIPPlus() { };

                            var curTransactionCount = TransCount.Where(p => p.Month == item.Record_Date.Month && p.Year == item.Record_Date.Year).ToArray().Count();


                            if ((item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && (Input.NoOfSwitchPerMonth != 0 && curTransactionCount < Input.NoOfSwitchPerMonth)) ||
                            (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Input.NoOfSwitchPerMonth == 0))
                            {

                                // Calculate Switching Amount Based On The Range Selected

                                var calSwitch = Input.SwitchingRange.Where(p => item.Index_Value <= p.Key).ToList().OrderBy(p => p.Key).FirstOrDefault().Value;

                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.SIPUnit = objOutput.SIPUnit + (calSwitch / destinationSchemeNav);
                                objOutput.CurrentValuation = Math.Round(objOutput.SIPUnit * destinationSchemeNav, 6);
                                TotalSwitchedAmt = TotalSwitchedAmt + calSwitch;
                                TotalInvestedAmount = TotalInvestedAmount + calSwitch;
                                objOutput.IsTriggard = true;
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);
                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = TotalSwitchedAmt;
                                TransCount.Add(new SIPPlusMonthlyDeductionCount() { Month = item.Record_Date.Month, Year = item.Record_Date.Year });
                            }
                            else
                            {
                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.CurrentValuation = Math.Round((objOutput.SIPUnit * destinationSchemeNav), 6);
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);

                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = objLastSIP.InvestedAmt;
                                objOutput.IsTriggard = false;
                                objOutput.IsTriggard = false;

                            }
                            objOutput.CalculatingDate = item.Record_Date;
                            outputList.Add(objOutput);

                        }
                        lastIndexDetails = item;
                    }
                    else
                    {
                        var calcDate = Input.ToDate;
                        var currentLastSIP = outputList.Last();
                        OutputSIPPlus objTrgOutput = new OutputSIPPlus()
                        {
                            CalculatingDate = item.Record_Date,
                            DestinationSchemeNav = destinationSchemeNav,
                            SIPUnit = objLastSIP.SIPUnit,
                            CurrentValuation = objLastSIP.SIPUnit * destinationSchemeNav,
                            InvestedAmt = TotalSwitchedAmt,
                            IsTriggard = false
                        };
                        outputList.Add(objTrgOutput);
                    }
                }
                #endregion

                // Return The Amount Invested Logic For SIP Plus Only

                //TotalInvestedAmount = TransCount.Count * SwitchAmount;

                return outputList.Select(p => new OutputTriggeredSIP
                {
                    CalculateDate = p.CalculatingDate,
                    Edge_Unit = p.SIPUnit,
                    IsTriggard = p.IsTriggard,
                    Edge = p.CurrentValuation.Value,
                    SIP_Return = p.SIPReturn,
                    SIP_Nav = p.CurrentValuation.Value,
                    Swithch_Amt = SwitchAmount,
                    DestinationNav = p.DestinationSchemeNav,
                    Liquid = p.InvestedAmt
                }).ToArray();
            }
            catch (Exception ex)
            {
                strError = ex.Message.ToString();
                return null;
            }
        }

        public OutputTriggeredSIP[] CalculateSIPPlusAdditionalIndex(EdelweissSIPInput Input, out decimal TotalInvestedAmount, out string strError)
        {
            strError = string.Empty;
            TotalInvestedAmount = default(decimal);
            TransCount = new List<SIPPlusMonthlyDeductionCount>();
            var uniqeDt = DateList.ToList();
            var TrigIndexData = GetTriggeredIndexData();
            if (uniqeDt.First().Date > Input.FromDate) uniqeDt.Insert(0, Input.FromDate);

            var IndexData = (from indexData in TrigIndexData
                             join uniqueDate in uniqeDt on indexData.Record_Date equals uniqueDate
                             select indexData).ToArray();


            IList<OutputSIPPlus> outputList = new List<OutputSIPPlus>();
            var SwitchAmount = Input.SwitchAmtSIPPlus;
            decimal TotalSwitchedAmt = 0;

            var IndexID = GetIndexID(Input.AdditionalBenchmark);

            if (IndexID == null) { strError = "Index Name Error."; return null; }


            try
            {
                #region Calculate 1

                TriggerredIndexDetails lastIndexDetails = null;
                foreach (var item in IndexData)
                {
                    var destinationSchemeNav = GetLatestAdditionalIndexRec(item.Record_Date, Convert.ToInt32(IndexID.Value)).Nav_Value;

                    OutputSIPPlus objLastSIP = null;

                    if (outputList.Count > 0)
                        objLastSIP = outputList.Last();

                    if (item.Record_Date <= Input.ToDate)
                    {
                        if (outputList.Count == 0)
                        {
                            var curTransactionCount = TransCount.Where(p => p.Month == item.Record_Date.Month && p.Year == item.Record_Date.Year).ToArray().Count();

                            if ((item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && (Input.NoOfSwitchPerMonth != 0 && curTransactionCount < Input.NoOfSwitchPerMonth)) ||
                           (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Input.NoOfSwitchPerMonth == 0))
                            {

                                // Calculate Switching Amount Based On The Range Selected
                                OutputSIPPlus objOutput = new OutputSIPPlus() { };

                                var calSwitch = Input.SwitchingRange.Where(p => item.Index_Value <= p.Key).ToList().OrderBy(p => p.Key).FirstOrDefault().Value;

                                //objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.SIPUnit = objOutput.SIPUnit + (calSwitch / destinationSchemeNav);
                                objOutput.CurrentValuation = Math.Round(objOutput.SIPUnit * destinationSchemeNav, 6);
                                TotalSwitchedAmt = TotalSwitchedAmt + calSwitch;

                                TotalInvestedAmount = TotalInvestedAmount + calSwitch;

                                objOutput.IsTriggard = true;
                                //if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                //    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                //        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);
                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = TotalSwitchedAmt;
                                TransCount.Add(new SIPPlusMonthlyDeductionCount() { Month = item.Record_Date.Month, Year = item.Record_Date.Year });

                                objOutput.CalculatingDate = item.Record_Date;
                                outputList.Add(objOutput);
                                continue;
                            }
                            else
                            {


                                OutputSIPPlus objOutput = new OutputSIPPlus()
                                {
                                    DestinationSchemeNav = destinationSchemeNav,
                                    SIPUnit = 0,
                                    TransferredAmt = 0,
                                    CalculatingDate = item.Record_Date,
                                    CurrentValuation = 0
                                };
                                outputList.Add(objOutput);
                                continue;
                            }
                        }

                        if (outputList.Count > 0)
                        {
                            OutputSIPPlus objOutput = new OutputSIPPlus() { };

                            var curTransactionCount = TransCount.Where(p => p.Month == item.Record_Date.Month && p.Year == item.Record_Date.Year).ToArray().Count();

                            if ((item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && (Input.NoOfSwitchPerMonth != 0 && curTransactionCount < Input.NoOfSwitchPerMonth)) ||
                           (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Input.NoOfSwitchPerMonth == 0))
                            {

                                // Calculate Switching Amount Based On The Range Selected

                                var calSwitch = Input.SwitchingRange.Where(p => item.Index_Value <= p.Key).ToList().OrderBy(p => p.Key).FirstOrDefault().Value;

                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.SIPUnit = objOutput.SIPUnit + (calSwitch / destinationSchemeNav);
                                objOutput.CurrentValuation = Math.Round(objOutput.SIPUnit * destinationSchemeNav, 6);
                                TotalSwitchedAmt = TotalSwitchedAmt + calSwitch;

                                TotalInvestedAmount = TotalInvestedAmount + calSwitch;

                                objOutput.IsTriggard = true;
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);
                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = TotalSwitchedAmt;
                                TransCount.Add(new SIPPlusMonthlyDeductionCount() { Month = item.Record_Date.Month, Year = item.Record_Date.Year });
                            }
                            else
                            {
                                objOutput.SIPUnit = objLastSIP.SIPUnit;
                                objOutput.CurrentValuation = Math.Round((objOutput.SIPUnit * destinationSchemeNav), 6);
                                if (objOutput.CurrentValuation.HasValue && objLastSIP.CurrentValuation.HasValue)
                                    if (objOutput.CurrentValuation.Value != 0 && objLastSIP.CurrentValuation.Value != 0)
                                        objOutput.SIPReturn = Math.Round(((objOutput.CurrentValuation.Value / objLastSIP.CurrentValuation.Value) - 1) * 100, 6);

                                objOutput.DestinationSchemeNav = destinationSchemeNav;
                                objOutput.InvestedAmt = objLastSIP.InvestedAmt;
                                objOutput.IsTriggard = false;
                                objOutput.IsTriggard = false;

                            }
                            objOutput.CalculatingDate = item.Record_Date;
                            outputList.Add(objOutput);

                        }
                        lastIndexDetails = item;
                    }
                    else
                    {
                        var calcDate = Input.ToDate;
                        var currentLastSIP = outputList.Last();
                        OutputSIPPlus objTrgOutput = new OutputSIPPlus()
                        {
                            CalculatingDate = item.Record_Date,
                            DestinationSchemeNav = destinationSchemeNav,
                            SIPUnit = objLastSIP.SIPUnit,
                            CurrentValuation = objLastSIP.SIPUnit * destinationSchemeNav,
                            InvestedAmt = TotalSwitchedAmt,
                            IsTriggard = false
                        };
                        outputList.Add(objTrgOutput);
                    }
                }
                #endregion

                // Return The Amount Invested Logic For SIP Plus Only

                //TotalInvestedAmount = TransCount.Count * SwitchAmount;

                return outputList.Select(p => new OutputTriggeredSIP
                {
                    CalculateDate = p.CalculatingDate,
                    Edge_Unit = p.SIPUnit,
                    IsTriggard = p.IsTriggard,
                    Edge = p.CurrentValuation.Value,
                    SIP_Return = p.SIPReturn,
                    SIP_Nav = p.CurrentValuation.Value,
                    Swithch_Amt = SwitchAmount,
                    DestinationNav = p.DestinationSchemeNav,
                    Liquid = p.InvestedAmt
                }).ToArray();
            }
            catch (Exception ex)
            {
                strError = ex.Message.ToString();
                return null;
            }
        }
        private LatestIndexRecords GetLatestIndexRec4TRI(DateTime RecordDate, int index_ID)
        {
            IndexData latestIndex = null;

            latestIndex = IndexValWithTRI.Where(p => p.Index_ID == index_ID &&
             p.Index_Date.Date >= RecordDate.Date).OrderBy(p => p.Index_Date).FirstOrDefault();

            if (latestIndex != null)
                return new LatestIndexRecords() { Nav_Date = latestIndex.Index_Date, Nav_Value = Convert.ToDecimal(latestIndex.Index_Val) };
            else
            {
                latestIndex = IndexVal.Where(p => p.Index_ID == index_ID && p.Index_Date.Date <= RecordDate.Date).OrderByDescending(p => p.Index_Date).FirstOrDefault();
                return new LatestIndexRecords() { Nav_Date = latestIndex.Index_Date, Nav_Value = Convert.ToDecimal(latestIndex.Index_Val) };
            }
        }
        private LatestIndexRecords GetLatestIndexRec(DateTime RecordDate, int index_ID)
        {
            IndexData latestIndex = null;

            latestIndex = IndexVal.Where(p => p.Index_ID == index_ID &&
             p.Index_Date.Date >= RecordDate.Date).OrderBy(p => p.Index_Date).FirstOrDefault();

            if (latestIndex != null)
                return new LatestIndexRecords() { Nav_Date = latestIndex.Index_Date, Nav_Value = Convert.ToDecimal(latestIndex.Index_Val) };
            else
            {
                latestIndex = IndexVal.Where(p => p.Index_ID == index_ID && p.Index_Date.Date <= RecordDate.Date).OrderByDescending(p => p.Index_Date).FirstOrDefault();
                return new LatestIndexRecords() { Nav_Date = latestIndex.Index_Date, Nav_Value = Convert.ToDecimal(latestIndex.Index_Val) };
            }
        }

        private LatestIndexRecords GetLatestAdditionalIndexRec(DateTime RecordDate, int index_ID)
        {
            IndexData latestIndex = null;

            latestIndex = AdditionalIndexVal.Where(p => p.Index_ID == index_ID &&
             p.Index_Date.Date >= RecordDate.Date).OrderBy(p => p.Index_Date).FirstOrDefault();

            if (latestIndex != null)
                return new LatestIndexRecords() { Nav_Date = latestIndex.Index_Date, Nav_Value = Convert.ToDecimal(latestIndex.Index_Val) };
            else
            {
                latestIndex = IndexVal.Where(p => p.Index_ID == index_ID && p.Index_Date.Date <= RecordDate.Date).OrderByDescending(p => p.Index_Date).FirstOrDefault();
                return new LatestIndexRecords() { Nav_Date = latestIndex.Index_Date, Nav_Value = Convert.ToDecimal(latestIndex.Index_Val) };
            }
        }

        public IList<OutputTriggeredSIP> CalculateTriggeredSIPChanged(EdelweissSIPInput Input, out string Msg)
        {
            Msg = string.Empty;
            var uniqeDt = DateList.ToList();
            var TrigIndexData = GetTriggeredIndexData();
            if (uniqeDt.First().Date > Input.FromDate) uniqeDt.Insert(0, Input.FromDate);
            var IndexData = (from indexData in TrigIndexData
                             join uniqueDate in uniqeDt on indexData.Record_Date equals uniqueDate
                             select indexData).ToArray();


            IList<OutputTriggeredSIP> outputList = new List<OutputTriggeredSIP>();
            //var SwitchAmount = Input.InitialInvestment * (Input.SwitchAmount / 100);
            var SwitchAmount = Input.SwitchAmount;
            //var MaxSwitch = Math.Round((Input.InitialInvestment / SwitchAmount), 0);

            var CurrentSwitch = 0;

            try
            {
                #region Calculate 1

                TriggerredIndexDetails lastIndexDetails = null;
                foreach (var item in IndexData)
                {
                    var sourceSchemeNav = GetLatestSchemeNav(item.Record_Date, Input.SourceSchemeId).Nav_Value;
                    var destinationSchemeNav = GetLatestSchemeNav(item.Record_Date, Input.DestinitionSchemeId).Nav_Value;

                    OutputTriggeredSIP objLastSIP = null;

                    if (outputList.Count > 0)
                        objLastSIP = outputList.Last();

                    if (item.Record_Date <= Input.ToDate)
                    {
                        if (outputList.Count == 0)
                        {
                            OutputTriggeredSIP objOutput = new OutputTriggeredSIP()
                            {
                                SourceNav = sourceSchemeNav,
                                DestinationNav = destinationSchemeNav,
                                Liquid = Input.InitialInvestment,
                                Liquid_Unit = Math.Round(Convert.ToDecimal((Input.InitialInvestment / sourceSchemeNav)), 6),
                                Edge = 0,
                                Edge_Unit = 0,
                                CalculateDate = item.Record_Date
                            };
                            objOutput.SIP_Nav = (objOutput.Liquid + objOutput.Edge);
                            outputList.Add(objOutput);
                            continue;
                        }

                        if (outputList.Count > 0)
                        {
                            OutputTriggeredSIP objOutput = new OutputTriggeredSIP() { };

                            //if (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && CurrentSwitch < MaxSwitch)
                            //{
                            //if (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6) > SwitchAmount)
                            //{
                            if (item.IsTriggerred == EdelweissTriggerType.NegativeTrigger && Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6) > 0)
                            {
                                var calSwitch = Input.SwitchingRange.Where(p => item.Index_Value <= p.Key).ToList().OrderBy(p => p.Key).FirstOrDefault().Value; //add new
                                SwitchAmount = calSwitch;
                                // Switch amount is deducted from liquid to edge on the day after the triggered is fired
                                // divided by that day's latest nav
                                objOutput.Liquid_Unit = objLastSIP.Liquid_Unit;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = objLastSIP.Edge_Unit;
                                objOutput.Edge = Math.Round(objOutput.Edge_Unit * destinationSchemeNav, 6);

                                if (Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6) <= SwitchAmount)
                                    SwitchAmount = Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6);

                                //if (CurrentSwitch + 1 == MaxSwitch)
                                //    SwitchAmount = Math.Round(objLastSIP.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Liquid_Unit = Math.Round((objOutput.Liquid - SwitchAmount) / sourceSchemeNav, 6);

                                // Transfer the switch amount from liquid to edge
                                objOutput.Edge = objOutput.Edge + SwitchAmount;
                                objOutput.Swithch_Amt = SwitchAmount;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = Math.Round((objOutput.Edge / destinationSchemeNav), 6);
                                // Sum Of Liquid & Edge Value
                                objOutput.SIP_Nav = objOutput.Liquid + objOutput.Edge;
                                objOutput.SIP_Return = Math.Round(((objOutput.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 2);
                                objOutput.SourceNav = sourceSchemeNav;
                                objOutput.DestinationNav = destinationSchemeNav;
                                objOutput.IsTriggard = true;
                                CurrentSwitch++;
                            }
                            else
                            {
                                objOutput.Liquid_Unit = objLastSIP.Liquid_Unit;
                                objOutput.Liquid = Math.Round(objOutput.Liquid_Unit * sourceSchemeNav, 6);
                                objOutput.Edge_Unit = objLastSIP.Edge_Unit;
                                objOutput.Edge = Math.Round(objOutput.Edge_Unit * destinationSchemeNav, 6);
                                objOutput.SIP_Nav = objOutput.Liquid + objOutput.Edge;
                                objOutput.SIP_Return = Math.Round(((objOutput.SIP_Nav / objLastSIP.SIP_Nav) - 1) * 100, 6);
                                objOutput.SourceNav = sourceSchemeNav;
                                objOutput.DestinationNav = destinationSchemeNav;
                                objOutput.IsTriggard = false;
                            }
                            objOutput.CalculateDate = item.Record_Date;
                            outputList.Add(objOutput);

                        }
                        lastIndexDetails = item;
                    }
                    else
                    {
                        //if (lastCalculatedSIP == null) lastCalculatedSIP = outputList.Last();

                        var calcDate = Input.ToDate;
                        var currentLastSIP = outputList.Last();
                        OutputTriggeredSIP objTrgOutput = new OutputTriggeredSIP()
                        {
                            //CalculateDate = calcDate,
                            CalculateDate = item.Record_Date,
                            SourceNav = sourceSchemeNav,
                            DestinationNav = destinationSchemeNav,
                            Edge_Unit = objLastSIP.Edge_Unit,
                            Liquid_Unit = objLastSIP.Liquid_Unit,
                            Edge = (destinationSchemeNav * objLastSIP.Edge_Unit),
                            Liquid = (sourceSchemeNav * objLastSIP.Liquid_Unit),
                        };
                        objTrgOutput.SIP_Nav = (objTrgOutput.Liquid + objTrgOutput.Edge);
                        objTrgOutput.SIP_Return = Math.Round((objTrgOutput.SIP_Nav / currentLastSIP.SIP_Nav) - 1, 6) * 100;
                        outputList.Add(objTrgOutput);
                    }
                }
                #endregion



                return outputList;
            }
            catch (Exception ex)
            {
                Msg = ex.Message.ToString();
                return null;
            }
        }

        #endregion

        public static EdelSchemeIndex[] GetIndex(int[] indexIds)
        {
            using (var db = new PrincipalCalcDataContext())
            {
                return db.T_INDEX_MASTERs.Where(p => indexIds.Contains(Convert.ToInt32(p.INDEX_ID)))
                    .Select(q => new EdelSchemeIndex() { IndexName = q.INDEX_NAME, IndexID = Convert.ToInt32(q.INDEX_ID) }).ToArray();

            }
        }
        public decimal GetIndexID(decimal SchemeId)
        {
            using (var db = new PrincipalCalcDataContext())
            {
                var Index = db.T_SCHEMES_INDEXes.Where(p => p.SCHEME_ID == SchemeId).FirstOrDefault();
                return  Index.INDEX_ID;
            }
        }
        public EdelSchemeIndex GetIndexIdTRI(string IndexName)
        {
            EdelSchemeIndex _EdelSchemeIndex = null;
            using (var db = new PrincipalCalcDataContext())
            {
                var Index = db.T_INDEX_MASTERs.Where(p => p.INDEX_NAME.Replace(" ", "").ToUpper() == IndexName.Replace(" ", "").ToUpper());//;.FirstOrDefault();
                if (Index.Any())
                {
                    _EdelSchemeIndex=new EdelSchemeIndex{IndexID=Convert.ToInt32(Index.FirstOrDefault().INDEX_ID),IndexName=Index.FirstOrDefault().INDEX_NAME};                
                }
                //return Index == null ? default(decimal?) : Index.INDEX_ID;
                return _EdelSchemeIndex;
            }
        }
        public SchemeNature[] GetNatureSubnature(decimal Schemeid)
        {
            using (var db = new PrincipalCalcDataContext())
            {
                return (from tbl in db.T_SCHEMES_MASTERs
                        from tbl2 in db.T_FUND_MASTERs
                        from tbl3 in db.T_SCHEMES_NATUREs
                        from tbl4 in db.T_SCHEMES_SUB_NATUREs
                        where tbl.Fund_Id == tbl2.FUND_ID && tbl2.NATURE_ID == tbl3.Nature_ID 
                        && tbl4.Sub_Nature_ID==tbl2.SUB_NATURE_ID && tbl.Scheme_Id == Schemeid
                        select new SchemeNature()
                        {
                            Nature = tbl3.Nature,
                            //NatureID = tbl3.Nature_ID,
                            SubNature = tbl4.Sub_Nature
                            //SubNatureId=tbl4.Sub_Nature_ID
                        }).ToArray();
                                      
            }
        }
    }
}