using GK_P4.Cameras;
using GK_P4.Utilities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Shaders
{
    public class StaticShader : ShaderProgram
    {
        private const string vFile = "Shaders/vertexShader.glsl";
        private const string fFile = "Shaders/fragmentShader.glsl";
        private int location_transformationMatrix;
        private int location_projectionMatrix;
        private int location_viewMatrix;
        public StaticShader() : base(vFile, fFile) { }

        protected override void Initialize(string vShader, string fShader)
        {
            base.Initialize(vShader, fShader);
        }
        protected override void bindAttributes()
        {
            base.bindAttribute(0, "position");
            base.bindAttribute(1, "textureCoords");
        }
        protected override void GetAllUniformLocations()
        {
            location_transformationMatrix = base.getUniformLocation("transformationMatrix");
            location_projectionMatrix = base.getUniformLocation("projectionMatrix");
            location_viewMatrix = base.getUniformLocation("viewMatrix");
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
    }
}
