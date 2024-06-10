using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ReportData
{
    public class HostOrderSummary
    {
        public string HostName { get; set; }
        public int NumberOrder { get; set; }
        public int? TotalOrderMoney { get; set; }
    }
}
