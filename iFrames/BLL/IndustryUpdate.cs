using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFrames.DAL;

namespace iFrames.BLL
{
    public class IndustryUpdate
    {
        public static DataTable getAMCCorpOffice(string MutCode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                var data = from m in db.mut_funds
                           where m.Mut_Code.Trim() == MutCode
                           select new
                           {
                               m.Mut_Name,
                               m.Cor_Add1,
                               m.Cor_Add2,
                               m.Cor_City,
                               m.Cor_Conper,
                               m.Cor_Fax,
                               m.Cor_Phone1,
                               m.Cor_Phone2,
                               m.Cor_Phone3,
                               m.Cor_Pin,
                               m.E_Mail,
                               m.Web_Site,
                               m.Reg_Add1,
                               m.Reg_Add2,
                               m.Reg_city,
                               m.Reg_Conper,
                               m.Reg_Fax,
                               m.Reg_Phone1,
                               m.Reg_Phone2,
                               m.Reg_Phone3,
                               m.Reg_Pin,
                               m.partner
                           };
                dt = data.ToDataTable();
            }
            return dt;
        }

        public static DataTable getAMCBranchOffice(string MutCode)
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                var data = from m in db.mut_brs
                           where m.Mut_Code.Trim() == MutCode
                           select new { m.BR_ADD1, m.BR_ADD2, m.BR_CITY, m.BR_CONPER, m.Br_Phone1, m.BR_FAX, m.Br_Phone2, m.Br_Phone3, m.BR_PIN };
                dt = data.ToDataTable();
            }
            return dt;
        }

        public static DataTable getAMFIdate()
        {
            DataTable dt = new DataTable();
            using (var db = new iFramesDataContext())
            {
                dt = (((from a in db.amfis
                        select new { datVal = a.date.Value.Date, datText = "" }).Distinct()).OrderByDescending(a => a.datVal)).ToDataTable();
            }
            foreach (DataRow dr in dt.Rows)
            {
                dr["datText"] = Convert.ToDateTime(dr["datVal"].ToString()).ToString("MMM dd, yyyy");
            }
            dt.AcceptChanges();
            return dt;
        }

        public static DataTable getNewSchemes(DateTime dat)
        {
            DataTable dtMain = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtResult = new DataTable();
            int new_sch_open = 0, new_sch_close = 0;
            double new_sch_amt_open = 0.00, new_sch_amt_close = 0.00;

            using (var db = new iFramesExplorerDataContext())
            {
                var data = (from a in db.amfiExplorers
                            where a.date.Value.AddDays(276) == dat && a.type == "open-end"
                            select new
                            {
                                a.nature,
                                new_sch_open = a.new_sch == null ? 0 : (a.new_sch - 53) / 76,
                                new_sch_amt_open = a.new_sch_amt == null ? 0 : (a.new_sch_amt - 53) / 76,
                                new_sch_close,
                                new_sch_amt_close
                            }).Distinct();
                dtMain = data.ToDataTable();

                var data1 = (from b in db.amfiExplorers
                             where b.date.Value.AddDays(276) == dat && b.type == "close-end"
                             select new
                             {
                                 b.nature,
                                 new_sch_open,
                                 new_sch_amt_open,
                                 new_sch_close = b.new_sch == null ? 0 : (b.new_sch - 53) / 76,
                                 new_sch_amt_close = b.new_sch_amt == null ? 0 : (b.new_sch_amt - 53) / 76,
                             }).Distinct();
                dt1 = data1.ToDataTable();

                dtMain.Merge(dt1);
                var x = from row in dtMain.AsEnumerable()
                        group row by row.Field<string>("nature") into rows
                        select new
                        {
                            Nature = rows.Key,
                            new_sch_open = rows.Sum(r => r.Field<int>("new_sch_open")),
                            new_sch_amt_open = rows.Sum(r => r.Field<double>("new_sch_amt_open")),
                            new_sch_close = rows.Sum(r => r.Field<int>("new_sch_close")),
                            new_sch_amt_close = rows.Sum(r => r.Field<double>("new_sch_amt_close"))
                        };
                dtResult = x.ToDataTable();
            }


            DataColumn totSch = new DataColumn("TotSch", System.Type.GetType("System.Int32"));
            DataColumn totAmt = new DataColumn("TotAmt", System.Type.GetType("System.Double"));
            dtResult.Columns.Add(totSch);
            dtResult.Columns.Add(totAmt);
            foreach (DataRow dr in dtResult.Rows)
            {
                dr["TotSch"] = (Convert.ToInt32(dr["new_sch_open"]) + Convert.ToInt32(dr["new_sch_close"]));
                dr["TotAmt"] = (Convert.ToDouble(dr["new_sch_amt_open"]) + Convert.ToDouble(dr["new_sch_amt_close"]));
            }
            DataRow drTot = dtResult.NewRow();
            drTot["nature"] = "Total";
            drTot["new_sch_open"] = dtResult.Compute("sum(new_sch_open)", "");
            drTot["new_sch_amt_open"] = dtResult.Compute("sum(new_sch_amt_open)", "");
            drTot["new_sch_close"] = dtResult.Compute("sum(new_sch_close)", "");
            drTot["new_sch_amt_close"] = dtResult.Compute("sum(new_sch_amt_close)", "");
            drTot["TotSch"] = dtResult.Compute("sum(TotSch)", "");
            drTot["TotAmt"] = dtResult.Compute("sum(TotAmt)", "");
            dtResult.Rows.Add(drTot);

            dtResult.AcceptChanges();
            return dtResult;
        }

        public static DataTable getExistingSchemes(DateTime dat)
        {
            DataTable dtMain = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtResult = new DataTable();
            int ext_sch_open = 0, ext_sch_close = 0;
            double ext_sch_amt_open = 0.00, ext_sch_amt_close = 0.00;

            using (var db = new iFramesDataContext())
            {
                var data = (from a in db.amfis
                            where a.date == dat && a.type == "Open-End"
                            select new
                            {
                                a.nature,
                                ext_sch_open = a.exists_sch == null ? 0 : a.exists_sch,
                                ext_sch_amt_open = a.exists_sch_amt == null ? 0 : a.exists_sch_amt,
                                ext_sch_close = ext_sch_close,
                                ext_sch_amt_close = ext_sch_amt_close
                            }).Distinct();
                dtMain = data.ToDataTable();

                var data1 = (from b in db.amfis
                             where b.date == dat && b.type == "Close-End"
                             select new
                             {
                                 b.nature,
                                 ext_sch_open = ext_sch_open,
                                 ext_sch_amt_open = ext_sch_amt_open,
                                 ext_sch_close = b.exists_sch == null ? 0 : b.exists_sch,
                                 ext_sch_amt_close = b.exists_sch_amt == null ? 0 : b.exists_sch_amt,
                             }).Distinct();
                dt1 = data1.ToDataTable();

                dtMain.Merge(dt1);
                var x = from row in dtMain.AsEnumerable()
                        group row by row.Field<string>("nature") into rows
                        select new
                        {
                            Nature = rows.Key,
                            ext_sch_open = rows.Sum(r => r.Field<int>("ext_sch_open")),
                            ext_sch_amt_open = rows.Sum(r => r.Field<double>("ext_sch_amt_open")),
                            ext_sch_close = rows.Sum(r => r.Field<int>("ext_sch_close")),
                            ext_sch_amt_close = rows.Sum(r => r.Field<double>("ext_sch_amt_close"))
                        };
                dtResult = x.ToDataTable();
            }


            DataColumn totSch = new DataColumn("TotSch", System.Type.GetType("System.Int32"));
            DataColumn totAmt = new DataColumn("TotAmt", System.Type.GetType("System.Double"));
            dtResult.Columns.Add(totSch);
            dtResult.Columns.Add(totAmt);
            foreach (DataRow dr in dtResult.Rows)
            {
                dr["TotSch"] = (Convert.ToInt32(dr["ext_sch_open"]) + Convert.ToInt32(dr["ext_sch_close"]));
                dr["TotAmt"] = (Convert.ToDouble(dr["ext_sch_amt_open"]) + Convert.ToDouble(dr["ext_sch_amt_close"]));
            }
            DataRow drTot = dtResult.NewRow();
            drTot["nature"] = "Total";
            drTot["ext_sch_open"] = dtResult.Compute("sum(ext_sch_open)", "");
            drTot["ext_sch_amt_open"] = dtResult.Compute("sum(ext_sch_amt_open)", "");
            drTot["ext_sch_close"] = dtResult.Compute("sum(ext_sch_close)", "");
            drTot["ext_sch_amt_close"] = dtResult.Compute("sum(ext_sch_amt_close)", "");
            drTot["TotSch"] = dtResult.Compute("sum(TotSch)", "");
            drTot["TotAmt"] = dtResult.Compute("sum(TotAmt)", "");
            dtResult.Rows.Add(drTot);

            dtResult.AcceptChanges();
            return dtResult;
        }

        public static DataTable getAllSchemes(DateTime dat)
        {
            DataTable dtMain = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtResult = new DataTable();
            int All_sch_open = 0, All_sch_close = 0;
            double All_sch_amt_open = 0.00, All_sch_amt_close = 0.00;

            using (var db = new iFramesDataContext())
            {
                var data = (from a in db.amfis
                            where a.date == dat && a.type == "Open-End"
                            select new
                            {
                                a.nature,
                                All_sch_open = a.tot_sch == null ? 0 : a.tot_sch,
                                All_sch_amt_open = a.tot_amt == null ? 0 : a.tot_amt,
                                All_sch_close = All_sch_close,
                                All_sch_amt_close = All_sch_amt_close
                            }).Distinct();
                dtMain = data.ToDataTable();

                var data1 = (from b in db.amfis
                             where b.date == dat && b.type == "Close-End"
                             select new
                             {
                                 b.nature,
                                 All_sch_open = All_sch_open,
                                 All_sch_amt_open = All_sch_amt_open,
                                 All_sch_close = b.tot_sch == null ? 0 : b.tot_sch,
                                 All_sch_amt_close = b.tot_amt == null ? 0 : b.tot_amt,
                             }).Distinct();
                dt1 = data1.ToDataTable();

                dtMain.Merge(dt1);
                var x = from row in dtMain.AsEnumerable()
                        group row by row.Field<string>("nature") into rows
                        select new
                        {
                            Nature = rows.Key,
                            All_sch_open = rows.Sum(r => r.Field<int>("All_sch_open")),
                            All_sch_amt_open = rows.Sum(r => r.Field<double>("All_sch_amt_open")),
                            All_sch_close = rows.Sum(r => r.Field<int>("All_sch_close")),
                            All_sch_amt_close = rows.Sum(r => r.Field<double>("All_sch_amt_close"))
                        };
                dtResult = x.ToDataTable();
            }


            DataColumn totSch = new DataColumn("TotSch", System.Type.GetType("System.Int32"));
            DataColumn totAmt = new DataColumn("TotAmt", System.Type.GetType("System.Double"));
            dtResult.Columns.Add(totSch);
            dtResult.Columns.Add(totAmt);
            foreach (DataRow dr in dtResult.Rows)
            {
                dr["TotSch"] = (Convert.ToInt32(dr["All_sch_open"]) + Convert.ToInt32(dr["All_sch_close"]));
                dr["TotAmt"] = (Convert.ToDouble(dr["All_sch_amt_open"]) + Convert.ToDouble(dr["All_sch_amt_close"]));
            }
            DataRow drTot = dtResult.NewRow();
            drTot["nature"] = "Total";
            drTot["All_sch_open"] = dtResult.Compute("sum(All_sch_open)", "");
            drTot["All_sch_amt_open"] = dtResult.Compute("sum(All_sch_amt_open)", "");
            drTot["All_sch_close"] = dtResult.Compute("sum(All_sch_close)", "");
            drTot["All_sch_amt_close"] = dtResult.Compute("sum(All_sch_amt_close)", "");
            drTot["TotSch"] = dtResult.Compute("sum(TotSch)", "");
            drTot["TotAmt"] = dtResult.Compute("sum(TotAmt)", "");
            dtResult.Rows.Add(drTot);

            dtResult.AcceptChanges();
            return dtResult;
        }

        public static DataTable getRedemption(DateTime dat)
        {
            DataTable dtMain = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtResult = new DataTable();
            double Redem_close = 0, Redem_open = 0;
            using (var db = new iFramesDataContext())
            {
                var data = (from a in db.amfis
                            where a.date == dat && a.type == "Open-End"
                            select new
                            {
                                a.nature,
                                Redem_open = a.redemption == null ? 0 : a.redemption,
                                Redem_close
                            }).Distinct();
                dtMain = data.ToDataTable();

                var data1 = (from b in db.amfis
                             where b.date == dat && b.type == "Close-End"
                             select new
                             {
                                 b.nature,
                                 Redem_close = b.redemption == null ? 0 : b.redemption,
                                 Redem_open
                             }).Distinct();
                dt1 = data1.ToDataTable();

                dtMain.Merge(dt1);
                var x = from row in dtMain.AsEnumerable()
                        group row by row.Field<string>("nature") into rows
                        select new
                        {
                            Nature = rows.Key,
                            Redem_open = rows.Sum(r => r.Field<double>("Redem_open")),
                            Redem_close = rows.Sum(r => r.Field<double>("Redem_close"))
                        };
                dtResult = x.ToDataTable();
            }


            DataColumn totRedem = new DataColumn("TotRedem", System.Type.GetType("System.Double"));
            dtResult.Columns.Add(totRedem);

            foreach (DataRow dr in dtResult.Rows)
            {
                dr["TotRedem"] = (Convert.ToDouble(dr["Redem_open"]) + Convert.ToDouble(dr["Redem_close"]));
            }
            DataRow drTot = dtResult.NewRow();
            drTot["nature"] = "Total";
            drTot["Redem_open"] = dtResult.Compute("sum(Redem_open)", "");
            drTot["Redem_close"] = dtResult.Compute("sum(Redem_close)", "");
            drTot["TotRedem"] = dtResult.Compute("sum(TotRedem)", "");
            dtResult.Rows.Add(drTot);

            dtResult.AcceptChanges();
            return dtResult;
        }

        public static DataTable getTransactionData()
        {
            // DataTable dtResult = new DataTable();
            using (var db = new iFramesDataContext())
            {
                var data = ((from s in db.sale_purchases
                             where s.org != null | s.org.Trim() != "" && s.date == ((from ss in db.sale_purchases
                                                                                     where ss.org == s.org
                                                                                     select new { ss.date }).Max(a => a.date))
                             select new { s.org,s.date }).Distinct()).ToDataTable();                
                return data;
            }

        }

        public static DataTable getTransactionDataInnerList(string Orgs)
        {            
            using (var db = new iFramesDataContext())
            {
                var dt1 = (from s in db.sale_purchases
                           where s.org == Orgs && s.date == ((from ss in db.sale_purchases
                                                              where ss.org == Orgs
                                                               select new { ss.date }).Max(a => a.date))
                           select new { s.date, s.nature, s.org, pur=Convert.ToDouble(s.purchase), sale=Convert.ToDouble(s.sale) }).ToDataTable();

                DataColumn net = new DataColumn("net", System.Type.GetType("System.Double"));
                net.Expression = "pur - sale";
                dt1.Columns.Add(net);                
                return dt1;
            }
        }
    }
}