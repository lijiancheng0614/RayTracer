using System;
using RayTracer.Model.Materials;

namespace RayTracer.Model.Geometries
{
    class Sphere : Geometry
    {
        Vector3 center;
        double radius;

        public Sphere(Vector3 _center, double _radius, Material _material = null)
            : base(_material)
        {
            center = _center;
            radius = _radius;
        }
        public double SqrRadius()
        {
            return radius * radius;
        }
        public override IntersectResult Intersect(Ray3 ray)
        {
            Vector3 v = ray.Origin.Subtract(center);
            double a0 = v.SqrLength() - SqrRadius();
            double DdotV = ray.Direction.Dot(v);
            if (DdotV <= 0)
            {
                double discr = DdotV * DdotV - a0;
                if (discr >= 0)
                {
                    double distance = -DdotV - Math.Sqrt(discr);
                    Vector3 position = ray.GetPoint(distance);
                    Vector3 normal = position.Subtract(center).Normalize();
                    IntersectResult result = new IntersectResult(this, distance, position, normal);
                    return result;
                }
            }
            return IntersectResult.NoHit();
        }
    }
}
