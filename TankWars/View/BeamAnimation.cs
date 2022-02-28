// By Brevin Bell, and Charles Li
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWars;
namespace View
{
    /// <summary>
    /// Contains information for a beam animation
    /// </summary>
    public class BeamAnimation
    {
        private double frame;
        private int id;
        private Beam beam;
        public BeamAnimation(int Id,Beam b)
        {
            ///frame determines how long we want the animation to last for
            frame = 10;
            id = Id;
            beam = b;
        }

        /// <summary>
        /// decrements frame
        /// </summary>
        public void increment()
        {
            frame -= .2;
        }
        /// <summary>
        /// getter for drame
        /// </summary>
        /// <returns></returns>
        public int getFrame()
        {
            return (int)frame;
        }
        /// <summary>
        /// getter for id
        /// </summary>
        /// <returns></returns>
        public int getId()
        {
            return id;
        }
        /// <summary>
        /// getter for beam
        /// </summary>
        /// <returns></returns>
        public Beam getbeam()
        {
            return beam;
        }
    }
}
