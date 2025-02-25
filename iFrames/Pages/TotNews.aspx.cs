using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using iFrames.BLL;

namespace iFrames.Pages
{
    public partial class TotNews : MyBasePage
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtMutName = new DataTable();
                dtMutName = AllMethods.getMutName();
                DataRow drMutName = dtMutName.NewRow();
                drMutName["Mut_Name"] = "Choose any Option";
                drMutName["mut_code"] = "";
                dtMutName.Rows.InsertAt(drMutName, 0);
                ddlMutName.DataSource = dtMutName;
                ddlMutName.DataTextField = "Mut_Name";
                ddlMutName.DataValueField = "mut_code";
                ddlMutName.DataBind();

                string mcodeByQuerystr = Request.QueryString["mcode"] != null ? Request.QueryString["mcode"].ToString() : null;
                if (mcodeByQuerystr != null)
                {
                    this.ddlMutName.SelectedIndex = this.ddlMutName.Items.IndexOf(this.ddlMutName.Items.FindByValue(mcodeByQuerystr));                
                }

                DataTable dtYear = new DataTable();
                dtYear = AllMethods.getYears();
                DataRow dr=dtYear.NewRow();
                dr["yrs"] = "Choose a Year";                
                dtYear.Rows.InsertAt(dr,0);
                ddlYear.DataSource = dtYear;
                ddlYear.DataTextField = "yrs";
                ddlYear.DataValueField = "yrs";
                ddlYear.DataBind();

                DataTable dtMon = new DataTable();
                dtMon = AllMethods.getMonths();
                DataRow drMon = dtMon.NewRow();
                drMon["digimons"] = 0;
                drMon["charmon"] = "Choose a Month";
                dtMon.Rows.InsertAt(drMon, 0);
                ddlMonth.DataSource = dtMon;
                ddlMonth.DataTextField = "charmon";
                ddlMonth.DataValueField = "digimons";
                ddlMonth.DataBind();

                //News = getNewsHeadLines(Request.QueryString["req"]);
                //News = getNewsHeadLines("M");
                DataTable dtNews = AllMethods.getNewsHeadLines(false,"M");
                ViewState["Result"] = dtNews;
                lstResult.DataSource = dtNews;
                lstResult.DataBind();

            }

        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                lstResult.DataSource = AllMethods.getNewsHeadLines(true, "", ddlMutName.SelectedValue, ddlYear.SelectedValue.ToString(), ddlMonth.SelectedValue.ToString());
                lstResult.DataBind();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {            
                lstResult.DataSource = AllMethods.getNewsHeadLines(true, "", ddlMutName.SelectedValue, ddlYear.SelectedValue.ToString(), ddlMonth.SelectedValue.ToString());
                lstResult.DataBind(); 
        }            
    }
}