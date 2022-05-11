using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace project.Infrastructure
{
    /// <summary>
    /// Totally 5 statuses
    /// 1:pending
    /// 2:approved
    /// 3:rejected
    /// 4:expired
    /// 5:hired
    /// 6:closed
    /// </summary>
    public class JobStatus

    {
        [Key] //Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //Self-increasing
        public int Id { get; set; }
        public string  Name { get; set; }
        public string Color { get; set; }
    }
}
