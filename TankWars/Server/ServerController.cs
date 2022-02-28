// By Brevin Bell, and Charles Li
using System;
using System.Collections.Generic;
using System.Text;
using TankWars;
using NetworkUtil;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json; 

namespace Server
{/// <summary>
/// Handels networking with clients and tells world to update the logic
/// </summary>
    public class ServerController
    {
        private Settings settings { get; }

        private World theWorld { get; }
        private string startupInfo;
        private Dictionary<int, SocketState> clients = new Dictionary<int, SocketState>();
        
        public ServerController(Settings setting)
        {
            // setting up the world
            settings = setting;
            theWorld = new World(settings.UniversSize);
            theWorld.setGameMode(setting.GameMode);
            foreach (Wall wall in settings.Walls)
            {
                theWorld.getWalls()[wall.getId()] = wall;
            }
            Vector2D newloc = theWorld.getRandomLoc();
            theWorld.getPowerups()[0] = new Powerup(0, newloc.GetX(), newloc.GetY());
            newloc = theWorld.getRandomLoc();
            theWorld.getPowerups()[1] = new Powerup(1, newloc.GetX(), newloc.GetY());

            // setting up startup info to send
            StringBuilder sb = new StringBuilder();
            sb.Append(theWorld.getSize());
            sb.Append("\n");
            foreach (Wall wall in theWorld.getWalls().Values)
            {
                sb.Append(wall.ToString());
            }
            startupInfo = sb.ToString();
        }

        /// <summary>
        /// starts the server
        /// </summary>
        internal void Start()
        {
            Networking.StartServer(NewClient, 11000);
            Thread t = new Thread(Update);
            t.Start();
        }

        /// <summary>
        /// updates the world and sends info to clients
        /// </summary>
        /// <param name="obj"></param>
        private void Update(object obj)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                while (watch.ElapsedMilliseconds < settings.MSPerFrame)
                {

                }
                watch.Restart();
                StringBuilder sb = new StringBuilder();
                lock (theWorld)
                {
                    theWorld.Update();
                    foreach (Tank tank in theWorld.getTanks().Values)
                    {
                        sb.Append(tank.ToString());
                    }
                    foreach (Projectile p in theWorld.getProjectile().Values)
                    {
                        sb.Append(p.ToString());
                    }
                    foreach (Powerup p in theWorld.getPowerups().Values)
                    {
                        sb.Append(p.ToString());
                    }
                    foreach (Beam b in theWorld.getBeams().Values)
                    {
                        sb.Append(b.ToString());
                    }
                }
                string frame = sb.ToString();
                lock (clients)
                {
                    foreach( SocketState client in clients.Values)
                    {
                        Networking.Send(client.TheSocket, frame);
                    }
                }
            }
        }

        /// <summary>
        /// called when reciving a new client
        /// </summary>
        /// <param name="client"></param>
        private void NewClient(SocketState client)
        {
            client.OnNetworkAction = RecivePlayerName;
            Networking.GetData(client);
        }

        /// <summary>
        /// gets the player name and sends them startupinfo
        /// add them to the world and then starts to gets ready to recive commands
        /// </summary>
        /// <param name="client"></param>
        private void RecivePlayerName(SocketState client)
        {
            string name = client.GetData();
            if (!name.EndsWith("\n"))
            {
                client.GetData();
                return;
            }
            Networking.Send(client.TheSocket, client.ID + "\n");
            Networking.Send(client.TheSocket, startupInfo);
            client.RemoveData(0,name.Length);
            name = name.Trim();
            lock (theWorld)
            {
                Random rand = new Random();
                int size = settings.UniversSize;
                Vector2D newloc = theWorld.getRandomLoc();
                theWorld.getTanks()[(int)client.ID] = new Tank((int)client.ID, name, settings.maxHP, newloc);
                theWorld.getTanks()[(int)client.ID].fireRate = settings.FramesPerShot;
                theWorld.getTanks()[(int)client.ID].respawnRate = settings.RespawnRate;
            }
            lock (clients)
            {
                clients.Add((int)client.ID, client);
            }
            client.OnNetworkAction = ReciveControlCommand;
            Networking.GetData(client);
        }

        /// <summary>
        /// recive control commands and handels disconecting clients
        /// </summary>
        /// <param name="client"></param>
        private void ReciveControlCommand(SocketState client)
        {
            if (client.ErrorOccurred)
            {
                lock (clients)
                {
                    theWorld.getTanks()[(int)client.ID].dcTank();
                    clients.Remove((int)client.ID);
                }
                return;
            }
            string totalData = client.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            foreach (string p in parts)
            {
                if (p.Length == 0)
                {
                    continue;
                }
                if (p[p.Length - 1] != '\n')
                {
                    break;
                }
                ControlCommand controlCommand;
                try
                {
                    controlCommand = JsonConvert.DeserializeObject<ControlCommand>(p);
                }
                catch (Exception)
                {
                    Console.WriteLine("Bad input from clinet disconecting them");
                    lock (clients)
                    {
                        theWorld.getTanks()[(int)client.ID].dcTank();
                        clients.Remove((int)client.ID);
                    }
                    return;
                }
                lock (theWorld)
                {
                    theWorld.controlCommands[(int)client.ID] = controlCommand;
                }
                client.RemoveData(0,p.Length);
            }
            Networking.GetData(client);
        }
    }
}
