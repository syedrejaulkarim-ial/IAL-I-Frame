using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.CanaraClient
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["resetToken"]))
                {
                    BLL.CanaraClient_BLL da = new BLL.CanaraClient_BLL();
                    string EmailId = Utilities.DESDeCode(Request.QueryString["resetToken"]);
                    var _data = da.getUserDetails(EmailId);
                    if (_data.Count > 0)
                    {
                        da.resetPassword(EmailId, txtNewPass.Value);
                        //showMsg("OK", "Password reset successfully.");
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "OK", "alert('Password reset successfully.'); window.location.href='Login.aspx';", true);
                    }
                    else
                    {
                        showMsg("TAMPURL", "URL tampered.");
                    }
                }
                else
                {
                    showMsg("INVALIDURL", "Invalid URL.");
                }
            }
            catch (Exception ex)
            {
                showMsg("ERR", ex.Message);
                iFrames.BLL.DSPAppUpload.WriteLog("ResetPassword_BtnSubmit_Click:" + ex.ToString());
            }
        }

        private void showMsg(string key, string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), key, "alert('" + msg + "');", true);
        }
    }
}