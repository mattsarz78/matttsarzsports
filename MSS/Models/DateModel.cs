using System;
using System.Collections.Generic;

namespace MSS.Models
{
    public class DateModel
    {
        public string Year { get; set; }
        public string Title { get; set; }
        public bool IsFootball { get; set; }
        public bool IsBasketballWithPostseason { get; set; }
    	public List<YearDate> YearDatesList { get; set; }
        public string ConferenceListBase { get; set; }
    }

    public class YearDate
    {
        public int Week { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
