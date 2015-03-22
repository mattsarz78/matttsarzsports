using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace New_MSS.Shared
{
    public interface ITimeZoneHelper
    {
        List<SelectListItem> CreateTimeZoneList(string timeZone);
        string DeterminePostDropDownValue(FormCollection fc);
        DateTime Offset(string timeZone, DateTime gameTime);
        string FormatTelevisedTime(DateTime gameTimeInput, string caller, string timeZone);
    }
}
