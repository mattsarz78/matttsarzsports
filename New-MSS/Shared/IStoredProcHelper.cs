using New_MSS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace New_MSS.Shared
{
    public interface IStoredProcHelper
    {
        SqlDataReader RunDataReader(StoredProcParmList parmList, SqlConnection conn, string procName);
    }
}
