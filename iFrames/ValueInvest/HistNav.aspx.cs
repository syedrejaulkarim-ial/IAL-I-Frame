using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;

namespace iFrames.ValueInvest
{
	public partial class HistNav : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				getFundHouse();
			}
		}

		protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlFundHouse.SelectedIndex != 0)
				getSchemesList(ddlFundHouse.SelectedItem.Value);
			else
			{
				listboxSchemeName.Items.Clear();
			}
		}

		protected void listboxSchemeName_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void getSchemesList(string fundId)
		{
            DataTable dtScheme = AllMethods.getScheme(false, false, fundId);
			listboxSchemeName.DataSource = dtScheme;
			listboxSchemeName.DataTextField = "Sch_Short_Name";
			listboxSchemeName.DataValueField = "Scheme_Id";
			listboxSchemeName.DataBind();
		}

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

		protected void btnSubmit_Click(object sender, EventArgs e)
		{


			getNavDetail();
		}

		private void getNavDetail()
		{

			DataTable _dt = null;
			string SchmeId = GetSelectedScheme();
			DateTime frmDate, toDate;
			if (txtfromDate.Text != string.Empty && txtToDate.Text != string.Empty)
			{
				frmDate = Convert.ToDateTime(txtfromDate.Text);
				toDate = Convert.ToDateTime(txtToDate.Text);
			}
			else
			{
				frmDate = DateTime.Today.AddDays(-8);
				toDate = DateTime.Today.AddDays(-2);
			}

			try
			{
				_dt = AllMethods.getHistNavDetails(SchmeId, frmDate, toDate);

				if (_dt != null)
				{
					_dt = _dt.OrderBy("Nav_Date desc");

					if (_dt.Rows.Count > 0)
					{
						gvNavHistDetail.DataSource = _dt;
						gvNavHistDetail.DataBind();
					}
				}
				else
				{
					gvNavHistDetail.DataSource = _dt;
					gvNavHistDetail.DataBind();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}


		}

		private string GetSelectedScheme()
		{
			string strScmId = string.Empty;
			foreach (ListItem li in listboxSchemeName.Items)
			{
				if (li.Selected == true)
				{
					strScmId += li.Value + ",";
				}

			}
			strScmId = strScmId.Trim().TrimEnd(',');
			return strScmId;
		}
	}
}