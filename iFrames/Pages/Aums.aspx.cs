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
    public partial class Aums : MyBasePage
    {
        DataTable dt = new DataTable();
        Int64 OpenEnd = 0; Int64 CloseEnd = 0; Int64 Total = 0;
        protected int lastrow=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dt = Aum.GetNatureAum();

                string Sum_openaum = dt.Compute("sum(openaum)", "").ToString();
                string Sum_closeaum = dt.Compute("sum(closeaum)", "").ToString();
                string Sum_Total = dt.Compute("sum(Total)", "").ToString();

                DataRow draddsum = dt.NewRow();

                draddsum["nature"] = "Total";
                draddsum["openaum"] = Sum_openaum;
                draddsum["closeaum"] = Sum_closeaum;
                draddsum["Total"] = Sum_Total;
                dt.Rows.Add(draddsum);
                lastrow = dt.Rows.Count;
                //if (dt.Rows.Count != 0 && dt != null)
                //{
                    LstNatureAum.DataSource = dt;
                    LstNatureAum.DataBind();
                    LblAsonDate.Text = Convert.ToDateTime(dt.Rows[0]["date"]).ToString("MMM dd,yyyy");
                //}
            }
        }
        protected Int64 GetOpenEnd(Int64 getsale)
        { OpenEnd += getsale; return getsale; }

        protected Int64 GetTotalOpenEnd()
        { return OpenEnd; }
        protected Int64 GetCloseEnd(Int64 getsale)
        { CloseEnd += getsale; return getsale; }

        protected Int64 GetTotalCloseEnd()
        { return CloseEnd; }
        protected Int64 GetTotal(Int64 getsale)
        { Total += getsale; return getsale; }

        protected Int64 GetTotalTotal()
        { return Total; }
        protected void BtnBack_Click(object sender, EventArgs e)
        {
           // Page.ClientScript.RegisterStartupScript(this.GetType(), "ttt", "back();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ttt", "back();", true);
        }
    }
}