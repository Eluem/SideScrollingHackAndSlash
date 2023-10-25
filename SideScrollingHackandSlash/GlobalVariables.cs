//******************************************************
// File: GlobalVariables.cs
//
// Purpose: Contains the class definition for
// GlobalVariables. GlobalVariables is used to store
// any variables which all classes may need access to.
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
    static class GlobalVariables
    {
        //I determined that this would be a good way to solve certain problems
        //such as having two objects with the exact same rectangle size, speed, and acceleration
        //collide and land on each other with their centers.
        static Random randomizer = new Random(DateTime.Now.Millisecond);


        static int playerObjectIDCounter = 0;//Used for counting up the player's objectIDs as they're added to
                                             //the game



        //Audio initializations
        static AudioEngine audioEngine = new AudioEngine(@"Content\SideScrollerSounds.xgs");
        static WaveBank waveBank = new WaveBank(audioEngine, @"Content\Wave Bank001.xwb");
        static SoundBank soundBank = new SoundBank(audioEngine, @"Content\Sound Bank001.xsb");


        //Global list used to delete objects
        static List<PhysicsObject> deleteList = new List<PhysicsObject>();

        //Global list used to add objects
        static List<PhysicsObject> addList = new List<PhysicsObject>();

        //Global dictionary used to access pointers to textures
        static Dictionary<string, Texture2D> imageDict = new Dictionary<string, Texture2D>();


        //Global settings class (which isn't used as much as I wanted to...)
        static Settings settings = new Settings();


        //Properties
        public static List<PhysicsObject> DeleteList
        {
            get
            {
                return deleteList;
            }
        }

        public static Settings Settings
        {
            get
            {
                return settings;
            }
        }

        public static List<PhysicsObject> AddList
        {
            get
            {
                return addList;
            }
        }

        public static Dictionary<string, Texture2D> ImageDict
        {
            get
            {
                return imageDict;
            }
        }

        public static Random Randomizer
        {
            get
            {
                return randomizer;
            }
        }

        public static int PlayerObjectIDCounter
        {
            get
            {
                return playerObjectIDCounter;
            }
        }

        public static WaveBank WaveBank
        {
            get
            {
                return waveBank;
            }
        }

        public static SoundBank SoundBank
        {
            get
            {
                return soundBank;
            }
        }

        public static AudioEngine AudioEngine
        {
            get
            {
                return audioEngine;
            }
        }
    }
}
