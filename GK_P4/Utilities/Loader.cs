using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace GK_P4.Models
{
    public class Loader
    {
        private List<int> vaos = new List<int>();
        private List<int> vbos = new List<int>();
        private List<int> textures = new List<int>();
        public RawModel LoadToVAO(float[] positions, float[] textureCoords, int[] indices)
        {
            int vaoID = createVAO();
            bindIndicesBuffer(indices);
            storeDataInAttributeList(0, 3, positions);
            storeDataInAttributeList(1, 2, textureCoords);

            unbindVAO();
            return new RawModel(vaoID, indices.Length);
        }
        
        //TO DO: check if everything is fine
        public int LoadTexture(string filePath)
        {
            Texture texture = new Texture();
            using (Bitmap bmp = (Bitmap)Image.FromFile(filePath))
            {
                texture.Width = bmp.Width;
                texture.Height = bmp.Height;
                texture.bitmap3 = new float[sizeof(float) * texture.Width * texture.Height * 3 ];
                int i = 0;
                for (int y = 0; y < texture.Height; ++y)
                    for (int x = 0; x < texture.Width; ++x)
                    {
                        Color color = bmp.GetPixel(x, y);
                        texture.bitmap3[i++] = color.R / 255f;
                        texture.bitmap3[i++] = color.G / 255f;
                        texture.bitmap3[i++] = color.B / 255f;
                        texture.bitmap3[i++] = color.A / 255f;
                    }
            }

            GL.CreateTextures(TextureTarget.Texture2D, 1, out int textureID);
            GL.TextureStorage2D(textureID, 5, SizedInternalFormat.Rgba32f, texture.Width, texture.Height);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TextureSubImage2D(textureID, 0, 0, 0, texture.Width, texture.Height, PixelFormat.Rgba, PixelType.Float, texture.bitmap3);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TextureParameter(textureID, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

            GL.GetFloat((GetPName)OpenTK.Graphics.ES30.ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out float maxAnsio);
            GL.TextureParameter(textureID, (TextureParameterName)OpenTK.Graphics.ES30.ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAnsio);

            textures.Add(textureID);
            return textureID;
        }
        public void CleanUp()
        {

            foreach (int vao in vaos)
            {
                GL.DeleteVertexArray(vao);
            }
            foreach (int vbo in vbos)
            {
                GL.DeleteBuffer(vbo);
            }
            foreach(var texture in textures)
            {
                GL.DeleteTexture(texture);
            }
        }
        private int createVAO()
        {
            int vaoID = GL.GenVertexArray();
            vaos.Add(vaoID);
            GL.BindVertexArray(vaoID);
            return vaoID;
        }
        private void storeDataInAttributeList(int attributeNumber, int dimensions, float[] data)
        {
            int vboID = GL.GenBuffer();
            vbos.Add(vboID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * data.Length, data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, dimensions, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        private void unbindVAO()
        {
            GL.BindVertexArray(0);
        }
        private void bindIndicesBuffer(int[] indices)
        {
            int vboID = GL.GenBuffer();
            vbos.Add(vboID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);
        }
    }
}
