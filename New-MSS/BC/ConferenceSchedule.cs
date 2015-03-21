using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using New_MSS.Models;
using New_MSS.Shared;

namespace New_MSS.BC
{
    public class ConferenceSchedule : StoredProcHelper
    {
        public static List<ConfGame> CreateIndependentsGameList(int year)
        {
			var independentsList = Bools.CheckSportYearAttributes(String.Concat("football", year), "independents").Split(Convert.ToChar(",")).ToList();
        	var indyList = new List<ConfGame>();
        	foreach (var independent in independentsList)
        	{
        		indyList.AddRange(CreateConferenceGameList(independent, year.ToString()));
        	}
            
            return indyList;
        }

        public static List<ConfGame> CreateConferenceGameList(string conference, string year)
        {
            var confGames = new List<ConfGame>();
            var parmList = new StoredProcParmList
            {
                StoredProcParms = new List<StoredProcParm> { new StoredProcParm {ParmName = "@Conference", ParmValue = conference},
                                                             new StoredProcParm {ParmName = "@Season", ParmValue = year} }
            };
            using (var conn = new SqlConnection(Constants.ConnString))
            {
                using (var resultSet = RunDataReader(parmList, conn, "GetConferenceGames"))
                {
                    while (resultSet.Read())
                    {
                        var confGame = new ConfGame
                        {
                            Game = resultSet[Constants.GAME].ToString(),
                            Time = FormatTelevisedTime(Convert.ToDateTime(resultSet[Constants.TIME].ToString()), "conference", "Eastern"),
                            MediaIndicator = resultSet[Constants.MEDIAINDICATOR].ToString(),
							Network = resultSet[Constants.MEDIAINDICATOR].ToString() == "W" ? FormatCoverageNotes(resultSet[Constants.NETWORKJPG].ToString()) : FormatNetworkJpg(resultSet[Constants.NETWORKJPG].ToString()),
							TvType = resultSet[Constants.MEDIAINDICATOR].ToString() == "T" ? resultSet[Constants.TVTYPE].ToString() : string.Empty,
							Conference = resultSet[Constants.CONFERENCE].ToString()
                        };
                        confGames.Add(confGame);
                    }
                }
            }
            return confGames;
        }
    }
}