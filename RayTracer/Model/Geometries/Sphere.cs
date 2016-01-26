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
                    result.TextureCoordinates = new Vector2(
                        (Math.PI + Math.Atan2(position.Z - center.Z, position.X - center.X)) / (Math.PI * 2),
                        1.0 - ((Math.PI / 2) + Math.Asin((position.Y - center.Y) / radius)) / Math.PI);
                    return result;
                }
            }
            return IntersectResult.NoHit();
        }
    }
}
