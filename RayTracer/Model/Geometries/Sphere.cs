using System;
using RayTracer.Model.Materials;

namespace RayTracer.Model.Geometries
{
    class Sphere : Geometry
    {
        Vector3 center;
        double radius;

        public Sphere(Vector3 center, double radius, Material material = null)
            : base(material)
        {
            this.center = center;
            this.radius = radius;
        }
        public override IntersectResult Intersect(Ray3 ray)
        {
            Vector3 v = ray.Origin - center;
            double a0 = v.SqrLength() - radius * radius;
            double DdotV = ray.Direction ^ v;
            if (DdotV <= 0)
            {
                double discr = DdotV * DdotV - a0;
                if (discr >= 0)
                {
                    double distance = -DdotV - Math.Sqrt(discr);
                    Vector3 position = ray.GetPoint(distance);
                    Vector3 normal = (position - center).Normalize();
                    IntersectResult result = new IntersectResult(this, distance, position, normal);
                    return result;
                }
            }
            return IntersectResult.NoHit();
        }
    }
}
