using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Data.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Hexace.Models
{
    public class LeaderboardModel 
    {
        public class UserInfo
        {
            public Profile ProfileInfo { get; set; }
            public string Nickname { get; set; }
        }
        public List<UserInfo> Users { get; set; }

        public string ButtonValue { get; set; }

        public LeaderboardModel() { Users = new List<UserInfo>();}

        public LeaderboardModel(List<UserInfo> users)
        {
            Users = users;
        }

    }
}