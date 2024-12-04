using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Enemy
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }

        private int windowWidth;
        private int windowHeight;
        private int radius;
        private Random random;
        private Vector2 direction;
        private Vector2 goal;
        private double changeDirectionTime;
        private double ElaspedTime;
        private double goalReachedTime;
        private bool isSeraching;
        private Vector2 previousPosition;
        private double stuckTime;
        private const double maxstuckTime = 3.0;
        private List<Vector2> directions;

        public Enemy(Texture2D texture, Vector2 startPosition, float speed, int windowWidth, int windowHeight)
        {
            Texture = texture;
            Position = startPosition;
            Speed = speed;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.radius = texture.Width / 2;
            this.random = new Random();
            direction = Vector2.Zero;
            goal = Vector2.Zero;
            changeDirectionTime = 5.0;
            ElaspedTime = 0;
            goalReachedTime = 10.0;
            isSeraching = false;
            previousPosition = Position;
            stuckTime = 0;
            directions = new List<Vector2>
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, 1),
                new Vector2(0, -1)
            };
        }

        private void setRandomDirection()
        {
            direction = directions[random.Next(directions.Count)];
        }
        private void SetRandomGoal()
        {
            goal = new Vector2(
                    random.Next(radius, windowWidth - radius),
                    random.Next(radius, windowHeight - radius)
            );
            direction = Vector2.Normalize(goal - Position);
            isSeraching = true;
        }

        public void Update(GameTime gameTime, List<Rectangle> wallRectangles)
        {
            ElaspedTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (isSeraching)
            {
                if (Vector2.Distance(Position, goal) < Speed)
                {
                    isSeraching = false;
                    ElaspedTime = 0;
                }
            }
            else
            {
                if (ElaspedTime >= changeDirectionTime)
                {
                    setRandomDirection();
                    ElaspedTime = 0;

                }
                if (ElaspedTime >= goalReachedTime)
                {
                    SetRandomGoal();
                    ElaspedTime = 0;
                }
            }

            Vector2 newPosition = Position + direction * Speed;
            foreach (var wall in wallRectangles)
            {
                if (new Rectangle(newPosition.ToPoint(), new Point(Texture.Width, Texture.Height)).Intersects(wall))
                {
                    setRandomDirection();
                    return;
                }

            }
            if (newPosition.X < 0 || newPosition.X > windowWidth - Texture.Width || newPosition.Y < 0 || newPosition.Y > windowHeight - Texture.Height)
            {
                setRandomDirection();
            }
            else
            {
                Position = newPosition;
            }
            //detect stuck loop from here
            if (Vector2.Distance(Position, previousPosition) < Speed * 0.1)
            {
                stuckTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (stuckTime > maxstuckTime)
                {
                    setRandomDirection();
                    stuckTime = 0;
                }
            }
            else
            {
                stuckTime = 0;
                previousPosition = Position;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
