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
        public override void Initialize()
        {
            for (int i = 0; i < lights.Count; ++i)
                lights[i].Initialize();
        }
        public void Clear()
        {
            lights.Clear();
        }
        public List<LightSample> GetLightSample(Geometry geometry, Vector3 position)
        {
            List<LightSample> list = new List<LightSample>();
            for (int i = 0; i < lights.Count; ++i)
            {
                LightSample lightSample = lights[i].Sample(geometry, position);
                if (lightSample != LightSample.Zero)
                {
                    list.Add(lightSample);
                }
            }
            return list;
        }
    }
}
