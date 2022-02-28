// By Brevin Bell, and Charles Li
using System;
using System.Collections.Generic;

//tank and other classes like that

namespace TankWars
{
    /// <summary>
    /// Contains information for a World
    /// </summary>
    public class World
    {
        private int size;
        //dictionaries that contain the elements of a world
        private Dictionary<int, Tank> tanks;
        private Dictionary<int, Powerup> powerups;
        private Dictionary<int, Wall> walls;
        private Dictionary<int, Projectile> projectiles;
        private Dictionary<int, Beam> beams;
        public Dictionary<int, ControlCommand> controlCommands = new Dictionary<int, ControlCommand>();
        //notify controller listener if tank died
        public delegate void TankDiedHandler(Tank t);
        public event TankDiedHandler TankDied;
        public int projectileId;
        private int beamId;
        private List<Tank> tankdc;
        Random rand = new Random();
        private string GameMode;
        /// <summary>
        /// creates a world with given size
        /// </summary>
        /// <param name="Size"></param>
        public World(int Size)
        {
            size = Size;
            tanks = new Dictionary<int, Tank>();
            powerups = new Dictionary<int, Powerup>();
            walls = new Dictionary<int, Wall>();
            projectiles = new Dictionary<int, Projectile>();
            beams = new Dictionary<int, Beam>();
            projectileId = 0;
            beamId = 0;
            tankdc = new List<Tank>();
        }

        /// <summary>
        /// getter for size
        /// </summary>
        /// <returns></returns>
        public int getSize()
        {
            return size;
        }
        /// <summary>
        /// getter for tanks
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Tank> getTanks()
        {
            return tanks;
        }

        /// <summary>
        /// getter for walls
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Wall> getWalls()
        {
            return walls;
        }

        /// <summary>
        /// getter for projectiles
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Projectile> getProjectile()
        {
            return projectiles;
        }

        /// <summary>
        /// getter for powerups
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Powerup> getPowerups()
        {
            return powerups;
        }

        /// <summary>
        /// getter for beams
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Beam> getBeams()
        {
            return beams;
        }

        /// <summary>
        /// finds a radom location that is not anything else is
        /// </summary>
        /// <returns></returns>
        public Vector2D getRandomLoc()
        {
            Vector2D newloc = new Vector2D(rand.Next(-size / 2, size / 2), rand.Next(-size / 2, size / 2));
            bool randomCollision = true;
            // will keep check new locations untill a good one is found
            while (randomCollision == true)
            {
                randomCollision = false;
                newloc = new Vector2D(rand.Next(-size / 2, size / 2), rand.Next(-size / 2, size / 2));
                // check if wall is at location
                foreach (Wall wall in walls.Values)
                {
                    if (wall.CollidesTank(newloc))
                    {
                        randomCollision = true;

                    }
                }
                // check if tank is at location
                foreach (Tank t in tanks.Values)
                {
                    if (!(Math.Abs(t.loc.GetX() - newloc.GetX()) > 50 && Math.Abs(t.loc.GetY() - newloc.GetY()) > 50))
                    {
                        randomCollision = true;
                    }
                }
                // check if powerup is at location
                foreach (Powerup p in powerups.Values)
                {
                    if (!(Math.Abs(p.getLocation().GetX() - newloc.GetX()) > 30 && Math.Abs(p.getLocation().GetY() - newloc.GetY()) > 30))
                    {
                        randomCollision = true;
                    }
                }
                // check if projectile is at location
                foreach (Projectile p in projectiles.Values)
                {
                    if (!(Math.Abs(p.getLocation().GetX() - newloc.GetX()) > 30 && Math.Abs(p.getLocation().GetY() - newloc.GetY()) > 30))
                    {
                        randomCollision = true;
                    }
                }
            }
            return newloc;
        }

        /// <summary>
        /// Determines if a ray interescts a circle
        /// </summary>
        /// <param name="rayOrig">The origin of the ray</param>
        /// <param name="rayDir">The direction of the ray</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="r">The radius of the circle</param>
        /// <returns></returns>
        public static bool Intersects(Vector2D rayOrig, Vector2D rayDir, Vector2D center, double r)
        {
            // ray-circle intersection test
            // P: hit point
            // ray: P = O + tV
            // circle: (P-C)dot(P-C)-r^2 = 0
            // substituting to solve for t gives a quadratic equation:
            // a = VdotV
            // b = 2(O-C)dotV
            // c = (O-C)dot(O-C)-r^2
            // if the discriminant is negative, miss (no solution for P)
            // otherwise, if both roots are positive, hit

            double a = rayDir.Dot(rayDir);
            double b = ((rayOrig - center) * 2.0).Dot(rayDir);
            double c = (rayOrig - center).Dot(rayOrig - center) - r * r;

            // discriminant
            double disc = b * b - 4.0 * a * c;

            if (disc < 0.0)
                return false;

            // find the signs of the roots
            // technically we should also divide by 2a
            // but all we care about is the sign, not the magnitude
            double root1 = -b + Math.Sqrt(disc);
            double root2 = -b - Math.Sqrt(disc);

            return (root1 > 0.0 && root2 > 0.0);
        }

