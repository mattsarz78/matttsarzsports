using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using New_MSS.Models;
using New_MSS.Shared;

namespace New_MSS.BC
{
    public class WeeklyTextSchedule 
    {
        public static WeeklyModel GetWeeklyTextData(int week, string sportYear, string year, string sport, string timeZone)
        {
			var fullYearDates = SeasonContents.CreateDateModel(year);
			var isBowlWeekOrNIT = PageHelper.CheckIfBowlWeekOrNIT(week, fullYearDates);
        	var isFootball = sport.Contains("football");
        	var hasPostseason = Bools.CheckSportYearAttributesBool(sportYear, "hasPostseason");

			var textModel = new WeeklyModel
								{
									TelevisedGamesList = CreateGamesList(timeZone, week, year, sport),
                                    TimeZoneList = TimeZoneHelper.CreateTimeZoneList(timeZone),
									Week = week.ToString(),
									SportYear = sportYear,
									IsBowlWeek = isFootball && isBowlWeekOrNIT,
                                    IsBasketballPostseason = !isFootball && hasPostseason && PageHelper.CheckIfBasketballPostseason(week, fullYearDates),
									IsNIT = !isFootball && isBowlWeekOrNIT
								};
            
            return textModel;
        }

        private static List<TelevisedGame> CreateGamesList(string timeZone, int week, string year, string sport)
        {
            var gameList = new List<TelevisedGame>();
            var parmList = new StoredProcHelper.StoredProcParmList
            {
                StoredProcParms = new List<StoredProcHelper.StoredProcParm> { 
                    new StoredProcHelper.StoredProcParm {ParmName = "@Week", ParmValue = week.ToString()},
                    new StoredProcHelper.StoredProcParm {ParmName = "@Sport", ParmValue = sport},
                    new StoredProcHelper.StoredProcParm {ParmName = "@Season", ParmValue = year},
                }
            };
            using (var conn = new SqlConnection(Constants.ConnString))
            {
                using (SqlDataReader resultSet = StoredProcHelper.RunDataReader(parmList, conn, "GetTVGames"))
                {
                    bool ppvExists = PPVColumnExists(resultSet);
                    while (resultSet.Read())
                    {
                        var game = new TelevisedGame { 
							Game = resultSet["Game"].ToString(), 
							Network = resultSet["Network"].ToString(),
							PPV = ppvExists && Bools.IsESPNPPV(resultSet["PPV"].ToString(), resultSet["CoverageNotes"].ToString()) ? "X" : string.Empty,
							TimeString = Convert.ToDateTime(resultSet["Time"].ToString()).TimeOfDay.ToString() == "00:00:00" ? "TBA" : String.Format("{0:g}", TimeZoneHelper.Offset(timeZone, Convert.ToDateTime(resultSet["Time"].ToString())))
						};
                        gameList.Add(game);
                    }
                }
            }
            return gameList;
        }

        private static bool PPVColumnExists(SqlDataReader resultSet)
        {
            for (int i = 0; i < resultSet.FieldCount; i++)
            {
                if (resultSet.GetName(i).Equals("PPV"))
                    return true;
            }
            return false;
        }
    }
}