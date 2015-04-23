using System.Web.Mvc;
using New_MSS.BC;
using System;
using New_MSS.Shared;

namespace New_MSS.Controllers
{
    public class ScheduleController : BaseController
    {
        IBools _bools;
        IWeeklySchedule _ws;
        IWeeklyTextSchedule _wts;
        ITimeZoneHelper _tzh;
        IStoredProcHelper _sph;
        IPageHelper _ph;

        public ScheduleController()
        {
            _sph = new StoredProcHelper();
            _bools = new Bools();
            _ph = new PageHelper(_bools);
            _tzh = new TimeZoneHelper();
            _ws = new WeeklySchedule(_bools, _ph, new CoverageNotesHelper(_bools), _sph, new SeasonContents(_sph), _tzh);
            _wts = new WeeklyTextSchedule(_bools, _ph, new SeasonContents(_sph), _sph, _tzh);
        }

        [HttpGet]
        public ActionResult Weekly(int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyModel = _ws.GetWeeklyData(week, sportYear,
                    GetYear(sportYear), "Eastern", GetSport(sportYear));
                return View(weeklyModel);
            }
            throw new Exception();
        }

        [HttpPost]
        public ActionResult Weekly(FormCollection fc, int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyModel = _ws.GetWeeklyData(week, sportYear,
                    GetYear(sportYear), _tzh.DeterminePostDropDownValue(fc), GetSport(sportYear));
				return View(weeklyModel);
            }
            throw new Exception();
        }

        [HttpGet]
        public ActionResult WeeklyText(int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyTextModel = _wts.GetWeeklyTextData(week, sportYear,
                    GetYear(sportYear), GetSport(sportYear), "Eastern");
                return View(weeklyTextModel);
            }
            throw new Exception();
        }

        [HttpPost]
        public ActionResult WeeklyText(FormCollection fc, int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyTextModel = _wts.GetWeeklyTextData(week, sportYear,
                    GetYear(sportYear), GetSport(sportYear), _tzh.DeterminePostDropDownValue(fc));
                return View(weeklyTextModel);
            }
            throw new Exception();
        }
    }
}