using MSS.BC;
using MSS.Models;
using MSS.Shared;
using System;
using System.Web.Http;

namespace MSS.Controllers
{
    public class BaseApiController : ApiController
    {
	    readonly IBools _bools;
	    readonly IConferenceSchedule _confSched;
	    readonly IPageHelper _ph;
	    readonly IWeeklySchedule _ws;
	    readonly IDailySchedule _ds;
	    readonly IWeeklyTextSchedule _wts;
	    readonly IDailyTextSchedule _dts;
	    readonly ISeasonContents _sc;

        public BaseApiController()
        {
            _bools = new Bools();
            _ph = new PageHelper(_bools);
            _confSched = new ConferenceSchedule(_bools, new CoverageNotesHelper(_bools), new StoredProcHelper(), new TimeZoneHelper());
            _ws = new WeeklySchedule(_bools, new PageHelper(_bools), new CoverageNotesHelper(_bools), new StoredProcHelper(), new SeasonContents(new StoredProcHelper()), new TimeZoneHelper());
			_ds = new DailySchedule(_bools, new PageHelper(_bools), new CoverageNotesHelper(_bools), new StoredProcHelper(), new SeasonContents(new StoredProcHelper()), new TimeZoneHelper());
			_wts = new WeeklyTextSchedule(_bools, new PageHelper(_bools), new SeasonContents(new StoredProcHelper()), new StoredProcHelper(), new TimeZoneHelper());
			_dts = new DailyTextSchedule(_bools, new PageHelper(_bools), new SeasonContents(new StoredProcHelper()), new StoredProcHelper(), new TimeZoneHelper());
			_sc = new SeasonContents(new StoredProcHelper());
        }

        public IHttpActionResult GetGameList(string conference, int year)
        {
            if (_bools.CheckXMLDoc("ConferenceNames", conference.ToLower()) || !_bools.CheckXMLDoc("ValidYears", "Year" + year))
            {
                var sportYear = String.Concat("football", year);
                var isIndependents = conference.ToLower().Contains("independents");
                return Ok(new ConferenceModel
                {
                    ContractTexts = _ph.GetTextFromXml(conference, year.ToString()),
                    ConferenceGames = isIndependents ? _confSched.CreateIndependentsGameList(year)
                                          : _confSched.CreateConferenceGameList(AddSpaces(conference), year.ToString()),
                    SportYear = sportYear,
                    Year = year.ToString(),
                    ConferenceName = AddSpaces(conference),
                    FlexScheduleLink = _ph.CheckForFlexSchedule(year.ToString())
                });
            }
            return NotFound();
        }

        public IHttpActionResult GetWeekly(string timeZoneValue, int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
                return Ok(_ws.GetWeeklyData(week, sportYear, GetYear(sportYear), timeZoneValue, GetSport(sportYear)));
            return NotFound();
        }

        public IHttpActionResult GetWeeklyText(string timeZoneValue, int week, string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
                return Ok(_wts.GetWeeklyTextData(week, sportYear, GetYear(sportYear), GetSport(sportYear), timeZoneValue));
            return NotFound();
        }

		public IHttpActionResult GetDaily(string timeZoneValue, string sportYear)
		{
			if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
				return Ok(_ds.GetDailyData(sportYear, GetYear(sportYear), timeZoneValue, GetSport(sportYear)));
			return NotFound();
		}

		public IHttpActionResult GetDailyText(string timeZoneValue, string sportYear)
		{
			if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
				return Ok(_dts.GetDailyTextData(sportYear, GetYear(sportYear), GetSport(sportYear), timeZoneValue));
			return NotFound();
		}

		public IHttpActionResult GetContents(string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
                var season = GetYear(sportYear);
                var isFootball = GetSport(sportYear).Contains("football");
                return Ok(new DateModel
                {
                    YearDatesList = _sc.CreateDateModel(season),
                    Year = sportYear.ToLower(),
                    IsFootball = isFootball,
                    Title = _sc.CreateTitle(sportYear),
                    IsBasketballWithPostseason = !isFootball && _bools.CheckSportYearAttributesBool(sportYear, "hasPostseason"),
                    ConferenceListBase = isFootball ? _bools.CheckSportYearAttributes(sportYear, "conferenceListBase") : string.Empty
                });
            }
            return NotFound();
        }

        private string GetYear(string sportYear)
        {
            return sportYear.ToLower().Contains("football") ? sportYear.Substring(8, 4) : sportYear.Substring(10, 6);
        }

        private string GetSport(string sportYear)
        {
            return sportYear.ToLower().Contains("football") ? "football" : "basketball";
        }

        private string AddSpaces(string conference)
        {
            return (conference.Substring(0, 3).Contains("Big") || conference.Substring(0, 3).Contains("Sun")) ?
                conference.Replace(conference.Substring(0, 3), conference.Substring(0, 3) + " ") : conference;
        }

    }
}
