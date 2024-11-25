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
        public GameState state { get; set; } = GameState.Menu;

        public void Update(KeyboardState keyboardState, Game1 game)
        {
            switch (state)
            {
                case GameState.Menu:
                    MenuInput(keyboardState, game);
                    break;
                case GameState.Play:
                    GameInput(keyboardState, game);
                    break;
            }
        }

        private void MenuInput(KeyboardState keyboardState, Game1 game)
        {
            //start playing the game
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                state = GameState.Play;
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
    }
}
