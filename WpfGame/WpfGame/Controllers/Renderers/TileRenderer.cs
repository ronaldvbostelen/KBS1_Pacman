using System.Collections.Generic;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Renderer
{
    public class TileRenderer
    {
        private List<TileMockup> _tileMockups;
        private List<Tile> tiles;
        private GameValues _gameValues;

        public TileRenderer(List<TileMockup> list, GameValues gameValues)
        {
            _tileMockups = list;
            _gameValues = gameValues;
            tiles = new List<Tile>();

            RenderTiles(tiles, _tileMockups);
        }

        private void RenderTiles(List<Tile> outList, List<TileMockup> inList)
        {
            int counter = 0;

            for (int i = 0; i < _gameValues.AmountofYtiles; i++)
            {
                for (int j = 0; j < _gameValues.AmountOfXtiles; j++)
                {
                    var currMockup = inList[counter];

                    var tile = new Tile(_gameValues.TileWidth, _gameValues.TileHeight, i * _gameValues.TileHeight, j * _gameValues.TileWidth,
                        currMockup.IsWall, currMockup.HasCoin, currMockup.HasObstacle, currMockup.IsStart,
                        currMockup.IsEnd);
                    outList.Add(tile);

                    counter++;
                }
            }
        }


        public List<Tile> GetRenderdTiles() => tiles;
    }
}