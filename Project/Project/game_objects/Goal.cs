using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Graphics;

namespace game_objects
{
    public class Goal : CollidableGameObject
    {
        private Quad quad;
        Texture2D texture;
        private float quadWidthScale = 2f;
        private float quadHeightScale = 2f;

        public Goal(Renderer3D renderer, List<CollidableGameObject> collidableObjects)
            : base(renderer, collidableObjects)
        {
            Visible = false;
        }

        public override void Load(ContentManager cManager)
        {
            texture = cManager.Load<Texture2D>("tmp_goal");
            quadHeightScale *= ((float)texture.Height / texture.Width);
            createQuad();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!Visible)
            {
                BoundingFrustum bf = new BoundingFrustum(((Renderer3D)Renderer).Cam.View * ((Renderer3D)Renderer).Cam.Projection);
                ContainmentType containment = bf.Contains(BoundingBox);
                if (containment != ContainmentType.Disjoint)
                {
                    Visible = true;
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            if (Visible)
                ((Renderer3D)Renderer).Draw(texture, quad, BlendState.AlphaBlend, BoundingBox);
        }

        public override bool Collided(CollidableGameObject obj)
        {
            return base.Collided(obj);
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
                createQuad();
            }
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            base.ImediateTranslate(amount);
            quad.Translate(amount);
            boundingBox.Min += amount;
            boundingBox.Max += amount;
        }

        private void createQuad()
        {
            quad = new Quad(position + new Vector3(0, quadHeightScale/2, 0), new Vector3(0, 0, -1), Vector3.Up, quadWidthScale, quadHeightScale);
            Vector3 backUpperLeft = quad.Vertices[1].Position;

            Vector3 frontBottomRight = quad.Vertices[2].Position;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }
    }
}
