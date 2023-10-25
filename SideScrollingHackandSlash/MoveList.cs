//******************************************************
// File: MoveList.cs
//
// Purpose: Contains the class definition of
// MoveList. MoveList will be
// inherited by any definition of moves
// for thumbsticks using my grid system.
// I will most likely only define 2 move lists.
//
// Written By: Salvatore Hanusiewicz
//******************************************************

//NOTE TO SELF
//Possibly convert this entire class to staticness, and all that inherit it, so that ActionFlow doesn't
//need to have instances of it

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
    class MoveList
    {
        protected List<List<Point>> moveList; //Holds a list of all the moves. A move is 
        //defined by a series of points in a 3x3 grid
        //a value of -2 represents a wild card of sorts,
        //meaning that position should be ignored in checking
        //if the player entered this move

        protected List<List<Point>> timeRangeList; //Holds a list of all the ranges of time that the player must
        //satisfy with their attempt at entering a combo to be successful
        //-2 represents a wild card here as well. The X value is used for the low range and Y for the high.

        protected List<int> moveListAction;       //Stores the integer value (which will be cast to th enum value)
        //of the action that a particular move should trigger
        //action 0 is reserved for doing noAction in all MoveLists


        protected List<List<Vector2[]>> speedRangeList; //Holds the min and maximum speed ranges that satisfy
        //a move's requirements (all vector arrays should be of size 2) index 0 is low index 1 is high


        //****************************************************
        // Method: MoveList
        //
        // Purpose: Constructor for MoveList
        //****************************************************
        public MoveList()
        {

            //List initializations
            moveList = new List<List<Point>>();
            timeRangeList = new List<List<Point>>();
            moveListAction = new List<int>();
            speedRangeList = new List<List<Vector2[]>>();


            //Note to self for creating an object that inherits MoveList:
            //I would suggest that, when creating multiple actions that could potentially be nested in one another
            //(especially short actions that watch, for example, the last two items in the action list)
            //that you add actions to your game in order of longer actions first and smaller actions last
            //This way, smaller actions won't break the program out of the loop before hitting the longer actions
            //Though you should still be warey, when implementing the actions, of how you want the game to react
            //in respect to having a smaller action that could be nested in a larger action occuring right after
            //each other. To prevent issues like this you can tweak your actions using the different options
            //so that they might nest well but won't be triggered in chain, unless that is what is desired.
            //Also, for the implementation you can prevent one action from occuring after another if you check
            //that the previously registered action wasn't an action you wouldn't want to stack.
        }


        //****************************************************
        // Method: MoveCheck
        //
        // Purpose: Checks if someFlow fits any moves
        // and upon finding one, reports the integer value
        // of that action, if any. 0 is returned by default.
        //****************************************************
        public int MoveCheck(FlowList someFlow)
        {
            Boolean moveSuccess;//used to track if a move succeeded while running through the loop
            //timeRangeList enumerators
            IEnumerator<List<Point>> moveTime; //acts as the iterator through timeRangeList
            IEnumerator<Point> time;          //acts as the iterator through moveTime

            moveTime = timeRangeList.GetEnumerator();//points the iterator into the timeRangeList list

            //moveListAction enumerator
            IEnumerator<int> moveAction; //acts as the iterator through moveListAction

            moveAction = moveListAction.GetEnumerator();//points the iterator into the moveListAction list

            //speedRangeList enumerators
            IEnumerator<List<Vector2[]>> speedRanges; //acts as the iterator through speedRangeList
            IEnumerator<Vector2[]> speedRange; //acts as the iterator through speedRanges

            speedRanges = speedRangeList.GetEnumerator(); //points the iterator into the speedRangeList List

            //someFlow enumerators
            IEnumerator<Point> toTestAction; //acts as the iterator through someFlow.PositionFlow
            IEnumerator<int> toTestTime;     //acts as the iterator through someFlow.TimeFlow
            IEnumerator<Vector2> toTestSpeed; //acts as iterator through someFlow.SpeedFlow

            toTestAction = someFlow.PositionFlow.GetEnumerator();//points the iterator into the PositionFlow list
            toTestTime = someFlow.TimeFlow.GetEnumerator();//points the iterator into the TimeFlow list
            toTestSpeed = someFlow.SpeedFlow.GetEnumerator();//points the iterator into the SpeedFlow list
            
            foreach (List<Point> move in moveList)
            {
                //Outer iterations
                moveTime.MoveNext();
                moveAction.MoveNext();
                speedRanges.MoveNext();

                moveSuccess=true;//initialzes the move's success to true
                time = moveTime.Current.GetEnumerator();//points the iterator into the moveTime list
                speedRange = speedRanges.Current.GetEnumerator();//points the iterator into the speedRanges list
                foreach (Point step in move)
                {
                    //Inner iterations
                    toTestTime.MoveNext();
                    toTestAction.MoveNext();
                    toTestSpeed.MoveNext();
                    time.MoveNext();
                    speedRange.MoveNext();

                    //Check if the flow has the correct x position in the grid
                    if (step.X != -2 && step.X != toTestAction.Current.X)
                    {
                        moveSuccess = false;
                        break;
                    }

                    //Check if the flow has the correct y position in the grid
                    if (step.Y != -2 && step.Y != toTestAction.Current.Y)
                    {
                        moveSuccess = false;
                        break;
                    }

                    //Check if the flow has the minimum required time for move to occur
                    if (time.Current.X != -2 && time.Current.X > toTestTime.Current)
                    {
                        moveSuccess = false;
                        break;
                    }

                    //Check if the flow has the maximum required time for the move to occur
                    if (time.Current.Y != -2 && time.Current.Y < toTestTime.Current)
                    {
                        moveSuccess = false;
                        break;
                    }

                    //Check if the flow has the minimum required speed on the x axis
                    if (speedRange.Current[0].X != -2 && speedRange.Current[0].X > toTestSpeed.Current.X)
                    {
                        moveSuccess = false;
                        break;
                    }

                    //Check if the flow has the maximum required speed on the x axis
                    if (speedRange.Current[1].X != -2 && speedRange.Current[1].X < toTestSpeed.Current.X)
                    {
                        moveSuccess = false;
                        break;
                    }

                    //Check if the flow has the minimum required speed on the y axis
                    if (speedRange.Current[0].Y != -2 && speedRange.Current[0].Y > toTestSpeed.Current.Y)
                    {
                        moveSuccess = false;
                        break;
                    }

                    //Check if the flow has the maximum required speed on the y axis
                    if (speedRange.Current[1].Y != -2 && speedRange.Current[1].Y < toTestSpeed.Current.Y)
                    {
                        moveSuccess = false;
                        break;
                    }
                }

                //Return action if a matching move is found
                if (moveSuccess)
                {
                    return moveAction.Current;
                }

                //someFlow iterator resets
                toTestTime.Reset();
                toTestAction.Reset();
                toTestSpeed.Reset();
            }
            return 0;
        }
    }
}
