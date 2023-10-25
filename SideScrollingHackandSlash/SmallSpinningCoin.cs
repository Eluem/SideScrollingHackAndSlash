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
    class SmallSpinningCoin:PhysicsObject
    {
        Sprite coinSprite;
        int lifeTime;
        //****************************************************
        // Method: SpinningCoin
        //
        // Purpose: SpinningCoin constructor
        //****************************************************
        public SmallSpinningCoin(Vector2 Position)
            : base("CopperCoin", true, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), .001F, .5F, new Vector2(0, 0), .5F, .4F)
        {
            coinSprite = new Sprite(100, new Point((int)Position.X, (int)Position.Y), "CopperCoin", .25F, new Point(41, 42), new Point(3, 3));
            rect.Width = 11;
            rect.Height = 10;
            speed.X = GlobalVariables.Randomizer.Next(-500, 500);
            speed.Y = GlobalVariables.Randomizer.Next(-900, -100);
            lifeTime = GlobalVariables.Randomizer.Next(4000, 10000);
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            Boolean baseCollideAnyway = true;
            if (obj.SpecialProperties)
            {
                if (obj is PlayerObject)
                {
                    baseCollideAnyway = false;
                    Delete();
                }
                if (trueObjPointer is SmallSpinningCoin)
                {
                    baseCollideAnyway = false;
                }
            }
            if (baseCollideAnyway)
            {
                base.Collide(obj, trueObjPointer, overLap, gameTime);
            }
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
            coinSprite.Update(gameTime);
            base.Update(gameTime, port);
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Calls the draw functions of the sprites
        // that the object contains.
        //****************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            coinSprite.UpdatePos((int)pos.X, (int)pos.Y);
            coinSprite.Draw(spriteBatch, layer);
        }
    }
}
