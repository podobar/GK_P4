using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Models
{
    public class ModelTexture
    {
        public int textureID { get; }
        public float shineDamper { get; set; }
        public float reflectivity { get; set; }

        public ModelTexture(int id)
        {
            textureID = id;
            shineDamper = 1;
            reflectivity = 0;
        }
    }
}
