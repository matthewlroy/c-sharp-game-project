using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// extra references
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Blackbeard
{
    /// <summary>
    /// The main enemy scallywag for blackbeard
    /// </summary>
    class Scallywag
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

        // scallywag state support
        bool active = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs the scallywag
        /// </summary>
        /// <param name="sprite">the sprite for scallywag</param>
        /// <param name="location">the location of the scallywag</param>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        public Scallywag(Texture2D sprite, Vector2 location, int windowWidth, int windowHeight)
        {
            // store resolution variables
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            // variable support
            this.sprite = sprite;

            // movement support
            while (movementX == 0 ||
                movementY == 0)
            {
                movementX = rand.Next(-5, 5);
                movementY = rand.Next(-3, 3);
            }

            // construct draw rectangle
            drawRectangle = new Rectangle(
                (int)(location.X - sprite.Width / 2),
                (int)(location.Y - sprite.Height / 2),
                sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Constructs the scallywag
        /// </summary>
        /// <param name="sprite">the sprite for scallywag</param>
        /// <param name="location">the location of the scallywag</param>
        /// <param name="windowWidth">the width of the window</param>
        /// <param name="windowHeight">the height of the window</param>
        /// <param name="mapGroundHeight">the height of the map from the top of the screen</param>
        public Scallywag(Texture2D sprite, Vector2 location, int windowWidth, int windowHeight, int mapGroundHeight)
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
                movementX = rand.Next(-5, 5);
                movementY = rand.Next(-3, 3);
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
        /// Draws the scallywag
        /// </summary>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(sprite, drawRectangle, Color.White);
            }
        }

        /// <summary>
        /// Updates the scallywag
        /// </summary>
        public void Update()
        {
            // gives direction to the scallywag
            drawRectangle.X += movementX;
            drawRectangle.Y += movementY;

            // keep scallywag in screen
            BounceOffWalls(windowWidth, windowHeight, mapGroundHeight);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the active state for the scallywag
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets the collision rectangle of the scallywag
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Keeps the scallywag in the screen and bounces off the walls
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
