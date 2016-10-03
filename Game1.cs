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

namespace Blackbeard
{
    /// <summary>
    /// CODED BY MATTHEW ROY
    /// v0.2.2
    /// NOVEMBER 28, 2013. 11:00 AM.
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // resolution support
        const int WINDOW_WIDTH = 1440;
        const int WINDOW_HEIGHT = 900;

        // sprite support
        Texture2D scallywagSprite;
        Texture2D bulletSprite;
        Texture2D scallywagCorpseSprite;
        Texture2D townsmanSprite;

        // drawing support
        Texture2D menu;
        Texture2D house;
        Texture2D enterMessage;
        Texture2D leaveMessage;
        Texture2D shopMessage;
        Rectangle drawRectangleShopMessage;
        Texture2D door;
        Texture2D shopKeeper;
        Rectangle drawRectangleShopKeeper;
        Rectangle drawRectangleLeaveMessage;
        Rectangle drawRectangleDoor;
        Rectangle drawRectangleEnterMessage;
        Rectangle drawRectangleMenu;
        Rectangle drawRectangleHouse;

        // background support
        List<Texture2D> background = new List<Texture2D>(3);
        Vector2 backgroundPosition;
        BackgroundNumber backgroundNumber = BackgroundNumber.Zero;
        int mapGroundHeight;

        // object list support
        List<Scallywag> scallywags = new List<Scallywag>();
        List<Bullet> bullets = new List<Bullet>();
        List<ScallywagCorpse> scallywagCorpses = new List<ScallywagCorpse>();
        List<Townsman> townsmans = new List<Townsman>();

        // object support
        Vector2 scallywagCorpseLocation;
        public Blackbeard blackbeard;
        WeaponHud weaponHud;

        // spawning support
        const int TOTAL_SPAWN_DELAY_MILLISECONDS = 1000;
        int elapsedSpawnDelayMilliseconds = 0;

        //// bullet time support
        //const int TOTAL_BULLET_MILLISECONDS = 1500;
        //int elapsedBulletMilliseconds = 0;

        // random support
        Random rand = new Random();

        // set the game state
        GameState gameState = GameState.Menu;

        // button support
        bool startButtonPreviouslyPressed = false;
        bool rTriggerPreviouslyPressed = false;
        bool aButtonPreviouslyPressed = false;
        bool yButtonPreviouslyPressed = false;
        bool bButtonPreviouslyPressed = false;
        
        // audio components
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        // text support
        SpriteFont font;
        int gold = 0;
        int health = 3;
        int ammo = 20;
        int xp = 0;
        int level = 1;

        // get locations
        float scallywagLocationX;
        float scallywagLocationY;
        float gunLocationX;
        bool blackbeardEnteringHouse;
        bool blackbeardLeavingHouse;
        bool blackbeardShopping;

        #endregion

        #region Constructors

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // set resolution
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            //IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        #endregion

        #region Methods

        #region Load Content

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load audio content
            audioEngine = new AudioEngine(@"Content\BlackbeardAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Sound Bank.xsb");

            // load font
            font = Content.Load<SpriteFont>("Arial");

            // play background music
            soundBank.PlayCue("backgroundMusic");

            // load blackbeard
            blackbeard = new Blackbeard(Content, "blackbeard", WINDOW_WIDTH / 8,
                WINDOW_HEIGHT / 2, WINDOW_WIDTH, WINDOW_HEIGHT, soundBank, mapGroundHeight);

            // load sprites
            scallywagSprite = Content.Load<Texture2D>("scallywag");
            bulletSprite = Content.Load<Texture2D>("bullet");
            scallywagCorpseSprite = Content.Load<Texture2D>("scallywagCorpse");
            townsmanSprite = Content.Load<Texture2D>("townsman");

            // load the weapon hud
            weaponHud = new WeaponHud(Content, 0, 0);

