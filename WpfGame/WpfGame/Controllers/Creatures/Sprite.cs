using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGame.Controllers.Creatures
{
    class Sprite
    {
        private const int SpriteSpeed = 3;
        private bool IsAlive;

        public Sprite(bool isAlive)
        {
            IsAlive = isAlive;
        }
    }
}
