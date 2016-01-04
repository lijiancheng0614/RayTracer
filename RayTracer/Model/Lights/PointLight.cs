using System;
using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class PointLight : Light
    {
        Color intensity;
        Vector3 position;
        bool shadow;

        public PointLight(Color _intensity, Vector3 _position, bool _shadow = true)
        {
            intensity = _intensity;
            position = _position;
            shadow = _shadow;
        }
        public override LightSample Sample(Geometry scene, Vector3 position)
        {
            Vector3 delta = this.position.Subtract(position);
            double rr = delta.SqrLength();
            double r = Math.Sqrt(rr);
            Vector3 l = delta.Divide(r);
            if (shadow)
            {
                var shadowRay = new Ray3(position, l);
                var shadowResult = scene.Intersect(shadowRay);
                if (shadowResult.Geometry != null && shadowResult.Distance <= r)
                    return LightSample.Zero;
            }
            double attenuation = 1 / rr;
            return new LightSample(l, intensity.Multiply(attenuation));
        }
    }
}
