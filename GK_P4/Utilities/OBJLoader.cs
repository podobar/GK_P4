using GK_P4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using System.Globalization;
namespace GK_P4.Utilities
{
    public class OBJLoader
    {
        public static RawModel LoadOBJModel(string file, Loader loader)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();
            float[] verticesArray = null;
            float[] normalsArray = null;
            float[] textureArray = null;
            int[] indicesArray = null;

            using (var streamReader = new StreamReader("Resources/" + file + ".obj"))
            {
                string line = "";
                while (streamReader.EndOfStream == false)
                {
                    if (streamReader.Peek() == 'f')
                    {
                        textureArray = new float[vertices.Count * 2];
                        normalsArray = new float[vertices.Count * 3];
                        break;
                    }
                        
                    line = streamReader.ReadLine();
                    var tokens = line.Split(' ');

                    if (line.StartsWith("v "))
                    {
                        Vector3 vertex = new Vector3(
                            float.Parse(tokens[1], CultureInfo.InvariantCulture),
                            float.Parse(tokens[2], CultureInfo.InvariantCulture),
                            float.Parse(tokens[3], CultureInfo.InvariantCulture));
                        vertices.Add(vertex);
                    }
                    else if (line.StartsWith("vt "))
                    {
                        Vector2 texture = new Vector2(
                            float.Parse(tokens[1], CultureInfo.InvariantCulture),
                            float.Parse(tokens[2], CultureInfo.InvariantCulture));
                        textures.Add(texture);
                    }
                    else if (line.StartsWith("vn "))
                    {
                        Vector3 normal = new Vector3(
                            float.Parse(tokens[1], CultureInfo.InvariantCulture),
                            float.Parse(tokens[2], CultureInfo.InvariantCulture),
                            float.Parse(tokens[3], CultureInfo.InvariantCulture));
                        normals.Add(normal);
                    }
                }
                while (streamReader.EndOfStream == false)
                {
                    line = streamReader.ReadLine();
                    if (line.StartsWith("f ") == false)
                    {
                        line = streamReader.ReadLine();
                        continue;
                    }
                    var tokens = line.Split(' ');
                    processVertex(tokens[1].Split('/'), indices, textures, normals, textureArray, normalsArray);
                    processVertex(tokens[2].Split('/'), indices, textures, normals, textureArray, normalsArray);
                    processVertex(tokens[3].Split('/'), indices, textures, normals, textureArray, normalsArray);
                }
            }
            verticesArray = new float[vertices.Count * 3];
            indicesArray = new int[indices.Count];
            int i = 0;
            foreach (var vertex in vertices)
            {
                verticesArray[i++] = vertex.X;
                verticesArray[i++] = vertex.Y;
                verticesArray[i++] = vertex.Z;

            }
            for(int j = 0; j < indices.Count; ++j)
            {
                indicesArray[j] = indices[j];
            }
            return loader.LoadToVAO(verticesArray, textureArray, normalsArray, indicesArray);
        }
        private static void processVertex(string[]vertexData, List<int> indices, List<Vector2> textures, List<Vector3> normals, float[] textureArray, float[] normalsArray)
        {
            try
            {
                int currentVertex = int.Parse(vertexData[0]) - 1;
                indices.Add(currentVertex);
                Vector2 currentTexture = textures[int.Parse(vertexData[1]) - 1];
                textureArray[currentVertex * 2] = currentTexture.X;
                textureArray[currentVertex * 2 + 1] = 1 - currentTexture.Y;
                Vector3 currentNorm = normals[int.Parse(vertexData[2]) - 1];
                normalsArray[currentVertex * 3] = currentNorm.X;
                normalsArray[currentVertex * 3 + 1] = currentNorm.Y;
                normalsArray[currentVertex * 3 + 2] = currentNorm.Z;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
