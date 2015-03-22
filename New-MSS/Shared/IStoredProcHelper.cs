using New_MSS.Models;
using System.Data.SqlClient;

namespace New_MSS.Shared
{
    public interface IStoredProcHelper
    {
        SqlDataReader RunDataReader(StoredProcParmList parmList, SqlConnection conn, string procName);
    }
}
