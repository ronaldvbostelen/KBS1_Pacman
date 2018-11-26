using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfGame.Generals;

namespace WpfGame.Controllers.Creatures
{
    public abstract class Sprite
    {
        private const int SpriteSpeed = 3;
        private bool IsAlive;
        public Image Image { get; set; }
        public double Y { get; set; }
        public double X { get; set; }
        public Move CurrentMove { get; set; }
        public Move NextMove { get; set; }

        protected Sprite(double x, double y, bool isAlive)
        {
            X = x;
            Y = y;
            IsAlive = isAlive;
            CurrentMove = NextMove = Move.Stop;
        }
    }
}
