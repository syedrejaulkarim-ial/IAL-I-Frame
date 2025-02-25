using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using inout = System.IO;
using Text = System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Globalization;
using iFrames.DAL;

namespace iFrames.CanaraClient
{
    public partial class CanaraUploadExcel : System.Web.UI.Page
    {
        public List<string> errSheets = new List<string>();

        #region: Global Variable

        string filepath = string.Empty;
        string filename = string.Empty;
        string fileext = string.Empty;
        //string connstr = ConfigurationManager.ConnectionStrings["TestcsIcraclient"].ConnectionString;
        string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        SqlConnection conn = new SqlConnection();
        #endregion

        #region: Constructor

        public CanaraUploadExcel()
        {
            conn.ConnectionString = connstr;
        }

        #endregion
        #region: Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;

            if (Session["UserId"] != null)
            {
                if (ddlIndex.Items.Count == 0)
                {
                    FillDropdown(ddlIndex);
                }

                pnlUpload.Visible = true;
                pnlInstruction.Visible = true;
            }
            else
            {
                Response.Redirect("Login.aspx");
                //pnlLogin.Visible = true;
                //pnlUpload.Visible = false;
                //lnlLogout.Visible = false;
                //pnlInstruction.Visible = false;
            }
        }
        #endregion

        #region: Button Event

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");


            UploadExcell();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");

