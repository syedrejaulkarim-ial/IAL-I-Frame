using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace iFrames.Pages
{
    public partial class SchemWatch : MyBasePage
    {
        DataSet ds = new DataSet();
        int flagodd = 0; int flageven = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {


                ds.ReadXml(Server.MapPath("../App_Data/SchemeWatch.xml"));
                LstSchemeWatch.DataSource = ds;
                LstSchemeWatch.DataBind();
            }
        }
        protected void LstSchemeWatch_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            ds.Clear();
            var dp = (sender as ListView).FindControl("SchemeWatchDataPager") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                ds.ReadXml(Server.MapPath("../App_Data/SchemeWatch.xml"));
                LstSchemeWatch.DataSource = ds;
                LstSchemeWatch.DataBind();
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

            return null ;
        }
    }
}