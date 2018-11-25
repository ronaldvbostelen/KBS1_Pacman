using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Controllers.Renderer;
using WpfGame.Controllers.Views;
using WpfGame.Views;

namespace WpfGame.Controllers
{
    public class Player : Sprite
    {
        public int Y { get; set; }
        public int X { get; set; }
        public string PlayerImage { get; set; }

        public Player() 
            : base(true)
        {
            X = 55;
            Y = 55;
        }
    }
}
