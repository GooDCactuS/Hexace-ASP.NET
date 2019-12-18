using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Hexace.Data.Objects
{
    public class User
    {
        public string nickname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime datetime { get; set; }
        public int user_type_id { get; set; }
        public DateTime last_signin { get; set; }
        [Key]
        public int user_id { get; set; }
    }
}
