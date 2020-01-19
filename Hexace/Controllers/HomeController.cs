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
using Hexace.Data.Objects;

namespace Hexace.Controllers
{
    public class HomeController : Controller
    {
        private CellContext db;
        public HomeController(CellContext context)
        {
            db = context;
        }
        [HttpPost]
        [Authorize]
        public IActionResult In
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var fractions = db.Fractions.ToList();
            //var side = 10;
            //var width = 30;
            //var start = side - 1;
            //var count = 0;
            ////отступ в зависимости от количества ячеек
            //var indent = 0;
            //for (var j = 0; j < side * 2 - 1; j++)
            //{
            //    if (j < side)
            //    {
            //        start++;
            //        if (start % 2 == 0)
            //            indent++;
            //    }
            //    else
            //    {
            //        start--;
            //        if (start % 2 == 1)
            //            indent--;
            //    }

            //    for (var i = width / 2 - start + indent; i < width / 2 + indent; i++)
            //    {
            //        var obj = (x: i, y: j, color: fractions[0].Color, isFill: false);
            //        if (!db.FieldCells.Any(x => x.X == i && x.Y == j))
            //            db.FieldCells.Add(new FieldCell
            //            {
            //                Id = ++count,
            //                X = i,
            //                Y = j,
            //                IsFilled = false,
            //                IsStroked = false
            //            });
            //        db.SaveChanges();
            //    }
            //}

            HomeModel.Cells.Clear();
            foreach (var cell in db.FieldCells.ToList())
            {
                var colorDef= cell.IsFilled?db.Fractions.First(x=>x.Id==cell.FractionDefId).Color:null;
                var colorAttack = cell.IsStroked ? db.Fractions.First(x => x.Id == cell.FractionAttackId).Color : null;
                HomeModel.Cells.Add(new ObjectCell(cell.X, cell.Y, cell.IsFilled, cell.IsStroked, colorAttack, colorDef));
            }
            return View();
        }

    }
}
