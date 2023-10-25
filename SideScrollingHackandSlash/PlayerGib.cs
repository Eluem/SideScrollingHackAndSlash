//******************************************************
// File: PlayerGib.cs
// Purpose: Contains the class definition of
// PlayerGib.
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
    class PlayerGib:ThrowableObject
    {
        int lifeTime;
        int soundsLeft;
        int soundTimer;
        //Cue gibSound;

        //****************************************************
        // Method: PlayerGib
        //
        // Purpose: PlayerGib constructor
        //****************************************************
        public PlayerGib(Vector2 Position)
            : base("PlayerGib", -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), 15F, .5F, new Vector2(0, 0), .5F, .4F)
        {
            rect.Width = 15;
            rect.Height = 15;
            speed.X = GlobalVariables.Randomizer.Next(-500, 500);
            speed.Y = GlobalVariables.Randomizer.Next(-900, -100);
            lifeTime = GlobalVariables.Randomizer.Next(6000, 10000);
            soundTimer = 0;
            soundsLeft = 2;
            //gibSound = GlobalVariables.SoundBank.GetCue("GoreImpact");
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

            soundTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (soundTimer <= 0 && soundsLeft < 2)
            {
                ++soundsLeft;
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
            if (!(trueObjPointer is PlayerGib))
            {
                Vector2 tempSpeed = speed;
                base.Collide(obj, trueObjPointer, overLap, gameTime);
                Vector2 speedChangeCheck = new Vector2(Math.Abs(tempSpeed.X - speed.X), Math.Abs(tempSpeed.Y - speed.Y));

                if ((speedChangeCheck.X > 0 || speedChangeCheck.Y > 0) && soundsLeft > 0)
                {
                    GlobalVariables.SoundBank.PlayCue("GoreImpact");
                    --soundsLeft;
                    soundTimer = 1000;
                }
            }

            /*
            float tempFloat;
            if (speedChangeCheck.X > 0)
            {
                tempFloat = speedChangeCheck.X;
            }
            else
            {
                tempFloat = speedChangeCheck.Y;
            }
            if (tempFloat > 100)
            {
                tempFloat = 100;
            }
            gibSound.SetVariable("MyVolume", tempFloat);
            if (gibSound.IsStopped)
            {
                gibSound.Dispose();
                gibSound = GlobalVariables.SoundBank.GetCue("GoreImpact");
            }
            if (!gibSound.IsPlaying)
            {
                gibSound.Play();
            }
            */
        }
    }
}
