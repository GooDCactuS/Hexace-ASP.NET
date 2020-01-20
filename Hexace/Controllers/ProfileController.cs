using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hexace.Data;
using Hexace.Data.Objects;
using Hexace.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Hexace.Controllers
{
    public class ProfileController : Controller
    {
        private UserContext db;

        public ProfileController(UserContext context)
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
            if (ModelState["Email"].ValidationState == ModelValidationState.Valid && ModelState["Password"].ValidationState == ModelValidationState.Valid)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Home");
                }
                
            }
            ModelState.AddModelError("", "Incorrect email or password.");
            return View("Login", model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.PasswordConfirmation)
                {
                    ModelState.AddModelError("", "Password and password confirmation field are not the same.");
                }
                else
                {
                    User user = await db.Users.FirstOrDefaultAsync(u =>
                        u.Email == model.Email || u.Nickname == model.Nickname);
                    if (user == null)
                    {
                        db.Users.Add(new User
                        {
                            Nickname = model.Nickname,
                            Email = model.Email,
                            Password = model.Password,
                            RegistrationDate = DateTime.Today.Date,
                            UserType = "Player"
                        });

                        await db.SaveChangesAsync();

                        db.Profiles.Add(new Profile
                        {
                            AttackAttempts = 0,
                            DefenseAttempts = 0,
                            SeasonId = 1,
                            FractionId = new Random().Next(1, 4),
                            SuccessfulAttacks = 0,
                            SuccessfulDefences = 0,
                            UserId = db.Users.First(x => x.Nickname == model.Nickname).Id
                        });

                        await db.SaveChangesAsync();
                        await Authenticate(model.Email);

                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("", "User with this email or nickname exists.");
                }
            }

            return View("Login", model);
        }

        private async Task Authenticate(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Profile");
        }

        [Authorize]
        public IActionResult Index()
        {
            var user = db.Users.First(x => x.Email == HttpContext.User.Identity.Name);
            var profile = db.Profiles.First(x => x.UserId == user.Id);
            var userAchievements = db.UsersAchievements.Where(x=>x.UserId==user.Id).ToList();
            var achievements = db.Achievements.ToList();
            
            var model = new ProfileModel(user, profile, achievements, userAchievements);
            
            
            return View("Index", model);
        }
    }
}