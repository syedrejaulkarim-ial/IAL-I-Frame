using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;

namespace iFrames.DSPApp
{
    public partial class Active : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {  
                if (Session["UserId"] == null)
                    Response.Redirect("Login.aspx");

                if (!Page.IsPostBack)
                {
                    if (HttpContext.Current.Request.Browser.IsMobileDevice)
                        gvwUser.Style.Add("width", "1000");
                    else
                        gvwUser.Style.Add("width", "100%");                   

                    if (Convert.ToBoolean(Session["IsAdmin"]) == false)
                    {
                        liUserMngmnt.Style.Add("display", "none");
                        liUploadExl.Style.Add("display", "none");
                        dvContent.Style.Add("display", "none");
                    }
                    else
                        getUser();
                }
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("Active_Page_Load:" + ex.ToString());
            }
        }

        private void getUser()
        {
            try
            {
                BLL.DSPApp da = new BLL.DSPApp();
                var _data = da.getUserDetails();

                if (_data.Count > 0)
                {
                    _data.Insert(0, new DSPLogin());
                    ddlUser.DataSource = _data;
                    ddlUser.DataTextField = "Email_Id";
                    ddlUser.DataValueField = "User_Id";
                    ddlUser.DataBind();

                    ddlUser.SelectedIndex = -1;

                    _data.RemoveAt(0);
                    if (Cache["Userdet"] != null)
                        Cache.Remove("Userdet");
                    Cache.Insert("Userdet", _data);

                    //rptUser.DataSource = _data;
                    //rptUser.DataBind();
                    bindGrid(_data);
                }
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("Active_getUser:" + ex.ToString());
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlUser.SelectedIndex > 0)
                {
                    if (Cache["Userdet"] != null)
                    {
                        var _data = (List<DSPLogin>)Cache["Userdet"];
                        var _filterdata = _data.Where(x => x.User_Id == Convert.ToInt32(ddlUser.SelectedValue) && x.User_Id != 0).ToList<DSPLogin>();

                        bindGrid(_filterdata);
                    }
                    else
                    {
                        BLL.DSPApp da = new BLL.DSPApp();
                        var _data = da.getUserDetails(ddlUser.SelectedItem.Text);

                        bindGrid(_data);
                    }
                }               
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("Active_btnSearch_Click:" + ex.ToString());
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                //if (ddlUser.SelectedIndex > 0)
                //{
                ddlUser.SelectedIndex = -1;
                if (Cache["Userdet"] != null)
                {
                    var _data = (List<DSPLogin>)Cache["Userdet"];
                    bindGrid(_data);
                }
                else
                {
                    BLL.DSPApp da = new BLL.DSPApp();
                    var _data = da.getUserDetails();

                    bindGrid(_data);
                }
                //}
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("Active_btnReset_Click:" + ex.ToString());
            }
        }

        [System.Web.Services.WebMethod]
        public static bool UpdateUserStatus(int UserId, bool Status)
        {
            bool retVal = false;
            try
            {
                BLL.DSPApp da = new BLL.DSPApp();
                retVal = da.updateUserStatus(UserId, Status);
                
                var _data = (List<DSPLogin>)HttpContext.Current.Cache["Userdet"];
                _data.Where(x => x.User_Id == UserId).FirstOrDefault().IsActive = Status;
                HttpContext.Current.Cache["Userdet"] = _data;
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("Active_UpdateUserStatus:" + ex.ToString());
            }
            return retVal;
        }

        protected void gvwUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var _data = new List<DSPLogin>();

                if (Cache["Userdet"] != null)
                {
                    _data = (List<DSPLogin>)Cache["Userdet"];
                }
                else
                {
                    BLL.DSPApp da = new BLL.DSPApp();
                    _data = da.getUserDetails();                  
                }
                if (ddlUser.SelectedIndex > 0)
                {
                    _data = _data.Where(x => x.User_Id == Convert.ToInt32(ddlUser.SelectedValue)).ToList<DSPLogin>();
                }
                gvwUser.PageIndex = e.NewPageIndex;
                bindGrid(_data);
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("Active_gvwUser_PageIndexChanging:" + ex.ToString());
            }
        }

        private void bindGrid(object data)
        {
            gvwUser.DataSource = data;
            gvwUser.DataBind();
        }

        protected void gvwUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hdnUserId = (HiddenField)e.Row.FindControl("hdnUserId");
                    CheckBox chkStatus = (CheckBox)e.Row.FindControl("chkStatus");
                    chkStatus.Attributes["onclick"] = "return UpdateAction(this," + hdnUserId.Value + ")";
                    System.Web.UI.HtmlControls.HtmlGenericControl lblStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("lblStatus");
                    lblStatus.Attributes["for"] = chkStatus.ClientID;
                }
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("Active_gvwUser_RowDataBound:" + ex.ToString());
            }
        }
    }
}