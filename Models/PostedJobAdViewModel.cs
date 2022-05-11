using Microsoft.AspNetCore.Mvc.Rendering;
using project.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class PostedJobAdViewModel
    {
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> WorkTypes { get; set; }
        public List<SelectListItem> PayTypes { get; set; }
        public List<SelectListItem> ContractTypes { get; set; }

        public int JobId { get; set; }
        [Required(ErrorMessage = "The job title is required.")]
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "The location is required.")]
        public string Location { get; set; }
        public int CategoryId { get; set; }
        public string  Category { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkType { get; set; }
        public int PayTypeId { get; set; }
        public string PayType { get; set; }
    
        public int JobStatusId { get; set; }
         public string JobStatus { get; set; }
        public int ContractTypeId { get; set; }
        public string ContractType { get; set; }
        [Required(ErrorMessage = "The role description is required.")]
        public string RoleDescription { get; set; }
        [Required(ErrorMessage = "The key skill field is required.")]
        public string KeySkills { get; set; }
        [Required(ErrorMessage = "The contact name is required.")]
        public string ContactName { get; set; }
        [Required(ErrorMessage = "The contact position title is required.")]
        public string ContactPositionTitle { get; set; }
        [Required(ErrorMessage = "The contact email is required.")]
        [RegularExpression(@"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z",
        ErrorMessage = "Please enter correct email address")]
        public string ContactEmail { get; set; }
        [Required(ErrorMessage = "The contact email is required.")]
        [RegularExpression(@"\A[0-9]*\Z",
        ErrorMessage = "Phone number is invalid")]
        public string ContactPhone { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Company detail is required.")]
        public string CompanyDetail { get; set; }

        [Required(ErrorMessage = "The closing date is required.")]
        public string ClosingDateString { get; set; }
        [Required(ErrorMessage = "The start date is required.")]
        public string StartDateString { get; set; }
       
        [Required(ErrorMessage = "The closing date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ClosingDate { get; set; }
        [Required(ErrorMessage = "The start date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime SubmitDate { get; set; } = DateTime.Today;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ApprovalDate { get; set; }

    }
}
