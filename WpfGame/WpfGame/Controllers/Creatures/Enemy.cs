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
        Position position;

        public Enemy(int x, int y)
            : base(true)
        {
            this.x = x;
            this.y = y;
            position = new Position(x, y);
        }

        public void Draw()
        {
            //Position position = new Position(100, 100);
            Size size = new Size(50, 50);
            image = GetSpriteImage();

            SpriteRenderer.Draw(image, position, size, MainWindow.canvas);
        }

        public Image GetSpriteImage()
        {
            image = new Image();

            image.Source = new BitmapImage(new Uri(@"\Assets\Sprites\Enemy\blinky-right-2.png", UriKind.Relative));

            return image;
        }
    }
}
