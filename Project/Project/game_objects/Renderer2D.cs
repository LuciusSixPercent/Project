using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project;

namespace game_objects
{
    public class Renderer2D : Renderer
    {
        private SpriteBatch spriteBatch;
        bool beganSpriteBatch;

        public Renderer2D(GraphicsDevice gDevice)
            : base(gDevice)
        {
            spriteBatch = new SpriteBatch(gDevice);
            TextureHelper.SpriteBatch = spriteBatch;
        }

        public void Begin()
        {
            if (!beganSpriteBatch)
            {
                spriteBatch.Begin();
                beganSpriteBatch = true;
            }
        }

        public void End()
        {
            if (beganSpriteBatch)
            {
                spriteBatch.End();
                beganSpriteBatch = false;
            }
        }

        public void Draw(GameTime gameTime, Texture2D texture, Vector2 position, Color color, BlendState blendState)
        {
            GDevice.BlendState = blendState;
            if (!beganSpriteBatch)
            {
                Begin();
            }
            if (beganSpriteBatch)
            {
                spriteBatch.Draw(texture, position, color * Alpha);
            }
        }
    }
}
