using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using New_MSS.Models;
using New_MSS.Shared;

namespace New_MSS.BC
{
    public class SeasonContents : StoredProcHelper
    {
		public static List<YearDate> CreateDateModel(string season)
        {
            var yearDateList = new List<YearDate>();
            var parmList = new StoredProcParmList { StoredProcParms = new List<StoredProcParm> { 
                new StoredProcParm { ParmName = "@Season", ParmValue = season }
            }};
            using (var conn = new SqlConnection(Constants.ConnString))
            {
                using (var resultSet = RunDataReader(parmList, conn, "GetFullYearDates"))
                {
                    while (resultSet.Read())
                    {
                        var yearDate = new YearDate
                        {
                            Week = Int32.Parse(resultSet["Week"].ToString()),
                            StartDate = DateTime.Parse(resultSet["StartDate"].ToString()),
                            EndDate = DateTime.Parse(resultSet["EndDate"].ToString())
                        };

                        yearDateList.Add(yearDate);
                    }
                }
            }
            return yearDateList;
        }

        public static string CreateTitle(string sportYear)
        {
            return sportYear.ToLower().Contains("football")
                       ? String.Concat(sportYear.Substring(8, 4)," Football")
                       : String.Concat(sportYear.Substring(10, 4),"-",sportYear.Substring(14, 2)," Basketball");
        }
    }
}