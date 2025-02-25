using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.BansalCapital
{
    public partial class BansalProxy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string proxyURL = string.Empty;

            try
            {

                proxyURL = HttpUtility.UrlDecode(Request.QueryString["u"].ToString());

            }

            catch { }



            if (proxyURL != string.Empty)
            {
                //var strData = new WebClient().DownloadString(proxyURL);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(proxyURL);
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");

                //request.Headers.Add("UserId", "SSO@dspim.com");
                //request.Headers.Add("Password", "7BB5D3BE-DFFE-4753-B662-3235A5AFF80E");

                request.Accept = " application/json, text/javascript, */*; q=0.01";
                request.ContentType = " application/json; charset=utf-8";
                request.Method = "POST";
                request.ContentLength = 0;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();



                if (response.StatusCode.ToString().ToLower() == "ok")
                {
                    string contentType = response.ContentType;

                    Stream content = response.GetResponseStream();

                    StreamReader contentReader = new StreamReader(content);

                    //Response.ContentType = contentType;
                    var strData = contentReader.ReadToEnd();
                    //Response.Write(contentReader.ReadToEnd());

                }

            }

        }


        //[System.Web.Services.WebMethod]
        //public static string getSession()
        //{
        //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.askmefund.com/Method.aspx/isloggiegin");
        //    //request.Headers.Add("X-Requested-With", "XMLHttpRequest");
        //    //request.Accept = " application/json, text/javascript, */*; q=0.01";
        //    //request.ContentType = " application/json; charset=utf-8";
        //    //request.Method = "POST";
        //    //request.ContentLength = 0;
        //    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    //if (response.StatusCode.ToString().ToLower() == "ok")
        //    //{
        //    //    Stream content = response.GetResponseStream();
        //    //    StreamReader contentReader = new StreamReader(content);
        //    //    var strData = contentReader.ReadToEnd();
        //    //    return (strData);
        //    //}
        //    return ("1");
        //}


        [System.Web.Services.WebMethod]
        public static string Add2Watchlist(int schemeId, string user)
        {
            #region Do not delete
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:52348/WebMethod.aspx/AddToWatchlist");
            //request.Method = "POST";
            //request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            //request.Accept = " application/json, text/javascript, */*; q=0.01";
            //request.ContentType = " application/json; charset=utf-8";
            //string data = "schemeId=" + schemeId; //replace <value>
            //byte[] dataStream = System.Text.Encoding.Unicode.GetBytes(data);
            //request.ContentLength = dataStream.Length;
            //using (var reqstrm = request.GetRequestStream())
            //{
            //    reqstrm.Write(dataStream, 0, dataStream.Length);
            //}
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //if (response.StatusCode.ToString().ToLower() == "ok")
            //{
            //    Stream content = response.GetResponseStream();
            //    StreamReader contentReader = new StreamReader(content);
            //    var strData = contentReader.ReadToEnd();
            //    return (strData);
            //}
            #endregion

            return (Communicate("http://www.askmefund.com/Method.aspx/addwatchlist", "POST", "",
                "application/x-www-form-urlencoded", "{schemeId:" + schemeId + ",user:'" + user + "'}"));
        }


        [System.Web.Services.WebMethod]
        public static string RemoveFrmWatchlist(int schemeId, string user)
        {
            return (Communicate("http://www.askmefund.com/Method.aspx/removewatchlist", "POST", "",
                "application/x-www-form-urlencoded", "{schemeId:" + schemeId + ",user:'" + user + "'}"));
        }


        private static CookieContainer Cookie = new CookieContainer();

        public static string Communicate(string Uri, string Method, string Refrance, string ContentType, string Data)
        {
            try
            {
                HttpWebRequest web = (HttpWebRequest)WebRequest.Create(Uri);
                web.Method = Method;
                //web.Referer = Refrance;
                web.ContentLength = Data.Length;
                web.CookieContainer = Cookie;
                //web.KeepAlive = true;
                web.ContentType = ContentType;
                web.Headers.Add("X-Requested-With", "XMLHttpRequest");
                //web.Accept = "application/json, text/javascript, */*; q=0.01";
                web.ContentType = "application/json";
                if (Data != string.Empty)
                {
                    using (var Request = web.GetRequestStream())
                    {
                        Request.Write(
                            System.Text.ASCIIEncoding.Default.GetBytes(Data),
                            0,
                            (int)web.ContentLength);
                    }

                }
                var Response = web.GetResponse();
                var ResquestStream = Response.GetResponseStream();
                ICollection<byte> bt = new List<byte>();
                while (true)
                {
                    int i = ResquestStream.ReadByte();
                    if (i == -1)
                        break;
                    bt.Add((byte)i);
                }
                var output = System.Text.ASCIIEncoding.Default.GetString(bt.ToArray());
                return output;
            }
            catch (Exception ex)
            {
                string error = ex.InnerException.ToString();
                return error;
            }
        }
    }
}