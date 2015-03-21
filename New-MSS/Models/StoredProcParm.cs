using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace New_MSS.Models
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