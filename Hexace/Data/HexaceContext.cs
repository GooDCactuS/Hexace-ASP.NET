using Hexace.Data.Objects;
using Microsoft.EntityFrameworkCore;

namespace Hexace.Data
{
    public class HexaceContext : DbContext
    {
        public DbSet<Fraction> Fractions { get; set; }
        public DbSet<FieldCell> FieldCells { get; set; }        
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UsersAchievements { get; set; }
        public DbSet<FractionScore> FractionScores { get; set; }

        public HexaceContext(DbContextOptions<HexaceContext> options) : base(options)
        {
        }
    }
}