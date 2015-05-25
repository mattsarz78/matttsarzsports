using MSS.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MSS.Controllers
{
    public class ContractController : Controller
    {
        public async Task<ActionResult> GameList(string conference, int year)
        {
            var client = new HttpClient();
            var urlHost = "http://" + Request.Url.Authority + @"/api/BaseApi/GetGameList/" + conference + @"/" + year;
            var response = await client.GetAsync(urlHost);
            var data = await response.Content.ReadAsAsync<ConferenceModel>();
            return View(data);
        }
    }
}