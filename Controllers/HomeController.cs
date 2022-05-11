using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using project.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public HomeController(AppDbContext appDbContext, ILogger<HomeController> logger, IConfiguration config)
        {
            _dbContext = appDbContext;
            _logger = logger;
            _config = config;
        }
        //show student login page
        public IActionResult Index()
        {
            ViewData["GoogleClientId"] = _config.GetValue<string>("GoogleClientId");
            return View();
        }
       
   

    }
}
