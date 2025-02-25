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
    public partial class DivDec : MyBasePage
    {
        protected string ComID;   
        protected void Page_Load(object sender, EventArgs e)
        {  
            ComID=Request.QueryString["comID"];
            if (!IsPostBack)
            {
                BindEquity();
                BindDebt();
                BindBalance();                
            }
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                switch ((sender as ListView).ID.ToUpper())
                {
                    case "LSTEQUITY": 
                        BindEquity();
                        break;
                    case "LSTDEBT":
                        BindDebt();
                        break;
                    case "LSTBALANCE":
                        BindBalance();
                        break;
                }  
            }
        }

        private void BindEquity()
        {
            lstEquity.DataSource = AllMethods.getDivDecEquityFundData("Equity");
            lstEquity.DataBind();
        }

        private void BindDebt()
        {
            lstDebt.DataSource = AllMethods.getDivDecEquityFundData("Debt");
            lstDebt.DataBind();
        }

        private void BindBalance()
        {
            lstBalance.DataSource = AllMethods.getDivDecEquityFundData("Balance");
            lstBalance.DataBind();
        }
    }
}