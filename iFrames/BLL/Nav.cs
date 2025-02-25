using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iFrames.DAL;
namespace iFrames.BLL
{
    public class Nav
    {
        static public DataTable Getdtscheme(string mutcode)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    
                    var tbl=        
                     from s in indcap.GetTable<scheme_Info>()
                     where s.nav_check != "red" && s.mut_code == mutcode                       
                   
                             orderby  s.short_name
                     select new {s.sch_code,s.short_name};
                    return tbl.ToDataTable();
                }
              }
                catch (Exception)
                {
                    return null;
                }
            }

        static public DataTable GetdtFund(string MutcodeFromBase = "")
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    List<string> arr = new List<string>();
                    if (MutcodeFromBase != "")
                    {
                        string[] path = MutcodeFromBase.Split(new char[] { ',' });
                        for (int i = 0; i < path.Length; i++)
                        {
                            arr.Add(path[i]);
                        }
                    }

                    var tbl = from m in indcap.mut_funds
                              where
                            ((from s in indcap.GetTable<scheme_Info>()
                              where (MutcodeFromBase != "" ?arr.Contains(m.Mut_Code):true) && s.nav_check != "red"
                              select s.mut_code).Distinct()).Contains(m.Mut_Code)
                              orderby m.Mut_Name
                              select new { m.Mut_Code, m.Mut_Name };
                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtNav(string schcode)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {

                    var tbl = (from t in indcap.top_funds
                              from l in indcap.latest_navs
                              where t.sch_code == l.Sch_Code && t.sch_code == schcode
                              select new { date = l.Date, navrs = l.Nav_Rs
                              ,ninetyonedays=t.per_91days
                              ,oneeighttwodays = t.per_182days
                              ,oneyr = t.per_1yr}).ToDataTable();
                    var tbl1 = (from t in indcap.top_funds
                                select new { t.date }).Max(tt => tt.date);
                   
                    tbl.Columns.Add("maxdt");
                    tbl.Rows[0]["maxdt"] = tbl1.ToString();
                    return tbl;//.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtCloseyr(string schcode)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {

                    var tbl = (from s in indcap.GetTable<scheme_Info>()
                              where s.nav_check == "yes" && s.sch_code == schcode
                              select new { s.close_date.Value.Year}).ToDataTable();
                   
                    return tbl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtHistNav(string schcode,string dtminus,string dtadd)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {

                    var tbl = (from n in indcap.nav_news
                               where n.Sch_Code == schcode && n.Date >= Convert.ToDateTime(dtminus) && n.Date <= Convert.ToDateTime(dtadd)
                               orderby n.Date
                               select new { n.Sch_Code,n.Date,n.Nav_Rs}).Distinct();

                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtHistNav(string schcode, DateTime dtminus, DateTime dtadd)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {

                    var tbl = (from n in indcap.nav_news
                               where n.Sch_Code == schcode && n.Date >= dtminus && n.Date <= dtadd
                               orderby n.Date
                               select new { n.Sch_Code, n.Date, n.Nav_Rs }).Distinct();

                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtFundList(string nature1 = "", string subnature1 = "", string nat_sp = "", string MutcodeFromBase = "")
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var tbl = new DataTable();
                    List<string> arr = new List<string>();
                    if (MutcodeFromBase != "")
                    {
                        string[] path = MutcodeFromBase.Split(new char[] { ',' });
                        for (int i = 0; i < path.Length; i++)
                        {
                            arr.Add(path[i]);
                        }
                    }
                    if(subnature1!="")
                    {
                          tbl = (from s in indcap.GetTable<scheme_Info>()
                                  from sc in indcap.sch_nature_details
                                 where (MutcodeFromBase != "" ? arr.Contains(s.mut_code) : true) && s.sch_code == sc.sch_code && sc.nature1 == nature1 && sc.subnature1 == subnature1
                                  && s.variant != "DP" && s.nav_check != "red"
                                  orderby s.short_name
                                  select new {s.sch_code, s.short_name}).ToDataTable();
                       
                     }
                    if (nature1 != "" && subnature1 == "")
                    {
                        if (nature1 == "etf")
                        {
                            tbl = (from s in indcap.GetTable<scheme_Info>()
                                   from sc in indcap.sch_nature_details
                                   where (MutcodeFromBase != "" ? arr.Contains(s.mut_code) : true) &&  s.sch_code == sc.sch_code && sc.nature1 == nature1 && s.nat_sp == ""
                                   && s.variant != "DP" && s.nav_check != "red"
                                   orderby s.short_name
                                   select new { s.sch_code, s.short_name }).ToDataTable();
                        }
                        else 
                        {
                            tbl = (from s in indcap.GetTable<scheme_Info>()
                                   from sc in indcap.sch_nature_details
                                   where (MutcodeFromBase != "" ? arr.Contains(s.mut_code) : true) && s.sch_code == sc.sch_code && sc.nature1 == nature1
                                   && s.variant != "DP" && s.nav_check != "red"
                                   orderby s.short_name
                                   select new { s.sch_code, s.short_name }).ToDataTable();

                        }
                        
                    }
                    
                    else if (nature1=="")
                    {
                         tbl = (from s in indcap.GetTable<scheme_Info>()
                                  from sc in indcap.sch_nature_details
                                where (MutcodeFromBase != "" ? arr.Contains(s.mut_code) : true) && s.sch_code == sc.sch_code
                                  && s.variant != "DP" && s.nav_check != "red"
                                  orderby s.short_name
                                  select new { s.sch_code, s.short_name }).ToDataTable();
                        
                    }

                    return tbl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        static public DataTable GetdtIndex()
        {
            try
            {
                using (var mfiexplorer = new iFramesExplorerDataContext())
                {
                    var tbl = (from n in mfiexplorer.ind_infoExplorers
                               select new { n.ind_name, n.ind_code });

                    return tbl.ToDataTable();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetdtFundComp(string sname,string exchange)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    //string st = "AX010,AX011,BB014,";
                    string[] path = sname.Split(new char[] { ',' });
                    List<string> arr = new List<string>();
                    for (int i = 0; i < path.Length; i++)
                    {
                        arr.Add(path[i]);
                    }
                    var tbl = (from s in indcap.GetTable<scheme_Info>()
                              from l in indcap.latest_navs
                              from t in indcap.top_funds
                              where t.date ==
                              ((from cc in indcap.top_funds
                                where cc.sch_code == t.sch_code
                                select new { cc.date }).Max(tt => tt.date)) && t.sch_code == s.sch_code && s.sch_code == l.Sch_Code
                                && (arr).Contains(s.sch_code)
                              orderby t.per_1yr
                              select new { s.short_name,
                                           per_30days = (Double?)Math.Round((double)t.per_30days, 2, MidpointRounding.AwayFromZero),
                                per_91days=(Double?)Math.Round((double)t.per_91days, 2, MidpointRounding.AwayFromZero),
                                per_182days=(Double?)Math.Round((double)t.per_182days, 2, MidpointRounding.AwayFromZero),
                                per_1yr=(Double?)Math.Round((double)t.per_1yr, 2, MidpointRounding.AwayFromZero),
                                per_3yr=(Double?)Math.Round((double)t.per_3yr, 2, MidpointRounding.AwayFromZero),
                                Nav_Rs=(Double?)Math.Round((double)l.Nav_Rs, 2, MidpointRounding.AwayFromZero), 
                                s.type1, s.nature, nature1 = t.nature}).ToDataTable();
                    tbl.Columns.Add("status");
                    foreach (DataRow dr in tbl.Rows)
                    {
                        dr["status"] = "1";
                    }


                    var tbl2 = (from ttt in (from s in indcap.GetTable<scheme_Info>()
                               from l in indcap.latest_navs
                               from t in indcap.top_funds
                               where t.date ==
                               ((from cc in indcap.top_funds
                                 where cc.sch_code == t.sch_code
                                 select new { cc.date }).Max(tt => tt.date)) && t.sch_code == s.sch_code && s.sch_code == l.Sch_Code
                                 && (arr).Contains(s.sch_code)

                               select new
                               {
                                   t.per_30days,t.per_91days,  t.per_182days, t.per_1yr,t.per_3yr,l.Nav_Rs,
                                   Dummy = "x"
                               }) group ttt by new { ttt.Dummy } into g
                               select new
                               {
                                   per_30days = (Double?)Math.Round((double)g.Average(p => p.per_30days), 2, MidpointRounding.AwayFromZero),
                                   per_91days = (Double?)Math.Round((double)g.Average(p => p.per_91days), 2, MidpointRounding.AwayFromZero),
                                   per_182days = (Double?)Math.Round((double)g.Average(p => p.per_182days), 2, MidpointRounding.AwayFromZero),
                                   per_1yr = (Double?)Math.Round((double)g.Average(p => p.per_1yr), 2, MidpointRounding.AwayFromZero),
                                   per_3yr = (Double?)Math.Round((double)g.Average(p => p.per_3yr), 2, MidpointRounding.AwayFromZero),
                                   Nav_Rs = (Double?)Math.Round((double)g.Average(p => p.Nav_Rs), 2, MidpointRounding.AwayFromZero)
                               }).ToDataTable();

                    DataRow dradd = tbl.NewRow();
                    dradd["short_name"] = "Average performance of similar category funds";
                    dradd["per_30days"] = tbl2.Rows.Count != 0 && tbl2 != null ? tbl2.Rows[0]["per_30days"] : null;
                    dradd["per_91days"] = tbl2.Rows.Count != 0 && tbl2 != null ? tbl2.Rows[0]["per_91days"] : null;                 
                    dradd["per_182days"] = tbl2.Rows.Count != 0 && tbl2 != null ? tbl2.Rows[0]["per_182days"] : null;
                    dradd["per_1yr"] = tbl2.Rows.Count != 0 && tbl2 != null ? tbl2.Rows[0]["per_1yr"] : null;
                    dradd["per_3yr"] = tbl2.Rows.Count != 0 && tbl2 != null ? tbl2.Rows[0]["per_3yr"] : null;
                    dradd["Nav_Rs"] = tbl2.Rows.Count != 0 && tbl2 != null ? tbl2.Rows[0]["Nav_Rs"] : null;
                    dradd["type1"] = "---";
                    dradd["nature"] = "---";
                    dradd["status"] = "2";
                    tbl.Rows.Add(dradd);

                    var mfiexplorer = new iFramesExplorerDataContext();
                    string[] indcode = exchange.Split(new char[] { ',' });
                    List<string> arrind = new List<string>();
                    for (int i = 0; i < indcode.Length; i++)
                    {
                        arrind.Add(indcode[i]);
                    }
                    var tbl3 = (from t in mfiexplorer.top_fund_Indexes
                               from i in mfiexplorer.ind_infoExplorers
                               where t.ind_code == i.ind_code && (arrind).Contains(t.ind_code)
                               select new { i.ind_name, dt = Convert.ToDateTime(t.date).AddDays(276),
                                   per_30days = (t.per_30days - 53) / 76,  per_91days = (t.per_91days - 53) / 76,
                                   per_182days = (t.per_182days - 53) / 76, per_1yr = (t.per_1yr - 53) / 76,
                                   per_3yr = (t.per_3yr - 53) / 76}).ToDataTable();
                    foreach (DataRow record in tbl3.Rows)
                    {
                        
                         dradd = tbl.NewRow();
                         dradd["short_name"] = tbl3.Rows.Count != 0 && tbl3 != null ? record["ind_name"] : null;
                         dradd["per_30days"] = tbl3.Rows.Count != 0 && tbl3 != null ? (Double?)Math.Round((double)record["per_30days"], 2, MidpointRounding.AwayFromZero) : null;
                         dradd["per_91days"] = tbl3.Rows.Count != 0 && tbl3 != null ? (Double?)Math.Round((double)record["per_91days"], 2, MidpointRounding.AwayFromZero) : null;
                         dradd["per_182days"] = tbl3.Rows.Count != 0 && tbl3 != null ? (Double?)Math.Round((double)record["per_182days"], 2, MidpointRounding.AwayFromZero) : null;
                         dradd["per_1yr"] = tbl3.Rows.Count != 0 && tbl3 != null ? (Double?)Math.Round((double)record["per_1yr"], 2, MidpointRounding.AwayFromZero) : null;
                         dradd["per_3yr"] = tbl3.Rows.Count != 0 && tbl3 != null ? (Double?)Math.Round((double)record["per_3yr"], 2, MidpointRounding.AwayFromZero) : null;
                        
                        dradd["type1"] = "---";
                        dradd["nature"] = "---";
                        dradd["status"] = "3";
                        tbl.Rows.Add(dradd);
                       
                    }
                    return tbl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetNavDetails(string schCode, DateTime fromDate)
        {
          try
          {
            using (var indcap = new iFramesDataContext())
            {

              var IssueDt = (from c in indcap.GetTable<scheme_Info>()
                                  where c.mut_code == "MF024" && c.nav_check != "red" && c.type1 == "open ended"
                                  orderby c.short_name
                                  select new { c.sch_code, c.short_name });

            }










            using (var indcap = new iFramesDataContext())
            {
              var dtSchDetails = (from c in indcap.GetTable<scheme_Info>()
                                  where c.mut_code == "MF024" && c.nav_check != "red" && c.type1 == "open ended"
                                  orderby c.short_name
                                  select new { c.sch_code, c.short_name });

              return dtSchDetails.ToDataTable();
            }
          }
          catch (Exception)
          {
            return null;
          }
        }
     }
    }
