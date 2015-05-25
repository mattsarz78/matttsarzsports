using MSS.Models;
using System.Data.SqlClient;

namespace MSS.Shared
{
    public interface IStoredProcHelper
    {
        SqlDataReader RunDataReader(StoredProcParmList parmList, SqlConnection conn, string procName);
    }
}
