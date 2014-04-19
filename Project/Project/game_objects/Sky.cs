using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace game_objects
{
    public class Sky : DrawableGameObject
    {
        Texture2D tex;

        public Sky(Renderer2D renderer)
            : base(renderer)
        {
        }

        public override void Load(Microsoft.Xna.Framework.Content.ContentManager cManager)
        {
            tex = cManager.Load<Texture2D>("Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "sky");
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer2D)Renderer).Draw(gameTime, tex, Vector2.Zero, Color.White, BlendState.Opaque);
        }
    }
}
