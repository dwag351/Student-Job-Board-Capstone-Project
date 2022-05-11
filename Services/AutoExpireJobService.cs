using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using project.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace project.Services
{
    public class AutoExpireJobService : BackgroundService
    {
        private readonly AppDbContext _dbContext;
        private readonly EmailService _emailService;


        public AutoExpireJobService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            _emailService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<EmailService>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
               
                await new TaskFactory().StartNew(() =>
                {
                    try
                    {

                        /// Totally 4 statuses
                        /// 1:pending
                        /// 2:approved
                        /// 3:rejected
                        /// 4:expired
                        // get all job ads which meet the expired condition
                        var expiredJobs = _dbContext.JobAds.Where(j => j.JobStatusId == 2 && j.ApprovalDate <= DateTime.Now.AddMonths(-1)).ToList();
                        foreach (var job in expiredJobs)
                        {
                            job.JobStatusId = 4;//change the status to expired
                            _dbContext.Entry(job).State = EntityState.Modified;//mark this record should be modified
                            _emailService.SendSurveyToEmployer(job.Id);
                        }
                        _dbContext.SaveChanges(); //save in the database                   
                    }
                    catch (Exception exp)
                    {
                     
                    }

                   
                    Thread.Sleep(60* 1000);//ervey 60 seconds will trigger the event
                });
            }



        }

       
    }
}
