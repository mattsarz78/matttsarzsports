using System.Collections.Generic;
using System.Data.SqlClient;
using MSS.Models;
using MSS.Shared;

namespace MSS.BC
{
    public class WeeklyTextSchedule : TextSchedule, IWeeklyTextSchedule
    {
        readonly IBools _bools;
        readonly ISeasonContents _sc;
        readonly IStoredProcHelper _sph;

        public WeeklyTextSchedule(IBools bools, ISeasonContents sc, IStoredProcHelper sph, ITimeZoneHelper tzh)
            : base(bools, tzh)
        {
            _bools = bools;
            _sc = sc;
            _sph = sph;
        }

        public ScheduleModel GetWeeklyTextData(int week, string sportYear, string year, string sport, string timeZone)
        {
            var fullYearDates = _sc.CreateDateModel(year);
            var isBowlWeek = _bools.CheckIfBowlWeek(week, fullYearDates);
            var isFootball = sport.Contains("football");
            var hasPostseason = _bools.CheckSportYearAttributesBool(sportYear, "hasPostseason");

            var textModel = new ScheduleModel
            {
                TelevisedGamesList = CreateGamesList(timeZone, week, year, sport),
                Week = week.ToString(),
                SportYear = sportYear,
                Year = year,
                IsBowlWeek = isFootball && isBowlWeek,
                IsBasketballPostseason = !isFootball && hasPostseason && _bools.CheckIfBasketballPostseason(week, fullYearDates),
                IsNIT = !isFootball && _bools.CheckIfNIT(week, fullYearDates)
            };

            return textModel;
        }

        private List<TelevisedGame> CreateGamesList(string timeZone, int week, string year, string sport)
        {
            List<TelevisedGame> gameList;
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
                gameList = FillGameList(timeZone, resultSet, false);
            }
            return gameList;
        }
    }
}