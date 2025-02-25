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
    public partial class PortFolioView :MyBasePage
    {
        DataTable dt = new DataTable();
        int i = 0;
        protected string comID;
        protected void Page_Load(object sender, EventArgs e)
        {
            string sname=Request.QueryString["sname"].ToString();
            comID = Request.QueryString["comID"].ToString();
            if (!IsPostBack)
            {
                dt = Portfolio.GetdtPortDetail(sname);
               // LblScheme.Text = Request.QueryString["scheme"].ToString();
                LblScheme.Text = (Portfolio.GetShemeDetail(sname)).Rows[0]["sch_name"].ToString();
                string nature = (Portfolio.GetShemeDetail(sname)).Rows[0]["nature"].ToString();

                if (dt.Rows.Count != 0 && dt != null)
                {
                    LblFsizeAson.Text = dt.Rows[0]["fsizeAson"] != null ? Convert.ToDateTime(dt.Rows[0]["fsizeAson"]).ToString("MMM dd,yyyy") : "NA";
                    LblFundSize.Text = dt.Rows[0]["fund_Size"] != null ? Math.Round(double.Parse(dt.Rows[0]["fund_Size"].ToString()), 2, MidpointRounding.AwayFromZero).ToString() : "NA";
                    LblAssetAson.Text = dt.Rows[0]["AsssetAson"] != null ? Convert.ToDateTime(dt.Rows[0]["AsssetAson"]).ToString("MMM dd,yyyy") : "NA";
                    LblEquity.Text = dt.Rows[0]["Equity"] != null ? dt.Rows[0]["Equity"].ToString() + "%" : "NA";
                    LblDebt.Text = dt.Rows[0]["Debt"] != null ? dt.Rows[0]["Debt"].ToString() + "%" : "NA";
                    LblOthers.Text = dt.Rows[0]["Others"] != null ? dt.Rows[0]["Others"].ToString() + "%" : "NA";
                }
                dt.Clear();
                dt = Portfolio.GetTopTenHoldDetail(sname);
                if (dt.Rows.Count != 0 && dt != null)
                {
                    if (nature == "EQ")
                    {
                        InstrumentAlloc.InnerText = "SECTOR ALLOCATION";
                        InstrumentAlloc.HRef = "sector_graph.aspx?sch_name=" + sname + "&comID=" + comID;
                        CompletePort.HRef = "CompanyPortfolio.aspx?sch_code=" + sname + "&comID=" + comID;
                    }
                    else
                    {
                        InstrumentAlloc.InnerText = "INSTRUMENT ALLOCATION";
                        InstrumentAlloc.HRef = "sector_graph.aspx?sch_name=" + sname + "&comID=" + comID;
                        CompletePort.HRef = "CompanyPortfolio.aspx?sch_code=" + sname + "&comID=" + comID;
                    }
                    LbltoptenAson.Text=dt.Rows[0]["Date"].ToString()!=""?Convert.ToDateTime(dt.Rows[0]["Date"]).ToString("MMM dd,yyyy"):"NA";
                    DataRow[] drDebt = dt.Select("Nature='Debt'");
                    DataRow[] drEquity = dt.Select("Nature='EQ'");
                    DataRow[] drOther = dt.Select("Nature='Others'");

                    DataTable tblDebt = new DataTable();
                    DataTable tblEquity = new DataTable();
                    DataTable tblOther = new DataTable();
                    tblDebt = dt.Clone();
                    tblOther = dt.Clone();
                    tblEquity = dt.Clone();
                    foreach (DataRow row in drDebt)
                    {
                        tblDebt.ImportRow(row);
                        i += 1;
                        if (i == 10)
                        {
                            i = 0;
                            break;
                        }
                    }

                    foreach (DataRow row in drEquity)
                    {
                        tblEquity.ImportRow(row);
                        i += 1;
                        if (i == 10)
                        {
                            i = 0;
                            break;
                        }
                    }

                    foreach (DataRow row in drOther)
                    {

                        tblOther.ImportRow(row);
                        i += 1;
                        if (i == 10)
                        {
                            i = 0;
                            break;
                        }
                    }
                    if (tblDebt.Rows.Count != 0 && tblDebt != null)
                    {
                        LstToptenDebt.DataSource = tblDebt;
                        LstToptenDebt.DataBind();
                    }
                    if (tblEquity.Rows.Count != 0 && tblEquity != null)
                    {
                        LstToptenEquity.DataSource = tblEquity;
                        LstToptenEquity.DataBind();
                    }
                    if (tblOther.Rows.Count != 0 && tblOther != null)
                    {
                        LstToptenOther.DataSource = tblOther;
                        LstToptenOther.DataBind();
                    }
                }
            }
        }
    }
}