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
        private Camera camera;
        private KeyboardH keyboard = new KeyboardH();
        private MouseH mouse = new MouseH();
        private string ShadingMode = "Flat";
        private Entity trolley;
        private Light reflector = new Light(
                new Vector3(0, 10, 0),
                new Vector3(0.5f, 0.75f, 0.5f),
                new Vector3(0, 0.0025f, 0),
                new Vector3(0, -1, 0),
                30);
        
        private List<Light> lights = new List<Light>()
        {
            new Light(
                new Vector3(10000,1,20),
                new Vector3(1f, 1f, 1f),
                new Vector3(0.0001f,0,0),
                new Vector3(0,1,0), 
                90),
        };
        private List<Entity> entities = new List<Entity>();
        private List<Terrain> terrains = new List<Terrain>();

        private float amplitude = 15f;
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
                camera = new FollowingCamera(new Vector3(15, 20, 30), 0, 0, 0,trolley);
                renderer = new MainRenderer("Flat");
                lights.Add(reflector);
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
                updateTrolleyPosition();
                updateSpotlight();
                camera.Move();
                foreach (var ent in entities)
                {
                    ent.IncreaseRotation(0.1f, 0.2f, 0);
                    ent.IncreasePosition(0f, 0f, 0f);
                    renderer.ProcessEntity(ent);
                }
                foreach (var ter in terrains)
                {
                    renderer.ProcessTerrain(ter);
                }
                renderer.Render(lights, camera);
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
                        if(camera is CCTVCamera)
                            camera = new FollowingCamera(new Vector3(0, 50, 40),40,0,0, trolley);
                        else if (camera is FollowingCamera)
                            camera = new GoProCamera(new Vector3(0, 0, 0), 30, 0, 0, mouse, trolley);
                        else
                            camera = new CCTVCamera(new Vector3(0, 50, 40), 40, 0, 0);
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
            RawModel model2 = OBJLoader.LoadOBJModel("sphere", loader);
            TexturedModel staticModel2 = new TexturedModel(model2, new ModelTexture(loader.LoadTexture("Resources/white.png")));
            staticModel2.Texture.reflectivity = 1;
            staticModel2.Texture.shineDamper = 10;
            trolley = new Entity(staticModel2, new Vector3(0, 5, 0), new Vector3(0, 0.52f, 0), 3);
            entities.Add(trolley);

        }
        private void generateTerrains()
        {
            terrains.Add(new Terrain(-0.5f, -0.5f, loader, new ModelTexture(loader.LoadTexture("Resources/grass.png"))));
        }
        private void updateSpotlight()
        {
            const float amplitude = 15f;
            float delta = 1f;

            //update position
            reflector.Position = trolley.position + new Vector3(0, -5, 0);
            //update direction
            if (keyboard.A_Pressed)
            {
                reflector.RelativeAngle = reflector.RelativeAngle - delta < -amplitude
                    ? -amplitude
                    : reflector.RelativeAngle - delta;
            }
            else if (keyboard.D_Pressed)
            {
                reflector.RelativeAngle = reflector.RelativeAngle + delta > amplitude
                       ? amplitude
                       : reflector.RelativeAngle + delta;
            }
            reflector.ConeOfLightDirection = Matrix3.CreateRotationY(MathHelper.DegreesToRadians(reflector.RelativeAngle)) * new Vector3(1f, 0, 0);
        }
        private void updateTrolleyPosition()
        {
            //add wind effect
            const float delta = 0.3f;
            if (keyboard.X_Pressed)
            {
                if (trolley.position.X + delta > amplitude)
                    trolley.position.X = amplitude;
                else
                {
                    trolley.position.X += delta;
                    trolley.position.Y += delta;

                }
            }
            else if (keyboard.W_Pressed)
            {
                if (trolley.position.X - delta < -amplitude)
                    trolley.position.X = -amplitude;
                else
                {
                    trolley.position.X -= delta;
                    trolley.position.Y -= delta;
                }
            }
        }
    }
}
