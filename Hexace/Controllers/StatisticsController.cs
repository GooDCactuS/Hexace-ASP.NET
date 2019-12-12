using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hexace.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            StatisticsModel model = new StatisticsModel();
            return View(model);
        }
    }
}