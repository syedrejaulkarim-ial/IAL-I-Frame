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
    public partial class TopperPages : MyBasePage
    {
        public string MainHeader, subHeader;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                DataBind();
            }
        }

        protected void lstOpenTaxSect_Sorting(object sender, ListViewSortEventArgs e)
        {
            DataBind(e.SortExpression);
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

        private void DataBind(string sortcol = "", string sortOrder = "")
        {
            DataTable dt = new DataTable();
            switch (Request.QueryString["page"])
            {
                case "OpenTaxSect":
                    MainHeader = "Equity Funds (Including tax saving & sector funds)";
                    //subHeader = "Ranked on the basis of their performance (%) as on Nov 2, 2010. Click on 'Time Period' to rank funds on a particular period of your choice.";
                    dt = AllMethods.getTopperFunds("OpenTaxSect");
                    break;
                case "OpenTaxSectOther":
                    MainHeader = "Equity Funds (Excluding tax saving & sector funds)";
                    //subHeader = "Ranked on the basis of their performance (%) as on Nov 2, 2010. Click on 'Time Period' to rank funds on a particular period of your choice.";
                    dt = AllMethods.getTopperFunds("OpenTaxSectOther");
                    break;
                case "SectSpeSchAll":
                    MainHeader = "Sector Specific Schemes (All)";
                    //subHeader = "Ranked on the basis of their performance (%) as on Nov 2, 2010. Click on 'Time Period' to rank funds on a particular period of your choice.";
                    dt = AllMethods.getTopperFunds("SectSpeSchAll");
                    break;
                case "InfoScheme":
                    MainHeader = "Infotech Schemes";
                    //subHeader = "Ranked on the basis of their performance (%) as on Nov 2, 2010. Click on 'Time Period' to rank funds on a particular period of your choice.";
                    dt = AllMethods.getTopperFunds("InfoScheme");
                    break;
                case "PharmaSch":
                    MainHeader = "Pharma Schemes";
                    //subHeader = "";
                    dt = AllMethods.getTopperFunds("PharmaSch");
                    break;
                case "FmcgSch":
                    MainHeader = "FMCG Schemes";
                    //subHeader = "";
                    dt = AllMethods.getTopperFunds("FmcgSch");
                    break;
                case "OpenTax":
                    MainHeader = "Tax Planning Schemes";
                    //subHeader = "";
                    dt = AllMethods.getTopperFunds("OpenTax");
                    break;
                case "OpenBalance":
                    MainHeader = "Balanced Schemes";
                    //subHeader = "";
                    dt = AllMethods.getTopperFunds("OpenBalance");
                    break;
                case "DebtShort":
                    MainHeader = "Short Term Debt Schemes";
                    //subHeader = "";
                    dt = AllMethods.getTopperFunds("DebtShort");
                    break;
                case "DebtSch":
                    MainHeader = "Debt Schemes";
                    //subHeader = "";
                    dt = AllMethods.getTopperFunds("DebtSch");
                    break;
                case "LiquidSch":
                    MainHeader = "Liquid Schemes";
                    //subHeader = "";
                    dt = AllMethods.getTopperFunds("LiquidSch");
                    break;
                case "GiltSch":
                    MainHeader = "Gilt Schemes";
                    //subHeader = "";
                    dt = AllMethods.getTopperFunds("GiltSch");
                    break;

            }
            DataRow[] drarr;
            if (sortcol != "")           
                drarr = dt.Select("", sortcol + " asc");
            else
                drarr = dt.Select("", "short_name asc");
            DataTable dtResult = dt.Clone();
            DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));
            dtResult.Columns.Add(rankCol);
            int i = 1;
            foreach (DataRow dr in drarr)
            {
                dtResult.ImportRow(dr);
            }
            foreach (DataRow dr in dtResult.Rows)
            {
                dr["Rank"] = i;
                i++;
            }
            lstOpenTaxSect.DataSource = dtResult;
            lstOpenTaxSect.DataBind();
        }
    }
}