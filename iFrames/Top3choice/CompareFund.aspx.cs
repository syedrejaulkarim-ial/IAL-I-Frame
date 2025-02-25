using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;
using System.Text;
using System.Collections;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.IO;

namespace iFrames.Top3choice
{
    public partial class CompareFund1 : System.Web.UI.Page
    {
    
        #region GlobalVar
        readonly string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        readonly SqlConnection conn = new SqlConnection();
        string columnName = "Per_1_Year";
        int flag = 0;
        #endregion


        #region PageEvent

        protected void Page_Load(object sender, EventArgs e)
        {          
            if (!IsPostBack)
            {
                getFundHouse();
                getIndicesName();
                LoadNature();
                LoadAllSubNature();
                LoadStructure();
                loadOption();
                DivFundShow.Visible = false;
                DivShowPerformance.Visible = false;
            }
            
            lblErrMsg.Text = "";
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

                    sethidSchIndDataTable();
                    setSchemelist();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        #region Gridview Event



        protected void GrdCompFund_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string sortexpression = null;
            if (e.CommandName == "Per_30_Days")
            {
                sortexpression = "Per_30_Days DESC";
                columnName = "Per_30_Days";
                ViewState["columnName"] = columnName;
                bindGridview(sortexpression);
                ViewState.Remove("columnName");
            }
            else if (e.CommandName == "Per_91_Days")
            {
                columnName = "Per_91_Days";
                ViewState["columnName"] = columnName;
                sortexpression = "Per_91_Days DESC";
                bindGridview(sortexpression);
                ViewState.Remove("columnName");
            }
            else if (e.CommandName == "Per_182_Days")
            {
                columnName = "Per_182_Days";
                ViewState["columnName"] = columnName;
                sortexpression = "Per_182_Days DESC";
                bindGridview(sortexpression);
                ViewState.Remove("columnName");
            }
            else if (e.CommandName == "Per_1_Year")
            {
                columnName = "Per_1_Year";
                ViewState["columnName"] = columnName;
                sortexpression = "Per_1_Year DESC";
                bindGridview(sortexpression);
                ViewState.Remove("columnName");
            }
            else if (e.CommandName == "Per_3_Year")
            {
                columnName = "Per_3_Year";
                ViewState["columnName"] = columnName;
                sortexpression = "Per_3_Year DESC";
                bindGridview(sortexpression);
                ViewState.Remove("columnName");
            }
        }

        protected void GrdCompFund_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ViewState["columnName"] != null)
                columnName = ViewState["columnName"].ToString();

