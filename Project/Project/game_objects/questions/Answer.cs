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

        private const float scale = 0.5f;
        private Quad quad;
        private Texture2D texture;

        public string Text
        {
            get { return text; }
            set { 
                text = value;
                CreateQuad();
            }
        }

        public Texture2D Texture
        {
            get { 
                if(!textureLoaded)
                    textureLoaded = TextHelper.StringToTexture(Text, out texture);
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
                if (value.Y > 0)
                {
                    VariableMovementComponent vmc = GetComponent<VariableMovementComponent>();
                    vmc.CurrentVelocity = Vector3.Down / 1000 * (float)PublicRandom.NextDouble(0.05f);
                    vmc.Acceleration = Vector3.Down / 10;
                }
                CreateQuad();
            }
        }


        public Answer(Renderer3D renderer, IEnumerable<CollidableGameObject> collidableObjects, string text)
            : base(renderer, collidableObjects)
        {
            this.text = text;
            //"gravidade"
            VariableMovementComponent vmc = 
                new VariableMovementComponent(this, 30,
                    Vector3.Down / 1000 * (float)PublicRandom.NextDouble(0.01f), 
                    Vector3.Down / 100);
            vmc.LowerVelocityThreshold = Vector3.Down / 4;
            addComponent(vmc);
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            base.ImediateTranslate(amount);
            boundingBox.Max += amount;
            boundingBox.Min += amount;
            Vector3 upAmount = Vector3.Zero;
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
                        VariableMovementComponent vmc = GetComponent<VariableMovementComponent>();
                        vmc.Acceleration = Vector3.Zero;
                        vmc.CurrentVelocity = Vector3.Zero;
                        vmc.AccelerationVariation = Vector3.Zero;
                        amount += upAmount;
                    }
                    break;
                }
            }
            Quad.Translate(amount);            
        }


        public override void Load(ContentManager cManager)
        {
            textureLoaded = TextHelper.StringToTexture(Text, out texture);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                TextHelper.StringToTexture(Text, out texture);
                if (textureLoaded)
                    ((Renderer3D)Renderer).Draw(texture, Quad, BlendState.AlphaBlend, BoundingBox);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                base.Update(gameTime);
            }
        }
        public override bool Collided(CollidableGameObject obj)
        {
            return base.Collided(obj) && GetComponent<VariableMovementComponent>().CurrentVelocity == Vector3.Zero;
        }

        private void CreateQuad()
        {
            Vector3 nrm = new Vector3(0, 0, -1);
            float proportion = (float)Texture.Width / Texture.Height;
            quad = new Quad(position, nrm, Vector3.Up, scale * proportion, scale);

            Vector3 backUpperLeft = Quad.Vertices[1].Position;

            Vector3 frontBottomRight = Quad.Vertices[2].Position;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

    }
}
