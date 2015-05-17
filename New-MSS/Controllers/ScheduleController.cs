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
        IStoredProcHelper _sph;
        IPageHelper _ph;

        public ScheduleController()
        {
            _sph = new StoredProcHelper();
            _bools = new Bools();
            _ph = new PageHelper(_bools);
            _ws = new WeeklySchedule(_bools, _ph, new CoverageNotesHelper(_bools), _sph, new SeasonContents(_sph), new TimeZoneHelper());
            _wts = new WeeklyTextSchedule(_bools, _ph, new SeasonContents(_sph), _sph, new TimeZoneHelper());
        }

        [HttpGet]
        public ActionResult Weekly(int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
                return View(_ws.GetWeeklyData(week, sportYear, GetYear(sportYear), "Eastern", GetSport(sportYear)));
            throw new Exception();
        }

        [HttpPost]
        public ActionResult Weekly(string timeZoneValue, int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
                return PartialView("WeeksBase", _ws.GetWeeklyData(week, sportYear, GetYear(sportYear), timeZoneValue, GetSport(sportYear)));
            throw new Exception();
        }

        [HttpGet]
        public ActionResult WeeklyText(int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
                return View(_wts.GetWeeklyTextData(week, sportYear, GetYear(sportYear), GetSport(sportYear), "Eastern"));
            throw new Exception();
        }

        [HttpPost]
        public ActionResult WeeklyText(string timeZoneValue, int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
                return PartialView("TextGames", _wts.GetWeeklyTextData(week, sportYear, GetYear(sportYear), GetSport(sportYear), timeZoneValue));
            throw new Exception();
        }
    }
}