using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;

namespace iFrames.WiseInvest
{
    public partial class NfoUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNature();
            }

        }

        protected void LoadNature()
        {

            DataTable _dt = AllMethods.getNature();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("Select All ", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlCategory.Items[0].Selected = true;
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

            dpNFO.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GetNfoUpdate();
        }

        private void GetNfoUpdate()
        {
            try
            {
                DataTable _dt = AllMethods.getNFOUpdate(Convert.ToInt32(ddlCategory.SelectedItem.Value));

                if (_dt != null && _dt.Rows.Count > 0)
                {
                    _dt = _dt.OrderBy("Nfo_Close_Date desc");
                }
                listVwNFODetail.DataSource = _dt;
                listVwNFODetail.DataBind();

                if (_dt == null)
                    resetDp();
                else
                    dpNFO.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GetNfoUpdate();
            dpNFO.SetPageProperties(0, dpNFO.MaximumRows, true);
        }

        private void resetDp()
        {

            //var dpCurrentPageLabel = dpNFO.Controls[0].FindControl("CurrentPageLabel") as Label;
            //if (dpCurrentPageLabel != null) dpCurrentPageLabel.Text = "0";

            //var dpTotalPagesLabel = dpNFO.Controls[0].FindControl("TotalPagesLabel") as Label;
            //if (dpTotalPagesLabel != null) dpTotalPagesLabel.Text = "0";

            //var dpTotalItemsLabel = dpNFO.Controls[0].FindControl("TotalItemsLabel") as Label;
            //if (dpTotalItemsLabel != null) dpTotalItemsLabel.Text = "0";

            dpNFO.Visible = false;

        }
    }
}