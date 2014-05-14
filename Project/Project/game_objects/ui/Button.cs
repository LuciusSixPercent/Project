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
    public class Button : Scalable2DGameObject
    {
        protected Texture2D[] textures;

        private ButtonStates state;

        private string text;
        private bool useText;
        private int fontSize;
        private float fontScale;

        private Color defaultColor;
        private Color hoverColor;
        private Color pressColor;
        private Vector2 textPosition;

        private bool enabled;

        public delegate void MouseClicked(Button btn);
        public event MouseClicked mouseClicked;
        private bool forcingClick;

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

        public int FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                this.fontScale = ((float)fontSize / TextHelper.FontSize);
            }
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
            this.enabled = true;
            this.FontSize = 20;
            this.textures = new Texture2D[3];
            ColorModifier = 1f;
        }

        protected virtual void cc_hover(bool hovering)
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

        protected virtual void cc_release()
        {
            if (state != ButtonStates.HOVERING)
            {
                state = ButtonStates.NORMAL;
                Color = defaultColor;
            }
        }

        protected virtual void cc_press()
        {
            if (enabled)
            {
                state = ButtonStates.PRESSING;
                Color = pressColor;
            }
        }

        protected virtual void cc_exit()
        {

        }

        protected virtual void cc_enter()
        {
        }

        protected virtual void cc_click()
        {
            if (CanClick())
            {
                mouseClicked(this);
            }
        }

        protected bool CanClick()
        {
            return mouseClicked != null && (forcingClick || (Visible && Enabled));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            texture = textures[(int)state];
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                base.Draw(gameTime);
                DrawText(gameTime);
            }
        }

        protected void DrawText(GameTime gameTime)
        {
            if (!String.IsNullOrEmpty(text) && UseText)
            {                
                ((Renderer2D)Renderer).DrawString(text, textPosition, Color * ColorModifier, 0, Vector2.Zero, fontScale);
            }
        }

        public override void Load(ContentManager cManager)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = "Menu" + Path.AltDirectorySeparatorChar;
            }
            if (!string.IsNullOrEmpty(BaseFileName))
            {
                textures[0] = GetTexture(FilePath + BaseFileName + "N", cManager);
                textures[1] = GetTexture(FilePath + BaseFileName + "H", cManager);
                textures[2] = GetTexture(FilePath + BaseFileName + "P", cManager);

                texture = textures[0];
            }
            CenterText();
        }

        protected Texture2D GetTexture(string fullPath, ContentManager cManager)
        {
            Texture2D tex = null;

            try
            {
                tex = cManager.Load<Texture2D>(fullPath);
            }
            catch (ContentLoadException ex)
            {
            }

            return tex;
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
                if (textSize.X * fontScale > Width) //texto só mudará de escala caso seja maior que o botão
                {
                    scale = Width / (textSize.X * fontScale);
                    fontScale = scale;
                }
                textPosition = new Vector2(X + (Width - textSize.X * fontScale) / 2, Y + (Height - textSize.Y * fontScale) / 2);
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
