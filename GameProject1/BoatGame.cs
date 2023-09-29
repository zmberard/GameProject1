using GameProject1.Screens;
using GameProject1.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
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

    public class BoatGame : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;
        private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;
        private InputManager inputManager;
        private Boat boat;
        private LifePreserver lifePreserver;
        private Iceberg[] iceberg;
        private Driftwood[] driftwood;
        private SpriteFont spriteFont;
        private int preserversLeft = 1;
        private SpriteFont escText;
        private SoundEffect preserverPickup;
        private SoundEffect damage;
        private SoundEffect death;

        private Song backgroundMusic;

        private Vector2 _playerPosition = new Vector2(50, 375);


        public Random rng = new Random();

        public int[] icebergXpos = new int[3] { 400, 275, 600 };
        public int[] icebergYpos = new int[3] { 200, 75, 275 };

        public int[] driftwoodXpos = new int[3] { 100, 300, 600 };
        public int[] driftwoodYpos = new int[3] { 100, 150, 300 };

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public BoatGame()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
               new[] { Buttons.Start, Buttons.Back },
               new[] { Keys.Back }, true);

        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("gamefont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);
            boat = new Boat();
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
            //_spriteBatch = new SpriteBatch(GraphicsDevice);
            boat.LoadContent(_content);
            lifePreserver.LoadContent(_content);
            foreach (Iceberg i in iceberg) i.LoadContent(_content);
            foreach (Driftwood d in driftwood) d.LoadContent(_content);
            spriteFont = _content.Load<SpriteFont>("OverlockSC");
            escText = _content.Load<SpriteFont>("Arial");
            // TODO: use this.Content to load your game content here
            backgroundMusic = _content.Load<Song>("my-heart-will-go-on-titanic-theme");
            preserverPickup = _content.Load<SoundEffect>("Pickup_Coin3");
            death = _content.Load<SoundEffect>("Explosion6");
            damage = _content.Load<SoundEffect>("Explosion14");
            MediaPlayer.Volume = 0.45f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            
            boat.Position = _playerPosition;
            inputManager = new InputManager();

            ScreenManager.Game.ResetElapsedTime();
        }
        //new Iceberg(generateSpawnPoint(2, SpriteType.iceberg));
        //new Driftwood(generateSpawnPoint(2, SpriteType.driftwood));
       /*
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
       */
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

        
        /*
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
            backgroundMusic = Content.Load<Song>("my-heart-will-go-on-titanic-theme");
            preserverPickup = Content.Load<SoundEffect>("Pickup_Coin3");
            death = Content.Load<SoundEffect>("Explosion6");
            damage = Content.Load <SoundEffect>("Explosion14");
            MediaPlayer.Volume = 0.45f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);


        }
        */

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            /*
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            inputManager.Update(gameTime);
            if (inputManager.Exit) Exit();
            */
            // TODO: Add your update logic here

            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);
            if (IsActive)
            {
                boat.Position += inputManager.Direction;

                boat.Update(gameTime);
                boat.Color = Color.White;
                //Detect and process collisions

                foreach (var i in iceberg)
                {

                    if (i.Bounds.CollidesWith(boat.Bounds) && i.wasHit == false)
                    {
                        i.isHit = true;

                        if (i.isHit == true && i.wasHit == false)
                        {
                            boat.Color = Color.Red;
                            damage.Play();
                            boat.Damage -= 30;
                            i.isHit = false;
                            i.wasHit = true;
                        }

                    }
                    else if (!i.Bounds.CollidesWith(boat.Bounds))
                    {
                        i.wasHit = false;
                    }



                }
                foreach (var d in driftwood)
                {
                    if (d.Bounds.CollidesWith(boat.Bounds) && d.wasHit == false)
                    {
                        d.isHit = true;

                        if (d.isHit == true && d.wasHit == false)
                        {
                            boat.Color = Color.Red;
                            damage.Play();
                            boat.Damage -= 10;
                            d.isHit = false;
                            d.wasHit = true;
                        }

                    }
                    else if (!d.Bounds.CollidesWith(boat.Bounds))
                    {
                        d.wasHit = false;
                    }
                }

                if (lifePreserver.Bounds.CollidesWith(boat.Bounds) && !lifePreserver.Collected)
                {
                    boat.Color = Color.Red;
                    lifePreserver.Collected = true;
                    preserversLeft--;
                    preserverPickup.Play();

                }


                
            }
        }
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 2.5f;
                boat.Position = _playerPosition;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.SteelBlue);

            var _spriteBatch = ScreenManager.SpriteBatch;

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
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }

            base.Draw(gameTime);
        }
    }
}