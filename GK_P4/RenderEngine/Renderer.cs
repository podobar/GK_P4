using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK_P4.Entities;
using GK_P4.Models;
using GK_P4.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace GK_P4.RenderEngine
{
    public class Renderer
    {
        private const float FOV = 70;
        private const float NEAR_PLANE = 0.1f;
        private const float FAR_PLANE = 1000;
        private StaticShader shader;
        public Matrix4 ProjectionMatrix { get; set; }
        public Renderer(StaticShader shader)
        {
            this.shader = shader;
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            createProjectionMatrix();
            shader.Start();
            shader.LoadProjectionMatrix(ProjectionMatrix);
            shader.Stop();
        }
        public void Prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);
            GL.ClearColor(1f, 0f, 0f, 1f);
        }
        public void Render(Dictionary<TexturedModel, List<Entity>> entities)
        {
            foreach(var model in entities.Keys)
            {
                prepareTexturedModel(model);
                foreach(var entity in entities[model])
                {
                    prepareInstance(entity);
                    GL.DrawElements(PrimitiveType.Triangles, model.Model.VertexCount, DrawElementsType.UnsignedInt, 0);
                }
                unbindTexturedModel();
            }
        }
        private void prepareTexturedModel(TexturedModel model)
        {
            RawModel rawModel = model.Model;
            GL.BindVertexArray(rawModel.VaoID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            ModelTexture texture = model.Texture;
            shader.LoadShineVariables(texture.shineDamper, texture.reflectivity);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, model.Texture.textureID);
        }
        private void unbindTexturedModel()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.BindVertexArray(0);
        }
        private void prepareInstance(Entity entity)
        {
            Matrix4 transformationMatrix = Utilities.Matrices.CreateTransformationMatrix(entity.position, entity.rotation, entity.scale);
            shader.LoadTransformationMatrix(transformationMatrix);
        }
        public void Render(RawModel model)
        {
            GL.BindVertexArray(model.VaoID);
            GL.EnableVertexAttribArray(0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.VertexCount);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
        public void Render(TexturedModel texturedModel)
        {
            RawModel model = texturedModel.Model;
            GL.BindVertexArray(model.VaoID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.textureID);
            GL.DrawElements(PrimitiveType.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }
        //public void Render(Entity entity, StaticShader shader)
        //{
        //    TexturedModel model = entity.model;
        //    RawModel rawModel = model.Model;
        //    GL.BindVertexArray(rawModel.VaoID);
        //    GL.EnableVertexAttribArray(0);
        //    GL.EnableVertexAttribArray(1);
        //    GL.EnableVertexAttribArray(2);
        //    Matrix4 transformationMatrix = Utilities.Matrices.CreateTransformationMatrix(entity.position, entity.rotation, entity.scale);
        //    shader.LoadTransformationMatrix(transformationMatrix);

        //    ModelTexture texture = model.Texture;
        //    shader.LoadShineVariables(texture.shineDamper, texture.reflectivity);

        //    GL.ActiveTexture(TextureUnit.Texture0);
        //    GL.BindTexture(TextureTarget.Texture2D, model.Texture.textureID);
        //    GL.DrawElements(PrimitiveType.Triangles, rawModel.VertexCount, DrawElementsType.UnsignedInt, 0);
        //    GL.DisableVertexAttribArray(0);
        //    GL.DisableVertexAttribArray(1);
        //    GL.DisableVertexAttribArray(2);
        //    GL.BindVertexArray(0);
        //}
        private void createProjectionMatrix()
        {
            float aspectRatio = (float)1024 / (float)768f;
            float y_scale = (float)(1f / Math.Tan(MathHelper.DegreesToRadians(FOV / 2f)) * aspectRatio);
            float x_scale = y_scale / aspectRatio;
            float frustum_lenght = FAR_PLANE - NEAR_PLANE;

            ProjectionMatrix = new Matrix4
            {
                M11 = x_scale,
                M22 = y_scale,
                M33 = -((FAR_PLANE + NEAR_PLANE) / frustum_lenght),
                M34 = -1,
                M43 = -(2 * NEAR_PLANE * FAR_PLANE / frustum_lenght),
                M44 = 0
            };
        }
    }
}
