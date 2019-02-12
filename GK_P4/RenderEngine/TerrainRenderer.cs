using GK_P4.Models;
using GK_P4.Shaders;
using GK_P4.Terrains;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.RenderEngine
{
    public class TerrainRenderer
    {
        private TerrainShader shader;

        public TerrainRenderer(TerrainShader shader, Matrix4 projectionMatrix)
        {
            this.shader = shader;
            shader.Start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
        }
        public void Render(List<Terrain> terrains)
        {
            foreach(var terrain in terrains)
            {
                prepareTerrain(terrain);
                loadModelMatrix(terrain);
                GL.DrawElements(PrimitiveType.Triangles, terrain.Model.VertexCount, DrawElementsType.UnsignedInt, 0);
                unbindTexturedModel();
                
            }
        }
        private void prepareTerrain(Terrain terrain)
        {
            RawModel rawModel = terrain.Model;
            
            GL.BindVertexArray(rawModel.VaoID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            ModelTexture texture = terrain.Texture;
            shader.LoadShineVariables(texture.shineDamper, texture.reflectivity);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, terrain.Texture.textureID);
            
        }
        private void unbindTexturedModel()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.BindVertexArray(0);
        }
        private void loadModelMatrix(Terrain terrain)
        {
            Matrix4 transformationMatrix = Utilities.Matrices.CreateTransformationMatrix(new Vector3(terrain.X,-1,terrain.Z), new Vector3(0,0,0),1);
            shader.LoadTransformationMatrix(transformationMatrix);
        }
    }
}
