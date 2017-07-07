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
        static public bool LoadLevel(int levelID, ref Grid grid, ref List <Brick> bricks)
        {
            if (bricks != null && bricks.Count != 0)
                foreach (Brick brick in bricks)
                    grid.Children.Remove(brick);

            bricks = new List<Brick>();
            try
            {
                StreamReader file = new StreamReader(System.IO.Path.GetFullPath("Levels/lvl-" + levelID + ".lvl"));
                int line = 1;
                do
                {
                    string lineStr = file.ReadLine();
                    string[] IDsStr = lineStr.Split(' ');
                    int[] IDs = new int[IDsStr.Length];
                    
                    for (int i = 0; i < IDsStr.Length; i++)
                        IDs[i] = Convert.ToInt32(IDsStr[i]);
                    
                    for (int i = 0; i < IDs.Length / 2; i++)    //Każdej cegiełce przypisuje się 2 liczby - Typ i kolor
                    {
                        switch (IDs[i * 2])
                        {
                            //0 - brak cegiełki
                            case 1: //1 - Zwykła cegiełka
                                    bricks.Add(new Brick(ref grid, Convert.ToUInt16(IDs[i * 2 + 1]), i + 1, line));
                                    break;
                        }
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
