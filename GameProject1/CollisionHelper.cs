using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1
{
    public class CollisionHelper
    {
        /// <summary>
        /// Detects collision between two bounding circles
        /// </summary>
        /// <param name="a">circle one</param>
        /// <param name="b">circle 2</param>
        /// <returns>true for collision</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= (
                Math.Pow(a.Center.X - b.Center.X, 2) +
                Math.Pow(a.Center.Y - b.Center.Y, 2));
        }
        /// <summary>
        /// detects rectangle collisions between 2 bounding rectangles
        /// </summary>
        /// <param name="a">the first shape</param>
        /// <param name="b">the second shape</param>
        /// <returns>true for collisions false otherwise</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right ||
                     a.Top > b.Bottom || a.Bottom < b.Top);
        }
        /// <summary>
        /// detects a collision between circle and rectangle
        /// </summary>
        /// <param name="c">circle</param>
        /// <param name="r">rectangles</param>
        /// <returns>true if collision false if otherwise</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >= (
                Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2));
        }

        public static bool Collides(BoundingRectangle r, BoundingCircle c)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >= (
                Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2));
        }
    }
}
