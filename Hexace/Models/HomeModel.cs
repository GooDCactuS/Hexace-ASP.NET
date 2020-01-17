using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hexace.Models
{
    public class ObjectCell
    {
        public int X;
        public int Y;
        public string ColorDef;
        public bool IsFilled;
        public bool IsStroked;
    }
    public class HomeModel
    {
        public static List<ObjectCell> Cells;
    }
}