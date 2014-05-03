using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project;

namespace game_objects
{
    class Prop : DrawableGameObject
    {
        private Texture2D texture;
        private Quad quad;

        float baseScale;
        float widthScale;
        float heightScale;

        BoundingBox bbox;

        public float BaseScale
        {
            get { return baseScale; }
            set { baseScale = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set {
                if (texture != value)
                {
                    widthScale = heightScale = baseScale;
                    texture = value;
                    if (texture.Height > texture.Width)
                    {
                        widthScale *= (float)texture.Width / texture.Height;
                    }
                    else
                    {
                        heightScale *= (float)texture.Height / texture.Width;
                    }
                }
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
                bbox = new BoundingBox(position, position);
                CreateQuad();
            }
        }

        private void CreateQuad()
        {
            Vector3 origin = position;
            origin.Y += heightScale/2;

            quad = new Quad(origin, Vector3.Forward, Vector3.Up, widthScale, heightScale);
        }

        public Prop(Renderer3D renderer, float baseScale)
            : base(renderer)
        {
            this.baseScale = baseScale;
        }

        public override void Load(ContentManager cManager)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(texture, quad, BlendState.AlphaBlend, bbox);
        }
    }
}
