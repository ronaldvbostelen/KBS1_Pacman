namespace WpfGame.Generals
{

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
        Erase
    }


    public static class General
    {
        public const string playgroundPath = @"\Playgrounds\";
    }
}