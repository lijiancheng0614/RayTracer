using RayTracer.Model.Materials;

namespace RayTracer.Model.Geometries
{
    abstract class Geometry
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
        
        public abstract IntersectResult Intersect(Ray3 ray);
    }
}
