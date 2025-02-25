using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Text;
using System.Collections;
using System.Drawing;

namespace iFrames.Chart
{
    public partial class NavGraph : System.Web.UI.Page
    {
        #region: Global Variable
        clsNavGraph objNavGraph = new clsNavGraph();
        Random random = new Random();
        #endregion

        #region: Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            lblGridMsg.Text = string.Empty;
            lblDateRangeMsg.Text = string.Empty;
            divLegands.InnerHtml = string.Empty;
            if (!IsPostBack)
            {
                //string a = ConfigurationManager.AppSettings["code"].ToString();
                if (Request.QueryString["mucode"] != null)
                //if(!string.IsNullOrEmpty(a))
                {
                    string[] arrStr = new string[5];
                    string a = Convert.ToString(Request.QueryString["mucode"]);
                    arrStr = a.Split(',');
                    objNavGraph.FillDropdown(ddlMutualFund, objNavGraph.FetchExcludedMutualFund(arrStr), "mut_name", "mut_code", "Mutual Fund-");
                    FillIndices();
                }
                else
                {
                    FillMutualFund();
                    FillIndices();
                }

            }

        }
        #endregion

        #region: Datagrid Event
        protected void dglist_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    string autoid = Convert.ToString(e.CommandArgument);
                    DataTable dt = new DataTable();
                    dt = RestoreDatafromGrid();
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "AutoID <>" + autoid + "";
                    dt = dv.ToTable();

