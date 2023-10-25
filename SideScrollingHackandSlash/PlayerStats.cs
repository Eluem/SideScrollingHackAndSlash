//**********************************************************
// File: PlayerStats.cs
//
// Purpose: Contains the class definition of 
// PlayerStats. PlayerStats contains the methods and
// variables used to track a player's current lives
// and kills, perhaps in the future...
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
    class PlayerStats
    {

        protected int lives; //Stores the player's current lives
        protected int respawnTimer; //Stores the player's current respawn timer
        protected Boolean spawned; //Stores a flag stating whether or not the player is currently spawned

        public PlayerStats()
        {
            lives = GlobalVariables.Settings.startingLives;     //initializes the player's lives based on the settings
            respawnTimer = GlobalVariables.Settings.startingRespawnTimer; //initializes the player's respawn timer based on the settings
            spawned = true; //initializes the player's spawned status
        }

        public int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
            }
        }

        public int RespawnTimer
        {
            get
            {
                return respawnTimer;
            }
            set
            {
                respawnTimer = value;
            }
        }

        public Boolean Spawned
        {
            set
            {
                spawned = value;
            }
            get
            {
                return spawned;
            }
        }
    }
}
