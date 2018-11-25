using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;


namespace WpfGame.Controllers.Creatures
{
    class Enemy : Sprite
    {
        private Image image;
        int x, y;

        public Enemy(int x, int y)
            : base(true)
        {
            this.x = x;
            this.y = y;
        }

        public void Draw()
        {
            image = SpriteRenderer.GetSpriteImage(@"\Assets\Sprites\Enemy\blinky-right-2.png");
            SpriteRenderer.Draw(image, new Position(x,y), new Size(50,50), MainWindow.canvas);
        }

    }
}
