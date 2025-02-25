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
    public partial class FundFactSheet4 : MyBasePage
    {
        public string schCode, shortName;
        protected void Page_Load(object sender, EventArgs e)
        {
            schCode=Request.QueryString["sch"];
            DataTable dtPortfolio = FactSheets.getPortfolioAttribute(schCode);
            DataTable dtTopTen = FactSheets.getTopTenHoldings(schCode);       

            if (dtPortfolio.Rows.Count > 0)
            {
                
                lblPE.Text = dtPortfolio.Rows[0]["PE"] == null ? "NA" : Math.Round(Convert.ToDecimal(dtPortfolio.Rows[0]["PE"]), 2).ToString() + " as on " + Convert.ToDateTime(dtPortfolio.Rows[0]["CalcDate"].ToString()).ToString("MMM, yyyy");
                lblPB.Text = dtPortfolio.Rows[0]["PB"] == null ? "NA" : Math.Round(Convert.ToDecimal(dtPortfolio.Rows[0]["PB"]), 2).ToString() + " as on " + Convert.ToDateTime(dtPortfolio.Rows[0]["CalcDate"].ToString()).ToString("MMM, yyyy");
                lblDivYield.Text = dtPortfolio.Rows[0]["DivYield"] == null ? "NA" : Math.Round(Convert.ToDecimal(dtPortfolio.Rows[0]["DivYield"]), 2).ToString() + " as on " + Convert.ToDateTime(dtPortfolio.Rows[0]["CalcDate"].ToString()).ToString("MMM, yyyy");
                lblMrkCap.Text = dtPortfolio.Rows[0]["MrkCap"].ToString() == "NA" ? dtPortfolio.Rows[0]["MrkCap"].ToString() : dtPortfolio.Rows[0]["MrkCap"].ToString() + Convert.ToDateTime(dtPortfolio.Rows[0]["CalcDate"].ToString()).ToString("MMM, yyyy");
                lblLarge.Text = dtPortfolio.Rows[0]["Large"].ToString();
                lblMid.Text = dtPortfolio.Rows[0]["Mid"].ToString();
                lblSmall.Text = dtPortfolio.Rows[0]["Small"].ToString();
                lblStocks.Text = FactSheets.getProtfolioStockNo(schCode);
                lblExpRatio.Text = dtPortfolio.Rows[0]["actual_per"] == null ? "NA" : Math.Round(Convert.ToDecimal(dtPortfolio.Rows[0]["actual_per"]), 2).ToString();
            }
            if (dtTopTen.Rows.Count > 0)
            {
                DataView dvTopTen = dtTopTen.DefaultView;
                //dvTopTen.RowFilter = "Nature='eq' or Nature='debt'";
                lstTop10Holding.DataSource = dvTopTen;
                lstTop10Holding.DataBind();


                lblTop5.Text = FactSheets.getTop5Holding(schCode, dtTopTen.Rows[0]["SN_Nature"].ToString(), dtTopTen.Rows[0]["Date"].ToString());
                shortName = dtTopTen.Rows[0]["SN"].ToString();
                if (dtTopTen.Rows[0]["SN_Nature"].ToString().ToLower() == "equity")
                {
                    DataTable dtInOut = FactSheets.getInOut(schCode, dtTopTen.Rows[0]["Date"].ToString(), "In");
                    if (dtInOut.Rows.Count < 1)
                        lblIn.Text = "No Changes";
                    else
                    {
                        grdIn.DataSource = dtInOut;
                        grdIn.DataBind();
                        dtInOut.Clear();
                    }
                    dtInOut = FactSheets.getInOut(schCode, dtTopTen.Rows[0]["Date"].ToString(), "Out");
                    if (dtInOut.Rows.Count < 1)
                        lblOut.Text = "No Changes";
                    else
                    {
                        grdOut.DataSource = dtInOut;
                        grdOut.DataBind();
                        dtInOut.Clear();
                    }
                }
                else
                {
                    lblIn.Text = "NA";
                    lblOut.Text = "NA";
                }
            }
            else
            {
                lblTop10.Text = "Data not available.";
            }
            DataTable dtSectAlot = FactSheets.getSectorAllocation(schCode);
            if (dtSectAlot.Rows.Count > 0)
            {
                grdSecAlot.DataSource = dtSectAlot;
                grdSecAlot.DataBind();
            }
            else
                lblSecAlot.Text = "Data not available";

            DataTable dtAssettAlot = FactSheets.getAssetAllocation(schCode);
            if (dtAssettAlot.Rows.Count > 0)
            {
                grdAssetAlot.DataSource = dtAssettAlot;
                grdAssetAlot.DataBind();
            }
            else
                lblSecAlot.Text = "Data not available";
        }
    }
}