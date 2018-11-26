namespace WpfGame.Models
{
    public class TileCoinEdit
    {
        public TileEdit TileEdit { get; set; }
        public CoinEdit CoinEdit { get; set; }

        public TileCoinEdit(TileEdit tileEdit, CoinEdit coinEdit)
        {
            TileEdit = tileEdit;
            CoinEdit = coinEdit;
        }
    }
}