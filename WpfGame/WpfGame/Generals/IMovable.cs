namespace WpfGame.Generals
{
    public interface IMovable : IPlaygroundObject
    {
        Move CurrentMove { get; set; }
        Move NextMove { get; set; }
    }
}