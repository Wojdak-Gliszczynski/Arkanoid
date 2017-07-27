using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid
{
    class Scoreboard
    {
        private ScoreboardItem[] _scoreboardItems;

        public ScoreboardItem[] Item
        {
            get { return _scoreboardItems; }
            set { _scoreboardItems = value; }
        }
        //-------------------------------------------
        public Scoreboard()
        {
            _scoreboardItems = new ScoreboardItem[10];
            for (int i = 0; i < _scoreboardItems.Length; i++)
                _scoreboardItems[i] = new ScoreboardItem();
        }
        //-------------------------------------------
        public void AddItem(ScoreboardItem item)
        {
            for (int i = _scoreboardItems.Length - 2; i >= 0; i--)
            {
                if (item > _scoreboardItems[i])
                {
                    _scoreboardItems[i + 1] = _scoreboardItems[i];
                    if (i == 0) //Best score
                        _scoreboardItems[0] = item;
                }
                else //First higher than that one
                {
                    _scoreboardItems[i + 1] = item;
                    break;
                }
            }
        }
        public bool IsAScoreBeaten(ScoreboardItem item)
        {
            return (item > _scoreboardItems[_scoreboardItems.Length - 1]);
        }
        public bool IsAScoreBeaten(int score)
        {
            return (score > _scoreboardItems[_scoreboardItems.Length - 1].Score);
        }
    }
}
