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
using iFrames.Classes;

namespace iFrames.WiseInvest
{
    public partial class CompareFund_New : System.Web.UI.Page
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
                getSchemesList("-1"); LoadNature(); getIndicesName();
            }
            lblErrMsg.Text = "";
        }



        #endregion

        #region DropDownListEvent


        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedIndex != 0)
                getSchemesList(ddlCategory.SelectedItem.Value);
            else
            {
                ddlSchemes.Items.Clear();
                getSchemesList(ddlCategory.SelectedItem.Value);
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
            // AddSchemesToList();
            AddScheme();
        }

        protected void btnAddIndices_Click(object sender, EventArgs e)
        {
            //AddIndicesToList();
            AddIndices();
        }

        protected void btnCompareFund_Click(object sender, EventArgs e)
        {
            // PopulateSchemeIndexCompareFund();
            ShowCompareFund();
        }
        #endregion

        #region GetData

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




        private void getSchemesList(string NatureId)
        {
            DataTable dtScheme = new DataTable();
            if (NatureId == "-1")
                dtScheme = AllMethods.getSchemeCategory();
            else
                dtScheme = AllMethods.getSchemeCategory(NatureId);


            DataRow drSch = dtScheme.NewRow();
            drSch["Sch_Short_Name"] = "Select";
            drSch["Scheme_Id"] = 0;
            dtScheme.Rows.InsertAt(drSch, 0);


            ddlSchemes.DataSource = dtScheme;

            ddlSchemes.DataTextField = "Sch_Short_Name";
            ddlSchemes.DataValueField = "Scheme_Id";
            ddlSchemes.DataBind();
        }

        private void getSchemesListFilter(string NatureId, List<decimal> lstSchmId)
        {

            try
            {
                DataTable dtScheme = new DataTable();
                if (NatureId == "-1")
                    dtScheme = AllMethods.getSchemeCategory();
                else
                    dtScheme = AllMethods.getSchemeCategory(NatureId);


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
                            if (dtToBind.Rows.Count <= 3)// Max 3 record  should be there to inssert
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
                getSchemesList(ddlCategory.SelectedItem.Value);
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

        #region old Logic
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


            _dtFinal.Columns.Add("Sch_Short_Name");
            _dtFinal.Columns.Add("Per_30_Days");
            _dtFinal.Columns.Add("Per_91_Days");
            _dtFinal.Columns.Add("Per_182_Days");
            _dtFinal.Columns.Add("Per_1_Year");
            _dtFinal.Columns.Add("Per_3_Year");
            _dtFinal.Columns.Add("Nav_Rs");
            _dtFinal.Columns.Add("Structure_Name");
            _dtFinal.Columns.Add("Nature");
            _dtFinal.Columns.Add("status");



            if (strSch != string.Empty)
                _dtSch = AllMethods.getFundComp(strSch);

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
                _dtInd.Columns.Add("Structure_Name");
                _dtInd.Columns.Add("status");

                if (_dtFinal != null)
                {
                    for (int i = 0; i < _dtInd.Rows.Count; i++)
                    {
                        _dtInd.Rows[i]["status"] = "3";
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
            string strRollingPeriodin = "30 D,91 D,182 D,1 YYYY,3 YYYY";// ,5 YYYY,0 Si
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
                if (dtIndexAbsolute.Columns.Contains("INDEX_ID"))
                    dtIndexAbsolute.Columns.Remove("INDEX_ID");
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

        #endregion

        private void ShowCompareFund()
        {
            #region Variable  Declaration and initilisation
            StringBuilder sb = new StringBuilder();
            DataSet dsCompFund = new DataSet("dataSetCompareFund");
            DataTableUtility dtu = new DataTableUtility();
            DataTable dtScheme = new DataTable("Scheme Table");
            string strPeriods = "91 D,182 D,1 YYYY,3 YYYY,5 YYYY";
            DateTime curentDate = DateTime.Today.AddDays(-1);// YesterDay Date
            DataTable SipdataTable = new DataTable();
            #endregion

             #region Collect Data Points

            #region Get Selected Scheme from the List
            List<string> SelectList = getSelectedItem();
            #endregion

            #region Get Latest Portfolio Date
            DateTime latestPortfolioDate = AllMethods.getLatestPortfiloDate(SelectList[0]);        

            #endregion

            #region    Get scheme id and name details
            dtScheme = AllMethods.getSchName(SelectList[0]);
            dsCompFund.Tables.Add(dtScheme);

            #endregion

            #region portfolio date datatable

            //portfolio dtae
            dsCompFund.Tables.Add(AllMethods.getLatestPortfiloDateDT(SelectList[0]));

            #endregion

            #region Schemes Nature

            // Get Schemes Nature
            dsCompFund.Tables.Add(AllMethods.getCategoryOfScheme(SelectList[0]));

            #endregion

            #region Get Schemes FundManger Name

            // // Get Schemes FundManger Name
            dsCompFund.Tables.Add(AllMethods.getFundManager(SelectList[0]));

            #endregion

            #region Scheme ICRON Rank

            // Get Scheme iCRON Rank
            dsCompFund.Tables.Add(AllMethods.getSchemeICRONRank(SelectList[0], "1 year"));

            #endregion

            #region Get Scheme Asset Allocation
            // Get Scheme Asset Allocation
            dsCompFund.Tables.Add(AllMethods.getAssetAllocation(SelectList[0]));

            #endregion

            #region Get Market Cap
            //Get Market Cap

            dsCompFund.Tables.Add(AllMethods.getMarketCapClassification(SelectList[0]));

            #endregion

            #region Top Holding

            // Get Scheme Top Holding
            dsCompFund.Tables.Add(AllMethods.getTopHolding(SelectList[0], 5));

            #endregion

            #region Top 3 Sector

            // Get top 3 Sector 
            dsCompFund.Tables.Add(AllMethods.getTopSector(SelectList[0], 3));

            #endregion

            #region Get Portfolio PE
            // dsCompFund.Tables.Add(AllMethods.getPortfolioPE(SelectList[0]));
            dsCompFund.Tables.Add(AllMethods.getPortfolioPE(SelectList[0],latestPortfolioDate));

            #endregion

            #region Get Avg_Mat YTM
            // get  Avg_Mat YTM

            dsCompFund.Tables.Add(AllMethods.getAvg_Mat_YTM(SelectList[0]));

            #endregion

            #region Get Expense ratio

            //get expense ratio
            dsCompFund.Tables.Add(AllMethods.getExpenseRatio(SelectList[0]));

            #endregion

            #region Get exit load


            //get exit load

            //dsCompFund.Tables.Add(AllMethods.getEntryExitLoad(SelectList[0]));
           
            //dtScheme.Columns.Add("SchemeId");

            //foreach (string strSch in SelectList[0].Split(',').ToList())
            //{
            //    dtScheme.Rows.Add(new object[] { strSch });
            //}

            dsCompFund.Tables.Add(dtu.GetEntryExitLoad(dtScheme, "Scheme_Id", "Exit", ""));

            #endregion

            #region performance
            //get performance          

            //get data from sp
            //dsCompFund.Tables.Add(AllMethods.getPerformance2(SelectList[0],strPeriods,latestPortfolioDate,2,2));          
            // get data from top Fund           

            dsCompFund.Tables.Add(AllMethods.getPerformance_TF(SelectList[0]));

            #endregion

            #region Sip analysis
            //Get sip analysis

            // get latest nav date of each scheme respective
            //DataTable navdateTable = new DataTable();
            //navdateTable = AllMethods.getLatestNavDetail(SelectList[0]);
            //DateTime leastNavDate = navdateTable.AsEnumerable().Select(x => x.Field<DateTime>("Nav_Date")).Min();
            //dsCompFund.Tables.Add(AllMethods.getSIPAnalysis(SelectList[0], 5000, leastNavDate.AddYears(-3), leastNavDate, leastNavDate, "Monthly", "Individual/HUF"));

           
            SipdataTable = AllMethods.getSIPAnalysis(SelectList[0], 5000, curentDate.AddYears(-3).AddMonths(1), curentDate, curentDate, "Monthly", "Individual/HUF");
          //  dsCompFund.Tables.Add(AllMethods.getSIPAnalysis(SelectList[0], 5000, curentDate.AddYears(-3).AddMonths(1), curentDate, curentDate, "Monthly", "Individual/HUF"));


            #endregion

            #region Risk Measure
            // Get Risk Measure

            dsCompFund.Tables.Add(AllMethods.getRiskMeasure(SelectList[0]));
            #endregion

             #endregion

            int schCount = SelectList[0].Split(',').ToList().Count();
            int SchemeWidth = ((100 - 30) / schCount);

            #region String Append start

           // sb.Append("Compare Fund<br/>");
            // Sch_Short_Name
            sb.Append(@" <div class=""Resultdiv""><div class=""mainheadingdiv""><div class=""titlediv""></div> ");

            #region Portfolio Date,Category Name,Fund Manager
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                sb.Append(@"<div class=""titledivRest""  >");// width = " + SchemeWidth.ToString() + " %
                sb.Append(dr["Sch_Short_Name"].ToString());
                sb.Append("</div>");
            }
            
            sb.Append(@"</div><div class=""blankdiv""></div><div class=""portfoliodiv""><div class=""portfolio"">Portfolio Date</div>");

            foreach (DataRow dr in dsCompFund.Tables[1].Rows)
            {
                sb.Append(@"<div  class=""portfolio_date"" >");//width =" + SchemeWidth.ToString() + "% 
                sb.Append(dr["PortDate"].ToString().Split(' ')[0]);
                sb.Append("</div>");
            }

            sb.Append(@"</div><div class=""portfoliodiv""><div class=""portfolio"">Category Name</div>");
            foreach (DataRow dr in dsCompFund.Tables[2].Rows)
            {
                sb.Append(@"<div class=""portfolio_date"" >");//width =" + SchemeWidth.ToString() + "% 
                sb.Append(dr["Nature"].ToString());
                sb.Append("</div>");
            }

            sb.Append(@"</div><div class=""portfoliodiv""><div class=""portfolio""><strong>Fund Manager</strong></div>");
            
            //foreach (DataRow dr in dsCompFund.Tables[3].Rows)
            //{
            //    sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
            //    sb.Append(dr["FUND_MANAGER_NAME"].ToString());
            //    sb.Append("</div>");
            //}


            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[3].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p =>  p.Field<string>("FUND_MANAGER_NAME"));
                var fundManName = string.Empty;
                if (data != null && data.Count() > 0)
                {
                    fundManName = string.Join(",", data.ToList());
                }

                sb.Append(@"<div class=""portfolio_date"">");// width =" + SchemeWidth.ToString() + "% 

                sb.Append(fundManName.ToString());
                sb.Append("</div>");

            }

            #endregion 

            #region Rank

            sb.Append(@"</div<div class=""portfoliodiv""><div class=""portfolio""><strong>ICRON Rank (1 Year)</strong></div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[4].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                   .Select(p => p.Field<string>("RANK")).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" >");//width =" + SchemeWidth.ToString() + "% 
                if (data != null)
                {
                    var rank = data.ToString();
                    if (!string.IsNullOrEmpty(rank))
                    {
                        for (int n = 1; n <= Convert.ToInt16(rank.TrimStart().Split(' ')[0]); n++)
                        {
                            //sb.Append(@"<i class=""icon-star""></i>");
                            sb.Append(@"<img src =""./img/glyphicons-halflings.png"" style ="" height: 13px;width: 12px;""></img>");
                        }
                    }
                    else
                    {
                        sb.Append("NIL");
                    }
                }
                else
                {
                    sb.Append("NIL");
                }

                sb.Append("</div>");
            }

            #endregion

            #region asset
//            sb.Append(@" <div class=""accordion"">
//                	<div class=""accordion-group"">
//                   	  <div class=""accordion-heading"">
//                       	<A class=""accordion-toggle"" href=""#collapseFive"" data-toggle=""collapse"" data-parent=""#accordion5"">Asset Allocation</A>
//                      </div><div id=""collapseFive"" class=""accordion-body collapse in""><div class=""portfoliodiv"">
//                                <div class=""asset"">Equity</div>");

            sb.Append(@" <div class=""dhtmlgoodies_question"">Asset Allocation</div><div class=""dhtmlgoodies_answer"">");
            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">Equity</div>");

            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[5].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                    .Where(x => x.Field<string>("NATURE_NAME") == "EQ")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("Corpus_per")), 2)).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(data.ToString() +" %");
                sb.Append("</div>");
            }
            sb.Append(@"</div><div class=""portfoliodiv""><div class=""asset"">Debt</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[5].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                    .Where(x => x.Field<string>("NATURE_NAME") == "Debt")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("Corpus_per")), 2)).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(data.ToString() + " %");
                sb.Append("</div>");
            }

            sb.Append(@"</div><div class=""portfoliodiv""><div class=""asset"">Others</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[5].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                    .Where(x => x.Field<string>("NATURE_NAME") == "Others")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("Corpus_per")), 2)).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(data.ToString() + " %");
                sb.Append("</div>");
            }
          //  sb.Append("</div></div></div></div>");
            sb.Append("</div></div>");
            #endregion
            
            #region Market Cap
