using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using MSS.Models;
using System.Collections.Generic;

namespace MSS.Shared
{
    public class Bools : IBools
    {
        public  bool IsImage(string coverageNote)
        {
            return coverageNote.EndsWith("jpg") && !coverageNote.Contains("assets.espn")
                && !coverageNote.Contains("espncdn") && !coverageNote.Contains("espngameplan");
        }

        public  bool IsImageHyperlink(string coverageNote)
        {
			var path = HttpContext.Current.Server.MapPath(@"~/Content/ImagesForURLs.xml");
			return File.Exists(path) && XDocument.Load(path).Root.Elements("Address").Attributes("Link").Any(x => coverageNote.Contains(x.Value));
        }

        public  bool IsTextStreamingLink(string coverageNote)
        {
            return IsNBC(coverageNote) || IsBTN(coverageNote) ||
                IsSyndAffiliates(coverageNote) || IsCoverageMap(coverageNote) || IsGamePlanMap(coverageNote)
                || IsOleMissLHN(coverageNote) || IsPPVProviders(coverageNote) || IsSpecialCoverageNote(coverageNote);
        }

        public  bool IsOleMissLHN(string coverageNote)
        {
            return coverageNote.Contains("http://www.olemisssports.com/sports/m-footbl/spec-rel/071613aab.html");
        }

        public  bool IsPPVProviders(string coverageNote)
        {
            return coverageNote.Contains("http://www.soonersports.com/ViewArticle.dbml?SPSID=750323&SPID=127245&DB_LANG=C&DB_OEM_ID=31000&ATCLID=209609473");
        }

        public  bool IsGamePlanMap(string coverageNote)
        {
			return coverageNote.Contains("http://assets.espn.go.com/gameplan/") || coverageNote.Contains("http://assets.espn.go.com/espn3/") || coverageNote.Contains("espngameplan.espn.com") || coverageNote.Contains("blackout");
        }

        public  bool IsThe506CoverageMap(string coverageNote)
        {
            return coverageNote.Contains("http://www.the506.com");
        }

        public  bool IsCoverageMap(string coverageNote)
        {
	        if (IsCoverageMapLink(coverageNote))
			{
				return true;
			}
	        return coverageNote.Contains("http://assets.espn.go.com/photo/") ||
	               (coverageNote.Contains("espncdn") && !coverageNote.Contains("blackout")) ||
	               coverageNote.Contains("http://www.seminoles.com/blog/Screen%20Shot%202013-11-07%20at%2011.42.17%20AM.png") ||
	               coverageNote.Contains("http://espnmediazone.com/us/files/2013/08/CF_Oct29_Maps_MZ.pdf");
        }

	    private bool IsCoverageMapLink(string coverageNote)
		{
			var path = HttpContext.Current.Server.MapPath(@"~/Content/ABCCoverageMapLinks.xml");
			return File.Exists(path) && XDocument.Load(path).Root.Elements().Any(xItem => xItem.Attribute("link").Value == coverageNote);
		}

		public  bool IsSyndAffiliates(string coverageNote)
        {
            return coverageNote.Contains("acctourney.theacc.com") || coverageNote.Contains("theacc.com/live") || coverageNote.Contains("raycomsports")
                || coverageNote.Contains("SECNetWk") || coverageNote.Contains("Big12NetWk") || (coverageNote.Contains("wacsports") && !coverageNote.ToLower().Contains("wacsports.com/live")) || coverageNote.Contains("theacc.com/news")
                || coverageNote.Contains("BigEastWk") || coverageNote.Contains("http://tinyurl.com/slctvstations") || coverageNote.Contains("theacc.com/sports")|| IsASNLink(coverageNote);
        }

        public  bool IsNBC(string coverageNote)
        {
            return coverageNote.ToLower().Contains("nbcsports.com") && !coverageNote.ToLower().Contains("stream.nbcsports.com");
        }

        public  bool IsBTN(string coverageNote)
        {
            return coverageNote.ToLower().Contains("bigtennetwork.com") || coverageNote.ToLower().Contains("btn.com");
        }

        public  bool IsHyperlink(string coverageNote)
        {
            return coverageNote.ToLower().Contains("http") || coverageNote.ToLower().Contains(".com");
        }

        public  bool IsSpecialCoverageNote(string coverageNote)
        {
            return coverageNote.Contains("http://www.osubeavers.com/ViewArticle.dbml?DB_OEM_ID=30800&ATCLID=209713667");
        }

        public  bool IsP12Networks(string coverageNote)
        {
            return coverageNote.Contains("http://pac-12.com/AboutPac-12Enterprises/ChannelFinder.aspx");
        }

        public  bool CheckXMLDoc(string docName, string element)
        {
            var path = HttpContext.Current.Server.MapPath(@"~/Content/" + docName + ".xml");
            if (File.Exists(path))
            {
                var xElement = XDocument.Load(path).Element(docName).Element(element);
                return xElement != null;
            }
            return false;
        }

        private  bool IsASNLink(string coverageNote)
        {
            var path = HttpContext.Current.Server.MapPath(@"~/Content/ASNLinks.xml");
            return File.Exists(path) && XDocument.Load(path).Root.Elements().Any(xItem => xItem.Attribute("link").Value == coverageNote);
        }

		public  bool CheckSportYearAttributesBool(string p, string attributeName)
		{
			var path = HttpContext.Current.Server.MapPath(@"~/Content/ValidSportYears.xml");
			return File.Exists(path) && XDocument.Load(path).Root.Elements(p).Any(xItem => Convert.ToBoolean(xItem.Attribute(attributeName).Value));
		}

		public  bool IsESPNPPV(string ppv, string coverageNotes)
		{
			return ppv.Contains("gameplan") || coverageNotes.Contains("gameplan") || ppv.Contains("espnfc") || 
				coverageNotes.Contains("espnfc");
		}

        public bool CheckIfBowlWeek(int week, List<YearDate> fullYearDates)
        {
            return week == fullYearDates.Last().Week;
        }

        public bool CheckIfFirstWeek(int week, List<YearDate> fullYearDates)
        {
            return week == fullYearDates.First().Week;
        }

        public bool CheckIfBasketballPostseason(int week, List<YearDate> fullYearDates)
        {
            return fullYearDates.Any(x => x.Week == week
            && (x.PostseasonInd.Contains("N") || x.PostseasonInd.Contains("I") || x.PostseasonInd.Contains("O")));
        }

        public bool CheckIfNIT(int week, List<YearDate> fullYearDates)
        {
            return fullYearDates.Any(x => x.Week == week && x.PostseasonInd.Contains("I"));
        }

        public bool CheckIfOtherMBKTourney(int week, List<YearDate> fullYearDates)
        {
            return fullYearDates.Any(x => x.Week == week && x.PostseasonInd.Contains("O"));
        }
    }
}