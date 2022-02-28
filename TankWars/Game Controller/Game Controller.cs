// By Brevin Bell, and Charles Li
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NetworkUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TankWars;
using System.Threading;

// for useing data from the server
namespace Game_Controller
{

    /// <summary>
    /// controller for TankWars
    /// </summary>
    public class Controller
    {
        // Event handers to notify the view
        public delegate void ServerUpdateHandler();
        public event ServerUpdateHandler UpdateArrived;

        public delegate void ConnectedHandler();
        public event ConnectedHandler Connected;

        public delegate void ErrorHandler(string err);
        public event ErrorHandler Error;

        int playerID;

        SocketState theServer;
        World world;
        string playerName;
        ControlCommand command;

        /// <summary>
        /// Starts connection proccess with information provided
        /// </summary>
        /// <param name="hostname">address</param>
        /// <param name="name">player name</param>
        public void StartConnection(string hostname, string name)
        {
            //set a default turret angle
            Vector2D startTurretAngel = new Vector2D(0,1);
            startTurretAngel.Normalize();
            command = new ControlCommand("none","none",startTurretAngel);
            if (name.Length > 16)
            {
                Error("Name was to long");
                return;
            }
            lock ("theServer")
            {
                playerName = name;
                Networking.ConnectToServer(OnConnect, hostname, 11000);
            }

        }

        /// <summary>
        /// callback from StartConnection that starts to send and receive info
        /// </summary>
        /// <param name="state"></param>
        private void OnConnect(SocketState state)
        {
            lock ("theServer")
            {
                if (state.ErrorOccurred)
                {
                    // inform the view
                    Error(state.ErrorMessage);
                    return;
                }

                theServer = state;
                theServer.OnNetworkAction = ProcessMessages;
                Networking.Send(theServer.TheSocket, playerName + "\n");
                Networking.GetData(theServer);

            }
        }

        /// <summary>
        /// deals with information received from server
        /// </summary>
        /// <param name="state"></param>
        private void ProcessMessages(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                Error(state.ErrorMessage);
                return;
            }
            string totalData = state.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.

            List<string> newMessages = new List<string>();

            
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                // build a list of messages to send to the view
                newMessages.Add(p);

                // Then remove it from the SocketState's growable buffer
                state.RemoveData(0, p.Length);
            }

            // inform the view
            MessagesArrived(newMessages);
        }

        /// <summary>
        /// updates World  with json from server
        /// </summary>
        /// <param name="newMessages"></param>
        private void MessagesArrived(List<string> newMessages)
        {

            int sizeOrId = 0;
                foreach (string message in newMessages)
                {
                    //deals with information at start of handshake, id and worldsize
                    if (WorldStart(message, sizeOrId))
                    {
                        continue;
                    }

                    JObject obj = JObject.Parse(message);

                    if (obj["wall"] != null)
                    {
                        Wall wall = JsonConvert.DeserializeObject<Wall>(message);
                        world.wallAdder(wall);

                        //do wall stuff
                    }
                    else if (obj["tank"] != null)
                    {
                        Tank tank = JsonConvert.DeserializeObject<Tank>(message);
                        world.tankAdder(tank);
                    }
                    else if (obj["proj"] != null)
                    {
                        Projectile proj = JsonConvert.DeserializeObject<Projectile>(message);
                        world.projAdder(proj);
                    }
                    else if (obj["beam"] != null)
                    {
                        Beam beam = JsonConvert.DeserializeObject<Beam>(message);
                        world.beamAdder(beam);
                        //frame beam problem
                    }
                    else if (obj["power"] != null)
                    {
                        Powerup power = JsonConvert.DeserializeObject<Powerup>(message);
                        world.powerAdder(power);
                    }
                    else
                    {
                    Error("problem with json information");
                    }
            

                //event update arrived to notify view listener
            }
            UpdateArrived();
            sendData();

            //continue to receive information
            Networking.GetData(theServer);
        }
        /// <summary>
        /// send info to server
        /// </summary>
        public void sendData()
        {
            string message = JsonConvert.SerializeObject(command);
            Networking.Send(theServer.TheSocket, message + "\n");
            //prevents all beams from firing at once
            if (command.getFireing() == "alt")
            {
                command.setFiring("none");
            }
        }

        /// <summary>
        /// id getter
        /// </summary>
        /// <returns></returns>
        public int getID()
        {
            return playerID;

        }    

        /// <summary>
        /// helper for start of handshake information
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sizeOrId"></param>
        /// <returns></returns>
        private bool WorldStart(string message, int sizeOrId)
        {

            if (int.TryParse(message, out int i))
            {
                //if its an id
                if (i < 1000)
                {
                    playerID = i;
                    return true;
                }
                //else its a worldsize
                else
                {
                    world = new World(i);
                    Connected();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// sets move to up in Control Command to send to server
        /// </summary>
        public void moveUp()
        {
            command.setMoving("up");
        }

        /// <summary>
        /// sets move to down in Control Command to send to server
        /// </summary>
        public void moveDown()
        {
            command.setMoving("down");
        }

        /// <summary>
        /// sets move to left in Control Command to send to server
        /// </summary>
        public void moveLeft()
        {
            command.setMoving("left");

        }
        /// <summary>
        /// sets move to right in Control Command to send to server
        /// </summary>
        public void moveRight()
        {
            command.setMoving("right");

        }
        /// <summary>
        /// sets move to none in Control Command to send to server
        /// </summary>
        public void noMove()
        {
            command.setMoving("none");
        }

        /// <summary>
        /// sets direction in Control Command to send to server
        /// </summary>
        public void lookDir(int x, int y)
        {
            Vector2D tdir = new Vector2D(x,y);
            tdir.Normalize();
            command.setDirection(tdir);
        }

        /// <summary>
        /// sets which firing mode in Control Command to send to server
        /// </summary>
        public void fireTurret(string weapon)
        {
            command.setFiring(weapon);
        }

        /// <summary>
        /// returns world
        /// </summary>
        public World getWorld()
        {
            return world;
        }

    }
}
