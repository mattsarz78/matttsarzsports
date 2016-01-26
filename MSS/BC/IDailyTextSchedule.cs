using MSS.Models;

namespace MSS.BC
{
    public interface IDailyTextSchedule
    {
		ScheduleModel GetDailyTextData(string sportYear, string year, string sport, string timeZone);
	}
}
