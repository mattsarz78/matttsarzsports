﻿using MSS.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MSS.Controllers
{
    public class ScheduleController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Weekly(int week, string sportYear)
        {
            var data = await GetWeekly(week, sportYear, "Eastern");
            return View(data);
        }

        [HttpGet]
        public async Task<ActionResult> TVWindows(string sportYear)
        {
            var data = await GetTVWindows(sportYear);
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Weekly(string timeZoneValue, int week, string sportYear)
        {
            var data = await GetWeekly(week, sportYear, timeZoneValue);
            return PartialView("WeeksBase", data);
        }

        [HttpGet]
        public async Task<ActionResult> WeeklyText(int week, string sportYear)
        {
            var data = await GetWeeklyText(week, sportYear, "Eastern");
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> WeeklyText(string timeZoneValue, int week, string sportYear)
        {
            var data = await GetWeeklyText(week, sportYear, timeZoneValue);
            return PartialView("TextGames", data);
        }

		[HttpGet]
		public async Task<ActionResult> Daily(string sportYear)
		{
			var data = await GetDaily(sportYear, "Eastern");
			return View(data);
		}

		[HttpPost]
		public async Task<ActionResult> Daily(string timeZoneValue, string sportYear)
		{
			var data = await GetDaily(sportYear, timeZoneValue);
			return PartialView("WeeksBase", data);
		}

		[HttpGet]
		public async Task<ActionResult> DailyText(string sportYear)
		{
			var data = await GetDailyText(sportYear, "Eastern");
			return View(data);
		}

		[HttpPost]
		public async Task<ActionResult> DailyText(string timeZoneValue, string sportYear)
		{
			var data = await GetDailyText(sportYear, timeZoneValue);
			return PartialView("TextGames", data);
		}

        private async Task<TVWindowsModel> GetTVWindows(string sportYear)
        {
            var client = new HttpClient();
            var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetTVWindows/" + sportYear;
            var response = await client.GetAsync(urlHost);
            var data = await response.Content.ReadAsAsync<TVWindowsModel>();
            return data;
        }

        private async Task<ScheduleModel> GetWeekly(int week, string sportYear, string timeZoneValue)
        {
            var client = new HttpClient();
            var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetWeekly/" + sportYear + @"/" + week + @"/" + timeZoneValue;
            var response = await client.GetAsync(urlHost);
            var data = await response.Content.ReadAsAsync<ScheduleModel>();
            return data;
        }

        private async Task<object> GetWeeklyText(int week, string sportYear, string timeZoneValue)
        {
            var client = new HttpClient();
            var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetWeeklyText/" + sportYear + @"/" + week + @"/" + timeZoneValue;
            var response = await client.GetAsync(urlHost);
            var data = await response.Content.ReadAsAsync<ScheduleModel>();
            return data;
        }

		private async Task<ScheduleModel> GetDaily(string sportYear, string timeZoneValue)
		{
			var client = new HttpClient();
			var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetDaily/" + sportYear + @"/" + timeZoneValue;
			var response = await client.GetAsync(urlHost);
			var data = await response.Content.ReadAsAsync<ScheduleModel>();
			return data;
		}

		private async Task<object> GetDailyText(string sportYear, string timeZoneValue)
		{
			var client = new HttpClient();
			var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetDailyText/" + sportYear + @"/" + timeZoneValue;
			var response = await client.GetAsync(urlHost);
			var data = await response.Content.ReadAsAsync<ScheduleModel>();
			return data;
		}
	}
}