//            sb.Append(@" <div class=""accordion"">
//                	<div class=""accordion-group"">
//                   	  <div class=""accordion-heading"">
//                       	<A class=""accordion-toggle"" href=""#collapseOne"" data-toggle=""collapse"" data-parent=""#accordion1"">
//Market Cap</A>
//                      </div><div id=""collapseOne"" class=""accordion-body collapse in""><div class=""portfoliodiv"">
//                                <div class=""asset"">Large Cap</div>");

            sb.Append(@" <div class=""dhtmlgoodies_question"">Market Cap</div><div class=""dhtmlgoodies_answer"">");
            sb.Append(@"<div class=""portfoliodiv""><div class=""asset"">Large Cap</div>");

            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[6].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                      .Where(x => x.Field<string>("MARKET_SLAB") == "Large Cap")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("MCAPALLOCATION")), 2)).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(data.ToString() + " %");
                sb.Append("</div>");
            }
            sb.Append(@"</div><div class=""portfoliodiv""><div class=""asset"">Mid Cap</div>");

            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[6].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                      .Where(x => x.Field<string>("MARKET_SLAB") == "Mid Cap")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("MCAPALLOCATION")), 2)).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(data.ToString() + " %");
                sb.Append("</div>");
            }
            sb.Append(@"</div><div class=""portfoliodiv""><div class=""asset"">Small Cap</div>");

            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[6].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                      .Where(x => x.Field<string>("MARKET_SLAB") == "Small Cap")
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("MCAPALLOCATION")), 2)).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(data.ToString() + " %");
                sb.Append("</div>");
            }
           // sb.Append("</div></div></div></div>");
            sb.Append("</div></div>");
            #endregion

            #region Top 5 Holding

            sb.Append(@"</div<div class=""portfoliodiv""><div class=""portfolio""><strong>Top 5 Holding</strong></div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[7].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                    .Select(p => Math.Round(Convert.ToDecimal(p.Field<double>("SumCorpus")), 2)).SingleOrDefault();
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(data.ToString() +" %");
                sb.Append("</div>");
            }

            #endregion

            #region Top 3 sector
