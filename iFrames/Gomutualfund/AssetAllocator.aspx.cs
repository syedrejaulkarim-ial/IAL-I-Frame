using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace iFrames.Gomutualfund
{
    public partial class AssetAllocator : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
           if (!string.IsNullOrEmpty(Request.QueryString["user_id"]))
           {
               hdUserId.Value = Request.QueryString["user_id"];
           }
           if (!string.IsNullOrEmpty(Request.QueryString["IsCal"]))
           {
               hdIsCal.Value = Request.QueryString["IsCal"];
           }
           if (!string.IsNullOrEmpty(Request.QueryString["CurrentAge"]))
           {
               hdAssetAllocatorAge.Value = Request.QueryString["CurrentAge"];
           }
           if (!string.IsNullOrEmpty(Request.QueryString["RiskAppetite"]))
           {
               hdRiskAppetite.Value = Request.QueryString["RiskAppetite"];
           }
           if (!string.IsNullOrEmpty(Request.QueryString["InvestmentHorizon"]))
           {
               hdInvestmentHorizon.Value = Request.QueryString["InvestmentHorizon"];
           }
           if (!string.IsNullOrEmpty(Request.QueryString["Yourview"]))
           {
               hdYourView.Value = Request.QueryString["Yourview"];
           }

            var pathQueAndAns = HttpRuntime.AppDomainAppPath + "\\RiskAnalyserTool\\RiskAnalyserXML\\AssetAllocatorQueAns.xml";
            var pathParam = HttpRuntime.AppDomainAppPath + "\\RiskAnalyserTool\\RiskAnalyserXML\\AssetAllocationParam.xml";

            var xmlQueAndAnsDoc = XDocument.Load(pathQueAndAns);

            var LstQuestion = (from i in xmlQueAndAnsDoc.Descendants("Question")
                               select new RiskQuestion
                               {
                                   Question = (string)i.Attribute("value"),
                                   QuestionId = (string)i.Attribute("QId"),
                                   AnsId = (string)i.Attribute("AnsId"),
                                   Answers = (from v in i.Elements()
                                              select new RiskAnswer
                                              {
                                                  Answer = v.Value,
                                                  Value = (string)v.Attribute("value")
                                              }).ToList()

                               }).OrderBy(x => x.QuestionId).ToList();

            foreach (RiskQuestion Rq in LstQuestion)
            {
               // ((Label)FindControl(Rq.QuestionId)).Text = Rq.Question;
                if (Rq.AnsId == "RdbAns1") continue;
                var Drop = ((RadioButtonList)FindControl(Rq.AnsId));
                Drop.DataTextField = "Answer";
                Drop.DataValueField = "Value";
                Drop.DataSource = Rq.Answers;
                Drop.DataBind();
            }

            var xParamDoc = XDocument.Load(pathParam);

            var LstParams = (from i in xParamDoc.Descendants("Weightage")
                             select new Weightage
                             {
                                 StartVal = (int)i.Attribute("StartVal"),
                                 EndVal = (int)i.Attribute("EndVal"),
                                 Name = (string)(string)i.Attribute("value"),
                                 Profile = (string)(string)i.Attribute("profile"),
                                 LstAsset = (from v in i.Elements().Where(x => x.Name == "assets").Select(x => x).Elements()
                                             select new Asset
                                             {
                                                 Name = v.Value,
                                                 Value = (string)v.Attribute("allocation")
                                             }).ToList(),
                                 LstNature = (from v in i.Elements().Where(x => x.Name == "natures").Select(x => x).Elements()
                                              select new Nature
                                              {
                                                  Name = v.Value,
                                                  Value = (string)v.Attribute("allocation")
                                              }).ToList()
                             }).ToArray();
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(LstParams);
            hdJsonParam.Value = json;
        }
    }

    public class RiskQuestion
    {
        public RiskQuestion()
        {
            Answers = new List<RiskAnswer>();
        }
        public string Question { get; set; }
        public string QuestionId { get; set; }
        public string AnsId { get; set; }

        public List<RiskAnswer> Answers { get; set; }
    }

    public class RiskAnswer
    {
        public string Answer { get; set; }
        public string Value { get; set; }
    }

    public class Weightage
    {
        public Weightage()
        {
            LstAsset = new List<Asset>();
            LstNature = new List<Nature>();
        }
        public int RiskAppetiteValue { get; set; }
        public int StartVal { get; set; }
        public int EndVal { get; set; }
        public string Name { get; set; }
        public List<Asset> LstAsset { get; set; }
        public List<Nature> LstNature { get; set; }
        public string Profile { get; set; }
    }

    public class Asset
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Nature
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}