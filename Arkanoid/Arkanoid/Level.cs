using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arkanoid
{
    class Level
    {
        static private void ClearLevel(ref Grid grid, ref List<Brick> bricks, ref List<Bonus> bonuses)
        {
            //Cleaning the list of bricks
            if (bricks != null)
            {
                if (bricks.Count != 0)
                {
                    foreach (Brick brick in bricks)
                        grid.Children.Remove(brick);
                    bricks.Clear();
                }
            }
            else
                bricks = new List<Brick>();
            //Cleaning the list of bonuses
            if (bonuses != null)
            {
                if (bonuses.Count != 0)
                {
                    foreach (Bonus bonus in bonuses)
                        grid.Children.Remove(bonus);
                    bonuses.Clear();
                }
            }
            else
                bonuses = new List<Bonus>();
        }

        static public bool LoadLevel(int levelID, ref Grid grid, ref List <Brick> bricks, ref List<Bonus> bonuses)
        {
            ClearLevel(ref grid, ref bricks, ref bonuses);

            try
            {
                StreamReader file = new StreamReader(Path.GetFullPath("Levels/lvl-" + levelID + ".lvl"));
                int line = 1;
                do
                {
                    string lineStr = file.ReadLine();
                    string[] IDsStr = lineStr.Split(' ');
                    int[] IDs = new int[IDsStr.Length];
                    
                    for (int i = 0; i < IDsStr.Length; i++)
                        IDs[i] = Convert.ToInt32(IDsStr[i]);
                    
                    for (int i = 0; i < IDs.Length / 2; i++)    //2 IDs for each brick - type and colour
                    {
                        if (IDs[i * 2] != 0)
                            bricks.Add(new Brick(ref grid, (Brick.BrickType)IDs[i * 2], (Brick.BrickColor)IDs[i * 2 + 1], i + 1, line));
                    }

                    line++;
                } while (!file.EndOfStream);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
