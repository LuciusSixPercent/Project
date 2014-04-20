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
    public class Field : DrawableGameObject
    {
        Quad[] floorTiles;
        Texture2D[] textures;
        int rows;
        int columns;
        private const float scale = 1f;

        public Field(Renderer3D renderer, int rows, int columns)
            : base(renderer)
        {
            this.rows = rows;
            this.columns = columns;
            textures = new Texture2D[1];
            initQuads();
        }

        private void initQuads()
        {
            floorTiles = new Quad[rows * columns];

            float leftColumnX = -scale * 0.5f * columns - scale * 0.5f;

            Vector3 initialCoord = new Vector3(leftColumnX, 0f, ((Renderer3D)Renderer).Cam.Z);
            Vector3 normal = new Vector3(0, 1, 0);
            Vector3 up = new Vector3(0, 0, -1);
            for (int row = 0; row < rows; row++)
            {

                for (int col = 0; col < columns; col++)
                {
                    if (columns * row + col >= 56)
                    {
                        int a = col;
                    }
                    initialCoord.X += scale;
                    floorTiles[row * columns + col] = new Quad(initialCoord, normal, up, scale, scale);
                }
                initialCoord.Z += scale;
                initialCoord.X = leftColumnX;
            }
        }

        public override void Load(ContentManager cManager)
        {
            textures[0] = cManager.Load<Texture2D>("Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "grass");
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(gameTime, textures[0], floorTiles, BlendState.Opaque);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            translateQuads();
        }

        private void translateQuads()
        {
            foreach (Quad quad in floorTiles)
            {
                if (quad.Coord.Z <= ((Renderer3D)Renderer).Cam.Z)
                {
                    quad.Translate(new Vector3(0, 0, scale * rows));
                }
            }
        }
    }
}
