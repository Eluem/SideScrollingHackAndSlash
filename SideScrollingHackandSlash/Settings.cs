//******************************************************
// File: Settings.cs
//
// Purpose: Contains all the general settings.
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
    class Settings
    {
        protected float gravityAccel; //Stores acceleration due to gravity
        protected int Fps; // Stores the frames per second
        protected int StartingHealth; //Stores the starting health for all players
        protected int StartingMana;  //Stores the starting mana for all players
        protected int StartingStamina;  //Stores the starting stamina for all players
        protected int StartingLives; //Stores the starting lives for all players
        protected int StartingRespawnTimer; //Time that the respawn timer for a player gets reset to
        protected Boolean Gore; //Flag for whether or not gore is enabled

        

        //****************************************************
        // Method: Settings
        //
        // Purpose: Settings constructor
        //****************************************************
        public Settings()
        {
            //gravityAccel = 9.82F;
            gravityAccel = 600F;
            Fps = 60;
            StartingHealth = 100;
            StartingMana = 100;
            StartingStamina = 100;
            StartingLives = 5;
            StartingRespawnTimer = 1300;
            gore = true;
        }

        //****************************************************
        // Method: Default
        //
        // Purpose: Resets all the settings
        //****************************************************
        public void Default()
        {
            gravityAccel = 600F;
            Fps = 60;
            StartingHealth = 100;
            StartingMana = 100;
            StartingStamina = 100;
            StartingLives = 5;
            StartingRespawnTimer = 3000;
            gore = true;
        }


        //Properties
        public float Gravity
        {
            get
            {
                return gravityAccel;
            }
            set
            {
                gravityAccel = value;
            }
        }

        public int fps
        {
            get
            {
                return Fps;
            }
            set
            {
                Fps = value;
            }
        }
        public int startingHealth
        {
            get
            {
                return StartingHealth;
            }
            set
            {
                StartingHealth = value;
            }
        }
        public int startingMana
        {
            get
            {
                return StartingMana;
            }
            set
            {
                StartingMana = value;
            }
        }
        public int startingStamina
        {
            get
            {
                return StartingStamina;
            }
            set
            {
                StartingStamina = value;
            }
        }
        public int startingLives
        {
            get
            {
                return StartingLives;
            }
            set
            {
                StartingLives = value;
            }
        }
        public int startingRespawnTimer
        {
            get
            {
                return StartingRespawnTimer;
            }
            set
            {
                StartingRespawnTimer = value;
            }
        }
        public Boolean gore
        {
            get
            {
                return Gore;
            }
            set
            {
                Gore = value;
            }
        }
    }
}
