using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1.Collisions
{
    public class BoundingCircle
    {
        /// <summary>
        /// center of the bounding circle
        /// </summary>
        public Vector2 Center;
        /// <summary>
        /// the radius of the bounding circle
        /// </summary>
        public float Radius;
        /// <summary>
        /// constructs new bounding circle
        /// </summary>
        /// <param name="center">the center</param>
        /// <param name="radius">the radius</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        /// <summary>
        /// tests for a collisions between this and another bounding circle
        /// </summary>
        /// <param name="other">the other bounding circle</param>
        /// <returns>true for collision false otherwise</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingRectangle r)
        {
            return CollisionHelper.Collides(this, r);
        }
    }
}
