using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using WpfGame.Generals;
using WpfGame.Models.EditModels;

namespace WpfGame.Tooling
{
    public class JsonPlaygroundWriter
    {
        private List<TileEdit> _tileEdits;
        private int currentPlaygroundsAmount;
        private string writePath;

        public JsonPlaygroundWriter(List<TileEdit> list)
        {
            writePath = AppDomain.CurrentDomain.BaseDirectory + General.PlaygroundPath;

            try
            {
                _tileEdits = ValidateTileEditList(list);

                if (!PlaygroundFolderDoesExist(writePath))
                {
                    CreatePlaygroundsFolder(writePath);
                }

                currentPlaygroundsAmount =
                    currentAmountOfPlaygroundFiles(writePath);

                WriteNewJsonPlayground(_tileEdits);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private List<TileEdit> ValidateTileEditList(List<TileEdit> list)
        {
            if (list == null || !list.Any() || list.Count < 300)
            {
                throw new ValidationException();
            }

            return list;
        }

        private void CreatePlaygroundsFolder(string path) => Directory.CreateDirectory(path);

        private bool PlaygroundFolderDoesExist(string path) => new DirectoryInfo(path).Exists;

        private int currentAmountOfPlaygroundFiles(string path) => new DirectoryInfo(path).GetFiles().Length;

        private void WriteNewJsonPlayground(List<TileEdit> list)
        {
            StringBuilder sb;

            try
            {
                sb = new StringBuilder();
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new JsonWriterException();
            }

            try
            {
                File.WriteAllText(writePath + $"MyPlayground_{currentPlaygroundsAmount}.json", sb.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new IOException();
            }
        }
    }
}