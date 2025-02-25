using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.DSPApp;
using System.IO;
using System.Windows.Controls;
using Microsoft.VisualBasic.ApplicationServices;
using iFrames.BLL;
using iFrames.DAL;
using System.Windows.Documents;
using System.Data.Linq;
//using Microsoft.Office.Interop.Excel;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Functions;
using System.Windows.Shell;
using System.Collections.Concurrent;
using System.Windows.Shapes;

namespace iFrames.CanaraClient
{
    public partial class ExcelFileUpload : System.Web.UI.Page
    {
        private static int rowNum;
        private static object rowArray;
        public bool statechecker=false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");

            //if (IsPostBack)
            //{
            //    BtnActionEnableFalse();
            //    Button3.Enabled = false;
            //}
            grvExcelData.Visible = false;
            Button2.Visible = false;
            //Button3.Enabled = false;
            //BtnActionEnableFalse();
        }
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        //Export Template With Data
        protected void btnTmpDownload_Click(object sender, EventArgs e)
        {
            // Sample System.Data.DataTable
            
            BLL.CanaraClient_BLL bl = new BLL.CanaraClient_BLL();
            System.Data.DataTable dt = bl.GetTemplateGenarateCode();
            string todate = System.DateTime.Now.ToString("ddMMyyyy");
            byte[] fileContents = GenarateDTtoExcel(dt);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=StaticData-CanaraMF-" + todate + ".xlsx");
            Response.BinaryWrite(fileContents);
            Response.End();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLoader", "hideLoader();", true);
        }
        //End

