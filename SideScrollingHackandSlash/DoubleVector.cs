//******************************************************
// File: DoubleVector.cs
//
// Purpose: Contains the class definition of
// DoubleVector. DoubleVector is a version of Vector2
// meant for more accurate calculations.
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
    class DoubleVector
    {
        protected double x;
        protected double y;

        //****************************************************
        // Method: DoubleVector
        //
        // Purpose: DoubleVector constructor
        //****************************************************
        public DoubleVector()
        {
            x = 0;
            y = 0;
        }

        //****************************************************
        // Method: DoubleVector
        //
        // Purpose: DoubleVector overloaded constructor
        //****************************************************
        public DoubleVector(double X, double Y)
        {
            x = X;
            y = Y;
        }

        //****************************************************
        // Method: StoreVector2
        //
        // Purpose: To convert and store a normal Vector2
        //****************************************************
        public void StoreVector2(Vector2 Vector)
        {
            x = (double)Vector.X;
            y = (double)Vector.Y;
        }


        //Properties
        public double X
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

        public double Y
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

        public Vector2 FloatVector
        {
            get
            {
                return (new Vector2((float)x, (float)y));
            }
        }

        public static DoubleVector Zero
        {
            get
            {
                return(new DoubleVector());
            }
        }
    }
}
