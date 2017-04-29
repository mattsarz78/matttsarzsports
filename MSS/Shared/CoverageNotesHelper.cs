using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Xml.Linq;

namespace MSS.Shared
{
    public class CoverageNotesHelper : ICoverageNotesHelper
    {
        public const string NBSP = "<label>&nbsp</label>";
        public const string BR = "<br>";
        public const string AFFILIATES = "Affiliates";
        public const string CHANNELFINDER = "Channel Finder";
        public const string BLACKOUTMAP = "Blackout Map";
        public const string COVERAGEMAP = "Coverage Map";
        public const string COVERAGEMAP506 = "Coverage Map Courtesy The506.com";

	    readonly IBools _bools;

        public CoverageNotesHelper(IBools bools)
        {
            _bools = bools;
        }

        public string FormatCoverageNotes(string year, string coverageNotesInput)
        {
            var coverageNotesList = new List<string>();
            var coverageNotesString = new StringBuilder();
            if (string.IsNullOrWhiteSpace(coverageNotesInput))
                coverageNotesList.Add(NBSP);
            else
            {
                OrderCoverageNotes(year, coverageNotesInput, coverageNotesList);
            }

            foreach (string item in coverageNotesList)
		        coverageNotesString.Append(item);
	        string coverageNotesReturn = coverageNotesString.ToString();
            coverageNotesString.Clear();
            return coverageNotesReturn;
        }

        private void OrderCoverageNotes(string year, string coverageNotesInput, List<string> coverageNotesList)
        {
            string[] coverageNotes = coverageNotesInput.Split(',');
            if (String.IsNullOrEmpty(coverageNotes[0]))
                coverageNotesList.Add(NBSP);
            else
            {
                var imgCoverage = new List<string>();
                var streamingCoverage = new List<string>();
                var stringCoverage = new List<string>();
                foreach (string t in coverageNotes)
                    DetermineCoverageType(t, imgCoverage, streamingCoverage, stringCoverage);
                FormatCoverageNotesListTypes(year, imgCoverage, coverageNotesList, ValidateFieldData(coverageNotes), streamingCoverage, stringCoverage);
            }
        }

        private void FormatCoverageNotesListTypes(string year, List<string> imgCoverage, List<string> coverageNotesList, List<int> coverageNotesTypeList,
            List<string> streamingCoverage, List<string> stringCoverage)
        {
            FormatImages(year, imgCoverage.ToArray(), coverageNotesList);
            if (coverageNotesTypeList[0] > 0 && coverageNotesTypeList[1] > 0 && coverageNotesList.Last() != BR)
                coverageNotesList.Add(BR);
            FormatTextStreaming(streamingCoverage.ToArray(), coverageNotesList);
            if ((coverageNotesTypeList[0] > 0 || coverageNotesTypeList[1] > 0) && stringCoverage.Count > 0 &&
                coverageNotesList.Last() != BR)
                coverageNotesList.Add(BR);
            FormatString(stringCoverage.ToArray(), coverageNotesList);
        }

        private void DetermineCoverageType(string t, List<string> imgCoverage, List<string> streamingCoverage, List<string> stringCoverage)
        {
            if (_bools.IsImage(t) || _bools.IsImageHyperlink(t))
                imgCoverage.Add(t);
            else if (_bools.IsTextStreamingLink(t))
                streamingCoverage.Add(t);
            else
                stringCoverage.Add(t);
        }

        private void FormatString(IEnumerable<string> p, List<string> coverageNotesList)
        {
            foreach (string stringText in p)
                ConfigureText(coverageNotesList, stringText);
        }

        private void FormatTextStreaming(IEnumerable<string> p, List<string> coverageNotesList)
        {
            int counter = 0;
            foreach (string stream in p)
            {
                counter++;
                if ((counter % 2 == 0 && counter >= 2) && coverageNotesList.Count > 0)
                    ConfigureTextHyperlink(coverageNotesList, stream, BR);
                else
                    ConfigureTextHyperlink(coverageNotesList, stream, null);
            }
        }
        private void FormatImages(string year, IEnumerable<string> p, List<string> coverageNotesList)
        {
            int counter = 0;
            foreach (string imageName in p)
            {
                counter++;
                if (_bools.IsImage(imageName))
                {
                    if (counter % 3 == 0 && (counter == 4 || counter >= 7))
                        ConfigureImage(coverageNotesList, imageName, BR);
                    else
                        ConfigureImage(coverageNotesList, imageName, null);
                }
                else
                {
                    ConfigureImgHyperlink(year, coverageNotesList, imageName);
                }
            }
        }