        //Import Template Data to Database
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Button1.Enabled = false;
                if (Session["UserId"] == null)
                    Response.Redirect("Login.aspx");
                grvExcelData.DataSource = null;
                grvExcelData.DataBind();
                string Outputmsg = "";
                grvExcelData.Visible = false;
                Button2.Visible = false;
                if (FileUpload1.HasFile)
                {
                    string filepath = string.Empty;
                    string filename = string.Empty;
                    string fileext = string.Empty;
                    string path1 = HttpContext.Current.Server.MapPath("~/CanaraClient/UploadFile/");
                    if (!Directory.Exists(path1))
                    {
                        //Create the directory on the given path  
                        Directory.CreateDirectory(path1);
                    }
                    string todate = System.DateTime.Now.ToString("MMddyyyyhhmmss");
                    filepath = HttpContext.Current.Server.MapPath("~/CanaraClient/UploadFile/") + FileUpload1.FileName;
                    fileext = System.IO.Path.GetExtension(filepath);
                    if (fileext.ToLowerInvariant() == ".xlsx")
                    {
                        filename = System.IO.Path.GetFileNameWithoutExtension(filepath);
                        filepath = HttpContext.Current.Server.MapPath("~/CanaraClient/UploadFile/") + filename + "_" + todate + fileext;

                        if (Directory.Exists(HttpContext.Current.Server.MapPath("~/CanaraClient/UploadFiles")))
                        {
                            string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/CanaraClient/UploadFile"));
                            foreach (string filePath in filePaths)
                                File.Delete(filePath);
                        }
                        FileUpload1.SaveAs(filepath);
                        DataTable dt = ExcelToDataTable(filepath);
                        //DataTable dt1 = ReadExcelToDatatable(filepath);

                        SaveUpdateRecord(dt);
                        //UpdateRecord(dt);
                        string[] keyColumns = { "Scheme Name", "Amfi Code", "Risk", "Benchmark Risk", "AUM", "Aum Date", "Inception Date", "Horizon", "Goal", "Benchmark", "Additional Benchmark", "About Fund", "Investment Objective", "Exit Load", "Entry Load", "Prescribed Asset Allocation","Suitable For", "PT Ratio", "Expense Ratio Regular", "Expense Ratio Direct", "Product Suitable for", "Reason to invest", "Fund Manager Name", "From Date", "image link", "Doc Link", "Min amountSIP", "Min amountSWP", "Min amountSTP", "Min amountLumpsum", "Min amountRedeem" }; // Replace with your actual column names
                        var distinctRows = dt.AsEnumerable()
                                             .GroupBy(row => string.Join("|", keyColumns.Select(col => row[col].ToString())))
                                             .Select(g => g.First())
                                             .CopyToDataTable();
                        grvExcelData.DataSource = distinctRows;
                        grvExcelData.DataBind();
                        grvExcelData.Visible = true;
                        pnlGridview.Visible = true;
                        Button2.Visible = true;
                        Outputmsg = "Record(s) uploaded successfully.";
                        Response.Write($"<script LANGUAGE='JavaScript' >alert('{Outputmsg}')</script>");
                        Button1.Enabled = true;
                    }
                    else
                    {
                        Response.Write("<script LANGUAGE='JavaScript' >alert('Sorry! only .xlsx file allow to process')</script>");
                        Button1.Enabled = true;
                    }
                }
                else
                {
                    Response.Write("<script LANGUAGE='JavaScript' >alert('Please Select a File')</script>");
                    Button1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                grvExcelData.Visible = false;
                string errorMessage = HttpUtility.JavaScriptStringEncode(ex.Message.ToString());
                Response.Write($"<script LANGUAGE='JavaScript' >alert('{errorMessage}')</script>");
                Button1.Enabled = true;
                ClientScript.RegisterStartupScript(this.GetType(), "hideLoader", "hideLoader();", true);
            }
            
        }
        public DataTable ExcelToDataTable(string filePath)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;

                for (int i = 0; i < cellCount; i++)
                {
                    dt.Columns.Add(headerRow.GetCell(i).ToString());
                }

                //for (int i = 1; i <= sheet.LastRowNum; i++)
                //{
                //    IRow row = sheet.GetRow(i);
                //    DataRow dataRow = dt.NewRow();

                //    for (int j = 0; j < cellCount; j++)
                //    {
                //        if (row.GetCell(j) != null)
                //        {
                //            dataRow[j] = row.GetCell(j).ToString();
                //        }
                //    }

                //    dt.Rows.Add(dataRow);
                //}
                Parallel.For(1, sheet.LastRowNum + 1, i =>
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) return; // Skip empty rows

                    DataRow dataRow = dt.NewRow();

                    for (int j = 0; j < cellCount; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell != null)
                        {
                            dataRow[j] = cell.ToString();
                        }
                    }

                    lock (dt)
                    {
                        dt.Rows.Add(dataRow);
                    }
                });
            }
            return dt;
        }

        //End
        //Export Preview Data to Excel
        protected void btnExport_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            // Add columns to System.Data.DataTable.
            foreach (System.Web.UI.WebControls.TableCell cell in grvExcelData.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            // Add rows to System.Data.DataTable.
            foreach (GridViewRow row in grvExcelData.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dr[i] = row.Cells[i].Text.Replace("&nbsp;", "");
                }
                dt.Rows.Add(dr);
            }
            
            byte[] fileContents=GenarateDTtoExcel(dt);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=ExportedData.xlsx");
            Response.BinaryWrite(fileContents);
            Response.End();
        }
        //End
        //Page Refresh
        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        //End
        
        private byte[] GenarateDTtoExcel(DataTable dt)
        {
            using (var memoryStream = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");

                // Create header row
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                }

                // Create data rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                workbook.Write(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private void SaveUpdateRecord(System.Data.DataTable Exceldt)
        {
            string Outputmsg = "Record(s) uploaded successfully.";
            try {
                int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                BLL.CanaraClient_BLL dal = new BLL.CanaraClient_BLL();
                Parallel.For(0, Exceldt.Rows.Count, j =>
                //for (var j = 0; j < Exceldt.Rows.Count; j++)
                {
                    //using (var dc = new CanaraClientDataContext())
                    //{
                    string AmpCode = Exceldt.Rows[j][1]?.ToString();
                    //DateTime? dateValue = null;
                    DateTime? Aum_Date = null;
                    DateTime? Inception_Date = null;
                    if (DateTime.TryParse(Exceldt.Rows[j][5].ToString(), out DateTime tempDate))
                    {
                        Aum_Date = tempDate;
                    }
                    if (DateTime.TryParse(Exceldt.Rows[j][6].ToString(), out DateTime tempDate1))
                    {
                        Inception_Date = tempDate1;
                    }
                    if (!string.IsNullOrEmpty(AmpCode))
                    {
                        int SchemeID = dal.GetSchemeIDByAMpCode(Convert.ToInt32(AmpCode));
                        //DateTime? Aum_Date = Exceldt.Rows[j][5] != DBNull.Value ? (DateTime?)Convert.ToDateTime(Exceldt.Rows[j][5]) : null;
                        //DateTime? Inception_Date = Exceldt.Rows[j][6] != DBNull.Value ? (DateTime?)Convert.ToDateTime(Exceldt.Rows[j][6]) : null;
                        decimal? tempAUM = null;
                        if (decimal.TryParse(Exceldt.Rows[j][4]?.ToString().Trim(), out decimal result))
                        {
                            tempAUM = result;
                        }
                        decimal? tempPTRatio = null;
                        if (decimal.TryParse(Exceldt.Rows[j][17]?.ToString().Trim(), out decimal result1))
                        {
                            tempPTRatio = result1;
                        }

                        T_CANARA_SCHEMES_STATIC_DATA objStaticData = new T_CANARA_SCHEMES_STATIC_DATA
                        {
                            Amfi_Code = Convert.ToInt32(AmpCode),
                            Scheme_ID = SchemeID,
                            Risk = Exceldt.Rows[j][2]?.ToString(),
                            Benchmark_Risk = Exceldt.Rows[j][3]?.ToString(),
                            AUM = tempAUM,
                            Aum_Date = Aum_Date,
                            Inception_Date = Inception_Date,
                            Horizon = Exceldt.Rows[j][7]?.ToString(),
                            Goal = Exceldt.Rows[j][8]?.ToString(),
                            Benchmark = Exceldt.Rows[j][9]?.ToString(),
                            Additional_Benchmark = Exceldt.Rows[j][10]?.ToString(),
                            About_Fund = Exceldt.Rows[j][11]?.ToString(),
                            Investment_Objective = Exceldt.Rows[j][12]?.ToString(),
                            Exit_Load = Exceldt.Rows[j][13]?.ToString(),
                            Entry_Load = Exceldt.Rows[j][14]?.ToString(),
                            Prescribed_Asset_Allocation = Exceldt.Rows[j][15]?.ToString(),
                            Suitable_For = Exceldt.Rows[j][16]?.ToString(),
                            PT_Ratio = tempPTRatio,
                            Expense_Ratio_Regular = Exceldt.Rows[j][18]?.ToString(),
                            Expense_Ratio_Direct = Exceldt.Rows[j][19]?.ToString(),
                            Product_Suitable_for = Exceldt.Rows[j][20]?.ToString(),
                            Reason_To_Invest = Exceldt.Rows[j][21]?.ToString(),
                            Min_Amount_SIP = Exceldt.Rows[j][26]?.ToString(),
                            Min_Amount_SWP = Exceldt.Rows[j][27]?.ToString(),
                            Min_Amount_STP = Exceldt.Rows[j][28]?.ToString(),
                            Min_Amount_Lumpsum = Exceldt.Rows[j][29]?.ToString(),
                            Min_Amount_Redeem = Exceldt.Rows[j][30]?.ToString()
                        };
                        // FundManagerName Section
                        dal.UpdataStaticDataSIngleObj(objStaticData, userId);
                        dal.DeleteFundmanagerData(SchemeID);
                        List<FundManagerClass> fundManagerDataList = new List<FundManagerClass>();
                        string fundManagerNames = Exceldt.Rows[j][22]?.ToString();
                        string fromDates = Exceldt.Rows[j][23]?.ToString();
                        string imageLinks = Exceldt.Rows[j][24]?.ToString();
                        string docLinks = Exceldt.Rows[j][25]?.ToString();
                        if (!string.IsNullOrEmpty(fundManagerNames))
                        {
                            string[] fundManagerArray = fundManagerNames.Split('#');
                            string[] fromDatesArray = fromDates?.Split('#') ?? new string[0];
                            string[] imageLinksArray = imageLinks?.Split('#') ?? new string[0];
                            string[] docLinksArray = docLinks?.Split('#') ?? new string[0];

                            for (int z = 0; z < fundManagerArray.Length; z++)
                            {
                                DateTime? FromDates = null;
                                if (fromDatesArray.Length > z)
                                {
                                    if (DateTime.TryParse(fromDatesArray[z].ToString(), out DateTime tempDate3))
                                    {
                                        FromDates = tempDate3;
                                    }
                                }

                                FundManagerClass data = new FundManagerClass
                                {
                                    FundManagerNames = fundManagerArray[z].Trim(),
                                    //index = z + 1,
                                    FromDates = (z < fromDatesArray.Length && !string.IsNullOrEmpty(fromDatesArray[z])) ? FromDates : null,
                                    ImageLinks = z < imageLinksArray.Length && !string.IsNullOrEmpty(imageLinksArray[z]) ? imageLinksArray[z] : null,
                                    DocLinks = z < docLinksArray.Length && !string.IsNullOrEmpty(docLinksArray[z]) ? docLinksArray[z] : null
                                };
                                fundManagerDataList.Add(data);
                            }
                            dal.InsertFundmanagerData(fundManagerDataList, userId, SchemeID);
                        }
                    }
                    //}
                });
                //var dupes = sdarray.GroupBy(x => new { x.Scheme_ID, x.Amfi_Code }).Where(x => x.Skip(1).Any());
                //var bar = sdarray.GroupBy(x => x.Scheme_ID).Select(x => x.First()).ToList();
                Outputmsg = "Record(s) uploaded successfully.";
            }
            catch(Exception ex) {
                Outputmsg = HttpUtility.JavaScriptStringEncode(ex.Message.ToString());
                throw;
            }
            //return Outputmsg;
        }
        private string SaveUpdateRecord_backup(System.Data.DataTable Exceldt)
        {
            string Outputmsg = "Record(s) uploaded successfully.";
            try
            {
                int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                BLL.CanaraClient_BLL dal = new BLL.CanaraClient_BLL();
                for (int j = 0; j < Exceldt.Rows.Count; j++)
                {
                    string AmpCode = "";
                    if (Exceldt.Rows[j][1] != DBNull.Value && !string.IsNullOrEmpty(Exceldt.Rows[j][1].ToString()))
                    {
                        AmpCode = Exceldt.Rows[j][1]?.ToString();
                        int SchemeID = dal.GetSchemeIDByAMpCode(Convert.ToInt32(AmpCode));
                        DateTime? Aum_Date = Exceldt.Rows[j][5] != DBNull.Value ? (DateTime?)Convert.ToDateTime(Exceldt.Rows[j][5]) : null;
                        DateTime? Inception_Date = Exceldt.Rows[j][6] != DBNull.Value ? (DateTime?)Convert.ToDateTime(Exceldt.Rows[j][6]) : null;
                        string cellAUMValue = Exceldt.Rows[j][4].ToString().Trim();
                        decimal? tempAUM = null;
                        if (!string.IsNullOrEmpty(cellAUMValue) && decimal.TryParse(cellAUMValue, out decimal result))
                        {
                            tempAUM = result;
                        }
                        string cellPTRatioValue = Exceldt.Rows[j][17].ToString().Trim();
                        decimal? tempPTRatio = null;
                        if (!string.IsNullOrEmpty(cellPTRatioValue) && decimal.TryParse(cellPTRatioValue, out decimal result1))
                        {
                            tempPTRatio = result1;
                        }
                        T_CANARA_SCHEMES_STATIC_DATA objStaticData = new T_CANARA_SCHEMES_STATIC_DATA
                        {
                            Amfi_Code = Convert.ToInt32(AmpCode),
                            Scheme_ID = SchemeID,
                            Risk = Exceldt.Rows[j][2]?.ToString(),
                            Benchmark_Risk = Exceldt.Rows[j][3]?.ToString(),
                            AUM = tempAUM,//Exceldt.Rows[j][4] != DBNull.Value ? Convert.ToDecimal(Exceldt.Rows[j][4]) : 0,
                            Aum_Date = Aum_Date,
                            Inception_Date = Inception_Date,
                            Horizon = Exceldt.Rows[j][7]?.ToString(),
                            Goal = Exceldt.Rows[j][8]?.ToString(),
                            Benchmark = Exceldt.Rows[j][9]?.ToString(),
                            Additional_Benchmark = Exceldt.Rows[j][10]?.ToString(),
                            About_Fund = Exceldt.Rows[j][11]?.ToString(),
                            Investment_Objective = Exceldt.Rows[j][12]?.ToString(),
                            Exit_Load = Exceldt.Rows[j][13]?.ToString(),
                            Entry_Load = Exceldt.Rows[j][14]?.ToString(),
                            Prescribed_Asset_Allocation = Exceldt.Rows[j][15]?.ToString(),
                            Suitable_For = Exceldt.Rows[j][16]?.ToString(),
                            PT_Ratio = tempPTRatio,
                            Expense_Ratio_Regular = Exceldt.Rows[j][18]?.ToString(),
                            Expense_Ratio_Direct = Exceldt.Rows[j][19]?.ToString(),
                            Product_Suitable_for = Exceldt.Rows[j][20]?.ToString(),
                            Reason_To_Invest = Exceldt.Rows[j][21]?.ToString(),
                            Min_Amount_SIP = Exceldt.Rows[j][26]?.ToString(),
                            Min_Amount_SWP = Exceldt.Rows[j][27]?.ToString(),
                            Min_Amount_STP = Exceldt.Rows[j][28]?.ToString(),
                            Min_Amount_Lumpsum = Exceldt.Rows[j][29]?.ToString()
                        };
                        //dal.UpdataStaticData(objStaticData, userId);
                        dal.DeleteFundmanagerData(SchemeID);
                        //FundManagerName Section
                        List<FundManagerClass> fundManagerDataList = new List<FundManagerClass>();
                        string fundManagerNames = Exceldt.Rows[j][22]?.ToString();
                        string fromDates = Exceldt.Rows[j][23]?.ToString();
                        string imageLinks = Exceldt.Rows[j][24]?.ToString();
                        string docLinks = Exceldt.Rows[j][25]?.ToString();

                        FundManagerClass fundmagObj = new FundManagerClass();
                        if (fundManagerNames != null)
                        {
                            string[] fundManagerArray = fundManagerNames.Split(',');
                            int z = 1;
                            foreach (string fundManagerName in fundManagerArray)
                            {
                                FundManagerClass data = new FundManagerClass
                                {
                                    FundManagerNames = fundManagerName.Trim(),
                                    index = z
                                };

                                fundManagerDataList.Add(data);
                            }
                            z++;
                        }
                        if (imageLinks != null)
                        {
                            string[] imageLinksArray = imageLinks.Split(',');
                            int z = 1;
                            foreach (string Inglink in imageLinksArray)
                            {
                                var obj = fundManagerDataList.FirstOrDefault(x => x.index == z);
                                if (obj != null) obj.ImageLinks = Inglink;
                            }
                            z++;
                        }
                        if (fromDates != null)
                        {
                            string[] fromDatesArray = fromDates.Split(',');
                            int z = 1;
                            foreach (string fromDate in fromDatesArray)
                            {
                                var obj = fundManagerDataList.FirstOrDefault(x => x.index == z);
                                if (obj != null) obj.FromDates = (fromDate == "" ? null : (DateTime?)Convert.ToDateTime(fromDate));
                            }
                            z++;
                        }
                        if (docLinks != null)
                        {
                            string[] docLinksArray = docLinks.Split(',');
                            int z = 1;
                            foreach (string docLink in docLinksArray)
                            {
                                var obj = fundManagerDataList.FirstOrDefault(x => x.index == z);
                                if (obj != null) obj.DocLinks = docLink;
                            }
                            z++;
                        }
                        dal.InsertFundmanagerData(fundManagerDataList, userId, SchemeID);
                    }
                }
                Outputmsg = "Record(s) uploaded successfully.";
            }
            catch (Exception ex)
            {
                Outputmsg = HttpUtility.JavaScriptStringEncode(ex.Message.ToString()); ;
            }
            return Outputmsg;
        }
        private DataTable ReadExcelToDatatable(string filepath)
        {
            DataTable dt = new DataTable();
            using(var stream =new FileStream(filepath,FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet == null|| sheet.PhysicalNumberOfRows==0) { return dt; }
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                HashSet<string> columnNames = new HashSet<string>();
                for (int i = 0; i < cellCount; i++) {
                    string columnName = headerRow.GetCell(i)?.ToString().Trim() ?? $"Column{i}";
                    int counter = 1;
                    string originalColumnName = columnName;
                    while (columnNames.Contains(originalColumnName)) {
                        columnName = $"{originalColumnName}_{counter}";
                        counter++;
                    }
                    columnNames.Add(columnName);
                    dt.Columns.Add(columnName);
                }
                ConcurrentBag<DataRow> rowsBag = new ConcurrentBag<DataRow>();
                Parallel.ForEach(Enumerable.Range(1, sheet.LastRowNum), rowIndex => { 
                    IRow excelRow = sheet.GetRow(rowIndex);
                    if (excelRow == null || excelRow.Cells.All(c=>c.CellType==CellType.Blank)) { return; }
                    DataRow dr=dt.NewRow();
                    for (int col = 0; col < cellCount; col++) { 
                        ICell cell=excelRow.GetCell(col);
                        dr[col]=GetCellValue(cell);
                    }
                    rowsBag.Add(dr);
                });
                foreach(var row in rowsBag) { dt.Rows.Add(row); }
            }
            return dt;
        }
        private object GetCellValue(ICell cell) { 
            if(cell == null) return null;
            switch (cell.CellType) { 
                case CellType.String:
                    return cell.StringCellValue.Trim();
                case CellType.Numeric:
                    return DateUtil.IsCellDateFormatted(cell)? cell.DateCellValue:(object)cell.NumericCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Formula:
                    return cell.CachedFormulaResultType==CellType.Numeric?(object)cell.NumericCellValue:cell.StringCellValue;
                default:
                    return DBNull.Value;
            }
        }
        private void UpdateRecord(DataTable dt)
        {
            int batchSize = 50;
            int UserID = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            //Parallel.ForEach(Partitioner.Create(0, dt.Rows.Count, batchSize), range => {
                using (var db = new CanaraClientDataContext())
                {
                    //for (int i = range.Item1; i <= range.Item2; i++) { 
                    for (int i = 0; i <= dt.Rows.Count; i++) { 
                        DataRow row = dt.Rows[i];
                        string AmpCode = row["Amfi Code"]?.ToString();
                        if (!string.IsNullOrEmpty(AmpCode))
                        {
                            var sd = db.T_CANARA_SCHEMES_STATIC_DATAs.Where(o => o.Amfi_Code == Convert.ToInt32(AmpCode)).Select(o => o.Scheme_ID).FirstOrDefault();
                            int SchemeID = sd;
                            DateTime? Aum_Date = null;
                            DateTime? Inception_Date = null;
                            if (DateTime.TryParse(row["Aum Date"].ToString(), out DateTime tempDate))
                            {
                                Aum_Date = tempDate;
                            }
                            if (DateTime.TryParse(row["Inception Date"].ToString(), out DateTime tempDate1))
                            {
                                Inception_Date = tempDate1;
                            }
                            decimal? tempAUM = null;
                            if (decimal.TryParse(row["AUM"]?.ToString().Trim(), out decimal result))
                            {
                                tempAUM = result;
                            }
                            decimal? tempPTRatio = null;
                            if (decimal.TryParse(row["PT Ratio"]?.ToString().Trim(), out decimal result1))
                            {
                                tempPTRatio = result1;
                            }
                            var _data = (from um in db.T_CANARA_SCHEMES_STATIC_DATAs where um.Scheme_ID == SchemeID && um.Amfi_Code == Convert.ToInt32(AmpCode) select um).SingleOrDefault();
                            if (_data != null) {
                                _data.Risk = row["Risk"].ToString();
                                _data.Benchmark_Risk = row["Benchmark Risk"].ToString();
                                _data.AUM = tempAUM;
                                _data.Aum_Date = Aum_Date;
                                _data.Inception_Date = Inception_Date;
                                _data.Horizon = row["Horizon"].ToString();
                                _data.Goal = row["Goal"].ToString();
                                _data.Benchmark = row["Benchmark"].ToString();
                                _data.Additional_Benchmark = row["Additional Benchmark"].ToString();
                                _data.About_Fund = row["About Fund"].ToString();
                                _data.Investment_Objective = row["Investment Objective"].ToString();
                                _data.Exit_Load = row["Exit Load"].ToString();
                                _data.Entry_Load = row["Entry Load"].ToString();
                                _data.Prescribed_Asset_Allocation = row["Prescribed Asset Allocation"].ToString();
                                _data.Suitable_For = row["Suitable For"].ToString();
                                _data.Expense_Ratio_Direct = row["Expense Ratio Direct"].ToString();
                                _data.Expense_Ratio_Regular = row["Expense Ratio Regular"].ToString();
                                _data.Product_Suitable_for = row["Product Suitable for"].ToString();
                                _data.Reason_To_Invest = row["Reason to invest"].ToString();
                                _data.Min_Amount_SIP = row["Min amountSIP"].ToString();
                                _data.Min_Amount_SWP = row["Min amountSWP"].ToString();
                                _data.Min_Amount_STP = row["Min amountSTP"].ToString();
                                _data.Min_Amount_Lumpsum = row["Min amountLumpsum"].ToString();
                                _data.Min_Amount_Redeem = row["Min amountRedeem"].ToString();
                                _data.PT_Ratio = tempPTRatio;
                                _data.Modified_On = DateTime.Now;
                                _data.Modified_By = UserID.ToString();
                            }
                            //dal.DeleteFundmanagerData(SchemeID);
                            List<FundManagerClass> fundManagerDataList = new List<FundManagerClass>();
                            string fundManagerNames = row["Fund Manager Name"]?.ToString();
                            string fromDates = row["From Date"]?.ToString();
                            string imageLinks = row["image link"]?.ToString();
                            string docLinks = row["Doc Link"]?.ToString();
                            if (!string.IsNullOrEmpty(fundManagerNames))
                            {
                                string[] fundManagerArray = fundManagerNames.Split('#');
                                string[] fromDatesArray = fromDates?.Split('#') ?? new string[0];
                                string[] imageLinksArray = imageLinks?.Split('#') ?? new string[0];
                                string[] docLinksArray = docLinks?.Split('#') ?? new string[0];

                                for (int z = 0; z < fundManagerArray.Length; z++)
                                {
                                    DateTime? FromDates = null;
                                    if (fromDatesArray.Length > z)
                                    {
                                        if (DateTime.TryParse(fromDatesArray[z].ToString(), out DateTime tempDate3))
                                        {
                                            FromDates = tempDate3;
                                        }
                                    }
                                    FundManagerClass data = new FundManagerClass
                                    {
                                        FundManagerNames = fundManagerArray[z].Trim(),
                                        //index = z + 1,
                                        FromDates = (z < fromDatesArray.Length && !string.IsNullOrEmpty(fromDatesArray[z])) ? FromDates : null,
                                        ImageLinks = z < imageLinksArray.Length && !string.IsNullOrEmpty(imageLinksArray[z]) ? imageLinksArray[z] : null,
                                        DocLinks = z < docLinksArray.Length && !string.IsNullOrEmpty(docLinksArray[z]) ? docLinksArray[z] : null
                                    };
                                    fundManagerDataList.Add(data);
                                }
                                foreach (FundManagerClass data in fundManagerDataList)
                                {
                                    var fundmng = new T_CANARA_SCHEMES_fundmanager
                                    {
                                        FundManagerName = data.FundManagerNames,
                                        From_Date = data.FromDates,
                                        Image_link = data.ImageLinks,
                                        Doc_Link = data.DocLinks,
                                        Scheme_id = SchemeID,
                                        Modified_By = UserID.ToString(),
                                        Modified_On = DateTime.Now,
                                        Inserted_By = UserID.ToString()
                                    };
                                    if (fundmng.FundManagerName != "" && fundmng.From_Date != null && fundmng.Image_link != null && fundmng.Doc_Link != null)
                                    {
                                        db.ExecuteCommand("INSERT INTO T_CANARA_SCHEMES_fundmanagers (FundManagerName, From_Date, Image_link, Doc_Link, Scheme_id, Modified_By, Modified_On,Inserted_By,Is_active) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6},{7},{8})",
                                                      fundmng.FundManagerName, fundmng.From_Date, fundmng.Image_link, fundmng.Doc_Link, fundmng.Scheme_id, fundmng.Modified_By, fundmng.Modified_On, fundmng.Inserted_By, 1);
                                    }

                                }
                                
                            }
                        }
                    }
                    db.SubmitChanges();
                }
                
            //});
        }
        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string path1 = HttpContext.Current.Server.MapPath("~/CanaraClient/ErrorLog/");
            if (!Directory.Exists(path1))
            {
                //Create the directory on the given path  
                Directory.CreateDirectory(path1);
            }
            string logFilePath = HttpContext.Current.Server.MapPath("~/CanaraClient/ErrorLog/");
            //string logFilePath = "E:\\Logs\\";
            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
            log.Close();

        }
    }
}