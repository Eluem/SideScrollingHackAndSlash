//**********************************************************
// File: PlayerObject.cs
//
// Purpose: Contains the class definition of 
// PlayerObject. PlayerObject contains the methods and
// variables used to control a player controlled object
// in the game.
//
// Written By: Salvatore Hanusiewicz
//**********************************************************

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
    enum walking { backwards, forwards };
    enum Facing { left, right }; //Enumeration of the direction the player is facing
    enum OffDefState { attacking, blocking, neutral }; //Enumeration of whether the player is
                                                       //in the middle of an attack, blocking, or
                                                       //simply holding a stance

    enum spell{none, fireball, lightningbolt, forcepush, cancel}; //Enumeration for which spell the player is charing currently

    enum playerGrab {Nothing, Grabbing, Dropping, Throwing, Holding, Using, PickingUp };
    class PlayerObject : PhysicsObject
    {
        protected PlayerIndex player; //Stores which controller input controls this player

        protected int health; //Stores the player's health
        protected int mana;   //Stores the payer's mana
        protected float stamina; //Stores the player's stamina
        
        protected ActionFlow myFlow; //Stores the ActionFlow for this player. To be used to determine
                                     //actions triggered by moving the sticks in a certain pattern
                                     //(currently up to a maximum length of 5 stick positions);
        protected Facing facingDirection; //Stores the direction the player is facing
        protected OffDefState aggressionState; //Stores whether the player is swinging, blocking, or neither
        protected Vector2 maxPlayerSpeed; //Stores the maximum speed due to player's control
        protected Vector2 playerAccel; //Stores acceleration due to player's actions
        protected Vector2 playerRunningForce; //Stores how 'hard' the player's character is 'pushing' to move
        protected PlayerSprite playerSprite; //Stores the player's sprite object


        protected GamePadState gamePadState; //Stores the gamePadState so that it doesn't need to be called repeatedly
        protected GamePadState gamePadStateBackUp; //Stores the previous gamePadState to be used for forcing a
                                                   //player to release and press down a button again
        protected int maxAirJumps; //Stores the maximum number of air jumps the player can have (powerups may change this?)
        protected int airJumps; //Stores the number of air jumps the player currently has

        protected Boolean successfulBlock; //Stores a flag stating that the player successfully blocked an attack
        //protected Boolean swordHit; //Stores a flag that states that the player hit something with their sword
        protected Boolean blocked; //Stores a flag stating that the player has been blocked successfully
        protected Boolean hit; //Stores a flag stating that the player has been hit by a sword successfully
        protected int painTimer; //Stores the timer for the next time pain sounds can be played
        protected int hitTimer; //Stores a timer stating how long the player will be stunned
        protected int blockTimer; //Stores a timer stating how long the player has been blocking
        protected Boolean parried; //Stores a flag stating that the player has been parried
        protected int blockedStunTimer; //Stores a timer for how long the player will be stunned due to being blocked
        protected int parriedStunTimer; //Stores a timer for how long the player will be stunned due to being parried

        protected List<Point> last2SwordPositions; //Used to form a really cludgy/pseudo dynamic block animation (which WILL have issues)
        protected int blockedAnimationTimer; //Stores a timer for the block animation
        protected int blockedAnimationFrame; //Stores the current frame for the block animation


        protected int staminaTimer; //Stores a timer stating how long ago the player did some action last
                                   //This is to prevent the stamina from regenerating constantly while the player
                                   //attacks

        protected int staminaCrashTimer; //Stores a timer stating how long the player must wait to recharge stamina
                                         //because they drained their stamina completely

        protected int dashingTimer; //Stores a timer stating how long the player has left to dash
        protected int dashCooldownTimer; //Stores how long the player has to wait before dashing again

        protected Attacks lockedAttack; //Stores the attack that the player is currently locked in
        protected MeleeSlash001 myBlade; //Stores a pointer to the collision box of the player's sword attacks
        protected int attackLockDuration; //Stores a timer for how long the attack that is occuring must take
                                          //to complete before it releases the attack controls back to the player

        protected int clashSoundTimer; //Stores a timer for how much longer the playerObject must wait before
                                       //playing the clash sound again

        protected int magicChargeTimer; //Stores a timer for how long the player has been holding a spell button
        protected spell currentSpell; //Stores a flag for the current spell that the player is charging

        protected playerGrab grabbing; //Stores a flag stating the current grabbing state of the player

        protected walking walkingDirection; //Stores a flag stating the direction in which the 
                                            //player is walking (relative to their facing direction)

        protected Boolean staminaCrashed; //Stores a flag stating that the player has run out of stamina

        protected Attacks currentAttack; //Stores the player's current attack action
        protected Action currentAction; //Stores the player's current movement action

        protected Point previousSwordPosition; //Stores the player's previous sword position

        protected Boolean allowedToAttack; //Stores a flag stating whether or not the player is allowed to attack
        protected Boolean allowedToUseMagic; //Stores a flag stating whether or not the player is allowed to use magic

        protected int lightningBoltStunTimer; //Stores a timer for how long the player is stunned by lightning bolt

        protected Boolean grabbingLedge; //Stores a variable stating that the player is grabbing a ledge

        //****************************************************
        // Method: Player
        //
        // Purpose: Player constructor
        //****************************************************
        public PlayerObject(PlayerIndex Player)
            : base("", true, (int)Player, -1, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), 40, .7F, new Vector2(.02F, .005F), 0F /*.1F*/, .8F)
        {
            //Sets the player's starting positions
            List<Vector2> playerSpawnLocations = new List<Vector2>(); //List of locations that players can spawn at randomly

            if (Player == PlayerIndex.One)
            {
                playerSpawnLocations.Add(new Vector2(300, 850));
                playerSpawnLocations.Add(new Vector2(1130, 52));
            }
            else
            {
                playerSpawnLocations.Add(new Vector2(150, 52));
                playerSpawnLocations.Add(new Vector2(980, 850));
            }
            int tempRandomLocationIndex = GlobalVariables.Randomizer.Next(0, 2);
            pos.X = playerSpawnLocations[tempRandomLocationIndex].X;
            pos.Y = playerSpawnLocations[tempRandomLocationIndex].Y;


            
            health = GlobalVariables.Settings.startingHealth; //initializes the player's health based on the settings
            mana = GlobalVariables.Settings.startingMana;     //initializes the player's mana based on the settings
            stamina = GlobalVariables.Settings.startingStamina; //initializes the player's stamina based on the settings
            facingDirection = Facing.right;   //initializes the player to be facing to the right
            aggressionState = OffDefState.neutral; //initailizes the player to be in a neutral attack state
            player = Player;                      //Sets the controller that has control of this character

            //initializes the collision box dimensions
            rect.Width = 12;//56
            rect.Height = 74;//55


            //Generic initializations
            maxPlayerSpeed = new Vector2(300, 0);
            playerAccel = new Vector2(0, 0);
            playerRunningForce = new Vector2(0, 0);

            maxAirJumps = 1;
            airJumps = maxAirJumps;

            grabbing = playerGrab.Nothing;

            gamePadState = GamePad.GetState(Player);
            gamePadStateBackUp = gamePadState;

            myFlow = new ActionFlow();
            playerSprite = new PlayerSprite(new Point(0, 0), new Vector2(35,19));

            attackLockDuration = 0;
            lockedAttack = Attacks.noAction;
            allowedToAttack = true;

            grabbingLedge = false;


            painTimer = 0;
            dashingTimer = 0;
            dashCooldownTimer = 0;
            staminaTimer = 0;
            staminaCrashTimer = 0;
            clashSoundTimer = 0;
            currentSpell = spell.none;
            last2SwordPositions = new List<Point>();
            lightningBoltStunTimer = 0;

            myBlade = new MeleeSlash001(this);
            GlobalVariables.AddList.Add(myBlade);
        }

        //****************************************************
        // Method: Player
        //
        // Purpose: Player constructor for DeepCopy
        //****************************************************
        public PlayerObject(PlayerIndex Player, Vector2 Position, Vector2 Speed, Vector2 Accel, float Mass, Dictionary<side, Boolean> Sided, float Friction, float Bounciness, float Layer, int Health, int Mana, float Stamina, Facing FacingDirection, OffDefState AggressionState, FloatRectangle Rect, Vector2 MaxPlayerSpeed, Vector2 PlayerAccel, ActionFlow MyFlow, PlayerSprite PassedPlayerSprite, List<Point> GridLoc)
            : base("", true, (int)Player, -1, Position, Speed, Accel, Mass, Friction, new Vector2(.2F, .005F), Bounciness, Layer)
        {
            //Initialize the deep copy
            health = Health;
            mana = Mana;
            stamina = Stamina;
            sided = Sided;
            facingDirection = FacingDirection;
            aggressionState = AggressionState;
            player = Player;

            rect = Rect;

            maxPlayerSpeed = MaxPlayerSpeed;
            playerAccel = PlayerAccel;

            gridLoc = GridLoc;

            myFlow = MyFlow;
            playerSprite = PassedPlayerSprite;
        }



        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            if (hit)
            {
                GushBlood();
            }
            if (grabbingLedge)
            {
                airJumps = maxAirJumps;
                if (speed.X != 0 || speed.Y != 0)
                {
                    grabbingLedge = false;
                }
            }
            Controls(gameTime); //Responds to player input
            myBlade.Update();
            attackLockDuration -= gameTime.ElapsedGameTime.Milliseconds;
            painTimer -= gameTime.ElapsedGameTime.Milliseconds;
            hitTimer -= gameTime.ElapsedGameTime.Milliseconds;
            dashingTimer -= gameTime.ElapsedGameTime.Milliseconds;
            clashSoundTimer -= gameTime.ElapsedGameTime.Milliseconds;
            blockedStunTimer -= gameTime.ElapsedGameTime.Milliseconds;
            lightningBoltStunTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (blockedStunTimer <= 0)
            {
                parriedStunTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            if (blockedStunTimer <= 0)
            {
                blocked = false;
            }

            if (parriedStunTimer <= 0)
            {
                parried = false;
            }

            if (dashingTimer > 0)
            {
                if (facingDirection == Facing.right)
                {
                    speed.X = 800;
                }
                else
                {
                    speed.X = -800;
                }
            }
            dashCooldownTimer -= gameTime.ElapsedGameTime.Milliseconds;
            staminaTimer -= gameTime.ElapsedGameTime.Milliseconds;
            
            if (stamina == 0 && staminaCrashTimer <= 0)
            {
                staminaCrashTimer = 800;
                staminaCrashed = true;
            }
            staminaCrashTimer -= gameTime.ElapsedGameTime.Milliseconds;


            if (stamina < 100 && staminaTimer <= 0 && staminaCrashTimer <= 0)
            {
                stamina += 15 / 60F;
                if (stamina > 100)
                {
                    stamina = 100;
                }
            }

            if (stamina > 10)
            {
                staminaCrashed = false;
            }

            if (health <= 0 && !deleted)
            {
                Death();
            }

            if (grabbingLedge)
            {
                rect.X = pos.X;
                rect.Y = pos.Y;
            }
            else
            {
                base.Update(gameTime, port);
            }
            playerSprite.Update(gameTime, speed, walkingDirection, !sided[side.bottom] && Math.Abs(speed.Y) > 0);
        }

        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            bool baseCollideAnyway = true; //Used to determine if the base collision method should be called.


            if (obj.SpecialProperties) //Determines if a special collision should occur
            {
                //********************************************************
                // Some objects in my game need to be interacted
                // with (by certain other objects) in a specific
                // manner, which will not be defined in the base
                // collision method. When, for example, a
                // PlayerObject hits a MeleeSlash001's collision box
                // the PlayerObject will need to interact with the
                // MeleeSlash001 in a specific manner which will
                // require accessing variables that only a
                // MeleeSlash001 has. This access cannot occur outside
                // of an if statement or try/catch structure
                // otherwise the program will crash if the object
                // is anything but a MeleeSlash001.
                //********************************************************
                if (obj is SpinningCoin)
                {
                    SpinningCoin temp = ((SpinningCoin)obj); //Casts the PhysicsObject up into a SpinningCoin
                    baseCollideAnyway = false; //Disables the base collision
                    GlobalVariables.AddList.Add(new SpinningCoin(new Vector2(GlobalVariables.Randomizer.Next(0, 500), GlobalVariables.Randomizer.Next(400, 500))));
                    ++maxAirJumps;
                }
                else if (trueObjPointer is GrabCollisionBox) //Prevents players from bumping into grab boxes
                {
                    baseCollideAnyway = false;
                }
                else if (obj is PlayerObject)
                {
                    PlayerObject temp = ((PlayerObject)obj); //Casts the PhysicsObject up into a PlayerObject
                }
                else if (trueObjPointer is ThrowableObject)//Prevents players from bumping into throwable objects that they are holding
                {
                    ThrowableObject tempThrowableObject = ((ThrowableObject)trueObjPointer);
                    if (tempThrowableObject.Grabber != null || (tempThrowableObject.OldGrabber != null && tempThrowableObject.OldGrabber.Equals(this)))
                    {
                        baseCollideAnyway = false;
                    }
                    //Handles players colliding with fire balls
                    if (trueObjPointer is FireBall && (((FireBall)trueObjPointer).OwnerSafeTimer <= 0 || !((FireBall)trueObjPointer).OwnerPointer.Equals(this)))
                    {
                        hit = true;
                        health -= 20;
                    }
                }
                else if (obj is TestForcePush) //Prevents players from behing hit by my test force push object
                {
                    baseCollideAnyway = false;
                }

                else if (trueObjPointer is SmallSpinningCoin) //Prevents players from stopping when they hit small spinning coins
                {
                    baseCollideAnyway = false;
                }

                //Causes player to be damaged by the fireballs
                //that bombs project and prevents players from being
                //stopped when they hit them.
                else if (trueObjPointer is BombFire) 
                {
                    baseCollideAnyway = false;
                    health -= 15;
                }

                //Causes players to explode when they are touched by an explosion
                else if (trueObjPointer is Explosion)
                {
                    baseCollideAnyway = false;
                    BlownUp();
                }

                else if (trueObjPointer is ConcExplosion)
                {
                    baseCollideAnyway = false;
                    if (obj.Rect.Center.X < rect.Center.X)
                    {
                        speed.X += 4000 / mass;
                    }
                    else
                    {
                        speed.X -= 4000 / mass;
                    }
                    speed.Y -= 1600 / mass;
                    if (lightningBoltStunTimer <= 800)
                    {
                        lightningBoltStunTimer = 800;
                    }
                }
                //Prevents object from colliding with the PlayerBlood effect (should have an effects list)....
                else if (trueObjPointer is PlayerBlood)
                {
                    baseCollideAnyway = false;
                }
                else if (trueObjPointer is DeathAnimation)
                {
                    baseCollideAnyway = false;
                }

                /*
                //Handles players being hit by FireBalls
                else if (trueObjPointer is FireBall)
                {
                    baseCollideAnyway = false;
                    if (((FireBall)trueObjPointer).OwnerSafeTimer <= 0 || !((FireBall)trueObjPointer).OwnerPointer.Equals(this))
                    {
                        hit = true;
                        health -= 20;
                    }
                }
                */
                //handles player being hit by a force push
                else if (trueObjPointer is ForcePush)
                {
                    baseCollideAnyway = false;
                    if (((ForcePush)trueObjPointer).Direction == Facing.right)
                    {
                        speed.X = 400;
                    }
                    else
                    {
                        speed.X = -400;
                    }
                    speed.Y -= 200;
                }

                //handles player being hit by a lightning bolt
                else if (trueObjPointer is LightningBolt)
                {
                    hit = true;
                    baseCollideAnyway = false;
                    health -= 10;
                    lightningBoltStunTimer = ((LightningBolt)trueObjPointer).LifeTime * 6;
                    if (lightningBoltStunTimer > 15000)
                    {
                        lightningBoltStunTimer = 15000;
                    }
                }

                //Handels players being hit by a melee slash
                //if a player blocks successfully that is detected here
                else if (trueObjPointer is MeleeSlash001)
                {
                    baseCollideAnyway = false;
                    if (trueObjPointer.OwnerID != objectID)
                    {
                        MeleeSlash001 tempObj = ((MeleeSlash001)trueObjPointer);
                        if (tempObj.Damage > 0)
                        {
                            if (aggressionState == OffDefState.blocking)
                            {
                                if (tempObj.Direction != facingDirection && tempObj.SlashHeight == myFlow.CurrentActionPosRight.Y)
                                {
                                    tempObj.OwnerPointer.Blocked = true; //notifies the player that they were successfully blocked
                                    tempObj.OwnerPointer.blockedStunTimer = 600; //Sets their stun timer for being blocked
                                    tempObj.OwnerPointer.blockedAnimationFrame = tempObj.OwnerPointer.Last2SwordPositions.Count - 1;
                                    if (blockTimer < 250)
                                    {
                                        tempObj.OwnerPointer.Parried = true; //notifies the player that they were successfully parried
                                        tempObj.OwnerPointer.ParriedStunTimer = 1000; //Sets their stun timer for being parried
                                    }
                                    successfulBlock = true; //sets the flag stating that the player blocked successfully to true
                                    if (clashSoundTimer <= 0)
                                    {
                                        clashSoundTimer = 100;
                                        GlobalVariables.SoundBank.PlayCue("Sword Clash");
                                    }
                                }
                                else
                                {
                                    //registers a hit
                                    health -= tempObj.Damage; //damages the player

                                    //Knocks the player up/back if they're hit
                                    speed.X += trueObjPointer.Speed.X / (mass / 3);
                                    speed.Y += trueObjPointer.Speed.Y / (mass / 3);
                                    hit = true;
                                    hitTimer = 100;
                                }
                            }
                            else
                            {
                                //registers a hit
                                health -= tempObj.Damage; //damages the player

                                //Knocks the player up/back if they're hit
                                speed.X += trueObjPointer.Speed.X / (mass / 3);
                                speed.Y += trueObjPointer.Speed.Y / (mass / 3);
                                hit = true;
                                hitTimer = 100;
                            }
                        }
                    }
                }
            }

            //handles ledge grabbing
            if (trueObjPointer is ScaleablePlatform)
            {
                if (grabbing == playerGrab.Nothing && overLap.X < overLap.Y && speed.Y > -20)
                {
                    if (rect.Center.X < obj.Rect.Center.X && facingDirection == Facing.right && pos.Y < trueObjPointer.Pos.Y - 10 && pos.Y > trueObjPointer.Pos.Y - 40 && gamePadState.ThumbSticks.Left.X > .5F) //hit the left side
                    {
                        baseCollideAnyway = false;
                        grabbingLedge = true;
                        pos.Y = trueObjPointer.Pos.Y - 10;
                        speed.X = 0;
                        speed.Y = 0;
                    }
                    else if (facingDirection == Facing.left && pos.Y < trueObjPointer.Pos.Y - 10 && pos.Y > trueObjPointer.Pos.Y - 40 && gamePadState.ThumbSticks.Left.X < -.5F) //hit the right side
                    {
                        baseCollideAnyway = false;
                        grabbingLedge = true;
                        pos.Y = trueObjPointer.Pos.Y - 10;
                        speed.X = 0;
                        speed.Y = 0;
                    }
                }
                else if (grabbingLedge)
                {
                    baseCollideAnyway = false;
                }
            }
            
            if (baseCollideAnyway) //Determines if the base collision method should still be called
            {
                Vector2 overLapBeforeAppliable = overLap; //Backs up the old overlap for use later
                side collisionSide = side.top; //Used to store the side the collision occured on
                axis collisionAxis = axis.x; //Used to store the axis upon which the collision occured

                //REQUIRES WORK

                //Obtains the total friction between objects
                //1 removes all speed from the object
                //0 doesn't affect the object
                float totalFriction = obj.Friction + friction;
                if (totalFriction > 1)
                {
                    totalFriction = 1F;
                }
                if (obj.Friction * friction == 0)
                {
                    totalFriction = 0F;
                }

                //*********************************************************************
                // overLapAppliable is Used to apply only the amount of the overLap
                // that the object contributed.
                //*********************************************************************
                Vector2 overLapAppliable = new Vector2(Math.Abs(speed.X) / (Math.Abs(speed.X) + Math.Abs(obj.Speed.X)), Math.Abs(speed.Y) / (Math.Abs(speed.Y) + Math.Abs(obj.Speed.Y)));


                //*******************************************************
                //Used to determine which side the collision occured on
                //*******************************************************
                if (rect.Center != obj.Rect.Center) //Makes sure the objects don't overlap on their center
                {
                    if (overLap.X < overLap.Y) //The collision occured on the x axis
                    {
                        collisionAxis = axis.x; //Sets the axis of collision to x

                        if (rect.Center.X < obj.Rect.Center.X) //hit the left side
                        {
                            collisionSide = side.left;
                        }
                        else //hit the right side
                        {
                            collisionSide = side.right;
                        }

                    }
                    else //collision occured on y axis
                    {
                        collisionAxis = axis.y; //Sets the axis of collision to y

                        if (rect.Center.Y < obj.Rect.Center.Y) //landed on top of the object
                        {
                            collisionSide = side.top;
                        }

                        else //hit the bottom of the object
                        {
                            collisionSide = side.bottom;
                        }
                    }
                }

                else //If the objects have the same exact center position something different needs to be done
                {
                    //As long as the object's previous position isn't the same as its current position
                    //this should work
                    if ((int)prevPos.X != (int)obj.PrevPos.X || (int)prevPos.Y != (int)obj.PrevPos.Y)
                    {
                        pos = prevPos;
                    }
                    //Moves the object in a "random" direction by 1
                    //Useful if objects are spawned directly on each other
                    else
                    {
                        switch (GlobalVariables.Randomizer.Next(0, 4))
                        {
                            case 0:
                                collisionAxis = axis.y;
                                collisionSide = side.top;
                                break;
                            case 1:
                                collisionSide = side.bottom;
                                collisionAxis = axis.y;
                                break;
                            case 2:
                                collisionSide = side.right;
                                collisionAxis = axis.x;
                                break;
                            case 3:
                                collisionSide = side.left;
                                collisionAxis = axis.x;
                                break;
                        }
                    }

                }

                //******************
                // Apply collisions
                //******************
                if (collisionAxis == axis.x)
                {
                    //************************************************************************************************
                    //Applies overLapAppliable to the overLap for the x axis if and only if the calculation succeeded
                    //otherwise the overLap is applied in full because of the fact that, for the overLapAppliable
                    //to fail its equation, both objects need a 0 speed. Thus, the descrepency in movement would
                    //either not exist or be so miniscule that it does not matter.
                    //************************************************************************************************
                    if (!overLapAppliable.X.Equals(float.NaN))
                    {
                        overLap.X *= overLapAppliable.X;
                    }

                    if (collisionSide == side.left)
                    {
                        pos.X -= overLap.X; //Moves the object out of the other object by the correct amount
                        sided[side.left] = true;
                    }
                    else
                    {
                        pos.X += overLap.X; //Moves the object out of the other object by the correct amount
                        sided[side.right] = true;
                    }

                    prevSpeed.X = speed.X; //Stores the speed of the object before the collision occurs
                    //Everything in here should on occur during an overlap
                    //(somethings should occur when there's no overlap but the objects are next to each other"
                    if (overLapBeforeAppliable.X > 0 && !(collisionSide == side.left && speed.X < 0 && obj.Speed.X > speed.X) && !(collisionSide == side.right && speed.X > 0 && obj.Speed.X < speed.X) && !(collisionSide == side.left && obj.Speed.X > 0 && obj.Speed.X > speed.X) && !(collisionSide == side.right && obj.Speed.X < 0 && obj.Speed.X < speed.X))
                    {
                        //Applies conservation of momentum
                        speed.X = (((((mass - obj.Mass) / (mass + obj.Mass)) * speed.X) + (((2 * obj.Mass) / (mass + obj.Mass)) * obj.Speed.X)));
                        //speed.X += (speed.X - prevSpeed.X) * (bounciness + obj.Bounciness); //Applies equal and opposite force of impact
                        //speed.X = ((speed.X * (mass - obj.Mass) + 2 * (obj.Mass * obj.Speed.X)) / (mass + obj.Mass));
                        speed.X *= bounciness + obj.Bounciness; //Applies bounciness
                    }

                    //Applies friction to the Y axis

                    //Determines the direction the object is moving in.
                    if (speed.Y > 0)
                    {
                        //**********************************************************************
                        // This calculates the change in speed due to friction.
                        // totalFriction acts as the coefficent of friction for the equation
                        // which calculates frictional force. The rest of the equation I
                        // created by looking up and remembering the physics equations for
                        // impulse and force, then I divided by mass to find the accleration
                        // that I should apply.
                        //**********************************************************************
                        speed.Y -= (totalFriction * (prevSpeed.X - speed.X + accel.X)) / GlobalVariables.Settings.fps;
                        if (speed.Y < 0)
                        {
                            speed.Y = 0; //This prevents friction from changing an object's direction of movement
                        }
                    }
                    else if (speed.Y < 0)
                    {
                        //**********************************************************************
                        // This calculates the change in speed due to friction.
                        // totalFriction acts as the coefficent of friction for the equation
                        // which calculates frictional force. The rest of the equation I
                        // created by looking up and remembering the physics equations for
                        // impulse and force, then I divided by mass to find the accleration
                        // that I should apply.
                        //**********************************************************************
                        speed.Y += (totalFriction * (prevSpeed.X - speed.X + accel.X)) / GlobalVariables.Settings.fps;
                        if (speed.Y > 0)
                        {
                            speed.Y = 0; //This prevents friction from changing an object's direction of movement
                        }
                    }
                }
                else
                {
                    //************************************************************************************************
                    //Applies overLapAppliable to the overLap for the y axis if and only if the calculation succeeded
                    //************************************************************************************************
                    if (!overLapAppliable.Y.Equals(float.NaN))
                    {
                        overLap.Y *= overLapAppliable.Y;
                    }

                    if (collisionSide == side.top)
                    {
                        pos.Y -= overLap.Y;//Moves the object out of the other object by the correct amount
                        sided[side.bottom] = true; //Sets the flag stating that the object is on a surface to true
                        airJumps = maxAirJumps; //Resets the number of air jumps the player gets
                    }
                    else
                    {
                        pos.Y += overLap.Y; //Moves the object out of the other object by the correct amount
                        sided[side.top] = true;
                    }

                    prevSpeed.Y = speed.Y; //Stores the speed of the object before the collision occurs

                    //Everything in here should on occur during an overlap
                    //(somethings should occur when there's no overlap but the objects are next to each other"
                    if (overLapBeforeAppliable.Y > 0 && !(collisionSide == side.top && speed.Y < 0 && obj.Speed.Y > speed.Y) && !(collisionSide == side.bottom && speed.Y > 0 && obj.Speed.Y < speed.Y) && !(collisionSide == side.top && obj.Speed.Y > 0 && obj.Speed.Y > speed.Y) && !(collisionSide == side.bottom && obj.Speed.Y < 0 && obj.Speed.Y < speed.Y))
                    {
                        //Apply conservation of momentum
                        speed.Y = (((mass - obj.Mass) / (mass + obj.Mass)) * speed.Y) + (((2 * obj.Mass) / (mass + obj.Mass)) * obj.Speed.Y);
                        //speed.Y = ((speed.Y * (mass - obj.Mass) + 2 * (obj.Mass * obj.Speed.Y)) / (mass + obj.Mass));

                        //Cludge to prevent player jumping issues
                        if (collisionSide == side.bottom)
                        {
                            speed.Y *= bounciness + obj.Bounciness; //Applies bounciness
                        }
                        else
                        {
                            speed.Y = 0;
                        }
                    }
                    //Applies friction to the X axis

                    //Determines the direction the object is moving in.
                    if (speed.X > 0)
                    {
                        Boolean speedWasGreaterBeforeChange = playerRunningForce.X > 0F && speed.X > maxPlayerSpeed.X * playerRunningForce.X;
                        //**********************************************************************
                        // The equation below calculates the change in speed due to friction.
                        // totalFriction acts as the coefficent of friction for the equation
                        // which calculates frictional force. The rest of the equation I
                        // created by looking up and remembering the physics equations for
                        // impulse and force, then I divided by mass to find the accleration
                        // that I should apply. playerRunningForce is multiplied into the
                        // equation to determine what percentage of the maximum acceleration
                        // due to the friction should be applied.
                        //**********************************************************************
                        speed.X += (totalFriction * (prevSpeed.Y - speed.Y + accel.Y + GlobalVariables.Settings.Gravity) * playerRunningForce.X) / GlobalVariables.Settings.fps;
                        if(playerRunningForce.X > 0F && speed.X > maxPlayerSpeed.X * playerRunningForce.X && !speedWasGreaterBeforeChange)
                        {
                            speed.X = maxPlayerSpeed.X * playerRunningForce.X;
                        }
                        if (playerRunningForce.X == 0)
                        {
                                speed.X -= 2;
                        }
                        if (speed.X < 0)
                        {
                            speed.X = 0; //This prevents friction from changing an object's direction of movement
                        }
                    }
                    else if (speed.X < 0)
                    {
                        Boolean speedWasLessBeforeChange = playerRunningForce.X < 0F && speed.X < maxPlayerSpeed.X * playerRunningForce.X;
                        //**********************************************************************
                        // The equation below calculates the change in speed due to friction.
                        // totalFriction acts as the coefficent of friction for the equation
                        // which calculates frictional force. The rest of the equation I
                        // created by looking up and remembering the physics equations for
                        // impulse and force, then I divided by mass to find the accleration
                        // that I should apply. playerRunningForce is multiplied into the
                        // equation to determine what percentage of the maximum acceleration
                        // due to the friction should be applied.
                        //**********************************************************************
                        speed.X += (totalFriction * (prevSpeed.Y - speed.Y + accel.Y + GlobalVariables.Settings.Gravity) * playerRunningForce.X) / GlobalVariables.Settings.fps;
                        if (playerRunningForce.X < 0F && speed.X < maxPlayerSpeed.X * playerRunningForce.X && !speedWasLessBeforeChange)
                        {
                            speed.X = maxPlayerSpeed.X * playerRunningForce.X;
                        }
                        if (playerRunningForce.X == 0)
                        {
                            speed.X += 2;
                        }
                        if (speed.X > 0)
                        {
                            speed.X = 0; //This prevents friction from changing an object's direction of movement
                        }
                    }
                    else
                    {
                        //**********************************************************************
                        // The equation below calculates the change in speed due to friction.
                        // totalFriction acts as the coefficent of friction for the equation
                        // which calculates frictional force. The rest of the equation I
                        // created by looking up and remembering the physics equations for
                        // impulse and force, then I divided by mass to find the accleration
                        // that I should apply. playerRunningForce is multiplied into the
                        // equation to determine what percentage of the maximum acceleration
                        // due to the friction should be applied.
                        //**********************************************************************
                        speed.X += (totalFriction * (prevSpeed.Y - speed.Y + accel.Y + GlobalVariables.Settings.Gravity) * playerRunningForce.X) / GlobalVariables.Settings.fps;
                    }
                }
            }
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Calls the draw functions of the sprites
        // that the object contains.
        //****************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.UpdatePos(pos, facingDirection); //Updates the position of the sprite based on the true position
            playerSprite.Draw(spriteBatch,facingDirection, layer, hitTimer); //Draws the character to the screen
        }

        //****************************************************
        // Method: Controls
        //
        // Purpose: To define how the player object reacts
        // to input from a controller.
        //****************************************************
        public virtual void Controls(GameTime gameTime)
        {
            //REQUIRES WORK
            if (!hit && lightningBoltStunTimer <= 0)
            {
                gamePadState = GamePad.GetState(player);
                //Attempt at implementing ActionFlows
                myFlow.Update(gameTime, gamePadState);
                if (myFlow.CurrentActionPosRight != previousSwordPosition)
                {
                    stamina = MathHelper.Clamp(stamina - .5F, 0F, 100F);
                    staminaTimer = 250;
                }

                if (attackLockDuration <= 0)
                {
                    lockedAttack = Attacks.noAction;
                }

                //Updates the player's current action variables
                if (grabbing != playerGrab.Nothing || aggressionState == OffDefState.blocking || blocked || parried || grabbingLedge)
                {
                    currentAttack = Attacks.noAction;
                    allowedToAttack = false;
                    allowedToUseMagic = false;
                }
                else
                {
                    currentAttack = myFlow.rightAction();
                    allowedToAttack = true;
                    allowedToUseMagic = true;
                }
                if (stamina <= 0)
                {
                    allowedToAttack = false;
                }
                if (mana <= 0)
                {
                    allowedToUseMagic = false;
                }
                currentAction = myFlow.leftAction();

                if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed && gamePadStateBackUp.Buttons.LeftShoulder == ButtonState.Released)
                {
                    currentAction = Action.jump;
                }

                switch (currentAction)
                {
                    case Action.jump:
                        if (sided[side.bottom] || grabbingLedge)
                        {
                            speed.Y = -395;
                            grabbingLedge = false;
                        }
                        else if (airJumps > 0)
                        {
                            --airJumps;
                            speed.Y = -280;
                            if ((gamePadState.ThumbSticks.Left.X / speed.X < 0) || (gamePadState.ThumbSticks.Left.X / -gamePadState.ThumbSticks.Left.X == speed.X / -speed.X && Math.Abs(gamePadState.ThumbSticks.Left.X) * 100 > Math.Abs(speed.X)))
                            {
                                speed.X = gamePadState.ThumbSticks.Left.X * 100;
                            }
                        }
                        break;

                    case Action.dashRight:
                        if (dashCooldownTimer <= 0 && sided[side.bottom] && facingDirection == Facing.right && mana >= 10)
                        {
                            dashingTimer = 100;
                            dashCooldownTimer = 1500;
                            mana -= 10;
                        }
                        break;
                    case Action.dashLeft:
                        if (dashCooldownTimer <= 0 && sided[side.bottom] && facingDirection == Facing.left && mana >= 10)
                        {
                            dashingTimer = 100;
                            dashCooldownTimer = 1500;
                            mana -= 10;
                        }
                        break;
                    default:
                        break;
                }

                //Handles the flow of different attacks (very shotty code imo)
                if (allowedToAttack && attackLockDuration <= 0)
                {
                    if (facingDirection == Facing.right)
                    {
                        switch (currentAttack)
                        {
                            case Attacks.ShortJabRight:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.ShortJabRight;
                                break;
                            case Attacks.LongJabRight:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.LongJabRight;
                                break;
                            case Attacks.LongOverheadRight:
                                attackLockDuration = 400;
                                lockedAttack = Attacks.LongOverheadRight;
                                break;
                            case Attacks.UpsweepRight:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.UpsweepRight;
                                break;
                        }
                    }
                    else
                    {
                        switch (currentAttack)
                        {
                            case Attacks.ShortJabLeft:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.ShortJabLeft;
                                break;
                            case Attacks.LongJabLeft:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.LongJabLeft;
                                break;
                            case Attacks.LongOverheadLeft:
                                attackLockDuration = 400;
                                lockedAttack = Attacks.LongOverheadLeft;
                                break;
                            case Attacks.UpsweepLeft:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.UpsweepLeft;
                                break;
                        }
                    }
                }

                //Updates the player's aggressionState
                if (attackLockDuration > 0)
                {
                    aggressionState = OffDefState.attacking;
                    blockTimer = 0;
                }
                else if (gamePadState.Triggers.Right > .9F && stamina > 0 && !(parried || blocked) && !staminaCrashed && grabbing == playerGrab.Nothing && !grabbingLedge)
                {
                    stamina -= 10 / 60F;
                    staminaTimer = 250;
                    blockTimer += gameTime.ElapsedGameTime.Milliseconds;
                    aggressionState = OffDefState.blocking;
                    if (stamina < 0)
                    {
                        stamina = 0;
                    }
                }
                else
                {
                    aggressionState = OffDefState.neutral;
                    blockTimer = 0;
                }

                if (!blocked && !parried)
                {
                    //Update's the player's sword arm sprite
                    if (aggressionState == OffDefState.neutral)
                    {
                        if (allowedToAttack)
                        {
                            if (facingDirection == Facing.left)
                            {
                                playerSprite.SelectFrame(myFlow.CurrentActionPosRight.X, myFlow.CurrentActionPosRight.Y);
                                Point addPoint = new Point(myFlow.CurrentActionPosRight.X, myFlow.CurrentActionPosRight.Y);
                                if (last2SwordPositions.Count == 0 || addPoint != last2SwordPositions[last2SwordPositions.Count - 1])
                                {
                                    if (last2SwordPositions.Count < 2)
                                    {
                                        last2SwordPositions.Add(addPoint);
                                    }
                                    else
                                    {
                                        last2SwordPositions.Remove(Last2SwordPositions[0]);
                                        last2SwordPositions.Add(addPoint);
                                    }
                                }
                            }
                            else
                            {
                                Point tempPoint = new Point(myFlow.CurrentActionPosRight.X, myFlow.CurrentActionPosRight.Y);
                                tempPoint.X = -tempPoint.X + 2;
                                playerSprite.SelectFrame(tempPoint.X, tempPoint.Y);
                                Point addPoint = tempPoint;
                                if (last2SwordPositions.Count ==0 || addPoint != last2SwordPositions[last2SwordPositions.Count - 1])
                                {
                                    if (last2SwordPositions.Count < 2)
                                    {
                                        last2SwordPositions.Add(addPoint);
                                    }
                                    else
                                    {
                                        last2SwordPositions.Remove(Last2SwordPositions[0]);
                                        last2SwordPositions.Add(addPoint);
                                    }
                                }
                            }
                        }
                        else
                        {
                            playerSprite.SelectFrame(1, 1);
                        }
                    }
                    else if (aggressionState == OffDefState.blocking)
                    {
                        playerSprite.SelectFrame(4, myFlow.CurrentActionPosRight.Y);
                    }
                    else
                    {
                        if (lockedAttack == Attacks.ShortJabRight || lockedAttack == Attacks.ShortJabLeft)
                        {
                            playerSprite.SelectFrame(3, 1);
                            if (last2SwordPositions.Count < 2)
                            {
                                last2SwordPositions.Add(new Point(3, 1));
                            }
                            else
                            {
                                last2SwordPositions.Remove(Last2SwordPositions[0]);
                                last2SwordPositions.Add(new Point(3, 1));
                            }
                            if (currentAttack != Attacks.LongJabRight && currentAttack != Attacks.LongJabLeft)
                            {
                                currentAttack = Attacks.noAction;
                            }
                        }
                        else if (lockedAttack == Attacks.LongJabRight || lockedAttack == Attacks.LongJabLeft)
                        {
                            playerSprite.SelectFrame(0, 1);
                            currentAttack = Attacks.noAction;
                        }
                        else if (lockedAttack == Attacks.UpsweepRight || lockedAttack == Attacks.UpsweepLeft)
                        {
                            playerSprite.SelectFrame(3, 2);
                            currentAttack = Attacks.noAction;
                        }
                        else if (lockedAttack == Attacks.LongOverheadLeft || lockedAttack == Attacks.LongOverheadRight)
                        {
                            if (attackLockDuration > 340)
                            {
                                playerSprite.SelectFrame(0, 1);
                            }
                            else
                            {
                                playerSprite.SelectFrame(0, 2);
                            }
                            currentAttack = Attacks.noAction;
                        }
                    }
                }
                else
                {
                    playerSprite.SelectFrame(Last2SwordPositions[blockedAnimationFrame].X, Last2SwordPositions[blockedAnimationFrame].Y);
                    blockedAnimationTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if (blockedAnimationTimer <= 0)
                    {
                        --blockedAnimationFrame;
                        if (blockedAnimationFrame < 0)
                        {
                            blockedAnimationFrame = 0;
                        }
                        blockedAnimationTimer = 100;
                    }
                }

                //Handles the flow of different attacks (very shotty code imo)
                if (allowedToAttack)
                {
                    if (facingDirection == Facing.right)
                    {
                        switch (currentAttack)
                        {
                            case Attacks.ShortJabRight:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.ShortJabRight;
                                break;
                            case Attacks.LongJabRight:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.LongJabRight;
                                break;
                            case Attacks.LongOverheadRight:
                                attackLockDuration = 400;
                                lockedAttack = Attacks.LongOverheadRight;
                                break;
                            case Attacks.UpsweepRight:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.UpsweepRight;
                                break;
                        }
                    }
                    else
                    {
                        switch (currentAttack)
                        {
                            case Attacks.ShortJabLeft:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.ShortJabLeft;
                                break;
                            case Attacks.LongJabLeft:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.LongJabLeft;
                                break;
                            case Attacks.LongOverheadLeft:
                                attackLockDuration = 400;
                                lockedAttack = Attacks.LongOverheadLeft;
                                break;
                            case Attacks.UpsweepLeft:
                                attackLockDuration = 300;
                                lockedAttack = Attacks.UpsweepLeft;
                                break;
                        }
                    }
                }

                if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    magicChargeTimer = 0;
                    currentSpell = spell.cancel;
                }

                //Handles the player's ability to use magic
                if (allowedToUseMagic)
                {
                    //Check for spell charging

                    //Lightning bolt charge
                    if (gamePadState.Buttons.Y == ButtonState.Pressed && (currentSpell == spell.none || currentSpell == spell.lightningbolt))
                    {
                        currentSpell = spell.lightningbolt;
                        magicChargeTimer += gameTime.ElapsedGameTime.Milliseconds;
                    }

                    //Force push charge
                    else if (gamePadState.Buttons.X == ButtonState.Pressed && (currentSpell == spell.none || currentSpell == spell.forcepush))
                    {
                        currentSpell = spell.forcepush;
                        magicChargeTimer += gameTime.ElapsedGameTime.Milliseconds;
                    }
                     
                    //Fire ball charge
                    else if (gamePadState.Buttons.B == ButtonState.Pressed && (currentSpell == spell.none || currentSpell == spell.fireball))
                    {
                        currentSpell = spell.fireball;
                        magicChargeTimer += gameTime.ElapsedGameTime.Milliseconds;
                    }

                    //Fire ball cast
                    if (gamePadState.Buttons.B == ButtonState.Released && currentSpell == spell.fireball)
                    {
                        currentSpell = spell.none;
                        if (magicChargeTimer > 350 && mana - 25 >= 0)
                        {
                            GlobalVariables.AddList.Add(new FireBall(this));
                            mana -= 25;
                        }
                    }

                    //lightning bolt cast
                    else if (gamePadState.Buttons.Y == ButtonState.Released && currentSpell == spell.lightningbolt)
                    {
                        currentSpell = spell.none;
                        if (magicChargeTimer > 0 && mana - 50 >= 0)
                        {
                            GlobalVariables.AddList.Add(new LightningBolt(this));
                            mana -= 50;
                        }
                    }

                    //Force Push cast
                    else if (gamePadState.Buttons.X == ButtonState.Released && currentSpell == spell.forcepush)
                    {
                        currentSpell = spell.none;
                        if (magicChargeTimer > 400 && mana - 15 >= 0)
                        {
                            GlobalVariables.AddList.Add(new ForcePush(this));
                            mana -= 15;
                        }
                    }
                }
                if (currentSpell == spell.none)
                {
                    magicChargeTimer = 0;
                }
                if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    magicChargeTimer = 0;
                    currentSpell = spell.cancel;
                }
                if (currentSpell == spell.cancel && gamePadState.Buttons.Y == ButtonState.Released && gamePadState.Buttons.B == ButtonState.Released && gamePadState.Buttons.X == ButtonState.Released)
                {
                    currentSpell = spell.none;
                }

                if (!grabbingLedge)
                {
                    if ((gamePadState.Buttons.RightShoulder == ButtonState.Pressed && gamePadStateBackUp.Buttons.RightShoulder == ButtonState.Released) && grabbing == playerGrab.Nothing)
                    {
                        magicChargeTimer = 0;
                        currentSpell = spell.none;
                        grabbing = playerGrab.Grabbing;
                        GlobalVariables.AddList.Add(new GrabCollisionBox(this));
                    }
                    else if (gamePadState.Buttons.RightShoulder == ButtonState.Released && grabbing == playerGrab.PickingUp)
                    {
                        grabbing = playerGrab.Holding;
                    }
                    else if ((gamePadState.Buttons.A == ButtonState.Pressed && gamePadStateBackUp.Buttons.A == ButtonState.Released) && grabbing == playerGrab.Holding)
                    {
                        grabbing = playerGrab.Using;
                    }
                    else if ((gamePadState.Buttons.RightShoulder == ButtonState.Pressed && gamePadStateBackUp.Buttons.RightShoulder == ButtonState.Released) && grabbing == playerGrab.Holding)
                    {
                        grabbing = playerGrab.Throwing;
                    }
                    /*
                    else if ((gamePadState.Buttons.B == ButtonState.Pressed && gamePadStateBackUp.Buttons.B == ButtonState.Released) && grabbing == playerGrab.Holding)
                    {
                        grabbing = playerGrab.Dropping;
                    }
                    */
                    else if (grabbing != playerGrab.Holding && grabbing != playerGrab.Using && grabbing != playerGrab.PickingUp)
                    {
                        grabbing = playerGrab.Nothing;
                    }
                    else if (grabbing == playerGrab.Using)
                    {
                        grabbing = playerGrab.Holding;
                    }

                    if (sided[side.bottom])
                    {
                        playerRunningForce.X = gamePadState.ThumbSticks.Left.X;
                        if (walkingDirection == walking.backwards)
                        {
                            playerRunningForce.X /= 2;
                        }
                    }
                    else
                    {
                        speed.X += gamePadState.ThumbSticks.Left.X * 300 * airResistance.X;
                    }

                    //walking direction and facing direction controls
                    walkingDirection = walking.forwards;
                    if (speed.X > 0)
                    {
                        if (facingDirection == Facing.left)
                        {
                            if (gamePadState.ThumbSticks.Left.X > 0 && gamePadState.Triggers.Left < .9F)
                            {
                                facingDirection = Facing.right;
                            }
                            else
                            {
                                walkingDirection = walking.backwards;
                            }
                        }
                        else
                        {
                            facingDirection = Facing.right;
                        }
                    }
                    else if (speed.X < 0)
                    {
                        if (facingDirection == Facing.right)
                        {
                            if (gamePadState.ThumbSticks.Left.X < 0 && gamePadState.Triggers.Left < .9F)
                            {
                                facingDirection = Facing.left;
                            }
                            else
                            {
                                walkingDirection = walking.backwards;
                            }
                        }
                        else
                        {
                            facingDirection = Facing.left;
                        }
                    }
                    if (walkingDirection == walking.forwards && gamePadState.Buttons.LeftStick == ButtonState.Pressed && Math.Abs(speed.X) > 0)
                    {
                        walkingDirection = walking.backwards;
                        if (facingDirection == Facing.right)
                        {
                            facingDirection = Facing.left;
                        }
                        else
                        {
                            facingDirection = Facing.right;
                        }
                    }
                }
                if (gamePadState.ThumbSticks.Left.Y < -.5F)
                {
                    grabbingLedge = false;
                }
            }
            else
            {
                currentAttack = Attacks.noAction;
                lockedAttack = Attacks.noAction;
                attackLockDuration = 0;
                currentSpell = 0;
                magicChargeTimer = 0;
                playerRunningForce = Vector2.Zero;
                hit = false;
                grabbingLedge = false;
            }
            gamePadStateBackUp = gamePadState;
            previousSwordPosition = myFlow.CurrentActionPosRight;
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
            return (new PlayerObject(player, pos, speed, accel, mass, sided, friction, bounciness, layer, health, mana, stamina, facingDirection, aggressionState, rect, maxPlayerSpeed, playerAccel, myFlow, playerSprite, gridLoc));
        }

        //**************************************************
        // Method: BlownUp
        //
        // Purpose: Code which handles making the player
        // explode.
        //**************************************************
        public void BlownUp()
        {
            health = 0;
            if (!deleted)
            {
                if (GlobalVariables.Settings.gore)
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        GlobalVariables.AddList.Add(new PlayerGib(pos));
                    }
                }
            }

            Delete();
        }

        //**************************************************
        // Method: Death
        //
        // Purpose: Handles what should happen when the
        // player dies.
        //**************************************************
        public void Death()
        {
            GlobalVariables.AddList.Add(new DeathAnimation(facingDirection, pos, speed, accel, mass, friction, airResistance, bounciness, layer));
            Delete();
        }


        //****************************************************
        // Method: Delete
        //
        // Purpose: Handles deleting
        //****************************************************
        public override void Delete()
        {
            if (myBlade != null)
            {
                myBlade.Delete();
            }
            base.Delete();
        }

        //**************************************************
        // Method: Respawn
        //
        // Purpose: Resets most of the player's variables
        //**************************************************
        public void Respawn(Vector2 Pos)
        {
            health = GlobalVariables.Settings.startingHealth;
            mana = GlobalVariables.Settings.startingMana;
            stamina = GlobalVariables.Settings.startingStamina;
            pos = Pos;
            speed = new Vector2(0, 0);
            aggressionState = OffDefState.neutral;
            rect.Width = 45;//56
            rect.Height = 85;//55
            maxPlayerSpeed = new Vector2(300, 0);
            playerAccel = new Vector2(0, 0);
            playerRunningForce = new Vector2(0, 0);
            maxAirJumps = 1;
            airJumps = maxAirJumps;
            grabbing = playerGrab.Nothing;
            deleted = false;

            myFlow = new ActionFlow();
            playerSprite = new PlayerSprite(new Point(0, 0), new Vector2(35,19));
        }
        //**************************************************
        // Method: GushBlood
        //
        // Purpose: Handles the visual and audio details of
        //a player being hit by a sword successfully.
        //**************************************************
        public void GushBlood()
        {
            if (GlobalVariables.Settings.gore)
            {
                int totalBlood = GlobalVariables.Randomizer.Next(2, 5);
                for (int i = 0; i < totalBlood; ++i)
                {
                    GlobalVariables.AddList.Add(new PlayerBlood(pos));
                }
            }
            if (painTimer <= 0)
            {
                GlobalVariables.SoundBank.PlayCue("Male Grunt");
                painTimer = 200;
            }
        }



        //Properties
        public PlayerIndex Player
        {
            get
            {
                return player;
            }
        }

        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public int Mana
        {
            get
            {
                return mana;
            }
            set
            {
                mana = value;
            }
        }

        public float Stamina
        {
            get
            {
                return stamina;
            }
        }

        public Facing FacingDirection
        {
            get
            {
                return facingDirection;
            }
        }

        public OffDefState AggressionState
        {
            get
            {
                return aggressionState;
            }
        }

        public Vector2 MaxPlayerSpeed
        {
            get
            {
                return maxPlayerSpeed;
            }
        }

        public Vector2 PlayerAccel
        {

            get
            {
                return playerAccel;
            }
        }

        public PlayerSprite PlayerSprite
        {
            get
            {
                return PlayerSprite;
            }
        }

        public override Vector2 Speed
        {
            get
            {
                return speed;
            }
        }

        public int AirJumps
        {
            get
            {
                return airJumps;
            }
        }

        public playerGrab Grabbing
        {
            get
            {
                return grabbing;
            }
            set
            {
                grabbing = value;
            }
        }

        public ActionFlow MyFlow
        {
            get
            {
                return myFlow;
            }
        }
        public Boolean SuccessfulBlock
        {
            get
            {
                return successfulBlock;
            }
            set
            {
                successfulBlock = value;
            }
        }

        public Boolean Blocked
        {
            get
            {
                return blocked;
            }
            set
            {
                blocked = value;
            }
        }

        public Action CurrentAction
        {
            get
            {
                return currentAction;
            }
            set
            {
                currentAction = value;
            }
        }

        public Attacks CurrentAttack
        {
            get
            {
                return currentAttack;
            }
            set
            {
                currentAttack = value;
            }
        }

        public Attacks LockedAttack
        {
            get
            {
                return lockedAttack;
            }
            set
            {
                lockedAttack = value;
            }
        }

        public Boolean Hit
        {
            get
            {
                return hit;
            }

            set
            {
                hit = value;
            }
        }
        public int DashingTimer
        {
            get
            {
                return dashingTimer;
            }
        }
        public Boolean Parried
        {
            get
            {
                return parried;
            }
            set
            {
                parried = value;
            }
        }

        public int ParriedStunTimer
        {
            get
            {
                return parriedStunTimer;
            }
            set
            {
                parriedStunTimer = value;
            }
        }
        public int BlockedStunTimer
        {
            get
            {
                return blockedStunTimer;
            }
            set
            {
                blockedStunTimer = value;
            }
        }
        public List<Point> Last2SwordPositions
        {
            get
            {
                return last2SwordPositions;
            }
        }

        public int BlockedAnimationFrame
        {
            get
            {
                return blockedAnimationFrame;
            }
            set
            {
                blockedAnimationFrame = value;
            }
        }

        public GamePadState GamePadState
        {
            get
            {
                return gamePadState;
            }
        }
    }
}
