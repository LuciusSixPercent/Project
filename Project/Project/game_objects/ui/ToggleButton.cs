using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace game_objects.ui
{
    public class ToggleButton : Button
    {
        private bool lockToggleState;

        public bool LockToggleState
        {
            get { return lockToggleState; }
            set { lockToggleState = value; }
        }

        public ToggleButton(Renderer2D r2D, Rectangle bounds)
            : base(r2D, bounds)
        {
            textures = new Texture2D[4];
        }

        protected override void cc_click()
        {
            if (State == ButtonStates.TOGGLED)
            {
                if (!lockToggleState)
                {
                    State = ButtonStates.NORMAL;
                }
            }
            else
            {
                State = ButtonStates.TOGGLED;
                base.cc_click();
            }
        }

        protected override void cc_hover(bool hovering)
        {
            if(State != ButtonStates.TOGGLED)
                base.cc_hover(hovering);
        }

        protected override void cc_press()
        {
            if (State != ButtonStates.TOGGLED)
                base.cc_press();
        }

        protected override void cc_release()
        {
            if (State != ButtonStates.TOGGLED)
                base.cc_release();
        }

        public override void Load(ContentManager cManager)
        {
            base.Load(cManager);

            textures[3] = GetTexture(FilePath + BaseFileName + "C", cManager);
        }
    }
}
