using EnglishWord7000.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EnglishWord7000.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Controllers
{
    public class HomeController : Controller
    {
        private IRepeatWordPage repeatWordPage;
        public HomeController(IRepeatWordPage repeatWordPage,AplicationContext DB)
        {
            this.repeatWordPage = repeatWordPage;
            Console.WriteLine(DB.Words.Count());
        }

        public IActionResult Index()
        {
            ViewBag.repeat = repeatWordPage.ExistPage;
                return View();
        }


            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}