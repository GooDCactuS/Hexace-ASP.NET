using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexace.Data.Objects
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public int AchievementId { get; set; }
        public int UserId { get; set; }
        public DateTime AchievementGotDatetime { get; set; }
    }
}
