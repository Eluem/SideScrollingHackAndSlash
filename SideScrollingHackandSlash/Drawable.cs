//******************************************************
// File: Drawable.cs
//
// Purpose: Contains the class definition for Drawable.
// Drawable will be inherited by any class that will
// be drawn to the screen.
//
// Written By: Salvatore Hanusiewicz
//******************************************************

using System;
using System.Collections.Generic;
using System.Linq;
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
    class Drawable
    {
        protected Rectangle rect; //Stores the rectangle that the image will be drawn to
        protected string image;   //Stores the name of the image file
        protected Rectangle sourceRect; //Stores the source rect of the image
        protected Vector2 position; //Stores the position of the image
//        protected float scale; //Stores the scale of the image

        //****************************************************
        // Method: Drawable
        //
        // Purpose: Constructor for Drawable class.
        //****************************************************
        public Drawable(string Image, Point Pos)
        {
            image = Image;
            rect = new Rectangle(Pos.X, Pos.Y, GlobalVariables.ImageDict[Image].Width, GlobalVariables.ImageDict[Image].Height);
            sourceRect = new Rectangle(0, 0, rect.Width, rect.Height);
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Draws the object to the screen.
        //****************************************************
        public virtual void Draw(SpriteBatch spriteBatch, float layer)
        {
            spriteBatch.Draw(GlobalVariables.ImageDict[image], position, sourceRect, Color.White, 0F, Vector2.Zero, 1F, SpriteEffects.None, layer);
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Draws the object to the screen.
        // using the rectangle
        //****************************************************
        public virtual void RectDraw(SpriteBatch spriteBatch, float layer)
        {
            spriteBatch.Draw(GlobalVariables.ImageDict[image], rect, sourceRect, Color.White, 0F, Vector2.Zero, SpriteEffects.None, layer);
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Draws the object to the screen.
        //****************************************************
        public virtual void Draw(SpriteBatch spriteBatch, float layer, Facing Direction)
        {
            if (Direction == Facing.right)
            {
                spriteBatch.Draw(GlobalVariables.ImageDict[image], rect, sourceRect, Color.White, 0F, Vector2.Zero, SpriteEffects.None, layer);
            }
            else
            {
                spriteBatch.Draw(GlobalVariables.ImageDict[image], rect, sourceRect, Color.White, 0F, Vector2.Zero, SpriteEffects.FlipHorizontally, layer);
            }
        }




        //****************************************************
        // Method: UpdatePos
        //
        // Purpose: Updates the position of the rectangle
        // that the sprite will be drawn to.
        //****************************************************
        public virtual void UpdatePos(Vector2 Pos)
        {
            position = Pos;
            rect.X = (int)Pos.X;
            rect.Y = (int)Pos.Y;
        }

        //Properties
        public virtual string Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                rect.Width = GlobalVariables.ImageDict[image].Width;
                rect.Height = GlobalVariables.ImageDict[image].Height;
            }
        }

        public int Width
        {
            get
            {
                return GlobalVariables.ImageDict[image].Width;
            }
        }

        public int Height
        {
            get
            {
                return GlobalVariables.ImageDict[image].Height;
            }
        }

        public Rectangle DrawRect
        {
            set
            {
                rect.Width = value.Width;
                rect.Height = value.Height;
            }
        }
    }
}
