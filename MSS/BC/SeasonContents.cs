using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MSS.Models;
using MSS.Shared;

namespace MSS.BC
{
    public class SeasonContents : ISeasonContents
    {
        IStoredProcHelper _sph;

        public SeasonContents(IStoredProcHelper sph)
        {
            _sph = sph;
        }

		public List<YearDate> CreateDateModel(string season)
        {
            var yearDateList = new List<YearDate>();
            var parmList = new StoredProcParmList { StoredProcParms = new List<StoredProcParm> { 
                new StoredProcParm { ParmName = "@Season", ParmValue = season }
            }};
			using (var conn = new SqlConnection(Constants.ConnString))
			using (var resultSet = _sph.RunDataReader(parmList, conn, "GetFullYearDates"))
				while (resultSet.Read())
				{
                    yearDateList.Add(new YearDate
                    {
                        Week = int.Parse(resultSet["Week"].ToString()),
                        StartDate = DateTime.Parse(resultSet["StartDate"].ToString()),
                        EndDate = DateTime.Parse(resultSet["EndDate"].ToString()),
						PostseasonInd = resultSet["PostseasonInd"].ToString()
                    });
				}
			return yearDateList;
        }

        public string CreateTitle(string sportYear)
        {
            return sportYear.ToLower().Contains("football")
                       ? string.Format("{0} Football", sportYear.Substring(8, 4))
                       : string.Format("{0}-{1} Basketball", sportYear.Substring(10, 4),sportYear.Substring(14, 2));

		}
    }
}