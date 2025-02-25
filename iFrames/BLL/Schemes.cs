using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iFrames.DAL;

namespace iFrames.BLL
{
    static public class Schemes
    {
        static public DataTable GetSchemes()
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    return indcap.GetTable<scheme_Info>().Select(f => f).GetTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTbl(string MutcodeFromBase="")
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    //return indcap.GetTable<scheme_Info>().Select(f => f).GetTable();
                    List<string> arr = new List<string>();
                    if (MutcodeFromBase != "")
                    {
                        string[] path = MutcodeFromBase.Split(new char[] { ',' });
                        for (int i = 0; i < path.Length; i++)
                        {
                            arr.Add(path[i]);
                        }
                    }
                    var tbl = from s in indcap.GetTable<scheme_Info>()
                              from m in indcap.mut_funds
                              where (MutcodeFromBase != ""?arr.Contains(s.mut_code) :true)&& s.mut_code==m.Mut_Code && s.nav_check=="new" &&
                              s.close_date >= Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"))
                              orderby s.sch_name
                              select new { s.sch_code, s.sch_name, s.close_date, m.Mut_Name,s.mut_code };
                    
                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTblRetrive(string sch_code)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    //return indcap.GetTable<scheme_Info>().Select(f => f).GetTable();
                    var tbl = from s in indcap.GetTable<scheme_Info>()
                              from m in indcap.mut_funds
                              where s.mut_code == m.Mut_Code && s.sch_code.Trim()==sch_code
                              select new { s.sch_code,s.sch_name,s.Iss_date,s.close_date,s.type1,s.nature,s.type2,s.type3_sp,s.@object,
                              s.min_invt,s.incr_invt,s.tax_ben1,s.tax_ben2,s.tax_ben3,s.tax_ben4,s.sip,s.nri,m.Mut_Name,m.Reg_Add1,m.Reg_Add2,m.Reg_city,
                              m.Reg_Phone1,s.reg_code};
                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTblAmc(string MutcodeFromBase = "")
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    //return indcap.GetTable<scheme_Info>().Select(f => f).GetTable();
                    List<string> arr = new List<string>();
                    if (MutcodeFromBase != "")
                    {
                        string[] path = MutcodeFromBase.Split(new char[] { ',' });
                        for (int i = 0; i < path.Length; i++)
                        {
                            //arr.Add(path[i]);
                            MutcodeFromBase += path[i] + "',";
                        }
                        MutcodeFromBase = MutcodeFromBase.TrimEnd(',');
                        MutcodeFromBase += ")";
                    }
                    var tbl = (from s in indcap.GetTable<scheme_Info>()
                               from m in indcap.AMCs
                               from t in indcap.amc_details
                               where s.amc_code == m.AMC_Code && m.AMC_Code == t.AMC_Code && s.nav_check != "red" && t.Detail1 != null
                               select new { m.AMC_Code, m.AMC_Name }).Distinct().OrderBy(m => m.AMC_Name);

