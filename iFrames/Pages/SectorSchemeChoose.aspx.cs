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
    public partial class SectorSchemeChoose : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = Schemes.getSectors();
                DllDataBind(dt);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        { 
            BindData();
        }

        protected void DllDataBind(DataTable dtFund)
        {            
            DataRow drFund = dtFund.NewRow();
            drFund["Sect_name"] = "Select";            
            dtFund.Rows.InsertAt(drFund, 0);
            ddlSector.DataSource = dtFund;
            ddlSector.DataTextField = "Sect_name";
            ddlSector.DataValueField = "Sect_name";
            ddlSector.DataBind();
        }

        //protected void btnClear_Click(object sender, EventArgs e)
        //{
        //    ddlSector.SelectedIndex = 0;
        //    ddlPercentage.SelectedIndex=0;            
        //}

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                lstSchemes.DataSource = ViewState["SectorInfo"];
                lstSchemes.DataBind();
            }
        }

        protected void BindData()
        {
            ViewState["SectorInfo"]=Schemes.getSectorInfos(ddlSector.SelectedItem.Text, ddlPercentage.SelectedItem.Value.ToString());
            lstSchemes.DataSource = ViewState["SectorInfo"];
            lstSchemes.DataBind();
        }
    }
}