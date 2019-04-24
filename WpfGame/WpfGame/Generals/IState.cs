namespace WpfGame.Generals
{
    public interface IState : IPlaygroundObject
    {
        bool State { get; set; }
    }
}