﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Arkanoid
{
    class Brick : TransformingImage
    {
        ushort _typeID;
        ushort _colorID;
        
        enum BrickType : ushort { Normal = 1, Reinforced };
        enum BrickColor : ushort { Black = 1, Red, Orange, Yellow, Green, Cyan, Blue, Purple, Magenta, White };

        public Brick(ref Grid grid, ushort typeID, ushort colorID, int xOnGrid, int yOnGrid) : base(GetPath(typeID, colorID), grid, 160 + xOnGrid * 32 - 32, yOnGrid * 16, 32, 16)
        {
            _typeID = typeID;
            _colorID = colorID;
        }

        static private Uri GetPath(ushort typeID, ushort colorID)
        {
            string path = "./Graphics/";
            switch(typeID)
            {
                case 1: path += "brick-"; break;
                case 2: path += "reinforced_brick-"; break;
            }
            path += colorID + ".png";

            return new Uri(path, UriKind.Relative);
        }

        public bool Collisions(ref List <Ball> balls) //TRUE - cegiełka istnieje; FALSE - cegiełka została zniszczona
        {
            Rect collisionsArea = new Rect(Margin.Left, Margin.Top, Width, Height);

            foreach(Ball ball in balls)
            {
                Rect ballArea = new Rect(ball.Margin.Left, ball.Margin.Top, ball.Width, ball.Height);

                if (collisionsArea.IntersectsWith(ballArea))
                {
                    ball.Bounce(collisionsArea);

                    switch(_typeID)
                    {
                        case 1:
                            GameControl.AddPoints(25);
                            return false;
                        case 2:
                            GameControl.AddPoints(10);
                            _typeID = (ushort)BrickType.Normal;
                            Source = new BitmapImage(GetPath(_typeID, _colorID));
                            break;
                    }
                }
            }
            return true;
        }
    }
}
