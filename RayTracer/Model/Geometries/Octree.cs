using System;
using System.Collections.Generic;

namespace RayTracer.Model.Geometries
{
    class Octree : Geometry
    {
        private Box boundingBox;

        internal Box BoundingBox
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

        private readonly List<Triangle> triangles;
        private List<Triangle> nodeTriangles;
        private List<Octree> children;

        public Octree(List<Triangle> _triangles)
        {
            triangles = _triangles;
        }

        public override void Initialize()
        {
            if (triangles.Count > Constant.TriangleCountInLeaf)
            {
                children = new List<Octree>();
                Vector3 min = BoundingBox.Min;
                Vector3 max = BoundingBox.Max;
                double avgX = (min.X + max.X) / 2;
                double avgY = (min.Y + max.Y) / 2;
                double avgZ = (min.Z + max.Z) / 2;
                SubDivide(triangles, avgX, avgY, avgZ);
                foreach (var child in children)
                {
                    child.Initialize();
                }
            }
            else
            {
                nodeTriangles = triangles;
            }
            if (nodeTriangles != null)
            {
                foreach (var triangle in nodeTriangles)
                {
                    triangle.Initialize();
                }
            }
        }

        public override IntersectResult Intersect(Ray3 ray)
        {
            return Intersect(ray, Constant.Infinity);
        }

        public IntersectResult Intersect(Ray3 ray, double maxDistance)
        {
            if (!BoundingBox.Intersect(ray, maxDistance))
                return IntersectResult.NoHit();
            double minDistance = maxDistance;
            IntersectResult minResult = IntersectResult.NoHit();
            if (nodeTriangles != null)
            {
                foreach (var triangle in nodeTriangles)
                {
                    IntersectResult result = triangle.Intersect(ray, minDistance);
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

        private Box GetBoundingBox()
        {
            Vector3 min = triangles[0].BoundingBox.Min;
            Vector3 max = triangles[0].BoundingBox.Max;
            double minX = min.X, maxX = max.X;
            double minY = min.Y, maxY = max.Y;
            double minZ = min.Z, maxZ = max.Z;
            foreach (Triangle triangle in triangles)
            {
                min = triangle.BoundingBox.Min;
                max = triangle.BoundingBox.Max;
                minX = Math.Min(minX, min.X);
                maxX = Math.Max(maxX, max.X);
                minY = Math.Min(minY, min.Y);
                maxY = Math.Max(maxY, max.Y);
                minZ = Math.Min(minZ, min.Z);
                maxZ = Math.Max(maxZ, max.Z);
            }
            return new Box(minX, maxX, minY, maxY, minZ, maxZ);
        }

        private void SubDivide(List<Triangle> triangles, double avgX, double avgY, double avgZ)
        {
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
            Vector3 min = triangle.BoundingBox.Min;
            Vector3 max = triangle.BoundingBox.Max;
            if (min.X > avgX)
            {
                subNodeIndex += 1;
            }
            else if (max.X > avgX)
            {
                return -1;
            }
            if (min.Y > avgY)
            {
                subNodeIndex += 2;
            }
            else if (max.Y > avgY)
            {
                return -1;
            }
            if (min.Z > avgZ)
            {
                subNodeIndex += 4;
            }
            else if (max.Z > avgZ)
            {
                return -1;
            }
            return subNodeIndex;
        }
    }
}
