using System;
using System.Linq;
using System.Xml.Linq;
using System.Web.UI.WebControls;
using iFrames.BLL;
using System.Data;

namespace iFrames.Pages
{
    public partial class stpCalculator : MyBasePage
    {
        DataTable _dtScheme;
        DataTable _dtSchemeFrm;
        DataTable _dtBenchMark;
        DataTable _dtNavIndx;
        DataTable _dtNavIndxTrnFrm;
        DataTable _dtNavIndxTrnFrmReg;
        DataSet _ds;
        DateTime _fromDate;
        DateTime _toDate;
        DateTime _calDate;
        DateTime _lastSipDate;
        DateTime _asOnDate;
        double _asOnDateNav;
        double _asOnDateNavFrm;
        int _startIndex;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            fillScheme();
            fillBenchMark();
        }

        public void xmlReader(string myXmLfile, string myXmLfileFrom)
        {
            _ds = new DataSet();
            var fsReadXml = new System.IO.FileStream(myXmLfile, System.IO.FileMode.Open);
            try
            {
                _ds.ReadXml(fsReadXml);
                _dtScheme = _ds.Tables[0];
                _dtScheme.Columns.Add("Iss_date", typeof(DateTime));
                var distinctScheme = (from DataRow dRow in _dtScheme.Rows
                                      orderby (string)dRow["name"] ascending
                                      select new { name = dRow["name"], SCH_CODE = dRow["SCH_CODE"] }).Distinct().ToDataTable();
                ddlSchemeName.DataSource = distinctScheme;
                ddlSchemeName.DataTextField = "name";
                ddlSchemeName.DataValueField = "SCH_CODE";
                ddlSchemeName.DataBind();
                ddlSchemeName.Items.Insert(0, "--");
                fsReadXml.Close();
                var ds1 = new DataSet();
                fsReadXml = new System.IO.FileStream(myXmLfileFrom, System.IO.FileMode.Open);
                ds1.ReadXml(fsReadXml);
                _dtSchemeFrm = ds1.Tables[0];
                var distinctSchemeFrm = (from DataRow dRow in _dtSchemeFrm.Rows
                                         orderby (string)dRow["name"] ascending
                                         select new { name = dRow["name"], SCH_CODE = dRow["SCH_CODE"] }).Distinct().ToDataTable();
                ddlSchemeNameTrnFrm.DataSource = distinctSchemeFrm;
                ddlSchemeNameTrnFrm.DataTextField = "name";
                ddlSchemeNameTrnFrm.DataValueField = "SCH_CODE";
                ddlSchemeNameTrnFrm.DataBind();
                ddlSchemeNameTrnFrm.Items.Insert(0, "--");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                fsReadXml.Close();
            }
        }

        private void fillScheme()
        {
            var myXmLfile = Server.MapPath("../App_Data/STPTo.xml");
            var myXmLfileFrm = Server.MapPath("../App_Data/STPFrom.xml");
            xmlReader(myXmLfile, myXmLfileFrm);
            Session["dtScheme"] = _dtScheme;
        }

        private void fillBenchMark()
        {
            ddlBenchMark.Items.Clear();
            _dtBenchMark = Schemes.GetAllInd();
            ddlBenchMark.DataSource = _dtBenchMark;
            ddlBenchMark.DataTextField = "ind_name";
            ddlBenchMark.DataValueField = "ind_code";
            ddlBenchMark.DataBind();
        }

