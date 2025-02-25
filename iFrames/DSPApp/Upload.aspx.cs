using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using iFrames.BLL;



namespace iFrames.DSPApp
{
    public partial class Upload : System.Web.UI.Page
    {
        DateTime rptdate;
        //string flag = "N";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");


             if (!IsPostBack)
            {
                EventDate.Value = DateTime.Today.ToString("dd MMM yyyy");

                if (Convert.ToBoolean(Session["IsAdmin"]) == false)
                {
                    liUserMngmnt.Style.Add("display", "none");
                    liUploadExl.Style.Add("display", "none");
                    dvContent.Style.Add("display", "none");
                }

             }

             btnUpload.Attributes.Add("onclick", "javascript:return validate();");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            try
            {
                
                string ConStr = "";
                // string prevcolumnname = "";
                string query = "";
                string tablename = "";
                string LoggedUserid =  Session["EmailId"].ToString();
                //string LoggedUserid = "arvind";
                string rptcolumn = "";
               // string pathNew = "";

               // iFrames.BLL.DSPAppUpload.WriteLog("Start");

                string ext = Path.GetExtension(FileUpload1.FileName);

              
                string path = Server.MapPath("~/DSPApp/FileFolder/" + FileUpload1.FileName);

             
                FileUpload1.SaveAs(path);
             

               ///rptdate = iFrames.BLL.DSPAppUpload.Formatfile(path);
              

                string date = iFrames.BLL.DSPAppUpload.ExtractDatesFromFileNames(FileUpload1.FileName);                
                //var date1 = DateTime.ParseExact(date, "yyyyMMdd", null);


                rptdate = Convert.ToDateTime(date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

              
     
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
               // iFrames.BLL.DSPAppUpload.WriteLog("con stringn -->" + ConStr);

                OleDbConnection conn = new OleDbConnection(ConStr);

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

              //  iFrames.BLL.DSPAppUpload.WriteLog("connection open");

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

                    if (SheetName == "Summary" || SheetName == "Summary Direct")
                    {
                     
                        //iFrames.BLL.DSPAppUpload.WriteLog("summary");

                        SheetName = SheetName.Replace("$", "");
                        tablename = "T_DSP_RETURN_ANALYSIS_SUMMARY";
                        //iFrames.BLL.DSPAppUpload.WriteLog("summary column");
                        DataTable Excelcol = iFrames.BLL.DSPAppUpload.ColumnExist(tablename);
                        //iFrames.BLL.DSPAppUpload.WriteLog("summary delete");
                        iFrames.BLL.DSPAppUpload.DelExistRecords(tablename, Convert.ToDateTime(rptdate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), SheetName);
                        //iFrames.BLL.DSPAppUpload.WriteLog("summary upload");
                        rptcolumn = iFrames.BLL.DSPAppUpload.BulkUpload(SheetName, tablename, Excelcol, Exceldt);
                        //iFrames.BLL.DSPAppUpload.WriteLog("summary update");
                        iFrames.BLL.DSPAppUpload.UpdateToEventLogStatus(SheetName, FileUpload1.FileName, Convert.ToDateTime(rptdate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), Convert.ToDateTime(EventDate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), LoggedUserid, tablename, rptcolumn);
                        //iFrames.BLL.DSPAppUpload.WriteLog("summary end");
                    }
                    else
                    {
                        if (SheetName != "Relative Performance")
                        {
                            tablename = "T_DSP_RETURN_ANALYSIS_DETAIL";
                            //iFrames.BLL.DSPAppUpload.WriteLog("detail column");
                            DataTable Excelcol = iFrames.BLL.DSPAppUpload.ColumnExist(tablename);
                            //iFrames.BLL.DSPAppUpload.WriteLog("detail delete");
                            iFrames.BLL.DSPAppUpload.DelExistRecords(tablename, Convert.ToDateTime(rptdate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), SheetName);
                            //iFrames.BLL.DSPAppUpload.WriteLog("detail upload");
                            rptcolumn = iFrames.BLL.DSPAppUpload.BulkUpload(SheetName, tablename, Excelcol, Exceldt);
                    
                            iFrames.BLL.DSPAppUpload.UpdateToEventLogStatus(SheetName, FileUpload1.FileName, Convert.ToDateTime(rptdate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), Convert.ToDateTime(EventDate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), LoggedUserid, tablename, rptcolumn);
                            //iFrames.BLL.DSPAppUpload.WriteLog("detail end");
                        }

                    }

                    i++;

                }

               
                conn.Close();
                //iFrames.BLL.DSPAppUpload.WriteLog("file delete");
                File.Delete(path);
           
                //iFrames.BLL.DSPAppUpload.WriteLog("done");

                Response.Write("<script LANGUAGE='JavaScript' >alert('Record(s) uploaded successfully.')</script>");
            }

            catch (Exception ex)

            {

                iFrames.BLL.DSPAppUpload.WriteLog(ex.ToString());

                if (ex.Message.Contains("was not recognized as a valid DateTime.") )
                {
                    Response.Write("<script LANGUAGE='JavaScript' >alert('File Name should be Filename_DD-MM-YYYY.xls/xlsx')</script>");
                }
                else if (ex.Message.Contains("Index and length must refer to a location within the string")) 
                {
                Response.Write("<script LANGUAGE='JavaScript' >alert('File Name should be Filename_DD-MM-YYYY.xls/xlsx')</script>");
                }                   
                else
                {
                    Response.Write("<script LANGUAGE='JavaScript' >alert('File not in proper format .')</script>");
                }
                               
            }

        }

       
        protected void btnReset_Click(object sender, EventArgs e)
        {
            EventDate.Value = DateTime.Today.ToString("dd MMM yyyy");
        }

    }
}