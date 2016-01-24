using System.Web.Mvc;
using System.Web.Routing;

namespace MSS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Home", "{controller}/{action}", new { controller = "Home", action = "Index" });
            routes.MapRoute("Archive", "{controller}/{action}", new { controller = "Home", action = "Archive" });
            routes.MapRoute("ContractGameList", "Contract/GameList/{conference}/{year}",
                new { controller = "Contract", action = "GameList", conference = string.Empty, year = 0 });
            routes.MapRoute("WeeklySchedule", "Schedule/Weekly/{sportYear}/{week}",
                new { controller = "Schedule", action = "Weekly", sportYear = string.Empty, week = 0 });
            routes.MapRoute("WeeklyTextSchedule", "Schedule/WeeklyText/{sportYear}/{week}",
                new { controller = "Schedule", action = "WeeklyText", sportYear = string.Empty, week = 0 });
            routes.MapRoute("ChangeTimeZone", "Schedule/Weekly/{timeZoneValue}/{sportYear}/{week}",
                new { controller = "Schedule", action = "Weekly", timeZoneValue = string.Empty, sportYear = string.Empty, week = 0 });
            routes.MapRoute("ChangeTimeZoneText", "Schedule/WeeklyText/{timeZoneValue}/{sportYear}/{week}",
                new { controller = "Schedule", action = "WeeklyText", timeZoneValue = string.Empty, sportYear = string.Empty, week = 0 });
			routes.MapRoute("DailySchedule", "Schedule/Daily/{sportYear}",
				new { controller = "Schedule", action = "Daily", sportYear = string.Empty });
			routes.MapRoute("DailyTextSchedule", "Schedule/DailyText/{sportYear}",
				new { controller = "Schedule", action = "DailyText", sportYear = string.Empty });
			routes.MapRoute("ChangeTimeZoneDaily", "Schedule/Daily/{timeZoneValue}/{sportYear}",
				new { controller = "Schedule", action = "Daily", timeZoneValue = string.Empty, sportYear = string.Empty });
			routes.MapRoute("ChangeTimeZoneDailyText", "Schedule/DailyText/{timeZoneValue}/{sportYear}",
				new { controller = "Schedule", action = "DailyText", timeZoneValue = string.Empty, sportYear = string.Empty });
			routes.MapRoute("Season", "{controller}/{action}/{sportYear}",
                new { controller = "Season", action = "Contents", sportYear = string.Empty });
        }
    }
}
