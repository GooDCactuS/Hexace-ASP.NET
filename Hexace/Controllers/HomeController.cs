using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hexace.Models;
using Microsoft.AspNetCore.Authorization;
using System.Web;

namespace Hexace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CellContext db;
        public HomeController(ILogger<HomeController> logger, CellContext context)
        {
            _logger = logger;
            
        }

        [Authorize]
        public IActionResult Index()
        {

            
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
