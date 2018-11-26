namespace WpfGame.Generals
{

    public enum NextMove
    {
        Left,
        Right,
        Up,
        Down
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