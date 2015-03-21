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
			var expanded = conference;
			if (conference.StartsWith("Big"))
				expanded = conference.Replace("Big", "Big ");

			if (conference.StartsWith("Sun"))
				expanded = conference.Replace("Sun", "Sun ");
			return expanded;
		}

    }
}
