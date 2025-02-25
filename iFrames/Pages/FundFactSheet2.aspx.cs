using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;

namespace iFrames.Pages
{
    public partial class FundFactSheet2 : MyBasePage
    {
        public string navRs, ind_name, ind_val, MaxNav, MinNav, schCode, shortName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                schCode = Request.QueryString["sch"];
                DataTable dt = FactSheets.getFactsheet2(schCode);
                //DataTable dt = AllMethods.getFactsheet2("PI017");
                foreach (DataRow dr in dt.Rows)
                {
                    shortName = dr["short_name"].ToString();
                    navRs = dr["navRs"] == null ? "NA" : Math.Round(Convert.ToDecimal(dr["navRs"]),2).ToString() + " as on " + Convert.ToDateTime(dr["Date"].ToString()).ToString("MMM dd, yyyy");
                    ind_name = dr["ind_name"] == null ? "" : " - " + dr["ind_name"];
                    ind_val = dr["ind_val"] == null ? "NA" : Math.Round(Convert.ToDecimal(dr["ind_val"]), 2).ToString() + " as on " + Convert.ToDateTime(dr["indRecDt"].ToString()).ToString("MMM dd, yyyy");
                    MaxNav = Convert.ToDouble(dr["MaxNav"]) == 0 ? dr["MaxNav"].ToString() : Math.Round(Convert.ToDecimal(dr["MaxNav"]),2).ToString() + " as on " + Convert.ToDateTime(dr["MaxNavDate"].ToString()).ToString("MMM dd, yyyy");
                    MinNav = Convert.ToDouble(dr["MinNav"]) == 0 ? dr["MinNav"].ToString() : Math.Round(Convert.ToDecimal(dr["MinNav"]),2).ToString() + " as on " + Convert.ToDateTime(dr["MinNavDate"].ToString()).ToString("MMM dd, yyyy");
                }
                dt = Nav.GetdtHistNav(schCode, DateTime.Today.AddDays(-30), DateTime.Today);
                dt.Columns.Remove("Sch_code");
                dt.Columns["NAV_rs"].ColumnName = "Nav";
                BindToDataTable(dt, "Date", Chart1);
            }
        }

        public void BindToDataTable(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart MainChart)
        {
            MainChart.ChartAreas[0].AxisY.Minimum = 10;
            MainChart.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
            MainChart.Series.Clear();
            foreach (DataColumn dc in _dt.Columns)
            {
                if (dc.ColumnName == xField)
                    continue;
                MainChart.Series.Add(dc.ColumnName);
                MainChart.Series[dc.ColumnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, dc.ColumnName);
                MainChart.Series[dc.ColumnName].IsValueShownAsLabel = true;
                MainChart.Series[dc.ColumnName].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                //MainChart.Legends[0].Enabled = true;
            }
            foreach (System.Web.UI.DataVisualization.Charting.Series s in MainChart.Series)
            {
                s["LabelStyle"] = "Auto";
            }
        }

    }
}