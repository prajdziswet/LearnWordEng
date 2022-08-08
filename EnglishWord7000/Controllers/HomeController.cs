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
                return View();
        }

        //private void AddnewWords(IList<Word> addList,List<Word> allword)
        //{
        //    foreach (var element in addList)
        //    {
        //        Word newWord=new Word();
        //        newWord.WordEng = element.WordEng;
        //        newWord.WordRu = element.WordRu;
        //        newWord.CheckWords =element.CheckWords;
        //        allword.Add(newWord);
        //    }
        //}

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}