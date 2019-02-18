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
    public class EntityRenderer
    {
        
        private ShaderProgram shader;

        public EntityRenderer(ShaderProgram shader, Matrix4 projectionMatrix)
        {
            this.shader = shader;
            shader.Start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
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
        
    }
}
