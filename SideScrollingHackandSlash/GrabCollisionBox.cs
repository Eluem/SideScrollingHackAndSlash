//******************************************************
// File: GrabCollisionBox.cs
// Purpose: Contains the class definition of
// GrabCollisionBox. GrabCollisionBox will be used
// to detect collisions of a player's grab attempts.
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
    class GrabCollisionBox:PhysicsObject
    {
        public PlayerObject ownerPointer; //Pointer to the owner of the slash
        public GrabCollisionBox(PlayerObject OwnerPointer):base("", true, -1, OwnerPointer.ObjectID, new Vector2(0,0), new Vector2(0,0), new Vector2(0,0), 0F, 0F, new Vector2(0,0), 0F, 0F)
        {
            ownerPointer = OwnerPointer;
            rect.Width = 15;
            rect.Height = OwnerPointer.Rect.Height;
            pos.X = ownerPointer.Pos.X;
            pos.Y = ownerPointer.Pos.Y;
            if (ownerPointer.FacingDirection == Facing.right)
            {
                pos.X += ownerPointer.Rect.Width;
            }
            else
            {
                pos.X -= rect.Width;
            }
            rect.X = pos.X;
            rect.Y = pos.Y;
            GridLocationUpdate();
        }

        public override void Update(GameTime gameTime, Viewport port)
        {
           //base.Update(gameTime, port);
            Delete();
        }

        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            if (trueObjPointer is ThrowableObject)
                {
                    ThrowableObject temp = ((ThrowableObject)trueObjPointer);
                    if (temp.Grabber == null)
                    {
                        if (ownerPointer.Grabbing == playerGrab.Grabbing)
                        {
                            temp.Grabber = ownerPointer;
                            temp.MyState = ThrowableState.Grabbed;
                            ownerPointer.Grabbing = playerGrab.PickingUp;
                            temp.OldGrabber = null;
                            temp.OldGrabberCollidedLastFrame = false;
                        }

                        if (temp is FireBall)
                        {
                            FireBall tempFireBall = ((FireBall)temp);
                            tempFireBall.OwnerPointer = ownerPointer;
                            tempFireBall.LifeTime = 2000;
                            tempFireBall.OwnerSafeTimer = 600;
                            tempFireBall.OwnerID = ownerID;
                        }
                    }
                }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
        }
    }
}
