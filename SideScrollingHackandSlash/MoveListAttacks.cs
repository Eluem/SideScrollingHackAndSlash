//******************************************************
// File: MoveListAttacks.cs
//
// Purpose: Contains the class definition of
// MoveListAttacks. MoveListAttacks contains the list
// of attack gestures the player can use.
//
// Written By: Salvatore Hanusiewicz
//******************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
namespace SideScrollingHackandSlash
{
    enum Attacks { noAction, PushLeft, PushRight, LongOverheadRight, LongOverheadLeft, ShortJabRight, ShortJabLeft, LongJabRight, LongJabLeft, UpsweepRight, UpsweepLeft };
    class MoveListAttacks:MoveList
    {
        //****************************************************
        // Method: MoveListAttacks
        //
        // Purpose: Constructor for MoveListAttacks
        //****************************************************
        public MoveListAttacks() : base()
        {
            List<Point> moveTestList;
            List<Point> timeTestList;
            List<Vector2[]> speedTestList;

            //Long Overhead Left
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(2, 0));
            moveTestList.Add(new Point(1, 0));
            moveTestList.Add(new Point(0, 0));
            moveTestList.Add(new Point(0, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, 150));
            timeTestList.Add(new Point(-2, 150));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(4);


            //Long Overhead Right
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(0, 0));
            moveTestList.Add(new Point(1, 0));
            moveTestList.Add(new Point(2, 0));
            moveTestList.Add(new Point(2, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, 150));
            timeTestList.Add(new Point(-2, 150));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(3);



            //Short Jab Right
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(0, 1));
            moveTestList.Add(new Point(1, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(.135F / 29, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(5);


            //Short Jab Left
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(2, 1));
            moveTestList.Add(new Point(1, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(.135F / 29, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(6);

            //Long Jab Right
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(0, 1));
            moveTestList.Add(new Point(1, 1));
            moveTestList.Add(new Point(2, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, 150));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(7);


            //Long Jab Left
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(2, 1));
            moveTestList.Add(new Point(1, 1));
            moveTestList.Add(new Point(0, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, 150));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(8);

            //Upsweep Right
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(0, 2));
            moveTestList.Add(new Point(1, 2));
            moveTestList.Add(new Point(2, 2));
            moveTestList.Add(new Point(2, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, 150));
            timeTestList.Add(new Point(-2, 150));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(9);


            //Upsweep Left
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(2, 2));
            moveTestList.Add(new Point(1, 2));
            moveTestList.Add(new Point(0, 2));
            moveTestList.Add(new Point(0, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, 150));
            timeTestList.Add(new Point(-2, 150));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(10);

            /*
            //Short Overhead Right
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(0, 0));
            moveTestList.Add(new Point(1, 0));
            moveTestList.Add(new Point(2, 0));
            moveTestList.Add(new Point(2, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(9);


            //Short Overhead Left
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(2, 0));
            moveTestList.Add(new Point(1, 0));
            moveTestList.Add(new Point(0, 0));
            moveTestList.Add(new Point(0, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(10);
            */




            /*
            //Push Left
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(2, 1));
            moveTestList.Add(new Point(1, 1));
            moveTestList.Add(new Point(0, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(1);

            //Push Right
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, 1));
            moveTestList.Add(new Point(-2, 1));
            moveTestList.Add(new Point(0, 1));
            moveTestList.Add(new Point(1, 1));
            moveTestList.Add(new Point(2, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(2);
            */
        }
    }
}
