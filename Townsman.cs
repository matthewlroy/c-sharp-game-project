﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// extra references
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Blackbeard
{
    /// <summary>
    /// The townsman for blackbeard
    /// </summary>
    class Townsman
    {
        #region Fields

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;

        // resolution support
        int windowWidth;
        int windowHeight;
        int mapGroundHeight;

        // movement support
        int movementX;
        int movementY;

        // random support
        Random rand = new Random();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a townsman
        /// </summary>
        /// <param name="sprite">the sprite for the townsman</param>
        /// <param name="location">the center location for the townsman</param>
        public Townsman(Texture2D sprite, Vector2 location)
        {
            // variable support
            this.sprite = sprite;

            // construct draw rectangle
            drawRectangle = new Rectangle(
                (int)(location.X - sprite.Width / 2),
                (int)(location.Y - sprite.Height / 2),
                sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Constructs a townsman
        /// </summary>
        /// <param name="sprite">the sprite for the townsman</param>
        /// <param name="location">the center location for the townsman</param>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        public Townsman(Texture2D sprite, Vector2 location, int windowWidth, int windowHeight)
        {
            // store resolution variables
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            // variable support
            this.sprite = sprite;

            // construct draw rectangle
            drawRectangle = new Rectangle(
                (int)(location.X - sprite.Width / 2),
                (int)(location.Y - sprite.Height / 2),
                sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Constructs a townsman
        /// </summary>
        /// <param name="sprite">the sprite for the townsman</param>
        /// <param name="location">the center location for the townsman</param>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        /// <param name="mapGroundHeight">the height of the ground from the top of the screen</param>
        public Townsman(Texture2D sprite, Vector2 location, int windowWidth, int windowHeight, int mapGroundHeight)
        {
            // store resolution variables
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.mapGroundHeight = mapGroundHeight;

            // variable support
            this.sprite = sprite;

            // movement support
            while (movementX == 0 ||
                movementY == 0)
            {
                movementX = rand.Next(-1, 1);
                movementY = rand.Next(-1, 1);
            }

            // construct draw rectangle
            drawRectangle = new Rectangle(
                (int)(location.X - sprite.Width / 2),
                (int)(location.Y - sprite.Height / 2),
                sprite.Width, sprite.Height);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draw the townsman
        /// </summary>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        /// <summary>
        /// Updates the townsman
        /// </summary>
        public void Update()
        {
            // gives direction to the townsman
            drawRectangle.X += movementX;

            // change movement randomly
            int randomNumber = rand.Next(0, 120);
            if (randomNumber == 3)
            {
                movementX *= -1;
            }

            // keep townsman in screen
            BounceOffWalls(windowWidth, windowHeight, mapGroundHeight);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Keeps the townsman in the screen and bounces off the walls
        /// </summary>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        /// <param name="mapGroundHeight">the height of the map from the top of the screen</param>
        private void BounceOffWalls(int windowWidth, int windowHeight, int mapGroundHeight)
        {
            // left and right
            if (drawRectangle.Left < 0)
            {
                movementX *= -1;
            }
            if (drawRectangle.Right > windowWidth)
            {
                movementX *= -1;
            }

            // top and bottom
            if (drawRectangle.Top < mapGroundHeight)
            {
                movementY *= -1;
            }
            if (drawRectangle.Bottom > windowHeight)
            {
                movementY *= -1;
            }
        }

        #endregion
    }
}
