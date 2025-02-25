using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WkHtmlToXSharp;
using NUnit.Framework;
using System.IO;
using System.Data;
using System.Collections;

namespace iFrames.Pages
{
    public partial class BigDaySipDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["path"] != null)
                {
                    iFrames.Chart.Chart objChart = new iFrames.Chart.Chart();
                    objChart.DownloadFile(Convert.ToString(Request.QueryString["path"]), true);
                    //if (Request.QueryString["check"] != null)
                    //{
                    //objChart.DownloadFile(Convert.ToString(Request.QueryString["check"]), Convert.ToString(Request.QueryString["path"]), true);
                    //}
                }
            }
        }

    }
}