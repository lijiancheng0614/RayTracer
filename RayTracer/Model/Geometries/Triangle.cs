using System;
using System.Collections.Generic;
using RayTracer.Model.Materials;

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

        List<Vector3> vertices;
        List<Vector3> normals;
        List<Vector2> textures;

        Vector3 edgeAB;
        Vector3 edgeAC;
        Vector3 normal;

        public Triangle(List<Vector3> vertices, List<Vector3> normals, List<Vector2> textures, Material material = null)
            : base(material)
        {
            this.vertices = vertices;
            this.normals = normals;
            this.textures = textures;
        }

        public override void Initialize()
        {
            edgeAB = vertices[1] - vertices[0];
            edgeAC = vertices[2] - vertices[0];
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
            Vector3 w0 = ray.Origin - vertices[0];
            double x = -normal ^ w0;
            double y = normal ^ ray.Direction;
            double distance = x / y;
            if (y == 0 || distance < 0)
                return IntersectResult.NoHit();
            Vector3 position = ray.GetPoint(distance);
            double uu = edgeAB ^ edgeAB;
            double uv = edgeAB ^ edgeAC;
            double vv = edgeAC ^ edgeAC;
            Vector3 w = position - vertices[0];
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
            if (distance > 0 && normals != null)
            {
                Vector3 n1Interpolated = normals[0] * alpha;
                Vector3 n2Interpolated = normals[1] * beta;
                Vector3 n3Interpolated = normals[2] * gamma;
                newNormal = n1Interpolated + n2Interpolated + n3Interpolated;
            }
            IntersectResult result = new IntersectResult(this, distance, position, newNormal);
            if (textures != null)
            {
                Vector2 t1Interpolated = textures[0] * alpha;
                Vector2 t2Interpolated = textures[1] * beta;
                Vector2 t3Interpolated = textures[2] * gamma;
                result.TextureCoordinates = t1Interpolated + t2Interpolated + t3Interpolated;
            }
            return result;
        }

        private Box GetBoundingBox()
        {
            double minX = vertices[0].X, maxX = vertices[0].X;
            double minY = vertices[0].Y, maxY = vertices[0].Y;
            double minZ = vertices[0].Z, maxZ = vertices[0].Z;
            for (int i = 1; i < 3; ++i)
            {
                minX = Math.Min(minX, vertices[i].X);
                maxX = Math.Max(maxX, vertices[i].X);
                minY = Math.Min(minY, vertices[i].Y);
                maxY = Math.Max(maxY, vertices[i].Y);
                minZ = Math.Min(minZ, vertices[i].Z);
                maxZ = Math.Max(maxZ, vertices[i].Z);
            }
            return new Box(minX, maxX, minY, maxY, minZ, maxZ);
        }
    }
}
