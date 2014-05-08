using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace game_objects.ui
{
    public class ProgressBar : DrawableGameObject
    {
        private Texture2D goal;
        private Texture2D barBg;
        private Texture2D barFill;
        private Texture2D barMeter;

        private Vector2 barBgPos;
        private Vector2 fillPos;
        private Vector2 meterPos;
        private Vector2 goalPos;

        private Rectangle fillTexSource;

        private int loaded;
        private int load;
        private int total;

        private float fillOpacity;

        private BarOrientation orientation;

        private string filePath;

        private bool positionsSet;

        private string meterFileName;

        public string MeterFileName
        {
            get { return meterFileName; }
            set { meterFileName = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                if (!string.IsNullOrEmpty(filePath) && filePath[filePath.Length - 1] != Path.AltDirectorySeparatorChar)
                {
                    filePath += Path.AltDirectorySeparatorChar;
                }
            }
        }

        public float FillOpacity
        {
            get { return fillOpacity; }
            set { fillOpacity = value; }
        }

        public int Loaded
        {
            get { return loaded; }
        }

        public int Total
        {
            get { return total; }
            set { total = value; }
        }

        public BarOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                positionsSet = false;
            }
        }

        public override Vector3 Dimensions
        {
            get
            {
                return base.Dimensions;
            }
            set
            {
            }
        }

        public ProgressBar(Renderer2D r2d)
            : base(r2d)
        {
            orientation = BarOrientation.HORIZONTAL;
            fillTexSource = Rectangle.Empty;
            filePath = "";
        }

        public override void Load(ContentManager cManager)
        {
            float width = 0;
            float height = 0;
            try
            {
                barBg = cManager.Load<Texture2D>(FilePath + "bar");
                width = barBg.Width;
                height = barBg.Height;

                barFill = cManager.Load<Texture2D>(FilePath + "fill");
                if (barFill.Width > width) width = barFill.Width;
                if (barFill.Height > height) height = barFill.Height;

                goal = cManager.Load<Texture2D>(FilePath + "goal");
                if (goal.Width > width) width = goal.Width;
                else if (orientation == BarOrientation.HORIZONTAL) width += goal.Width;

                if (goal.Height > height) height = goal.Height;
                else if (orientation == BarOrientation.VERTICAL)
                {
                    height += goal.Height;
                    barBgPos.Y += goal.Height;
                }
            }
            catch (FileNotFoundException ex) { }

            dimensions = new Vector3(width, height, 0);
        }

        public void LoadMeter(ContentManager cManager)
        {
            if (!string.IsNullOrEmpty(meterFileName))
            {
                barMeter = cManager.Load<Texture2D>(FilePath + meterFileName);
                if (barMeter.Width > dimensions.X) dimensions.X = barMeter.Width;
                if (barMeter.Height > dimensions.Y) dimensions.Y = barMeter.Height;
                positionsSet = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (load > 0)
            {
                loaded += load;
                load = 0;
                UpdateFill();
            }
            if (!positionsSet)
                UpdatePositions();
        }

        private void UpdateFill()
        {
            float percentageLoaded = (float)loaded / total;
            switch (orientation)
            {
                case BarOrientation.HORIZONTAL:
                    fillTexSource.Width = (int)(barFill.Width * percentageLoaded);
                    fillTexSource.Height = barFill.Height;
                    break;
                case BarOrientation.VERTICAL:
                    fillTexSource.Height = (int)(barFill.Height * percentageLoaded);
                    fillTexSource.Width = barFill.Width;
                    fillTexSource.Y = barFill.Height - fillTexSource.Height;
                    break;
            }
            PositionFill();
        }

        private void UpdatePositions()
        {
            positionsSet = true;
            switch (orientation)
            {
                case BarOrientation.HORIZONTAL:

                    barBgPos = new Vector2(position.X + dimensions.X / 2 - barBg.Width / 2, position.Y);
                    if (goal != null)
                    {
                        goalPos = new Vector2(position.X + dimensions.X - goal.Width, position.Y + (dimensions.Y - goal.Height) / 2);
                    }

                    fillPos.Y = barBgPos.Y + (barBg.Height - barFill.Height) / 2;
                    fillPos.X = barBgPos.X + (barBg.Width - barFill.Width) / 2;
                    
                    if (barMeter != null)
                        meterPos.Y = dimensions.Y + (barMeter.Height - barMeter.Height) / 2;

                    break;
                case BarOrientation.VERTICAL:

                    barBgPos = new Vector2(position.X + dimensions.X / 2 - barBg.Width / 2, position.Y);
                    if (goal != null)
                    {
                        goalPos = new Vector2(position.X + (dimensions.X - goal.Width), position.Y);
                        barBgPos.Y += goal.Height;
                    }

                    fillPos.X = barBgPos.X + (barBg.Width - barFill.Width) / 2;

                    if (barMeter != null)
                        meterPos.X = barBgPos.X + (barBg.Width - barMeter.Width) / 2;

                    break;
            }
            PositionFill();
        }

        private void PositionFill()
        {
            switch (orientation)
            {
                case BarOrientation.HORIZONTAL:

                    if (barMeter != null)
                    {
                        meterPos.X = fillPos.X + fillTexSource.Width - barMeter.Width / 2;
                    }
                    break;
                case BarOrientation.VERTICAL:

                    fillPos.Y = barBgPos.Y + barBg.Height - fillTexSource.Height - (barBg.Height - barFill.Height) / 2;
                    if (barMeter != null)
                    {
                        meterPos.Y = fillPos.Y - barMeter.Height / 2;
                    }
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                Renderer2D r2d = (Renderer2D)Renderer;
                DrawPart(barBgPos, barBg, null);
                DrawPart(fillPos, barFill, fillTexSource, fillOpacity);
                if (fillTexSource.Height > 0 && fillTexSource.Width > 0)
                    DrawPart(meterPos, barMeter, null);
                DrawPart(goalPos, goal, null);
            }
        }

        private void DrawPart(Vector2 pos, Texture2D tex, Nullable<Rectangle> source, float opacity = 1f)
        {
            if (tex != null && pos != null)
            {
                Renderer2D r2d = (Renderer2D)Renderer;
                if (source == null)
                {
                    r2d.Draw(tex, pos, Color.White*opacity, BlendState.AlphaBlend);
                }
                else
                {
                    r2d.Draw(tex, pos, (Rectangle)source, Color.White*opacity, BlendState.AlphaBlend);
                }
            }
        }

        public void AddProgress(int amount)
        {
            load += amount;
        }

        public void Reset()
        {
            loaded = 0;
            load = 0;
        }
    }
}
