using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid
{
    class ScoreboardItem
    {
        private int _score;
        private string _name;

        public int Score { get { return _score; } }
        public string Name { get { return _name; } }
        //-------------------------------------------
        public ScoreboardItem(string name = "Noname", int score = 0)
        {
            _score = score;
            _name = name;
        }
        //-------------------------------------------
        public void ChangeValues(int score, string name)
        {
            if (score > 0)
            {
                _score = score;
                _name = name;
            }
        }

        //Operators 
        public static bool operator >(ScoreboardItem ItemA, ScoreboardItem ItemB)
        {
            return (ItemA._score > ItemB._score);
        }
        public static bool operator <(ScoreboardItem ItemA, ScoreboardItem ItemB)
        {
            return (ItemA._score < ItemB._score);
        }
        public static bool operator >=(ScoreboardItem ItemA, ScoreboardItem ItemB)
        {
            return (ItemA._score >= ItemB._score);
        }
        public static bool operator <=(ScoreboardItem ItemA, ScoreboardItem ItemB)
        {
            return (ItemA._score <= ItemB._score);
        }
        public static bool operator ==(ScoreboardItem ItemA, ScoreboardItem ItemB)
        {
            return (ItemA._score == ItemB._score);
        }
        public static bool operator !=(ScoreboardItem ItemA, ScoreboardItem ItemB)
        {
            return (ItemA._score != ItemB._score);
        }
    }
}
