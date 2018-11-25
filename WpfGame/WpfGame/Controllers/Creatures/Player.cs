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
    class Player : ViewController
    {
        private int _y, _x;

        public Player(MainWindow mainWindow) 
            : base(mainWindow)
        {
            _x = 55;
            _y = 55;
            SpriteRenderer.Draw(new Position(_x, _y), new Behaviour.Size(20, 20), @"\Assets\Sprites\Pacman\pacman-left-halfopenjaw.png");
        }

        public void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    _y += 25;
                    Step.SetStep(SpriteRenderer.GetSpriteImage(@"\Assets\Sprites\Pacman\pacman-down-halfopenjaw.png"), _y, _x);
                    break;
                case Key.Up:
                    _y -= 25;
                    Step.SetStep(SpriteRenderer.GetSpriteImage(@"\Assets\Sprites\Pacman\pacman-up-halfopenjaw.png"), _y, _x);
                    break;
                case Key.Left:
                    _x -= 25;
                    Step.SetStep(SpriteRenderer.GetSpriteImage(@"\Assets\Sprites\Pacman\pacman-left-halfopenjaw.png"), _y, _x);
                    break;
                case Key.Right:
                    _x += 25;
                    Step.SetStep(SpriteRenderer.GetSpriteImage(@"\Assets\Sprites\Pacman\pacman-right-halfopenjaw.png"), _y, _x);      
                    break;
            }
        }
    }
}