        /// <summary>
        /// handels the logic of updating the world
        /// </summary>
        public void Update()
        {
            // makes it so beams olny show for one frame
            foreach (Beam b in beams.Values)
            {
                lock ("world")
                {
                    beams.Remove(b.getId());
                }
            }
            // handeling control Commands from client
            foreach (KeyValuePair<int, ControlCommand> c in controlCommands)
            {
                Tank t = tanks[c.Key];
                if (t.getspawnTimer() != 0)
                {
                    continue;
                }
                // handels moving
                switch (c.Value.moving)
                {
                    case "up":
                        t.Velocity = new Vector2D(0, -1);
                        t.bdir = new Vector2D(0, -1);
                        break;
                    case "down":
                        t.Velocity = new Vector2D(0, 1);
                        t.bdir = new Vector2D(0, 1);
                        break;
                    case "right":
                        t.Velocity = new Vector2D(1, 0);
                        t.bdir = new Vector2D(1, 0);
                        break;
                    case "left":
                        t.Velocity = new Vector2D(-1, 0);
                        t.bdir = new Vector2D(-1, 0);
                        break;
                    default:
                        t.Velocity = new Vector2D(0, 0);
                        break;
                }
                t.Velocity *= Tank.EnginePower;
                t.tdir = c.Value.tdir;
                t.countdown();

                // handels fireing weapons
                if (c.Value.fire == "main" && t.getCooldown() == 0)
                {
                    Projectile p = new Projectile(projectileId, t.getLocation().GetX() + t.tdir.GetX(), t.getLocation().GetY() + t.tdir.GetY(), t.tdir.GetX(), t.tdir.GetY(), t.getId());
                    projectileId += 1;
                    p.Velocity = p.dir;
                    p.Velocity.Normalize();
                    p.Velocity *= Projectile.speed;
                    projAdder(p);
                    t.resetCooldown();
                }
                if (c.Value.fire == "alt" && t.beams != 0)
                {
                    t.beams -= 1;
                    Beam b = new Beam(beamId,t.loc.GetX(),t.loc.GetY(),t.tdir.GetX(),t.tdir.GetY(),t.getId());
                    beamAdder(b);
                    beamId++;
                    foreach (Tank othertank in tanks.Values)
                    {
                        if (Intersects(t.loc,t.tdir,othertank.loc,30))
                        {
                            if (t.getId() == othertank.getId())
                            {
                                continue;
                            }
                            othertank.died = true;
                            othertank.beamhit = true;
                            othertank.hp = 0;
                            t.incrementScore();
                        }
                    }
                }
            }

            controlCommands.Clear();

            // handels powerup logic
            foreach (Powerup p in powerups.Values)
            {
                if (p.isdead())
                {
                    if (p.spawnTimer == 0)
                    {
                        Vector2D newloc = getRandomLoc();
                        p.setLocation(newloc);
                        p.respawn();
                    }
                    p.spawnTimer -= 1;
                }
                foreach (Tank t in tanks.Values)
                {
                    if (p.CollidesTank(t.loc))
                    {
                        p.dies(t);
                    }
                }
            }

            // handels removing dc tanks
            foreach (Tank t in tankdc)
            {
                tanks.Remove(t.getId());
            }
            tankdc.Clear();
            // handels tank logic
            foreach (Tank t in tanks.Values)
            {
                if (t.getDC())
                {
                    tankdc.Add(t);
                    continue;
                }
          
                if (t.died == true)
                {
                    if (t.beamhit)
                    {
                        t.beamhit = false;
                        continue;
                    }
                    t.died = false;
                    t.resetRespawnCD();

                    continue;
                }
                // handels respawning tanks
                if (t.getspawnTimer() != 0)
                {
                    t.incrementSpawnTimer();
                    if (t.getspawnTimer() == 0)
                    {
                        Vector2D newloc = getRandomLoc();
                        t.ResetHP();
                        t.loc = newloc;
                    }
                    continue;
                }
                //handels collistions with walls
                if (t.Velocity.Length() == 0)
                {
                    continue;
                }
                Vector2D newLoc = t.getLocation() + t.Velocity;
                bool collision = false;
                foreach (Wall wall in walls.Values)
                {
                    if (wall.CollidesTank(newLoc))
                    {
                        collision = true;
                        t.Velocity = new Vector2D(0, 0);
                        break;
                    }
                }
                if (!collision)
                {
                    t.loc = newLoc;
                }
                t.wrapAround(size);
            }

            //handels projectile logic
            foreach (Projectile p in projectiles.Values)
            {

                if (p.isdead())
                {
                    projectiles.Remove(p.proj);
                    continue;
                }
                Vector2D newLoc = p.getLocation() + p.Velocity;

                p.wrapAround(size);

                // wall colistion based on the gamemode you are in
                bool collision = false;
                if (GameMode == "basic")
                {
                    foreach (Wall wall in walls.Values)
                    {
                        if (wall.CollidesProjectile(newLoc))
                        {
                            collision = true;
                            p.died = true;
                            break;
                        }

                    }
                    if (!collision)
                    {
                        p.loc = newLoc;
                    }
                }
                // extra gamemode wall colistions
                else if (GameMode == "extra")
                {
                    foreach (Wall wall in walls.Values)
                    {
                        if (wall.CollidesProjectile(newLoc))
                        {
                            if (wall.CollidesProjectileX(newLoc))
                            {
                                p.dir = new Vector2D(-p.dir.GetX(), p.dir.GetY());
                                p.Velocity = p.dir;
                                p.Velocity *= Projectile.speed;
                                newLoc = p.getLocation() + p.Velocity;
                            }
                            else
                            {
                                p.dir = new Vector2D(p.dir.GetX(), -p.dir.GetY());
                                p.Velocity = p.dir;
                                p.Velocity *= Projectile.speed;
                                newLoc = p.getLocation() + p.Velocity;
                            }
                            break;
                        }
                    }
                    p.loc = newLoc;
                }

                // handels hitting tanks
                foreach (Tank tank in tanks.Values)
                {
                    if (tank.getspawnTimer() != 0)
                    {
                        continue;
                    }
                    if (tank.CollidesProjectileTank(newLoc, p))
                    {
                        collision = true;
                        p.died = true;

                        tank.hitTank();
                        if (tank.died)
                        {
                            tanks[p.getId()].incrementScore();
                        }
                        break;

                    }

                }
                if (!collision)
                {
                    p.loc = newLoc;
                }
            }
        }

