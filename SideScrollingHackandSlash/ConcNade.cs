//******************************************************
// File: ConcNade.cs
//
// Purpose: Contains the class definition of
// ConcNade. ConcNade is an object that will explode
// 3 seconds after the player actives it.
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
    //    enum ThrowableState { Normal, Grabbed, Thrown };
    class ConcNade : ThrowableObject
    {
        Boolean concNadeActivate;
        Boolean fuseBurningAlready;
        Cue fuseSound;
        int concNadeTimer;

        //****************************************************
        // Method: ConcNade
        //
        // Purpose: Constructor for ConcNade
        //****************************************************
        public ConcNade(Vector2 Pos, Vector2 Speed)
            : base("Grenade", -1, -1, Pos, Speed, new Vector2(0, 0), 20F, .2F, new Vector2(0, 0), .4F, 1)
        {
            myState = ThrowableState.Normal;
            concNadeTimer = 3000;
            concNadeActivate = false;
            fuseBurningAlready = false;
            fuseSound = GlobalVariables.SoundBank.GetCue("BombTimer");
        }

        //********************************************************
        // Method: Explode
        //
        // Purpose: When the concNade's fuse runs out this
        // this function is called. This function is designed
        // to handle all the details of how the concNade should
        // act when it explodes.
        //********************************************************
        public void Explode()
        {
            GlobalVariables.AddList.Add(new ConcExplosion(pos));
            GlobalVariables.SoundBank.PlayCue("Explosion Sound 001");
            Delete();
        }


        //****************************************************
        // Method: Delete
        //
        // Purpose: Handles deleting
        //****************************************************
        public override void Delete()
        {
            fuseSound.Stop(AudioStopOptions.Immediate);
            base.Delete();
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
                concNadeActivate = true;
            }
            if (concNadeActivate)
            {
                if (!fuseBurningAlready)
                {
                    try
                    {
                        fuseSound.Play();
                    }
                    catch (Exception e)
                    {
                    }
                    fuseBurningAlready = true;
                }
                concNadeTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }
            if (concNadeTimer <= 0)
            {
                Explode();
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
            if (trueObjPointer is Explosion && !deleted)
            {
                Explode();
            }
            else if (trueObjPointer is LightningBolt)
            {
                concNadeActivate = true;
            }
            base.Collide(obj, trueObjPointer, overLap, gameTime);
        }
    }
}
