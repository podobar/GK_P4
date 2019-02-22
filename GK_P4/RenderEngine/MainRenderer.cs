using GK_P4.Cameras;
using GK_P4.Entities;
using GK_P4.Lights;
using GK_P4.Models;
using GK_P4.Shaders;
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using GK_P4.Terrains;

namespace GK_P4.RenderEngine
{
    public class MainRenderer
    {
        private const float FOV = 70;
        private const float NEAR_PLANE = 0.1f;
        private const float FAR_PLANE = 1000;
        private const float R = 0.5f, G = 0.3f, B = 0.5f;
        public Matrix4 ProjectionMatrix { get; set; }
        private EntityRenderer renderer;
        private EntityShader entityShader;
        private Dictionary<TexturedModel, List<Entity>> entities = new Dictionary<TexturedModel, List<Entity>>();
        private List<Terrain> terrains = new List<Terrain>();
        private TerrainRenderer terrainRenderer;
        private TerrainShader terrainShader;
        public MainRenderer(string shadingMode)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            
            terrainShader = new TerrainShader(shadingMode);
            entityShader = new EntityShader(shadingMode);

            createProjectionMatrix();

            renderer = new EntityRenderer(entityShader, ProjectionMatrix);
            terrainRenderer = new TerrainRenderer(terrainShader, ProjectionMatrix);
        }
        public MainRenderer()
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            createProjectionMatrix();
            entityShader = new EntityShader("Flat");
            renderer = new EntityRenderer(entityShader,ProjectionMatrix);
            terrainRenderer = new TerrainRenderer(terrainShader, ProjectionMatrix);
            terrainShader = new TerrainShader("Flat");
        }
        public void Render(Light sun, Camera camera)
        {
            Prepare();
            entityShader.Start();
            entityShader.LoadFogColour(R, G, B);
            entityShader.LoadLight(sun);
            entityShader.LoadViewMatrix(camera);
            renderer.Render(entities);
            entityShader.Stop();
            terrainShader.Start();
            terrainShader.LoadFogColour(R, G, B);
            terrainShader.LoadLight(sun);
            terrainShader.LoadViewMatrix(camera);
            terrainRenderer.Render(terrains);
            terrainShader.Stop();
            terrains.Clear();
            entities.Clear();
        }
        public void Render(List<Light> lights, Camera camera)
        {
            Prepare();
            entityShader.Start();
            entityShader.LoadFogColour(R, G, B);
            entityShader.LoadLights(lights);
            entityShader.LoadViewMatrix(camera);
            renderer.Render(entities);
            entityShader.Stop();
            terrainShader.Start();
            terrainShader.LoadFogColour(R, G, B);
            terrainShader.LoadLights(lights);
            terrainShader.LoadViewMatrix(camera);
            terrainRenderer.Render(terrains);
            terrainShader.Stop();
            terrains.Clear();
            entities.Clear();
        }
        public void Prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(R,G,B, 1f);
        }
        public void CleanUp()
        {
            entityShader.CleanUp();
            terrainShader.CleanUp();
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
        public void ProcessTerrain(Terrain terrain)
        {
            terrains.Add(terrain);
        }
        private void createProjectionMatrix()
        {
            float aspectRatio = (float)1024 / (float)768f;
            float y_scale = (float)(1f / Math.Tan(MathHelper.DegreesToRadians(FOV / 2f)) * aspectRatio);
            float x_scale = y_scale / aspectRatio;
            float frustum_lenght = FAR_PLANE - NEAR_PLANE;

            ProjectionMatrix = new Matrix4
            {
                M11 = x_scale,
                M22 = y_scale,
                M33 = -((FAR_PLANE + NEAR_PLANE) / frustum_lenght),
                M34 = -1,
                M43 = -(2 * NEAR_PLANE * FAR_PLANE / frustum_lenght),
                M44 = 0
            };
        }
    }
}
