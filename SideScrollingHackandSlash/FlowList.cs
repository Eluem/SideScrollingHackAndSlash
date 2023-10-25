//******************************************************
// File: FlowList.cs
//
// Purpose: Contains the class definition of
// FlowList. FlowList contains
// member variables and functions to store the
// complexities of an 'action flow' as defined by me.
// (See DevThoughts.doc for a description)
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
    //Position enumerations for the position grid (only really used here because of the fact that the position
    //is stored in a Point).
    enum actionPosX{Left,Mid,Right};
    enum actionPosY{High,Mid,Low};
    class FlowList
    {

        protected const int maxFlowLength = 5;   //Maximum length that a flow can be
        protected int lastTime;                            //Accumulates the time that will be entered
                                                 //into the timeFlow when a change in grid
                                                 //positions occurs
        
        protected List<Point> positionFlow; //Stores the grid position of the thumb stick

        protected List<int> timeFlow;       //Stores the time it took to change grid positions for the thumb stick

        protected List<Vector2> speedFlow;  //Stores the speed that the player changed grid positions for the thumb stick
        protected Boolean changed;             //Stores whether or not the current action list has been changed

        protected Vector2 lastPosition;      //Stores the position of the thumbstick temporarily
                                             //so that it may be used to preserve the total change in
                                             // thumbstick movement for use in the speedFlow

        protected Vector2 startPosition;     //Stores the position at the start of the movement, for the purpose of calculating the total change
        protected int changeTime;            //Stores the total time the thumbstick has been moving for, so that I can calculate speed;
        protected int lewayTime;             //Stores the time that the thumbstick has been sitting still for (Milliseconds)
        protected int maxLewayTime;          //maximum time that the player can leave the stick sitting still (Milliseconds)

        protected float midBoxX;             //stores the size of the center box of the grid (x and y are the same)

        protected Point currentActionPos;       //Stores the current position of the stick in my grid

        //****************************************************
        // Method: FlowList
        //
        // Purpose: FlowList constructor
        //****************************************************
        public FlowList()
        {
            //positionFlow initialization
            positionFlow = new List<Point>();
            positionFlow.Add(new Point(-1, -1));
            positionFlow.Add(new Point(-1, -1));
            positionFlow.Add(new Point(-1, -1));
            positionFlow.Add(new Point(-1, -1));
            positionFlow.Add(new Point(-1, -1));

            //timeFlow initializations
            timeFlow = new List<int>();
            timeFlow.Add(0);
            timeFlow.Add(0);
            timeFlow.Add(0);
            timeFlow.Add(0);
            timeFlow.Add(0);

            //speedFLow initializations
            speedFlow = new List<Vector2>();
            speedFlow.Add(new Vector2(0, 0));
            speedFlow.Add(new Vector2(0, 0));
            speedFlow.Add(new Vector2(0, 0));
            speedFlow.Add(new Vector2(0, 0));
            speedFlow.Add(new Vector2(0, 0));

            //initalize variables for finding total change
            lastPosition = new Vector2(0, 0);
            startPosition = new Vector2(0, 0);
            lewayTime = 0;
            maxLewayTime = 100;
            changeTime = 0;

            midBoxX = .325F; //Sets the size of the middle box, which causes all the other parts of the grid
                                   //to conform

            currentActionPos = new Point(-1, -1); //Initializes currentActionPos to be outside of the grid, as to prevent
                                                  //any issues

            changed = false; //initialize changed to false
            lastTime = 0; //initialize the lastTime to 0
        }



        //****************************************************
        // Method: UpdateStick
        //
        // Purpose: To update the current flow based on the
        // floating point passed down, thus abstracting
        // this code from the ActionFlow.
        //****************************************************
        public void UpdateStick(GameTime gameTime, Vector2 thumbStick)
        {
            changeTime += gameTime.ElapsedGameTime.Milliseconds; //Increments changeTime
            //If the thumbstick didn't move, start counting the timer
            if (lastPosition == thumbStick)
            {
                lewayTime += gameTime.ElapsedGameTime.Milliseconds; //counts the lewayTime because the player
                                                                    //isn't moving the stick
                if (lewayTime >= maxLewayTime)
                {
                    //Updates the startPosition so that when the player tries to do an action the distance is
                    //calculated properly
                    startPosition.X = thumbStick.X;
                    startPosition.Y = thumbStick.Y;


                    changeTime = 0; //resets the changeTime because the player is clearly not attempting to do an action
                    lewayTime = 0;  //resets the lewayTime
                }
            }
            else
            {
                lewayTime = 0; //resets the lewayTime because the user moved the stick
            }
            //updates the last position
            lastPosition.X = thumbStick.X;
            lastPosition.Y = thumbStick.Y;

            //Finds the currentActionPos.X position in my grid
            if (thumbStick.X <= midBoxX && thumbStick.X >= -midBoxX)
            {
                currentActionPos.X = (int)actionPosX.Mid;
            }
            else if (thumbStick.X < -midBoxX)
            {
                currentActionPos.X = (int)actionPosX.Left;
            }
            else if (thumbStick.X > midBoxX)
            {
                currentActionPos.X = (int)actionPosX.Right;
            }


            //Finds the currentActionPos.Y position in my grid
            if (thumbStick.Y <= midBoxX && thumbStick.Y >= -midBoxX)
            {
                currentActionPos.Y = (int)actionPosY.Mid;
            }
            else if (thumbStick.Y < -midBoxX)
            {
                currentActionPos.Y = (int)actionPosY.Low;
            }
            else if (thumbStick.Y > midBoxX)
            {
                currentActionPos.Y = (int)actionPosY.High;
            }

            //Updates the current FlowList
            Update(gameTime, thumbStick);
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: To update the current flow of a
        // thumbstick
        //****************************************************
        public void Update(GameTime gameTime, Vector2 thumbStick)
        {
            changed = false;
            lastTime += gameTime.ElapsedGameTime.Milliseconds; //Increments timer
            if (positionFlow[positionFlow.Count - 1] != currentActionPos) //Checks for change
            {
                changed = true; //Sets change flag to true
                positionFlow.Remove(positionFlow[0]); //Removes oldest position
                timeFlow.Remove(timeFlow[0]);         //Removes oldest time
                speedFlow.Remove(speedFlow[0]);       //removes oldest speed


                positionFlow.Add(currentActionPos);    //adds new position
                timeFlow.Add(lastTime);                //adds new time

                //adds new speed
                speedFlow.Add(new Vector2(Math.Abs(thumbStick.X - startPosition.X) / changeTime, Math.Abs(thumbStick.Y - startPosition.Y) / changeTime));
                
                //Resets timers
                changeTime = 0;
                lewayTime = 0;
                lastTime = 0;
            }
        }

        //Properties
        public List<Point> PositionFlow
        {
            get
            {
                return positionFlow;
            }
        }

        public List<int> TimeFlow
        {
            get
            {
                return timeFlow;
            }
        }

        public List<Vector2> SpeedFlow
        {
            get
            {
                return speedFlow;
            }
        }

        public Boolean Changed
        {
            get
            {
                return changed;
            }
        }

        public Point CurrentActionPos
        {
            get
            {
                return currentActionPos;
            }
        }
    }
}
