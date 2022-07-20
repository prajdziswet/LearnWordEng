using System.Xml.Serialization;
using EnglishWord7000.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Controllers
{
    [Authorize]
    public class Learn : Controller
    {
        private AplicationContext dbContext;
        private List<Word> words;
        private StatusLearn statusLearn;
        private int index = 0;

        public Learn(AplicationContext DB, IHttpContextAccessor contextAccessor)
        {
            dbContext = DB;
            statusLearn = dbContext.StatusLearns.Where(x => x.User.Email == contextAccessor.HttpContext.User.Identity.Name).Include(x => x.LearnWords).FirstOrDefault();
            if (statusLearn == null) throw new ArgumentNullException();
            else if (statusLearn.LearnWords.Count==0)
            {
                words = dbContext.Words.Where(x => x.WordEng.Level == statusLearn.level).Include(x => x.WordEng)
                    .Include(x => x.CheckWords).Include(x => x.WordRu).ToList();
            }
            else
            {
                words = dbContext.Words.Where(x => x.WordEng.Level == statusLearn.level&& !statusLearn.LearnWords.Any(y=>y.learnedWord==x))
                    .Include(x => x.WordEng)
                    .Include(x => x.CheckWords).Include(x => x.WordRu).ToList();
            }

            words.ShuffleInPlace();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
