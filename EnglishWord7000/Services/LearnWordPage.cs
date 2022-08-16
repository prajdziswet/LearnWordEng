using EnglishWord7000.Models;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Services
{
    public class LearnWordPage
    {
        private AplicationContext DbContext;
        private User user;
        private PropertyUser propertyUser; 
        private Levels levels;
        private List<Word> words;

        private List<Tuple<int,string>> linkAdditionWord;
        private Word word;

        public void NextIndex()
        {
            if (index < words.Count) index++;
        }

        public LearnWordPage(AplicationContext DB, IHttpContextAccessor contextAccessor)
        {
            DbContext = DB;
            user = DbContext.Users.AsNoTracking().FirstOrDefault(x => x.Login == contextAccessor.HttpContext.User.Identity.Name);

            if (user != null)
            {
                propertyUser = DbContext.PropertyUsers.FirstOrDefault(x => x.User == user);
                levels = new Levels(propertyUser.level);
                SetListWords();
                SetWord();
            } else throw new ArgumentNullException("user in LearnWordPage");

        }

        private void SetListWords()
        {
            int takeWord = propertyUser.WordLearn;
            if (takeWord<levels.FinishLevel- propertyUser.StartLearn - propertyUser.WordLearn) takeWord= levels.FinishLevel-propertyUser.StartLearn - propertyUser.WordLearn;
            words =DbContext.Words.AsNoTracking().Skip(propertyUser.StartLearn).Take(takeWord).Include(x => x.WordEng)
                .Include(x => x.WordRu).ToList();
        }

        private void SetWord()
        {
            if (words.Count != 0)
            {
                word = words.First();
                linkAdditionWord = DbContext.Words.AsNoTracking().Skip(levels.FinishLevel)
                    .Take(levels.FinishAddWord - levels.FinishLevel).Where(x => x.WordEng.Word == word.WordEng.Word)
                    .Select(x =>new Tuple<int,string>(x.Id,x.WordEng.obj)).ToList();
            }
        }


        public string GetPage(int id=0)
        {
            if (word!=null&&id==0)
            {
                return word.WordEng.HtmlEng + word.WordRu.HtmlRu;
            }
            else if (word != null)
            {
                Word wordtemp = DbContext.Words.AsNoTracking().Where(x => x.Id == id).Include(x=>x.WordEng).Include(x=>x.WordRu).FirstOrDefault();
                if (wordtemp != null)
                    return wordtemp.WordEng.HtmlEng + word.WordRu.HtmlRu;
                else return null;
            }
            else return null;
        }

        public string GetLinkAdditionWord()
        {
            string retstr = "";
            if (linkAdditionWord.Count > 0)
            {
                foreach (Tuple<int,string> element in linkAdditionWord)
                {
                    retstr += $"<a asp-action=\"Index\" asp-route-id=\"{element.Item1}\">{word.WordEng.Word+element.Item2}</a><br>";
                }
            }

            return retstr;
        }
    }
}
