using System;
using System.Media;
using System.Windows;
using WpfGame.Generals;
using WpfGame.Properties;

namespace WpfGame.Sounds
{
    public class Sound
    {
        private readonly SoundPlayer _soundPlayer;

        public Sound()
        {
            _soundPlayer = new SoundPlayer();
        }

        public void BtnStartGameOnClick(object sender, RoutedEventArgs e)
        {
            _soundPlayer.Stream = Resources.pacman_beginning;
            _soundPlayer.Play();
        }

        public void OnObstacleCollision(object sender, EventArgs e)
        {
            _soundPlayer.Stream = Resources.pacman_death;
            _soundPlayer.Play();
        }

        public void OnOnEnemyCollision(object sender, EventArgs e)
        {
            _soundPlayer.Stream = Resources.pacman_death;
            _soundPlayer.Play();
        }

        public void OnPlaytimeIsOver(object sender, EventArgs e)
        {
            _soundPlayer.Stream = Resources.pacman_death;
            _soundPlayer.Play();
        }

        public void OnCoinCollision(object sender, ImmovableEventArgs args)
        {
            _soundPlayer.Stream = Resources.pacman_chomp;
            _soundPlayer.Play();
        }

        public void OnEndpointCollision(object sender, EventArgs e)
        {
            _soundPlayer.Stream = Resources.pacman_extrapac;
            _soundPlayer.Play();
        }
    }
}