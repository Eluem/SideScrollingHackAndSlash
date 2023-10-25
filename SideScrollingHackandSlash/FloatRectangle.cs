//******************************************************
// File: FloatRectangle.cs
//
// Purpose: Contains the class definition of
// FloatRectangle. FloatRectangle is an
// implementation of a rectangle that uses floating
// point values to store its information.
// (Note: This rectangle only implements basic
//  properties because that's all I use it for.)
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
    class FloatRectangle
    {

        protected float width; //Stores the width of the rectangle
        protected float height; //Stores the height of the rectangle
        protected float x;      //stores the x component of the position of the rectangle
        protected float y;      //stores the y component of the position of the rectangle
        protected Vector2 center; //stores the center position of
                                  //the rectangle (used to prevent wasting heap)

        //****************************************************
        // Method: FloatRectangle
        //
        // Purpose: FloatRectangle default constructor
        //****************************************************
        public FloatRectangle()
        {
            //Default initializations
            width = 0F;
            height = 0F;
            x = 0F;
            y = 0F;
            center = new Vector2();
        }

        //****************************************************
        // Method: FloatRectangle
        //
        // Purpose: FloatRectangle overloaded constructor
        //****************************************************
        public FloatRectangle(float X, float Y, float Width, float Height)
        {
            //Default initializations
            width = Width;
            height = Height;
            x = X;
            y = Y;
            center = new Vector2();
        }


        //Properties
        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public float Top
        {
            get
            {
                return y;
            }
        }

        public float Bottom
        {
            get
            {
                return y + height;
            }
        }

        public float Left
        {
            get
            {
                return x;
            }
        }

        public float Right
        {
            get
            {
                return x + width;
            }
        }
        public Vector2 Center
        {
            get
            {
                center.X = x + (width / 2);
                center.Y = y + (height / 2);
                return center;
            }
        }
    }
}
