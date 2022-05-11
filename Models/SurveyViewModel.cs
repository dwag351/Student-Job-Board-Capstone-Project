using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class SurveyViewModel
    {
        [DisplayName("Have you found any qualified candidate?")]
        public bool HasCandidate { get; set; }
        [DisplayName("If you find a right employee, is he or she from UOA?")]
        public bool IsUoaStudent { get; set; }
        [DisplayName("Did you find the candidates through our website?")]
        public bool IsFromOurWeb { get; set; }
        [DisplayName(" How many people did you hire for your job?")]
        public int CandidateQty { get; set; } 
        [DisplayName("Are you satisfied with our site?")]
        public bool IsSatisfied { get; set; }
        [DisplayName("Could you please give some suggestions?")]
        public string  Feedback { get; set; }
        public PostedJobAdViewModel postedJobAdViewModel { get; set; }

    }
}
