using System.Collections.Generic;

namespace New_MSS.Models
{
    public class ConferenceModel
    {
        public List<ConfGame> ConferenceGames { get; set; }
        public string ConferenceName { get; set; }
        public string FlexScheduleLink { get; set; }
        public List<ContractText> ContractTexts { get; set ; }
        public string SportYear { get; set; }
        public string Year { get; set; }
    }

    public class ConfGame
    {
        public string Game { get; set; }
        public string Network { get; set; }
        public string Time { get; set; }
        public string TvType { get; set; }
        public string MediaIndicator { get; set; }
        public string Conference { get; set; }
    }

	public class ContractText
	{
		public string Conference { get; set; }
		public string ContractXmlText { get; set; }
	}
}
