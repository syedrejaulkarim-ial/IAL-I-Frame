using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iFrames.BLL;

namespace iFrames.Classes 
{
    public class DataTableUtility
    {
        public DataTable JoinDataTable(DataTable dtMain, DataTable dtAditional, string strKeyColumn)
        {
            DataTable dtfinal = null;
            try
            {
                foreach (DataColumn dc in dtAditional.Columns)
                {
                    if (dc.ColumnName.ToUpper() == strKeyColumn.Trim().ToUpper())
                        continue;
                    dtMain.Columns.Add(dc.ColumnName, dc.DataType);
                }



                foreach (DataRow dr in dtMain.Rows)
                {
                    DataRow[] drC = dtAditional.Select(strKeyColumn + "='" + dr[strKeyColumn] + "'");
                    if (drC.Length > 0)
                    {
                        foreach (DataColumn dc in dtAditional.Columns)
                        {
                            if (dc.ColumnName.ToUpper() == strKeyColumn.Trim().ToUpper())
                                continue;
                            dr[dc.ColumnName] = drC[0][dc.ColumnName];
                        }
                    }
                }
                dtfinal = dtMain;
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message.ToString(), "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return dtfinal;
        }

        public void CopyColumns(DataTable source, DataTable dest, params string[] columns)
        {
            try
            {
                foreach (DataRow sourcerow in source.Rows)
                {
                    DataRow destRow = dest.NewRow();
                    foreach (string colname in columns)
                        destRow[colname] = sourcerow[colname];
                    dest.Rows.Add(destRow);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString(), "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public DataTable TransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }
        
        public DataTable GetEntryExitLoad(DataTable _dtSchemeId, string _SchemeIdColumnName,string _strEntryExit,  string _NullReplaceBy)
        {
            try
            {
                var _strSchemeId = String.Join(",", (from row in _dtSchemeId.AsEnumerable() select row[_SchemeIdColumnName]).ToArray().OfType<object>().Select(o => o.ToString()).ToArray());
                var _dtLoads = AllMethods.getEntryExitLoad(_strSchemeId);
                var _FinalLoads = new DataTable();
                _FinalLoads.Columns.Add("Scheme_Id"); _FinalLoads.Columns.Add("Statement"); _FinalLoads.Columns.Add("Load");
                DataRow[] _drLoads = null;
                
                if( string.IsNullOrEmpty(_strEntryExit))
                    _drLoads = _dtLoads.Copy().Select();//.Select("EE_Type='" + _strEntryExit.Trim() + "'");
                else
                    _drLoads = _dtLoads.Copy().Select("EE_Type='" + _strEntryExit.Trim() + "'");



                _dtLoads.Clear();
                foreach (DataRow _dr in _drLoads)
                    _dtLoads.ImportRow(_dr);
                foreach (DataRow drLoad in _dtLoads.Rows)
                {
                    string strStatement = "";
                    decimal _Load = 0;
                    if (drLoad["Cond_Type"].ToString().Trim() != "")
                    {
                        if (drLoad["Cond_Type"].ToString().Trim().ToUpper() == "PERIOD")
                            if (drLoad["Cond_Type"].ToString().Trim() != "" && Convert.ToInt32(Math.Round(Convert.ToDecimal(drLoad["Less_Cond"].ToString().Trim()))) == 0)
                                strStatement += " If redeemed aft. " + drLoad["Greater_cond"].ToString().Trim() + " " + drLoad["dur_type1"].ToString().Trim() + " ";
                            else
                                strStatement += " If redeemed bet. " + drLoad["Greater_cond"].ToString().Trim() + " " + drLoad["dur_type1"].ToString().Trim() + " to " + drLoad["Less_Cond"].ToString().Trim() + " " + drLoad["dur_type2"].ToString().Trim() + " ";
                        else if (drLoad["Cond_Type"].ToString().Trim().ToUpper() == "AMOUNT")
                            if (Convert.ToInt32(drLoad["Less_Cond"].ToString().Trim()) == 0)
                                strStatement += " Amt. grt. than " + Math.Round(Convert.ToDecimal(drLoad["Greater_cond"].ToString().Trim()) / 10000000, 2).ToString() + "crs.then ";
                            else
                                strStatement += " Amt. grt. than " + Math.Round(Convert.ToDecimal(drLoad["Greater_cond"].ToString().Trim()) / 10000000, 2).ToString() + "crs. to " + Math.Round(Convert.ToDecimal(drLoad["Less_Cond"].ToString().Trim()) / 10000000, 2).ToString() + "crs.then ";
                    }
                    if (strStatement.Trim() == "" && Convert.ToDecimal(drLoad["EE_Load"].ToString().Trim()) == 0)
                    {
                        strStatement += drLoad["EE_Type"].ToString().Trim() + " Load is Nil";
                        _Load = 0;
                    }
                    else if (strStatement.Trim() == "" && Convert.ToDecimal(drLoad["EE_Load"].ToString().Trim()) != 0)
                    {
                        strStatement += drLoad["EE_Type"].ToString().Trim() + " Load is " + Convert.ToDecimal(drLoad["EE_Load"].ToString().Trim()).ToString() + "%";
                        _Load = Convert.ToDecimal(drLoad["EE_Load"].ToString().Trim());
                    }
                    else if (Convert.ToDecimal(drLoad["CDSC"].ToString().Trim()) == 0)
                    {
                        strStatement += drLoad["EE_Type"].ToString().Trim() + " Load is Nil";
                        _Load = 0;
                    }
                    else
                    {
                        strStatement += drLoad["EE_Type"].ToString().Trim() + " Load is " + drLoad["CDSC"].ToString().Trim() + "%";
                        _Load = Convert.ToDecimal(drLoad["CDSC"].ToString().Trim());
                    }

                    _FinalLoads.Rows.Add(new object[] { drLoad["Scheme_Id"], strStatement, _Load });
                }

                string finalStat = "";
                DataTable _FinalResult = new DataTable();
                _FinalResult = _FinalLoads.Clone();
                foreach (string sid in _strSchemeId.Split(new char[] { ',' }))
                {
                    finalStat = "";
                    DataRow[] rows = _FinalLoads.Select("Scheme_Id='" + sid + "'", "");
                    if (rows.Count() != 0)
                    {
                        if (rows.Count() > 1)
                        {
                            foreach (DataRow dr in rows)
                            {
                                if (finalStat == "")
                                    finalStat = dr["Statement"].ToString();
                                else
                                    finalStat += "," + dr["Statement"].ToString();
                            }
                            if (finalStat != "")
                                _FinalResult.Rows.Add(new object[] { sid, finalStat, 1 });
                        }
                        else
                        {
                            _FinalResult.Rows.Add(new object[] { rows[0].ItemArray[0].ToString(), rows[0].ItemArray[1].ToString(), rows[0].ItemArray[2].ToString() });
                        }
                    }
                    else
                        _FinalResult.Rows.Add(new object[] { sid, _NullReplaceBy, _NullReplaceBy });
                }

                var _varOut = (from sch in _dtSchemeId.AsEnumerable()
                               join LOAD in _FinalResult.AsEnumerable()
                               on Convert.ToString(sch.Field<object>(_SchemeIdColumnName)).Trim()
                               equals Convert.ToString(LOAD.Field<object>("Scheme_Id")).Trim() into ords
                               from o in ords.DefaultIfEmpty()
                               select new
                               {
                                   Scheme_Id = sch.Field<object>(_SchemeIdColumnName),
                                   Statement = !Convert.IsDBNull(o.Field<object>("Statement")) ? o.Field<object>("Statement") : _NullReplaceBy,
                                   Load = !Convert.IsDBNull(o.Field<object>("Load")) ? o.Field<object>("Load") : _NullReplaceBy,
                               });
                return _varOut.ToDataTable();


            }
            catch (Exception ex)
            {
               // _Error = _Error + ex.Message.ToString();
                return null;
            }
        }

    }
}