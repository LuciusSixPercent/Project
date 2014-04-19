using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using components;
using Project;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace game_objects
{
    public class Character : DrawableGameObject
    {
        private Quad quad;
        private float quadWidthScale = 1f;
        private float quadHeightScale = 1f;
        private const float MAX_X = 2f;
        private const float MIN_X = -2f;
        private Texture2D spriteSheet;

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set
            {
                spriteSheet = value;
                if (spriteSheet != null)
                {
                    quadWidthScale *= ((float)spriteSheet.Width / spriteSheet.Height);
                }
            }
        }
        private int currentFrame;
        private string name;
        private GameObjectsManager oManager;

        public Quad Sprite
        {
            get
            {
                if (quad == null)
                    createQuad();
                return quad;
            }
        }

        public Character(GameObjectsManager oManager, Renderer3D renderer, string name)
            : base(renderer)
        {
            PlayerMovementComponent pmc = new PlayerMovementComponent(this, 15, 0.2f, MAX_X);
            addComponent(pmc);

            ConstantMovementComponent cmc = new ConstantMovementComponent(this, new Vector3(0, 0, 0.1f), 10);
            addComponent(cmc);

            currentFrame = 0;

            this.oManager = oManager;

            this.name = name;
        }

        public override void Translate(Vector3 amount)
        {
            float newX = Position.X + amount.X;
            if (newX > MAX_X)
                amount.X -= newX - MAX_X;
            else if (newX < MIN_X)
                amount.X += MIN_X - newX;
            base.Translate(amount);
            quad.translate(amount);
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

        private void createQuad()
        {
            quad = new Quad(position, new Vector3(0, 0, -1), Vector3.Up, quadWidthScale, quadHeightScale);
        }
        
        public override void Load(ContentManager cManager)
        {
            SpriteSheet = cManager.Load<Texture2D>("Imagem" + Path.AltDirectorySeparatorChar + "Personagem" + Path.AltDirectorySeparatorChar + name);
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(gameTime, spriteSheet, quad, BlendState.AlphaBlend);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (GameObject obj in oManager.CollidableGameObjects)
            {
            }
        }
    }
}
