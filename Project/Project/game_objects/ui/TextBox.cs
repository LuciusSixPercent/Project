using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Project;

namespace game_objects.ui
{
    public class TextBox : DrawableGameObject
    {

        private List<string> lines;
        private string text;

        private Vector2 padding;
        private float lineSpacing;
        private int size;
        private float fontScale;

        public int Size
        {
            get { return size; }
            set { 
                size = value;
                this.fontScale = ((float)size / TextHelper.FontSize);
            }
        }

        public Vector2 Padding
        {
            get { return padding; }
            set
            {
                padding = value;
                UpdateLines();
            }
        }

        public float LineSpacing
        {
            get { return lineSpacing; }
            set { lineSpacing = value; }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                SplitLines();
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
                Vector3 oldDimensions = dimensions;
                base.Dimensions = value;
                if (oldDimensions.X < dimensions.X)
                {
                    UpdateLines();
                }
                else if (oldDimensions.X > dimensions.X)
                {
                    SplitLines();
                }
            }
        }

        /// <summary>
        /// Separa o texto de acordo com as linhas predefinidas
        /// </summary>
        private void SplitLines()
        {
            lines.Clear();
            Renderer2D r2d = (Renderer2D)Renderer;
            string[] predefinedLines = text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            foreach (string predefinedLine in predefinedLines)
            {
                lines.Add(predefinedLine);
            }
            UpdateLines();
        }


        /// <summary>
        /// Ajusta as linhas para que caibam dentro das dimensões da TexBox (quebra as linhas predefinidas em mais linhas, se necessário).
        /// </summary>
        private void UpdateLines()
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (TextHelper.SpriteFont.MeasureString(lines[i]).X * fontScale > dimensions.X - 2 * padding.X)
                {

                    List<string> subLines = new List<string>();
                    subLines.Add(lines[i]);
                    lines.RemoveAt(i);

                    for (int index = 0; index < subLines.Count; index++)
                    {
                        while (TextHelper.SpriteFont.MeasureString(subLines[index]).X * fontScale > dimensions.X - 2 * padding.X)
                        {
                            int wordStartIndex = subLines[index].LastIndexOf(' ') + 1;
                            if (wordStartIndex < 0 || wordStartIndex >= subLines[index].Length) wordStartIndex = subLines[index].Length - 1;

                            if (index + 1 == subLines.Count)
                            {
                                subLines.Add(subLines[index].Substring(wordStartIndex));
                            }
                            else
                            {
                                subLines[index + 1] = subLines[index].Substring(wordStartIndex) + subLines[index + 1];
                            }
                            subLines[index] = subLines[index].Remove(wordStartIndex);
                        }
                    }
                    int newIndex = i;
                    foreach (string s in subLines)
                    {
                        if (newIndex < lines.Count)
                        {
                            lines.Insert(newIndex, s.Trim());                            
                        }
                        else
                        {
                            lines.Add(s.Trim());
                        }
                        newIndex++;
                    }
                }
            }
        }

        public TextBox(Renderer2D r2d, Vector3 dimensions)
            : base(r2d)
        {
            this.lines = new List<string>();
            this.dimensions = dimensions;
            this.Size = 20;
            this.lineSpacing = TextHelper.SpriteFont.LineSpacing;
        }

        public override void Load(ContentManager cManager)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 initialPos = new Vector2(position.X + Padding.Y, position.Y + Padding.X);
            foreach (string line in lines)
            {
                if (initialPos.Y + LineSpacing*fontScale >= dimensions.Y + position.Y) break;

                ((Renderer2D)Renderer).DrawString(line, initialPos, Color.White, 0, Vector2.Zero, fontScale);
                initialPos.Y += LineSpacing*fontScale;
            }
        }
    }
}
