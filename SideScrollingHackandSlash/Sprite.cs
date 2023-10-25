//******************************************************
// File: Sprite.cs
//
// Purpose: Contains the class definition for sprite.
// Sprite contains its own draw function and update
// functions.
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
    class Sprite : Drawable
    {
        Facing direction; //Stores the direction of the animation
        protected Point currentFrame; //Holds the position of the current frame
        protected Point sheetSize;    //Holds the dimensions of the sprite sheet
        protected Point frameSize;    //Holds the dimensions of each frame
        protected Rectangle drawRect;  //Stores the rectangle that each frame will be drawn to
        protected int currentFrameTime; //Counts the time between frames
        protected int timePerFrame;    //Holds the max time between frames
        protected float scale;         //Stores the scale of the image
        protected Boolean directionMatters; //Stores a flag stating that the direction matters

        //****************************************************
        // Method: Sprite
        //
        // Purpose: Constructor for Sprite class
        //****************************************************
        public Sprite(int TimePerFrame, Point Pos, string image, float Scale, Point FrameSize, Point SheetSize):base(image, Pos)
        {
            currentFrame = new Point(0, 0);
            currentFrameTime = 0;
            timePerFrame = TimePerFrame;
            sheetSize = SheetSize;
            frameSize = FrameSize;
            rect.Width = frameSize.X;
            rect.Height = frameSize.Y;
            drawRect = new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y);
            scale = Scale;
            directionMatters = false;
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: Loops through a sprite at the set speed.
        //****************************************************
        public void Update(GameTime gameTime)
        {
            currentFrameTime += gameTime.ElapsedGameTime.Milliseconds; //Updates currentFrameTime
            
            //If the frame has been shown for the set ammount of time, the next frame is displayed.
            if (currentFrameTime >= timePerFrame) 
            {
                //currentFrameTime -= timePerFrame;
                currentFrameTime = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
                drawRect.X = currentFrame.X * frameSize.X;
                drawRect.Y = currentFrame.Y * frameSize.Y;
            }
        }

        //****************************************************
        // Method: UpdatePos
        //
        // Purpose: Updates the position of the rectangle
        // that the sprite will be drawn to.
        //****************************************************
        public void UpdatePos(int xPos, int yPos)
        {
            rect.X = xPos;
            rect.Y = yPos;
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Overrides Drawable's Draw function
        //****************************************************
        public override void Draw(SpriteBatch spriteBatch, float layer)
        {
            if (!directionMatters)
            {
                spriteBatch.Draw(GlobalVariables.ImageDict[image], new Vector2(rect.X, rect.Y), drawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer);
            }
            else
            {
                if (direction == Facing.right)
                {
                    spriteBatch.Draw(GlobalVariables.ImageDict[image], new Vector2(rect.X, rect.Y), drawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.FlipHorizontally, layer);
                }
                else
                {
                    spriteBatch.Draw(GlobalVariables.ImageDict[image], new Vector2(rect.X, rect.Y), drawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer);
                }
            }
        }

        //****************************************************
        // Method: SelectFrame
        //
        // Purpose: Allows an object to set the frame that
        // it's on. Useful when a sprite needs to do
        // something odd, that sprite can define it's
        // own looping.
        //****************************************************
        public void SelectFrame(int x, int y)
        {
            currentFrame.X = x;
            currentFrame.Y = y;
            drawRect.X = currentFrame.X * frameSize.X;
            drawRect.Y = currentFrame.Y * frameSize.Y;
        }

        //Properties for sprite which allow selective access to member variables
        public Point CurrentFrame
        {
            get
            {
                return currentFrame;
            }
            set
            {
                currentFrame = value;
            }
        }

        public Point FrameSize
        {
            get
            {
                return frameSize;
            }
        }
        public int DrawRectWidth
        {
            get
            {
                return drawRect.Width;
            }
            set
            {
                drawRect.Width = value;
            }
        }
        public int DrawRectHeight
        {
            get
            {
                return drawRect.Height;
            }
            set
            {
                drawRect.Height = value;
            }
        }
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }
        public int CurrentFrameTime
        {
            get
            {
                return currentFrameTime;
            }
            set
            {
                currentFrameTime = value;
            }
        }

        public int TimerPerFrame
        {
            get
            {
                return timePerFrame;
            }
            set
            {
                timePerFrame = value;
            }
        }
        public Facing Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        public Boolean DirectionMatters
        {
            get
            {
                return directionMatters;
            }
            set
            {
                directionMatters = value;
            }
        }
    }
}
