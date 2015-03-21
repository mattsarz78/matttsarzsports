using New_MSS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace New_MSS.BC
{
    public interface ISeasonContents
    {
        List<YearDate> CreateDateModel(string season);
        string CreateTitle(string sportYear);
    }
}
