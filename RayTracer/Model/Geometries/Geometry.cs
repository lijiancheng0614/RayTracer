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

        public Geometry(Material material = null)
        {
            this.material = material;
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
