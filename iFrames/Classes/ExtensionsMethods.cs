using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames
{
    public static class ExtensionsMethods
    {
        public static bool Contains(this string[] stringList, string item)
        {
            foreach (string s in stringList)
                if (item.Equals(s))
                    return true;
            return false;
        }

        public static DataTable GetTable(this IEnumerable ien)
        {
            DataTable dt = new DataTable();
            foreach (object obj in ien)
            {
                Type t = obj.GetType();
                PropertyInfo[] pis = t.GetProperties();
                if (dt.Columns.Count == 0)
                    foreach (PropertyInfo pi in pis)
                        dt.Columns.Add(pi.Name);
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pi in pis)
                {
                    object value = pi.GetValue(obj, null);
                    dr[pi.Name] = value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private static DataTable MargeTableSideBySide(this DataTable sourceTable, DataTable secondTable, string[] primaryKeys, out string strError)
        {
            strError = "";
            if (primaryKeys.Any(column => !sourceTable.Columns.Contains(column) || !secondTable.Columns.Contains(column)))
            {
                strError = "Any one of primarykeys doesn't contain in both tables.";
                return null;
            }
            foreach (var dc in from DataColumn dc in sourceTable.Columns
                               where !primaryKeys.Contains(dc.ColumnName)
                               where secondTable.Columns.Contains(dc.ColumnName)
                               select dc)
            {
                strError = "Common column name exists in both tables, the name is " + dc.ColumnName;
                return null;
            }
            var dtOutPut = new DataTable();
            foreach (DataColumn dc in sourceTable.Columns)
                dtOutPut.Columns.Add(dc);
            foreach (var dc in secondTable.Columns.Cast<DataColumn>().Where(dc => !primaryKeys.Contains(dc.ColumnName)))
                dtOutPut.Columns.Add(dc);

            return dtOutPut;
        }

        /// <summary>
        /// Concatenates all the strings in the specified sequence.
        /// </summary>
        /// <param name="values">The values to concatenate.</param>
        /// <returns>The concatenated sequence.</returns>
        public static string Concatenate(this IEnumerable<string> values)
        {
            return values.Concatenate(string.Empty);
        }

        /// <summary>
        /// Concatenates all the strings in the specified sequence, separated by
        /// the specified separator.
        /// </summary>
        /// <param name="values">The values to concatenate.</param>
        /// <param name="separator">A string that will be inserted between all the values
        /// in the resulting string.</param>
        /// <returns>The concatenated sequence, separated by
        /// the specified separator.</returns>
        public static string Concatenate(this IEnumerable<string> values, string separator)
        {
            return values.Concatenate(separator, string.Empty, string.Empty);
        }

        /// <summary>
        /// Concatenates all the strings in the specified sequence, separated by
        /// the specified separator, prefixed by the value specified in <paramref name="prefix" /> and
        /// suffixed by the value specified in <paramref name="suffix"/>.
        /// </summary>
        /// <param name="values">The values to concatenate.</param>
        /// <param name="separator">A string that will be inserted between all the values
        /// in the resulting string.</param>
        /// <param name="prefix">A string that will be the start of the result string.</param>
        /// <param name="suffix">A string that will be the end of the result string.</param>
        /// <returns>The concatenated sequence, separated by
        /// the specified separator, prefixed by the value specified in <paramref name="prefix" /> and
        /// suffixed by the value specified in <paramref name="suffix"/>.</returns>
        public static string Concatenate(this IEnumerable<string> values, string separator, string prefix, string suffix)
        {

            var result = new StringBuilder();
            result.Append(prefix);
            foreach (var value in values)
            {
                if (result.Length > prefix.Length)
                    result.Append(separator);
                result.Append(value);
            }
            result.Append(suffix);
            return result.ToString();
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            var dtReturn = new DataTable();
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;

            foreach (var rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = rec.GetType().GetProperties();
                    foreach (var pi in oProps)
                    {
                        var colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            colType = colType.GetGenericArguments()[0];
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                var dr = dtReturn.NewRow();
                foreach (var pi in oProps)
                    dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static DataTable OrderBy(this DataTable input, string orderByClause)
        {
            var dtTemp = input.Clone();
            try
            {
                var drs = input.Select("", orderByClause);
                foreach (var dr in drs)
                    dtTemp.ImportRow(dr);
            }
            catch (Exception)
            {
                return null;
            }
            return dtTemp;
        }

        public static bool IsPrime(this int number)
        {
            if ((number % 2) == 0)
                return number == 2;
            var sqrt = (int)Math.Sqrt(number);
            for (var t = 3; t <= sqrt; t = t + 2)
                if (number % t == 0)
                    return false;
            return number != 1;
        }

        public static void ToCsvFile(this DataTable dataTable, string csvFilePath, char delimiter)
        {
            try
            {
                if (dataTable == null || dataTable.Columns.Count == 0)
                    throw new NullReferenceException("Can't export null to CSV file.", new Exception("Datatable is null or no datacolumn exists"));
                File.Delete(csvFilePath);
                using (var file = new StreamWriter(csvFilePath, false))
                {
                    var columnHeading = "";
                    columnHeading = dataTable.Columns.Cast<DataColumn>().Aggregate(columnHeading, (current, dc) => current + (dc.ColumnName + delimiter));
                    file.WriteLine(columnHeading);
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        foreach (DataColumn dc in dataTable.Columns)
                            file.Write("\"" + dr[dc].ToString().Replace("\"", "\"\"") + "\"" + delimiter);
                        file.Write(Environment.NewLine);
                    }
                }
                return;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static ITable getTableByName(this DataContext context, string tableName)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (tableName == null)
                throw new ArgumentNullException("tableName");
            return (ITable)context.GetType().GetProperty(tableName).GetValue(context, null);
        }

        public static bool isValidNumeric(this string str)
        {
            if (str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentSymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PerMilleSymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
                return true;
            return str.Aggregate(true, (current, t) => current & Char.IsDigit(t));
        }

        public static void SaveToFile(this Stream inputStream, string filePath)
        {
            using (var fileStream = File.Create(filePath, (int)inputStream.Length))
            {
                var bytesInStream = new byte[inputStream.Length];
                fileStream.Read(bytesInStream, 0, bytesInStream.Length);
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }

        public static void SaveToExcel(this DataSet InputDataSet, string filePath)
        {
            for (var i = 0; i < InputDataSet.Tables.Count; i++)
            {
                var grid = new DataGrid
                               {
                                   DataSource = InputDataSet.Tables[i],
                                   DataMember = InputDataSet.Tables[i].TableName
                               };
                grid.HeaderStyle.Font.Bold = true;
                grid.DataBind();
                using (var sw = new StreamWriter(filePath.Insert(filePath.LastIndexOf('.'), InputDataSet.Tables[i].TableName)))
                using (var hw = new HtmlTextWriter(sw))
                    grid.RenderControl(hw);
            }
        }

        public static void toExcelFile(this DataTable InputDataTbl, string filePath)
        {
            var grid = new DataGrid
                           {
                               DataSource = InputDataTbl,
                               DataMember = InputDataTbl.TableName
                           };
            grid.HeaderStyle.Font.Bold = true;
            grid.DataBind();
            using (
                var sw = new StreamWriter(filePath.Insert(filePath.LastIndexOf('.'), InputDataTbl.TableName)))
            using (var hw = new HtmlTextWriter(sw))
                grid.RenderControl(hw);
        }
    }
}