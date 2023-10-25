//******************************************************
// File: PhysicsObject.cs
//
// Purpose: Contains the class definition of
// PhysicsObject. PhysicsObject will be
// inherited by any object which should be able
// to handle movement and other 'physical' activities.
// All objects which will interact will each other
// will inherit from PhysicsObject.
// It will be the type for the main list, so that
// objects that interact with each other can be placed
// in a single list.
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
    //Used for determining the side an object struck another object
    enum side { left, right, bottom, top };

    //Used for determining the axis on which a collision occured
    enum axis { y, x };
    class PhysicsObject : CollideableObject
    {
        protected float mass; //Stores object mass
        protected Vector2 accel; //Stores object acceleration
        protected Vector2 speed; //Stores object speed
        protected Vector2 prevSpeed; //Stores the objects speed from the frame before
        protected Dictionary<side, Boolean> sided; //Dictionary of flags stating whether the object is being
                                                   //touched on any of its four sides
        protected float bounciness; //Number between 0 and 1 that determines how much energy will be retained
                                    //after colliding with an object
        protected float friction;  //Number between 0 and 1 which determines how much friction this object has
                                   //Depending on the type of object and action it may affect it differently.
                                   //I.E. players tying to walk compared to trying to stop.
                                   //The greater the number, the greater the resistance.

        protected Vector2 airResistance; //Stores the air resistance of the object. The airResistance will be between
                                       //0 and 1. The higher the number the less the resistance.

        protected List<Point> gridLoc; //Stores the grid locations that the object lies in


        //****************************************************
        // Method: PhysicsObject
        //
        // Purpose: PhysicsObject constructor
        //****************************************************
        public PhysicsObject(String Image, Boolean SpecialProperties, int ObjectID, int OwnerID, Vector2 Position, Vector2 Speed, Vector2 Accel, float Mass, float Friction, Vector2 AirResistance, float Bounciness, float Layer)
            : base(Image, SpecialProperties, ObjectID, OwnerID, Position, Layer)
        {
            mass = Mass;
            accel = Accel;
            speed = Speed;
            sided = new Dictionary<side, bool>();
            sided.Add(side.left, false);
            sided.Add(side.right, false);
            sided.Add(side.top, false);
            sided.Add(side.bottom, false);
            bounciness = Bounciness;
            friction = Friction;
            airResistance = AirResistance;
            gridLoc = new List<Point>();
        }

        //****************************************************
        // Method: PhysicsObject
        //
        // Purpose: PhysicsObject constructor for deep copy
        //****************************************************
        public PhysicsObject(String Image, Boolean SpecialProperties, int ObjectID, int OwnerID, Vector2 Position, Vector2 Speed, Vector2 Accel, float Mass, float Friction, Vector2 AirResistance, float Bounciness, float Layer, FloatRectangle Rect, Dictionary<side, Boolean> Sided, List<Point> GridLoc)
            : base(Image, SpecialProperties, ObjectID, OwnerID, Position, Layer)
        {
            mass = Mass;
            accel = Accel;
            speed = Speed;
            sided = Sided;
            rect = Rect;
            bounciness = Bounciness;
            friction = Friction;
            airResistance = AirResistance;
            gridLoc = GridLoc;
        }



        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            //REQUIRES WORK

            prevPos = pos; //Updates the previous position

            //Updates verticle accel based on gravity if the object isn't on the ground
            if (!sided[side.bottom])
            {
                speed.Y += GlobalVariables.Settings.Gravity / GlobalVariables.Settings.fps;
            }
            //To prevent objects which are sliding on the ground from sliding at very small speeds
            //for a long period of time I just set the speed to 0 when it's moving too slowly.
            else
            {
                if (Math.Abs(speed.X) < 1)
                {
                    speed.X = 0;

                }
                //This is done to prevent objects from bouncing off of each other forever (not perfect solution....)
                if (Math.Abs(speed.Y) < 30)
                {
                    speed.Y = 0;
                }
            }

            speed -= speed * airResistance; //Calculates air resistance

            //Update speed based on accel
            speed.X += accel.X / GlobalVariables.Settings.fps;
            speed.Y += accel.Y / GlobalVariables.Settings.fps;

            //Update position based on speed
            pos.X += speed.X / GlobalVariables.Settings.fps;
            pos.Y += speed.Y / GlobalVariables.Settings.fps;

            //Update rectangle collision box position based on floating position (may remove rectangles as collision methods)
            rect.X = pos.X;
            rect.Y = pos.Y;

            //resets whether the object is touching anything on any sides or not
            sided[side.bottom] = false;
            sided[side.top] = false;
            sided[side.left] = false;
            sided[side.right] = false;

            //If any object falls off the bottom of the screen, delete it
            if (rect.Y > port.Height)
            {
                Delete();
            }

            //Updates the grid location
            GridLocationUpdate();

            prevSpeed = speed;
        }


        //****************************************************
        // Method: Collide
        //
        // Purpose: Handles collision between objects.
        //****************************************************
        public override void Collide(PhysicsObject obj, PhysicsObject trueObjPointer, Vector2 overLap, GameTime gameTime)
        {
            Boolean baseCollideAnyway = true;
            if (obj.specialProperties)
            {
                if (trueObjPointer is Explosion)
                {
                    baseCollideAnyway = false;
                    if (obj.Rect.Center.X < rect.Center.X)
                    {
                        speed.X += 300/mass;
                    }
                    else
                    {
                        speed.X -= 300/mass;
                    }
                    speed.Y -= 250 / mass;
                }
                else if (trueObjPointer is ConcExplosion)
                {
                    baseCollideAnyway = false;
                    if (obj.Rect.Center.X < rect.Center.X)
                    {
                        speed.X += 2000 / mass;
                    }
                    else
                    {
                        speed.X -= 2000 / mass;
                    }
                    speed.Y -= 1600 / mass;
                }
                else if (trueObjPointer is ThrowableObject)
                {
                    if (((ThrowableObject)trueObjPointer).Grabber != null)
                    {
                        baseCollideAnyway = false;
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
                else if (trueObjPointer is MeleeSlash001)
                {
                    baseCollideAnyway = false;
                    speed.X += trueObjPointer.Speed.X / mass;
                    speed.Y += trueObjPointer.Speed.Y / mass;
                }
                else if (trueObjPointer is GrabCollisionBox)
                {
                    baseCollideAnyway = false;
                }
                //handles object being hit by a force push
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

                //handles object being hit by a lightning bolt
                else if (trueObjPointer is LightningBolt)
                {
                    baseCollideAnyway = false;
                }
                else if (trueObjPointer is BombFire)
                {
                    baseCollideAnyway = false;
                }
            }
            if (baseCollideAnyway)
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
                if (obj.friction * friction == 0)
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
                if (rect.Center != obj.rect.Center) //Makes sure the objects don't overlap on their center
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
                    if ((int)prevPos.X != (int)obj.PrevPos.X && (int)prevPos.Y != (int)obj.PrevPos.Y)
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
                        sided[side.right] = true;
                    }
                    else
                    {
                        pos.X += overLap.X; //Moves the object out of the other object by the correct amount
                        sided[side.left] = true;
                    }

                    prevSpeed.X = speed.X; //Stores the speed of the object before the collision occurs

                    //Everything in here should on occur during an overlap
                    //(somethings should occur when there's no overlap but the objects are next to each other"

                    if (overLapBeforeAppliable.X > 0 && !(collisionSide == side.left && speed.X < 0 && obj.Speed.X > speed.X) && !(collisionSide == side.right && speed.X > 0 && obj.Speed.X < speed.X) && !(collisionSide == side.left && obj.Speed.X > 0 && obj.Speed.X > speed.X) && !(collisionSide == side.right && obj.Speed.X < 0 && obj.Speed.X < speed.X))
                    {
                        //Applies conservation of momentum
                        speed.X = (((((mass - obj.Mass) / (mass + obj.Mass)) * speed.X) + (((2 * obj.Mass) / (mass + obj.Mass)) * obj.Speed.X)));
                        //speed.X = ((speed.X * (mass - obj.Mass) + 2 * (obj.Mass * obj.Speed.X)) / (mass + obj.Mass));

                        //speed.X += (speed.X - prevSpeed.X) * (bounciness + obj.Bounciness); //Applies equal and opposite impulse force
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
                        //speed.Y += (speed.Y - prevSpeed.Y) * (bounciness + obj.Bounciness); //Applies equal and opposite force of impact
                        //speed.Y = ((speed.Y * (mass - obj.Mass) + 2 * (obj.Mass * obj.Speed.Y)) / (mass + obj.Mass));
                        speed.Y *= bounciness + obj.Bounciness; //Applies bounciness
                    }

                    //Applies friction to the X axis

                    //Determines the direction the object is moving in.
                    if (speed.X > 0)
                    {
                        //**********************************************************************
                        // This calculates the change in speed due to friction.
                        // totalFriction acts as the coefficent of friction for the equation
                        // which calculates frictional force. The rest of the equation I
                        // created by looking up and remembering the physics equations for
                        // impulse and force, then I divided by mass to find the accleration
                        // that I should apply.
                        //**********************************************************************
                        speed.X -= (totalFriction * (prevSpeed.Y - speed.Y + accel.Y + GlobalVariables.Settings.Gravity)) / GlobalVariables.Settings.fps;
                        if (speed.X < 0)
                        {
                            speed.X = 0; //This prevents friction from changing an object's direction of movement
                        }
                    }
                    else if (speed.X < 0)
                    {
                        //**********************************************************************
                        // This calculates the change in speed due to friction.
                        // totalFriction acts as the coefficent of friction for the equation
                        // which calculates frictional force. The rest of the equation I
                        // created by looking up and remembering the physics equations for
                        // impulse and force, then I divided by mass to find the accleration
                        // that I should apply.
                        //**********************************************************************
                        speed.X += (totalFriction * (prevSpeed.Y - speed.Y + accel.Y + GlobalVariables.Settings.Gravity)) / GlobalVariables.Settings.fps;
                        if (speed.X > 0)
                        {
                            speed.X = 0; //This prevents friction from changing an object's direction of movement
                        }
                    }
                }
            }
        }

        //**************************************************
        // Method: GridLocationUpdate
        //
        // Purpose: To update the list of grid locations
        // for the object.
        //**************************************************
        public void GridLocationUpdate()
        {
            gridLoc.Clear();
            Point startingPoint = new Point(((int)(pos.X / 50)) - 1,((int)(pos.Y / 50)) - 1);
            gridLoc.Add(startingPoint);
            Point span = new Point(((int)(rect.Width / 50)) + 2, ((int)(rect.Height / 50)) + 2);
            for (int i = 1; i <= span.X; ++i)
            {
                for (int j = 1; j <= span.Y; ++j)
                {
                    gridLoc.Add(new Point(startingPoint.X + i, startingPoint.Y + j));
                }
            }
        }

        //**************************************************
        // Method: DeepClone
        //
        // Purpose: To make a deep copy of this object so
        // that, during collisions the system can stay
        // consistent.
        //**************************************************
        public virtual PhysicsObject DeepClone()
        {
            return (new PhysicsObject(strImage, specialProperties, objectID, ownerID, pos, speed, accel, mass, friction, airResistance, bounciness, layer, rect, sided, gridLoc));
        }

        //****************************************************
        // Method: Delete
        //
        // Purpose: Sets the delete flag to true
        //****************************************************
        public override void Delete()
        {
            deleted = true;
            GlobalVariables.DeleteList.Add(this);
        }

        //Properties
        public float Mass
        {
            get
            {
                return mass;
            }
        }

        public virtual Vector2 Accel
        {
            get
            {
                return accel;
            }
        }

        public virtual Vector2 Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }


        public Dictionary<side, Boolean> Sided
        {
            get
            {
                return sided;
            }
        }
        public float Friction
        {
            get
            {
                return friction;
            }
        }
        public float Bounciness
        {
            get
            {
                return bounciness;
            }
        }

        public List<Point> GridLoc
        {
            get
            {
                return gridLoc;
            }
        }
    }
}
