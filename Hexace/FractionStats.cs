using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Data;
using Hexace.Data.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hexace
{
    public class FractionStats
    {
        private HexaceContext db;

        public static Dictionary<string, FractionScore> FractionInfo { get; set; }

        public static Dictionary<string, int> Stats =>
            new Dictionary<string, int>
            {
                {"Turquoise", FractionInfo["Turquoise"].Score}, {"Burgundy", FractionInfo["Burgundy"].Score},
                {"Purple", FractionInfo["Purple"].Score}
            };

        public FractionStats(HexaceContext context)
        {
            db = context;

            FractionInfo = new Dictionary<string, FractionScore>();

            Dictionary<string, int> fractions = new Dictionary<string, int>()
                {{"Turquoise", 1}, {"Burgundy", 2}, {"Purple", 3}};
            DateTime maxDate = new DateTime(1);


            foreach (var fractionName in fractions)
            {
                FractionInfo.Add(fractionName.Key, null);
                FractionScore score = null;
                foreach (var item in db.FractionScores)
                {
                    if (item.FractionId == fractionName.Value && item.ScoreDatetime >= maxDate)
                    {
                        maxDate = item.ScoreDatetime;
                        score = item;
                    }

                }

                FractionInfo[fractionName.Key] = score;
            }
            
        }

        public async void UpdateStats()
        {
            var scope = Program.host.Services.CreateScope();
            db = scope.ServiceProvider.GetService<HexaceContext>();
            foreach (var item in FractionInfo.Values)
            {
                db.Database.OpenConnection();
                item.ScoreDatetime = DateTime.Now;
                db.FractionScores.Add(new FractionScore
                {
                    AttackAttempts = item.AttackAttempts,
                    DefenseAttempts = item.DefenseAttempts, 
                    FractionId = item.FractionId,
                    HexesUnderControl = item.HexesUnderControl,
                    Score = item.Score,
                    SeasonId = item.SeasonId,
                    SuccessfulAttacks = item.SuccessfulAttacks,
                    SuccessfulDefences = item.SuccessfulDefences,
                    ScoreDatetime = DateTime.Now
                });
            }
            await db.SaveChangesAsync();
        }
    }
}
