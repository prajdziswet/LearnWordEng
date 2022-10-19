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
        public HomeController(IRepeatWordPage repeatWordPage)
        {
            this.repeatWordPage = repeatWordPage;
            //List<Word> words = DB.Words.Include(x => x.WordRu).Include(x => x.CheckWords).ToList();
            //foreach (Word element in words)
            //{
            //    if (element.CheckWords.Count == 0)
            //    {
            //        element.CheckWords = WriteInHtmlRu.ReturnList(element.WordRu.HtmlRu);
            //    }
            //}

            //DB.SaveChanges();
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