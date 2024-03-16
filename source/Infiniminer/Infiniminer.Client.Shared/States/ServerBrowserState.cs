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

using System;
using System.Collections.Generic;
using System.Net;
using StateMasher;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Infiniminer.States
{
    public class ServerBrowserState : State
    {
        SpriteBatch spriteBatch;
        BasicEffect uiEffect;
        Texture2D texMenu;
        Rectangle drawRect;
        string nextState = null;
        List<ServerInformation> serverList = new List<ServerInformation>();
        List<int> descWidths;
        SpriteFont uiFont;
        bool directConnectIPEnter = false;
        string directConnectIP = "";
        KeyMap keyMap;

        ClickRegion[] clkMenuServer = new ClickRegion[2] {
            new ClickRegion(new Rectangle(763,713,243,42), "refresh"),
            new ClickRegion(new Rectangle(0,713,425,42), "direct")
        };

        public override void OnEnter(string oldState)
        {
            _SM.IsMouseVisible = true;
            (_SM as InfiniminerGame).ResetPropertyBag();
            _P = _SM.propertyBag;

            spriteBatch = new SpriteBatch(_SM.GraphicsDevice);
            uiEffect = new BasicEffect(_SM.GraphicsDevice);
            uiEffect.TextureEnabled = true;
            uiEffect.VertexColorEnabled = true;
            texMenu = _SM.Content.Load<Texture2D>("menus/tex_menu_server");
            UpdateUIViewport(_SM.GraphicsDevice.Viewport);

            uiFont = _SM.Content.Load<SpriteFont>("font_04b08");
            keyMap = new KeyMap();

            serverList = (_SM as InfiniminerGame).EnumerateServers(0.5f);
        }

        const int VWidth = 1024;
        const int VHeight = 768;
        const float VAspect = (float)VWidth / (float)VHeight;
        private void UpdateUIViewport(Viewport viewport)
        {
            // calculate virtual resolution
            float aspect = viewport.AspectRatio;
            float vWidth = (aspect > VAspect) ? (VHeight * aspect) : VWidth;
            float vHeight = (aspect < VAspect) ? (VWidth / aspect) : VHeight;

            drawRect = new Rectangle((int)vWidth / 2 - VWidth / 2,
                                     (int)vHeight / 2 - VHeight / 2,
                                     1024,
                                     1024);

            Matrix world = Matrix.CreateScale(1f, -1f, -1f) // Flip Y and Depth
                         * Matrix.CreateTranslation(-vWidth / 2f, vHeight / 2f, 0f) // offset center
                         * Matrix.CreateScale(1f / vWidth, 1f / vWidth, 1f); // normalize scale

            if (_SM.propertyBag.playerCamera.UseVrCamera)
            {
                float uiScale = 1f; // scale UI 1meter across.
                world *= Matrix.CreateScale(uiScale, uiScale, 1f);

                // position UI panel
                world *= Matrix.CreateTranslation(0.0f, 0.1f, -1.0f);

                uiEffect.World = world;
                uiEffect.View = _SM.propertyBag.playerCamera.ViewMatrix;
                uiEffect.Projection = _SM.propertyBag.playerCamera.ProjectionMatrix;
            }
            else
            {
                float fov = MathHelper.ToRadians(70);
                float uiScale = ((float)Math.Tan(fov * 0.5)) * aspect * 2f; // scale to fit nearPlane
                world *= Matrix.CreateScale(uiScale, uiScale, 1f);

                world *= Matrix.CreateTranslation(0.0f, 0.0f, -1.0f); // position to near plane

                uiEffect.World = world;
                uiEffect.View = Matrix.Identity;
                uiEffect.Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspect, 1f, 1000.0f);
            }
        }

        public override void OnLeave(string newState)
        {

        }

        public override string OnUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            return nextState;
        }

        public override void OnRenderAtEnter(GraphicsDevice graphicsDevice)
        {

        }

        public override void OnRenderAtUpdate(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            UpdateUIViewport(graphicsDevice.Viewport);

            descWidths = new List<int>();
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred, effect: uiEffect);
            spriteBatch.Draw(texMenu, drawRect, Color.White);

            int drawY = 80;
            foreach (ServerInformation server in serverList)
            {
                if (drawY < 660)
                {
                    int textWidth = (int)(uiFont.MeasureString(server.GetServerDesc()).X);
                    descWidths.Add(textWidth + 30);
                    spriteBatch.DrawString(uiFont, server.GetServerDesc(), new Vector2(drawRect.X + VWidth / 2 - textWidth / 2, drawRect.Y + drawY), Color.White);
                    drawY += 25;
                }
            }

            spriteBatch.DrawString(uiFont, Defines.INFINIMINER_VERSION, new Vector2(10, drawRect.Y * 2 + VHeight - 20), Color.White);

            if (directConnectIPEnter)
                spriteBatch.DrawString(uiFont, "ENTER IP: " + directConnectIP, new Vector2(drawRect.X + 30, drawRect.Y + 690), Color.White);

            spriteBatch.End();
        }

        public override void OnKeyDown(Keys key)
        {
            if (directConnectIPEnter)
            {
                if (key == Keys.Escape)
                {
                    directConnectIPEnter = false;
                    directConnectIP = "";
                }

                if (key == Keys.Back && directConnectIP.Length > 0)
                {
                    directConnectIP = directConnectIP.Substring(0, directConnectIP.Length - 1);
                }

                if (key == Keys.Enter)
                {
                    // Try what was entered first as an IP, and then second as a host name.
                    directConnectIPEnter = false;
                    _P.PlaySound(InfiniminerSound.ClickHigh);
                    IPAddress connectIp = null;
                    if (!IPAddress.TryParse(directConnectIP, out connectIp))
                    {
                        connectIp = null;
                        try
                        {
                            IPAddress[] resolveResults = Dns.GetHostAddresses(directConnectIP);
                            for (int i = 0; i < resolveResults.Length; i++)
                                if (resolveResults[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    connectIp = resolveResults[i];
                                    break;
                                }
                        }
                        catch (Exception)
                        {
                            // So, GetHostAddresses() might fail, but we don't really care. Just leave connectIp as null.
                        }
                    }
                    if (connectIp != null)
                    {
                        (_SM as InfiniminerGame).JoinGame(new IPEndPoint(connectIp, 5565));
                        nextState = "Infiniminer.States.LoadingState";
                    }
                    directConnectIP = "";
                }

                if (key == Keys.V && (Keyboard.GetState().IsKeyDown(Keys.LeftControl) || Keyboard.GetState().IsKeyDown(Keys.RightControl)))
                {
                    try
                    {
                        // [MG_PORT_NOTES] System.Windows.Forms is Windows Only
                        // directConnectIP += System.Windows.Forms.Clipboard.GetText();
                    }
                    catch (Exception e)
                    {
                    }
                }
                else if (keyMap.IsKeyMapped(key))
                {
                    directConnectIP += keyMap.TranslateKey(key, false);
                }
            }
            else
            {
                if (key == Keys.Escape)
                {
                    nextState = "Infiniminer.States.TitleState";
                }
            }
        }

        public override void OnKeyUp(Keys key)
        {

        }

        public override void OnMouseDown(MouseButton button, int x, int y)
        {
            ScreenToUI(uiEffect, ref x, ref y);
            x -= drawRect.X;
            y -= drawRect.Y;

            if (directConnectIPEnter == false)
            {
                int serverIndex = (y - 75) / 25;
                if (serverIndex >= 0 && serverIndex < serverList.Count)
                {
                    int distanceFromCenter = Math.Abs(VWidth / 2 - x);
                    if (distanceFromCenter < descWidths[serverIndex] / 2)
                    {
                        (_SM as InfiniminerGame).JoinGame(serverList[serverIndex].ipEndPoint);
                        nextState = "Infiniminer.States.LoadingState";
                        _P.PlaySound(InfiniminerSound.ClickHigh);
                    }
                }

                switch (ClickRegion.HitTest(clkMenuServer, new Point(x, y)))
                {
                    case "refresh":
                        _P.PlaySound(InfiniminerSound.ClickHigh);
                        serverList = (_SM as InfiniminerGame).EnumerateServers(0.5f);
                        break;

                    case "direct":
                        directConnectIPEnter = true;
                        _P.PlaySound(InfiniminerSound.ClickHigh);
                        break;
                }
            }
        }

        public override void OnMouseUp(MouseButton button, int x, int y)
        {
            ScreenToUI(uiEffect, ref x, ref y);

        }

        public override void OnMouseScroll(int scrollDelta)
        {

        }

        // convert mouse screen position to UI world position
        private void ScreenToUI(IEffectMatrices matrices, ref int x, ref int y)
        {
            Viewport vp = _SM.GraphicsDevice.Viewport;

            Vector3 position3 = vp.Unproject(
                            new Vector3(x, y, 0),
                            matrices.Projection,
                            matrices.View,
                            matrices.World);

            x = (int)position3.X;
            y = (int)position3.Y;
        }
    }
}
