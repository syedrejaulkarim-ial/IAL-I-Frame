using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFrames.DAL;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;



namespace iFrames.BLL
{
    public static class DSPAppUpload
    {
        static string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        static SqlConnection conn = new SqlConnection();

        

        public static string BulkUpload(string SheetName, string tablename, DataTable Excelcol, DataTable Exceldt)
           {
               try
               {
                   string prevcolumnname = "";
                   string rptcolumn = "";

                   SqlBulkCopy objbulk = new SqlBulkCopy(conn);
                   objbulk.DestinationTableName = tablename;

                    for (int m = 0; m < Exceldt.Columns.Count; m++)
                    {
                        string columnName;
                        string dbcolumnName;

                        columnName = Exceldt.Columns[m].ColumnName;
                        dbcolumnName = Exceldt.Columns[m].ColumnName;

                        if (SheetName != "Summary" && SheetName != "Summary Direct")
                        {
                            if (Exceldt.Columns[m].ColumnName.Contains("Rank") == true)
                            {
                                Exceldt.Columns[m].ColumnName = "Rank " + prevcolumnname;
                                dbcolumnName = "Rank " + prevcolumnname;
                                columnName = "Rank " + prevcolumnname;
                            }
                            prevcolumnname = Exceldt.Columns[m].ColumnName;
                        }

                        if (iFrames.BLL.DSPAppUpload.CheckFieldsDB(columnName, Excelcol) == true)
                        {
                              objbulk.ColumnMappings.Add(columnName, dbcolumnName);

                              if (columnName != "Scheme Name" && columnName != "AUM" && columnName != "NAV" && columnName != "Table Detail" && columnName != "SHEET_NAME" && columnName != "Expense Ratio(Latest)")
                              {
                                  if (columnName.Contains("Rank") == false)
                                   {
                                      if (rptcolumn == "")
                                      {
                                          rptcolumn = columnName;
                                      }
                                      else
                                      {
                                          rptcolumn = rptcolumn + ", "+ columnName;
                                      }
                                   }
                             }
                        }
                        
                    }
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.ConnectionString = connstr;
                        conn.Open();
                    }
                   objbulk.WriteToServer(Exceldt);
                   conn.Close();

                   return rptcolumn;

               }
               catch (Exception ex)
               {
                   WriteLog("bulk upload");
                   WriteLog(ex.Message.ToString());
                   throw ex;
               }
            
             }



        public static bool UpdateToEventLogStatus(string sheetname, string filename, DateTime rptdate, DateTime date, string LoggedUserid, string tablename,string rptcolumn)
        {
            try
            {

                using (var dc = new DSPAppDataContext() { CommandTimeout = 6000 })
                {

                    if(tablename == "T_DSP_RETURN_ANALYSIS_DETAIL")
                    {
                  
                        dc.T_DSP_RETURN_ANALYSIS_DETAILs
                        .Where(x => x.FILE_NAME == null)
                        .ToList()
                        .ForEach(a =>{
                            
                            a.DATE = date ;
                            a.FILE_NAME = filename;
                            a.SHEET_NAME = sheetname;
                            a.Column_Name = rptcolumn;
                            a.Report_Date = rptdate;
                            a.Loggeduserid = LoggedUserid;
                            a.Timest = DateTime.Now;
                            }
                            );
                        dc.SubmitChanges();

                    }
                    else
                    {
                   
                        dc.T_DSP_RETURN_ANALYSIS_SUMMARies
                       .Where(x => x.SHEET_NAME == null)
                       .ToList()
                       .ForEach(a =>
                       {

                           a.DATE = date;
                           a.FILE_NAME = filename;
                           a.SHEET_NAME = sheetname;
                           a.Report_Date = rptdate;
                           a.Loggeduserid = LoggedUserid;
                           a.Timest = DateTime.Now;
                       }
                           );
                        dc.SubmitChanges();
                    }                   
              
                }
                return true;
            }
            catch (Exception ex)
            {

                WriteLog("update status");
                WriteLog(ex.Message.ToString());
                return false;
            }
        }


        public static bool CheckFieldsDB(string Columnname, DataTable ds2)
        {

            string colname = "";
            for (int j = 0; j < ds2.Rows.Count; j++)
            {
                colname = ds2.Rows[j]["column_name"].ToString();

                if (colname == Columnname)
                {
                    return true;
                }
            }
            return false;
        }



        public static string ExtractDatesFromFileNames(string fileName)        {

            string date =  fileName.Substring(fileName.IndexOf("_") + 1, 10);
           
            return date.Replace("-", "/");
        }


