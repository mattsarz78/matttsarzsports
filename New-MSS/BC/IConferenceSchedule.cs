using New_MSS.Models;
using New_MSS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace New_MSS.BC
{
    public interface IConferenceSchedule
    {
        List<ConfGame> CreateIndependentsGameList(int year);
        List<ConfGame> CreateConferenceGameList(string conference, string year);
    }
}
