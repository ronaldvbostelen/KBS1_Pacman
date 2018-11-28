namespace WpfGame.Generals
{
    public interface IMovable
    {
        Move CurrentMove { get; set; }
        Move NextMove { get; set; }
    }
}