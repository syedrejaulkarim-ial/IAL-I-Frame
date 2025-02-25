using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using iFrames.DAL;

namespace iFrames
{
    public partial class MF_News : System.Web.UI.Page
    {
       //static  string con= ConfigurationManager.ConnectionStrings["MFDB"].ConnectionString;
       //static  SqlConnection conn = new SqlConnection();

       // private SqlCommand objCmd;
       // private DataSet dsGlobal;
       // private DataTable dtGlobal;
       // private SqlDataAdapter objDataAdapter;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (conn.State == ConnectionState.Closed)
                //{
                //    conn.ConnectionString = con;
                //    conn.Open();
                //}
                //string strErr = "";
                //string sqlStrig = "select top 12 row_number() over (order by rm.Title) as rowid, rm.Title,rd.HtmlBody,rm.PublishedOn from T_Web_Report_Details rd join T_Web_Report_Master rm on rd.id=rm.Id where rm.ReportType=2 and rm.IsActive=1 order by rm.PublishedOn desc";
                //ExecuteQuery(sqlStrig, ref strErr);

                DataTable dt = LoadNews();
                news.DataSource = dt;
                news.DataBind();
                newsptr.DataSource = dt;
                newsptr.DataBind();
            }
            catch (Exception ex)
            { }
        }


        private DataTable LoadNews()
        {
            DataTable _dt = null;
            using (var dcWiseInv = new SIP_ClientDataContext())
            {

                var objNews = (from wiseNew in dcWiseInv.T_MFI_NEW_Clients
                                   //where wiseNew.DISPLAY_DATE > DateTime.Now.AddDays(-7)
                               orderby wiseNew.DISPLAY_DATE ascending
                               select new
                               {
                                   Title = wiseNew.NEWS_HEADLINE,
                                   HtmlBody = wiseNew.MATTER,
                                   PublishedOn = wiseNew.DISPLAY_DATE
                               }).OrderByDescending(XhtmlMobileDocType => XhtmlMobileDocType.PublishedOn).Take(12);
                
                _dt = objNews.ToDataTable();

                _dt.Columns.Add("rowid", typeof(int));
                int RAnk = 1;
                _dt.AsEnumerable().ToList().ForEach(x => x["rowid"] = RAnk++);
            }
            return _dt;
        }

        //public bool ExecuteQuery(string strSQL, ref string strErrMsg)
        //{
        //    try
        //    {
        //        objDataAdapter = new SqlDataAdapter();
        //        dsGlobal = new DataSet();

        //        objCmd = new SqlCommand(strSQL, conn);
        //        objCmd.CommandTimeout = 6000;
        //        objDataAdapter.SelectCommand = objCmd;
        //        objDataAdapter.Fill(dsGlobal);
        //        //	clsConnection.Disconnect();

        //        dtGlobal = dsGlobal.Tables[0];

        //        //dsGlobal.Dispose();
        //        objDataAdapter.Dispose();
        //        objCmd.Dispose();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        strErrMsg = ex.Message + ":" + ex.Source;
        //        return false;
        //    }
        //}

        //public DataTable GetDataTable
        //{
        //    get
        //    {
        //        if (dtGlobal == null)
        //            dtGlobal = new DataTable();

        //        return dtGlobal;
        //    }
        //}
    }
}