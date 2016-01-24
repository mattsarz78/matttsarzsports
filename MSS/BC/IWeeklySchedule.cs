using MSS.Models;

namespace MSS.BC
{
    public interface IWeeklySchedule
    {
        WeeklyModel GetWeeklyData(int week, string sportYear, string year, string timeZone, string sport);
		WeeklyModel GetDailyData(string sportYear, string year, string timeZone, string sport);
	}
}
