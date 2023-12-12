using GameProject1.BoatThings;
using GameProject1.Obstacles;
using GameProject1.Screens;
using GameProject1.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing.Text;
using SharpDX.Direct2D1;

namespace GameProject1
{
    public class BoatGameLevel3 : GameScreen
    {
        const float LINEAR_ACCELERATION = 10;
        const float ANGULAR_ACCELERATION = 5;
        private ContentManager _content;
        private SpriteFont _gameFont;
        private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;
        private InputManager inputManager;

        //objects and object counters
        private Boat boat;
        private LifePreserver lifePreserver;
        private List<Tentacle> Cycle1 = new List<Tentacle>();
        private List<Tentacle> Cycle2 = new List<Tentacle>();
        //private Tentacle[] Cycle2;
        private Driftwood[] driftwood;
        private SpriteFont spriteFont;
        private int preserversLeft = 1;
        private Texture2D _background;
        private Kraken Kraken;
        private int boatDam;

        private int[] C1xlist = { 10, 150, 450, 525 };
        private int[] C1ylist = { 175, 200, 150, 30 };

        private int[] C2xlist = { 160, 275, 570, 685 };
        private int[] C2ylist = { 40, 100, 215, 215 };

        //sounds
        private SpriteFont escText;
        private SoundEffect preserverPickup;
        private SoundEffect damage;
        private SoundEffect death;

        private double spawnTimer1;
        private double spawnTimer2;
        //particle effects
        private BoatParticle crashParticle;

        private Song backgroundMusic;

        private Vector2 _playerPosition = new Vector2(50, 375);


        public Random rng = new Random();

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public BoatGameLevel3(int Damage)
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


            lifePreserver = new LifePreserver(new Vector2(655, 355));
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);
            boat = new Boat(ScreenManager.Game);
            Kraken = new Kraken(new Vector2(230, 260));
            boat.Position = new Vector2(25, 25);
            boat.Damage = boatDam;
            boat.LoadContent(_content);
            Tentacle t = new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 1), _content);
            Cycle1.Add(t);
            Cycle1.Add(new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 1), _content));
            Cycle1.Add(new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 1), _content));
            Cycle1.Add(new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 1), _content));


            Cycle2.Add(new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 2), _content));
            Cycle2.Add(new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 2), _content));
            Cycle2.Add(new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 2), _content));
            Cycle2.Add(new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 2), _content));


            //_background = ScreenManager.Game.Content.Load<Texture2D>("whale background");
            //_spriteBatch = new SpriteBatch(GraphicsDevice);
            lifePreserver.LoadContent(_content);
            Kraken.LoadContent(_content);
            foreach (Tentacle i in Cycle1) i.LoadContent(_content);
            foreach (Tentacle i in Cycle2) i.LoadContent(_content);
            //foreach (Driftwood d in driftwood) d.LoadContent(_content);
            spriteFont = _content.Load<SpriteFont>("OverlockSC");
            escText = _content.Load<SpriteFont>("Arial");
            // TODO: use this.Content to load your game content here
            backgroundMusic = _content.Load<Song>("Pirates");
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
        public Vector2 generateSpawnPoint(int bound, SpriteType spriteType, int cycle)
        {
            int X = 0;
            int Y = 0;
            
            if (spriteType == SpriteType.tentacle && cycle == 1)
            {
                X = C1xlist[Cycle1.Count];//returns random integers < bound
                Y = C1ylist[Cycle1.Count];

            }
            else if(spriteType == SpriteType.tentacle && cycle == 2)
            {
                X = C2xlist[Cycle2.Count];//returns random integers < bound
                Y = C2ylist[Cycle2.Count];
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
        private void SpawnObjectCycle1()
        {
            // Create and spawn your object here
            Tentacle obj = new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 1), _content);
            Cycle1.Add(obj);

        }

        private void SpawnObjectCycle2()
        {
            // Create and spawn your object here
            Tentacle obj = new Tentacle(generateSpawnPoint(3, SpriteType.tentacle, 2), _content);
            Cycle2.Add(obj);

        }

        private void DeSpawnObjects()
        {
            Cycle1.Clear();
        }

        private void DeSpawnObjects2()
        {
            Cycle2.Clear();
        }


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
            spawnTimer1 += gameTime.ElapsedGameTime.TotalSeconds;
            
            spawnTimer2 += gameTime.ElapsedGameTime.TotalSeconds;

            if (spawnTimer1 >= 20)
            {
                DeSpawnObjects();
                SpawnObjectCycle1();
                SpawnObjectCycle1();
                SpawnObjectCycle1();
                SpawnObjectCycle1();
                spawnTimer1 = 0;  // Reset the timer
            }
            
            if(spawnTimer2 >= 15)
            {
                DeSpawnObjects2();
                SpawnObjectCycle2();
                SpawnObjectCycle2();
                SpawnObjectCycle2();
                SpawnObjectCycle2();
                spawnTimer2 = 0;
            }


            foreach (Tentacle t in Cycle1) t.Update(gameTime);
            foreach (Tentacle t in Cycle2) t.Update(gameTime);
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

                foreach (var i in Cycle1)
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
                foreach (var d in Cycle2)
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
                
                
                if (lifePreserver.Bounds.CollidesWith(boat.Bounds) && !lifePreserver.Collected)
                {
                    boat.Color = Color.Red;
                    lifePreserver.Collected = true;
                    preserversLeft--;
                    preserverPickup.Play();
                    ScreenManager.AddScreen(new VictoryScreen(), ControllingPlayer);

                }

                if (Kraken.Bounds.CollidesWith(boat.Bounds) && Kraken.wasHit == false)
                {
                        Kraken.isHit = true;

                        if (Kraken.isHit == true && Kraken.wasHit == false)
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

                            Kraken.isHit = false;
                            Kraken.wasHit = true;

                        }

                }
                else if (!Kraken.Bounds.CollidesWith(boat.Bounds))
                {
                    Kraken.wasHit = false;
                }





                if (preserversLeft == 0)
                {
                    ScreenManager.AddScreen(new VictoryScreen(), ControllingPlayer);
                }

                _playerPosition.X = MathHelper.Clamp(_playerPosition.X, 0, ScreenManager.Game.GraphicsDevice.Viewport.Width - (boat.texture.Width/4) + 200);
                _playerPosition.Y = MathHelper.Clamp(_playerPosition.Y, 0, ScreenManager.Game.GraphicsDevice.Viewport.Height - boat.texture.Height+145);

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
            lifePreserver.Draw(gameTime, _spriteBatch);
            Kraken.Draw(gameTime, _spriteBatch);
            foreach (Tentacle i in Cycle1) i.Draw(gameTime, _spriteBatch);
            foreach (Tentacle d in Cycle2) d.Draw(gameTime, _spriteBatch);
            
                //foreach (Tentacle i in Cycle2) i.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(spriteFont, $"Preservers Left: {preserversLeft}", new Vector2(2, 2), Color.Gold);
                
            
            
                //foreach (Tentacle d in Cycle2) d.Draw(gameTime, _spriteBatch);
                //spawnTimer2 = 0;
            
            

            boat.Draw(gameTime, _spriteBatch);
            
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
