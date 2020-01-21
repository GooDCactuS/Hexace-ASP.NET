using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hexace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hexace.Models;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using Hexace.Data.Objects;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Timer = Hexace.Models.Timer;

namespace Hexace.Controllers
{
    public class HomeController : Controller
    {
        private HexaceContext db;

        public HomeController(HexaceContext context)
        {
            db = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var fractions = db.Fractions.ToList();
            //var side = 10;
            //var width = 30;
            //var start = side - 1;
            //var count = 0;
            ////отступ в зависимости от количества ячеек
            //var indent = 0;
            //for (var j = 0; j < side * 2 - 1; j++)
            //{
            //    if (j < side)
            //    {
            //        start++;
            //        if (start % 2 == 0)
            //            indent++;
            //    }
            //    else
            //    {
            //        start--;
            //        if (start % 2 == 1)
            //            indent--;
            //    }

            //    for (var i = width / 2 - start + indent; i < width / 2 + indent; i++)
            //    {
            //        var obj = (x: i, y: j, color: fractions[0].Color, isFill: false);
            //        if (!db.FieldCells.Any(x => x.X == i && x.Y == j))
            //            db.FieldCells.Add(new FieldCell
            //            {
            //                Id = ++count,
            //                X = i,
            //                Y = j,
            //                IsFilled = false,
            //                IsStroked = false
            //            });
            //        db.SaveChanges();
            //    }


            //}
            HomeModel model = new HomeModel();
            var userId = db.Users.First(u => u.Email == HttpContext.User.Identity.Name).Id;
            if (!MainLogic.Timer.UsersLastClicks.ContainsKey(userId))
            {
                MainLogic.Timer.UpdateClickUser(userId);
            }
            model.LastClick = MainLogic.Timer.UsersLastClicks[userId];
            model.CellString = GetJsonString(MainLogic.GameModel.Cells);
            return View(model);
        }

        public static List<ObjectCell> GetObjectCells(string str)
        {
            return JsonConvert.DeserializeObject<ObjectCell[]>(str).ToList();
        }

        public static string GetJsonString(List<ObjectCell> cells)
        {
            return JsonConvert.SerializeObject(cells);
        }

        public int GetCurrentUserFraction()
        {
            var user = db.Users.First(u => u.Email == HttpContext.User.Identity.Name);
            var profile = db.Profiles.First(p => p.UserId == user.Id);
            return profile.FractionId;
        }

        public string GetCurrentUserFractionColor()
        {
            return db.Fractions.First(x => x.Id == GetCurrentUserFraction()).Color;
        }

        [HttpPost]
        [Route("UpdateField")]
        public string UpdateField(string userState)
        {
            var cells = GetObjectCells(userState);
            var isDifferent = false;
            foreach (var item in cells)
            {
                if (!MainLogic.GameModel.Cells.Any(c => c.Equals(item)))
                {
                    isDifferent = true;
                    break;
                }
            }

            if (isDifferent)
            {
                var cellString = GetJsonString(MainLogic.GameModel.Cells);

                if (cellString == null)
                {
                    return "null";
                }

                return cellString;
            }



            return "null";
        }

        [HttpPost]
        [Route("SendUserAction")]
        public string SendUserAction(string userAction)
        {
            var coords = userAction.Split(' ');
            var coordX = Int32.Parse(coords[0]);
            var coordY = Int32.Parse(coords[1]);

            var userId = db.Users.First(u => u.Email == HttpContext.User.Identity.Name).Id;

            if (MainLogic.Timer.UsersLastClicks[userId] < Timer.GetTimeNow())
            {
                var ediCell = MainLogic.GameModel.Cells.First(x => x.x == coordX && x.y == coordY);

                if (ediCell.colorDef == GetCurrentUserFractionColor() && ediCell.colorAttack == null)
                {
                    return null;
                }

                if (ediCell.colorAttack == GetCurrentUserFractionColor())
                {
                    return null;
                }

                
                ediCell.LastAttackTime = (long)Timer.GetTimeNow();
                ediCell.PlayerId = userId;

                if (ediCell.colorDef == GetCurrentUserFractionColor())
                {
                    GameModel.IncreasePlayersStats(userId, GameModel.Stats.SuccessfulDefends);
                    ediCell.isStroked = false;
                }
                else
                {
                    GameModel.IncreasePlayersStats(userId, GameModel.Stats.AttackAttempts);
                    ediCell.isStroked = true;
                }
                ediCell.colorAttack = ediCell.colorDef == GetCurrentUserFractionColor() ? null : GetCurrentUserFractionColor();
                
                
                MainLogic.UpdateTimerForUser(userId);
                return MainLogic.Timer.UsersLastClicks[userId].ToString();
            }


            return null;
        }

        [HttpGet]
        [Route("UpdateChat")]
        public JsonResult UpdateChat(string lastMessage)
        {
            var fractionId = GetCurrentUserFraction();
            var messages = new HomeModel(fractionId, lastMessage).Messages;
            if (messages == null)
            {
                return new JsonResult("null");
            }

            return new JsonResult(messages);

        }

        [HttpPost]
        [Route("SendMessage")]
        public void SendMessage(string message)
        {
            var user = db.Users.First(u => u.Email == HttpContext.User.Identity.Name);
            var profile = db.Profiles.First(p => p.UserId == user.Id);
            MainLogic.Chat.Chats[profile.FractionId].Add(new ChatMessage(user.Id, message, DateTime.Now, profile.FractionId));
            MainLogic.Chat.Users.Add(new User() { Id = user.Id, Nickname = user.Nickname });
        }
    }
}
