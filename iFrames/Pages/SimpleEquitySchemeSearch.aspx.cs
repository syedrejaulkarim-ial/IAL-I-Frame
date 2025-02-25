using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iFrames.BLL;

namespace iFrames.Pages
{
    public partial class SimpleEquitySchemeSearch : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = Schemes.getSectorsNames();
                DataRow dr = dt.NewRow();
                dr["nat_sp"] = "Any Sector";
                dt.Rows.InsertAt(dr, 0);
                ddlSector.DataSource = dt;
                ddlSector.DataTextField = "nat_sp";
                ddlSector.DataValueField = "nat_sp";
                ddlSector.SelectedIndex = 0;
                ddlSector.DataBind();
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            string ChkTaxBenefit1 = "", ChkTaxBenefit2 = "", RadioIncDivVal = "", RadioListingVal = "", RadioNriInvtVal = "", RadioRepartialVal="";
            foreach (ListItem l in ChkTaxBenefit.Items)
            {
                if (l.Selected && l.Value == "88")
                    ChkTaxBenefit1 = "88";                
                if (l.Selected && l.Value == "112")
                    ChkTaxBenefit2="112";               
            }
            if (RadioIncDiv.SelectedIndex > -1)
                RadioIncDivVal = RadioIncDiv.SelectedItem.Text;
            if (RadioListing.SelectedIndex > -1)
                RadioListingVal = RadioListing.SelectedItem.Text;
            if (RadioNriInvt.SelectedIndex > -1)
                RadioNriInvtVal = RadioNriInvt.SelectedItem.Text;
            if (RadioRepartial.SelectedIndex > -1)
                RadioRepartialVal = RadioRepartial.SelectedItem.Text;

            DataTable dt = Schemes.getSimpleEquityScheme(RadioStructureLst.SelectedItem.Text, 
                DrpNature.SelectedItem.Value.ToString(), 
                DrpFundAge.SelectedItem.Value.ToString(), 
                ddlSector.SelectedItem.Value.ToString(),
                RadioIncDivVal, 
                DrpMinInvst.SelectedItem.Value.ToString(), 
                DrpFundSize.SelectedItem.Value.ToString(), 
                DrpPeriopd.SelectedItem.Value.ToString(), 
                DrpReturn.SelectedItem.Value.ToString(), 
                DrpCurDivYield.SelectedItem.Value.ToString(),
                RadioListingVal,
                RadioNriInvtVal,
                RadioRepartialVal, 
                ChkTaxBenefit1, 
                ChkTaxBenefit2);


            if (DrpCurDivYield.SelectedItem.Value.Trim() != "")
            {
                if (DrpPeriopd.SelectedItem.Value.Trim() != "")
                {
                    if (DrpPeriopd.SelectedItem.Value.Trim() == "per_3yr" | DrpPeriopd.SelectedItem.Value.Trim() == "per_1yr")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["div_Yld"] = dr["div_Yld"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["div_Yld"].ToString()), 1).ToString();
                            dr["per_91days"] = dr["per_91days"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_91days"].ToString()), 1).ToString();
                            dr["per_1yr"] = dr["per_1yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_1yr"].ToString()), 1).ToString();
                            dr["per_3yr"] = dr["per_3yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_3yr"].ToString()), 1).ToString();
                        }
                    }
                    else //if (DrpPeriopd.SelectedItem.Value.Trim() == "per_5yr" | DrpPeriopd.SelectedItem.Value.Trim() == "since_incept")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["div_Yld"] =dr["div_Yld"].ToString().Trim()==""?"--": Math.Round(Convert.ToDecimal(dr["div_Yld"].ToString()), 1).ToString();
                            dr[DrpPeriopd.SelectedItem.Value.Trim()] = dr[DrpPeriopd.SelectedItem.Value.Trim()].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr[DrpPeriopd.SelectedItem.Value.Trim()].ToString()), 1).ToString();
                            dr["per_1yr"] = dr["per_1yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_1yr"].ToString()), 1).ToString();
                            dr["per_3yr"] = dr["per_3yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_3yr"].ToString()), 1).ToString();
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["div_Yld"] = dr["div_Yld"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["div_Yld"].ToString()), 1).ToString();
                        dr["per_91days"] = dr["per_91days"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_91days"].ToString()), 1).ToString();
                        dr["per_1yr"] = dr["per_1yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_1yr"].ToString()), 1).ToString();
                        dr["per_3yr"] = dr["per_3yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_3yr"].ToString()), 1).ToString();
                    }
                }
               
            }
            else
            {
                if (DrpPeriopd.SelectedItem.Value.Trim() != "")
                {
                    if (DrpPeriopd.SelectedItem.Value.Trim() == "per_3yr" | DrpPeriopd.SelectedItem.Value.Trim() == "per_1yr")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["per_91days"] = dr["per_91days"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_91days"].ToString()), 1).ToString();
                            dr["per_1yr"] = dr["per_1yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_1yr"].ToString()), 1).ToString();
                            dr["per_3yr"] = dr["per_3yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_3yr"].ToString()), 1).ToString();
                        }

                    }
                    else //if (DrpPeriopd.SelectedItem.Value.Trim() == "per_5yr" | DrpPeriopd.SelectedItem.Value.Trim() == "since_incept")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr[DrpPeriopd.SelectedItem.Value.Trim()] = dr[DrpPeriopd.SelectedItem.Value.Trim()].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr[DrpPeriopd.SelectedItem.Value.Trim()].ToString()), 1).ToString();
                            dr["per_1yr"] = dr["per_1yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_1yr"].ToString()), 1).ToString();
                            dr["per_3yr"] = dr["per_3yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_3yr"].ToString()), 1).ToString();
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["per_91days"] = dr["per_91days"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_91days"].ToString()), 1).ToString();
                        dr["per_1yr"] = dr["per_1yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_1yr"].ToString()), 1).ToString();
                        dr["per_3yr"] = dr["per_3yr"].ToString().Trim() == "" ? "--" : Math.Round(Convert.ToDecimal(dr["per_3yr"].ToString()), 1).ToString();
                    }
                }
            }
            dt.AcceptChanges();
            ViewState["dtChooseScheme"] = dt;
            lstChooseScheme.DataSource = dt;
            lstChooseScheme.DataBind();
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