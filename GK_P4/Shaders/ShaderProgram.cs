using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using GK_P4.Cameras;
using GK_P4.Utilities;
using GK_P4.Lights;
using System.Collections.Generic;

namespace GK_P4.Shaders
{
    public abstract class ShaderProgram
    {
        private const int LIGHT_COUNT = 2;
        private int programID;
        private int vertexShaderID;
        private int fragmentShaderID;

        private int location_transformationMatrix;
        private int location_projectionMatrix;
        private int location_viewMatrix;
        private int location_reflectivity;
        private int location_shineDamper;
        private int location_fogColour;

        private int[] location_lightPositions;
        private int[] location_lightColours;
        private int[] location_coneOfLightAngles;
        private int[] location_attenuations;
        private int[] location_coneOfLightDirections;
        public void Start()
        {
            GL.UseProgram(programID);
        }
        public void Stop()
        {
            GL.UseProgram(0);
        }

        public void CleanUp()
        {
            Stop();
            GL.DetachShader(programID, vertexShaderID);
            GL.DetachShader(programID, fragmentShaderID);
            GL.DeleteShader(vertexShaderID);
            GL.DeleteShader(fragmentShaderID);
            GL.DeleteProgram(programID);
        }
        protected virtual void bindAttributes()
        {
            bindAttribute(0, "position");
            bindAttribute(1, "textureCoords");
            bindAttribute(2, "normal");
        }
        protected void bindAttribute(int attribute, string variableName)
        {
            GL.BindAttribLocation(programID, attribute, variableName);
        }

        protected virtual void Initialize(string vShader, string fShader)
        {
            vertexShaderID = loadShader(vShader, ShaderType.VertexShader);
            fragmentShaderID = loadShader(fShader, ShaderType.FragmentShader);
            programID = GL.CreateProgram();
            GL.AttachShader(programID, vertexShaderID);
            GL.AttachShader(programID, fragmentShaderID);
            bindAttributes();
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);
            GetAllUniformLocations();
        }
        private static int loadShader(string file, ShaderType type)
        {
            int shaderID = GL.CreateShader(type);
            GL.ShaderSource(shaderID, File.ReadAllText(file));
            GL.CompileShader(shaderID);

            string msg = GL.GetShaderInfoLog(shaderID);
            if (msg != string.Empty)
            {
                Console.WriteLine(msg);
                throw new Exception(msg);
            }

            return shaderID;
        }
        #region Uniforms
        protected int getUniformLocation(string uniName)
        {
            return GL.GetUniformLocation(programID, uniName);
        }
        protected virtual void GetAllUniformLocations()
        {
            location_transformationMatrix = getUniformLocation("transformationMatrix");
            location_projectionMatrix = getUniformLocation("projectionMatrix");
            location_viewMatrix = getUniformLocation("viewMatrix");
            location_reflectivity = getUniformLocation("reflectivity");
            location_shineDamper = getUniformLocation("shineDamper");
            location_fogColour = getUniformLocation("fogColour");

            location_lightPositions = new int[LIGHT_COUNT];
            location_lightColours = new int[LIGHT_COUNT];
            location_coneOfLightAngles = new int[LIGHT_COUNT];
            location_attenuations = new int[LIGHT_COUNT];
            location_coneOfLightDirections = new int[LIGHT_COUNT];
            
            
            for (int i = 0; i < LIGHT_COUNT; i++)
            {
                location_lightPositions[i] = getUniformLocation("lightPositions[" + i + "]");
                location_lightColours[i] = getUniformLocation("lightColours[" + i + "]");
                location_attenuations[i] = getUniformLocation("attenuations[" + i + "]");
                location_coneOfLightDirections[i] = getUniformLocation("coneOfLightDirections[" + i + "]");
                location_coneOfLightAngles[i] = getUniformLocation("coneOfLightAngles[" + i + "]");
            }
        }
        protected void LoadFloat(int location, float value)
        {
            GL.Uniform1(location, value);
        }

        protected void LoadVector(int location, Vector3 vector)
        {
            GL.Uniform3(location, vector);
        }

        protected void LoadVector(int location, Vector2 vector)
        {
            GL.Uniform2(location, vector);
        }
        protected void LoadMatrix(int location, Matrix4 matrix)
        {
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void LoadFogColour(float r, float g, float b)
        {
            LoadVector(location_fogColour, new Vector3(r, g, b));
        }
        public void LoadTransformationMatrix(Matrix4 matrix)
        {
            LoadMatrix(location_transformationMatrix, matrix);
        }
        public void LoadProjectionMatrix(Matrix4 matrix)
        {
            LoadMatrix(location_projectionMatrix, matrix);
        }
        public void LoadViewMatrix(Camera camera)
        {
            Matrix4 viewMatrix = Matrices.CreateViewMatrix(camera);
            LoadMatrix(location_viewMatrix, viewMatrix);
        }
        public void LoadLights(List<Light> lights)
        {
            for (int i = 0; i < LIGHT_COUNT; ++i)
            {
                LoadVector(location_lightPositions[i], lights[i].Position);
                LoadVector(location_lightColours[i], lights[i].Colour);
                LoadFloat(location_coneOfLightAngles[i], lights[i].ConeOfLightAngle);
                LoadVector(location_attenuations[i], lights[i].Attenuation);
                LoadVector(location_coneOfLightDirections[i], lights[i].ConeOfLightDirection);
            }
        }
        public void LoadShineVariables(float damper, float reflectivity)
        {
            LoadFloat(location_shineDamper, damper);
            LoadFloat(location_reflectivity, reflectivity);
        }
        #endregion
    }
}
