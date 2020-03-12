using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Models;
using Microsoft.AspNetCore.Mvc;
using Hexace.Data;
using Hexace.Data.Objects;

namespace Hexace.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            StatisticsModel model = new StatisticsModel(FractionStats.Stats);
            return View(model);
        }
    }
}