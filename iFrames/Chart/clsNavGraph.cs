using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text;

namespace iFrames.Chart
{
    public class clsNavGraph
    {
        #region: Global Variable
        string connstr = ConfigurationManager.ConnectionStrings["MFIExplorerConnectionString"].ConnectionString;
        //string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;

        SqlConnection conn = new SqlConnection();
        string firstRowEPS = string.Empty;

        #endregion

        #region: Constructor
        public clsNavGraph()
        { conn.ConnectionString = connstr; }
        #endregion

        #region: DropDownlist Method
        public void FillDropdown(Control ddl, DataTable _dt, string textfield, string valuefield, string defaulttext)
        {
            try
            {
                DataTable dt = _dt;
                if (dt.Rows.Count > 0)
                {
                    DropDownList drpdwn = (DropDownList)ddl;
                    if (drpdwn.Items.Count > 0)
                    {
                        drpdwn.Items.Clear();
                    }
                    drpdwn.DataSource = dt;
                    drpdwn.DataTextField = textfield;
                    drpdwn.DataValueField = valuefield;
                    drpdwn.DataBind();
                    drpdwn.Items.Insert(0, new ListItem("-Select " + defaulttext, "0"));
                    drpdwn.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {


            }
        }
        #endregion

        #region: Fetch Method
        public DataTable FetchExcludedMutualFund(string[] arrStr)
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            string sql = string.Empty;
            try
            {
                sql = "select  distinct MF.mut_code,MF.mut_name from MUT_FUND as MF inner join  scheme_Info as SI on  MF.mut_code=SI.mut_code and nav_check <> 'red' and MF.mut_code in (";

                //sql = "select mut_code,mut_name from mut_fund where mut_code not in(";
                for (int i = 0; i < arrStr.Length; i++)
                {
                    if (i == 0)
                    {
                        sql = sql + "'" + Convert.ToString(arrStr[i]) + "'";
                    }
                    else
                    {
                        sql = sql + "," + "'" + Convert.ToString(arrStr[i]) + "'";
                    }

                }
                sql = sql + ") order by mut_name";

                //SqlCommand cmd = new SqlCommand("select mut_code,mut_name from mut_fund where mut_code not in ('MF057','MF005','MF042')  order by mut_name", conn);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dt;
        }
        public DataTable FetchMutualFund()
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            try
            {
                //SqlCommand cmd = new SqlCommand("select distinct mut_code,mut_name from MUT_FUND order by mut_name", conn);
                SqlCommand cmd = new SqlCommand("select  distinct MF.mut_code,MF.mut_name from MUT_FUND as MF inner join  scheme_Info as SI on  MF.mut_code=SI.mut_code and nav_check <> 'red' order by mut_name", conn);

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
        public DataTable FetchSchemes(string SchemeCode)
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            SqlParameter mutcodeParam = new SqlParameter();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select sch_code,sch_name from scheme_Info where mut_code=@mut_code and nav_check <> 'red' order by sch_name";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;


                mutcodeParam.ParameterName = "@mut_code";
                mutcodeParam.SqlDbType = SqlDbType.VarChar;
                mutcodeParam.Size = 15;
                mutcodeParam.Direction = ParameterDirection.Input;
                mutcodeParam.Value = SchemeCode;

                cmd.Parameters.Add(mutcodeParam);
                cmd.Connection.Open();

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);


            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public DataTable FetchIndices()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("select ind_name,ind_code from ind_info order by ind_name", conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dt;
        }
        public DataTable FetchIndicesAgainstScheme(string schemecode)
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            SqlParameter schcodeParam = new SqlParameter();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select distinct SInfo.sch_name ,SI.sch_code,SI.ind_code,II.ind_name from schemeindex SI,ind_info II,scheme_Info SInfo where SInfo.sch_code=SI.sch_code  and  SI.ind_code=II.ind_code and  SI.sch_code=@sch_code";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                schcodeParam.ParameterName = "@sch_code";
                schcodeParam.SqlDbType = SqlDbType.VarChar;
                schcodeParam.Size = 15;
                schcodeParam.Direction = ParameterDirection.Input;
                schcodeParam.Value = schemecode;

                cmd.Parameters.Add(schcodeParam);
                cmd.Connection.Open();

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public int FetchNavAgainstScheme(string schemecode, string startdate, string enddate)
        {

            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            SqlParameter schcodeParam = new SqlParameter();
            SqlParameter startdateParam = new SqlParameter();
            SqlParameter enddateParam = new SqlParameter();
            try
            {
                SqlCommand cmd = new SqlCommand();

               // cmd.CommandText = "select  CONVERT(VARCHAR(10),date,101) as date ,nav_rs as schemenav from nav_rec where date between @startdate and @enddate and sch_code=@sch_code";
                cmd.CommandText = "select  count(*) from nav_rec where dateadd(d,276,date) between @startdate and @enddate and sch_code=@sch_code";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;


                schcodeParam.ParameterName = "@sch_code";
                schcodeParam.SqlDbType = SqlDbType.VarChar;
                schcodeParam.Size = 15;
                schcodeParam.Direction = ParameterDirection.Input;
                schcodeParam.Value = schemecode;

                startdateParam.ParameterName = "@startdate";
                startdateParam.SqlDbType = SqlDbType.VarChar;
                startdateParam.Size = 15;
                startdateParam.Direction = ParameterDirection.Input;
                //startdateParam.Value = "2004-09-27";
                startdateParam.Value = startdate;

                enddateParam.ParameterName = "@enddate";
                enddateParam.SqlDbType = SqlDbType.VarChar;
                enddateParam.Size = 15;
                enddateParam.Direction = ParameterDirection.Input;
                //enddateParam.Value = "2004-09-28";
                enddateParam.Value = enddate;

                cmd.Parameters.Add(startdateParam);
                cmd.Parameters.Add(enddateParam);

                cmd.Parameters.Add(schcodeParam);
                cmd.Connection.Open();


                return Convert.ToInt32(cmd.ExecuteScalar());
                //SqlDataAdapter adp = new SqlDataAdapter(cmd);
               // adp.Fill(dt);
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int FetchNavAgainstIndices(string indcode, string startdate, string enddate)
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            SqlParameter indcodeParam = new SqlParameter();
            SqlParameter startdateParam = new SqlParameter();
            SqlParameter enddateParam = new SqlParameter();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "select count(*) from  ind_rec where dateadd(d,276,dt1) between @startdate and @enddate and ind_code=@ind_code";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;


                indcodeParam.ParameterName = "@ind_code";
                indcodeParam.SqlDbType = SqlDbType.VarChar;
                indcodeParam.Size = 15;
                indcodeParam.Direction = ParameterDirection.Input;
                indcodeParam.Value = indcode;

                startdateParam.ParameterName = "@startdate";
                startdateParam.SqlDbType = SqlDbType.VarChar;
                startdateParam.Size = 15;
                startdateParam.Direction = ParameterDirection.Input;
                //startdateParam.Value = "2004-09-27";
                startdateParam.Value = startdate;

                enddateParam.ParameterName = "@enddate";
                enddateParam.SqlDbType = SqlDbType.VarChar;
                enddateParam.Size = 15;
                enddateParam.Direction = ParameterDirection.Input;
                //enddateParam.Value = "2004-09-28";
                enddateParam.Value = enddate;

                cmd.Parameters.Add(indcodeParam);
                cmd.Parameters.Add(startdateParam);
                cmd.Parameters.Add(enddateParam);
                cmd.Connection.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable FetchNav(string sql, string startdate, string enddate, ArrayList arrParam_Name, ArrayList arrParam_Value)
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            SqlParameter StartdateParam = new SqlParameter();
            SqlParameter EnddateParam = new SqlParameter();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                for (int j = 0; j < arrParam_Value.Count; j++)
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@" + arrParam_Name[j].ToString();
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 15;
                    param.Direction = ParameterDirection.Input;
                    param.Value = arrParam_Value[j].ToString();
                    cmd.Parameters.Add(param);

                }

                StartdateParam.ParameterName = "@Start_Date";
                StartdateParam.SqlDbType = SqlDbType.VarChar;
                StartdateParam.Size = 15;
                StartdateParam.Direction = ParameterDirection.Input;
                StartdateParam.Value = startdate;

                EnddateParam.ParameterName = "@End_Date";
                EnddateParam.SqlDbType = SqlDbType.VarChar;
                EnddateParam.Size = 15;
                EnddateParam.Direction = ParameterDirection.Input;
                EnddateParam.Value = enddate;

                cmd.Parameters.Add(StartdateParam);
                cmd.Parameters.Add(EnddateParam);
                cmd.Connection.Open();

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
        #endregion

        #region: Rebase Method
        public DataTable CalculateRebase(DataTable _dt)
        {

            ArrayList arrColumnValue = new ArrayList();

            DataTable dtReturn = new DataTable();
            dtReturn = _dt.Clone();
            Int32 count = 0;
            try
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    DataRow _dr = dtReturn.NewRow();
                    if (count == 0)
                    {
                        foreach (DataColumn column in _dt.Columns)
                        {
                            if (column.ColumnName.StartsWith("scheme"))
                            {
                                arrColumnValue.Add(_dt.Rows[0][column.ColumnName].ToString());
                            }
                            else if (column.ColumnName.StartsWith("ind"))
                            {
                                arrColumnValue.Add(_dt.Rows[0][column.ColumnName].ToString());
                            }
                        }



                    }
                    Int32 j = 0;
                    foreach (DataColumn clmn in _dt.Columns)
                    {
                        if (clmn.ColumnName == "date")
                        {
                            _dr[clmn.ColumnName] = dr[clmn.ColumnName].ToString();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(dr[clmn.ColumnName].ToString()))
                            {
                                _dr[clmn.ColumnName] = Math.Round(Convert.ToDouble(Rebase(dr[clmn.ColumnName].ToString(), arrColumnValue[j].ToString())), 2).ToString();
                                j = j + 1;
                            }
                            else
                            {
                                _dr[clmn.ColumnName] = dr[clmn.ColumnName].ToString();
                            }

                        }

                    }
                    dtReturn.Rows.Add(_dr);
                    count = count + 1;
                    j = 0;
                }


            }
            catch (Exception ex)
            {


            }
            return dtReturn;
        }
        public string Rebase(string _value, string firstRowValue)
        {
            string ReturnValue = string.Empty;
            Int32 fix = 100;
            try
            {
                ReturnValue = Convert.ToString((fix * Convert.ToDouble(_value)) / Convert.ToDouble(firstRowValue));

            }
            catch (Exception ex)
            {


            }
            return ReturnValue;

        }
        #endregion

