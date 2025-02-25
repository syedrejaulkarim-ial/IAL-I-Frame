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

namespace iFrames.Chart
{
    public partial class ChartDownload : System.Web.UI.Page
    {
        Chart objChart = new Chart();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["path"]!=null)
                // {
                //objChart.DownloadFile(Convert.ToString(Request.QueryString["path"]), true);

                //}
                if (Request.QueryString["filename"] != null)
                {
                    if (Request.QueryString["opt"] != null)
                    {
                        var temp = new Chart()._SimpleConversion(Request.QueryString["opt"].ToString(), Request.QueryString["filename"].ToString(), Request.QueryString["cname"].ToString());

                        if (temp != null)
                        {
                            HttpContext.Current.Response.ContentType = "application/pdf";
                            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["filename"].ToString());
                            HttpContext.Current.Response.BinaryWrite(temp);
                        }
                        else
                            HttpContext.Current.Response.Write("Nothing to print....");
                        HttpContext.Current.Response.End();
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Close();
                    }


                }

            }
        }

    }
}
