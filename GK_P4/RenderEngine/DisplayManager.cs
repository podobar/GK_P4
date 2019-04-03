using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        private Entity trolley;
        private Light reflector;
        
        private List<Light> lights = new List<Light>()
        {
            new Light(
                new Vector3(100,1,20),
                new Vector3(1f, 1f, 1f),
                new Vector3(0.0001f,0,0),
                new Vector3(0,1,0), 
                90),
            new Light(
                new Vector3(0, 10, 0),
                new Vector3(0.5f, 0.75f, 0.5f),
                new Vector3(0, 0.0025f, 0),
                new Vector3(0, -1, 0),
                30),
             new Light(
                new Vector3(0,100,0),
                new Vector3(139/255f, 0f, 0f),
                new Vector3(0f,0,0),
                new Vector3(0,-1,0),
                30),
            new Light(
                new Vector3(0, 10, 0),
                new Vector3(0.5f, 0.75f, 0.5f),
                new Vector3(0, 0.0025f, 0),
                new Vector3(0, -1, 0),
                30)
    };
        private List<Entity> entities = new List<Entity>();
        private List<Terrain> terrains = new List<Terrain>();
        private Stopwatch stopwatch = new Stopwatch();
        private long previousMilis = 0;
        private float theta = 0;
        private const double bunnyRadius = 5;
        private const float amplitude = 7.5f;
        private const float delta = 0.3f;
        private string ShadingMode = "Flat";
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
                camera = new FollowingCamera(new Vector3(0, 50, 40), 40, 0, 0, trolley);
                renderer = new MainRenderer("Flat");
                reflector = lights[1];
                stopwatch.Start();
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
                updateLights();
                updateTrolley();
                updateBunnies();
                camera.Move();
                foreach (var ent in entities)
                {
                    ent.IncreaseRotation(0f, 0f, 0);
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
                            camera = new FollowingCamera(new Vector3(0, 50, 40), 40, 0, 0, trolley);
                        else if (camera is FollowingCamera)
                            camera = new GoProCamera(new Vector3(0, 0, 0), 70, 0, 0, mouse, trolley);
                        else
                            camera = new CCTVCamera(new Vector3(0, 50, 40), 40, 0, 0);
                        break;
                    }
                case Key.M:
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
                case Key.KeypadPlus:
                    {
                        foreach(var light in lights)
                        {
                            float dx =0, dy=0, dz=0;
                            if (light.Attenuation.X <= 0.99)
                                dx = 0.001f;
                            if (light.Attenuation.Y <= 0.99)
                                dy = 0.001f;
                            if (light.Attenuation.Z <= 0.99)
                                dz = 0.001f;
                            light.Attenuation = light.Attenuation + new Vector3(dx, dy, dz);

                        }
                        break;
                    }
                case Key.KeypadMinus:
                    {
                        foreach (var light in lights)
                        {
                            float dx = 0, dy = 0, dz = 0;
                            if (light.Attenuation.X > 0)
                                dx = 0.001f;
                            if (light.Attenuation.Y > 0)
                                dy = 0.001f;
                            if (light.Attenuation.Z > 0)
                                dz = 0.001f;
                            light.Attenuation = light.Attenuation - new Vector3(dx, dy, dz);

                        }
                        break;
                    }
                case Key.N:
                    {
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
            RawModel model2 = OBJLoader.LoadOBJModel("dragon", loader);
            TexturedModel staticModel2 = new TexturedModel(model2, new ModelTexture(loader.LoadTexture("Resources/white.png")));
            staticModel2.Texture.reflectivity = 1;
            staticModel2.Texture.shineDamper = 10;
            trolley = new Entity(staticModel2, new Vector3(0, 5, 0), new Vector3(0, 0, 0), 3);
            entities.Add(trolley);

            RawModel bunnyModel = OBJLoader.LoadOBJModel("bunny", loader);
            TexturedModel staticModel = new TexturedModel(bunnyModel, new ModelTexture(loader.LoadTexture("Resources/green.png")));
            staticModel.Texture.reflectivity = 0.5f;
            staticModel.Texture.shineDamper =7;

            for(int i = -5; i < 5; i+=2)
                for(int j = -5; j < 5; j+=2)
                {
                    entities.Add(new Entity(staticModel, new Vector3(10*i, 5, 10*j), new Vector3(0, 0, 0), 0.5f));
                }
        }
        private void generateTerrains()
        {
            terrains.Add(new Terrain(-0.5f, -0.5f, loader, new ModelTexture(loader.LoadTexture("Resources/grass.png"))));
        }
        private void updateLights()
        {
            //update position
            reflector.Position = trolley.position + new Vector3(0, 5, 0);
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
            reflector.ConeOfLightDirection = Matrix3.CreateRotationY(MathHelper.DegreesToRadians(reflector.RelativeAngle)) * new Vector3(-1f, 0.10f, 0);
            for(int i = 2; i <= 3; ++i)
            {
                if(stopwatch.ElapsedMilliseconds-previousMilis > 30)
                {
                    theta += 0.025f;
                    float sin = (float)(Math.Sin(theta));
                    float cos = (float)(Math.Cos(theta));
                    lights[2].Position = new Vector3(20*sin, 50, 20*cos);
                    lights[3].Position = new Vector3((-25 * sin), 150f, -25 * cos);
                    lights[2].ConeOfLightDirection = new Vector3(sin, -1, cos);
                    previousMilis = stopwatch.ElapsedMilliseconds;
                }
                    
            }
        }
        private void updateTrolley()
        {
            if (keyboard.S_Pressed)
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
        private void updateBunnies()
        {
            var elapsedTime = stopwatch.Elapsed.Seconds;
            for(int i = 1; i <= 25; ++i)
            {
                if (i % 2 == 0)
                {
                    if (elapsedTime % 2 == 0)
                        entities[i].IncreasePosition(0.1f, 0, 0.1f);
                    else
                        entities[i].IncreasePosition(-0.1f, 0, - 0.1f);
                    entities[i].IncreaseRotation(0, 0.5f, 0);
                }
                else
                {
                    if (elapsedTime % 2 == 0)
                        entities[i].IncreasePosition(0.3f, 0, 0.2f);
                    else
                        entities[i].IncreasePosition(-0.3f, 0, -0.2f);
                    entities[i].IncreaseRotation(-0.4f, 0.1f, 0);
                }
            }
        }
    }
}
