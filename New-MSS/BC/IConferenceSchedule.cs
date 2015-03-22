using New_MSS.Models;
using System.Collections.Generic;

namespace New_MSS.BC
{
    public interface IConferenceSchedule
    {
        List<ConfGame> CreateIndependentsGameList(int year);
        List<ConfGame> CreateConferenceGameList(string conference, string year);
    }
}
