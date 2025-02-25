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
    public partial class FundRedeemedRecently : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected void BindInYear()
        {
            lstInYear.DataSource = FactSheets.getFundRedeemedInYear(txtSchemes.Text.Trim(), this.PropMutCode);
            lstInYear.DataBind();
        }

        protected void BindTillDate(){
            lstTillDate.DataSource = FactSheets.getFundRedeemedInYear(txtSchemes.Text.Trim(), this.PropMutCode);
            lstTillDate.DataBind();
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                switch ((sender as ListView).ID.ToUpper())
                {
                    case "LSTINYEAR": 
                        BindInYear();
                        break;
                    case "LSTTILLDATE":
                        BindTillDate();
                        break;                    
                }  
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            lblinYear.Text = "SCHEME REDEEMED IN " + DateTime.Today.Year;
            PlaceHolder1.Visible = true;
            BindInYear();
            BindTillDate();
        }
        
    }
}