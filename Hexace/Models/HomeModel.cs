using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hexace.Models
{
    public class ObjectCell
    {
        public ObjectCell(int x, int y, bool isFilled, bool isStroked, string colorAttack = null, string colorDef = null)
        {
            this.x = x;
            this.y = y;
            this.colorDef = colorDef;
            this.colorAttack = colorAttack;
            this.isFilled = isFilled;
            this.isStroked = isStroked;
        }
        public int x;
        public int y;
        public string? colorDef;
        public string? colorAttack;
        public bool isFilled;
        public bool isStroked;
    }
    public class HomeModel
    {
        public static List<ObjectCell> Cells = new List<ObjectCell>();
        public string UserMessage { get; set; }
        public List<string> Messages { get; set; }

        public static string GetJsonString()
        {
            return JsonConvert.SerializeObject(Cells.ToArray());
        }

        public HomeModel()
        {

        }

        public HomeModel(int fractionId)
        {
            Messages = new List<string>();
            foreach (var item in MainLogic.Chat.Chats[fractionId])
            {
                var user = MainLogic.Chat.Users.First(u => u.Id == item.UserID);
                Messages.Add(item.MessageDatetime+" "+ user.Nickname.Trim() + ": " + item.MessageText);
            }
        }
    }
}