//            sb.Append(@" <div class=""accordion"">
//                	<div class=""accordion-group"">
//                   	  <div class=""accordion-heading"">
//                       	<A class=""accordion-toggle"" href=""#collapseSec"" data-toggle=""collapse"" data-parent=""#accordionSec"">
//Top 3 Sectors</A>
//                      </div><div id=""collapseSec"" class=""accordion-body collapse in"">");

            sb.Append(@" <div class=""dhtmlgoodies_question_re toggleClass""><A href=""#"" id =""top3sec"" > Top 3 Sectors</A> </div>");
            #region total
            //sb.Append(@"<div class=""portfoliodiv""><div class=""portfolio""><strong>Total:</strong></div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[8].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => new { SumCorpus = p.Field<double>("SumCorpus") });

                sb.Append(@"<div class=""top_3sectorRe_dhtmlgoodies"">");
                sb.Append(Math.Round(data.Select(s => s.SumCorpus).Sum(), 2).ToString() + " %");
                sb.Append("</div>");

            }
            sb.Append("</div>");
            #endregion
            sb.Append(@"<div class=""dhtmlgoodies_answer"" id =""top3secDiv"">");
            for (int i = 0; i < 3; i++)
            {
                sb.Append(@"<div class=""portfoliodiv"">");
                //sb.Append(@"<div class=""top_3sector"" width =""20%"" ></div>");
              // sb.Append(@"<div style=""width:30%; margin-left:25%"">");
                sb.Append(@"<div class=""assetRe"">" + (i+1).ToString() +" </div>");
                foreach (DataRow dr in dsCompFund.Tables[0].Rows)
                {

                    var collection = dsCompFund.Tables[8].AsEnumerable();
                    var data = collection.Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => new { Sector_Name = p.Field<string>("Sector_Name"), SumCorpus = p.Field<double>("SumCorpus") }).Skip(i).Take(1);

                    sb.Append(@"<div class=""top_3sectorRe""  >");//width =" + Convert.ToInt32((80 / schCount)).ToString() + "%
                    //var valData = "--";
                    //valData = Math.Round(Convert.ToDecimal(data.Select(x => x.SumCorpus).FirstOrDefault().ToString()), 2).ToString() + "% (" + data.Select(x => x.Sector_Name).FirstOrDefault().ToString() + ")";
                    sb.Append(Math.Round(Convert.ToDecimal( data.Select(x => x.SumCorpus).FirstOrDefault().ToString()),2).ToString() + "% (" + data.Select(x => x.Sector_Name).FirstOrDefault().ToString() + ")");
                    sb.Append("</div>");
                }
                sb.Append("</div>");
            }
            
            sb.Append("</div>");
            sb.Append(@"<p class=""divLine"">&nbsp;&nbsp;</p>");
              #endregion

          //  sb.Append(@"<p class=""divLine""> &nbsp;<hr/></p>");

            #region Portfolio PE (Times)
            sb.Append(@"<div class=""portfoliodiv""><div class=""portfolio""><strong>Portfolio PE (Times)</strong></div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[9].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => new { PRICE_EARNING = p.Field<double>("PRICE_EARNING") });

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(Math.Round(data.Select(s => s.PRICE_EARNING).Take(1).SingleOrDefault(), 2).ToString() + "%");
                sb.Append("</div>");

            }
            sb.Append("</div>");
            #endregion

            
            #region Average Maturity
            sb.Append(@"<div class=""portfoliodiv""><div class=""portfolio""><strong>Average Maturity</strong></div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[10].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("AVG_MAT")).Take(1).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                var valdata = "--";
                if (data != null && data != -1)
                    valdata = Convert.ToString( Math.Round((double)data / 365, 2)) + " Years";

                sb.Append(valdata);
                sb.Append("</div>");

            }
            sb.Append("</div>");
            #endregion


            #region YTM
            sb.Append(@"<div class=""portfoliodiv""><div class=""portfolio""><strong>YTM</strong></div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[10].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("YTM")).Take(1).SingleOrDefault();

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                var valdata = "--";
                if (data != null)
                    valdata = Convert.ToString(Math.Round((double)data, 2)) + " %";

                sb.Append(valdata.ToString());
                sb.Append("</div>");

            }
            sb.Append("</div>");
            #endregion

          
             #region  Expense Ratio
            sb.Append(@"<div class=""portfoliodiv""><div class=""portfolio"" style=""height:35px;""><strong> Expense Ratio </strong></div>");//#MaxExpenseDate# 
            //var maxExpRatioDate = dsCompFund.Tables[11].AsEnumerable().Select(p => p.Field<DateTime?>("Calc_date")).Max();
            //if (maxExpRatioDate != null)
            //    sb.Replace("#MaxExpenseDate#", "( as on " + maxExpRatioDate.ToString().Split(' ')[0] + " )");
            //else
            //    sb.Replace("#MaxExpenseDate#", " ");

            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[11].AsEnumerable().Where(x => Convert.ToString(x.Field<decimal>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                    .OrderBy(p => p.Field<double?>("Exp_Ratio"))
                       .Select(p => p.Field<double?>("Exp_Ratio")).Take(1).SingleOrDefault();                

                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                if (data != null)
                    data = Math.Round((double)data, 2);
                else
                    data = 0;
                sb.Append(data.ToString() + " %");
                sb.Append("</div>");

            }
            sb.Append("</div>");
            #endregion


            #region  Exit load
            sb.Append(@"<div class=""portfoliodiv""><div class=""portfolio"" style=""height:35px;""><strong> Exit load</strong></div>");
       

            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[12].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())                    
                       .Select(p => p.Field<string>("Statement")).Take(1).SingleOrDefault();

                sb.Append(@"<div class=""top_3sector"" width =" + SchemeWidth.ToString() + "% >");
                
                sb.Append(data.ToString());
                sb.Append("</div>");

            }
            sb.Append("</div>");
            #endregion

            #region Performance
