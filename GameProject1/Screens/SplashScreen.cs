﻿using GameProject1.StateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;

namespace GameProject1.Screens
{
    public class SplashScreen : GameScreen
    {
        ContentManager _content;
        Texture2D _background;
        TimeSpan _displayTime;
        TimeSpan _textDisplayTime;

        private TimeSpan _initialDisplayTime = TimeSpan.FromSeconds(8); // Set the initial display time to 5 seconds
        private TimeSpan _whiteTextDisplayTime = TimeSpan.FromSeconds(8);

        private SpriteFont _gameFont1;
        private SpriteFont _gameFont2;
        private SpriteFont _gameFont3;

        private Song _morseCodeSoundEffect;

        public override void Activate()
        {
            base.Activate();
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _morseCodeSoundEffect = _content.Load<Song>("Morse Code Sound Effect");
            _displayTime = TimeSpan.FromSeconds(60);
            _textDisplayTime = TimeSpan.FromSeconds(20);
            MediaPlayer.Stop();
            MediaPlayer.Volume = .45f;
            MediaPlayer.Play(_morseCodeSoundEffect);

            _gameFont1 = _content.Load<SpriteFont>("OverlockSC");

        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _displayTime -= gameTime.ElapsedGameTime;
            _textDisplayTime -= gameTime.ElapsedGameTime;

            if (_whiteTextDisplayTime <= TimeSpan.Zero) 
            {
                ExitScreen();
                LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new BoatGame());
            } 

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            var _spriteBatch = ScreenManager.SpriteBatch;
            //ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.Black);
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            //_spriteBatch.DrawString(_gameFont1, "While anchored in the Atlanic ocean", new Vector2(150, 200), Color.WhiteSmoke);
            //_spriteBatch.DrawString(_gameFont1, "you hear a garbled distress signal...", new Vector2(150, 240), Color.WhiteSmoke);

            _textDisplayTime -= gameTime.ElapsedGameTime; // Update the elapsed time
            if (_initialDisplayTime > TimeSpan.Zero)
            {
                ScreenManager.GraphicsDevice.Clear(Color.Black);
                _spriteBatch.DrawString(_gameFont1, "While anchored in the Atlanic ocean", new Vector2(150, 200), Color.WhiteSmoke);
                _spriteBatch.DrawString(_gameFont1, "you hear a garbled distress signal...", new Vector2(150, 240), Color.WhiteSmoke);

                _initialDisplayTime -= gameTime.ElapsedGameTime; // Update the elapsed time for initial display

                if (_initialDisplayTime <= TimeSpan.Zero)
                {
                    _initialDisplayTime = TimeSpan.Zero; // Ensure it doesn't go negative
                }
            }
            else if (_whiteTextDisplayTime > TimeSpan.Zero)
            {
                // Draw the white text
                _spriteBatch.DrawString(_gameFont1, "ship sinking rapidly.....rogue wave", new Vector2(150, 200), Color.White);
                _spriteBatch.DrawString(_gameFont1, "massive shad.....SOS....3 men aboard..", new Vector2(150, 240), Color.White);

                _whiteTextDisplayTime -= gameTime.ElapsedGameTime; // Update the elapsed time for white text display

                if (_whiteTextDisplayTime <= TimeSpan.Zero)
                {
                    _whiteTextDisplayTime = TimeSpan.Zero; // Ensure it doesn't go negative

                    // Reset the white text to transparent
                    _spriteBatch.DrawString(_gameFont1, "ship sinking rapidly.....rogue wave", new Vector2(150, 200), Color.Transparent);
                    _spriteBatch.DrawString(_gameFont1, "massive shad.....SOS....3 men aboard..", new Vector2(150, 240), Color.Transparent);
                }
            }
            

            ScreenManager.SpriteBatch.End();
        }
    }
}
