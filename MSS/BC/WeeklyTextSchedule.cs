﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MSS.Models;
using MSS.Shared;

namespace MSS.BC
{
    public class WeeklyTextSchedule : IWeeklyTextSchedule
    {
        IBools _bools;
        IPageHelper _ph;
        ISeasonContents _sc;
        IStoredProcHelper _sph;
        ITimeZoneHelper _tzh;

        public WeeklyTextSchedule(IBools bools, IPageHelper ph, ISeasonContents sc, IStoredProcHelper sph, ITimeZoneHelper tzh)
        {
            _bools = bools;
            _ph = ph;
            _sc = sc;
            _sph = sph;
            _tzh = tzh;
        }

        public WeeklyModel GetWeeklyTextData(int week, string sportYear, string year, string sport, string timeZone)
        {
			var fullYearDates = _sc.CreateDateModel(year);
			var isBowlWeekOrNIT = _ph.CheckIfBowlWeekOrNIT(week, fullYearDates);
        	var isFootball = sport.Contains("football");
        	var hasPostseason = _bools.CheckSportYearAttributesBool(sportYear, "hasPostseason");

			var textModel = new WeeklyModel
								{
									TelevisedGamesList = CreateGamesList(timeZone, week, year, sport),
									Week = week.ToString(),
									SportYear = sportYear,
									IsBowlWeek = isFootball && isBowlWeekOrNIT,
                                    IsBasketballPostseason = !isFootball && hasPostseason && _ph.CheckIfBasketballPostseason(week, fullYearDates),
									IsNIT = !isFootball && isBowlWeekOrNIT
								};
            
            return textModel;
        }

        private List<TelevisedGame> CreateGamesList(string timeZone, int week, string year, string sport)
        {
            var gameList = new List<TelevisedGame>();
            var parmList = new StoredProcParmList
            {
                StoredProcParms = new List<StoredProcParm> { 
                    new StoredProcParm {ParmName = "@Week", ParmValue = week.ToString()},
                    new StoredProcParm {ParmName = "@Sport", ParmValue = sport},
                    new StoredProcParm {ParmName = "@Season", ParmValue = year},
                }
            };
	        using (var conn = new SqlConnection(Constants.ConnString))
	        using (SqlDataReader resultSet = _sph.RunDataReader(parmList, conn, "GetTVGames"))
	        {
		        bool ppvExists = PPVColumnExists(resultSet);
		        while (resultSet.Read())
		        {
			        gameList.Add(new TelevisedGame
			        {
				        Game = resultSet["Game"].ToString(),
				        Network = resultSet["Network"].ToString(),
				        PPV =
					        ppvExists && _bools.IsESPNPPV(resultSet["PPV"].ToString(), resultSet["CoverageNotes"].ToString())
						        ? "X"
						        : string.Empty,
				        TimeString =
					        Convert.ToDateTime(resultSet["Time"].ToString()).TimeOfDay.ToString() == "00:00:00"
						        ? "TBA"
						        : String.Format("{0:g}", _tzh.Offset(timeZone, Convert.ToDateTime(resultSet["Time"].ToString())))
			        });
		        }
	        }
	        return gameList;
        }

        private bool PPVColumnExists(SqlDataReader resultSet)
        {
            for (int i = 0; i < resultSet.FieldCount; i++)
                if (resultSet.GetName(i).Equals("PPV"))
                    return true;
            return false;
        }
    }
}