using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFrames.DAL;

namespace iFrames.BLL
{
    public class FactSheets
    {
        public static DataTable getFactsheet1(string schCode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesExplorerDataContext())
            {
                var ratio = (from p in db.ptratios
                             where p.Sch_code == schCode
                             select p.Ratio).Max();

                var maxDate = (from f in db.GetTable<FundSize>()
                               where f.sch_Code == schCode
                               select f.Date).Max();

                var fundCode = from cf in db.cur_fund_manExplorers
                               where cf.sch_code == schCode && cf.flg_ActInact == true
                               select cf.fund_code;

                var IncpDate = (from l in db.sch_launches
                                where l.sch_code == schCode
                                select l.launch_dt.Value.AddDays(276)).ToDataTable();

                DataTable ER = (from e in db.expenses
                                where e.sch_code == schCode && e.date_from == (from ex in db.expenses where ex.sch_code == schCode select ex.date_from).Max()
                                select new { ap = e.actual_per == null ? 0.00 : e.actual_per }).ToDataTable();

                var ExpenseRatio =ER.Rows.Count>0? Math.Round((Convert.ToDouble(ER.Rows[0]["ap"]) - 53) / 76, 2):0.00;

                DataTable FVal = (from s in db.GetTable<scheme_Info>()
                                  where s.sch_code == schCode
                                  select new { fv = s.face_val == null ? 0.00 : s.face_val }).ToDataTable();

                DataTable dtFundSize = ((from f in db.GetTable<FundSize>()
                                         where f.sch_Code == schCode
                                         select new { fd = f.fund_Size == null ? 0.00 : f.fund_Size, f.Date }).OrderByDescending(s => s.Date)).ToDataTable();

                var IncDecFundSize = dtFundSize.Rows.Count < 1 ? "NA" : (Math.Round(Convert.ToDouble(dtFundSize.Rows[0]["fd"]) - Convert.ToDouble(dtFundSize.Rows[1]["fd"]), 2)).ToString();
                IncDecFundSize = IncDecFundSize + "  (Quartely Avg. Diff.) ";

                DataTable divRec = (from d in db.GetTable<div_rec>()
                                    where d.Sch_code == schCode
                                    select new { d.Date, Divid_pt = d.Divid_pt == null ? 0 : d.Divid_pt }).ToDataTable();
                var DivDate = divRec.Rows.Count < 1 ? "" : Convert.ToDateTime(divRec.Rows[0]["Date"].ToString()).ToString("MMM dd, yyyy");

                var data = from si in db.scheme_InfoExplorers
                           from mf in db.MUT_FUNDs
                           from sc in db.Scheme_Info_Additionals
                           from f in db.GetTable<FundSize>()
                           from a in db.GetTable<AMC>()
                           from r in db.GetTable<REGIST>()
                           where si.mut_code == mf.Mut_Code && si.sch_code == sc.Sch_Code && si.sch_code == f.sch_Code
                                 && si.amc_code == a.AMC_Code && r.Reg_Code == si.reg_code
                               && f.Date == maxDate // && mf.Mut_Code == fm.Mut_Code && fundCode.Contains(fm.Fund_Code)
                               && si.sch_code == schCode
                           select new
                           {
                               a.AMC_Name,
                               amcAddress1 = a.Address1 == null ? "" : a.Address1,
                               amcAddress2 = a.Address2 == null ? "" : a.Address2,
                               amcCity = a.City == null ? "" : a.City,
                               amcPin = a.Pin == null ? "" : a.Pin,
                               amcPhone1 = a.Phone1 == null ? "" : a.Phone1,
                               si.amc_code,
                               si.sch_code,
                               si.reg_code,
                               si.sch_name,
                               si.@object,
                               si.type1,
                               si.nature,
                               si.type3,
                               si.close_date,
                               si.min_invt,
                               si.pur_redm,
                               si.nav_calc,
                               si.mut_code,
                               si.tax_ben1,
                               si.tax_ben2,
                               si.tax_ben3,
                               si.spl_feat,
                               si.sip,
                               mf.Mut_Name,
                               Reg_Add1 = mf.Reg_Add1 == null ? "" : mf.Reg_Add1,
                               Reg_Add2 = mf.Reg_Add2 == null ? "" : mf.Reg_Add2,
                               Reg_city = mf.Reg_city == null ? "" : mf.Reg_city,
                               Reg_Phone1 = mf.Reg_Phone1 == null ? "" : mf.Reg_Phone1,
                               Reg_Phone2 = mf.Reg_Phone2 == null ? "" : mf.Reg_Phone2,
                               Reg_Phone3 = mf.Reg_Phone3 == null ? "" : mf.Reg_Phone3,
                               E_Mail = mf.E_Mail == null ? "" : mf.E_Mail,
                               //Fund_Manager1 = fm.Fund_Manager1 == null ? "" : fm.Fund_Manager1,
                               FundSizeDate = f.Date,
                               f.fund_Size,
                               ratio,
                               sc.SWPAllowedFlag,
                               sc.STPInAllowedFlag,
                               ExpenseRatio,
                               InceptionDate = IncpDate.Rows[0][0],
                               FaceVal = (Convert.ToDouble(FVal.Rows[0][0]) - 53) / 76,
                               IncDecFundSize,
                               Reg_Name = r.Reg_Name == null ? "" : r.Reg_Name,
                               Add1 = r.Add1 == null ? "" : r.Add1,
                               Add2 = r.Add2 == null ? "" : r.Add2,
                               City = r.City == null ? "" : r.City,
                               Phone1 = r.Phone1 == null ? "" : r.Phone1,
                               Phone2 = r.Phone2 == null ? "" : r.Phone2,
                               Phone3 = r.Phone3 == null ? "" : r.Phone3,
                               DivDate,
                               Divid_pt = divRec.Rows.Count < 1 ? "" : divRec.Rows[0]["Divid_pt"]
                           };
                dt = data.ToDataTable();
                dt.Columns.Add("Fund_Manager1");
                var fms = from t in db.Fund_ManagerExplorers
                         where
                             (from t0 in db.cur_fund_manExplorers
                              where
                                t0.sch_code == schCode &&
                                t0.flg_ActInact == true
                              select t0.fund_code).Contains(t.Fund_Code)
                          select t.Fund_Manager1;
                if (dt.Rows.Count > 0)
                    dt.Rows[0]["Fund_Manager1"] = string.Join(",", fms.ToArray());
            }
            return dt;
        }

