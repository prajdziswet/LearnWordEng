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
        private LearnWordPage learnWordPage;

        public Learn(AplicationContext DB, IHttpContextAccessor contextAccessor)
        {
            if (learnWordPage==null) { learnWordPage = new LearnWordPage(DB, contextAccessor); }
        }
        public IActionResult Index(int? id)
        {
            if (id!=null) learnWordPage.NextIndex();
            @ViewData["Page"]= learnWordPage.GetPage();
            return RedirectToAction("LearnWord");
            //return View();
        }

        public IActionResult LearnWord(int? id)
        {
            return View();
        }
    }
}
