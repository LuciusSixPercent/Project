using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace game_objects
{
    public class Goal : CollidableGameObject
    {
        private Quad quad;
        Texture2D texture;
        private float quadWidthScale = 1.5f;
        private float quadHeightScale = 1.5f;

        public Goal(Renderer3D renderer)
            : base(renderer)
        {
            Visible = false;
        }

        public override void Load(ContentManager cManager)
        {
            string path = "Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "Bate_Bola" + Path.AltDirectorySeparatorChar;
            texture = cManager.Load<Texture2D>(path+"tmp_goal");
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

            BoundingBox = new BoundingBox(quad.LowerLeft, quad.UpperRight);
        }
    }
}
