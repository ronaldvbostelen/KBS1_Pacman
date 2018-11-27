using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WpfGame.Generals;
using WpfGame.Models;

namespace WpfGame.Editor
{
    public class JsonPlaygroundParser
    {
        private JsonTextReader _jsonTextReader;
        private StreamReader _streamReader;
        private List<TileMockup> _tileMockups;
        private DirectoryInfo _directoryInfo;
        private string playgroundJsonPath;
        private FileInfo[] files;
        private string fileName;


        public JsonPlaygroundParser(string fileName)
        {
            //ToDo: Implement exception handling. You'll get an error now if you don't have the playground
            this.fileName = fileName;
            playgroundJsonPath = System.AppDomain.CurrentDomain.BaseDirectory + General.playgroundPath + fileName;

            _streamReader = new StreamReader(playgroundJsonPath);
            _jsonTextReader = new JsonTextReader(_streamReader);

            _tileMockups = new List<TileMockup>();

            ReadJson(_jsonTextReader, _tileMockups);
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
                        wall = (long)reader.Value == 1;
                    }

                    if (reader.Value.Equals("hasCoin"))
                    {
                        reader.Read();
                        coin = (long)reader.Value == 1;
                    }

                    if (reader.Value.Equals("hasObstacle"))
                    {
                        reader.Read();
                        obstacle = (long)reader.Value == 1;
                    }

                    if (reader.Value.Equals("isStart"))
                    {
                        reader.Read();
                        start = (long)reader.Value == 1;
                    }

                    if (reader.Value.Equals("isEnd"))
                    {
                        reader.Read();
                        end = (long)reader.Value == 1;
                        
                    }

                    if (reader.Value.Equals("isSpawn"))
                    {
                        reader.Read();
                        spawn = (long)reader.Value == 1;
                        tileTag = true;
                    }
                }
            }
            list.Add(new TileMockup(wall, coin, obstacle, start, end,spawn));
        }



        private void SetStreamReader(StreamReader streamReader, string filePath)
        {
            try
            {
                streamReader = new StreamReader(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "NO INPUT PLAYGROUND");
                throw;
            }
        }

        
        private void getFiles(string baseDir)
        {
            _directoryInfo = new DirectoryInfo(baseDir + General.playgroundPath);
            files = _directoryInfo.GetFiles("*.json");
        }


        public List<TileMockup> GetOutputList() => _tileMockups;
    }
}
