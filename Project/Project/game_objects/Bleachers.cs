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
    public class Bleachers : CollidableGameObject
    {
        private Quad quad;
        private Texture2D[] frames;
        private int currentFrame;
        private float quadWidthScale = 5;
        private float quadHeightScale = 5;
        private int animationInterval;
        private int elapsed;

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

        public Bleachers(Renderer3D renderer)
            : base(renderer)
        {
            animationInterval = 100;
            frames = new Texture2D[3];
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            base.ImediateTranslate(amount);
            quad.Translate(amount);

            boundingBox.Min += amount;
            boundingBox.Max += amount;

        }

        public override void Load(ContentManager cManager)
        {
            for (int i = 0; i < frames.Length; i++)
                frames[i] =
                    cManager.Load<Texture2D>(
                    "Imagem" + Path.AltDirectorySeparatorChar +
                    "Props" + Path.AltDirectorySeparatorChar +
                    "Arquibancada" + Path.AltDirectorySeparatorChar + (i + 1));
            quadHeightScale *= ((float)frames[0].Height/frames[0].Width);
            createQuad();
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(frames[currentFrame], quad, BlendState.AlphaBlend, BoundingBox);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateFrame(gameTime);
        }

        private void UpdateFrame(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed >= animationInterval)
            {
                elapsed = 0;
                currentFrame++;
                if (currentFrame >= frames.Length)
                    currentFrame = 0;
            }
        }

        private void createQuad()
        {
            quad = new Quad(position + new Vector3(0, quadHeightScale / 2, 0), new Vector3(0, 0, -1), Vector3.Up, quadWidthScale, quadHeightScale);

            Vector3 backUpperLeft = quad.Vertices[1].Position;

            Vector3 frontBottomRight = quad.Vertices[2].Position;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }
    }
}