//            sb.Append(@" <div class=""accordion"">
//                	<div class=""accordion-group"">
//                   	  <div class=""accordion-heading"">
//                       	<A class=""accordion-toggle"" href=""#collapsePerf"" data-toggle=""collapse"" data-parent=""#accordionPerf"">
//Performance</A>
//                      </div><div id=""collapsePerf"" class=""accordion-body collapse in"">");
            sb.Append(@" <div class=""dhtmlgoodies_question"">Performance</div><div class=""dhtmlgoodies_answer"">"); 
            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">3 Months Return</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[13].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("Per_91_Days")).Take(1).SingleOrDefault();
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";  
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">6 Months Return</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[13].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("Per_182_Days")).Take(1).SingleOrDefault();
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
            sb.Append("</div>");


            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">1 Year Return</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[13].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("Per_1_Year")).Take(1).SingleOrDefault();
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
            sb.Append("</div>");


            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">3 Years Return</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[13].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("Per_3_Year")).Take(1).SingleOrDefault();
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">5 Years Return</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[13].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("Per_5_Year")).Take(1).SingleOrDefault();
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
           // sb.Append("</div></div></div></div>");
            sb.Append("</div></div>");
            #endregion

            #region SIP
//            sb.Append(@" <div class=""accordion"">
//                	<div class=""accordion-group"">
//                   	  <div class=""accordion-heading"">
//                       	<A class=""accordion-toggle"" href=""#collapseSip"" data-toggle=""collapse"" data-parent=""#accordionSip"">
//SIP Analysis</A>
//                      </div><div id=""collapseSip"" class=""accordion-body collapse in"">");
            sb.Append(@" <div class=""dhtmlgoodies_question"">SIP Analysis (Installment Amount Rs 5000 per month for 3 years)</div><div class=""dhtmlgoodies_answer"">");
            sb.Append(@" <div class=""portfoliodiv""><div class=""returnSip""> Invested Amount (Rs.)</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                //var data = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme")) == dr["Sch_Short_Name"].ToString())
                //       .Select(p => p.Field<double?>("TOTAL_AMOUNT")).Take(1).SingleOrDefault();

                var data = SipdataTable.AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme")) == dr["Sch_Short_Name"].ToString())
                       .Select(p => p.Field<double?>("TOTAL_AMOUNT")).Take(1).SingleOrDefault();

                
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString();
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">Value of SIP (Rs.)</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                //var data = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme")) == dr["Sch_Short_Name"].ToString())
                //       .Select(p => p.Field<double?>("PRESENT_VALUE")).Take(1).SingleOrDefault();

                var data = SipdataTable.AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme")) == dr["Sch_Short_Name"].ToString())
                       .Select(p => p.Field<double?>("PRESENT_VALUE")).Take(1).SingleOrDefault();


                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString();
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">Return </div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                //var data = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme")) == dr["Sch_Short_Name"].ToString())
                //       .Select(p => p.Field<double?>("YIELD")).Take(1).SingleOrDefault();

                 var data = SipdataTable.AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme")) == dr["Sch_Short_Name"].ToString())
                       .Select(p => p.Field<double?>("YIELD")).Take(1).SingleOrDefault();



                 var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
           // sb.Append("</div></div></div></div>");
            sb.Append("</div></div>");

            #endregion

           

            #region Risk Measure
