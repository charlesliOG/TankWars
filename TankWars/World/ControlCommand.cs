// By Brevin Bell, and Charles Li
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TankWars;

namespace TankWars
{
    /// <summary>
    /// contains information to send to server
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ControlCommand
    {
        [JsonProperty]
        public string moving;
        [JsonProperty]
        public string fire;
        [JsonProperty]
        public Vector2D tdir;

        public ControlCommand(string Moving, string Fire, Vector2D dir)
        {
            moving = Moving;
            fire = Fire;
            tdir = dir;
        }

        public void setMoving(string Moving)
        {
            moving = Moving;
        }

        public void setDirection(Vector2D Dir)
        {
            tdir = Dir;
        }

        public string getFireing()
        {
            return fire;
        }

        public void setFiring(string weapon)
        {
            fire = weapon;
        }
    }
}
