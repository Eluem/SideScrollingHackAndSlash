//******************************************************
// File: MeleeSlash001.cs
// Purpose: Contains the class definition of
// MeleeSlash001. To act as the damage box for the
// melee attacks.
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
    class MeleeSlash001 : PhysicsObject
    {
        protected int damage; //How much damage the slash will deal if it strikes something that can be hurt
        protected Facing direction; //Stores the direction of the slash (the player whom is blocking must be
                                    //facing opposite to this direction in order to block successfully)
        protected PlayerObject ownerPointer; //Stores a pointer to the owner of this slash
        protected int slashHeight; //Stores the height at which a player must be blocking to be successful
        protected Boolean swordHit; //Stores a flag that states that the player hit something with their sword
        protected Attacks previousAttack;//Stores the previous attack state

        //****************************************************
        // Method: MeleeSlash001
        //
        // Purpose: MeleeSlash001 constructor
        //****************************************************
        public MeleeSlash001(PlayerObject OwnerPointer)
            : base("block_green", true, -1, -1, new Vector2(0,0), new Vector2(0,0), new Vector2(0, 0), 50F, .5F, new Vector2(0, 0), .5F, .4F)
        {
            //Initializes the melee slash object's details based on certain details about the player it belongs to
            ownerPointer = OwnerPointer;
            ownerID = OwnerPointer.ObjectID;
            direction = OwnerPointer.FacingDirection;
            pos.X = ownerPointer.Pos.X;
            pos.Y = ownerPointer.Pos.Y;

            Update();

            damage = (int)speed.X;
            rect.X = pos.X;
            rect.Y = pos.Y;
            GridLocationUpdate();

            swordHit = false;
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            if (ownerPointer.LockedAttack != Attacks.noAction && !(trueObjPointer is DeathAnimation))
            {
                swordHit = true;
            }
        }


        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            //base.Update(gameTime, port);
        }

        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public void Update()
        {
            //Finds the position of the player who the slash belongs to
            pos.X = ownerPointer.Pos.X;
            pos.Y = ownerPointer.Pos.Y;

            Vector2 speedDamageMultiplier = new Vector2(0, 0);
            speed.X = 0;
            speed.Y = 0;
            int baseDamage = 0;
            damage = 0;

            //updates the direction variable
            direction = ownerPointer.FacingDirection;

            //Makes sure the sword hit still
            if (swordHit)
            {
                swordHit = previousAttack == ownerPointer.LockedAttack;
            }


            if (!swordHit)
            {
                //Finds the details of the slash object's collision box and current position relative to the player
                //based on certain details about the current player's state
                if (direction == Facing.right)
                {
                    switch (ownerPointer.LockedAttack)
                    {
                        case Attacks.LongOverheadRight:
                            rect.Width = 33;
                            rect.Height = 60;
                            speed.X = 400;
                            speed.Y = -100;
                            baseDamage = 30;
                            slashHeight = 0;
                            speedDamageMultiplier.Y = 1.2F;
                            speedDamageMultiplier.X = .4F;
                            break;

                        case Attacks.ShortJabRight:
                            rect.Width = 20;
                            rect.Height = 10;
                            speed.X = 200;
                            speed.Y = 0;
                            baseDamage = 5;
                            slashHeight = 1;
                            speedDamageMultiplier.Y = 0F;
                            speedDamageMultiplier.X = .9F;
                            pos.Y += 20;
                            break;

                        case Attacks.LongJabRight:
                            rect.Width = 38;
                            rect.Height = 10;
                            speed.X = 600;
                            speed.Y = 0;
                            baseDamage = 30;
                            slashHeight = 1;
                            speedDamageMultiplier.Y = 0F;
                            speedDamageMultiplier.X = 1.5F;
                            pos.Y += 20;
                            break;
                        case Attacks.UpsweepRight:
                            rect.Width = 33;
                            rect.Height = 20;
                            speed.X = 1500;
                            speed.Y = -5500;
                            baseDamage = 20;
                            slashHeight = 2;
                            speedDamageMultiplier.Y = 0F;
                            speedDamageMultiplier.X = 0F;
                            pos.Y += 50;
                            break;
                    }
                    pos.X += ownerPointer.Rect.Width + 1;
                }
                else
                {
                    switch (ownerPointer.LockedAttack)
                    {
                        case Attacks.LongOverheadLeft:
                            rect.Width = 33;
                            rect.Height = 60;
                            speed.X = -400;
                            speed.Y = -100;
                            baseDamage = 30;
                            slashHeight = 0;
                            speedDamageMultiplier.Y = 1.2F;
                            speedDamageMultiplier.X = .4F;
                            break;
                        case Attacks.ShortJabLeft:
                            rect.Width = 20;
                            rect.Height = 10;
                            speed.X = -200;
                            speed.Y = 0;
                            baseDamage = 5;
                            slashHeight = 1;
                            speedDamageMultiplier.Y = 0F;
                            speedDamageMultiplier.X = .9F;
                            pos.Y += 20;
                            break;
                        case Attacks.LongJabLeft:
                            rect.Width = 38;
                            rect.Height = 10;
                            speed.X = -600;
                            speed.Y = 0;
                            baseDamage = 30;
                            slashHeight = 1;
                            speedDamageMultiplier.Y = 0F;
                            speedDamageMultiplier.X = 1.5F;
                            pos.Y += 20;
                            break;
                        case Attacks.UpsweepLeft:
                            rect.Width = 33;
                            rect.Height = 20;
                            speed.X = -1500;
                            speed.Y = -5500;
                            baseDamage = 20;
                            slashHeight = 2;
                            speedDamageMultiplier.Y = 0F;
                            speedDamageMultiplier.X = 0F;
                            pos.Y += 50;
                            break;
                    }
                    pos.X -= rect.Width - 1;
                }



                //**************************************************************************************
                // This is my attempt at a damage formula which includes speed. Each attack will have
                // its own  values for speedDamageMultiplier on the x and y axis. This allows each
                // attack to get different benefits from speed. For example, a forward thrust
                // (jab) would get a large x speedDamageMultiplier but little or no
                // y speedDamageMultiplier. Also, each attack has a base damage.
                //**************************************************************************************
                if (ownerPointer.LockedAttack != Attacks.noAction)
                {
                    if (ownerPointer.DashingTimer > 0)
                    {
                        baseDamage += (int)(10F * speedDamageMultiplier.X);
                    }
                    damage = ((int)(40 * speedDamageMultiplier.X * MathHelper.Clamp(Math.Abs(ownerPointer.Speed.X) / 600, 0, 1))) + ((int)(40 * speedDamageMultiplier.Y * MathHelper.Clamp(Math.Abs(ownerPointer.Speed.Y) / 500, 0, 1))) + baseDamage;
                    speed.X += ownerPointer.Speed.X;
                    speed.Y += ownerPointer.Speed.Y;
                }
                else if (OwnerPointer.Speed.Y > 400 && (OwnerPointer.MyFlow.CurrentActionPosRight.X == 1 && OwnerPointer.MyFlow.CurrentActionPosRight.Y == 2))
                {
                    damage = 30 + (int)(20 * MathHelper.Clamp(Math.Abs(ownerPointer.Speed.Y) / 500, 0, 1));
                    rect.Width = 12;
                    rect.Height = 10;
                    pos.X = ownerPointer.Pos.X;
                    pos.Y += OwnerPointer.Rect.Height;
                    slashHeight = -1;
                }
            }
                rect.X = pos.X;
                rect.Y = pos.Y;
            
            //Updates the melee slash's grid location list for the purpose of prechecking for collision
            GridLocationUpdate();
            previousAttack = ownerPointer.LockedAttack;
        }


        //***************************************************
        // Method: Draw
        //
        // Purpose: Overrides the draw so that it doesn't
        // do anything.
        //***************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
        }



        //Properties
        public int Damage
        {
            get
            {
                return damage;
            }
        }

        public Facing Direction
        {
            get
            {
                return direction;
            }
        }

        public PlayerObject OwnerPointer
        {
            get
            {
                return ownerPointer;
            }
        }

        public int SlashHeight
        {
            get
            {
                return slashHeight;
            }
        }
    }
}
