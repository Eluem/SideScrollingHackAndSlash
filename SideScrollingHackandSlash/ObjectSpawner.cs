//******************************************************
// File: ObjectSpawner.cs
//
// Purpose: Contains the class definition of
// ObjectSpawner. ObjectSpawner is designed to randomly
// spawn objects at its position as long as it
// doesn't have as many objects spawned as it is
// allowed at maximum.
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
    class ObjectSpawner : PhysicsObject
    {
        protected int maximumNumberOfObjects; //Stores the maximum number of objects this object can have
                                              //spawned at any time.
        protected List<PhysicsObject> spawnedObjects; //Stores a list of all the objects that it has spawned
                                                      //so that they can be checked if they've been deleted yet

        int minSpawnTime; //Stores the minimum object spawn time
        int maxSpawnTime; //Stores the maximum object spawn time
        int spawnTimer; //Stores the current spawn timer

        Point minSpeed; //Stores the minimum speed an object can be spawned with
        Point maxSpeed; //Stores teh maximum speed an object can be spawned wtih


        enum objectType { health, mana, bomb, concNade};

        public ObjectSpawner(Vector2 Position, Point MinSpeed, Point MaxSpeed, int MinTime, int MaxTime, int MaxObjects)
            : base("", true, -1, -1, Position, Vector2.Zero, Vector2.Zero, 0F, 0F, Vector2.Zero, 0F, 0F)
        {
            minSpawnTime = MinTime;
            maxSpawnTime = MaxTime;
            spawnTimer = GlobalVariables.Randomizer.Next(minSpawnTime, maxSpawnTime);

            maximumNumberOfObjects = MaxObjects;

            minSpeed = MinSpeed;
            maxSpeed = MaxSpeed;

            spawnedObjects = new List<PhysicsObject>();
        }




        //***************************************************
        // Method: Draw
        //
        // Purpose: To override draw because this object
        // shouldn't be drawn.
        //***************************************************
        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
        }


        //***************************************************
        // Method: Update
        //
        // Purpose: Handles updating the details of the
        // object.
        //***************************************************
        public override void Update(GameTime gameTime, Viewport port)
        {
            List<PhysicsObject> DeleteList = new List<PhysicsObject>();

            foreach (PhysicsObject checkMe in spawnedObjects)
            {
                if (checkMe.Deleted)
                {
                    DeleteList.Add(checkMe);
                }
            }

            foreach (PhysicsObject deleteMe in DeleteList)
            {
                spawnedObjects.Remove(deleteMe);
            }

            if (spawnedObjects.Count < maximumNumberOfObjects)
            {
                if (spawnTimer <= 0)
                {
                    spawnTimer = GlobalVariables.Randomizer.Next(minSpawnTime, maxSpawnTime);
                    int spawnNum = GlobalVariables.Randomizer.Next(0, 101);
                    objectType spawnMe = objectType.bomb;
                    if (spawnNum < 30)
                    {
                        spawnMe = (objectType)GlobalVariables.Randomizer.Next(0, 2);
                    }
                    else if (spawnNum < 60)
                    {
                        spawnMe = objectType.bomb;
                    }
                    else
                    {
                        spawnMe = objectType.concNade;
                    }
                    switch (spawnMe)
                    {
                        case objectType.bomb:
                            Bomb tempBomb = new Bomb(pos, new Vector2((float)GlobalVariables.Randomizer.Next(minSpeed.X, maxSpeed.X), (float)GlobalVariables.Randomizer.Next(minSpeed.Y, maxSpeed.Y)));
                            spawnedObjects.Add(tempBomb);
                            GlobalVariables.AddList.Add(tempBomb);
                            break;
                        case objectType.concNade:
                            ConcNade tempConcNade = new ConcNade(pos, new Vector2((float)GlobalVariables.Randomizer.Next(minSpeed.X, maxSpeed.X), (float)GlobalVariables.Randomizer.Next(minSpeed.Y, maxSpeed.Y)));
                            spawnedObjects.Add(tempConcNade);
                            GlobalVariables.AddList.Add(tempConcNade);
                            break;
                        case objectType.health:
                            HealthPotion tempHealthPotion = new HealthPotion(pos, new Vector2((float)GlobalVariables.Randomizer.Next(minSpeed.X, maxSpeed.X), (float)GlobalVariables.Randomizer.Next(minSpeed.Y, maxSpeed.Y)));
                            spawnedObjects.Add(tempHealthPotion);
                            GlobalVariables.AddList.Add(tempHealthPotion);
                            break;
                        case objectType.mana:
                            ManaPotion tempManaPotion = new ManaPotion(pos, new Vector2((float)GlobalVariables.Randomizer.Next(minSpeed.X, maxSpeed.X), (float)GlobalVariables.Randomizer.Next(minSpeed.Y, maxSpeed.Y)));
                            spawnedObjects.Add(tempManaPotion);
                            GlobalVariables.AddList.Add(tempManaPotion);
                            break;
                    }
                }
                else
                {
                    spawnTimer -= gameTime.ElapsedGameTime.Milliseconds;
                }
            }
        }
    }
}
