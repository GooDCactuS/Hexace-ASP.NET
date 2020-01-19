using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexace.Data.Objects
{
    public class FieldCell
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsFilled { get; set; }
        public bool IsStroked { get; set; }
        public int? FractionAttackId { get; set; } 
        public int? FractionDefId { get; set; }
    }
}
