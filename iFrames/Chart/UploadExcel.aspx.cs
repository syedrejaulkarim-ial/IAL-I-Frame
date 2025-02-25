using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using inout = System.IO;
namespace iFrames.Chart
{
    public partial class UploadExcel : System.Web.UI.Page
    {
        #region: Global Variable
        string filepath = string.Empty;
        string filename = string.Empty;
        string fileext = string.Empty;
        Chart objChart = new Chart();
        #endregion

        #region: Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            lblMsgError.Text = string.Empty;

            if (Session["userid"] != null && Session["password"] != null)
            {
                pnlLogin.Visible = false;
                pnlUpload.Visible = true;
                lnlLogout.Visible = true;
            }
            else
            {
                pnlLogin.Visible = true;
                pnlUpload.Visible = false;
                lnlLogout.Visible = false;
            }


        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
        #endregion

        #region: Button Event
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            UploadExcell();
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Login())
            {
                pnlLogin.Visible = false;
                pnlUpload.Visible = true;
            }
            else
            {
                pnlLogin.Visible = true;
                pnlUpload.Visible = false;
            }
        }
        #endregion

        #region: Link Button Event
        protected void lnlLogout_Click(object sender, EventArgs e)
        {
            pnlLogin.Visible = true;
            pnlUpload.Visible = false;
            lnlLogout.Visible = false;
            SessionClear();
            Clear();
        }
        #endregion

        #region: Method
        private void UploadExcell()
        {
            try
            {
                if (FUExcell.HasFile == true)
                {
                    string todate = System.DateTime.Now.ToString("MMddyyyyhhmmss");
                    filepath = Server.MapPath("~//Chart//Excels//") + FUExcell.FileName;
                    fileext = inout.Path.GetExtension(filepath);
                    filename = inout.Path.GetFileNameWithoutExtension(filepath);
                    filepath = Server.MapPath("~//Chart//Excels//") + filename + "_" + todate + fileext;

                    if (inout.Directory.Exists(Server.MapPath("~//Chart//Excels")))
                    {
                        string[] filePaths = inout.Directory.GetFiles(Server.MapPath("~//Chart//Excels"));
                        foreach (string filePath in filePaths)
                            inout.File.Delete(filePath);
                    }
                    FUExcell.SaveAs(filepath);
                    DataTable dt1 = objChart.FillGridFromExcell(filepath);
                    DataView dv = dt1.DefaultView;
                    dv.Sort = "CompanyName, FY_End ASC";
                    DataTable dt = dv.ToTable();
                    //objChart.InsertDataFromExcell(dt);
                    objChart.InsertDataFromExcellToDb(dt);
                    lblMsg.Text = "FileUploaded Succesfully.";
                    
                }
                else
                {
                    lblMsg.Text = "Please choose a company excel file.";
                }
            }
            catch (Exception ex)
            {


            }



        }
        private bool Login()
        {
            bool success = false;
            string userid = string.Empty;
            string password = string.Empty;
            userid = "admin";
            password = "nimda";
            try
            {
                if (txtUserid.Text == "admin" && txtPassword.Text == "nimda")
                {
                    success = true;
                    lblMsgError.Text = string.Empty;
                    Session["userid"] = txtUserid.Text;
                    Session["password"] = txtPassword.Text;
                    lnlLogout.Visible = true;
                }
                else
                {
                    lblMsgError.Text = "Please Enter Correct Userid or Password.";
                    lnlLogout.Visible = false;
                    Clear();
                }


            }
            catch (Exception ex)
            {


            }
            return success;

        }
        private void Clear()
        {
            txtUserid.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
        private void SessionClear()
        {
            Session["userid"] = null;
            Session["password"] = null;
            Session.Abandon();
        }

        #endregion

    }
}