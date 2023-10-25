//******************************************************
// File: BombFire.cs
// Purpose: Contains the class definition of
// BombFire. To act as the little fire balls that come
// out of bombs.
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
    class BombFire : PhysicsObject
    {
        Sprite bombFireSprite;
        int lifeTime;
        //****************************************************
        // Method: BombFire
        //
        // Purpose: BombFire constructor
        //****************************************************
        public BombFire(Vector2 Position)
            : base("BombFire", true, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), 10F, .5F, new Vector2(0, 0), .5F, .4F)
        {
            bombFireSprite = new Sprite(100, new Point((int)Position.X, (int)Position.Y), "BombFire", .5F, new Point(32, 32), new Point(3, 2));
            rect.Width = 15;
            rect.Height = 13;
            speed.X = GlobalVariables.Randomizer.Next(-800, 800);
            speed.Y = GlobalVariables.Randomizer.Next(-300, 100);
            lifeTime = GlobalVariables.Randomizer.Next(300, 600);
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            Boolean baseCollideAnyway = true;
            if (obj.SpecialProperties)
            {
                if (trueObjPointer is BombFire || trueObjPointer is Explosion)
                {
                    baseCollideAnyway = false;
                }
                else if(trueObjPointer is ConcExplosion)
                {
                    baseCollideAnyway = false;
                    base.Collide(obj, trueObjPointer, overLap, gameTime);
                }
            }
            if (baseCollideAnyway)
            {
                //base.Collide(obj, trueObjPointer, overLap, gameTime);
                if (!obj.Deleted)
                {
                    Delete();
                }
            }
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
            bombFireSprite.Update(gameTime);
            base.Update(gameTime, port);
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Calls the draw functions of the sprites
        // that the object contains.
        //****************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            bombFireSprite.UpdatePos((int)pos.X, (int)pos.Y);
            bombFireSprite.Draw(spriteBatch, layer);
        }
    }
}
