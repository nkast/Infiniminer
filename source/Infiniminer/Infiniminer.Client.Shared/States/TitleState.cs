/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2009 Zach Barth
Copyright (c) 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using StateMasher;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Infiniminer.States
{
    public class TitleState : State
    {
        SpriteBatch spriteBatch;
        Texture2D texMenu;
        Rectangle drawRect;
        string nextState = null;

        public override void OnEnter(string oldState)
        {
            _SM.IsMouseVisible = true;

            spriteBatch = new SpriteBatch(_SM.GraphicsDevice);
            texMenu = _SM.Content.Load<Texture2D>("menus/tex_menu_title");

            UpdateUIViewport(_SM.GraphicsDevice.Viewport);
        }

        private void UpdateUIViewport(Viewport viewport)
        {
            drawRect = new Rectangle(viewport.Width / 2 - 1024 / 2,
                                     viewport.Height / 2 - 768 / 2,
                                     1024,
                                     1024);
        }

        public override string OnUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            _P.inputEngine.Update(gameTime);

            if(_P.inputEngine.MenuConfirm.Pressed())
            {
                _P.inputEngine.MenuConfirm.ConsumePress();
                nextState = "Infiniminer.States.ServerBrowserState";
                _P.PlaySound(InfiniminerSound.ClickHigh);
            }
            else if(_P.inputEngine.MenuBack.Pressed())
            {
                _SM.Exit();
            }
            
            return nextState;
        }

        public override void OnRenderAtUpdate(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            UpdateUIViewport(graphicsDevice.Viewport);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
            spriteBatch.Draw(texMenu, drawRect, Color.White);
            spriteBatch.End();
        }
    }
}
