using RayTracer.Model.Materials;

namespace RayTracer.Model.Geometries
{
    class Plane : Geometry
    {
        Vector3 normal;
        double d;
        Vector3 position;

        public Plane(Vector3 normal, double d, Material material = null)
            : base(material)
        {
            this.normal = normal;
            this.d = d;
        }
        public override void Initialize()
        {
            position = normal * d;
        }
        public override IntersectResult Intersect(Ray3 ray)
        {
            double a = ray.Direction ^ normal;
            if (a >= 0)
                return IntersectResult.NoHit();
            double b = normal ^ (ray.Origin - position);
            double distance = -b / a;
            return new IntersectResult(this, distance, ray.GetPoint(distance), normal);
        }
    }
}
