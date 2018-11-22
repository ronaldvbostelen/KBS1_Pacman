using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;
using WpfGame.Views;

namespace WpfGame.Controllers.Objects
{
    class Coin
    {
        public Coin()
        {
            SpriteRenderer.Draw(new Position(100, 100), new Size(18, 18), @"\Assets\Sprites\Objects\coin.png");
        }
    }
}