        public static DataTable getFactsheet2(string schCode)
        {
            DataTable dt = new DataTable();
            DataTable dtInd = new DataTable();
            using (var db = new iFramesExplorerDataContext())
            {
                var data = (from a in db.ind_recExplorers
                            from b in db.SchemeIndexExplorers
                            from c in db.ind_infoExplorers
                            where
                            a.ind_code == b.Ind_Code &&
                            b.Sch_Code == schCode &&
                            a.ind_code == c.ind_code
                            orderby a.dt1 descending
                            select new
                            {
                                indRecDt = a.dt1.Value.AddDays(276),
                                ind_val = (System.Double?)((a.ind_val - 53) / 76),
                                c.ind_name
                            }).Take(1);
                dtInd = data.ToDataTable();
            }

            using (var db = new iFramesDataContext())
            {
                var MinnavRs = (from n in db.nav_news
                                where n.Sch_Code == schCode && n.Date >= DateTime.Now.AddDays(-365)
                                select new { n.Nav_Rs }).Min(a => a.Nav_Rs);

                var MaxnavRs = (from n in db.nav_news
                                where n.Sch_Code == schCode && n.Date >= DateTime.Now.AddDays(-365)
                                select new { n.Nav_Rs }).Max(a => a.Nav_Rs);

                DataTable DTmaxNav = (from nn in db.nav_news
                                      where nn.Nav_Rs == MaxnavRs && nn.Sch_Code == schCode && nn.Date >= DateTime.Now.AddDays(-365)
                                      select new { nn.Date, nn.Nav_Rs }).ToDataTable();

                DataTable DTminNav = (from nn in db.nav_news
                                      where nn.Nav_Rs == MinnavRs && nn.Sch_Code == schCode && nn.Date >= DateTime.Now.AddDays(-365)
                                      select new { nn.Date, nn.Nav_Rs }).ToDataTable();


                var data = from si in db.GetTable<scheme_Info>()
                           from ln in db.latest_navs
                           where si.sch_code == schCode && si.sch_code == ln.Sch_Code
                           select new
                           {
                               si.short_name,
                               ln.Date,
                               navRs = ln.Nav_Rs == null ? 0 : ln.Nav_Rs,
                               indRecDt = dtInd.Rows[0]["indRecDt"],
                               ind_val = dtInd.Rows[0]["ind_val"],
                               ind_name = dtInd.Rows[0]["ind_name"],
                               MaxNav = DTmaxNav.Rows.Count > 0 ? DTmaxNav.Rows[0]["nav_rs"] : 0,
                               MaxNavDate = DTmaxNav.Rows.Count > 0 ? DTmaxNav.Rows[0]["Date"] : "",
                               MinNav = DTminNav.Rows.Count > 0 ? DTminNav.Rows[0]["nav_rs"] : 0,
                               MinNavDate = DTminNav.Rows.Count > 0 ? DTminNav.Rows[0]["Date"] : ""

                           };

                dt = data.ToDataTable();
            }
            return dt;

        }

