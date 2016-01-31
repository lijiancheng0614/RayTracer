using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using RayTracer.Model.Geometries;
using RayTracer.Model.Lights;

namespace RayTracer.Model
{
    class Scene
    {
        public enum AntiAlaising
        {
            None, SuperSampling, Jitter
        };
        Random random = new Random();
        UnionGeometry geometries;
        UnionLight lights;
        PerspectiveCamera camera;

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        EventHandler taskEndEventHandler;
        AntiAlaising antiAlaising;
        MultiThreadImage image;
        DateTime startDateTime;
        double renderTime;
        int threadCount;
        bool depthFlag;
        int rayTracingMaxReflect;
        int width;
        int height;
        double dx;
        double dy;

        public Scene(UnionGeometry geometries, UnionLight lights, PerspectiveCamera camera)
        {
            this.geometries = geometries;
            this.lights = lights;
            this.camera = camera;
        }
        public void Initialize()
        {
            geometries.Initialize();
            lights.Initialize();
            camera.Initialize();
        }
        public void GetImage(int width, int height, EventHandler taskEndEventHandler = null, int threadCount = 1,
            bool depthFlag = false, int rayTracingMaxReflect = 0, AntiAlaising antiAlaising = AntiAlaising.SuperSampling)
        {
            this.width = width;
            this.height = height;
            this.taskEndEventHandler = taskEndEventHandler;
            this.threadCount = threadCount;
            this.depthFlag = depthFlag;
            this.rayTracingMaxReflect = rayTracingMaxReflect;
            this.antiAlaising = antiAlaising;
            image = new MultiThreadImage(new Bitmap(width, height));
            dx = 1.0 / width;
            dy = 1.0 / height;
            StartTask();
        }
        public void CancelTask()
        {
            cancellationTokenSource.Cancel();
        }
        void StartTask()
        {
            startDateTime = DateTime.Now;
            TaskFactory taskFactory = new TaskFactory();
            Task[] tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; ++i)
            {
                int threadId = i; // Be careful!
                tasks[threadId] = taskFactory.StartNew(() => DoTask(threadId, cancellationTokenSource.Token));
            }
            taskFactory.ContinueWhenAll(tasks, EndTask, CancellationToken.None);
        }
        void EndTask(Task[] tasks)
        {
            renderTime = (DateTime.Now - startDateTime).TotalSeconds;
            taskEndEventHandler(this, new RenderEventArgs(image.Image, renderTime));
        }
        void DoTask(int threadId, CancellationToken cancellationToken)
        {
            switch (antiAlaising)
            {
                case AntiAlaising.None:
                    for (int y = 0; y < height; ++y)
                    {
                        double sy = 1 - dy * y;
                        for (int x = threadId; x < width; x += threadCount)
                        {
                            double sx = dx * x;
                            Ray3 ray = camera.GenerateRay(sx, sy);
                            Color color = rayTracing(ray, rayTracingMaxReflect);
                            image.SetPixel(x, y, color.GetSystemColor());
                        }
                    }
                    break;
                case AntiAlaising.SuperSampling:
                    for (int y = 0; y < height; ++y)
                    {
                        double sy = 1 - dy * (y - 0.5);
                        double ty = 1 - dy * (y + 0.5);
                        for (int x = threadId; x < width; x += threadCount)
                        {
                            double sx = dx * (x - 0.5);
                            double tx = dx * (x + 0.5);
                            Color color = Color.Black;
                            Ray3[] rays = new Ray3[]{
                                camera.GenerateRay(sx, sy),
                                camera.GenerateRay(sx, ty),
                                camera.GenerateRay(tx, sy),
                                camera.GenerateRay(tx, ty),
                            };
                            foreach (Ray3 ray in rays)
                                color = color + rayTracing(ray, rayTracingMaxReflect);
                            color = color * (1.0 / rays.Length);
                            image.SetPixel(x, y, color.GetSystemColor());
                        }
                    }
                    break;
                case AntiAlaising.Jitter:
                    for (int y = 0; y < height; ++y)
                    {
                        double sy = 1 - dy * (y - random.NextDouble() * 0.5);
                        double ty = 1 - dy * (y + random.NextDouble() * 0.5);
                        for (int x = threadId; x < width; x += threadCount)
                        {
                            double sx = dx * (x - random.NextDouble() * 0.5);
                            double tx = dx * (x + random.NextDouble() * 0.5);
                            Color color = Color.Black;
                            Ray3[] rays = new Ray3[]{
                                camera.GenerateRay(sx, sy),
                                camera.GenerateRay(sx, ty),
                                camera.GenerateRay(tx, sy),
                                camera.GenerateRay(tx, ty),
                            };
                            foreach (Ray3 ray in rays)
                                color = color + rayTracing(ray, rayTracingMaxReflect);
                            color = color * (1.0 / rays.Length);
                            image.SetPixel(x, y, color.GetSystemColor());
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        Color rayTracing(Ray3 ray, int maxReflect)
        {
            IntersectResult result = geometries.Intersect(ray);
            Color color = Color.Black;
            if (result.Geometry != null)
            {
                if (depthFlag)
                {
                    color = result.GetDepthColor();
                }
                else if (result.Geometry.Material != null)
                {
                    List<LightSample> lightSamples = lights.GetLightSample(geometries, result.Position);
                    double reflectiveness = result.Geometry.Material.Reflectiveness;
                    foreach (LightSample lightSample in lightSamples)
                    {
                        color = color + result.Geometry.Material.Sample(ray, lightSample, result.Normal, result.Position, result.TextureCoordinates);
                    }
                    color = color * (1 - reflectiveness);

                    if (reflectiveness > 0 && maxReflect > 0)
                    {
                        Vector3 r = result.Normal * (result.Normal ^ ray.Direction * (-2)) + ray.Direction;
                        Ray3 newRay = new Ray3(result.Position, r);
                        Color reflectedColor = rayTracing(newRay, maxReflect - 1);
                        color = color + reflectedColor * reflectiveness;
                    }
                }
            }
            return color;
        }
    }
}
