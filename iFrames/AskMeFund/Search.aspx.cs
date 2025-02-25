using iFrames.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iFrames.AskMeFund
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string[] GetCustomers(string prefix)
        {
            //List<string> customers = new List<string>();
            //customers.Add("test-1");
            //customers.Add("teseeet-3");

            //customers.Add("tessdfrt-7");

            //return customers.ToArray();

            var data = AllMethods.getAutoCompleteScheme(prefix);
            if (data != null)
                return data.ToArray();
            else
                return new string[] { };
        }
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string[] TopSearchScheme(string prefix)
        //{
        //    //List<string> customers = new List<string>();
        //    //customers.Add("test-1");
        //    //customers.Add("teseeet-3");

        //    //customers.Add("tessdfrt-7");

        //    //return customers.ToArray();

        //    var data = AllMethods.getTopSearch(prefix);
        //    if (data != null)
        //        return data.ToArray();
        //    else
        //        return new string[] { };
        //}

    }
}