        public static DataTable getFactSheet3(string schCode)
        {
            DataTable dt = new DataTable();
            DataTable dtReturn = new DataTable();
            using (var db = new iFramesDataContext())
            {
                var maxDate = (from t in db.top_funds
                               where t.sch_code == schCode
                               select t.date).Max();

                dtReturn = (from s in db.GetTable<scheme_Info>()
                           from m in db.mut_funds
                           from t in db.top_funds
                           where s.mut_code == m.Mut_Code && s.sch_code == schCode && t.sch_code==s.sch_code
                            select new
                            {
                                maxDate,
                                s.short_name,
                                m.Mut_Name,
                                t.per_30days,
                                t.per_91days,
                                t.per_182days,
                               t.per_1yr,
                               t.per_3yr,
                               t.per_5yr,
                               t.since_incept }).ToDataTable();
            }

            using (var ex = new iFramesExplorerDataContext())
            {
                var data = (from p in ex.PreCalculatedRatios_MFIs
                           where p.sch_code == schCode
                           select new {
                               maxDate = dtReturn.Rows[0]["maxDate"] == null ? "NA" : Convert.ToDateTime(dtReturn.Rows[0]["maxDate"].ToString()).ToString("MMM dd, yyyy"),
                               short_name = dtReturn.Rows[0]["short_name"] == null ? "" : dtReturn.Rows[0]["short_name"].ToString(),
                               Mut_Name = dtReturn.Rows[0]["Mut_Name"] == null ? "" : dtReturn.Rows[0]["Mut_Name"].ToString(),
                               per_30days = dtReturn.Rows[0]["per_30days"] == null ? "NA" : dtReturn.Rows[0]["per_30days"].ToString(),
                               per_91days = dtReturn.Rows[0]["per_91days"] == null ? "NA" : dtReturn.Rows[0]["per_91days"].ToString(),
                               per_182days = dtReturn.Rows[0]["per_182days"] == null ? "NA" : dtReturn.Rows[0]["per_182days"].ToString(),
                               per_1yr = dtReturn.Rows[0]["per_1yr"] == null ? "NA" : dtReturn.Rows[0]["per_1yr"].ToString(),
                               per_3yr = dtReturn.Rows[0]["per_3yr"] == null ? "NA" : dtReturn.Rows[0]["per_3yr"].ToString(),
                               per_5yr = dtReturn.Rows[0]["per_5yr"] == null ? "NA" : dtReturn.Rows[0]["per_5yr"].ToString(),
                               since_incept = dtReturn.Rows[0]["since_incept"] == null ? "NA" : dtReturn.Rows[0]["since_incept"].ToString(),
                               average = (p.Average == null ? 0 : p.Average - 53) / 76,
                               stdv = (p.stdv == null ? 0 : p.stdv - 53) / 76,
                               sharp = (p.sharp == null ? 0 : p.sharp - 53) / 76,
                               beta = (p.beta == null ? 0 : p.beta - 53) / 76,
                               Treynor = (p.Treynor == null ? 0 : p.Treynor - 53) / 76,
                               Sortino = (p.Sortino == null ? 0 : p.Sortino - 53) / 76,
                               beta_Correlation = (p.Beta_Correlation == null ? 0 : p.Beta_Correlation - 53) / 76,
                               fama = (p.fama == null ? 0 : p.fama - 53) / 76
                           }).Take(1);

                dt = data.ToDataTable();
            }
            return dt;
        }

