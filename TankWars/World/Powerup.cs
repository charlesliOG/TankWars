// By Brevin Bell, and Charles Li
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    /// <summary>
    /// Contains information for a powerup
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Powerup
    {
        [JsonProperty]
        private int power;
        [JsonProperty]
        private Vector2D loc;
        [JsonProperty]
        private bool died;

        private double top, bottom, left, right;
        private int respawnRate = 1650;
        private int size = 5;

        public int spawnTimer { get; internal set; }
        /// <summary>
        /// sets up information for powerup
        /// </summary>
        public Powerup(int id, double locX, double locY)
        {
            power = id;
            loc = new Vector2D(locX, locY);
            died = false;

        }

        /// <summary>
        /// check if a tank picks up a powerup
        /// </summary>
        /// <param name="TankLoc"></param>
        /// <returns></returns>
        public bool CollidesTank(Vector2D TankLoc)
        {

            double Expansion = size / 2 + Tank.Size / 2;
            left = Math.Min(loc.GetX(), loc.GetX()) - Expansion;
            right = Math.Max(loc.GetX(), loc.GetX()) + Expansion;
            top = Math.Min(loc.GetY(), loc.GetY()) - Expansion;
            bottom = Math.Max(loc.GetY(), loc.GetY()) + Expansion;
            return left < TankLoc.GetX()
                && right > TankLoc.GetX()
                && top < TankLoc.GetY()
                && bottom > TankLoc.GetY()
                && !died;
        }

        /// <summary>
        /// getter for loc
        /// </summary>
        /// <returns></returns>
        public Vector2D getLocation()
        {
            return loc;
        }

        /// <summary>
        /// sets spawn timer to a random number between the respawn rate
        /// </summary>
        public void resetSpawnTimer()
        {
            Random rand = new Random();
            spawnTimer = rand.Next(0,respawnRate);
        }

        /// <summary>
        /// getter for power
        /// </summary>
        /// <returns></returns>
        public int getId()
        {
            return power;
        }
        /// <summary>
        /// getter for died
        /// </summary>
        /// <returns></returns>
        public bool isdead()
        {
            return died;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }

        internal void setLocation(Vector2D newloc)
        {
            loc = newloc;
        }

        internal void respawn()
        {
            died = false;
        }

        /// <summary>
        /// powerup is picked up
        /// </summary>
        /// <param name="t"></param>
        internal void dies(Tank t)
        {
            died = true;
            resetSpawnTimer();
            t.beams += 1;
        }
    }
}
