//******************************************************
// File: ThumbstickTrackingTest.cs
// Purpose: Contains the class definition of
// ThumbstickTrackingTest. ThumbstickTrackingTest is an
// attempt at using a more dynamic and fluid method
// to track the thumb stick for sword combat.
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
    class ThumbstickTrackingTest : PhysicsObject
    {
        Vector2 shadowThumbStick; //Stores the shadow of the thumb stick
        Vector2 followThumbStick; //Stores the following thumbstick's position
        Vector2 followSpeed; //Stores the speed of the following thumbstick
        Vector2 followAccel; //Stores the acceleration of the following thumbstick

        Vector2 previousThumbStick; //Stores the previos thumbstick position
        Vector2 currentThumbStick; //Stores the current thumbstick position

        Vector2 previousShadow; //Stores the previous shadow

        Drawable myStick; //Image for the thumb stick
        Drawable followStickImage;


        //int orbitTimer; //tracks the amount of time the shadow has left to orbit
        //double distanceDifference; //the range of difference between distances of the orbiting shadow which will
                                  //be required to be considered orbiting

        //double previousDistance; //Stores the previous distance

        Boolean moved; //Stores a flag stating whether or not the player has moved the thumbstick

        int[] passed; //Stores 2 flags, one for the x axis and one for the y axis stating whether or not
        //the shadow has passed the current thumb stick on either axis since the thumb stick stopped moving

        /*
        int lockTimer; //Used to enable 1:1 tracking when the shadow stick is close enough to the player stick
                       //for long enough

        int lockCount; //Number of times the shadow can pass through the radius of the thumb stick before it locks to it

        Boolean inCircle; //Stores a flag stating whether or not the shadow was in the buffer around the thumb stick or not
        */
         
        //****************************************************
        // Method: ThumbstickTrackingTest
        //
        // Purpose: ThumbstickTrackingTest constructor
        //****************************************************
        public ThumbstickTrackingTest(Vector2 Position, Vector2 WidthHeight)
            : base("platform_flat_long", false, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), 9999999999999F, .001F, new Vector2(0, 0), 0F, .4F)
        {
            rect.Width = WidthHeight.X;
            rect.Height = WidthHeight.Y;
            image.DrawRect = new Rectangle(0, 0, (int)rect.Width, (int)rect.Height);
            GridLocationUpdate();
            followThumbStick = Vector2.Zero;

            myStick = new Drawable("platform_flat_long_red", Point.Zero);
            myStick.DrawRect = new Rectangle(0, 0, (int)rect.Width, (int)rect.Height);
            followStickImage = new Drawable("platform_flat_long_blue", Point.Zero);
            followStickImage.DrawRect = new Rectangle(0, 0, (int)rect.Width, (int)rect.Height);

            shadowThumbStick = new Vector2();
            followThumbStick = new Vector2();
            followSpeed = new Vector2();
            followAccel = new Vector2();
            previousThumbStick = new Vector2();
            currentThumbStick = new Vector2();
            previousShadow = new Vector2();
            //orbitTimer = 0;
            //distanceDifference = 0.1;

            passed = new int[2];

            /*
            lockTimer = 0;
            lockCount = 0;
            inCircle = false;
            */
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
            currentThumbStick.X = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular).ThumbSticks.Right.X;
            currentThumbStick.Y = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular).ThumbSticks.Right.Y;


            float maxSpeed = .3F;

            //float maxSpeed = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y/2F;

            float maxAccel = .1F;
            


            /*
            if (MathHelper.Distance(currentThumbStick.X, followThumbStick.X) > minDistance || MathHelper.Distance(currentThumbStick.Y, followThumbStick.Y) > minDistance)
            {
                followAccel.X = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.X - followThumbStick.X + .5F);
                followSpeed.X = MathHelper.Clamp(followSpeed.X + followAccel.X, -maxSpeed, maxSpeed);

                followAccel.Y = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.Y - followThumbStick.Y + .5F);
                followSpeed.Y = MathHelper.Clamp(followSpeed.Y + followAccel.Y, -maxSpeed, maxSpeed);

                followThumbStick.X = MathHelper.Clamp(followThumbStick.X + followSpeed.X, -1F, 1F);
                followThumbStick.Y = MathHelper.Clamp(followThumbStick.Y + followSpeed.Y, -1F, 1F);

                lockTimer = 0;
            }
            else if (lockTimer >= 100)
            {
                followSpeed = Vector2.Zero;

                followThumbStick = currentThumbStick;
            }
            else
            {
                lockTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            */



            if (passed[0] < 1 && passed[1] < 1)
            {
                followAccel.X = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.X - followThumbStick.X + .5F);
                followSpeed.X = MathHelper.Clamp(followSpeed.X + followAccel.X, -maxSpeed, maxSpeed);

                followAccel.Y = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.Y - followThumbStick.Y + .5F);
                followSpeed.Y = MathHelper.Clamp(followSpeed.Y + followAccel.Y, -maxSpeed, maxSpeed);
            }
            else
            {
                followSpeed.X = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.X - followThumbStick.X + .5F);
                followSpeed.Y = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.Y - followThumbStick.Y + .5F);
            }


            if (MathHelper.Distance(previousThumbStick.X, currentThumbStick.X) < .1 && MathHelper.Distance(previousThumbStick.Y, currentThumbStick.Y) < .1)
            {
                moved = false;
            }
            else
            {
                moved = true;
                passed[0] = 0;
                passed[1] = 0;
            }


            /*
            DoubleVector tempShadow = new DoubleVector();
            tempShadow.StoreVector2(shadowThumbStick);
            DoubleVector tempCurrent = new DoubleVector();
            tempCurrent.StoreVector2(currentThumbStick);
            double currentDistance = Math.Sqrt(Math.Pow(Math.Abs(tempCurrent.X - tempShadow.X), 2) + Math.Pow(Math.Abs(tempCurrent.Y - tempShadow.Y), 2));
            if (currentDistance - previousDistance <= distanceDifference)
            {
                orbitTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                orbitTimer = 0;
            }
            */

            if (!moved && (previousShadow.X < currentThumbStick.X && shadowThumbStick.X > currentThumbStick.X || previousShadow.X > currentThumbStick.X && shadowThumbStick.X < currentThumbStick.X))
            {
                ++passed[0];
            }
            if (!moved && (previousShadow.Y < currentThumbStick.Y && shadowThumbStick.Y > currentThumbStick.Y || previousShadow.Y > currentThumbStick.Y && shadowThumbStick.Y < currentThumbStick.Y))
            {
                ++passed[1];
            }


            previousShadow = shadowThumbStick;

            followThumbStick.X = followThumbStick.X + followSpeed.X;
            followThumbStick.Y = followThumbStick.Y + followSpeed.Y;
            shadowThumbStick.X = MathHelper.Clamp(followThumbStick.X, -1F, 1F);
            shadowThumbStick.Y = MathHelper.Clamp(followThumbStick.Y, -1F, 1F);

            previousThumbStick.X = currentThumbStick.X;
            previousThumbStick.Y = currentThumbStick.Y;
        }

        
        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public void Update(GameTime gameTime, Vector2 stick)
        {
            /*
            currentThumbStick.X = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular).ThumbSticks.Right.X;
            currentThumbStick.Y = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular).ThumbSticks.Right.Y;
            */

            currentThumbStick = stick;



            float maxSpeed = .3F;

            //float maxSpeed = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y/2F;

            float maxAccel = .1F;
            


            /*
            if (MathHelper.Distance(currentThumbStick.X, followThumbStick.X) > minDistance || MathHelper.Distance(currentThumbStick.Y, followThumbStick.Y) > minDistance)
            {
                followAccel.X = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.X - followThumbStick.X + .5F);
                followSpeed.X = MathHelper.Clamp(followSpeed.X + followAccel.X, -maxSpeed, maxSpeed);

                followAccel.Y = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.Y - followThumbStick.Y + .5F);
                followSpeed.Y = MathHelper.Clamp(followSpeed.Y + followAccel.Y, -maxSpeed, maxSpeed);

                followThumbStick.X = MathHelper.Clamp(followThumbStick.X + followSpeed.X, -1F, 1F);
                followThumbStick.Y = MathHelper.Clamp(followThumbStick.Y + followSpeed.Y, -1F, 1F);

                lockTimer = 0;
            }
            else if (lockTimer >= 100)
            {
                followSpeed = Vector2.Zero;

                followThumbStick = currentThumbStick;
            }
            else
            {
                lockTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            */



            if (passed[0] < 1 && passed[1] < 1)
            {
                followAccel.X = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.X - followThumbStick.X + .5F);
                followSpeed.X = MathHelper.Clamp(followSpeed.X + followAccel.X, -maxSpeed, maxSpeed);

                followAccel.Y = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.Y - followThumbStick.Y + .5F);
                followSpeed.Y = MathHelper.Clamp(followSpeed.Y + followAccel.Y, -maxSpeed, maxSpeed);
            }
            else
            {
                followSpeed.X = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.X - followThumbStick.X + .5F);
                followSpeed.Y = MathHelper.SmoothStep(-maxAccel, maxAccel, currentThumbStick.Y - followThumbStick.Y + .5F);
            }


            if (MathHelper.Distance(previousThumbStick.X, currentThumbStick.X) < .1 && MathHelper.Distance(previousThumbStick.Y, currentThumbStick.Y) < .1)
            {
                moved = false;
            }
            else
            {
                moved = true;
                passed[0] = 0;
                passed[1] = 0;
            }

            /*
            DoubleVector tempShadow = new DoubleVector();
            tempShadow.StoreVector2(shadowThumbStick);
            DoubleVector tempCurrent = new DoubleVector();
            tempCurrent.StoreVector2(currentThumbStick);
            double currentDistance = Math.Sqrt(Math.Pow(Math.Abs(tempCurrent.X - tempShadow.X), 2) + Math.Pow(Math.Abs(tempCurrent.Y - tempShadow.Y), 2));
            if (currentDistance - previousDistance <= distanceDifference)
            {
                orbitTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                orbitTimer = 0;
            }
            */
            if (!moved && (previousShadow.X < currentThumbStick.X && shadowThumbStick.X > currentThumbStick.X || previousShadow.X > currentThumbStick.X && shadowThumbStick.X < currentThumbStick.X))
            {
                ++passed[0];
            }
            if (!moved && (previousShadow.Y < currentThumbStick.Y && shadowThumbStick.Y > currentThumbStick.Y || previousShadow.Y > currentThumbStick.Y && shadowThumbStick.Y < currentThumbStick.Y))
            {
                ++passed[1];
            }


            //previousDistance = currentDistance;
            previousShadow = shadowThumbStick;

            followThumbStick.X = followThumbStick.X + followSpeed.X;
            followThumbStick.Y = followThumbStick.Y + followSpeed.Y;
            shadowThumbStick.X = MathHelper.Clamp(followThumbStick.X, -1F, 1F);
            shadowThumbStick.Y = MathHelper.Clamp(followThumbStick.Y, -1F, 1F);

            previousThumbStick.X = currentThumbStick.X;
            previousThumbStick.Y = currentThumbStick.Y;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 tempPos = new Vector2(pos.X + (shadowThumbStick.X * 20), pos.Y - (shadowThumbStick.Y * 20));

            Vector2 tempPos2 = new Vector2(pos.X + (currentThumbStick.X * 20), pos.Y - (currentThumbStick.Y * 20));

            Vector2 tempPos3 = new Vector2(pos.X + (followThumbStick.X * 20), pos.Y - (followThumbStick.Y * 20));


            image.UpdatePos(tempPos);
            image.RectDraw(spriteBatch, layer);

            myStick.UpdatePos(tempPos2);
            myStick.RectDraw(spriteBatch, layer);

            followStickImage.UpdatePos(tempPos3);
            followStickImage.RectDraw(spriteBatch, layer);
        }

        /*
         * Vector2 followThumbStick; //Stores the following thumbstick's position
        Vector2 followSpeed; //Stores the speed of the following thumbstick
        Vector2 followAccel; //Stores the acceleration of the following thumbstick

        Vector2 previousThumbStick; //Stores the previos thumbstick position
        Vector2 currentThumbStick; //Stores the current thumbstick position
         */

        public Vector2 FollowThumbStick
        {
            get
            {
                return followThumbStick;
            }
        }

        public Vector2 FollowSpeed
        {
            get
            {
                return followSpeed;
            }
        }
        public Vector2 FollowAccel
        {
            get
            {
                return followAccel;
            }
        }
        public Vector2 CurrentThumbStick
        {
            get
            {
                return currentThumbStick;
            }
        }
        public Vector2 PreviousThumbStick
        {
            get
            {
                return previousThumbStick;
            }
        }

        public Vector2 ShadowThumbStick
        {
            get
            {
                return shadowThumbStick;
            }
        }

        public int[] Passed
        {
            get
            {
                return passed;
            }
        }

        /*
        public Boolean InCircle
        {
            get
            {
                return inCircle;
            }
        }
        public int LockCount
        {
            get
            {
                return lockCount;
            }
        }
        */


    }
}
