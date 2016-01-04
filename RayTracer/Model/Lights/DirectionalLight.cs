using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class DirectionalLight : Light
    {
        Vector3 l;
        Color irradiance;
        Vector3 direction;
        bool shadow;

        public DirectionalLight(Color _irradiance, Vector3 _direction, bool _shadow = true)
        {
            irradiance = _irradiance;
            direction = _direction;
            shadow = _shadow;
        }
        public override void Initialize()
        {
            l = direction.Normalize().Negate();
        }
        public override LightSample Sample(Geometry scene, Vector3 position)
        {
            if (shadow)
            {
                Ray3 shadowRay = new Ray3(position, l);
                IntersectResult shadowResult = scene.Intersect(shadowRay);
                if (shadowResult.Geometry != null)
                {
                    return LightSample.Zero;
                }
            }
            return new LightSample(l, irradiance);
        }
    }
}
