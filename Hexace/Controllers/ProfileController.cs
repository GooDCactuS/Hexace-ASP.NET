using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private HexaceContext db;

        public ProfileController(HexaceContext context)
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
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null && VerifyHashedPassword(user.Password, model.Password))
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
                            Password = HashPassword(model.Password),
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

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
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

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            for (int i = 0; i < buffer3.Length; i++)
            {
                if (buffer3[i] != buffer4[i])
                {
                    return false;
                }
            }

            return true;
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