using System.Web.Mvc;
using New_MSS.BC;
using New_MSS.Models;
using New_MSS.Shared;
using System;

namespace New_MSS.Controllers
{
    public class SeasonController : Controller
    {
        IBools _bools;
        ISeasonContents _sc;
        BaseController _bc;
        public SeasonController()
        {
            _bools = new Bools();
            _sc = new SeasonContents(new StoredProcHelper());
            _bc = new BaseController();
        }

        public ActionResult Contents(string sportYear)
        {
            if (_bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
            	var season = _bc.GetYear(sportYear);
                var isFootball = _bc.GetSport(sportYear).Contains("football");

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
