using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Controllers.Views;
using WpfGame.Values;
using WpfGame.Views;

namespace WpfGame.Controllers.Behaviour
{
    class Step
    {

        public void SetStep(Sprite sprite)
        {
            Canvas.SetTop(sprite.Image, sprite.Y);
            Canvas.SetLeft(sprite.Image,sprite.X);
        }
    }

}
