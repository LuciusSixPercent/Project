using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using game_objects;

namespace components
{
    public class ClickComponent : Component
    {
        public delegate void Click();
        public event Click click;

        public delegate void Press();
        public event Press press;

        public delegate void Release();
        public event Release release;

        public delegate void Enter();
        public event Enter enter;

        public delegate void Exit();
        public event Exit exit;

        public delegate void Hover(bool hovering);
        public event Hover hover;

        private bool mousePressing;
        private bool mouseHovering;

        private Rectangle bounds;

        public ClickComponent(GameObject owner, Rectangle bounds)
            : base(owner)
        {
            this.bounds = bounds;
        }
        
        public override void Update(GameTime gameTime)
        {
            MouseState state= Mouse.GetState();
            bool wasHovering = mouseHovering;
            mouseHovering = mouseWithinBounds(state);
            if (mouseHovering)
            {
                if (hover != null)
                    hover(true);
                if (!wasHovering && enter != null)
                {
                    enter();
                }
                if (state.LeftButton == ButtonState.Pressed)
                {
                    if (press != null)
                        press();
                    mousePressing = true;
                }
                else
                {
                    if (mousePressing) //se o mouse deixou de ser pressionado, então houve um click
                    {
                        if (click != null)
                            click();
                    }
                    mousePressing = false;
                    if (release != null)
                        release();
                }
            }
            else
            {
                if (hover != null)
                    hover(false);
                if (wasHovering && exit != null)
                {
                    exit();
                }
                else if (state.LeftButton != ButtonState.Pressed && mousePressing)
                {
                    mousePressing = false;
                    if (release != null)
                        release();
                }
            }
        }

        private bool mouseWithinBounds(MouseState state)
        {
            return state.X >= bounds.Left && state.X <= bounds.Right && state.Y >= bounds.Top && state.Y <= bounds.Bottom;
        }
    }
}
