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
        //private LearnWordPageOld learnWordPage;
        private AplicationContext DB;

        public Learn(AplicationContext DB, IHttpContextAccessor contextAccessor)
        {
            this.DB = DB;
            //if (learnWordPage==null) { learnWordPage = new LearnWordPageOld(DB, contextAccessor); }
        }
        public IActionResult Index(int? id)
        {
            //if (id!=null) learnWordPage.NextIndex();
            //@ViewData["Page"]= learnWordPage.GetPage();
            Word word=DB.Words.Find(14400);
            
            return RedirectToAction("LearnWord");
            //return View();
        }

        public IActionResult LearnWord(uint? id=null)
        {
                //@ViewData["Page"] = learnWordPage.GetPage(id);
                //if (id == 0)
                //{
                //    id = null;
                //    return RedirectToAction("LearnWord");
                //}
                //@ViewData["id"] = id;
                //@ViewData["listAddition"] = learnWordPage.GetLinkAdditionWord();

            return View();
        }
    }
}