        public static void DelExistRecords(string tablename,DateTime date,string sheetname)
        {
            try
            {
                using (var dc = new DSPAppDataContext() { CommandTimeout = 6000 })
                {
                    if (tablename == "T_DSP_RETURN_ANALYSIS_DETAIL")
                    {
                        var del = dc.T_DSP_RETURN_ANALYSIS_DETAILs.Where(c => c.Report_Date == date && c.SHEET_NAME == sheetname);

                        dc.T_DSP_RETURN_ANALYSIS_DETAILs.DeleteAllOnSubmit(del);
                        dc.SubmitChanges();
                    }

                    else
                    {
                        var del = dc.T_DSP_RETURN_ANALYSIS_SUMMARies.Where(c => c.Report_Date == date && c.SHEET_NAME == sheetname);

                        dc.T_DSP_RETURN_ANALYSIS_SUMMARies.DeleteAllOnSubmit(del);
                        dc.SubmitChanges();
                    }

                }
               
            }
            catch (Exception ex)
            {
                WriteLog("delete exist record");
                WriteLog(ex.Message.ToString());
                throw ex;
            }

        }

        public static DataTable ColumnExist(string tablename)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }
                var cmd = conn.CreateCommand();
                //cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns where TABLE_NAME = '" + tablename + "'";

                cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns where TABLE_NAME = @tablename";
                cmd.Parameters.AddWithValue("@tablename", tablename);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable Excelcol = ds.Tables[0];
                conn.Close();

                return Excelcol;
               
            }
            catch (Exception ex)
            {
                WriteLog("exist column");
                WriteLog(ex.Message.ToString());
                throw ex;
            }
            
        }
        


        public static DateTime Formatfile(string path)
        {
            try
            {
               
               
                Excel.Application excelApp = new Excel.Application();  // Creates a new Excel Application
                // excelApp.Visible = true;  // Makes Excel visible to the user.           
                // The following code opens an existing workbook
                DateTime rptdate = Convert.ToDateTime("01/01/1900", System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                string flag = "N";
                string workbookPath = path;
              //  string SaveAsFileName = path.Replace(".xls", "_1.xls");
                Excel.Workbook excelWorkbook = null;
              

                try
                {
                    excelWorkbook = excelApp.Workbooks.Open(workbookPath, 0,
                    false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true,
                    false, 0, true, false, false);
                }
                catch
                {
                    //Create a new workbook if the existing workbook failed to open.
                    excelWorkbook = excelApp.Workbooks.Add();
                }
                // The following gets the Worksheets collection

               
                Excel.Sheets excelSheets = excelWorkbook.Worksheets;
              

                for (int k = 1; k < excelSheets.Count + 1; k++)
                {

                    Excel.Worksheet worksheet = (Excel.Worksheet)excelSheets.get_Item(k);
                  
                    string strWorksheetName = worksheet.Name;
                    Excel.Range range;
                    if (strWorksheetName == "Summary")
                    {
                     
                        range = worksheet.get_Range("A1", "A2");
                        range.EntireRow.Delete(Excel.XlDirection.xlUp);
                     
                        worksheet.Cells[1, 1] = "CATEGORY";
                        Excel.Range xlRange = worksheet.UsedRange;
                   
                        xlRange.Replace("%", "/");
                        xlRange.Replace("--", "0");
                        xlRange.NumberFormat = "@";
                        excelWorkbook.Save();
                      //  excelWorkbook.SaveAs(SaveAsFileName);
                      
                    }

                    else
                    {

                        if (strWorksheetName != "Relative Performance")
                        {
                      
                            string flag1 = "N";

                            if (flag == "N")
                            {
                                rptdate =  (DateTime)(worksheet.Cells[2, 2] as Excel.Range).Value;
                                flag = "Y";
                            }
                            range = worksheet.get_Range("A1", "A4");
                            range.EntireRow.Delete(Excel.XlDirection.xlUp);
                            excelWorkbook.Save();
                          //  excelWorkbook.SaveAs(SaveAsFileName);
                         
                            Excel.Range xlRange = worksheet.UsedRange;
                            xlRange.Replace("%", "/");
                            xlRange.Replace("--", "0");
                            xlRange.NumberFormat = "@";
                            int tcol = xlRange.Columns.Count;
                            int trow = xlRange.Rows.Count;
                            int srow = 0;
                            worksheet.Cells[1, tcol + 1] = "Table Detail";
                            worksheet.Cells[1, tcol + 2] = "SHEET_NAME";
                        
                            for (int n = 2; n < trow; n++)
                            {
                                string val = (string)(worksheet.Cells[n, 1] as Excel.Range).Value;
                              
                                if (val == null)
                                {

                                    if (flag1 == "Y")
                                    {
                                        worksheet.Cells[n, tcol + 1] = "Others";
                                        worksheet.Cells[n, tcol + 2] = strWorksheetName;
                                    }
                                    if (flag1 == "N")
                                    {
                                        (worksheet.Cells[n + 1, 2] as Excel.Range).NumberFormat = "dd/mmm/yy";
                                        (worksheet.Cells[n + 1, 3] as Excel.Range).NumberFormat = "dd/mmm/yy";
                                        flag1 = "Y";
                                    }
                                 

                                }
                                else
                                {
                                    if (flag1 == "N")
                                    {
                                        worksheet.Cells[n, tcol + 1] = "Main";
                                        worksheet.Cells[n, tcol + 2] = strWorksheetName;
                                    }
                                    else
                                    {
                                        worksheet.Cells[n, tcol + 1] = "Others";
                                        worksheet.Cells[n, tcol + 2] = strWorksheetName;
                                    }
                                    if (val.Contains("Note:") == true)
                                    {
                                        srow = n;
                                        break;
                                    }
                                }

                            }

                          
                            range = worksheet.get_Range("A" + srow, "A" + trow);
                            range.EntireRow.Delete(Excel.XlDirection.xlUp);
                        
                            excelWorkbook.Save();
                           // excelWorkbook.SaveAs(SaveAsFileName);
                                                WriteLog("13");
                        }

                    }

                }
             
                excelWorkbook.Save();
                //excelWorkbook.SaveAs(SaveAsFileName);
                //System.IO.File.Move("SaveAsFileName", "newfilename");
                excelWorkbook.Close(false);
                excelApp.Application.Quit();
            
                return rptdate;

            }
            catch (Exception ex)
            {
                WriteLog("file format");
                WriteLog(ex.Message.ToString());
                WriteLog(ex.ToString());
                return Convert.ToDateTime("01/01/1900", System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            }

        }


        public static DataTable GetCategory()
        {
            try
            {
                using (var dc = new DSPAppDataContext() { CommandTimeout = 6000 })
                {
                    return
                        (from a in dc.T_DSP_RETURN_ANALYSIS_DETAILs
                        .Where(x => x.SHEET_NAME != "" )
                         orderby
                           a.SHEET_NAME
                         select new
                         {
                             a.SHEET_NAME

                         }).Distinct().OrderBy(a => a.SHEET_NAME).ToDataTable();
                }
            }
            catch (Exception)
            {

                return null;
            }
        }


        public static string GetDate()
        {
            try
            {
                using (var dc = new DSPAppDataContext() { CommandTimeout = 6000 })
                {
                    
                    var MaxDate = (from d in dc.T_DSP_RETURN_ANALYSIS_DETAILs select d.Report_Date).Max();

                    return Convert.ToDateTime(MaxDate).ToString("dd MMM yyyy");
                }
            }
            catch (Exception)
            {

                return GetDate();
            }
        }


        public static string GetPeriod(string sheetname, DateTime rptdate)
        {
            try
            {
                string sqlDate = rptdate.Date.ToString("yyyy-MM-dd");

                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }
                var cmd = conn.CreateCommand();
                sheetname = "Liquid Fund";
                //cmd.CommandText = "SELECT DISTINCT COLUMN_NAME FROM T_DSP_RETURN_ANALYSIS_DETAIL where SHEET_NAME = '" + sheetname + "'  and Report_Date = '" + sqlDate + "'";
                cmd.CommandText = "SELECT DISTINCT COLUMN_NAME FROM T_DSP_RETURN_ANALYSIS_DETAIL where SHEET_NAME = @sheetname  and Report_Date = @sqlDate";

                cmd.Parameters.AddWithValue("@sheetname", sheetname);
                //cmd.Parameters["@sheetname"].Value = sheetname;

                cmd.Parameters.AddWithValue("@sqlDate", sqlDate);
                //cmd.Parameters["@sqlDate"].Value = sqlDate;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                string Excelcol = ds.Tables[0].Rows[0][0].ToString();
                conn.Close();

                return Excelcol;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DataTable GetRecards(string columnname,string sheetname,DateTime rptdate)
        {
            try
            {

                string sqlDate = rptdate.Date.ToString("yyyy-MM-dd");


                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }
               
                var cmd = conn.CreateCommand();
                //cmd.CommandText = "SELECT " + columnname + " FROM T_DSP_RETURN_ANALYSIS_DETAIL A left join T_SCHEMES_MASTER B on A.[Scheme Name] = b.Sch_Short_Name  where SHEET_NAME = '" + sheetname + "' AND [Table Detail] = 'Main'  and Report_Date = '" + sqlDate + "'  order by event_id";
                

                cmd.CommandText = "SELECT " + columnname + " FROM T_DSP_RETURN_ANALYSIS_DETAIL A left join T_SCHEMES_MASTER B on A.[Scheme Name] = b.Sch_Short_Name  where SHEET_NAME = @sheetname AND [Table Detail] = @Main  and Report_Date = @sqlDate  order by event_id";
                cmd.Parameters.AddWithValue("@sheetname", sheetname);
                cmd.Parameters.AddWithValue("@Main", "Main");
                cmd.Parameters.AddWithValue("@sqlDate", sqlDate);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable Excelcol = ds.Tables[0];

                conn.Close();

                return Excelcol;


            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static DataTable GetRecards1(string columnname, string sheetname, DateTime rptdate)
        {
            try
            {
                string sqlDate = rptdate.Date.ToString("yyyy-MM-dd");
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }

                //rptdate = format( rptdate);
                var cmd = conn.CreateCommand();

                //cmd.CommandText = "SELECT [SCHEME NAME] ,AUM ,[Expense Ratio(Latest)]  FROM T_DSP_RETURN_ANALYSIS_DETAIL where SHEET_NAME = '" + sheetname + "'  AND [Table Detail] = 'Others'  and Report_Date = '" + sqlDate + "'  order by event_id";

                cmd.CommandText = "SELECT [SCHEME NAME] ,AUM ,[Expense Ratio(Latest)]  FROM T_DSP_RETURN_ANALYSIS_DETAIL where SHEET_NAME = @sheetname AND [Table Detail] = @Others  and Report_Date = @sqlDate order by event_id";
                cmd.Parameters.AddWithValue("@sheetname", sheetname);
                cmd.Parameters.AddWithValue("@Others", "Others");
                cmd.Parameters.AddWithValue("@sqlDate", sqlDate);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
               
                DataTable Excelcol = ds.Tables[0];

                conn.Close();

                return Excelcol;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static DataTable GetSummary(string sheetname, DateTime rptdate)
        {
            try
            {
                string sqlDate = rptdate.Date.ToString("yyyy-MM-dd");
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }

                //rptdate = format( rptdate);
                var cmd = conn.CreateCommand();
                //cmd.CommandText = " SELECT  '' HIDECOL,Category,Fund,[Quartile Positioning], CASE WHEN [Fund Positioning] = '0' THEN '--' ELSE [Fund Positioning] END [Fund Positioning] ,CASE WHEN [Compound Annualized Returns] = '0' THEN '--' ELSE cast(cast(Replace([Compound Annualized Returns],'/','') as numeric(38,2)) AS varchar(100))+'%' END [Compound Annualized Returns] ,CASE WHEN [Sector Average] = '0' THEN '--' ELSE cast(cast(Replace([Sector Average],'/','') as numeric(38,2)) AS varchar(100))+'%' END [Sector Average] ,CASE WHEN [Lowest in Top Quartile] = '0' THEN '--' ELSE cast(cast(Replace([Lowest in Top Quartile],'/','') as numeric(38,2)) AS varchar(100))+'%' END [Lowest in Top Quartile] ,Rank,[No# of Funds], CASE WHEN [Alpha] = '0' THEN '--' ELSE cast(cast(Replace([Alpha],'/','') as numeric(38,2)) AS varchar(100))+'%' END [Alpha] from T_DSP_RETURN_ANALYSIS_SUMMARY where Report_Date = '" + sqlDate + "' and sheet_name = '" + sheetname + "'  order by event_id";
                cmd.CommandText = " SELECT  '' HIDECOL,Category,Fund,[Quartile Positioning], CASE WHEN [Fund Positioning] = '0' THEN '--' ELSE [Fund Positioning] END [Fund Positioning] ,CASE WHEN [Compound Annualized Returns] = '0' THEN '--' ELSE cast(cast(Replace([Compound Annualized Returns],'/','') as numeric(38,2)) AS varchar(100))+'%' END [Compound Annualized Returns] ,CASE WHEN [Sector Average] = '0' THEN '--' ELSE cast(cast(Replace([Sector Average],'/','') as numeric(38,2)) AS varchar(100))+'%' END [Sector Average] ,CASE WHEN [Lowest in Top Quartile] = '0' THEN '--' ELSE cast(cast(Replace([Lowest in Top Quartile],'/','') as numeric(38,2)) AS varchar(100))+'%' END [Lowest in Top Quartile] ,Rank,[No# of Funds], CASE WHEN [Alpha] = '0' THEN '--' ELSE cast(cast(Replace([Alpha],'/','') as numeric(38,2)) AS varchar(100))+'%' END [Alpha] from T_DSP_RETURN_ANALYSIS_SUMMARY where Report_Date =@sqlDate and sheet_name = @sheetname order by event_id";
                cmd.Parameters.AddWithValue("@sqlDate", sqlDate);
                cmd.Parameters.AddWithValue("@sheetname", sheetname);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable Excelcol = ds.Tables[0];

                conn.Close();

                return Excelcol;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string logFilePath = HttpContext.Current.Server.MapPath("~/DSPApp/ErrorLog/");
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