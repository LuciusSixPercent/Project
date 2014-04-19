using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project;

namespace game_objects
{
    public class Renderer3D : Renderer
    {
        BasicEffect basicEffect;
        Camera cam;
        private BoundingFrustum frustum;

        public Camera Cam
        {
            get { return cam; }
            set { 
                cam = value;
                cam.cam_moved += new Camera.CameraMoved(cam_cam_moved);
                changeFog(cam.NearView, cam.FarView);
                updateEffect(cam.View, cam.Projection);
            }
        }

        public override float Alpha
        {
            get
            {
                return base.Alpha;
            }
            set
            {
                base.Alpha = value;
                basicEffect.Alpha = value;
            }
        }

        public Renderer3D(GraphicsDevice gDevice)
            : base(gDevice)
        {
            initEffect();
        }

        void cam_cam_moved()
        {
            updateEffect(cam.View, cam.Projection);
        }

        private void initEffect()
        {
            basicEffect = new BasicEffect(gDevice);
            basicEffect.FogEnabled = true;
            basicEffect.FogColor = Color.Gray.ToVector3();
            basicEffect.World = Matrix.Identity;
            basicEffect.TextureEnabled = true;
        }

        public void updateEffect(Matrix view, Matrix projection)
        {
            basicEffect.View = view;
            basicEffect.Projection = projection;
            frustum = new BoundingFrustum(basicEffect.View * basicEffect.Projection);
        }

        public void changeFog(float start, float end)
        {
            basicEffect.FogStart = start;
            basicEffect.FogEnd = end;
        }

        public void Draw(GameTime gameTime, Texture2D texture, IEnumerable<Quad> quads, BlendState blendState)
        {
            gDevice.BlendState = blendState;
            basicEffect.Texture = texture;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (Quad quad in quads)
                {
                    drawQuad(quad);
                }
            }
        }

        public void Draw(GameTime gameTime, Texture2D texture, Quad quad, BlendState blendState)
        {
            gDevice.BlendState = blendState;
            basicEffect.Texture = texture;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                drawQuad(quad);
            }
        }

        private void drawQuad(Quad quad)
        {
            BoundingBox box = new BoundingBox(quad.Vertices[1].Position, quad.Vertices[2].Position);
            if (frustum.Contains(box) != ContainmentType.Disjoint)
            {
                gDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quad.Vertices, 0, 4,
                    Quad.Indexes, 0, 2);
            }
        }
    }
}
