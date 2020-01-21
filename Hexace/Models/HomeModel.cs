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
    public class HomeModel
    {
        public string UserMessage { get; set; }
        public List<string> Messages { get; set; }

        public string CellString { get; set; }
        public double LastClick { get; set; }
        public int X { get; set; }
        public int Y { get; set; }


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