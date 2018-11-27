using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using WpfGame.Models;
using WpfGame.Generals;

namespace WpfGame.Editor
{
    public class JsonPlaygroundWriter
    {
        private JsonTextWriter _jsonTextWriter;
        private List<TileEdit> _tileEdits;
        private int currentPlaygroundsAmount;
        
        public JsonPlaygroundWriter(List<TileEdit> list)
        {
            //nb foutafhandeling is r-u-k//

            _tileEdits = list;
            currentPlaygroundsAmount =
                currentAmountOfPlaygroundFiles(System.AppDomain.CurrentDomain.BaseDirectory + General.playgroundPath);

            WriteNewJsonPlayground(_tileEdits);

        }

        private int currentAmountOfPlaygroundFiles(string path)
        {
            return new DirectoryInfo(path).GetFiles().Length;
        }

        private void WriteNewJsonPlayground(List<TileEdit> list)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartObject();
            writer.WritePropertyName("Tiles");
            writer.WriteStartObject();

            for (int i = 0; i < list.Count; i++)
            {
                writer.WritePropertyName("Tile");
                writer.WriteStartObject();
                writer.WritePropertyName("isWall");
                writer.WriteValue(list[i].IsWall ? 1 : 0);
                writer.WritePropertyName("hasCoin");
                writer.WriteValue(list[i].HasCoin ? 1 : 0);
                writer.WritePropertyName("hasObstacle");
                writer.WriteValue(list[i].HasObstacle ? 1 : 0);
                writer.WritePropertyName("isStart");
                writer.WriteValue(list[i].IsStart ? 1 : 0);
                writer.WritePropertyName("isEnd");
                writer.WriteValue(list[i].IsEnd ? 1 : 0);
                writer.WritePropertyName("isSpawn");
                writer.WriteValue(list[i].IsSpawn ? 1 : 0);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
            
            System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + General.playgroundPath + $"MyPlayground_{currentPlaygroundsAmount}.json", sb.ToString());
        }
    }
}
