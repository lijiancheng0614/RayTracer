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
        public Color GetColor(Geometry scene, IntersectResult result)
        {
            Color color = Color.Black;
            for (int i = 0; i < lights.Count; ++i)
            {
                LightSample lightSample = lights[i].Sample(scene, result.Position);

                if (lightSample != LightSample.Zero)
                {
                    var NdotL = result.Normal.Dot(lightSample.L);
                    if (NdotL >= 0)
                        color = color.Add(lightSample.Irradiance.Multiply(NdotL));
                }
            }
            return color;
        }
    }
}
