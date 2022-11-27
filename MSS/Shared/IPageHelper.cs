using MSS.Models;
using System.Collections.Generic;

namespace MSS.Shared
{
    public interface IPageHelper
    {
        bool CheckForFlexSchedule(string year);

        string GetFlexScheduleLink(string year);
        string CheckSportYearAttributes(string p, string attributeName);
        List<ContractText> GetTextFromXml(string conference, string year);
	}
}
