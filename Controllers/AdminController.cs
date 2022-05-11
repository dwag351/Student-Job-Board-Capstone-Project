using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using project.Infrastructure;
using project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;
        public AdminController(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        /// <summary>
        /// url: /admin/index
        /// method:get
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //check if admin user login
            string isAdminSignIn = HttpContext.Session.GetString("admin_login");
            int adminUserId = Convert.ToInt32(HttpContext.Session.GetString("admin_user_id"));
            if (isAdminSignIn != "true")
            {
                return Redirect("/admin/signin");//if not go back signin page
            }
            /// 1:pending
            /// 2:approved
            /// 3:rejected
            /// 4:expired
   
            DashboardViewModel model = new DashboardViewModel
            {
          
            
                PendingJobCount = _dbContext.JobAds.Where(c => c.JobStatusId == 1).Count(),//pending job number
                ApprovalJobCount = _dbContext.JobAds.Where(c => c.JobStatusId == 2).Count(),//approved job number

                RejectedJobCount = _dbContext.JobAds.Where(c => c.JobStatusId == 3).Count(),//rejected job number
                ExpiredJobCount = _dbContext.JobAds.Where(c => c.JobStatusId == 4).Count(),//expired job number
            };
    
      
      
            return View("dashboard",model);
        }
        /// <summary>
        /// url:/admin/users
        /// method:get
        /// </summary>
        /// <returns></returns>
        public IActionResult Users()
        {
            string isAdminSignIn = HttpContext.Session.GetString("admin_login");
            int adminUserId = Convert.ToInt32(HttpContext.Session.GetString("admin_user_id"));
            if (isAdminSignIn != "true")
            {
                return Redirect("/admin/signin");
            }
            List<AdminUser> adminUsers = new List<AdminUser>();
            adminUsers = _dbContext.AdminUsers.ToList<AdminUser>();
            return View(adminUsers);
        }
        /// <summary>
        /// url:/admin/users
        /// method:post
        /// create a new admin user
        /// </summary>
        /// <param name="adminUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Users(AdminUser adminUser)
        {
            _dbContext.AdminUsers.Add(adminUser);
            await _dbContext.SaveChangesAsync();
            return Redirect("/admin/users");
        }
       

        /// <summary>
        /// url:/admin/signin
        /// method :get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        /// <summary>
        /// url:/admin/signin
        /// method:post
        /// log in
        /// </summary>
        /// <param name="adminUserSignInViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SignIn(AdminUserSignInViewModel adminUserSignInViewModel)
        {
            //get admin user
            var result = _dbContext.AdminUsers
                        .Where(c => c.Name == adminUserSignInViewModel.Name && c.Password == adminUserSignInViewModel.Password)
                        .FirstOrDefault();

            if (result != null)
            {
                HttpContext.Session.SetString("admin_login", "true");
                HttpContext.Session.SetString("admin_user_id", result.Id.ToString());
                return Redirect("/admin/index");
            }
            else
            {
               
                TempData["msgCode"] = 2;
                TempData["msgText"] = "Email or password is wrong, please try again. ";
                return View("SignIn");
            }



        }



        /// <summary>
        /// url:/admin/jobmanagement
        /// method:get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult JobManagement()
        {
            //check if admin user login
            string isAdminSignIn = HttpContext.Session.GetString("admin_login");
            int adminUserId = Convert.ToInt32(HttpContext.Session.GetString("admin_user_id"));
            if (isAdminSignIn != "true")
            {
                return Redirect("/admin/signin");
            }
            JobListViewModel jobListViewModel = new JobListViewModel();
            //category dropdown list
            var categorySelectListItems = _dbContext.Categories
                                   .Select(c =>
                                       new SelectListItem
                                       {
                                           Text = c.Name,
                                           Value = c.Id.ToString()
                                       }).ToList<SelectListItem>();
            jobListViewModel.Categories = categorySelectListItems;
            //work type dropdown list
            var workTypeSelectListItems = _dbContext.WorkTypes
                                .Select(c =>
                                    new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList<SelectListItem>();
            jobListViewModel.WorkTypes = workTypeSelectListItems;
            //pay type dropdown list
            var payTypeSelectListItems = _dbContext.PayTypes
                                .Select(c =>
                                    new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList<SelectListItem>();
            jobListViewModel.PayTypes = payTypeSelectListItems;
            //contract type dropdown list
            var contractTypeSelectListItems = _dbContext.ContractTypes
                                .Select(c => new SelectListItem
                                {
                                    Text = c.Name,
                                    Value = c.Id.ToString()
                                }).ToList<SelectListItem>();
            jobListViewModel.ContactTypes = contractTypeSelectListItems;
            //job status dropdown list
            var jobStatusSelectListItems = _dbContext.JobStatuses
                             .Select(c => new SelectListItem
                             {
                                 Text = c.Name,
                                 Value = c.Id.ToString()
                             }).ToList<SelectListItem>();
            jobListViewModel.JobStatuses = jobStatusSelectListItems;

            //company dropdown list
            //var companySelectListItems = _dbContext.Companies
            //                 .Select(c => new SelectListItem
            //                 {
            //                     Text = c.Name,
            //                     Value = c.Id.ToString()
            //                 }).ToList<SelectListItem>();
            //jobListViewModel.Companies = companySelectListItems;

            return View(jobListViewModel);
        }
        /// <summary>
        /// url:/admin/signout
        /// method:get
        /// log out
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            //clear session
            //HttpContext.Session.Clear();
  
            HttpContext.Session.Remove("admin_login");
            HttpContext.Session.Remove("admin_user_id");
            //go back to signin page
            return RedirectToAction("signin");
        }
    }
}
