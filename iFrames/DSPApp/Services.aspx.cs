using iFrames.BLL;
using iFrames.DSPApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.HDFC_SIP
{
    public partial class Services : System.Web.UI.Page
    {
        //public IWebSockets WebSockets { get; set; }

        private static Dictionary<string, AuthDetails> _SocketsList = new Dictionary<string, AuthDetails>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //var JSONData = Request.Form.GetValues(1);
            if (Request.Form.Count != 0)
            {
                Response.Clear();
                Response.Write(GetToken(Request.Form[0]));
                Response.Flush();
                //Response.End();
                Response.Close();
            }

            StreamReader stream = new StreamReader(Request.InputStream);
            string x = stream.ReadToEnd();
            string xml = HttpUtility.UrlDecode(x);

            if(!string.IsNullOrEmpty(xml))
            {
                var val1 = xml.Replace("Data=", "");
                Response.Write(GetToken(val1));
                Response.Flush();
                //Response.End();
                Response.Close();
            }
            //string a = GetToken("abc@dsp.com", "7BB5D3BE-DFFE-4753-B662-3235A5AFF80E");
            // ExternalLogin(a, "admin@dspim.com");

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://mfiframes.mutualfundsindia.com/DSPApp/Services.aspx/GetToken");
            //request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            //request.Accept = " application/json, text/javascript, */*; q=0.01";
            //request.ContentType = " application/json; charset=utf-8";
            //request.Method = "POST";
            //request.ContentLength = 0; 

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //if (response.StatusCode.ToString().ToLower() == "ok")
            //{
            //    string contentType = response.ContentType;

            //    Stream content = response.GetResponseStream();

            //    StreamReader contentReader = new StreamReader(content);

            //    //Response.ContentType = contentType;
            //    var strData = contentReader.ReadToEnd();
            //    //Response.Write(contentReader.ReadToEnd());

            //}
        }

        [WebMethod]
        //public static string GetToken(string UserId, string Password, string Date)
        public static string GetToken(String Data)
        {
            //var Data = HttpContext.Current.Request.Form["Data"];
            string json = Base64Decode(Data);
            var ObjApiInputs = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiInputs>(json);
        
            if (_SocketsList.Count > 0 && _SocketsList.Any())
            {
                foreach (var sd in _SocketsList)
                {
                    if (sd.Value.RequestEnds <= DateTime.Now.AddHours(-1))
                    {
                        _SocketsList.Remove(sd.Key);                       
                    }
                    if (_SocketsList.Count == 0)
                        break;
                }
            }
            BLL.DSPApp app = new BLL.DSPApp();
            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            ApiExternalLoginEntity ApiExternalLogin = new ApiExternalLoginEntity();
            ApiExternalLogin.Password = ASCIIEncoding.Default.GetBytes(ObjApiInputs.Password);
            ApiExternalLogin.UserId = ObjApiInputs.UserId;

            var LoginInfo = app.GenerateExternalLoginToken(ApiExternalLogin);

            if (LoginInfo != null && !String.IsNullOrEmpty(LoginInfo.UserId))
            {
                var Auth = new AuthDetails
                {
                    EmailId = LoginInfo.UserId,
                    AuthenticationType = LoginInfo.AuthenticationTag
                };
                var AuthTokenKey = Auth.GetUniqueId();
                Auth.RequestStarts = DateTime.Now;
                Auth.RequestEnds = DateTime.Now.AddMinutes(ApiExternalLoginEntity.APITokenStamp);
                var AuthTokenValue = Auth.GetUniqueId();
                var UserDetails = (AuthDetails)AuthTokenValue;

                if (!_SocketsList.Where(x => x.Key == AuthTokenKey).Any())
                {                   
                    _SocketsList.Add(AuthTokenKey, UserDetails);
                }

                AuthToken _AuthToken = new AuthToken() { Token = AuthTokenKey };

                string AuthToken= Newtonsoft.Json.JsonConvert.SerializeObject(_AuthToken);
                return AuthToken;
            }
            else
            {
                return "Warning!!Invalid authentication.";
            }
        }

        [WebMethod]
        public static DSPLogin ExternalLogin(string TokenKey, string Email)
        {
            DSPApp.Login logIn = new DSPApp.Login();
            try
            {
                var socketData = _SocketsList.Where(x => x.Key == TokenKey).FirstOrDefault();
                if (socketData.Value != null && socketData.Key.Any())
                {
                    ApiExternalLoginEntity ApiExternalLogin = new ApiExternalLoginEntity();
                    var userData = socketData.Value;
                    if ((DateTime.Now <= userData.RequestEnds && DateTime.Now >= userData.RequestEnds.AddMinutes(-ApiExternalLoginEntity.APITokenStamp)))
                    {
                        ApiExternalLogin.UserId = Email;
                        BLL.DSPApp app = new BLL.DSPApp();
                        var loginUser = app.GetExternalLoginUser(ApiExternalLogin);
                        if (loginUser != null)
                        {
                            return loginUser;
                        }
                        else
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return null;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }

    public class ApiInputs
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Date { get; set; }
    }

}