using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
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

        //public ObjectCell(int x, int y)
        //{
        //    this.x = x;
        //    this.y = y;
        //}
        public int x;
        public int y;
        public string? colorDef;
        public string? colorAttack;
        public bool isFilled;
        public bool isStroked;
    }

    public class HomeModel
    {
        public string UserMessage { get; set; }
        public List<string> Messages { get; set; }

        public List<ObjectCell> Cells { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public int Id { get; set; }
        public string CellString { get; set; }

        public static List<ObjectCell> GetObjectCells(string str)
        {
            return JsonConvert.DeserializeObject<ObjectCell[]>(str).ToList();
        }

        public static string GetJsonString(List<ObjectCell> cells)
        {
            return JsonConvert.SerializeObject(cells);
        }

        public HomeModel()
        {

        }

        public HomeModel(int fractionId, string lastMessage)
        {
            Messages = new List<string>();
            foreach (var item in MainLogic.Chat.Chats[fractionId])
            {
                var user = MainLogic.Chat.Users.First(u => u.Id == item.UserID);
                Messages.Add(item.MessageDatetime + " " + user.Nickname.Trim() + ": " + item.MessageText);
            }

            if (lastMessage != null && Messages.Last() == lastMessage)
                Messages = null;
        }
    }
}