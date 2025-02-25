using System;
using System.Collections.Generic;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Windows;


namespace iFrames
{
    /// <summary>
    /// This Class Is Used To Handel Excel Activities
    /// </summary>
    public class ExcelWrite
    {
        protected Microsoft.Office.Interop.Excel._Application _ExcelApplication;
        protected Workbook _ExcelWorkBook;
        public Workbook _ExcelWorkbookTemp;
        protected Worksheet _ExcelWorkSheet;
        string _ExcelFilePath;
        bool _VisibleOrNot = true;
        bool _DisplayAlerts = false;
        bool _ReadOnlyOrNot = false;
        ArrayList _SheetNames = null;
        /// <summary>
        /// </summary>
        public ExcelWrite()
        {
        }
        /// <summary>
        /// This Constractor Takes An Excel File Name To Open, If The File Does Not Exists It Creates The File
        /// </summary>
        /// <param name="ExcelFilePath">Excel File Name</param>
        public ExcelWrite(string ExcelFilePath)
        {
            _ExcelFilePath = ExcelFilePath;
            try
            {
                if (!File.Exists(ExcelFilePath) && ExcelFilePath != "")
                {
                    //using (FileStream file = File.Create(ExcelFilePath)) ;
                    Excel.Application oXL = new Excel.Application();
                    oXL.Visible = false;
                    Workbook oWB = (Workbook)(oXL.Workbooks.Add(Missing.Value));
                    //if(ExcelFilePath.ToString().Trim().ToUpper().EndsWith(".XLSX"))
                    //    oWB.SaveAs(_ExcelFilePath, Excel.XlFileFormat.xlOpenXMLWorkbook, null, null, false, false, Excel.XlSaveAsAccessMode.xlNoChange, false, false, null, null, null);
                    //else
                        oWB.SaveAs(_ExcelFilePath, Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Excel.XlSaveAsAccessMode.xlNoChange, false, false, null, null, null);
                    oWB.Close(0, null, null);
                    oXL.Quit();
                }
            }
            catch (Exception ex) { }
        }
        
        /// <summary>
        /// Gets Or Sets The Visibility Property Of Excel, By Default False
        /// </summary>
        [DefaultValue(false)]
        public bool IsVisible
        {
            get
            {
                return _VisibleOrNot;
            }
            set
            {
                _VisibleOrNot = value;
            }
        }

        /// <summary>
        /// Gets Or Sets The DisplayAlert Property Of Excel, By Default False
        /// </summary>
        [DefaultValue(false)]
        public bool ShowNotification
        {
            get
            {
                return _DisplayAlerts;
            }
            set
            {
                _DisplayAlerts = value;
            }
        }

        /// <summary>
        /// Gets Or Sets The ReadOnly Property Of The Excel File, By Default True. And If The Property Is True You Can Not Save The File After Any Modification
        /// </summary>
        [DefaultValue(true)]
        public bool IsReadOnly
        {
            get
            {
                return _ReadOnlyOrNot;
            }
            set
            {
                _ReadOnlyOrNot = value;
            }
        }

