using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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

        //Controller initialization
        Controller controller = new Controller();

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
            Background_Music = Content.Load<SoundEffect>("BG_MUSIC");
            Menu = Content.Load<SpriteFont>("Menu");

            Background_Music.Play();
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
                    
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
