using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Data.OleDb;
using AjaxControlToolkit;
using inout = System.IO;
using System.Data.SqlClient;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Net;

namespace iFrames.Chart
{
    public partial class CompanyPAT_ShareChart : System.Web.UI.Page
    {
        #region: Global Variable

        #region: Anonymous Global Variable

        string filepath = string.Empty;
        Chart objChart = new Chart();
        ArrayList arr = new ArrayList();

        #endregion

        #endregion

        #region: Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            gvCompanyShare.PageIndex = 0;
            pnlCompanyShare.Visible = false;
            pnlgvCompanyShare.Visible = false;
            lblMsg.Text = string.Empty;
            lblMsgError.Text = string.Empty;
            lblCompName.Text = string.Empty;



            if (!IsPostBack)
            {
                objChart.FillDropdown(ddlCompany);
                string pdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\Sensex_EPS_Share.pdf";
                if (File.Exists(pdfpath))
                {
                    File.Delete(pdfpath);
                }
                ViewState["selectedval"] = "Sensex";
                OnloadShowChart();
            }

        }
        #endregion

        #region: Button Event
        protected void btnShow_Click(object sender, EventArgs args)
        {
            ShowChart();

        }
        #endregion

        #region: LinkButton Events

        protected void lnkbtnDownload_Click(object sender, EventArgs e)
        {
            string path = string.Empty;

            //*******pdf convertion******
            //_SimpleConversion();
            //***************************

            if (ddlCompany.SelectedIndex != 0)
            {
                //path = Server.MapPath("~") + "\\Chart\\cache_pdf\\" + ddlCompany.SelectedItem.Text.Trim() + "_EPS_Share.pdf";
                //Response.Redirect("ChartDownload.aspx?path=" + path + "");
                Response.Redirect("ChartDownload.aspx?filename=" + ddlCompany.SelectedItem.Text.Trim() + "_EPS_Share.pdf&cname=" + ddlCompany.SelectedItem.Text.Trim() + "&opt=EPS_Share");

            }
            else
            {
                //path = Server.MapPath("~") + "\\Chart\\cache_pdf\\Sensex_EPS_Share.pdf";
                //Response.Redirect("ChartDownload.aspx?path=" + path + "");
                string cmpsensex = string.Empty;
                cmpsensex = "Sensex";
                Response.Redirect("ChartDownload.aspx?filename=" + cmpsensex + "_EPS_Share.pdf&cname=" + cmpsensex + "&opt=EPS_Share");
            }

        }

        #endregion

