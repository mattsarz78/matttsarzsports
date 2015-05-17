using System;

namespace New_MSS.Shared
{
    public class TimeZoneHelper : ITimeZoneHelper 
    {
		public DateTime Offset(string timeZone, DateTime gameTime)
		{
            const string st = " Standard Time";
			var sourceTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern" + st);
            var destinationTimeZone = string.Format("Eastern{0}", st);
		    switch (timeZone)
            {
				case "Atlantic": destinationTimeZone = string.Format("Atlantic{0}", st); break;
				case "Newfoundland": destinationTimeZone = string.Format("Newfoundland{0}", st); break;
				case "Central": destinationTimeZone = string.Format("Central{0}", st); break;
                case "Mountain": destinationTimeZone = string.Format("Mountain{0}", st); break;
                case "Arizona": destinationTimeZone = string.Format("US Mountain{0}", st); break;
                case "Pacific": destinationTimeZone = string.Format("Pacific{0}", st); break;
                case "Alaska": destinationTimeZone = string.Format("Alaskan{0}", st); break;
                case "Hawai'i": destinationTimeZone = string.Format("Hawaiian{0}", st); break;
            }
			var adjustedTime = TimeZoneInfo.ConvertTime(gameTime, sourceTimeZoneInfo,
			                                            TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZone));
			return IsEndOfHour(destinationTimeZone, adjustedTime) ? adjustedTime.AddMinutes(1) : adjustedTime;
		}

    	private bool IsEndOfHour(string destinationTimeZone, DateTime adjustedTime)
    	{
    		return adjustedTime.Minute == 59 && !destinationTimeZone.Contains("Eastern") || adjustedTime.Minute == 29 && destinationTimeZone.Contains("Newfoundland");
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