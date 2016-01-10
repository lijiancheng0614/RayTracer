using RayTracer.Model.Geometries;

namespace RayTracer.Model.Lights
{
    class Light
    {
        public virtual void Initialize()
        {
        }
        public virtual LightSample Sample(Geometry geometry, Vector3 position)
        {
            return LightSample.Zero;
        }
    }
}
