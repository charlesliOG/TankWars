// By Brevin Bell, and Charles Li
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    /// <summary>
    /// Contains information for a beam
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Beam
    {
        [JsonProperty]
        private int beam;
        [JsonProperty]
        private Vector2D org;
        [JsonProperty]
        private Vector2D dir;
        [JsonProperty]
        private int owner;

        /// <summary>
        /// sets up information for a beam
        /// </summary>
        public Beam(int id, double orgX, double orgY, double dirX, double dirY, int ownerID)
        {
            beam = id;
            org = new Vector2D(orgX, orgY);
            dir = new Vector2D(dirX, dirY);
            owner = ownerID;
        }
        /// <summary>
        /// getter for beam
        /// </summary>
        /// <returns></returns>
        public int getId()
        {
            return beam;
        }
        /// <summary>
        /// getter for org
        /// </summary>
        /// <returns></returns>
        public Vector2D getLocation()
        {
            return org;
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
        /// turn into a json string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }
    }
}
