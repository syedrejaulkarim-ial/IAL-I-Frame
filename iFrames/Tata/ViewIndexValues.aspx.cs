using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace iFrames.Tata
{
    public partial class ViewIndexValues : System.Web.UI.Page
    {
        #region: Global Variable

        // string connstr = ConfigurationManager.ConnectionStrings["TestcsIcraclient"].ConnectionString;
        string connstr = ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;
        SqlConnection conn = new SqlConnection();

        #endregion

        #region: Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userid"] != null && Session["password"] != null)
            {
                lnlLogout.Visible = true;
                if (!IsPostBack)
                {
                    FillDropdownIndex();
                }
            }
            else
            {
                Response.Redirect("TataUploadExcel.aspx");
                lnlLogout.Visible = false;
            }


        }

        #endregion

        #region: Datagid Events

        protected void gvCompanyShare_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        #endregion

        #region: Button Events

        protected void btnShow_Click(object sender, EventArgs e)
        {
            FetchIndexValues();
        }

        #endregion

        #region: Link Button Event

        protected void lnlLogout_Click(object sender, EventArgs e)
        {
            SessionClear();
            Response.Redirect("TataUploadExcel.aspx");
        }

        #endregion

        #region: Methods

        #region: Annonymous Method

        public string FomatDate(object date)
        {
            string res = string.Empty;
            try
            {
                res = Convert.ToDateTime(date).ToString("dd/MM/yyyy");

            }
            catch (Exception ex)
            {


            }
            return res;
        }
        private void SessionClear()
        {
            Session["userid"] = null;
            Session["password"] = null;
            Session.Abandon();
        }

        #endregion

        #region: Fill Methods
        public void FillDropdownIndex()
        {
            FillDropdown(ddlIndex);

        }
        #endregion

        #region: DropDown Method

        public void FillDropdown(Control ddl)
        {
            try
            {
                DataTable dt = FetchIndex();
                if (dt.Rows.Count > 0)
                {
                    DropDownList drpdwn = (DropDownList)ddl;
                    drpdwn.DataSource = dt;
                    drpdwn.DataTextField = "CUSTOM_INDEX_NAME";
                    drpdwn.DataValueField = "CUSTOM_INDEX_ID";
                    drpdwn.DataBind();
                    drpdwn.Items.Insert(0, new ListItem("-Select Index-", "0"));
                    drpdwn.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {


            }
        }

        #endregion

        #region: Fetch Methods

        public DataTable FetchIndex()
        {
            conn.ConnectionString = connstr;
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Empty;
                sql = "select CUSTOM_INDEX_ID,CUSTOM_INDEX_NAME from T_MFIE_CUSTOM_INDEX_MASTER";
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public void FetchIndexValues()
        {
            DataTable dt = new DataTable();
            lblMsg.Text = string.Empty;
            try
            {
                conn.ConnectionString = connstr;
                string sql = string.Empty;
                string start_record_date = string.Empty;
                string End_record_date = string.Empty;

                if (txtStarttDate.Text != "DD-MMM-YYYY" && txtEndDate.Text != "DD-MMM-YYYY")
                {
                    start_record_date = Convert.ToDateTime(txtStarttDate.Text).ToString("dd-MM-yyyy");
                    End_record_date = Convert.ToDateTime(txtEndDate.Text).ToString("dd-MM-yyyy");
                    sql = "select CUSTOM_INDEX_ID, RECORD_DATE,CUSTOM_INDEX_VALUE,LOGIN_ID from T_MFIE_CUSTOM_INDEX_RECORDS where CUSTOM_INDEX_ID='" + ddlIndex.SelectedItem.Value.ToString() + "' and RECORD_DATE >=convert(datetime,'" + start_record_date + "',103) and  RECORD_DATE <= convert(datetime,'" + End_record_date + "',103) order by RECORD_DATE desc";
                }
                //else if (txtStarttDate.Text != "DD-MMM-YYYY" && txtEndDate.Text == "DD-MMM-YYYY")
                //{
                //    start_record_date = Convert.ToDateTime(txtStarttDate.Text).ToString("dd-MM-yyyy");
                //    sql = "select CUSTOM_INDEX_ID, RECORD_DATE,CUSTOM_INDEX_VALUE,LOGIN_ID from T_MFIE_CUSTOM_INDEX_RECORDS where CUSTOM_INDEX_ID='" + ddlIndex.SelectedItem.Value.ToString() + "' and RECORD_DATE >=convert(datetime,'" + start_record_date + "',103) order by RECORD_DATE desc";
                //}
                //else if (txtStarttDate.Text == "DD-MMM-YYYY" && txtEndDate.Text != "DD-MMM-YYYY")
                //{
                //    End_record_date = Convert.ToDateTime(txtEndDate.Text).ToString("dd-MM-yyyy");
                //    sql = "select CUSTOM_INDEX_ID, RECORD_DATE,CUSTOM_INDEX_VALUE,LOGIN_ID from T_MFIE_CUSTOM_INDEX_RECORDS where CUSTOM_INDEX_ID='" + ddlIndex.SelectedItem.Value.ToString() + "' and  RECORD_DATE <= convert(datetime,'" + End_record_date + "',103) order by RECORD_DATE desc";
                //}
                else
                {
                    sql = "select CUSTOM_INDEX_ID, RECORD_DATE,CUSTOM_INDEX_VALUE,LOGIN_ID from T_MFIE_CUSTOM_INDEX_RECORDS where CUSTOM_INDEX_ID='" + ddlIndex.SelectedItem.Value.ToString() + "' order by RECORD_DATE desc";
                }

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    gvIndexValue.Visible = true;
                    gvIndexValue.DataSource = dt;
                    gvIndexValue.DataBind();
                    pnlList.Visible = true;
                }
                else
                {
                    gvIndexValue.Visible = false;
                    lblMsg.Text = "There is no record.";
                    pnlList.Visible = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion



        #endregion
    }
}