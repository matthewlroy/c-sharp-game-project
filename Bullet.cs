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
    /// The bullet for the pistol
    /// </summary>
    class Bullet
    {
        #region Fields

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;

        // movement support
        const int BULLET_MOVEMENT_SPEED = 12;

        // bullet state support
        bool active = false;
        string direction = "right";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a bullet
        /// </summary>
        /// <param name="sprite">the sprite of bullet</param>
        /// <param name="location">the location of the bullet</param>
        public Bullet(Texture2D sprite, Vector2 location)
        {
            // variable support
            this.sprite = sprite;

            // construct draw rectangle
            drawRectangle = new Rectangle(
                (int)(location.X - sprite.Width / 2),
                (int)(location.Y - sprite.Height / 2),
                sprite.Width, sprite.Height);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws the bullet
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
        /// Updates the bullet
        /// </summary>
        public void Update()
        {
            if (direction == "right")
            {
                drawRectangle.X += BULLET_MOVEMENT_SPEED;
            }
            else if (direction == "left")
            {
                drawRectangle.X -= BULLET_MOVEMENT_SPEED;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the direction of the bullet
        /// </summary>
        public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Gets and sets the active state for the bullet
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets the collision rectangle of the bullet
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        #endregion
    }
}
