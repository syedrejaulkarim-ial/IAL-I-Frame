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
    public partial class NewScheme : MyBasePage
    {
        protected DateTime ReqDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReqDate = Convert.ToDateTime(Request.QueryString["dt"].ToString());
                BindData(ReqDate);
            }
        }

        //protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    var dp = (sender as ListView).FindControl("dp") as DataPager;
        //    if (dp != null)
        //    {
        //        dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        //        ReqDate = Convert.ToDateTime(Request.QueryString["dt"].ToString());
        //        BindData(ReqDate);
        //    }
        //}

        protected void BindData(DateTime qdat)
        {
            lstNewSchemes.DataSource = IndustryUpdate.getNewSchemes(qdat);
            lstNewSchemes.DataBind();
        }
    }
}