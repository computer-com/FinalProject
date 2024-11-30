using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Player
    {
        public Vector2 Position { get; private set; }
        public float Speed { get; set; }
        private int windowWidth;
        private int windowHeight;
        private int radius;
        public int Radius { get; private set; }
        private Vector2 initialPosition;


        public Player(Vector2 initialPosition, float speed, int windowWidth, int windowHeight, int radius)
        {
            this.initialPosition = initialPosition;
            Position = initialPosition;
            Speed = speed;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.Radius = radius;
        }

        public void Update(GameTime gameTime, List<Rectangle> walls)
        {
            KeyboardState state = Keyboard.GetState();
            Vector2 newPosition = Position;
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                newPosition.X -= Speed;
                if (newPosition.X < 0)
                {
                    newPosition.X = 0;
                }
            }
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                newPosition.X += Speed;
                if (newPosition.X > windowWidth - radius * 2)
                {
                    newPosition.X = windowWidth - radius * 2;
                }
            }
            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                newPosition.Y -= Speed;
                if (newPosition.Y < 0)
                {
                    newPosition.Y = 0;
                }
            }
            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                newPosition.Y += Speed;
                if (newPosition.Y > windowHeight - radius * 2)
                {
                    newPosition.Y = windowHeight - radius * 2;
                }

            }


            Rectangle playerRectangle = new Rectangle((int)newPosition.X, (int)newPosition.Y, 50, 50);
            bool collidesWithWall = false;

            foreach (var wall in walls)
            {
                if (playerRectangle.Intersects(wall))
                {
                    collidesWithWall = true;
                    break;
                }
            }


            if (!collidesWithWall)
            {
                Position = newPosition;
            }
        }

        public void ResetPosition()
        {
            Position = initialPosition;
        }
    }
}