        public static DataTable getTopTenHoldings(string schCode)
        {
            DataTable dtIndCap = new DataTable();
            using (var db = new iFramesDataContext())               
            {                
                var dtShortName = (from s in db.GetTable<scheme_Info>()
                                   where s.sch_code == schCode
                                   select new { s.short_name, s.nature }).ToDataTable();

                var MaxDate = (from t in db.top_holds
                               where t.Sch_Code == schCode
                               select  t.Date).Max();

                var data = (from t in db.top_holds
                           join c in db.Com_details on t.c_code equals c.C_code into ps
                           from c in ps.DefaultIfEmpty()
                           join s in db.Sector_inds on c.Sect_Code equals s.Sect_code into sect
                           from s in sect.DefaultIfEmpty()
                           where t.Sch_Code == schCode && t.Date.Value.Date == MaxDate
                           orderby t.corpus_per descending, t.Mkt_Value descending
                           select new
                           {
                               SN_Nature = dtShortName.Rows[0]["nature"],
                               SN = dtShortName.Rows[0]["short_name"],
                               t.Date,
                               t.c_name,
                               t.Nature,
                               t.Mkt_Value,
                               t.corpus_per,
                               No_of_shares = t.No_of_shares == null ? 0 : t.No_of_shares,
                               Sect_name=s.Sect_name==null?"NA":s.Sect_name,
                               t.c_code,
                               t.Instrument
                           }).Take(10);

                dtIndCap = data.ToDataTable();
            }
            DataColumn colPE = new DataColumn();
            DataColumn colPCM = new DataColumn();
            colPCM.ColumnName = "PCM";
            colPE.ColumnName = "PE";
            dtIndCap.Columns.Add(colPE);
            dtIndCap.Columns.Add(colPCM);

            foreach (DataRow dr in dtIndCap.Rows)
            {
                dr["Mkt_Value"] = dr["Mkt_Value"] == null | dr["Mkt_Value"] .ToString()==""? "0" : Math.Round(Convert.ToDecimal(dr["Mkt_Value"]), 2).ToString();
                dr["corpus_per"] = dr["corpus_per"] == null ? "NA" : Math.Round(Convert.ToDecimal(dr["corpus_per"]), 2).ToString();
                               
                using (var ex = new iFramesExplorerDataContext())
                {
                    //if (dr["SN_Nature"].ToString().ToLower() == "equity")
                    //{
                        var pe = ((from b in ex.BSEPortfolio_Stats
                                    where b.C_Code == dr["c_code"] && b.Import_Date == ((from d in ex.BSEPortfolio_Stats
                                                                                         where d.C_Code == dr["c_code"]
                                                                                         select d.Import_Date).Max())
                                    select new { val = (b.Price_Earning - 53) / 76 }).Take(1)).ToDataTable();
                        dr["PE"] = pe.Rows.Count > 0 ? Math.Round(Convert.ToDecimal(pe.Rows[0][0]),2).ToString() : "NA";

                        var mktVal = ((from c in ex.com_pots
                                     where c.Sch_Code == schCode && c.c_code == dr["c_code"] &&
                                     c.Date.Value.AddDays(276).Month == Convert.ToDateTime(dr["Date"]).AddMonths(-1).Month &&
                                     c.Date.Value.AddDays(276).Year == Convert.ToDateTime(dr["Date"]).AddMonths(-1).Year
                                     select new { mkt_value = (c.Mkt_Value - 53) / 76 }).Take(1)).ToDataTable();
                        
                        double mktv=0;
                        if (mktVal.Rows.Count >0)
                        {
                            if(mktVal.Rows[0][0].ToString().Trim() != "")
                                mktv =  Convert.ToDouble(mktVal.Rows[0][0]);
                        }

                        if (mktv == 0)
                            dr["PCM"] = "NA";
                        else
                        {
                            dr["PCM"] = ((Convert.ToDouble(dr["Mkt_Value"]) - mktv) / mktv) * 100;
                            dr["PCM"] = Math.Round(Convert.ToDecimal(dr["PCM"]), 2).ToString();
                        }


                    //}
                    //else
                    //{
                    //    dr["PE"] = (from b in ex.BSEPortfolio_Stats
                    //                where b.C_Code == dr["c_code"] && b.Import_Date == ((from d in ex.BSEPortfolio_Stats
                    //                                                                     where d.C_Code == dr["c_code"]
                    //                                                                     select d.Import_Date).Max())
                    //                select new { val = (b.Price_Earning - 53) / 76 }).Take(1).ToString();

                    //    dr["PCM"] = (from c in ex.com_pots
                    //                 where c.Sch_Code == schCode && c.c_code == dr["c_code"] &&
                    //                 c.Date.Value.AddDays(276).Month == Convert.ToDateTime(dr["Date"]).AddMonths(-1).Month &&
                    //                 c.Date.Value.AddDays(276).Year == Convert.ToDateTime(dr["Date"]).AddMonths(-1).Year
                    //                 select new { mkt_value = (c.Mkt_Value - 53) / 76 }).Take(1).ToString();
                    //}
                }
            }
            dtIndCap.AcceptChanges();
            return dtIndCap;
        }

