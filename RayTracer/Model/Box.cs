using System;

namespace RayTracer.Model.Geometries
{
    class Box
    {
        private readonly double minX;
        private readonly double maxX;
        private readonly double minY;
        private readonly double maxY;
        private readonly double minZ;
        private readonly double maxZ;

        public Box(double _minX, double _maxX, double _minY, double _maxY, double _minZ, double _maxZ)
        {
            minX = _minX;
            maxX = _maxX;
            minY = _minY;
            maxY = _maxY;
            minZ = _minZ;
            maxZ = _maxZ;
        }

        public Vector3 Min
        {
            get
            {
                return new Vector3(minX, minY, minZ);
            }
        }

        public Vector3 Max
        {
            get
            {
                return new Vector3(maxX, maxY, maxZ);
            }
        }

        public bool Intersect(Ray3 ray, double maxDistance)
        {
            double tMin;
            double tMax;
            double rayDirectionXInverse = 1 / ray.Direction.X;
            if (rayDirectionXInverse >= 0)
            {
                tMin = (minX - ray.Origin.X) * rayDirectionXInverse;
                tMax = (maxX - ray.Origin.X) * rayDirectionXInverse;
            }
            else
            {
                tMax = (minX - ray.Origin.X) * rayDirectionXInverse;
                tMin = (maxX - ray.Origin.X) * rayDirectionXInverse;
            }

            double tyMin;
            double tyMax;
            double rayDirectionYInverse = 1 / ray.Direction.Y;
            if (rayDirectionYInverse >= 0)
            {
                tyMin = (minY - ray.Origin.Y) * rayDirectionYInverse;
                tyMax = (maxY - ray.Origin.Y) * rayDirectionYInverse;
            }
            else
            {
                tyMax = (minY - ray.Origin.Y) * rayDirectionYInverse;
                tyMin = (maxY - ray.Origin.Y) * rayDirectionYInverse;
            }

            if (tMin > tyMax || tyMin > tMax)
            {
                return false;
            }

            tMin = Math.Max(tMin, tyMin);
            tMax = Math.Min(tMax, tyMax);

            double tzMin;
            double tzMax;
            double rayDirectionZInverse = 1 / ray.Direction.Z;
            if (rayDirectionZInverse >= 0)
            {
                tzMin = (minZ - ray.Origin.Z) * rayDirectionZInverse;
                tzMax = (maxZ - ray.Origin.Z) * rayDirectionZInverse;
            }
            else
            {
                tzMax = (minZ - ray.Origin.Z) * rayDirectionZInverse;
                tzMin = (maxZ - ray.Origin.Z) * rayDirectionZInverse;
            }

            if (tMin > tzMax || tzMin > tMax)
            {
                return false;
            }

            tMin = Math.Max(tMin, tzMin);
            tMax = Math.Min(tMax, tzMax);

            return tMin <= tMax && tMin <= maxDistance && tMax >= 0;
        }
    }
}
