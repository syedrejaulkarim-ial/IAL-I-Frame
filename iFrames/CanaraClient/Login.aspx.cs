using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;

namespace iFrames.CanaraClient
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                username.Value = "";
                password.Value = "";
                username.Focus();
                if (!string.IsNullOrEmpty(Request.QueryString["param"]))
                {
                    //BLL.DSPApp da = new BLL.DSPApp();
                    //da.updateLoginStatus(Session["EmailId"].ToString());
                    Session.Clear();
                    Session.RemoveAll();
                    Session.Abandon();
                }
            }
        }
        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if(username.Value.ToString()!="" && password.Value.ToString() != "")
            {
                checkValidUser(username.Value, password.Value);
            }
            else
            {
                showMsg("INVALID", "Please Provide Username and Password");
            }
            
        }
        public void checkValidUser(string EmailId, string Password)
        {
            //string retVal = "#";
            try
            {
                int WrongCount = 3;//Convert.ToInt32(ConfigurationManager.AppSettings["DSPWrongPwdAttmeptCount"]);
                BLL.CanaraClient_BLL da = new BLL.CanaraClient_BLL();
                var _data = da.getUserDetails(EmailId);
                if (_data.Count() > 0)
                {
                    var _user = _data.FirstOrDefault();
                    if (_user.IsActive == false)
                    {
                        showMsg("INACTIVE", "Your account is Inactive.");
                        //return false;
                        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "INACTIVE", "alert('Your account is Inactive.');", true);
                        //return "INACTIVE#";
                    }
                    else if (_user.IsLockedOut == true)
                    {
                        DateTime? lastLockedDate = _user.Last_Locked_Out_Date;
                        double diffMin = DateTime.Now.Subtract(lastLockedDate.Value).TotalMinutes;
                        if (diffMin >= 5)
                            da.lockUser(EmailId, false);
                        else
                        {
                            showMsg("LOCKED", "Your account is blocked. Wait 5 minutes to unlock.");
                            //return false;
                            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "LOCKED", "alert('Your account is blocked. Wait 5 minutes to unlock.');", true);
                            //return "LOCKED#";
                        }
                    }
                    else
                    {
                        string encPassword = Utilities.DESEnCode(Password);
                        var _validUser = _data.Where(x => x.Password == encPassword);
                        if (_validUser.Count() > 0)
                        {
                            HttpContext.Current.Session["UserId"] = _user.User_Id.ToString();
                            HttpContext.Current.Session["EmailId"] = _user.Email_Id;
                            HttpContext.Current.Session["IsAdmin"] = (_user.IsAdmin == null ? false : _user.IsAdmin);
                            if (da.updateLastLogin(EmailId, encPassword) == true)
                            {
                                //showMsg("Success", "Login Succesfully");
                                //if (Convert.ToBoolean(Session["IsAdmin"]) == true)
                                //{
                                //    //Response.Redirect("CreateUser.aspx", false);
                                //    //Response.End();

                                //}
                                //else
                                //{
                                //    Response.Redirect("Viewreport.aspx", false);
                                //    Response.End();
                                //}
                                Response.Redirect("ExcelFileUpload.aspx", false);
                                Response.End();
                            }
                            //return true;
                            //return "OK#";
                        }
                        else
                        {
                            int WrongCnt = da.updateWrongAttempCount(EmailId);
                            if (WrongCnt >= WrongCount)
                            {
                                da.lockUser(EmailId, true);
                                showMsg("LOCKED", "Your account is blocked. Wait 5 minutes to unlock.");
                                //return false;
                                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "LOCKED", "alert('Your account is blocked. Wait 5 minutes to unlock.');", true);
                                //return "LOCKED#";
                            }
                            else
                            {
                                showMsg("WRONGPWD", "Wrong Password. After " + WrongCount.ToString() + " wrong attempt your account will be locked.");
                                //return false;
                                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "WRONGPWD", "alert('Wrong Password.\nAfter " + WrongCount.ToString() + " wrong attempt your account will be locked.');", true);
                                //return "WRONGPWD#" + WrongCnt.ToString();
                            }
                        }
                    }
                }
                else
                {
                    showMsg("INVALID", "Invalid Username.");
                    //return false;
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "INVALID", "alert('Invalid Username.');", true);
                    //return "INVALID#";
                }
            }
            catch (Exception ex)
            {
                showMsg("ERR", ex.Message);
                iFrames.BLL.DSPAppUpload.WriteLog("Login_checkValidUser:" + ex.ToString());
                //return false;
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "ERR", "alert('" + ex.Message +"');", true);
                //return "ERR#" + ex.Message;
            }
            //return retVal;
        }

        private void showMsg(string key, string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), key, "alert('" + msg + "');", true);
        }
    }
}