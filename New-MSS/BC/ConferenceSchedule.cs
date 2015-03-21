using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using New_MSS.Models;
using New_MSS.Shared;

namespace New_MSS.BC
{
    public class ConferenceSchedule : IConferenceSchedule
    {
        IBools _bools;
        ICoverageNotesHelper _cnh;
        IStoredProcHelper _sph;
        ITimeZoneHelper _tzh;
        
        public ConferenceSchedule(IBools bools, ICoverageNotesHelper cnh, IStoredProcHelper sph, ITimeZoneHelper tzh)
        {
            _bools = bools;
            _cnh = cnh;
            _sph = sph;
            _tzh = tzh;
        }

        public List<ConfGame> CreateIndependentsGameList(int year)
        {
			var independentsList = _bools.CheckSportYearAttributes(String.Concat("football", year), "independents").Split(Convert.ToChar(",")).ToList();
        	var indyList = new List<ConfGame>();
        	foreach (var independent in independentsList)
        	{
        		indyList.AddRange(CreateConferenceGameList(independent, year.ToString()));
        	}
            
            return indyList;
        }

        public List<ConfGame> CreateConferenceGameList(string conference, string year)
        {
            var confGames = new List<ConfGame>();
            var parmList = new StoredProcParmList
            {
                StoredProcParms = new List<StoredProcParm> { new StoredProcParm {ParmName = "@Conference", ParmValue = conference},
                                                             new StoredProcParm {ParmName = "@Season", ParmValue = year} }
            };
            using (var conn = new SqlConnection(Constants.ConnString))
            {
                using (var resultSet = _sph.RunDataReader(parmList, conn, "GetConferenceGames"))
                {
                    while (resultSet.Read())
                    {
                        var confGame = new ConfGame
                        {
                            Game = resultSet[Constants.GAME].ToString(),
                            Time = _tzh.FormatTelevisedTime(Convert.ToDateTime(resultSet[Constants.TIME].ToString()), "conference", "Eastern"),
                            MediaIndicator = resultSet[Constants.MEDIAINDICATOR].ToString(),
							Network = resultSet[Constants.MEDIAINDICATOR].ToString() == "W" ? _cnh.FormatCoverageNotes(resultSet[Constants.NETWORKJPG].ToString()) : _cnh.FormatNetworkJpg(resultSet[Constants.NETWORKJPG].ToString()),
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