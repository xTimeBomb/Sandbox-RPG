using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SandboxRPG
{
     public class Game1 : Microsoft.Xna.Framework.Game
     {
          public static int WIDTH = 1280;
          public static int HEIGHT = 720;

          GraphicsDeviceManager graphics;
          SpriteBatch spriteBatch;

          public Game1()
          {
               graphics = new GraphicsDeviceManager(this);
               Content.RootDirectory = "Content";
               IsMouseVisible = true;

               graphics.PreferredBackBufferWidth = WIDTH;
               graphics.PreferredBackBufferHeight = HEIGHT;
          }

          protected override void Initialize()
          {
               Components.Add(new Inventory(this));

               base.Initialize();
          }

          protected override void LoadContent()
          {
               spriteBatch = new SpriteBatch(GraphicsDevice);
          }

          protected override void UnloadContent()
          {
          }

          protected override void Update(GameTime gameTime)
          {
               if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();

               base.Update(gameTime);
          }

          protected override void Draw(GameTime gameTime)
          {
               GraphicsDevice.Clear(Color.CornflowerBlue);

               base.Draw(gameTime);
          }
     }
}