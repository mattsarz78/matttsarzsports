using System.Web.Mvc;

namespace New_MSS.Controllers
{
    public class BaseController : Controller
    {
		public string GetYear(string sportYear)
		{
			return sportYear.ToLower().Contains("football") ? sportYear.Substring(8, 4) : sportYear.Substring(10, 6);
		}

		public string GetSport(string sportYear)
		{
			return sportYear.ToLower().Contains("football") ? "football" : "basketball";
		}

		public string AddSpaces(string conference)
		{
            if (conference.Substring(0,3).Contains("Big") || conference.Substring(0,3).Contains("Sun"))
            {
                conference = conference.Replace(conference.Substring(0,3), conference.Substring(0,3) +" ");
            }
			return conference;
		}

    }
}
