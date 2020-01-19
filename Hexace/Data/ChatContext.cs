using Hexace.Data.Objects;
using Microsoft.EntityFrameworkCore;

namespace Hexace.Data
{
    public class ChatContext : DbContext
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Fraction> Fractions { get; set; }

        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
