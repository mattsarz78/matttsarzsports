using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using New_MSS.Models;
using New_MSS.Shared;
using System.IO;
using System.Web;

namespace New_MSS.BC
{
    public class WeeklySchedule : IWeeklySchedule
    {
        IBools _bools;
        IPageHelper _ph;
        ICoverageNotesHelper _cnh;
        IStoredProcHelper _sph;
        ISeasonContents _sc;
        ITimeZoneHelper _tzh;

        public WeeklySchedule(IBools bools, IPageHelper ph, ICoverageNotesHelper cnh, IStoredProcHelper sph, ISeasonContents sc, ITimeZoneHelper tzh)
        {
            _bools = bools;
            _ph = ph;
            _cnh = cnh;
            _sph = sph;
            _sc = sc;
            _tzh = tzh;
        }

        private class FSNGames
        {
            public string Game { get; set; }
            public string Parm { get; set; }
        }

        public WeeklyModel GetWeeklyData(int week, string sportYear, string year, string timeZone, string sport)
        {
        	var showTVPartialView = _bools.CheckSportYearAttributesBool(sportYear, "hasNoTVGames");
            var showPPVColumn = _bools.CheckSportYearAttributesBool(sportYear, "showPPVColumn");
        	var isFootball = sport.ToLower().Contains("football");
			var fullYearDates = _sc.CreateDateModel(year);
			var isBowlWeekOrNIT = _ph.CheckIfBowlWeekOrNIT(week, fullYearDates);
			var isNextWeekBowlWeekOrNIT = _ph.CheckIfBowlWeekOrNIT(week + 1, fullYearDates);
            var hasPostseason = _bools.CheckSportYearAttributesBool(sportYear, "hasPostseason");
			
			var weeklyFootballModel = new WeeklyModel
                                      	{
                                      		TimeZoneList = _tzh.CreateTimeZoneList(timeZone),
											Week = week.ToString(),
											SportYear = sportYear,
											Year = year,
											FlexScheduleLink = _ph.CheckForFlexSchedule(year),
											ShowRSNPartialView = CheckForPartialView(week, sportYear),
											ShowPPVColumn = showPPVColumn,
											WeekDates = GetWeekDates(week, year),
											IsFootball = isFootball,
											ShowNoTVPartialView = showTVPartialView,
											NoTVGameList = showTVPartialView ? FormatNoTvGames(week, year) : new List<NonTelevisedGame>(),
											TelevisedGamesList = FormatTelevisedGames(week, year, timeZone, sport, showPPVColumn),
											IsBowlWeek = isFootball && isBowlWeekOrNIT,
											IsNextWeekBowlWeek = isFootball && isNextWeekBowlWeekOrNIT,
											IsBasketballPostseason = !isFootball && hasPostseason && _ph.CheckIfBasketballPostseason(week, fullYearDates),
											IsNextWeekBasketballPostseason = !isFootball && hasPostseason && _ph.CheckIfBasketballPostseason(week + 1, fullYearDates),
											IsNIT = !isFootball && hasPostseason && isBowlWeekOrNIT
                                      	};

            return weeklyFootballModel;
        }
        
        private bool CheckForPartialView(int week, string sportYear)
        {
            return File.Exists(HttpContext.Current.Server.MapPath("/Views/Schedule/CoverageNotes/" + sportYear + "/FSNWeek" + week.ToString() + ".cshtml"));
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
			{
                using (SqlDataReader resultSet = _sph.RunDataReader(parmList, conn, "GetWeeklyDates"))
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

		
		private List<TelevisedGame> FormatTelevisedGames(int week, string year, string timeZone, string sport, bool showPPVColumn)
        {
            var televisedGamesList = new List<TelevisedGame>();

            var FSNGamesList = GetFSNGamesList(year);

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
                    while (resultSet.Read())
                    {
                        DateTime gameTime = Convert.ToDateTime(resultSet["Time"].ToString());
                        var tvGame = new TelevisedGame
                        {
                            Game = resultSet["Game"].ToString(),
                            PPV = _cnh.FormatCoverageNotes(resultSet["PPV"].ToString()),
                            Time = FormatTime(gameTime, timeZone),
                            TimeString = _tzh.FormatTelevisedTime(gameTime, "web", timeZone),
                            ShowPPVColumn = showPPVColumn,
							Mediaindicator = sport.Contains("football") ? resultSet["Mediaindicator"].ToString() : string.Empty,
                        };

                        tvGame.Network = tvGame.Mediaindicator == "W" ? _cnh.FormatCoverageNotes(resultSet["NetworkJPG"].ToString()) :
                            _cnh.FormatNetworkJpg(resultSet["NetworkJPG"].ToString());

                        var parmValue = FSNGamesList.Where(x => x.Game == tvGame.Game.Trim());
                        if (parmValue.Any())
                        {
                            tvGame.CoverageNotes = FormatRSNLink(week, String.Concat(sport, year), parmValue.First());
                            string additionalNotes = _cnh.FormatCoverageNotes(resultSet["CoverageNotes"].ToString());
                            if (additionalNotes != "<label>&nbsp</label>")
                            {
                                var sb = new StringBuilder();
                                sb.Append(additionalNotes);
                                sb.Append(String.Concat("<br>", tvGame.CoverageNotes));
                                tvGame.CoverageNotes = sb.ToString();
                            }
                        }
                        else
                            tvGame.CoverageNotes = _cnh.FormatCoverageNotes(resultSet["CoverageNotes"].ToString());

                        televisedGamesList.Add(tvGame);
                    }
                }
            }
            return televisedGamesList;
        }

        private string FormatRSNLink(int week, string sportYear, FSNGames parm)
        {
            return String.Concat("<a class=\"FSNLink ", sportYear, "week", week, parm.Parm, "\" >RSN Affiliates</a>");
        }


        private List<FSNGames> GetFSNGamesList(string year)
        {
            var FSNGamesList = new List<FSNGames>();
            using (var conn = new SqlConnection(Constants.ConnString))
            {
                using (SqlDataReader resultSet = _sph.RunDataReader(
                    new StoredProcParmList
                    {
                        StoredProcParms = new List<StoredProcParm> { new StoredProcParm { ParmName = "@Season", ParmValue = year } }
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

        private DateTime FormatTime(DateTime gameTime, string timeZone)
        {
            return gameTime.TimeOfDay.ToString() == "00:00:00" ? gameTime : _tzh.Offset(timeZone, gameTime);
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