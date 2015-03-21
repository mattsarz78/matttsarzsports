using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace New_MSS.Models
{
    public class WeeklyModel
    {
        public bool IsBasketballPostseason { get; set; }
		public bool IsBowlWeek { get; set; }
        public bool IsNIT { get; set; }
        public List<NonTelevisedGame> NoTVGameList { get; set; }
        public List<TelevisedGame> TelevisedGamesList { get; set; }
        public WeekDates WeekDates { get; set; }
        public bool IsFootball { get; set; }
        public List<SelectListItem> TimeZoneList { get; set; }
        public string Week { get; set; }
        public bool ShowRSNPartialView { get; set; }
        public bool ShowNoTVPartialView { get; set; }
        public string SportYear { get; set; }
        public bool ShowPPVColumn { get; set; }

        public string Year { get; set; }

        public string FlexScheduleLink { get; set; }

		public bool IsNextWeekBowlWeek { get; set; }

		public bool IsNextWeekBasketballPostseason { get; set; }
	}

    public class NonTelevisedGame
    {
        public string Game { get; set; }
        public string Conference { get; set; }
        public string TVOptions { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime Time { get; set; }
        public bool FCS { get; set; }
    }

    public class TelevisedGame
    {
        public string Game { get; set; }
        public string Network { get; set; }
        public string CoverageNotes { get; set; }
        public string PPV { get; set; }
        public string Mediaindicator { get; set; }
        public DateTime Time { get; set; }
        public string TimeString { get; set; }
        public bool ShowPPVColumn { get; set; }
    }

    public class WeekDates
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
