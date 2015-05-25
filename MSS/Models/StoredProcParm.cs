using System.Collections.Generic;

namespace MSS.Models
{
    public class StoredProcParmList
    {
        public List<StoredProcParm> StoredProcParms { get; set; }
    }

    public class StoredProcParm
    {
        public string ParmName { get; set; }
        public string ParmValue { get; set; }
    }
}