        /// <summary>
        /// Opens The Specified Excel [Readonly Or Not Acc As The IsReadOnly Property Set Before Opening It]
        /// </summary>
        /// <returns>If Suceeds True Else False</returns>
        public bool OpenExcel()
        {
            try
            {
                _ExcelApplication = new Excel.Application();
                _ExcelApplication.Visible = _VisibleOrNot;
                _ExcelApplication.DisplayAlerts = _DisplayAlerts;
                _ExcelWorkBook = _ExcelApplication.Workbooks.Open(_ExcelFilePath, 1, _ReadOnlyOrNot,5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, Missing.Value, false);
                _ExcelWorkbookTemp = _ExcelWorkBook;
                _SheetNames = new ArrayList();

                foreach (var wsWorksheetName in _ExcelWorkBook.Sheets)
                {
                    if (wsWorksheetName is Excel.Worksheet)
                    {
                        //wsWorksheetName.Visible = XlSheetVisibility.xlSheetVisible;
                        _SheetNames.Add(((Worksheet)wsWorksheetName).Name.ToString().ToUpper());
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Opens New Workbook Inside The Previously Opened Excel Object, This Workbook Object Is Not Stored In Memory
        /// </summary>
        /// <param name="NewExcelFilePath">New Excel File Path</param>
        /// <returns>True Or False</returns>
        public bool OpenNewWorkbook(string NewExcelFilePath)
        {
            try
            {
                _ExcelApplication.Workbooks.Open(NewExcelFilePath, 1, _ReadOnlyOrNot, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, Missing.Value, false);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets The Sheet Names Of The Excel Opened, All In Full Caps
        /// </summary>
        public ArrayList SheetNames
        {
            get
            {
                return _SheetNames;
            }
        }

        /// <summary>
        /// Returns The Used Row Count Of A Particular Sheet Of The Excel
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <returns>UsedRowCount As Integer</returns>
        public int GetUsedRowCount(string SheetName)
        {
            _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
            return _ExcelWorkSheet.UsedRange.Rows.Count;
        }

        /// <summary>
        /// Returns The Used Column Count Of A Particular Sheet Of The Excel
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <returns>UsedColumnCount As Integer</returns>
        public int GetUsedColumnCount(string SheetName)
        {
            _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
            return _ExcelWorkSheet.UsedRange.Columns.Count;
        }

        /// <summary>
        /// Saves The Excel As A New Workbook
        /// </summary>
        /// <param name="FileName">Workbook Name To Save As</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SaveExcelAs(string FileName)
        {
            try
            {
                string ExcelVersion = _ExcelApplication.Version;
                //if (ExcelVersion=="12.0")
                //    _ExcelWorkBook.CheckCompatibility = false;
                _ExcelWorkBook.SaveAs(FileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Saves The Excel As A New Workbook
        /// </summary>
        /// <param name="FileName">Workbook Name To Save As</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool Save()
        {
            try
            {
                string ExcelVersion = _ExcelApplication.Version;
                //if (ExcelVersion == "12.0")
                //    _ExcelWorkBook.CheckCompatibility = false;
                _ExcelWorkBook.Save();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SaveWithPassword(string SheetName, string Pwd)
        {
            try
            {
                //_ExcelWorkBook.Password = Pwd;
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Protect(Pwd, true, true, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //_ExcelApplication.ActiveWorkbook.Protect(Pwd, true,Type.Missing);
                _ExcelWorkBook.Save();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Save Any DataTable As Excel
        /// </summary>
        /// <param name="ExcelFilePath">Full FileName To Save As</param>
        /// <param name="InputDataSet">DataTable Provided</param>
        /// <returns>True If Suceeds</returns>
        public static bool SaveDataTableAsExcel(string ExcelFilePath, System.Data.DataTable InputDataTable)
        {
            try
            {
                if (InputDataTable == null)
                    return false;
                System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
                grid.HeaderStyle.Font.Bold = true;
                grid.DataSource = InputDataTable;
                grid.DataMember = InputDataTable.TableName;
                grid.DataBind();
                using (StreamWriter sw = new StreamWriter(@ExcelFilePath.Insert(@ExcelFilePath.LastIndexOf('.'), "_Temp")))
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    grid.RenderControl(hw);
                Excel.Application ExcelApplication = new Excel.Application();
                //ExcelApplication.Visible = true;
                Workbook ExcelWorkBook = ExcelApplication.Workbooks.Open(@ExcelFilePath.Insert(@ExcelFilePath.LastIndexOf('.'), "_Temp"), 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, Missing.Value, false);
                File.Delete(@ExcelFilePath);
                ExcelWorkBook.SaveAs(@ExcelFilePath, XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                ExcelWorkBook.Close(0, null, null);
                ExcelApplication.Quit();
                File.Delete(@ExcelFilePath.Insert(@ExcelFilePath.LastIndexOf('.'), "_Temp"));
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool CopyWorkSheet(string SourceSheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SourceSheetName];
                _ExcelWorkSheet.Copy(_ExcelWorkBook.Worksheets[1], Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Save Any DataSet As Excel
        /// </summary>
        /// <param name="ExcelFilePath">Full FileName To Save As</param>
        /// <param name="InputDataSet">DataSet Provided</param>
        /// <returns>True If Suceeds</returns>
        public static bool SaveDataSetAsExcel(string ExcelFilePath, System.Data.DataSet InputDataSet)
        {
            try
            {
                for (int i = 0; i < InputDataSet.Tables.Count; i++)
                {
                    System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
                    grid.HeaderStyle.Font.Bold = true;
                    grid.DataSource = InputDataSet.Tables[i];
                    grid.DataMember = InputDataSet.Tables[i].TableName;
                    grid.DataBind();
                    using (StreamWriter sw = new StreamWriter(@ExcelFilePath.Insert(@ExcelFilePath.LastIndexOf('.'), InputDataSet.Tables[i].TableName + "_Temp")))
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        grid.RenderControl(hw);
                }

                Excel.Application oXL = new Excel.Application();
                oXL.Visible = false;
                Workbook oWB = (Workbook)(oXL.Workbooks.Add(Missing.Value));
                oWB.SaveAs(ExcelFilePath.TrimEnd('x') + "x", Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Excel.XlSaveAsAccessMode.xlNoChange, false, false, null, null, null);
                oWB.Close(0, null, null);
                //////oXL.Quit();

                //////oXL = new Excel.Application();
                //oXL.Visible = true;
                oWB = oXL.Workbooks.Open(ExcelFilePath.TrimEnd('x') + "x", 0, false, 1, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, Missing.Value, false);
                Excel.Workbook wb;
                Excel.Worksheet ws;
                int wsi = 3;
                for (int i = 0; i < InputDataSet.Tables.Count; i++)
                {
                    string _FileName = @ExcelFilePath.Insert(@ExcelFilePath.LastIndexOf('.'), InputDataSet.Tables[i].TableName + "_Temp");
                    wb = oXL.Workbooks.Open(_FileName, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, Missing.Value, false);
                    ws = (Excel.Worksheet)wb.Sheets[1];
                    ws.Move(Missing.Value, oWB.Worksheets[wsi++]);
                    ws = (Worksheet)oWB.Worksheets[wsi];
                    ws.Name = InputDataSet.Tables[i].TableName;
                    File.Delete(ExcelFilePath.Insert(@ExcelFilePath.LastIndexOf('.'), InputDataSet.Tables[i].TableName + "_Temp"));
                }
                for (int k = 3; k >= 1; k--) // Delete First 3 Sheets
                {
                    ws = (Worksheet)oWB.Worksheets[k];
                    ws.Delete();
                }
                oWB.Save();
                oWB.SaveAs(ExcelFilePath.TrimEnd('x'), Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Excel.XlSaveAsAccessMode.xlNoChange, false, false, null, null, null);
                oWB.Close(0, null, null);
                oXL.Quit();
                //File.Copy(ExcelFilePath.TrimEnd('x') + "x", ExcelFilePath.TrimEnd('x'));
                //File.Copy(ExcelFilePath.TrimEnd('x'), ExcelFilePath.TrimEnd('x') + "x", true);
                ////File.Delete(ExcelFilePath.TrimEnd('x'));
                File.Delete(ExcelFilePath.TrimEnd('x') + "x");


                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get Data From A Range
        /// </summary>
        /// <param name="StartCellRowIndex">StartCellRowIndex</param>
        /// <param name="StartCellColumnIndex">StartCellColumnIndex</param>
        /// <param name="EndCellRowIndex">EndCellRowIndex</param>
        /// <param name="EndCellColumnIndex">EndCellColumnIndex</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>Tow Dimentional Array Object</returns>
        public object[,] FetchRangeData(int StartCellRowIndex, int StartCellColumnIndex, int EndCellRowIndex, int EndCellColumnIndex, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                return (object[,])_ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartCellRowIndex, StartCellColumnIndex], _ExcelWorkSheet.Cells[EndCellRowIndex, EndCellColumnIndex]).Value2;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Data From A Range
        /// </summary>
        /// <param name="StartCellAddress">StartCellAddress</param>
        /// <param name="EndCellAddress">EndCellAddress</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>Tow Dimentional Array Object</returns>
        public object[,] FetchRangeData(string StartCellAddress, string EndCellAddress, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                return (object[,])_ExcelWorkSheet.get_Range(StartCellAddress, EndCellAddress).Value2;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Data From A Cell
        /// </summary>
        /// <param name="CellAddress">CellAddress</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>String Value</returns>
        public string FetchCellData(string CellAddress, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                if (_ExcelWorkSheet.get_Range(CellAddress, CellAddress).Value2 != null)
                    return _ExcelWorkSheet.get_Range(CellAddress, CellAddress).Value2.ToString().Trim();
                else
                    return null;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        public string FetchCellDatas(string CellAddress, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                if (_ExcelWorkSheet.Range[CellAddress, CellAddress].Value2 != null)
                    return _ExcelWorkSheet.Range[CellAddress, CellAddress].Value2.ToString().Trim();
                else
                    return null;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Formula From A Cell
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="column">column</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>String Formula</returns>
        public string FetchCellFormula(int row, int column, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                if (_ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[row, column], _ExcelWorkSheet.Cells[row, column]).Formula != null)
                    return _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[row, column], _ExcelWorkSheet.Cells[row, column]).Formula.ToString().Trim();
                else
                    return null;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Fetch A Single Column Data
        /// </summary>
        /// <param name="StartCellRowIndex">StartCellRowIndex</param>
        /// <param name="EndCellRowIndex">EndCellRowIndex</param>
        /// <param name="ColumnIndex">ColumnIndex</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>ArrayList</returns>
        public ArrayList FetchColumnData(int StartCellRowIndex, int EndCellRowIndex, int ColumnIndex, string SheetName)
        {
            ArrayList alTemp = new ArrayList();
            if (StartCellRowIndex == EndCellRowIndex)
            {
                alTemp.Add(_ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartCellRowIndex, ColumnIndex], _ExcelWorkSheet.Cells[EndCellRowIndex, ColumnIndex]).Value2);
                return alTemp;
            }
            object[,] objData = FetchRangeData(StartCellRowIndex, ColumnIndex, EndCellRowIndex, ColumnIndex, SheetName);
            for (int i = 1; i <= objData.GetLength(0); i++)
                alTemp.Add(objData[i, 1]);
            return alTemp;
        }

        /// <summary>
        /// Fetch A Single Column Data
        /// </summary>
        /// <param name="StartCellRowIndex">StartCellRowIndex</param>
        /// <param name="EndCellRowIndex">EndCellRowIndex</param>
        /// <param name="ColumnName">ColumnName</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>ArrayList</returns>
        public ArrayList FetchColumnData(int StartCellRowIndex, int EndCellRowIndex, string ColumnName, string SheetName)
        {
            ArrayList alTemp = new ArrayList();
            if (StartCellRowIndex == EndCellRowIndex)
            {
                alTemp.Add(FetchCellData(ColumnName + StartCellRowIndex.ToString(), SheetName));
                return alTemp;
            }
            object[,] objData = FetchRangeData(ColumnName + StartCellRowIndex.ToString(), ColumnName + EndCellRowIndex.ToString(), SheetName);
            for (int i = 1; i <= objData.GetLength(0); i++)
                alTemp.Add(objData[i, 1]);
            return alTemp;
        }

        /// <summary>
        /// Get Data From A Cell
        /// </summary>
        /// <param name="CellRowIndex">CellRowIndex</param>
        /// <param name="CellColumnIndex">CellColumnIndex</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>String Value</returns>
        public string FetchCellData(int CellRowIndex, int CellColumnIndex, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                if (_ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[CellRowIndex, CellColumnIndex], _ExcelWorkSheet.Cells[CellRowIndex, CellColumnIndex]).Value2 != null)
                    return _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[CellRowIndex, CellColumnIndex], _ExcelWorkSheet.Cells[CellRowIndex, CellColumnIndex]).Value2.ToString().Trim();
                else
                    return "";
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Fetch Sheet As DataTable
        /// </summary>
        /// <param name="SheetName">SheetName</param>
        /// <returns>A DataTable</returns>
        public System.Data.DataTable FetchSheet(string SheetName)
        {
            try
            {
                string sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + _ExcelFilePath + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                OleDbConnection objConn = new OleDbConnection(sConnectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + SheetName + "$]", objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                System.Data.DataTable dtReturned = new System.Data.DataTable();
                objAdapter.Fill(dtReturned);
                objConn.Close();
                return dtReturned;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Fetch Sheet As DataTable
        /// </summary>
        /// <param name="SheetName">SheetName</param>
        /// <param name="NonBlankColumnIndex">Non-Blank ColumnIndex e.g 1 For Col. A, 2 For Col. B</param>
        /// <returns>A DataTable</returns>
        public System.Data.DataTable FetchSheet(string SheetName, int NonBlankColumnIndex)
        {
            try
            {
                string sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + _ExcelFilePath + ";" + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
                OleDbConnection objConn = new OleDbConnection(sConnectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + SheetName + "$]", objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                System.Data.DataTable dtReturned = new System.Data.DataTable();
                objAdapter.Fill(dtReturned);
                objConn.Close();
                System.Data.DataTable dtReturned_1 = dtReturned.Clone();

                DataRow[] drs = dtReturned.Select("[" + dtReturned.Columns[NonBlankColumnIndex - 1].ColumnName + "] is not null And len('" + dtReturned.Columns[NonBlankColumnIndex - 1].ColumnName.ToString().Trim() + "') <> 0");
                foreach (DataRow dr in drs)
                    dtReturned_1.ImportRow(dr);

                return dtReturned_1;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Adds A WorkSheet At The Beginning Of The Workbook
        /// </summary>
        /// <param name="SheetName">Name Of The Sheet To Add</param>
        public void AddWorkSheet(string SheetName)
        {
            try
            {
                _ExcelWorkBook.Worksheets.Add(_ExcelWorkBook.Worksheets[1], Type.Missing, Type.Missing, Type.Missing);
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[1];
                _ExcelWorkSheet.Name = SheetName;
                _SheetNames = new ArrayList();
                foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                    _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Adds A WorkSheet At The end Of The Workbook
        /// </summary>
        /// <param name="SheetName">Name Of The Sheet To Add</param>
        public void AddWorkSheet(string SheetName, bool isEnd)
        {
            try
            {
                if (isEnd)
                {
                    _ExcelWorkBook.Worksheets.Add(Type.Missing, _ExcelWorkBook.Worksheets[_ExcelWorkBook.Worksheets.Count], Type.Missing, Type.Missing);
                    _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[_ExcelWorkBook.Worksheets.Count];
                    _ExcelWorkSheet.Name = SheetName;
                    _SheetNames = new ArrayList();
                    foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                        _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
                }
                else
                {
                    _ExcelWorkBook.Worksheets.Add(_ExcelWorkBook.Worksheets[1], Type.Missing, Type.Missing, Type.Missing);
                    _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[1];
                    _ExcelWorkSheet.Name = SheetName;
                    _SheetNames = new ArrayList();
                    foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                        _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }


        /// Adds A WorkSheet At The end Of The Workbook with ZOOM without Page fit
        /// </summary>
        /// <param name="SheetName">Name Of The Sheet To Add</param>
        public void AddWorkSheet(string SheetName, bool isEnd, int ZOOM, bool PagebreakPreview)
        {
            try
            {
                if (isEnd)
                {
                    _ExcelWorkBook.Worksheets.Add(Type.Missing, _ExcelWorkBook.Worksheets[_ExcelWorkBook.Worksheets.Count], Type.Missing, Type.Missing);
                    _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[_ExcelWorkBook.Worksheets.Count];
                    _ExcelWorkSheet.Name = SheetName;
                    _ExcelWorkSheet.Application.ActiveWindow.Zoom = ZOOM;
                    if (PagebreakPreview)
                        _ExcelWorkSheet.Application.ActiveWindow.View = Excel.XlWindowView.xlPageBreakPreview;
                    _SheetNames = new ArrayList();
                    foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                        _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
                }
                else
                {
                    _ExcelWorkBook.Worksheets.Add(_ExcelWorkBook.Worksheets[1], Type.Missing, Type.Missing, Type.Missing);
                    _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[1];
                    _ExcelWorkSheet.Name = SheetName;
                    _ExcelWorkSheet.Application.ActiveWindow.Zoom = ZOOM;
                    if (PagebreakPreview)
                        _ExcelWorkSheet.Application.ActiveWindow.View = Excel.XlWindowView.xlPageBreakPreview;
                    _SheetNames = new ArrayList();
                    foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                        _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }



        /// Adds A WorkSheet At The end Of The Workbook with ZOOM without Page fit
        /// </summary>
        /// <param name="SheetName">Name Of The Sheet To Add</param>
        public void AddWorkSheet(string SheetName, bool isEnd, int ZOOM, bool PagebreakPreview, bool Landscape, bool FittoOnePage)
        {
            try
            {
                if (isEnd)
                {
                    _ExcelWorkBook.Worksheets.Add(Type.Missing, _ExcelWorkBook.Worksheets[_ExcelWorkBook.Worksheets.Count], Type.Missing, Type.Missing);
                    _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[_ExcelWorkBook.Worksheets.Count];
                    _ExcelWorkSheet.Name = SheetName;
                    if (PagebreakPreview)
                        _ExcelWorkSheet.Application.ActiveWindow.View = Excel.XlWindowView.xlPageBreakPreview;
                    if (Landscape)
                        _ExcelWorkSheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                    if (FittoOnePage)
                    {
                        _ExcelWorkSheet.PageSetup.FitToPagesTall = 1;
                        _ExcelWorkSheet.PageSetup.FitToPagesWide = 1;
                        _ExcelWorkSheet.PageSetup.Zoom = false;
                    }
                    _ExcelWorkSheet.Application.ActiveWindow.Zoom = ZOOM;
                    _SheetNames = new ArrayList();
                    foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                        _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
                }
                else
                {
                    _ExcelWorkBook.Worksheets.Add(_ExcelWorkBook.Worksheets[1], Type.Missing, Type.Missing, Type.Missing);
                    _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[1];
                    _ExcelWorkSheet.Name = SheetName;
                    if (PagebreakPreview)
                        _ExcelWorkSheet.Application.ActiveWindow.View = Excel.XlWindowView.xlPageBreakPreview;
                    if (Landscape)
                        _ExcelWorkSheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                    if (FittoOnePage)
                    {
                        _ExcelWorkSheet.PageSetup.FitToPagesTall = 1;
                        _ExcelWorkSheet.PageSetup.FitToPagesWide = 1;
                        _ExcelWorkSheet.PageSetup.Zoom = false;
                    }
                    _ExcelWorkSheet.Application.ActiveWindow.Zoom = ZOOM;
                    _SheetNames = new ArrayList();
                    foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                        _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SheetName"></param>
        public void HideWorkSheet(string SheetName)
        {
            try
            {
                _ExcelApplication.DisplayAlerts = false;
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[SheetName];
                _ExcelWorkSheet.Visible = XlSheetVisibility.xlSheetHidden;
                _SheetNames = new ArrayList();
                foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                    _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Rename A Worksheet
        /// </summary>
        /// <param name="OldName">OldName</param>
        /// <param name="NewName">NewName</param>
        public void RenameSheet(string OldName, string NewName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[OldName];
                _ExcelWorkSheet.Name = NewName;
                _SheetNames = new ArrayList();
                foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                    _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Rename A Worksheet
        /// </summary>
        /// <param name="SheetIndex">Index Of The Sheet To Rename</param>
        /// <param name="Name">New Sheet Name</param>
        public void RenameSheet(int SheetIndex, string Name)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[SheetIndex];
                _ExcelWorkSheet.Name = Name;
                _SheetNames = new ArrayList();
                foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                    _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Add Multiple No Of Sheets At The Beginning
        /// </summary>
        /// <param name="SheetCount">No Of Sheets To Add</param>
        public void AddWorkSheet(int SheetCount)
        {
            try
            {
                _ExcelWorkBook.Worksheets.Add(_ExcelWorkBook.Worksheets[1], Type.Missing, SheetCount, Type.Missing);
                _SheetNames = new ArrayList();
                foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                    _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Deletes A Specified WorkSheet
        /// </summary>
        /// <param name="SheetName">SheetName To Delete</param>
        public void DeleteWorkSheet(string SheetName)
        {
            try
            {
                _ExcelApplication.DisplayAlerts = false;
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[SheetName];
                _ExcelWorkSheet.Delete();
                _SheetNames = new ArrayList();
                foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                    _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
                _ExcelApplication.DisplayAlerts = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Deletes A Specified WorkSheet
        /// </summary>
        /// <param name="SheetIndex">SheetIndex To Delete</param>
        public void DeleteWorkSheet(int SheetIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Worksheets[SheetIndex];
                _ExcelWorkSheet.Delete();
                _SheetNames = new ArrayList();
                foreach (Worksheet wsWorksheetName in _ExcelWorkBook.Sheets)
                    _SheetNames.Add(wsWorksheetName.Name.ToString().ToUpper());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Sets Data To A Specified Cell
        /// </summary>
        /// <param name="CellAddress">Address Of The Cell</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Value">Value To Set In The Cell</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SetCellData(string CellAddress, string SheetName, string Value)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[CellAddress, CellAddress].Value2 = Value;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetCellData(int Row, int Column, string SheetName, string Value)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[Row, Column], _ExcelWorkSheet.Cells[Row, Column]).Value2 = Value;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool ClearAllData(string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Cells.ClearContents();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetComment(string CellAddress, string SheetName, string strComment)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[CellAddress, CellAddress].AddComment(strComment);
                _ExcelWorkSheet.Range[CellAddress, CellAddress].Comment.Visible = false;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Sets Data To A Specified Cell
        /// </summary>
        /// <param name="Raw">Rawindex</param>
        /// <param name="Column">Columnindex</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Formula">Formula To Set In The Cell</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SetCellFormula(int Raw, int Column, string SheetName, string Formula)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[Raw, Column], _ExcelWorkSheet.Cells[Raw, Column]).Formula = Formula;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool SetCellFormula(string CellAddress, string SheetName, string Formula)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[CellAddress, CellAddress].Formula = Formula;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Sets Data To A Specified Column
        /// </summary>
        /// <param name="ColumnName">Name Of The Column</param>
        /// <param name="StartRow">Row To Start</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Values">One Dimentional String Array To Paste</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SetColumnData(string ColumnName, int StartRow, string SheetName, string[] Values)
        {
            try
            {
                object[,] objTemp = new object[Values.Length, 1];
                int i = 0;
                foreach (string strVal in Values)
                {
                    objTemp[i, 0] = strVal;
                    i++;
                }
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(ColumnName + StartRow.ToString(), ColumnName + (StartRow + Values.Length - 1).ToString()).Value2 = objTemp;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets Data To A Specified Row
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="RowIndex">Index Of The Row</param>
        /// <param name="StartColumnIndex">Start Column Index To Paste Data</param>
        /// <param name="Values">Data In A String Array</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SetRowData(string SheetName, int RowIndex, int StartColumnIndex, string[] Values)
        {
            try
            {
                object[,] objTemp = new object[1, Values.Length];
                int i = 0;
                foreach (string strVal in Values)
                {
                    objTemp[0, i] = strVal;
                    i++;
                }
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[RowIndex, StartColumnIndex], _ExcelWorkSheet.Cells[RowIndex, StartColumnIndex + Values.Length - 1]].Value2 = objTemp;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        
        /// <summary>
        /// Sets Data From Datatable
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="RowIndex">Index Of The Row</param>
        /// <param name="StartColumnIndex">Start Column Index To Paste Data</param>
        /// <param name="dtValue">Datatable dtValue</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SetDataFromDatatable(string SheetName, int RowIndex, int StartColumnIndex, System.Data.DataTable dtValue)
        {
            try
            {
                object[,] objTemp = new object[dtValue.Rows.Count, dtValue.Columns.Count];
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    for (int j = 0; j < dtValue.Columns.Count; j++)
                    {
                        if (dtValue.Columns[j].ColumnName.ToUpper().Contains("DATE") && dtValue.Rows[i][j] != DBNull.Value )
                            objTemp[i, j] = Convert.ToDateTime(dtValue.Rows[i][j].ToString().Trim().Split('/')[1] + "/" + dtValue.Rows[i][j].ToString().Trim().Split('/')[0] + "/" + dtValue.Rows[i][j].ToString().Trim().Split('/')[2]);
                        else
                            objTemp[i, j] = dtValue.Rows[i][j].ToString().Trim();
                    }
                }
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                /////////VS 2010////////
                if (SheetName != "Summary")
                {
                    Excel.Range foundrange = _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[RowIndex, 2], _ExcelWorkSheet.Cells[RowIndex + dtValue.Rows.Count - 1, 2]];
                    foundrange.NumberFormat = "[$-409]dd-MMM-yyyy;@";
                }
                Excel.Range _Range = _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[RowIndex, StartColumnIndex], _ExcelWorkSheet.Cells[RowIndex + dtValue.Rows.Count - 1, StartColumnIndex + dtValue.Columns.Count - 1]];
                _Range.Value2 = objTemp;
                /////////******//////////

                ////////VS 2005////////
                //_ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[RowIndex, StartColumnIndex], _ExcelWorkSheet.Cells[RowIndex + dtValue.Rows.Count - 1, StartColumnIndex + dtValue.Columns.Count - 1]).Value2 = objTemp;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets Data From Datatable
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="RowIndex">Index Of The Row</param>
        /// <param name="StartColumnIndex">Start Column Index To Paste Data</param>
        /// <param name="dtValue">Datatable dtValue</param>
        /// <param name="dtCommentTest">Datatable dtCommentTest</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SetDataFromDatatable(string SheetName, int RowIndex, int StartColumnIndex, System.Data.DataTable dtValue, System.Data.DataTable dtCommentTest)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                object[,] objTemp = new object[dtValue.Rows.Count, dtValue.Columns.Count];
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    for (int j = 0; j < dtValue.Columns.Count; j++)
                    {
                        objTemp[i, j] = dtValue.Rows[i][j].ToString().Trim();
                        if (dtValue.Rows[i][j].ToString().Trim().ToLower() == "false")
                        {
                            _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[i + RowIndex, j + StartColumnIndex], _ExcelWorkSheet.Cells[i + RowIndex, j + StartColumnIndex]).AddComment("LIVE||TEST\n" + dtCommentTest.Rows[i][j].ToString().Trim());
                            _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[i + RowIndex, j + StartColumnIndex], _ExcelWorkSheet.Cells[i + RowIndex, j + StartColumnIndex]).Interior.ColorIndex = 36;
                        }
                    }
                }
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[RowIndex, StartColumnIndex], _ExcelWorkSheet.Cells[RowIndex + dtValue.Rows.Count - 1, StartColumnIndex + dtValue.Columns.Count - 1]).Value2 = objTemp;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool SetDataFromDatatable(string SheetName, int RowIndex, int StartColumnIndex, System.Data.DataTable dtValue, bool IsHighLight)
        {

            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                object[,] objTemp = new object[dtValue.Rows.Count, dtValue.Columns.Count];
                for (int i = 0; i < dtValue.Rows.Count; i++)
                {
                    for (int j = 0; j < dtValue.Columns.Count; j++)
                    {
                        objTemp[i, j] = dtValue.Rows[i][j].ToString().Trim();
                        if ((bool)IsHighLight == true)
                        {
                            if (dtValue.Rows[i][j].ToString().Trim() == "1")
                            {
                                Excel.Range _RangeHighLight = _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[i + RowIndex, j + StartColumnIndex], _ExcelWorkSheet.Cells[i + RowIndex, j + StartColumnIndex]];
                                _RangeHighLight.Interior.ColorIndex = 36;
                            }
                        }
                    }
                }

                Excel.Range _Range = _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[RowIndex, StartColumnIndex], _ExcelWorkSheet.Cells[RowIndex + dtValue.Rows.Count - 1, StartColumnIndex + dtValue.Columns.Count - 1]];
                _Range.Value2 = objTemp;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Sets Data To A Specified Range
        /// </summary>
        /// <param name="StartRow">Row To Start</param>
        /// <param name="StartColumn">Column To Start</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Values">Two Dimentional String Array To Paste</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SetRangeData(int StartRow, int StartColumn, string SheetName, string[,] Values)
        {
            try
            {
                object[,] objTemp = new object[Values.GetLength(0), Values.GetLength(1)];
                for (int i = 0; i < Values.GetLength(0); i++)
                    for (int j = 0; j < Values.GetLength(1); j++)
                        objTemp[i, j] = Values.GetValue(i, j);
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[StartRow + Values.GetLength(0) - 1, StartColumn + Values.GetLength(1) - 1]).Value2 = objTemp;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets Data To A Specified Range As A Paste Special
        /// </summary>
        /// <param name="StartRow">Row To Start</param>
        /// <param name="StartColumn">Column To Start</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Value">String Value To Paste</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SetPasteSpecial(int StartRow, int StartColumn, string SheetName, string Value)
        {
            try
            {
               Clipboard.SetText(Value);
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[StartRow, StartColumn]).PasteSpecial(XlPasteType.xlPasteAll, XlPasteSpecialOperation.xlPasteSpecialOperationNone, Type.Missing, Type.Missing);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Auto Fit Columns
        /// </summary>
        /// <param name="ColumnNames">ColumnNames</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool AutoFitColumn(string[] ColumnNames, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                foreach (string ColumnName in ColumnNames)
                    _ExcelWorkSheet.get_Range(ColumnName + "1", ColumnName + "1").EntireColumn.AutoFit();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Auto Fit Columns
        /// </summary>
        /// <param name="ColumnIndexes">ColumnIndexes</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool AutoFitColumn(int[] ColumnIndexes, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                foreach (int ColumnIndex in ColumnIndexes)
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[1, ColumnIndex], _ExcelWorkSheet.Cells[1, ColumnIndex]).EntireColumn.AutoFit();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Wraps Text In A Specified Range
        /// </summary>
        /// <param name="StartCellAddress">Start Cell Address</param>
        /// <param name="EndCellAddress">End Cell Address</param>
        /// <param name="SheetName">SheetName</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool WrapText(string StartCellAddress, string EndCellAddress, string SheetName)
        {
            try
            {
                ((Worksheet)_ExcelWorkBook.Sheets[SheetName]).get_Range(StartCellAddress, EndCellAddress).WrapText = true;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Color Cell Interior Of A Cell
        /// </summary>
        /// <param name="CellAddress">Cell Address</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="ColorIndex">ColorIndex</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool ColorCellInterior(string CellAddress, string SheetName, int ColorIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[CellAddress, CellAddress].Interior.ColorIndex = ColorIndex;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Color Cell Interior Of A Range
        /// </summary>
        /// <param name="StartCellAddress">Start Cell Address</param>
        /// <param name="EndCellAddress">End Cell Address</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="ColorIndex">ColorIndex</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool ColorCellInterior(string StartCellAddress, string EndCellAddress, string SheetName, int ColorIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(StartCellAddress, EndCellAddress).Interior.ColorIndex = ColorIndex;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Color Cell Interior Of A Range
        /// </summary>
        /// <param name="StartRow">StartRow Index</param>
        /// <param name="StartColumn">StartColumn Index</param>
        /// <param name="EndRow">EndRow Index</param>
        /// <param name="EndColumn">EndColumn Index</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="ColorIndex">ColorIndex</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool ColorCellInterior(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int ColorIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Interior.ColorIndex = ColorIndex;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }


        public bool ColorCellInterior(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, double ColorIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Interior.Color = ColorIndex;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Copyies A Sheet As A New CSV File
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="NewCSVFileName">CSV File Name To Save As</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool CopySheetAsANewCSVFile(string SheetName, string NewCSVFileName)
        {
            try
            {
                Workbook ExcelWorkBook = _ExcelApplication.Workbooks.Add(Missing.Value);
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Copy(ExcelWorkBook.Worksheets[1], Missing.Value);
                File.Delete(NewCSVFileName);
                ExcelWorkBook.SaveAs(NewCSVFileName, Excel.XlFileFormat.xlCSV, null, null, false, false, Excel.XlSaveAsAccessMode.xlNoChange, false, false, null, null, null);
                ExcelWorkBook.Close(0, null, null);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Problem While Extracting Sheet Named \"" + SheetName + "\" As Csv To Temp Folder\n" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes A Column
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="ColumnName">Column Name</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool DeleteColumn(string SheetName, string ColumnName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(ColumnName + "1", ColumnName + "1").EntireColumn.Delete(Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes A Column
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="ColumnIndex">Column Index</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool DeleteColumn(string SheetName, int ColumnIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[1, ColumnIndex], _ExcelWorkSheet.Cells[1, ColumnIndex]).EntireColumn.Delete(Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes Columns
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="ColumnIndexs">Column Indexes</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool DeleteColumn(string SheetName, int[] ColumnIndexs)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                Array.Sort(ColumnIndexs);
                Array.Reverse(ColumnIndexs);
                foreach (int ColumnIndex in ColumnIndexs)
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[1, ColumnIndex], _ExcelWorkSheet.Cells[1, ColumnIndex]).EntireColumn.Delete(Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes A Row
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="RowIndex">Row Index</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool DeleteRow(string SheetName, string cellAddress)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[cellAddress, cellAddress].EntireRow.Delete(Missing.Value);
                //_ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[RowIndex, 1], _ExcelWorkSheet.Cells[RowIndex, 1]).EntireRow.Delete(Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Inserts A Column
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="BeforeColumnIndex">Column Index Before Which The Column Will Be Added</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool InsertColumn(string SheetName, int BeforeColumnIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[1, BeforeColumnIndex], _ExcelWorkSheet.Cells[1, BeforeColumnIndex]).EntireColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, false);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Inserts Specified No. Of Columns
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="BeforeColumnIndex">Column Index Before Which The Columns Will Be Added</param>
        /// <param name="NumberOfColumnsToInsert">Number Of Columns To Insert</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool InsertColumn(string SheetName, int BeforeColumnIndex, int NumberOfColumnsToInsert)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                for (int i = 1; i <= NumberOfColumnsToInsert; i++)
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[1, BeforeColumnIndex], _ExcelWorkSheet.Cells[1, BeforeColumnIndex]).EntireColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, false);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Inserts A Column
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="BeforeColumnnName">Column Name Before Which The Column Will Be Added</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool InsertColumn(string SheetName, string BeforeColumnnName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(BeforeColumnnName + "1", BeforeColumnnName + "1").EntireColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, false);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Inserts Specified No. Of Columns
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="BeforeColumnnName">Column Name Before Which The Columns Will Be Added</param>
        /// <param name="NumberOfColumnsToInsert">Number Of Columns To Insert</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool InsertColumn(string SheetName, string BeforeColumnnName, int NumberOfColumnsToInsert)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                for (int i = 1; i <= NumberOfColumnsToInsert; i++)
                    _ExcelWorkSheet.get_Range(BeforeColumnnName + "1", BeforeColumnnName + "1").EntireColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight, false);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool InsertRow(string SheetName, int RowNo, int RowIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                for (int i = 0; i < RowNo; i++)
                {
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[RowIndex, 1], _ExcelWorkSheet.Cells[RowIndex, 1]).EntireRow.Insert(Missing.Value, Missing.Value);
                }
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Fill Down Cells [Same As Control+D In Excel]
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="StartCellAddress">Start Cell Address</param>
        /// <param name="EndCellAddress">End Cell Address</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool FillDownCells(string SheetName, string StartCellAddress, string EndCellAddress)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                Regex regex = new Regex(@"([A-Za-z]+)([0-9]+)");
                string StartColumn = regex.Match(StartCellAddress).Groups[1].Value;
                int StartRow = Convert.ToInt32(regex.Match(StartCellAddress).Groups[2].Value);
                string EndColumn = regex.Match(EndCellAddress).Groups[1].Value;
                int EndRow = Convert.ToInt32(regex.Match(EndCellAddress).Groups[2].Value);
                if (EndRow <= StartRow)
                    return true;
                _ExcelWorkSheet.get_Range(StartCellAddress, EndCellAddress).FillDown();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Auto Fill Cells In Default Format
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="StartCellAddress">Start Cell Address</param>
        /// <param name="EndCellAddress">End Cell Address</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool DefaultAutoFillCells(string SheetName, string StartCellAddress, string EndCellAddress)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                Regex regex = new Regex(@"([A-Za-z]+)([0-9]+)");
                string StartColumn = regex.Match(StartCellAddress).Groups[1].Value;
                int StartRow = Convert.ToInt32(regex.Match(StartCellAddress).Groups[2].Value);
                string EndColumn = regex.Match(EndCellAddress).Groups[1].Value;
                int EndRow = Convert.ToInt32(regex.Match(EndCellAddress).Groups[2].Value);
                if (EndRow <= StartRow + 1)
                    return true;
                _ExcelWorkSheet.get_Range(StartColumn + StartRow.ToString(), EndColumn + (StartRow + 1).ToString()).AutoFill(_ExcelWorkSheet.get_Range(StartCellAddress, EndCellAddress), XlAutoFillType.xlFillDefault);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Auto Fill Cells In An Increasing Series Format
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="StartCellAddress">Start Cell Address</param>
        /// <param name="EndCellAddress">End Cell Address</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool SeriesAutoFillCells(string SheetName, string StartCellAddress, string EndCellAddress)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                Regex regex = new Regex(@"([A-Za-z]+)([0-9]+)");
                string StartColumn = regex.Match(StartCellAddress).Groups[1].Value;
                int StartRow = Convert.ToInt32(regex.Match(StartCellAddress).Groups[2].Value);
                string EndColumn = regex.Match(EndCellAddress).Groups[1].Value;
                int EndRow = Convert.ToInt32(regex.Match(EndCellAddress).Groups[2].Value);
                if (EndRow <= StartRow)
                    return true;
                _ExcelWorkSheet.get_Range(StartColumn + StartRow.ToString(), EndColumn + StartRow.ToString()).AutoFill(_ExcelWorkSheet.get_Range(StartCellAddress, EndCellAddress), XlAutoFillType.xlFillSeries);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get First Blank Cell RowIndex In A Specific Column
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="ColumnName">Column Name</param>
        /// <returns>If Suceeds True Else False</returns>
        public int GetFirstBlankCellRowIndexInAColumn(string SheetName, string ColumnName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                int i = 1;
                while (true)
                {
                    if (FetchCellData(ColumnName + i.ToString(), SheetName) == null || FetchCellData(ColumnName + i.ToString(), SheetName) == "")
                        break;
                    i++;
                }
                return i;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Get First Blank Cell RowIndex In A Specific Column
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="ColumnIndex">Column Index</param>
        /// <returns>If Suceeds True Else False</returns>
        public int GetFirstBlankCellRowIndexInAColumn(string SheetName, int ColumnIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                int i = 1;
                while (true)
                {
                    if (FetchCellData(i, ColumnIndex, SheetName) == null || FetchCellData(i, ColumnIndex, SheetName) == "")
                        break;
                    i++;
                }
                return i - 1;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Finds A String In Excel And Returns Cell Addresses In An String Array Of Found Cells, Else Returns null.
        /// </summary>
        /// <param name="SheetName">SheetName</param>
        /// <param name="FindMe">Text To Find</param>
        /// <param name="IsWholeCellContent">True If Whole Cell Content Is To Match</param>
        /// <returns>String Array</returns>
        public string[] Find(string SheetName, string FindMe, bool IsWholeCellContent)
        {
            _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
            string sFirstFoundAddress;
            ArrayList alAddresses = new ArrayList();
            Excel.Range rgFound;
            rgFound = _ExcelWorkSheet.Cells.Find(FindMe, _ExcelWorkSheet.Cells[1, 1], Excel.XlFindLookIn.xlValues, ((IsWholeCellContent ? Excel.XlLookAt.xlWhole : Excel.XlLookAt.xlPart)), Missing.Value,
                            Excel.XlSearchDirection.xlNext, false, Missing.Value, false);
            if (rgFound != null)
            {
                sFirstFoundAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                alAddresses.Add(sFirstFoundAddress);
                rgFound = _ExcelWorkSheet.Cells.FindNext(rgFound);
                string sAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                while (!sAddress.Equals(sFirstFoundAddress))
                {
                    alAddresses.Add(sAddress);
                    rgFound = _ExcelWorkSheet.Cells.FindNext(rgFound);
                    sAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                }
            }
            if (alAddresses.Count != 0) return (string[])alAddresses.ToArray(typeof(string));
            return null;
        }
        /// <summary>
        /// Finds A String In Excel  And Returns Cell Addresses In An String Array Of Found Cells, Else Returns null.
        /// </summary>
        /// <param name="SheetName">SheetName</param>
        /// <param name="FindMe">Text To Find</param>
        /// <param name="IsWholeCellContent">True If Whole Cell Content Is To Match</param>
        /// <returns>String Array</returns>
        public ArrayList Find(string FindMe, bool IsWholeCellContent, List<string> IgnoreSheets)
        {
            ArrayList alAddresses = new ArrayList();
            try
            {
                foreach (Worksheet _ExcelWorkSheet in _ExcelWorkBook.Sheets)
                {
                    //_ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                    if (!IgnoreSheets.Contains(_ExcelWorkSheet.Name))
                    {
                        string sFirstFoundAddress;
                        // ArrayList alAddresses = new ArrayList();
                        Excel.Range rgFound;
                        rgFound = _ExcelWorkSheet.Cells.Find(FindMe, _ExcelWorkSheet.Cells[1, 1], Excel.XlFindLookIn.xlValues, ((IsWholeCellContent ? Excel.XlLookAt.xlWhole : Excel.XlLookAt.xlPart)), Missing.Value,
                                        Excel.XlSearchDirection.xlNext, false, Missing.Value, false);
                        if (rgFound != null)
                        {
                            sFirstFoundAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                            alAddresses.Add(_ExcelWorkSheet.Name);
                            alAddresses.Add(_ExcelWorkSheet.Range[sFirstFoundAddress]);
                            rgFound = _ExcelWorkSheet.Cells.FindNext(rgFound);
                            if (rgFound != null)
                            {
                                string sAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                                while (!sAddress.Equals(sFirstFoundAddress))
                                {
                                    alAddresses.Add(_ExcelWorkSheet.Name);
                                    alAddresses.Add(_ExcelWorkSheet.Range[sAddress]);
                                    rgFound = _ExcelWorkSheet.Cells.FindNext(rgFound);
                                    sAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                                }
                            }
                        }
                    }
                }
                if (alAddresses.Count != 0) return (alAddresses);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// Adds A Hyperlink To The Specified Cell
        /// </summary>
        /// <param name="CellAddress">CellAddress</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Destination">Destination Address</param>
        /// <returns>True Or False Acc. As Success Or Failure</returns>
        public bool AddHyperlink(string CellAddress, string SheetName, string Destination)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                Excel.Hyperlink link = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(_ExcelWorkSheet.get_Range(CellAddress, Missing.Value), Destination, Missing.Value, Missing.Value, Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Adds A Hyperlink To The Specified Cell
        /// </summary>
        /// <param name="RowIndex">RowIndex</param>
        /// <param name="ColumnIndex">ColumnIndex</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Destination">Destination Address</param>
        /// <returns>True Or False Acc. As Success Or Failure</returns>
        public bool AddHyperlink(int RowIndex, int ColumnIndex, string SheetName, string Destination)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                Excel.Hyperlink link = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(_ExcelWorkSheet.Cells[RowIndex, ColumnIndex], Destination, Missing.Value, Missing.Value, Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }


        //mukesh overload
        /// <summary>
        /// Adds A Hyperlink To The Specified Cell
        /// </summary>
        /// <param name="RowIndex">RowIndex</param>
        /// <param name="ColumnIndex">ColumnIndex</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Destination">Destination Address</param>
        /// /// <param name="SubDestination">SubDestination Address</param>
        /// <returns>True Or False Acc. As Success Or Failure</returns>
        public bool AddHyperlink(int RowIndex, int ColumnIndex, string SheetName, string Destination, string SubDestination)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                Excel.Hyperlink link = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(_ExcelWorkSheet.Cells[RowIndex, ColumnIndex], Destination, SubDestination, Missing.Value, Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool AddImage(string sheetName, string imageAddress, float left, float top, float width, float height)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.Shapes.AddPicture(imageAddress, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoTrue, left, top, width, height);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool AddImageHyperlink(string sheetName, string imageAddress, string hyperlink, float left, float top, float width, float height)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];

                Excel.Shape rect = _ExcelWorkSheet.Shapes.AddPicture(imageAddress, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoTrue, left, top, width, height);

                Excel.Hyperlink hyper = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(rect, "", "'" + hyperlink + "'!A1", Type.Missing, Type.Missing);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool MergeCells(int StartRow, int EndRow, int StartColumn, int EndColumn, string sheetName)
        {
            try
            {

                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].MergeCells = true;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        //public bool MergeCells(string startCellAddress, string endCellAddress, string sheetName)
        //{
        //    try
        //    {
        //        _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
        //        _ExcelWorkSheet.Range[startCellAddress, endCellAddress].Merge(false);                
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //        return false;
        //    }
        //}


        public bool InsertChart(string ChartSheetName, string ChartDataSheetName, Microsoft.Office.Interop.Excel.XlChartType chartType)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range("D1", "F" + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, "D") - 1));
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(355.25, 260.25, 238.5, 100);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.SetSourceData(chartRange, System.Reflection.Missing.Value);
                //chartPage.ApplyLayout(3,Missing.Value);
                chartPage.ChartType = chartType;
                //chartPage.ApplyLayout(3, chartType);
                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis.CategoryType = Microsoft.Office.Interop.Excel.XlCategoryType.xlTimeScale;
                axis.MajorUnit = 6;
                axis.MajorUnitScale = Microsoft.Office.Interop.Excel.XlTimeUnit.xlMonths;
                axis.MinorUnit = 6;
                axis.MinorUnitScale = Microsoft.Office.Interop.Excel.XlTimeUnit.xlMonths;
                axis.TickLabels.NumberFormat = "[$-409]mmm-yy;@";
                //axis.TickLabelSpacing = 180;
                axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis.MinimumScale = 5;
                axis.MaximumScale = 100;
                Worksheet ws = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                //((Excel.Series)chartPage.SeriesCollection(1)).Delete();
                ((Excel.Series)chartPage.SeriesCollection(1)).XValues = ws.get_Range("D2", "D" + ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).UsedRange.Rows.Count.ToString());
                chartPage.ChartArea.Font.Size = 7;
                //chartPage.ChartTitle.Delete();
                chartPage.Legend.Top = 0;
                chartPage.Legend.Font.Size = 5;
                chartPage.PlotArea.Top = 25;
                chartPage.PlotArea.Height = chartPage.PlotArea.Height + 35;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;
                //chartPage.Legend.Border = XlBorderWeight.xlThin;
                chartPage.PlotArea.ClearFormats();
                //((Excel.Series)chartPage.SeriesCollection(1)).Format.Line.Weight = (float)0.000005f;
                //((Excel.Series)chartPage.SeriesCollection(2)).Format.Line.Weight = (float)0.000005f;
                //((Excel.Series)chartPage.SeriesCollection(1)).Format.ThreeD.RotationX = 0.0f;
                //((Excel.Series)chartPage.SeriesCollection(1)).Format.ThreeD.RotationY = 0.0f;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertLineChart(string ChartSheetName, string ChartDataSheetName)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range("D1", "F" + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, "D") - 1));
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(216, 170, 201.2, 155);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlLine;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;
                chartPage.PlotArea.ClearFormats();
                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis.TickLabels.NumberFormat = "[$-409]mmm-yy;@";
                axis.TickLabels.Font.Size = 6;
                Excel.Axis axis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis1.TickLabels.Font.Size = 6;
                chartPage.Legend.Font.Size = 6;
                axis1.MajorGridlines.Delete();
                ((Excel.Series)chartPage.SeriesCollection(1)).Border.ColorIndex = 25;
                ((Excel.Series)chartPage.SeriesCollection(2)).Border.ColorIndex = 8;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertPieChart(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, int StartColumn, int EndColumn, int StartRow, int EndRow)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(getColumnName(StartColumn) + StartRow, getColumnName(EndColumn) + EndRow);
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);//216, 357, 201.2, 95
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xl3DPie;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.PlotArea.Height = Hight - 5;
                chartPage.PlotArea.Width = Width - 5;
                chartPage.HasLegend = false;
                chartPage.PlotArea.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.Interior.ColorIndex = XlColorIndex.xlColorIndexNone;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, false, false, true, true, false, false, Missing.Value);
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(1)).Interior.ColorIndex = 25;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(2)).Interior.ColorIndex = 30;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(3)).Interior.ColorIndex = 28;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(4)).Interior.ColorIndex = 50;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(5)).Interior.ColorIndex = 47;
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                ((Excel.Series)chartPage.SeriesCollection(1)).Explosion = 10;
                ((Excel.ChartGroup)chartPage.ChartGroups(1)).FirstSliceAngle = 20;
                chartPage.Elevation = 10;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertPieChartRating(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, int StartCol, int EndCol)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(getColumnName(StartCol) + 1, getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1));

                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);//216, 357, 201.2, 95
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xl3DPie;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.PlotArea.Height = Hight - 5;
                chartPage.PlotArea.Width = Width - 50;

                chartPage.HasLegend = false;
                chartPage.PlotArea.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.Interior.ColorIndex = XlColorIndex.xlColorIndexNone;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, false, false, true, true, false, false, Missing.Value);
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                ((Excel.Series)chartPage.SeriesCollection(1)).Explosion = 10;
                chartPage.Elevation = 10;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(1)).Interior.ColorIndex = 25;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(2)).Interior.ColorIndex = 30;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(3)).Interior.ColorIndex = 28;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(4)).Interior.ColorIndex = 50;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(5)).Interior.ColorIndex = 47;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertPieChartInstrument(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, int StartCol, int EndCol)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(getColumnName(StartCol) + 1, getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1));

                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);//216, 357, 201.2, 95
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xl3DPie;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.PlotArea.Height = Hight - 5;
                chartPage.PlotArea.Width = Width - 5;
                chartPage.HasLegend = false;
                chartPage.PlotArea.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.Interior.ColorIndex = XlColorIndex.xlColorIndexNone;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, false, false, true, true, false, false, Missing.Value);


                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                ((Excel.Series)chartPage.SeriesCollection(1)).Explosion = 10;
                chartPage.Elevation = 10;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(1)).Interior.ColorIndex = 25;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(2)).Interior.ColorIndex = 30;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(3)).Interior.ColorIndex = 28;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(4)).Interior.ColorIndex = 50;
                ((Microsoft.Office.Interop.Excel.Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(5)).Interior.ColorIndex = 47;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBarchartMarketCap(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, string Column1, string Colimn2)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(Column1 + "1", Colimn2 + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, Colimn2) - 1));
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlColumnClustered;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlContinuous;
                chartPage.ChartArea.Font.Size = 6;
                chartPage.PlotArea.ClearFormats();
                chartPage.PlotArea.Width = 394;
                chartPage.PlotArea.Left = 30;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Text = "Allocation (%)";
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Left = 1;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Top = 10;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MajorUnit = 3;
                ((ChartGroup)chartPage.ChartGroups(1)).Overlap = 50;
                ((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 0;
                ((Series)chartPage.SeriesCollection(1)).Interior.ColorIndex = 30;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
                ((Series)chartPage.SeriesCollection(2)).Interior.ColorIndex = 8;
                ((Series)chartPage.SeriesCollection(2)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
                ((Series)chartPage.SeriesCollection(3)).Interior.ColorIndex = 25;
                ((Series)chartPage.SeriesCollection(3)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
                chartPage.PlotArea.Height = 55;
                chartPage.PlotArea.Top = 25;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertBarchartAUM(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, string Column1, string Colimn2)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(Column1 + "1", Colimn2 + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, Colimn2) - 1));
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);//2, 675, 188, 87
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlColumnClustered;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Delete();
                chartPage.ChartArea.Font.Size = 6;
                chartPage.PlotArea.ClearFormats();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinorUnit = 1000;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinimumScale = 0;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
                ((Series)chartPage.SeriesCollection(1)).Interior.ColorIndex = 25;
                chartPage.PlotArea.Height = Hight - 5;
                chartPage.PlotArea.Width = Width;
                //((ChartGroup)chartPage.ChartGroups(1)).Overlap = 50;
                //((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 0;
                //((Series)chartPage.SeriesCollection(1)).Interior.ColorIndex = 30;
                //((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
                //((Series)chartPage.SeriesCollection(2)).Interior.ColorIndex = 8;
                //((Series)chartPage.SeriesCollection(2)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertBarchartAssetAllocationMovement(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, string Column1, string Colimn2)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(Column1 + "3", Colimn2 + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, Colimn2) - 1));
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlColumnClustered;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlContinuous;
                chartPage.ChartArea.Font.Size = 6;
                chartPage.PlotArea.ClearFormats();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MajorUnit = 3;
                ((ChartGroup)chartPage.ChartGroups(1)).Overlap = 0;
                ((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 0;
                chartPage.PlotArea.Height = 170;
                chartPage.PlotArea.Width = 201.2;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBarchartPerformanceBenchmark(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, int StartColumn, int EndColumn, string BenchmarkIndex)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(getColumnName(StartColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(EndColumn)) - 1), getColumnName(EndColumn) + "1");//data taken from bottom

                SetCellData(getColumnName(EndColumn + 1) + 1, ChartDataSheetName, "MIN-MAX");
                SetCellFormula(2, EndColumn + 1, ChartDataSheetName, "=MIN(" + getColumnName(EndColumn) + "2:" + getColumnName(EndColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartColumn)) - 1));
                SetCellFormula(3, EndColumn + 1, ChartDataSheetName, "=MAX(" + getColumnName(EndColumn) + "2:" + getColumnName(EndColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartColumn)) - 1));

                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();

                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlColumnClustered;
                chartPage.PlotBy = XlRowCol.xlColumns;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Delete();
                chartPage.ChartTitle.Delete();

                chartPage.PlotArea.ClearFormats();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                //((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MaximumScale = 8;
                //((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinimumScale = -8;
                //((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorUnit = 5;
                //((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinorUnit = 1;
                //((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).CrossesAt = 0;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorTickMark = XlTickMark.xlTickMarkOutside;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinorTickMark = XlTickMark.xlTickMarkNone;

                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MajorUnit = 2;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).TickLabelPosition = XlTickLabelPosition.xlTickLabelPositionLow;
                //((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).TickLabels.Orientation = XlTickLabelOrientation.xlTickLabelOrientationUpward;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Text = "(%)";
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).ReversePlotOrder = false;
                ((ChartGroup)chartPage.ChartGroups(1)).Overlap = 0;
                ((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 150;
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                chartPage.PlotArea.Height = Hight;
                chartPage.PlotArea.Width = Width;

                //Excel.Shape shape = _ExcelWorkSheet.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, (float)Left+170, (float)Top+10, 104, 14);
                //shape.ZOrder(MsoZOrderCmd.msoBringToFront);

                //_ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.Characters(Missing.Value, Missing.Value).Text = "Monthly Fund Return +/- " + BenchmarkIndex;

                //_ExcelWorkSheet.Shapes._Default(shape.Name).Line.Visible = MsoTriState.msoFalse;
                //_ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.Characters(Missing.Value, Missing.Value).Font.Name = "Calibri";
                //_ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.Characters(Missing.Value, Missing.Value).Font.FontStyle = "Regular";
                //_ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.Characters(Missing.Value, Missing.Value).Font.Size = 7;
                //_ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.AutoSize = true;

                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                //axis.TickLabels.NumberFormat = "[$-409]mmm-yy;@";
                //axis.TickLabels.Font.Size = 6;
                Excel.Axis axis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                //axis1.TickLabels.Font.Size = 6;
                double Min = Math.Round((FetchCellData(getColumnName(EndColumn + 1) + "2", "Data").Trim() != "" ? Convert.ToDouble(FetchCellData(getColumnName(EndColumn + 1) + "2", "Data")) : axis1.MinimumScale), 2);
                axis1.MinimumScale = Math.Round(Min) - 1;//- Convert.ToInt64(Min.ToString().Substring(Min.ToString().Length - 1, 1));
                double Max = (FetchCellData(getColumnName(EndColumn + 1) + "3", "Data").Trim() != "" ? Convert.ToDouble(FetchCellData(getColumnName(EndColumn + 1) + "3", "Data")) : axis1.MaximumScale);
                axis1.MaximumScale = Math.Round(Max) + 1;//+ 10 - Convert.ToInt64(Max.ToString().Substring(Max.ToString().Length - 1, 1));
                //axis1.MajorUnit = Math.Round((axis1.MaximumScale - axis1.MinimumScale) , 0);


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertScateredChart(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, int StartColumn, int EndColumn)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(getColumnName(StartColumn) + "1", getColumnName(EndColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, EndColumn)));
                //SetCellData(getColumnName(EndColumn + 1) + 1, ChartDataSheetName, "MIN-MAX");
                //SetCellFormula(2, EndColumn + 1, ChartDataSheetName, "=ROUND(MIN(" + getColumnName(StartColumn) + "2:" + getColumnName(EndColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartColumn)) - 1) + "),0)");

                //SetCellFormula(3, EndColumn + 1, ChartDataSheetName, "=ROUND(MAX(" + getColumnName(StartColumn) + "2:" + getColumnName(EndColumn) + "2),0)");
                //SetCellFormula(4, EndColumn + 1, ChartDataSheetName, "=ROUND(MAX(" + getColumnName(StartColumn + 1) + "2:" + getColumnName(EndColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartColumn)) - 1) + "),0)");

                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlXYScatter;
                chartPage.SetSourceData(chartRange, XlRowCol.xlRows);
                chartPage.HasLegend = true;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionBottom;
                chartPage.PlotArea.Interior.ColorIndex = 2;
                ((Series)chartPage.SeriesCollection(1)).Name = "=Data!R1C8";
                ((Series)chartPage.SeriesCollection(1)).XValues = "=Data!R2C8";
                ((Series)chartPage.SeriesCollection(1)).Values = "=Data!R3C8";
                ((Series)chartPage.SeriesCollection(2)).Name = "=Data!R1C9";
                ((Series)chartPage.SeriesCollection(2)).XValues = "=Data!R2C9";
                ((Series)chartPage.SeriesCollection(2)).Values = "=Data!R3C9";

                //chartPage.ChartTitle.Delete();
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).ReversePlotOrder = false;
                //((ChartGroup)chartPage.ChartGroups(1)).Overlap = 0;
                //((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 150;
                chartPage.PlotArea.Height = Hight - 55;
                chartPage.PlotArea.Width = Width;

                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();

                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorUnitIsAuto = true;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinorUnitIsAuto = true;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinimumScaleIsAuto = true;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MaximumScaleIsAuto = true;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Text = "Annualised Sharp";
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MajorUnitIsAuto = true;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MinorUnitIsAuto = true;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MaximumScaleIsAuto = true;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MinimumScaleIsAuto = true;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).AxisTitle.Text = "Annualised Return";

                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, true, true, Missing.Value, false, false, false, false, false, Missing.Value);
                ((Series)chartPage.SeriesCollection(2)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, true, true, Missing.Value, false, false, false, false, false, Missing.Value);
                ((Series)chartPage.SeriesCollection(1)).MarkerStyle = XlMarkerStyle.xlMarkerStyleCircle;
                ((Series)chartPage.SeriesCollection(1)).MarkerSize = 10;
                ((Series)chartPage.SeriesCollection(1)).MarkerBackgroundColorIndex = XlColorIndex.xlColorIndexAutomatic;
                //((Series)chartPage.SeriesCollection(1)).MarkerForegroundColor = 30;
                //((Series)chartPage.SeriesCollection(1)).MarkerBackgroundColor = 30;

                ((Series)chartPage.SeriesCollection(2)).MarkerStyle = XlMarkerStyle.xlMarkerStyleCircle;
                ((Series)chartPage.SeriesCollection(2)).MarkerSize = 10;
                //((Series)chartPage.SeriesCollection(2)).MarkerBackgroundColor = 5;
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                chartPage.PlotArea.Border.LineStyle = 0;
                //chartPage.PlotArea.Interior.ColorIndex = 2;
                //Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                ////axis.TickLabels.NumberFormat = "[$-409]mmm-yy;@";
                //axis.TickLabels.Font.Size = 6;
                //Excel.Axis axis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                //axis1.TickLabels.Font.Size = 6;
                //int Min = (int)(FetchCellData(getColumnName(EndCol + 1) + "2", "Data").Trim() != "" ? Convert.ToDouble(FetchCellData(getColumnName(EndCol + 1) + "2", "Data")) : axis1.MinimumScale);
                //axis1.MinimumScale = Min - Convert.ToInt64(Min.ToString().Substring(Min.ToString().Length - 1, 1));
                //int Max = (int)(FetchCellData(getColumnName(EndCol + 1) + "3", "Data").Trim() != "" ? Convert.ToDouble(FetchCellData(getColumnName(EndCol + 1) + "3", "Data")) : axis1.MaximumScale);
                //axis1.MaximumScale = Max + 10 - Convert.ToInt64(Max.ToString().Substring(Max.ToString().Length - 1, 1));
                //axis1.MajorUnit = Math.Round((axis1.MaximumScale - axis1.MinimumScale) / 5, 0);





                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertLineChart(string ChartSheetName, string ChartDataSheetName, int StartCol, int EndCol, int Series1Col, int Series2Col, int Series3Col)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(getColumnName(StartCol) + 1, getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1));
                SetCellData(getColumnName(EndCol + 1) + 1, ChartDataSheetName, "MIN-MAX");
                SetCellFormula(2, EndCol + 1, ChartDataSheetName, "=ROUND(MIN(" + getColumnName(StartCol + 1) + "2:" + getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1) + "),0)");
                SetCellFormula(3, EndCol + 1, ChartDataSheetName, "=ROUND(MAX(" + getColumnName(StartCol + 1) + "2:" + getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1) + "),0)");

                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(216, 170, 201.2, 155);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlLine;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;
                chartPage.PlotArea.ClearFormats();
                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis.TickLabels.NumberFormat = "[$-409]mmm-yy;@";
                axis.TickLabels.Font.Size = 6;
                Excel.Axis axis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis1.TickLabels.Font.Size = 6;
                int Min = (int)(FetchCellData(getColumnName(EndCol + 1) + "2", "Data").Trim() != "" ? Convert.ToDouble(FetchCellData(getColumnName(EndCol + 1) + "2", "Data")) : axis1.MinimumScale);
                axis1.MinimumScale = Min - Convert.ToInt64(Min.ToString().Substring(Min.ToString().Length - 1, 1));
                int Max = (int)(FetchCellData(getColumnName(EndCol + 1) + "3", "Data").Trim() != "" ? Convert.ToDouble(FetchCellData(getColumnName(EndCol + 1) + "3", "Data")) : axis1.MaximumScale);
                axis1.MaximumScale = Max + 10 - Convert.ToInt64(Max.ToString().Substring(Max.ToString().Length - 1, 1));
                axis1.MajorUnit = Math.Round((axis1.MaximumScale - axis1.MinimumScale) / 5, 0);
                chartPage.Legend.Font.Size = 6;
                axis1.MajorGridlines.Delete();
                ((Excel.Series)chartPage.SeriesCollection(1)).Border.ColorIndex = Series1Col;
                ((Excel.Series)chartPage.SeriesCollection(2)).Border.ColorIndex = Series2Col;
                ((Excel.Series)chartPage.SeriesCollection(3)).Border.ColorIndex = Series3Col;
                //((Excel.Series)chartPage.SeriesCollection(1)).Border.ColorIndex = 25;
                //((Excel.Series)chartPage.SeriesCollection(2)).Border.ColorIndex = 8;
                //((Excel.Series)chartPage.SeriesCollection(3)).Border.Color=
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertLineBarchartAssetAllocationMovement(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, string Column1, string Colimn2)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(Column1 + "3", Colimn2 + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, Column1) - 1));
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];                
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlColumnClustered;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionBottom;
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.ChartArea.Font.Size = 6;
                chartPage.ChartArea.Font.Name = "Arial";
                chartPage.PlotArea.ClearFormats();
                chartPage.ChartArea.Border.LineStyle = XlLineStyle.xlContinuous;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                

                ((ChartGroup)chartPage.ChartGroups(1)).Overlap = 6;
                ((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 40;

                
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).TickLabels.NumberFormat = "[$-409]dd-MMM-yy;@";
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).CategoryType = XlCategoryType.xlCategoryScale;


                ((Series)chartPage.SeriesCollection(2)).Interior.Color = 10078719;
                ((Series)chartPage.SeriesCollection(2)).Border.LineStyle = XlLineStyle.xlLineStyleNone;
                ((Series)chartPage.SeriesCollection(3)).Interior.Color = 26367;
                ((Series)chartPage.SeriesCollection(3)).Border.LineStyle = XlLineStyle.xlLineStyleNone;                

                ///////////////////////////Line Chart///////////////////////////////
                ((Series)chartPage.SeriesCollection(1)).Border.Color = 39423;
                ((Series)chartPage.SeriesCollection(1)).ChartType = XlChartType.xlLine;
                ((Series)chartPage.SeriesCollection(1)).AxisGroup = Excel.XlAxisGroup.xlSecondary;
                ///////////////////////////Line Chart///////////////////////////////
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).TickLabelPosition = XlTickLabelPosition.xlTickLabelPositionLow;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Closes The Excel Running
        /// </summary>
        /// <param name="SaveOrNot">Save Or Not [If True Is Passed As Parameter But IsReadOnly Property Is Set To True Then You Can Not Save The Excel]</param>
        public void CloseExcel(bool SaveOrNot)
        {
            try
            {
                if (SaveOrNot && !_ReadOnlyOrNot)
                    _ExcelWorkBook.Save();
                _ExcelWorkBook.Close(0, null, null);
                //_ExcelWorkbookTemp.Close(0, null, null);
                _ExcelApplication.Quit();
                if (_ExcelWorkSheet != null)
                    Marshal.ReleaseComObject(_ExcelWorkSheet);
                if (_ExcelWorkBook != null)
                    Marshal.ReleaseComObject(_ExcelWorkBook);
                if (_ExcelWorkbookTemp != null)
                    Marshal.ReleaseComObject(_ExcelWorkbookTemp);
                if (_ExcelApplication != null)
                    Marshal.ReleaseComObject(_ExcelApplication);
                _SheetNames = null;
                _ExcelWorkBook = null;
                _ExcelWorkbookTemp = null;
                _ExcelApplication = null;
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Kills Any Excel Process Running Behind
        /// </summary>
        public static void KillExcel()
        {
            Process[] processArr = Process.GetProcessesByName("EXCEL");
            if (processArr.Length > 0)
            {
                //Excel.Application oXL = (Excel.Application)Marshal.GetActiveObject("Excel.Application");
                //foreach (Workbook oWB1 in oXL.Workbooks)
                //    oWB1.Close(0, null, null);
                //oXL.Quit();
                foreach (Process pr in processArr)
                    pr.Kill();
            }
        }
        /// <summary>
        /// Given a ColumnNumber Returns ColumnName
        /// </summary>
        public string getColumnName(int Val)
        {
            Val = Val + 64;

            if (Val > 272)
            {

                return ("I" + (char)((Val - 272) + 64));

            }

            if (Val > 246)
            {

                return ("G" + (char)((Val - 246) + 64));

            }

            if (Val > 220)
            {

                return ("F" + (char)((Val - 220) + 64));

            }

            if (Val > 194)
            {

                return ("E" + (char)((Val - 194) + 64));

            }

            if (Val > 168)
            {

                return ("D" + (char)((Val - 168) + 64));

            }

            if (Val > 142)
            {

                return ("C" + (char)((Val - 142) + 64));

            }

            if (Val > 116)
            {

                return ("B" + (char)((Val - 116) + 64));

            }

            if (Val > 90)
            {

                return ("A" + (char)((Val - 90) + 64));

            }

            return "" + (char)Val;

        }

        #region Give Alphabet and Get Column Number
        public int getColumnNumber(string alphabet)
        {
            switch (alphabet)
            {
                case "A":
                    return 1;
                case "B":
                    return 2;

                case "C":
                    return 3;

                case "D":
                    return 4;

                case "E":
                    return 5;

                case "F":
                    return 6;

                case "G":
                    return 7;

                case "H":
                    return 8;

                case "I":
                    return 9;

                case "J":
                    return 10;

                case "K":
                    return 11;

                case "L":
                    return 12;

                case "M":
                    return 13;

                case "N":
                    return 14;

                case "O":
                    return 15;

                case "P":
                    return 16;

                case "Q":
                    return 17;

                case "R":
                    return 18;

                case "S":
                    return 19;

                case "T":
                    return 20;

                case "U":
                    return 21;

                case "V":
                    return 22;

                case "W":
                    return 23;

                case "X":
                    return 24;

                case "Y":
                    return 25;

                case "Z":
                    return 26;

                case "AA":
                    return 27;

                case "AB":
                    return 28;

                case "AC":
                    return 29;

                case "AD":
                    return 30;

                case "AE":
                    return 31;

                case "AF":
                    return 32;

                case "AG":
                    return 33;

                case "AH":
                    return 34;

                case "AI":
                    return 35;

                case "AJ":
                    return 36;

                case "AK":
                    return 37;

                case "AL":
                    return 38;

                case "AM":
                    return 39;

                case "AN":
                    return 40;

                case "AO":
                    return 41;

                case "AP":
                    return 42;

                case "AQ":
                    return 43;

                case "AR":
                    return 44;

                case "AS":
                    return 45;

                case "AT":
                    return 46;

                case "AU":
                    return 47;

                case "AV":
                    return 48;

                case "AW":
                    return 49;

                case "AX":
                    return 50;

                case "AY":
                    return 51;

                case "AZ":
                    return 52;

                default:
                    return 1;

            }


        }
        #endregion

        public bool FindReplaceText(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, string FindText, string ReplaceText)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Replace(FindText, ReplaceText, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool MargeCellsData(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, string Values)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].MergeCells = true;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Value2 = Values;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool MargeCellsData(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, string Values, bool isHAlignmentCentre, bool isVAlignmentCentre, bool isWrapText, bool isBorder)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Merge(null);
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Value2 = Values;
                if (isHAlignmentCentre)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                else
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                if (isVAlignmentCentre)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                if (isWrapText)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].WrapText = true;
                if (isBorder)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Borders.LineStyle = Excel.Constants.xlSolid;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Format A Specified Range
        /// </summary>
        /// <param name="StartRow">Row To Start</param>
        /// <param name="StartColumn">Column To Start</param>
        /// <param name="StartRow">Row To end</param>
        /// <param name="StartColumn">Column To end</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Values">String</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool FormatCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Fontcolor, int fontsize, string font, object backcolorindex, bool isBold, bool isCenter)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.ColorIndex = Fontcolor;
                if (isCenter)
                {
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                }
                else
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Bold = isBold;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Size = fontsize;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Name = font;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Interior.Color = backcolorindex == null ? XlColorIndex.xlColorIndexNone : backcolorindex;
                return true;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool FormatCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Fontcolor, int fontsize, string font, bool isBold, bool isCenter)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.ColorIndex = Fontcolor;
                if (isCenter)
                {
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                }
                else
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Bold = isBold;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Size = fontsize;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Name = font;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool FormatCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int color, int fontsize, string font, int backcolorindex, bool isBold, bool isCenter, string isleft)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Color = (object)color;
                if (isCenter)
                {
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }
                else if (isleft.ToUpper() == "YES")
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                else
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Bold = isBold;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Size = fontsize;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Name = font;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Interior.Color = (object)backcolorindex;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool FormatCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Fontcolor, int fontsize, string font, object backcolorindex, bool isBold, bool isCenter, bool isItalic)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.ColorIndex = Fontcolor;
                if (isCenter)
                {
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                }
                else
                {
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Bold = isBold;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Size = fontsize;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Name = font;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Interior.Color = backcolorindex == null ? XlColorIndex.xlColorIndexNone : backcolorindex;
                if (isItalic)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Italic = isItalic;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool FormatCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Fontcolor, int fontsize, string font, object backcolorindex, bool isBold, bool isCenter, bool isItalic, bool isBorder)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.ColorIndex = Fontcolor;
                if (isCenter)
                {
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                }
                else
                {
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Bold = isBold;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Size = fontsize;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Name = font;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Interior.ColorIndex = backcolorindex == null ? XlColorIndex.xlColorIndexNone : backcolorindex;
                if (isItalic)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Italic = isItalic;
                if (isBorder)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Borders.LineStyle = Excel.Constants.xlSolid;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool FormatCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Fontcolor, int fontsize, string font, int backcolorindex, bool isBold, bool isCenter, bool isItalic, bool isBorder)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Color = Fontcolor;
                if (isCenter)
                {
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                }
                else
                {
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Bold = isBold;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Size = fontsize;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Name = font;
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Interior.Color = backcolorindex ;
                if (isItalic)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Italic = isItalic;
                if (isBorder)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Borders.LineStyle = Excel.Constants.xlSolid;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }


        public bool FormatCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, bool isBorder, string isleft)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                if (isleft.ToUpper() == "LEFT")
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                else if (isleft.ToUpper() == "CENTER")
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                else
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                if (isBorder)
                    _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Borders.LineStyle = Excel.Constants.xlSolid;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetNumberFormat(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, string DecimalPrecision, string DecimalScale)
        {
            try
            {
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].NumberFormat = DecimalPrecision + "." + DecimalScale;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool SetTextFormat(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName)
        {
            try
            {
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].EntireColumn.NumberFormat = "@";
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetFontColorFromRGB(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Red, int Green, int Blue)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(Red, Green, Blue));
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetBackgroundColorFromRGB(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Red, int Green, int Blue)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(Red, Green, Blue));
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetColorIndexFromRGB(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Red, int Green, int Blue)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(Red, Green, Blue));
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Value = _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Interior.ColorIndex;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Border A Specified Range
        /// </summary>
        /// <param name="StartRow">Row To Start</param>
        /// <param name="StartColumn">Column To Start</param>
        /// <param name="StartRow">Row To end</param>
        /// <param name="StartColumn">Column To end</param>
        /// <param name="SheetName">SheetName</param>
        /// <param name="Values">String</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool BorderCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Borders.LineStyle = Excel.Constants.xlSolid;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetColumnWith(int StartRow, int StartColumn, int EndRow, int EndColumn, double ColumnWidth, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).ColumnWidth = ColumnWidth;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public void SetColumnWidth(int col, int width)
        {
            ((Range)_ExcelWorkSheet.Cells[1, col]).EntireColumn.ColumnWidth = width;
        }

        public bool SetRowHeight(int StartRow, int StartColumn, int EndRow, int EndColumn, double RowHeight, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].RowHeight = RowHeight;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public Excel.Range getRange(string startCell, string endCell, string sheetName)
        {
            Excel.Range reqRange = null;
            try
            {
                reqRange = ((Worksheet)_ExcelWorkBook.Sheets[sheetName]).get_Range(startCell, endCell);
                return reqRange;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return reqRange;

            }
        }
        public Excel.Range getRange(int Row, int Column, string sheetName)
        {
            Excel.Range reqRange = null;
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                reqRange = _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[Row, Column], _ExcelWorkSheet.Cells[Row, Column]);
                return reqRange;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return reqRange;

            }
        }
        public Excel.Range getRange(int SRow, int SColumn, int ERow, int EColumn, string sheetName)
        {
            Excel.Range reqRange = null;
            try
            {
                // _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                reqRange = ((Worksheet)_ExcelWorkBook.Sheets[sheetName]).get_Range(_ExcelWorkSheet.Cells[SRow, SColumn], _ExcelWorkSheet.Cells[ERow, EColumn]);
                return reqRange;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return reqRange;

            }
        }

        public bool FreezePanes(string SheetName, string CellAddress, bool value)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(CellAddress, CellAddress).Select();
                _ExcelWorkSheet.Application.ActiveWindow.FreezePanes = true;

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool ColumnFreezePanes(string SheetName, int ColumnNumber, bool value)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Application.ActiveWindow.SplitColumn = ColumnNumber;
                _ExcelWorkSheet.Application.ActiveWindow.FreezePanes = value;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool HideRow(string SheetName, string cellAddress)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.Range[cellAddress, cellAddress].EntireRow.Hidden = true;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool InsertLineChart(string ChartSheetName, string ChartDataSheetName, Microsoft.Office.Interop.Excel.XlChartType chartType,int StartCol, int EndCol, double left, double top, double width, double height, int DiffDate,int Series1Color, int Series2Color,int minVal,int maxVal)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).Range["A1", getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1)];
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();

                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(left, top, width, height); 
                Excel.Chart chartPage = myChart.Chart;
                chartPage.SetSourceData(chartRange, System.Reflection.Missing.Value);
                //chartPage.ApplyLayout(3,Missing.Value);
                chartPage.ChartType = chartType;
                chartPage.ChartArea.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                //chartPage.ApplyLayout(3, chartType);

                //////////////////////////////Add Secondary Axis////////////////////////////////
                //Excel.Series series1 = (Excel.Series)chartPage.SeriesCollection(3);
                //series1.AxisGroup = Excel.XlAxisGroup.xlSecondary;
                //Excel.Axis snd_x_axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlSecondary);
                //////////////////////////////Add Secondary Axis////////////////////////////////
                Worksheet ws = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                chartPage.ChartArea.Font.Size = 8;
                chartPage.ChartArea.Font.Name = "Zurich BT";
                //chartPage.Legend.Top = 0;
                chartPage.Legend.Font.Size = 8;
                chartPage.Legend.Font.Name = "Zurich BT";
                //chartPage.PlotArea.Width = 280;
                //chartPage.PlotArea.Height = chartPage.PlotArea.Height + 35;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;

                ((Excel.Series)chartPage.SeriesCollection(1)).Border.Weight = 2;
                ((Excel.Series)chartPage.SeriesCollection(1)).Border.Color = Series1Color;

                ((Excel.Series)chartPage.SeriesCollection(2)).Border.Weight = 2;
                ((Excel.Series)chartPage.SeriesCollection(2)).Border.Color = Series2Color;

                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis.CategoryType = Microsoft.Office.Interop.Excel.XlCategoryType.xlTimeScale;
                ////addition////    
                axis.MajorUnitScale = XlTimeUnit.xlDays;
                axis.MajorUnit = Math.Ceiling(Convert.ToDouble(DiffDate / 10)) - 1;
                ////addition////
                //axis.MajorUnit = 6;                
                //axis.MinorUnit = 1;                
                axis.TickLabels.NumberFormat = "[$-409]mmm-yy;@";
                axis.TickLabels.Orientation = XlTickLabelOrientation.xlTickLabelOrientationUpward;

                //axis.TickLabelSpacing = 180;
                axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);

                //((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).HasTitle = true;
                //((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Text = "Net Asset Value";

                axis.MinimumScale = minVal;
                axis.MaximumScale = maxVal;
                axis.TickLabels.NumberFormat = "0";
                //axis.MaximumScale = 250;
                axis.MajorGridlines.Delete();

                //((Excel.Series)chartPage.SeriesCollection(3)).Border.Weight = 2;
                //((Excel.Series)chartPage.SeriesCollection(3)).Border.ColorIndex = 1;
                //((Excel.Series)chartPage.SeriesCollection(3)).Border.LineStyle = XlLineStyle.xlDash;
                //((Excel.Series)chartPage.SeriesCollection(3)).MarkerSize = 3;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertLineChart(string ChartSheetName, string ChartDataSheetName, int StartCol, int EndCol, int Series1Col, int Series2Col, int Series3Col, double left, double top, double width, double height, double Majorunit)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).Range["A3", getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1)];
                //SetCellData(getColumnName(EndCol + 1) + 1, ChartDataSheetName, "MIN-MAX");
                // SetCellFormula("D2", ChartDataSheetName, "=ROUND(MIN(" + getColumnName(StartCol + 1) + "2:" + getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1) + "),0)");
                // SetCellFormula("D3", ChartDataSheetName, "=ROUND(MAX(" + getColumnName(StartCol + 1) + "2:" + getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1) + "),0)");
                //chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(left, top, width, height);//195, 170, 201.2, 155
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlLine;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;
                chartPage.PlotArea.ClearFormats();
                chartPage.ChartArea.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                ((Excel.Series)chartPage.SeriesCollection(1)).Border.ColorIndex = Series1Col;
                ((Excel.Series)chartPage.SeriesCollection(2)).Border.ColorIndex = Series2Col;
                //((Excel.Series)chartPage.SeriesCollection(1)).Format.Line.Weight = (float)0.000005f;
                //((Excel.Series)chartPage.SeriesCollection(2)).Format.Line.Weight = (float)0.000005f;
                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis.TickLabels.NumberFormat = "[$-409]mmm-yy;@";
                axis.TickLabels.Font.Size = 8;
                axis.MajorUnit = Majorunit;
                axis.MajorUnitScale = Microsoft.Office.Interop.Excel.XlTimeUnit.xlDays;
                axis.MinorUnitIsAuto = false;
                axis.MinorUnit = 1;
                axis.MinorUnitScale = Excel.XlTimeUnit.xlMonths;
                axis.MinorUnitIsAuto = true;
                Excel.Axis axis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis1.TickLabels.Font.Size = 8;
                //axis1.AxisTitle.Text = "Rupees";
                //int Min = (int)(FetchCellDatas(getColumnName(EndCol + 1) + "2", ChartDataSheetName).Trim() != "" ? Convert.ToDouble(FetchCellDatas(getColumnName(EndCol + 1) + "2", ChartDataSheetName)) : axis1.MinimumScale);
                //axis1.MinimumScale = Min - Convert.ToInt64(Min.ToString().Substring(Min.ToString().Length - 1, 1));
                //int Max = (int)(FetchCellDatas(getColumnName(EndCol + 1) + "3", ChartDataSheetName).Trim() != "" ? Convert.ToDouble(FetchCellDatas(getColumnName(EndCol + 1) + "3", ChartDataSheetName)) : axis1.MaximumScale);
                //axis1.MaximumScale = Max + 10 - Convert.ToInt64(Max.ToString().Substring(Max.ToString().Length - 1, 1));
                //axis1.MajorUnit = Math.Round((axis1.MaximumScale - axis1.MinimumScale) / 5, 0);
                chartPage.Legend.Font.Size = 8;
                axis1.MajorGridlines.Delete();
                //((Excel.Series)chartPage.SeriesCollection(1)).Select();
                //((Excel.Series)chartPage.SeriesCollection(1)).Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(128, 0, 128).ToArgb();
                //((Excel.Series)chartPage.SeriesCollection(1)).Border.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(128, 0, 128));


                //((Excel.Series)chartPage.SeriesCollection(1)).Border.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 200, 100));
                //((Excel.Series)chartPage.SeriesCollection(2)).Border.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(79, 129, 189));
                // ((Excel.Series)chartPage.SeriesCollection(2)).Format.Line.ForeColor.RGB = System.Drawing.Color.FromArgb(255, 200, 100).ToArgb();

                //var seriesCollection = (SeriesCollection)chartPage.SeriesCollection(Type.Missing);
                //Excel.Series series = seriesCollection.Item(seriesCollection.Count);
                //series.ClearFormats();


                //                ((Excel.Series)chartPage.SeriesCollection(1)).
                //                Sub ChartLineWidth()
                //    Dim c As Chart, s As Series
                //    For Each co In ActiveSheet.ChartObjects()
                //        Set c = co.Chart
                //        For Each s In c.SeriesCollection
                //            s.Format.Line.Weight = 1
                //        Next s
                //    Next co
                //End Sub
                //Excel.Series ser;
                //foreach (ChatSerie cs in chartPage.se)
                //{

                //    cs.Format.Options.IsVaryColor = true;

                //    cs.DataPoints.DefaultDataPoint.DataLabels.HasValue = true;

                //}

                // foreach(Excel.Series ser in seriesCollection)
                // {

                //     ser.DataPoints.DefaultDataPoint.DataLabels.HasValue = true;
                // }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       
        //anirban
        public bool InsertLineChart(string ChartSheetName, string ChartDataSheetName, int StartCol, int EndCol,int StartRow, double left, double top, double width, double height, double Majorunit,string ChartTitle)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).Range[getColumnName(StartCol) + StartRow, getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1)];
                
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(left, top, width, height);//195, 170, 201.2, 155
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlLine;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.HasTitle = true;
                chartPage.ChartTitle.Text = ChartTitle;                
                chartPage.ChartArea.Border.LineStyle = XlLineStyle.xlContinuous;
                
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlContinuous;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionBottom;
                chartPage.PlotArea.ClearFormats();                
                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                               
                axis.TickLabels.NumberFormat = "dd/mm/yyyy";
                axis.TickLabels.Font.Size = 8;
                axis.MajorUnit = Majorunit;
                axis.MajorUnitScale = Microsoft.Office.Interop.Excel.XlTimeUnit.xlDays;
                axis.MinorUnitIsAuto = false;
                axis.MinorUnit = 1;
                axis.MinorUnitScale = Excel.XlTimeUnit.xlMonths;
                axis.MinorUnitIsAuto = true;
                Excel.Axis axis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis1.TickLabels.Font.Size = 8;
                
