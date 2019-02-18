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
    public class TerrainShader : ShaderProgram
    {
       
        public TerrainShader(string directoryPath)
        {
            Initialize(
                "Shaders/" + directoryPath + "/terrainVertexShader.glsl",
                "Shaders/" + directoryPath + "/terrainFragmentShader.glsl");
        }
        protected override void bindAttributes()
        {
            base.bindAttributes();
        }
        protected override void GetAllUniformLocations()
        {
            base.GetAllUniformLocations();
        }
        }
    }
