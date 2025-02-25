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
    public partial class FundFactSheet1 : MyBasePage
    {
        public string schCode,schName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                schCode = Request.QueryString["sch"];
                DataTable dt = FactSheets.getFactsheet1(schCode);
                dtvFundManager.DataSource = dt;
                dtvFundManager.DataBind();
                dtvSchemeType.DataSource = dt;
                dtvSchemeType.DataBind();
                dtvLastDivdend.DataSource = dt;
                dtvLastDivdend.DataBind();
                dtvFundFacts.DataSource = dt;
                dtvFundFacts.DataBind();
                schName = dt.Rows.Count > 0 ? dt.Rows[0]["sch_name"].ToString() : "";
            }
        }

    }
}