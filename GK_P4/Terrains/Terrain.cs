using GK_P4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Terrains
{
    
    public class Terrain
    {
        private static readonly float SIZE = 200;
        private static readonly int VERTEX_COUNT = 128;
        public RawModel Model { get; set; }
        public ModelTexture Texture { get; set; }
        public float X { get; set; }
        public float Z { get; set; }

        public Terrain(float x, float z, Loader loader, ModelTexture texture)
        {
            Texture = texture;
            X = x * SIZE;
            Z = z * SIZE;
            Model = generateTerrain(loader);
        }
        private RawModel generateTerrain(Loader loader)
        {
            int count = VERTEX_COUNT * VERTEX_COUNT;
            float[] vertices = new float[count * 3];
            float[] normals = new float[count * 3];
            float[] textureCoords = new float[count * 2];
            int[] indices = new int[6 * (VERTEX_COUNT - 1) * (VERTEX_COUNT - 1)];
            int index = 0;
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                for (int j = 0; j < VERTEX_COUNT; j++)
                {
                    vertices[index * 3] = j / ((float)VERTEX_COUNT - 1) * SIZE;
                    vertices[index * 3 + 1] = 0;
                    vertices[index * 3 + 2] = i / ((float)VERTEX_COUNT - 1) * SIZE;
                    normals[index * 3] = 0;
                    normals[index * 3 + 1] = 1;
                    normals[index * 3 + 2] = 0;
                    textureCoords[index * 2] = j / ((float)VERTEX_COUNT - 1);
                    textureCoords[index * 2 + 1] = i / ((float)VERTEX_COUNT - 1);
                    index++;
                }
            }
            index = 0;
            for (int z = 0; z < VERTEX_COUNT - 1; z++)
            {
                for (int x = 0; x < VERTEX_COUNT - 1; x++)
                {
                    int topLeft = (z * VERTEX_COUNT) + x;
                    int topRight = topLeft + 1;
                    int bottomLeft = ((z + 1) * VERTEX_COUNT) + x;
                    int bottomRight = bottomLeft + 1;
                    indices[index++] = topLeft;
                    indices[index++] = bottomLeft;
                    indices[index++] = topRight;
                    indices[index++] = topRight;
                    indices[index++] = bottomLeft;
                    indices[index++] = bottomRight;
                }
            }
            return loader.LoadToVAO(vertices, textureCoords, normals, indices);
        }
    }
}
