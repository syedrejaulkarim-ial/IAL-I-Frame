using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iFrames.DAL;
namespace iFrames.BLL
{
    public class Aum
    {
        static public DataTable GetdtAumAll(string MutcodeFromBase = "")
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    using (DataTable dtadd = new DataTable())
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
                        dtadd.Columns.Add("Mut_Code");
                        dtadd.Columns.Add("Mut_Name");
                        dtadd.Columns.Add("Total");
                        dtadd.Columns.Add("Date");
                        dtadd.Columns.Add("Corpus_Crs_");
                        dtadd.Columns.Add("Investor_base");
                        dtadd.Columns.Add("amc_code");
                        dtadd.Columns.Add("secondcorpus");
                        dtadd.Columns.Add("seconddate");
                        dtadd.Columns.Add("secondmutcode");
                        dtadd.Columns.Add("netnic");
                        

                    var tbl =((from s in indcap.GetTable<scheme_Info>()
                              from m in indcap.mut_funds
                              from c in indcap.AverageCorpus
                              where (MutcodeFromBase != "" ?arr.Contains(m.Mut_Code):true) &&c.Date ==
                              ((from cc in indcap.AverageCorpus

                                where cc.Mut_code==m.Mut_Code 
                                select new { cc.Date }).Max(tt => tt.Date)) && s.mut_code == m.Mut_Code &&
                                c.Mut_code == m.Mut_Code && s.nav_check != "red"
                                
                              select new { m.Mut_Code,m.Mut_Name,c.Date,c.Corpus_Crs_,c.Investor_base,
                                           s.amc_code,
                                           }).Distinct().OrderBy(t => t.Mut_Name)).ToDataTable();
                    foreach (DataRow record in tbl.Rows)
                    {
                        DataRow dradd = dtadd.NewRow();
                        double lat_fs = 0.0; double prev_fs = 0.0;  double netinc=0.0;
                        var tbl1 = ((from a in indcap.AverageCorpus
                                     where a.Mut_code == record["Mut_Code"].ToString()
                                     orderby a.Mut_code, a.Date descending
                                     select new
                                     {
                                         secondcorpus = a.Corpus_Crs_,
                                         seconddate = a.Date,
                                         secondmutcode = a.Mut_code
                                     }).Take(2)).ToDataTable();
                        if (tbl1.Rows.Count <3 && tbl1!=null)
                        {
                            lat_fs = tbl1.Rows.Count > 0 ? double.Parse(tbl1.Rows[0]["secondcorpus"].ToString()) : lat_fs;
                            prev_fs = tbl1.Rows.Count > 1 ? double.Parse(tbl1.Rows[1]["secondcorpus"].ToString()) : prev_fs;
                            netinc = lat_fs - prev_fs;
                        }

                        var tbl3 = (from s in indcap.GetTable<scheme_Info>()
                                    where s.nav_check != "red" && s.mut_code == record["Mut_Code"].ToString()
                                    select new { s.mut_code }).Count();

                        dradd["Mut_Code"] = (record["Mut_Code"].ToString());
                        dradd["Mut_Name"] = (record["Mut_Name"].ToString());
                        dradd["Total"] = tbl3.ToString();
                        dradd["Date"] = (record["Date"].ToString());
                        dradd["Corpus_Crs_"] = record["Corpus_Crs_"].ToString()!="" ?(Double?)Math.Round(double.Parse(record["Corpus_Crs_"].ToString()),2, MidpointRounding.AwayFromZero):null;
                        dradd["Investor_base"] = (record["Investor_base"].ToString());
                        dradd["amc_code"] = (record["amc_code"].ToString());
                        dradd["secondcorpus"] = tbl1.Rows.Count>1 && tbl1.Rows[1]["secondcorpus"].ToString()!="" ?(Double?)Math.Round(double.Parse(prev_fs.ToString()), 2, MidpointRounding.AwayFromZero):null;
                        dradd["seconddate"] = (tbl1.Rows[1]["seconddate"].ToString());
                        dradd["secondmutcode"] = (tbl1.Rows[1]["secondmutcode"].ToString());
                        dradd["netnic"] =tbl1.Rows.Count > 0 ? netinc.ToString() :null;
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

        static public DataTable GetdtAumMutData()
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var tbl = (from a in indcap.AUMs
                              where a.Date ==
                             (from aa in indcap.AUMs
                              select new { aa.Date }).Max(tt => tt.Date) && a.Sector!="Foreign"
                              select new
                              {
                                  a.Date,
                                  a.Sector,
                                  a.New_Launched_Sales,
                                  a.New_Schemes_Sales,
                                  a.Exist_Schemes_Sales,
                                  a.Redemption,
                                  a.AUM1
                                  }).ToDataTable();

                    tbl.Columns.Add("prevdt");
                    tbl.Columns.Add("TotalSales", System.Type.GetType("System.Int64"));
                    tbl.Columns.Add("AUMPREV", System.Type.GetType("System.Int64"));
                    tbl.Columns.Add("IOflow", System.Type.GetType("System.Int64"));
                    tbl.Columns.Add("status");
                    var prevdt = (from a in indcap.AUMs
                                 where a.Date ==
                                 ((from aa in indcap.AUMs
                                   where aa.Date != Convert.ToDateTime(tbl.Rows[0]["Date"])
                                   select new { aa.Date }).Max(tt => tt.Date))
                                 select a.Date).ToDataTable();
                    foreach (DataRow record in tbl.Rows)
                    {
                        
                            var tbl1 = (from a in indcap.AUMs
                                        where a.Sector == record["Sector"].ToString() && a.Date == Convert.ToDateTime(prevdt.Rows[0]["Date"])
                                        select new { a.AUM1 }).ToDataTable();
                            record["TotalSales"] = Convert.ToInt64(record["New_Schemes_Sales"]) +
                                Convert.ToInt64(record["Exist_Schemes_Sales"]);
                            
                            record["prevdt"] = Convert.ToDateTime(prevdt.Rows[0]["Date"]).ToString("MMM dd,yyyy");

                            if (record["Sector"].ToString() == "Unit Trust of India") { record["status"] = "A"; }
                            else if (record["Sector"].ToString() == "Bank Sponsored") { record["status"] = "B"; }
                            else if (record["Sector"].ToString() == "Institutions") { record["status"] = "C"; }
                            else { record["status"] = "D"; }

                            if (tbl1.Rows.Count != 0 && tbl1 != null)
                            {
                                record["AUMPREV"] = tbl1.Rows[0]["AUM1"].ToString();
                                record["IOflow"] = (Convert.ToInt64(record["AUM1"]) -
                                Convert.ToInt64(tbl1.Rows[0]["AUM1"])).ToString();
                            }
                            else
                            {
                                record["AUMPREV"] =null;
                                record["IOflow"] = (Convert.ToInt64(record["AUM1"])).ToString();
                            }
                        
                    }
                               
                    return tbl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetNatureAum()
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {

                    var tbl = ((from a in indcap.amfis                               
                               where a.date ==
                               ((from aa in indcap.amfis
                                 select new { aa.date }).Max(tt => tt.date)) && a.type== "Open-End"
                               select new { a.type,a.date,a.nature,openaum=Convert.ToInt64(a.aum)}).Distinct()).ToDataTable();

                    tbl.Columns.Add("closeaum",System.Type.GetType("System.Int64"));
                    DataColumn dc =new  DataColumn();                    
                    dc.DataType = System.Type.GetType("System.Int64");
                    dc.ColumnName = "Total";
                    
                    var tbl1 = ((from a in indcap.amfis
                                where a.date ==
                                ((from aa in indcap.amfis
                                  select new { aa.date }).Max(tt => tt.date)) && a.type == "Close-End"
                                select new { a.type, a.date, a.nature, a.aum }).Distinct()).ToDataTable();
                    int cnt = 0;
                    foreach (DataRow drrow in tbl1.Rows)
                    {
                        if (tbl.Rows[cnt]["nature"].ToString() == drrow["nature"].ToString())
                        {
                            tbl.Rows[cnt]["closeaum"] = Convert.ToInt64(drrow["aum"]);
                        }
                        cnt++;
                    }
                    
                    //dc.Expression = "openaum +closeaum";
                    dc.Expression = "openaum +closeaum";

                    tbl.Columns.Add(dc);

                    return tbl;          
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}