using iFrames.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using Saplin.Controls;

namespace iFrames.BlueChip
{
    public partial class NFO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadNature();
                //LoadSubNature();
                //LoadAMC();
                //LoadStructure();
                btnSubmit_Click(null, null);
            }
        }

        protected void LoadNature()
        {

            DataTable _dt = AllMethods.getSebiNature();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("Select All ", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlCategory.Items[0].Selected = true;


            // BIND DATABASE TO SELECT.
            //ddlNature.DataSource = _dt;
            //ddlNature.DataTextField = "Sebi_Nature";
            //ddlNature.DataValueField = "Sebi_Nature_ID";
            //ddlNature.DataBind();


        }

        protected void LoadSubNature(int natureId = 0)
        {

            if (natureId == 0 || natureId == -1)
            {
                DataTable _dt = AllMethods.getAllSebiSubNature();
                ddlSubNature.Items.Clear();
                ddlSubNature.Items.Add(new ListItem("Select All ", "-1"));
                foreach (DataRow drow in _dt.Rows)
                {
                    ddlSubNature.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }

                ddlSubNature.Items[0].Selected = true;

            }
            else
            {
                DataTable _dt = AllMethods.getSebiSubNature(natureId);
                ddlSubNature.Items.Clear();
                ddlSubNature.Items.Add(new ListItem("Select All ", "-1"));
                foreach (DataRow drow in _dt.Rows)
                {
                    ddlSubNature.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
                }

                ddlSubNature.Items[0].Selected = true;
            }

            // BIND DATABASE TO SELECT.
            //ddlSubNature.DataSource = _dt;
            //ddlSubNature.DataTextField = "Sebi_Sub_Nature";
            //ddlSubNature.DataValueField = "Sebi_Sub_Nature_ID";
            //ddlSubNature.DataBind();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nature_id = Convert.ToInt32(ddlCategory.SelectedValue);
            LoadSubNature(nature_id);
        }



        protected void LoadAMC()
        {
            DataTable dtFund;
            dtFund = AllMethods.getFundHouse();
            ddlFundHouse.DataSource = dtFund;
            ddlFundHouse.DataTextField = "MutualFund_Name";
            ddlFundHouse.DataValueField = "MutualFund_ID";
            ddlFundHouse.DataBind();

        }

        protected void LoadStructure()
        {
            ddlFundHouse.ClearSelection();
            DataTable _dt = AllMethods.getStructure();
            ddlType.Items.Clear();
            ddlType.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlType.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }

            ddlType.Items[0].Selected = true;
        }

        protected void lst_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = (sender as ListView).FindControl("dpTopFund") as DataPager;//dpTopFund
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                //DataBind();
                if (hdIsLoad.Value == "1")
                    ResultDataBind();
                else
                    PageLoadDataBind();
            }
        }

        private void GetNfoUpdate()
        {
            try
            {
                //DataTable _dt = AllMethods.getNFOUpdate(Convert.ToInt32(ddlCategory.SelectedItem.Value));

                //if (_dt != null && _dt.Rows.Count > 0)
                //{
                //    _dt = _dt.OrderBy("Nfo_Close_Date desc");
                //}
                //listVwNFODetail.DataSource = _dt;
                //listVwNFODetail.DataBind();

                //if (_dt == null)
                //    resetDp();
                //else
                //    dpNFO.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lstResult.DataSource = null;
            lstResult.DataBind();


            //changed after client changed there's no need of category 
            //var selectedAMC = new List<string>();
            //foreach (ListItem li in ddlFundHouse.Items)
            //{
            //    if (li.Selected)
            //    {
            //        selectedAMC.Add(li.Selected.ToString());
            //    }
            //}


            ResultDataBind();

            var dp = lstResult.FindControl("dpTopFund") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(0, dp.MaximumRows, true);
            }
            lstResult.Visible = true;
            Result.Visible = true;

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //GetNfoUpdate();
            //dpNFO.SetPageProperties(0, dpNFO.MaximumRows, true);

            Response.Redirect("/BlueChip/NFO.aspx");
        }

        private void resetDp()
        {
            //dpNFO.Visible = false;

        }

        #region Fetch Data

        private void PageLoadDataBind()
        {

            DataTable dtResult = new DataTable();

            if (Cache["dtResult"] == null)
            {
                dtResult = AllMethods.getNFOUpdateWithAddedFilter().Tables[1];
                //if (dtResult != null)
                //{
                //    DataColumn rankCol = new DataColumn("Rank", System.Type.GetType("System.Int32"));

                //    dtResult.Columns.Add(rankCol);

                //    int i = 1;
                //    foreach (DataRow dr in dtResult.Rows)
                //    {
                //        dr["Rank"] = i;
                //        i++;
                //    }
                //}
                Cache.Add("dtResult", dtResult, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                dtResult = (DataTable)Cache["dtResult"];
            }


            //lstResult.DataSource = dtResult;
            if (dtResult == null)
            {
                lstResult.DataSource = new DataTable();
            }
            else
            {
                lstResult.DataSource = dtResult;
            }
            lstResult.DataBind();

            //if (dtResult != null)
            //{
            //    lbtopText.Text = ddlRank.SelectedItem.Text + " the schemes on the basis of : " + ddlPeriod.SelectedItem.Text;
            //}
            //else
            //{
            //    lbtopText.Text = "";
            //}
        }

        private void ResultDataBind()
        {
            hdIsLoad.Value = "1";
            //string RankYearChk = System.Configuration.ConfigurationManager.AppSettings["RankYearChk"];

            DataTable dtResult = new DataTable();

            //changed After client removed category filetraion
            //List<string> AMC = new List<string>();
            //foreach (ListItem ID in ddlFundHouse.Items)
            //{
            //    if (ID.Selected)
            //    {
            //        AMC.Add(ID.Value.ToString());
            //    }
            //}

            //DateTime? sipStartDate = new DateTime();
            //DateTime? sipEndDate = new DateTime();

            //if (!string.IsNullOrEmpty(txtfromDate.Text))
            //{
            //    try
            //    {
            //        sipStartDate = new DateTime(Convert.ToInt16(txtfromDate.Text.Split('/')[2]),
            //                                 Convert.ToInt16(txtfromDate.Text.Split('/')[1]),
            //                                 Convert.ToInt16(txtfromDate.Text.Split('/')[0]));
            //    }
            //    catch
            //    {
            //        sipStartDate = null;
            //    }
            //}
            //else
            //{
            //    sipStartDate = null;
            //}

            //if (!string.IsNullOrEmpty(txtToDate.Text))
            //{
            //    try
            //    {
            //        sipEndDate = new DateTime(Convert.ToInt16(txtToDate.Text.Split('/')[2]),
            //                                 Convert.ToInt16(txtToDate.Text.Split('/')[1]),
            //                                 Convert.ToInt16(txtToDate.Text.Split('/')[0]));
            //    }
            //    catch
            //    {
            //        sipEndDate = null;
            //    }
            //}
            //else
            //{
            //    sipEndDate = null;
            //}


            //var dtMast = AllMethods.getNFOUpdateWithAddedFilter(Convert.ToInt32(ddlCategory.SelectedValue), AMC, SubNature: Convert.ToInt32(ddlSubNature.SelectedValue), startDate: sipStartDate, endDate: sipEndDate, structure: Convert.ToInt32(ddlType.SelectedValue));
            var dtMast = AllMethods.getNFOUpdateWithAddedFilter();
            if (dtMast != null)
            {
                dtResult = dtMast.Tables[1];
                var dtMaster = dtMast.Tables[0];
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    lstResult.DataSource = dtResult;
                    lstResult.DataBind();
                }
                else
                {
                    lstResult.DataSource = new DataTable();
                    lstResult.DataBind();
                }

                foreach (var e in lstResult.Items)
                {
                    HiddenField lblRollNo = (HiddenField)e.FindControl("hdSchID");

                    Repeater innerRepeater = (Repeater)e.FindControl("innerRepeater");
                    var dtData = AllMethods.SchemeMinIvest(Convert.ToInt32(lblRollNo.Value));
                    innerRepeater.DataSource = dtData;
                    innerRepeater.DataBind();
                }
            }
            else
            {
                lstResult.DataSource = new DataTable();
                lstResult.DataBind();
            }
        }

        #endregion
    }
}