        private void ConfigureTextHyperlink(List<string> coverageNotesList, string coverageNote, string breakSymbol)
        {
            string textLink = ConfigureTextLink(coverageNote);

            if (!string.IsNullOrWhiteSpace(breakSymbol))
                coverageNotesList.Add(BR);

            textLink = string.IsNullOrWhiteSpace(textLink) ? ConfigureTexyHyperlinkSpecifics(coverageNote, textLink) : textLink;

            coverageNotesList.Add(string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", coverageNote, textLink));
        }

        private string ConfigureTexyHyperlinkSpecifics(string coverageNote, string textLink)
        {
            if (_bools.IsNBC(coverageNote))
                textLink = "NBCSports.com";
            else if (_bools.IsOleMissLHN(coverageNote) || _bools.IsSpecialCoverageNote(coverageNote))
                textLink = "See link for full coverage information";
            else if (_bools.IsPPVProviders(coverageNote))
                textLink = "PPV Information";
            else
            {
                int index = coverageNote.IndexOf(".com", StringComparison.Ordinal);
                if (index > 0)
                    textLink = coverageNote.Substring(5, index + 4);
            }
            return textLink;
        }

        private void ConfigureImgHyperlink(string year, List<string> coverageNotesList, string coverageNote)
        {

			var path = HttpContext.Current.Server.MapPath(@"~/Content/ImagesForURLs.xml");
			if (File.Exists(path))
			{
				var doc = XDocument.Load(path);
				var xElements = doc.Root.Elements("Address").Where(x => coverageNote.Contains(x.Attribute("Link").Value));
				string imageUrl = SetImageUrl(year, xElements);

				coverageNotesList.Add(string.Format("<a href=\"{0}\" target=\"_blank\" ><img class=\"imageDimensions\" src=\"/Images/{1}\" /></a>", coverageNote, imageUrl));
			}
		}

		private static string SetImageUrl(string year, IEnumerable<XElement> xElements)
		{
			string imageUrl = string.Empty;
			foreach (var item in xElements)
			{
				bool donotuse = false;
				if (item.Attribute("Yearend") != null)
				{
					var years = item.Attribute("Yearend").Value.Split('|').ToList();
					var yeartocompare = years.First(x => x.Length == year.Length);
					donotuse = (Convert.ToInt32(year) > Convert.ToInt32(yeartocompare));
					imageUrl = !donotuse ? item.Attribute("Image").Value : string.Empty;
					if (imageUrl.Length > 0)
						break;
				}
				else
					imageUrl = item.Attribute("Image").Value;
			}

			return imageUrl;
		}

		public void ConfigureText(List<string> coverageNotesList, string stringText)
        {
            if (_bools.IsHyperlink(stringText))
            {
                if (coverageNotesList.Count > 0 && coverageNotesList.Last() != BR)
                    coverageNotesList.Add(BR);

                string textLink = ConfigureTextLink(stringText);
                textLink = string.IsNullOrWhiteSpace(textLink) ? "Live Video" : textLink;

                coverageNotesList.Add(string.Format("<a class=\"linkblock\" href=\"{0}\" target=\"_blank\">{1}</a>", stringText, textLink));
            }
            else
            {
                string p = string.Format("<label>{0}</label>", stringText);
                if (coverageNotesList.Count > 0 && coverageNotesList.Last() != BR)
                    coverageNotesList.Add(BR + p);
                else
                    coverageNotesList.Add(p);
            }
        }

        private string ConfigureTextLink(string stringText) 
        {
            string textLink = string.Empty;
            if (_bools.IsSyndAffiliates(stringText))
                textLink = AFFILIATES;
            else if (_bools.IsCoverageMap(stringText))
                textLink = COVERAGEMAP;
            else if (_bools.IsThe506CoverageMap(stringText))
                textLink = COVERAGEMAP506;
            else if (_bools.IsGamePlanMap(stringText))
                textLink = BLACKOUTMAP;
            else if (_bools.IsBTN(stringText) || _bools.IsP12Networks(stringText))
                textLink = CHANNELFINDER;
            return textLink;
        }

        public void ConfigureImage(List<string> coverageNotesList, string network, string breakSymbol)
        {
            if (!string.IsNullOrWhiteSpace(breakSymbol))
                coverageNotesList.Add(BR);
            coverageNotesList.Add(string.Format("<img class=\"imageDimensions\" src=\"/Images/{0}\" />", network));
        }

        public List<int> ValidateFieldData(string[] coverageNotes)
        {
            int imgCount = 0;
            int stringCount = 0;
            int streamingCount = 0;

            foreach (string t in coverageNotes)
            {
                if (_bools.IsImage(t) || _bools.IsImageHyperlink(t))
                    imgCount++;
                else if (_bools.IsTextStreamingLink(t))
                    streamingCount++;
                else
                    stringCount++;
            }

            return new List<int> { imgCount, streamingCount, stringCount };
        }

        public string FormatNetworkJpg(string networksString)
        {
            string[] networks = networksString.Split(',');
            var networksList = new List<string>();
            var networksStringBuilder = new StringBuilder();

            var coverageNotesTypeList = ValidateFieldData(networks);
            OrderNetworks(networks, networksList, coverageNotesTypeList);

            foreach (string item in networksList)
                networksStringBuilder.Append(item);

            string networksReturn = networksStringBuilder.ToString();
            networksStringBuilder.Clear();
            return networksReturn;
        }

        private void OrderNetworks(string[] networks, List<string> networksList, List<int> coverageNotesTypeList)
        {
            if ((coverageNotesTypeList[0] == 1 && coverageNotesTypeList[2] == 0))
                ConfigureImage(networksList, networks[0], null);
            else if (coverageNotesTypeList[0] > 1 && coverageNotesTypeList[2] == 0)
            {
                foreach (string network in networks)
                    ConfigureImage(networksList, network, null);
            }
            else if (coverageNotesTypeList[0] == 1 && coverageNotesTypeList[2] == 1)
            {
                ConfigureImage(networksList, networks[0], null);
                ConfigureText(networksList, networks[1]);
            }
            else
                ConfigureText(networksList, networks[0]);
        }
    }
}