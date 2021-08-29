using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MSS.Models;
using MSS.Shared;

namespace MSS.BC
{
    public class TextSchedule
    {
	    readonly IBools _bools;
	    readonly ITimeZoneHelper _tzh;

        public TextSchedule(IBools bools, ITimeZoneHelper tzh)
        {
            _bools = bools;
            _tzh = tzh;
        }

        public bool PPVColumnExists(SqlDataReader resultSet)
        {
            for (int i = 0; i < resultSet.FieldCount; i++)
                if (resultSet.GetName(i).Equals("PPV"))
                    return true;
            return false;
        }

		public List<TelevisedGame> FillGameList(string timeZone, SqlDataReader resultSet, bool useWeek)
		{
			bool ppvExists = PPVColumnExists(resultSet);
			var gameList = new List<TelevisedGame>();
			while (resultSet.Read())
			{
				gameList.Add(new TelevisedGame
				{
					GameTitle = resultSet[Constants.GAMETITLE].ToString(),
					VisitingTeam = resultSet[Constants.VISITINGTEAM].ToString().Split(',').ToList(),
					HomeTeam = resultSet[Constants.HOMETEAM].ToString().Split(',').ToList(),
					Location = resultSet[Constants.LOCATION].ToString(),
					Network = resultSet["Network"].ToString(),
					PPV =
						ppvExists && _bools.IsESPNPPV(resultSet["PPV"].ToString(), resultSet["CoverageNotes"].ToString())
							? "X"
							: string.Empty,
					TimeString =
						Convert.ToDateTime(resultSet["Time"].ToString()).TimeOfDay.ToString() == "00:00:00"
							? "TBA"
							: String.Format("{0:g}", _tzh.Offset(timeZone, Convert.ToDateTime(resultSet["Time"].ToString()))),
					Week = useWeek ? resultSet["Week"].ToString() : string.Empty
				});
			}
			return gameList;
		}

    }
}