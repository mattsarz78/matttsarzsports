using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace New_MSS.Shared
{
    public interface ICoverageNotesHelper
    {
        string FormatCoverageNotes(string coverageNotesInput);
        void ConfigureText(List<string> coverageNotesList, string stringText);
        void ConfigureImage(List<string> coverageNotesList, string network, string breakSymbol);
        List<int> ValidateFieldData(string[] coverageNotes);
        string FormatNetworkJpg(string networksString);
    }
}
