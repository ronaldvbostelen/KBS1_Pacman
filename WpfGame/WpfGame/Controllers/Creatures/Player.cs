using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Controllers.Renderer;
using WpfGame.Views;

namespace WpfGame.Controllers
{
    class Player : Sprite
    {
        public Image Image { get; set; }

        public Player() 
            : base(true)
        {
            Image = GetSpriteImage();
        }

        public void Draw()
        {
            Position position = new Position(50, 50);
            Size size = new Size(50, 50);
            Image = GetSpriteImage();

            SpriteRenderer.Draw(Image, position, size, GameView.Canvas);
        }

        public Image GetSpriteImage()
        {
            Image = new Image();

            Image.Source = new BitmapImage(new Uri(@"\Assets\Sprites\Pacman\pacman-left-halfopenjaw.png", UriKind.Relative));

            return Image;
        }
    }
}
