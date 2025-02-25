using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;
using System.Data;
using System.Data.SqlClient;

namespace iFrames.Pages
{
    public partial class AssetUnderMgmt : MyBasePage
    {
        Int64 Total = 0; Int64 New_Schemes = 0; Int64 Exist_Schemes = 0;
        Int64 TotalSales = 0; Int64 Redemption = 0; Int64 AUM1 = 0;
        Int64 AUMPREV = 0; Int64 IOflow = 0; protected int lastrow = 0;
        string MutcodeFromBase = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            // MutcodeFromBase = "MF024,MF057,MF059";
            if (!IsPostBack)
            {

                dataBind(Aum.GetdtAumAll(this.PropMutCode));
                DataTable dt = new DataTable();
                dt = Aum.GetdtAumMutData();

                string Sum_New_Launched_Sales = dt.Compute("sum(New_Launched_Sales)", "").ToString();
                string Sum_New_Schemes_Sales = dt.Compute("sum(New_Schemes_Sales)", "").ToString();
                string Sum_Exist_Schemes_Sales = dt.Compute("sum(Exist_Schemes_Sales)", "").ToString();
                string Sum_TotalSales = dt.Compute("sum(TotalSales)", "").ToString();
                string Sum_Redemption = dt.Compute("sum(Redemption)", "").ToString();
                string Sum_AUM1 = dt.Compute("sum(AUM1)", "").ToString();
                string Sum_AUMPREV = dt.Compute("sum(AUMPREV)", "").ToString();
                string Sum_IOflow = dt.Compute("sum(IOflow)", "").ToString();

                DataRow[] drGroup2 = dt.Select("status='D'");
                DataRow[] drGroup1 = dt.Select("status<>'D'");

                DataTable tblGroup1 = new DataTable();
                DataTable tblGroup2 = new DataTable();
                tblGroup2 = dt.Clone();
                tblGroup1 = dt.Clone();
                foreach (DataRow row in drGroup1)
                {
                    tblGroup1.ImportRow(row);
                }
                foreach (DataRow row in drGroup2)
                {
                    tblGroup2.ImportRow(row);
                }
                DataRow draddsum = tblGroup2.NewRow();
               // draddsum["Date"] = DateTime.Now;
                draddsum["Sector"] ="Grand Total" ;
                draddsum["New_Launched_Sales"] = Sum_New_Launched_Sales;
                draddsum["New_Schemes_Sales"] = Sum_New_Schemes_Sales;
                draddsum["Exist_Schemes_Sales"] = Sum_Exist_Schemes_Sales;
                draddsum["TotalSales"] = Sum_TotalSales;
                draddsum["Redemption"] = Sum_Redemption;
                draddsum["AUM1"] = Sum_AUM1;
                draddsum["AUMPREV"] = Sum_AUMPREV;
                draddsum["IOflow"] = Sum_IOflow;
                //draddsum["status"] = "E";
                tblGroup2.Rows.Add(draddsum);

                lastrow = tblGroup2.Rows.Count;
                LstMutDataGr1.DataSource = tblGroup1;
                LstMutDataGr1.DataBind();
                LstMutDataGr2.DataSource = tblGroup2;
                LstMutDataGr2.DataBind();
                Label nextdt = LstMutDataGr1.FindControl("LblNextDate") as Label;
                Label prevdt = LstMutDataGr1.FindControl("LblPrvDate") as Label;
                nextdt.Text = Convert.ToDateTime(tblGroup1.Rows[0]["Date"]).ToString("MMM dd,yyyy");
                prevdt.Text = Convert.ToDateTime(tblGroup1.Rows[0]["prevdt"]).ToString("MMM dd,yyyy");
                LblDate.Text = Convert.ToDateTime(tblGroup1.Rows[0]["Date"]).ToString("MMM dd,yyyy");
            }
        }

        protected Int64 GetSales(Int64 getsale)
        { Total += getsale; return getsale; }

        protected Int64 GetTotal()
        { return Total; }
        

        protected Int64 GetNew_Schemes(Int64 getsale)
        { New_Schemes += getsale; return getsale; }

        protected Int64 GetTotalNew_Schemes()
        { return New_Schemes; }

        protected Int64 GetExist_Schemes(Int64 getsale)
        { Exist_Schemes += getsale; return getsale; }

        protected Int64 GetTotalExist_Schemes()
        { return Exist_Schemes; }

        protected Int64 GetTotalSales(Int64 getsale)
        { TotalSales += getsale; return getsale; }

        protected Int64 GetTotalTotalSales()
        { return TotalSales; }

        protected Int64 GetRedemption(Int64 getsale)
        { Redemption += getsale; return getsale; }

        protected Int64 GetTotalRedemption()
        { return Redemption; }

        protected Int64 GetAUM1(Int64 getsale)
        { AUM1 += getsale; return getsale; }

        protected Int64 GetTotalAUM1()
        { return AUM1; }

        protected Int64 GetAUMPREV(Int64 getsale)
        { AUMPREV += getsale; return getsale; }

        protected Int64 GetTotalAUMPREV()
        { return AUMPREV; }

        protected Int64 GetIOflow(Int64 getsale)
        { IOflow += getsale; return getsale; }

        protected Int64 GetTotalIOflow()
        { return IOflow; }

        protected void LstAum_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            //MutcodeFromBase = "MF024,MF057,MF059";
            var dp = (sender as ListView).FindControl("AsmDataPager") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                dataBind(Aum.GetdtAumAll(this.PropMutCode));
            }
        }
        private void dataBind(DataTable dt)
        {

            LstAum.DataSource = dt;
            LstAum.DataBind();
        }
    }
}