using GK_P4.Cameras;
using GK_P4.Entities;
using GK_P4.Lights;
using GK_P4.Models;
using GK_P4.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_P4.RenderEngine
{
    public class MainRenderer
    {
        private StaticShader shader;
        private Renderer renderer;
        private Dictionary<TexturedModel, List<Entity>> entities = new Dictionary<TexturedModel, List<Entity>>();

        public MainRenderer()
        {
            shader = new StaticShader();
            renderer = new Renderer(shader);
        }
        public void Render(Light sun, Camera camera)
        {
            renderer.Prepare();
            shader.Start();
            shader.LoadLight(sun);
            shader.LoadViewMatrix(camera);
            renderer.Render(entities);
            shader.Stop();
            entities.Clear();
        }
        public void CleanUp()
        {
            shader.CleanUp();
        }
        public void ProcessEntity(Entity entity)
        {
            TexturedModel entityModel = entity.model;
            entities.TryGetValue(entityModel, out List<Entity>sameModel);
            if (sameModel != null)
                sameModel.Add(entity);
            else
                entities.Add(entityModel, new List<Entity>(new Entity[] { entity }));
                

        }
    }
}
