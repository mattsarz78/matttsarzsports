﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MSS.Models;
using MSS.Shared;
using System.Linq;

namespace MSS.BC
{
    public class DailyTextSchedule : TextSchedule, IDailyTextSchedule
    {
	    readonly IBools _bools;
	    readonly ISeasonContents _sc;
	    readonly IStoredProcHelper _sph;
		readonly ITimeZoneHelper _tzh;

		public DailyTextSchedule(IBools bools, ISeasonContents sc, IStoredProcHelper sph, ITimeZoneHelper tzh) 
			: base(bools, tzh)
        {
            _bools = bools;
            _sc = sc;
            _sph = sph;
			_tzh = tzh;
		}

		public ScheduleModel GetDailyTextData(string sportYear, string year, string sport, string timeZone)
		{
			DateTime timeAsEastern = _tzh.GetServerTime();
			var dateToQuery = (timeAsEastern.Hour <= 4) ? DateTime.Now.AddDays(-1) : DateTime.Now;
			var fullYearDates = _sc.CreateDateModel(year);
			var isFootball = sport.Contains("football");
			var hasPostseason = _bools.CheckSportYearAttributesBool(sportYear, "hasPostseason");

			var textModel = new ScheduleModel
			{
				TelevisedGamesList = CreateGamesList(timeZone, dateToQuery, year, sport),
				SportYear = sportYear,
				WeekDates = new WeekDates
				{
					CurrentDate = new DateTime(dateToQuery.Year, dateToQuery.Month, dateToQuery.Day, 0, 0, 0)
				}
			};

			if (textModel.TelevisedGamesList.Any())
			{
				textModel.Week = textModel.TelevisedGamesList[0].Week;
				var week = Convert.ToInt32(textModel.Week);

				var isBowlWeekOrNIT = _bools.CheckIfBowlWeek(week, fullYearDates);

				textModel.IsBowlWeek = isFootball && isBowlWeekOrNIT;
				textModel.IsNIT = !isFootball && isBowlWeekOrNIT;
				textModel.IsBasketballPostseason = !isFootball && hasPostseason && _bools.CheckIfBasketballPostseason(week, fullYearDates);
			}

			return textModel;
		}

		private List<TelevisedGame> CreateGamesList(string timeZone, DateTime dateToQuery, string year, string sport)
		{
			var startdate = new DateTime(dateToQuery.Year, dateToQuery.Month, dateToQuery.Day, 0, 0, 0);
            var enddate = startdate.AddDays(1).AddHours(5);
			List<TelevisedGame> gameList;

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
			using (SqlDataReader resultSet = _sph.RunDataReader(parmList, conn, "GetDailyTVGames"))
			{
				gameList = FillGameList(timeZone, resultSet, true);
			}
			return gameList;
		}
    }
}