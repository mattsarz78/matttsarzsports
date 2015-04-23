using New_MSS.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace New_MSS.Shared
{
    public class StoredProcHelper : IStoredProcHelper
    {
        public SqlDataReader RunDataReader(StoredProcParmList parmList, SqlConnection conn, string procName)
        {
            conn.Open();
            using (var storedProcCommand = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure })
            {
                foreach (var parm in parmList.StoredProcParms)
                {
                    storedProcCommand.Parameters.Add(parm.ParmName == "@Week"
                                                         ? new SqlParameter(parm.ParmName, Int32.Parse(parm.ParmValue))
                                                         : new SqlParameter(parm.ParmName, parm.ParmValue));
                }
                return storedProcCommand.ExecuteReader();
            }
        }
    }
}