        protected void ddlSchemeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPeriod.Items.Clear();
            ddlBenchMark.SelectedValue = Schemes.GetInd(ddlSchemeName.SelectedValue).Rows[0][0].ToString();
            var xdoc = XElement.Load(Server.MapPath("../App_Data/STPTo.xml"));
            var lv1S =
                xdoc.Descendants("Scheme").Where(lv1 => lv1.Attribute("SCH_CODE").Value == ddlSchemeName.SelectedValue).
                    Select(lv1 => lv1.Descendants());
            DataTable dtPeriod = null;
            foreach (var t in lv1S)
                dtPeriod = t.ToDataTable();
            ddlPeriod.Items.Clear();
            if (dtPeriod != null)
                foreach (var strPeriod in
                    dtPeriod.Rows.Cast<DataRow>().Select(drItem => drItem["FirstAttribute"].ToString().Split('=')).Where(strPeriod => strPeriod[1] == "\"true\""))
                    ddlPeriod.Items.Add(strPeriod[0]);
        }

        protected DateTime GetLastBusinessDay(int year, int month)
        {
            DateTime lastBusinessDay;
            var lastOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            if (lastOfMonth.DayOfWeek == DayOfWeek.Sunday)
                lastBusinessDay = lastOfMonth.AddDays(-2);
            else if (lastOfMonth.DayOfWeek == DayOfWeek.Saturday)
                lastBusinessDay = lastOfMonth.AddDays(-1);
            else
                lastBusinessDay = lastOfMonth;
            return lastBusinessDay;
        }

        protected void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                var lastBDay = (Convert.ToInt32(ddlShipDate.SelectedValue) == -1)
                                   ? GetLastBusinessDay(Convert.ToInt16(txtfrmDate.Text.Split('/')[2]),
                                                        Convert.ToInt16(txtfrmDate.Text.Split('/')[1])).Day
                                   : Convert.ToInt32(ddlShipDate.SelectedValue);
                if (
                    new DateTime(Convert.ToInt16(txtfrmDate.Text.Split('/')[2]),
                                 Convert.ToInt16(txtfrmDate.Text.Split('/')[1]),
                                 Convert.ToInt16(txtfrmDate.Text.Split('/')[0])).Day == lastBDay)
                {
                    Response.Write("<script>alert(\"-- 'From day’ Can't be equal to the 'Ship day' ('" +
                                   ddlShipDate.SelectedValue + "')\")</script>");
                    return;
                }

                #region  Declaration

                DataTable dtAllNav;
                DataTable dtAllIndex;

                DataTable dtAllNavTrnFrm;
                DataTable dtAllIndexTrnFrm;

                _toDate = new DateTime(Convert.ToInt16(txtToDate.Text.Split('/')[2]),
                                       Convert.ToInt16(txtToDate.Text.Split('/')[1]),
                                       Convert.ToInt16(txtToDate.Text.Split('/')[0]));
                _fromDate = new DateTime(Convert.ToInt16(txtfrmDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtfrmDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtfrmDate.Text.Split('/')[0]));
                _asOnDate = new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]),
                                         Convert.ToInt16(txtAsOn.Text.Split('/')[1]),
                                         Convert.ToInt16(txtAsOn.Text.Split('/')[0]));
                _dtScheme = (DataTable) Session["dtScheme"];

                _dtNavIndx = new DataTable();
                _dtNavIndx.Columns.Add("Date", typeof (DateTime));
                _dtNavIndx.Columns.Add("Nav_Rs", typeof (double));
                _dtNavIndx.Columns.Add("ind_val", typeof (double));

                _dtNavIndxTrnFrm = new DataTable();
                _dtNavIndxTrnFrm.Columns.Add("Date", typeof (DateTime));
                _dtNavIndxTrnFrm.Columns.Add("Nav_Rs", typeof (double));
                _dtNavIndxTrnFrm.Columns.Add("ind_val", typeof (double));

                #endregion

                //if (_dtScheme.Select("sch_code = '" + ddlSchemeName.SelectedValue + "'").ToArray().Length > 0)
                //{
                //DateTime IssDate = Convert.ToDateTime(Schemes.GetIssDate(ddlSchemeName.SelectedValue)).AddDays(276);
                //DateTime lastNavDate = Convert.ToDateTime(Schemes.GetLastNav(ddlSchemeName.SelectedValue)).AddDays(276);
                var firstNavDate = Convert.ToDateTime(Schemes.GetFirstNav(ddlSchemeName.SelectedValue)).AddDays(276);
                var firstNavDateFrnSch =
                    Convert.ToDateTime(Schemes.GetFirstNav(ddlSchemeNameTrnFrm.SelectedValue)).AddDays(276);
                if (firstNavDate > _toDate)
                {
                    Response.Write("'To date' can not be less than the 'issue date' of the Scheme");
                    return;
                }
                if (firstNavDate > _fromDate)
                    _fromDate = firstNavDate;

                dtAllNav = Schemes.GetAllNav(ddlSchemeName.SelectedValue);
                //dtAllIndex = Schemes.GetAllInd(ddlBenchMark.SelectedValue);
                dtAllNavTrnFrm = Schemes.GetAllNav(ddlSchemeNameTrnFrm.SelectedValue);
                //dtAllIndexTrnFrm = Schemes.GetAllInd(Schemes.GetInd(ddlSchemeNameTrnFrm.SelectedValue).Rows[0][0].ToString());

                //=======================as on date nav calculation===================================//
                var drdtAsOnNav = dtAllNav.Select("Date<='" + _asOnDate.AddDays(-276) + "'");
                var drdtAsOnNavfrm = dtAllNavTrnFrm.Select("Date<='" + _asOnDate.AddDays(-276) + "'");
                _asOnDateNav = (Convert.ToDouble(drdtAsOnNav[drdtAsOnNav.Length - 1][0].ToString()) - 53)/76;
                _asOnDateNavFrm = (Convert.ToDouble(drdtAsOnNavfrm[drdtAsOnNavfrm.Length - 1][0].ToString()) - 53)/76;
                //=================================end=================================================//

                _dtNavIndx.Rows.Clear();
                _dtNavIndxTrnFrm.Rows.Clear();

                #region only for Quarterly calculation

                if (ddlPeriod.SelectedValue == "Quarterly")
                {
                    var qm = firstNavDate.Month;
                    int[] calMonth = {
                                         qm, qm + 3 > 12 ? qm + 3 - 12 : qm + 3, qm + 6 > 12 ? qm + 6 - 12 : qm + 6,
                                         qm + 9 > 12 ? qm + 9 - 12 : qm + 9
                                     };
                    var flag = 0;
                    while (flag == 0)
                    {
                        foreach (int kk in calMonth)
                        {
                            if (kk != _toDate.Month)
                                flag = 0;
                            else
                            {
                                flag = 1;
                                break;
                            }
                        }
                        if (flag == 0)
                            _toDate = _toDate.AddMonths(-1);
                    }
                }

                #endregion

                if (ddlPeriod.SelectedValue != "Daily")
                {
                    if (_toDate.Day > lastBDay)
                        _calDate = _toDate.AddDays(-(_toDate.Day - lastBDay));
                    else if (_toDate.Day < lastBDay)
                    {
                        var dd = GetLastBusinessDay(_toDate.AddMonths(-1).Year, _toDate.AddMonths(-1).Month).Day;
                        dd = (dd >= lastBDay) ? lastBDay : dd;
                        _calDate = new DateTime(_toDate.AddMonths(-1).Year, _toDate.AddMonths(-1).Month, dd);
                    }
                    else
                        _calDate = _toDate;
                }
                else
                    _calDate = _toDate;

                DataRow dataRow;
                DataRow[] dtdrNav;
                DataRow[] dtdrInd;

                DataRow dataRowTrnFrm;
                DataRow[] dtdrNavTrnFrm;
                DataRow[] dtdrIndTrnFrm;

                _lastSipDate = _calDate;
                while ((_calDate >= _fromDate) && (_calDate >= firstNavDateFrnSch))
                {
                    dtdrNav = dtAllNav.Select("Date='" + _calDate.AddDays(-276) + "'");
                    //dtdrInd = dtAllIndex.Select("dt1='" + CalDate.AddDays(-276) + "'");
                    dtdrNavTrnFrm = dtAllNavTrnFrm.Select("Date<='" + _calDate.AddDays(-276) + "'");
                    //dtdrIndTrnFrm = dtAllIndexTrnFrm.Select("dt1<='" + CalDate.AddDays(-276) + "'");

                    if (dtdrNav.Length > 0)
                    {
                        #region For Transfer From Scheam

                        if (dtdrNavTrnFrm.Length > 0)
                        {
                            dataRowTrnFrm = _dtNavIndxTrnFrm.NewRow();
                            dataRowTrnFrm["Date"] = _calDate;
                            dataRowTrnFrm["Nav_Rs"] =
                                (Convert.ToDouble(dtdrNavTrnFrm[dtdrNavTrnFrm.Length - 1][0].ToString()) - 53)/76;
                            //if (dtdrIndTrnFrm.Length > 0)
                            //    dataRowTrnFrm["ind_val"] = (Convert.ToDouble(dtdrIndTrnFrm[0][0].ToString()) - 53) / 76;
                            _dtNavIndxTrnFrm.Rows.Add(dataRowTrnFrm);
                        }

                        #endregion

                        dataRow = _dtNavIndx.NewRow();
                        dataRow["Date"] = _calDate;
                        dataRow["Nav_Rs"] = (Convert.ToDouble(dtdrNav[0][0].ToString()) - 53)/76;

                        //if (dtdrInd.Length > 0)
                        //    dataRow["ind_val"] = (Convert.ToDouble(dtdrInd[0][0].ToString()) - 53) / 76;

                        _dtNavIndx.Rows.Add(dataRow);
                        //===========If the 'CalDate' is exist the 'from date' the 'while' loop will be terminated ===========//
                        if ((_lastSipDate.Month == _fromDate.Month) && (_lastSipDate.Year == _fromDate.Year))
                            break;

                        if (ddlPeriod.SelectedValue == "Monthly")
                            _calDate = _lastSipDate.AddMonths(-1);
                        if (ddlPeriod.SelectedValue == "Quarterly")
                            _calDate = _lastSipDate.AddMonths(-3);
                        if (ddlPeriod.SelectedValue == "Weekly")
                            _calDate = _lastSipDate.AddDays(-7);
                        if (ddlPeriod.SelectedValue == "Daily")
                            _calDate = _lastSipDate.AddDays(-1);
                        if (ddlPeriod.SelectedValue != "Daily")
                        {
                            var dd = GetLastBusinessDay(_calDate.Year, _calDate.Month).Day;
                            dd = (dd >= lastBDay) ? lastBDay : dd;
                            _calDate = new DateTime(_calDate.Year, _calDate.Month, dd);
                        }
                        _lastSipDate = _calDate;
                        if ((_lastSipDate.Month == _fromDate.Month) && (_lastSipDate.Year == _fromDate.Year) &&
                            _lastSipDate < _fromDate) break;
                    }
                    else
                    {
                        _calDate = ddlPeriod.SelectedValue != "Daily" ? _calDate.AddDays(1) : _calDate.AddDays(-1);
                    }
                }
                _startIndex = _dtNavIndx.Rows.Count - 1;

                #region for first nav insertion

                dtdrNav = dtAllNav.Select("Date>='" + _fromDate.AddDays(-276) + "'");
                dtdrNavTrnFrm = dtAllNavTrnFrm.Select("Date>='" + _fromDate.AddDays(-276) + "'");
                dataRow = _dtNavIndx.NewRow();
                dataRowTrnFrm = _dtNavIndxTrnFrm.NewRow();
                dataRow["Date"] = _fromDate;
                dataRow["Nav_Rs"] = (Convert.ToDouble(dtdrNav[dtdrNav.Length - 1][0].ToString()) - 53)/76;
                dataRowTrnFrm["Date"] = _fromDate;
                dataRowTrnFrm["Nav_Rs"] = (Convert.ToDouble(dtdrNavTrnFrm[dtdrNavTrnFrm.Length - 1][0].ToString()) - 53)/
                                          76;
                _dtNavIndx.Rows.Add(dataRow);
                _dtNavIndxTrnFrm.Rows.Add(dataRowTrnFrm);

                #endregion

                GetCalculation(Convert.ToDecimal(txtTranAmt.Text));
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<script>alert(\"--opps ('a generic error happened, please contact administrator.')\")</script>");
            }
        }

        public void GetCalculation(decimal installedAmt)
        {
            var dtRegStp = _dtNavIndx.Copy();
            var matchDate = DateTime.Today;
            var matchIndex = 0;
            var matchIndexReg = 0;
            if (_dtNavIndx.Rows.Count < 2)
                return;
            _dtNavIndx.Columns.Add("MonReturn", typeof (double));
            _dtNavIndx.Columns.Add("MarketValue", typeof (double));
            _dtNavIndx.Columns.Add("AmtInvest", typeof (double));
            _dtNavIndx.Columns.Add("CumulAmtInvested", typeof (double));
            _dtNavIndx.Columns.Add("UnitBrought", typeof (double));
            _dtNavIndx.Columns.Add("CumulUnitsBought", typeof (double));
            _dtNavIndx.Columns.Add("NetAvgCostPerUnit", typeof (double));

            _dtNavIndx.Columns.Add("MonTargetVal", typeof (double));
            _dtNavIndx.Columns.Add("IncrAmt", typeof (double));
            _dtNavIndx.Columns.Add("MonVTPamt", typeof (double));

            _dtNavIndx.Rows[_startIndex]["MonTargetVal"] = installedAmt;

            _dtNavIndx.Rows[_startIndex]["MarketValue"] = installedAmt;
            _dtNavIndx.Rows[_startIndex]["AmtInvest"] = installedAmt;
            _dtNavIndx.Rows[_startIndex]["CumulAmtInvested"] =
                Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[_startIndex]["AmtInvest"]), 4);
            _dtNavIndx.Rows[_startIndex]["UnitBrought"] =
                Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[_startIndex]["AmtInvest"])/
                           Convert.ToDecimal(_dtNavIndx.Rows[_startIndex]["Nav_Rs"]), 4);
            _dtNavIndx.Rows[_startIndex]["CumulUnitsBought"] =
                Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[_startIndex]["UnitBrought"]), 4);
            _dtNavIndx.Rows[_startIndex]["NetAvgCostPerUnit"] =
                Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[_startIndex]["CumulAmtInvested"])/
                           Convert.ToDecimal(_dtNavIndx.Rows[_startIndex]["CumulUnitsBought"]), 4);

            #region For regular STP

            dtRegStp.Columns.Add("InvestedAmt", typeof (double));
            dtRegStp.Columns.Add("CumulAmtInvested", typeof (double));
            dtRegStp.Columns.Add("Unitsbought", typeof (double));
            dtRegStp.Columns.Add("CumulUnitsBought", typeof (double));
            dtRegStp.Columns.Add("CostPerUnit", typeof (double));

            //==================new reverse calculation================//
            _dtNavIndxTrnFrmReg = _dtNavIndxTrnFrm.Copy();

            _dtNavIndxTrnFrmReg.Columns.Add("CashFlow", typeof (double));
            _dtNavIndxTrnFrmReg.Columns.Add("InvestAmount", typeof (double));
            _dtNavIndxTrnFrmReg.Columns.Add("Unit", typeof (double));
            _dtNavIndxTrnFrmReg.Columns.Add("CumulativeUnits", typeof (double));
            _dtNavIndxTrnFrmReg.Columns.Add("CumulativeAmount", typeof (double));

            _dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["CashFlow"] =
                Convert.ToDecimal(txtinstallAmt.Text);
            _dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["InvestAmount"] =
                Convert.ToDecimal(txtinstallAmt.Text);
            _dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["Unit"] =
                Math.Round(Convert.ToDecimal(txtinstallAmt.Text)/
                           Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["Nav_Rs"]), 4);

            _dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["CumulativeUnits"] =
                Math.Round(Convert.ToDecimal(txtinstallAmt.Text)/
                           Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["Nav_Rs"]), 4);

            _dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["CumulativeAmount"] =
                Math.Round(Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["Nav_Rs"])*
                           Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["Unit"]), 2);
            decimal cashFlow = 0;

            for (var i = _startIndex; i >= 0; i--)
            {
                _dtNavIndxTrnFrmReg.Rows[i]["CashFlow"] = Convert.ToDecimal(txtTranAmt.Text);
                _dtNavIndxTrnFrmReg.Rows[i]["InvestAmount"] = Math.Round(Convert.ToDecimal(txtTranAmt.Text), 4);
                cashFlow = cashFlow + Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[i]["CashFlow"]);

                _dtNavIndxTrnFrmReg.Rows[i]["Unit"] =
                    Math.Round(Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[i]["CashFlow"])/
                               Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[i]["Nav_Rs"]), 4);

                _dtNavIndxTrnFrmReg.Rows[i]["CumulativeUnits"] =
                    Math.Round(Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[i + 1]["CumulativeUnits"]) -
                               Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[i]["Unit"]), 4);

                _dtNavIndxTrnFrmReg.Rows[i]["CumulativeAmount"] =
                    Math.Round(Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[i]["Nav_Rs"])*
                               Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[i]["CumulativeUnits"]), 2);

                if (Convert.ToDecimal(_dtNavIndxTrnFrmReg.Rows[i]["CumulativeAmount"]) >
                    Convert.ToDecimal(txtTranAmt.Text)) continue;
                matchIndexReg = i;
                break;
            }

            #endregion

            #region From Scheme Calculation

            _dtNavIndxTrnFrm.Columns.Add("Unit", typeof (double));
            _dtNavIndxTrnFrm.Columns.Add("CumulativeUnits", typeof (double));
            _dtNavIndxTrnFrm.Columns.Add("CashFlow", typeof (double));
            _dtNavIndxTrnFrm.Columns.Add("CumulativeAmount", typeof (double));

            _dtNavIndxTrnFrm.Rows[_dtNavIndxTrnFrm.Rows.Count - 1]["Unit"] =
                Math.Round(Convert.ToDecimal(txtinstallAmt.Text)/
                           Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[_dtNavIndxTrnFrm.Rows.Count - 1]["Nav_Rs"]), 4);
            _dtNavIndxTrnFrm.Rows[_dtNavIndxTrnFrm.Rows.Count - 1]["CumulativeUnits"] =
                Math.Round(Convert.ToDecimal(txtinstallAmt.Text)/
                           Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[_dtNavIndxTrnFrm.Rows.Count - 1]["Nav_Rs"]), 4);
            _dtNavIndxTrnFrm.Rows[_dtNavIndxTrnFrm.Rows.Count - 1]["CashFlow"] = Convert.ToDecimal(txtinstallAmt.Text);
            _dtNavIndxTrnFrm.Rows[_dtNavIndxTrnFrm.Rows.Count - 1]["CumulativeAmount"] =
                Convert.ToDecimal(txtinstallAmt.Text);

            #endregion

            for (var i = _startIndex - 1; i >= 0; i--)
            {
                _dtNavIndx.Rows[i]["MonReturn"] =
                    Math.Round(
                        ((Convert.ToDecimal((_dtNavIndx.Rows[i]["Nav_Rs"])) -
                          Convert.ToDecimal(_dtNavIndx.Rows[i + 1]["Nav_Rs"]))
                         /Convert.ToDecimal(_dtNavIndx.Rows[i + 1]["Nav_Rs"]))*100, 2);

                _dtNavIndx.Rows[i]["MonTargetVal"] = Convert.ToDecimal(_dtNavIndx.Rows[i + 1]["MonTargetVal"]) +
                                                     installedAmt;

                _dtNavIndx.Rows[i]["MarketValue"] =
                    Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[i + 1]["MonTargetVal"])*
                               (1 + (Convert.ToDecimal(_dtNavIndx.Rows[i]["MonReturn"])/100)), 4);

                if (Convert.ToDecimal(_dtNavIndx.Rows[i]["MarketValue"]) + installedAmt >
                    Convert.ToDecimal(_dtNavIndx.Rows[i]["MonTargetVal"]))
                    _dtNavIndx.Rows[i]["IncrAmt"] = Math.Round(installedAmt, 2);
                else
                    _dtNavIndx.Rows[i]["IncrAmt"] =
                        Math.Round(installedAmt + Convert.ToDecimal(_dtNavIndx.Rows[i]["MonTargetVal"])
                                   - Convert.ToDecimal(_dtNavIndx.Rows[i]["MarketValue"]) - installedAmt, 2);
                decimal val = 0;
                for (var k = _startIndex; k - 1 >= i; k--)
                    val += Convert.ToDecimal(_dtNavIndx.Rows[k]["AmtInvest"]);
                if ((Convert.ToDecimal(txtinstallAmt.Text) - val) < (Convert.ToDecimal(_dtNavIndx.Rows[i]["IncrAmt"])))
                    _dtNavIndx.Rows[i]["AmtInvest"] = Math.Round(Convert.ToDecimal(txtinstallAmt.Text) - val, 2);
                else
                    _dtNavIndx.Rows[i]["AmtInvest"] = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[i]["IncrAmt"]), 4);

                _dtNavIndx.Rows[i]["CumulAmtInvested"] =
                    Math.Round(
                        Convert.ToDecimal(_dtNavIndx.Rows[i + 1]["CumulAmtInvested"]) +
                        Convert.ToDecimal(_dtNavIndx.Rows[i]["AmtInvest"]), 4);

                _dtNavIndx.Rows[i]["UnitBrought"] = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[i]["AmtInvest"])/
                                                               Convert.ToDecimal(_dtNavIndx.Rows[i]["Nav_Rs"]), 4);
                _dtNavIndx.Rows[i]["CumulUnitsBought"] =
                    Math.Round(
                        Convert.ToDecimal(_dtNavIndx.Rows[i + 1]["CumulUnitsBought"]) +
                        Convert.ToDecimal(_dtNavIndx.Rows[i]["UnitBrought"]), 4);

                _dtNavIndx.Rows[i]["NetAvgCostPerUnit"] =
                    Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[i]["CumulAmtInvested"])/
                               Convert.ToDecimal(_dtNavIndx.Rows[i]["CumulUnitsBought"]), 4);

                if (Convert.ToDecimal(_dtNavIndx.Rows[i]["CumulAmtInvested"]) != Convert.ToDecimal(txtinstallAmt.Text))
                    continue;
                matchDate = Convert.ToDateTime(_dtNavIndx.Rows[i]["Date"]);
                matchIndex = i;
                break;
            }

            #region For regular STP

            foreach (DataRow drTo in _dtNavIndxTrnFrmReg.Rows)
            {
                var row = dtRegStp.Rows.Cast<DataRow>()
                    .Where(dr => Convert.ToDateTime(dr["Date"]) == Convert.ToDateTime(drTo["Date"])
                                 && drTo["InvestAmount"] != DBNull.Value && drTo["InvestAmount"] != null &&
                                 !string.IsNullOrEmpty(drTo["InvestAmount"].ToString())).
                    Select(dr => dr);

                if (row.Count() > 0)
                    row.Single()["InvestedAmt"] = string.IsNullOrEmpty(drTo["InvestAmount"].ToString())
                                                      ? DBNull.Value
                                                      : drTo["InvestAmount"];
            }

            dtRegStp.Rows[_startIndex]["CumulAmtInvested"] = Convert.ToDouble(txtTranAmt.Text);
            dtRegStp.Rows[_startIndex]["Unitsbought"] =
                Math.Round(Convert.ToDecimal(txtTranAmt.Text)/Convert.ToDecimal(dtRegStp.Rows[_startIndex]["Nav_Rs"]), 4);
            dtRegStp.Rows[_startIndex]["CumulUnitsBought"] =
                Math.Round(Convert.ToDecimal(dtRegStp.Rows[_startIndex]["Unitsbought"]), 4);

            dtRegStp.Rows[_startIndex]["CostPerUnit"] =
                Math.Round(Convert.ToDecimal(dtRegStp.Rows[_startIndex]["CumulAmtInvested"])/
                Convert.ToDecimal(dtRegStp.Rows[_startIndex]["CumulUnitsBought"]),4);

            for (var i = _startIndex - 1; i >= matchIndexReg; i--)
            {
                dtRegStp.Rows[i]["CumulAmtInvested"] =
                    Math.Round(
                        Convert.ToDecimal(dtRegStp.Rows[i + 1]["CumulAmtInvested"]) +
                        Convert.ToDecimal(dtRegStp.Rows[i]["InvestedAmt"]), 4);
                dtRegStp.Rows[i]["Unitsbought"] =
                    Math.Round(
                        Convert.ToDecimal(dtRegStp.Rows[i]["InvestedAmt"])/
                        Convert.ToDecimal(_dtNavIndx.Rows[i]["Nav_Rs"]), 4);
                dtRegStp.Rows[i]["CumulUnitsBought"] =
                    Math.Round(
                        Convert.ToDecimal(dtRegStp.Rows[i + 1]["CumulUnitsBought"]) +
                        Convert.ToDecimal(dtRegStp.Rows[i]["Unitsbought"]), 4);
                dtRegStp.Rows[i]["CostPerUnit"] =
                    Math.Round(
                        Convert.ToDecimal(dtRegStp.Rows[i]["CumulAmtInvested"])/
                        Convert.ToDecimal(dtRegStp.Rows[i]["CumulUnitsBought"]), 4);
            }

            #endregion

            foreach (DataRow drTo in _dtNavIndx.Rows)
            {
                var row = _dtNavIndxTrnFrm.Rows.Cast<DataRow>()
                    .Where(dr => Convert.ToDateTime(dr["Date"]) == Convert.ToDateTime(drTo["Date"])
                                 && drTo["AmtInvest"] != DBNull.Value && drTo["AmtInvest"] != null &&
                                 !string.IsNullOrEmpty(drTo["AmtInvest"].ToString())).
                    Select(dr => dr);
                if (row.Count() > 0)
                    row.Single()["CashFlow"] = string.IsNullOrEmpty(drTo["AmtInvest"].ToString())
                                                   ? DBNull.Value
                                                   : drTo["AmtInvest"];
            }

            for (var n = _dtNavIndxTrnFrm.Rows.Count - 2; n >= 0; n--)
            {
                if (((_dtNavIndxTrnFrm.Rows[n]["CashFlow"]) == DBNull.Value) ||
                    (Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[n]["CashFlow"]) <= 0) ||
                    matchDate < Convert.ToDateTime(_dtNavIndxTrnFrm.Rows[n]["Date"])) continue;
                _dtNavIndxTrnFrm.Rows[n]["Unit"] = Math.Round(Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[n]["CashFlow"])/
                                                              Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[n]["Nav_Rs"]), 4);

                _dtNavIndxTrnFrm.Rows[n]["CumulativeUnits"] =
                    Math.Round(Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[n + 1]["CumulativeUnits"]) -
                               Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[n]["Unit"]), 4);

                _dtNavIndxTrnFrm.Rows[n]["CumulativeAmount"] =
                    Math.Round(Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[n]["Nav_Rs"])*
                               Convert.ToDecimal(_dtNavIndxTrnFrm.Rows[n]["CumulativeUnits"]), 2);
            }

            var totUnitsPur = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[matchIndex]["CumulUnitsBought"]), 4);
            lbltotUnitsPur.Text = totUnitsPur.ToString();
            var totAmtInvest = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[matchIndex]["CumulAmtInvested"]), 4);
            lbltotAmtInvest.Text = totAmtInvest.ToString();
            var finalMarketVal =
                Math.Round(
                    Convert.ToDecimal(_dtNavIndx.Rows[matchIndex]["CumulUnitsBought"])*
                    Convert.ToDecimal(_dtNavIndx.Rows[matchIndex]["Nav_Rs"]), 4);
            lblfinalMarketVal.Text = finalMarketVal.ToString();
            var totUnitsPur1 = Math.Round(Convert.ToDecimal(dtRegStp.Rows[matchIndexReg]["CumulUnitsBought"]), 4);
            lbltotUnitsPur_1.Text = totUnitsPur1.ToString();
            var totAmtInvest1 = Math.Round(Convert.ToDecimal(dtRegStp.Rows[matchIndexReg]["CumulAmtInvested"]), 4);
            lbltotAmtInvest_1.Text = totAmtInvest1.ToString();
            var finalMarketVal1 =
                Math.Round(
                    Convert.ToDecimal(dtRegStp.Rows[matchIndexReg]["CumulUnitsBought"])*
                    Convert.ToDecimal(_dtNavIndx.Rows[matchIndex]["Nav_Rs"]), 4);
            lblfinalMarketVal_1.Text = finalMarketVal1.ToString();

            _dtNavIndxTrnFrm.Rows[_dtNavIndxTrnFrm.Rows.Count - 1]["CashFlow"] =
                -Convert.ToDouble(_dtNavIndxTrnFrm.Rows[_dtNavIndxTrnFrm.Rows.Count - 1]["CashFlow"]);
            _dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["CashFlow"] =
                -Convert.ToDouble(_dtNavIndxTrnFrmReg.Rows[_dtNavIndxTrnFrmReg.Rows.Count - 1]["CashFlow"]);

            #region As on date calculation

            var drNavIndxAsOn = _dtNavIndx.NewRow();
            drNavIndxAsOn["Date"] = new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]),
                                                 Convert.ToInt16(txtAsOn.Text.Split('/')[1]),
                                                 Convert.ToInt16(txtAsOn.Text.Split('/')[0]));
            drNavIndxAsOn["Nav_Rs"] = _asOnDateNav;
            drNavIndxAsOn["CumulUnitsBought"] = Convert.ToDouble(_dtNavIndx.Rows[matchIndex]["CumulUnitsBought"]);
            drNavIndxAsOn["CumulAmtInvested"] = Math.Round(Convert.ToDouble(_dtNavIndx.Rows[matchIndex]["CumulUnitsBought"])*
                                                _asOnDateNav,2);
            _dtNavIndx.Rows.Add(drNavIndxAsOn);

            var drdRegStp = dtRegStp.NewRow();
            drdRegStp["Date"] = new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]),
                                             Convert.ToInt16(txtAsOn.Text.Split('/')[1]),
                                             Convert.ToInt16(txtAsOn.Text.Split('/')[0]));
            drdRegStp["Nav_Rs"] = _asOnDateNav;
            drdRegStp["CumulUnitsBought"] = Convert.ToDouble(dtRegStp.Rows[matchIndexReg]["CumulUnitsBought"]);
            drdRegStp["CumulAmtInvested"] = Math.Round(Convert.ToDouble(dtRegStp.Rows[matchIndexReg]["CumulUnitsBought"])*
                                            _asOnDateNav,2);
            dtRegStp.Rows.Add(drdRegStp);

            var drNavIndxTrnFrmAsOn = _dtNavIndxTrnFrm.NewRow();
            drNavIndxTrnFrmAsOn["Date"] = new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]),
                                                       Convert.ToInt16(txtAsOn.Text.Split('/')[1]),
                                                       Convert.ToInt16(txtAsOn.Text.Split('/')[0]));
            drNavIndxTrnFrmAsOn["Nav_Rs"] = _asOnDateNavFrm;
            drNavIndxTrnFrmAsOn["CumulativeUnits"] =
                Convert.ToDouble(_dtNavIndxTrnFrm.Rows[matchIndex]["CumulativeUnits"]);
            drNavIndxTrnFrmAsOn["CumulativeAmount"] =
              Math.Round(Convert.ToDouble(_dtNavIndxTrnFrm.Rows[matchIndex]["CumulativeUnits"])*_asOnDateNavFrm,2);

            _dtNavIndxTrnFrm.Rows.Add(drNavIndxTrnFrmAsOn);

            var drNavIndxTrnFrmAsOnReg = _dtNavIndxTrnFrmReg.NewRow();
            drNavIndxTrnFrmAsOnReg["Date"] = new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]),
                                                          Convert.ToInt16(txtAsOn.Text.Split('/')[1]),
                                                          Convert.ToInt16(txtAsOn.Text.Split('/')[0]));
            drNavIndxTrnFrmAsOnReg["Nav_Rs"] = _asOnDateNavFrm;
            drNavIndxTrnFrmAsOnReg["CumulativeUnits"] =
                Convert.ToDouble(_dtNavIndxTrnFrmReg.Rows[matchIndexReg]["CumulativeUnits"]);
            drNavIndxTrnFrmAsOnReg["CumulativeAmount"] =
               Math.Round(Convert.ToDouble(_dtNavIndxTrnFrmReg.Rows[matchIndexReg]["CumulativeUnits"])*_asOnDateNavFrm,2);
            _dtNavIndxTrnFrmReg.Rows.Add(drNavIndxTrnFrmAsOnReg);

            #endregion

            var viewNavIndx = new DataView(_dtNavIndx);
            var viewRegStp = new DataView(dtRegStp);
            var viewNavIndxTrnFrm = new DataView(_dtNavIndxTrnFrm);
            var viewNavIndxTrnFrmReg = new DataView(_dtNavIndxTrnFrmReg);

            viewNavIndx.Sort = "Date";
            viewRegStp.Sort = "Date";
            viewNavIndxTrnFrm.Sort = "Date";
            viewNavIndxTrnFrmReg.Sort = "Date";

            LstSwp.DataSource = viewNavIndx;
            LstSwp.DataBind();

            lstRegular.DataSource = viewRegStp;
            lstRegular.DataBind();

            lst_FromScheme.DataSource = viewNavIndxTrnFrm;
            lst_FromScheme.DataBind();

            lstFromSchemeReg.DataSource = viewNavIndxTrnFrmReg;
            lstFromSchemeReg.DataBind();

            ((Label) lst_FromScheme.FindControl("lblFromSchName")).Text = "Transfer From : " +
                                                                          ddlSchemeNameTrnFrm.SelectedItem.Text;
            ((Label) LstSwp.FindControl("lblToSchSwp")).Text = "Transfer To : " + ddlSchemeName.SelectedItem.Text;

            ((Label) lstFromSchemeReg.FindControl("lblFromSchNameReg")).Text = "Transfer From (Regular STP) : " +
                                                                               ddlSchemeNameTrnFrm.SelectedItem.Text;
            ((Label) lstRegular.FindControl("lblToSchSwpReg")).Text = "Transfer To (Regular STP) : " +
                                                                      ddlSchemeName.SelectedItem.Text;

            Div_FromSchemeReg.Visible = true;
            div_FromScheme.Visible = true;
            div_Swp.Visible = true;
            div_regular.Visible = true;
            divCal.Visible = true;
            tcResults.Visible = true;
        }

        public string shortDate(DateTime dateTime)
        {
            return dateTime.ToString("d");
        }
    }
}