                    if (dt.Rows.Count > 0)
                    {
                        dglist.Visible = true;
                        dglist.DataSource = dt;
                        dglist.DataBind();
                    }
                    else
                    {

                        dglist.DataSource = dt;
                        dglist.DataBind();
                        dglist.Visible = false;

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region: Button Event
        protected void btnAddScheme_Click(object sender, EventArgs e)
        {
            AddScheme();
        }
        protected void btnAddIndices_Click(object sender, EventArgs e)
        {
            AddIndices();
        }
        protected void btnPlotChart_Click(object sender, EventArgs e)
        {
            MergeData();
        }
        #endregion

        #region: Dropdownlist Event
        protected void ddlMutualFund_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlMutualFund.SelectedIndex != 0)
            {
                FillScheme(ddlMutualFund.SelectedItem.Value.Trim());

                ddlSchemes.SelectedIndex = 0;
                ddlIndices.SelectedIndex = 0;

                if (dglist.Items.Count > 0)
                {
                    DataTable dt = RestoreDatafromGrid();
                    dt = dt.Clone();
                    dglist.DataSource = dt;
                    dglist.DataBind();
                    dglist.Visible = false;
                }

            }
            else
            {
                if (dglist.Items.Count > 0)
                {
                    DataTable dt = RestoreDatafromGrid();
                    dt = dt.Clone();
                    dglist.DataSource = dt;
                    dglist.DataBind();
                    dglist.Visible = false;
                }
                ddlSchemes.SelectedIndex = 0;
                ddlIndices.SelectedIndex = 0;

            }

        }
        #endregion

        #region: Fill Method
        public void FillMutualFund()
        {

            try
            {
                DataTable dt = objNavGraph.FetchMutualFund();
                objNavGraph.FillDropdown(ddlMutualFund, dt, "mut_name", "mut_code", "Mutual Fund-");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void FillScheme(string mut_code)
        {
            try
            {
                DataTable dt = objNavGraph.FetchSchemes(mut_code);
                if (dt.Rows.Count > 0)
                {
                    objNavGraph.FillDropdown(ddlSchemes, dt, "sch_name", "sch_code", "Scheme-");
                }
                else
                {
                    if (ddlSchemes.Items.Count > 0)
                    {
                        ddlSchemes.Items.Clear();
                        ddlSchemes.Items.Insert(0, new ListItem("-Select Scheme- ", "0"));
                        ddlSchemes.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlSchemes.Items.Insert(0, new ListItem("-Select Scheme- ", "0"));
                        ddlSchemes.SelectedIndex = 0;
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void FillIndices()
        {
            try
            {
                DataTable dt = objNavGraph.FetchIndices();
                if (dt.Rows.Count > 0)
                {
                    objNavGraph.FillDropdown(ddlIndices, dt, "ind_name", "ind_code", "Index -");
                }
                else
                {
                    if (ddlIndices.Items.Count > 0)
                    {
                        ddlIndices.Items.Clear();
                        ddlIndices.Items.Insert(0, new ListItem("-Select Index-", "0"));
                        ddlIndices.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlIndices.Items.Insert(0, new ListItem("-Select Index-", "0"));
                        ddlIndices.SelectedIndex = 0;
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region: Default Method
        public DataTable RestoreDatafromGrid()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("AutoID", typeof(System.String));
                dt.Columns.Add("SchemeCode", typeof(System.String));
                dt.Columns.Add("SchemeName", typeof(System.String));
                dt.Columns.Add("IndCode", typeof(System.String));
                dt.Columns.Add("IndName", typeof(System.String));
                dt.Columns.Add("Status", typeof(System.String));

                if (dglist.Items.Count > 0)
                {
                    for (int i = 0; i < dglist.Items.Count; i++)
                    {
                        string val_AutoID = ((Label)dglist.Items[i].Cells[2].FindControl("lblAutoID")).Text;
                        string val_SchemeCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeCode")).Text;
                        string val_SchemeName = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeName")).Text;
                        string val_IndCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndCode")).Text;
                        string val_IndName = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndName")).Text;
                        DataRow dr = dt.NewRow();
                        dr["AutoID"] = val_AutoID;
                        dr["SchemeCode"] = val_SchemeCode;
                        dr["SchemeName"] = val_SchemeName;
                        dr["IndCode"] = val_IndCode;
                        dr["IndName"] = val_IndName;
                        CheckBox cb = dglist.Items[i].FindControl("chkItem") as CheckBox;
                        if (cb.Checked == true)
                        {
                            dr["Status"] = "1";
                        }
                        else
                        {
                            dr["Status"] = "0";
                        }
                        dt.Rows.Add(dr);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dt;
        }

        public DataTable RestoreDatafromGrid1(string startDate, string endDate)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("AutoID", typeof(System.String));
                dt.Columns.Add("SchemeCode", typeof(System.String));
                dt.Columns.Add("SchemeName", typeof(System.String));
                dt.Columns.Add("IndCode", typeof(System.String));
                dt.Columns.Add("IndName", typeof(System.String));
                dt.Columns.Add("Status", typeof(System.String));

                if (dglist.Items.Count > 0)
                {
                    for (int i = 0; i < dglist.Items.Count; i++)
                    {
                        string val_AutoID = ((Label)dglist.Items[i].Cells[2].FindControl("lblAutoID")).Text;
                        string val_SchemeCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeCode")).Text;
                        string val_SchemeName = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeName")).Text;
                        string val_IndCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndCode")).Text;
                        string val_IndName = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndName")).Text;

                        if (val_SchemeCode != "")
                        {
                            if (objNavGraph.FetchNavAgainstScheme(val_SchemeCode, startDate, endDate) == 0)
                                continue;
                        }
                        else
                        {
                            if (objNavGraph.FetchNavAgainstIndices(val_IndCode, startDate, endDate) == 0)
                                continue;
                        }
                        DataRow dr = dt.NewRow();
                        dr["AutoID"] = val_AutoID;
                        dr["SchemeCode"] = val_SchemeCode;
                        dr["SchemeName"] = val_SchemeName;
                        dr["IndCode"] = val_IndCode;
                        dr["IndName"] = val_IndName;
                        CheckBox cb = dglist.Items[i].FindControl("chkItem") as CheckBox;
                        if (cb.Checked == true)
                        {
                            dr["Status"] = "1";
                        }
                        else
                        {
                            dr["Status"] = "0";
                        }
                        dt.Rows.Add(dr);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dt;
        }
        #endregion

        #region: Add Method
        public void AddScheme()
        {
            try
            {
                if (ddlSchemes.SelectedIndex != 0)
                {
                    //DataTable dt = objNavGraph.FetchIndicesAgainstScheme(ddlSchemes.SelectedItem.Value.Trim());
                    DataTable dt = new DataTable();
                    dt.Columns.Add("sch_code", typeof(System.String));
                    dt.Columns.Add("sch_name", typeof(System.String));
                    dt.Columns.Add("ind_code", typeof(System.String));
                    dt.Columns.Add("ind_name", typeof(System.String));

                    DataView dv = objNavGraph.FetchIndicesAgainstScheme(ddlSchemes.SelectedItem.Value.Trim()).DefaultView;
                    DataTable dtScheme = dv.ToTable(true, "sch_code", "sch_name");
                    DataTable dtIndex = dv.ToTable(true, "ind_code", "ind_name");

                    string schcode = string.Empty;
                    string schname = string.Empty;
                    schcode = dtScheme.Rows[0]["sch_code"].ToString();
                    schname = dtScheme.Rows[0]["sch_name"].ToString();
                    foreach (DataRow drInex in dtIndex.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["sch_code"] = schcode;
                        dr["sch_name"] = schname;
                        dr["ind_code"] = drInex["ind_code"];
                        dr["ind_name"] = drInex["ind_name"];
                        dt.Rows.Add(dr);
                        schcode = string.Empty;
                        schname = string.Empty;
                    }

                    DataTable dtToBind = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dtToBind = RestoreDatafromGrid();

                        if (dtToBind.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                foreach (DataColumn clmn in dt.Columns)
                                {
                                    if (clmn.ColumnName == "sch_code" && !string.IsNullOrEmpty(dr["sch_code"].ToString()))
                                    {
                                        if (!objNavGraph.CheckerSameField(dt, dtToBind, "scheme"))
                                        {
                                            DataRow dr1 = dtToBind.NewRow();
                                            dr1["AutoID"] = dtToBind.Rows.Count.ToString();
                                            dr1["SchemeCode"] = Convert.ToString(dr[clmn.ColumnName]);
                                            dr1["SchemeName"] = Convert.ToString(dr["sch_name"]);
                                            dtToBind.Rows.Add(dr1);
                                        }

                                    }

                                    if (clmn.ColumnName == "ind_code" && !string.IsNullOrEmpty(dr["ind_code"].ToString()))
                                    {

                                        if (!objNavGraph.CheckerSameField(dt, dtToBind, "ind"))
                                        {
                                            DataRow dr2 = dtToBind.NewRow();
                                            dr2["AutoID"] = dtToBind.Rows.Count.ToString();
                                            dr2["IndCode"] = Convert.ToString(dr[clmn.ColumnName]);
                                            dr2["IndName"] = Convert.ToString(dr["ind_name"]);
                                            dtToBind.Rows.Add(dr2);
                                        }

                                    }

                                }
                            }
                        }
                        else
                        {
                            int i = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                foreach (DataColumn clmn in dt.Columns)
                                {
                                    if (clmn.ColumnName == "sch_code" && !string.IsNullOrEmpty(dr["sch_code"].ToString()))
                                    {
                                        DataRow dr1 = dtToBind.NewRow();
                                        dr1["AutoID"] = i.ToString();
                                        dr1["SchemeCode"] = Convert.ToString(dr[clmn.ColumnName]);
                                        dr1["SchemeName"] = Convert.ToString(dr["sch_name"]);
                                        dtToBind.Rows.Add(dr1);
                                        i = i + 1;
                                    }

                                    if (clmn.ColumnName == "ind_code" && !string.IsNullOrEmpty(dr["ind_code"].ToString()))
                                    {
                                        DataRow dr2 = dtToBind.NewRow();
                                        dr2["AutoID"] = i.ToString();
                                        dr2["IndCode"] = Convert.ToString(dr[clmn.ColumnName]);
                                        dr2["IndName"] = Convert.ToString(dr["ind_name"]);
                                        dtToBind.Rows.Add(dr2);
                                        i = i + 1;
                                    }

                                }
                            }

                        }
                    }
                    else
                    {

                        dtToBind = RestoreDatafromGrid();
                        if (dtToBind.Rows.Count > 0)
                        {
                            DataRow dr = dtToBind.NewRow();
                            dr["AutoID"] = dtToBind.Rows.Count;
                            dr["SchemeCode"] = ddlSchemes.SelectedItem.Value;
                            dr["SchemeName"] = ddlSchemes.SelectedItem.Text;
                            dtToBind.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = dtToBind.NewRow();
                            dr["AutoID"] = "0";
                            dr["SchemeCode"] = ddlSchemes.SelectedItem.Value;
                            dr["SchemeName"] = ddlSchemes.SelectedItem.Text;
                            dtToBind.Rows.Add(dr);
                        }

                    }

                    if (dtToBind.Rows.Count > 0)
                    {
                        dglist.Visible = true;
                        dglist.DataSource = dtToBind;
                        dglist.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void AddIndices()
        {
            DataTable dt = new DataTable();
            try
            {
                if (ddlIndices.SelectedIndex != 0)
                {
                    dt = RestoreDatafromGrid();
                    if (dt.Rows.Count > 0)
                    {
                        DataView dvIndcode = dt.DefaultView;
                        string indcode = ddlIndices.SelectedItem.Value.ToString();
                        dvIndcode.RowFilter = "IndCode<>''";
                        DataTable dtAfterFilter = dvIndcode.ToTable();
                        DataView dvFilterInd = dtAfterFilter.DefaultView;
                        dvFilterInd.RowFilter = "IndCode='" + indcode + "'";
                        dtAfterFilter = dvFilterInd.ToTable();
                        if (dtAfterFilter.Rows.Count == 0)
                        {
                            dt = RestoreDatafromGrid();
                            DataRow dr = dt.NewRow();
                            dr["AutoID"] = Convert.ToString(dt.Rows.Count);
                            dr["IndCode"] = ddlIndices.SelectedItem.Value;
                            dr["IndName"] = ddlIndices.SelectedItem.Text;
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            dt = RestoreDatafromGrid();
                        }

                    }
                    else
                    {
                        //dt.Columns.Add("AutoID", typeof(System.String));
                        //dt.Columns.Add("SchemeCode", typeof(System.String));
                        //dt.Columns.Add("SchemeName", typeof(System.String));
                        //dt.Columns.Add("IndCode", typeof(System.String));
                        //dt.Columns.Add("IndName", typeof(System.String));
                        DataRow dr = dt.NewRow();
                        dr["AutoID"] = "0".ToString();
                        dr["IndCode"] = ddlIndices.SelectedItem.Value;
                        dr["IndName"] = ddlIndices.SelectedItem.Text;
                        dt.Rows.Add(dr);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    dglist.Visible = true;
                    dglist.DataSource = dt;
                    dglist.DataBind();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region: Merge Method
        public void MergeData()
        {
            string enddate = string.Empty;
            string startdate = string.Empty;
            DataTable dtCommon = new DataTable();
            DataTable dtSchemeNav = new DataTable();
            DataTable dtIndNav = new DataTable();

            try
            {
                if (rbTime.Checked == true)
                {
                    enddate = DateTime.Now.ToString("MM/dd/yyyy");
                    startdate = DateTime.Today.AddDays(Convert.ToDouble("-" + ddlTime.SelectedItem.Value)).ToString("MM/dd/yyyy");
                }
                else
                {
                    if (hdnStartDate.Value != "" && hdnEndDate.Value != "")
                    {
                        enddate = Convert.ToDateTime(hdnEndDate.Value).ToString("MM/dd/yyyy");
                        startdate = Convert.ToDateTime(hdnStartDate.Value).ToString("MM/dd/yyyy");
                    }
                }

                if (Convert.ToDateTime(startdate) > Convert.ToDateTime(enddate))
                {
                    lblDateRangeMsg.Text = "From date can not be greater than to date.";

                }

                int j1 = 0;

                DataTable dt = RestoreDatafromGrid1(startdate, enddate);
                Session["FilteredDataGrid"] = dt;
                StringBuilder sbsqlMain = new StringBuilder();
                StringBuilder sbsqlTop = new StringBuilder();
                StringBuilder sbsqlMiddle = new StringBuilder();
                StringBuilder sbsqlBottom = new StringBuilder();
                StringBuilder sbLast = new StringBuilder();
                ArrayList arrParam_Name = new ArrayList();
                ArrayList arrParam_Value = new ArrayList();

                if (dt.Rows.Count > 0)
                {
                    //sbsqlBottom.AppendLine("select * into #temp from( ");
                    sbLast.AppendLine("select");
                    foreach (DataRow dr in dt.Select("", "SchemeCode desc"))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["SchemeCode"])) && Convert.ToString(dr["Status"]) != "0")
                        {
                            string schemecode = Convert.ToString(dr["SchemeCode"]);
                            arrParam_Name.Add("Sch_Code" + j1);
                            arrParam_Value.Add(schemecode);
                            sbsqlTop.AppendLine("declare @First_" + arrParam_Name[j1].ToString() + " as float;");
                            //sbsqlTop.AppendLine("select top(1) @First_" + arrParam_Name[j1].ToString() + " = nav_rs from nav_rec where dateadd(d,276, date) >= @Start_Date and sch_code=@" + arrParam_Name[j1].ToString() + " order by date asc;");
                            sbsqlTop.AppendLine("select top(1) @First_" + arrParam_Name[j1].ToString() + " = schemenav" + j1.ToString() + " from #temp where date = (select min(date) from #temp) ");


                            if (j1 != 0)
                            {
                                sbsqlBottom.AppendLine("inner join");

                            }

                            //sbsqlBottom.AppendLine("(select CAST(100 * nav_rs/@First_" + arrParam_Name[j1].ToString() + " as decimal(18,2) ) as schemenav" + j1 + ",dateadd(d,276, date) as date from nav_rec where dateadd(d,276, date) between  @Start_Date and @End_Date and sch_code=@Sch_Code" + j1 + ") Sch_Code" + j1 + "");
                            sbsqlBottom.AppendLine("(select nav_rs as schemenav" + j1 + ",dateadd(d,276, date) as date from nav_rec where dateadd(d,276, date) between  @Start_Date and @End_Date and sch_code=@Sch_Code" + j1 + ") Sch_Code" + j1 + "");
                            if (j1 != 0)
                            {
                                //sbLast.AppendLine(",isnull(CAST(100 * schemenav" + j1 + "/@First_" + arrParam_Name[j1].ToString() + " as decimal(18,2) ),0) as schemenav" + j1 + "");
                                sbLast.AppendLine(",CAST(100 * schemenav" + j1 + "/@First_" + arrParam_Name[j1].ToString() + " as decimal(18,2) ) as schemenav" + j1 + "");

                            }
                            else
                            {
                                sbLast.AppendLine("CAST(100 * schemenav" + j1 + "/@First_" + arrParam_Name[j1].ToString() + " as decimal(18,2) ) as schemenav" + j1 + "");
                            }


                            if (j1 != 0)
                            {
                                sbsqlBottom.AppendLine("on " + arrParam_Name[0].ToString() + ".date = Sch_Code" + j1 + ".date");
                            }


                            j1 = j1 + 1;

                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dr["IndCode"])) && Convert.ToString(dr["Status"]) != "0")
                        {
                            string indcode = Convert.ToString(dr["IndCode"]);
                            arrParam_Name.Add("Ind_Code" + j1);
                            arrParam_Value.Add(indcode);
                            sbsqlTop.AppendLine("declare @First_" + arrParam_Name[j1].ToString() + " as float;");
                            //sbsqlTop.AppendLine("select top(1) @First_" + arrParam_Name[j1].ToString() + " = ind_val from ind_rec where dateadd(d,276, dt1) >= @Start_Date and  ind_code=@" + arrParam_Name[j1].ToString() + ";");
                            sbsqlTop.AppendLine("select top(1) @First_" + arrParam_Name[j1].ToString() + " = indnav" + j1.ToString() + " from #temp where date = (select Min(date) from #temp) ;");

                            if (j1 != 0)
                            {
                                sbsqlBottom.AppendLine("inner join");

                            }

                            //sbsqlBottom.AppendLine("(select CAST( 100 *ind_val/@First_Ind_Code" + j1 + " AS decimal(18,2)) as indnav" + j1 + ",dateadd(d,276, dt1) as date from ind_rec where dateadd(d,276, dt1) between  @Start_Date and @End_Date and  ind_code=@Ind_Code" + j1 + ") Ind_Code" + j1 + "");
                            sbsqlBottom.AppendLine("(select ind_val as indnav" + j1 + ",dateadd(d,276, dt1) as date from ind_rec where dateadd(d,276, dt1) between  @Start_Date and @End_Date and  ind_code=@Ind_Code" + j1 + ") Ind_Code" + j1 + "");

                            if (j1 != 0)
                            {
                                //sbLast.AppendLine(",isnull(CAST( 100 *indnav" + j1 + "/@First_Ind_Code" + j1 + " AS decimal(18,2)),0) as indnav" + j1 + "");
                                sbLast.AppendLine(",CAST( 100 *indnav" + j1 + "/@First_Ind_Code" + j1 + " AS decimal(18,2)) as indnav" + j1 + "");
                            }
                            else
                            {
                                //sbLast.AppendLine("isnull(CAST( 100 *indnav" + j1 + "/@First_Ind_Code" + j1 + " AS decimal(18,2)),0) as indnav" + j1 + "");
                                sbLast.AppendLine("CAST( 100 *indnav" + j1 + "/@First_Ind_Code" + j1 + " AS decimal(18,2)) as indnav" + j1 + "");
                            }

                            if (j1 != 0)
                            {
                                sbsqlBottom.AppendLine("on " + arrParam_Name[0].ToString() + ".date = Ind_Code" + j1 + ".date");
                            }


                            j1 = j1 + 1;
                        }

                    }

                    if (arrParam_Name.Count > 0)
                    {
                        sbsqlMiddle.AppendLine("select * into #temp from( select  ");
                        string str = string.Empty;
                        for (int j2 = 0; j2 < arrParam_Name.Count; j2++)
                        {

                            if (arrParam_Name[j2].ToString().StartsWith("Sch"))
                            {
                                sbsqlMiddle.Append("schemenav" + j2 + ",");
                            }
                            else
                            {
                                sbsqlMiddle.Append("indnav" + j2 + ",");
                            }

                            if (j2 != arrParam_Name.Count - 1)
                            {
                                if (string.IsNullOrEmpty(str))
                                {
                                    str = "isnull(" + arrParam_Name[j2] + ".date," + arrParam_Name[j2 + 1].ToString() + ".date)";
                                }
                                else
                                {
                                    str = "isnull(" + str + "," + arrParam_Name[j2 + 1] + ".date)";
                                }
                            }

                        }
                        sbsqlMiddle.Append(str + "date");
                        sbsqlMiddle.AppendLine(" from");

                        //sbsqlMain.AppendLine(sbsqlTop.ToString());
                        //sbsqlMain.AppendLine(sbsqlMiddle.ToString());



                        sbsqlBottom.AppendLine(")A");

                        sbsqlMain.AppendLine(sbsqlMiddle.ToString());
                        sbsqlMain.AppendLine(sbsqlBottom.ToString());
                        sbsqlMain.AppendLine(sbsqlTop.ToString());
                        //sbsqlMain.AppendLine("order by date");
                        sbLast.AppendLine(",date from #temp");
                        sbLast.AppendLine("drop table #temp");
                        sbsqlMain.AppendLine(sbLast.ToString());

                        dtCommon = objNavGraph.FetchNav(sbsqlMain.ToString(), startdate, enddate, arrParam_Name, arrParam_Value);
                        CreateNAVChart(dtCommon);
                    }
                    else
                    {
                        lblGridMsg.Text = "Please Select Scheme or Index to Chart.";
                        //code
                    }

                }
                else
                {
                    lblGridMsg.Text = "Please Select Scheme or Index to Chart.";
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region: Create Chart Method
        private void CreateNAVChart(DataTable _dtorg)
        {
            try
            {
                if (_dtorg.Rows.Count > 0)
                {

                    DataTable dtGrid = ((DataTable)Session["FilteredDataGrid"]);

                    BindToDataTable(objNavGraph.ReplaceName(_dtorg, dtGrid), "date", chrtNavGraph);
                    chrtNavGraph.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    chrtNavGraph.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                    chrtNavGraph.BackColor = System.Drawing.ColorTranslator.FromHtml("#E8F5FC");
                    chrtNavGraph.BackGradientStyle = GradientStyle.Center;
                    chrtNavGraph.IsSoftShadows = true;
                }
                else
                {
                    TextAnnotation annotation = new TextAnnotation();
                    annotation.X = 50;
                    annotation.Y = 50;
                    annotation.Text = "No Data";
                    annotation.ForeColor = System.Drawing.Color.Red;
                    System.Drawing.Font fnt = new System.Drawing.Font("verdana", 9.0f);
                    annotation.Font = fnt;
                    chrtNavGraph.Annotations.Add(annotation);
                }

            }
            catch (Exception ex)
            {


            }
        }
        private void BindToDataTable(DataTable _dt, string xField, System.Web.UI.DataVisualization.Charting.Chart MainChart)
        {
            try
            {
                MainChart.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;

                double max = 0, min = 0; int count = 0;
                foreach (DataColumn col in _dt.Columns)
                {

                    //if (_dt.Select("[" + col.ColumnName + "] = 0").Count() == _dt.Rows.Count)
                    //    continue;

                    if (col.ColumnName.ToUpper() == "DATE") continue;
                    double tmax = _dt.Rows.Cast<DataRow>().Where(x => x[col.ColumnName] != DBNull.Value && x[col.ColumnName].ToString().Trim() != "").Select(x => Convert.ToDouble(x[col.ColumnName].ToString())).Max();
                    double tmin = _dt.Rows.Cast<DataRow>().Where(x => x[col.ColumnName] != DBNull.Value && x[col.ColumnName].ToString().Trim() != "").Select(x => Convert.ToDouble(x[col.ColumnName].ToString())).Min();
                    if (count == 0)
                    {
                        max = tmax;
                        min = tmin;
                    }

                    if (max < tmax && count != 0)
                    {
                        max = tmax;
                    }
                    else
                    {
                        max = max;
                    }

                    if (min < tmin && count != 0)
                    {
                        min = min;
                    }
                    else
                    {
                        min = tmin;
                    }
                    count++;
                }

                MainChart.ChartAreas[0].AxisY.Minimum = min - 5;
                MainChart.ChartAreas[0].AxisY.Maximum = max + 5;

                List<DateTime> leveldate = _dt.AsEnumerable().Select(al => al.Field<DateTime>("date")).Distinct().ToList();
                DateTime maxdate = leveldate.Max();
                DateTime mindate = leveldate.Min();

                MainChart.ChartAreas[0].AxisX.Minimum = leveldate.Min().ToOADate();
                //MainChart.ChartAreas[0].AxisX.Maximum = leveldate.Max().AddMonths(4).ToOADate();

                TimeSpan range = maxdate - mindate;
                if (range.TotalDays > 365 * 3)
                {
                    MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    MainChart.ChartAreas[0].AxisX.Interval = 24;
                    MainChart.ChartAreas[0].AxisX.Maximum = leveldate.Max().AddMonths(1).ToOADate();
                    //MainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -80;
                }
                else if (range.TotalDays > 365 && range.TotalDays <= 365 * 3)
                {
                    MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                    MainChart.ChartAreas[0].AxisX.Interval = 3;
                    MainChart.ChartAreas[0].AxisX.Maximum = leveldate.Max().AddMonths(2).ToOADate();
                    //MainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -80;
                }
                else if (range.TotalDays <= 365 && range.TotalDays > 180)
                {
                    MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    MainChart.ChartAreas[0].AxisX.Interval = 20;
                    MainChart.ChartAreas[0].AxisX.Maximum = leveldate.Max().AddDays(2).ToOADate();
                    MainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -80;
                }
                else if (range.TotalDays <= 180 && range.TotalDays > 90)
                {
                    MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    MainChart.ChartAreas[0].AxisX.Interval = 10;
                    MainChart.ChartAreas[0].AxisX.Maximum = leveldate.Max().AddDays(2).ToOADate();
                    //MainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -80;
                }
                else if (range.TotalDays <= 90 && range.TotalDays > 30)
                {
                    MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    MainChart.ChartAreas[0].AxisX.Interval = 5;
                    MainChart.ChartAreas[0].AxisX.Maximum = leveldate.Max().AddDays(2).ToOADate();
                    //MainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -80;
                }
                else if (range.TotalDays <= 30 && range.TotalDays > 7)
                {
                    MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    MainChart.ChartAreas[0].AxisX.Interval = 2;
                    MainChart.ChartAreas[0].AxisX.Maximum = leveldate.Max().AddDays(2).ToOADate();
                    // MainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -80;

                }
                else if (range.TotalDays <= 7)
                {
                    MainChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    MainChart.ChartAreas[0].AxisX.Interval = 1;
                    MainChart.ChartAreas[0].AxisX.Maximum = leveldate.Max().AddDays(1).ToOADate();
                    //MainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -80;
                }
                MainChart.ChartAreas[0].AxisY.LabelStyle.Format = "D1";
                // MainChart.ChartAreas[0].AxisX.LabelStyle.Format = Convert.ToDateTime(_dt.Rows[0]["date"]).ToString("MMM").Replace("M", "\\M").Replace("y", "\\y") + "-yyyy";

                MainChart.Series.Clear();
                Dictionary<string, Color> dic = new Dictionary<string, Color>();
                foreach (DataColumn dc in _dt.Columns)
                {
                    if (dc.ColumnName == xField)
                        continue;
                    MainChart.Series.Add(dc.ColumnName);
                    MainChart.Series[dc.ColumnName].ChartType = SeriesChartType.Line;
                    MainChart.Series[dc.ColumnName].Color = GetRandomColor();
                    dic.Add(MainChart.Series[dc.ColumnName].Name, MainChart.Series[dc.ColumnName].Color);
                    MainChart.Series[dc.ColumnName].Points.DataBindXY(_dt.DefaultView, xField, _dt.DefaultView, dc.ColumnName);
                    MainChart.Series[dc.ColumnName].BorderWidth = 1;
                }

                FormatChartLegend(dic);
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        private void FormatChartLegend(Dictionary<string, Color> dic)
        {
            try
            {
                var html = "<table width='100%' cellpadding='0' cellspacing='0' border='0' style='color: #007AB9;font-family: Verdana;font-size: 10pt;' >";
                DataTable dt = dic.ToDataTable();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (i % 2 == 0) html += "<tr>";

                    var schemeName = dt.Rows[i]["Key"].ToString();
                    Color color = (Color)dt.Rows[i]["value"];
                    html += "<td style='width: 50%;'>";
                    html += "<div>";
                    html += "<table width='100%' cellpadding='0' cellspacing='0' border='0'>";
                    html += "<tr>";
                    html += "<td style='width: 8%' align='center'>";
                    html += "<div id='color' style='border: 1px solid black; width: 10px; height: 10px; background-color: " + ColorTranslator.ToHtml(color) + ";'></div>";
                    html += "</td>";
                    html += "<td style='width: 95%' align='left'>" + schemeName;
                    html += "</td>";
                    html += "</tr>";
                    html += "</table>";
                    html += "</div>";
                    html += "</td>";

                    if (i % 2 != 0) html += "</tr>";

                    if (i == dt.Rows.Count - 1)
                    {
                        html += "</tr>";
                    }
                }

                html += "</table>";
                html += "</div>";
                divLegands.InnerHtml = html;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region: Random Color Method
        private Color GetRandomColor()
        {
            return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(1, 255));
        }
        #endregion

    }
}