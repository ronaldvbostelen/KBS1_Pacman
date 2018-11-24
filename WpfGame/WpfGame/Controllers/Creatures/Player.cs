using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Controllers.Renderer;
using WpfGame.Views;

namespace WpfGame.Controllers
{
    class Player : Sprite
    {
        public Player() 
            : base(true)
        {
            SpriteRenderer.Draw(new Position(50, 50), new Size(18, 18), @"\Assets\Sprites\Pacman\pacman-left-halfopenjaw.png");
        }
    }
}
