using System.Runtime.CompilerServices;
using System.Text;
using EnglishWord7000.Models;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Services;

public class RepeatWordPage
{
    #region Value
    private AplicationContext DbContext { get; set; }
    private List<LearnWord> LearnWords { get; set; }

    private bool ExistPage
    {
        get
        {
            if (LearnWords.Count == 0) return false;
            else return true;
        }
    }
    #endregion



    public RepeatWordPage(AplicationContext DB, string UserName)
    {
        if (UserName.IsNullOrEmpty()) throw new ArgumentNullException("UserName in RepeatWordPage");
        DbContext = DB;

        LearnWords = DbContext.LearnWord.Where(x => x.User.Login == UserName&& x.FistTime != null && x.FistTime <= DateTime.Now.AddDays(-1) && x.Repeat == 1).Include(y =>
            y.learnedWord).ToList();
    }

    public string GetPage()
    {
        if (ExistPage)
        {
            return null;
        }
        else return null;
    }
}