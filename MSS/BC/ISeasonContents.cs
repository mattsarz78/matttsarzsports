using MSS.Models;
using System.Collections.Generic;

namespace MSS.BC
{
    public interface ISeasonContents
    {
        List<YearDate> CreateDateModel(string season);
        string CreateTitle(string sportYear);
	}
}
