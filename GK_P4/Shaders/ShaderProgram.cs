using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace GK_P4.Shaders
{
    public abstract class ShaderProgram
    {
        private int programID;
        private int vertexShaderID;
        private int fragmentShaderID;
        public ShaderProgram(string vFile, string fFile)
        {
            vertexShaderID = loadShader(vFile, ShaderType.VertexShader);
            fragmentShaderID = loadShader(fFile, ShaderType.FragmentShader);
            programID = GL.CreateProgram();
            GL.AttachShader(programID, vertexShaderID);
            GL.AttachShader(programID, fragmentShaderID);
            bindAttributes();
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);
            GetAllUniformLocations();
        }
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
        protected abstract void bindAttributes();
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
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);
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
        protected abstract void GetAllUniformLocations();
        protected void LoadFloat(int location, float value)
        {
            GL.Uniform1(location, value);
        }

        protected void LoadInt(int location, int value)
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

        protected void LoadBoolean(int location, bool value)
        {
            float toLoad = value == true ? 1 : 0;
            GL.Uniform1(location, toLoad);
        }

        protected void LoadMatrix(int location, Matrix4 matrix)
        {
            GL.UniformMatrix4(location, false, ref matrix);
        }
        #endregion
    }
}
