//******************************************************
// File: CollideCheckReturn.cs
//
// Purpose: Contains the class definition of
// CollideCheckReturn. CollideCheckReturn is used to
// store 2 pointers and a deep clone of an object
// as well as a boolean value and an overlap
// to assit in collision detection.
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
    class CollideCheckReturn
    {
        private Boolean collided; //Used to check if the objects collided
        private Vector2 overLap;  //Used for the purpose of handling the collision
        private PhysicsObject primaryObjectPointer; //Used to modify an object to react to the collision
        private PhysicsObject primaryObjectClone; //Used for the purpose of consistency
        private PhysicsObject secondaryObjectClone; //Used for the purpose of consistency
        private PhysicsObject secondaryObjectPointer; //Used to modify an object to react to the collision

        //****************************************************
        // Method: CollideCheckReturn
        //
        // Purpose: Constructor for CollideCheckReturn
        //****************************************************
        public CollideCheckReturn(Boolean Collided, Vector2 OverLap, PhysicsObject PrimaryObjectClone, PhysicsObject PrimaryObjectPointer, PhysicsObject SecondaryObjectClone, PhysicsObject SecondaryObjectPointer)
        {
            collided = Collided;
            overLap = OverLap;
            primaryObjectPointer = PrimaryObjectPointer;
            primaryObjectClone = PrimaryObjectClone;
            secondaryObjectClone = SecondaryObjectClone;
            secondaryObjectPointer = SecondaryObjectPointer;
        }

        //Properties

        public Boolean Collided
        {
            get
            {
                return collided;
            }
        }

        public Vector2 Overlap
        {
            get
            {
                return overLap;
            }
        }

        public PhysicsObject PrimaryObjectClone
        {
            get
            {
                return primaryObjectClone;
            }
        }

        public PhysicsObject PrimaryObjectPointer
        {
            get
            {
                return primaryObjectPointer;
            }
        }

        public PhysicsObject SecondaryObjectClone
        {
            get
            {
                return secondaryObjectClone;
            }
        }

        public PhysicsObject SecondaryObjectPointer
        {
            get
            {
                return secondaryObjectPointer;
            }
        }

    }
}
