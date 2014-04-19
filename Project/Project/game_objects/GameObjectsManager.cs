using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using game_states;
using Microsoft.Xna.Framework.Content;

namespace game_objects
{
    public class GameObjectsManager
    {
        private List<GameObject> gameObjects;
        private List<DrawableGameObject> drawableGameObjects;
        private List<DrawableGameObject> collidableGameObjects;


        private GraphicsDevice graphicsDevice;
        private Renderer2D r2D;
        private Renderer3D r3D;

        public List<DrawableGameObject> CollidableGameObjects
        {
            get { return collidableGameObjects; }
        }

        public Renderer2D R2D
        {
            get { return r2D; }
        }

        public Renderer3D R3D
        {
            get { return r3D; }
        }

        public List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }

        public GameObjectsManager(GraphicsDevice graphicsDevice)
        {
            gameObjects = new List<GameObject>();
            drawableGameObjects = new List<DrawableGameObject>();
            collidableGameObjects = new List<DrawableGameObject>();

            this.graphicsDevice = graphicsDevice;
            r2D = new Renderer2D(graphicsDevice);
            r3D = new Renderer3D(graphicsDevice);
        }


        public void AddObject(GameObject obj)
        {
            gameObjects.Add(obj);
            if (obj is DrawableGameObject)
            {
                drawableGameObjects.Add((DrawableGameObject)obj);
            }

        }

        public void Load(ContentManager cManager)
        {
            foreach (DrawableGameObject dgObj in drawableGameObjects)
                dgObj.Load(cManager);
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject gObj in gameObjects)
            {
                gObj.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            Renderer lastRenderer = null;
            foreach (DrawableGameObject dgObj in drawableGameObjects)
            {
                if (lastRenderer != dgObj.Renderer)
                {
                    if (lastRenderer is Renderer2D)
                        ((Renderer2D)lastRenderer).End();
                    lastRenderer = dgObj.Renderer;
                }
                dgObj.Draw(gameTime);
            }
            r2D.End();
        }
    }
}
