using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Final_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Images, Fonts, and Sounds
        Texture2D Player;
        Texture2D Index;
        Texture2D Key;
        Texture2D coin;
        SpriteFont Menu;
        SoundEffect Background_Music;
        SoundEffect GameOver;
        SoundEffect CoinCollect;
        SoundEffect KeyCollect;
        SoundEffect KeyPress;

        // Player and Enemies
        private Player player;
        private List<Enemy> enemies;
        // Multiple enemies
        private Texture2D enemyTexture;
        private Texture2D spiderTexture;
        // Walls and Doorways
        private List<Rectangle> wallRectangles;
        private Texture2D wallTexture;
        private Texture2D enemy3;

        // Controller
        private Controller controller;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1450;
            _graphics.PreferredBackBufferHeight = 1035;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Assets
            coin = Content.Load<Texture2D>("Coin");
            Index = Content.Load<Texture2D>("INDEX");
            Player = Content.Load<Texture2D>("blue plater");
            Key = Content.Load<Texture2D>("KEY");
            Background_Music = Content.Load<SoundEffect>("BG_MUSIC");
            CoinCollect = Content.Load<SoundEffect>("COINCOLLECT");
            KeyCollect = Content.Load<SoundEffect>("KEYCOLLECT");
            GameOver = Content.Load<SoundEffect>("GAMEOVER");
            KeyPress = Content.Load<SoundEffect>("CLICK");
            Menu = Content.Load<SpriteFont>("Menu");
            enemyTexture = Content.Load<Texture2D>("Enemy");
            spiderTexture = Content.Load<Texture2D>("download-removebg-preview (1)");
            enemy3 =Content.Load<Texture2D>("ene-removebg-preview");

            // Initialize Player
            player = new Player(new Vector2(100, 150), 5f, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 20);

            // Initialize Enemies
            List<Vector2> enemyPositions = new List<Vector2>
            {
                new Vector2(200, 200),
                new Vector2(700, 200),
                new Vector2(1200, 200),
                new Vector2(200, 600),
                new Vector2(750, 600),
                new Vector2(1200, 600),
            };

            enemies = new List<Enemy>();

            // Load all enemy textures
            List<Texture2D> enemyTextures = new List<Texture2D> { enemyTexture, spiderTexture, enemy3 };

            // Create Random instance
            Random random = new Random();

            // Assign random textures to each enemy
            for (int i = 0; i < enemyPositions.Count; i++)
            {
                // Pick a random texture from the list
                Texture2D texture = enemyTextures[random.Next(enemyTextures.Count)];
                enemies.Add(new Enemy(texture, enemyPositions[i], 4f, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
            }


            // Initialize Walls
            wallRectangles = new List<Rectangle>
            {
                new Rectangle(20, 120, 1095, 6),
                new Rectangle(1220, 120, 195, 6),
                new Rectangle(20, _graphics.PreferredBackBufferHeight - 120, 1395, 6),
                new Rectangle(20, 120, 6, 800),
                new Rectangle(1415, 120, 6, 800),
                new Rectangle(1045, 120, 6, 635),
                new Rectangle(360, 120, 6, 60),
                new Rectangle(360, 270, 6, 310),
                new Rectangle(1045, 855, 6, 60),
                new Rectangle(710, 740, 6, 180),
                new Rectangle(710, 580, 6, 60),
                new Rectangle(20, 580, 110, 6),
                new Rectangle(235, 580, 200, 6),
                new Rectangle(540, 580, 505, 6),
                new Rectangle(1050, 470, 60, 6),
                new Rectangle(1200, 470, 215, 6),
            };
           

            // Create Wall Texture
            wallTexture = new Texture2D(GraphicsDevice, 1, 1);
            wallTexture.SetData(new[] { Color.White });

            // Initialize Controller
            controller = new Controller(CoinCollect, KeyCollect, GameOver, KeyPress);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (controller.state == GameState.Play)
            {
                // Update Player
                player.Update(gameTime, wallRectangles);

                // Update Enemies - start moving immediately
                foreach (var enemy in enemies)
                {
                    enemy.Update(gameTime, wallRectangles);

                    // Check for collision between player and enemy
                    if (Vector2.Distance(player.Position, enemy.Position) < 50)
                    {
                        controller.state = GameState.Menu; // Game Over
                        controller.Messages = "Game Over! You collided with an enemy.";
                        GameOver.Play();
                        break;
                    }
                }
            }

            // Update Controller
            controller.Update(Keyboard.GetState(), this, gameTime, player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            _spriteBatch.Begin();

            switch (controller.state)
            {
                case GameState.Menu:
                    _spriteBatch.Draw(Index, new Vector2(0, 0), Color.White);
                    string buttonText = controller.HasExited ? "Replay ( Press Enter)" : "Play ( Press Enter)";
                    _spriteBatch.DrawString(Menu, buttonText, new Vector2(_graphics.PreferredBackBufferWidth / 2 - 150, _graphics.PreferredBackBufferHeight / 2 - 300), Color.Red);
                    _spriteBatch.DrawString(Menu, "Exit ( Press Esc  )", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 150, _graphics.PreferredBackBufferHeight / 2 - 250), Color.Red);
                    _spriteBatch.DrawString(Menu, "BACKROOM : THE HAUNT", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 150, _graphics.PreferredBackBufferHeight / 2 + 250), Color.Red);
                    if (!string.IsNullOrEmpty(controller.Messages))
                    {
                        _spriteBatch.DrawString(Menu, controller.Messages, new Vector2(_graphics.PreferredBackBufferWidth / 2 - 150, _graphics.PreferredBackBufferHeight / 2 - 150), Color.Red);
                    }
                    break;

                case GameState.Play:
                    // Draw Coins
                    foreach (var coins in controller.CoinPositions)
                    {
                        _spriteBatch.Draw(coin, new Rectangle((int)coins.X, (int)coins.Y, 40, 40), Color.White);
                    }

                    // Draw Walls
                    foreach (var wall in wallRectangles)
                    {
                        _spriteBatch.Draw(wallTexture, wall, Color.Red);
                    }

                   

                    // Draw Player
                    _spriteBatch.Draw(Player, new Rectangle((int)player.Position.X, (int)player.Position.Y, 50, 50), Color.White);

                    // Draw Enemies
                    foreach (var enemy in enemies)
                    {
                        enemy.Draw(_spriteBatch);
                    }

                    // Draw Key
                    if (!controller.KeyCollected)
                    {
                        _spriteBatch.Draw(Key, new Rectangle((int)controller.KeyPosition.X, (int)controller.KeyPosition.Y, 30, 30), Color.White);
                    }

                    // Draw Score
                    _spriteBatch.DrawString(Menu, $"Score : {controller.score}", new Vector2(1200, 20), Color.Red);

                    // Draw Timer
                    _spriteBatch.DrawString(Menu, $"Time Remaining: {50 - controller.SecondElapsed} seconds", new Vector2(20, 20), Color.Red);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
