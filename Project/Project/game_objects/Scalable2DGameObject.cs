using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game_objects
{
    public class Scalable2DGameObject : DrawableGameObject
    {
        private Rectangle bounds;

        protected Texture2D texture;

        private string textureFileName;
        private string textureFilePath;

        private Color color;

        private float colorModifier;

        public string TextureFileName
        {
            get { return textureFileName; }
            set { textureFileName = value; }
        }

        public string TextureFilePath
        {
            get { return textureFilePath; }
            set { textureFilePath = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public float ColorModifier
        {
            get { return colorModifier; }
            set { colorModifier = value; }
        }

        public override float Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                bounds.Width = (int)value;
            }
        }

        public override float Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
                bounds.Height = (int)value;
            }
        }

        public override Vector3 Dimensions
        {
            get
            {
                return base.Dimensions;
            }
            set
            {
                base.Dimensions = value;
                bounds.Width = (int)value.X;
                bounds.Height = (int)value.Y;
            }
        }

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                bounds.X = (int)value.X;
                bounds.Y = (int)value.Y;
            }
        }

        public override float X
        {
            get
            {
                return base.X;
            }
            set
            {
                base.X = value;
                bounds.X = (int)value;
            }
        }

        public override float Y
        {
            get
            {
                return base.Y;
            }
            set
            {
                base.Y = value;
                bounds.Y = (int)value;
            }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }


        public Scalable2DGameObject(Renderer2D r2d)
            : base(r2d)
        {
            this.bounds = Rectangle.Empty;
            color = Color.White;
            colorModifier = 1;
        }

        public override void Load(ContentManager cManager)
        {
            if (!string.IsNullOrEmpty(textureFileName) && !string.IsNullOrEmpty(textureFilePath))
            {
                texture = cManager.Load<Texture2D>(textureFilePath + textureFileName);
                Width = texture.Width;
                Height = texture.Height;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer2D)Renderer).Draw(texture, Bounds, Color*colorModifier, BlendState.AlphaBlend);
        }
    }
}
