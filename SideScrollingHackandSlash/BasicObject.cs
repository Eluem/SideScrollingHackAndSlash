//******************************************************
// File: BasicObject.cs
//
// Purpose: Contains the class definition for BasicObject.
// BasicObject contains base functions which nearly
// every object in the game will call. It is not
// collideable, and thus cannot be used as the type
// for the main object list.
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
    abstract class BasicObject
    {
        protected Vector2 pos;       //position of the object's top left corner
        protected Boolean deleted;   //flag used to delete objects
        protected float layer;       //stores the object's layer to be drawn to
        protected Drawable image;    //stores the Drawable class for an object

        //**********************************************************************
        // Stores the id of the object. The id of an object is used to identify
        // certain things such as who damaged a player. All objects which are
        // not players, for now, will be considered to be "game objects" and
        // will be given an id of -1. Players are given ids that count up
        // from there.
        // (May not be fully implemented by the end of the class)
        //**********************************************************************
        protected int objectID;

        //**********************************************************************
        // Stores the id of the object that owns/created this object.
        // All objects which are not owned by a player will be owned by the
        // game (which has the -1 id). Objects that a player owns, such as
        // a sword slash or projectile that a player may shoot will be given
        // an ownerID equal to that player's objectID. The object will still
        // be considered a game object, however and will get an objectID of -1.
        // (May not be fully implemented by the end of the class)
        //**********************************************************************
        protected int ownerID;


        //**********************************************************************
        // specialProperties is a flag that will be used for certain objects
        // to allow certain other objects, like a PlayerObject to know that
        // they should do a series of try, catch attempts to attempt to cast
        // the object up to its true form, so that it can be interacted with
        // in an appropriate manner.
        //**********************************************************************
        protected Boolean specialProperties;

        protected String strImage;   //Stores the name of the image

        //****************************************************
        // Method: BasicObject
        //
        // Purpose: BasicObject constructor
        //****************************************************
        public BasicObject(String Image, Boolean SpecialProperties, int ObjectID, int OwnerID, Vector2 Position, float Layer)
        {
            //Prevents wasting the space of assigning an image to an object that doesn't need it while allowing
            //abstraction from the creation of an image inside of a class.
            if (Image != "")
            {
                image = new Drawable(Image, new Point((int)Position.X, (int)Position.Y));
            }

            //Basic initializations
            deleted = false;
            pos = Position;
            layer = Layer;
            strImage = Image;
            specialProperties = SpecialProperties;
            ownerID = OwnerID;
            objectID = ObjectID;
        }


        //****************************************************
        // Method: Update
        //
        // Purpose: Updates the details of the object.
        //****************************************************
        public virtual void Update(GameTime gameTime, Viewport port)
        {
        }

        //****************************************************
        // Method: Draw
        //
        // Purpose: Calls the draw functions of the sprites
        // that the object contains.
        //****************************************************
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            image.UpdatePos(pos);
            image.Draw(spriteBatch, layer);
        }

        //****************************************************
        // Method: Delete
        //
        // Purpose: Sets the delete flag to true
        //****************************************************
        public virtual void Delete()
        {
            deleted = true;
        }



        //Properties
        public virtual Vector2 Pos
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }

        public Boolean Deleted
        {
            get
            {
                return deleted;
            }
            set
            {
                deleted = false;
            }
        }
        public String Image
        {
            get
            {
                return strImage;
            }
        }

        public float Layer
        {
            get
            {
                return layer;
            }
        }

        public Boolean SpecialProperties
        {
            get
            {
                return specialProperties;
            }
        }

        public int ObjectID
        {
            get
            {
                return objectID;
            }
        }

        public int OwnerID
        {
            get
            {
                return ownerID;
            }
            set
            {
                ownerID = value;
            }
        }

        public string StrImage
        {
            get
            {
                return strImage;
            }
        }
    }
}
