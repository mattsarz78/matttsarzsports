using MSS.Models;

namespace MSS.BC
{
    public interface IWeeklySchedule
    {
        ScheduleModel GetWeeklyData(int week, string sportYear, string year, string timeZone, string sport);
	}
}
