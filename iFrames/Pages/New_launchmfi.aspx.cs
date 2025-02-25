using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iFrames.BLL;
namespace iFrames.Pages
{
    public partial class New_launchmfi : MyBasePage
    {
        string MutcodeFromBase = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
           //MutcodeFromBase = "MF024,MF057,MF059";
            if (!IsPostBack)
            {
                DataTable Getdt = new DataTable();
                Getdt = Schemes.GetdtTbl(this.PropMutCode);
                lstVwLaunch.DataSource = Getdt;
                lstVwLaunch.DataBind();
                Session["dtTable"] =Getdt;
            }
        }

        //protected void lstVwLaunch_PreRender(object sender, EventArgs e)
        //{
        //    lstVwLaunch.DataSource = Session["dtTable"] as DataTable;
        //    lstVwLaunch.DataBind();
            
        //}
        protected void Page_PreRender(object sender, EventArgs e)
        {
            lstVwLaunch.DataSource = Session["dtTable"] as DataTable;
            lstVwLaunch.DataBind();

        }

        protected void lstVwLaunch_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            //if (e.CommandName == "view")
            //{
            //    //if (e.Item.ItemType == ListViewItemType.DataItem)
            //    ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            //    string sch_code =lstVwLaunch.DataKeys[dataItem.DisplayIndex].Value.ToString();
            //    //DataTable dtRetrive = Schemes.GetdtTblRetrive(sch_code);
            //    Label lblOpen = (e.CommandSource as LinkButton).NamingContainer.FindControl("lblOpen") as Label;
            //    lblOpen.Text = sch_code;
            //}

        }

        protected void lstVwLaunch_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                ListView lstvw = dataItem.FindControl("lstVwLaunchinfo") as ListView;
                string sch_code = lstVwLaunch.DataKeys[dataItem.DisplayIndex].Value.ToString();

                //Label lblOpen = (lstVwLaunch.Items[dataItem.DisplayIndex+1] as ListViewDataItem).FindControl("lblOpen") as Label;// (Label)e.Item.FindControl("lblOpen");
                //lblOpen.Text = sch_code;
                //DataTable dtRetrive = Schemes.GetdtTblRetrive(sch_code);
                DataTable dt = Schemes.GetdtTblRetrive(sch_code);
                lstvw.DataSource = Schemes.GetdtTblRetrive(sch_code);
                lstvw.DataBind();

            }

        }

        
    }
}