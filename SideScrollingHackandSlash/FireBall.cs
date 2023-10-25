//******************************************************
// File: FireBall.cs
// Purpose: Contains the class definition of
// FireBall. FireBall is designed as a spell that the
// player can cast.
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
    class FireBall : ThrowableObject
    {
        Sprite bombFireSprite;
        int ownerSafeTimer;
        PlayerObject ownerPointer;
        int lifeTime;
        //****************************************************
        // Method: FireBall
        //
        // Purpose: FireBall constructor
        //****************************************************
        public FireBall(PlayerObject OwnerPointer)
            : base("BombFire", -1, OwnerPointer.ObjectID, Vector2.Zero, new Vector2(0, 0), new Vector2(0, 0), .001F, .5F, new Vector2(0, 0), .5F, .4F)
        {
            ownerPointer = OwnerPointer;
            bombFireSprite = new Sprite(100, new Point((int)ownerPointer.Pos.X, (int)ownerPointer.Pos.Y), "BombFire", 1F, new Point(32, 32), new Point(3, 2));
            rect.Width = 30;
            rect.Height = 26;
            grabOffest.Y = +6;
            pos.Y = ownerPointer.Pos.Y;
            pos.X = ownerPointer.Pos.X;


            grabber = ownerPointer;
            myState = ThrowableState.Grabbed;
            ownerPointer.Grabbing = playerGrab.PickingUp;

            ownerSafeTimer = 600;
            lifeTime = 2000;
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
                if (trueObjPointer is BombFire || trueObjPointer is Explosion || trueObjPointer is MeleeSlash001 || trueObjPointer is GrabCollisionBox)
                {
                    baseCollideAnyway = false;
                }
                if (trueObjPointer is PlayerObject && ownerSafeTimer > 0 && ((PlayerObject)trueObjPointer).Equals(ownerPointer))
                {
                    baseCollideAnyway = false;
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
            if (myState != ThrowableState.Grabbed)
            {
                ownerSafeTimer -= gameTime.ElapsedGameTime.Milliseconds;
                lifeTime -= gameTime.ElapsedGameTime.Milliseconds;
            }
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

        //Properties
        public PlayerObject OwnerPointer
        {
            get
            {
                return ownerPointer;
            }
            set
            {
                ownerPointer = value;
            }
        }

        public int OwnerSafeTimer
        {
            get
            {
                return ownerSafeTimer;
            }
            set
            {
                ownerSafeTimer = value;
            }
        }

        public int LifeTime
        {
            get
            {
                return lifeTime;
            }
            set
            {
                lifeTime = value;
            }
        }
    }
}
