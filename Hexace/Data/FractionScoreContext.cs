using Hexace.Data.Objects;
using Microsoft.EntityFrameworkCore;

namespace Hexace.Data
{
    public class FractionScoreContext : DbContext
    {
        public DbSet<FractionScore> FractionScores { get; set; }

        public FractionScoreContext(DbContextOptions<FractionScoreContext> options) : base(options)
        {
            Database.EnsureCreated(); 
        }
    }
}
