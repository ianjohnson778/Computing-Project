using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Squared.Tiled;
using System.Collections.Generic;

namespace Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int bufferWidth = 1280;
        int bufferHight = 720;

        Map map;
        Layer collision;
        Vector2 viewportPosition;
        int tilepixel;

        Texture2D cartexture;
        Car car;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = bufferWidth;
            graphics.PreferredBackBufferHeight = bufferHight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            map = Map.Load(Path.Combine(Content.RootDirectory, "racingMap.tmx"), Content);
            collision = map.Layers["A_boundry"];
            tilepixel = map.TileWidth;
            //viewportPosition = new Vector2(map.ObjectGroups["C_Objects"].Objects["Player"].X - (bufferWidth/2), map.ObjectGroups["C_Objects"].Objects["Player"].Y - (bufferHight/2));

            cartexture = Content.Load<Texture2D>("Car");
            Vector2 carPos = new Vector2((graphics.PreferredBackBufferWidth / 2) - (cartexture.Width / 2), (graphics.PreferredBackBufferHeight/2)-(cartexture.Height/2));
            car = new Car();
            
            car.Position = new Vector2(map.ObjectGroups["C_Objects"].Objects["Player"].X - (bufferWidth / 2), map.ObjectGroups["C_Objects"].Objects["Player"].Y - (bufferHight / 2));

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            car.Init(cartexture, Content.Load<Texture2D>("marker"), carPos);

            // TODO: use this.Content to load your game content here
        }

        public bool CheckBounds()
        {
            bool check = false;
            List<Vector2> corners = car.Corners;

            foreach(Vector2 cr in corners)
            {
                Vector2 corner = cr + car.Position;
                Rectangle cornerrec = new Rectangle((int)corner.X, (int)corner.Y, 0, 0);

                int i = (int)corner.X / 128;
                int j = (int)corner.Y / 128;

                if (i > 0 && i < 30 && j > 0 && j < 15)
                {
                    if (collision.GetTile(i, j) != 0)
                    {
                        check = true;
                    }
                }
                else check = true;
            }

            return check;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Vector2 carPos = car.Position;

            if (CheckBounds())
            {
                car.Position = carPos;
            }

            car.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();
            map.Draw(spriteBatch, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), car.Position);
            car.Draw(spriteBatch);
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
