using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MSS.Models;
using MSS.Shared;

namespace MSS.BC
{
    public class ConferenceSchedule : IConferenceSchedule
    {
        ICoverageNotesHelper _cnh;
        IStoredProcHelper _sph;
        ITimeZoneHelper _tzh;
        IPageHelper _ph;

        public ConferenceSchedule(ICoverageNotesHelper cnh, IStoredProcHelper sph, ITimeZoneHelper tzh, IPageHelper ph)
        {
            _cnh = cnh;
            _sph = sph;
            _tzh = tzh;
            _ph = ph;
        }

        public List<ConfGame> CreateIndependentsGameList(int year)
        {
			var independentsList = _ph.CheckSportYearAttributes(string.Format("football{0}", year), "independents").Split(',').ToList();
        	var indyList = new List<ConfGame>();
        	foreach (var independent in independentsList)
        		indyList.AddRange(CreateConferenceGameList(independent, year.ToString()));
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
	        using (var resultSet = _sph.RunDataReader(parmList, conn, "GetConferenceGames"))
		        while (resultSet.Read())
		        {
			        confGames.Add(new ConfGame
			        {
				        Game = resultSet[Constants.GAME].ToString(),
				        Time = _tzh.FormatTelevisedTime(Convert.ToDateTime(resultSet[Constants.TIME].ToString()), "conference", "Eastern"),
				        MediaIndicator = resultSet[Constants.MEDIAINDICATOR].ToString(),
				        Network =
					        resultSet[Constants.MEDIAINDICATOR].ToString() == "W"
						        ? _cnh.FormatCoverageNotes(year, resultSet[Constants.NETWORKJPG].ToString())
						        : _cnh.FormatNetworkJpg(resultSet[Constants.NETWORKJPG].ToString()),
				        TvType = resultSet[Constants.TVTYPE].ToString(),
				        Conference = resultSet[Constants.CONFERENCE].ToString()
			        });
		        }
	        return confGames;
        }
    }
}