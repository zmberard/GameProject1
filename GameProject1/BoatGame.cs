using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Transactions;
using Color = Microsoft.Xna.Framework.Color;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace GameProject1
{
    public enum SpriteType
    {
        iceberg,
        driftwood
    }

    public class BoatGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private InputManager inputManager;
        private Boat boat;
        private LifePreserver lifePreserver;
        private Iceberg[] iceberg;
        private Driftwood[] driftwood;
        private SpriteFont spriteFont;
        private int preserversLeft = 1;
        private SpriteFont escText; 
        



        public Random rng = new Random();

        public int[] icebergXpos = new int[3] { 400, 275, 600 };
        public int[] icebergYpos = new int[3] { 200, 75, 275 };

        public int[] driftwoodXpos = new int[3] { 100, 300, 600 };
        public int[] driftwoodYpos = new int[3] { 100, 150, 300 };

        /// <summary>
        /// Constructs the game
        /// </summary>
        public BoatGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

        }
        //new Iceberg(generateSpawnPoint(2, SpriteType.iceberg));
        //new Driftwood(generateSpawnPoint(2, SpriteType.driftwood));
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            boat = new Boat(this);
            boat.Position = new Vector2(50, 375);
            inputManager = new InputManager();
            lifePreserver = new LifePreserver(new Vector2(655, 10));
            iceberg = new Iceberg[]
            {
                new Iceberg(generateSpawnPoint(3, SpriteType.iceberg)),
                new Iceberg(generateSpawnPoint(3, SpriteType.iceberg)),
                new Iceberg(generateSpawnPoint(3, SpriteType.iceberg))
            };

            driftwood = new Driftwood[]
            {
                new Driftwood(generateSpawnPoint(3, SpriteType.driftwood)),
                new Driftwood(generateSpawnPoint(3, SpriteType.driftwood)),
                new Driftwood(generateSpawnPoint(3, SpriteType.driftwood))
            };
            base.Initialize();
        }
        public Vector2 generateSpawnPoint(int bound, SpriteType spriteType)
        {
            int X = 0;
            int Y = 0;
            if(spriteType == 0)
            {
                X = icebergXpos[rng.Next(bound)];//returns random integers < bound
                Y = icebergYpos[rng.Next(bound)];
                
            }
            else
            {
                X = driftwoodXpos[rng.Next(bound)];//returns random integers < bound
                Y = driftwoodYpos[rng.Next(bound)];
            }
            
            
            return new Vector2(X,Y);
        }

        

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            boat.LoadContent(Content);
            lifePreserver.LoadContent(Content); 
            foreach (Iceberg i in iceberg) i.LoadContent(Content);
            foreach (Driftwood d in driftwood) d.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("OverlockSC");
            escText = Content.Load<SpriteFont>("Arial");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            inputManager.Update(gameTime);
            if (inputManager.Exit) Exit();
            // TODO: Add your update logic here

            boat.Position += inputManager.Direction;
            
            boat.Update(gameTime);
            boat.Color = Color.White;
            //Detect and process collisions

            foreach (var i in iceberg)
            {
                if (i.Bounds.CollidesWith(boat.Bounds))
                {
                    boat.Color = Color.Red;
                    boat.Damage -= 1;
                }

            }
            foreach(var d in driftwood)
            {
                if (d.Bounds.CollidesWith(boat.Bounds))
                {
                    boat.Color = Color.Red;
                    boat.Damage -= 1;
                }
            }

            if (lifePreserver.Bounds.CollidesWith(boat.Bounds) && !lifePreserver.Collected)
             {
                 boat.Color = Color.Red;
                 lifePreserver.Collected = true;
                 preserversLeft--;
                 
             }

            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SteelBlue);
            _spriteBatch.Begin();
            lifePreserver.Draw(gameTime, _spriteBatch);
            
            foreach (Iceberg i in iceberg) i.Draw(gameTime, _spriteBatch);
            foreach(Driftwood d in driftwood) d.Draw(gameTime, _spriteBatch);

            boat.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(spriteFont, $"Preservers Left: {preserversLeft}", new Vector2(2, 2), Color.Gold);
            _spriteBatch.DrawString(spriteFont, $"Damage: {boat.Damage}", new Vector2(2, 29), Color.Gold);
            _spriteBatch.DrawString(escText, "press esc. to exit", new Vector2(600, 440), Color.White);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}