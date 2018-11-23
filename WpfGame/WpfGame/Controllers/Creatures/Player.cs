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
        private Image image;

        public Player() 
            : base(true)
        {
            
        }

        public void Draw()
        {
            Position position = new Position(50, 50);
            Size size = new Size(50, 50);
            image = GetSpriteImage();

                // dit compileerde niet meer
//            SpriteRenderer.Draw(image, position, size, GameView.canvas);

        }

        public Image GetSpriteImage()
        {
            image = new Image();

            image.Source = new BitmapImage(new Uri(@"\Assets\Sprites\Pacman\pacman-left-halfopenjaw.png", UriKind.Relative));

            return image;
        }
    }
}
