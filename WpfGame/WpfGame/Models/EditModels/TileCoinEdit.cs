namespace WpfGame.Models.EditModels
{
    public class TileCoinEdit
    {
        public TileEdit TileEdit { get; }
        public CoinEdit CoinEdit { get; }

        public TileCoinEdit(TileEdit tileEdit, CoinEdit coinEdit)
        {
            TileEdit = tileEdit;
            CoinEdit = coinEdit;
        }
    }
}