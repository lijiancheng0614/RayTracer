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
                    IntersectResult result = new IntersectResult();
                    result.Geometry = this;
                    result.Distance = -DdotV - Math.Sqrt(discr);
                    result.Position = ray.GetPoint(result.Distance);
                    result.Normal = result.Position.Subtract(center).Normalize();
                    return result;
                }
            }
            return IntersectResult.NoHit();
        }
    }
}
