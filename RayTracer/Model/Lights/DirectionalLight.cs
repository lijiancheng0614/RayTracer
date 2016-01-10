using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class DirectionalLight : Light
    {
        Vector3 vector;
        Color irradiance;
        bool shadow;

        public DirectionalLight(Color _irradiance, Vector3 _direction, bool _shadow = true)
        {
            irradiance = _irradiance;
            vector = _direction.Normalize().Negate();
            shadow = _shadow;
        }
        public override LightSample Sample(Geometry geometry, Vector3 position)
        {
            if (shadow)
            {
                Ray3 shadowRay = new Ray3(position, vector);
                IntersectResult shadowResult = geometry.Intersect(shadowRay);
                if (shadowResult.Geometry != null)
                {
                    return LightSample.Zero;
                }
            }
            return new LightSample(vector, irradiance);
        }
    }
}
