﻿using System;
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

		public Schedule(ICoverageNotesHelper cnh, IStoredProcHelper sph, ITimeZoneHelper tzh)
		{
			_cnh = cnh;
			_sph = sph;
			_tzh = tzh;
		}
		public class FSNGames
		{
			public string Game { get; set; }
			public string Parm { get; set; }
		}

		public string FormatRSNLink(int week, string sportYear, FSNGames parm)
		{
			return String.Concat("<a class=\"FSNLink ", sportYear, "week", week, parm.Parm, "\" >RSN Affiliates</a>");
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
					Game = resultSet["Game"].ToString(),
					PPV = _cnh.FormatCoverageNotes(resultSet["PPV"].ToString()),
					Time = FormatTime(gameTime, timeZone),
					TimeString = _tzh.FormatTelevisedTime(gameTime, "web", timeZone),
					ShowPPVColumn = showPPVColumn,
					Mediaindicator = resultSet["Mediaindicator"].ToString(),
					Week = useWeek ? resultSet["Week"].ToString() : week.ToString()
				};

				tvGame.Network = tvGame.Mediaindicator == "W"
					? _cnh.FormatCoverageNotes(resultSet["NetworkJPG"].ToString())
					: _cnh.FormatNetworkJpg(resultSet["NetworkJPG"].ToString());

				var parmValue = rsnGames.Where(x => x.Game == tvGame.Game.Trim());
				if (parmValue.Any())
				{
					tvGame.CoverageNotes = FormatRSNLink(Convert.ToInt16(tvGame.Week), String.Concat(sport, year), parmValue.First());
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
			return televisedGamesList;
		}

	}
}