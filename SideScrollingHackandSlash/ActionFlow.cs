//******************************************************
// File: ActionFlow.cs
//
// Purpose: Contains the class definition of
// ActionFlow. ActionFlow contains
// member variables and functions to interact with
// a FlowList for a character.
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
    class ActionFlow
    {
        ThumbstickTrackingTest test;
        protected FlowList leftThumbStick;  //Stores the FlowList of the left stick
        protected FlowList rightThumbStick; //Stores the FlowList of the right stick
        protected MoveList leftMoveList;    //Stores the object that can output the appropriate moves for the left stick
        protected MoveList rightMoveList;   //Stores the object that can output the appropriate moves for the right stick
        
        //****************************************************
        // Method: ActionFlow
        //
        // Purpose: Constructor for ActionFlow class
        //****************************************************
        public ActionFlow()
        {
            test = new ThumbstickTrackingTest(Vector2.Zero, Vector2.Zero);

            leftThumbStick = new FlowList();
            rightThumbStick = new FlowList();
            leftMoveList = new MoveListMovements();
            rightMoveList = new MoveListAttacks();
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: To update the player's current thumb
        // stick flows
        //****************************************************
        public void Update(GameTime gameTime, GamePadState gamePadState)
        {
            //Updating the information for the flow of both sticks
            leftThumbStick.UpdateStick(gameTime, gamePadState.ThumbSticks.Left);

            test.Update(gameTime, gamePadState.ThumbSticks.Right);
            //rightThumbStick.UpdateStick(gameTime, gamePadState.ThumbSticks.Right);
            rightThumbStick.UpdateStick(gameTime, test.ShadowThumbStick);
        }

        //****************************************************
        // Method: ActionCheckLeft
        //
        // Purpose: To find if any actions occurred and
        // report them back to the control function
        // so that they can be responded to.
        //****************************************************
        public Action leftAction()
        {
            if (leftThumbStick.Changed)
            {
                return (Action)leftMoveList.MoveCheck(leftThumbStick);
            }
            return Action.noAction;
        }

        //****************************************************
        // Method: ActionCheckRight
        //
        // Purpose: To find if any actions occurred and
        // report them back to the control function
        // so that they can be responded to.
        //****************************************************
        public Attacks rightAction()
        {
            if (rightThumbStick.Changed)
            {
                return (Attacks)rightMoveList.MoveCheck(rightThumbStick);
            }
            return Attacks.noAction;
        }

        
        public Point CurrentActionPosLeft
        {
            get
            {
                return leftThumbStick.CurrentActionPos;
            }
        }

        public Point CurrentActionPosRight
        {
            get
            {
                return rightThumbStick.CurrentActionPos;
            }
        }
    }
}
