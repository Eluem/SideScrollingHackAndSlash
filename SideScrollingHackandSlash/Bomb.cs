//******************************************************
// File: Bomb.cs
//
// Purpose: Contains the class definition of
// Bomb. Bomb is an object that will explode 3 seconds
// after the player lights the fuse.
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
    class Bomb : ThrowableObject
    {
        Sprite bombSprite;
        Boolean bombActivate;
        Boolean fuseBurningAlready;
        Cue fuseSound;
        int bombTimer;

        //****************************************************
        // Method: Bomb
        //
        // Purpose: Constructor for Bomb
        //****************************************************
        public Bomb(Vector2 Pos, Vector2 Speed)
            : base("", -1, -1, Pos, Speed, new Vector2(0, 0), 20F, .2F, new Vector2(0, 0), .4F, 1)
        {
            myState = ThrowableState.Normal;
            rect.Width = 26.5F;
            rect.Height = 33.5F;
            bombSprite = new Sprite(750, new Point(0,0), "DABOMB", .5F, new Point(53,67), new Point(5,1));
            bombTimer = 3000;
            bombActivate = false;
            fuseBurningAlready = false;
            fuseSound = GlobalVariables.SoundBank.GetCue("FuseBurning");
        }

        //****************************************************
        // Method: Explode
        //
        // Purpose: When the bomb's fuse runs out this
        // this function is called. This function is designed
        // to handle all the details of how the bomb should
        // act when it explodes.
        //****************************************************
        public void Explode()
        {
            for (int i = 0; i < 25; ++i)
            {
                GlobalVariables.AddList.Add(new BombFire(pos));
            }
            GlobalVariables.AddList.Add(new Explosion(pos));
            GlobalVariables.SoundBank.PlayCue("Explosion Sound 001");
            Delete();

        }


        //****************************************************
        // Method: Draw
        //
        // Purpose: Calls the draw functions of the sprites
        // that the object contains.
        //****************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            bombSprite.UpdatePos((int)pos.X, (int)pos.Y);
            bombSprite.Draw(spriteBatch, layer);
           //base.Draw(spriteBatch);
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
                bombActivate = true;
            }
            if (bombActivate)
            {
                if (!fuseBurningAlready)
                {
                    fuseSound.Play();
                    fuseBurningAlready = true;
                    bombSprite.SelectFrame(1, 0);
                }
                bombTimer -= gameTime.ElapsedGameTime.Milliseconds;
                bombSprite.Update(gameTime);
            }
            if (bombTimer <= 0)
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
            else if ((trueObjPointer is BombFire || trueObjPointer is FireBall) && !deleted)
            {
                bombActivate = true;
            }
            base.Collide(obj, trueObjPointer, overLap, gameTime);
        }
    }
}
