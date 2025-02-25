using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using iFrames.DAL;

namespace iFrames
{
    public partial class msRankings : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var minYear = Request.QueryString["MinYear"];
            //if (minYear == null)
            //    minYear = "2012";
            //else
            //{
            //    if (Convert.ToInt32(minYear) < 2012)
            //        minYear = "2012";
            //    }

            //var period = Request.QueryString["rkperiod"];

            ////added for test purpose
            //if (period == null)
            //    period = "35";
            //else
            //{
            //    if (Convert.ToInt32(period) < 35)
            //        period = "35";
            //}

            var minYear = Request.QueryString["MinYear"];
            if (minYear == null)
                minYear = "2012";
            else
            {
                if (Convert.ToInt32(minYear) < 2012)
                    minYear = "2012";
            }

            var period = Request.QueryString["rkperiod"];

            //added for test purpose
            if (period == null)
                period = "35";
            else
            {
                if (Convert.ToInt32(period) < 35)
                    period = "35";
            }


            divDebt1.Controls.Clear();
            divDebt3.Controls.Clear();
            divEquity3.Controls.Clear();
            divEquity1.Controls.Clear();
            divHybrid1.Controls.Clear();
            divHybrid3.Controls.Clear();
            foreach (DataRow item in GetLinks(period, 1, "Debt").Rows)
            {
                var lnk = new LinkButton
                              {
                                  Text = item["newcategory"].ToString(),
                                  PostBackUrl = "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] + "~" + "1" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                              };
                divDebt1.Controls.Add(lnk);
                divDebt1.Controls.Add(new Literal { Text = "<br/>" });
            }
            foreach (DataRow item in GetLinks(period, 3, "Debt").Rows)
            {
                var lnk3 = new LinkButton
                               {
                                   Text = item["newcategory"].ToString(),
                                   PostBackUrl = "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] + "~" + "3" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                               };
                divDebt3.Controls.Add(lnk3);
                divDebt3.Controls.Add(new Literal { Text = "<br/>" });
            }
            foreach (DataRow item in GetLinks(period, 3, "Equity").Rows)
            {
                var lnk3 = new LinkButton
                               {
                                   Text = item["newcategory"].ToString(),
                                   PostBackUrl = "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] + "~" + "3" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                               };
                divEquity3.Controls.Add(lnk3);
                divEquity3.Controls.Add(new Literal { Text = "<br/>" });
            }
            foreach (DataRow item in GetLinks(period, 1, "Equity").Rows)
            {
                var lnk = new LinkButton
                              {
                                  Text = item["newcategory"].ToString(),
                                  PostBackUrl = "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] + "~" + "1" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                              };
                divEquity1.Controls.Add(lnk);
                divEquity1.Controls.Add(new Literal { Text = "<br/>" });
            }
            foreach (DataRow item in GetLinks(period, 3, "Hybrid").Rows)
            {
                var lnk3 = new LinkButton
                               {
                                   Text = item["newcategory"].ToString(),
                                   PostBackUrl = "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] + "~" + "3" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                               };
                divHybrid3.Controls.Add(lnk3);
                divHybrid3.Controls.Add(new Literal { Text = "<br/>" });
            }
            foreach (DataRow item in GetLinks(period, 1, "Hybrid").Rows)
            {
                var lnk = new LinkButton
                              {
                                  Text = item["newcategory"].ToString(),
                                  PostBackUrl = "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] + "~" + "1" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                              };
                divHybrid1.Controls.Add(lnk);
                divHybrid1.Controls.Add(new Literal { Text = "<br/>" });
            }

            if (IsPostBack) return;
            try
            {
                using (var dal = new RankingsDataContext())
                {
                    if (string.IsNullOrEmpty(minYear))
                    {
                        //var data = dal.rankperiods.Select(f => new { f.id, f.rkperiod }).Take(1).OrderByDescending(f => f.id).ToDataTable();
                        var data = dal.rankperiods.Select(f => new { f.id, f.rkperiod }).Take(1).OrderBy(f => f.id).ToDataTable();
                        //ddlPeriods.DataSource = data;
                        //ddlPeriods.DataValueField = "id";
                        //ddlPeriods.DataTextField = "rkperiod";
                        //ddlPeriods.DataBind();
                        if(data.Rows.Count>0)
                        {
                            lblPeriod.Text = Convert.ToString(data.Rows[0][1]);
                            lblPeriodId.Text = Convert.ToString(data.Rows[0][0]);
                            ViewState["rkperiodId"] = Convert.ToString(data.Rows[0][0]);
                        }
                    }
                    else
                    {
                        //var data = dal.rankperiods.Where(p => Convert.ToInt32(p.actualdate.Substring(p.actualdate.LastIndexOf("/") + 1)) >= Convert.ToInt32(minYear))
                        //        .Select(f => new { f.id, f.rkperiod }).OrderByDescending(f => f.id).Take(1).ToDataTable();
                        var data = dal.rankperiods.Where(p => Convert.ToInt32(p.actualdate.Substring(p.actualdate.LastIndexOf("/") + 1)) >= Convert.ToInt32(minYear))
                                .Select(f => new { f.id, f.rkperiod }).OrderByDescending(f => f.id).Take(1).ToDataTable();


                        //ddlPeriods.DataSource = data;
                        //ddlPeriods.DataValueField = "id";
                        //ddlPeriods.DataTextField = "rkperiod";
                        //ddlPeriods.DataBind();
                        if (data.Rows.Count > 0)
                        {
                            lblPeriod.Text = Convert.ToString(data.Rows[0][1]);
                            lblPeriodId.Text = Convert.ToString(data.Rows[0][0]);
                            ViewState["rkperiodId"] = Convert.ToString(data.Rows[0][0]);
                            period = Convert.ToString(ViewState["rkperiodId"]);
                        }
                    }
                    //if (!string.IsNullOrEmpty(period))
                    //    ddlPeriods.SelectedValue = period;
                }
            }
            catch (Exception ex)
            {
            }
        }


        //not using
        protected void ddlPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var period = ddlPeriods.SelectedItem.Value;
            var period = ViewState["rkperiodId"];
            Response.Redirect("~/Rankings/RankingIndex.aspx?rkperiod=" + period);
        }

#region old code

        //if (ddlPeriods.SelectedItem == null) return;
            //if (DateTime.Parse(ddlPeriods.SelectedItem.Text) < DateTime.Parse("28 Feb 2011"))
            //    Page.ClientScript.RegisterStartupScript(GetType(), "Load",
            //                                            "<script type='text/javascript'>window.parent.location.href = '" +
            //                                            "http://www.mutualfundsindia.com/aindex.asp?rkperiod=" +
            //                                            ddlPeriods.SelectedItem.Value + "'; </script>");
            //else
            //{
                //divDebt1.Controls.Clear();
                //divDebt3.Controls.Clear();
                //divEquity3.Controls.Clear();
                //divEquity1.Controls.Clear();
                //divHybrid1.Controls.Clear();
                //divHybrid3.Controls.Clear();
                //var minYear = Request.QueryString["MinYear"];
                
                //foreach (DataRow item in GetLinks(period, 1, "Debt").Rows)
                //{
                //    var lnk = new LinkButton
                //                  {
                //                      Text = item["newcategory"].ToString(),
                //                      PostBackUrl =
                //                          "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] +
                //                          "~" + "1" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                //                  };
                //    divDebt1.Controls.Add(lnk);
                //    divDebt1.Controls.Add(new Literal {Text = "<br/>"});
                //}
                //foreach (DataRow item in GetLinks(period, 3, "Debt").Rows)
                //{
                //    var lnk3 = new LinkButton
                //                   {
                //                       Text = item["newcategory"].ToString(),
                //                       PostBackUrl =
                //                           "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] +
                //                           "~" + "3" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                //                   };
                //    divDebt3.Controls.Add(lnk3);
                //    divDebt3.Controls.Add(new Literal {Text = "<br/>"});
                //}
                //foreach (DataRow item in GetLinks(period, 3, "Equity").Rows)
                //{
                //    var lnk3 = new LinkButton
                //                   {
                //                       Text = item["newcategory"].ToString(),
                //                       PostBackUrl =
                //                           "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] +
                //                           "~" + "3" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                //                   };
                //    divEquity3.Controls.Add(lnk3);
                //    divEquity3.Controls.Add(new Literal {Text = "<br/>"});
                //}
                //foreach (DataRow item in GetLinks(period, 1, "Equity").Rows)
                //{
                //    var lnk = new LinkButton
                //                  {
                //                      Text = item["newcategory"].ToString(),
                //                      PostBackUrl =
                //                          "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] +
                //                          "~" + "1" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                //                  };
                //    divEquity1.Controls.Add(lnk);
                //    divEquity1.Controls.Add(new Literal {Text = "<br/>"});
                //}
                //foreach (DataRow item in GetLinks(period, 3, "Hybrid").Rows)
                //{
                //    var lnk3 = new LinkButton
                //                   {
                //                       Text = item["newcategory"].ToString(),
                //                       PostBackUrl =
                //                           "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] +
                //                           "~" + "3" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                //                   };
                //    divHybrid3.Controls.Add(lnk3);
                //    divHybrid3.Controls.Add(new Literal {Text = "<br/>"});
                //}
                //foreach (DataRow item in GetLinks(period, 1, "Hybrid").Rows)
                //{
                //    var lnk = new LinkButton
                //                  {
                //                      Text = item["newcategory"].ToString(),
                //                      PostBackUrl =
                //                          "~/Rankings/DispalyRank.aspx?rkperiod=" + period + "&CatYear=" + item["id"] +
                //                          "~" + "1" + (string.IsNullOrEmpty(minYear) ? "" : "&MinYear=" + minYear)
                //                  };
                //    divHybrid1.Controls.Add(lnk);
                //    divHybrid1.Controls.Add(new Literal {Text = "<br/>"});
                //}
                
            //}
        // }
#endregion

        DataTable GetLinks(string period, int yr, string cat)
        {
            var _period = int.Parse(period);
            using (var dal = new RankingsDataContext())
            {
                var item = dal.categories.Where(f => f.categoryNature == cat &&
                                                     dal.RANKING_PARAMETER_RANKs.Where(g => g.YEAR_CHECK == yr + " Year" && g.QUARTER_ID == _period)
                                                         .Select(g => g.CATEGORY_ID).Distinct().Contains(f.id)
                    ).Select(f => new { f.id, f.newcategory });
                return item.ToDataTable();
            }
        }
    }
}