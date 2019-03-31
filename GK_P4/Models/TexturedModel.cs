using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Models
{
    public class TexturedModel
    {
        public RawModel Model { get; }
        public ModelTexture Texture { get;  }

        public float Height { get; }
        public TexturedModel(RawModel model, ModelTexture texture)
        {
            Model = model;
            Texture = texture;
        }
    }
}
