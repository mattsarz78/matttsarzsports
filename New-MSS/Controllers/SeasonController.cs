using System.Web.Mvc;
using New_MSS.BC;
using New_MSS.Models;
using New_MSS.Shared;
using System;

namespace New_MSS.Controllers
{
    public class SeasonController : BaseController
    {
        public ActionResult Contents(string sportYear)
        {
            if (Bools.CheckXMLDoc("ValidSportYears", sportYear.ToLower()))
            {
            	var season = GetYear(sportYear);
            	var isFootball = GetSport(sportYear).Contains("football");

            	var dateModel = new DateModel
            	                	{
            	                		YearDatesList = SeasonContents.CreateDateModel(season),
            	                		Year = sportYear.ToLower(),
            	                		IsFootball = isFootball,
										Title = SeasonContents.CreateTitle(sportYear),
										IsBasketballWithPostseason = !isFootball && Bools.CheckSportYearAttributesBool(sportYear, "hasPostseason"),
										ConferenceListBase = isFootball ? Bools.CheckSportYearAttributes(sportYear, "conferenceListBase") : string.Empty
            	                	};
                return View(dateModel);
            }
            throw new Exception();
        }
    }
}
