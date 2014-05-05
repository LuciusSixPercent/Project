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

        private bool pressedOutOfBounds;

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
                HandleMouseEnter(wasHovering);
                HandlePressWithinBounds(state);
            }
            else
            {
                HandleMouseExit(wasHovering);
                HandlePressOutOfBounds(state);
            }
        }

        private void HandleMouseEnter(bool wasHovering)
        {
            if (hover != null)
                hover(true);

            if (!wasHovering && enter != null)
            {
                enter();
            }
        }

        private void HandleMouseExit(bool wasHovering)
        {
            if (hover != null)
                hover(false);

            if (wasHovering)
            {
                if (exit != null)
                    exit();
            }
        }

        private void HandlePressOutOfBounds(MouseState state)
        {
            if (state.LeftButton != ButtonState.Pressed)
            {
                HandleMouseRelease();
            }
            else if (!mousePressing)
            {
                pressedOutOfBounds = true;
            }
        }

        /// <summary>
        /// Lida com o mouse quando o mesmo é tem o botão esquerdo pressionado dentro da área do botão.
        /// </summary>
        /// <param name="state">O estado do mouse.</param>
        private void HandlePressWithinBounds(MouseState state)
        {
            if (state.LeftButton == ButtonState.Pressed) //apenas reconheceremos o mouse como pressionado se o mesmo foi pressionado dentro dos limites estabelecidos
            {
                if (!pressedOutOfBounds)
                {
                    if (press != null)
                        press();
                    mousePressing = true;
                }
            }
            else
            {
                HandleMouseRelease();
            }
        }

        private void HandleMouseRelease()
        {
            if (mousePressing) //se o mouse deixou de ser pressionado, então houve um click
            {
                if (mouseHovering && !pressedOutOfBounds)
                {
                    if (click != null)
                        click();
                }
                if (release != null)
                    release();
            }
            pressedOutOfBounds = false;
            mousePressing = false;
            
        }

        private bool mouseWithinBounds(MouseState state)
        {
            return state.X >= bounds.Left && state.X <= bounds.Right && state.Y >= bounds.Top && state.Y <= bounds.Bottom;
        }
    }
}
