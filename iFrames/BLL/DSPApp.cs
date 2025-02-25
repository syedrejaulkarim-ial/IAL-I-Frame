using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Web;
using iFrames.DAL;
using System.Data.SqlClient;

namespace iFrames.BLL
{
    public class ApiInputs
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Date { get; set; }
    }
    public class DSPLogin
    {
        public int User_Id { get; set; }
        public string Email_Id { get; set; }
        public string Password { get; set; }
        public System.Nullable<System.DateTime> Last_Login_Date { get; set; }
        public System.DateTime Password_Changed_Date { get; set; }
        public int Wrong_Attmept_Count { get; set; }
        //public string Comment { get; set; }
        public int Created_By { get; set; }
        public System.DateTime Created_On { get; set; }
        //public System.Nullable<int> Modified_BY { get; set; }
        //public System.Nullable<System.DateTime> Modified_On { get; set; }
        public bool IsActive { get; set; }
        public System.Nullable<System.DateTime> Last_Locked_Out_Date { get; set; }
        public System.Nullable<bool> IsLockedOut { get; set; }
        public System.Nullable<bool> IsOnLine { get; set; }
        public System.Nullable<bool> IsAdmin { get; set; }
    }

    public class ApiExternalLoginEntity
    {
        public const int APITokenStamp = 5;
        public string UserId { get; set; }
        public byte[] Password { get; set; }
        public string ClientIp { get; set; }
        public int ClientId { get; set; }
        public string AuthenticationTag { get; set; }
    }

    public class DSPApp
    {
        public bool resetPassword(string EmailId, string Password)
        {
            bool retVal = false;
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };
                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.Email_Id == EmailId
                             select um).SingleOrDefault();
                _data.Password = Utilities.DESEnCode(Password);
                _data.Password_Changed_Date = DateTime.Now;
                dc.SubmitChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public bool lockUser(string EmailId, bool isLock)
        {
            bool retVal = false;
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.Email_Id == EmailId
                             select um).SingleOrDefault();
                if (isLock)
                    _data.Last_Locked_Out_Date = DateTime.Now;
                else
                {
                    _data.Last_Locked_Out_Date = null;
                    _data.Wrong_Attmept_Count = 0;
                }
                _data.IsLockedOut = isLock;
                dc.SubmitChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public bool updateUserStatus(int UserId, bool Status)
        {
            bool retVal = false;
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };
                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.User_Id == UserId
                             select um).SingleOrDefault();
                _data.IsActive = Status;
                dc.SubmitChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public bool updateLoginStatus(string EmailId)
        {
            bool retVal = false;
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.Email_Id == EmailId
                             select um).SingleOrDefault();
                _data.IsOnLine = null;

                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public bool updateLastLogin(string EmailId, string Password)
        {
            bool retVal = false;
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.Email_Id == EmailId && um.Password == Password
                             select um).SingleOrDefault();
                _data.Last_Login_Date = DateTime.Now;
                //reset wrong attempt count on successful login
                _data.Wrong_Attmept_Count = 0;
                _data.Last_Locked_Out_Date = null;
                _data.IsLockedOut = false;
                _data.IsOnLine = true;

                dc.SubmitChanges();
                retVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public int updateWrongAttempCount(string EmailId)
        {
            int retVal = 0;
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.Email_Id == EmailId
                             select um).SingleOrDefault();

                _data.Wrong_Attmept_Count = _data.Wrong_Attmept_Count + 1;
                retVal = _data.Wrong_Attmept_Count;
                dc.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public List<DSPLogin> getUserDetails()
        {
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             orderby um.Email_Id ascending
                             select new DSPLogin
                             {
                                 User_Id = um.User_Id,
                                 Email_Id = um.Email_Id,
                                 Password = um.Password,
                                 Last_Login_Date = um.Last_Login_Date,
                                 Password_Changed_Date = um.Password_Changed_Date,
                                 Wrong_Attmept_Count = um.Wrong_Attmept_Count,
                                 //Comment = um.Comment,
                                 Created_By = um.Created_By,
                                 Created_On = um.Created_On,
                                 //Modified_BY = um.Modified_BY,
                                 //Modified_On = um.Modified_On,
                                 IsActive = um.IsActive,
                                 Last_Locked_Out_Date = um.Last_Locked_Out_Date,
                                 IsLockedOut = um.IsLockedOut,
                                 IsOnLine = um.IsOnLine,
                                 IsAdmin = um.IsAdmin
                             }
                             );
                return _data.ToList<DSPLogin>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DSPLogin> getUserDetails(string EmailId)
        {
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.Email_Id == EmailId
                             select new DSPLogin
                             {
                                 User_Id = um.User_Id,
                                 Email_Id = um.Email_Id,
                                 Password = um.Password,
                                 Last_Login_Date = um.Last_Login_Date,
                                 Password_Changed_Date = um.Password_Changed_Date,
                                 Wrong_Attmept_Count = um.Wrong_Attmept_Count,
                                 //Comment = um.Comment,
                                 Created_By = um.Created_By,
                                 Created_On = um.Created_On,
                                 //Modified_BY = um.Modified_BY,
                                 //Modified_On = um.Modified_On,
                                 IsActive = um.IsActive,
                                 Last_Locked_Out_Date = um.Last_Locked_Out_Date,
                                 IsLockedOut = um.IsLockedOut,
                                 IsOnLine = um.IsOnLine,
                                 IsAdmin = um.IsAdmin
                             }
                             );
                return _data.ToList<DSPLogin>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<T_DSP_LOGIN_MASTER> getUserDetails(string EmailId, string Password)
        //{
        //    List<T_DSP_LOGIN_MASTER> retData = new List<T_DSP_LOGIN_MASTER>();
        //    try
        //    {
        //        var dc = new DSPAppDataContext() { CommandTimeout = 6000 };
        //        retData = (from um in dc.T_DSP_LOGIN_MASTERs
        //                   where um.Email_Id == EmailId && um.Password == Password
        //                   select um).ToList<T_DSP_LOGIN_MASTER>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return retData;
        //}

        public bool createUser(string Emailid, string Password, int CreatedBy, string BranchName)
        {
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };
                //{
                T_DSP_LOGIN_MASTER lm = new T_DSP_LOGIN_MASTER();
                lm.Email_Id = Emailid;
                lm.Password = Utilities.DESEnCode(Password);
                lm.Created_By = CreatedBy;
                lm.Created_On = DateTime.Now;
                lm.Password_Changed_Date = DateTime.Now;
                lm.Wrong_Attmept_Count = 0;
                lm.IsActive = true;
                lm.BRANCH_NAME = BranchName;
                dc.T_DSP_LOGIN_MASTERs.InsertOnSubmit(lm);
                dc.SubmitChanges();
                return true;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return false;
        }

        public DataTable getUserBranch()
        {
            DataTable retDt = new DataTable();
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };
                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.BRANCH_NAME.ToString().Trim() != ""
                             orderby um.BRANCH_NAME ascending
                             select new
                             {
                                 um.BRANCH_NAME
                             }
                            ).Distinct().OrderBy(x => x.BRANCH_NAME);
                retDt = _data.ToDataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retDt;
        }

        public DataTable getLoginHistory(string BranchName, DateTime FromDate, DateTime ToDate)
        {
            DataTable retDt = new DataTable();
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };
                var _data = dc.MFI_IFRAME_DSP_LOGIN_HISTORY(BranchName, FromDate, ToDate);
                retDt = _data.ToDataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retDt;
        }


        public static DataTable GetExistLoginId(string EmailId)
        {
            try
            {
                using (var dal = new DSPAppDataContext())
                {
                    var Login = from ev in dal.T_DSP_LOGIN_MASTERs
                                where ev.Email_Id.Trim().ToUpper() == EmailId.Trim().ToUpper()
                                select new { ev.Email_Id };

                    return Login.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool UpdateLoginDetail(string EmailId, string Branchname, int ModifyBy)
        {
            try
            {
                using (var dal = new DSPAppDataContext())
                {
                    var env = (from ev in dal.T_DSP_LOGIN_MASTERs
                               where ev.Email_Id == EmailId
                               select ev).First();
                    env.BRANCH_NAME = Branchname;
                    env.Modified_BY = ModifyBy;
                    env.Modified_On = DateTime.Now;
                    dal.SubmitChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ApiExternalLoginEntity GenerateExternalLoginToken(ApiExternalLoginEntity ExternalLoginInfo)
        {
            try
            {
                var Password = System.Text.Encoding.UTF8.GetString(ExternalLoginInfo.Password);
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_SINGLE_SIGNONs
                             where um.IsActive && um.User_Id == ExternalLoginInfo.UserId && um.Password == Password
                             select new ApiExternalLoginEntity
                             {
                                 UserId = um.User_Id,
                                 ClientId = um.Client_Id,
                             }).FirstOrDefault();
                return _data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DSPLogin GetExternalLoginUser(ApiExternalLoginEntity ExternalLoginInfo)
        {
            try
            {
                var dc = new DSPAppDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_DSP_LOGIN_MASTERs
                             where um.IsActive && um.Email_Id == ExternalLoginInfo.UserId
                             select new DSPLogin
                             {
                                 Email_Id = um.Email_Id,
                                 User_Id = um.User_Id,
                                 Password = um.Password
                             }).FirstOrDefault();
                return _data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}