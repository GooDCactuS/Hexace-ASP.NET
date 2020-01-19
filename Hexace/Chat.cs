using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Data;
using Hexace.Data.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hexace
{
    public class Chat
    {
        private ChatContext db;
        public Dictionary<int, List<ChatMessage>> Chats { get; set; }

        public Chat(ChatContext context)
        {
            db = context;
            
            Chats = new Dictionary<int, List<ChatMessage>>();
            for (int i = 1; i < 4; i++)
            {
                var fracChat = db.ChatMessages.Where(x => x.FractionId == i); 
                var messages = new List<ChatMessage>();
                foreach (var item in fracChat)
                {
                    messages.Add(item);
                }
                Chats.Add(i, messages);
            }

        }
        public async void UpdateMessages()
        {
            var scope = Program.host.Services.CreateScope();
            db = scope.ServiceProvider.GetService<ChatContext>();
            //db.Database.OpenConnection();
            for (int i = 1; i < 4; i++)
            {
                foreach (var message in Chats[i])
                {
                    if (db.ChatMessages.First(x => x.Id == message.Id) == null)
                    {
                        db.ChatMessages.Add(message);
                    }
                }
            }

            await db.SaveChangesAsync();
        }
    }
}
