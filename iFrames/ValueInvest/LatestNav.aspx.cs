using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;
using iFrames.DAL;
using iFrames.Classes;

namespace iFrames.ValueInvest
{
	public partial class LatestNav : System.Web.UI.Page
	{
		#region Page Method

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				getFundHouse();
			}
		}


		protected void btnGo_Click(object sender, EventArgs e)
		{


			//getLatestNav(lbSchemeName.SelectedItem.Value);
			getLatestNav(GetSelectedScheme());

		}

		private string GetSelectedScheme()
		{
			string strScmId = string.Empty;
			foreach (ListItem li in lbSchemeName.Items)
			{
				if (li.Selected == true)
				{
					strScmId += li.Value + ",";
				}

			}
			strScmId = strScmId.Trim().TrimEnd(',');
			return strScmId;
		}

		#endregion

		#region DropdownIndexChange

		protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlFundHouse.SelectedIndex != 0)
				getSchemesList(ddlFundHouse.SelectedItem.Value);
			else
			{
				lbSchemeName.Items.Clear();
			}
		}

		protected void lbSchemeName_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		#endregion

		#region GetData



		private void getFundHouse()
		{
            DataTable dtFund;
            if (Cache["dtFund"] == null)
            {
                dtFund = AllMethods.getFundHouse();
                DataRow drFund = dtFund.NewRow();
                drFund["MutualFund_Name"] = "Select";
                drFund["MutualFund_ID"] = 0;
                dtFund.Rows.InsertAt(drFund, 0);
                Cache.Add("dtFund", dtFund, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtFund = (DataTable)Cache["dtFund"];
            }
			ddlFundHouse.DataSource = dtFund;
			ddlFundHouse.DataTextField = "MutualFund_Name";
			ddlFundHouse.DataValueField = "MutualFund_ID";
			ddlFundHouse.DataBind();
		}

		private void getSchemesList(string fundId)
		{
            DataTable dtScheme = AllMethods.getScheme(false, false, fundId);
			lbSchemeName.DataSource = dtScheme;
			lbSchemeName.DataTextField = "Sch_Short_Name";
			lbSchemeName.DataValueField = "Scheme_Id";
			lbSchemeName.DataBind();
		}

		private void getLatestNav(string SchmeId)
		{
			DataTable _dt = null;

			DataTable dtNav = AllMethods.getLatestNavDetail(SchmeId);

			DataTable dtTopfund = AllMethods.getTopFundData(SchmeId);



			try
			{
				if (dtNav != null && dtTopfund != null && dtNav.Rows.Count > 0 && dtTopfund.Rows.Count > 0)
				{
					//IEnumerable<DataRow> 
					var _result = from nav in dtNav.AsEnumerable()
								  join topfund in dtTopfund.AsEnumerable()
								  on nav["Scheme_Id"] equals topfund["Scheme_Id"]
								  select new
								  {
									  Scheme_Id = nav["Scheme_Id"],
									  Sch_Short_Name = nav["Sch_Short_Name"],
									  Nav_Date = nav["Nav_Date"],
									  Nav = nav["Nav"],
									  Per_91_Days = topfund["Per_91_Days"],
									  Per_182_Days = topfund["Per_182_Days"],
									  Per_1_Year = topfund["Per_1_Year"],
								  };


					DataTableUtility dtu = new DataTableUtility();

					_dt = dtu.JoinDataTable(dtNav, dtTopfund, "Scheme_Id");


				}
			}
			catch (Exception ex)
			{

				throw;
			}



			gvNavDetail.DataSource = _dt;
			gvNavDetail.DataBind();

		}

		#endregion



	}
}