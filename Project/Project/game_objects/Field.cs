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
        private Prop[] props;
        private Texture2D[] cactus;
        private Texture2D[] grass;
        private Texture2D[] rock;
        private Texture2D[] tree;
        private float[] propsWeight;

        private Goal goal;
        private Bleachers bleachers;

        private int rows;
        private int columns;
        private const float scale = 1f;
        private bool keepMoving;

        //quantas linhas a frente serão solo normal e não gramado
        private int fieldPadding = 8;

        public Goal Goal
        {
            get { return goal; }
        }

        public bool KeepMoving
        {
            get { return keepMoving; }
            set { keepMoving = value; }
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
                foreach (Prop p in props)
                    p.Position = GetRandomPropPosition();
            }
        }

        public Field(Renderer3D renderer, int rows, int columns)
            : base(renderer)
        {
            goal = new Goal(renderer);
            bleachers = new Bleachers(renderer);
            this.rows = rows;
            this.columns = columns;
            keepMoving = true;
            textures = new Texture2D[4];
            floorTiles = new Quad[rows * columns];
            props = new Prop[256];
            cactus = new Texture2D[4];
            grass = new Texture2D[12];
            rock = new Texture2D[13];
            tree = new Texture2D[4];
            DefinePropsWeight();
            initQuads();
        }

        private void DefinePropsWeight()
        {
            propsWeight = new float[4];
            propsWeight[0] = 0.01f;  //tree
            propsWeight[1] = 0.2f;  //cactus
            propsWeight[2] = 0.65f;  //grass
            propsWeight[3] = 0.95f;  //rock
            
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
            BoundingBox = new BoundingBox(floorTiles[0].LowerLeft, floorTiles[floorTiles.Length - 1].UpperRight);
            goal.Position = new Vector3(0, 0, boundingBox.Max.Z - fieldPadding - 0.5f);
            bleachers.Position = new Vector3(0, 0, boundingBox.Max.Z - fieldPadding + 1.5f);
        }

        public override void Load(ContentManager cManager)
        {
            string path = "Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "Bate_Bola" + Path.AltDirectorySeparatorChar;
            textures[0] = cManager.Load<Texture2D>(path + "grass");
            textures[1] = cManager.Load<Texture2D>(path + "soil");
            textures[2] = cManager.Load<Texture2D>(path + "grass_soil_transition");
            textures[3] = Flip(textures[2]);
            goal.Load(cManager);
            bleachers.Load(cManager);
            path += "Props" + Path.AltDirectorySeparatorChar;
            LoadPropsTexture(path, "Cactus", cactus, cManager);
            LoadPropsTexture(path, "Grass", grass, cManager);
            LoadPropsTexture(path, "Rock", rock, cManager);
            LoadPropsTexture(path, "Tree", tree, cManager);
            CreateProps();
        }

        private void CreateProps()
        {
            for (int i = 0; i < props.Length; i++)
            {
                float baseScale = 0f;
                Texture2D tex = GetRandomPropTexture(out baseScale, propsWeight[3]);
                props[i] = new Prop((Renderer3D)Renderer, baseScale);
                props[i].Texture = tex;
                props[i].Position = GetRandomPropPosition();
            }
        }

        private Vector3 GetRandomPropPosition(int min = 0)
        {
            float x;
            do
            {
                x = (float)(PublicRandom.Next(columns * 100) - PublicRandom.Next(columns * 50)) / 100;
            } while ((x >= -3 && x <= 3) || (x > 10 || x < -10));
            float z = (float)PublicRandom.Next(min, rows * 100) / 100;

            return new Vector3(x, 0, z) + position;
        }

        private Texture2D GetRandomPropTexture(out float baseScale, double max = 1.0)
        {
            baseScale = 0;
            Texture2D tex;
            switch (GetRandomPropType(max))
            {
                case PropType.CACTUS:
                    tex = cactus[PublicRandom.Next(cactus.Length)];
                    baseScale = (float)PublicRandom.NextDouble(0.5);
                    break;
                case PropType.GRASS:
                    tex = grass[PublicRandom.Next(grass.Length)];
                    baseScale = (float)PublicRandom.NextDouble(0.1, 0.4);
                    break;
                case PropType.ROCK:
                    tex = rock[PublicRandom.Next(rock.Length)];
                    baseScale = (float)PublicRandom.NextDouble(0.1, 0.75);
                    break;
                case PropType.TREE:
                    tex = tree[PublicRandom.Next(tree.Length)];
                    baseScale = (float)PublicRandom.NextDouble(0.5) + PublicRandom.Next(1, 3) + PublicRandom.Next(2);
                    break;
                default:
                    tex = null;
                    break;
            }

            return tex;
        }

        private PropType GetRandomPropType(double max = 1.0)
        {
            double rdn = PublicRandom.NextDouble(0, max);
            PropType type = PropType.NONE;
            if (rdn < propsWeight[0])
            {
                type = PropType.TREE;
            }
            else if (rdn < propsWeight[1])
            {
                type = PropType.CACTUS;
            }
            else if (rdn < propsWeight[2])
            {
                type = PropType.GRASS;
            }
            else if (rdn < propsWeight[3])
            {
                type = PropType.ROCK;
            }
            return type;
        }
        private void LoadPropsTexture(string path, string name, Texture2D[] tex, ContentManager cManager)
        {
            path += name + Path.AltDirectorySeparatorChar;
            for (int i = 0; i < tex.Length; i++)
            {
                tex[i] = cManager.Load<Texture2D>(path + name + "_" + (i + 1));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int textureIndex = 1;
            int grassLeftBound = (int)Math.Round((float)columns / 2, MidpointRounding.ToEven) + 3;
            int grassRightBound = (int)Math.Round((float)columns / 2, MidpointRounding.ToEven) - 3;
            for (int i = 0; i < floorTiles.Length; i++)
            {
                int column = i % columns;
                int row = i / columns;

                if (column < grassRightBound || column > grassLeftBound || row >= rows - fieldPadding)
                {
                    textureIndex = 1;
                }
                else if (column == grassLeftBound)
                {
                    textureIndex = 2;
                }
                else if (column == grassRightBound)
                {
                    textureIndex = 3;
                }
                else
                {
                    textureIndex = 0;
                }

                ((Renderer3D)Renderer).Draw(textures[textureIndex], floorTiles[i], BlendState.Opaque, BoundingBox);
            }
            bleachers.Draw(gameTime);
            goal.Draw(gameTime);
            foreach (Prop p in props)
                p.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            translateQuads();
            goal.Update(gameTime);
            bleachers.Update(gameTime);
            CheckProps();
        }

        private void CheckProps()
        {
            float minZ = ((Renderer3D)Renderer).Cam.Position.Z + 2;
            int minSpawnZ = ((Renderer3D)Renderer).Cam.FarView * 100;
            foreach (Prop p in props)
            {
                if (p.Position.Z <= minZ)
                {
                    float baseScale;
                    Texture2D tex = GetRandomPropTexture(out baseScale);
                    if (baseScale > 0)
                    {
                        p.BaseScale = baseScale;
                        p.Texture = tex;
                    }
                    p.Position = GetRandomPropPosition(minSpawnZ);
                }
            }
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

        public override bool Collided(CollidableGameObject obj)
        {
            return obj.BoundingBox.Min.Y < position.Y;
        }

        public static Texture2D Flip(Texture2D source)
        {
            Texture2D flipped = new Texture2D(source.GraphicsDevice, source.Width, source.Height, true, SurfaceFormat.Color);
            SpriteBatch sb = new SpriteBatch(source.GraphicsDevice);
            float scale = 1f;
            for (int i = 0; i < source.LevelCount; i++)
            {
                RenderTarget2D rt2d = new RenderTarget2D(source.GraphicsDevice, (int)(source.Width * scale), (int)(source.Height * scale));
                rt2d.GraphicsDevice.SetRenderTarget(rt2d);
                                
                sb.Begin();
                sb.Draw(source, Vector2.Zero, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                sb.End();

                rt2d.GraphicsDevice.SetRenderTarget(null);
                
                Color[] flippedData = new Color[rt2d.Width * rt2d.Height];
                rt2d.GetData<Color>(flippedData);
                flipped.SetData<Color>(i, null, flippedData, 0, flippedData.Length);
                rt2d.Dispose();
                scale /= 2;
            }
            
            sb.Dispose();
            return flipped;
        }

        internal void Reset()
        {
            KeepMoving = true;
            Position = Vector3.Backward;
        }
    }
}
