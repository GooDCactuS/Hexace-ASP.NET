using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hexace.Models;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using Hexace.Data.Objects;
using Newtonsoft.Json;

namespace Hexace.Controllers
{
    public class HomeController : Controller
    {
        private HexaceContext db;

        public HomeController(HexaceContext context)
        {
            db = context;
        }

        [HttpPost]
        [Authorize]
        public IActionResult BoardActionResult(HomeModel model)
        {
            var ediCell = MainLogic.GameModel.Cells.First(x => x.x == model.X && x.y == model.Y);
            ediCell.isStroked = true;
            ediCell.colorAttack = db.Fractions.First(x => x.Id == GetCurrentUserFraction()).Color;
            ediCell.LastAttackTime = Timer.GetTimeNow();

            var editCell = db.FieldCells.First(x => x.X == model.X && x.Y == model.Y);
            editCell.IsStroked = true;
            editCell.FractionAttackId = GetCurrentUserFraction();
            db.SaveChanges();

            MainLogic.Timer.UpdateClickUser(db.Users.First(u => u.Email == HttpContext.User.Identity.Name).Id);
            model.LastClick =
                MainLogic.Timer.UsersLastClicks[db.Users.First(u => u.Email == HttpContext.User.Identity.Name).Id];
            model.CellString = GetJsonString(MainLogic.GameModel.Cells);
            return View("Index", model);
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
            //MainLogic.UpdateTimerForUser(userId);
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

        [HttpGet]
        [Route("UpdateField")]
        public string UpdateField()
        {
            HomeModel model = new HomeModel();
            model.CellString = GetJsonString(MainLogic.GameModel.Cells);

            if (model.CellString == null)
            {
                return "null";
            }

            return model.CellString;
        }

        [HttpPost]
        [Route("SendUserAction")]
        public string SendUserAction(string userAction, string strX, string strY)
        {

            var ediCell = MainLogic.GameModel.Cells.First(x => x.x == model.X && x.y == model.Y);
            ediCell.isStroked = true;
            ediCell.colorAttack = db.Fractions.First(x => x.Id == GetCurrentUserFraction()).Color;
            ediCell.LastAttackTime = Timer.GetTimeNow();

            var editCell = db.FieldCells.First(x => x.X == model.X && x.Y == model.Y);
            editCell.IsStroked = true;
            editCell.FractionAttackId = GetCurrentUserFraction();
            db.SaveChanges();

            MainLogic.Timer.UpdateClickUser(db.Users.First(u => u.Email == HttpContext.User.Identity.Name).Id);
            model.LastClick =
                MainLogic.Timer.UsersLastClicks[db.Users.First(u => u.Email == HttpContext.User.Identity.Name).Id];
            model.CellString = GetJsonString(MainLogic.GameModel.Cells);
            return View("Index", model);
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
