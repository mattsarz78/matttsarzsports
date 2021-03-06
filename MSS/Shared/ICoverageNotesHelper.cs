﻿using System.Collections.Generic;

namespace MSS.Shared
{
    public interface ICoverageNotesHelper
    {
        string FormatCoverageNotes(string year, string coverageNotesInput);
        void ConfigureText(List<string> coverageNotesList, string stringText);
        void ConfigureImage(List<string> coverageNotesList, string network, string breakSymbol);
        List<int> ValidateFieldData(string[] coverageNotes);
        string FormatNetworkJpg(string networksString);
    }
}
