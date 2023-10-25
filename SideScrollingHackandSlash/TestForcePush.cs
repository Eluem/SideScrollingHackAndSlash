//******************************************************
// File: TestForcePush.cs
//
// Purpose: Contains the class definition of
// TestForcePush. TestForcePush is a test object that
// will be used to play around with my right thumbstick.
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
    class TestForcePush : PhysicsObject
    {
        Sprite skullBallSprite;
        public TestForcePush(Vector2 Speed, Vector2 Pos, int Owner)
            : base("", true, -1, Owner, Pos, Speed, new Vector2(0, 0), 20F, 0F, new Vector2(0,0), .5F, 1F)
        {
            skullBallSprite = new Sprite(50, new Point((int)pos.X, (int)pos.Y), "threerings", 1/5F, new Point(75, 75), new Point(6, 8));
            
            rect.Width = 15;
            rect.Height = 20;
        }
        public override void Update(GameTime gameTime, Viewport port)
        {
            sided[side.bottom] = true;
            base.Update(gameTime, port);
            skullBallSprite.Update(gameTime);
        }
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            if (!(obj is PlayerObject))
            {
                Delete();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            skullBallSprite.UpdatePos(pos);
            skullBallSprite.Draw(spriteBatch, layer);
        }
        public override PhysicsObject DeepClone()
        {
            return (new TestForcePush(speed, pos, ownerID));
        }
    }
}
