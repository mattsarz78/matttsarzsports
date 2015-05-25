using MSS.Models;

namespace MSS.BC
{
    public interface IWeeklyTextSchedule
    {
        WeeklyModel GetWeeklyTextData(int week, string sportYear, string year, string sport, string timeZone);
    }
}