        public static string getTop5Holding(string schCode, string nature, string Dat)
        {            
            string top5;        
            using (var db = new iFramesDataContext())
            {
                if (nature.ToLower() == "equity")
                {
                    var data = (from t in db.top_holds
                                from s in db.Sector_inds
                                where t.Nature == "eq" && t.Date == Convert.ToDateTime(Dat) && t.Sch_Code == schCode && t.Sect_code == s.Sect_code
                                orderby t.corpus_per descending, t.Mkt_Value descending
                                select new { t.c_code }).Take(5).Distinct();

                    top5 = ((from t in db.top_holds
                             from d in data
                             where t.Date == Convert.ToDateTime(Dat) && t.Sch_Code == schCode && t.Nature == "eq"
                             && d.c_code == t.c_code
                             select new { corp = (t.corpus_per == null ? 0 : t.corpus_per) }).Sum(a => a.corp)).ToString();                     
                }
                else
                {
                    var data = (from t in db.top_holds
                                from s in db.Sector_inds
                                where t.Date == Convert.ToDateTime(Dat) && t.Sch_Code == schCode && t.Sect_code == s.Sect_code
                                orderby t.corpus_per descending, t.Mkt_Value descending
                                select new { t.c_code }).Take(5).Distinct();

                    top5 = ((from t in db.top_holds
                             from d in data
                             where t.Date == Convert.ToDateTime(Dat) && t.Sch_Code == schCode && t.Nature != "eq"
                             && d.c_code == t.c_code
                             select new { corp = (t.corpus_per == null ? 0 : t.corpus_per) }).Sum(a => a.corp)).ToString();                    
                }
            }
            return top5 =top5 == "" ? "NA" : Math.Round(Convert.ToDecimal(top5),2).ToString() + " as on " + Convert.ToDateTime(Dat).ToString("MMM, yyyy");
        }

