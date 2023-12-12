using GameProject1.BoatThings;
using GameProject1.Obstacles;
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
using SharpDX.MediaFoundation;
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
    

    public class BoatGameLevel2 : GameScreen
    {
        const float LINEAR_ACCELERATION = 10;
        const float ANGULAR_ACCELERATION = 5;
        private ContentManager _content;
        private SpriteFont _gameFont;
        private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;
        private InputManager inputManager;
        private int boatDam;

        //objects and object counters
        private Boat boat;
        private LifePreserver[] lifePreserver;
        private Iceberg[] iceberg;
        private Driftwood[] driftwood;
        private SpriteFont spriteFont;
        private int preserversLeft = 2;
        private Texture2D _background;

        //sounds
        private SpriteFont escText;
        private SoundEffect preserverPickup;
        private SoundEffect damage;
        private SoundEffect death;

        //particle effects
        private BoatParticle crashParticle;

        private Song backgroundMusic;

        private Vector2 _playerPosition = new Vector2(50, 375);


        public Random rng = new Random();

        public int[] icebergXpos = new int[4] { 400, 275, 600, 700 };
        public int[] icebergYpos = new int[4] { 200, 75, 275, 250 };

        public int[] driftwoodXpos = new int[4] { 100, 300, 600, 200 };
        public int[] driftwoodYpos = new int[4] { 100, 150, 300, 350 };

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public BoatGameLevel2(int Damage)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            boatDam = Damage;
            _pauseAction = new InputAction(
               new[] { Buttons.Start, Buttons.Back },
               new[] { Keys.Back }, true);


        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("gamefont");

            crashParticle = new BoatParticle(ScreenManager.Game, 20);
            ScreenManager.Game.Components.Add(crashParticle);

            
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);
            boat = new Boat(ScreenManager.Game);
            lifePreserver = new LifePreserver[]
            {
                new LifePreserver(new Vector2(655, 10)),
                new LifePreserver(new Vector2(655, 355))
            };
            
            iceberg = new Iceberg[]
            {
                new Iceberg(generateSpawnPoint(3, SpriteType.iceberg)),
                new Iceberg(generateSpawnPoint(3, SpriteType.iceberg)),
                new Iceberg(generateSpawnPoint(3, SpriteType.iceberg)),
                new Iceberg(generateSpawnPoint(3, SpriteType.iceberg))
            };
            driftwood = new Driftwood[]
            {
                new Driftwood(generateSpawnPoint(3, SpriteType.driftwood)),
                new Driftwood(generateSpawnPoint(3, SpriteType.driftwood)),
                new Driftwood(generateSpawnPoint(3, SpriteType.driftwood)),
                new Driftwood(generateSpawnPoint(3, SpriteType.driftwood))
            };
            //_background = ScreenManager.Game.Content.Load<Texture2D>("whale background");
            //_spriteBatch = new SpriteBatch(GraphicsDevice);
            boat.LoadContent(_content);
            boat.Damage = boatDam;
            foreach (LifePreserver l in lifePreserver) l.LoadContent(_content);
            foreach (Iceberg i in iceberg) i.LoadContent(_content);
            foreach (Driftwood d in driftwood) d.LoadContent(_content);
            spriteFont = _content.Load<SpriteFont>("OverlockSC");
            escText = _content.Load<SpriteFont>("Arial");
            // TODO: use this.Content to load your game content here
            backgroundMusic = _content.Load<Song>("OCEAN SOUND EFFECT");
            preserverPickup = _content.Load<SoundEffect>("Pickup_Coin3");
            death = _content.Load<SoundEffect>("Explosion6");
            damage = _content.Load<SoundEffect>("Explosion14");
            MediaPlayer.Volume = 0.15f;
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
            if (spriteType == 0)
            {
                X = icebergXpos[rng.Next(bound)];//returns random integers < bound
                Y = icebergYpos[rng.Next(bound)];

            }
            else
            {
                X = driftwoodXpos[rng.Next(bound)];//returns random integers < bound
                Y = driftwoodYpos[rng.Next(bound)];
            }


            return new Vector2(X, Y);
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
                //boat.Position += inputManager.Direction;

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
                            boat.Damage -= 30;
                            boat.Color = Color.Red;
                            if (boat.Damage > 0)
                            {
                                damage.Play();
                                crashParticle.PlaceFirework(new Vector2(_playerPosition.X, _playerPosition.Y - 30));
                            }
                            else
                            {
                                death.Play();
                                ScreenManager.AddScreen(new DeathScreen(), ControllingPlayer);
                                crashParticle.PlaceFirework(new Vector2(_playerPosition.X, _playerPosition.Y - 30));
                            }

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
                            boat.Damage -= 10;
                            boat.Color = Color.Red;
                            if (boat.Damage > 0)
                            {
                                damage.Play();
                                crashParticle.PlaceFirework(new Vector2(_playerPosition.X, _playerPosition.Y - 30));
                            }
                            else
                            {
                                death.Play();
                                ScreenManager.AddScreen(new DeathScreen(), ControllingPlayer);
                                crashParticle.PlaceFirework(new Vector2(_playerPosition.X, _playerPosition.Y - 30));
                            }

                            d.isHit = false;
                            d.wasHit = true;

                        }

                    }
                    else if (!d.Bounds.CollidesWith(boat.Bounds))
                    {
                        d.wasHit = false;
                    }
                }

                foreach(LifePreserver l in lifePreserver)
                {
                    if (l.Bounds.CollidesWith(boat.Bounds) && !l.Collected)
                    {
                        boat.Color = Color.Red;
                        l.Collected = true;
                        preserversLeft--;
                        preserverPickup.Play();
                        //ScreenManager.AddScreen(new Cutscene(), ControllingPlayer);

                    }
                }



                if(preserversLeft == 0)
                {
                    ScreenManager.AddScreen(new SecondSplashScreen(boat.Damage), ControllingPlayer);
                }

                _playerPosition.X = MathHelper.Clamp(_playerPosition.X, 0, ScreenManager.Game.GraphicsDevice.Viewport.Width - (boat.texture.Width / 4) + 200);
                _playerPosition.Y = MathHelper.Clamp(_playerPosition.Y, 0, ScreenManager.Game.GraphicsDevice.Viewport.Height - boat.texture.Height + 145);

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

                keyboardState = Keyboard.GetState();
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;




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
        /// <summary>
        /// Draws all components for my BoatGame
        /// </summary>
        /// <param name="gameTime">the in game time</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.SteelBlue);

            var _spriteBatch = ScreenManager.SpriteBatch;

            //float playerY = MathHelper.Clamp(_playerPosition.Y, 100, 3200);
            float offsetY = 100 - _playerPosition.Y;

            //requirement 1 met: sprite transform
            Matrix transform;



            transform = Matrix.CreateTranslation(0, offsetY - 3000, 0);
            _spriteBatch.Begin(transformMatrix: transform);
            //_spriteBatch.Draw(_background, Vector2.Zero , Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin();
            foreach(LifePreserver l in lifePreserver) l.Draw(gameTime, _spriteBatch);

            foreach (Iceberg i in iceberg) i.Draw(gameTime, _spriteBatch);
            foreach (Driftwood d in driftwood) d.Draw(gameTime, _spriteBatch);

            boat.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(spriteFont, $"Preservers Left: {preserversLeft}", new Vector2(2, 2), Color.Gold);
            _spriteBatch.DrawString(spriteFont, $"Damage: {boat.Damage}", new Vector2(2, 29), Color.Gold);
            //_spriteBatch.DrawString(escText, "press esc. to exit", new Vector2(600, 440), Color.White);
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