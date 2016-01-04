
namespace RayTracer.Model.Geometries
{
    class Plane : Geometry
    {
        Vector3 normal;
        double d;
        Vector3 position;

        public Plane(Vector3 _normal, double _d)
        {
            normal = _normal;
            d = _d;
            position = Vector3.Zero();
        }
        public Plane(Plane p)
        {
            normal = p.normal;
            d = p.d;
            position = Vector3.Zero();
        }
        public override void Initialize()
        {
            position = normal.Multiply(d);
        }
        public override IntersectResult Intersect(Ray3 ray)
        {
            double a = ray.Direction.Dot(normal);
            if (a >= 0)
                return IntersectResult.NoHit();
            double b = normal.Dot(ray.Origin.Subtract(position));
            IntersectResult result = new IntersectResult();
            result.Geometry = this;
            result.Distance = -b / a;
            result.Position = ray.GetPoint(result.Distance);
            result.Normal = normal;
            return result;
        }
    }
}
