using GameProject1.Collisions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject1.BoatThings;

namespace GameProject1.Obstacles
{
    public class Tentacle
    {
        private const float ANIMATION_SPEED = .3f;

        private double animationTimer;

        private double spawnTimer;

        private int animationFrame;

        public Vector2 position;

        private Texture2D texture;

        public double TimeToLive { get; set; } = 10;  // Object lifespan in seconds

       

        private BoundingRectangle bounds;
        /// <summary>
        /// bonding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public bool isHit { get; set; } = false;

        public bool wasHit { get; set; } = false;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public Tentacle(Vector2 position, ContentManager content)
        {
            this.position = position;
            bounds = new BoundingRectangle(position + new Vector2(15, 44), 105, 115);
            texture = content.Load<Texture2D>("Tentacel3");
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Tentacel3");
        }

        public void Update(GameTime gameTime)
        {
            // Update object behavior here

            // Decrease the time to live
            TimeToLive -= gameTime.ElapsedGameTime.TotalSeconds;
        }
        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (TimeToLive < 0)
            {
                bounds = new BoundingRectangle(0, 0, 0, 0);
                return;
            }

            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 128, 0, 128, 128);
            spriteBatch.Draw(texture, position, source, Color.White);

            
        }
    }
}
