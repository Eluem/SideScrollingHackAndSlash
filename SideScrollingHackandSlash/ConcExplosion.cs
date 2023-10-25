//******************************************************
// File: ConcExplosion.cs
// Purpose: Contains the class definition of
// ConcExplosion. To act as the non-damaging explosion
// of a ConcGrenade. (Conc stands for concussion)
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
    class ConcExplosion : PhysicsObject
    {
        Sprite explosionSprite;
        int lifeTime;
        //****************************************************
        // Method: ConcExplosion
        //
        // Purpose: ConcExplosion constructor
        //****************************************************
        public ConcExplosion(Vector2 Position)
            : base("NukeExplosion", true, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), .001F, .5F, new Vector2(0, 0), .5F, .4F)
        {
            explosionSprite = new Sprite(50, new Point((int)Position.X, (int)Position.Y), "NukeExplosion", 5F, new Point(40, 40), new Point(13, 1));
            rect.Width = 200;
            rect.Height = 200;
            pos.X -= 100;
            pos.Y -= 100;
            rect.X = pos.X;
            rect.Y = pos.Y;
            GridLocationUpdate();
            lifeTime = 50*13;
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
            lifeTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (lifeTime <= 0)
            {
                Delete();
            }
            explosionSprite.Update(gameTime);
            //base.Update(gameTime, port);
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Calls the draw functions of the sprites
        // that the object contains.
        //****************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            explosionSprite.UpdatePos((int)pos.X, (int)pos.Y);
            explosionSprite.Draw(spriteBatch, layer);
        }
    }
}
