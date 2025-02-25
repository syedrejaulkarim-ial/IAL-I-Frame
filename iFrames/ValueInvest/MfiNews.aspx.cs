using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.DAL;
using System.Data;

namespace iFrames.ValueInvest
{
	public partial class MfiNews : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
				lvMfinews.DataSource = GetDefaultNewsData();
				lvMfinews.DataBind();
			}

			// lvMfinews.PagePropertiesChanged += new EventHandler(lvpagechanged);
		}


		private DataTable GetDefaultNewsData()
		{
			string searchableTerm = string.Empty;
			if (!string.IsNullOrEmpty(txtSearchBox.Text.Trim()))
			{
				searchableTerm = txtSearchBox.Text.Trim();
				searchableTerm = Server.HtmlEncode(searchableTerm);
			}
			DataTable dt = LoadNews(searchableTerm);

			return dt;
		}



		private DataTable LoadNews(string strSearch)
		{
			DataTable _dt = null;
			using (var dcWiseInv = new SIP_ClientDataContext())
			{

				var objNews = (from wiseNew in dcWiseInv.T_MFI_NEW_Clients
							   //where wiseNew.DISPLAY_DATE > DateTime.Now.AddDays(-7)
							   orderby wiseNew.DISPLAY_DATE ascending
							   select new
							   {
								   wiseNew.NEWS_HEADLINE,
								   wiseNew.MATTER,
								   wiseNew.DISPLAY_DATE
							   }).OrderByDescending(XhtmlMobileDocType => XhtmlMobileDocType.DISPLAY_DATE).Take(50);

				if (!string.IsNullOrEmpty(strSearch))
				{

					objNews = (from wiseNew in dcWiseInv.T_MFI_NEW_Clients
							   where wiseNew.MATTER.Contains(strSearch) || wiseNew.NEWS_HEADLINE.Contains(strSearch)
							   orderby wiseNew.DISPLAY_DATE ascending
							   select new
							   {
								   wiseNew.NEWS_HEADLINE,
								   wiseNew.MATTER,
								   wiseNew.DISPLAY_DATE
							   }).OrderByDescending(XhtmlMobileDocType => XhtmlMobileDocType.DISPLAY_DATE).Take(50);
				}

				_dt = objNews.ToDataTable();
			}
			return _dt;
		}

		//protected void gvNews_PageIndexChanging(object sender, GridViewPageEventArgs e)
		//{
		//    gvMfiNews.PageIndex = e.NewPageIndex;
		//    setDataTable();
		//}

		protected void lvMfinews_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
		{

			dpNews.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
			lvMfinews.DataSource = GetDefaultNewsData();
			lvMfinews.DataBind();

		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			lvMfinews.DataSource = GetDefaultNewsData();
			lvMfinews.DataBind();
			dpNews.SetPageProperties(0, dpNews.MaximumRows, true);// for setting first page           

		}

		protected override void OnPreRender(EventArgs e)
		{
			lvMfinews.DataSource = GetDefaultNewsData();
			lvMfinews.DataBind();
			base.OnPreRender(e);
		}


		//protected void dpNews_PreRender(object sender, EventArgs e)
		//{
		//    lvMfinews.DataSource = GetDefaultNewsData();
		//    lvMfinews.DataBind();
		//}

		//protected void lvpagechanged(object sender, EventArgs e)
		//{
		//    lvMfinews.DataSource = GetDefaultNewsData();
		//    lvMfinews.DataBind();
		//}


	}
}