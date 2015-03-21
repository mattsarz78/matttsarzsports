using System;
using System.Collections.Generic;

namespace New_MSS.Models
{
    public class DateModel
    {
        public string Year;
        public string Title;
    	public bool IsFootball;
    	public bool IsBasketballWithPostseason;
    	public List<YearDate> YearDatesList { get; set; }
    	public string ConferenceListBase;
    }

    public class YearDate
    {
        public int Week { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
