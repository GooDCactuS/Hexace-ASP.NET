using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Hexace.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Hexace
{
    public class MainLogic
    {
        public static int SeasonId { get; private set; }
        public static FractionStats FractionStats { get; private set; }
        public static Chat Chat { get; private set; }

        public MainLogic(IServiceProvider services)
        {
            SeasonId = 1;
            FractionStats = new FractionStats(services.GetService<HexaceContext>());
            Chat = new Chat(services.GetService<HexaceContext>());
        }

        public void UpdateChat()
        {
            Chat.UpdateMessages();
        }

        public void UpdateInfo()
        {
            FractionStats.UpdateStats();
            Chat.UpdateMessages();
        }
    }
}
