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
    public partial class FundPortfolio : MyBasePage
    {
        string MutcodeFromBase = string.Empty;
        string comID;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DrpCustom.lblfund.Text = "FUND HOUSE";
            this.DrpCustom.lblscheme.Text = "SCHEME NAME";
            //MutcodeFromBase = "MF024,MF057,MF059";
            if (!IsPostBack)
            {
                this.DrpCustom.ddlfund.Items.Clear();
                this.DrpCustom.ddlfund.DataSource = Nav.GetdtFund(this.PropMutCode);
                this.DrpCustom.ddlfund.AppendDataBoundItems = true;
                this.DrpCustom.ddlfund.Items.Insert(0, new ListItem("---", "0"));
                this.DrpCustom.ddlfund.DataTextField = "Mut_Name";
                this.DrpCustom.ddlfund.DataValueField = "Mut_Code";
                this.DrpCustom.ddlfund.DataBind();

                string mcodeByQuerystr = Request.QueryString["mcode"] != null ? Request.QueryString["mcode"].ToString() : null;
                if (mcodeByQuerystr != null)
                {
                    this.DrpCustom.ddlfund.SelectedIndex = this.DrpCustom.ddlfund.Items.IndexOf(this.DrpCustom.ddlfund.Items.FindByValue(mcodeByQuerystr));
                    ddlfund_selectedindexchanged(null, null);
                }
            }
        }
        public void ddlfund_selectedindexchanged(object sender, EventArgs e)
        {
            this.DrpCustom.ddlscheme.DataSource = Nav.Getdtscheme(this.DrpCustom.ddlfund.SelectedValue);
            this.DrpCustom.ddlscheme.Items.Clear();
            this.DrpCustom.ddlscheme.AppendDataBoundItems = true;
            this.DrpCustom.ddlscheme.Items.Insert(0, new ListItem("---", "0"));
            this.DrpCustom.ddlscheme.DataTextField = "short_name";
            this.DrpCustom.ddlscheme.DataValueField = "sch_code";
            this.DrpCustom.ddlscheme.DataBind();
            
        }
        protected void BtnGo_Click(object sender, EventArgs e)
        {
          //  Response.Redirect(Page.ResolveUrl("~/Pages/PortFolioView.aspx?scheme="+ this.DrpCustom.ddlscheme.SelectedItem.Text+"&sname=" + this.DrpCustom.ddlscheme.SelectedValue), true);
            Response.Redirect(Page.ResolveUrl("~/Pages/PortFolioView.aspx?sname=" + this.DrpCustom.ddlscheme.SelectedValue + "&comID=" + this.PropCompID), true);
        }
    }
}