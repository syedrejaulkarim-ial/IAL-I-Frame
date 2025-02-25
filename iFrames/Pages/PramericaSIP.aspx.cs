using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.IO;
using System.Data.SqlTypes;
using System.Globalization;
using iFrames.DAL;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using iFrames.Pages;
using System.Reflection;
using System.Xml.Linq;
using System.Configuration;

namespace iFrames.Pages
{
    public partial class PramericaSIP : System.Web.UI.Page
    {
        #region: Page Event

        protected void Page_Load(object sender, EventArgs e)
        {

            showSensexDiv.Visible = false;
            showResultDiv.Visible = false;
            compareDiv.Visible = false;
        }

        #endregion
        
        #region: Button Event
        protected void sipCalcBtn_Click(object sender, EventArgs e)
        {

            //string xlpath = UploadXL
            //DateTime Sip_start_date = Convert.ToDateTime(StartDate.Text.ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime start_Date = new DateTime(Convert.ToInt16(StartDate.Text.Split('/')[2]),
                                      Convert.ToInt16(StartDate.Text.Split('/')[1]),
                                      Convert.ToInt16(StartDate.Text.Split('/')[0]));
            DateTime end_Date = new DateTime(Convert.ToInt16(EndDate.Text.Split('/')[2]),
                                      Convert.ToInt16(EndDate.Text.Split('/')[1]),
                                      Convert.ToInt16(EndDate.Text.Split('/')[0]));
            Double amount = Convert.ToDouble(Amount.Text);

            TimeSpan daydiff = end_Date.Subtract(start_Date);
            int daybetween = daydiff.Days;
            Double fdCagr = 0, SensexCagr = 0, sensexVal = 0, fdVal = 0;
            GetDataThruLinq(start_Date, end_Date, amount, ref sensexVal, ref fdVal);

            GetCagrValue(start_Date, end_Date, amount, sensexVal, ref SensexCagr);
            GetCagrValue(start_Date, end_Date, amount, fdVal, ref fdCagr);


            returnSensexCagr.Text = SensexCagr.ToString() + "%";
            returnFdCagr.Text = fdCagr.ToString() + "%";

            //GetDataThruSP(start_Date, end_Date, amount);
            //DataTable sensexRecordTable = new DataTable();
            //sensexRecordTable.Columns.Add("SensexDate", typeof(String));
            //sensexRecordTable.Columns.Add("SensexValue", typeof(String));

            //using (var pramerica = new PramericaDataContext())
            //{
            //    Double sensexValMindate = 0, sensexValMaxdate = 0;
            //    //PrincipalCalcDataContext pc = new PrincipalCalcDataContext();
            //    //var sensexMindateValue = (from indx in pramerica.T_INDEX_RECORD_PRMs
            //    //                     where indx.RECORD_DATE <= start_Date && indx.INDEX_ID == 13
            //    //                     orderby indx.RECORD_DATE descending
            //    //                          select indx.INDEX_VALUE);


            //    var sensexMindateValue = (from indx in pramerica.GetTable<T_INDEX_RECORD>()
            //                              where indx.RECORD_DATE <= start_Date && indx.INDEX_ID == 13
            //                              orderby indx.RECORD_DATE descending
            //                              select indx.INDEX_VALUE);


            //    if (sensexMindateValue.Count() > 0)
            //        sensexValMindate = sensexMindateValue.Take(1).Single();

            //    var sensexMaxdateValue = (from indx in pramerica.T_INDEX_RECORD_PRMs
            //                              where indx.RECORD_DATE <= end_Date && indx.INDEX_ID == 13
            //                              orderby indx.RECORD_DATE descending
            //                              select indx.INDEX_VALUE);
            //    if (sensexMaxdateValue.Count() > 0)
            //        sensexValMaxdate = sensexMaxdateValue.Take(1).Single(); //Single();


