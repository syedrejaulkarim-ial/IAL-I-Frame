using System;
using System.Text.RegularExpressions;
using System.Web;

namespace iFrames
{
    public class HttpModuleIFrame : IHttpModule
	{
		public HttpModuleIFrame()
		{

		}
		/// <summary>
		/// You will need to configure this module in the web.config file of your
		/// web and register it with IIS before being able to use it. For more information
		/// see the following link: http://go.microsoft.com/?linkid=8101007
		/// </summary>
		#region IHttpModule Members

		public void Dispose()
		{
			//clean-up code here.
		}

		public void Init(HttpApplication context)
		{
            context.BeginRequest += new EventHandler(context_BeginRequest);

            context.EndRequest += new EventHandler(context_EndRequest);
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);

        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            var Context = ((HttpApplication)sender).Context;
            try
            {
                //assume ThreadAbortException occurs here


                var CurURL = Context.Request.Url.LocalPath.ToString().ToUpper();
                HttpRequest request = ((HttpApplication)sender).Request;
                //var ListOfUrls = ("TOPFUND.ASPX~COMPAREFUND.ASPX").Split('~');
                var ListOfUrls = ("TOPFUNDtest.ASPX~COMPAREFUNDtest.ASPX").Split('~');

                var ChkReqChkAntiXXS = true;
                foreach (var Link in ListOfUrls)
                {
                    if ((string.IsNullOrEmpty(Link)) || (Link == ""))
                        continue;
                    if (CurURL.Contains(Link.ToUpper()))
                    {
                        ChkReqChkAntiXXS = false;
                        break;
                    }
                }
                if (ChkReqChkAntiXXS)
                    if (request.HttpMethod == "POST")
                        if (request.Form.Count > 0)
                        {
                            if (CheckCollection(request.Form))
                            {
                                HttpResponse Response = HttpContext.Current.Response;
                                Context.Response.Redirect("/GenericErrorPage.htm", true);
                                Context.ApplicationInstance.CompleteRequest();
                                return;
                            }
                        }
                        else if (HttpContext.Current.Request.Headers["X-Requested-With"] != null
                            && HttpContext.Current.Request.Headers["X-Requested-With"].ToUpper() == "XMLHTTPREQUEST")
                        {
                            var bytes = new byte[request.InputStream.Length];
                            request.InputStream.Read(bytes, 0, bytes.Length);
                            request.InputStream.Position = 0;
                            string StrStream = System.Text.Encoding.ASCII.GetString(bytes);
                            if (ContainsHTML(StrStream))
                            {
                                Context.Response.StatusCode = 500;
                            }
                        }
            }
            catch (System.Exception Ex)
            {
                HttpResponse Response = HttpContext.Current.Response;
                Context.Response.Redirect("/GenericErrorPage.htm", true);
                Context.Response.StatusCode = 510;
                return;
            }
        }
        void context_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                var Context = ((HttpApplication)sender).Context;
                var URlRef = HttpContext.Current.Request.UrlReferrer;
                if (URlRef != null)
                    if (!(URlRef.Scheme.ToUpper() == "HTTP" || URlRef.Scheme.ToUpper() == "HTTPS"))
                        Context.Response.Redirect("/GenericErrorPage.htm", true);

            }
            catch (Exception ex)
            {

            }
        }

        void context_EndRequest(object sender, EventArgs e)
		{
			try
			{
				var CurRespns = HttpContext.Current.Response;
				if (!string.IsNullOrEmpty(CurRespns.Headers["Server"]))
					CurRespns.Headers.Remove("Server");
				if (!string.IsNullOrEmpty(CurRespns.Headers["X-AspNet-Version"]))
					CurRespns.Headers.Remove("X-AspNet-Version");

                //CurRespns.Headers.Remove("Access-Control-Allow-Origin");
                //CurRespns.AppendHeader("Access-Control-Allow-Origin", "*");
                CurRespns.Headers.Add("Access-Control-Allow-Origin", "*");


            }
            catch (Exception ex)
			{
				// Do your resolution
			}
		}

		#endregion

		public void OnLogRequest(Object source, EventArgs e)
		{
			//custom logging logic can go here
		}

        private static bool CheckCollection(System.Collections.Specialized.NameValueCollection collection)
        {
            // Both the form and query string collections are read-only by 
            // default, so use Reflection to make them writable:

            //System.Reflection.PropertyInfo readonlyProperty = collection.GetType()
            //    .GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            //  readonlyProperty.SetValue(collection, false, null);
            bool HasHtmlEncodeExist = false;
            for (int i = 0; i < collection.Count; i++)
            {
                var chkAND = "";
                if (collection[i].Contains("&") == true)
                {
                    chkAND = collection[i].Replace("&", "");
                }
                else
                {
                    chkAND = collection[i];
                }

                var chkSingleQuote = Regex.Replace(chkAND, @"\s", "");
                string pattern_1 = @"[^a-z0-9]'";
                string pattern_2 = @"'[^a-z0-9]";
                Match match1 = Regex.Match(chkSingleQuote, pattern_1);
                Match match2 = Regex.Match(chkSingleQuote, pattern_2);
                if (chkSingleQuote.Contains("'") && (!(match1.Success || match2.Success)))
                {
                    chkAND = collection[i].Replace("'", "");
                }
                //  var chkAND =  collection[i].Contains("&")==true?? collection[i].Replace("&",""):collection[i];

                string HtmlEncodedString = HttpUtility.HtmlEncode(chkAND);
                if (chkAND != HtmlEncodedString)
                {
                    HasHtmlEncodeExist = true;
                    break;
                    //collection[collection.Keys[i]] = HtmlEncodedString;
                }
            }
            return HasHtmlEncodeExist;
            // readonlyProperty.SetValue(collection, true, null);
        }

        private bool ContainsHTML(string CheckString)
        {
            return Regex.IsMatch(CheckString, "<(.|\n)*?>");
        }
    }
}

