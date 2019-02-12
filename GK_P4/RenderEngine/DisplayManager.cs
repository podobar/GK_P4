using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK_P4.Cameras;
using GK_P4.Entities;
using GK_P4.Lights;
using GK_P4.Models;
using GK_P4.Shaders;
using GK_P4.Terrains;
using GK_P4.UserInputHandlers;
using GK_P4.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace GK_P4.RenderEngine
{
    public class Project : GameWindow
    {
        private Loader loader = new Loader();
        private MainRenderer renderer;
        private List<Terrain> terrains = new List<Terrain>();
        private Camera camera;
        private KeyboardHandler keyboard = new KeyboardHandler();
        private MouseHandler mouse = new MouseHandler();
        private Light light = new Light(new Vector3(0, 100, 20), new Vector3(212f/255f, 175f/255f, 55f/255f));
        private string ShadingMode = "Flat";

        List<Entity> entities = new List<Entity>();
        public Project(): base(
                 1024, 768,
                 new GraphicsMode(new ColorFormat(8, 8, 8, 8)),
                 "3D Trolley",
                 GameWindowFlags.FixedWindow,
                 DisplayDevice.Default,
                 4,
                 0,
                 GraphicsContextFlags.ForwardCompatible)
        {
            GL.Enable(EnableCap.Multisample);
        }
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                generateEntities();
                generateTerrains();
                camera = new StaticCamera(new Vector3(0, 40, 40), 40, 0, 0);
                renderer = new MainRenderer();
            }
            catch(Exception ef)
            {
                Console.WriteLine(ef.Message);
            }
           
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            renderer.CleanUp();
            loader.CleanUp();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            try
            {
                camera.Move();
                foreach (var ent in entities)
                {
                    ent.IncreaseRotation(0, 0.5f, 0);
                    //ent.IncreasePosition(0, 0, 0.2f);
                    renderer.ProcessEntity(ent);
                }
                foreach (var ter in terrains)
                {
                    renderer.ProcessTerrain(ter);
                }
                //terrain = new Terrain(0, 0, loader, new ModelTexture(loader.LoadTexture("Resources/grass.png")));
                renderer.Render(light, camera);
                //renderer.Prepare();
                //shader.Start();
                //shader.LoadLight(light);
                //shader.LoadViewMatrix(camera);
                //renderer.Render(entity, shader);
                //shader.Stop();
                mouse.Reset();
                SwapBuffers();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.C:
                    {
                        if(camera is StaticCamera)
                            camera = new FollowingCamera(camera.Position,camera.Pitch,camera.Yaw,camera.Roll, entities[0]);
                        else if (camera is FollowingCamera)
                            camera = new GoProCamera(new Vector3(0, 0, 0), 30, 0, 0, mouse, entities[0]);
                        else
                            camera = new StaticCamera(new Vector3(0, 40, 40), 40, 0, 0);
                        break;
                    }
                case Key.S:
                    {
                        if (ShadingMode == "Flat")
                            ShadingMode = "Phong";
                        else if (ShadingMode == "Phong")
                            ShadingMode = "Gouraud";
                        else
                            ShadingMode = "Flat";
                        renderer.CleanUp();
                        renderer = new MainRenderer(ShadingMode);
                            break;
                    }
                case Key.E:
                    {
                        Close();
                        break;
                    }

            }

            keyboard.KeyPressed(e, true);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            keyboard.KeyPressed(e, false);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            mouse.Update(e);
        }
        private void generateEntities()
        {
            RawModel model = OBJLoader.LoadOBJModel("dragon", loader);
            TexturedModel staticModel = new TexturedModel(model, new ModelTexture(loader.LoadTexture("Resources/white.png")));
            staticModel.Texture.reflectivity = 1;
            staticModel.Texture.shineDamper = 10;
            
            entities.Add(new Entity(staticModel, new Vector3(10, 0, -15), new Vector3(0, 1f, 0), 1));
           
        }
        private void generateTerrains()
        {
            terrains.Add(new Terrain(-0.25f, -0.25f, loader, new ModelTexture(loader.LoadTexture("Resources/grass.png"))));
        }
        
    }
}
