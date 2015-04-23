﻿using New_MSS.Models;
using System.Collections.Generic;

namespace New_MSS.Shared
{
    public interface IPageHelper
    {
        string CheckForFlexSchedule(string year);
        List<ContractText> GetTextFromXml(string conference, string year);
        bool CheckIfBowlWeekOrNIT(int week, List<YearDate> fullYearDates);
        bool CheckIfBasketballPostseason(int week, List<YearDate> fullYearDates);
    }
}
