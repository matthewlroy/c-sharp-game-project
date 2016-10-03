using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// extra references
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Blackbeard
{
    /// <summary>
    /// The main character blackbeard
    /// </summary>
    public class Blackbeard
    {
        #region Fields

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;
        Texture2D gunTexture;
        Texture2D gunTextureLeft;
        Rectangle drawRectangleGun;
        Rectangle drawRectangleGunLeft;
        Texture2D swordTexture;
        Texture2D swordTextureLeft;
        Rectangle drawRectangleSword;
        Rectangle drawRectangleSwordLeft;
        Texture2D spriteRight;
        Rectangle drawRectangleRight;
        Texture2D spriteLeft;
        Rectangle drawRectangleLeft;

        // movement support
        const int THUMBSTICK_DEFLECTION_AMOUNT = 7;
        bool facingRight = true;
        Vector2 deflection;
        BlackbeardDirection blackbeardDirection = BlackbeardDirection.Front;

        // resolution support
        int windowWidth;
        int windowHeight;
        int mapGroundHeight;

        // button support
        bool xButtonPreviouslyPressed = false;

        // weapon support
        CurrentWeapon currentWeapon = CurrentWeapon.None;
        bool weaponIsChanged = false;

        // audio components
        SoundBank soundBank;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs blackbeard
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="spriteName">the sprite name for blackbeard</param>
        /// <param name="x">the x location</param>
        /// <param name="y">the y location</param>
        public Blackbeard(ContentManager contentManager, string spriteName, int x, int y)
        {
            // load content
            LoadContent(contentManager, spriteName, x, y);
        }

        /// <summary>
        /// Constructs blackbeard
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="spriteName">the sprite name for blackbeard</param>
        /// <param name="x">the x location</param>
        /// <param name="y">the y location</param>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        public Blackbeard(ContentManager contentManager, string spriteName, int x, int y, int windowWidth, int windowHeight)
        {
            // store resolution variables
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            // load content
            LoadContent(contentManager, spriteName, x, y);
        }

        /// <summary>
        /// Constructs blackbeard
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="spriteName">the sprite name for blackbeard</param>
        /// <param name="x">the x location</param>
        /// <param name="y">the y location</param>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        /// <param name="soundBank">the sound bank for playing cues</param>
        public Blackbeard(ContentManager contentManager, string spriteName, int x, int y, int windowWidth, int windowHeight, SoundBank soundBank)
        {
            // store resolution variables
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            // variable support
            this.soundBank = soundBank;

            // load content
            LoadContent(contentManager, spriteName, x, y);
        }

        /// <summary>
        /// Constructs blackbeard
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="spriteName">the sprite name for blackbeard</param>
        /// <param name="x">the x location</param>
        /// <param name="y">the y location</param>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        /// <param name="soundBank">the sound bank for playing cues</param>
        /// <param name="mapGroundHeight">the height of the map from the top of the screen</param>
        public Blackbeard(ContentManager contentManager, string spriteName, int x, int y, int windowWidth, int windowHeight, SoundBank soundBank, int mapGroundHeight)
        {
            // store resolution variables
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.mapGroundHeight = mapGroundHeight;

            // variable support
            this.soundBank = soundBank;

            // load content
            LoadContent(contentManager, spriteName, x, y);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws blackbeard
        /// </summary>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw blackbeard
            if (blackbeardDirection == BlackbeardDirection.Front)
            {
                spriteBatch.Draw(sprite, drawRectangle, Color.White);

                // draw weapons
                if (currentWeapon == CurrentWeapon.Pistol)
                {
                    if (facingRight)
                    {
                        spriteBatch.Draw(gunTexture, drawRectangleGun, Color.White);
                    }
                    else if (!facingRight)
                    {
                        spriteBatch.Draw(gunTextureLeft, drawRectangleGunLeft, Color.White);
                    }
                }
                else if (currentWeapon == CurrentWeapon.Sword)
                {
                    if (facingRight)
                    {
                        spriteBatch.Draw(swordTexture, drawRectangleSword, Color.White);
                    }
                    if (!facingRight)
                    {
                        spriteBatch.Draw(swordTextureLeft, drawRectangleSwordLeft, Color.White);
                    }
                }
            }
            else if (blackbeardDirection == BlackbeardDirection.Right)
            {
                spriteBatch.Draw(spriteRight, drawRectangleRight, Color.White);

                // draw weapons
                if (currentWeapon == CurrentWeapon.Pistol)
                {
                    spriteBatch.Draw(gunTexture, drawRectangleGun, Color.White);
                }
                else if (currentWeapon == CurrentWeapon.Sword)
                {
                    spriteBatch.Draw(swordTexture, drawRectangleSword, Color.White);
                } 
            }
            else if (blackbeardDirection == BlackbeardDirection.Left)
            {
                spriteBatch.Draw(spriteLeft, drawRectangleLeft, Color.White);

                // draw weapons
                if (currentWeapon == CurrentWeapon.Pistol)
                {
                    spriteBatch.Draw(gunTextureLeft, drawRectangleGunLeft, Color.White);
                }
                else if (currentWeapon == CurrentWeapon.Sword)
                {
                    spriteBatch.Draw(swordTextureLeft, drawRectangleSwordLeft, Color.White);
                }
            }
        }

        /// <summary>
        /// Gives movement and updates blackbeard
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="gamePad">the game pad</param>
        public void Update(GameTime gameTime, GamePadState gamePad)
        {
            // gamepad support
            deflection = gamePad.ThumbSticks.Left;

            // blackbeard movement
            drawRectangle.X += (int)(deflection.X * THUMBSTICK_DEFLECTION_AMOUNT);
            drawRectangle.Y -= (int)(deflection.Y * THUMBSTICK_DEFLECTION_AMOUNT);
            drawRectangleRight.X += (int)(deflection.X * THUMBSTICK_DEFLECTION_AMOUNT);
            drawRectangleRight.Y -= (int)(deflection.Y * THUMBSTICK_DEFLECTION_AMOUNT);
            drawRectangleLeft.X += (int)(deflection.X * THUMBSTICK_DEFLECTION_AMOUNT);
            drawRectangleLeft.Y -= (int)(deflection.Y * THUMBSTICK_DEFLECTION_AMOUNT);

            // get direction for blackbeard
            if (deflection.X > 0)
            {
                blackbeardDirection = BlackbeardDirection.Right;
                facingRight = true;
            }
            else if (deflection.X < 0)
            {
                blackbeardDirection = BlackbeardDirection.Left;
                facingRight = false;
            }
            else
            {
                blackbeardDirection = BlackbeardDirection.Front;
            }

            // clamp blackbeard to screen
            KeepInScreen(windowWidth, windowHeight, mapGroundHeight);

            // change weapon on x button
            if (gamePad.IsButtonUp(Buttons.X) &&
                xButtonPreviouslyPressed)
            {
                // no weapon to sword
                if (currentWeapon == CurrentWeapon.None &&
                    !weaponIsChanged)
                {
                    // change weapon
                    currentWeapon = CurrentWeapon.Sword;
                    weaponIsChanged = true;

                    // play audio
                    soundBank.PlayCue("equipSword");
                }

                // sword to pistol
                if (currentWeapon == CurrentWeapon.Sword &&
                    !weaponIsChanged)
                {
                    // change weapon
                    currentWeapon = CurrentWeapon.Pistol;
                    weaponIsChanged = true;

                    // play audio
                    soundBank.PlayCue("equipGun");
                }

                // pistol to no weapon
                if (currentWeapon == CurrentWeapon.Pistol &&
                    !weaponIsChanged)
                {
                    // change weapon
                    currentWeapon = CurrentWeapon.None;
                    weaponIsChanged = true;

                    // play audio
                    soundBank.PlayCue("equipNone");
                }
            }
            weaponIsChanged = false;
            xButtonPreviouslyPressed = gamePad.IsButtonDown(Buttons.X);

            // have weapons follow blackbeard
            if (blackbeardDirection == BlackbeardDirection.Front)
            {
                if (facingRight)
                {
                    drawRectangleGun.X = drawRectangle.Right - drawRectangleGun.Width / 4;
                    drawRectangleGun.Y = drawRectangle.Y + drawRectangle.Height / 2;
                    drawRectangleSword.X = drawRectangle.Right - drawRectangleSword.Width / 3;
                    drawRectangleSword.Y = drawRectangle.Y + drawRectangle.Height / 4;
                }

                else if (!facingRight)
                {
                    drawRectangleGun.X = drawRectangle.Left - gunTextureLeft.Width / 2 - gunTextureLeft.Width / 4;
                    drawRectangleGun.Y = drawRectangle.Y + drawRectangle.Height / 2;
                    drawRectangleGunLeft.X = drawRectangle.Left - gunTextureLeft.Width / 2 - gunTextureLeft.Width / 4;
                    drawRectangleGunLeft.Y = drawRectangle.Y + drawRectangle.Height / 2;
                    drawRectangleSword.X = drawRectangle.Left - drawRectangle.Height / 4;
                    drawRectangleSword.Y = drawRectangle.Y + drawRectangle.Height / 4;
                    drawRectangleSwordLeft.X = drawRectangle.Left - drawRectangle.Height / 4;
                    drawRectangleSwordLeft.Y = drawRectangle.Y + drawRectangle.Height / 4;
                }
            }
            else if (blackbeardDirection == BlackbeardDirection.Right)
            {
                drawRectangleGun.X = drawRectangleRight.Right;
                drawRectangleGun.Y = drawRectangleRight.Y + drawRectangleRight.Height / 2;
                drawRectangleSword.X = drawRectangleRight.Right - drawRectangleSword.Width / 3;
                drawRectangleSword.Y = drawRectangleRight.Y + drawRectangleRight.Height / 4;
            }
            else if (blackbeardDirection == BlackbeardDirection.Left)
            {
                drawRectangleGun.X = drawRectangleLeft.Left - gunTextureLeft.Width;
                drawRectangleGun.Y = drawRectangleLeft.Y + drawRectangleLeft.Height / 2;
                drawRectangleGunLeft.X = drawRectangleLeft.Left - gunTextureLeft.Width;
                drawRectangleGunLeft.Y = drawRectangleLeft.Y + drawRectangleLeft.Height / 2;
                drawRectangleSword.X = drawRectangleLeft.Right - drawRectangleSword.Width / 3;
                drawRectangleSword.Y = drawRectangleLeft.Y + drawRectangleLeft.Height / 4;
                drawRectangleSwordLeft.X = drawRectangleLeft.Left - drawRectangleSwordLeft.Width / 3 - swordTextureLeft.Width / 3;
                drawRectangleSwordLeft.Y = drawRectangleLeft.Y + drawRectangleLeft.Height / 4;
            }
        }

        /// <summary>
        /// Keeps blackbeard's position constant to the screen change
        /// </summary>
        /// <param name="changeDirection">"left" or "right" change of screen, "enterHouse" or "leaveHouse" change of screen</param>
        public void ScreenChange(string changeDirection)
        {
            // changes screen if blackbeard is on the right
            if (changeDirection == "right")
            {
                drawRectangle.X = 0;
                drawRectangleRight.X = 0;
                drawRectangleLeft.X = 0;
            }

            // changes screen if blackbeard is on the left
            else if (changeDirection == "left")
            {
                drawRectangle.X = windowWidth - sprite.Width;
                drawRectangleRight.X = windowWidth - spriteRight.Width;
                drawRectangleLeft.X = windowWidth - spriteLeft.Width;
            }

            // changes screen if blackbeard is going into house
            else if (changeDirection == "enterHouse")
            {
                drawRectangle.X = windowWidth / 20 + 7;
                drawRectangleRight.X = windowWidth / 20 + 7;
                drawRectangleLeft.X = windowWidth / 20 + 7;

                drawRectangle.Y = 425 - sprite.Height;
                drawRectangleRight.Y = 425 - spriteRight.Height;
                drawRectangleLeft.Y = 425 - spriteLeft.Height;
            }
            
            // changes screen if blackbeard is leaving house
            else if (changeDirection == "leaveHouse")
            {
                drawRectangle.X = 1153 - sprite.Width + 13;
                drawRectangleRight.X = 1153 - spriteRight.Width + 13;
                drawRectangleLeft.X = 1153 - spriteLeft.Width + 13;

                drawRectangle.Y = 263 - sprite.Height;
                drawRectangleRight.Y = 263 - spriteRight.Height;
                drawRectangleLeft.Y = 263 - spriteLeft.Height;
            }

            // returns null if incorrect
            else
            {
                changeDirection = null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the window height variable for blackbeard
        /// </summary>
        public int WindowHeight
        {
            get { return windowHeight; }
            set { windowHeight = value; }
        }
        
        /// <summary>
        /// Gets and sets the map ground height
        /// </summary>
        public int MapGroundHeight
        {
            get { return mapGroundHeight; }
            set { mapGroundHeight = value; }
        }

        /// <summary>
        /// Gets the face side of blackbeard
        /// </summary>
        public bool FacingRight
        {
            get { return facingRight; }
        }

        /// <summary>
        /// Gets the direction of blackbeard
        /// </summary>
        public BlackbeardDirection BlackbeardDirection
        {
            get { return blackbeardDirection; }
        }

        /// <summary>
        /// Get the draw rectangle for blackbeard's gun
        /// </summary>
        public Rectangle GunRectangle
        {
            get
            {
                if (blackbeardDirection == BlackbeardDirection.Right)
                {
                    return drawRectangleGun;
                }
                else if (blackbeardDirection == BlackbeardDirection.Left)
                {
                    return drawRectangleGunLeft;
                }
                else if (blackbeardDirection == BlackbeardDirection.Front)
                {
                    if (facingRight)
                    {
                        return drawRectangleGun;
                    }
                    else if (!facingRight)
                    {
                        return drawRectangleGunLeft;
                    }
                    else
                    {
                        return drawRectangleGun;
                    }
                }
                else
                {
                    return drawRectangleGun;
                }
            }
        }

        /// <summary>
        /// Get the draw rectangle for blackbeard's sword
        /// </summary>
        public Rectangle SwordRectangle
        {
            get
            {
                if (blackbeardDirection == BlackbeardDirection.Right)
                {
                    return drawRectangleSword;
                }
                else if (blackbeardDirection == BlackbeardDirection.Left)
                {
                    return drawRectangleSwordLeft;
                }
                else if (blackbeardDirection == BlackbeardDirection.Front)
                {
                    if (facingRight)
                    {
                        return drawRectangleSword;
                    }
                    else if (!facingRight)
                    {
                        return drawRectangleSwordLeft;
                    }
                    else
                    {
                        return drawRectangleSword;
                    }
                }
                else
                {
                    return drawRectangleSword;
                }
            }
        }

        /// <summary>
        /// Gets the currently equipped weapon of blackbeard
        /// </summary>
        public CurrentWeapon EquippedWeapon
        {
            get { return currentWeapon; }
        }

        /// <summary>
        /// Gets the collision rectangle of blackbeard
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get {
                if (blackbeardDirection == BlackbeardDirection.Front)
                {
                    return drawRectangle;
                }
                else if (blackbeardDirection == BlackbeardDirection.Right)
                {
                    return drawRectangleRight;
                }
                else if (blackbeardDirection == BlackbeardDirection.Left)
                {
                    return drawRectangleLeft;
                }
                else
                {
                    return drawRectangle;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads blackbeard
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="spriteName">the sprite name for blackbeard</param>
        /// <param name="x">the x location</param>
        /// <param name="y">the y location</param>
        private void LoadContent(ContentManager contentManager, string spriteName, int x, int y)
        {
            // load blackbeard
            sprite = contentManager.Load<Texture2D>(spriteName);

            // construct draw rectangle
            drawRectangle = new Rectangle(x, y, sprite.Width, sprite.Height);

            // load gun and sword
            gunTexture = contentManager.Load<Texture2D>("gun");
            gunTextureLeft = contentManager.Load<Texture2D>("gunleft");
            swordTexture = contentManager.Load<Texture2D>("sword");
            swordTextureLeft = contentManager.Load<Texture2D>("swordleft");

            // construct draw rectangles
            drawRectangleGun = new Rectangle(x, y, gunTexture.Width, gunTexture.Height);
            drawRectangleGunLeft = new Rectangle(x, y, gunTextureLeft.Width, gunTextureLeft.Height);
            drawRectangleSword = new Rectangle(x, y, swordTexture.Width, swordTexture.Height);
            drawRectangleSwordLeft = new Rectangle(x, y, swordTextureLeft.Width, swordTextureLeft.Height);

            // load directions for blackbeard
            spriteRight = contentManager.Load<Texture2D>("blackbeardright");
            spriteLeft = contentManager.Load<Texture2D>("blackbeardleft");

            // construct draw rectangles
            drawRectangleRight = new Rectangle(x, y, spriteRight.Width, spriteRight.Height);
            drawRectangleLeft = new Rectangle(x, y, spriteLeft.Width, spriteLeft.Height);
        }

        /// <summary>
        /// Keeps blackbeard within the screen
        /// </summary>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        /// <param name="mapGroundHeight">the map ground height from the top of the screen</param>
        private void KeepInScreen(int windowWidth, int windowHeight, int mapGroundHeight)
        {
            // keep FRONT blackbeard in screen
            //y-axis boundaries
            if (drawRectangle.Top < mapGroundHeight)
            {
                drawRectangle.Y = mapGroundHeight;
            }
            if (drawRectangle.Bottom > windowHeight)
            {
                drawRectangle.Y = windowHeight - sprite.Height;
            }
            // x-axis boundaries
            if (drawRectangle.Left < 0)
            {
                drawRectangle.X = 0;
            }
            if (drawRectangle.Right > windowWidth)
            {
                drawRectangle.X = windowWidth - sprite.Width;
            }

            // keep RIGHT blackbeard in screen
            //y-axis boundaries
            if (drawRectangleRight.Top < mapGroundHeight)
            {
                drawRectangleRight.Y = mapGroundHeight;
            }
            if (drawRectangleRight.Bottom > windowHeight)
            {
                drawRectangleRight.Y = windowHeight - spriteRight.Height;
            }

            // x-axis boundaries
            if (drawRectangleRight.Left < 0)
            {
                drawRectangleRight.X = 0;
            }
            if (drawRectangleRight.Right > windowWidth)
            {
                drawRectangleRight.X = windowWidth - spriteRight.Width;
            }

            // keep LEFT blackbeard in screen
            //y-axis boundaries
            if (drawRectangleLeft.Top < mapGroundHeight)
            {
                drawRectangleLeft.Y = mapGroundHeight;
            }
            if (drawRectangleLeft.Bottom > windowHeight)
            {
                drawRectangleLeft.Y = windowHeight - spriteLeft.Height;
            }

            // x-axis boundaries
            if (drawRectangleLeft.Left < 0)
            {
                drawRectangleLeft.X = 0;
            }
            if (drawRectangleLeft.Right > windowWidth)
            {
                drawRectangleLeft.X = windowWidth - spriteLeft.Width;
            }
        }

        #endregion
    }
}
