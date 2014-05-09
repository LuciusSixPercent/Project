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
    public class Button : Simple2DGameObject
    {
        protected Texture2D[] textures;

        private ButtonStates state;

        private string text;
        private bool useText;
        private Color defaultColor;
        private Color hoverColor;
        private Color pressColor;

        private Vector2 textPosition;

        private bool enabled;

        public delegate void MouseClicked(Button btn);
        public event MouseClicked mouseClicked;

        private string filePath;
        private string baseFileName;
        private bool forcingClick;
        private bool clicked;
        private bool lockOnClick;

        public bool Clicked
        {
            get { return clicked; }
            set { clicked = value; }
        }

        public bool LockOnClick
        {
            get { return lockOnClick; }
            set { lockOnClick = value; }
        }
        
        public bool Enabled
        {
            get { return enabled; }
        }

        protected Color DefaultColor
        {
            get { return defaultColor; }
            set { defaultColor = value; }
        }

        public Color HoverColor
        {
            get { return hoverColor; }
            set { hoverColor = value; }
        }

        public Color PressColor
        {
            get { return pressColor; }
            set { pressColor = value; }
        }

        public Vector2 TextPosition
        {
            get { return textPosition; }
            set { textPosition = value; }
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

        public virtual ButtonStates State
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
            this.Color = this.defaultColor = this.hoverColor = this.pressColor = Color.White;
            Bounds = bounds;
            this.position = new Vector3(Bounds.X, Bounds.Y, 0);
            this.dimensions = new Vector3(Bounds.Width, Bounds.Height, 0);
            this.textPosition = Vector2.Zero;
            this.useText = true;
            this.Visible = true;
            this.enabled = true;
            ColorModifier = 1f;
        }

        protected void cc_hover(bool hovering)
        {
            if (!enabled) return;
            if (hovering)
            {
                if (state != ButtonStates.PRESSING)
                {
                    state = ButtonStates.HOVERING;
                    Color = hoverColor;
                }
            }
            else if (state != ButtonStates.PRESSING)
            {
                state = ButtonStates.NORMAL;
                Color = defaultColor;
            }
        }

        protected void cc_release()
        {
            if (state != ButtonStates.HOVERING)
            {
                state = ButtonStates.NORMAL;
                Color = defaultColor;
            }
        }

        protected void cc_press()
        {
            if (enabled)
            {
                state = ButtonStates.PRESSING;
                Color = pressColor;
            }
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
            {
                Clicked = true;
                mouseClicked(this);
            }
        }

        protected bool CanClick()
        {
            return mouseClicked != null && (forcingClick || (Visible && Enabled));
        }

        public override void Update(GameTime gameTime)
        {
            if (!lockOnClick && Clicked)
                Clicked = false;

            base.Update(gameTime);

            texture = textures[(int)state];
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                base.Draw(gameTime);
                //((Renderer2D)Renderer).Draw(textures[(int)state], Bounds, Color * ColorModifier, BlendState.AlphaBlend);
                DrawText(gameTime);
            }
        }

        protected void DrawText(GameTime gameTime)
        {
            if (!String.IsNullOrEmpty(text) && UseText)
            {                
                ((Renderer2D)Renderer).DrawString(text, textPosition, Color * ColorModifier);
            }
        }

        public override void Load(ContentManager cManager)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = "Menu" + Path.AltDirectorySeparatorChar;
            }
            this.textures = new Texture2D[4];
            if (!string.IsNullOrEmpty(baseFileName))
            {
                textures[0] = cManager.Load<Texture2D>(filePath + baseFileName + "N");
                textures[1] = cManager.Load<Texture2D>(filePath + baseFileName + "H");
                textures[2] = cManager.Load<Texture2D>(filePath + baseFileName + "P");
                textures[3] = textures[2];
            }
            CenterText();
        }

        /// <summary>
        /// Centraliza o texto no botão.
        /// </summary>
        protected void CenterText()
        {
            if (!string.IsNullOrEmpty(text))
            {
                Vector2 textSize = TextHelper.SpriteFont.MeasureString(text);
                float scale = 1;
                if (textSize.X > Bounds.Width) //texto só mudará de escala caso seja maior que o botão
                    scale = (float)Bounds.Width / textSize.X;
                textPosition = new Vector2(Bounds.X + (Bounds.Width - textSize.X * scale) / 2, Bounds.Y + (Bounds.Height - textSize.Y * scale) / 2);
            }
        }

        public void Disable()
        {
            enabled = false;
            ColorModifier = 0.5f;
        }

        public void Enable()
        {
            enabled = true;
            ColorModifier = 1f;
        }

        public void ForceClick()
        {
            forcingClick = true;
            if (mouseClicked != null)
                cc_click();
        }
    }
}
