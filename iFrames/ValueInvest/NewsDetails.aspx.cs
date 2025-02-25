using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.DAL;

namespace iFrames.ValueInvest
{
	public partial class NewsDetails : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// newsHeader.Text = Request.QueryString[0];
				newsHeader.Text = Request.QueryString["newsHeadr"];
				detailNew.Text = GetQueryStr();
			}
		}


		private string GetQueryStr()
		{
			// string newsHeader = Request.QueryString[0];
			string newsHeader = Request.QueryString["newsHeadr"];
			//newsHeader = Server.HtmlEncode(newsHeader);
			string res = string.Empty;
			using (var dcWiseInv = new SIP_ClientDataContext())
			{
				var objNews = (from wiseNew in dcWiseInv.T_MFI_NEW_Clients
							   where wiseNew.NEWS_HEADLINE.ToUpper().Trim() == newsHeader.Trim().ToUpper()
							   select wiseNew.MATTER).Take(1).First();
				if (objNews != null)
					res = objNews.ToString();
			}

			return res;

		}
	}
}