using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.DAL;
using iFrames.BLL;
using iFrames.Pages;
using System.Linq.Dynamic;

namespace iFrames.ValueInvest
{
	public partial class TopFund : System.Web.UI.Page
	{



		#region Page
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				LoadNature();
				LoadStructure();
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			// DataBind();
			base.OnPreRender(e);
		}

		#endregion


		#region Event

		protected void btnGo_Click(object sender, EventArgs e)
		{
			DataBind();
			var dp = lstResult.FindControl("dpTopFund") as DataPager;
			if (dp != null)
			{
				dp.SetPageProperties(0, dp.MaximumRows, true);
			}
			lstResult.Visible = true;
		}

		protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
		{
			var dp = (sender as ListView).FindControl("dpTopFund") as DataPager;
			if (dp != null)
			{
				dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
				DataBind();
			}
		}


		#endregion


		#region Load Data

		protected void LoadNature()
		{
			DataTable _dt = AllMethods.getNature();
			ddlCategory.Items.Clear();
			ddlCategory.Items.Add(new ListItem("All", "-1"));
			foreach (DataRow drow in _dt.Rows)
			{
				ddlCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
			}

			ddlCategory.Items[0].Selected = true;
		}


		protected void LoadStructure()
		{
			DataTable _dt = AllMethods.getStructure();
			ddlType.Items.Clear();
			ddlType.Items.Add(new ListItem("All", "-1"));
			foreach (DataRow drow in _dt.Rows)
			{
				ddlType.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
			}

			ddlType.Items[0].Selected = true;
		}


		#endregion


		#region Fetch Data

		private void DataBind()
		{
			DataTable dtResult = new DataTable();
            dtResult = AllMethods.getTopFundRank_exceptDivandDir(Convert.ToInt32(ddlRank.SelectedItem.Value), Convert.ToInt32(ddlType.SelectedItem.Value), Convert.ToInt32(ddlCategory.SelectedItem.Value), ddlPeriod.SelectedItem.Value);

			if (dtResult != null)
			{
				DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));
				dtResult.Columns.Add(rankCol);
				int i = 1;
				foreach (DataRow dr in dtResult.Rows)
				{
					dr["Rank"] = i;
					i++;
				}
			}
			lstResult.DataSource = dtResult;
			lstResult.DataBind();
			if (dtResult != null)
				lbtopText.Text = ddlRank.SelectedItem.Text + " Funds Period :" + ddlPeriod.SelectedItem.Text;
			else lbtopText.Text = "";
		}


		#endregion

		protected void btnRestClick(object sender, EventArgs e)
		{
			ddlCategory.SelectedIndex = 0;
			ddlPeriod.SelectedIndex = 0;
			ddlRank.SelectedIndex = 0;
			ddlType.SelectedIndex = 0;
			lstResult.Visible = false;
			lbtopText.Text = "";
		}

	}
}