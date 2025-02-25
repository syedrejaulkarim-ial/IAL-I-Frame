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
using iFrames.DAL;

namespace iFrames.LNT.Tools
{
    public partial class NAVGraph : System.Web.UI.Page
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
            //  Response.Redirect("BansalProxy.aspx?u=http%3a%2f%2flocalhost:52348/WebMethod.aspx/CheckSession");
            //this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "","self.parent.location=’DEFAULT.aspx’;”, true);
            if (!string.IsNullOrEmpty(Request.QueryString["user"]))
            {
                Userid.Value = Request.QueryString["user"];
            }
            else
            {
                Userid.Value = "WrongUser";
            }
            if (!IsPostBack)
            {
                getIndicesName();
                loadOption();
                getSchemesList();
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


                    //new
                    var _Schs = dt.AsEnumerable().Where(x => !string.IsNullOrEmpty(Convert.ToString(x["SCHEME_ID"])))
                        .Select(y => y["SCHEME_ID"]);
                    if (_Schs.Any())
                    {
                        var Schs = string.Join(",", _Schs);
                        DataView SchemesIndex = AllMethods.getIndicesAgainstScheme(Schs).DefaultView;

                        var Sch = dt.AsEnumerable().Where(x => Convert.ToString(x["AutoID"]) == autoid);
                        if (Sch.Any())
                        {
                            var SchId = Convert.ToString(Sch.FirstOrDefault()["SCHEME_ID"]);
                            if (!string.IsNullOrEmpty(SchId))
                            {
                                DataView dvIndex = AllMethods.getIndicesAgainstScheme(SchId).DefaultView;
                                for (int s = 0; s < dvIndex.Table.Rows.Count; s++)
                                {
                                    var Ind = dt.AsEnumerable().Where(x => Convert.ToString(x["INDEX_ID"]) == Convert.ToString(dvIndex[s].Row["INDEX_ID"]));
                                    if (Ind.Any())
                                    {
                                        var IndId = Convert.ToString(Ind.FirstOrDefault()["INDEX_ID"]);
                                        var ChkDuplicate = SchemesIndex.Table.AsEnumerable().Where(v => Convert.ToString(v["INDEX_ID"]) == IndId);
                                        if (ChkDuplicate.Count() > 1)
                                            break;
                                        if (dv.Table.AsEnumerable().Where(x => Convert.ToString(x["INDEX_ID"]) == IndId).Any())
                                        {
                                            var IndexAutoID = dv.Table.AsEnumerable().Where(x => Convert.ToString(x["INDEX_ID"]) == IndId)
                                                .FirstOrDefault()["AutoID"];
                                            if (string.IsNullOrEmpty(dv.RowFilter))
                                                dv.RowFilter = "AutoID <>" + IndexAutoID + "";
                                            else
                                                dv.RowFilter = dv.RowFilter + "and AutoID <>" + IndexAutoID + "";

                                        }
                                    }
                                }
                            }
                        }
                    }
                    //end New
                    if (string.IsNullOrEmpty(dv.RowFilter))
                        dv.RowFilter = "AutoID <>" + autoid + "";
                    else
                        dv.RowFilter = dv.RowFilter + "and AutoID <>" + autoid + "";

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
            ddlIndices.SelectedIndex = -1;
        }

        protected void btnCompareFund_Click(object sender, EventArgs e)
        {
            DivShowPerformance.Visible = true;
            PopulateSchemeIndexCompareFund();
        }
        #endregion

        #region GetData    

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
            dtScheme = AllMethods.getScheme(-1, -1, -1, Convert.ToInt32(rdbOption.SelectedValue), 43); //HSBC  //For L&T     

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