                chartPage.Legend.Font.Size = 8;
                axis1.MajorGridlines.Delete();                

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

        //Dwarika 15/12/2011
        /*
        public bool SetShape(string sheetName, Microsoft.Office.Core.MsoAutoShapeType shapetype, string text, int left, int top, int width, int height)
        {
            try
            {
                string ExcelVersion = _ExcelApplication.Version;

                if (ExcelVersion == "12.0")
                {
                    _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                    _ExcelWorkSheet.Activate();
                    Excel.Shape rect = _ExcelWorkSheet.Shapes.AddShape(shapetype, left, top, width, height);
                    rect.TextEffect.FontBold = Microsoft.Office.Core.MsoTriState.msoTrue;
                    rect.Select();
                    rect.Adjustments[1] = 0.2f;
                    rect.Fill.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 128));
                    rect.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;                    
                    rect.TextEffect.Text = text;
                    rect.TextEffect.Alignment = Microsoft.Office.Core.MsoTextEffectAlignment.msoTextEffectAlignmentCentered;
                    Excel.Hyperlink hyper = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(rect, "", "'" + text + "'!A1", Type.Missing, Type.Missing);
                    return true;
                }
                else
                {
                    _ExcelWorkBook.CheckCompatibility = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        */

        public bool SetShape(string sheetName, Microsoft.Office.Core.MsoAutoShapeType shapetype, string text, int left, int top, int width, int height)
        {
            try
            {
                string ExcelVersion = _ExcelApplication.Version;

                if (ExcelVersion == "12.0")
                {
                    _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                    _ExcelWorkSheet.Activate();
                    Excel.Shape rect = _ExcelWorkSheet.Shapes.AddShape(shapetype, left, top, width, height);
                    rect.TextEffect.FontBold = Microsoft.Office.Core.MsoTriState.msoTrue;
                    rect.Select();
                    rect.Adjustments[1] = 0.2f;
                    rect.Fill.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 0, 128));
                    rect.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                    //gred add//

                    //gred add//
                    rect.TextEffect.Text = text;
                    rect.TextEffect.Alignment = Microsoft.Office.Core.MsoTextEffectAlignment.msoTextEffectAlignmentCentered;
                    Excel.Hyperlink hyper = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(rect, "", "'" + text + "'!A1", Type.Missing, Type.Missing);
                    return true;
                }
                else
                {
                    //_ExcelWorkBook.CheckCompatibility = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }


        // colorfullshape forecolor
        public bool SetShape(string sheetName, Microsoft.Office.Core.MsoAutoShapeType shapetype, string text, string hyperlinksheet, int left, int top, int width, int height, int red, int green, int blue)
        {
            try
            {

                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.Activate();
                Excel.Shape rect = _ExcelWorkSheet.Shapes.AddShape(shapetype, left, top, width, height);
                rect.TextEffect.FontBold = Microsoft.Office.Core.MsoTriState.msoTrue;
                rect.Select();
                rect.Adjustments[1] = 0.2f;
                rect.Fill.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(red, green, blue));
                //rect.Fill.BackColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(red, green, blue));
                // rect.Fill.BackColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(red, green, blue));
                rect.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                rect.TextEffect.Text = text;
                rect.Fill.TwoColorGradient(Microsoft.Office.Core.MsoGradientStyle.msoGradientHorizontal, 2);
                // rect.Fill.GradientDegree
                //rect.Fill.GradientColorType = Microsoft.Office.Core.MsoGradientColorType.msoGradientTwoColors;
                rect.TextEffect.Alignment = Microsoft.Office.Core.MsoTextEffectAlignment.msoTextEffectAlignmentCentered;
                Excel.Hyperlink hyper = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(rect, "", "'" + hyperlinksheet + "'!A1", Type.Missing, Type.Missing);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        // colorfullshape forecolor with gradient
        public bool SetShape(string sheetName, Microsoft.Office.Core.MsoAutoShapeType shapetype, string text, string hyperlinksheet, int fontsize, string fontname, int left, int top, int width, int height, int red, int green, int blue, int gradientVarient)
        {
            try
            {

                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.Activate();
                Excel.Shape rect = _ExcelWorkSheet.Shapes.AddShape(shapetype, left, top, width, height);
                rect.TextEffect.FontBold = Microsoft.Office.Core.MsoTriState.msoTrue;
                rect.Select();
                rect.Adjustments[1] = 0.2f;
                rect.Fill.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(red, green, blue));
                rect.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                rect.TextEffect.Text = text;
                rect.TextEffect.FontSize = fontsize;
                rect.TextEffect.FontName = fontname;
                rect.Fill.TwoColorGradient(Microsoft.Office.Core.MsoGradientStyle.msoGradientHorizontal, gradientVarient);

                //rect.Fill.GradientStyle = Microsoft.Office.Core.MsoGradientStyle.msoGradientHorizontal;
                // rect.Fill.OneColorGradient(Microsoft.Office.Core.MsoGradientStyle.msoGradientHorizontal, 2, 270);
                //rect.Fill.GradientColorType = Microsoft.Office.Core.MsoGradientColorType.msoGradientTwoColors;
                rect.TextEffect.Alignment = Microsoft.Office.Core.MsoTextEffectAlignment.msoTextEffectAlignmentCentered;
                Excel.Hyperlink hyper = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(rect, "", "'" + hyperlinksheet + "'!A1", Type.Missing, Type.Missing);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Added by Beauty
        public bool SetShape(string sheetName, Microsoft.Office.Core.MsoAutoShapeType shapetype, string text, string hyperlinksheet, int fontsize, string fontname, int left, int top, int width, int height, int red, int green, int blue)
        {
            try
            {

                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.Activate();
                Excel.Shape rect = _ExcelWorkSheet.Shapes.AddShape(shapetype, left, top, width, height);
                rect.TextEffect.FontBold = Microsoft.Office.Core.MsoTriState.msoTrue;
                rect.Select();
                rect.Adjustments[1] = 0.2f;
                rect.Fill.ForeColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(red, green, blue));
                rect.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                rect.TextEffect.Text = text;
                rect.TextEffect.FontSize = fontsize;
                rect.TextEffect.FontName = fontname;

                //rect.Fill.GradientStyle = Microsoft.Office.Core.MsoGradientStyle.msoGradientHorizontal;
                // rect.Fill.OneColorGradient(Microsoft.Office.Core.MsoGradientStyle.msoGradientHorizontal, 2, 270);
                //rect.Fill.GradientColorType = Microsoft.Office.Core.MsoGradientColorType.msoGradientTwoColors;
                rect.TextEffect.Alignment = Microsoft.Office.Core.MsoTextEffectAlignment.msoTextEffectAlignmentCentered;
                Excel.Hyperlink hyper = (Excel.Hyperlink)_ExcelWorkSheet.Hyperlinks.Add(rect, "", "'" + hyperlinksheet + "'!A1", Type.Missing, Type.Missing);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool AddTextBoxShape(string sheetName, string text, string fontName, int fontSize, int left, int top, int width, int height) //, int red, int green, int blue
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                Excel.Shape shape = _ExcelWorkSheet.Shapes.AddTextbox(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, (float)left, (float)top, (float)width, (float)height);
                shape.TextEffect.Text = text;
                shape.TextEffect.FontName = fontName;
                shape.TextEffect.FontBold = Microsoft.Office.Core.MsoTriState.msoFalse;
                shape.TextEffect.FontSize = fontSize;
                shape.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                shape.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;
                // shape.
                //shape.Fill.BackColor.RGB = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(red, green, blue));
                //shape.ZOrder(MsoZOrderCmd.msoBringToFront);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SplicFirstColumn(string sheetName)
        {
            _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
            _ExcelWorkSheet.Activate();
            _ExcelWorkSheet.Application.ActiveWindow.SplitColumn = 1;
            _ExcelWorkSheet.Application.ActiveWindow.FreezePanes = true;
        }

        public void HideGridlines(string sheetName)
        {
            _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
            Excel.Application olx = _ExcelWorkSheet.Application;
            olx.Windows.get_Item(1).DisplayGridlines = false;
        }

        public void CopyPasetRangeOfLines(string CopyFromSheetName, int CopyFromStartRow, int CopyFromStartColumn, int CopyFromEndRow, int CopyFromEndColumn, string PasteToSheetName, int PasteToSheetRow, int PasteToSheetColumn)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[CopyFromSheetName];
                _ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[CopyFromStartRow, CopyFromStartColumn], _ExcelWorkSheet.Cells[CopyFromEndRow, CopyFromEndColumn]].Copy(Type.Missing);
                Worksheet _ExcelWorkSheet1 = (Worksheet)_ExcelWorkBook.Sheets[PasteToSheetName];
                _ExcelWorkSheet1.Range[_ExcelWorkSheet1.Cells[PasteToSheetRow, PasteToSheetColumn], _ExcelWorkSheet1.Cells[PasteToSheetRow, PasteToSheetColumn]].Select();
                _ExcelWorkSheet1.Paste(Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            {

            }
        }

        public bool FindAndColorCellData(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, string FindText, int Fontcolor, int fontsize, string font, object backcolorindex, bool isBold, bool isCenter, bool isItalic, bool isBorder, bool IsWholeCellContent)
        {
            try
            {
                string sFirstFoundAddress;
                ArrayList alAddresses = new ArrayList();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                //Range _Range=_ExcelWorkSheet.Range[_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]].Find(FindText, Missing.Value, Missing.Value, Missing.Value, Missing.Value,XlSearchDirection.xlNext,true,true);



                Range rgFound = _ExcelWorkSheet.Cells.Find(FindText, _ExcelWorkSheet.Cells[StartRow, StartColumn], Excel.XlFindLookIn.xlValues, ((IsWholeCellContent ? Excel.XlLookAt.xlWhole : Excel.XlLookAt.xlPart)), Missing.Value,
                          Excel.XlSearchDirection.xlNext, false, Missing.Value, false);
                if (rgFound != null)
                {
                    sFirstFoundAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                    alAddresses.Add(_ExcelWorkSheet.Range[sFirstFoundAddress]);
                    rgFound = _ExcelWorkSheet.Cells.FindNext(rgFound);
                    if (rgFound != null)
                    {
                        string sAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                        while (!sAddress.Equals(sFirstFoundAddress))
                        {
                            alAddresses.Add(_ExcelWorkSheet.Range[sAddress]);
                            rgFound = _ExcelWorkSheet.Cells.FindNext(rgFound);
                            sAddress = rgFound.get_Address(true, true, Excel.XlReferenceStyle.xlA1, Missing.Value, Missing.Value);
                        }
                    }
                }



                foreach (Range _Range in alAddresses)
                {
                    _Range.Font.ColorIndex = Fontcolor;
                    if (isCenter)
                    {
                        _Range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        _Range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                    }
                    else
                    {
                        _Range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        _Range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    }
                    _Range.Font.Bold = isBold;
                    _Range.Font.Size = fontsize;
                    _Range.Font.Name = font;
                    _Range.Interior.Color = backcolorindex == null ? XlColorIndex.xlColorIndexNone : backcolorindex;
                    if (isItalic)
                        _Range.Font.Italic = isItalic;
                    if (isBorder)
                        _Range.Borders.LineStyle = Excel.Constants.xlSolid;
                }
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Added by Beauty
        public bool PageHeaderTitle(string SheetName, string title)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append(@"&""Lucida Sans,Bold""&14&K972A27" + title + "");
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.ActiveSheet;
                _ExcelWorkSheet.PageSetup.LeftHeader = str.ToString();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool PageRightHeaderPicture(string ImageName, float width, float height)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.ActiveSheet;
                _ExcelWorkSheet.PageSetup.RightHeaderPicture.Filename = ImageName;
                _ExcelWorkSheet.PageSetup.RightHeaderPicture.Width = width;
                _ExcelWorkSheet.PageSetup.RightHeaderPicture.Height = height;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool PageLeftHeaderPicture_LP(string sheetName, string ImageName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.PageSetup.LeftHeaderPicture.Filename = ImageName;
                //_ExcelWorkSheet.PageSetup.Orientation = XlPageOrientation.xlPortrait;
                //_ExcelWorkSheet.PageSetup.FitToPagesTall = 1;
                //_ExcelWorkSheet.PageSetup.FitToPagesWide = 1;
                //_ExcelWorkSheet.PageSetup.PrintQuality=600;
                //_ExcelWorkSheet.PageSetup.PaperSize=XlPaperSize.xlPaperA4;

                _ExcelWorkSheet.PageSetup.LeftHeader = "";
                _ExcelWorkSheet.PageSetup.CenterHeader = "";
                _ExcelWorkSheet.PageSetup.RightHeader = "";
                _ExcelWorkSheet.PageSetup.LeftFooter = "";
                _ExcelWorkSheet.PageSetup.CenterFooter = "";
                _ExcelWorkSheet.PageSetup.RightFooter = "";
                _ExcelWorkSheet.PageSetup.LeftMargin = 0.7f;
                _ExcelWorkSheet.PageSetup.RightMargin = 0.7f;
                _ExcelWorkSheet.PageSetup.TopMargin = 0.75f;
                _ExcelWorkSheet.PageSetup.BottomMargin = 0.75f;
                _ExcelWorkSheet.PageSetup.HeaderMargin = 0.3f;
                _ExcelWorkSheet.PageSetup.FooterMargin = 0.3f;
                _ExcelWorkSheet.PageSetup.PrintHeadings = false;
                _ExcelWorkSheet.PageSetup.PrintGridlines = false;
                _ExcelWorkSheet.PageSetup.PrintComments = XlPrintLocation.xlPrintNoComments;
                _ExcelWorkSheet.PageSetup.PrintQuality = 600;
                _ExcelWorkSheet.PageSetup.CenterHorizontally = false;
                _ExcelWorkSheet.PageSetup.CenterVertically = false;
                _ExcelWorkSheet.PageSetup.Orientation = XlPageOrientation.xlPortrait;
                _ExcelWorkSheet.PageSetup.Draft = false;
                _ExcelWorkSheet.PageSetup.PaperSize = XlPaperSize.xlPaperA4;
                // _ExcelWorkSheet.PageSetup.FirstPageNumber = xlAutomatic;
                _ExcelWorkSheet.PageSetup.Order = XlOrder.xlDownThenOver;
                _ExcelWorkSheet.PageSetup.BlackAndWhite = false;
                _ExcelWorkSheet.PageSetup.Zoom = false;
                _ExcelWorkSheet.PageSetup.FitToPagesWide = 1;
                _ExcelWorkSheet.PageSetup.FitToPagesTall = 1;
                _ExcelWorkSheet.PageSetup.PrintErrors = XlPrintErrors.xlPrintErrorsDisplayed;
                //_ExcelWorkSheet.PageSetup.OddAndEvenPagesHeaderFooter = false;
                //_ExcelWorkSheet.PageSetup.DifferentFirstPageHeaderFooter = false;
                //_ExcelWorkSheet.PageSetup.ScaleWithDocHeaderFooter = true;
                //_ExcelWorkSheet.PageSetup.AlignMarginsHeaderFooter = true;
                //_ExcelWorkSheet.PageSetup.EvenPage.LeftHeader.Text = "";
                //_ExcelWorkSheet.PageSetup.EvenPage.CenterHeader.Text = "";
                //_ExcelWorkSheet.PageSetup.EvenPage.RightHeader.Text = "";
                //_ExcelWorkSheet.PageSetup.EvenPage.LeftFooter.Text = "";
                //_ExcelWorkSheet.PageSetup.EvenPage.CenterFooter.Text = "";
                //_ExcelWorkSheet.PageSetup.EvenPage.RightFooter.Text = "";
                //_ExcelWorkSheet.PageSetup.FirstPage.LeftHeader.Text = "";
                //_ExcelWorkSheet.PageSetup.FirstPage.CenterHeader.Text = "";
                //_ExcelWorkSheet.PageSetup.FirstPage.RightHeader.Text = "";
                //_ExcelWorkSheet.PageSetup.FirstPage.LeftFooter.Text = "";
                //_ExcelWorkSheet.PageSetup.FirstPage.CenterFooter.Text = "";
                //_ExcelWorkSheet.PageSetup.FirstPage.RightFooter.Text = "";
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SelectSheet(string sheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.Select();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        //Added by Beauty
        public bool InsertBarchart(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, string Column1, string Colimn2, int DataStartRow, string ChartTitle, object BackColor1, object BackColor2, object BackColor3, int TickLabelsOffset)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(Column1 + DataStartRow, Colimn2 + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, Column1) - 1));
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlColumnClustered;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionBottom;
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlContinuous;
                chartPage.ChartArea.Font.Size = 9;
                chartPage.ChartArea.Font.Name = "Arial";
                chartPage.PlotArea.ClearFormats();
                
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                chartPage.ChartArea.Border.LineStyle = XlLineStyle.xlContinuous;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).AxisTitle.Text = ChartTitle;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).TickLabels.Offset = TickLabelsOffset;
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).AxisTitle.Left = left;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).AxisTitle.Top = 5;

                ((ChartGroup)chartPage.ChartGroups(1)).Overlap = 6;                
                ((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 150;


                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).TickLabels.NumberFormat = "[$-409]dd-MMM-yy;@";
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).CategoryType = XlCategoryType.xlCategoryScale;

                ((Series)chartPage.SeriesCollection(1)).Interior.Color = BackColor1;
                ((Series)chartPage.SeriesCollection(1)).Border.LineStyle = XlLineStyle.xlContinuous;
                ((Series)chartPage.SeriesCollection(2)).Interior.Color = BackColor2;
                ((Series)chartPage.SeriesCollection(2)).Border.LineStyle = XlLineStyle.xlContinuous;
                ((Series)chartPage.SeriesCollection(3)).Interior.Color = BackColor3;
                ((Series)chartPage.SeriesCollection(3)).Border.LineStyle = XlLineStyle.xlContinuous;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IndustryAUMLineChart(string ChartSheetName, string ChartDataSheetName, int StartCol, int EndCol, int StartRow, double left, double top, double width, double height, string ChartTitle)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).Range[getColumnName(StartCol) + StartRow, getColumnName(EndCol) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartCol)) - 1)];

                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(left, top, width, height);//195, 170, 201.2, 155
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlLine;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).AxisTitle.Text = ChartTitle;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).AxisTitle.Top = 14;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).AxisTitle.Left = 153;
                chartPage.ChartArea.Border.LineStyle = XlLineStyle.xlContinuous;
                //chartPage.ChartArea.Format.Fill.BackColor.RGB = 12632256;
                //chartPage.PlotArea.Format.Fill.BackColor.RGB = 12632256;

                chartPage.Legend.Border.LineStyle = XlLineStyle.xlContinuous;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionBottom;
                chartPage.PlotArea.ClearFormats();
                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis.TickLabels.NumberFormat = "MMM-yy";
                axis.TickLabels.Font.Size = 8;
                axis.MajorUnit = 1;
                axis.MajorUnitScale = Microsoft.Office.Interop.Excel.XlTimeUnit.xlMonths;
                axis.MinorUnitIsAuto = false;
                axis.MinorUnit = 1;
                axis.MinorUnitScale = Excel.XlTimeUnit.xlMonths;
                axis.MinorUnitIsAuto = false;
                Excel.Axis axis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis1.TickLabels.Font.Size = 8;
                axis1.MinimumScale = 5.5;
                axis1.MaximumScale = 8.3;
                chartPage.Legend.Delete();
                axis1.MajorGridlines.Delete();
                chartPage.ChartTitle.Delete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}