            // load the menu
            menu = Content.Load<Texture2D>("menu");
            drawRectangleMenu = new Rectangle(WINDOW_WIDTH / 2 - menu.Width / 2,
                WINDOW_HEIGHT / 2 - menu.Height / 2, menu.Width, menu.Height);

            // load the backgrounds
            for (int i = 0; i < 8; i++)
            {
                background.Add(Content.Load<Texture2D>("background" + i));
            }
            backgroundPosition = new Vector2(0, 0);

            // load the house
            house = Content.Load<Texture2D>("house");
            drawRectangleHouse = new Rectangle(WINDOW_WIDTH - 375, WINDOW_HEIGHT - 875,
                house.Width, house.Height);

            // load the enter/leave messages
            enterMessage = Content.Load<Texture2D>("entermessage");
            drawRectangleEnterMessage = new Rectangle(WINDOW_WIDTH - enterMessage.Width, 0, 
                enterMessage.Width, enterMessage.Height);
            leaveMessage = Content.Load<Texture2D>("leavemessage");
            drawRectangleLeaveMessage = new Rectangle(WINDOW_WIDTH - leaveMessage.Width, 0,
                leaveMessage.Width, leaveMessage.Height);

            // load the door
            door = Content.Load<Texture2D>("door");
            drawRectangleDoor = new Rectangle(WINDOW_WIDTH / 20,
                WINDOW_HEIGHT / 2 - door.Height / 2 - 75, door.Width, door.Height);

            // load the shopkeeper
            shopKeeper = Content.Load<Texture2D>("shopkeeper");
            drawRectangleShopKeeper = new Rectangle(WINDOW_WIDTH - (shopKeeper.Width * 5),
                WINDOW_HEIGHT / 2 - shopKeeper.Height / 2, shopKeeper.Width, shopKeeper.Height);

