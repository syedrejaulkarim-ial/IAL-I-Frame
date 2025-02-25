using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace iFrames.DSPApp
{
    public partial class CreateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");

            if (!Page.IsPostBack)
            {
                txtEmailId.Value = "";
                txtPassword.Value = "";
                txtRetypePassword.Value = "";
                if (Convert.ToBoolean(Session["IsAdmin"]) == false)
                {
                    liUserMngmnt.Style.Add("display", "none");
                    liUploadExl.Style.Add("display", "none");
                    dvContent.Style.Add("display", "none");
                }
                else
                {
                    getUserBranch();
                  
                }
            }

            Button1.Attributes.Add("onclick", "javascript:return validate();");
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
                iFrames.BLL.DSPAppUpload.WriteLog("CreateUser_getUserBranch:" + ex.ToString());
            }
        }

        [System.Web.Services.WebMethod]
        public static bool checkUserExist(string EmailId)
        {
            bool retval = false;
            try
            {
                BLL.DSPApp da = new BLL.DSPApp();
                var _user = da.getUserDetails(EmailId);
                if (_user.Count > 0)
                    retval = true; ;
            }
            catch (Exception ex)
            {
                iFrames.BLL.DSPAppUpload.WriteLog("CreateUser_checkUserExist:" + ex.ToString());
            }
            return retval;
        }

        [System.Web.Services.WebMethod]
        public static string createUser(string EmailId, string Password,string BranchName)
        {
            string retVal = "";
            try
            {
                if (HttpContext.Current.Session["UserId"] == null)
                    retVal = "1";
                else
                {
                    int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    BLL.DSPApp da = new BLL.DSPApp();
                    if (da.createUser(EmailId, Password, userId, BranchName))
                        retVal = "2";
                }
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
                iFrames.BLL.DSPAppUpload.WriteLog("CreateUser_createUser:" + ex.ToString());
            }
            return retVal;
        }

        protected void btnExcelUpload_Click(object sender, EventArgs e)
        {
            CreatUser.Visible = false;
            Upload.Visible = true;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string ConStr = "";
                string query = "";            
                string LoggedUserid = Session["EmailId"].ToString();         
                string ext = Path.GetExtension(FileUpload1.FileName);
                string path = Server.MapPath("~/DSPApp/FileFolder/" + FileUpload1.FileName);

                FileUpload1.SaveAs(path);
            
                if (ext.Trim() == ".xls")
                {
                    //connection string for that file which extantion is .xls  
                    // ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    //  ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;  Data Source=" + pathNew + ";Extended Properties=Excel 8.0;HDR=Yes;IMEX=2\"";

                    ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                }

                else if (ext.Trim() == ".xlsx")
                {
                    //connection string for that file which extantion is .xlsx  
                    // ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathNew + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + "Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";

                }       

                OleDbConnection conn = new OleDbConnection(ConStr);

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                       
                System.Data.DataTable dt = null;

                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {

                }

                int i = 0;

                foreach (DataRow row in dt.Rows)
                {

                    string SheetName;
                    SheetName = row["TABLE_NAME"].ToString();
                    SheetName = SheetName.Replace("'", "");

                   

                    query = "SELECT * FROM [" + SheetName + "] ";



                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        DataTable Exceldt = ds.Tables[0];

                        SheetName = SheetName.Replace("$", "");

                   if (SheetName == "Login")
                   {

                        int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                        string defaltPwd = "Ds@12345";

                        for (int j = 0; j < Exceldt.Rows.Count; j++)
                        {

                            if (Exceldt.Rows[j][0].ToString() != null && Exceldt.Rows[j][1].ToString() != null)
                            {

                                if (Exceldt.Rows[j][0].ToString().ToLower().Contains("@dspim.com") == true)
                                {
                                    System.Data.DataTable dtLoginId = iFrames.BLL.DSPApp.GetExistLoginId(Exceldt.Rows[j][0].ToString());
                                    if (dtLoginId.Rows.Count > 0)
                                    {

                                        iFrames.BLL.DSPApp.UpdateLoginDetail(Exceldt.Rows[j][0].ToString(), Exceldt.Rows[j][1].ToString(), userId);

                                    }
                                    else
                                    {
                                        BLL.DSPApp dal = new BLL.DSPApp();
                                        dal.createUser(Exceldt.Rows[j][0].ToString(), defaltPwd, userId, Exceldt.Rows[j][1].ToString());
                                    }

                                }

                            }
                        }

                    }
                        i++;    

                }


                conn.Close();
        
                File.Delete(path);

            

                Response.Write("<script LANGUAGE='JavaScript' >alert('Record(s) uploaded successfully.')</script>");
           }

            catch (Exception ex)
            {

                iFrames.BLL.DSPAppUpload.WriteLog(ex.ToString());
               
                Response.Write("<script LANGUAGE='JavaScript' >alert('File not in proper format .')</script>");
             

            }
        }
        

        protected void btnReset_Click(object sender, EventArgs e)
        {
            CreatUser.Visible = true;
            Upload.Visible = false;
        }
    }
}