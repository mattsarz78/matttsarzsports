using New_MSS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace New_MSS.BC
{
    public interface IWeeklySchedule
    {
        WeeklyModel GetWeeklyData(int week, string sportYear, string year, string timeZone, string sport, ControllerContext controllerContext);
    }
}
