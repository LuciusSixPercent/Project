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

        public delegate void Enter();
        public event Enter enter;

        public delegate void Exit();
        public event Exit exit;

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
                if (!wasHovering && enter != null)
                {
                    enter();
                }
                if (state.LeftButton == ButtonState.Pressed)
                {
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
                }
            }
            else if (wasHovering && exit != null)
            {
                exit();
            }
        }

        private bool mouseWithinBounds(MouseState state)
        {
            return state.X >= bounds.Left && state.X <= bounds.Right && state.Y >= bounds.Top && state.Y <= bounds.Bottom;
        }
    }
}
