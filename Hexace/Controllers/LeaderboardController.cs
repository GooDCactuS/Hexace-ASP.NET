using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Data;
using Hexace.Data.Objects;
using Hexace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hexace.Controllers
{
    public class LeaderboardController : Controller
    {
        private UserContext db;

        public LeaderboardController(UserContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LeaderboardModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LeaderboardModel model)
        {
            IQueryable<Profile> users = null;
            
            switch (model.ButtonValue)
            {
                case "ta": // Turquoise Attack
                    users = db.Profiles.Where(x=>x.FractionId==1).OrderBy(x=>x.SuccessfulAttacks).Take(5);
                    break;

                case "td": // Turquoise Defence
                    users = db.Profiles.Where(x => x.FractionId == 1).OrderBy(x => x.SuccessfulDefences).Take(5);
                    break;

                case "ba": // Burgundy Attack
                    users = db.Profiles.Where(x => x.FractionId == 2).OrderBy(x => x.SuccessfulAttacks).Take(5);
                    break;

                case "bd": // Burgundy Defence
                    users = db.Profiles.Where(x => x.FractionId == 2).OrderBy(x => x.SuccessfulDefences).Take(5);
                    break;

                case "pa": // Purple Attack
                    users = db.Profiles.Where(x => x.FractionId == 3).OrderBy(x => x.SuccessfulAttacks).Take(5);
                    break;

                case "pd": // Purple Defence
                    users = db.Profiles.Where(x => x.FractionId == 3).OrderBy(x => x.SuccessfulDefences).Take(5);
                    break;
            }
            foreach (var item in users)
            {
                var userInfo = new LeaderboardModel.UserInfo();
                userInfo.ProfileInfo = item;
                userInfo.Nickname = db.Users.Where(x => x.Id == item.UserId).First().Nickname;
                model.Users.Add(userInfo);
            }
            return View("Index", model);
        }
    }
}