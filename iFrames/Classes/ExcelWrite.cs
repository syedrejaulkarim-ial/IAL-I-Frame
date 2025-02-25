using System;
using System.Collections.Generic;
using System.Text;
using Excel = Microsoft.Office.Interop.Access;
using System.Windows.Forms;
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
//using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

namespace DowJonesFactsheet
{
    /// <summary>
    /// This Class Is Used To Handel Excel Activities
    /// </summary>
    public class ExcelWrite
    {       
        object misValue = System.Reflection.Missing.Value;
        protected Excel.ApplicationClass _ExcelApplication;
        protected Workbook _ExcelWorkBook;
        public Workbook _ExcelWorkbookTemp;
        protected Worksheet _ExcelWorkSheet;
        protected Worksheet _ExcelWorkSheet1;
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
            if (!File.Exists(ExcelFilePath))
            {
                //using (FileStream file = File.Create(ExcelFilePath)) ;
                Excel.Application oXL = new Excel.Application();
                oXL.Visible = false;
                Workbook oWB = (Workbook)(oXL.Workbooks.Add(Missing.Value));
                oWB.SaveAs(_ExcelFilePath, Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Excel.XlSaveAsAccessMode.xlNoChange, false, false, null, null, null);
                oWB.Close(0, null, null);
                oXL.Quit();
            }
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
                _ExcelApplication = new Excel.ApplicationClass();
                _ExcelApplication.Visible = _VisibleOrNot;
                _ExcelApplication.DisplayAlerts = _DisplayAlerts;
                _ExcelWorkBook = _ExcelApplication.Workbooks.Open(_ExcelFilePath, 1, _ReadOnlyOrNot, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, Missing.Value, false);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
        public string FetchCellFormula(int row,int column, string SheetName)
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
                MessageBox.Show(ex.Message);
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
                alTemp.Add(objData[i,1]);
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
            object[,] objData = FetchRangeData(ColumnName + StartCellRowIndex.ToString(), ColumnName + EndCellRowIndex.ToString(), SheetName);
            for (int i = 1; i < objData.GetLength(0); i++)
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
                MessageBox.Show(ex.Message);
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
                string sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + _ExcelFilePath + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
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
                MessageBox.Show(ex.Message);
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
                
                DataRow[] drs = dtReturned.Select("["+dtReturned.Columns[NonBlankColumnIndex - 1].ColumnName + "] is not null And len('" + dtReturned.Columns[NonBlankColumnIndex - 1].ColumnName.ToString().Trim() + "') <> 0");
                foreach (DataRow dr in drs)
                    dtReturned_1.ImportRow(dr); 

