using MSS.Models;
using System.Collections.Generic;

namespace MSS.BC
{
    public interface IConferenceSchedule
    {
        List<ConfGame> CreateIndependentsGameList(int year);
        List<ConfGame> CreateConferenceGameList(string conference, string year);
    }
}
