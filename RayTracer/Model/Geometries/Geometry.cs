using RayTracer.Model.Materials;

namespace RayTracer.Model.Geometries
{
    class Geometry
    {
        Material material;

        public Material Material
        {
            get { return material; }
        }

        public Geometry(Material _material = null)
        {
            material = _material;
        }
        public virtual void Initialize()
        {
        }
        public virtual IntersectResult Intersect(Ray3 ray)
        {
            return IntersectResult.NoHit();
        }
    }
}
