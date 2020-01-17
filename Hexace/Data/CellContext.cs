using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Data.Objects;
using Microsoft.EntityFrameworkCore;

namespace Hexace.Data
{
    public class CellContext:DbContext
    {
        public DbSet<Cell> Cells { get; set; }

        public CellContext(DbContextOptions<CellContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
