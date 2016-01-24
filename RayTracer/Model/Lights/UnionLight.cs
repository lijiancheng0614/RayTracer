using System.Collections.Generic;
using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class UnionLight : Light
    {
        List<Light> lights;

        public UnionLight()
        {
            lights = new List<Light>();
        }
        public void Add(Light light)
        {
            lights.Add(light);
        }
        public void Clear()
        {
            lights.Clear();
        }
        public override void Initialize()
        {
            foreach (Light light in lights)
                light.Initialize();
        }
        public List<LightSample> GetLightSample(Geometry geometry, Vector3 position)
        {
            List<LightSample> list = new List<LightSample>();
            foreach (Light light in lights)
            {
                LightSample lightSample = light.Sample(geometry, position);
                if (lightSample != LightSample.Zero)
                {
                    list.Add(lightSample);
                }
            }
            return list;
        }
    }
}
