using RayTracer.Model.Materials;
using System;

namespace RayTracer.Model.Geometries
{
    class Triangle : Geometry
    {
        Box boundingBox;

        public Box BoundingBox
        {
            get
            {
                if (boundingBox == null)
                {
                    boundingBox = GetBoundingBox();
                }
                return boundingBox;
            }
        }

        Vector3 a;
        Vector3 b;
        Vector3 c;
        Vector3 n1;
        Vector3 n2;
        Vector3 n3;

        Vector3 edgeAB;
        Vector3 edgeAC;
        Vector3 normal;
        bool hasVertexNormals = false;

        public Triangle(Vector3 a, Vector3 b, Vector3 c, Material material = null)
            : base(material)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            hasVertexNormals = false;
        }

        public Triangle(Vector3 a, Vector3 n1, Vector3 b, Vector3 n2, Vector3 c, Vector3 n3, Material material = null)
            : base(material)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.n1 = n1;
            this.n2 = n2;
            this.n3 = n3;
            hasVertexNormals = true;
        }

        public override void Initialize()
        {
            edgeAB = b - a;
            edgeAC = c - a;
            normal = (edgeAB * edgeAC).Normalize();
        }

        public override IntersectResult Intersect(Ray3 ray)
        {
            return Intersect(ray, Constant.Infinity);
        }

        public IntersectResult Intersect(Ray3 ray, double maxDistance)
        {
            if (!BoundingBox.Intersect(ray, maxDistance))
                return IntersectResult.NoHit();
            if (normal.SqrLength() == 0)
                return IntersectResult.NoHit();
            Vector3 w0 = ray.Origin - a;
            double x = -normal ^ w0;
            double y = normal ^ ray.Direction;
            double distance = x / y;
            if (y == 0 || distance < 0)
                return IntersectResult.NoHit();
            Vector3 position = ray.GetPoint(distance);
            double uu = edgeAB ^ edgeAB;
            double uv = edgeAB ^ edgeAC;
            double vv = edgeAC ^ edgeAC;
            Vector3 w = position - a;
            double wu = w ^ (edgeAB);
            double wv = w ^ (edgeAC);
            double D = uv * uv - uu * vv;
            double beta = (uv * wv - vv * wu) / D;
            if (beta < 0 || beta > 1)
                return IntersectResult.NoHit();
            double gamma = (uv * wu - uu * wv) / D;
            if (gamma < 0 || beta + gamma > 1)
                return IntersectResult.NoHit();
            double alpha = 1 - beta - gamma;
            Vector3 newNormal = normal;
            if (distance > 0 && hasVertexNormals)
            {
                Vector3 n1Interpolated = n1 * alpha;
                Vector3 n2Interpolated = n2 * beta;
                Vector3 n3Interpolated = n3 * gamma;
                newNormal = n1Interpolated + n2Interpolated + n3Interpolated;
            }
            return new IntersectResult(this, distance, position, newNormal);
        }

        private Box GetBoundingBox()
        {
            double minX = Math.Min(a.X, Math.Min(b.X, c.X));
            double maxX = Math.Max(a.X, Math.Max(b.X, c.X));
            double minY = Math.Min(a.Y, Math.Min(b.Y, c.Y));
            double maxY = Math.Max(a.Y, Math.Max(b.Y, c.Y));
            double minZ = Math.Min(a.Z, Math.Min(b.Z, c.Z));
            double maxZ = Math.Max(a.Z, Math.Max(b.Z, c.Z));
            return new Box(minX, maxX, minY, maxY, minZ, maxZ);
        }
    }
}
