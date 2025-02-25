using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iFrames.BLL;
namespace iFrames.Pages
{
    public partial class ChooseDebtScheme : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.DrpRating.Items.Clear();
                this.DrpRating.DataSource = Schemes.GetRating();
                this.DrpRating.AppendDataBoundItems = true;
                this.DrpRating.Items.Insert(0, new ListItem("Select Rating", " "));
                this.DrpRating.DataTextField = "Rating";
                this.DrpRating.DataValueField = "Rating";
                this.DrpRating.DataBind();
                
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            string check = "0"; string condition = null;
           string structure = null; string investobj = null; string nature = null;
            DateTime FundaAgeDate =  new DateTime();
            string MinInvst = null;
            string FundSize = null;
            string period = null; string anyrturn = null; string dividend = null;
            string nri = null; string repartiability = null; string taxbenefit = null;
            string rating = null; string anypercent = null;
            switch (DrpFundAge.SelectedItem.Value.ToString())
            {
                case "3month": FundaAgeDate = DateTime.Now.AddMonths(-3); condition = ">="; break;
                case "6month": FundaAgeDate = DateTime.Now.AddMonths(-3); condition = "<"; break;
                case "1year": FundaAgeDate = DateTime.Now.AddYears(-1); condition = "<"; break;
                case "3year": FundaAgeDate = DateTime.Now.AddYears(-3); condition = "<"; break;
                case "5year": FundaAgeDate = DateTime.Now.AddYears(-5); condition = "<"; break;
            }

             structure = RadioStructureLst.SelectedValue != " " ? RadioStructureLst.SelectedValue.ToString() : null;
             investobj = RadioIncDiv.SelectedValue != " " ? RadioIncDiv.SelectedValue.ToString() : null;
             nature = DrpNature.SelectedValue != " " ? DrpNature.SelectedValue.ToString() : null;
             MinInvst = DrpMinInvst.SelectedValue != " "?DrpMinInvst.SelectedValue.ToString():null;
             FundSize = DrpFundSize.SelectedValue != " " ? DrpFundSize.SelectedValue.ToString() : null;
             period = DrpPeriopd.SelectedValue != " " ? DrpPeriopd.SelectedValue.ToString() : null;
             anyrturn = DrpReturn.SelectedValue != " " ? DrpReturn.SelectedValue.ToString() : null;
             dividend = DrpCurDivYield.SelectedValue != " " ? DrpCurDivYield.SelectedValue.ToString() : null;
             nri = RadioNriInvt.SelectedValue != " " ? RadioNriInvt.SelectedValue.ToString() : null;
             repartiability = RadioRepartial.SelectedValue != " " ? RadioRepartial.SelectedValue.ToString() : null;
             taxbenefit = ChkTaxBnft.Checked ==true ? ChkTaxBnft.Text.ToString() : null;
             rating = DrpRating.SelectedValue != " " ? DrpRating.SelectedValue.ToString() : null;
             anypercent = DrpPercent.SelectedValue != " " ? DrpPercent.SelectedValue.ToString() : null;

             //if (FundSize != null)
             //    check += ",1";
             //if (MinInvst != null)
             //    check += ",2";
             
             //if (DrpFundAge.SelectedItem.Value != " ")             
             //    check+=",3";
             
             //if (rating!=null)             
             //    check += ",4";



            //string querybuilder = null;
            //querybuilder="select distinct s.sch_code,s.Short_name,s.type1 'Structure',s.type3 'Type' ";
            //querybuilder += FundSize != null ? ",f.fund_size " : null;
            //querybuilder += MinInvst != null ? ",s.min_invt " : null;
            //querybuilder += DrpFundAge.SelectedItem.Value.ToString() != " " ? ",s.close_date " : null;
            //querybuilder += rating != null ? ",p.rt_per " : null;
            //querybuilder += "from scheme_info s ,mut_fund m ";
            //querybuilder += FundSize != null ? ",fundsize f " : null;
            //querybuilder += period != null || dividend!=null  ? ",top_fund t " : null;
            //querybuilder += rating != null ? ",port_rate p " : null;
            //querybuilder+="where s.mut_code =m.mut_code ";            
            //querybuilder += FundSize != null ? " and s.sch_code=f.sch_code " : null;
            //querybuilder += period != null || dividend != null ? "and t.sch_code=s.sch_code  " : null;
            //querybuilder += rating != null ? " and s.sch_code=p.sch_code " : null;
            //querybuilder += " and s.nav_check<>'red'" + " and s.nature not like 'Equity%'";
            //querybuilder += MinInvst != null ? " and s.min_invt " + MinInvst : null;
            //querybuilder += " and s.type1='" + structure+"'";
            //querybuilder += nature != null ? "and s.nature like '" + nature+"%' " : null;
            //querybuilder += investobj != null ? " and s.type3='" + investobj+"'" : null;
            //querybuilder += DrpFundAge.SelectedItem.Value.ToString() != " " ? " and s.close_date " + condition +"'"+ Convert.ToDateTime(FundaAgeDate).ToString("MM/dd/yyyy")+"'" : null;
            //querybuilder += FundSize != null ? " and f.fund_size "+DrpFundSize.SelectedValue+" and f.date = (select max(date) from fundsize where sch_code = f.sch_code) " : null;
            //querybuilder += period != null && anyrturn != null ? " and t." + period+DrpReturn.SelectedValue : null;
            //querybuilder += dividend != null ? " and t.div_yld " +  DrpCurDivYield.SelectedValue : null;
            //querybuilder += nri != null ? " and s.nri='" + nri+"'" : null;
            //querybuilder += repartiability != null ? " and s.repatri='" + repartiability+"'" : null;
            //querybuilder += taxbenefit != null ? " and s.tax_ben3='" + taxbenefit+"'" : null;
            //querybuilder += rating != null ? " and p.rating='" + rating+"'" : null;
            //querybuilder += rating != null && anypercent != null ? " and p.rt_per" + anypercent : null;





            DataTable dtChooseScheme = Schemes.GetChooseSchemeDetail(structure, investobj, nature,nri, repartiability, DrpFundAge.SelectedItem.Value.ToString()
                , FundaAgeDate, MinInvst, FundSize, period, anyrturn, dividend, taxbenefit, rating, anypercent, condition);
            lstChooseScheme.DataSource = dtChooseScheme;
            lstChooseScheme.DataBind();
            ViewState["dtChooseScheme"] = dtChooseScheme;
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                lstChooseScheme.DataSource = ViewState["dtChooseScheme"] as DataTable;
                lstChooseScheme.DataBind();                
            }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
}