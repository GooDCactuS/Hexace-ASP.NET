using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexace.Data.Objects
{
    public class FractionScore
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int FractionId { get; set; }
        public int Score { get; set; }
        public int HexesUnderControl { get; set; }
        public int AttackAttempts { get; set; }
        public int SuccessfulAttacks { get; set; }
        public int DefenseAttempts { get; set; }
        public int SuccessfulDefences { get; set; }
        public DateTime ScoreDatetime { get; set; } 

        public FractionScore(){}

        public FractionScore(FractionScore obj)
        {
            Id = obj.Id;
            SeasonId = obj.SeasonId;
            FractionId = obj.FractionId;
            Score = obj.Score;
            HexesUnderControl = obj.HexesUnderControl;
            AttackAttempts = obj.AttackAttempts;
            SuccessfulAttacks = obj.SuccessfulAttacks;
            DefenseAttempts = obj.DefenseAttempts;
            SuccessfulDefences = obj.SuccessfulDefences;
            ScoreDatetime = obj.ScoreDatetime;
        }
    }
}
