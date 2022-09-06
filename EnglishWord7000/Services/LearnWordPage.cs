using EnglishWord7000.Models;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Services
{
    public class LearnWordPage:ILearnWordPage
    {
        private AplicationContext Db;
        private int Id;
        private int? count;
        private User user;
        private PropertyUser propertyUser;
        private Levels levels;
        private ISetCookies setCookies;
        private IGetCookies getCookies;
        private Word word { get;  set; }
        private List<Tuple<int, string>> linkAdditionWord;


        public LearnWordPage(AplicationContext Db, IHttpContextAccessor contextAccessor)
        {
            this.Db = Db;
            this.setCookies=new SetCookies(contextAccessor.HttpContext.Response.Cookies);
            getCookies = new GetCookies(contextAccessor.HttpContext.Request.Cookies);
            user = Db.Users.FirstOrDefault(x => x.Login == contextAccessor.HttpContext.User.Identity.Name);
            if (user!=null)
            {
                propertyUser = Db.PropertyUsers.FirstOrDefault(x => x.User == user);
                levels = new Levels(propertyUser.level);
                if (getCookies.count.HasValue)
                {
                    Id= getCookies.Id;
                    count = getCookies.count.Value;
                }
                PrepareThisClass();
            }
        }

        private void PrepareThisClass()
        {
            if (count.HasValue)
            {
                SetWord();
            }
            else
            {
                SetCountFist();
                SetIdWord();
                SetCookies();
            }
        }

        private void SetWord()
        {
            if (Id!=0)
            {
            word = Db.Words.Include(x=>x.WordEng).Include(x=>x.CheckWords).Include(x=>x.WordRu).FirstOrDefault(x => x.Id == Id);

            if (word!=null) linkAdditionWord = Db.Words.AsNoTracking().Skip(levels.FinishLevel)
                    .Take(levels.FinishAddWord - levels.FinishLevel).Where(x => x.WordEng.Word == word.WordEng.Word)
                    .Select(x =>new Tuple<int,string>(x.Id,x.WordEng.obj)).ToList();
            }

        }

        public string GetRawPage(int id=0)
        {
            if (Id != 0&&id==0)
            {
                return word.WordEng.HtmlEng + word.WordRu.HtmlRu;
            }
            else if (id != 0)
            {
                Word wordtemp = Db.Words.AsNoTracking().Skip(levels.FinishLevel)
                    .Take(levels.FinishAddWord - levels.FinishLevel).Where(x => x.Id == id).Include(x => x.WordEng).Include(x => x.WordRu).FirstOrDefault();
                if (wordtemp != null)
                    return wordtemp.WordEng.HtmlEng + word.WordRu.HtmlRu;
                else return null;
            }
            else return null;
        }

        public string GetLinkAdditionWords()
        {
                string retstr = "";
                if (linkAdditionWord.Count > 0)
                {
                    foreach (Tuple<int, string> element in linkAdditionWord)
                    {
                        string temp = (element.Item2 != null) ? $"  <strong>{element.Item2}</strong>" : "";
                        retstr += $"<a asp-action=\"Index\" asp-route-id=\"{element.Item1}\">{word.WordEng.Word + temp}</a><br>";
                    }
                    return retstr;
                } else return null;
        }

        public bool Next()
        {
            if (count < propertyUser.WordLearn)
            {
                count++;
                SaveChange();
                return true;
            }
            else return false;
        }

        private void SetCountFist()
        {
            count=Db.LearnWord.Where(x => x.User == user).AsEnumerable().Where(x => x.FistTime.Date == DateTime.Now.Date).Count();
        }

        private void SetIdWord()
        {
            Id = Db.Words.Skip(propertyUser.StartLearn).FirstOrDefault().Id;
        }

        private void SaveChange()
        {
            LearnWord learnWord = new LearnWord()
            {
                learnedWord = word,
                Repeat = 0,
                User = user,
                FistTime = DateTime.Now
            };
            Db.LearnWord.Add(learnWord);
            if (levels.FinishLevel >= propertyUser.StartLearn + 1)
                propertyUser.StartLearn++;
            else
            {
                levels.Next();
                propertyUser.level = levels.Level;
                propertyUser.StartLearn = levels.StartLevel;
            }
            Db.SaveChanges();
            SetIdWord();
            SetCookies();
        }

        private void SetCookies()
        {
            setCookies.Set(Id,count.Value);
        }
    }
}
