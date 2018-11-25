using System.Collections.Generic;
using WpfGame.Models;

namespace WpfGame.Controllers.Renderer
{
    public class TileRenderer
    {
        private List<TileMockup> tileMockups;
        private List<Tile> tiles;
        private double tileWidth, tileHeigth;
        private int amountofXtiles;
        private double amountofYTiles;

        public TileRenderer(List<TileMockup> list, double tileWidth, double tileHeigth, int amountofXTiles, double amountofYTiles)
        {
            tileMockups = list;
            tiles = new List<Tile>();

            this.tileWidth = tileWidth;
            this.tileHeigth = tileHeigth;
            this.amountofXtiles = amountofXTiles;
            this.amountofYTiles = amountofYTiles;

            RenderTiles(tiles, tileMockups);

        }

        private void RenderTiles(List<Tile> outList, List<TileMockup> inList)
        {
            int counter = 0;

            for (int i = 0; i < amountofYTiles; i++)
            {
                for (int j = 0; j < amountofXtiles; j++)
                {
                    var currMockup = inList[counter];

                    var tile = new Tile(tileWidth, tileHeigth, i * tileHeigth, j * tileWidth,
                        currMockup.IsWall, currMockup.HasCoin, currMockup.HasObstacle, currMockup.IsStart,
                        currMockup.IsEnd);
                    tiles.Add(tile);

                    counter++;
                }
            }
        }


        public List<Tile> GetRenderdTiles() => tiles;
    }
}