using GameProject1.Collisions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject1.Obstacles
{
    public static class SpriteBatchExtensions
    {
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            Texture2D dummyTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new[] { Color.White });

            spriteBatch.Draw(dummyTexture, rectangle, color);
        }
    }

    public class Kraken
    {


        public Vector2 position;

        private Texture2D texture;

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
        public Kraken(Vector2 position)
        {
            this.position = position;
            bounds = new BoundingRectangle(position + new Vector2(100, 88), 75, 200);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Kraken2");
        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //if (Collected) return;
            //var source = new Rectangle(128, 0, 128, 128);
            //spriteBatch.DrawRectangle(new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height), Color.Red);
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0,0), 2f, SpriteEffects.None, 0);
        }
    }
}
