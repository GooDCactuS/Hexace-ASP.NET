using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexace.Data.Objects
{
    public class Profile
    {
        public int Id { get; set; }
        public int FractionId { get; set; }
        public int SeasonId { get; set; }
        public int AttackAttempts { get; set; }
        public int SuccessfulAttacks { get; set; }
        public int DefenseAttempts { get; set; }
        public int SuccessfulDefences { get; set; }
        public int UserId { get; set; }
    }
}
