using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.ComponentModel;

namespace iFrames.WebServices
{
    /// <summary>
    /// Summary description for NAVChart
    /// </summary>
    [WebService(Namespace = "http://www.mutualfundsindia.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NAVChart : System.Web.Services.WebService
    {
        string connstr = ConfigurationManager.ConnectionStrings["MFIExplorerConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection();

        [WebMethod]
        public NameAndId[] FetchMutualFund(string mFIds)
        {
            if (mFIds.Trim().Length>0)
            {
                conn.ConnectionString = connstr;
                DataSet dt = new DataSet();
                string sql = string.Empty;
                try
                {
                    sql = "select  distinct NameAndId.mut_code,NameAndId.mut_name from MUT_FUND as NameAndId inner join  scheme_Info as SI on  NameAndId.mut_code=SI.mut_code and nav_check <> 'red' and NameAndId.mut_code in (";
                    for (int i = 0; i < mFIds.Split(',').Length; i++)
                    {
                        if (i == 0)
                            sql = sql + "'" + Convert.ToString(mFIds.Split(',')[i]) + "'";
                        else
                            sql = sql + ',' + "'" + Convert.ToString(mFIds.Split(',')[i]) + "'";
                    }
                    sql = sql + ") order by mut_name";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new NameAndId { Id = x["mut_code"].ToString(), Name = x["mut_name"].ToString() }).ToArray();
            }
            else
            {
                conn.ConnectionString = connstr;
                DataSet dt = new DataSet();
                try
                {
                    SqlCommand cmd = new SqlCommand("select  distinct NameAndId.mut_code,NameAndId.mut_name from MUT_FUND as NameAndId inner join  scheme_Info as SI on  NameAndId.mut_code=SI.mut_code and nav_check <> 'red' order by mut_name", conn);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new NameAndId { Id = x["mut_code"].ToString(), Name = x["mut_name"].ToString() }).ToArray();
            }
        }

        [WebMethod]
        public NameAndId[] FetchSchemes(string mutCode)
        {
            conn.ConnectionString = connstr;
            DataSet dt = new DataSet();
            SqlParameter mutcodeParam = new SqlParameter();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select sch_code,short_name as sch_name from scheme_Info where mut_code=@mut_code and nav_check <> 'red' order by sch_name";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;


                mutcodeParam.ParameterName = "@mut_code";
                mutcodeParam.SqlDbType = SqlDbType.VarChar;
                mutcodeParam.Size = 15;
                mutcodeParam.Direction = ParameterDirection.Input;
                mutcodeParam.Value = mutCode;

                cmd.Parameters.Add(mutcodeParam);
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
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new NameAndId { Id = x["sch_code"].ToString(), Name = x["sch_name"].ToString() }).ToArray();
        }

        [WebMethod]
        public NameAndId[] FetchIndices()
        {
            DataSet dt = new DataSet();
            conn.ConnectionString = connstr;
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
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new NameAndId { Id = x["ind_code"].ToString(), Name = x["ind_name"].ToString() }).ToArray();
        }

        [WebMethod]
        public NameAndId[] FetchIndicesAgainstScheme(string schemecode)
        {
            conn.ConnectionString = connstr;
            DataSet dt = new DataSet();
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
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt.Tables[0].Rows.Cast<DataRow>().Select(x => new NameAndId { Id = x["ind_code"].ToString(), Name = x["ind_name"].ToString() }).ToArray();
        }

        private int FetchNavCountAgainstScheme(string schemecode, string startdate, string enddate)
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

        private int FetchIndexCountAgainstIndex(string indcode, string startdate, string enddate)
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

        private DataSet FetchNav(string sql, string startdate, string enddate, List<string> arrParam_Name, List<string> arrParam_Value, List<string> arrColumn_Names)
        {
            conn.ConnectionString = connstr;
            DataSet dt = new DataSet();
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
                for (int i = 0; i < arrColumn_Names.Count; i++)
                    dt.Tables[0].Columns[i].ColumnName = arrColumn_Names[i].Replace(" ", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        [WebMethod]
        [Description("FetchNav from Webservice")]
        public DataSet FetchNav(List<SelectedItems> items, string startdate, string enddate)
        {
            try
            {
                int j1 = 0;
                StringBuilder sbsqlMain = new StringBuilder();
                StringBuilder sbsqlTop = new StringBuilder();
                StringBuilder sbsqlMiddle = new StringBuilder();
                StringBuilder sbsqlBottom = new StringBuilder();
                StringBuilder sbLast = new StringBuilder();
                List<string> arrParam_Name = new List<string>();
                List<string> arrParam_Value = new List<string>();
                List<string> arrColumn_Names = new List<string>();
                sbLast.AppendLine("select");
                foreach (SelectedItems dr in items)
                {
                    if (!dr.IsIndex)
                    {
                        string schemecode = dr.Id;
                        arrParam_Name.Add("Sch_Code" + j1);
                        arrParam_Value.Add(schemecode);
                        arrColumn_Names.Add(dr.Name);
                        sbsqlTop.AppendLine("declare @First_" + arrParam_Name[j1].ToString() + " as float;");
                        sbsqlTop.AppendLine("select top(1) @First_" + arrParam_Name[j1].ToString() + " = schemenav" + j1.ToString() + " from #temp where date = (select min(date) from #temp) ");
                        if (j1 != 0)
                            sbsqlBottom.AppendLine("inner join");
                        sbsqlBottom.AppendLine("(select nav_rs as schemenav" + j1 + ",dateadd(d,276, date) as date from nav_rec where dateadd(d,276, date) between  @Start_Date and @End_Date and sch_code=@Sch_Code" + j1 + ") Sch_Code" + j1 + "");
                        if (j1 != 0)
                            sbLast.AppendLine(",CAST(100 * schemenav" + j1 + "/@First_" + arrParam_Name[j1].ToString() + " as decimal(18,2) ) as schemenav" + j1 + "");
                        else
                            sbLast.AppendLine("CAST(100 * schemenav" + j1 + "/@First_" + arrParam_Name[j1].ToString() + " as decimal(18,2) ) as schemenav" + j1 + "");
                        if (j1 != 0)
                            sbsqlBottom.AppendLine("on " + arrParam_Name[0].ToString() + ".date = Sch_Code" + j1 + ".date");
                        j1 = j1 + 1;
                    }

                    else
                    {
                        string indcode = dr.Id;
                        arrParam_Name.Add("Ind_Code" + j1);
                        arrParam_Value.Add(indcode);
                        arrColumn_Names.Add(dr.Name);
                        sbsqlTop.AppendLine("declare @First_" + arrParam_Name[j1].ToString() + " as float;");
                        sbsqlTop.AppendLine("select top(1) @First_" + arrParam_Name[j1].ToString() + " = indnav" + j1.ToString() + " from #temp where date = (select Min(date) from #temp) ;");
                        if (j1 != 0)
                            sbsqlBottom.AppendLine("inner join");
                        sbsqlBottom.AppendLine("(select ind_val as indnav" + j1 + ",dateadd(d,276, dt1) as date from ind_rec where dateadd(d,276, dt1) between  @Start_Date and @End_Date and  ind_code=@Ind_Code" + j1 + ") Ind_Code" + j1 + "");
                        if (j1 != 0)
                            sbLast.AppendLine(",CAST( 100 *indnav" + j1 + "/@First_Ind_Code" + j1 + " AS decimal(18,2)) as indnav" + j1 + "");
                        else
                            sbLast.AppendLine("CAST( 100 *indnav" + j1 + "/@First_Ind_Code" + j1 + " AS decimal(18,2)) as indnav" + j1 + "");
                        if (j1 != 0)
                            sbsqlBottom.AppendLine("on " + arrParam_Name[0].ToString() + ".date = Ind_Code" + j1 + ".date");
                        j1 = j1 + 1;
                    }
                }
                if (arrParam_Name.Count > 0)
                {
                    sbsqlMiddle.AppendLine("select * into #temp from( select  ");
                    string str = string.Empty;
                    for (int j2 = 0; j2 < arrParam_Name.Count; j2++)
                    {
                        if (arrParam_Name[j2].ToString().StartsWith("Sch"))
                            sbsqlMiddle.Append("schemenav" + j2 + ",");
                        else
                            sbsqlMiddle.Append("indnav" + j2 + ",");
                        if (j2 != arrParam_Name.Count - 1)
                        {
                            if (string.IsNullOrEmpty(str))
                                str = "isnull(" + arrParam_Name[j2] + ".date," + arrParam_Name[j2 + 1].ToString() + ".date)";
                            else
                                str = "isnull(" + str + "," + arrParam_Name[j2 + 1] + ".date)";
                        }
                    }
                    sbsqlMiddle.Append(str + "date");
                    sbsqlMiddle.AppendLine(" from");
                    sbsqlBottom.AppendLine(")A");
                    sbsqlMain.AppendLine(sbsqlMiddle.ToString());
                    sbsqlMain.AppendLine(sbsqlBottom.ToString());
                    sbsqlMain.AppendLine(sbsqlTop.ToString());
                    sbLast.AppendLine(",date from #temp");
                    sbLast.AppendLine("drop table #temp");
                    sbsqlMain.AppendLine(sbLast.ToString());
                    return FetchNav(sbsqlMain.ToString(), startdate, enddate, arrParam_Name, arrParam_Value, arrColumn_Names);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod]
        public List<SelectedItems> FilterData(List<SelectedItems> items, string startdate, string enddate)
        {
            var lstResult = new List<SelectedItems>();
            conn.ConnectionString = connstr;
            try
            {
                foreach (var item in items)
                {
                    if (item.IsIndex)
                    {
                        if (FetchIndexCountAgainstIndex(item.Id, startdate, enddate) != 0)
                            lstResult.Add(item);
                    }
                    else
                    {
                        if (FetchNavCountAgainstScheme(item.Id, startdate, enddate) != 0)
                            lstResult.Add(item);
                    }
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [Serializable]
    public class SelectedItems
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsIndex { get; set; }
        [DefaultValue(true)]
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class NameAndId
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
