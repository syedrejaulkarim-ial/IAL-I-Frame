using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using iFrames.DAL;
using NPOI.SS.Formula.Functions;

namespace iFrames.BLL
{
    public class CanaraClient_BLL
    {
        public List<CanaraLogin> getUserDetails(string EmailId)
        {
            try
            {
                var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_WEB_LOGIN_MASTERs
                             where um.Email_Id == EmailId
                             select new CanaraLogin
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
                return _data.ToList<CanaraLogin>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool lockUser(string EmailId, bool isLock)
        {
            bool retVal = false;
            try
            {
                var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_WEB_LOGIN_MASTERs
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
        public bool updateLastLogin(string EmailId, string Password)
        {
            bool retVal = false;
            try
            {
                var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_WEB_LOGIN_MASTERs
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
                var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };

                var _data = (from um in dc.T_WEB_LOGIN_MASTERs
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
        public void UpdataStaticData1(List<T_CANARA_SCHEMES_STATIC_DATA> objList,int userId)
        {
            try
            {
                ////var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };
                //using (var dc = new CanaraClientDataContext())
                //{
                //    var _data = (from um in dc.T_CANARA_SCHEMES_STATIC_DATAs
                //                 where um.Scheme_ID == obj.Scheme_ID && um.Amfi_Code == obj.Amfi_Code
                //                 select um).SingleOrDefault();
                //    //_data.Last_Login_Date = DateTime.Now;
                //    if (_data != null)
                //    {
                //        _data.Risk = obj.Risk;
                //        _data.Benchmark_Risk = obj.Benchmark_Risk;
                //        _data.AUM = obj.AUM;
                //        _data.Aum_Date = obj.Aum_Date;
                //        _data.Inception_Date = obj.Inception_Date;
                //        _data.Horizon = obj.Horizon;
                //        _data.Goal = obj.Goal;
                //        _data.Benchmark = obj.Benchmark;
                //        _data.Additional_Benchmark = obj.Additional_Benchmark;
                //        _data.About_Fund = obj.About_Fund;
                //        _data.Investment_Objective = obj.Investment_Objective;
                //        _data.Exit_Load = obj.Exit_Load;
                //        _data.Entry_Load = obj.Entry_Load;
                //        _data.Prescribed_Asset_Allocation = obj.Prescribed_Asset_Allocation;
                //        _data.Suitable_For = obj.Suitable_For;
                //        _data.Expense_Ratio_Direct = obj.Expense_Ratio_Direct;
                //        _data.Expense_Ratio_Regular = obj.Expense_Ratio_Regular;
                //        _data.Product_Suitable_for = obj.Product_Suitable_for;
                //        _data.Reason_To_Invest = obj.Reason_To_Invest;
                //        _data.Min_Amount_SIP = obj.Min_Amount_SIP;
                //        _data.Min_Amount_SWP = obj.Min_Amount_SWP;
                //        _data.Min_Amount_STP = obj.Min_Amount_STP;
                //        _data.Min_Amount_Lumpsum = obj.Min_Amount_Lumpsum;
                //        _data.Min_Amount_Redeem = obj.Min_Amount_Redeem;
                //        _data.Modified_On = DateTime.Now;
                //        _data.Modified_By = userId.ToString();
                //        dc.SubmitChanges();
                //    }
                //    else
                //    {
                //        // Handle the case where no matching record is found
                //        Console.WriteLine("No matching record found.");
                //    }
                //}
                Parallel.ForEach(objList, obj =>
                {
                    using (var dc = new CanaraClientDataContext())
                    {
                        var _data = (from um in dc.T_CANARA_SCHEMES_STATIC_DATAs
                                     where um.Scheme_ID == obj.Scheme_ID && um.Amfi_Code == obj.Amfi_Code
                                     select um).SingleOrDefault();

                        if (_data != null)
                        {
                            _data.Risk = obj.Risk;
                            _data.Benchmark_Risk = obj.Benchmark_Risk;
                            _data.AUM = obj.AUM;
                            _data.Aum_Date = obj.Aum_Date;
                            _data.Inception_Date = obj.Inception_Date;
                            _data.Horizon = obj.Horizon;
                            _data.Goal = obj.Goal;
                            _data.Benchmark = obj.Benchmark;
                            _data.Additional_Benchmark = obj.Additional_Benchmark;
                            _data.About_Fund = obj.About_Fund;
                            _data.Investment_Objective = obj.Investment_Objective;
                            _data.Exit_Load = obj.Exit_Load;
                            _data.Entry_Load = obj.Entry_Load;
                            _data.Prescribed_Asset_Allocation = obj.Prescribed_Asset_Allocation;
                            _data.Suitable_For = obj.Suitable_For;
                            _data.Expense_Ratio_Direct = obj.Expense_Ratio_Direct;
                            _data.Expense_Ratio_Regular = obj.Expense_Ratio_Regular;
                            _data.Product_Suitable_for = obj.Product_Suitable_for;
                            _data.Reason_To_Invest = obj.Reason_To_Invest;
                            _data.Min_Amount_SIP = obj.Min_Amount_SIP;
                            _data.Min_Amount_SWP = obj.Min_Amount_SWP;
                            _data.Min_Amount_STP = obj.Min_Amount_STP;
                            _data.Min_Amount_Lumpsum = obj.Min_Amount_Lumpsum;
                            _data.Min_Amount_Redeem = obj.Min_Amount_Redeem;
                            _data.Modified_On = DateTime.Now;
                            _data.Modified_By = userId.ToString();
                            dc.SubmitChanges();
                        }
                        else
                        {
                            // Handle the case where no matching record is found
                            Console.WriteLine("No matching record found for Scheme_ID: " + obj.Scheme_ID + " and Amfi_Code: " + obj.Amfi_Code);
                        }
                    }
                });

                //return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw ex;
            }
        }
        public void UpdataStaticData(List<T_CANARA_SCHEMES_STATIC_DATA> objList, int userId)
        {
            int batchSize = 50; // Adjust the batch size as needed
            for (int i = 0; i < objList.Count; i += batchSize)
            {
                var batch = objList.Skip(i).Take(batchSize).ToList();
                var a = batch.GroupBy(x => new { x.Scheme_ID, x.Amfi_Code }).Select(g => g.First()).ToList();
                foreach (var obj in a)
                {
                    try
                    {
                        using (var dc = new CanaraClientDataContext())
                        {
                            var _data = (from um in dc.T_CANARA_SCHEMES_STATIC_DATAs
                                         where um.Scheme_ID == obj.Scheme_ID && um.Amfi_Code == obj.Amfi_Code
                                         select um).SingleOrDefault();

                            if (_data != null)
                            {
                                _data.Risk = obj.Risk;
                                _data.Benchmark_Risk = obj.Benchmark_Risk;
                                _data.AUM = obj.AUM;
                                _data.Aum_Date = obj.Aum_Date;
                                _data.Inception_Date = obj.Inception_Date;
                                _data.Horizon = obj.Horizon;
                                _data.Goal = obj.Goal;
                                _data.Benchmark = obj.Benchmark;
                                _data.Additional_Benchmark = obj.Additional_Benchmark;
                                _data.About_Fund = obj.About_Fund;
                                _data.Investment_Objective = obj.Investment_Objective;
                                _data.Exit_Load = obj.Exit_Load;
                                _data.Entry_Load = obj.Entry_Load;
                                _data.Prescribed_Asset_Allocation = obj.Prescribed_Asset_Allocation;
                                _data.Suitable_For = obj.Suitable_For;
                                _data.Expense_Ratio_Direct = obj.Expense_Ratio_Direct;
                                _data.Expense_Ratio_Regular = obj.Expense_Ratio_Regular;
                                _data.Product_Suitable_for = obj.Product_Suitable_for;
                                _data.Reason_To_Invest = obj.Reason_To_Invest;
                                _data.Min_Amount_SIP = obj.Min_Amount_SIP;
                                _data.Min_Amount_SWP = obj.Min_Amount_SWP;
                                _data.Min_Amount_STP = obj.Min_Amount_STP;
                                _data.Min_Amount_Lumpsum = obj.Min_Amount_Lumpsum;
                                _data.Min_Amount_Redeem = obj.Min_Amount_Redeem;
                                _data.Modified_On = DateTime.Now;
                                _data.Modified_By = userId.ToString();
                                dc.SubmitChanges();
                            }
                            else
                            {
                                // Handle the case where no matching record is found
                                Console.WriteLine("No matching record found for Scheme_ID: " + obj.Scheme_ID + " and Amfi_Code: " + obj.Amfi_Code);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
            }
        }
        public void UpdataStaticDataSIngleObj(T_CANARA_SCHEMES_STATIC_DATA obj, int userId)
        {
            using (var dc = new CanaraClientDataContext())
            {
                var _data = (from um in dc.T_CANARA_SCHEMES_STATIC_DATAs
                             where um.Scheme_ID == obj.Scheme_ID && um.Amfi_Code == obj.Amfi_Code
                             select um).SingleOrDefault();

                if (_data != null)
                {
                    _data.Risk = obj.Risk;
                    _data.Benchmark_Risk = obj.Benchmark_Risk;
                    _data.AUM = obj.AUM;
                    _data.Aum_Date = obj.Aum_Date;
                    _data.Inception_Date = obj.Inception_Date;
                    _data.Horizon = obj.Horizon;
                    _data.Goal = obj.Goal;
                    _data.Benchmark = obj.Benchmark;
                    _data.Additional_Benchmark = obj.Additional_Benchmark;
                    _data.About_Fund = obj.About_Fund;
                    _data.Investment_Objective = obj.Investment_Objective;
                    _data.Exit_Load = obj.Exit_Load;
                    _data.Entry_Load = obj.Entry_Load;
                    _data.Prescribed_Asset_Allocation = obj.Prescribed_Asset_Allocation;
                    _data.Suitable_For = obj.Suitable_For;
                    _data.Expense_Ratio_Direct = obj.Expense_Ratio_Direct;
                    _data.Expense_Ratio_Regular = obj.Expense_Ratio_Regular;
                    _data.Product_Suitable_for = obj.Product_Suitable_for;
                    _data.Reason_To_Invest = obj.Reason_To_Invest;
                    _data.Min_Amount_SIP = obj.Min_Amount_SIP;
                    _data.Min_Amount_SWP = obj.Min_Amount_SWP;
                    _data.Min_Amount_STP = obj.Min_Amount_STP;
                    _data.Min_Amount_Lumpsum = obj.Min_Amount_Lumpsum;
                    _data.Min_Amount_Redeem = obj.Min_Amount_Redeem;
                    _data.Modified_On = DateTime.Now;
                    _data.Modified_By = userId.ToString();
                    dc.SubmitChanges();
                }
                else
                {
                    // Handle the case where no matching record is found
                    Console.WriteLine("No matching record found for Scheme_ID: " + obj.Scheme_ID + " and Amfi_Code: " + obj.Amfi_Code);
                }
            }
        }
        public void DeleteFundmanagerData(int Scheme_ID)
        {
            try {
                var dc = new CanaraClientDataContext();
                string sql = "DELETE FROM T_CANARA_SCHEMES_fundmanagers WHERE Scheme_id = {0}";
                dc.ExecuteCommand(sql, Scheme_ID);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void InsertFundmanagerData(List<FundManagerClass> obj, int userId, int Scheme_ID)
        {
            try
            {
                var a = obj.GroupBy(x => new { x.FundManagerNames, x.FromDates,x.ImageLinks,x.DocLinks }).Select(g => g.First()).ToList();
                foreach (FundManagerClass data in a)
                {
                    using (var db = new CanaraClientDataContext())
                    {
                        var fundmng = new T_CANARA_SCHEMES_fundmanager
                        {
                            FundManagerName = data.FundManagerNames,
                            From_Date = data.FromDates,
                            Image_link = data.ImageLinks,
                            Doc_Link = data.DocLinks,
                            Scheme_id = Scheme_ID,
                            Modified_By = userId.ToString(),
                            Modified_On = DateTime.Now,
                            Inserted_By=userId.ToString()
                        };
                        if (fundmng.FundManagerName != "" && fundmng.From_Date != null && fundmng.Image_link!=null && fundmng.Doc_Link!=null)
                        {
                            db.ExecuteCommand("INSERT INTO T_CANARA_SCHEMES_fundmanagers (FundManagerName, From_Date, Image_link, Doc_Link, Scheme_id, Modified_By, Modified_On,Inserted_By,Is_active) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6},{7},{8})",
                                          fundmng.FundManagerName, fundmng.From_Date, fundmng.Image_link, fundmng.Doc_Link, fundmng.Scheme_id, fundmng.Modified_By, fundmng.Modified_On,fundmng.Inserted_By,1);
                        }

                    }

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetSchemeIDByAMpCode(int Amfi_Code)
        {
            int SchemeId = 0;
            try {
                var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };
                var sd = dc.T_CANARA_SCHEMES_STATIC_DATAs
                        .Where(o => o.Amfi_Code == Amfi_Code)
                        .Select(o => o.Scheme_ID)
                        .FirstOrDefault();
                SchemeId = sd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SchemeId;
        }
        public DataTable getAllStaticData()
        {
            DataTable retDt = new DataTable();
            try
            {
                var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };
                var _data = from sm in dc.T_CANARA_SCHEMES_STATIC_DATAs select sm;
                retDt = _data.ToDataTable();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retDt;
        }
        public DataTable GetTemplateGenarateCode()
        {
            DataTable retDt = new DataTable();
            try
            {
                //var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };
                //var _data = from sm in dc.T_SCHEMES_MASTERs select sm;
                //retDt = _data.ToDataTable();
                using (var context = new CanaraClientDataContext())
                {
                    string sql = "select sd.Scheme_ID,sd.Scheme_Name,sd.Amfi_Code,sd.Risk,sd.Benchmark_Risk,sd.AUM,sd.Aum_Date,sd.Inception_Date,sd.Horizon,sd.Goal,sd.Benchmark,sd.Additional_Benchmark,sd.About_Fund,sd.Investment_Objective,sd.Exit_Load,sd.Entry_Load,sd.Prescribed_Asset_Allocation,sd.Product_Suitable_for,sd.Suitable_For,sd.PT_Ratio,sd.Expense_Ratio_Regular,sd.Expense_Ratio_Direct,sd.Reason_To_Invest,string_agg(cf.FundManagerName, '# ') AS FundManagerName,string_agg(cf.From_Date, '# ') AS From_Date,string_agg(cf.Image_link, '# ') AS Image_link,string_agg(cf.Doc_Link, '# ') AS Doc_Link,sd.Min_Amount_SIP,sd.Min_Amount_SWP,sd.Min_Amount_STP,sd.Min_Amount_Lumpsum,sd.Min_Amount_Redeem from T_CANARA_SCHEMES_STATIC_DATA  sd left join T_CANARA_SCHEMES_fundmanagers cf on sd.Scheme_ID=cf.Scheme_id and ISNULL(cf.Is_active,0)=1 left join T_CANARA_SCHEMES_AssetAllocation ca on sd.Scheme_ID=ca.Scheme_id where sd.Amfi_Code IS NOT NULL group by  sd.Scheme_ID,sd.Scheme_Name,sd.Amfi_Code,sd.Risk,sd.Benchmark_Risk,sd.AUM,sd.Aum_Date,sd.Inception_Date,sd.Horizon,sd.Goal,sd.Benchmark,sd.Additional_Benchmark,sd.About_Fund,sd.Investment_Objective,sd.Exit_Load,sd.Entry_Load,sd.Prescribed_Asset_Allocation,sd.Product_Suitable_for,sd.Suitable_For,sd.PT_Ratio,sd.Expense_Ratio_Regular,sd.Expense_Ratio_Direct,sd.Reason_To_Invest,sd.Min_Amount_SIP,sd.Min_Amount_SWP,sd.Min_Amount_STP,sd.Min_Amount_Lumpsum,sd.Min_Amount_Redeem order by Amfi_Code asc";
                    var result = context.ExecuteQuery<ExcelTemplateFormat>(sql).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Scheme Name");
                    dt.Columns.Add("Amfi Code");
                    dt.Columns.Add("Risk");
                    dt.Columns.Add("Benchmark Risk");
                    dt.Columns.Add("AUM");
                    dt.Columns.Add("Aum Date");
                    dt.Columns.Add("Inception Date");
                    dt.Columns.Add("Horizon");
                    dt.Columns.Add("Goal");
                    dt.Columns.Add("Benchmark");
                    dt.Columns.Add("Additional Benchmark");
                    dt.Columns.Add("About Fund");
                    dt.Columns.Add("Investment Objective");
                    dt.Columns.Add("Exit Load");
                    dt.Columns.Add("Entry Load");
                    dt.Columns.Add("Prescribed Asset Allocation");
                    //dt.Columns.Add("Expense Ratio");
                    dt.Columns.Add("Suitable For");
                    dt.Columns.Add("PT Ratio");
                    dt.Columns.Add("Expense Ratio Regular");
                    dt.Columns.Add("Expense Ratio Direct");
                    dt.Columns.Add("Product Suitable for");
                    dt.Columns.Add("Reason to invest");
                    dt.Columns.Add("Fund Manager Name");
                    dt.Columns.Add("From Date");
                    dt.Columns.Add("image link");
                    dt.Columns.Add("Doc Link");
                    dt.Columns.Add("Min amountSIP");
                    dt.Columns.Add("Min amountSWP");
                    dt.Columns.Add("Min amountSTP");
                    dt.Columns.Add("Min amountLumpsum");
                    dt.Columns.Add("Min amountRedeem");
                    //dt.Columns.Add("Prescribed Asset Allocation");
                    foreach (var item in result)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Scheme Name"] = item.Scheme_Name;
                        dr["Amfi Code"] = item.Amfi_Code;
                        dr["Risk"] = item.Risk;
                        dr["Benchmark Risk"] = item.Benchmark_Risk;
                        dr["AUM"] = item.AUM;
                        dr["Aum Date"] = item.Aum_Date;
                        dr["Inception Date"] = item.Inception_Date;
                        dr["Horizon"] = item.Horizon;
                        dr["Goal"] = item.Goal;
                        dr["Benchmark"] = item.Benchmark;
                        dr["Additional Benchmark"] = item.Additional_Benchmark;
                        dr["About Fund"] = item.About_Fund;
                        dr["Investment Objective"] = item.Investment_Objective;
                        dr["Exit Load"] = item.Exit_Load;
                        dr["Entry Load"] = item.Entry_Load;
                        dr["Prescribed Asset Allocation"] = item.Prescribed_Asset_Allocation;
                        dr["Suitable For"] = item.Suitable_For;
                        dr["PT Ratio"] = item.PT_Ratio;
                        dr["Expense Ratio Regular"] = item.Expense_Ratio_Regular;
                        dr["Expense Ratio Direct"] = item.Expense_Ratio_Direct;
                        dr["Product Suitable for"] = item.Product_Suitable_for;
                        dr["Reason to invest"] = item.Reason_To_Invest;
                        dr["Fund Manager Name"] = item.FundManagerName;
                        dr["From Date"] = item.From_Date;
                        dr["image link"] = item.Image_link;
                        dr["Doc Link"] = item.Doc_Link;
                        dr["Min amountSIP"] = item.Min_Amount_SIP;
                        dr["Min amountSWP"] = item.Min_Amount_SWP;
                        dr["Min amountSTP"] = item.Min_Amount_STP;
                        dr["Min amountLumpsum"] = item.Min_Amount_Lumpsum;
                        dr["Min amountRedeem"] = item.Min_Amount_Redeem;
                        dt.Rows.Add(dr);
                    }
                    retDt = dt;// result.ToDataTable();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retDt;
        }
        public bool resetPassword(string EmailId, string Password)
        {
            bool retVal = false;
            try
            {
                var dc = new CanaraClientDataContext() { CommandTimeout = 6000 };
                var _data = (from um in dc.T_WEB_LOGIN_MASTERs
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

    }

    //
    public class CanaraLogin
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
    public class FundManagerClass
    {
        public string FundManagerNames { get; set; }
        public DateTime? FromDates { get; set; }
        public string ImageLinks { get; set; }
        public string DocLinks { get; set; }
        public int index { get; set; }
    }
    public class ExcelTemplateFormat
    {
        public int Scheme_ID { get; set; }
        public string Scheme_Name { get; set; }
        public int Amfi_Code { get; set; }
        public string Risk { get; set; }
        public string Benchmark_Risk { get; set; }
        public decimal? AUM { get; set; }
        public System.DateTime? Aum_Date { get; set; }
        public System.DateTime? Inception_Date { get; set; }
        public string Horizon { get; set; }
        public string Goal { get; set; }
        public string Benchmark { get; set; }
        public string Additional_Benchmark { get; set; }
        public string About_Fund { get; set; }
        public string Investment_Objective { get; set; }
        public string Exit_Load { get; set; }
        public string Entry_Load { get; set; }
        public string Prescribed_Asset_Allocation { get; set; }
        //public string Expense_Ratio { get; set; }

        public string Suitable_For { get; set; }
        public decimal? PT_Ratio { get; set; }
        public string Expense_Ratio_Regular { get; set; }
        public string Expense_Ratio_Direct { get; set; }
        public string Product_Suitable_for { get; set; }
        public string Reason_To_Invest { get; set; }
        public string FundManagerName { get; set; }
        public string From_Date { get; set; }
        public string Image_link { get; set; }
        public string Doc_Link { get; set; }
        public string Min_Amount_SIP { get; set; }
        public string Min_Amount_SWP { get; set; }
        public string Min_Amount_STP { get; set; }
        public string Min_Amount_Lumpsum { get; set; }
        public string Min_Amount_Redeem { get; set; }

    }
}