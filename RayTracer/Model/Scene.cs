using System.Collections.Generic;
using System.Drawing;
using RayTracer.Model.Geometries;
using RayTracer.Model.Lights;

namespace RayTracer.Model
{
    class Scene
    {
        UnionGeometry geometries;
        UnionLight lights;
        PerspectiveCamera camera;

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
        public Bitmap GetSystemBitmap(int width, int height, bool depthFlag = false, int rayTracingMaxReflect = 0)
        {
            double dx = 1.0 / width;
            double dy = 1.0 / height;
            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; ++y)
            {
                double sy = 1 - dy * y;
                for (int x = 0; x < width; ++x)
                {
                    double sx = dx * x;
                    Ray3 ray = new Ray3(camera.GenerateRay(sx, sy));
                    Color color = rayTracing(geometries, ray, lights, rayTracingMaxReflect, depthFlag);
                    bitmap.SetPixel(x, y, color.GetSystemColor());
                }
            }
            return bitmap;
        }
        Color rayTracing(Geometry geometries, Ray3 ray, UnionLight lights, int maxReflect, bool depthFlag = false)
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
                        Color reflectedColor = rayTracing(geometries, newRay, lights, maxReflect - 1, depthFlag);
                        color = color.Add(reflectedColor.Multiply(reflectiveness));
                    }
                }
            }
            return color;
        }
    }
}
