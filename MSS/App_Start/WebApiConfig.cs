﻿using System.Web.Http;

namespace MSS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("BaseApiContract", "api/BaseApi/GetGameList/{conference}/{year}",
                new { controller = "BaseApi", action = "GetGameList", conference = string.Empty, year = 0 });

            config.Routes.MapHttpRoute("BaseApiWeekly", "api/BaseApi/GetWeekly/{sportYear}/{week}/{timeZoneValue}",
                new { controller = "BaseApi", action = "GetWeekly", sportYear = string.Empty, week = 0, timeZoneValue = string.Empty });

            config.Routes.MapHttpRoute("BaseApiWeeklyText", "api/BaseApi/GetWeeklyText/{sportYear}/{week}/{timeZoneValue}",
                new { controller = "BaseApi", action = "GetWeeklyText", sportYear = string.Empty, week = 0, timeZoneValue = string.Empty });

            config.Routes.MapHttpRoute("BaseApiContents", "api/BaseApi/GetContents/{sportYear}",
                new { controller = "BaseApi", action = "GetContents", sportYear = string.Empty });
        }
    }
}