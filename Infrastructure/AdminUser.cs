using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace project.Infrastructure
{
    public class AdminUser
    {
        [Key] //Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //Self-increasing
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string  Name { get; set; }

    }
}
