//******************************************************
// File: ThrowableObject.cs
//
// Purpose: Contains the class definition of
// ThrowableObject. ThrowableObject is a test object
// that a player can grab, put down, and throw.
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
    enum ThrowableState { Normal, Grabbed, Thrown };
    class ThrowableObject : PhysicsObject
    {
        protected ThrowableState myState; //Stores the current state of the throwable object
        protected PlayerObject grabber; //Stores a pointer to the player holding the throwable object
        protected PlayerObject oldGrabber; //Stores a pointer to the player that just dropped/threw this object
                                           //so that it can be checked to during collisions until the throwable
                                           //object stops colliding to it for a frame, before it can collide after being thrown
        protected Boolean oldGrabberCollidedLastFrame; //Stores a flag stating that this object collided with the
                                                       //player that threw it in the last collide check.
                                                       //until this is false, oldGrabber will stay allocated
                                                       //and this object will not calculate collisions with the player

        protected Vector2 grabOffest; //Stores the offset of the grab
                                                 

        public ThrowableObject(String Image, int ObjectID, int OwnerID, Vector2 Position, Vector2 Speed, Vector2 Accel, float Mass, float Friction, Vector2 AirResistance, float Bounciness, float Layer)
            : base(Image, true, ObjectID, OwnerID, Position, Speed, Accel, Mass, Friction, AirResistance, Bounciness, 0F) //Forced Layer
        {
            myState = ThrowableState.Normal;
            grabOffest = Vector2.Zero;
        }

        public override void Update(GameTime gameTime, Viewport port)
        {
            if (!oldGrabberCollidedLastFrame)
            {
                oldGrabber = null;
            }
            if (myState == ThrowableState.Grabbed)
            {
                speed.X = 0;
                speed.Y = 0;
                if (grabber.FacingDirection == Facing.left)
                {
                    pos.X = grabber.Pos.X - rect.Width + grabOffest.X;
                    pos.Y = grabber.Pos.Y + grabOffest.Y;
                }
                else
                {
                    pos.X = grabber.Pos.X + grabber.Rect.Width + grabOffest.X;
                    pos.Y = grabber.Pos.Y + grabOffest.Y;
                }
                if (grabber.Grabbing == playerGrab.Throwing)
                {
                    if (grabber.FacingDirection == Facing.right)
                    {
                        speed.X = 600 * MathHelper.Clamp(grabber.GamePadState.ThumbSticks.Right.X, 0F, 1F) + grabber.Speed.X;
                        speed.Y = -600 * MathHelper.Clamp(grabber.GamePadState.ThumbSticks.Right.Y, -1F, 1F) + grabber.Speed.Y;
                    }
                    else
                    {
                        speed.X = 600 * MathHelper.Clamp(grabber.GamePadState.ThumbSticks.Right.X, -1F, 0F) + grabber.Speed.X;
                        speed.Y = -600 * MathHelper.Clamp(grabber.GamePadState.ThumbSticks.Right.Y, -1F, 1F) + grabber.Speed.Y;
                    }
                    myState = ThrowableState.Thrown;
                    oldGrabber = grabber;
                    oldGrabberCollidedLastFrame = true;
                    grabber = null;
                }
                else if (grabber.Grabbing == playerGrab.Dropping || grabber.Deleted)
                {
                    speed.X = grabber.Speed.X;
                    speed.Y = grabber.Speed.Y;
                    myState = ThrowableState.Normal;
                    grabber = null;
                }
                GridLocationUpdate();
                rect.X = pos.X;
                rect.Y = pos.Y;
            }
            else
            {
                base.Update(gameTime, port);
            }
            oldGrabberCollidedLastFrame = false;
        }

        public override void Delete()
        {
            if (grabber != null)
            {
                grabber.Grabbing = playerGrab.Nothing;
            }
            base.Delete();
        }

        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            if (grabber == null)
            {
                if (!trueObjPointer.Equals(grabber) && !trueObjPointer.Equals(oldGrabber))
                {
                    base.Collide(obj, trueObjPointer, overLap, gameTime);
                }
                if (trueObjPointer.Equals(oldGrabber))
                {
                    oldGrabberCollidedLastFrame = true;
                }
            }
            //TEST CLUDGE
            else if(trueObjPointer != grabber && !(trueObjPointer is MeleeSlash001 && ((MeleeSlash001)trueObjPointer).OwnerPointer == grabber))
            {
                if (!(overLap.Y < overLap.X && rect.Center.Y < obj.Rect.Center.Y))
                {
                    grabber.Collide(obj, trueObjPointer, overLap, gameTime);
                }
            }
        }

        public PlayerObject Grabber
        {
            get
            {
                return grabber;
            }
            set
            {
                grabber = value;
            }
        }
        public PlayerObject OldGrabber
        {
            get
            {
                return oldGrabber;
            }
            set
            {
                oldGrabber = value;
            }
        }

        public Boolean OldGrabberCollidedLastFrame
        {
            get
            {
                return oldGrabberCollidedLastFrame;
            }
            set
            {
                oldGrabberCollidedLastFrame = value;
            }
        }

        public ThrowableState MyState
        {
            get
            {
                return myState;
            }
            set
            {
                myState = value;
            }
        }

    }
}
