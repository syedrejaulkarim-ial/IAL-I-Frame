using System;
using System.Linq;
using System.Xml.Linq;
using iFrames.BLL;
using System.Data;

namespace iFrames.Pages
{
    public partial class sipCalculator : MyBasePage
    {
        DataTable _dtScheme;
        DataTable _dtBenchMark;
        DataTable _dtNavIndx;
        DataTable _dtAllNav;
        DataTable _dtAllIndex;
        DataSet _ds;
        DateTime _fromDate;
        DateTime _toDate;
        DateTime _calDate;
        DataView _viewCal;
        DateTime _asOnDate;
        double _asOnDateNav;
        double _asOnDateIndex;
        double _currentValue;
        double _finalIndexVal;
        double _indXirr;
        readonly DataTable _dtReturn = new DataTable();

        public string shortDate(DateTime dateTime)
        {
            return dateTime.ToString("d");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            fillScheme();
            fillBenchMark();
        }

        public void xmlReader(string myXmLfile)
        {
            _ds = new DataSet();
            var fsReadXml = new System.IO.FileStream
                (myXmLfile, System.IO.FileMode.Open);
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
            var myXmLfile = Server.MapPath("../App_Data/SIP.xml");
            xmlReader(myXmLfile);
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {


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
                _dtNavIndx.Columns.Add("Units", typeof (double));
                _dtNavIndx.Columns.Add("Cash_Flow", typeof (double));
                _dtNavIndx.Columns.Add("Amount", typeof (double));
                _dtNavIndx.Columns.Add("SIP_Value", typeof (double));
                _dtNavIndx.Columns.Add("ind_unit", typeof (double));
                _dtNavIndx.Columns.Add("ind_val", typeof (double));

                #region for Return

                _dtReturn.Columns.Add("Period", typeof (string));
                _dtReturn.Columns.Add("SipStartDate", typeof (string));
                _dtReturn.Columns.Add("TotAmtInvest", typeof (double));
                _dtReturn.Columns.Add("SchMarketVal", typeof (double));
                _dtReturn.Columns.Add("SchSipRtn", typeof (string));
                _dtReturn.Columns.Add("BenchMarkVal", typeof (double));
                _dtReturn.Columns.Add("BenchMarkSipRtn", typeof (string));

                #endregion

                string[] duration = {"dateRange", "1", "3", "5", "Issue"};
                var firstNavDate = Convert.ToDateTime(Schemes.GetFirstNav(ddlSchemeName.SelectedValue)).AddDays(276);
                if (firstNavDate > _toDate)
                    Response.Write(
                        "<script>alert(\"-- 'To date can not be less than the 'issue date' of the Scheme)\")</script>");

                _dtAllNav = Schemes.GetAllNav(ddlSchemeName.SelectedValue);
                _dtAllIndex = Schemes.GetAllInd(ddlBenchMark.SelectedValue);

                var drdtAsOnNav = _dtAllNav.Select("Date<='" + _asOnDate.AddDays(-276) + "'");
                var drdtdAsOnIndex = _dtAllIndex.Select("dt1<='" + _asOnDate.AddDays(-276) + "'");
                _asOnDateNav = (Convert.ToDouble(drdtAsOnNav[drdtAsOnNav.Length - 1]["Nav_Rs"].ToString()) - 53)/76;
                _asOnDateIndex = (Convert.ToDouble(drdtdAsOnIndex[drdtdAsOnIndex.Length - 1]["ind_val"].ToString()) - 53)/
                                 76;

                foreach (var strDuration in duration)
                {
                    _dtNavIndx.Rows.Clear();
                    switch (strDuration)
                    {
                        case "Issue":
                            _fromDate = firstNavDate;
                            break;
                        case "dateRange":
                            _fromDate =
                                new DateTime(Convert.ToInt16(txtfrmDate.Text.Split('/')[2]),
                                             Convert.ToInt16(txtfrmDate.Text.Split('/')[1]),
                                             Convert.ToInt16(txtfrmDate.Text.Split('/')[0])) < firstNavDate
                                    ? firstNavDate
                                    : new DateTime(Convert.ToInt16(txtfrmDate.Text.Split('/')[2]),
                                                   Convert.ToInt16(txtfrmDate.Text.Split('/')[1]),
                                                   Convert.ToInt16(txtfrmDate.Text.Split('/')[0]));
                            break;
                        default:
                            _fromDate = _toDate.AddYears(-Convert.ToInt32(strDuration));
                            if (_fromDate < firstNavDate)
                                _fromDate = firstNavDate;
                            break;
                    }

                    #region only for Quaterly calculation

                    if (ddlPeriod.SelectedValue == "Quaterly")
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

                    //_calDate = _toDate.Day > Convert.ToInt32(ddlShipDate.SelectedValue)
                    //               ? _toDate.AddDays(-(_toDate.Day - Convert.ToInt32(ddlShipDate.SelectedValue)))
                    //               : _toDate.AddMonths(-1).AddDays(Convert.ToInt32(ddlShipDate.SelectedValue) -
                    //                                               _toDate.Day);

                    _calDate = _toDate.Day >= Convert.ToInt32(ddlShipDate.SelectedValue)
                                 ? _toDate.AddDays(-(_toDate.Day - Convert.ToInt32(ddlShipDate.SelectedValue)))
                                 : _toDate.AddMonths(-1).AddDays(Convert.ToInt32(ddlShipDate.SelectedValue) -
                                                                 _toDate.Day);

                    var month = _calDate.Month;
                    DataRow[] dtdrNav;
                    DataRow[] dtdrInd;
                    while (_calDate >= _fromDate)
                    {
                        dtdrNav = _dtAllNav.Select("Date='" + _calDate.AddDays(-276) + "'");
                        dtdrInd = _dtAllIndex.Select("dt1='" + _calDate.AddDays(-276) + "'");
                        if (dtdrNav.Length > 0)
                        {
                            var dataRow = _dtNavIndx.NewRow();
                            var nav = (Convert.ToDouble(dtdrNav[0][0].ToString()) - 53)/76;
                            dataRow["Date"] = _calDate;
                            dataRow["Nav_Rs"] = nav;

                            if (dtdrInd.Length > 0)
                                dataRow["ind_val"] = (Convert.ToDouble(dtdrInd[0][0].ToString()) - 53)/76;

                            _dtNavIndx.Rows.Add(dataRow);
                            //If the 'CalDate' is exist the 'from date' the 'while' loop will be terminated
                            if ((_calDate.Month == _fromDate.Month) && (_calDate.Year == _fromDate.Year))
                                break;
                            if (_calDate.Day != Convert.ToInt32(ddlShipDate.SelectedValue))
                                _calDate = month == _calDate.Month
                                               ? _calDate.AddDays(Convert.ToInt32(ddlShipDate.SelectedValue) -
                                                                  _calDate.Day)
                                               : _calDate.AddDays(Convert.ToInt32(ddlShipDate.SelectedValue) -
                                                                  _calDate.Day)
                                                     .AddMonths(-1);
                            if (ddlPeriod.SelectedValue == "Monthly")
                                _calDate = _calDate.AddMonths(-1);
                            if (ddlPeriod.SelectedValue == "Quaterly")
                                _calDate = _calDate.AddMonths(-3);
                            month = _calDate.Month;
                            //For extract the first Nav of the Partiqular scheme for the given duration
                            //if ((_calDate.Month == _fromDate.Month) && (_calDate.Year == _fromDate.Year))
                            //    _calDate = _fromDate;
                        }
                        else
                            _calDate = _calDate.AddDays(1);
                    }
                    getCalculator(strDuration);
                }
                lstReturn.DataSource = _dtReturn;
                lstReturn.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert(\"--opps ('a generic error happened, please contact administrator.')\")</script>");
            }
        }

