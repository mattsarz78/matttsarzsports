using MSS.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MSS.Controllers
{
    public class SeasonController : Controller
    {
        public async Task<ActionResult> Contents(string sportYear)
        {
            var client = new HttpClient();
            var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetContents/" + sportYear;
            var response = await client.GetAsync(urlHost);
            var data = await response.Content.ReadAsAsync<DateModel>();
            return View(data);
        }
    }
}