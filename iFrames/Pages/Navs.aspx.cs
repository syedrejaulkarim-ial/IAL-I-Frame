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
    public partial class Navs : MyBasePage
    {
        DataTable dt = new DataTable();
        string MutcodeFromBase = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //MutcodeFromBase = "MF024,MF057,MF059";
            this.DrpCustom.lblfund.Text = "FUND HOUSE";
            this.DrpCustom.lblscheme.Text = "SCHEME NAME";
            if (!IsPostBack)
            {
                this.DrpCustom.ddlfund.Items.Clear();
                this.DrpCustom.ddlfund.DataSource = Nav.GetdtFund(this.PropMutCode);
                this.DrpCustom.ddlfund.AppendDataBoundItems = true;
                this.DrpCustom.ddlfund.Items.Insert(0, new ListItem("---", "0"));
                this.DrpCustom.ddlfund.DataTextField = "Mut_Name";
                this.DrpCustom.ddlfund.DataValueField = "Mut_Code";
                this.DrpCustom.ddlfund.DataBind();
                drpnavType.SelectedIndex = 0;

                //Dim ddlControl As DropDownList
                //ddlControl = CType(e.Item.FindControl("ddlCardType"), DropDownList)
                //ddlControl.SelectedIndex = ddlControl.Items.IndexOf(ddlControl.Items.FindByText(strType))

                string mcodeByQuerystr = Request.QueryString["mcode"] != null ? Request.QueryString["mcode"].ToString() : null;
                if (mcodeByQuerystr != null)
                {
                    this.DrpCustom.ddlfund.SelectedIndex = this.DrpCustom.ddlfund.Items.IndexOf(this.DrpCustom.ddlfund.Items.FindByValue(mcodeByQuerystr));
                    ddlfund_selectedindexchanged(null, null);
                }

            }
            if (drpnavType.SelectedItem.Text == "Historical Nav")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tdShow();", true);
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
            drpnavType.SelectedIndex = 0;
        }

        protected void BtnGo_Click(object sender, EventArgs e)
        {
            if (drpnavType.SelectedItem.Text == "Latest Nav")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tblHideHistNav();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(),"pqr", "tblShowLatNav();", true);
                dt.Clear();
                dt = Nav.GetdtNav(this.DrpCustom.ddlscheme.SelectedValue);
                //if (dt != null && dt.Rows.Count != 0)
                //{
                    LblNavDt.Text = Convert.ToDateTime(dt.Rows[0]["date"].ToString()).ToString("MMM dd,yyyy");
                    LblNav.Text = dt.Rows[0]["navrs"].ToString() != "" ? Math.Round(double.Parse(dt.Rows[0]["navrs"].ToString()), 3).ToString() : "NA";
                    Lbl91.Text = dt.Rows[0]["ninetyonedays"].ToString() != "" ? Math.Round(double.Parse(dt.Rows[0]["ninetyonedays"].ToString()), 2).ToString() : "NA";
                    Lbl182.Text = dt.Rows[0]["oneeighttwodays"].ToString() != "" ? Math.Round(double.Parse(dt.Rows[0]["oneeighttwodays"].ToString()), 2).ToString() : "NA";
                    Lbl1yr.Text = dt.Rows[0]["oneyr"].ToString() != "" ? Math.Round(double.Parse(dt.Rows[0]["oneyr"].ToString()), 2).ToString() : "NA";
                    LblperAsondt.Text = "Performance % as on " + Convert.ToDateTime(dt.Rows[0]["maxdt"].ToString()).ToString("MMM dd,yyyy");
                    LblSchCode.Text = this.DrpCustom.ddlscheme.SelectedValue.ToString();
                    lblSchName.Text = this.DrpCustom.ddlscheme.SelectedItem.Text.ToString();
                    AncScode.HRef = "FundFactSheet1.aspx?sname=" + this.DrpCustom.ddlscheme.SelectedValue.ToString();
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "xyz", "alert('no data to show');", true);
                //}
            }

            if (drpnavType.SelectedItem.Text == "Historical Nav")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tblHideLatNav();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "xyz", "tblShowHistNav();", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tdShow();", true);
                dt.Clear();
               
                //string histnavdate = (drpMon.SelectedItem.Text + "/" + drpDay.SelectedItem.Text + "/" + drpYr.SelectedItem.Text).ToString();
                DateTime dtadd = new DateTime(Convert.ToInt32(drpYr.SelectedItem.Text), Convert.ToInt32(drpMon.SelectedItem.Text), Convert.ToInt32(drpDay.SelectedItem.Text));

                dt = Nav.GetdtHistNav(this.DrpCustom.ddlscheme.SelectedValue, dtadd.AddDays(-3).ToString("MM/dd/yyyy"), dtadd.AddDays(4).ToString("MM/dd/yyyy"));
                //if (dt != null && dt.Rows.Count != 0)
                //{
                    lblScheme.Text = this.DrpCustom.ddlscheme.SelectedItem.Text;
                    AncScheme.HRef = "FundFactSheet1.aspx?sname=" + this.DrpCustom.ddlscheme.SelectedValue.ToString();
                    lstVwHistNav.DataSource = dt;
                    lstVwHistNav.DataBind();
                   // ShowHistNav();   
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "xyz", "alert('no data to show');", true);
                //}
            }
            
            
        }

        protected void drpnavType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpnavType.SelectedItem.Text == "Historical Nav")
            {

                ShowHistNav();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tblHideLatNav();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "xyz", "tblHideHistNav();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tblHideLatNav();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "xyz", "tblHideHistNav();", true);
            }
        }

        private void ShowHistNav()
        {
            drpDay.Items.Clear();
            drpMon.Items.Clear();
            drpYr.Items.Clear();
            dt.Clear();
            for (int dy = 1; dy <= 31; dy++)
            {
                drpDay.Items.Add(new ListItem(dy.ToString()));
            }
            for (int mon = 1; mon <= 12; mon++)
            {
                drpMon.Items.Add(new ListItem(mon.ToString()));
            }
            dt = Nav.GetdtCloseyr(this.DrpCustom.ddlscheme.SelectedValue);
            if (dt != null && dt.Rows.Count != 0)
            {
                int yr = Convert.ToInt32(dt.Rows[0][0]);

                for (int year = yr; year <= Convert.ToInt32(DateTime.Today.Year); year++)
                {
                    drpYr.Items.Add(new ListItem(year.ToString()));
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tdShow();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tdShow();", true);
                drpYr.Items.Add(new ListItem(Convert.ToString(DateTime.Today.Year)));
            }
            
        }
       
    }
}