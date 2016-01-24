using System;
using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class SpotLight : Light
    {
        Color intensity;
        Vector3 position;
        Vector3 vector;
        double cosTheta;
        double cosPhi;
        double falloff;
        bool shadow;
        double baseMultiplier;

        public SpotLight(Color intensity, Vector3 position, Vector3 direction, double theta, double phi, double falloff, bool shadow = true)
        {
            this.intensity = intensity;
            this.position = position;
            vector = -direction.Normalize();
            cosTheta = Math.Cos(theta * Math.PI / 180 / 2);
            cosPhi = Math.Cos(phi * Math.PI / 180 / 2);
            this.falloff = falloff;
            this.shadow = shadow;
        }
        public override void Initialize()
        {
            baseMultiplier = 1 / (cosTheta - cosPhi);
        }
        public override LightSample Sample(Geometry geometry, Vector3 position)
        {
            Vector3 delta = this.position - position;
            double rr = delta.SqrLength();
            double r = Math.Sqrt(rr);
            Vector3 l = delta / r;
            double spot;
            double SdotL = vector ^ l;
            if (SdotL >= cosTheta)
                spot = 1;
            else if (SdotL <= cosPhi)
                spot = 0;
            else
                spot = Math.Pow((SdotL - cosPhi) * baseMultiplier, falloff);
            if (shadow)
            {
                var shadowRay = new Ray3(position, l);
                var shadowResult = geometry.Intersect(shadowRay);
                if (shadowResult.Geometry != null && shadowResult.Distance <= r)
                    return LightSample.Zero;
            }
            double attenuation = 1 / rr;
            return new LightSample(l, intensity * (attenuation * spot));
        }
    }
}
