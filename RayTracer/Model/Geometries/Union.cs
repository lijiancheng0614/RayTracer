using System.Collections.Generic;

namespace RayTracer.Model.Geometries
{
    class Union : Geometry
    {
        List<Geometry> geometries;

        public Union()
        {
            geometries = new List<Geometry>();
        }
        public void Add(Geometry geometry)
        {
            geometries.Add(geometry);
        }
        public override void Initialize()
        {
            for (int i = 0; i < geometries.Count; ++i)
                geometries[i].Initialize();
        }
        public override IntersectResult Intersect(Ray3 ray)
        {
            double Infinity = 1e30;
            double minDistance = Infinity;
            IntersectResult minResult = IntersectResult.NoHit();
            for (int i = 0; i < geometries.Count; ++i)
            {
                IntersectResult result = geometries[i].Intersect(ray);
                if (result.Geometry != null && result.Distance < minDistance)
                {
                    minDistance = result.Distance;
                    minResult = result;
                }
            }
            return minResult;
        }
        public void Clear()
        {
            geometries.Clear();
        }
    }
}
