//******************************************************
// File: Game1.cs
//
// Purpose: Contains all the main logical structures
// for the game.
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch backGroundBatch; //Used to draw teh background

        enum gameState { Start, Menu, InGameMenu, Settings, Playing, GameOver, Initialize, InitializeMenu, Credits, InitializeInGameMenu, InitializeSettings };
        gameState currentGameState; //Stores the current state of the game

        //Menu variables
        enum inGameMenuItem { Continue, Quit };
        enum menuItem { Play, Settings, Credits, Quit };
        enum settingsMenuItem { Gore, StartingLives, Default, Done };

        Color[] colorArray;
        menuItem selectedItem;

        inGameMenuItem selectedInGameMenuItem;
        Color[] inGameColorArray;

        settingsMenuItem selectedSettingsItem;
        Color[] settingsColorArray;


        List<PhysicsObject> objectList; //List of objects in the game
        List<PhysicsObject> deleteList; //List of objects to be deleted
        List<PhysicsObject> tempObjectList; //Stores a temporary list of objects used for colliding

        PlayerObject playerOne; //Stores a pointer to playerOne
        PlayerObject playerTwo; //Stores a pointer to playerTwo
        Rectangle HUDBarSourceRect; //Stores the source rectangle for the HUD stat bars because it's required
                                    //to reach an overload of draw which allows for layering

        PlayerStats playerOneStats; //Stores the stats of player one
        PlayerStats playerTwoStats; //Stores the stats of player two

        GamePadState PreviousGamepadState;
        int gameOverTimer; //Timer which starts counting down when a player dies
        Boolean gameOver; //A flag saying that a win condition has occurred

        string winMessage; //Stores the win message that will be displayed on the game over screen

        ThumbstickTrackingTest testThumbStick;

        int splashScreenTimer;

        SpriteFont Arial_20PT;

        System.Diagnostics.Stopwatch myWatch;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Initializations
            tempObjectList = new List<PhysicsObject>();
            objectList = new List<PhysicsObject>();
            deleteList = new List<PhysicsObject>();
            currentGameState = gameState.Start;
            myWatch = new System.Diagnostics.Stopwatch();
            splashScreenTimer = 10000;
            PreviousGamepadState = GamePad.GetState(PlayerIndex.One);
            colorArray = new Color[4];
            inGameColorArray = new Color[2];
            settingsColorArray = new Color[4];
            HUDBarSourceRect = new Rectangle(0, 0, 200, 20);


            //Full Screen Initialization
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Create a background batch
            backGroundBatch = new SpriteBatch(GraphicsDevice);

            //Add skullball sprite sheet
            GlobalVariables.ImageDict.Add("skullball", Content.Load<Texture2D>("skullball"));

            //Add threerings sprite sheet
            GlobalVariables.ImageDict.Add("threerings", Content.Load<Texture2D>("threerings"));

            //Add platform_flat_long image
            GlobalVariables.ImageDict.Add("platform_flat_long", Content.Load<Texture2D>("platform_flat_long"));

            //Add platform_flat_long_red image
            GlobalVariables.ImageDict.Add("platform_flat_long_red", Content.Load<Texture2D>("platform_flat_long_red"));
            
            //Add platform_flat_long_blue image
            GlobalVariables.ImageDict.Add("platform_flat_long_blue", Content.Load<Texture2D>("platform_flat_long_blue"));

            //Add sal sword ani edit sprite sheet
            GlobalVariables.ImageDict.Add("sal sword ani edit", Content.Load<Texture2D>("sal sword ani edit"));

            //Add sal run sprite sheet
            GlobalVariables.ImageDict.Add("sal run", Content.Load<Texture2D>("sal run"));

            //Add small_platform
            GlobalVariables.ImageDict.Add("small_platform", Content.Load<Texture2D>("small_platform"));

            //Add CopperCoin sprite
            GlobalVariables.ImageDict.Add("CopperCoin", Content.Load<Texture2D>("CopperCoin"));

            //Add block_green image
            GlobalVariables.ImageDict.Add("block_green", Content.Load<Texture2D>("block_green"));

            //Add block_red image
            GlobalVariables.ImageDict.Add("block_red", Content.Load<Texture2D>("block_red"));

            //Add block_white image
            GlobalVariables.ImageDict.Add("block_white", Content.Load<Texture2D>("block_white"));

            //Add wall image
            GlobalVariables.ImageDict.Add("wall", Content.Load<Texture2D>("wall"));

            //Add bomb and bombfire
            GlobalVariables.ImageDict.Add("BombFire", Content.Load<Texture2D>("BombFire"));
            GlobalVariables.ImageDict.Add("Bomb", Content.Load<Texture2D>("Bomb"));
            GlobalVariables.ImageDict.Add("NukeExplosion", Content.Load<Texture2D>("NukeExplosion"));

            //Add lightning bolt
            GlobalVariables.ImageDict.Add("LightningBolt", Content.Load<Texture2D>("LightningBolt"));

            //Add force push
            GlobalVariables.ImageDict.Add("ForcePush", Content.Load<Texture2D>("ForcePush"));

            //Add splash screen picutre
            GlobalVariables.ImageDict.Add("MyEye", Content.Load<Texture2D>("MyEye"));

            //Add DABOMB
            GlobalVariables.ImageDict.Add("DABOMB", Content.Load<Texture2D>("DABOMB"));

            //Add Grenade
            GlobalVariables.ImageDict.Add("Grenade", Content.Load<Texture2D>("Grenade"));

            //Add BackGround
            GlobalVariables.ImageDict.Add("BackGround", Content.Load<Texture2D>("BackGround"));

            //Add potions
            GlobalVariables.ImageDict.Add("healthPotion", Content.Load<Texture2D>("healthPotion"));
            GlobalVariables.ImageDict.Add("manaPotion", Content.Load<Texture2D>("manaPotion"));

            //Add player sprites
            GlobalVariables.ImageDict.Add("PlayerGib", Content.Load<Texture2D>("PlayerGib"));
            GlobalVariables.ImageDict.Add("foreward slash left arm", Content.Load<Texture2D>(@"foreward slash left arm"));
            GlobalVariables.ImageDict.Add("foreward slash right arm", Content.Load<Texture2D>(@"foreward slash right arm"));
            GlobalVariables.ImageDict.Add("foreward slash stance", Content.Load<Texture2D>(@"foreward slash stance"));
            GlobalVariables.ImageDict.Add("torso", Content.Load<Texture2D>(@"torso"));
            GlobalVariables.ImageDict.Add("walking left arm", Content.Load<Texture2D>(@"walking left arm"));
            GlobalVariables.ImageDict.Add("walking right arm", Content.Load<Texture2D>(@"walking right arm"));
            GlobalVariables.ImageDict.Add("walkinglegsfinal", Content.Load<Texture2D>(@"walkinglegsfinal"));
            GlobalVariables.ImageDict.Add("swordArms", Content.Load<Texture2D>(@"swordArms"));
            GlobalVariables.ImageDict.Add("death", Content.Load<Texture2D>(@"death"));

            //HUD stuff
            GlobalVariables.ImageDict.Add("Health", Content.Load<Texture2D>("Health"));
            GlobalVariables.ImageDict.Add("Stamina", Content.Load<Texture2D>("Stamina"));
            GlobalVariables.ImageDict.Add("Mana", Content.Load<Texture2D>("Mana"));



            //Loads the game fonts in
            Arial_20PT = Content.Load<SpriteFont>(@"Arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (currentGameState == gameState.Playing && GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && PreviousGamepadState.Buttons.Start == ButtonState.Released)
            {
                currentGameState = gameState.InitializeInGameMenu;
            }


            //myWatch.Reset();
            //myWatch.Start();
            switch (currentGameState)
            {
                case gameState.Start:
                    //Display splash screen for 2 seconds and allow user input to force its end
                    splashScreenTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if (splashScreenTimer <= 0 || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                    {
                        currentGameState = gameState.InitializeMenu;
                        //GlobalVariables.SoundBank.PlayCue("moon8 - All");
                    }
                    break;
                case gameState.InitializeMenu:
                    selectedItem = menuItem.Play;
                    currentGameState = gameState.Menu;
                    break;
                case gameState.Menu:
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.2 && PreviousGamepadState.ThumbSticks.Left.Y > -.2)
                    {
                        if(selectedItem != menuItem.Quit)
                            ++selectedItem;
                    }
                    else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .2 && PreviousGamepadState.ThumbSticks.Left.Y <.2)
                    {
                        if(selectedItem != menuItem.Play)
                            --selectedItem;
                    }
                    for(int color = 0; color < 4; ++color)
                    {
                        colorArray[color] = Color.Black;
                    }
                    colorArray[(int)selectedItem] = Color.White;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && PreviousGamepadState.Buttons.A == ButtonState.Released)
                    {
                        switch(selectedItem)
                        {
                            case menuItem.Play:
                                currentGameState = gameState.Initialize;
                                break;
                            case menuItem.Settings:
                                currentGameState = gameState.InitializeSettings;
                                break;
                            case menuItem.Credits:
                                currentGameState = gameState.Credits;
                                break;
                            case menuItem.Quit:
                                this.Exit();
                                break;
                        }
                    }

                    break;
                case gameState.Credits:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && PreviousGamepadState.Buttons.A == ButtonState.Released)
                    {
                        currentGameState = gameState.InitializeMenu;
                    }
                    break;

                case gameState.InitializeInGameMenu:
                    selectedInGameMenuItem = inGameMenuItem.Continue;
                    currentGameState = gameState.InGameMenu;
                    break;
                case gameState.InGameMenu:
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.2 && PreviousGamepadState.ThumbSticks.Left.Y > -.2)
                    {
                        if (selectedInGameMenuItem != inGameMenuItem.Quit)
                            ++selectedInGameMenuItem;
                    }
                    else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .2 && PreviousGamepadState.ThumbSticks.Left.Y < .2)
                    {
                        if (selectedInGameMenuItem != inGameMenuItem.Continue)
                            --selectedInGameMenuItem;
                    }
                    for (int color = 0; color < 2; ++color)
                    {
                        inGameColorArray[color] = Color.Black;
                    }
                    inGameColorArray[(int)selectedInGameMenuItem] = Color.White;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && PreviousGamepadState.Buttons.A == ButtonState.Released)
                    {
                        switch (selectedInGameMenuItem)
                        {
                            case inGameMenuItem.Continue:
                                currentGameState = gameState.Playing;
                                break;
                            case inGameMenuItem.Quit:
                                currentGameState = gameState.InitializeMenu;
                                break;
                        }
                    }
                    break;
                case gameState.InitializeSettings:
                    selectedSettingsItem = settingsMenuItem.Gore;
                    currentGameState = gameState.Settings;
                    break;
                case gameState.Settings:
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.2 && PreviousGamepadState.ThumbSticks.Left.Y > -.2)
                    {
                        if (selectedSettingsItem != settingsMenuItem.Done)
                            ++selectedSettingsItem;
                    }
                    else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .2 && PreviousGamepadState.ThumbSticks.Left.Y < .2)
                    {
                        if (selectedSettingsItem != settingsMenuItem.Gore)
                            --selectedSettingsItem;
                    }

                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -.2 && PreviousGamepadState.ThumbSticks.Left.X > -.2)
                    {
                        switch (selectedSettingsItem)
                        {
                            case settingsMenuItem.StartingLives:
                                --GlobalVariables.Settings.startingLives;
                                break; 
                        }
                    }
                    else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > .2 && PreviousGamepadState.ThumbSticks.Left.X < .2)
                    {
                        switch (selectedSettingsItem)
                        {
                            case settingsMenuItem.StartingLives:
                                ++GlobalVariables.Settings.startingLives;
                                break;
                        }
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Triggers.Right > .9 && PreviousGamepadState.Triggers.Right < .9)
                    {
                        switch (selectedSettingsItem)
                        {
                            case settingsMenuItem.StartingLives:
                                GlobalVariables.Settings.startingLives += 50;
                                break;
                        }
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Triggers.Left > .9 && PreviousGamepadState.Triggers.Left < .9)
                    {
                        switch (selectedSettingsItem)
                        {
                            case settingsMenuItem.StartingLives:
                                GlobalVariables.Settings.startingLives -= 50;
                                break;
                        }
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed && PreviousGamepadState.Buttons.RightShoulder == ButtonState.Released)
                    {
                        switch (selectedSettingsItem)
                        {
                            case settingsMenuItem.StartingLives:
                                GlobalVariables.Settings.startingLives += 10;
                                break;
                        }
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed && PreviousGamepadState.Buttons.LeftShoulder == ButtonState.Released)
                    {
                        switch (selectedSettingsItem)
                        {
                            case settingsMenuItem.StartingLives:
                                GlobalVariables.Settings.startingLives -= 10;
                                break;
                        }
                    }
                    if (GlobalVariables.Settings.startingLives < 1)
                    {
                        GlobalVariables.Settings.startingLives = 1;
                    }
                    for (int color = 0; color < 4; ++color)
                    {
                        settingsColorArray[color] = Color.Black;
                    }
                    settingsColorArray[(int)selectedSettingsItem] = Color.White;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && PreviousGamepadState.Buttons.A == ButtonState.Released)
                    {
                        switch (selectedSettingsItem)
                        {
                            case settingsMenuItem.Gore:
                                if (GlobalVariables.Settings.gore)
                                {
                                    GlobalVariables.Settings.gore = false;
                                }
                                else
                                {
                                    GlobalVariables.Settings.gore = true;
                                }
                                break;
                            case settingsMenuItem.Done:
                                currentGameState = gameState.InitializeMenu;
                                break;
                            case settingsMenuItem.Default:
                                GlobalVariables.Settings.Default();
                                break;
                        }
                    }
                    break;
                case gameState.Initialize:
                    //Clears the object list
                    objectList.Clear();

                    //Sets the game over timer to 6 seconds and sets the game over flag to false
                    gameOverTimer = 6000;
                    gameOver = false;

                    //Refreshes the player's stats
                    playerOneStats = new PlayerStats();
                    playerTwoStats = new PlayerStats();

                    //Refreshes the player objects
                    playerOne = new PlayerObject(PlayerIndex.One);
                    playerTwo = new PlayerObject(PlayerIndex.Two);

                    objectList.Add(playerOne);
                    objectList.Add(playerTwo);

                    //Initializes object list with objects
                    //Map001 (1280x1024)
                    objectList.Add(new ScaleablePlatform(new Vector2(150, 974), new Vector2(300, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(830, 974), new Vector2(300, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(0, 860), new Vector2(150, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(187, 746), new Vector2(150, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(0, 632), new Vector2(150, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(755, 860), new Vector2(150, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(942, 746), new Vector2(150, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(1130, 632), new Vector2(150, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(225, 518), new Vector2(350, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(780, 518), new Vector2(400, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(600, 404), new Vector2(150, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(0, 290), new Vector2(605, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(750, 290), new Vector2(530, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(0, 176), new Vector2(300, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(980, 176), new Vector2(300, 20)));
                    objectList.Add(new ScaleablePlatform(new Vector2(530, 176), new Vector2(200, 20)));
                    objectList.Add(new ObjectSpawner(new Vector2(550, 20), new Point(-100, 0), new Point(100, 0), 13000, 23000, 2));
                    objectList.Add(new ObjectSpawner(new Vector2(420, 380), new Point(-100, 0), new Point(100, 0), 13500, 22000, 2));
                    objectList.Add(new ObjectSpawner(new Vector2(580, 690), new Point(-110, -360), new Point(-107, -190), 11000, 25000, 2));
                    objectList.Add(new ObjectSpawner(new Vector2(600, 690), new Point(125, -220), new Point(130, -195), 10000, 21000, 1));

                    //TESTING
                    testThumbStick = new ThumbstickTrackingTest(new Vector2(640, 40), new Vector2(2, 2));
                    //objectList.Add(testThumbStick);
                    

                    //Test room (800x600)
                    /*
                    objectList.Add(new Platform001(new Vector2(0,580)));
                    //objectList.Add(new SpinningCoin(new Vector2(200, 500)));
                    objectList.Add(new TestBox001(new Vector2(250, 400), "block_red", 10));
                    objectList.Add(new TestBox001(new Vector2(250, 400), "block_red", 10));
                    objectList.Add(new Bomb(new Vector2(300, 300), Vector2.Zero));
                    objectList.Add(new ObjectSpawner(new Vector2(300, 300), new Point(-400, -500), new Point(-400, 0), 30000, 60000, 10));
                    objectList.Add(new Wall001(new Vector2(0, 300)));
                    objectList.Add(new Wall001(new Vector2(0, 0)));
                    objectList.Add(new Wall001(new Vector2(782, 300)));
                    objectList.Add(new Wall001(new Vector2(782, 0)));
                    */


                    currentGameState = gameState.Playing;
                    break;
                case gameState.Playing:
                    //Loop to update game objects
                    foreach (PhysicsObject obj in objectList)
                    {
                        obj.Update(gameTime, graphics.GraphicsDevice.Viewport);
                    }


                    //Adds objects to the list based on what the object wanted
                    foreach (PhysicsObject addMe in GlobalVariables.AddList)
                    {
                        objectList.Add(addMe);
                    }
                    GlobalVariables.AddList.Clear(); //Clears the global add list

                    //Deletes all objects in delete lists
                    foreach (PhysicsObject obj in GlobalVariables.DeleteList)
                    {
                        objectList.Remove(obj);
                    }
                    GlobalVariables.DeleteList.Clear(); //clears the delete list

                    myWatch.Reset();
                    myWatch.Start();

                    //Collision loop initialization

                    //******************************************************
                    // tempObjectArray is used in the inner collision
                    // detection loop. This is so that there will be
                    // consistency between the values that are being
                    // checked as objects update themselves when they
                    // detect that they've collided.
                    //******************************************************
                    PhysicsObject[] tempObjectArray = new PhysicsObject[objectList.Count];
                    objectList.CopyTo(tempObjectArray);

                    //List<CollideCheckReturn> collidePairList = new List<CollideCheckReturn>();

                    int objListCount = objectList.Count;
                    int j;
                    int i = 0;
                    Boolean inGrid;
                    //Outer collision detection loop
                    foreach(PhysicsObject firstObj in objectList)
                    {
                        List<Point> tempLoc = firstObj.GridLoc; //To save time
                        //Inner collision detection loop
                        for (j = i + 1; j < objListCount; ++j)
                        {
                            inGrid = false; //Grids are used for precolliding
                            List<Point> tempLoc2 = tempObjectArray[j].GridLoc; //Also to save time
                            //Loop used to check if any of the "grid locations" match up for either object
                            foreach (Point checkMe in tempLoc)
                            {
                                foreach(Point checkMe2 in tempLoc2)
                                {
                                    if (checkMe2 == checkMe)
                                    {
                                        inGrid = true;
                                        break;
                                    }
                                }
                                if (inGrid)
                                {
                                    break;
                                }
                            }
                            //If a match is found, a more accurate check occurs
                            if (inGrid)
                            {
                                CollideCheckReturn tempCollideCheckReturn = firstObj.CollideCheck(tempObjectArray[j].DeepClone(), tempObjectArray[j]);
                                //If the more accurate check passes, the objects are collided
                                if (tempCollideCheckReturn.Collided)
                                {
                                    PhysicsObject temp = firstObj.DeepClone();
                                    firstObj.Collide(tempObjectArray[j], tempObjectArray[j], tempCollideCheckReturn.Overlap, gameTime);
                                    tempObjectArray[j].Collide(temp, firstObj, tempCollideCheckReturn.Overlap, gameTime);
                                }
                            }
                        }
                        ++i;
                    }
                    /*
                    //Collide application loop
                    foreach (CollideCheckReturn colliders in collidePairList)
                    {
                        colliders.PrimaryObjectPointer.Collide(colliders.SecondaryObjectClone, colliders.SecondaryObjectPointer, colliders.Overlap, gameTime);
                        colliders.SecondaryObjectPointer.Collide(colliders.PrimaryObjectClone, colliders.PrimaryObjectPointer, colliders.Overlap, gameTime);
                    }
                    */
                    myWatch.Stop();


                    //Handles setting flags when players are deleted
                    if (playerOne.Deleted)
                    {
                        --playerOneStats.Lives;
                        playerOneStats.Spawned = false;
                        playerOneStats.RespawnTimer = GlobalVariables.Settings.startingRespawnTimer;

                        playerOne = new PlayerObject(PlayerIndex.One);
                        if (playerOneStats.Lives <= 0)
                        {
                            gameOver = true;
                        }
                    }
                    if (playerTwo.Deleted)
                    {
                        --playerTwoStats.Lives;
                        playerTwoStats.Spawned = false;
                        playerTwoStats.RespawnTimer = GlobalVariables.Settings.startingRespawnTimer;

                        playerTwo = new PlayerObject(PlayerIndex.Two);
                        if (playerTwoStats.Lives <= 0)
                        {
                            gameOver = true;
                        }
                    }

                    //Handles respawning a player if they're supposed to be respawned
                    if (!playerOneStats.Spawned && !gameOver)
                    {
                        if (playerOneStats.RespawnTimer <= 0)
                        {
                            GlobalVariables.AddList.Add(playerOne);
                            playerOneStats.Spawned = true;
                        }
                        else
                        {
                            playerOneStats.RespawnTimer -= gameTime.ElapsedGameTime.Milliseconds;
                        }
                    }
                    if (!playerTwoStats.Spawned && !gameOver)
                    {
                        if (playerTwoStats.RespawnTimer <= 0)
                        {
                            GlobalVariables.AddList.Add(playerTwo);
                            playerTwoStats.Spawned = true;
                        }
                        else
                        {
                            playerTwoStats.RespawnTimer -= gameTime.ElapsedGameTime.Milliseconds;
                        }
                    }


                    //Allows the game to continue for a few seconds before ending the game
                    if (gameOver)
                    {
                        gameOverTimer -= gameTime.ElapsedGameTime.Milliseconds;
                        if (gameOverTimer <= 0)
                        {
                            currentGameState = gameState.GameOver;
                        }

                        if (playerOneStats.Lives > 0 && !(playerTwoStats.Lives > 0))
                        {
                            winMessage = "Player One won the game!";
                        }
                        else if (playerTwoStats.Lives > 0 && !(playerOneStats.Lives > 0))
                        {
                            winMessage = "Player Two won the game!";
                        }
                        else
                        {
                            winMessage = "DRAW!";
                        }
                    }
                    break;
                case gameState.GameOver:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && PreviousGamepadState.Buttons.Start != ButtonState.Pressed)
                    {
                        currentGameState = gameState.InitializeMenu;
                    }
                    break;
            }
            //myWatch.Stop();

            PreviousGamepadState = GamePad.GetState(PlayerIndex.One);
            GlobalVariables.AudioEngine.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
            //Switch statement that determines what will be displayed to the screen based on current game state
            switch (currentGameState)
            {
                case gameState.Start:
                    spriteBatch.Draw(GlobalVariables.ImageDict["MyEye"], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    break;
                case gameState.Menu:
                    spriteBatch.DrawString(Arial_20PT, "Main Menu", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Main Menu").X)/2, 30), Color.Black);
                    spriteBatch.DrawString(Arial_20PT, "Play", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Play").X) / 2, 60), colorArray[(int)menuItem.Play]);
                    spriteBatch.DrawString(Arial_20PT, "Settings", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Settings").X) / 2, 90), colorArray[(int)menuItem.Settings]);
                    spriteBatch.DrawString(Arial_20PT, "Credits", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Credits").X) / 2, 120), colorArray[(int)menuItem.Credits]);
                    spriteBatch.DrawString(Arial_20PT, "Quit", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Quit").X) / 2, 150), colorArray[(int)menuItem.Quit]);
                    break;
                case gameState.Credits:
                    spriteBatch.DrawString(Arial_20PT, "Code and Game Concepts", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Code and Game Concepts").X) / 2, 30), Color.Black);
                    spriteBatch.DrawString(Arial_20PT, "Salvatore Hanusiewcz", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Salvatore Hanusiewicz").X) / 2, 60), Color.White);
                    spriteBatch.DrawString(Arial_20PT, "In Game Art", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("In Game Art").X) / 2, 120), Color.Black);
                    spriteBatch.DrawString(Arial_20PT, "Jessica Polio", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Jessica Polio").X) / 2, 150), Color.White);
                    spriteBatch.DrawString(Arial_20PT, "Splash Screen", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Splash Screen").X) / 2, 210), Color.Black);
                    spriteBatch.DrawString(Arial_20PT, "Christine Zimmerman", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Christine Zimmerman").X) / 2, 240), Color.White);
                    spriteBatch.DrawString(Arial_20PT, "Map Designer", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Map Designer").X) / 2, 300), Color.Black);
                    spriteBatch.DrawString(Arial_20PT, "Joseph Hanusiewicz", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Joseph Hanusiewicz").X) / 2, 330), Color.White);
                    break;
                case gameState.InGameMenu:
                    spriteBatch.DrawString(Arial_20PT, "Menu", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Menu").X) / 2, 30), Color.Black);
                    spriteBatch.DrawString(Arial_20PT, "Continue", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Continue").X) / 2, 60), inGameColorArray[(int)inGameMenuItem.Continue]);
                    spriteBatch.DrawString(Arial_20PT, "Quit", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Quit").X) / 2, 90), inGameColorArray[(int)inGameMenuItem.Quit]);
                    break;
                case gameState.Settings:
                    spriteBatch.DrawString(Arial_20PT, "Settings", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Settings").X) / 2, 30), Color.Black);
                    spriteBatch.DrawString(Arial_20PT, "Gore            " + GlobalVariables.Settings.gore.ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Gore").X) / 2, 60), settingsColorArray[(int)settingsMenuItem.Gore]);
                    spriteBatch.DrawString(Arial_20PT, "Starting Lives      " + GlobalVariables.Settings.startingLives.ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Starting Lives").X) / 2, 90), settingsColorArray[(int)settingsMenuItem.StartingLives]);
                    spriteBatch.DrawString(Arial_20PT, "Default", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Default").X) / 2, 120), settingsColorArray[(int)settingsMenuItem.Default]);
                    spriteBatch.DrawString(Arial_20PT, "Done", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Done").X) / 2, 150), settingsColorArray[(int)settingsMenuItem.Done]);
                    break;
                case gameState.Initialize:
                    break;
                case gameState.Playing:
                    backGroundBatch.Begin();
                    backGroundBatch.Draw(GlobalVariables.ImageDict["BackGround"], new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Rectangle(0, 0, GlobalVariables.ImageDict["BackGround"].Width, GlobalVariables.ImageDict["BackGround"].Height), Color.White, 0F, Vector2.Zero, SpriteEffects.None, 0F);
                    backGroundBatch.End();

                    //Loops through all the game objects and calls their draw function
                    //spriteBatch is passed so that they may draw the images themselves
                    //and access all the pictures which are loaded.
                    foreach (PhysicsObject obj in objectList)
                    {
                        obj.Draw(spriteBatch);
                    }

                    //CLUDGE TEST!!!! SHOULDN'T UPDATE HERE
                    testThumbStick.Update(gameTime, graphics.GraphicsDevice.Viewport);
                    testThumbStick.Draw(spriteBatch);
                    /*
                    Vector2 tempVector = new Vector2(MathHelper.Distance(testThumbStick.CurrentThumbStick.X, testThumbStick.FollowThumbStick.X), MathHelper.Distance(testThumbStick.CurrentThumbStick.Y, testThumbStick.FollowThumbStick.Y));

                    Boolean testBool = MathHelper.Distance(testThumbStick.CurrentThumbStick.X, testThumbStick.FollowThumbStick.X) < .3 && MathHelper.Distance(testThumbStick.CurrentThumbStick.Y, testThumbStick.FollowThumbStick.Y) < .3;
                    */

                    //Player One HUD
                    spriteBatch.Draw(GlobalVariables.ImageDict["Health"], new Rectangle(0, 0, 200 * playerOne.Health / 100, 5), HUDBarSourceRect, Color.Red, 0F, Vector2.Zero, SpriteEffects.None, 1F);
                    spriteBatch.Draw(GlobalVariables.ImageDict["Mana"], new Rectangle(0, 6, 200 * playerOne.Mana / 100, 5), HUDBarSourceRect, Color.Blue, 0F, Vector2.Zero, SpriteEffects.None, 1F);
                    spriteBatch.Draw(GlobalVariables.ImageDict["Stamina"], new Rectangle(0, 12, 200 * (int)playerOne.Stamina / 100, 5), HUDBarSourceRect, Color.Yellow, 0F, Vector2.Zero, SpriteEffects.None, 1F);
                    spriteBatch.DrawString(Arial_20PT, "Lives: " + playerOneStats.Lives.ToString(), new Vector2(210, 0), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    //spriteBatch.DrawString(Arial_20PT, "Passed: " + testThumbStick.Passed[0].ToString() + ", " + testThumbStick.Passed[1].ToString(), new Vector2(210, 0), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    /*
                    spriteBatch.DrawString(Arial_20PT, "Distance: " + tempVector.ToString(), new Vector2(210, 0), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(Arial_20PT, "Follow Pos: " +testThumbStick.FollowThumbStick.ToString(), new Vector2(210, 30), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(Arial_20PT, "Current Pos: " + testThumbStick.CurrentThumbStick.ToString(), new Vector2(210, 60), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(Arial_20PT, "Current Speed: " + testThumbStick.FollowSpeed.ToString(), new Vector2(210, 90), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(Arial_20PT, "Current Accel: " + testThumbStick.FollowAccel.ToString(), new Vector2(210, 120), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    
                    spriteBatch.DrawString(Arial_20PT, "InCircle: " + testThumbStick.InCircle.ToString(), new Vector2(210, 150), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    spriteBatch.DrawString(Arial_20PT, "LockCount: " + testThumbStick.LockCount.ToString(), new Vector2(210, 180), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    
                    spriteBatch.DrawString(Arial_20PT, "testBool: " + testBool.ToString(), new Vector2(210, 210), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);
                    */
                    //Player Two HUD
                    spriteBatch.Draw(GlobalVariables.ImageDict["Health"], new Rectangle(graphics.GraphicsDevice.Viewport.Width - 200 * playerTwo.Health / 100, 0, 200 * playerTwo.Health / 100, 5), HUDBarSourceRect, Color.Red, 0F, Vector2.Zero, SpriteEffects.None, 1F);
                    spriteBatch.Draw(GlobalVariables.ImageDict["Mana"], new Rectangle(graphics.GraphicsDevice.Viewport.Width - 200 * playerTwo.Mana / 100, 6, 200 * playerTwo.Mana / 100, 5), HUDBarSourceRect, Color.Blue, 0F, Vector2.Zero, SpriteEffects.None, 1F);
                    spriteBatch.Draw(GlobalVariables.ImageDict["Stamina"], new Rectangle(graphics.GraphicsDevice.Viewport.Width - 200 * (int)playerTwo.Stamina / 100, 12, 200 * (int)playerTwo.Stamina / 100, 5), HUDBarSourceRect, Color.Yellow, 0F, Vector2.Zero, SpriteEffects.None, 1F);
                    spriteBatch.DrawString(Arial_20PT, "Lives: " + playerTwoStats.Lives.ToString(), new Vector2(graphics.GraphicsDevice.Viewport.Width - 205 - Arial_20PT.MeasureString("Lives: " + playerTwoStats.Lives.ToString()).X, 0), Color.Red, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1);


                    /*
                    spriteBatch.DrawString(Arial_20PT, "Health: " + playerOne.Health.ToString(), new Vector2(15, 0), Color.Red);
                    spriteBatch.DrawString(Arial_20PT, "Mana: " + playerOne.Mana.ToString(), new Vector2(15, 40), Color.Blue);
                    spriteBatch.DrawString(Arial_20PT, "Lives: " + playerOneStats.Lives.ToString(), new Vector2(15, 80), Color.Red);

                    spriteBatch.DrawString(Arial_20PT, "Health: " + playerTwo.Health.ToString(), new Vector2(graphics.GraphicsDevice.Viewport.Width - 100, 0), Color.Red);
                    spriteBatch.DrawString(Arial_20PT, "Mana: " + playerTwo.Mana.ToString(), new Vector2(graphics.GraphicsDevice.Viewport.Width - 100, 40), Color.Blue);
                    spriteBatch.DrawString(Arial_20PT, "Lives: " + playerTwoStats.Lives.ToString(), new Vector2(graphics.GraphicsDevice.Viewport.Width - 100, 80), Color.Red);
                    */
                    //spriteBatch.DrawString(Arial_20PT, "Obj Count: " + objectList.Count.ToString(), new Vector2(0, 0), Color.Red);
                    //spriteBatch.DrawString(Arial_20PT, "Miliseconds on Section: " + myWatch.ElapsedMilliseconds.ToString(), new Vector2(0, 50), Color.Red);
                    break;
                case gameState.GameOver:
                    spriteBatch.DrawString(Arial_20PT, "Game Over!", new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString("Game Over").X)/2, 100), Color.Purple);
                    spriteBatch.DrawString(Arial_20PT, winMessage, new Vector2((graphics.GraphicsDevice.Viewport.Width - Arial_20PT.MeasureString(winMessage).X)/2, 200), Color.Purple);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
