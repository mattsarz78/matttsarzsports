using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MSS.Models;

namespace MSS.Shared
{
    public class PageHelper : IPageHelper
    {
		public PageHelper() {}

        public bool CheckForFlexSchedule(string year)
        {
            var link = false;
            var path = HttpContext.Current.Server.MapPath(@"~/Content/FlexScheduleLinks.xml");
            if (File.Exists(path))
            {
                var xElement = XDocument.Load(path).Element("Links").Element("Link" + year);
                if (xElement != null)
                    link = true;
            }
            return link;
        }

        public string GetFlexScheduleLink(string year)
        {
            var link = string.Empty;
            var path = HttpContext.Current.Server.MapPath(@"~/Content/FlexScheduleLinks.xml");
            if (File.Exists(path))
            {
                var xElement = XDocument.Load(path).Element("Links").Element("Link" + year);
                if (xElement != null)
                    link = xElement.Value;
            }
            return link;
        }
        public List<ContractText> GetTextFromXml(string conference, string year)
        {
        	var confXmlText = new List<ContractText>();
			if (conference.ToLower() == "independents")
			{
				var independentsList = CheckSportYearAttributes(string.Format("football{0}", year), "independents").Split(',').ToList();
				independentsList.Remove("Notre Dame");
				independentsList.Add("NotreDame");
				confXmlText.AddRange(independentsList.Select(independent => GetXmlText(independent, year)));
				return confXmlText;
			}
        	confXmlText.Add(GetXmlText(conference, year));
        	return confXmlText;
        }

		private ContractText GetXmlText(string conference, string year)
        {
            var path = HttpContext.Current.Server.MapPath(@"~/Content/ConferenceXml/" + year + ".xml");
            var node = String.Empty;
            if (File.Exists(path))
            {
                var xElement = XDocument.Load(path).Element(Constants.CONFERENCE);
                if (xElement != null)
                {
                    var element = xElement.Element("Football" + year);
                    if (element != null)
                    {
                        var xElement1 = element.Element(conference + "Div");
                        if (xElement1 != null)
                            node = xElement1.Value;
                    }
                }
            }
            return new ContractText{ Conference = conference, ContractXmlText = node};
        }

        public string CheckSportYearAttributes(string p, string attributeName)
        {
            var path = HttpContext.Current.Server.MapPath(@"~/Content/ValidSportYears.xml");
            return File.Exists(path) ? XDocument.Load(path).Root.Elements(p).Attributes(attributeName).First().Value : string.Empty;
        }
    }
}