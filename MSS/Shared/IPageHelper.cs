using MSS.Models;
using System.Collections.Generic;

namespace MSS.Shared
{
    public interface IPageHelper
    {
        string CheckForFlexSchedule(string year);
        string CheckSportYearAttributes(string p, string attributeName);
        List<ContractText> GetTextFromXml(string conference, string year);
	}
}
