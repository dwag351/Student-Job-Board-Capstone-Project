using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using project.Infrastructure;
using project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace project.Services
{
    public  class EmailService
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public EmailService( AppDbContext appDbContext, IWebHostEnvironment env, IConfiguration config)
        {
            _dbContext = appDbContext;
            _env = env;
            _config = config;

        }
  
        public void SendSurveyToEmployer(int jobAdId)
        {

            using (var message = new MailMessage())
            {

                string projectEmail = "myjobseek399@gmail.com";//All email will be sent from this email;
           
                var jobAd = _dbContext.JobAds.Where(j => j.Id == jobAdId).FirstOrDefault();
                message.To.Add(new MailAddress(jobAd.ContactEmail, jobAd.ContactName));
                message.From = new MailAddress(projectEmail, "StudentJobBoard");
                ////cc
                //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                ////bcc
                //message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                message.Subject = "Please complete the survey on " + jobAd.Position;
                message.Body = GetSurveyEmailBody(jobAdId);
                message.IsBodyHtml = true;
                // use google smtp to send email
                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(projectEmail, "project399");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }

      

        public void SendSurveyResultToAdminUser(int jobAdId, SurveyViewModel surveyViewModel)
        {

            using (var message = new MailMessage())
            {

                string projectEmail = "myjobseek399@gmail.com";//All email will be sent from this email;
                var adminUsers = _dbContext.AdminUsers.Select(a =>new { a.Email, a.Name }).ToList();
                foreach(var user in adminUsers)
                {
                    message.To.Add(new MailAddress(user.Email, user.Name));
                }
                var company = (from j in _dbContext.JobAds
                           
                               where j.Id == jobAdId
                               select j).FirstOrDefault();
                var jobAd = _dbContext.JobAds.Where(j => j.Id == jobAdId).FirstOrDefault();
             
                message.From = new MailAddress(projectEmail, jobAd.Company);
                ////cc
                //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                ////bcc
                //message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                message.Subject = "You receive one survey result from "+ jobAd.Company;
                message.Body = GetSurveyResultEmailBody(jobAdId, surveyViewModel);
                message.IsBodyHtml = true;
                // use google smtp to send email
                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(projectEmail, "project399");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
        private string GetSurveyResultEmailBody(int jobAdId, SurveyViewModel surveyViewModel)
        {
            //    /wwwroot/email.html
            string body = File.ReadAllText(Path.Combine(_env.WebRootPath, "survey-result.html"));
            var jobAd= (from j in _dbContext.JobAds
                          join ca in _dbContext.Categories on j.CategoryId equals ca.Id
                          join wt in _dbContext.WorkTypes on j.WorkTypeId equals wt.Id
                          join pt in _dbContext.PayTypes on j.PayTypeId equals pt.Id
                          join ct in _dbContext.ContractTypes on j.ContractTypeId equals ct.Id
                          where j.Id == jobAdId
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
        



            body = body.Replace("@@Position", jobAd.JobTitle);
            body = body.Replace("@@Company", jobAd.CompanyName);
            body = body.Replace("@@Location", jobAd.Location);
            body = body.Replace("@@ContactorName",jobAd.ContactName);
            body = body.Replace("@@ContactorPhone", jobAd.ContactPhone);
            body = body.Replace("@@ContactorEmail", jobAd.ContactEmail);
            body = body.Replace("@@WorkType", jobAd.WorkType);
            body = body.Replace("@@PayType", jobAd.PayType);
            body = body.Replace("@@ContractType", jobAd.ContractType);
            body = body.Replace("@@RoleDesciption", jobAd.RoleDescription);
            body = body.Replace("@@ContactorPosition", jobAd.ContactPositionTitle);

            body = body.Replace("@@HasCandidate", surveyViewModel.HasCandidate?"Yes":"No");
            body = body.Replace("@@IsUoaStudent", surveyViewModel.IsUoaStudent? "Yes" : "No");
            body = body.Replace("@@IsFromOurWeb", surveyViewModel.IsFromOurWeb? "Yes" : "No");
            body = body.Replace("@@IsSatisfied", surveyViewModel.IsSatisfied? "Yes" : "No");
            body = body.Replace("@@CandidateQty", surveyViewModel.CandidateQty.ToString());
            body = body.Replace("@@Feedback", surveyViewModel.Feedback);
            return body;
        }


        private string GetSurveyEmailBody(int jobAdId)
        {
            //    /wwwroot/survey-email.html
            string body = File.ReadAllText(Path.Combine(_env.WebRootPath, "survey-email.html"));
            var jobAd = (from j in _dbContext.JobAds
                         join ca in _dbContext.Categories on j.CategoryId equals ca.Id
                         join wt in _dbContext.WorkTypes on j.WorkTypeId equals wt.Id
                         join pt in _dbContext.PayTypes on j.PayTypeId equals pt.Id
                         join ct in _dbContext.ContractTypes on j.ContractTypeId equals ct.Id
                         where j.Id == jobAdId
                         select j).FirstOrDefault();

            string surveyUrl = _config.GetValue<string>("HostUrl") + "/employer/survey?id=" + jobAd.Id + "&key=" + jobAd.AuthKey;
            string newAdUrl = _config.GetValue<string>("HostUrl") + "/employer";
            body = body.Replace("@@JOBAD_TITLE", jobAd.Position);
            body = body.Replace("@@CONTACT_NAME", jobAd.ContactName);
            body = body.Replace("@@SURVEY_URL", surveyUrl);
            body = body.Replace("@@NEW_JOBAD_URL", newAdUrl);
            return body;
        }


    }
}
