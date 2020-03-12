using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Hexace.Controllers;
using Hexace.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Hexace
{
    public class MainLogic
    {
        public static int SeasonId { get; private set; }
        public static FractionStats FractionStats { get; private set; }
        public static Chat Chat { get; private set; }
        public static Timer Timer { get; private set; }
        public static GameModel GameModel { get; private set; }

        public MainLogic(IServiceProvider services)
        {
            SeasonId = 1;
            FractionStats = new FractionStats(services.GetService<HexaceContext>());
            Chat = new Chat(services.GetService<HexaceContext>());
            Timer = new Timer(services.GetService<HexaceContext>());
            GameModel = new GameModel(services.GetService<HexaceContext>());
        }

        

        public static void UpdateCells()
        {
            foreach (var cell in GameModel.Cells.Where(x => x.isStroked))
            {
                if ((long)Timer.GetTimeNow() - cell.LastAttackTime > 1000 * 60)
                {
                    cell.isFilled = true;
                    cell.isStroked = false;
                    cell.colorDef = cell.colorAttack;
                    cell.LastAttackTime = 0;
                    cell.colorAttack = "";

                    GameModel.IncreasePlayersStats(cell.PlayerId, GameModel.Stats.SuccessfulAttacks);
                    cell.PlayerId = 0;
                }
            }
        }

        public static void UpdateCellsInDb()
        {
                GameModel.SaveChanges();
        }

        public static void UpdateChat()
        {
            lock (new object())
            {
                Chat.UpdateMessages();
            }
        }


        public static void UpdateTimerForUser(int userId)
        {
            lock (new object())
            {
                Timer.UpdateClickUser(userId);
            }
        }

        public static void UpdateInfo()
        {
            lock (new object())
            {
                FractionStats.UpdateStats();
            }

        }
    }
}
