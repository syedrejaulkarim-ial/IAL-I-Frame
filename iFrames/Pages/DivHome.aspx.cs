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

    public partial class DivHome : MyBasePage
    {
        public string PageHead;
        protected void Page_Load(object sender, EventArgs e)        {
           
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtFund = AllMethods.getUcontrolFundHouse();
                UControl.lblfund.Text = "Fund House";
                UControl.lblscheme.Text = "Scheme name";
                DataRow drFund = dtFund.NewRow();
                drFund["Mut_Name"] = "Select";
                drFund["Mut_Code"] = "";
                dtFund.Rows.InsertAt(drFund, 0);
                UControl.ddlfund.DataSource = dtFund;
                UControl.ddlfund.DataTextField = "Mut_Name";
                UControl.ddlfund.DataValueField = "Mut_Code";
                UControl.ddlfund.DataBind();

                string mcodeByQuerystr = Request.QueryString["mcode"] != null ? Request.QueryString["mcode"].ToString() : null;
                if (mcodeByQuerystr != null)
                {
                    this.UControl.ddlfund.SelectedIndex = this.UControl.ddlfund.Items.IndexOf(this.UControl.ddlfund.Items.FindByValue(mcodeByQuerystr));
                    ddlfund_selectedindexchanged(null, null);
                }
            }
            
        }

        public void ddlfund_selectedindexchanged(object sender, EventArgs e)
        {
            DataTable dtScheme = AllMethods.getUcontrolSchemeName("DivHome", UControl.ddlfund.SelectedValue.ToString());
            DataRow drFund = dtScheme.NewRow();
            drFund["sch_code"] = "";
            drFund["short_name"] = "--";
            dtScheme.Rows.InsertAt(drFund, 0);
            UControl.ddlscheme.DataSource = dtScheme;
            UControl.ddlscheme.DataTextField = "short_name";
            UControl.ddlscheme.DataValueField = "sch_code";
            UControl.ddlscheme.DataBind();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            DataTable dtResult = AllMethods.getDivHomeResult(UControl.ddlscheme.SelectedValue.ToString());
            DataRow drow =dtResult.Rows[0];
            PageHead = "Dividend history of " + drow["sch_name"].ToString();
            grdResult.DataSource=dtResult;
            grdResult.DataBind();
        }

        protected void grdResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResult.PageIndex = e.NewPageIndex;
            DataTable dtResult = AllMethods.getDivHomeResult(UControl.ddlscheme.SelectedValue.ToString());
            grdResult.DataSource = dtResult;
            grdResult.DataBind();
        }
    }
}