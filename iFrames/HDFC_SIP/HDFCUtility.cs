using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using iFrames.DAL;

namespace iFrames.HDFC_SIP
{
    public class HDFCUtility
    {
        //static readonly string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        static readonly SqlConnection conn = new SqlConnection();
        static SqlDataAdapter myAdapter;

        private static void InitConnection()
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
            myAdapter = new SqlDataAdapter();
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
        }


        public static bool SqlExecuteNonQuery(string sql, SqlParameter[] objparams)
        {
            bool success = false;

            try
            {
                InitConnection();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                //foreach (SqlParameter parm in objparams)
                //{
                //    cmd.Parameters.Add(parm);
                //}

                cmd.Parameters.AddRange(objparams);


                //cmd.Parameters.Add(preparedBy_Param);
                //cmd.Parameters.Add(mobile_ph_Param);
                //cmd.Parameters.Add(email_id_Param);
                //cmd.Parameters.Add(prepared_for_Param);
                //cmd.Parameters.Add(invest_mode_Param);
                //cmd.Parameters.Add(arn_no_Param);


                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }
            return success;
        }

        public DataTable executeSelectQuery(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();
            try
            {
                myCommand.Connection = conn;
                myCommand.CommandText = _query;
                myCommand.Parameters.AddRange(sqlParameter);
                myCommand.ExecuteNonQuery();
                myAdapter.SelectCommand = myCommand;
                myAdapter.Fill(ds);
                dataTable = ds.Tables[0];
            }
            catch (SqlException e)
            {
                throw e;
                return null;
            }
            finally
            {

            }
            return dataTable;
        }


        public void SetDistributorCredential(DSPDistributor objDSPDistributor)
        {

            string sql = string.Empty;

            string Preparedby = string.Empty;
            string PreparedFor = string.Empty;
            string Mobile = string.Empty;
            string Email = string.Empty;
            string InvestMode = string.Empty;
            string ArnNo = string.Empty;

            SqlParameter preparedBy_Param = new SqlParameter();
            SqlParameter mobile_ph_Param = new SqlParameter();
            SqlParameter email_id_Param = new SqlParameter();
            SqlParameter prepared_for_Param = new SqlParameter();
            SqlParameter invest_mode_Param = new SqlParameter();
            SqlParameter arn_no_Param = new SqlParameter();

            if (objDSPDistributor.Preparedby.Length > 0)
            {
                // ViewState["Preparedby"] = objDSPDistributor.Preparedby;
                Preparedby = objDSPDistributor.Preparedby.Trim();
            }

            if (objDSPDistributor.PreparedFor.Length > 0)
            {
                // ViewState["PreparedFor"] = objDSPDistributor.PreparedFor;
                PreparedFor = objDSPDistributor.PreparedFor.Trim();

            }

            if (objDSPDistributor.Mobile.Length > 0)
            {
                //ViewState["Mobile"] = objDSPDistributor.Mobile;
                Mobile = objDSPDistributor.Mobile.Trim();
            }

            if (objDSPDistributor.Email.Length > 0)
            {
                //ViewState["Email"] = objDSPDistributor.Email;
                Email = objDSPDistributor.Email.Trim();
            }

            if (objDSPDistributor.ArnNo.Length > 0)
            {
                // ViewState["ArnNo"] = objDSPDistributor.ArnNo;
                ArnNo = objDSPDistributor.ArnNo.Trim();
            }

            InvestMode = objDSPDistributor.Investment_mode;

            try
            {


                sql = "INSERT INTO [T_Iframe_DSP_Distributor] ([Distributor_Name],[Mobile_No],[Email_Id],[Prapared_For],[Investment_mode],[Arn_No]) VALUES (@Distributor_Name,@Mobile_No,@Email_Id,@Prapared_For,@Investment_mode,@Arn_no)";

                preparedBy_Param.ParameterName = "@Distributor_Name";
                preparedBy_Param.SqlDbType = SqlDbType.NVarChar;
                preparedBy_Param.Direction = ParameterDirection.Input;
                preparedBy_Param.Value = Preparedby;

                mobile_ph_Param.ParameterName = "@Mobile_No";
                mobile_ph_Param.SqlDbType = SqlDbType.NVarChar;
                mobile_ph_Param.Direction = ParameterDirection.Input;
                mobile_ph_Param.Value = Mobile;


                email_id_Param.ParameterName = "@Email_Id";
                email_id_Param.SqlDbType = SqlDbType.NVarChar;
                email_id_Param.Direction = ParameterDirection.Input;
                email_id_Param.Value = Email;

                prepared_for_Param.ParameterName = "@Prapared_For";
                prepared_for_Param.SqlDbType = SqlDbType.NVarChar;
                prepared_for_Param.Direction = ParameterDirection.Input;
                prepared_for_Param.Value = PreparedFor;

                invest_mode_Param.ParameterName = "@Investment_mode";
                invest_mode_Param.SqlDbType = SqlDbType.NVarChar;
                invest_mode_Param.Direction = ParameterDirection.Input;
                invest_mode_Param.Value = InvestMode;

                arn_no_Param.ParameterName = "@Arn_No";
                arn_no_Param.SqlDbType = SqlDbType.NVarChar;
                arn_no_Param.Direction = ParameterDirection.Input;
                arn_no_Param.Value = ArnNo;

                //  conn.Open();



                SqlExecuteNonQuery(sql, new SqlParameter[] { preparedBy_Param, mobile_ph_Param, email_id_Param, prepared_for_Param, invest_mode_Param, arn_no_Param });

            }
            catch (Exception)
            {
                throw;
            }

        }


        public bool InsertDistributorCredential(DSPDistributor objDSPDistributor)
        {
            bool success = false;
            using (var datacontext = new SIP_ClientDataContext())
            {
                T_Iframe_DSP_Distributor objDist = new T_Iframe_DSP_Distributor()
                {
                    Arn_No = objDSPDistributor.ArnNo,
                    Distributor_Name = objDSPDistributor.Preparedby,
                    Email_Id = objDSPDistributor.Email,
                    Investment_Mode = objDSPDistributor.Investment_mode,
                    Prapared_For = objDSPDistributor.PreparedFor,
                    Mobile_No = objDSPDistributor.Mobile
                };

                datacontext.T_Iframe_DSP_Distributors.InsertOnSubmit(objDist);
                datacontext.SubmitChanges();
                //objDist.
            }
            return success;
        }
    }

    public class DSPDistributor
    {
        public string Preparedby { get; set; }

        public string PreparedFor { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string ArnNo { get; set; }

        public string Investment_mode { get; set; }

    }
}