using MSS.Models;
using System.Collections.Generic;

namespace MSS.Shared
{
    public interface IPageHelper
    {
        string CheckForFlexSchedule(string year);
        List<ContractText> GetTextFromXml(string conference, string year);
        bool CheckIfBowlWeek(int week, List<YearDate> fullYearDates);
        bool CheckIfBasketballPostseason(int week, List<YearDate> fullYearDates);
		bool CheckIfNIT(int week, List<YearDate> fullYearDates);
	}
}
