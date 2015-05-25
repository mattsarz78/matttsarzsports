using MSS.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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

        private async Task<WeeklyModel> GetWeekly(int week, string sportYear, string timeZoneValue)
        {
            var client = new HttpClient();
            var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetWeekly/" + sportYear + @"/" + week + @"/" + timeZoneValue;
            var response = await client.GetAsync(urlHost);
            var data = await response.Content.ReadAsAsync<WeeklyModel>();
            return data;
        }

        private async Task<object> GetWeeklyText(int week, string sportYear, string timeZoneValue)
        {
            var client = new HttpClient();
            var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetWeeklyText/" + sportYear + @"/" + week + @"/" + timeZoneValue;
            var response = await client.GetAsync(urlHost);
            var data = await response.Content.ReadAsAsync<WeeklyModel>();
            return data;
        }
    }
}