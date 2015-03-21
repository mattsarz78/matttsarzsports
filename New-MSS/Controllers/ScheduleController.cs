using System.Web.Mvc;
using New_MSS.BC;
using System;
using New_MSS.Shared;

namespace New_MSS.Controllers
{
    public class ScheduleController : Controller
    {
        IBools _bools;
        IWeeklySchedule _ws;
        IWeeklyTextSchedule _wts;
        ITimeZoneHelper _tzh;
        IStoredProcHelper _sph;
        IPageHelper _ph;
        BaseController _bc;

        public ScheduleController()
        {
            _sph = new StoredProcHelper();
            _bools = new Bools();
            _ph = new PageHelper();
            _tzh = new TimeZoneHelper();
            _ws = new WeeklySchedule(_bools, _ph, new CoverageNotesHelper(_bools, _ph), _sph, new SeasonContents(_sph), _tzh);
            _wts = new WeeklyTextSchedule(_bools, _ph, new SeasonContents(_sph), _sph, _tzh);
            _bc = new BaseController();
        }

        public ScheduleController(IBools bools, IWeeklySchedule ws, IWeeklyTextSchedule wts, ITimeZoneHelper tzh, BaseController bc)
        {
        }

        [HttpGet]
        public ActionResult Weekly(int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
				var weeklyModel = _ws.GetWeeklyData(week, sportYear,
                    _bc.GetYear(sportYear), "Eastern", _bc.GetSport(sportYear), ControllerContext);
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
                    _bc.GetYear(sportYear), _tzh.DeterminePostDropDownValue(fc), _bc.GetSport(sportYear), ControllerContext);
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
                    _bc.GetYear(sportYear), _bc.GetSport(sportYear), "Eastern");
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
                    _bc.GetYear(sportYear), _bc.GetSport(sportYear), _tzh.DeterminePostDropDownValue(fc));
                return View(weeklyTextModel);
            }
            throw new Exception();
        }
    }
}