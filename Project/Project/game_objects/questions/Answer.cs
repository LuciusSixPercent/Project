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
        private float scale = 1f;

        public Answer(Renderer3D renderer, string text)
            : base(renderer)
        {
            this.text = text;
        }

        private void CreateQuad()
        {
            Vector3 backUpperLeft = quad.Vertices[1].Position;

            Vector3 frontBottomRight = quad.Vertices[2].Position;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

        public override void Translate(Vector3 amount)
        {
            base.Translate(amount);
            quad.Translate(amount);
            boundingBox.Max += amount;
            boundingBox.Min += amount;
        }

        public override void Load(ContentManager cManager)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            //((Renderer3D)Renderer).Draw(gameTime, TextureHelper.StringToTexture(texte), quad, BlendState.AlphaBlend);
        }
    }
}
