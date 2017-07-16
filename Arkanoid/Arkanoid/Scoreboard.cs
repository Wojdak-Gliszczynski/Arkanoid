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
            for (int i = 1; i < _scoreboardItems.Length; i++)
            {
                if (item >= _scoreboardItems[i])
                {
                    _scoreboardItems[i - 1] = _scoreboardItems[i];
                    if (item == _scoreboardItems[i])
                    {
                        _scoreboardItems[i] = item;
                        break;
                    }
                }
            }
        }
    }
}
