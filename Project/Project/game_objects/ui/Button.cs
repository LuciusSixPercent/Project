using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using components;
using Microsoft.Xna.Framework.Content;
using Project;
using System.IO;

namespace game_objects.ui
{
    public class Button : DrawableGameObject
    {
        Rectangle bounds;

        Texture2D[] textures;

        private ButtonStates state;

        private string text;
        private bool useText;
        private Color color;

        public delegate void MouseClicked(Button btn);
        public event MouseClicked mouseClicked;

        private string filePath;
        private string baseFileName;

        /// <summary>
        /// O nome da textura que será utilizada nem nenhum sufixo (N, H, P).
        /// Por exemplo, para um botão que possua 3 arquivos "voltarN", "voltarH" e "voltarP",
        /// BaseFileName deve ser somente "voltar".
        /// </summary>
        public string BaseFileName
        {
            get { return baseFileName; }
            set { baseFileName = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public bool UseText
        {
            get { return useText; }
            set { useText = value; }
        }

        public ButtonStates State
        {
            get { return state; }
            set { state = value; }
        }

        public Button(Renderer2D r2D, Rectangle bounds)
            : base(r2D)
        {
            ClickComponent cc = new ClickComponent(this, bounds);
            cc.click += new ClickComponent.Click(cc_click);
            cc.press += new ClickComponent.Press(cc_press);
            cc.release += new ClickComponent.Release(cc_release);
            cc.enter += new ClickComponent.Enter(cc_enter);
            cc.exit += new ClickComponent.Exit(cc_exit);
            cc.hover += new ClickComponent.Hover(cc_hover);
            addComponent(cc);
            this.state = ButtonStates.DEFAULT;
            this.color = Color.White;
            this.bounds = bounds;
            this.textures = new Texture2D[3];
            this.Visible = true;
        }

        void cc_hover(bool hovering)
        {
            if (hovering)
            {
                if (state != ButtonStates.PRESSING)
                    state = ButtonStates.HOVERING;
            }
            else if (state != ButtonStates.PRESSING)
            {
                state = ButtonStates.DEFAULT;
            }
        }

        void cc_release()
        {
            if(state != ButtonStates.HOVERING)
                state = ButtonStates.DEFAULT;
        }

        void cc_press()
        {
            state = ButtonStates.PRESSING;
        }

        void cc_exit()
        {
            
        }

        void cc_enter()
        {
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
                ((Renderer2D)Renderer).Draw(textures[(int)state], bounds, color, BlendState.AlphaBlend);
                if (!String.IsNullOrEmpty(text) && UseText)
                {
                    DrawText(gameTime);
                }
            }
        }

        private void DrawText(GameTime gameTime)
        {
            Vector2 textSize = TextHelper.SpriteFont.MeasureString(text);
            float scale = 1;
            if (textSize.X > bounds.Width) //texto só mudará de escala caso seja maior que o botão
                scale = (float)bounds.Width / textSize.X;
            Vector2 position = new Vector2(bounds.X + (bounds.Width - textSize.X * scale) / 2, bounds.Y + (bounds.Height - textSize.Y * scale) / 2); //centraliza texto no botão
            ((Renderer2D)Renderer).DrawString(text, position, color);
        }

        public override void Load(ContentManager cManager)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = "Menu" + Path.AltDirectorySeparatorChar;
            }
            if (!string.IsNullOrEmpty(baseFileName))
            {
                textures[0] = cManager.Load<Texture2D>(filePath + baseFileName + "N");
                textures[1] = cManager.Load<Texture2D>(filePath + baseFileName + "H");
                textures[2] = cManager.Load<Texture2D>(filePath + baseFileName + "P");
            }
        }
    }
}
