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
        private AlphaTestEffect basicEffect;
        private BoundingFrustum frustum;
        private Camera cam;
        private Quad[] singleQuad;
        private SamplerState sampler;
        public static readonly BoundingBox DEFAULT_BOX = new BoundingBox(Vector3.Zero, Vector3.Zero);

        public Camera Cam
        {
            get { return cam; }
            set
            {
                cam = value;
                cam.cam_moved += new Camera.CameraMoved(cam_cam_moved);
                changeFog((float)cam.FarView - 5, cam.FarView);
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
            singleQuad = new Quad[1];
            sampler = new SamplerState();
            sampler.Filter = TextureFilter.Linear;
            sampler.MipMapLevelOfDetailBias = -0.5f;
        }

        void cam_cam_moved()
        {
            updateEffect(cam.View, cam.Projection);
        }

        private void initEffect()
        {
            basicEffect = new AlphaTestEffect(GDevice);
            basicEffect.FogEnabled = true;
            basicEffect.FogColor = Color.Gray.ToVector3();
            basicEffect.World = Matrix.Identity;
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

        public void Draw(Texture2D texture, IEnumerable<Quad> quads, BlendState blendState, BoundingBox bbox, SamplerState sampler = null)
        {
            if (texture != null && !texture.IsDisposed)
            {
                //necessário para que os objetos "3D" sejam desenhados na ordem correta
                GDevice.DepthStencilState = DepthStencilState.Default;
                GDevice.BlendState = blendState;
                basicEffect.Texture = texture;
                if (sampler != null)
                {
                    gDevice.SamplerStates[0] = sampler;
                }
                else
                {
                    gDevice.SamplerStates[0] = this.sampler;
                }

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (Quad quad in quads)
                    {
                        ContainmentType containment = ContainmentType.Intersects;
                        if (!DEFAULT_BOX.Equals(bbox))
                        {
                            containment = frustum.Contains(bbox);
                        }
                        bool visibleToCamera = containment != ContainmentType.Disjoint;
                        if(visibleToCamera)
                            drawQuad(quad);
                    }
                }
            }
        }

        public void Draw(Texture2D texture, Quad quad, BlendState blendState, BoundingBox bbox)
        {
            singleQuad[0] = quad;
            Draw(texture, singleQuad, blendState, bbox);
        }

        public void Draw(Texture2D texture, Quad quad, BlendState blendState, BoundingBox bbox, SamplerState ss = null)
        {
            singleQuad[0] = quad;
            Draw(texture, singleQuad, blendState, bbox);
        }

        private void drawQuad(Quad quad)
        {
            GDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quad.Vertices, 0, 4,
                    Quad.Indexes, 0, 2);
        }
    }
}
