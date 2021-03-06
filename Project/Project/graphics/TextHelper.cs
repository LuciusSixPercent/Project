﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Project
{
    public static class TextHelper
    {
        private static Dictionary<string, Texture2D> cachedTextures;

        private static SpriteBatch spriteBatch;
        private static SpriteFont defaultSpriteFont;
        private static SpriteFont spriteFont;
        private static Queue<string> queuedStrings;
        private static Color primaryColor;
        private static int fontSize;

        public static int FontSize
        {
            get {
                if (fontSize == 0) fontSize = GetSize();
                return fontSize;
            }
        }

        private static int GetSize()
        {
            int size = 0;
            for (char c = 'A'; c <= 'Z'; c++)
            {
                Vector2 letterSize = SpriteFont.MeasureString(c.ToString());
                if (letterSize.Y > size)
                    size = (int)letterSize.Y;
            }
            return size;
        }

        public static Color PrimaryColor
        {
            get {
                if (TextHelper.primaryColor == Color.Transparent)
                    TextHelper.primaryColor = Color.White;

                return TextHelper.primaryColor; }
            set { TextHelper.primaryColor = value; }
        }
        private static Color secondaryColor;

        public static Color SecondaryColor
        {
            get {
                if (TextHelper.secondaryColor == Color.Transparent)
                    TextHelper.secondaryColor = Color.Black;
                return TextHelper.secondaryColor; 
            }
            set { TextHelper.secondaryColor = value; }
        }

        private static Queue<string> QueuedStrings
        {
            get
            {
                if (TextHelper.queuedStrings == null)
                {
                    TextHelper.queuedStrings = new Queue<string>();
                }
                return TextHelper.queuedStrings;
            }
        }

        public static SpriteFont SpriteFont
        {
            get { return TextHelper.spriteFont; }
            set { TextHelper.spriteFont = value; }
        }

        public static SpriteBatch SpriteBatch
        {
            get { return TextHelper.spriteBatch; }
            set
            {
                TextHelper.spriteBatch = value;
            }
        }

        private static Dictionary<string, Texture2D> CachedTextures
        {
            get
            {
                if (cachedTextures == null)
                    cachedTextures = new Dictionary<string, Texture2D>(10);
                return TextHelper.cachedTextures;
            }
        }


        public static bool StringToTexture(string text, out Texture2D texture)
        {
            if (spriteBatch != null && !string.IsNullOrEmpty(text))
            {

                if (!CachedTextures.ContainsKey(text))
                {
                    QueuedStrings.Enqueue(text);
                }
                else
                {
                    texture = CachedTextures[text];
                    return true;
                }

            }
            texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            return false;
        }

        private static Texture2D CreateTexture(string text)
        {
            Vector2 txtSize = spriteFont.MeasureString(text);

            RenderTarget2D rt2D = new RenderTarget2D(spriteBatch.GraphicsDevice, (int)txtSize.X+1, (int)txtSize.Y+1);

            spriteBatch.GraphicsDevice.SetRenderTarget(rt2D);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            BlendState defaultBlendState = spriteBatch.GraphicsDevice.BlendState;
            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, text, Vector2.Zero, SecondaryColor);
            spriteBatch.DrawString(spriteFont, text, Vector2.One, PrimaryColor);
            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.GraphicsDevice.BlendState = defaultBlendState;

            return (Texture2D)rt2D;
        }

        public static void LoadDefaultFont(ContentManager contentManager)
        {
            if (defaultSpriteFont == null)
            {
                defaultSpriteFont = contentManager.Load<SpriteFont>("Fonte" + Path.AltDirectorySeparatorChar + "Arial");
                spriteFont = defaultSpriteFont;
            }
        }

        public static void AddToCache(string text)
        {
            if (!CachedTextures.ContainsKey(text))
            {
                Texture2D tmp = CreateTexture(text);
                Texture2D texture = new Texture2D(tmp.GraphicsDevice, tmp.Width, tmp.Height);
                Color[] colorData = new Color[tmp.Width * tmp.Height];
                tmp.GetData<Color>(colorData);
                texture.SetData<Color>(colorData);
                tmp.Dispose();
                CachedTextures.Add(text, texture);
            }
        }
        public static void CacheQueued()
        {
            int counter = 0; //evitar que o jogo se prenda tentando criar muitas texturas de uma só vez
            while (QueuedStrings.Count > 0 && counter < 5)
            {
                AddToCache(QueuedStrings.Dequeue());
                counter++;
            }
            if (counter == 5)
            {
                counter = 0;
            }
        }

        public static void FlushCache()
        {
            cachedTextures.Clear();
        }
    }
}
