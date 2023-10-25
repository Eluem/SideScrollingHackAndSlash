//******************************************************
// File: ScaleablePlatform.cs
// Purpose: Contains the class definition of
// ScaleablePlatform. ScaleablePlatform is a basic
// platform whose size can be chosen upon creation.
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
    class ScaleablePlatform:PhysicsObject
    {
        //****************************************************
        // Method: ScaleablePlatform
        //
        // Purpose: ScaleablePlatform constructor
        //****************************************************
        public ScaleablePlatform(Vector2 Position, Vector2 WidthHeight)
            : base("platform_flat_long", false, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), 9999999999999F, .001F, new Vector2(0, 0), 0F, .4F)
        {
            rect.Width = WidthHeight.X;
            rect.Height = WidthHeight.Y;
            image.DrawRect = new Rectangle(0,0, (int)rect.Width, (int)rect.Height);
            GridLocationUpdate();
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            image.RectDraw(spriteBatch, layer);
        }
    }
}
