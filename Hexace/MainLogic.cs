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

        public void UpdateCells()
        {
            foreach (var cell in GameModel.Cells)
            {
                if(cell.isStroked&& cell.LastAttackTime - Timer.GetTimeNow() > 1000 * 60)
                {
                    cell.isFilled = true;
                    cell.isStroked = false;
                    cell.colorDef = cell.colorAttack;
                    cell.colorAttack = "";
                    GameModel.SaveChanges(cell);
                }
            }
        }

        public static void UpdateChat()
        {
            Chat.UpdateMessages();
        }


        public static void UpdateTimerForUser(int userId)
        {
            Timer.UpdateClickUser(userId);
        }
        
        public static void UpdateInfo()
        {
            FractionStats.UpdateStats();
            //Chat.UpdateMessages();
        }
    }
}