            if (Save())
            {
                lblMsg.Text = "Data Saved Successfully.";

            }
            else
            {
                lblMsg.Text = "Data Already Exists.";
            }
            Reset();

        }

        #endregion


        #region: Method

        #region: Fetch Method

        public DataTable FetchCustomIndex()
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            try
            {
                sql = "SELECT CUSTOM_INDEX_ID,CUSTOM_INDEX_NAME FROM T_MFIE_CUSTOM_INDEX_MASTER order by CUSTOM_INDEX_NAME";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region: Dropdown Method

        public void FillDropdown(Control ddl)
        {
            try
            {
                DataTable dt = FetchCustomIndex();
                if (dt.Rows.Count > 0)
                {
                    DropDownList drpdwn = (DropDownList)ddl;
                    drpdwn.DataSource = dt;
                    drpdwn.DataTextField = "CUSTOM_INDEX_NAME";
                    drpdwn.DataValueField = "CUSTOM_INDEX_ID";
                    drpdwn.DataBind();
                    drpdwn.Items.Insert(0, new ListItem("-Select Index-", "0"));
                    drpdwn.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {


            }
        }

        #endregion

        #region: Upload Method

        private void UploadExcell()
        {
            //try
            //{
            if (FUExcell.HasFile == true)
            {
                string todate = System.DateTime.Now.ToString("MMddyyyyhhmmss");
                filepath = Server.MapPath("~//CanaraClient//UploadFile//") + FUExcell.FileName;
                fileext = inout.Path.GetExtension(filepath);
                filename = inout.Path.GetFileNameWithoutExtension(filepath);
                filepath = Server.MapPath("~//CanaraClient//UploadFile//") + filename + "_" + todate + fileext;

                if (inout.Directory.Exists(Server.MapPath("~//CanaraClient//UploadFile")))
                {
                    string[] filePaths = inout.Directory.GetFiles(Server.MapPath("~//CanaraClient//UploadFile"));
                    foreach (string filePath in filePaths)
                        inout.File.Delete(filePath);
                }
                FUExcell.SaveAs(filepath);

                //string CustomIndexId = GetCustomIndexId(inout.Path.GetFileNameWithoutExtension(FUExcell.FileName));

                //DataTable dt1 = FillGridFromExcell(filepath);
                DataTable dt1 = GetRequestsDataFromExcel(filepath);

                DataView dv = dt1.DefaultView;
                dv.RowFilter = "CONVERT(Isnull(CUSTOM_INDEX_ID,''), System.String) <> ''";
                dt1 = dv.ToTable();

                DataTable dtBulk = new DataTable();
                dtBulk = dt1.Copy();
                dtBulk.Columns.Add("LoginId", typeof(System.String));

                foreach (DataRow dr in dtBulk.Rows)
                {
                    if (Session["userid"] != null)
                    {
                        if (Convert.ToString(Session["userid"]) == "tataindex")
                            dr["LoginId"] = "0";
                    }
                    else
                    {
                        dr["LoginId"] = "-1";
                    }
                }

                //string msg = "";
                //if (errSheets.Count > 0)
                //{
                //    msg = " No index found for this sheet : ";
                //    foreach (var r in errSheets)
                //        msg += r + ", ";
                //}

                if (dtBulk.Rows.Count > 0)
                {
                    if (BulkInsertToTemp(dtBulk))
                    {
                        if (InsertAfterCheck())
                            lblMsg.Text = "File has been uploaded succesfully. ";
                        //lblMsg.Text = "File has been uploaded succesfully. " + msg;
                        else
                            lblMsg.Text = "Data has been updated successfully. ";
                        //lblMsg.Text = "Data has been updated successfully. " + msg;
                    }
                    #region USP_GROSS_VALUE_CALC_TATA
                    //SqlCommand cmd = new SqlCommand("USP_GROSS_VALUE_CALC_TATA", conn);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //if (conn.State == ConnectionState.Closed)
                    //{
                    //    conn.Open();
                    //}
                    //int  i = cmd.ExecuteNonQuery();

                    #endregion
                }
                else
                {
                    lblMsg.Text = "No data found to upload. ";
                }
            }
            else
            {
                lblMsg.Text = "Please choose a correct file.";
            }
            //}
            //catch (Exception ex)
            //{


            //}

        }

        #endregion

        #region: Save/Insert Method

        public bool Save()
        {
            bool success = false;
            string sql = string.Empty;

            string CUSTOM_INDEX_ID = string.Empty;
            string RECORD_DATE = string.Empty;
            string CUSTOM_INDEX_VALUE = string.Empty;
            string LOGIN_ID = string.Empty;

            SqlParameter custom_index_idParam = new SqlParameter();
            SqlParameter record_dateParam = new SqlParameter();
            SqlParameter custom_index_valueParam = new SqlParameter();
            SqlParameter login_idParam = new SqlParameter();

            try
            {
                DataTable dt = CheckDbValue(ddlIndex.SelectedItem.Value.ToString(), txttDate.Text.Trim(), txtIndexValue.Text.Trim());
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (dt.Rows.Count > 0)
                {
                    string record_date = Convert.ToDateTime(txttDate.Text).ToString();
                    sql = "UPDATE [T_MFIE_CUSTOM_INDEX_RECORDS] SET [CUSTOM_INDEX_VALUE]=@CUSTOM_INDEX_VALUE,LOGIN_ID=@LOGIN_ID WHERE [RECORD_DATE]=@RECORD_DATE and CUSTOM_INDEX_ID=@CUSTOM_INDEX_ID";

                    custom_index_idParam.ParameterName = "@CUSTOM_INDEX_ID";
                    custom_index_idParam.SqlDbType = SqlDbType.NVarChar;
                    custom_index_idParam.Direction = ParameterDirection.Input;
                    custom_index_idParam.Value = ddlIndex.SelectedItem.Value;

                    record_dateParam.ParameterName = "@RECORD_DATE";
                    record_dateParam.SqlDbType = SqlDbType.DateTime;
                    record_dateParam.Direction = ParameterDirection.Input;
                    record_dateParam.Value = Convert.ToDateTime(txttDate.Text);

                    custom_index_valueParam.ParameterName = "@CUSTOM_INDEX_VALUE";
                    custom_index_valueParam.SqlDbType = SqlDbType.Float;
                    custom_index_valueParam.Direction = ParameterDirection.Input;
                    custom_index_valueParam.Value = txtIndexValue.Text.Trim();

                    login_idParam.ParameterName = "@LOGIN_ID";
                    login_idParam.SqlDbType = SqlDbType.Int;
                    login_idParam.Direction = ParameterDirection.Input;
                    if (Session["userid"] != null)
                    {
                        if (Convert.ToString(Session["userid"]) == "tataindex")
                            login_idParam.Value = 0;
                    }
                    else
                    {
                        login_idParam.Value = -1;
                        ErrorLog(sql, "There is no login ID");
                    }


                }
                else
                {

                    sql = "INSERT INTO [T_MFIE_CUSTOM_INDEX_RECORDS] ([CUSTOM_INDEX_ID],[RECORD_DATE],[CUSTOM_INDEX_VALUE],[LOGIN_ID]) VALUES (@CUSTOM_INDEX_ID,@RECORD_DATE,@CUSTOM_INDEX_VALUE,@LOGIN_ID)";

                    custom_index_idParam.ParameterName = "@CUSTOM_INDEX_ID";
                    custom_index_idParam.SqlDbType = SqlDbType.NVarChar;
                    custom_index_idParam.Direction = ParameterDirection.Input;
                    custom_index_idParam.Value = ddlIndex.SelectedItem.Value;

                    record_dateParam.ParameterName = "@RECORD_DATE";
                    record_dateParam.SqlDbType = SqlDbType.DateTime;
                    record_dateParam.Direction = ParameterDirection.Input;
                    string record_date = Convert.ToDateTime(txttDate.Text).ToString("dd/MM/yyyy");
                    record_dateParam.Value = Convert.ToDateTime(txttDate.Text);

                    custom_index_valueParam.ParameterName = "@CUSTOM_INDEX_VALUE";
                    custom_index_valueParam.SqlDbType = SqlDbType.Float;
                    custom_index_valueParam.Direction = ParameterDirection.Input;
                    custom_index_valueParam.Value = txtIndexValue.Text.Trim();

                    login_idParam.ParameterName = "@LOGIN_ID";
                    login_idParam.SqlDbType = SqlDbType.Int;
                    login_idParam.Direction = ParameterDirection.Input;
                    if (Session["userid"] != null)
                    {
                        if (Convert.ToString(Session["userid"]) == "tataindex")
                            login_idParam.Value = 0;
                    }
                    else
                    {
                        login_idParam.Value = -1;
                        ErrorLog(sql, "There is no login ID");
                    }
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(custom_index_idParam);
                cmd.Parameters.Add(record_dateParam);
                cmd.Parameters.Add(custom_index_valueParam);
                cmd.Parameters.Add(login_idParam);





                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    success = true;
                }

                #region USP_GROSS_VALUE_CALC_TATA
                cmd = new SqlCommand("USP_GROSS_VALUE_CALC_TATA", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                i = cmd.ExecuteNonQuery();

                #endregion

                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable CheckDbValue(string _indexID, string _date, string _value)
        {
            bool flag = false;
            DataTable dt = new DataTable();
            string sql = string.Empty;

            sql = "SELECT CUSTOM_INDEX_ID,RECORD_DATE,CUSTOM_INDEX_VALUE FROM T_MFIE_CUSTOM_INDEX_RECORDS WHERE CUSTOM_INDEX_ID=@CUSTOM_INDEX_ID and RECORD_DATE=@RECORD_DATE";

            SqlParameter indexidParam = new SqlParameter();
            SqlParameter dateParam = new SqlParameter();
            SqlParameter valueParam = new SqlParameter();

            indexidParam.ParameterName = "@CUSTOM_INDEX_ID";
            indexidParam.SqlDbType = SqlDbType.Int;
            indexidParam.Direction = ParameterDirection.Input;
            indexidParam.Value = Convert.ToInt32(_indexID);

            dateParam.ParameterName = "@RECORD_DATE";
            dateParam.SqlDbType = SqlDbType.DateTime;
            dateParam.Direction = ParameterDirection.Input;
            dateParam.Value = Convert.ToDateTime(_date);

            valueParam.ParameterName = "@CUSTOM_INDEX_VALUE";
            valueParam.SqlDbType = SqlDbType.Float;
            valueParam.Direction = ParameterDirection.Input;
            valueParam.Value = Convert.ToDouble(_value);


            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(indexidParam);
                cmd.Parameters.Add(dateParam);
                cmd.Parameters.Add(valueParam);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertAfterCheck()
        {
            bool success = false;
            string sql = string.Empty;
            System.Text.StringBuilder sb = new Text.StringBuilder();
            SqlCommand cmd = null;

            //sb.AppendLine("INSERT INTO T_MFIE_CUSTOM_INDEX_RECORDS  (CUSTOM_INDEX_ID,RECORD_DATE,CUSTOM_INDEX_VALUE,LOGIN_ID)");
            sb.AppendLine("SELECT DISTINCT");
            sb.AppendLine("CUSTOM_INDEX_ID,RECORD_DATE,CUSTOM_INDEX_VALUE,LOGIN_ID FROM tblTemp AS s WHERE NOT EXISTS");
            sb.AppendLine("(SELECT *  FROM T_MFIE_CUSTOM_INDEX_RECORDS As t WHERE t.CUSTOM_INDEX_ID = s.CUSTOM_INDEX_ID and t.RECORD_DATE = s.RECORD_DATE and t.CUSTOM_INDEX_VALUE = s.CUSTOM_INDEX_VALUE)");

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(sb.ToString().Trim(), conn);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                sb = null;

                foreach (DataRow dr in dt.Rows)
                {
                    sb = new Text.StringBuilder();
                    sb.AppendLine(" update T_MFIE_CUSTOM_INDEX_RECORDS set CUSTOM_INDEX_VALUE='" + dr["CUSTOM_INDEX_VALUE"] + "' where CUSTOM_INDEX_ID='" + Convert.ToString(dr["CUSTOM_INDEX_ID"]) + "' and RECORD_DATE='" + Convert.ToDateTime(dr["RECORD_DATE"]).ToString("dd MMM yyyy") + "'");
                    cmd = new SqlCommand(sb.ToString().Trim(), conn);
                    int iUpdate = cmd.ExecuteNonQuery();
                    sb = null;
                    sb = new Text.StringBuilder();
                    if (iUpdate == 0)
                    {
                        sb.AppendLine(" INSERT INTO T_MFIE_CUSTOM_INDEX_RECORDS (CUSTOM_INDEX_ID,RECORD_DATE,CUSTOM_INDEX_VALUE,LOGIN_ID) VALUES('" + Convert.ToString(dr["CUSTOM_INDEX_ID"]) + "','" + Convert.ToDateTime(dr["RECORD_DATE"]).ToString("dd MMM yyyy") + "','" + Convert.ToString(dr["CUSTOM_INDEX_VALUE"]) + "','" + Convert.ToString(dr["LOGIN_ID"]) + "') ");

                        cmd = new SqlCommand(sb.ToString().Trim(), conn);
                        int iInsert = cmd.ExecuteNonQuery();
                        if (iInsert > 0)
                        {
                            success = true;
                        }
                    }
                }

                sql = string.Empty;
                sql = "truncate table tblTemp ";
                cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                int j = cmd.ExecuteNonQuery();

                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        

        #region: Clear/Reset Method
        private void Reset()
        {
            ddlIndex.SelectedIndex = 0;
            txttDate.Text = "DD-MMM-YYYY";
            txtIndexValue.Text = string.Empty;
        }

        #endregion

        #region: Excell Method

        public DataTable FillGridFromExcell(string _strFileName)
        {
            DataTable dtReturned = new DataTable();
            string _ExcelFilePath = null;
            string SheetName = null;
            _ExcelFilePath = _strFileName;
            SheetName = "Upload Index";

            try
            {
                string sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + _ExcelFilePath + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                using (OleDbConnection objConn = new OleDbConnection(sConnectionString))
                {
                    objConn.Open();
                    OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + SheetName + "$]", objConn);
                    OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                    objAdapter.SelectCommand = objCmdSelect;
                    objAdapter.Fill(dtReturned);
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {


            }
            return dtReturned;
        }
        public bool InsertDataFromExcellToDb(DataTable _dt, string CustomIndexId)
        {
            bool success = false;
            string sql = string.Empty;
            string filepath = string.Empty;
            filepath = HttpContext.Current.Server.MapPath("~//CanaraClient//ErrorLog.txt");
            System.IO.File.Delete(filepath);
            using (System.IO.File.Create(filepath))
            {
            }
            try
            {
                sql = string.Empty;

                string CUSTOM_INDEX_ID = string.Empty;
                string RECORD_DATE = string.Empty;
                string CUSTOM_INDEX_VALUE = string.Empty;
                string LOGIN_ID = string.Empty;

                SqlParameter custom_index_idParam = null;
                SqlParameter record_dateParam = null;
                SqlParameter custom_index_valueParam = null;
                SqlParameter login_idParam = null;

                foreach (DataRow dr in _dt.Rows)
                {
                    RECORD_DATE = dr["Date"].ToString();
                    CUSTOM_INDEX_VALUE = dr[1].ToString();

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    custom_index_idParam = new SqlParameter();
                    record_dateParam = new SqlParameter();
                    custom_index_valueParam = new SqlParameter();
                    login_idParam = new SqlParameter();

                    sql = "INSERT INTO [T_MFIE_CUSTOM_INDEX_RECORDS] ([CUSTOM_INDEX_ID],[RECORD_DATE],[CUSTOM_INDEX_VALUE],[LOGIN_ID]) VALUES (@CUSTOM_INDEX_ID,@RECORD_DATE,@CUSTOM_INDEX_VALUE,@LOGIN_ID)";

                    custom_index_idParam.ParameterName = "@CUSTOM_INDEX_ID";
                    custom_index_idParam.SqlDbType = SqlDbType.NVarChar;
                    custom_index_idParam.Direction = ParameterDirection.Input;
                    custom_index_idParam.Value = Convert.ToInt32(CustomIndexId);

                    record_dateParam.ParameterName = "@RECORD_DATE";
                    record_dateParam.SqlDbType = SqlDbType.DateTime;
                    record_dateParam.Direction = ParameterDirection.Input;
                    record_dateParam.Value = Convert.ToDateTime(dr["Date"]);

                    custom_index_valueParam.ParameterName = "@CUSTOM_INDEX_VALUE";
                    custom_index_valueParam.SqlDbType = SqlDbType.VarChar;
                    custom_index_valueParam.Direction = ParameterDirection.Input;

                    if (inout.Path.GetFileNameWithoutExtension(FUExcell.FileName) == "MSCI World - Historical Indices")
                    {
                        if (!dr.IsNull(1))
                        {
                            custom_index_valueParam.Value = Convert.ToDouble(dr[1]);
                        }
                        else
                        {
                            custom_index_valueParam.Value = 0.00;
                            ErrorLog(sql, "WORLD Standard (Large+Mid Cap)  is null ");
                        }
                    }
                    else if (inout.Path.GetFileNameWithoutExtension(FUExcell.FileName) == "MSCI Emerging Markets - Historical Indices")
                    {
                        if (!dr.IsNull(1))
                        {
                            custom_index_valueParam.Value = Convert.ToDouble(dr[1]);
                        }
                        else
                        {
                            custom_index_valueParam.Value = 0.00;
                            ErrorLog(sql, "EM (EMERGING MARKETS) Standard (Large+Mid Cap) is null ");
                        }
                    }


                    login_idParam.ParameterName = "@LOGIN_ID";
                    login_idParam.SqlDbType = SqlDbType.Int;
                    login_idParam.Direction = ParameterDirection.Input;
                    if (Session["userid"] != null)
                    {
                        if (Convert.ToString(Session["userid"]) == "tataindex")
                            login_idParam.Value = 1;
                    }
                    else
                    {
                        login_idParam.Value = 0;
                        ErrorLog(sql, "There is no login ID");
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(custom_index_idParam);
                    cmd.Parameters.Add(record_dateParam);
                    cmd.Parameters.Add(custom_index_valueParam);
                    cmd.Parameters.Add(login_idParam);

                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        success = true;
                    }
                    sql = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(sql, ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return success;
        }
        public string GetCustomIndexId(string filename)
        {
            string sql = string.Empty;
            DataSet ds = new DataSet();
            string CustomIndexId = string.Empty;
            try
            {
                sql = "SELECT CUSTOM_INDEX_ID FROM T_MFIE_CUSTOM_INDEX_MASTER WHERE CUSTOM_INDEX_NAME=@CUSTOM_INDEX_NAME";

                SqlParameter custom_index_idParam = new SqlParameter();
                custom_index_idParam.ParameterName = "@CUSTOM_INDEX_NAME";
                custom_index_idParam.SqlDbType = SqlDbType.VarChar;
                custom_index_idParam.Direction = ParameterDirection.Input;
                custom_index_idParam.Value = filename;

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(custom_index_idParam);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    CustomIndexId = ds.Tables[0].Rows[0]["CUSTOM_INDEX_ID"].ToString();
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return CustomIndexId;
        }

        private static List<ISheet> GetFileStream(string fullFilePath)
        {
            var fileExtension = Path.GetExtension(fullFilePath);
            string sheetName;
            List<ISheet> sheets = new List<ISheet>();
            switch (fileExtension)
            {
                case ".xlsx":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        int numSheets = wb.NumberOfSheets;
                        for (int i = 0; i < numSheets; i++)
                        {
                            sheetName = wb.GetSheetAt(i).SheetName;
                            var sheet = (XSSFSheet)wb.GetSheet(sheetName);
                            sheets.Add(sheet);
                        }
                    }
                    break;
                case ".xls":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        int numSheets = wb.NumberOfSheets;
                        for (int i = 0; i < numSheets; i++)
                        {
                            sheetName = wb.GetSheetAt(i).SheetName;
                            sheets.Add((XSSFSheet)wb.GetSheet(sheetName));
                        }
                    }
                    break;
            }
            return sheets;
        }

        private DataTable GetRequestsDataFromExcel(string fullFilePath)
        {
            try
            {
                string[] sheetList = { "DOMESTIC PRICE OF GOLD", "CRISIL DYNAMIC GILT", "CRISIL ULTRA SHORT TERM DEBT", "CRISIL MONEY MARKET INDEX" };
                var sheets = GetFileStream(fullFilePath);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sheets[0].GetRow(0);
                int colCount = headerRow.LastCellNum;
                dtExcelTable.Columns.Add("CUSTOM_INDEX_ID", typeof(string));
                dtExcelTable.Columns.Add("RECORD_DATE", typeof(string));
                dtExcelTable.Columns.Add("CUSTOM_INDEX_VALUE", typeof(string));
                Dictionary<string, decimal> lstIndex = new Dictionary<string, decimal>();
                foreach (var row in headerRow)
                {
                    using (var DBContext = new TataCalculatorDataContext())
                    {
                        decimal indexId = 0;
                        string indexName = RemoveSpecificChar(row.StringCellValue.ToString().Trim().ToUpper(), false, true, false);
                        indexId = (from r in DBContext.T_MFIE_CUSTOM_INDEX_MASTERs
                                   where r.CUSTOM_INDEX_NAME.ToUpper() == indexName.ToUpper()
                                   select r.CUSTOM_INDEX_ID).FirstOrDefault();
                        lstIndex.Add(row.StringCellValue.ToString(), indexId);
                    }
                }
                for (int i = 1; i <= sheets[0].LastRowNum; i++)
                {
                    if (sheets[0].GetRow(i) != null)
                    {
                        var date = GetCellValue(sheets[0].GetRow(i).GetCell(0));
                        if (!String.IsNullOrEmpty(date))
                        {
                            for (int j = 1; j < colCount; j++)
                            {
                                string indexName = GetCellValue(headerRow.GetCell(j)).ToUpper();
                                var indxId = lstIndex.Where(x => x.Key.ToUpper() == indexName).Select(x => x.Value).FirstOrDefault();
                                var cellValue = GetCellValue(sheets[0].GetRow(i).GetCell(j)); //CUSTOM INDEX VALUE
                                if (indxId > 0 && !String.IsNullOrEmpty(cellValue))
                                {
                                    DataRow dr = dtExcelTable.NewRow();
                                    dr[0] = indxId;
                                    dr[1] = date;
                                    dr[2] = cellValue;
                                    dtExcelTable.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                return dtExcelTable;
                //foreach (ISheet sheet in sheets)
                //{
                //    decimal indexId = 0;
                //    if (sheetList.Contains(sheet.SheetName.Trim().ToUpper()))
                //    {
                //        if (!String.IsNullOrEmpty(sheet.SheetName))
                //        {
                //            using (var DBContext = new TataCalculatorDataContext())
                //            {
                //                indexId = (from r in DBContext.T_MFIE_CUSTOM_INDEX_MASTERs
                //                           where r.CUSTOM_INDEX_NAME.ToUpper() == sheet.SheetName.Trim().ToUpper()
                //                           select r.CUSTOM_INDEX_ID).FirstOrDefault();
                //            }
                //        }
                //        if (indexId > 0)
                //            BindSheetToDT(sheet, indexId, ref dtExcelTable);
                //        else
                //            errSheets.Add(sheet.SheetName);
                //    }
                //return dtExcelTable;
                //}
            }

            catch (Exception e)
            {
                throw;
            }
        }

        private string RemoveSpecificChar(string input, bool first, bool last, bool anyWhere)
        {
            string[] charList = { "*" };
            string result = input;
            int index = -1;
            foreach (var c in charList)
            {
                if (input.ToLower().Contains(c))
                {
                    index = input.ToLower().IndexOf(c);
                    if (first && index == 0)
                    {
                        result = input.Substring(0);
                    }
                    index = result.ToLower().IndexOf(c);
                    if (last && index == result.Length - 1)
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                    if (anyWhere)
                    {
                        result = result.Replace(c, "");
                    }
                }
            }
            return result;
        }

        public string GetCellValue(NPOI.SS.UserModel.ICell val)
        {
            string output = string.Empty;
            if (val == null)
                return output;

            switch (val.CellType)
            {
                case NPOI.SS.UserModel.CellType.Numeric:
                    //output = val.NumericCellValue.ToString().Trim();
                    output = DateUtil.IsCellDateFormatted(val)
                        ? val.DateCellValue.ToString()
                        : val.NumericCellValue.ToString();
                    break;
                case NPOI.SS.UserModel.CellType.Blank:
                    output = string.Empty;
                    break;
                case NPOI.SS.UserModel.CellType.Boolean:
                    output = val.BooleanCellValue.ToString().Trim();
                    break;
                case NPOI.SS.UserModel.CellType.Error:
                    output = string.Empty;
                    break;
                case NPOI.SS.UserModel.CellType.String:
                    output = val.StringCellValue.Trim();
                    break;
                case NPOI.SS.UserModel.CellType.Formula:
                    switch (val.CachedFormulaResultType)
                    {
                        case CellType.String:
                            output = val.RichStringCellValue.String.Trim();
                            break;

                        case CellType.Numeric:
                            output = val.NumericCellValue.ToString().Trim();
                            break;
                    }
                    break;
                case NPOI.SS.UserModel.CellType.Unknown:
                    output = string.Empty;
                    break;
                default:
                    output = string.Empty;
                    break;
            }
            return output;
        }

        public void BindSheetToDT(ISheet sh, decimal indexId, ref DataTable dtExcelTable)
        {
            //DataTable dtExcelTable = new DataTable();
            //try
            //{

            string shetName = sh.SheetName.ToString().Trim().ToUpper();
            var i = 1;
            var currentRow = sh.GetRow(i);
            while (currentRow != null)
            {
                DataRow dr = dtExcelTable.NewRow();

                for (var j = 0; j < 2; j++)
                {
                    var cell = currentRow.GetCell(j);

                    if (cell != null)
                        switch (cell.CellType)
                        {
                            case CellType.Numeric:
                                dr[j + 1] = DateUtil.IsCellDateFormatted(cell)
                                    ? String.IsNullOrEmpty(cell.NumericCellValue.ToString().Trim()) ? "" : Convert.ToDateTime(cell.DateCellValue).ToString("dd MMM yyyy").Trim()
                                    : String.IsNullOrEmpty(cell.NumericCellValue.ToString().Trim()) ? "0" : cell.NumericCellValue.ToString().Trim();
                                break;
                            case CellType.String:
                                dr[j + 1] = cell.StringCellValue.Trim();
                                break;
                            case CellType.Blank:

                                break;
                        }

                }
                dr[0] = indexId;
                if (dr[1] != DBNull.Value && dr[2] != DBNull.Value)
                    dtExcelTable.Rows.Add(dr);
                i++;
                currentRow = sh.GetRow(i);
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return dtExcelTable;
        }
        #endregion

        #region: Common

        public string HandleQuate(string str)
        {
            return str.Replace("'", "''");
        }
        public void ErrorLog(string query, string errormsg)
        {
            try
            {
                string filepath = HttpContext.Current.Server.MapPath("~//CanaraClient//ErrorLog.txt");
                if (System.IO.File.Exists(filepath))
                {
                    System.Text.StringBuilder sb = new Text.StringBuilder();
                    sb.AppendLine(" ");
                    sb.AppendLine(" ");
                    sb.AppendLine("SQL QUERY ");
                    sb.AppendLine(" ");
                    sb.AppendLine(" ");
                    sb.AppendLine(query);
                    sb.AppendLine(" ");
                    sb.AppendLine(" ");
                    sb.AppendLine("Error Message");
                    sb.AppendLine(" ");
                    sb.AppendLine(" ");
                    sb.AppendLine(errormsg);
                    System.IO.File.AppendAllText(filepath, sb.ToString());
                }
            }
            catch (Exception ex)
            {


            }
        }

        #endregion

        #region: Bluk method

        public bool BulkInsertToTemp(DataTable _dt)
        {
            bool success = false;
            DataSet ds = new DataSet();
            conn.Open();

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.DestinationTableName = "dbo.tblTemp";
                //try
                //{
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(0, "CUSTOM_INDEX_ID"));
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(1, "RECORD_DATE"));
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(2, "CUSTOM_INDEX_VALUE"));
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(3, "LOGIN_ID"));

                bulkCopy.WriteToServer(_dt);
                conn.Close();
                success = true;
                return success;
                //}
                //catch (Exception ex)
                //{
                //    return success;
                //}
            }
        }

        #endregion

        #endregion
    }
    #region Enum
    public enum EnumIndexName
    {
        None,
        ISecCompositeIndex,
        MSCIWorldHistoricalIndices,
        MSCIEmergingMarketsHistoricalIndices
    }
    #endregion
}