using GK_P4.Cameras;
using GK_P4.Lights;
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
        private int location_lightPosition;
        private int location_lightColour;
        private int location_reflectivity;
        private int location_shineDamper;
        public StaticShader() : base(vFile, fFile) { }

        protected override void Initialize(string vShader, string fShader)
        {
            base.Initialize(vShader, fShader);
        }
        protected override void bindAttributes()
        {
            base.bindAttribute(0, "position");
            base.bindAttribute(1, "textureCoords");
            base.bindAttribute(2, "normal");
        }
        protected override void GetAllUniformLocations()
        {
            location_transformationMatrix = base.getUniformLocation("transformationMatrix");
            location_projectionMatrix = base.getUniformLocation("projectionMatrix");
            location_viewMatrix = base.getUniformLocation("viewMatrix");
            location_lightPosition = base.getUniformLocation("lightPosition");
            location_lightColour = base.getUniformLocation("lightColour");
            location_reflectivity = base.getUniformLocation("reflectivity");
            location_shineDamper = base.getUniformLocation("shineDamper");
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
        public void LoadLight(Light light)
        {
            base.LoadVector(location_lightPosition, light.Position);
            base.LoadVector(location_lightColour, light.Colour);
        }
        public void LoadShineVariables(float damper, float reflectivity)
        {
            base.LoadFloat(location_shineDamper, damper);
            base.LoadFloat(location_reflectivity, reflectivity);
        }
    }
}
