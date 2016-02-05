using System;

namespace MSS.Shared
{
    public class TimeZoneHelper : ITimeZoneHelper 
    {
		public DateTime Offset(string timeZone, DateTime gameTime)
		{
            const string st = " Standard Time";
			var sourceTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern" + st);
		    var destinationTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(DestinationTimeZone(timeZone, st));

            var adjustedTime = TimeZoneInfo.ConvertTime(gameTime, sourceTimeZoneInfo, destinationTimeZoneInfo);
			return IsEndOfHour(destinationTimeZoneInfo.DisplayName, adjustedTime) ? adjustedTime.AddMinutes(1) : adjustedTime;
		}

        private string DestinationTimeZone(string timeZone, string st)
        {
            string destinationTimeZone = string.Empty;
            switch (timeZone)
            {
				case "GMT":
                case "Atlantic":
                case "Newfoundland":
                case "Central":
                case "Mountain":
                case "Pacific":
                case "Eastern":
                    destinationTimeZone = string.Format(timeZone + st);
                    break;
                case "Arizona":
                    destinationTimeZone = string.Format("US Mountain{0}", st);
                    break;
                case "Alaska":
                    destinationTimeZone = string.Format("Alaskan{0}", st);
                    break;
                case "Hawai'i":
                    destinationTimeZone = string.Format("Hawaiian{0}", st);
                    break;
            }
            return destinationTimeZone;
        }

        private bool IsEndOfHour(string destinationTimeZone, DateTime adjustedTime)
    	{
    		return adjustedTime.Minute == 59 && !destinationTimeZone.Contains("Eastern") || adjustedTime.Minute == 29 && destinationTimeZone.Contains("Newfoundland");
    	}

    	public string FormatTelevisedTime(DateTime gameTimeInput, string caller, string timeZone)
        {
    	    DateTime gameTime = DateTime.Parse(gameTimeInput.ToString());
			var hours = gameTime.TimeOfDay.ToString() == "00:00:00" ? "TBA" : Offset(timeZone, gameTime).ToString("h:mm tt");
    	    string gameTimeString = caller == "web"
    	        ? hours
    	        : string.Format("<label>{0}<br>{1}<br>{2}</label>", gameTime.ToString("dddd"), gameTime.ToString("M/d"), hours);
    	    return gameTimeString;
        }

		public DateTime GetServerTime()
		{
			var serverTimeZoneInfo = TimeZoneInfo.Local;
			var destinationTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

			return TimeZoneInfo.ConvertTime(DateTime.Now, serverTimeZoneInfo, destinationTimeZoneInfo);

		}
	}
}