                    DataRow[] drs;
                    if (MutcodeFromBase != "")
                        drs = tbl.ToDataTable().Select(MutcodeFromBase,"");
                    else
                        drs = tbl.ToDataTable().Select("","");
                    DataTable dt = tbl.ToDataTable().Clone();
                    foreach (var drResult in drs)
                    {
                        dt.ImportRow(drResult);
                    }
                    return dt; 
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTblAmcDetail(string amcval)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    //return indcap.GetTable<scheme_Info>().Select(f => f).GetTable();
                    var tbl = from  m  in indcap.amc_details
                               where m.AMC_Code==amcval
                               select new {m.AMC_Code,m.Detail1,m.Detail2,m.Detail3,m.Key_Persons,m.Total_Corpus__Crs_};
                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTblAmcCount(string amcval)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    //return indcap.GetTable<scheme_Info>().Select(f => f).GetTable();
                    var count1 = (from s in indcap.GetTable<scheme_Info>()
                              where s.amc_code == amcval && s.nav_check != "red"
                              select s.Fund_Name).Distinct().Count();
                    var count2 = (from s in indcap.GetTable<scheme_Info>()
                                  where s.amc_code == amcval && s.nav_check != "red"
                                  select s.short_name).Distinct().Count();
                   using ( DataTable dtadd = new DataTable())
                   {
                    dtadd.Columns.Add("count1");
                    dtadd.Columns.Add("count2");
                    dtadd.Rows.Add(count1, count2);
                    return dtadd;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTblShmeCount(string amcval)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    //return indcap.GetTable<scheme_Info>().Select(f => f).GetTable();
                    var tbl = from s in indcap.GetTable<scheme_Info>()
                              where s.amc_code == amcval && s.nav_check != "red"
                              group s by new { s.nature } into g
                              orderby g.Key.nature
                              select new {nature=g.Key.nature.Trim(),count=g.Count()};

                    return tbl.ToDataTable(); 
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTblAmCorpus(string amcval)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    //return indcap.GetTable<scheme_Info>().Select(f => f).GetTable();
                    var tbl = (from s in indcap.GetTable<scheme_Info>()
                              from m in indcap.amc_details
                              from c in indcap.corpus
                              where c.Date ==
                              ((from cc in indcap.corpus

                                where cc.Mut_code == s.mut_code && s.amc_code == amcval
                                select new { cc.Date }).Max(tt => tt.Date)) && s.amc_code == m.AMC_Code && s.mut_code == c.Mut_code && s.amc_code == amcval

                              select new { s.mut_code, m.AMC_Code, c.Corpus_Crs_, c.Date }).Distinct();   

                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTblFundmgr(string amcval)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {                   
                    var tbl = (from s in indcap.GetTable<scheme_Info>()
                               from c in indcap.cur_fund_mans
                               from f in indcap.Fund_Managers
                               from m in indcap.mut_funds
                               where c.app_dt ==
                               ((from cc in indcap.cur_fund_mans
                                 where cc.fund_code ==c.fund_code && (cc.ch_dt==null)
                                 select new { cc.app_dt }).Max(tt => tt.app_dt)) && s.sch_code == c.sch_code && c.fund_code == f.Fund_Code && f.Mut_Code == m.Mut_Code 
                                 && s.amc_code == amcval && s.nav_check!="red"                             

                               select new { f.Fund_Manager1,m.Web_Site}).Distinct();

                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtTblOpenSheme(string amcval)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    using (DataTable dtadd = new DataTable())
                    {
                        dtadd.Columns.Add("sch_code");
                        dtadd.Columns.Add("SchemeName");
                        dtadd.Columns.Add("PerAsOn");
                        dtadd.Columns.Add("30Days");
                        dtadd.Columns.Add("91Days");
                        dtadd.Columns.Add("1Year");
                        dtadd.Columns.Add("3Year");
                        dtadd.Columns.Add("RsinCr");
                        dtadd.Columns.Add("Ason");
                        dtadd.Columns.Add("mutcode");
                        // DataRow dradd=dtadd.NewRow();
                        var tbl1 = (from s in indcap.GetTable<scheme_Info>()
                                   where s.amc_code == amcval && s.type1 == "open ended" && s.nav_check != "red"
                                   orderby s.short_name
                                   select new { s.sch_code, s.short_name, s.mut_code }).ToDataTable();
                        foreach (DataRow record in tbl1.Rows)
                        {
                            DataRow dradd = dtadd.NewRow();
                            var tbl2= (from  s in indcap.GetTable<scheme_Info>()
                                      from t in indcap.top_funds
                                      where t.date==
                                      ((from cc in indcap.top_funds
                                 where cc.sch_code ==s.sch_code 
                                 select new { cc.date }).Max(tt => tt.date)) && s.sch_code==t.sch_code && s.sch_code==record["sch_code"].ToString()
                                 select new { t.date,t.per_30days,t.per_91days,t.per_1yr,t.per_3yr}).ToDataTable();
                            var tbl3 = (from f in indcap.FundSizes
                                        where f.Date ==
                                         ((from cc in indcap.FundSizes
                                           where cc.sch_Code == record["sch_code"].ToString()
                                           select new { cc.Date }).Max(tt => tt.Date)) && f.sch_Code == record["sch_code"].ToString()
                                        select new { f.sch_Code, f.Date, f.fund_Size }).ToDataTable();
                            //if (tbl2.Rows.Count > 0)
                            //{

                            //}
                            //dradd[0] = (tbl1.Rows.Count > 0 ? record["sch_code"].ToString() : "NA");
                            //dradd[1] = (tbl1.Rows.Count > 0 ? record["short_name"].ToString() : "NA");
                            //dradd[2] = (tbl2.Rows.Count > 0 ? tbl2.Rows[0]["date"].ToString() : "NA");
                            //dradd[3] = (tbl2.Rows.Count > 0 ? tbl2.Rows[0]["per_30days"].ToString() : "NA");
                            //dradd[4] = (tbl2.Rows.Count > 0 ? tbl2.Rows[0]["per_91days"].ToString() : "NA");
                            //dradd[5] = (tbl2.Rows.Count > 0 ? tbl2.Rows[0]["per_1yr"].ToString() : "NA");
                            //dradd[6] = (tbl2.Rows.Count > 0 ? tbl2.Rows[0]["per_3yr"].ToString() : "NA");
                            //dradd[7] = (tbl3.Rows.Count > 0 ? tbl3.Rows[0]["fund_Size"].ToString() : "NA");
                            //dradd[8] = (tbl3.Rows.Count > 0 ? tbl3.Rows[0]["Date"].ToString() : "NA");
                            //dradd[9] = (tbl1.Rows.Count > 0 ? record["mut_code"].ToString() : "NA");

                            dradd[0] = (tbl1.Rows.Count > 0 && tbl1 !=null ? record["sch_code"].ToString() : null);
                            dradd[1] = (tbl1.Rows.Count > 0 && tbl1 != null ? record["short_name"].ToString() : null);
                            dradd[2] = (tbl2.Rows.Count > 0 && tbl2 != null ? tbl2.Rows[0]["date"].ToString() : null);
                            dradd[3] = (tbl2.Rows.Count > 0 && tbl2 != null ? tbl2.Rows[0]["per_30days"].ToString() : null);
                            dradd[4] = (tbl2.Rows.Count > 0 && tbl2 != null ? tbl2.Rows[0]["per_91days"].ToString() : null);
                            dradd[5] = (tbl2.Rows.Count > 0 && tbl2 != null ? tbl2.Rows[0]["per_1yr"].ToString() : null);
                            dradd[6] = (tbl2.Rows.Count > 0 && tbl2 != null ? tbl2.Rows[0]["per_3yr"].ToString() : null);
                            dradd[7] = (tbl3.Rows.Count > 0 && tbl3 != null ? tbl3.Rows[0]["fund_Size"].ToString() : null);
                            dradd[8] = (tbl3.Rows.Count > 0 && tbl3 != null ? tbl3.Rows[0]["Date"].ToString() : null);
                            dradd[9] = (tbl1.Rows.Count > 0 && tbl1 != null ? record["mut_code"].ToString() : null);
                            dtadd.Rows.Add(dradd);
                        }
                        return dtadd;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetSchDetails(string mutCode)
        {
          try
          {
            using (var indcap = new iFramesDataContext())
            {
              var dtSchDetails=(from c in indcap.GetTable<scheme_Info>()
                                where c.mut_code == mutCode && c.nav_check != "red" && c.type1 == "open ended"                               
                                orderby c.short_name
                                select new{c.sch_code,c.short_name,c.Iss_date});

              return dtSchDetails.ToDataTable();
            }
          }
          catch (Exception)
          {
            return null;
          }
        }

        static public DataTable GetInd(string schCode)
        {
          try
          {
            using (var mfiEXP = new iFramesExplorerDataContext())
            {
                return (from indinf in mfiEXP.ind_infoExplorers
                        where
                                  (from c in mfiEXP.GetTable<SchemeIndex>()
                                  where c.Sch_Code == schCode
                                   select c.Ind_Code).Distinct().Contains(indinf.ind_code)
                                  select new {indinf.ind_code }).ToDataTable();
            }
          }
          catch (Exception)
          {
            return null;
          }
        }

        static public DataTable GetIndPrnpl(string schName)
        {
            try
            {
                using (var principl = new PrincipalCalcDataContext())
                {
                    return (from t_index_masters in principl.T_INDEX_MASTERs
                                      where
                                        t_index_masters.INDEX_ID ==
                                          ((from t_schemes_indexes in principl.T_SCHEMES_INDEXes
                                            where
                                              t_schemes_indexes.SCHEME_ID ==
                                                ((from t_schemes_masters in principl.T_SCHEMES_MASTERs
                                                  where
                                                    t_schemes_masters.Scheme_Name == schName
                                                  select new
                                                  {
                                                      t_schemes_masters.Scheme_Id
                                                  }).First().Scheme_Id)
                                            select new
                                            {
                                                t_schemes_indexes.INDEX_ID
                                            }).Take(1).First().INDEX_ID)
                                      select new
                                      {
                                          t_index_masters.INDEX_NAME
                                      }).ToDataTable();
                                       
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetAllInd()
        {
            try
            {
                using (var mfiEXP = new iFramesExplorerDataContext())
                {
                    return (from indinf in mfiEXP.ind_infoExplorers                          
                            select new { indinf.ind_name, indinf.ind_code }).ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetNav(string schCode,DateTime dt)
        {
            try
            {
                using (var mfiEXP = new iFramesExplorerDataContext())
                {
                    var navRs = (from c in mfiEXP.Nav_Recs
                                        where c.Sch_Code == schCode && c.Date == dt                                        
                                        select new { c.Nav_Rs}).ToDataTable();
                    return navRs;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetAllNav(string schCode)
        {
            try
            {
                using (var mfiEXP = new iFramesExplorerDataContext())
                {
                    var navRs = (from c in mfiEXP.Nav_Recs
                                 where c.Sch_Code == schCode
                                 select new { c.Nav_Rs,c.Date }).ToDataTable();
                    return navRs;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetInd(string indCode, DateTime dt)
        {
            try
            {
                using (var mfiEXP = new iFramesExplorerDataContext())
                {
                    var navRs = (from c in mfiEXP.ind_recExplorers
                                 where c.ind_code == indCode && c.dt1== dt
                                 select new { c.ind_val}).ToDataTable();
                    return navRs;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetAllInd(string indCode)
        {
            try
            {
                using (var mfiEXP = new iFramesExplorerDataContext())
                {
                    var navRs = (from c in mfiEXP.ind_recExplorers
                                 where c.ind_code == indCode
                                 select new { c.ind_val,c.dt1 }).ToDataTable();
                    return navRs;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public string GetLastNav(string schCode)
        {
            try
            {
                using (var mfiEXP = new iFramesExplorerDataContext())
                {
                    var navDate = (from c in mfiEXP.Nav_Recs
                                   where c.Sch_Code == schCode
                                   select new { c.Date }).Max(tt => tt.Date).ToString();
                    return navDate;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public string GetFirstNav(string schCode)
        {
            try
            {
                using (var mfiEXP = new iFramesExplorerDataContext())
                {
                    var navDate = (from c in mfiEXP.Nav_Recs
                                   where c.Sch_Code == schCode
                                   select new { c.Date }).Min(tt => tt.Date).ToString();
                    return navDate;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public string GetIssDate(string schCode)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var IssDate = (from c in indcap.GetTable<scheme_Info>()
                                        where c.sch_code == schCode
                                        select new { c.Iss_date }).Min(tt => tt.Iss_date).ToString();
                    return IssDate;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DataTable getCompany(string Comp)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                dt = ((from n in db.net_com_pots
                           from s in db.GetTable<scheme_Info>()
                           where n.Sch_Code == s.sch_code && s.nav_check != "red" && s.nature.StartsWith("equity")
                           && n.c_code.StartsWith("c") && n.c_name.StartsWith(Comp)
                           select new {n.c_name }).Distinct()).ToDataTable();
            }
            return dt;
        }

        public static DataTable getSectors()
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                //string[] arr={"Debt Investments","Current Assets","Securities"};
                var data = ((from s in db.Sector_inds
                             select new { s.Sect_name }).Distinct()).ToDataTable();
                DataRow[] drs = data.Select("Sect_name not in ('Debt Investments','Current Assets','Securities')", "Sect_name asc");
                dt = data.Clone();
                foreach (DataRow dr in drs)
                {
                    dt.ImportRow(dr);
                }
                dt.AcceptChanges();
            }
            return dt;
        }

        public static DataTable getSchemeInfos(string CompName,string percentage)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                var data = (from s in db.GetTable<scheme_Info>()
                           from n in db.net_com_pots
                           from l in db.latest_navs
                           from t in db.top_funds
                           where t.date == ((from tt in db.top_funds
                                             where tt.sch_code == t.sch_code
                                             select tt.date.Value).Max()) && n.Sch_Code == l.Sch_Code &&
                                              s.sch_code == l.Sch_Code && s.sch_code == t.sch_code && s.nav_check != "red"
                                              && s.nature.StartsWith("equity") && n.Date == ((from nn in db.net_com_pots
                                                                                              where nn.Sch_Code == n.Sch_Code && nn.c_name == CompName
                                                                                              select nn.Date.Value).Max())
                                                                                               && n.c_name == CompName
                           select new {
                               s.short_name,
                               LDate=l.Date,
                               l.Nav_Rs,
                               s.type1,
                               s.type3,
                               n.c_name,
                               netDate=n.Date,
                               n.corpus_per,
                               t.per_91days,
                               t.per_1yr,
                               t.per_3yr,
                               s.sch_code,
                               TDate=t.date });

                if (Convert.ToDouble(percentage) > 0)
                    data = data.Where(a => a.corpus_per >= Convert.ToDouble(percentage)).OrderBy(a=>a.short_name);

                dt = data.ToDataTable();
            }
            return dt;
        }

        public static DataTable getSectorInfos(string Sector, string percentage)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                var data = (from s in db.GetTable<scheme_Info>()
                            from i in db.Sector_inds
                            from n in db.net_ind_pot1s
                            from l in db.latest_navs
                            from t in db.top_funds
                            where t.date == ((from tt in db.top_funds
                                              where tt.sch_code == t.sch_code
                                              select tt.date.Value).Max()) &&
                                              n.Sect_Code==i.Sect_code &&
                                              n.Sch_Code == l.Sch_Code &&
                                               s.sch_code == l.Sch_Code && 
                                               s.sch_code == t.sch_code && 
                                               s.nav_check != "red"
                                               && s.nature.StartsWith("equity") && 
                                               n.Date == ((from nn in db.net_com_pots
                                                            where nn.Sch_Code == n.Sch_Code
                                                            select nn.Date.Value).Max()) && 
                                                            i.Sect_name == Sector
                            select new
                            {
                                s.short_name,
                                LDate = l.Date,
                                l.Nav_Rs,
                                s.type1,
                                s.type3,
                                i.Sect_name,
                                netDate = n.Date,
                                n.Percent,
                                t.per_91days,
                                t.per_1yr,
                                t.per_3yr,
                                s.sch_code,
                                TDate = t.date
                            });

                if (Convert.ToDouble(percentage) > 0)
                    data = data.Where(a => a.Percent >= Convert.ToDouble(percentage)).OrderBy(a => a.short_name);

                dt = data.ToDataTable();
            }
            return dt;
        }

        static public DataTable GetRating()
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var tbl = ((from c in indcap.net_com_pots

                                select new { c.Rating }).Distinct().OrderBy(x => x.Rating)).ToDataTable();
                    return tbl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetChooseSchemeDetail(string structure, string investobj, string nature, string nri, string repartiability,
            string FundaAge, DateTime FundaAgeDate, string MinInvst, string FundSize, string period, string anyrturn, string dividend,
            string taxbenefit, string rating, string anypercent, string condition)
        {

            try
            {
                using (var indcap = new iFramesDataContext())
                {

                    //var sql = @"SElect s.sch_code,m.mut_code,m.mut_name from mut_fund m ,scheme_info s where   s.mut_code=m.mut_code
                    //and m.mut_code='MF001'";
                    // var cust = indcap.ExecuteQuery<RunQueryFromContext>(sql, "seattle");
                    //                    string q = "MF001";
                    //                    var sql = @"exec sp_executesql N'SElect s.sch_code,m.mut_code,m.mut_name from mut_fund m ,scheme_info s where  s.mut_code=m.mut_code and
                    //                    s.mut_code=@mutcode',N'@mutcode as varchar(5)'," + q;


                    //string querybuilder = null;

                    string[] mininvt = MinInvst != null ? MinInvst.Split(new char[] { ',' }) : null;
                    string[] fsize = FundSize != null ? FundSize.Split(new char[] { ',' }) : null;
                    string[] andreturn = period != null && anyrturn != null ? anyrturn.Split(new char[] { ',' }) : null;
                    string[] divyield = dividend != null ? dividend.Split(new char[] { ',' }) : null;
                    string[] percent = rating != null && anypercent != null ? anypercent.Split(new char[] { ',' }) : null;
                    var querybuilder = @"exec sp_executesql N'select distinct s.sch_code,m.mut_code,s.short_name,s.type1 ,s.type3  ";
                    querybuilder += FundSize != null ? ",f.fund_size " : null;
                    querybuilder += MinInvst != null ? ",s.min_invt " : null;
                    querybuilder += FundaAge != " " ? ",s.close_date " : null;
                    querybuilder += rating != null ? ",p.rt_per " : null;
                    querybuilder += "from scheme_info s ,mut_fund m ";
                    querybuilder += FundSize != null ? ",fundsize f " : null;
                    querybuilder += period != null || dividend != null ? ",top_fund t " : null;
                    querybuilder += rating != null ? ",port_rate p " : null;
                    querybuilder += "where s.mut_code =m.mut_code ";
                    querybuilder += FundSize != null ? " and s.sch_code=f.sch_code " : null;
                    querybuilder += period != null || dividend != null ? "and t.sch_code=s.sch_code  " : null;
                    querybuilder += rating != null ? " and s.sch_code=p.sch_code " : null;
                    querybuilder += " and s.nav_check<>@red" + " and s.nature not like @Equity";
                    querybuilder += MinInvst != null ? " and s.min_invt " + mininvt[0] + "@MinInvst" : null;
                    querybuilder += " and s.type1=" + "@structure";
                    querybuilder += nature != null ? " and s.nature like " + "@nature" : null;
                    querybuilder += investobj != null ? " and s.type3=" + "@investobj" : null;
                    querybuilder += FundaAge != " " ? " and s.close_date " + condition + "@FundaAgeDate" : null;
                    querybuilder += FundSize != null ? " and f.fund_size " + fsize[0] + "@FundSize" + " and f.date = (select max(date) from fundsize where sch_code = f.sch_code) " : null;
                    querybuilder += period != null && anyrturn != null ? " and t." + period + andreturn[0] + "@anyrturn" : null;
                    querybuilder += dividend != null ? " and t.div_yld " + divyield[0] + "@dividend" : null;
                    querybuilder += nri != null ? " and s.nri=" + "@nri" : null;
                    querybuilder += repartiability != null ? " and s.repatri=" + "@repartiability" : null;
                    querybuilder += taxbenefit != null ? " and s.tax_ben3=" + "@taxbenefit" : null;
                    querybuilder += rating != null ? " and p.rating=" + "@rating" : null;
                    querybuilder += rating != null && anypercent != null ? " and p.rt_per" + percent[0] + "@anypercent" : null;
                    querybuilder += " order by s.short_name";

                    querybuilder += " ',N'@red as varchar(10),@Equity as varchar(50),@structure as varchar(50)";
                    querybuilder += ",@investobj as varchar(50)";
                    querybuilder += nature != null ? " ,@nature as varchar(50)" : null;
                    querybuilder += MinInvst != null ? " ,@MinInvst as float" : null;
                    querybuilder += FundaAge != " " ? " ,@FundaAgeDate as datetime" : null;
                    querybuilder += FundSize != null ? " ,@FundSize as float " : null;
                    querybuilder += period != null && anyrturn != null ? " ,@anyrturn as float" : null;
                    querybuilder += dividend != null ? " ,@dividend as float" : null;
                    querybuilder += nri != null ? " ,@nri as varchar(10)" : null;
                    querybuilder += repartiability != null ? " ,@repartiability as varchar(10)" : null;
                    querybuilder += taxbenefit != null ? " ,@taxbenefit as varchar(10)" : null;
                    querybuilder += rating != null ? " ,@rating as varchar(10)" : null;
                    querybuilder += rating != null && anypercent != null ? " ,@anypercent as varchar(10)" : null;

                    querybuilder += " ','red','Equity%'";
                    querybuilder += ",'" + structure + "','" + investobj + "'";
                    querybuilder += nature != null ? ",'" + nature + "%'" : null;
                    querybuilder += MinInvst != null ? " ," + mininvt[1] : null;
                    querybuilder += FundaAge != " " ? " ,'" + Convert.ToDateTime(FundaAgeDate).ToString("MM/dd/yyyy") + "'" : null;
                    querybuilder += FundSize != null ? "," + fsize[1] : null;
                    querybuilder += period != null && anyrturn != null ? "," + andreturn[1] : null;
                    querybuilder += dividend != null ? " ," + divyield[1] : null;
                    querybuilder += nri != null ? ",'" + nri + "'" : null;

                    querybuilder += repartiability != null ? ",'" + repartiability + "'" : null;
                    querybuilder += taxbenefit != null ? ",'" + taxbenefit + "'" : null;
                    querybuilder += rating != null ? " ,'" + rating + "'" : null;
                    querybuilder += rating != null && anypercent != null ? " ," + percent[1] : null;

                    var cust = indcap.ExecuteQuery<RunQueryFromContext>(querybuilder, "seattle");
                    DataTable tbl = cust.ToDataTable();
                    //tbl = tbl.Where(x => x.closedate < Convert.ToDateTime(FundaAgeDate.ToString("MM/dd/yyyy")));

                    using (DataTable dtadd = new DataTable())
                    {
                        dtadd.Columns.Add("sch_code");
                        dtadd.Columns.Add("mut_code");
                        dtadd.Columns.Add("short_name");
                        dtadd.Columns.Add("type1");
                        dtadd.Columns.Add("type3");
                        dtadd.Columns.Add("navason");
                        dtadd.Columns.Add("nav_rs");
                        dtadd.Columns.Add("fundage");
                        dtadd.Columns.Add("91days", System.Type.GetType("System.String"));
                        dtadd.Columns.Add("1yrs", System.Type.GetType("System.String"));
                        dtadd.Columns.Add("3yrs", System.Type.GetType("System.String"));
                        dtadd.Columns.Add("TopdtAson");
                        foreach (DataRow record in tbl.Rows)
                        {
                            string sqlPerformance = null;
                            DataRow dradd = dtadd.NewRow();
                            dradd[0] = record["sch_code"].ToString();
                            dradd[1] = record["mut_code"].ToString();
                            dradd[2] = record["short_name"].ToString();
                            dradd[3] = record["type1"].ToString();
                            dradd[4] = record["type3"].ToString();

                            var getNav = (from ln in indcap.latest_navs
                                          where ln.Sch_Code == record["sch_code"].ToString()
                                          select new { ln.Date, ln.Nav_Rs }).ToDataTable();
                            if (getNav.Rows.Count != 0)
                            {
                                dradd[5] = Convert.ToDateTime(getNav.Rows[0]["Date"]).ToString("MMM dd,yyyy");
                                dradd[6] = getNav.Rows[0]["Nav_Rs"].ToString();
                            }
                            dradd[7] = FundaAge != " " ? Convert.ToDateTime(record["close_date"]).ToString("MMM dd,yyyy") : "NA";

                            if (period != null && dividend != null)
                            {
                                if (period.Substring(4) == "3yr" || period.Substring(4) == "1yr")
                                {
                                    sqlPerformance = @"select round(isnull(div_Yld,-1),2) col1,round(isnull(per_91days,-1),2) col2,round(isnull(per_1yr,-1),2) col3,round(isnull(per_3yr,-1),2) col4,date 
                            from top_fund ";
                                }
                                else if (period.Substring(4) == "5yr" || period.Substring(4) == "e_incept")
                                {
                                    sqlPerformance = @"select round(isnull(div_Yld,-1),2) col1,round(isnull(per_1yr,-1),2) col2,round(isnull(per_3yr,-1),2) col3,round(isnull(" + period + ",-1),2) col4,date from top_fund ";
                                }
                                else
                                {
                                    sqlPerformance = @"select round(isnull(div_Yld,-1),2) col1,round(isnull(" + period + ",-1),2) col2,round(isnull(per_1yr,-1),2) col3,round(isnull(per_3yr,-1),2) col4,date from top_fund ";

                                }
                            }
                            else if (period == null && dividend != null)
                            {
                                sqlPerformance = @"select round(isnull(div_Yld,-1),2) col1,round(isnull(per_91days,-1),2) col2,round(isnull(per_1yr,-1),2) col3,round(isnull(per_3yr,-1),2) col4 ,
                                date from top_fund ";
                            }

                            else if (period != null && dividend == null)
                            {
                                if (period.Substring(4) == "3yr" || period.Substring(4) == "1yr")
                                {
                                    sqlPerformance = @"select round(isnull(per_91days,-1),2) col1,round(isnull(per_1yr,-1),2) col2,round(isnull(per_3yr,-1),2) col3,date from top_fund ";
                                }
                                else if (period.Substring(4) == "5yr" || period.Substring(4) == "e_incept")
                                {
                                    sqlPerformance = @"select round(isnull(per_1yr,-1),2) col1,round(isnull(per_3yr,-1),2) col2,round(isnull(" + period + ",-1),2) col3,date from top_fund ";
                                }
                                else
                                {
                                    sqlPerformance = @"select round(isnull(" + period + ",-1),2) col1,round(isnull(per_1yr,-1),2) col2,round(isnull(per_3yr,-1),2) col3,date from top_fund ";
                                }
                            }
                            else if (period == null && dividend == null)
                            {
                                sqlPerformance = @"select round(isnull(per_91days,-1),2) col1,round(isnull(per_1yr,-1),2) col2,round(isnull(per_3yr,-1),2) col3,date from top_fund ";
                            }
                            sqlPerformance += "where ltrim(sch_code)='" + record["sch_code"] + "'";
                            var getPerformance = indcap.ExecuteQuery<RunQueryFromContextGetPerformance>(sqlPerformance, "seattle");
                            DataTable tblPerformance = getPerformance.ToDataTable();
                            if (tblPerformance.Rows.Count != 0)
                            {
                                dradd[8] = tblPerformance.Rows[0]["col1"].ToString() != "-1" ? tblPerformance.Rows[0]["col1"].ToString() : "NA";
                                dradd[9] = tblPerformance.Rows[0]["col2"].ToString() != "-1" ? tblPerformance.Rows[0]["col2"].ToString() : "NA";
                                dradd[10] = tblPerformance.Rows[0]["col3"].ToString() != "-1" ? tblPerformance.Rows[0]["col3"].ToString() : "NA";
                                dradd[11] = Convert.ToDateTime(tblPerformance.Rows[0]["date"]).ToString("MMM dd ,yyyy");
                            }
                            dtadd.Rows.Add(dradd);
                        }


                        return dtadd;
                    }


                }


            }


            catch (Exception)
            {
                return null;
            }
        }

        public static DataTable getSectorsNames()
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                dt = (from s in db.GetTable<scheme_Info>()
                            where s.nat_sp.Trim() != ""
                            select new { s.nat_sp }).Distinct().OrderBy(a=>a.nat_sp).ToDataTable();
            }
            return dt;            
        }

        public static DataTable getSimpleEquityScheme(string RadioStructureLst, string DrpNature, string DrpFundAge, string ddlSector, string RadioIncDiv, string DrpMinInvst, string DrpFundSize, string DrpPeriopd, string DrpReturn, string DrpCurDivYield, string RadioListing, string RadioNriInvt, string RadioRepartial, string ChkTaxBenefit1, string ChkTaxBenefit2)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                List<string> Product = new List<string>();
                List<string> ParaType = new List<string>();
                List<string> ParaVal = new List<string>();
                DateTime gDate;
                string fundage = "", ffrom = "", fwhere = "", mfield = "", ffield = "", period = "", perfrom = "", perwhere = "", ssql = "";
                Product.Add("si.type1= @p1");
                ParaType.Add("@p1 varchar(100)");
                ParaVal.Add(RadioStructureLst);

                if (DrpNature.Trim() != "")
                {
                    Product.Add("si.nature= @p2");
                    ParaType.Add("@p2 varchar(100)");
                    ParaVal.Add(DrpNature.Trim());
                }

                if (DrpFundAge.Trim() != "")
                {
                    switch (DrpFundAge.Trim())
                    {
                        case "3month":
                            gDate = DateTime.Today.AddMonths(-3);
                            Product.Add("si.close_date >= @p3");
                            ParaType.Add("@p3 varchar(100)");
                            ParaVal.Add(gDate.ToString());
                            fundage = ",si.close_date ";
                            break;
                        case "6month":
                            gDate = DateTime.Today.AddMonths(-3);
                            Product.Add("si.close_date < @p3");
                            ParaType.Add("@p3 varchar(100)");
                            ParaVal.Add(gDate.ToString());
                            fundage = ",si.close_date ";
                            break;
                        case "1year":
                            gDate = DateTime.Today.AddYears(-1);
                            Product.Add("si.close_date < @p3");
                            ParaType.Add("@p3 varchar(100)");
                            ParaVal.Add(gDate.ToString());
                            fundage = ",si.close_date ";
                            break;
                        case "3year":
                            gDate = DateTime.Today.AddYears(-3);
                            Product.Add("si.close_date < @p3");
                            ParaType.Add("@p3 varchar(100)");
                            ParaVal.Add(gDate.ToString());
                            fundage = ",si.close_date ";
                            break;
                        case "5year":
                            gDate = DateTime.Today.AddYears(-5);
                            Product.Add("si.close_date < @p3");
                            ParaType.Add("@p3 varchar(100)");
                            ParaVal.Add(gDate.ToString());
                            fundage = ",si.close_date ";
                            break;

                    }
                    ffrom = ",fundsize f";
                    fwhere = " and si.sch_code=f.sch_code ";
                }

                if (ddlSector.Trim() != "Any Sector")
                {
                    Product.Add("si.nat_sp= @p4");
                    ParaType.Add("@p4 varchar(100)");
                    ParaVal.Add(ddlSector.Trim());
                }

                if (RadioIncDiv.Trim()!="")
                {
                    Product.Add("si.type3 = @p5");
                    ParaType.Add("@p5 varchar(100)");
                    ParaVal.Add(RadioIncDiv.Trim());
                }

                if (DrpMinInvst.Trim() != "")
                {
                    string[] minInvst = DrpMinInvst.Trim().Split(new char[] { ',' });
                    Product.Add("si.min_invt " + minInvst[0] + "@p6");
                    ParaType.Add("@p6 numeric(18,2)");
                    ParaVal.Add(minInvst[1]);
                    mfield = ",si.min_invt [Min. Invst.]";
                }

                if (DrpFundSize.Trim() != "")
                {
                    string[] fsize = DrpFundSize.Trim().Split(new char[] { ',' });
                    Product.Add("f.fund_size" + fsize[0] + "@p7 and f.date = (select max(date) from fundsize where sch_code = f.sch_code) ");
                    ParaType.Add("@p7 numeric(18,2)");
                    ParaVal.Add(fsize[1]);
                    if (fundage.Trim() == "")
                    {
                        ffrom = ",fundsize f";
                        fwhere = " and si.sch_code=f.sch_code ";
                        ffield = ",f.fund_size [Fund Size (Cr.)]";
                    }
                    else
                    { ffield = ",f.fund_size [Fund Size (Cr.)]"; }
                }
                else
                {
                    ffrom = "";
                    fwhere = "";
                    ffield = "";
                }

                if (DrpPeriopd.Trim() != "")
                {
                    period = "t." + DrpPeriopd.Trim();
                    perfrom = ",top_fund t";
                    perwhere = " and si.sch_code=t.sch_code ";

                    if (DrpReturn.Trim() != "")
                    {
                        string[] retn = DrpReturn.Trim().Split(new char[] { ',' });
                        Product.Add(DrpPeriopd.Trim() + retn[0] + "@p9");
                        ParaType.Add("@p9 numeric(18,2)");
                        ParaVal.Add(retn[1]);
                    }


                }
                else
                { period = ""; }

                if (DrpCurDivYield.Trim() != "")
                {
                    perfrom = ",top_fund t";
                    perwhere = " and si.sch_code=t.sch_code ";
                    string[] Curdiv = DrpCurDivYield.Trim().Split(new char[] { ',' });
                    Product.Add(" t.div_yld " + Curdiv[0] + "@p10");
                    ParaType.Add("@p10 numeric(18,2)");
                    ParaVal.Add(Curdiv[1]);
                }

                if (RadioListing.Trim()!="")
                {
                    if (RadioListing == "Yes")
                        Product.Add("ltrim(rtrim(si.listing)) <>''''");
                    else
                        Product.Add("si.listing = ''''");
                }

                if (RadioNriInvt.Trim()!="")
                    Product.Add(" si.nri = ''" + RadioNriInvt.Trim() + "''");

                if (RadioRepartial.Trim()!="")
                    Product.Add("si.repatri = ''" + RadioRepartial.Trim() + "''");

                
                    if (ChkTaxBenefit1.Trim() != "" && ChkTaxBenefit1 == "88")
                        Product.Add("si.tax_ben1 = ''" + ChkTaxBenefit1 + "''");
                    if (ChkTaxBenefit2.Trim() != "" && ChkTaxBenefit2 == "112")
                        Product.Add("si.tax_ben3 = ''" + ChkTaxBenefit2 + "''");
                

                ssql = "exec sp_executesql N'select distinct si.sch_code,si.Short_name,";
                ssql += "si.type1 ,si.type3 ";
                ssql += ffield + mfield + fundage;
                ssql += " from scheme_info si,mut_fund mf" + perfrom + ffrom;
                ssql += " where si.mut_code =mf.mut_code and si.nav_check<>''red'' and si.nature like ''Equity%''";
                ssql += perwhere + fwhere;
                foreach (string s in Product)
                {
                    ssql += " and " + s;
                }
                ssql += " order by si.short_name',N'";
                foreach (string s in ParaType)
                {
                    ssql += s + ",";
                }
                ssql = ssql.TrimEnd(',');
                ssql += "',";
                foreach (string s in ParaVal)
                {
                    if (IsDouble(s))
                        ssql += s + ",";
                    else
                        ssql += "'" + s + "',";
                }
                ssql = ssql.TrimEnd(',');
                var cust = db.ExecuteQuery<RunQueryFromContext>(ssql, "");
                dt = cust.ToDataTable();
            }
            DataTable returnDt=getSimpleEquitySchemeCalculation(dt);
            return returnDt;
        }

        static bool IsDouble(string theValue)
        {
            try
            {
                Convert.ToDouble(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable getSimpleEquitySchemeCalculation(DataTable rawData)
        {
            DataTable dt = new DataTable();            
            using (var db = new iFramesDataContext())
            {
                var data = from l in db.latest_navs                          
                           from t in db.top_funds
                           where l.Sch_Code == t.sch_code
                           select new { l.Sch_Code, 
                               LDate=l.Date,
                               l.Nav_Rs, 
                               t.per_3yr,
                               TDate=t.date,
                               t.div_yld,
                               t.nature,
                               t.per_15days,
                               t.per_182days,
                               t.per_1yr,
                               t.per_30days,
                               t.per_5yr,
                               t.per_7days,
                               t.per_91days,
                               t.since_incept };
                dt = data.ToDataTable();               

            }
            foreach (DataColumn col1 in dt.Columns)
            {
                if (col1.ColumnName != "Sch_Code")
                    rawData.Columns.Add(col1.ColumnName, System.Type.GetType("System.String"));
            }            
            foreach (DataRow dr in rawData.Rows)
            {                
                DataRow[] drs = dt.Select("Sch_Code='" + dr["Sch_Code"].ToString() + "'", "");

                foreach (DataColumn col1 in dt.Columns)
                {
                    if (col1.ColumnName != "Sch_Code")
                        dr[col1.ColumnName] = drs[0][col1.ColumnName].ToString();
                }
            }
            rawData.AcceptChanges();

            return rawData;
        }


    }

    public class RunQueryFromContext
    {
        public string sch_code { get; set; }
        public string mut_code { get; set; }
        public string short_name { get; set; }
        public string type1 { get; set; }
        public string type3 { get; set; }
        public double fund_size { get; set; }
        public double min_invt { get; set; }
        public DateTime close_date { get; set; }
        public string rt_per { get; set; }

    }

    public class RunQueryFromContextGetPerformance
    {
        public double col1 { get; set; }
        public double col2 { get; set; }
        public double col3 { get; set; }
        public double col4 { get; set; }
        public DateTime date { get; set; }
    }

    public class SchemeInfo
    {
        public string Sch_Short_Name { get; set; }
        public decimal SCHEME_ID { get; set; }
        public DateTime? Launch_Date { get; set; }
        public decimal? Sebi_Nature_ID { get; set; }
        public string SEBI_NATURE { get; set; }
        public decimal? Sebi_Sub_Nature_ID { get; set; }
        public string SEBI_SUB_NATURE { get; set; }
        public decimal MutualFund_ID { get; set; }
        public decimal Options { get; set; }
        public decimal Structure_ID { get; set; }
        public string BENCHMARK { get; set; }
        public decimal INDEX_ID { get; set; }
    }

}