using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;
namespace iFrames.Pages
{
    public partial class CompareFund : MyBasePage
    {
        string nature1 = string.Empty;
        string nat_sp = string.Empty; string subnature1 = string.Empty;
        string MutcodeFromBase = string.Empty;
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            //MutcodeFromBase = "MF024,MF057,MF059";
            if (!IsPostBack)
            {
                DrpCat.Items.Clear();
                DrpCat.Items.Add(new ListItem("Select", "0")); DrpCat.Items.Add(new ListItem("Equity", "1"));
                DrpCat.Items.Add(new ListItem("ETF", "2")); DrpCat.Items.Add(new ListItem("Fund of Funds", "3"));
                DrpCat.Items.Add(new ListItem("Liquid", "4")); DrpCat.Items.Add(new ListItem("Equity (Tax Planning)", "5"));
                DrpCat.Items.Add(new ListItem("Equity (Diversified)", "6")); DrpCat.Items.Add(new ListItem("Equity (Sector)", "7"));
                DrpCat.Items.Add(new ListItem("Balanced", "8")); DrpCat.Items.Add(new ListItem("Equity (Index)", "9"));
                DrpCat.Items.Add(new ListItem("Debt", "10")); DrpCat.Items.Add(new ListItem("Gilt", "11"));
                DrpCat.Items.Add(new ListItem("Debt (Short Term)", "12")); DrpCat.Items.Add(new ListItem("Debt (Income)", "13"));
                DrpCat.Items.Add(new ListItem("Debt (MIP)", "14")); DrpCat.Items.Add(new ListItem("ALL", "15"));

                LstBxIndex.DataSource = Nav.GetdtIndex();
                LstBxIndex.AppendDataBoundItems = true;
                LstBxIndex.Items.Insert(0, "Select");
                LstBxIndex.DataTextField = "ind_name";
                LstBxIndex.DataValueField = "ind_code";
                LstBxIndex.DataBind();
            }
        }

        protected void DrpCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MutcodeFromBase = "MF024,MF057,MF059";
            LstBxFund.Items.Clear();
            LstBxCompFund.Items.Clear();
            switch (DrpCat.SelectedItem.Text.ToString())
            {
                case "Equity":
                    nature1 = "equity";
                    break;
                case "ETF":
                    nature1 = "etf";nat_sp="";
                    break;
                case "Fund of Funds":
                    nature1="Fund of Funds";
                    break;

                case "Liquid":
                    nature1="liquid";
                    break;
                case "Equity (Tax Planning)":
                    nature1 = "Equity";
                    subnature1 = "Tax Planning";
                    break;
                case "Equity (Diversified)":
                    nature1 = "Equity";
                    subnature1 = "Diversified";
                    break;
                case "Equity (Sector)":
                    nature1 = "Equity";
                    subnature1 = "Sector";
                    break;
                case "Balanced":
                    nature1="Balanced";
                    break;
                case "Equity (Index)":
                    nature1 = "Equity";
                    subnature1 = "index";
                    break;
                case "Debt":
                    nature1="debt";
                    break;
                case "Gilt":
                    nature1="gilt";
                    break;
                case "Debt (Short Term)":
                    nature1 = "Debt";
                    subnature1 = "Short term";
                    break;
                case "Debt (Income)":
                   nature1 = "Debt";
                   subnature1 = "Income";
                    break;
                case "Debt (MIP)":
                    nature1 = "Debt";
                    subnature1 = "MIP";
                    break; 
            }
            dt = Nav.GetdtFundList(nature1, subnature1, nat_sp,this.PropMutCode);
            if (dt!= null && dt.Rows.Count != 0)
            {
                LstBxFund.DataSource = dt;
                LstBxFund.DataTextField = "short_name";
                LstBxFund.DataValueField="sch_code";
                LstBxFund.DataBind();
            }
        }

        protected void BtnForward_Click(object sender, EventArgs e)
        {
            for (int i = LstBxFund.Items.Count - 1; i >= 0; i--)
            {
                if (LstBxFund.Items[i].Selected == true && this.LstBxCompFund.Items.Contains(LstBxFund.Items[i]) == false)
                {
                    LstBxCompFund.Items.Add(new ListItem(LstBxFund.Items[i].ToString(),LstBxFund.Items[i].Value));
                    //ListItem li = ListBox1.Items[i];
                    //ListBox1.Items.Remove(li);               
                }
                
            } 

        }

        protected void BtnBackward_Click(object sender, EventArgs e)
        {
            for (int i = LstBxCompFund.Items.Count - 1; i >= 0; i--)
            {
                if (LstBxCompFund.Items[i].Selected == true)
                {
                    LstBxCompFund.Items.Remove(LstBxCompFund.Items[i]);                 
                }
            }      

        }

        protected void BtnCompare_Click(object sender, EventArgs e)
        {
            string sname = null; string exchange = null;
            for (int i = LstBxCompFund.Items.Count - 1; i >= 0; i--)
            {
                sname = sname + LstBxCompFund.Items[i].Value + ",";
            }
            for (int i = LstBxIndex.Items.Count - 1; i >= 0; i--)
            {
                if (LstBxIndex.Items[i].Selected == true)
                {
                    exchange = exchange + LstBxIndex.Items[i].Value + ",";
                }
            }   
            Response.Redirect(Page.ResolveUrl("~/Pages/ComapreFundRpt.aspx?comid="+this.PropCompID+"&sname=" + sname+"&exchange="+exchange), true);
        }
    }
}