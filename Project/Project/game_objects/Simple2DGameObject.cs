using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game_objects
{
    public class Simple2DGameObject : DrawableGameObject
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

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }


        public Simple2DGameObject(Renderer2D r2d)
            : base(r2d)
        {
            this.bounds = Rectangle.Empty;
        }

        public override void Load(ContentManager cManager)
        {
            if (!string.IsNullOrEmpty(textureFileName) && !string.IsNullOrEmpty(textureFilePath))
            {
                texture = cManager.Load<Texture2D>(textureFilePath + textureFileName);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer2D)Renderer).Draw(texture, Bounds, Color*colorModifier, BlendState.AlphaBlend);
        }
    }
}
