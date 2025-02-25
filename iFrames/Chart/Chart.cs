using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using Text = System.Text;
using System.Collections;
using System.Reflection;
using System.Configuration;

//****HTML to PDF*****
using WkHtmlToXSharp;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Diagnostics;
//********************

namespace iFrames.Chart
{
    public class Chart
    {
        #region: Global Variable

        #region: Anonymous Global Variable

        string firstRowEPS = string.Empty;
        string firstRowMcap = string.Empty;
        string firstRowSharePrice = string.Empty;
        string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        SqlConnection conn = new SqlConnection();

        #endregion

        #region: PDF Global Variable

        private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string SimplePageFile = null;
        public static int count = 0;

        #endregion

        #endregion

        #region: Constructor
        public Chart()
        {
            conn.ConnectionString = connstr;
        }
        #endregion

        #region: Methods
        /// <summary>
        /// Extract data from excel function.
        /// </summary>
        /// <param name="_strFileName"></param>
        /// <returns>Datatable</returns>
        public DataTable FillGridFromExcell(string _strFileName)
        {
            DataTable dtReturned = new DataTable();
            string _ExcelFilePath = null;
            string SheetName = null;
            _ExcelFilePath = _strFileName;
            SheetName = "Sheet1";
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
        /// <summary>
        /// Fetch all data from database against a company function.
        /// </summary>
        /// <param name="_comp"></param>
        /// <returns>Datatable</returns>
        public DataTable FetchCompanyEPS(string _comp)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            SqlParameter compParam = new SqlParameter();
            try
            {

                sql = "select [Comp_Name],[FY_End],[EPS],[M_Cap],[PE],[SharePrice],[EPSGrowth],[M_CapGrowth],[Dividend_Yield] from  [Pramerica_EPS_MCap_Share] where [Comp_Name]=@comp_name order by [FY_End] asc";

                compParam.ParameterName = "@comp_name";
                compParam.SqlDbType = SqlDbType.VarChar;
                //compParam.Size = 15;
                compParam.Direction = ParameterDirection.Input;
                compParam.Value = _comp;

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(compParam);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;

        }
        /// <summary>
        /// Calculate rebase on value function.
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns>Datatable</returns>
        public DataTable CalculateRebase(DataTable _dt)
        {
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("CompanyName", typeof(System.String));
            dtReturn.Columns.Add("FY_End", typeof(System.String));
            dtReturn.Columns.Add("EPS", typeof(System.String));
            dtReturn.Columns.Add("M_Cap", typeof(System.String));
            dtReturn.Columns.Add("PE", typeof(System.String));
            dtReturn.Columns.Add("SharePrice", typeof(System.String));
            dtReturn.Columns.Add("EPSGrowth", typeof(System.String));
            dtReturn.Columns.Add("M_CapGrowth", typeof(System.String));

            Int32 count = 0;
            double minEPS = 0.0;
            double maxEPS = 0.0;
            double minMcap = 0.0;
            double maxMcap = 0.0;
            double minSharePrice = 0.0;
            double maxSharePrice = 0.0;

            try
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    DataRow _dr = dtReturn.NewRow();
                    if (count == 0)
                    {



                        List<double> levelEPS = _dt.AsEnumerable().Select(al => al.Field<double>("EPS")).Distinct().ToList();
                        minEPS = levelEPS.Min();
                        maxEPS = levelEPS.Max();
                        firstRowEPS = Convert.ToString(maxEPS);
                        firstRowEPS = _dt.Rows[0]["EPS"].ToString();

                        List<double> levelMCap = _dt.AsEnumerable().Select(al => al.Field<double>("M_Cap")).Distinct().ToList();
                        minMcap = levelMCap.Min();
                        maxMcap = levelMCap.Max();
                        firstRowMcap = _dt.Rows[0]["M_Cap"].ToString();

                        List<double> levelSharePrice = _dt.AsEnumerable().Select(al => al.Field<double>("SharePrice")).Distinct().ToList();
                        minSharePrice = levelSharePrice.Min();
                        maxSharePrice = levelSharePrice.Max();
                        firstRowSharePrice = _dt.Rows[0]["SharePrice"].ToString();


                    }
                    _dr["CompanyName"] = dr["Comp_Name"].ToString();
                    _dr["FY_End"] = dr["FY_End"].ToString();
                    _dr["EPS"] = Math.Round(Convert.ToDouble(Rebase(dr["EPS"].ToString(), minEPS, maxEPS, firstRowEPS, "eps")), 2).ToString();
                    _dr["M_Cap"] = Math.Round(Convert.ToDouble(Rebase(dr["M_Cap"].ToString(), minMcap, maxMcap, firstRowMcap, "mcap")), 2).ToString();
                    _dr["PE"] = dr["PE"].ToString();
                    _dr["SharePrice"] = Math.Round(Convert.ToDouble(Rebase(dr["SharePrice"].ToString(), minSharePrice, maxSharePrice, firstRowSharePrice, "shareprice")), 2).ToString();
                    _dr["EPSGrowth"] = dr["EPSGrowth"].ToString();
                    _dr["M_CapGrowth"] = dr["M_CapGrowth"].ToString();
                    dtReturn.Rows.Add(_dr);
                    count = count + 1;
                }


            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dtReturn;
        }
        /// <summary>
        /// Main rebase calculation function
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="check"></param>
        /// <returns>String Value</returns>
        public string Rebase(string _value, double min, double max, string firstRowValue, string check)
        {
            string ReturnValue = string.Empty;
            Int32 fix = 100;
            try
            {
                if (check == "eps")
                {
                    if (_value == "0" && firstRowValue == "0")
                    {

                        _value = "0.01";
                        firstRowValue = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else if (_value == "0")
                    {
                        _value = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else if (firstRowValue == "0")
                    {
                        firstRowValue = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else
                    {
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }

                    //ReturnValue = Convert.ToString((((Convert.ToDouble(_value) - min) / max) * fix) + 5);
                }
                else if (check == "mcap")
                {
                    if (_value == "0" && firstRowValue == "0")
                    {

                        _value = "0.01";
                        firstRowValue = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else if (_value == "0")
                    {
                        _value = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else if (firstRowValue == "0")
                    {
                        firstRowValue = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else
                    {
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    //ReturnValue = Convert.ToString((((Convert.ToDouble(_value) - min) / max) * fix) + 5);
                }
                else if (check == "shareprice")
                {

                    if (_value == "0" && firstRowValue == "0")
                    {

                        _value = "0.01";
                        firstRowValue = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else if (_value == "0")
                    {
                        _value = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else if (firstRowValue == "0")
                    {
                        firstRowValue = "0.01";
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    else
                    {
                        ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));
                    }
                    //ReturnValue = Convert.ToString((((Convert.ToDouble(_value) - min) / max) * fix) + 5);
                }
                else
                {

                }


            }
            catch (Exception ex)
            {
                throw ex;

            }
            return ReturnValue;

        }
        /// <summary>
        /// This funaction is being called to calculate CAGR
        /// </summary>
        /// <param name="dtRebasePer"></param>
        /// <returns>Datatable</returns>
        public DataTable CalculateEPSRebasePer(DataTable dtRebasePer)
        {

            double Begining_EPS_Value = 0;
            double Begining_Mcap_Value = 0;
            double Begining_SharePrice_Value = 0;

            double Ending_EPS_Value = 0;
            double Ending_Mcap_Value = 0;
            double Ending_SharePrice_Value = 0;
            double pow = Convert.ToDouble(1.0 / dtRebasePer.Rows.Count);

            DataTable dt = new DataTable();
            dt.Columns.Add("CAGR_EPS", typeof(System.String));
            dt.Columns.Add("CAGR_Mcap", typeof(System.String));
            dt.Columns.Add("CAGR_SharePrice", typeof(System.String));

            try
            {
                Begining_EPS_Value = GetBeginingValue(dtRebasePer, "EPS");
                Begining_Mcap_Value = GetBeginingValue(dtRebasePer, "M_Cap");
                Begining_SharePrice_Value = GetBeginingValue(dtRebasePer, "SharePrice");
                DataTable dtEnd = dtRebasePer.AsEnumerable().Skip(dtRebasePer.Rows.Count - 1).CopyToDataTable();

                foreach (DataRow drEnd in dtEnd.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(drEnd["EPS"])))
                    {
                        Ending_EPS_Value = Convert.ToDouble(drEnd["EPS"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(drEnd["M_Cap"])))
                    {
                        Ending_Mcap_Value = Convert.ToDouble(drEnd["M_Cap"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(drEnd["SharePrice"])))
                    {
                        Ending_SharePrice_Value = Convert.ToDouble(drEnd["SharePrice"]);
                    }
                }

                DataRow dr = dt.NewRow();
                if (Begining_EPS_Value != 0)
                {
                    dr["CAGR_EPS"] = CalCAGR(Begining_EPS_Value, Ending_EPS_Value, pow);
                }
                else
                {
                    dr["CAGR_EPS"] = 0.0;
                }

                if (Begining_Mcap_Value != 0)
                {
                    dr["CAGR_Mcap"] = CalCAGR(Begining_Mcap_Value, Ending_Mcap_Value, pow);
                }
                else
                {
                    dr["CAGR_Mcap"] = 0.0;
                }

                if (Begining_SharePrice_Value != 0)
                {
                    dr["CAGR_SharePrice"] = CalCAGR(Begining_SharePrice_Value, Ending_SharePrice_Value, pow); ;
                }
                else
                {
                    dr["CAGR_SharePrice"] = 0.0;
                }

                dt.Rows.Add(dr);

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dt;

        }
        /// <summary>
        /// Main CAGR calculation function
        /// </summary>
        /// <param name="BeginingValue"></param>
        /// <param name="EndingValue"></param>
        /// <param name="numberyear"></param>
        /// <returns>Double Value</returns>
        public double CalCAGR(double BeginingValue, double EndingValue, double numberyear)
        {
            double res = 0.0;
            double result = 0;
            try
            {
                result = (EndingValue / BeginingValue);
                res = Math.Pow(result, numberyear);
                res = res - 1;
                res = res * 100;
                res = Math.Round(res, 2);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return res;
        }
        /// <summary>
        /// Check EPS,MCap,Share Price Negetive or positive.
        /// </summary>
        /// <param name="dtRebasePer"></param>
        /// <param name="FieldName"></param>
        /// <returns>Double Value</returns>
        public double GetBeginingValue(DataTable dtRebasePer, string FieldName)
        {
            double BeginingValue = 0.0;
            bool check = false;
            DataTable dtBegin = new DataTable();

            try
            {
                for (int i = 1; i < dtRebasePer.Rows.Count; i++)
                {
                    if (i == 1)
                    {
                        dtBegin = dtRebasePer.AsEnumerable().Take(i).CopyToDataTable();
                    }
                    else
                    {
                        dtBegin = dtRebasePer.AsEnumerable().Take(i).CopyToDataTable();
                        dtBegin = dtBegin.AsEnumerable().Skip(i - 1).CopyToDataTable();
                    }

                    check = CheckPosNeg(Convert.ToString(dtBegin.Rows[0][FieldName]));
                    if (check == false)
                    {
                        dtBegin.Rows.Clear();
                        continue;
                    }
                    else
                    {
                        BeginingValue = Convert.ToDouble(dtBegin.Rows[0][FieldName]);
                        dtBegin.Rows.Clear();
                        check = false;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return BeginingValue;
        }
        /// <summary>
        /// Fetch only all company name from database.
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable FetchCompany()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Empty;
                //sql = "select * from (select count(*)as total_row,comp_name from Pramerica_EPS_MCap_Share group by comp_name)A where total_row>=10 order by comp_name asc";
                sql = "select * from (select count(*)as total_row,comp_name from Pramerica_EPS_MCap_Share group by comp_name)A where total_row>=10 and comp_name<>'Sensex' order by comp_name asc";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Insert excel data into database function.
        /// </summary>
        /// <param name="_dt"></param>
        public void InsertDataFromExcell(DataTable _dt)
        {
            int j = 0;
            string sql = string.Empty;
            Int32 count = 0;
            string filepath = string.Empty;
            filepath = HttpContext.Current.Server.MapPath("~//Chart//ErrorLog.txt");
            System.IO.File.Delete(filepath);
            using (System.IO.File.Create(filepath))
            {
            }
            try
            {
                conn.Open();
                sql = "truncate table Pramerica_EPS_MCap_Share";
                SqlCommand cmdTruncate = new SqlCommand(sql, conn);
                cmdTruncate.CommandType = CommandType.Text;
                j = cmdTruncate.ExecuteNonQuery();
                sql = string.Empty;

                string MCapGrowth = string.Empty;
                string PrevEPS = string.Empty;
                string CurrEPS = string.Empty;
                string PrevMCap = string.Empty;
                string CurrMCap = string.Empty;

                if (j == -1)
                {
                    foreach (DataRow dr in _dt.Rows)
                    {
                        sql = "INSERT INTO [Pramerica_EPS_MCap_Share]([Comp_Name],[FY_End],[EPS],[M_Cap],[PE],[SharePrice],[EPSGrowth],[M_CapGrowth],[Dividend_Yield]) VALUES('" + HandleQuate(Convert.ToString(dr["CompanyName"])) + "','" + Convert.ToDateTime(dr["FY_End"]) + "',EPS1,M_cap1,PE1,SharePrice1,0,0,Dividend_Yield1)";



                        if (dr["EPS"] != DBNull.Value)
                        {
                            sql = sql.Replace("EPS1", Math.Round(Convert.ToDouble(dr["EPS"]), 2).ToString());
                        }
                        else
                        {
                            sql = sql.Replace("EPS1", Math.Round(Convert.ToDouble(0), 2).ToString());
                            string value = Convert.ToString(dr["EPS"]);
                            ErrorLog(sql, "EPS value problem, EPS value is: " + value, count);
                        }

                        if (dr["M_cap"] != DBNull.Value)
                        {
                            sql = sql.Replace("M_cap1", Math.Round(Convert.ToDouble(dr["M_cap"]), 2).ToString());
                        }
                        else
                        {
                            sql = sql.Replace("M_cap1", Math.Round(Convert.ToDouble(0), 2).ToString());
                            string value = Convert.ToString(dr["M_cap"]);
                            ErrorLog(sql, "M Cap value problem, M Cap value is: " + value, count);
                        }

                        if (dr["PE"] != DBNull.Value)
                        {
                            sql = sql.Replace("PE1", Math.Round(Convert.ToDouble(dr["PE"]), 2).ToString());
                        }
                        else
                        {
                            //sql = sql.Replace("PE1", Math.Round(Convert.ToDouble(0), 2).ToString());
                            sql = sql.Replace("PE1", "'N.A'");
                            string value = Convert.ToString(dr["PE"]);
                            ErrorLog(sql, "PE value problem, PE value is: " + value, count);
                        }

                        if (dr["SharePrice"] != DBNull.Value)
                        {
                            sql = sql.Replace("SharePrice1", Math.Round(Convert.ToDouble(dr["SharePrice"]), 2).ToString());
                        }
                        else
                        {
                            sql = sql.Replace("SharePrice1", Math.Round(Convert.ToDouble(0), 2).ToString());
                            string value = Convert.ToString(dr["SharePrice"]);
                            ErrorLog(sql, "SharePrice value problem, SharePrice value is: " + value, count);
                        }

                        if (dr["Dividend_Yield"] != DBNull.Value)
                        {
                            sql = sql.Replace("Dividend_Yield1", Math.Round(Convert.ToDouble(dr["Dividend_Yield"]), 2).ToString());
                        }
                        else
                        {
                            //sql = sql.Replace("Dividend_Yield1", Math.Round(Convert.ToDouble(0), 2).ToString());
                            sql = sql.Replace("Dividend_Yield1", "'N.A'");
                            string value = Convert.ToString(dr["Dividend_Yield"]);
                            ErrorLog(sql, "Divident value problem, SharePrice value is: " + value, count);
                        }

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.CommandType = CommandType.Text;
                        int i = cmd.ExecuteNonQuery();
                        sql = string.Empty;
                        count = count + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(sql, ex.ToString(), count);
            }
            finally
            {
                conn.Close();
            }
        }
        public void InsertDataFromExcellToDb(DataTable _dt)
        {
            int j = 0;
            string sql = string.Empty;
            Int32 count = 0;
            string filepath = string.Empty;
            filepath = HttpContext.Current.Server.MapPath("~//Chart//ErrorLog.txt");
            System.IO.File.Delete(filepath);
            using (System.IO.File.Create(filepath))
            {
            }
            try
            {
                conn.Open();
                sql = "truncate table Pramerica_EPS_MCap_Share";
                SqlCommand cmdTruncate = new SqlCommand(sql, conn);
                cmdTruncate.CommandType = CommandType.Text;
                j = cmdTruncate.ExecuteNonQuery();
                sql = string.Empty;

                string MCapGrowth = string.Empty;
                string PrevEPS = string.Empty;
                string CurrEPS = string.Empty;
                string PrevMCap = string.Empty;
                string CurrMCap = string.Empty;

                SqlParameter compnameParam = null;
                SqlParameter fyendParam = null;
                SqlParameter epsParam = null;
                SqlParameter mcapdParam = null;
                SqlParameter peParam = null;
                SqlParameter sharepriceParam = null;
                SqlParameter epsgrowthParam = null;
                SqlParameter mcapgrowthParam = null;
                SqlParameter dividendParam = null;



                if (j == -1)
                {
                    foreach (DataRow dr in _dt.Rows)
                    {
                        compnameParam = new SqlParameter();
                        fyendParam = new SqlParameter();
                        epsParam = new SqlParameter();
                        mcapdParam = new SqlParameter();
                        peParam = new SqlParameter();
                        sharepriceParam = new SqlParameter();
                        epsgrowthParam = new SqlParameter();
                        mcapgrowthParam = new SqlParameter();
                        dividendParam = new SqlParameter();

                        sql = "INSERT INTO [Pramerica_EPS_MCap_Share]([Comp_Name],[FY_End],[EPS],[M_Cap],[PE],[SharePrice],[EPSGrowth],[M_CapGrowth],[Dividend_Yield]) VALUES(@Comp_Name,@FY_End,@EPS,@M_cap,@PE,@SharePrice,@EPSGrowth,@M_CapGrowth,@Dividend_Yield)";

                        compnameParam.ParameterName = "@Comp_Name";
                        compnameParam.SqlDbType = SqlDbType.NVarChar;
                        //compnameParam.Size = 500;
                        compnameParam.Direction = ParameterDirection.Input;
                        compnameParam.Value = HandleQuate(Convert.ToString(dr["CompanyName"]));

                        fyendParam.ParameterName = "@FY_End";
                        fyendParam.SqlDbType = SqlDbType.VarChar;
                        //fyendParam.Size = 15;
                        fyendParam.Direction = ParameterDirection.Input;
                        fyendParam.Value = Convert.ToDateTime(dr["FY_End"]).ToString();

                        epsParam.ParameterName = "@EPS";
                        epsParam.SqlDbType = SqlDbType.VarChar;
                        //epsParam.Size = 15;
                        epsParam.Direction = ParameterDirection.Input;

                        if (dr["EPS"] != DBNull.Value)
                        {
                            epsParam.Value = Math.Round(Convert.ToDouble(dr["EPS"]), 2).ToString();
                        }
                        else
                        {
                            epsParam.Value = Math.Round(Convert.ToDouble(0), 2).ToString();
                            string value = Convert.ToString(dr["EPS"]);
                            ErrorLog(sql, "EPS value problem, EPS value is: " + value, count);
                        }

                        mcapdParam.ParameterName = "@M_cap";
                        mcapdParam.SqlDbType = SqlDbType.VarChar;
                        //mcapdParam.Size = 15;
                        mcapdParam.Direction = ParameterDirection.Input;
                        if (dr["M_cap"] != DBNull.Value)
                        {
                            mcapdParam.Value = Math.Round(Convert.ToDouble(dr["M_cap"]), 2).ToString();
                        }
                        else
                        {
                            mcapdParam.Value = Math.Round(Convert.ToDouble(0), 2).ToString();
                            string value = Convert.ToString(dr["M_cap"]);
                            ErrorLog(sql, "M Cap value problem, M Cap value is: " + value, count);
                        }

                        peParam.ParameterName = "@PE";
                        peParam.SqlDbType = SqlDbType.VarChar;
                        //peParam.Size = 15;
                        peParam.Direction = ParameterDirection.Input;
                        if (dr["PE"] != DBNull.Value)
                        {
                            peParam.Value = Math.Round(Convert.ToDouble(dr["PE"]), 2).ToString();
                        }
                        else
                        {
                            peParam.Value = "N.A";
                            string value = Convert.ToString(dr["PE"]);
                            ErrorLog(sql, "PE value problem, PE value is: " + value, count);
                        }

                        sharepriceParam.ParameterName = "@SharePrice";
                        sharepriceParam.SqlDbType = SqlDbType.VarChar;
                        //sharepriceParam.Size = 15;
                        sharepriceParam.Direction = ParameterDirection.Input;
                        if (dr["SharePrice"] != DBNull.Value)
                        {
                            sharepriceParam.Value = Math.Round(Convert.ToDouble(dr["SharePrice"]), 2).ToString();
                        }
                        else
                        {
                            sharepriceParam.Value = Math.Round(Convert.ToDouble(0), 2).ToString();
                            string value = Convert.ToString(dr["SharePrice"]);
                            ErrorLog(sql, "SharePrice value problem, SharePrice value is: " + value, count);
                        }

                        epsgrowthParam.ParameterName = "@EPSGrowth";
                        epsgrowthParam.SqlDbType = SqlDbType.VarChar;
                        //epsgrowthParam.Size = 15;
                        epsgrowthParam.Direction = ParameterDirection.Input;
                        epsgrowthParam.Value = "0";

                        mcapgrowthParam.ParameterName = "@M_CapGrowth";
                        mcapgrowthParam.SqlDbType = SqlDbType.VarChar;
                        // mcapgrowthParam.Size = 15;
                        mcapgrowthParam.Direction = ParameterDirection.Input;
                        mcapgrowthParam.Value = "0";

                        dividendParam.ParameterName = "@Dividend_Yield";
                        dividendParam.SqlDbType = SqlDbType.VarChar;
                        //dividendParam.Size = 15;
                        dividendParam.Direction = ParameterDirection.Input;
                        if (dr["Dividend_Yield"] != DBNull.Value)
                        {
                            dividendParam.Value = Math.Round(Convert.ToDouble(dr["Dividend_Yield"]), 2).ToString();
                        }
                        else
                        {
                            dividendParam.Value = "N.A";
                            string value = Convert.ToString(dr["Dividend_Yield"]);
                            ErrorLog(sql, "Divident value problem, SharePrice value is: " + value, count);
                        }
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(compnameParam);
                        cmd.Parameters.Add(fyendParam);
                        cmd.Parameters.Add(epsParam);
                        cmd.Parameters.Add(mcapdParam);
                        cmd.Parameters.Add(peParam);
                        cmd.Parameters.Add(sharepriceParam);
                        cmd.Parameters.Add(epsgrowthParam);
                        cmd.Parameters.Add(mcapgrowthParam);
                        cmd.Parameters.Add(dividendParam);
                        int i = cmd.ExecuteNonQuery();
                        sql = string.Empty;
                        count = count + 1;

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(sql, ex.ToString(), count);
            }
            finally
            {
                conn.Close();
            }

        }
        /// <summary>
        /// Handle string value at the time of excel data insertion function.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string HandleQuate(string str)
        {
            return str.Replace("'", "''");
        }
        /// <summary>
        /// Calculate growth  percentage function.
        /// </summary>
        /// <param name="dtChart"></param>
        /// <returns>Datatable</returns>
        public DataTable CalGrowth(DataTable dtChart)
        {
            DataTable dtGrowth = new DataTable();
            dtGrowth.Columns.Add("Comp_Name", typeof(System.String));
            dtGrowth.Columns.Add("FY_End", typeof(System.String));
            dtGrowth.Columns.Add("EPS", typeof(System.String));
            dtGrowth.Columns.Add("M_Cap", typeof(System.String));
            dtGrowth.Columns.Add("PE", typeof(System.String));
            dtGrowth.Columns.Add("SharePrice", typeof(System.String));
            dtGrowth.Columns.Add("EPSGrowth", typeof(System.String));
            dtGrowth.Columns.Add("M_CapGrowth", typeof(System.String));
            dtGrowth.Columns.Add("Dividend_Yield", typeof(System.String));
            dtGrowth.Columns.Add("SharePriceGrowth", typeof(System.String));

            try
            {
                //dtGrowth = dtChart.Clone();

                int count = 0;
                string MCapGrowth = string.Empty;
                string PrevEPS = string.Empty;
                string CurrEPS = string.Empty;
                string PrevMCap = string.Empty;
                string CurrMCap = string.Empty;
                string PrevSharePrice = string.Empty;
                string CurrSharePrice = string.Empty;

                foreach (DataRow dr in dtChart.Rows)
                {

                    if (count == 0)
                    {

                        DataRow dr1 = dtGrowth.NewRow();
                        dr1["Comp_Name"] = dr["Comp_Name"].ToString();
                        dr1["FY_End"] = dr["FY_End"].ToString();
                        dr1["EPS"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["EPS"].ToString()))).ToString();
                        dr1["M_Cap"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["M_Cap"].ToString()))).ToString();
                        if (Convert.ToString(dr["PE"]) != "N.A")
                        {
                            dr1["PE"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["PE"].ToString()))).ToString();
                        }
                        else
                        {
                            dr1["PE"] = Convert.ToString(dr["PE"]);
                        }
                        dr1["SharePrice"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["SharePrice"].ToString()))).ToString();
                        dr1["EPSGrowth"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["EPSGrowth"].ToString()))).ToString();
                        dr1["M_CapGrowth"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["M_CapGrowth"].ToString()))).ToString();
                        dr1["Dividend_Yield"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["Dividend_Yield"].ToString()))).ToString();
                        dr1["SharePriceGrowth"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal("0"))).ToString(); ;
                        dtGrowth.Rows.Add(dr1);
                        PrevSharePrice = dr["SharePrice"].ToString(); ;
                        PrevEPS = dr["EPS"].ToString();
                        PrevMCap = dr["M_Cap"].ToString();
                    }
                    else
                    {
                        DataRow dr1 = dtGrowth.NewRow();
                        dr1["Comp_Name"] = dr["Comp_Name"].ToString();
                        dr1["FY_End"] = dr["FY_End"].ToString();
                        dr1["EPS"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["EPS"].ToString()))).ToString();
                        dr1["M_Cap"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["M_Cap"].ToString()))).ToString();
                        if (Convert.ToString(dr["PE"]) != "N.A")
                        {
                            dr1["PE"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["PE"].ToString()))).ToString();
                        }
                        else
                        {
                            dr1["PE"] = Convert.ToString(dr["PE"]);
                        }
                        dr1["SharePrice"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["SharePrice"].ToString()))).ToString();
                        string EPS_Growth = CalEPSGrowth(PrevEPS, Convert.ToString(dr["EPS"]));

                        Double number;
                        bool result = Double.TryParse(EPS_Growth, out number);
                        if (result)
                        {
                            dr1["EPSGrowth"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(EPS_Growth))).ToString();
                        }
                        else
                        {
                            dr1["EPSGrowth"] = EPS_Growth;
                        }
                        string MCap_Growth = CalMCapGrowth(PrevMCap, Convert.ToString(dr["M_cap"]));
                        dr1["M_CapGrowth"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(MCap_Growth))).ToString();
                        string SharePrice_Growth = CalSharePriceGrowth(PrevSharePrice, Convert.ToString(dr["SharePrice"]));
                        dr1["SharePriceGrowth"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(SharePrice_Growth))).ToString();
                        dr1["Dividend_Yield"] = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(dr["Dividend_Yield"].ToString()))).ToString();
                        dtGrowth.Rows.Add(dr1);
                        PrevEPS = Convert.ToString(dr["EPS"]);
                        PrevMCap = Convert.ToString(dr["M_cap"]);
                        PrevSharePrice = Convert.ToString(dr["SharePrice"]); ;
                    }
                    count = count + 1;

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dtGrowth;
        }
        /// <summary>
        /// Main EPS gowth calculation function per year wise
        /// </summary>
        /// <param name="_PrevEPS"></param>
        /// <param name="_CurEPS"></param>
        /// <returns>String Value</returns>
        private string CalEPSGrowth(string _PrevEPS, string _CurEPS)
        {
            double EPSDiff;
            string EPSGrowth = string.Empty;
            try
            {

                EPSDiff = Convert.ToDouble(_CurEPS) - Convert.ToDouble(_PrevEPS);
                EPSGrowth = Convert.ToString((EPSDiff * 100) / Convert.ToDouble(_PrevEPS));
                EPSGrowth = Convert.ToString(Math.Round(Convert.ToDouble(EPSGrowth), 2));

                if (EPSGrowth == "Infinity")
                {
                    EPSGrowth = "N.A";
                }

                if (_PrevEPS == "0" && _CurEPS == "0")
                {
                    EPSGrowth = "0";
                }

                if (EPSGrowth == "0")
                {
                    // No change between previous and current value.Thats why "EPSGrowth" set to -1
                    EPSGrowth = "-1";
                }



            }
            catch (Exception ex)
            {
                throw ex;

            }
            return EPSGrowth;
        }
        /// <summary>
        /// Main M Cap gowth calculation function per year wise
        /// </summary>
        /// <param name="_PrevMCap"></param>
        /// <param name="_CurMCap"></param>
        /// <returns>String</returns>
        private string CalMCapGrowth(string _PrevMCap, string _CurMCap)
        {
            double MCapDiff;
            string MCapGrowth = string.Empty;
            try
            {
                MCapDiff = Convert.ToDouble(_CurMCap) - Convert.ToDouble(_PrevMCap);
                MCapGrowth = Convert.ToString((MCapDiff * 100) / Convert.ToDouble(_PrevMCap));
                MCapGrowth = Convert.ToString(Math.Round(Convert.ToDouble(MCapGrowth), 2));

                if (MCapGrowth == "Infinity")
                {
                    MCapGrowth = "0";
                }

                if (_PrevMCap == "0" && _CurMCap == "0")
                {
                    MCapGrowth = "0";
                }

                if (MCapGrowth == "0")
                {
                    // No change between previous and current value.Thats why "MCapGrowth" set to -1
                    MCapGrowth = "-1";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return MCapGrowth;
        }
        /// <summary>
        /// Main Share Price growth calculation function per year wise
        /// </summary>
        /// <param name="_PrevMCap"></param>
        /// <param name="_CurMCap"></param>
        /// <returns>String</returns>
        private string CalSharePriceGrowth(string _PrevSharePrice, string _CurSharePrice)
        {
            double SharePriceDiff;
            string SharePriceGrowth = string.Empty;
            try
            {
                SharePriceDiff = Convert.ToDouble(_CurSharePrice) - Convert.ToDouble(_PrevSharePrice);
                SharePriceGrowth = Convert.ToString((SharePriceDiff * 100) / Convert.ToDouble(_PrevSharePrice));
                SharePriceGrowth = Convert.ToString(Math.Round(Convert.ToDouble(SharePriceGrowth), 2));

                if (SharePriceGrowth == "Infinity")
                {
                    SharePriceGrowth = "0";
                }

                if (_PrevSharePrice == "0" && _CurSharePrice == "0")
                {
                    SharePriceGrowth = "0";
                }

                if (SharePriceGrowth == "0")
                {
                    // No change between previous and current value.Thats why "SharePriceGrowth" set to -1
                    SharePriceGrowth = "-1";
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return SharePriceGrowth;
        }
        /// <summary>
        /// Fetch max min FY_END function
        /// </summary>
        /// <param name="check"></param>
        /// <returns>String Value</returns>
        public string FetchMaxMinYear(string check)
        {
            DataTable dt = new DataTable();
            string num = string.Empty;
            try
            {
                if (check == "min")
                {
                    SqlCommand cmd = new SqlCommand("select min(FY_End) from Pramerica_EPS_MCap_Share", conn);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    num = dt.Rows[0][0].ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select max(FY_End) from Pramerica_EPS_MCap_Share", conn);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    num = dt.Rows[0][0].ToString();
                }


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return num;
        }
        /// <summary>
        /// Error logging for excel file function
        /// </summary>
        /// <param name="query"></param>
        /// <param name="errormsg"></param>
        /// <param name="count"></param>
        public void ErrorLog(string query, string errormsg, Int32 count)
        {

            try
            {

                string filepath = HttpContext.Current.Server.MapPath("~//Chart//ErrorLog.txt");
                if (System.IO.File.Exists(filepath))
                {
                    System.Text.StringBuilder sb = new Text.StringBuilder();
                    sb.AppendLine("Line Number");
                    sb.AppendLine(" ");
                    sb.AppendLine(" ");
                    sb.AppendLine(Convert.ToString(count + 1));
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
                    //System.IO.File.WriteAllText(filepath, sb.ToString());

                }

            }
            catch (Exception ex)
            {


            }
        }
        /// <summary>
        /// Check positive or negetive value function
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Bool value</returns>
        public bool CheckPosNeg(string value)
        {
            bool res = false;
            try
            {
                bool positive = Convert.ToDouble(value) > Convert.ToDouble(0);
                res = positive;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return res;

        }
        #endregion

        #region: DropDown Method
        /// <summary>
        /// Fill company dropdown function.
        /// </summary>
        /// <param name="ddl"></param>
        public void FillDropdown(Control ddl)
        {
            try
            {
                DataTable dt = FetchCompany();
                if (dt.Rows.Count > 0)
                {
                    DropDownList drpdwn = (DropDownList)ddl;
                    drpdwn.DataSource = dt;
                    drpdwn.DataTextField = "Comp_Name";
                    drpdwn.DataValueField = "Comp_Name";
                    drpdwn.DataBind();
                    drpdwn.Items.Insert(0, new ListItem("-Select Company-", "0"));
                    drpdwn.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {


            }
        }
        #endregion

        #region: Add WaterMark inside image
        /// <summary>
        /// Write total year against a company on  bydirectional arrow image function.
        /// </summary>
        /// <param name="watermarkText"></param>
        /// <param name="_imagename"></param>
        public void AddWatermark(string watermarkText, string _imagename)
        {
            try
            {
                System.Drawing.Image bitmap = (System.Drawing.Image)Bitmap.FromFile(HttpContext.Current.Server.MapPath("~//Chart//images//imagesboth.jpeg")); // set image
                Font font = new Font("Verdana", 55, FontStyle.Bold, GraphicsUnit.Pixel);
                Color color = Color.White;
                Point atpoint = new Point(bitmap.Width / 2, bitmap.Height / 2);
                SolidBrush brush = new SolidBrush(color);
                Graphics graphics = Graphics.FromImage(bitmap);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                graphics.DrawString(watermarkText, font, brush, atpoint, sf);
                graphics.Dispose();
                MemoryStream m = new MemoryStream();
                //bitmap.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Save(HttpContext.Current.Server.MapPath("~//Chart//images//" + _imagename));
                //m.WriteTo(Response.OutputStream);
                //m.Dispose();
                //base.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region: Old Code
        public DataTable CalculateEPSRebasePerSP(DataTable dtRebasePer)
        {

            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Pramerica_FetchRebasePer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dt;

        }
        public void InsertDataFromExcellSP(DataTable _dt)
        {
            int j = 0;
            try
            {
                conn.ConnectionString = connstr;
                conn.Open();
                SqlCommand cmdTruncate = new SqlCommand("Pramerica_TruncateCompany", conn);
                cmdTruncate.CommandType = CommandType.StoredProcedure;
                j = cmdTruncate.ExecuteNonQuery();

                if (j == -1)
                {
                    foreach (DataRow dr in _dt.Rows)
                    {
                        SqlCommand cmd = new SqlCommand("Pramerica_InsertCompany", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Comp_Name", SqlDbType.NVarChar).Value = dr["CompanyName"];
                        cmd.Parameters.Add("@FY_End", SqlDbType.DateTime).Value = dr["FY_End"];
                        cmd.Parameters.Add("@EPS", SqlDbType.Float).Value = dr["EPS"];
                        cmd.Parameters.Add("@M_Cap", SqlDbType.Float).Value = dr["M_cap"];
                        cmd.Parameters.Add("@PE", SqlDbType.Float).Value = dr["PE"];
                        cmd.Parameters.Add("@SharePrice", SqlDbType.Float).Value = dr["SharePrice"];
                        if (!string.IsNullOrEmpty(dr["EPSGrowth"].ToString()))
                        {
                            cmd.Parameters.Add("@EPSGrowth", SqlDbType.Float).Value = dr["EPSGrowth"];
                        }
                        else
                        {
                            cmd.Parameters.Add("@EPSGrowth", SqlDbType.Float).Value = Convert.ToDouble("0");
                        }

                        if (!string.IsNullOrEmpty(dr["M_CapGrowth"].ToString()))
                        {
                            cmd.Parameters.Add("@M_CapGrowth", SqlDbType.Float).Value = dr["M_CapGrowth"];
                        }
                        else
                        {
                            cmd.Parameters.Add("@M_CapGrowth", SqlDbType.Float).Value = Convert.ToDouble("0");
                        }


                        int i = cmd.ExecuteNonQuery();
                    }
                }
                //lblMsg.Text = "FileUploaded Succesfully.";

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion

        #region: Download Method
        public void DownloadFile(string downloadPath, bool forceDownload)
        {
            try
            {
                //string path = HttpContext.Current.Server.MapPath(fname);
                string path = downloadPath;
                //string name = Path.GetFileName(path);
                string name ="BigDaySip.pdf";
                string ext = Path.GetExtension(path);
                string type = "";
                // set known types based on file extension  
                if (ext != null)
                {
                    switch (ext.ToLower())
                    {

                        case ".asf":
                            type = "video/x-ms-asf";
                            break;
                        case ".avi":
                            type = "video/avi";
                            break;
                        case ".doc":
                            type = "application/msword";
                            break;
                        case ".zip":
                            type = "application/zip";
                            break;
                        case ".xls":
                            type = "application/vnd.ms-excel";
                            break;
                        case ".gif":
                            type = "image/gif";
                            break;
                        case ".jpg":
                        case "jpeg":
                            type = "image/jpeg";
                            break;
                        case ".wav":
                            type = "audio/wav";
                            break;
                        case ".mp3":
                            type = "audio/mpeg3";
                            break;
                        case ".mpg":
                        case "mpeg":
                            type = "video/mpeg";
                            break;
                        case ".rtf":
                            type = "application/rtf";
                            break;
                        case ".htm":
                        case ".html":
                            type = "text/HTML";
                            break;
                        case ".txt":
                            type = "text/plain";
                            break;
                        default:
                            //Handle All Other Files
                            type = "application/octet-stream";
                            break;

                    }
                }
                if (forceDownload)
                {
                    HttpContext.Current.Response.AppendHeader("content-disposition",
                        "attachment; filename=" + name);
                }
                if (type != "")
                    HttpContext.Current.Response.ContentType = type;
                HttpContext.Current.Response.WriteFile(path);
                HttpContext.Current.Response.Flush();

                //string DirPath = Path.GetDirectoryName(downloadPath);
                //string[] filePaths = Directory.GetFiles(DirPath);
                //foreach (string filePath in filePaths)
                //    File.Delete(filePath);
                // HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Downloadpdf(string filename)
        {
            try
            {
                byte[] temp = (byte[])HttpContext.Current.Session["pdfcontent"];
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("Byte count: " + temp.Length.ToString());
                //HttpContext.Current.Response.ContentType = "application/octet-stream";
                //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                //HttpContext.Current.Response.BinaryWrite(temp);
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
            catch (Exception ex)
            {


            }
        }
        #endregion

        #region: Create HTML Methods

        public void CreateHTML(GridView dgv, string CompanyName, string DistributorName, ArrayList arr, string checker)
        {
            System.Text.StringBuilder strHTML = new System.Text.StringBuilder();
            try
            {
                if (checker == "EPS_Share")
                {
                    strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Chart/Templates/ShareTemplate.htm")));
                }
                else
                {
                    strHTML.Append(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Chart/Templates/McapTemplate.htm")));
                }

                strHTML = strHTML.Replace("<!CompanyName!>", CompanyName);

                string DistributorLogoPath = string.Empty;
                string imgname = string.Empty;
                if (!string.IsNullOrEmpty(DistributorName))
                {
                    imgname = DistributorName.Replace(" ", "_") + ".jpeg";
                    DistributorLogoPath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\images\\DistributorLogo\\" + imgname;
                    if (File.Exists(DistributorLogoPath))
                    {
                        imgname = "../images/DistributorLogo/" + imgname;
                        strHTML = strHTML.Replace("<!DistributorLogo!>", "<img alt='' src='" + imgname + "' width='100' height='100' />");
                    }

                }


                if (checker == "EPS_Share")
                {
                    if (CompanyName == "Sensex")
                    {
                        strHTML = strHTML.Replace(" <!Sensex!>", " Sensex");
                    }
                    else
                    {
                        strHTML = strHTML.Replace(" <!Sensex!>", " Share Price");
                    }
                }


                strHTML = strHTML.Replace("<!DistributorName!>", DistributorName);
                if (arr.Count > 0)
                {
                    strHTML = strHTML.Replace("<!GrowthSince!>", arr[0].ToString());
                    strHTML = strHTML.Replace("<!EPSPER!>", arr[1].ToString());
                    strHTML = strHTML.Replace("<!PER!>", arr[2].ToString());

                }
                if (checker == "EPS_Share")
                {
                    //strHTML = strHTML.Replace("<!ImageSource!>", "../images/" + CompanyName + "_EPS_Share.Jpeg");
                    strHTML = strHTML.Replace("<!ImageSource!>", "http://mfiframes.mutualfundsindia.com/Chart/images/" + CompanyName + "_EPS_Share.Jpeg");
                    
                }
                else
                {
                    //strHTML = strHTML.Replace("<!ImageSource!>", "../images/" + CompanyName + "_EPS_Mcap.Jpeg");
                    strHTML = strHTML.Replace("<!ImageSource!>", "http://mfiframes.mutualfundsindia.com/Chart/images/" + CompanyName + "_EPS_Mcap.Jpeg");
                }

                string table = FillHtmlTable(dgv, CompanyName, checker);
                strHTML = strHTML.Replace("<!List!>", table);


                string path = string.Empty;

                if (checker == "EPS_Share")
                {
                    path = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf/" + CompanyName + "_EPS_Share.htm");
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    using (File.Create(path))
                    {

                    }
                    File.WriteAllText(path, strHTML.ToString());
                }
                else
                {
                    path = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf/" + CompanyName + "_EPS_Mcap.htm");

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    using (File.Create(path))
                    {

                    }
                    File.WriteAllText(path, strHTML.ToString());
                }
                strHTML = null;
            }
            catch (Exception ex)
            {
                strHTML = null;
                throw ex;
            }

        }
        private string FillHtmlTable1(DataTable dt, string company, string checker)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                sb.AppendLine("<table cellspacing='0' cellpadding='4' border='1' style='background-color: White; border-color: #3366CC; border-width: 1px; border-style: None;font-  size: Small; width: 95%; border-collapse: collapse;' id='gvCompanyShare' rules='all'>");
                sb.AppendLine("<tbody>");
                sb.AppendLine("<tr style='color:White; background-color: #007AC2; font-family: Verdana; font-size: 9pt;font-weight: bold;'>");
                sb.AppendLine("<th style='width: 5%;' scope='col'>FY End</th>");
                if (checker == "EPS_Share")
                {
                    if (company == "Sensex")
                    {
                        sb.AppendLine("<th style='width: 10%;' scope='col'>Sensex EPS</th>");
                    }
                    else
                    {
                        sb.AppendLine("<th style='width: 10%;' scope='col'>EPS</th>");
                    }
                }
                else
                {
                    sb.AppendLine("<th style='width: 10%;' scope='col'>EPS</th>");
                }

                sb.AppendLine("<th style='width: 10%;' scope='col'>YoY EPS Growth (%) </th>");

                if (checker == "EPS_Share")
                {
                    sb.AppendLine("<th style='width: 10%;' scope='col'>Share Price</th>");
                }
                else
                {
                    sb.AppendLine("<th style='width: 10%;' scope='col'>M Cap</th>");
                }

                if (checker == "EPS_Share")
                {
                    sb.AppendLine("<th style='width: 15%;' scope='col'>YoY Share Price Growth (%)</th>");
                }
                else
                {
                    sb.AppendLine("<th style='width: 15%;' scope='col'>YoY M Cap Growth (%)</th>");
                }

                if (checker == "EPS_Share")
                {
                    sb.AppendLine("<th style='width: 10%;' scope='col'>Dividend Yield (%) </th>");
                }

                sb.AppendLine("<th style='width: 10%;' scope='col'>PE Ratio</th>");
                sb.AppendLine("</tr>");


                foreach (DataRow dr in dt.Rows)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 5%;'>");
                    sb.AppendLine(Convert.ToDateTime(dr["FY_End"]).ToString("yyyy"));
                    sb.AppendLine("</td>");

                    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                    sb.AppendLine(Convert.ToString(dr["EPS"]));
                    sb.AppendLine("</td>");

                    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                    if (Convert.ToString(dr["EPSGrowth"]) != "0.00")
                    {
                        sb.AppendLine(Convert.ToString(dr["EPSGrowth"]));
                    }
                    else
                    {
                        sb.AppendLine(string.Empty);
                    }

                    sb.AppendLine("</td>");

                    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                    if (checker == "EPS_Share")
                    {
                        sb.AppendLine(Convert.ToString(dr["SharePrice"]));
                    }
                    else
                    {
                        sb.AppendLine(Convert.ToString(dr["M_Cap"]));
                    }

                    sb.AppendLine("</td>");

                    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                    if (checker == "EPS_Share")
                    {
                        if (Convert.ToString(dr["SharePriceGrowth"]) != "0.00")
                        {
                            sb.AppendLine(Convert.ToString(dr["SharePriceGrowth"]));
                        }
                        else
                        {
                            sb.AppendLine(string.Empty);
                        }

                    }
                    else
                    {
                        if (Convert.ToString(dr["M_CapGrowth"]) != "0.00")
                        {
                            sb.AppendLine(Convert.ToString(dr["M_CapGrowth"]));
                        }
                        else
                        {
                            sb.AppendLine(string.Empty);
                        }

                    }

                    sb.AppendLine("</td>");

                    if (checker == "EPS_Share")
                    {
                        sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                        sb.AppendLine(Convert.ToString(dr["Dividend_Yield"]));
                        sb.AppendLine("</td>");
                    }

                    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                    sb.AppendLine(Convert.ToString(dr["PE"]));
                    sb.AppendLine("</td>");

                    sb.AppendLine("</tr>");

                }
                sb.AppendLine("</tbody></table>");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private string FillHtmlTable(GridView dgv, string company, string checker)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                //sb.AppendLine("<table cellspacing='0' cellpadding='4' border='1' style='background-color: White; border-color: #3366CC; border-width: 1px; border-style: None;font-  size: Small; width: 95%; border-collapse: collapse;' id='gvCompanyShare' rules='all'>");
                //sb.AppendLine("<tbody>");
                //sb.AppendLine("<tr style='color:White; background-color: #007AC2; font-family: Verdana; font-size: 9pt;font-weight: bold;'>");
                //sb.AppendLine("<th style='width: 5%;' scope='col'>FY End</th>");
                //if (checker == "EPS_Share")
                //{
                //    if (company == "Sensex")
                //    {
                //        sb.AppendLine("<th style='width: 10%;' scope='col'>Sensex EPS</th>");
                //    }
                //    else
                //    {
                //        sb.AppendLine("<th style='width: 10%;' scope='col'>EPS</th>");
                //    }
                //}
                //else
                //{
                //    sb.AppendLine("<th style='width: 10%;' scope='col'>EPS</th>");
                //}

                //sb.AppendLine("<th style='width: 10%;' scope='col'>YoY EPS Growth (%) </th>");

                //if (checker == "EPS_Share")
                //{
                //    sb.AppendLine("<th style='width: 10%;' scope='col'>Share Price</th>");
                //}
                //else
                //{
                //    sb.AppendLine("<th style='width: 10%;' scope='col'>M Cap</th>");
                //}

                //if (checker == "EPS_Share")
                //{
                //    sb.AppendLine("<th style='width: 15%;' scope='col'>YoY Share Price Growth (%)</th>");
                //}
                //else
                //{
                //    sb.AppendLine("<th style='width: 15%;' scope='col'>YoY M Cap Growth (%)</th>");
                //}

                //if (checker == "EPS_Share")
                //{
                //    sb.AppendLine("<th style='width: 10%;' scope='col'>Dividend Yield (%) </th>");
                //}

                //sb.AppendLine("<th style='width: 10%;' scope='col'>PE Ratio</th>");
                //sb.AppendLine("</tr>");


                //foreach (DataRow dr in dt.Rows)
                //{
                //    sb.AppendLine("<tr>");
                //    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 5%;'>");
                //    sb.AppendLine(Convert.ToDateTime(dr["FY_End"]).ToString("yyyy"));
                //    sb.AppendLine("</td>");

                //    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                //    sb.AppendLine(Convert.ToString(dr["EPS"]));
                //    sb.AppendLine("</td>");

                //    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                //    if (Convert.ToString(dr["EPSGrowth"]) != "0.00")
                //    {
                //        sb.AppendLine(Convert.ToString(dr["EPSGrowth"]));
                //    }
                //    else
                //    {
                //        sb.AppendLine(string.Empty);
                //    }

                //    sb.AppendLine("</td>");

                //    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                //    if (checker == "EPS_Share")
                //    {
                //        sb.AppendLine(Convert.ToString(dr["SharePrice"]));
                //    }
                //    else
                //    {
                //        sb.AppendLine(Convert.ToString(dr["M_Cap"]));
                //    }

                //    sb.AppendLine("</td>");

                //    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                //    if (checker == "EPS_Share")
                //    {
                //        if (Convert.ToString(dr["SharePriceGrowth"]) != "0.00")
                //        {
                //            sb.AppendLine(Convert.ToString(dr["SharePriceGrowth"]));
                //        }
                //        else
                //        {
                //            sb.AppendLine(string.Empty);
                //        }

                //    }
                //    else
                //    {
                //        if (Convert.ToString(dr["M_CapGrowth"]) != "0.00")
                //        {
                //            sb.AppendLine(Convert.ToString(dr["M_CapGrowth"]));
                //        }
                //        else
                //        {
                //            sb.AppendLine(string.Empty);
                //        }

                //    }

                //    sb.AppendLine("</td>");

                //    if (checker == "EPS_Share")
                //    {
                //        sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                //        sb.AppendLine(Convert.ToString(dr["Dividend_Yield"]));
                //        sb.AppendLine("</td>");
                //    }

                //    sb.AppendLine("<td align='center' style='font-family: verdana; font-size: 9pt; width: 10%;'>");
                //    sb.AppendLine(Convert.ToString(dr["PE"]));
                //    sb.AppendLine("</td>");

                //    sb.AppendLine("</tr>");

                //}
                //sb.AppendLine("</tbody></table>");

                HttpContext.Current.Response.Clear();
                using (System.IO.StringWriter stringWrite = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite))
                    {
                        dgv.RenderControl(htmlWrite);
                        sb.Append(stringWrite.ToString());
                        //sb.Replace("../Images/rsymbol.JPG", "../../Images/rsymbol.JPG");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region: Pdf Methods
        /// <summary>
        /// Main pdf converter function
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="company"></param>
        public byte[] _SimpleConversion(string checker, string filename, string company)
        {
            try
            {
                byte[] tmp;
                using (var wk = new MultiplexingConverter())
                //using(WkHtmlToPdfConverter wk = new WkHtmlToPdfConverter())
                {

                    //_Log.DebugFormat("Performing conversion..");

                    wk.GlobalSettings.Margin.Top = "0cm";
                    wk.GlobalSettings.Margin.Bottom = "0cm";
                    wk.GlobalSettings.Margin.Left = "0cm";
                    wk.GlobalSettings.Margin.Right = "0cm";

                    wk.ObjectSettings.Web.EnablePlugins = true;
                    wk.ObjectSettings.Web.EnableJavascript = true;
                    wk.ObjectSettings.Web.LoadImages = true;
                    wk.ObjectSettings.Web.PrintMediaType = true;
                    //wk.ObjectSettings.Web.EnableIntelligentShrinking = true;

                    string htmlfile = string.Empty;

                    if (checker == "EPS_Share")
                    {
                        htmlfile = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf/" + company + "_EPS_Share.htm");
                        //htmlfile = "http://mfiframes.mutualfundsindia.com/Chart/cache_pdf/" + company + "_EPS_Share.htm";
                    }
                    else
                    {
                        htmlfile = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf/" + company + "_EPS_Mcap.htm");
                        //htmlfile = "http://mfiframes.mutualfundsindia.com/Chart/cache_pdf/" + company + "_EPS_Mcap.htm";
                    }

                    //wk.ObjectSettings.Page = htmlfile;
                    wk.ObjectSettings.Load.Proxy = "none";

                    //var tmp = wk.Convert();
                    //tmp = wk.Convert(File.ReadAllText(htmlfile).Replace("../images/" + company + "_EPS_Mcap.Jpeg", "http://mfiframes.mutualfundsindia.com/Chart/" + "images/" + company + "_EPS_Mcap.Jpeg"));
                    tmp = wk.Convert(File.ReadAllText(htmlfile));

                    //Assert.IsNotEmpty(tmp);
                    //var number = 0;
                    //lock (this) number = count++;

                    //string savepdfpath = string.Empty;
                    string savehtmlpath = string.Empty;


                    if (checker == "EPS_Share")
                    {
                        if (!string.IsNullOrEmpty(company))
                        {
                            //        savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Share.pdf";
                            savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Share.htm";

                            //        if (File.Exists(savepdfpath))
                            //        {
                            //            File.Delete(savepdfpath);
                            //        }
                            //        File.WriteAllBytes(savepdfpath, tmp);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(company))
                        {
                    //        savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Mcap.pdf";
                            savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Mcap.htm";

                    //        if (File.Exists(savepdfpath))
                    //        {
                    //            File.Delete(savepdfpath);
                    //        }
                    //        File.WriteAllBytes(savepdfpath, tmp);
                        }
                    }
                    //if (File.Exists(savehtmlpath))
                    //{
                    //    File.Delete(savehtmlpath);
                    //}

                    //string _chartpath = string.Empty;
                    //_chartpath = HttpContext.Current.Server.MapPath("images/" + company + "_EPS_Mcap.Jpeg");
                    //if (File.Exists(_chartpath))
                    //{
                    //    File.Delete(_chartpath);
                    //}
                    //_chartpath = string.Empty;
                    //_chartpath = HttpContext.Current.Server.MapPath("images/" + company + "_EPS_Share.Jpeg");
                    //if (File.Exists(_chartpath))
                    //{
                    //    File.Delete(_chartpath);
                    //}
                }
                return tmp;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.ToString());
                HttpContext.Current.Response.Write(ex.InnerException.ToString());
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
                return null;
            }
        }
        /// <summary>
        /// Modified pdf converter function
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="company"></param>
        public void _SimpleConversion1(string checker, string company)
        {
            try
            {
                //using (var wk = _GetConverter())
                using (WkHtmlToPdfConverter wk = new WkHtmlToPdfConverter())
                {

                    _Log.DebugFormat("Performing conversion..");

                    wk.GlobalSettings.Margin.Top = "0cm";
                    wk.GlobalSettings.Margin.Bottom = "0cm";
                    wk.GlobalSettings.Margin.Left = "0cm";
                    wk.GlobalSettings.Margin.Right = "0cm";
                    //wk.GlobalSettings.Out = @"c:\temp\tmp.pdf";

                    wk.ObjectSettings.Web.EnablePlugins = false;
                    wk.ObjectSettings.Web.EnableJavascript = true;
                    wk.ObjectSettings.Web.LoadImages = true;
                    wk.ObjectSettings.Page = SimplePageFile;

                    string htmlfile = string.Empty;

                    if (checker == "EPS_Share")
                    {
                        htmlfile = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf/" + company + "_EPS_Share.htm");
                    }
                    else
                    {
                        htmlfile = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf/" + company + "_EPS_Mcap.htm");
                    }

                    wk.ObjectSettings.Page = htmlfile;
                    wk.ObjectSettings.Load.Proxy = "none";

                    //string html = File.ReadAllText(htmlfile);
                    //byte[] strHTML = wk.Convert(html);



                    var tmp = wk.Convert();
                    Assert.IsNotEmpty(tmp);
                    var number = 0;
                    lock (this) number = count++;

                    string savepdfpath = string.Empty;
                    string savehtmlpath = string.Empty;


                    if (checker == "EPS_Share")
                    {
                        if (!string.IsNullOrEmpty(company))
                        {
                            savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Share.pdf";
                            savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Share.htm";
                            if (File.Exists(savepdfpath))
                            {
                                File.Delete(savepdfpath);
                            }
                            File.WriteAllBytes(savepdfpath, tmp);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(company))
                        {
                            savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Mcap.pdf";
                            savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Mcap.htm";
                            if (File.Exists(savepdfpath))
                            {
                                File.Delete(savepdfpath);
                            }
                            File.WriteAllBytes(savepdfpath, tmp);
                        }
                    }
                    if (File.Exists(savehtmlpath))
                    {
                        File.Delete(savehtmlpath);
                    }

                    string _chartpath = string.Empty;
                    _chartpath = HttpContext.Current.Server.MapPath("images/" + company + "_EPS_Mcap.Jpeg");
                    if (File.Exists(_chartpath))
                    {
                        File.Delete(_chartpath);
                    }
                    _chartpath = string.Empty;
                    _chartpath = HttpContext.Current.Server.MapPath("images/" + company + "_EPS_Share.Jpeg");
                    if (File.Exists(_chartpath))
                    {
                        File.Delete(_chartpath);
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// Modify pdf conversion function
        /// </summary>
        /// <returns></returns>
        public void _SimpleConversion2(string checker, string company)
        {
            try
            {
                string htmlfile = string.Empty;

                if (checker == "EPS_Share")
                {
                    htmlfile = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf/" + company + "_EPS_Share.htm");
                }
                else
                {
                    htmlfile = HttpContext.Current.Server.MapPath("~/Chart/cache_pdf/" + company + "_EPS_Mcap.htm");
                }


                //var wkhtmlDir = @"C:\Program Files\wkhtmltopdf\";
                var wkhtmlDir = HttpContext.Current.Server.MapPath("~/Chart/");

                //var wkhtml = @"C:\Program Files\wkhtmltopdf\wkhtmltopdf.exe";
                var wkhtml = HttpContext.Current.Server.MapPath("~/Chart/wkhtmltopdf.exe");

                var p = new Process();

                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = wkhtml;
                p.StartInfo.WorkingDirectory = wkhtmlDir;





                string savepdfpath = string.Empty;
                string savehtmlpath = string.Empty;
                if (checker == "EPS_Share")
                {
                    if (!string.IsNullOrEmpty(company))
                    {
                        savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Share.pdf";
                        savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Share.htm";
                        if (File.Exists(savepdfpath))
                        {
                            File.Delete(savepdfpath);
                        }

                        string switches = "";
                        switches += "--print-media-type ";
                        switches += "--margin-top 10mm --margin-bottom 10mm --margin-right 10mm --margin-left 10mm ";
                        switches += "--page-size Letter ";
                        p.StartInfo.Arguments = switches + " " + htmlfile + " " + savepdfpath;
                        p.Start();



                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(company))
                    {
                        savepdfpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Mcap.pdf";
                        savehtmlpath = HttpContext.Current.Server.MapPath("~") + "\\Chart\\cache_pdf\\" + company + "_EPS_Mcap.htm";
                        if (File.Exists(savepdfpath))
                        {
                            File.Delete(savepdfpath);
                        }

                        string switches = "";
                        switches += "--print-media-type ";
                        switches += "--margin-top 10mm --margin-bottom 10mm --margin-right 10mm --margin-left 10mm ";
                        switches += "--page-size Letter ";
                        p.StartInfo.Arguments = switches + " " + htmlfile + " " + savepdfpath;
                        p.Start();

                    }
                }


                //read output
                byte[] buffer = new byte[32768];
                byte[] file;
                using (var ms = new MemoryStream())
                {
                    while (true)
                    {
                        int read = p.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);

                        if (read <= 0)
                        {
                            break;
                        }
                        ms.Write(buffer, 0, read);
                    }
                    file = ms.ToArray();
                }

                // wait or exit
                p.WaitForExit(60000);

                // read the exit code, close process
                int returnCode = p.ExitCode;
                p.Close();



                if (File.Exists(savehtmlpath))
                {
                    File.Delete(savehtmlpath);
                }

                string _chartpath = string.Empty;
                _chartpath = HttpContext.Current.Server.MapPath("images/" + company + "_EPS_Mcap.Jpeg");
                if (File.Exists(_chartpath))
                {
                    File.Delete(_chartpath);
                }
                _chartpath = string.Empty;
                _chartpath = HttpContext.Current.Server.MapPath("images/" + company + "_EPS_Share.Jpeg");
                if (File.Exists(_chartpath))
                {
                    File.Delete(_chartpath);
                }


            }
            catch (Exception ex)
            {

            }

        }
        private MultiplexingConverter _GetConverter()
        {
            var obj = new MultiplexingConverter();
            obj.Begin += (s, e) => _Log.DebugFormat("Conversion begin, phase count: {0}", e.Value);
            obj.Error += (s, e) => _Log.Error(e.Value);
            obj.Warning += (s, e) => _Log.Warn(e.Value);
            obj.PhaseChanged += (s, e) => _Log.InfoFormat("PhaseChanged: {0} - {1}", e.Value, e.Value2);
            obj.ProgressChanged += (s, e) => _Log.InfoFormat("ProgressChanged: {0} - {1}", e.Value, e.Value2);
            obj.Finished += (s, e) => _Log.InfoFormat("Finished: {0}", e.Value ? "success" : "failed!");
            return obj;
        }
        public string getImage(string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            string tempInput = input;
            string pattern = "<IMG(.|)+?>";
            string src = string.Empty;
            HttpContext context = HttpContext.Current;
            //Change the relative URL's to absolute URL's for an image, if any in the HTML code. 
            foreach (Match m in Regex.Matches(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.RightToLeft))
            {
                if (m.Success)
                {
                    string tempM = m.Value;
                    string pattern1 = "src=['|\"](.+?)['|\"]";
                    Regex reImg = new Regex(pattern1, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    Match mImg = reImg.Match(m.Value);
                    if (mImg.Success)
                    {
                        src = mImg.Value.ToLower().Replace("src=", "").Replace("\"", "");
                        if (src.ToLower().Contains("http://") == false)
                        {
                            //IIf you want to access through you can use commented src line below 
                            //   src = "src=\"" + context.Request.Url.Scheme + "://" + context.Request.Url.Authority + "/" + src + "\"";

                            src = "src=\"" + HttpContext.Current.Server.MapPath("~") + "\\" + src + "\"";


                            try
                            {
                                tempM = tempM.Remove(mImg.Index, mImg.Length);
                                tempM = tempM.Insert(mImg.Index, src);
                                //insert new url img tag in whole html code 
                                tempInput = tempInput.Remove(m.Index, m.Length);
                                tempInput = tempInput.Insert(m.Index, tempM);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                }
            }
            return tempInput;
        }

        #endregion

    }
}
