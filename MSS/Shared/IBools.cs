using MSS.Models;
using System.Collections.Generic;

namespace MSS.Shared
{
    public interface IBools
    {
        bool IsImage(string coverageNote);
        bool IsImageHyperlink(string coverageNote);
        bool IsTextStreamingLink(string coverageNote);
        bool IsOleMissLHN(string coverageNote);
        bool IsPPVProviders(string coverageNote);
        bool IsGamePlanMap(string coverageNote);
        bool IsThe506CoverageMap(string coverageNote);
        bool IsCoverageMap(string coverageNote);
        bool IsSyndAffiliates(string coverageNote);
        bool IsNBC(string coverageNote);
        bool IsBTN(string coverageNote);
        bool IsHyperlink(string coverageNote);
        bool IsSpecialCoverageNote(string coverageNote);
        bool IsP12Networks(string coverageNote);
        bool CheckXMLDoc(string docName, string element);
        bool CheckSportYearAttributesBool(string p, string attributeName);
        bool IsESPNPPV(string ppv, string coverageNotes);
        bool CheckIfBowlWeek(int week, List<YearDate> fullYearDates);
        bool CheckIfBasketballPostseason(int week, List<YearDate> fullYearDates);
        bool CheckIfNIT(int week, List<YearDate> fullYearDates);
        bool CheckIfOtherMBKTourney(int week, List<YearDate> fullYearDates);
        bool CheckIfFirstWeek(int week, List<YearDate> fullYearDates);
        bool isConferenceTournament(string sport, string game);
    }
}
