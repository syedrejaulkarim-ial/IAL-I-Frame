using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.Pages
{
    public partial class MyBasePage : System.Web.UI.Page
    {
        DataSet ds= new DataSet();
        DataTable dt =new DataTable();
        public string PropCompID
        {
            set; 
            get; 
        }
        public string PropMutCode
        {
            set;
            get;
        }
        public string Themes
        {
            set;
            get;
        }

        public MyBasePage()

       { 

       }       

        protected void Page_Load(object sender, EventArgs e)
        {         
           

        }

        protected override void OnPreInit(EventArgs e)
        { 
            PropCompID = Request.QueryString["comID"];
            if (PropCompID == null)
            {
                if (Request.UrlReferrer != null)
                {
                    string qry = Request.UrlReferrer.Query;
                    string[] qry1 = qry.TrimStart('?').Split('&');
                    foreach (string str in qry1)
                    {
                        if (str.ToLower().StartsWith("comid"))
                        {
                            int i = str.IndexOf('=');
                            PropCompID = str.Substring(i + 1);
                        }
                    }
                }
            }
            DataRow getRow = getThemes(PropCompID == null || PropCompID == "" ? "1" : PropCompID);
            Themes = getRow["theme"].ToString();
            PropMutCode = getRow["mutid"].ToString();            
            this.Theme = Themes;            
        }

        

        public DataRow getThemes(string ComID)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("../App_Data/data.xml"));
            ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["id"] };
            return ds.Tables[0].Rows.Find(ComID);
        }

    }
}