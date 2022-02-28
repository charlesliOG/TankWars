using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankWars;

namespace Lab10
{
    public class DrawingPanel
    {
        private World theWorld;
        public DrawingPanel(World w)
        {
            theWorld = w;
        }


        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);


        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            e.Graphics.TranslateTransform((int)worldX, (int)worldY);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }
        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            if (t.GetID() == ...)
                color = ...;
            else if (...)
                color = ...;
            Rectangle r = new Rectangle(-(tankWidth / 2), -(tankWidth / 2), tankWidth, tankWidth);
            e.Graphics.FillRectangle(someBrush, r);
        }


        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            // Center the view on the middle of the world,
            // since the image and world use different coordinate systems
            int viewSize = Size.Width; // view is square, so we can just use width
            e.Graphics.TranslateTransform(viewSize / 2, viewSize / 2);

            lock (theWorld)
            {
                // Draw the players
                foreach (Player play in theWorld.Players.Values)
                {
                    DrawObjectWithTransform(e, play, play.GetLocation().GetX(), play.GetLocation().GetY(), play.GetOrientation().ToAngle(), Player2Drawer);
                }

                // Draw the powerups
                foreach (Powerup pow in theWorld.Powerups.Values)
                {
                    DrawObjectWithTransform(e, pow, pow.GetLocation().GetX(), pow.GetLocation().GetY(), 0, PowerupDrawer);
                }
            }


            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }

    }
}

