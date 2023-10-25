//******************************************************
// File: DeathAnimation.cs
//
// Purpose: Contains the class definition of
// DeathAnimation. DeathAnimation simply serves as the
// image which the player leaves behind as their
// death animation.
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
    class DeathAnimation : PhysicsObject
    {
        Sprite deathSprite;
        int fadeTimer;
        public DeathAnimation(Facing direction, Vector2 Position, Vector2 Speed, Vector2 Accel, float Mass, float Friction, Vector2 AirResistance, float Bounciness, float Layer):base("", true, -1, -1, Position, Speed, Accel, Mass, Friction, AirResistance, Bounciness, Layer)
        {
            deathSprite = new Sprite(100, new Point(0, 0), "death", 1 / 4F, new Point(382, 303), new Point(5, 1));
            deathSprite.Direction = direction;
            deathSprite.DirectionMatters = true;
            rect.Height = 20;
            rect.Width = 10;
            fadeTimer = 2000;
        }

        public override void Update(GameTime gameTime, Viewport port)
        {
            if (deathSprite.CurrentFrame.X != 4)
            {
                deathSprite.Update(gameTime);
            }
            else
            {
                fadeTimer -= gameTime.ElapsedGameTime.Milliseconds;
                if (fadeTimer <= 0)
                {
                    Delete();
                }
            }

            base.Update(gameTime, port);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            deathSprite.UpdatePos(new Vector2(pos.X, pos.Y - (303/4 - 20)));
            deathSprite.Draw(spriteBatch, layer);
        }
    }
}