        public void getCalculator(string strDuration)
        {
            #region Index Rebase
            var firstIndexVal = Convert.ToDouble(_dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["ind_val"]);
            for (var i = 0; i < _dtNavIndx.Rows.Count; i++)
                _dtNavIndx.Rows[i]["ind_val"] = Convert.ToDouble(_dtNavIndx.Rows[i]["ind_val"]) * 10 / firstIndexVal;
            #endregion

            double tempUnit = 0;
            double calInd = 0;
            for (var i = _dtNavIndx.Rows.Count - 1; i >= 0; i--)
            {
                _dtNavIndx.Rows[i]["Cash_Flow"] = -Convert.ToDouble(txtinstallAmt.Text);
                if (i == _dtNavIndx.Rows.Count - 1)
                {
                    _dtNavIndx.Rows[i]["Units"] = Math.Round(-(Convert.ToDouble(_dtNavIndx.Rows[i]["Cash_Flow"]) / Convert.ToDouble(_dtNavIndx.Rows[i]["Nav_Rs"])), 4);
                    _dtNavIndx.Rows[i]["SIP_Value"] = txtinstallAmt.Text;
                    _dtNavIndx.Rows[i]["Amount"] = txtinstallAmt.Text;
                }
                else
                {
                    _dtNavIndx.Rows[i]["Units"] = Math.Round(-(Convert.ToDouble(_dtNavIndx.Rows[i]["Cash_Flow"]) / Convert.ToDouble(_dtNavIndx.Rows[i]["Nav_Rs"])) + Convert.ToDouble(_dtNavIndx.Rows[i + 1]["Units"]), 4);
                    _dtNavIndx.Rows[i]["SIP_Value"] = Math.Round(Convert.ToDouble(_dtNavIndx.Rows[i]["Nav_Rs"]) * Convert.ToDouble(_dtNavIndx.Rows[i]["Units"]), 2);
                    _dtNavIndx.Rows[i]["Amount"] = Convert.ToDouble(_dtNavIndx.Rows[i + 1]["Amount"]) + Convert.ToDouble(txtinstallAmt.Text);
                }
                //Index Value Calculation
                if (Convert.ToString(_dtNavIndx.Rows[i]["ind_val"]) == "")
                {
                    if (tempUnit == 0)
                        _dtNavIndx.Rows[i]["ind_unit"] = tempUnit;
                    else
                    {
                        tempUnit = Convert.ToDouble(tempUnit + (Convert.ToDouble(txtinstallAmt.Text) / calInd));
                        _dtNavIndx.Rows[i]["ind_unit"] = Math.Round(tempUnit, 4);
                    }
                }
                else
                {
                    if (tempUnit == 0)
                        tempUnit = Convert.ToDouble(tempUnit + (Convert.ToDouble(txtinstallAmt.Text) / (Convert.ToDouble(_dtNavIndx.Rows[i]["ind_val"]))));
                    else
                    {
                        tempUnit = Convert.ToDouble(tempUnit + (Convert.ToDouble(txtinstallAmt.Text) / (Convert.ToDouble(_dtNavIndx.Rows[i]["ind_val"]))));
                        calInd = Convert.ToDouble(_dtNavIndx.Rows[i]["ind_val"]);
                    }
                    _dtNavIndx.Rows[i]["ind_unit"] = Math.Round(tempUnit, 4);
                }
            }

            var dates = _dtNavIndx.Rows.Cast<DataRow>().Select(r => Convert.ToDateTime(r["Date"])).ToList();
            var values = _dtNavIndx.Rows.Cast<DataRow>().Select(r => Convert.ToDouble(r["Cash_Flow"])).ToList();
            _currentValue = Math.Round(Convert.ToDouble(_dtNavIndx.Rows[0]["Units"]) * _asOnDateNav, 4);
            _finalIndexVal = Math.Round(Convert.ToDouble(_dtNavIndx.Rows[0]["ind_unit"]) * _asOnDateIndex * 10 / firstIndexVal, 4);

            #region For As on Date calculated row
            var drAson = _dtNavIndx.NewRow();
            drAson["Date"] = new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]), Convert.ToInt16(txtAsOn.Text.Split('/')[1]), Convert.ToInt16(txtAsOn.Text.Split('/')[0]));
            drAson["Nav_Rs"] = _asOnDateNav;
            drAson["Cash_Flow"] = Math.Round(_currentValue,2);
            _dtNavIndx.Rows.Add(drAson);
            #endregion

            _viewCal = new DataView(_dtNavIndx) {Sort = "Date"};
            if (strDuration == "1" || strDuration == "3" || strDuration == "5" || strDuration == "Issue")
            {
                var drRtn = _dtReturn.NewRow();
                values.Insert(0, _currentValue);
                dates.Insert(0, new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]), Convert.ToInt16(txtAsOn.Text.Split('/')[1]), Convert.ToInt16(txtAsOn.Text.Split('/')[0])));
                drRtn["SchMarketVal"] = Math.Round(_currentValue,2);
                drRtn["SchSipRtn"] = Math.Round(Utilities.XIRR(values.ToArray(), dates.ToArray()) * 100, 4) + " %";
                values[0] = _finalIndexVal;
                _indXirr = Utilities.XIRR(values.ToArray(), dates.ToArray());
                drRtn["Period"] = (strDuration == "1" || strDuration == "3" || strDuration == "5")
                                      ? strDuration + " Years"
                                      : "Since Inception";
                drRtn["SipStartDate"] = dates.Min();
                drRtn["TotAmtInvest"] = Convert.ToDouble(_dtNavIndx.Rows[0]["Amount"].ToString());
                drRtn["BenchMarkVal"] = Math.Round(_finalIndexVal, 2);
                drRtn["BenchMarkSipRtn"] = Math.Round(_indXirr * 100, 4) + " %";
                _dtReturn.Rows.Add(drRtn);
            }
            else if (strDuration == "dateRange")
            {
                LstSipCal_DateRange.DataSource = _viewCal;
                LstSipCal_DateRange.DataBind();
                lblInitialValue_Range.Text = _viewCal.Table.Rows[0][4].ToString();
                lblCurrentValue_Range.Text = _currentValue.ToString();
                values.Insert(0, _currentValue);
                dates.Insert(0, new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]), Convert.ToInt16(txtAsOn.Text.Split('/')[1]), Convert.ToInt16(txtAsOn.Text.Split('/')[0])));
                lblReturnValue_Range.Text = Math.Round(Utilities.XIRR(values.ToArray(), dates.ToArray()) * 100, 4) + " %";
                divReturnValue_Range.Visible = true;
                values[0] = _finalIndexVal;
                lblBenchMrkRtn.Text = Math.Round(Utilities.XIRR(values.ToArray(), dates.ToArray()) * 100, 4) + " %";
            }
        }

        protected void ddlSchemeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSchemeName.SelectedIndex <= 1) return;
            ddlBenchMark.SelectedValue = Schemes.GetInd(ddlSchemeName.SelectedValue).Rows[0][0].ToString();
            var xdoc = XElement.Load(Server.MapPath("../App_Data/SIP.xml"));
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
    }
}