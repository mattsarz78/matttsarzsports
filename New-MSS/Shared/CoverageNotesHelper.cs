﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Xml.Linq;

namespace New_MSS.Shared
{
    public class CoverageNotesHelper 
    {
        public const string NBSP = "<label>&nbsp</label>";
        public const string BR = "<br>";
        public const string AFFILIATES = "Affiliates";
        public const string CHANNELFINDER = "Channel Finder";
        public const string BLACKOUTMAP = "Blackout Map";
        public const string COVERAGEMAP = "Coverage Map";
        public const string COVERAGEMAP506 = "Coverage Map Courtesy The506.com";

        public static string FormatCoverageNotes(string coverageNotesInput)
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
                    {
                        if (Bools.IsImage(t) || Bools.IsImageHyperlink(t))
                            imgCoverage.Add(t);
                        else if (Bools.IsTextStreamingLink(t))
                            streamingCoverage.Add(t);
                        else
                            stringCoverage.Add(t);
                    }

                    FormatImages(imgCoverage.ToArray(), coverageNotesList);
                    if (coverageNotesTypeList[0] > 0 && coverageNotesTypeList[1] > 0 && coverageNotesList.Last() != BR)
                        coverageNotesList.Add(BR);
                    FormatTextStreaming(streamingCoverage.ToArray(), coverageNotesList);
                    if ((coverageNotesTypeList[0] > 0 || coverageNotesTypeList[1] > 0) && stringCoverage.Count > 0 && coverageNotesList.Last() != BR)
                        coverageNotesList.Add(BR);
                    FormatString(stringCoverage.ToArray(), coverageNotesList);
                }
            }

            foreach (string item in coverageNotesList)
            {
                coverageNotesString.Append(item);
            }
            string coverageNotesReturn = coverageNotesString.ToString();
            coverageNotesString.Clear();
            return coverageNotesReturn;
        }

        private static void FormatString(IEnumerable<string> p, List<string> coverageNotesList)
        {
            foreach (string stringText in p)
                ConfigureText(coverageNotesList, stringText);
        }

        private static void FormatTextStreaming(IEnumerable<string> p, List<string> coverageNotesList)
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
        private static void FormatImages(IEnumerable<string> p, List<string> coverageNotesList)
        {
            int counter = 0;
            foreach (string imageName in p)
            {
                counter++;
                if (Bools.IsImage(imageName))
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

        private static void ConfigureTextHyperlink(List<string> coverageNotesList, string coverageNote, string breakSymbol)
        {
            string textLink = string.Empty;

            if (!string.IsNullOrWhiteSpace(breakSymbol))
                coverageNotesList.Add(BR);

            if (Bools.IsNBC(coverageNote))
                textLink = "NBCSports.com";
            else if (Bools.IsBTN(coverageNote) || Bools.IsP12Networks(coverageNote))
                textLink = CHANNELFINDER;
            else if (Bools.IsSyndAffiliates(coverageNote))
                textLink = AFFILIATES;
            else if (Bools.IsCoverageMap(coverageNote))
                textLink = COVERAGEMAP;
            else if (Bools.IsThe506CoverageMap(coverageNote))
                textLink = COVERAGEMAP506;
            else if (Bools.IsGamePlanMap(coverageNote))
                textLink = BLACKOUTMAP;
			else if (Bools.IsOleMissLHN(coverageNote) || Bools.IsSpecialCoverageNote(coverageNote))
				textLink = "See link for full coverage information";
			else if (Bools.IsPPVProviders(coverageNote))
                textLink = "PPV Information";
            else
            {
                int index = coverageNote.IndexOf(".com", StringComparison.Ordinal);
                if (index > 0)
                    textLink = coverageNote.Substring(5, index + 4);
            }

            coverageNotesList.Add(String.Concat("<a href=\"", coverageNote, "\" target=\"_blank\">", textLink, "</a>"));
        }

        private static void ConfigureImgHyperlink(List<string> coverageNotesList, string coverageNote)
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

        public static void ConfigureText(List<string> coverageNotesList, string stringText)
        {
            if (Bools.IsHyperlink(stringText))
            {
                if (coverageNotesList.Count > 0 && coverageNotesList.Last() != BR)
                    coverageNotesList.Add(BR);

                string textLink;
                if (Bools.IsSyndAffiliates(stringText))
                    textLink = AFFILIATES;
                else if (Bools.IsCoverageMap(stringText))
                    textLink = COVERAGEMAP;
                else if (Bools.IsThe506CoverageMap(stringText))
                    textLink = COVERAGEMAP506;
                else if (Bools.IsGamePlanMap(stringText))
                    textLink = BLACKOUTMAP;
                else if (Bools.IsBTN(stringText) || Bools.IsP12Networks(stringText))
                    textLink = CHANNELFINDER;
                else
                    textLink = "Live Web Video";

                coverageNotesList.Add(String.Concat("<a href=\"", stringText, "\" target=\"_blank\">", textLink, "</a>"));
            }
            else
            {
                string p = String.Concat("<label>", stringText, "</label>");

                if (coverageNotesList.Count > 0 && coverageNotesList.Last() != BR)
                    coverageNotesList.Add(BR + p);
                else
                    coverageNotesList.Add(p);
            }
        }

        public static void ConfigureImage(List<string> coverageNotesList, string network, string breakSymbol)
        {
            if (!string.IsNullOrWhiteSpace(breakSymbol))
                coverageNotesList.Add(BR);

            coverageNotesList.Add(String.Concat("<img class=\"imageDimensions\" src=\"/Images/", network, "\" />"));
        }

        public static List<int> ValidateFieldData(string[] coverageNotes)
        {
            int imgCount = 0;
            int stringCount = 0;
            int streamingCount = 0;

            foreach (string t in coverageNotes)
            {
                if (Bools.IsImage(t) || Bools.IsImageHyperlink(t))
                    imgCount++;
                else if (Bools.IsTextStreamingLink(t))
                    streamingCount++;
                else
                    stringCount++;
            }

            return new List<int> { imgCount, streamingCount, stringCount };
        }

        public static string FormatNetworkJpg(string networksString)
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