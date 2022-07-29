using EnglishWord7000.Models;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Services
{
    public class LearnWordPage
    {
        private AplicationContext DbContext { get; set; }
        private List<Word> words { get; set; }
        private StatusLearn statusLearn { get; set; }
        private int index { get; set; }=0;
        public void NextIndex()
        {
            if (index < words.Count) index++;
        }

        public LearnWordPage(AplicationContext DB, IHttpContextAccessor contextAccessor)
        {
            DbContext = DB;
            var x = DbContext.StatusLearns.Count();
            statusLearn = DbContext.StatusLearns.Include(x => x.LearnWords).FirstOrDefault(x => x.User.Login == contextAccessor.HttpContext.User.Identity.Name);
            if (statusLearn == null) throw new ArgumentNullException();
            else if (statusLearn.LearnWords.Count == 0)
            {
                words = DbContext.Words.Where(x => x.WordEng.Level == statusLearn.level).Include(x => x.WordEng)
                    .Include(x => x.CheckWords).Include(x => x.WordRu).ToList();
            }
            else
            {
                words = DbContext.Words.Where(x => x.WordEng.Level == statusLearn.level && !statusLearn.LearnWords.Any(y => y.learnedWord == x))
                    .Include(x => x.WordEng)
                    .Include(x => x.CheckWords).Include(x => x.WordRu).ToList();
            }

            words.ShuffleInPlace();
        }


        public string GetPage()
        {
            if (index < words.Count)
            {
                return words[index].WordEng.HtmlEng + words[index].WordRu.HtmlRu;
            }
            else
                return "";
        }
    }
}
