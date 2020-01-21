using System;
using System.Collections.Generic;
using System.Linq;
using Hexace.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Hexace.Models
{
    public class ObjectCell
    {
        public ObjectCell(int x, int y, bool isFilled, bool isStroked, string colorAttack = null, string colorDef = null)
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
        public long LastAttackTime;

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ObjectCell oc = (ObjectCell)obj;
                return (x == oc.x) && (y == oc.y) && (colorDef == oc.colorDef) && (colorAttack == oc.colorAttack) &&
                       (isFilled == oc.isFilled) && (isStroked == oc.isStroked) &&
                       (LastAttackTime == oc.LastAttackTime);
            }
        }
    }

    public class GameModel
    {
        private HexaceContext db;
        public List<ObjectCell> Cells { get; set; }
        public string CellString { get; set; }

        public GameModel(HexaceContext context)
        {
            db = context;
            Cells = new List<ObjectCell>();
            foreach (var cell in db.FieldCells.ToList())
            {
                string colorDef;
                if (cell.FractionDefId == 4)
                {
                    colorDef = null;
                }
                else
                {
                    colorDef = cell.IsFilled ? db.Fractions.First(x => x.Id == cell.FractionDefId).Color : null;
                }
                cell.IsStroked = false;
                cell.FractionAttackId = null;
                Cells.Add(new ObjectCell(cell.X, cell.Y, cell.IsFilled, cell.IsStroked, null, colorDef));
            }
        }

        public void SaveChanges()
        {
            Dictionary<int, string> colors = new Dictionary<int, string>();
            
            var scope = Program.host.Services.CreateScope();
            db = scope.ServiceProvider.GetService<HexaceContext>();
            db.Database.OpenConnection();

            foreach (var item in db.Fractions)
            {
                colors.Add(item.Id, item.Color);
            }
            foreach (var cell in Cells)
            {
                var updateCell = db.FieldCells.First(c => c.X==cell.x && c.Y == cell.y);
                if (cell.colorDef == null)
                {
                    updateCell.FractionDefId = null;
                }
                else
                {
                    updateCell.FractionDefId = colors.First(c => c.Value == cell.colorDef).Key;
                }
                
                updateCell.FractionAttackId = null;
                updateCell.IsFilled = cell.isFilled;
                updateCell.IsStroked = false;
            }
            db.SaveChanges();
        }


    }
}