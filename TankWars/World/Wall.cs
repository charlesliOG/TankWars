// By Brevin Bell, and Charles Li
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TankWars
{
    /// <summary>
    /// Contains information for a wall
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        private double top, bottom, left, right;
        private double topP, bottomP, leftP, rightP;
        private const double thickness = 50;

        [JsonProperty]
        private int wall;
        [JsonProperty]
        private Vector2D p1;
        [JsonProperty]
        private Vector2D p2;

        /// <summary>
        /// sets wall information 
        /// </summary>
        public Wall(int id, double p1X, double p1Y, double p2X, double p2Y)
        {
            wall = id;
            p1 = new Vector2D(p1X, p1Y);
            p2 = new Vector2D(p2X, p2Y);

            double Expansion = thickness / 2 + Tank.Size / 2;
            left = Math.Min(p1.GetX(), p2.GetX()) - Expansion;
            right = Math.Max(p1.GetX(), p2.GetX()) + Expansion;
            top = Math.Min(p1.GetY(), p2.GetY()) - Expansion;
            bottom = Math.Max(p1.GetY(), p2.GetY()) + Expansion;

            Expansion = thickness / 2 + Projectile.Size / 2;
            leftP = Math.Min(p1.GetX(), p2.GetX()) - Expansion;
            rightP = Math.Max(p1.GetX(), p2.GetX()) + Expansion;
            topP = Math.Min(p1.GetY(), p2.GetY()) - Expansion;
            bottomP = Math.Max(p1.GetY(), p2.GetY()) + Expansion;
        }

        private static int nextId = 0;
        public Wall()
        {

        }


        public Wall(Vector2D P1, Vector2D P2)
        {
            wall = nextId++;
            p1 = P1;
            p2 = P2;


            // getting the top and bottome of the wall adjusted for tanks below for projectiles
            double Expansion = thickness / 2 + Tank.Size / 2;
            left = Math.Min(p1.GetX(), p2.GetX()) - Expansion;
            right = Math.Max(p1.GetX(), p2.GetX()) + Expansion;
            top = Math.Min(p1.GetY(), p2.GetY()) - Expansion;
            bottom = Math.Max(p1.GetY(), p2.GetY()) + Expansion;

            Expansion = thickness / 2 + Projectile.Size / 2;
            leftP = Math.Min(p1.GetX(), p2.GetX()) - Expansion;
            rightP = Math.Max(p1.GetX(), p2.GetX()) + Expansion;
            topP = Math.Min(p1.GetY(), p2.GetY()) - Expansion;
            bottomP = Math.Max(p1.GetY(), p2.GetY()) + Expansion;
        }


        public Vector2D getLocation()
        {
            return p1;
        }
        /// <summary>
        /// getter for for wall id
        /// </summary>
        /// <returns></returns>
        public int getId()
        {
            return wall;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }

        /// <summary>
        /// check for colistion with tank
        /// </summary>
        /// <param name="projLoc"></param>
        /// <returns></returns>
        public bool CollidesTank(Vector2D TankLoc)
        {
            return left < TankLoc.GetX()
                && right > TankLoc.GetX()
                && top < TankLoc.GetY()
                && bottom > TankLoc.GetY();
        }

        /// <summary>
        /// check for colistion with projetile
        /// </summary>
        /// <param name="projLoc"></param>
        /// <returns></returns>
        public bool CollidesProjectile(Vector2D projLoc)
        {
            return leftP < projLoc.GetX()
                && rightP > projLoc.GetX()
                && topP < projLoc.GetY()
                && bottomP > projLoc.GetY();
        }

        /// <summary>
        /// check for colistion with projetile in the x direction
        /// </summary>
        /// <param name="projLoc"></param>
        /// <returns></returns>
        public bool CollidesProjectileX(Vector2D projLoc)
        {
            if (((leftP - projLoc.GetX()) <=(topP - projLoc.GetY())) && ((leftP - projLoc.GetX()) <= (bottomP - projLoc.GetY())))
            {
                return false;
            }
            if (((rightP - projLoc.GetX()) <= (topP - projLoc.GetY())) && ((rightP - projLoc.GetX()) <= (bottomP - projLoc.GetY())))
            {
                return false;
            }
            return true;
        }

        //segment helper method
        public List<Vector2D> wallSegments()
        {
            List<Vector2D> wallSegs = new List<Vector2D>();
            //determine how many x or y segments a wall has
            double ySegments = (Math.Abs(p1.GetY() - p2.GetY())) / 50;
            double xSegments = Math.Abs(p1.GetX() - p2.GetX()) / 50;
            //if the wall has y segments
            if (!(ySegments == 0))
            {
                for (int y = 0; y <= ySegments; y++)
                {
                    Vector2D seg;
                    if (p1.GetY() > p2.GetY())
                    {
                        //getting the x,y of each segment
                        seg = new Vector2D(p1.GetX() , p2.GetY()+(50*y));
                    }
                    else
                    {
                        seg = new Vector2D(p1.GetX(), p1.GetY() + (50 * y));
                    }
                    wallSegs.Add(seg);

                }
            }
            //if wall has x segments
            else
            {
                for (int x = 0; x <= xSegments; x++)
                {
                    Vector2D seg;
                    if (p1.GetX() > p2.GetX())
                    {
                        //getting the x,y of each segment
                        seg = new Vector2D( p2.GetX() + (50 * x), p1.GetY());
                    }
                    else
                    {
                        seg = new Vector2D(p1.GetX() + (50 * x), p1.GetY());
                    }
                    wallSegs.Add(seg);

                }
            }
            return wallSegs;
        }

    }
}
