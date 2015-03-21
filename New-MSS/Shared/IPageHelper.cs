using New_MSS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace New_MSS.Shared
{
    public interface IPageHelper
    {
        string CheckForFlexSchedule(string year);
        string GetTextFromXml(string conference, string year);
        bool CheckIfBowlWeekOrNIT(int week, List<YearDate> fullYearDates);
        bool CheckIfBasketballPostseason(int week, List<YearDate> fullYearDates);
    }
}
