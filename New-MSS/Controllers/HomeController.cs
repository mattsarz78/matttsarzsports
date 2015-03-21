using System.Web.Mvc;

namespace New_MSS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Archive()
		{
			return View();
		}

		public ActionResult CopyrightAndDisclaimer()
		{
			return View();
		}
	}
}
