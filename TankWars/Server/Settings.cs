// By Brevin Bell, and Charles Li
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TankWars;

namespace Server
{
    /// <summary>
    /// information from the setting file
    /// </summary>
    public class Settings
    {
        public int UniversSize { get; } = 2000;

        public int MSPerFrame { get; } = 17;

        public int FramesPerShot { get; }

        public int RespawnRate { get; }

        public string GameMode { get; }

        public int maxHP { get; } = 3;
        public HashSet<Wall> Walls { get; } = new HashSet<Wall>();
        public Settings(string filepath)
        {
            
            // reading the settings file
            try
            {
                using (XmlReader reader = XmlReader.Create(filepath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "UniverseSize":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int u);
                                    UniversSize = u;
                                    break;
                                case "GameMode":
                                    reader.Read();
                                    GameMode = reader.Value;
                                    break;
                                case "MSPerFrame":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int m);
                                    MSPerFrame = m;
                                    break;
                                case "FramesPerShot":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int f);
                                    FramesPerShot = f;
                                    break;
                                case "RespawnRate":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int r);
                                    RespawnRate = r;
                                    break;
                                case "Wall":
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    int.TryParse(reader.Value, out int p1x);
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    int.TryParse(reader.Value, out int p1y);
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    int.TryParse(reader.Value, out int p2x);
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    int.TryParse(reader.Value, out int p2y);
                                    Walls.Add(new Wall(new Vector2D(p1x, p1y), new Vector2D(p2x, p2y)));                                   
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("bad settings file");
            }
        }

    }
}
