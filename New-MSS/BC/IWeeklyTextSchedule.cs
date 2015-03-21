using New_MSS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace New_MSS.BC
{
    public interface IWeeklyTextSchedule
    {
        WeeklyModel GetWeeklyTextData(int week, string sportYear, string year, string sport, string timeZone);
    }
}
