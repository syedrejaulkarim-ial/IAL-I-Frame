using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;
namespace iFrames.Pages
{
    public partial class SchemeConverted : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
            }
        }

        protected void BindInYear()
        {
            lstConvertedScheme.DataSource = FactSheets.getShemeConveretd(txtSchemes.Text.Trim());
            lstConvertedScheme.DataBind();
        }

        
        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                BindInYear();
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "return test();", true);
            BindInYear();
        }
    }
}