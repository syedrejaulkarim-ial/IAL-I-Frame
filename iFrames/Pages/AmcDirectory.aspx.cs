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
    public partial class AmcDirectory : MyBasePage
    {
        public string FundName=null, CorpAddr=null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tblCorp.Style.Add(HtmlTextWriterStyle.Display, "none");
                DataTable dtFund = AllMethods.getUcontrolFundHouse();                
                DataRow drFund = dtFund.NewRow();
                drFund["Mut_Name"] = "Select";
                drFund["Mut_Code"] = "";
                dtFund.Rows.InsertAt(drFund, 0);
                ddlFundHouse.DataSource = dtFund;
                ddlFundHouse.DataTextField = "Mut_Name";
                ddlFundHouse.DataValueField = "Mut_Code";
                ddlFundHouse.DataBind();

            }
            
            //ClientScript.RegisterClientScriptBlock(this.GetType(),"ValidateForm", "function validateForm(){var ddl = document.getElementById('ddlFundHouse'); if (ddl.value == '') {alert('Please select any Fund Name.'); return false;} else {return true;}}",true);
            btnGo.Attributes.Add("OnClick", "return validateForm();");
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Binddata();
        }

        private void Binddata()
        {
            DataTable dtCorp = IndustryUpdate.getAMCCorpOffice(ddlFundHouse.SelectedValue.ToString());
            if (dtCorp.Rows.Count > 0)
            {
                tblCorp.Style.Add(HtmlTextWriterStyle.Display, "block");
                lblMutName.Text = dtCorp.Rows[0]["Mut_Name"].ToString();
                FundName = dtCorp.Rows[0]["Mut_Name"].ToString();
                CorpAddr = dtCorp.Rows[0]["Reg_Add1"] == null | dtCorp.Rows[0]["Reg_Add1"].ToString().Trim() == "" ? "" : dtCorp.Rows[0]["Reg_Add1"].ToString() + " <br/>";
                CorpAddr += dtCorp.Rows[0]["Reg_Add2"] == null | dtCorp.Rows[0]["Reg_Add2"].ToString().Trim() == "" ? "" : dtCorp.Rows[0]["Reg_Add2"].ToString() + " <br/>";
                CorpAddr += dtCorp.Rows[0]["Reg_city"] == null | dtCorp.Rows[0]["Reg_city"].ToString().Trim() == "" ? "" : dtCorp.Rows[0]["Reg_city"].ToString() + " - ";
                CorpAddr += dtCorp.Rows[0]["Reg_Pin"] == null | dtCorp.Rows[0]["Reg_Pin"].ToString().Trim() == "" ? "" : dtCorp.Rows[0]["Reg_Pin"].ToString() + " <br/>";
                CorpAddr += dtCorp.Rows[0]["Reg_Phone1"] == null | dtCorp.Rows[0]["Reg_Phone1"].ToString().Trim() == "" ? "" : "Tel.- " + dtCorp.Rows[0]["Reg_Phone1"].ToString();
                CorpAddr += dtCorp.Rows[0]["Reg_Phone2"] == null | dtCorp.Rows[0]["Reg_Phone2"].ToString().Trim() == "" ? "" : ", " + dtCorp.Rows[0]["Reg_Phone2"].ToString();
                CorpAddr += dtCorp.Rows[0]["Reg_Phone3"] == null | dtCorp.Rows[0]["Reg_Phone3"].ToString().Trim() == "" ? "" : ", " + dtCorp.Rows[0]["Reg_Phone3"].ToString();
                CorpAddr += dtCorp.Rows[0]["Reg_Fax"] == null | dtCorp.Rows[0]["Reg_Fax"].ToString().Trim() == "" ? "" : " Fax - " + dtCorp.Rows[0]["Reg_Fax"].ToString() + " <br/>";
                CorpAddr += dtCorp.Rows[0]["Reg_Conper"] == null | dtCorp.Rows[0]["Reg_Conper"].ToString().Trim() == "" ? "" : "Contact Person- " + dtCorp.Rows[0]["Reg_Conper"].ToString() + " <br/>";
                CorpAddr += dtCorp.Rows[0]["E_Mail"] == null | dtCorp.Rows[0]["E_Mail"].ToString().Trim() == "" ? "" : "Email : " + dtCorp.Rows[0]["E_Mail"].ToString() + " <br/>";
                if (dtCorp.Rows[0]["Web_Site"].ToString().StartsWith("http://") == true)
                    CorpAddr += dtCorp.Rows[0]["Web_Site"] == null | dtCorp.Rows[0]["Web_Site"].ToString().Trim() == "" ? "" : "Website : <a href='" + dtCorp.Rows[0]["Web_Site"].ToString() + "' target='_blank'>" + dtCorp.Rows[0]["Web_Site"].ToString() + "</a><br/>";
                else
                    CorpAddr += dtCorp.Rows[0]["Web_Site"] == null | dtCorp.Rows[0]["Web_Site"].ToString().Trim() == "" ? "" : "Website : <a href='http://" + dtCorp.Rows[0]["Web_Site"].ToString() + "' target='_blank'>" + dtCorp.Rows[0]["Web_Site"].ToString() + "</a><br/>";

            }
            DataTable dtBrOff = IndustryUpdate.getAMCBranchOffice(ddlFundHouse.SelectedValue.ToString());
            DataColumn br = new DataColumn();
            br.ColumnName = "br_name";
            dtBrOff.Columns.Add(br);
            int i=1;
            foreach (DataRow dr in dtBrOff.Rows)
            {
                dr["br_name"] = "Branch " + i.ToString();
                i++;
            }
            lstBranchOff.DataSource = dtBrOff;
            lstBranchOff.DataBind();
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false); 
                Binddata();
            }
        }

    }
}