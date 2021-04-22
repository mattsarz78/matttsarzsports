using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MSS.Models;
using MSS.Shared;

namespace MSS.BC
{
	public class WeeklySchedule : Schedule, IWeeklySchedule
	{
		readonly IBools _bools;
		readonly IPageHelper _ph;
		readonly IStoredProcHelper _sph;
		readonly ISeasonContents _sc;

		public WeeklySchedule(IBools bools, IPageHelper ph, ICoverageNotesHelper cnh, IStoredProcHelper sph, ISeasonContents sc, ITimeZoneHelper tzh) 
			: base(cnh, sph, tzh, bools)
		{
			_bools = bools;
			_ph = ph;
			_sph = sph;
			_sc = sc;
		}

		public ScheduleModel GetWeeklyData(int week, string sportYear, string year, string timeZone, string sport)
		{
			var showTVPartialView = _bools.CheckSportYearAttributesBool(sportYear, "hasNoTVGames");
			var showPPVColumn = _bools.CheckSportYearAttributesBool(sportYear, "showPPVColumn");
			var isFootball = sport.ToLower().Contains("football");
			var fullYearDates = _sc.CreateDateModel(year);
			var isBowlWeek = _bools.CheckIfBowlWeek(week, fullYearDates);
			var isNextWeekBowlWeekOrNIT = _bools.CheckIfBowlWeek(week + 1, fullYearDates);
			var isFirstWeek = _bools.CheckIfFirstWeek(week, fullYearDates);
			var hasPostseason = _bools.CheckSportYearAttributesBool(sportYear, "hasPostseason");

			var weeklyFootballModel = new ScheduleModel
			{
				Week = week.ToString(),
				SportYear = sportYear,
				Year = year,
				FlexScheduleLink = _ph.CheckForFlexSchedule(year),
				ShowRSNPartialView = CheckForPartialView(week, sportYear),
				ShowPPVColumn = showPPVColumn,
				WeekDates = GetWeekDates(week, year),
				IsFootball = isFootball,
				IsFirstWeek = isFirstWeek,
				ShowNoTVPartialView = showTVPartialView,
				NoTVGameList = showTVPartialView ? FormatNoTvGames(week, year) : new List<NonTelevisedGame>(),
				TelevisedGamesList = FormatTelevisedGames(week, year, timeZone, sport, showPPVColumn),
				IsBowlWeek = isFootball && isBowlWeek,
				IsNextWeekBowlWeek = isFootball && isNextWeekBowlWeekOrNIT,
				IsBasketballPostseason = !isFootball && hasPostseason && _bools.CheckIfBasketballPostseason(week, fullYearDates),
				IsNextWeekBasketballPostseason = !isFootball && hasPostseason && _bools.CheckIfBasketballPostseason(week + 1, fullYearDates),
				IsNIT = !isFootball && hasPostseason && _bools.CheckIfNIT(week, fullYearDates),
				IsOtherMBKEvent = _bools.CheckIfOtherMBKTourney(week, fullYearDates)
		};

			return weeklyFootballModel;
		}

		private WeekDates GetWeekDates(int week, string year)
		{
			var weekDates = new WeekDates();
			var parmList = new StoredProcParmList
			{
				StoredProcParms = new List<StoredProcParm>
														 {
															 new StoredProcParm {ParmName = "@Week", ParmValue = week.ToString()},
														new StoredProcParm {ParmName = "@Season", ParmValue = year}
														 }
			};
			using (var conn = new SqlConnection(Constants.ConnString))
			using (SqlDataReader resultSet = _sph.RunDataReader(parmList, conn, "GetWeeklyDates"))
				while (resultSet.Read())
				{
					weekDates.StartDate = Convert.ToDateTime(resultSet["StartDate"]);
					weekDates.EndDate = Convert.ToDateTime(resultSet["EndDate"]);
					weekDates.PostseasonInd = resultSet["PostseasonInd"].ToString();
				}
			return weekDates;
		}

		private List<TelevisedGame> FormatTelevisedGames(int week, string year, string timeZone, string sport, bool showPPVColumn)
		{
			List<TelevisedGame> televisedGamesList;

			var parmList = new StoredProcParmList
			{
				StoredProcParms = new List<StoredProcParm> {
					new StoredProcParm {ParmName = "@Week", ParmValue = week.ToString()},
					new StoredProcParm {ParmName = "@Sport", ParmValue = sport},
					new StoredProcParm {ParmName = "@Season", ParmValue = year},
				}
			};
			using (var conn = new SqlConnection(Constants.ConnString))
			{
				using (SqlDataReader resultSet = _sph.RunDataReader(parmList, conn, "GetTVGames"))
				{
					televisedGamesList = FillGamesList(week, year, timeZone, sport, showPPVColumn, 
						resultSet, false);
				}
			}
			return televisedGamesList;
		}


		private List<NonTelevisedGame> FormatNoTvGames(int week, string season)
		{
			var noTvGamesList = new List<NonTelevisedGame>();

			var parmList = new StoredProcParmList
			{
				StoredProcParms = new List<StoredProcParm>
														 {
															 new StoredProcParm {ParmName = "@Week", ParmValue = week.ToString()},
															 new StoredProcParm {ParmName = "@Season", ParmValue = season}
														 }
			};
			using (var conn = new SqlConnection(Constants.ConnString))
			{
				using (SqlDataReader resultSet = _sph.RunDataReader(parmList, conn, "GetNoTVGames"))
				{
					while (resultSet.Read())
					{
						noTvGamesList.Add(new NonTelevisedGame
						{
							Game = resultSet[Constants.GAME].ToString(),
							GameTitle = resultSet[Constants.GAMETITLE].ToString(),
							VisitingTeam = resultSet[Constants.VISITINGTEAM].ToString(),
							HomeTeam = resultSet[Constants.HOMETEAM].ToString(),
							Location = resultSet[Constants.LOCATION].ToString(),
							Conference = resultSet[Constants.CONFERENCE].ToString(),
							TVOptions = resultSet[Constants.TVOPTIONS].ToString(),
							Time = Convert.ToDateTime(resultSet[Constants.TIME].ToString()),
							FCS = resultSet[Constants.FCS].ToString() == "Y",
							DayOfWeek = Convert.ToDateTime(resultSet[Constants.TIME].ToString()).DayOfWeek
						});
					}
				}
			}
			return noTvGamesList;
		}
	}
}