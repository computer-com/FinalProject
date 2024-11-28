using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public enum GameState
    {
        Menu, Play
    }

    internal class Controller
    {
        public TimeSpan elapsedTime;
        public int SecondElapsed;
        public bool Health;
        public String Messages;

        public GameState state { get; set; } = GameState.Menu;

        public Controller()
        {
            this.elapsedTime = TimeSpan.Zero;
            this.SecondElapsed = 0;
            this.Health = false;
        }



        public void Update(KeyboardState keyboardState, Game1 game ,GameTime gameTime)
        {
            UpdateTimer(gameTime);

            switch (state)
            {
                case GameState.Menu:
                    MenuInput(keyboardState, game);
                    break;
                case GameState.Play:
                    GameInput(keyboardState, game);
                    break;
            }

            EndGame();
        }

        private void MenuInput(KeyboardState keyboardState, Game1 game)
        {
            //start playing the game
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                state = GameState.Play;
                SecondElapsed = 0;
            }
            //exit the game
            else if (keyboardState.IsKeyDown(Keys.Escape))
            {
                game.Exit();
            }
        }

        private void GameInput(KeyboardState keyboardState, Game1 game)
        {
            //exit the game
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                game.Exit();
            }
        }

        public void UpdateTimer(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime.TotalSeconds >= 1)
            {
                SecondElapsed++;
                elapsedTime = TimeSpan.Zero;
            }
        }

        public void EndGame()
        {
            if (SecondElapsed >= 10 && state == GameState.Play)
            {
                state = GameState.Menu;
                Messages = "GameOver ! Time up";
            }

        }
    }
}
