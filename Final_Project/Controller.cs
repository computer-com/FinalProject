using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

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
        public bool KeyCollected;
        public String Messages;
        public int score;
        public SoundEffect KeyCollect;
        public SoundEffect CoinCollect;
        public SoundEffect KeyPress;
        public SoundEffect GameOver;

        public bool HasExited { get;  set; }

        public GameState state { get; set; } = GameState.Menu;
        public List<Vector2> CoinPositions { get; set; }
        public float CoinRadius { get; set; } = 15;
        public Vector2 KeyPosition {  get; set; }
        public float KeyRadius { get; set; } = 20;
        public Vector2 ExitPoint { get; set; }
        public float ExitPointRadius { get; set; } = 50;

        public Controller(SoundEffect coinCollect, SoundEffect keyCollect, SoundEffect gameOver, SoundEffect keyPress)
        {
            this.elapsedTime = TimeSpan.Zero;
            this.SecondElapsed = 0;
            this.KeyCollected = false;
            this.HasExited = false;
            this.CoinPositions = new List<Vector2>();
            this.CoinCollect = coinCollect;
            this.KeyPress = keyPress;
            this.GameOver = gameOver;
            this.KeyCollect = keyCollect;
            GenerateCoins();
            GenerateKey();
            GenerateExit();
        }
        private void GenerateCoins()
        {
            Random rand = new Random();
            CoinPositions.Clear();
            for (int i = 0; i < 8; i++)
            {
                int x = rand.Next(100, 1300);
                int y = rand.Next(100, 700);
                CoinPositions.Add(new Vector2(x, y));
            }
        }

        private void GenerateKey()
        {
            Random random = new Random();
            int x = random.Next(100, 1300);
            int y = random.Next(100, 700);
            KeyPosition = new Vector2(x, y);
        }

        private void GenerateExit()
        {
            ExitPoint = new Vector2(1120, 100);
        }


        public void Update(KeyboardState keyboardState, Game1 game ,GameTime gameTime , Player player)
        {
            UpdateTimer(gameTime);

            switch (state)
            {
                case GameState.Menu:
                    MenuInput(keyboardState, game , player);
                    break;
                case GameState.Play:
                    GameInput(keyboardState, game , player);
                    break;
            }

            EndGame();
        }

        private void MenuInput(KeyboardState keyboardState, Game1 game, Player player)
        {
            //start playing the game
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                KeyPress.Play();
                state = GameState.Play;
                SecondElapsed = 0;
                score = 0;
                HasExited = false;
                KeyCollected = false;
                GenerateKey();
                GenerateCoins();
                player.ResetPosition();
            }
            //exit the game
            else if (keyboardState.IsKeyDown(Keys.Escape))
            {
                KeyPress.Play();
                game.Exit();
            }
        }

        private void GameInput(KeyboardState keyboardState, Game1 game, Player player)
        {
            //exit the game
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                KeyPress.Play();
                game.Exit();
            }
            //Coins
            for (int i = 0; i < CoinPositions.Count; i++)
            {
                //Coins Collision
                if (IsCollision(player.Position, CoinPositions[i], player.Radius, CoinRadius))
                {
                    CoinPositions.RemoveAt(i);
                    score++;
                    CoinCollect.Play();
                    break;
                }
            }
            //Collision for key
            if (IsCollision(player.Position, KeyPosition, player.Radius, KeyRadius))
            {
                KeyCollected = true;
                KeyCollect.Play();
                Messages = "You collected the key!";
            }
            if (!KeyCollected && IsCollision(player.Position, ExitPoint, player.Radius, ExitPointRadius))
            {
                Messages = "You need to collect the key to exit!";
                return;
            }

            //Exit Game
            if (KeyCollected && IsCollision(player.Position, ExitPoint, player.Radius, ExitPointRadius))
            {
                state = GameState.Menu;
                HasExited = true;
                GameOver.Play();
                Messages = "You Escaped";
            }


        }
        private bool IsCollision(Vector2 playerposition, Vector2 objectposition, float playerRadius, float objectRadius)
        {
            float distance = Vector2.Distance(playerposition, objectposition);

            return distance <= (playerRadius + objectRadius);
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
            if (SecondElapsed >= 50 && state == GameState.Play)
            {
                state = GameState.Menu;
                GameOver.Play();
                Messages = "GameOver ! Time up";
            }

        }
    }
}
