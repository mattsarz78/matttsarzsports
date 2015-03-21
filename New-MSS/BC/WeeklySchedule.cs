using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using New_MSS.Models;
using New_MSS.Shared;

namespace New_MSS.BC
{
    public class WeeklySchedule : PageHelper
    {
        private class FSNGames
        {
            public string Game { get; set; }
            public string Parm { get; set; }
        }

        public static WeeklyModel GetWeeklyData(int week, string sportYear, string year, string timeZone, string sport, ControllerContext controllerContext)
        {
        	var showTVPartialView = Bools.CheckSportYearAttributesBool(sportYear, "hasNoTVGames");
        	var showPPVColumn = Bools.CheckSportYearAttributesBool(sportYear, "showPPVColumn");
        	var isFootball = sport.ToLower().Contains("football");
			var fullYearDates = SeasonContents.CreateDateModel(year);
			var isBowlWeekOrNIT = CheckIfBowlWeekOrNIT(week, fullYearDates);
			var isNextWeekBowlWeekOrNIT = CheckIfBowlWeekOrNIT(week + 1, fullYearDates);
        	var hasPostseason = Bools.CheckSportYearAttributesBool(sportYear, "hasPostseason");
			
			var weeklyFootballModel = new WeeklyModel
                                      	{
                                      		TimeZoneList = TimeZoneHelper.CreateTimeZoneList(timeZone),
											Week = week.ToString(),
											SportYear = sportYear,
											Year = year,
											FlexScheduleLink = CheckForFlexSchedule(year),
											ShowRSNPartialView = CheckForPartialView(week, sportYear, controllerContext),
											ShowPPVColumn = showPPVColumn,
											WeekDates = GetWeekDates(week, year),
											IsFootball = isFootball,
											ShowNoTVPartialView = showTVPartialView,
											NoTVGameList = showTVPartialView ? FormatNoTvGames(week, year) : new List<NonTelevisedGame>(),
											TelevisedGamesList = FormatTelevisedGames(week, year, timeZone, sport, showPPVColumn),
											IsBowlWeek = isFootball && isBowlWeekOrNIT,
											IsNextWeekBowlWeek = isFootball && isNextWeekBowlWeekOrNIT,
											IsBasketballPostseason = !isFootball && hasPostseason && CheckIfBasketballPostseason(week, fullYearDates),
											IsNextWeekBasketballPostseason = !isFootball && hasPostseason && CheckIfBasketballPostseason(week + 1, fullYearDates),
											IsNIT = !isFootball && hasPostseason && isBowlWeekOrNIT
                                      	};

            return weeklyFootballModel;
        }
        
        private static bool CheckForPartialView(int week, string sportYear, ControllerContext controllerContext)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(controllerContext, "CoverageNotes/" + sportYear + "/FSNWeek" + week.ToString(), null);
            return (result.View != null);
        }

