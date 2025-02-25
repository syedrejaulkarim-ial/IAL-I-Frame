using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFrames.DAL;

using System.Globalization;
using System.Text;
using System.Linq.Dynamic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Web.Caching;

namespace iFrames.BLL
{
    public static class AllMethods
    {
        static string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        static SqlConnection conn = new SqlConnection();


        #region Old

        public static DataTable getTopFundResult(int rank, string type, string cate, string per, string MutCode = "")
        {
            per = per.Trim();
            cate = cate.Trim();
            type = type.Trim();
            using (var dc = new iFrames.DAL.iFramesDataContext())
            {
                var dayss = (from dtf in dc.top_funds orderby dtf.date descending select dtf.date.Value).Max();
                var maxDate = dayss.AddDays(-3);

                var data = (from tf in dc.top_funds
                            from si in dc.GetTable<scheme_Info>()
                            from ln in dc.latest_navs
                                //tf.date >= maxDate &&
                                //where tf.date >= maxDate && si.sch_code == tf.sch_code && ln.Sch_Code == si.sch_code
                            where tf.date >= maxDate && si.sch_code == tf.sch_code && ln.Sch_Code == si.sch_code
                            select new
                            {
                                si.mut_code,
                                ln.Nav_Rs,
                                tf.date.Value.Date,
                                tf.per_91days,
                                tf.per_15days,
                                tf.per_182days,
                                tf.per_1yr,
                                tf.per_30days,
                                tf.per_3yr,
                                tf.per_5yr,
                                tf.per_7days,
                                tf.since_incept,
                                si.sch_code,
                                si.sch_name,
                                si.nature,
                                si.type1,
                                si.type2,
                                si.type3
                            });



                if (cate != "" && cate != "Equity" && cate != "Equity & Debt" && cate != "Debt" && cate != "Gilt" && cate != "ETF" && type == "" && cate != "Fund of Funds" && cate != "Short Term Debt")
                {
                    if (cate == "Tax")
                        data = data.Where(f => f.nature == "Equity" && f.type2 == "Tax Planning");
                    else if (cate == "Diversified")
                        data = data.Where(f => f.nature == "Equity" && f.type3 == "Growth" && f.type2 == "Diversified");
                    else if (cate == "Sector")
                        data = data.Where(f => f.nature == "Equity" && f.type3 == "Growth" && f.type2 == "Sector");
                    else if (cate == "Index")
                        data = data.Where(f => f.nature == "Equity" && f.type3 == "Growth" && f.type2 == "Index");
                    else if (cate == "Short")
                        data = data.Where(f => f.nature == "Debt" && f.type3 == "Growth" && f.type2 == "Short term");
                    else if (cate == "Income")
                        data = data.Where(f => f.nature == "Debt" && f.type3 == "Growth" && f.type3 != "Growth" && f.type2 != "MIP");
                    else if (cate == "MIP")
                        data = data.Where(f => f.nature == "Debt" && f.type3 == "Growth" && f.type2 == "MIP");
                }

                else if (cate != "" && cate != "Equity" && cate != "Equity & Debt" && cate != "Debt" && cate != "Gilt" && cate != "ETF" && type != "" && cate != "Fund of Funds" && cate != "Short Term Debt")
                {
                    if (cate == "Tax")
                        data = data.Where(f => f.nature == "Equity" && f.type2 == "Tax Planning" && f.type3 == "Growth" && f.type1 == type);
                    else if (cate == "Diversified")
                        data = data.Where(f => f.nature == "Equity" && f.type3 == "Growth" && f.type2 == "Diversified" && f.type1 == type);
                    else if (cate == "Sector")
                        data = data.Where(f => f.nature == "Equity" && f.type3 == "Growth" && f.type2 == "Sector" && f.type1 == type);
                    else if (cate == "Index")
                        data = data.Where(f => f.nature == "Equity" && f.type3 == "Growth" && f.type2 == "Index" && f.type1 == type);
                    else if (cate == "Short")
                        data = data.Where(f => f.nature == "Debt" && f.type3 == "Growth" && f.type2 == "Short term" && f.type1 == type);
                    else if (cate == "Income")
                        data = data.Where(f => f.nature == "Debt" && f.type3 != "Growth" && f.type2 != "MIP" && f.type1 == type);
                    else if (cate == "MIP")
                        data = data.Where(f => f.nature == "Debt" && f.type3 == "Growth" && f.type2 == "MIP" && f.type1 == type);
                }
                else if (cate != "" && type != "")
                {
                    data = data.Where(f => f.nature == cate && f.type3 == "Growth" && f.type1 == type);
                }
                else if (cate != "" && type == "")
                {
                    data = data.Where(f => f.nature == cate && f.type3 == "Growth");
                }
                else if (cate == "" && type != "")
                {
                    // data = data.Where(f => f.type3 == "Growth" && f.type1 == type);
                    data = data.Where(f => f.type1 == type);
                }
                else
                {
                    data = data.Where(f => f.type3 == "Growth");
                }

                DataTable dtAlldata = data.ToDataTable();
                DataRow[] drs;
                if (MutCode != "")
                    drs = getMutResult(MutCode, "mut_code", dtAlldata).Select("", per + " desc");
                else
                    drs = dtAlldata.Select("", per + " desc");
                DataTable dt = dtAlldata.Clone();
                foreach (var drResult in drs.Take(rank))
                {
                    dt.ImportRow(drResult);
                }
                return dt;
            }
        }

        public static DataTable getMutResult(string MutCode, string colName, DataTable dtVal)
        {
            string[] path = MutCode.Split(new char[] { ',' });
            MutCode = colName + " in ('";
            for (int i = 0; i < path.Length; i++)
            {
                MutCode += path[i] + "','";
            }
            MutCode = MutCode.EndsWith(",'") == true ? MutCode.TrimEnd('\'') : MutCode;
            MutCode = MutCode.EndsWith(",'") == true ? MutCode.TrimEnd(',') : MutCode;
            MutCode += ")";

            DataRow[] drs = dtVal.Select(MutCode, "");
            DataTable dt = dtVal.Clone();
            foreach (var drResult in drs)
            {
                dt.ImportRow(drResult);
            }
            return dt;
        }

        public static DataTable getMutName(string MutCode = "")
        {
            List<string> arr = new List<string>();
            if (MutCode != "")
            {
                string[] path = MutCode.Split(new char[] { ',' });
                for (int i = 0; i < path.Length; i++)
                {
                    arr.Add(path[i]);
                }
            }
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var data = (from mn in db.mfi_news
                            from mf in db.mut_funds
                            where MutCode != "" ? arr.Contains(mf.Mut_Code) && mf.Mut_Code == mn.Mut_Name : mf.Mut_Code == mn.Mut_Name
                            orderby mf.Mut_Name
                            select new { mf.Mut_Name, mut_code = mn.Mut_Name }).Distinct();

                DataTable dt = data.ToDataTable();
                return dt;
            }
        }

