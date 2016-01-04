using System;
using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class SpotLight : Light
    {
        Color intensity;
        Vector3 position;
        double falloff;
        bool shadow;
        Vector3 S;
        double cosTheta;
        double cosPhi;
        double baseMultiplier;

        public SpotLight(Color _intensity, Vector3 _position, Vector3 _direction, double _theta, double _phi, double _falloff, bool _shadow = true)
        {
            intensity = _intensity;
            position = _position;
            S = _direction.Normalize().Negate();
            cosTheta = Math.Cos(_theta * Math.PI / 180 / 2);
            cosPhi = Math.Cos(_phi * Math.PI / 180 / 2);
            falloff = _falloff;
            shadow = _shadow;
        }
        public override void Initialize()
        {
            
            baseMultiplier = 1 / (cosTheta - cosPhi);
        }
        public override LightSample Sample(Geometry scene, Vector3 position)
        {
            Vector3 delta = this.position.Subtract(position);
            double rr = delta.SqrLength();
            double r = Math.Sqrt(rr);
            Vector3 l = delta.Divide(r);
            double spot;
            double SdotL = S.Dot(l);
            if (SdotL >= cosTheta)
                spot = 1;
            else if (SdotL <= cosPhi)
                spot = 0;
            else
                spot = Math.Pow((SdotL - cosPhi) * baseMultiplier, falloff);
            if (shadow)
            {
                var shadowRay = new Ray3(position, l);
                var shadowResult = scene.Intersect(shadowRay);
                if (shadowResult.Geometry != null && shadowResult.Distance <= r)
                    return LightSample.Zero;
            }
            double attenuation = 1 / rr;
            return new LightSample(l, intensity.Multiply(attenuation * spot));
        }
    }
}
