using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace iFrames.HttpModule
{
    public class SimpleHandler : IHttpHandler
    {
        #region IHttpHandler Members  

        bool IHttpHandler.IsReusable
        {
            get { return true; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            response.Write("<html><body><h1>Wow.. We created our first handler");
            response.Write("</h1></body></html>");
        }
        #endregion
    }
}