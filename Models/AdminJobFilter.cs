using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class AdminJobFilter
    {
        public int CategoryId { get; set; }
        public int JobStatusId { get; set; }
    
        public int WorkTypeId { get; set; }
        public int ContractTypeId { get; set; }
        public int PayTypeId { get; set; }
    }
}
