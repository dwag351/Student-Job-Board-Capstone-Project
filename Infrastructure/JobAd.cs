using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace project.Infrastructure
{
    public class JobAd
    {
        [Key] //Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //Self-increasing
        public int Id { get; set; }
        public string Position { get; set; }
        public string  Location { get; set; }
        public int CategoryId { get; set; }
        public int WorkTypeId { get; set; }
        public int PayTypeId { get; set; }
        //public int CompanyId { get; set; }
        public string Company { get; set; }
        public string CompanyInfo { get; set; }

        [DefaultValue(false)]
        public bool IsHired { get; set; }
        [DefaultValue(false)]
        public bool CompleteSurvey { get; set; }
        public int JobStatusId { get; set; }
        public int ContractTypeId{ get; set; }
        public string RoleDescription { get; set; }
        public string KeySkills { get; set; }

        public string ContactName { get; set; }
        public string ContactPositionTitle { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime SubmitDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string AuthKey { get; set; }

    }
}
