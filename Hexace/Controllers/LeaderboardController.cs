using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Data;
using Hexace.Data.Objects;
using Hexace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hexace.Controllers
{
    public class LeaderboardController : Controller
    {
        public class UserInfo
        {
            public int Value { get; set; }
            public string Nickname { get; set; }
        }

        private HexaceContext db;

        public LeaderboardController(HexaceContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("ChooseLeaderboard")]
        public JsonResult ChooseLeaderboard(string leaderboardFraction)
        {
            IQueryable<Profile> users = null;

            switch (leaderboardFraction)
            {
                case "ta": // Turquoise Attack
                    users = db.Profiles.Where(x => x.FractionId == 1).OrderByDescending(x => x.SuccessfulAttacks).Take(5);
                    break;

                case "td": // Turquoise Defence
                    users = db.Profiles.Where(x => x.FractionId == 1).OrderByDescending(x => x.SuccessfulDefences).Take(5);
                    break;

                case "ba": // Burgundy Attack
                    users = db.Profiles.Where(x => x.FractionId == 2).OrderByDescending(x => x.SuccessfulAttacks).Take(5);
                    break;

                case "bd": // Burgundy Defence
                    users = db.Profiles.Where(x => x.FractionId == 2).OrderByDescending(x => x.SuccessfulDefences).Take(5);
                    break;

                case "pa": // Purple Attack
                    users = db.Profiles.Where(x => x.FractionId == 3).OrderByDescending(x => x.SuccessfulAttacks).Take(5);
                    break;

                case "pd": // Purple Defence
                    users = db.Profiles.Where(x => x.FractionId == 3).OrderByDescending(x => x.SuccessfulDefences).Take(5);
                    break;
            }

            var userInfo = new List<UserInfo>();
            foreach (var item in users)
            {
                var tmp = new UserInfo();
                tmp.Value = leaderboardFraction[1] == 'a' ? item.SuccessfulAttacks : item.SuccessfulDefences;
                tmp.Nickname = db.Users.Where(x => x.Id == item.UserId).First().Nickname;
                userInfo.Add(tmp);
            }
            return new JsonResult(userInfo);
        }
    }
}