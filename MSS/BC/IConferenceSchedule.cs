using MSS.Models;
using System.Collections.Generic;

namespace MSS.BC
{
    public interface IConferenceSchedule
    {
        List<ConfGame> CreateIndependentsGameList(string year);
        List<ConfGame> CreateConferenceGameList(string conference, string year);
    }
}
