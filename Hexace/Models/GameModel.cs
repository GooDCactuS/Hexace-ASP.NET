using System;
using System.Collections.Generic;
using System.Linq;
using Hexace.Controllers;
using Hexace.Data;
using Hexace.Data.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Hexace.Models
{
    public class ObjectCell
    {
        public ObjectCell(int x, int y, bool isFilled, bool isStroked, string colorAttack = null, string colorDef = null)
        {
            this.x = x;
            this.y = y;
            this.colorDef = colorDef;
            this.colorAttack = colorAttack;
            this.isFilled = isFilled;
            this.isStroked = isStroked;
            this.PlayerId = 0;
        }

        //public ObjectCell(int x, int y)
        //{
        //    this.x = x;
        //    this.y = y;
        //}

        public int x;
        public int y;
        public string? colorDef;
        public string? colorAttack;
        public bool isFilled;
        public bool isStroked;
        public long LastAttackTime;
        public int PlayerId;
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ObjectCell oc = (ObjectCell)obj;
                return (x == oc.x) && (y == oc.y) && (colorDef == oc.colorDef) && (colorAttack == oc.colorAttack) &&
                       (isFilled == oc.isFilled) && (isStroked == oc.isStroked) &&
                       (LastAttackTime == oc.LastAttackTime);
            }
        }
    }

    public class GameModel
    {
        public enum Stats
        {
            SuccessfulAttacks,
            SuccessfulDefends,
            AttackAttempts
        }

        private static HexaceContext db;
        public List<ObjectCell> Cells { get; set; }

        public GameModel(HexaceContext context)
        {
            db = context;
            Cells = new List<ObjectCell>();
            foreach (var cell in db.FieldCells.ToList())
            {
                string colorDef;
                if (cell.FractionDefId == 4)
                {
                    colorDef = null;
                }
                else
                {
                    colorDef = cell.IsFilled ? db.Fractions.First(x => x.Id == cell.FractionDefId).Color : null;
                }
                cell.IsStroked = false;
                cell.FractionAttackId = null;
                Cells.Add(new ObjectCell(cell.X, cell.Y, cell.IsFilled, cell.IsStroked, null, colorDef));
            }
        }

        public void SaveChanges()
        {
            Dictionary<int, string> colors = new Dictionary<int, string>();

            var scope = Program.host.Services.CreateScope();
            db = scope.ServiceProvider.GetService<HexaceContext>();
            db.Database.OpenConnection();

            foreach (var item in db.Fractions)
            {
                colors.Add(item.Id, item.Color);
            }
            foreach (var cell in Cells)
            {
                var updateCell = db.FieldCells.First(c => c.X == cell.x && c.Y == cell.y);
                if (cell.colorDef == null)
                {
                    updateCell.FractionDefId = null;
                }
                else
                {
                    updateCell.FractionDefId = colors.First(c => c.Value == cell.colorDef).Key;
                }

                updateCell.FractionAttackId = null;
                updateCell.IsFilled = cell.isFilled;
                updateCell.IsStroked = false;
            }
            db.SaveChanges();
        }

        public static void IncreasePlayersStats(int userId, Stats stat, int oldFractionId = 0)
        {
            var scope = Program.host.Services.CreateScope();
            var curDbContext = scope.ServiceProvider.GetService<HexaceContext>();

            var profile = curDbContext.Profiles.First(p => p.UserId == userId);
            var fractionName = curDbContext.Fractions.First(f => f.Id == profile.FractionId).Name.TrimEnd();
            switch (stat)
            {
                case Stats.SuccessfulAttacks:
                    profile.SuccessfulAttacks++;
                    FractionStats.FractionInfo[fractionName].SuccessfulAttacks++;
                    FractionStats.FractionInfo[fractionName].HexesUnderControl++;
                    FractionStats.FractionInfo[fractionName].Score++;
                    CheckForAchievements(userId, curDbContext);
                    if (oldFractionId > 0)
                    {
                        curDbContext.FractionScores.First(f => f.Id == oldFractionId).HexesUnderControl--;
                    }
                    break;
                case Stats.SuccessfulDefends:
                    profile.SuccessfulDefences++;
                    FractionStats.FractionInfo[fractionName].Score++;
                    FractionStats.FractionInfo[fractionName].SuccessfulDefences++;
                    CheckForAchievements(userId, curDbContext);
                    break;
                case Stats.AttackAttempts:
                    profile.AttackAttempts++;
                    FractionStats.FractionInfo[fractionName].AttackAttempts++;
                    CheckForAchievements(userId, curDbContext);
                    break;
            }

            curDbContext.SaveChangesAsync();

        }

        public static void CheckForAchievements(int userId, HexaceContext context)
        {
            var userAchievements = context.UsersAchievements.Where(ua => ua.UserId == userId);
            var lockedAchievements = new List<Achievement>();

            foreach (var item in context.Achievements.ToList())
            {
                var isLocked = true;
                foreach (var userAchievement in userAchievements)
                {
                    if (item.Id == userAchievement.Id)
                    {
                        isLocked = false;
                    }
                }

                if (isLocked)
                {
                    lockedAchievements.Add(item);
                }

            }

            foreach (var item in lockedAchievements)
            {
                switch (item.AchievementName.TrimEnd())
                {
                    case "Attacker I":
                        if (context.Profiles.First(p => p.UserId == userId).SuccessfulAttacks >= 5)
                        {
                            context.UsersAchievements.Add(new UserAchievement
                            {
                                AchievementGotDatetime = DateTime.Now, AchievementId = item.Id, UserId = userId
                            });
                        }
                        break;

                    case "Attacker II":
                        if (context.Profiles.First(p => p.UserId == userId).SuccessfulAttacks >= 50)
                        {
                            context.UsersAchievements.Add(new UserAchievement
                            {
                                AchievementGotDatetime = DateTime.Now,
                                AchievementId = item.Id,
                                UserId = userId
                            });
                        }
                        break;

                    case "Attacker III":
                        if (context.Profiles.First(p => p.UserId == userId).SuccessfulAttacks >= 100)
                        {
                            context.UsersAchievements.Add(new UserAchievement
                            {
                                AchievementGotDatetime = DateTime.Now,
                                AchievementId = item.Id,
                                UserId = userId
                            });
                        }
                        break;

                    case "Defender I":
                        if (context.Profiles.First(p => p.UserId == userId).SuccessfulDefences >= 10)
                        {
                            context.UsersAchievements.Add(new UserAchievement
                            {
                                AchievementGotDatetime = DateTime.Now,
                                AchievementId = item.Id,
                                UserId = userId
                            });
                        }
                        break;

                    case "Defender II":
                        if (context.Profiles.First(p => p.UserId == userId).SuccessfulDefences >= 100)
                        {
                            context.UsersAchievements.Add(new UserAchievement
                            {
                                AchievementGotDatetime = DateTime.Now,
                                AchievementId = item.Id,
                                UserId = userId
                            });
                        }
                        break;

                    case "Defender III":
                        if (context.Profiles.First(p => p.UserId == userId).SuccessfulDefences >= 1000)
                        {
                            context.UsersAchievements.Add(new UserAchievement
                            {
                                AchievementGotDatetime = DateTime.Now,
                                AchievementId = item.Id,
                                UserId = userId
                            });
                        }
                        break;

                    case "Long-liver":
                        if (DateTime.Now.Subtract(context.Users.First(p => p.Id == userId).RegistrationDate).TotalDays >
                            365)
                            context.UsersAchievements.Add(new UserAchievement
                            {
                                AchievementGotDatetime = DateTime.Now,
                                AchievementId = item.Id,
                                UserId = userId
                            });
                        break;
                }
            }

            context.SaveChangesAsync();
        }


    }
}