using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class DashboardViewModel
    {
        public int PendingJobCount { get; set; }
        public int ApprovalJobCount { get; set; }
        public int ExpiredJobCount{ get; set; }
        public int RejectedJobCount { get; set; }
    }
}