        public static string getProtfolioStockNo(string schCode)
        {
            using (var db = new iFramesDataContext())
            {
                var MaxDate = (from t in db.top_holds
                            where t.Sch_Code == schCode
                            select  t.Date).Max();
                var data= (from t in db.top_holds
                        where t.Sch_Code == schCode && t.Date == MaxDate && t.Nature!="Others"
                        select new { t.c_code}).Count().ToString();

                return data = Convert.ToDouble(data) == 0 ? "NA" : data;
            }
        }        

        public static DataTable getPortfolioAttribute(string schCode) {
            DataTable dtFinalresult = new DataTable();          

            using (var ex = new iFramesExplorerDataContext())               
            {
                var MaximpDate= (from b in ex.BSEPortfolio_Stats select b.Import_Date).Max();
                var MaxComDate=(from c in ex.com_pots where c.Sch_Code==schCode select c.Date).Max();
                var MaxCalcDate=(from p in ex.preCalcPEPBs where p.Sch_code==schCode select p.CalcDate).Max();
                var MaxExpDate = (from e in ex.expenses where e.sch_code == schCode select e.date_from).Max();

                var MrkCap =(from b in ex.BSEPortfolio_Stats
                           from c in ex.com_pots
                           where b.C_Code == c.c_code && c.Sch_Code == schCode && b.Import_Date.Date == MaximpDate.Date 
                           && c.Date.Value.Date == MaxComDate
                           select new { MarketCap=(b.MarketCap-53)/76 }).Sum(a=>a.MarketCap).ToString();

                MrkCap = MrkCap == null | MrkCap == "" ? "NA" : Math.Round(Convert.ToDecimal(MrkCap), 2).ToString() + " as on "; 

                var dtLarge = (from m in ex.marketcapclassification_details
                            where m.sch_code == schCode && m.Market_Slab == "Large Cap"
                            select new {m.calcDate,m.MCapAllocation}).ToDataTable();

                string Large="NA";
                if(dtLarge.Rows.Count < 1){
                    if (Convert.ToDouble(dtLarge.Rows[0]["MCapAllocation"]) == 0.00)
                        Large = "NA";
                    else
                        Large=Math.Round(Convert.ToDecimal(dtLarge.Rows[0]["MCapAllocation"]), 2).ToString() + " as on " + Convert.ToDateTime(dtLarge.Rows[0]["calcDate"].ToString()).ToString("MMM dd, yyyy");

                }
                var dtMid =( from m in ex.marketcapclassification_details
                          where m.sch_code == schCode && m.Market_Slab == "Mid Cap"
                             select new { m.calcDate, m.MCapAllocation }).ToDataTable();
                var Mid = dtMid.Rows.Count < 1 | Convert.ToDouble(dtMid.Rows[0]["MCapAllocation"]) == 0.00 ? "NA" : Math.Round(Convert.ToDecimal(dtMid.Rows[0]["MCapAllocation"]), 2).ToString() + " as on " + Convert.ToDateTime(dtMid.Rows[0]["calcDate"].ToString()).ToString("MMM dd, yyyy");

                var dtSmall = (from m in ex.marketcapclassification_details
                            where m.sch_code == schCode && m.Market_Slab == "Small Cap"
                               select new { m.calcDate, m.MCapAllocation }).ToDataTable();
                var Small = dtSmall.Rows.Count < 1 | Convert.ToDouble(dtSmall.Rows[0]["MCapAllocation"]) == 0.00 ? "NA" : Math.Round(Convert.ToDecimal(dtSmall.Rows[0]["MCapAllocation"]), 2).ToString() + " as on " + Convert.ToDateTime(dtSmall.Rows[0]["calcDate"].ToString()).ToString("MMM dd, yyyy");

                var data = from p in ex.preCalcPEPBs
                           from e in ex.expenses
                            where p.Sch_code == schCode && p.CalcDate.Value.Date == MaxCalcDate &&
                            e.sch_code == p.Sch_code && e.date_from == MaxExpDate
                            select new
                            {
                                actual_per=(e.actual_per-53)/76,
                                p.CalcDate, 
                                p.PB, 
                                p.PE, 
                                MarketCap = p.MarketCap == null ? 0 : p.MarketCap, 
                                p.DivYield, 
                                MrkCap,
                                Large,
                                Mid,
                                Small                                
                            };

                dtFinalresult = data.ToDataTable();
            }
            return dtFinalresult;
        }

