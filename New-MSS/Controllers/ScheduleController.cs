using System.Web.Mvc;
using New_MSS.BC;
using System;
using New_MSS.Shared;
using System.Collections.Generic;
using System.Linq;

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
            {
				var weeklyModel = _ws.GetWeeklyData(week, sportYear,
                    GetYear(sportYear), "Eastern", GetSport(sportYear));
                weeklyModel.TimeZoneList = CreateTimeZoneList("Eastern");
                return View(weeklyModel);
            }
            throw new Exception();
        }

        [HttpPost]
        public ActionResult Weekly(FormCollection fc, int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
                var timeZone = DeterminePostDropDownValue(fc);
				var weeklyModel = _ws.GetWeeklyData(week, sportYear,
                    GetYear(sportYear), timeZone, GetSport(sportYear));
                weeklyModel.TimeZoneList = CreateTimeZoneList(timeZone);
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
                weeklyTextModel.TimeZoneList = CreateTimeZoneList("Eastern");
                return View(weeklyTextModel);
            }
            throw new Exception();
        }

        [HttpPost]
        public ActionResult WeeklyText(FormCollection fc, int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
                var timeZone = DeterminePostDropDownValue(fc);
                var weeklyTextModel = _wts.GetWeeklyTextData(week, sportYear,
                    GetYear(sportYear), GetSport(sportYear), timeZone);
                weeklyTextModel.TimeZoneList = CreateTimeZoneList(timeZone);
                return View(weeklyTextModel);
            }
            throw new Exception();
        }

        public List<SelectListItem> timeZoneList = new List<SelectListItem>
        { 
            new SelectListItem { Text = "Eastern", Value = "0" },
            new SelectListItem { Text = "Central", Value = "1" },
            new SelectListItem { Text = "Mountain", Value = "2" },
            new SelectListItem { Text = "Arizona", Value = "3" },
            new SelectListItem { Text = "Pacific", Value = "4" },
            new SelectListItem { Text = "Alaska", Value = "5" },
            new SelectListItem { Text = "Hawai'i", Value = "6" }
        };

        public List<SelectListItem> CreateTimeZoneList(string timeZone)
        {
            foreach (var item in timeZoneList)
                item.Selected = item.Text == timeZone;
            return timeZoneList;
        }

        public string DeterminePostDropDownValue(FormCollection fc)
        {
            string dropDownListItem = fc.Get("DropDownTimeZone");
            return timeZoneList.First(x => x.Value == dropDownListItem).Text;
        }

    }
}