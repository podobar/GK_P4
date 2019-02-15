using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Shaders
{
    public class EntityShader : StaticShader
    {
        public EntityShader(string directoryPath)
        {
            Initialize("Shaders/" + directoryPath + "/vertexShader.glsl", "Shaders/" + directoryPath + "/fragmentShader.glsl");
        }
    }
}
