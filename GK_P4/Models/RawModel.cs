using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.Models
{
    public class RawModel
    {
        public int VaoID { get; }
        public int VertexCount { get; }

        public RawModel(int vaoID, int vertexCount)
        {
            VaoID = vaoID;
            VertexCount = vertexCount;
        }
    }
}
