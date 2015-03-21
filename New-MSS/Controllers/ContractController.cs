﻿using System.Web.Mvc;
using New_MSS.BC;
using New_MSS.Models;
using New_MSS.Shared;
using System;

namespace New_MSS.Controllers
{
    public class ContractController : Controller
    {
        IBools _bools;
        IConferenceSchedule _confSched;
        IPageHelper _ph;
        BaseController _bc;
        public ContractController()
        {
            _bools = new Bools();
            _ph = new PageHelper();
            _confSched = new ConferenceSchedule(_bools, new CoverageNotesHelper(_bools, _ph), new StoredProcHelper(), new TimeZoneHelper()); 
            _bc = new BaseController();
        }

        public ActionResult GameList(string conference, int year)
        {
            if (_bools.CheckXMLDoc("ConferenceNames", conference.ToLower()) || !_bools.CheckXMLDoc("ValidYears", "Year" + year))
            {
                var sportYear = String.Concat("football", year);
            	var isIndependents = conference.ToLower().Contains("independents");
                var conferenceModel = new ConferenceModel
                {
                    ContractText = isIndependents ? string.Empty : _ph.GetTextFromXml(conference, year.ToString()),
                    ConferenceGames = isIndependents ? _confSched.CreateIndependentsGameList(year)
                                          : _confSched.CreateConferenceGameList(_bc.AddSpaces(conference), year.ToString()),
                    SportYear = sportYear,
                    Year = year.ToString(),
                    ConferenceName = _bc.AddSpaces(conference),
					FlexScheduleLink = _ph.CheckForFlexSchedule(year.ToString())
                };
                return View(conferenceModel);
            }
            throw new Exception();
        }
    }
}
