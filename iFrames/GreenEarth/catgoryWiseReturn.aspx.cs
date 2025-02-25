using iFrames.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.GreenEarth
{
    public partial class catgoryWiseReturn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNature();
            }
        }

        protected void LoadNature()
        {



            DataTable _dt = AllMethods.getSebiNature();
            foreach (DataRow dr in _dt.Rows)
            {
                int nature_id = Convert.ToInt32(dr["Sebi_Nature_id"].ToString());
                LoadSubNature(nature_id);
            }
        }
        protected void LoadSubNature(int id)
        {

            DataTable dtEqity = new DataTable();
            dtEqity.Columns.Add("_Sebi_Sub_Nature");
            dtEqity.Columns.Add("Sebi_Sub_Nature");

            dtEqity.Rows.Add("Multi Cap Fund", "Multi Cap");
            dtEqity.Rows.Add("Flexi Cap Fund", "Flexicap");
            dtEqity.Rows.Add("Large Cap Fund", "Large Cap");
            dtEqity.Rows.Add("Large & Mid Cap Fund", "Large & MidCap");
            dtEqity.Rows.Add("Mid Cap Fund", "Mid Cap");
            dtEqity.Rows.Add("Small cap Fund", "Small Cap");
            dtEqity.Rows.Add("ELSS", "ELSS");
            dtEqity.Rows.Add("Dividend Yield Fund", "Div. Yield");
            dtEqity.Rows.Add("Focused Fund", "Focused");
            dtEqity.Rows.Add("Sectoral", "Sectoral");
            dtEqity.Rows.Add("Contra Fund", "Contra");
            dtEqity.Rows.Add("Thematic", "Thematic ");
            dtEqity.Rows.Add("Value Fund", "Value");
            dtEqity.Rows.Add("Others", "Others");

            DataTable DtDebt = new DataTable();
            DtDebt.Columns.Add("_Sebi_Sub_Nature");
            DtDebt.Columns.Add("Sebi_Sub_Nature");

            DtDebt.Rows.Add("Long Duration Fund", "Long Duration");
            DtDebt.Rows.Add("Medium to Long Duration Fund", "Medium to Long Dur.");
            DtDebt.Rows.Add("Medium Duration Fund", "Medium Dur.");
            DtDebt.Rows.Add("Low Duration Fund", "Low Duration");
            DtDebt.Rows.Add("Short Duration Fund", "Short Duration");
            DtDebt.Rows.Add("Ultra Short Duration Fund", "Ultra Short Duration");
            DtDebt.Rows.Add("Money Market Fund", "Money Market Fund");
            DtDebt.Rows.Add("Liquid Fund", "Liquid Fund");
            DtDebt.Rows.Add("Overnight Fund", "Overnight Fund");
            DtDebt.Rows.Add("Banking and PSU Fund", "Banking and PSU");
            DtDebt.Rows.Add("Corporate Bond Fund", "Corporate Bond ");
            DtDebt.Rows.Add("Credit Risk Fund", "Credit Risk");
            DtDebt.Rows.Add("Dynamic Bond", "Dynamic Bond");
            DtDebt.Rows.Add("Floater Fund", "Floater");
            DtDebt.Rows.Add("Gilt Fund", "Gilt");
            DtDebt.Rows.Add("Gilt Fund with 10 year constant duration", "Gilt (10 Yrs Constant Duration)");
            DtDebt.Rows.Add("FMP", "FMP");
            DtDebt.Rows.Add("Others", "Others");

            DataTable DtHybrid = new DataTable();
            DtHybrid.Columns.Add("_Sebi_Sub_Nature");
            DtHybrid.Columns.Add("Sebi_Sub_Nature");

            DtHybrid.Rows.Add("Aggressive Hybrid Fund", "Aggressive Hybrid");
            DtHybrid.Rows.Add("Conservative Hybrid Fund", "Conservative Hybrid");
            DtHybrid.Rows.Add("Dynamic Asset Allocation or Balanced Advantage", "Dynamic Asset Allocation");
            DtHybrid.Rows.Add("Multi Asset Allocation", "Multi Asset Alocation");
            DtHybrid.Rows.Add("Equity Savings", "Equity Savings");
            DtHybrid.Rows.Add("Arbitrage Fund", "Arbitrage Fund");
            DtHybrid.Rows.Add("Capital Protection funds", "Capital Protection Fund");
            DtHybrid.Rows.Add("Others", "Other");


            if (id == -1)
                return;
            else if (id == 4)
            {
                DataTable _dt = AllMethods.getSebiSubNature(id);

                var joinREs = (from t in dtEqity.AsEnumerable()
                               join p in _dt.AsEnumerable()
                               on t.Field<string>("_Sebi_Sub_Nature") equals p.Field<string>("Sebi_Sub_Nature")
                               select new
                               {
                                   Sebi_Sub_Nature = t.Field<string>("Sebi_Sub_Nature"),
                                   Sebi_Sub_Nature_ID = p.Field<decimal>("Sebi_Sub_Nature_ID"),
                                   //Sebi_Sub_Nature = p.Field<string>("Sebi_Sub_Nature")

                               }).ToDataTable();


                RepeaterEquity.DataSource = joinREs; //_dt;
                RepeaterEquity.DataBind();

            }
            else if (id == 3)
            {

                DataTable _dt = AllMethods.getSebiSubNature(id);

                var joinREs = (from t in DtDebt.AsEnumerable()
                               join p in _dt.AsEnumerable()
                               on t.Field<string>("_Sebi_Sub_Nature") equals p.Field<string>("Sebi_Sub_Nature")
                               select new
                               {
                                   Sebi_Sub_Nature = t.Field<string>("Sebi_Sub_Nature"),
                                   Sebi_Sub_Nature_ID = p.Field<decimal>("Sebi_Sub_Nature_ID"),
                                   //Sebi_Sub_Nature = p.Field<string>("Sebi_Sub_Nature")

                               }).ToDataTable();

                RepeaterDebt.DataSource = joinREs;//_dt;
                RepeaterDebt.DataBind();
            }
            else if (id == 1)
            {
                DataTable _dt = AllMethods.getSebiSubNature(id);

                var joinREs = (from t in DtHybrid.AsEnumerable()
                               join p in _dt.AsEnumerable()
                               on t.Field<string>("_Sebi_Sub_Nature") equals p.Field<string>("Sebi_Sub_Nature")
                               select new
                               {
                                   Sebi_Sub_Nature = t.Field<string>("Sebi_Sub_Nature"),
                                   Sebi_Sub_Nature_ID = p.Field<decimal>("Sebi_Sub_Nature_ID"),
                                   //Sebi_Sub_Nature = p.Field<string>("Sebi_Sub_Nature")

                               }).ToDataTable();

                RepeaterHybrid.DataSource = joinREs;// _dt;
                RepeaterHybrid.DataBind();
            }
            else if (id == 6)
            {
                DataTable _dt = AllMethods.getSebiSubNature(id);
                RepeaterSolution_Oriented.DataSource = _dt;
                RepeaterSolution_Oriented.DataBind();
            }
            else if (id == 5)
            {
                DataTable _dt = AllMethods.getSebiSubNature(id);
                RepeaterOther.DataSource = _dt;
                RepeaterOther.DataBind();
            }
        }
        [System.Web.Services.WebMethod]
        public static string getTopFundData(int SebiSubNatureId, int SebiNatureId,string period)
        {
            DataTable dtMain = new DataTable();
            period = period.ToUpper() == "FUND_SIZE" ? "" : period;
            DataTable dtResult = AllMethods.getSebiTopFundRank(10, 2, SebiNatureId, period, SebiSubNatureId, 2, -1, 500, -1, -1);
            dtMain = dtResult.Clone();
            foreach (DataRow drRes in dtResult.Rows)
            {
                dtMain.Rows.Add(drRes.ItemArray);
            }
            string JSONresult;
            JSONresult = Newtonsoft.Json.JsonConvert.SerializeObject(dtMain);

            return JSONresult;
        }
    }
}