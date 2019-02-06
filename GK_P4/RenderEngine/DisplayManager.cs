using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK_P4.Cameras;
using GK_P4.Entities;
using GK_P4.Models;
using GK_P4.Shaders;
using GK_P4.UserInputHandlers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace GK_P4.RenderEngine
{
    public class Game : GameWindow
    {
        private Loader loader = new Loader();
        private Renderer renderer;
        private TexturedModel txtmodel;
        private StaticShader shader;
        private Camera camera;
        private KeyboardHandler keyboard = new KeyboardHandler();
        Entity entity;
        public Game()
            : base(
                 1024, 768,
                 new GraphicsMode(new ColorFormat(8, 8, 8, 8)),
                 "3D Trolley",
                 GameWindowFlags.Default,
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
                shader = new StaticShader();
                renderer = new Renderer(shader);
                camera = new Camera(new Vector3(0, 0, 0), 0, 0, 0, keyboard);
            }
            catch(Exception ef)
            {
                Console.WriteLine(ef.Message);
            }
            GenerateEntities();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            try
            {
                entity.IncreaseRotation(0,0.5f,0);
                camera.Move();
                renderer.Prepare();
                shader.Start();
                shader.LoadViewMatrix(camera);
                renderer.Render(entity, shader);
                shader.Stop();
                SwapBuffers();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            keyboard.KeyPressed(e, true);
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            keyboard.KeyPressed(e, false);
        }
        private void GenerateEntities()
        {
            float[] vertices =
            {
                -0.5f, 0.5f, 0f,
                -0.5f, -0.5f, 0f,
                0.5f, -0.5f, 0f,
                0.5f, 0.5f, 0f
            };
            int[] indices =
            {
                0,1,3,
                2,1,3
                
            };
            float[] textureCoords =
            {
                0,0,
                0,1,
                1,1,
                1,0
            };
            
            RawModel model = loader.LoadToVAO(vertices, textureCoords, indices);
            ModelTexture texture = new ModelTexture(loader.LoadTexture("Resources/john.png"));
            txtmodel = new TexturedModel(model, texture);
            entity = new Entity(txtmodel, new Vector3(0,0,-1),new Vector3(0,0,0),1);
        }
        
    }
}