        /// <summary>
        /// adds a wall to its dictionary
        /// </summary>
        public void wallAdder(Wall wall)
        {
            lock ("world")
            {
                walls[wall.getId()] = wall;
            }
        }
        /// <summary>
        /// adds a tank to its dictionary
        /// </summary>
        /// <param name="tank"></param>
        public void tankAdder(Tank tank)
        {
            lock ("world")
            {
                //if tank is dead notify listener and remove tank from dictionary
                if (tank.isdead())
                {
                    if (tanks.ContainsKey(tank.getId()))
                    {
                        TankDied(tank);
                        tanks.Remove(tank.getId());

                    }
                    return;
                }
                //add tank back
                tanks[tank.getId()] = tank;
            }
        }
        /// <summary>
        /// adds a projectile to its dictionary
        /// </summary>
        public void projAdder(Projectile proj)
        {
            lock ("world")
            {
                if (proj.isdead())
                {
                    if (projectiles.ContainsKey(proj.proj))
                    {
                        projectiles.Remove(proj.proj);
                    }
                    return;
                }
                projectiles[proj.proj] = proj;
            }
        }
        /// <summary>
        /// adds a beam to its dictionary
        /// </summary>
        public void beamAdder(Beam beam)
        {
            lock ("world")
            {
                beams[beam.getId()] = beam;
            }
        }
        /// <summary>
        /// adds a powerup to its dictionary
        /// </summary>
        public void powerAdder(Powerup power)
        {
            lock ("world")
            {
                if (power.isdead())
                {
                    if (powerups.ContainsKey(power.getId()))
                    {
                        powerups.Remove(power.getId());
                    }
                    return;
                }
                powerups[power.getId()] = power;
            }
        }

        public void setGameMode(string g)
        {
            GameMode = g;
        }
    }
}
