using System.Security.Claims;
using EnglishWord7000.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishWord7000.Controllers
{
    [Authorize]
    public class Progress : Controller
    {
        private AplicationContext dbContext;
        private User user;

        public Progress (AplicationContext dbContext, IHttpContextAccessor contextAccessor)
        {
            this.dbContext = dbContext;
            user = dbContext.Users.FirstOrDefault(x => x.Email == contextAccessor.HttpContext.User.Identity.Name);
        }

        public IActionResult Index()
        {
            StatusLearn statusLearn = dbContext.StatusLearns.Where(x => x.User.Login == User.Identity.Name).Include(x => x.User).Include(x => x.LearnWords).FirstOrDefault();
            ViewData["Name"] = statusLearn.User.Login;
            ViewData["level"] = statusLearn.level;
            ViewData["count"] = statusLearn.LearnWords.Count;
            return View();
        }
        [HttpGet]
        public IActionResult Password()
        {
            ViewData["Name"] = user.Login;
            ViewData["Email"] = user.Email;
            return View();
        }
        [HttpPost]
        public IActionResult Password(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                user.Password= registerModel.Password;
                dbContext.SaveChanges();
                return Index();
            }
            else
            {
                ModelState.AddModelError("PasswordConfirm", "Паролі не супадаюць");
                return View(registerModel);
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Name"] = user.Login;
            ViewData["Email"] = user.Email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (registerModel.Email != user.Email &&
                    dbContext.Users.FirstOrDefault(x => x.Email == registerModel.Email) != null)
                {
                    ModelState.AddModelError("Email", "Такі Email есць у базе");
                }
                else if(registerModel.Login != user.Login &&
                        dbContext.Users.FirstOrDefault(x => x.Login == registerModel.Login) != null)
                {
                    ModelState.AddModelError("Login", "Такі Login есць у базе");
                }
                else if (registerModel.Email != user.Email || registerModel.Login != user.Login)
                {
                    user.Email = registerModel.Email;
                    user.Login = registerModel.Login;
                    dbContext.SaveChanges();
                    await OutSignIn(registerModel.Email);
                    return Index();
                }

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Штосьці неадпавядае бяспеке");
                return View(registerModel);
            }

        }

        private async Task OutSignIn(string username)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