        public static DataTable getInOut(string schCode, string Dat, string InOut)
        {
            DataTable dtRetVal=new DataTable();            
            using (var ex = new iFramesExplorerDataContext())
            {
                if (InOut == "In")
                {
                    var Cname=(from c in ex.com_pots
                              where c.Sch_Code==schCode && (c.Nature=="debt" | c.Nature=="eq") 
                              && c.Date.Value.AddDays(276).Month==Convert.ToDateTime(Dat).AddMonths(-1).Month 
                              && c.Date.Value.AddDays(276).Year==Convert.ToDateTime(Dat).AddMonths(-1).Year                              
                              select new {c.c_name}).Distinct();

                    dtRetVal = (from c in ex.com_pots
                               from n in Cname
                               where c.Sch_Code == schCode && (c.Nature == "debt" | c.Nature == "eq")
                               && c.Date.Value.AddDays(276).Month == Convert.ToDateTime(Dat).Month
                               && c.Date.Value.AddDays(276).Year == Convert.ToDateTime(Dat).Year &&
                               c.c_name!=n.c_name orderby c.corpus_per descending
                               select new { c.c_name }).Take(4).ToDataTable();
                }
                else if (InOut == "Out")
                {
                    var Cname = (from c in ex.com_pots
                                 where c.Sch_Code == schCode && (c.Nature == "debt" | c.Nature == "eq")
                                 && c.Date.Value.AddDays(276).Month == Convert.ToDateTime(Dat).Month
                                 && c.Date.Value.AddDays(276).Year == Convert.ToDateTime(Dat).Year
                                 select new { c.c_name }).Distinct();

                    dtRetVal = (from c in ex.com_pots
                               from n in Cname
                               where c.Sch_Code == schCode && (c.Nature == "debt" | c.Nature == "eq")
                               && c.Date.Value.AddDays(276).Month == Convert.ToDateTime(Dat).AddMonths(-1).Month
                               && c.Date.Value.AddDays(276).Year == Convert.ToDateTime(Dat).AddMonths(-1).Year &&
                               c.c_name != n.c_name
                               orderby c.corpus_per descending
                                select new { c.c_name }).Take(4).ToDataTable();
                }                
            }
            return dtRetVal;            
        }

        public static DataTable getSectorAllocation(string schCode)
        {
            DataTable dt = new DataTable();
            using (var ex = new iFramesExplorerDataContext())
            {
                dt = (from a in ex.com_pots
                     from b in ex.Sector_indExplorers
                     where a.Sch_Code == schCode && a.Date==((from t in ex.com_pots
                                                     where t.Sch_Code == schCode
                                                     select t.Date ).Max())
                                                     && a.Sect_code == b.Sect_code
                     group new { b, a } by new { b.Sect_name } into g
                     select new
                     {
                         Sect_name=g.Key.Sect_name,
                         corpus_per = (System.Double?)g.Sum(p => (p.a.corpus_per - 53) / 76),
                         Mkt_Value = (System.Double?)g.Sum(p => (p.a.Mkt_Value - 53) / 76)
                     }).ToDataTable();
            }
            return dt;
        }

