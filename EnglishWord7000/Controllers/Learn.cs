using System.Xml.Serialization;
using EnglishWord7000.Models;
using EnglishWord7000.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Controllers
{
    [Authorize]
    public class Learn : Controller
    {
        private IRepeatWordPage repeatWordPage;
        private ILearnWordPage learnWordPage=null;
        private AplicationContext Db;

        public Learn(IRepeatWordPage repeatWordPage, AplicationContext Db, IHttpContextAccessor contextAccessor)
        {
            this.repeatWordPage = repeatWordPage;
            if (!repeatWordPage.ExistPage) learnWordPage = new LearnWordPage(Db,contextAccessor);
            ViewBag.repeat = repeatWordPage.ExistPage;
        }
        public IActionResult Index()
        {
            if (repeatWordPage.ExistPage) return Redirect("Learn/Repeat");
            else return Redirect("Learn/LearnWord");
        }

        [HttpGet]
        public IActionResult LearnWord(int id=0)
        {
            if (learnWordPage!=null)
            {
                SetViewDataLearnWord(id);
            }
            return View();
        }

        [HttpPost]
        public IActionResult LearnWord()
        {
            if (learnWordPage.Next())
            {
                SetViewDataLearnWord();
            }

            return View();
        }

        private void SetViewDataLearnWord(int id = 0)
        {
            @ViewData["Page"] = learnWordPage.GetRawPage(id);
            @ViewData["id"] = id;
            @ViewData["listAddition"] = learnWordPage.GetLinkAdditionWords();
            @ViewData["level"] = learnWordPage.word.WordEng.Level;
        }

        public IActionResult Repeat(bool boolit=false)
        {
            if (boolit)
            {
                repeatWordPage.Next();
                boolit = false;
            }
            if (repeatWordPage.ExistPage)
            {
                @ViewData["Page"] = repeatWordPage.GetPage();
            }
            else
            {
                return Redirect("Learn/LearnWord");
            }
            return View();
        }
    }
}
