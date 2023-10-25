//******************************************************
// File: FadingWall.cs
// Purpose: Contains the class definition of
// FadingWall. Fading wall is a test attempt at
// creating an object that the player can spawn
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
    class FadingWall:PhysicsObject
    {
        int timeToLive; //Stores the time the object will last before being deleted

        //****************************************************
        // Method: FadingWall
        //
        // Purpose: FadingWall constructor
        //****************************************************
        public FadingWall(Vector2 Position, int TimeToLive)
            : base("small_platform", false, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), 999999999999F, .5F, new Vector2(0, 0), 0F, .4F)
        {
            timeToLive = TimeToLive;
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            timeToLive -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeToLive <= 0)
            {
                Delete();
            }
        }

    }
}