        public static DataTable getAssetAllocation(string schcode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                dt=(from i in db.Invt_Pats
                        where i.sch_code==schcode && i.Date==((from ii in db.Invt_Pats
                                                                  where ii.sch_code==schcode
                                                                  select ii.Date).Max())
                    select new {i.Date,i.Equity,i.Debt,i.Cas_MonM}).ToDataTable();
            }
            return dt;
        }

        static public DataTable GetFactSheetFive(string sname, string yr = "Choose Year", string mon = "0")
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    DataTable tbl = new DataTable();
                    if (yr == "Choose Year")
                    {
                        tbl = ((from s in indcap.GetTable<scheme_Info>()
                                from mf in indcap.mfi_news
                                where mf.DATE1 >= DateTime.Now.AddMonths(-6) && mf.DATE1 <= DateTime.Now &&
                                mf.Mut_Name == s.mut_code && s.sch_code == sname
                                select new { mf.news_headline,
                                    mf.newspaper_name,
                                    mf.DATE1,
                                    mf.Mut_Name,
                                    s.short_name }).Distinct().OrderByDescending(t => t.DATE1)).ToDataTable();

                    }
                    if (yr != "Choose Year")
                    {
                        if (mon != "0")
                        {
                            tbl = ((from s in indcap.GetTable<scheme_Info>()
                                    from mf in indcap.mfi_news
                                    where mf.DATE.Value.Month.ToString() == mon
                                    && Convert.ToDateTime(mf.DATE.ToString()).Year.ToString() == yr &&
                                    mf.Mut_Name == s.mut_code && s.sch_code == sname
                                    select new { mf.news_headline, mf.newspaper_name, mf.DATE1, mf.Mut_Name, s.short_name }).Distinct().OrderByDescending(t => t.DATE1)).ToDataTable();
                        }
                        else
                        {
                            tbl = ((from s in indcap.GetTable<scheme_Info>()
                                    from mf in indcap.mfi_news
                                    where mf.DATE.Value.Year.ToString() == yr &&
                                    mf.Mut_Name == s.mut_code && s.sch_code == sname
                                    orderby mf.DATE1 descending
                                    select new { mf.news_headline, mf.newspaper_name, mf.DATE1, mf.Mut_Name, s.short_name }).Distinct().OrderByDescending(t => t.DATE1)).ToDataTable();
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

        public static DataTable getFundRedeemedInYear(string sname,string MutCode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                var frmDate="01/01/"+DateTime.Today.Year;
                var data = from s in db.GetTable<scheme_Info>()
                           where s.redmpdate >= Convert.ToDateTime(frmDate) && s.redmpdate <= DateTime.Today
                           && (s.short_name == sname | s.short_name.Contains(sname) | s.sch_code == sname | s.sch_code.Contains(sname))
                           orderby s.redmpdate descending
                           select new {s.mut_code,s.short_name,s.redmpdate,s.type1 };

                if (MutCode != "")
                    dt = AllMethods.getMutResult(MutCode, "mut_code", data.ToDataTable());
                else
                    dt = data.ToDataTable();
            }
            return dt;
        }

        public static DataTable getFundRedeemedTillDate(string sname, string MutCode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                var frmDate = "01/01/1900";
                var data = (from s in db.GetTable<scheme_Info>()
                           where s.redmpdate != Convert.ToDateTime(frmDate) && s.redmpdate <= DateTime.Today
                           && (s.short_name == sname | s.short_name.Contains(sname) | s.sch_code == sname | s.sch_code.Contains(sname))
                           orderby s.redmpdate descending
                           select new {  s.short_name,s.mut_code, s.redmpdate, s.type1 }).Distinct();

                if (MutCode != "")
                    dt = AllMethods.getMutResult(MutCode, "mut_code", data.ToDataTable());
                else
                    dt = data.ToDataTable();
            }
            return dt;
        }

        static public DataTable getShemeConveretd(string sname)
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var tbl = ((from s in indcap.GetTable<scheme_Info>()
                                where s.Fund_Text != ""
                                && (s.short_name == sname | s.short_name.Contains(sname) | s.sch_code == sname | s.sch_code.Contains(sname))
                                orderby s.redmpdate descending
                                select new { s.short_name, s.mut_code, s.rolled_over_date, s.rold_over, s.Fund_Text }).Distinct()).ToDataTable();
                    return tbl;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public DataTable getBookClosure()
        {
            try
            {
                using (var indcap = new iFramesDataContext())
                {
                    var tbl = (from bc in indcap.Book_Closes
                               where bc.BC_Till >= Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yy"))
                               orderby bc.Sch_Name
                               select new { bc.Sch_Name, bc.Sch_code, bc.BC_from, bc.BC_Till, bc.Notice }).ToDataTable();
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