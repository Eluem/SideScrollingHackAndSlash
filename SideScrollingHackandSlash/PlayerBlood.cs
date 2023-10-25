//******************************************************
// File: PlayerBlood.cs
// Purpose: Contains the class definition of
// PlayerBlood. Which is used as a visual for blood
// spirts.
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
    class PlayerBlood:PhysicsObject
    {
        int lifeTime;

        //****************************************************
        // Method: PlayerBlood
        //
        // Purpose: PlayerBlood constructor
        //****************************************************
        public PlayerBlood(Vector2 Position)
            : base("PlayerGib", true, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), 15F, .5F, new Vector2(0, 0), .5F, .4F)
        {
            rect.Width = 15;
            rect.Height = 15;
            speed.X = GlobalVariables.Randomizer.Next(-500, 500);
            speed.Y = GlobalVariables.Randomizer.Next(-900, -100);
            lifeTime = GlobalVariables.Randomizer.Next(100, 500);
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            lifeTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (lifeTime <= 0)
            {
                Delete();
            }
            base.Update(gameTime, port);
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collisions
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
        }
    }
}
