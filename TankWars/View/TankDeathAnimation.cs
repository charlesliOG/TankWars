using System;
using System.Collections.Generic;
using System.Linq;
// By Brevin Bell, and Charles Li
using System.Text;
using System.Threading.Tasks;
using TankWars;
namespace View
{
    /// <summary>
    /// Contains information for a beam animation
    /// </summary>
    public class TankDeathAnimation
    {
        private double frame;
        private Tank t;
        private int id;
        public TankDeathAnimation(Tank tank)
        {
            //determines how long we want animation to last
            frame = 300;
            t = tank;
            id = tank.getId();
        }

        /// <summary>
        /// decrements frame
        /// </summary>
        public void increment()
        {
            if (frame <= 0)
            {
                return;
            }
            frame -= .3;
        }
        /// <summary>
        /// getter for frame
        /// </summary>
        /// <returns></returns>
        public int getFrame()
        {
            return (int)frame;
        }
        /// <summary>
        /// getter for t
        /// </summary>
        /// <returns></returns>
        public Tank getTank()
        {
            return t;
        }
    }
}
