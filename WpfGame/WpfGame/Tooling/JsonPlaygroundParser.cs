using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
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
        private string playgroundJsonPath;
        private string _filename;


        public JsonPlaygroundParser(string fileName)
        {
            _filename = fileName;

            playgroundJsonPath = SetFilePath(AppDomain.CurrentDomain.BaseDirectory, fileName);
            SetStreamReader(playgroundJsonPath);

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
            list.Add(new TileMockup(wall, coin, obstacle, start, end, spawn));

            if (list.Count < 300)
            {
                MessageBox.Show(
                    $"Your playground.json file ({_filename}) isn't valide. Please select a different playground or create a valid playground",
                    "Invalid playground",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                throw new IOException("Invalid jsonfile");
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
                Console.WriteLine(e);
                MessageBox.Show($"Cannot read file: {filePath} please select a correct file.",
                    "Unable to read playground", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            
        }

        
        private string SetFilePath(string baseDir, string fileName)
        {

            try
            {
                var dirInfo = new DirectoryInfo(baseDir + General.playgroundPath);
                if (!dirInfo.Exists)
                {
                    throw new IOException("Your playground folder doesn't exist");
                }

                FileInfo[] files = dirInfo.GetFiles(fileName);
                if (files.Length <= 0)
                {
                    throw new IOException($"Your Playgrounds folder doesn't contain the selected playground: {fileName}");
                }

                return baseDir + General.playgroundPath + fileName;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.Message,
                    "Unable to load playground", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            
        }
        
        public List<TileMockup> GetOutputList() => _tileMockups;
    }
}
