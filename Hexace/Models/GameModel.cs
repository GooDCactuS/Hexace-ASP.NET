using System.Collections.Generic;
using System.Linq;
using Hexace.Data;
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
        public double LastAttackTime;
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
                var colorDef = cell.IsFilled ? db.Fractions.First(x => x.Id == cell.FractionDefId).Color : null;
                var colorAttack = cell.IsStroked ? db.Fractions.First(x => x.Id == cell.FractionAttackId).Color : null;
                Cells.Add(new ObjectCell(cell.X, cell.Y, cell.IsFilled, cell.IsStroked, colorAttack, colorDef));
            }
        }

        public void SaveChanges(ObjectCell cell)
        {
            var updateCell = db.FieldCells.First(x => x.Y == cell.x && x.Y==cell.y);
            updateCell.FractionDefId = updateCell.FractionAttackId;
            updateCell.FractionAttackId = null;
            updateCell.IsFilled = true;
            updateCell.IsStroked = false;
            db.SaveChanges();
        }


    }
}