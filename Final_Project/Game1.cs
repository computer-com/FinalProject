using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Final_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //Images , Font, SoundEffect
        Texture2D Index;
        SpriteFont Menu;
        SoundEffect Background_Music;
        //Adding Map
        Texture2D Map;

        //Controller initialization
        Controller controller = new Controller();

        //Wall Delclaration
        private List<Rectangle> wallRectangles;
        private Texture2D wallTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1450;
            _graphics.PreferredBackBufferHeight = 1035;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
            Index = Content.Load<Texture2D>("INDEX");
            //Loading Map As Content
            Map = Content.Load<Texture2D>("Maze-Level1");
            Background_Music = Content.Load<SoundEffect>("BG_MUSIC");
            Menu = Content.Load<SpriteFont>("Menu");

            Background_Music.Play();

            //Loading walls with coordinates
            wallRectangles = new List<Rectangle>
            {
                //coordinated of top and bottom outer walls
                new Rectangle(20, 120, 1395, 6),
                new Rectangle(20, _graphics.PreferredBackBufferHeight - 120, 1395, 6),
                //coordinated of left and right outer walls
                new Rectangle(20, 120, 6, 800),
                new Rectangle(1415, 120, 6, 800),

                //Now for the maze walls inside starting from vertical walls of map
                new Rectangle(1045, 120, 6, 635), //long wall on right most
                new Rectangle(360, 120, 6, 60), //left small edge on top
                new Rectangle(360, 270, 6, 310),//left long wall in the middle
                new Rectangle(1045, 855, 6, 60),//small edge below left long wall
                new Rectangle(710, 740, 6, 180),//middle bottom wall
                new Rectangle(710, 580, 6, 60),//middle small edge right above

                //Now for the maze walls inside starting from horizontal walls of map
                new Rectangle(20, 580, 110, 6), //left small edge on the middle
                new Rectangle(235, 580, 200, 6),//middle long wall 
                new Rectangle(540, 580, 505, 6),// right middle long wall
                new Rectangle(1050, 470, 60, 6),//small on the right small edge 
                new Rectangle(1200, 470, 215, 6),//rightmost edge wall
            };
            wallTexture = new Texture2D(GraphicsDevice, 1, 1);
            wallTexture.SetData(new[] {Color.White});
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            controller.Update(Keyboard.GetState(),this);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            _spriteBatch.Begin();

            // TODO: Add your drawing code here
            switch (controller.state)
            {
                case GameState.Menu:
                    _spriteBatch.Draw(Index, new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(Menu, "Play ( Press Enter)", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 150, _graphics.PreferredBackBufferHeight / 2 - 300), Color.GhostWhite);
                    _spriteBatch.DrawString(Menu, "Exit ( Press Esc  )", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 150, _graphics.PreferredBackBufferHeight / 2 - 250), Color.WhiteSmoke);
                    _spriteBatch.DrawString(Menu, "WELCOME TO BACKROOM : THE HAUNT", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 250, _graphics.PreferredBackBufferHeight / 2 + 250), Color.White);
                    break;
                case GameState.Play:
                    //DECLARED MAP TO DRAW METHOD
                    _spriteBatch.Draw(Map, new Vector2(0, 0), Color.LightBlue);
                    //drawing walls
                    foreach (var wall in wallRectangles)
                    {
                        _spriteBatch.Draw(wallTexture, wall, Color.Red);
                    }
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