            if (flag != 1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    DataRowView drv = (DataRowView)e.Row.DataItem;

                    for (int i = 0; i < drv.DataView.Table.Columns.Count; i++)
                    {
                        //GridViewRow gr = e.Row as GridViewRow;
                        Label lbl = e.Row.FindControl("lblSchName") as Label;
                        if (lbl.Text != "Average performance of similar category funds")
                        {
                            if (drv.DataView.Table.Columns[i].ColumnName.Equals(this.columnName))
                            {

                                e.Row.Cells[i].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e5eef9");
                                // e.Row.Cells[i].BackColor = System.Drawing.Color.LawnGreen;
                            }
                        }
                        else
                        {
                            flag = 1;
                            break;
                        }

                    }

                }
            }


        }


        #endregion

        #region ButtonEvent

        protected void btnAddScheme_Click(object sender, EventArgs e)
        {
            DivFundShow.Visible = true;
            AddScheme();
        }

        protected void btnAddIndices_Click(object sender, EventArgs e)
        {
            DivFundShow.Visible = true;
            AddIndices();
        }

        protected void btnCompareFund_Click(object sender, EventArgs e)
        {
            DivShowPerformance.Visible = true;
            PopulateSchemeIndexCompareFund();
        }
        #endregion

        #region GetData

        private void getFundHouse()
        {
            DataTable dtFund;
            if (Cache["dtFund"] == null)
            {
                dtFund = AllMethods.getFundHouse();
                DataRow drFund = dtFund.NewRow();
                drFund["MutualFund_Name"] = "Select";
                drFund["MutualFund_ID"] = 0;
                dtFund.Rows.InsertAt(drFund, 0);
                Cache.Add("dtFund", dtFund, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtFund = (DataTable)Cache["dtFund"];
            }
            ddlFundHouse.DataSource = dtFund;
            ddlFundHouse.DataTextField = "MutualFund_Name";
            ddlFundHouse.DataValueField = "MutualFund_ID";
            ddlFundHouse.DataBind();
        }
        protected void LoadNature()
        {

            DataTable _dt = AllMethods.getNature();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlCategory.Items[0].Selected = true;
        }

        protected void LoadAllSubNature()
        {
            DataTable _dt = AllMethods.getAllSubNature();
            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Add(new ListItem("All", "-1"));
            ddlSubCategory.Items.Add(new ListItem("Large Cap", "9000"));
            ddlSubCategory.Items.Add(new ListItem("Mid & Small Cap", "9001"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlSubCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlSubCategory.Items[0].Selected = true;
        }

        protected void LoadSubNature(int id)
        {
            if (id == -1)
            {
                ddlSubCategory.Items[0].Selected = true;
                return;
            }
            DataTable _dt = AllMethods.getSubNature(id);
            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Add(new ListItem("All", "-1"));
            //if (id == 3)
            //{
            //    ddlSubCategory.Items.Add(new ListItem("Large Cap", "9000"));
            //    ddlSubCategory.Items.Add(new ListItem("Mid & Small Cap", "9001"));
            //}
            if (_dt != null)
                foreach (DataRow drow in _dt.Rows)
                {
                    ddlSubCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }

            ddlSubCategory.Items[0].Selected = true;
        }

        protected void LoadStructure()
        {
            DataTable _dt = AllMethods.getStructure();
            ddlType.Items.Clear();
            ddlType.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlType.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlType.Items[0].Selected = true;
        }

        protected void loadOption()
        {
            var OptionDataTable = AllMethods.getOption();
            rdbOption.DataSource = OptionDataTable;
            rdbOption.DataTextField = "Name";
            rdbOption.DataValueField = "Id";
            rdbOption.DataBind();
            rdbOption.SelectedIndex = 0;
        }

        private void getSchemesList()
        {
            DataTable dtScheme = new DataTable();
            //if (NatureId == "-1")
            //    dtScheme = AllMethods.getSchemeCategory(false, false);
            //else
            //    dtScheme = AllMethods.getSchemeCategory(false, false, NatureId);
            dtScheme = AllMethods.getScheme(Convert.ToInt32(ddlCategory.SelectedItem.Value),
   Convert.ToInt32(ddlType.SelectedItem.Value), Convert.ToInt32(ddlSubCategory.SelectedValue),
   Convert.ToInt32(rdbOption.SelectedValue), Convert.ToInt32(ddlFundHouse.SelectedValue));

            //DataRow drSch = dtScheme.NewRow();
            //drSch["Sch_Short_Name"] = "Select";
            //drSch["Scheme_Id"] = 0;
            //dtScheme.Rows.InsertAt(drSch, 0);


            //ddlSchemes.DataSource = dtScheme;

            //ddlSchemes.DataTextField = "Sch_Short_Name";
            //ddlSchemes.DataValueField = "Scheme_Id";
            //ddlSchemes.DataBind();

            ddlSchemes.Items.Clear();
            if (dtScheme != null)
            {
                DataRow drSch = dtScheme.NewRow();
                drSch["Sch_Short_Name"] = "Select";
                drSch["Scheme_Id"] = 0;
                dtScheme.Rows.InsertAt(drSch, 0);

                ddlSchemes.DataSource = dtScheme;
                ddlSchemes.DataTextField = "Sch_Short_Name";
                ddlSchemes.DataValueField = "Scheme_Id";
                ddlSchemes.DataBind();
            }
            else
            {
                ddlSchemes.Items.Add(new ListItem("Select", "-1"));
            }
        }

        private void getSchemesListFilter(string NatureId, List<decimal> lstSchmId)
        {

            try
            {
                //DataTable dtScheme = new DataTable();
                //if (NatureId == "-1")
                //    dtScheme = AllMethods.getSchemeCategory(false, false);
                //else
                //    dtScheme = AllMethods.getSchemeCategory(false, false, NatureId);

                DataTable dtScheme = AllMethods.getScheme(Convert.ToInt32(ddlCategory.SelectedItem.Value), Convert.ToInt32(ddlType.SelectedItem.Value), Convert.ToInt32(ddlSubCategory.SelectedValue), Convert.ToInt32(rdbOption.SelectedValue), Convert.ToInt32(ddlFundHouse.SelectedValue));
                var _dtSchemeData = dtScheme.AsEnumerable().Where(x => !lstSchmId.Contains(x.Field<decimal>("Scheme_Id")));

                if (_dtSchemeData.Any())
                {


                    DataTable vdtScheme = dtScheme.AsEnumerable().Where(x => !lstSchmId.Contains(x.Field<decimal>("Scheme_Id"))).CopyToDataTable();

                    DataRow drSch = vdtScheme.NewRow();
                    drSch["Sch_Short_Name"] = "Select";
                    drSch["Scheme_Id"] = 0;
                    vdtScheme.Rows.InsertAt(drSch, 0);

                    ddlSchemes.DataSource = vdtScheme;
                    ddlSchemes.DataTextField = "Sch_Short_Name";
                    ddlSchemes.DataValueField = "Scheme_Id";
                    ddlSchemes.DataBind();
                }
                else
                {
                    ddlSchemes.Items.Clear();
                    ddlSchemes.Items.Add(new ListItem("Select", "-1"));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private void getIndicesName()
        {
            DataTable dtIndex = AllMethods.getIndices();
            DataRow drIndex = dtIndex.NewRow();
            drIndex["INDEX_NAME"] = "Select";
            drIndex["INDEX_ID"] = 0;
            dtIndex.Rows.InsertAt(drIndex, 0);
            ddlIndices.DataSource = dtIndex;
            ddlIndices.DataTextField = "INDEX_NAME";
            ddlIndices.DataValueField = "INDEX_ID";
            ddlIndices.DataBind();
        }
        #endregion

        #region: Add Method

        public void AddScheme()
        {
            try
            {
                if (ddlSchemes.SelectedIndex != 0)
                {

                    DataTable dt = new DataTable();
                    dt.Columns.Add("SCHEME_ID", typeof(System.String));
                    dt.Columns.Add("Sch_Short_Name", typeof(System.String));
                    dt.Columns.Add("INDEX_ID", typeof(System.String));
                    dt.Columns.Add("INDEX_NAME", typeof(System.String));

                    DataView dv = AllMethods.getIndicesAgainstScheme(ddlSchemes.SelectedItem.Value).DefaultView;
                    DataTable dtScheme = dv.ToTable(true, "SCHEME_ID", "Sch_Short_Name");
                    DataTable dtIndex = dv.ToTable(true, "INDEX_ID", "INDEX_NAME");

                    string schcode = string.Empty;
                    string schname = string.Empty;
                    schcode = dtScheme.Rows[0]["SCHEME_ID"].ToString();
                    schname = dtScheme.Rows[0]["Sch_Short_Name"].ToString();
                    foreach (DataRow drInex in dtIndex.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["SCHEME_ID"] = schcode;
                        dr["Sch_Short_Name"] = schname;
                        dr["INDEX_ID"] = drInex["INDEX_ID"];
                        dr["INDEX_NAME"] = drInex["INDEX_NAME"];
                        dt.Rows.Add(dr);
                        schcode = string.Empty;
                        schname = string.Empty;
                    }

                    DataTable dtToBind = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dtToBind = RestoreDatafromGrid();

                        #region EnterRecords

                        // For Data Already exist in  Table
                        if (dtToBind.Rows.Count > 0)
                        {
                            if (dtToBind.Rows.Count <= 4)// Max 3 record  should be there to inssert
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    foreach (DataColumn clmn in dt.Columns)
                                    {
                                        if (clmn.ColumnName == "SCHEME_ID" && !string.IsNullOrEmpty(dr["SCHEME_ID"].ToString()))
                                        {
                                            DataRow dr1 = dtToBind.NewRow();
                                            int AutoID = getAutoIdfromDT(dtToBind.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x[0])).ToArray());
                                            dr1["AutoID"] = AutoID.ToString();//
                                            //  dr1["AutoID"] = dtToBind.Rows.Count.ToString();
                                            dr1["SCHEME_ID"] = Convert.ToString(dr[clmn.ColumnName]);
                                            dr1["Sch_Short_Name"] = Convert.ToString(dr["Sch_Short_Name"]);
                                            //  dr1["ImgID"] = Convert.ToString("img/key" + AutoID + ".gif");
                                            dtToBind.Rows.Add(dr1);
                                        }

                                        //if (clmn.ColumnName == "INDEX_ID" && !string.IsNullOrEmpty(dr["INDEX_ID"].ToString()))
                                        //{

                                        //    DataTable _dtIndex = dtToBind.Copy();
                                        //    DataView _dvIndId = _dtIndex.DefaultView;
                                        //    string IndexId = dr["INDEX_ID"].ToString();
                                        //    _dvIndId.RowFilter = "INDEX_ID<>''";

                                        //    DataTable _dtAfterFilter = _dvIndId.ToTable();
                                        //    DataView _dvFilterInd = _dtAfterFilter.DefaultView;
                                        //    _dvFilterInd.RowFilter = "INDEX_ID='" + IndexId + "'";
                                        //    _dtAfterFilter = _dvFilterInd.ToTable();

                                        //    if (_dtAfterFilter.Rows.Count == 0)
                                        //    {

                                        //        DataRow dr2 = dtToBind.NewRow();
                                        //        int AutoID = getAutoIdfromDT(dtToBind.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x[0])).ToArray());
                                        //        dr2["AutoID"] = AutoID.ToString();//
                                        //        //dr2["AutoID"] = dtToBind.Rows.Count.ToString();
                                        //        dr2["INDEX_ID"] = Convert.ToString(dr[clmn.ColumnName]);
                                        //        dr2["INDEX_NAME"] = Convert.ToString(dr["INDEX_NAME"]);
                                        //        // dr2["ImgID"] = Convert.ToString("img/key" + dtToBind.Rows.Count + ".gif");
                                        //       // dr2["ImgID"] = Convert.ToString("img/key" + AutoID + ".gif");
                                        //        dtToBind.Rows.Add(dr2);
                                        //    }

                                        //}

                                    }
                                }
                            }
                        }
                        else // For NO Data exist in  Table
                        {
                            int i = 0;
                            if (dt.Rows.Count > 0 && dt.Rows.Count < 3)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    foreach (DataColumn clmn in dt.Columns)
                                    {
                                        if (clmn.ColumnName == "SCHEME_ID" && !string.IsNullOrEmpty(dr["SCHEME_ID"].ToString()))
                                        {
                                            DataRow dr1 = dtToBind.NewRow();
                                            dr1["AutoID"] = i.ToString();
                                            dr1["SCHEME_ID"] = Convert.ToString(dr[clmn.ColumnName]);
                                            dr1["Sch_Short_Name"] = Convert.ToString(dr["Sch_Short_Name"]);
                                            // dr1["ImgID"] = Convert.ToString("img/key" + i + ".gif");
                                            dtToBind.Rows.Add(dr1);
                                            i = i + 1;
                                        }

                                        //if (clmn.ColumnName == "INDEX_ID" && !string.IsNullOrEmpty(dr["INDEX_ID"].ToString()))
                                        //{
                                        //    DataRow dr2 = dtToBind.NewRow();
                                        //    dr2["AutoID"] = i.ToString();
                                        //    dr2["INDEX_ID"] = Convert.ToString(dr[clmn.ColumnName]);
                                        //    dr2["INDEX_NAME"] = Convert.ToString(dr["INDEX_NAME"]);
                                        //    //dr2["ImgID"] = Convert.ToString("img/key" + i + ".gif");
                                        //    dtToBind.Rows.Add(dr2);
                                        //    i = i + 1;
                                        //}

                                    }
                                }
                            }

                        }

                        #endregion
                        //}
                    }
                    else
                    {

                        dtToBind = RestoreDatafromGrid();
                        if (dtToBind.Rows.Count > 0)
                        {
                            DataRow dr = dtToBind.NewRow();
                            dr["AutoID"] = dtToBind.Rows.Count;
                            dr["SCHEME_ID"] = ddlSchemes.SelectedItem.Value;
                            dr["Sch_Short_Name"] = ddlSchemes.SelectedItem.Text;
                            // dr["ImgID"] = Convert.ToString(@"..\img\key0.gif");
                            dtToBind.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = dtToBind.NewRow();
                            dr["AutoID"] = "0";
                            dr["SCHEME_ID"] = ddlSchemes.SelectedItem.Value;
                            dr["Sch_Short_Name"] = ddlSchemes.SelectedItem.Text;
                            //dr["ImgID"] = Convert.ToString(@"..\img\key0.gif");
                            dtToBind.Rows.Add(dr);
                        }
                    }

                    if (dtToBind.Rows.Count > 0)
                    {
                        dglist.Visible = true;

                        dglist.DataSource = dtToBind;
                        dglist.DataBind();
                        btnCompareFund.Visible = true;

                        setSchemelist();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            sethidSchIndDataTable();
        }


        private void setSchemelist()
        {
            DataTable dtToBind = RestoreDatafromGrid();
            DataView dvSchId = dtToBind.DefaultView;
            dvSchId.RowFilter = "SCHEME_ID<>''";
            DataTable dtAfterFilter = dvSchId.ToTable();

            List<decimal> lstSchId = dtAfterFilter.Rows.Cast<DataRow>().Select(x => Convert.ToDecimal(x["SCHEME_ID"])).ToList();

            if (lstSchId.Count > 0)
                getSchemesListFilter(ddlCategory.SelectedItem.Value, lstSchId);
            else
                getSchemesList();
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
                        if (dt.Rows.Count <= 4)
                        {

                            DataView dvIndcode = dt.DefaultView;
                            string indcode = ddlIndices.SelectedItem.Value.ToString();
                            dvIndcode.RowFilter = "INDEX_ID<>''";
                            DataTable dtAfterFilter = dvIndcode.ToTable();
                            DataView dvFilterInd = dtAfterFilter.DefaultView;
                            dvFilterInd.RowFilter = "INDEX_ID='" + indcode + "'";
                            dtAfterFilter = dvFilterInd.ToTable();
                            if (dtAfterFilter.Rows.Count == 0)
                            {
                                dt = RestoreDatafromGrid();
                                DataRow dr = dt.NewRow();
                                dr["AutoID"] = Convert.ToString(dt.Rows.Count);
                                //dr["ImgID"] = Convert.ToString("img/key" + dt.Rows.Count + ".gif");
                                dr["INDEX_ID"] = ddlIndices.SelectedItem.Value;
                                dr["INDEX_NAME"] = ddlIndices.SelectedItem.Text;
                                dt.Rows.Add(dr);
                            }
                            else
                            {

                                // Response.Write(@"<script>alert(""The selected Index " + ddlIndices.SelectedItem.Text + @" is already exist in Selection List.."")</script>");
                                lblErrMsg.Text = "The selected Index " + ddlIndices.SelectedItem.Text + @" is already exist in Selection List..";
                                dt = RestoreDatafromGrid();
                            }
                        }

                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["AutoID"] = "0".ToString();
                        // dr["ImgID"] = Convert.ToString("img/key0.gif");
                        dr["INDEX_ID"] = ddlIndices.SelectedItem.Value;
                        dr["INDEX_NAME"] = ddlIndices.SelectedItem.Text;
                        dt.Rows.Add(dr);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    dglist.Visible = true;
                    dglist.DataSource = dt;
                    dglist.DataBind();
                    btnCompareFund.Visible = true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            sethidSchIndDataTable();
        }


        #endregion

        #region Rest

        private int getAutoIdfromDT(params int[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!args.Contains(i))
                    return i;
            }
            return 10;
        }

        private SchemeIndexList GetSchIndDataTable()
        {
            DataTable dt = null;
            dt = RestoreDatafromGrid();

            SchemeIndexList ojSchemeIndexList = new SchemeIndexList();

            foreach (DataRow dr in dt.Select("", "SCHEME_ID desc"))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["SCHEME_ID"])) && Convert.ToString(dr["Status"]) != "0")
                {
                    ojSchemeIndexList.ListScheme.Add(Convert.ToDecimal(dr["SCHEME_ID"]));
                }
            }

            foreach (DataRow dr in dt.Select("", "INDEX_ID desc"))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["INDEX_ID"])) && Convert.ToString(dr["Status"]) != "0")
                {
                    ojSchemeIndexList.ListIndex.Add(Convert.ToDecimal(dr["INDEX_ID"]));
                }
            }

            return ojSchemeIndexList;
        }

        private void sethidSchIndDataTable()
        {
            DataTable dt = null;
            dt = RestoreDatafromGrid();

            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Select("", "SCHEME_ID desc"))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["SCHEME_ID"])))
                {
                    sb.Append("s" + dr["SCHEME_ID"] + "#");
                }
            }

            foreach (DataRow dr in dt.Select("", "INDEX_ID desc"))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["INDEX_ID"])))
                {
                    sb.Append("i" + dr["INDEX_ID"] + "#");
                }
            }

            //  hidSchindSelected.Value = sb.ToString();
        }

        #endregion


        #region Calculation Method

        public DataTable RestoreDatafromGrid()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("AutoID", typeof(System.String));
                dt.Columns.Add("SCHEME_ID", typeof(System.String));
                dt.Columns.Add("Sch_Short_Name", typeof(System.String));
                dt.Columns.Add("INDEX_ID", typeof(System.String));
                dt.Columns.Add("INDEX_NAME", typeof(System.String));
                dt.Columns.Add("Status", typeof(System.String));
                // dt.Columns.Add("ImgID", typeof(System.String));


                if (dglist.Items.Count > 0)
                {
                    for (int i = 0; i < dglist.Items.Count; i++)
                    {
                        string val_AutoID = ((Label)dglist.Items[i].Cells[2].FindControl("lblAutoID")).Text;
                        string val_SchemeCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeId")).Text;
                        string val_SchemeName = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeName")).Text;
                        string val_IndCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndId")).Text;
                        string val_IndName = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndName")).Text;
                        // string val_ImgPath = ((Image)dglist.Items[i].Cells[0].FindControl("imgKey")).ImageUrl;


                        DataRow dr = dt.NewRow();
                        dr["AutoID"] = val_AutoID;
                        dr["SCHEME_ID"] = val_SchemeCode;
                        dr["Sch_Short_Name"] = val_SchemeName;
                        dr["INDEX_ID"] = val_IndCode;
                        dr["INDEX_NAME"] = val_IndName;
                        // dr["ImgID"] = val_ImgPath;
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

        private void PopulateSchemeIndexCompareFund()
        {
            try
            {
                #region GetSchmeIndex

                GrdCompFund.DataSource = GetGridData();
                GrdCompFund.DataBind();
                lbRetrnMsg.Visible = true;
                lblSortPeriod.Visible = true;
                btnCompareFund.Visible = true;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        private DataTable GetGridData()
        {
            List<string> SelectList = getSelectedItem();

            DataTable _dtFinal = GetData(SelectList[0], SelectList[1]);
            return _dtFinal;
        }

        private List<string> getSelectedItem()
        {
            List<string> itemList = new List<string>();
            SchemeIndexList objSchInd = GetSchIndDataTable();
            StringBuilder _objSbSch = new StringBuilder();
            StringBuilder _objSbInd = new StringBuilder();

            if (objSchInd.ListScheme.Count() > 0)
            {
                _objSbSch.Append(string.Join(",", objSchInd.ListScheme.Select(k => k.ToString(CultureInfo.InstalledUICulture)).ToArray()));
            }

            if (objSchInd.ListIndex.Count() > 0)
            {
                _objSbInd.Append(string.Join(",", objSchInd.ListIndex.Select(k => k.ToString(CultureInfo.InvariantCulture)).ToList()));
            }

            if (_objSbSch.ToString() != string.Empty)
                itemList.Add(_objSbSch.ToString());
            else
                itemList.Add(string.Empty);

            if (_objSbInd.ToString() != string.Empty)
                itemList.Add(_objSbInd.ToString());
            else
                itemList.Add(string.Empty);


            return itemList;
        }

        private DataTable GetData(string strSch, string strIndex)
        {
            DataTable _dtSch = new DataTable();
            DataTable _dtInd = new DataTable();
            DataTable _dtFinal = new DataTable();

            _dtFinal.Columns.Add("Sch_id");
            _dtFinal.Columns.Add("Sch_Short_Name");
            _dtFinal.Columns.Add("Per_30_Days");
            _dtFinal.Columns.Add("Per_91_Days");
            _dtFinal.Columns.Add("Per_182_Days");
            _dtFinal.Columns.Add("Per_1_Year");
            _dtFinal.Columns.Add("Per_3_Year");
            _dtFinal.Columns.Add("Per_Since_Inception");
            _dtFinal.Columns.Add("Nav_Rs");            
            _dtFinal.Columns.Add("Nature");
            _dtFinal.Columns.Add("Sub_Nature");
            _dtFinal.Columns.Add("Option_Id");
            _dtFinal.Columns.Add("Structure_Name");
            _dtFinal.Columns.Add("status");



            if (strSch != string.Empty)
                _dtSch = AllMethods.getFundComparisonWithSI(strSch);

            if (_dtFinal != null)
            {
                for (int i = 0; i < _dtSch.Rows.Count; i++)
                {
                    _dtFinal.Rows.Add(_dtSch.Rows[i].ItemArray);
                }
            }

            _dtInd = CalculateIndexHistPerf(DateTime.Today.AddDays(-1), strIndex);


            if (_dtInd != null && _dtInd.Rows.Count > 0)
            {
                _dtInd.Columns.Add("Nav_Rs");
                _dtInd.Columns.Add("Nature");
                _dtInd.Columns.Add("Sub_Nature");
                _dtInd.Columns.Add("Option_Id");
                _dtInd.Columns.Add("Structure_Name");
                _dtInd.Columns.Add("status");

                if (_dtFinal != null)
                {
                    for (int i = 0; i < _dtInd.Rows.Count; i++)
                    {
                        _dtInd.Rows[i]["status"] = "3";
                        _dtInd.Rows[i][0] = "-1";// for disablinnf link
                        _dtFinal.Rows.Add(_dtInd.Rows[i].ItemArray);
                    }
                }
            }

            return _dtFinal;
        }

        public DataTable CalculateHistPerf(DateTime EndDate, string schmeId, string indexId)
        {
            #region :Historical Performance

            conn.ConnectionString = connstr;
            string strRollingPeriodin = "30 D,91 D,182 D,1 YYYY,3 YYYY,5 YYYY,0 Si";
            int val = 0;

            # region calling sp


            DataTable dtSchemeAbsolute = new DataTable();
            DataTable dtIndexAbsolute = new DataTable();

            SqlCommand cmd;
            SqlDataAdapter da = new SqlDataAdapter();


            //calling the sp to get Scheme Absolute return
            cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 2000;
            cmd.Parameters.Add(new SqlParameter("@SchemeIDs", schmeId));
            cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
            cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
            cmd.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
            cmd.Parameters.Add(new SqlParameter("@RollingPeriod", val));
            cmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
            cmd.Parameters.Add(new SqlParameter("@RollingFrequency", val));
            cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
            cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



            da.SelectCommand = cmd;
            da.Fill(dtSchemeAbsolute);




            cmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 2000;
            cmd.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
            cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
            cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
            cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
            cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



            da.SelectCommand = cmd;
            da.Fill(dtIndexAbsolute);



            #endregion






            if (dtSchemeAbsolute != null && dtSchemeAbsolute.Rows.Count > 0)
            {
                if (dtSchemeAbsolute.Columns.Contains("SCHEME_ID"))
                    dtSchemeAbsolute.Columns.Remove("SCHEME_ID");

            }



            if (dtIndexAbsolute != null && dtIndexAbsolute.Rows.Count > 0)
            {
                if (dtIndexAbsolute.Columns.Contains("INDEX_ID"))
                    dtIndexAbsolute.Columns.Remove("INDEX_ID");
                if (dtIndexAbsolute.Columns.Contains("INDEX_TYPE"))
                    dtIndexAbsolute.Columns.Remove("INDEX_TYPE");

                for (int i = 0; i < dtIndexAbsolute.Rows.Count; i++)
                    dtSchemeAbsolute.Rows.Add(dtIndexAbsolute.Rows[i].ItemArray);

            }



            return dtSchemeAbsolute;



            #endregion
        }

        public DataTable CalculateIndexHistPerf(DateTime EndDate, string indexId)
        {
            #region :Historical Performance

            conn.ConnectionString = connstr;
            string strRollingPeriodin = "30 D,91 D,182 D,1 YYYY,3 YYYY,0 Si";// ,5 YYYY,0 Si
            int val = 0;

            # region calling sp


            DataTable dtIndexAbsolute = new DataTable();

            SqlCommand cmd;
            SqlDataAdapter da = new SqlDataAdapter();

            cmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 2000;
            cmd.Parameters.Add(new SqlParameter("@IndexIDs", indexId));
            cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
            cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
            cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
            cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
            cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

            da.SelectCommand = cmd;
            da.Fill(dtIndexAbsolute);

            #endregion



            if (dtIndexAbsolute != null && dtIndexAbsolute.Rows.Count > 0)
            {
                //if (dtIndexAbsolute.Columns.Contains("INDEX_ID"))
                //    dtIndexAbsolute.Columns.Remove("INDEX_ID");
                if (dtIndexAbsolute.Columns.Contains("INDEX_TYPE"))
                    dtIndexAbsolute.Columns.Remove("INDEX_TYPE");
            }

            return dtIndexAbsolute;

            #endregion
        }


        private void bindGridview(string sortexpression)
        {
            DataTable dt = GetGridData();
            dt = SortDataTable(sortexpression, dt);
            if (dt != null && dt.Rows.Count != 0)
            {
                DataRow[] dr = dt.Select("status=1");
                using (DataTable newdt = dt.Clone())
                {
                    foreach (DataRow newdr in dr)
                    {
                        newdt.ImportRow(newdr);
                    }


                    DataView dataView = new DataView(newdt);
                    //Neeed to sort data here

                    // dataView.Sort = sortexpression;
                    //GrdCompFund.DataSource = dt;
                    // GrdCompFund.DataBind();                   
                    DataTable finaldt = dt.Clone();
                    using (finaldt = dataView.ToTable())
                    {

                        DataRow[] newaddr = dt.Select("status>1");

                        foreach (DataRow newddr in newaddr)
                        {
                            DataRow newRow = finaldt.NewRow();
                            //DataRowView newRow = dataView.AddNew();
                            newRow["Sch_Short_Name"] = newddr["Sch_Short_Name"];
                            newRow["Per_30_Days"] = newddr["Per_30_Days"];
                            newRow["Per_91_Days"] = newddr["Per_91_Days"];
                            newRow["Per_182_Days"] = newddr["Per_182_Days"];
                            newRow["Per_1_Year"] = newddr["Per_1_Year"];
                            newRow["Per_3_Year"] = newddr["Per_3_Year"];
                            newRow["Structure_Name"] = newddr["Structure_Name"];
                            newRow["Nature"] = newddr["Nature"];
                            newRow["Nav_Rs"] = newddr["Nav_Rs"];
                            newRow["status"] = newddr["status"];
                            finaldt.Rows.Add(newRow);
                        }
                        GrdCompFund.DataSource = finaldt;
                        GrdCompFund.DataBind();

                    }
                }
            }
        }

        private DataTable SortDataTable(string sortExp, DataTable _dtble)
        {
            DataTable tempDt = _dtble.Copy();
            int leastVal = -999999;
            string colmToSort = sortExp.Split(' ')[0].ToString();
            double dbVal = 0;

            if (tempDt != null && tempDt.Rows.Count > 0 && tempDt.Columns.Contains(colmToSort))
            {
                for (int i = 0; i < tempDt.Rows.Count; i++)
                {
                    for (int k = 0; k < tempDt.Columns.Count; k++)
                    {
                        if (tempDt.Columns[k].ColumnName.Trim().ToUpper() == colmToSort.Trim().ToUpper())
                        {

                            if (double.TryParse(tempDt.Rows[i][k].ToString().Trim(), out dbVal))
                            {
                                tempDt.Rows[i][k] = dbVal;
                            }
                            else
                            {
                                tempDt.Rows[i][k] = leastVal;
                            }
                        }
                    }
                }
            }


            DataTable dttemp = tempDt.Clone();//temp datatable for sorting
            dttemp.Columns[colmToSort].DataType = typeof(System.Double);

            foreach (DataRow drow in tempDt.Rows) { dttemp.ImportRow(drow); }
            //
            DataView dataView = new DataView(dttemp);
            dataView.Sort = sortExp;


            DataTable _dt2 = dataView.ToTable();

            DataTable _dttt = _dt2.Clone();
            _dttt.Columns[colmToSort].DataType = typeof(System.String);

            foreach (DataRow drow in _dt2.Rows) { _dttt.ImportRow(drow); }


            for (int i = 0; i < _dttt.Rows.Count; i++)
            {
                for (int k = 0; k < _dttt.Columns.Count; k++)
                {
                    if (_dttt.Columns[k].ColumnName.Trim().ToUpper() == colmToSort.Trim().ToUpper())
                    {

                        if (_dttt.Rows[i][k].ToString().Trim() == leastVal.ToString())
                        {
                            _dttt.Rows[i][k] = "NA";
                        }

                    }
                }
            }

            //return dataView.ToTable();
            return _dttt;


        }


        public string SetHyperlinkFundDetail(string schemeId, string SchemeName)
        {
            string resultStr = string.Empty;
            int Amficode = AllMethods.getAMFICode(schemeId);

            //if (schemeId != "-1")
            //    resultStr = " <a href='FundDetails.aspx?id=" + schemeId + "'><asp:Label ID='lblSchName' runat='server' Text='" + SchemeName + "'/></a>";
            //else
            //    resultStr = "<asp:Label ID='lblSchName' runat='server' Text='" + SchemeName + "'/>";

            if (schemeId != "-1")
                resultStr = " <a href='http://www.top3choice.com/factsheets.php?param=" + Amficode + "' id='urlID' target='_blank'>" + SchemeName + "</a>";
            else
                resultStr = SchemeName;

            return resultStr;
        }

        #region DropDownListEvent


        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedIndex == 0) return;
            if (ddlFundHouse.SelectedIndex != 0)
                getSchemesList();

            int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            LoadSubNature(nature_id);
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundHouse.SelectedIndex != 0)
                getSchemesList();
        }

        protected void rdbOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundHouse.SelectedIndex != 0)
                getSchemesList();
        }
        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundHouse.SelectedIndex != 0)
                getSchemesList();
        }
        protected void ddlMutualFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFundHouse.SelectedIndex != 0)
                getSchemesList();
            //else
            //{
            //    ddlSchemes.Items.Clear();
            //}
        }

        #endregion

        //protected void btnAddScheme_Click(object sender, ImageClickEventArgs e)
        //{
        //    DivFundShow.Visible = true;
        //}

        //protected void btnAddIndices_Click(object sender, ImageClickEventArgs e)
        //{
        //    DivFundShow.Visible = true;
        //}
    }
    public class SchemeIndexList
    {
        public List<decimal> ListScheme { get; set; }
        public List<decimal> ListIndex { get; set; }

        public SchemeIndexList()
        {
            ListScheme = new List<decimal>();
            ListIndex = new List<decimal>();
        }

    }
}