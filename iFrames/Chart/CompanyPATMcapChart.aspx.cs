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


namespace iFrames.Chart
{
    public partial class CompanyPATMcapChart : System.Web.UI.Page
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
            pnlChart.Visible = false;
            lblMsg.Text = string.Empty;
            lblMsgError.Text = string.Empty;
            lblCompName.Text = string.Empty;

            if (!IsPostBack)
            {
                objChart.FillDropdown(ddlCompany);
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
                //path = Server.MapPath("~") + "\\Chart\\cache_pdf\\" + ddlCompany.SelectedItem.Text.Trim() + "_EPS_Mcap.pdf";
                //Response.Redirect("ChartDownload.aspx?path=" + path + "");
                Response.Redirect("ChartDownload.aspx?filename=" + ddlCompany.SelectedItem.Text.Trim() + "_EPS_Mcap.pdf&cname=" + ddlCompany.SelectedItem.Text.Trim() + "&opt=EPS_Mcap");
                
            }

        }

        #endregion

        #region: Chart Method
        private void CreateChartForCompEPSMcap(DataTable _dt, DataTable _dtorg)
        {
            string CAGR_EPS = string.Empty;
            string CAGR_Mcap = string.Empty;
            string CAGR_SharePrice = string.Empty;
            DateTime FYEndDate = System.DateTime.Now;
            try
            {
                tdCompName.BgColor = "#007AC2";
                tdLegend.Visible = true;
                tdLegend.BgColor = "#FBFDFE";
                tdEps.BgColor = "#50B000";
                tdMcap.BgColor = "#FF0000";

                lblCompName.Text = ddlCompany.SelectedItem.Text + " " + "EPS Vs Market Capital";
                DataTable dt = objChart.CalculateEPSRebasePer(_dtorg);
                if (dt.Rows.Count > 0)
                {
                    CAGR_EPS = Convert.ToString(dt.Rows[0]["CAGR_EPS"]) + "%";
                    CAGR_Mcap = Convert.ToString(dt.Rows[0]["CAGR_Mcap"]) + "%";
                    CAGR_SharePrice = Convert.ToString(dt.Rows[0]["CAGR_SharePrice"]) + "%";
                }
                DataTable dtFinal = new DataTable();
                dtFinal.Columns.Add("FY_End", typeof(System.DateTime));
                dtFinal.Columns.Add("EPS", typeof(System.Double));
                dtFinal.Columns.Add("M_Cap", typeof(System.Double));
                foreach (DataRow dr in _dt.Rows)
                {
                    DataRow dr1 = dtFinal.NewRow();
                    dr1["FY_End"] = Convert.ToDateTime(dr["FY_End"]);
                    dr1["EPS"] = dr["EPS"];
                    dr1["M_Cap"] = dr["M_Cap"];
                    dtFinal.Rows.Add(dr1);
                }
                lblEps.Text = CAGR_EPS;
                lblMcap.Text = CAGR_Mcap;

                arr.Add(CAGR_EPS);
                arr.Add(CAGR_Mcap);


                BindToDataTable(dtFinal, "FY_End", chrtCompanyPrice, CAGR_EPS, CAGR_Mcap);

                pnlChart.Visible = true;
                chrtCompanyPrice.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chrtCompanyPrice.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chrtCompanyPrice.ChartAreas[0].AxisX2.MajorGrid.Enabled = false;
                chrtCompanyPrice.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                chrtCompanyPrice.ChartAreas[0].AxisY.Title = "M Cap";
                chrtCompanyPrice.ChartAreas[0].AxisY2.Title = "EPS";

                chrtCompanyPrice.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8F5FC");
                chrtCompanyPrice.BackGradientStyle = GradientStyle.Center;
                chrtCompanyPrice.IsSoftShadows = true;

                //Save Image
                string OrgChartImagePath = string.Empty;
                OrgChartImagePath = Server.MapPath("images/" + ddlCompany.Text.Trim() + "_EPS_Mcap.Jpeg");
                if (inout.File.Exists(OrgChartImagePath))
                {
                    inout.File.Delete(OrgChartImagePath);
                }
                chrtCompanyPrice.SaveImage(OrgChartImagePath, ChartImageFormat.Jpeg);
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
                Font = new Font("Trebuchet MS", 10.0f, FontStyle.Bold),
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
        public void BindToDataTable(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart MainChart, string CAGR_EPS, string CAGR_Mcap)
        {
            try
            {
                MainChart.ChartAreas[0].AxisX.Minimum = _dt.Rows.Cast<DataRow>().Select(x => Convert.ToDateTime(x["FY_End"])).Min().ToOADate();
                MainChart.ChartAreas[0].AxisX.Maximum = _dt.Rows.Cast<DataRow>().Select(x => Convert.ToDateTime(x["FY_End"])).Max().AddMonths(4).ToOADate();


                //List<double> levelEPS = _dt.AsEnumerable().Select(al => al.Field<double>("EPS")).Distinct().ToList();
                //double maxEPS = levelEPS.Max();
                //List<double> levelMCap = _dt.AsEnumerable().Select(al => al.Field<double>("M_Cap")).Distinct().ToList();
                //double maxMcap = levelMCap.Max();

                //if (maxEPS > maxMcap)
                //{

                //    MainChart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(0);
                //    double maxvalue = Math.Round(maxEPS);
                //    double finlmaxvalue = 0.0;
                //    Int32 devidn = Convert.ToInt32(maxvalue / 5);
                //    finlmaxvalue = (devidn + 11) * 5;
                //    MainChart.ChartAreas[0].AxisY.Maximum = finlmaxvalue;
                //    MainChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;


                //}
                //else
                //{
                //    MainChart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(0);
                //    double maxvalue = Math.Round(maxMcap);
                //    double finlmaxvalue = 0.0;
                //    Int32 devidn = Convert.ToInt32(maxvalue / 5);
                //    finlmaxvalue = (devidn + 11) * 5;
                //    MainChart.ChartAreas[0].AxisY.Maximum = finlmaxvalue;
                //    MainChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                //}

                MainChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                MainChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;

                //MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
                //MainChart.ChartAreas[0].AxisX.LabelStyle.Format = "MMM-yyyy";

                MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
                MainChart.ChartAreas[0].AxisX.LabelStyle.Format = Convert.ToDateTime(_dt.Rows[0][0]).ToString("MMM").Replace("M", "\\M") + "-yyyy";


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
                        // MainChart.Series[dc.ColumnName].LegendText = "PAT";
                        MainChart.Series[dc.ColumnName].LegendText = "EPS";
                        MainChart.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#50B000");
                        //  MainChart.Series[dc.ColumnName].Color = System.Drawing.Color.Green;
                    }
                    else
                    {
                        MainChart.Series[dc.ColumnName].YAxisType = AxisType.Primary;
                        MainChart.Series[dc.ColumnName].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                        // MainChart.Series[dc.ColumnName].Color = System.Drawing.Color.Red;
                        MainChart.Series[dc.ColumnName].LegendText = "M Cap";
                    }
                    //MainChart.Series[dc.ColumnName].Color = (dc.ColumnName == "PAT") ? System.Drawing.ColorTranslator.FromHtml("#007AC2") : System.Drawing.ColorTranslator.FromHtml("#00A79D");
                    MainChart.Series[dc.ColumnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, dc.ColumnName);
                    MainChart.Series[dc.ColumnName].BorderWidth = 1;
                }
                //formatChart(_dt, CAGR_EPS, CAGR_Mcap);
                //MainChart.Legends[0].Enabled = true;
            }
            catch (Exception ex)
            {


            }
        }
        void formatChart(DataTable _dt, string CAGR_EPS, string CAGR_Mcap)
        {
            LegendItem item = new LegendItem();
            item.SeparatorColor = Color.White;
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
            EPSText.ForeColor = Color.White;
            EPSText.Font = new Font("Verdana", 8, FontStyle.Bold);
            EPSText.Margins.Top = 20;
            EPSText.Margins.Bottom = 20;
            item.Cells.Add(LegendCellType.Text, "     ", ContentAlignment.MiddleCenter);
            item.Cells.Add(EPSText);
            chrtCompanyPrice.Legends[0].CustomItems.Add(item);

            LegendItem item2 = new LegendItem();
            item2.SeparatorColor = Color.White;
            item2.Name = "lgnchtArea3";
            item2.BorderWidth = 4;
            item2.ShadowOffset = 1;

            LegendCell seperator = new LegendCell();
            seperator.CellType = LegendCellType.Image;
            seperator.Alignment = ContentAlignment.MiddleCenter;
            seperator.Name = "Cell2";
            seperator.ForeColor = Color.Black;
            if (inout.File.Exists(Server.MapPath("~/Chart/images/CompPatMcap.jpeg")))
                inout.File.Delete(Server.MapPath("~/Chart/images/CompPatMcap.jpeg"));
            objChart.AddWatermark(_dt.Rows.Count.ToString() + " Years", "CompPatMcap.jpeg");
            seperator.Image = "~/Chart/images/CompPatMcap.jpeg";
            Size sz = new Size();
            sz.Width = 600;
            sz.Height = 150;
            seperator.ImageSize = sz;
            item2.Cells.Add(seperator);
            chrtCompanyPrice.Legends[0].CustomItems.Add(item2);

            LegendItem item1 = new LegendItem();
            item1.SeparatorColor = Color.White;
            item1.Name = "lgnchtArea2";
            item1.BorderWidth = 4;
            item1.ShadowOffset = 1;

            LegendCell MCapText = new LegendCell();
            MCapText.Alignment = ContentAlignment.MiddleCenter;
            MCapText.CellType = LegendCellType.Text;
            MCapText.Name = "Cell3";
            MCapText.Text = CAGR_Mcap;
            //MCapText.BackColor = System.Drawing.ColorTranslator.FromHtml("#00A79D");
            MCapText.BackColor = System.Drawing.Color.Red;
            MCapText.ForeColor = Color.White;
            MCapText.Font = new Font("Verdana", 8, FontStyle.Bold);
            MCapText.Margins.Top = 15;
            MCapText.Margins.Bottom = 15;
            item1.Cells.Add(MCapText);
            chrtCompanyPrice.Legends[0].CustomItems.Add(item1);
        }
        #endregion

        #region: Dropdown Event
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlChart.Visible = false;
            pnlgvCompanyShare.Visible = false;
            tdLegend.Visible = false;
            tdCompName.BgColor = "";
            if (ddlCompany.SelectedIndex != 0)
            {
                lnkbtnDownload.Visible = true;
            }
            else
            {
                lnkbtnDownload.Visible = false;
            }


            //********************** Delete Previous Files from Cache Folder****************************
            if (ViewState["selectedval"] != null)
            {
                string compname = string.Empty;
                compname = Convert.ToString(ViewState["selectedval"]);
                string pdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + compname + "_EPS_Mcap.pdf";
                if (File.Exists(pdfpath))
                {
                    File.Delete(pdfpath);
                }
                if (ddlCompany.SelectedIndex != 0)
                {
                    ViewState["selectedval"] = ddlCompany.SelectedItem.Text;
                }
            }
            else
            {
                if (ddlCompany.SelectedIndex != 0)
                {
                    ViewState["selectedval"] = ddlCompany.SelectedItem.Text;
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
            DataTable dtChart = objChart.FetchCompanyEPS(ddlCompany.SelectedItem.Text);
            DataTable dtAfterRebase = objChart.CalculateRebase(dtChart);
            if (dtAfterRebase.Rows.Count > 0)
            {
                CreateChartForCompEPSMcap(dtAfterRebase, dtChart);
                if (dtChart.Rows.Count > 0)
                {
                    DataTable dtorg = objChart.CalGrowth(dtChart);
                    gvCompanyShare.DataSource = dtorg;
                    gvCompanyShare.DataBind();
                }

            }
        }
        #endregion

        #region: Method

        #region: Fill Method

        public void FillDropdownCompany()
        {
            try
            {
                DataTable dt = objChart.FetchCompany();
                if (dt.Rows.Count > 0)
                {
                    ddlCompany.DataSource = dt;
                    ddlCompany.DataTextField = "symbol";
                    ddlCompany.DataValueField = "c_code";
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem("-Select Company-", "0"));
                    ddlCompany.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {


            }
        }

        #endregion

        #region: Anonymous Methods

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

        #region: Show Chart

        public void ShowChart()
        {
            DataTable dtAfterRebase = null;
            if (ddlCompany.SelectedItem.Text != "-Select Company-")
            {
                DataTable dtChart = objChart.FetchCompanyEPS(objChart.HandleQuate(ddlCompany.SelectedItem.Text));
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

                }
                dtAfterRebase = objChart.CalculateRebase(dtChart);

                if (dtAfterRebase.Rows.Count > 0)
                {
                    CreateChartForCompEPSMcap(dtAfterRebase, dtChart);
                    pnlgvCompanyShare.Visible = true;
                }
                else
                {
                    lblMsg.Text = "There is no record.";
                    pnlChart.Visible = false;
                }

                if (dtChart.Rows.Count > 0)
                {
                    pnlgvCompanyShare.Visible = true;
                    DataTable dtorg = objChart.CalGrowth(dtChart);
                    gvCompanyShare.DataSource = dtorg;
                    gvCompanyShare.DataBind();

                    //PDF Creation Start
                    if (ddlCompany.SelectedIndex != 0)
                    {
                        objChart.CreateHTML(gvCompanyShare, ddlCompany.SelectedItem.Text.Trim(), txtDistributor.Text.Trim(), arr, "EPS_Mcap");
                        //var temp = objChart._SimpleConversion("EPS_Mcap", ddlCompany.SelectedItem.Text.Trim());
                        //if (temp != null)
                        //{
                        //    Session["pdfcontent"] = temp;
                        //    this.Response.Clear();
                        //    this.Response.Write("Byte count: " + temp.Length.ToString());
                        //    //Response.Clear();
                        //    //this.Response.ContentType = "application/pdf";
                        //    //this.Response.AddHeader("content-disposition", "attachment;filename=" + "temp.pdf");
                        //    //this.Response.BinaryWrite(temp);
                        //    this.Response.End();
                        //    this.Response.Flush();
                        //    this.Response.Close();
                        //}

                    }
                    //End
                }
                else
                {
                    lblMsg.Text = "There is no record.";
                    pnlChart.Visible = false;

                }

            }
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
