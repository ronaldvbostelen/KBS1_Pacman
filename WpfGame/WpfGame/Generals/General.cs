using System;

namespace WpfGame.Generals
{

    public delegate void PlaytimeIsOVerEventHandeler(object sender, EventArgs e);

    public enum ObjectType
    {
        Coin,
        Obstacle,
        Wall,
        Path,
        Enemy,
        Player,
        SpawnPoint,
        StartPoint,
        EndPoint
    }

    public enum Move
    {
        Stop,
        Left,
        Right,
        Up,
        Down
    }

    public enum PacmanFacing
    {
        Left,
        LeftOpen,
        Right,
        RightOpen,
        Up,
        UpOpen,
        Down,
        DownOpen
    }

    public enum SelectedItem
    {
        None,
        Wall,
        Coin,
        Obstacle,
        Start,
        End,
        Erase,
        Spawn
    }

    public enum NextStep
    {
        Clear,
        Wall,
        Border,
        Coin,
        Obstacle,
        Endpoint,
        Player,
        Enemy
    }

    public static class General
    {
        public const string playgroundPath = @"\Playgrounds\";
    }
}