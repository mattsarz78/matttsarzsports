using System.Web.Mvc;
using New_MSS.BC;
using System;
using New_MSS.Shared;

namespace New_MSS.Controllers
{
    public class ScheduleController : BaseController
    {
        [HttpGet]
        public ActionResult Weekly(int week, string sportYear)
        {
            if (Bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyModel = WeeklySchedule.GetWeeklyData(week, sportYear,
					GetYear(sportYear), "Eastern", GetSport(sportYear), ControllerContext);
				return View(weeklyModel);
            }
            throw new Exception();
        }

        [HttpPost]
        public ActionResult Weekly(FormCollection fc, int week, string sportYear)
        {
            if (Bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyModel = WeeklySchedule.GetWeeklyData(week, sportYear,
					GetYear(sportYear), TimeZoneHelper.DeterminePostDropDownValue(fc), GetSport(sportYear), ControllerContext);
				return View(weeklyModel);
            }
            throw new Exception();
        }

        [HttpGet]
        public ActionResult WeeklyText(int week, string sportYear)
        {
            if (Bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyTextModel = WeeklyTextSchedule.GetWeeklyTextData(week, sportYear, 
					GetYear(sportYear), GetSport(sportYear), "Eastern");
                return View(weeklyTextModel);
            }
            throw new Exception();
        }

        [HttpPost]
        public ActionResult WeeklyText(FormCollection fc, int week, string sportYear)
        {
            if (Bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyTextModel = WeeklyTextSchedule.GetWeeklyTextData(week, sportYear,
					GetYear(sportYear), GetSport(sportYear), TimeZoneHelper.DeterminePostDropDownValue(fc));
                return View(weeklyTextModel);
            }
            throw new Exception();
        }
    }
}