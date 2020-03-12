using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;

namespace Hexace.Data.Objects
{
    public class Cell
    {
        public int Id;
        public int X;
        public int Y;
        public int DefFractionId;
        public int AttackingFractionId;
        public bool IsFilled;
        public bool IsStroked;

    }
    //ColorTranslator.FromHtml("#F08080");
}