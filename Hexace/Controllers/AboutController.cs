using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexace.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hexace.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            AboutModel model = new AboutModel();
            return View(model);
        }
    }
}