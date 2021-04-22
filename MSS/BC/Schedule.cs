using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MSS.Models;
using MSS.Shared;

namespace MSS.BC
{
	public class Schedule
	{
		readonly ICoverageNotesHelper _cnh;
		readonly IStoredProcHelper _sph;
		readonly ITimeZoneHelper _tzh;
        readonly IBools _bools;

        public Schedule(ICoverageNotesHelper cnh, IStoredProcHelper sph, ITimeZoneHelper tzh, IBools bools)
		{
			_cnh = cnh;
			_sph = sph;
			_tzh = tzh;
            _bools = bools;
		}

        public class FSNGames
		{
			public string Game { get; set; }
			public string Parm { get; set; }
		}

		public string FormatRSNLink(int week, string sportYear, FSNGames parm)
		{
			return string.Format("<a class=\"FSNLink {0}week{1}{2}\" >RSN Affiliates</a>", sportYear, week, parm.Parm);
		}

		public bool CheckForPartialView(int week, string sportYear)
		{
			return File.Exists(HttpContext.Current.Server.MapPath("/Views/Schedule/CoverageNotes/" + sportYear + "/FSNWeek" + week.ToString() + ".cshtml"));
		}

		public DateTime FormatTime(DateTime gameTime, string timeZone)
		{
			return gameTime.TimeOfDay.ToString() == "00:00:00" ? gameTime : _tzh.Offset(timeZone, gameTime);
		}

		public List<FSNGames> GetFSNGamesList(string year)
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
						FSNGamesList.Add(new FSNGames
						{
							Game = resultSet["Game"].ToString(),
							Parm = resultSet["KeyValue"].ToString()
						});
					}
				}
			}
			return FSNGamesList;
		}

		public List<TelevisedGame> FillGamesList(int week, string year, string timeZone, string sport, bool showPPVColumn, 
			SqlDataReader resultSet, bool useWeek)
		{
			var televisedGamesList = new List<TelevisedGame>();
			var rsnGames = GetFSNGamesList(year);
			while (resultSet.Read())
			{
				DateTime gameTime = Convert.ToDateTime(resultSet["Time"].ToString());
				var tvGame = new TelevisedGame
				{
					GameTitle = resultSet[Constants.GAMETITLE].ToString(),
					VisitingTeam = resultSet[Constants.VISITINGTEAM].ToString(),
					HomeTeam = resultSet[Constants.HOMETEAM].ToString(),
					Location = resultSet[Constants.LOCATION].ToString(),
					PPV = _cnh.FormatCoverageNotes(year, resultSet[Constants.PPV].ToString()),
					Time = FormatTime(gameTime, timeZone),
					TimeString = _tzh.FormatTelevisedTime(gameTime, "web", timeZone),
					ShowPPVColumn = showPPVColumn,
					Mediaindicator = resultSet[Constants.MEDIAINDICATOR].ToString(),
					Week = useWeek ? resultSet[Constants.WEEK].ToString() : week.ToString()
				};

				tvGame.Network = tvGame.Mediaindicator == "W"
					? _cnh.FormatCoverageNotes(year, resultSet["NetworkJPG"].ToString())
					: _cnh.FormatNetworkJpg(resultSet["NetworkJPG"].ToString());

                IEnumerable<FSNGames> parmValue;
                if (sport == "football" || !_bools.isConferenceTournament(sport, tvGame.Game))
                {
                    parmValue = rsnGames.Where(x => tvGame.Game.Trim().Equals(x.Game));
                }
                else
                {
                    parmValue = rsnGames.Where(x => tvGame.Game.Trim().StartsWith(x.Game));
                }

                if (parmValue.Any())
				{
                    if (parmValue.Count() > 1) {
                        parmValue = parmValue.Where(x => tvGame.Game.Trim().Equals(x.Game));
                    }
					tvGame.CoverageNotes = FormatRSNLink(Convert.ToInt16(tvGame.Week), string.Format("{0}{1}", sport, year), parmValue.First());
					string additionalNotes = _cnh.FormatCoverageNotes(year, resultSet["CoverageNotes"].ToString());
					if (additionalNotes != "<label>&nbsp</label>")
					{
						var sb = new StringBuilder();
						sb.Append(additionalNotes);
						sb.Append(String.Format("<br>{0}", tvGame.CoverageNotes));
						tvGame.CoverageNotes = sb.ToString();
					}
				}
				else
					tvGame.CoverageNotes = _cnh.FormatCoverageNotes(year, resultSet["CoverageNotes"].ToString());

				televisedGamesList.Add(tvGame);
			}
			return televisedGamesList;
		}

        private bool isConferenceTournament()
        {
            throw new NotImplementedException();
        }
    }
}