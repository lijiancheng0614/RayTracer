﻿using System;
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
        UnionGeometry geometries;
        UnionLight lights;
        PerspectiveCamera camera;
        
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        EventHandler taskEndEventHandler;
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

        public Scene(UnionGeometry _geometries, UnionLight _lights, PerspectiveCamera _camera)
        {
            geometries = _geometries;
            lights = _lights;
            camera = _camera;
        }
        public void Initialize()
        {
            geometries.Initialize();
            lights.Initialize();
            camera.Initialize();
        }
        public void GetImage(int width, int height, EventHandler taskEndEventHandler = null, int threadCount = 1, bool depthFlag = false, int rayTracingMaxReflect = 0)
        {
            this.width = width;
            this.height = height;
            this.taskEndEventHandler = taskEndEventHandler;
            this.threadCount = threadCount;
            this.depthFlag = depthFlag;
            this.rayTracingMaxReflect = rayTracingMaxReflect;
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
            for (int y = 0; y < height; ++y)
            {
                double sy = 1 - dy * y;
                for (int x = threadId; x < width; x += threadCount)
                {
                    double sx = dx * x;
                    Ray3 ray = new Ray3(camera.GenerateRay(sx, sy));
                    Color color = rayTracing(ray, rayTracingMaxReflect);
                    image.SetPixel(x, y, color.GetSystemColor());
                }
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
                        color = color.Add(result.Geometry.Material.Sample(ray, result.Normal, result.Position, lightSample));
                    }
                    color = color.Multiply(1 - reflectiveness);

                    if (reflectiveness > 0 && maxReflect > 0)
                    {
                        Vector3 r = result.Normal.Multiply(-2 * result.Normal.Dot(ray.Direction)).Add(ray.Direction);
                        Ray3 newRay = new Ray3(result.Position, r);
                        Color reflectedColor = rayTracing(newRay, maxReflect - 1);
                        color = color.Add(reflectedColor.Multiply(reflectiveness));
                    }
                }
            }
            return color;
        }
    }
}
