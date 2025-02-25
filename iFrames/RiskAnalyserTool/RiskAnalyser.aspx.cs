using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace iFrames.RiskAnalyserTool
{
    public partial class RiskAnalyser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var pathQueAndAns = HttpRuntime.AppDomainAppPath + "\\RiskAnalyserTool\\RiskAnalyserXML\\RiskAnalyserQueAns.xml";
            var pathParam = HttpRuntime.AppDomainAppPath + "\\RiskAnalyserTool\\RiskAnalyserXML\\RiskAnalyzerParam.xml";

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
                ((Label)FindControl(Rq.QuestionId)).Text = Rq.Question;
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