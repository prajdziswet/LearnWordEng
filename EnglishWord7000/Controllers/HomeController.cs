using EnglishWord7000.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AplicationContext dbContext;
        public HomeController(ILogger<HomeController> logger, AplicationContext Context)
        {
            dbContext=Context;
            _logger = logger;
        }

        public IActionResult Index()
        {

            //if (dbContext.Words.Count() == 0)
            //{
            //    CreateList cr = new CreateList();
            //    //dbContext.Words.RemoveRange(dbContext.Words);
            //    dbContext.Words.AddRange(cr.Words);
            //    dbContext.SaveChanges();
            //}

            return View();
        }

        [Authorize]
        public IActionResult Learn()
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