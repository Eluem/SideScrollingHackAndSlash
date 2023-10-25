//******************************************************
// File: CollideableObject.cs
//
// Purpose: Contains the class definition of
// CollideableObject. CollideableObject is used to
// provide basic collision definitions and to
// abstract some of th details from the objects below
// it.
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
    abstract class CollideableObject : BasicObject
    {
        protected FloatRectangle rect; //Stores the collision box for the object
        protected Vector2 prevPos; //Stores previous position for the purpose of my current attempt at responding to collision

        //****************************************************
        // Method: CollideableObject
        //
        // Purpose: CollideableObject constructor
        //****************************************************
        public CollideableObject(String Image, Boolean SpecialProperties, int ObjectID, int OwnerID, Vector2 Position, float Layer)
            : base(Image, SpecialProperties, ObjectID, OwnerID, Position, Layer)
        {
            //Basic Initialization
            prevPos = Position;

            //Allows objects which do not actually implement the image variable to pass ""
            //since BasicObject does not assign an image when "" is passed (to prevent wasting heap memory
            //the image pointer is null, thus a special case must be defined here for this.
            if (Image != "")
            {
                rect = new FloatRectangle(Position.X, Position.Y, image.Width, image.Height);
            }
            else
            {
                rect = new FloatRectangle(Position.X, Position.Y, 0F, 0F);
            }
        }

        //****************************************************
        // Method: CollideCheck
        //
        // Purpose: Checks for collision.
        //****************************************************
        public virtual CollideCheckReturn CollideCheck(PhysicsObject obj, PhysicsObject trueObjPointer)
        {
            //Checks for collision using N Tutorial A (http://www.metanetsoftware.com/technique/tutorialA.html)
            Vector2 overLap = new Vector2();
            //Finds how much the rectangles are overlapping, if they are, in the x axis
            overLap.X = rect.Width / 2 + obj.Rect.Width / 2 - Math.Abs(obj.Rect.Center.X - rect.Center.X);
            if (overLap.X >= 0) //if the rects do overlap in the x axis then there's more checking to be done
            {
                //Finds how much the rectangles are overlapping, if they are, in the y axis
                overLap.Y = rect.Height / 2 + obj.Rect.Height / 2 - Math.Abs(obj.Rect.Center.Y - rect.Center.Y);
                if (overLap.Y >= 0)//if the rects overlap in the x and y axis then there is a collision
                {
                    return (new CollideCheckReturn(true, overLap, ((PhysicsObject)this).DeepClone(), ((PhysicsObject)this), obj, trueObjPointer));
                }
            }
            return (new CollideCheckReturn(false, overLap, ((PhysicsObject)this).DeepClone(), ((PhysicsObject)this), obj, trueObjPointer));
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public virtual void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
        }


        //Properties
        public FloatRectangle Rect
        {
            get
            {
                return rect;
            }
        }

        public Vector2 PrevPos
        {
            get
            {
                return prevPos;
            }
        }
    }
}