            // load the shop message
            shopMessage = Content.Load<Texture2D>("shopmessage");
            drawRectangleShopMessage = new Rectangle(WINDOW_WIDTH / 2 - shopMessage.Width / 2,
                WINDOW_HEIGHT / 2 - shopMessage.Height / 2, shopMessage.Width, shopMessage.Height);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // gamepad support
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            if (gamePad.IsConnected)
            {

                #region Menu

                // play game on start button
                if (gameState == GameState.Menu)
                {
                    if (gamePad.IsButtonUp(Buttons.Start) &&
                        startButtonPreviouslyPressed)
                    {
                        gameState = GameState.Play;
                    }
                    startButtonPreviouslyPressed = gamePad.IsButtonDown(Buttons.Start);
                }

                #endregion

                #region Play Game

                else if (gameState == GameState.Play)
                {
                    #region Screen Properties

                    // general screen properties
                    if (backgroundNumber == BackgroundNumber.Zero ||
                        backgroundNumber == BackgroundNumber.One ||
                        backgroundNumber == BackgroundNumber.Two ||
                        backgroundNumber == BackgroundNumber.Three ||
                        backgroundNumber == BackgroundNumber.Four ||
                        backgroundNumber == BackgroundNumber.Five ||
                        backgroundNumber == BackgroundNumber.Six)
                    {
                        // set the map ground height
                        blackbeard.WindowHeight = WINDOW_HEIGHT;
                        mapGroundHeight = 330;
                        blackbeard.MapGroundHeight = mapGroundHeight;
                    }

                    if (backgroundNumber == BackgroundNumber.Six)
                    {
                        mapGroundHeight = 0;
                        blackbeard.MapGroundHeight = mapGroundHeight;
                    }
                    if (backgroundNumber == BackgroundNumber.Seven)
                    {
                        blackbeard.WindowHeight = 580;
                        mapGroundHeight = 330;
                        blackbeard.MapGroundHeight = mapGroundHeight;
                    }

                    #endregion

                    #region Changing Screens

                    // change screen at edge
                    if (blackbeard.CollisionRectangle.Right == WINDOW_WIDTH &&
                        backgroundNumber == BackgroundNumber.Zero)
                    {
                        // background 0 to 1
                        backgroundNumber = BackgroundNumber.One;
                        blackbeard.ScreenChange("right");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Left == 0 &&
                        backgroundNumber == BackgroundNumber.One)
                    {
                        // background 1 to 0
                        backgroundNumber = BackgroundNumber.Zero;
                        blackbeard.ScreenChange("left");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Right == WINDOW_WIDTH &&
                        backgroundNumber == BackgroundNumber.One)
                    {
                        // background 1 to 2
                        backgroundNumber = BackgroundNumber.Two;
                        blackbeard.ScreenChange("right");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Left == 0 &&
                        backgroundNumber == BackgroundNumber.Two)
                    {
                        // background 2 to 1
                        backgroundNumber = BackgroundNumber.One;
                        blackbeard.ScreenChange("left");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Right == WINDOW_WIDTH &&
                        backgroundNumber == BackgroundNumber.Two)
                    {
                        // background 2 to 3
                        backgroundNumber = BackgroundNumber.Three;
                        blackbeard.ScreenChange("right");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Left == 0 &&
                        backgroundNumber == BackgroundNumber.Three)
                    {
                        // background 3 to 2
                        backgroundNumber = BackgroundNumber.Two;
                        blackbeard.ScreenChange("left");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Right == WINDOW_WIDTH &&
                        backgroundNumber == BackgroundNumber.Three)
                    {
                        // background 3 to 4
                        backgroundNumber = BackgroundNumber.Four;
                        blackbeard.ScreenChange("right");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Left == 0 &&
                        backgroundNumber == BackgroundNumber.Four)
                    {
                        // background 4 to 3
                        backgroundNumber = BackgroundNumber.Three;
                        blackbeard.ScreenChange("left");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Right == WINDOW_WIDTH &&
                        backgroundNumber == BackgroundNumber.Four)
                    {
                        // background 4 to 5
                        backgroundNumber = BackgroundNumber.Five;
                        blackbeard.ScreenChange("right");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Left == 0 &&
                        backgroundNumber == BackgroundNumber.Five)
                    {
                        // background 5 to 4
                        backgroundNumber = BackgroundNumber.Four;
                        blackbeard.ScreenChange("left");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Right == WINDOW_WIDTH &&
                        backgroundNumber == BackgroundNumber.Five)
                    {
                        // background 5 to 6
                        backgroundNumber = BackgroundNumber.Six;
                        blackbeard.ScreenChange("right");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    else if (blackbeard.CollisionRectangle.Left == 0 &&
                        backgroundNumber == BackgroundNumber.Six)
                    {
                        // background 6 to 5
                        backgroundNumber = BackgroundNumber.Five;
                        blackbeard.ScreenChange("left");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    #endregion

                    #region Updating Elements

                    // pause game on start button 
                    if (gamePad.IsButtonUp(Buttons.Start) &&
                        startButtonPreviouslyPressed)
                    {
                        gameState = GameState.Menu;
                    }
                    startButtonPreviouslyPressed = gamePad.IsButtonDown(Buttons.Start);

                    // update blackbeard
                    blackbeard.Update(gameTime, gamePad);

                    // update scallywags
                    for (int i = 0; i < scallywags.Count; i++)
                    {
                        scallywags[i].Update();
                    }

                    // update townsman
                    for (int i = 0; i < townsmans.Count; i++)
                    {
                        townsmans[i].Update();
                    }

                    // update bullets
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        // update the bullet
                        bullets[i].Update();

                        // check for bullet outside of screen
                        if (bullets[i].CollisionRectangle.Left > WINDOW_WIDTH +
                            bullets[i].CollisionRectangle.Width ||
                            bullets[i].CollisionRectangle.Right < 0)
                        {
                            bullets[i].Active = false;
                        }

                        //// check for bullet lifetime (3 sec)
                        //elapsedBulletMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                        //if (elapsedBulletMilliseconds > TOTAL_BULLET_MILLISECONDS)
                        //{
                        //    // reset timer
                        //    elapsedBulletMilliseconds = 0;

                        //    // set the first bullet shot to be inactive
                        //    bullets.First<Bullet>().Active = false;
                        //}
                    }

                    // update the hud
                    weaponHud.Update(gameTime, gamePad);

                    #endregion

                    #region Actions

                    // buy bullets on Y button
                    if (gamePad.IsButtonUp(Buttons.Y) &&
                        yButtonPreviouslyPressed &&
                        blackbeardShopping &&
                        gold >= 35 &&
                        ammo < 100)
                    {
                        // subtract gold and give ammo
                        gold -= 35;
                        ammo += 10;

                        // play audio
                        soundBank.PlayCue("chaChing");
                    }
                    yButtonPreviouslyPressed = gamePad.IsButtonDown(Buttons.Y);

                    // buy health on B button
                    if (gamePad.IsButtonUp(Buttons.B) &&
                        bButtonPreviouslyPressed &&
                        blackbeardShopping &&
                        gold >= 50 &&
                        health < 5)
                    {
                        // subtract gold and give health
                        gold -= 50;
                        health += 1;

                        // play audio
                        soundBank.PlayCue("chaChing");
                    }
                    bButtonPreviouslyPressed = gamePad.IsButtonDown(Buttons.B);

                    // go into house on A button
                    if (gamePad.IsButtonUp(Buttons.A) &&
                        aButtonPreviouslyPressed &&
                        blackbeardEnteringHouse)
                    {
                        backgroundNumber = BackgroundNumber.Seven;
                        blackbeard.ScreenChange("enterHouse");

                        // play audio
                        soundBank.PlayCue("door");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    // leave house on A button
                    else if (gamePad.IsButtonUp(Buttons.A) &&
                        aButtonPreviouslyPressed &&
                        blackbeardLeavingHouse)
                    {
                        backgroundNumber = BackgroundNumber.Six;
                        blackbeard.ScreenChange("leaveHouse");

                        // play audio
                        soundBank.PlayCue("door");

                        // make entities inactive
                        for (int i = 0; i < scallywags.Count; i++)
                        {
                            // make scallywags inactive
                            scallywags[i].Active = false;
                        }
                        for (int j = 0; j < scallywagCorpses.Count; j++)
                        {
                            // make scallywag corpses inactive
                            scallywagCorpses[j].Active = false;
                        }
                        for (int k = 0; k < bullets.Count; k++)
                        {
                            // make bullets inactive
                            bullets[k].Active = false;
                        }
                    }

                    aButtonPreviouslyPressed = gamePad.IsButtonDown(Buttons.A);

                    // set max npcs in town to 4
                    if (townsmans.Count <= 4)
                    {
                        townsmans.Add(GetTownsman());
                    }

                    // end game at 0 health
                    if (health == 0)
                    {
                        gameState = GameState.GameOver;
                    }

                    // max ammo at 100
                    if (ammo > 100)
                    {
                        ammo = 100;
                    }

                    // restrict scallywag spawning
                    if (backgroundNumber == BackgroundNumber.Five ||
                        backgroundNumber == BackgroundNumber.Six ||
                        backgroundNumber == BackgroundNumber.Seven)
                    {
                        elapsedSpawnDelayMilliseconds = TOTAL_SPAWN_DELAY_MILLISECONDS * -1;
                    }

                    // spawn scallywag
                    elapsedSpawnDelayMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedSpawnDelayMilliseconds > TOTAL_SPAWN_DELAY_MILLISECONDS)
                    {
                        // reset timer
                        elapsedSpawnDelayMilliseconds = 0;

                        // spawn new enemy
                        scallywags.Add(GetScallywag());

                        // set scallywag to active
                        scallywags.Last<Scallywag>().Active = true;
                    }

                    // shoot a bullet on right trigger
                    if (gamePad.IsButtonUp(Buttons.RightTrigger) &&
                        rTriggerPreviouslyPressed)
                    {
                        // add a new bullet if pistol is equipped
                        if (blackbeard.EquippedWeapon == CurrentWeapon.Pistol)
                        {
                            // add a new bullet if there is ammo
                            if (ammo != 0)
                            {
                                // subtract ammo
                                ammo--;

                                // play audio
                                soundBank.PlayCue("gunShot");

                                // get new bullet
                                bullets.Add(GetBullet());

                                // set bullet to active
                                bullets.Last<Bullet>().Active = true;

                                // set direction of last bullet
                                if (blackbeard.BlackbeardDirection == BlackbeardDirection.Right)
                                {
                                    bullets.Last<Bullet>().Direction = "right";
                                }
                                else if (blackbeard.BlackbeardDirection == BlackbeardDirection.Left)
                                {
                                    bullets.Last<Bullet>().Direction = "left";
                                }
                                else if (blackbeard.BlackbeardDirection == BlackbeardDirection.Front)
                                {
                                    if (blackbeard.FacingRight)
                                    {
                                        bullets.Last<Bullet>().Direction = "right";
                                    }
                                    else if (!blackbeard.FacingRight)
                                    {
                                        bullets.Last<Bullet>().Direction = "left";
                                    }
                                }
                            }
                            else if (ammo == 0)
                            {
                                // play audio
                                soundBank.PlayCue("emptyGun");
                            }
                        }
                    }

                    // swing sword on right trigger
                    if (gamePad.IsButtonUp(Buttons.RightTrigger) &&
                        rTriggerPreviouslyPressed)
                    {
                        if (blackbeard.EquippedWeapon == CurrentWeapon.Sword)
                        {
                            // play audio
                            soundBank.PlayCue("swordSlash");

                            // sword and scallywag collision
                            for (int i = 0; i < scallywags.Count; i++)
                            {
                                if (blackbeard.SwordRectangle.Intersects(scallywags[i].CollisionRectangle))
                                {
                                    // get location for scallywag corpse
                                    float scallywagCorpseLocationX = scallywags[i].CollisionRectangle.X + scallywagSprite.Width / 2;
                                    float scallywagCorpseLocationY = scallywags[i].CollisionRectangle.Y + scallywagSprite.Height / 2;
                                    scallywagCorpseLocation = new Vector2(scallywagCorpseLocationX, scallywagCorpseLocationY);

                                    // add scallywag corpse and set to active
                                    scallywagCorpses.Add(GetScallywagCorpse(scallywagCorpseLocation));
                                    scallywagCorpses.Last<ScallywagCorpse>().Active = true;

                                    // set scallywag inactive
                                    scallywags[i].Active = false;

                                    // play audio
                                    soundBank.PlayCue("scallywagDeath");
                                }
                            }
                        }
                    }
                    rTriggerPreviouslyPressed = gamePad.IsButtonDown(Buttons.RightTrigger);

                    #endregion

                    #region Collision Detection

                    // check for collisions of blackbeard and shopkeeper
                    if (blackbeard.CollisionRectangle.Intersects(drawRectangleShopKeeper))
                    {
                        blackbeardShopping = true;
                    }
                    else
                    {
                        blackbeardShopping = false;
                    }

                    // check for collisions of blackbeard and door
                    if (blackbeard.CollisionRectangle.Intersects(drawRectangleDoor))
                    {
                        blackbeardLeavingHouse = true;
                    }
                    else
                    {
                        blackbeardLeavingHouse = false;
                    }

                    // check for collisions of blackbeard and house
                    if (blackbeard.CollisionRectangle.Intersects(drawRectangleHouse))
                    {
                        // put blackbeard in house
                        blackbeardEnteringHouse = true;
                    }
                    else
                    {
                        blackbeardEnteringHouse = false;
                    }

                    // check for collisions of bullets and scallywags
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        for (int j = 0; j < scallywags.Count; j++)
                        {
                            if (bullets[i].CollisionRectangle.Intersects(scallywags[j].CollisionRectangle))
                            {
                                // set bullet inactive
                                bullets[i].Active = false;

                                // get location for scallywag corpse
                                float scallywagCorpseLocationX = scallywags[j].CollisionRectangle.X + scallywagSprite.Width / 2;
                                float scallywagCorpseLocationY = scallywags[j].CollisionRectangle.Y + scallywagSprite.Height / 2;
                                scallywagCorpseLocation = new Vector2(scallywagCorpseLocationX, scallywagCorpseLocationY);
                                
                                // add scallywag corpse and set to active
                                scallywagCorpses.Add(GetScallywagCorpse(scallywagCorpseLocation));
                                scallywagCorpses.Last<ScallywagCorpse>().Active = true;

                                // set scallywag inactive
                                scallywags[j].Active = false;

                                // play audio
                                soundBank.PlayCue("scallywagDeath");

                                // add xp
                                xp += 1;
                            }
                        }
                    }

                    // check for collisions of blackbeard and scallywag corpse to loot
                    for (int i = 0; i < scallywagCorpses.Count; i++)
                    {
                        if (blackbeard.CollisionRectangle.Intersects(scallywagCorpses[i].CollisionRectangle))
                        {
                            // set scallywag corpse inactive
                            scallywagCorpses[i].Active = false;

                            // play looting sound
                            soundBank.PlayCue("lootingScallywag");

                            // add gold (random 1 through 3)
                            gold += rand.Next(1, 4);

                            // add ammo
                            ammo += rand.Next(0, 3);
                        }
                    }

                    // check for collisions of blackbeard and scallywag enemy
                    for (int i = 0; i < scallywags.Count; i++)
                    {
                        if (blackbeard.CollisionRectangle.Intersects(scallywags[i].CollisionRectangle))
                        {
                            // set enemy inactive
                            scallywags[i].Active = false;

                            // play audio
                            soundBank.PlayCue("blackbeardHurt");

                            // reduce health
                            health--;

                            // blink blackbeard to simulate the effect of taking damage

                            // vibrate controller
                        }
                    }

                    #endregion

                    #region Remove Entitites

                    // remove inactive corpses
                    for (int i = scallywagCorpses.Count - 1; i >= 0; i--)
                    {
                        if (!scallywagCorpses[i].Active)
                        {
                            scallywagCorpses.RemoveAt(i);
                        }
                    }

                    // remove inactive bullets
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        if (!bullets[i].Active)
                        {
                            bullets.RemoveAt(i);
                        }
                    }

                    // remove inactive scallywags
                    for (int i = scallywags.Count - 1; i >= 0; i--)
                    {
                        if (!scallywags[i].Active)
                        {
                            scallywags.RemoveAt(i);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Game Over

                else if (gameState == GameState.GameOver)
                {
                    this.Exit();
                }

                #endregion
            }

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // drawing support
            spriteBatch.Begin();

            #region Drawing Backgrounds

            // draw the background
            if (backgroundNumber == BackgroundNumber.Zero)
            {
                spriteBatch.Draw(background[0], backgroundPosition, Color.White);
            }
            else if (backgroundNumber == BackgroundNumber.One)
            {
                spriteBatch.Draw(background[1], backgroundPosition, Color.White);
            }
            else if (backgroundNumber == BackgroundNumber.Two)
            {
                spriteBatch.Draw(background[2], backgroundPosition, Color.White);
            }
            else if (backgroundNumber == BackgroundNumber.Three)
            {
                spriteBatch.Draw(background[3], backgroundPosition, Color.White);
            }
            else if (backgroundNumber == BackgroundNumber.Four)
            {
                spriteBatch.Draw(background[4], backgroundPosition, Color.White);
            }
            else if (backgroundNumber == BackgroundNumber.Five)
            {
                spriteBatch.Draw(background[5], backgroundPosition, Color.White);
            }
            else if (backgroundNumber == BackgroundNumber.Six)
            {
                spriteBatch.Draw(background[6], backgroundPosition, Color.White);
            }
            else if (backgroundNumber == BackgroundNumber.Seven)
            {
                spriteBatch.Draw(background[7], backgroundPosition, Color.White);
            }

            #endregion

            #region Drawing Elements

            // draw elements in town
            if (backgroundNumber == BackgroundNumber.Six)
            {
                // draw the house
                spriteBatch.Draw(house, drawRectangleHouse, Color.White);

                // draw the enter message
                if (blackbeardEnteringHouse)
                {
                    spriteBatch.Draw(enterMessage, drawRectangleEnterMessage, Color.White);
                }

                // draw the townsman
                for (int i = 0; i < townsmans.Count; i++)
                {
                    townsmans[i].Draw(spriteBatch);
                }
            }

            // draw elements in the house
            else if (backgroundNumber == BackgroundNumber.Seven)
            {
                // draw the door
                spriteBatch.Draw(door, drawRectangleDoor, Color.White);

                // draw the leave message
                if (blackbeardLeavingHouse)
                {
                    spriteBatch.Draw(leaveMessage, drawRectangleLeaveMessage, Color.White);
                }

                // draw the shopkeeper
                spriteBatch.Draw(shopKeeper, drawRectangleShopKeeper, Color.White);

                // draw the shopkeeper message
                if (blackbeardShopping)
                {
                    spriteBatch.Draw(shopMessage, drawRectangleShopMessage, Color.White);
                }
            }

            // draw the scallywags corpses
            for (int i = 0; i < scallywagCorpses.Count; i++)
            {
                scallywagCorpses[i].Draw(spriteBatch);
            }

            // draw the scallywags
            for (int i = 0; i < scallywags.Count; i++)
            {
                scallywags[i].Draw(spriteBatch);
            }

            // draw bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }

            // draw blackbeard
            blackbeard.Draw(spriteBatch);

            #endregion

            #region Drawing HUD and Menu

            // draw hud text
            spriteBatch.DrawString(font, "Current Weapon: " + blackbeard.EquippedWeapon, new Vector2(0, weaponHud.CollisionRectangle.Height), Color.Black);
            spriteBatch.DrawString(font, "Gold: " + gold, new Vector2(0, weaponHud.CollisionRectangle.Height + 20), Color.Black);
            spriteBatch.DrawString(font, "Health: " + health, new Vector2(0, weaponHud.CollisionRectangle.Height + 40), Color.Black);
            spriteBatch.DrawString(font, "Level: " + level + " (XP: " + xp + ")", new Vector2(0, weaponHud.CollisionRectangle.Height + 60), Color.Black);
            
            // draw ammo hud text if pistol is equipped
            if (blackbeard.EquippedWeapon == CurrentWeapon.Pistol)
            {
                spriteBatch.DrawString(font, "(Ammo: " + ammo + ")", new Vector2(weaponHud.CollisionRectangle.Width - 85, weaponHud.CollisionRectangle.Height), Color.Black);
            }

            // draw hud text in white if in house
            if (backgroundNumber == BackgroundNumber.Seven)
            {
                spriteBatch.DrawString(font, "Current Weapon: " + blackbeard.EquippedWeapon, new Vector2(0, weaponHud.CollisionRectangle.Height), Color.White);
                spriteBatch.DrawString(font, "Gold: " + gold, new Vector2(0, weaponHud.CollisionRectangle.Height + 20), Color.White);
                spriteBatch.DrawString(font, "Health: " + health, new Vector2(0, weaponHud.CollisionRectangle.Height + 40), Color.White);
                spriteBatch.DrawString(font, "Level: " + level + " (XP: " + xp + ")", new Vector2(0, weaponHud.CollisionRectangle.Height + 60), Color.White);

                // draw ammo hud text if pistol is equipped
                if (blackbeard.EquippedWeapon == CurrentWeapon.Pistol)
                {
                    spriteBatch.DrawString(font, "(Ammo: " + ammo + ")", new Vector2(weaponHud.CollisionRectangle.Width - 85, weaponHud.CollisionRectangle.Height), Color.White);
                }
            }

            // draw the weapon hud
            weaponHud.Draw(spriteBatch);

            // draw the menu
            if (gameState == GameState.Menu)
            {
                spriteBatch.Draw(menu, drawRectangleMenu, Color.White);
            }

            #endregion

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a new scallywag enemy
        /// </summary>
        /// <returns>scallywag</returns>
        private Scallywag GetScallywag()
        {
            // set initial location
            scallywagLocationX = rand.Next(1 + scallywagSprite.Width, WINDOW_WIDTH - scallywagSprite.Width);
            scallywagLocationY = rand.Next(1 + scallywagSprite.Height + mapGroundHeight, WINDOW_HEIGHT - scallywagSprite.Height);

            // get location for scallywag 90 pixels away from blackbeard
            while (blackbeard.CollisionRectangle.Contains((int)scallywagLocationX, (int)scallywagLocationY) ||
                Math.Abs(scallywagLocationX - blackbeard.CollisionRectangle.X) < 90 ||
                Math.Abs(scallywagLocationY - blackbeard.CollisionRectangle.Y) < 90)
            {
                scallywagLocationX = rand.Next(1 + scallywagSprite.Width, WINDOW_WIDTH - scallywagSprite.Width);
                scallywagLocationY = rand.Next(1 + scallywagSprite.Height + mapGroundHeight, WINDOW_HEIGHT - scallywagSprite.Height);
            }

            // return a new scallywag
            Vector2 location = new Vector2(scallywagLocationX, scallywagLocationY);
            return new Scallywag(scallywagSprite, location,
                WINDOW_WIDTH, WINDOW_HEIGHT, mapGroundHeight);
        }

        /// <summary>
        /// Gets a new townsman
        /// </summary>
        /// <returns>townsman</returns>
        private Townsman GetTownsman()
        {
            // get location for townsman
            float locationX = rand.Next(0 + townsmanSprite.Width +150,
                (WINDOW_WIDTH - townsmanSprite.Width) + 1 - 150);
            float locationY = rand.Next(0 + townsmanSprite.Height + 150,
                (WINDOW_HEIGHT - townsmanSprite.Height) + 1 - 150);
            Vector2 location = new Vector2(locationX, locationY);

            // return a new townsman
            return new Townsman(townsmanSprite, location, WINDOW_WIDTH, WINDOW_HEIGHT, mapGroundHeight);
        }

        /// <summary>
        /// Gets a new scallywag corpse
        /// </summary>
        /// <returns>scallywag corpse</returns>
        private ScallywagCorpse GetScallywagCorpse(Vector2 location)
        {
            // return a new scallywag corpse
            return new ScallywagCorpse(scallywagCorpseSprite, location);
        }

        /// <summary>
        /// Returns a new bullet
        /// </summary>
        /// <returns>bullet</returns>
        private Bullet GetBullet()
        {
            // get gunLocationX
            if (blackbeard.FacingRight)
            {
                gunLocationX = blackbeard.GunRectangle.Right + bulletSprite.Width;
            }
            else if (!blackbeard.FacingRight)
            {
                gunLocationX = blackbeard.GunRectangle.Left - bulletSprite.Width;
            }

            // get firing location for bullet
            float locationY = blackbeard.GunRectangle.Y + blackbeard.GunRectangle.Height / 8;
            Vector2 location = new Vector2(gunLocationX, locationY);

            // return the new bullet
            return new Bullet(bulletSprite, location);
        }

        #endregion
    }
}