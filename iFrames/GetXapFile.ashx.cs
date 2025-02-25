using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFrames
{
    public class HttpXapHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            // get file name from request query string
            string fileName = context.Request["fileName"];
            // check if the file is valid -> its up to you to validate in a way that makes sense to you...
            if (!validateFile(fileName))
            {
                context.Response.Write("<br>Bad file request<br>");
                context.Response.End();

            }

            // set mime type to zip file because a xap file is actually a zip file
            context.Response.ContentType = "application/x-zip-compressed";
            context.Response.TransmitFile(context.Server.MapPath(fileName));

            context.Response.End();

        }

        // naive test for valid xap file -> just test if the file requested is actualy a .xap file
        public bool validateFile(string fileName)
        {

            fileName = fileName.ToUpper();
            if ((fileName.Length > 4) && (fileName.Substring(fileName.Length - 4).CompareTo(".XAP") == 0)
             )
                return true;
            else
                return false;
        }
        
        public bool IsReusable
        {

            get
            {
                return false;
            }

        }
    } 
}