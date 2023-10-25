//******************************************************
// File: LightningBolt.cs
// Purpose: Contains the class definition of
// LightningBolt. LightningBolt is designed as a spell
// that the player can cast.
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
    class LightningBolt : PhysicsObject
    {
        int ownerSafeTimer;
        PlayerObject ownerPointer;
        Facing direction;
        int lifeTime;
        //****************************************************
        // Method: LightningBolt
        //
        // Purpose: LightningBolt constructor
        //****************************************************
        public LightningBolt(PlayerObject OwnerPointer)
            : base("LightningBolt", true, -1, OwnerPointer.ObjectID, Vector2.Zero, new Vector2(0, 0), new Vector2(0, 0), .001F, .5F, new Vector2(0, 0), .5F, .4F)
        {
            ownerPointer = OwnerPointer;
            speed.X = Speed.X;
            pos.Y = ownerPointer.Pos.Y + 10;
            pos.X = ownerPointer.Pos.X;
            direction = ownerPointer.FacingDirection;
            if (direction == Facing.right)
            {
                speed.X += 1200;
                pos.X += ownerPointer.Rect.Width;
            }
            else
            {
                speed.X -= 1200;
                pos.X -= rect.Width;
            }

            ownerSafeTimer = 600;
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
                if (trueObjPointer is BombFire || trueObjPointer is Explosion || trueObjPointer is MeleeSlash001)
                {
                    baseCollideAnyway = false;
                }
                if (trueObjPointer is PlayerObject && ownerSafeTimer > 0 && ((PlayerObject)trueObjPointer).Equals(ownerPointer))
                {
                    baseCollideAnyway = false;
                }
                if (trueObjPointer is ThrowableObject)
                {
                    if (((ThrowableObject)trueObjPointer).Grabber != null)
                    {
                        baseCollideAnyway = false;
                    }
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
        // Method: Draw
        //
        // Purpose: Calls the draw functions of the sprites
        // that the object contains.
        //****************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            image.UpdatePos(pos);
            image.Draw(spriteBatch, layer, direction);
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            lifeTime += gameTime.ElapsedGameTime.Milliseconds;
            ownerSafeTimer -= gameTime.ElapsedGameTime.Milliseconds;

            sided[side.bottom] = true;
            base.Update(gameTime, port);
        }

        //Properties
        public PlayerObject OwnerPointer
        {
            get
            {
                return ownerPointer;
            }
        }

        public int OwnerSafeTimer
        {
            get
            {
                return ownerSafeTimer;
            }
        }

        public int LifeTime
        {
            get
            {
                return lifeTime;
            }
        }
        public Facing Direction
        {
            get
            {
                return direction;
            }
        }
    }
}
