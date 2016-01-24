using System;
using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class PointLight : Light
    {
        Color intensity;
        Vector3 position;
        bool shadow;

        public PointLight(Color intensity, Vector3 position, bool shadow = true)
        {
            this.intensity = intensity;
            this.position = position;
            this.shadow = shadow;
        }
        public override LightSample Sample(Geometry geometry, Vector3 position)
        {
            Vector3 delta = this.position - position;
            double rr = delta.SqrLength();
            double r = Math.Sqrt(rr);
            Vector3 l = delta / r;
            if (shadow)
            {
                var shadowRay = new Ray3(position, l);
                var shadowResult = geometry.Intersect(shadowRay);
                if (shadowResult.Geometry != null && shadowResult.Distance <= r)
                    return LightSample.Zero;
            }
            double attenuation = 1 / rr;
            return new LightSample(l, intensity * attenuation);
        }
    }
}
