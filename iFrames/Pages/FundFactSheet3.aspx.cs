using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;

namespace iFrames.Pages
{
    public partial class FundFactSheet3 : MyBasePage
    {
        DataTable dt ;
        public string schCode, shortName, DateHeader;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            schCode = Request.QueryString["sch"];
            DataTable dt = FactSheets.getFactSheet3(schCode);
            if (dt.Rows.Count > 0)
            {
                lblFama.Text = Convert.ToDouble(dt.Rows[0]["fama"]) == 0 ? "NA" : Math.Round(Convert.ToDecimal(dt.Rows[0]["fama"]), 2).ToString();
                lblCorrelation.Text = Convert.ToDouble(dt.Rows[0]["beta_Correlation"]) == 0 ? "NA" : Math.Round(Convert.ToDecimal(dt.Rows[0]["beta_Correlation"]), 2).ToString();
                lblSortino.Text = Convert.ToDouble(dt.Rows[0]["Sortino"]) == 0 ? "NA" : Math.Round(Convert.ToDecimal(dt.Rows[0]["Sortino"]), 2).ToString();
                lblTreynor.Text = Convert.ToDouble(dt.Rows[0]["Treynor"]) == 0 ? "NA" : Math.Round(Convert.ToDecimal(dt.Rows[0]["Treynor"]), 2).ToString();
                lblBeta.Text = Convert.ToDouble(dt.Rows[0]["beta"]) == 0 ? "NA" : Math.Round(Convert.ToDecimal(dt.Rows[0]["beta"]), 2).ToString();
                lblSharpe.Text = Convert.ToDouble(dt.Rows[0]["sharp"]) == 0 ? "NA" : Math.Round(Convert.ToDecimal(dt.Rows[0]["sharp"]), 2).ToString();
                lblStDiv.Text = Convert.ToDouble(dt.Rows[0]["stdv"]) == 0 ? "NA" : Math.Round(Convert.ToDecimal(dt.Rows[0]["stdv"]), 2).ToString();
                lblmean.Text = Convert.ToDouble(dt.Rows[0]["average"]) == 0 ? "NA" : Math.Round(Convert.ToDecimal(dt.Rows[0]["average"]), 2).ToString();
                lblSinceInsp.Text = Convert.ToString(dt.Rows[0]["since_incept"]) != "NA" && Convert.ToString(dt.Rows[0]["since_incept"]).Trim() != "" ? Math.Round(Convert.ToDecimal(dt.Rows[0]["since_incept"]), 2).ToString() : "NA";
                lbl5yrs.Text = Convert.ToString(dt.Rows[0]["per_5yr"]) != "NA" && Convert.ToString(dt.Rows[0]["per_5yr"]).Trim()!="" ? Math.Round(Convert.ToDecimal(dt.Rows[0]["per_5yr"]), 2).ToString() : "NA";
                lbl3yrs.Text = Convert.ToString(dt.Rows[0]["per_3yr"]) != "NA" && Convert.ToString(dt.Rows[0]["per_3yr"]).Trim() !="" ? Math.Round(Convert.ToDecimal(dt.Rows[0]["per_3yr"]), 2).ToString() : "NA";
                lbl1yr.Text = Convert.ToString(dt.Rows[0]["per_1yr"]) != "NA" && Convert.ToString(dt.Rows[0]["per_1yr"]).Trim()!="" ? Math.Round(Convert.ToDecimal(dt.Rows[0]["per_1yr"]), 2).ToString() : "NA";
                lbl182days.Text = Convert.ToString(dt.Rows[0]["per_182days"]) != "NA" && Convert.ToString(dt.Rows[0]["per_182days"]).Trim()!=""? Math.Round(Convert.ToDecimal(dt.Rows[0]["per_182days"]), 2).ToString() : "NA";
                lbl91days.Text = Convert.ToString(dt.Rows[0]["per_91days"]) != "NA" && Convert.ToString(dt.Rows[0]["per_91days"]).Trim()!="" ? Math.Round(Convert.ToDecimal(dt.Rows[0]["per_91days"]), 2).ToString() : "NA";
                lbl30days.Text = Convert.ToString(dt.Rows[0]["per_30days"]) != "NA" && Convert.ToString(dt.Rows[0]["per_30days"]) !=""? Math.Round(Convert.ToDecimal(dt.Rows[0]["per_30days"]), 2).ToString() : "NA";
                DateHeader = "Scheme Performance (%) as on " + Convert.ToDateTime(dt.Rows[0]["maxDate"].ToString()).ToString("MMM dd, yyyy");
                shortName = dt.Rows[0]["short_name"].ToString();
            }
            else
            {
                lblFama.Text = "NA";
                lblCorrelation.Text = "NA";
                lblSortino.Text = "NA";
                lblTreynor.Text = "NA";
                lblBeta.Text = "NA";
                lblSharpe.Text = "NA";
                lblStDiv.Text = "NA";
                lblmean.Text = "NA";
                lblSinceInsp.Text = "NA";
                lbl5yrs.Text = "NA";
                lbl3yrs.Text = "NA";
                lbl1yr.Text = "NA";
                lbl182days.Text = "NA";
                lbl91days.Text = "NA";
                lbl30days.Text = "NA";
                DateHeader = "NA";
                shortName = "NA";
            }
        }
    }
}