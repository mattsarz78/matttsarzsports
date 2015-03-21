using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace New_MSS.Shared
{
    public class TimeZoneHelper : ITimeZoneHelper 
    {
        public List<SelectListItem> timeZoneList = new List<SelectListItem>
        { 
            new SelectListItem { Text = "Eastern", Value = "0" },
            new SelectListItem { Text = "Central", Value = "1" },
            new SelectListItem { Text = "Mountain", Value = "2" },
            new SelectListItem { Text = "Arizona", Value = "3" },
            new SelectListItem { Text = "Pacific", Value = "4" },
            new SelectListItem { Text = "Alaska", Value = "5" },
            new SelectListItem { Text = "Hawai'i", Value = "6" }
        };

        public List<SelectListItem> CreateTimeZoneList(string timeZone)
        {
            foreach (var item in timeZoneList)
                item.Selected = item.Text == timeZone;
            return timeZoneList;
        }

        public string DeterminePostDropDownValue(FormCollection fc)
        {
            string dropDownListItem = fc.Get("DropDownTimeZone");
            return timeZoneList.First(x => x.Value == dropDownListItem).Text;
        }

		public DateTime Offset(string timeZone, DateTime gameTime)
		{
            var st = " Standard Time";
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