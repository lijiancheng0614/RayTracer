using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class DirectionalLight : Light
    {
        Vector3 vector;
        Color irradiance;
        bool shadow;

        public DirectionalLight(Color irradiance, Vector3 direction, bool shadow = true)
        {
            this.irradiance = irradiance;
            vector = -direction.Normalize();
            this.shadow = shadow;
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
