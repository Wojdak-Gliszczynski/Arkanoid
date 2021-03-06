﻿using System;
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
        static private bool _gameOver;
        static private int _score;
        static private int _life;
        static private int _level;

        static private Label _labelScore;
        static private Label _labelLife;
        static private Label _labelLevel;

        static public int Score { get { return _score; } }

        public enum Sound
            { BouncingBall, BreakBrick, BreakReinforcement, Explosion, ExtraBall, ExtraPoints, NextLevel, Record }
        //-------------------------------------------------------
        static public void StartGame
            (ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            _gameOver = false;
            _score = 0;
            _life = 3;
            _level = 1;

            Uri pathToBackground = new Uri("./Graphics/background-0.png", UriKind.Relative);
            TransformingImage background = new TransformingImage(pathToBackground, grid, 160, 0, 480, 600);

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
            StartLevel(ref grid, ref platform, ref balls, ref bricks, ref bonuses);
        }

        static public void CheckGameState
            (ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List <Brick> bricks, ref List<Bonus> bonuses)
        {
            ushort destructibleBricksCount = 0;
            foreach (Brick brick in bricks)
                if (brick.Type != Brick.BrickType.Indestructible)
                    destructibleBricksCount++;

            if (destructibleBricksCount == 0 && !NextLevel(ref grid, ref platform, ref balls, ref bricks, ref bonuses))
                GameOver();
            if (balls.Count == 0)
                LostLife(ref grid, ref platform, ref balls, ref bricks, ref bonuses);
        }

        static public void LostLife
            (ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            _life--;
            if (_life == 0)
                GameOver();
            else
                StartLevel(ref grid, ref platform, ref balls, ref bricks, ref bonuses);
        }

        static public bool StartLevel
            (ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            //Cleaning graphics from the grid and from the lists of elements
            if (platform != null)
                platform.RemoveFromGrid(ref grid);
            if (balls != null)
            {
                foreach (Ball ball in balls)
                    grid.Children.Remove(ball);
                balls.Clear();
            }

            //Creating new elements
            platform = new Platform(grid);
            
            balls = new List<Ball>();
            balls.Add(new Ball(grid, platform));

            return (Level.LoadLevel(_level - 1, ref grid, ref bricks, ref bonuses) ? true : false);
        }

        static public bool NextLevel
            (ref Grid grid, ref Platform platform, ref List<Ball> balls, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            _level++;
            PlaySound(Sound.NextLevel);
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
            _gameOver = true;
        }

        static public bool IsTheGameOver()
        {
            return _gameOver;
        }

        static public void PlaySound(Sound sound)
        {
            string path = "./../../Sounds/";
            switch (sound)
            {
                case Sound.BouncingBall: path += "bouncing_ball";  break;
                case Sound.BreakBrick: path += "break_brick"; break;
                case Sound.BreakReinforcement: path += "break_reinforcement"; break;
                case Sound.Explosion: path += "explosion"; break;
                case Sound.ExtraBall: path += "extra_ball"; break;
                case Sound.ExtraPoints: path += "extra_points"; break;
                case Sound.NextLevel: path += "next_level"; break;
                case Sound.Record: path += "record"; break;
            }
            path += ".mp3";
            
            var player = new System.Windows.Media.MediaPlayer();
            player.Open(new Uri(path, UriKind.Relative));
            player.Play();
        }
    }
}
