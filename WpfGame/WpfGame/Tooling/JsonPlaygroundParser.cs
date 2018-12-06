using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WpfGame.Generals;
using WpfGame.Models;

namespace WpfGame.Tooling
{
    public class JsonPlaygroundParser
    {
        private JsonTextReader _jsonTextReader;
        private StreamReader _streamReader;
        private List<TileMockup> _tileMockups;
        private string playgroundJsonPath;


        public JsonPlaygroundParser(string fileName)
        {
            _tileMockups = new List<TileMockup>();

            try
            {
                playgroundJsonPath = SetFilePath(AppDomain.CurrentDomain.BaseDirectory, fileName);

                SetStreamReader(playgroundJsonPath);

                _jsonTextReader = new JsonTextReader(_streamReader);

                ReadJson(_jsonTextReader, _tileMockups);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ReadJson(JsonTextReader reader, List<TileMockup> list)
        {
            bool tileTag = false;
            bool wall = false;
            bool coin = false;
            bool obstacle = false;
            bool start = false;
            bool end = false;
            bool spawn = false;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.Equals("Tile"))
                    {
                        if (tileTag)
                        {
                            list.Add(new TileMockup(wall, coin, obstacle, start, end, spawn));
                            tileTag = wall = coin = obstacle = start = end = spawn = false;
                        }
                    }

                    if (reader.Value.Equals("isWall"))
                    {
                        reader.Read();
                        wall = (long) reader.Value == 1;
                    }

                    if (reader.Value.Equals("hasCoin"))
                    {
                        reader.Read();
                        coin = (long) reader.Value == 1;
                    }

                    if (reader.Value.Equals("hasObstacle"))
                    {
                        reader.Read();
                        obstacle = (long) reader.Value == 1;
                    }

                    if (reader.Value.Equals("isStart"))
                    {
                        reader.Read();
                        start = (long) reader.Value == 1;
                    }

                    if (reader.Value.Equals("isEnd"))
                    {
                        reader.Read();
                        end = (long) reader.Value == 1;
                    }

                    if (reader.Value.Equals("isSpawn"))
                    {
                        reader.Read();
                        spawn = (long) reader.Value == 1;
                        tileTag = true;
                    }
                }
            }

            list.Add(new TileMockup(wall, coin, obstacle, start, end, spawn));

            if (list.Count < 300)
            {
                throw new FormatException();
            }
        }


        private void SetStreamReader(string filePath)
        {
            try
            {
                _streamReader = new StreamReader(filePath);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw new FileNotFoundException();
            }
        }


        private string SetFilePath(string baseDir, string fileName)
        {
            DirectoryInfo dirInfo;

            try
            {
                dirInfo = new DirectoryInfo(baseDir + General.PlaygroundPath);
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw new DirectoryNotFoundException();
            }

            FileInfo[] files = dirInfo.GetFiles(fileName);

            if (files.Length <= 0)
            {
                throw new InvalidOperationException();
            }

            return baseDir + General.PlaygroundPath + fileName;
        }

        public List<TileMockup> GetOutputList() => _tileMockups;
    }
}