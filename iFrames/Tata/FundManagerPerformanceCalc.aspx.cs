using iFrames.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection.Emit;
using System.Threading;
using System.Reflection;
using System.Web.Script.Serialization;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.IO;
using System.Net;
using System.Text;
using NPOI.XSSF.UserModel;
namespace iFrames.Tata
{
    public partial class FundManagerPerformanceCalc : System.Web.UI.Page
    { 
     
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
            if (!IsPostBack)
            {
                LoadNatureNOption();
                LoadSchemes(-1, -1);
                Fillyear(DateTime.Now.Year);
              
            }
        }
      
        protected void LoadNatureNOption()
        {
            DataTable _dt = AllMethods.getNature();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("All", "-1"));
            foreach (DataRow drow in _dt.Rows)
            {
                ddlCategory.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }
            ddlCategory.Items[0].Selected = true;

            //getOption

            DataTable _dtOption = AllMethods.getOption();
            ddlOption.Items.Clear();
            ddlOption.Items.Add(new ListItem("All", "-1"));

            foreach (DataRow drow in _dtOption.Rows)
            {
                ddlOption.Items.Add(new ListItem(drow[1].ToString(), drow[0].ToString()));
            }
            ddlOption.Items[0].Selected = true;
        }

        public void LoadSchemes(int CategoryId, int OptionId)
        {
            ddlScheme.Items.Clear();
            ddlScheme.Items.Add(new ListItem("Select scheme", "-1"));
            DataTable _dt = AllMethods.getTataSchemes(CategoryId, OptionId);
            if (_dt == null)
                return;
           
            foreach (DataRow drow in _dt.Rows)
            {
                ddlScheme.Items.Add(new ListItem(drow[0].ToString(), drow[1].ToString()));
            }
            ddlScheme.Items[0].Selected = true;
            ddlBenchMark.Items.Clear();
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSchemes(Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlOption.SelectedValue));
        }
        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSchemes(Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlOption.SelectedValue));
        }
        protected void ddlScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillBenchMarkFM();
        }

        protected void FillBenchMarkFM()
        {
            string selected_Scheme = ddlScheme.SelectedValue.ToString();

            if (selected_Scheme == "9999999991")
                selected_Scheme = "8698";
            else if (selected_Scheme == "9999999992")
                selected_Scheme = "8698";


            if (selected_Scheme == "-1")
            {
                ddlBenchMark.Items.Clear();
                return;
            }
            using (var tata = new iFrames.DAL.TataCalculatorDataContext())
            {
                var index_name = (from t_index_masters in tata.T_INDEX_MASTER_tatas
                                  where
                                      ((from t_schemes_indexes in tata.T_SCHEMES_INDEX_tatas
                                        where
                                          t_schemes_indexes.SCHEME_ID == Convert.ToDecimal(selected_Scheme)
                                        select t_schemes_indexes.INDEX_ID
                                        ).Contains(t_index_masters.INDEX_ID))
                                  select new
                                  {

                                      t_index_masters.INDEX_NAME,
                                      t_index_masters.INDEX_ID
                                  });

                ddlBenchMark.Items.Clear();
                if (index_name.Count() == 1)
                {
                    ddlBenchMark.Items.Add(new ListItem(index_name.Single().INDEX_NAME.ToString(), index_name.Single().INDEX_ID.ToString()));
                }
                else if (index_name.Count() > 1)
                {
                    foreach (var v in index_name)
                    {
                        ddlBenchMark.Items.Add(new ListItem(v.INDEX_NAME.ToString(), v.INDEX_ID.ToString()));
                    }
                }
            }
        }

        protected void Fillyear(int yr)
        {

            ///year
            int year = yr;//DateTime.Today.Year; // change
            int year1 = DateTime.Today.Year; // change
            int month = DateTime.Today.Month;
            ddlYearEnd.Items.Clear();
            for (int i = year1 - 1, k = 0; i < year1 + 1; i++, k++)
            {
                ddlYearEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
               // if (i == year1)
               //     ddlYearEnd.Items[k].Selected = true;
            }
            if (yr == year1)
                ddlYearEnd.Items[1].Selected = true;
            else
                ddlYearEnd.Items[0].Selected = true;

            ddlQtrEnd.Items.Clear();
            ddlQtrEnd.Items.Add(new ListItem("31st January", "01-31-"));
            if(year%4==0)            
            ddlQtrEnd.Items.Add(new ListItem("29th February", "02-29-"));
            else
                ddlQtrEnd.Items.Add(new ListItem("28th February", "02-28-"));
            ddlQtrEnd.Items.Add(new ListItem("31st March", "03-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th April", "04-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st May", "05-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th June", "06-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st July", "07-31-"));
            ddlQtrEnd.Items.Add(new ListItem("31st August", "08-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th September", "09-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st October", "10-31-"));
            ddlQtrEnd.Items.Add(new ListItem("30th November", "11-30-"));
            ddlQtrEnd.Items.Add(new ListItem("31st December", "12-31-"));

            if (month != 0)
            {
              //  if (month > 9)
              //      ddlQtrEnd.Items[2].Selected = true;
              //  if (month > 6 && month < 10)
              //      ddlQtrEnd.Items[1].Selected = true;
              //  if (month > 3 && month < 7)
              //      ddlQtrEnd.Items[0].Selected = true;
              //  if (month > 1 && month < 4)
              //      ddlQtrEnd.Items[3].Selected = true;
                //add new 
                if (yr != year1)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        ddlQtrEnd.Items[i-1].Enabled = true;
                    }


                    if (month == 2)
                        ddlQtrEnd.Items[1].Selected = true;
                    else if (month == 3)
                        ddlQtrEnd.Items[2].Selected = true;
                    else if (month == 4)
                        ddlQtrEnd.Items[3].Selected = true;
                    else if (month == 5)
                        ddlQtrEnd.Items[4].Selected = true;
                    else if (month == 6)
                        ddlQtrEnd.Items[5].Selected = true;
                    else if (month == 7)
                        ddlQtrEnd.Items[6].Selected = true;
                    else if (month == 8)
                        ddlQtrEnd.Items[7].Selected = true;
                    else if (month == 9)
                        ddlQtrEnd.Items[8].Selected = true;
                    else if (month == 10)
                        ddlQtrEnd.Items[9].Selected = true;
                    else if (month == 11)
                        ddlQtrEnd.Items[10].Selected = true;
                    else if (month == 12)
                        ddlQtrEnd.Items[11].Selected = true;
                    else if (month == 1)
                        ddlQtrEnd.Items[0].Selected = true;
                }
                else
                {
                    if (month == 2)
                        ddlQtrEnd.Items[1].Selected = true;
                    else if (month == 3)
                        ddlQtrEnd.Items[2].Selected = true;
                    else if (month == 4)
                        ddlQtrEnd.Items[3].Selected = true;
                    else if (month == 5)
                        ddlQtrEnd.Items[4].Selected = true;
                    else if (month == 6)
                        ddlQtrEnd.Items[5].Selected = true;
                    else if (month == 7)
                        ddlQtrEnd.Items[6].Selected = true;
                    else if (month == 8)
                        ddlQtrEnd.Items[7].Selected = true;
                    else if (month == 9)
                        ddlQtrEnd.Items[8].Selected = true;
                    else if (month == 10)
                        ddlQtrEnd.Items[9].Selected = true;
                    else if (month == 11)
                        ddlQtrEnd.Items[10].Selected = true;
                    else if (month == 12)
                        ddlQtrEnd.Items[11].Selected = true;
                    else if (month == 1)
                        ddlQtrEnd.Items[0].Selected = true;

                    for (int i = month + 1; i <= 12; i++)
                    {
                        ddlQtrEnd.Items[i-1].Enabled = false;
                    }

                   
                }

            }

          // if (month == 1)
          // {
          //     ddlYearEnd.Items[ddlYearEnd.SelectedIndex].Selected = false;
          //     ddlQtrEnd.Items[0].Selected = true;//              
          //     ddlYearEnd.Items.FindByValue((year - 1).ToString()).Selected = true;
          // }
        }

        protected void ddlYearEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fillyear(Convert.ToInt32(ddlYearEnd.SelectedValue));
        }

        //[System.Web.Services.WebMethod]
        //public static string  ShowSchemeWiseReport(int schemeId)
        //{
        //    var dd = AllMethods.GetTataSchemeSEBIReturn(1, "12", new DateTime(2017, 2, 28));

        //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        //    List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();

        //    //Dictionary<string, object> parentRow = new Dictionary<string, object>();
        //    List<string> DS = new List<string>();
        //    if (dd.Rows.Count > 0)
        //        foreach (DataColumn col in dd.Columns)
        //            DS.Add(col.ColumnName);

        //    Dictionary<string, object> childRow;
        //    foreach (DataRow row in dd.Rows)
        //    {
        //        childRow = new Dictionary<string, object>();
        //        foreach (DataColumn col in dd.Columns)
        //        {
        //            childRow.Add(col.ColumnName, row[col]);
        //        }
        //        parentRow.Add(childRow);

        //    }

        //    DataStructure DSObj = new DataStructure();
        //    DSObj.PageDataStructure = DS;
        //    DSObj.PageData = parentRow;
        //    return jsSerializer.Serialize(DSObj);


        //    //return AllMethods.GetTataSchemeSEBIReturn(1, "12", new DateTime(2017, 2, 28));


        //}


        [System.Web.Services.WebMethod]
        public static string ShowSchemeWiseReport(int schemeId, int Year, string Day)
        {
           
            DateTime ReturnDate = new DateTime(Year, Convert.ToInt32(Convert.ToString(Day.Split('-')[0])), Convert.ToInt32(Convert.ToString(Day.Split('-')[1])));
            var DataAll = AllMethods.GetTataSchemeSEBIReturn(schemeId, "12", ReturnDate);
            if (DataAll == null)
                return null;

            var ReturnData = DataAll.DtData;

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<List<string>> parentRow = new List<List<string>>();

            List<string> DS = new List<string>();
            if (ReturnData.Rows.Count > 0)
                foreach (DataColumn col in ReturnData.Columns)
                    DS.Add(col.ColumnName);

            foreach (DataRow row in ReturnData.Rows)
            {
                List<string> childRow = new List<string>();

                //childRow = new List<string, object>();
                foreach (DataColumn col in ReturnData.Columns)
                {
                    childRow.Add(Convert.ToString(row[col]));
                }
                parentRow.Add(childRow);
            }

            DataStructure DSObj = new DataStructure();
            DSObj.PageDataStructure = DS;
            DSObj.PageData = parentRow;
            DSObj.PageDataReturnHeader = DataAll.ReturnColumn;
            return jsSerializer.Serialize(DSObj);
        }
    
        [System.Web.Services.WebMethod]
        public static string ShowFundManagerSchemeWiseReport(int schemeId, int Year, string Day)
        {
            DateTime ReturnDate = new DateTime(Year, Convert.ToInt32(Convert.ToString(Day.Split('-')[0])), Convert.ToInt32(Convert.ToString(Day.Split('-')[1])));

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<FundManagerDataStructure> LstDataStructure = new List<FundManagerDataStructure>();
            var LstFM = AllMethods.GetTataFundManager(schemeId);

            foreach (FundManager Fm in LstFM)
            {
                var LstFMScheme = AllMethods.GetTataManagerScheme(Fm.MFId);
                LstFMScheme.Remove(schemeId);
                FundManagerDataStructure ObjFundManagerDataStructure = new FundManagerDataStructure();
                List<DataStructure> LstSchemeStructure = new List<DataStructure>();
                foreach (int i in LstFMScheme)
                {
                    var dd = AllMethods.GetTataSchemeSEBIReturn(i, "12", ReturnDate);
                    if (dd == null) continue;
                    var ReturnData = dd.DtData;
                    List<List<string>> parentRow = new List<List<string>>();

                    List<string> DS = new List<string>();
                    if (ReturnData.Rows.Count > 0)
                        foreach (DataColumn col in ReturnData.Columns)
                            DS.Add(col.ColumnName);

                    foreach (DataRow row in ReturnData.Rows)
                    {
                        List<string> childRow = new List<string>();
                        foreach (DataColumn col in ReturnData.Columns)
                        {
                            childRow.Add(Convert.ToString(row[col]));
                        }
                        parentRow.Add(childRow);
                    }

                    DataStructure DSObj = new DataStructure();
                    DSObj.PageDataStructure = DS;
                    DSObj.PageData = parentRow;
                    DSObj.PageDataReturnHeader = dd.ReturnColumn;
                    LstSchemeStructure.Add(DSObj);
                }
                ObjFundManagerDataStructure.LstDataStructure = LstSchemeStructure;
                ObjFundManagerDataStructure.FundManagerName = Fm.MFName;
                LstDataStructure.Add(ObjFundManagerDataStructure);
            }
            return jsSerializer.Serialize(LstDataStructure);
        }
        
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(HdnYearEnd.Value.Trim());
            string day =HdnQtrEnd.Value.Trim();
            int schemeId = Convert.ToInt32(HdnScheme.Value.Trim());

            ExportToExcel(schemeId, year, day);
        }
     
       // [System.Web.Services.WebMethod]
        public void ExportToExcel(int schemeId, int Year, string Day)
        {
            try
            {
                DateTime ReturnDate = new DateTime(Year, Convert.ToInt32(Convert.ToString(Day.Split('-')[0])), Convert.ToInt32(Convert.ToString(Day.Split('-')[1])));
                var DataAll = AllMethods.GetTataSchemeSEBIReturn(schemeId, "12", ReturnDate);
                if (DataAll == null)
                    return ;

                var ReturnData = DataAll.DtData;

                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                List<List<string>> parentRow = new List<List<string>>();

                List<string> DS = new List<string>();
                if (ReturnData.Rows.Count > 0)
                    foreach (DataColumn col in ReturnData.Columns)
                        DS.Add(col.ColumnName);

                foreach (DataRow row in ReturnData.Rows)
                {
                    List<string> childRow = new List<string>();

                    //childRow = new List<string, object>();
                    foreach (DataColumn col in ReturnData.Columns)
                    {
                        childRow.Add(Convert.ToString(row[col]));
                    }
                    parentRow.Add(childRow);
                }

                DataStructure DSObj = new DataStructure();
                DSObj.PageDataStructure = DS;
                DSObj.PageData = parentRow;
                DSObj.PageDataReturnHeader = DataAll.ReturnColumn;



                DateTime ReturnDateLST = new DateTime(Year, Convert.ToInt32(Convert.ToString(Day.Split('-')[0])), Convert.ToInt32(Convert.ToString(Day.Split('-')[1])));

               
                List<FundManagerDataStructure> LstDataStructure = new List<FundManagerDataStructure>();
                var LstFM = AllMethods.GetTataFundManager(schemeId);
                FundManagerDataStructure ObjFundManagerDataStructure = null;
                foreach (FundManager Fm in LstFM)
                {
                    var LstFMScheme = AllMethods.GetTataManagerScheme(Fm.MFId);
                    LstFMScheme.Remove(schemeId);
                    ObjFundManagerDataStructure = new FundManagerDataStructure();
                    List<DataStructure> LstSchemeStructure = new List<DataStructure>();
                    foreach (int i in LstFMScheme)
                    {
                        var dd = AllMethods.GetTataSchemeSEBIReturn(i, "12", ReturnDateLST);
                        var ReturnDataLST = dd.DtData;
                        List<List<string>> parentRowLST = new List<List<string>>();

                        List<string> DSLST = new List<string>();
                        if (ReturnDataLST.Rows.Count > 0)
                            foreach (DataColumn col in ReturnDataLST.Columns)
                                DSLST.Add(col.ColumnName);

                        foreach (DataRow row in ReturnDataLST.Rows)
                        {
                            List<string> childRow = new List<string>();
                            foreach (DataColumn col in ReturnDataLST.Columns)
                            {
                                childRow.Add(Convert.ToString(row[col]));
                            }
                            parentRowLST.Add(childRow);
                        }

                        DataStructure DSObjLST = new DataStructure();
                        DSObjLST.PageDataStructure = DSLST;
                        DSObjLST.PageData = parentRowLST;
                        DSObjLST.PageDataReturnHeader = dd.ReturnColumn;
                        LstSchemeStructure.Add(DSObjLST);
                    }
                    ObjFundManagerDataStructure.LstDataStructure = LstSchemeStructure;
                    ObjFundManagerDataStructure.FundManagerName = Fm.MFName;
                    LstDataStructure.Add(ObjFundManagerDataStructure);
                }
                string FilePath = Server.MapPath(@"~/Images/tata.gif");
                XSSFWorkbook workbook = AllMethods.ExportToExcelWrite(DSObj, FilePath, LstDataStructure);
                using (var exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "TataFundManegerExport.xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End(); 
                }

              
             
            }
            catch (Exception p) { p.ToString(); }

           
        }

     
    }
   
}
    