//            sb.Append(@" <div class=""accordion"">
//                	<div class=""accordion-group"">
//                   	  <div class=""accordion-heading"">
//                       	<A class=""accordion-toggle"" href=""#collapseRisk"" data-toggle=""collapse"" data-parent=""#accordionRisk"">Risk Measures</A>
//                      </div><div id=""collapseRisk"" class=""accordion-body collapse in"">");

            sb.Append(@" <div class=""dhtmlgoodies_question"">Risk Measures</div><div class=""dhtmlgoodies_answer"">"); 

            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">Sharpe</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("Sharp")).Take(1).SingleOrDefault();
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString() );
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">Sortino</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("Sortino")).Take(1).SingleOrDefault();
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append(@" <div class=""portfoliodiv""><div class=""asset"">Standard Deviation</div>");
            foreach (DataRow dr in dsCompFund.Tables[0].Rows)
            {
                var data = dsCompFund.Tables[14].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Scheme_Id")) == dr["Scheme_Id"].ToString())
                       .Select(p => p.Field<double?>("STDV")).Take(1).SingleOrDefault();
                var no = "--";
                if (data != null)
                    no = Math.Round((double)data, 2).ToString() + " %";
                sb.Append(@"<div class=""portfolio_date"" width =" + SchemeWidth.ToString() + "% >");
                sb.Append(no.ToString());
                sb.Append("</div>");
            }
           // sb.Append("</div></div></div></div></div>");
            sb.Append("</div></div>");
            #endregion

            #endregion
            resultCompDiv.InnerHtml = sb.ToString();
        }


    }



}