using Hexace.Data.Objects;
using Hexace.Models;
using Microsoft.EntityFrameworkCore;

namespace Hexace.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> users { get; set; }


        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
    }
}
