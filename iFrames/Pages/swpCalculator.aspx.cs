using System;
using System.Linq;
using System.Xml.Linq;
using iFrames.BLL;
using System.Data;

namespace iFrames.Pages
{
    public partial class swpCalculator : MyBasePage
    {
        DataTable _dtScheme;
        DataTable _dtBenchMark;
        DataTable _dtNavIndx;
        DataSet _ds;
        DateTime _fromDate;
        DateTime _toDate;
        DateTime _calDate;
        DateTime _lastSipDate;
        DateTime _asOnDate;
        double _asOnDateNav;
        int _startIndex;

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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (new DateTime(Convert.ToInt16(txtfrmDate.Text.Split('/')[2]), Convert.ToInt16(txtfrmDate.Text.Split('/')[1]), Convert.ToInt16(txtfrmDate.Text.Split('/')[0])).Day == Convert.ToInt32(ddlShipDate.SelectedValue))
                {
                    Response.Write("<script>alert(\"'From day’ Can't be equal to the 'SWP day' ('" + ddlShipDate.SelectedValue + "')\")</script>");
                    return;
                }

                #region  Declaration
                _toDate = new DateTime(Convert.ToInt16(txtToDate.Text.Split('/')[2]), Convert.ToInt16(txtToDate.Text.Split('/')[1]), Convert.ToInt16(txtToDate.Text.Split('/')[0]));
                _fromDate = new DateTime(Convert.ToInt16(txtfrmDate.Text.Split('/')[2]), Convert.ToInt16(txtfrmDate.Text.Split('/')[1]), Convert.ToInt16(txtfrmDate.Text.Split('/')[0]));
                _asOnDate = new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]), Convert.ToInt16(txtAsOn.Text.Split('/')[1]), Convert.ToInt16(txtAsOn.Text.Split('/')[0]));
                _dtScheme = (DataTable)Session["dtScheme"];

                _dtNavIndx = new DataTable();
                _dtNavIndx.Columns.Add("Date", typeof(DateTime));
                _dtNavIndx.Columns.Add("Nav_Rs", typeof(double));
                _dtNavIndx.Columns.Add("ind_val", typeof(double));

                _dtNavIndx = new DataTable();
                _dtNavIndx.Columns.Add("Date", typeof(DateTime));
                _dtNavIndx.Columns.Add("Nav_Rs", typeof(double));
                _dtNavIndx.Columns.Add("ind_val", typeof(double));
                #endregion

                var lastNavDate = Convert.ToDateTime(Schemes.GetLastNav(ddlSchemeName.SelectedValue)).AddDays(276);
                var firstNavDate = Convert.ToDateTime(Schemes.GetFirstNav(ddlSchemeName.SelectedValue)).AddDays(276);
                if (firstNavDate > _toDate)
                {
                    Response.Write("'To date' can not be less than the 'issue date' of the Scheme");
                    return;
                }
                if (firstNavDate > _fromDate)
                    _fromDate = firstNavDate;

                var dtAllNav = Schemes.GetAllNav(ddlSchemeName.SelectedValue);
                var drdtAsOnNav = dtAllNav.Select("Date<='" + _asOnDate.AddDays(-276) + "'");
                _asOnDateNav = (Convert.ToDouble(drdtAsOnNav[drdtAsOnNav.Length - 1]["Nav_Rs"].ToString()) - 53) / 76;
                _dtNavIndx.Rows.Clear();

                #region only for Quarterly calculation
                if (ddlPeriod.SelectedValue == "Quarterly")
                {
                    var qm = firstNavDate.Month;
                    int[] calMonth = { qm, qm + 3 > 12 ? qm + 3 - 12 : qm + 3, qm + 6 > 12 ? qm + 6 - 12 : qm + 6, qm + 9 > 12 ? qm + 9 - 12 : qm + 9 };
                    var flag = 0;
                    while (flag == 0)
                    {
                        foreach (var kk in calMonth)
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
                    if (_toDate.Day > Convert.ToInt32(ddlShipDate.SelectedValue))
                        _calDate = _toDate.AddDays(-(_toDate.Day - Convert.ToInt32(ddlShipDate.SelectedValue)));
                    else if (_toDate.Day < Convert.ToInt32(ddlShipDate.SelectedValue))
                        _calDate = _toDate.AddMonths(-1).AddDays(Convert.ToInt32(ddlShipDate.SelectedValue) - _toDate.Day);
                    else
                        _calDate = _toDate;
                }
                else
                    _calDate = _toDate;

                var month = _calDate.Month;
                DataRow[] dtdrNav;
                while (_calDate >= _fromDate)
                {
                    if (_calDate.Date > DateTime.Today)
                        _calDate = lastNavDate;
                    if (_lastSipDate == _calDate)
                        break;
                    dtdrNav = dtAllNav.Select("Date='" + _calDate.AddDays(-276) + "'");
                    if (dtdrNav.Length > 0)
                    {
                        var dataRow = _dtNavIndx.NewRow();
                        dataRow["Date"] = _calDate;
                        dataRow["Nav_Rs"] = (Convert.ToDouble(dtdrNav[0][0].ToString()) - 53) / 76;
                        _dtNavIndx.Rows.Add(dataRow);
                        _lastSipDate = _calDate;
                        if ((_calDate.Month == _fromDate.Month) && (_calDate.Year == _fromDate.Year))
                            break;
                        if (ddlPeriod.SelectedValue != "Daily")
                        {
                            if (_calDate.Day != Convert.ToInt32(ddlShipDate.SelectedValue))
                                _calDate = month == _calDate.Month
                                              ? _calDate.AddDays(Convert.ToInt32(ddlShipDate.SelectedValue) -
                                                                _calDate.Day)
                                              : _calDate.AddDays(Convert.ToInt32(ddlShipDate.SelectedValue) -
                                                                _calDate.Day).AddMonths(-1);
                        }
                        if (ddlPeriod.SelectedValue == "Monthly")
                            _calDate = _calDate.AddMonths(-1);
                        if (ddlPeriod.SelectedValue == "Quarterly")
                            _calDate = _calDate.AddMonths(-3);
                        if (ddlPeriod.SelectedValue == "Weekly")
                            _calDate = _calDate.AddDays(-7);
                        if (ddlPeriod.SelectedValue == "Daily")
                            _calDate = _calDate.AddDays(-1);
                        month = _calDate.Month;
                        if ((_calDate.Month == _fromDate.Month) && (_calDate.Year == _fromDate.Year) &&
                            _calDate < _fromDate) break;
                    }
                    else
                        _calDate = ddlPeriod.SelectedValue != "Daily" ? _calDate.AddDays(1) : _calDate.AddDays(-1);
                }
                _startIndex = _dtNavIndx.Rows.Count - 1;

                #region for first nav insertion
                var drFirstNavIndex = _dtNavIndx.NewRow();
                dtdrNav = dtAllNav.Select("Date>='" + _fromDate.AddDays(-276) + "'", "Date desc");
                drFirstNavIndex["Date"] = _fromDate;
                drFirstNavIndex["Nav_Rs"] = (Convert.ToDouble(dtdrNav[dtdrNav.Length - 1]["Nav_Rs"].ToString()) - 53) / 76;
                _dtNavIndx.Rows.Add(drFirstNavIndex);
                #endregion

                GetCalculator(Convert.ToDecimal(txtTranAmt.Text));
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert(\"-- opps ('a generic error happened, please contact administrator.')\")</script>");
            }
        }

        public void GetCalculator(decimal installedAmt)
        {
            var matchIndexReg = 0;
            _dtNavIndx.Columns.Add("Unit", typeof(double));
            _dtNavIndx.Columns.Add("InvestAmount", typeof(double));
            _dtNavIndx.Columns.Add("CumulativeUnits", typeof(double));
            _dtNavIndx.Columns.Add("CumulativeAmount", typeof(double));
            _dtNavIndx.Columns.Add("CashFlow", typeof(double));

            _dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["CashFlow"] = Convert.ToDecimal(txtinstallAmt.Text);
            _dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["InvestAmount"] = Convert.ToDecimal(txtinstallAmt.Text);
            _dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["Unit"] = Math.Round(Convert.ToDecimal(txtinstallAmt.Text) /
              Convert.ToDecimal(_dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["Nav_Rs"]), 4);

            _dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["CumulativeUnits"] = Math.Round(Convert.ToDecimal(txtinstallAmt.Text) /
              Convert.ToDecimal(_dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["Nav_Rs"]), 4);

            _dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["CumulativeAmount"] = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["Nav_Rs"]) *
                Convert.ToDecimal(_dtNavIndx.Rows[_dtNavIndx.Rows.Count - 1]["Unit"]), 2);
            decimal cashFlow = 0;
            for (var i = _startIndex; i >= 0; i--)
            {
                _dtNavIndx.Rows[i]["CashFlow"] = Convert.ToDecimal(txtTranAmt.Text);
                _dtNavIndx.Rows[i]["InvestAmount"] = Math.Round(Convert.ToDecimal(txtTranAmt.Text), 4);
                cashFlow = cashFlow + Convert.ToDecimal(_dtNavIndx.Rows[i]["CashFlow"]);

                _dtNavIndx.Rows[i]["Unit"] = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[i]["CashFlow"]) /
                                                  Convert.ToDecimal(_dtNavIndx.Rows[i]["Nav_Rs"]), 4);

                _dtNavIndx.Rows[i]["CumulativeUnits"] = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[i + 1]["CumulativeUnits"]) -
                                                           Convert.ToDecimal(_dtNavIndx.Rows[i]["Unit"]), 4);

                _dtNavIndx.Rows[i]["CumulativeAmount"] = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[i]["Nav_Rs"]) *
                                                            Convert.ToDecimal(_dtNavIndx.Rows[i]["CumulativeUnits"]), 2);

                if (Convert.ToDecimal(_dtNavIndx.Rows[i]["CumulativeAmount"]) > Convert.ToDecimal(txtTranAmt.Text))
                    continue;
                matchIndexReg = i;
                break;
            }

            #region As on date Value calculation
            var dr = _dtNavIndx.NewRow();
            dr["Date"] = new DateTime(Convert.ToInt16(txtAsOn.Text.Split('/')[2]), Convert.ToInt16(txtAsOn.Text.Split('/')[1]), Convert.ToInt16(txtAsOn.Text.Split('/')[0]));
            dr["Nav_Rs"] = _asOnDateNav;
            dr["CumulativeUnits"] = Math.Round(Convert.ToDecimal(_dtNavIndx.Rows[matchIndexReg]["CumulativeUnits"]), 4);
            dr["CumulativeAmount"] = Math.Round(_asOnDateNav * Convert.ToDouble(_dtNavIndx.Rows[matchIndexReg]["CumulativeUnits"]), 2);
            dr["CashFlow"] = Math.Round(_asOnDateNav * Convert.ToDouble(_dtNavIndx.Rows[matchIndexReg]["CumulativeUnits"]), 4);
            _dtNavIndx.Rows.Add(dr);
            #endregion

            var viewNavIndx = new DataView(_dtNavIndx) {RowFilter = "CumulativeUnits is not null", Sort = "Date"};
            LstSWP.DataSource = viewNavIndx;
            LstSWP.DataBind();
        }

        public void xmlReader(string myXmLfile)
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
            var myXmLfile = Server.MapPath("../App_Data/SWP.xml");
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

        protected void ddwscname_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBenchMark.SelectedValue = Schemes.GetInd(ddlSchemeName.SelectedValue).Rows[0][0].ToString();
            var xdoc = XElement.Load(Server.MapPath("../App_Data/SWP.xml"));
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