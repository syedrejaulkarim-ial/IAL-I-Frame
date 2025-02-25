using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iFrames.BLL;
namespace iFrames.Pages
{
    
    public partial class amcSnapShot : MyBasePage
    {
        DataTable dt = new DataTable();
        int i = 0;
        string MutcodeFromBase = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //string muuu = PropMutCode;
            //MutcodeFromBase = "MF024,MF057,MF059";
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tblhide();", true);
                //DataTable dt = new DataTable();
                //dt = Schemes.GetdtTblAmc();
                DrpAmc.DataSource = Schemes.GetdtTblAmc(this.PropMutCode);                 
                DrpAmc.DataTextField = "AMC_Name";
                DrpAmc.DataValueField = "AMC_Code";
                DrpAmc.DataBind();                
                
            }
        }

        protected void BtnGo_Click(object sender, EventArgs e)
        {
            cleardata();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tblShow();", true);
            lblAmcName.Text = DrpAmc.SelectedItem.Text;
            
            dt.Clear();
            dt = Schemes.GetdtTblAmcDetail(DrpAmc.SelectedValue);
            if (dt.Rows.Count != 0 && dt != null)
            {
                lblAmcDetal.Text =  dt.Rows[0][1].ToString();
                LblKeyPerson.Text = dt.Rows[0]["Key_Persons"].ToString();
            }
            dt.Clear();
            dt = Schemes.GetdtTblAmcCount(DrpAmc.SelectedValue);
            LblNosheme.Text = dt.Rows[0][0].ToString();
            LblNoshemeOpt.Text = dt.Rows[0][1].ToString();
            dt.Clear();
            dt = Schemes.GetdtTblShmeCount(DrpAmc.SelectedValue);
            foreach (DataRow record in dt.Rows)
            {
                switch (record["nature"].ToString())
                {
                    case "Debt": 
                        LblDebt.Text = record["count"].ToString();
                        break;
                    case "Equity":
                        LblEquity.Text = record["count"].ToString();
                        break;
                    case "Equity & Debt":
                        LblEqDebt.Text = record["count"].ToString();
                        break;
                    case "Short Term Debt":
                        LblShortDebt.Text = record["count"].ToString();
                        break;
                    case "Gilt":
                        LblGlit.Text = record["count"].ToString();
                        break;
                    case "Money Market":
                        LblMoney.Text = record["count"].ToString();
                        break;
                }
            }

            dt.Clear();
            dt = Schemes.GetdtTblAmCorpus(DrpAmc.SelectedValue);
            LblCormgmt.Text ="Rs."+dt.Rows[0]["Corpus_Crs_"].ToString()+"Crs. as on "+Convert.ToDateTime(dt.Rows[0]["date"].ToString()).ToString("MMM dd, yyyy");
            dt.Clear();
            dt=Schemes.GetdtTblFundmgr(DrpAmc.SelectedValue);
            foreach (DataRow record in dt.Rows)
            {
                i = i + 1;
                if (i == dt.Rows.Count)
                {
                    LblFundmgr.Text += record["Fund_Manager1"].ToString() + ".";
                }
                else
                {
                    LblFundmgr.Text += record["Fund_Manager1"].ToString() + ",";
                }
            }
            dt.Clear();
            dt = Schemes.GetdtTblOpenSheme(DrpAmc.SelectedValue);
            ancNav.HRef = "Navs.aspx?mcode=" + dt.Rows[0]["mutcode"];
            ancNews.HRef = "TotNews.aspx?mcode=" + dt.Rows[0]["mutcode"];
            ancPortfolio.HRef = "FundPortfolio.aspx?mcode=" + dt.Rows[0]["mutcode"];
            //ancFundManager.HRef = "FundMangr.aspx?mcode=" + dt.Rows[0]["mutcode"];
            //ancDividendRecord.HRef = "DivHome.aspx?mcode=" + dt.Rows[0]["mutcode"];
            //ancContactDetail.HRef = "Navs.aspx?mcode=" + dt.Rows[0]["mutcode"];
            if (dt.Rows.Count != 0 && dt != null)
            {
                LstopenEndSchme.DataSource = dt;
                LstopenEndSchme.DataBind();
                ViewState["dtTblOpenShme"] = dt;
            }
            
        }
        //protected void lstVwLaunch_PreRender(object sender, EventArgs e)
        //{
        //    LstopenEndSchme.DataSource = ViewState["dtTblOpenShme"] as DataTable;
        //    LstopenEndSchme.DataBind();

        //}
        protected void Page_PreRender(object sender, EventArgs e)
        {
           
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tblShow();", true);
            LstopenEndSchme.DataSource = ViewState["dtTblOpenShme"] as DataTable;
            LstopenEndSchme.DataBind();
        }

        private void cleardata()
        {
            LblDebt.Text = "0";LblEquity.Text = "0";
            LblGlit.Text = "0"; LblMoney.Text = "0";
            LblEqDebt.Text = "0"; LblShortDebt.Text = "0";            
            lblAmcName.Text = "0";lblAmcDetal.Text = "0";
            LblNosheme.Text = "0";LblNoshemeOpt.Text = "0";
            LblCormgmt.Text = " "; LblKeyPerson.Text = " ";
            LblFundmgr.Text = " ";
        }

        protected void DrpAmc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "tblhide();", true);
            
        } 
    }
}