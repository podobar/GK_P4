using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Shaders
{
    public class EntityShader : ShaderProgram
    {
        public EntityShader(string directoryPath)
        {
            Initialize("Shaders/" + directoryPath + "/vertexShader.glsl", "Shaders/" + directoryPath + "/fragmentShader.glsl");
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
