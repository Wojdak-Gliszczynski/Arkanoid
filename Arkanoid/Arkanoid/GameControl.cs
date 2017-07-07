using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arkanoid
{
    class GameControl
    {
        static private int _score;
        static private int _life;
        static private int _level;

        static private Label _labelScore;
        static private Label _labelLife;
        static private Label _labelLevel;

        static public void StartGame(ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            _score = 0;
            _life = 3;
            _level = 1;

            _labelScore = new Label();
            _labelScore.Margin = new Thickness(8, 16, 0, 0);
            grid.Children.Add(_labelScore);

            _labelLife = new Label();
            _labelLife.Margin = new Thickness(8, 32, 0, 0);
            grid.Children.Add(_labelLife);

            _labelLevel = new Label();
            _labelLevel.Margin = new Thickness(8, 48, 0, 0);
            grid.Children.Add(_labelLevel);

            RefreshStatistics(ref grid);

            //Uruchom poziom
            StartLevel(ref grid, ref platform, ref balls, ref bricks, ref bonuses);
        }

        static public void CheckGameState(ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List <Brick> bricks, ref List<Bonus> bonuses)
        {
            if (bricks.Count == 0)
            {
                if (!NextLevel(ref grid, ref platform, ref balls, ref bricks, ref bonuses))
                    GameOver();
            }
            if (balls.Count == 0)
                LostLife(ref grid, ref platform, ref balls, ref bricks, ref bonuses);
        }

        static public void LostLife(ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            _life--;
            if (_life == 0)
                GameOver();
            StartLevel(ref grid, ref platform, ref balls, ref bricks, ref bonuses);
        }

        static public bool StartLevel(ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            //Czyszczenie grafik z siatki
            if (platform != null)
                platform.RemoveFromGrid(ref grid);
            if (balls != null)
                foreach (Ball ball in balls)
                    grid.Children.Remove(ball);

            //Tworzenie nowych obiektów
            platform = new Platform(ref grid);
            
            balls = new List<Ball>();
            balls.Add(new Ball(grid));

            return (Level.LoadLevel(_level - 1, ref grid, ref bricks, ref bonuses) ? true : false);
        }

        static public bool NextLevel(ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            _level++;
            return (StartLevel(ref grid, ref platform, ref balls, ref bricks, ref bonuses) ? true : false);
        }

        static public void AddPoints(int points)
        {
            _score += points;
        }

        static public void AddLife()
        {
            _life++;
        }

        static public void RefreshStatistics(ref Grid grid)
        {
            _labelScore.Content = "Score: " + _score;
            _labelLife.Content = "Life: " + _life;
            _labelLevel.Content = "Level: " + _level;
        }

        static public void GameOver()
        {
            Application.Current.Shutdown();
        }
    }
}
