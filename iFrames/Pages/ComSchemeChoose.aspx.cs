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
    public partial class ComSchemeChoose : MyBasePage
    {
        protected string flag;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { flag = "N"; }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string compName = txtCompPortfolio.Text.Trim();
            txtCompPortfolio.Text = "";
            if (ddlCompany.Visible==false)
            {
                DataTable dt = Schemes.getCompany(compName);
                if (dt.Rows.Count < 1)
                {
                    lblMsg.Text = "* No record found. Please try another";
                    txtCompPortfolio.Text = compName;
                    lblMsg.Visible = true;
                }                
                else
                {
                    if (lblMsg.Visible == true)
                        lblMsg.Visible = false;
                    DllDataBind(dt);
                    trCompName.Visible = true;
                    trPercent.Visible = true;
                    lblMsg.Visible = false;
                    txtCompPortfolio.Enabled = false;
                    txtCompPortfolio.Text = compName;
                    flag = "Y";
                }
            }
            else
            {
                txtCompPortfolio.Text = compName;
                lstSchemes.Visible = true;
                BindData();
            }
        }

        protected void DllDataBind(DataTable dtFund)
        {            
            DataRow drFund = dtFund.NewRow();
            drFund["c_name"] = "Select";            
            dtFund.Rows.InsertAt(drFund, 0);
            ddlCompany.DataSource = dtFund;
            ddlCompany.DataTextField = "c_name";
            ddlCompany.DataValueField = "c_name";
            ddlCompany.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = ddlCompany.Items.Count - 1; i >= 0; i--)
            {
                ddlCompany.Items.RemoveAt(i);
            }
            trCompName.Visible = false;
            trPercent.Visible = false;
            txtCompPortfolio.Enabled = true;
            txtCompPortfolio.Text = "";
            flag = "N";
            lstSchemes.Visible = false;
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                lstSchemes.DataSource = ViewState["CompanyInfo"];
                lstSchemes.DataBind();
            }
        }

        protected void BindData()
        {
            ViewState["CompanyInfo"] = Schemes.getSchemeInfos(ddlCompany.SelectedItem.Text, ddlPercentage.SelectedItem.Value.ToString());
            lstSchemes.DataSource = ViewState["CompanyInfo"];
            lstSchemes.DataBind();
        }
    }
}