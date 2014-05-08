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
            TextHelper.SpriteBatch = spriteBatch;
        }

        public void Begin()
        {
            if (!beganSpriteBatch && !spriteBatch.IsDisposed)
            {
                spriteBatch.Begin();
                beganSpriteBatch = true;
            }
        }

        public void End()
        {
            if (beganSpriteBatch && !spriteBatch.IsDisposed)
            {
                spriteBatch.End();
                beganSpriteBatch = false;
            }
        }

        public void Draw(Texture2D texture, Vector2 position, Color color, BlendState blendState)
        {
            if (texture == null) return;
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

        public void Draw(Texture2D texture, Vector2 position, Rectangle source, Color color, BlendState blendState)
        {
            if (texture == null) return;
            GDevice.BlendState = blendState;
            if (!beganSpriteBatch)
            {
                Begin();
            }
            if (beganSpriteBatch)
            {
                spriteBatch.Draw(texture, position, source, color * Alpha);
            }
        }

        public void Draw(Texture2D texture, Rectangle bounds, Color color, BlendState blendState)
        {
            if (texture == null)
                return;
            GDevice.BlendState = blendState;
            if (!beganSpriteBatch)
            {
                Begin();
            }
            if (beganSpriteBatch)
            {
                spriteBatch.Draw(texture, bounds, color * Alpha);
            }
        }

        public void DrawString(string text, Vector2 position, Color color)
        {
            if (!beganSpriteBatch)
            {
                Begin();
            }
            if (beganSpriteBatch)
            {
                spriteBatch.DrawString(TextHelper.SpriteFont, text, position, color * Alpha);
            }
        }

        internal void DrawString(string text, Vector2 position, Color color, int rotation, Vector2 vector2, float scale)
        {
            if (!beganSpriteBatch)
            {
                Begin();
            }
            if (beganSpriteBatch)
            {
                spriteBatch.DrawString(TextHelper.SpriteFont, text, position, color * Alpha, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
