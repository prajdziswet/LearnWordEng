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
            //Console.WriteLine(dbContext.Words.Count());
            //if (dbContext.Words.Count() == 0)
            //{
            //    List<Word> allWords = new List<Word>();
            //    var group=dbContext.Words.Where(x=>x.WordEng.Level!=null).Include(x=>x.WordRu).Include(x=>x.WordEng).ToList().GroupBy(x=>x.WordEng.Level).OrderBy(x=>x.Key).ToDictionary(x=>x.Key,x=>x.ToList());

            //    List<string> consolewrite = new List<string>();
            //   foreach (var levelgroup in group)
            //   {
            //       if (levelgroup.Key != null)
            //       {
            //           var wordanother= dbContext.Words.Include(x => x.WordRu).Include(x => x.WordEng).ToList().Where(x => levelgroup.Value.Any(y => x.WordEng.Word == y.WordEng.Word && x.WordEng.Level != y.WordEng.Level)).ToList();
            //           levelgroup.Value.ShuffleInPlace();
            //           AddnewWords(levelgroup.Value,allWords);
            //           AddnewWords(wordanother, allWords);
            //       }

            //   }
            //   dbContext.Words.RemoveRange(dbContext.Words);
            //   dbContext.SaveChanges();
            //   Console.WriteLine("all:  " + allWords.Count);
            //   dbContext.Words.AddRange(allWords);
            //   dbContext.SaveChanges();
            //}
            //Console.WriteLine(dbContext.Words.Count());

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