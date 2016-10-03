using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// extra references
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Blackbeard
{
    /// <summary>
    /// The weapon HUD for blackbeard
    /// </summary>
    class WeaponHud
    {
        #region Fields

        // drawing support
        Texture2D weaponHud;
        Texture2D weaponHudNone;
        Texture2D weaponHudSword;
        Texture2D weaponHudPistol;
        Rectangle drawRectangle;

        // weapon support
        CurrentWeapon currentWeapon = CurrentWeapon.None;
        bool weaponIsChanged = false;

        // button support
        bool xButtonPreviouslyPressed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs the weapon hud
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="x">the x location</param>
        /// <param name="y">the y location</param>
        public WeaponHud(ContentManager contentManager, int x, int y)
        {
            // load content
            LoadContent(contentManager, x, y);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws the weapon hud
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // get the current weapon
            if (currentWeapon == CurrentWeapon.None)
            {
                spriteBatch.Draw(weaponHudNone, drawRectangle, Color.White);
            }
            else if (currentWeapon == CurrentWeapon.Sword)
            {
                spriteBatch.Draw(weaponHudSword, drawRectangle, Color.White);
            }
            else if (currentWeapon == CurrentWeapon.Pistol)
            {
                spriteBatch.Draw(weaponHudPistol, drawRectangle, Color.White);
            }
            else
            {
                // draw the normal hud
                spriteBatch.Draw(weaponHud, drawRectangle, Color.White);
            }
        }

        /// <summary>
        /// Updates the weapon hud
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime, GamePadState gamePad)
        {
            // change weapon on x button
            if (gamePad.IsButtonUp(Buttons.X) &&
                xButtonPreviouslyPressed)
            {
                // no weapon to sword
                if (currentWeapon == CurrentWeapon.None &&
                    !weaponIsChanged)
                {
                    currentWeapon = CurrentWeapon.Sword;
                    weaponIsChanged = true;
                }

                // sword to pistol
                if (currentWeapon == CurrentWeapon.Sword &&
                    !weaponIsChanged)
                {
                    currentWeapon = CurrentWeapon.Pistol;
                    weaponIsChanged = true;
                }

                // pistol to no weapon
                if (currentWeapon == CurrentWeapon.Pistol &&
                    !weaponIsChanged)
                {
                    currentWeapon = CurrentWeapon.None;
                    weaponIsChanged = true;
                }
            }
            weaponIsChanged = false;
            xButtonPreviouslyPressed = gamePad.IsButtonDown(Buttons.X);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the hud
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the weapon hud
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="x">the x location</param>
        /// <param name="y">the y location</param>
        private void LoadContent(ContentManager contentManager, int x, int y)
        {
            // load the textures
            weaponHud = contentManager.Load<Texture2D>("weaponhud");
            weaponHudNone = contentManager.Load<Texture2D>("weaponhudnone");
            weaponHudSword = contentManager.Load<Texture2D>("weaponhudsword");
            weaponHudPistol = contentManager.Load<Texture2D>("weaponhudpistol");

            // construct the draw rectangle
            drawRectangle = new Rectangle(x, y, weaponHud.Width, weaponHud.Height);
        }

        #endregion
    }
}
