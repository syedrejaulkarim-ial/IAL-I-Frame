using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

namespace iFrames.DSPApp
{
    public partial class LoginHistory : System.Web.UI.Page
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
                        gvwLoginHis.Style.Add("width", "1000");
                    else
                        gvwLoginHis.Style.Add("width", "100%");

                    if (Convert.ToBoolean(Session["IsAdmin"]) == false)
                    {
                        liUserMngmnt.Style.Add("display", "none");
                        liUploadExl.Style.Add("display", "none");
                        dvContent.Style.Add("display", "none");
                    }
                    else
                    {
                        getUserBranch();

                        txtFromDate.Value = DateTime.Now.AddMonths(-1).ToString("dd MMM yyyy");
                        txtToDate.Value = DateTime.Now.ToString("dd MMM yyyy");
                        getLoginHistory();
                    }
                }
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("LoginHistory_Page_Load:" + ex.ToString());
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        private void getUserBranch()
        {
            try
            {
                BLL.DSPApp da = new BLL.DSPApp();
                var _data = da.getUserBranch();

                if (_data.Rows.Count > 0)
                {
                    var _row = _data.NewRow();
                    _row[0] = "Select Branch";
                    _data.Rows.InsertAt(_row, 0);
                    ddlBranch.DataSource = _data;
                    ddlBranch.DataTextField = "BRANCH_NAME";
                    ddlBranch.DataValueField = "BRANCH_NAME";
                    ddlBranch.DataBind();

                    ddlBranch.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("LoginHistory_getUserBranch:" + ex.ToString());
            }
        }

        private void getLoginHistory()
        {
            try
            {
                if (ddlBranch.Items.Count == 0)
                    return;
                if (txtFromDate.Value.ToString().Trim() == "")
                    return;
                if (txtToDate.Value.ToString().Trim() == "")
                    return;

                string branch = "";
                if (ddlBranch.SelectedIndex > 0)
                    branch = ddlBranch.SelectedValue;

                BLL.DSPApp da = new BLL.DSPApp();
                var _data = da.getLoginHistory(branch, Convert.ToDateTime(txtFromDate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), Convert.ToDateTime(txtToDate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
                if (_data.Rows.Count > 0)
                {
                    gvwLoginHis.PageIndex = 0;
                    bindGrid(_data);
                    Cache["LoginHis"] = _data;
                }
                else
                {
                    bindGrid(null);
                    if (Cache.Count > 0)
                        Cache.Remove("LoginHis");
                }
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("LoginHistory_getLoginHistory:" + ex.ToString());
            }
        }

        private void bindGrid(object data)
        {
            gvwLoginHis.DataSource = data;
            gvwLoginHis.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            getLoginHistory();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (ddlBranch.Items.Count > 0)
            {
                ddlBranch.SelectedIndex = 0;
                txtFromDate.Value = DateTime.Now.AddMonths(-1).ToString("dd MMM yyyy");
                txtToDate.Value = DateTime.Now.ToString("dd MMM yyyy");
                getLoginHistory();
            }
        }

        protected void gvwLoginHis_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //var _data = new System.Data.DataTable();
                gvwLoginHis.PageIndex = e.NewPageIndex;
                if (Cache["LoginHis"] != null)
                {
                    var _data = Cache["LoginHis"];
                    bindGrid(_data);
                }
                else
                {
                    getLoginHistory();
                }
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("LoginHistory_gvwLoginHis_PageIndexChanging:" + ex.ToString());
            }
        }

        private void ExportGridToExcel()
        {
            try
            {
                if (Cache["LoginHis"] == null)
                    getLoginHistory();

                var dt = Cache["LoginHis"];
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                string FileName = "Login History " + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss tt") + ".xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);

                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.Write("<style type='text/css'>.FixedHeader {position: absolute;font-weight: bold;}</style>");
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                gv.AllowPaging = false;                
                gv.GridLines = GridLines.Both;
                gv.HeaderStyle.Font.Bold = true;
                //gv.HeaderStyle.CssClass = "FixedHeader";
                //gv.HeaderStyle.BackColor = Color.DarkGray;
                gv.RenderControl(htmltextwrtter);
                Response.Write(strwritter.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("LoginHistory_gvwLoginHis_PageIndexChanging:" + ex.ToString());
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }
    }
}
