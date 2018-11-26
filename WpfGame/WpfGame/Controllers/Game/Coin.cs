using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;
using WpfGame.Views;

namespace WpfGame.Controllers.Objects
{
    class Coin
    {
        public Coin()
        {
            SpriteRenderer.Draw(100,100, new Size(18, 18), new Image{Source  = new BitmapImage(new Uri(@"\Assets\Sprites\Objects\coin.png"))});
        }
    }
}
