//Zeplin Engine - Sprite.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/
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

namespace Zeplin
{
    /// <summary>
    /// Defines a texture resource, animation data, and drawing instructions. Wraps a Texture2D.
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// Constructs a Sprite from the specified Texture2D resource
        /// </summary>
        /// <param name="resource">The name of a Texture2D resource</param>
        public Sprite(string resource)
        {
            image = Engine.Content.Load<Texture2D>(resource);
            Color = Color.White;

            FrameSize.X = image.Width;
            FrameSize.Y = image.Height;
        }

        public Sprite(Texture2D texture)
        {
            image = texture;
            Color = Color.White;

            FrameSize.X = image.Width;
            FrameSize.Y = image.Height;
        }

        /// <summary>
        /// Gets the center point of the sprite's frame
        /// </summary>
        /// <returns>The vector point of the sprite frame's center point in texture space.</returns>
        public Vector2 Center
        {
            get { return new Vector2(FrameSize.X / 2, FrameSize.Y / 2); }
        }

        /// <summary>
        /// Gets or sets the opacity of the sprite
        /// </summary>
        public float Opacity
        {
            get { return Color.A; }
            set 
            {
                //clamp between 0 and 1
                if (value > 1) value = 1;
                else if (value < 0) value = 0;

                color.A = (byte)(value * 256);
            }
        }

        /// <summary>
        /// Draws this sprite using the given transformation and depth
        /// </summary>
        /// <param name="transformation">The transformation to apply to the sprite</param>
        /// <remarks>If on a layer, depth only applies within that layer.</remarks>
        internal void Draw(Transformation transformation)
        {
            Draw(transformation, null);
        }

        /// <summary>
        /// Draws this sprite using the given transformation and depth
        /// </summary>
        /// <param name="transformation">The transformation to apply to the sprite</param>
        /// <param name="depth">A depth value, from 0.0 (back) to 1.0 (front).</param>
        /// <param name="sourceRectangle">Subrectangle on the sprite generated by ProcessAnimation. Contains a single frame of the sprite sheet.</param>
        /// <remarks>If on a layer, depth only applies within that layer.</remarks>
        internal void Draw(Transformation transformation, Rectangle? sourceRectangle)
        {
            if (sourceRectangle.HasValue)
            {
                Rectangle texelRect = sourceRectangle.Value;

                ZeplinGame.spriteBatch.Draw(image, new Vector2(transformation.Position.X, -transformation.Position.Y), texelRect, 
                    color, transformation.Rotation, transformation.Pivot, transformation.Scale, Facing, transformation.Depth);
            }
            else
            {
                ZeplinGame.spriteBatch.Draw(image, new Vector2(transformation.Position.X, -transformation.Position.Y), null, 
                    color, transformation.Rotation, transformation.Pivot, transformation.Scale, Facing, transformation.Depth);
            }
        }

        /// <summary>
        /// Allows the sprite to be flipped horizontally or vertically.
        /// </summary>
        /// <remarks>Hacked in late. This will be changed to a property and wrapped into Transformation and not require importing XNA.Graphics to set.</remarks>
        public SpriteEffects Facing { get; set; }

        Texture2D image;
        /// <summary>
        /// Gets a reference to the Texture2D component of the Sprite.
        /// </summary>
        /// <remarks>Not certain this is needed public</remarks>
        public Texture2D Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }


        private Color color;
        /// <summary>
        /// Gets or sets the color tinting of the sprite
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        
        
        /// <summary>
        /// Gets or sets the dimensions of a frame's size on the sprite
        /// </summary>
        /// <remarks>This is used for animations that are composed of smaller images within the texture, known either as sprite sheet or filmstrip animation.</remarks>
        public Point FrameSize;
    }
}
