using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using System.Transactions;
using Color = Microsoft.Xna.Framework.Color;

namespace GameProject1
{
    public class BoatGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private InputManager inputManager;
        private Boat boat;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public BoatGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            boat = new Boat(this);
            boat.Position = new Vector2(282, 245);
            inputManager = new InputManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            boat.LoadContent(Content);
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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SteelBlue);
            _spriteBatch.Begin();
            boat.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}