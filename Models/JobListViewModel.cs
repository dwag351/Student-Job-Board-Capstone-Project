using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class JobListViewModel
    {
        public int FormCategroyId { get; set; }
        public int FormWorkTypeId { get; set; }
        public int FormContractTypeId { get; set; }
        public int FormPayTypeId { get; set; }
        public int FormJobStatusId { get; set; }
        public int FormCompanyId { get; set; }
        public List<PostedJobAdViewModel> postedJobAdViewModels { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> WorkTypes { get; set; }
        public List<SelectListItem> PayTypes { get; set; }
        public List<SelectListItem> ContactTypes { get; set; }
        public List<SelectListItem> Companies { get; set; }
        public List<SelectListItem> JobStatuses { get; set; }
    }
}
