using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames
{
    public partial class TestApi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

               HttpWebRequest LogOutReq = WebRequest.Create("http://mfiframes.mutualfundsindia.com/DSPApp/services.aspx/GetToken") as HttpWebRequest;
         
                var dtNow = DateTime.Now.ToString();

                LogOutReq.Method = "POST";
                string postData1 = @"{'UserId': 'SSO@dspim.com','Password': '7BB5D3BE-DFFE-4753-B662-3235A5AFF80E','Date':'" + dtNow + "'}";
                postData1 = @"{'Data': '" + Base64Encode(postData1) + "'}";

                LogOutReq.ContentType = "application/json;charset=utf-8";
                LogOutReq.ContentLength = postData1.Length;

                Stream dataStream1 = LogOutReq.GetRequestStream();
                UTF8Encoding encoder1 = new UTF8Encoding();
                byte[] bytes1 = encoder1.GetBytes(postData1);
                dataStream1.Write(bytes1, 0, bytes1.Length);

                HttpWebResponse res = (HttpWebResponse)LogOutReq.GetResponse();

               string Email = Request.QueryString["Email"].ToString(); // user email 
                var ResponseText1 = string.Empty;
                using (var strm = new StreamReader(res.GetResponseStream()))
                {
                    ResponseText1 = strm.ReadToEnd();
                    if (ResponseText1.Any())
                    {
                        var Token = ResponseText1.Split(':')[2].Replace("}", "").Split('"')[1].Split('\\')[0];
                        Response.Redirect("http://mfiframes.mutualfundsindia.com/DSPApp/Login.aspx?Email=" + Email + "&Token=" + Token);
                    }
                }
            }
            catch (Exception ex)
            {
               
                //throw;
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {

                //HttpWebRequest LogOutReq = WebRequest.Create("http://mfiframes.mutualfundsindia.com/DSPApp/services.aspx/GetToken") as HttpWebRequest;
                 HttpWebRequest LogOutReq = WebRequest.Create("http://localhost:20801/DSPApp/Services.aspx/GetToken") as HttpWebRequest;

                LogOutReq.Method = "POST";
                string postData1 = @"{""UserId"": ""SSO@dspim.com"",""Password"": ""7BB5D3BE-DFFE-4753-B662-3235A5AFF80E""}";

                LogOutReq.ContentType = "application/json;charset=utf-8";
                LogOutReq.ContentLength = postData1.Length;

                Stream dataStream1 = LogOutReq.GetRequestStream();
                UTF8Encoding encoder1 = new UTF8Encoding();
                byte[] bytes1 = encoder1.GetBytes(postData1);
                dataStream1.Write(bytes1, 0, bytes1.Length);

                HttpWebResponse res = (HttpWebResponse)LogOutReq.GetResponse();

                var ResponseText1 = string.Empty;
                using (var strm = new StreamReader(res.GetResponseStream()))
                {
                    ResponseText1 = strm.ReadToEnd();
                    if (ResponseText1.Any())
                    {
                        Response.Redirect("http://mfiframes.mutualfundsindia.com/DSPApp/Login.aspx?Email=SSO@dspim.com&Token=");
                    }
                    Response.Write("****");
                }

            }
            catch (Exception ex)
            {
                Response.Write("****");
                throw;
            }
        }
    }
}