        #region: Checker Field
        public bool CheckerSameField(DataTable dtResultSet, DataTable dtGridResult, string checker)
        {
            bool check = false;
            try
            {
                if (checker == "scheme")
                {
                    foreach (DataRow drResultSet in dtResultSet.Rows)
                    {
                        foreach (DataRow drGridResult in dtGridResult.Rows)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(drResultSet["sch_code"])))
                            {
                                if (Convert.ToString(drResultSet["sch_code"]) == Convert.ToString(drGridResult["SchemeCode"]))
                                {
                                    check = true;
                                    break;
                                }
                            }

                        }
                    }
                }
                else if (checker == "ind")
                {
                    foreach (DataRow drResultSet in dtResultSet.Rows)
                    {
                        foreach (DataRow drGridResult in dtGridResult.Rows)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(drResultSet["ind_code"])))
                            {
                                if (Convert.ToString(drResultSet["ind_code"]) == Convert.ToString(drGridResult["IndCode"]))
                                {
                                    check = true;
                                    break;
                                }
                            }


                        }
                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return check;
        }
        #endregion

        #region: Replace Name Method
        public DataTable ReplaceName(DataTable _dtorg, DataTable dtGrid)
        {
            try
            {
                foreach (DataRow dr in dtGrid.Select("", "SchemeCode desc"))
                {
                    if (!string.IsNullOrEmpty(dr["SchemeName"].ToString()) && Convert.ToString(dr["Status"]) != "0")
                    {
                        for (int i = 0; i < _dtorg.Columns.Count; i++)
                        {
                            if (_dtorg.Columns[i].ColumnName.StartsWith("schem"))
                            {

                                _dtorg.Columns[i].ColumnName = dr["SchemeName"].ToString();
                                break;
                            }

                        }
                    }

                    if (!string.IsNullOrEmpty(dr["IndName"].ToString()) && Convert.ToString(dr["Status"]) != "0")
                    {
                        for (int i = 0; i < _dtorg.Columns.Count; i++)
                        {
                            if (_dtorg.Columns[i].ColumnName.StartsWith("ind"))
                            {
                                _dtorg.Columns[i].ColumnName = dr["IndName"].ToString();
                                break;
                            }

                        }
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
            return _dtorg;
        }
        #endregion
    }
}