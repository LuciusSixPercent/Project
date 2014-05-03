using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace game_objects
{
    public class Background : DrawableGameObject
    {
        Texture2D sky;
        Texture2D mountains;
        Vector2 mountainsPosition;

        public Background(Renderer2D renderer)
            : base(renderer)
        {            
        }

        public override void Load(ContentManager cManager)
        {
            string path = "Imagem" + Path.AltDirectorySeparatorChar + "Cenario" + Path.AltDirectorySeparatorChar + "Bate_Bola" + Path.AltDirectorySeparatorChar;
            sky = cManager.Load<Texture2D>(path + "sky");
            mountains = cManager.Load<Texture2D>(path + "mountains");
            mountainsPosition = new Vector2(0, mountains.Height/10);
        }

        public override void Draw(GameTime gameTime)
        {
            Renderer2D r2d = ((Renderer2D)Renderer);
            r2d.Draw(sky, Vector2.Zero, Color.White, BlendState.Opaque);
            r2d.Draw(mountains, mountainsPosition, Color.White, BlendState.Opaque);
        }
    }
}
