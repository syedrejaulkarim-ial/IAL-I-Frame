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
    public partial class Transaction : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        //protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    var dp = (sender as ListView).FindControl("dp") as DataPager;
        //    if (dp != null)
        //    {
        //        dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        //        ReqDate = Convert.ToDateTime(Request.QueryString["dt"].ToString());
        //        BindData(ReqDate);
        //    }
        //}

        protected void BindData()
        {
            DataTable dt = IndustryUpdate.getTransactionData();
            lstTransaction.DataSource = dt;
            lstTransaction.DataBind();
        }


        protected void lst_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                ListView lstvw = dataItem.FindControl("lstItems") as ListView;
                string org = lstTransaction.DataKeys[dataItem.DisplayIndex].Value.ToString();
                lstvw.DataSource = IndustryUpdate.getTransactionDataInnerList(org);
                lstvw.DataBind();

            }
        }
    }
}