// By Brevin Bell, and Charles Li
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TankWars
{
    /// <summary>
    /// Contains information for a tank
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        [JsonProperty]
        private int tank;
        [JsonProperty]
        private string name;
        [JsonProperty]
        public Vector2D loc { get; internal set; }
        [JsonProperty]
        public Vector2D tdir { get; internal set; }
        [JsonProperty]
        public Vector2D bdir { get; internal set; }
        [JsonProperty]
        private int score;
        [JsonProperty]
        public int hp { get; internal set; }
        [JsonProperty]
        public bool died { get; internal set; }
        [JsonProperty]
        private bool dc;
        [JsonProperty]
        private bool join;

        public Vector2D Velocity { get; internal set; }
        public const double EnginePower = 3;
        public const int Size = 60;
        private int cooldown;
        public int respawnRate { get; set; }
        public int fireRate { get; set; }
        public int beams { get; set; }
        public bool beamhit { get; set; }
        private double topP, bottomP, leftP, rightP;
        private int spawnTimer;
        private int MaxHP;

        public Tank()
        { 
        
        }

        /// <summary>
        /// sets tank information
        /// </summary>
        public Tank(int id, double lX, double lY, double aX, double aY, double bX, double bY, string Name)
        {
            tank = id;
            loc = new Vector2D(lX, lY);
            tdir = new Vector2D(aX, aY);
            bdir = new Vector2D(bX, bY);
            name = Name;
            score = 0;
            hp = 3;
            died = false;
            dc = false;
            join = false;
            Velocity = new Vector2D(0, 0);
            cooldown = 0;
            beams = 0;
            beamhit = false;

        }
        public Tank(int id, string Name, int maxHP, Vector2D Loc)
        {
            tank = id;
            loc = Loc;
            bdir = new Vector2D(0, -1);
            tdir = bdir;
            name = Name;
            hp = maxHP;
            score = 0;
            died = false;
            dc = false;
            join = true;
            Velocity = new Vector2D(0, 0);
            cooldown = 0;
            MaxHP = maxHP;
            beams = 0;
            beamhit = false;
        }

        public void ResetHP()
        {
            hp = MaxHP;
        }

        /// <summary>
        /// counts down the cooldown
        /// </summary>
        public void countdown()
        {
            if (cooldown != 0)
            {
                cooldown -= 1;
            }
        }

        /// <summary>
        /// resets the cooldown to the firerate
        /// </summary>
        public void resetCooldown()
        {
            cooldown = fireRate;
        }

        public int getCooldown()
        {
            return cooldown;
        }

        /// <summary>
        /// sets spawnTimer to respawnRate
        /// </summary>
        public void resetRespawnCD()
        {
            spawnTimer = respawnRate;
        }

        public int getspawnTimer()
        {
            return spawnTimer;
        }

        public void incrementSpawnTimer()
        {
            spawnTimer -= 1;
        }

        /// <summary>
        /// call when tank is hit
        /// </summary>
        public void hitTank()
        {
            hp--;
            if (hp == 0)
            {
                died = true;
            }
        }

        /// <summary>
        /// getter for loc
        /// </summary>
        public Vector2D getLocation()
        {
            return loc;
        }
        /// <summary>
        /// getter for tdir
        /// </summary>
        /// <returns></returns>
        public Vector2D getOrientation()
        {
            return tdir;
        }
        /// <summary>
        /// getter for bdir
        /// </summary>
        /// <returns></returns>
        public Vector2D getOrientationB()
        {
            return bdir;
        }
        /// <summary>
        /// getter for name
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return name;
        }
        /// <summary>
        /// getter for score
        /// </summary>
        /// <returns></returns>
        public int getScore()
        {
            return score;
        }
        /// <summary>
        /// getter for tdir
        /// </summary>
        /// <returns></returns>
        public Vector2D getTdir()
        {
            return tdir;
        }

        /// <summary>
        /// call when tank disconnects
        /// </summary>
        public void dcTank()
        {
            dc = true;
            died = true;
            hp = 0;
        }

        /// <summary>
        /// getter for tank id
        /// </summary>
        /// <returns></returns>
        public int getId()
        {
            return tank;
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
        /// getter for hp
        /// </summary>
        /// <returns></returns>
        public int getHP()
        {
            return hp;
        }
        /// <summary>
        /// when the tank leaves the world wraps it around to the other side of the map
        /// </summary>
        /// <param name="worldSize"></param>
        public void wrapAround(int worldSize)
        {
            int leftW = -worldSize/2;
            int topW = worldSize/2;
            int rightW = worldSize/2;
            int bottomW = -worldSize/2;
            if (loc.GetX() + Size/2 > rightW)
            {
                Vector2D newloc = new Vector2D(leftW + Size / 2, loc.GetY());
                loc = newloc;
            }
            else if (loc.GetX() - Size / 2 <= leftW)
            {
                Vector2D newloc = new Vector2D(rightW - Size / 2, loc.GetY());
                loc = newloc;
            }
            else if (loc.GetY() + Size / 2 > topW)
            {
                Vector2D newloc = new Vector2D( loc.GetX(), bottomW + Size / 2);
                loc = newloc;
            }
            else if (loc.GetY() - Size / 2 <= bottomW)
            {
                Vector2D newloc = new Vector2D(loc.GetX(),topW - Size / 2);
                loc = newloc;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }

        /// <summary>
        /// check for colistion with projectile
        /// </summary>
        /// <param name="newLoc"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        internal bool CollidesProjectileTank(Vector2D newLoc, Projectile p)
        {
            double Expansion = Size / 2 + Projectile.Size / 2;
            leftP = Math.Min(loc.GetX(), loc.GetX()) - Expansion;
            rightP = Math.Max(loc.GetX(), loc.GetX()) + Expansion;
            topP = Math.Min(loc.GetY(), loc.GetY()) - Expansion;
            bottomP = Math.Max(loc.GetY(), loc.GetY()) + Expansion;

            return leftP < newLoc.GetX()
                && rightP > newLoc.GetX()
                && topP < newLoc.GetY()
                && bottomP > newLoc.GetY()
                && p.getId() != getId();
        }

        internal void incrementScore()
        {
            score++;
        }

        internal bool getDC()
        {
            return dc;
        }
    }
}
