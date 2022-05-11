using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using project.Infrastructure;
using project.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace project.Controllers
{
   
    public class JobController : Controller
    {

        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
     
        public JobController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
           
            _dbContext = appDbContext;
            _env = env;
        }

        /// <summary>
        /// url:/job/list
        /// method:get
        /// show job ad list and only approved job ad can be show
        /// </summary>
        /// <param name="jobListViewModel">filter conditions</param>
        /// <param name="p">page number</param>
        /// <returns></returns>
        [Route("job/list",Name = "JobList")]
        //[GoogleScopedAuthorize()]
        [HttpGet]
        public IActionResult List(JobListViewModel jobListViewModel,[FromQuery]int p=1)
        {
            string isGoogleSignIn = HttpContext.Session.GetString("google_login");
            string googleEmail = HttpContext.Session.GetString("google_email");

            if (isGoogleSignIn != "true")
            {
                return Redirect("/");//if not go back signin page
            }
            ViewData["google_email"] = googleEmail;
            var categorySelectListItems = _dbContext.Categories
                                   .Select(c =>
                                       new SelectListItem
                                       {
                                           Text = c.Name,
                                           Value = c.Id.ToString()
                                       }).ToList<SelectListItem>();
            jobListViewModel.Categories = categorySelectListItems;
            var workTypeSelectListItems = _dbContext.WorkTypes
                                .Select(c =>
                                    new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList<SelectListItem>();
            jobListViewModel.WorkTypes = workTypeSelectListItems;
            var payTypeSelectListItems = _dbContext.PayTypes
                                .Select(c =>
                                    new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList<SelectListItem>();
            jobListViewModel.PayTypes = payTypeSelectListItems;
            var contractTypeSelectListItems = _dbContext.ContractTypes
                                .Select(c => new SelectListItem
                                {
                                    Text = c.Name,
                                    Value = c.Id.ToString()
                                }).ToList<SelectListItem>();
            jobListViewModel.ContactTypes = contractTypeSelectListItems;



            var jobList = (from j in _dbContext.JobAds
                         
                           join ca in _dbContext.Categories on j.CategoryId equals ca.Id
                           join wt in _dbContext.WorkTypes on j.WorkTypeId equals wt.Id
                           join pt in _dbContext.PayTypes on j.PayTypeId equals pt.Id
                           join ct in _dbContext.ContractTypes on j.ContractTypeId equals ct.Id
                           where j.JobStatusId == 2//only show approval job ad
                           orderby j.ApprovalDate descending
                           select new PostedJobAdViewModel
                           {
                               JobId = j.Id,
                               JobTitle = j.Position,
                               Location = j.Location,
                               CategoryId = j.CategoryId,
                               Category = ca.Name,
                               WorkTypeId = j.WorkTypeId,
                               WorkType = wt.Name,
                               PayTypeId = j.PayTypeId,
                               PayType = pt.Name,
                             
                               CompanyName = j.Company,
                               CompanyDetail = j.CompanyInfo,
                               ContractTypeId = j.ContractTypeId,
                               ContractType = ct.Name,
                               RoleDescription = j.RoleDescription,
                               KeySkills = j.KeySkills,
                               ContactName = j.ContactName,
                               ContactPositionTitle = j.ContactPositionTitle,
                               ContactEmail = j.ContactEmail,
                               ContactPhone = j.ContactPhone,
                               ClosingDate = j.ClosingDate,
                               StartDate = j.StartDate,
                               SubmitDate = j.SubmitDate,
                               ApprovalDate = j.ApprovalDate
                           }).AsQueryable();
            if (jobListViewModel.FormCategroyId != 0)
            {

                jobList = jobList.Where(j => j.CategoryId == jobListViewModel.FormCategroyId);
            }
            if (jobListViewModel.FormWorkTypeId != 0)
            {
                jobList = jobList.Where(j => j.WorkTypeId == jobListViewModel.FormWorkTypeId);
            }
            if (jobListViewModel.FormPayTypeId != 0)
            {
                jobList = jobList.Where(j => j.PayTypeId == jobListViewModel.FormPayTypeId);
            }
            if (jobListViewModel.FormContractTypeId != 0)
            {
                jobList = jobList.Where(j => j.ContractTypeId == jobListViewModel.FormContractTypeId);
            }
            int pageSize = 7;//showing job ad number per page
            var count = jobList.Count();
            ViewBag.PageNumber = count%pageSize==0? count/pageSize :Math.Ceiling((float)count / pageSize);
            var items = jobList.Skip((p- 1) * pageSize).Take(pageSize).ToList();
            jobListViewModel.postedJobAdViewModels = items.ToList<PostedJobAdViewModel>();
            return View("index",jobListViewModel);

        }
        /// <summary>
        /// url:/job/{id}
        /// method:get
        /// only approved job can be shown
        /// </summary>
        /// <param name="id">job ad id</param>
        /// <returns></returns>
        [HttpGet("job/{id}", Name = "JobDetail")]
        public IActionResult Detail(int id)
        {
            string isGoogleSignIn = HttpContext.Session.GetString("google_login");
            string googleEmail = HttpContext.Session.GetString("google_email");

            if (isGoogleSignIn != "true")
            {
                return Redirect("/");//if not go back signin page
            }
            ViewData["google_email"] = googleEmail;
            var result = (from j in _dbContext.JobAds     
                         join ca in _dbContext.Categories on j.CategoryId equals ca.Id
                         join wt in _dbContext.WorkTypes on j.WorkTypeId equals wt.Id
                         join pt in _dbContext.PayTypes on j.PayTypeId equals pt.Id
                         join ct in _dbContext.ContractTypes on j.ContractTypeId equals ct.Id
                         where j.JobStatusId == 2 && j.Id == id
                         select new PostedJobAdViewModel
                         {
                             JobId=j.Id,
                             JobTitle = j.Position,
                             Location = j.Location,
                             CategoryId = j.CategoryId,
                             Category = ca.Name,
                             WorkTypeId = j.WorkTypeId,
                             WorkType = wt.Name,
                             PayTypeId = j.PayTypeId,
                             PayType = pt.Name,
                             CompanyName = j.Company,
                             CompanyDetail = j.CompanyInfo,
                             ContractTypeId = j.ContractTypeId,
                             ContractType = ct.Name,
                             RoleDescription = j.RoleDescription,
                             KeySkills = j.KeySkills,
                             ContactName = j.ContactName,
                             ContactPositionTitle = j.ContactPositionTitle,
                             ContactEmail = j.ContactEmail,
                             ContactPhone = j.ContactPhone,
                             ClosingDate = j.ClosingDate,
                             StartDate = j.StartDate,
                             SubmitDate = j.SubmitDate,
                             ApprovalDate = j.ApprovalDate
                         }).FirstOrDefault();

            
            return View("job",result);
        }
        
        [HttpPost("/job/login")]
        public async Task<IActionResult> Login(string token,string sub)
        {
          
            var client = new RestClient("https://www.googleapis.com/");
            var request = new RestRequest("oauth2/v3/tokeninfo", Method.GET);
            request.AddParameter("id_token", token); // adds to POST or URL querystring based on Method
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var jsonObj= JsonConvert.DeserializeObject<JToken>(content);
            if (jsonObj["sub"].ToString() == sub)
            {
                HttpContext.Session.SetString("google_login", "true");
                HttpContext.Session.SetString("google_email", jsonObj["email"].ToString());
                HttpContext.Session.SetString("google_name", jsonObj["name"].ToString());
                return Redirect("/job/list");
            }
            else
            {
                return Redirect("/job/logout");
            }
        }
        [HttpGet("/job/logout")]
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("google_login");
            HttpContext.Session.Remove("google_email");
            HttpContext.Session.Remove("google_name");
            return Redirect("/?return=logout");
        }
    }
}
