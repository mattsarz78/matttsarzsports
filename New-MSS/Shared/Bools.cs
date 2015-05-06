using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Web;

namespace New_MSS.Shared
{
    public class Bools : IBools
    {
        public  bool IsImage(string coverageNote)
        {
            return coverageNote.ToLower().Contains("jpg") && !coverageNote.Contains("assets.espn")
                && !coverageNote.Contains("espncdn") && !coverageNote.Contains("espngameplan");
        }

        public  bool IsImageHyperlink(string coverageNote)
        {
			var path = HttpContext.Current.Server.MapPath(@"~/Content/ImagesForURLs.xml");
			if (File.Exists(path))
			{
				var doc = XDocument.Load(path);
                return doc.Root.Elements("Address").Attributes("Link").Any(x => coverageNote.Contains(x.Value));
			}
        	return false;
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
            return coverageNote.Contains("http://assets.espn.go.com/gameplan/") || coverageNote.Contains("espngameplan.espn.com");
        }

        public  bool IsThe506CoverageMap(string coverageNote)
        {
            return coverageNote.Contains("http://www.the506.com");
        }

        public  bool IsCoverageMap(string coverageNote)
        {
            return coverageNote.Contains("http://assets.espn.go.com/photo/") || 
				coverageNote.Contains("espncdn") || 
				coverageNote.Contains("http://www.seminoles.com/blog/Screen%20Shot%202013-11-07%20at%2011.42.17%20AM.png") ||
				coverageNote.Contains("http://espnmediazone.com/us/files/2013/08/CF_Oct29_Maps_MZ.pdf");
        }

        public  bool IsSyndAffiliates(string coverageNote)
        {
            return coverageNote.Contains("acctourney.theacc.com") || coverageNote.Contains("theacc.com/live") || coverageNote.Contains("raycomsports")
                || coverageNote.Contains("SECNetWk") || coverageNote.Contains("Big12NetWk") || coverageNote.Contains("wacsports")
                || coverageNote.Contains("BigEastWk") || coverageNote.Contains("http://tinyurl.com/slctvstations") || IsASNLink(coverageNote);
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
                var doc = XDocument.Load(path);
                var xElement = doc.Element(docName).Element(element);
	            if (xElement != null)
		            return true;
            }
            return false;
        }

        private  bool IsASNLink(string coverageNote)
        {
            var path = HttpContext.Current.Server.MapPath(@"~/Content/ASNLinks.xml");
            if (File.Exists(path))
            {
                var doc = XDocument.Load(path);
                var xCollection = doc.Root.Elements();
            	return xCollection.Any(xItem => xItem.Attribute("link").Value == coverageNote);
            }
            return false;
        }

        public  bool IsFOXSportsGo(string coverageNote)
        {
            return coverageNote.ToLower().Contains("foxsportsgo.com");
        }

		public  bool CheckSportYearAttributesBool(string p, string attributeName)
		{
			var path = HttpContext.Current.Server.MapPath(@"~/Content/ValidSportYears.xml");
			if (File.Exists(path))
			{
				var doc = XDocument.Load(path);
				var xCollection = doc.Root.Elements(p);
				return xCollection.Any(xItem => Convert.ToBoolean(xItem.Attribute(attributeName).Value));
			}
			return false;
		}

		public  string CheckSportYearAttributes(string p, string attributeName)
		{
			var path = HttpContext.Current.Server.MapPath(@"~/Content/ValidSportYears.xml");
			if (File.Exists(path))
			{
				var doc = XDocument.Load(path);
				return doc.Root.Elements(p).Attributes(attributeName).First().Value;
			}
			return string.Empty;
		}

		public  bool IsESPNPPV(string ppv, string coverageNotes)
		{
			return ppv.Contains("gameplan") || coverageNotes.Contains("gameplan") || ppv.Contains("espnfc") || 
				coverageNotes.Contains("espnfc");
		}

    }
}