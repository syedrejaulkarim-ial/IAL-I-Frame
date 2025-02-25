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
    public partial class FundFactSheet : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtFund = AllMethods.getUcontrolFundHouse(this.PropMutCode);                
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
            }
        }

        public void ddlfund_selectedindexchanged(object sender, EventArgs e)
        {
            DataTable dtScheme = AllMethods.getUcontrolSchemeName("FundFactSheet", UControl.ddlfund.SelectedValue.ToString());
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
            Response.Redirect("FundFactSheet1.aspx?sch=" + UControl.ddlscheme.SelectedValue.ToString() + "&comID=" + this.PropCompID);            
        }
    }
}