using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Project;
using Microsoft.Xna.Framework.Graphics;
using components;

namespace game_objects.questions
{
    public class Answer : CollidableGameObject
    {
        private string text;
        private Quad quad;

        private const float scale = 0.5f;
        private Texture2D texture;

        public Texture2D Texture
        {
            get { 
                if(!textureLoaded)
                    textureLoaded = TextureHelper.StringToTexture(text, out texture);
                return texture; 
            }
        }

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

        public Answer(Renderer3D renderer, List<CollidableGameObject> collidableObjects, string text)
            : base(renderer, collidableObjects)
        {
            this.text = text;
            //"gravidade"
            VariableMovementComponent vmc = new VariableMovementComponent(this, 30, Vector3.Down / 100 * (float)PublicRandom.NextDouble(), Vector3.Down / 20 * (float)PublicRandom.NextDouble(0.5));
            vmc.TerminalVelocity = Vector3.Down/10;
            addComponent(vmc);
        }

        private void CreateQuad()
        {
            Vector3 nrm = new Vector3(0, 0, -1);
            float proportion = (float)Texture.Width / Texture.Height;
            quad = new Quad(position, nrm, Vector3.Up, scale * proportion, scale);

            Vector3 scaleVector = new Vector3((1 - scale*proportion)/2, 0, 0);

            Vector3 backUpperLeft = Quad.Vertices[1].Position + scaleVector;

            Vector3 frontBottomRight = Quad.Vertices[2].Position - scaleVector;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            base.ImediateTranslate(amount);
            boundingBox.Max += amount;
            boundingBox.Min += amount;
            Vector3 upAmount = new Vector3(0,0.1f,0);
            foreach (CollidableGameObject cgo in CollidableObjects)
            {
                if (cgo is Field)
                {
                    while(cgo.Collided(this))
                    {
                        upAmount = cgo.Position - boundingBox.Min;
                        upAmount *= Vector3.Up;
                        base.ImediateTranslate(upAmount); 
                        boundingBox.Max += upAmount;
                        boundingBox.Min += upAmount;
                        amount += upAmount;
                    }
                }
            }
            Quad.Translate(amount);            
        }


        public override void Load(ContentManager cManager)
        {
            textureLoaded = TextureHelper.StringToTexture(text, out texture);
        }

        public override void Draw(GameTime gameTime)
        {
            TextureHelper.StringToTexture(text, out texture);
            if(textureLoaded)
                ((Renderer3D)Renderer).Draw(gameTime, texture, Quad, BlendState.AlphaBlend);
        }
    }
}
