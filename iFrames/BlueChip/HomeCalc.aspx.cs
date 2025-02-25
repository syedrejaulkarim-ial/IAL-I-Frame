using iFrames.BLL;
using iFrames.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.BlueChip
{
    public partial class HomeCalc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSchemeMaster();
                LoadAMC();
                //LoadFund();
            }
        }

        protected void btnsubmit_Clicked(object sender, EventArgs e)
        {
            var SchemeID = ddlSchemes.SelectedValue;
            if(Convert.ToInt32(SchemeID) == -1)
            {
                Response.Write("<script>alert('No Scheme Found');</script>");
                return;
            }
            var TextFrom = txtfromDate.Text;
            if(!string.IsNullOrEmpty(TextFrom))
            {
                try
                {
                    var sipStartDate = new DateTime(Convert.ToInt16(txtfromDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtfromDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtfromDate.Text.Split('/')[0]));
                }
                catch
                {
                    Response.Write(@"<script>alert('Format issue in From Date Found');
                        document.getElementById('txtfromDate').focus();</script>");
                    return;
                }
            }
            else
            {
                Response.Write("<script>alert('Empty From Date Found');</script>");
                return;
            }


            var TextTo = txtToDate.Text;
            if (!string.IsNullOrEmpty(TextTo))
            {
                try
                {
                    var sipStartDate = new DateTime(Convert.ToInt16(txtToDate.Text.Split('/')[2]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[1]),
                                         Convert.ToInt16(txtToDate.Text.Split('/')[0]));
                }
                catch
                {
                    Response.Write(@"<script>alert('Format issue in End Date Found');
                        document.getElementById('txtToDate').focus();</script>");
                    return;
                }
            }
            else
            {
                Response.Write("<script>alert('Empty End Date Found');</script>");
                return;
            }

            var installAmmount = txtinstallLs.Text;
            if (!string.IsNullOrEmpty(installAmmount) && Double.TryParse(installAmmount,out double trydata))
            {
                
            }
            else
            {
                Response.Write("<script>alert('Issue in Installment Ammount');</script>");
                return;
            }

            var Param = SchemeID + "#" + TextFrom + "#" + TextTo + "#" + installAmmount;
            
            var encryptedString = Encrypt_QueryString(Param);

            //Response.Redirect("/BlueChip/Return%20Calc.aspx?param="+encryptedString, true);
            Response.Write("<script>");
            Response.Write("window.open('"+ "http://www.bluechipindia.co.in/Bluechipwebnew/MutualFund/frmReturnCalculator.aspx?param=" + encryptedString + "','_blank')");
            Response.Write("</script>");

        }

        public static string Encrypt_QueryString(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }


        protected void btnreset_Clicked(object sender, EventArgs e)
        {
            Response.Redirect("/BlueChip/HomeCalc.aspx", true);
        }


        protected void LoadAMC()
        {
            DataTable dtFund;
            dtFund = AllMethods.getFundHouse();
            DataRow drFund = dtFund.NewRow();
            drFund["MutualFund_Name"] = "-Select MutualFund-";
            drFund["MutualFund_ID"] = -1;
            dtFund.Rows.InsertAt(drFund, 0);

            ddlFundHouse.DataSource = dtFund;
            ddlFundHouse.DataTextField = "MutualFund_Name";
            ddlFundHouse.DataValueField = "MutualFund_ID";
            ddlFundHouse.DataBind();
        }

        protected void LoadSchemeMaster()
        {
            DataTable dtResult = new DataTable();
            dtResult = AllMethods.getSebiSchemeBluechip(-1, -1, -1, 2, -1, false);
            Cache.Add("dtSchemeAllMaster", dtResult, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);

            var drFund = dtResult.NewRow();
            drFund["Sch_Short_Name"] = "-Select Scheme-";
            drFund["Scheme_Id"] = -1;
            dtResult.Rows.InsertAt(drFund, 0);
            ddlSchemes.DataSource = dtResult;
            ddlSchemes.DataTextField = "Sch_Short_Name";
            ddlSchemes.DataValueField = "Scheme_Id";
            ddlSchemes.DataBind();
        }

        protected void LoadFund(int AMCID = -1)
        {
            DataTable dtResult = new DataTable();

            if (Cache["dtSchemeAllMaster"] == null)
            {
                dtResult = AllMethods.getSebiSchemeBluechip(-1, -1, -1, 2, -1, false);
                Cache.Add("dtSchemeAllMaster", dtResult, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtResult = (DataTable)Cache["dtSchemeAllMaster"];
            }

            var dtFund = new DataTable();
            if (AMCID == -1)
            {
                var _FundData = dtResult.AsEnumerable().Where(x => x["Fund_ID"] != DBNull.Value && !string.IsNullOrEmpty(x["Fund_ID"].ToString()))
                .Select(row => new
                {
                    Fund_ID = row.Field<object>("Fund_ID"),
                    Fund_Name = row.Field<object>("Fund_Name")
                }).Distinct();

                dtFund = _FundData.ToDataTable();
            }
            else
            {
                var _FundData = dtResult.AsEnumerable().Where(x => x["MUTUALFUND_ID"].ToString() == AMCID.ToString()).Where(x => x["Fund_ID"] != DBNull.Value && !string.IsNullOrEmpty(x["Fund_ID"].ToString()))
             .Select(row => new
             {
                 Fund_ID = row.Field<object>("Fund_ID"),
                 Fund_Name = row.Field<object>("Fund_Name")
             }).Distinct();

                dtFund = _FundData.ToDataTable();
            }

            DataRow drFund = dtFund.NewRow();
            drFund["Fund_ID"] = -1;
            drFund["Fund_Name"] = "-Select Fund-";
            dtFund.Rows.InsertAt(drFund, 0);

            ddlFundName.DataSource = dtFund;
            ddlFundName.DataTextField = "Fund_Name";
            ddlFundName.DataValueField = "Fund_ID";
            ddlFundName.DataBind();
        }

        protected void ddlFundHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            SIPSchDt.Text = "";
            txtfromDate.Text = "";
            txtToDate.Text = "";
            SchemeFilteration(Convert.ToInt32(ddlFundHouse.SelectedValue), -1);
        }

        protected void ddlFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            SchemeFilteration(Convert.ToInt32(ddlFundHouse.SelectedValue), Convert.ToInt32(ddlFundName.SelectedValue));
        }

        protected void ddlscheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            SIPSchDt.Text = "";
            if (ddlSchemes.SelectedIndex == 0 || ddlSchemes.SelectedItem.Value == "-1")
            {
                return;
            }

            #region Launch Date

            if (!string.IsNullOrEmpty(ddlSchemes.SelectedItem.Value) && Convert.ToInt32(ddlSchemes.SelectedItem.Value) != -1 && Convert.ToInt32(ddlSchemes.SelectedItem.Value) != 0)
            {
                using (var principl = new PrincipalCalcDataContext())
                {
                    string schmeId = ddlSchemes.SelectedItem.Value;
                    var allotdate = from ind in principl.T_SCHEMES_MASTERs
                                    where ind.Scheme_Id == Convert.ToDecimal(schmeId)
                                    select new
                                    {
                                        LaunchDate = ind.Launch_Date
                                    };
                    if (allotdate != null && allotdate.Count() > 0)
                    {
                        SIPSchDt.Text = Convert.ToDateTime(allotdate.Single().LaunchDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
            }
            #endregion
            SetInceptionOnDropDown();

            //txtinstall.Text = "";
            txtfromDate.Text = "";
            txtToDate.Text = "";
        }


        public void SetInceptionOnDropDown()
        {

            if ((ViewState["SchemeInception"] != null) && (ddlSchemes.Items.Count > 0) && ddlSchemes.SelectedIndex > 0)
            {

                Dictionary<string, string> SchemeInception = (Dictionary<string, string>)(ViewState["SchemeInception"]);


                for (int i = 0; i < ddlSchemes.Items.Count; i++)
                {
                    string s = "";
                    if (SchemeInception.TryGetValue(ddlSchemes.SelectedItem.Value, out s) && ddlSchemes.Items[i].Selected == true)
                    {
                        ddlSchemes.Items[i].Attributes.Add("title", s);
                    }
                }
            }
        }

        private void SchemeFilteration(int AMCID = -1, int FundID = -1)
        {
            DataTable dtResult = new DataTable();

            if (Cache["dtSchemeAllMaster"] == null)
            {
                dtResult = AllMethods.getSebiSchemeBluechip(-1, -1, -1, 2, -1, false);
                Cache.Add("dtSchemeAllMaster", dtResult, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtResult = (DataTable)Cache["dtSchemeAllMaster"];
            }

            dtResult = dtResult.AsEnumerable().Where(x => x["MUTUALFUND_ID"] != DBNull.Value && !string.IsNullOrEmpty(x["MUTUALFUND_ID"].ToString())).CopyToDataTable();

            if (AMCID != -1)
            {
                dtResult = dtResult.AsEnumerable().Where(x => x["MUTUALFUND_ID"].ToString() == AMCID.ToString()).CopyToDataTable();
            }
            if (FundID != -1)
            {
                dtResult = dtResult.AsEnumerable().Where(x => x["Fund_ID"].ToString() == FundID.ToString()).CopyToDataTable();
            }


            var drFund = dtResult.NewRow();
            drFund["Sch_Short_Name"] = "-Select Scheme-";
            drFund["Scheme_Id"] = -1;
            dtResult.Rows.InsertAt(drFund, 0);
            ddlSchemes.DataSource = dtResult;
            ddlSchemes.DataTextField = "Sch_Short_Name";
            ddlSchemes.DataValueField = "Scheme_Id";
            ddlSchemes.DataBind();

        }
    }
}