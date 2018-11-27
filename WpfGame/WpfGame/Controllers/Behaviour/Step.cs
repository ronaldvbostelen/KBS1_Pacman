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
using WpfGame.Models;
using WpfGame.Values;
using WpfGame.Views;

namespace WpfGame.Controllers.Behaviour
{
    class Step
    {

        public void SetStep(MovableObject sprite)
        {
            Canvas.SetTop(sprite.Image, sprite.Y);
            Canvas.SetLeft(sprite.Image,sprite.X);
        }

        /**
         * Removes the old player sprite and adds a new sprite to the
         * canvas that has a position based on the players input
         **/
        //        public static void SetStep(Image playerImage, double _y, double _x)
        //        {
        //            // This gets the player sprite based on an image tag
        //            var playerSprite = (from Image sprite in GameViewController.Canvas.Children.OfType<Image>() // Used OfType to prevent the query from trying to get other objects like Buttons
        //                         where "Player".Equals(sprite.Tag)
        //                         select sprite).First();
        //
        //
        //            //@Karen dit moet wel anders. ik zou het niet static doen. Nu het static is en ik ga met ESC terug en laad een andere playground krijg ik een
        //            // exception omdat canvas nog vast zit. als je het instantieert heb je daar geen last van.
        //            GameViewController.Canvas.Children.Remove(playerSprite);
        //            
        //            playerImage.Tag = "Player"; // This is an ID of some sort. Can be used to find the child on the Canvas.
        //
        //            Canvas.SetTop(playerImage, _y);
        //            Canvas.SetLeft(playerImage, _x);
        //            GameViewController.Canvas.Children.Add(playerImage);
    }

}
