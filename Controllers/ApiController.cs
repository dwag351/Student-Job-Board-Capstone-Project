using Microsoft.AspNetCore.Mvc;
using project.Infrastructure;
using project.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace project.Controllers
{
    public class ApiController : Controller
    {
        private readonly AppDbContext _dbContext;
        public ApiController(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        /// <summary>
        /// url:api/piechart
        /// method:get
        /// pie chart data for echarts.js in dashboard page
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/piechart")]
        public IActionResult PieChartData()
        {

            var pieChartDataDtos = _dbContext.JobAds
                    .GroupBy(j => j.JobStatusId)
                    .Select(g => new { JobStatus = g.Key, count = g.Count() })
                    .Join(_dbContext.JobStatuses, a => a.JobStatus, b => b.Id, (a, b) =>
                           new {
                               Name = b.Name,
                               JobStatusId = b.Id,
                               Value = a.count,
                               Color=b.Color
                           }).ToList();

            return Ok(pieChartDataDtos);
        }
        [HttpGet("api/category")]
        public IActionResult CatPieChartData()
        {

            var pieChartDataDtos = _dbContext.JobAds
                    .GroupBy(j => j.CategoryId)
                    .Select(g => new { CategoryId = g.Key, count = g.Count() })
                    .Join(_dbContext.Categories, a => a.CategoryId, b => b.Id, (a, b) =>
                           new
                           {
                               Name = b.Name,
                               CategoryId = b.Id,
                               Value = a.count
                           }).ToList();

            return Ok(pieChartDataDtos);
        }
      

        /// <summary>
        /// url:/api/job
        /// method:get
        /// job list table
        /// </summary>
        /// <param name="adminJobFilter"></param>
        /// <returns></returns>
        [HttpGet("api/job")]
        public IActionResult GetJobList(AdminJobFilter adminJobFilter)
        {
            var jobList = (from j in _dbContext.JobAds
                         
                           join ca in _dbContext.Categories on j.CategoryId equals ca.Id
                           join wt in _dbContext.WorkTypes on j.WorkTypeId equals wt.Id
                           join pt in _dbContext.PayTypes on j.PayTypeId equals pt.Id
                           join ct in _dbContext.ContractTypes on j.ContractTypeId equals ct.Id
                           join js in _dbContext.JobStatuses on j.JobStatusId equals js.Id
                           orderby  j.Id descending
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
                               JobStatus=js.Name,
                               JobStatusId=j.JobStatusId,
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
            if (adminJobFilter.CategoryId != 0)
            {
                jobList = jobList.Where(j => j.CategoryId == adminJobFilter.CategoryId);
            }
            if (adminJobFilter.WorkTypeId != 0)
            {
                jobList = jobList.Where(j => j.WorkTypeId == adminJobFilter.WorkTypeId);
            }
            if (adminJobFilter.PayTypeId != 0)
            {
                jobList = jobList.Where(j => j.PayTypeId == adminJobFilter.PayTypeId);
            }
            if (adminJobFilter.ContractTypeId != 0)
            {
                jobList = jobList.Where(j => j.ContractTypeId == adminJobFilter.ContractTypeId);
            }
     
            if (adminJobFilter.JobStatusId != 0)
            {
                jobList = jobList.Where(j => j.JobStatusId == adminJobFilter.JobStatusId);
            }

            List<PostedJobAdViewModel> jobListViewModel = jobList.ToList<PostedJobAdViewModel>();
            return Ok(jobListViewModel);

        }

        /// <summary>
        /// url:/api/job/approve
        /// method:post
        /// approve job ad
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpPost("api/job/approve")]
        public async Task<IActionResult> ApproveJob(int jobId)
        {
            var result = _dbContext.JobAds.SingleOrDefault(j => j.Id == jobId);
            //only status=1(pendding) job can be change to 2(approved)
          
            if (result != null && result.JobStatusId == 1)//1:pendding job
            {
              
                result.JobStatusId = 2;// change the status of job to approved
                result.ApprovalDate = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
        /// <summary>
        /// url:/api/job/reject
        /// method:post
        /// reject job ad
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpPost("api/job/reject")]
        public async Task<IActionResult> RejectJob(int jobId)
        {
            var result = _dbContext.JobAds.SingleOrDefault(j => j.Id == jobId);
            //only status=1(pendding) job can be change to 3(rejected)

            if (result != null && result.JobStatusId == 1)//1:pendding job
            {

                result.JobStatusId = 3;// change the status of job to rejected
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
        /// <summary>
        /// url:/api/adminuser/checkemail
        /// method:post
        /// check email if exists when create new admin user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("api/adminuser/checkemail")]
        public IActionResult CheckEmail(string email)
        {
            var result = _dbContext.AdminUsers.Any(c => c.Email == email);
            
                return Ok(!result);
            
        }
        /// <summary>
        /// url:/api/adminuser/checkname
        /// method:post
        /// check name if exists when create new admin user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("api/adminuser/checkname")]
        public IActionResult CheckName(string name)
        {
            var result = _dbContext.AdminUsers.Any(c => c.Name ==name.ToLower());

            return Ok(!result);

        }

        /// <summary>
        /// url:/api/adminuser/delete
        /// method:post
        /// delete admin user
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <returns></returns>
        [HttpPost("api/adminuser/delete")]
        public async Task<IActionResult> DeleteAdminUser(int adminUserId)
        {
            if (adminUserId == 1)
            {
                return Ok("Can not delete Root Admin User");
            }

            var adminUser= _dbContext.AdminUsers.Where(c=>c.Id==adminUserId).FirstOrDefault();
            _dbContext.Remove(adminUser);
            await _dbContext.SaveChangesAsync();
            return Ok("success");

        }
        /// <summary>
        /// url:/api/job/approvaldate
        /// method:post
        /// for testing and showing the result of auto expire job ad background service  
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="approvalDate"></param>
        /// <returns></returns>
        [HttpPost("api/job/approvaldate")]
        public async Task<IActionResult> UpdateApprovalDate(int jobId,string approvalDate)
        {

            DateTime approvalDt;
            DateTime.TryParseExact(approvalDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out approvalDt);
            if (approvalDt == DateTime.MinValue)
            {
                return BadRequest("invalid date format.");
            }

            var jobAd = _dbContext.JobAds.Where(c => c.Id == jobId).FirstOrDefault();
            jobAd.ApprovalDate = approvalDt;// change the approval date
            await _dbContext.SaveChangesAsync();
            return Ok("update approval date successfully.");

        }
    }
}
