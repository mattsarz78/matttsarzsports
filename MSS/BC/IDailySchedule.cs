using MSS.Models;

namespace MSS.BC
{
	interface IDailySchedule
	{
		ScheduleModel GetDailyData(string sportYear, string year, string timeZone, string sport);
	}
}
