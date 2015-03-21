using System.Web.Mvc;
using New_MSS.BC;
using New_MSS.Models;
using New_MSS.Shared;
using System;

namespace New_MSS.Controllers
{
    public class ContractController : BaseController
    {
        public ActionResult GameList(string conference, int year)
        {
            if (Bools.CheckXMLDoc("ConferenceNames", conference.ToLower()) || !Bools.CheckXMLDoc("ValidYears", "Year" + year))
            {
                var sportYear = String.Concat("football", year);
            	var isIndependents = conference.ToLower().Contains("independents");
                var conferenceModel = new ConferenceModel
                {
                    ContractText = isIndependents ? string.Empty : PageHelper.GetTextFromXml(conference, year.ToString()),
                    ConferenceGames = isIndependents ? ConferenceSchedule.CreateIndependentsGameList(year) 
                                          : ConferenceSchedule.CreateConferenceGameList(AddSpaces(conference), year.ToString()),
                    SportYear = sportYear,
                    Year = year.ToString(),
                    ConferenceName = AddSpaces(conference),
					FlexScheduleLink = PageHelper.CheckForFlexSchedule(year.ToString())
                };
                return View(conferenceModel);
            }
            throw new Exception();
        }
    }
}
