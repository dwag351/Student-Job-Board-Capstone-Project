using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using project.Infrastructure;
using project.Models;
using project.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace project.Controllers
{

    public class EmployerController : Controller
    {
        //use DBCONTEXT 
        private readonly AppDbContext _dbContext;
        private readonly EmailService _emailService;
        public EmployerController(AppDbContext appDbContext, EmailService emailService )
        {
            _dbContext = appDbContext;
            _emailService = emailService;
        }
    
     
        #region   post advert
        /// <summary>
        /// url:/employer/postad
        /// method:get
        /// show postad page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            
            PostedJobAdViewModel postedJobAdViewModel = new PostedJobAdViewModel();
            //Generate category dropdownlist
            var categorySelectListItems = _dbContext.Categories
                                    .Select(c =>
                                        new SelectListItem
                                        {
                                            Text = c.Name,
                                            Value = c.Id.ToString()
                                        }).ToList<SelectListItem>();
            postedJobAdViewModel.Categories = categorySelectListItems;
            //Generate work type radio button group
            var workTypeSelectListItems = _dbContext.WorkTypes
                                .Select(c =>
                                    new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList<SelectListItem>();
            postedJobAdViewModel.WorkTypes = workTypeSelectListItems;

            //Generate pay type radio button group

            var payTypeSelectListItems = _dbContext.PayTypes
                                .Select(c =>
                                    new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList<SelectListItem>();
            postedJobAdViewModel.PayTypes = payTypeSelectListItems;
            //Generate contract type radio button group
            var contractTypeSelectListItems = _dbContext.ContractTypes
                                .Select(c => new SelectListItem
                                {
                                    Text = c.Name,
                                    Value = c.Id.ToString()
                                }).ToList<SelectListItem>();
            postedJobAdViewModel.ContractTypes = contractTypeSelectListItems;
            //pass postedJobAdViewModel to the page view
            return View( postedJobAdViewModel);
        }
        /// <summary>
        /// url:/employer/postad
        /// method:post 
        /// post a job ad
        /// </summary>
        /// <param name="postedJobAdViewModel"></param>
        /// <returns></returns>
   
        [HttpPost]
        public async Task<IActionResult> Index(PostedJobAdViewModel postedJobAdViewModel)
        {
      
            DateTime startDate;
            DateTime closingDate;
            DateTime.TryParseExact(postedJobAdViewModel.StartDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            DateTime.TryParseExact(postedJobAdViewModel.ClosingDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out closingDate);
            postedJobAdViewModel.ClosingDate = closingDate;
            postedJobAdViewModel.StartDate = startDate;
            // check model validation
            if (ModelState.IsValid)
            {
               
                //get all job detail from front web page with post method
                JobAd jobAd= new JobAd
                {
                    Position= postedJobAdViewModel.JobTitle,
                    Location = postedJobAdViewModel.Location,
                    CategoryId = postedJobAdViewModel.CategoryId,
                    WorkTypeId = postedJobAdViewModel.WorkTypeId,
                    PayTypeId= postedJobAdViewModel.PayTypeId,
                    Company= postedJobAdViewModel.CompanyName,
                    CompanyInfo=postedJobAdViewModel.CompanyDetail,
                    JobStatusId=1,//pending
                    ContractTypeId=postedJobAdViewModel.ContractTypeId,
                    RoleDescription=postedJobAdViewModel.RoleDescription,
                    KeySkills=postedJobAdViewModel.KeySkills,
                    ContactName=postedJobAdViewModel.ContactName,
                    ContactPhone=postedJobAdViewModel.ContactPhone,
                    ContactEmail=postedJobAdViewModel.ContactEmail,
                    ContactPositionTitle=postedJobAdViewModel.ContactPositionTitle,
                    ClosingDate = closingDate,
                    StartDate= startDate,
                    SubmitDate=DateTime.Now,
                    AuthKey= GetJobAdRandomKey(10)
                };
                //save job ad into database use entity framework core
                _dbContext.JobAds.Add(jobAd);
                await _dbContext.SaveChangesAsync();
                //after execution redirect to "employer/index"
                //TempData["msgCode"] = 1;
                //TempData["msgText"] = "Your ad has been posted, please wait for administrators approval. ";

                ViewBag.msgTitle = "Thank you, your ad has been posted successfully.";
                ViewBag.info = "Please wait for administrators approval.";
                return View("result");
            }
            else
            {
                //if model is invalid show post ad page again 
                //Generate category dropdownlist
                var categorySelectListItems = _dbContext.Categories
                               .Select(c =>
                                   new SelectListItem
                                   {
                                       Text = c.Name,
                                       Value = c.Id.ToString()
                                   }).ToList<SelectListItem>();
                postedJobAdViewModel.Categories = categorySelectListItems;
                //Generate work type radio button group
                var workTypeSelectListItems = _dbContext.WorkTypes
                                    .Select(c =>
                                        new SelectListItem
                                        {
                                            Text = c.Name,
                                            Value = c.Id.ToString()
                                        }).ToList<SelectListItem>();
                postedJobAdViewModel.WorkTypes = workTypeSelectListItems;


                //Generate pay type radio button group
                var payTypeSelectListItems = _dbContext.PayTypes
                                    .Select(c =>
                                        new SelectListItem
                                        {
                                            Text = c.Name,
                                            Value = c.Id.ToString()
                                        }).ToList<SelectListItem>();
                postedJobAdViewModel.PayTypes = payTypeSelectListItems;
                //Generate contract type radio button group
                var contractTypeSelectListItems = _dbContext.ContractTypes
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList<SelectListItem>();
                postedJobAdViewModel.ContractTypes = contractTypeSelectListItems;
                TempData["msgCode"] = 2;
                TempData["msgText"] = "Please check your ad details. ";
                return View(postedJobAdViewModel);
            }


       
        }
        #endregion
        [HttpGet]
        public IActionResult Survey(int id,string key)
        {
            if (!CheckJobAdKey(id, key))//when authkey is incorrect, redirect
            {
                ViewBag.msgTitle = "Invalidation";
                ViewBag.info = "You don't have permission to view this page.";
                return View("result");
            }
            if (_dbContext.JobAds.Where(j => j.Id == id).FirstOrDefault().CompleteSurvey)
            {
                ViewBag.msgTitle = "Notice";
                ViewBag.info = "You have already completed the survey.";
                return View("result");
            }
           

            SurveyViewModel surveyViewModel = new SurveyViewModel();
            surveyViewModel.postedJobAdViewModel = (from j in _dbContext.JobAds

                          join ca in _dbContext.Categories on j.CategoryId equals ca.Id
                          join wt in _dbContext.WorkTypes on j.WorkTypeId equals wt.Id
                          join pt in _dbContext.PayTypes on j.PayTypeId equals pt.Id
                          join ct in _dbContext.ContractTypes on j.ContractTypeId equals ct.Id
                          where  j.Id == id
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
                          }).FirstOrDefault();
            return View(surveyViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Survey(int id, string key, SurveyViewModel surveyViewModel)
        {
            if (!CheckJobAdKey(id, key))//when authkey is incorrect, redirect
            {
                ViewBag.msgTitle = "Invalidation";
                ViewBag.info = "You don't have permission to view this page.";
                return View("result");
            }

            ViewBag.msgTitle = "Thank you so much for filling out our survey.";
            //ViewBag.info = "You don't have permission to view this page.";
            _emailService.SendSurveyResultToAdminUser(id, surveyViewModel);
           var ad= _dbContext.JobAds.Where(c => c.Id == id).FirstOrDefault();
            ad.CompleteSurvey = true;
            if (surveyViewModel.IsFromOurWeb)
            {
                ad.IsHired = true;
            }
            await _dbContext.SaveChangesAsync();

       
            return View("result");
        }


        private string GetJobAdRandomKey(int length)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string result="";
            string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; 
            for (int i = 0; i < length; i++)
            {
                result += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return result;
        }
       
        private bool CheckJobAdKey(int id,string key)
        {
            bool result = _dbContext.JobAds
                  .Where(c => c.Id == id && c.AuthKey == key)
                  .Any();

            return result;
        }
      

    }
}
