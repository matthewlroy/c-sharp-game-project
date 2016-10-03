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
    /// The corpse for the scallywag
    /// </summary>
    class ScallywagCorpse
    {
        #region Fields

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;

        // scallywag state support
        bool active = false;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructs the scallywag cropse
        /// </summary>
        /// <param name="sprite">the sprite for the scallywag corpse</param>
        /// <param name="location">the location for the corpse</param>
        public ScallywagCorpse(Texture2D sprite, Vector2 location)
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
        /// Draws the scallywag corpse
        /// </summary>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(sprite, drawRectangle, Color.White);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the active state for the corpse
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets the collision rectangle for the corpse
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        #endregion
    }
}
