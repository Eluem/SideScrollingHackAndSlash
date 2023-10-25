//******************************************************
// File: MoveListMovements.cs
//
// Purpose: Contains the class definition of
// MoveListMovements. MoveListMovements contains the
// list of movement gestures the player can use.
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
    //Action enumeration, used to enumerate the values for different actions which will be used in
    //a switch statement in the Control function for a player in testing.
    enum Action { noAction, dashRight, dashLeft, jump };
    class MoveListMovements:MoveList
    {
        //****************************************************
        // Method: MoveListMovements
        //
        // Purpose: Constructor for MoveListMovements
        //****************************************************
        public MoveListMovements()
            : base()
        {
            List<Point> moveTestList;
            List<Point> timeTestList;
            List<Vector2[]> speedTestList;

            /*
            //Thumb stick jumping
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, 1));
            moveTestList.Add(new Point(-2, 0));
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
            speedTestList.Add(new Vector2[2] { new Vector2(-2, .135F / 29), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(1);
            */

            //Dash right
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(2, 1));
            moveTestList.Add(new Point(1, 1));
            moveTestList.Add(new Point(2, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, 120));
            timeTestList.Add(new Point(-2, 120));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(1);

            //Dash Left
            moveTestList = new List<Point>();
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(-2, -2));
            moveTestList.Add(new Point(0, 1));
            moveTestList.Add(new Point(1, 1));
            moveTestList.Add(new Point(0, 1));
            moveList.Add(moveTestList);
            timeTestList = new List<Point>();
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, -2));
            timeTestList.Add(new Point(-2, 120));
            timeTestList.Add(new Point(-2, 120));
            timeRangeList.Add(timeTestList);

            speedTestList = new List<Vector2[]>();
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedTestList.Add(new Vector2[2] { new Vector2(-2, -2), new Vector2(-2, -2) });
            speedRangeList.Add(speedTestList);
            moveListAction.Add(2);
        }
    }
}
