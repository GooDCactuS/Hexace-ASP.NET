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
            var editCell = db.FieldCells.First(x => x.X == model.X && x.Y == model.Y);
            editCell.IsStroked = true;
            editCell.FractionAttackId = 2;
            db.SaveChanges();

            model.Cells = HomeModel.GetObjectCells(model.CellString); //нужно понять как нормально десериализировать

            model.Cells[model.Id].isStroked = true;
            model.Cells[model.Id].colorAttack = db.Fractions.First(x => x.Id == GetCurrentUserFraction()).Color; //проверка fraction id пользователя
            return View("Index", model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(HomeModel model)
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

            model.Cells = new List<ObjectCell>();
            foreach (var cell in db.FieldCells.ToList())
            {
                var colorDef = cell.IsFilled ? db.Fractions.First(x => x.Id == cell.FractionDefId).Color : null;
                var colorAttack = cell.IsStroked ? db.Fractions.First(x => x.Id == cell.FractionAttackId).Color : null;
                model.Cells.Add(new ObjectCell(cell.X, cell.Y, cell.IsFilled, cell.IsStroked, colorAttack, colorDef));
            }

            model.CellString = HomeModel.GetJsonString(model.Cells);
            return View(model);
        }


        public int GetCurrentUserFraction()
        {
            var user = db.Users.First(u => u.Email == HttpContext.User.Identity.Name);
            var profile = db.Profiles.First(p => p.UserId == user.Id);
            return profile.FractionId;
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
