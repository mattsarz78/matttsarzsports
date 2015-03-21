using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using New_MSS.Models;

namespace New_MSS.Shared
{
    public class PageHelper : StoredProcHelper
    {
        public static string CheckForFlexSchedule(string year)
        {
            var link = string.Empty;
            var path = HttpContext.Current.Server.MapPath(@"~/Content/FlexScheduleLinks.xml");
            if (File.Exists(path))
            {
                var doc = XDocument.Load(path);
                var xElement = doc.Element("Links").Element("Link" + year);
                if (xElement != null)
                    link = xElement.Value;
            }
            return link;

        }

        public static string GetTextFromXml(string conference, string year)
        {
            var path = HttpContext.Current.Server.MapPath(@"~/Content/ConferenceXml/" + year + ".xml");
            var node = String.Empty;
            if (File.Exists(path))
            {
                var doc = XDocument.Load(path);
                var xElement = doc.Element(Constants.CONFERENCE);
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
            return node;
        }

		public static bool CheckIfBowlWeekOrNIT(int week, List<YearDate> fullYearDates)
    	{
    		return week == fullYearDates.Last().Week;
    	}

		public static bool CheckIfBasketballPostseason(int week, List<YearDate> fullYearDates)
        {
            return week == fullYearDates.Last().Week || week == fullYearDates[fullYearDates.Count - 2].Week;
        }

    }
}