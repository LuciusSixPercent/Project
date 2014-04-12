using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using game_objects;
using components;
using Project;

namespace game_states
{
    class TestState : GameState
    {
        private Button[] buttons;
        Vector2 clickPosition;
        private int clickTime;
        private Random rdn;
        SpriteFont spriteFont;

        Color textColor;
        float textScale;
        float textRotation;

        public TestState(int id, Game1 parent)
            : base(id, parent)
        {
        }

        protected override void Initialize()
        {
            if (!initialized)
            {
                base.Initialize();
                enterTransitionDuration = 60;
                buttons = new Button[3];
                Rectangle initialBounds = new Rectangle(100, 100, 50, 50);
                rdn = new Random();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i] = new Button(initialBounds, SpriteBatch);
                    buttons[i].mouseClicked += new Button.MouseClicked(TestState_mouseClicked);
                    buttons[i].Visible = i < 2;
                    initialBounds.X += 75;
                    initialBounds.Y += rdn.Next(75) - rdn.Next(100);
                    double scale = rdn.NextDouble()*4 + 0.75;
                    initialBounds.Width = (int)(initialBounds.Width * scale);
                    initialBounds.Height = (int)(initialBounds.Height * scale);
                }

                buttons[0].Text = "Open";
                buttons[1].Text = "Close";
                clickPosition = new Vector2(initialBounds.Left + rdn.Next(10), initialBounds.Top + (rdn.Next(10) - rdn.Next(15)));

            }
        }

        void TestState_mouseClicked(Button btn)
        {
            if (btn.Equals(buttons[2]))
            {
                clickTime = 1000;
                clickPosition = new Vector2(rdn.Next(700), (float)rdn.Next(400));
                textColor = Color.Blue;
                alpha = 1f;
                textScale = (float)rdn.NextDouble()*2 + 0.25f;
                textRotation = MathHelper.ToRadians(rdn.Next(361));
            }
            else
            {
                buttons[2].Visible = btn.Equals(buttons[0]);
            }
        }

        protected override void LoadContent()
        {
            foreach (Button btn in buttons)
                btn.LoadContent(parent.Content);
            spriteFont = parent.Content.Load<SpriteFont>("Verdana");
            foreach (Button btn in buttons)
                btn.SpriteFont = spriteFont;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (stateEntered)
            {
                if (!exitingState)
                {
                    foreach (Button btn in buttons)
                        btn.Update(gameTime);
                    if (clickTime > 0)
                    {
                        clickTime -= gameTime.ElapsedGameTime.Milliseconds;
                        alpha = (float)clickTime / 1000;
                    }
                }
            }
            else if (!enteringState)
            {
                parent.ExitState(this.ID);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Button btn in buttons)
                btn.Draw(gameTime);
            if (clickTime > 0)
            {
                SpriteBatch.Begin();
                SpriteBatch.DrawString(spriteFont, "click", clickPosition, textColor*alpha, textRotation, Vector2.Zero, textScale, SpriteEffects.None, 0f);
                SpriteBatch.End();
            }

        }

        public override void EnterState(bool freezeBelow)
        {
            if (!exitingState)
            {
                base.EnterState(freezeBelow);
                Initialize();
                LoadContent();
            }
        }

        public override void ExitState()
        {
            if (!enteringState)
            {
                base.ExitState();
                parent.Content.Unload();
            }
        }
    }
}
