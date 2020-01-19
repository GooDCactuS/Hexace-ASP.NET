using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hexace.Models
{
    public class ObjectCell
    {
        public ObjectCell(int x, int y, bool isFilled, bool isStroked, string colorAttack=null, string colorDef= null)
        {
            this.x = x;
            this.y = y;
            this.colorDef = colorDef;
            this.colorAttack = colorAttack;
            this.isFilled = isFilled;
            this.isStroked = isStroked;
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
    }
    
    public class HomeModel
    {
        public List<ObjectCell> Cells= new List<ObjectCell>();
        public int Y;
        public int X;
        public int Id;
        public static string GetJsonString(List<ObjectCell> cells)
        {
            return JsonConvert.SerializeObject(cells.ToArray());
        }
    }
}