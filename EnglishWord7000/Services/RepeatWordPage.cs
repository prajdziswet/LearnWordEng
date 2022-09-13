using System.Runtime.CompilerServices;
using System.Text;
using EnglishWord7000.Models;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Services;

public class RepeatWordPage: IRepeatWordPage
{
    #region Value
    private AplicationContext Db { get; set; }
    private User user;
    private LearnWord learnWord { get; set; }

    public bool ExistPage
    {
        get;
        private set;
    }
    #endregion

    public RepeatWordPage(AplicationContext DB, IHttpContextAccessor contextAccessor)
    {       
        this.Db= DB;
        user = Db.Users.AsNoTracking().FirstOrDefault(x => x.Login == contextAccessor.HttpContext.User.Identity.Name);
        GetLearnedWord();
    }

    public string GetPage()
    {
        if (ExistPage)
        {
            string strReturn = "";
            if (learnWord == null && learnWord.learnedWord.CheckWords.Count > 0)
            {
                foreach (CheckWord element in learnWord.learnedWord.CheckWords)
                {
                    strReturn += "<div class=\"checkword\"" +
                                 "<span class=\"eng\">" +
                                 $"{element.EngPhrase}" +
                                 "</span>" +
                                 "<span class=\"ru\">   " +
                                 $"{element.RuPhrase}" +
                                 "</span>" +
                                 "</div><br/>";
                }
            }
            return strReturn;
        }
        else return "";
    }

    public void Next()
    {
        learnWord.Repeat++;
        Db.SaveChanges();
        GetLearnedWord();
    }

    private void GetLearnedWord()
    {
        if (user != null)
        {
            learnWord = Db.LearnWord.Include(y =>
                y.learnedWord).FirstOrDefault(x => x.User == user && x.Repeat == 0&&x.FistTime <= DateTime.Now.AddDays(-1).Date);
        }

        if (user == null || learnWord == null) ExistPage = false;
        else ExistPage = true;
    }
}