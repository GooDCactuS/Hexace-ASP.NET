using System;
using System.Collections.Generic;
using System.Linq;
using Hexace.Data;
using Hexace.Data.Objects;

namespace Hexace.Models
{
    public class Timer
    {
        private HexaceContext db;
        public List<User> Users { get; set; }
        public Dictionary<int, double> UsersLastClicks { get; set; }

        public Timer(HexaceContext context)
        {
            db = context;

            UsersLastClicks = new Dictionary<int, double>();
            Users = new List<User>(db.Users.ToList());
            foreach (var user in Users)
            {
                UsersLastClicks.Add(user.Id, Math.Floor(DateTime.UtcNow
                                            .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                                            .TotalMilliseconds));
            }

        }

        public void UpdateClickUser(int userId)
        {
            if (UsersLastClicks.ContainsKey(userId))
                UsersLastClicks[userId] = GetTimeNow() + 1000 * 2 * 60;
            else
            {
                UsersLastClicks.Add(userId, GetTimeNow() + 1000 * 2 * 60);
            }

        }

        public static double GetTimeNow()
        {
            return Math.Floor(DateTime.UtcNow
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds);
        }
    }
}