            //    //sensexRecordTable.Rows.Add(start_Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), sensexValMindate.ToString());
            //    //sensexRecordTable.Rows.Add(end_Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), sensexValMaxdate.ToString());
            //    investedAmountStartDate.Text = frstSensexdate.Text = start_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    investedAmountLastDate.Text = lastSensexdate.Text = end_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    frstSensexValue.Text = sensexValMindate.ToString();
            //    lastSensexValue.Text = sensexValMaxdate.ToString();

            //}


            investedAmountStartDate.Text = frstSensexdate.Text = start_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            investedAmountLastDate.Text = lastSensexdate.Text = end_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (showSensexDiv.Visible == false)
                showSensexDiv.Visible = true;
            if (showResultDiv.Visible == false)
                showResultDiv.Visible = true;
            if (compareDiv.Visible == false)
                compareDiv.Visible = true;
            if (daybetween < 365)
                showResultDiv.Visible = false;



        }
        #endregion
        
        #region: Without LINQ Not used
        public void GetDataThruSP(DateTime strdate, DateTime enddate, Double amnt)
        {
            Decimal fdValMax = 0, sensexValMax = 0, investedAmount = 0;
            SqlConnection objCon;
            SqlDataAdapter objAdapter;
            SqlCommand cmdCommand;
            DataSet objDataSet = new DataSet();
            DataTable dtble = new DataTable();
            string connectionString = global::System.Configuration.ConfigurationManager.ConnectionStrings["csIcraclient"].ConnectionString;

            XmlDocument doc = new XmlDocument();
            //string path =  Directory.GetCurrentDirectory();
            //string path2 = @"D:\Mukesh\iframe_12_09\iFrames\Images\Excel\Fdreturn.xml";

            string xmlpath = Server.MapPath("~");
            xmlpath += "Images\\Excel\\Fdreturn.xml";
            doc.Load(xmlpath);


            //XmlTextReader reader = new XmlTextReader(path2);
            //reader.Read();
            //doc.Load(reader);
            //SqlXml sqlXml = new SqlXml(reader); 
            //StringWriter sw = new StringWriter();
            //XmlTextWriter xmltxtwrtr = new XmlTextWriter(sw);
            //doc.WriteTo(xmltxtwrtr);



            try
            {
                objCon = new SqlConnection(connectionString);
                cmdCommand = new SqlCommand("MFI_SIP_CALC_PRAMERICA", objCon);
                cmdCommand.CommandType = CommandType.StoredProcedure;
                cmdCommand.Parameters.Add(new SqlParameter("@Plan_Start_Date", strdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdCommand.Parameters.Add(new SqlParameter("@Plan_End_Date", enddate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                cmdCommand.Parameters.Add(new SqlParameter("@SIP_AMT", amnt));
                cmdCommand.Parameters.Add("@fd_sip_xml", SqlDbType.Xml);
                cmdCommand.Parameters["@fd_sip_xml"].Value = new SqlXml(new XmlTextReader(doc.InnerXml, XmlNodeType.Document, null));// pasing xml as a parameter
                //cmdCommand.Parameters.Add(new SqlParameter("@sip_XML", xml));
                //cmdCommand.Parameters.Add(new SqlParameter "@FINAL_RETURN_SENSEX",SqlDbType.Float,30,ParameterDirection.Output,0,0,0,0,0,0));
                SqlParameter parm1 = new SqlParameter("@INVESTED_AMNT", SqlDbType.Decimal);
                //parm1.Size = 50;
                parm1.Direction = ParameterDirection.Output;
                cmdCommand.Parameters.Add(parm1);
                SqlParameter parm2 = new SqlParameter("@FINAL_RETURN_SENSEX", SqlDbType.Decimal);
                //parm2.Size = 50;
                parm2.Direction = ParameterDirection.Output;
                cmdCommand.Parameters.Add(parm2);
                SqlParameter parm3 = new SqlParameter("@FINAL_RETURN_FD", SqlDbType.Decimal);
                parm3.Direction = ParameterDirection.Output;
                cmdCommand.Parameters.Add(parm3);

                objAdapter = new SqlDataAdapter(cmdCommand);
                objAdapter.Fill(objDataSet);


                investedAmount = Convert.ToDecimal(parm1.Value);
                sensexValMax = Convert.ToDecimal(parm2.Value);
                fdValMax = Convert.ToDecimal(parm3.Value);

                investedAmountSx.Text = investedAmountFd.Text = investedAmount.ToString();
                returnAmountSx.Text = sensexValMax.ToString();
                returnAmountFd.Text = fdValMax.ToString();

                //string cmd = @"select *  from T_INDEX_RECORDS where INDEX_ID='13' and record_date <= " + enddate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " and record_date >= " + strdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " order by record_date";
                //cmdCommand2 = new SqlCommand(cmd, objCon);
                //DataTable dtable = new DataTable();

                //objAdapter.SelectCommand = cmdCommand2;
                //objAdapter.Fill(objDataSet);

                dtble = objDataSet.Tables[0];
                PRIndexListView.DataSource = dtble;
                PRIndexListView.DataBind();
                //dtable = objDataSet.Tables[1];

                //Double? cagrVal = null ;
                //double[] daarray  = {4,5,2};
                //DateTime[] datearray = { strdate, enddate };
                //cagrVal = Utilities.XIRR(daarray, datearray);



            }
            catch (Exception ex)
            {

            }
            finally
            {
                //frstSensexdate.Text= strdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                //lastSensexdate.Text = enddate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }


        }
        #endregion
        
        #region: Data retrieved by LINQ

        public void GetDataThruLinq(DateTime strdate, DateTime enddate, Double amnt, ref Double sensexVal, ref Double fdVal)
        {
            decimal? fdValMax = null, sensexValMax = null, investedAmount = null;
            XmlDocument xmldoc = new XmlDocument();
            string xmlpath = Server.MapPath("~");
            xmlpath += "Images\\Excel\\Fdreturn.xml";
            xmldoc.Load(xmlpath);
            SqlXml xmlparameter = new SqlXml(new XmlTextReader(xmldoc.InnerXml, XmlNodeType.Document, null));
            //string s = xmlparameter.Value;
            XElement xmlelement = XElement.Parse(xmlparameter.Value);
            DataTable resultDataTable = new DataTable();


            using (var pramrica = new PramericaDataContext())
            {
                try
                {
                    IMultipleResults dtb1 = pramrica.MFI_SIP_CALC_PRAMERICA(strdate, enddate, Convert.ToDecimal(amnt), xmlelement, ref investedAmount, ref  sensexValMax, ref fdValMax);
                    sensexVal = Convert.ToDouble(sensexValMax);
                    fdVal = Convert.ToDouble(fdValMax);
                    resultDataTable = dtb1.GetResult<SpPramericaReturnData>().ToDataTable();
                    if (resultDataTable.Rows.Count > 0)
                    {
                        PRIndexListView.DataSource = resultDataTable;
                        PRIndexListView.DataBind();
                    }

                    var sensexMindateValue = (from indx in pramrica.GetTable<T_INDEX_RECORD>()
                                              where indx.RECORD_DATE <= strdate && indx.INDEX_ID == 13
                                              orderby indx.RECORD_DATE descending
                                              select indx.INDEX_VALUE);


                    if (sensexMindateValue.Count() > 0)
                        frstSensexValue.Text = sensexMindateValue.Take(1).Single().ToString();
                    else
                        frstSensexValue.Text = "";

                    var sensexMaxdateValue = (from indx in pramrica.T_INDEX_RECORD_PRMs
                                              where indx.RECORD_DATE <= enddate && indx.INDEX_ID == 13
                                              orderby indx.RECORD_DATE descending
                                              select indx.INDEX_VALUE);
                    if (sensexMaxdateValue.Count() > 0)
                        lastSensexValue.Text = sensexMaxdateValue.Take(1).Single().ToString(); //Single();                   
                    else
                        lastSensexValue.Text = "";


                }
                catch (Exception exc)
                {
                }


            }
            investedAmountSx.Text = investedAmountFd.Text = investedAmount.ToString();
            returnAmountSx.Text = sensexValMax.ToString();
            returnAmountFd.Text = fdValMax.ToString();

            // Double? cagrVal = null;
            //// double[] daarray = { 4, 5, 2 };
            // //DateTime[] datearray = { strdate, enddate };
            // var dates = resultDataTable.Rows.Cast<DataRow>().Select(r => Convert.ToDateTime(r["Invested_Date"])).ToArray(); //.ToList();
            // var values = resultDataTable.Rows.Cast<DataRow>().Select(r => Convert.ToDouble(r["INVESTED_AMNT"])).ToArray();//.ToList();
            // cagrVal = Utilities.XIRR(values, dates);
        }
        #endregion

        #region: CAGR Calculation
        public void GetCagrValue(DateTime strdate, DateTime enddate, Double amnt, Double schemeLastValue, ref Double scheme)
        {

            try
            {
                List<DateTime> datearray = new List<DateTime>();
                List<Double> doublearray = new List<Double>();

                while (strdate < enddate)
                {
                    datearray.Add(strdate);
                    doublearray.Add(amnt);
                    //dataTable.Rows.Add(strdate, amnt);
                    strdate = strdate.AddMonths(1);
                }
                schemeLastValue = schemeLastValue * (-1);
                datearray.Add(enddate);
                doublearray.Add(schemeLastValue);
                datearray.Reverse();   //as reversed in utilities.cs
                doublearray.Reverse();
                scheme = Math.Round(Utilities.XIRR(doublearray.ToArray(), datearray.ToArray()) * 100, 2);

            }
            catch (Exception exc)
            {
            }
        }
        #endregion

    }


    #region : Class Defintion
    public class SpPramericaReturnData
    {
        public DateTime INVESTED_DATE { get; set; }
        public decimal? INVESTED_AMNT { get; set; }
        public decimal? SIP_CMPD_AMNT { get; set; }
        public decimal? FD_SIP_CMPD_AMNT { get; set; }
    }
    #endregion

}

namespace iFrames.DAL
{
    public partial class PramericaDataContext : System.Data.Linq.DataContext
    {

        [Function(Name = "dbo.MFI_SIP_CALC_PRAMERICA")]
        [ResultType(typeof(SpPramericaReturnData))]
        public IMultipleResults MFI_SIP_CALC_PRAMERICA([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "Plan_Start_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_Start_Date, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "Plan_End_Date", DbType = "DateTime")] System.Nullable<System.DateTime> plan_End_Date, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "SIP_Amt", DbType = "Decimal(30,10)")] System.Nullable<decimal> sIP_Amt, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FD_sip_XML", DbType = "Xml")] System.Xml.Linq.XElement fD_sip_XML, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "INVESTED_AMNT", DbType = "Decimal(30,2)")] ref System.Nullable<decimal> iNVESTED_AMNT, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FINAL_RETURN_SENSEX", DbType = "Decimal(30,2)")] ref System.Nullable<decimal> fINAL_RETURN_SENSEX, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "FINAL_RETURN_FD", DbType = "Decimal(30,2)")] ref System.Nullable<decimal> fINAL_RETURN_FD)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), plan_Start_Date, plan_End_Date, sIP_Amt, fD_sip_XML, iNVESTED_AMNT, fINAL_RETURN_SENSEX, fINAL_RETURN_FD);
            iNVESTED_AMNT = ((System.Nullable<decimal>)(result.GetParameterValue(4)));
            fINAL_RETURN_SENSEX = ((System.Nullable<decimal>)(result.GetParameterValue(5)));
            fINAL_RETURN_FD = ((System.Nullable<decimal>)(result.GetParameterValue(6)));
            return ((IMultipleResults)(result.ReturnValue));

        }

    }

}

