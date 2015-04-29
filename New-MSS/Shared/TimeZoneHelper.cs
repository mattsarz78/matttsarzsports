using System;

namespace New_MSS.Shared
{
    public class TimeZoneHelper : ITimeZoneHelper 
    {
		public DateTime Offset(string timeZone, DateTime gameTime)
		{
            const string st = " Standard Time";
			var sourceTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern" + st);
            var destinationTimeZone = "Eastern" + st;
		    switch (timeZone)
            {
                case "Central": destinationTimeZone = "Central" + st; break;
                case "Mountain": destinationTimeZone = "Mountain" + st; break;
                case "Arizona": destinationTimeZone = "US Mountain" + st; break;
                case "Pacific": destinationTimeZone = "Pacific" + st; break;
                case "Alaska": destinationTimeZone = "Alaskan" + st; break;
                case "Hawai'i": destinationTimeZone = "Hawaiian" + st; break;
            }
			return TimeZoneInfo.ConvertTime(gameTime, sourceTimeZoneInfo, TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZone));
		}

        public string FormatTelevisedTime(DateTime gameTimeInput, string caller, string timeZone)
        {
            string gameTimeString;

            DateTime gameTime = DateTime.Parse(gameTimeInput.ToString());
			var hours = gameTime.TimeOfDay.ToString() == "00:00:00" ? "TBA" : Offset(timeZone, gameTime).ToString("h:mm tt");
            if (caller == "web")
                gameTimeString = hours;
            else
                gameTimeString = String.Concat("<label>", gameTime.ToString("dddd"), "<br>", gameTime.ToString("M/d"),
                    "<br>",hours,"</label>");

            return gameTimeString;
        }
    }
}