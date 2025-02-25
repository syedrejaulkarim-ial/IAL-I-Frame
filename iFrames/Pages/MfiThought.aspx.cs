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
    public partial class MfiThought :MyBasePage
    {
        int flagodd = 0; int flageven = 0;
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                lstResult.DataSource = AllMethods.getThoughts();
                lstResult.DataBind();
            }            
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                lstResult.DataSource = AllMethods.getThoughts();
                lstResult.DataBind();
            }
        }

        protected string GetAlterColor(int displayindex)
        {

            if (displayindex % 2 != 0)
            {

                if (flagodd == 0)
                {
                    flagodd = 1;
                    return "AlternateRow";

                }
                else
                {
                    flagodd = 0;
                    return "ListtableRow";

                }

            }

            if (displayindex % 2 == 0)
            {

                if (flageven == 0)
                {
                    flageven = 1;
                    return "AlternateRow";

                }
                else
                {
                    flageven = 0;
                    return "ListtableRow";

                }

            }

            return null;
        }
        
    }
}