using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using iFrames.BLL;
using System.Drawing;
using System.Configuration;

namespace iFrames.DSPApp
{
    public partial class Viewreport : System.Web.UI.Page
    {
        Boolean Flag;

        [System.Web.Services.WebMethod]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                // EventDate.Value = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                EventDate.Value = iFrames.BLL.DSPAppUpload.GetDate();
                // EventDate.Value = 

                FillCategory();

                if (Convert.ToBoolean(Session["IsAdmin"]) == false)
                {
                    liUserMngmnt.Style.Add("display", "none");
                    liUploadExl.Style.Add("display", "none");
                }
                if (HttpContext.Current.Request.Browser.IsMobileDevice)
                {
                    dgViewStatus.AllowPaging = true;
                    dgViewStatus.AllowSorting = true;
                    dgViewStatus.PageSize = 7;
                }
            }

            btnSearch.Attributes.Add("onclick", "javascript:return validate();");

            //if (HttpContext.Current.Request.Browser.IsMobileDevice)
            //dgViewStatus.Style.Add("width", "2000");
            //else
            //   dgViewStatus.Style.Add("width", "100%");
            ddlperiod.Style.Add("width", "100%");
            ddlCategory.Style.Add("width", "100%");

        }




        protected void FillCategory()
        {
            ddlCategory.Items.Clear();
            DataTable dtCat = iFrames.BLL.DSPAppUpload.GetCategory();
            if (dtCat.Rows.Count > 0)
            {
                // dtCat.Rows.Add("Summary");
                System.Data.DataRow dr = dtCat.NewRow();
                dr[0] = "--- Select Category----";
                dtCat.Rows.InsertAt(dr, 0);
                dr = dtCat.NewRow();
                dr[0] = "Summary";
                dtCat.Rows.InsertAt(dr, 1);
                dr = dtCat.NewRow();
                dr[0] = "Summary Direct";
                dtCat.Rows.InsertAt(dr, 2);
            }

            ddlCategory.DataSource = dtCat;

            ddlCategory.DataTextField = "SHEET_NAME";

            ddlCategory.DataBind();

        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            ddlperiod.Items.Clear();
            //string a = iFrames.BLL.DSPAppUpload.GetPeriod(ddlCategory.SelectedItem.Text.ToString());
            //string[] period = a.Split(',');
            GridView1.DataSource = null;
            GridView1.Visible = false;
            dgViewStatus.DataSource = null;
            dgViewStatus.Visible = false;
            GridViewSummary.DataSource = null;
            GridViewSummary.Visible = false;
            lblrptDate.Text = "";

            if (EventDate.Value == "")
            {
                Response.Write("<script LANGUAGE='JavaScript' >alert('Please select Event Date.')</script>");

                return;
            }


            if (ddlCategory.SelectedItem.Text != "Summary" && ddlCategory.SelectedItem.Text != "Summary Direct" && ddlCategory.SelectedItem.Text != "--- Select Category----")
            {
                string a = iFrames.BLL.DSPAppUpload.GetPeriod(ddlCategory.SelectedItem.Text.ToString(), Convert.ToDateTime(EventDate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));

                if (a != null)
                {
                    string[] period = a.Split(',');

                    //foreach (string word in period)
                    //{
                    //    ddlperiod.Items.Add(word);   

                    ddlperiod.DataSource = period;
                    ddlperiod.DataBind();
                }

                else { Response.Write("<script LANGUAGE='JavaScript' >alert('Period not found for selected date and category.')</script>"); }
            }
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {

            lblrptDate.Text = "Report as on " + EventDate.Value;
            string hdnValue = hidSelectedReturnPeriod.Value;
            //added by sop
            string[] selectedPeridData = hdnValue.Split(',');

            string Flag = "N";

            dgViewStatus.DataSource = null;
            dgViewStatus.Visible = false;
            GridViewSummary.DataSource = null;
            GridViewSummary.Visible = false;
            if (ddlCategory.SelectedItem.Text == "Summary" || ddlCategory.SelectedItem.Text == "Summary Direct")
            {
                #region summary
                DataTable dt = iFrames.BLL.DSPAppUpload.GetSummary(ddlCategory.SelectedItem.Text.ToString(), Convert.ToDateTime(EventDate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
                if (dt != null && dt.Rows.Count > 0)
                {
                    GridViewSummary.Visible = true;
                    GridViewSummary.DataSource = dt;
                    GridViewSummary.DataBind();


                    Flag = "Y";
                }
                //syed
                dgViewStatus.Style.Add("width", Convert.ToString((dt.Columns.Count * 70) + 200) + "px");
                #endregion

            }
            else
            {
                #region Non Summary
                string columnname = "Scheme_id, a.[Scheme Name],case when ISNUMERIC([AUM])= 1  then replace(convert(varchar,cast(CAST(cast([AUM] as numeric(38,0)) AS VARCHAR) as money),1), '.00','') ELSE [AUM] END [AUM (in Cr)], CASE WHEN NAV = '0' THEN '--'  when ISNUMERIC([NAV])= 1  then CAST(cast([NAV] as numeric(38,2)) AS VARCHAR) END NAV";
                string StrStdCol = "";
                string allCol = "";


                //foreach (ListItem listItem in ddlperiod.Items)
                //{
                //    if (listItem.Selected == true)
                //    {
                //        var val = listItem.Value;
                //        var txt = listItem.Text;
                //    }
                //}



                ////Comented by sop
                //for (int i = 0; i < ddlperiod.Items.Count; i++)
                //{
                //    if (ddlperiod.Items[i].Selected)
                //    {
                //        StrStdCol += ",CASE WHEN [" + ddlperiod.Items[i].Value.Trim() + "] = '0' THEN '--' when ISNUMERIC(Replace([" + ddlperiod.Items[i].Value.Trim() + "],'/',''))= 1  then cast(cast(Replace([" + ddlperiod.Items[i].Value.Trim() + "],'/','') as numeric(38,4)) AS varchar(100))+'%' END [" + ddlperiod.Items[i].Value.Trim() + "]," + "[Rank " + ddlperiod.Items[i].Value.Trim() + "]";
                //    }

                //    //  allCol += ",Replace([" + ddlperiod.Items[i].Value.Trim() + "],'/','%')  [" + ddlperiod.Items[i].Value.Trim() + "] ," + "[Rank " + ddlperiod.Items[i].Value.Trim() + "]";

                //    allCol += ", CASE WHEN [" + ddlperiod.Items[i].Value.Trim() + "] = '0' THEN '--' when ISNUMERIC(Replace([" + ddlperiod.Items[i].Value.Trim() + "],'/',''))= 1  then cast(cast(Replace([" + ddlperiod.Items[i].Value.Trim() + "],'/','') as numeric(38,4)) AS varchar(100))+'%' END  [" + ddlperiod.Items[i].Value.Trim() + "] ," + "[Rank " + ddlperiod.Items[i].Value.Trim() + "]";

                //}
                ////end
                for (int i = 0; i < selectedPeridData.Length; i++)
                {

                    if (selectedPeridData[i].Trim() != "")
                    {

                        StrStdCol += ",CASE WHEN [" + selectedPeridData[i].Trim() + "] = '0' THEN '--' when ISNUMERIC(Replace([" + selectedPeridData[i].Trim() + "],'/',''))= 1  then cast(cast(Replace([" + selectedPeridData[i].Trim() + "],'/','') as numeric(38,2)) AS varchar(100))+'%' END [" + selectedPeridData[i].Trim() + "]," + "cast([Rank " + selectedPeridData[i].Trim() + "]  as numeric(38,0)) [Rank " + selectedPeridData[i].Trim() + "]";

                    }

                    // StrStdCol += ",CASE WHEN [" + selectedPeridData[i].Trim() + "] = '0' THEN '--' when ISNUMERIC(Replace([" + selectedPeridData[i].Trim() + "],'/',''))= 1  then cast(cast(Replace([" + selectedPeridData[i].Trim() + "],'/','') as numeric(38,4)) AS varchar(100))+'%' END [" + selectedPeridData[i].Trim() + "]," + "[Rank " + selectedPeridData[i].Trim() + "]";
                    //  allCol += ",Replace([" + ddlperiod.Items[i].Value.Trim() + "],'/','%')  [" + ddlperiod.Items[i].Value.Trim() + "] ," + "[Rank " + ddlperiod.Items[i].Value.Trim() + "]";
                }
                if (StrStdCol != "")
                {
                    columnname = columnname + StrStdCol;
                    //columnname = columnname + allCol;
                }
                else
                {
                    Response.Write("<script LANGUAGE='JavaScript' >alert('Please select atleast one Period.')</script>");
                    return;
                }


                DataTable dt = iFrames.BLL.DSPAppUpload.GetRecards(columnname, ddlCategory.SelectedItem.Text.ToString(), Convert.ToDateTime(EventDate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
                if (dt != null && dt.Rows.Count > 0)
                {
                    dgViewStatus.Visible = true;

                    if (Request.Browser.IsMobileDevice)
                    {
                        DataTable dtDspSchemes = dt.Clone();
                        DataTable dtOtherSchemes = dt.Clone();
                        DataTable dtIndex = dt.Clone();
                        bool indexFound = false;
                        string schemeName = "";
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            schemeName = dt.Rows[row]["Scheme Name"].ToString().ToUpper();
                            if (schemeName.StartsWith("DSP") && indexFound == false)
                            {
                                dtDspSchemes.ImportRow(dt.Rows[row]);
                            }
                            else if (schemeName == "BENCHMARK INDEX" || indexFound == true)
                            {
                                indexFound = true;
                                dtIndex.ImportRow(dt.Rows[row]);
                            }
                            else
                            {
                                dtOtherSchemes.ImportRow(dt.Rows[row]);
                            }
                        }
                        Cache["DspSchemes"] = dtDspSchemes.Copy();
                        Cache["OtherSchemes"] = dtOtherSchemes.Copy();
                        Cache["Index"] = dtIndex.Copy();
                        ViewState["SortExpression"] = null;
                        ViewState["SortDirection"] = null;
                        
                        dt = preparePageData();
                    }

                    dgViewStatus.PageIndex = 0;
                    dgViewStatus.DataSource = dt;
                    dgViewStatus.DataBind();
                    Cache["ViewReport"] = dt.Copy();
                    Flag = "Y";

                    SetFactsheetURL();

                    dgViewStatus.Style.Add("width", Convert.ToString((dt.Columns.Count * 70) + 600 >= 1950 ? 1950 : (dt.Columns.Count * 70) + 400) + "px");
                    //dgViewStatus.Style.Add("width", "100%");
                    HDGrdColCount.Value = Convert.ToString(dt.Columns.Count);

                }


                DataTable dt1 = iFrames.BLL.DSPAppUpload.GetRecards1(columnname, ddlCategory.SelectedItem.Text.ToString(), Convert.ToDateTime(EventDate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
                GridView1.DataSource = null;
                GridView1.Visible = false;

                if (dt1 != null && dt1.Rows.Count > 1)
                {
                    GridView1.Visible = true;
                    Flag = "Y";

                    dt1.Columns.Add("Change", typeof(System.String));
                    dt1.Columns.Add("BPS", typeof(System.String));
                    double val1 = 0;
                    double val2 = 0;
                    double val3 = 0;

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            //  dt1.Rows[i][3] = "Change";
                            // dt1.Rows[i][1] = (dt1.Rows[i][1]).Replace

                            dt1.Rows[i][1] = (Convert.ToDateTime(dt1.Rows[i][1]).ToString("dd-MMM-yyyy"));
                            dt1.Rows[i][2] = (Convert.ToDateTime(dt1.Rows[i][2]).ToString("dd-MMM-yyyy"));

                        }
                        else
                        {

                            double number;
                            if (double.TryParse(dt1.Rows[i][1].ToString(), out number))
                            {
                                val1 = Convert.ToDouble(dt1.Rows[i][1].ToString());

                                val2 = Convert.ToDouble(dt1.Rows[i][2].ToString());
                                val3 = (val1 - val2) * 100;
                                dt1.Rows[i][3] = Math.Round(val3, 2);
                                dt1.Rows[i][4] = "Bps";

                                dt1.Rows[i][1] = Math.Round(Convert.ToDouble(dt1.Rows[i][1].ToString()), 4);
                                dt1.Rows[i][2] = Math.Round(Convert.ToDouble(dt1.Rows[i][2].ToString()), 4);
                            }
                        }
                    }


                    for (int j = 0; j < dt1.Columns.Count; j++)
                    {
                        string columnName;
                        columnName = dt1.Columns[j].ColumnName;
                        if (columnName.Contains("SCHEME NAME") == true)
                        {
                            dt1.Columns[j].ColumnName = "Other Info";
                        }
                        if (columnName.Contains("AUM") == true)
                        { dt1.Columns[j].ColumnName = "Date1"; }

                        if (columnName.Contains("Expense Ratio(Latest)") == true)
                        { dt1.Columns[j].ColumnName = "Date2"; }

                    }

                    dt1.AcceptChanges();
                    GridView1.DataSource = dt1;
                    GridView1.DataBind();
                }
                GridView1.Style.Add("width", Convert.ToString((dt1.Columns.Count * 80) + 200) + "px");

                GridView1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                #endregion
            }

            if (Flag == "Y")
            {
                //Response.Write("<script LANGUAGE='JavaScript' >alert('Report Generated Successfully.')</script>");
            }
            else { Response.Write("<script LANGUAGE='JavaScript' >alert('Record not found.')</script>"); }
        }


        protected void dgViewStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[0].Style.Add("display", "none");
                if (e.Row.Cells.Count > 2)
                    e.Row.Cells[3].Style.Add("display", "none");
            }
            Flag = false;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Text = "Scheme Name " + "<br>" + "(Click scheme for details)";
                e.Row.Cells[1].VerticalAlign = VerticalAlign.Bottom;
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {

                    if (e.Row.Cells[i].Text.Contains("Rank") == true)
                    {
                        e.Row.Cells[i].Text = "Rank ";
                    }

                    //if (ddlCategory.SelectedItem.Text != "Summary")
                    //{
                    if (i == 1)
                    {
                        //dgViewStatus.Columns[1].SortExpression = "[Scheme Name]";
                        if (!Request.Browser.IsMobileDevice)
                        {
                            e.Row.Cells[1].Width = new Unit("410px");
                        }
                        else
                        {
                            e.Row.Cells[1].Width = new Unit("150px");
                            e.Row.Cells[1].Style.Add("overflow-x", "scroll");
                        }
                    }
                    else
                    {
                        e.Row.Cells[i].Width = new Unit("70px");
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                        //dgViewStatus.Columns[0].SortExpression = e.Row.Cells[i].Text;
                    }
                    //}
                    //else
                    //{
                    //    if (i == 1)
                    //    {
                    //        if (!Request.Browser.IsMobileDevice)
                    //        {
                    //            e.Row.Cells[1].Width = new Unit("600px");
                    //        }
                    //        else
                    //        {
                    //            e.Row.Cells[1].Width = new Unit("70px");
                    //            e.Row.Cells[1].Style.Add("overflow-x", "scroll");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        e.Row.Cells[i].Width = new Unit("100px");
                    //        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    //    }
                    //}
                    if (e.Row.Cells[i].HasControls() == true && dgViewStatus.AllowSorting == true)
                    {
                        if (e.Row.Cells[i].Controls[0] is LinkButton)
                        {
                            LinkButton headerControl = e.Row.Cells[i].Controls[0] as LinkButton;
                            //if (Convert.ToString(ViewState["OrgSortExpression"]) == headerControl.Text && ViewState["OrgSortExpression"] != null)
                            //{

                            //}
                            if (headerControl.Text.Contains("Rank") == true)
                            {
                                headerControl.Text = "Rank ";
                            }
                        }
                    }
                    e.Row.Cells[i].Font.Size = 9;

                    e.Row.Attributes.Add("height", "40px");
                    e.Row.Cells[i].Style.Add("cursor", "pointer");
                }
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int j = 1; j < e.Row.Cells.Count; j++)
                {

                    if (j == 1)
                    {
                        e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Left;

                        if (e.Row.Cells[j].Text.Contains("Benchmark Index") == true)
                        {
                            e.Row.Cells[j].ForeColor = Color.Maroon;
                        }

                        if (e.Row.Cells[j].Text.Contains("DSP BlackRock") == true)
                        {
                            // e.Row.Cells[j].ForeColor = Color.Blue;
                            e.Row.Cells[j].BackColor = Color.LightBlue;
                            Flag = true;
                        }

                    }
                    else
                    {
                        e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                        if (Flag == true)
                        {
                            //  e.Row.Cells[j].ForeColor = Color.Blue;
                            e.Row.Cells[j].BackColor = Color.LightBlue;
                        }
                    }
                    //if (ddlCategory.SelectedItem.Text == "Summary")
                    //{
                    //    if (e.Row.Cells[3].Text != "" &&  e.Row.Cells[3].Text != "&nbsp;")
                    //    {
                    //        if (Convert.ToDouble(e.Row.Cells[3].Text) == 3 || Convert.ToDouble(e.Row.Cells[3].Text) == 4)
                    //        {
                    //            e.Row.Cells[3].ForeColor = Color.Maroon;
                    //            e.Row.Cells[4].ForeColor = Color.Maroon;
                    //        }
                    //    }
                    //}                    
                }
                if (Request.Browser.IsMobileDevice)
                {
                    e.Row.Attributes.Add("height", "70px");
                    if (e.Row.RowType != DataControlRowType.Pager)
                    {
                        e.Row.Cells[0].Font.Size = 20;
                    }
                }
            }

        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int j = 1; j < e.Row.Cells.Count; j++)
                {

                    if (j == 0)
                    { e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Left; }
                    else
                    {
                        e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                    }

                }
            }
        }


        protected void GridViewSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Style.Add("display", "none");
            Flag = false;

            if (e.Row.RowType == DataControlRowType.Header)
            {

                for (int i = 1; i < e.Row.Cells.Count; i++)
                {

                    if (e.Row.Cells[i].Text.Contains("Rank") == true)
                    {
                        e.Row.Cells[i].Text = "Rank ";

                    }


                    if (i == 2)
                    {
                        if (!Request.Browser.IsMobileDevice)
                        {
                            e.Row.Cells[2].Width = new Unit("300px");
                        }
                        else
                        {
                            e.Row.Cells[2].Width = new Unit("100px");
                            // e.Row.Cells[1].Style.Add("overflow-x", "scroll");
                        }
                    }
                    else
                    {
                        e.Row.Cells[i].Width = new Unit("70px");
                        //e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;   
                    }

                    e.Row.Cells[i].Font.Size = 9;
                    //if (Request.Browser.IsMobileDevice)
                    //{
                    e.Row.Attributes.Add("height", "40px");
                    //}
                }
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int j = 1; j < e.Row.Cells.Count; j++)
                {

                    if (j == 1)
                    {
                        e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Left;

                    }
                    else
                    {
                        e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;

                    }

                    if (ddlCategory.SelectedItem.Text == "Summary" || ddlCategory.SelectedItem.Text == "Summary Direct")
                    {
                        if (e.Row.Cells[3].Text != "" && e.Row.Cells[3].Text != "&nbsp;")
                        {
                            if (Convert.ToDouble(e.Row.Cells[3].Text) == 3 || Convert.ToDouble(e.Row.Cells[3].Text) == 4)
                            {
                                e.Row.Cells[3].ForeColor = Color.Maroon;
                                e.Row.Cells[4].ForeColor = Color.Maroon;
                            }
                        }
                    }
                }
                if (Request.Browser.IsMobileDevice)
                {
                    e.Row.Attributes.Add("height", "70px");
                }
            }

        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            //EventDate.Value = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            EventDate.Value = iFrames.BLL.DSPAppUpload.GetDate();
            ddlCategory.SelectedIndex = 0;
            GridView1.DataSource = null;
            GridView1.Visible = false;
            dgViewStatus.DataSource = null;
            dgViewStatus.Visible = false;
            GridViewSummary.DataSource = null;
            GridViewSummary.Visible = false;
            ddlperiod.Items.Clear();
            lblrptDate.Text = "";

        }

        protected void dgViewStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dgViewStatus.PageIndex = e.NewPageIndex;
                if (Cache["ViewReport"] != null)
                {
                    dt = Cache["ViewReport"] as DataTable;
                    dgViewStatus.DataSource = dt;
                    dgViewStatus.DataBind();
                }
                else
                {
                    btnSearch_Click(btnSearch, new EventArgs());
                }

                SetFactsheetURL();
                
                dgViewStatus.Style.Add("width", Convert.ToString((dt.Columns.Count * 70) + 600 >= 1950 ? 1950 : (dt.Columns.Count * 70) + 400) + "px");

                HDGrdColCount.Value = Convert.ToString(dt.Columns.Count);
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("Viewreport_dgViewStatus_PageIndexChanging:" + ex.ToString());
            }
        }

        protected void dgViewStatus_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (e.SortExpression == null)
                    return;
                if (e.SortExpression.ToString().Trim() == "")
                    return;
                if (e.SortExpression.Contains("AUM") == true)
                    return;
               
                DataTable dt;
                string sortDirection = "";
                string sortExpression = e.SortExpression;
                string orderBy = "";

                ViewState["OrgSortExpression"] = e.SortExpression;
                if (e.SortExpression.Contains("Rank") == true)
                {
                    sortExpression = sortExpression.Replace("Rank", "").Trim() + " Rank";
                    sortDirection = GetSortDirection(e.SortExpression);
                }
                else
                {
                    sortExpression = sortExpression.Trim() + " Rank";
                    sortDirection = GetSortDirection("Rank " + e.SortExpression);
                }

                dt = preparePageData();

                dgViewStatus.DataSource = dt;
                dgViewStatus.DataBind();

                if (sortDirection == "ASC")
                {
                    orderBy = "Ascending";
                    //if (e.SortExpression.Contains("Rank") == false)
                    //    orderBy = "Descending";
                }
                else
                {
                    orderBy = "Descending";
                    //if (e.SortExpression.Contains("Rank") == false)
                    //    orderBy = "Ascending";
                }
                lblrptDate.Text = "Report as on " + EventDate.Value + ". Sort on '" + sortExpression + "' " + orderBy + ".";
                //else
                //    lblrptDate.Text = "Report as on " + EventDate.Value + ". Sort on '" + sortExpression + "' Descending.";

                SetFactsheetURL();
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.ToString());
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection.Trim();
        }

        private void SetFactsheetURL()
        {
            for (int i = 0; i < dgViewStatus.Rows.Count; i++)
            {

                //if (dgViewStatus.Rows[i].Cells[1].Text.Contains("Index") == true)
                if (dgViewStatus.Rows[i].Cells[1].Text.StartsWith("Benchmark") == true)
                {
                    break;
                }
                else
                {
                    int Number;
                    if (int.TryParse(dgViewStatus.Rows[i].Cells[0].Text, out Number))
                    {
                        HyperLink hlContro = new HyperLink();
                        //hlContro.NavigateUrl = "./Factsheet.aspx?ID=" + dgViewStatus.Rows[i].Cells[1].Text; 
                        hlContro.Attributes.Add("OnClick", "javascript:return showFactsheet('" + dgViewStatus.Rows[i].Cells[0].Text + "')");
                        hlContro.Attributes.Add("style", "cursor:pointer");
                        hlContro.Text = dgViewStatus.Rows[i].Cells[1].Text;
                        //hlContro.ImageUrl = "./sample.jpg";
                        // hlContro.Text = "Documents";
                        if (dgViewStatus.Rows[i].Cells[1].Text.Contains("DSP BlackRock") == true)
                        {
                            // hlContro.ForeColor = Color.Blue;
                            hlContro.BackColor = Color.LightBlue;
                        }
                        dgViewStatus.Rows[i].Cells[1].Controls.Add(hlContro);
                    }
                }
            }
        }

        private DataTable preparePageData()
        {
            DataTable retdt = new DataTable();
            try
            {
                DataTable dtDspSch = Cache["DspSchemes"] as DataTable;
                DataTable dtOthSch = Cache["OtherSchemes"] as DataTable;
                DataTable dtIndex = Cache["Index"] as DataTable;

                if (dtDspSch == null || dtOthSch == null || dtIndex == null)
                {
                    btnSearch_Click(btnSearch, new EventArgs());
                    return retdt;
                }

                int dspDisplayCount = 4;
                int othSchDispCount = 0;
                DataView dv;
                string sortExpression = ViewState["SortExpression"] as string;
                string sortDirection = ViewState["SortDirection"] as string;
                int dspSchCount = dtDspSch.Rows.Count;                

                if (ConfigurationManager.AppSettings["DSPSchemeCount"] != null)
                    dspDisplayCount = Convert.ToInt32(ConfigurationManager.AppSettings["DSPSchemeCount"]);
                retdt = dtDspSch.Clone();

                if (dspSchCount == 0)
                {
                    dv = dtOthSch.DefaultView;
                    if (sortExpression != null && sortDirection != null)
                        dv.Sort = "[" + sortExpression + "] " + sortDirection;
                   
                    dtOthSch = dv.ToTable();
                    retdt.Merge(dtOthSch);
                    if (dtIndex.Rows.Count > 0)
                        retdt.Merge(dtIndex);
                }
                else if (dspSchCount <= dspDisplayCount)
                {
                    if (sortExpression != null && sortDirection != null)
                    {
                        dv = dtDspSch.DefaultView;
                        dv.Sort = "[" + sortExpression + "] " + sortDirection;
                        dtDspSch = dv.ToTable();

                        dv = dtOthSch.DefaultView;
                        dv.Sort = "[" + sortExpression + "] " + sortDirection;
                        dtOthSch = dv.ToTable();
                    }
                    DataTable dtTmp = dtOthSch.Copy();

                    if (dtIndex.Rows.Count > 0)
                        dtTmp.Merge(dtIndex);

                    othSchDispCount = dgViewStatus.PageSize - dspSchCount;
                    for (int i = 0; i < dtTmp.Rows.Count; i++)
                    {
                        if (i % othSchDispCount == 0)
                        {
                            retdt.Merge(dtDspSch);
                            retdt.ImportRow(dtTmp.Rows[i]);
                        }
                        else
                        {
                            retdt.ImportRow(dtTmp.Rows[i]);
                        }
                    }
                }
                else
                {
                    retdt.Merge(dtDspSch);
                    retdt.Merge(dtOthSch);
                    dv = retdt.DefaultView;
                    if (sortExpression != null && sortDirection != null)
                        dv.Sort = "[" + sortExpression + "] " + sortDirection;
                    else
                        dv.Sort = "[Scheme Name] ASC";
                    retdt = dv.ToTable();
                    if (dtIndex.Rows.Count > 0)
                        retdt.Merge(dtIndex);
                }
                Cache["ViewReport"] = retdt.Copy();
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.ToString());
            }
            return retdt;
        }
    }
}