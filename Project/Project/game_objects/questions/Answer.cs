using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Project;
using Microsoft.Xna.Framework.Graphics;

namespace game_objects.questions
{
    public class Answer : CollidableGameObject
    {
        private string text;
        private Quad quad;

        private const float scale = 0.5f;
        private Texture2D texture;

        private bool textureLoaded;

        private Quad Quad
        {
            get {
                if (quad == null)
                    CreateQuad();
                return quad;
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
                CreateQuad();
            }
        }

        public Answer(Renderer3D renderer, string text)
            : base(renderer)
        {
            this.text = text;
        }

        private void CreateQuad()
        {
            Vector3 nrm = new Vector3(0, 0, -1);
            float proportion = (float)texture.Width / texture.Height;
            quad = new Quad(position, nrm, Vector3.Up, scale * proportion, scale);
            Vector3 test = new Vector3((1 - scale*proportion)/2, 0, 0);
            Vector3 backUpperLeft = Quad.Vertices[1].Position + test;

            Vector3 frontBottomRight = Quad.Vertices[2].Position - test;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

        public override void Translate(Vector3 amount)
        {
            base.Translate(amount);
            Quad.Translate(amount);
            boundingBox.Max += amount;
            boundingBox.Min += amount;
        }

        public override void Load(ContentManager cManager)
        {
            textureLoaded = TextureHelper.StringToTexture(text, out texture);
        }

        public override void Draw(GameTime gameTime)
        {
            if(textureLoaded)
                ((Renderer3D)Renderer).Draw(gameTime, texture, Quad, BlendState.AlphaBlend);
        }
    }
}
