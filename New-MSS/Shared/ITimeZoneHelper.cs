using System;

namespace New_MSS.Shared
{
    public interface ITimeZoneHelper
    {
        DateTime Offset(string timeZone, DateTime gameTime);
        string FormatTelevisedTime(DateTime gameTimeInput, string caller, string timeZone);
    }
}
