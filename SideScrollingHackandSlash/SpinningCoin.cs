//******************************************************
// File: SpinningCoin.cs
// Purpose: Contains the class definition of
// SpinningCoin. My little brother wanted me to make
// a coin that disapears when you touch it. So I did.
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
//, Dictionary<string, Point> SheetSizeDict, Dictionary<string, Point> FrameSizeDict
namespace SideScrollingHackandSlash
{
    class SpinningCoin:PhysicsObject
    {
        Sprite coinSprite;

        //****************************************************
        // Method: SpinningCoin
        //
        // Purpose: SpinningCoin constructor
        //****************************************************
        public SpinningCoin(Vector2 Position)
            : base("CopperCoin", true, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), 0F, .5F, new Vector2(0, 0), 0F, .4F)
        {
            coinSprite = new Sprite(100, new Point((int)Position.X, (int)Position.Y), "CopperCoin", 1F, new Point(41, 42), new Point(3, 3));
            rect.Width = 41;
            rect.Height = 42;
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            if (obj.SpecialProperties)
            {
                if(obj is PlayerObject)
                {
                    Delete();
                }
            }
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            coinSprite.Update(gameTime);
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Calls the draw functions of the sprites
        // that the object contains.
        //****************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            coinSprite.Draw(spriteBatch, layer);
        }

        //**************************************************
        // Method: DeepClone
        //
        // Purpose: To make a deep copy of this object so
        // that, during collisions the system can stay
        // consistent.
        //**************************************************
        public override PhysicsObject DeepClone()
        {
            return (new SpinningCoin(pos));
        }

    }
}