        #region: Chart Method
        private void CreateChartForCompEPSShare(DataTable _dt, DataTable _dtorg)
        {

            string CAGR_EPS = string.Empty;
            string CAGR_Mcap = string.Empty;
            string CAGR_SharePrice = string.Empty;
            try
            {
                tdCompName.BgColor = "#007AC2";
                tdLegend.Visible = true;
                tdLegend.BgColor = "#FBFDFE";
                tdEps.BgColor = "#50B000";
                tdShare.BgColor = "#FF0000";

                if (ddlCompany.SelectedItem.Value != "0")
                {
                    lblCompName.Text = ddlCompany.SelectedItem.Text + " " + "EPS Vs Share Price";
                }
                else
                {
                    lblCompName.Text = "Sensex" + " " + "EPS Vs Sensex";
                }


                // ........Providing rebased value as datatable...............
                DataTable dt = objChart.CalculateEPSRebasePer(_dtorg);

                // .............Providing rebased value as datatable..............
                //DataTable dt = objChart.CalculatePATRebasePer(_dt);

                if (dt.Rows.Count > 0)
                {
                    CAGR_EPS = Convert.ToString(dt.Rows[0]["CAGR_EPS"]) + "%";
                    CAGR_Mcap = Convert.ToString(dt.Rows[0]["CAGR_Mcap"]) + "%";
                    CAGR_SharePrice = Convert.ToString(dt.Rows[0]["CAGR_SharePrice"]) + "%";
                }

                DataTable dtFinal = new DataTable();
                dtFinal.Columns.Add("FY_End", typeof(System.DateTime));
                dtFinal.Columns.Add("EPS", typeof(System.Double));
                dtFinal.Columns.Add("SharePrice", typeof(System.Double));
                foreach (DataRow dr in _dt.Rows)
                {
                    DataRow dr1 = dtFinal.NewRow();
                    dr1["FY_End"] = Convert.ToDateTime(dr["FY_End"]);
                    dr1["EPS"] = dr["EPS"];
                    dr1["SharePrice"] = dr["SharePrice"];
                    dtFinal.Rows.Add(dr1);
                }
                lblEps.Text = CAGR_EPS;
                lblShare.Text = CAGR_SharePrice;

                arr.Add(CAGR_EPS);
                arr.Add(CAGR_SharePrice);

                BindToDataTable(dtFinal, "FY_End", chrtCompanyShare, CAGR_EPS, CAGR_Mcap, CAGR_SharePrice);
                chrtCompanyShare.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chrtCompanyShare.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chrtCompanyShare.ChartAreas[0].AxisX2.MajorGrid.Enabled = false;
                chrtCompanyShare.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                if (ddlCompany.SelectedIndex != 0)
                {
                    chrtCompanyShare.ChartAreas[0].AxisY.Title = "Share Price";
                    chrtCompanyShare.ChartAreas[0].AxisY2.Title = "EPS";
                }
                else
                {
                    chrtCompanyShare.ChartAreas[0].AxisY.Title = "Sensex";
                    chrtCompanyShare.ChartAreas[0].AxisY2.Title = "Sensex EPS";
                }

                pnlCompanyShare.Visible = true;

                chrtCompanyShare.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8F5FC");
                chrtCompanyShare.BackGradientStyle = GradientStyle.Center;
                chrtCompanyShare.IsSoftShadows = true;

                //Save Image
                string OrgChartImagePath = string.Empty;
                if (ddlCompany.SelectedIndex != 0)
                {
                    OrgChartImagePath = Server.MapPath("images/" + ddlCompany.SelectedItem.Text.Trim() + "_EPS_Share.Jpeg");
                    if (inout.File.Exists(OrgChartImagePath))
                    {
                        inout.File.Delete(OrgChartImagePath);
                    }
                }
                else
                {
                    OrgChartImagePath = Server.MapPath("images/Sensex_EPS_Share.Jpeg");
                    if (inout.File.Exists(OrgChartImagePath))
                    {
                        inout.File.Delete(OrgChartImagePath);
                    }
                }
                chrtCompanyShare.SaveImage(OrgChartImagePath, ChartImageFormat.Jpeg);

                //End


            }
            catch (Exception ex)
            {


            }
        }
        private Title BuildChartTitle(string _title)
        {
            Title title = new Title()
            {
                Docking = Docking.Top,
                Font = new System.Drawing.Font("Trebuchet MS", 10.0f, FontStyle.Bold),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#067DCD"),
                Alignment = System.Drawing.ContentAlignment.TopLeft
            };
            CustomizeChartTitle(title, _title);
            return title;
        }
        private void CustomizeChartTitle(Title title, string charttitle)
        {
            title.Text = ddlCompany.SelectedItem.Text + " " + charttitle;
        }
        public void BindToDataTable(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart MainChart, string CAGR_EPS, string CAGR_Mcap, string CAGR_SharePrice)
        {

            MainChart.ChartAreas[0].AxisX.Minimum = _dt.Rows.Cast<DataRow>().Select(x => Convert.ToDateTime(x["FY_End"])).Min().ToOADate();
            MainChart.ChartAreas[0].AxisX.Maximum = _dt.Rows.Cast<DataRow>().Select(x => Convert.ToDateTime(x["FY_End"])).Max().AddMonths(4).ToOADate();


            //List<double> levelEPS = _dt.AsEnumerable().Select(al => al.Field<double>("EPS")).Distinct().ToList();
            //double maxEPS = levelEPS.Max();
            //List<double> levelSharePrice = _dt.AsEnumerable().Select(al => al.Field<double>("SharePrice")).Distinct().ToList();
            //double maxlevelSharePrice = levelSharePrice.Max();

            //if (maxEPS > maxlevelSharePrice)
            //{
            //    MainChart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(0);
            //    double maxvalue = Math.Round(maxEPS);
            //    double finlmaxvalue = 0.0;
            //    Int32 devidn = Convert.ToInt32(maxvalue / 5);
            //    finlmaxvalue = (devidn + 11) * 5;
            //    MainChart.ChartAreas[0].AxisY.Maximum = finlmaxvalue;
            //}
            //else
            //{
            //    MainChart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(0);
            //    double maxvalue = Math.Round(maxlevelSharePrice);
            //    double finlmaxvalue = 0.0;
            //    Int32 devidn = Convert.ToInt32(maxvalue / 5);
            //    finlmaxvalue = (devidn + 11) * 5;
            //    MainChart.ChartAreas[0].AxisY.Maximum = finlmaxvalue;

            //}
            // MainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;

            MainChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            MainChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;

            MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
            MainChart.ChartAreas[0].AxisX.LabelStyle.Format = Convert.ToDateTime(_dt.Rows[0][0]).ToString("MMM").Replace("M", "\\M").Replace("y", "\\y") + "-yyyy";

            MainChart.Series.Clear();
            foreach (DataColumn dc in _dt.Columns)
            {
                if (dc.ColumnName == xField)
                    continue;
                MainChart.Series.Add(dc.ColumnName);
                MainChart.Series[dc.ColumnName].ChartType = SeriesChartType.Line;

                if (dc.ColumnName == "EPS")
                {
                    MainChart.Series[dc.ColumnName].YAxisType = AxisType.Secondary;
                    MainChart.Series[dc.ColumnName].LegendText = "EPS";
                    MainChart.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
                    //MainChart.Series[dc.ColumnName].Color = System.Drawing.Color.Green;
                }
                else
                {
                    MainChart.Series[dc.ColumnName].YAxisType = AxisType.Primary;
                    MainChart.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                    //MainChart.Series[dc.ColumnName].Color = System.Drawing.Color.Red;
                    MainChart.Series[dc.ColumnName].LegendText = "Share Price";
                }
                MainChart.Series[dc.ColumnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, dc.ColumnName);
                MainChart.Series[dc.ColumnName].BorderWidth = 1;
            }
            //formatChart(_dt, CAGR_EPS, CAGR_Mcap, CAGR_SharePrice);
            //MainChart.Legends[0].Enabled = true;
        }
        void formatChart(DataTable _dt, string CAGR_EPS, string CAGR_Mcap, string CAGR_SharePrice)
        {
            LegendItem item = new LegendItem();
            item.SeparatorColor = System.Drawing.Color.White;
            item.Name = "lgnchtArea";
            item.BorderWidth = 4;
            item.ShadowOffset = 1;

            LegendCell EPSText = new LegendCell();
            EPSText.Alignment = ContentAlignment.MiddleCenter;
            EPSText.CellType = LegendCellType.Text;
            EPSText.Name = "Cell1";
            EPSText.Text = CAGR_EPS;
            //PATText.BackColor = System.Drawing.ColorTranslator.FromHtml("#007AC2");
            EPSText.BackColor = System.Drawing.Color.Green;
            EPSText.ForeColor = System.Drawing.Color.White;
            EPSText.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            EPSText.Margins.Top = 20;
            EPSText.Margins.Bottom = 20;
            item.Cells.Add(LegendCellType.Text, "     ", ContentAlignment.MiddleCenter);
            item.Cells.Add(EPSText);

            chrtCompanyShare.Legends[0].CustomItems.Add(item);

            LegendItem item2 = new LegendItem();
            item2.SeparatorColor = System.Drawing.Color.White;
            item2.Name = "lgnchtArea3";
            item2.BorderWidth = 4;
            item2.ShadowOffset = 1;

            LegendCell seperator = new LegendCell();
            seperator.CellType = LegendCellType.Image;
            seperator.Alignment = ContentAlignment.MiddleCenter;
            seperator.Name = "Cell2";
            seperator.ForeColor = System.Drawing.Color.Black;
            if (inout.File.Exists(Server.MapPath("~/Chart/images/CompPatShare.jpeg")))
                inout.File.Delete(Server.MapPath("~/Chart/images/CompPatShare.jpeg"));
            objChart.AddWatermark(_dt.Rows.Count.ToString() + " Years", "CompPatShare.jpeg");
            seperator.Image = "~/Chart/images/CompPatShare.jpeg";
            Size sz = new Size();
            sz.Width = 800;
            sz.Height = 150;
            seperator.ImageSize = sz;
            item2.Cells.Add(seperator);
            chrtCompanyShare.Legends[0].CustomItems.Add(item2);

            LegendItem item1 = new LegendItem();
            item1.SeparatorColor = System.Drawing.Color.White;
            item1.Name = "lgnchtArea2";
            item1.BorderWidth = 4;
            item1.ShadowOffset = 1;

            LegendCell SharepText = new LegendCell();
            SharepText.Alignment = ContentAlignment.MiddleCenter;
            SharepText.CellType = LegendCellType.Text;
            SharepText.Name = "Cell3";
            SharepText.Text = CAGR_SharePrice;
            //SharepText.BackColor = System.Drawing.ColorTranslator.FromHtml("#00A79D");
            SharepText.BackColor = System.Drawing.Color.Red;
            SharepText.ForeColor = System.Drawing.Color.White;
            SharepText.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            SharepText.Margins.Top = 15;
            SharepText.Margins.Bottom = 15;
            item1.Cells.Add(SharepText);
            chrtCompanyShare.Legends[0].CustomItems.Add(item1);
        }
        #endregion

