﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject1
{
    public class Iceberg
    {
        private const float ANIMATION_SPEED = 0.12f;

        private double animationTimer;

        private int animationFrame;

        public Vector2 position;

        private Texture2D texture;

        private BoundingCircle bounds;
        /// <summary>
        /// bonding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        public bool Collected { get; set; } = false;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public Iceberg(Vector2 position)
        {
            this.position = position;
            this.bounds = new BoundingCircle(position + new Vector2(75, 100), 40);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Iceberg");
        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //if (Collected) return;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 4) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 128, 0, 128, 128);
            spriteBatch.Draw(texture, position, source, Color.White);
        }
    }
}