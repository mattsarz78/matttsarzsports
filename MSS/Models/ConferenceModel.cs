using System.Collections.Generic;

namespace MSS.Models
{
    public class ConferenceModel
    {
        public List<ConfGame> ConferenceGames { get; set; }
        public string ConferenceName { get; set; }
        public bool FlexScheduleLink { get; set; }
        public List<ContractText> ContractTexts { get; set ; }
        public string SportYear { get; set; }
        public string Year { get; set; }
    }

    public class ConfGame
    {
        public string GameTitle { get; set; }
        public string Network { get; set; }
        public string Time { get; set; }
        public string TvType { get; set; }
        public string MediaIndicator { get; set; }
        public string Conference { get; set; }
        public List<string> VisitingTeam { get; set; }
        public List<string> HomeTeam { get; set; }
        public string Location { get; set; }
    }

    public class ContractText
	{
		public string Conference { get; set; }
		public string ContractXmlText { get; set; }
	}
}
