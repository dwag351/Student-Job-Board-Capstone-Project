using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Infrastructure
{
    public class AppDbContext : DbContext
    {
        //constructor 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
 
        public DbSet<JobAd> JobAds { get; set; }
        public DbSet<JobStatus> JobStatuses { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<PayType> PayTypes { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser
                {
                    Id=1,
                    Email="myjobseek399@gmail.com",
                    Name="Admin",
                    Password="project399"
                });
            modelBuilder.Entity<PayType>().HasData(
                new PayType
                {
                    Id = 1,
                    Name = "Annual"
                },
                new PayType
                {
                    Id = 2,
                    Name = "Hourly"
                },
                new PayType
                {
                    Id = 3,
                    Name = "Market Rate"
                }
                );
            modelBuilder.Entity<WorkType>().HasData(
                new WorkType
                {
                    Id = 1,
                    Name = "Full Time"
                },
                 new WorkType
                 {
                     Id = 2,
                     Name = "Part Time"
                 }
                 
                );
            modelBuilder.Entity<ContractType>().HasData(
               new ContractType
               {
                   Id = 1,
                   Name = "Permanent"
               },
                new ContractType
                {
                    Id = 2,
                    Name = "Fixed"
                },
                new ContractType
                {
                    Id = 3,
                    Name = "Casual"
                }

               );
            modelBuilder.Entity<JobStatus>().HasData(
              new JobStatus
              {
                  Id = 1,
                  Name = "Pending",
                  Color= "#007bff"
              },
              new JobStatus
              {
                  Id = 2,
                  Name = "Approved",
                  Color = "#28a745"
              },
              new JobStatus
              {
                  Id = 3,
                  Name = "Rejected",
                   Color = "#dc3545"
              },
                new JobStatus
                {
                    Id = 4,
                    Name = "Expired",
                    Color = "#6c757d"
                }
            
              );
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Support specialist"
                },
                new Category
                {
                    Id = 2,
                    Name = "Computer programmer"
                },
                new Category
                {
                    Id = 3,
                    Name = "Quality assurance tester"
                },
                new Category
                {
                    Id = 4,
                    Name = "Web developer"
                },
                new Category
                {
                    Id = 5,
                    Name = "IT technician"
                },
                new Category
                {
                    Id = 6,
                    Name = "Systems analyst"
                },
                new Category
                {
                    Id = 7,
                    Name = "Network engineer"
                },
                new Category
                {
                    Id = 8,
                    Name = "User experience designer"
                },
                new Category
                {
                    Id = 9,
                    Name = "Database administrator"
                },
                    new Category
                    {
                        Id = 10,
                        Name = "Computer scientist"
                    },
                      new Category
                      {
                          Id = 11,
                          Name = "Software engineer"
                      },
                        new Category
                        {
                            Id = 12,
                            Name = "IT security specialist"
                        },
                          new Category
                          {
                              Id = 13,
                              Name = "Data scientist"
                          },
                        new Category
                        {
                            Id = 14,
                            Name = "IT director"
                        },
                        new Category
                        {
                            Id = 15,
                            Name = "Others"
                        }

                );
        }
    }
}
