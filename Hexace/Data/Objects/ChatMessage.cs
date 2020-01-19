using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexace.Data.Objects
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDatetime { get; set; }
        public int FractionId { get; set; }

        public ChatMessage() { }

        public ChatMessage(int userId, string messageText, DateTime messageDatetime, int fractionId)
        {
            UserID = userId;
            MessageText = messageText;
            MessageDatetime = messageDatetime;
            FractionId = fractionId;
        }
    }
}
