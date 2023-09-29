using GameProject1.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1.Screens
{
    public class VictoryScreen : GameScreen
    {
        ContentManager _content;
        Texture2D _background;

        private float restartAlpha;
        private readonly InputAction _restartAction;
        private readonly InputAction _menuAction;
        private LifePreserver lifePreserver1 = new LifePreserver(new Vector2(115, 150));
        private LifePreserver lifePreserver2 = new LifePreserver(new Vector2(600, 150));



        public VictoryScreen()
        {
            _restartAction = new InputAction(
               new[] { Buttons.Start, Buttons.Back },
               new[] { Keys.Space }, true);

            _menuAction = new InputAction(
               new[] { Buttons.Start, Buttons.Back },
               new[] { Keys.Back }, true);

            
            
        }


        public override void Activate()
        {
            base.Activate();
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            
            _background = _content.Load<Texture2D>("VictoryScreen1");
            lifePreserver1.LoadContent(_content);
            lifePreserver2.LoadContent(_content);



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
            if (_restartAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new BoatGame(), ControllingPlayer);
            }
            if (_menuAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {

                ScreenManager.AddScreen(new MainMenuScreen(), ControllingPlayer);

            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
            lifePreserver1.Draw(gameTime, ScreenManager.SpriteBatch);
            lifePreserver2.Draw(gameTime, ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
        }
    }
}
