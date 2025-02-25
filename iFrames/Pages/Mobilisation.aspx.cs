using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;

namespace iFrames.Pages
{
    public partial class Mobilisation : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = IndustryUpdate.getAMFIdate();
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["datVal"] = DateTime.MinValue;
                    dr["datText"] = "Select";                    
                    dt.Rows.InsertAt(dr, 0);
                    ddlDate.DataSource = dt;
                    ddlDate.DataTextField = "datText";
                    ddlDate.DataValueField = "datVal";
                    ddlDate.DataBind();                    
                }
                //ddlDate.Attributes.Add("OnTextChanged", "return validDate();");
            }
        }
    }
}