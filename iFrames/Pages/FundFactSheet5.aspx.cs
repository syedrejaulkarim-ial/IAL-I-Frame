using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;
using System.Data;
namespace iFrames.Pages
{
    public partial class FundFactSheet5 : MyBasePage
    {
        public string schCode, shortName;
        
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            schCode = Request.QueryString["sch"];
            if (!IsPostBack)
            {
                
                DrpYear.DataSource = AllMethods.getYears();
                DrpYear.AppendDataBoundItems = true;
                DrpYear.Items.Insert(0, new ListItem("Choose Year", "0"));
                DrpYear.DataTextField ="yrs";
                DrpYear.DataValueField = "yrs";
                DrpYear.DataBind();
                DrpMon.DataSource = AllMethods.getMonths();
                DrpMon.AppendDataBoundItems = true;
                DrpMon.Items.Insert(0, new ListItem("Choose Month", "0"));
                DrpMon.DataTextField = "charmon";
                DrpMon.DataValueField = "digimons";                
                DrpMon.DataBind();
                
                dt = FactSheets.GetFactSheetFive(schCode);
                LstFundNews.DataSource = dt;
                LstFundNews.DataBind();
                shortName=dt.Rows.Count>0?dt.Rows[0]["short_name"].ToString():"";
                ////GetSales("ICICI Prudential Mutual Fund revises exit load structure under its Gilt Fund - Treasury Plan");
            }

        }

        protected void BtnGo_Click(object sender, EventArgs e)
        {
            dt.Clear();
            dt = FactSheets.GetFactSheetFive(schCode, DrpYear.SelectedItem.Text,DrpMon.SelectedValue.ToString());
            LstFundNews.DataSource = dt;
            LstFundNews.DataBind();
            
        }
        protected void  GetSales(string newsheadline)
        {
            //string concat = string.Empty;
            ////string[] a = newsheadline.Split(new char[] {' '});
            //newsheadline = newsheadline.Replace(' ', '+');
            //HttpUtility.UrlEncode(
           
        }
    }
}