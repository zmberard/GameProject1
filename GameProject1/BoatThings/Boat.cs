using GameProject1.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1.BoatThings
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
        private GamePadState gamePadState;

        private KeyboardState keyboardState;
        public int Damage { get; set; } = 100;
        /// <summary>
        /// The game this boat is a part of
        /// </summary>
        //Game game;

        const float LINEAR_ACCELERATION = 10;
        const float ANGULAR_ACCELERATION = 2;

        public float angle;
        public float angularVelocity;

        Vector2 position;
        public Vector2 velocity;
        public Vector2 direction;

        /// <summary>
        /// The texture to apply to a boat
        /// </summary>
        Texture2D texture;
        /// <summary>
        /// color of boat
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Direction of the boat
        /// </summary>
        public Direction Direction;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 25, 200 - 75), 32, 100);

        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// The position of the boat in the game world
        /// </summary>
        public Vector2 Position { get; set; }

        private double directionTimer;

        private double animationTimer;

        private short animationFrame = 1;

        private float rotation;
        private bool turningUp;
        private bool turningDown;

        /// <summary>
        /// Constructs a new boat instance
        /// </summary>
        /// <param name="game">The game this ball belongs in</param>
        /// <param name="color">A color to distinguish this ball</param>
        public Boat()
        {
            this.position = new Vector2(375, 250);
            direction = -Vector2.UnitY;

        }
        

        /// <summary>
        /// Loads the boat's texture
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("BoatSprite");
        }
        /// <summary>
        /// Updates the bat spire to fly in a direction
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;
            gamePadState = GamePad.GetState(0);
            KeyboardState keyboardState = Keyboard.GetState();
           

            
            // Apply the gamepad movement with inverted Y axis
            Position += gamePadState.ThumbSticks.Left * new Vector2(1, -1);
            if (gamePadState.ThumbSticks.Left.X < 0) rotation = 90;
            if (gamePadState.ThumbSticks.Left.X > 0) rotation = 90;

            // Apply keyboard movement
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                Position += new Vector2(0, -1);
                turningUp = true;
                rotation = 0;
                turningDown = false;
                
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                Position += new Vector2(0, 1);
                turningUp = false;
                
                turningDown = true;
                
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-1, 0);
                turningUp = false;
                rotation = 175;
                turningDown = false;
                

            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(1, 0);
                turningUp = false;
                rotation = 95;
                turningDown = false;
                

            }
            
            //updates bounds
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 16;



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
            SpriteEffects spriteEffects = turningDown ? SpriteEffects.FlipVertically : SpriteEffects.None;
            var source = new Rectangle(animationFrame * 200, (int)Direction * 200, 200, 200);
            spriteBatch.Draw(texture, Position, source, Color, angle, new Vector2(110, 110), .7f, spriteEffects, 0);
        }


    }
}
