using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using components;
using Microsoft.Xna.Framework.Content;

namespace game_objects
{
    public class Button : DrawableGameObject
    {
        Texture2D texture;
        SpriteBatch spriteBatch;
        Rectangle bounds;
        private string text;

        public delegate void MouseClicked(Button btn);
        public event MouseClicked mouseClicked;
        private Color color;
        private SpriteFont spriteFont;

        public SpriteFont SpriteFont
        {
            get { return spriteFont; }
            set { spriteFont = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Button(Rectangle bounds, SpriteBatch spriteBatch, Renderer2D r2D)
            : base(r2D)
        {
            ClickComponent cc = new ClickComponent(this, bounds);
            cc.click += new ClickComponent.Click(cc_click);
            cc.enter += new ClickComponent.Enter(cc_enter);
            cc.exit += new ClickComponent.Exit(cc_exit);
            addComponent(cc);
            this.color = Color.White;
            this.bounds = bounds;
            this.spriteBatch = spriteBatch;
        }

        //estou usando cores, mas poderia mudar a textura do botão
        void cc_exit()
        {
            color = Color.White;
        }

        void cc_enter()
        {
            color = Color.Red;
        }

        void cc_click()
        {
            if (mouseClicked != null && Visible)
                mouseClicked(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                ((Renderer2D)Renderer).Draw(texture, bounds, color, BlendState.AlphaBlend);
                if (!String.IsNullOrEmpty(text) && spriteFont != null)
                {
                    Vector2 textSize = spriteFont.MeasureString(text);
                    float scale = 1;
                    //if (textSize.X > bounds.Width) -> se tirar o comentário daqui, o texto só mudará de escala caso seja maior que o botão
                    scale = (float)bounds.Width / textSize.X;
                    Vector2 position = new Vector2(bounds.X + (bounds.Width - textSize.X * scale) / 2, bounds.Y + (bounds.Height - textSize.Y * scale) / 2);
                    ((Renderer2D)Renderer).DrawString(text, position, color);
                }
            }
        }

        public override void Load(ContentManager cManager)
        {
            texture = cManager.Load<Texture2D>("btn");
        }
    }
}
