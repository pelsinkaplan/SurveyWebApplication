using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SurveyWebApplication.Models;
using SurveyWebApplication.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SurveyWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserService userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);
            ViewBag.User = user;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
