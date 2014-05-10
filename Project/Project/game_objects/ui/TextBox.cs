using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Project;

namespace game_objects.ui
{
    public enum DisplayType
    {
        ALL,
        WORD_BY_WORD,
        LETTER_BY_LETTER,
        LINE_BY_LINE
    }

    public class TextBox : DrawableGameObject
    {

        private List<string> lines;
        private string text;
        private DisplayType display;
        private int displayDelay;
        private int elapsed;

        private Vector2 padding;
        private float lineSpacing;
        private int fontSize;
        private float fontScale;

        private int lastVisibleCharIndex;
        private int lastVisibleLine;
        private int charCount;

        private bool lineWrappingPerformed;
        private bool basicLineSplitPerformed;

        public DisplayType Display
        {
            get { return display; }
            set { display = value; }
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
                basicLineSplitPerformed = false;
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
                if (oldDimensions.X != dimensions.X)
                {
                    basicLineSplitPerformed = false;
                }
            }
        }
        public TextBox(Renderer2D r2d)
            : base(r2d)
        {
            this.lines = new List<string>();
            this.FontSize = 20;
            this.lineSpacing = TextHelper.SpriteFont.LineSpacing;
            this.display = DisplayType.ALL;
            this.displayDelay = 500;
        }

        public override void Load(ContentManager cManager)
        {

        }

        /// <summary>
        /// Separa o texto de acordo com as linhas predefinidas
        /// </summary>
        private void SplitLines()
        {
            lines.Clear();
            lastVisibleCharIndex = 0;
            lastVisibleLine = 0;

            Renderer2D r2d = (Renderer2D)Renderer;
            string[] predefinedLines = text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            foreach (string predefinedLine in predefinedLines)
            {
                lines.Add(predefinedLine);
                charCount += predefinedLine.Length;
            }
            basicLineSplitPerformed = true;
            lineWrappingPerformed = false;
        }


        /// <summary>
        /// Ajusta as linhas para que caibam dentro das dimensões da TexBox (quebra as linhas predefinidas em mais linhas, se necessário).
        /// </summary>
        private void UpdateLines()
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (TextHelper.SpriteFont.MeasureString(lines[i]).X * fontScale > Width - 2 * padding.X)
                {

                    List<string> subLines = new List<string>();
                    subLines.Add(lines[i]);
                    lines.RemoveAt(i);

                    for (int index = 0; index < subLines.Count; index++)
                    {
                        while (TextHelper.SpriteFont.MeasureString(subLines[index]).X * fontScale > Width - 2 * padding.X)
                        {
                            int wordStartIndex = subLines[index].LastIndexOf(' ');

                            if (wordStartIndex < 0 || wordStartIndex >= subLines[index].Length) 
                                wordStartIndex = subLines[index].Length - 1;

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
                    AddWrappedLines(i, subLines);
                }
            }
        }

        private void AddWrappedLines(int newIndex, IEnumerable<string>subLines)
        {
            bool removedWhiteSpace = false;
            foreach (string s in subLines)
            {
                string newLine = s;
                if (s.IndexOf(' ') == 0 && newIndex > 0)
                {
                    removedWhiteSpace = true;
                    newLine = s.Substring(1);
                }
                if (removedWhiteSpace)
                {
                    lines[newIndex - 1] += ' ';
                    removedWhiteSpace = false;
                }

                if (newIndex < lines.Count)
                {
                    lines.Insert(newIndex, newLine);
                }
                else
                {
                    lines.Add(newLine);
                }
                newIndex++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!basicLineSplitPerformed)
            {
                SplitLines();
            }

            if (!lineWrappingPerformed)
            {
                UpdateLines();
            }
            if (Display != DisplayType.ALL && lastVisibleCharIndex < charCount)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsed > displayDelay)
                {
                    elapsed = 0;
                    UpdateVisibleChars();
                }
            }
        }

        private void UpdateVisibleChars()
        {
            int chars = 0;
            for (int lineIndex = 0; lineIndex < lastVisibleLine; lineIndex++)
            {
                chars += lines[lineIndex].Length;
            }
            switch (Display)
            {
                case DisplayType.LETTER_BY_LETTER:
                    lastVisibleCharIndex++;
                    if (chars < lastVisibleCharIndex)
                        lastVisibleLine++;

                    break;
                case DisplayType.WORD_BY_WORD:

                    int index = lastVisibleCharIndex;
                    if(lastVisibleLine > 0)
                        index -= chars;

                    string remainingCharsOnLine = lines[lastVisibleLine].Substring(index);
                    string newWord = "";
                    foreach (char c in remainingCharsOnLine)
                    {
                        newWord += c;
                        lastVisibleCharIndex++;
                        if (c == ' ')
                            break;
                    }
                    if (chars + lines[lastVisibleLine].Length <= lastVisibleCharIndex)
                        lastVisibleLine++;

                    break;
                case DisplayType.LINE_BY_LINE:
                    if (lastVisibleLine < lines.Count)
                    {
                        lastVisibleCharIndex += (lines[lastVisibleLine].Length);
                        lastVisibleLine++;
                    }
                    break;
                    
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 initialPos = new Vector2(X + Padding.X, Y + Padding.Y);
            int charCount = 0;
            bool keepGoing = true;
            for (int lineIndex = 0; lineIndex < lines.Count && keepGoing; lineIndex++)
            {
                string line = lines[lineIndex];

                charCount += line.Length;

                string visibleLine;

                if (lastVisibleCharIndex < charCount && Display != DisplayType.ALL)
                {
                    visibleLine = line.Substring(0, line.Length - (charCount - lastVisibleCharIndex));
                    keepGoing = false;
                }
                else
                {
                    visibleLine = line;
                }

                ((Renderer2D)Renderer).DrawString(visibleLine, initialPos, Color.White, 0, Vector2.Zero, fontScale);
                initialPos.Y += LineSpacing * fontScale;

                if (initialPos.Y >= Height + Y)
                    keepGoing = false; ;
            }
        }
    }
}
