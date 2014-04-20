using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace game_objects.questions
{
    public class Question : CollidableGameObject
    {
        private QuestionSubject type;
        private string[] answers;

        private int correctAnswerIndex;
        private string header;

        private Quad answerQuad;

        private Texture2D texture;

        private bool quadsCreated;

        private float answersPadding = 1;
        private SpriteFont spriteFont;
        private Renderer2D secondaryRenderer;

        private const float scale = 3f;

        public string[] Answers
        {
            get { return answers; }
        }

        public Quad AnswerQuad
        {
            get
            {
                if (!quadsCreated)
                    createQuads();
                return answerQuad;
            }
        }

        public Question(Renderer3D renderer, Renderer2D secondaryRenderer, QuestionSubject type, string header, string[] answers, int correctAnswerIndex)
            : base(renderer)
        {
            this.type = type;
            this.header = header;
            this.answers = answers;
            this.correctAnswerIndex = correctAnswerIndex;
            this.secondaryRenderer = secondaryRenderer;
        }

        private void createQuads()
        {
            Vector3 nrm = new Vector3(0, 0, -1);
            quadsCreated = true;
            float proportion = (float)texture.Height / texture.Width;
            answerQuad = new Quad(position, nrm, Vector3.Up, scale, scale * proportion);

            Vector3 backUpperLeft = answerQuad.Vertices[1].Position;

            Vector3 frontBottomRight = answerQuad.Vertices[2].Position;

            BoundingBox = new BoundingBox(frontBottomRight, backUpperLeft);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Translate(Vector3 amount)
        {
            base.Translate(amount);
            answerQuad.Translate(amount);
            boundingBox.Max += amount;
            boundingBox.Min += amount;
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
                createQuads();
            }
        }

        public override void Load(ContentManager cManager)
        {
            spriteFont = cManager.Load<SpriteFont>("Fonte" + Path.AltDirectorySeparatorChar + "Verdana");

            PrepareTexture(cManager);
        }

        private void PrepareTexture(ContentManager cManager)
        {
            Vector2 totalSize = getAnswerSize();
            
            answersPadding = totalSize.X / 3;

            totalSize.X += answersPadding * 6;

            texture = CreateTexture(totalSize);
        }

        private Texture2D CreateTexture(Vector2 totalSize)
        {
            RenderTarget2D rt2D = secondaryRenderer.CreateRenderTarget((int)totalSize.X, (int)totalSize.Y);
            secondaryRenderer.SetRenderTarget(rt2D, true);

            Vector2 position = Vector2.Zero;
            foreach (string s in answers)
            {
                position.X += answersPadding;
                secondaryRenderer.DrawString(null, spriteFont, s, position, Color.White, BlendState.AlphaBlend, true);
                position.X += spriteFont.MeasureString(s).X + answersPadding;
            }
            secondaryRenderer.End();
            secondaryRenderer.SetRenderTarget(null, true);
            return (Texture2D)rt2D;
        }

        private Vector2 getAnswerSize()
        {
            Vector2 totalSize = new Vector2(0, 0);
            foreach (string s in answers)
            {
                Vector2 size = spriteFont.MeasureString(s);
                totalSize.X += size.X;
                if (totalSize.Y < size.Y)
                    totalSize.Y = size.Y;
            }
            return totalSize;
        }

        public override void Draw(GameTime gameTime)
        {
            ((Renderer3D)Renderer).Draw(gameTime, texture, AnswerQuad, BlendState.AlphaBlend);
        }

        public bool CorrecAnswerAxis(float x)
        {
            int answerIndex = 1;
            if (x >= position.X + scale / 6)
            {
                answerIndex = 0;
            }
            else if (x <= position.X - scale / 6)
            {
                answerIndex = 2;
            }

            return answerIndex == correctAnswerIndex;
        }
    }
}
