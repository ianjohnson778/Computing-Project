using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Car
    {
        Texture2D texture;

        Texture2D marker;

        Vector2 position;
        Vector2 origin;
        float steer = 0;

        float speed = 0;

        List<Vector2> corners = new List<Vector2>();
        Vector2 center;

        public List<Vector2> Corners
        {
            get { return corners; }
        }

        public void Init(Texture2D t, Texture2D m, Vector2 pos)
        {
            position = pos;
            texture = t;
            marker = m;
            origin = new Vector2(t.Width / 2, t.Height / 2);

            center = position + new Vector2(t.Width - 2,t.Height - 2);
        }

        public void GetInputs(KeyboardState input)
        {
            if (input.IsKeyDown(Keys.W) && speed < 10)
            {
                speed += 2;
            }

            if (input.IsKeyDown(Keys.S) && speed > -7)
            {
                speed -= 1;
            }

            int swap = 1;
            if (speed < 0)
                swap = -1;

            if (input.IsKeyDown(Keys.A))
            {
                steer -= (.05f * swap);
            }

            if (input.IsKeyDown(Keys.D))
            {
                steer += (.05f * swap);
            }

            if (speed > 0)
                speed -= 1;
            else if (speed < 0)
                speed += .5f;
            else
                speed = 0;
        }

        public void GetInput(GamePadState input)
        {
            if (input.IsButtonDown(Buttons.A))
            {
                speed += 2;
            }
        }

        public void Update(GameTime gameTime)
        {
            GetInputs(Keyboard.GetState());
            GetInput(GamePad.GetState(0));

            position = position + new Vector2(speed * (float)Math.Cos(steer), speed * (float)Math.Sin(steer));

            corners.Clear();

            float cx = center.X;
            float cy = center.Y;

            List<Vector2> coordinates = new List<Vector2>();
            coordinates.Add(new Vector2((texture.Width / 2), (texture.Height / 2)));
            coordinates.Add(new Vector2(-(texture.Width / 2), -(texture.Height / 2)));
            coordinates.Add(new Vector2((texture.Width / 2), -(texture.Height / 2)));
            coordinates.Add(new Vector2(-(texture.Width / 2), (texture.Height / 2)));

            foreach (Vector2 point in coordinates)
            {
                float x = cx - point.X;
                float y = cy + point.Y;

                float tempX = x - cx;
                float tempY = y - cy;

                float rotatedX = (float)(tempX * Math.Cos(steer) - tempY * Math.Sin(steer));
                float rotatedY = (float)(tempX * Math.Sin(steer) + tempY * Math.Cos(steer));

                x = rotatedX + cx;
                y = rotatedY + cy;
                corners.Add(new Vector2((int)x, (int)y));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, origin+new Vector2(640,360), null, null, origin, steer, null, null, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.Draw(marker, center, Color.Purple);
            foreach (Vector2 corner in corners)
                spriteBatch.Draw(marker, corner, Color.Red);

        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
