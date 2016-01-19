using System;
using System.Collections.Generic;

namespace RayTracer.Model.Geometries
{
    class Octree : Geometry
    {
        private const int TriangleCountInLeaf = 10;
        private Box boundingBox;
        private List<Octree> children;
        private List<Triangle> nodeTriangles;

        public Octree(List<Triangle> triangles)
        {
            double minX = triangles[0].A.X, maxX = triangles[0].A.X;
            double minY = triangles[0].A.Y, maxY = triangles[0].A.Y;
            double minZ = triangles[0].A.Z, maxZ = triangles[0].A.Z;
            foreach (Triangle triangle in triangles)
            {
                minX = Math.Min(minX, Math.Min(triangle.A.X, Math.Min(triangle.B.X, triangle.C.X)));
                maxX = Math.Max(maxX, Math.Max(triangle.A.X, Math.Max(triangle.B.X, triangle.C.X)));
                minY = Math.Min(minY, Math.Min(triangle.A.Y, Math.Min(triangle.B.Y, triangle.C.Y)));
                maxY = Math.Max(maxY, Math.Max(triangle.A.Y, Math.Max(triangle.B.Y, triangle.C.Y)));
                minZ = Math.Min(minZ, Math.Min(triangle.A.Z, Math.Min(triangle.B.Z, triangle.C.Z)));
                maxZ = Math.Max(maxZ, Math.Max(triangle.A.Z, Math.Max(triangle.B.Z, triangle.C.Z)));
            }
            Vector3 min = new Vector3(minX, minY, minZ);
            Vector3 max = new Vector3(maxX, maxY, maxZ);
            boundingBox = new Box(minX, maxX, minY, maxY, minZ, maxZ);
            if (triangles.Count > TriangleCountInLeaf)
            {
                children = new List<Octree>();
                SubDivide(triangles, min, max);
            }
            else
            {
                nodeTriangles = triangles;
            }
        }

        public override void Initialize()
        {
            if (nodeTriangles != null)
            {
                foreach (var triangle in nodeTriangles)
                {
                    triangle.Initialize();
                }
            }
            if (children != null)
            {
                foreach (var child in children)
                {
                    child.Initialize();
                }
            }
        }

        public override IntersectResult Intersect(Ray3 ray)
        {
            return Intersect(ray, 1e30); // fake infinity
        }

        public IntersectResult Intersect(Ray3 ray, double maxDistance)
        {
            if (!boundingBox.Intersect(ray, maxDistance))
                return IntersectResult.NoHit();
            double minDistance = maxDistance;
            IntersectResult minResult = IntersectResult.NoHit();
            if (nodeTriangles != null)
            {
                foreach (var triangle in nodeTriangles)
                {
                    IntersectResult result = triangle.Intersect(ray);
                    if (result.Geometry != null && result.Distance < minDistance)
                    {
                        minDistance = result.Distance;
                        minResult = result;
                    }
                }
            }
            if (children != null)
            {
                foreach (var child in children)
                {
                    IntersectResult result = child.Intersect(ray, minDistance);
                    if (result.Geometry != null && result.Distance < minDistance)
                    {
                        minDistance = result.Distance;
                        minResult = result;
                    }
                }
            }
            return minResult;
        }

        private void SubDivide(List<Triangle> triangles, Vector3 min, Vector3 max)
        {
            double avgX = (min.X + max.X) / 2;
            double avgY = (min.Y + max.Y) / 2;
            double avgZ = (min.Z + max.Z) / 2;
            List<Triangle>[] subnodes = new List<Triangle>[8];
            for (int i = 0; i < 8; ++i)
            {
                subnodes[i] = new List<Triangle>();
            }
            nodeTriangles = new List<Triangle>();
            foreach (Triangle triangle in triangles)
            {
                int subnodeIndex = AssignSubnodeIndex(triangle, avgX, avgY, avgZ);
                if (subnodeIndex == -1)
                {
                    nodeTriangles.Add(triangle);
                }
                else
                {
                    subnodes[subnodeIndex].Add(triangle);
                }
            }
            for (int i = 0; i < 8; ++i)
            {
                if (subnodes[i].Count == 0)
                    continue;
                children.Add(new Octree(subnodes[i]));
            }
        }

        private static int AssignSubnodeIndex(Triangle triangle, double avgX, double avgY, double avgZ)
        {
            int subNodeIndex = 0;
            if (triangle.A.X > avgX && triangle.B.X > avgX && triangle.C.X > avgX)
            {
                subNodeIndex += 1;
            }
            else if (triangle.A.X > avgX || triangle.B.X > avgX || triangle.C.X > avgX)
            {
                return -1;
            }
            if (triangle.A.Y > avgY && triangle.B.Y > avgY && triangle.C.Y > avgY)
            {
                subNodeIndex += 2;
            }
            else if (triangle.A.Y > avgY || triangle.B.Y > avgY || triangle.C.Y > avgY)
            {
                return -1;
            }
            if (triangle.A.Z > avgZ && triangle.B.Z > avgZ && triangle.C.Z > avgZ)
            {
                subNodeIndex += 4;
            }
            else if (triangle.A.Z > avgZ || triangle.B.Z > avgZ || triangle.C.Z > avgZ)
            {
                return -1;
            }
            return subNodeIndex;
        }
    }
}
