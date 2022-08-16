using System.Security.Claims;
using EnglishWord7000.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishWord7000.Controllers
{
    public class Account : Controller
    {
        private AplicationContext db;
        public Account(AplicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.Login == model.Email);
                if (user != null)
                {
                    user = await db.Users.FirstOrDefaultAsync(u =>
                        u.Email == model.Email || u.Login == model.Email && u.Password == model.Password);

                    if (user != null)
                    {
                        await Authenticate(user.Login); // аутентификация

                        return RedirectToAction("Index", "Home");
                    }
                    else ModelState.AddModelError("", "Не карэктны пароль");
                }
                ModelState.AddModelError("", "Такога логіна/email не існуе");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.Login == model.Login);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    User newuser = new User { Login = model.Login, Email = model.Email, Password = model.Password };
                    db.Users.Add(newuser);
                    db.PropertyUsers.Add(new PropertyUser { User = newuser, level = Levels.levels.First() });
                    db.SaveChanges();

                    await Authenticate(model.Login); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else if (user.Login == model.Login)
                    ModelState.AddModelError("Login", "Некарэктны логін, it's exist");
                else
                    ModelState.AddModelError("Email", "Некарэктны Email, it's exist");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
