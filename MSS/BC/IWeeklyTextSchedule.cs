using MSS.Models;

namespace MSS.BC
{
    public interface IWeeklyTextSchedule
    {
	    ScheduleModel GetWeeklyTextData(int week, string sportYear, string year, string sport, string timeZone);
    }
}
