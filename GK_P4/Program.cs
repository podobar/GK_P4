using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK_P4.RenderEngine;
namespace GK_P4
{
    static class Program
    {
        static void Main()
        {
            try
            {
                new Project().Run(120);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
