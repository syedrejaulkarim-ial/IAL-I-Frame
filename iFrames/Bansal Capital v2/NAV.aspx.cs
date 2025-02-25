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
using System.IO;


namespace iFrames.BansalCapital
{
    public partial class NAV : System.Web.UI.Page
    {

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
                DivGridContain.Visible = false;
                DivPlotChart.Visible = false;
              //  DivLast.Visible = false;
            }
            
            lblErrMsg.Text = "";
        }

        #endregion

        protected void LoadNature()
        {
            DataTable _dt = AllMethods.getSebiNature();
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
            DataTable _dt = AllMethods.getAllSebiSubNature();
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
            ddlSubCategory.ClearSelection();
            if (id == -1)
            {
                ddlSubCategory.Items[0].Selected = true;
                return;
            }
            DataTable _dt = AllMethods.getSebiSubNature(id);
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

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            LoadSubNature(nature_id);
            //if (ddlCategory.SelectedIndex != 0)
                getSchemesList();
        }

        #region DropDownListEvent


        protected void ddlMutualFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlFundHouse.SelectedIndex != 0)
                getSchemesList();
            //else
            //{
            //    ddlSchemes.Items.Clear();
            //}
        }

        #endregion

        #region ButtonEvent

        protected void btnAddScheme_Click(object sender, EventArgs e)
        {
            DivGridContain.Visible = true;
            DivPlotChart.Visible = true;
         //   DivLast.Visible = true;
            AddScheme();
        }

        protected void btnAddIndices_Click(object sender, EventArgs e)
        {
            DivGridContain.Visible = true;
            DivPlotChart.Visible = true;
          //  DivLast.Visible = true;
            AddIndices();
        }

        protected void btnPlotChart_Click(object sender, EventArgs e)
        {
            
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

        private void getSchemesList()
        {
            ddlSchemes.ClearSelection();
            DataTable dtScheme = AllMethods.getSebiScheme(Convert.ToInt32(ddlCategory.SelectedItem.Value),
                Convert.ToInt32(ddlType.SelectedItem.Value), Convert.ToInt32(ddlSubCategory.SelectedValue), 
                Convert.ToInt32(rdbOption.SelectedValue), Convert.ToInt32(ddlFundHouse.SelectedValue),false);

            ddlSchemes.DataSource = dtScheme;
            ddlSchemes.DataTextField = "Sch_Short_Name";
            ddlSchemes.DataValueField = "Scheme_Id";
            ddlSchemes.DataBind();
        }

        private void getSchemesListFilter(List<decimal> lstSchmId)
        {

            try
            {
                DataTable dtScheme = AllMethods.getSebiScheme(Convert.ToInt32(ddlCategory.SelectedItem.Value), Convert.ToInt32(ddlType.SelectedItem.Value), Convert.ToInt32(ddlSubCategory.SelectedValue), Convert.ToInt32(rdbOption.SelectedValue), Convert.ToInt32(ddlFundHouse.SelectedValue),false);
                var dt = dtScheme.AsEnumerable().Where(x => !lstSchmId.Contains(x.Field<decimal>("Scheme_Id")));
                if (dt.Any())
                {
                    DataTable vdtScheme = dt.CopyToDataTable();
                    ddlSchemes.DataSource = vdtScheme;
                    ddlSchemes.DataTextField = "Sch_Short_Name";
                    ddlSchemes.DataValueField = "Scheme_Id";
                    ddlSchemes.DataBind();
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
                if (ddlSchemes.SelectedIndex != -1)
                {
                    //DataTable dt = objNavGraph.FetchIndicesAgainstScheme(ddlSchemes.SelectedItem.Value.Trim());
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
                                            dr1["ImgID"] = Convert.ToString("img/key" + AutoID + ".gif");
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
                                                dr2["ImgID"] = Convert.ToString("img/key" + AutoID + ".gif");
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
                                            dr1["ImgID"] = Convert.ToString("img/key" + i + ".gif");
                                            dtToBind.Rows.Add(dr1);
                                            i = i + 1;
                                        }

                                        if (clmn.ColumnName == "INDEX_ID" && !string.IsNullOrEmpty(dr["INDEX_ID"].ToString()))
                                        {
                                            DataRow dr2 = dtToBind.NewRow();
                                            dr2["AutoID"] = i.ToString();
                                            dr2["INDEX_ID"] = Convert.ToString(dr[clmn.ColumnName]);
                                            dr2["INDEX_NAME"] = Convert.ToString(dr["INDEX_NAME"]);
                                            dr2["ImgID"] = Convert.ToString("img/key" + i + ".gif");
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
                            dr["ImgID"] = Convert.ToString(@"..\img\key0.gif");
                            dtToBind.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = dtToBind.NewRow();
                            dr["AutoID"] = "0";
                            dr["SCHEME_ID"] = ddlSchemes.SelectedItem.Value;
                            dr["Sch_Short_Name"] = ddlSchemes.SelectedItem.Text;
                            dr["ImgID"] = Convert.ToString(@"..\img\key0.gif");
                            dtToBind.Rows.Add(dr);
                        }
                    }

                    if (dtToBind.Rows.Count > 0)
                    {
                        dglist.Visible = true;

                        dglist.DataSource = dtToBind;
                        dglist.DataBind();

                        if (dtToBind != null)
                            divTimePeriod.Visible = true;

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
                getSchemesListFilter(lstSchId);
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
                    if (dt != null && dt.Rows.Count > 0)
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
                                dr["ImgID"] = Convert.ToString("img/key" + dt.Rows.Count + ".gif");
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
                        dr["ImgID"] = Convert.ToString("img/key0.gif");
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
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            sethidSchIndDataTable();
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
                dt.Columns.Add("ImgID", typeof(System.String));


                if (dglist.Items.Count > 0)
                {
                    for (int i = 0; i < dglist.Items.Count; i++)
                    {
                        string val_AutoID = ((Label)dglist.Items[i].Cells[2].FindControl("lblAutoID")).Text;
                        string val_SchemeCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeId")).Text;
                        string val_SchemeName = ((Label)dglist.Items[i].Cells[0].FindControl("lblSchemeName")).Text;
                        string val_IndCode = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndId")).Text;
                        string val_IndName = ((Label)dglist.Items[i].Cells[0].FindControl("lblIndName")).Text;
                        string val_ImgPath = ((Image)dglist.Items[i].Cells[0].FindControl("imgKey")).ImageUrl;


                        DataRow dr = dt.NewRow();
                        dr["AutoID"] = val_AutoID;
                        dr["SCHEME_ID"] = val_SchemeCode;
                        dr["Sch_Short_Name"] = val_SchemeName;
                        dr["INDEX_ID"] = val_IndCode;
                        dr["INDEX_NAME"] = val_IndName;
                        dr["ImgID"] = val_ImgPath;
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

        // function to populate chart from selected Schme iNdex
        //private void PopulateSchemeIndexChart()
        //{
        //    DateTime endDate = DateTime.Today;
        //    DateTime startDate = DateTime.Today;



        //    try
        //    {
        //        #region DateRange


        //        if (rbTime.Checked == true)
        //        {
        //            endDate = DateTime.Now;//.ToString("MM-dd-yyyy")
        //            startDate = DateTime.Today.AddDays(Convert.ToDouble("-" + ddlTime.SelectedItem.Value));//.ToString("MM-dd-yyyy")
        //        }
        //        else
        //        {
        //            if (txtfromDate.Text != "" && txtToDate.Text != "")
        //            {
        //                endDate = Convert.ToDateTime(txtToDate.Text);
        //                startDate = Convert.ToDateTime(txtfromDate.Text);//.ToString("MM-dd-yyyy")

        //            }
        //        }
        //        #endregion


        //        #region GetSchmeIndex

        //        SchemeIndexList objSchInd = GetSchIndDataTable();
        //        DataTable _dtSch = new DataTable();
        //        DataTable _dtInd = new DataTable();
        //        DataTable _dtFinal = new DataTable();

        //        if (objSchInd.ListScheme.Count() > 0)
        //        {
        //            _dtSch = AllMethods.getHistNavDetails(AllMethods.getFundSchemeIdStr(objSchInd.ListScheme), startDate, endDate);
        //            if (_dtSch.Rows.Count > 0)
        //            {
        //                _dtFinal = _dtSch.Copy();
        //            }
        //        }

        //        if (objSchInd.ListIndex.Count() > 0)
        //        {
        //            _dtInd = AllMethods.getHistIndexRecordDetails(AllMethods.getFundSchemeIdStr(objSchInd.ListIndex), startDate, endDate);
        //            if (_dtInd.Rows.Count > 0)
        //            {
        //                //_dtFinal.Rows.Add(_dtInd.Rows.
        //                _dtInd.AsEnumerable().CopyToDataTable(_dtFinal, LoadOption.OverwriteChanges);
        //            }
        //        }


        //        BindDataTableToChart(_dtFinal, "", chrtNavGraph);



        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


        #endregion

        #region asp Chart

        private void BindDataTableToChart(DataTable dtChart, string _xBaseField, System.Web.UI.DataVisualization.Charting.Chart chrt)
        {


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

            hidSchindSelected.Value = sb.ToString();
        }

        #endregion

        #region Excel Work
        private void ShowExcel(string FundName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FundName + "_Detail_Report.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";


            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            System.Text.StringBuilder objFinalstr = new System.Text.StringBuilder();

            objFinalstr.Append("<br/>");
            objFinalstr.Append("<table width='520px' style='text-align:justify'><tr><td width='100%'>");
            objFinalstr.Append(sw.ToString());
            objFinalstr.Append("<br/><b>Mutual Fund investments are subject to market risks, read all scheme related documents carefully.</b>");
            objFinalstr.Replace("<table", "<p height='90px'/><table");
            objFinalstr.Replace("width:220px", "width:350px");
            objFinalstr.Append("</td></tr></table>");
            objFinalstr.Append("<br/>");

            string ImagePath = string.Empty;
            ImagePath = HttpContext.Current.Server.MapPath("~") + FundName + "\\img\\";
            objFinalstr.Replace("<img src='img/rimg.png' style='vertical-align:middle;'>", "");
            objFinalstr.Replace(@"class=""grdHead""", "style ='text-align: center;	font-family: Arial;	color: #ffffff;	font-size: 11px;background:#02509b;	font-weight:normal;'");
            Response.Write(objFinalstr.ToString());
            Response.Flush();
            Response.End();
        }


        #endregion

        #region WebMethod

        // Call this method from Clent Side to get Chart Data
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

                            OrginalValue = Math.Round(x.Field<double>("Nav"),2)
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

        #endregion   

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlSubCategory.SelectedIndex != 0)
                getSchemesList();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlType.SelectedIndex != 0)
                getSchemesList();
        }

        protected void rdbOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlType.SelectedIndex != 0)
            getSchemesList();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            getFundHouse();
            getIndicesName();
            LoadNature();
            LoadAllSubNature();
            LoadStructure();
            loadOption();
            getSchemesList();
            DivGridContain.Visible = false;
            DivPlotChart.Visible = false;
        }
    }


}