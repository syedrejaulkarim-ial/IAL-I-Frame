using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace iFrames.Pages
{
    public partial class CompanyPortfolio : MyBasePage
    {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            string sname=Request.QueryString["sch_code"].ToString();
            if (!IsPostBack)
            {
                dt = Portfolio.GetCompletePortDetail(sname);
               // TextWriter txw = new StringWriter();
              // DataSet ds = new DataSet();
               // ds.Tables.Add(dt);
               // string a=ds.GetXml()
               //ds.Tables[0].WriteXml(txw);
                //ds.ReadXml(Server.MapPath("../scheme.xml"));
               //ds.ReadXml(Server.MapPath("../Merge.xml"));
                LblPortScheme.Text = (Portfolio.GetShemeDetail(sname)).Rows[0]["sch_name"].ToString();
                string nature = (Portfolio.GetShemeDetail(sname)).Rows[0]["nature"].ToString();
                if (dt.Rows.Count != 0 && dt != null)
                {
                    if (nature == "EQ")
                    {
                        InstrumentAlloc.InnerText = "SECTOR ALLOCATION";
                        InstrumentAlloc.HRef = "sector_graph.asp?sch_name=" + sname;
                        CompletePort.HRef = "PortFolioView.aspx?sname=" + sname;
                    }
                    else
                    {
                        InstrumentAlloc.InnerText = "INSTRUMENT ALLOCATION";
                        InstrumentAlloc.HRef = "sector_graph.asp?sch_name=" + sname;
                        CompletePort.HRef = "PortFolioView.aspx?sname=" + sname;
                    }
                    LblComportAson.Text = dt.Rows[0]["Date"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["Date"]).ToString("MMM dd,yyyy") : "NA";
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
                        
                    }

                    foreach (DataRow row in drEquity)
                    {
                        tblEquity.ImportRow(row);
                        
                    }

                    foreach (DataRow row in drOther)
                    {

                        tblOther.ImportRow(row);
                        
                    }
                    if (tblDebt.Rows.Count != 0 && tblDebt != null)
                    {
                        LstComPortDebt.DataSource = tblDebt;
                        LstComPortDebt.DataBind();
                    }
                    if (tblEquity.Rows.Count != 0 && tblEquity != null)
                    {
                        LstComPortEquity.DataSource = tblEquity;
                        LstComPortEquity.DataBind();
                    }
                    if (tblOther.Rows.Count != 0 && tblOther != null)
                    {
                        LstComPortOther.DataSource = tblOther;
                        LstComPortOther.DataBind();
                    }
                }
            }
        }

        protected void LstComPortEquity_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            string sname = Request.QueryString["sch_code"].ToString();
            var dp = (sender as ListView).FindControl("EquityDataPager") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                dataBind(Portfolio.GetCompletePortDetail(sname));
            }
        }
        private void dataBind(DataTable dt)
        {
            DataRow[] drEquity = dt.Select("Nature='EQ'");
            DataTable tblEquity = new DataTable();
            tblEquity = dt.Clone();
            foreach (DataRow row in drEquity)
            {
                tblEquity.ImportRow(row);
            }
            LstComPortEquity.DataSource = tblEquity;
            LstComPortEquity.DataBind();
        }


    }

}