                return dtReturned_1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                _ExcelWorkSheet.get_Range(CellAddress, CellAddress).Value2 = Value;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
        public bool SetCellFormula(int Raw,int Column, string SheetName, string Formula)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[Raw, Column], _ExcelWorkSheet.Cells[Raw, Column]).Formula = Formula;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[RowIndex, StartColumnIndex], _ExcelWorkSheet.Cells[RowIndex, StartColumnIndex + Values.Length - 1]).Value2 = objTemp;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                for (int i = 0; i < dtValue.Rows.Count;i++)
                {
                    for (int j = 0; j < dtValue.Columns.Count; j++)
                    {
                        objTemp[i, j] = dtValue.Rows[i][j].ToString().Trim();
                    }
                }
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[RowIndex, StartColumnIndex], _ExcelWorkSheet.Cells[RowIndex + dtValue.Rows.Count - 1, StartColumnIndex + dtValue.Columns.Count - 1]).Value2 = objTemp;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                foreach(string ColumnName in ColumnNames)
                    _ExcelWorkSheet.get_Range(ColumnName + "1", ColumnName + "1").EntireColumn.AutoFit();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                ((Worksheet)_ExcelWorkBook.Sheets[SheetName]).get_Range(StartCellAddress, EndCellAddress).WrapText = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                _ExcelWorkSheet.get_Range(CellAddress, CellAddress).Interior.ColorIndex = ColorIndex;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show("Problem While Extracting Sheet Named \"" + SheetName + "\" As Csv To Temp Folder\n" + ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes A Row
        /// </summary>
        /// <param name="SheetName">Sheet Name</param>
        /// <param name="RowIndex">Row Index</param>
        /// <returns>If Suceeds True Else False</returns>
        public bool DeleteRow(string SheetName, int RowIndex)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[RowIndex, 1], _ExcelWorkSheet.Cells[RowIndex, 1]).EntireRow.Delete(Missing.Value);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                return i-1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool AddImage(string sheetName, string imageAddress, float left, float top, float width, float height)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.Shapes.AddPicture(imageAddress, MsoTriState.msoFalse, MsoTriState.msoTrue, left, top, width, height);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool MergeCells(string startCellAddress, string endCellAddress, string sheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[sheetName];
                _ExcelWorkSheet.get_Range(startCellAddress, endCellAddress).Merge(true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool InsertChart(string ChartSheetName,string ChartDataSheetName, Microsoft.Office.Interop.Excel.XlChartType chartType)
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
                axis.CategoryType=Microsoft.Office.Interop.Excel.XlCategoryType.xlTimeScale;
                axis.MajorUnit=6;
                axis.MajorUnitScale=Microsoft.Office.Interop.Excel.XlTimeUnit.xlMonths;
                axis.MinorUnit=6;
                axis.MinorUnitScale=Microsoft.Office.Interop.Excel.XlTimeUnit.xlMonths;
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
                chartPage.SetSourceData(chartRange,XlRowCol.xlColumns);
                chartPage.Legend.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;
                chartPage.PlotArea.ClearFormats();
                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                axis.TickLabels.NumberFormat = "[$-409]mmm-yy;@";   
                axis.TickLabels.Font.Size=6;
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

        public bool InsertPieChart(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight,int StartColumn,int EndColumn,int StartRow,int EndRow, string sch_id,string file_path)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();

                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(getColumnName(StartColumn)+StartRow, getColumnName(EndColumn)+EndRow);
                chartRange.Select();
                chartRange.Activate();
                
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);//216, 357, 201.2, 95
                myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);//216, 357, 201.2, 95
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xl3DPie;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.HasLegend = false;
                
                
                chartPage.PlotArea.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.Interior.ColorIndex = XlColorIndex.xlColorIndexNone;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, false, false, true, true, false, false, Missing.Value);
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(1)).Interior.ColorIndex = 25;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(2)).Interior.ColorIndex = 30;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(3)).Interior.ColorIndex = 28;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(4)).Interior.ColorIndex = 50;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(5)).Interior.ColorIndex = 47;
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                ((Excel.Series)chartPage.SeriesCollection(1)).Explosion = 10;
                ((Excel.ChartGroup)chartPage.ChartGroups(1)).FirstSliceAngle = 20;
                chartPage.Elevation = 20;
                chartPage.PlotArea.Height = ((Hight - chartPage.PlotArea.Height) + chartPage.PlotArea.Height)-5;// Convert.ToDouble((Hight - 5));
                chartPage.PlotArea.Width = Width - 5;
                //chartPage.Export(@"C:\\DowJones\\DowJones_report\\PERFORMANCE_VIS-A-VIS_BENCHMARK_" + sch_id + ".jpg", "JPG", misValue);
                DateTime dt_img = System.DateTime.Today;
                string img_date = dt_img.ToString("MMyyyy");

                chartPage.Export(file_path + "\\CAPITALIZATION_EXPOSURE_" + sch_id + "_" + img_date + ".png", "PNG", misValue);
                //chartPage.Export(@"C:\Market_Cap.jpg" + dt.Second, "JPG", misValue);
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        public bool InsertPieChartRating(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, int StartCol, int EndCol,string sch_code,string file_path)
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
                //chartPage.PlotArea.Height = Hight - 5;
                //chartPage.PlotArea.Width = Width - 5;
                
                chartPage.HasLegend = false;
                chartPage.PlotArea.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.Interior.ColorIndex = XlColorIndex.xlColorIndexNone;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, false, false, true, true, false, false, Missing.Value);
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                ((Excel.Series)chartPage.SeriesCollection(1)).Explosion = 10;
                chartPage.Elevation = 20;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(1)).Interior.ColorIndex = 25;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(2)).Interior.ColorIndex = 30;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(3)).Interior.ColorIndex = 28;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(4)).Interior.ColorIndex = 50;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(5)).Interior.ColorIndex = 47;
               
                //DateTime dt = System.DateTime.Now;
                //TimeSpan ts = new TimeSpan();
                //dt += ts;
                chartPage.PlotArea.Height = ((Hight - chartPage.PlotArea.Height) + chartPage.PlotArea.Height) - 5;// Convert.ToDouble((Hight - 5));
                chartPage.PlotArea.Width = Width - 5;

                DateTime dt_img = System.DateTime.Today;
                string img_date = dt_img.ToString("MMyyyy");
                chartPage.Export(file_path + "\\CREDIT_QUALITY_RATINGS_" + sch_code + "_" + img_date + ".png", "PNG", misValue);
                //chartPage.Export(@"C:\Credit_Quality_Break_Up.jpg" + dt.Second, "JPG", misValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertPieChartInstrument(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, int StartCol, int EndCol,string sch_id,string file_path)
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
                //chartPage.PlotArea.Height = Hight - 5;
                //chartPage.PlotArea.Width = Width - 5;
                chartPage.HasLegend = false;
                chartPage.PlotArea.Border.LineStyle = XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.Interior.ColorIndex = XlColorIndex.xlColorIndexNone;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, false, false, true, true, false, false, Missing.Value);
               

                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                ((Excel.Series)chartPage.SeriesCollection(1)).Explosion = 10;
                chartPage.Elevation = 20;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(1)).Interior.ColorIndex = 25;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(2)).Interior.ColorIndex = 30;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(3)).Interior.ColorIndex = 28;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(4)).Interior.ColorIndex = 50;
                //((Point)((Excel.Series)chartPage.SeriesCollection(1)).Points(5)).Interior.ColorIndex = 47;
                

                chartPage.PlotArea.Height = ((Hight - chartPage.PlotArea.Height) + chartPage.PlotArea.Height) - 5;// Convert.ToDouble((Hight - 5));
                chartPage.PlotArea.Width = Width - 5;
                DateTime dt_img = System.DateTime.Today;
                string img_date = dt_img.ToString("MMyyyy");
                chartPage.Export(file_path+"\\INSTRUMENT_BREAKUP_" + sch_id +"_"+img_date+".png", "PNG", misValue);
                //chartPage.Export(@"C:\Instrument_Classifcation.jpg" + dt.Second, "JPG", misValue);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBarchartMarketCap(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight,string Column1,string Colimn2, string sch_id, string File_Path)
        {
            try
            {
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                double FirstCellVal =Math.Round(Convert.ToDouble(((Excel.Range)_ExcelWorkSheet.Cells[1, 9]).Value2.ToString()),0);
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(Column1 + "1", Colimn2 + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, Colimn2) - 1));
                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight+5.0);
                Excel.Chart chartPage = myChart.Chart;                               

                chartPage.ChartType = XlChartType.xlColumnClustered;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
              
                //chartPage.Legend.Position = XlLegendPosition.xlLegendPositionTop;
               // chartPage.Legend.Border.LineStyle = XlLineStyle.xlContinuous;
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.ClearFormats();
                
                //chartPage.PlotArea.Width = 394;
                //chartPage.PlotArea.Left = 30;
                //ChartGroup grp = (ChartGroup)myChart.Chart.ChartGroups(1);
                //grp.VaryByCategories = true;
                ((ChartGroup)chartPage.ChartGroups(1)).VaryByCategories = true;
                chartPage.Legend.Clear();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Delete();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinimumScale = 0.0;
               ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MaximumScale = 1.0;
               
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Text = "(%)";
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Left = 1;
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Top = 10;

                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MinimumScaleIsAuto = true;
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MaximumScaleIsAuto = true;
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MajorUnitIsAuto = true;
               // ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MinorUnitIsAuto = true;

                //((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MajorUnit = 10.0;                
                //((ChartGroup)chartPage.ChartGroups(1)).Overlap = 50;
                //((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 30;
                //((Series)chartPage.SeriesCollection(1)).Interior.ColorIndex = 30;
                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
                //((Series)chartPage.SeriesCollection(2)).Interior.ColorIndex = 8;
                //((Series)chartPage.SeriesCollection(2)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
                //((Series)chartPage.SeriesCollection(3)).Interior.ColorIndex = 25;
                //((Series)chartPage.SeriesCollection(3)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
                //chartPage.PlotArea.Height = 55;
                //chartPage.PlotArea.Top = 25;
                
                DateTime dt_img = System.DateTime.Today;
                string img_date = dt_img.ToString("MMyyyy");

                chartPage.Export(File_Path + "\\CAPITALIZATION_EXPOSURE_" + sch_id + "_" + img_date + ".png", "PNG", misValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBarchartRating(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, string Column1, string Colimn2, string sch_id, string File_Path)
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
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.ClearFormats();
               
                ((ChartGroup)chartPage.ChartGroups(1)).VaryByCategories = true;
                chartPage.Legend.Clear();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Delete();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinimumScale = 0.0;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MaximumScale = 1.0;
               // ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorUnit = 10.0;
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Text = "(%)";               
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Left = 1;
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Top = 10;

                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);
               
                DateTime dt_img = System.DateTime.Today;
                string img_date = dt_img.ToString("MMyyyy");

                chartPage.Export(File_Path + "\\CREDIT_QUALITY_RATINGS_" + sch_id + "_" + img_date + ".png", "PNG", misValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBarchartInstrument(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, string Column1, string Colimn2, string sch_id, string File_Path)
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
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight+10.0);
                Excel.Chart chartPage = myChart.Chart;

                chartPage.ChartType = XlChartType.xlColumnClustered;
                chartPage.SetSourceData(chartRange, XlRowCol.xlColumns);
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                chartPage.PlotArea.ClearFormats();

                ((ChartGroup)chartPage.ChartGroups(1)).VaryByCategories = true;
                chartPage.Legend.Clear();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Delete();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinimumScale = 0.0;
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MaximumScale = 1.0;
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Text = "(%)";               
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Left = 1;
                //((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Top = 10;

                ((Series)chartPage.SeriesCollection(1)).ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowValue, false, true, Missing.Value, false, false, true, false, false, Missing.Value);

                DateTime dt_img = System.DateTime.Today;
                string img_date = dt_img.ToString("MMyyyy");

                chartPage.Export(File_Path + "\\INSTRUMENT_BREAKUP_" + sch_id + "_" + img_date + ".png", "PNG", misValue);
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
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left,Top,Width,Hight);//2, 675, 188, 87
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
        public bool InsertBarchartPerformanceBenchmark(string ChartSheetName, string ChartDataSheetName, double Left, double Top, double Width, double Hight, int StartColumn, int EndColumn, string BenchmarkIndex, string sch_id, string file_path)
        {
            try
            {
                
                _ExcelWorkSheet = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]);
                _ExcelWorkSheet.Activate();
                Excel.Range chartRange = ((Worksheet)_ExcelWorkBook.Sheets[ChartDataSheetName]).get_Range(getColumnName(StartColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(EndColumn)) - 1), getColumnName(EndColumn) + "1");//data taken from bottom
                
                SetCellData(getColumnName(EndColumn + 1) + 1, ChartDataSheetName, "MIN-MAX");
                SetCellFormula(2, EndColumn + 1, ChartDataSheetName, "=MIN(" + getColumnName(EndColumn) + "2:" + getColumnName(EndColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartColumn)) - 1) );
                SetCellFormula(3, EndColumn + 1, ChartDataSheetName, "=MAX(" + getColumnName(EndColumn) + "2:" + getColumnName(EndColumn) + (GetFirstBlankCellRowIndexInAColumn(ChartDataSheetName, getColumnName(StartColumn)) - 1) );

                chartRange.Select();
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[ChartSheetName];
                _ExcelWorkSheet.Activate();
                
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)_ExcelWorkSheet.ChartObjects(Type.Missing);
                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(Left, Top, Width, Hight);
                Excel.Chart chartPage = myChart.Chart;
                chartPage.ChartType = XlChartType.xlColumnClustered;
                //chartPage.PlotBy =  Microsoft.Office.Interop.Excel.XlRowCol.xlRows;
                chartPage.SetSourceData(chartRange, Microsoft.Office.Interop.Excel.XlRowCol.xlColumns);
                chartPage.Legend.Delete();
                chartPage.ChartTitle.Delete();
             
                chartPage.PlotArea.ClearFormats();
                ((Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorGridlines.Delete();
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MajorTickMark = XlTickMark.xlTickMarkOutside;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).MinorTickMark = XlTickMark.xlTickMarkNone;

                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).MajorUnit = 3;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).TickLabelPosition = XlTickLabelPosition.xlTickLabelPositionLow;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).HasTitle = true;
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).AxisTitle.Text = "(%)";
                ((Axis)chartPage.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary)).ReversePlotOrder = false;
                ((ChartGroup)chartPage.ChartGroups(1)).Overlap = 0;
                ((ChartGroup)chartPage.ChartGroups(1)).GapWidth = 150;
                chartPage.ChartArea.Font.Size = 7;
                chartPage.ChartArea.Font.Name = "Calibri";
                chartPage.ChartArea.Font.FontStyle = "Regular";
                chartPage.ChartArea.Border.LineStyle = 0;
                //chartPage.PlotArea.Height = Hight;
                //chartPage.PlotArea.Width = Width;
                ((Axis)chartPage.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary)).TickLabels.NumberFormat = "[$-409]mmm-yy;@";
                Excel.Shape shape = _ExcelWorkSheet.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, (float)Left+170, (float)Top+10, 104, 14);
                shape.ZOrder(MsoZOrderCmd.msoBringToFront);
                
                _ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.Characters(Missing.Value, Missing.Value).Text = "Monthly Fund Return +/- " + BenchmarkIndex;
              
                _ExcelWorkSheet.Shapes._Default(shape.Name).Line.Visible = MsoTriState.msoFalse;
                _ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.Characters(Missing.Value, Missing.Value).Font.Name = "Calibri";
                _ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.Characters(Missing.Value, Missing.Value).Font.FontStyle = "Regular";
                _ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.Characters(Missing.Value, Missing.Value).Font.Size = 7;
                _ExcelWorkSheet.Shapes._Default(shape.Name).TextFrame.AutoSize = true;

                Excel.Axis axis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                Excel.Axis axis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                double Min = Math.Round((FetchCellData(getColumnName(EndColumn + 1) + "2", "Data").Trim() != "" ? Convert.ToDouble(FetchCellData(getColumnName(EndColumn + 1) + "2", "Data")) : axis1.MinimumScale),2);
                axis1.MinimumScale = Math.Round(Min)-1;//- Convert.ToInt64(Min.ToString().Substring(Min.ToString().Length - 1, 1));
                double Max = (FetchCellData(getColumnName(EndColumn + 1) + "3", "Data").Trim() != "" ? Convert.ToDouble(FetchCellData(getColumnName(EndColumn + 1) + "3", "Data")) : axis1.MaximumScale);
                axis1.MaximumScale = Math.Round(Max) + 1;//+ 10 - Convert.ToInt64(Max.ToString().Substring(Max.ToString().Length - 1, 1));
                
                //DateTime dt = System.DateTime.Now;
                //TimeSpan ts = new TimeSpan();
                //dt += ts;
                chartPage.PlotArea.Height = ((Hight - chartPage.PlotArea.Height) + chartPage.PlotArea.Height);// Convert.ToDouble((Hight - 5));
                chartPage.PlotArea.Width = Width;

                DateTime dt_img = System.DateTime.Today;
                string img_date = dt_img.ToString("MMyyyy");
                //chartPage.Export(@"C:\\DowJones\\DowJones_report\\PERFORMANCE_VIS-A-VIS_BENCHMARK_" + sch_id + ".png", "PNG", misValue);
                chartPage.Export(file_path+"\\PERFORMANCE_VIS-A-VIS_BENCHMARK_" + sch_id +"_"+img_date+".png", "PNG", misValue);
               
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
                chartPage.PlotArea.Height = Hight-55;
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



        /// <summary>
        /// Closes The Excel Running
        /// </summary>
        /// <param name="SaveOrNot">Save Or Not [If True Is Passed As Parameter But IsReadOnly Property Is Set To True Then You Can Not Save The Excel]</param>
        public void CloseExcel(bool SaveOrNot)
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

        public bool FindReplaceText(int Row, int Column, string SheetName, string FindText,string ReplaceText)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[Row, Column], _ExcelWorkSheet.Cells[Row, Column]).Replace(FindText, ReplaceText, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool MargeCellsData(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, string Values)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Merge(null);
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Value2 = Values;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool MargeCellsData(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, string Values, bool isHAlignmentCentre, bool isVAlignmentCentre,bool isWrapText,bool isBorder)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Merge(null);
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Value2 = Values;
                if(isHAlignmentCentre)
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                if (isVAlignmentCentre)
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                if (isWrapText)
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).WrapText = true;
                if(isBorder)
                     _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Borders.LineStyle = Excel.Constants.xlSolid;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.ColorIndex = Fontcolor;
                if (isCenter)
                {
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                }
                else
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Bold = isBold;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Size = fontsize;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Name = font;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Interior.ColorIndex = backcolorindex == null ? XlColorIndex.xlColorIndexNone : backcolorindex;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool FormatCells(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Fontcolor, int fontsize, string font, object backcolorindex, bool isBold, bool isCenter,bool isItalic)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.ColorIndex = Fontcolor;
                if (isCenter)
                {
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                }
                else
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Bold = isBold;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Size = fontsize;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Name = font;
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Interior.ColorIndex = backcolorindex == null ? XlColorIndex.xlColorIndexNone : backcolorindex;
                if(isItalic)
                    _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Italic = isItalic;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool SetNumberFormat(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName,string DecimalPrecision,string DecimalScale)
        {
            try
            {
                 _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).NumberFormat=DecimalPrecision+"."+DecimalScale;
                 return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetFontColorFromRGB(int StartRow, int StartColumn, int EndRow, int EndColumn, string SheetName, int Red,int Green,int Blue)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(Red, Green, Blue));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).Borders.LineStyle = Excel.Constants.xlSolid;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetRowHeight(int StartRow, int StartColumn, int EndRow, int EndColumn, double RowHeight, string SheetName)
        {
            try
            {
                _ExcelWorkSheet = (Worksheet)_ExcelWorkBook.Sheets[SheetName];
                _ExcelWorkSheet.get_Range(_ExcelWorkSheet.Cells[StartRow, StartColumn], _ExcelWorkSheet.Cells[EndRow, EndColumn]).RowHeight = RowHeight;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
              
    }
}