        public static DataTable getYears()
        {
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var data = (from mn in db.mfi_news
                            where
                              mn.DATE1.ToString().Trim() != ""
                            select new { yrs = mn.DATE1.Value.Year.ToString() }).Distinct().OrderBy(yr => yr.yrs);

                DataTable dt = data.ToDataTable();
                return dt;
            }
        }

        public static DataTable getMonths()
        {
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var data = (from mn in db.mfi_news
                            where
                              mn.DATE1.ToString().Trim() != ""
                            select new
                            {
                                digimons = (System.Int32?)mn.DATE1.Value.Month,
                                charmon = mn.DATE1.Value.Month.ToString()
                            }).Distinct();

                DataTable dt = data.ToDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    dr["charmon"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(dr["charmon"]));
                }
                dt.AcceptChanges();
                DataRow[] drs = dt.Select("", "digimons asc");
                DataTable dtmon = dt.Clone();
                foreach (var drResult in drs)
                {
                    dtmon.ImportRow(drResult);
                }
                return dtmon;
            }
        }

        public static DataTable getNewsHeadLines(bool btnClick, string req = "", string mutName = "", string yrs = "", string mons = "")
        {
            //List<string> arr = new List<string>();
            //    if (MutCode != "")
            //    {
            //        string[] path = MutCode.Split(new char[] { ',' });                    
            //        for (int i = 0; i < path.Length; i++)
            //        {
            //            arr.Add(path[i]);
            //        }
            //    }
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var data = (from mn in db.mfi_news
                                //from mf in db.mut_funds
                                //where MutCode != "" ? arr.Contains(mf.Mut_Code) : true && mf.Mut_Name==mn.Mut_Name
                            select new
                            {
                                mn.news_headline,
                                mn.newspaper_name,
                                mn.DATE,
                                mn.Mut_Name
                            }).Distinct();

                if (btnClick == true)
                {
                    if (yrs.Trim() != "Choose a Year" && mutName.Trim() == "" && Convert.ToInt32(mons) == 0)
                        data = data.Where(m => m.DATE.Value.Year == Convert.ToInt32(yrs));
                    else if (yrs.Trim() != "Choose a Year" && mutName != "" && Convert.ToInt32(mons) == 0)
                        data = data.Where(m => m.DATE.Value.Year == Convert.ToInt32(yrs) && m.Mut_Name == mutName);
                    else if (yrs != "Choose a Year" && mutName != "" && Convert.ToInt32(mons) != 0)
                        data = data.Where(m => m.DATE.Value.Year == Convert.ToInt32(yrs) && m.DATE.Value.Month == Convert.ToInt32(mons) && m.Mut_Name == mutName);
                    else if (yrs == "Choose a Year" && mutName != "" && Convert.ToInt32(mons) != 0)
                        data = data.Where(m => m.DATE.Value.Month == Convert.ToInt32(mons) && m.Mut_Name == mutName);
                    else if (yrs == "Choose a Year" && mutName == "" && Convert.ToInt32(mons) != 0)
                        data = data.Where(m => m.DATE.Value.Month == Convert.ToInt32(mons));
                    else if (yrs == "Choose a Year" && mutName != "" && Convert.ToInt32(mons) == 0)
                        data = data.Where(m => m.Mut_Name == mutName);
                    else if (yrs != "Choose a Year" && mutName == "" && Convert.ToInt32(mons) != 0)
                        data = data.Where(m => m.DATE.Value.Year == Convert.ToInt32(yrs) && m.DATE.Value.Month == Convert.ToInt32(mons));
                }
                else
                {
                    data = data.Where(mn => mn.Mut_Name.Contains(req) && mn.DATE >= DateTime.Today.AddDays(-365) && mn.DATE <= DateTime.Today);
                }

                DataTable dt = data.ToDataTable();

                if (dt.Rows.Count > 0)
                {
                    DataRow[] drArr = dt.Select("", "DATE desc");
                    DataTable dtres = dt.Clone();
                    foreach (DataRow dr in drArr)
                    {
                        dtres.ImportRow(dr);
                    }
                    return dtres;
                }
                else
                    return dt;
            }
        }

        public static DataTable getThoughts()
        {
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var data = (from fn in db.front_news
                            orderby fn.date descending
                            select new { fn.d_line, fn.html, fn.date });

                DataTable dt = data.ToDataTable();
                return dt;
            }
        }

        public static DataTable getDivDecEquityFundData(string lst)
        {
            DataTable dt = new DataTable();
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                if (lst == "Equity")
                {
                    var data = (((from dv in db.div_recs
                                  from si in db.GetTable<scheme_Info>()
                                  where si.sch_code == dv.Sch_code && (dv.Record_date >= DateTime.Now.Date.AddDays(-30) || dv.Record_date > DateTime.Now.Date) && si.nature.StartsWith("equity") && si.type3 != "Growth"
                                  select new { si.nature, si.short_name, dv.Record_date, dv.Divid_pt, si.sch_code }).Distinct()).OrderByDescending(a => a.Record_date)).OrderBy(b => b.short_name);

                    dt = data.ToDataTable();
                    return dt;
                }
                else if (lst == "Debt")
                {
                    var data = ((((from dv in db.div_recs
                                   from si in db.GetTable<scheme_Info>()
                                   where si.sch_code == dv.Sch_code && (dv.Record_date >= DateTime.Now.Date.AddDays(-7) || dv.Record_date > DateTime.Now.Date) && si.nature.StartsWith("debt") && si.type3 != "Growth"
                                   select new { si.nature, si.short_name, dv.Record_date, dv.Divid_pt, si.sch_code }).Distinct()).OrderByDescending(a => a.nature)).OrderBy(b => b.short_name)).OrderByDescending(c => c.Record_date);

                    dt = data.ToDataTable();
                    return dt;
                }
                else if (lst == "Balance")
                {
                    var data = ((((from dv in db.div_recs
                                   from si in db.GetTable<scheme_Info>()
                                   where si.sch_code == dv.Sch_code && (dv.Record_date >= DateTime.Now.Date.AddDays(-7) || dv.Record_date > DateTime.Now.Date) && si.nature.StartsWith("balanced") && si.type3 != "Growth"
                                   select new { si.nature, si.short_name, dv.Record_date, dv.Divid_pt, si.sch_code }).Distinct()).OrderByDescending(a => a.nature)).OrderBy(b => b.short_name)).OrderByDescending(c => c.Record_date);

                    dt = data.ToDataTable();
                    return dt;
                }
                else
                {
                    return dt;
                }
            }
        }

        public static DataTable getUcontrolFundHouse(string MutCode = "")
        {
            DataTable dt = new DataTable();
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var mutCode = (from si in db.GetTable<scheme_Info>()
                               where si.nav_check != "red"
                               select si.mut_code).Distinct();

                var data = (from mf in db.mut_funds
                            where mutCode.Contains(mf.Mut_Code)
                            select new { mf.Mut_Code, mf.Mut_Name }).OrderBy(a => a.Mut_Name);

                if (MutCode != "")
                    dt = getMutResult(MutCode, "Mut_Code", data.ToDataTable());
                else
                    dt = data.ToDataTable();
            }

            return dt;
        }

        public static DataTable getEntryExitLoadFundHouse(string MutCode = "")
        {
            DataTable dt = new DataTable();
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var data = (from si in db.GetTable<scheme_Info>()
                            from mf in db.mut_funds
                            from e in db.exitentry_loads
                            where si.mut_code == mf.Mut_Code && si.sch_code == e.sch_code
                            select new { si.mut_code, mf.Mut_Name }).Distinct().OrderBy(a => a.Mut_Name);

                if (MutCode != "")
                    dt = getMutResult(MutCode, "Mut_Code", data.ToDataTable());
                else
                    dt = data.ToDataTable();
            }

            return dt;
        }

        public static DataTable getUcontrolSchemeName(string callingPage, string mutCode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var data = (from si in db.GetTable<scheme_Info>()
                            where si.nav_check != "red" && si.mut_code == mutCode
                            select new { si.sch_code, si.short_name, si.sch_name, si.variant });

                if (callingPage == "DivHome")
                {
                    var schCode = from dr in db.div_recs select dr.Sch_code;
                    data = data.Where(d => schCode.Contains(d.sch_code) && d.variant != "dp").Distinct().OrderBy(a => a.short_name);
                }
                else if (callingPage == "FundFactSheet")
                    data = data.OrderBy(a => a.short_name);

                dt = data.ToDataTable();
            }
            return dt;
        }

        public static DataTable getEntryExitLoadSchemeName(string mutCode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var data = (from si in db.GetTable<scheme_Info>()
                            from e in db.exitentry_loads
                            where si.nav_check != "red" && si.mut_code == mutCode && e.sch_code == si.sch_code
                            && si.type1.ToLower() == "open ended" && e.app_from == (from ex in db.exitentry_loads
                                                                                    where ex.sch_code == e.sch_code
                                                                                    select ex.app_from).Max()
                            select new { si.sch_code, si.short_name });

                dt = data.ToDataTable();
            }
            return dt;
        }

        public static DataTable getDivHomeResult(string schCode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var result = ((from dr in db.div_recs
                               from si in db.GetTable<scheme_Info>()
                               where dr.Sch_code == schCode && dr.Record_date.ToString() != "" && si.sch_code == dr.Sch_code
                               select new { si.sch_name, dr.Record_date, Divid_pt = Math.Round(dr.Divid_pt.Value, 2, MidpointRounding.AwayFromZero), div = dr.dividend == null ? "NA" : dr.dividend }).OrderByDescending(a => a.Record_date));

                dt = result.ToDataTable();
            }
            return dt;
        }

        public static DataTable getTopperFunds(string callingPage, string MutCode = "")
        {
            DataTable dt = new DataTable();
            using (var db = new iFrames.DAL.iFramesDataContext())
            {
                var dayss = (from dtf in db.top_funds orderby dtf.date descending select dtf.date.Value).Max();
                var maxDate = dayss.AddDays(-3);

                var data = (from si in db.GetTable<scheme_Info>()
                            from tf in db.top_funds
                            from sc in db.sch_nature_details
                            where sc.sch_code == si.sch_code && tf.sch_code == sc.sch_code &&
                           ((tf.per_7days == null ? 0 : tf.per_7days) != 0 | (tf.per_15days == null ? 0 : tf.per_15days) != 0 |
                           (tf.per_30days == null ? 0 : tf.per_30days) != 0 | (tf.per_91days == null ? 0 : tf.per_91days) != 0 |
                           (tf.per_182days == null ? 0 : tf.per_182days) != 0 | (tf.per_1yr == null ? 0 : tf.per_1yr) != 0 |
                           (tf.per_3yr == null ? 0 : tf.per_3yr) != 0) && tf.date >= maxDate && si.type1 == "Open Ended" && si.nav_check != "red"

                            select new
                            {
                                tf.date,
                                tf.nature,
                                si.short_name,
                                tf.per_182days,
                                tf.per_91days,
                                tf.per_1yr,
                                tf.per_3yr,
                                tf.per_30days,
                                si.sch_code,
                                tf.since_incept,
                                sc.subnature1,
                                sc.subnature3,
                                sc.subnature2,
                                sc.nature1,
                                si.type3,
                                si.mut_code
                            });


                switch (callingPage)
                {
                    case "OpenTaxSect":
                        data = data.Where(a => a.nature1 == "equity" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "OpenTaxSectOther":
                        data = data.Where(a => a.subnature1 != "Sector" && a.subnature1 != "Tax Planning" && a.nature1 == "equity" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "SectSpeSchAll":
                        data = data.Where(a => a.subnature1 == "Sector" && a.nature1 == "equity" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "InfoScheme":
                        data = data.Where(a => a.subnature3 == "Infotech" && a.nature1 == "equity" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "PharmaSch":
                        data = data.Where(a => a.subnature3 == "Pharma" && a.subnature1 == "Sector" && a.nature1 == "equity" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "FmcgSch":
                        data = data.Where(a => a.subnature3 == "FMCG" && a.subnature1 == "Sector" && a.nature1 == "equity" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "OpenTax":
                        data = data.Where(a => a.subnature1 == "Tax Planning" && a.nature1 == "equity" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "OpenBalance":
                        data = data.Where(a => a.nature1 == "Balanced" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "DebtShort":
                        data = data.Where(a => a.nature1 == "Debt" && a.subnature2 == "regular" && a.subnature1 == "Short Term" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "DebtSch":
                        data = data.Where(a => a.nature1 == "Debt" && a.subnature2 == "regular" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "LiquidSch":
                        data = data.Where(a => a.nature1 == "liquid" && a.subnature2 == "Regular" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                    case "GiltSch":
                        data = data.Where(a => a.nature1 == "gilt" && a.subnature2 == "Regular" && a.type3 == "Growth").OrderBy(a => a.short_name);
                        break;
                }

                if (MutCode != "")
                    dt = getMutResult(MutCode, "mut_code", data.ToDataTable());
                else
                    dt = data.ToDataTable();

            }
            return dt;
        }


        #endregion
        //Added by Mukesh
        #region XIRR

        public static double getXIRR(string DATE_STR, string AMT_STR)
        {

            double XIRR = 0;
            try
            {
                //conn.ConnectionString = connstr;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }
                SqlCommand sqlcmd;
                sqlcmd = new SqlCommand("USP_SP_XIRR", conn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandTimeout = 2000;
                sqlcmd.Parameters.Add(new SqlParameter("@DATE_STR", DATE_STR));
                sqlcmd.Parameters.Add(new SqlParameter("@AMOUNT_STR", AMT_STR));
                sqlcmd.Parameters.Add(new SqlParameter("@XIRROUT", SqlDbType.Float, 0));
                sqlcmd.Parameters["@XIRROUT"].Direction = ParameterDirection.Output;
                sqlcmd.ExecuteNonQuery();
                XIRR = (double)sqlcmd.Parameters["@XIRROUT"].Value;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return XIRR;
        }


        #endregion
        #region Get BenchMark and Additional benchmark
        public static decimal AddbenchMark(decimal schmeId)
        {
            using (var principl = new PrincipalCalcDataContext())
            {
                var DbRet = (from tfm in principl.RETURN_SIP_PRINCIPALs
                             where tfm.Scheme_ID == Convert.ToInt32(schmeId)

                             select tfm.Additional_Index_ID);
                return DbRet.FirstOrDefault().Value;
            }
        }
        public static decimal GetIndexId(decimal schmeId)
        {
            decimal IndexId = 0;
            using (var principl = new PrincipalCalcDataContext())
            {
                var index_name = (from t_index_masters in principl.T_INDEX_MASTERs
                                  where

                                      ((from t_schemes_indexes in principl.T_SCHEMES_INDEXes
                                        where
                                          t_schemes_indexes.SCHEME_ID ==
                                            ((from t_schemes_masters in principl.T_SCHEMES_MASTERs
                                              where
                                                t_schemes_masters.Scheme_Id == Convert.ToDecimal(schmeId)
                                              select new
                                              {
                                                  t_schemes_masters.Scheme_Id
                                              }).First().Scheme_Id)
                                        select t_schemes_indexes.INDEX_ID
                                        )).Contains(t_index_masters.INDEX_ID)// need to remove take 1
                                  select new
                                  {
                                      t_index_masters.INDEX_NAME,
                                      t_index_masters.INDEX_ID
                                  });
                if (index_name != null && index_name.Count() > 0)
                {
                    //HdnbenchMarkId.Value = index_name.Single().INDEX_ID.ToString();
                    //HiddenFieldName.Value = index_name.Single().INDEX_NAME.ToString();
                    //var gg = index_name.FirstOrDefault();
                    //IndexId = gg.INDEX_ID;
                    return index_name.FirstOrDefault().INDEX_ID;
                }

            }
            return IndexId;
        }
        #endregion
        #region Function Added Mukesh

        //Get Scheme Name
        public static DataTable getSchName(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 where lstSchemeId.Contains(sm.Scheme_Id)
                                 orderby sm.Sch_Short_Name
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name
                                 }).Distinct();

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        public static List<decimal> getFundSchemeId(string MutFund)
        {
            List<decimal> listFund = new List<decimal>();
            if (MutFund != string.Empty)
            {
                string[] path = MutFund.Split(new char[] { ',' });
                for (int i = 0; i < path.Length; i++)
                {
                    listFund.Add(Convert.ToDecimal(path[i]));
                }
            }
            return listFund;
        }

        public static string getFundSchemeIdStr(List<decimal> listSchFund)
        {

            string finalSchFundId = string.Empty;
            if (listSchFund.Count > 0)
            {
                foreach (decimal _decSch in listSchFund)
                {
                    finalSchFundId += _decSch.ToString().Trim() + ",";
                }
                finalSchFundId = finalSchFundId.TrimEnd(',');
            }

            return finalSchFundId;
        }

        // Fetch the List of all Nature
        public static DataTable getNature()
        {
            DataTable dtNature = null;
            try
            {
                using (var natureData = new SIP_ClientDataContext())
                {
                    var nature = from nat in natureData.T_SCHEMES_NATURE_Clients
                                 where nat.Nature != "N.A"
                                 orderby nat.Nature
                                 select new
                                 {
                                     nat.Nature,
                                     nat.Nature_ID
                                 };

                    if (nature.Count() > 0)
                    {
                        dtNature = nature.ToDataTable().OrderBy("Nature asc");
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {

            }

            return dtNature;
        }

        // Fetch the List of all sebi Nature
        public static DataTable getSebiNature()
        {
            DataTable dtNature = null;
            try
            {
                using (var natureData = new SIP_ClientDataContext())
                {
                    var nature = from nat in natureData.T_SEBI_SCHEMES_NATUREs
                                 where nat.Sebi_Nature != "N.A"
                                 orderby nat.Sebi_Nature
                                 select new
                                 {
                                     nat.Sebi_Nature,
                                     nat.Sebi_Nature_ID
                                 };

                    if (nature.Count() > 0)
                    {
                        dtNature = nature.ToDataTable();
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

            }

            return dtNature;
        }

        //Fetch Structure List and Name
        public static DataTable getStructure()
        {
            DataTable dtStructure = null;
            try
            {
                using (var dcData = new SIP_ClientDataContext())
                {

                    var _struc = dcData.T_SCHEMES_STRUCTURE_Clients
                        .Where(x => x.Structure_Name != "N.A")
                        //.OrderBy(x => x.Structure_Name)
                        .Select(x => new { x.Structure_Name, x.Structure_ID });

                    if (_struc.Count() > 0)
                    {
                        dtStructure = _struc.ToDataTable();
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

            }

            return dtStructure;
        }

        //Fetch 

        #region SubNature   //created by atrayee


        public static DataTable getSubNature(int nature)
        {
            DataTable dtSubNature = null;
            try
            {
                using (var subnatureData = new SIP_ClientDataContext())
                {
                    var _fund = (from v in subnatureData.T_FUND_MASTER_clients

                                 where v.NATURE_ID == nature && v.SUB_NATURE_ID != null
                                 select new
                                 {
                                     _subNatureId = v.SUB_NATURE_ID
                                 }).Distinct();

                    var subnature = (from subnat in subnatureData.T_SCHEMES_SUB_NATURE_Clients
                                     join v in _fund on subnat.Sub_Nature_ID equals v._subNatureId
                                     where subnat.Sub_Nature != "N.A"
                                     orderby subnat.Sub_Nature
                                     select new
                                     {
                                         subnat.Sub_Nature,
                                         subnat.Sub_Nature_ID

                                     });

                    if (subnature.Any())
                    {
                        dtSubNature = subnature.ToDataTable();
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

            }

            return dtSubNature;
        }

        public static DataTable getAllSubNature()
        {
            DataTable dtSubNature = null;
            try
            {
                using (var subnatureData = new SIP_ClientDataContext())
                {
                    var _fund = (from v in subnatureData.T_FUND_MASTER_clients
                                 where v.SUB_NATURE_ID != null
                                 select new
                                 {
                                     _subNatureId = v.SUB_NATURE_ID
                                 }).Distinct();

                    var subnature = (from subnat in subnatureData.T_SCHEMES_SUB_NATURE_Clients
                                     join v in _fund on subnat.Sub_Nature_ID equals v._subNatureId
                                     where subnat.Sub_Nature != "N.A"
                                     orderby subnat.Sub_Nature
                                     select new
                                     {
                                         subnat.Sub_Nature,
                                         subnat.Sub_Nature_ID

                                     });

                    if (subnature.Any())
                    {
                        dtSubNature = subnature.ToDataTable();
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

            }

            return dtSubNature;
        }

        public static DataTable getSebiSubNature(int nature)
        {
            DataTable dtSubNature = null;
            try
            {
                using (var subnatureData = new SIP_ClientDataContext())
                {
                    var _fund = (from v in subnatureData.T_FUND_MASTER_clients
                                 where v.SEBI_NATURE_ID == nature && v.SEBI_SUB_NATURE_ID != null
                                 select new
                                 {
                                     _subNatureId = v.SEBI_SUB_NATURE_ID
                                 }).Distinct();

                    var subnature = (from subnat in subnatureData.T_SEBI_SCHEMES_SUB_NATUREs
                                     join v in _fund on subnat.Sebi_Sub_Nature_ID equals v._subNatureId.Value
                                     where subnat.Sebi_Sub_Nature != "N.A"
                                     orderby subnat.Sebi_Sub_Nature ascending
                                     select new
                                     {
                                         subnat.Sebi_Sub_Nature,
                                         subnat.Sebi_Sub_Nature_ID

                                     });

                    if (subnature.Any())
                    {
                        dtSubNature = subnature.ToDataTable();
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

            }

            return dtSubNature;
        }

        public static DataTable getAllSebiSubNature()
        {
            DataTable dtSubNature = null;
            try
            {
                using (var subnatureData = new SIP_ClientDataContext())
                {
                    var _fund = (from v in subnatureData.T_FUND_MASTER_clients
                                 where v.SEBI_SUB_NATURE_ID != null
                                 select new
                                 {
                                     _subNatureId = v.SEBI_SUB_NATURE_ID
                                 }).Distinct();

                    var subnature = (from subnat in subnatureData.T_SEBI_SCHEMES_SUB_NATUREs
                                     join v in _fund on subnat.Sebi_Sub_Nature_ID equals v._subNatureId.Value
                                     where subnat.Sebi_Sub_Nature != "N.A"
                                     orderby subnat.Sebi_Sub_Nature ascending
                                     select new
                                     {
                                         subnat.Sebi_Sub_Nature,
                                         subnat.Sebi_Sub_Nature_ID

                                     });

                    if (subnature.Any())
                    {
                        dtSubNature = subnature.ToDataTable();
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

            }

            return dtSubNature;
        }


        public static DataTable getOption()
        {
            DataTable dtOption = null;
            try
            {
                using (var subnatureData = new SIP_ClientDataContext())
                {
                    var _Option = (from v in subnatureData.T_SCHEMES_OPTION_Clients
                                   where v.Option_Name != "N.A"
                                   select new
                                   {
                                       Id = v.Option_ID,
                                       Name = v.Option_Name == "Income/Dividend" ? "Dividend" : v.Option_Name
                                   });

                    if (_Option.Any())
                    {
                        dtOption = _Option.ToDataTable();
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {

            }

            return dtOption;
        }

        #region Fund Risk Button



        #endregion

        //public static DataTable getSubNature( int nature)
        //{
        //        DataTable dtSubNature = null;
        //    try
        //    {
        //        using (var subnatureData = new SIP_ClientDataContext())
        //        {
        //            var _fund= (from v in subnatureData.T_FUND_MASTER_clients
        //                       where v.NATURE_ID== nature && v.SUB_NATURE_ID!=null
        //                       select new 
        //                       {
        //                           _subNatureId= v.SUB_NATURE_ID
        //                       }).Distinct().ToArray().Cast<decimal>();

        //            var subnature = (from subnat in subnatureData.T_SCHEMES_SUB_NATURE_Clients
        //                             where  _fund.Contains(subnat.Sub_Nature_ID)
        //                         select new
        //                         {
        //                             subnat.Sub_Nature,
        //                             subnat.Sub_Nature_ID

        //                         }).ToDataTable();

        //            //if (subnature.c > 0)
        //            //{
        //            //    dtSubNature = subnature.ToDataTable().OrderBy("Nature asc");
        //            //}
        //        }
        //    }
        //    catch (Exception exp)
        //    {

        //    }
        //    finally
        //    {

        //    }

        //    return dtSubNature;
        //}

        #endregion
        public static DataTable getTopFundRank(int Fetachrank, int Type, int Category, string Period)
        {

            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext())
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();

                    maxDate = maxDate.AddDays(-7);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 //join nd in dc.T_NAV_DIV_clients on  si.Scheme_Id equals nd.Scheme_Id
                                 where
                                 //tf.Scheme_Id == si.Scheme_Id
                                 //&& si.Fund_Id == fm.FUND_ID
                                 //&& fm.NATURE_ID == sn.Nature_ID
                                 //&& fm.STRUCTURE_ID == ss.Structure_ID                                 
                                 //&& si.Scheme_Id == nd.Scheme_Id
                                 tf.Calculation_Date >= maxDate
                                 // && nd.Nav_Date.Value == maxDate
                                 //  && sn.Nature_ID == Category
                                 //  && ss.Structure_ID == Type
                                 //orderby tf.Calculation_Date descending
                                 select new TopFundRank
                                 {
                                     Sch_Name = si.Sch_Short_Name,
                                     Calculation_Date = tf.Calculation_Date,
                                     Div_Yield = tf.Div_Yield,
                                     Per_7_Days = tf.Per_7_Days,
                                     Per_30_Days = tf.Per_30_Days,
                                     Per_91_Days = tf.Per_91_Days,
                                     Per_182_Days = tf.Per_182_Days,
                                     Per_1_Year = tf.Per_1_Year,
                                     Per_3_Year = tf.Per_3_Year,
                                     Per_5_Year = tf.Per_5_Year,
                                     Since_Inception = tf.Since_Inception,
                                     Nature_ID = fm.NATURE_ID.Value,
                                     Nature = sn.Nature,
                                     Structure_ID = ss.Structure_ID,
                                     Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                 });



                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.Nature_ID == Convert.ToDecimal(Category)).Distinct();

                    }

                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
                    }

                    //switch (Period)
                    //{
                    //    case "Per_7_Days": dynamicPeriod = "Per_7_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_7_Days desc";
                    //        break;
                    //    case "Per_30_Days": dynamicPeriod = "Per_30_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_30_Days desc";
                    //        break;
                    //    case "Per_91_Days": dynamicPeriod = "Per_91_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_91_Days desc";
                    //        break;
                    //    case "Per_182_Days": dynamicPeriod = "Per_182_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_182_Days desc";
                    //        break;
                    //    case "Per_1_Year": dynamicPeriod = "Per_1_Year!=null"; dynamicOrder = "Calculation_Date desc,Per_1_Year desc";
                    //        break;
                    //    case "Per_3_Year": dynamicPeriod = "Per_3_Year!=null"; dynamicOrder = "Calculation_Date desc,Per_3_Year desc";
                    //        break;
                    //    case "Per_5_Year": dynamicPeriod = "Per_5_Year!=null"; dynamicOrder = "Calculation_Date desc,Per_5_Year desc";
                    //        break;
                    //    default: dynamicPeriod = "Per_7_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_7_Days desc";
                    //        break;
                    //}
                    //_data = _data.OrderBy(dynamicOrder).Where(dynamicPeriod);

                    #region Without Dynamic Linq


                    switch (Period)
                    {
                        case "Per_7_Days":
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _data = _data.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _data = _data.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _data = _data.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _data = _data.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _data = _data.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _data = _data.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_7_Days);
                            break;
                    }

                    #endregion

                    if (_data.Count() > 0)
                    {
                        _data = _data.Select(x => x).Take(Fetachrank);
                        dtResult = _data.ToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }



        public static DataTable getTopFundRank(int Fetachrank, int Type, int Category, string Period,
         int SubCategory, int Option, int Risk, int min_invest, int MinSIreturn)
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
                    //var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
                    maxDate = maxDate.AddDays(-7);

                    //var _data1 = (from tf in dc.T_TOP_FUND_Clients
                    //              join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                    //              join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                    //              join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                    //              join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                    //              join sc in dc.T_SCHEMES_SUB_NATURE_Clients on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                    //              join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                    //              join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                    //              join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                    //              join rm in
                    //                  (from zz in dc.T_RANKING_MFR_Clients
                    //                   where
                    //                   zz.Quarter_Id == maxQuater
                    //                   && zz.MFR >= Rank
                    //                   && zz.Year_Check == YearChk
                    //                   select zz)
                    //              on si.Scheme_Id equals rm.Scheme_Id into grp

                    //              from x in grp.DefaultIfEmpty()
                    //              where
                    //    tf.Calculation_Date >= maxDate
                    //    && op.Option_ID == 3
                    //              select tf).ToDataTable();



                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SCHEMES_SUB_NATURE_Clients on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                                 join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                                 where

                                 tf.Calculation_Date >= maxDate
                                 && si.Nav_Check == 3
                                 && si.Sub_Nature2_Id != 46
                                 && si.Sub_Nature2_Id != 10
                                 && tf.Since_Inception > MinSIreturn
                                 select new
                                 {
                                     tf = tf,
                                     si = si,
                                     fm = fm,
                                     sn = sn,
                                     ss = ss,
                                     sc = sc,
                                     op = op,
                                     ri = ri,
                                     sd = sd
                                 });

                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.ss.Structure_ID == Convert.ToDecimal(Type));
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.op.Option_ID == Convert.ToDecimal(Option));
                    }

                    if (Risk != -1)
                    {
                        _data = _data.Where(x => x.ri.ID == Convert.ToDecimal(Risk));
                    }

                    _data = _data.Where(x => x.sd.Min_Investment >= min_invest);


                    //orderby tf.Calculation_Date descending
                    //select new TopFundRank
                    //{
                    //    //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //    Sch_Name = si.Sch_Short_Name,
                    //    SchemeId = si.Scheme_Id,
                    //    Calculation_Date = tf.Calculation_Date,
                    //    Div_Yield = tf.Div_Yield,
                    //    Per_7_Days = tf.Per_7_Days,
                    //    Per_30_Days = tf.Per_30_Days,
                    //    Per_91_Days = tf.Per_91_Days,
                    //    Per_182_Days = tf.Per_182_Days,
                    //    Per_1_Year = tf.Per_1_Year,
                    //    Per_3_Year = tf.Per_3_Year,
                    //    Per_5_Year = tf.Per_5_Year,
                    //    Since_Inception = tf.Since_Inception,
                    //    Nature_ID = fm.NATURE_ID.Value,
                    //    Nature = sn.Nature,
                    //    Structure_ID = ss.Structure_ID,
                    //    SubcategoryId = sc.Sub_Nature_ID,
                    //    SubNature = sc.Sub_Nature,
                    //    OptionId = op.Option_ID,
                    //    RiskColorId = ri.ID,
                    //    MinInvest = sd.Min_Investment,
                    //   //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //    Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x=> x.Scheme_Id==si.Scheme_Id).Max(y=>y.Nav_Date)).FirstOrDefault().Nav,
                    //    //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //    Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x=>x.SCHEME_ID==si.Scheme_Id).Max(y=>y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //}).Distinct();
                    IQueryable<TopFundRank> _FData;
                    if (SubCategory == 9000 || SubCategory == 9001 || SubCategory == 6)
                    {
                        var maxcaldate = (from dtf in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients orderby dtf.CALC_DATE descending select dtf.CALC_DATE).Max();
                        _data = _data.Where(x => (x.sn.Nature_ID == Convert.ToDecimal(3) && x.sc.Sub_Nature == "Diversified")).Distinct();
                        if (SubCategory == 9000)
                        {
                            _FData = (from dta in _data
                                      join mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients on dta.si.Fund_Id equals mcap.FUND_ID
                                      where mcap.CALC_DATE >= maxcaldate && mcap.MARKET_SLAB == "Large Cap" && mcap.MCAPALLOCATION > 65

                                      select new TopFundRank
                                      {
                                          //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                                          Sch_Name = dta.si.Sch_Short_Name,
                                          SchemeId = dta.si.Scheme_Id,
                                          Calculation_Date = dta.tf.Calculation_Date,
                                          Div_Yield = dta.tf.Div_Yield,
                                          Per_7_Days = dta.tf.Per_7_Days,
                                          Per_30_Days = dta.tf.Per_30_Days,
                                          Per_91_Days = dta.tf.Per_91_Days,
                                          Per_182_Days = dta.tf.Per_182_Days,
                                          Per_1_Year = dta.tf.Per_1_Year,
                                          Per_3_Year = dta.tf.Per_3_Year,
                                          Per_5_Year = dta.tf.Per_5_Year,
                                          Since_Inception = dta.tf.Since_Inception,
                                          Nature_ID = dta.sn.Nature_ID,
                                          Nature = dta.sn.Nature,
                                          Structure_ID = dta.ss.Structure_ID,
                                          SubcategoryId = dta.sc.Sub_Nature_ID,
                                          SubNature = "Large Cap",
                                          OptionId = dta.op.Option_ID,
                                          RiskColorId = dta.ri.ID,
                                          MinInvest = dta.sd.Min_Investment,
                                          //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                          Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                                          //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                          Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                                      }).Distinct();
                        }
                        else if (SubCategory == 9001)
                        {
                            var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                                             group mcap by mcap.FUND_ID into newGroupwhere
                                             select new
                                             {
                                                 FUND_ID = newGroupwhere.Key,
                                                 Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                                             }
                                            );

                            _FData = (from dta in _data
                                      join mcap in _mcapdata on dta.si.Fund_Id equals mcap.FUND_ID
                                      where mcap.Cap > 65
                                      select new TopFundRank
                                      {
                                          //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                                          Sch_Name = dta.si.Sch_Short_Name,
                                          SchemeId = dta.si.Scheme_Id,
                                          Calculation_Date = dta.tf.Calculation_Date,
                                          Div_Yield = dta.tf.Div_Yield,
                                          Per_7_Days = dta.tf.Per_7_Days,
                                          Per_30_Days = dta.tf.Per_30_Days,
                                          Per_91_Days = dta.tf.Per_91_Days,
                                          Per_182_Days = dta.tf.Per_182_Days,
                                          Per_1_Year = dta.tf.Per_1_Year,
                                          Per_3_Year = dta.tf.Per_3_Year,
                                          Per_5_Year = dta.tf.Per_5_Year,
                                          Since_Inception = dta.tf.Since_Inception,
                                          Nature_ID = dta.sn.Nature_ID,
                                          Nature = dta.sn.Nature,
                                          Structure_ID = dta.ss.Structure_ID,
                                          SubcategoryId = dta.sc.Sub_Nature_ID,
                                          SubNature = "Mid & Small Cap",
                                          OptionId = dta.op.Option_ID,
                                          RiskColorId = dta.ri.ID,
                                          MinInvest = dta.sd.Min_Investment,
                                          //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                          Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                                          //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                          Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                                      }).Distinct();
                        }
                        else
                        {
                            var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => p.MARKET_SLAB == "Large Cap" && p.CALC_DATE >= maxcaldate && p.MCAPALLOCATION > 65)
                                             select new
                                             {
                                                 FUND_ID = mcap.FUND_ID
                                             }
                                            ).Distinct();

                            var _mcapdata_temp = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                                                  group mcap by mcap.FUND_ID into newGroupwhere
                                                  select new
                                                  {
                                                      FUND_ID = newGroupwhere.Key,
                                                      Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                                                  }
                                            );

                            var _mcapdata_temp2 = (from mcap in _mcapdata_temp.Where(p => p.Cap > 65)
                                                   select new
                                                   {
                                                       FUND_ID = mcap.FUND_ID
                                                   }
                                            ).Distinct();

                            _mcapdata = (from u in _mcapdata select new { u.FUND_ID })
                                            .Union(from u in _mcapdata_temp2 select new { u.FUND_ID }).Distinct();

                            _FData = (from dta in _data
                                      join mcap in _mcapdata on dta.si.Fund_Id equals mcap.FUND_ID into p
                                      from mcap in p.DefaultIfEmpty()
                                      where mcap.FUND_ID == null
                                      select new TopFundRank
                                      {
                                          //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                                          Sch_Name = dta.si.Sch_Short_Name,
                                          SchemeId = dta.si.Scheme_Id,
                                          Calculation_Date = dta.tf.Calculation_Date,
                                          Div_Yield = dta.tf.Div_Yield,
                                          Per_7_Days = dta.tf.Per_7_Days,
                                          Per_30_Days = dta.tf.Per_30_Days,
                                          Per_91_Days = dta.tf.Per_91_Days,
                                          Per_182_Days = dta.tf.Per_182_Days,
                                          Per_1_Year = dta.tf.Per_1_Year,
                                          Per_3_Year = dta.tf.Per_3_Year,
                                          Per_5_Year = dta.tf.Per_5_Year,
                                          Since_Inception = dta.tf.Since_Inception,
                                          Nature_ID = dta.sn.Nature_ID,
                                          Nature = dta.sn.Nature,
                                          Structure_ID = dta.ss.Structure_ID,
                                          SubcategoryId = dta.sc.Sub_Nature_ID,
                                          SubNature = "Diversified",
                                          OptionId = dta.op.Option_ID,
                                          RiskColorId = dta.ri.ID,
                                          MinInvest = dta.sd.Min_Investment,
                                          //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                          Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                                          //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                          Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                                      }).Distinct();
                        }

                    }
                    else
                    {
                        if (Category != -1)
                        {
                            _data = _data.Where(x => x.sn.Nature_ID == Convert.ToDecimal(Category));
                        }
                        if (SubCategory != -1)
                        {
                            _data = _data.Where(x => x.sc.Sub_Nature_ID == Convert.ToDecimal(SubCategory));
                        }
                        _FData = (from dta in _data
                                  select new TopFundRank
                                  {
                                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                                      Sch_Name = dta.si.Sch_Short_Name,
                                      SchemeId = dta.si.Scheme_Id,
                                      Calculation_Date = dta.tf.Calculation_Date,
                                      Div_Yield = dta.tf.Div_Yield,
                                      Per_7_Days = dta.tf.Per_7_Days,
                                      Per_30_Days = dta.tf.Per_30_Days,
                                      Per_91_Days = dta.tf.Per_91_Days,
                                      Per_182_Days = dta.tf.Per_182_Days,
                                      Per_1_Year = dta.tf.Per_1_Year,
                                      Per_3_Year = dta.tf.Per_3_Year,
                                      Per_5_Year = dta.tf.Per_5_Year,
                                      Since_Inception = dta.tf.Since_Inception,
                                      Nature_ID = dta.sn.Nature_ID,
                                      Nature = dta.sn.Nature,
                                      Structure_ID = dta.ss.Structure_ID,
                                      SubcategoryId = dta.sc.Sub_Nature_ID,
                                      SubNature = dta.sc.Sub_Nature,
                                      OptionId = dta.op.Option_ID,
                                      RiskColorId = dta.ri.ID,
                                      MinInvest = dta.sd.Min_Investment,
                                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                                  }).Distinct();
                    }
                    #region Without Dynamic Linq

                    switch (Period)
                    {
                        case "Per_7_Days":
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _FData = _FData.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _FData = _FData.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _FData = _FData.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _FData = _FData.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _FData = _FData.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _FData = _FData.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                    }

                    #endregion

                    //if (_FData.Count() > 0)
                    //{
                    if (Fetachrank != -1)
                        _FData = _FData.Select(x => x).Take(Fetachrank);
                    else
                        _FData = _FData.Select(x => x);
                    dtResult = _FData.ToDataTable();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        ///sebi seacrh submit
        public static DataTable getSebiTopFundRank(int Fetachrank, int Type, int Category, string Period,
         int SubCategory, int Option, int Risk, int min_invest, int MinSIreturn)
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
                    //var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
                    maxDate = maxDate.AddDays(-7);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                                 join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                                 where

                                 tf.Calculation_Date >= maxDate
                                 && si.Nav_Check == 3
                                 && si.Sub_Nature2_Id != 46 //commented by syed 1611
                                 //&& si.Sub_Nature2_Id != 10 //commented by syed 1611
                                 && tf.Since_Inception > MinSIreturn
                                 select new
                                 {
                                     tf = tf,
                                     si = si,
                                     fm = fm,
                                     sn = sn,
                                     ss = ss,
                                     sc = sc,
                                     op = op,
                                     ri = ri,
                                     sd = sd
                                 });
                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.ss.Structure_ID == Convert.ToDecimal(Type));
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.op.Option_ID == Convert.ToDecimal(Option));
                    }

                    if (Risk != -1)
                    {
                        _data = _data.Where(x => x.ri.COLOR_CODE == Convert.ToDecimal(Risk));
                    }

                    //Commented on 27th Apr 2022 after taking approval from Business and marketing team
                    //_data = _data.Where(x => x.sd.Min_Investment >= min_invest);


                    IQueryable<TopFundRank> _FData;
                    #region comment
                    //if (SubCategory == 9000 || SubCategory == 9001 || SubCategory == 6)
                    //{
                    //    var maxcaldate = (from dtf in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients orderby dtf.CALC_DATE descending select dtf.CALC_DATE).Max();
                    //    _data = _data.Where(x => (x.sn.Sebi_Nature_ID == Convert.ToDecimal(3) && x.sc.Sebi_Sub_Nature == "Diversified")).Distinct();
                    //    if (SubCategory == 9000)
                    //    {
                    //        _FData = (from dta in _data
                    //                  join mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients on dta.si.Fund_Id equals mcap.FUND_ID
                    //                  where mcap.CALC_DATE >= maxcaldate && mcap.MARKET_SLAB == "Large Cap" && mcap.MCAPALLOCATION > 65

                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Large Cap",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }
                    //    else if (SubCategory == 9001)
                    //    {
                    //        var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                    //                         group mcap by mcap.FUND_ID into newGroupwhere
                    //                         select new
                    //                         {
                    //                             FUND_ID = newGroupwhere.Key,
                    //                             Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                    //                         }
                    //                        );

                    //        _FData = (from dta in _data
                    //                  join mcap in _mcapdata on dta.si.Fund_Id equals mcap.FUND_ID
                    //                  where mcap.Cap > 65
                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Mid & Small Cap",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }
                    //    else
                    //    {
                    //        var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => p.MARKET_SLAB == "Large Cap" && p.CALC_DATE >= maxcaldate && p.MCAPALLOCATION > 65)
                    //                         select new
                    //                         {
                    //                             FUND_ID = mcap.FUND_ID
                    //                         }
                    //                        ).Distinct();

                    //        var _mcapdata_temp = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                    //                              group mcap by mcap.FUND_ID into newGroupwhere
                    //                              select new
                    //                              {
                    //                                  FUND_ID = newGroupwhere.Key,
                    //                                  Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                    //                              }
                    //                        );

                    //        var _mcapdata_temp2 = (from mcap in _mcapdata_temp.Where(p => p.Cap > 65)
                    //                               select new
                    //                               {
                    //                                   FUND_ID = mcap.FUND_ID
                    //                               }
                    //                        ).Distinct();

                    //        _mcapdata = (from u in _mcapdata select new { u.FUND_ID })
                    //                        .Union(from u in _mcapdata_temp2 select new { u.FUND_ID }).Distinct();

                    //        _FData = (from dta in _data
                    //                  join mcap in _mcapdata on dta.si.Fund_Id equals mcap.FUND_ID into p
                    //                  from mcap in p.DefaultIfEmpty()
                    //                  where mcap.FUND_ID == null
                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Diversified",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }

                    //}
                    //else
                    //{
                    #endregion

                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.sn.Sebi_Nature_ID == Convert.ToDecimal(Category));
                    }
                    if (SubCategory != -1)
                    {
                        _data = _data.Where(x => x.sc.Sebi_Sub_Nature_ID == Convert.ToDecimal(SubCategory));
                    }
                    var dd = _data.Select(v => v.ri.ID).ToArray().Distinct();
                    _FData = (from dta in _data
                              select new TopFundRank
                              {
                                  //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                                  Sch_Name = dta.si.Sch_Short_Name,
                                  SchemeId = dta.si.Scheme_Id,
                                  Calculation_Date = dta.tf.Calculation_Date,
                                  Div_Yield = dta.tf.Div_Yield,
                                  Per_7_Days = dta.tf.Per_7_Days,
                                  Per_30_Days = dta.tf.Per_30_Days,
                                  Per_91_Days = dta.tf.Per_91_Days,
                                  Per_182_Days = dta.tf.Per_182_Days,
                                  Per_1_Year = dta.tf.Per_1_Year,
                                  Per_3_Year = dta.tf.Per_3_Year,
                                  Per_5_Year = dta.tf.Per_5_Year,
                                  Since_Inception = dta.tf.Since_Inception,
                                  Nature_ID = dta.sn.Sebi_Nature_ID,
                                  Nature = dta.sn.Sebi_Nature,
                                  Structure_ID = dta.ss.Structure_ID,
                                  SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                                  SubNature = dta.sc.Sebi_Sub_Nature,
                                  OptionId = dta.op.Option_ID,
                                  RiskColorId = dta.ri.ID,
                                  MinInvest = dta.sd.Min_Investment,
                                  //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                  Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                                  //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                  Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                              }).Distinct();
                    //  }
                    #region Without Dynamic Linq

                    switch (Period)
                    {
                        case "Per_7_Days":
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _FData = _FData.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _FData = _FData.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _FData = _FData.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _FData = _FData.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _FData = _FData.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _FData = _FData.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                    }

                    #endregion

                    //if (_FData.Count() > 0)
                    //{
                    if (Fetachrank != -1)
                        _FData = _FData.Select(x => x).Take(Fetachrank);
                    else
                        _FData = _FData.Select(x => x);
                    dtResult = _FData.ToDataTable();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        public static DataTable getSebiTopFundRank1(int Fetachrank, int Type, int Category, string Period,
        int SubCategory, int Option, List<int> Risk, int min_invest, int MinSIreturn)
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
                    //var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
                    maxDate = maxDate.AddDays(-7);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                                 join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                                 where

                                 tf.Calculation_Date >= maxDate
                                 && si.Nav_Check == 3
                                 && si.Sub_Nature2_Id != 46 //commented by syed 1611
                                 //&& si.Sub_Nature2_Id != 10 //commented by syed 1611
                                 && tf.Since_Inception > MinSIreturn
                                 select new
                                 {
                                     tf = tf,
                                     si = si,
                                     fm = fm,
                                     sn = sn,
                                     ss = ss,
                                     sc = sc,
                                     op = op,
                                     ri = ri,
                                     sd = sd
                                 });
                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.ss.Structure_ID == Convert.ToDecimal(Type));
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.op.Option_ID == Convert.ToDecimal(Option));
                    }

                    if (!Risk.Contains(-1))
                    {

                        //_data = _data.Where(x =>  x.ri.COLOR_CODE == Convert.ToDecimal(Risk));
                        _data = _data.Where(x => Risk.Contains(x.ri.COLOR_CODE.Value));

                    }

                    //Commented on 27th Apr 2022 after taking approval from Business and marketing team
                    //_data = _data.Where(x => x.sd.Min_Investment <= min_invest);


                    IQueryable<TopFundRank> _FData;
                    #region commented
                    //if (SubCategory == 9000 || SubCategory == 9001 || SubCategory == 6)
                    //{
                    //    var maxcaldate = (from dtf in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients orderby dtf.CALC_DATE descending select dtf.CALC_DATE).Max();
                    //    _data = _data.Where(x => (x.sn.Sebi_Nature_ID == Convert.ToDecimal(3) && x.sc.Sebi_Sub_Nature == "Diversified")).Distinct();
                    //    if (SubCategory == 9000)
                    //    {
                    //        _FData = (from dta in _data
                    //                  join mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients on dta.si.Fund_Id equals mcap.FUND_ID
                    //                  where mcap.CALC_DATE >= maxcaldate && mcap.MARKET_SLAB == "Large Cap" && mcap.MCAPALLOCATION > 65

                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Large Cap",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }
                    //    else if (SubCategory == 9001)
                    //    {
                    //        var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                    //                         group mcap by mcap.FUND_ID into newGroupwhere
                    //                         select new
                    //                         {
                    //                             FUND_ID = newGroupwhere.Key,
                    //                             Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                    //                         }
                    //                        );

                    //        _FData = (from dta in _data
                    //                  join mcap in _mcapdata on dta.si.Fund_Id equals mcap.FUND_ID
                    //                  where mcap.Cap > 65
                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Mid & Small Cap",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }
                    //    else
                    //    {
                    //        var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => p.MARKET_SLAB == "Large Cap" && p.CALC_DATE >= maxcaldate && p.MCAPALLOCATION > 65)
                    //                         select new
                    //                         {
                    //                             FUND_ID = mcap.FUND_ID
                    //                         }
                    //                        ).Distinct();

                    //        var _mcapdata_temp = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                    //                              group mcap by mcap.FUND_ID into newGroupwhere
                    //                              select new
                    //                              {
                    //                                  FUND_ID = newGroupwhere.Key,
                    //                                  Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                    //                              }
                    //                        );

                    //        var _mcapdata_temp2 = (from mcap in _mcapdata_temp.Where(p => p.Cap > 65)
                    //                               select new
                    //                               {
                    //                                   FUND_ID = mcap.FUND_ID
                    //                               }
                    //                        ).Distinct();

                    //        _mcapdata = (from u in _mcapdata select new { u.FUND_ID })
                    //                        .Union(from u in _mcapdata_temp2 select new { u.FUND_ID }).Distinct();

                    //        _FData = (from dta in _data
                    //                  join mcap in _mcapdata on dta.si.Fund_Id equals mcap.FUND_ID into p
                    //                  from mcap in p.DefaultIfEmpty()
                    //                  where mcap.FUND_ID == null
                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Diversified",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }

                    //}
                    //else
                    //{
                    #endregion
                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.sn.Sebi_Nature_ID == Convert.ToDecimal(Category));
                    }
                    if (SubCategory != -1)
                    {
                        _data = _data.Where(x => x.sc.Sebi_Sub_Nature_ID == Convert.ToDecimal(SubCategory));
                    }
                    var dd = _data.Select(v => v.ri.ID).ToArray().Distinct();
                    _FData = (from dta in _data
                              select new TopFundRank
                              {
                                  //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                                  Sch_Name = dta.si.Sch_Short_Name,
                                  SchemeId = dta.si.Scheme_Id,
                                  Calculation_Date = dta.tf.Calculation_Date,
                                  Div_Yield = dta.tf.Div_Yield,
                                  Per_7_Days = dta.tf.Per_7_Days,
                                  Per_30_Days = dta.tf.Per_30_Days,
                                  Per_91_Days = dta.tf.Per_91_Days,
                                  Per_182_Days = dta.tf.Per_182_Days,
                                  Per_1_Year = dta.tf.Per_1_Year,
                                  Per_3_Year = dta.tf.Per_3_Year,
                                  Per_5_Year = dta.tf.Per_5_Year,
                                  Since_Inception = dta.tf.Since_Inception,
                                  Nature_ID = dta.sn.Sebi_Nature_ID,
                                  Nature = dta.sn.Sebi_Nature,
                                  Structure_ID = dta.ss.Structure_ID,
                                  SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                                  SubNature = dta.sc.Sebi_Sub_Nature,
                                  OptionId = dta.op.Option_ID,
                                  RiskColorId = dta.ri.ID,
                                  MinInvest = dta.sd.Min_Investment,
                                  //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                  Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                                  //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                  Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                              }).Distinct();

                    _FData = _FData.Where(x => x.Fund_Size <= Convert.ToDouble(min_invest));
                    //  }
                    #region Without Dynamic Linq

                    switch (Period)
                    {
                        case "Per_7_Days":
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _FData = _FData.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _FData = _FData.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _FData = _FData.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _FData = _FData.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _FData = _FData.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _FData = _FData.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                    }

                    #endregion

                    //if (_FData.Count() > 0)
                    //{
                    if (Fetachrank != -1)
                        _FData = _FData.Select(x => x).Take(Fetachrank);
                    else
                        _FData = _FData.Select(x => x);
                    dtResult = _FData.ToDataTable();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        public static DataTable getSebiTopFundRank(int Fetachrank, int Type, int Category, string Period,
         int SubCategory, int Option, int Risk, int min_invest, int MinSIreturn, int MutualFund_ID)//int Risk,
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
                    //var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
                    maxDate = maxDate.AddDays(-7);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                                 join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                                 where

                                 tf.Calculation_Date >= maxDate
                                 && si.Nav_Check == 3
                                 && si.Sub_Nature2_Id != 46 //commented by syed 1611
                                 //&& si.Sub_Nature2_Id != 10 //commented by syed 1611
                                 && tf.Since_Inception > MinSIreturn
                                 select new
                                 {
                                     tf = tf,
                                     si = si,
                                     fm = fm,
                                     sn = sn,
                                     ss = ss,
                                     sc = sc,
                                     op = op,
                                     ri = ri,
                                     sd = sd
                                 });

                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.ss.Structure_ID == Convert.ToDecimal(Type));
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.op.Option_ID == Convert.ToDecimal(Option));
                    }

                    //if (Risk != -1)
                    //{
                    //    _data = _data.Where(x => x.ri.ID == Convert.ToDecimal(Risk));
                    //}
                    if (MutualFund_ID > 0)
                    {
                        _data = _data.Where(x => x.fm.MUTUALFUND_ID == Convert.ToDecimal(MutualFund_ID));
                    }

                    //_data = _data.Where(x => x.sd.Min_Investment >= min_invest);


                    IQueryable<TopFundRank> _FData;
                    //if (SubCategory == 9000 || SubCategory == 9001 || SubCategory == 6)
                    //{
                    //    var maxcaldate = (from dtf in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients orderby dtf.CALC_DATE descending select dtf.CALC_DATE).Max();
                    //    _data = _data.Where(x => (x.sn.Sebi_Nature_ID == Convert.ToDecimal(3) && x.sc.Sebi_Sub_Nature == "Diversified")).Distinct();
                    //    if (SubCategory == 9000)
                    //    {
                    //        _FData = (from dta in _data
                    //                  join mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients on dta.si.Fund_Id equals mcap.FUND_ID
                    //                  where mcap.CALC_DATE >= maxcaldate && mcap.MARKET_SLAB == "Large Cap" && mcap.MCAPALLOCATION > 65

                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Large Cap",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }
                    //    else if (SubCategory == 9001)
                    //    {
                    //        var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                    //                         group mcap by mcap.FUND_ID into newGroupwhere
                    //                         select new
                    //                         {
                    //                             FUND_ID = newGroupwhere.Key,
                    //                             Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                    //                         }
                    //                        );

                    //        _FData = (from dta in _data
                    //                  join mcap in _mcapdata on dta.si.Fund_Id equals mcap.FUND_ID
                    //                  where mcap.Cap > 65
                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Mid & Small Cap",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }
                    //    else
                    //    {
                    //        var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => p.MARKET_SLAB == "Large Cap" && p.CALC_DATE >= maxcaldate && p.MCAPALLOCATION > 65)
                    //                         select new
                    //                         {
                    //                             FUND_ID = mcap.FUND_ID
                    //                         }
                    //                        ).Distinct();

                    //        var _mcapdata_temp = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                    //                              group mcap by mcap.FUND_ID into newGroupwhere
                    //                              select new
                    //                              {
                    //                                  FUND_ID = newGroupwhere.Key,
                    //                                  Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                    //                              }
                    //                        );

                    //        var _mcapdata_temp2 = (from mcap in _mcapdata_temp.Where(p => p.Cap > 65)
                    //                               select new
                    //                               {
                    //                                   FUND_ID = mcap.FUND_ID
                    //                               }
                    //                        ).Distinct();

                    //        _mcapdata = (from u in _mcapdata select new { u.FUND_ID })
                    //                        .Union(from u in _mcapdata_temp2 select new { u.FUND_ID }).Distinct();

                    //        _FData = (from dta in _data
                    //                  join mcap in _mcapdata on dta.si.Fund_Id equals mcap.FUND_ID into p
                    //                  from mcap in p.DefaultIfEmpty()
                    //                  where mcap.FUND_ID == null
                    //                  select new TopFundRank
                    //                  {
                    //                      //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                    //                      Sch_Name = dta.si.Sch_Short_Name,
                    //                      SchemeId = dta.si.Scheme_Id,
                    //                      Calculation_Date = dta.tf.Calculation_Date,
                    //                      Div_Yield = dta.tf.Div_Yield,
                    //                      Per_7_Days = dta.tf.Per_7_Days,
                    //                      Per_30_Days = dta.tf.Per_30_Days,
                    //                      Per_91_Days = dta.tf.Per_91_Days,
                    //                      Per_182_Days = dta.tf.Per_182_Days,
                    //                      Per_1_Year = dta.tf.Per_1_Year,
                    //                      Per_3_Year = dta.tf.Per_3_Year,
                    //                      Per_5_Year = dta.tf.Per_5_Year,
                    //                      Since_Inception = dta.tf.Since_Inception,
                    //                      Nature_ID = dta.sn.Sebi_Nature_ID,
                    //                      Nature = dta.sn.Sebi_Nature,
                    //                      Structure_ID = dta.ss.Structure_ID,
                    //                      SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                    //                      SubNature = "Diversified",
                    //                      OptionId = dta.op.Option_ID,
                    //                      RiskColorId = dta.ri.ID,
                    //                      MinInvest = dta.sd.Min_Investment,
                    //                      //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                    //                      Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                    //                      //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                    //                      Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                    //                  }).Distinct();
                    //    }

                    //}
                    //else
                    //{
                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.sn.Sebi_Nature_ID == Convert.ToDecimal(Category));
                    }
                    if (SubCategory != -1)
                    {
                        _data = _data.Where(x => x.sc.Sebi_Sub_Nature_ID == Convert.ToDecimal(SubCategory));
                    }
                    _FData = (from dta in _data
                              select new TopFundRank
                              {
                                  //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                                  Sch_Name = dta.si.Sch_Short_Name,
                                  SchemeId = dta.si.Scheme_Id,
                                  AmfiCode=dta.si.Amfi_Code,
                                  Calculation_Date = dta.tf.Calculation_Date,
                                  Div_Yield = dta.tf.Div_Yield,
                                  Per_7_Days = dta.tf.Per_7_Days,
                                  Per_30_Days = dta.tf.Per_30_Days,
                                  Per_91_Days = dta.tf.Per_91_Days,
                                  Per_182_Days = dta.tf.Per_182_Days,
                                  Per_1_Year = dta.tf.Per_1_Year,
                                  Per_3_Year = dta.tf.Per_3_Year,
                                  Per_5_Year = dta.tf.Per_5_Year,
                                  Since_Inception = dta.tf.Since_Inception,
                                  Nature_ID = dta.sn.Sebi_Nature_ID,
                                  Nature = dta.sn.Sebi_Nature,
                                  Structure_ID = dta.ss.Structure_ID,
                                  SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                                  SubNature = dta.sc.Sebi_Sub_Nature,
                                  OptionId = dta.op.Option_ID,
                                  RiskColorId = dta.ri.ID,
                                  MinInvest = dta.sd.Min_Investment,
                                  //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                  Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                                  //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                  Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                              }).Distinct();
                    //  }
                    #region Without Dynamic Linq

                    switch (Period)
                    {
                        case "Per_7_Days":
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _FData = _FData.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _FData = _FData.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _FData = _FData.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _FData = _FData.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _FData = _FData.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _FData = _FData.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Per_5_Year);
                            break;
                        case "Since_Inception":
                            _FData = _FData.Where(x => x.Since_Inception != null).OrderByDescending(x => x.Since_Inception);
                            break;
                        default:
                            _FData = _FData.Where(x => x.Fund_Size != null).OrderByDescending(x => x.Fund_Size);
                            break;
                    }

                    #endregion

                    //if (_FData.Count() > 0)
                    //{
                    if (Fetachrank != -1)
                        _FData = _FData.Select(x => x).Take(Fetachrank);
                    else
                        _FData = _FData.Select(x => x);
                    dtResult = _FData.ToDataTable();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        public static DataTable getSebiTopFundRankBluechip(int Fetachrank, int Type, int Category, string Period,
        int SubCategory, int Option, int Risk, int min_invest, int MinSIreturn, List<decimal> MutualFund_ID)//int Risk,
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
                    //var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
                    maxDate = maxDate.AddDays(-7);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                                 join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                                 where

                                 tf.Calculation_Date >= maxDate
                                 && si.Nav_Check == 3
                                 && si.Sub_Nature2_Id != 46 //commented by syed 1611
                                 //&& si.Sub_Nature2_Id != 10 //commented by syed 1611
                                 && tf.Since_Inception > MinSIreturn
                                 select new
                                 {
                                     tf = tf,
                                     si = si,
                                     fm = fm,
                                     sn = sn,
                                     ss = ss,
                                     sc = sc,
                                     op = op,
                                     ri = ri,
                                     sd = sd
                                 });

                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.ss.Structure_ID == Convert.ToDecimal(Type));
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.op.Option_ID == Convert.ToDecimal(Option));
                    }
                    if (MutualFund_ID != null && MutualFund_ID.Count > 0)
                    {
                        _data = _data.Where(x => MutualFund_ID.Contains(x.fm.MUTUALFUND_ID));
                    }

                    _data = _data.Where(x => x.sd.Min_Investment >= min_invest);


                    IQueryable<TopFundRank> _FData;
                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.sn.Sebi_Nature_ID == Convert.ToDecimal(Category));
                    }
                    if (SubCategory != -1)
                    {
                        _data = _data.Where(x => x.sc.Sebi_Sub_Nature_ID == Convert.ToDecimal(SubCategory));
                    }
                    _FData = (from dta in _data
                              select new TopFundRank
                              {
                                  //Sch_Name = "<a href='Factsheet.aspx?param="+si.Scheme_Id+">"+si.Sch_Short_Name+"</a>",
                                  Sch_Name = dta.si.Sch_Short_Name,
                                  SchemeId = dta.si.Scheme_Id,
                                  Calculation_Date = dta.tf.Calculation_Date,
                                  Div_Yield = dta.tf.Div_Yield,
                                  Per_7_Days = dta.tf.Per_7_Days,
                                  Per_30_Days = dta.tf.Per_30_Days,
                                  Per_91_Days = dta.tf.Per_91_Days,
                                  Per_182_Days = dta.tf.Per_182_Days,
                                  Per_1_Year = dta.tf.Per_1_Year,
                                  Per_3_Year = dta.tf.Per_3_Year,
                                  Per_5_Year = dta.tf.Per_5_Year,
                                  Since_Inception = dta.tf.Since_Inception,
                                  Nature_ID = dta.sn.Sebi_Nature_ID,
                                  Nature = dta.sn.Sebi_Nature,
                                  Structure_ID = dta.ss.Structure_ID,
                                  SubcategoryId = dta.sc.Sebi_Sub_Nature_ID,
                                  SubNature = dta.sc.Sebi_Sub_Nature,
                                  OptionId = dta.op.Option_ID,
                                  RiskColorId = dta.ri.ID,
                                  MinInvest = dta.sd.Min_Investment,
                                  //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                  Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == dta.si.Scheme_Id && v.Nav_Date == dc.T_NAV_DIV_clients.Where(x => x.Scheme_Id == dta.si.Scheme_Id).Max(y => y.Nav_Date)).FirstOrDefault().Nav,
                                  //Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                  Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == dta.si.Scheme_Id && v.FUNDSIZE_DATE == dc.T_FUND_SIZEs.Where(x => x.SCHEME_ID == dta.si.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE
                              }).Distinct();
                    //  }
                    #region Without Dynamic Linq

                    switch (Period)
                    {
                        case "Per_7_Days":
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _FData = _FData.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _FData = _FData.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _FData = _FData.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _FData = _FData.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _FData = _FData.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _FData = _FData.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _FData = _FData.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                    }

                    #endregion

                    //if (_FData.Count() > 0)
                    //{
                    if (Fetachrank != -1)
                        _FData = _FData.Select(x => x).Take(Fetachrank);
                    else
                        _FData = _FData.Select(x => x);
                    dtResult = _FData.ToDataTable();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }


        public static DataTable getTopFundRank(int Type, int Category, string Period, int SubCategory, int Option)
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext())
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
                    //var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
                    maxDate = maxDate.AddDays(-7);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SCHEMES_SUB_NATURE_Clients on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                                 join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                                 //join rm in
                                 //    (from zz in dc.T_RANKING_MFR_Clients
                                 //     where zz.Quarter_Id == maxQuater && zz.Year_Check == RankYearChk
                                 //     select zz)
                                 //on si.Scheme_Id equals rm.Scheme_Id into grp
                                 //from x in grp.DefaultIfEmpty()
                                 where tf.Calculation_Date >= maxDate
                                 select new TopFundRank
                                 {
                                     SchemeId = si.Scheme_Id,
                                     Sch_Name = si.Sch_Short_Name,
                                     Calculation_Date = tf.Calculation_Date,
                                     Div_Yield = tf.Div_Yield,
                                     Per_7_Days = tf.Per_7_Days,
                                     Per_30_Days = tf.Per_30_Days,
                                     Per_91_Days = tf.Per_91_Days,
                                     Per_182_Days = tf.Per_182_Days,
                                     Per_1_Year = tf.Per_1_Year,
                                     Per_3_Year = tf.Per_3_Year,
                                     Per_5_Year = tf.Per_5_Year,
                                     Since_Inception = tf.Since_Inception,
                                     Nature_ID = fm.NATURE_ID.Value,
                                     Nature = sn.Nature,
                                     Structure_ID = ss.Structure_ID,
                                     SubcategoryId = sc.Sub_Nature_ID,
                                     SubNature = sc.Sub_Nature,
                                     OptionId = op.Option_ID,
                                     RiskColorId = ri.ID,
                                     MinInvest = sd.Min_Investment,
                                     //MF_Rank = x.MFR,
                                     //MF_Rank_Html = getIcronRankHtml(Convert.ToInt32(x.MFR)),
                                     Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                 }).Distinct();


                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.Nature_ID == Convert.ToDecimal(Category)).Distinct();
                    }

                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
                    }

                    if (SubCategory != -1)
                    {
                        _data = _data.Where(x => x.SubcategoryId == Convert.ToDecimal(SubCategory)).Distinct();
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.OptionId == Convert.ToDecimal(Option)).Distinct();
                    }
                    _data = _data.Distinct();

                    switch (Period)
                    {
                        case "Per_7_Days":
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _data = _data.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _data = _data.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _data = _data.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _data = _data.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _data = _data.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _data = _data.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                    }

                    if (_data.Count() > 0)
                    {
                        dtResult = _data.Select(x => x).ToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }


        public static DataTable getSchemeDetails(List<WatchScheme> _LstSchmes, string Period)
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext())
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
                    var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
                    maxDate = maxDate.AddDays(-7);
                    var uo = _LstSchmes.Select(s => s.SchemeId).ToList();
                    var _Schmes = dc.T_SCHEMES_MASTER_Clients.Where(x => uo.Contains(Convert.ToInt32(x.Scheme_Id))).Select(c => c);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in _Schmes on tf.Scheme_Id equals si.Scheme_Id

                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SCHEMES_SUB_NATURE_Clients on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                                 join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                                 //join rm in
                                 //    (from zz in dc.T_RANKING_MFR_Clients
                                 //     where zz.Quarter_Id == maxQuater && zz.Year_Check == RankYearChk
                                 //     select zz)
                                 //on si.Scheme_Id equals rm.Scheme_Id into grp
                                 //from x in grp.DefaultIfEmpty()
                                 where tf.Calculation_Date >= maxDate
                                 select new TopFundRank
                                 {
                                     SchemeId = si.Scheme_Id,
                                     Sch_Name = si.Sch_Short_Name,
                                     Calculation_Date = tf.Calculation_Date,
                                     Div_Yield = tf.Div_Yield,
                                     Per_7_Days = tf.Per_7_Days,
                                     Per_30_Days = tf.Per_30_Days,
                                     Per_91_Days = tf.Per_91_Days,
                                     Per_182_Days = tf.Per_182_Days,
                                     Per_1_Year = tf.Per_1_Year,
                                     Per_3_Year = tf.Per_3_Year,
                                     Per_5_Year = tf.Per_5_Year,
                                     Since_Inception = tf.Since_Inception,
                                     Nature_ID = fm.NATURE_ID.Value,
                                     Nature = sn.Nature,
                                     Structure_ID = ss.Structure_ID,
                                     SubcategoryId = sc.Sub_Nature_ID,
                                     SubNature = sc.Sub_Nature,
                                     OptionId = op.Option_ID,
                                     RiskColorId = ri.ID,
                                     MinInvest = sd.Min_Investment,
                                     //MF_Rank = x.MFR,
                                     //MF_Rank_Html = getIcronRankHtml(x.MFR),
                                     Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                     Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                 }).Distinct();


                    //var bbb = _data.ToList();
                    _data = _data.Distinct();

                    switch (Period)
                    {
                        case "Per_7_Days":
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            
                            break;
                        case "Per_30_Days":
                            _data = _data.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _data = _data.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _data = _data.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _data = _data.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _data = _data.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _data = _data.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                    }

                    if (_data.Count() > 0)
                    {
                        dtResult = _data.Select(x => x).ToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        public static DataTable getSebiSchemeDetails(List<WatchScheme> _LstSchmes, string Period)
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext())
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
                    var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
                    maxDate = maxDate.AddDays(-7);
                    var uo = _LstSchmes.Select(s => s.SchemeId).ToList();
                    var _Schmes = dc.T_SCHEMES_MASTER_Clients.Where(x => uo.Contains(Convert.ToInt32(x.Scheme_Id))).Select(c => c);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in _Schmes on tf.Scheme_Id equals si.Scheme_Id

                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
                                 join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID

                                 //join rm in 
                                 //    (from zz in dc.T_RANKING_MFR_Clients
                                 //     where zz.Quarter_Id == maxQuater && zz.Year_Check == RankYearChk
                                 //     select zz)
                                 //on si.Scheme_Id equals rm.Scheme_Id into grp
                                 //from x in grp.DefaultIfEmpty()
                                 where tf.Calculation_Date >= maxDate
                                 select new TopFundRank
                                 {
                                     SchemeId = si.Scheme_Id,
                                     Sch_Name = si.Sch_Short_Name,
                                     Calculation_Date = tf.Calculation_Date,
                                     Div_Yield = tf.Div_Yield,
                                     Per_7_Days = tf.Per_7_Days,
                                     Per_30_Days = tf.Per_30_Days,
                                     Per_91_Days = tf.Per_91_Days,
                                     Per_182_Days = tf.Per_182_Days,
                                     Per_1_Year = tf.Per_1_Year,
                                     Per_3_Year = tf.Per_3_Year,
                                     Per_5_Year = tf.Per_5_Year,
                                     Since_Inception = tf.Since_Inception,
                                     Nature_ID = fm.SEBI_NATURE_ID,
                                     Nature = sn.Sebi_Nature,
                                     Structure_ID = ss.Structure_ID,
                                     SubcategoryId = sc.Sebi_Sub_Nature_ID,
                                     SubNature = sc.Sebi_Sub_Nature,
                                     OptionId = op.Option_ID,
                                     RiskColorId = ri.ID,
                                     MinInvest = sd.Min_Investment,
                                     //MF_Rank = x.MFR,
                                     //MF_Rank_Html = getIcronRankHtml(x.MFR),
                                     Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault(),
                                     Fund_Size = dc.T_FUND_SIZEs.Where(v => v.SCHEME_ID == si.Scheme_Id).OrderByDescending(k => k.FUNDSIZE_DATE).Select(c => c.MONTHLY_FUND_SIZE).FirstOrDefault()
                                 }).Distinct();


                    //var bbb = _data.ToList();
                    _data = _data.Distinct();

                    switch (Period)
                    {
                        case "Per_7_Days":
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _data = _data.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _data = _data.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _data = _data.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _data = _data.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _data = _data.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _data = _data.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Per_7_Days);
                            break;
                    }

                    if (_data.Count() > 0)
                    {
                        dtResult = _data.Select(x => x).ToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        public static List<string> getAutoCompleteScheme(string strScheme)
        {
            using (var dc = new SIP_ClientDataContext())
            {
                var _Schmes = (from x in dc.T_SCHEMES_MASTER_Clients
                               where
                              x.Sub_Nature2_Id!=46 &&
                               (x.Sch_Short_Name.StartsWith(strScheme) || x.Scheme_Name.StartsWith(strScheme)) && x.Nav_Check == 3
                               //select new
                               //{
                               //    id = x.Sch_Short_Name + "#" + Convert.ToString(x.Scheme_Id)
                               //}).Cast<string>().ToList();

                               select new
                               {
                                   SchemeName = x.Sch_Short_Name,
                                   SchemeId = x.Scheme_Id

                               });

                if (_Schmes.Any())
                {
                    return _Schmes.ToList().Select(x => Convert.ToString(x.SchemeName) + "#" + x.SchemeId).ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<string> getTopSearch(string strScheme)
        {
            using (var dc = new SIP_ClientDataContext())
            {
                var _Schmes = (from x in dc.VW_ACTIVE_SCHEMES_DETAILS_WITH_SEBIs
                               where (x.SCHEME_SHORT_NAME.StartsWith(strScheme) || x.SCHEMENAME.StartsWith(strScheme)) && x.SUB_PLAN_ID != 46
                               //select new
                               //{
                               //    id = x.Sch_Short_Name + "#" + Convert.ToString(x.Scheme_Id)
                               //}).Cast<string>().ToList();

                               select new
                               {
                                   SchemeName = x.SCHEME_SHORT_NAME,
                                   SchemeId = x.SCHEME_ID

                               });

                if (_Schmes.Any())
                {
                    return _Schmes.ToList().Select(x => Convert.ToString(x.SchemeName) + "#" + x.SchemeId).ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        //public static DataTable getTopFundRank(int Fetachrank, int Type, int Category, string Period,
        //    int SubCategory, int Option, int Risk, int min_invest,int Rank,string YearChk)
        //{
        //    string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
        //    DataTable dtResult = null;

        //    try
        //    {
        //        using (var dc = new SIP_ClientDataContext())
        //        {
        //            var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();
        //            var maxQuater = dc.T_RANKING_QUARTER_MASTER_Clients.Select(x => x.Quarter_Id).Max();
        //            maxDate = maxDate.AddDays(-7);

        //            var _data1 = (from tf in dc.T_TOP_FUND_Clients
        //                          join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
        //                          join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
        //                          join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
        //                          join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
        //                          join sc in dc.T_SCHEMES_SUB_NATURE_Clients on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
        //                          join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
        //                          join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
        //                          join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
        //                          join rm in                                     
        //                           ( from  zz in  dc.T_RANKING_MFR_Clients 
        //                            where 
        //                            zz.Quarter_Id == maxQuater
        //                            && zz.MFR >= Rank
        //                            && zz.Year_Check == YearChk 
        //                                            select zz)                                  
        //                          on si.Scheme_Id equals rm.Scheme_Id  into grp

        //                          from x in grp.DefaultIfEmpty()
        //                                     where
        //                           tf.Calculation_Date >= maxDate
        //                           && op.Option_ID==3
        //                          select tf).ToDataTable();



        //            var _data = (from tf in dc.T_TOP_FUND_Clients
        //                         join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
        //                         join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
        //                         join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
        //                         join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
        //                         join sc in dc.T_SCHEMES_SUB_NATURE_Clients on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
        //                         join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
        //                         join ri in dc.T_SCHEME_INFO_FUND_COLOR_MASTs on fm.FUND_COLOR_MAST_ID equals ri.ID
        //                         join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
        //                         join rm in
        //                             (from zz in dc.T_RANKING_MFR_Clients
        //                              where
        //                              zz.Quarter_Id == maxQuater
        //                              && zz.MFR >= Rank
        //                              && zz.Year_Check == YearChk
        //                              select zz) on si.Scheme_Id equals rm.Scheme_Id into grp
        //                         from x in grp.DefaultIfEmpty()
        //                         where
        //                         tf.Calculation_Date >= maxDate
        //                         orderby tf.Calculation_Date descending
        //                         select new TopFundRank
        //                         {
        //                             Sch_Name = si.Sch_Short_Name,
        //                             Calculation_Date = tf.Calculation_Date,
        //                             Div_Yield = tf.Div_Yield,
        //                             Per_7_Days = tf.Per_7_Days,
        //                             Per_30_Days = tf.Per_30_Days,
        //                             Per_91_Days = tf.Per_91_Days,
        //                             Per_182_Days = tf.Per_182_Days,
        //                             Per_1_Year = tf.Per_1_Year,
        //                             Per_3_Year = tf.Per_3_Year,
        //                             Per_5_Year = tf.Per_5_Year,
        //                             Since_Inception = tf.Since_Inception,
        //                             Nature_ID = fm.NATURE_ID.Value,
        //                             Nature = sn.Nature,
        //                             Structure_ID = ss.Structure_ID,
        //                             SubcategoryId = sc.Sub_Nature_ID,
        //                             SubNature = sc.Sub_Nature,
        //                             OptionId = op.Option_ID,
        //                             RiskColorId = ri.ID,
        //                             MinInvest = sd.Min_Investment,
        //                             MF_Rank = x.MFR,
        //                             MF_Rank_Html = x.MFR == null ? "" : getIcronRankHtml(Convert.ToInt32(x.MFR)),
        //                             Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
        //                         }).Distinct();

        //            if (Category != -1)
        //            {
        //                _data = _data.Where(x => x.Nature_ID == Convert.ToDecimal(Category)).Distinct();
        //            }

        //            if (Type != -1)
        //            {
        //                _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
        //            }

        //            if (SubCategory != -1)
        //            {
        //                _data = _data.Where(x => x.SubcategoryId == Convert.ToDecimal(SubCategory)).Distinct();
        //            }

        //            if (Option != -1)
        //            {
        //                _data = _data.Where(x => x.OptionId == Convert.ToDecimal(Option)).Distinct();
        //            }

        //            if (Risk != -1)
        //            {
        //                _data = _data.Where(x => x.RiskColorId == Convert.ToDecimal(Risk)).Distinct();
        //            }

        //            _data = _data.Where(x => x.MinInvest >= min_invest).Distinct();

        //            #region Without Dynamic Linq

        //            switch (Period)
        //            {
        //                case "Per_7_Days":
        //                    _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_7_Days);
        //                    break;
        //                case "Per_30_Days":
        //                    _data = _data.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_30_Days);
        //                    break;
        //                case "Per_91_Days":
        //                    _data = _data.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_91_Days);
        //                    break;
        //                case "Per_182_Days":
        //                    _data = _data.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_182_Days);
        //                    break;
        //                case "Per_1_Year":
        //                    _data = _data.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_1_Year);
        //                    break;
        //                case "Per_3_Year":
        //                    _data = _data.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_3_Year);
        //                    break;
        //                case "Per_5_Year":
        //                    _data = _data.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_5_Year);
        //                    break;
        //                default:
        //                    _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_7_Days);
        //                    break;
        //            }

        //            #endregion

        //            if (_data.Count() > 0)
        //            {
        //                _data = _data.Select(x => x).Take(Fetachrank).Distinct();
        //                dtResult = _data.ToDataTable();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return dtResult;
        //}

        private static string getIcronRankHtml(int intRank)
        {
            string html = "";
            switch (intRank)
            {
                case 1:
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
                case 2:
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
                case 3:
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
                case 4:
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
                case 5:
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
            }
            return html;
        }

        private static string getIcronRankHtml(decimal? intRank)
        {
            string html = "";
            switch (Convert.ToString(intRank))
            {
                case "1":
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
                case "2":
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
                case "3":
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
                case "4":
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
                case "5":
                    html = "<img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' /><img src='images/icon_star_filled.gif' alt='Rating star' />";
                    break;
            }
            return html;
        }
        public static DataTable getTopFundRank_exceptDivandDir(int Fetachrank, int Type, int Category, string Period)
        {

            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext())
                {
                    var maxDate = (from dtf in dc.T_TOP_FUND_Clients orderby dtf.Calculation_Date descending select dtf.Calculation_Date).Max();

                    maxDate = maxDate.AddDays(-7);

                    var _data = (from tf in dc.T_TOP_FUND_Clients
                                 join si in dc.T_SCHEMES_MASTER_Clients on tf.Scheme_Id equals si.Scheme_Id
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 //join nd in dc.T_NAV_DIV_clients on  si.Scheme_Id equals nd.Scheme_Id
                                 where
                                 //tf.Scheme_Id == si.Scheme_Id
                                 //&& si.Fund_Id == fm.FUND_ID
                                 //&& fm.NATURE_ID == sn.Nature_ID
                                 //&& fm.STRUCTURE_ID == ss.Structure_ID                                 
                                 //&& si.Scheme_Id == nd.Scheme_Id
                                 tf.Calculation_Date >= maxDate
                                 && si.Option_Id == 2 && si.Sub_Nature2_Id != 46
                                 // && nd.Nav_Date.Value == maxDate
                                 //  && sn.Nature_ID == Category
                                 //  && ss.Structure_ID == Type
                                 orderby tf.Calculation_Date descending
                                 select new TopFundRank
                                 {
                                     Sch_Name = si.Sch_Short_Name,
                                     Calculation_Date = tf.Calculation_Date,
                                     Div_Yield = tf.Div_Yield,
                                     Per_7_Days = tf.Per_7_Days,
                                     Per_30_Days = tf.Per_30_Days,
                                     Per_91_Days = tf.Per_91_Days,
                                     Per_182_Days = tf.Per_182_Days,
                                     Per_1_Year = tf.Per_1_Year,
                                     Per_3_Year = tf.Per_3_Year,
                                     Per_5_Year = tf.Per_5_Year,
                                     Since_Inception = tf.Since_Inception,
                                     Nature_ID = fm.NATURE_ID.Value,
                                     Nature = sn.Nature,
                                     Structure_ID = ss.Structure_ID,
                                     Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                 });



                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.Nature_ID == Convert.ToDecimal(Category)).Distinct();

                    }

                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
                    }

                    //switch (Period)
                    //{
                    //    case "Per_7_Days": dynamicPeriod = "Per_7_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_7_Days desc";
                    //        break;
                    //    case "Per_30_Days": dynamicPeriod = "Per_30_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_30_Days desc";
                    //        break;
                    //    case "Per_91_Days": dynamicPeriod = "Per_91_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_91_Days desc";
                    //        break;
                    //    case "Per_182_Days": dynamicPeriod = "Per_182_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_182_Days desc";
                    //        break;
                    //    case "Per_1_Year": dynamicPeriod = "Per_1_Year!=null"; dynamicOrder = "Calculation_Date desc,Per_1_Year desc";
                    //        break;
                    //    case "Per_3_Year": dynamicPeriod = "Per_3_Year!=null"; dynamicOrder = "Calculation_Date desc,Per_3_Year desc";
                    //        break;
                    //    case "Per_5_Year": dynamicPeriod = "Per_5_Year!=null"; dynamicOrder = "Calculation_Date desc,Per_5_Year desc";
                    //        break;
                    //    default: dynamicPeriod = "Per_7_Days!=null"; dynamicOrder = "Calculation_Date desc,Per_7_Days desc";
                    //        break;
                    //}
                    //_data = _data.OrderBy(dynamicOrder).Where(dynamicPeriod);

                    #region Without Dynamic Linq


                    switch (Period)
                    {
                        case "Per_7_Days":
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_7_Days);
                            break;
                        case "Per_30_Days":
                            _data = _data.Where(x => x.Per_30_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_30_Days);
                            break;
                        case "Per_91_Days":
                            _data = _data.Where(x => x.Per_91_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_91_Days);
                            break;
                        case "Per_182_Days":
                            _data = _data.Where(x => x.Per_182_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_182_Days);
                            break;
                        case "Per_1_Year":
                            _data = _data.Where(x => x.Per_1_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_1_Year);
                            break;
                        case "Per_3_Year":
                            _data = _data.Where(x => x.Per_3_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_3_Year);
                            break;
                        case "Per_5_Year":
                            _data = _data.Where(x => x.Per_5_Year != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_5_Year);
                            break;
                        default:
                            _data = _data.Where(x => x.Per_7_Days != null).OrderByDescending(x => x.Calculation_Date).ThenByDescending(x => x.Per_7_Days);
                            break;
                    }

                    #endregion

                    if (_data.Count() > 0)
                    {
                        _data = _data.Select(x => x).Take(Fetachrank);
                        dtResult = _data.ToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        public static DataTable getFundHouse(string MutCode = "")
        {
            List<decimal> arr = new List<decimal>();
            DataTable dt = null;
            if (MutCode != string.Empty)
            {
                arr = getFundSchemeId(MutCode);
            }

            try
            {

                using (var db = new SIP_ClientDataContext())
                {

                    var data = (from mf in db.T_MF_MASTER_Clients
                                join fm in db.T_FUND_MASTER_clients
                                on mf.MutualFund_ID equals fm.MUTUALFUND_ID
                                join sm in db.T_SCHEMES_MASTER_Clients
                                on fm.FUND_ID equals sm.Fund_Id
                                orderby mf.MutualFund_Name
                                where sm.Nav_Check == 3// != 2
                                select new
                                {
                                    mf.MutualFund_ID,
                                    mf.MutualFund_Name
                                }).Distinct();


                    if (data.Count() > 0)
                    {
                        if (arr.Count > 0)
                        {
                            data = data.Where(x => arr.Contains(x.MutualFund_ID));
                        }
                    }
                    dt = data.OrderBy(x => x.MutualFund_Name).ToDataTable();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return dt;

        }

        public static DataTable getScheme(string MutFundId = "")
        {

            List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            if (MutFundId != string.Empty)
                listFund = getFundSchemeId(MutFundId);

            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    var _data = (from fm in db.T_FUND_MASTER_clients
                                 join sm in db.T_SCHEMES_MASTER_Clients
                                 on fm.FUND_ID equals sm.Fund_Id
                                 where sm.Nav_Check == 3// != 2
                                 select new
                                 {
                                     fm.MUTUALFUND_ID,
                                     sm.Sch_Short_Name,
                                     sm.Scheme_Id,
                                     sm.Launch_Date
                                 }).Distinct();

                    if (_data.Count() > 0)
                    {
                        if (listFund.Count > 0)
                        {
                            _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                        }
                    }

                    dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return dt;

        }

        public static DataTable getScheme(bool IsDividend, bool Isdirect, string MutFundId = "")
        {
            List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            if (MutFundId != string.Empty)
                listFund = getFundSchemeId(MutFundId);

            try
            {
                if (IsDividend == false && Isdirect == false)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where sm.Nav_Check == 3 && sm.Option_Id == 2 && sm.Sub_Nature2_Id != 46 && sm.Launch_Date != null
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
                else if (IsDividend == true && Isdirect == false)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where sm.Nav_Check == 3 && sm.Sub_Nature2_Id != 46
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
                else if (IsDividend == false && Isdirect == true)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where sm.Nav_Check == 3 && sm.Option_Id == 2
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
                else if (IsDividend == true && Isdirect == true)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where sm.Nav_Check == 3
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return dt;

        }

        public static List<SchemeInfo> getAllScheme(bool isDirectScheme = false)
        {

            try
            {
                var _data = new List<SchemeInfo>();
                using (var db = new SIP_ClientDataContext())
                {
                    if (!isDirectScheme)
                    {
                        _data = (from fm in db.VW_ACTIVE_SCHEMES_DETAILS_WITH_SEBIs
                                 join sm in db.T_INDEX_MASTER_Clients
                                 on fm.BENCHMARK_CODE equals sm.INDEX_CODE
                                 where fm.Nav_Check == 3
                                 && fm.LAUNCH_DATE.HasValue
                                 && fm.SUB_PLAN_ID != 46
                                 select new SchemeInfo
                                 {
                                     Sch_Short_Name = fm.SCHEME_SHORT_NAME,
                                     SCHEME_ID = fm.SCHEME_ID,
                                     Launch_Date = fm.LAUNCH_DATE,
                                     Structure_ID = fm.Structure_ID,
                                     Sebi_Nature_ID = fm.SEBI_NATURE_ID,
                                     SEBI_NATURE = fm.SEBI_NATURE,
                                     Sebi_Sub_Nature_ID = fm.SEBI_SUB_NATURE_ID,
                                     SEBI_SUB_NATURE = fm.SEBI_SUB_NATURE,
                                     MutualFund_ID = fm.MutualFund_ID,
                                     Options = fm.Option_ID,
                                     BENCHMARK = fm.BENCHMARK,
                                     INDEX_ID = sm.INDEX_ID,
                                 }).Distinct().ToList();
                    }
                    else
                    {
                        _data = (from fm in db.VW_ACTIVE_SCHEMES_DETAILS_WITH_SEBIs
                                 join sm in db.T_INDEX_MASTER_Clients
                                 on fm.BENCHMARK_CODE equals sm.INDEX_CODE
                                 where fm.Nav_Check == 3
                                 && fm.LAUNCH_DATE.HasValue
                                 select new SchemeInfo
                                 {
                                     Sch_Short_Name = fm.SCHEME_SHORT_NAME,
                                     SCHEME_ID = fm.SCHEME_ID,
                                     Launch_Date = fm.LAUNCH_DATE,
                                     Structure_ID = fm.Structure_ID,
                                     Sebi_Nature_ID = fm.SEBI_NATURE_ID,
                                     SEBI_NATURE = fm.SEBI_NATURE,
                                     Sebi_Sub_Nature_ID = fm.SEBI_SUB_NATURE_ID,
                                     SEBI_SUB_NATURE = fm.SEBI_SUB_NATURE,
                                     MutualFund_ID = fm.MutualFund_ID,
                                     Options = fm.Option_ID,
                                     BENCHMARK = fm.BENCHMARK,
                                     INDEX_ID = sm.INDEX_ID,
                                 }).Distinct().ToList();
                    }

                    _data = _data.OrderBy(x => x.Sch_Short_Name).ToList();

                    if (_data != null && _data.Count > 0)
                    {
                        return _data;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return null;

        }

        //--Add by saurodip
        public static DataTable getScheme(int Category, int Type, int SubCategory, int Option, int MutFundId)
        {
            //List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            //if (MutFundId != string.Empty)
            //    listFund = getFundSchemeId(MutFundId);

            try
            {

                //--new    
                using (var dc = new SIP_ClientDataContext())
                {
                    var _data = (from si in dc.T_SCHEMES_MASTER_Clients
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join MFund in dc.T_MF_MASTER_Clients on fm.MUTUALFUND_ID equals MFund.MutualFund_ID
                                 join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SCHEMES_SUB_NATURE_Clients on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 where si.Nav_Check == 3
                                 && si.Sub_Nature2_Id != 46
                                 select new
                                 {
                                     MUTUALFUND_ID = MFund.MutualFund_ID,
                                     Sch_Name = si.Sch_Short_Name,
                                     Scheme_Id = si.Scheme_Id,
                                     Sch_Short_Name = si.Sch_Short_Name,
                                     NATURE_ID = fm.NATURE_ID.Value,
                                     Nature = sn.Nature,
                                     Structure_ID = ss.Structure_ID,
                                     SubcategoryId = sc.Sub_Nature_ID,
                                     SubNature = sc.Sub_Nature,
                                     OptionId = op.Option_ID,
                                     Launch_Date = si.Launch_Date,
                                     //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                 }).Distinct();

                    if (MutFundId != -1)
                    {
                        _data = _data.Where(x => x.MUTUALFUND_ID == Convert.ToDecimal(MutFundId)).Distinct();
                    }
                    if (SubCategory == 9000 || SubCategory == 9001 || SubCategory == 6)
                    {
                        var maxcaldate = (from dtf in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients orderby dtf.CALC_DATE descending select dtf.CALC_DATE).Max();
                        if (DateTime.Today.Day < 17 && DateTime.Today.Day > 8)
                            maxcaldate = (from dtf in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients where dtf.CALC_DATE < maxcaldate orderby dtf.CALC_DATE descending select dtf.CALC_DATE).Max();
                        _data = _data.Where(x => (x.NATURE_ID == Convert.ToDecimal(3) && x.SubNature == "Diversified")).Distinct();
                        //_data = _data.Where(x => x.Nature_ID == Convert.ToDecimal(3) & x.SubNature == "Diversified").Distinct();
                        if (SubCategory == 9000)
                        {
                            _data = (from dta in _data
                                     join si in dc.T_SCHEMES_MASTER_Clients on dta.Scheme_Id equals si.Scheme_Id
                                     join mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients on si.Fund_Id equals mcap.FUND_ID
                                     where mcap.CALC_DATE >= maxcaldate && mcap.MARKET_SLAB == "Large Cap" && mcap.MCAPALLOCATION > 75
                                     select new
                                     {
                                         MUTUALFUND_ID = dta.MUTUALFUND_ID,
                                         Sch_Name = dta.Sch_Name,
                                         Scheme_Id = dta.Scheme_Id,
                                         Sch_Short_Name = dta.Sch_Short_Name,
                                         NATURE_ID = dta.NATURE_ID,
                                         Nature = dta.Nature,
                                         Structure_ID = dta.Structure_ID,
                                         SubcategoryId = dta.SubcategoryId,
                                         SubNature = "Large Cap",
                                         OptionId = dta.OptionId,
                                         Launch_Date = dta.Launch_Date,
                                         //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                     }).Distinct();
                        }
                        else if (SubCategory == 9001)
                        {
                            var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                                             group mcap by mcap.FUND_ID into newGroupwhere
                                             select new
                                             {
                                                 FUND_ID = newGroupwhere.Key,
                                                 Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                                             }
                                           );

                            _data = (from dta in _data
                                     join si in dc.T_SCHEMES_MASTER_Clients on dta.Scheme_Id equals si.Scheme_Id
                                     join mcap in _mcapdata on si.Fund_Id equals mcap.FUND_ID
                                     where mcap.Cap > 75
                                     select new
                                     {
                                         MUTUALFUND_ID = dta.MUTUALFUND_ID,
                                         Sch_Name = dta.Sch_Name,
                                         Scheme_Id = dta.Scheme_Id,
                                         Sch_Short_Name = dta.Sch_Short_Name,
                                         NATURE_ID = dta.NATURE_ID,
                                         Nature = dta.Nature,
                                         Structure_ID = dta.Structure_ID,
                                         SubcategoryId = dta.SubcategoryId,
                                         SubNature = "Mid & Small Cap",
                                         OptionId = dta.OptionId,
                                         Launch_Date = dta.Launch_Date,
                                         //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                     }).Distinct();
                        }
                        else
                        {
                            var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => p.MARKET_SLAB == "Large Cap" && p.CALC_DATE >= maxcaldate && p.MCAPALLOCATION > 75)
                                             select new
                                             {
                                                 FUND_ID = mcap.FUND_ID
                                             }
                                            ).Distinct();

                            var _mcapdata_temp = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                                                  group mcap by mcap.FUND_ID into newGroupwhere
                                                  select new
                                                  {
                                                      FUND_ID = newGroupwhere.Key,
                                                      Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                                                  }
                                            );

                            var _mcapdata_temp2 = (from mcap in _mcapdata_temp.Where(p => p.Cap > 75)
                                                   select new
                                                   {
                                                       FUND_ID = mcap.FUND_ID
                                                   }
                                            ).Distinct();

                            _mcapdata = (from u in _mcapdata select new { u.FUND_ID })
                                            .Union(from u in _mcapdata_temp2 select new { u.FUND_ID }).Distinct();

                            _data = (from dta in _data
                                     join si in dc.T_SCHEMES_MASTER_Clients on dta.Scheme_Id equals si.Scheme_Id
                                     join mcap in _mcapdata on si.Fund_Id equals mcap.FUND_ID into p
                                     from mcap in p.DefaultIfEmpty()
                                     where mcap.FUND_ID == null
                                     select new
                                     {
                                         MUTUALFUND_ID = dta.MUTUALFUND_ID,
                                         Sch_Name = dta.Sch_Name,
                                         Scheme_Id = dta.Scheme_Id,
                                         Sch_Short_Name = dta.Sch_Short_Name,
                                         NATURE_ID = dta.NATURE_ID,
                                         Nature = dta.Nature,
                                         Structure_ID = dta.Structure_ID,
                                         SubcategoryId = dta.SubcategoryId,
                                         SubNature = "Diversified",
                                         OptionId = dta.OptionId,
                                         Launch_Date = dta.Launch_Date,
                                         //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                     }).Distinct();
                        }
                    }
                    else
                    {
                        if (Category != -1)
                        {
                            _data = _data.Where(x => x.NATURE_ID == Convert.ToDecimal(Category)).Distinct();
                        }
                        if (SubCategory != -1)
                        {
                            _data = _data.Where(x => x.SubcategoryId == Convert.ToDecimal(SubCategory)).Distinct();
                        }
                    }
                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.OptionId == Convert.ToDecimal(Option)).Distinct();
                    }
                    _data = _data.Distinct();
                    if (_data.Any())
                    {
                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }


                #region old
                //---end new    
                //if (IsDividend == false && Isdirect == false)
                //{
                //    using (var db = new SIP_ClientDataContext())
                //    {
                //        var _data = (from fm in db.T_FUND_MASTER_clients
                //                     join sm in db.T_SCHEMES_MASTER_Clients
                //                     on fm.FUND_ID equals sm.Fund_Id
                //                     where sm.Nav_Check == 3 && sm.Option_Id == 2 && sm.Sub_Nature2_Id != 46 && sm.Launch_Date != null
                //                     select new
                //                     {
                //                         fm.MUTUALFUND_ID,
                //                         sm.Sch_Short_Name,
                //                         sm.Scheme_Id,
                //                         sm.Launch_Date
                //                     }).Distinct();

                //        if (_data.Count() > 0)
                //        {
                //            if (listFund.Count > 0)
                //            {
                //                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                //            }
                //        }

                //        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                //    }
                //}
                //else if (IsDividend == true && Isdirect == false)
                //{
                //    using (var db = new SIP_ClientDataContext())
                //    {
                //        var _data = (from fm in db.T_FUND_MASTER_clients
                //                     join sm in db.T_SCHEMES_MASTER_Clients
                //                     on fm.FUND_ID equals sm.Fund_Id
                //                     where sm.Nav_Check == 3 && sm.Sub_Nature2_Id != 46
                //                     select new
                //                     {
                //                         fm.MUTUALFUND_ID,
                //                         sm.Sch_Short_Name,
                //                         sm.Scheme_Id,
                //                         sm.Launch_Date
                //                     }).Distinct();

                //        if (_data.Count() > 0)
                //        {
                //            if (listFund.Count > 0)
                //            {
                //                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                //            }
                //        }

                //        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                //    }
                //}
                //else if (IsDividend == false && Isdirect == true)
                //{
                //    using (var db = new SIP_ClientDataContext())
                //    {
                //        var _data = (from fm in db.T_FUND_MASTER_clients
                //                     join sm in db.T_SCHEMES_MASTER_Clients
                //                     on fm.FUND_ID equals sm.Fund_Id
                //                     where sm.Nav_Check == 3 && sm.Option_Id == 2
                //                     select new
                //                     {
                //                         fm.MUTUALFUND_ID,
                //                         sm.Sch_Short_Name,
                //                         sm.Scheme_Id,
                //                         sm.Launch_Date
                //                     }).Distinct();

                //        if (_data.Count() > 0)
                //        {
                //            if (listFund.Count > 0)
                //            {
                //                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                //            }
                //        }

                //        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                //    }
                //}
                //else if (IsDividend == true && Isdirect == true)
                //{
                //    using (var db = new SIP_ClientDataContext())
                //    {
                //        var _data = (from fm in db.T_FUND_MASTER_clients
                //                     join sm in db.T_SCHEMES_MASTER_Clients
                //                     on fm.FUND_ID equals sm.Fund_Id
                //                     where sm.Nav_Check == 3
                //                     select new
                //                     {
                //                         fm.MUTUALFUND_ID,
                //                         sm.Sch_Short_Name,
                //                         sm.Scheme_Id,
                //                         sm.Launch_Date
                //                     }).Distinct();

                //        if (_data.Count() > 0)
                //        {
                //            if (listFund.Count > 0)
                //            {
                //                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                //            }
                //        }

                //        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                //    }
                //}
                #endregion
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return dt;

        }

        public static DataTable getScheme(int Category, int Type, int SubCategory, int Option, int MutFundId, String RegularOrDirrect)
        {
            //List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            //if (MutFundId != string.Empty)
            //    listFund = getFundSchemeId(MutFundId);

            try
            {

                //--new    
                using (var dc = new SIP_ClientDataContext())
                {
                    var _data = (from si in dc.T_SCHEMES_MASTER_Clients
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join MFund in dc.T_MF_MASTER_Clients on fm.MUTUALFUND_ID equals MFund.MutualFund_ID
                                 join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SCHEMES_SUB_NATURE_Clients on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 where si.Nav_Check == 3
                                 //&& si.Sub_Nature2_Id != 46
                                 select new
                                 {
                                     MUTUALFUND_ID = MFund.MutualFund_ID,
                                     Sch_Name = si.Sch_Short_Name,
                                     Scheme_Id = si.Scheme_Id,
                                     Sch_Short_Name = si.Sch_Short_Name,
                                     NATURE_ID = fm.NATURE_ID.Value,
                                     Nature = sn.Nature,
                                     Structure_ID = ss.Structure_ID,
                                     SubcategoryId = sc.Sub_Nature_ID,
                                     SubNature = sc.Sub_Nature,
                                     OptionId = op.Option_ID,
                                     Launch_Date = si.Launch_Date,
                                     Sub_Nature2_Id= si.Sub_Nature2_Id
                                     //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                 }).Distinct();
                    if(RegularOrDirrect=="REGULAR")
                        _data = _data.Where(x => x.Sub_Nature2_Id != 46).Distinct();
                    if (RegularOrDirrect == "DIRECT")
                        _data = _data.Where(x => x.Sub_Nature2_Id == 46).Distinct();

                    if (MutFundId != -1)
                    {
                        _data = _data.Where(x => x.MUTUALFUND_ID == Convert.ToDecimal(MutFundId)).Distinct();
                    }
                    if (SubCategory == 9000 || SubCategory == 9001 || SubCategory == 6)
                    {
                        var maxcaldate = (from dtf in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients orderby dtf.CALC_DATE descending select dtf.CALC_DATE).Max();
                        if (DateTime.Today.Day < 17 && DateTime.Today.Day > 8)
                            maxcaldate = (from dtf in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients where dtf.CALC_DATE < maxcaldate orderby dtf.CALC_DATE descending select dtf.CALC_DATE).Max();
                        _data = _data.Where(x => (x.NATURE_ID == Convert.ToDecimal(3) && x.SubNature == "Diversified")).Distinct();
                        //_data = _data.Where(x => x.Nature_ID == Convert.ToDecimal(3) & x.SubNature == "Diversified").Distinct();
                        if (SubCategory == 9000)
                        {
                            _data = (from dta in _data
                                     join si in dc.T_SCHEMES_MASTER_Clients on dta.Scheme_Id equals si.Scheme_Id
                                     join mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients on si.Fund_Id equals mcap.FUND_ID
                                     where mcap.CALC_DATE >= maxcaldate && mcap.MARKET_SLAB == "Large Cap" && mcap.MCAPALLOCATION > 75
                                     select new
                                     {
                                         MUTUALFUND_ID = dta.MUTUALFUND_ID,
                                         Sch_Name = dta.Sch_Name,
                                         Scheme_Id = dta.Scheme_Id,
                                         Sch_Short_Name = dta.Sch_Short_Name,
                                         NATURE_ID = dta.NATURE_ID,
                                         Nature = dta.Nature,
                                         Structure_ID = dta.Structure_ID,
                                         SubcategoryId = dta.SubcategoryId,
                                         SubNature = "Large Cap",
                                         OptionId = dta.OptionId,
                                         Launch_Date = dta.Launch_Date,
                                         Sub_Nature2_Id = dta.Sub_Nature2_Id
                                         //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                     }).Distinct();
                        }
                        else if (SubCategory == 9001)
                        {
                            var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                                             group mcap by mcap.FUND_ID into newGroupwhere
                                             select new
                                             {
                                                 FUND_ID = newGroupwhere.Key,
                                                 Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                                             }
                                           );

                            _data = (from dta in _data
                                     join si in dc.T_SCHEMES_MASTER_Clients on dta.Scheme_Id equals si.Scheme_Id
                                     join mcap in _mcapdata on si.Fund_Id equals mcap.FUND_ID
                                     where mcap.Cap > 75
                                     select new
                                     {
                                         MUTUALFUND_ID = dta.MUTUALFUND_ID,
                                         Sch_Name = dta.Sch_Name,
                                         Scheme_Id = dta.Scheme_Id,
                                         Sch_Short_Name = dta.Sch_Short_Name,
                                         NATURE_ID = dta.NATURE_ID,
                                         Nature = dta.Nature,
                                         Structure_ID = dta.Structure_ID,
                                         SubcategoryId = dta.SubcategoryId,
                                         SubNature = "Mid & Small Cap",
                                         OptionId = dta.OptionId,
                                         Launch_Date = dta.Launch_Date,
                                         Sub_Nature2_Id = dta.Sub_Nature2_Id
                                         //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                     }).Distinct();
                        }
                        else
                        {
                            var _mcapdata = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => p.MARKET_SLAB == "Large Cap" && p.CALC_DATE >= maxcaldate && p.MCAPALLOCATION > 75)
                                             select new
                                             {
                                                 FUND_ID = mcap.FUND_ID
                                             }
                                            ).Distinct();

                            var _mcapdata_temp = (from mcap in dc.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(p => (p.MARKET_SLAB == "Mid Cap" || p.MARKET_SLAB == "Small Cap") && p.CALC_DATE >= maxcaldate)
                                                  group mcap by mcap.FUND_ID into newGroupwhere
                                                  select new
                                                  {
                                                      FUND_ID = newGroupwhere.Key,
                                                      Cap = newGroupwhere.Sum(q => q.MCAPALLOCATION)
                                                  }
                                            );

                            var _mcapdata_temp2 = (from mcap in _mcapdata_temp.Where(p => p.Cap > 75)
                                                   select new
                                                   {
                                                       FUND_ID = mcap.FUND_ID
                                                   }
                                            ).Distinct();

                            _mcapdata = (from u in _mcapdata select new { u.FUND_ID })
                                            .Union(from u in _mcapdata_temp2 select new { u.FUND_ID }).Distinct();

                            _data = (from dta in _data
                                     join si in dc.T_SCHEMES_MASTER_Clients on dta.Scheme_Id equals si.Scheme_Id
                                     join mcap in _mcapdata on si.Fund_Id equals mcap.FUND_ID into p
                                     from mcap in p.DefaultIfEmpty()
                                     where mcap.FUND_ID == null
                                     select new
                                     {
                                         MUTUALFUND_ID = dta.MUTUALFUND_ID,
                                         Sch_Name = dta.Sch_Name,
                                         Scheme_Id = dta.Scheme_Id,
                                         Sch_Short_Name = dta.Sch_Short_Name,
                                         NATURE_ID = dta.NATURE_ID,
                                         Nature = dta.Nature,
                                         Structure_ID = dta.Structure_ID,
                                         SubcategoryId = dta.SubcategoryId,
                                         SubNature = "Diversified",
                                         OptionId = dta.OptionId,
                                         Launch_Date = dta.Launch_Date,
                                         Sub_Nature2_Id = dta.Sub_Nature2_Id
                                         //Nav = dc.T_NAV_DIV_clients.Where(v => v.Scheme_Id == si.Scheme_Id).OrderByDescending(k => k.Nav_Date).Select(c => c.Nav).FirstOrDefault()
                                     }).Distinct();
                        }
                    }
                    else
                    {
                        if (Category != -1)
                        {
                            _data = _data.Where(x => x.NATURE_ID == Convert.ToDecimal(Category)).Distinct();
                        }
                        if (SubCategory != -1)
                        {
                            _data = _data.Where(x => x.SubcategoryId == Convert.ToDecimal(SubCategory)).Distinct();
                        }
                    }
                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.OptionId == Convert.ToDecimal(Option)).Distinct();
                    }
                    _data = _data.Distinct();
                    if (_data.Any())
                    {
                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return dt;

        }
        public static DataTable getSebiScheme(int Category, int Type, int SubCategory, int Option,
            int MutFundId, bool IsDirictSchemeRerqired, int SubPlanID = 0)
        {
            //List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            //if (MutFundId != string.Empty)
            //    listFund = getFundSchemeId(MutFundId);


            try
            {

                //--new    
                using (var dc = new SIP_ClientDataContext())
                {
                    var scheams = (from nd in dc.T_NAV_DIV_clients
                                   group nd by new { nd.Scheme_Id } into g
                                   select g.Key);

                    var _data = (from si in dc.T_SCHEMES_MASTER_Clients
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join MFund in dc.T_MF_MASTER_Clients on fm.MUTUALFUND_ID equals MFund.MutualFund_ID
                                 join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 join ass in dc.VW_ACTIVE_SCHEMES_DETAILS_WITH_SEBIs on MFund.MutualFund_ID equals ass.MutualFund_ID
                                 join schs in scheams on si.Scheme_Id equals schs.Scheme_Id
                                 where si.Nav_Check == 3
                                 //&& si.Sub_Nature2_Id != 46
                                 select new
                                 {
                                     MUTUALFUND_ID = MFund.MutualFund_ID,
                                     Sch_Name = si.Sch_Short_Name,
                                     Scheme_Id = si.Scheme_Id,
                                     Sch_Short_Name = si.Sch_Short_Name,
                                     NATURE_ID = sn.Sebi_Nature_ID,
                                     Nature = sn.Sebi_Nature,
                                     Structure_ID = ss.Structure_ID,
                                     SubcategoryId = sc.Sebi_Sub_Nature_ID,
                                     SubNature = sc.Sebi_Sub_Nature,
                                     OptionId = op.Option_ID,
                                     Launch_Date = si.Launch_Date,
                                     Sub_Nature_Id = si.Sub_Nature2_Id,
                                     SubPlan_ID = ass.SUB_PLAN_ID
                                 }).Distinct();

                    if (MutFundId != -1)
                    {
                        _data = _data.Where(x => x.MUTUALFUND_ID == Convert.ToDecimal(MutFundId)).Distinct();
                    }

                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.NATURE_ID == Convert.ToDecimal(Category)).Distinct();
                    }
                    if (SubCategory != -1)
                    {
                        _data = _data.Where(x => x.SubcategoryId == Convert.ToDecimal(SubCategory)).Distinct();
                    }
                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
                    }

                    if (SubPlanID != -1 && SubPlanID != 0)
                    {
                        _data = _data.Where(x => x.SubPlan_ID == Convert.ToDecimal(SubPlanID)).Distinct();
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.OptionId == Convert.ToDecimal(Option)).Distinct();
                    }
                    if (!IsDirictSchemeRerqired)
                    {
                        _data = _data.Where(x => x.Sub_Nature_Id != 46).Distinct();
                    }
                    _data = _data.Distinct();
                    if (_data.Any())
                    {
                        dt = _data.OrderBy(x => x.Sch_Short_Name)
                            
                            .ToDataTable();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return dt;

        }


        //---End add by saurodip--//

        public static SchemeInfo getSchemeDeatils(int SchemeId)
        {
            try
            {
                using (var dc = new SIP_ClientDataContext())
                {

                    var _data = (from si in dc.VW_ACTIVE_SCHEMES_DETAILS_WITH_SEBIs

                                 where si.SCHEME_ID == SchemeId
                                 select new SchemeInfo
                                 {
                                     MutualFund_ID = si.MutualFund_ID,
                                     Sch_Short_Name = si.SCHEME_SHORT_NAME,
                                     SCHEME_ID = si.SCHEME_ID,
                                     Sebi_Nature_ID = si.SEBI_NATURE_ID,
                                     SEBI_NATURE = si.SEBI_NATURE,
                                     Structure_ID = si.Structure_ID,
                                     Sebi_Sub_Nature_ID = si.SEBI_SUB_NATURE_ID,
                                     SEBI_SUB_NATURE = si.SEBI_SUB_NATURE,
                                     Options = si.Option_ID,
                                     Launch_Date = si.LAUNCH_DATE
                                 });
                    if (_data.Any())
                        return _data.FirstOrDefault();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return null;
        }
        public static DataTable getSebiSchemeBluechip(int Category, int Type, int SubCategory, int Option,
            List<decimal> MutFundId, bool IsDirictSchemeRerqired)
        {
            //List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            //if (MutFundId != string.Empty)
            //    listFund = getFundSchemeId(MutFundId);

            try
            {

                //--new    
                using (var dc = new SIP_ClientDataContext())
                {
                    //var scheams = (from nd in dc.T_NAV_DIV_clients
                    //               group nd by new { nd.Scheme_Id } into g
                    //               select g.Key);

                    var _data = (from si in dc.T_SCHEMES_MASTER_Clients
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join MFund in dc.T_MF_MASTER_Clients on fm.MUTUALFUND_ID equals MFund.MutualFund_ID
                                 join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 where si.Nav_Check == 3
                                 //&& si.Sub_Nature2_Id != 46
                                 select new
                                 {
                                     MUTUALFUND_ID = MFund.MutualFund_ID,
                                     Sch_Name = si.Sch_Short_Name,
                                     Scheme_Id = si.Scheme_Id,
                                     Sch_Short_Name = si.Sch_Short_Name,
                                     NATURE_ID = sn.Sebi_Nature_ID,
                                     Nature = sn.Sebi_Nature,
                                     Structure_ID = ss.Structure_ID,
                                     SubcategoryId = sc.Sebi_Sub_Nature_ID,
                                     SubNature = sc.Sebi_Sub_Nature,
                                     OptionId = op.Option_ID,
                                     Launch_Date = si.Launch_Date,
                                     Sub_Nature_Id = si.Sub_Nature2_Id
                                 }).Distinct();

                    if (MutFundId != null && MutFundId.Count > 0)
                    {
                        _data = _data.Where(x => MutFundId.Contains(x.MUTUALFUND_ID));
                    }

                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.NATURE_ID == Convert.ToDecimal(Category)).Distinct();
                    }
                    if (SubCategory != -1)
                    {
                        _data = _data.Where(x => x.SubcategoryId == Convert.ToDecimal(SubCategory)).Distinct();
                    }
                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.OptionId == Convert.ToDecimal(Option)).Distinct();
                    }
                    if (!IsDirictSchemeRerqired)
                    {
                        _data = _data.Where(x => x.Sub_Nature_Id != 46).Distinct();
                    }
                    _data = _data.Distinct();
                    if (_data.Any())
                    {
                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return dt;

        }


        public static DataTable getSebiSchemeBluechip(int Category, int Type, int SubCategory, int Option,
       int MutFundId, bool IsDirictSchemeRerqired)
        {
            //List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            //if (MutFundId != string.Empty)
            //    listFund = getFundSchemeId(MutFundId);

            try
            {

                //--new    
                using (var dc = new SIP_ClientDataContext())
                {


                    var _data = (from si in dc.T_SCHEMES_MASTER_Clients
                                 join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                 join MFund in dc.T_MF_MASTER_Clients on fm.MUTUALFUND_ID equals MFund.MutualFund_ID
                                 join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                 where si.Nav_Check == 3
                                 //&& si.Sub_Nature2_Id != 46
                                 select new
                                 {
                                     MUTUALFUND_ID = MFund.MutualFund_ID,
                                     Fund_Name = fm.FUND_NAME,
                                     Fund_ID = fm.FUND_ID,
                                     Sch_Name = si.Sch_Short_Name,
                                     Scheme_Id = si.Scheme_Id,
                                     Sch_Short_Name = si.Sch_Short_Name,
                                     NATURE_ID = sn.Sebi_Nature_ID,
                                     Nature = sn.Sebi_Nature,
                                     Structure_ID = ss.Structure_ID,
                                     SubcategoryId = sc.Sebi_Sub_Nature_ID,
                                     SubNature = sc.Sebi_Sub_Nature,
                                     OptionId = op.Option_ID,
                                     Launch_Date = si.Launch_Date,
                                     Sub_Nature_Id = si.Sub_Nature2_Id
                                 }).Distinct();

                    if (MutFundId != -1)
                    {
                        _data = _data.Where(x => x.MUTUALFUND_ID == Convert.ToDecimal(MutFundId)).Distinct();
                    }

                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.NATURE_ID == Convert.ToDecimal(Category)).Distinct();
                    }
                    if (SubCategory != -1)
                    {
                        _data = _data.Where(x => x.SubcategoryId == Convert.ToDecimal(SubCategory)).Distinct();
                    }
                    if (Type != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == Convert.ToDecimal(Type)).Distinct();
                    }

                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.OptionId == Convert.ToDecimal(Option)).Distinct();
                    }
                    if (!IsDirictSchemeRerqired)
                    {
                        _data = _data.Where(x => x.Sub_Nature_Id != 46).Distinct();
                    }

                    if (_data.Any())
                    {
                        _data = _data.Distinct();
                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return dt;

        }


        public static DataTable getSchemeOption(string optionId, string MutFundId = "")
        {

            List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            if (MutFundId != string.Empty)
                listFund = getFundSchemeId(MutFundId);

            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    var _data = (from fm in db.T_FUND_MASTER_clients
                                 join sm in db.T_SCHEMES_MASTER_Clients
                                 on fm.FUND_ID equals sm.Fund_Id
                                 join so in db.T_SCHEMES_OPTION_Clients
                                on sm.Option_Id equals so.Option_ID
                                 where sm.Nav_Check == 3// != 2 
                                 && sm.Option_Id == Convert.ToDecimal(optionId)
                                 select new
                                 {
                                     fm.MUTUALFUND_ID,
                                     sm.Sch_Short_Name,
                                     sm.Scheme_Id,
                                     sm.Launch_Date
                                 }).Distinct();

                    if (_data.Count() > 0)
                    {
                        if (listFund.Count > 0)
                        {
                            _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                        }
                    }

                    dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return dt;

        }
        public static DataTable getSchemeOption(bool Isdirect, string optionId, string MutFundId = "")
        {

            List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            if (MutFundId != string.Empty)
                listFund = getFundSchemeId(MutFundId);

            try
            {
                if (Isdirect)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     join so in db.T_SCHEMES_OPTION_Clients
                                    on sm.Option_Id equals so.Option_ID
                                     where sm.Nav_Check == 3// != 2 
                                     && sm.Option_Id == Convert.ToDecimal(optionId)
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
                else
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     join so in db.T_SCHEMES_OPTION_Clients
                                    on sm.Option_Id equals so.Option_ID
                                     where sm.Nav_Check == 3// != 2 
                                     && sm.Option_Id == Convert.ToDecimal(optionId) && sm.Sub_Nature2_Id != 46
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.MUTUALFUND_ID));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return dt;

        }


        public static DataTable getSchemeCategory(string CategoryId = "")
        {

            List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            if (CategoryId != string.Empty)
                listFund = getFundSchemeId(CategoryId);

            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    var _data = (from fm in db.T_FUND_MASTER_clients
                                 join sm in db.T_SCHEMES_MASTER_Clients
                                 on fm.FUND_ID equals sm.Fund_Id
                                 where sm.Nav_Check == 3// != 2
                                 select new
                                 {
                                     fm.MUTUALFUND_ID,
                                     sm.Sch_Short_Name,
                                     sm.Scheme_Id,
                                     fm.NATURE_ID,
                                     sm.Launch_Date
                                 }).Distinct();

                    if (_data.Count() > 0)
                    {
                        if (listFund.Count > 0)
                        {
                            _data = _data.Where(x => listFund.Contains(x.NATURE_ID.Value));
                        }
                    }

                    dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return dt;

        }

        public static DataTable getSchemeCategory(bool IsDividend, bool Isdirect, string CategoryId = "")
        {

            List<decimal> listFund = new List<decimal>();
            DataTable dt = null;
            if (CategoryId != string.Empty)
                listFund = getFundSchemeId(CategoryId);

            try
            {
                if (IsDividend == false && Isdirect == false)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where sm.Nav_Check == 3 && sm.Option_Id == 2 && sm.Sub_Nature2_Id != 46
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         fm.NATURE_ID,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.NATURE_ID.Value));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
                else if (IsDividend == true && Isdirect == false)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where sm.Nav_Check == 3 && sm.Sub_Nature2_Id != 46
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         fm.NATURE_ID,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.NATURE_ID.Value));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
                else if (IsDividend == false && Isdirect == true)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where sm.Nav_Check == 3 && sm.Option_Id == 2
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         fm.NATURE_ID,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.NATURE_ID.Value));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
                else if (IsDividend == true && Isdirect == true)
                {
                    using (var db = new SIP_ClientDataContext())
                    {
                        var _data = (from fm in db.T_FUND_MASTER_clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where sm.Nav_Check == 3
                                     select new
                                     {
                                         fm.MUTUALFUND_ID,
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         fm.NATURE_ID,
                                         sm.Launch_Date
                                     }).Distinct();

                        if (_data.Count() > 0)
                        {
                            if (listFund.Count > 0)
                            {
                                _data = _data.Where(x => listFund.Contains(x.NATURE_ID.Value));
                            }
                        }

                        dt = _data.OrderBy(x => x.Sch_Short_Name).ToDataTable();
                    }
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return dt;

        }

        public static DateTime getMaxNavDate(decimal SchemeID)
        {
            DateTime maxDate = DateTime.Today.AddDays(-3);

            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    var maxdate = (from nd in db.T_NAV_DIV_clients
                                   where nd.Scheme_Id == SchemeID
                                   select nd.Nav_Date).Max();
                    if (maxdate.HasValue)
                    {
                        maxDate = Convert.ToDateTime(maxdate);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return maxDate;
        }

        public static DataTable getLatestNavDetail(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                using (var db = new SIP_ClientDataContext())
                {

                    var _data = from nd in db.T_NAV_DIV_clients
                                join sm in db.T_SCHEMES_MASTER_Clients
                                on nd.Scheme_Id equals sm.Scheme_Id
                                where nd.Nav_Date.Value == getMaxNavDate(lstSchemeId[0])
                               // && nd.Scheme_Id == lstSchemeId[0]
                               && lstSchemeId.Contains(nd.Scheme_Id)
                                select new
                                {
                                    nd.Scheme_Id,
                                    sm.Sch_Short_Name,
                                    nd.Nav_Date,
                                    nd.Nav
                                };

                    if (_data.Count() > 0)
                    {
                        dt = _data.ToDataTable();
                    }

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        public static DataTable getTopFundData(string SchemeIds = "")
        {
            List<decimal> arr = new List<decimal>();
            DataTable dt = null;
            if (SchemeIds != string.Empty)
            {
                arr = getFundSchemeId(SchemeIds);
            }

            try
            {

                using (var db = new SIP_ClientDataContext())
                {
                    var data = from tf in db.T_TOP_FUND_Clients
                               select tf;

                    if (arr.Count > 0)
                    {
                        //data = data.Where( x => x.Calculation_Date == getMaxNavDate(arr[0]) && x.Scheme_Id == arr[0]).Select(x => x);
                        //data = data.Where(x => x.Calculation_Date == getMaxNavDate(arr[0]) && arr.Contains(x.Scheme_Id)).Select(x => x); Changed by Arindam Jash on 22nd Sept, 2015
                        data = data.Where(x => arr.Contains(x.Scheme_Id)).Select(x => x);
                    }

                    dt = data.ToDataTable();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static DataTable getHistNavDetails(string SchemeIds, DateTime frmdate, DateTime toDate)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                using (var db = new SIP_ClientDataContext())
                {

                    var _data = from nd in db.T_NAV_DIV_clients
                                join sm in db.T_SCHEMES_MASTER_Clients
                                on nd.Scheme_Id equals sm.Scheme_Id
                                where (nd.Nav_Date.Value >= frmdate && nd.Nav_Date <= toDate)
                               && lstSchemeId.Contains(nd.Scheme_Id)
                                select new
                                {
                                    nd.Scheme_Id,
                                    sm.Sch_Short_Name,
                                    nd.Nav_Date,
                                    nd.Nav
                                };

                    if (_data.Count() > 0)
                    {
                        dt = _data.ToDataTable().OrderBy("Scheme_Id,Nav_Date asc");
                    }

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        public static DataTable getHistIndexRecordDetails(string IndexIds, DateTime frmdate, DateTime toDate)
        {
            DataTable dt = null;

            List<decimal> lstIndexIds = new List<decimal>();
            if (!string.IsNullOrEmpty(IndexIds))
            {
                lstIndexIds = getFundSchemeId(IndexIds);
            }

            try
            {
                using (var db = new SIP_ClientDataContext())
                {

                    var _data = from ir in db.T_INDEX_RECORD_Clients
                                join im in db.T_INDEX_MASTER_Clients
                                on ir.INDEX_ID equals im.INDEX_ID
                                where (ir.RECORD_DATE >= frmdate && ir.RECORD_DATE <= toDate)
                              && lstIndexIds.Contains(ir.INDEX_ID)
                                select new
                                {
                                    ir.INDEX_ID,
                                    im.INDEX_NAME,
                                    ir.RECORD_DATE,
                                    ir.INDEX_VALUE
                                };

                    if (_data.Count() > 0)
                    {
                        dt = _data.ToDataTable().OrderBy("INDEX_ID,RECORD_DATE asc");
                    }

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }

        public static DataTable getDividendDetails(string SchemeIds, DateTime frmdate, DateTime toDate)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                using (var db = new SIP_ClientDataContext())
                {

                    var _data = from nd in db.T_NAV_DIV_clients
                                join sm in db.T_SCHEMES_MASTER_Clients
                                on nd.Scheme_Id equals sm.Scheme_Id
                                where (nd.Nav_Date.Value >= frmdate && nd.Nav_Date <= toDate)
                               && lstSchemeId.Contains(nd.Scheme_Id)
                               && nd.Div_Flag == true && nd.Record_Date != null
                                select new
                                {
                                    Sch_Short_Name = sm.Sch_Short_Name,
                                    Record_Date = nd.Record_Date,
                                    Div_Ind = nd.Div_Ind == null ? 0 : nd.Div_Ind.Value
                                };

                    if (_data.Count() > 0)
                    {
                        dt = _data.ToDataTable();
                    }

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        public static DataTable getNFOUpdate(int Nature = -1)
        {
            DataTable dtResult = null;

            try
            {
                using (var dc = new SIP_ClientDataContext())
                {
                    var _data = (
                                from sm in dc.T_SCHEMES_MASTER_Clients
                                join so in dc.T_SCHEMES_OPTION_Clients
                                on sm.Option_Id equals so.Option_ID
                                join fm in dc.T_FUND_MASTER_clients
                                on sm.Fund_Id equals fm.FUND_ID
                                join mf in dc.T_MF_MASTER_Clients
                                on fm.MUTUALFUND_ID equals mf.MutualFund_ID
                                join ss in dc.T_SCHEMES_STRUCTURE_Clients
                                on fm.STRUCTURE_ID equals ss.Structure_ID
                                join sn in dc.T_SCHEMES_NATURE_Clients
                                on fm.NATURE_ID equals sn.Nature_ID
                                where
                                sm.Nfo_Close_Date.Value >= DateTime.Today
                                && sm.Nav_Check == 1       //for nfo update
                                //&& fm.NATURE_ID == Nature
                                orderby sn.Nature, sm.Sch_Short_Name, sm.Nfo_Close_Date descending
                                select new
                                {
                                    sm.Scheme_Id,
                                    sm.Sch_Short_Name,
                                    fm.NATURE_ID,
                                    sn.Nature,
                                    mf.MutualFund_ID,
                                    sm.Nfo_Close_Date
                                });


                    if (Nature != -1)
                    {
                        _data = _data.Where(x => x.NATURE_ID == Nature);
                    }

                    if (_data != null && _data.Count() > 0)
                    {
                        dtResult = _data.ToDataTable();
                    }




                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        /// <summary>
        /// Added by Tamal for Bluechip
        /// </summary>
        /// <param name="Nature"></param>
        /// <param name="AMCID"></param>
        /// <param name="SubNature"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DataSet getNFOUpdateWithAddedFilter(int? Nature = -1, List<string> AMCID = null, int? SubNature = -1, DateTime? startDate = null, DateTime? endDate = null, int? structure = null)
        {
            DataSet dtResult = new DataSet();
            DateTime tryDate = new DateTime();
            try
            {
                using (var dc = new SIP_ClientDataContext())
                {
                    var _data = (
                                from sm in dc.VW_ACTIVE_SCHEMES_DETAILS_WITH_SEBIs
                                where
                                sm.Nav_Check == 1       //for nfo update
                                && sm.ISSUEOPEN.HasValue
                                && (sm.ISSUECLOSE.HasValue && sm.ISSUECLOSE.Value.Date >= DateTime.Now.Date)
                                && ((!(sm.LAUNCH_DATE.HasValue)) || sm.LAUNCH_DATE.Value.Date >= DateTime.Now.Date)                               
                                orderby sm.ISSUEOPEN.Value descending
                                select new
                                {
                                    sm.SCHEME_ID,
                                    sm.FUND_ID,
                                    sm.FUNDNAME,
                                    sm.SCHEME_SHORT_NAME,
                                    sm.SEBI_NATURE_ID,
                                    sm.SEBI_NATURE,
                                    sm.SEBI_SUB_NATURE,
                                    sm.SEBI_SUB_NATURE_ID,
                                    sm.ISSUEOPEN,
                                    sm.ISSUECLOSE,
                                    sm.FUND_MANAGER,
                                    sm.OBJECTIVE,
                                    sm.Structure_ID,
                                    sm.MutualFund_ID
                                });


                    //_data = (
                    //     (from sm in dc.T_SCHEMES_MASTER_Clients
                    //      join fm in dc.T_FUND_MASTER_clients on sm.Fund_Id equals fm.FUND_ID
                    //      join MFund in dc.T_MF_MASTER_Clients on fm.MUTUALFUND_ID equals MFund.MutualFund_ID
                    //      join sn in dc.T_SEBI_SCHEMES_NATUREs on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                    //      join ss in dc.T_SCHEMES_STRUCTURE_Clients on fm.STRUCTURE_ID equals ss.Structure_ID
                    //      join sc in dc.T_SEBI_SCHEMES_SUB_NATUREs on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                    //      where sm.Nav_Check == 1
                    //      && sm.Issue_Date.HasValue
                    //      && sm.Nfo_Close_Date.HasValue
                    //      && !(sm.Launch_Date.HasValue)
                    //      orderby sm.Issue_Date.Value descending
                    //      select new
                    //      {
                    //          sm.Scheme_Id,
                    //          sm.Fund_Id,
                    //          fm.FUND_NAME,
                    //          sm.Sch_Short_Name,
                    //          sn.Sebi_Nature_ID,
                    //          sn.Sebi_Nature,
                    //          sc.Sebi_Sub_Nature_ID,
                    //          sc.Sebi_Sub_Nature,
                    //          sm.Issue_Date,
                    //          sm.Nfo_Close_Date,
                    //          fm,
                    //          sm.OBJECTIVE,
                    //          sm.Structure_ID,
                    //          sm.MutualFund_ID
                    //      });


                    if (_data != null && _data.ToList().Count > 0 && Nature != null && Nature != -1)
                    {
                        _data = _data.Where(x => x.SEBI_NATURE_ID == Nature);
                    }

                    if (_data != null && _data.ToList().Count > 0 && SubNature != null && SubNature != -1)
                    {
                        _data = _data.Where(x => x.SEBI_SUB_NATURE_ID == SubNature);
                    }

                    if (_data != null && _data.ToList().Count > 0 && startDate.HasValue)
                    {
                        _data = _data.Where(x => x.ISSUEOPEN.Value.Date >= Convert.ToDateTime(startDate).Date);
                    }

                    if (_data != null && _data.ToList().Count > 0 && endDate.HasValue)
                    {
                        _data = _data.Where(x => x.ISSUECLOSE.Value.Date <= Convert.ToDateTime(endDate).Date);
                    }

                    if (_data != null && _data.ToList().Count > 0 && structure != null && structure != -1)
                    {
                        _data = _data.Where(x => x.Structure_ID == structure);
                    }

                    if (_data != null && _data.ToList().Count > 0 && AMCID != null && AMCID.Count > 0)
                    {
                        _data = _data.Where(x => AMCID.Contains(x.MutualFund_ID.ToString()));
                    }

                    if (_data != null && _data.ToList().Count > 0)
                    {
                        var tempDt = _data.ToDataTable();
                        var LstAssetAllocation = _data.GroupBy(x => x.FUND_ID).Select(x => x.First()).ToList();
                        dtResult.Tables.Add(tempDt);
                        dtResult.Tables.Add(LstAssetAllocation.ToDataTable());
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        public static DataTable getIndices(string IndiceCode = "")
        {


            DataTable dt = null;
            List<decimal> lstIndexId = new List<decimal>();
            if (!string.IsNullOrEmpty(IndiceCode))
            {
                lstIndexId = getFundSchemeId(IndiceCode);
            }


            try
            {

                using (var db = new SIP_ClientDataContext())
                {

                    var data = (from im in db.T_INDEX_MASTER_Clients
                                orderby im.INDEX_NAME
                                select new
                                {
                                    im.INDEX_ID,
                                    im.INDEX_NAME
                                }).Distinct();


                    if (data.Count() > 0)
                    {
                        if (lstIndexId.Count > 0)
                        {
                            data = data.Where(x => lstIndexId.Contains(x.INDEX_ID));
                        }
                    }
                    dt = data.OrderBy(x => x.INDEX_NAME).ToDataTable();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return dt;

        }

        public static DataTable getIndicesAgainstScheme(string SchemeIds)
        {

            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from si in db.T_SCHEMES_INDEX_clients
                                 join
                                 sm in db.T_SCHEMES_MASTER_Clients
                                 on si.SCHEME_ID equals sm.Scheme_Id
                                 join
                                 im in db.T_INDEX_MASTER_Clients
                                 on si.INDEX_ID equals im.INDEX_ID
                                 where
                                 lstSchemeId.Contains(si.SCHEME_ID)
                                 select new
                                 {
                                     si.SCHEME_ID,
                                     sm.Sch_Short_Name,
                                     im.INDEX_ID,
                                     im.INDEX_NAME
                                 }).Distinct();

                    if (_data.Count() > 0)
                    {
                        dt = _data.ToDataTable();
                    }

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }

        public static DataTable getFundComp(string SchIndIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchIndIds))
            {
                lstSchemeId = getFundSchemeId(SchIndIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join tp in db.T_TOP_FUND_Clients
                                 on sm.Scheme_Id equals tp.Scheme_Id
                                 join fm in db.T_FUND_MASTER_clients
                                 on sm.Fund_Id equals fm.FUND_ID
                                 join ss in db.T_SCHEMES_STRUCTURE_Clients
                                 on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sn in db.T_SCHEMES_NATURE_Clients
                                 on fm.NATURE_ID equals sn.Nature_ID
                                 join
                                 nd in db.T_NAV_DIV_clients
                                 on sm.Scheme_Id equals nd.Scheme_Id
                                 where tp.Calculation_Date ==
                                 ((from ctp in db.T_TOP_FUND_Clients
                                   where ctp.Scheme_Id == tp.Scheme_Id
                                   select new { ctp.Calculation_Date }).Max(t => t.Calculation_Date))
                                   && tp.Scheme_Id == sm.Scheme_Id
                                   && sm.Scheme_Id == nd.Scheme_Id
                                   && lstSchemeId.Contains(sm.Scheme_Id)
                                   && ss.Structure_ID == fm.STRUCTURE_ID
                                   && fm.NATURE_ID == sn.Nature_ID
                                   && tp.Calculation_Date == nd.Nav_Date
                                 orderby tp.Per_1_Year
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     Per_30_Days = (Double?)Math.Round((double)tp.Per_30_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_91_Days = (Double?)Math.Round((double)tp.Per_91_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_182_Days = (Double?)Math.Round((double)tp.Per_182_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_1_Year = (Double?)Math.Round((double)tp.Per_1_Year, 2, MidpointRounding.AwayFromZero),
                                     Per_3_Year = (Double?)Math.Round((double)tp.Per_3_Year, 2, MidpointRounding.AwayFromZero),
                                     // Per_5_Year = (Double?)Math.Round((double)tp.Per_5_Year, 2, MidpointRounding.AwayFromZero),
                                     Nav_Rs = (Double?)Math.Round((double)nd.Nav, 2, MidpointRounding.AwayFromZero),
                                     ss.Structure_Name,
                                     sn.Nature
                                 }).ToDataTable();



                    if (_data != null && _data.Rows.Count > 0)
                    {
                        _data.Columns.Add("status");
                        foreach (DataRow dr in _data.Rows)
                        {
                            dr["status"] = "1";
                        }
                    }

                    var _data2 = (from ttt in
                                      (from sm in db.T_SCHEMES_MASTER_Clients
                                       join tp in db.T_TOP_FUND_Clients
                                       on sm.Scheme_Id equals tp.Scheme_Id
                                       join fm in db.T_FUND_MASTER_clients
                                       on sm.Fund_Id equals fm.FUND_ID
                                       join ss in db.T_SCHEMES_STRUCTURE_Clients
                                       on fm.STRUCTURE_ID equals ss.Structure_ID
                                       join sn in db.T_SCHEMES_NATURE_Clients
                                       on fm.NATURE_ID equals sn.Nature_ID
                                       join
                                       nd in db.T_NAV_DIV_clients
                                       on sm.Scheme_Id equals nd.Scheme_Id
                                       where tp.Calculation_Date ==
                                       ((from ctp in db.T_TOP_FUND_Clients
                                         where ctp.Scheme_Id == tp.Scheme_Id
                                         select new { ctp.Calculation_Date }).Max(t => t.Calculation_Date))
                                         && tp.Scheme_Id == sm.Scheme_Id
                                         && sm.Scheme_Id == nd.Scheme_Id
                                         && lstSchemeId.Contains(sm.Scheme_Id)
                                         && ss.Structure_ID == fm.STRUCTURE_ID
                                         && fm.NATURE_ID == sn.Nature_ID
                                         && tp.Calculation_Date == nd.Nav_Date
                                       orderby tp.Per_1_Year
                                       select new
                                       {
                                           tp.Per_30_Days,
                                           tp.Per_91_Days,
                                           tp.Per_182_Days,
                                           tp.Per_1_Year,
                                           tp.Per_3_Year,
                                           nd.Nav,
                                           Dummy = "x"
                                       })
                                  group ttt by new { ttt.Dummy } into g
                                  select new
                                  {
                                      Per_30_Days = (Double?)Math.Round((double)g.Average(p => p.Per_30_Days), 2, MidpointRounding.AwayFromZero),
                                      Per_91_Days = (Double?)Math.Round((double)g.Average(p => p.Per_91_Days), 2, MidpointRounding.AwayFromZero),
                                      Per_182_Days = (Double?)Math.Round((double)g.Average(p => p.Per_182_Days), 2, MidpointRounding.AwayFromZero),
                                      Per_1_Year = (Double?)Math.Round((double)g.Average(p => p.Per_1_Year), 2, MidpointRounding.AwayFromZero),
                                      Per_3_Year = (Double?)Math.Round((double)g.Average(p => p.Per_3_Year), 2, MidpointRounding.AwayFromZero),
                                      Nav_Rs = (Double?)Math.Round((double)g.Average(p => p.Nav), 2, MidpointRounding.AwayFromZero)
                                  }).ToDataTable();

                    //commented by syed as per arindam suggest not to show average performance

                    //if (_data.Rows.Count != 0)
                    //{
                    //    DataRow dradd = _data.NewRow();
                    //    dradd["Scheme_Id"] = "-1";
                    //    dradd["Sch_Short_Name"] = "Average performance of similar category funds";
                    //    dradd["Per_30_Days"] = _data2.Rows.Count != 0 && _data2 != null ? _data2.Rows[0]["Per_30_Days"] : null;
                    //    dradd["Per_91_Days"] = _data2.Rows.Count != 0 && _data2 != null ? _data2.Rows[0]["Per_91_Days"] : null;
                    //    dradd["Per_182_Days"] = _data2.Rows.Count != 0 && _data2 != null ? _data2.Rows[0]["Per_182_Days"] : null;
                    //    dradd["Per_1_Year"] = _data2.Rows.Count != 0 && _data2 != null ? _data2.Rows[0]["Per_1_Year"] : null;
                    //    dradd["Per_3_Year"] = _data2.Rows.Count != 0 && _data2 != null ? _data2.Rows[0]["Per_3_Year"] : null;
                    //    dradd["Nav_Rs"] = _data2.Rows.Count != 0 && _data2 != null ? _data2.Rows[0]["Nav_Rs"] : null;
                    //    dradd["Structure_Name"] = "---";
                    //    dradd["Nature"] = "---";
                    //    dradd["status"] = "2";

                    //    _data.Rows.Add(dradd);
                    //}

                    dt = _data;//.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }
        public static DataTable getFundComparison(string SchIndIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchIndIds))
            {
                lstSchemeId = getFundSchemeId(SchIndIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join tp in db.T_TOP_FUND_Clients
                                 on sm.Scheme_Id equals tp.Scheme_Id
                                 join fm in db.T_FUND_MASTER_clients
                                 on sm.Fund_Id equals fm.FUND_ID
                                 join ss in db.T_SCHEMES_STRUCTURE_Clients
                                 on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in db.T_SCHEMES_SUB_NATURE_Clients
                                 on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join sn in db.T_SCHEMES_NATURE_Clients
                                 on fm.NATURE_ID equals sn.Nature_ID
                                 join
                                 nd in db.T_NAV_DIV_clients
                                 on sm.Scheme_Id equals nd.Scheme_Id
                                 where tp.Calculation_Date ==
                                 ((from ctp in db.T_TOP_FUND_Clients
                                   where ctp.Scheme_Id == tp.Scheme_Id
                                   select new { ctp.Calculation_Date }).Max(t => t.Calculation_Date))
                                   && tp.Scheme_Id == sm.Scheme_Id
                                   && sm.Scheme_Id == nd.Scheme_Id
                                   && lstSchemeId.Contains(sm.Scheme_Id)
                                   && ss.Structure_ID == fm.STRUCTURE_ID
                                   && fm.NATURE_ID == sn.Nature_ID
                                   && tp.Calculation_Date == nd.Nav_Date
                                 orderby tp.Per_1_Year
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     Per_30_Days = (Double?)Math.Round((double)tp.Per_30_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_91_Days = (Double?)Math.Round((double)tp.Per_91_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_182_Days = (Double?)Math.Round((double)tp.Per_182_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_1_Year = (Double?)Math.Round((double)tp.Per_1_Year, 2, MidpointRounding.AwayFromZero),
                                     Per_3_Year = (Double?)Math.Round((double)tp.Per_3_Year, 2, MidpointRounding.AwayFromZero),
                                     // Per_5_Year = (Double?)Math.Round((double)tp.Per_5_Year, 2, MidpointRounding.AwayFromZero),
                                     Nav_Rs = (Double?)Math.Round((double)nd.Nav, 2, MidpointRounding.AwayFromZero),
                                     sn.Nature,
                                     sc.Sub_Nature,
                                     sm.Option_Id,
                                     ss.Structure_Name
                                 }).ToDataTable();



                    if (_data != null && _data.Rows.Count > 0)
                    {
                        _data.Columns.Add("status");
                        foreach (DataRow dr in _data.Rows)
                        {
                            dr["status"] = "1";
                        }
                    }

                    dt = _data;//.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }

        public static DataTable getFundComparisonWithSI(string SchIndIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchIndIds))
            {
                lstSchemeId = getFundSchemeId(SchIndIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join tp in db.T_TOP_FUND_Clients
                                 on sm.Scheme_Id equals tp.Scheme_Id
                                 join fm in db.T_FUND_MASTER_clients
                                 on sm.Fund_Id equals fm.FUND_ID
                                 join ss in db.T_SCHEMES_STRUCTURE_Clients
                                 on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in db.T_SCHEMES_SUB_NATURE_Clients
                                 on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join sn in db.T_SCHEMES_NATURE_Clients
                                 on fm.NATURE_ID equals sn.Nature_ID
                                 join
                                 nd in db.T_NAV_DIV_clients
                                 on sm.Scheme_Id equals nd.Scheme_Id
                                 where tp.Calculation_Date ==
                                 ((from ctp in db.T_TOP_FUND_Clients
                                   where ctp.Scheme_Id == tp.Scheme_Id
                                   select new { ctp.Calculation_Date }).Max(t => t.Calculation_Date))
                                   && tp.Scheme_Id == sm.Scheme_Id
                                   && sm.Scheme_Id == nd.Scheme_Id
                                   && lstSchemeId.Contains(sm.Scheme_Id)
                                   && ss.Structure_ID == fm.STRUCTURE_ID
                                   && fm.NATURE_ID == sn.Nature_ID
                                   && tp.Calculation_Date == nd.Nav_Date
                                 orderby tp.Per_1_Year
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     Per_30_Days = (Double?)Math.Round((double)tp.Per_30_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_91_Days = (Double?)Math.Round((double)tp.Per_91_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_182_Days = (Double?)Math.Round((double)tp.Per_182_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_1_Year = (Double?)Math.Round((double)tp.Per_1_Year, 2, MidpointRounding.AwayFromZero),
                                     Per_3_Year = (Double?)Math.Round((double)tp.Per_3_Year, 2, MidpointRounding.AwayFromZero),
                                     Since_Inception = (Double?)Math.Round((double)tp.Since_Inception, 2, MidpointRounding.AwayFromZero),
                                     // Per_5_Year = (Double?)Math.Round((double)tp.Per_5_Year, 2, MidpointRounding.AwayFromZero),
                                     Nav_Rs = (Double?)Math.Round((double)nd.Nav, 2, MidpointRounding.AwayFromZero),
                                     sn.Nature,
                                     sc.Sub_Nature,
                                     sm.Option_Id,
                                     ss.Structure_Name
                                 }).ToDataTable();



                    if (_data != null && _data.Rows.Count > 0)
                    {
                        _data.Columns.Add("status");
                        foreach (DataRow dr in _data.Rows)
                        {
                            dr["status"] = "1";
                        }
                    }

                    dt = _data;//.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }
        public static DataTable getFundComparisonWithSI4HSBC(string SchIndIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchIndIds))
            {
                lstSchemeId = getFundSchemeId(SchIndIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join tp in db.T_TOP_FUND_Clients
                                 on sm.Scheme_Id equals tp.Scheme_Id
                                 join fm in db.T_FUND_MASTER_clients
                                 on sm.Fund_Id equals fm.FUND_ID
                                 join ss in db.T_SCHEMES_STRUCTURE_Clients
                                 on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in db.T_SCHEMES_SUB_NATURE_Clients
                                 on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join sn in db.T_SCHEMES_NATURE_Clients
                                 on fm.NATURE_ID equals sn.Nature_ID
                                 join
                                 nd in db.T_NAV_DIV_clients
                                 on sm.Scheme_Id equals nd.Scheme_Id
                                 where tp.Calculation_Date ==
                                 ((from ctp in db.T_TOP_FUND_Clients
                                   where ctp.Scheme_Id == tp.Scheme_Id
                                   select new { ctp.Calculation_Date }).Max(t => t.Calculation_Date))
                                   && tp.Scheme_Id == sm.Scheme_Id
                                   && sm.Scheme_Id == nd.Scheme_Id
                                   && lstSchemeId.Contains(sm.Scheme_Id)
                                   && ss.Structure_ID == fm.STRUCTURE_ID
                                   && fm.NATURE_ID == sn.Nature_ID
                                   && tp.Calculation_Date == nd.Nav_Date
                                 orderby tp.Per_1_Year
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     Per_30_Days = (Double?)Math.Round((double)tp.Per_30_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_91_Days = (Double?)Math.Round((double)tp.Per_91_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_182_Days = (Double?)Math.Round((double)tp.Per_182_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_1_Year = (Double?)Math.Round((double)tp.Per_1_Year, 2, MidpointRounding.AwayFromZero),
                                     Per_3_Year = (Double?)Math.Round((double)tp.Per_3_Year, 2, MidpointRounding.AwayFromZero),
                                     Per_5_Year = (Double?)Math.Round((double)tp.Per_5_Year, 2, MidpointRounding.AwayFromZero),
                                     Since_Inception = (Double?)Math.Round((double)tp.Since_Inception, 2, MidpointRounding.AwayFromZero),
                                     //Per_5_Year = (Double?)Math.Round((double)tp.Per_5_Year, 2, MidpointRounding.AwayFromZero),
                                     Nav_Rs = (Double?)Math.Round((double)nd.Nav, 2, MidpointRounding.AwayFromZero),
                                     sn.Nature,
                                     sc.Sub_Nature,
                                     sm.Option_Id,
                                     ss.Structure_Name
                                 }).ToDataTable();



                    if (_data != null && _data.Rows.Count > 0)
                    {
                        _data.Columns.Add("status");
                        foreach (DataRow dr in _data.Rows)
                        {
                            dr["status"] = "1";
                        }
                    }

                    dt = _data;//.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }
        public static DataTable getFundComparisonWithSI_LNT(string SchIndIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchIndIds))
            {
                lstSchemeId = getFundSchemeId(SchIndIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join tp in db.T_TOP_FUND_Clients
                                 on sm.Scheme_Id equals tp.Scheme_Id
                                 join fm in db.T_FUND_MASTER_clients
                                 on sm.Fund_Id equals fm.FUND_ID
                                 join ss in db.T_SCHEMES_STRUCTURE_Clients
                                 on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in db.T_SCHEMES_SUB_NATURE_Clients
                                 on fm.SUB_NATURE_ID equals sc.Sub_Nature_ID
                                 join sn in db.T_SCHEMES_NATURE_Clients
                                 on fm.NATURE_ID equals sn.Nature_ID
                                 join
                                 nd in db.T_NAV_DIV_clients
                                 on sm.Scheme_Id equals nd.Scheme_Id
                                 where tp.Calculation_Date ==
                                 ((from ctp in db.T_TOP_FUND_Clients
                                   where ctp.Scheme_Id == tp.Scheme_Id
                                   select new { ctp.Calculation_Date }).Max(t => t.Calculation_Date))
                                   && tp.Scheme_Id == sm.Scheme_Id
                                   && sm.Scheme_Id == nd.Scheme_Id
                                   && lstSchemeId.Contains(sm.Scheme_Id)
                                   && ss.Structure_ID == fm.STRUCTURE_ID
                                   && fm.NATURE_ID == sn.Nature_ID
                                   && tp.Calculation_Date == nd.Nav_Date
                                 orderby tp.Per_1_Year
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     //Per_30_Days = (Double?)Math.Round((double)tp.Per_30_Days, 2, MidpointRounding.AwayFromZero),
                                     //Per_91_Days = (Double?)Math.Round((double)tp.Per_91_Days, 2, MidpointRounding.AwayFromZero),
                                     //Per_182_Days = (Double?)Math.Round((double)tp.Per_182_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_1_Year = (Double?)Math.Round((double)tp.Per_1_Year, 4, MidpointRounding.AwayFromZero),
                                     Per_3_Year = (Double?)Math.Round((double)tp.Per_3_Year, 4, MidpointRounding.AwayFromZero),
                                     Per_5_Year = (Double?)Math.Round((double)tp.Per_5_Year, 4, MidpointRounding.AwayFromZero),
                                     Since_Inception = (Double?)Math.Round((double)tp.Since_Inception, 4, MidpointRounding.AwayFromZero),
                                     Nav_Rs = (Double?)Math.Round((double)nd.Nav, 4, MidpointRounding.AwayFromZero),
                                     sn.Nature,
                                     sc.Sub_Nature,
                                     sm.Option_Id,
                                     ss.Structure_Name,
                                     tp.Calculation_Date
                                 }).ToDataTable();



                    if (_data != null && _data.Rows.Count > 0)
                    {
                        _data.Columns.Add("status");
                        foreach (DataRow dr in _data.Rows)
                        {
                            dr["status"] = "1";
                        }
                    }

                    dt = _data;//.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }
        //sebi compairions
        public static DataTable getSebiFundComparisonWithSI(string SchIndIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchIndIds))
            {
                lstSchemeId = getFundSchemeId(SchIndIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join tp in db.T_TOP_FUND_Clients
                                 on sm.Scheme_Id equals tp.Scheme_Id
                                 join fm in db.T_FUND_MASTER_clients
                                 on sm.Fund_Id equals fm.FUND_ID
                                 join ss in db.T_SCHEMES_STRUCTURE_Clients
                                 on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in db.T_SEBI_SCHEMES_SUB_NATUREs
                                 on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join sn in db.T_SEBI_SCHEMES_NATUREs
                                 on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join
                                 nd in db.T_NAV_DIV_clients
                                 on sm.Scheme_Id equals nd.Scheme_Id
                                 where tp.Calculation_Date ==
                                 ((from ctp in db.T_TOP_FUND_Clients
                                   where ctp.Scheme_Id == tp.Scheme_Id
                                   select new { ctp.Calculation_Date }).Max(t => t.Calculation_Date))
                                   && tp.Scheme_Id == sm.Scheme_Id
                                   && sm.Scheme_Id == nd.Scheme_Id
                                   && lstSchemeId.Contains(sm.Scheme_Id)
                                   && ss.Structure_ID == fm.STRUCTURE_ID
                                   && fm.SEBI_NATURE_ID == sn.Sebi_Nature_ID
                                   && tp.Calculation_Date == nd.Nav_Date
                                 orderby tp.Per_1_Year
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     Per_30_Days = (Double?)Math.Round((double)tp.Per_30_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_91_Days = (Double?)Math.Round((double)tp.Per_91_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_182_Days = (Double?)Math.Round((double)tp.Per_182_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_1_Year = (Double?)Math.Round((double)tp.Per_1_Year, 2, MidpointRounding.AwayFromZero),
                                     Per_3_Year = (Double?)Math.Round((double)tp.Per_3_Year, 2, MidpointRounding.AwayFromZero),
                                     Since_Inception = (Double?)Math.Round((double)tp.Since_Inception, 2, MidpointRounding.AwayFromZero),
                                     // Per_5_Year = (Double?)Math.Round((double)tp.Per_5_Year, 2, MidpointRounding.AwayFromZero),
                                     Nav_Rs = (Double?)Math.Round((double)nd.Nav, 2, MidpointRounding.AwayFromZero),
                                     sn.Sebi_Nature,
                                     sc.Sebi_Sub_Nature,
                                     sm.Option_Id,
                                     ss.Structure_Name,

                                 }).OrderByDescending(x => x.Per_1_Year).ToDataTable();



                    if (_data != null && _data.Rows.Count > 0)
                    {
                        _data.Columns.Add("status");
                        foreach (DataRow dr in _data.Rows)
                        {
                            dr["status"] = "1";
                        }
                    }

                    dt = _data;//.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }

        public static DataTable getSebiFundComparisonWithSI1(string SchIndIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchIndIds))
            {
                lstSchemeId = getFundSchemeId(SchIndIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext())
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join tp in db.T_TOP_FUND_Clients
                                 on sm.Scheme_Id equals tp.Scheme_Id
                                 join fm in db.T_FUND_MASTER_clients
                                 on sm.Fund_Id equals fm.FUND_ID
                                 join ss in db.T_SCHEMES_STRUCTURE_Clients
                                 on fm.STRUCTURE_ID equals ss.Structure_ID
                                 join sc in db.T_SEBI_SCHEMES_SUB_NATUREs
                                 on fm.SEBI_SUB_NATURE_ID.Value equals sc.Sebi_Sub_Nature_ID
                                 join sn in db.T_SEBI_SCHEMES_NATUREs
                                 on fm.SEBI_NATURE_ID equals sn.Sebi_Nature_ID
                                 join
                                 nd in db.T_NAV_DIV_clients
                                 on sm.Scheme_Id equals nd.Scheme_Id
                                 where tp.Calculation_Date ==
                                 ((from ctp in db.T_TOP_FUND_Clients
                                   where ctp.Scheme_Id == tp.Scheme_Id
                                   select new { ctp.Calculation_Date }).Max(t => t.Calculation_Date))
                                   && tp.Scheme_Id == sm.Scheme_Id
                                   && sm.Scheme_Id == nd.Scheme_Id
                                   && lstSchemeId.Contains(sm.Scheme_Id)
                                   && ss.Structure_ID == fm.STRUCTURE_ID
                                   && fm.SEBI_NATURE_ID == sn.Sebi_Nature_ID
                                   && tp.Calculation_Date == nd.Nav_Date
                                 orderby tp.Per_1_Year
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     Per_30_Days = (Double?)Math.Round((double)tp.Per_30_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_91_Days = (Double?)Math.Round((double)tp.Per_91_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_182_Days = (Double?)Math.Round((double)tp.Per_182_Days, 2, MidpointRounding.AwayFromZero),
                                     Per_1_Year = (Double?)Math.Round((double)tp.Per_1_Year, 2, MidpointRounding.AwayFromZero),
                                     Per_3_Year = (Double?)Math.Round((double)tp.Per_3_Year, 2, MidpointRounding.AwayFromZero),
                                     Since_Inception = (Double?)Math.Round((double)tp.Since_Inception, 2, MidpointRounding.AwayFromZero),
                                     // Per_5_Year = (Double?)Math.Round((double)tp.Per_5_Year, 2, MidpointRounding.AwayFromZero),
                                     Nav_Rs = (Double?)Math.Round((double)nd.Nav, 2, MidpointRounding.AwayFromZero),
                                     sn.Sebi_Nature,
                                     sc.Sebi_Sub_Nature,
                                     sm.Option_Id,
                                     ss.Structure_Name,
                                     Fund_Size = (Double?)Math.Round(Convert.ToDecimal(db.T_FUND_SIZEs.Where(v => v.SCHEME_ID == sm.Scheme_Id && v.FUNDSIZE_DATE == db.T_FUND_SIZEs.Where(x => x.SCHEME_ID == sm.Scheme_Id).Max(y => y.FUNDSIZE_DATE)).FirstOrDefault().MONTHLY_FUND_SIZE), 2, MidpointRounding.AwayFromZero)

                                 }).OrderByDescending(x => x.Per_1_Year).ToDataTable();



                    if (_data != null && _data.Rows.Count > 0)
                    {
                        _data.Columns.Add("status");
                        foreach (DataRow dr in _data.Rows)
                        {
                            dr["status"] = "1";
                        }
                    }

                    dt = _data;//.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;

        }

        // Get the Latest portfolio Date
        public static DateTime getLatestPortfiloDate(string SchemeIds)
        {
            DateTime dt = DateTime.Today.AddDays(-30);

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    var _data = from p in
                                    (from pd in db.T_COM_POT_Clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on pd.FUND_ID equals sm.Fund_Id
                                     where lstSchemeId.Contains(sm.Scheme_Id)
                                     select new
                                     {
                                         pd.FUND_ID,
                                         pd.PORT_DATE
                                     })
                                group p by p.FUND_ID into grp
                                select new
                                {
                                    PortDate = grp.Max(t => t.PORT_DATE)
                                };

                    if (_data != null)
                    {
                        dt = Convert.ToDateTime(_data.Min(x => x.PortDate));
                    }



                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        public static DataTable getLatestPortfiloDateDT(string SchemeIds)
        {

            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    var _data = from p in
                                    (from pd in db.T_COM_POT_Clients
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on pd.FUND_ID equals sm.Fund_Id
                                     where lstSchemeId.Contains(sm.Scheme_Id)
                                     select new
                                     {
                                         pd.FUND_ID,
                                         pd.PORT_DATE
                                     })
                                group p by p.FUND_ID into grp
                                select new
                                {
                                    PortDate = grp.Max(t => t.PORT_DATE)
                                };

                    if (_data != null)
                    {
                        dt = _data.ToDataTable();
                    }



                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        // Fetch The Category of the respective Scheme
        public static DataTable getCategoryOfScheme(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join fm in db.T_FUND_MASTER_clients on sm.Fund_Id equals fm.FUND_ID
                                 join sn in db.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                 where lstSchemeId.Contains(sm.Scheme_Id)
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     sn.Nature_ID,
                                     sn.Nature,
                                     fm.FUND_NAME
                                 });

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }


        //fetch the respective fundmanager names
        public static DataTable getFundManager(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join fm in db.T_FUND_MASTER_clients
                                 on sm.Fund_Id equals fm.FUND_ID
                                 join cfm in db.T_CURRENT_FUND_MANAGER_Clients
                                 on fm.FUND_ID equals cfm.FUND_ID
                                 join tfm in db.T_FUND_MANAGER_Clients
                                 on cfm.FUNDMAN_ID equals tfm.FUNDMAN_ID
                                 where lstSchemeId.Contains(sm.Scheme_Id)
                                 && cfm.LATEST_FUNDMAN == true
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     cfm.FUNDMAN_ID,
                                     tfm.FUND_MANAGER_NAME,
                                     fm.FUND_NAME
                                 });

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        // get the Icron ranking of the schemes
        public static DataTable getSchemeICRONRank(string SchemeIds, string yearChk, int maxQtrCnt = 1)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {


                    var qtrId = (from rqm in db.T_RANKING_QUARTER_MASTER_Clients
                                 orderby rqm.Quarter_Id descending
                                 select rqm.Quarter_Id).Take(maxQtrCnt).ToList();


                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join r in db.T_RANKING_MFR_Clients
                                 on sm.Scheme_Id equals r.Scheme_Id
                                 join rcm in db.T_RANKING_CATEGORY_MASTER_Clients
                                 on r.Category_Id equals rcm.Category_Id
                                 join rqm in db.T_RANKING_QUARTER_MASTER_Clients
                                 on r.Quarter_Id equals rqm.Quarter_Id
                                 where lstSchemeId.Contains(r.Scheme_Id)
                                 && r.Year_Check.ToUpper() == yearChk.ToUpper()
                                 && qtrId.Contains(r.Quarter_Id)
                                 orderby r.Quarter_Id descending, sm.Sch_Short_Name ascending,
                                 rcm.Category ascending, r.Year_Check ascending, r.MFR descending
                                 select new
                                 {
                                     SCHEME_NAME = sm.Sch_Short_Name,
                                     SCHEME_ID = Convert.ToInt32(sm.Scheme_Id),
                                     RANK = (r.MFR.ToString() + " STAR"),
                                     rqm.Quarter_Name
                                 });

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }


        //Get Asset Allocation
        public static DataTable getAssetAllocation(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {

                    DateTime portFolioDate = getLatestPortfiloDate(SchemeIds);

                    var _data = from p in
                                    (from cp in db.T_COM_POT_Clients
                                     join cpd in db.T_COM_POT_DETAIL_Clients
                                     on cp.PORT_ID equals cpd.PORT_ID
                                     join cn in db.T_COMPANY_NATURE_Clients
                                     on cpd.COM_NATURE_ID equals cn.COM_NATURE_ID
                                     join fm in db.T_FUND_MASTER_clients
                                     on cp.FUND_ID equals fm.FUND_ID
                                     join sm in db.T_SCHEMES_MASTER_Clients
                                     on fm.FUND_ID equals sm.Fund_Id
                                     where lstSchemeId.Contains(sm.Scheme_Id)
                                     && cp.PORT_DATE == portFolioDate
                                     select new
                                     {
                                         sm.Sch_Short_Name,
                                         sm.Scheme_Id,
                                         fm.FUND_NAME,
                                         fm.FUND_ID,
                                         cpd.COM_NATURE_ID,
                                         cn.NATURE_NAME,
                                         cpd.CORPUS_PER
                                     })
                                group p by new
                                {
                                    p.FUND_ID,
                                    p.FUND_NAME,
                                    p.Sch_Short_Name,
                                    p.Scheme_Id,
                                    p.NATURE_NAME,
                                    p.COM_NATURE_ID
                                } into grp
                                select new
                                {
                                    grp.Key.Scheme_Id,
                                    grp.Key.Sch_Short_Name,
                                    grp.Key.FUND_ID,
                                    grp.Key.FUND_NAME,
                                    grp.Key.COM_NATURE_ID,
                                    grp.Key.NATURE_NAME,
                                    Corpus_per = grp.Sum(t => t.CORPUS_PER)
                                };

                    _data = _data.OrderBy(t => t.Sch_Short_Name).ThenBy(t => t.NATURE_NAME);


                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Top holding Function
        public static DataTable getTopHolding(string SchemeIds, int top = 5)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    DateTime portFolioDate = getLatestPortfiloDate(SchemeIds);



                    var _data = (from p in
                                     (from cp in db.T_COM_POT_Clients
                                      join cpd in db.T_COM_POT_DETAIL_Clients
                                      on cp.PORT_ID equals cpd.PORT_ID
                                      join sm in db.T_SCHEMES_MASTER_Clients
                                      on cp.FUND_ID equals sm.Fund_Id
                                      orderby cpd.CORPUS_PER descending
                                      where lstSchemeId.Contains(sm.Scheme_Id)
                                      && cp.PORT_DATE == portFolioDate
                                      select new
                                      {
                                          cpd.CORPUS_PER,
                                          sm.Sch_Short_Name,
                                          sm.Scheme_Id,
                                          cpd.PORT_ID
                                      })
                                 group p by new { p.Sch_Short_Name, p.Scheme_Id, p.PORT_ID } into grp
                                 select
                                new topHolding
                                {
                                    SumCorpus = grp.Take(top).Sum(t => t.CORPUS_PER),
                                    Sch_Short_Name = grp.Key.Sch_Short_Name,
                                    Scheme_Id = grp.Key.Scheme_Id,
                                    PORT_ID = grp.Key.PORT_ID
                                });

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Market Cap classification
        public static DataTable getMarketCapClassification(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    DateTime portFolioDate = getLatestPortfiloDate(SchemeIds);

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join mc in db.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients
                                 on sm.Fund_Id equals mc.FUND_ID
                                 where lstSchemeId.Contains(sm.Scheme_Id)
                                 && mc.CALC_DATE.Month == portFolioDate.Month
                                 && mc.CALC_DATE.Year == portFolioDate.Year
                                 select new
                                 {
                                     sm.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     mc.FUND_ID,
                                     mc.CALC_DATE,
                                     mc.MARKET_SLAB,
                                     mc.MCAPALLOCATION
                                 });

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Top Sector Function
        public static DataTable getTopCompanyHolding(string SchemeIds, int top = 5)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    DateTime portFolioDate = getLatestPortfiloDate(SchemeIds);

                    var _data = (from p in
                                     (from cp in db.T_COM_POT_Clients
                                      join cpd in db.T_COM_POT_DETAIL_Clients
                                      on cp.PORT_ID equals cpd.PORT_ID
                                      join sm in db.T_SCHEMES_MASTER_Clients
                                      on cp.FUND_ID equals sm.Fund_Id
                                      join cm in db.T_COMPANY_MASTER_Clients
                                      on cpd.COMPANY_ID equals cm.Company_Id
                                      orderby cpd.CORPUS_PER descending
                                      where lstSchemeId.Contains(sm.Scheme_Id)
                                      && cp.PORT_DATE == portFolioDate
                                      select new
                                      {
                                          cpd.CORPUS_PER,
                                          sm.Sch_Short_Name,
                                          sm.Scheme_Id,
                                          cpd.PORT_ID,
                                          cm.Company_Name,
                                          cm.Company_Id
                                      })
                                 group p by new { p.Sch_Short_Name, p.PORT_ID, p.Company_Name, p.Scheme_Id, p.Company_Id } into grp
                                 select new
                                 {
                                     SumCorpus = grp.Sum(t => t.CORPUS_PER),
                                     grp.Key.Company_Name,
                                     grp.Key.Company_Id,
                                     grp.Key.Sch_Short_Name,
                                     grp.Key.Scheme_Id,
                                     grp.Key.PORT_ID

                                 })
                               .OrderByDescending(t => t.SumCorpus);

                    var dddata = (from d in _data.AsEnumerable()
                                  group d by d.Scheme_Id
                                      into grp
                                  let topsum = grp.Select(t => t.SumCorpus).OrderByDescending(k => k.Value).Take(top)
                                  from p in grp
                                  where topsum.Contains(p.SumCorpus)
                                  orderby p.SumCorpus descending
                                  select p).OrderBy(t => t.Scheme_Id);

                    if (dddata != null)
                        dt = dddata.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Top Sector Function
        public static DataTable getTopSector(string SchemeIds, int top = 3)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    DateTime portFolioDate = getLatestPortfiloDate(SchemeIds);

                    var _data = (from p in
                                     (from cp in db.T_COM_POT_Clients
                                      join cpd in db.T_COM_POT_DETAIL_Clients
                                      on cp.PORT_ID equals cpd.PORT_ID
                                      join sm in db.T_SCHEMES_MASTER_Clients
                                      on cp.FUND_ID equals sm.Fund_Id
                                      join cm in db.T_COMPANY_MASTER_Clients
                                      on cpd.COMPANY_ID equals cm.Company_Id
                                      join scm in db.T_SECTOR_MASTER_Clients
                                      on cm.Sector_Id equals scm.Sector_ID
                                      orderby cpd.CORPUS_PER descending
                                      where lstSchemeId.Contains(sm.Scheme_Id)
                                      && cp.PORT_DATE == portFolioDate
                                      select new
                                      {
                                          cpd.CORPUS_PER,
                                          sm.Sch_Short_Name,
                                          sm.Scheme_Id,
                                          cpd.PORT_ID,
                                          scm.Sector_Name
                                      })
                                 group p by new { p.Sch_Short_Name, p.PORT_ID, p.Sector_Name, p.Scheme_Id } into grp
                                 select new
                                 {
                                     SumCorpus = grp.Sum(t => t.CORPUS_PER),
                                     grp.Key.Sector_Name,
                                     grp.Key.Sch_Short_Name,
                                     grp.Key.Scheme_Id,
                                     grp.Key.PORT_ID

                                 })
                               .OrderByDescending(t => t.SumCorpus);

                    //  var _dataa = _data.Select((item, i) => new { Item = item, Index = i });


                    //var ddata = new List<topHolding>();
                    //foreach (decimal sch in lstSchemeId)
                    //{
                    //    var d = _data.Where(x => x.Scheme_Id == sch).OrderByDescending(x => x.SumCorpus).Select(x => x).Take(top);

                    //    var dta = (from rec in d.AsEnumerable()
                    //               select new topHolding
                    //                   {
                    //                       PORT_ID = rec.PORT_ID,
                    //                       Sch_Short_Name = rec.Sch_Short_Name,
                    //                       Scheme_Id = rec.Scheme_Id,
                    //                       Sector_Name = rec.Sector_Name,
                    //                       SumCorpus = rec.SumCorpus
                    //                   }).ToList();
                    //    ddata.AddRange(dta);
                    //}

                    var dddata = (from d in _data.AsEnumerable()
                                  group d by d.Scheme_Id
                                      into grp
                                  let topsum = grp.Select(t => t.SumCorpus).OrderByDescending(k => k.Value).Take(top)
                                  from p in grp
                                  where topsum.Contains(p.SumCorpus)
                                  orderby p.SumCorpus descending
                                  select p).OrderBy(t => t.Scheme_Id);

                    if (dddata != null)
                        dt = dddata.ToDataTable();


                    //if (ddata != null && ddata.Count > 0)
                    //    dt = ddata.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }


        //Get Portfolio PT Function
        public static DataTable getPortfolioPT(string SchemeIds)
        {
            DataTable dt = null;
            DateTime portFolioDate = getLatestPortfiloDate(SchemeIds);
            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join pr in db.T_PT_RATIO_Clients
                                 on sm.Fund_Id equals pr.Fund_Id
                                 where lstSchemeId.Contains(sm.Scheme_Id)
                                      && pr.Import_Date.Month == portFolioDate.Month
                                      && pr.Import_Date.Year == portFolioDate.Year
                                 select new
                                 {
                                     pr.Fund_Id,
                                     pr.Import_Date,
                                     pr.Ratio,
                                     sm.Scheme_Id
                                 });

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Portfolio PT Function
        public static DataTable getPortfolioPE(string SchemeIds, DateTime date)
        {
            DataTable dt = null;
            DateTime portFolioDate = getLatestPortfiloDate(SchemeIds);
            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    IMultipleResults _dataT = db.MFIE_SP_ATTRIBUTION_ANALYSIS(SchemeIds, date.ToString("MM-dd-yyyy").Split(' ')[0], "");

                    var _data = _dataT.GetResult<AttributeAnalysisOutput>();
                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Avg_Mat YTM
        public static DataTable getAvg_Mat_YTM(string SchemeIds)
        {
            DataTable dt = null;
            DateTime portFolioDate = getLatestPortfiloDate(SchemeIds);

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join cp in db.T_COM_POT_Clients
                                 on sm.Fund_Id equals cp.FUND_ID
                                 where lstSchemeId.Contains(sm.Scheme_Id)
                                      && cp.PORT_DATE.Month == portFolioDate.Month
                                      && cp.PORT_DATE.Year == portFolioDate.Year
                                 select new
                                 {
                                     cp.FUND_ID,
                                     cp.AVG_MAT,
                                     cp.YTM,
                                     cp.PORT_DATE,
                                     sm.Scheme_Id
                                 });

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Expense Ratio
        public static DataTable getExpenseRatio(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    var _data = (from p in
                                     (
                                         from sm in db.T_SCHEMES_MASTER_Clients
                                         join er in db.T_SCHEME_EXPENSE_Clients
                                         on sm.Scheme_Id equals er.SCHEME_ID
                                         where lstSchemeId.Contains(sm.Scheme_Id)
                                         select new
                                         {
                                             sm.Scheme_Id,
                                             Calc_date = er.DATE_TO,
                                             Exp_Ratio = er.ACTUAL_PERCENTAGE,
                                             Seq = "1"
                                         })
                                 group p by p.Scheme_Id
                                     into grp
                                 let MaxDate = grp.Max(x => x.Calc_date)
                                 from rp in grp
                                 where rp.Calc_date == MaxDate
                                 select rp)
                                .Union
                                (from p in
                                     (
                                         from sm in db.T_SCHEMES_MASTER_Clients
                                         join sec in db.T_SCHEME_EXPENSE_CURRENT_Clients
                                         on sm.Scheme_Id equals sec.SCHEME_ID
                                         where lstSchemeId.Contains(sm.Scheme_Id)
                                         select new
                                         {
                                             sm.Scheme_Id,
                                             Calc_date = sec.EXPENSE_DATE,
                                             Exp_Ratio = sec.EXPENSE_PERCENT,
                                             Seq = "2"
                                         })
                                 group p by p.Scheme_Id
                                     into grp
                                 let MaxDate = grp.Max(x => x.Calc_date)
                                 from rp in grp
                                 where rp.Calc_date == MaxDate
                                 select rp)
                                     .Union
                                       (from p in
                                            (
                                                from sm in db.T_SCHEMES_MASTER_Clients
                                                join seh in db.T_SCHEME_EXPENSE_HALFYEARLY_Clients
                                                on sm.Scheme_Id equals seh.SCHEME_ID
                                                where lstSchemeId.Contains(sm.Scheme_Id)
                                                select new
                                                {
                                                    sm.Scheme_Id,
                                                    Calc_date = seh.EXPENSE_DATE,
                                                    Exp_Ratio = seh.EXPENSE_PERCENT,
                                                    Seq = "3"
                                                })
                                        group p by p.Scheme_Id
                                            into grp
                                        let MaxDate = grp.Max(x => x.Calc_date)
                                        from rp in grp
                                        where rp.Calc_date == MaxDate
                                        select rp);


                    var _data2 = from d in _data.AsEnumerable()
                                 group d by d.Scheme_Id
                                     into grpp
                                 let MaxxDate = grpp.Max(x => x.Calc_date)
                                 from p in grpp
                                 where p.Calc_date == MaxxDate
                                 select p;


                    //select new
                    //{
                    //  //  grp.Key.Exp_Ratio,
                    //    grp.Key.Scheme_Id,
                    //    Calc_date = Convert.ToDateTime(grp.Select(t => t.Calc_date).Take(1))
                    //}

                    if (_data2 != null)
                        dt = _data2.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Exit Load Function
        public static DataTable getEntryExitLoad(string SchemeIds)
        {
            DataTable dt = null;

            try
            {
                if (string.IsNullOrEmpty(SchemeIds))
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {

                    IMultipleResults datatble = db.MFIE_SP_SCHEME_GENERAL_INFORMATION("LATEST_ENTRY_EXIT", SchemeIds, "");
                    //principl.MFIE_SP_SIP_CALCULATER(strSchid, _fromDatemodified, _toDate, _asOnDate, dblSIPamt, strFrequency, strInvestorType, 0, 0, null, 1);
                    var firstTable = datatble.GetResult<ExitLoadOutput>();
                    //dt = firstTable.ToDataTable();


                    if (firstTable != null)
                        dt = firstTable.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get performance Return from T_TOP_FUND Table
        public static DataTable getPerformance_TF(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join tf in db.T_TOP_FUND_Clients
                                 on sm.Scheme_Id equals tf.Scheme_Id
                                 where lstSchemeId.Contains(tf.Scheme_Id)
                                 && tf.Calculation_Date >= db.T_TOP_FUND_Clients.Select(x => x.Calculation_Date).Max().AddDays(-10)
                                 select new
                                 {
                                     sm.Sch_Short_Name,
                                     tf.Scheme_Id,
                                     tf.Calculation_Date,
                                     tf.Per_7_Days,
                                     tf.Per_15_Days,
                                     tf.Per_91_Days,
                                     tf.Per_30_Days,
                                     tf.Per_182_Days,
                                     tf.Per_1_Year,
                                     tf.Per_3_Year,
                                     tf.Per_5_Year
                                 }
                                 );

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get performance Return from MFIE_SP_SCHEME_P2P_ROLLING_RETURN  procedure
        public static DataTable getPerformance_SP(string SchemeIds, string strRollingPeriodin, DateTime EndDate, int settingSet = 2, int RoundTill = 2)
        {
            DataTable dt = null;
            conn.ConnectionString = connstr;

            try
            {
                if (string.IsNullOrEmpty(SchemeIds))
                    return dt;


                int val = 0;
                SqlCommand cmd;
                SqlDataAdapter da = new SqlDataAdapter();


                DataTable dtSchemee = new DataTable();
                //calling the sp to get Scheme Absolute return
                cmd = new SqlCommand("MFIE_SP_SCHEME_P2P_ROLLING_RETURN", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 2000;
                cmd.Parameters.Add(new SqlParameter("@SchemeIDs", SchemeIds));
                cmd.Parameters.Add(new SqlParameter("@SettingSetID", 2));
                cmd.Parameters.Add(new SqlParameter("@DateFrom", ""));
                cmd.Parameters.Add(new SqlParameter("@DateTo", EndDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmd.Parameters.Add(new SqlParameter("@RoundTill", 2));
                cmd.Parameters.Add(new SqlParameter("@RollingPeriodin", strRollingPeriodin));
                cmd.Parameters.Add(new SqlParameter("@RollingPeriod", val));
                cmd.Parameters.Add(new SqlParameter("@RollingFrequencyin", ""));
                cmd.Parameters.Add(new SqlParameter("@RollingFrequency", val));
                cmd.Parameters.Add(new SqlParameter("@Rolling_P2P", "p2p"));
                cmd.Parameters.Add(new SqlParameter("@OtherCalculation", 'N'));



                da.SelectCommand = cmd;
                da.Fill(dtSchemee);

                if (dtSchemee != null)
                    dt = dtSchemee.Copy();

            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }



        //Get SIP Analysis
        public static DataTable getSIPAnalysis(string SchemeIds, double SIPAmount, DateTime investmentDate, DateTime sipEndDate, DateTime sipAsonDate, string strFrequency, string strInvestorType = "Individual/Huf")
        {
            DataTable dt = null;
            DataTable SipDtable1 = new DataTable();
            DataTable SipDtable2 = new DataTable();
            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (string.IsNullOrEmpty(SchemeIds))
                    return dt;



                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {


                    /////////////////---By Calling sp Single time  --------#######################################

                    IMultipleResults datatble = db.MFIE_SP_SIP_CALCULATER_CLIENT_Scheme(SchemeIds, investmentDate,
                        sipEndDate, sipAsonDate, SIPAmount, strFrequency, strInvestorType, 0, 0, null, 1, "Y");

                    //foreach (string str in SchemeIds.Split(',').ToList())
                    //{
                    //    var firstTable = datatble.GetResult<SipCalcReturnDataDetail>();
                    //}

                    var secondTable = datatble.GetResult<SipCalcReturnDataSummary>();

                    /////////////////---By Calling sp Single time  --------#######################################

                    /////////////////---By Calling sp Multiple  time  --------#######################################

                    // List<SipCalcReturnDataSummary> objListSummary = new List<SipCalcReturnDataSummary>();

                    // foreach (string str in SchemeIds.Split(',').ToList())
                    // {
                    //     IMultipleResults datatble2 = db.MFIE_SP_SIP_CALCULATER_CLIENT(str, investmentDate,
                    //sipEndDate, sipAsonDate, SIPAmount, strFrequency, strInvestorType, 0, 0, null, 1, "Y");

                    //     var firstTable2 = datatble2.GetResult<SipCalcReturnDataDetail>();
                    //     var secondTable2 = datatble2.GetResult<SipCalcReturnDataSummary>();
                    //     SipCalcReturnDataSummary dataSelect = (SipCalcReturnDataSummary)secondTable2.AsEnumerable().Where(x => x.ID == 1).Select(x => x).FirstOrDefault();
                    //     objListSummary.Add(dataSelect);
                    // }


                    /////////////////---By Calling sp Multiple time  --------#######################################


                    //var _data = from sm in db.T_SCHEMES_MASTER_Clients.AsEnumerable()
                    //            join d in secondTable.AsEnumerable()// objListSummary.AsEnumerable() 
                    //           on Convert.ToString(sm.Sch_Short_Name).Trim() equals Convert.ToString(d.SCHEME).Trim()                              
                    //            into ords                                
                    //           from m in ords.DefaultIfEmpty()
                    //            where lstSchemeId.Contains(sm.Scheme_Id)
                    //           select new
                    //           {                                  
                    //               sm.Sch_Short_Name,
                    //               sm.Scheme_Id,
                    //               m.PRESENT_VALUE,
                    //               m.YIELD,                                  
                    //               m.TOTAL_AMOUNT,
                    //               m.TOTAL_UNIT,
                    //               m.PROFIT_ONETIME,                                 
                    //               m.PROFIT_SIP
                    //           };

                    //if (_data != null)
                    //    dt = _data.ToDataTable();

                    if (secondTable != null)
                        dt = secondTable.ToDataTable();


                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return dt;
        }

        //Get Risk Measure Analysis
        public static DataTable getRiskMeasure(string SchemeIds)
        {
            DataTable dt = null;

            List<decimal> lstSchemeId = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeId = getFundSchemeId(SchemeIds);
            }

            try
            {
                if (lstSchemeId.Count == 0)
                    return dt;

                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {

                    var _data = (from sm in db.T_SCHEMES_MASTER_Clients
                                 join pcr in db.T_PRECALCULATED_RATIOS_MFI_Clients
                                 on sm.Scheme_Id equals pcr.Scheme_Id
                                 where lstSchemeId.Contains(pcr.Scheme_Id)
                                 select new
                                 {
                                     pcr.Scheme_Id,
                                     sm.Sch_Short_Name,
                                     pcr.Sharp,
                                     pcr.Sortino,
                                     pcr.STDV
                                 });

                    if (_data != null)
                        dt = _data.ToDataTable();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return dt;
        }

        ////Get Sample Function
        //public static DataTable getSample(string SchemeIds)
        //{
        //    DataTable dt = null;

        //    List<decimal> lstSchemeId = new List<decimal>();
        //    if (!string.IsNullOrEmpty(SchemeIds))
        //    {
        //        lstSchemeId = getFundSchemeId(SchemeIds);
        //    }

        //    try
        //    {
        //        if (lstSchemeId.Count == 0)
        //            return dt;

        //        using (var db = new SIP_ClientDataContext())
        //        {

        //            var _data = (from sm in db.T_SCHEMES_MASTER_Clients
        //                         select sm);

        //            if (_data != null)
        //                dt = _data.ToDataTable();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }


        //    return dt;
        //}

        public static List<AssetAlocation> getAssetAllocationUsingFundId(int SchemeId)
        {
            List<AssetAlocation> LstAssetAllocation = null;
            try
            {
                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    DateTime MaxPortDate = DateTime.MinValue;
                    decimal portId;
                    var FundId = db.T_SCHEMES_MASTER_Clients.Where(n => n.Scheme_Id == SchemeId).Select(x => x.Fund_Id).FirstOrDefault();
                    var ChkPort = db.T_COM_POT_Clients.Where(v => v.FUND_ID == FundId).Select(c => c);
                    if (ChkPort.Count() > 0)
                    {
                        MaxPortDate = ChkPort.OrderByDescending(v => v.PORT_DATE).Select(v => v.PORT_DATE).FirstOrDefault();
                        portId = ChkPort.Where(c => c.PORT_DATE == MaxPortDate).Select(k => k.PORT_ID).FirstOrDefault();
                    }
                    else
                    {
                        return LstAssetAllocation;
                    }

                    LstAssetAllocation = (from v in db.T_COM_POT_DETAIL_Clients.Where(v => v.PORT_ID == portId).Select(c => c)
                                          join cn in db.T_COMPANY_NATURE_Clients.Select(k => k) on v.COM_NATURE_ID equals cn.COM_NATURE_ID
                                          select new AssetAlocation
                                          {
                                              NatureId = cn.COM_NATURE_ID,
                                              NatureName = cn.NATURE_NAME,
                                              Value = string.IsNullOrEmpty(Convert.ToString(v.CORPUS_PER.Value)) ? 0 : v.CORPUS_PER.Value,
                                              Date = MaxPortDate.ToString("dd-MM-yyyy")
                                          }).ToList();

                    LstAssetAllocation = (from v in LstAssetAllocation
                                          group v by v.NatureId into Rows
                                          select (
                                          new AssetAlocation
                                          {
                                              NatureName = Rows.Select(j => j.NatureName).FirstOrDefault() == "EQ" ? "Equity" : Rows.Select(j => j.NatureName).FirstOrDefault(),
                                              Value = Math.Round((double)Rows.Sum(r => r.Value), 2),
                                              Date = MaxPortDate.ToString("dd-MM-yyyy")
                                          })).ToList();
                }
                LstAssetAllocation = (from u in LstAssetAllocation
                                      where u.Value != 0
                                      select (
                                      new AssetAlocation
                                      {
                                          NatureName = u.NatureName,
                                          Value = u.Value,
                                          Date = u.Date
                                      })).ToList();

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return LstAssetAllocation;
        }


        public static List<List<NavReturnModelSP>> GetNavFrmSP(decimal schemeIds, DateTime fromDate, DateTime toDate, string indexIds)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = connstr;
                conn.Open();
            }
            List<List<NavReturnModelSP>> navRtn = new List<List<NavReturnModelSP>>();
            try
            {
                SqlCommand cmd;
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet dsRtn = new DataSet();
                cmd = new SqlCommand("MFIE_SP_HISTORICAL_REINVESTADDBACK_NAV_MUKESH", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 2000;
                cmd.Parameters.Add(new SqlParameter("@SCHEME_IDS", schemeIds));
                cmd.Parameters.Add(new SqlParameter("@DATEFROM", fromDate));
                cmd.Parameters.Add(new SqlParameter("@DATETO", toDate));
                cmd.Parameters.Add(new SqlParameter("@DIVTYPE", "INDIVIDUAL"));
                cmd.Parameters.Add(new SqlParameter("@PREV_DATE_MOVEMENT", ""));
                cmd.Parameters.Add(new SqlParameter("@MAIN_DATE_MOVEMENT", ""));
                cmd.Parameters.Add(new SqlParameter("@INDEX_IDS", indexIds));
                cmd.Parameters.Add(new SqlParameter("@COMPOSITE_IDS", ""));
                da.SelectCommand = cmd;
                da.Fill(dsRtn);
                var lstNav = (from v in dsRtn.Tables[0].AsEnumerable()
                              select new NavReturnModelSP
                              {
                                  Id = Convert.ToDecimal(string.IsNullOrEmpty(Convert.ToString(v["ID"])) ? null : v["ID"]),
                                  Scheme_Name = Convert.ToString(v["SCHEME_NAME"]),
                                  Nav = Convert.ToDouble((string.IsNullOrEmpty(Convert.ToString(v["NAV"])) ? null : v["NAV"])),
                                  ReInvest_Nav = Convert.ToDouble((string.IsNullOrEmpty(Convert.ToString(v["REINVEST_NAV"])) ? null : v["REINVEST_NAV"])),
                                  Date = Convert.ToDateTime((string.IsNullOrEmpty(Convert.ToString(v["DATE"])) ? null : v["DATE"]))
                              }).ToList();

                navRtn.Add(lstNav);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return navRtn;
        }

        public static ChartNavReturnModel GetMFINav(decimal schemeIds, DateTime fromDate, DateTime toDate)
        {
            string indexid = "";
            using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
            {
                var indexIds = db.T_SCHEMES_INDEX_clients.Where(b => b.SCHEME_ID == schemeIds).Select(c => Convert.ToString(c.INDEX_ID));
                if (indexIds.Count() > 0)
                {
                    indexid = string.Join(",", indexIds);
                }
            }
            var navs = GetNavFrmSP(schemeIds, fromDate, toDate, indexid);
            var preModel = navs[0].Cast<NavReturnModelSP>().OrderBy(x => x.Scheme_Name).ThenBy(x => x.Date).ToList();
            var names = preModel.Select(x => x.Scheme_Name).Distinct();
            var preScheme = "";
            var firstNav = 0d;
            for (int i = 0; i < preModel.Count(); i++)
            {
                if (preModel[i].Scheme_Name != preScheme)
                {
                    if (preModel[i].Id != 0)
                    {
                        firstNav = preModel[i].ReInvest_Nav;
                        preModel[i].ReInvest_Nav = 100;
                    }
                    else
                    {
                        firstNav = preModel[i].Nav;
                        preModel[i].ReInvest_Nav = 100;
                    }
                }
                else
                {
                    if (preModel[i].Id != 0)
                    {
                        if (preModel[i - 1].ReInvest_Nav != 0)
                            preModel[i].ReInvest_Nav = Convert.ToString(((100 * preModel[i].ReInvest_Nav) / firstNav)) == "NaN" ? 0.0 : (100 * preModel[i].ReInvest_Nav) / firstNav;
                    }
                    else
                    {
                        if (preModel[i - 1].Nav != 0)
                            preModel[i].ReInvest_Nav = (100 * preModel[i].Nav) / firstNav;
                    }
                }
                preScheme = preModel[i].Scheme_Name;
            }
            var returnData = names.Select(name => new SimpleNavReturnModel
            {
                Name = name,
                ValueAndDate =
                    preModel.Where(x => x.Scheme_Name == name)
                        .Select(x => new ValueAndDate { Date = x.Date.ToString("MM/dd/yyyy"), Value = x.ReInvest_Nav })
                        .ToList()
            }).ToList();

            return new iFrames.BLL.ChartNavReturnModel
            {
                MaxDate = preModel.Select(x => x.Date).Max().ToString("MM/dd/yyyy"),
                MinDate = preModel.Select(x => x.Date).Min().ToString("MM/dd/yyyy"),
                MaxVal = preModel.Select(x => x.ReInvest_Nav).Max(),
                //MinVal = preModel.Select(x => x.ReInvest_Nav).Min(),
                SimpleNavReturnModel = returnData
            };
        }

        /// <summary>
        ///  It will returns Nav and index history with the given date range for the given scheme id and indexid respectively.
        /// </summary>
        /// <param name="schemeId"> IEnumerable of decimal schemeIds : selected schemes Ids</param>
        /// <param name="fromDate">DateTime fromDate : selected form date</param>
        /// <param name="toDate"> DateTime toDate : selected to date</param>
        /// <param name="indexId">IEnumerable of decimal indexIds : selected indexids</param>
        /// <returns></returns>
        //public static ChartNavReturnModel GetMFINav(decimal schemeId, DateTime fromDate, DateTime toDate, decimal? indexId)
        //{
        //	var indices = new List<decimal>();
        //	if (indexId != null)
        //		indices.Add(indexId.Value);
        //	return GetMFINav( schemeId , fromDate, toDate);
        //}

        public static DataTable getTopCompany(int SchemeId, int NoOfCom)
        {
            DataTable DtTopCompany = new DataTable();
            DateTime MaxPortDate = DateTime.MinValue;
            try
            {
                using (var db = new SIP_ClientDataContext() { CommandTimeout = 600 })
                {
                    var FundId = db.T_SCHEMES_MASTER_Clients.Where(n => n.Scheme_Id == SchemeId).Select(x => x.Fund_Id).FirstOrDefault();
                    var ChkPort = db.T_COM_POT_Clients.Where(v => v.FUND_ID == FundId).Select(c => c);
                    if (ChkPort.Count() > 0)
                    {
                        MaxPortDate = ChkPort.OrderByDescending(v => v.PORT_DATE).Select(v => v.PORT_DATE).FirstOrDefault();
                    }
                    else
                    {
                        return DtTopCompany;
                    }
                }
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.ConnectionString = connectionString;
                            connection.Open();
                        }

                        using (var dA = new SqlDataAdapter())
                        {
                            var cmd = new SqlCommand("MFIE_SP_PORTFOLIODETAILS", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 2000;
                            cmd.Parameters.Add(new SqlParameter("@InformationType", "DETAILEDPORTFOLIO"));
                            cmd.Parameters.Add(new SqlParameter("@SchemeId", SchemeId));
                            cmd.Parameters.Add(new SqlParameter("@PortfolioDate", MaxPortDate));
                            cmd.Parameters.Add(new SqlParameter("@NatureId", "2,4,3"));
                            cmd.Parameters.Add(new SqlParameter("@InstrumentId", ""));
                            cmd.Parameters.Add(new SqlParameter("@RatingsId", ""));
                            cmd.Parameters.Add(new SqlParameter("@SectorId", ""));
                            cmd.Parameters.Add(new SqlParameter("@TopHoldings", NoOfCom));
                            cmd.Parameters.Add(new SqlParameter("@LoginID", 0));
                            dA.SelectCommand = cmd;
                            dA.Fill(DtTopCompany);
                        }
                    }
                    catch
                    {
                        return DtTopCompany;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return DtTopCompany;
        }

        public static DataSet getPeerPerformance(int SchemeId, int TopCount = 5)
        {
            DataSet dsPeerSchemes = new DataSet();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }
                SqlCommand cmd;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("WEB_SP_PEER_RETURN", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 2000;
                cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", SchemeId));
                cmd.Parameters.Add(new SqlParameter("@RETURN_PERIOD", "Per_1_Year"));
                cmd.Parameters.Add(new SqlParameter("@TOP_RETURN_NO", TopCount.ToString()));
                da.SelectCommand = cmd;
                da.Fill(dsPeerSchemes);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return dsPeerSchemes;
        }

        public static DataSet getSebiPeerPerformance(int SchemeId, int TopCount = 5)
        {
            DataSet dsPeerSchemes = new DataSet();
            try
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.ConnectionString = connectionString;
                            connection.Open();
                        }
                        using (var dA = new SqlDataAdapter())
                        {
                            var cmd = new SqlCommand("WEB_SP_PEER_RETURN_SEBI", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 2000;
                            cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", SchemeId));
                            cmd.Parameters.Add(new SqlParameter("@RETURN_PERIOD", "Per_1_Year"));
                            cmd.Parameters.Add(new SqlParameter("@TOP_RETURN_NO", TopCount.ToString()));
                            dA.SelectCommand = cmd;
                            dA.Fill(dsPeerSchemes);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return dsPeerSchemes;
        }

        public static List<PortfolioMktValue> getPortfolioAsset(int schemeId)
        {
            #region using linq
            //using (var db = new SIP_ClientDataContext())
            //{
            //	var FundId = db.T_SCHEMES_MASTER_Clients.Where(n => n.Scheme_Id == schemeId).Select(x => x.Fund_Id).FirstOrDefault();

            //	var Last6port = (from v in db.T_COM_POT_Clients
            //					 where v.FUND_ID == FundId
            //					 orderby v.PORT_DATE descending
            //					 select v).ToList();

            //	if (Last6port.Count() > 0)
            //	{
            //		if (Last6port.Count() >= 6)
            //		{
            //			Last6port = Last6port.Take(6).ToList();
            //		}
            //	}

            //	var data = (from v in Last6port
            //				join n in db.T_COM_POT_DETAIL_Clients on v.PORT_ID equals n.PORT_ID into grp
            //				from j3 in grp
            //				group j3 by v.PORT_ID into grp1
            //				select new
            //				{
            //					id = grp1.First().PORT_ID,
            //					value = grp1.Sum(c => c.CORPUS_PER)
            //				}).ToList();

            //	return data.ToDataTable();
            //}
            #endregion
            DataTable DtPeerSchemes = new DataTable();
            try
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.ConnectionString = connectionString;
                            connection.Open();
                        }
                        using (var dA = new SqlDataAdapter())
                        {
                            var cmd = new SqlCommand("WEB_SP_FUND_CORPUS_PERFORMANCE", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 2000;
                            cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", schemeId));
                            cmd.Parameters.Add(new SqlParameter("@CNT_MONTH", 6));
                            dA.SelectCommand = cmd;
                            dA.Fill(DtPeerSchemes);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return (from v in DtPeerSchemes.AsEnumerable()
                    select new PortfolioMktValue
                    {
                        MatketValue = Convert.ToDouble(v["MKT_VALUE"]),
                        PortDate = Convert.ToString(v["PORT_DATE"])
                    }).ToList();
        }

        public static FundInvestmentInfo getInvestInfo(int schemeId)
        {
            DataTable DtInvestmentInfo = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }
                SqlCommand cmd;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("WEB_SP_FACTSHEET_INVESTMENT_INFO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 2000;
                cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", schemeId));
                da.SelectCommand = cmd;
                da.Fill(DtInvestmentInfo);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            var vv = (from v in DtInvestmentInfo.AsEnumerable()
                      select new FundInvestmentInfo
                      {
                          FundName = Convert.ToString(v["FundName"]),
                          CurrentNav = Convert.ToString(v["CurrentNav"]),
                          FundSize = Convert.ToString(v["FundSize"]),
                          PrevNav = Convert.ToString(v["PrevNav"]),
                          StructureName = Convert.ToString(v["StructureName"]),
                          MinInvestment = Convert.ToString(v["MinInvestment"]),
                          LunchDate = v["LunchDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(v["LunchDate"]),
                          FundMan = Convert.ToString(v["FundMan"]),
                          LatestDiv = Convert.ToString(v["LatestDiv"]),
                          BenchMark = Convert.ToString(v["BenchMark"]),
                          Email = Convert.ToString(v["Email"]),
                          Bonous = Convert.ToString(v["Bonous"]),
                          AmcName = Convert.ToString(v["AmcName"]),
                          EntryLoad = Convert.ToString(v["EntryLoad"]),
                          ExitLoad = Convert.ToString(v["ExitLoad"]),
                          Website = Convert.ToString(v["Website"]),
                          SchemeObject = Convert.ToString(v["SchemeObject"]),
                          InvestmentPlan = Convert.ToString(v["InvestmentPlan"]),
                          CurrentNavDate = v["CurrentNavDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(v["CurrentNavDate"]),
                      }).FirstOrDefault();

            return vv;
        }

        public static DataTable getCurrentNavInfo()
        {
            DataTable DtInvestmentInfo = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }
                SqlCommand cmd;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand("WEB_SP_CURRENT_NAV", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 2000;
                //cmd.Parameters.Add(new SqlParameter("@SCHEME_IDS", schemeIds));
                da.SelectCommand = cmd;
                da.Fill(DtInvestmentInfo);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //var vv = (from v in DtInvestmentInfo.AsEnumerable()
            //          select new
            //          {
            //              SchemeObject = Convert.ToString(v["scheme_id"]),
            //              CurrentNav = Convert.ToString(v["CurrentNav"]),
            //              PrevNav = Convert.ToString(v["PrevNav"]),
            //          }).ToDataTable();

            return DtInvestmentInfo;
        }

        /// <summary>
        /// Added new SP to get NAV_Date for bluechip Tamal
        /// </summary>
        /// <param name="schemeId"></param>
        /// <returns></returns>
        public static FundInvestmentInfoNew getInvestInfoNew(int schemeId)
        {
            DataTable DtInvestmentInfo = new DataTable();
            try
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.ConnectionString = connectionString;
                            connection.Open();
                        }
                        using (var dA = new SqlDataAdapter())
                        {
                            var cmd = new SqlCommand("WEB_SP_FACTSHEET_INVESTMENT_INFO_New", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 2000;
                            cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", schemeId));
                            dA.SelectCommand = cmd;
                            dA.Fill(DtInvestmentInfo);
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            var vv = (from v in DtInvestmentInfo.AsEnumerable()
                      select new FundInvestmentInfoNew
                      {
                          FundName = Convert.ToString(v["FundName"]),
                          CurrentNav = Convert.ToString(v["CurrentNav"]),
                          CurrentNavDate = Convert.ToString(v["CurrentNavDate"]),
                          FundSize = Convert.ToString(v["FundSize"]),
                          PrevNav = Convert.ToString(v["PrevNav"]),
                          StructureName = Convert.ToString(v["StructureName"]),
                          MinInvestment = Convert.ToString(v["MinInvestment"]),
                          LunchDate = v["LunchDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(v["LunchDate"]),
                          FundMan = Convert.ToString(v["FundMan"]),
                          LatestDiv = Convert.ToString(v["LatestDiv"]),
                          BenchMark = Convert.ToString(v["BenchMark"]),
                          Email = Convert.ToString(v["Email"]),
                          Bonous = Convert.ToString(v["Bonous"]),
                          AmcName = Convert.ToString(v["AmcName"]),
                          EntryLoad = Convert.ToString(v["EntryLoad"]),
                          ExitLoad = Convert.ToString(v["ExitLoad"]),
                          Website = Convert.ToString(v["Website"]),
                          SchemeObject = Convert.ToString(v["SchemeObject"]),
                          InvestmentPlan = Convert.ToString(v["InvestmentPlan"]),
                      }).FirstOrDefault();

            return vv;
        }
        #endregion

        #region Added by Arindam

        public static int getFundId(decimal SchemeId)
        {
            var FundId = 0;
            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    FundId = Convert.ToInt32((from nd in db.T_SCHEMES_MASTER_Clients
                                              where nd.Scheme_Id == SchemeId
                                              select nd.Fund_Id).FirstOrDefault());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FundId;
        }

        public static int getMFID(decimal SchemeId)
        {
            var FundId = 0;
            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    FundId = Convert.ToInt32((from nd in db.T_SCHEMES_MASTER_Clients
                                              join mf in db.T_FUND_MASTER_clients on
                                              nd.Fund_Id equals mf.FUND_ID
                                              where nd.Scheme_Id == SchemeId
                                              select mf.MUTUALFUND_ID).FirstOrDefault());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FundId;
        }
        public static int getSchemeId(string AMFIcode)
        {
            var SchemeId = 0;
            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    SchemeId = Convert.ToInt32((from nd in db.T_SCHEMES_MASTER_Clients
                                                where nd.Amfi_Code == AMFIcode
                                                select nd.Scheme_Id).FirstOrDefault());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SchemeId;
        }
        public static int getAMFICode(string SchemeId)
        {
            var AMFICode = 0;
            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    AMFICode = Convert.ToInt32((from nd in db.T_SCHEMES_MASTER_Clients
                                                where Convert.ToString(nd.Scheme_Id) == SchemeId
                                                select nd.Amfi_Code).FirstOrDefault());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AMFICode;
        }
        #endregion

        #region Added by Sudheer
        public static int getSchemeIdBySchShortName(string SchemeShortName)
        {
            var SchemeId = 0;
            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    SchemeId = Convert.ToInt32((from nd in db.T_SCHEMES_MASTER_Clients
                                                where nd.Sch_Short_Name == SchemeShortName
                                                select nd.Scheme_Id).FirstOrDefault());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SchemeId;
        }
        #endregion

        public static BansalCapitalMcapAvgMaturity getMCapAvgMaturity(int schId)
        {
            BansalCapitalMcapAvgMaturity ObjData = new BansalCapitalMcapAvgMaturity();
            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    var FundId = db.T_SCHEMES_MASTER_Clients.Where(x => x.Scheme_Id == schId).Select(a => a.Fund_Id).FirstOrDefault().Value;

                    var maxPortDates = db.T_COM_POT_Clients.Where(z => z.FUND_ID == FundId).OrderByDescending(q => q.PORT_DATE).Select(x => x.PORT_DATE).Take(3);

                    var styleBoxNature = db.T_STYLEBOX_DETAILs.Where(x => x.FUND_ID == FundId)
                        .OrderByDescending(q => q.PORT_DATE).Select(w => w.NATURE_ID);

                    DataTable dsMcap = new DataTable();

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.ConnectionString = connstr;
                        conn.Open();
                    }
                    SqlCommand cmd;
                    SqlDataAdapter da = new SqlDataAdapter();
                    DateTime MaxPortData = maxPortDates.FirstOrDefault();
                    if (styleBoxNature.Any())
                    {
                        if (styleBoxNature.FirstOrDefault().Value == db.T_SCHEMES_NATURE_Clients.
                            Where(x => x.Nature.Contains("Equity")).Select(q => q.Nature_ID).FirstOrDefault())
                        {
                            cmd = new SqlCommand("MFIE_SP_EQUITY_STOCK_HOLDING", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 2000;
                            cmd.Parameters.Add(new SqlParameter("@INFOTYPE", "MARKET_CAP"));
                            cmd.Parameters.Add(new SqlParameter("@IDS", schId.ToString()));
                            cmd.Parameters.Add(new SqlParameter("@PORTFOLIODATE", maxPortDates.FirstOrDefault().ToString("dd MMM yyyy")));
                            cmd.Parameters.Add(new SqlParameter("@FILTERTYPE", "0"));
                            cmd.Parameters.Add(new SqlParameter("@COUNTER", 0));
                            cmd.Parameters.Add(new SqlParameter("@GRBYCONDITION", ""));
                            cmd.Parameters.Add(new SqlParameter("@NEWCOMPANY", ""));
                            cmd.Parameters.Add(new SqlParameter("@EQUITYINSTRUMENT", ""));
                            cmd.Parameters.Add(new SqlParameter("@ISDICTINCT", 0));
                            da.SelectCommand = cmd;
                            da.Fill(dsMcap);

                            var lstMcap = (from v in dsMcap.AsEnumerable()
                                           select new MarketCap
                                           {
                                               Port_Date = Convert.ToDateTime(v["CALC_DATE"]),
                                               Scheme_Short_Name = Convert.ToString(v["SCH_SHORT_NAME"]),
                                               Market_Slap = Convert.ToString(v["MARKET_SLAB"]),
                                               M_Cap = Convert.ToDouble(v["MCAPALLOCATION"]),
                                           }).ToList();

                            ObjData.LstMarketCap = lstMcap;
                            ObjData.IsMCap = true;
                            ObjData.isSchemeEquity = true;
                        }
                        else
                        {
                            var dataes = maxPortDates.ToArray().Select(c => c.ToString("dd/MMM/yyyy"));
                            cmd = new SqlCommand("WEB_SP_AVERAGE_MATURITY", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 2000;
                            cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", schId.ToString()));
                            cmd.Parameters.Add(new SqlParameter("@PORT_DATE", string.Join("'',''", dataes)));
                            da.SelectCommand = cmd;
                            da.Fill(dsMcap);

                            var lstAvgMaturity = (from v in dsMcap.AsEnumerable()
                                                  select new AverageMaturity
                                                  {
                                                      Port_Date = Convert.ToDateTime(v["Port_Date"]),
                                                      Scheme_Name = Convert.ToString(v["Scheme_Name"]),
                                                      Average_Maturity = Convert.ToDouble(v["Average_Maturity"]),
                                                      MonthYear = Convert.ToString(v["MonthYear"]),
                                                  }).ToList();

                            ObjData.LstAverageMaturity = lstAvgMaturity;
                            ObjData.IsMCap = false;
                            ObjData.isSchemeEquity = false;
                        }
                        var LastDate = new DateTime(MaxPortData.Year, MaxPortData.Month, DateTime.DaysInMonth(MaxPortData.Year, MaxPortData.Month));
                        var Firstdate = new DateTime(MaxPortData.Year, MaxPortData.Month, 1);
                        var StyleBoximgName = db.T_STYLEBOX_DETAILs.Where(x => x.FUND_ID == FundId)
                            .Where(x => (x.PORT_DATE.Value >= Firstdate) && (x.PORT_DATE.Value <= LastDate))
                            .OrderByDescending(q => q.PORT_DATE).Select(w => w.IMAGE_Name);

                        if (StyleBoximgName.Any())
                        {
                            ObjData.StyleBoxImgName = StyleBoximgName.FirstOrDefault();
                            ObjData.isStyleBoxExist = true;
                        }
                    }
                }
                return ObjData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BansalCapitalMcapAvgMaturity getMCapAvgMaturityWoutRebase(int schId)
        {
            BansalCapitalMcapAvgMaturity ObjData = new BansalCapitalMcapAvgMaturity();
            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    var FundId = db.T_SCHEMES_MASTER_Clients.Where(x => x.Scheme_Id == schId).Select(a => a.Fund_Id).FirstOrDefault().Value;

                    var maxPortDates = db.T_COM_POT_Clients.Where(z => z.FUND_ID == FundId).OrderByDescending(q => q.PORT_DATE).Select(x => x.PORT_DATE).Take(3);

                    var styleBoxNature = db.T_STYLEBOX_DETAILs.Where(x => x.FUND_ID == FundId)
                        .OrderByDescending(q => q.PORT_DATE).Select(w => w.NATURE_ID);

                    DataTable dsMcap = new DataTable();

                    DateTime MaxPortData = maxPortDates.FirstOrDefault();
                    if (styleBoxNature.Any())
                    {
                        if (styleBoxNature.FirstOrDefault().Value == db.T_SCHEMES_NATURE_Clients.
                            Where(x => x.Nature.Contains("Equity")).Select(q => q.Nature_ID).FirstOrDefault())
                        {
                            var mcapData = db.T_MARKET_CAP_CLASSIFICATION_DETAIL_Clients.Where(x => x.CALC_DATE.Month == MaxPortData.Month && x.CALC_DATE.Year == MaxPortData.Year).Where(x => x.FUND_ID == FundId).Select(x => x);
                            dsMcap = mcapData.ToDataTable();
                            if (mcapData.Where(x => x.MCAPALLOCATION != null).Any())
                            {
                                var SUM = mcapData.Where(x => x.MCAPALLOCATION != null).Sum(x => x.MCAPALLOCATION.Value);
                                SUM = Math.Round((100 - SUM), 2);

                                if (SUM != 100 && SUM != 0)
                                {
                                    var drMcap = dsMcap.NewRow();
                                    drMcap["FUND_ID"] = FundId;
                                    drMcap["CALC_DATE"] = MaxPortData;
                                    drMcap["MARKET_SLAB"] = "Debt & Others";
                                    drMcap["MCAPALLOCATION"] = SUM;
                                    dsMcap.Rows.Add(drMcap);
                                }
                            }

                            var lstMcap = (from v in dsMcap.AsEnumerable()
                                           select new MarketCap
                                           {
                                               Port_Date = Convert.ToDateTime(v["CALC_DATE"]),
                                               Scheme_Short_Name = "",
                                               Market_Slap = Convert.ToString(v["MARKET_SLAB"]),
                                               M_Cap = Convert.ToDouble(v["MCAPALLOCATION"]),
                                           }).ToList();

                            ObjData.LstMarketCap = lstMcap;
                            ObjData.IsMCap = true;
                            ObjData.isSchemeEquity = true;
                        }
                        else
                        {
                            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                try
                                {
                                    if (connection.State == ConnectionState.Closed)
                                    {
                                        connection.ConnectionString = connectionString;
                                        connection.Open();
                                    }
                                    using (var dA = new SqlDataAdapter())
                                    {
                                        var dataes = maxPortDates.ToArray().Select(c => c.ToString("dd/MMM/yyyy"));
                                        //var cmd = new SqlCommand("WEB_SP_AVERAGE_MATURITY", connection);
                                        var cmd = new SqlCommand("WEB_SP_AVERAGE_MATURITY_IFRAME", connection);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandTimeout = 2000;
                                        cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", schId.ToString()));
                                        cmd.Parameters.Add(new SqlParameter("@PORT_DATE", string.Join(",", dataes)));
                                        dA.SelectCommand = cmd;
                                        dA.Fill(dsMcap);

                                        var lstAvgMaturity = (from v in dsMcap.AsEnumerable()
                                                              select new AverageMaturity
                                                              {
                                                                  Port_Date = Convert.ToDateTime(v["PORT_DATE"]),
                                                                  Scheme_Name = Convert.ToString(v["Scheme_Name"]),
                                                                  Average_Maturity = Convert.ToDouble(v["Average_Maturity"]),
                                                                  MonthYear = Convert.ToString(v["MonthYear"]),
                                                              }).ToList();

                                        ObjData.LstAverageMaturity = lstAvgMaturity;
                                        ObjData.IsMCap = false;
                                        ObjData.isSchemeEquity = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                        var LastDate = new DateTime(MaxPortData.Year, MaxPortData.Month, DateTime.DaysInMonth(MaxPortData.Year, MaxPortData.Month));
                        var Firstdate = new DateTime(MaxPortData.Year, MaxPortData.Month, 1);
                        var StyleBoximgName = db.T_STYLEBOX_DETAILs.Where(x => x.FUND_ID == FundId)
                            .Where(x => (x.PORT_DATE.Value >= Firstdate) && (x.PORT_DATE.Value <= LastDate))
                            .OrderByDescending(q => q.PORT_DATE).Select(w => w.IMAGE_Name);

                        if (StyleBoximgName.Any())
                        {
                            ObjData.StyleBoxImgName = StyleBoximgName.FirstOrDefault();
                            ObjData.isStyleBoxExist = true;
                        }
                    }
                }
                return ObjData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Top5Sector> getTopSector(int schId)
        {
            try
            {
                using (var db = new SIP_ClientDataContext())
                {
                    var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.ConnectionString = connectionString;
                                connection.Open();
                            }

                            using (var dA = new SqlDataAdapter())
                            {
                                DataTable DtTopSector = new DataTable();
                                var cmd = new SqlCommand("WEB_SP_TOP_SECTOR", connection);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 2000;
                                cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", schId.ToString()));
                                dA.SelectCommand = cmd;
                                dA.Fill(DtTopSector);

                                var datalist = (from v in DtTopSector.AsEnumerable()
                                                select new Top5Sector
                                                {
                                                    Port_Date = Convert.ToDateTime(v["Port_Date"]),
                                                    Scheme_Name = Convert.ToString(v["Scheme_Name"]),
                                                    Sector_Name = Convert.ToString(v["Sector_Name"]),
                                                    Scheme = Convert.ToDouble(v["Scheme"]),
                                                }).ToList();

                                if (datalist.Count() > 4)
                                {
                                    return datalist.Take(5).ToList();
                                }
                                else
                                {
                                    return datalist;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable getSip4MutualfundWala(int schId)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = connstr;
                    conn.Open();
                }
                SqlCommand cmd;
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable DbRet = new DataTable();
                cmd = new SqlCommand("USP_RETURN_SIP_MUTUALFUNDWALA", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 2000;
                cmd.Parameters.Add(new SqlParameter("@P_SCHEME_ID", schId.ToString()));
                da.SelectCommand = cmd;
                da.Fill(DbRet);

                return DbRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ExpenseRatio> getExpenseRatio4DSPApp(int SchemeId)
        {
            try
            {
                DataTable dtExpanseRation = new DataTable();

                using (var SipContext = new SIP_ClientDataContext())
                {
                    var Expense = (from x in SipContext.T_SCHEME_EXPENSE_Clients
                                   where x.SCHEME_ID == SchemeId
                                   orderby x.DATE_TO descending
                                   select x.DATE_TO.Value).Distinct();

                    if (Expense.Count() >= 5)
                    {
                        Expense = Expense.OrderByDescending(a => a).Take(5);
                    }
                    var StrDate = string.Join(",", Expense.ToArray().Select(x => x.ToString("dd/MMM/yyyy")));

                    //var StrDate = "31-May-2016,30-Apr-2016,30-Apr-2016,30-Mar-2016,29-Feb-2016";

                    SqlCommand cmd;
                    SqlDataAdapter da = new SqlDataAdapter();



                    cmd = new SqlCommand("WEB_SP_EXPENSE_RATIO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 2000;
                    cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", SchemeId));
                    cmd.Parameters.Add(new SqlParameter("@PORT_DATES", StrDate));

                    da.SelectCommand = cmd;
                    da.Fill(dtExpanseRation);

                    if (dtExpanseRation.Rows.Count > 0)
                    {
                        // var test0 = Convert.ToString(dtExpanseRation.Rows[0]["Expense_Date"]);
                        var data = (from v in dtExpanseRation.AsEnumerable()
                                    select new ExpenseRatio
                                    {
                                        StrValue = Convert.ToString(v["Actual_Percentage"]),
                                        Date = Convert.ToDateTime(v["Expense_Date"]),

                                        //Date = string.IsNullOrEmpty(Convert.ToString(v["Expense_Date"])) ?
                                        //"" : Convert.ToDateTime(v["Expense_Date"]).ToString("MMM yyyy"),
                                        //value = string.IsNullOrEmpty(Convert.ToString(v["Actual_Percentage"])) ?
                                        //0.0 : Convert.ToDouble(v["Actual_Percentage"])
                                    });
                        if (data.Any())
                        {
                            return data.ToList();
                        }
                    }
                    //var test = new List<ExpenseRatio>();
                    //test.Add(new ExpenseRatio() { value = 2.0, Date = "3 Mar 2016" });
                    //test.Add(new ExpenseRatio() { value = 3.0, Date = "3 Apr 2016" });
                    //return test;

                }
            }
            catch (Exception exp)
            {
            }
            finally
            {
            }
            return null;
        }


        public static CreditRatingInsBreakup getCreditRatingInsBreakup(int SchemeId)
        {
            try
            {
                DataTable dtCreditRating = new DataTable();

                using (var SipContext = new SIP_ClientDataContext())
                {
                    var FundId = SipContext.T_SCHEMES_MASTER_Clients.Where(x => x.Scheme_Id == SchemeId).FirstOrDefault().Fund_Id;
                    var Expense = from x in SipContext.T_COM_POT_Clients
                                  where x.FUND_ID == FundId
                                  orderby x.PORT_DATE descending
                                  select x.PORT_DATE;

                    if (Expense.Any())
                    {
                        var FinalData = new CreditRatingInsBreakup();
                        var StrDate = Expense.FirstOrDefault();

                        SqlCommand cmd;
                        SqlDataAdapter da = new SqlDataAdapter();

                        cmd = new SqlCommand("WEB_SP_PORTFOLIO_CUST_RATING", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 2000;
                        cmd.Parameters.Add(new SqlParameter("@SchemeId", SchemeId));
                        cmd.Parameters.Add(new SqlParameter("@PortfolioDate", StrDate.ToString("dd/MMM/yyyy")));
                        da.SelectCommand = cmd;
                        da.Fill(dtCreditRating);

                        var data1 = (from v in dtCreditRating.AsEnumerable()
                                     select new BasicData
                                     {
                                         DataHead = Convert.ToString(v["Rating_Name"]),
                                         DataActual = Convert.ToDouble(v["Net_Asset"])
                                     });
                        if (data1.Any())
                            FinalData.LstCreditrating = data1.ToArray();


                        //---------------ins break up

                        SqlCommand cmd2;
                        SqlDataAdapter da2 = new SqlDataAdapter();

                        cmd2 = new SqlCommand("MFIE_SP_PORTFOLIODETAILS", conn);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandTimeout = 2000;
                        cmd2.Parameters.Add(new SqlParameter("@InformationType", "instrumentAllocation"));
                        cmd2.Parameters.Add(new SqlParameter("@SchemeId", SchemeId));
                        cmd2.Parameters.Add(new SqlParameter("@PortfolioDate", StrDate.ToString("dd/MMM/yyyy")));
                        cmd2.Parameters.Add(new SqlParameter("@NatureId", ""));
                        cmd2.Parameters.Add(new SqlParameter("@InstrumentId", ""));
                        cmd2.Parameters.Add(new SqlParameter("@RatingsId", ""));
                        cmd2.Parameters.Add(new SqlParameter("@SectorId", ""));
                        cmd2.Parameters.Add(new SqlParameter("@TopHoldings", 0));
                        cmd2.Parameters.Add(new SqlParameter("@LoginID", 0));

                        da2.SelectCommand = cmd2;
                        var dtIns = new DataTable();
                        da2.Fill(dtIns);

                        var data2 = (from d in dtIns.AsEnumerable()
                                     select new BasicData
                                     {
                                         DataActual = Convert.ToDouble(d["Net_Asset"]),
                                         DataHead = Convert.ToString(d["Instrumet_Name"])
                                     });
                        if (data2.Any())
                            FinalData.LstInsBreakup = data2.ToArray();

                        return FinalData;
                    }

                }
            }
            catch (Exception exp)
            {
            }
            finally
            {
            }
            return null;
        }

        public static CreditRatingInsBreakup getCreditRatingInsBreakupBluechip(int SchemeId)
        {
            try
            {
                DataTable dtCreditRating = new DataTable();

                using (var SipContext = new SIP_ClientDataContext())
                {
                    var FundId = SipContext.T_SCHEMES_MASTER_Clients.Where(x => x.Scheme_Id == SchemeId).FirstOrDefault().Fund_Id;
                    var Expense = from x in SipContext.T_COM_POT_Clients
                                  where x.FUND_ID == FundId
                                  orderby x.PORT_DATE descending
                                  select x.PORT_DATE;

                    if (Expense.Any())
                    {
                        var FinalData = new CreditRatingInsBreakup();
                        var StrDate = Expense.FirstOrDefault();

                        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                if (connection.State == ConnectionState.Closed)
                                {
                                    connection.ConnectionString = connectionString;
                                    connection.Open();
                                }
                                using (var dA = new SqlDataAdapter())
                                {
                                    var cmd = new SqlCommand("MFIE_SP_PORTFOLIODETAILS", connection);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandTimeout = 2000;
                                    cmd.Parameters.Add(new SqlParameter("@InformationType", "RatingAllocation"));
                                    cmd.Parameters.Add(new SqlParameter("@SchemeId", SchemeId));
                                    cmd.Parameters.Add(new SqlParameter("@PortfolioDate", StrDate.ToString("dd/MMM/yyyy")));
                                    cmd.Parameters.Add(new SqlParameter("@NatureId", ""));
                                    cmd.Parameters.Add(new SqlParameter("@InstrumentId", ""));
                                    cmd.Parameters.Add(new SqlParameter("@RatingsId", ""));
                                    cmd.Parameters.Add(new SqlParameter("@SectorId", ""));
                                    cmd.Parameters.Add(new SqlParameter("@TopHoldings", 0));
                                    cmd.Parameters.Add(new SqlParameter("@LoginID", 0));

                                    dA.SelectCommand = cmd;
                                    dA.Fill(dtCreditRating);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        var data1 = (from v in dtCreditRating.AsEnumerable()
                                     select new BasicData
                                     {
                                         DataHead = Convert.ToString(v["Rating_Name"]),
                                         DataActual = Convert.ToDouble(v["Net_Asset"])
                                     });
                        if (data1.Any())
                            FinalData.LstCreditrating = data1.ToArray();


                        //---------------ins break up
                        var dtIns = new DataTable();
                        using (SqlConnection connection2 = new SqlConnection(connectionString))
                        {
                            try
                            {
                                if (connection2.State == ConnectionState.Closed)
                                {
                                    connection2.ConnectionString = connectionString;
                                    connection2.Open();
                                }
                                using (var dA2 = new SqlDataAdapter())
                                {
                                    var cmd2 = new SqlCommand("MFIE_SP_PORTFOLIODETAILS", connection2);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.CommandTimeout = 2000;
                                    cmd2.Parameters.Add(new SqlParameter("@InformationType", "instrumentAllocation"));
                                    cmd2.Parameters.Add(new SqlParameter("@SchemeId", SchemeId));
                                    cmd2.Parameters.Add(new SqlParameter("@PortfolioDate", StrDate.ToString("dd/MMM/yyyy")));
                                    cmd2.Parameters.Add(new SqlParameter("@NatureId", ""));
                                    cmd2.Parameters.Add(new SqlParameter("@InstrumentId", ""));
                                    cmd2.Parameters.Add(new SqlParameter("@RatingsId", ""));
                                    cmd2.Parameters.Add(new SqlParameter("@SectorId", ""));
                                    cmd2.Parameters.Add(new SqlParameter("@TopHoldings", 0));
                                    cmd2.Parameters.Add(new SqlParameter("@LoginID", 0));

                                    dA2.SelectCommand = cmd2;
                                    dA2.Fill(dtIns);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        var data2 = (from d in dtIns.AsEnumerable()
                                     select new BasicData
                                     {
                                         DataActual = Convert.ToDouble(d["Net_Asset"]),
                                         DataHead = Convert.ToString(d["Instrumet_Name"])
                                     });
                        if (data2.Any())
                            FinalData.LstInsBreakup = data2.ToArray();

                        return FinalData;
                    }

                }
            }
            catch (Exception exp)
            {
            }
            finally
            {
            }
            return null;
        }


        public static PortfolioOthers getPortfolioOthers(int SchemeId)
        {
            try
            {
                DataSet dsPortfolioOthers = new DataSet();

                using (var SipContext = new SIP_ClientDataContext())
                {
                    var FundId = SipContext.T_SCHEMES_MASTER_Clients.Where(x => x.Scheme_Id == SchemeId).FirstOrDefault().Fund_Id;
                    var PortDate = from x in SipContext.T_COM_POT_Clients
                                   where x.FUND_ID == FundId
                                   orderby x.PORT_DATE descending
                                   select new { x.PORT_DATE, x.YTM };

                    if (PortDate.Any())
                    {
                        var FinalData = new CreditRatingInsBreakup();
                        var StrDate = PortDate.FirstOrDefault().PORT_DATE;

                        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                if (connection.State == ConnectionState.Closed)
                                {
                                    connection.ConnectionString = connectionString;
                                    connection.Open();
                                }
                                using (var dA = new SqlDataAdapter())
                                {
                                    var cmd = new SqlCommand("WEB_SP_PORTFOLIO_OTHERS_PARAMETER", connection);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandTimeout = 2000;
                                    cmd.Parameters.Add(new SqlParameter("@SCHEME_ID", SchemeId));
                                    cmd.Parameters.Add(new SqlParameter("@PORT_DATE", StrDate.ToString("dd/MMM/yyyy")));
                                    dA.SelectCommand = cmd;
                                    dA.Fill(dsPortfolioOthers);

                                    DataTable DtPtr = dsPortfolioOthers.Tables[0];
                                    DataTable DtAttAna = dsPortfolioOthers.Tables[1];
                                    var ptrData = (from c in DtPtr.AsEnumerable()
                                                   select new PTR
                                                   {
                                                       RATIO = Convert.ToDouble(c["RATIO"]),
                                                       IMPORT_DATE = Convert.ToString(c["IMPORT_DATE"])
                                                   }).FirstOrDefault();

                                    var AttData = (from c in DtAttAna.AsEnumerable()
                                                   select new AttributionAnalysis
                                                   {
                                                       PRICE_EARNING = Convert.ToDouble(c["PRICE_EARNING"]),
                                                       PRICE_TO_BOOKVAL = Convert.ToDouble(c["PRICE_TO_BOOKVAL"]),
                                                       PORT_DATE = Convert.ToDateTime(c["PORT_DATE"])
                                                   }).FirstOrDefault();

                                    YTM ytm = new YTM();
                                    //ytm.Scheme_Id = Convert.ToInt32(id);
                                    //ytm.Scheme_Name = selectedScheme.ShortName;
                                    ytm.Port_Date = StrDate;
                                    ytm.YTM_Value = PortDate.FirstOrDefault().YTM.HasValue ? PortDate.FirstOrDefault().YTM.Value : 0.00;


                                    PortfolioOthers ObjPortfolioOthers = new PortfolioOthers() { Ptr = ptrData, AttributionAnalysis = AttData, Ytm = ytm };
                                    return ObjPortfolioOthers;
                                }
                            }
                            catch (Exception ex)
                            {
                                return null;
                            }
                        }

                    }

                }
            }
            catch (Exception exp)
            {
            }
            finally
            {
            }
            return null;
        }

        /// <summary>
        /// Find A value That Falls On A Range and Return The Corresponding Value
        /// </summary>
        /// <param name="RangeData"></param>
        /// <param name="ValueFind"></param>
        /// <returns></returns>
        //public static object GetRangeData(this IDictionary<decimal, object> RangeData, decimal ValueFind)
        //{
        //    var OrderedItem = RangeData.OrderBy(p => p.Key);
        //    for (int i = 0; i < OrderedItem.Count(); i++)
        //    {

        //    }
        //}

        /// <summary>
        /// Lump sum Calculation, Dividend Payout
        /// </summary>
        /// <param name="SchemeID"></param>
        /// <param name="InitialInvestment"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="IsCalculate">Whether Dividend Payout Calculation To Be Done or Not</param>
        /// <param name="TotalDividendPayoutAmt">Total Payout Amount</param>
        /// <returns></returns>
        public static DataTable DividendPayoutCalcSundaram(decimal SchemeID, double InitialInvestment, DateTime FromDate, DateTime ToDate, bool IsCalculate, out double? TotalDividendPayoutAmt,bool IsDivPerUnit=false)
        {
            TotalDividendPayoutAmt = default(double?);
            using (var db = new PrincipalCalcDataContext())
            {
                try
                {
                    var Data = db.T_NAV_DIVs
                            .Where(tbl => tbl.Scheme_Id == SchemeID && tbl.Nav_Date <= FromDate.Date)
                            .OrderByDescending(tbl => tbl.Nav_Date).FirstOrDefault();

                    // If No Nav Available On Or Before The Selected Scheme Then Check For First Occurrence On Future Date
                    if (Data == null)
                        Data = db.T_NAV_DIVs
                            .Where(tbl => tbl.Scheme_Id == SchemeID && tbl.Nav_Date >= FromDate.Date)
                            .OrderBy(tbl => tbl.Nav_Date).FirstOrDefault();

                    if (Data != null)
                    {
                        if (IsCalculate)
                        {
                            // Calc Initial Unit
                            var divUnit = InitialInvestment / Data.Nav.Value;

                            var result = (from tbl in db.T_SCHEMES_MASTERs
                                          join tbl2 in db.T_NAV_DIVs on tbl.Scheme_Id equals tbl2.Scheme_Id
                                          where tbl.Scheme_Id == SchemeID && tbl2.Nav_Date >= FromDate.Date && tbl2.Nav_Date <= ToDate.Date
                                          && tbl2.Div_Flag.Value == true
                                          select new
                                          {
                                              Nav_Date = tbl2.Nav_Date,
                                              Nav = tbl2.Nav,
                                              Dividend = !IsDivPerUnit ? tbl2.Div_Ind : ((tbl.Face_Value * tbl2.Div_Ind) / 100),
                                              Bonus = tbl2.Bon_Ind,
                                              Payout_Amount = ((divUnit * tbl.Face_Value * tbl2.Div_Ind) / 100)
                                          }).ToArray();

                            //+ CalculateBonus(divUnit, tbl2.Bon_Ind, tbl2.Nav.Value)
                            var ResultWithBonus = (from tbl in result
                                                   select new
                                                   {
                                                       Nav_Date = tbl.Nav_Date,
                                                       Nav = tbl.Nav,
                                                       Dividend = tbl.Dividend,
                                                       Bonus = tbl.Bonus,
                                                       Payout_Amount = tbl.Payout_Amount.Value + CalculateBonus(divUnit, tbl.Bonus, tbl.Nav.Value)
                                                   });

                            if (ResultWithBonus.Any())
                            {
                                TotalDividendPayoutAmt = Math.Round(ResultWithBonus.Select(p => new { div_amt = p.Payout_Amount }.div_amt).Sum(), 2);
                                return ResultWithBonus.ToDataTable();
                            }
                        }
                        else
                        {
                            var result = (from tbl in db.T_SCHEMES_MASTERs
                                          join tbl2 in db.T_NAV_DIVs on tbl.Scheme_Id equals tbl2.Scheme_Id
                                          where tbl.Scheme_Id == SchemeID && tbl2.Nav_Date >= FromDate.Date && tbl2.Nav_Date <= ToDate.Date
                                          && tbl2.Div_Flag.Value == true
                                          select new
                                          {
                                              Nav_Date = tbl2.Nav_Date,
                                              Nav = tbl2.Nav,
                                              Dividend = !IsDivPerUnit ? tbl2.Div_Ind : ((tbl.Face_Value * tbl2.Div_Ind) / 100),
                                              Bonus = tbl2.Bon_Ind,
                                              Payout_Amount = default(double?)
                                          }).ToArray();
                            return result.ToDataTable();
                        }
                        return null;
                    }
                    return null;

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        private static double CalculateBonus(double DivUnit, string Bonus, double Nav)
        {
            if (Bonus != string.Empty && Bonus != null)
            {
                object[] ratios = Bonus.Split(':');
                if (ratios.Length > 0)
                {
                    var ActualUnit = DivUnit * (Convert.ToDouble(ratios[0]) / Convert.ToDouble(ratios[1]));
                    return ActualUnit * Nav;
                }
                return 0;
            }
            return 0;
        }

        public static DataTable getTataSchemes(int Category, int Option)
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = null;
            try
            {
                using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
                {
                    var _data = (
                                  from si in dc.T_SCHEMES_MASTER_Clients
                                  join fm in dc.T_FUND_MASTER_clients on si.Fund_Id equals fm.FUND_ID
                                  join sn in dc.T_SCHEMES_NATURE_Clients on fm.NATURE_ID equals sn.Nature_ID
                                  join op in dc.T_SCHEMES_OPTION_Clients on si.Option_Id equals op.Option_ID
                                  join sd in dc.T_SCHEMES_DETAILs on si.Scheme_Id equals sd.Scheme_ID
                                  where

                                  si.Nav_Check == 3
                                  //&& si.Sub_Nature2_Id != 46
                                  && si.Sub_Nature2_Id != 10
                                  && fm.MUTUALFUND_ID == 31
                                  select new
                                  {
                                      si = si,
                                      fm = fm,
                                      sn = sn,
                                      op = op,
                                      sd = sd
                                  });


                    if (Option != -1)
                    {
                        _data = _data.Where(x => x.op.Option_ID == Convert.ToDecimal(Option));
                    }

                    if (Category != -1)
                    {
                        _data = _data.Where(x => x.sn.Nature_ID == Convert.ToDecimal(Category));
                    }

                    if (_data.Any())
                    {
                        var finaldata = (from c in _data
                                         select new
                                         {
                                             SchemeName = c.si.Scheme_Name,
                                             SchemeId = c.si.Scheme_Id,
                                         }).OrderBy(x => x.SchemeName).ToArray();

                        dtResult = finaldata.ToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;
        }

        //public static DataTable GetTataSchemeSEBIReturn(decimal schemeIds, string indexIds, DateTime RtnDate)
        //{
        //    string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
        //    DataTable dtResult = new DataTable();
        //    try
        //    {
        //        using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
        //        {
        //            var SchemeIndex = (from x in dc.T_SCHEMES_BENCHMARK_ADDBENCHMARKs
        //                               where x.Scheme_Id == schemeIds
        //                               select new
        //                               {
        //                                   Scheme_Id = x.Scheme_Id,
        //                                   Benchmark_Id = x.Benchmark_Id,
        //                                   Additional_Benchmark_Id = x.Additional_Benchmark
        //                               });
        //            if (SchemeIndex.Any())
        //            {
        //                var SchemeIndexRelation = SchemeIndex.FirstOrDefault();

        //                var ReturnAll = (from x in dc.T_SEBI_Returns
        //                                 where x.Scheme_Index_Id == SchemeIndexRelation.Scheme_Id || x.Scheme_Index_Id == SchemeIndexRelation.Benchmark_Id
        //                                 || x.Scheme_Index_Id == SchemeIndexRelation.Additional_Benchmark_Id && x.Return_Date == RtnDate
        //                                 select new ClassRepository
        //                                 {
        //                                     Year = x.Particulars,
        //                                     Scheme_Index_Id = x.Scheme_Index_Id,
        //                                     Scheme_Index_name = x.Scheme_Index_name,
        //                                     Total_Amount_Invest = x.Total_Amount_Invest,
        //                                     Scheme_Return = x.Return_Value,
        //                                     Scheme_Market_Value = x.Market_value,
        //                                     Inception_Date = x.Inception_Date
        //                                 });

        //                if (ReturnAll.Any())
        //                {
        //                    var SchemeReturnData = ReturnAll.Where(m => m.Scheme_Index_Id == SchemeIndexRelation.Scheme_Id).ToList();
        //                    //return SchemeReturnData;

        //                    dtResult.Columns.Add("Scheme_Name");
        //                    foreach (var z in SchemeReturnData.Select(b => b.Year))
        //                    {
        //                        dtResult.Columns.Add(z + "_Rtn");
        //                        dtResult.Columns.Add(z + "_Amt");
        //                    }
        //                    dtResult.Columns.Add("Inception_Date");

        //                    //-------Scheme
        //                    DataRow DrScheme = dtResult.NewRow();
        //                    DrScheme["Scheme_Name"] = SchemeReturnData.FirstOrDefault().Scheme_Index_name;
        //                    foreach (var z in SchemeReturnData.Select(b => b.Year))
        //                    {
        //                        DrScheme[z + "_Rtn"] = SchemeReturnData.Where(x => x.Year == z).FirstOrDefault().Scheme_Return;
        //                        DrScheme[z + "_Amt"] = SchemeReturnData.Where(x => x.Year == z).FirstOrDefault().Scheme_Market_Value;
        //                    }
        //                    DrScheme["Inception_Date"] = SchemeReturnData.Where(x => x.Inception_Date.HasValue).FirstOrDefault().Inception_Date;
        //                    dtResult.Rows.Add(DrScheme);

        //                    //------Index                            
        //                    var IndexReturnData = ReturnAll.Where(m => m.Scheme_Index_Id == SchemeIndexRelation.Benchmark_Id);
        //                    if (IndexReturnData.Any())
        //                    {
        //                        DataRow DrIndex = dtResult.NewRow();
        //                        DrIndex["Scheme_Name"] = IndexReturnData.FirstOrDefault().Scheme_Index_name;
        //                        foreach (var z in IndexReturnData.Select(b => b.Year))
        //                        {
        //                            DrIndex[z + "_Rtn"] = IndexReturnData.Where(x => x.Year == z).FirstOrDefault().Scheme_Return;
        //                            DrIndex[z + "_Amt"] = IndexReturnData.Where(x => x.Year == z).FirstOrDefault().Scheme_Market_Value;
        //                        }
        //                        dtResult.Rows.Add(DrIndex);
        //                    }
        //                    //------Add Index                            
        //                    var AddIndexReturnData = ReturnAll.Where(m => m.Scheme_Index_Id == SchemeIndexRelation.Additional_Benchmark_Id);
        //                    if (AddIndexReturnData.Any())
        //                    {
        //                        DataRow DrAddIndex = dtResult.NewRow();
        //                        DrAddIndex["Scheme_Name"] = AddIndexReturnData.FirstOrDefault().Scheme_Index_name;
        //                        foreach (var z in AddIndexReturnData.Select(b => b.Year))
        //                        {
        //                            DrAddIndex[z + "_Rtn"] = AddIndexReturnData.Where(x => x.Year == z).FirstOrDefault().Scheme_Return;
        //                            DrAddIndex[z + "_Amt"] = AddIndexReturnData.Where(x => x.Year == z).FirstOrDefault().Scheme_Market_Value;
        //                        }
        //                        dtResult.Rows.Add(DrAddIndex);
        //                    }

        //                    return dtResult;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return null;
        //}

        //public static DataTable GetTataSchemeSEBIReturn(decimal schemeIds, string indexIds, DateTime RtnDate)
        //{
        //    string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
        //    DataTable dtResult = new DataTable();
        //    try
        //    {
        //        using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
        //        {
        //            var SchemeIndex = (from x in dc.T_SCHEMES_BENCHMARK_ADDBENCHMARKs
        //                               where x.Scheme_Id == schemeIds
        //                               select new
        //                               {
        //                                   Scheme_Id = x.Scheme_Id,
        //                                   Benchmark_Id = x.Benchmark_Id,
        //                                   Additional_Benchmark_Id = x.Additional_Benchmark
        //                               });
        //            if (SchemeIndex.Any())
        //            {
        //                var SchemeIndexRelation = SchemeIndex.FirstOrDefault();

        //                var ReturnAll = (from x in dc.T_SEBI_Returns
        //                                 where x.Scheme_ID == SchemeIndexRelation.Scheme_Id
        //                                 select new
        //                                 {
        //                                     Particulars = x.Particulars,
        //                                     Return_Date = x.Return_Date,
        //                                     Scheme_Id = x.Scheme_ID,
        //                                     Scheme_Name = x.Scheme_name,
        //                                     Total_Amount_Invest = x.Total_Amount_Invest,
        //                                     Scheme_Return = x.Scheme_Return,
        //                                     Scheme_Market_Value = x.Scheme_Market_value,
        //                                     Index_Id = x.Index_ID,
        //                                     Index_Name = x.Index_Name,
        //                                     Bechmark_Return = x.Bechmark_return,
        //                                     Benchmark_Market_Value = x.Benchmark_Market_value,
        //                                     Additional_Index_Id = x.Additional_Index_ID,
        //                                     Additional_Index_Name = x.Additional_Index_Name,
        //                                     Additional_Bechmark_Return = x.Additional_Bechmark_return,
        //                                     Additional_Benchmark_Market_Value = x.Additional_Benchmark_Market_value,
        //                                     Inception_Date = x.Inception_Date
        //                                 });

        //                if (ReturnAll.Any())
        //                {
        //                    var SchemeReturnData = ReturnAll.ToList();
        //                    int index = 0;
        //                    dtResult.Columns.Add(++index + "#Scheme_Name");

        //                    foreach (var z in SchemeReturnData.Select(b => b.Particulars))
        //                    {

        //                        dtResult.Columns.Add(++index + "#" + z + "_Rtn");
        //                        dtResult.Columns.Add(++index + "#" + z + "_Amt");
        //                    }
        //                    dtResult.Columns.Add(++index + "#Inception_Date");

        //                    //-------Scheme
        //                    DataRow DrScheme = dtResult.NewRow();
        //                    index = 0;
        //                    DrScheme[++index + "#" + "Scheme_Name"] = SchemeReturnData.FirstOrDefault().Scheme_Name;
        //                    foreach (var z in SchemeReturnData.Select(b => b.Particulars))
        //                    {
        //                        DrScheme[++index + "#" + z + "_Rtn"] = SchemeReturnData.Where(x => x.Particulars == z).FirstOrDefault().Scheme_Return;
        //                        DrScheme[++index + "#" + z + "_Amt"] = SchemeReturnData.Where(x => x.Particulars == z).FirstOrDefault().Scheme_Market_Value;
        //                    }
        //                    DrScheme[++index + "#" + "Inception_Date"] = SchemeReturnData.Where(x => x.Inception_Date.HasValue).FirstOrDefault().Inception_Date;
        //                    dtResult.Rows.Add(DrScheme);

        //                    //------Index
        //                    var IndexReturnData = ReturnAll;
        //                    index = 0;
        //                    if (IndexReturnData.Any())
        //                    {
        //                        DataRow DrIndex = dtResult.NewRow();
        //                        DrIndex[++index + "#" + "Scheme_Name"] = IndexReturnData.FirstOrDefault().Index_Name;
        //                        foreach (var z in IndexReturnData.Select(b => b.Particulars))
        //                        {
        //                            DrIndex[++index + "#" + z + "_Rtn"] = IndexReturnData.Where(x => x.Particulars == z).FirstOrDefault().Scheme_Return;
        //                            DrIndex[++index + "#" + z + "_Amt"] = IndexReturnData.Where(x => x.Particulars == z).FirstOrDefault().Scheme_Market_Value;
        //                        }
        //                        dtResult.Rows.Add(DrIndex);
        //                    }
        //                    //------Additional Index
        //                    var AddIndexReturnData = ReturnAll;
        //                    index = 0;

        //                    if (AddIndexReturnData.Any())
        //                    {
        //                        DataRow DrAddIndex = dtResult.NewRow();
        //                        DrAddIndex[++index + "#" + "Scheme_Name"] = AddIndexReturnData.FirstOrDefault().Additional_Index_Name;
        //                        foreach (var z in AddIndexReturnData.Select(b => b.Particulars))
        //                        {
        //                            DrAddIndex[++index + "#" + z + "_Rtn"] = AddIndexReturnData.Where(x => x.Particulars == z).FirstOrDefault().Scheme_Return;
        //                            DrAddIndex[++index + "#" + z + "_Amt"] = AddIndexReturnData.Where(x => x.Particulars == z).FirstOrDefault().Scheme_Market_Value;
        //                        }
        //                        dtResult.Rows.Add(DrAddIndex);
        //                    }
        //                    return dtResult;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return null;
        //}

        public static TataSebiParam GetTataSchemeSEBIReturn(decimal schemeIds, string indexIds, DateTime RtnDate)
        {
            string dynamicPeriod = string.Empty, dynamicOrder = string.Empty;
            DataTable dtResult = new DataTable();
            List<string> LstCol = new List<string>();
            try
            {
                using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
                {
                    //var SchemeIndex = (from x in dc.T_SCHEMES_BENCHMARK_ADDBENCHMARKs
                    //                   where x.Scheme_Id == schemeIds
                    //                   select new
                    //                   {
                    //                       Scheme_Id = x.Scheme_Id,
                    //                       Benchmark_Id = x.Benchmark_Id,
                    //                       Additional_Benchmark_Id = x.Additional_Benchmark
                    //                   });
                    //if (SchemeIndex.Any())
                    //{
                    //var SchemeIndexRelation = SchemeIndex.FirstOrDefault();

                    var ReturnAll = (from x in dc.T_SEBI_Returns
                                     where x.Scheme_ID == schemeIds && x.Return_Date == RtnDate
                                     select new
                                     {
                                         Particulars = x.Particulars,
                                         Return_Date = x.Return_Date,
                                         Scheme_Id = x.Scheme_ID,
                                         Scheme_Name = x.Scheme_name,
                                         Total_Amount_Invest = x.Total_Amount_Invest,
                                         Scheme_Return = x.Scheme_Return,
                                         Scheme_Market_Value = x.Scheme_Market_value,
                                         Index_Id = x.Index_ID,
                                         Index_Name = x.Index_Name,
                                         Bechmark_Return = x.Bechmark_return,
                                         Benchmark_Market_Value = x.Benchmark_Market_value,
                                         Additional_Index_Id = x.Additional_Index_ID,
                                         Additional_Index_Name = x.Additional_Index_Name,
                                         Additional_Bechmark_Return = x.Additional_Bechmark_return,
                                         Additional_Benchmark_Market_Value = x.Additional_Benchmark_Market_value,
                                         Inception_Date = x.Inception_Date
                                     });

                    if (ReturnAll.Any())
                    {
                        var SchemeReturnData = ReturnAll.ToList();
                        // int index = 0;
                        dtResult.Columns.Add("Scheme_Name");

                        foreach (var z in SchemeReturnData.Select(b => b.Particulars))
                        {

                            dtResult.Columns.Add(z + "_Rtn");
                            dtResult.Columns.Add(z + "_Amt");
                            LstCol.Add(z);
                        }
                        dtResult.Columns.Add("Inception_Date");

                        //-------Scheme
                        DataRow DrScheme = dtResult.NewRow();
                        //index = 0;
                        DrScheme["Scheme_Name"] = SchemeReturnData.FirstOrDefault().Scheme_Name;
                        foreach (var z in SchemeReturnData.Select(b => b.Particulars))
                        {
                            var DataReturn = SchemeReturnData.Where(x => x.Particulars == z).FirstOrDefault();
                            if (DataReturn.Scheme_Return.HasValue)
                                DrScheme[z + "_Rtn"] = Math.Round(DataReturn.Scheme_Return.Value, 2);
                            // DrScheme[z + "_Amt"] =Math.Round( SchemeReturnData.Where(x => x.Particulars == z).FirstOrDefault().Scheme_Market_Value.Value,2); change
                            if (DataReturn.Scheme_Market_Value.HasValue)
                                DrScheme[z + "_Amt"] = Math.Round(DataReturn.Scheme_Market_Value.Value, 2);
                        }
                        DrScheme["Inception_Date"] = Convert.ToDateTime(SchemeReturnData.Where(x => x.Inception_Date.HasValue).FirstOrDefault().Inception_Date).ToString("dd-MMM-yyyy");
                        dtResult.Rows.Add(DrScheme);

                        //------Index
                        var IndexReturnData = ReturnAll;
                        //index = 0;
                        if (IndexReturnData.Any())
                        {
                            DataRow DrIndex = dtResult.NewRow();
                            DrIndex["Scheme_Name"] = IndexReturnData.FirstOrDefault().Index_Name;
                            foreach (var z in IndexReturnData.Select(b => b.Particulars))
                            {
                                var DataReturn = IndexReturnData.Where(x => x.Particulars == z).FirstOrDefault();
                                if (DataReturn.Bechmark_Return.HasValue)
                                    DrIndex[z + "_Rtn"] = Math.Round(DataReturn.Bechmark_Return.Value, 2);
                                if (DataReturn.Benchmark_Market_Value.HasValue)
                                    DrIndex[z + "_Amt"] = Math.Round(DataReturn.Benchmark_Market_Value.Value, 2);
                            }
                            dtResult.Rows.Add(DrIndex);
                        }
                        //------Additional Index
                        var AddIndexReturnData = ReturnAll;
                        // index = 0;

                        if (AddIndexReturnData.Any())
                        {
                            DataRow DrAddIndex = dtResult.NewRow();
                            DrAddIndex["Scheme_Name"] = AddIndexReturnData.FirstOrDefault().Additional_Index_Name;
                            foreach (var z in AddIndexReturnData.Select(b => b.Particulars))
                            {
                                var DataReturn = AddIndexReturnData.Where(x => x.Particulars == z).FirstOrDefault();
                                if (DataReturn.Additional_Bechmark_Return.HasValue)
                                    DrAddIndex[z + "_Rtn"] = Math.Round(DataReturn.Additional_Bechmark_Return.Value, 2);
                                if (DataReturn.Additional_Benchmark_Market_Value.HasValue)
                                    DrAddIndex[z + "_Amt"] = Math.Round(DataReturn.Additional_Benchmark_Market_Value.Value, 2);
                            }
                            dtResult.Rows.Add(DrAddIndex);
                        }
                        return new TataSebiParam() { ReturnColumn = LstCol, DtData = dtResult };
                    }
                }
                //}
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public static List<FundManager> GetTataFundManager(int schemeId)
        {
            try
            {
                // using (var dc = new SIP_ClientDataContext() { CommandTimeout = 6000 })
                //{

                using (var FundmanagerData = new iFrames.DAL.SIP_ClientDataContext())
                {
                    var FundManager = (from fd in FundmanagerData.T_FUND_MANAGER_Clients
                                       join
                                    cfm in FundmanagerData.T_CURRENT_FUND_MANAGER_Clients on fd.FUNDMAN_ID equals cfm.FUNDMAN_ID
                                       join
                                       fms in FundmanagerData.T_FUND_MASTER_clients on cfm.FUND_ID equals fms.FUND_ID
                                       join
                                       sm in FundmanagerData.T_SCHEMES_MASTER_Clients on fms.FUND_ID equals sm.Fund_Id
                                       where
                                       sm.Scheme_Id == Convert.ToDecimal(schemeId) && cfm.LATEST_FUNDMAN == true
                                       select new FundManager
                                       {
                                           MFName = fd.FUND_MANAGER_NAME,
                                           MFId = (int)fd.FUNDMAN_ID
                                       });

                    if (FundManager.Count() > 0)
                    {
                        return FundManager.ToList();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception exp)
            {

            }
            return null;
        }

        public static List<int> GetTataManagerScheme(int FMId)
        {
            try
            {
                using (var FundmanagerData = new iFrames.DAL.SIP_ClientDataContext())
                {
                    var FundManager = (from fd in FundmanagerData.T_FUND_MANAGER_Clients
                                       from cfm in FundmanagerData.T_CURRENT_FUND_MANAGER_Clients
                                       from fms in FundmanagerData.T_FUND_MASTER_clients
                                       from sm in FundmanagerData.T_SCHEMES_MASTER_Clients
                                           // from ss in FundmanagerData.T_SCHEMES_STRUCTURE_Clients
                                       where
                                        fd.FUNDMAN_ID == cfm.FUNDMAN_ID && cfm.FUND_ID == fms.FUND_ID && fms.FUND_ID == sm.Fund_Id
                                       && fd.FUNDMAN_ID == Convert.ToDecimal(FMId) && sm.Nav_Check != 2 && fms.STRUCTURE_ID == 2 && sm.Parent_Scheme_Flag == true
                                       && cfm.LATEST_FUNDMAN == true
                                       select new
                                       {
                                           SchemeId = (int)sm.Scheme_Id
                                       });

                    if (FundManager.Count() > 0)
                    {
                        return FundManager.Select(x => Convert.ToInt32(x.SchemeId)).ToList();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception exp)
            {

            }
            return null;
        }

        public static XSSFWorkbook ExportToExcelWrite(DataStructure DSObj, string FilePath, List<FundManagerDataStructure> LstDataStructure)//ObjFundManagerDataStructure
        {
            try
            {
                string[] UserStatColumn = new string[] { "Scheme / Benchmark", "Return %", "Amount(Rs.)", "Return %", "Amount(Rs.)", "Return %", "Amount(Rs.)", "Return %", "Amount(Rs.)", "Inception Date" };

                //  if (System.IO.File.Exists(FilePath + "FundDetails.xlsx"))
                //      System.IO.File.Delete(FilePath + "FundDetails.xlsx");
                XSSFWorkbook WorkBook = new XSSFWorkbook();
                XSSFSheet sheet = (XSSFSheet)WorkBook.CreateSheet("Details");
                ICellStyle HeaderStyle = WorkBook.CreateCellStyle();
                HeaderStyle.BorderBottom = BorderStyle.Thin;
                HeaderStyle.BorderLeft = BorderStyle.Thin;
                HeaderStyle.BorderRight = BorderStyle.Thin;
                HeaderStyle.BorderTop = BorderStyle.Thin;
                NPOI.SS.UserModel.IFont headerFont = WorkBook.CreateFont();
                headerFont.Boldweight = (short)FontBoldWeight.Bold;
                headerFont.Color = (short)NPOI.HSSF.Util.HSSFColor.Black.Index;
                HeaderStyle.SetFont(headerFont);
                int rowcount = 3;
                IRow Row;
                ICell Cell;
                //for logo
                Row = sheet.CreateRow(0);
                Cell = Row.CreateCell(0);
                MergeCells(0, 0, 0, 8 + 1, sheet);
                if (File.Exists(FilePath))
                {
                    byte[] data = File.ReadAllBytes(FilePath);
                    string Extention = Path.GetExtension(FilePath);
                    int picInd = 0;
                    if (Extention.Equals(".png"))
                    {
                        picInd = WorkBook.AddPicture(data, XSSFWorkbook.PICTURE_TYPE_PNG);
                    }
                    else if (Extention.Equals(".gif"))
                    {
                        picInd = WorkBook.AddPicture(data, XSSFWorkbook.PICTURE_TYPE_GIF);
                    }
                    ICreationHelper helper = WorkBook.GetCreationHelper();
                    IDrawing drawing = sheet.CreateDrawingPatriarch();
                    IClientAnchor anchor = helper.CreateClientAnchor();
                    anchor.Col1 = 1;
                    anchor.Row1 = 0;
                    IPicture pict = drawing.CreatePicture(anchor, picInd);
                    pict.Resize();
                }
                //
                Row = sheet.CreateRow(1);
                int temp = 0;

                for (int y = 0; y < DSObj.PageDataReturnHeader.Count(); y++)
                {

                    if (y == 0)
                        temp = 1;
                    else
                        temp = temp + 2;
                    Cell = Row.CreateCell(temp);
                    MergeCells(1, 1, temp, temp + 1, sheet);
                    Cell.SetCellValue(DSObj.PageDataReturnHeader[y]);
                    Cell.CellStyle = HeaderStyle;
                }

                Row = sheet.CreateRow(2);
                for (int y = 0; y < UserStatColumn.Count(); y++)
                {
                    Cell = Row.CreateCell(y);
                    Cell.SetCellValue(UserStatColumn[y]);
                    Cell.CellStyle = HeaderStyle;

                }
                foreach (var item in DSObj.PageData)
                {
                    Row = sheet.CreateRow(rowcount++);
                    Cell = Row.CreateCell(0);
                    Cell.SetCellValue(Convert.ToString(item[0]));
                    Cell = Row.CreateCell(1);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[1]) ? "" : item[1]);
                    Cell = Row.CreateCell(2);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[2]) ? "" : item[2]);
                    Cell = Row.CreateCell(3);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[3]) ? "" : item[3]);
                    Cell = Row.CreateCell(4);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[4]) ? "" : item[4]);
                    Cell = Row.CreateCell(5);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[5]) ? "" : item[5]);
                    Cell = Row.CreateCell(6);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[6]) ? "" : item[6]);
                    Cell = Row.CreateCell(7);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[7]) ? "" : item[7]);
                    Cell = Row.CreateCell(8);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[8]) ? "" : item[8]);
                    Cell = Row.CreateCell(9);
                    Cell.SetCellValue(string.IsNullOrEmpty(item[9]) ? "" : item[9]);

                }
                rowcount = rowcount + 3;
                foreach (var ObjFundManagerDataStructure in LstDataStructure)
                {
                    Row = sheet.CreateRow(rowcount++);
                    Cell = Row.CreateCell(0);
                    MergeCells(Row.RowNum, Row.RowNum, 0, 8 + 1, sheet);
                    Cell.SetCellValue(string.IsNullOrEmpty(ObjFundManagerDataStructure.FundManagerName) ? "" : "Performance of other schemes managed by the " + ObjFundManagerDataStructure.FundManagerName);
                    Cell.CellStyle = HeaderStyle;

                    foreach (var item11 in ObjFundManagerDataStructure.LstDataStructure)
                    {

                        Row = sheet.CreateRow(rowcount++);

                        int temp1 = 0;
                        for (int y = 0; y < item11.PageDataReturnHeader.Count(); y++)
                        {
                            if (y == 0)
                                temp1 = 1;
                            else
                                temp1 = temp1 + 2;
                            Cell = Row.CreateCell(temp1);
                            MergeCells(Row.RowNum, Row.RowNum, temp1, temp1 + 1, sheet);
                            Cell.SetCellValue(item11.PageDataReturnHeader[y]);
                            Cell.CellStyle = HeaderStyle;

                        }
                        Row = sheet.CreateRow(rowcount++);
                        for (int y = 0; y < UserStatColumn.Count(); y++)
                        {
                            Cell = Row.CreateCell(y);
                            Cell.SetCellValue(UserStatColumn[y]);
                            Cell.CellStyle = HeaderStyle;

                        }
                        foreach (var item in item11.PageData)
                        {
                            Row = sheet.CreateRow(rowcount++);
                            Cell = Row.CreateCell(0);
                            Cell.SetCellValue(Convert.ToString(item[0]));
                            Cell = Row.CreateCell(1);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[1]) ? "" : item[1]);
                            Cell = Row.CreateCell(2);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[2]) ? "" : item[2]);
                            Cell = Row.CreateCell(3);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[3]) ? "" : item[3]);
                            Cell = Row.CreateCell(4);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[4]) ? "" : item[4]);
                            Cell = Row.CreateCell(5);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[5]) ? "" : item[5]);
                            Cell = Row.CreateCell(6);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[6]) ? "" : item[6]);
                            Cell = Row.CreateCell(7);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[7]) ? "" : item[7]);
                            Cell = Row.CreateCell(8);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[8]) ? "" : item[8]);
                            Cell = Row.CreateCell(9);
                            Cell.SetCellValue(string.IsNullOrEmpty(item[9]) ? "" : item[9]);
                        }
                        rowcount = rowcount + 1;
                    }
                    rowcount = rowcount + 2;
                }





                for (int y = 0; y < DSObj.PageDataReturnHeader.Count(); y++)
                {
                    sheet.AutoSizeColumn(y);
                }
                for (int y = 0; y < UserStatColumn.Count(); y++)
                {
                    sheet.AutoSizeColumn(y);
                }
                //  using (var fs = new FileStream(FilePath + "FundDetails.xlsx", FileMode.OpenOrCreate))
                //  {
                //      WorkBook.Write(fs);
                //  }

                return WorkBook;
                //return WriteToStream(WorkBook);
            }
            catch { return null; }
        }

        public static void MergeCells(int startrow, int endrow, int startcol, int endcol, XSSFSheet sheet)
        {
            try
            {
                sheet.AddMergedRegion(new CellRangeAddress(startrow, endrow, startcol, endcol));
            }
            catch (Exception ex)
            { }
        }




        public static DateTime getSchmindate(string SchemeIds)
        {

            List<decimal> lstSchemeIds = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeIds = getFundSchemeId(SchemeIds);
            }


            string MinDate = "";
            try
            {

                using (var db = new SIP_ClientDataContext())
                {
                    var data = (from d in db.T_NAV_DIV_clients
                                where lstSchemeIds.Contains(d.Scheme_Id)
                                select d.Nav_Date).Min();
                    MinDate = data.ToString();

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            return Convert.ToDateTime(MinDate);
        }

        public static DateTime? getBansalSchmindate(string SchemeIds)
        {

            List<decimal> lstSchemeIds = new List<decimal>();
            if (!string.IsNullOrEmpty(SchemeIds))
            {
                lstSchemeIds = getFundSchemeId(SchemeIds);
            }
            string MinDate = "";
            try
            {

                using (var db = new SIP_ClientDataContext())
                {
                    var data = (from d in db.T_NAV_DIV_clients
                                where lstSchemeIds.Contains(d.Scheme_Id)
                                select d.Nav_Date).Min();
                    MinDate = data.ToString();

                }
                if (MinDate == "")
                    return null;
                else
                    return Convert.ToDateTime(MinDate);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return null;
        }


        public static DataTable HDFC_Lumpsum_Sip(decimal Scheme, decimal index, decimal AddBenchmark, DateTime frmdate, DateTime toDate, decimal InitialInvestment, string frequency)
        {

            DataTable dtResult = new DataTable();
            DataTable _dtSch = new DataTable();
            DataTable _dtInd = new DataTable();
            DataTable _dtBenchMark = new DataTable();

            try
            {
                using (var db = new PrincipalCalcDataContext())
                {


                    IList<OutputSIP> OutResult = new List<OutputSIP>();
                    OutputSIP objLastSIP = null;

                    #region Calculation Between From And To Date

                    #region Get Sipdate for monthly,weekly,quarterly

                    var _SchMinDate = db.GetTable<T_NAV_DIV>().Where(x => x.Scheme_Id == Scheme).Select(x => x.Nav_Date);
                    var _IndMinDate = db.T_INDEX_RECORDs.Where(x => x.INDEX_ID == index).Select(x => x.RECORD_DATE);
                    var _AddIndMinDate = db.T_INDEX_RECORDs.Where(x => x.INDEX_ID == AddBenchmark).Select(x => x.RECORD_DATE);

                    DateTime? IndMinDate = null;
                    DateTime? AddIndMinDate = null;

                    var SchMinDate = _SchMinDate.Any() ? db.GetTable<T_NAV_DIV>().Where(x => x.Scheme_Id == Scheme).Select(x => x.Nav_Date).Min() : null;
                    if (_IndMinDate.Any())
                        IndMinDate = db.T_INDEX_RECORDs.Where(x => x.INDEX_ID == index).Select(x => x.RECORD_DATE).Min();
                    if (_AddIndMinDate.Any())
                        AddIndMinDate = db.T_INDEX_RECORDs.Where(x => x.INDEX_ID == AddBenchmark).Select(x => x.RECORD_DATE).Min();



                    var CommonNav = (from tbl in db.GetTable<T_NAV_DIV>()
                                     join schm in db.GetTable<T_SCHEMES_MASTER>() on tbl.Scheme_Id equals schm.Scheme_Id
                                     join tbINRec in db.T_INDEX_RECORDs.Where(x => x.INDEX_ID == index) on tbl.Nav_Date equals tbINRec.RECORD_DATE into ps
                                     join tbIRAB in db.T_INDEX_RECORDs.Where(x => x.INDEX_ID == AddBenchmark) on tbl.Nav_Date equals tbIRAB.RECORD_DATE into ps1
                                     where tbl.Nav_Date >= frmdate && tbl.Nav_Date <= toDate && schm.Scheme_Id == (Scheme)
                                     from tbINRec1 in ps.DefaultIfEmpty()
                                     from tbIRAB1 in ps1.DefaultIfEmpty()
                                     select new
                                     {
                                         SchemeName = schm.Scheme_Name,
                                         SchemeId = schm.Scheme_Id,
                                         CalculateDate = tbl.Nav_Date.Value,
                                         Scheme_Nav = (decimal)tbl.Nav,
                                         Scheme_Div_Amt = (decimal)(schm.Launch_Price.Value * (tbl.Div_Ind.HasValue ? tbl.Div_Ind.Value : 0) / 100),
                                         IndexName = db.T_INDEX_MASTERs.Where(x => x.INDEX_ID == index).FirstOrDefault().INDEX_NAME,
                                         IndexId = db.T_INDEX_MASTERs.Where(x => x.INDEX_ID == index).FirstOrDefault().INDEX_ID,
                                         IndexVal = (decimal)(tbINRec1.INDEX_VALUE != null ? tbINRec1.INDEX_VALUE : 0),
                                         AbName = db.T_INDEX_MASTERs.Where(x => x.INDEX_ID == AddBenchmark).FirstOrDefault().INDEX_NAME,
                                         ABId = db.T_INDEX_MASTERs.Where(x => x.INDEX_ID == AddBenchmark).FirstOrDefault().INDEX_ID,
                                         ABNavValue = (decimal)(tbIRAB1.INDEX_VALUE != null ? tbIRAB1.INDEX_VALUE : 0)
                                     }).OrderBy(p => p.CalculateDate);
                    List<DateTime> _DtSimpleSip = new List<DateTime>();

                    var AllCommonDateForSip = CommonNav.ToArray();

                    for (DateTime d1 = AllCommonDateForSip.First().CalculateDate;
                        d1 <= AllCommonDateForSip.Last().CalculateDate; d1 = (frequency == "Weekly" ? d1.AddDays(7) : frequency == "Quarterly" ? d1.AddMonths(3) : d1.AddMonths(1)))
                    {
                        if (AllCommonDateForSip.ToArray().Where(c => c.CalculateDate == d1).Any())
                        {
                            _DtSimpleSip.Add(d1);
                        }
                        else
                        {
                            for (DateTime d2 = d1; d2 <= AllCommonDateForSip.Last().CalculateDate; d2 = d2.AddDays(1))
                            {
                                if (AllCommonDateForSip.ToArray().Where(c => c.CalculateDate == d2).Any())
                                {
                                    _DtSimpleSip.Add(d2);
                                    break;
                                }

                            }
                        }
                    }


                    #endregion
                    foreach (var currentData in CommonNav)
                    {
                        OutputSIP lastCalculatedSIP = null; // This object holds the last triggared SIP Item
                        OutputSIP objSIPMonthly = null;

                        if (currentData.CalculateDate <= toDate)
                        {
                            if (OutResult.Count > 0)
                                objLastSIP = OutResult.Last();
                            if (OutResult.Count == 0)
                            {
                                objSIPMonthly = new OutputSIP()
                                {
                                    CalculateDate = currentData.CalculateDate,
                                    SchemeId = currentData.SchemeId,
                                    IndexId = currentData.IndexId,
                                    ABenchMarkId = currentData.ABId,
                                    InvestmentAmt = InitialInvestment,

                                    Scheme_Nav = currentData.Scheme_Nav,
                                    Index_Nav = currentData.IndexVal,
                                    AB_Nav = currentData.ABNavValue,

                                    SchemeName = currentData.SchemeName,
                                    IndexName = currentData.IndexName,
                                    ABName = currentData.AbName,

                                    Scheme_Investment_Value = InitialInvestment,
                                    Index_Investment_Value = InitialInvestment,
                                    AB_Investment_Value = InitialInvestment
                                };
                                objSIPMonthly.Scheme_Unit = Math.Round((objSIPMonthly.Scheme_Investment_Value / currentData.Scheme_Nav), 6);
                                if (currentData.IndexVal != 0)
                                    objSIPMonthly.Index_Unit = Math.Round((objSIPMonthly.Index_Investment_Value / currentData.IndexVal), 6);
                                if (currentData.ABNavValue != 0)
                                    objSIPMonthly.AB_Unit = Math.Round((objSIPMonthly.AB_Investment_Value / currentData.ABNavValue), 6);

                                objSIPMonthly.Scheme_SIP_Return = 0M;
                                objSIPMonthly.Index_SIP_Return = 0M;
                                objSIPMonthly.AB_SIP_Return = 0M;

                                if (currentData.Scheme_Div_Amt != 0)
                                {
                                    objSIPMonthly.Scheme_Div_Unit = Math.Round(currentData.Scheme_Div_Amt / currentData.Scheme_Nav, 6);
                                }

                                objSIPMonthly.Scheme_Cumulative_Unit = objSIPMonthly.Scheme_Unit + objSIPMonthly.Scheme_Div_Unit;
                                objSIPMonthly.Index_Cumulative_Unit = objSIPMonthly.Index_Unit;
                                objSIPMonthly.AB_Cumulative_Unit = objSIPMonthly.AB_Unit;

                            }
                            if (objLastSIP != null)
                            {
                                if (frequency == "Daily")
                                {
                                    objSIPMonthly = new OutputSIP()
                                    {
                                        CalculateDate = currentData.CalculateDate,
                                        SchemeId = currentData.SchemeId,
                                        IndexId = currentData.IndexId,
                                        ABenchMarkId = currentData.ABId,

                                        SchemeName = currentData.SchemeName,
                                        IndexName = currentData.IndexName,
                                        ABName = currentData.AbName,

                                        InvestmentAmt = InitialInvestment,

                                        Scheme_Nav = currentData.Scheme_Nav,
                                        Index_Nav = currentData.IndexVal,
                                        AB_Nav = currentData.ABNavValue,

                                        //Scheme_Unit = objLastSIP.Scheme_Unit,
                                        // Index_Unit = objLastSIP.Index_Unit,
                                        // AB_Unit = objLastSIP.AB_Unit,

                                    };
                                    objSIPMonthly.Scheme_Unit = Math.Round((InitialInvestment / currentData.Scheme_Nav), 6);
                                    if (currentData.IndexVal != 0)
                                        objSIPMonthly.Index_Unit = Math.Round((InitialInvestment / currentData.IndexVal), 6);
                                    if (currentData.ABNavValue != 0)
                                        objSIPMonthly.AB_Unit = Math.Round((InitialInvestment / currentData.ABNavValue), 6);

                                    if (currentData.Scheme_Div_Amt != 0)
                                    {
                                        //objSIPMonthly.Scheme_Div_Amt = Math.Round(Launch_Price.FirstOrDefault().Launch_Price * (latestSchemeDiv / 100), 6);
                                        objSIPMonthly.Scheme_Div_Unit = Math.Round(objLastSIP.Scheme_Cumulative_Unit * (currentData.Scheme_Div_Amt / currentData.Scheme_Nav), 6);
                                    }
                                    objSIPMonthly.Scheme_Cumulative_Unit = objSIPMonthly.Scheme_Unit + objSIPMonthly.Scheme_Div_Unit + objLastSIP.Scheme_Cumulative_Unit;
                                    objSIPMonthly.Index_Cumulative_Unit = objSIPMonthly.Index_Unit + objLastSIP.Index_Cumulative_Unit;
                                    objSIPMonthly.AB_Cumulative_Unit = objSIPMonthly.AB_Unit + objLastSIP.AB_Cumulative_Unit;

                                    objSIPMonthly.Scheme_Investment_Value = Math.Round((objSIPMonthly.Scheme_Cumulative_Unit * currentData.Scheme_Nav), 6);
                                    if (currentData.IndexVal != 0)
                                        objSIPMonthly.Index_Investment_Value = Math.Round((objSIPMonthly.Index_Cumulative_Unit * currentData.IndexVal), 6);
                                    else
                                        objSIPMonthly.Index_Investment_Value = objLastSIP.Index_Investment_Value;
                                    if (currentData.ABNavValue != 0)
                                        objSIPMonthly.AB_Investment_Value = Math.Round((objSIPMonthly.AB_Cumulative_Unit * currentData.ABNavValue), 6);
                                    else
                                        objSIPMonthly.AB_Investment_Value = objLastSIP.AB_Investment_Value;

                                    // objSIPMonthly.Scheme_SIP_Return = Math.Round((((objSIPMonthly.Scheme_Investment_Value / objSIPMonthly.Scheme_Cumulative_Unit) / (objLastSIP.Scheme_Investment_Value / objLastSIP.Scheme_Cumulative_Unit)) - 1) * 100, 6);
                                    // objSIPMonthly.Index_SIP_Return = Math.Round((((objSIPMonthly.Index_Investment_Value / objSIPMonthly.Index_Cumulative_Unit) / (objLastSIP.Index_Investment_Value / objLastSIP.Index_Cumulative_Unit)) - 1) * 100, 6);
                                    // objSIPMonthly.AB_SIP_Return = Math.Round((((objSIPMonthly.AB_Investment_Value / objSIPMonthly.AB_Cumulative_Unit) / (objLastSIP.AB_Investment_Value / objLastSIP.AB_Cumulative_Unit)) - 1) * 100, 6);

                                }
                                else if (frequency == "LumpSum")
                                {
                                    objSIPMonthly = new OutputSIP()
                                    {
                                        CalculateDate = currentData.CalculateDate,
                                        SchemeId = currentData.SchemeId,
                                        IndexId = currentData.IndexId,
                                        ABenchMarkId = currentData.ABId,

                                        SchemeName = currentData.SchemeName,
                                        IndexName = currentData.IndexName,
                                        ABName = currentData.AbName,

                                        Scheme_Nav = currentData.Scheme_Nav,
                                        Index_Nav = currentData.IndexVal,
                                        AB_Nav = currentData.ABNavValue,

                                        //Scheme_Unit = objLastSIP.Scheme_Unit,
                                        // Index_Unit = objLastSIP.Index_Unit,
                                        // AB_Unit = objLastSIP.AB_Unit,

                                    };

                                    if (currentData.Scheme_Div_Amt != 0)
                                    {
                                        //objSIPMonthly.Scheme_Div_Amt = Math.Round(Launch_Price.FirstOrDefault().Launch_Price * (latestSchemeDiv / 100), 6);
                                        objSIPMonthly.Scheme_Div_Unit = Math.Round(objLastSIP.Scheme_Cumulative_Unit * (currentData.Scheme_Div_Amt / currentData.Scheme_Nav), 6);
                                    }
                                    objSIPMonthly.Scheme_Cumulative_Unit = objSIPMonthly.Scheme_Unit + objSIPMonthly.Scheme_Div_Unit + objLastSIP.Scheme_Cumulative_Unit;
                                    objSIPMonthly.Index_Cumulative_Unit = objSIPMonthly.Index_Unit + objLastSIP.Index_Cumulative_Unit;
                                    objSIPMonthly.AB_Cumulative_Unit = objSIPMonthly.AB_Unit + objLastSIP.AB_Cumulative_Unit;

                                    objSIPMonthly.Scheme_Investment_Value = Math.Round((objSIPMonthly.Scheme_Cumulative_Unit * currentData.Scheme_Nav), 6);
                                    if (currentData.IndexVal != 0)
                                        objSIPMonthly.Index_Investment_Value = Math.Round((objSIPMonthly.Index_Cumulative_Unit * currentData.IndexVal), 6);
                                    else
                                        objSIPMonthly.Index_Investment_Value = objLastSIP.Index_Investment_Value;
                                    if (currentData.ABNavValue != 0)
                                        objSIPMonthly.AB_Investment_Value = Math.Round((objSIPMonthly.AB_Cumulative_Unit * currentData.ABNavValue), 6);
                                    else
                                        objSIPMonthly.AB_Investment_Value = objLastSIP.AB_Investment_Value;

                                    // objSIPMonthly.Scheme_SIP_Return = Math.Round(((objSIPMonthly.Scheme_Investment_Value / objLastSIP.Scheme_Investment_Value) - 1) * 100, 6);
                                    // objSIPMonthly.Index_SIP_Return = Math.Round(((objSIPMonthly.Index_Investment_Value / objLastSIP.Index_Investment_Value) - 1) * 100, 6);
                                    // objSIPMonthly.AB_SIP_Return = Math.Round(((objSIPMonthly.AB_Investment_Value / objLastSIP.AB_Investment_Value) - 1) * 100, 6);
                                }
                                else
                                {
                                    if (_DtSimpleSip.Contains(currentData.CalculateDate) && objLastSIP.Scheme_Investment_Value > 0)
                                    {

                                        objSIPMonthly = new OutputSIP()
                                        {
                                            CalculateDate = currentData.CalculateDate,
                                            SchemeId = currentData.SchemeId,
                                            IndexId = currentData.IndexId,
                                            ABenchMarkId = currentData.ABId,

                                            SchemeName = currentData.SchemeName,
                                            IndexName = currentData.IndexName,
                                            ABName = currentData.AbName,

                                            InvestmentAmt = InitialInvestment,

                                            Scheme_Nav = currentData.Scheme_Nav,
                                            Index_Nav = currentData.IndexVal,
                                            AB_Nav = currentData.ABNavValue,

                                        };
                                        objSIPMonthly.Scheme_Unit = Math.Round((InitialInvestment / currentData.Scheme_Nav), 6);
                                        if (currentData.IndexVal != 0)
                                            objSIPMonthly.Index_Unit = Math.Round((InitialInvestment / currentData.IndexVal), 6);
                                        if (currentData.ABNavValue != 0)
                                            objSIPMonthly.AB_Unit = Math.Round((InitialInvestment / currentData.ABNavValue), 6);

                                        if (currentData.Scheme_Div_Amt != 0)
                                        {
                                            objSIPMonthly.Scheme_Div_Unit = Math.Round(objLastSIP.Scheme_Cumulative_Unit * (currentData.Scheme_Div_Amt / currentData.Scheme_Nav), 6);
                                        }
                                        objSIPMonthly.Scheme_Cumulative_Unit = objSIPMonthly.Scheme_Unit + objSIPMonthly.Scheme_Div_Unit + objLastSIP.Scheme_Cumulative_Unit;
                                        objSIPMonthly.Index_Cumulative_Unit = objSIPMonthly.Index_Unit + objLastSIP.Index_Cumulative_Unit;
                                        objSIPMonthly.AB_Cumulative_Unit = objSIPMonthly.AB_Unit + objLastSIP.AB_Cumulative_Unit;

                                        objSIPMonthly.Scheme_Investment_Value = Math.Round((objSIPMonthly.Scheme_Cumulative_Unit * currentData.Scheme_Nav), 6);
                                        if (currentData.IndexVal != 0)
                                            objSIPMonthly.Index_Investment_Value = Math.Round((objSIPMonthly.Index_Cumulative_Unit * currentData.IndexVal), 6);
                                        else
                                            objSIPMonthly.Index_Investment_Value = objLastSIP.Index_Investment_Value;
                                        if (currentData.ABNavValue != 0)
                                            objSIPMonthly.AB_Investment_Value = Math.Round((objSIPMonthly.AB_Cumulative_Unit * currentData.ABNavValue), 6);
                                        else
                                            objSIPMonthly.AB_Investment_Value = objLastSIP.AB_Investment_Value;
                                    }
                                    else
                                    {
                                        objSIPMonthly = new OutputSIP()
                                        {
                                            CalculateDate = currentData.CalculateDate,
                                            SchemeId = currentData.SchemeId,
                                            IndexId = currentData.IndexId,
                                            ABenchMarkId = currentData.ABId,

                                            SchemeName = currentData.SchemeName,
                                            IndexName = currentData.IndexName,
                                            ABName = currentData.AbName,

                                            Scheme_Nav = currentData.Scheme_Nav,
                                            Index_Nav = currentData.IndexVal,
                                            AB_Nav = currentData.ABNavValue,

                                            //Scheme_Unit = objLastSIP.Scheme_Unit,
                                            // Index_Unit = objLastSIP.Index_Unit,
                                            // AB_Unit = objLastSIP.AB_Unit,

                                        };

                                        if (currentData.Scheme_Div_Amt != 0)
                                        {
                                            //objSIPMonthly.Scheme_Div_Amt = Math.Round(Launch_Price.FirstOrDefault().Launch_Price * (latestSchemeDiv / 100), 6);
                                            objSIPMonthly.Scheme_Div_Unit = Math.Round(objLastSIP.Scheme_Cumulative_Unit * (currentData.Scheme_Div_Amt / currentData.Scheme_Nav), 6);
                                        }
                                        objSIPMonthly.Scheme_Cumulative_Unit = objSIPMonthly.Scheme_Unit + objSIPMonthly.Scheme_Div_Unit + objLastSIP.Scheme_Cumulative_Unit;
                                        objSIPMonthly.Index_Cumulative_Unit = objSIPMonthly.Index_Unit + objLastSIP.Index_Cumulative_Unit;
                                        objSIPMonthly.AB_Cumulative_Unit = objSIPMonthly.AB_Unit + objLastSIP.AB_Cumulative_Unit;

                                        objSIPMonthly.Scheme_Investment_Value = Math.Round((objSIPMonthly.Scheme_Cumulative_Unit * currentData.Scheme_Nav), 6);
                                        if (currentData.IndexVal != 0)
                                            objSIPMonthly.Index_Investment_Value = Math.Round((objSIPMonthly.Index_Cumulative_Unit * currentData.IndexVal), 6);
                                        else
                                            objSIPMonthly.Index_Investment_Value = objLastSIP.Index_Investment_Value;
                                        if (currentData.ABNavValue != 0)
                                            objSIPMonthly.AB_Investment_Value = Math.Round((objSIPMonthly.AB_Cumulative_Unit * currentData.ABNavValue), 6);
                                        else
                                            objSIPMonthly.AB_Investment_Value = objLastSIP.AB_Investment_Value;

                                        //  objSIPMonthly.Scheme_SIP_Return = Math.Round(((objSIPMonthly.Scheme_Investment_Value / objLastSIP.Scheme_Investment_Value) - 1) * 100, 6);
                                        //  if(objLastSIP.Index_Investment_Value>0)
                                        //  objSIPMonthly.Index_SIP_Return = Math.Round(((objSIPMonthly.Index_Investment_Value / objLastSIP.Index_Investment_Value) - 1) * 100, 6);
                                        //  if (objLastSIP.AB_Investment_Value > 0)
                                        //  objSIPMonthly.AB_SIP_Return = Math.Round(((objSIPMonthly.AB_Investment_Value / objLastSIP.AB_Investment_Value) - 1) * 100, 6);

                                    }
                                }

                            }
                        }
                        OutResult.Add(objSIPMonthly);
                    }



                    #endregion





                    var SchemeData = (from p in OutResult
                                      select new
                                      {
                                          Scheme_Id = p.SchemeId,
                                          Sch_Short_Name = p.SchemeName,
                                          Nav_Date = p.CalculateDate,
                                          Nav = p.Scheme_Nav,
                                          InvestAmount = p.InvestmentAmt,
                                          InvestValue = p.Scheme_Investment_Value,
                                      });
                    _dtSch = SchemeData.ToDataTable();
                    // foreach (DataRow dr in _dtSch.Rows)
                    //    dtResult.Rows.Add(dr.ItemArray);
                    dtResult = _dtSch.Copy();
                    if (IndMinDate != null && AddIndMinDate != null)
                    {
                        if ((frmdate >= IndMinDate) && (frmdate >= AddIndMinDate))
                        {
                            var IndexData = (from p in OutResult
                                             select new
                                             {
                                                 p.IndexId,
                                                 p.IndexName,
                                                 p.CalculateDate,
                                                 p.Index_Nav,
                                                 p.InvestmentAmt,
                                                 p.Index_Investment_Value,
                                             });
                            _dtInd = IndexData.ToDataTable();
                            foreach (DataRow dr in _dtInd.Rows)
                                dtResult.Rows.Add(dr.ItemArray);

                            //additional bench mark section
                            var BenchmarkNAList = new List<decimal> { 3617, 4610 };
                            var FundId = db.T_SCHEMES_MASTERs.Where(n => n.Scheme_Id == Convert.ToDecimal(Scheme)).FirstOrDefault().Fund_Id;
                            if (!BenchmarkNAList.Contains(FundId.Value))
                            {
                                var ABData = (from p in OutResult
                                              select new
                                              {
                                                  p.ABenchMarkId,
                                                  p.ABName,
                                                  p.CalculateDate,
                                                  p.AB_Nav,
                                                  p.InvestmentAmt,
                                                  p.AB_Investment_Value,
                                              });
                                _dtBenchMark = ABData.ToDataTable();
                                foreach (DataRow dr in _dtBenchMark.Rows)
                                    dtResult.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                    var DtFinal = dtResult.Clone();
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        if (Convert.ToInt32(dr["Nav"]) != 0)
                            DtFinal.Rows.Add(dr.ItemArray);
                    }
                    //var Final = dtResult.AsEnumerable().Where(c => Convert.ToInt32(c["Nav"]) != 0).ToDataTable();
                    return DtFinal;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResult;

        }


        public static EdelSchemeIndex FetchTRIIndexName(decimal index_Id)
        {
            EdelSchemeIndex _EdelSchemeIndex = null;
            try
            {
                using (var principl = new PrincipalCalcDataContext())
                {
                    var Index = principl.T_INDEX_MASTERs.Where(x => x.INDEX_ID == index_Id).FirstOrDefault();
                    if (Index != null)
                    {
                        var CheckIsTri = Index.IsTRI.HasValue ? Index.IsTRI.Value : false;
                        if (CheckIsTri)
                            _EdelSchemeIndex = new EdelSchemeIndex { IndexID = Convert.ToInt32(Index.INDEX_ID), IndexName = Index.INDEX_NAME };
                        else
                        {
                            if (Index.TRI_PRI_Index.HasValue)
                            {
                                var TriIndexValue = principl.T_INDEX_MASTERs.Where(x => x.INDEX_ID == Index.TRI_PRI_Index.Value).FirstOrDefault();
                                _EdelSchemeIndex = new EdelSchemeIndex { IndexID = Convert.ToInt32(TriIndexValue.INDEX_ID), IndexName = TriIndexValue.INDEX_NAME };
                            }
                            else
                            {
                                _EdelSchemeIndex = new EdelSchemeIndex { IndexID = Convert.ToInt32(Index.INDEX_ID), IndexName = Index.INDEX_NAME };
                            }
                        }
                        return _EdelSchemeIndex;
                    }
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
            return _EdelSchemeIndex;
        }
        //End

        //for bansal capital fracsheet
        public static PreCalculatedRatioResults getPreCalculatedRatio(int SchemeId)
        {
            try
            {
                using (var DBContext = new iFrames.DAL.SIP_ClientDataContext())
                {
                    var data = (from PR in DBContext.T_PRECALCULATED_RATIOS_MFI_Clients
                                where PR.Scheme_Id == SchemeId
                                select new PreCalculatedRatioResults
                                {
                                    Scheme_Id = (int)PR.Scheme_Id,
                                    Beta = PR.Beta,
                                    RSQR = PR.RSQR,
                                    Sharp = PR.Sharp,
                                    Sortino = PR.Sortino,
                                    STDV = PR.STDV
                                }).FirstOrDefault();

                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception exp)
            {

            }
            return null;
        }

        public static Dictionary<string, string> GetHighLowNav(int schemeId)
        {
            try
            {
                using (var DBContext = new iFrames.DAL.SIP_ClientDataContext())
                {
                    var data = (from PR in DBContext.T_NAV_DIV_clients
                                where PR.Scheme_Id == schemeId && PR.Nav_Date.Value <= DateTime.Now.Date && PR.Nav_Date.Value >= DateTime.Now.AddDays(-(7 * 52)).Date
                                orderby PR.Nav descending
                                select new
                                {
                                    Scheme_Id = (int)PR.Scheme_Id,
                                    Nav_Date = PR.Nav_Date,
                                    Nav = PR.Nav
                                }).ToArray();

                    var max = data.FirstOrDefault();
                    var min = data.LastOrDefault();
                    Dictionary<string, string> obj = new Dictionary<string, string>();
                    obj.Add("max_nav", max.Nav != null ? Math.Round(Convert.ToDecimal(max.Nav), 2).ToString() : "--");
                    obj.Add("min_nav", min.Nav != null ? Math.Round(Convert.ToDecimal(min.Nav), 2).ToString() : "--");
                    obj.Add("min_nav_date", min.Nav_Date != null ? Convert.ToDateTime(min.Nav_Date).ToString("dd MMM yyyy") : "--");
                    obj.Add("max_nav_date", max.Nav_Date != null ? Convert.ToDateTime(max.Nav_Date).ToString("dd MMM yyyy") : "--");

                    return obj;
                }
            }
            catch (Exception exp)
            {

            }
            return null;
        }

        public static Dictionary<string, string> GetHighLowNavAskMeFund(int schemeId)
        {
            try
            {
                using (var DBContext = new iFrames.DAL.SIP_ClientDataContext())
                {
                    var data = (from PR in DBContext.T_NAV_DIV_clients
                                where PR.Scheme_Id == schemeId && PR.Nav_Date.Value <= DateTime.Now.Date && PR.Nav_Date.Value >= DateTime.Now.AddDays(-(7 * 52)).Date
                                orderby PR.Nav descending
                                select new
                                {
                                    Scheme_Id = (int)PR.Scheme_Id,
                                    Nav_Date = PR.Nav_Date,
                                    Nav = PR.Nav
                                }).ToArray();

                    var max = data.FirstOrDefault();
                    var min = data.LastOrDefault();
                    Dictionary<string, string> obj = new Dictionary<string, string>();
                    obj.Add("max_nav", max.Nav != null ? Math.Round(Convert.ToDecimal(max.Nav), 2).ToString() : "--");
                    obj.Add("min_nav", min.Nav != null ? Math.Round(Convert.ToDecimal(min.Nav), 2).ToString() : "--");
                    obj.Add("min_nav_date", min.Nav_Date != null ? Convert.ToDateTime(min.Nav_Date).ToString("dd/MM/yyyy") : "--");
                    obj.Add("max_nav_date", max.Nav_Date != null ? Convert.ToDateTime(max.Nav_Date).ToString("dd/MM/yyyy") : "--");

                    return obj;
                }
            }
            catch (Exception exp)
            {

            }
            return null;
        }


        public static DataTable SchemeMinIvest(int FundID)
        {
            try
            {
                using (var DBContext = new SIP_ClientDataContext())
                {
                    var data = (from PR in DBContext.T_SCHEMES_MASTER_Clients
                                join mi in DBContext.T_SCHEMES_DETAILs on
                                PR.Scheme_Id equals mi.Scheme_ID
                                where
                                PR.Fund_Id == FundID && PR.Nav_Check == 1
                                select new
                                {
                                    PR.Scheme_Id,
                                    PR.Sch_Short_Name,
                                    mi.Min_Investment
                                });



                    return data.ToDataTable();
                }
            }
            catch (Exception exp)
            {

            }
            return null;
        }
    }

    #region XIRR Calculation
    public class XIRRCalculation
    {
        private const Double DaysPerYear = 365.0;
        private const int MaxIterations = 100;
        private const double DefaultTolerance = 1E-6;
        private const double DefaultGuess = 0.1;

        public static readonly Func<IEnumerable<XIRRData>, Double> NewthonsMethod =
            cf => NewtonsMethodImplementation(cf, Xnpv, XnpvPrime);

        public static readonly Func<IEnumerable<XIRRData>, Double> BisectionMethod =
            cf => BisectionMethodImplementation(cf, Xnpv);

        public static double CalcXirr(IEnumerable<XIRRData> cashFlow, Func<IEnumerable<XIRRData>, double> method)
        {
            if (cashFlow.Count(cf => cf.Amount > 0) == 0)
                throw new ArgumentException("Add at least one positive item");

            if (cashFlow.Count(c => c.Amount < 0) == 0)
                throw new ArgumentException("Add at least one negative item");

            var result = method(cashFlow);

            if (Double.IsInfinity(result))
                throw new InvalidOperationException("Could not calculate: Infinity");

            if (Double.IsNaN(result))
                throw new InvalidOperationException("Could not calculate: Not a number");

            return result;
        }

        private static Double NewtonsMethodImplementation(IEnumerable<XIRRData> cashFlow,
                                                          Func<IEnumerable<XIRRData>, Double, Double> f,
                                                          Func<IEnumerable<XIRRData>, Double, Double> df,
                                                          Double guess = DefaultGuess,
                                                          Double tolerance = DefaultTolerance,
                                                          int maxIterations = MaxIterations)
        {
            var x0 = guess;
            var i = 0;
            Double error;
            do
            {
                var dfx0 = df(cashFlow, x0);
                if (Math.Abs(dfx0 - 0) < Double.Epsilon)
                    throw new InvalidOperationException("Could not calculate: No solution found. df(x) = 0");

                var fx0 = f(cashFlow, x0);
                var x1 = x0 - fx0 / dfx0;
                error = Math.Abs(x1 - x0);

                x0 = x1;
            } while (error > tolerance && ++i < maxIterations);
            if (i == maxIterations)
                throw new InvalidOperationException("Could not calculate: No solution found. Max iterations reached.");

            return x0;
        }

        internal static Double BisectionMethodImplementation(IEnumerable<XIRRData> cashFlow,
                                                             Func<IEnumerable<XIRRData>, Double, Double> f,
                                                             Double tolerance = DefaultTolerance,
                                                             int maxIterations = MaxIterations)
        {
            // From "Applied Numerical Analysis" by Gerald
            var brackets = Brackets.Find(Xnpv, cashFlow);
            if (Math.Abs(brackets.First - brackets.Second) < Double.Epsilon)
                throw new ArgumentException("Could not calculate: bracket failed");

            Double f3;
            Double result;
            var x1 = brackets.First;
            var x2 = brackets.Second;

            var i = 0;
            do
            {
                var f1 = f(cashFlow, x1);
                var f2 = f(cashFlow, x2);

                if (Math.Abs(f1) < Double.Epsilon && Math.Abs(f2) < Double.Epsilon)
                    throw new InvalidOperationException("Could not calculate: No solution found");

                if (f1 * f2 > 0)
                    throw new ArgumentException("Could not calculate: bracket failed for x1, x2");

                result = (x1 + x2) / 2;
                f3 = f(cashFlow, result);

                if (f3 * f1 < 0)
                    x2 = result;
                else
                    x1 = result;
            } while (Math.Abs(x1 - x2) / 2 > tolerance && Math.Abs(f3) > Double.Epsilon && ++i < maxIterations);

            if (i == maxIterations)
                throw new InvalidOperationException("Could not calculate: No solution found");

            return result;
        }

        private static Double Xnpv(IEnumerable<XIRRData> cashFlow, Double rate)
        {
            if (rate <= -1)
                rate = -1 + 1E-10; // Very funky ... Better check what an IRR <= -100% means

            var startDate = cashFlow.OrderBy(i => i.Date).First().Date;
            return
                (from item in cashFlow
                 let days = -(item.Date - startDate).Days
                 select item.Amount * Math.Pow(1 + rate, days / DaysPerYear)).Sum();
        }

        private static Double XnpvPrime(IEnumerable<XIRRData> cashFlow, Double rate)
        {
            var startDate = cashFlow.OrderBy(i => i.Date).First().Date;
            return (from item in cashFlow
                    let daysRatio = -(item.Date - startDate).Days / DaysPerYear
                    select item.Amount * daysRatio * Math.Pow(1.0 + rate, daysRatio - 1)).Sum();
        }

        public struct Brackets
        {
            public readonly Double First;
            public readonly Double Second;

            public Brackets(Double first, Double second)
            {
                First = first;
                Second = second;
            }

            internal static Brackets Find(Func<IEnumerable<XIRRData>, Double, Double> f,
                                          IEnumerable<XIRRData> cashFlow,
                                          Double guess = DefaultGuess,
                                          int maxIterations = MaxIterations)
            {
                const Double bracketStep = 0.5;
                var leftBracket = guess - bracketStep;
                var rightBracket = guess + bracketStep;
                var i = 0;
                while (f(cashFlow, leftBracket) * f(cashFlow, rightBracket) > 0 && i++ < maxIterations)
                {
                    leftBracket -= bracketStep;
                    rightBracket += bracketStep;
                }

                return i >= maxIterations
                           ? new Brackets(0, 0)
                           : new Brackets(leftBracket, rightBracket);
            }
        }


    }

    public struct XIRRData
    {
        public DateTime Date;
        public Double Amount;

        public XIRRData(DateTime date, Double amount)
        {
            Date = date;
            Amount = amount;
        }
    }

    #endregion
}