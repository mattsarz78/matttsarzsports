using System.Web.Mvc;
using New_MSS.BC;
using New_MSS.Models;
using New_MSS.Shared;
using System;

namespace New_MSS.Controllers
{
    public class SeasonController : BaseController
    {
        IBools _bools;
        ISeasonContents _sc;

        public SeasonController()
        {
            _bools = new Bools();
            _sc = new SeasonContents(new StoredProcHelper());
        }

        public ActionResult Contents(string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
            	var season = GetYear(sportYear);
                var isFootball = GetSport(sportYear).Contains("football");

            	var dateModel = new DateModel
            	                	{
            	                		YearDatesList = _sc.CreateDateModel(season),
            	                		Year = sportYear.ToLower(),
            	                		IsFootball = isFootball,
										Title = _sc.CreateTitle(sportYear),
                                        IsBasketballWithPostseason = !isFootball && _bools.CheckSportYearAttributesBool(sportYear, "hasPostseason"),
                                        ConferenceListBase = isFootball ? _bools.CheckSportYearAttributes(sportYear, "conferenceListBase") : string.Empty
            	                	};
                return View(dateModel);
            }
            throw new Exception();
        }
    }
}
