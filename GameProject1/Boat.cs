using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1
{
    public enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }
    public class Boat
    {
        
        /// <summary>
        /// The game this boat is a part of
        /// </summary>
        Game game;

        /// <summary>
        /// The texture to apply to a boat
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// Direction of the boat
        /// </summary>
        public Direction Direction;

        /// <summary>
        /// The position of the boat in the game world
        /// </summary>
        public Vector2 Position { get; set; }

        private double directionTimer;

        private double animationTimer;

        private short animationFrame = 1;

        /// <summary>
        /// Constructs a new boat instance
        /// </summary>
        /// <param name="game">The game this ball belongs in</param>
        /// <param name="color">A color to distinguish this ball</param>
        public Boat(Game game)
        {
            this.game = game;
            
            
        }

        /// <summary>
        /// Loads the boat's texture
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            texture = game.Content.Load<Texture2D>("BoatSprite");
        }
        /// <summary>
        /// Updates the bat spire to fly in a direction
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

        }
        /// <summary>
        /// Draws the boat at its current position
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //upadate animation frame
            if (animationTimer > 0.3)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= 0.3;
            }
            //draws the animation
            var source = new Rectangle(animationFrame * 200, (int)Direction * 200, 200, 200);
            spriteBatch.Draw(texture, Position, source, Color.White);
        }
        

    }
}
