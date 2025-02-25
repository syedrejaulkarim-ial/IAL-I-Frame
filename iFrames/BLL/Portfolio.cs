using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iFrames.DAL;

namespace iFrames.BLL
{
    public class Portfolio
    {

        static public DataTable GetdtPortDetail(string sname)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {

                    bool f1 = false; bool f2 = false;

                    DataTable tbl1 = new DataTable();
                    DataTable tbl2 = new DataTable();
                    DataTable tbl4 = new DataTable();
                    DataTable tbl5 = new DataTable();
                    
                    var tbl3=(from th in indcap.top_holds
                              where th.Date==
                              ((from thh in indcap.top_holds
                               where thh.Sch_Code == sname
                               select new { thh.Date }).Max(tt => tt.Date)) && th.Sch_Code==sname
                               select new { th.Date}).ToDataTable();

                    tbl5 = (from ip in indcap.Invt_Pats
                            where ip.Date ==
                        ((from nc in indcap.net_com_pots
                          where nc.Sch_Code == sname
                          select new { nc.Date }).Max(tt => tt.Date)) && ip.sch_code == sname
                            select new { ip.sch_code, ip.Date, ip.Equity, ip.Debt, ip.Cas_MonM }).ToDataTable();

                    tbl2 = (from f in indcap.FundSizes
                            where f.Date ==
                         ((from fs in indcap.net_com_pots
                           where fs.Sch_Code == sname
                           select new { fs.Date }).Max(tt => tt.Date)) && f.sch_Code == sname
                            select new { f.sch_Code, f.Date, f.fund_Size }).ToDataTable();

                    
                    if (tbl3.Rows.Count != 0 && tbl3 != null)
                    {
                         tbl4 = (from ip in indcap.Invt_Pats
                          where ip.Date ==
                      ((from nc in indcap.Invt_Pats
                        where nc.sch_code == sname
                        select new { nc.Date }).Max(tt => tt.Date)) && ip.sch_code == sname
                          select new { ip.sch_code, ip.Date, ip.Equity, ip.Debt, ip.Cas_MonM }).ToDataTable();

                         tbl1 = (from f in indcap.FundSizes
                                 where f.Date ==
                              ((from fs in indcap.FundSizes
                                where fs.sch_Code == sname
                                select new { fs.Date }).Max(tt => tt.Date)) && f.sch_Code == sname
                                 select new { f.sch_Code, f.Date, f.fund_Size }).ToDataTable();


                    }
                   


                    if (tbl2.Rows.Count != 0 && tbl2 != null)
                    {
                        if (tbl2.Rows[0]["Date"] != tbl3.Rows[0]["Date"])
                        {
                            f1 = true;
                        }
                    }
                    if (f1 != true && tbl3.Rows.Count != 0 && tbl3 != null)
                    {
                        f2 = true;
                    }

                   
                        using (DataTable dtadd = new DataTable())
                        {
                            dtadd.Columns.Add("fsizeAson");
                            dtadd.Columns.Add("fund_Size");
                            dtadd.Columns.Add("AsssetAson");
                            dtadd.Columns.Add("Equity");
                            dtadd.Columns.Add("Debt");
                            dtadd.Columns.Add("Others");
                            DataRow dradd = dtadd.NewRow();
                            if (f1 == true)
                            {
                                dradd[0] = (tbl2.Rows.Count != 0 && tbl2 != null ? tbl2.Rows[0]["Date"] : tbl3.Rows[0]["Date"]);
                                dradd[1] = (tbl2.Rows.Count != 0 && tbl2 != null ? tbl2.Rows[0]["fund_Size"] : null);
                                dradd[2] = (tbl5.Rows.Count != 0 && tbl5 != null ? tbl5.Rows[0]["Date"] : null);
                                dradd[3] = (tbl5.Rows.Count != 0 && tbl5 != null ? tbl5.Rows[0]["Equity"] : null);
                                dradd[4] = (tbl5.Rows.Count != 0 && tbl5 != null ? tbl5.Rows[0]["Debt"] : null);
                                dradd[5] = (tbl5.Rows.Count != 0 && tbl5 != null ? tbl5.Rows[0]["Cas_MonM"] : null);
                                dtadd.Rows.Add(dradd);
                            }
                            else if (f2 == true)
                            {
                                dradd[0] = (tbl1.Rows.Count != 0 && tbl1 != null ? tbl1.Rows[0]["Date"] : null);
                                dradd[1] = (tbl1.Rows.Count != 0 && tbl1 != null ? tbl1.Rows[0]["fund_Size"] : null);
                                dradd[2] = (tbl4.Rows.Count != 0 && tbl4 != null ? tbl4.Rows[0]["Date"] : null);
                                dradd[3] = (tbl4.Rows.Count != 0 && tbl4 != null ? tbl4.Rows[0]["Equity"] : null);
                                dradd[4] = (tbl4.Rows.Count != 0 && tbl4 != null ? tbl4.Rows[0]["Debt"] : null);
                                dradd[5] = (tbl4.Rows.Count != 0 && tbl4 != null ? tbl4.Rows[0]["Cas_MonM"] : null);
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


        static public DataTable GetTopTenHoldDetail(string sname)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var tbl = (from th in indcap.top_holds
                                where th.Date ==
                                ((from thh in indcap.top_holds
                                  where thh.Sch_Code == sname
                                  select new { thh.Date }).Max(tt => tt.Date)) && th.Sch_Code == sname && th.Nature!=null
                                  orderby th.Nature,th.corpus_per descending,th.Mkt_Value descending
                                select new { th.Date,th.Nature,th.Coupon_Rate,th.c_name,th.mt_date_txt,
                                th.Instrument,th.Rating,th.No_of_shares,th.Mkt_Value,
                                th.corpus_per}).ToDataTable();
                    return tbl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        static public DataTable GetCompletePortDetail(string sname)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var tbl = (from th in indcap.top_holds
                               where th.Date ==
                               ((from ncp in indcap.net_com_pots
                                 where ncp.Sch_Code == sname
                                 select new { ncp.Date }).Max(tt => tt.Date)) && th.Sch_Code == sname && th.Nature != null
                               orderby th.Nature, th.corpus_per descending, th.Mkt_Value descending
                               select new
                               {
                                   th.Date,
                                   th.Nature,
                                   th.Coupon_Rate,
                                   th.c_name,
                                   th.mt_date_txt,
                                   th.Instrument,
                                   th.Rating,
                                   th.No_of_shares,
                                   th.Mkt_Value,
                                   th.corpus_per
                               }).ToDataTable();
                    return tbl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable GetShemeDetail(string sname)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var tbl = (from s in indcap.GetTable<scheme_Info>()
                               where s.sch_code == sname 
                               select new
                               {
                                   s.sch_name,s.nature
                               }).ToDataTable();
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