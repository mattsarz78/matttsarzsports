using New_MSS.Models;
using System.Collections.Generic;

namespace New_MSS.BC
{
    public interface ISeasonContents
    {
        List<YearDate> CreateDateModel(string season);
        string CreateTitle(string sportYear);
    }
}
