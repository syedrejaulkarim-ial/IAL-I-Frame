using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iFrames.BLL;
namespace iFrames.Pages
{
    public partial class ComapreFundRpt : MyBasePage
    {
        DataTable dt = new DataTable();
        string sname;
        string exchange = null;
        string columnName = "per_1yr";
        int flag = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            sname = Request.QueryString["sname"].ToString();
            exchange = Request.QueryString["exchange"].ToString();
            if (!IsPostBack)
            {
                ViewState["columnName"] = columnName;
                string sortexpression = "per_1yr DESC";
                bindGridview(sortexpression);
                
                //dt = Nav.GetdtFundComp(sname, exchange);
                //if (dt != null && dt.Rows.Count != 0)
                //{
                //    DataRow[] dr = dt.Select("status=1");
                //    DataTable newdt = dt.Clone();
                //    foreach (DataRow newdr  in dr)
                //    {
                //        newdt.ImportRow(newdr);
                //    }
                //    DataView dataView = new DataView(newdt);                    
                //    dataView.Sort = "per_1yr DESC";
                //    //GrdCompFund.DataSource = dt;
                //   // GrdCompFund.DataBind();                   
                //    DataTable finaldt = dt.Clone();
                //    finaldt=dataView.ToTable();

                //    DataRow[] newaddr = dt.Select("status>1");

                //    foreach(DataRow newddr in newaddr)
                //    {
                //        DataRow newRow = finaldt.NewRow();
                //    //DataRowView newRow = dataView.AddNew();
                //    newRow["short_name"] =newddr["short_name"];
                //    newRow["per_30days"] = newddr["per_30days"];
                //    newRow["per_91days"] = newddr["per_91days"];
                //    newRow["per_182days"] = newddr["per_182days"];
                //    newRow["per_1yr"] = newddr["per_1yr"];
                //    newRow["per_3yr"] = newddr["per_3yr"];
                //    newRow["type1"] = newddr["type1"];
                //    newRow["nature"] = newddr["nature"];
                //    newRow["Nav_Rs"] = newddr["Nav_Rs"];
                //    finaldt.Rows.Add(newRow);
                //    }
                //    GrdCompFund.DataSource = finaldt;
                //    GrdCompFund.DataBind();
                //}
                
            }
        }

       
        private void bindGridview(string sortexpression)
        {
            dt = Nav.GetdtFundComp(sname, exchange);
            if (dt != null && dt.Rows.Count != 0)
            {
                DataRow[] dr = dt.Select("status=1");
                using (DataTable newdt = dt.Clone())
                {
                    foreach (DataRow newdr in dr)
                    {
                        newdt.ImportRow(newdr);
                    }
                    DataView dataView = new DataView(newdt);

                    dataView.Sort = sortexpression;
                    //GrdCompFund.DataSource = dt;
                    // GrdCompFund.DataBind();                   
                    DataTable finaldt = dt.Clone();
                    using (finaldt = dataView.ToTable())
                    {

                        DataRow[] newaddr = dt.Select("status>1");

                        foreach (DataRow newddr in newaddr)
                        {
                            DataRow newRow = finaldt.NewRow();
                            //DataRowView newRow = dataView.AddNew();
                            newRow["short_name"] = newddr["short_name"];
                            newRow["per_30days"] = newddr["per_30days"];
                            newRow["per_91days"] = newddr["per_91days"];
                            newRow["per_182days"] = newddr["per_182days"];
                            newRow["per_1yr"] = newddr["per_1yr"];
                            newRow["per_3yr"] = newddr["per_3yr"];
                            newRow["type1"] = newddr["type1"];
                            newRow["nature"] = newddr["nature"];
                            newRow["Nav_Rs"] = newddr["Nav_Rs"];
                            finaldt.Rows.Add(newRow);
                        }
                        GrdCompFund.DataSource = finaldt;
                        GrdCompFund.DataBind();

                    }
                }
            }
        }

        protected void GrdCompFund_RowCommand(object sender, GridViewCommandEventArgs e)
        {
          
            string sortexpression = null;
            if (e.CommandName == "per_30days")
            {
                sortexpression= "per_30days DESC";
                columnName = "per_30days";
                ViewState["columnName"] = columnName;
                 bindGridview(sortexpression);
                 ViewState.Remove("columnName");
            }
            else if (e.CommandName == "per_91days")
            {
                columnName = "per_91days";
                ViewState["columnName"] = columnName;
                sortexpression = "per_91days DESC";
                bindGridview(sortexpression);
                ViewState.Remove("columnName");
            }
            else if (e.CommandName == "per_182days")
            {
                columnName = "per_182days";
                ViewState["columnName"] = columnName;
                sortexpression = "per_182days DESC";
                bindGridview(sortexpression);
                ViewState.Remove("columnName");
            }
            else if (e.CommandName == "per_1yr")
            {
                columnName = "per_1yr";
                ViewState["columnName"] = columnName;
                sortexpression = "per_1yr DESC";
                bindGridview(sortexpression);
                ViewState.Remove("columnName");
            }
            else if (e.CommandName == "per_3yr")
            {
                columnName = "per_3yr";
                ViewState["columnName"] = columnName;
                sortexpression = "per_3yr DESC";
                 bindGridview(sortexpression);
                 ViewState.Remove("columnName");
            }
        }

        protected void GrdCompFund_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            columnName = ViewState["columnName"].ToString();
            if (flag != 1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    DataRowView drv = (DataRowView)e.Row.DataItem;

                    for (int i = 0; i < drv.DataView.Table.Columns.Count; i++)
                    {
                        //GridViewRow gr = e.Row as GridViewRow;
                        Label lbl = e.Row.FindControl("lblSchName") as Label;
                        if (lbl.Text != "Average performance of similar category funds")
                        {
                            if (drv.DataView.Table.Columns[i].ColumnName.Equals(this.columnName))
                            {

                                e.Row.Cells[i].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e5eef9");
                                // e.Row.Cells[i].BackColor = System.Drawing.Color.LawnGreen;
                            }
                        }
                        else
                        {
                            flag = 1;
                            break;
                        }

                    }

                }
            }
           

        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "xyz", "back();", true);
        }
    }
}