		private static WeekDates GetWeekDates(int week, string year)
		{
			var weekDates = new WeekDates();

            var parmList = new StoredProcHelper.StoredProcParmList
			{
                StoredProcParms = new List<StoredProcHelper.StoredProcParm>
    		               		                  	{
    		               		                  		new StoredProcHelper.StoredProcParm {ParmName = "@Week", ParmValue = week.ToString()},
                                                        new StoredProcHelper.StoredProcParm {ParmName = "@Season", ParmValue = year}
    		               		                  	}
			};
			using (var conn = new SqlConnection(Constants.ConnString))
			{
                using (SqlDataReader resultSet = StoredProcHelper.RunDataReader(parmList, conn, "GetWeeklyDates"))
				{
					while (resultSet.Read())
					{
						weekDates.StartDate = Convert.ToDateTime(resultSet["StartDate"]);
						weekDates.EndDate = Convert.ToDateTime(resultSet["EndDate"]);
					}
				}
			}
			return weekDates;
		}

		
		private static List<TelevisedGame> FormatTelevisedGames(int week, string year, string timeZone, string sport, bool showPPVColumn)
        {
            var televisedGamesList = new List<TelevisedGame>();

            var FSNGamesList = GetFSNGamesList(year);

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
                    while (resultSet.Read())
                    {
                        DateTime gameTime = Convert.ToDateTime(resultSet["Time"].ToString());
                        var tvGame = new TelevisedGame
                        {
                            Game = resultSet["Game"].ToString(),
                            PPV = CoverageNotesHelper.FormatCoverageNotes(resultSet["PPV"].ToString()),
                            Time = FormatTime(gameTime, timeZone),
                            TimeString = TimeZoneHelper.FormatTelevisedTime(gameTime, "web", timeZone),
                            ShowPPVColumn = showPPVColumn,
							Mediaindicator = sport.Contains("football") ? resultSet["Mediaindicator"].ToString() : string.Empty,
                        };

                        tvGame.Network = tvGame.Mediaindicator == "W" ? CoverageNotesHelper.FormatCoverageNotes(resultSet["NetworkJPG"].ToString()) :
                            CoverageNotesHelper.FormatNetworkJpg(resultSet["NetworkJPG"].ToString());

                        var parmValue = FSNGamesList.Where(x => x.Game == tvGame.Game.Trim());
                        if (parmValue.Any())
                        {
                            tvGame.CoverageNotes = FormatRSNLink(week, String.Concat(sport, year), parmValue.First());
                            string additionalNotes = CoverageNotesHelper.FormatCoverageNotes(resultSet["CoverageNotes"].ToString());
                            if (additionalNotes != "<label>&nbsp</label>")
                            {
                                var sb = new StringBuilder();
                                sb.Append(additionalNotes);
                                sb.Append(String.Concat("<br>", tvGame.CoverageNotes));
                                tvGame.CoverageNotes = sb.ToString();
                            }
                        }
                        else
                            tvGame.CoverageNotes = CoverageNotesHelper.FormatCoverageNotes(resultSet["CoverageNotes"].ToString());

                        televisedGamesList.Add(tvGame);
                    }
                }
            }
            return televisedGamesList;
        }

        private static string FormatRSNLink(int week, string sportYear, FSNGames parm)
        {
            return String.Concat("<a class=\"FSNLink ", sportYear, "week", week, parm.Parm, "\" >RSN Affiliates</a>");
        }


        private static List<FSNGames> GetFSNGamesList(string year)
        {
            var FSNGamesList = new List<FSNGames>();
            using (var conn = new SqlConnection(Constants.ConnString))
            {
                using (SqlDataReader resultSet = StoredProcHelper.RunDataReader(
                    new StoredProcHelper.StoredProcParmList
                    {
                        StoredProcParms = new List<StoredProcHelper.StoredProcParm> { new StoredProcHelper.StoredProcParm { ParmName = "@Season", ParmValue = year } }
                    },
                    conn, "GetRSNGames"))
                {
                    while (resultSet.Read())
                    {
                        var FSNGame = new FSNGames
                        {
                            Game = resultSet["Game"].ToString(),
                            Parm = resultSet["KeyValue"].ToString()
                        };
                        FSNGamesList.Add(FSNGame);
                    }
                }
            }
            return FSNGamesList;
        }

        private static DateTime FormatTime(DateTime gameTime, string timeZone)
        {
            return gameTime.TimeOfDay.ToString() == "00:00:00" ? gameTime : TimeZoneHelper.Offset(timeZone, gameTime);
        }

        private static List<NonTelevisedGame> FormatNoTvGames(int week, string season)
        {
            var noTvGamesList = new List<NonTelevisedGame>();

            var parmList = new StoredProcHelper.StoredProcParmList
            {
                StoredProcParms = new List<StoredProcHelper.StoredProcParm>
                                                         {
                                                             new StoredProcHelper.StoredProcParm {ParmName = "@Week", ParmValue = week.ToString()},
                                                             new StoredProcHelper.StoredProcParm {ParmName = "@Season", ParmValue = season}
                                                         }
            };
            using (var conn = new SqlConnection(Constants.ConnString))
            {
                using (SqlDataReader resultSet = StoredProcHelper.RunDataReader(parmList, conn, "GetNoTVGames"))
                {
                    while (resultSet.Read())
                    {
                        var noTvGame = new NonTelevisedGame
                        {
                            Game = resultSet["Game"].ToString(),
                            Conference = resultSet["Conference"].ToString(),
                            TVOptions = resultSet["TVOptions"].ToString(),
                            Time = Convert.ToDateTime(resultSet["Time"].ToString()),
                            FCS = resultSet["FCS"].ToString() == "Y",
                            DayOfWeek = Convert.ToDateTime(resultSet["Time"].ToString()).DayOfWeek
                        };
                        noTvGamesList.Add(noTvGame);
                    }
                }
            }
            return noTvGamesList;
        }
    }
}