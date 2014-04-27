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
        private Bleachers bleachers;

        private int rows;
        private int columns;
        private const float scale = 1f;
        private bool keepMoving;

        //quantas linhas a frente serão solo normal e não gramado
        private int fieldPadding = 6;

        public Goal Goal
        {
            get { return goal; }
        }

        public bool KeepMoving
        {
            get { return keepMoving; }
            set { keepMoving = value; }
        }

        public Field(Renderer3D renderer, int rows, int columns)
            : base(renderer)
        {
            goal = new Goal(renderer);
            bleachers = new Bleachers(renderer);
            this.rows = rows;
            this.columns = columns;
            keepMoving = true;
            textures = new Texture2D[2];
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
            BoundingBox = new BoundingBox(floorTiles[0].Vertices[0].Position, floorTiles[floorTiles.Length - 1].Vertices[3].Position);
            goal.Position = new Vector3(0, 0, boundingBox.Max.Z - fieldPadding - 0.5f);
            bleachers.Position = new Vector3(0, 0, boundingBox.Max.Z - fieldPadding + 1.5f);
        }

        public override void Load(ContentManager cManager)
        {
            textures[0] = cManager.Load<Texture2D>("Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "grass");
            textures[1] = cManager.Load<Texture2D>("Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "soil");
            goal.Load(cManager);
            bleachers.Load(cManager);
        }

        public override void Draw(GameTime gameTime)
        {
            int textureIndex = 1;
            for (int i = 0; i < floorTiles.Length; i++)
            {
                if (i % columns == Math.Round((float)columns / 2, MidpointRounding.ToEven) + 3 || i / columns >= rows - fieldPadding)
                {
                    textureIndex = 1;
                } else
                if (i % columns == Math.Round((float)columns/2, MidpointRounding.AwayFromZero) - 3)
                {
                    textureIndex = 0;
                }

                ((Renderer3D)Renderer).Draw(textures[textureIndex], floorTiles[i], BlendState.Opaque, BoundingBox);
            }
            bleachers.Draw(gameTime);
            goal.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            translateQuads();
            goal.Update(gameTime);
            bleachers.Update(gameTime);
        }

        private void translateQuads()
        {
            if (KeepMoving)
            {
                bool quadsMoved = false;
                for (int i = 0; i < floorTiles.Length; i++)
                {
                    Quad quad = floorTiles[i];
                    if (quad.Coord.Z <= ((Renderer3D)Renderer).Cam.Z)
                    {
                        quad.Translate(new Vector3(0, 0, scale * (rows - fieldPadding)));
                        quadsMoved = true;
                    }
                }

                if (quadsMoved)
                {
                    for (int row = rows - 1; row >= rows - fieldPadding; row--)
                    {
                        for (int col = 0; col < columns; col++)
                        {
                            floorTiles[row * columns + col].Translate(new Vector3(0, 0, scale));
                        }
                    }
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
            bleachers.Translate(amount);
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
