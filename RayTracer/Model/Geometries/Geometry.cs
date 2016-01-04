using RayTracer.Model.Materials;

namespace RayTracer.Model.Geometries
{
    class Geometry
    {
        Material material;

        public Material Material
        {
            get { return material; }
            set { material = value; }
        }
        public virtual void Initialize()
        {
        }
        public virtual IntersectResult Intersect(Ray3 ray) {
            return IntersectResult.NoHit();
        }
    }
}
