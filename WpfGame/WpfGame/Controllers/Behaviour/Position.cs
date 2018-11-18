using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGame.Controllers.Behaviour
{
    class Position
    {
        public int Left { get; set; }
        public int Top { get; set; }

        public Position(int left, int top)
        {
            Left = left;
            Top = top;
        }
    }
}
