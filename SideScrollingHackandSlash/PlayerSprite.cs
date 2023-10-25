//******************************************************
// File: PlayerSprite.cs
//
// Purpose: Contains the class definition for
// PlayerSprite.
// PlayerSprite contains its own draw function and
// update functions which are intended to interact with
// a single sprite sheet that stores all of the
// different parts of a player sprite, thus abstracting
// these complexities from the Player class.
//
// PlayerSprite doesn't inherit from the Drawable class
// or the Sprite class because I determined that the
// differences between the PlayerSprite class and the
// other two were so great that the inheriting of them
// wouldn't be beneifical overall.
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
    class PlayerSprite
    {
        protected Point currentFrame; //Holds the position of the current frame
        protected Point sheetSize;    //Holds the dimensions of the sprite sheet
        protected Point frameSize;    //Holds the dimensions of each frame
        protected Rectangle drawRect;  //Stores the rectangle that will be the window into the sprite sheet
        protected int currentFrameTime; //Counts the time between frames
        protected int timePerFrame;    //Holds the max time between frames


        protected Point armsCurrentFrame; //Holds the position of the current frame
        protected Point armsSheetSize;//Holds the dimensions of the sprite sheet
        protected Point armsFrameSize;//Holds the dimensions of each frame
        protected Rectangle armsDrawRect; //Stores the rectangle that will be the window into the sprite sheet

        //Declarations that were originally in Drawable
        //protected Rectangle rect; //Stores the rectangle that the image will be drawn to
        protected Vector2 pos; //Stores the floating point location
        protected Vector2 offSet; //Stores the floating point offset of the image

        protected string legImage;   //Stores the name of the leg image file
        protected string rightArmImage;   //Stores the name of the right arm image file
        //protected string leftArmImage;   //Stores the name of the left arm image file
        protected string torsoImage;   //Stores the name of the torso image file

        protected float scale; //stores the scale of the player image

        //****************************************************
        // Method: PlayerSprite
        //
        // Purpose: Constructor for PlayerSprite class
        //****************************************************
        public PlayerSprite(Point Pos, Vector2 OffSet)
        {

            //Body Stuff
            currentFrame = new Point(0, 0);
            currentFrameTime = 0;
            timePerFrame = 100;
            sheetSize = new Point(5,1);//new Point(9,2);
            frameSize = new Point(284, 378);//new Point(56,47);
            //rect.Width = frameSize.X;
            //rect.Height = frameSize.Y;
            currentFrame.Y = 0;
            drawRect = new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y);
            drawRect.Y = currentFrame.Y * frameSize.Y;

            //Arms
            armsSheetSize = new Point(5, 1);
            armsFrameSize = new Point(284, 378);
            armsCurrentFrame = new Point(0, 0);
            armsDrawRect = new Rectangle(armsCurrentFrame.X * armsFrameSize.X, armsCurrentFrame.Y * armsFrameSize.Y, armsFrameSize.X, armsFrameSize.Y);
            scale = 1/4F;


            offSet = OffSet;

            //Initializations that were originally in Drawable
            legImage = "walkinglegsfinal";
            rightArmImage = "swordArms";
            //leftArmImage = "foreward slash left arm";
            torsoImage = "torso";

            //rect = new Rectangle(Pos.X, Pos.Y, frameSize.X, frameSize.Y);
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: Loops through a sprite at the set speed.
        //****************************************************
        public void Update(GameTime gameTime, Vector2 speed, walking walkingDirection, Boolean jumping)
        {
            currentFrameTime += gameTime.ElapsedGameTime.Milliseconds; //Updates currentFrameTime
            if (currentFrameTime >= timePerFrame)
            {
                if ((int)Math.Abs(speed.X) != 0)
                {
                    currentFrameTime -= timePerFrame;
                    timePerFrame = 10000 / (int)Math.Abs(speed.X);
                    if (timePerFrame > 100)
                        timePerFrame = 100;
                    if (walkingDirection == walking.forwards)
                    {
                        ++currentFrame.X;
                    }
                    else
                    {
                        --currentFrame.X;
                    }
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                    if (currentFrame.X < 0)
                    {
                        currentFrame.X = sheetSize.X - 1;
                    }
                    drawRect.X = currentFrame.X * frameSize.X;
                }
                else
                {
                    currentFrameTime = 0; //Makes sure to keep the updatespeed reset
                    currentFrame.X = 6;
                    drawRect.X = currentFrame.X * frameSize.X;
                }
                if (jumping)
                {
                    currentFrameTime = 0;
                    currentFrame.X = 5;
                    drawRect.X = currentFrame.X * frameSize.X;
                }
            }

            //If the frame has been shown for the set ammount of time, the next frame is displayed.
            /*
            if (currentFrameTime >= timePerFrame) 
            {
                currentFrameTime -= timePerFrame;
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
            */
        }

        //****************************************************
        // Method: UpdatePos
        //
        // Purpose: Updates the position of the rectangle
        // that the sprite will be drawn to.
        //****************************************************
        public void UpdatePos(Vector2 Pos, Facing direction)
        {
            if (direction == Facing.left)
            {
                pos.X = Pos.X - offSet.X;
            }
            else
            {
                pos.X = Pos.X -offSet.X + 10;
            }
            pos.Y = Pos.Y - offSet.Y;
            /*
            rect.X = (int)(Pos.X - 21);
            rect.Y = (int)(Pos.Y - 3);
             */
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Draws the player's sprite to the screen
        //****************************************************
        public void Draw(SpriteBatch spriteBatch, Facing facing, float layer, int hitTimer)
        {
            Color myColor = Color.White;
            if (hitTimer > 0)
            {
                myColor = Color.Red;
                myColor.A = 100;
            }
            else
            {
                myColor = Color.TransparentBlack;
            }

            //Draws the sprites and flips them horrizontally if required, to show proper facing direction
            if (facing == Facing.left)
            {
                spriteBatch.Draw(GlobalVariables.ImageDict[legImage], pos, drawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer);
                spriteBatch.Draw(GlobalVariables.ImageDict[torsoImage], pos, drawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer+.1F);
                spriteBatch.Draw(GlobalVariables.ImageDict[rightArmImage], pos, armsDrawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer+.15F);
                spriteBatch.Draw(GlobalVariables.ImageDict[legImage], pos, drawRect, myColor, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer + .2F);
                spriteBatch.Draw(GlobalVariables.ImageDict[torsoImage], pos, drawRect, myColor, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer + .2F);
                spriteBatch.Draw(GlobalVariables.ImageDict[rightArmImage], pos, armsDrawRect, myColor, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer + .2F);
                //spriteBatch.Draw(GlobalVariables.ImageDict[leftArmImage], pos, armsDrawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.None, layer);
                
            }
            else
            {
                spriteBatch.Draw(GlobalVariables.ImageDict[legImage], pos, drawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.FlipHorizontally, layer);
                spriteBatch.Draw(GlobalVariables.ImageDict[torsoImage], pos, drawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.FlipHorizontally, layer+.1F);
                spriteBatch.Draw(GlobalVariables.ImageDict[rightArmImage], pos, armsDrawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.FlipHorizontally, layer+.15F);
                spriteBatch.Draw(GlobalVariables.ImageDict[legImage], pos, drawRect, myColor, 0F, new Vector2(0, 0), scale, SpriteEffects.FlipHorizontally, layer + .2F);
                spriteBatch.Draw(GlobalVariables.ImageDict[torsoImage], pos, drawRect, myColor, 0F, new Vector2(0, 0), scale, SpriteEffects.FlipHorizontally, layer + .2F);
                spriteBatch.Draw(GlobalVariables.ImageDict[rightArmImage], pos, armsDrawRect, myColor, 0F, new Vector2(0, 0), scale, SpriteEffects.FlipHorizontally, layer + .2F);
                //spriteBatch.Draw(GlobalVariables.ImageDict[leftArmImage], pos, armsDrawRect, Color.White, 0F, new Vector2(0, 0), scale, SpriteEffects.FlipHorizontally, layer);
            }
        }

        //****************************************************
        // Method: SelectFrame
        //
        // Purpose: Allows an object to set the frame that
        // it's on. Mostly useful for character sprites.
        //****************************************************
        public void SelectFrame(int x, int y)
        {
            armsCurrentFrame.X = x;
            armsCurrentFrame.Y = y;
            armsDrawRect.X = armsCurrentFrame.X * armsFrameSize.X;
            armsDrawRect.Y = armsCurrentFrame.Y * armsFrameSize.Y;
        }
    }
}
