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
        private Rectangle bounds;

        protected Texture2D[] textures;

        private ButtonStates state;

        private string text;
        private bool useText;
        private Color btnColor;

        private Vector2 textPosition;

        private bool enabled;

        public delegate void MouseClicked(Button btn);
        public event MouseClicked mouseClicked;

        private string filePath;
        private string baseFileName;
        private float colorModifier;

        public float ColorModifier
        {
            get { return colorModifier; }
            set { colorModifier = value; }
        }
        
        public bool Enabled
        {
            get { return enabled; }
        }

        protected Color BtnColor
        {
            get { return btnColor; }
            set { btnColor = value; }
        }

        public Vector2 TextPosition
        {
            get { return textPosition; }
            set { textPosition = value; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

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
            this.state = ButtonStates.NORMAL;
            this.btnColor = Color.White;
            this.bounds = bounds;
            this.textPosition = Vector2.Zero;
            this.useText = true;
            this.Visible = true;
            this.enabled = true;
            this.colorModifier = 1f;
        }

        protected void cc_hover(bool hovering)
        {
            if (!enabled) return;

            if (hovering)
            {
                if (state != ButtonStates.PRESSING)
                    state = ButtonStates.HOVERING;
            }
            else if (state != ButtonStates.PRESSING)
            {
                state = ButtonStates.NORMAL;
            }
        }

        protected void cc_release()
        {
            if (state != ButtonStates.HOVERING)
                state = ButtonStates.NORMAL;
        }

        protected void cc_press()
        {
            if(enabled)
                state = ButtonStates.PRESSING;
        }

        protected void cc_exit()
        {

        }

        protected void cc_enter()
        {
        }

        protected virtual void cc_click()
        {
            if (CanClick())
                mouseClicked(this);
        }

        protected bool CanClick()
        {
            return mouseClicked != null && Visible && Enabled;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                int index = (int)state;

                ((Renderer2D)Renderer).Draw(textures[index], bounds, btnColor*colorModifier, BlendState.AlphaBlend);
                DrawText(gameTime);
            }
        }

        protected void DrawText(GameTime gameTime)
        {
            if (!String.IsNullOrEmpty(text) && UseText)
            {                
                ((Renderer2D)Renderer).DrawString(text, textPosition, btnColor * colorModifier);
            }
        }

        public override void Load(ContentManager cManager)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = "Menu" + Path.AltDirectorySeparatorChar;
            }
            if (!string.IsNullOrEmpty(baseFileName))
            {
                this.textures = new Texture2D[4];
                textures[0] = cManager.Load<Texture2D>(filePath + baseFileName + "N");
                textures[1] = cManager.Load<Texture2D>(filePath + baseFileName + "H");
                textures[2] = cManager.Load<Texture2D>(filePath + baseFileName + "P");
                textures[3] = textures[2];
            }
            CenterText();
        }

        protected void CenterText()
        {
            if (!string.IsNullOrEmpty(text))
            {
                Vector2 textSize = TextHelper.SpriteFont.MeasureString(text);
                float scale = 1;
                if (textSize.X > bounds.Width) //texto só mudará de escala caso seja maior que o botão
                    scale = (float)bounds.Width / textSize.X;
                textPosition = new Vector2(bounds.X + (bounds.Width - textSize.X * scale) / 2, bounds.Y + (bounds.Height - textSize.Y * scale) / 2); //centraliza texto no botão
            }
        }

        public void Disable()
        {
            enabled = false;
            colorModifier = 0.5f;
        }

        public void Enable()
        {
            enabled = true;
            colorModifier = 1f;
        }
    }
}
