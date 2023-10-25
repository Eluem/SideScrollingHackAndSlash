//******************************************************
// File: TestBox001.cs
// Purpose: Contains the class definition of
// TestBox001. TestBox001 is a box platform that
// will be used to test elastic collisions.
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
    class TestBox001 : ThrowableObject
    {
        //****************************************************
        // Method: TestBox001
        //
        // Purpose: TestBox001 constructor
        //****************************************************
        public TestBox001(Vector2 Position, string blockType, float mass)
            : base(blockType, -1, -1, Position, new Vector2(0, 0), new Vector2(0, 0), mass, .1F, new Vector2(.02F, .008F), .4F, .4F)
        {
        }
    }
}