                DataTable dtScheme = AllMethods.getScheme(-1, -1, -1, Convert.ToInt32(rdbOption.SelectedValue), 43);
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
                                            dr1["IsVisible"] = true;
                                            dtToBind.Rows.Add(dr1);
                                        }

                                        if (clmn.ColumnName == "INDEX_ID" && !string.IsNullOrEmpty(dr["INDEX_ID"].ToString()))
                                        {

                                            DataTable _dtIndex = dtToBind.Copy();
                                            DataView _dvIndId = _dtIndex.DefaultView;
                                            string IndexId = dr["INDEX_ID"].ToString();
                                            _dvIndId.RowFilter = "INDEX_ID<>''";

                                            DataTable _dtAfterFilter = _dvIndId.ToTable();
                                            DataView _dvFilterInd = _dtAfterFilter.DefaultView;
                                            _dvFilterInd.RowFilter = "INDEX_ID='" + IndexId + "'";
                                            _dtAfterFilter = _dvFilterInd.ToTable();

                                            if (_dtAfterFilter.Rows.Count == 0)
                                            {

                                                DataRow dr2 = dtToBind.NewRow();
                                                int AutoID = getAutoIdfromDT(dtToBind.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x[0])).ToArray());
                                                dr2["AutoID"] = AutoID.ToString();//
                                                //dr2["AutoID"] = dtToBind.Rows.Count.ToString();
                                                dr2["INDEX_ID"] = Convert.ToString(dr[clmn.ColumnName]);
                                                dr2["INDEX_NAME"] = Convert.ToString(dr["INDEX_NAME"]);
                                                // dr2["ImgID"] = Convert.ToString("img/key" + dtToBind.Rows.Count + ".gif");
                                                dr2["IsVisible"] = false;

                                                dtToBind.Rows.Add(dr2);
                                            }

                                        }

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
                                            dr1["IsVisible"] = true;

                                            dtToBind.Rows.Add(dr1);
                                            i = i + 1;
                                        }

                                        if (clmn.ColumnName == "INDEX_ID" && !string.IsNullOrEmpty(dr["INDEX_ID"].ToString()))
                                        {
                                            DataRow dr2 = dtToBind.NewRow();
                                            dr2["AutoID"] = i.ToString();
                                            dr2["INDEX_ID"] = Convert.ToString(dr[clmn.ColumnName]);
                                            dr2["INDEX_NAME"] = Convert.ToString(dr["INDEX_NAME"]);
                                            dr2["IsVisible"] = false;

                                            dtToBind.Rows.Add(dr2);
                                            i = i + 1;
                                        }

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
                            dr["IsVisible"] = true;
                            dtToBind.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = dtToBind.NewRow();
                            dr["AutoID"] = "0";
                            dr["SCHEME_ID"] = ddlSchemes.SelectedItem.Value;
                            dr["Sch_Short_Name"] = ddlSchemes.SelectedItem.Text;
                            dr["IsVisible"] = true;
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
                getSchemesListFilter("-1", lstSchId);
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
                                dr["IsVisible"] = true;
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
                        dr["IsVisible"] = true;
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
                dt.Columns.Add("IsVisible", typeof(System.Boolean));


                if (dglist.Items.Count > 0)
                {
                    for (int i = 0; i < dglist.Items.Count; i++)
                    {
                        string val_AutoID = ((Label)dglist.Items[i].Cells[2].FindControl("lblAutoID")).Text;
                        string val_SchemeCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeId")).Text;
                        string val_SchemeName = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeName")).Text;
                        string val_IndCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndId")).Text;
                        string val_IndName = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndName")).Text;
                        string val_IsVisible = ((Label)dglist.Items[i].Cells[0].FindControl("IsVisible")).Text;


                        DataRow dr = dt.NewRow();
                        dr["AutoID"] = val_AutoID;
                        dr["SCHEME_ID"] = val_SchemeCode;
                        dr["Sch_Short_Name"] = val_SchemeName;
                        dr["INDEX_ID"] = val_IndCode;
                        dr["INDEX_NAME"] = val_IndName;
                        dr["IsVisible"] = Convert.ToBoolean(val_IsVisible);

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
            HdSchemes.Value = string.Join("#", SelectList[0].Split(',').Select(x => "s" + x.ToString())) + "#" +
                              string.Join("#", SelectList[1].Split(',').Select(x => "i" + x.ToString()));

            HdSchemes.Value = HdSchemes.Value.TrimEnd('i');
            HdSchemes.Value = HdSchemes.Value.TrimEnd('s');
            HdSchemes.Value = HdSchemes.Value.TrimEnd('#');


            HdToData.Value = DateTime.Today.ToString("dd MMM yyyy");
            HdFromData.Value = DateTime.Today.AddYears(-3).ToString("dd MMM yyyy");

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

            #region New
            DataTable dtOnlySch = new DataTable();
            dtOnlySch.Columns.Add("SCHEME_ID");
            dtOnlySch.Columns.Add("SCH_SHORT_NAME");
            dtOnlySch.Columns.Add("INDEX_IDS");

            DataView dvAll = AllMethods.getIndicesAgainstScheme(strSch).DefaultView;
            if(dvAll.Count>0)
            {
                foreach (DataRow r1 in dvAll.ToTable().Rows)
                {
                    var ChkMultipleSCH = dtOnlySch.AsEnumerable().Where(x => Convert.ToString(x["SCHEME_ID"]) == Convert.ToString(r1["SCHEME_ID"]));
                    if (ChkMultipleSCH.Any())
                    {
                        ChkMultipleSCH.FirstOrDefault()["INDEX_IDS"] =
                            Convert.ToString(ChkMultipleSCH.FirstOrDefault()["INDEX_IDS"]) + "," + Convert.ToString(r1["INDEX_ID"]);
                    }
                    else
                    {
                        DataRow _r = dtOnlySch.NewRow();
                        _r["SCHEME_ID"] = r1["SCHEME_ID"];
                        _r["SCH_SHORT_NAME"] = r1["Sch_Short_Name"];
                        if (strIndex.Split(',').Contains(Convert.ToString(r1["INDEX_ID"])))
                            _r["INDEX_IDS"] = r1["INDEX_ID"];
                        dtOnlySch.Rows.Add(_r);
                    }
                }
            }
            
            Repeater1.DataSource = dtOnlySch;
            Repeater1.DataBind();

            StringBuilder stringBuilder = new StringBuilder();
            using (var db = new PrincipalCalcDataContext())
            {
                //foreach (DataRow r1 in dtOnlySch.Rows)
                for(int h=0;h< dtOnlySch.Rows.Count;h++)
                {
                    DataRow r1 = dtOnlySch.Rows[h];
                    var val_SchemeCode = Convert.ToString(r1["SCHEME_ID"]);
                    var val_SchemeName = Convert.ToString(r1["SCH_SHORT_NAME"]);

                    if (!string.IsNullOrEmpty(val_SchemeCode))
                    {
                        if (stringBuilder.Length > 0 && h != 0)
                        {
                            if (h == dtOnlySch.Rows.Count - 1)
                                stringBuilder.Append(" and ");
                            else
                                stringBuilder.Append(" , ");
                        }
                        var fundid = db.T_SCHEMES_MASTERs.Where(n => n.Scheme_Id == int.Parse(val_SchemeCode)).Select(x => x.Fund_Id).FirstOrDefault();

                        var fundMang = db.T_CURRENT_FUND_MANAGERs.Where(x => x.FUND_ID == fundid && x.LATEST_FUNDMAN == true).Select(x => x.FUNDMAN_ID);

                        var _fundManagerName = db.T_FUND_MANAGERs.Where(x => fundMang.Contains(x.FUNDMAN_ID)).Select(x => x.FUND_MANAGER_NAME);
                        if(_fundManagerName.Any())
                        {
                            var fundManagerName = _fundManagerName.ToArray();
                            for (int o = 0; o < fundManagerName.Length; o++)
                            {
                                if (stringBuilder.Length > 0 && o != 0)
                                {
                                    if (o == fundManagerName.Length - 1)
                                        stringBuilder.Append(" and Mr. ");
                                    else
                                        stringBuilder.Append(" , ");
                                }
                                stringBuilder.Append(fundManagerName[o]);
                            }
                            stringBuilder.Append(" manages ");
                            stringBuilder.Append(val_SchemeName);
                        }
                        
                    }
                }
            }
            string _StrSch = "";
            if (dtOnlySch.Rows.Count>0)
                foreach (DataRow _s in dtOnlySch.Rows)
                    _StrSch = _StrSch + Convert.ToString(_s["SCH_SHORT_NAME"]) + ",";

            _StrSch = _StrSch.TrimEnd(',');
            if (_StrSch.Split(',').Count() > 1)
                _StrSch = _StrSch.Remove(_StrSch.LastIndexOf(','), 5)
                    .Insert(_StrSch.LastIndexOf(','), " and ");

            divClientDisclaimer.InnerHtml = @"<div style='width:100%; float:left;'>
                                                       *Disclaimer: Past performance may or may not be sustained in the future. *Point to Point (PTP) Returns in INR show 
                                                        the value of Rs. 10,000/- invested. Since inception return is calculated on NAV of Rs. 10/- invested at inception. CAGR is compounded annualized. 
                                                        Date of inception is deemed to be date of allotment. Mr. " + stringBuilder + @" schemes. Different plans shall have a different expense structure.
                                                        The performance details have been provided for " + _StrSch + @" Plan. In case, the start/end date of the concerned period is a non - business day (NBD), 
                                                        the NAV of the previous date is considered for computation of returns.
                                                    </div>";
            #endregion




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
        public DataTable CalculateIndexHistPerfNew(DateTime EndDate, string indexId, string strSch, DataTable swdataTable)
        {
            try
            {
                #region :Historical Performance

                conn.ConnectionString = connstr;
                double amount = 10000;
                string strRollingPeriodin = "1 YYYY,3 YYYY,5 YYYY,0 Si";

                DateTime CurrentDatyOfvMth = DateTime.Now.AddDays(-1);

                //TimeSpan dateDiff4Index = LastDatyOfPrevMth.Subtract(allotDate);
                int dayDiffIndex = 0;

                int val = 0;
                Decimal dcmlIndexid = 0;
                string schemeAmficode = String.Empty;
                string strNature;
                int SettingId = 2;
                DataTable perfDataTable = new DataTable();
                using (var db = new PrincipalCalcDataContext())
                {
                    var data = AllMethods.getFundSchemeId(strSch);

                    var MaxNavDt = db.T_NAV_DIVs.Where(x => data.Contains(x.Scheme_Id)).Select(x => x).OrderByDescending(v => v.Nav_Date).FirstOrDefault();

                    var data1 = db.T_SCHEMES_MASTERs.Where(x => data.Contains(x.Scheme_Id)).Select(x => new { Launch_Date = x.Launch_Date, Fund_id = x.Fund_Id, Scheme_Id = x.Scheme_Id }).ToList();
                    
                    var indexSplit = indexId.Split(',');
                    perfDataTable.Columns.Add("Sch_Short_Name", typeof(string));

                    perfDataTable.Columns.Add("Per_1_Year", typeof(string));
                    perfDataTable.Columns.Add("1YAmount", typeof(string));
                    perfDataTable.Columns.Add("Per_3_Year", typeof(string));
                    perfDataTable.Columns.Add("3YAmount", typeof(string));
                    perfDataTable.Columns.Add("Per_5_Year", typeof(string));
                    perfDataTable.Columns.Add("5YAmount", typeof(string));
                    perfDataTable.Columns.Add("Per_Since_Inception", typeof(string));
                    perfDataTable.Columns.Add("SIAmount", typeof(string));
                    perfDataTable.Columns.Add("Nav_Rs", typeof(string));
                    perfDataTable.Columns.Add("Nature", typeof(string));
                    perfDataTable.Columns.Add("Sub_Nature", typeof(string));
                    perfDataTable.Columns.Add("Option_Id", typeof(string));
                    perfDataTable.Columns.Add("Structure_Name", typeof(string));
                    perfDataTable.Columns.Add("status", typeof(string));

                    perfDataTable.Columns.Add("ID", typeof(int));
                    perfDataTable.Columns.Add("TYPE", typeof(string));
                    perfDataTable.Columns.Add("REF_SCH", typeof(int));

                    DataView dvAll = AllMethods.getIndicesAgainstScheme(strSch).DefaultView;
                    var IndividualIndex = new List<string>();
                    foreach (var schInd in indexSplit)
                    {
                        var AllSchemeInd = dvAll.ToTable(true, "INDEX_ID", "INDEX_NAME").AsEnumerable().
                       Where(x => Convert.ToString(x["INDEX_ID"]) == schInd);
                        if (!AllSchemeInd.Any())
                            IndividualIndex.Add(schInd);
                    }

                    List<double> SchemeNavVal = new List<double>();
                    List<double> SchemeAmountVal = new List<double>();
                    int k = 0;
                    foreach (var m in data1)
                    {
                        for (var b = 0; b < swdataTable.Rows.Count; b++)
                        {
                            if (Convert.ToString(swdataTable.Rows[b]["Sch_Id"]) == m.Scheme_Id.ToString())
                            {
                                k = b;
                                break;
                            }
                        }

                        if (swdataTable.Rows.Count > 0)
                        {
                            for (int i = 2; i < swdataTable.Columns.Count; i++)
                            {

                                if (m.Scheme_Id == Decimal.Parse(swdataTable.Rows[k]["Sch_Id"].ToString()))
                                {
                                    int day2;
                                    if (swdataTable.Rows[k][i].ToString() != "" && swdataTable.Rows[k][i].ToString() != "N/A")
                                    {
                                        if (i < 6)
                                        {
                                            var daysDiffVal1 = Convert.ToDouble(swdataTable.Rows[k][i]);
                                            SchemeNavVal.Add(Convert.ToDouble(daysDiffVal1));
                                            if (i == 2)
                                            {
                                                day2 = 365 * 1;
                                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                                            }
                                            else if (i == 3)
                                            {
                                                day2 = 365 * 3;
                                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                                            }
                                            else if (i == 4)
                                            {
                                                day2 = 365 * 5;
                                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                                            }

                                            else if (i == 5)
                                            {
                                                DateTime dateDiffSiw = MaxNavDt.Nav_Date.Value;
                                                //DateTime dateDiffSiw = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                                                TimeSpan dateDiffSi = dateDiffSiw.Subtract(m.Launch_Date.Value);
                                                day2 = dateDiffSi.Days;
                                                SchemeAmountVal.Add(amount * Math.Pow(1 + Math.Round((double)daysDiffVal1, 2) / 100, Math.Round((double)day2 / 365, 6)));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        SchemeNavVal.Add(0);
                                        SchemeAmountVal.Add(0);
                                    }
                                }

                            }

                            perfDataTable.Rows.Add(swdataTable.Rows[k]["Sch_Short_Name"].ToString(),
                                                    SchemeNavVal[0] == 0 ? "--" : SchemeNavVal[0].ToString(),
                                                    SchemeAmountVal[0] == 0 ? "--" : SchemeAmountVal[0].ToString(),
                                                    SchemeNavVal[1] == 0 ? "--" : SchemeNavVal[1].ToString(),
                                                    SchemeAmountVal[1] == 0 ? "--" : SchemeAmountVal[1].ToString(),
                                                    SchemeNavVal[2] == 0 ? "--" : SchemeNavVal[2].ToString(),
                                                    SchemeAmountVal[2] == 0 ? "--" : SchemeAmountVal[2].ToString(),
                                                    SchemeNavVal[3] == 0 ? "--" : SchemeNavVal[3].ToString(),
                                                    SchemeAmountVal[3] == 0 ? "--" : SchemeAmountVal[3].ToString(),
                                                    swdataTable.Rows[k]["Per_5_Year"].ToString(),
                                                    swdataTable.Rows[k]["5YAmount"].ToString(),
                                                    swdataTable.Rows[k]["Sub_Nature"].ToString(),
                                                    swdataTable.Rows[k]["Option_Id"].ToString(),
                                                    swdataTable.Rows[k]["Nav_Rs"].ToString(),
                                                    swdataTable.Rows[k]["status"].ToString(),
                                                    swdataTable.Rows[k]["Sch_id"].ToString(),
                                                    "SCH",
                                                    0
                                                    );
                        }
                        SchemeNavVal.Clear();
                        SchemeAmountVal.Clear();

                        DataView dv = AllMethods.getIndicesAgainstScheme(m.Scheme_Id.ToString()).DefaultView;
                        //m.Scheme_Id
                        //DataTable dtScheme = dv.ToTable(true, "SCHEME_ID", "Sch_Short_Name");
                        //DataTable dtIndex = dv.ToTable(true, "INDEX_ID", "INDEX_NAME");
                        var _indexSplit = dv.ToTable(true, "INDEX_ID", "INDEX_NAME").AsEnumerable().
                            Where(c => indexSplit.Contains(Convert.ToString(c["INDEX_ID"])))
                            .Select(d => Convert.ToString(d["INDEX_ID"])).ToList();

                        #region Include Individual INDEX                      
                        //if (IndividualIndex.Any())
                        //    _indexSplit.AddRange(IndividualIndex);

                        //IndividualIndex = new List<string>();
                        #endregion
                        //foreach (var s in indexSplit)
                        foreach (var s in _indexSplit)
                        {
                            dcmlIndexid = 0;
                            var SchMasterRow = db.T_Client_Additional_Benchmarks.Where(n => n.Fund_ID == m.Fund_id).FirstOrDefault();
                            if (SchMasterRow != null && SchMasterRow.Fund_ID.Value > 0)
                            {
                                var AllIndAdd = perfDataTable.AsEnumerable().Where(x => Convert.ToString(x["ID"]) == Convert.ToString(SchMasterRow.Add_Bench_ID) && Convert.ToString(x["TYPE"]) == "IND");
                                if (!AllIndAdd.Any())
                                    dcmlIndexid = SchMasterRow.Add_Bench_ID.Value;
                            }

                            var AllInd = perfDataTable.AsEnumerable().Where(x => Convert.ToString(x["ID"]) == s && Convert.ToString(x["TYPE"]) == "IND");
                            if (AllInd.Any() && dcmlIndexid == 0) continue;

                            DataTable dtIndexAbsolute = new DataTable();
                            //Nature
                            using (var db1 = new SIP_ClientDataContext())
                            {
                                var NatureData = (from sm in db1.T_SCHEMES_MASTER_Clients
                                                  join fm in db1.T_FUND_MASTER_clients on sm.Fund_Id equals fm.FUND_ID
                                                  join tsn in db1.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals tsn.Sebi_Nature_ID
                                                  where sm.Scheme_Id == Convert.ToDecimal(m.Scheme_Id)
                                                  select new { tsn.Sebi_Nature, tsn.Sebi_Nature_ID, sm.Amfi_Code, sm.Scheme_Name, sm.Launch_Date }).FirstOrDefault();

                                strNature = NatureData.Sebi_Nature;
                            }
                            if (strNature == "Debt")
                                SettingId = 34;//simple
                            else
                                SettingId = 33;

                            if (m.Launch_Date != null)
                            {
                                TimeSpan dateDiff4Index = CurrentDatyOfvMth.Subtract(m.Launch_Date.Value);
                                dayDiffIndex = dateDiff4Index.Days;
                                strRollingPeriodin = "1 YYYY,3 YYYY,5 YYYY," + dayDiffIndex + " d";
                                SqlCommand cmd;
                                SqlDataAdapter da = new SqlDataAdapter();
                                List<double> IndexNavVal = new List<double>();
                                List<double> IndexAmountVal = new List<double>();

                                List<double> AddBenchNavVal = new List<double>();
                                List<double> AddBenchAmountVal = new List<double>();

                                if (!string.IsNullOrEmpty(s))
                                {
                                    cmd = new SqlCommand("MFIE_SP_INDEX_P2P_ROLLING_RETURN", conn);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandTimeout = 2000;
                                    if (!AllInd.Any())
                                        cmd.Parameters.Add(new SqlParameter("@IndexIDs", s + "," + dcmlIndexid));
                                    else
                                        cmd.Parameters.Add(new SqlParameter("@IndexIDs", dcmlIndexid));

                                    //if (!AllInd.Any())
                                    //    cmd.Parameters.Add(new SqlParameter("@IndexIDs", dcmlIndexid + "," + s));
                                    //else
                                    //    cmd.Parameters.Add(new SqlParameter("@IndexIDs", dcmlIndexid));

                                    cmd.Parameters.Add(new SqlParameter("@SettingSetID", SettingId));
                                    cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                                    cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                                    cmd.Parameters.Add(new SqlParameter("@RoundTill", 4));
                                    cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriodin", strRollingPeriodin));
                                    cmd.Parameters.Add(new SqlParameter("@IndxRollingPeriod", val));
                                    cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequencyin", ""));
                                    cmd.Parameters.Add(new SqlParameter("@IndxRollingFrequency", val));
                                    cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                                    cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));

                                    da.SelectCommand = cmd;
                                    da.Fill(dtIndexAbsolute);

                                    if (dtIndexAbsolute != null && dtIndexAbsolute.Rows.Count > 0)
                                    {
                                        if (dtIndexAbsolute.Columns.Contains("INDEX_TYPE"))
                                            dtIndexAbsolute.Columns.Remove("INDEX_TYPE");
                                    }
                                    //Calculation

                                    for (int i = 2; i < dtIndexAbsolute.Columns.Count; i++)
                                    {
                                        int day3;
                                        int indxId = Convert.ToInt32(dtIndexAbsolute.Rows[0][0]);
                                        if (dtIndexAbsolute.Rows[0][i].ToString() != "N/A" && dtIndexAbsolute.Rows[0][i].ToString() != "")
                                        {
                                            if (i < 6)
                                            {
                                                var daysDiffVal = Convert.ToDouble(dtIndexAbsolute.Rows[0][i]);
                                                IndexNavVal.Add(Convert.ToDouble(daysDiffVal));
                                                if (i == 2)
                                                {
                                                    day3 = 365 * 1;
                                                    IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                                }
                                                else if (i == 3)
                                                {
                                                    day3 = 365 * 3;
                                                    IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                                }
                                                else if (i == 4)
                                                {
                                                    day3 = 365 * 5;
                                                    IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                                }
                                                else if (i == 5)
                                                {
                                                    DateTime dateDiffIndex = MaxNavDt.Nav_Date.Value;

                                                    //DateTime dateDiffIndex = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                                                    TimeSpan dateDiffSi = dateDiffIndex.Subtract(m.Launch_Date.Value);
                                                    day3 = dateDiffSi.Days;
                                                    IndexAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day3 / 365, 4)));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            IndexNavVal.Add(0);
                                            IndexAmountVal.Add(0);
                                        }
                                        int day4;

                                        if (dtIndexAbsolute.Rows.Count > 1)
                                        {
                                            indxId = Convert.ToInt32(dtIndexAbsolute.Rows[1][0]);
                                            if (dtIndexAbsolute.Rows[1][i].ToString() != "N/A")
                                            {
                                                if (i < 6)
                                                {
                                                    var daysDiffVal = Convert.ToDouble(dtIndexAbsolute.Rows[1][i]);
                                                    AddBenchNavVal.Add(Convert.ToDouble(daysDiffVal));
                                                    if (i == 2)
                                                    {
                                                        day4 = 365 * 1;
                                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));
                                                    }
                                                    else if (i == 3)
                                                    {
                                                        day4 = 365 * 3;
                                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));
                                                    }
                                                    else if (i == 4)
                                                    {
                                                        day4 = 365 * 5;
                                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));
                                                    }
                                                    else if (i == 5)
                                                    {
                                                        DateTime _ToDate = MaxNavDt.Nav_Date.Value;

                                                        //DateTime _ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                                                        TimeSpan dateDiffSi = _ToDate.Subtract(m.Launch_Date.Value);
                                                        day4 = dateDiffSi.Days;
                                                        AddBenchAmountVal.Add(amount * Math.Pow(1 + (double)daysDiffVal / 100, Math.Round((double)day4 / 365, 4)));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                AddBenchNavVal.Add(0);
                                                AddBenchAmountVal.Add(0);
                                            }
                                        }
                                    }

                                    

                                    if (dtIndexAbsolute.Rows.Count > 1)
                                    {
                                        var _row1 = perfDataTable.NewRow();
                                        _row1[0] = dtIndexAbsolute.Rows[1]["Index_Name"].ToString();
                                        _row1[1] = AddBenchNavVal[0] == 0 ? "--" : AddBenchNavVal[0].ToString();
                                        _row1[2] = AddBenchAmountVal[0] == 0 ? "--" : AddBenchAmountVal[0].ToString();
                                        _row1[3] = AddBenchNavVal[1] == 0 ? "--" : AddBenchNavVal[1].ToString();
                                        _row1[4] = AddBenchAmountVal[1] == 0 ? "--" : AddBenchAmountVal[1].ToString();
                                        _row1[5] = AddBenchNavVal[2] == 0 ? "--" : AddBenchNavVal[2].ToString();
                                        _row1[6] = AddBenchAmountVal[2] == 0 ? "--" : AddBenchAmountVal[2].ToString();
                                        _row1[7] = AddBenchNavVal[3] == 0 ? "--" : AddBenchNavVal[3].ToString();
                                        _row1[8] = AddBenchAmountVal[3] == 0 ? "--" : AddBenchAmountVal[3].ToString();
                                        _row1["ID"] = dtIndexAbsolute.Rows[1]["INDEX_ID"];
                                        _row1["TYPE"] = "IND";
                                        _row1["REF_SCH"] = m.Scheme_Id;
                                        perfDataTable.Rows.Add(_row1);
                                    }
                                    var _row = perfDataTable.NewRow();
                                    _row[0] = dtIndexAbsolute.Rows[0]["Index_Name"].ToString();
                                    _row[1] = IndexNavVal[0] == 0 ? "--" : IndexNavVal[0].ToString();
                                    _row[2] = IndexAmountVal[0] == 0 ? "--" : IndexAmountVal[0].ToString();
                                    _row[3] = IndexNavVal[1] == 0 ? "--" : IndexNavVal[1].ToString();
                                    _row[4] = IndexAmountVal[1] == 0 ? "--" : IndexAmountVal[1].ToString();
                                    _row[5] = IndexNavVal[2] == 0 ? "--" : IndexNavVal[2].ToString();
                                    _row[6] = IndexAmountVal[2] == 0 ? "--" : IndexAmountVal[2].ToString();
                                    _row[7] = IndexNavVal[3] == 0 ? "--" : IndexNavVal[3].ToString();
                                    _row[8] = IndexAmountVal[3] == 0 ? "--" : IndexAmountVal[3].ToString();
                                    _row["ID"] = dtIndexAbsolute.Rows[0]["INDEX_ID"];
                                    _row["TYPE"] = "IND";
                                    _row["REF_SCH"] = m.Scheme_Id;
                                    perfDataTable.Rows.Add(_row);
                                }
                                IndexNavVal.Clear();
                                IndexAmountVal.Clear();
                                AddBenchNavVal.Clear();
                                AddBenchAmountVal.Clear();
                            }
                        }
                        k++;
                    }

                    #region Suffeling of scheme index
                    //DataTable dt = null;
                    //dt = RestoreDatafromGrid();

                    //var FinalData = perfDataTable.Clone();
                    //foreach (DataRow tblData in dt.Rows)
                    //{
                    //    if (!string.IsNullOrEmpty(Convert.ToString(tblData["SCHEME_ID"])))
                    //    {
                    //        var SchTbl = perfDataTable.AsEnumerable()
                    //           .Where(x => Convert.ToString(x["ID"]) == Convert.ToString(tblData["SCHEME_ID"]));
                    //        if (SchTbl.Any())
                    //            FinalData.ImportRow(SchTbl.FirstOrDefault());

                    //        var AddInd = perfDataTable.AsEnumerable()
                    //            .Where(x => Convert.ToString(x["REF_SCH"]) == Convert.ToString(tblData["SCHEME_ID"]));
                    //        foreach (var _dt in AddInd)
                    //        {
                    //            FinalData.ImportRow(_dt);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var chk = FinalData.AsEnumerable()
                    //           .Where(x => Convert.ToString(x["ID"]) == Convert.ToString(tblData["INDEX_ID"])
                    //           && Convert.ToString(x["TYPE"]) == "IND");
                    //        if (!chk.Any())
                    //        {
                    //            var IndTbl = perfDataTable.AsEnumerable()
                    //               .Where(x => Convert.ToString(x["ID"]) == Convert.ToString(tblData["INDEX_ID"]));
                    //            if (IndTbl.Any())
                    //                FinalData.ImportRow(IndTbl.FirstOrDefault());
                    //        }
                    //    }
                    //}
                    #endregion
                    perfDataTable.Columns.Remove("TYPE");
                    perfDataTable.Columns.Remove("ID");

                    return perfDataTable;
                }
                #endregion
            }
            catch (Exception EX)
            {

            }
            return null;
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
            string strRollingPeriodin = "30 D,91 D,182 D,1 YYYY,3 YYYY,0 Si";
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
            resultStr = SchemeName;

            return resultStr;
        }

        #region DropDownListEvent     

        protected void rdbOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if (ddlFundHouse.SelectedIndex != 0)
            getSchemesList();
        }


        #endregion

        #region Nav data 

        #endregion
        [WebMethod]
        public static ChartNavReturnModel getChartData(string schIndexid, DateTime startDate, DateTime endDate)// string startdate, string enddate, List<string> schList,List<string> indList
        {
            ChartNavReturnModel objChartNavReturnModel = null;
            List<string> schList = new List<string>();
            List<string> indList = new List<string>();
            List<decimal> sortList = new List<decimal>();

            if (!string.IsNullOrEmpty(schIndexid))
            {
                var strArray = schIndexid.Split('#').ToList();

                foreach (string str in strArray)
                {
                    if (str.Trim().ToUpper().StartsWith("S"))
                    {
                        schList.Add(str.Trim().ToUpper().TrimStart('S'));
                        sortList.Add(Convert.ToDecimal(str.Trim().ToUpper().TrimStart('S')));
                    }

                    if (str.Trim().ToUpper().StartsWith("I"))
                    {
                        indList.Add(str.Trim().ToUpper().TrimStart('I'));
                        sortList.Add(Convert.ToDecimal(str.Trim().ToUpper().TrimStart('I')));
                    }

                }
            }



            DataTable _dtFinal = new DataTable();
            _dtFinal.Columns.Add("Scheme_Id", typeof(decimal));
            _dtFinal.Columns.Add("Sch_Short_Name", typeof(string));
            _dtFinal.Columns.Add("Nav_Date", typeof(DateTime));
            _dtFinal.Columns.Add("Nav", typeof(double));


            try
            {

                #region GetSchmeIndex

                SchemeIndexList objSchInd = new SchemeIndexList();
                objSchInd.ListScheme = schList.Select(x => Convert.ToDecimal(x)).ToList();
                objSchInd.ListIndex = indList.Select(x => Convert.ToDecimal(x)).ToList();

                DataTable _dtSch = new DataTable();
                DataTable _dtInd = new DataTable();

                DateTime mindate = AllMethods.getSchmindate(AllMethods.getFundSchemeIdStr(objSchInd.ListScheme));


                if (objSchInd.ListScheme.Count() > 0)
                {
                    _dtSch = AllMethods.getHistNavDetails(AllMethods.getFundSchemeIdStr(objSchInd.ListScheme), mindate, System.DateTime.Now);
                    if (_dtSch != null && _dtSch.Rows.Count > 0)
                    {
                        _dtFinal = _dtSch.Copy();
                    }
                }

                if (objSchInd.ListIndex.Count() > 0)
                {
                    _dtInd = AllMethods.getHistIndexRecordDetails(AllMethods.getFundSchemeIdStr(objSchInd.ListIndex), mindate, System.DateTime.Now);
                    if (_dtInd != null && _dtInd.Rows.Count > 0)
                    {

                        if (_dtFinal != null && _dtFinal.Rows.Count == 0)
                        {
                            // var dtble = _dtInd.AsEnumerable().Select(x => x).ToDataTable();
                            foreach (DataRow dr in _dtInd.Rows)
                                _dtFinal.Rows.Add(dr.ItemArray);
                        }
                        else
                            _dtInd.AsEnumerable().CopyToDataTable(_dtFinal, LoadOption.OverwriteChanges);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }




            IEnumerable<DataRow> _testdtIndex = from sl in sortList.AsEnumerable()
                                                join tst in _dtFinal.AsEnumerable()
                                                on sl.ToString() equals tst.Field<decimal>("Scheme_Id").ToString()
                                                select tst;
            DataTable _dttt = new DataTable("tblSort");
            _dttt = _testdtIndex.CopyToDataTable<DataRow>();
            _dtFinal = _dttt.Copy();

            _dtFinal = RebaseDT(_dtFinal);


            var data = _dtFinal.AsEnumerable().Select(x => x);

            var names = data.Select(x => x.Field<string>("Sch_Short_Name")).Distinct();

            //#region ReBase Algo
            //var data2 = _dtFinal.AsEnumerable().Cast<NavReturnModelSP>().ToList();//.OrderBy(x => x.Scheme_Name).ThenBy(x => x.Date).ToList();



            //#endregion
            //ObjWebIner.EventDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")

            var returnData = names.Select(name => new SimpleNavReturnModel
            {
                Name = name,
                ValueAndDate =
                    data.Where(x => x.Field<string>("Sch_Short_Name") == name)
                        //.Select(x => new ValueAndDate { Date = x.Field<DateTime>("Nav_Date"), Value = x.Field<double>("ReInvestNav") })  //Nav
                        .Select(x => new ValueAndDate
                        {
                            Date = x.Field<DateTime>("Nav_Date").ToString("yyyy-MM-dd"),
                            Value = x.Field<double>("ReInvestNav"),

                            OrginalValue = Math.Round(x.Field<double>("Nav"), 2)
                        })  //Nav

                        .ToList()
            }).ToList();



            objChartNavReturnModel = new ChartNavReturnModel
            {
                MaxDate = data.Select(x => x.Field<DateTime>("Nav_Date")).Max().ToString("MM/dd/yyyy"),
                MinDate = data.Select(x => x.Field<DateTime>("Nav_Date")).Min().ToString("MM/dd/yyyy"),
                MaxVal = data.Select(x => x.Field<double>("ReInvestNav")).Max(),
                MinVal = data.Select(x => x.Field<double>("ReInvestNav")).Min(),
                SimpleNavReturnModel = returnData
            };
            //return _dtFinal;
            //return "sucess";
            return objChartNavReturnModel;
        }

        //This  function will rebse to 100
        public static DataTable RebaseDT(DataTable dt)
        {

            DataTable _dtrebase = dt.Copy();
            var preScheme = "";
            var firstNav = 0d;
            DataColumn dCol = new DataColumn("ReInvestNav", typeof(double));
            _dtrebase.Columns.Add(dCol);

            DataTable data = _dtrebase.Copy();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                data.Rows[i][4] = 0;

                if (data.Rows[i][1].ToString() != preScheme)
                {
                    firstNav = Convert.ToDouble(data.Rows[i][3]);
                    data.Rows[i][4] = 100;
                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                else
                {
                    if (Convert.ToDouble(data.Rows[i - 1][3]) != 0)
                        data.Rows[i][4] = (100 * Convert.ToDouble(data.Rows[i][3])) / firstNav;
                    if (data.Rows[i][1].ToString().ToUpper().Contains("CRISIL"))
                        data.Rows[i][3] = "-1";
                }
                preScheme = data.Rows[i][1].ToString();
            }

            return data;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField lblRollNo = (HiddenField)e.Item.FindControl("txtSCHEME_ID");
                HiddenField lblIndexs = (HiddenField)e.Item.FindControl("txtINDEX_IDS");

                Repeater innerRepeater = (Repeater)e.Item.FindControl("innerRepeater");
                DataTable dt = GetPerformanceDT(Convert.ToInt32(lblRollNo.Value), lblIndexs.Value);
                innerRepeater.DataSource = dt;
                innerRepeater.DataBind();
            }
        }

        public DataTable GetPerformanceDT(int SchId, string strIndex)
        {
            //DataTable DT = (DataTable)Session["SEBI_DATA"];
            DataTable _dtFinal = new DataTable();

            try
            {
                var strSch = SchId.ToString();

                DataTable _dtSch = new DataTable();

                _dtFinal.Columns.Add("Sch_id");
                _dtFinal.Columns.Add("Sch_Short_Name");
                _dtFinal.Columns.Add("Per_30_Days");
                _dtFinal.Columns.Add("Per_91_Days");
                _dtFinal.Columns.Add("Per_182_Days");
                _dtFinal.Columns.Add("Per_1_Year");
                _dtFinal.Columns.Add("1YAmount");
                _dtFinal.Columns.Add("Per_3_Year");
                _dtFinal.Columns.Add("3YAmount");
                _dtFinal.Columns.Add("Per_5_Year");
                _dtFinal.Columns.Add("5YAmount");
                _dtFinal.Columns.Add("Per_Since_Inception");
                _dtFinal.Columns.Add("SIAmount");
                _dtFinal.Columns.Add("Nav_Rs");
                _dtFinal.Columns.Add("Nature");
                _dtFinal.Columns.Add("Sub_Nature");
                _dtFinal.Columns.Add("Option_Id");
                _dtFinal.Columns.Add("Structure_Name");
                _dtFinal.Columns.Add("status");


                if (strSch != string.Empty)
                    _dtSch = AllMethods.getFundComparisonWithSI_LNT(strSch);

                if (_dtFinal != null)
                {
                    for (int i = 0; i < _dtSch.Rows.Count; i++)
                    {
                        _dtFinal.Rows.Add(_dtSch.Rows[i].ItemArray);
                    }
                }
                var _dtIndNew = CalculateIndexHistPerfNew(DateTime.Today.AddDays(-1), strIndex, strSch, _dtFinal);
                return _dtIndNew;
            }
            catch (Exception ex)
            {

            }
            return _dtFinal;
        }
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
    public class SimpleNavReturnModel
    {
        public string Name { get; set; }
        public decimal Id { get; set; }
        public bool IsIndex { get; set; }
        public IList<ValueAndDate> ValueAndDate { get; set; }
        public string MaxDate { get; set; }
        public string MinDate { get; set; }
    }
    public class ValueAndDate
    {
        public string Date { get; set; }
        public double Value { get; set; }
        public double OrginalValue { get; set; }
    }
    public class ChartNavReturnModel
    {
        public IList<SimpleNavReturnModel> SimpleNavReturnModel { get; set; }
        public string MaxDate { get; set; }
        public string MinDate { get; set; }
        public double MaxVal { get; set; }
        public double MinVal { get; set; }
    }
}