using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.Classes;

namespace iFrames.Canara
{
    public partial class ForgetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string ResetPassword(string EmailId)
        {
            string retVal = "";
            try
            {
                BLL.DSPApp da = new BLL.DSPApp();
                var _data = da.getUserDetails(EmailId);
                if (_data.Count > 0)
                {
                    var _user = _data.FirstOrDefault();
                    if (_user.IsActive == false)
                    {
                        showMsg("INACTIVE", "Your account is Inactive.");
                    }
                    else
                    {
                        MailAddress toAddress = new MailAddress(EmailId);
                        string url = HttpContext.Current.Request.Url.AbsoluteUri.Replace("ForgetPassword", "ResetPassword");
                        string href = HttpUtility.HtmlEncode(url + "?resetToken=" + Utilities.DESEnCode(txtEmail.Value));

                        string mailbody = @"<span style='font-family:verdana; font-size:12px;'>Dear User,<br/><br/>This is response to your password-reset request.
                               Click the following link to reset your password.</span>";
                        string body = string.Format(mailbody + "<br /> <a href='{0}'>Reset your password</a>" + "<br/><br/>Thanks,<br/>ICRON HelpDesk", href);

                        MessageService MS = new MessageService();
                        if (MS.SendEmail(toAddress, body))
                        {
                            showMsg("OK", "Your password reset request has been accepted, please check your mail for more details.");
                            txtEmail.Value = "";
                            //retVal = "OK";
                        }
                    }
                }
                else
                {
                    showMsg("INVALIDEMAIL", "Invalid Email Id.");
                    txtEmail.Focus();
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "INVALIDEMAIL", "alert('Invalid Email Id.');", true);
                    //retVal = "INVALIDEMAIL";
                }
            }
            catch (Exception ex)
            {
                showMsg("ERR", ex.Message);
                //retVal = ex.Message;
            }
            return retVal;
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            ResetPassword(txtEmail.Value);
        }

        private void showMsg(string key, string msg)
        {
            msg = msg.Replace("'", "");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), key, "alert('" + msg + "');", true);
        }
    }
}