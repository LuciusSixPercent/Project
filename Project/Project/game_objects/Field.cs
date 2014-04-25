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
    public class Field : CollidableGameObject
    {

        private Quad[] floorTiles;
        private Texture2D[] textures;

        private Goal goal;

        private int rows;
        private int columns;
        private const float scale = 1f;
        private bool keepMoving;

        public Goal Goal
        {
            get { return goal; }
        }

        public bool KeepMoving
        {
            get { return keepMoving; }
            set { keepMoving = value; }
        }

        public Field(Renderer3D renderer, List<CollidableGameObject> collidableObjects, int rows, int columns)
            : base(renderer, collidableObjects)
        {
            goal = new Goal(renderer, collidableObjects);
            this.rows = rows;
            this.columns = columns;
            keepMoving = true;
            textures = new Texture2D[1];
            floorTiles = new Quad[rows * columns];
            initQuads();
        }

        private void initQuads()
        {
            float leftColumnX = -scale * 0.5f * columns - scale * 0.5f;

            Vector3 initialCoord = new Vector3(leftColumnX, 0f, ((Renderer3D)Renderer).Cam.Z);
            Vector3 normal = new Vector3(0, 1, 0);
            Vector3 up = new Vector3(0, 0, 1);
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    initialCoord.X += scale;
                    floorTiles[row * columns + col] = new Quad(initialCoord, normal, up, scale, scale);
                }
                initialCoord.Z += scale;
                initialCoord.X = leftColumnX;
            }
            BoundingBox = new BoundingBox(floorTiles[0].Vertices[1].Position, floorTiles[floorTiles.Length - 1].Vertices[2].Position);
            goal.Position = new Vector3(0, 0, boundingBox.Max.Z);
        }

        public override void Load(ContentManager cManager)
        {
            textures[0] = cManager.Load<Texture2D>("Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "grass");
            goal.Load(cManager);
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(textures[0], floorTiles, BlendState.Opaque, BoundingBox);
            goal.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            translateQuads();
            goal.Update(gameTime);
        }

        private void translateQuads()
        {
            if (KeepMoving)
            {
                bool quadsMoved = false;
                foreach (Quad quad in floorTiles)
                {
                    if (quad.Coord.Z <= ((Renderer3D)Renderer).Cam.Z)
                    {
                        quad.Translate(new Vector3(0, 0, scale * rows));
                        quadsMoved = true;
                    }
                }
                if (quadsMoved)
                {
                    ImediateTranslate(new Vector3(0, 0, scale));
                }
            }
        }

        public override void ImediateTranslate(Vector3 amount)
        {
            base.ImediateTranslate(amount);
            boundingBox.Min += amount;
            boundingBox.Max += amount;
            goal.Translate(amount);
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
                initQuads();
            }
        }

        public override bool Collided(CollidableGameObject obj)
        {
            return obj.BoundingBox.Min.Y < position.Y;
        }
    }
}
