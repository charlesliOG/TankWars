// By Brevin Bell, and Charles Li
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TankWars
{
    /// <summary>
    /// Contains information for a projectile
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty]
        public int proj { get; internal set; }
        [JsonProperty]
        public Vector2D loc { get; internal set; }
        [JsonProperty]
        public Vector2D dir { get; internal set; }
        [JsonProperty]
        public bool died { get; internal set; }
        [JsonProperty]
        private int owner;
        public Vector2D Velocity { get; internal set; }
        public const double speed = 25;
        public const int Size = 30;

        /// <summary>
        /// sets up projectile information
        /// </summary>
        public Projectile(int id, double locX, double locY, double dirX, double dirY, int ownerID)
        {
            proj = id;
            loc = new Vector2D(locX, locY);
            dir = new Vector2D(dirX, dirY);
            owner = ownerID;
            died = false;
            Velocity = new Vector2D(0, 0);
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
        /// getter for dir
        /// </summary>
        /// <returns></returns>
        public Vector2D getOrientation()
        {
            return dir;
        }
        /// <summary>
        /// getter for owner
        /// </summary>
        /// <returns></returns>
        public int getId()
        {
            return owner;
        }


        /// <summary>
        /// getter for died
        /// </summary>
        /// <returns></returns>
        public bool isdead()
        {
            return died;
        }

        /// <summary>
        /// destroys projectiles that go out of the map
        /// </summary>
        /// <param name="worldSize"></param>
        public void wrapAround(int worldSize)
        {
            int leftW = -worldSize / 2;
            int topW = worldSize / 2;
            int rightW = worldSize / 2;
            int bottomW = -worldSize / 2;
            if (loc.GetX() + Size / 2 > rightW)
            {

                died = true;
            }
            else if (loc.GetX() - Size / 2 <= leftW)
            {
                died = true;
            }
            else if (loc.GetY() + Size / 2 > topW)
            {
                died = true;
            }
            else if (loc.GetY() - Size / 2 <= bottomW)
            {
                died = true;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }
    }
}