using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MSS.Models;
using MSS.Shared;

namespace MSS.BC
{
	public class DailySchedule : Schedule, IDailySchedule
	{
		readonly IBools _bools;
		readonly IPageHelper _ph;
		readonly IStoredProcHelper _sph;
		readonly ISeasonContents _sc;
		readonly ITimeZoneHelper _tzh;

		public DailySchedule(IBools bools, IPageHelper ph, ICoverageNotesHelper cnh, IStoredProcHelper sph, ISeasonContents sc, ITimeZoneHelper tzh)
			: base(cnh, sph, tzh)
		{
			_bools = bools;
			_ph = ph;
			_sph = sph;
			_sc = sc;
			_tzh = tzh;
		}

		public ScheduleModel GetDailyData(string sportYear, string year, string timeZone, string sport)
		{
			DateTime timeAsEastern = _tzh.GetServerTime();
			var dateToQuery = (timeAsEastern.Hour <= 4) ? timeAsEastern.AddDays(-1) : timeAsEastern;
			var isFootball = sport.ToLower().Contains("football");
			var fullYearDates = _sc.CreateDateModel(year);
			var hasPostseason = _bools.CheckSportYearAttributesBool(sportYear, "hasPostseason");

			var weeklyModel = new ScheduleModel
			{
				FlexScheduleLink = _ph.CheckForFlexSchedule(year),
				TelevisedGamesList = FormatTelevisedGames(dateToQuery, year, timeZone, sport),
				WeekDates = new WeekDates
				{
					CurrentDate = new DateTime(dateToQuery.Year, dateToQuery.Month, dateToQuery.Day, 0, 0, 0),
					StartDate = new DateTime(dateToQuery.Year, dateToQuery.Month, dateToQuery.Day, 0, 0, 0),
				},
				SportYear = sportYear,
				IsFootball = isFootball
			};
			weeklyModel.WeekDates.EndDate = weeklyModel.WeekDates.StartDate.AddDays(1).AddHours(5);
			weeklyModel.Week = weeklyModel.TelevisedGamesList[0].Week;
			var week = Convert.ToInt32(weeklyModel.Week);

			var isBowlWeekOrNIT = _ph.CheckIfBowlWeekOrNIT(week, fullYearDates);
			weeklyModel.IsBowlWeek = isFootball && isBowlWeekOrNIT;
			weeklyModel.IsBasketballPostseason = !isFootball && hasPostseason && _ph.CheckIfBasketballPostseason(week, fullYearDates);
			weeklyModel.IsNIT = !isFootball && hasPostseason && isBowlWeekOrNIT;
			weeklyModel.ShowRSNPartialView = CheckForPartialView(week, sportYear);
			return weeklyModel;
		}

		private List<TelevisedGame> FormatTelevisedGames(DateTime dateToQuery, string year, string timeZone, string sport)
		{
			var startdate = new DateTime(dateToQuery.Year, dateToQuery.Month, dateToQuery.Day, 0, 0, 0);
			var enddate = startdate.AddDays(1).AddHours(5);
			List<TelevisedGame> televisedGamesList;

			var parmList = new StoredProcParmList
			{
				StoredProcParms = new List<StoredProcParm> {
					new StoredProcParm {ParmName = "@StartDate", ParmValue = startdate.ToString()},
					new StoredProcParm {ParmName = "@EndDate", ParmValue = enddate.ToString()},
					new StoredProcParm {ParmName = "@Season", ParmValue = year},
					new StoredProcParm {ParmName = "@Sport", ParmValue = sport},
				}
			};
			using (var conn = new SqlConnection(Constants.ConnString))
			{
				using (SqlDataReader resultSet = _sph.RunDataReader(parmList, conn, "GetDailyTVGames"))
				{
					televisedGamesList = FillGamesList(0, year, timeZone, sport, false, resultSet, true);
				}
			}
			return televisedGamesList;
		}

	}
}