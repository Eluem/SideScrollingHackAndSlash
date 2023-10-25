//******************************************************
// File: Platform001.cs
// Purpose: Contains the class definition of
// Platform001. Platform001 is a basic platform that
// doesn't move and can be collided with.
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
    class Platform001:PhysicsObject
    {
        //****************************************************
        // Method: Platform001
        //
        // Purpose: Platform001 constructor
        //****************************************************
        public Platform001(Vector2 Position)
            : base("platform_flat_long", false, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), 9999999999999F, .001F, new Vector2(0, 0), 0F, .4F)
        {
            GridLocationUpdate();
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
        }

    }
}
