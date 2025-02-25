using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.DAL;
using iFrames.BLL;

namespace iFrames.Pages
{
    public partial class TopFund : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        } 
        
        protected void btnGo_Click(object sender, EventArgs e)
        {
            DataBind();    
        }


        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                DataBind();
            }
        }

        private void DataBind()
        {
            DataTable dtResult = new DataTable();
            dtResult = AllMethods.getTopFundResult(Convert.ToInt32(ddlRank.SelectedValue), ddlType.SelectedValue, ddlCategory.SelectedValue, ddlPeriod.SelectedValue);
            DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));
            dtResult.Columns.Add(rankCol);
            int i = 1;
            foreach (DataRow dr in dtResult.Rows)
            {
                dr["Rank"] = i;
                i++;
            }            
            lstResult.DataSource = dtResult;
            lstResult.DataBind();
        }

    }
}