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

        public string FormatCoverageNotes(string coverageNotesInput)
        {
            var coverageNotesList = new List<string>();
            var coverageNotesString = new StringBuilder();
            if (string.IsNullOrWhiteSpace(coverageNotesInput))
                coverageNotesList.Add(NBSP);
            else
            {
                string[] coverageNotes = coverageNotesInput.Split(',');
                if (String.IsNullOrEmpty(coverageNotes[0]))
                    coverageNotesList.Add(NBSP);
                else
                {
                    List<int> coverageNotesTypeList = ValidateFieldData(coverageNotes);

                    var imgCoverage = new List<string>();
                    var streamingCoverage = new List<string>();
                    var stringCoverage = new List<string>();
                    foreach (string t in coverageNotes)
                        DetermineCoverageType(t, imgCoverage, streamingCoverage, stringCoverage);
                    FormatCoverageNotesListTypes(imgCoverage, coverageNotesList, coverageNotesTypeList, streamingCoverage, stringCoverage);
                }
            }

	        foreach (string item in coverageNotesList)
		        coverageNotesString.Append(item);
	        string coverageNotesReturn = coverageNotesString.ToString();
            coverageNotesString.Clear();
            return coverageNotesReturn;
        }

        private void FormatCoverageNotesListTypes(List<string> imgCoverage, List<string> coverageNotesList, List<int> coverageNotesTypeList,
            List<string> streamingCoverage, List<string> stringCoverage)
        {
            FormatImages(imgCoverage.ToArray(), coverageNotesList);
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
        private void FormatImages(IEnumerable<string> p, List<string> coverageNotesList)
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
                    ConfigureImgHyperlink(coverageNotesList, imageName);
                }
            }
        }

        private void ConfigureTextHyperlink(List<string> coverageNotesList, string coverageNote, string breakSymbol)
        {
            string textLink = ConfigureTextLink(coverageNote);

            if (!string.IsNullOrWhiteSpace(breakSymbol))
                coverageNotesList.Add(BR);

            textLink = String.IsNullOrWhiteSpace(textLink) ? ConfigureTexyHyperlinkSpecifics(coverageNote, textLink) : textLink;

            coverageNotesList.Add(String.Concat("<a href=\"", coverageNote, "\" target=\"_blank\">", textLink, "</a>"));
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

        private void ConfigureImgHyperlink(List<string> coverageNotesList, string coverageNote)
        {
            string imageUrl = string.Empty;

			var path = HttpContext.Current.Server.MapPath(@"~/Content/ImagesForURLs.xml");
			if (File.Exists(path))
			{
				var doc = XDocument.Load(path);
				var xElement = doc.Root.Elements("Address").First(x => coverageNote.Contains(x.Attribute("Link").Value));
				if (xElement != null)
					imageUrl = xElement.Attribute("Image").Value;
				coverageNotesList.Add(String.Concat("<a href=\"", coverageNote, "\" target=\"_blank\" ><img class=\"imageDimensions\" src=\"/Images/",
					imageUrl, "\" /></a>"));
			}
        }

        public void ConfigureText(List<string> coverageNotesList, string stringText)
        {
            if (_bools.IsHyperlink(stringText))
            {
                if (coverageNotesList.Count > 0 && coverageNotesList.Last() != BR)
                    coverageNotesList.Add(BR);

                string textLink = ConfigureTextLink(stringText);
                textLink = string.IsNullOrWhiteSpace(textLink) ? "Live Video" : textLink;

                coverageNotesList.Add(string.Concat("<a class=\"linkblock\" href=\"", stringText, "\" target=\"_blank\">", textLink, "</a>"));
            }
            else
            {
                string p = string.Concat("<label>", stringText, "</label>");
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
            coverageNotesList.Add(string.Concat("<img class=\"imageDimensions\" src=\"/Images/", network, "\" />"));
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

            foreach (string item in networksList)
                networksStringBuilder.Append(item);

			string networksReturn = networksStringBuilder.ToString();
            networksStringBuilder.Clear();
            return networksReturn;
        }
    }
}