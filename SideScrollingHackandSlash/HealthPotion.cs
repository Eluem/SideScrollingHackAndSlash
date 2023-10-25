﻿//******************************************************
// File: HealthPotion.cs
//
// Purpose: Contains the class definition of
// HealthPotion. HealthPotion is an object that will
// heal a player by up to 50 points of health after
// being used.
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
    class HealthPotion : ThrowableObject
    {

        //****************************************************
        // Method: HealthPotion
        //
        // Purpose: Constructor for HealthPotion
        //****************************************************
        public HealthPotion(Vector2 Pos, Vector2 Speed)
            : base("healthPotion", -1, -1, Pos, Speed, new Vector2(0, 0), 20F, .2F, new Vector2(0, 0), .4F, 1)
        {
            myState = ThrowableState.Normal;
        }


        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            if (grabber != null && grabber.Grabbing == playerGrab.Using)
            {
                Drink();
            }
            base.Update(gameTime, port);
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            if (!(trueObjPointer is Explosion))
            {
                base.Collide(obj, trueObjPointer, overLap, gameTime);
            }
            else
            {
                Delete();
            }
        }


        //****************************************************
        // Method: Drink
        //
        // Purpose: When a player uses a potion, this is
        // what's called. It handles the details of a player
        // drinking a potion.
        //****************************************************
        public void Drink()
        {
            grabber.Health += 50;
            if (grabber.Health > 100)
            {
                grabber.Health = 100;
            }
            Delete();
        }
    }
}