        #region: Dropdown Event
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlCompanyShare.Visible = false;
            pnlgvCompanyShare.Visible = false;
            tdLegend.Visible = false;
            tdCompName.BgColor = "";

            //********************** Delete Previous Files from Cache Folder****************************
            if (ViewState["selectedval"] != null)
            {
                string compname = string.Empty;
                compname = Convert.ToString(ViewState["selectedval"]);
                string pdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + compname + "_EPS_Share.pdf";
                if (File.Exists(pdfpath))
                {
                    File.Delete(pdfpath);
                }
                if (ddlCompany.SelectedIndex != 0)
                {
                    ViewState["selectedval"] = ddlCompany.SelectedItem.Text;
                }
                else
                {
                    ViewState["selectedval"] = "Sensex";
                }
            }
            else
            {
                if (ddlCompany.SelectedIndex != 0)
                {
                    ViewState["selectedval"] = ddlCompany.SelectedItem.Text;
                }
                else
                {
                    ViewState["selectedval"] = "Sensex";
                }
            }
            //*******************************Enbd************************************************************

            ShowChart();
        }
        #endregion

        #region: GridView Event
        protected void gvCompanyShare_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCompanyShare.PageIndex = e.NewPageIndex;
            pnlgvCompanyShare.Visible = true;
            string comp = string.Empty;
            DataTable dtAfterRebase = new DataTable();
            if (ddlCompany.SelectedItem.Value != "0")
            {
                comp = ddlCompany.SelectedItem.Text;
            }
            else
            {
                comp = "Sensex";
            }
            DataTable dtChart = objChart.FetchCompanyEPS(comp);
            if (dtChart.Rows.Count > 0)
            {
                dtAfterRebase = objChart.CalculateRebase(dtChart);
            }
            if (dtAfterRebase.Rows.Count > 0)
            {
                CreateChartForCompEPSShare(dtAfterRebase, dtChart);
                if (dtChart.Rows.Count > 0)
                {
                    DataTable dtorg = objChart.CalGrowth(dtChart);
                    gvCompanyShare.Columns[5].HeaderText = (ddlCompany.SelectedIndex == 0) ? "Sensex" : "Share Price";
                    gvCompanyShare.Columns[6].HeaderText = (ddlCompany.SelectedIndex == 0) ? "YoY Sensex Growth (%)" : "YoY Share Price Growth (%)";
                    gvCompanyShare.Columns[1].HeaderText = (ddlCompany.SelectedIndex == 0) ? "Sensex EPS" : "EPS";
                    gvCompanyShare.DataSource = dtorg;
                    gvCompanyShare.DataBind();
                }

            }
        }
        #endregion

        #region: Method

        #region: Fill Methods
        public void FillDropdownCompany()
        {
            objChart.FillDropdown(ddlCompany);

        }
        #endregion

        #region: Show Chart
        public void ShowChart()
        {
            DataTable dtAfterRebase = new DataTable();
            string company = string.Empty;

            if (Request.QueryString["company"] != null)
            {
                company = Convert.ToString(Request.QueryString["company"]);
            }
            else
            {
                if (ddlCompany.SelectedItem.Text != "-Select Company-")
                {
                    company = objChart.HandleQuate(ddlCompany.SelectedItem.Text);
                }
                else
                {
                    company = "Sensex";
                }
            }


            DataTable dtChart = objChart.FetchCompanyEPS(company);
            if (dtChart.Rows.Count > 0)
            {
                List<System.DateTime> levelEPS = dtChart.AsEnumerable().Select(al => al.Field<System.DateTime>("FY_End")).Distinct().ToList();
                DateTime minyear = levelEPS.Min();
                if (minyear.Month.ToString().Length == 1)
                {
                    lblGrowth.Text = "Compound Annualized Growth Rate since " + "0" + minyear.Month.ToString() + "/" + minyear.Year.ToString();
                    arr.Add(lblGrowth.Text);
                }
                else
                {
                    lblGrowth.Text = " Compound Annualized Growth Rate since " + minyear.Month.ToString() + "/" + minyear.Year.ToString();
                    arr.Add(lblGrowth.Text);
                }

                dtAfterRebase = objChart.CalculateRebase(dtChart);
            }


            if (dtAfterRebase.Rows.Count > 0)
            {
                CreateChartForCompEPSShare(dtAfterRebase, dtChart);
                pnlgvCompanyShare.Visible = true;
            }
            else
            {
                lblMsg.Text = "There is no record.";
                pnlCompanyShare.Visible = false;
            }


            if (dtChart.Rows.Count > 0)
            {
                pnlgvCompanyShare.Visible = true;
                DataTable dtorg = objChart.CalGrowth(dtChart);
                gvCompanyShare.DataSource = dtorg;
                gvCompanyShare.Columns[5].HeaderText = (ddlCompany.SelectedIndex == 0) ? "Sensex" : "Share Price";
                gvCompanyShare.Columns[6].HeaderText = (ddlCompany.SelectedIndex == 0) ? "YoY Sensex Growth (%)" : "YoY Share Price Growth (%)";
                gvCompanyShare.Columns[1].HeaderText = (ddlCompany.SelectedIndex == 0) ? "Sensex EPS" : "EPS";
                gvCompanyShare.DataBind();

                //string DirPath = string.Empty;
                //DirPath = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf");
                //string[] filePaths = Directory.GetFiles(DirPath);
                //foreach (string filePath in filePaths)
                //    File.Delete(filePath);

                if (ddlCompany.SelectedIndex != 0)
                {
                    //objChart.CreateHTML(dtorg, ddlCompany.SelectedItem.Text.Trim(), txtDistributor.Text.Trim(), arr, "EPS_Share");
                    objChart.CreateHTML(gvCompanyShare, ddlCompany.SelectedItem.Text.Trim(), txtDistributor.Text.Trim(), arr, "EPS_Share");
                    //objChart._SimpleConversion("EPS_Share", ddlCompany.SelectedItem.Text.Trim());
                }
                else
                {
                    //objChart.CreateHTML(dtorg, "Sensex", txtDistributor.Text.Trim(), arr, "EPS_Share");
                    objChart.CreateHTML(gvCompanyShare, "Sensex", txtDistributor.Text.Trim(), arr, "EPS_Share");
                    //objChart._SimpleConversion("EPS_Share", "Sensex");
                }
            }
            else
            {
                lblMsg.Text = "There is no record.";
                pnlCompanyShare.Visible = false;
            }


        }
        public void OnloadShowChart()
        {
            DataTable dtAfterRebase = new DataTable();
            string company = string.Empty;

            if (Request.QueryString["company"] != null)
            {
                company = Convert.ToString(Request.QueryString["company"]);
            }
            else
            {
                company = "Sensex";
            }

            DataTable dtChart = objChart.FetchCompanyEPS(company);
            if (dtChart.Rows.Count > 0)
            {
                List<System.DateTime> levelEPS = dtChart.AsEnumerable().Select(al => al.Field<System.DateTime>("FY_End")).Distinct().ToList();
                DateTime minyear = levelEPS.Min();
                if (minyear.Month.ToString().Length == 1)
                {
                    lblGrowth.Text = "Compound Annualized Growth Rate since " + "0" + minyear.Month.ToString() + "/" + minyear.Year.ToString();
                    arr.Add(lblGrowth.Text);
                }
                else
                {
                    lblGrowth.Text = "Compound Annualized Growth Rate since " + minyear.Month.ToString() + "/" + minyear.Year.ToString();
                    arr.Add(lblGrowth.Text);
                }

                dtAfterRebase = objChart.CalculateRebase(dtChart);
            }


            if (dtAfterRebase.Rows.Count > 0)
            {
                CreateChartForCompEPSShare(dtAfterRebase, dtChart);
                pnlgvCompanyShare.Visible = true;
            }
            else
            {
                lblMsg.Text = "There is no record.";
                pnlCompanyShare.Visible = false;
            }


            if (dtChart.Rows.Count > 0)
            {
                pnlgvCompanyShare.Visible = true;
                DataTable dtorg = objChart.CalGrowth(dtChart);
                gvCompanyShare.Columns[5].HeaderText = (ddlCompany.SelectedIndex == 0) ? "Sensex" : "Share Price";
                gvCompanyShare.Columns[6].HeaderText = (ddlCompany.SelectedIndex == 0) ? "YoY Sensex Growth (%)" : "YoY Share Price Growth (%)";
                gvCompanyShare.Columns[1].HeaderText = (ddlCompany.SelectedIndex == 0) ? "Sensex EPS" : "EPS";
                gvCompanyShare.DataSource = dtorg;
                gvCompanyShare.DataBind();

                objChart.CreateHTML(gvCompanyShare, "Sensex", txtDistributor.Text.Trim(), arr, "EPS_Share");
                //objChart._SimpleConversion("EPS_Share", "Sensex");


            }
            else
            {
                lblMsg.Text = "There is no record.";
                pnlCompanyShare.Visible = false;
            }

        }
        #endregion

        #region: Anonymous
        public string FomatDate(object date)
        {
            string res = string.Empty;
            try
            {
                res = Convert.ToDateTime(date).ToString("yyyy");

            }
            catch (Exception ex)
            {


            }
            return res;
        }
        public string CheckBlank(object value)
        {
            string res = string.Empty;
            try
            {
                if (Convert.ToString(value) == "0.00")
                {
                    res = string.Empty;

                }
                else if (Convert.ToString(value) == "-1.00")
                {
                    res = "0.00";
                }

                else
                {
                    res = Convert.ToString(value);
                }
            }
            catch (Exception ex)
            {


            }
            return res;

        }
        #endregion

        #endregion

        #region: VerifyRenderingInServerForm(Require to pass gridview control)

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the
            // specified ASP.NET server control at run time.
            // No code required here.
            return;
        }

        #endregion

    }

}



